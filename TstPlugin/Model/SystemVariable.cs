using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using WRPlugIn;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace PxP
{
    public class SystemVariable
    {
        internal static string ConfigFileName;                     // 儲存 XML 路徑可自訂義(預設\CPxP\conf\setup.xml)
        internal static string GradeConfigFileName;                // 儲存等級評分定義 XML 路徑 (預設\CPxP\grade\default.xml)
        internal static e_Language Language = e_Language.English;  // 預設為英語
        internal static string FlawLock = "FlawLock";              // OnFlaws & OnCut 鎖定
        internal static bool IsReadHistory = false;                // 判斷是否讀取歷史紀錄
        internal static bool IsSystemFreez = false;                // 判斷系統現在是否在 Offline 凍結狀態
        internal static IWebDBConnectionInfo DBConnectInfo;
        internal static string DBConnectString;

        #region Constructor

        public SystemVariable() { }

        #endregion

        #region Refactoring

        // ASC number to char
        public static char Chr(int Num)
        {
            char C = Convert.ToChar(Num);
            return C;
        }

        #endregion

        #region 取得設定檔參數Method

        // 取得語系檔
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

        // 取得 sys_conf/sys.xml 用來設定自訂參數檔及 Grid 排序位置
        internal static XDocument GetSysConfXDoc()
        {
            string path = Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\..\\Parameter Files\\CPxP\\sys_conf\\";
            string FullFilePath = path + "sys.xml";
            XDocument XD = XDocument.Load(FullFilePath);

            return XD;
        }

        // 取得 conf 底下參數檔可自訂
        internal static XDocument GetConfig()
        {
            if (String.IsNullOrEmpty(SystemVariable.ConfigFileName))
            {
                LoadSystemConfig();
            }
            string path = Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\..\\Parameter Files\\CPxP\\conf\\";
            string FullFilePath = string.Format("{0}{1}", path, SystemVariable.ConfigFileName);
            XDocument XD2 = XDocument.Load(FullFilePath);
            return XD2;
        }

        // 取得 grade conf 參數檔
        internal static XDocument GetGradeConfXDoc()
        {
            if (String.IsNullOrEmpty(SystemVariable.ConfigFileName))
            {
                LoadSystemConfig();
            }
            string path = Path.GetDirectoryName(
             Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\..\\Parameter Files\\CPxP\\grade\\";
            string FullFilePath = string.Format("{0}{1}", path, SystemVariable.GradeConfigFileName);
            XDocument XD2 = XDocument.Load(FullFilePath);
            return XD2;
        }
       
        // 載入/sys_conf/sys.xml  ==> 根據有定義語系資料再變更一次參數
        internal static void LoadSystemConfig()
        {
            //db
            DebugTool.WriteLog(2, 0, "SystemVariable.cs", "LoadSystemConfig()","");
            #region 註解

            /*
             * 開啟程式啟動完執行緒之後, 立刻載入相關設定檔, 將設定檔的值帶入 Model 全域變數中
             * 系統變數包含 : 
             *   1. 載入其他 Conf 檔的檔名 (因為 User 可自訂 Conf 檔)
             *   2. 右上角缺陷 DataGridView 的欄位左右排列順序
             */

            #endregion
            try
            {
                XDocument XSysConf = GetSysConfXDoc();
                XElement XConfFile = XSysConf.Element("SystemConfig").Element("ConfFile");  // 儲存 Conf 檔名到 SystemVariable 變數
                IEnumerable<XElement> XDoffGrid = XSysConf.Element("SystemConfig").Element("DoffGrid").Elements("Column");  // 自動儲存右上方 GridView 的排序和欄位 Size
                SystemVariable.ConfigFileName = XConfFile.Value + ".xml";

                // 儲存GradeConfig到全域變數
                XElement XGradeConfFile = XSysConf.Element("SystemConfig").Element("GradeConfigFile");  // 儲存 GradeConf 檔名到 SystemVariable 變數
                SystemVariable.GradeConfigFileName = XGradeConfFile.Value + ".xml"; ;
                PxPVariable.DoffGridSetup.Clear();
                foreach (XElement el in XDoffGrid)
                {
                    DoffGridColumns d = new DoffGridColumns(int.Parse(el.Element("Index").Value), el.Attribute("Name").Value, int.Parse(el.Element("Size").Value));
                    PxPVariable.DoffGridSetup.Add(d);
                }
                XElement OrderByColumn = XSysConf.Element("SystemConfig").Element("DoffGrid").Element("OrderBy");
                PxPVariable.FlawGridViewOrderColumn = OrderByColumn.Value;

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

                XElement XLimit = XSysConf.Element("SystemConfig").Element("Limit");
                PxPVariable.PieceLimit = int.TryParse(XLimit.Value, out PxPVariable.PieceLimit) ? PxPVariable.PieceLimit : 20;
            }
            catch (Exception ex)
            {
                DebugTool.WriteLog(1, 2, "SystemVariable.cs", "LoadSystemConfig()", ex.Message);
            }
            //db
            DebugTool.WriteLog(2, 1, "SystemVariable.cs", "LoadSystemConfig()", "");
        }

        // 載入 /CPxP/conf/setup.xml 或自訂的設定檔
        // 如果該變數會受 Conf 檔影響, 請記得在此補上
        internal static void LoadConfig()
        {
            //db
            DebugTool.WriteLog(2, 0, "SystemVariable.cs", "LoadConfig()", "");

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
                DebugTool.WriteLog(1, 2, "SystemVariable.cs", "LoadConfig() > GetConfig()", ex.Message);
            }

            IEnumerable<XElement> xMapFlawTypeName = XConf.Element("Config").Element("MapVariable").Elements("FlawTypeName");
            try
            {
                PxPVariable.FlawTypeName.Clear();
                foreach (XElement el in xMapFlawTypeName)
                {
                    FlawTypeNameExtend tmp = new FlawTypeNameExtend();
                    tmp.Display = (int.Parse(el.Element("Display").Value) == 1) ? true : false;
                    tmp.Color = el.Element("Color").Value.ToString();
                    tmp.Name = el.Element("Name").Value.ToString();
                    tmp.Shape = ((Shape)Enum.Parse(typeof(Shape), el.Element("Shape").Value.ToString(), false)).ToGraphic();

                    tmp.FlawType = int.Parse(el.Element("FlawType").Value.ToString());
                    PxPVariable.FlawTypeName.Add(tmp);
                }
            }
            catch (Exception ex)
            {
                DebugTool.WriteLog(1, 2, "SystemVariable.cs", "LoadConfig() > Set FlawTypeName", ex.Message);
                System.Windows.Forms.MessageBox.Show("Initialize Load Config Fail :  MapDoffTypeGrid \n" + ex.Message);
            }
            //db
            DebugTool.WriteLog(2, 1, "SystemVariable.cs", "LoadConfig()", "");
        }

        // 載入 Grade 的 xml 內容到全域變數
        internal static void LoadGradeConfig()
        {
            //db
            DebugTool.WriteLog(2, 0, "SystemVariable.cs", "LoadGradeConfig()", "");

            XDocument XGrade = GetGradeConfXDoc();
            // 載入 Roi 模式
            XElement XRoiMode = XGrade.Element("GradeConfig").Element("Roi").Element("RoiMode");
            XElement XRoiColumns = XGrade.Element("GradeConfig").Element("Roi").Element("RoiColumns");
            XElement XRoiRows = XGrade.Element("GradeConfig").Element("Roi").Element("RoiRows");

            try
            {
                GradeVariable.RoiMode = int.TryParse(XRoiMode.Value, out GradeVariable.RoiMode) ? GradeVariable.RoiMode : 0;
                GradeVariable.RoiGradeColumns = int.TryParse(XRoiColumns.Value, out GradeVariable.RoiGradeColumns) ? GradeVariable.RoiGradeColumns : 0;
                GradeVariable.RoiGradeRows = int.TryParse(XRoiRows.Value, out GradeVariable.RoiGradeRows) ? GradeVariable.RoiGradeRows : 0;
            }
            catch (Exception ex)
            {
                DebugTool.WriteLog(1, 2, "SystemVariable.cs", "LoadGradeConfig() > Set Roi mode", ex.Message);
            }

            // 載入 Roi 欄位 start, end 座標資料
            try
            {
                IEnumerable<XElement> XColumns = XGrade.Element("GradeConfig").Element("Roi").Elements("Column");
                GradeVariable.RoiColumnsGrid.Clear();
                foreach (XElement c in XColumns)
                {
                    RoiGrade tmpRoiColumn = new RoiGrade();
                    tmpRoiColumn.Name = c.Attribute("Name").Value;
                    tmpRoiColumn.Start = double.Parse(c.Element("Start").Value);
                    tmpRoiColumn.End = double.Parse(c.Element("End").Value);
                    GradeVariable.RoiColumnsGrid.Add(tmpRoiColumn);
                }

                IEnumerable<XElement> XRows = XGrade.Element("GradeConfig").Element("Roi").Elements("Row");
                GradeVariable.RoiRowsGrid.Clear();
                foreach (XElement r in XRows)
                {
                    RoiGrade tmpRoiRow = new RoiGrade();
                    tmpRoiRow.Name = r.Attribute("Name").Value;
                    tmpRoiRow.Start = double.Parse(r.Element("Start").Value);
                    tmpRoiRow.End = double.Parse(r.Element("End").Value);
                    GradeVariable.RoiRowsGrid.Add(tmpRoiRow);
                }
            }
            catch (Exception ex)
            {
                DebugTool.WriteLog(1, 2, "SystemVariable.cs", "LoadGradeConfig() > 載入 Roi 欄位 start, end 座標資料", ex.Message);
            }

            // 載入 Grade setting > Point xml default value
            try
            {
                XElement XPointEnable = XGrade.Element("GradeConfig").Element("Grade").Element("PointGrade").Element("Enable");
                GradeVariable.IsPointEnable = (int.Parse(XPointEnable.Value) == 1) ? true : false;

                IEnumerable<XElement> XPointSubPieces = XGrade.Element("GradeConfig").Element("Grade").Element("PointGrade").Elements("SubPiece");
                GradeVariable.PointSubPieces.Clear();
                
                // 1. 先加入一筆 ALL 的 SubPiece 指全部使用相同設定
                PointSubPiece allsame = new PointSubPiece();
                allsame.Name = "ALL";
                allsame.Grades = new List<PointGrade>();
                foreach (FlawTypeNameExtend i in PxPVariable.FlawTypeName)
                {
                    PointGrade tmpPG = new PointGrade();
                    tmpPG.ClassId = i.FlawType;
                    tmpPG.ClassName = i.Name;
                    tmpPG.Score = 0;

                    allsame.Grades.Add(tmpPG);
                }
                GradeVariable.PointSubPieces.Add(allsame);

                // 2. 用 Rows 和 Columns 造出所有欄位再去 xml 補資料
                foreach (RoiGrade r in GradeVariable.RoiRowsGrid)
                {
                    foreach (RoiGrade c in GradeVariable.RoiColumnsGrid)
                    {
                        PointSubPiece tmpPointSubPiece = new PointSubPiece();
                        tmpPointSubPiece.Name = "ROI-" + r.Name + c.Name;
                        tmpPointSubPiece.Grades = new List<PointGrade>();
                        foreach (FlawTypeNameExtend i in PxPVariable.FlawTypeName)
                        {
                            PointGrade tmpPG = new PointGrade();
                            tmpPG.ClassId = i.FlawType;
                            tmpPG.ClassName = i.Name;
                            tmpPG.Score = 0;
                            tmpPointSubPiece.Grades.Add(tmpPG);
                        }
                        GradeVariable.PointSubPieces.Add(tmpPointSubPiece);
                    }
                }
                // 3. 補上 XML 有記錄的資料到 GradeVariable.PointSubPieses
                foreach (XElement xsp in XPointSubPieces)
                {
                    foreach (PointSubPiece gsp in GradeVariable.PointSubPieces)
                    {
                        if (gsp.Name == xsp.Attribute("Name").Value)
                        {
                            foreach (PointGrade g in gsp.Grades)
                            {
                                foreach (XElement ft in xsp.Elements("FlawType"))
                                { 
                                    if(g.ClassId == int.Parse(ft.Attribute("Id").Value))
                                        g.Score = int.Parse(ft.Value);
                                }
                            }
                        }
                    }

                }                
            }
            catch (Exception ex)
            {
                DebugTool.WriteLog(1, 2, "SystemVariable.cs", "LoadGradeConfig() > 載入 Grade setting > Point xml default value", ex.Message);
                System.Windows.Forms.MessageBox.Show("Setting global point variable errors : " + ex.Message);
            }

            // 載入 Grade setting > Mark grade xml default
            try
            {
                XElement XMarkGradetEnable = XGrade.Element("GradeConfig").Element("Grade").Element("MarkGrade").Element("Enable");
                GradeVariable.IsMarkGradeEnable = (int.Parse(XMarkGradetEnable.Value) == 1) ? true : false;

                // Set gridview of markgrade value
                IEnumerable<XElement> XMarkGradeSubPieces = XGrade.Element("GradeConfig").Element("Grade").Element("MarkGrade").Elements("SubPiece");
                GradeVariable.MarkGradeSubPieces.Clear();

                // 1. 先加入一筆 ALL 的 SubPiece 指全部使用相同設定
                MarkSubPiece allsame = new MarkSubPiece();
                allsame.Name = "ALL";
                allsame.Grades = new List<MarkGrade>();
                foreach (XElement i in XMarkGradeSubPieces)
                {
                    int asc = 65;
                    Regex rgx = new Regex("^[A-Z]*$");
                    if (i.Attribute("Name").Value == allsame.Name)
                    {
                        foreach (XElement j in i.Elements("GradeRow"))
                        {
                            MarkGrade tmp = new MarkGrade();
                            tmp.GradeName = rgx.IsMatch(j.Attribute("Id").Value.ToString()) ? j.Attribute("Id").Value : Chr(asc).ToString();
                            asc++;
                            tmp.Score = int.Parse(j.Value);
                            allsame.Grades.Add(tmp);
                        }
                        
                    }
                }
                if (allsame.Grades.Count <= 0)
                {
                    MarkGrade tmpMG = new MarkGrade();
                    tmpMG.GradeName = Chr(65).ToString();
                    allsame.Grades.Add(tmpMG);
                }
                GradeVariable.MarkGradeSubPieces.Add(allsame);

                // 2. 用 Rows 和 Columns 造出所有欄位再去 xml 補資料
                foreach (RoiGrade r in GradeVariable.RoiRowsGrid)
                {
                    foreach (RoiGrade c in GradeVariable.RoiColumnsGrid)
                    {
                        MarkSubPiece tmpMarkSubPiece = new MarkSubPiece();
                        tmpMarkSubPiece.Name = "ROI-" + r.Name + c.Name;
                        tmpMarkSubPiece.Grades = new List<MarkGrade>();
                        foreach (XElement i in XMarkGradeSubPieces)
                        {
                            if (i.Attribute("Name").Value == tmpMarkSubPiece.Name)
                            {
                                // 3. 補上 XML 有記錄的資料
                                int asc = 65;
                                Regex rgx = new Regex("^[A-Z]*$");
                                foreach (XElement j in i.Elements("GradeRow"))
                                {
                                    MarkGrade tmp = new MarkGrade();

                                    tmp.GradeName = rgx.IsMatch(j.Attribute("Id").Value.ToString()) ? j.Attribute("Id").Value : Chr(asc).ToString();
                                    asc++;
                                    tmp.Score = int.Parse(j.Value);
                                    tmpMarkSubPiece.Grades.Add(tmp);
                                }
                                // 4. 如果沒有新增一筆 A
                                if (tmpMarkSubPiece.Grades.Count <= 0)
                                {
                                    MarkGrade tmg = new MarkGrade();
                                    tmg.GradeName = Chr(65).ToString();
                                    tmpMarkSubPiece.Grades.Add(tmg);
                                }
                            }
                        }
                        GradeVariable.MarkGradeSubPieces.Add(tmpMarkSubPiece);
                    }
                }
            }
            catch (Exception ex)
            {
                DebugTool.WriteLog(1, 2, "SystemVariable.cs", "LoadGradeConfig() > 載入 Grade setting > Mark grade xml default", ex.Message);
            }

            // 載入 Grade setting > Pass or fail filter socre xml default
            try
            {
                XElement XPFEnable = XGrade.Element("GradeConfig").Element("Grade").Element("PassFail").Element("Enable");
                GradeVariable.IsPassOrFailScoreEnable = (int.Parse(XPFEnable.Value) == 1) ? true : false;
                XElement XScore = XGrade.Element("GradeConfig").Element("Grade").Element("PassFail").Element("Score");
                GradeVariable.PassOrFileScore = int.Parse(XScore.Value);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Setting global pass or fail variable errors : " + ex.Message);
                DebugTool.WriteLog(1, 2, "SystemVariable.cs", "LoadGradeConfig() > 載入 Grade setting > Pass or fail filter socre xml default", ex.Message);
            }

            //db
            DebugTool.WriteLog(2, 1, "SystemVariable.cs", "LoadGradeConfig()", "");
        }

        #endregion
    }
}
