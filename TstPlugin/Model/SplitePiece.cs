using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PxP
{
    public class SplitePieces
    {
        internal int Id { set; get; }
        internal List<SplitePiece> Pieces { set; get; }
    }

    public class SplitePiece
    {
        internal string Name{set; get;}
        internal string GradeLevel{set; get;}
        internal double Socre{set; get;}
        internal string Note { set; get; }
        public SplitePiece()
        {
            this.Socre = 0;
        }

    }
}
