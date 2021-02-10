namespace workschedule.ReportsForm
{
    partial class ReportWorkScheduleMensMenu
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
            this.cmbTargetYear = new System.Windows.Forms.ComboBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblTargetDate = new System.Windows.Forms.Label();
            this.lblTargetYear = new System.Windows.Forms.Label();
            this.cmbTargetMonth = new System.Windows.Forms.ComboBox();
            this.lblTargetMonth = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbTargetYear
            // 
            this.cmbTargetYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTargetYear.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbTargetYear.FormattingEnabled = true;
            this.cmbTargetYear.Items.AddRange(new object[] {
            "",
            "委員会のため",
            "外出のため",
            "研修のため",
            "その他(※手入力してください)"});
            this.cmbTargetYear.Location = new System.Drawing.Point(99, 22);
            this.cmbTargetYear.Name = "cmbTargetYear";
            this.cmbTargetYear.Size = new System.Drawing.Size(93, 27);
            this.cmbTargetYear.TabIndex = 141;
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(150)))), ((int)(((byte)(146)))));
            this.btnPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPrint.Location = new System.Drawing.Point(236, 68);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(85, 50);
            this.btnPrint.TabIndex = 140;
            this.btnPrint.Text = "印刷";
            this.btnPrint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(117)))), ((int)(((byte)(117)))));
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(28, 68);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(85, 50);
            this.btnCancel.TabIndex = 139;
            this.btnCancel.Text = "キャンセル";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblTargetDate
            // 
            this.lblTargetDate.AutoSize = true;
            this.lblTargetDate.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblTargetDate.Location = new System.Drawing.Point(24, 25);
            this.lblTargetDate.Name = "lblTargetDate";
            this.lblTargetDate.Size = new System.Drawing.Size(69, 19);
            this.lblTargetDate.TabIndex = 136;
            this.lblTargetDate.Text = "勤務内容";
            // 
            // lblTargetYear
            // 
            this.lblTargetYear.AutoSize = true;
            this.lblTargetYear.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblTargetYear.Location = new System.Drawing.Point(198, 25);
            this.lblTargetYear.Name = "lblTargetYear";
            this.lblTargetYear.Size = new System.Drawing.Size(24, 19);
            this.lblTargetYear.TabIndex = 142;
            this.lblTargetYear.Text = "年";
            // 
            // cmbTargetMonth
            // 
            this.cmbTargetMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTargetMonth.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbTargetMonth.FormattingEnabled = true;
            this.cmbTargetMonth.Items.AddRange(new object[] {
            "",
            "委員会のため",
            "外出のため",
            "研修のため",
            "その他(※手入力してください)"});
            this.cmbTargetMonth.Location = new System.Drawing.Point(228, 22);
            this.cmbTargetMonth.Name = "cmbTargetMonth";
            this.cmbTargetMonth.Size = new System.Drawing.Size(63, 27);
            this.cmbTargetMonth.TabIndex = 143;
            // 
            // lblTargetMonth
            // 
            this.lblTargetMonth.AutoSize = true;
            this.lblTargetMonth.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblTargetMonth.Location = new System.Drawing.Point(297, 25);
            this.lblTargetMonth.Name = "lblTargetMonth";
            this.lblTargetMonth.Size = new System.Drawing.Size(24, 19);
            this.lblTargetMonth.TabIndex = 144;
            this.lblTargetMonth.Text = "月";
            // 
            // ReportWorkScheduleMensMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(347, 133);
            this.Controls.Add(this.lblTargetMonth);
            this.Controls.Add(this.cmbTargetMonth);
            this.Controls.Add(this.lblTargetYear);
            this.Controls.Add(this.cmbTargetYear);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblTargetDate);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReportWorkScheduleMensMenu";
            this.Text = "勤務計画表(男性のみ)";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbTargetYear;
        public System.Windows.Forms.Button btnPrint;
        public System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblTargetDate;
        private System.Windows.Forms.Label lblTargetYear;
        private System.Windows.Forms.ComboBox cmbTargetMonth;
        private System.Windows.Forms.Label lblTargetMonth;
    }
}