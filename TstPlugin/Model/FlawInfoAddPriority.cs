using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WRPlugIn;

namespace PxP
{
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
        public double PointScore { get; set; }
        public string SubPieceName { get; set; }
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
}
