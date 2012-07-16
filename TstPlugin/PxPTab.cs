using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Win32;
using Nevron.Chart;
using Nevron.GraphicsCore;
using WRPlugIn;

namespace PxP
{
    [Export(typeof(IWRPlugIn))]
    public partial class PxPTab : UserControl, IWRPlugIn, IWRMapWindow, IOnFlaws, IOnEvents, IOnCut, IOnJobLoaded,
                                  IOnJobStarted, IOnLanguageChanged, IOnJobStopped, IOnWebDBConnected, IOnSync,
                                  IOnGlassEdges, IOnOnline, IOnUserTermsChanged, IOnDoffResult, IOnPxPConfig,
                                  IOnClassifyFlaw, IOnCognitiveScience, IOnRollResult, IOnOpenHistory, IWRFireEvent,
                                  IOnUnitsChanged
    {
        #region 註解流程說明
        /*
         * 1. 所有物件與變數資訊都由 PxPTab 控制, MapWindow 需要時, MapWindow 提供 Method 由 PxPTab 將值傳入.
         * 
         * */
        #endregion

        /// <summary>
        /// Error Log Object
        /// </summary>
        [Import(typeof(IWRMessageLog))]
        IWRMessageLog MsgLog;
        [Import(typeof(IWRJob))]
        IWRJob Job;

        #region Local Variables

        public PictureBox[] pbFlaws;
        public int ImgPlaceHolderWidth;
        public int ImgPlaceHolderHeight;  // 右下角 TableLayout 內置放圖片容器寬高
        bool SortSwitch = false;

        #endregion

        #region Initialize Thread

        #region 註解
        /*
         *  PxPThread : 執行外掛程式本身接續在 ON EVENT 之後的動作當 UI Thread 停止時的 Background Thread
         *  MapThread : 判斷在暫停時部分需要中斷的程序或需要重繪的物件
         */
        #endregion

        private Thread PxPThread = null;
        private AutoResetEvent PxPThreadEvent = new AutoResetEvent(false);
        private Thread MapThread = null;
        public static AutoResetEvent MapThreadEvent = new AutoResetEvent(false);
        private IAsyncResult CutResult;
        
        #endregion

        #region Constructor

        // 建構子
        public PxPTab()
        {
            InitializeComponent();
            
            // 執行緒啟動
            MapThread = new Thread(new ThreadStart(RefreshCheck));
            MapThread.Start();
            PxPThread = new Thread(new ThreadStart(WorkerThread));
            PxPThread.Start();

            SystemVariable.LoadSystemConfig();
            SystemVariable.LoadConfig();
            SystemVariable.LoadGradeConfig();
            
            InitTableLayout(tlpDoffGrid);
            DefineDataGridView(gvFlaw);
        }

        // 解構子, 關閉時儲存一些調整過的設定
        ~PxPTab()
        {
            try
            {
                // Save columns sorted
                string FolderPath = Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\..\\Parameter Files\\CPxP\\sys_conf\\";
                string FullFilePath = FolderPath + "sys.xml";
                XDocument XDoc = XDocument.Load(FullFilePath);
                IEnumerable<XElement> Columns = XDoc.Element("SystemConfig").Element("DoffGrid").Elements("Column");
                for (int i = 0; i < gvFlaw.Columns.Count; i++)
                {
                    foreach (XElement column in Columns)
                    {
                        if (column.Attribute("Name").Value.ToString() == gvFlaw.Columns[i].Name.ToString())
                        {
                            column.SetElementValue("Index", gvFlaw.Columns[i].DisplayIndex.ToString());
                            column.SetElementValue("Size", gvFlaw.Columns[i].Width);
                        }
                    }
                }
                // Order by which column
                XElement OrderByColumn = XDoc.Element("SystemConfig").Element("DoffGrid").Element("OrderBy");
                OrderByColumn.Value = PxPVariable.FlawGridViewOrderColumn;
                XDoc.Save(FullFilePath);

                // MapConf : Save MapWindow Radio Button index that is checked
                XDocument XDocConf = SystemVariable.GetConfig();
                XElement ShowFlag = XDocConf.Element("Config").Element("MapVariable").Element("ShowFlag");
                ShowFlag.Value = MapWindowVariable.ShowFlag.ToString();
                string path = Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\..\\Parameter Files\\CPxP\\conf\\";
                string FullConfFilePath = string.Format("{0}{1}", path, SystemVariable.ConfigFileName);
                XDocConf.Save(FullConfFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Close Program Error");
            }

            PxPThreadStatus.IsOnShutdown = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region Refactoring

        // 定義右上角DataGridView
        void DefineDataGridView(DataGridView Dgv)
        {
            // 全部欄位都有實作 不要的從最下面Visible關閉
            bsFlaw.DataSource = MapWindowVariable.FlawPiece;
            Dgv.DataSource = bsFlaw;
            Dgv.AllowUserToOrderColumns = true;

            foreach (DoffGridColumns column in PxPVariable.DoffGridSetup)
            {
                Dgv.Columns[column.ColumnName].SortMode = DataGridViewColumnSortMode.Automatic;
                Dgv.Columns[column.ColumnName].HeaderText = column.HeaderText;
                Dgv.Columns[column.ColumnName].DisplayIndex = column.Index;
                Dgv.Columns[column.ColumnName].Width = column.Width;
            }
            // Hide columns
            Dgv.Columns["Images"].Visible = false;
            Dgv.Columns["LeftEdge"].Visible = false;
            Dgv.Columns["RightEdge"].Visible = false;
            Dgv.Columns["FlawType"].Visible = false;
            //Dgv.Columns["RMD"].Visible = false;
            //Dgv.Columns["RCD"].Visible = false;
            Dgv.Columns["ORMD"].Visible = false;
            Dgv.Columns["ORCD"].Visible = false;
            Dgv.Columns["OArea"].Visible = false;
            Dgv.Columns["OCD"].Visible = false;
            Dgv.Columns["OLength"].Visible = false;
            Dgv.Columns["OMD"].Visible = false;
            Dgv.Columns["OWidth"].Visible = false;
            Dgv.Columns["SubPieceName"].Visible = false;
            Dgv.Columns["PointScore"].Visible = false;
        }       

        // 更新頁面, 該換圖或 Map 調整, 語系變更, 全域變數變更時更新物件資料
        public void PageRefresh()
        {
            tlpDoffGrid.Controls.Clear();
            InitTableLayout(tlpDoffGrid);  // 重置 TableLayout
            DefineDataGridView(gvFlaw);    // 重繪右上角 DataGridView
        }

        // 右邊 DataGridView 更新, 連動 DataSource 吃 gvFlaw 的Controls
        public void TableLayoutRefresh()
        {
            this.tlpDoffGrid.Refresh();
           
            foreach (DataGridViewRow r in gvFlaw.Rows)
            {
                if (PxPVariable.ChooseFlawID == Convert.ToInt32(r.Cells["FlawID"].Value))
                {
                    r.Selected = true;
                    gvFlaw.FirstDisplayedScrollingRowIndex = r.Index;
                }
            }
        }

        // 當變換 Piece 時單純只要重畫 TableLayout
        public void TableLayoutChangePiece()
        {
            bsFlaw.DataSource = MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1];
            DrawTablePictures(MapWindowVariable.FlawPieces, MapWindowVariable.CurrentPiece, 1);
        }

        // 設定初始化 TableLayoutPanel
        void InitTableLayout(TableLayoutPanel Tlp)
        {
            Tlp.ColumnStyles.Clear();
            Tlp.RowCount = PxPVariable.ImgRowsSet;
            Tlp.ColumnCount = PxPVariable.ImgColsSet;

            ImgPlaceHolderHeight = Tlp.Height / Tlp.RowCount;
            ImgPlaceHolderWidth = Tlp.Width / Tlp.ColumnCount;

            for (int i = 0; i < PxPVariable.ImgRowsSet; i++)
            {
                Tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            }

            for (int i = 0; i < PxPVariable.ImgColsSet; i++)
            {
                Tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            }
        }

        // 繪製 TableLayoutPanel 將圖片置入 Control 包含計算頁面
        public void DrawTablePictures(List<List<FlawInfoAddPriority>> FlawPieces, int PieceID, int PageNum)
        {
            PxPVariable.PageCurrent = (PageNum < 1) ? 1 : PageNum;
            PxPVariable.PageTotal = gvFlaw.Rows.Count % PxPVariable.PageSize == 0 ?
                                       gvFlaw.Rows.Count / PxPVariable.PageSize :
                                       gvFlaw.Rows.Count / PxPVariable.PageSize + 1;
            
            if (PxPVariable.PageTotal < 1)
            {
                lbPageCurrent.Text = "--";
                lbPageTotal.Text = "--";
            }
            else
            {
                lbPageCurrent.Text = PxPVariable.PageCurrent.ToString();
                lbPageTotal.Text = PxPVariable.PageTotal.ToString();
            }

            // Deal with button enable status
            if (PxPVariable.PageCurrent < PxPVariable.PageTotal && PxPVariable.PageCurrent > 1)
            {
                btnNextGrid.Enabled = true;
                btnPrevGrid.Enabled = true;
            }
            else if (PxPVariable.PageCurrent == 1)
            {
                btnPrevGrid.Enabled = false;
                btnNextGrid.Enabled = true;
            }
            else if (PxPVariable.PageCurrent == PxPVariable.PageTotal)
            {
                btnPrevGrid.Enabled = true;
                btnNextGrid.Enabled = false;
            }
            else
            {
                btnNextGrid.Enabled = false;
                btnPrevGrid.Enabled = false;
            }

            int FlawPointStart = (PxPVariable.PageCurrent - 1) * PxPVariable.PageSize;
            int FlawPointEnd = ((FlawPointStart + PxPVariable.PageSize) > gvFlaw.Rows.Count) ? gvFlaw.Rows.Count : (FlawPointStart + PxPVariable.PageSize);
            
            tlpDoffGrid.Controls.Clear();
            tlpDoffGrid.Refresh();
            tlpDoffGrid.Visible = false;  // For improve efficacy

            // Add controls into table layout
            if (gvFlaw.Rows.Count > 0)
            {
                for (int i = 0; i < PxPVariable.PageSize; i++)
                {
                    int flag = ((PxPVariable.PageCurrent - 1) * PxPVariable.PageSize) + i;
                    if (flag >= gvFlaw.Rows.Count) break;
                    flag = gvFlaw.Rows[flag].Index;
                    if (flag < FlawPieces[PieceID - 1].Count )
                    {
                        MessageBox.Show("FlawPieces Count: " + FlawPieces.Count.ToString());
                        MessageBox.Show("PieceID: " + PieceID.ToString());
                        MessageBox.Show("flag: " + flag.ToString());

                        SingleFlawControl sfc = new SingleFlawControl(FlawPieces[PieceID - 1][flag]);
                        sfc.Width = ImgPlaceHolderWidth;
                        sfc.Height = ImgPlaceHolderHeight;
                        sfc.Name = sfc.Tag.ToString();
                        tlpDoffGrid.Controls.Add(sfc);
                        sfc.Dock = DockStyle.Fill;
                    }
                }
            }
            tlpDoffGrid.Visible = true;
        }

        // DataGridView 排序
        public void SortGridViewByColumn(string ColumnName)
        {
            // SortSwitch = false : 沒點過
            // SortSwitch = true  : 點過
            switch (ColumnName)
            {
                case "FlawID":
                    if (PxPVariable.FlawGridViewOrderColumn == ColumnName && SortSwitch)
                    {
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f2.FlawID.CompareTo(f1.FlawID); });
                        MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1].Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f2.FlawID.CompareTo(f1.FlawID); });
                    }
                    else
                    {
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f1.FlawID.CompareTo(f2.FlawID); });
                        MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1].Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f1.FlawID.CompareTo(f2.FlawID); });
                    }
                    SortSwitch = !SortSwitch;
                    gvFlaw.Refresh();
                    break;

                case "Priority":
                    if (PxPVariable.FlawGridViewOrderColumn == ColumnName && SortSwitch)
                    {
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f2.Priority.CompareTo(f1.Priority); });
                        MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1].Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f2.Priority.CompareTo(f1.Priority); });
                    }
                    else
                    {
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f1.Priority.CompareTo(f2.Priority); });
                        MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1].Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f1.Priority.CompareTo(f2.Priority); });
                    }
                    SortSwitch = !SortSwitch;
                    gvFlaw.Refresh();
                    break;

                case "FlawClass":
                    if (PxPVariable.FlawGridViewOrderColumn == ColumnName && SortSwitch)
                    {
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f2.FlawClass.CompareTo(f1.FlawClass); });
                        MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1].Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f2.FlawClass.CompareTo(f1.FlawClass); });
                    }
                    else
                    {
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f1.FlawClass.CompareTo(f2.FlawClass); });
                        MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1].Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f1.FlawClass.CompareTo(f2.FlawClass); });
                    }
                    SortSwitch = !SortSwitch;
                    gvFlaw.Refresh();
                    break;

                case "MD":
                    if (PxPVariable.FlawGridViewOrderColumn == ColumnName && SortSwitch)
                    {
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f2.MD.CompareTo(f1.MD); });
                        MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1].Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f2.MD.CompareTo(f1.MD); });
                    }
                    else
                    {
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f1.MD.CompareTo(f2.MD); });
                        MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1].Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f1.MD.CompareTo(f2.MD); });
                    }
                    SortSwitch = !SortSwitch;
                    gvFlaw.Refresh();
                    break;

                case "CD":
                    if (PxPVariable.FlawGridViewOrderColumn == ColumnName && SortSwitch)
                    {
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f2.CD.CompareTo(f1.CD); });
                        MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1].Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f2.CD.CompareTo(f1.CD); });
                    }
                    else
                    {
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f1.CD.CompareTo(f2.CD); });
                        MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1].Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f1.CD.CompareTo(f2.CD); });
                    }
                    SortSwitch = !SortSwitch;
                    gvFlaw.Refresh();
                    break;

                case "Area":
                    if (PxPVariable.FlawGridViewOrderColumn == ColumnName && SortSwitch)
                    {
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f2.Area.CompareTo(f1.Area); });
                        MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1].Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f2.Area.CompareTo(f1.Area); });
                    }
                    else
                    {
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f1.Area.CompareTo(f2.Area); });
                        MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1].Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f1.Area.CompareTo(f2.Area); });

                    }
                    SortSwitch = !SortSwitch;
                    gvFlaw.Refresh();
                    break;

                case "Width":
                    if (PxPVariable.FlawGridViewOrderColumn == ColumnName && SortSwitch)
                    {
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f2.Width.CompareTo(f1.Width); });
                        MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1].Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f2.Width.CompareTo(f1.Width); });
                    }
                    else
                    {
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f1.Width.CompareTo(f2.Width); });
                        MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1].Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f1.Width.CompareTo(f2.Width); });
                    }
                    SortSwitch = !SortSwitch;
                    gvFlaw.Refresh();
                    break;

                case "Length":
                    if (PxPVariable.FlawGridViewOrderColumn == ColumnName && SortSwitch)
                    {
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f2.Length.CompareTo(f1.Length); });
                        MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1].Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f2.Length.CompareTo(f1.Length); });
                    }
                    else
                    {
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f1.Length.CompareTo(f2.Length); });
                        MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1].Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f1.Length.CompareTo(f2.Length); });
                    }
                    SortSwitch = !SortSwitch;
                    gvFlaw.Refresh();
                    break;
            };
        }

        // Find Control
        public IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }

        // Deal with history cut
        public void OnHistoryCut(double MD)
        {
            double Convertion = Convert.ToDouble(PxPVariable.UnitsData.Tables["unit"].Rows[PxPVariable.UnitsKeys["Flaw List MD"]].ItemArray[2].ToString());
            MD = Math.Round(MD * Convertion, 2);
            MapWindowVariable.FlawPiece.Clear();
            foreach (FlawInfoAddPriority f in MapWindowVariable.Flaws)
            {
                if (f.MD <= MD && f.MD > PxPVariable.CurrentCutPosition)
                {
                    // Adjust RMD, RCD value
                    f.RMD = Math.Round(f.MD - PxPVariable.CurrentCutPosition, 2);
                    f.RCD = Math.Round(f.CD - PxPVariable.CurrentCutPosition, 2);
                    MapWindowVariable.FlawPiece.Add(f);
                }
            }

            List<FlawInfoAddPriority> subPiece = new List<FlawInfoAddPriority>();
            foreach (FlawInfoAddPriority f in MapWindowVariable.FlawPiece)
            {
                subPiece.Add(f);
            }
            subPiece = CalcFlawScore(subPiece);  // 計算個缺陷點分數及歸屬子片
            MapWindowVariable.FlawPieces.Add(subPiece);  // 把 PxP 處理完的每一片儲存
            MapWindowVariable.CurrentPiece = MapWindowVariable.FlawPieces.Count;

            // 處理分數缺點分類
            MapWindowVariable.PieceResult[PxPVariable.DoffNum] = GetPassOfPiece(MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1]);
            if (MapWindowVariable.PieceResult[PxPVariable.DoffNum])
            {
                PxPVariable.DoffNum++;
                PxPVariable.PassNum++;
            }
            else
            {
                PxPVariable.DoffNum++;
                PxPVariable.FailNum++;
            }

            DealSplitPieces(subPiece);

            //UNDONE : 由 OnHistoryCut 做最後一片的 Pass/Fail 處理
            // 最後一次 Cut 因為沒有 DoffResult 所以最後一片直接先算不合格
            if (MapWindowVariable.tmpPieceKey == MapWindowVariable.PieceResult.Count)
            {
                MapWindowVariable.PieceResult[MapWindowVariable.PieceResult.Count] = false;
                PxPVariable.DoffNum++;
                PxPVariable.FailNum++;
            }
            MapWindowVariable.tmpPieceKey = MapWindowVariable.PieceResult.Count;

            PxPVariable.CurrentCutPosition = MD;
            gvFlaw.DataSource = bsFlaw;
            bsFlaw.ResetBindings(false);
            bsFlaw.ResumeBinding();
            PxPThreadStatus.IsOnCut = true;
            PxPThreadEvent.Set();
        }

        public static Bitmap ToGrayBitmap(byte[] rawValues, int width, int height)
        {
            // Declare bitmap variable and lock memory
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);

            // Get image parameter
            int stride = bmpData.Stride;  // Width of scan line
            int offset = stride - width;  // Display width and the scan line width of the gap
            IntPtr iptr = bmpData.Scan0;  // Get bmpData start position in memory
            int scanBytes = stride * height;  // Size of the memory area

            // Convert the original display size of the byte array into an array of bytes actually stored in the memory
            int posScan = 0, posReal = 0;  // Declare two pointer, point to source and destination arrays
            byte[] pixelValues = new byte[scanBytes];  // Declare array size
            for (int x = 0; x < height; x++)
            {
                // Emulate line scanning
                for (int y = 0; y < width; y++)
                {
                    pixelValues[posScan++] = rawValues[posReal++];
                }
                posScan += offset;  //Line scan finished
            }

            // Using Marshal.Copy function copy pixelValues to BitmapData
            Marshal.Copy(pixelValues, 0, iptr, scanBytes);
            bmp.UnlockBits(bmpData);  // Unlock memory

            // Change 8 bit bitmap index table to Grayscale
            ColorPalette tempPalette;
            using (Bitmap tempBmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed))
            {
                tempPalette = tempBmp.Palette;
            }
            for (int i = 0; i < 256; i++)
            {
                tempPalette.Entries[i] = Color.FromArgb(i, i, i);
            }

            bmp.Palette = tempPalette;

            return bmp;
        }

        // Set Unit
        public void InitUnitsData()
        {
            var unitsDoc = XDocument.Load(PxPVariable.UnitsXMLPath);

            // Get Flaw index into units table
            var flawUnits = from component in unitsDoc.Element("UnitsConfig").Element("Components").Elements("Component")
                            select new { Name = component.Attribute("name").Value, Unit = component.Attribute("unit").Value };
            PxPVariable.UnitsKeys.Clear();
            foreach (var record in flawUnits)
            {
                PxPVariable.UnitsKeys.Add(record.Name, Convert.ToInt32(record.Unit));
            }

            // Get units
            PxPVariable.UnitsData = new DataSet();
            PxPVariable.UnitsData.ReadXml(PxPVariable.UnitsXMLPath);
        }

        #endregion

        #region Public Methods

        // 提供其他 Form 設定 Online
        public  void SetJobOnline()
        {
            Job.SetOnline();
            PxPThreadStatus.IsOnOnline = true;
        }

        // 提供其他 Form 設定 Offline
        public  void SetJobOffline()
        {
            Job.SetOffline();
            PxPThreadStatus.IsOnOnline = false;
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

        // 判斷 piece 是 Pass 或 Fail
        public bool GetPassOfPiece(List<FlawInfoAddPriority> list)
        {
            double score = CountPieceScore(list);

            if (score >= GradeVariable.PassOrFileScore)
                return false;
            else
                return true;
        }

        // 處理大片裡面切割的小片評分機制
        public void DealSplitPieces(List<FlawInfoAddPriority> piece)
        {
            SplitPieces tmpSplitPieces = new SplitPieces();
            tmpSplitPieces.Pieces = new List<SplitPiece>();
            foreach (RoiGrade r in GradeVariable.RoiRowsGrid)
            {
                foreach (RoiGrade c in GradeVariable.RoiColumnsGrid)
                {
                    SplitPiece tmpsp = new SplitPiece();
                    tmpsp.Name = string.Format("ROI-{0}{1}", r.Name, c.Name);
                    tmpsp.Score = 0;
                    if (GradeVariable.IsPointEnable)
                    {
                        foreach (FlawInfoAddPriority s in piece)
                        {
                            if (s.SubPieceName == string.Format("ROI-{0}{1}", r.Name, c.Name))
                            {
                                tmpsp.Score += s.PointScore;
                            }
                        }
                    }
                   
                    if (GradeVariable.IsMarkGradeEnable)
                    {
                        foreach (MarkSubPiece m in GradeVariable.MarkGradeSubPieces)
                        {
                            int tmpPrevScore = -1;
                            foreach (MarkGrade g in m.Grades)
                            {
                                if (m.Name == tmpsp.Name)
                                {
                                    if (tmpsp.Score <= g.Score && tmpsp.Score > tmpPrevScore)
                                    {
                                        tmpsp.GradeLevel = g.GradeName;
                                        break;
                                    }
                                    else
                                    {
                                        tmpsp.GradeLevel = "F";
                                        tmpPrevScore = g.Score;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        tmpsp.GradeLevel = "None";
                    }

                    tmpSplitPieces.Pieces.Add(tmpsp);
                }
            }
            GradeVariable.SplitPiecesContainer.Add(tmpSplitPieces);
        }

        // 計算個缺陷點分數及歸屬子片
        public List<FlawInfoAddPriority> CalcFlawScore(List<FlawInfoAddPriority> SubPiece)
        {
            foreach (FlawInfoAddPriority f in SubPiece)
            {
                // get point score
                // 如果沒設定的話分數取用0
                if (!GradeVariable.IsPointEnable)
                {
                    f.PointScore = 0;
                }
                else  // 如果有設定的話按照位置去給定分數
                {
                    foreach (RoiGrade r in GradeVariable.RoiRowsGrid)
                    {
                        foreach (RoiGrade c in GradeVariable.RoiColumnsGrid)
                        {
                            foreach (PointSubPiece p in GradeVariable.PointSubPieces)
                            {
                                if (p.Name == string.Format("ROI-{0}{1}", r.Name, c.Name))
                                {
                                    if (((f.RMD >= r.Start && f.RMD <= r.End) && (f.CD >= c.Start && f.CD <= c.End) && (MapWindowVariable.BottomAxe == 0)) ||
                                        ((f.CD >= r.Start && f.CD <= r.End) && (f.RMD >= c.Start && f.RMD <= c.End) && (MapWindowVariable.BottomAxe == 1)))
                                    {
                                        foreach (PointGrade g in p.Grades)
                                        {
                                            if (f.FlawType == g.ClassId)
                                            {
                                                f.PointScore = g.Score;
                                                f.SubPieceName = p.Name;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return SubPiece;
        }

        public void ProcessDoffResult(int doffNumber)
        {
            try
            {
                //MapWindowVariable.PieceResult.Add(doffNumber, pass);
                //if (pass)
                //    PxPVariable.PassNum++;
                //else
                //    PxPVariable.FailNum++;

                // 判斷 Pass/Fail 改由分數機制判斷 
                MapWindowVariable.CurrentPiece = MapWindowVariable.FlawPieces.Count;
                MapWindowVariable.PieceResult.Add(doffNumber, GetPassOfPiece(MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1]));
                if (GetPassOfPiece(MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1]))
                    PxPVariable.PassNum++;
                else
                    PxPVariable.FailNum++;

                PxPVariable.DoffNum = doffNumber;
                PxPThreadStatus.IsOnDoffResult = true;
                if (PxPThreadStatus.IsOnOnline)
                    MapWindowVariable.MapWindowController.SetMapInfoLabel();

                MapWindowVariable.MapWindowController.SetTotalScoreLabel(CountPieceScore(MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1]));
                PxPThreadEvent.Set();
            }
            catch (Exception ex)
            {
                MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.GeneralError, "OnDoffResult() Error!" + ex.Message, null, 0);
            }
        }

        #endregion

        #region Inherit Interface

        #region 流程說明
        /**
         * 本 Plugin 的重點都在相關流程事件的控制, 繼承之後左邊區塊為 IWPlugin
         * 右邊則是 IWRMapWindow
         * 其他 OnCut, OnFlaw 等目前都只有在 PxPTab 實作
         * 主要共用的變數依照功能放於 Model 資料夾內
         * 
         */
        #endregion

        #region IWRPlugIn 成員

        /*
         * IWRPlugIn 提供列舉型屬性
         *   e_CDReference
         *   e_EventID
         *   e_GlassEvents : 等級A - E 和 Fail
         *   e_Language : Chinese, English, German, Korean
         *   e_LogID : MessageLog, TCPLog
         *   e_LogVisibility
         *   e_SeverityType
         */

        // 取得目前控制項視窗代碼並傳回指標
        public void GetControlHandle(out IntPtr hndl)
        {
            hndl = Handle;
        }

        // 設定控制項顯示的X,Y位置座標及寬高
        public void SetPosition(int w, int h)
        {
            SetBounds(0, 0, w, h); // Default : w760,h747
        }

        // 主畫面透過 GetName 根據語系取得分頁名稱, 在 Plugin 內部則是透過 out 設定傳入變數
        public void GetName(e_Language lang, out string name)
        {
            switch (lang)
            {
                case e_Language.Chinese:
                    name = "片狀檢查";
                    break;
                default:
                    name = "PxP";
                    break;
            }
            if (MapWindowVariable.FlawPieces.Count > 0)
            {
                MapWindowThreadStatus.IsChangePiece = true;
                PxPTab.MapThreadEvent.Set();
            }
        }

        public void Initialize(string unitsXMLPath)
        {
            PxPVariable.UnitsXMLPath = unitsXMLPath;
            InitUnitsData();
        }

        // 卸載 Plugin
        public void Unplug()
        {
            //trigger on close the window
        }

        #endregion

        #region IWRMapWindow 成員

        public void GetMapControlHandle(out IntPtr hndl)
        {
            hndl = MapWindowVariable.MapWindowController.Handle;
        }

        public void SetMapPosition(int w, int h)
        {
            MapWindowVariable.MapWindowController.SetBounds(0, 0, w, h);
        }

        #endregion

        #region IOnFlaws 成員

        public void OnFlaws(IList<IFlawInfo> flaws)
        {
            try
            {
                // Deal with flaws extend other data
                IList<FlawInfoAddPriority> temp = new List<FlawInfoAddPriority>();
                double ConverArea = Convert.ToDouble(PxPVariable.UnitsData.Tables["unit"].Rows[PxPVariable.UnitsKeys["Flaw List Area"]].ItemArray[2].ToString());
                double ConverCD = Convert.ToDouble(PxPVariable.UnitsData.Tables["unit"].Rows[PxPVariable.UnitsKeys["Flaw List CD"]].ItemArray[2].ToString());
                double ConverLength = Convert.ToDouble(PxPVariable.UnitsData.Tables["unit"].Rows[PxPVariable.UnitsKeys["Flaw List Height"]].ItemArray[2].ToString());
                double ConverMD = Convert.ToDouble(PxPVariable.UnitsData.Tables["unit"].Rows[PxPVariable.UnitsKeys["Flaw List MD"]].ItemArray[2].ToString());
                double ConverWidth = Convert.ToDouble(PxPVariable.UnitsData.Tables["unit"].Rows[PxPVariable.UnitsKeys["Flaw List Width"]].ItemArray[2].ToString());
                foreach (IFlawInfo i in flaws)
                {
                    FlawInfoAddPriority f = new FlawInfoAddPriority();
                    f.Area = (Convert.ToDouble(i.Area) * ConverArea).ToString("0.######");
                    f.CD = Math.Round(i.CD * ConverCD, 2);
                    f.FlawClass = i.FlawClass;
                    f.FlawID = i.FlawID;
                    f.FlawType = i.FlawType;
                    f.Images = i.Images;
                    f.LeftEdge = i.LeftEdge ;
                    f.Length = i.Length * ConverLength;
                    f.MD = Math.Round(i.MD * ConverMD, 2);
                    f.RMD = Math.Round(i.MD * ConverMD - PxPVariable.CurrentCutPosition, 2);
                    f.RCD = Math.Round(i.CD * ConverCD - PxPVariable.CurrentCutPosition, 2);
                    f.RightEdge = i.RightEdge ;
                    f.Width = Math.Round(i.Width * ConverWidth, 4);

                    // Keep original value
                    f.ORCD = Math.Round(i.CD - PxPVariable.CurrentCutPosition, 6);
                    f.ORMD = Math.Round(i.MD - PxPVariable.CurrentCutPosition, 6);
                    f.OArea = i.Area.ToString("0.######");
                    f.OCD = i.CD;
                    f.OLength = i.Length;
                    f.OMD =i.MD;
                    f.OWidth =i.Width;

                    // 特別處理 Priority
                    int opv;
                    if (PxPVariable.SeverityInfo.Count > 0)
                        f.Priority = PxPVariable.SeverityInfo[0].Flaws.TryGetValue(f.FlawType, out opv) ? opv : 0;
                    else
                        f.Priority = 0;

                    // 因讀取歷史資料, 特別處理 Image
                    if (SystemVariable.IsReadHistory)
                    {
                        bool blnShowImg = false;
                        int intW = 0;
                        int intH = 0;
                        using (SqlConnection cn = new SqlConnection(SystemVariable.DBConnectString))
                        {
                            cn.Open();
                            string QueryStr = "Select iImage From dbo.Jobs T1, dbo.Flaw T2, dbo.Image T3 Where T1.klKey = T2.klJobKey AND T2.pklFlawKey = T3.klFlawKey AND T1.JobID = @JobID AND T2.lFlawId = @FlawID";
                            SqlCommand cmd = new SqlCommand(QueryStr, cn);
                            cmd.Parameters.AddWithValue("@JobID", PxPVariable.JobInfo.JobID);
                            cmd.Parameters.AddWithValue("@FlawID", i.FlawID);
                            SqlDataReader sd = cmd.ExecuteReader();
                            sd.Read();
                            byte[] images = (Byte[])sd["iImage"];

                            intW = images[0] + images[1] * 256;
                            intH = images[4] + images[5] * 256;

                            if (intW == 0 & intH == 0)
                            {
                                intW = 1;
                                intH = 1;
                                blnShowImg = false;
                            }
                            else
                            {
                                blnShowImg = true;
                            }
                            Bitmap bmpShowImg = new Bitmap(intW, intH);

                            if (blnShowImg)
                            {
                                bmpShowImg = ToGrayBitmap(images, intW, intH);
                            }

                            IImageInfo tmpImg = new ImageInfo(bmpShowImg, 0);
                            f.Images.Add(tmpImg);
                        }
                    }
                    else
                    {
                        f.Images = i.Images;
                    }
                    temp.Add(f);
                }

                MapWindowVariable.Flaws.AddRange(temp);
            }
            catch (Exception ex)
            {
                MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.GeneralError, "OnFlaws() Error!" + ex.Message, null, 0);
            }
            PxPThreadStatus.IsOnFlaws = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnEvents 成員

        public void OnEvents(IList<IEventInfo> events)
        {
            PxPThreadStatus.IsOnEvents = true;
            foreach (IEventInfo eventInfo in events)
            {
                switch ((e_EventID)eventInfo.EventType)
                {
                    case e_EventID.STOP_JOB:
                        PxPThreadStatus.IsOnOnline = false;
                        PxPThreadStatus.IsOnJobStopped = true;
                        if (MapWindowVariable.CurrentPiece > 0)
                        {
                            PxPVariable.FreezPiece = MapWindowVariable.FlawPieces.Count;
                        }
                        break;

                    case e_EventID.STOP_INSPECTION:
                        PxPThreadStatus.IsOnOnline = false;
                        if (MapWindowVariable.CurrentPiece > 0)
                        {
                            PxPVariable.FreezPiece = MapWindowVariable.FlawPieces.Count;
                            bsFlaw.DataSource = MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1];
                        }
                        break;

                    case e_EventID.START_INSPECTION:
                        PxPThreadStatus.IsOnOnline = true;
                        bsFlaw.DataSource = MapWindowVariable.FlawPiece;
                        break;

                    case e_EventID.CUT_SIGNAL:
                        if (SystemVariable.IsReadHistory)
                        {
                            OnHistoryCut(eventInfo.MD);

                            MapWindowVariable.MapWindowController.SetMapInfoLabel();
                            MapWindowVariable.MapWindowController.SetTotalScoreLabel(CountPieceScore(MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1]));
                        }
                        break;

                    //case e_EventID.DOFF_PASSED:
                    //    if (SystemVariable.IsReadHistory)
                    //    {
                    //        MapWindowVariable.PieceResult[PxPVariable.DoffNum] = true;
                    //        PxPVariable.DoffNum++;
                    //        PxPVariable.PassNum++;
                    //    }
                    //    break;

                    //case e_EventID.DOFF_FAILED:
                    //    if (SystemVariable.IsReadHistory)
                    //    {
                    //        MapWindowVariable.PieceResult[PxPVariable.DoffNum] = false;
                    //        PxPVariable.DoffNum++;
                    //        PxPVariable.FailNum++;
                    //    }
                    //    break;
                        
                    default:
                        break;                        
                }
            }
            PxPThreadEvent.Set();
        }

        #endregion
        
        #region IOnCut 成員

        public void OnCut(double md)
        {
            MapWindowVariable.FlawPiece.Clear();
            foreach (FlawInfoAddPriority f in MapWindowVariable.Flaws)
            {
                if (f.MD < PxPVariable.CurrentCutPosition + PxPVariable.PxPHeight && f.MD > PxPVariable.CurrentCutPosition)
                    MapWindowVariable.FlawPiece.Add(f);
            }
            MapWindowVariable.Flaws.Clear();

            List<FlawInfoAddPriority> subPiece = new List<FlawInfoAddPriority>();
            foreach (FlawInfoAddPriority f in MapWindowVariable.FlawPiece)
            {
                subPiece.Add(f);
            }

            subPiece = CalcFlawScore(subPiece);  // 計算個缺陷點分數及歸屬子片
            MapWindowVariable.FlawPieces.Add(subPiece);  // 把 PxP 處理完的每一片儲存

            // 先處理統計不使用 Pieces 改用 SubPiece
            foreach (FlawTypeNameExtend ft in PxPVariable.FlawTypeName)
            {
                ft.DoffNum = 0;
            }
           
            // 處理 Pass/Fail 分數並設定到 Mapwindow
            MapWindowVariable.MapWindowController.SetTotalScoreLabel(CountPieceScore(subPiece));
            DealSplitPieces(subPiece);
            
            PxPVariable.CurrentCutPosition = md * Convert.ToDouble(PxPVariable.UnitsData.Tables["unit"].Rows[PxPVariable.UnitsKeys["Flaw List MD"]].ItemArray[2].ToString()); ; //UnitTest

            //UNDONE: ProcessDoffResult
            ProcessDoffResult(MapWindowVariable.FlawPieces.Count - 1);

            if (PxPThreadStatus.IsOnOnline)
            {
                gvFlaw.DataSource = bsFlaw;
                bsFlaw.ResetBindings(false);
                bsFlaw.ResumeBinding();
                
                // Update left datagridview of flawtype
                foreach (FlawInfoAddPriority f in subPiece)
                {
                    foreach (FlawTypeNameExtend ft in PxPVariable.FlawTypeName)
                    {
                        if (ft.FlawType == f.FlawType)
                        {
                            if (ft.OfflineJobNum > 0)
                            {
                                ft.JobNum += ft.OfflineJobNum;
                                ft.OfflineJobNum = 0;
                            }
                            if (ft.OfflineDoffNum > 0)
                            {
                                ft.DoffNum += ft.OfflineDoffNum;
                                ft.OfflineDoffNum = 0;
                            }
                            ft.JobNum++;
                            ft.DoffNum++;
                        }
                    }
                }

                int count = MapWindowVariable.FlawPieces.Count;
                if (count > PxPVariable.PieceLimit)
                {
                    for (int i = 0; i < count - PxPVariable.PieceLimit; i++)
                        MapWindowVariable.FlawPieces[i] = null;
                }

                MapWindowVariable.MapWindowController.RefreshGvFlawClass();
                PxPThreadStatus.IsOnCut = true;
                PxPThreadEvent.Set();
            }
            else
            {
                // Update left datagridview of flawtype in offline
                foreach (FlawInfoAddPriority f in subPiece)
                {
                    foreach (FlawTypeNameExtend ft in PxPVariable.FlawTypeName)
                    {
                        if (ft.FlawType == f.FlawType)
                        {
                            ft.OfflineDoffNum++;
                            ft.OfflineJobNum++;
                        }
                    }
                }
            }
        }

        #endregion

        #region IOnJobLoaded 成員

        public void OnJobLoaded(IList<IFlawTypeName> flawTypes, IList<ILaneInfo> lanes, IList<ISeverityInfo> severityInfo, IJobInfo jobInfo)
        {
            #region 註解
            /*
             * 1. 使用 Model 來存取介面取得之參數.
             * 2. 啟動並回復執行緒
             */
            #endregion

            // Clear Some relative data
            MapWindowVariable.Flaws.Clear();
            MapWindowVariable.FlawPiece.Clear();
            MapWindowVariable.FlawPieces.Clear();
            GradeVariable.SplitPiecesContainer.Clear();
            MapWindowVariable.CurrentPiece = 0;
            MapWindowVariable.MapWindowController.InitLabel();
            PxPVariable.CurrentCutPosition = 0;
            // Reload Grade config useing mcs value
            SystemVariable.LoadGradeConfig();

            // 開啟歷史資料時重新計算 Pass/Fail
            PxPVariable.DoffNum = 0;
            PxPVariable.FailNum = 0;
            PxPVariable.PassNum = 0;
            foreach (FlawTypeNameExtend ft in PxPVariable.FlawTypeName)
            {
                ft.DoffNum = 0;
                ft.JobNum = 0;
            }

            // 完整處理完 tmpList 再丟到 PxPVariable.FlawTypeName
            List<FlawTypeNameExtend> tmpList = new List<FlawTypeNameExtend>();
            tmpList.AddRange(PxPVariable.FlawTypeName);  // 一開始 Conf 會載入儲存於 Config 中所有的設定到 PxPVariable.FlawTypeName
            // 把 flawTypes 沒有的項目先從 tmpList 移除
            var DifferencesConfigList = PxPVariable.FlawTypeName.Where(x => !flawTypes.Any(x1 => x1.Name == x.Name && x1.FlawType == x.FlawType));
            foreach (FlawTypeNameExtend r in DifferencesConfigList)
            {
                tmpList.Remove(r);
            }

            // 差異比對, 補上 conf 沒有但 flawTypes 有的, 相同的就跳過不處理
            var DifferencesList = flawTypes.Where(x => !tmpList.Any(x1 => x1.Name == x.Name && x1.FlawType == x.FlawType ));
            foreach (IFlawTypeName i in DifferencesList)
            {
                // Default                
                FlawTypeNameExtend tmp = new FlawTypeNameExtend();
                tmp.FlawType = i.FlawType;
                tmp.Name = i.Name;
                tmp.Display = true;
                foreach (FlawLegend f in PxPVariable.FlawLegend)
                {
                    if (f.Name.Trim() == tmp.Name.Trim())
                    {
                        tmp.Color = String.Format("#{0:X2}{1:X2}{2:X2}", ColorTranslator.FromWin32((int)f.Color).R,
                                                        ColorTranslator.FromWin32((int)f.Color).G,
                                                        ColorTranslator.FromWin32((int)f.Color).B);
                        break;
                    }
                    else
                    {
                        tmp.Color = "#000000";
                    }
                }
                
                tmp.Shape = Shape.Cone.ToGraphic();
                tmpList.Add(tmp);
            }

            PxPVariable.FlawTypeName.Clear();
            PxPVariable.FlawTypeName.AddRange(tmpList);
            
            PxPVariable.FlawTypeName.Sort(delegate(FlawTypeNameExtend f1, FlawTypeNameExtend f2) { return f1.FlawType.CompareTo(f2.FlawType); });

            // Copy a temp for setup gridview display. Not save to global variable just for display.
            PxPVariable.TmpFlawTypeNameForSetup.Clear();
            foreach (FlawTypeNameExtend ft in PxPVariable.FlawTypeName)
            {
                FlawTypeNameExtend tmp = new FlawTypeNameExtend();
                tmp.FlawType = ft.FlawType;
                tmp.Name = ft.Name;
                tmp.Display = ft.Display;
                tmp.Count = ft.Count;
                tmp.Color = ft.Color;
                tmp.Shape = ft.Shape;
                PxPVariable.TmpFlawTypeNameForSetup.Add(tmp);
            }
            PxPVariable.JobInfo = jobInfo;
            PxPVariable.SeverityInfo = severityInfo;
            PxPThreadStatus.IsOnJobLoaded = true;
            
            // Disable MapSetup Button
            MapWindowVariable.MapWindowController.btnMapSetup.Enabled = true;
            MapWindowVariable.MapWindowController.btnGradeSetting.Enabled = true;
            MapWindowVariable.MapWindowController.SetJobInfo();
            MapWindowVariable.MapWindowController.SetMapAxis();
            MapWindowVariable.MapWindowController.bsFlawType.ResetBindings(false);
            MapWindowVariable.MapWindowController.InitSubPiece();
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnJobStarted 成員

        public void OnJobStarted(int jobKey)
        {
            PxPVariable.JobKey = jobKey;
            PxPThreadStatus.IsOnJobStarted = true;
            MapWindowVariable.PieceResult.Clear();
           
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnLanguageChanged 成員

        public void OnLanguageChanged(e_Language language)
        {
            #region 註解
            /*
             * 需要變更語系的變數或屬性列表
             * 1. 右上方缺陷點DataGridView Columns
             * 2. 左上方Lables, Buttons
             * 3. 左下方缺陷點分類DataGridView Columns
             * 4. 
             */
            #endregion

            SystemVariable.Language = language;

            // 載入語系並設定需變更的值
            try
            {
                XDocument XDocLang = SystemVariable.GetLangXDoc(SystemVariable.Language);
                IEnumerable<XElement> PxPDoffGridLangItem = XDocLang.Element("Language").Element("PxPTab").Element("DoffGrid").Elements("Column");
                foreach (DoffGridColumns column in PxPVariable.DoffGridSetup)
                {
                    foreach (XElement i in PxPDoffGridLangItem)
                    {
                        if (column.ColumnName == i.Attribute("Name").Value)
                        {
                            column.HeaderText = i.Value;
                        }
                    }
                }

                IEnumerable<XElement> MapFlawClassGridColumns = XDocLang.Element("Language").Element("MapWindow").Element("DoffSettingGrid").Elements("Column");
                foreach (DoffGridColumns column in MapWindowVariable.DoffTypeGridSetup)
                {
                    foreach (XElement i in MapFlawClassGridColumns)
                    {
                        if (column.ColumnName == i.Attribute("Name").Value)
                        {
                            column.HeaderText = i.Value.ToString();
                        }
                    }
                }

                IEnumerable<XElement> MapWindowButtons = XDocLang.Element("Language").Element("MapWindow").Element("Object").Elements("Button");
                var btns = GetAll(MapWindowVariable.MapWindowController, typeof(Button));
                foreach (Control btn in btns)
                {
                    foreach (XElement i in MapWindowButtons)
                    {
                        if (btn.Name == i.Attribute("Name").Value)
                        {
                            btn.Text = i.Value.ToString();
                        }
                    }
                }

                IEnumerable<XElement> MapWindowRadioButtons = XDocLang.Element("Language").Element("MapWindow").Element("Object").Elements("RadioButton");
                var rbtns = GetAll(MapWindowVariable.MapWindowController, typeof(RadioButton));
                foreach (Control btn in rbtns)
                {
                    foreach (XElement i in MapWindowRadioButtons)
                    {
                        if (btn.Name == i.Attribute("Name").Value)
                        {
                            btn.Text = i.Value.ToString();
                        }
                    }
                }
                
                // 更新所有須改變語系的物件
                PageRefresh();
                MapWindowVariable.MapWindowController.InitGvFlawClass();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Language loading error");
            }
        }

        #endregion
        
        #region IOnJobStopped 成員

        public void OnJobStopped(double md)
        {
            MapWindowVariable.MapWindowController.btnMapSetup.Enabled = true;
            MapWindowVariable.MapWindowController.btnGradeSetting.Enabled = true;
            PxPThreadStatus.IsOnJobStopped = true;
            if (SystemVariable.IsReadHistory)
            {
                OnHistoryCut(md);
                PxPVariable.FreezPiece = MapWindowVariable.FlawPieces.Count();
                // 統計 FlawPiece 裡面的 FlawType 分類統計
                for (int i = 0; i < MapWindowVariable.FlawPieces.Count(); i++)
                {
                    List<FlawInfoAddPriority> fs = MapWindowVariable.FlawPieces[i];
                    foreach (FlawInfoAddPriority f in fs)
                    {
                        if (i < MapWindowVariable.FlawPieces.Count() - 1)
                        {
                            foreach (FlawTypeNameExtend ft in PxPVariable.FlawTypeName)
                            {
                                if (ft.FlawType == f.FlawType)
                                {
                                    ft.JobNum++;
                                }
                            }
                        }
                        else
                        {
                            foreach (FlawTypeNameExtend ft in PxPVariable.FlawTypeName)
                            {
                                if (ft.FlawType == f.FlawType)
                                {
                                    ft.JobNum++;
                                    ft.DoffNum++;
                                }
                            }
                        }
                    }
                }

                MapWindowVariable.MapWindowController.RefreshGvFlawClass();
                //UNDONE : OnJobStopped 不處理最後一片 Pass/Fail
                // 最後一次 Cut 因為沒有 DoffResult 所以最後一片直接先算不合格
                //if (PxPVariable.CurrentCutPosition < (Math.Round(md, 2) * Convert.ToDouble(PxPVariable.UnitsData.Tables["unit"].Rows[PxPVariable.UnitsKeys["Flaw List MD"]].ItemArray[2].ToString())))
                //{
                //    MapWindowVariable.PieceResult[PxPVariable.DoffNum] = false;
                //    PxPVariable.DoffNum++;
                //    PxPVariable.FailNum++;
                //}
                MapWindowVariable.MapWindowController.SetMapInfoLabel();
                MapWindowVariable.MapWindowController.SetTotalScoreLabel(CountPieceScore(MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1]));
            }
            SystemVariable.IsReadHistory = false;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnWebDBConnected 成員

        public void OnWebDBConnected(IWebDBConnectionInfo info)
        {
            SystemVariable.DBConnectInfo = info;
            SystemVariable.DBConnectString = String.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3};",info.ServerName,info.DatabaseName,info.UserName,info.Password);
            
            PxPThreadStatus.IsOnWebDBConnected = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnSync 成員

        public void OnSync(double md)
        {
            PxPThreadStatus.IsOnSync = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnGlassEdges 成員

        public void OnGlassEdges(double md, double le1, double le2, double le3, double leftROI, double rightROI, double re3, double re2, double re1)
        {
            PxPThreadStatus.IsOnGlassEdges = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnOnline 成員

        public void OnOnline(bool isOnline)
        {
            PxPThreadStatus.IsOnOnline = isOnline;
            
            if (isOnline)
            {
                MapWindowVariable.MapWindowController.btnMapSetup.Enabled = false;
                MapWindowVariable.MapWindowController.btnGradeSetting.Enabled = false;
                bsFlaw.DataSource = MapWindowVariable.FlawPiece;
                gvFlaw.Rows.Clear();
                tlpDoffGrid.Controls.Clear();
                MapWindowVariable.MapWindowController.ClearMap();
                MapWindowVariable.MapWindowController.ResetGvFlawClassDoffNum();

                btnNextGrid.Enabled = false;
                btnPrevGrid.Enabled = false;
                lbPageTotal.Text = "--";
                lbPageCurrent.Text = "--";
            }
            else
            {
                if (MapWindowVariable.CurrentPiece > 0)
                {
                    MapWindowVariable.MapWindowController.SetPieceTotalLabel();
                }
            }
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnUserTermsChanged 成員

        public void OnUserTermsChanged(IUserTerms terms)
        {
            MapWindowVariable.MapWindowController.SetUserTermLabel(terms);
            PxPThreadStatus.IsOnUserTermsChanged = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnDoffResult 成員

        public void OnDoffResult(double md, int doffNumber, bool pass)
        {
            //try
            //{
            //    //MapWindowVariable.PieceResult.Add(doffNumber, pass);
            //    //if (pass)
            //    //    PxPVariable.PassNum++;
            //    //else
            //    //    PxPVariable.FailNum++;

            //    // 判斷 Pass/Fail 改由分數機制判斷 
            //    MapWindowVariable.CurrentPiece = MapWindowVariable.FlawPieces.Count;
            //    MapWindowVariable.PieceResult.Add(doffNumber, GetPassOfPiece(MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1]));
            //    if (GetPassOfPiece(MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1]))
            //        PxPVariable.PassNum++;
            //    else
            //        PxPVariable.FailNum++;

            //    PxPVariable.DoffNum = doffNumber;
            //    PxPThreadStatus.IsOnDoffResult = true;
            //    if (PxPThreadStatus.IsOnOnline)
            //    MapWindowVariable.MapWindowController.SetMapInfoLabel();

            //    MapWindowVariable.MapWindowController.SetTotalScoreLabel(CountPieceScore(MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1]));
            //    PxPThreadEvent.Set();
            //}
            //catch (Exception ex)
            //{
            //    MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.GeneralError, "OnDoffResult() Error!" + ex.Message, null, 0);
            //}
        }

        #endregion

        #region IOnPxPConfig 成員

        public void OnPxPConfig(IPxPInfo info)
        {
            PxPVariable.PxPInfo = info;
            PxPVariable.PxPWidth = PxPVariable.PxPInfo.Width * Convert.ToDouble(PxPVariable.UnitsData.Tables["unit"].Rows[PxPVariable.UnitsKeys["Flaw Map CD"]].ItemArray[2].ToString());
            PxPVariable.PxPHeight = PxPVariable.PxPInfo.Height * Convert.ToDouble(PxPVariable.UnitsData.Tables["unit"].Rows[PxPVariable.UnitsKeys["Flaw Map MD"]].ItemArray[2].ToString());

            PxPThreadStatus.IsOnPxPConfig = true;
            PxPThreadEvent.Set();
        }
        
        #endregion

        #region IOnRollResult 成員

        public void OnRollResult(double cd, double md, int doffNumber, int laneNumber, bool pass)
        {
            PxPThreadStatus.IsOnRollResult = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnOpenHistory 成員

        public void OnOpenHistory(double startMD, double stopMD)
        {
            MapWindowVariable.Flaws.Clear();
            // 開啟歷史資料時重新計算 Pass/Fail
            PxPVariable.DoffNum = 0;
            PxPVariable.FailNum = 0;
            PxPVariable.PassNum = 0;
            SystemVariable.IsReadHistory = true;
            PxPThreadStatus.IsOnOpenHistory = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IWRFireEvent 成員

        public void FireEvent(int eventID, double cd, double md)
        {
            PxPThreadStatus.IsFireEvent = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnClassifyFlaw 成員

        public void OnClassifyFlaw(ref WRPlugIn.IFlawInfo flaw, ref bool deleteFlaw)
        {
            PxPThreadStatus.IsOnClassifyFlaw = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnUnitsChanged 成員

        public void OnUnitsChanged()
        {
            InitUnitsData();
            
            // Change unit already into DataSource : MapWindowVariable.FlawPieces
            double ConverArea = Convert.ToDouble(PxPVariable.UnitsData.Tables["unit"].Rows[PxPVariable.UnitsKeys["Flaw List Area"]].ItemArray[2].ToString());
            double ConverCD = Convert.ToDouble(PxPVariable.UnitsData.Tables["unit"].Rows[PxPVariable.UnitsKeys["Flaw List CD"]].ItemArray[2].ToString());
            double ConverLength = Convert.ToDouble(PxPVariable.UnitsData.Tables["unit"].Rows[PxPVariable.UnitsKeys["Flaw List Height"]].ItemArray[2].ToString());
            double ConverMD = Convert.ToDouble(PxPVariable.UnitsData.Tables["unit"].Rows[PxPVariable.UnitsKeys["Flaw List MD"]].ItemArray[2].ToString());
            double ConverWidth = Convert.ToDouble(PxPVariable.UnitsData.Tables["unit"].Rows[PxPVariable.UnitsKeys["Flaw List Width"]].ItemArray[2].ToString());
            foreach (List<FlawInfoAddPriority> flaws in MapWindowVariable.FlawPieces)
            {
                foreach (FlawInfoAddPriority flaw in flaws)
                {
                    flaw.CD = flaw.OCD * ConverCD;
                    flaw.Area = (Convert.ToDouble(flaw.OArea) * ConverArea).ToString("0.######");
                    flaw.Length = flaw.OLength * ConverLength;
                    flaw.MD = flaw.OMD * ConverMD;
                    flaw.RCD = flaw.ORCD * ConverCD;
                    flaw.RMD = flaw.ORMD * ConverMD;
                    flaw.Width = flaw.OWidth * ConverWidth;
                }
            }
            MapWindowVariable.MapWindowController.SetMapAxis();

            gvFlaw.Refresh();

            // 自動換算 MapSetup 裡面的單位
            MapWindowVariable.MapCDSet = MapWindowVariable.MapCDSet * (ConverCD / MapWindowVariable.LastMapCDConvertion);
            MapWindowVariable.MapMDSet = MapWindowVariable.MapMDSet * (ConverMD / MapWindowVariable.LastMapMDConvertion);

            // 自動儲存 config
            string path = Path.GetDirectoryName(
             Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\..\\Parameter Files\\CPxP\\conf\\";
            string FullFilePath = string.Format("{0}{1}", path, SystemVariable.ConfigFileName);
            XDocument XDocConf = XDocument.Load(FullFilePath);
            XElement xCDConvertion = XDocConf.Element("Config").Element("MapVariable").Element("LastMapCDConvertion");
            xCDConvertion.Value = ConverCD.ToString();
            XElement xMDConvertion = XDocConf.Element("Config").Element("MapVariable").Element("LastMapMDConvertion");
            xMDConvertion.Value = ConverMD.ToString();
            XElement xMD = XDocConf.Element("Config").Element("MapVariable").Element("MapMDSet");
            xMD.Value = MapWindowVariable.MapCDSet.ToString();
            XElement xCD = XDocConf.Element("Config").Element("MapVariable").Element("MapCDSet");
            xCD.Value = MapWindowVariable.MapMDSet.ToString();
            MapWindowVariable.LastMapCDConvertion = ConverCD;
            MapWindowVariable.LastMapMDConvertion = ConverMD;
            XDocConf.Save(FullFilePath);
            PxPThreadStatus.IsOnUnitsChanged = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnCognitiveScience 成員

        public void OnCognitiveScience(Bitmap webImage, int leftEdge, int rightEdge, double md, double scaleFactor)
        {
            PxPThreadStatus.IsOnCognitiveScience = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnClassifyFlaw 成員

        // 存取顏色資訊
        public void OnSetFlawLegend(List<FlawLegend> legend)
        {
            PxPVariable.FlawLegend.Clear();
            PxPVariable.FlawLegend.AddRange(legend);
        }

        #endregion

        #region IOnGlassEdges 成員

        public void OnInitializeGlassEdges(int glassLeftMarginToROI, int glassRightMarginToROI)
        {

        }

        #endregion

        #endregion

        #region Thread Method
        
        // 主要執行緒, 負責處理繼承的介面, 接收狀態值切換動作

        #region 流程說明
        /**
         * 每一次按照繼承的 Interface 的 OnEvent 之後會接一個執行緒, 功能為當 UI Thread 被停止時還可以
         * 執行後續事件, 但是本程式比重很少
         */
        #endregion

        private void WorkerThread()
        {
            try
            {
                while (true)
                {
                    PxPThreadEvent.WaitOne();
                    if (PxPThreadStatus.IsOnShutdown)
                        return;

                    if (PxPThreadStatus.IsOnLanguageChanged)
                    {
                        PxPThreadStatus.IsOnLanguageChanged = false;
                        MethodInvoker LanguageChanged = new MethodInvoker(ProcessOnLanguageChenged);
                        this.BeginInvoke(LanguageChanged);
                    }

                    if (PxPThreadStatus.IsOnJobLoaded)
                    {
                        PxPThreadStatus.IsOnJobLoaded = false;
                        MethodInvoker JobLoaded = new MethodInvoker(ProcessJobLoaded);
                        this.BeginInvoke(JobLoaded);
                    }

                    if (PxPThreadStatus.IsOnJobStarted)
                    {
                        PxPThreadStatus.IsOnJobStarted = false;
                        MethodInvoker JobStarted = new MethodInvoker(ProcessJobStarted);
                        this.BeginInvoke(JobStarted);
                    }

                    //if (PxPThreadStatus.IsOnOnline)
                    //{
                    //    PxPThreadStatus.IsOnOnline = false;
                    //    MethodInvoker Online = new MethodInvoker(ProcessOnOnline);
                    //    this.BeginInvoke(Online);
                    //}

                    if (PxPThreadStatus.IsOnFlaws)
                    {
                        PxPThreadStatus.IsOnFlaws = false;
                        MethodInvoker Flaws = new MethodInvoker(ProcessOnFlaws);
                        this.BeginInvoke(Flaws);
                    }

                    if (PxPThreadStatus.IsOnCut)
                    {
                        PxPThreadStatus.IsOnCut = false;
                        MethodInvoker Cut = new MethodInvoker(ProcessOnCut);
                        this.BeginInvoke(Cut);
                    }

                    if (PxPThreadStatus.IsOnEvents)
                    {
                        PxPThreadStatus.IsOnEvents = false;
                        MethodInvoker Events = new MethodInvoker(ProcessOnEvents);
                        this.BeginInvoke(Events);
                    }

                    if (PxPThreadStatus.IsOnPxPConfig)
                    {
                        PxPThreadStatus.IsOnPxPConfig = false;
                        MethodInvoker PxPConfig = new MethodInvoker(ProcessOnPxPConfig);
                        this.BeginInvoke(PxPConfig);
                    }

                    if (PxPThreadStatus.IsOnWebDBConnected)
                    {
                        PxPThreadStatus.IsOnWebDBConnected = false;
                        MethodInvoker WebDBConnected = new MethodInvoker(ProcessOnWebDBConnected);
                        this.BeginInvoke(WebDBConnected);
                    }

                    if (PxPThreadStatus.IsOnSync)
                    {
                        PxPThreadStatus.IsOnSync = false;
                        MethodInvoker Sync = new MethodInvoker(ProcessOnSync);
                        this.BeginInvoke(Sync);
                    }

                    if (PxPThreadStatus.IsOnDoffResult)
                    {
                        PxPThreadStatus.IsOnDoffResult = false;
                        MethodInvoker DoffResult = new MethodInvoker(ProcessOnDoffResult);
                        this.BeginInvoke(DoffResult);
                    }

                    if (PxPThreadStatus.IsOnRollResult)
                    {
                        PxPThreadStatus.IsOnRollResult = false;
                        MethodInvoker RollResult = new MethodInvoker(ProcessOnRollResult);
                        this.BeginInvoke(RollResult);
                    }

                    if (PxPThreadStatus.IsOnOpenHistory)
                    {
                        PxPThreadStatus.IsOnOpenHistory = false;
                        MethodInvoker OpenHistory = new MethodInvoker(ProcessOnOpenHistory);
                        this.BeginInvoke(OpenHistory);
                    }

                    if (PxPThreadStatus.IsOnClassifyFlaw)
                    {
                        PxPThreadStatus.IsOnClassifyFlaw = false;
                        MethodInvoker ClassifyFlaw = new MethodInvoker(ProcessOnClassifyFlaw);
                        this.BeginInvoke(ClassifyFlaw);
                    }

                    if (PxPThreadStatus.IsFireEvent)
                    {
                        PxPThreadStatus.IsFireEvent = false;
                        MethodInvoker FireEvent = new MethodInvoker(ProcessFireEvent);
                        this.BeginInvoke(FireEvent);
                    }

                    if (PxPThreadStatus.IsOnUnitsChanged)
                    {
                        PxPThreadStatus.IsOnUnitsChanged = false;
                        MethodInvoker UnitsChanged = new MethodInvoker(ProcessOnUnitsChanged);
                        this.BeginInvoke(UnitsChanged);
                    }

                    if (PxPThreadStatus.IsOnJobStopped)
                    {
                        PxPThreadStatus.IsOnJobStopped = false;
                        MethodInvoker JobStopped = new MethodInvoker(ProcessOnJobStopped);
                        this.BeginInvoke(JobStopped);
                    }

                    if (PxPThreadStatus.IsOnGlassEdges)
                    {
                        PxPThreadStatus.IsOnGlassEdges = false;
                        MethodInvoker GlassEdges = new MethodInvoker(ProcessOnGlassEdges);
                        this.BeginInvoke(GlassEdges);
                    }

                    if (PxPThreadStatus.IsOnUserTermsChanged)
                    {
                        PxPThreadStatus.IsOnUserTermsChanged = false;
                        MethodInvoker UserTermsChanged = new MethodInvoker(ProcessOnUserTermsChanged);
                        this.BeginInvoke(UserTermsChanged);
                    }

                    if (PxPThreadStatus.IsOnCognitiveScience)
                    {
                        PxPThreadStatus.IsOnCognitiveScience = false;
                        MethodInvoker CognitiveScience = new MethodInvoker(ProcessOnCognitiveScience);
                        this.BeginInvoke(CognitiveScience);
                    }
                }
            }
            catch (Exception ex)
            {
                MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.GeneralError, "WorkerThread() Error!" + ex.Message, null, 0);
            }
        }

        // Map 執行緒負責處理繪圖畫面更新, 接收狀態值切換動作
        private void RefreshCheck()
        {
            try
            {
                while (true)
                {
                    MapThreadEvent.WaitOne();
                    // 強制中斷參數目前程式中沒有使用
                    if (MapWindowThreadStatus.StopMapThreading)
                        return;

                    // 除了 PxPTab 外其他頁面需要暫停 Job 使用
                    if (MapWindowThreadStatus.UpdateChange)
                    {
                        Job.SetOffline();
                        PxPThreadStatus.IsOnOnline = false;
                    }

                    // MapWindow 等其他頁面呼叫 Refresh Layout
                    if (MapWindowThreadStatus.IsTableLayoutRefresh)
                    {
                        MapWindowThreadStatus.IsTableLayoutRefresh = false;
                        MethodInvoker RefreshThread = new MethodInvoker(TableLayoutRefresh);
                        this.BeginInvoke(RefreshThread);
                    }

                    // 右邊 Grid 清空, 重繪右下角等 TableLayout
                    if (MapWindowThreadStatus.IsPageRefresh)
                    {
                        MapWindowThreadStatus.IsPageRefresh = false;
                        MethodInvoker RefreshThread = new MethodInvoker(PageRefresh);
                        this.BeginInvoke(RefreshThread);
                    }

                    // 當左邊切換頁面時, 重繪 DrawTablePictures
                    if (MapWindowThreadStatus.IsChangePiece)
                    {
                        MapWindowThreadStatus.IsChangePiece = false;
                        MethodInvoker RefreshThread = new MethodInvoker(TableLayoutChangePiece);
                        this.BeginInvoke(RefreshThread);
                    }
                }
            }
            catch(Exception ex)
            {
                MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.GeneralError, "RefreshCheck() Error!" + ex.Message, null, 0);
            }
        }

        #endregion

        #region Thread Method Functions
        
        // Deal with IWRPlugIn Interface Event
        public void ProcessJobLoaded()
        {
            PxPThreadStatus.IsOnJobLoaded = false;
        }

        public void ProcessJobStarted()
        {
            // 工單開始執行之後的動作
            MapWindowVariable.MapWindowController.SetGvFlawClass(PxPVariable.FlawTypeName);
            PxPThreadStatus.IsOnJobStarted = false;
        }

        public void ProcessOnCut()
        {
            // UNDONE : 新增判斷非讀取歷史資料時設定 CurrentPiece
            //if (!SystemVariable.IsReadHistory)
            //{
            //    MapWindowVariable.CurrentPiece = MapWindowVariable.FlawPieces.Count;
            //}
            MapWindowVariable.MapWindowController.DrawPieceFlaw(MapWindowVariable.CurrentPiece - 1, true);
            
            // 處理右下角圖片
            DrawTablePictures(MapWindowVariable.FlawPieces,MapWindowVariable.CurrentPiece,1);
            PxPThreadStatus.IsOnCut = false;
        }

        public void ProcessOnDoffResult()
        {
            PxPThreadStatus.IsOnDoffResult = false;
        }

        public void ProcessOnPxPConfig()
        {
            PxPThreadStatus.IsOnPxPConfig = false;
        }

        public void ProcessOnOpenHistory() 
        {
            PxPThreadStatus.IsOnOpenHistory = false;
        }

        public void ProcessOnUnitsChanged()
        {
            PxPThreadStatus.IsOnUnitsChanged = false;
        }

        public void ProcessOnLanguageChenged()
        {
            PxPThreadStatus.IsOnLanguageChanged = false;
        }

        public void ProcessOnFlaws()
        {
            PxPThreadStatus.IsOnFlaws = false;
        }

        public void ProcessOnEvents()
        {
            PxPThreadStatus.IsOnEvents = false;
        }

        public void ProcessOnWebDBConnected()
        {
            PxPThreadStatus.IsOnWebDBConnected = false;
        }

        public void ProcessOnSync()
        {
            PxPThreadStatus.IsOnSync = false;
        }

        public void ProcessOnRollResult()
        {
            PxPThreadStatus.IsOnRollResult = false;
        }

        public void ProcessOnClassifyFlaw()
        {
            PxPThreadStatus.IsOnClassifyFlaw = false;
        }

        public void ProcessFireEvent()
        {
            PxPThreadStatus.IsFireEvent = false;
        }

        public void ProcessOnJobStopped()
        {
            PxPThreadStatus.IsOnJobStopped = false;
        }

        public void ProcessOnGlassEdges()
        {
            PxPThreadStatus.IsOnGlassEdges = false;
        }

        public void ProcessOnUserTermsChanged()
        {
            PxPThreadStatus.IsOnUserTermsChanged = false;
        }

        public void ProcessOnCognitiveScience()
        {
            PxPThreadStatus.IsOnCognitiveScience = false;
        }
        
        #endregion

        #region Action Events

        // 操作事件
        // Images Next Grid 
        private void btnNextGrid_Click(object sender, EventArgs e)
        {
            Job.SetOffline();
            PxPThreadStatus.IsOnOnline = false;
            int Page = (PxPVariable.PageCurrent + 1 > PxPVariable.PageTotal) ? PxPVariable.PageTotal : PxPVariable.PageCurrent + 1;
            DrawTablePictures(MapWindowVariable.FlawPieces, MapWindowVariable.CurrentPiece, Page);
        }

        // Images Previous Grid
        private void btnPrevGrid_Click(object sender, EventArgs e)
        {
            Job.SetOffline();
            PxPThreadStatus.IsOnOnline = false;
            int Page = (PxPVariable.PageCurrent - 1 < 1) ? 1 : PxPVariable.PageCurrent - 1;
            DrawTablePictures(MapWindowVariable.FlawPieces, MapWindowVariable.CurrentPiece, Page);
        }

        // DataGridView Sort by Column header click
        private void gvFlaw_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Job.SetOffline();
            PxPThreadStatus.IsOnOnline = false;

            // Check which column is selected, otherwise set NewColumn to null.
            if (gvFlaw.Rows.Count > 0)
            {
                DataGridViewColumn NewColumn = gvFlaw.Columns[e.ColumnIndex];
                DataGridViewColumn OlderColumn = gvFlaw.SortedColumn;

                PxPVariable.FlawGridViewOrderColumn = NewColumn.Name;
                SortGridViewByColumn(PxPVariable.FlawGridViewOrderColumn);
                DrawTablePictures(MapWindowVariable.FlawPieces, MapWindowVariable.CurrentPiece, 1);
                MapWindowVariable.MapWindowController.DrawPieceFlaw(MapWindowVariable.CurrentPiece - 1, false);
            }        
        }

        private void tlpDoffGrid_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                Graphics g = e.Graphics;
                Control c = tlpDoffGrid.Controls[PxPVariable.ChooseFlawID.ToString()] as SingleFlawControl;
                Pen p = new Pen(Color.SandyBrown, 6.0f);
                Rectangle rec = new Rectangle(c.Location, new Size(c.Width, c.Height));
                g.DrawRectangle(p, rec);
            }
            catch (Exception ex)
            {
                
            }
        }

        private void gvFlaw_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Job.SetOffline();
            PxPThreadStatus.IsOnOnline = false;
            int v = -1;
            int p = e.RowIndex / PxPVariable.PageSize + 1;
            if(e.RowIndex == -1)
                PxPVariable.ChooseFlawID = int.TryParse(gvFlaw.Rows[0].Cells["FlawID"].Value.ToString(), out v) ? v : -1;
            else
                PxPVariable.ChooseFlawID = int.TryParse(gvFlaw.Rows[e.RowIndex].Cells["FlawID"].Value.ToString(), out v) ? v : -1;
            
            if (PxPVariable.PageCurrent != p)
            {
                PxPVariable.PageCurrent = p;
                DrawTablePictures(MapWindowVariable.FlawPieces, MapWindowVariable.CurrentPiece, p);
            }
            else
            {
                this.tlpDoffGrid.Refresh();
            }
        }

        #endregion
    }
}
