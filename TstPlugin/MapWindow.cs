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
            InitNChart(ref nChart, out nChartMap, out nPoint);
            InitGvFlawClass();
            
        }
        #endregion

        #region Refactoring
        void InitNChart(ref NChartControl ncc,out NChart nc,out NPointSeries np)
        {
            ncc.Settings.RenderDevice = RenderDevice.GDI;
            NLabel nchartHeader = nChart.Labels.AddHeader("缺陷圖");
            nc = ncc.Charts[0];
            nc.Axis(StandardAxis.Depth).Visible = false;
            np = (NPointSeries)nChartMap.Series.Add(SeriesType.Point);
            np.Name = "PointName";
            np.BorderStyle.Width = new NLength(0, NGraphicsUnit.Millimeter);
            np.Legend.Mode = SeriesLegendMode.DataPoints;
            np.PointShape = PointShape.Cross;
            np.Size = new NLength(2,NRelativeUnit.ParentPercentage);
        }
        
        #endregion

        #region Inherit Interface



        #endregion

        #region Method
        public void DrawPieceFlaw(List<IFlawInfo> flawPiece)
        {
            nPoint.ClearDataPoints();
            MapWindowVariable.FlawPieces.Add(flawPiece); //把PxP處理完的每一片儲存
            if (flawPiece.Count > 0)
            {
                foreach (var f in flawPiece)
                {
                    nPoint.AddDataPoint(new NDataPoint(f.MD, f.CD));
                }
            }
            nChart.Refresh();
        }

        public void InitGvFlawClass()
        {
            DebugTool.WriteLog("MapWindow.cs", "InitGvFlawClass");

            IList<IFlawTypeName> tmpFlawTypes = new List<IFlawTypeName>();
            gvFlawClass.DataSource = bsFlawType;
            bsFlawType.DataSource = tmpFlawTypes; 
            gvFlawClass.Columns["FlawType"].HeaderText = "缺陷類型";
            gvFlawClass.Columns["Name"].HeaderText = "缺陷名稱";
            DataGridViewCheckBoxColumn cboxDisplay = new DataGridViewCheckBoxColumn();
            cboxDisplay.Name = "Display";
            cboxDisplay.HeaderText = "顯示";
            gvFlawClass.Columns.Insert(0, cboxDisplay);
            gvFlawClass.Columns.Add("JobNum","工單數量");
            gvFlawClass.Columns.Add("DoffNum","缺陷數量");
        }
        public void SetGvFlawClass(IList<IFlawTypeName> flawTypes)
        {
            DebugTool.WriteLog("MapWindow.cs", "SetGvFlawClass");

            bsFlawType.DataSource = flawTypes;
        }
        #endregion

        
    }
}
