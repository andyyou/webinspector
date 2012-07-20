using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WRPlugIn;

namespace PxP
{
    public class MapWindowVariable
    {
        internal static MapWindow MapWindowController = new MapWindow();
        internal static List<FlawInfoAddPriority> Flaws = new List<FlawInfoAddPriority>();                   // 記錄 OnFlaws 整條資訊
        internal static List<FlawInfoAddPriority> FlawPiece = new List<FlawInfoAddPriority>();               // 暫存單片 Piece
        internal static int InitMapWidth;                                                                    // 記錄初始 Map 寬
        internal static int InitMapHeight;                                                                   // 記錄初始 Map 高
        internal static bool IsMapInit;                                                                      // 記錄 Map 是否紀錄初始狀態
        internal static List<IList<IFlawInfo>> OriginFlawPieces = new List<IList<IFlawInfo>>();              // 儲存原始資料已用來變更單位計算
        internal static List<List<FlawInfoAddPriority>> FlawPieces = new List<List<FlawInfoAddPriority>>();  // 儲存 Piece 切割後的所有檔案
        internal static int CurrentPiece = 0;     // 儲存左邊目前看到哪片玻璃
        internal static int MapProportion = 0;    // 記錄 Map 比例 0->1:1, 2->1:1, 2->2:1, 3->4:3, 4->3:4,  5->16:9
        internal static bool ShowGridSet = true;  // 是否顯示格線
        internal static int MapGridSet = 1;       // 選擇使用的Grid間隔依據 0: EachCellSize , 1: EachCellCount
        internal static double MapMDSet = 3;      // Map Size 的間隔大小
        internal static double MapCDSet = 3;
        internal static int SeriesSet = 0;        // 記錄選用的紀錄方式 0->Sharp , 1->Letter
        internal static int BottomAxe = 0;        // 記錄 MD 或 CD 為 Bottom Axes 0:CD, 1:MD
        internal static int MDInver = 0;          // 記錄是否反轉座標軸
        internal static int CDInver = 0;
        internal static int ShowFlag = 0;         // 記錄顯示項目 0:All, 1:Pass, 2:Fail
        internal static List<DoffGridColumns> DoffTypeGridSetup;    // 記錄左下角 DataGridView 欄位左右排序
        internal static Dictionary<int, bool> PieceResult = new Dictionary<int, bool>();  // 記錄每片玻璃的檢測結果
        internal static int tmpPieceKey = -1;        // 暫存 PieceResult 數量, 供判斷是否已達最末片
        internal static double LastMapMDConvertion;  // 記錄上一次 MD 單位變更的比例
        internal static double LastMapCDConvertion;  // 記錄上一次 CD 單位變更的比例
        public MapWindowVariable() { }
    }
}
