using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PxP
{
    // 單筆資料
    public class MarkGrade
    {
        public string GradeName { set; get; }
        public int Score { set; get; }
    }
    
    // SubPiece 切出來的區塊(一格) ex: ROI-11, ROI-12 etc...
    public class MarkSubPiece
    {
        public string Name;
        public List<MarkGrade> Grades = new List<MarkGrade>();
    }
}
