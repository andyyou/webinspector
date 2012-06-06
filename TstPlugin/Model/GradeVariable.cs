using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PxP
{
    public class GradeVariable
    {
        /// <summary>
        /// Grade > Roi setting
        /// </summary>
        internal static int RoiMode;  //0 => No ROI, 1 => Symmetrical 
        internal static int RoiGradeColumns;  //Page of GradeSetup.cs 設定欄位數
        internal static int RoiGradeRows;    //Page of GradeSetup.cs 設定列數
        internal static List<RoiGrade> RoiColumnsGrid = new List<RoiGrade>();
        internal static List<RoiGrade> RoiRowsGrid = new List<RoiGrade>();

        ///////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Grade > Grade setting > Point
        /// </summary>
        internal static bool IsPointEnable;
        internal static List<PointSubPiece> PointSubPieces = new List<PointSubPiece>();

        ///////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Grade > Grade setting > Grade
        /// </summary>
        internal static bool IsMarkGradeEnable;
        internal static List<MarkSubPiece> MarkGradeSubPieces = new List<MarkSubPiece>();


        ///////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Grade > Grade setting > Pass/Fail
        /// </summary>
        internal static bool IsPassOrFailScoreEnable;
        internal static int PassOrFileScore;

        ///////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
