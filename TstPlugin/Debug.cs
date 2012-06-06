using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PxP
{
    public class DebugTool
    {
        const string path = @"c:\temp\PxPLifeCycle.txt";
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
        static void Some()
        { }


    }
}
