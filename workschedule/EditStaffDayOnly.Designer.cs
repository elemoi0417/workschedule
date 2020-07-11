namespace workschedule
{
    partial class EditStaffDayOnly
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.grdStaff = new workschedule.Controls.DataGridViewEx();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.targetdate_start = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.targetdate_end = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.holiday_flag = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.office_flag = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.staff_level = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.btnSetStaffDayOnly = new System.Windows.Forms.Button();
            this.btnReleaseStaffDayOnly = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grdStaff)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(127)))), ((int)(((byte)(117)))));
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(592, 534);
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
            this.btnClose.Location = new System.Drawing.Point(12, 534);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(79, 50);
            this.btnClose.TabIndex = 21;
            this.btnClose.Text = "閉じる";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // grdStaff
            // 
            this.grdStaff.AllowUserToAddRows = false;
            this.grdStaff.AllowUserToDeleteRows = false;
            this.grdStaff.AllowUserToResizeColumns = false;
            this.grdStaff.AllowUserToResizeRows = false;
            this.grdStaff.ColumnHeaderBorderStyle = workschedule.Controls.DataGridViewEx.HeaderCellBorderStyle.SingleLine;
            this.grdStaff.ColumnHeaderRowCount = 1;
            this.grdStaff.ColumnHeaderRowHeight = 17;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdStaff.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grdStaff.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdStaff.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.name,
            this.targetdate_start,
            this.targetdate_end,
            this.holiday_flag,
            this.office_flag,
            this.staff_level});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdStaff.DefaultCellStyle = dataGridViewCellStyle3;
            this.grdStaff.Location = new System.Drawing.Point(12, 12);
            this.grdStaff.Name = "grdStaff";
            this.grdStaff.RowHeadersVisible = false;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.grdStaff.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.grdStaff.RowTemplate.Height = 21;
            this.grdStaff.Size = new System.Drawing.Size(659, 509);
            this.grdStaff.TabIndex = 19;
            this.grdStaff.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdStaff_CellEnter);
            this.grdStaff.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdStaff_CellValidated);
            this.grdStaff.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdStaff_KeyDown);
            // 
            // name
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.name.DefaultCellStyle = dataGridViewCellStyle2;
            this.name.HeaderText = "氏名";
            this.name.Name = "name";
            this.name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.name.Width = 140;
            // 
            // targetdate_start
            // 
            this.targetdate_start.HeaderText = "常日勤開始日";
            this.targetdate_start.Name = "targetdate_start";
            this.targetdate_start.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // targetdate_end
            // 
            this.targetdate_end.HeaderText = "常日勤終了日";
            this.targetdate_end.Name = "targetdate_end";
            this.targetdate_end.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // holiday_flag
            // 
            this.holiday_flag.HeaderText = "土日出勤";
            this.holiday_flag.Items.AddRange(new object[] {
            "〇",
            "×"});
            this.holiday_flag.Name = "holiday_flag";
            this.holiday_flag.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // office_flag
            // 
            this.office_flag.HeaderText = "事務業務";
            this.office_flag.Items.AddRange(new object[] {
            "〇",
            "×"});
            this.office_flag.Name = "office_flag";
            // 
            // staff_level
            // 
            this.staff_level.HeaderText = "職員レベル";
            this.staff_level.Items.AddRange(new object[] {
            "レベル１",
            "レベル２",
            "レベル３"});
            this.staff_level.Name = "staff_level";
            // 
            // btnSetStaffDayOnly
            // 
            this.btnSetStaffDayOnly.BackColor = System.Drawing.Color.Teal;
            this.btnSetStaffDayOnly.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSetStaffDayOnly.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetStaffDayOnly.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSetStaffDayOnly.ForeColor = System.Drawing.Color.White;
            this.btnSetStaffDayOnly.Location = new System.Drawing.Point(316, 534);
            this.btnSetStaffDayOnly.Name = "btnSetStaffDayOnly";
            this.btnSetStaffDayOnly.Size = new System.Drawing.Size(79, 50);
            this.btnSetStaffDayOnly.TabIndex = 22;
            this.btnSetStaffDayOnly.Text = "常日勤\r\n設定";
            this.btnSetStaffDayOnly.UseVisualStyleBackColor = false;
            this.btnSetStaffDayOnly.Click += new System.EventHandler(this.btnSetStaffDayOnly_Click);
            // 
            // btnReleaseStaffDayOnly
            // 
            this.btnReleaseStaffDayOnly.BackColor = System.Drawing.Color.Teal;
            this.btnReleaseStaffDayOnly.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnReleaseStaffDayOnly.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReleaseStaffDayOnly.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnReleaseStaffDayOnly.ForeColor = System.Drawing.Color.White;
            this.btnReleaseStaffDayOnly.Location = new System.Drawing.Point(401, 534);
            this.btnReleaseStaffDayOnly.Name = "btnReleaseStaffDayOnly";
            this.btnReleaseStaffDayOnly.Size = new System.Drawing.Size(79, 50);
            this.btnReleaseStaffDayOnly.TabIndex = 23;
            this.btnReleaseStaffDayOnly.Text = "常日勤\r\n解除";
            this.btnReleaseStaffDayOnly.UseVisualStyleBackColor = false;
            this.btnReleaseStaffDayOnly.Click += new System.EventHandler(this.btnReleaseStaffDayOnly_Click);
            // 
            // EditStaffDayOnly
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(683, 596);
            this.Controls.Add(this.btnReleaseStaffDayOnly);
            this.Controls.Add(this.btnSetStaffDayOnly);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grdStaff);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditStaffDayOnly";
            this.Text = "常日勤設定";
            ((System.ComponentModel.ISupportInitialize)(this.grdStaff)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.DataGridViewEx grdStaff;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn targetdate_start;
        private System.Windows.Forms.DataGridViewTextBoxColumn targetdate_end;
        private System.Windows.Forms.DataGridViewComboBoxColumn holiday_flag;
        private System.Windows.Forms.DataGridViewComboBoxColumn office_flag;
        private System.Windows.Forms.DataGridViewComboBoxColumn staff_level;
        private System.Windows.Forms.Button btnSetStaffDayOnly;
        private System.Windows.Forms.Button btnReleaseStaffDayOnly;
    }
}