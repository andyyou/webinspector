using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PxP
{
    public class DebugPut
    {
        const string path = @"c:\Project\PxPLifeCycle.txt";
        public StreamWriter sw = new StreamWriter(path);
        public static int i = 1;
        public DebugPut()
        {
            
        }
        ~DebugPut()
        {
            sw.Close();
        }
        public void WriteLog(string FileName, string Msg, bool? BoolMsg)
        {
            lock (sw)
            {
                sw.WriteLine(i.ToString() + ". " + FileName + " | " + Msg);
                sw.Flush();
                i++;
            }

        }
        public static void DebugMethod()
        {

        }

    }
}
