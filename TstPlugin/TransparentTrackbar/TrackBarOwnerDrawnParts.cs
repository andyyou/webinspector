using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PxP.Toolkit
{
    [Flags]
    public enum TrackBarOwnerDrawParts
    {
        Channel = 4,
        None = 0,
        Thumb = 2,
        Ticks = 1
    }
}
