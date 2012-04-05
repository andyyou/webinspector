using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PxP
{
    public class TransparentTrackBar : TrackBar
    {
        protected override void OnCreateControl()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            if (Parent != null)
                BackColor = Parent.BackColor;

            base.OnCreateControl();
        }
    }
}
