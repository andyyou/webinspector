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
using System.Data;

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
        internal static int PieceLimit = 20;                                                      //Piece限制數量
        internal static int PieceTotal;                                                      //紀錄目前Cut幾片
        internal static List<FlawTypeNameExtend> FlawTypeName = new List<FlawTypeNameExtend>();       //載入工單時先儲存方便各事件處理
        internal static List<FlawTypeNameExtend> TmpFlawTypeNameForSetup = new List<FlawTypeNameExtend>();       //載入工單時先儲存方便各事件處理
        internal static IList<ISeverityInfo> SeverityInfo = new List<ISeverityInfo>();       //嚴重缺點優先順序
        internal static IList<ILaneInfo> LaneInfo = new List<ILaneInfo>();
        internal static List<FlawLegend> FlawLegend = new List<FlawLegend>();
        internal static IPxPInfo PxPInfo;
        internal static double PxPHeight = 0;
        internal static double PxPWidth = 0;
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
        
        internal static int ChooseFlawID = -1;
        internal static int PassNum = 0;
        internal static int FailNum = 0;
        internal static int DoffNum = 0;

        public static Dictionary<string, int> UnitsKeys = new Dictionary<string,int>();
        public static DataSet UnitsData ;

    }
    public class MapWindowThreadStatus
    {
        public static bool IsRunSetup = false;
        public static bool StopMapThreading = false;
        public static bool IsTableLayoutRefresh= false;
        public static bool IsPageRefresh = false;
        public static bool UpdateChange = false;
        public static bool IsChangePiece = false;
       
    }
    public class MapWindowVariable
    {
        internal static MapWindow MapWindowController = new MapWindow();
        internal static List<FlawInfoAddPriority> Flaws = new List<FlawInfoAddPriority>();                                          //紀錄OnFlaws整條資訊
        internal static List<FlawInfoAddPriority> FlawPiece = new List<FlawInfoAddPriority>();                                      //暫存單片Piece
        internal static int InitMapWidth;                                                                       //紀錄初始Map寬
        internal static int InitMapHeight;                                                                      //紀錄初始Map高
        internal static bool IsMapInit;                                                                         //紀錄Map是否紀錄初始狀態
        internal static List<IList<IFlawInfo>> OriginFlawPieces = new List<IList<IFlawInfo>>();                   //儲存原始資料已用來變更單位計算
        internal static List<List<FlawInfoAddPriority>> FlawPieces = new List<List<FlawInfoAddPriority>>();     //儲存Piece切割後的所有檔案
        internal static int CurrentPiece = 0 ;                                                                  //儲存左邊目前看到哪片玻璃
        internal static int MapProportion = 0;                      //紀錄Map比例 0->1:1, 2->1:1, 2->2:1, 3->4:3, 4->3:4,  5->16:9
        internal static bool ShowGridSet = true;                    //是否顯示格線
        internal static int MapGridSet = 1;                         //選擇使用的Grid間隔依據 0: EachCellSize , 1: EachCellCount
        internal static double MapMDSet = 3;                        //Map Size 的間隔大小
        internal static double MapCDSet = 3;
        internal static int SeriesSet = 0;                          //紀錄選用的紀錄方式 0->Sharp , 1->Letter
        internal static int BottomAxe = 0;                          //紀錄MD或CD為Bottom Axes 0:CD, 1:MD
        internal static int MDInver = 0;                            //紀錄是否反轉座標軸
        internal static int CDInver = 0;
        internal static int ShowFlag = 0;                           //紀錄顯示項目 0:All, 1:Pass, 2:Fail
        internal static List<DoffGridColumns> DoffTypeGridSetup;    //紀錄左下角DataGrid欄位左右排序
        internal static Dictionary<int, bool> PieceResult = new Dictionary<int, bool>();          //紀錄每片玻璃的檢測結果
        internal static double LastMapMDConvertion;                 //紀錄上一次MD單位變更的比例
        internal static double LastMapCDConvertion;                 //紀錄上一次CD單位變更的比例
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
        internal static bool IsReadHistory = false;                 //判斷是否讀取歷史紀錄
        internal static bool IsSystemFreez = false;                 //判斷系統現在是否在offline凍結狀態
        internal static IWebDBConnectionInfo DBConnectInfo;
        internal static string DBConnectString;

        #region Constructor
        public SystemVariable()
        {

        }
        #endregion

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
            string FullFilePath = string.Format("{0}{1}", path, SystemVariable.ConfigFileName);
            XDocument XD2 = XDocument.Load(FullFilePath);
            return XD2;
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

                ////////////////////////////////////////////////////////////////////////////////////////////////
                if (MapWindowVariable.DoffTypeGridSetup == null)
                    MapWindowVariable.DoffTypeGridSetup = new List<DoffGridColumns>();
                else
                    MapWindowVariable.DoffTypeGridSetup.Clear();
                IEnumerable<XElement> XDoffTypeGrid = XSysConf.Element("SystemConfig").Element("DoffTypeGrid").Elements("Column"); //自動儲存右上方GridView的排序和欄位Size
                foreach (XElement el in XDoffTypeGrid)
                {
                    DoffGridColumns d = new DoffGridColumns(int.Parse(el.Element("Index").Value), el.Attribute("Name").Value, int.Parse(el.Element("Size").Value));
                    MapWindowVariable.DoffTypeGridSetup.Add(d);
                }

                ////////////////////////////////////////////////////////////////////////////////////////////////
                XElement XLimit = XSysConf.Element("SystemConfig").Element("Limit");
                PxPVariable.PieceLimit = int.TryParse(XLimit.Value, out PxPVariable.PieceLimit) ? PxPVariable.PieceLimit : 20;
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
            XElement MapVariable = XConf.Element("Config").Element("MapVariable");
            try
            {
                PxPVariable.ImgRowsSet = int.Parse(MapVariable.Element("ImgRowsSet").Value);
                PxPVariable.ImgColsSet = int.Parse(MapVariable.Element("ImgColsSet").Value);
                MapWindowVariable.MapProportion = int.Parse(MapVariable.Element("MapProportion").Value);
                MapWindowVariable.ShowGridSet = (int.Parse(MapVariable.Element("ShowGridSet").Value) == 1) ? true : false;
                MapWindowVariable.MapGridSet = int.Parse(MapVariable.Element("MapGridSet").Value);
                MapWindowVariable.MapMDSet = double.Parse(MapVariable.Element("MapMDSet").Value);
                MapWindowVariable.MapCDSet = double.Parse(MapVariable.Element("MapCDSet").Value);
                MapWindowVariable.SeriesSet = int.Parse(MapVariable.Element("SeriesSet").Value);
                MapWindowVariable.BottomAxe = int.Parse(MapVariable.Element("BottomAxe").Value);
                MapWindowVariable.MDInver = int.Parse(MapVariable.Element("MDInver").Value);
                MapWindowVariable.CDInver = int.Parse(MapVariable.Element("CDInver").Value);
                MapWindowVariable.ShowFlag = int.Parse(MapVariable.Element("ShowFlag").Value);
                MapWindowVariable.LastMapCDConvertion = double.Parse(MapVariable.Element("LastMapCDConvertion").Value);
                MapWindowVariable.LastMapMDConvertion = double.Parse(MapVariable.Element("LastMapMDConvertion").Value);
               
                PxPVariable.PageSize = PxPVariable.ImgRowsSet * PxPVariable.ImgColsSet;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Initialize Load Config Fail : \n" + ex.Message);
                PxPVariable.ImgRowsSet = 2;
                PxPVariable.ImgColsSet = 2;
                MapWindowVariable.MapProportion = 0;
                MapWindowVariable.ShowGridSet = true;
                MapWindowVariable.MapGridSet = 1;
                MapWindowVariable.MapMDSet = 3;
                MapWindowVariable.MapCDSet = 3;
                MapWindowVariable.SeriesSet = 0;
                MapWindowVariable.BottomAxe = 0;
                MapWindowVariable.MDInver = 0;
                MapWindowVariable.CDInver = 0;
                MapWindowVariable.LastMapCDConvertion = 1.00;
                MapWindowVariable.LastMapMDConvertion = 1.00;
            }

            ///////////////////////////////////////////////////////////////////////////////////////////
            IEnumerable<XElement> xMapFlawTypeName = XConf.Element("Config").Element("MapVariable").Elements("FlawTypeName");
            try
            {
                PxPVariable.FlawTypeName.Clear();
                foreach (var el in xMapFlawTypeName)
                {
                    FlawTypeNameExtend tmp = new FlawTypeNameExtend();
                    tmp.Display = (int.Parse(el.Element("Display").Value) == 1) ? true : false;
                    tmp.Color = el.Element("Color").Value.ToString();
                    tmp.Name = el.Element("Name").Value.ToString();
                    tmp.Shape = ((Shape)Enum.Parse(typeof(Shape),el.Element("Shape").Value.ToString(),false)).ToGraphic();
                    
                    tmp.FlawType = int.Parse(el.Element("FlawType").Value.ToString());
                    PxPVariable.FlawTypeName.Add(tmp);
                }
                
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Initialize Load Config Fail :  MapDoffTypeGrid \n" + ex.Message);
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
        public string Area { set; get; }
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
        public double RCD { get; set; }
        //Keep origin value
        public string OArea { set; get; }
        public double OCD { set; get; }
        public double OMD { set; get; }
        public double OLength { set; get; }
        public double OWidth { set; get; }
        public double ORMD { set; get; }
        public double ORCD { set; get; }
        
        public FlawInfoAddPriority()
        { 
            
        }
        ////////////////////////////////////////////////////////////////////////////////
        

    }

    public class FlawTypeNameExtend
    {
        public string Name { set; get; }
        public int FlawType { set; get; }
        //Add Other Properties
        public bool Display { set; get; }
        public int Count { set; get; }
        //public string Letter { set; get; }
        public string Color { set; get; }
        public string Shape { set; get; }
        public int JobNum { set; get; }
        public int DoffNum { set; get; }
        public int OfflineJobNum { set; get; }
        public int OfflineDoffNum { set; get; }
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
    
    public class ConfFile { public string Name { get; set; } }

    // 三角形, 倒三角形, 正方形, 圓形, 十字, 叉叉, 星號
    //public enum Shape { Triangle, Ellipse, Square, Cone, Cross, LineDiagonalCross, Star };
    public enum Shape {
        [DescriptionAttribute("▲"), EnumDescription("▲")]
        Triangle,
        [DescriptionAttribute("▼"), EnumDescription("▼")]
        Ellipse,
        [DescriptionAttribute("■"), EnumDescription("■")]
        Square,
        [DescriptionAttribute("●"), EnumDescription("●")]
        Cone,
        [DescriptionAttribute("+"), EnumDescription("+")]
        Cross,
        [DescriptionAttribute("╳"), EnumDescription("╳")]
        LineDiagonalCross,
        [DescriptionAttribute("★"), EnumDescription("★")]
        Star 
    };
    public class ImageInfo :IImageInfo
    {

        #region IImageInfo 成員

        public System.Drawing.Bitmap Image { set; get; }

        public int Station { set; get; }
        public ImageInfo(System.Drawing.Bitmap image, int station)
        {
            this.Image = image;
            this.Station = station;
        }
        #endregion
    }
    #endregion 


    #region 增加列舉型別ENUM其他功能
    static class ExtensionMethods
    {
        public static string ToGraphic(this Enum en) //ext method
        {

            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(
                                              typeof(DescriptionAttribute), 
                                              false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return en.ToString();
        }
    }
    /// <summary>
    /// Provides a description for an enumerated type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field,
     AllowMultiple = false)]
    public sealed class EnumDescriptionAttribute : Attribute
    {
        private string description;

        /// <summary>
        /// Gets the description stored in this attribute.
        /// </summary>
        /// <value>The description stored in the attribute.</value>
        public string Description
        {
            get
            {
                return this.description;
            }
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="EnumDescriptionAttribute"/> class.
        /// </summary>
        /// <param name="description">The description to store in this attribute.
        /// </param>
        public EnumDescriptionAttribute(string description)
            : base()
        {
            this.description = description;
        }
    }
    /// <summary>
    /// Provides a static utility object of methods and properties to interact
    /// with enumerated types.
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Gets the <see cref="DescriptionAttribute" /> of an <see cref="Enum" />
        /// type value.
        /// </summary>
        /// <param name="value">The <see cref="Enum" /> type value.</param>
        /// <returns>A string containing the text of the
        /// <see cref="DescriptionAttribute"/>.</returns>
        public static string GetDescription(Enum value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            string description = value.ToString();
            FieldInfo fieldInfo = value.GetType().GetField(description);
            EnumDescriptionAttribute[] attributes =
               (EnumDescriptionAttribute[])
             fieldInfo.GetCustomAttributes(typeof(EnumDescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
            {
                description = attributes[0].Description;
            }
            return description;
        }

        /// <summary>
        /// Converts the <see cref="Enum" /> type to an <see cref="IList" /> 
        /// compatible object.
        /// </summary>
        /// <param name="type">The <see cref="Enum"/> type.</param>
        /// <returns>An <see cref="IList"/> containing the enumerated
        /// type value and description.</returns>
        public static IList ToList(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            ArrayList list = new ArrayList();
            Array enumValues = Enum.GetValues(type);

            foreach (Enum value in enumValues)
            {
                list.Add(new KeyValuePair<Enum, string>(value, GetDescription(value)));
            }

            return list;
        }
        public static string GetItemString(string Description)
        {
            string result = "";
            switch (Description)
            {
                case "▲":
                    result = "Triangle";
                    break;
                case "▼":
                    result = "Ellipse";
                    break;
                case "■":
                    result = "Square";
                    break;
                case "●":
                    result = "Cone";
                    break;
                case "+":
                    result = "Cross";
                    break;
                case "╳":
                    result = "LineDiagonalCross";
                    break;
                case "★":
                    result = "Star";
                    break;
                default:
                    break;
            }
            return result;
        }
    }
    #endregion

    



}
