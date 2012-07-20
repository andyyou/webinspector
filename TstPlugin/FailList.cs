using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PxP
{
    public partial class FailList : Form
    {
        #region Local Variables

        int CurrentPieceTmp;
        bool btnPrevStatus, btnNextStatus;

        #endregion

        #region Constructor

        public FailList()
        {
            InitializeComponent();
        }

        ~FailList()
        {

        }

        #endregion

        #region Action Events

        private void FailList_Load(object sender, EventArgs e)
        {
            btnPrevStatus = MapWindowVariable.MapWindowController.btnPrevPiece.Enabled;
            btnNextStatus = MapWindowVariable.MapWindowController.btnNextPiece.Enabled;
            MapWindowVariable.MapWindowController.btnPrevPiece.Enabled = false;
            MapWindowVariable.MapWindowController.btnNextPiece.Enabled = false;
            CurrentPieceTmp = MapWindowVariable.CurrentPiece;
            foreach (KeyValuePair<int,bool> result in MapWindowVariable.PieceResult)
            {
                if (result.Value == false)
                {
                    gvFailList.Rows.Add(result.Key + 1, MapWindowVariable.FlawPieces[result.Key].Count);
                }
            }
        }

        private void gvFailList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ChangePiece((int)gvFailList.Rows[e.RowIndex].Cells[0].Value);
        }

        private void FailList_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CurrentPieceTmp > 0)
            {
                ChangePiece(CurrentPieceTmp);
            }

            MapWindowVariable.MapWindowController.btnPrevPiece.Enabled = btnPrevStatus;
            MapWindowVariable.MapWindowController.btnNextPiece.Enabled = btnNextStatus;
        }

        #endregion

        #region Method

        private void ChangePiece(int pieceNum)
        {
            if (gvFailList.Rows.Count > 0)
            {
                MapWindowVariable.CurrentPiece = pieceNum;
                MapWindowVariable.MapWindowController.CountFlawPieceDoffNum();
                MapWindowVariable.MapWindowController.lbPageCurrent.Text = MapWindowVariable.CurrentPiece.ToString();
                MapWindowVariable.MapWindowController.SetTotalScoreLabel(MapWindowVariable.MapWindowController.CountPieceScore(MapWindowVariable.FlawPieces[MapWindowVariable.CurrentPiece - 1]));
                MapWindowVariable.MapWindowController.DrawPieceFlaw(MapWindowVariable.CurrentPiece - 1, false);

                MapWindowThreadStatus.IsChangePiece = true;
                PxPTab.MapThreadEvent.Set();
            }
        }

        #endregion
    }
}
