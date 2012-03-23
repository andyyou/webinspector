﻿using System;
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
            MessageBox.Show("PxPTab");
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
            MessageBox.Show("IWRPlugIn-GetControlHandle");
            hndl = Handle;
        }
        /// <summary>
        /// 設定控制項顯示的X,Y位置座標及寬高
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public void SetPosition(int w, int h)
        {
            MessageBox.Show("IWRPlugIn-SetPosition");
            SetBounds(0, 0, w, h); //Default : w760,h747
        }

        /// <summary>
        /// 主畫面透過GetName根據語系取得Tab名稱,在Plugin內部則是透過out 設定傳入變數
        /// </summary>
        /// <param name="lang"></param>
        /// <param name="name"></param>
        public void GetName(e_Language lang, out string name)
        {
            MessageBox.Show("IWRPlugIn-GetName");
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
            MessageBox.Show("IWRPlugIn-Unplug");
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
            MessageBox.Show("IOnFlaws");
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
            MessageBox.Show("OnEvents");
            PxPThreadStatus.IsOnEvents = true;
            PxPThreadEvent.Set();
        }

        #endregion
        
        #region IOnCut 成員

        public void OnCut(double md)
        {
            MessageBox.Show("OnCut");
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
            MessageBox.Show("OnJobLoaded");
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
            MessageBox.Show("OnJobStarted");
            PxPVariable.JobKey = jobKey;
            PxPThreadStatus.IsOnJobStarted = true;
            PxPThreadEvent.Set();
        }
        #endregion

        #region IOnLanguageChanged 成員

        public void OnLanguageChanged(e_Language language)
        {
            MessageBox.Show("OnLanguageChanged");
            SystemVariable.Language = language;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnJobStopped 成員

        public void OnJobStopped(double md)
        {
            MessageBox.Show("OnJobStopped");
            PxPThreadStatus.IsOnJobStopped = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnWebDBConnected 成員

        public void OnWebDBConnected(IWebDBConnectionInfo info)
        {
            MessageBox.Show("OnWebDBConnected");
            PxPThreadStatus.IsOnWebDBConnected = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnSync 成員

        public void OnSync(double md)
        {
            MessageBox.Show("OnSync");
            PxPThreadStatus.IsOnSync = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnGlassEdges 成員

        public void OnGlassEdges(double md, double le1, double le2, double le3, double leftROI, double rightROI, double re3, double re2, double re1)
        {
            MessageBox.Show("OnGlassEdges");
            PxPThreadStatus.IsOnGlassEdges = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnOnline 成員

        public void OnOnline(bool isOnline)
        {
            MessageBox.Show("OnOnline");
            PxPThreadStatus.IsOnOnline = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnUserTermsChanged 成員

        public void OnUserTermsChanged(IUserTerms terms)
        {
            MessageBox.Show("OnUserTermsChanged");
            PxPThreadStatus.IsOnUserTermsChanged = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnDoffResult 成員

        public void OnDoffResult(double md, int doffNumber, bool pass)
        {
            MessageBox.Show("OnDoffResult");
            PxPThreadStatus.IsOnDoffResult = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnPxPConfig 成員

        public void OnPxPConfig(IPxPInfo info, string unitsXMLPath)
        {
            MessageBox.Show("OnPxPConfig");
            PxPThreadStatus.IsOnPxPConfig = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnRollResult 成員

        public void OnRollResult(double cd, double md, int doffNumber, int laneNumber, bool pass)
        {
            MessageBox.Show("OnRollResult");
            PxPThreadStatus.IsOnRollResult = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnOpenHistory 成員

        public void OnOpenHistory(double startMD, double stopMD)
        {
            MessageBox.Show("OnOpenHistory");
            PxPThreadStatus.IsOnOpenHistory = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IWRFireEvent 成員

        public void FireEvent(int eventID, double cd, double md)
        {
            MessageBox.Show("FireEvent");
            PxPThreadStatus.IsFireEvent = true;
            PxPThreadEvent.Set();

        }

        #endregion

        #region IOnClassifyFlaw 成員
        public void OnClassifyFlaw(ref WRPlugIn.IFlawInfo flaw, ref bool deleteFlaw)
        {
            MessageBox.Show("OnClassifyFlaw");
            PxPThreadStatus.IsOnClassifyFlaw = true;
            PxPThreadEvent.Set();
        }
        #endregion

        #region IOnUnitsChanged 成員

        public void OnUnitsChanged()
        {
            MessageBox.Show("OnUnitsChanged");
            PxPThreadStatus.IsOnUnitsChanged = true;
            PxPThreadEvent.Set();
        }

        #endregion

        #region IOnCognitiveScience 成員

        public void OnCognitiveScience(Bitmap webImage, int leftEdge, int rightEdge, double md, double scaleFactor)
        {
            MessageBox.Show("OnCognitiveScience");
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
            MessageBox.Show("ProcessJobLoaded");
            PxPThreadStatus.IsOnJobLoaded = false;
        }
        public void ProcessJobStarted()
        {
            //工單開始執行之後的動作
            MessageBox.Show("ProcessJobStarted");
            MapWindowVariable.MapWindowController.SetGvFlawClass(PxPVariable.FlawTypeName);
            PxPThreadStatus.IsOnJobStarted = false;
        }
        public void ProcessOnCut()
        {
            //處理Cut
            MessageBox.Show("ProcessOnCut");
            MapWindowVariable.MapWindowController.DrawPieceFlaw(MapWindowVariable.FlawPiece);
            PxPThreadStatus.IsOnCut = false;
        }
        public void ProcessOnOnline()
        {
            MessageBox.Show("ProcessOnOnline");
            //在這裡先測試畫圖
            PxPThreadStatus.IsOnOnline = false;
        }
        public void ProcessOnDoffResult()
        {
            MessageBox.Show("ProcessOnDoffResult");
            PxPThreadStatus.IsOnDoffResult = false;
        }
        public void ProcessOnPxPConfig()
        {
            MessageBox.Show("ProcessOnPxPConfig");
            PxPThreadStatus.IsOnPxPConfig = false;
        }
        public void ProcessOnOpenHistory() 
        {
            MessageBox.Show("ProcessOnOpenHistory");
            PxPThreadStatus.IsOnOpenHistory = false;
        }
        public void ProcessOnUnitsChanged()
        {
            MessageBox.Show("ProcessOnUnitsChanged");
            PxPThreadStatus.IsOnUnitsChanged = false;
        }
        public void ProcessOnLanguageChenged()
        {
            MessageBox.Show("ProcessOnLanguageChenged");
            PxPThreadStatus.IsOnLanguageChanged = false;
        }
        public void ProcessOnFlaws()
        {
            MessageBox.Show("ProcessOnFlaws");
            PxPThreadStatus.IsOnFlaws = false;
        }
        public void ProcessOnEvents()
        {
            MessageBox.Show("ProcessOnEvents");
            PxPThreadStatus.IsOnEvents = false;
        }
        public void ProcessOnWebDBConnected()
        {
            MessageBox.Show("ProcessOnWebDBConnected");
            PxPThreadStatus.IsOnWebDBConnected = false;
        }
        public void ProcessOnSync()
        {
            MessageBox.Show("ProcessOnSync");
            PxPThreadStatus.IsOnSync = false;
        }
        public void ProcessOnRollResult()
        {
            MessageBox.Show("ProcessOnRollResult");
            PxPThreadStatus.IsOnRollResult = false;
        }
        public void ProcessOnClassifyFlaw()
        {
            MessageBox.Show("ProcessOnClassifyFlaw");
            PxPThreadStatus.IsOnClassifyFlaw = false;
        }
        public void ProcessFireEvent()
        {
            MessageBox.Show("ProcessFireEvent");
            PxPThreadStatus.IsFireEvent = false;
        }
        public void ProcessOnJobStopped()
        {
            MessageBox.Show("ProcessOnJobStopped");
            PxPThreadStatus.IsOnJobStopped = false;
        }
        public void ProcessOnGlassEdges()
        {
            MessageBox.Show("ProcessOnGlassEdges");
            PxPThreadStatus.IsOnGlassEdges = false;
        }
        public void ProcessOnUserTermsChanged()
        {
            MessageBox.Show("ProcessOnUserTermsChanged");
            PxPThreadStatus.IsOnUserTermsChanged = false;
        }
        public void ProcessOnCognitiveScience()
        {
            MessageBox.Show("ProcessOnCognitiveScience");
            PxPThreadStatus.IsOnCognitiveScience = false;
        }
        
        #endregion

        #region Action Events
        //操作事件
        #endregion




        
    }
    
    
}
