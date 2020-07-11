using System;
using System.Windows.Forms;
using workschedule.ReportsForm;

namespace workschedule
{
    public partial class ReportListMenu : Form
    {
        public ReportListMenu()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            // リストに項目をセット
            SetReportList();
        }

        // --- ボタンイベント ---

        /// <summary>
        /// 「選択」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            //選択した帳票のメニューを表示
            ShowSelectReportMenu();
        }

        /// <summary>
        /// 「閉じる」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        // --- ファンクション、サブルーチン ---

        /// <summary>
        /// 帳票一覧リストのリストをセット
        /// </summary>
        private void SetReportList()
        {
            // リストのクリア
            lstReport.Items.Clear();

            // リストの項目をセット
            lstReport.Items.Add("勤務計画表(月初)");
            lstReport.Items.Add("勤務計画表(締翌日)");
            lstReport.Items.Add("様式9");
            lstReport.Items.Add("実績項目集計");
        }

        /// <summary>
        /// 選択した帳票のメニュー画面を表示
        /// </summary>
        private void ShowSelectReportMenu()
        {
            // リストから選択されているか確認
            if(lstReport.SelectedItems.Count > 0)
            {
                switch (lstReport.SelectedItem.ToString())
                {
                    case "勤務計画表(月初)":
                        ReportWorkScheduleMenu frmReportWorkScheduleMenu = new ReportWorkScheduleMenu();
                        frmReportWorkScheduleMenu.ShowDialog();
                        break;
                    case "勤務計画表(締翌日)":
                        ReportWorkScheduleHalfMenu frmReportWorkScheduleHalfMenu = new ReportWorkScheduleHalfMenu();
                        frmReportWorkScheduleHalfMenu.ShowDialog();
                        break;
                    case "様式9":
                        ReportYoushiki9Menu frmReportYoushiki9Menu = new ReportYoushiki9Menu();
                        frmReportYoushiki9Menu.ShowDialog();
                        break;
                    case "実績項目集計":
                        ReportResultDetailItemList frmReportResultDetailItemList = new ReportResultDetailItemList();
                        frmReportResultDetailItemList.ShowDialog();
                        break;
                }
            }
        }
    }
}
