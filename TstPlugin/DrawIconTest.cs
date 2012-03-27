using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PxP
{
    public partial class DrawIconTest : UserControl
    {
        public DrawIconTest()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawString("Hello", new Font("Verdana", 18), new SolidBrush(Color.Red), 45, 40);
        }
    }
}
