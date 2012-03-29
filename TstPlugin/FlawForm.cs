﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WRPlugIn;

namespace PxP
{
    public partial class FlawForm : Form
    {
        public FlawForm()
        {
            InitializeComponent();
        }

        #region Action Events
        
        #endregion

        public void GetFlawInfo(int PointIndex)
        {
            PictureBox pbFlaw = new PictureBox();
            pbFlaw.Width = tcPicture.Width;
            pbFlaw.Height = tcPicture.Height;
            pbFlaw.SizeMode = PictureBoxSizeMode.Zoom;
            pbFlaw.Location = new Point(0, 0);
            tcPicture.TabPages.Add("Flaw");
            tcPicture.TabPages[0].Controls.Add(pbFlaw);

            lbFlawIDVal.Text = MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece -1 ][PointIndex].FlawID.ToString();
            lbFlawTypeVal.Text = MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece -1][PointIndex].FlawType.ToString();
            lbFlawClassVal.Text = MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece -1][PointIndex].FlawClass.ToString();
            lbAreaVal.Text = MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece-1][PointIndex].Area.ToString("0.######");
            lbMDVal.Text = MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece-1][PointIndex].MD.ToString();
            lbCDVal.Text = MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece-1][PointIndex].CD.ToString();
            lbWidthVal.Text = MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece-1][PointIndex].Width.ToString();
            lbLengthVal.Text = MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece-1][PointIndex].Length.ToString();

            foreach (IImageInfo image in MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece-1][PointIndex].Images)
            {
                pbFlaw.Image = image.Image;
            }
        }
    }
}
