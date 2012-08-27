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
        //public static StreamWriter sw = new StreamWriter(path);
        static object lockMe = new object();
        public static int i = 1;
        public DebugTool()
        {

        }
        public static void WriteLog(int OrderMode, int MethodMode, string FileName, string Method, string Msg )
        {
            //OrderMode : 0 => 成功執行。 1 => 錯誤。 2 => Sub function & Thread & Action Event 。 3 => Inherit interface。 
            lock (lockMe)
            {
                using (StreamWriter sw = new StreamWriter(path, true))
                {
                    sw.WriteLine("<tr>");
                    switch(OrderMode)
                    {
                        case 0 :
                            sw.WriteLine("  <td><span class='badge badge-success'>{0}</span></td>", i.ToString());
                            break;
                        case 1:
                            sw.WriteLine("  <td><span class='badge badge-important'>{0}</span></td>", i.ToString());
                            break;
                        case 2:
                            sw.WriteLine("  <td><span class='badge badge-info'>{0}</span></td>", i.ToString());
                            break;
                        case 3:
                            sw.WriteLine("  <td><span class='badge badge-inverse'>{0}</span></td>", i.ToString());
                            break;
                    }
                   
                    sw.WriteLine("  <td>{0}</td>", FileName);
                    switch(MethodMode)
                    {
                        case 0 :
                            sw.WriteLine("  <td><span class='label'>Start</span> {0}</td>", Method);
                            break;
                        case 1:
                            sw.WriteLine("  <td><span class='label label-warning'>End</span> {0}</td>", Method);
                            break;
                        default:
                            sw.WriteLine("  <td><span class='label label-info'>Process</span> {0}</td>", Method);
                            break;
                    }
                    sw.WriteLine("  <td>{0}</td>", Msg);
                    sw.WriteLine("</tr>");
                    //sw.WriteLine(i.ToString() + ". " + FileName.PadRight(15) + " | " + Msg.PadRight(50));
                    sw.Close(); 
                } 
                i++;
            }
        }
    }
}
