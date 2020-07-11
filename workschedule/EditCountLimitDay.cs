using System;
using System.Data;
using System.Windows.Forms;
using workschedule.Controls;
using workschedule.Functions;

namespace workschedule
{
    public partial class EditCountLimitDay : Form
    {
        public string pstrTargetWard;                       // 対象病棟

        DataTable dtCountLimitDay_Nurse;                    // 曜日別勤務日数制限値データ(看護師)
        DataTable dtCountLimitDay_Care;                     // 曜日別勤務日数制限値データ(ケア)

        // 使用クラス宣言
        DatabaseControl clsDatabaseControl = new DatabaseControl();
        DataTableControl clsDataTableControl = new DataTableControl();

        public EditCountLimitDay(string strWard)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            // 共通変数設定
            pstrTargetWard = strWard;

            // 初期起動時の処理
            InitialProcess();

            // テキストボックスのデータをクリア
            ClearTextBoxData();

            // 既存データをコントロールにセット
            SetCountLimitDay();
        }

        /// <summary>
        /// 「保存」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // DB登録処理
            SaveCountLimitDay();

            // メッセージ表示
            MessageBox.Show("登録完了");
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

        /// <summary>
        /// 初期化ルーチン処理
        /// </summary>
        private void InitialProcess()
        {
            // 各種データテーブル取得
            dtCountLimitDay_Nurse = clsDatabaseControl.GetCountLimitDay_Ward(pstrTargetWard, "01");
            dtCountLimitDay_Care = clsDatabaseControl.GetCountLimitDay_Ward(pstrTargetWard, "02");
        }

        /// <summary>
        /// 既存データをコントロールにセット
        /// </summary>
        private void SetCountLimitDay()
        {
            // 看護師データ
            foreach(DataRow row in dtCountLimitDay_Nurse.Rows)
            {
                switch(row["day_of_week"])
                {
                    case "01":  // 日
                        txtNurseMinDaySun.Text = row["day_min"].ToString();
                        txtNurseMaxNightSun.Text = row["night_max"].ToString();
                        break;
                    case "02":  // 月
                        txtNurseMinDayMon.Text = row["day_min"].ToString();
                        txtNurseMaxNightMon.Text = row["night_max"].ToString();
                        break;
                    case "03":  // 火
                        txtNurseMinDayTue.Text = row["day_min"].ToString();
                        txtNurseMaxNightTue.Text = row["night_max"].ToString();
                        break;
                    case "04":  // 水
                        txtNurseMinDayWed.Text = row["day_min"].ToString();
                        txtNurseMaxNightWed.Text = row["night_max"].ToString();
                        break;
                    case "05":  // 木
                        txtNurseMinDayThu.Text = row["day_min"].ToString();
                        txtNurseMaxNightThu.Text = row["night_max"].ToString();
                        break;
                    case "06":  // 金
                        txtNurseMinDayFri.Text = row["day_min"].ToString();
                        txtNurseMaxNightFri.Text = row["night_max"].ToString();
                        break;
                    case "07":  // 土
                        txtNurseMinDaySat.Text = row["day_min"].ToString();
                        txtNurseMaxNightSat.Text = row["night_max"].ToString();
                        break;
                    case "08":  // 祝
                        txtNurseMinDayHol.Text = row["day_min"].ToString();
                        txtNurseMaxNightHol.Text = row["night_max"].ToString();
                        break;
                }
            }

            // ケアデータ
            foreach (DataRow row in dtCountLimitDay_Care.Rows)
            {
                switch (row["day_of_week"])
                {
                    case "01":  // 日
                        txtCareMinDaySun.Text = row["day_min"].ToString();
                        txtCareMaxNightSun.Text = row["night_max"].ToString();
                        break;
                    case "02":  // 月
                        txtCareMinDayMon.Text = row["day_min"].ToString();
                        txtCareMaxNightMon.Text = row["night_max"].ToString();
                        break;
                    case "03":  // 火
                        txtCareMinDayTue.Text = row["day_min"].ToString();
                        txtCareMaxNightTue.Text = row["night_max"].ToString();
                        break;
                    case "04":  // 水
                        txtCareMinDayWed.Text = row["day_min"].ToString();
                        txtCareMaxNightWed.Text = row["night_max"].ToString();
                        break;
                    case "05":  // 木
                        txtCareMinDayThu.Text = row["day_min"].ToString();
                        txtCareMaxNightThu.Text = row["night_max"].ToString();
                        break;
                    case "06":  // 金
                        txtCareMinDayFri.Text = row["day_min"].ToString();
                        txtCareMaxNightFri.Text = row["night_max"].ToString();
                        break;
                    case "07":  // 土
                        txtCareMinDaySat.Text = row["day_min"].ToString();
                        txtCareMaxNightSat.Text = row["night_max"].ToString();
                        break;
                    case "08":  // 祝
                        txtCareMinDayHol.Text = row["day_min"].ToString();
                        txtCareMaxNightHol.Text = row["night_max"].ToString();
                        break;
                }
            }
        }

        /// <summary>
        /// 各テキストボックスのデータをクリア
        /// </summary>
        private void ClearTextBoxData()
        {
            // 看護師
            txtNurseMinDaySun.Text = "0";
            txtNurseMinDayMon.Text = "0";
            txtNurseMinDayTue.Text = "0";
            txtNurseMinDayWed.Text = "0";
            txtNurseMinDayThu.Text = "0";
            txtNurseMinDayFri.Text = "0";
            txtNurseMinDaySat.Text = "0";
            txtNurseMinDayHol.Text = "0";

            txtNurseMaxNightSun.Text = "0";
            txtNurseMaxNightMon.Text = "0";
            txtNurseMaxNightTue.Text = "0";
            txtNurseMaxNightWed.Text = "0";
            txtNurseMaxNightThu.Text = "0";
            txtNurseMaxNightFri.Text = "0";
            txtNurseMaxNightSat.Text = "0";
            txtNurseMaxNightHol.Text = "0";

            // ケア
            txtCareMinDaySun.Text = "0";
            txtCareMinDayMon.Text = "0";
            txtCareMinDayTue.Text = "0";
            txtCareMinDayWed.Text = "0";
            txtCareMinDayThu.Text = "0";
            txtCareMinDayFri.Text = "0";
            txtCareMinDaySat.Text = "0";
            txtCareMinDayHol.Text = "0";

            txtCareMaxNightSun.Text = "0";
            txtCareMaxNightMon.Text = "0";
            txtCareMaxNightTue.Text = "0";
            txtCareMaxNightWed.Text = "0";
            txtCareMaxNightThu.Text = "0";
            txtCareMaxNightFri.Text = "0";
            txtCareMaxNightSat.Text = "0";
            txtCareMaxNightHol.Text = "0";
        }

        /// <summary>
        /// 入力データをDBに登録
        /// </summary>
        private void SaveCountLimitDay()
        {
            DataTable dtCountLimitDay;
            DataRow drCountLimitDay;

            dtCountLimitDay = clsDataTableControl.GetTable_CountLimitDay();
            drCountLimitDay = dtCountLimitDay.NewRow();

            // 既存データの削除
            clsDatabaseControl.DeleteCountLimitDay_Ward(pstrTargetWard);

            // データを登録(看護師)
            for (int iDayOfWeek = 1; iDayOfWeek <= 8; iDayOfWeek++)
            {
                drCountLimitDay["ward"] = pstrTargetWard;
                drCountLimitDay["staff_kind"] = "01";
                drCountLimitDay["day_of_week"] = String.Format("{0:D2}", iDayOfWeek);
                switch(String.Format("{0:D2}", iDayOfWeek))
                {
                    case "01":
                        drCountLimitDay["day_min"] = txtNurseMinDaySun.Text;
                        drCountLimitDay["night_max"] = txtNurseMaxNightSun.Text;
                        break;
                    case "02":
                        drCountLimitDay["day_min"] = txtNurseMinDayMon.Text;
                        drCountLimitDay["night_max"] = txtNurseMaxNightMon.Text;
                        break;
                    case "03":
                        drCountLimitDay["day_min"] = txtNurseMinDayTue.Text;
                        drCountLimitDay["night_max"] = txtNurseMaxNightTue.Text;
                        break;
                    case "04":
                        drCountLimitDay["day_min"] = txtNurseMinDayWed.Text;
                        drCountLimitDay["night_max"] = txtNurseMaxNightWed.Text;
                        break;
                    case "05":
                        drCountLimitDay["day_min"] = txtNurseMinDayThu.Text;
                        drCountLimitDay["night_max"] = txtNurseMaxNightThu.Text;
                        break;
                    case "06":
                        drCountLimitDay["day_min"] = txtNurseMinDayFri.Text;
                        drCountLimitDay["night_max"] = txtNurseMaxNightFri.Text;
                        break;
                    case "07":
                        drCountLimitDay["day_min"] = txtNurseMinDaySat.Text;
                        drCountLimitDay["night_max"] = txtNurseMaxNightSat.Text;
                        break;
                    case "08":
                        drCountLimitDay["day_min"] = txtNurseMinDayHol.Text;
                        drCountLimitDay["night_max"] = txtNurseMaxNightHol.Text;
                        break;
                }

                clsDatabaseControl.InsertCountLimitDay(drCountLimitDay);
            }

            // データを登録(ケア)
            for (int iDayOfWeek = 1; iDayOfWeek <= 8; iDayOfWeek++)
            {
                drCountLimitDay["ward"] = pstrTargetWard;
                drCountLimitDay["staff_kind"] = "02";
                drCountLimitDay["day_of_week"] = String.Format("{0:D2}", iDayOfWeek);
                switch (String.Format("{0:D2}", iDayOfWeek))
                {
                    case "01":
                        drCountLimitDay["day_min"] = txtCareMinDaySun.Text;
                        drCountLimitDay["night_max"] = txtCareMaxNightSun.Text;
                        break;
                    case "02":
                        drCountLimitDay["day_min"] = txtCareMinDayMon.Text;
                        drCountLimitDay["night_max"] = txtCareMaxNightMon.Text;
                        break;
                    case "03":
                        drCountLimitDay["day_min"] = txtCareMinDayTue.Text;
                        drCountLimitDay["night_max"] = txtCareMaxNightTue.Text;
                        break;
                    case "04":
                        drCountLimitDay["day_min"] = txtCareMinDayWed.Text;
                        drCountLimitDay["night_max"] = txtCareMaxNightWed.Text;
                        break;
                    case "05":
                        drCountLimitDay["day_min"] = txtCareMinDayThu.Text;
                        drCountLimitDay["night_max"] = txtCareMaxNightThu.Text;
                        break;
                    case "06":
                        drCountLimitDay["day_min"] = txtCareMinDayFri.Text;
                        drCountLimitDay["night_max"] = txtCareMaxNightFri.Text;
                        break;
                    case "07":
                        drCountLimitDay["day_min"] = txtCareMinDaySat.Text;
                        drCountLimitDay["night_max"] = txtCareMaxNightSat.Text;
                        break;
                    case "08":
                        drCountLimitDay["day_min"] = txtCareMinDayHol.Text;
                        drCountLimitDay["night_max"] = txtCareMaxNightHol.Text;
                        break;
                }

                clsDatabaseControl.InsertCountLimitDay(drCountLimitDay);
            }

        }

    }
}
