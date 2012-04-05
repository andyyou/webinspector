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
            InitGvFlawClass();
            SetFilterRadioButtons();
            
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
            chart.BoundsMode = BoundsMode.Stretch;
            chart.Axis(StandardAxis.PrimaryX).View = new NRangeAxisView(new NRange1DD(0, 0), true, false);
            chart.Axis(StandardAxis.PrimaryX).ScaleConfigurator = GetScaleConfigurator();
            chart.Axis(StandardAxis.PrimaryX).PagingView.MinPageLength = 0.01f;
            chart.Axis(StandardAxis.PrimaryY).View = new NRangeAxisView(new NRange1DD(0, 0), true, false);
            chart.Axis(StandardAxis.PrimaryY).ScaleConfigurator = GetScaleConfigurator();
            chart.Axis(StandardAxis.PrimaryY).PagingView.MinPageLength = 0.01f;

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
            
            MapWindowVariable.FlawPieces.Add(flawPiece); //把PxP處理完的每一片儲存
            if (flawPiece.Count > 0)
            {
                foreach (var f in flawPiece)
                {
                    nPoint.AddDataPoint(new NDataPoint(f.CD, f.RMD));
                }
            }
            nChart.Refresh();

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

            IList<IFlawTypeName> tmpFlawTypes = new List<IFlawTypeName>();
            gvFlawClass.DataSource = bsFlawType;
            bsFlawType.DataSource = tmpFlawTypes; 

            //gvFlawClass.Columns["FlawType"].HeaderText = "缺陷類型";
            //gvFlawClass.Columns["Name"].HeaderText = "缺陷名稱";
            DataGridViewCheckBoxColumn cboxDisplay = new DataGridViewCheckBoxColumn();
            cboxDisplay.Name = "Display";
            //cboxDisplay.HeaderText = "顯示";
            gvFlawClass.Columns.Insert(0, cboxDisplay);
            gvFlawClass.Columns.Add("JobNum", "JobNum");
            gvFlawClass.Columns.Add("DoffNum", "DoffNum");
        }
        public void SetGvFlawClass(IList<IFlawTypeName> flawTypes)
        {
            DebugTool.WriteLog("MapWindow.cs", "SetGvFlawClass");

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
        #endregion

        #region Action Events
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
        #endregion

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


    }
}
