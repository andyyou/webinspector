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
            this.lbMeterialTypeValue = new System.Windows.Forms.Label();
            this.lbDoffValue = new System.Windows.Forms.Label();
            this.lbJobIDValue = new System.Windows.Forms.Label();
            this.lbOrderNumber = new System.Windows.Forms.Label();
            this.lbJobID = new System.Windows.Forms.Label();
            this.lbDoff = new System.Windows.Forms.Label();
            this.lbMeterialType = new System.Windows.Forms.Label();
            this.lbOperator = new System.Windows.Forms.Label();
            this.lbPass = new System.Windows.Forms.Label();
            this.lbFail = new System.Windows.Forms.Label();
            this.lbYield = new System.Windows.Forms.Label();
            this.lbPassValue = new System.Windows.Forms.Label();
            this.lbFailValue = new System.Windows.Forms.Label();
            this.lbYieldValue = new System.Windows.Forms.Label();
            this.lbOrderNumberValue = new System.Windows.Forms.Label();
            this.lbOperatorValue = new System.Windows.Forms.Label();
            this.lbDateTime = new System.Windows.Forms.Label();
            this.lbDateTimeValue = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.rbFail = new System.Windows.Forms.RadioButton();
            this.rbPass = new System.Windows.Forms.RadioButton();
            this.btnMapSetup = new System.Windows.Forms.Button();
            this.lbPageCurrent = new System.Windows.Forms.Label();
            this.lbPageTotal = new System.Windows.Forms.Label();
            this.lbNothing = new System.Windows.Forms.Label();
            this.btnNextPiece = new System.Windows.Forms.Button();
            this.btnPrevPiece = new System.Windows.Forms.Button();
            this.btnGradeSetting = new System.Windows.Forms.Button();
            this.cboxGradeConfigFile = new System.Windows.Forms.ComboBox();
            this.lbGradeConfig = new System.Windows.Forms.Label();
            this.lbTotalScore = new System.Windows.Forms.Label();
            this.lbTotalScoreValue = new System.Windows.Forms.Label();
            this.bsGradConfigList = new System.Windows.Forms.BindingSource(this.components);
            this.btnFailList = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gvFlawClass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFlawType)).BeginInit();
            this.tlpMapInfo.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsGradConfigList)).BeginInit();
            this.SuspendLayout();
            // 
            // nChart
            // 
            this.nChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.nChart.AutoRefresh = false;
            this.nChart.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.nChart.InputKeys = new System.Windows.Forms.Keys[0];
            this.nChart.Location = new System.Drawing.Point(14, 199);
            this.nChart.Name = "nChart";
            this.nChart.Size = new System.Drawing.Size(622, 403);
            this.nChart.State = ((Nevron.Chart.WinForm.NState)(resources.GetObject("nChart.State")));
            this.nChart.TabIndex = 0;
            this.nChart.MouseClick += new System.Windows.Forms.MouseEventHandler(this.nChart_MouseClick);
            // 
            // gvFlawClass
            // 
            this.gvFlawClass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gvFlawClass.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvFlawClass.Location = new System.Drawing.Point(14, 639);
            this.gvFlawClass.Name = "gvFlawClass";
            this.gvFlawClass.RowHeadersVisible = false;
            this.gvFlawClass.RowTemplate.Height = 24;
            this.gvFlawClass.Size = new System.Drawing.Size(622, 194);
            this.gvFlawClass.TabIndex = 1;
            this.gvFlawClass.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gvFlawClass_CellFormatting);
            this.gvFlawClass.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvFlawClass_CellContentClick);
            // 
            // tlpMapInfo
            // 
            this.tlpMapInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpMapInfo.AutoSize = true;
            this.tlpMapInfo.BackColor = System.Drawing.Color.Transparent;
            this.tlpMapInfo.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tlpMapInfo.ColumnCount = 4;
            this.tlpMapInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpMapInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpMapInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpMapInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpMapInfo.Controls.Add(this.lbMeterialTypeValue, 1, 3);
            this.tlpMapInfo.Controls.Add(this.lbDoffValue, 1, 2);
            this.tlpMapInfo.Controls.Add(this.lbJobIDValue, 1, 1);
            this.tlpMapInfo.Controls.Add(this.lbOrderNumber, 0, 0);
            this.tlpMapInfo.Controls.Add(this.lbJobID, 0, 1);
            this.tlpMapInfo.Controls.Add(this.lbDoff, 0, 2);
            this.tlpMapInfo.Controls.Add(this.lbMeterialType, 0, 3);
            this.tlpMapInfo.Controls.Add(this.lbOperator, 0, 4);
            this.tlpMapInfo.Controls.Add(this.lbPass, 2, 0);
            this.tlpMapInfo.Controls.Add(this.lbFail, 2, 1);
            this.tlpMapInfo.Controls.Add(this.lbYield, 2, 2);
            this.tlpMapInfo.Controls.Add(this.lbPassValue, 3, 0);
            this.tlpMapInfo.Controls.Add(this.lbFailValue, 3, 1);
            this.tlpMapInfo.Controls.Add(this.lbYieldValue, 3, 2);
            this.tlpMapInfo.Controls.Add(this.lbOrderNumberValue, 1, 0);
            this.tlpMapInfo.Controls.Add(this.lbOperatorValue, 1, 4);
            this.tlpMapInfo.Controls.Add(this.lbDateTime, 2, 3);
            this.tlpMapInfo.Controls.Add(this.lbDateTimeValue, 3, 3);
            this.tlpMapInfo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tlpMapInfo.Location = new System.Drawing.Point(14, 17);
            this.tlpMapInfo.Margin = new System.Windows.Forms.Padding(5);
            this.tlpMapInfo.Name = "tlpMapInfo";
            this.tlpMapInfo.RowCount = 5;
            this.tlpMapInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpMapInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpMapInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpMapInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpMapInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpMapInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMapInfo.Size = new System.Drawing.Size(500, 131);
            this.tlpMapInfo.TabIndex = 2;
            // 
            // lbMeterialTypeValue
            // 
            this.lbMeterialTypeValue.AutoSize = true;
            this.lbMeterialTypeValue.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbMeterialTypeValue.Location = new System.Drawing.Point(128, 79);
            this.lbMeterialTypeValue.Name = "lbMeterialTypeValue";
            this.lbMeterialTypeValue.Size = new System.Drawing.Size(18, 16);
            this.lbMeterialTypeValue.TabIndex = 16;
            this.lbMeterialTypeValue.Text = "--";
            // 
            // lbDoffValue
            // 
            this.lbDoffValue.AutoSize = true;
            this.lbDoffValue.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbDoffValue.Location = new System.Drawing.Point(128, 53);
            this.lbDoffValue.Name = "lbDoffValue";
            this.lbDoffValue.Size = new System.Drawing.Size(18, 16);
            this.lbDoffValue.TabIndex = 15;
            this.lbDoffValue.Text = "--";
            // 
            // lbJobIDValue
            // 
            this.lbJobIDValue.AutoSize = true;
            this.lbJobIDValue.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbJobIDValue.Location = new System.Drawing.Point(128, 27);
            this.lbJobIDValue.Name = "lbJobIDValue";
            this.lbJobIDValue.Size = new System.Drawing.Size(18, 16);
            this.lbJobIDValue.TabIndex = 14;
            this.lbJobIDValue.Text = "--";
            // 
            // lbOrderNumber
            // 
            this.lbOrderNumber.AutoEllipsis = true;
            this.lbOrderNumber.AutoSize = true;
            this.lbOrderNumber.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbOrderNumber.ForeColor = System.Drawing.Color.Black;
            this.lbOrderNumber.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lbOrderNumber.Location = new System.Drawing.Point(4, 1);
            this.lbOrderNumber.Name = "lbOrderNumber";
            this.lbOrderNumber.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbOrderNumber.Size = new System.Drawing.Size(91, 16);
            this.lbOrderNumber.TabIndex = 0;
            this.lbOrderNumber.Text = "Order Number";
            // 
            // lbJobID
            // 
            this.lbJobID.AutoSize = true;
            this.lbJobID.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbJobID.Location = new System.Drawing.Point(4, 27);
            this.lbJobID.Name = "lbJobID";
            this.lbJobID.Size = new System.Drawing.Size(44, 16);
            this.lbJobID.TabIndex = 1;
            this.lbJobID.Text = "Job ID";
            // 
            // lbDoff
            // 
            this.lbDoff.AutoSize = true;
            this.lbDoff.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDoff.Location = new System.Drawing.Point(4, 53);
            this.lbDoff.Name = "lbDoff";
            this.lbDoff.Size = new System.Drawing.Size(33, 16);
            this.lbDoff.TabIndex = 2;
            this.lbDoff.Text = "Doff";
            // 
            // lbMeterialType
            // 
            this.lbMeterialType.AutoSize = true;
            this.lbMeterialType.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMeterialType.Location = new System.Drawing.Point(4, 79);
            this.lbMeterialType.Name = "lbMeterialType";
            this.lbMeterialType.Size = new System.Drawing.Size(86, 16);
            this.lbMeterialType.TabIndex = 3;
            this.lbMeterialType.Text = "Meterial Type";
            // 
            // lbOperator
            // 
            this.lbOperator.AutoSize = true;
            this.lbOperator.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.lbOperator.Location = new System.Drawing.Point(4, 105);
            this.lbOperator.Name = "lbOperator";
            this.lbOperator.Size = new System.Drawing.Size(60, 16);
            this.lbOperator.TabIndex = 4;
            this.lbOperator.Text = "Operator";
            // 
            // lbPass
            // 
            this.lbPass.AutoSize = true;
            this.lbPass.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.lbPass.Location = new System.Drawing.Point(252, 1);
            this.lbPass.Name = "lbPass";
            this.lbPass.Size = new System.Drawing.Size(32, 16);
            this.lbPass.TabIndex = 6;
            this.lbPass.Text = "Pass";
            // 
            // lbFail
            // 
            this.lbFail.AutoSize = true;
            this.lbFail.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.lbFail.Location = new System.Drawing.Point(252, 27);
            this.lbFail.Name = "lbFail";
            this.lbFail.Size = new System.Drawing.Size(27, 16);
            this.lbFail.TabIndex = 7;
            this.lbFail.Text = "Fail";
            // 
            // lbYield
            // 
            this.lbYield.AutoSize = true;
            this.lbYield.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.lbYield.Location = new System.Drawing.Point(252, 53);
            this.lbYield.Name = "lbYield";
            this.lbYield.Size = new System.Drawing.Size(36, 16);
            this.lbYield.TabIndex = 8;
            this.lbYield.Text = "Yield";
            // 
            // lbPassValue
            // 
            this.lbPassValue.AutoSize = true;
            this.lbPassValue.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.lbPassValue.Location = new System.Drawing.Point(376, 1);
            this.lbPassValue.Name = "lbPassValue";
            this.lbPassValue.Size = new System.Drawing.Size(15, 16);
            this.lbPassValue.TabIndex = 10;
            this.lbPassValue.Text = "0";
            // 
            // lbFailValue
            // 
            this.lbFailValue.AutoSize = true;
            this.lbFailValue.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.lbFailValue.Location = new System.Drawing.Point(376, 27);
            this.lbFailValue.Name = "lbFailValue";
            this.lbFailValue.Size = new System.Drawing.Size(15, 16);
            this.lbFailValue.TabIndex = 11;
            this.lbFailValue.Text = "0";
            // 
            // lbYieldValue
            // 
            this.lbYieldValue.AutoSize = true;
            this.lbYieldValue.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.lbYieldValue.Location = new System.Drawing.Point(376, 53);
            this.lbYieldValue.Name = "lbYieldValue";
            this.lbYieldValue.Size = new System.Drawing.Size(15, 16);
            this.lbYieldValue.TabIndex = 12;
            this.lbYieldValue.Text = "0";
            // 
            // lbOrderNumberValue
            // 
            this.lbOrderNumberValue.AutoSize = true;
            this.lbOrderNumberValue.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbOrderNumberValue.Location = new System.Drawing.Point(128, 1);
            this.lbOrderNumberValue.Name = "lbOrderNumberValue";
            this.lbOrderNumberValue.Size = new System.Drawing.Size(18, 16);
            this.lbOrderNumberValue.TabIndex = 13;
            this.lbOrderNumberValue.Text = "--";
            // 
            // lbOperatorValue
            // 
            this.lbOperatorValue.AutoSize = true;
            this.lbOperatorValue.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbOperatorValue.Location = new System.Drawing.Point(128, 105);
            this.lbOperatorValue.Name = "lbOperatorValue";
            this.lbOperatorValue.Size = new System.Drawing.Size(18, 16);
            this.lbOperatorValue.TabIndex = 17;
            this.lbOperatorValue.Text = "--";
            // 
            // lbDateTime
            // 
            this.lbDateTime.AutoSize = true;
            this.lbDateTime.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.lbDateTime.Location = new System.Drawing.Point(252, 79);
            this.lbDateTime.Name = "lbDateTime";
            this.lbDateTime.Size = new System.Drawing.Size(63, 16);
            this.lbDateTime.TabIndex = 5;
            this.lbDateTime.Text = "DateTime";
            // 
            // lbDateTimeValue
            // 
            this.lbDateTimeValue.AutoSize = true;
            this.lbDateTimeValue.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbDateTimeValue.Location = new System.Drawing.Point(376, 79);
            this.lbDateTimeValue.Name = "lbDateTimeValue";
            this.lbDateTimeValue.Size = new System.Drawing.Size(18, 16);
            this.lbDateTimeValue.TabIndex = 18;
            this.lbDateTimeValue.Text = "--";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.rbAll);
            this.panel1.Controls.Add(this.rbFail);
            this.panel1.Controls.Add(this.rbPass);
            this.panel1.Location = new System.Drawing.Point(522, 17);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(114, 68);
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
            this.rbAll.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
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
            this.rbFail.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
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
            this.rbPass.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // btnMapSetup
            // 
            this.btnMapSetup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMapSetup.Location = new System.Drawing.Point(523, 96);
            this.btnMapSetup.Name = "btnMapSetup";
            this.btnMapSetup.Size = new System.Drawing.Size(113, 23);
            this.btnMapSetup.TabIndex = 4;
            this.btnMapSetup.Text = "Map Setting";
            this.btnMapSetup.UseVisualStyleBackColor = true;
            this.btnMapSetup.Click += new System.EventHandler(this.btnMapSetup_Click);
            // 
            // lbPageCurrent
            // 
            this.lbPageCurrent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPageCurrent.BackColor = System.Drawing.Color.Transparent;
            this.lbPageCurrent.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPageCurrent.Location = new System.Drawing.Point(233, 610);
            this.lbPageCurrent.Name = "lbPageCurrent";
            this.lbPageCurrent.Size = new System.Drawing.Size(82, 23);
            this.lbPageCurrent.TabIndex = 12;
            this.lbPageCurrent.Text = "--";
            this.lbPageCurrent.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbPageTotal
            // 
            this.lbPageTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPageTotal.BackColor = System.Drawing.Color.Transparent;
            this.lbPageTotal.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPageTotal.Location = new System.Drawing.Point(335, 610);
            this.lbPageTotal.Name = "lbPageTotal";
            this.lbPageTotal.Size = new System.Drawing.Size(82, 23);
            this.lbPageTotal.TabIndex = 11;
            this.lbPageTotal.Text = "--";
            this.lbPageTotal.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbNothing
            // 
            this.lbNothing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbNothing.AutoSize = true;
            this.lbNothing.BackColor = System.Drawing.Color.Transparent;
            this.lbNothing.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNothing.Location = new System.Drawing.Point(318, 611);
            this.lbNothing.Name = "lbNothing";
            this.lbNothing.Size = new System.Drawing.Size(14, 20);
            this.lbNothing.TabIndex = 10;
            this.lbNothing.Text = "/";
            // 
            // btnNextPiece
            // 
            this.btnNextPiece.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNextPiece.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnNextPiece.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnNextPiece.Location = new System.Drawing.Point(444, 606);
            this.btnNextPiece.Name = "btnNextPiece";
            this.btnNextPiece.Size = new System.Drawing.Size(30, 30);
            this.btnNextPiece.TabIndex = 9;
            this.btnNextPiece.Text = ">";
            this.btnNextPiece.UseVisualStyleBackColor = false;
            this.btnNextPiece.Click += new System.EventHandler(this.btnNextPiece_Click);
            // 
            // btnPrevPiece
            // 
            this.btnPrevPiece.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrevPiece.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnPrevPiece.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnPrevPiece.Location = new System.Drawing.Point(177, 606);
            this.btnPrevPiece.Name = "btnPrevPiece";
            this.btnPrevPiece.Size = new System.Drawing.Size(30, 30);
            this.btnPrevPiece.TabIndex = 8;
            this.btnPrevPiece.Text = "<";
            this.btnPrevPiece.UseVisualStyleBackColor = false;
            this.btnPrevPiece.Click += new System.EventHandler(this.btnPrevPiece_Click);
            // 
            // btnGradeSetting
            // 
            this.btnGradeSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGradeSetting.Location = new System.Drawing.Point(523, 125);
            this.btnGradeSetting.Name = "btnGradeSetting";
            this.btnGradeSetting.Size = new System.Drawing.Size(113, 23);
            this.btnGradeSetting.TabIndex = 13;
            this.btnGradeSetting.Text = "Grade Setting";
            this.btnGradeSetting.UseVisualStyleBackColor = true;
            this.btnGradeSetting.Click += new System.EventHandler(this.btnGradeSetting_Click);
            // 
            // cboxGradeConfigFile
            // 
            this.cboxGradeConfigFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboxGradeConfigFile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxGradeConfigFile.FormattingEnabled = true;
            this.cboxGradeConfigFile.Location = new System.Drawing.Point(132, 157);
            this.cboxGradeConfigFile.Name = "cboxGradeConfigFile";
            this.cboxGradeConfigFile.Size = new System.Drawing.Size(382, 20);
            this.cboxGradeConfigFile.TabIndex = 14;
            this.cboxGradeConfigFile.SelectedIndexChanged += new System.EventHandler(this.cboxGradeConfigFile_SelectedIndexChanged);
            // 
            // lbGradeConfig
            // 
            this.lbGradeConfig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbGradeConfig.AutoSize = true;
            this.lbGradeConfig.BackColor = System.Drawing.Color.Transparent;
            this.lbGradeConfig.Location = new System.Drawing.Point(37, 161);
            this.lbGradeConfig.Name = "lbGradeConfig";
            this.lbGradeConfig.Size = new System.Drawing.Size(89, 12);
            this.lbGradeConfig.TabIndex = 15;
            this.lbGradeConfig.Text = "Grade Config File";
            // 
            // lbTotalScore
            // 
            this.lbTotalScore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbTotalScore.AutoSize = true;
            this.lbTotalScore.BackColor = System.Drawing.Color.Transparent;
            this.lbTotalScore.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTotalScore.Location = new System.Drawing.Point(481, 616);
            this.lbTotalScore.Name = "lbTotalScore";
            this.lbTotalScore.Size = new System.Drawing.Size(73, 14);
            this.lbTotalScore.TabIndex = 16;
            this.lbTotalScore.Text = "TotalScore";
            // 
            // lbTotalScoreValue
            // 
            this.lbTotalScoreValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbTotalScoreValue.AutoSize = true;
            this.lbTotalScoreValue.BackColor = System.Drawing.Color.Transparent;
            this.lbTotalScoreValue.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTotalScoreValue.Location = new System.Drawing.Point(560, 615);
            this.lbTotalScoreValue.Name = "lbTotalScoreValue";
            this.lbTotalScoreValue.Size = new System.Drawing.Size(17, 14);
            this.lbTotalScoreValue.TabIndex = 17;
            this.lbTotalScoreValue.Text = "--";
            // 
            // btnFailList
            // 
            this.btnFailList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFailList.Location = new System.Drawing.Point(523, 154);
            this.btnFailList.Name = "btnFailList";
            this.btnFailList.Size = new System.Drawing.Size(113, 23);
            this.btnFailList.TabIndex = 13;
            this.btnFailList.Text = "Fail List";
            this.btnFailList.UseVisualStyleBackColor = true;
            this.btnFailList.Click += new System.EventHandler(this.btnFailList_Click);
            // 
            // MapWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.BackgroundImage = global::PxP.Properties.Resources.bgLeft;
            this.Controls.Add(this.lbTotalScoreValue);
            this.Controls.Add(this.lbTotalScore);
            this.Controls.Add(this.lbGradeConfig);
            this.Controls.Add(this.cboxGradeConfigFile);
            this.Controls.Add(this.btnFailList);
            this.Controls.Add(this.btnGradeSetting);
            this.Controls.Add(this.lbPageCurrent);
            this.Controls.Add(this.lbPageTotal);
            this.Controls.Add(this.lbNothing);
            this.Controls.Add(this.btnNextPiece);
            this.Controls.Add(this.btnPrevPiece);
            this.Controls.Add(this.btnMapSetup);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tlpMapInfo);
            this.Controls.Add(this.gvFlawClass);
            this.Controls.Add(this.nChart);
            this.Name = "MapWindow";
            this.Size = new System.Drawing.Size(650, 848);
            this.Load += new System.EventHandler(this.MapWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvFlawClass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFlawType)).EndInit();
            this.tlpMapInfo.ResumeLayout(false);
            this.tlpMapInfo.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsGradConfigList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Nevron.Chart.WinForm.NChartControl nChart;
        public System.Windows.Forms.DataGridView gvFlawClass;
        public System.Windows.Forms.BindingSource bsFlawType;
        private System.Windows.Forms.TableLayoutPanel tlpMapInfo;
        private System.Windows.Forms.Label lbOrderNumber;
        private System.Windows.Forms.Label lbJobID;
        private System.Windows.Forms.Label lbDoff;
        private System.Windows.Forms.Label lbMeterialType;
        private System.Windows.Forms.Label lbOperator;
        private System.Windows.Forms.Label lbDateTime;
        private System.Windows.Forms.Label lbPass;
        private System.Windows.Forms.Label lbFail;
        private System.Windows.Forms.Label lbYield;
        private System.Windows.Forms.Label lbPassValue;
        private System.Windows.Forms.Label lbFailValue;
        private System.Windows.Forms.Label lbYieldValue;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.RadioButton rbFail;
        private System.Windows.Forms.RadioButton rbPass;
        public System.Windows.Forms.Button btnMapSetup;
        private System.Windows.Forms.Label lbOrderNumberValue;
        private System.Windows.Forms.Label lbMeterialTypeValue;
        private System.Windows.Forms.Label lbDoffValue;
        private System.Windows.Forms.Label lbJobIDValue;
        private System.Windows.Forms.Label lbOperatorValue;
        private System.Windows.Forms.Label lbDateTimeValue;
        private System.Windows.Forms.Label lbPageTotal;
        private System.Windows.Forms.Label lbNothing;
        private System.Windows.Forms.ComboBox cboxGradeConfigFile;
        private System.Windows.Forms.Label lbGradeConfig;
        private System.Windows.Forms.Label lbTotalScore;
        private System.Windows.Forms.Label lbTotalScoreValue;
        private System.Windows.Forms.BindingSource bsGradConfigList;
        private System.Windows.Forms.Button btnFailList;
        public System.Windows.Forms.Button btnGradeSetting;
        public System.Windows.Forms.Label lbPageCurrent;
        public System.Windows.Forms.Button btnNextPiece;
        public System.Windows.Forms.Button btnPrevPiece;

    }
}
