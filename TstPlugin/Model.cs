using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WRPlugIn;
using System.Xml.Linq;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using System.Collections;

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
        internal static int PieceTotal;                                                      //紀錄目前Cut幾片
        internal static IList<IFlawTypeName> FlawTypeName = new List<IFlawTypeName>();       //載入工單時先儲存方便各事件處理
        internal static IList<ISeverityInfo> SeverityInfo = new List<ISeverityInfo>();       //嚴重缺點優先順序
        internal static IList<ILaneInfo> LaneInfo = new List<ILaneInfo>();
        internal static IPxPInfo PxPInfo;
        internal static string UnitsXMLPath;
        internal static IJobInfo JobInfo;                                                    //工單資訊
        internal static int JobKey;
        internal static int TotalPiece = 0;                                                  //用來處理目前總共有幾片
        internal static int FreezPiece = 0;                                                  //當OnCut或需要畫面凍結時紀錄當下跑到第幾片Piece
        internal static int ImgRowsSet = 3;                         //ImgGrid設定大小
        internal static int ImgColsSet = 3;
        internal static int PageSize = ImgRowsSet * ImgColsSet;    //右下角TableLayoutPanel 圖片數量
        internal static int PageCurrent = 0;                       //
        internal static int PageTotal = 0;                         //計算當OnCut發生時右下角DataGrid的頁數
        internal static string FlawGridViewOrderColumn = "";       //右上角GridView排序的欄位
        internal static List<DoffGridColumns> DoffGridSetup = new List<DoffGridColumns>();      //紀錄右上角DataGrid欄位左右排序
    }
    public class MapWindowThreadStatus
    {
        public static bool IsRunSetup;
    }
    public class MapWindowVariable
    {
        internal static MapWindow MapWindowController = new MapWindow();
        internal static List<FlawInfoAddPriority> Flaws = new List<FlawInfoAddPriority>();                                          //紀錄OnFlaws整條資訊
        internal static List<FlawInfoAddPriority> FlawPiece = new List<FlawInfoAddPriority>();                                      //暫存單片Piece
        internal static int InitMapWidth;                                                                       //紀錄初始Map寬
        internal static int InitMapHeight;                                                                      //紀錄初始Map高
        internal static bool IsMapInit;                                                                         //紀錄Map是否紀錄初始狀態
        internal static List<List<FlawInfoAddPriority>> FlawPieces = new List<List<FlawInfoAddPriority>>();                         //儲存Piece切割後的所有檔案
        internal static int CurrentPiece = 0 ;                                                                  //儲存左邊目前看到哪片玻璃
        public MapWindowVariable()
        {
            //FlawInfoExtend x = new FlawInfoExtend();
            
        }
    }
    public class SystemVariable
    {
        internal static string ConfigFileName ;                     //儲存XML路徑可自訂義(預設\CPxP\conf\setup.xml)
        internal static e_Language Language = e_Language.English;   //預設為英語
        internal static string FlawLock = "FlawLock";               //OnFlaws & OnCut 鎖定
        internal static int MapProportion = 0;                      //紀錄Map比例 0->1:1, 2->1:1, 2->2:1, 3->4:3, 4->3:4,  5->16:9
        internal static int ShowGridSet = 1;                        //是否顯示格線
        internal static int MapGridSet = 1;                         //選擇使用的Grid間隔依據 0: EachCellSize , 1: EachCellCount
        internal static double MapMDSet = 3;                        //Map Size 的間隔大小
        internal static double MapCDSet = 3;
        internal static int SeriesSet = 0;                          //紀錄選用的紀錄方式
        internal static int BottomAxe = 0;                          //紀錄MD或CD為Bottom Axes 0:CD, 1:MD
        internal static int MDInver = 0;                            //紀錄是否反轉座標軸
        internal static int CDInver = 0;
        internal static int ShowFlag = 0;                           //紀錄顯示項目 0:All, 1:Pass, 2:Fail
       
        internal static bool IsSystemFreez = false;                 //判斷系統現在是否在offline凍結狀態
        
       


        #region 取得設定檔參數Method
        //取得語系檔
        internal static XDocument GetLangXDoc(e_Language lang)
        {
            #region 註解
            /*
             * 需要變更語系的變數或屬性列表
             * 1. 右上方缺陷點DataGridView Columns
             * 2. 左上方Lables
             * 3. 左下方缺陷點分類DataGridView Columns
             * 4. 
             */
            #endregion
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
               Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "/../Parameter Files/CPxP/i18n/";
            string filename = file + selectedFile;
            XDocument xd = XDocument.Load(filename);
            return xd;
        }
        //取得sys_conf/sys.xml 用來設定自訂參數檔 & Grid排序位置
        internal static XDocument GetSysConfXDoc()
        {
            string path = Path.GetDirectoryName(
              Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\..\\Parameter Files\\CPxP\\sys_conf\\";
            string FullFilePath = path + "sys.xml";
            XDocument XD = XDocument.Load(FullFilePath);
            return XD;
        }
        //取得conf底下參數檔可自訂
        internal static XDocument GetConfig()
        {
            LoadSystemConfig();
            string path = Path.GetDirectoryName(
             Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\..\\Parameter Files\\CPxP\\conf\\";
            string FullFilePath = string.Format("{0}{1}.xml", path, SystemVariable.ConfigFileName);
            XDocument XD = XDocument.Load(FullFilePath);
            return XD;
        }
        //載入/sys_conf/sys.xml  ==> 根據有定義語系資料再變更一次參數
        internal static void LoadSystemConfig()
        {
            #region 註解
            /*
             * 開啟程式啟動完執行緒之後,立刻載入相關設定檔,將設定檔的值帶入Model 全域變數中
             * 系統變數包含 : 
             *   1. 載入其他Conf檔的檔名 (因為User可自訂Conf檔)
             *   2. 右上角缺陷DataGridView的欄位左右排列順序
             */
            #endregion
            try
            {
                XDocument XSysConf = GetSysConfXDoc();
                XElement XConfFile = XSysConf.Element("SystemConfig").Element("ConfFile"); //儲存Conf檔名到SystemVariable變數
                IEnumerable<XElement> XDoffGrid = XSysConf.Element("SystemConfig").Element("DoffGrid").Elements("Column"); //自動儲存右上方GridView的排序和欄位Size
                SystemVariable.ConfigFileName = XConfFile.Value + ".xml";
                PxPVariable.DoffGridSetup.Clear();
                foreach (XElement el in XDoffGrid)
                {
                    DoffGridColumns d = new DoffGridColumns(int.Parse(el.Element("Index").Value), el.Attribute("Name").Value, int.Parse(el.Element("Size").Value));
                    PxPVariable.DoffGridSetup.Add(d);
                }
                XElement OrderByColumn = XSysConf.Element("SystemConfig").Element("DoffGrid").Element("OrderBy");
                PxPVariable.FlawGridViewOrderColumn = OrderByColumn.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Load System Config Error : \n" + ex.Message);
            }
            


        }
        //載入/CPxP/conf/setup.xml 或 自定義的設定檔
        //如果該變數會受Conf檔影響,請記得在此補上
        internal static void LoadConfig()
        {
            XDocument XConf = GetConfig();
            XElement SysVariable = XConf.Element("Config").Element("SystemVariable");
            try
            {
                PxPVariable.ImgRowsSet = int.Parse(SysVariable.Element("ImgRowsSet").Value);
                PxPVariable.ImgColsSet = int.Parse(SysVariable.Element("ImgColsSet").Value);
                SystemVariable.MapProportion = int.Parse(SysVariable.Element("MapProportion").Value);
                SystemVariable.ShowGridSet = int.Parse(SysVariable.Element("ShowGridSet").Value);
                SystemVariable.MapGridSet = int.Parse(SysVariable.Element("MapGridSet").Value);
                SystemVariable.MapMDSet = double.Parse(SysVariable.Element("MapMDSet").Value);
                SystemVariable.MapCDSet = double.Parse(SysVariable.Element("MapCDSet").Value);
                SystemVariable.SeriesSet = int.Parse(SysVariable.Element("SeriesSet").Value);
                SystemVariable.BottomAxe = int.Parse(SysVariable.Element("BottomAxe").Value);
                SystemVariable.MDInver = int.Parse(SysVariable.Element("MDInver").Value);
                SystemVariable.CDInver = int.Parse(SysVariable.Element("CDInver").Value);
                PxPVariable.PageSize = PxPVariable.ImgRowsSet * PxPVariable.ImgColsSet;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Initialize Load Config Fail : \n" + ex.Message);
                PxPVariable.ImgRowsSet = 2;
                PxPVariable.ImgColsSet = 2;
                SystemVariable.MapProportion = 0;
                SystemVariable.ShowGridSet = 1;
                SystemVariable.MapGridSet = 1;
                SystemVariable.MapMDSet = 3;
                SystemVariable.MapCDSet = 3;
                SystemVariable.SeriesSet = 0;
                SystemVariable.BottomAxe = 0;
                SystemVariable.MDInver = 0;
                SystemVariable.CDInver = 0;
            }
        }
        #endregion
    }

    public class DoffGridColumns
    {
        public int Index { set; get; }
        public string ColumnName { set; get; }
        public int Width { set; get; }
        public string HeaderText { set; get; }
        public DoffGridColumns()
        { }
        public DoffGridColumns(int index, string column, int width)
        {
            this.Index = index;
            this.ColumnName = column;
            this.Width = width;
        }
    }
    public class FlawInfoAddPriority
    {
        public int FlawID { set; get; }
        public double Area { set; get; }
        public double CD { set; get; }
        public double MD { set; get; }
        public string FlawClass { set; get; }
        public int FlawType { set; get; }
        public System.Collections.Generic.IList<IImageInfo> Images { get; set; }
        public double LeftEdge { set; get; }
        public double RightEdge { set; get; }
        public double Length { set; get; }
        public double Width { set; get; }
        //Add Column
        public int Priority { get; set; }
        public double RMD { get; set; }
        public FlawInfoAddPriority()
        { 
            
        }
        ////////////////////////////////////////////////////////////////////////////////

       
        
    }
    public class Pair
    {
        public string Key { set; get; } 
        public int Value { set; get; }
        public Pair() { }
        public Pair(string k, int v)
        {
            Key = k;
            Value = v;
        }
    }
    #endregion



    

    
}
