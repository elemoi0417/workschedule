namespace workschedule
{
    partial class EditStaffMasterDetail
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
            this.lblWard = new System.Windows.Forms.Label();
            this.cmbWard = new System.Windows.Forms.ComboBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblID = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtID = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblStaffKind = new System.Windows.Forms.Label();
            this.cmbStaffKind = new System.Windows.Forms.ComboBox();
            this.lblStaffPosition = new System.Windows.Forms.Label();
            this.cmbStaffPosition = new System.Windows.Forms.ComboBox();
            this.cmbSex = new System.Windows.Forms.ComboBox();
            this.lblSex = new System.Windows.Forms.Label();
            this.cmbUsingFlag = new System.Windows.Forms.ComboBox();
            this.lblUsingFlag = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblWard
            // 
            this.lblWard.AutoSize = true;
            this.lblWard.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblWard.Location = new System.Drawing.Point(34, 140);
            this.lblWard.Name = "lblWard";
            this.lblWard.Size = new System.Drawing.Size(39, 19);
            this.lblWard.TabIndex = 6;
            this.lblWard.Text = "病棟";
            // 
            // cmbWard
            // 
            this.cmbWard.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWard.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbWard.FormattingEnabled = true;
            this.cmbWard.Location = new System.Drawing.Point(110, 137);
            this.cmbWard.Name = "cmbWard";
            this.cmbWard.Size = new System.Drawing.Size(93, 27);
            this.cmbWard.TabIndex = 7;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblName.Location = new System.Drawing.Point(34, 62);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(39, 19);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "氏名";
            // 
            // lblID
            // 
            this.lblID.AutoSize = true;
            this.lblID.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblID.Location = new System.Drawing.Point(34, 23);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(26, 19);
            this.lblID.TabIndex = 0;
            this.lblID.Text = "ID";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(117)))), ((int)(((byte)(117)))));
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(38, 266);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(85, 50);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "キャンセル";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(150)))), ((int)(((byte)(146)))));
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSave.Location = new System.Drawing.Point(294, 266);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(85, 50);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "登録";
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtID
            // 
            this.txtID.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtID.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtID.Location = new System.Drawing.Point(110, 20);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(93, 27);
            this.txtID.TabIndex = 1;
            this.txtID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtName.Location = new System.Drawing.Point(110, 59);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(169, 27);
            this.txtName.TabIndex = 3;
            this.txtName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblStaffKind
            // 
            this.lblStaffKind.AutoSize = true;
            this.lblStaffKind.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblStaffKind.Location = new System.Drawing.Point(34, 179);
            this.lblStaffKind.Name = "lblStaffKind";
            this.lblStaffKind.Size = new System.Drawing.Size(39, 19);
            this.lblStaffKind.TabIndex = 8;
            this.lblStaffKind.Text = "職種";
            // 
            // cmbStaffKind
            // 
            this.cmbStaffKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStaffKind.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbStaffKind.FormattingEnabled = true;
            this.cmbStaffKind.Location = new System.Drawing.Point(110, 176);
            this.cmbStaffKind.Name = "cmbStaffKind";
            this.cmbStaffKind.Size = new System.Drawing.Size(93, 27);
            this.cmbStaffKind.TabIndex = 9;
            // 
            // lblStaffPosition
            // 
            this.lblStaffPosition.AutoSize = true;
            this.lblStaffPosition.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblStaffPosition.Location = new System.Drawing.Point(232, 179);
            this.lblStaffPosition.Name = "lblStaffPosition";
            this.lblStaffPosition.Size = new System.Drawing.Size(39, 19);
            this.lblStaffPosition.TabIndex = 10;
            this.lblStaffPosition.Text = "役職";
            // 
            // cmbStaffPosition
            // 
            this.cmbStaffPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStaffPosition.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbStaffPosition.FormattingEnabled = true;
            this.cmbStaffPosition.Location = new System.Drawing.Point(286, 176);
            this.cmbStaffPosition.Name = "cmbStaffPosition";
            this.cmbStaffPosition.Size = new System.Drawing.Size(93, 27);
            this.cmbStaffPosition.TabIndex = 11;
            // 
            // cmbSex
            // 
            this.cmbSex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSex.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbSex.FormattingEnabled = true;
            this.cmbSex.Location = new System.Drawing.Point(110, 98);
            this.cmbSex.Name = "cmbSex";
            this.cmbSex.Size = new System.Drawing.Size(93, 27);
            this.cmbSex.TabIndex = 5;
            // 
            // lblSex
            // 
            this.lblSex.AutoSize = true;
            this.lblSex.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblSex.Location = new System.Drawing.Point(34, 101);
            this.lblSex.Name = "lblSex";
            this.lblSex.Size = new System.Drawing.Size(39, 19);
            this.lblSex.TabIndex = 4;
            this.lblSex.Text = "性別";
            // 
            // cmbUsingFlag
            // 
            this.cmbUsingFlag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUsingFlag.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbUsingFlag.FormattingEnabled = true;
            this.cmbUsingFlag.Location = new System.Drawing.Point(110, 215);
            this.cmbUsingFlag.Name = "cmbUsingFlag";
            this.cmbUsingFlag.Size = new System.Drawing.Size(93, 27);
            this.cmbUsingFlag.TabIndex = 15;
            // 
            // lblUsingFlag
            // 
            this.lblUsingFlag.AutoSize = true;
            this.lblUsingFlag.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblUsingFlag.Location = new System.Drawing.Point(34, 218);
            this.lblUsingFlag.Name = "lblUsingFlag";
            this.lblUsingFlag.Size = new System.Drawing.Size(71, 19);
            this.lblUsingFlag.TabIndex = 14;
            this.lblUsingFlag.Text = "使用フラグ";
            // 
            // EditStaffMasterDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(418, 336);
            this.Controls.Add(this.cmbUsingFlag);
            this.Controls.Add(this.lblUsingFlag);
            this.Controls.Add(this.cmbSex);
            this.Controls.Add(this.lblSex);
            this.Controls.Add(this.cmbStaffPosition);
            this.Controls.Add(this.lblStaffPosition);
            this.Controls.Add(this.cmbStaffKind);
            this.Controls.Add(this.lblStaffKind);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtID);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblID);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.cmbWard);
            this.Controls.Add(this.lblWard);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditStaffMasterDetail";
            this.Text = "職員登録";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblWard;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblID;
        public System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblStaffKind;
        private System.Windows.Forms.Label lblStaffPosition;
        private System.Windows.Forms.Label lblSex;
        internal System.Windows.Forms.ComboBox cmbWard;
        internal System.Windows.Forms.ComboBox cmbStaffKind;
        internal System.Windows.Forms.ComboBox cmbStaffPosition;
        internal System.Windows.Forms.ComboBox cmbSex;
        internal System.Windows.Forms.ComboBox cmbUsingFlag;
        private System.Windows.Forms.Label lblUsingFlag;
    }
}