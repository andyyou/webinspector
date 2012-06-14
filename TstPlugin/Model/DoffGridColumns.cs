using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PxP
{
    public class DoffGridColumns
    {
        public int Index { set; get; }
        public string ColumnName { set; get; }
        public int Width { set; get; }
        public string HeaderText { set; get; }
        public DoffGridColumns()
        { }
        public DoffGridColumns(int index, string column, int width)
        {
            this.Index = index;
            this.ColumnName = column;
            this.Width = width;
        }
    }
}

