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
            DebugTool.WriteLog("PxPTab.cs", "MapWindow Contructor");
            InitializeComponent();
            SystemVariable.LoadConfig();
            InitNChart(ref nChart, out nChartMap);
            
            SetFilterRadioButtons();
            btnPrevPiece.Enabled = false;
            btnNextPiece.Enabled = false;
        }
        ~MapWindow()
        {
            
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
        
        #endregion

        #region Inherit Interface



        #endregion

        #region Method
        public void DrawPieceFlaw(List<FlawInfoAddPriority> flawPiece, bool drawFlag)
        {
            nChartMap.Series.Clear();

           
            if (flawPiece.Count > 0)
            {
                foreach (var f in flawPiece)
                {
                    NPointSeries point = (NPointSeries)nChartMap.Series.Add(SeriesType.Point);
                    point.Name = f.FlawID.ToString();
                    point.PointShape = PointShape.Cross;
                    point.Size = new NLength(3, NRelativeUnit.ParentPercentage);
                    point.BorderStyle.Width = new NLength(0, NGraphicsUnit.Millimeter);
                    point.DataLabelStyle.Visible = false;
                    point.FillStyle = new NColorFillStyle(Color.Blue);
                    point.UseXValues = true;
                    point.AddDataPoint(new NDataPoint(f.CD, f.RMD));
                }

            }
            nChart.Refresh();
            if (drawFlag)
            {
                MapWindowVariable.CurrentPiece = MapWindowVariable.FlawPieces.Count;
                lbPageCurrent.Text = MapWindowVariable.FlawPieces.Count.ToString();
                lbPageTotal.Text = lbPageCurrent.Text;
                if (MapWindowVariable.FlawPieces.Count > 1)
                {
                    btnPrevPiece.Enabled = true;
                }
            }
        }
        public void ClearMap()
        {
            nChartMap.Series.Clear();
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

            linearScale.MajorGridStyle.SetShowAtWall(ChartWallType.Back, MapWindowVariable.ShowGridSet);
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
                    FlawForm FlawFormController = new FlawForm(
                        Convert.ToInt32(series.Name) - MapWindowVariable.FlawPieces[PxPVariable.FreezPiece - 1][0].FlawID
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
                    point.FillStyle = new NColorFillStyle(Color.Blue);
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
            nChart.Refresh();
        }
        public void SetMapAxis()
        {
            nChartMap.Axis(StandardAxis.PrimaryX).View = new NRangeAxisView(new NRange1DD(0, PxPVariable.PxPInfo.Width), true, true);
            nChartMap.Axis(StandardAxis.PrimaryX).ScaleConfigurator = GetScaleConfigurator();
            nChartMap.Axis(StandardAxis.PrimaryX).PagingView.MinPageLength = 0.01f;
            nChartMap.Axis(StandardAxis.PrimaryY).View = new NRangeAxisView(new NRange1DD(0, PxPVariable.PxPInfo.Height), true, true);
            nChartMap.Axis(StandardAxis.PrimaryY).ScaleConfigurator = GetScaleConfigurator();
            nChartMap.Axis(StandardAxis.PrimaryY).PagingView.MinPageLength = 0.01f;
            nChart.Refresh();
        }
        public void SetMapInfoLabel()
        {
            lbDoffValue.Text = PxPVariable.DoffNum.ToString();
            lbFailValue.Text = PxPVariable.FailNum.ToString();
            lbPassValue.Text = PxPVariable.PassNum.ToString();
            lbYieldValue.Text = (Math.Round((double)PxPVariable.PassNum / (double)(PxPVariable.PassNum + PxPVariable.FailNum), 4) * 100).ToString() + "%";
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
            DrawPieceFlaw(MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1], false);
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
            DrawPieceFlaw(MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1], false);
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

            if (e.ColumnIndex == 5)
                e.CellStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml(gvFlawClass.Rows[e.RowIndex].Cells["Color"].Value.ToString());
            if (e.ColumnIndex == 4)
                e.CellStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml(e.Value.ToString());

        }

       

      

        

        

       

       


    }
}
