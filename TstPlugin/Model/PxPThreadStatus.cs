using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PxP
{
    public class PxPThreadStatus
    {
        public static bool IsOnShutdown;   // 關閉
        public static bool IsOnCut;        // 裁切
        public static bool IsOnJobStarted; // 工單開始執行
        public static bool IsOnJobLoaded;  // 工單載入完成
        public static bool IsOnJobStopped; // 工單停止
        public static bool IsOnOnline;     // 上載工單完成之後進入可啟動狀態
        public static bool IsOnFlaws;      // 開始跑 Flaws
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

        // 初始化全部狀態都關閉
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
}
