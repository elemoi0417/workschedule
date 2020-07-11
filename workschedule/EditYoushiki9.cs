using System;
using System.Data;
using System.Windows.Forms;
using workschedule.Controls;
using workschedule.Functions;

namespace workschedule
{
    public partial class EditYoushiki9 : Form
    {
        public DataTable dtWard;            // 病棟マスタデーブル

        // 使用クラス宣言
        DatabaseControl clsDatabaseControl = new DatabaseControl();
        DataTableControl clsDataTableControl = new DataTableControl();

        public EditYoushiki9()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            // 初期起動時の処理
            InitialProcess();

            // テキストボックスの様式9データをクリア
            ClearTextBoxData();

            // 様式9データをセット
            SetData(lblTargetMonth.Text.Substring(0, 4) + lblTargetMonth.Text.Substring(5, 2));
        }

        // --- ボタンイベント ---

        /// <summary>
        /// 「→」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNextMonth_Click(object sender, EventArgs e)
        {
            DateTime dt;

            // 現在の年月をDateTimeにセット
            dt = DateTime.ParseExact(lblTargetMonth.Text.Substring(0, 4) + lblTargetMonth.Text.Substring(5, 2) + "01", "yyyyMMdd", null);

            // 前月にする
            dt = dt.AddMonths(1);

            // ラベルに年月をセットする
            lblTargetMonth.Text = dt.ToString("yyyy年MM月");

            // テキストボックスの様式9データをクリア
            ClearTextBoxData();

            // 様式9データをセット
            SetData(lblTargetMonth.Text.Substring(0, 4) + lblTargetMonth.Text.Substring(5, 2));
        }

        /// <summary>
        /// 「←」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBeforeMonth_Click(object sender, EventArgs e)
        {
            DateTime dt;

            // 現在の年月をDateTimeにセット
            dt = DateTime.ParseExact(lblTargetMonth.Text.Substring(0, 4) + lblTargetMonth.Text.Substring(5, 2) + "01", "yyyyMMdd", null);

            // 前月にする
            dt = dt.AddMonths(-1);

            // ラベルに年月をセットする
            lblTargetMonth.Text = dt.ToString("yyyy年MM月");

            // テキストボックスの様式9データをクリア
            ClearTextBoxData();

            // 様式9データをセット
            SetData(lblTargetMonth.Text.Substring(0, 4) + lblTargetMonth.Text.Substring(5, 2));
        }

        /// <summary>
        /// 「前月のデータを引用」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportBeforeData_Click(object sender, EventArgs e)
        {

            DataTable dtWardYoushiki9;
            DateTime dt;

            // 現在の年月をDateTimeにセット
            dt = DateTime.ParseExact(lblTargetMonth.Text.Substring(0, 4) + lblTargetMonth.Text.Substring(5, 2) + "01", "yyyyMMdd", null);

            // 前月にする
            dt = dt.AddMonths(-1);

            // 前月データをDBから取得
            dtWardYoushiki9 = clsDatabaseControl.GetWardYoushiki9_TargetMonth(dt.ToString("yyyyMM"));

            // 前月データの存在チェック
            if (dtWardYoushiki9.Rows.Count <= 0)
            {
                // データ無しのメッセージを表示して処理終了
                MessageBox.Show("前月のデータがありません。", "", MessageBoxButtons.OK);
                return;
            }

            // テキストボックスの様式9データをクリア
            ClearTextBoxData();

            // 前月データをテキストボックスにセット
            SetData(dt.ToString("yyyyMM"));
        }

        /// <summary>
        /// 「保存」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // 確認メッセージの表示
            if (MessageBox.Show("保存してもよろしいですか？", "確認", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // 様式9データの保存処理
                SaveData();
            }
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

        // --- 各種イベント ---

        // --- ファンクション、サブルーチン ---


        /// <summary>
        /// 初期化ルーチン処理
        /// </summary>
        private void InitialProcess()
        {
            // 各種データテーブル取得
            dtWard = clsDatabaseControl.GetWard();

            // 現在の年月をセット
            lblTargetMonth.Text = DateTime.Now.ToString("yyyy年MM月");
        }

        /// <summary>
        /// 各テキストボックスの様式9データをクリア
        /// </summary>
        private void ClearTextBoxData()
        {
            for (int iWard = 1; iWard <= 6; iWard++)
            {
                Controls["txtKubun_Ward" + iWard.ToString()].Text = "";
                Controls["txtNurseCount_Ward" + iWard.ToString()].Text = "";
                Controls["txtCareCount_Ward" + iWard.ToString()].Text = "";
                Controls["txtWardCount_Ward" + iWard.ToString()].Text = "";
                Controls["txtBedCount_Ward" + iWard.ToString()].Text = "";
                Controls["txtAverageDay_Ward" + iWard.ToString()].Text = "";
                Controls["txtNursePercentage1_Ward" + iWard.ToString()].Text = "";
                Controls["txtNursePercentage2_Ward" + iWard.ToString()].Text = "";
                Controls["txtAverageYear_Ward" + iWard.ToString()].Text = "";
            }
        }

        /// <summary>
        /// 様式9データを各テキストボックスにセット
        /// </summary>
        private void SetData(string strTargetMonth)
        {
            DataTable dtWardYoushiki9;

            dtWardYoushiki9 = clsDatabaseControl.GetWardYoushiki9_TargetMonth(strTargetMonth);

            foreach (DataRow row in dtWardYoushiki9.Rows)
            {
                for (int iWard = 1; iWard <= 6; iWard++)
                {
                    if (row["ward"].ToString() == String.Format("{0:D2}", iWard))
                    {
                        Controls["txtKubun_Ward" + iWard.ToString()].Text = row["kubun"].ToString();
                        Controls["txtNurseCount_Ward" + iWard.ToString()].Text = row["nurse_count"].ToString();
                        Controls["txtCareCount_Ward" + iWard.ToString()].Text = row["care_count"].ToString();
                        Controls["txtWardCount_Ward" + iWard.ToString()].Text = row["ward_count"].ToString();
                        Controls["txtBedCount_Ward" + iWard.ToString()].Text = row["bed_count"].ToString();
                        Controls["txtAverageDay_Ward" + iWard.ToString()].Text = row["average_day"].ToString();
                        Controls["txtNursePercentage1_Ward" + iWard.ToString()].Text = row["nurse_percentage1"].ToString();
                        Controls["txtNursePercentage2_Ward" + iWard.ToString()].Text = row["nurse_percentage2"].ToString();
                        Controls["txtAverageYear_Ward" + iWard.ToString()].Text = row["average_year"].ToString();
                    }
                }
            }
        }

        /// <summary>
        /// 様式9データを保存
        /// </summary>
        private void SaveData()
        {
            DataTable dtWardYoushiki9;
            DataRow drWardYoushiki9;

            // 勤務予定ヘッダの作成
            dtWardYoushiki9 = clsDataTableControl.GetTable_WardYoushiki9();
            drWardYoushiki9 = dtWardYoushiki9.NewRow();

            // 既存の様式9データを削除
            clsDatabaseControl.DeleteWardYoushiki9_TargetMonth(lblTargetMonth.Text.Substring(0, 4) + lblTargetMonth.Text.Substring(5, 2));

            // 様式9データをDataRowにセット
            for (int iWard = 1; iWard <= 6; iWard++)
            {
                drWardYoushiki9["ward"] = String.Format("{0:D2}", iWard);
                drWardYoushiki9["target_month"] = lblTargetMonth.Text.Substring(0, 4) + lblTargetMonth.Text.Substring(5, 2);
                drWardYoushiki9["kubun"] = Controls["txtKubun_Ward" + iWard.ToString()].Text;
                drWardYoushiki9["nurse_count"] = Controls["txtNurseCount_Ward" + iWard.ToString()].Text;
                drWardYoushiki9["care_count"] = Controls["txtCareCount_Ward" + iWard.ToString()].Text;
                drWardYoushiki9["ward_count"] = Controls["txtWardCount_Ward" + iWard.ToString()].Text;
                drWardYoushiki9["bed_count"] = Controls["txtBedCount_Ward" + iWard.ToString()].Text;
                drWardYoushiki9["average_day"] = Controls["txtAverageDay_Ward" + iWard.ToString()].Text;
                drWardYoushiki9["nurse_percentage1"] = Controls["txtNursePercentage1_Ward" + iWard.ToString()].Text;
                drWardYoushiki9["nurse_percentage2"] = Controls["txtNursePercentage2_Ward" + iWard.ToString()].Text;
                drWardYoushiki9["average_year"] = Controls["txtAverageYear_Ward" + iWard.ToString()].Text;

                // データ登録処理
                clsDatabaseControl.InsertWardYoushiki9(drWardYoushiki9);
            }

            // 完了メッセージの表示
            MessageBox.Show("保存完了しました。", "", MessageBoxButtons.OK);
        }
    }
}
