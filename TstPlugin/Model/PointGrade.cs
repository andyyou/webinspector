using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PxP
{
    //單筆資料
    public class PointGrade
    {
        public int ClassId {set;get;}
        public string ClassName { set; get; }
        public int Score { set; get; }
    }
    //2012/06/04
    //SubPiece 切出來的棺材 一格  ROI-11 ROI-12
    public class PointSubPiece
    {
        public string Name;
        public List<PointGrade> Grades;
    }
}
