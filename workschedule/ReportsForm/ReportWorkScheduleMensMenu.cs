// Add WataruT 2021.02.10 男性職員の予定表出力機能追加
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using workschedule.Controls;
using workschedule.Functions;
using workschedule.Reports;

namespace workschedule.ReportsForm
{
    public partial class ReportWorkScheduleMensMenu : Form
    {
        // 使用クラス宣言
        DatabaseControl clsDatabaseControl = new DatabaseControl();

        public ReportWorkScheduleMensMenu()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            // 各種コンボボックスをセット
            SetTargetYearComboBox();
            SetTargetMonthComboBox();
        }

        /// <summary>
        /// 「キャンセル」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 「印刷」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintWorkScheduleMens clsPrintWorkSchedule = new PrintWorkScheduleMens(cmbTargetYear.Text, cmbTargetMonth.Text);
            clsPrintWorkSchedule.SaveFile();
        }

        // --- ファンクション、サブルーチン ---


        /// <summary>
        /// 対象年をコンボボックスにセット
        /// </summary>
        public void SetTargetYearComboBox()
        {
            cmbTargetYear.Items.Clear();

            cmbTargetYear.Items.Add("2020");
            cmbTargetYear.Items.Add("2021");
            cmbTargetYear.Items.Add("2022");
            cmbTargetYear.Items.Add("2023");
            cmbTargetYear.Items.Add("2024");
            cmbTargetYear.Items.Add("2025");
            cmbTargetYear.Items.Add("2026");

            cmbTargetYear.Text = DateTime.Now.ToString("yyyy");
        }

        /// <summary>
        /// 対象月をコンボボックスにセット
        /// </summary>
        public void SetTargetMonthComboBox()
        {
            cmbTargetMonth.Items.Clear();

            cmbTargetMonth.Items.Add("01");
            cmbTargetMonth.Items.Add("02");
            cmbTargetMonth.Items.Add("03");
            cmbTargetMonth.Items.Add("04");
            cmbTargetMonth.Items.Add("05");
            cmbTargetMonth.Items.Add("06");
            cmbTargetMonth.Items.Add("07");
            cmbTargetMonth.Items.Add("08");
            cmbTargetMonth.Items.Add("09");
            cmbTargetMonth.Items.Add("10");
            cmbTargetMonth.Items.Add("11");
            cmbTargetMonth.Items.Add("12");

            cmbTargetMonth.Text = DateTime.Now.ToString("MM");
        }
    }
}
