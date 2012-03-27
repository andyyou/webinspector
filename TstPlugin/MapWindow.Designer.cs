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
            this.tlpMapInfo = new System.Windows.Forms.TableLayoutPanel();
            this.lbJob = new System.Windows.Forms.Label();
            this.lbPiece = new System.Windows.Forms.Label();
            this.lbFile = new System.Windows.Forms.Label();
            this.lbOperator = new System.Windows.Forms.Label();
            this.lbSize = new System.Windows.Forms.Label();
            this.lbDateTime = new System.Windows.Forms.Label();
            this.lbPass = new System.Windows.Forms.Label();
            this.lbFail = new System.Windows.Forms.Label();
            this.lbYield = new System.Windows.Forms.Label();
            this.lbMark = new System.Windows.Forms.Label();
            this.lbPassValue = new System.Windows.Forms.Label();
            this.lbFailValue = new System.Windows.Forms.Label();
            this.lbYieldValue = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.rbFail = new System.Windows.Forms.RadioButton();
            this.rbPass = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.gvFlawClass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFlawType)).BeginInit();
            this.tlpMapInfo.SuspendLayout();
            this.panel1.SuspendLayout();
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
            // tlpMapInfo
            // 
            this.tlpMapInfo.AutoSize = true;
            this.tlpMapInfo.BackColor = System.Drawing.Color.Transparent;
            this.tlpMapInfo.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tlpMapInfo.ColumnCount = 4;
            this.tlpMapInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpMapInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpMapInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpMapInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpMapInfo.Controls.Add(this.lbJob, 0, 0);
            this.tlpMapInfo.Controls.Add(this.lbPiece, 0, 1);
            this.tlpMapInfo.Controls.Add(this.lbFile, 0, 2);
            this.tlpMapInfo.Controls.Add(this.lbOperator, 0, 3);
            this.tlpMapInfo.Controls.Add(this.lbSize, 0, 4);
            this.tlpMapInfo.Controls.Add(this.lbDateTime, 0, 5);
            this.tlpMapInfo.Controls.Add(this.lbPass, 2, 0);
            this.tlpMapInfo.Controls.Add(this.lbFail, 2, 1);
            this.tlpMapInfo.Controls.Add(this.lbYield, 2, 2);
            this.tlpMapInfo.Controls.Add(this.lbMark, 2, 3);
            this.tlpMapInfo.Controls.Add(this.lbPassValue, 3, 0);
            this.tlpMapInfo.Controls.Add(this.lbFailValue, 3, 1);
            this.tlpMapInfo.Controls.Add(this.lbYieldValue, 3, 2);
            this.tlpMapInfo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tlpMapInfo.Location = new System.Drawing.Point(14, 17);
            this.tlpMapInfo.Margin = new System.Windows.Forms.Padding(5);
            this.tlpMapInfo.Name = "tlpMapInfo";
            this.tlpMapInfo.RowCount = 6;
            this.tlpMapInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpMapInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpMapInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpMapInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpMapInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpMapInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpMapInfo.Size = new System.Drawing.Size(500, 150);
            this.tlpMapInfo.TabIndex = 2;
            // 
            // lbJob
            // 
            this.lbJob.AutoEllipsis = true;
            this.lbJob.AutoSize = true;
            this.lbJob.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbJob.ForeColor = System.Drawing.Color.Black;
            this.lbJob.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lbJob.Location = new System.Drawing.Point(4, 1);
            this.lbJob.Name = "lbJob";
            this.lbJob.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbJob.Size = new System.Drawing.Size(42, 15);
            this.lbJob.TabIndex = 0;
            this.lbJob.Text = "Lot# ";
            // 
            // lbPiece
            // 
            this.lbPiece.AutoSize = true;
            this.lbPiece.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPiece.Location = new System.Drawing.Point(4, 25);
            this.lbPiece.Name = "lbPiece";
            this.lbPiece.Size = new System.Drawing.Size(49, 15);
            this.lbPiece.TabIndex = 1;
            this.lbPiece.Text = "Piece ";
            // 
            // lbFile
            // 
            this.lbFile.AutoSize = true;
            this.lbFile.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFile.Location = new System.Drawing.Point(4, 49);
            this.lbFile.Name = "lbFile";
            this.lbFile.Size = new System.Drawing.Size(35, 15);
            this.lbFile.TabIndex = 2;
            this.lbFile.Text = "File";
            // 
            // lbOperator
            // 
            this.lbOperator.AutoSize = true;
            this.lbOperator.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbOperator.Location = new System.Drawing.Point(4, 73);
            this.lbOperator.Name = "lbOperator";
            this.lbOperator.Size = new System.Drawing.Size(63, 15);
            this.lbOperator.TabIndex = 3;
            this.lbOperator.Text = "Operator";
            // 
            // lbSize
            // 
            this.lbSize.AutoSize = true;
            this.lbSize.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSize.Location = new System.Drawing.Point(4, 97);
            this.lbSize.Name = "lbSize";
            this.lbSize.Size = new System.Drawing.Size(35, 15);
            this.lbSize.TabIndex = 4;
            this.lbSize.Text = "Size";
            // 
            // lbDateTime
            // 
            this.lbDateTime.AutoSize = true;
            this.lbDateTime.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDateTime.Location = new System.Drawing.Point(4, 121);
            this.lbDateTime.Name = "lbDateTime";
            this.lbDateTime.Size = new System.Drawing.Size(63, 15);
            this.lbDateTime.TabIndex = 5;
            this.lbDateTime.Text = "DateTime";
            // 
            // lbPass
            // 
            this.lbPass.AutoSize = true;
            this.lbPass.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPass.Location = new System.Drawing.Point(252, 1);
            this.lbPass.Name = "lbPass";
            this.lbPass.Size = new System.Drawing.Size(35, 15);
            this.lbPass.TabIndex = 6;
            this.lbPass.Text = "Pass";
            // 
            // lbFail
            // 
            this.lbFail.AutoSize = true;
            this.lbFail.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFail.Location = new System.Drawing.Point(252, 25);
            this.lbFail.Name = "lbFail";
            this.lbFail.Size = new System.Drawing.Size(35, 15);
            this.lbFail.TabIndex = 7;
            this.lbFail.Text = "Fail";
            // 
            // lbYield
            // 
            this.lbYield.AutoSize = true;
            this.lbYield.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbYield.Location = new System.Drawing.Point(252, 49);
            this.lbYield.Name = "lbYield";
            this.lbYield.Size = new System.Drawing.Size(42, 15);
            this.lbYield.TabIndex = 8;
            this.lbYield.Text = "Yield";
            // 
            // lbMark
            // 
            this.lbMark.AutoSize = true;
            this.lbMark.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMark.Location = new System.Drawing.Point(252, 73);
            this.lbMark.Name = "lbMark";
            this.lbMark.Size = new System.Drawing.Size(35, 15);
            this.lbMark.TabIndex = 9;
            this.lbMark.Text = "Mark";
            // 
            // lbPassValue
            // 
            this.lbPassValue.AutoSize = true;
            this.lbPassValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPassValue.Location = new System.Drawing.Point(376, 1);
            this.lbPassValue.Name = "lbPassValue";
            this.lbPassValue.Size = new System.Drawing.Size(14, 15);
            this.lbPassValue.TabIndex = 10;
            this.lbPassValue.Text = "0";
            // 
            // lbFailValue
            // 
            this.lbFailValue.AutoSize = true;
            this.lbFailValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFailValue.Location = new System.Drawing.Point(376, 25);
            this.lbFailValue.Name = "lbFailValue";
            this.lbFailValue.Size = new System.Drawing.Size(14, 15);
            this.lbFailValue.TabIndex = 11;
            this.lbFailValue.Text = "0";
            // 
            // lbYieldValue
            // 
            this.lbYieldValue.AutoSize = true;
            this.lbYieldValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbYieldValue.Location = new System.Drawing.Point(376, 49);
            this.lbYieldValue.Name = "lbYieldValue";
            this.lbYieldValue.Size = new System.Drawing.Size(14, 15);
            this.lbYieldValue.TabIndex = 12;
            this.lbYieldValue.Text = "0";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.rbAll);
            this.panel1.Controls.Add(this.rbFail);
            this.panel1.Controls.Add(this.rbPass);
            this.panel1.Location = new System.Drawing.Point(522, 18);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(114, 100);
            this.panel1.TabIndex = 3;
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Checked = true;
            this.rbAll.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbAll.Location = new System.Drawing.Point(3, 47);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(46, 19);
            this.rbAll.TabIndex = 2;
            this.rbAll.TabStop = true;
            this.rbAll.Text = "All";
            this.rbAll.UseVisualStyleBackColor = true;
            // 
            // rbFail
            // 
            this.rbFail.AutoSize = true;
            this.rbFail.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbFail.Location = new System.Drawing.Point(3, 25);
            this.rbFail.Name = "rbFail";
            this.rbFail.Size = new System.Drawing.Size(53, 19);
            this.rbFail.TabIndex = 1;
            this.rbFail.TabStop = true;
            this.rbFail.Text = "Fail";
            this.rbFail.UseVisualStyleBackColor = true;
            // 
            // rbPass
            // 
            this.rbPass.AutoSize = true;
            this.rbPass.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbPass.Location = new System.Drawing.Point(3, 3);
            this.rbPass.Name = "rbPass";
            this.rbPass.Size = new System.Drawing.Size(53, 19);
            this.rbPass.TabIndex = 0;
            this.rbPass.TabStop = true;
            this.rbPass.Text = "Pass";
            this.rbPass.UseVisualStyleBackColor = true;
            // 
            // MapWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.BackgroundImage = global::PxP.Properties.Resources.bgLeft;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tlpMapInfo);
            this.Controls.Add(this.gvFlawClass);
            this.Controls.Add(this.nChart);
            this.Name = "MapWindow";
            this.Size = new System.Drawing.Size(650, 848);
            ((System.ComponentModel.ISupportInitialize)(this.gvFlawClass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFlawType)).EndInit();
            this.tlpMapInfo.ResumeLayout(false);
            this.tlpMapInfo.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Nevron.Chart.WinForm.NChartControl nChart;
        private System.Windows.Forms.DataGridView gvFlawClass;
        private System.Windows.Forms.BindingSource bsFlawType;
        private System.Windows.Forms.TableLayoutPanel tlpMapInfo;
        private System.Windows.Forms.Label lbJob;
        private System.Windows.Forms.Label lbPiece;
        private System.Windows.Forms.Label lbFile;
        private System.Windows.Forms.Label lbOperator;
        private System.Windows.Forms.Label lbSize;
        private System.Windows.Forms.Label lbDateTime;
        private System.Windows.Forms.Label lbPass;
        private System.Windows.Forms.Label lbFail;
        private System.Windows.Forms.Label lbYield;
        private System.Windows.Forms.Label lbMark;
        private System.Windows.Forms.Label lbPassValue;
        private System.Windows.Forms.Label lbFailValue;
        private System.Windows.Forms.Label lbYieldValue;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.RadioButton rbFail;
        private System.Windows.Forms.RadioButton rbPass;

    }
}
