namespace PxP
{
    partial class MapSetup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lbLabelConfig = new System.Windows.Forms.Label();
            this.cboxConfList = new System.Windows.Forms.ComboBox();
            this.gbImgSettings = new System.Windows.Forms.GroupBox();
            this.lbImgX = new System.Windows.Forms.Label();
            this.lbImgColumn = new System.Windows.Forms.Label();
            this.lbImgRow = new System.Windows.Forms.Label();
            this.ndImgRows = new System.Windows.Forms.NumericUpDown();
            this.ndImgCols = new System.Windows.Forms.NumericUpDown();
            this.gbMapSettings = new System.Windows.Forms.GroupBox();
            this.cbCDInverse = new System.Windows.Forms.CheckBox();
            this.cbMDInverse = new System.Windows.Forms.CheckBox();
            this.cboxButtomAxe = new System.Windows.Forms.ComboBox();
            this.lbBottomAxe = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbSCCD = new System.Windows.Forms.Label();
            this.lbSCMD = new System.Windows.Forms.Label();
            this.lbSCellCDUnit = new System.Windows.Forms.Label();
            this.lbSCellMDUnit = new System.Windows.Forms.Label();
            this.lbCountSizeCD = new System.Windows.Forms.Label();
            this.lbCountSizeMD = new System.Windows.Forms.Label();
            this.tboxCountSizeCD = new System.Windows.Forms.TextBox();
            this.tboxCountSizeMD = new System.Windows.Forms.TextBox();
            this.tboxFixSizeCD = new System.Windows.Forms.TextBox();
            this.tboxFixSizeMD = new System.Windows.Forms.TextBox();
            this.lbFixSizeCD = new System.Windows.Forms.Label();
            this.lbFixSizeMD = new System.Windows.Forms.Label();
            this.rbCountSize = new System.Windows.Forms.RadioButton();
            this.rbFixCellSize = new System.Windows.Forms.RadioButton();
            this.rbMapGridOff = new System.Windows.Forms.RadioButton();
            this.rbMapGridOn = new System.Windows.Forms.RadioButton();
            this.lbMapGridShow = new System.Windows.Forms.Label();
            this.cboxMapSize = new System.Windows.Forms.ComboBox();
            this.lbMapSize = new System.Windows.Forms.Label();
            this.gbSeriesSetting = new System.Windows.Forms.GroupBox();
            this.gvSeries = new System.Windows.Forms.DataGridView();
            this.rbLetter = new System.Windows.Forms.RadioButton();
            this.rbSharp = new System.Windows.Forms.RadioButton();
            this.btnSaveAs = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.bsConfList = new System.Windows.Forms.BindingSource(this.components);
            this.bsFlawTypeName = new System.Windows.Forms.BindingSource(this.components);
            this.gbImgSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ndImgRows)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndImgCols)).BeginInit();
            this.gbMapSettings.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gbSeriesSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvSeries)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsConfList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFlawTypeName)).BeginInit();
            this.SuspendLayout();
            // 
            // lbLabelConfig
            // 
            this.lbLabelConfig.AutoSize = true;
            this.lbLabelConfig.Location = new System.Drawing.Point(17, 20);
            this.lbLabelConfig.Name = "lbLabelConfig";
            this.lbLabelConfig.Size = new System.Drawing.Size(65, 12);
            this.lbLabelConfig.TabIndex = 0;
            this.lbLabelConfig.Text = "Load Config";
            // 
            // cboxConfList
            // 
            this.cboxConfList.FormattingEnabled = true;
            this.cboxConfList.Location = new System.Drawing.Point(94, 16);
            this.cboxConfList.Name = "cboxConfList";
            this.cboxConfList.Size = new System.Drawing.Size(171, 20);
            this.cboxConfList.TabIndex = 1;
            // 
            // gbImgSettings
            // 
            this.gbImgSettings.Controls.Add(this.lbImgX);
            this.gbImgSettings.Controls.Add(this.lbImgColumn);
            this.gbImgSettings.Controls.Add(this.lbImgRow);
            this.gbImgSettings.Controls.Add(this.ndImgRows);
            this.gbImgSettings.Controls.Add(this.ndImgCols);
            this.gbImgSettings.Location = new System.Drawing.Point(14, 55);
            this.gbImgSettings.Name = "gbImgSettings";
            this.gbImgSettings.Size = new System.Drawing.Size(251, 100);
            this.gbImgSettings.TabIndex = 2;
            this.gbImgSettings.TabStop = false;
            this.gbImgSettings.Text = "Image Grid Settings";
            // 
            // lbImgX
            // 
            this.lbImgX.AutoSize = true;
            this.lbImgX.Location = new System.Drawing.Point(63, 55);
            this.lbImgX.Name = "lbImgX";
            this.lbImgX.Size = new System.Drawing.Size(13, 12);
            this.lbImgX.TabIndex = 4;
            this.lbImgX.Text = "X";
            // 
            // lbImgColumn
            // 
            this.lbImgColumn.AutoSize = true;
            this.lbImgColumn.Location = new System.Drawing.Point(10, 25);
            this.lbImgColumn.Name = "lbImgColumn";
            this.lbImgColumn.Size = new System.Drawing.Size(47, 12);
            this.lbImgColumn.TabIndex = 3;
            this.lbImgColumn.Text = "Columns";
            // 
            // lbImgRow
            // 
            this.lbImgRow.AutoSize = true;
            this.lbImgRow.Location = new System.Drawing.Point(80, 25);
            this.lbImgRow.Name = "lbImgRow";
            this.lbImgRow.Size = new System.Drawing.Size(31, 12);
            this.lbImgRow.TabIndex = 2;
            this.lbImgRow.Text = "Rows";
            // 
            // ndImgRows
            // 
            this.ndImgRows.Location = new System.Drawing.Point(80, 50);
            this.ndImgRows.Name = "ndImgRows";
            this.ndImgRows.Size = new System.Drawing.Size(50, 22);
            this.ndImgRows.TabIndex = 1;
            // 
            // ndImgCols
            // 
            this.ndImgCols.Location = new System.Drawing.Point(10, 50);
            this.ndImgCols.Name = "ndImgCols";
            this.ndImgCols.Size = new System.Drawing.Size(50, 22);
            this.ndImgCols.TabIndex = 0;
            // 
            // gbMapSettings
            // 
            this.gbMapSettings.Controls.Add(this.cbCDInverse);
            this.gbMapSettings.Controls.Add(this.cbMDInverse);
            this.gbMapSettings.Controls.Add(this.cboxButtomAxe);
            this.gbMapSettings.Controls.Add(this.lbBottomAxe);
            this.gbMapSettings.Controls.Add(this.panel1);
            this.gbMapSettings.Controls.Add(this.rbMapGridOff);
            this.gbMapSettings.Controls.Add(this.rbMapGridOn);
            this.gbMapSettings.Controls.Add(this.lbMapGridShow);
            this.gbMapSettings.Controls.Add(this.cboxMapSize);
            this.gbMapSettings.Controls.Add(this.lbMapSize);
            this.gbMapSettings.Location = new System.Drawing.Point(14, 162);
            this.gbMapSettings.Name = "gbMapSettings";
            this.gbMapSettings.Size = new System.Drawing.Size(251, 302);
            this.gbMapSettings.TabIndex = 3;
            this.gbMapSettings.TabStop = false;
            this.gbMapSettings.Text = "Map Settings";
            // 
            // cbCDInverse
            // 
            this.cbCDInverse.AutoSize = true;
            this.cbCDInverse.Location = new System.Drawing.Point(113, 272);
            this.cbCDInverse.Name = "cbCDInverse";
            this.cbCDInverse.Size = new System.Drawing.Size(77, 16);
            this.cbCDInverse.TabIndex = 9;
            this.cbCDInverse.Text = "CD Inverse";
            this.cbCDInverse.UseVisualStyleBackColor = true;
            this.cbCDInverse.Visible = false;
            // 
            // cbMDInverse
            // 
            this.cbMDInverse.AutoSize = true;
            this.cbMDInverse.Location = new System.Drawing.Point(12, 273);
            this.cbMDInverse.Name = "cbMDInverse";
            this.cbMDInverse.Size = new System.Drawing.Size(79, 16);
            this.cbMDInverse.TabIndex = 8;
            this.cbMDInverse.Text = "MD Inverse";
            this.cbMDInverse.UseVisualStyleBackColor = true;
            this.cbMDInverse.Visible = false;
            // 
            // cboxButtomAxe
            // 
            this.cboxButtomAxe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxButtomAxe.FormattingEnabled = true;
            this.cboxButtomAxe.Items.AddRange(new object[] {
            "CD",
            "MD"});
            this.cboxButtomAxe.Location = new System.Drawing.Point(113, 228);
            this.cboxButtomAxe.Name = "cboxButtomAxe";
            this.cboxButtomAxe.Size = new System.Drawing.Size(121, 20);
            this.cboxButtomAxe.TabIndex = 7;
            // 
            // lbBottomAxe
            // 
            this.lbBottomAxe.AutoSize = true;
            this.lbBottomAxe.Location = new System.Drawing.Point(10, 232);
            this.lbBottomAxe.Name = "lbBottomAxe";
            this.lbBottomAxe.Size = new System.Drawing.Size(62, 12);
            this.lbBottomAxe.TabIndex = 6;
            this.lbBottomAxe.Text = "Bottom Axe";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbSCCD);
            this.panel1.Controls.Add(this.lbSCMD);
            this.panel1.Controls.Add(this.lbSCellCDUnit);
            this.panel1.Controls.Add(this.lbSCellMDUnit);
            this.panel1.Controls.Add(this.lbCountSizeCD);
            this.panel1.Controls.Add(this.lbCountSizeMD);
            this.panel1.Controls.Add(this.tboxCountSizeCD);
            this.panel1.Controls.Add(this.tboxCountSizeMD);
            this.panel1.Controls.Add(this.tboxFixSizeCD);
            this.panel1.Controls.Add(this.tboxFixSizeMD);
            this.panel1.Controls.Add(this.lbFixSizeCD);
            this.panel1.Controls.Add(this.lbFixSizeMD);
            this.panel1.Controls.Add(this.rbCountSize);
            this.panel1.Controls.Add(this.rbFixCellSize);
            this.panel1.Location = new System.Drawing.Point(12, 76);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(222, 144);
            this.panel1.TabIndex = 5;
            // 
            // lbSCCD
            // 
            this.lbSCCD.AutoSize = true;
            this.lbSCCD.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbSCCD.Location = new System.Drawing.Point(179, 49);
            this.lbSCCD.Name = "lbSCCD";
            this.lbSCCD.Size = new System.Drawing.Size(23, 12);
            this.lbSCCD.TabIndex = 13;
            this.lbSCCD.Text = "mm";
            // 
            // lbSCMD
            // 
            this.lbSCMD.AutoSize = true;
            this.lbSCMD.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbSCMD.Location = new System.Drawing.Point(179, 22);
            this.lbSCMD.Name = "lbSCMD";
            this.lbSCMD.Size = new System.Drawing.Size(23, 12);
            this.lbSCMD.TabIndex = 12;
            this.lbSCMD.Text = "mm";
            // 
            // lbSCellCDUnit
            // 
            this.lbSCellCDUnit.AutoSize = true;
            this.lbSCellCDUnit.Location = new System.Drawing.Point(178, 45);
            this.lbSCellCDUnit.Name = "lbSCellCDUnit";
            this.lbSCellCDUnit.Size = new System.Drawing.Size(0, 12);
            this.lbSCellCDUnit.TabIndex = 11;
            // 
            // lbSCellMDUnit
            // 
            this.lbSCellMDUnit.AutoSize = true;
            this.lbSCellMDUnit.Location = new System.Drawing.Point(184, 17);
            this.lbSCellMDUnit.Name = "lbSCellMDUnit";
            this.lbSCellMDUnit.Size = new System.Drawing.Size(0, 12);
            this.lbSCellMDUnit.TabIndex = 10;
            // 
            // lbCountSizeCD
            // 
            this.lbCountSizeCD.AutoSize = true;
            this.lbCountSizeCD.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.lbCountSizeCD.Location = new System.Drawing.Point(120, 108);
            this.lbCountSizeCD.Name = "lbCountSizeCD";
            this.lbCountSizeCD.Size = new System.Drawing.Size(21, 12);
            this.lbCountSizeCD.TabIndex = 9;
            this.lbCountSizeCD.Text = "CD";
            // 
            // lbCountSizeMD
            // 
            this.lbCountSizeMD.AutoSize = true;
            this.lbCountSizeMD.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.lbCountSizeMD.Location = new System.Drawing.Point(118, 80);
            this.lbCountSizeMD.Name = "lbCountSizeMD";
            this.lbCountSizeMD.Size = new System.Drawing.Size(23, 12);
            this.lbCountSizeMD.TabIndex = 8;
            this.lbCountSizeMD.Text = "MD";
            // 
            // tboxCountSizeCD
            // 
            this.tboxCountSizeCD.Location = new System.Drawing.Point(144, 103);
            this.tboxCountSizeCD.Name = "tboxCountSizeCD";
            this.tboxCountSizeCD.Size = new System.Drawing.Size(34, 22);
            this.tboxCountSizeCD.TabIndex = 7;
            // 
            // tboxCountSizeMD
            // 
            this.tboxCountSizeMD.Location = new System.Drawing.Point(144, 75);
            this.tboxCountSizeMD.Name = "tboxCountSizeMD";
            this.tboxCountSizeMD.Size = new System.Drawing.Size(34, 22);
            this.tboxCountSizeMD.TabIndex = 6;
            // 
            // tboxFixSizeCD
            // 
            this.tboxFixSizeCD.Location = new System.Drawing.Point(144, 40);
            this.tboxFixSizeCD.Name = "tboxFixSizeCD";
            this.tboxFixSizeCD.Size = new System.Drawing.Size(34, 22);
            this.tboxFixSizeCD.TabIndex = 5;
            // 
            // tboxFixSizeMD
            // 
            this.tboxFixSizeMD.Location = new System.Drawing.Point(144, 12);
            this.tboxFixSizeMD.Name = "tboxFixSizeMD";
            this.tboxFixSizeMD.Size = new System.Drawing.Size(34, 22);
            this.tboxFixSizeMD.TabIndex = 4;
            // 
            // lbFixSizeCD
            // 
            this.lbFixSizeCD.AutoSize = true;
            this.lbFixSizeCD.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbFixSizeCD.Location = new System.Drawing.Point(120, 45);
            this.lbFixSizeCD.Name = "lbFixSizeCD";
            this.lbFixSizeCD.Size = new System.Drawing.Size(21, 12);
            this.lbFixSizeCD.TabIndex = 3;
            this.lbFixSizeCD.Text = "CD";
            // 
            // lbFixSizeMD
            // 
            this.lbFixSizeMD.AutoSize = true;
            this.lbFixSizeMD.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbFixSizeMD.Location = new System.Drawing.Point(118, 17);
            this.lbFixSizeMD.Name = "lbFixSizeMD";
            this.lbFixSizeMD.Size = new System.Drawing.Size(23, 12);
            this.lbFixSizeMD.TabIndex = 2;
            this.lbFixSizeMD.Text = "MD";
            // 
            // rbCountSize
            // 
            this.rbCountSize.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.rbCountSize.Location = new System.Drawing.Point(11, 72);
            this.rbCountSize.Name = "rbCountSize";
            this.rbCountSize.Size = new System.Drawing.Size(200, 60);
            this.rbCountSize.TabIndex = 1;
            this.rbCountSize.TabStop = true;
            this.rbCountSize.Text = "Equal Cell Count";
            this.rbCountSize.UseVisualStyleBackColor = false;
            this.rbCountSize.CheckedChanged += new System.EventHandler(this.rbCountSize_CheckedChanged);
            // 
            // rbFixCellSize
            // 
            this.rbFixCellSize.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.rbFixCellSize.Location = new System.Drawing.Point(11, 7);
            this.rbFixCellSize.Name = "rbFixCellSize";
            this.rbFixCellSize.Size = new System.Drawing.Size(200, 60);
            this.rbFixCellSize.TabIndex = 0;
            this.rbFixCellSize.TabStop = true;
            this.rbFixCellSize.Text = "Specify Cell Size";
            this.rbFixCellSize.UseVisualStyleBackColor = false;
            this.rbFixCellSize.CheckedChanged += new System.EventHandler(this.rbFixCellSize_CheckedChanged);
            // 
            // rbMapGridOff
            // 
            this.rbMapGridOff.AutoSize = true;
            this.rbMapGridOff.Location = new System.Drawing.Point(156, 44);
            this.rbMapGridOff.Name = "rbMapGridOff";
            this.rbMapGridOff.Size = new System.Drawing.Size(39, 16);
            this.rbMapGridOff.TabIndex = 4;
            this.rbMapGridOff.TabStop = true;
            this.rbMapGridOff.Text = "Off";
            this.rbMapGridOff.UseVisualStyleBackColor = true;
            // 
            // rbMapGridOn
            // 
            this.rbMapGridOn.AutoSize = true;
            this.rbMapGridOn.Location = new System.Drawing.Point(113, 44);
            this.rbMapGridOn.Name = "rbMapGridOn";
            this.rbMapGridOn.Size = new System.Drawing.Size(37, 16);
            this.rbMapGridOn.TabIndex = 3;
            this.rbMapGridOn.TabStop = true;
            this.rbMapGridOn.Text = "On";
            this.rbMapGridOn.UseVisualStyleBackColor = true;
            // 
            // lbMapGridShow
            // 
            this.lbMapGridShow.AutoSize = true;
            this.lbMapGridShow.Location = new System.Drawing.Point(10, 48);
            this.lbMapGridShow.Name = "lbMapGridShow";
            this.lbMapGridShow.Size = new System.Drawing.Size(79, 12);
            this.lbMapGridShow.TabIndex = 2;
            this.lbMapGridShow.Text = "Map Grid Show";
            // 
            // cboxMapSize
            // 
            this.cboxMapSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxMapSize.FormattingEnabled = true;
            this.cboxMapSize.Items.AddRange(new object[] {
            "1:1",
            "2:1 ",
            "4:3 ",
            "3:4 ",
            "16:9"});
            this.cboxMapSize.Location = new System.Drawing.Point(113, 14);
            this.cboxMapSize.Name = "cboxMapSize";
            this.cboxMapSize.Size = new System.Drawing.Size(121, 20);
            this.cboxMapSize.TabIndex = 1;
            // 
            // lbMapSize
            // 
            this.lbMapSize.AutoSize = true;
            this.lbMapSize.Location = new System.Drawing.Point(10, 22);
            this.lbMapSize.Name = "lbMapSize";
            this.lbMapSize.Size = new System.Drawing.Size(48, 12);
            this.lbMapSize.TabIndex = 0;
            this.lbMapSize.Text = "Map Size";
            // 
            // gbSeriesSetting
            // 
            this.gbSeriesSetting.Controls.Add(this.gvSeries);
            this.gbSeriesSetting.Controls.Add(this.rbLetter);
            this.gbSeriesSetting.Controls.Add(this.rbSharp);
            this.gbSeriesSetting.Location = new System.Drawing.Point(271, 55);
            this.gbSeriesSetting.Name = "gbSeriesSetting";
            this.gbSeriesSetting.Size = new System.Drawing.Size(394, 409);
            this.gbSeriesSetting.TabIndex = 4;
            this.gbSeriesSetting.TabStop = false;
            this.gbSeriesSetting.Text = "Series Settings";
            // 
            // gvSeries
            // 
            this.gvSeries.AllowUserToAddRows = false;
            this.gvSeries.AllowUserToDeleteRows = false;
            this.gvSeries.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvSeries.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gvSeries.Location = new System.Drawing.Point(6, 50);
            this.gvSeries.Name = "gvSeries";
            this.gvSeries.RowHeadersVisible = false;
            this.gvSeries.RowTemplate.Height = 24;
            this.gvSeries.Size = new System.Drawing.Size(382, 301);
            this.gvSeries.TabIndex = 2;
            this.gvSeries.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvSeries_CellDoubleClick);
            this.gvSeries.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gvSeries_CellFormatting);
            // 
            // rbLetter
            // 
            this.rbLetter.AutoSize = true;
            this.rbLetter.Location = new System.Drawing.Point(75, 25);
            this.rbLetter.Name = "rbLetter";
            this.rbLetter.Size = new System.Drawing.Size(50, 16);
            this.rbLetter.TabIndex = 1;
            this.rbLetter.TabStop = true;
            this.rbLetter.Text = "Letter";
            this.rbLetter.UseVisualStyleBackColor = true;
            this.rbLetter.Visible = false;
            // 
            // rbSharp
            // 
            this.rbSharp.AutoSize = true;
            this.rbSharp.Location = new System.Drawing.Point(20, 25);
            this.rbSharp.Name = "rbSharp";
            this.rbSharp.Size = new System.Drawing.Size(50, 16);
            this.rbSharp.TabIndex = 0;
            this.rbSharp.TabStop = true;
            this.rbSharp.Text = "Sharp";
            this.rbSharp.UseVisualStyleBackColor = true;
            this.rbSharp.Visible = false;
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.Location = new System.Drawing.Point(428, 477);
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(75, 23);
            this.btnSaveAs.TabIndex = 5;
            this.btnSaveAs.Text = "Save As";
            this.btnSaveAs.UseVisualStyleBackColor = true;
            this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(509, 477);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(75, 23);
            this.btnConfirm.TabIndex = 6;
            this.btnConfirm.Text = "Apply";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(590, 477);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // MapSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 512);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.btnSaveAs);
            this.Controls.Add(this.gbSeriesSetting);
            this.Controls.Add(this.gbMapSettings);
            this.Controls.Add(this.gbImgSettings);
            this.Controls.Add(this.cboxConfList);
            this.Controls.Add(this.lbLabelConfig);
            this.Name = "MapSetup";
            this.Text = "MapSetup";
            this.gbImgSettings.ResumeLayout(false);
            this.gbImgSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ndImgRows)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndImgCols)).EndInit();
            this.gbMapSettings.ResumeLayout(false);
            this.gbMapSettings.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gbSeriesSetting.ResumeLayout(false);
            this.gbSeriesSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvSeries)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsConfList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFlawTypeName)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbLabelConfig;
        private System.Windows.Forms.ComboBox cboxConfList;
        private System.Windows.Forms.GroupBox gbImgSettings;
        private System.Windows.Forms.NumericUpDown ndImgRows;
        private System.Windows.Forms.NumericUpDown ndImgCols;
        private System.Windows.Forms.Label lbImgX;
        private System.Windows.Forms.Label lbImgColumn;
        private System.Windows.Forms.Label lbImgRow;
        private System.Windows.Forms.GroupBox gbMapSettings;
        private System.Windows.Forms.ComboBox cboxMapSize;
        private System.Windows.Forms.Label lbMapSize;
        private System.Windows.Forms.RadioButton rbMapGridOn;
        private System.Windows.Forms.Label lbMapGridShow;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbCountSizeCD;
        private System.Windows.Forms.Label lbCountSizeMD;
        private System.Windows.Forms.TextBox tboxCountSizeCD;
        private System.Windows.Forms.TextBox tboxCountSizeMD;
        private System.Windows.Forms.TextBox tboxFixSizeCD;
        private System.Windows.Forms.TextBox tboxFixSizeMD;
        private System.Windows.Forms.Label lbFixSizeCD;
        private System.Windows.Forms.Label lbFixSizeMD;
        private System.Windows.Forms.RadioButton rbCountSize;
        private System.Windows.Forms.RadioButton rbFixCellSize;
        private System.Windows.Forms.RadioButton rbMapGridOff;
        private System.Windows.Forms.Label lbBottomAxe;
        private System.Windows.Forms.ComboBox cboxButtomAxe;
        private System.Windows.Forms.CheckBox cbCDInverse;
        private System.Windows.Forms.CheckBox cbMDInverse;
        private System.Windows.Forms.GroupBox gbSeriesSetting;
        private System.Windows.Forms.DataGridView gvSeries;
        private System.Windows.Forms.RadioButton rbLetter;
        private System.Windows.Forms.RadioButton rbSharp;
        private System.Windows.Forms.Button btnSaveAs;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.BindingSource bsConfList;
        private System.Windows.Forms.BindingSource bsFlawTypeName;
        private System.Windows.Forms.Label lbSCellCDUnit;
        private System.Windows.Forms.Label lbSCellMDUnit;
        private System.Windows.Forms.Label lbSCCD;
        private System.Windows.Forms.Label lbSCMD;
    }
}