namespace workschedule
{
    partial class EditStaffList
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnToRight = new System.Windows.Forms.Button();
            this.lstWard = new System.Windows.Forms.ListBox();
            this.lstStaffLeft = new System.Windows.Forms.ListBox();
            this.lstStaffRight = new System.Windows.Forms.ListBox();
            this.btnToRightAll = new System.Windows.Forms.Button();
            this.btnToLeftAll = new System.Windows.Forms.Button();
            this.btnToLeft = new System.Windows.Forms.Button();
            this.lblTargetDateValue = new System.Windows.Forms.Label();
            this.lblTargetDate = new System.Windows.Forms.Label();
            this.lblTargetWard = new System.Windows.Forms.Label();
            this.lblTargetWardValue = new System.Windows.Forms.Label();
            this.lblStaffKind = new System.Windows.Forms.Label();
            this.lblStaffKindValue = new System.Windows.Forms.Label();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(127)))), ((int)(((byte)(117)))));
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(464, 460);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(79, 50);
            this.btnSave.TabIndex = 20;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(117)))), ((int)(((byte)(117)))));
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(12, 460);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(79, 50);
            this.btnClose.TabIndex = 21;
            this.btnClose.Text = "閉じる";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnToRight
            // 
            this.btnToRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(117)))), ((int)(((byte)(117)))));
            this.btnToRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnToRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToRight.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnToRight.ForeColor = System.Drawing.Color.White;
            this.btnToRight.Location = new System.Drawing.Point(319, 196);
            this.btnToRight.Name = "btnToRight";
            this.btnToRight.Size = new System.Drawing.Size(51, 33);
            this.btnToRight.TabIndex = 24;
            this.btnToRight.Text = ">";
            this.btnToRight.UseVisualStyleBackColor = false;
            this.btnToRight.Click += new System.EventHandler(this.btnToRight_Click);
            // 
            // lstWard
            // 
            this.lstWard.Font = new System.Drawing.Font("メイリオ", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lstWard.FormattingEnabled = true;
            this.lstWard.ItemHeight = 23;
            this.lstWard.Location = new System.Drawing.Point(12, 105);
            this.lstWard.Name = "lstWard";
            this.lstWard.Size = new System.Drawing.Size(128, 349);
            this.lstWard.TabIndex = 25;
            this.lstWard.SelectedIndexChanged += new System.EventHandler(this.lstWard_SelectedIndexChanged);
            // 
            // lstStaffLeft
            // 
            this.lstStaffLeft.Font = new System.Drawing.Font("メイリオ", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lstStaffLeft.FormattingEnabled = true;
            this.lstStaffLeft.ItemHeight = 23;
            this.lstStaffLeft.Location = new System.Drawing.Point(146, 105);
            this.lstStaffLeft.Name = "lstStaffLeft";
            this.lstStaffLeft.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstStaffLeft.Size = new System.Drawing.Size(167, 349);
            this.lstStaffLeft.TabIndex = 26;
            // 
            // lstStaffRight
            // 
            this.lstStaffRight.Font = new System.Drawing.Font("メイリオ", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lstStaffRight.FormattingEnabled = true;
            this.lstStaffRight.ItemHeight = 23;
            this.lstStaffRight.Location = new System.Drawing.Point(376, 105);
            this.lstStaffRight.Name = "lstStaffRight";
            this.lstStaffRight.Size = new System.Drawing.Size(167, 349);
            this.lstStaffRight.TabIndex = 27;
            // 
            // btnToRightAll
            // 
            this.btnToRightAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(117)))), ((int)(((byte)(117)))));
            this.btnToRightAll.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnToRightAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToRightAll.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnToRightAll.ForeColor = System.Drawing.Color.White;
            this.btnToRightAll.Location = new System.Drawing.Point(319, 235);
            this.btnToRightAll.Name = "btnToRightAll";
            this.btnToRightAll.Size = new System.Drawing.Size(51, 33);
            this.btnToRightAll.TabIndex = 28;
            this.btnToRightAll.Text = ">>";
            this.btnToRightAll.UseVisualStyleBackColor = false;
            this.btnToRightAll.Click += new System.EventHandler(this.btnToRightAll_Click);
            // 
            // btnToLeftAll
            // 
            this.btnToLeftAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(117)))), ((int)(((byte)(117)))));
            this.btnToLeftAll.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnToLeftAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToLeftAll.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnToLeftAll.ForeColor = System.Drawing.Color.White;
            this.btnToLeftAll.Location = new System.Drawing.Point(319, 274);
            this.btnToLeftAll.Name = "btnToLeftAll";
            this.btnToLeftAll.Size = new System.Drawing.Size(51, 33);
            this.btnToLeftAll.TabIndex = 30;
            this.btnToLeftAll.Text = "<<";
            this.btnToLeftAll.UseVisualStyleBackColor = false;
            this.btnToLeftAll.Click += new System.EventHandler(this.btnToLeftAll_Click);
            // 
            // btnToLeft
            // 
            this.btnToLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(117)))), ((int)(((byte)(117)))));
            this.btnToLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnToLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToLeft.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnToLeft.ForeColor = System.Drawing.Color.White;
            this.btnToLeft.Location = new System.Drawing.Point(319, 313);
            this.btnToLeft.Name = "btnToLeft";
            this.btnToLeft.Size = new System.Drawing.Size(51, 33);
            this.btnToLeft.TabIndex = 29;
            this.btnToLeft.Text = "<";
            this.btnToLeft.UseVisualStyleBackColor = false;
            this.btnToLeft.Click += new System.EventHandler(this.btnToLeft_Click);
            // 
            // lblTargetDateValue
            // 
            this.lblTargetDateValue.AutoSize = true;
            this.lblTargetDateValue.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblTargetDateValue.Location = new System.Drawing.Point(97, 39);
            this.lblTargetDateValue.Name = "lblTargetDateValue";
            this.lblTargetDateValue.Size = new System.Drawing.Size(107, 19);
            this.lblTargetDateValue.TabIndex = 31;
            this.lblTargetDateValue.Text = "YYYY年DD月";
            // 
            // lblTargetDate
            // 
            this.lblTargetDate.AutoSize = true;
            this.lblTargetDate.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblTargetDate.Location = new System.Drawing.Point(12, 39);
            this.lblTargetDate.Name = "lblTargetDate";
            this.lblTargetDate.Size = new System.Drawing.Size(84, 19);
            this.lblTargetDate.TabIndex = 32;
            this.lblTargetDate.Text = "対象年月：";
            // 
            // lblTargetWard
            // 
            this.lblTargetWard.AutoSize = true;
            this.lblTargetWard.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblTargetWard.Location = new System.Drawing.Point(12, 9);
            this.lblTargetWard.Name = "lblTargetWard";
            this.lblTargetWard.Size = new System.Drawing.Size(84, 19);
            this.lblTargetWard.TabIndex = 34;
            this.lblTargetWard.Text = "対象病棟：";
            // 
            // lblTargetWardValue
            // 
            this.lblTargetWardValue.AutoSize = true;
            this.lblTargetWardValue.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblTargetWardValue.Location = new System.Drawing.Point(97, 9);
            this.lblTargetWardValue.Name = "lblTargetWardValue";
            this.lblTargetWardValue.Size = new System.Drawing.Size(61, 19);
            this.lblTargetWardValue.TabIndex = 33;
            this.lblTargetWardValue.Text = "XX病棟";
            // 
            // lblStaffKind
            // 
            this.lblStaffKind.AutoSize = true;
            this.lblStaffKind.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblStaffKind.Location = new System.Drawing.Point(12, 69);
            this.lblStaffKind.Name = "lblStaffKind";
            this.lblStaffKind.Size = new System.Drawing.Size(84, 19);
            this.lblStaffKind.TabIndex = 36;
            this.lblStaffKind.Text = "対象職種：";
            // 
            // lblStaffKindValue
            // 
            this.lblStaffKindValue.AutoSize = true;
            this.lblStaffKindValue.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblStaffKindValue.Location = new System.Drawing.Point(97, 69);
            this.lblStaffKindValue.Name = "lblStaffKindValue";
            this.lblStaffKindValue.Size = new System.Drawing.Size(75, 19);
            this.lblStaffKindValue.TabIndex = 35;
            this.lblStaffKindValue.Text = "XXXXXX";
            // 
            // btnUp
            // 
            this.btnUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(117)))), ((int)(((byte)(117)))));
            this.btnUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUp.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnUp.ForeColor = System.Drawing.Color.White;
            this.btnUp.Location = new System.Drawing.Point(546, 105);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(25, 33);
            this.btnUp.TabIndex = 37;
            this.btnUp.Text = "↑";
            this.btnUp.UseVisualStyleBackColor = false;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(117)))), ((int)(((byte)(117)))));
            this.btnDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDown.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnDown.ForeColor = System.Drawing.Color.White;
            this.btnDown.Location = new System.Drawing.Point(546, 144);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(25, 33);
            this.btnDown.TabIndex = 38;
            this.btnDown.Text = "↓";
            this.btnDown.UseVisualStyleBackColor = false;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // EditStaffList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(583, 524);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.lblStaffKind);
            this.Controls.Add(this.lblStaffKindValue);
            this.Controls.Add(this.lblTargetWard);
            this.Controls.Add(this.lblTargetWardValue);
            this.Controls.Add(this.lblTargetDate);
            this.Controls.Add(this.lblTargetDateValue);
            this.Controls.Add(this.btnToLeftAll);
            this.Controls.Add(this.btnToLeft);
            this.Controls.Add(this.btnToRightAll);
            this.Controls.Add(this.lstStaffRight);
            this.Controls.Add(this.lstStaffLeft);
            this.Controls.Add(this.lstWard);
            this.Controls.Add(this.btnToRight);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditStaffList";
            this.Text = "職員指定";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnToRight;
        private System.Windows.Forms.ListBox lstWard;
        private System.Windows.Forms.ListBox lstStaffLeft;
        private System.Windows.Forms.ListBox lstStaffRight;
        private System.Windows.Forms.Button btnToRightAll;
        private System.Windows.Forms.Button btnToLeftAll;
        private System.Windows.Forms.Button btnToLeft;
        private System.Windows.Forms.Label lblTargetDateValue;
        private System.Windows.Forms.Label lblTargetDate;
        private System.Windows.Forms.Label lblTargetWard;
        private System.Windows.Forms.Label lblTargetWardValue;
        private System.Windows.Forms.Label lblStaffKind;
        private System.Windows.Forms.Label lblStaffKindValue;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
    }
}