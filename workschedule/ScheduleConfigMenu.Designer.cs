namespace workschedule
{
    partial class ScheduleConfigMenu
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
            this.btnCountLimitDay = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnStaffSelect = new System.Windows.Forms.Button();
            this.btnStaffDayOnly = new System.Windows.Forms.Button();
            this.lblWard = new System.Windows.Forms.Label();
            this.lblWardValue = new System.Windows.Forms.Label();
            this.lblStaffKindValue = new System.Windows.Forms.Label();
            this.lblStaffKind = new System.Windows.Forms.Label();
            this.btnEmergencyDate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCountLimitDay
            // 
            this.btnCountLimitDay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(127)))), ((int)(((byte)(117)))));
            this.btnCountLimitDay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnCountLimitDay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCountLimitDay.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnCountLimitDay.ForeColor = System.Drawing.Color.White;
            this.btnCountLimitDay.Location = new System.Drawing.Point(50, 229);
            this.btnCountLimitDay.Name = "btnCountLimitDay";
            this.btnCountLimitDay.Size = new System.Drawing.Size(292, 50);
            this.btnCountLimitDay.TabIndex = 5;
            this.btnCountLimitDay.Text = "曜日別勤務人数設定";
            this.btnCountLimitDay.UseVisualStyleBackColor = false;
            this.btnCountLimitDay.Click += new System.EventHandler(this.btnCountLimitDay_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(117)))), ((int)(((byte)(117)))));
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(50, 384);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(292, 48);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "終了";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnStaffSelect
            // 
            this.btnStaffSelect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(127)))), ((int)(((byte)(117)))));
            this.btnStaffSelect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnStaffSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStaffSelect.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnStaffSelect.ForeColor = System.Drawing.Color.White;
            this.btnStaffSelect.Location = new System.Drawing.Point(50, 117);
            this.btnStaffSelect.Name = "btnStaffSelect";
            this.btnStaffSelect.Size = new System.Drawing.Size(292, 50);
            this.btnStaffSelect.TabIndex = 10;
            this.btnStaffSelect.Text = "対象月の勤務スタッフ登録";
            this.btnStaffSelect.UseVisualStyleBackColor = false;
            this.btnStaffSelect.Click += new System.EventHandler(this.btnStaffSelect_Click);
            // 
            // btnStaffDayOnly
            // 
            this.btnStaffDayOnly.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(127)))), ((int)(((byte)(117)))));
            this.btnStaffDayOnly.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnStaffDayOnly.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStaffDayOnly.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnStaffDayOnly.ForeColor = System.Drawing.Color.White;
            this.btnStaffDayOnly.Location = new System.Drawing.Point(50, 173);
            this.btnStaffDayOnly.Name = "btnStaffDayOnly";
            this.btnStaffDayOnly.Size = new System.Drawing.Size(292, 50);
            this.btnStaffDayOnly.TabIndex = 11;
            this.btnStaffDayOnly.Text = "常日勤設定";
            this.btnStaffDayOnly.UseVisualStyleBackColor = false;
            this.btnStaffDayOnly.Click += new System.EventHandler(this.btnStaffDayOnly_Click);
            // 
            // lblWard
            // 
            this.lblWard.AutoSize = true;
            this.lblWard.Font = new System.Drawing.Font("Meiryo UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblWard.Location = new System.Drawing.Point(46, 25);
            this.lblWard.Name = "lblWard";
            this.lblWard.Size = new System.Drawing.Size(80, 24);
            this.lblWard.TabIndex = 12;
            this.lblWard.Text = "病棟　：";
            // 
            // lblWardValue
            // 
            this.lblWardValue.AutoSize = true;
            this.lblWardValue.Font = new System.Drawing.Font("Meiryo UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblWardValue.Location = new System.Drawing.Point(132, 25);
            this.lblWardValue.Name = "lblWardValue";
            this.lblWardValue.Size = new System.Drawing.Size(67, 24);
            this.lblWardValue.TabIndex = 13;
            this.lblWardValue.Text = "Ｘ病棟";
            // 
            // lblStaffKindValue
            // 
            this.lblStaffKindValue.AutoSize = true;
            this.lblStaffKindValue.Font = new System.Drawing.Font("Meiryo UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblStaffKindValue.Location = new System.Drawing.Point(132, 65);
            this.lblStaffKindValue.Name = "lblStaffKindValue";
            this.lblStaffKindValue.Size = new System.Drawing.Size(67, 24);
            this.lblStaffKindValue.TabIndex = 14;
            this.lblStaffKindValue.Text = "看護師";
            // 
            // lblStaffKind
            // 
            this.lblStaffKind.AutoSize = true;
            this.lblStaffKind.Font = new System.Drawing.Font("Meiryo UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblStaffKind.Location = new System.Drawing.Point(46, 65);
            this.lblStaffKind.Name = "lblStaffKind";
            this.lblStaffKind.Size = new System.Drawing.Size(80, 24);
            this.lblStaffKind.TabIndex = 15;
            this.lblStaffKind.Text = "職種　：";
            // 
            // btnEmergencyDate
            // 
            this.btnEmergencyDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(127)))), ((int)(((byte)(117)))));
            this.btnEmergencyDate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnEmergencyDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEmergencyDate.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnEmergencyDate.ForeColor = System.Drawing.Color.White;
            this.btnEmergencyDate.Location = new System.Drawing.Point(50, 285);
            this.btnEmergencyDate.Name = "btnEmergencyDate";
            this.btnEmergencyDate.Size = new System.Drawing.Size(292, 50);
            this.btnEmergencyDate.TabIndex = 16;
            this.btnEmergencyDate.Text = "救急指定日設定";
            this.btnEmergencyDate.UseVisualStyleBackColor = false;
            this.btnEmergencyDate.Click += new System.EventHandler(this.btnEmergencyDate_Click);
            // 
            // ScheduleConfigMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(392, 458);
            this.Controls.Add(this.btnEmergencyDate);
            this.Controls.Add(this.lblStaffKind);
            this.Controls.Add(this.lblStaffKindValue);
            this.Controls.Add(this.lblWardValue);
            this.Controls.Add(this.lblWard);
            this.Controls.Add(this.btnStaffDayOnly);
            this.Controls.Add(this.btnStaffSelect);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCountLimitDay);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScheduleConfigMenu";
            this.Text = "詳細設定メニュー";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnCountLimitDay;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnStaffSelect;
        private System.Windows.Forms.Button btnStaffDayOnly;
        private System.Windows.Forms.Label lblWard;
        private System.Windows.Forms.Label lblWardValue;
        private System.Windows.Forms.Label lblStaffKindValue;
        private System.Windows.Forms.Label lblStaffKind;
        private System.Windows.Forms.Button btnEmergencyDate;
    }
}