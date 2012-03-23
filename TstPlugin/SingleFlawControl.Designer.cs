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
            this.SuspendLayout();
            // 
            // tabFlawControl
            // 
            this.tabFlawControl.Location = new System.Drawing.Point(2, 25);
            this.tabFlawControl.Name = "tabFlawControl";
            this.tabFlawControl.SelectedIndex = 0;
            this.tabFlawControl.Size = new System.Drawing.Size(206, 120);
            this.tabFlawControl.TabIndex = 0;
            // 
            // lbFlawID
            // 
            this.lbFlawID.AutoSize = true;
            this.lbFlawID.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbFlawID.Location = new System.Drawing.Point(3, 6);
            this.lbFlawID.Name = "lbFlawID";
            this.lbFlawID.Size = new System.Drawing.Size(54, 16);
            this.lbFlawID.TabIndex = 1;
            this.lbFlawID.Text = "FlawID : ";
            // 
            // SingleFlawControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::PxP.Properties.Resources.bgSingle;
            this.Controls.Add(this.lbFlawID);
            this.Controls.Add(this.tabFlawControl);
            this.Name = "SingleFlawControl";
            this.Size = new System.Drawing.Size(210, 150);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabFlawControl;
        private System.Windows.Forms.Label lbFlawID;
    }
}
