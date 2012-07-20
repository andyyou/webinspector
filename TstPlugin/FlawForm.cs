using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WRPlugIn;

namespace PxP
{
    public partial class FlawForm : Form
    {
        #region Local Variables

        public PictureBox[] pb;
        public double[] pb_ratio;
        private Image[] SrcImg;
        private FlawInfoAddPriority flaws;
        private Point MousePt = new Point();
        private bool FormDragging = false;

        #endregion

        #region Constructor

        public FlawForm(int PointIndex)
        {
            InitializeComponent();
            flaws = MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1][PointIndex];
            pb = new PictureBox[PxPVariable.JobInfo.NumberOfStations];

            SrcImg = new Image[PxPVariable.JobInfo.NumberOfStations];
            pb_ratio = new double[PxPVariable.JobInfo.NumberOfStations];
        }

        ~FlawForm()
        {

        }

        #endregion

        #region Action Events

        private void FlawForm_Load(object sender, EventArgs e)
        {
            lbFlawIDVal.Text = flaws.FlawID.ToString();
            lbFlawTypeVal.Text = flaws.FlawType.ToString();
            lbFlawClassVal.Text = flaws.FlawClass.ToString();
            lbAreaVal.Text = flaws.Area;
            lbMDVal.Text = flaws.MD.ToString();
            lbCDVal.Text = flaws.CD.ToString();
            lbWidthVal.Text = flaws.Width.ToString();
            lbLengthVal.Text = flaws.Length.ToString();

            for (int i = 0; i < PxPVariable.JobInfo.NumberOfStations; i++)
            {
                tcPicture.TabPages.Add("S" + ((i + 1).ToString()));
                tcPicture.TabPages[i].AutoScroll = true;

                pb[i] = new PictureBox();
                pb[i].Width = tcPicture.TabPages[i].Width;
                pb[i].Height = tcPicture.TabPages[i].Height;
                pb[i].SizeMode = PictureBoxSizeMode.Zoom;
                pb[i].Location = new Point(0, 0);

                pb[i].MouseDown += new MouseEventHandler(pb_MouseDown);
                pb[i].MouseMove += new MouseEventHandler(pb_MouseMove);
                pb[i].MouseUp += new MouseEventHandler(pb_MouseUp);
                pb[i].MouseClick += new MouseEventHandler(pb_Click);
                pb[i].MouseDoubleClick += new MouseEventHandler(pb_MouseDoubleClick);
                pb[i].Cursor = System.Windows.Forms.Cursors.Hand;

                tcPicture.TabPages[i].Controls.Add(pb[i]);
            }

            foreach (IImageInfo image in flaws.Images)
            {
                SrcImg[image.Station] = image.Image;
                pb_ratio[image.Station] = Init_Image(image.Image, tcPicture.TabPages[image.Station], pb[image.Station]);
            }
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            PicZoom("IN");
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            PicZoom("OUT");
        }

        private void tkbImage_Scroll(object sender, EventArgs e)
        {
            PicZoomByPercent(tkbImage.Value);
        }

        private void pb_MouseDown(object sender, MouseEventArgs e)
        {
            this.MousePt = e.Location;
            FormDragging = true;
        }

        private void pb_MouseMove(object sender, MouseEventArgs e)
        {
            if (FormDragging)
            {
                tcPicture.TabPages[tcPicture.SelectedIndex].AutoScrollPosition = new Point(-tcPicture.TabPages[tcPicture.SelectedIndex].AutoScrollPosition.X + (MousePt.X - e.X),
                    -tcPicture.TabPages[tcPicture.SelectedIndex].AutoScrollPosition.Y + (MousePt.Y - e.Y));
            }
        }

        private void pb_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            FormDragging = false;
        }

        public void pb_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            tkbImage.Value = 100;
            PicZoomByPercent(tkbImage.Value);
        }

        public void pb_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                tkbImage.Focus();
            }
        }

        #endregion

        #region Method

        public double Init_Image(Bitmap bmp, TabPage tp, PictureBox pb)
        {
            double Width_d = (double)bmp.Width / (double)tp.ClientSize.Width;
            double Height_d = (double)bmp.Height / (double)tp.ClientSize.Height;
            double ratio = 1.0;
            if (Width_d > 1 || Height_d > 1)
            {
                if (Width_d > Height_d)
                {
                    ratio = Width_d;
                }
                else
                {
                    ratio = Height_d;
                }
            }
            else if (Width_d < 1 && Height_d < 1)
            {
                if (Width_d > Height_d)
                    ratio = Width_d;
                else
                    ratio = Height_d;
            }
            pb.Width = (int)Math.Round(bmp.Width / ratio);
            pb.Height = (int)Math.Round(bmp.Height / ratio);

            Image src = bmp;
            Bitmap dest = new Bitmap(pb.Width, pb.Height);

            Graphics g = Graphics.FromImage(dest);
            g.DrawImage(src, new Rectangle(0, 0, dest.Width, dest.Height));
            pb.Height = dest.Height;
            pb.Width = dest.Width;
            pb.Image = dest;
            return ratio;
        }

        public void PicZoom(string ZoomType)
        {
            PictureBox pb = null;
            foreach (Control control in tcPicture.SelectedTab.Controls)
            {

                if (control.GetType().Name == "PictureBox")
                {
                    pb = (PictureBox)control;
                    break;
                }
            }

            if (pb != null)
            {
                Image src = pb.Image;
                Bitmap dest = null;

                if (ZoomType.Equals("IN"))
                {
                    if (((double)src.Width * 2) >= ((double)SrcImg[tcPicture.SelectedIndex].Width / pb_ratio[tcPicture.SelectedIndex] * 4))
                    {
                        dest = new Bitmap(src.Width, src.Height);
                    }
                    else
                    {
                        dest = new Bitmap(src.Width * 2, src.Height * 2);
                    }
                }
                else if (ZoomType.Equals("OUT"))
                {
                    if (((double)src.Width / 2) <= ((double)SrcImg[tcPicture.SelectedIndex].Width / pb_ratio[tcPicture.SelectedIndex] / 6))
                    {
                        dest = new Bitmap(src.Width, src.Height);
                    }
                    else
                    {
                        dest = new Bitmap(src.Width / 2, src.Height / 2);
                    }
                }

                Graphics g = Graphics.FromImage(dest);
                g.DrawImage(SrcImg[tcPicture.SelectedIndex], new Rectangle(0, 0, dest.Width, dest.Height));
                pb.Height = dest.Height;
                pb.Width = dest.Width;
                pb.Image = dest;
            }
        }

        public void PicZoomByPercent(int ZoomPercent)
        {
            PictureBox pb = null;
            foreach (Control control in tcPicture.SelectedTab.Controls)
            {
                if (control.GetType().Name == "PictureBox")
                {
                    pb = (PictureBox)control;
                    break;
                }
            }

            if (pb != null)
            {
                Image src = pb.Image;
                Bitmap dest = null;

                int newWidth = (int)(((double)SrcImg[tcPicture.SelectedIndex].Width / pb_ratio[tcPicture.SelectedIndex]) * ((double)tkbImage.Value / 100));
                int newHeight = (int)(((double)SrcImg[tcPicture.SelectedIndex].Height / pb_ratio[tcPicture.SelectedIndex]) * ((double)tkbImage.Value / 100));
                dest = new Bitmap(newWidth, newHeight);

                Graphics g = Graphics.FromImage(dest);
                g.DrawImage(SrcImg[tcPicture.SelectedIndex], new Rectangle(0, 0, dest.Width, dest.Height));
                pb.Height = dest.Height;
                pb.Width = dest.Width;
                pb.Image = dest;
            }
        }

        #endregion
    }
}


