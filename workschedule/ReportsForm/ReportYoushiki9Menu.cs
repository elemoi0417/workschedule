using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using workschedule.Controls;
using workschedule.Functions;
using workschedule.Reports;

namespace workschedule.ReportsForm
{
    public partial class ReportYoushiki9Menu : Form
    {
        // 使用クラス宣言
        DatabaseControl clsDatabaseControl = new DatabaseControl();

        public ReportYoushiki9Menu()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            // 各種コンボボックスをセット
            SetWardComboBox();
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
            switch(cmbWard.SelectedValue)
            {
                case "01":
                    Ward1_Youshiki9 clsWard1_Youshiki9 = new Ward1_Youshiki9(cmbTargetYear.Text + cmbTargetMonth.Text, cmbWard.SelectedValue.ToString(), cmbWard.Text);
                    clsWard1_Youshiki9.SaveFile();
                    break;
                case "02":
                    Ward2_Youshiki9 clsWard2_Youshiki9 = new Ward2_Youshiki9(cmbTargetYear.Text + cmbTargetMonth.Text, cmbWard.SelectedValue.ToString(), cmbWard.Text);
                    clsWard2_Youshiki9.SaveFile();
                    break;
                case "03":
                    Ward3_Youshiki9 clsWard3_Youshiki9 = new Ward3_Youshiki9(cmbTargetYear.Text + cmbTargetMonth.Text, cmbWard.SelectedValue.ToString(), cmbWard.Text);
                    clsWard3_Youshiki9.SaveFile();
                    break;
                case "04":
                    Ward4_Youshiki9 clsWard4_Youshiki9 = new Ward4_Youshiki9(cmbTargetYear.Text + cmbTargetMonth.Text, cmbWard.SelectedValue.ToString(), cmbWard.Text);
                    clsWard4_Youshiki9.SaveFile();
                    break;
                case "05":
                    Ward5_Youshiki9 clsWard5_Youshiki9 = new Ward5_Youshiki9(cmbTargetYear.Text + cmbTargetMonth.Text, cmbWard.SelectedValue.ToString(), cmbWard.Text);
                    clsWard5_Youshiki9.SaveFile();
                    break;
                case "06":
                    Ward6_Youshiki9 clsWard6_Youshiki9 = new Ward6_Youshiki9(cmbTargetYear.Text + cmbTargetMonth.Text, cmbWard.SelectedValue.ToString(), cmbWard.Text);
                    clsWard6_Youshiki9.SaveFile();
                    break;
            }
        }

        // --- ファンクション、サブルーチン ---

        /// <summary>
        /// 病棟マスタをコンボボックスにセット
        /// </summary>
        public void SetWardComboBox()
        {
            DataTable dtWard;
            List<ItemSet> srcWard = new List<ItemSet>();

            cmbWard.DataSource = null;

            dtWard = clsDatabaseControl.GetWard();

            foreach (DataRow row in dtWard.Rows)
            {
                srcWard.Add(new ItemSet(row["name"].ToString(), row["id"].ToString()));
            }

            cmbWard.DataSource = srcWard;
            cmbWard.DisplayMember = "ItemDisp";
            cmbWard.ValueMember = "ItemValue";
            cmbWard.SelectedIndex = 0;
        }

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
