using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WRPlugIn;

namespace PxP
{
    #region Variable Model Class
    public class PxPThreadStatus
    {
        public static bool IsOnShutdown;   //關閉
        public static bool IsOnCut;        //裁切
        public static bool IsOnJobStarted; //工單開始執行
        public static bool IsOnJobLoaded;  //工單載入完成
        public static bool IsOnJobStopped; //工單停止
        public static bool IsOnOnline;     //上載工單完成之後進入可啟動狀態
        public static bool IsOnFlaws;      //開始跑Flaws
        public static bool IsOnEvents;
        public static bool IsOnLanguageChanged;
        public static bool IsOnWebDBConnected;
        public static bool IsOnSync;
        public static bool IsOnGlassEdges;
        public static bool IsOnUserTermsChanged;
        public static bool IsOnDoffResult;
        public static bool IsOnPxPConfig;
        public static bool IsOnRollResult;
        public static bool IsOnOpenHistory;
        public static bool IsOnClassifyFlaw;
        public static bool IsFireEvent;
        public static bool IsOnUnitsChanged;
        public static bool IsOnCognitiveScience;


        //初始化全部狀態都關閉
        public PxPThreadStatus()
        {
            IsOnShutdown = false;
            IsOnCut = false;
            IsOnJobStarted = false;
            IsOnJobLoaded = false;
            IsOnJobStopped = false;
            IsOnOnline = false;
            IsOnFlaws = false;
            IsOnEvents = false;
            IsOnLanguageChanged = false;
            IsOnWebDBConnected = false;
            IsOnSync = false;
            IsOnGlassEdges = false;
            IsOnUserTermsChanged = false;
            IsOnDoffResult = false;
            IsOnPxPConfig = false;
            IsOnRollResult = false;
            IsOnOpenHistory = false;
            IsOnClassifyFlaw = false;
            IsFireEvent = false;
            IsOnUnitsChanged = false;
            IsOnCognitiveScience = false;
        }

    }
    public class PxPVariable
    {
        internal static double CurrentCutPosition = 0;                                       //紀錄目前裁切位置
        internal static int PieceLimit;                                                      //Piece限制數量
        internal static IList<IFlawTypeName> FlawTypeName = new List<IFlawTypeName>();       //載入工單時先儲存方便各事件處理
        internal static IList<ISeverityInfo> SeverityInfo = new List<ISeverityInfo>();       //嚴重缺點優先順序
        internal static IList<ILaneInfo> LaneInfo = new List<ILaneInfo>();
        internal static IJobInfo JobInfo;                                                    //工單資訊
        internal static int JobKey;
    }
    public class MapWindowThreadStatus
    {
        public static bool IsRunSetup;
    }
    public class MapWindowVariable
    {
        internal static MapWindow MapWindowController = new MapWindow();
        internal static List<IFlawInfo> Flaws = new List<IFlawInfo>();                                          //紀錄OnFlaws整條資訊
        internal static List<IFlawInfo> FlawPiece = new List<IFlawInfo>();                                      //暫存單片Piece
        internal static int InitMapWidth;                                                                       //紀錄初始Map寬
        internal static int InitMapHeight;                                                                      //紀錄初始Map高
        internal static bool IsMapInit;                                                                         //紀錄Map是否紀錄初始狀態
        internal static List<List<IFlawInfo>> FlawPieces = new List<List<IFlawInfo>>();                         //儲存Piece切割後的所有檔案
    }
    public class SystemVariable
    {
        internal static e_Language Language = e_Language.English;   //預設為英語
        internal static string FlawLock = "FlawLock";               //OnFlaws & OnCut 鎖定
        internal static int ImgRowsSet = 2;                         //ImgGrid設定大小
        internal static int ImgColsSet = 2;
        internal static int MapSizeSet = 0;                         //紀錄Map比例 0->1:1, 2->1:1, 2->2:1, 3->4:3, 4->3:4,  5->16:9
        internal static int ShowGridSet = 1;                        //是否顯示格線
        internal static int MapGridSet = 1;                         //選擇使用的Grid間隔依據 0: EachCellSize , 1: EachCellCount
        internal static double MapMDSet = 3;                        //Map Size 的間隔大小
        internal static double MapCDSet = 3;
        internal static int SeriesSet = 0;                          //紀錄選用的紀錄方式
        internal static int BottomAxe = 0;                          //紀錄MD或CD為Bottom Axes 0:CD, 1:MD
        internal static int MDInver = 0;                            //紀錄是否反轉座標軸
        internal static int CDInver = 0;
        internal static int ShowFlag = 0;                           //紀錄顯示項目 0:All, 1:Pass, 2:Fail

    }
    #endregion
}
