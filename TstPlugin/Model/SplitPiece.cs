using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PxP
{
    public class SplitPieces
    {
        internal int Id { set; get; }
        internal List<SplitPiece> Pieces { set; get; }
    }

    public class SplitPiece
    {
        internal string Name{set; get;}
        internal string GradeLevel{set; get;}
        internal double Score{set; get;}
        internal string Note { set; get; }
        public SplitPiece()
        {
            this.Score = 0;
        }

    }
}
