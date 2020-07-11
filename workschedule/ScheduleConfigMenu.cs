using System;
using System.Windows.Forms;

namespace workschedule
{
    public partial class ScheduleConfigMenu : Form
    {
        public string pstrWardID;
        public string pstrWardText;
        public string pstrTargetMonth;
        public string pstrStaffKindID;
        public string pstrStaffKindName;

        public ScheduleConfigMenu(string strWardID, string strWardText, string strTargetMonth, string strStaffKindID, string strStaffKindName)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            pstrWardID = strWardID;
            pstrWardText = strWardText;
            pstrTargetMonth = strTargetMonth;
            pstrStaffKindID = strStaffKindID;
            pstrStaffKindName = strStaffKindName;

            lblWardValue.Text = strWardText;
            lblStaffKindValue.Text = strStaffKindName;

            // 特定病棟のみ表示するメニュー
            switch(strWardID)
            {
                case "00":
                case "06":
                    btnEmergencyDate.Visible = true;
                    break;
                default:
                    btnEmergencyDate.Visible = false;
                    break;
            }
        }


        // ボタンイベント

        /// <summary>
        /// 「対象月の勤務スタッフ登録」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStaffSelect_Click(object sender, EventArgs e)
        {
            EditStaffList frmEditStaff = new EditStaffList(pstrWardID, pstrWardText, pstrTargetMonth, pstrStaffKindID, pstrStaffKindName);

            this.Hide();
            frmEditStaff.ShowDialog();
            this.Show();
        }

        /// <summary>
        /// 「常日勤設定」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStaffDayOnly_Click(object sender, EventArgs e)
        {
            EditStaffDayOnly frmEditStaffDayOnly = new EditStaffDayOnly(pstrWardID, pstrTargetMonth, pstrStaffKindID);

            this.Hide();
            frmEditStaffDayOnly.ShowDialog();
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

        /// <summary>
        /// 「曜日別勤務人数設定」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCountLimitDay_Click(object sender, EventArgs e)
        {
            EditCountLimitDay frmEditCountLimitDay = new EditCountLimitDay(pstrWardID);

            this.Hide();
            frmEditCountLimitDay.ShowDialog();
            this.Show();
        }

        /// <summary>
        /// 「救急指定日設定」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEmergencyDate_Click(object sender, EventArgs e)
        {
            EditEmergencyDate frmEditEmergencyDate = new EditEmergencyDate();

            this.Hide();
            frmEditEmergencyDate.ShowDialog();
            this.Show();
        }
    }
}
