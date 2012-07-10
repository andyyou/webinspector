using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PxP
{
    public class MapWindowThreadStatus
    {
        public static bool IsRunSetup = false;
        public static bool StopMapThreading = false;
        public static bool IsTableLayoutRefresh = false;
        public static bool IsPageRefresh = false;
        public static bool UpdateChange = false;
        public static bool IsChangePiece = false;
    }
}