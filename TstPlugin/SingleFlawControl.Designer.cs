namespace PxP
{
    partial class SingleFlawControl
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改這個方法的內容。
        ///
        /// </summary>
        private void InitializeComponent()
        {
            this.tabFlawControl = new System.Windows.Forms.TabControl();
            this.lbFlawID = new System.Windows.Forms.Label();
            this.tkbImg = new PxP.Toolkit.FusionTrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.tkbImg)).BeginInit();
            this.SuspendLayout();
            // 
            // tabFlawControl
            // 
            this.tabFlawControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabFlawControl.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tabFlawControl.Location = new System.Drawing.Point(2, 19);
            this.tabFlawControl.Name = "tabFlawControl";
            this.tabFlawControl.SelectedIndex = 0;
            this.tabFlawControl.Size = new System.Drawing.Size(206, 128);
            this.tabFlawControl.TabIndex = 0;
            this.tabFlawControl.DoubleClick += new System.EventHandler(this.tabFlawControl_DoubleClick);
            this.tabFlawControl.SizeChanged += new System.EventHandler(this.tabFlawControl_SizeChanged);
            // 
            // lbFlawID
            // 
            this.lbFlawID.AutoSize = true;
            this.lbFlawID.BackColor = System.Drawing.Color.Transparent;
            this.lbFlawID.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbFlawID.Location = new System.Drawing.Point(0, 0);
            this.lbFlawID.Name = "lbFlawID";
            this.lbFlawID.Size = new System.Drawing.Size(54, 16);
            this.lbFlawID.TabIndex = 1;
            this.lbFlawID.Text = "FlawID : ";
            // 
            // tkbImg
            // 
            this.tkbImg.Location = new System.Drawing.Point(91, -1);
            this.tkbImg.Maximum = 400;
            this.tkbImg.Minimum = 25;
            this.tkbImg.Name = "tkbImg";
            this.tkbImg.Size = new System.Drawing.Size(100, 45);
            this.tkbImg.TabIndex = 2;
            this.tkbImg.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tkbImg.Value = 100;
            this.tkbImg.Scroll += new System.EventHandler(this.tkbImg_Scroll);
            // 
            // SingleFlawControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tabFlawControl);
            this.Controls.Add(this.tkbImg);
            this.Controls.Add(this.lbFlawID);
            this.Name = "SingleFlawControl";
            this.Size = new System.Drawing.Size(210, 150);
            ((System.ComponentModel.ISupportInitialize)(this.tkbImg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabFlawControl;
        private System.Windows.Forms.Label lbFlawID;
        private PxP.Toolkit.FusionTrackBar tkbImg;
    }
}