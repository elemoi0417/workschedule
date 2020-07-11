using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using workschedule.Controls;
using workschedule.Functions;

namespace workschedule
{
    public partial class Youshiki9Check : Form
    {
        public bool pbActivatedFlag = false;// アクティベート実行フラグ
        
        public string pstrWardID;           // 対象病棟ID
        public string pstrTargetMonth;      // 対象年月
        public string pstrTargetDays;       // 対象年月の日数

        DataTable dtWardYoushiki9Flag;      // 病棟別様式9チェックフラグマスタ
        DataTable dtWardYoushiki9;          // 様式9チェック値データ
        DataTable dtYoushiki9Check_Schedule;// 様式9チェック実データ(予定データ)
        DataTable dtYoushiki9Check_Result;  // 様式9チェック実データ(実績データ)

        DataRow drWardYoushiki9Flag;        // 様式9チェックフラグマスタのデータ行
        DataRow drWardYoushiki9;            // 様式9チェック値のデータ行

        // 使用クラス宣言
        DatabaseControl clsDatabaseControl = new DatabaseControl();
        Youshiki9CheckControl clsYoushiki9CheckControl = new Youshiki9CheckControl();

        public Youshiki9Check(string strWardID, string strWardName, string strTargetMonth)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            // 共通変数に各種値をセット
            pstrWardID = strWardID;
            pstrTargetMonth = strTargetMonth.Substring(0, 4) + strTargetMonth.Substring(5, 2);
            pstrTargetDays = DateTime.DaysInMonth(int.Parse(pstrTargetMonth.Substring(0, 4)), int.Parse(pstrTargetMonth.Substring(4, 2))).ToString();

            // 対象病棟、対象年月をラベルにセット
            lblWard.Text = strWardName;
            lblTargetMonth.Text = strTargetMonth;
        }

        // --- 各種イベント ---
        /// <summary>
        /// フォームアクティベート時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Youshiki9Check_Activated(object sender, EventArgs e)
        {
            if(pbActivatedFlag == false)
            {
                // 初期化ルーチン処理
                InitialProcess();

                // アクティベートフラグ変更
                pbActivatedFlag = true;
            }
        }

        // --- ボタンイベント ---

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
        /// 初期化ルーチン処理
        /// </summary>
        private void InitialProcess()
        {
            // 各種データテーブル取得
            dtWardYoushiki9Flag = clsDatabaseControl.GetWardYoushiki9Flag_Ward(pstrWardID);
            dtWardYoushiki9 = clsDatabaseControl.GetWardYoushiki9_TargetMonth_Ward(pstrTargetMonth, pstrWardID);
            dtYoushiki9Check_Schedule = clsDatabaseControl.GetYoushiki9Check_Schedule(pstrTargetMonth, pstrWardID);
            dtYoushiki9Check_Result = clsDatabaseControl.GetYoushiki9Check_Result(pstrTargetMonth, pstrWardID);

            // データが存在しない場合
            if (dtWardYoushiki9.Rows.Count <= 0)
            {
                MessageBox.Show("対象月の様式9マスタが無いため、チェックできませんでした。");
                // add start 2020.07.08 WataruT 様式9のマスタが無い場合、画面を閉じれなくなる
                Close();
                // add end 2020.07.08 WataruT 様式9のマスタが無い場合、画面を閉じれなくなる
                return;
            }

            // DataRowを作成
            drWardYoushiki9Flag = dtWardYoushiki9Flag.Rows[0];
            drWardYoushiki9 = dtWardYoushiki9.Rows[0];

            // 様式9の各値をテキストボックスにセット
            SetData();
        }

        /// <summary>
        /// 様式9の各値をテキストボックスにセット
        /// </summary>
        private void SetData()
        {
            // 基準値をセット
            SetDataBorder();

            // 予定値をセット
            SetDataSchedule();

            // 実績値をセット
            SetDataResult();

            // 実績値 - 基準値をセット
            SetDataResultDifference();
        }

        /// <summary>
        /// 基準値をセット
        /// </summary>
        private void SetDataBorder()
        {
            if (drWardYoushiki9Flag["flag_check1"].ToString() == "1")
                txtBorder_Check1.Text = clsYoushiki9CheckControl.GetCheck1_Border(drWardYoushiki9);
            else
                txtBorder_Check1.Enabled = false;

            if (drWardYoushiki9Flag["flag_check2"].ToString() == "1")
                txtBorder_Check2.Text = clsYoushiki9CheckControl.GetCheck2_Border(drWardYoushiki9,txtBorder_Check1.Text);
            else
                txtBorder_Check2.Enabled = false;

            if (drWardYoushiki9Flag["flag_check3"].ToString() == "1")
                txtBorder_Check3.Text = clsYoushiki9CheckControl.GetCheck3_Border(drWardYoushiki9);
            else
                txtBorder_Check3.Enabled = false;

            if (drWardYoushiki9Flag["flag_check4"].ToString() == "1")
                txtBorder_Check4.Text = clsYoushiki9CheckControl.GetCheck4_Border(drWardYoushiki9);
            else
                txtBorder_Check4.Enabled = false;

            if (drWardYoushiki9Flag["flag_check5"].ToString() == "1")
                txtBorder_Check5.Text = clsYoushiki9CheckControl.GetCheck5_Border(txtBorder_Check1.Text, pstrTargetDays);
            else
                txtBorder_Check5.Enabled = false;

            if (drWardYoushiki9Flag["flag_check6"].ToString() == "1")
                txtBorder_Check6.Text = clsYoushiki9CheckControl.GetCheck6_Border(txtBorder_Check2.Text, pstrTargetDays);
            else
                txtBorder_Check6.Enabled = false;

            if (drWardYoushiki9Flag["flag_check7"].ToString() == "1")
                txtBorder_Check7.Text = clsYoushiki9CheckControl.GetCheck7_Border(txtBorder_Check3.Text, pstrTargetDays);
            else
                txtBorder_Check7.Enabled = false;

            if (drWardYoushiki9Flag["flag_check8"].ToString() == "1")
                txtBorder_Check8.Text = drWardYoushiki9["nurse_percentage1"].ToString();
            else
                txtBorder_Check8.Enabled = false;

            if (drWardYoushiki9Flag["flag_check9"].ToString() == "1")
                txtBorder_Check9.Text = drWardYoushiki9["nurse_percentage2"].ToString();
            else
                txtBorder_Check9.Enabled = false;
        }

        /// <summary>
        /// 予定値をセット
        /// </summary>
        private void SetDataSchedule()
        {
            if (drWardYoushiki9Flag["flag_check5"].ToString() == "1")
            {
                txtSchedule_Check5.Text = clsYoushiki9CheckControl.GetCheck5_Schedule(dtYoushiki9Check_Schedule);
                if (double.Parse(txtBorder_Check5.Text) <= double.Parse(txtSchedule_Check5.Text))
                    txtSchedule_Check5.ForeColor = Color.Blue;
                else
                    txtSchedule_Check5.ForeColor = Color.Red;
            }
            else
                txtSchedule_Check5.Enabled = false;

            if (drWardYoushiki9Flag["flag_check6"].ToString() == "1")
            {
                txtSchedule_Check6.Text = clsYoushiki9CheckControl.GetCheck6_Schedule(dtYoushiki9Check_Schedule);
                if (double.Parse(txtBorder_Check6.Text) <= double.Parse(txtSchedule_Check6.Text))
                    txtSchedule_Check6.ForeColor = Color.Blue;
                else
                    txtSchedule_Check6.ForeColor = Color.Red;
            }
            else
                txtSchedule_Check6.Enabled = false;
            if (drWardYoushiki9Flag["flag_check7"].ToString() == "1")
            {
                txtSchedule_Check7.Text = clsYoushiki9CheckControl.GetCheck7_Schedule(dtYoushiki9Check_Schedule, txtSchedule_Check5.Text, txtBorder_Check6.Text,txtSchedule_Check6.Text);
                if (double.Parse(txtBorder_Check7.Text) <= double.Parse(txtSchedule_Check7.Text))
                    txtSchedule_Check7.ForeColor = Color.Blue;
                else
                    txtSchedule_Check7.ForeColor = Color.Red;
            }
            else
                txtSchedule_Check7.Enabled = false;

            if (drWardYoushiki9Flag["flag_check1"].ToString() == "1")
            {
                txtSchedule_Check1.Text = clsYoushiki9CheckControl.GetCheck1_Schedule(txtSchedule_Check5.Text, pstrTargetDays);
                if (double.Parse(txtBorder_Check1.Text) <= double.Parse(txtSchedule_Check1.Text))
                    txtSchedule_Check1.ForeColor = Color.Blue;
                else
                    txtSchedule_Check1.ForeColor = Color.Red;
            }
            else
                txtSchedule_Check1.Enabled = false;

            if (drWardYoushiki9Flag["flag_check2"].ToString() == "1")
            {
                txtSchedule_Check2.Text = clsYoushiki9CheckControl.GetCheck2_Schedule(txtSchedule_Check6.Text, pstrTargetDays);
                if (double.Parse(txtBorder_Check2.Text) <= double.Parse(txtSchedule_Check2.Text))
                    txtSchedule_Check2.ForeColor = Color.Blue;
                else
                    txtSchedule_Check2.ForeColor = Color.Red;
            }
            else
                txtSchedule_Check2.Enabled = false;

            if (drWardYoushiki9Flag["flag_check3"].ToString() == "1")
            {
                txtSchedule_Check3.Text = clsYoushiki9CheckControl.GetCheck3_Schedule(txtSchedule_Check7.Text, pstrTargetDays);
                if (double.Parse(txtBorder_Check3.Text) <= double.Parse(txtSchedule_Check3.Text))
                    txtSchedule_Check3.ForeColor = Color.Blue;
                else
                    txtSchedule_Check3.ForeColor = Color.Red;
            }
            else
                txtSchedule_Check3.Enabled = false;

            if (drWardYoushiki9Flag["flag_check4"].ToString() == "1")
            {
                txtSchedule_Check4.Text = clsYoushiki9CheckControl.GetCheck4_Schedule(dtYoushiki9Check_Schedule, pstrTargetDays);
                if (double.Parse(txtBorder_Check4.Text) <= double.Parse(txtSchedule_Check4.Text))
                    txtSchedule_Check4.ForeColor = Color.Blue;
                else
                    txtSchedule_Check4.ForeColor = Color.Red;
            }
            else
                txtSchedule_Check4.Enabled = false;

            if (drWardYoushiki9Flag["flag_check8"].ToString() == "1")
            {
                txtSchedule_Check8.Text = clsYoushiki9CheckControl.GetCheck8_Schedule(txtSchedule_Check6.Text, txtBorder_Check5.Text);
                if (double.Parse(txtBorder_Check8.Text) <= double.Parse(txtSchedule_Check8.Text))
                    txtSchedule_Check8.ForeColor = Color.Blue;
                else
                    txtSchedule_Check8.ForeColor = Color.Red;
            }
            else
                txtSchedule_Check8.Enabled = false;

            if (drWardYoushiki9Flag["flag_check9"].ToString() == "1")
            {
                txtSchedule_Check9.Text = clsYoushiki9CheckControl.GetCheck9_Schedule(dtYoushiki9Check_Schedule, txtBorder_Check6.Text);
                if (double.Parse(txtBorder_Check9.Text) <= double.Parse(txtSchedule_Check9.Text))
                    txtSchedule_Check9.ForeColor = Color.Blue;
                else
                    txtSchedule_Check9.ForeColor = Color.Red;
            }
            else
                txtSchedule_Check9.Enabled = false;
        }

        /// <summary>
        /// 実績値をセット
        /// </summary>
        private void SetDataResult()
        {
            if (drWardYoushiki9Flag["flag_check5"].ToString() == "1")
            {
                txtResult_Check5.Text = clsYoushiki9CheckControl.GetCheck5_Result(dtYoushiki9Check_Result, dtYoushiki9Check_Schedule, pstrTargetMonth, pstrTargetDays);
                if (double.Parse(txtBorder_Check5.Text) <= double.Parse(txtResult_Check5.Text))
                    txtResult_Check5.ForeColor = Color.Blue;
                else
                    txtResult_Check5.ForeColor = Color.Red;
            }
            else
                txtSchedule_Check5.Enabled = false;

            if (drWardYoushiki9Flag["flag_check6"].ToString() == "1")
            {
                txtResult_Check6.Text = clsYoushiki9CheckControl.GetCheck6_Result(dtYoushiki9Check_Result, dtYoushiki9Check_Schedule, pstrTargetMonth, pstrTargetDays);
                if (double.Parse(txtBorder_Check6.Text) <= double.Parse(txtResult_Check6.Text))
                    txtResult_Check6.ForeColor = Color.Blue;
                else
                    txtResult_Check6.ForeColor = Color.Red;
            }
            else
                txtSchedule_Check6.Enabled = false;

            if (drWardYoushiki9Flag["flag_check7"].ToString() == "1")
            {
                txtResult_Check7.Text = clsYoushiki9CheckControl.GetCheck7_Result(dtYoushiki9Check_Result, dtYoushiki9Check_Schedule, pstrTargetMonth, pstrTargetDays,
                                            txtResult_Check5.Text, txtBorder_Check6.Text, txtResult_Check6.Text);
                if (double.Parse(txtBorder_Check7.Text) <= double.Parse(txtResult_Check7.Text))
                    txtResult_Check7.ForeColor = Color.Blue;
                else
                    txtResult_Check7.ForeColor = Color.Red;
            }
            else
                txtSchedule_Check7.Enabled = false;

            if (drWardYoushiki9Flag["flag_check1"].ToString() == "1")
            {
                txtResult_Check1.Text = clsYoushiki9CheckControl.GetCheck1_Result(txtResult_Check5.Text, pstrTargetDays);
                if (double.Parse(txtBorder_Check1.Text) <= double.Parse(txtResult_Check1.Text))
                    txtResult_Check1.ForeColor = Color.Blue;
                else
                    txtResult_Check1.ForeColor = Color.Red;
            }
            else
                txtResult_Check1.Enabled = false;

            if (drWardYoushiki9Flag["flag_check2"].ToString() == "1")
            {
                txtResult_Check2.Text = clsYoushiki9CheckControl.GetCheck2_Result(txtResult_Check6.Text, pstrTargetDays);
                if (double.Parse(txtBorder_Check2.Text) <= double.Parse(txtResult_Check2.Text))
                    txtResult_Check2.ForeColor = Color.Blue;
                else
                    txtResult_Check2.ForeColor = Color.Red;
            }
            else
                txtResult_Check2.Enabled = false;

            if (drWardYoushiki9Flag["flag_check3"].ToString() == "1")
            {
                txtResult_Check3.Text = clsYoushiki9CheckControl.GetCheck3_Result(txtResult_Check7.Text, pstrTargetDays);
                if (double.Parse(txtBorder_Check3.Text) <= double.Parse(txtResult_Check3.Text))
                    txtResult_Check3.ForeColor = Color.Blue;
                else
                    txtResult_Check3.ForeColor = Color.Red;
            }
            else
                txtResult_Check3.Enabled = false;

            if (drWardYoushiki9Flag["flag_check4"].ToString() == "1")
            {
                txtResult_Check4.Text = clsYoushiki9CheckControl.GetCheck4_Result(dtYoushiki9Check_Result, dtYoushiki9Check_Schedule, pstrTargetMonth, pstrTargetDays);
                if (double.Parse(txtBorder_Check4.Text) <= double.Parse(txtResult_Check4.Text))
                    txtResult_Check4.ForeColor = Color.Blue;
                else
                    txtResult_Check4.ForeColor = Color.Red;
            }
            else
                txtResult_Check4.Enabled = false;

            if (drWardYoushiki9Flag["flag_check8"].ToString() == "1")
            {
                txtResult_Check8.Text = clsYoushiki9CheckControl.GetCheck8_Result(txtResult_Check6.Text, txtBorder_Check5.Text);
                if (double.Parse(txtBorder_Check8.Text) <= double.Parse(txtResult_Check8.Text))
                    txtResult_Check8.ForeColor = Color.Blue;
                else
                    txtResult_Check8.ForeColor = Color.Red;
            }
            else
                txtResult_Check8.Enabled = false;

            if (drWardYoushiki9Flag["flag_check9"].ToString() == "1")
            {
                txtResult_Check9.Text = clsYoushiki9CheckControl.GetCheck9_Result(dtYoushiki9Check_Result, dtYoushiki9Check_Schedule, txtBorder_Check6.Text, pstrTargetMonth, pstrTargetDays);
                if (double.Parse(txtBorder_Check9.Text) <= double.Parse(txtResult_Check9.Text))
                    txtResult_Check9.ForeColor = Color.Blue;
                else
                    txtResult_Check9.ForeColor = Color.Red;
            }
            else
                txtResult_Check9.Enabled = false;
        }

        /// <summary>
        /// 実績 - 基準をセット
        /// </summary>
        private void SetDataResultDifference()
        {
            

            if (drWardYoushiki9Flag["flag_check1"].ToString() == "1")
            {
                txtResultDifference_Check1.Text = (double.Parse(txtResult_Check1.Text) - double.Parse(txtBorder_Check1.Text)).ToString();
                if (double.Parse(txtResultDifference_Check1.Text) >= 0)
                    txtResultDifference_Check1.ForeColor = Color.Blue;
                else
                    txtResultDifference_Check1.ForeColor = Color.Red;
            }
            else
                txtResultDifference_Check1.Enabled = false;

            if (drWardYoushiki9Flag["flag_check2"].ToString() == "1")
            {
                txtResultDifference_Check2.Text = (double.Parse(txtResult_Check2.Text) - double.Parse(txtBorder_Check2.Text)).ToString();
                if (double.Parse(txtResultDifference_Check2.Text) >= 0)
                    txtResultDifference_Check2.ForeColor = Color.Blue;
                else
                    txtResultDifference_Check2.ForeColor = Color.Red;
            }
            else
                txtResultDifference_Check2.Enabled = false;

            if (drWardYoushiki9Flag["flag_check3"].ToString() == "1")
            {
                txtResultDifference_Check3.Text = (double.Parse(txtResult_Check3.Text) - double.Parse(txtBorder_Check3.Text)).ToString();
                if (double.Parse(txtResultDifference_Check3.Text) >= 0)
                    txtResultDifference_Check3.ForeColor = Color.Blue;
                else
                    txtResultDifference_Check3.ForeColor = Color.Red;
            }
            else
                txtResultDifference_Check3.Enabled = false;

            if (drWardYoushiki9Flag["flag_check4"].ToString() == "1")
            {
                txtResultDifference_Check4.Text = (double.Parse(txtResult_Check4.Text) - double.Parse(txtBorder_Check4.Text)).ToString();
                if (double.Parse(txtResultDifference_Check4.Text) >= 0)
                    txtResultDifference_Check4.ForeColor = Color.Blue;
                else
                    txtResultDifference_Check4.ForeColor = Color.Red;
            }
            else
                txtResultDifference_Check4.Enabled = false;
            
            if (drWardYoushiki9Flag["flag_check5"].ToString() == "1")
            {
                txtResultDifference_Check5.Text = (double.Parse(txtResult_Check5.Text) - double.Parse(txtBorder_Check5.Text)).ToString();
                if (double.Parse(txtResultDifference_Check5.Text) >= 0)
                    txtResultDifference_Check5.ForeColor = Color.Blue;
                else
                    txtResultDifference_Check5.ForeColor = Color.Red;
            }
            else
                txtResultDifference_Check5.Enabled = false;

            if (drWardYoushiki9Flag["flag_check6"].ToString() == "1")
            {
                txtResultDifference_Check6.Text = (double.Parse(txtResult_Check6.Text) - double.Parse(txtBorder_Check6.Text)).ToString();
                if (double.Parse(txtResultDifference_Check6.Text) >= 0)
                    txtResultDifference_Check6.ForeColor = Color.Blue;
                else
                    txtResultDifference_Check6.ForeColor = Color.Red;
            }
            else
                txtResultDifference_Check6.Enabled = false;

            if (drWardYoushiki9Flag["flag_check7"].ToString() == "1")
            {
                txtResultDifference_Check7.Text = (double.Parse(txtResult_Check7.Text) - double.Parse(txtBorder_Check7.Text)).ToString();
                if (double.Parse(txtResultDifference_Check7.Text) >= 0)
                    txtResultDifference_Check7.ForeColor = Color.Blue;
                else
                    txtResultDifference_Check7.ForeColor = Color.Red;
            }
            else
                txtResultDifference_Check7.Enabled = false;

            if (drWardYoushiki9Flag["flag_check8"].ToString() == "1")
            {
                txtResultDifference_Check8.Text = (double.Parse(txtResult_Check8.Text) - double.Parse(txtBorder_Check8.Text)).ToString();
                if (double.Parse(txtResultDifference_Check8.Text) >= 0)
                    txtResultDifference_Check8.ForeColor = Color.Blue;
                else
                    txtResultDifference_Check8.ForeColor = Color.Red;
            }
            else
                txtResultDifference_Check8.Enabled = false;

            if (drWardYoushiki9Flag["flag_check9"].ToString() == "1")
            {
                txtResultDifference_Check9.Text = (double.Parse(txtResult_Check9.Text) - double.Parse(txtBorder_Check9.Text)).ToString();
                if (double.Parse(txtResultDifference_Check9.Text) >= 0)
                    txtResultDifference_Check9.ForeColor = Color.Blue;
                else
                    txtResultDifference_Check9.ForeColor = Color.Red;
            }
            else
                txtResultDifference_Check9.Enabled = false;
        }

    }
}
