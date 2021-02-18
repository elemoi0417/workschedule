using System;
using System.Data;
using System.Threading;
using System.Windows.Forms;
using workschedule.Controls;

namespace workschedule
{
    public partial class Login : Form
    {
        // 使用クラス宣言
        DatabaseControl clsDatabaseControl = new DatabaseControl();

        public Login()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            txtExId.Focus();
        }

        /// <summary>
        /// 「LOG IN」ボタンクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string strLoginWard;

            // ログインチェック（開発中は無効化）
            strLoginWard = LoginCheck();

            if (strLoginWard == "")
            {
                MessageBox.Show("IDまたはパスワードが違います。", "");

                return;
            }

            MainSchedule frmMainSchedule = new MainSchedule(strLoginWard);

            this.Hide();
            frmMainSchedule.ShowDialog();
            this.Close();
        }

        /// <summary>
        /// ログインチェック
        /// </summary>
        /// <returns></returns>
        private string LoginCheck()
        {
            DataTable dt;

            dt = clsDatabaseControl.GetLoginStaff(txtExId.Text, txtExPassword.Text);

            if (dt != null && dt.Rows.Count != 0)
            {
                return dt.Rows[0]["ward"].ToString();
            }

            return "";

        }

        /// <summary>
        /// Enterキー制御
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (e.Shift)
                {
                    ProcessTabKey(false);
                }
                else
                {
                    ProcessTabKey(true);
                }
            }
        }
    }
}
