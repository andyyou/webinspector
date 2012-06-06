using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WRPlugIn;
using System.Data;

namespace PxP
{
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

        public static Dictionary<string, int> UnitsKeys = new Dictionary<string, int>();
        public static DataSet UnitsData;

    }
}
