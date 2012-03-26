namespace PxP
{
    partial class PxPTab
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PxPTab));
            this.gvFlaw = new System.Windows.Forms.DataGridView();
            this.bsFlaw = new System.Windows.Forms.BindingSource(this.components);
            this.tlpDoffGrid = new System.Windows.Forms.TableLayoutPanel();
            this.btnPrevGrid = new System.Windows.Forms.Button();
            this.btnNextGrid = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gvFlaw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFlaw)).BeginInit();
            this.SuspendLayout();
            // 
            // gvFlaw
            // 
            this.gvFlaw.AllowUserToAddRows = false;
            this.gvFlaw.AllowUserToDeleteRows = false;
            this.gvFlaw.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvFlaw.Location = new System.Drawing.Point(12, 12);
            this.gvFlaw.Name = "gvFlaw";
            this.gvFlaw.ReadOnly = true;
            this.gvFlaw.RowHeadersVisible = false;
            this.gvFlaw.RowTemplate.Height = 24;
            this.gvFlaw.Size = new System.Drawing.Size(735, 196);
            this.gvFlaw.TabIndex = 0;
            // 
            // tlpDoffGrid
            // 
            this.tlpDoffGrid.BackColor = System.Drawing.Color.Transparent;
            this.tlpDoffGrid.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tlpDoffGrid.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tlpDoffGrid.ColumnCount = 2;
            this.tlpDoffGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpDoffGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpDoffGrid.Location = new System.Drawing.Point(12, 225);
            this.tlpDoffGrid.Name = "tlpDoffGrid";
            this.tlpDoffGrid.RowCount = 2;
            this.tlpDoffGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpDoffGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpDoffGrid.Size = new System.Drawing.Size(735, 448);
            this.tlpDoffGrid.TabIndex = 1;
            // 
            // btnPrevGrid
            // 
            this.btnPrevGrid.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnPrevGrid.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnPrevGrid.Location = new System.Drawing.Point(250, 695);
            this.btnPrevGrid.Name = "btnPrevGrid";
            this.btnPrevGrid.Size = new System.Drawing.Size(30, 30);
            this.btnPrevGrid.TabIndex = 2;
            this.btnPrevGrid.Text = "<";
            this.btnPrevGrid.UseVisualStyleBackColor = false;
            // 
            // btnNextGrid
            // 
            this.btnNextGrid.Location = new System.Drawing.Point(387, 695);
            this.btnNextGrid.Name = "btnNextGrid";
            this.btnNextGrid.Size = new System.Drawing.Size(30, 30);
            this.btnNextGrid.TabIndex = 3;
            this.btnNextGrid.Text = "button2";
            this.btnNextGrid.UseVisualStyleBackColor = true;
            // 
            // PxPTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.Controls.Add(this.btnNextGrid);
            this.Controls.Add(this.btnPrevGrid);
            this.Controls.Add(this.tlpDoffGrid);
            this.Controls.Add(this.gvFlaw);
            this.Name = "PxPTab";
            this.Size = new System.Drawing.Size(760, 747);
            ((System.ComponentModel.ISupportInitialize)(this.gvFlaw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFlaw)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gvFlaw;
        private System.Windows.Forms.BindingSource bsFlaw;
        private System.Windows.Forms.TableLayoutPanel tlpDoffGrid;
        private System.Windows.Forms.Button btnPrevGrid;
        private System.Windows.Forms.Button btnNextGrid;
    }
}
