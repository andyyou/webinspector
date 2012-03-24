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
        //Degug
        DebugPut bugPut = new DebugPut();
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
            DefineDataGridView(gvFlaw);
            //MessageBox.Show("PxPTab");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "PxPTab-Constructor", "PxPTab.cs", 1); 
            
            //執行緒啟動
            MapThread = new Thread(new ThreadStart(RefreshCheck));
            MapThread.Start();
            PxPThread = new Thread(new ThreadStart(WorkerThread));
            PxPThread.Start();
            
        }
        #endregion

        #region Refactoring
        //定義右上角DataGridView
        void DefineDataGridView(DataGridView dgv)
        {
            bsFlaw.DataSource = MapWindowVariable.Flaws;
            dgv.DataSource = bsFlaw;
            dgv.Columns["FlawID"].HeaderText = "標號";
            dgv.Columns["FlawID"].SortMode = DataGridViewColumnSortMode.Automatic;
            dgv.Columns["FlawClass"].HeaderText = "缺陷分類";
            dgv.Columns["FlawClass"].SortMode = DataGridViewColumnSortMode.Automatic;
            dgv.Columns["CD"].HeaderText = "橫向位置";
            dgv.Columns["CD"].SortMode = DataGridViewColumnSortMode.Automatic;
            dgv.Columns["MD"].HeaderText = "縱向位置";
            dgv.Columns["MD"].SortMode = DataGridViewColumnSortMode.Automatic;
            dgv.Columns["Width"].HeaderText = "寬度";
            dgv.Columns["Width"].SortMode = DataGridViewColumnSortMode.Automatic;
            dgv.Columns["Length"].HeaderText = "高度";
            dgv.Columns["Length"].SortMode = DataGridViewColumnSortMode.Automatic;
            dgv.Columns["Area"].HeaderText = "面積";
            dgv.Columns["Area"].SortMode = DataGridViewColumnSortMode.Automatic;
            dgv.Columns["FlawType"].HeaderText = "型態";
            dgv.Columns["FlawType"].SortMode = DataGridViewColumnSortMode.Automatic;
            dgv.Columns["Images"].Visible = false;
            dgv.Columns["LeftEdge"].Visible = false;
            dgv.Columns["RightEdge"].Visible = false;
        }
        //取得語系檔
        XDocument GetLangXDoc(e_Language lang)
        {
            string selectedFile = "";
            switch (lang)
            {
                case e_Language.Chinese:
                    selectedFile = "zh.xml";
                    break;
                case e_Language.English:
                    selectedFile = "en.xml";
                    break;
                case e_Language.German:
                    selectedFile = "de.xml";
                    break;
                case e_Language.Korean:
                    selectedFile = "ko.xml";
                    break;
                default:
                    selectedFile = "en.xml";
                    break;
            }
            string file = Path.GetDirectoryName(
               Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\..\\Parameter Files\\PXP\\Language\\";
            string filename = file + selectedFile;
            XDocument xd = XDocument.Load(filename);
            return xd;
        }
        //取得Stepup.xml 用來設定ImgGridRows,Columns
        XDocument GetSetupXDoc()
        {
            string file = Path.GetDirectoryName(
              Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\..\\Parameter Files\\PXP\\Setup\\";
            string filename = file + "Setup.xml";
            XDocument xd = XDocument.Load(filename);
            return xd;
        }
        //更新頁面,該換圖或Map調整
        void PageRefresh()
        {
        }
        //繪製TableLayoutPanel
        void DrawTableLayout()
        {
            //if (gvFlaw.Rows.Count > 0) //追加判斷現在頁面第幾片
            //{
            //    for (int i = 0; i < count; i++)
            //    {
            //        int flag = ((current_Page - 1) * count) + i;
            //        if (flag >= dgvPXP.Rows.Count) break;
            //        flag = int.Parse(dgvPXP.Rows[flag].Cells["indexCol"].Value.ToString());
            //        if (flag < flawList[flag2].Count)
            //        {
            //            FlawInfoControl fic = new FlawInfoControl(flawList[flag2][flag], jobinfo.NumberOfStations);
            //            tableLayoutPanel1.Controls.Add(fic);
            //            fic.Dock = DockStyle.Fill;
            //        }

            //    }
            //}
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
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "IWRPlugIn-GetControlHandle", "PxPTab.cs", 1); 
            bugPut.WriteLog("PxPTab.cs", "IWRPlugIn-GetControlHandle", null);
            //DebugPut.DebugMethod(); //改用static
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
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "IWRPlugIn-SetPosition", "PxPTab.cs", 1); 
            bugPut.WriteLog("PxPTab.cs", "IWRPlugIn-SetPosition", null);
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
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "IWRPlugIn-GetName", "PxPTab.cs", 1); 
            bugPut.WriteLog("PxPTab.cs", "IWRPlugIn-GetName", null);
            switch (lang)
            {
                case e_Language.Chinese:
                    name = "安迪您好";
                    break;
                default:
                    name = "HelloAndy";
                    break;
            }
        }

        /// <summary>
        /// 卸載Plugin
        /// </summary>
        public void Unplug()
        {
            //MessageBox.Show("IWRPlugIn-Unplug");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "IWRPlugIn-Unplug", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "IWRPlugIn-Unplug", null);
            //trigger on close the window
        }

        #endregion

        #region IWRMapWindow 成員
        public void GetMapControlHandle(out IntPtr hndl)
        {
            MessageBox.Show("IWRMapWindow-GetMapControlHandle");
            hndl = MapWindowVariable.MapWindowController.Handle;
        }

        public void SetMapPosition(int w, int h)
        {
            MessageBox.Show("IWRMapWindow-SetMapPosition");
            MapWindowVariable.MapWindowController.SetBounds(0, 0, w, h);
        }

        #endregion

        #region IOnFlaws 成員
        public void OnFlaws(IList<IFlawInfo> flaws)
        {
            //MessageBox.Show("IOnFlaws");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "OnFlaws", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "OnFlaws", null);
            try
            {
                MapWindowVariable.Flaws.AddRange(flaws);
                bsFlaw.ResetBindings(false);
                bsFlaw.ResumeBinding();
                gvFlaw.FirstDisplayedScrollingRowIndex = gvFlaw.Rows.Count - 1;
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
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "OnEvents", "PxPTab.cs", 1); 
            bugPut.WriteLog("PxPTab.cs", "OnEvents", null);
            PxPThreadStatus.IsOnEvents = true;
            PxPThreadEvent.Set();
        }

        #endregion
        
        #region IOnCut 成員

        public void OnCut(double md)
        {
            //MessageBox.Show("OnCut");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "OnCut", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "OnCut", null);
            MapWindowVariable.FlawPiece.Clear();
            foreach (var f in MapWindowVariable.Flaws)
            {
                if (f.MD > PxPVariable.CurrentCutPosition + 3 || f.MD < 3)
                    MapWindowVariable.FlawPiece.Add(f);
            }
            PxPVariable.CurrentCutPosition = md;
            PxPThreadStatus.IsOnCut = true;
            PxPThreadEvent.Set();
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
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "OnJobLoaded", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "OnJobLoaded", null);

            PxPVariable.FlawTypeName.Clear();
            PxPVariable.FlawTypeName = flawTypes;
            PxPVariable.JobInfo = jobInfo;
            PxPThreadStatus.IsOnJobLoaded = true;
            PxPThreadEvent.Set();
        }
        #endregion

        #region IOnJobStarted 成員

        public void OnJobStarted(int jobKey)
        {
            //MessageBox.Show("OnJobStarted");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "OnJobStarted", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "OnJobStarted", null);

            PxPVariable.JobKey = jobKey;
            PxPThreadStatus.IsOnJobStarted = true;
            PxPThreadEvent.Set();
        }
        #endregion

        #region IOnLanguageChanged 成員

        public void OnLanguageChanged(e_Language language)
        {
            //MessageBox.Show("OnLanguageChanged");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "OnLanguageChanged", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "OnLanguageChanged", null);

            SystemVariable.Language = language;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnJobStopped 成員

        public void OnJobStopped(double md)
        {
            //MessageBox.Show("OnJobStopped");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "OnJobStopped", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "OnJobStopped", null);

            PxPThreadStatus.IsOnJobStopped = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnWebDBConnected 成員

        public void OnWebDBConnected(IWebDBConnectionInfo info)
        {
            //MessageBox.Show("OnWebDBConnected");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "OnWebDBConnected", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "OnWebDBConnected", null);

            PxPThreadStatus.IsOnWebDBConnected = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnSync 成員

        public void OnSync(double md)
        {
            //MessageBox.Show("OnSync");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "OnSync", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "OnSync", null);

            PxPThreadStatus.IsOnSync = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnGlassEdges 成員

        public void OnGlassEdges(double md, double le1, double le2, double le3, double leftROI, double rightROI, double re3, double re2, double re1)
        {
            //MessageBox.Show("OnGlassEdges");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "OnGlassEdges", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "OnGlassEdges", null);

            PxPThreadStatus.IsOnGlassEdges = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnOnline 成員

        public void OnOnline(bool isOnline)
        {
            //MessageBox.Show("OnOnline");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "OnOnline", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "OnOnline", null);

            PxPThreadStatus.IsOnOnline = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnUserTermsChanged 成員

        public void OnUserTermsChanged(IUserTerms terms)
        {
            //MessageBox.Show("OnUserTermsChanged");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "OnUserTermsChanged", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "OnUserTermsChanged", null);

            PxPThreadStatus.IsOnUserTermsChanged = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnDoffResult 成員

        public void OnDoffResult(double md, int doffNumber, bool pass)
        {
            //MessageBox.Show("OnDoffResult");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "OnDoffResult", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "OnDoffResult", null);

            PxPThreadStatus.IsOnDoffResult = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnPxPConfig 成員

        public void OnPxPConfig(IPxPInfo info, string unitsXMLPath)
        {
            //MessageBox.Show("OnPxPConfig");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "OnPxPConfig", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "OnPxPConfig", null);

            PxPThreadStatus.IsOnPxPConfig = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnRollResult 成員

        public void OnRollResult(double cd, double md, int doffNumber, int laneNumber, bool pass)
        {
            //MessageBox.Show("OnRollResult");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "OnRollResult", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "OnRollResult", null);

            PxPThreadStatus.IsOnRollResult = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnOpenHistory 成員

        public void OnOpenHistory(double startMD, double stopMD)
        {
            //MessageBox.Show("OnOpenHistory");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "OnOpenHistory", "PxPTab.cs", 1); 
            bugPut.WriteLog("PxPTab.cs", "OnOpenHistory", null);

            PxPThreadStatus.IsOnOpenHistory = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IWRFireEvent 成員

        public void FireEvent(int eventID, double cd, double md)
        {
            //MessageBox.Show("FireEvent");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "FireEvent", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "FireEvent", null);

            PxPThreadStatus.IsFireEvent = true;
            PxPThreadEvent.Set();

        }

        #endregion

        #region IOnClassifyFlaw 成員
        public void OnClassifyFlaw(ref WRPlugIn.IFlawInfo flaw, ref bool deleteFlaw)
        {
            //MessageBox.Show("OnClassifyFlaw");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "OnClassifyFlaw", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "OnClassifyFlaw", null);

            PxPThreadStatus.IsOnClassifyFlaw = true;
            PxPThreadEvent.Set();
        }
        #endregion

        #region IOnUnitsChanged 成員

        public void OnUnitsChanged()
        {
            //MessageBox.Show("OnUnitsChanged");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "OnUnitsChanged", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "OnUnitsChanged", null);

            PxPThreadStatus.IsOnUnitsChanged = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnCognitiveScience 成員

        public void OnCognitiveScience(Bitmap webImage, int leftEdge, int rightEdge, double md, double scaleFactor)
        {
            //MessageBox.Show("OnCognitiveScience");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "OnCognitiveScience", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "OnCognitiveScience", null);

            PxPThreadStatus.IsOnCognitiveScience = true;
            PxPThreadEvent.Set();
        }

        #endregion
        #endregion

        #region Thread Method
        //主要執行緒,負責處理繼承的介面,接收狀態值切換動作
        private void WorkerThread()
        {
            //string[] debugEvents = { "BoolOnShutdown", "BoolOnLanguageChanged", "BoolOnJobLoaded", "BoolOnJobStarted", "BoolOnOnline", "BoolOnFlaws", "BoolOnCut", "BoolOnEvents", "BoolOnPxPConfig", "BoolOnWebDBConnected", "BoolOnSync", "BoolOnDoffResult", "BoolOnRollResult", "BoolOnOpenHistory", "BoolOnClassifyFlaw", "BoolFireEvent", "BoolOnUnitsChanged", "BoolOnJobStopped", "IsOnGlassEdges", "BoolOnUserTermsChanged", "BoolOnCognitiveScience" };
            //bool?[] debugValues = new bool?[21];
            try
            {
                while (true)
                {
                    PxPThreadEvent.WaitOne();
                    //<Debgu>
                    //debugValues[0] = PxPThreadStatus.IsOnShutdown;
                    //debugValues[1] = PxPThreadStatus.IsOnLanguageChanged;
                    //debugValues[2] = PxPThreadStatus.IsOnJobLoaded;
                    //debugValues[3] = PxPThreadStatus.IsOnJobStarted;
                    //debugValues[4] = PxPThreadStatus.IsOnOnline;
                    //debugValues[5] = PxPThreadStatus.IsOnFlaws;
                    //debugValues[6] = PxPThreadStatus.IsOnCut;
                    //debugValues[7] = PxPThreadStatus.IsOnEvents;
                    //debugValues[8] = PxPThreadStatus.IsOnPxPConfig;
                    //debugValues[9] = PxPThreadStatus.IsOnWebDBConnected;
                    //debugValues[10] = PxPThreadStatus.IsOnSync;
                    //debugValues[11] = PxPThreadStatus.IsOnDoffResult;
                    //debugValues[12] = PxPThreadStatus.IsOnRollResult;
                    //debugValues[13] = PxPThreadStatus.IsOnOpenHistory;
                    //debugValues[14] = PxPThreadStatus.IsOnClassifyFlaw;
                    //debugValues[15] = PxPThreadStatus.IsFireEvent;
                    //debugValues[16] = PxPThreadStatus.IsOnUnitsChanged;
                    //debugValues[17] = PxPThreadStatus.IsOnJobStopped;
                    //debugValues[18] = PxPThreadStatus.IsOnGlassEdges;
                    //debugValues[19] = PxPThreadStatus.IsOnUserTermsChanged;
                    //debugValues[20] = PxPThreadStatus.IsOnCognitiveScience;
                        

                    //</Debgu>


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
                    if (PxPThreadStatus.IsOnOnline)
                    {
                        PxPThreadStatus.IsOnOnline = false;
                        MethodInvoker Online = new MethodInvoker(ProcessOnOnline);
                        this.BeginInvoke(Online);
                    }

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
                    //<dbug>
                    //for (int i = 0; i < 21; i++)
                    //{
                    //    string x = string.Format("{0}:{1}", debugEvents[i].ToString(), debugValues[i].ToString());
                    //    //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, x  , "PxPTab.cs", 1);
                    //    bugPut.WriteLog("PxPTab.cs", debugEvents[i].ToString(), debugValues[i]);
                    //}
                    //</dbug>
                    

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
                    //if (m_setting)
                    //{
                    //    m_setting = false;
                    //    MethodInvoker SetThread = new MethodInvoker(SetupConfig);
                    //    this.BeginInvoke(SetThread);
                    //}
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
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "ProcessJobLoaded", "PxPTab.cs", 1); 
            bugPut.WriteLog("PxPTab.cs", "ProcessJobLoaded", null);

            PxPThreadStatus.IsOnJobLoaded = false;
        }
        public void ProcessJobStarted()
        {
            //工單開始執行之後的動作
            //MessageBox.Show("ProcessJobStarted");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "ProcessJobStarted", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "ProcessJobStarted", null);

            MapWindowVariable.MapWindowController.SetGvFlawClass(PxPVariable.FlawTypeName);
            PxPThreadStatus.IsOnJobStarted = false;
        }
        public void ProcessOnCut()
        {
            //處理Cut
            //MessageBox.Show("ProcessOnCut");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "ProcessOnCut", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "ProcessOnCut", null);

            MapWindowVariable.MapWindowController.DrawPieceFlaw(MapWindowVariable.FlawPiece);
            PxPThreadStatus.IsOnCut = false;
        }
        public void ProcessOnOnline()
        {
            //MessageBox.Show("ProcessOnOnline");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "ProcessOnOnline", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "ProcessOnOnline", null);

            //在這裡先測試畫圖
            PxPThreadStatus.IsOnOnline = false;
        }
        public void ProcessOnDoffResult()
        {
            //MessageBox.Show("ProcessOnDoffResult");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "ProcessOnDoffResult", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "ProcessOnDoffResult", null);

            PxPThreadStatus.IsOnDoffResult = false;
        }
        public void ProcessOnPxPConfig()
        {
            //MessageBox.Show("ProcessOnPxPConfig");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "ProcessOnPxPConfig", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "ProcessOnPxPConfig", null);

            PxPThreadStatus.IsOnPxPConfig = false;
        }
        public void ProcessOnOpenHistory() 
        {
            //MessageBox.Show("ProcessOnOpenHistory");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "ProcessOnOpenHistory", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "ProcessOnOpenHistory", null);
            
            PxPThreadStatus.IsOnOpenHistory = false;
        }
        public void ProcessOnUnitsChanged()
        {
            //MessageBox.Show("ProcessOnUnitsChanged");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "ProcessOnUnitsChanged", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "ProcessOnUnitsChanged", null);
            
            PxPThreadStatus.IsOnUnitsChanged = false;
        }
        public void ProcessOnLanguageChenged()
        {
            //MessageBox.Show("ProcessOnLanguageChenged");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "ProcessOnLanguageChenged", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "ProcessOnLanguageChenged", null);
            
            PxPThreadStatus.IsOnLanguageChanged = false;
        }
        public void ProcessOnFlaws()
        {
            //MessageBox.Show("ProcessOnFlaws");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "ProcessOnFlaws", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "ProcessOnFlaws", null);

            PxPThreadStatus.IsOnFlaws = false;
        }
        public void ProcessOnEvents()
        {
            //MessageBox.Show("ProcessOnEvents");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "ProcessOnEvents", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "ProcessOnEvents", null);

            PxPThreadStatus.IsOnEvents = false;
        }
        public void ProcessOnWebDBConnected()
        {
            //MessageBox.Show("ProcessOnWebDBConnected");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "ProcessOnWebDBConnected", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "ProcessOnWebDBConnected", null);
            
            PxPThreadStatus.IsOnWebDBConnected = false;
        }
        public void ProcessOnSync()
        {
            //MessageBox.Show("ProcessOnSync");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "ProcessOnSync", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "ProcessOnSync", null);


            PxPThreadStatus.IsOnSync = false;
        }
        public void ProcessOnRollResult()
        {
            //MessageBox.Show("ProcessOnRollResult");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "ProcessOnRollResult", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "ProcessOnRollResult", null);

            PxPThreadStatus.IsOnRollResult = false;
        }
        public void ProcessOnClassifyFlaw()
        {
            //MessageBox.Show("ProcessOnClassifyFlaw");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "ProcessOnClassifyFlaw", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "ProcessOnClassifyFlaw", null);

            PxPThreadStatus.IsOnClassifyFlaw = false;
        }
        public void ProcessFireEvent()
        {
            //MessageBox.Show("ProcessFireEvent");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "ProcessFireEvent", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "ProcessFireEvent", null);

            PxPThreadStatus.IsFireEvent = false;
        }
        public void ProcessOnJobStopped()
        {
            //MessageBox.Show("ProcessOnJobStopped");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "ProcessOnJobStopped", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "ProcessOnJobStopped", null);

            PxPThreadStatus.IsOnJobStopped = false;
        }
        public void ProcessOnGlassEdges()
        {
            //MessageBox.Show("ProcessOnGlassEdges");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "ProcessOnGlassEdges", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "ProcessOnGlassEdges", null);

            PxPThreadStatus.IsOnGlassEdges = false;
        }
        public void ProcessOnUserTermsChanged()
        {
            //MessageBox.Show("ProcessOnUserTermsChanged");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "ProcessOnUserTermsChanged", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "ProcessOnUserTermsChanged", null);

            PxPThreadStatus.IsOnUserTermsChanged = false;
        }
        public void ProcessOnCognitiveScience()
        {
            //MessageBox.Show("ProcessOnCognitiveScience");
            //MsgLog.Log(e_LogID.MessageLog, e_LogVisibility.DebugMessage, "ProcessOnCognitiveScience", "PxPTab.cs", 1);
            bugPut.WriteLog("PxPTab.cs", "ProcessOnCognitiveScience", null);

            PxPThreadStatus.IsOnCognitiveScience = false;
        }
        
        #endregion

        #region Action Events
        //操作事件
        #endregion




        
    }
    
    
}
