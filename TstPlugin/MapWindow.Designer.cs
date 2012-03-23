namespace PxP
{
    partial class MapWindow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapWindow));
            this.nChart = new Nevron.Chart.WinForm.NChartControl();
            this.gvFlawClass = new System.Windows.Forms.DataGridView();
            this.bsFlawType = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gvFlawClass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFlawType)).BeginInit();
            this.SuspendLayout();
            // 
            // nChart
            // 
            this.nChart.AutoRefresh = false;
            this.nChart.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.nChart.InputKeys = new System.Windows.Forms.Keys[0];
            this.nChart.Location = new System.Drawing.Point(14, 199);
            this.nChart.Name = "nChart";
            this.nChart.Size = new System.Drawing.Size(622, 403);
            this.nChart.State = ((Nevron.Chart.WinForm.NState)(resources.GetObject("nChart.State")));
            this.nChart.TabIndex = 0;
            // 
            // gvFlawClass
            // 
            this.gvFlawClass.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvFlawClass.Location = new System.Drawing.Point(14, 625);
            this.gvFlawClass.Name = "gvFlawClass";
            this.gvFlawClass.RowHeadersVisible = false;
            this.gvFlawClass.RowTemplate.Height = 24;
            this.gvFlawClass.Size = new System.Drawing.Size(622, 208);
            this.gvFlawClass.TabIndex = 1;
            // 
            // MapWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.BackgroundImage = global::PxP.Properties.Resources.bgLeft;
            this.Controls.Add(this.gvFlawClass);
            this.Controls.Add(this.nChart);
            this.Name = "MapWindow";
            this.Size = new System.Drawing.Size(650, 848);
            ((System.ComponentModel.ISupportInitialize)(this.gvFlawClass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFlawType)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Nevron.Chart.WinForm.NChartControl nChart;
        private System.Windows.Forms.DataGridView gvFlawClass;
        private System.Windows.Forms.BindingSource bsFlawType;

    }
}
