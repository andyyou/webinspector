using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Nevron.Chart;
using Nevron.Chart.WinForm;
using Nevron.GraphicsCore;
using WRPlugIn;

namespace PxP
{
    public partial class MapWindow : UserControl
    {
        #region Local Variables

        private NCartesianChart nChartMap;
        private FailList fl = null;
        bool IsClickGradeSetup = false;

        
        #endregion

        #region Constructor

        public MapWindow()
        {
            InitializeComponent();
            SystemVariable.LoadConfig();
            InitNChart(ref nChart, out nChartMap);
            
            SetFilterRadioButtons();
            btnPrevPiece.Enabled = false;
            btnNextPiece.Enabled = false;

            // Get grade config list
            // 組態檔繫結
            bsGradConfigList.DataSource = GetGradeConfList();
            cboxGradeConfigFile.DataSource = bsGradConfigList.DataSource;
            SystemVariable.LoadConfig();
            cboxGradeConfigFile.SelectedItem = SystemVariable.GradeConfigFileName.Substring(0, SystemVariable.GradeConfigFileName.LastIndexOf("."));

        }

        ~MapWindow()
        {
            try
            {
                string FolderPath = Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\..\\Parameter Files\\CPxP\\conf\\";
                string FullSystemPath = FolderPath + SystemVariable.ConfigFileName;
                XDocument XDoc = XDocument.Load(FullSystemPath);
                XElement xShowFlag = XDoc.Element("Config").Element("MapVariable").Element("ShowFlag");
                // 紀錄顯示項目 0:All, 1:Pass, 2:Fail
                if (rbAll.Checked)
                    xShowFlag.Value = "0";
                else if (rbPass.Checked)
                    xShowFlag.Value = "1";
                else if (rbFail.Checked)
                    xShowFlag.Value = "2";
                else
                    xShowFlag.Value = "0";
                XDoc.Save(FullSystemPath);
            }
            catch (Exception ex)
            {
                DebugTool.WriteLog(1, 2, "MapWindow.cs", "~MapWindow()", ex.Message);
                MessageBox.Show("Map Setup Destructor Error");
            }
        }

        #endregion

        #region Refactoring

        public void InitNChart(ref NChartControl ncc, out NCartesianChart chart)
        {
            // 2D line chart
            ncc.Settings.RenderDevice = RenderDevice.GDI;

            // Add tools to chart controller
            ncc.Controller.Tools.Add(new NSelectorTool());
            ncc.Controller.Tools.Add(new NDataZoomTool());
            //ncc.Controller.Tools.Add(new NDataPanTool());
            ncc.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(OnChartMouseDoubleClick);
            ncc.MouseMove += new System.Windows.Forms.MouseEventHandler(OnChartMouseMove);

            chart = (NCartesianChart)ncc.Charts[0];

            // Set range selections property
            NRangeSelection rangeSelection = new NRangeSelection();
            
            // Reset Axis when zoom out
            rangeSelection.ZoomOutResetsAxis = true;
            chart.RangeSelections.Add(rangeSelection);

            // Set chart axis property
            chart.Axis(StandardAxis.Depth).Visible = false;
            SetMapProperty();            
        }

        // 取得 Folder 底下所有 XML 清單
        List<string> GetGradeConfList()
        {
            List<string> ConfList = new List<string>();
            string ConfPath = Path.GetDirectoryName(
               Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "/../Parameter Files/CPxP/grade/";
            DirectoryInfo di = new DirectoryInfo(ConfPath);
            FileInfo[] rgFiles = di.GetFiles("*.xml");

            foreach (FileInfo fi in rgFiles)
            {
                ConfList.Add(fi.Name.ToString().Substring(0, fi.Name.ToString().LastIndexOf(".")));
            }
            return ConfList;
        }

        // 計算 piece 的分數
        public double CountPieceScore(List<FlawInfoAddPriority> list)
        {
            double tmp = 0;
            foreach (FlawInfoAddPriority i in list)
            {
                tmp += i.PointScore;
            }
            return tmp;
        }

        // 判斷 piece 是 Pass or Fail
        public bool GetPassOfPiece(List<FlawInfoAddPriority> list)
        {
            double score = CountPieceScore(list);

            if (score >= GradeVariable.PassOrFileScore)
                return false;
            else
                return true;
        }

        #endregion

        #region Method

        // Draw Flaws on Map
        public void DrawPieceFlaw(int pieceNum, bool drawFlag)
        {
            List<FlawInfoAddPriority> flawPiece = MapWindowVariable.FlawPieces[pieceNum];

            nChartMap.Series.Clear();
            if (GradeVariable.RoiMode == 1)
            {
                SubPieceMarkup(pieceNum);
            }

            if (flawPiece.Count > 0)
            {
                bool skipFlag = false;
                foreach (FlawInfoAddPriority f in flawPiece)
                {
                    skipFlag = false;
                    NPointSeries point = (NPointSeries)nChartMap.Series.Add(SeriesType.Point);
                    point.Name = f.FlawID.ToString();
                    point.Labels.Add(f.FlawID);
                    point.Size = new NLength(3, NRelativeUnit.ParentPercentage);
                    point.BorderStyle.Width = new NLength(0, NGraphicsUnit.Millimeter);
                    NDataLabelStyle dataLabel = new NDataLabelStyle();
                    dataLabel.Format = f.FlawID.ToString();
                    dataLabel.TextStyle.StringFormatStyle.HorzAlign = Nevron.HorzAlign.Center;
                    point.DataLabelStyle = dataLabel;
                    point.DataLabelStyle.Visible = false;

                    foreach (DataGridViewRow row in gvFlawClass.Rows)
                    {
                        DataGridViewCheckBoxCell col = (DataGridViewCheckBoxCell)row.Cells[2];
                        if (f.FlawType == Convert.ToInt32(row.Cells["FlawType"].Value))
                        {
                            if (!Convert.ToBoolean(gvFlawLegend.Rows[row.Index].Cells["Display"].EditedFormattedValue))
                            {
                                skipFlag = true;
                                break;
                            }

                            point.FillStyle = new NColorFillStyle(System.Drawing.ColorTranslator.FromHtml(row.Cells["Color"].Value.ToString()));
                            switch (row.Cells["Shape"].Value.ToString())
                            {
                                case "▲":
                                    point.PointShape = PointShape.DiagonalCross;
                                    break;
                                case "▼":
                                    point.PointShape = PointShape.InvertedCone;
                                    break;
                                case "■":
                                    point.PointShape = PointShape.Bar;
                                    break;
                                case "●":
                                    point.PointShape = PointShape.Sphere;
                                    break;
                                case "+":
                                    point.PointShape = PointShape.Cross;
                                    break;
                                case "╳":
                                    point.PointShape = PointShape.DiagonalCross;
                                    break;
                                case "★":
                                    point.PointShape = PointShape.Star;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    if (skipFlag) continue;

                    point.Tag = f.FlawType;
                    point.UseXValues = true;
                    // Check is axes need to invert
                    if (MapWindowVariable.CDInver == 1)
                        f.CD = -f.CD;
                    if (MapWindowVariable.MDInver == 1)
                        f.RMD = -f.RMD;
                    // When BottomAxe equals zero, bottom axis is CD otherwise is RMD
                    if (MapWindowVariable.BottomAxe == 0)
                        point.AddDataPoint(new NDataPoint(f.CD, f.RMD));
                    else
                        point.AddDataPoint(new NDataPoint(f.RMD, f.CD));
                }
            }

            if (drawFlag)
            {
                //UNDONE : CurrentPiece 設定改至 OnCut -> ProcessDoffResult 處理
                //MapWindowVariable.CurrentPiece = MapWindowVariable.FlawPieces.Count;
                //lbPageCurrent.Text = MapWindowVariable.FlawPieces.Count.ToString();
                lbPageCurrent.Text = MapWindowVariable.CurrentPiece.ToString();
                lbPageTotal.Text = lbPageCurrent.Text;

                if (MapWindowVariable.FlawPieces.Count > 1)
                {
                    btnPrevPiece.Enabled = true;
                }
                btnNextPiece.Enabled = false;
            }

            Color red = Color.FromArgb(255, 150, 150);
            NGradientFillStyle WallFail = new NGradientFillStyle(red, red);
            NGradientFillStyle WallPass = new NGradientFillStyle(Color.White, Color.White);

            if (MapWindowVariable.PieceResult[MapWindowVariable.CurrentPiece - 1])
                nChartMap.Wall(ChartWallType.Back).FillStyle = WallPass;
            else
                nChartMap.Wall(ChartWallType.Back).FillStyle = WallFail;

            nChart.Refresh();
            SetMapInfoLabel();
        }

        public void ClearMap()
        {
            nChartMap.Series.Clear();
            nChart.Refresh();
        }

        public void InitGvFlawClass()
        {

            bsFlawType.DataSource = PxPVariable.FlawTypeName;
            gvFlawClass.DataSource = bsFlawType;
            gvFlawClass.AllowUserToAddRows = false;
            
            foreach (DoffGridColumns column in MapWindowVariable.DoffTypeGridSetup)
            {
                if (column.ColumnName == "Display")
                    gvFlawClass.Columns[column.ColumnName].ReadOnly = false;
                else
                    gvFlawClass.Columns[column.ColumnName].ReadOnly = true;

                gvFlawClass.Columns[column.ColumnName].SortMode = DataGridViewColumnSortMode.Automatic;
                gvFlawClass.Columns[column.ColumnName].HeaderText = column.HeaderText;
                gvFlawClass.Columns[column.ColumnName].DisplayIndex = column.Index;
                gvFlawClass.Columns[column.ColumnName].Width = column.Width;
                gvFlawClass.Columns[column.ColumnName].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            // Hide columns
            gvFlawClass.Columns["Display"].Visible = false;
            gvFlawClass.Columns["Count"].Visible = false;
            gvFlawClass.Columns["Color"].Visible = false;
            gvFlawClass.Columns["Shape"].Visible = false;
            gvFlawClass.Columns["OfflineDoffNum"].Visible = false;
            gvFlawClass.Columns["OfflineJobNum"].Visible = false;

        }
        public void InitGvFlawLegend()
        {
            gvFlawLegend.DataSource = gvFlawClass.DataSource;
            gvFlawLegend.AllowUserToAddRows = false;

            foreach (DoffGridColumns column in MapWindowVariable.DoffTypeGridSetup)
            {
                if (column.ColumnName == "Display")
                    gvFlawLegend.Columns[column.ColumnName].ReadOnly = false;
                else
                    gvFlawLegend.Columns[column.ColumnName].ReadOnly = true;

                gvFlawLegend.Columns[column.ColumnName].SortMode = DataGridViewColumnSortMode.Automatic;
                gvFlawLegend.Columns[column.ColumnName].HeaderText = column.HeaderText;
                gvFlawLegend.Columns[column.ColumnName].DisplayIndex = column.Index;
                gvFlawLegend.Columns[column.ColumnName].Width = column.Width;
                gvFlawLegend.Columns[column.ColumnName].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            // Hide columns
            gvFlawLegend.Columns["Name"].Visible = false;
            gvFlawLegend.Columns["FlawType"].Visible = false;
            gvFlawLegend.Columns["Count"].Visible = false;
            gvFlawLegend.Columns["Color"].Visible = false;
            //gvFlawLegend.Columns["Shape"].Visible = false;
            gvFlawLegend.Columns["JobNum"].Visible = false;
            gvFlawLegend.Columns["DoffNum"].Visible = false;
            gvFlawLegend.Columns["OfflineDoffNum"].Visible = false;
            gvFlawLegend.Columns["OfflineJobNum"].Visible = false;
        }
        public void SetGvFlawClass(IList<FlawTypeNameExtend> flawTypes)
        {
            bsFlawType.DataSource = flawTypes;
        }

        public void ResetGvFlawClassDoffNum()
        {
            foreach (DataGridViewRow r in gvFlawClass.Rows)
            {
                r.Cells["DoffNum"].Value = 0;
            }
        }

        public void RefreshGvFlawClass()
        {
            gvFlawClass.Refresh();
            gvFlawClass.EndEdit();
        }

        private NLinearScaleConfigurator GetScaleConfigurator()
        {
            NLinearScaleConfigurator linearScale = new NLinearScaleConfigurator();

            linearScale.SetPredefinedScaleStyle(PredefinedScaleStyle.Scientific);
            linearScale.MajorGridStyle.SetShowAtWall(ChartWallType.Back, MapWindowVariable.ShowGridSet);
            linearScale.MajorGridStyle.LineStyle.Pattern = LinePattern.Dash;

            linearScale.RoundToTickMin = false;
            linearScale.RoundToTickMax = false;
            return linearScale;
        }

        private void AxisScaleConfigurator()
        {
            double tmpScale = 0;

            // Configure X axis scale
            NAxis xAxis = nChartMap.Axis(StandardAxis.PrimaryX);
            NLinearScaleConfigurator xLinearScale = xAxis.ScaleConfigurator as NLinearScaleConfigurator;

            xLinearScale.MajorTickMode = MajorTickMode.CustomStep;
            if (MapWindowVariable.MapGridSet == 0)
            {
                xLinearScale.CustomStep = MapWindowVariable.MapCDSet;
            }
            else
            {
                if (PxPVariable.PxPInfo != null)
                {
                    xLinearScale.CustomStep = PxPVariable.PxPWidth / MapWindowVariable.MapCDSet;
                }
            }
            xLinearScale.CustomStep = Math.Round(xLinearScale.CustomStep, 2);

            // Configure Y axis scale
            NAxis yAxis = nChartMap.Axis(StandardAxis.PrimaryY);
            NLinearScaleConfigurator yLinearScale = yAxis.ScaleConfigurator as NLinearScaleConfigurator;

            yLinearScale.MajorTickMode = MajorTickMode.CustomStep;
            if (MapWindowVariable.MapGridSet == 0)
            {
                yLinearScale.CustomStep = MapWindowVariable.MapMDSet;
            }
            else
            {
                if (PxPVariable.PxPInfo != null)
                {
                    yLinearScale.CustomStep = PxPVariable.PxPHeight / MapWindowVariable.MapMDSet;
                }
            }
            yLinearScale.CustomStep = Math.Round(yLinearScale.CustomStep, 2);

            if (MapWindowVariable.BottomAxe == 1)
            {
                tmpScale = xLinearScale.CustomStep;
                xLinearScale.CustomStep = yLinearScale.CustomStep;
                yLinearScale.CustomStep = tmpScale;
            }
            xAxis.UpdateScale();
            yAxis.UpdateScale();

        }

        public void OnChartMouseDoubleClick(object sender, MouseEventArgs e)
        {
            MapWindowThreadStatus.UpdateChange = true;
            PxPTab.MapThreadEvent.Set();
            NHitTestResult hitTestResult = nChart.HitTest(e.Location);
            if (hitTestResult.ChartElement == ChartElement.DataPoint)
            {
                NSeries series = hitTestResult.Series as NSeries;
                if (series != null && series.Name != "Markup")
                {
                    FlawForm FlawFormController = new FlawForm(
                        Convert.ToInt32(series.Name) - MapWindowVariable.FlawPieces[Convert.ToInt32(lbPageCurrent.Text) - 1][0].FlawID
                    );
                    FlawFormController.ShowDialog();
                }
            }
        }

        public void OnChartMouseMove(object sender, MouseEventArgs e)
        {
            NHitTestResult hitTestResult = nChart.HitTest(e.Location);
            if (hitTestResult.ChartElement == ChartElement.DataPoint)
            {
                NSeries series = hitTestResult.Series as NSeries;
                if (series != null && series.Name != "Markup")
                {
                    series.FillStyle = new NColorFillStyle(Color.Red);
                    series.DataLabelStyle.Visible = true;
                }
            }
            else
            {
                foreach (NSeries point in nChartMap.Series)
                {
                    if (point.Tag == null) continue;
                    foreach (DataGridViewRow row in gvFlawClass.Rows)
                    {
                        if (point.Tag.ToString() == row.Cells["FlawType"].Value.ToString())
                        {
                            point.FillStyle = new NColorFillStyle(System.Drawing.ColorTranslator.FromHtml(row.Cells["Color"].Value.ToString()));
                        }
                    }

                    point.DataLabelStyle.Visible = false;
                }
            }
            nChart.Refresh();
        }

        public void SetJobInfo()
        {
            lbOrderNumberValue.Text = PxPVariable.JobInfo.OrderNumber;
            lbJobIDValue.Text = PxPVariable.JobInfo.JobID;
            lbMeterialTypeValue.Text = PxPVariable.JobInfo.MaterialType;
            lbOperatorValue.Text = PxPVariable.JobInfo.OperatorName;
            lbDateTimeValue.Text = DateTime.Now.ToShortDateString();
            lbDoffValue.Text = MapWindowVariable.CurrentPiece.ToString();
            lbPass.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
            lbPassValue.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
            lbFail.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
            lbFailValue.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        }

        public void SetFilterRadioButtons()
        {
            switch (MapWindowVariable.ShowFlag)
            {  // 紀錄顯示項目 0:All, 1:Pass, 2:Fail
                case 0:
                    rbAll.Checked = true;
                    break;
                case 1:
                    rbPass.Checked = true;
                    break;
                case 2:
                    rbFail.Checked = true;
                    break;
                default:
                    rbAll.Checked = true;
                    break;
            }

        }

        public void SetMapProperty()
        {

            switch (MapWindowVariable.MapProportion)
            {
                case 0:  // 1:1
                    nChartMap.Width = 400;
                    nChartMap.Height = 400;
                    break;
                case 1:  // 2:1
                    nChartMap.Width = 400;
                    nChartMap.Height = 200;
                    break;
                case 2:  // 4:3
                    nChartMap.Width = 400;
                    nChartMap.Height = 300;
                    break;
                case 3:  // 3:4
                    nChartMap.Width = 300;
                    nChartMap.Height = 400;
                    break;
                case 4:  // 16:9
                    nChartMap.Width = 608;
                    nChartMap.Height = 342;
                    break;
                default:
                    nChartMap.Width = 400;
                    nChartMap.Height = 400;
                    break;
            }

            nChartMap.Axis(StandardAxis.PrimaryX).ScaleConfigurator = GetScaleConfigurator();
            nChartMap.Axis(StandardAxis.PrimaryY).ScaleConfigurator = GetScaleConfigurator();
            nChartMap.Axis(StandardAxis.PrimaryX).View = new NRangeAxisView(new NRange1DD(0, 0), true, true);
            nChartMap.Axis(StandardAxis.PrimaryY).View = new NRangeAxisView(new NRange1DD(0, 0), true, true);
            AxisScaleConfigurator();
            nChart.Refresh();
        }

        public void SetMapAxis()
        {
            if (PxPVariable.PxPInfo != null)
            {
                PxPVariable.PxPWidth = PxPVariable.PxPInfo.Width * Convert.ToDouble(PxPVariable.UnitsData.Tables["unit"].Rows[PxPVariable.UnitsKeys["Flaw Map CD"]].ItemArray[2].ToString());
                PxPVariable.PxPHeight = PxPVariable.PxPInfo.Height * Convert.ToDouble(PxPVariable.UnitsData.Tables["unit"].Rows[PxPVariable.UnitsKeys["Flaw Map MD"]].ItemArray[2].ToString());

                //if (MapWindowVariable.BottomAxe == 0)
                //{
                //    nChartMap.Axis(StandardAxis.PrimaryX).View = new NRangeAxisView(new NRange1DD(0, PxPVariable.PxPWidth), true, true);
                //    nChartMap.Axis(StandardAxis.PrimaryY).View = new NRangeAxisView(new NRange1DD(0, PxPVariable.PxPHeight), true, true);
                //}
                //else
                //{
                //    nChartMap.Axis(StandardAxis.PrimaryX).View = new NRangeAxisView(new NRange1DD(0, PxPVariable.PxPHeight), true, true);
                //    nChartMap.Axis(StandardAxis.PrimaryY).View = new NRangeAxisView(new NRange1DD(0, PxPVariable.PxPWidth), true, true);
                //}
                if (MapWindowVariable.BottomAxe == 0)
                {
                    if (MapWindowVariable.CDInver == 0)
                    {
                        nChartMap.Axis(StandardAxis.PrimaryX).View = new NRangeAxisView(new NRange1DD(0, PxPVariable.PxPWidth), true, true);
                    }
                    else
                    {
                        nChartMap.Axis(StandardAxis.PrimaryX).View = new NRangeAxisView(new NRange1DD(-PxPVariable.PxPWidth, 0), true, true);
                    }
                    if (MapWindowVariable.MDInver == 0)
                    {
                        nChartMap.Axis(StandardAxis.PrimaryY).View = new NRangeAxisView(new NRange1DD(0, PxPVariable.PxPHeight), true, true);
                    }
                    else
                    {
                        nChartMap.Axis(StandardAxis.PrimaryY).View = new NRangeAxisView(new NRange1DD(-PxPVariable.PxPHeight, 0), true, true);
                    }
                }
                else
                {
                    if (MapWindowVariable.MDInver == 0)
                    {
                        nChartMap.Axis(StandardAxis.PrimaryX).View = new NRangeAxisView(new NRange1DD(0, PxPVariable.PxPHeight), true, true);
                    }
                    else
                    {
                        nChartMap.Axis(StandardAxis.PrimaryX).View = new NRangeAxisView(new NRange1DD(-PxPVariable.PxPHeight, 0), true, true);
                    }
                    if (MapWindowVariable.CDInver == 0)
                    {
                        nChartMap.Axis(StandardAxis.PrimaryY).View = new NRangeAxisView(new NRange1DD(0, PxPVariable.PxPWidth), true, true);
                    }
                    else
                    {
                        nChartMap.Axis(StandardAxis.PrimaryY).View = new NRangeAxisView(new NRange1DD(-PxPVariable.PxPWidth, 0), true, true);
                    }
                }
            }
            else
            {
                nChartMap.Axis(StandardAxis.PrimaryX).View = new NRangeAxisView(new NRange1DD(0, 0), true, true);
                nChartMap.Axis(StandardAxis.PrimaryY).View = new NRangeAxisView(new NRange1DD(0, 0), true, true);
            }
            nChartMap.Axis(StandardAxis.PrimaryX).ScaleConfigurator = GetScaleConfigurator();
            nChartMap.Axis(StandardAxis.PrimaryX).PagingView.MinPageLength = 0.01f;
            nChartMap.Axis(StandardAxis.PrimaryY).ScaleConfigurator = GetScaleConfigurator();
            nChartMap.Axis(StandardAxis.PrimaryY).PagingView.MinPageLength = 0.01f;
            nChart.Refresh();
            AxisScaleConfigurator();

        }

        public void SetMapInfoLabel()
        {
            lbDoffValue.Text = PxPVariable.DoffNum.ToString();
            lbFailValue.Text = PxPVariable.FailNum.ToString();
            lbPassValue.Text = PxPVariable.PassNum.ToString();
            lbYieldValue.Text = (Math.Round((double)PxPVariable.PassNum / (double)(PxPVariable.PassNum + PxPVariable.FailNum), 4) * 100).ToString() + "%";

             if (MapWindowVariable.PieceResult[MapWindowVariable.CurrentPiece - 1])
             {
                 lbFail.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
                 lbFailValue.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
                 lbPass.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00ff00");
                 lbPassValue.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00ff00");
             }
             else
             {
                 lbFail.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff0000");
                 lbFailValue.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff0000");
                 lbPass.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
                 lbPassValue.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
             }
        }

        public void SetMapInfoLabel(bool IsPass)
        {
            lbDoffValue.Text = PxPVariable.DoffNum.ToString();
            lbFailValue.Text = PxPVariable.FailNum.ToString();
            lbPassValue.Text = PxPVariable.PassNum.ToString();
            lbYieldValue.Text = (Math.Round((double)PxPVariable.PassNum / (double)(PxPVariable.PassNum + PxPVariable.FailNum), 4) * 100).ToString() + "%";

            if (IsPass)
            {
                lbFail.ForeColor = System.Drawing.ColorTranslator.FromHtml("#082d07");
                lbFailValue.ForeColor = System.Drawing.ColorTranslator.FromHtml("#082d07");
                lbPass.ForeColor = System.Drawing.ColorTranslator.FromHtml("#1a9318");
                lbPassValue.ForeColor = System.Drawing.ColorTranslator.FromHtml("#1a9318");
            }
            else
            {
                lbFail.ForeColor = System.Drawing.ColorTranslator.FromHtml("#d1260b");
                lbFailValue.ForeColor = System.Drawing.ColorTranslator.FromHtml("#d1260b");
                lbPass.ForeColor = System.Drawing.ColorTranslator.FromHtml("#082d07");
                lbPassValue.ForeColor = System.Drawing.ColorTranslator.FromHtml("#082d07");
            }
        }
        public void SetUserTermLabel(IUserTerms terms)
        {
            lbDoff.Text = terms.Doff;
            lbJobID.Text =   terms.JobID;
            lbMeterialType.Text =  terms.MaterialType;
            lbOperator.Text =  terms.OperatorName;
            lbOrderNumber.Text =  terms.OrderNumber;
        }
        
        public void SetTotalScoreLabel(double score)
        {
            lbTotalScoreValue.Text = score.ToString();
        }

        public void SetPieceTotalLabel()
        {
            lbPageTotal.Text = PxPVariable.FreezPiece.ToString();
        }

        public void CountFlawPieceDoffNum()
        {

            foreach (FlawTypeNameExtend c in PxPVariable.FlawTypeName)
            {
                c.DoffNum = 0;
            }
            foreach (FlawInfoAddPriority f in MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1])
            {
                
                foreach (FlawTypeNameExtend ft in PxPVariable.FlawTypeName)
                {
                    if (ft.FlawType == f.FlawType)
                    {
                        ft.DoffNum++;
                    }
                }
            }
            gvFlawClass.Refresh();
        }

        public int CheckPieceNum(int PageNum, string Direction)
        {
            if ((PageNum < PxPVariable.FreezPiece - PxPVariable.PieceLimit) || (PageNum > PxPVariable.FreezPiece))
            { }
            else
            {
                bool result = MapWindowVariable.PieceResult[PageNum - 1];

                if ((MapWindowVariable.ShowFlag == 1 && result) || (MapWindowVariable.ShowFlag == 2 && !result))
                    return PageNum;
                else
                    if (PageNum == 1)
                    { }
                    else
                        if (Direction == "Next")
                            return CheckPieceNum(PageNum + 1, Direction);
                        else
                            return CheckPieceNum(PageNum - 1, Direction);
            }

            return MapWindowVariable.CurrentPiece;  // Not Found
        }

        public void InitLabel()
        {
            lbPageCurrent.Text = "--";
            lbPageTotal.Text = "--";
            lbTotalScoreValue.Text = "--";
        }

        public void InitSubPiece()
        {
            // Draw horizontal line
            nChartMap.Axis(StandardAxis.PrimaryX).ConstLines.Clear();
            nChartMap.Axis(StandardAxis.PrimaryY).ConstLines.Clear();

            for (int i = 0; i < GradeVariable.RoiRowsGrid.Count; i++)
            {
                for (int j = 0; j < GradeVariable.RoiColumnsGrid.Count; j++)
                {
                    // Draw horizontal start line
                    NAxisConstLine cl = nChartMap.Axis(StandardAxis.PrimaryY).ConstLines.Add();
                    cl.StrokeStyle.Color = Color.FromArgb(80, Color.Blue);
                    cl.StrokeStyle.Width = new NLength(1.5f);
                    cl.Value = GradeVariable.RoiRowsGrid[i].Start;
                }
                for (int j = 0; j < GradeVariable.RoiColumnsGrid.Count; j++)
                {
                    // Draw horizontal end line
                    NAxisConstLine cl = nChartMap.Axis(StandardAxis.PrimaryY).ConstLines.Add();
                    cl.StrokeStyle.Color = Color.FromArgb(80, Color.Blue);
                    cl.StrokeStyle.Width = new NLength(1.5f);
                    cl.Value = GradeVariable.RoiRowsGrid[i].End;
                }
            }

            for (int i = 0, j = 0; i < GradeVariable.RoiRowsGrid.Count * 2 * GradeVariable.RoiColumnsGrid.Count; i++, j++)
            {
                DrawSubPieceHorizontal(i, GradeVariable.RoiColumnsGrid[j].Start, GradeVariable.RoiColumnsGrid[j].End);
                if (j == (GradeVariable.RoiColumnsGrid.Count - 1))
                    j = -1;
            }

            // Draw vertical line
            for (int i = 0; i < GradeVariable.RoiColumnsGrid.Count; i++)
            {
                for (int j = 0; j < GradeVariable.RoiRowsGrid.Count; j++)
                {
                    // Draw vertical start line
                    NAxisConstLine cl = nChartMap.Axis(StandardAxis.PrimaryX).ConstLines.Add();
                    cl.StrokeStyle.Color = Color.FromArgb(80, Color.Blue);
                    cl.StrokeStyle.Width = new NLength(1.5f);
                    cl.Value = GradeVariable.RoiColumnsGrid[i].Start;
                }
                for (int j = 0; j < GradeVariable.RoiRowsGrid.Count; j++)
                {
                    // Draw vertical end line
                    NAxisConstLine cl = nChartMap.Axis(StandardAxis.PrimaryX).ConstLines.Add();
                    cl.StrokeStyle.Color = Color.FromArgb(80, Color.Blue);
                    cl.StrokeStyle.Width = new NLength(1.5f);
                    cl.Value = GradeVariable.RoiColumnsGrid[i].End;
                }
            }

            for (int i = 0, j = 0; i < GradeVariable.RoiRowsGrid.Count * 2 * GradeVariable.RoiColumnsGrid.Count; i++, j++)
            {
                DrawSubPieceVertical(i, GradeVariable.RoiRowsGrid[j].Start, GradeVariable.RoiRowsGrid[j].End);
                if (j == (GradeVariable.RoiRowsGrid.Count - 1))
                    j = -1;
            }
        }

        private void DrawSubPieceHorizontal(int idx, double begin, double end)
        {
            NAxisConstLine cl = (NAxisConstLine)nChartMap.Axis(StandardAxis.PrimaryY).ConstLines[idx];
            NAxis referenceAxis = nChartMap.Axis(StandardAxis.PrimaryX);
            cl.ReferenceRanges.Add(new NReferenceAxisRange(referenceAxis, begin, end));
        }

        private void DrawSubPieceVertical(int idx, double begin, double end)
        {
            NAxisConstLine cl = (NAxisConstLine)nChartMap.Axis(StandardAxis.PrimaryX).ConstLines[idx];
            NAxis referenceAxis = nChartMap.Axis(StandardAxis.PrimaryY);
            cl.ReferenceRanges.Add(new NReferenceAxisRange(referenceAxis, begin, end));
        }

        private void SubPieceFail(int SubPieceNumber)
        {
            int top = 0, bottom = 0, left = 0, right = 0;

            bottom = (SubPieceNumber / GradeVariable.RoiColumnsGrid.Count) * (GradeVariable.RoiColumnsGrid.Count * 2) + (SubPieceNumber % GradeVariable.RoiColumnsGrid.Count);
            top = bottom + GradeVariable.RoiColumnsGrid.Count;
            left = (SubPieceNumber / GradeVariable.RoiColumnsGrid.Count) + (SubPieceNumber % GradeVariable.RoiColumnsGrid.Count * 2 * GradeVariable.RoiRowsGrid.Count);
            right = left + GradeVariable.RoiRowsGrid.Count;

            NAxisConstLine clFail = (NAxisConstLine)nChartMap.Axis(StandardAxis.PrimaryY).ConstLines[top];
            clFail.StrokeStyle.Color = Color.Red;
            clFail = (NAxisConstLine)nChartMap.Axis(StandardAxis.PrimaryY).ConstLines[bottom];
            clFail.StrokeStyle.Color = Color.Red;
            clFail = (NAxisConstLine)nChartMap.Axis(StandardAxis.PrimaryX).ConstLines[left];
            clFail.StrokeStyle.Color = Color.Red;
            clFail = (NAxisConstLine)nChartMap.Axis(StandardAxis.PrimaryX).ConstLines[right];
            clFail.StrokeStyle.Color = Color.Red;
        }

        private void SubPieceMarkup(int pieceNum)
        {
            SplitPieces SubPieces = GradeVariable.SplitPiecesContainer[pieceNum];

            foreach (RoiGrade r in GradeVariable.RoiRowsGrid)
            {
                foreach (RoiGrade c in GradeVariable.RoiColumnsGrid)
                {
                    foreach (SplitPiece s in SubPieces.Pieces)
                    {
                        if (s.Name == string.Format("ROI-{0}{1}", r.Name, c.Name))
                        {
                            NPointSeries labels = new NPointSeries();
                            
                            labels.Name = "Markup";
                            labels.UseXValues = true;
                            labels.MarkerStyle.Visible = false;
                            labels.DataLabelStyle.Format = "<label>";
                            labels.BorderStyle.Width = new NLength(0);
                            labels.FillStyle.SetTransparencyPercent(100);

                            labels.DataLabelStyle.TextStyle.FontStyle = new NFontStyle("Consolas", new NLength(12, NGraphicsUnit.Point), FontStyle.Bold);
                            labels.DataLabelStyle.TextStyle.FillStyle = new NColorFillStyle(Color.Blue);
                            labels.DataLabelStyle.TextStyle.BackplaneStyle.Visible = false;
                            labels.DataLabelStyle.TextStyle.Orientation = 0;
                            labels.DataLabelStyle.TextStyle.StringFormatStyle.HorzAlign = Nevron.HorzAlign.Left;
                            labels.DataLabelStyle.TextStyle.StringFormatStyle.VertAlign = Nevron.VertAlign.Top;
                            labels.DataLabelStyle.ArrowLength = new NLength(0);

                            labels.Values.Add((r.End - (r.End - r.Start) * 0.15));
                            labels.XValues.Add((c.Start + (c.End - c.Start) * 0.05));

                            labels.Labels.Add(string.Format("{0} - {1}({2})", s.Name, s.Score, s.GradeLevel));

                            nChartMap.Series.Add(labels);
                        }
                    }
                }
            }
        }

        #endregion

        #region Action Events

        private void MapWindow_Load(object sender, EventArgs e)
        {
            InitGvFlawClass();
            InitGvFlawLegend();
        }

        private void btnMapSetup_Click(object sender, EventArgs e)
        {
            if (!PxPThreadStatus.IsOnOnline && !PxPThreadStatus.IsOnJobLoaded || !PxPThreadStatus.IsOnOnline && PxPThreadStatus.IsOnJobStopped)
            {
                MapSetup MapSetup = new MapSetup();
                MapSetup.ShowDialog();
                gvFlawClass.Refresh();
            }
        }

        private void nChart_MouseClick(object sender, MouseEventArgs e)
        {
            MapWindowThreadStatus.UpdateChange = true;
            PxPTab.MapThreadEvent.Set();
            PxPVariable.FreezPiece = MapWindowVariable.FlawPieces.Count;
        }

        private void btnPrevPiece_Click(object sender, EventArgs e)
        {
            if (PxPThreadStatus.IsOnOnline)
            {
                // Set WebInspector Offline
                MapWindowThreadStatus.UpdateChange = true;
                PxPVariable.FreezPiece = MapWindowVariable.FlawPieces.Count;
                lbPageTotal.Text = PxPVariable.FreezPiece.ToString();
            }
            
            int PieceNum = MapWindowVariable.CurrentPiece - 1;
            if (MapWindowVariable.ShowFlag != 0)
            {
                PieceNum = CheckPieceNum(PieceNum, "Prev");
            }

            MapWindowVariable.CurrentPiece = PieceNum;
            if ((MapWindowVariable.CurrentPiece == 1) || (MapWindowVariable.CurrentPiece == PxPVariable.FreezPiece - PxPVariable.PieceLimit))
                btnPrevPiece.Enabled = false;
            if (MapWindowVariable.CurrentPiece == PxPVariable.FreezPiece)
                btnNextPiece.Enabled = false;
            else
                btnNextPiece.Enabled = true;
            CountFlawPieceDoffNum();
            lbPageCurrent.Text = MapWindowVariable.CurrentPiece.ToString();
            MapWindowVariable.MapWindowController.SetTotalScoreLabel(CountPieceScore(MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1]));

            DrawPieceFlaw(PieceNum - 1, false);
            MapWindowThreadStatus.IsChangePiece = true;
            PxPTab.MapThreadEvent.Set();
        }

        private void btnNextPiece_Click(object sender, EventArgs e)
        {
            if (PxPThreadStatus.IsOnOnline)
            {
                // Set WebInspector Offline
                MapWindowThreadStatus.UpdateChange = true;
                PxPVariable.FreezPiece = MapWindowVariable.FlawPieces.Count;
                lbPageTotal.Text = PxPVariable.FreezPiece.ToString();
            }

            int PieceNum = MapWindowVariable.CurrentPiece + 1;
            if (MapWindowVariable.ShowFlag != 0)
            {
                PieceNum = CheckPieceNum(PieceNum, "Next");
            }

            MapWindowVariable.CurrentPiece = PieceNum;
            if (MapWindowVariable.CurrentPiece == PxPVariable.FreezPiece)
                btnNextPiece.Enabled = false;
            if ((MapWindowVariable.CurrentPiece == 1) || (MapWindowVariable.CurrentPiece == PxPVariable.FreezPiece - PxPVariable.PieceLimit))
                btnPrevPiece.Enabled = false;
            else
                btnPrevPiece.Enabled = true;
            CountFlawPieceDoffNum();
            lbPageCurrent.Text = MapWindowVariable.CurrentPiece.ToString();
            MapWindowVariable.MapWindowController.SetTotalScoreLabel(CountPieceScore(MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1]));

            DrawPieceFlaw(PieceNum - 1, false);
            MapWindowThreadStatus.IsChangePiece = true;
            PxPTab.MapThreadEvent.Set();
        }

        private void rb_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                switch (((RadioButton)sender).Name)
                {
                    // 0: All, 1: Pass, 2: Fail
                    case "rbAll":
                        MapWindowVariable.ShowFlag = 0;
                        break;

                    case "rbFail":
                        MapWindowVariable.ShowFlag = 2;
                        break;

                    case "rbPass":
                        MapWindowVariable.ShowFlag = 1;
                        break;
                }
            }
        }

        private void gvFlawClass_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 5)
                e.CellStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml(gvFlawClass.Rows[e.RowIndex].Cells["Color"].Value.ToString());
            if (e.ColumnIndex == 4)
                e.CellStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml(e.Value.ToString());
        }

        private void gvFlawClass_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in gvFlawClass.Rows)
            {
                try
                {
                    if (e.ColumnIndex == row.Cells["Display"].ColumnIndex)
                    {
                        if (MapWindowVariable.FlawPieces.Count > 0)
                        {
                            if (Convert.ToBoolean(row.Cells["Display"].EditedFormattedValue))
                            {
                                DrawPieceFlaw(MapWindowVariable.CurrentPiece - 1, false);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void gvFlawLegend_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 5)
                e.CellStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml(gvFlawClass.Rows[e.RowIndex].Cells["Color"].Value.ToString());
            if (e.ColumnIndex == 4)
                e.CellStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml(e.Value.ToString());
        }

        private void gvFlawLegend_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in gvFlawLegend.Rows)
            {
                try
                {
                    if (e.ColumnIndex == row.Cells["Display"].ColumnIndex)
                    {
                        if (MapWindowVariable.FlawPieces.Count > 0)
                        {
                            if (Convert.ToBoolean(row.Cells["Display"].EditedFormattedValue))
                            {
                                DrawPieceFlaw(MapWindowVariable.CurrentPiece - 1, false);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void btnGradeSetting_Click(object sender, EventArgs e)
        {
            GradeSetup gs = new GradeSetup();
            gs.ShowDialog();

            // 組態檔繫結
            IsClickGradeSetup = true;
            bsGradConfigList.DataSource = GetGradeConfList(); 
            cboxGradeConfigFile.DataSource = bsGradConfigList.DataSource;

        }

        private void cboxGradeConfigFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsClickGradeSetup)
            {
                cboxGradeConfigFile.SelectedItem = SystemVariable.GradeConfigFileName.Substring(0, SystemVariable.GradeConfigFileName.LastIndexOf("."));
            }
            else
            {
                SystemVariable.GradeConfigFileName = cboxGradeConfigFile.SelectedValue + ".xml";
            }
            SystemVariable.LoadGradeConfig();
            IsClickGradeSetup = false;
        }

        private void btnFailList_Click(object sender, EventArgs e)
        {
            // Check whether the form is opened
            if (fl == null || fl.IsDisposed)
            {
                if (PxPThreadStatus.IsOnOnline)
                {
                    // Set WebInspector Offline
                    MapWindowThreadStatus.UpdateChange = true;
                    PxPVariable.FreezPiece = MapWindowVariable.FlawPieces.Count;
                    lbPageTotal.Text = PxPVariable.FreezPiece.ToString();
                }

                fl = new FailList();
                fl.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                fl.Location = new Point(Screen.PrimaryScreen.Bounds.Width - fl.Width - 5, 5);
                fl.Show();
            }
        }

        #endregion

       
    }
}
