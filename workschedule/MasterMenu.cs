using System;
using System.Windows.Forms;

namespace workschedule
{
    public partial class MasterMenu : Form
    {   
        public MasterMenu()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

        }

        // ボタンイベント

        /// <summary>
        /// 「職員マスタ管理」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStaff_Click(object sender, EventArgs e)
        {
            EditStaffMaster frmEditStaffMaster = new EditStaffMaster();

            this.Hide();
            frmEditStaffMaster.ShowDialog();
            this.Show();
        }

        /// <summary>
        /// 「様式９データ入力」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInputYoushiki9_Click(object sender, EventArgs e)
        {
            EditYoushiki9 frmEditYoushiki9 = new EditYoushiki9();

            this.Hide();
            frmEditYoushiki9.ShowDialog();
            this.Show();
        }

        /// <summary>
        /// 「終了」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }


    }
}
