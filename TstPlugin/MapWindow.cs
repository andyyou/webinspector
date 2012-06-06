using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WRPlugIn;
using Nevron.Chart;
using Nevron.GraphicsCore;
using Nevron.Chart.WinForm;
using System.Xml.Linq;
using System.IO;
using System.Reflection;

namespace PxP
{

    public partial class MapWindow : UserControl
    {
        #region MapWindow Variables
        private NCartesianChart nChartMap;
        #endregion

        #region Conturctor
        public MapWindow()
        {
            
            //MessageBox.Show("MapWindow Conturctor");
            InitializeComponent();
            SystemVariable.LoadConfig();
            InitNChart(ref nChart, out nChartMap);
            
            SetFilterRadioButtons();
            btnPrevPiece.Enabled = false;
            btnNextPiece.Enabled = false;

            //Get grade config list
            ///////////////////////////////////////////////////////////////////////////////////////
            //組態檔繫結
            bsGradConfigList.DataSource = GetGradeConfList();
            cboxGradeConfigFile.DataSource = bsGradConfigList.DataSource;
            cboxGradeConfigFile.SelectedItem = SystemVariable.ConfigFileName.ToString().Substring(0, SystemVariable.GradeConfigFileName.ToString().LastIndexOf("."));
            ///////////////////////////////////////////////////////////////////////////////////////
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
                //紀錄顯示項目 0:All, 1:Pass, 2:Fail
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
                MessageBox.Show("Map Setup UnConstructor Error");
            }
        }
        #endregion

        #region Refactoring
        public void InitNChart(ref NChartControl ncc, out NCartesianChart chart)
        {
            //2D line chart
            ncc.Settings.RenderDevice = RenderDevice.GDI;
            //Add chart header
            //NLabel nchartHeader = nChart.Labels.AddHeader("缺陷圖");

            //Add tools to chart controller
            ncc.Controller.Tools.Add(new NSelectorTool());
            ncc.Controller.Tools.Add(new NDataZoomTool());
            //ncc.Controller.Tools.Add(new NDataPanTool());
            ncc.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(OnChartMouseDoubleClick);
            ncc.MouseMove += new System.Windows.Forms.MouseEventHandler(OnChartMouseMove);

            chart = (NCartesianChart)ncc.Charts[0];

            //Set range selections property
            NRangeSelection rangeSelection = new NRangeSelection();
            //Reset Axis when zoom out
            rangeSelection.ZoomOutResetsAxis = true;
            chart.RangeSelections.Add(rangeSelection);

            //Set chart axis property
            chart.Axis(StandardAxis.Depth).Visible = false;
            //chart.BoundsMode = BoundsMode.Stretch;
            SetMapProperty();
            
        }
        //取得Folder底下所有XML清單
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
        //計算piece的分數
        public double CountPieceScore(List<FlawInfoAddPriority> list)
        {
            double tmp = 0;
            foreach (var i in list)
            {
                tmp += i.PointScore;
            }
            return tmp;
        }
        //判斷piece是Pass or Fail
        public bool GetPassOfPiece(List<FlawInfoAddPriority> list)
        {
            double score = CountPieceScore(list);

            if (score >= GradeVariable.PassOrFileScore)
                return false;
            else
                return true;


        }
        #endregion

        #region Inherit Interface



        #endregion

        #region Method
        public void DrawPieceFlaw(List<FlawInfoAddPriority> flawPiece, bool drawFlag)
        {
            nChartMap.Series.Clear();

            if (flawPiece.Count > 0)
            {
                bool skipFlag = false;
                foreach (var f in flawPiece)
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
                            if (!Convert.ToBoolean(row.Cells["Display"].EditedFormattedValue))
                            {
                                skipFlag = true;
                                break;
                            }

                            point.FillStyle = new NColorFillStyle(System.Drawing.ColorTranslator.FromHtml(row.Cells["Color"].Value.ToString()));
                            switch (row.Cells["Shape"].Value.ToString())
                            {
                                case "▲":
                                    point.PointShape = PointShape.DiagonalCross;
                                    //point.PointShape = PointShape.Cone
                                    break;
                                case "▼":
                                    point.PointShape = PointShape.InvertedCone;
                                    //point.PointShape = PointShape.InvertedPyramid;
                                    break;
                                case "■":
                                    point.PointShape = PointShape.Bar;
                                    break;
                                case "●":
                                    point.PointShape = PointShape.Sphere;
                                    //point.PointShape = PointShape.Ellipse
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
                    if (skipFlag)
                        continue;

                    point.Tag = f.FlawType;
                    point.UseXValues = true;
                    //When BottomAxe equals zero, bottom axis is CD otherwise is RMD
                    if (MapWindowVariable.BottomAxe == 0)
                        point.AddDataPoint(new NDataPoint(f.CD, f.RMD));
                    else
                        point.AddDataPoint(new NDataPoint(f.RMD, f.CD));

                }
            }

            
            if (drawFlag)
            {
                MapWindowVariable.CurrentPiece = MapWindowVariable.FlawPieces.Count;
                lbPageCurrent.Text = MapWindowVariable.FlawPieces.Count.ToString();
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
        }
        public void ClearMap()
        {
            nChartMap.Series.Clear();
            nChart.Refresh();
        }
        public void InitGvFlawClass()
        {

            //IList<FlawTypeNameExtend> tmpFlawTypes = new List<FlawTypeNameExtend>();
            bsFlawType.DataSource = PxPVariable.FlawTypeName;
            gvFlawClass.DataSource = bsFlawType;
            gvFlawClass.AllowUserToAddRows = false;
            
            foreach (var column in MapWindowVariable.DoffTypeGridSetup)
            {
                if (column.ColumnName == "Display")
                    gvFlawClass.Columns[column.ColumnName].ReadOnly = false;
                else
                    gvFlawClass.Columns[column.ColumnName].ReadOnly = true;

                gvFlawClass.Columns[column.ColumnName].SortMode = DataGridViewColumnSortMode.Automatic;
                gvFlawClass.Columns[column.ColumnName].HeaderText = column.HeaderText;
                gvFlawClass.Columns[column.ColumnName].DisplayIndex = column.Index;
                gvFlawClass.Columns[column.ColumnName].Width = column.Width;
            }
            //Display
            gvFlawClass.Columns["Count"].Visible = false;
            
         
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
            //linearScale.MinorTickCount = 5;

            linearScale.MajorGridStyle.SetShowAtWall(ChartWallType.Back, MapWindowVariable.ShowGridSet);
            linearScale.MajorGridStyle.LineStyle.Pattern = LinePattern.Dot;

            linearScale.RoundToTickMin = false;
            linearScale.RoundToTickMax = false;
            return linearScale;
        }
        private void AxisScaleConfigurator()
        {
            double tmpScale = 0;
            //Configure X axis scale
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

            //Configure Y axis scale
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
                if (series != null)
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
                if (series != null)
                {
                    //MessageBox.Show(hitTestResult.Series.Id.ToString());
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
            //lbPassValue.Text = "";
            //lbFailValue.Text = "";
            
        }
        public void SetFilterRadioButtons()
        {
            switch (MapWindowVariable.ShowFlag)
            {  //紀錄顯示項目 0:All, 1:Pass, 2:Fail
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
                case 0: // 1:1
                    nChartMap.Width = 400;
                    nChartMap.Height = 400;
                    break;
                case 1: // 2:1
                    nChartMap.Width = 400;
                    nChartMap.Height = 200;
                    break;
                case 2: // 4:3
                    nChartMap.Width = 400;
                    nChartMap.Height = 300;
                    break;
                case 3: // 3:4
                    nChartMap.Width = 300;
                    nChartMap.Height = 400;
                    break;
                case 4: // 16:9
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

                if (MapWindowVariable.BottomAxe == 0)
                {
                    nChartMap.Axis(StandardAxis.PrimaryX).View = new NRangeAxisView(new NRange1DD(0, PxPVariable.PxPWidth), true, true);
                    nChartMap.Axis(StandardAxis.PrimaryY).View = new NRangeAxisView(new NRange1DD(0, PxPVariable.PxPHeight), true, true);
                }
                else
                {
                    nChartMap.Axis(StandardAxis.PrimaryX).View = new NRangeAxisView(new NRange1DD(0, PxPVariable.PxPHeight), true, true);
                    nChartMap.Axis(StandardAxis.PrimaryY).View = new NRangeAxisView(new NRange1DD(0, PxPVariable.PxPWidth), true, true);
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
        }
        public void SetUserTermLabel(IUserTerms terms)
        {
            lbDoff.Text = terms.Doff;
            lbJobID.Text =   terms.JobID;
            lbMeterialType.Text =  terms.MaterialType;
            lbOperator.Text =  terms.OperatorName;
            lbOrderNumber.Text =  terms.OrderNumber;
        }
        //2012/06/04
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
            foreach (var c in PxPVariable.FlawTypeName)
            {
                c.DoffNum = 0;
            }
            foreach (var f in MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1])
            {
                
                foreach (var ft in PxPVariable.FlawTypeName)
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
            //return MapWindowVariable.CurrentPiece - 1; //Not Found
            else
            {
                bool result = MapWindowVariable.PieceResult[PageNum - 1];

                if ((MapWindowVariable.ShowFlag == 1 && result) || (MapWindowVariable.ShowFlag == 2 && !result))
                    return PageNum;
                else
                    if (PageNum == 1)
                    { }
                    //return MapWindowVariable.CurrentPiece - 1; //Not Found
                    else
                        if (Direction == "Next")
                            return CheckPieceNum(PageNum + 1, Direction);
                        else
                            return CheckPieceNum(PageNum - 1, Direction);
            }
            return MapWindowVariable.CurrentPiece; //Not Found
        }
        public void InitLabel()
        {
            lbPageCurrent.Text = "--";
            lbPageTotal.Text = "--";
            lbTotalScoreValue.Text = "--";
        }

        public void InitSubPiece()
        {
            // Clear constlines
            nChartMap.Axis(StandardAxis.PrimaryX).ConstLines.Clear();
            nChartMap.Axis(StandardAxis.PrimaryY).ConstLines.Clear();

            // Draw horizontal line
            for (int i = 0; i < GradeVariable.RoiRowsGrid.Count; i++)
            {
                for (int j = 0; j < GradeVariable.RoiColumnsGrid.Count; j++)
                {
                    // Draw horizontal start line
                    NAxisConstLine cl = nChartMap.Axis(StandardAxis.PrimaryY).ConstLines.Add();
                    cl.StrokeStyle.Color = Color.FromArgb(80, Color.Blue);
                    cl.StrokeStyle.Width = new NLength(1.5f);
                    //cl.FillStyle = new NColorFillStyle(new NArgbColor(12, Color.SteelBlue));
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

            //******************************************************************//
            // Draw vertical line
            for (int i = 0; i < GradeVariable.RoiColumnsGrid.Count; i++)
            {
                for (int j = 0; j < GradeVariable.RoiRowsGrid.Count; j++)
                {
                    // Draw vertical start line
                    NAxisConstLine cl = nChartMap.Axis(StandardAxis.PrimaryX).ConstLines.Add();
                    cl.StrokeStyle.Color = Color.FromArgb(80, Color.Blue);
                    cl.StrokeStyle.Width = new NLength(1.5f);
                    //cl.FillStyle = new NColorFillStyle(new NArgbColor(125, Color.SteelBlue));
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

        public void SubPieceFail(int SubPieceNumber)
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
        #endregion

        #region Action Events
        private void MapWindow_Load(object sender, EventArgs e)
        {
            InitGvFlawClass();
            
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
                //Set WebInspector Offline
                MapWindowThreadStatus.UpdateChange = true;
                PxPVariable.FreezPiece = MapWindowVariable.FlawPieces.Count;
                lbPageTotal.Text = PxPVariable.FreezPiece.ToString();
            }
           
            int PieceNum = MapWindowVariable.CurrentPiece - 1;
            if (MapWindowVariable.ShowFlag != 0)
            {
                PieceNum = CheckPieceNum(PieceNum, "Prev");
                //MessageBox.Show(PieceNum.ToString());
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
            MapWindowVariable.MapWindowController.SetTotalScoreLabel(CountPieceScore(MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece -1 ]));

            DrawPieceFlaw(MapWindowVariable.FlawPieces[PieceNum - 1], false);
            //2012-05-04 小心online時的不良影響 連動功能
            MapWindowThreadStatus.IsChangePiece = true;
            PxPTab.MapThreadEvent.Set();
        }
        private void btnNextPiece_Click(object sender, EventArgs e)
        {
            if (PxPThreadStatus.IsOnOnline)
            {
                //Set WebInspector Offline
                MapWindowThreadStatus.UpdateChange = true;
                PxPVariable.FreezPiece = MapWindowVariable.FlawPieces.Count;
                lbPageTotal.Text = PxPVariable.FreezPiece.ToString();
               
            }
           
            
            int PieceNum = MapWindowVariable.CurrentPiece + 1;
            if (MapWindowVariable.ShowFlag != 0)
            {
                PieceNum = CheckPieceNum(PieceNum, "Next");
                //MessageBox.Show(PieceNum.ToString());
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
            MapWindowVariable.MapWindowController.SetTotalScoreLabel(CountPieceScore(MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece-1]));

            DrawPieceFlaw(MapWindowVariable.FlawPieces[PieceNum - 1], false);
            MapWindowThreadStatus.IsChangePiece = true;
            PxPTab.MapThreadEvent.Set();
        }
        private void rb_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                switch (((RadioButton)sender).Name)
                {   //0:All, 1:Pass, 2:Fail
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
                            if (Convert.ToBoolean(row.Cells["Display"].EditedFormattedValue))
                                DrawPieceFlaw(MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1], false);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void lbTotalScore_Click(object sender, EventArgs e)
        {

        }

        private void btnGradeSetting_Click(object sender, EventArgs e)
        {
            GradeSetup gs = new GradeSetup();
            gs.ShowDialog();
        }

        private void cboxGradeConfigFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            SystemVariable.GradeConfigFileName = cboxGradeConfigFile.SelectedValue + ".xml";
            SystemVariable.LoadGradeConfig();
        }
        #endregion

       

       

       

      

        

        

       

       


    }
}
