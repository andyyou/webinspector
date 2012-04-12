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
using System.Text.RegularExpressions;

namespace PxP
{

    public partial class MapWindow : UserControl
    {
        #region MapWindow Variables
        private NChart nChartMap;
        private NPointSeries nPoint;
        #endregion

        #region Conturctor
        public MapWindow()
        {
            
            //MessageBox.Show("MapWindow Conturctor");
            DebugTool.WriteLog("PxPTab.cs", "MapWindow Contructor");
            InitializeComponent();
            SystemVariable.LoadConfig();
            InitNChart(ref nChart, out nChartMap, out nPoint);
            
            SetFilterRadioButtons();
            btnPrevPiece.Enabled = false;
            btnNextPiece.Enabled = false;
            lbPageCurrent.Text = "0";
            lbPageTotal.Text = "0";
        }
        ~MapWindow()
        {
            
        }
        #endregion

        #region Refactoring
        void InitNChart(ref NChartControl ncc, out NChart nc, out NPointSeries np)
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

            nc = ncc.Charts[0];
            NCartesianChart chart = (NCartesianChart)nc;

            //Set range selections property
            NRangeSelection rangeSelection = new NRangeSelection();
            //Reset Axis when zoom out
            rangeSelection.ZoomOutResetsAxis = true;
            chart.RangeSelections.Add(rangeSelection);

            //Set chart axis property
            chart.Axis(StandardAxis.Depth).Visible = false;
            //chart.BoundsMode = BoundsMode.Stretch;
            SetMapSize();
            chart.Axis(StandardAxis.PrimaryX).View = new NRangeAxisView(new NRange1DD(0, 0), true, true);
            //chart.Axis(StandardAxis.PrimaryX).ScaleConfigurator = GetScaleConfigurator();
            //chart.Axis(StandardAxis.PrimaryX).PagingView.MinPageLength = 0.01f;
            chart.Axis(StandardAxis.PrimaryY).View = new NRangeAxisView(new NRange1DD(0, 0), true, true);
            //chart.Axis(StandardAxis.PrimaryY).ScaleConfigurator = GetScaleConfigurator();
            //chart.Axis(StandardAxis.PrimaryY).PagingView.MinPageLength = 0.01f;

            np = (NPointSeries)nChartMap.Series.Add(SeriesType.Point);
            np.Name = "PointName";
            //Set datapoint shape
            np.PointShape = PointShape.Cross;
            np.Size = new NLength(2, NRelativeUnit.ParentPercentage);
            np.BorderStyle.Width = new NLength(0, NGraphicsUnit.Millimeter);
            //Hide point label
            np.DataLabelStyle.Visible = false;
            //Using x axis value, otherwise the datapoint will be incorrect
            np.UseXValues = true;
        }
        
        #endregion

        #region Inherit Interface



        #endregion

        #region Method
        public void DrawPieceFlaw(List<FlawInfoAddPriority> flawPiece)
        {
            nPoint.ClearDataPoints();

            //List<FlawInfoAddPriority> subPiece = new List<FlawInfoAddPriority>();
            //foreach (FlawInfoAddPriority flaw in flawPiece)
            //{
            //    subPiece.Add(flaw);
            //}
            //MapWindowVariable.FlawPieces.Add(subPiece); //把PxP處理完的每一片儲存

            //MapWindowVariable.FlawPieces.Add(flawPiece); //把PxP處理完的每一片儲存
            if (flawPiece.Count > 0)
            {
                foreach (var f in flawPiece)
                {
                    nPoint.AddDataPoint(new NDataPoint(f.CD, f.RMD));
                }

            }
            nChart.Refresh();
            MapWindowVariable.CurrentPiece = MapWindowVariable.FlawPieces.Count;
            lbPageCurrent.Text = MapWindowVariable.FlawPieces.Count.ToString();
            lbPageTotal.Text = lbPageCurrent.Text;
            if (MapWindowVariable.FlawPieces.Count > 1)
            {
                btnPrevPiece.Enabled = true;
            }
        }
        public void RefreshPieceFlaw(List<FlawInfoAddPriority> flawPiece)
        {
            nPoint.ClearDataPoints();
            if (flawPiece.Count > 0)
            {
                foreach (var f in flawPiece)
                {
                    nPoint.AddDataPoint(new NDataPoint(f.CD, f.RMD));
                }
            }
            nChart.Refresh();

        }
        public void ClearMap()
        {
            nPoint.ClearDataPoints();
            nChart.Refresh();
        }
        public void InitGvFlawClass()
        {
            DebugTool.WriteLog("MapWindow.cs", "InitGvFlawClass");

            //IList<FlawTypeNameExtend> tmpFlawTypes = new List<FlawTypeNameExtend>();
            bsFlawType.DataSource = PxPVariable.FlawTypeName;
            gvFlawClass.DataSource = bsFlawType;
            gvFlawClass.AllowUserToAddRows = false;
           


            foreach (var column in MapWindowVariable.DoffTypeGridSetup)
            {
                gvFlawClass.Columns[column.ColumnName].SortMode = DataGridViewColumnSortMode.Automatic;
                gvFlawClass.Columns[column.ColumnName].HeaderText = column.HeaderText;
                gvFlawClass.Columns[column.ColumnName].DisplayIndex = column.Index;
                gvFlawClass.Columns[column.ColumnName].Width = column.Width;
            }
            //bsFlawType.ResetBindings(false);
            //gvFlawClass.Columns["Display"].DisplayIndex = 0 ;
            //gvFlawClass.Columns["FlawType"].DisplayIndex = 1 ;
            //gvFlawClass.Columns["FlawType"].HeaderText = "缺陷類型";
            //gvFlawClass.Columns["Name"].HeaderText = "缺陷名稱";
            //DataGridViewCheckBoxColumn cboxDisplay = new DataGridViewCheckBoxColumn();
            //cboxDisplay.Name = "Display";
            //cboxDisplay.HeaderText = "顯示";
            //gvFlawClass.Columns.Insert(0, cboxDisplay);
            //gvFlawClass.Columns.Add("JobNum", "JobNum");
            //gvFlawClass.Columns.Add("DoffNum", "DoffNum");
        }
        public void SetGvFlawClass(IList<FlawTypeNameExtend> flawTypes)
        {
            //DebugTool.WriteLog("MapWindow.cs", "SetGvFlawClass");

            bsFlawType.DataSource = flawTypes;
        }
        private NLinearScaleConfigurator GetScaleConfigurator()
        {
            NLinearScaleConfigurator linearScale = new NLinearScaleConfigurator();

            linearScale.SetPredefinedScaleStyle(PredefinedScaleStyle.Scientific);
            linearScale.MinorTickCount = 5;

            linearScale.MajorGridStyle.SetShowAtWall(ChartWallType.Back, true);
            linearScale.MajorGridStyle.SetShowAtWall(ChartWallType.Floor, true);
            linearScale.MajorGridStyle.LineStyle.Pattern = LinePattern.Dot;

            return linearScale;
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
                    FlawForm FlawFormController = new FlawForm(hitTestResult.DataPointIndex);
                    //FlawFormController.GetFlawInfo();
                    FlawFormController.Show();
                    //MessageBox.Show("Flaw point [" + hitTestResult.DataPointIndex.ToString() + "]");
                }
            }
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
        public void SetMapSize()
        {
            NCartesianChart chart = (NCartesianChart)nChart.Charts[0];
            switch (MapWindowVariable.MapProportion)
            {
                case 0: // 1:1
                    chart.Width = 400;
                    chart.Height = 400;
                    break;
                case 1: // 2:1
                    chart.Width = 400;
                    chart.Height = 200;
                    break;
                case 2: // 4:3
                    chart.Width = 400;
                    chart.Height = 300;
                    break;
                case 3: // 3:4
                    chart.Width = 300;
                    chart.Height = 400;
                    break;
                case 4: // 16:9
                    chart.Width = 608;
                    chart.Height = 342;
                    break;
                default:
                    chart.Width = 400;
                    chart.Height = 400;
                    break;
            }
            nChart.Refresh();
        }
        public void SetMapAxis()
        {
            NCartesianChart chart = (NCartesianChart)nChart.Charts[0];
            chart.Axis(StandardAxis.PrimaryX).View = new NRangeAxisView(new NRange1DD(0, PxPVariable.PxPInfo.Width), true, true);
            chart.Axis(StandardAxis.PrimaryX).ScaleConfigurator = GetScaleConfigurator();
            chart.Axis(StandardAxis.PrimaryX).PagingView.MinPageLength = 0.01f;
            chart.Axis(StandardAxis.PrimaryY).View = new NRangeAxisView(new NRange1DD(0, PxPVariable.PxPInfo.Height), true, true);
            chart.Axis(StandardAxis.PrimaryY).ScaleConfigurator = GetScaleConfigurator();
            chart.Axis(StandardAxis.PrimaryY).PagingView.MinPageLength = 0.01f;
            nChart.Refresh();
        }
        public void ResetBinding()
        { 
            
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
            }
        }

        private void nChart_MouseClick(object sender, MouseEventArgs e)
        {
            MapWindowThreadStatus.UpdateChange = true;
            PxPTab.MapThreadEvent.Set();
        }

        private void btnPrevPiece_Click(object sender, EventArgs e)
        {
            if (PxPThreadStatus.IsOnOnline)
            {
                //Set WebInspector Offline
                MapWindowThreadStatus.UpdateChange = true;
                PxPTab.MapThreadEvent.Set();
                PxPVariable.FreezPiece = MapWindowVariable.CurrentPiece;
                lbPageTotal.Text = PxPVariable.FreezPiece.ToString();
            }

            MapWindowVariable.CurrentPiece--;

            if ((MapWindowVariable.CurrentPiece == 1) || (MapWindowVariable.CurrentPiece == PxPVariable.FreezPiece - PxPVariable.PieceLimit))
                btnPrevPiece.Enabled = false;
            btnNextPiece.Enabled = true;

            lbPageCurrent.Text = MapWindowVariable.CurrentPiece.ToString();
            RefreshPieceFlaw(MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1]);
        }

        private void btnNextPiece_Click(object sender, EventArgs e)
        {
            if (PxPThreadStatus.IsOnOnline)
            {
                //Set WebInspector Offline
                MapWindowThreadStatus.UpdateChange = true;
                PxPTab.MapThreadEvent.Set();
                PxPVariable.FreezPiece = MapWindowVariable.CurrentPiece;
                lbPageTotal.Text = PxPVariable.FreezPiece.ToString();
            }

            MapWindowVariable.CurrentPiece++;

            if (MapWindowVariable.CurrentPiece == PxPVariable.FreezPiece)
                btnNextPiece.Enabled = false;
            btnPrevPiece.Enabled = true;

            lbPageCurrent.Text = MapWindowVariable.CurrentPiece.ToString();
            RefreshPieceFlaw(MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1]);
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
        
        #endregion

        private void gvFlawClass_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //int[] rgb = {
            //         Convert.ToInt32(gvFlawClass.Rows[e.RowIndex].Cells["Color"].Value.ToString().Substring(1,2)),
            //         Convert.ToInt32(gvFlawClass.Rows[e.RowIndex].Cells["Color"].Value.ToString().Substring(3,2)),
            //         Convert.ToInt32(gvFlawClass.Rows[e.RowIndex].Cells["Color"].Value.ToString().Substring(5,2)),
            //};

           
            //string[] rgb = Regex.Split(gvFlawClass.Rows[e.RowIndex].Cells["Color"].Value.ToString(), @"(\d{2})");
          

            if (e.ColumnIndex == 5)
                e.CellStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml(gvFlawClass.Rows[e.RowIndex].Cells["Color"].Value.ToString());
            if (e.ColumnIndex == 4)
                e.CellStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml(e.Value.ToString());

        }

       

      

        

        

       

       


    }
}
