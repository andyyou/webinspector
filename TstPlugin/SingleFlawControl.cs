using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WRPlugIn;
using System.Threading;

namespace PxP
{
    public partial class SingleFlawControl : UserControl
    {
        public PictureBox[] pb;
        private Label lbCoordinate;
        private double[] dRatio;
        private double dCurrentRatio = 0;
        private Image[] SrcImg;

        private FlawInfoAddPriority flaw;

        #region Constructor

        public SingleFlawControl(FlawInfoAddPriority info)
        {
            InitializeComponent();

            flaw = info;
            lbFlawID.Text += info.FlawID.ToString();
            lbCoordinate = new Label();
            lbCoordinate.BackColor = Color.LightSkyBlue;
            lbCoordinate.AutoSize = true;
            lbCoordinate.Visible = false;

            pb = new PictureBox[PxPVariable.JobInfo.NumberOfStations];
            SrcImg = new Image[PxPVariable.JobInfo.NumberOfStations];
            dRatio = new double[PxPVariable.JobInfo.NumberOfStations];

            for (int i = 0; i < PxPVariable.JobInfo.NumberOfStations; i++)
            {
                tabFlawControl.TabPages.Add("S" + ((i + 1).ToString()));
                pb[i] = new PictureBox();
                pb[i].SizeMode = PictureBoxSizeMode.Zoom;
                pb[i].Location = new Point(0, 0);
                pb[i].BackColor = Color.Transparent;
                pb[i].MouseClick += new MouseEventHandler(PictureBox_Click);
                //pb[i].DoubleClick += new EventHandler(SingleFlawControl_DoubleClick);
                pb[i].MouseMove += new MouseEventHandler(PictureBox_MouseMove);
                //pb[i].MouseHover += new EventHandler(PictureBox_MouseMove);
                pb[i].MouseLeave += new EventHandler(PictrueBox_MouseLeave);
                //pb[i].Click +=new EventHandler(PictureBox_Click);
                tabFlawControl.TabPages[i].AutoScroll = true;
                tabFlawControl.TabPages[i].Controls.Add(pb[i]);
                tabFlawControl.TabPages[i].BackColor = Color.Transparent;

            }

            foreach (IImageInfo image in info.Images)
            {
                SrcImg[image.Station] = image.Image;
                dRatio[image.Station] = Init_Image(image.Image, tabFlawControl.TabPages[image.Station], pb[image.Station]);
                //pb[image.Station].Image = image.Image;
            }

            this.Tag = info.FlawID;
        }

        #endregion

        #region Action Events

        private MouseEventArgs e;
        private object sender;
        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            this.e = e;
            this.sender = sender;
            new Thread(() =>
            {
                MethodInvoker GetThread = new MethodInvoker(GetLabel);
                this.BeginInvoke(GetThread);

            }).Start();
        }

        private void PictrueBox_MouseLeave(object sender, EventArgs e)
        {
            lbCoordinate.Visible = false;
            tabFlawControl.SelectedTab.Controls.Remove(lbCoordinate);
        }

        private void tabFlawControl_SizeChanged(object sender, EventArgs e)
        {
            foreach (IImageInfo image in flaw.Images)
            {
                Init_Image(image.Image, tabFlawControl.TabPages[image.Station], pb[image.Station]);
            }
        }

        #endregion

        #region Method

        private void GetLabel()
        {
            TabPage tp = tabFlawControl.SelectedTab;
            Point pbPoint = e.Location;
            dCurrentRatio = dRatio[tabFlawControl.SelectedIndex];
            int x = pbPoint.X + ((PictureBox)sender).Location.X;
            int y = pbPoint.Y + ((PictureBox)sender).Location.Y;
            int xResult = x + 10;
            int yResult = y + 10;
            lbCoordinate.Text = ((int)(pbPoint.X / dCurrentRatio)).ToString() + ", " + ((int)(pbPoint.Y / dCurrentRatio)).ToString();

            if (xResult + lbCoordinate.Width + 5 > tp.ClientSize.Width)
            {
                xResult = tp.ClientSize.Width - lbCoordinate.Width - 10;
            }

            if (yResult + lbCoordinate.Height + 5 > tp.ClientSize.Height)
            {
                yResult = tp.ClientSize.Height - lbCoordinate.Height - 10;
            }

            tabFlawControl.SelectedTab.Controls.Add(lbCoordinate);
            lbCoordinate.Location = new Point(xResult, yResult);
            lbCoordinate.Visible = true;
            lbCoordinate.BringToFront();
        }

        public double Init_Image(Bitmap bmp, TabPage tp, PictureBox pb)
        {
            double Width_d = (double)bmp.Width / (double)tp.ClientSize.Width * 1.1;
            double Height_d = (double)bmp.Height / (double)tp.ClientSize.Height * 1.1;
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

        public void PictureBox_Click(object sender, MouseEventArgs e)
        {
            PictureBox pb = null;
            foreach (Control control in tabFlawControl.SelectedTab.Controls)
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

                if (e.Button == MouseButtons.Left)
                {
                    if (((double)src.Width * 2) >= ((double)SrcImg[tabFlawControl.SelectedIndex].Width / dRatio[tabFlawControl.SelectedIndex] * 4))
                    {
                        dest = new Bitmap(src.Width, src.Height);
                    }
                    else
                    {
                        dest = new Bitmap(src.Width * 2, src.Height * 2);
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    //dest = new Bitmap(src.Width / 2, src.Height / 2);
                    if (((double)src.Width / 2) <= ((double)SrcImg[tabFlawControl.SelectedIndex].Width / dRatio[tabFlawControl.SelectedIndex] / 6))
                    {
                        dest = new Bitmap(src.Width, src.Height);
                    }
                    else
                    {
                        dest = new Bitmap(src.Width / 2, src.Height / 2);
                    }
                    //dRatio[tabFlawControl.SelectedIndex] /= 2;
                }

                Graphics g = Graphics.FromImage(dest);
                g.DrawImage(SrcImg[tabFlawControl.SelectedIndex], new Rectangle(0, 0, dest.Width, dest.Height));
                pb.Height = dest.Height;
                pb.Width = dest.Width;
                pb.Image = dest;
            }
        }

        #endregion

        private void tabFlawControl_DoubleClick(object sender, EventArgs e)
        {
            MapWindowThreadStatus.UpdateChange = true; 
            PxPVariable.ChooseFlawID = Convert.ToInt32(this.Tag);
            MapWindowThreadStatus.IsTableLayoutRefresh = true;
            PxPTab.MapThreadEvent.Set();
        }
    }
}