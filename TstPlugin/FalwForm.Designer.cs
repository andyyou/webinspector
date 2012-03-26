namespace PxP
{
    partial class FalwForm
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
            this.tcPicture = new System.Windows.Forms.TabControl();
            this.tlbFlawInfo = new System.Windows.Forms.TableLayoutPanel();
            this.lbFlawID = new System.Windows.Forms.Label();
            this.lbFlawClass = new System.Windows.Forms.Label();
            this.lbMD = new System.Windows.Forms.Label();
            this.lbWidth = new System.Windows.Forms.Label();
            this.lbFlawType = new System.Windows.Forms.Label();
            this.lbArea = new System.Windows.Forms.Label();
            this.lbCD = new System.Windows.Forms.Label();
            this.lbLength = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tlbFlawInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcPicture
            // 
            this.tcPicture.Location = new System.Drawing.Point(12, 119);
            this.tcPicture.Name = "tcPicture";
            this.tcPicture.SelectedIndex = 0;
            this.tcPicture.Size = new System.Drawing.Size(460, 227);
            this.tcPicture.TabIndex = 0;
            // 
            // tlbFlawInfo
            // 
            this.tlbFlawInfo.BackColor = System.Drawing.Color.Transparent;
            this.tlbFlawInfo.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tlbFlawInfo.ColumnCount = 4;
            this.tlbFlawInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlbFlawInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlbFlawInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlbFlawInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlbFlawInfo.Controls.Add(this.lbFlawID, 0, 0);
            this.tlbFlawInfo.Controls.Add(this.lbFlawClass, 0, 1);
            this.tlbFlawInfo.Controls.Add(this.lbMD, 0, 2);
            this.tlbFlawInfo.Controls.Add(this.lbWidth, 0, 3);
            this.tlbFlawInfo.Controls.Add(this.lbFlawType, 2, 0);
            this.tlbFlawInfo.Controls.Add(this.lbArea, 2, 1);
            this.tlbFlawInfo.Controls.Add(this.lbCD, 2, 2);
            this.tlbFlawInfo.Controls.Add(this.lbLength, 2, 3);
            this.tlbFlawInfo.Font = new System.Drawing.Font("Ubuntu Mono", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tlbFlawInfo.Location = new System.Drawing.Point(13, 13);
            this.tlbFlawInfo.Name = "tlbFlawInfo";
            this.tlbFlawInfo.RowCount = 4;
            this.tlbFlawInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlbFlawInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlbFlawInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlbFlawInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlbFlawInfo.Size = new System.Drawing.Size(392, 100);
            this.tlbFlawInfo.TabIndex = 1;
            // 
            // lbFlawID
            // 
            this.lbFlawID.AutoSize = true;
            this.lbFlawID.Font = new System.Drawing.Font("Ubuntu Mono", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFlawID.Location = new System.Drawing.Point(4, 1);
            this.lbFlawID.Name = "lbFlawID";
            this.lbFlawID.Size = new System.Drawing.Size(57, 16);
            this.lbFlawID.TabIndex = 0;
            this.lbFlawID.Text = "Flaw ID";
            // 
            // lbFlawClass
            // 
            this.lbFlawClass.AutoSize = true;
            this.lbFlawClass.Font = new System.Drawing.Font("Ubuntu Mono", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFlawClass.Location = new System.Drawing.Point(4, 25);
            this.lbFlawClass.Name = "lbFlawClass";
            this.lbFlawClass.Size = new System.Drawing.Size(78, 16);
            this.lbFlawClass.TabIndex = 1;
            this.lbFlawClass.Text = "Flaw Class";
            // 
            // lbMD
            // 
            this.lbMD.AutoSize = true;
            this.lbMD.Font = new System.Drawing.Font("Ubuntu Mono", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMD.Location = new System.Drawing.Point(4, 49);
            this.lbMD.Name = "lbMD";
            this.lbMD.Size = new System.Drawing.Size(22, 16);
            this.lbMD.TabIndex = 2;
            this.lbMD.Text = "MD";
            // 
            // lbWidth
            // 
            this.lbWidth.AutoSize = true;
            this.lbWidth.Font = new System.Drawing.Font("Ubuntu Mono", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbWidth.Location = new System.Drawing.Point(4, 73);
            this.lbWidth.Name = "lbWidth";
            this.lbWidth.Size = new System.Drawing.Size(43, 16);
            this.lbWidth.TabIndex = 3;
            this.lbWidth.Text = "Width";
            // 
            // lbFlawType
            // 
            this.lbFlawType.AutoSize = true;
            this.lbFlawType.Font = new System.Drawing.Font("Ubuntu Mono", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFlawType.Location = new System.Drawing.Point(198, 1);
            this.lbFlawType.Name = "lbFlawType";
            this.lbFlawType.Size = new System.Drawing.Size(71, 16);
            this.lbFlawType.TabIndex = 4;
            this.lbFlawType.Text = "Flaw Type";
            // 
            // lbArea
            // 
            this.lbArea.AutoSize = true;
            this.lbArea.Font = new System.Drawing.Font("Ubuntu", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbArea.Location = new System.Drawing.Point(198, 25);
            this.lbArea.Name = "lbArea";
            this.lbArea.Size = new System.Drawing.Size(37, 17);
            this.lbArea.TabIndex = 5;
            this.lbArea.Text = "Area";
            // 
            // lbCD
            // 
            this.lbCD.AutoSize = true;
            this.lbCD.Font = new System.Drawing.Font("Ubuntu Mono", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCD.Location = new System.Drawing.Point(198, 49);
            this.lbCD.Name = "lbCD";
            this.lbCD.Size = new System.Drawing.Size(22, 16);
            this.lbCD.TabIndex = 6;
            this.lbCD.Text = "CD";
            // 
            // lbLength
            // 
            this.lbLength.AutoSize = true;
            this.lbLength.Font = new System.Drawing.Font("Ubuntu Mono", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLength.Location = new System.Drawing.Point(198, 73);
            this.lbLength.Name = "lbLength";
            this.lbLength.Size = new System.Drawing.Size(50, 16);
            this.lbLength.TabIndex = 7;
            this.lbLength.Text = "Length";
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::PxP.Properties.Resources.Zoom_In;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button1.Location = new System.Drawing.Point(432, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(40, 40);
            this.button1.TabIndex = 2;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.BackgroundImage = global::PxP.Properties.Resources.Zoom_Out;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button2.Location = new System.Drawing.Point(432, 58);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(40, 40);
            this.button2.TabIndex = 3;
            this.button2.UseVisualStyleBackColor = true;
            // 
            // FalwForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::PxP.Properties.Resources.bgRight;
            this.ClientSize = new System.Drawing.Size(484, 358);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tlbFlawInfo);
            this.Controls.Add(this.tcPicture);
            this.Name = "FalwForm";
            this.Text = "FalwForm";
            this.tlbFlawInfo.ResumeLayout(false);
            this.tlbFlawInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcPicture;
        private System.Windows.Forms.TableLayoutPanel tlbFlawInfo;
        private System.Windows.Forms.Label lbFlawID;
        private System.Windows.Forms.Label lbFlawClass;
        private System.Windows.Forms.Label lbMD;
        private System.Windows.Forms.Label lbWidth;
        private System.Windows.Forms.Label lbFlawType;
        private System.Windows.Forms.Label lbArea;
        private System.Windows.Forms.Label lbCD;
        private System.Windows.Forms.Label lbLength;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}