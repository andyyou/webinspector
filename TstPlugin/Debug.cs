using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PxP
{
    public  class DebugTool
    {
        const string path = @"c:\Project\PxPLifeCycle.txt";
        public static StreamWriter sw = new StreamWriter(path);
        public static int i = 1;
        public DebugTool()
        {

        }
        ~DebugTool()
        {
            sw.Close();
        }
        public static void WriteLog(string FileName, string Msg)
        {
            lock (sw)
            {
                sw.WriteLine(i.ToString() + ". " + FileName.PadRight(15) + " | " + Msg.PadRight(50));
                sw.Flush();
                i++;
            }

        }
        public static void WriteLog(string FileName, string Msg, string OtherMsg)
        {
            lock (sw)
            {
                sw.WriteLine(i.ToString() + ". " + FileName.PadRight(15) + " | " + Msg.PadRight(50) + " | " + OtherMsg.PadRight(50));
                sw.Flush();
                i++;
            }

        }
        public static void WriteLog(string FileName, string Msg, string PiecesCount, string CurrentPiece)
        {
            lock (sw)
            {
                sw.WriteLine("{0} | {1} | PiecesCount:{2} | CurrentPiece:{3} ",FileName.PadRight(15), Msg.PadRight(50), PiecesCount.PadRight(10), CurrentPiece.PadRight(10) );
                sw.Flush();
                i++;
            }

        }
        static void Some()
        { }


    }
}
