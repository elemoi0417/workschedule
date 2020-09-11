using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using workschedule.Controls;
using workschedule.Functions;

namespace workschedule
{
    public partial class EditResultData : Form
    {
        int piScheduleStaff;    // 配列で使用する職員の順番

        // 使用クラス宣言
        DatabaseControl clsDatabaseControl = new DatabaseControl();

        // 親フォーム
        MainSchedule frmMainSchedule;

        public EditResultData(MainSchedule frmMainSchedule_Parent)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            frmMainSchedule = frmMainSchedule_Parent;

            // 配列で使用する職員の順番を変数にセット
            SetScheduleStaffNumber();

            // 勤務種類のコンボボックスをセット
            SetWorkKindComboBox();

            // 各種データをコントロールにセット
            SetData();
        }

        // --- ボタンイベント ---

        /// <summary>
        /// 「登録」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // 各入力項目の内容をチェック
            if (CheckInputOtherTime() == false)
                return;

            // 勤務時間をグリッドにセット
            SetWorkTimeToMainGrid();

            // 勤務種類を共通変数にセット
            SetWorkKind();

            // その他業務を共通変数にセット
            SetOtherWork();

            // 修正フラグをセット
            SetChangeFlag();

            // フォームを閉じる
            Close();
        }

        /// <summary>
        /// 「キャンセル」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // フォームを閉じる
            Close();
        }

        /// <summary>
        /// 「クリア」ボタン(その他1)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear1_Click(object sender, EventArgs e)
        {
            // Mod Start WataruT 2020.09.07 実績入力画面のクリアボタンが動作しない
            //if (MessageBox.Show("クリアしてもよろしいですか？","") == DialogResult.Yes)
            if (MessageBox.Show("クリアしてもよろしいですか？", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            // Mod End   WataruT 2020.09.07 実績入力画面のクリアボタンが動作しない
            {
                cmbOther1WorkKind.Text = "";
                cmbOther1StartTimeHour.SelectedIndex = 0;
                cmbOther1StartTimeMinute.SelectedIndex = 0;
                cmbOther1EndTimeHour.SelectedIndex = 0;
                cmbOther1EndTimeMinute.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 「クリア」ボタン(その他2)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear2_Click(object sender, EventArgs e)
        {
            // Mod Start WataruT 2020.09.07 実績入力画面のクリアボタンが動作しない
            //if (MessageBox.Show("クリアしてもよろしいですか？","") == DialogResult.Yes)
            if (MessageBox.Show("クリアしてもよろしいですか？", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            // Mod End   WataruT 2020.09.07 実績入力画面のクリアボタンが動作しない
            {
                cmbOther2WorkKind.Text = "";
                cmbOther2StartTimeHour.SelectedIndex = 0;
                cmbOther2StartTimeMinute.SelectedIndex = 0;
                cmbOther2EndTimeHour.SelectedIndex = 0;
                cmbOther2EndTimeMinute.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 「クリア」ボタン(その他3)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear3_Click(object sender, EventArgs e)
        {
            // Mod Start WataruT 2020.09.07 実績入力画面のクリアボタンが動作しない
            //if (MessageBox.Show("クリアしてもよろしいですか？","") == DialogResult.Yes)
            if (MessageBox.Show("クリアしてもよろしいですか？", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            // Mod End   WataruT 2020.09.07 実績入力画面のクリアボタンが動作しない
            {
                cmbOther3WorkKind.Text = "";
                cmbOther3StartTimeHour.SelectedIndex = 0;
                cmbOther3StartTimeMinute.SelectedIndex = 0;
                cmbOther3EndTimeHour.SelectedIndex = 0;
                cmbOther3EndTimeMinute.SelectedIndex = 0;
            }

        }

        // --- 各種イベント ---

        // --- ファンクション、サブルーチン ---

        /// <summary>
        /// 各種データをコントロールにセット
        /// </summary>
        private void SetData()
        {
            // 職員氏名
            lblStaffValue.Text = GetStaffName();
            
            // 対象日時
            lblTargetDateValue.Text = GetTargetDate();

            // 勤務種類
            cmbWorkKind.SelectedValue = GetWorkKind();

            // その他業務
            SetOtherWorkToControl();
        }

        /// <summary>
        /// 対象となる職員の配列番号をセット
        /// </summary>
        private void SetScheduleStaffNumber()
        {
            // 先頭なら0とする(計算エラーとなるため)
            if (frmMainSchedule.piGrdMain_CurrentRow == 0)
                piScheduleStaff = 0;
            else
                piScheduleStaff = int.Parse(Math.Floor(double.Parse((frmMainSchedule.piGrdMain_CurrentRow / 3).ToString())).ToString());
        }

        /// <summary>
        /// 勤務種類のコンボボックスにアイテムをセット
        /// </summary>
        private void SetWorkKindComboBox()
        {
            List<ItemSet> srcWorkKind = new List<ItemSet>();

            cmbWorkKind.DataSource = null;

            // 勤務なしを先頭にセット
            srcWorkKind.Add(new ItemSet("勤務無し", ""));

            foreach (DataRow row in frmMainSchedule.dtWorkKind.Rows)
            {
                srcWorkKind.Add(new ItemSet(row["name"].ToString(), row["id"].ToString()));
            }

            cmbWorkKind.DataSource = srcWorkKind;
            cmbWorkKind.DisplayMember = "ItemDisp";
            cmbWorkKind.ValueMember = "ItemValue";
        }

        /// <summary>
        /// 対象職員の氏名を返す
        /// </summary>
        /// <returns></returns>
        private string GetStaffName()
        {
            return frmMainSchedule.astrScheduleStaff[piScheduleStaff, 1];
        }

        /// <summary>
        /// 対象日時を返す
        /// </summary>
        /// <returns></returns>
        private string GetTargetDate()
        {
            return frmMainSchedule.lblTargetMonth.Text + String.Format("{0:D2}", (frmMainSchedule.piGrdMain_CurrentColumn - 1)) + "日";
        }

        /// <summary>
        /// 勤務種類の名称を返す
        /// </summary>
        /// <returns></returns>
        private string GetWorkKind()
        {
            return frmMainSchedule.astrResultWorkKind[piScheduleStaff, frmMainSchedule.piGrdMain_CurrentColumn - 2];
        }

        /// <summary>
        /// その他業務内容を各コントロールにセット
        /// </summary>
        private void SetOtherWorkToControl()
        {
            for (int i = 1; i <= 3; i++)
            {
                if (frmMainSchedule.astrResultOtherWorkTime[piScheduleStaff, frmMainSchedule.piGrdMain_CurrentColumn - 2, i - 1, 0] != "" &&
                    frmMainSchedule.astrResultOtherWorkTime[piScheduleStaff, frmMainSchedule.piGrdMain_CurrentColumn - 2, i - 1, 0] != null)
                {
                    // 業務種類
                    Controls["cmbOther" + i.ToString() + "WorkKind"].Text = 
                        frmMainSchedule.astrResultOtherWorkTime[piScheduleStaff, frmMainSchedule.piGrdMain_CurrentColumn - 2, i - 1, 0];
                    // 開始(時間)
                    Controls["cmbOther" + i.ToString() + "StartTimeHour"].Text = 
                        frmMainSchedule.astrResultOtherWorkTime[piScheduleStaff, frmMainSchedule.piGrdMain_CurrentColumn - 2, i - 1, 1].Substring(0, 2);
                    // 開始(分)
                    Controls["cmbOther" + i.ToString() + "StartTimeMinute"].Text =
                        frmMainSchedule.astrResultOtherWorkTime[piScheduleStaff, frmMainSchedule.piGrdMain_CurrentColumn - 2, i - 1, 1].Substring(3, 2);
                    // 終了(時間)
                    Controls["cmbOther" + i.ToString() + "EndTimeHour"].Text = 
                        frmMainSchedule.astrResultOtherWorkTime[piScheduleStaff, frmMainSchedule.piGrdMain_CurrentColumn - 2, i - 1, 2].Substring(0, 2);
                    // 終了(分)
                    Controls["cmbOther" + i.ToString() + "EndTimeMinute"].Text =
                        frmMainSchedule.astrResultOtherWorkTime[piScheduleStaff, frmMainSchedule.piGrdMain_CurrentColumn - 2, i - 1, 2].Substring(3, 2);
                }
                // Add Start WataruT 2020.09.02 業務種類の項目変更
                if (Controls["cmbOther" + i.ToString() + "WorkKind"].Text == "")
                    ((ComboBox)Controls["cmbOther" + i.ToString() + "WorkKind"]).SelectedIndex = 0;
                // Add End   WataruT 2020.09.02 業務種類の項目変更
            }
        }

        /// <summary>
        /// 勤務時間をセット
        /// </summary>
        private void SetWorkTimeToMainGrid()
        {
            double dTempTime;
            double dOtherTime;

            dOtherTime = GetOtherWorkTimeTotal(piScheduleStaff);

            switch (cmbWorkKind.SelectedValue)
            {
                // 夜勤
                case "02":
                    if (7.0 - dOtherTime < 0)
                        dTempTime = 0;
                    else
                        dTempTime = 7.0 - dOtherTime;
                    frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn, (piScheduleStaff + 1) * 3 - 3].Value = "";
                    frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn, (piScheduleStaff + 1) * 3 - 2].Value = string.Format("{0:F2}", dTempTime);
                    frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn, (piScheduleStaff + 1) * 3 - 1].Value = string.Format("{0:F2}", dTempTime);
                    break;
                // 夜明
                case "03":
                    if (9.0 - dOtherTime < 0)
                        dTempTime = 0;
                    else
                        dTempTime = 9.0 - dOtherTime;
                    frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn, (piScheduleStaff + 1) * 3 - 3].Value = "";
                    frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn, (piScheduleStaff + 1) * 3 - 2].Value = string.Format("{0:F2}", dTempTime);
                    frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn, (piScheduleStaff + 1) * 3 - 1].Value = string.Format("{0:F2}", dTempTime);
                    break;
                // Add Start WataruT 2020.07.30 遅出の表示対応
                // 遅出
                case "11":
                    if (2.0 - dOtherTime < 0)
                        dTempTime = 0;
                    else
                        dTempTime = 2.0 - dOtherTime;
                    frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn, (piScheduleStaff + 1) * 3 - 3].Value = string.Format("{0:F2}", dTempTime);
                    frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn, (piScheduleStaff + 1) * 3 - 2].Value = "6.00";
                    frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn, (piScheduleStaff + 1) * 3 - 1].Value = "6.00";
                    break;

                // Add End   WataruT 2020.07.30 遅出の表示対応
                default:
                    if (clsDatabaseControl.GetWorkKind_WorkTime(cmbWorkKind.SelectedValue.ToString()) == "")
                        frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn, (piScheduleStaff + 1) * 3 - 3].Value = "";
                    else
                    {
                        dTempTime = double.Parse(clsDatabaseControl.GetWorkKind_WorkTime(cmbWorkKind.SelectedValue.ToString()));
                        if (dTempTime - dOtherTime < 0)
                            dTempTime = 0;
                        else
                            dTempTime = dTempTime - dOtherTime;
                        frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn, (piScheduleStaff + 1) * 3 - 3].Value = string.Format("{0:F2}", dTempTime);
                    }
                    frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn, (piScheduleStaff + 1) * 3 - 2].Value = "";
                    frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn, (piScheduleStaff + 1) * 3 - 1].Value = "";
                    break;
            }
        }

        /// <summary>
        /// その他業務を共通変数にセット
        /// </summary>
        private void SetOtherWork()
        {
            for (int i = 1; i <= 3; i++)
            {
                if (Controls["cmbOther" + i.ToString() + "WorkKind"].Text != "")
                {
                    // 業務種類
                    frmMainSchedule.astrResultOtherWorkTime[piScheduleStaff, frmMainSchedule.piGrdMain_CurrentColumn - 2, i - 1, 0] = 
                        Controls["cmbOther" + i.ToString() + "WorkKind"].Text;
                    // 開始時刻
                    frmMainSchedule.astrResultOtherWorkTime[piScheduleStaff, frmMainSchedule.piGrdMain_CurrentColumn - 2, i - 1, 1] = 
                        Controls["cmbOther" + i.ToString() + "StartTimeHour"].Text + ":" + Controls["cmbOther" + i.ToString() + "StartTimeMinute"].Text;
                    // 終了時刻
                    frmMainSchedule.astrResultOtherWorkTime[piScheduleStaff, frmMainSchedule.piGrdMain_CurrentColumn - 2, i - 1, 2] = 
                        Controls["cmbOther" + i.ToString() + "EndTimeHour"].Text + ":" + Controls["cmbOther" + i.ToString() + "EndTimeMinute"].Text;
                }
                // Add Start WataruT 2020.09.11 実績クリア時のデータ登録不具合対応
                else
                {
                    // 業務種類
                    frmMainSchedule.astrResultOtherWorkTime[piScheduleStaff, frmMainSchedule.piGrdMain_CurrentColumn - 2, i - 1, 0] = "";
                    // 開始時刻
                    frmMainSchedule.astrResultOtherWorkTime[piScheduleStaff, frmMainSchedule.piGrdMain_CurrentColumn - 2, i - 1, 1] = "";
                    // 終了時刻
                    frmMainSchedule.astrResultOtherWorkTime[piScheduleStaff, frmMainSchedule.piGrdMain_CurrentColumn - 2, i - 1, 2] = "";
                }
                // Add End   WataruT 2020.09.11 実績クリア時のデータ登録不具合対応
            }
        }
        
        /// <summary>
        /// 勤務種類のIDを共通変数にセット
        /// </summary>
        private void SetWorkKind()
        {
            // 共通変数にセット
            frmMainSchedule.astrResultWorkKind[piScheduleStaff, frmMainSchedule.piGrdMain_CurrentColumn - 2] = cmbWorkKind.SelectedValue.ToString();
        }

        /// <summary>
        /// 変更フラグを共通変数にセット
        /// </summary>
        private void SetChangeFlag()
        {
            // 共通変数にセット
            frmMainSchedule.astrResultChangeFlag[piScheduleStaff, frmMainSchedule.piGrdMain_CurrentColumn - 2] = "1";
        }

        /// <summary>
        /// その他業務の合計時間を返す
        /// </summary>
        private double GetOtherWorkTimeTotal(int iScheduleStaff)
        {
            TimeSpan tsStartTime, tsEndTime;                    // 開始時刻・終了時刻の計算用TimeSpan変数
            string strTotalTime;                                // 合計時間
            double dOtherWorkTimeTotal = 0;                     // 計算時の一時変数

            // 現在のその他業務の時刻を配列にセット
            for (int i = 1; i <= 3; i++)
            {
                if(Controls["cmbOther" +  i.ToString() + "WorkKind"].Text != "")
                {
                    // Mod Start WataruT 2020.09.02 その他業務の合計時間を全時間帯対象とする
                    // 開始時刻
                    //if (int.Parse(Controls["cmbOther" + i.ToString() + "StartTimeHour"].Text + Controls["cmbOther" + i.ToString() + "StartTimeMinute"].Text) < 900)
                    //    tsStartTime = new TimeSpan(9, 0, 0);
                    //else if(int.Parse(Controls["cmbOther" + i.ToString() + "StartTimeHour"].Text + Controls["cmbOther" + i.ToString() + "StartTimeMinute"].Text) > 1800)
                    //    tsStartTime = new TimeSpan(18, 0, 0);
                    //else
                    //    tsStartTime = new TimeSpan(int.Parse(Controls["cmbOther" + i.ToString() + "StartTimeHour"].Text), int.Parse(Controls["cmbOther" + i.ToString() + "StartTimeMinute"].Text), 0);

                    //// 終了時刻
                    //if (int.Parse(Controls["cmbOther" + i.ToString() + "EndTimeHour"].Text + Controls["cmbOther" + i.ToString() + "EndTimeMinute"].Text) < 900)
                    //    tsEndTime = new TimeSpan(9, 0, 0);
                    //else if (int.Parse(Controls["cmbOther" + i.ToString() + "EndTimeHour"].Text + Controls["cmbOther" + i.ToString() + "EndTimeMinute"].Text) > 1800)
                    //    tsEndTime = new TimeSpan(18, 0, 0);
                    //else
                    //    tsEndTime = new TimeSpan(int.Parse(Controls["cmbOther" + i.ToString() + "EndTimeHour"].Text), int.Parse(Controls["cmbOther" + i.ToString() + "EndTimeMinute"].Text), 0);
                    tsStartTime = new TimeSpan(int.Parse(Controls["cmbOther" + i.ToString() + "StartTimeHour"].Text), int.Parse(Controls["cmbOther" + i.ToString() + "StartTimeMinute"].Text), 0);
                    tsEndTime = new TimeSpan(int.Parse(Controls["cmbOther" + i.ToString() + "EndTimeHour"].Text), int.Parse(Controls["cmbOther" + i.ToString() + "EndTimeMinute"].Text), 0);
                    // Mod End   WataruT 2020.09.02 その他業務の合計時間を全時間帯対象とする

                    // 時刻の差分を変数にセット
                    strTotalTime = (tsEndTime - tsStartTime).ToString();

                    // 数値に変換して一時ファイルにセット
                    dOtherWorkTimeTotal += double.Parse(strTotalTime.Substring(0, 2)) + double.Parse(strTotalTime.Substring(3, 2)) / 60;
                }
            }

            return dOtherWorkTimeTotal;
        }

        /// <summary>
        /// その他業務の入力内容チェック
        /// </summary>
        /// <returns></returns>
        private bool CheckInputOtherTime()
        {
            int iTemp = 0;          // チェック用の一時変数

            for(int i = 1; i <= 3; i++)
            {
                // 開始時刻または終了時刻が入力されている場合、業務種類の入力も確認
                if (Controls["cmbOther" + i.ToString() + "WorkKind"].Text != "" ||
                    Controls["cmbOther" + i.ToString() + "StartTimeHour"].Text != "" || Controls["cmbOther" + i.ToString() + "StartTimeMinute"].Text != "" ||
                    Controls["cmbOther" + i.ToString() + "EndTimeHour"].Text != "" || Controls["cmbOther" + i.ToString() + "EndTimeMinute"].Text != "")
                {
                    if(Controls["cmbOther" + i.ToString() + "WorkKind"].Text == "")
                    {
                        MessageBox.Show("業務種類を入力してください。");
                        return false;
                    }
                    else
                    {
                        // 開始時刻の時間フォーマットチェック(00 ～ 24)
                        if (Controls["cmbOther" + i.ToString() + "StartTimeHour"].Text != "")
                        {
                            if (int.TryParse(Controls["cmbOther" + i.ToString() + "StartTimeHour"].Text, out iTemp) == true)
                            {
                                if (iTemp < 0 || iTemp > 24)
                                {
                                    MessageBox.Show("開始時刻の時間は\"00\" ～ \"24\"の間の数字で入力してください。");
                                    return false;
                                }
                            }
                            else
                            {
                                MessageBox.Show("開始時刻には数字を入力してください。");
                                return false;
                            }
                        }
                        else
                        {
                            MessageBox.Show("開始時刻を正しく入力してください。");
                            return false;
                        }

                        // 開始時刻の分フォーマットチェック(00 ～ 59)
                        if (Controls["cmbOther" + i.ToString() + "StartTimeMinute"].Text != "")
                        {
                            if (int.TryParse(Controls["cmbOther" + i.ToString() + "StartTimeMinute"].Text, out iTemp) == true)
                            {
                                if (iTemp < 0 || iTemp > 59)
                                {
                                    MessageBox.Show("開始時刻の\"分\"は\"00\" ～ \"59\"の間の数字で入力してください。");
                                    return false;
                                }
                            }
                            else
                            {
                                MessageBox.Show("開始時刻には数字を入力してください。");
                                return false;
                            }
                        }
                        else
                        {
                            MessageBox.Show("開始時刻を正しく入力してください。");
                            return false;
                        }

                        // 終了時刻の時間フォーマットチェック(00 ～ 24)
                        if (Controls["cmbOther" + i.ToString() + "EndTimeHour"].Text != "")
                        {
                            if (int.TryParse(Controls["cmbOther" + i.ToString() + "EndTimeHour"].Text, out iTemp) == true)
                            {
                                if (iTemp < 0 || iTemp > 24)
                                {
                                    MessageBox.Show("終了時刻の時間は\"00\" ～ \"24\"の間の数字で入力してください。");
                                    return false;
                                }
                            }
                            else
                            {
                                MessageBox.Show("終了時刻には数字を入力してください。");
                                return false;
                            }
                        }
                        else
                        {
                            MessageBox.Show("終了時刻を正しく入力してください。");
                            return false;
                        }

                        // 終了時刻の分フォーマットチェック(00 ～ 59)
                        if (Controls["cmbOther" + i.ToString() + "EndTimeMinute"].Text != "")
                        {
                            if (int.TryParse(Controls["cmbOther" + i.ToString() + "EndTimeMinute"].Text, out iTemp) == true)
                            {
                                if (iTemp < 0 || iTemp > 59)
                                {
                                    MessageBox.Show("終了時刻の\"分\"は\"00\" ～ \"59\"の間の数字で入力してください。");
                                    return false;
                                }
                            }
                            else
                            {
                                MessageBox.Show("終了時刻には数字を入力してください。");
                                return false;
                            }
                        }
                        else
                        {
                            MessageBox.Show("終了時刻を正しく入力してください。");
                            return false;
                        }
                        // 開始時刻が終了時刻以前か確認
                        if(int.Parse(Controls["cmbOther" + i.ToString() + "StartTimeHour"].Text + Controls["cmbOther" + i.ToString() + "StartTimeMinute"].Text) >=
                            int.Parse(Controls["cmbOther" + i.ToString() + "EndTimeHour"].Text + Controls["cmbOther" + i.ToString() + "EndTimeMinute"].Text))
                        {
                            MessageBox.Show("終了時刻は開始時刻以降を入力してください。");
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 勤務内容選択時、その他なら手入力可能とする
        /// Add WataruT 2020.09.02 業務種類の項目変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbOtherWorkKind_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbOther1WorkKind.SelectedIndex >= 0 && cmbOther1WorkKind.SelectedIndex <= 3)
                cmbOther1WorkKind.DropDownStyle = ComboBoxStyle.DropDownList;
            else
                cmbOther1WorkKind.DropDownStyle = ComboBoxStyle.DropDown;
            if (cmbOther2WorkKind.SelectedIndex >= 0 && cmbOther2WorkKind.SelectedIndex <= 3)
                cmbOther2WorkKind.DropDownStyle = ComboBoxStyle.DropDownList;
            else
                cmbOther2WorkKind.DropDownStyle = ComboBoxStyle.DropDown;
            if (cmbOther3WorkKind.SelectedIndex >= 0 && cmbOther3WorkKind.SelectedIndex <= 3)
                cmbOther3WorkKind.DropDownStyle = ComboBoxStyle.DropDownList;
            else
                cmbOther3WorkKind.DropDownStyle = ComboBoxStyle.DropDown;

        }
    }
}
