using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PxP
{
    public class Pair
    {
        public string Key { set; get; }
        public int Value { set; get; }
        public Pair() { }
        public Pair(string k, int v)
        {
            Key = k;
            Value = v;
        }

    }
}

