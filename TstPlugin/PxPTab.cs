using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Xml;
using System.IO;
using System.Threading;
using System.ComponentModel.Composition;
using WRPlugIn;
using Nevron.Chart;
using Nevron.GraphicsCore;
using Microsoft.Win32;
using System.Xml.Linq;
using System.ComponentModel;

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
         * 1. 所有物件與變數資訊都由PxPTab控制, MapWindow需要時,MapWindow提供Method由PxPTab將值傳入.
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
        #region Local Variable

        public PictureBox[] pbFlaws;
        public int ImgPlaceHolderWidth;
        public int ImgPlaceHolderHeight;                  //右下角DataGrid內置放圖片容器寬高
        string NowColumn = "";
        bool SortSwitch = false;

        
        #endregion

        //////////////////////////////////////////////////////////////////////////

        #region Initialize Thread
        #region 註解
        /*
         *  PxPThread : 執行外掛程式本身
         *  MapThread : 持續繪圖至左邊區塊
         */
        #endregion
        private Thread PxPThread = null;
        private AutoResetEvent PxPThreadEvent = new AutoResetEvent(false);
        private Thread MapThread = null;
        public static AutoResetEvent MapThreadEvent = new AutoResetEvent(false);
        private IAsyncResult CutResult;
        #endregion
        //////////////////////////////////////////////////////////////////////////

        #region Contructor
        //建構子
        public PxPTab()
        {
            InitializeComponent();
            //MessageBox.Show("PxPTab");
            //DebugTool.WriteLog("PxPTab.cs", "PxPTab Constructor");
            
            //執行緒啟動
            MapThread = new Thread(new ThreadStart(RefreshCheck));
            MapThread.Start();
            PxPThread = new Thread(new ThreadStart(WorkerThread));
            PxPThread.Start();

            SystemVariable.LoadSystemConfig();
            SystemVariable.LoadConfig();

            InitTableLayout(tlpDoffGrid);
            DefineDataGridView(gvFlaw);
           
        }
        //解構子
        ~PxPTab()
        {
            try
            {
                //Save columns sorted
                string FolderPath = Path.GetDirectoryName(
            Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\..\\Parameter Files\\CPxP\\sys_conf\\";
                string FullFilePath = FolderPath + "sys.xml";
                XDocument XDoc = XDocument.Load(FullFilePath);
                IEnumerable<XElement> Columns = XDoc.Element("SystemConfig").Element("DoffGrid").Elements("Column");
                for (int i = 0; i < gvFlaw.Columns.Count; i++)
                {
                    foreach (var column in Columns)
                    {
                        if (column.Attribute("Name").Value.ToString() == gvFlaw.Columns[i].Name.ToString())
                        {
                            column.SetElementValue("Index", gvFlaw.Columns[i].DisplayIndex.ToString());
                            column.SetElementValue("Size", gvFlaw.Columns[i].Width);
                        }

                    }
                }
                //Order by which column
                XElement OrderByColumn = XDoc.Element("SystemConfig").Element("DoffGrid").Element("OrderBy");
                OrderByColumn.Value = NowColumn;
                XDoc.Save(FullFilePath);
                /////////////////////////////////////////////////////////////////////////////////////////////////

           
                //MapConf : Save MapWindow Radio Button checked index
                XDocument XDocConf = SystemVariable.GetConfig();
                XElement ShowFlag = XDocConf.Element("Config").Element("MapVariable").Element("ShowFlag");
                ShowFlag.Value = MapWindowVariable.ShowFlag.ToString();
                string path = Path.GetDirectoryName(
                 Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\..\\Parameter Files\\CPxP\\conf\\";
                string FullConfFilePath = string.Format("{0}{1}", path, SystemVariable.ConfigFileName);
                XDocConf.Save(FullConfFilePath);
               
                
            }
            catch (Exception e)
            {
                MessageBox.Show("Close Program Error");
            }

            PxPThreadStatus.IsOnShutdown = true;
            PxPThreadEvent.Set();
        }
        #endregion

        #region Refactoring
        //定義右上角DataGridView
        void DefineDataGridView(DataGridView Dgv)
        {
            //全部欄位都有實作 不要的從最下面Visible關閉
            bsFlaw.DataSource = MapWindowVariable.FlawPiece;
            Dgv.DataSource = bsFlaw;
            Dgv.AllowUserToOrderColumns = true;
            

            foreach (var column in PxPVariable.DoffGridSetup)
            {
                Dgv.Columns[column.ColumnName].SortMode = DataGridViewColumnSortMode.Automatic;
                Dgv.Columns[column.ColumnName].HeaderText = column.HeaderText;
                Dgv.Columns[column.ColumnName].DisplayIndex = column.Index;
                Dgv.Columns[column.ColumnName].Width = column.Width;
            }
            //Disable column
            Dgv.Columns["Images"].Visible = false;
            Dgv.Columns["LeftEdge"].Visible = false;
            Dgv.Columns["RightEdge"].Visible = false;
            Dgv.Columns["FlawType"].Visible = false;
            Dgv.Columns["RMD"].Visible = false;
            Dgv.Columns["RCD"].Visible = false;
        }       
        //更新頁面,該換圖或Map調整,語系變更,全域變數變更時更新物件資料
        public void PageRefresh()
        {
            InitTableLayout(tlpDoffGrid); //重置TableLayout
            DefineDataGridView(gvFlaw);   //重繪右上角DataGridView
        }
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
        //設定初始化TableLayoutPanel
        void InitTableLayout(TableLayoutPanel Tlp)
        {
            Tlp.ColumnStyles.Clear();
            Tlp.RowCount = PxPVariable.ImgRowsSet;
            Tlp.ColumnCount = PxPVariable.ImgColsSet;
            //pbFlaws = new PictureBox[Tlp.RowCount * Tlp.ColumnCount];
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
        //繪製TableLayoutPanel將圖片置入Control
        public void DrawTablePictures(List<List<FlawInfoAddPriority>> FlawPieces, int PieceID, int PageNum)
        {
            
            PxPVariable.PageCurrent = (PageNum < 1) ? 1 : PageNum;
            PxPVariable.PageTotal = gvFlaw.Rows.Count % PxPVariable.PageSize == 0 ?
                                       gvFlaw.Rows.Count / PxPVariable.PageSize :
                                       gvFlaw.Rows.Count / PxPVariable.PageSize + 1;
            lbPageCurrent.Text = PxPVariable.PageCurrent.ToString();
            lbPageTotal.Text = PxPVariable.PageTotal.ToString();
            //Deal Button enable
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

            int FlawPointStart = (PxPVariable.PageCurrent - 1) * 9;
            int FlawPointEnd = ((FlawPointStart + PxPVariable.PageSize) > gvFlaw.Rows.Count) ? gvFlaw.Rows.Count : (FlawPointStart + PxPVariable.PageSize);
            
            tlpDoffGrid.Controls.Clear();
            tlpDoffGrid.Refresh();
            tlpDoffGrid.Visible = false;
            //Add controls into table layout
            if (gvFlaw.Rows.Count > 0)
            {
                for (int i = 0; i < PxPVariable.PageSize; i++)
                {
                    int flag = ((PxPVariable.PageCurrent - 1) * PxPVariable.PageSize) + i;
                    if (flag >= gvFlaw.Rows.Count) break;
                    flag = gvFlaw.Rows[flag].Index;
                    if (flag < FlawPieces[PieceID - 1].Count )
                    {
                        SingleFlawControl sfc = new SingleFlawControl(FlawPieces[PieceID - 1][flag]);
                        sfc.Width = ImgPlaceHolderWidth;
                        sfc.Height = ImgPlaceHolderHeight;
                        sfc.Name = sfc.Tag.ToString();
                        tlpDoffGrid.Controls.Add(sfc);
                        sfc.Dock = DockStyle.Fill;
                    }
                   
                }
            }
            // For improve efficacy
            tlpDoffGrid.Visible = true;
        }
        //調整圖片縮放讓整張圖可以完整呈現
        public void ImageAdjust(Bitmap Bmp, PictureBox Pb)
        {
            double Width_d = (double)Bmp.Width / Pb.Width;
            double Height_d = (double)Bmp.Height / Pb.Height;
            double Ratio = 1.0;
            if (Width_d > 1 || Height_d > 1)
            {
                if (Width_d > Height_d)
                {
                    Ratio = Width_d;

                }
                else
                {
                    Ratio = Height_d;
                }
            }
            else if (Width_d < 1 && Height_d < 1)
            {
                if (Width_d > Height_d)
                    Ratio = Width_d;
                else
                    Ratio = Height_d;
            }
            Pb.Width = (int)Math.Round(Bmp.Width / Ratio);
            Pb.Height = (int)Math.Round(Bmp.Height / Ratio);
            Pb.Image = Bmp;
        }
        //DataGridView排序
        public void SortGridViewByColumn(string ColumnName)
        {
            /*
              * false = 沒點過
              * true = 點過
              */
            switch (ColumnName)
            {
                case "FlawID":
                    if (NowColumn == ColumnName && SortSwitch)
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f2.FlawID.CompareTo(f1.FlawID); });
                    else
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f1.FlawID.CompareTo(f2.FlawID); });
                    NowColumn = "FlawID";
                    SortSwitch = !SortSwitch;
                    gvFlaw.Refresh();
                    break;
                case "Priority":
                    if (NowColumn == ColumnName && SortSwitch)
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f2.Priority.CompareTo(f1.Priority); });
                    else
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f1.Priority.CompareTo(f2.Priority); });
                    NowColumn = "Priority";
                    SortSwitch = !SortSwitch;
                    gvFlaw.Refresh();
                    break;
                case "FlawClass":
                    if (NowColumn == ColumnName && SortSwitch)
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f2.FlawClass.CompareTo(f1.FlawClass); });
                    else
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f1.FlawClass.CompareTo(f2.FlawClass); });
                    NowColumn = "FlawClass";
                    SortSwitch = !SortSwitch;
                    gvFlaw.Refresh();
                    break;
                case "MD":
                    if (NowColumn == ColumnName && SortSwitch)
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f2.MD.CompareTo(f1.MD); });
                    else
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f1.MD.CompareTo(f2.MD); });
                    NowColumn = "MD";
                    SortSwitch = !SortSwitch;
                    gvFlaw.Refresh();
                    break;
                case "CD":
                    if (NowColumn == ColumnName && SortSwitch)
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f2.CD.CompareTo(f1.CD); });
                    else
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f1.CD.CompareTo(f2.CD); });
                    NowColumn = "CD";
                    SortSwitch = !SortSwitch;
                    gvFlaw.Refresh();
                    break;
                case "Area":
                    if (NowColumn == ColumnName && SortSwitch)
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f2.Area.CompareTo(f1.Area); });
                    else
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f1.Area.CompareTo(f2.Area); });
                    NowColumn = "Area";
                    SortSwitch = !SortSwitch;
                    gvFlaw.Refresh();
                    break;
                case "Width":
                    if (NowColumn == ColumnName && SortSwitch)
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f2.Width.CompareTo(f1.Width); });
                    else
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f1.Width.CompareTo(f2.Width); });
                    NowColumn = "Width";
                    SortSwitch = !SortSwitch;
                    gvFlaw.Refresh();
                    break;
                case "Length":
                    if (NowColumn == ColumnName && SortSwitch)
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f2.Length.CompareTo(f1.Length); });
                    else
                        MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) { return f1.Length.CompareTo(f2.Length); });
                    NowColumn = "Length";
                    SortSwitch = !SortSwitch;
                    gvFlaw.Refresh();
                    break;
            };
        }
        //Find Control
        public IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }
        #endregion

        #region Public Methods
        public  void SetJobOnline()
        {
            Job.SetOnline();
            PxPThreadStatus.IsOnOnline = true;
        }
        public  void SetJobOffline()
        {
            Job.SetOffline();
            PxPThreadStatus.IsOnOnline = false;
        }
        
        #endregion

        #region Inherit Interface

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
        /// <summary>
        /// 取得目前控制項視窗代碼並傳回指標
        /// </summary>
        /// <param name="hndl"></param>
        public void GetControlHandle(out IntPtr hndl)
        {
            //MessageBox.Show("IWRPlugIn-GetControlHandle");
            //DebugTool.WriteLog("PxPTab.cs", "IWRPlugIn-GetControlHandle");
            hndl = Handle;
        }
        /// <summary>
        /// 設定控制項顯示的X,Y位置座標及寬高
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public void SetPosition(int w, int h)
        {
            //MessageBox.Show("IWRPlugIn-SetPosition");
            //DebugTool.WriteLog("PxPTab.cs", "IWRPlugIn-SetPosition");
            SetBounds(0, 0, w, h); //Default : w760,h747
        }

        /// <summary>
        /// 主畫面透過GetName根據語系取得Tab名稱,在Plugin內部則是透過out 設定傳入變數
        /// </summary>
        /// <param name="lang"></param>
        /// <param name="name"></param>
        public void GetName(e_Language lang, out string name)
        {
            //MessageBox.Show("IWRPlugIn-GetName");
            //DebugTool.WriteLog("PxPTab.cs", "IWRPlugIn-GetName");
            switch (lang)
            {
                case e_Language.Chinese:
                    name = "玻璃組件";
                    break;
                default:
                    name = "PxP";
                    break;
            }
        }

        /// <summary>
        /// 卸載Plugin
        /// </summary>
        public void Unplug()
        {
            //MessageBox.Show("IWRPlugIn-Unplug");
            //DebugTool.WriteLog("PxPTab.cs", "IWRPlugIn-Unplug");
            //trigger on close the window
        }
        #endregion

        #region IWRMapWindow 成員
        public void GetMapControlHandle(out IntPtr hndl)
        {
            //MessageBox.Show("IWRMapWindow-GetMapControlHandle");
            //DebugTool.WriteLog("PxPTab.cs", "IWRMapWindow-GetMapControlHandle");
            hndl = MapWindowVariable.MapWindowController.Handle;
        }

        public void SetMapPosition(int w, int h)
        {
            //MessageBox.Show("IWRMapWindow-SetMapPosition");
            //DebugTool.WriteLog("PxPTab.cs", "IWRMapWindow-SetMapPosition");
            MapWindowVariable.MapWindowController.SetBounds(0, 0, w, h);
        }

        #endregion

        #region IOnFlaws 成員
        public void OnFlaws(IList<IFlawInfo> flaws)
        {
            //MessageBox.Show("OnFlaws");
            //DebugTool.WriteLog("PxPTab.cs", "OnFlaws");
            try
            {   // Deal flaws  extend other data
                IList<FlawInfoAddPriority> temp = new List<FlawInfoAddPriority>();
                foreach (var i in flaws)
                {
                    FlawInfoAddPriority f = new FlawInfoAddPriority();
                    f.Area = i.Area.ToString("0.######");
                    f.CD = Math.Round(i.CD,2);
                    f.FlawClass = i.FlawClass;
                    f.FlawID = i.FlawID;
                    f.FlawType = i.FlawType;
                    f.Images = i.Images;
                    f.LeftEdge = i.LeftEdge;
                    f.Length = i.Length;
                    f.MD = Math.Round(i.MD,2);
                    f.RMD = Math.Round(i.MD - PxPVariable.CurrentCutPosition, 2);
                    f.RCD = Math.Round(i.CD - PxPVariable.CurrentCutPosition, 2);
                    f.RightEdge = i.RightEdge;
                    f.Width = Math.Round(i.Width,4);
                    //特別處理Priority
                    int opv;
                    f.Priority = PxPVariable.SeverityInfo[0].Flaws.TryGetValue(f.FlawType, out opv) ? opv : 0;

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
            //MessageBox.Show("OnEvents");
            //DebugTool.WriteLog("PxPTab.cs", "OnEvents");
            PxPThreadStatus.IsOnEvents = true;
            foreach (var eventInfo in events)
            {
                switch ((e_EventID)eventInfo.EventType)
                {
                    case e_EventID.STOP_JOB:
                        PxPThreadStatus.IsOnOnline = false;
                        PxPThreadStatus.IsOnJobStopped = true;
                        break;
                    default: break;
                }
            }
            PxPThreadEvent.Set();
        }

        #endregion
        
        #region IOnCut 成員

        public void OnCut(double md)
        {
            //MessageBox.Show("OnCut");
            //DebugTool.WriteLog("PxPTab.cs", "OnCut");
            MapWindowVariable.FlawPiece.Clear();
            foreach (var f in MapWindowVariable.Flaws)
            {
                if (f.MD < PxPVariable.CurrentCutPosition + PxPVariable.PxPInfo.Height && f.MD > PxPVariable.CurrentCutPosition)
                    MapWindowVariable.FlawPiece.Add(f);
            }
            MapWindowVariable.Flaws.Clear();

            List<FlawInfoAddPriority> subPiece = new List<FlawInfoAddPriority>();
            foreach (var f in MapWindowVariable.FlawPiece)
            {
                subPiece.Add(f);
            }
            MapWindowVariable.FlawPieces.Add(subPiece); //把PxP處理完的每一片儲存
            PxPVariable.CurrentCutPosition = md;

            if (PxPThreadStatus.IsOnOnline)
            {
                gvFlaw.DataSource = bsFlaw;
                SortGridViewByColumn(PxPVariable.FlawGridViewOrderColumn);
                //MapWindowVariable.FlawPiece.Sort(delegate(FlawInfoAddPriority f1, FlawInfoAddPriority f2) {  return f2.Width.CompareTo(f1.Width); });
                bsFlaw.ResetBindings(false);
                
                bsFlaw.ResumeBinding();

                PxPThreadStatus.IsOnCut = true;
                PxPThreadEvent.Set();
            }
        }

        #endregion

        #region IOnJobLoaded 成員
        public void OnJobLoaded(IList<IFlawTypeName> flawTypes, IList<ILaneInfo> lanes, IList<ISeverityInfo> severityInfo, IJobInfo jobInfo)
        {
            #region 註解
            /*
             * 1. 使用Model來存取介面取得之參數.
             * 2. 啟動並回復執行緒
             */
            #endregion
            //MessageBox.Show("OnJobLoaded");
            //DebugTool.WriteLog("PxPTab.cs", "OnJobLoaded");
            //PxPVariable.FlawTypeName.Clear();
            //PxPVariable.FlawTypeName = flawTypes;
            IList<FlawTypeNameExtend> tmpList = new List<FlawTypeNameExtend>();
            var random = new Random();
            foreach (var i in flawTypes)
            {
                foreach(var f in PxPVariable.FlawTypeName )
                {
                    
                    if(i.Name == f.Name)
                    {
                       
                    }else
                    {
                        //Default
                        FlawTypeNameExtend tmp = new FlawTypeNameExtend();
                        tmp.FlawType = i.FlawType;
                        tmp.Name = i.Name;
                        tmp.Display =true; 
                       
                        tmp.Color =String.Format("#{0:X6}", random.Next(0x1000000));
                        tmp.Shape = Shape.Cross.ToGraphic();
                        tmpList.Add(tmp);

                    }
                    
                }
            }
            foreach (var el in tmpList)
            {
                PxPVariable.FlawTypeName.Add(el);
            }
            PxPVariable.FlawTypeName.Sort(delegate(FlawTypeNameExtend f1, FlawTypeNameExtend f2) { return f1.FlawType.CompareTo(f2.FlawType); });
            PxPVariable.JobInfo = jobInfo;
            PxPVariable.SeverityInfo = severityInfo;
            PxPThreadStatus.IsOnJobLoaded = true;
            
            //Disable MapSetup Button
            MapWindowVariable.MapWindowController.btnMapSetup.Enabled = false;
            MapWindowVariable.MapWindowController.SetJobInfo();
            MapWindowVariable.MapWindowController.SetMapAxis();
            MapWindowVariable.MapWindowController.bsFlawType.ResetBindings(false);
            PxPThreadEvent.Set();
        }
        #endregion

        #region IOnJobStarted 成員

        public void OnJobStarted(int jobKey)
        {
            //MessageBox.Show("OnJobStarted");
            //DebugTool.WriteLog("PxPTab.cs", "OnJobStarted");
            PxPVariable.JobKey = jobKey;
            PxPThreadStatus.IsOnJobStarted = true;
            PxPThreadEvent.Set();
        }
        #endregion

        #region IOnLanguageChanged 成員

        public void OnLanguageChanged(e_Language language)
        {
            //MessageBox.Show("OnLanguageChanged");
            //DebugTool.WriteLog("PxPTab.cs", "OnLanguageChanged");
            SystemVariable.Language = language;
            #region 註解
            /*
             * 需要變更語系的變數或屬性列表
             * 1. 右上方缺陷點DataGridView Columns
             * 2. 左上方Lables, Buttons
             * 3. 左下方缺陷點分類DataGridView Columns
             * 4. 
             */


            #endregion
            //載入語系並設定需變更的值
            try
            {
                /////////////////////////////////////////////////////////////////////////////////
                XDocument XDocLang = SystemVariable.GetLangXDoc(SystemVariable.Language);
                IEnumerable<XElement> PxPDoffGridLangItem = XDocLang.Element("Language").Element("PxPTab").Element("DoffGrid").Elements("Column");
                foreach (var column in PxPVariable.DoffGridSetup)
                {
                    foreach (var i in PxPDoffGridLangItem)
                    {
                        if (column.ColumnName == i.Attribute("Name").Value)
                        {
                            column.HeaderText = i.Value;
                        }
                    }
                }
                /////////////////////////////////////////////////////////////////////////////////
                IEnumerable<XElement> MapFlawClassGridColumns = XDocLang.Element("Language").Element("MapWindow").Element("DoffSettingGrid").Elements("Column");
                //foreach (DataGridViewColumn column in MapWindowVariable.MapWindowController.gvFlawClass.Columns)
                //{
                //    foreach (var i in MapFlawClassGridColumns)
                //    {
                //        if (column.Name == i.Attribute("Name").Value)
                //        {
                //            column.HeaderText = i.Value.ToString();
                //        }
                //    }
                //}
                foreach (var column in MapWindowVariable.DoffTypeGridSetup)
                {
                    foreach (var i in MapFlawClassGridColumns)
                    {
                        if (column.ColumnName == i.Attribute("Name").Value)
                        {
                            column.HeaderText = i.Value.ToString();
                        }
                    }
                }
                /////////////////////////////////////////////////////////////////////////////////
                IEnumerable<XElement> MapWindowButtons = XDocLang.Element("Language").Element("MapWindow").Element("Object").Elements("Button");
                var btns = GetAll(MapWindowVariable.MapWindowController, typeof(Button));
                foreach (var btn in btns)
                {
                    foreach (var i in MapWindowButtons)
                    {
                        if (btn.Name == i.Attribute("Name").Value)
                        {
                            btn.Text = i.Value.ToString();
                        }
                    }
                }
                /////////////////////////////////////////////////////////////////////////////////
                IEnumerable<XElement> MapWindowRadioButtons = XDocLang.Element("Language").Element("MapWindow").Element("Object").Elements("RadioButton");
                var rbtns = GetAll(MapWindowVariable.MapWindowController, typeof(RadioButton));
                foreach (var btn in rbtns)
                {
                    foreach (var i in MapWindowRadioButtons)
                    {
                        if (btn.Name == i.Attribute("Name").Value)
                        {
                            btn.Text = i.Value.ToString();
                        }
                    }
                }
                
                 
                /////////////////////////////////////////////////////////////////////////////////
                //刷新所有須改變語系的物件
                PageRefresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Language loading error");
            }
            // PxPThreadEvent.Set();
        }

        #endregion

        #region IOnJobStopped 成員

        public void OnJobStopped(double md)
        {
            //MessageBox.Show("OnJobStopped");
            //DebugTool.WriteLog("PxPTab.cs", "OnJobStopped");
            MapWindowVariable.MapWindowController.btnMapSetup.Enabled = true;
            PxPThreadStatus.IsOnJobStopped = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnWebDBConnected 成員

        public void OnWebDBConnected(IWebDBConnectionInfo info)
        {
            //MessageBox.Show("OnWebDBConnected");
            //DebugTool.WriteLog("PxPTab.cs", "OnWebDBConnected");
            PxPThreadStatus.IsOnWebDBConnected = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnSync 成員

        public void OnSync(double md)
        {
            //MessageBox.Show("OnSync");
            //DebugTool.WriteLog("PxPTab.cs", "OnSync");
            PxPThreadStatus.IsOnSync = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnGlassEdges 成員

        public void OnGlassEdges(double md, double le1, double le2, double le3, double leftROI, double rightROI, double re3, double re2, double re1)
        {
            //MessageBox.Show("OnGlassEdges");
            //DebugTool.WriteLog("PxPTab.cs", "OnGlassEdges");
            PxPThreadStatus.IsOnGlassEdges = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnOnline 成員

        public void OnOnline(bool isOnline)
        {
            //MessageBox.Show("OnOnline");
            //DebugTool.WriteLog("PxPTab.cs", "OnOnline");
            PxPThreadStatus.IsOnOnline = isOnline;
           
            if (isOnline)
            {
                bsFlaw.DataSource = MapWindowVariable.FlawPiece;
                gvFlaw.Rows.Clear();
                tlpDoffGrid.Controls.Clear();
                MapWindowVariable.MapWindowController.ClearMap();

                lbPageTotal.Text = "--";
                lbPageCurrent.Text = "--";
            }
            else
            {
                if (MapWindowVariable.CurrentPiece > 0)
                {
                    PxPVariable.FreezPiece = MapWindowVariable.CurrentPiece;
                    bsFlaw.DataSource = MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1];
                }
            }
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnUserTermsChanged 成員

        public void OnUserTermsChanged(IUserTerms terms)
        {
            //MessageBox.Show("OnUserTermsChanged");
            //DebugTool.WriteLog("PxPTab.cs", "OnUserTermsChanged");

            PxPThreadStatus.IsOnUserTermsChanged = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnDoffResult 成員

        public void OnDoffResult(double md, int doffNumber, bool pass)
        {
            //MessageBox.Show("OnDoffResult");
            //DebugTool.WriteLog("PxPTab.cs", "OnDoffResult");
            if (pass)
                PxPVariable.PassNum++;
            else
                PxPVariable.FailNum++;

            PxPVariable.DoffNum = doffNumber;
            PxPThreadStatus.IsOnDoffResult = true;
            MapWindowVariable.MapWindowController.SetMapInfoLabel();
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnPxPConfig 成員

        public void OnPxPConfig(IPxPInfo info, string unitsXMLPath)
        {
            //MessageBox.Show("OnPxPConfig");
            //DebugTool.WriteLog("PxPTab.cs", "OnPxPConfig");
            PxPVariable.PxPInfo = info;
            PxPVariable.UnitsXMLPath = unitsXMLPath;

            PxPThreadStatus.IsOnPxPConfig = true;
            PxPThreadEvent.Set();
        }
        
        #endregion

        #region IOnRollResult 成員

        public void OnRollResult(double cd, double md, int doffNumber, int laneNumber, bool pass)
        {
            //MessageBox.Show("OnRollResult");
            //DebugTool.WriteLog("PxPTab.cs", "OnRollResult");

            PxPThreadStatus.IsOnRollResult = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnOpenHistory 成員

        public void OnOpenHistory(double startMD, double stopMD)
        {
            //MessageBox.Show("OnOpenHistory");
            //DebugTool.WriteLog("PxPTab.cs", "OnOpenHistory");

            PxPThreadStatus.IsOnOpenHistory = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IWRFireEvent 成員

        public void FireEvent(int eventID, double cd, double md)
        {
            //MessageBox.Show("FireEvent");
            //DebugTool.WriteLog("PxPTab.cs", "FireEvent");

            PxPThreadStatus.IsFireEvent = true;
            PxPThreadEvent.Set();

        }

        #endregion

        #region IOnClassifyFlaw 成員
        public void OnClassifyFlaw(ref WRPlugIn.IFlawInfo flaw, ref bool deleteFlaw)
        {
            //MessageBox.Show("OnClassifyFlaw");
            //DebugTool.WriteLog("PxPTab.cs", "OnClassifyFlaw");

            PxPThreadStatus.IsOnClassifyFlaw = true;
            PxPThreadEvent.Set();
        }
        #endregion

        #region IOnUnitsChanged 成員

        public void OnUnitsChanged()
        {
            //MessageBox.Show("OnUnitsChanged");
            //DebugTool.WriteLog("PxPTab.cs", "OnUnitsChanged");

            PxPThreadStatus.IsOnUnitsChanged = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnCognitiveScience 成員

        public void OnCognitiveScience(Bitmap webImage, int leftEdge, int rightEdge, double md, double scaleFactor)
        {
            //MessageBox.Show("OnCognitiveScience");
            //DebugTool.WriteLog("PxPTab.cs", "OnCognitiveScience");

            PxPThreadStatus.IsOnCognitiveScience = true;
            PxPThreadEvent.Set();
        }

        #endregion
        #endregion

        #region Thread Method
        //主要執行緒,負責處理繼承的介面,接收狀態值切換動作
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

                    /*if (PxPThreadStatus.IsOnOnline)
                    {
                        PxPThreadStatus.IsOnOnline = false;
                        MethodInvoker Online = new MethodInvoker(ProcessOnOnline);
                        this.BeginInvoke(Online);
                    }*/

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
        //Map執行緒負責處理繪圖畫面更新,接收狀態值切換動作
        private void RefreshCheck()
        {
            try
            {
                while (true)
                {
                    MapThreadEvent.WaitOne();
                    if (MapWindowThreadStatus.StopMapThreading)
                        return;

                    if (MapWindowThreadStatus.UpdateChange)
                    {
                        Job.SetOffline();
                        PxPThreadStatus.IsOnOnline = false;
                    }

                    if (MapWindowThreadStatus.IsTableLayoutRefresh)
                    {
                        MapWindowThreadStatus.IsTableLayoutRefresh = false;
                        MethodInvoker RefreshThread = new MethodInvoker(TableLayoutRefresh);
                        this.BeginInvoke(RefreshThread);
                    }

                    if (MapWindowThreadStatus.IsPageRefresh)
                    {
                        MapWindowThreadStatus.IsPageRefresh = false;
                        MethodInvoker RefreshThread = new MethodInvoker(PageRefresh);
                        this.BeginInvoke(RefreshThread);
                    }
                    //if (v_update)
                    //{
                    //    v_update = false;
                    //    MethodInvoker UpdateThread = new MethodInvoker(UpdateChange);
                    //    this.BeginInvoke(UpdateThread);
                    //}

                    //if (prev)
                    //{
                    //    prev = false;
                    //    MethodInvoker GetPrev = new MethodInvoker(GetPrevFlaw);
                    //    this.BeginInvoke(GetPrev);
                    //}
                    //if (next)
                    //{
                    //    next = false;
                    //    MethodInvoker GetNext = new MethodInvoker(GetNextFlaw);
                    //    this.BeginInvoke(GetNext);
                    //}
                    //if (filter_Change)
                    //{
                    //    filter_Change = false;
                    //    MethodInvoker filter_Refresh = new MethodInvoker(filter_Update);
                    //    this.BeginInvoke(filter_Refresh);
                    //}
                    //if (show_update)
                    //{
                    //    show_update = false;
                    //    MethodInvoker show_change = new MethodInvoker(showChange);
                    //    this.BeginInvoke(show_change);
                    //}
                    //if (GetPicID != -1)
                    //{
                    //    getid = GetPicID;
                    //    GetPicID = -1;
                    //    MethodInvoker get_info = new MethodInvoker(GetControlID);
                    //    this.BeginInvoke(get_info);
                    //}
                }
            }
            catch(Exception ex)
            {
                MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.GeneralError, "RefreshCheck() Error!" + ex.Message, null, 0);
            }
        }
        #endregion

        #region Thread Method Functions
        //Deal IWRPlugIn Interface Event 
        public void ProcessJobLoaded()
        {
            //MessageBox.Show("ProcessJobLoaded");
            //DebugTool.WriteLog("PxPTab.cs", "ProcessJobLoaded");
            PxPThreadStatus.IsOnJobLoaded = false;
        }
        public void ProcessJobStarted()
        {
            //工單開始執行之後的動作
            //MessageBox.Show("ProcessJobStarted");
            //DebugTool.WriteLog("PxPTab.cs", "ProcessJobStarted");

            MapWindowVariable.MapWindowController.SetGvFlawClass(PxPVariable.FlawTypeName);
            PxPThreadStatus.IsOnJobStarted = false;
        }
        public void ProcessOnCut()
        {
            //MessageBox.Show("ProcessOnCut");
            //DebugTool.WriteLog("PxPTab.cs", "ProcessOnCut");

            MapWindowVariable.MapWindowController.DrawPieceFlaw(MapWindowVariable.FlawPiece, true);
            //處理右下角圖片
            DrawTablePictures(MapWindowVariable.FlawPieces,MapWindowVariable.CurrentPiece,1);
            //MapWindowVariable.CurrentPiece++;
            /*
            foreach (var i in MapWindowVariable.FlawPiece)
            {
                PictureBox pic = new PictureBox();
                pic.Image = i.Images as Image;
                tlpDoffGrid.Controls.Add(pic);
            }
           */
            PxPThreadStatus.IsOnCut = false;
        }

        /* Avoid IsOnOnline argument wrong.
        public void ProcessOnOnline()
        {
            MessageBox.Show("ProcessOnOnline");
            DebugTool.WriteLog("PxPTab.cs", "ProcessOnOnline");
            PxPThreadStatus.IsOnOnline = false;
        }*/

        public void ProcessOnDoffResult()
        {
            //MessageBox.Show("ProcessOnDoffResult");
            //DebugTool.WriteLog("PxPTab.cs", "ProcessOnDoffResult");

            PxPThreadStatus.IsOnDoffResult = false;
        }
        public void ProcessOnPxPConfig()
        {
            //MessageBox.Show("ProcessOnPxPConfig");
            //DebugTool.WriteLog("PxPTab.cs", "ProcessOnPxPConfig");

            PxPThreadStatus.IsOnPxPConfig = false;
        }
        public void ProcessOnOpenHistory() 
        {
            //MessageBox.Show("ProcessOnOpenHistory");
            //DebugTool.WriteLog("PxPTab.cs", "ProcessOnOpenHistory");
            PxPThreadStatus.IsOnOpenHistory = false;
        }
        public void ProcessOnUnitsChanged()
        {
            //MessageBox.Show("ProcessOnUnitsChanged");
            //DebugTool.WriteLog("PxPTab.cs", "ProcessOnUnitsChanged");

            PxPThreadStatus.IsOnUnitsChanged = false;
        }
        public void ProcessOnLanguageChenged()
        {
            //MessageBox.Show("ProcessOnLanguageChenged");
            //DebugTool.WriteLog("PxPTab.cs", "ProcessOnLanguageChenged");

            PxPThreadStatus.IsOnLanguageChanged = false;
        }
        public void ProcessOnFlaws()
        {
            //MessageBox.Show("ProcessOnFlaws");
            //DebugTool.WriteLog("PxPTab.cs", "ProcessOnFlaws");

            PxPThreadStatus.IsOnFlaws = false;
        }
        public void ProcessOnEvents()
        {
            //MessageBox.Show("ProcessOnEvents");
            //DebugTool.WriteLog("PxPTab.cs", "ProcessOnEvents");

            PxPThreadStatus.IsOnEvents = false;
        }
        public void ProcessOnWebDBConnected()
        {
            //MessageBox.Show("ProcessOnWebDBConnected");
            //DebugTool.WriteLog("PxPTab.cs", "ProcessOnWebDBConnected");
            
            PxPThreadStatus.IsOnWebDBConnected = false;
        }
        public void ProcessOnSync()
        {
            //MessageBox.Show("ProcessOnSync");
            //DebugTool.WriteLog("PxPTab.cs", "ProcessOnSync");

            PxPThreadStatus.IsOnSync = false;
        }
        public void ProcessOnRollResult()
        {
            //MessageBox.Show("ProcessOnRollResult");
            //DebugTool.WriteLog("PxPTab.cs", "ProcessOnRollResult");

            PxPThreadStatus.IsOnRollResult = false;
        }
        public void ProcessOnClassifyFlaw()
        {
            //MessageBox.Show("ProcessOnClassifyFlaw");
            //DebugTool.WriteLog("PxPTab.cs", "ProcessOnClassifyFlaw");

            PxPThreadStatus.IsOnClassifyFlaw = false;
        }
        public void ProcessFireEvent()
        {
            //MessageBox.Show("ProcessFireEvent");
            //DebugTool.WriteLog("PxPTab.cs", "ProcessFireEvent");

            PxPThreadStatus.IsFireEvent = false;
        }
        public void ProcessOnJobStopped()
        {
            //MessageBox.Show("ProcessOnJobStopped");
            //DebugTool.WriteLog("PxPTab.cs", "ProcessOnJobStopped");

            PxPThreadStatus.IsOnJobStopped = false;
        }
        public void ProcessOnGlassEdges()
        {
            //MessageBox.Show("ProcessOnGlassEdges");
            //DebugTool.WriteLog("PxPTab.cs", "ProcessOnGlassEdges");

            PxPThreadStatus.IsOnGlassEdges = false;
        }
        public void ProcessOnUserTermsChanged()
        {
            //MessageBox.Show("ProcessOnUserTermsChanged");
            //DebugTool.WriteLog("PxPTab.cs", "ProcessOnUserTermsChanged");

            PxPThreadStatus.IsOnUserTermsChanged = false;
        }
        public void ProcessOnCognitiveScience()
        {
            //MessageBox.Show("ProcessOnCognitiveScience");
            //DebugTool.WriteLog("PxPTab.cs", "ProcessOnCognitiveScience");

            PxPThreadStatus.IsOnCognitiveScience = false;
        }
        
        #endregion

        #region Action Events
        //操作事件
         //Images Next Grid 
        private void btnNextGrid_Click(object sender, EventArgs e)
        {
            Job.SetOffline();
            //PxPThreadStatus.IsOnCut = false;
            PxPThreadStatus.IsOnOnline = false;
            int Page = (PxPVariable.PageCurrent + 1 > PxPVariable.PageTotal) ? PxPVariable.PageTotal : PxPVariable.PageCurrent + 1;
            DrawTablePictures(MapWindowVariable.FlawPieces, MapWindowVariable.CurrentPiece, Page);
        }
        //Images Previous Grid
        private void btnPrevGrid_Click(object sender, EventArgs e)
        {
            Job.SetOffline();
            //PxPThreadStatus.IsOnCut = false;
            PxPThreadStatus.IsOnOnline = false;
            int Page = (PxPVariable.PageCurrent - 1 < 1) ? 1 : PxPVariable.PageCurrent - 1;
            DrawTablePictures(MapWindowVariable.FlawPieces, MapWindowVariable.CurrentPiece, Page);
        }
        //DataGridView Sort by Column header click
        private void gvFlaw_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Job.SetOffline();
            PxPThreadStatus.IsOnOnline = false;
            // Check which column is selected, otherwise set NewColumn to null.
            if (gvFlaw.Rows.Count > 0)
            {
                DataGridViewColumn NewColumn = gvFlaw.Columns[e.ColumnIndex];
                DataGridViewColumn OlderColumn = gvFlaw.SortedColumn;
                ListSortDirection lsd;
                SortGridViewByColumn(NewColumn.Name);
                DrawTablePictures(MapWindowVariable.FlawPieces, MapWindowVariable.CurrentPiece, 1);
                MapWindowVariable.MapWindowController.DrawPieceFlaw(MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece], false);
            }
                /*
            // Offical Way
           
            // If oldColumn is null, then the DataGridView is not currently sorted.
            if (OlderColumn != null)
            {
                // Sort the same column again, reversing the SortOrder.
                if (OlderColumn == NewColumn &&
                    gvFlaw.SortOrder == SortOrder.Ascending)
                {
                    lsd = ListSortDirection.Descending;
                }
                else
                {
                    // Sort a new column and remove the old SortGlyph.
                    lsd = ListSortDirection.Ascending;
                    OlderColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                }
            }
            else
            {
                lsd = ListSortDirection.Ascending;
            }

            gvFlaw.Sort(NewColumn, lsd);

            NewColumn.HeaderCell.SortGlyphDirection =
                lsd == ListSortDirection.Ascending ?
                SortOrder.Ascending : SortOrder.Descending;
           */
        
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
