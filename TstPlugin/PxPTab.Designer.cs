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
            this.lbNothing = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
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
            this.btnPrevGrid.Location = new System.Drawing.Point(269, 700);
            this.btnPrevGrid.Name = "btnPrevGrid";
            this.btnPrevGrid.Size = new System.Drawing.Size(30, 30);
            this.btnPrevGrid.TabIndex = 2;
            this.btnPrevGrid.Text = "<";
            this.btnPrevGrid.UseVisualStyleBackColor = false;
            // 
            // btnNextGrid
            // 
            this.btnNextGrid.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnNextGrid.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnNextGrid.Location = new System.Drawing.Point(461, 700);
            this.btnNextGrid.Name = "btnNextGrid";
            this.btnNextGrid.Size = new System.Drawing.Size(30, 30);
            this.btnNextGrid.TabIndex = 3;
            this.btnNextGrid.Text = ">";
            this.btnNextGrid.UseVisualStyleBackColor = false;
            // 
            // lbNothing
            // 
            this.lbNothing.AutoSize = true;
            this.lbNothing.BackColor = System.Drawing.Color.Transparent;
            this.lbNothing.Font = new System.Drawing.Font("Ubuntu", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNothing.Location = new System.Drawing.Point(371, 705);
            this.lbNothing.Name = "lbNothing";
            this.lbNothing.Size = new System.Drawing.Size(17, 21);
            this.lbNothing.TabIndex = 5;
            this.lbNothing.Text = "/";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(399, 705);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "99";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(325, 705);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 23);
            this.label2.TabIndex = 7;
            this.label2.Text = "99";
            // 
            // PxPTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbNothing);
            this.Controls.Add(this.btnNextGrid);
            this.Controls.Add(this.btnPrevGrid);
            this.Controls.Add(this.tlpDoffGrid);
            this.Controls.Add(this.gvFlaw);
            this.Font = new System.Drawing.Font("新細明體-ExtB", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PxPTab";
            this.Size = new System.Drawing.Size(760, 747);
            ((System.ComponentModel.ISupportInitialize)(this.gvFlaw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFlaw)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView gvFlaw;
        private System.Windows.Forms.BindingSource bsFlaw;
        public System.Windows.Forms.TableLayoutPanel tlpDoffGrid;
        private System.Windows.Forms.Button btnPrevGrid;
        private System.Windows.Forms.Button btnNextGrid;
        private System.Windows.Forms.Label lbNothing;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
