﻿using System;
using System.Data;
using System.Windows.Forms;
using workschedule.Controls;
using workschedule.Functions;

namespace workschedule
{
    public partial class EditStaffDayOnly : Form
    {
        public string pstrTargetWard;                       // 対象病棟
        public string pstrTargetMonth;                      // 対象年月
        public string pstrStaffKind;                        // 対象職種

        public DataTable dtScheduleStaff;                   // 職員マスタテーブル
        public DataTable dtStaffDayOnly;                    // 常日勤データテーブル

        string[,] astrStaffDayOnly;                         // 常日勤マスタ配列(人数、ID・開始日・終了日・休日・事務)

        // 使用クラス宣言
        CommonControl clsCommonControl = new CommonControl();
        DatabaseControl clsDatabaseControl = new DatabaseControl();
        DataTableControl clsDataTableControl = new DataTableControl();

        public EditStaffDayOnly(string strWard, string strTargetMonth, string strStaffKind)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            // 共通変数設定
            pstrTargetWard = strWard;
            pstrTargetMonth = strTargetMonth.Substring(0, 4) + strTargetMonth.Substring(5, 2);
            pstrStaffKind = strStaffKind;

            InitialProcess();
            SetStaffDayOnly();
            SetDataGridView();
        }

        /// <summary>
        /// 「保存」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveStaffDayOnly();

            Close();
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

        // -- コントロールイベント --

        /// <summary>
        /// グリッドの入力チェック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdStaff_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            DateTime dt;
            string strFormat = "yyyy/MM/dd";

            // 日付フォーマットチェック
            if ((e.ColumnIndex == 1 || e.ColumnIndex == 2) && grdStaff[e.ColumnIndex, e.RowIndex].Value.ToString() != "")
            {
                if (DateTime.TryParse(grdStaff[e.ColumnIndex, e.RowIndex].Value.ToString(), out dt))
                {
                    grdStaff[e.ColumnIndex, e.RowIndex].Value = dt.ToString(strFormat);
                }
                else
                {
                    MessageBox.Show("日付を正しく入力してください。\n\n例）2020/02/01" +"");
                    grdStaff[e.ColumnIndex, e.RowIndex].Value = "";
                }
            }
        }

        /// <summary>
        /// コンボボックスワンクリック表示イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdStaff_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            if (dgv.Columns[e.ColumnIndex].Name == "holiday_flag" && dgv.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn)
            {
                SendKeys.Send("{F4}");
            }
            if (dgv.Columns[e.ColumnIndex].Name == "office_flag" && dgv.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn)
            {
                SendKeys.Send("{F4}");
            }
            if (dgv.Columns[e.ColumnIndex].Name == "staff_level" && dgv.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn)
            {
                SendKeys.Send("{F4}");
            }
        }

        /// <summary>
        /// メイングリッドのKeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdStaff_KeyDown(object sender, KeyEventArgs e)
        {
            // Deleteキー（カラム削除）
            if (e.KeyData == Keys.Delete)
            {
                if (grdStaff.CurrentCell.ColumnIndex == 1 || grdStaff.CurrentCell.ColumnIndex == 2)
                {
                    grdStaff.CurrentCell.Value = "";
                }
            }
        }


        // -- 各種メソッド、ファンクション --

        /// <summary>
        /// 初期化ルーチン処理
        /// </summary>
        private void InitialProcess()
        {
            // 各種データテーブル取得
            dtScheduleStaff = clsDatabaseControl.GetScheduleStaff_List(pstrTargetWard, pstrTargetMonth, pstrStaffKind);
            dtStaffDayOnly = clsDatabaseControl.GetStaffDayOnly_Ward(pstrTargetWard, pstrStaffKind);

            // 変数の初期化
            CreateNewValiable();

            // データグリッドの複数セル選択不可
            grdStaff.MultiSelect = false;
        }

        /// <summary>
        /// 変数の初期設定
        /// </summary>
        private void CreateNewValiable()
        {
            int iStaffDayOnlyCount = dtStaffDayOnly.Rows.Count;

            astrStaffDayOnly = new string[iStaffDayOnlyCount, 6];
        }

        /// <summary>
        /// 職員情報をグリッドにセット
        /// </summary>
        private void SetDataGridView()
        {
            DataTable dt = new DataTable();
            DataRow nr;

            // 列の自動作成無効
            grdStaff.AutoGenerateColumns = false;

            // データテーブルのカラムをセット
            dt.Columns.Add("STAFF", Type.GetType("System.String"));
            dt.Columns.Add("TARGET_DAY_START", Type.GetType("System.String"));
            dt.Columns.Add("TARGET_DAY_END", Type.GetType("System.String"));
            dt.Columns.Add("HOLIDAY_FLAG", Type.GetType("System.String"));
            dt.Columns.Add("OFFICE_FLAG", Type.GetType("System.String"));
            dt.Columns.Add("STAFF_LEVEL", Type.GetType("System.String"));

            for (int i = 0; i < dtScheduleStaff.Rows.Count; i++)
            {
                nr = dt.NewRow();
                nr["STAFF"] = dtScheduleStaff.Rows[i]["name"].ToString();
                nr["TARGET_DAY_START"] = GetStaffDayOnlyStartDate(dtScheduleStaff.Rows[i]["id"].ToString());
                nr["TARGET_DAY_END"] = GetStaffDayOnlyEndDate(dtScheduleStaff.Rows[i]["id"].ToString());
                nr["HOLIDAY_FLAG"] = GetStaffDayOnlyHolidayFlag(dtScheduleStaff.Rows[i]["id"].ToString());
                nr["OFFICE_FLAG"] = GetStaffDayOnlyOfficeFlag(dtScheduleStaff.Rows[i]["id"].ToString());
                nr["STAFF_LEVEL"] = GetStaffDayOnlyStaffLevelFlag(dtScheduleStaff.Rows[i]["id"].ToString());
                dt.Rows.Add(nr);
            }

            // メイングリッドにデータをセット
            this.grdStaff.DataSource = dt;

            // プロパティ名をセット
            grdStaff.Columns[0].DataPropertyName = dt.Columns[0].ColumnName;
            grdStaff.Columns[1].DataPropertyName = dt.Columns[1].ColumnName;
            grdStaff.Columns[2].DataPropertyName = dt.Columns[2].ColumnName;
            grdStaff.Columns[3].DataPropertyName = dt.Columns[3].ColumnName;
            grdStaff.Columns[4].DataPropertyName = dt.Columns[4].ColumnName;
            grdStaff.Columns[5].DataPropertyName = dt.Columns[5].ColumnName;

            // 職員氏名は編集不可とする
            grdStaff.Columns[0].ReadOnly = true;
        }

        /// <summary>
        /// 常日勤データを登録
        /// </summary>
        private void SaveStaffDayOnly()
        {
            DataTable dtStaffDayOnly;
            DataTable dtScheduleStaff_Update;
            DataRow drStaffDayOnly;
            DataRow drScheduleStaff_Update;

            dtStaffDayOnly = clsDataTableControl.GetTable_StaffDayOnly();
            drStaffDayOnly = dtStaffDayOnly.NewRow();

            dtScheduleStaff_Update = clsDataTableControl.GetTable_ScheduleStaff();
            drScheduleStaff_Update = dtScheduleStaff_Update.NewRow();

            // 既存データの削除
            clsDatabaseControl.DeleteStaffDayOnly_Ward(pstrTargetWard, pstrStaffKind);

            // グリッドのデータを登録
            for(int iRow = 0; iRow < grdStaff.RowCount; iRow++)
            {
                drStaffDayOnly["staff"] = dtScheduleStaff.Rows[iRow]["id"].ToString();
                drStaffDayOnly["staff_kind"] = pstrStaffKind;
                drStaffDayOnly["ward"] = pstrTargetWard;
                drStaffDayOnly["target_day_start"] = grdStaff[1, iRow].Value.ToString();
                drStaffDayOnly["target_day_end"] = grdStaff[2, iRow].Value.ToString();
                drStaffDayOnly["holiday_flag"] = clsCommonControl.ChangeHolidayFlagFormat(grdStaff[3, iRow].Value.ToString());
                drStaffDayOnly["office_flag"] = clsCommonControl.ChangeOfficeFlagFormat(grdStaff[4, iRow].Value.ToString());
                drStaffDayOnly["staff_level"] = clsCommonControl.ChangeStaffLevelFormat(grdStaff[5, iRow].Value.ToString());

                clsDatabaseControl.InsertStaffDayOnly(drStaffDayOnly);

                drScheduleStaff_Update["staff_id"] = dtScheduleStaff.Rows[iRow]["id"].ToString();
                drScheduleStaff_Update["target_month"] = pstrTargetMonth;
                drScheduleStaff_Update["office_flag"] = clsCommonControl.ChangeOfficeFlagFormat(grdStaff[4, iRow].Value.ToString());

                clsDatabaseControl.UpdateScheduleStaff_OfficeFlag(drScheduleStaff_Update);
            }

            MessageBox.Show("保存完了", "", MessageBoxButtons.OK);
        }

        /// <summary>
        /// 土日勤務可能フラグのコンボボックスを返す
        /// </summary>
        /// <returns></returns>
        private DataGridViewComboBoxColumn GetDataGridViewComboBox_Holiday()
        {
            BindingSource bc = new BindingSource();
            DataGridViewComboBoxColumn grdCombo = new DataGridViewComboBoxColumn();

            bc.Add("〇");
            bc.Add("×");

            grdCombo.HeaderText = "土日勤務";
            grdCombo.DataSource = bc;

            return grdCombo;

        }

        /// <summary>
        /// 土日勤務可能フラグのコンボボックスを返す
        /// </summary>
        /// <returns></returns>
        private DataGridViewComboBoxColumn GetDataGridViewComboBox_Office()
        {
            BindingSource bc = new BindingSource();
            DataGridViewComboBoxColumn grdCombo = new DataGridViewComboBoxColumn();

            bc.Add("〇");
            bc.Add("×");

            grdCombo.HeaderText = "事務業務";
            grdCombo.DataSource = bc;

            return grdCombo;

        }

        /// <summary>
        /// 常日勤データをセット
        /// </summary>
        private void SetStaffDayOnly()
        {
            for (int i = 0; i < dtStaffDayOnly.Rows.Count; i++)
            {
                astrStaffDayOnly[i, 0] = dtStaffDayOnly.Rows[i]["staff"].ToString();
                if(dtStaffDayOnly.Rows[i]["target_day_start"].ToString() == "")
                {
                    astrStaffDayOnly[i, 1] = "";
                }
                else
                {
                    astrStaffDayOnly[i, 1] = dtStaffDayOnly.Rows[i]["target_day_start"].ToString().Substring(0, 10);
                }
                if (dtStaffDayOnly.Rows[i]["target_day_end"].ToString() == "")
                {
                    astrStaffDayOnly[i, 2] = "";
                }
                else
                {
                    astrStaffDayOnly[i, 2] = dtStaffDayOnly.Rows[i]["target_day_end"].ToString().Substring(0, 10);
                }
                astrStaffDayOnly[i, 3] = dtStaffDayOnly.Rows[i]["holiday_flag"].ToString();
                astrStaffDayOnly[i, 4] = dtStaffDayOnly.Rows[i]["office_flag"].ToString();
                astrStaffDayOnly[i, 5] = dtStaffDayOnly.Rows[i]["staff_level"].ToString();
            }
        }

        /// <summary>
        /// 常日勤職員の開始日を取得
        /// </summary>
        /// <param name="strStaff"></param>
        /// <returns></returns>
        private string GetStaffDayOnlyStartDate(string strStaff)
        {
            for (int i = 0; i < astrStaffDayOnly.GetLength(0); i++)
            {
                if (astrStaffDayOnly[i, 0] == strStaff)
                    return astrStaffDayOnly[i, 1];
            }
            return "";
        }

        /// <summary>
        /// 常日勤職員の終了日を取得
        /// </summary>
        /// <param name="strStaff"></param>
        /// <returns></returns>
        private string GetStaffDayOnlyEndDate(string strStaff)
        {
            for (int i = 0; i < astrStaffDayOnly.GetLength(0); i++)
            {
                if (astrStaffDayOnly[i, 0] == strStaff)
                    return astrStaffDayOnly[i, 2];
            }
            return "";
        }

        /// <summary>
        /// 休日フラグを取得
        /// </summary>
        /// <param name="strStaff"></param>
        /// <returns></returns>
        private string GetStaffDayOnlyHolidayFlag(string strStaff)
        {
            for (int i = 0; i < astrStaffDayOnly.GetLength(0); i++)
            {
                if (astrStaffDayOnly[i, 0] == strStaff)
                    if (astrStaffDayOnly[i, 3] == "1")
                        return "〇";
            }
            return "×";
        }

        /// <summary>
        /// 事務作業フラグを取得
        /// </summary>
        /// <param name="strStaff"></param>
        /// <returns></returns>
        private string GetStaffDayOnlyOfficeFlag(string strStaff)
        {
            for (int i = 0; i < astrStaffDayOnly.GetLength(0); i++)
            {
                if (astrStaffDayOnly[i, 0] == strStaff)
                    if (astrStaffDayOnly[i, 4] == "1")
                        return "〇";
            }
            return "×";
        }

        /// <summary>
        /// 職員レベルを取得
        /// </summary>
        /// <param name="strStaff"></param>
        /// <returns></returns>
        private string GetStaffDayOnlyStaffLevelFlag(string strStaff)
        {
            for (int i = 0; i < astrStaffDayOnly.GetLength(0); i++)
            {
                if (astrStaffDayOnly[i, 0] == strStaff)
                    switch(astrStaffDayOnly[i, 5])
                    {
                        case "1":
                            return "レベル１";
                        case "2":
                            return "レベル２";
                        case "3":
                            return "レベル３";
                        default:
                            return "レベル１";
                    }
            }

            return "レベル１";
        }

        /// <summary>
        /// 「常日勤設定」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetStaffDayOnly_Click(object sender, EventArgs e)
        {
            if (grdStaff.CurrentCell != null)
            {
                grdStaff[1, grdStaff.CurrentCell.RowIndex].Value = DateTime.Now.ToString("yyyy/MM/dd");
                grdStaff[2, grdStaff.CurrentCell.RowIndex].Value = "2049/12/31";
            }
        }

        /// <summary>
        /// 「常日勤解除」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReleaseStaffDayOnly_Click(object sender, EventArgs e)
        {
            if (grdStaff.CurrentCell != null)
            {
                grdStaff[1, grdStaff.CurrentCell.RowIndex].Value = "";
                grdStaff[2, grdStaff.CurrentCell.RowIndex].Value = "";
            }
        }
    }
}
