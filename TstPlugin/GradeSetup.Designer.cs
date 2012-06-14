namespace PxP
{
    partial class GradeSetup
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
            this.tabGradeSetup = new System.Windows.Forms.TabControl();
            this.tpROI = new System.Windows.Forms.TabPage();
            this.panelCreateGrid = new System.Windows.Forms.Panel();
            this.lbShowGvRow = new System.Windows.Forms.Label();
            this.lbShowGvColumn = new System.Windows.Forms.Label();
            this.gvRows = new System.Windows.Forms.DataGridView();
            this.gvColumns = new System.Windows.Forms.DataGridView();
            this.btnCreateGrid = new System.Windows.Forms.Button();
            this.lbX = new System.Windows.Forms.Label();
            this.lbRows = new System.Windows.Forms.Label();
            this.lbColumns = new System.Windows.Forms.Label();
            this.tboxColumns = new System.Windows.Forms.TextBox();
            this.tboxRows = new System.Windows.Forms.TextBox();
            this.gbROIItem = new System.Windows.Forms.GroupBox();
            this.rbSymmetrical = new System.Windows.Forms.RadioButton();
            this.rbNoRoi = new System.Windows.Forms.RadioButton();
            this.tpGradeGroup = new System.Windows.Forms.TabPage();
            this.tabGradeSetting = new System.Windows.Forms.TabControl();
            this.tpPoint = new System.Windows.Forms.TabPage();
            this.gbPointSetting = new System.Windows.Forms.GroupBox();
            this.gvPoint = new System.Windows.Forms.DataGridView();
            this.lbSubPieceOfPoint = new System.Windows.Forms.Label();
            this.cboxSubPieceOfPoint = new System.Windows.Forms.ComboBox();
            this.cboxEnablePTS = new System.Windows.Forms.CheckBox();
            this.tpGradeLevel = new System.Windows.Forms.TabPage();
            this.cboxEnableGrade = new System.Windows.Forms.CheckBox();
            this.gbGradeSetting = new System.Windows.Forms.GroupBox();
            this.gvGrade = new System.Windows.Forms.DataGridView();
            this.lbSubPieceOfGrade = new System.Windows.Forms.Label();
            this.cboxSubPieceOfGrade = new System.Windows.Forms.ComboBox();
            this.tpPassOrFail = new System.Windows.Forms.TabPage();
            this.lbScore = new System.Windows.Forms.Label();
            this.tboxFilterScore = new System.Windows.Forms.TextBox();
            this.cboxEnablePFS = new System.Windows.Forms.CheckBox();
            this.cboxGradeConfigFile = new System.Windows.Forms.ComboBox();
            this.lbGradeConfig = new System.Windows.Forms.Label();
            this.btnSaveGradeConfigFile = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.bsGradConfigList = new System.Windows.Forms.BindingSource(this.components);
            this.bsColumns = new System.Windows.Forms.BindingSource(this.components);
            this.bsRows = new System.Windows.Forms.BindingSource(this.components);
            this.bsRoiList = new System.Windows.Forms.BindingSource(this.components);
            this.bsPointSubPiece = new System.Windows.Forms.BindingSource(this.components);
            this.bsMarkSubPiece = new System.Windows.Forms.BindingSource(this.components);
            this.tabGradeSetup.SuspendLayout();
            this.tpROI.SuspendLayout();
            this.panelCreateGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvRows)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvColumns)).BeginInit();
            this.gbROIItem.SuspendLayout();
            this.tpGradeGroup.SuspendLayout();
            this.tabGradeSetting.SuspendLayout();
            this.tpPoint.SuspendLayout();
            this.gbPointSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvPoint)).BeginInit();
            this.tpGradeLevel.SuspendLayout();
            this.gbGradeSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvGrade)).BeginInit();
            this.tpPassOrFail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsGradConfigList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsRows)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsRoiList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsPointSubPiece)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsMarkSubPiece)).BeginInit();
            this.SuspendLayout();
            // 
            // tabGradeSetup
            // 
            this.tabGradeSetup.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabGradeSetup.Controls.Add(this.tpROI);
            this.tabGradeSetup.Controls.Add(this.tpGradeGroup);
            this.tabGradeSetup.Location = new System.Drawing.Point(3, 3);
            this.tabGradeSetup.Margin = new System.Windows.Forms.Padding(0);
            this.tabGradeSetup.Name = "tabGradeSetup";
            this.tabGradeSetup.Padding = new System.Drawing.Point(0, 0);
            this.tabGradeSetup.SelectedIndex = 0;
            this.tabGradeSetup.Size = new System.Drawing.Size(527, 451);
            this.tabGradeSetup.TabIndex = 0;
            // 
            // tpROI
            // 
            this.tpROI.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tpROI.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tpROI.Controls.Add(this.panelCreateGrid);
            this.tpROI.Controls.Add(this.gbROIItem);
            this.tpROI.Location = new System.Drawing.Point(4, 25);
            this.tpROI.Margin = new System.Windows.Forms.Padding(0);
            this.tpROI.Name = "tpROI";
            this.tpROI.Size = new System.Drawing.Size(519, 422);
            this.tpROI.TabIndex = 0;
            this.tpROI.Text = "ROI Setting";
            // 
            // panelCreateGrid
            // 
            this.panelCreateGrid.Controls.Add(this.lbShowGvRow);
            this.panelCreateGrid.Controls.Add(this.lbShowGvColumn);
            this.panelCreateGrid.Controls.Add(this.gvRows);
            this.panelCreateGrid.Controls.Add(this.gvColumns);
            this.panelCreateGrid.Controls.Add(this.btnCreateGrid);
            this.panelCreateGrid.Controls.Add(this.lbX);
            this.panelCreateGrid.Controls.Add(this.lbRows);
            this.panelCreateGrid.Controls.Add(this.lbColumns);
            this.panelCreateGrid.Controls.Add(this.tboxColumns);
            this.panelCreateGrid.Controls.Add(this.tboxRows);
            this.panelCreateGrid.Location = new System.Drawing.Point(8, 113);
            this.panelCreateGrid.Name = "panelCreateGrid";
            this.panelCreateGrid.Size = new System.Drawing.Size(504, 305);
            this.panelCreateGrid.TabIndex = 1;
            // 
            // lbShowGvRow
            // 
            this.lbShowGvRow.AutoSize = true;
            this.lbShowGvRow.Location = new System.Drawing.Point(256, 61);
            this.lbShowGvRow.Name = "lbShowGvRow";
            this.lbShowGvRow.Size = new System.Drawing.Size(31, 12);
            this.lbShowGvRow.TabIndex = 9;
            this.lbShowGvRow.Text = "Rows";
            // 
            // lbShowGvColumn
            // 
            this.lbShowGvColumn.AutoSize = true;
            this.lbShowGvColumn.Location = new System.Drawing.Point(6, 61);
            this.lbShowGvColumn.Name = "lbShowGvColumn";
            this.lbShowGvColumn.Size = new System.Drawing.Size(47, 12);
            this.lbShowGvColumn.TabIndex = 8;
            this.lbShowGvColumn.Text = "Columns";
            // 
            // gvRows
            // 
            this.gvRows.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvRows.Location = new System.Drawing.Point(256, 79);
            this.gvRows.Name = "gvRows";
            this.gvRows.RowTemplate.Height = 24;
            this.gvRows.Size = new System.Drawing.Size(240, 200);
            this.gvRows.TabIndex = 7;
            // 
            // gvColumns
            // 
            this.gvColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvColumns.Location = new System.Drawing.Point(6, 79);
            this.gvColumns.Name = "gvColumns";
            this.gvColumns.RowTemplate.Height = 24;
            this.gvColumns.Size = new System.Drawing.Size(240, 200);
            this.gvColumns.TabIndex = 6;
            // 
            // btnCreateGrid
            // 
            this.btnCreateGrid.Location = new System.Drawing.Point(245, 26);
            this.btnCreateGrid.Name = "btnCreateGrid";
            this.btnCreateGrid.Size = new System.Drawing.Size(75, 23);
            this.btnCreateGrid.TabIndex = 5;
            this.btnCreateGrid.Text = "Create";
            this.btnCreateGrid.UseVisualStyleBackColor = true;
            this.btnCreateGrid.Click += new System.EventHandler(this.btnCreateGrid_Click);
            // 
            // lbX
            // 
            this.lbX.AutoSize = true;
            this.lbX.Location = new System.Drawing.Point(112, 32);
            this.lbX.Name = "lbX";
            this.lbX.Size = new System.Drawing.Size(13, 12);
            this.lbX.TabIndex = 4;
            this.lbX.Text = "X";
            // 
            // lbRows
            // 
            this.lbRows.AutoSize = true;
            this.lbRows.Location = new System.Drawing.Point(130, 8);
            this.lbRows.Name = "lbRows";
            this.lbRows.Size = new System.Drawing.Size(31, 12);
            this.lbRows.TabIndex = 3;
            this.lbRows.Text = "Rows";
            // 
            // lbColumns
            // 
            this.lbColumns.AutoSize = true;
            this.lbColumns.Location = new System.Drawing.Point(5, 8);
            this.lbColumns.Name = "lbColumns";
            this.lbColumns.Size = new System.Drawing.Size(47, 12);
            this.lbColumns.TabIndex = 2;
            this.lbColumns.Text = "Columns";
            // 
            // tboxColumns
            // 
            this.tboxColumns.Location = new System.Drawing.Point(5, 27);
            this.tboxColumns.Name = "tboxColumns";
            this.tboxColumns.Size = new System.Drawing.Size(100, 22);
            this.tboxColumns.TabIndex = 1;
            // 
            // tboxRows
            // 
            this.tboxRows.Location = new System.Drawing.Point(130, 27);
            this.tboxRows.Name = "tboxRows";
            this.tboxRows.Size = new System.Drawing.Size(100, 22);
            this.tboxRows.TabIndex = 0;
            // 
            // gbROIItem
            // 
            this.gbROIItem.Controls.Add(this.rbSymmetrical);
            this.gbROIItem.Controls.Add(this.rbNoRoi);
            this.gbROIItem.Location = new System.Drawing.Point(6, 7);
            this.gbROIItem.Name = "gbROIItem";
            this.gbROIItem.Size = new System.Drawing.Size(506, 100);
            this.gbROIItem.TabIndex = 0;
            this.gbROIItem.TabStop = false;
            this.gbROIItem.Text = "ROI Settings";
            // 
            // rbSymmetrical
            // 
            this.rbSymmetrical.AutoSize = true;
            this.rbSymmetrical.Location = new System.Drawing.Point(7, 45);
            this.rbSymmetrical.Name = "rbSymmetrical";
            this.rbSymmetrical.Size = new System.Drawing.Size(81, 16);
            this.rbSymmetrical.TabIndex = 1;
            this.rbSymmetrical.TabStop = true;
            this.rbSymmetrical.Text = "Symmetrical";
            this.rbSymmetrical.UseVisualStyleBackColor = true;
            // 
            // rbNoRoi
            // 
            this.rbNoRoi.AutoSize = true;
            this.rbNoRoi.Location = new System.Drawing.Point(7, 22);
            this.rbNoRoi.Name = "rbNoRoi";
            this.rbNoRoi.Size = new System.Drawing.Size(60, 16);
            this.rbNoRoi.TabIndex = 0;
            this.rbNoRoi.TabStop = true;
            this.rbNoRoi.Text = "No ROI";
            this.rbNoRoi.UseVisualStyleBackColor = true;
            this.rbNoRoi.CheckedChanged += new System.EventHandler(this.rbNoRoi_CheckedChanged);
            // 
            // tpGradeGroup
            // 
            this.tpGradeGroup.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tpGradeGroup.Controls.Add(this.tabGradeSetting);
            this.tpGradeGroup.Location = new System.Drawing.Point(4, 25);
            this.tpGradeGroup.Name = "tpGradeGroup";
            this.tpGradeGroup.Padding = new System.Windows.Forms.Padding(3);
            this.tpGradeGroup.Size = new System.Drawing.Size(519, 422);
            this.tpGradeGroup.TabIndex = 1;
            this.tpGradeGroup.Text = "Grade Setting";
            // 
            // tabGradeSetting
            // 
            this.tabGradeSetting.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabGradeSetting.Controls.Add(this.tpPoint);
            this.tabGradeSetting.Controls.Add(this.tpGradeLevel);
            this.tabGradeSetting.Controls.Add(this.tpPassOrFail);
            this.tabGradeSetting.Location = new System.Drawing.Point(8, 9);
            this.tabGradeSetting.Name = "tabGradeSetting";
            this.tabGradeSetting.SelectedIndex = 0;
            this.tabGradeSetting.Size = new System.Drawing.Size(505, 410);
            this.tabGradeSetting.TabIndex = 0;
            // 
            // tpPoint
            // 
            this.tpPoint.Controls.Add(this.gbPointSetting);
            this.tpPoint.Controls.Add(this.cboxEnablePTS);
            this.tpPoint.Location = new System.Drawing.Point(4, 25);
            this.tpPoint.Name = "tpPoint";
            this.tpPoint.Padding = new System.Windows.Forms.Padding(3);
            this.tpPoint.Size = new System.Drawing.Size(497, 381);
            this.tpPoint.TabIndex = 0;
            this.tpPoint.Text = "Point";
            this.tpPoint.UseVisualStyleBackColor = true;
            // 
            // gbPointSetting
            // 
            this.gbPointSetting.Controls.Add(this.gvPoint);
            this.gbPointSetting.Controls.Add(this.lbSubPieceOfPoint);
            this.gbPointSetting.Controls.Add(this.cboxSubPieceOfPoint);
            this.gbPointSetting.Location = new System.Drawing.Point(13, 36);
            this.gbPointSetting.Name = "gbPointSetting";
            this.gbPointSetting.Size = new System.Drawing.Size(472, 340);
            this.gbPointSetting.TabIndex = 1;
            this.gbPointSetting.TabStop = false;
            this.gbPointSetting.Text = "Point setting";
            // 
            // gvPoint
            // 
            this.gvPoint.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvPoint.Location = new System.Drawing.Point(23, 51);
            this.gvPoint.Name = "gvPoint";
            this.gvPoint.RowTemplate.Height = 24;
            this.gvPoint.Size = new System.Drawing.Size(425, 268);
            this.gvPoint.TabIndex = 2;
            this.gvPoint.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gvPoint_DataError);
            // 
            // lbSubPieceOfPoint
            // 
            this.lbSubPieceOfPoint.AutoSize = true;
            this.lbSubPieceOfPoint.Location = new System.Drawing.Point(22, 24);
            this.lbSubPieceOfPoint.Name = "lbSubPieceOfPoint";
            this.lbSubPieceOfPoint.Size = new System.Drawing.Size(47, 12);
            this.lbSubPieceOfPoint.TabIndex = 1;
            this.lbSubPieceOfPoint.Text = "SubPiece";
            // 
            // cboxSubPieceOfPoint
            // 
            this.cboxSubPieceOfPoint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxSubPieceOfPoint.FormattingEnabled = true;
            this.cboxSubPieceOfPoint.Location = new System.Drawing.Point(76, 21);
            this.cboxSubPieceOfPoint.Name = "cboxSubPieceOfPoint";
            this.cboxSubPieceOfPoint.Size = new System.Drawing.Size(121, 20);
            this.cboxSubPieceOfPoint.TabIndex = 0;
            this.cboxSubPieceOfPoint.SelectedIndexChanged += new System.EventHandler(this.cboxSubPieceOfPoint_SelectedIndexChanged);
            // 
            // cboxEnablePTS
            // 
            this.cboxEnablePTS.AutoSize = true;
            this.cboxEnablePTS.Location = new System.Drawing.Point(13, 15);
            this.cboxEnablePTS.Name = "cboxEnablePTS";
            this.cboxEnablePTS.Size = new System.Drawing.Size(87, 16);
            this.cboxEnablePTS.TabIndex = 0;
            this.cboxEnablePTS.Text = "Enable Points";
            this.cboxEnablePTS.UseVisualStyleBackColor = true;
            this.cboxEnablePTS.CheckedChanged += new System.EventHandler(this.cboxEnablePTS_CheckedChanged);
            // 
            // tpGradeLevel
            // 
            this.tpGradeLevel.Controls.Add(this.cboxEnableGrade);
            this.tpGradeLevel.Controls.Add(this.gbGradeSetting);
            this.tpGradeLevel.Location = new System.Drawing.Point(4, 25);
            this.tpGradeLevel.Name = "tpGradeLevel";
            this.tpGradeLevel.Padding = new System.Windows.Forms.Padding(3);
            this.tpGradeLevel.Size = new System.Drawing.Size(497, 381);
            this.tpGradeLevel.TabIndex = 1;
            this.tpGradeLevel.Text = "Grade";
            this.tpGradeLevel.UseVisualStyleBackColor = true;
            // 
            // cboxEnableGrade
            // 
            this.cboxEnableGrade.AutoSize = true;
            this.cboxEnableGrade.Location = new System.Drawing.Point(13, 15);
            this.cboxEnableGrade.Name = "cboxEnableGrade";
            this.cboxEnableGrade.Size = new System.Drawing.Size(87, 16);
            this.cboxEnableGrade.TabIndex = 2;
            this.cboxEnableGrade.Text = "Enable Grade";
            this.cboxEnableGrade.UseVisualStyleBackColor = true;
            this.cboxEnableGrade.CheckedChanged += new System.EventHandler(this.cboxEnableGrade_CheckedChanged);
            // 
            // gbGradeSetting
            // 
            this.gbGradeSetting.Controls.Add(this.gvGrade);
            this.gbGradeSetting.Controls.Add(this.lbSubPieceOfGrade);
            this.gbGradeSetting.Controls.Add(this.cboxSubPieceOfGrade);
            this.gbGradeSetting.Location = new System.Drawing.Point(13, 36);
            this.gbGradeSetting.Name = "gbGradeSetting";
            this.gbGradeSetting.Size = new System.Drawing.Size(472, 340);
            this.gbGradeSetting.TabIndex = 3;
            this.gbGradeSetting.TabStop = false;
            this.gbGradeSetting.Text = "Grade setting";
            // 
            // gvGrade
            // 
            this.gvGrade.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvGrade.Location = new System.Drawing.Point(23, 51);
            this.gvGrade.Name = "gvGrade";
            this.gvGrade.RowTemplate.Height = 24;
            this.gvGrade.Size = new System.Drawing.Size(425, 268);
            this.gvGrade.TabIndex = 2;
            this.gvGrade.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gvGrade_MouseDown);
            this.gvGrade.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gvGrade_CellValidating);
            this.gvGrade.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvGrade_CellEndEdit);
            this.gvGrade.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gvGrade_DataError);
            this.gvGrade.KeyUp += new System.Windows.Forms.KeyEventHandler(this.gvGrade_KeyUp);
            // 
            // lbSubPieceOfGrade
            // 
            this.lbSubPieceOfGrade.AutoSize = true;
            this.lbSubPieceOfGrade.Location = new System.Drawing.Point(22, 24);
            this.lbSubPieceOfGrade.Name = "lbSubPieceOfGrade";
            this.lbSubPieceOfGrade.Size = new System.Drawing.Size(47, 12);
            this.lbSubPieceOfGrade.TabIndex = 1;
            this.lbSubPieceOfGrade.Text = "SubPiece";
            // 
            // cboxSubPieceOfGrade
            // 
            this.cboxSubPieceOfGrade.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxSubPieceOfGrade.FormattingEnabled = true;
            this.cboxSubPieceOfGrade.Location = new System.Drawing.Point(76, 21);
            this.cboxSubPieceOfGrade.Name = "cboxSubPieceOfGrade";
            this.cboxSubPieceOfGrade.Size = new System.Drawing.Size(121, 20);
            this.cboxSubPieceOfGrade.TabIndex = 0;
            this.cboxSubPieceOfGrade.SelectedIndexChanged += new System.EventHandler(this.cboxSubPieceOfGrade_SelectedIndexChanged);
            // 
            // tpPassOrFail
            // 
            this.tpPassOrFail.Controls.Add(this.lbScore);
            this.tpPassOrFail.Controls.Add(this.tboxFilterScore);
            this.tpPassOrFail.Controls.Add(this.cboxEnablePFS);
            this.tpPassOrFail.Location = new System.Drawing.Point(4, 25);
            this.tpPassOrFail.Name = "tpPassOrFail";
            this.tpPassOrFail.Padding = new System.Windows.Forms.Padding(3);
            this.tpPassOrFail.Size = new System.Drawing.Size(497, 381);
            this.tpPassOrFail.TabIndex = 2;
            this.tpPassOrFail.Text = "Pass / Fail";
            this.tpPassOrFail.UseVisualStyleBackColor = true;
            // 
            // lbScore
            // 
            this.lbScore.AutoSize = true;
            this.lbScore.Location = new System.Drawing.Point(13, 46);
            this.lbScore.Name = "lbScore";
            this.lbScore.Size = new System.Drawing.Size(31, 12);
            this.lbScore.TabIndex = 2;
            this.lbScore.Text = "Score";
            // 
            // tboxFilterScore
            // 
            this.tboxFilterScore.Location = new System.Drawing.Point(50, 41);
            this.tboxFilterScore.Name = "tboxFilterScore";
            this.tboxFilterScore.Size = new System.Drawing.Size(100, 22);
            this.tboxFilterScore.TabIndex = 1;
            // 
            // cboxEnablePFS
            // 
            this.cboxEnablePFS.AutoSize = true;
            this.cboxEnablePFS.Location = new System.Drawing.Point(13, 15);
            this.cboxEnablePFS.Name = "cboxEnablePFS";
            this.cboxEnablePFS.Size = new System.Drawing.Size(169, 16);
            this.cboxEnablePFS.TabIndex = 0;
            this.cboxEnablePFS.Text = "Enable Pass Or Fail Filter Score";
            this.cboxEnablePFS.UseVisualStyleBackColor = true;
            this.cboxEnablePFS.CheckedChanged += new System.EventHandler(this.cboxEnablePFS_CheckedChanged);
            // 
            // cboxGradeConfigFile
            // 
            this.cboxGradeConfigFile.FormattingEnabled = true;
            this.cboxGradeConfigFile.Location = new System.Drawing.Point(108, 469);
            this.cboxGradeConfigFile.Name = "cboxGradeConfigFile";
            this.cboxGradeConfigFile.Size = new System.Drawing.Size(234, 20);
            this.cboxGradeConfigFile.TabIndex = 1;
            // 
            // lbGradeConfig
            // 
            this.lbGradeConfig.AutoSize = true;
            this.lbGradeConfig.Location = new System.Drawing.Point(13, 472);
            this.lbGradeConfig.Name = "lbGradeConfig";
            this.lbGradeConfig.Size = new System.Drawing.Size(89, 12);
            this.lbGradeConfig.TabIndex = 2;
            this.lbGradeConfig.Text = "Grade Config File";
            // 
            // btnSaveGradeConfigFile
            // 
            this.btnSaveGradeConfigFile.Location = new System.Drawing.Point(348, 468);
            this.btnSaveGradeConfigFile.Name = "btnSaveGradeConfigFile";
            this.btnSaveGradeConfigFile.Size = new System.Drawing.Size(75, 23);
            this.btnSaveGradeConfigFile.TabIndex = 3;
            this.btnSaveGradeConfigFile.Text = "Save";
            this.btnSaveGradeConfigFile.UseVisualStyleBackColor = true;
            this.btnSaveGradeConfigFile.Click += new System.EventHandler(this.btnSaveGradeConfigFile_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(429, 467);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(23, 51);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(425, 268);
            this.dataGridView1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "SubPiece";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(76, 21);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 20);
            this.comboBox2.TabIndex = 0;
            // 
            // GradeSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(533, 497);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSaveGradeConfigFile);
            this.Controls.Add(this.lbGradeConfig);
            this.Controls.Add(this.cboxGradeConfigFile);
            this.Controls.Add(this.tabGradeSetup);
            this.Name = "GradeSetup";
            this.Text = "GradeSetup";
            this.tabGradeSetup.ResumeLayout(false);
            this.tpROI.ResumeLayout(false);
            this.panelCreateGrid.ResumeLayout(false);
            this.panelCreateGrid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvRows)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvColumns)).EndInit();
            this.gbROIItem.ResumeLayout(false);
            this.gbROIItem.PerformLayout();
            this.tpGradeGroup.ResumeLayout(false);
            this.tabGradeSetting.ResumeLayout(false);
            this.tpPoint.ResumeLayout(false);
            this.tpPoint.PerformLayout();
            this.gbPointSetting.ResumeLayout(false);
            this.gbPointSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvPoint)).EndInit();
            this.tpGradeLevel.ResumeLayout(false);
            this.tpGradeLevel.PerformLayout();
            this.gbGradeSetting.ResumeLayout(false);
            this.gbGradeSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvGrade)).EndInit();
            this.tpPassOrFail.ResumeLayout(false);
            this.tpPassOrFail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsGradConfigList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsColumns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsRows)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsRoiList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsPointSubPiece)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsMarkSubPiece)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabGradeSetup;
        private System.Windows.Forms.TabPage tpROI;
        private System.Windows.Forms.TabPage tpGradeGroup;
        private System.Windows.Forms.ComboBox cboxGradeConfigFile;
        private System.Windows.Forms.Label lbGradeConfig;
        private System.Windows.Forms.Button btnSaveGradeConfigFile;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox gbROIItem;
        private System.Windows.Forms.RadioButton rbSymmetrical;
        private System.Windows.Forms.RadioButton rbNoRoi;
        private System.Windows.Forms.Panel panelCreateGrid;
        private System.Windows.Forms.Label lbRows;
        private System.Windows.Forms.Label lbColumns;
        private System.Windows.Forms.TextBox tboxColumns;
        private System.Windows.Forms.TextBox tboxRows;
        private System.Windows.Forms.Label lbX;
        private System.Windows.Forms.DataGridView gvColumns;
        private System.Windows.Forms.Button btnCreateGrid;
        private System.Windows.Forms.DataGridView gvRows;
        private System.Windows.Forms.TabControl tabGradeSetting;
        private System.Windows.Forms.TabPage tpPoint;
        private System.Windows.Forms.TabPage tpGradeLevel;
        private System.Windows.Forms.TabPage tpPassOrFail;
        private System.Windows.Forms.GroupBox gbPointSetting;
        private System.Windows.Forms.Label lbSubPieceOfPoint;
        private System.Windows.Forms.ComboBox cboxSubPieceOfPoint;
        private System.Windows.Forms.CheckBox cboxEnablePTS;
        private System.Windows.Forms.DataGridView gvPoint;
        private System.Windows.Forms.CheckBox cboxEnableGrade;
        private System.Windows.Forms.GroupBox gbGradeSetting;
        private System.Windows.Forms.DataGridView gvGrade;
        private System.Windows.Forms.Label lbSubPieceOfGrade;
        private System.Windows.Forms.ComboBox cboxSubPieceOfGrade;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.CheckBox cboxEnablePFS;
        private System.Windows.Forms.Label lbScore;
        private System.Windows.Forms.TextBox tboxFilterScore;
        private System.Windows.Forms.BindingSource bsGradConfigList;
        private System.Windows.Forms.BindingSource bsColumns;
        private System.Windows.Forms.BindingSource bsRows;
        private System.Windows.Forms.BindingSource bsRoiList;
        private System.Windows.Forms.BindingSource bsPointSubPiece;
        private System.Windows.Forms.BindingSource bsMarkSubPiece;
        private System.Windows.Forms.Label lbShowGvRow;
        private System.Windows.Forms.Label lbShowGvColumn;
    }
}