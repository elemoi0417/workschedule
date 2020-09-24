using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using workschedule.Controls;
using workschedule.Functions;

namespace workschedule.MainScheduleControl
{
    class MainScheduleResultControl
    {
        // 共通変数
        const int GRID_WIDTH_COLUMN_STAFF  = 115;
        const int GRID_WIDTH_COLUMN_TIMEKIND = 62;
        const int GRID_WIDTH_COLUMN_DATA = 50;

        // 親フォーム
        MainSchedule frmMainSchedule;

        // 使用クラス宣言
        CommonControl clsCommonControl = new CommonControl();
        DatabaseControl clsDatabaseControl = new DatabaseControl();
        DataTableControl clsDataTableControl = new DataTableControl();

        // 初回処理
        public MainScheduleResultControl(MainSchedule frmMainSchedule_Parent)
        {
            frmMainSchedule = frmMainSchedule_Parent;
        }

        /// <summary>
        /// データグリッドに実績データをセット
        /// </summary>
        public void SetMainData_Result()
        {
            DataTable dtResultDetail;
            DataTable dt;
            DataRow dr;

            // データ数を変数にセット
            frmMainSchedule.piScheduleStaffCount = frmMainSchedule.dtScheduleStaff.Rows.Count;
            frmMainSchedule.piDayCount = clsCommonControl.GetTargetMonthDays(frmMainSchedule.lblTargetMonth.Text);
            frmMainSchedule.piWorkKindCount = frmMainSchedule.dtWorkKind.Rows.Count;

            //グリッドの描画処理停止
            frmMainSchedule.grdMain.SuspendLayout();
            frmMainSchedule.grdMainHeader.SuspendLayout();

            // グリッドの初期化
            frmMainSchedule.grdMain.DataSource = null;
            frmMainSchedule.grdMainHeader.DataSource = null;

            // 初期データをセット
            for (int iDay = 0; iDay < frmMainSchedule.piDayCount; iDay++)
            {
                for (int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.piScheduleStaffCount; iScheduleStaff++)
                {
                    frmMainSchedule.astrResultWorkKind[iScheduleStaff, iDay] = "";
                    frmMainSchedule.astrResultChangeFlag[iScheduleStaff, iDay] = "";
                }
            }

            //
            // --- メイングリッドヘッダ ---
            //

            // DataTableを初期化
            dt = new DataTable();

            // DataTableにカラムヘッダを作成
            dt.Columns.Add("NAME", Type.GetType("System.String"));
            dt.Columns.Add("TIMEKIND", Type.GetType("System.String"));
            for (int iDay = 1; iDay <= frmMainSchedule.piDayCount; iDay++)
            {
                dt.Columns.Add(iDay.ToString(), Type.GetType("System.String"));
            }

            // DataTableにデータをセット
            for (int iRow = 0; iRow < 2; iRow++)
            {
                dr = dt.NewRow();

                // 1行目：日にちをセット
                if (iRow == 0)
                {
                    for (int iDay = 1; iDay <= frmMainSchedule.piDayCount; iDay++)
                    {
                        dr[iDay.ToString()] = iDay.ToString();
                    }
                }
                // 2行目："氏名"と曜日をセット
                else
                {
                    dr["NAME"] = "氏名";
                    for (int iDay = 1; iDay <= frmMainSchedule.piDayCount; iDay++)
                    {
                        dr[iDay.ToString()] = clsCommonControl.GetWeekName(frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", iDay), frmMainSchedule.astrHoliday);
                    }
                }
                dt.Rows.Add(dr);
            }

            // グリッドにデータをセット
            frmMainSchedule.grdMainHeader.DataSource = dt;

            // デザイン設定
            for (int iRow = 0; iRow < 2; iRow++)
            {
                // 色・列幅(職員)
                frmMainSchedule.grdMainHeader[0, iRow].Style.ForeColor = Color.Black;
                frmMainSchedule.grdMainHeader[0, iRow].Style.BackColor = SystemColors.Control;
                frmMainSchedule.grdMainHeader.Columns[0].Width = GRID_WIDTH_COLUMN_STAFF;

                // 色・列幅(勤務時間帯)
                frmMainSchedule.grdMainHeader[1, iRow].Style.ForeColor = Color.Black;
                frmMainSchedule.grdMainHeader[1, iRow].Style.BackColor = SystemColors.Control;
                frmMainSchedule.grdMainHeader.Columns[1].Width = GRID_WIDTH_COLUMN_TIMEKIND;

                for (int iColumn = 2; iColumn < frmMainSchedule.grdMainHeader.Columns.Count; iColumn++)
                {
                    // 列幅
                    frmMainSchedule.grdMainHeader.Columns[iColumn].Width = GRID_WIDTH_COLUMN_DATA;

                    // 色(日付、曜日)
                    frmMainSchedule.grdMainHeader[iColumn, iRow].Style.ForeColor = clsCommonControl.GetWeekNameForeColor(frmMainSchedule.grdMainHeader[iColumn, iRow].Value.ToString());
                    frmMainSchedule.grdMainHeader[iColumn, iRow].Style.BackColor = clsCommonControl.GetWeekNameBackgroundColor(clsCommonControl.GetWeekName(frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", iColumn - 1), frmMainSchedule.astrHoliday));
                }
            }

            // 2列目までを固定とする
            frmMainSchedule.grdMainHeader.Columns[0].Frozen = true;
            frmMainSchedule.grdMainHeader.Columns[1].Frozen = true;

            //
            // --- メイングリッドデータ ---
            //

            // DataTableを初期化
            dt = new DataTable();

            // DataTableにカラムヘッダを作成
            dt.Columns.Add("NAME", Type.GetType("System.String"));
            dt.Columns.Add("TIMEKIND", Type.GetType("System.String"));
            for (int iDay = 1; iDay <= frmMainSchedule.piDayCount; iDay++)
            {
                dt.Columns.Add(iDay.ToString(), Type.GetType("System.String"));
            }

            // データをDataTableとして作成
            for (int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.piScheduleStaffCount; iScheduleStaff++)
            {
                // 病棟、職員ID、職種、対象年月から勤務実績データを取得
                dtResultDetail = clsDatabaseControl.GetResultDetail_Ward_Staff_StaffKind_TargetMonth(frmMainSchedule.cmbWard.SelectedValue.ToString(),
                    frmMainSchedule.astrScheduleStaff[iScheduleStaff, 0], frmMainSchedule.pstrStaffKind, frmMainSchedule.pstrTargetMonth);

                // データを初期化
                for (int iDay = 1; iDay <= frmMainSchedule.piDayCount; iDay++)
                {
                    frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 1, 0, 0] = "";
                    frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 1, 0, 1] = "";
                    frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 1, 0, 2] = "";
                    frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 1, 1, 0] = "";
                    frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 1, 1, 1] = "";
                    frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 1, 1, 2] = "";
                    frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 1, 2, 0] = "";
                    frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 1, 2, 1] = "";
                    frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 1, 2, 2] = "";
                }

                // 各職員ごとに3行データセット
                for (int iRow = 0; iRow < 3; iRow++)
                {
                    dr = dt.NewRow();

                    switch (iRow)
                    {
                        case 0:
                            dr["NAME"] = frmMainSchedule.astrScheduleStaff[iScheduleStaff, 1];
                            dr["TIMEKIND"] = "日勤";
                            break;
                        case 1:
                            dr["TIMEKIND"] = "夜勤";
                            break;
                        case 2:
                            dr["TIMEKIND"] = "総夜勤";
                            break;
                    }

                    // 既存データがある場合
                    if (dtResultDetail.Rows.Count != 0)
                    {
                        for (int iDay = 1; iDay <= frmMainSchedule.piDayCount; iDay++)
                        {
                            foreach (DataRow row in dtResultDetail.Rows)
                            {
                                if (DateTime.Parse(row["target_date"].ToString()).ToString("yyyyMMdd") == frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", iDay))
                                {
                                    switch (iRow)
                                    {
                                        case 0:
                                            // その他業務を設定
                                            frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 1, 0, 0] = row["other1_work_kind"].ToString();
                                            frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 1, 0, 1] = row["other1_start_time"].ToString();
                                            frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 1, 0, 2] = row["other1_end_time"].ToString();
                                            frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 1, 1, 0] = row["other2_work_kind"].ToString();
                                            frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 1, 1, 1] = row["other2_start_time"].ToString();
                                            frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 1, 1, 2] = row["other2_end_time"].ToString();
                                            frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 1, 2, 0] = row["other3_work_kind"].ToString();
                                            frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 1, 2, 1] = row["other3_start_time"].ToString();
                                            frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 1, 2, 2] = row["other3_end_time"].ToString();

                                            if (row["work_time_day"].ToString() != "0")
                                                dr[iDay.ToString()] = row["work_time_day"].ToString();
                                            break;
                                        case 1:
                                            if (row["work_time_night"].ToString() != "0")
                                                dr[iDay.ToString()] = row["work_time_night"].ToString();
                                            break;
                                        case 2:
                                            if (row["work_time_night_total"].ToString() != "0")
                                                dr[iDay.ToString()] = row["work_time_night_total"].ToString();
                                            break;
                                    }
                                    frmMainSchedule.astrResultChangeFlag[iScheduleStaff, iDay - 1] = row["change_flag"].ToString();
                                    frmMainSchedule.astrResultWorkKind[iScheduleStaff, iDay - 1] = row["work_kind"].ToString();

                                    break;
                                }
                            }
                        }
                    }

                    dt.Rows.Add(dr);
                }
            }

            // メイングリッドにデータをセット
            frmMainSchedule.grdMain.DataSource = dt;

            // 職員氏名欄のデザイン設定
            for (int i = 0; i < frmMainSchedule.piScheduleStaffCount * 3; i++)
            {
                frmMainSchedule.grdMain[0, i].Style.ForeColor = Color.Black;
                frmMainSchedule.grdMain[0, i].Style.BackColor = SystemColors.Control;
                frmMainSchedule.grdMain.Columns[0].Width = GRID_WIDTH_COLUMN_STAFF;
                frmMainSchedule.grdMain[1, i].Style.ForeColor = Color.Black;
                frmMainSchedule.grdMain[1, i].Style.BackColor = SystemColors.Control;
                frmMainSchedule.grdMain.Columns[1].Width = GRID_WIDTH_COLUMN_TIMEKIND;
            }

            // 勤務種類データのデザイン設定
            for (int iDay = 2; iDay <= frmMainSchedule.piDayCount + 1; iDay++)
            {
                //列幅
                frmMainSchedule.grdMain.Columns[iDay].Width = GRID_WIDTH_COLUMN_DATA;

                // 文字の色
                for (int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.piScheduleStaffCount * 3; iScheduleStaff++)
                {
                    frmMainSchedule.grdMain[iDay, iScheduleStaff].Style.ForeColor = clsCommonControl.GetWorkKindForeColor(frmMainSchedule.grdMain[iDay, iScheduleStaff].Value.ToString());
                    frmMainSchedule.grdMain[iDay, iScheduleStaff].Style.BackColor = clsCommonControl.GetWeekNameBackgroundColor(clsCommonControl.GetWeekName(frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", iDay - 1), frmMainSchedule.astrHoliday));
                }

                // 背景色
                for (int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.piScheduleStaffCount; iScheduleStaff++)
                {
                    SetChangeFlagColor(iDay - 2, iScheduleStaff);
                }
            }

            // 枠線の設定
            for (int iRow = 0; iRow < frmMainSchedule.grdMain.RowCount; iRow++)
            {
                if (iRow % 3 == 2)
                {
                    frmMainSchedule.grdMain.Rows[iRow].DividerHeight = 2;
                }
            }

            // 2列目までを固定とする
            frmMainSchedule.grdMain.Columns[0].Frozen = true;
            frmMainSchedule.grdMain.Columns[1].Frozen = true;

            // 行の合計グリッドをセット
            SetResultRowTotal();

            // 列の合計グリッドをセット
            SetResultColumnTotal();

            // グリッドの選択状態を解除
            frmMainSchedule.grdMain.CurrentCell = null;
            frmMainSchedule.grdMainHeader.CurrentCell = null;

            //グリッドの描画再開
            frmMainSchedule.grdMain.ResumeLayout();
            frmMainSchedule.grdMainHeader.ResumeLayout();
        }

        /// <summary>
        /// 行の合計をグリッドにセット(実績データ用)
        /// </summary>
        public void SetResultRowTotal()
        {
            // データテーブル作成
            DataTable dt = new DataTable();
            DataRow dr;
            double dRowTotalTemp;

            // グリッドの描画処理停止
            frmMainSchedule.grdRowTotal.SuspendLayout();
            frmMainSchedule.grdRowTotalHeader.SuspendLayout();
            frmMainSchedule.grdRowTotal.DataSource = null;
            frmMainSchedule.grdRowTotalHeader.DataSource = null;
            
            //
            // --- 行合計ヘッダ---
            //

            // DataTableを初期化
            dt = new DataTable();

            // データテーブルのカラムヘッダを作成
            dt.Columns.Add("TOTAL", Type.GetType("System.String"));

            // 勤務種類データをセット
            for (int iRow = 0; iRow < 2; iRow++)
            {
                dr = dt.NewRow();
                // 1行目
                if (iRow == 0)
                {
                    dr["TOTAL"] = "合";
                }
                // 2行目
                else
                {
                    dr["TOTAL"] = "計";
                }
                dt.Rows.Add(dr);
            }

            // 行合計ヘッダにデータをセット
            frmMainSchedule.grdRowTotalHeader.DataSource = dt;

            //列幅
            frmMainSchedule.grdRowTotalHeader.Columns["TOTAL"].Width = 62;

            ///
            /// --- 行合計データ ---
            ///

            // DataTableを初期化
            dt = new DataTable();

            // データテーブルのカラムヘッダを作成
            dt.Columns.Add("TOTAL", Type.GetType("System.String"));

            // 初期データをセット
            for (int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.piScheduleStaffCount; iScheduleStaff++)
            {
                for (int iTimeKind = 0; iTimeKind < 3; iTimeKind++)
                {
                    dr = dt.NewRow();
                    dRowTotalTemp = 0;
                    for (int iDay = 2; iDay < frmMainSchedule.piDayCount + 2; iDay++)
                    {
                        switch(iTimeKind)
                        {
                            case 0:
                                if (frmMainSchedule.grdMain[iDay, (iScheduleStaff + 1) * 3 - 3].Value.ToString() != "")
                                    dRowTotalTemp += double.Parse(frmMainSchedule.grdMain[iDay, (iScheduleStaff + 1) * 3 - 3].Value.ToString());
                                break;
                            case 1:
                                if (frmMainSchedule.grdMain[iDay, (iScheduleStaff + 1) * 3 - 2].Value.ToString() != "")
                                    dRowTotalTemp += double.Parse(frmMainSchedule.grdMain[iDay, (iScheduleStaff + 1) * 3 - 2].Value.ToString());
                                break;
                            case 2:
                                if (frmMainSchedule.grdMain[iDay, (iScheduleStaff + 1) * 3 - 1].Value.ToString() != "")
                                    dRowTotalTemp += double.Parse(frmMainSchedule.grdMain[iDay, (iScheduleStaff + 1) * 3 - 1].Value.ToString());
                                break;
                        }
                    }
                    dr["TOTAL"] = string.Format("{0:f2}", dRowTotalTemp);
                    dt.Rows.Add(dr);
                }
            }

            // グリッドにデータテーブルをセット
            frmMainSchedule.grdRowTotal.DataSource = dt;

            // グリッドの列幅
            frmMainSchedule.grdRowTotal.Columns[0].Width = 62;

            // グリッドのソートモード無効
            frmMainSchedule.grdRowTotal.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;

            // グリッドの選択状態を解除
            frmMainSchedule.grdRowTotalHeader.CurrentCell = null;
            frmMainSchedule.grdRowTotal.CurrentCell = null;

            // グリッドの描画処理停止
            frmMainSchedule.grdRowTotalHeader.ResumeLayout();
            frmMainSchedule.grdRowTotal.ResumeLayout();

        }

        /// <summary>
        /// 列の合計をグリッドにセット(実績データ用)
        /// </summary>
        public void SetResultColumnTotal()
        {
            double dColumnTotalTemp;
            // Add Start WataruT 2020.09.24 実績画面にひと月の合計時間数を表示
            double[] dResultTotalTime = new double[3];
            // Add End   WataruT 2020.09.24 実績画面にひと月の合計時間数を表示

            // データテーブル作成
            DataTable dt = new DataTable();

            // グリッドの描画処理停止
            frmMainSchedule.grdColumnTotal.SuspendLayout();
            frmMainSchedule.grdColumnTotal.DataSource = null;

            // データテーブルのカラムヘッダを作成
            dt.Columns.Add("NAME", Type.GetType("System.String"));
            dt.Columns.Add("TIMEKIND", Type.GetType("System.String"));
            for (int i = 1; i <= frmMainSchedule.piDayCount + 1; i++)
            {
                dt.Columns.Add("DAY" + i.ToString(), Type.GetType("System.String"));
            }

            // 初期データをセット
            for (int iTimeKind = 0; iTimeKind < 3; iTimeKind++)
            {
                DataRow dr = dt.NewRow();
                switch (iTimeKind)
                {
                    case 0:
                        dr["TIMEKIND"] = "日勤";
                        break;
                    case 1:
                        dr["TIMEKIND"] = "夜勤";
                        break;
                    case 2:
                        dr["TIMEKIND"] = "総夜勤";
                        break;
                }

                for (int iDay = 1; iDay <= frmMainSchedule.piDayCount; iDay++)
                {
                    dColumnTotalTemp = 0;
                    for (int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.piScheduleStaffCount; iScheduleStaff++)
                    {
                        switch(iTimeKind)
                        {
                            case 0:
                                // Mod Start WataruT 2020.09.24 実績画面にひと月の合計時間数を表示
                                //if (frmMainSchedule.grdMain[iDay + 1, (iScheduleStaff + 1) * 3 - 3].Value.ToString() != "")
                                //    dColumnTotalTemp += double.Parse(frmMainSchedule.grdMain[iDay + 1, (iScheduleStaff + 1) * 3 - 3].Value.ToString());
                                if (frmMainSchedule.grdMain[iDay + 1, (iScheduleStaff + 1) * 3 - 3].Value.ToString() != "")
                                {
                                    dColumnTotalTemp += double.Parse(frmMainSchedule.grdMain[iDay + 1, (iScheduleStaff + 1) * 3 - 3].Value.ToString());
                                    dResultTotalTime[iTimeKind] += double.Parse(frmMainSchedule.grdMain[iDay + 1, (iScheduleStaff + 1) * 3 - 3].Value.ToString());
                                }   
                                // Mod End   WataruT 2020.09.24 実績画面にひと月の合計時間数を表示
                                break;
                            case 1:
                                // Mod Start WataruT 2020.09.24 実績画面にひと月の合計時間数を表示
                                //if (frmMainSchedule.grdMain[iDay + 1, (iScheduleStaff + 1) * 3 - 2].Value.ToString() != "")
                                //    dColumnTotalTemp += double.Parse(frmMainSchedule.grdMain[iDay + 1, (iScheduleStaff + 1) * 3 - 2].Value.ToString());
                                //break;
                                if (frmMainSchedule.grdMain[iDay + 1, (iScheduleStaff + 1) * 3 - 2].Value.ToString() != "")
                                {
                                    dColumnTotalTemp += double.Parse(frmMainSchedule.grdMain[iDay + 1, (iScheduleStaff + 1) * 3 - 2].Value.ToString());
                                    dResultTotalTime[iTimeKind] += double.Parse(frmMainSchedule.grdMain[iDay + 1, (iScheduleStaff + 1) * 3 - 2].Value.ToString());
                                }   
                                // Mod End   WataruT 2020.09.24 実績画面にひと月の合計時間数を表示
                                break;
                            case 2:
                                // Mod Start WataruT 2020.09.24 実績画面にひと月の合計時間数を表示
                                //if (frmMainSchedule.grdMain[iDay + 1, (iScheduleStaff + 1) * 3 - 1].Value.ToString() != "")
                                //    dColumnTotalTemp += double.Parse(frmMainSchedule.grdMain[iDay + 1, (iScheduleStaff + 1) * 3 - 1].Value.ToString());
                                if (frmMainSchedule.grdMain[iDay + 1, (iScheduleStaff + 1) * 3 - 1].Value.ToString() != "")
                                {
                                    dColumnTotalTemp += double.Parse(frmMainSchedule.grdMain[iDay + 1, (iScheduleStaff + 1) * 3 - 1].Value.ToString());
                                    dResultTotalTime[iTimeKind] += double.Parse(frmMainSchedule.grdMain[iDay + 1, (iScheduleStaff + 1) * 3 - 1].Value.ToString());
                                }   
                                // Mod End   WataruT 2020.09.24 実績画面にひと月の合計時間数を表示
                                break;
                        }
                    }
                    dr["DAY" + iDay.ToString()] = string.Format("{0:f2}", dColumnTotalTemp);
                }
                dt.Rows.Add(dr);
            }

            // グリッドにデータテーブルをセット
            frmMainSchedule.grdColumnTotal.DataSource = dt;

            // 列幅(職員・勤務時間帯)
            frmMainSchedule.grdColumnTotal.Columns[0].Width = GRID_WIDTH_COLUMN_STAFF;
            frmMainSchedule.grdColumnTotal.Columns[1].Width = GRID_WIDTH_COLUMN_TIMEKIND;

            // ソートモード無効化(職員・勤務時間帯)
            frmMainSchedule.grdColumnTotal.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            frmMainSchedule.grdColumnTotal.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;

            // グリッドのオプション設定
            for (int i = 2; i < frmMainSchedule.piDayCount + 2; i++)
            {
                //列幅
                frmMainSchedule.grdColumnTotal.Columns[i].Width = GRID_WIDTH_COLUMN_DATA;
                //ソートモード
                frmMainSchedule.grdColumnTotal.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // フォント変更
            for (int iColumn = 0; iColumn < frmMainSchedule.grdColumnTotal.Columns.Count; iColumn++)
            {
                frmMainSchedule.grdColumnTotal.Columns[iColumn].DefaultCellStyle.Font = new Font("メイリオ", 9);
            }

            frmMainSchedule.grdColumnTotal.Columns[0].Frozen = true;
            frmMainSchedule.grdColumnTotal.Columns[1].Frozen = true;
            frmMainSchedule.grdColumnTotal.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;

            // Add Start WataruT 2020.09.24 実績画面にひと月の合計時間数を表示
            frmMainSchedule.lblResultDayTotalTime.Text = dResultTotalTime[0].ToString();
            frmMainSchedule.lblResultNightTotalTime.Text = dResultTotalTime[1].ToString();
            frmMainSchedule.lblResultAllNightTotalTime.Text = dResultTotalTime[2].ToString();
            // Add End   WataruT 2020.09.24 実績画面にひと月の合計時間数を表示
        }

        /// <summary>
        /// 予定データを取込む
        /// </summary>
        public void ImportScheduleData()
        {
            DataTable dtScheduleData;
            int iTargetColumn = frmMainSchedule.piGrdMain_CurrentColumn;

            // 確認メッセージの表示
            if (MessageBox.Show("対象日の予定データを実績画面に取り込みますか？\n\n" + "対象日　：　" + (iTargetColumn - 1).ToString() + "日",
                "", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            // グリッドの既存データをクリア
            ClearResultDataFromMainGridSelectColumn(iTargetColumn);

            // 予定データをデータテーブルに取得
            dtScheduleData = clsDatabaseControl.GetScheduleDetail_Ward_TargetMonth_StaffKind_TargetDate(
                frmMainSchedule.cmbWard.SelectedValue.ToString(),
                frmMainSchedule.pstrTargetMonth,
                frmMainSchedule.pstrStaffKind,
                frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", (iTargetColumn - 1)));

            // 予定データを1行ずつ確認
            foreach (DataRow row in dtScheduleData.Rows)
            {
                // 対象職員かチェック
                for (int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.astrScheduleStaff.GetLength(0); iScheduleStaff++)
                {
                    // 対象職員の場合
                    if(row["staff"].ToString() == frmMainSchedule.astrScheduleStaff[iScheduleStaff, 0])
                    {
                        // Mod Start WataruT 2020.07.30 遅出の表示対応
                        //// 夜勤または夜明の場合
                        //if (row["work_kind"].ToString() == "02" || row["work_kind"].ToString() == "03")
                        // 遅出の場合
                        if (row["work_kind"].ToString() == "11")
                        {
                            // 日勤データ
                            frmMainSchedule.grdMain[iTargetColumn, (iScheduleStaff + 1) * 3 - 3].Value = "2.00";
                            CheckWorkKindForRowTotalData(iScheduleStaff, 0, 2);
                            CheckWorkKindForColumnTotalData(iTargetColumn - 1, 0, 2);
                            // 夜勤データ
                            frmMainSchedule.grdMain[iTargetColumn, (iScheduleStaff + 1) * 3 - 2].Value = "6.00";
                            CheckWorkKindForRowTotalData(iScheduleStaff, 1, 6);
                            CheckWorkKindForColumnTotalData(iTargetColumn - 1, 1, 6);
                            // 総夜勤データ
                            frmMainSchedule.grdMain[iTargetColumn, (iScheduleStaff + 1) * 3 - 1].Value = "6.00";
                            CheckWorkKindForRowTotalData(iScheduleStaff, 2, 6);
                            CheckWorkKindForColumnTotalData(iTargetColumn - 1, 2, 6);
                        }
                        // 夜勤または夜明の場合
                        else if(row["work_kind"].ToString() == "02" || row["work_kind"].ToString() == "03")
                        // Mod End   WataruT 2020.07.30 遅出の表示対応
                        {
                            // 夜勤データ
                            if (frmMainSchedule.grdMain[iTargetColumn, (iScheduleStaff + 1) * 3 - 2].Value.ToString() != "")
                            {
                                CheckWorkKindForRowTotalData(iScheduleStaff, 1, -1 * double.Parse(frmMainSchedule.grdMain[iTargetColumn, (iScheduleStaff + 1) * 3 - 2].Value.ToString()));
                                CheckWorkKindForColumnTotalData(iTargetColumn - 1, 1, -1 * double.Parse(frmMainSchedule.grdMain[iTargetColumn, (iScheduleStaff + 1) * 3 - 2].Value.ToString()));
                            }
                            frmMainSchedule.grdMain[iTargetColumn, (iScheduleStaff + 1) * 3 - 2].Value = string.Format("{0:f2}", double.Parse(row["work_time"].ToString()));
                            CheckWorkKindForRowTotalData(iScheduleStaff, 1, double.Parse(row["work_time"].ToString()));
                            CheckWorkKindForColumnTotalData(iTargetColumn - 1, 1,double.Parse(row["work_time"].ToString()));

                            // 総夜勤データ
                            frmMainSchedule.grdMain[iTargetColumn, (iScheduleStaff + 1) * 3 - 1].Value = string.Format("{0:f2}", double.Parse(row["work_time"].ToString()));
                            CheckWorkKindForRowTotalData(iScheduleStaff, 2, double.Parse(row["work_time"].ToString()));
                            CheckWorkKindForColumnTotalData(iTargetColumn - 1, 2, double.Parse(row["work_time"].ToString()));
                        }
                        // それ以外の場合
                        else
                        {
                            // 日勤データ
                            frmMainSchedule.grdMain[iTargetColumn, (iScheduleStaff + 1) * 3 - 3].Value = string.Format("{0:f2}", double.Parse(row["work_time"].ToString()));
                            CheckWorkKindForRowTotalData(iScheduleStaff, 0, double.Parse(row["work_time"].ToString()));
                            CheckWorkKindForColumnTotalData(iTargetColumn - 1, 0, double.Parse(row["work_time"].ToString()));
                        }
                        // 勤務種類ID
                        frmMainSchedule.astrResultWorkKind[iScheduleStaff, iTargetColumn - 2] = row["work_kind"].ToString();
                        frmMainSchedule.astrResultChangeFlag[iScheduleStaff, iTargetColumn - 2] = "0";

                        // 色の変更
                        SetChangeFlagColor(iTargetColumn - 2, iScheduleStaff);

                        // 次の予定データに進む
                        break;
                    }
                }
            }

            // 夜勤・夜明の人数調整
            CheckAndChangeNightTime(iTargetColumn);

            // 行の合計をグリッドにセット
            SetResultRowTotal();

            // 列の合計をグリッドにセット
            SetResultColumnTotal();

            // 完了メッセージの表示
            MessageBox.Show("データ引用が完了しました。","",MessageBoxButtons.OK);
        }

        /// <summary>
        /// 行合計用のデータ追加
        /// </summary>
        private void CheckWorkKindForRowTotalData(int iScheduleStaff, int iTimeKind, double dAddNumber)
        {
            double dRowTotalTemp = 0;

            switch(iTimeKind)
            {
                case 0: // 日勤
                    dRowTotalTemp = double.Parse(frmMainSchedule.grdRowTotal[0, (iScheduleStaff + 1) * 3 - 3].Value.ToString()) + dAddNumber;
                    frmMainSchedule.grdRowTotal[0, (iScheduleStaff + 1) * 3 - 3].Value = string.Format("{0:f2}", dRowTotalTemp);
                    break;
                case 1: // 夜勤
                    dRowTotalTemp = double.Parse(frmMainSchedule.grdRowTotal[0, (iScheduleStaff + 1) * 3 - 2].Value.ToString()) + dAddNumber;
                    frmMainSchedule.grdRowTotal[0, (iScheduleStaff + 1) * 3 - 2].Value = string.Format("{0:f2}", dRowTotalTemp);
                    break;
                case 2: // 総夜勤
                    dRowTotalTemp = double.Parse(frmMainSchedule.grdRowTotal[0, (iScheduleStaff + 1) * 3 - 1].Value.ToString()) + dAddNumber;
                    frmMainSchedule.grdRowTotal[0, (iScheduleStaff + 1) * 3 - 1].Value = string.Format("{0:f2}", dRowTotalTemp);
                    break;
            }
        }

        /// <summary>
        /// 列合計用のデータ追加
        /// </summary>
        private void CheckWorkKindForColumnTotalData(int iDay, int iTimeKind, double dAddNumber)
        {
            double dColumnTotalTemp = 0;

            switch (iTimeKind)
            {
                case 0: // 日勤
                    dColumnTotalTemp = double.Parse(frmMainSchedule.grdColumnTotal[iDay + 1, 0].Value.ToString()) + dAddNumber;
                    frmMainSchedule.grdColumnTotal[iDay + 1, 0].Value = string.Format("{0:f2}", dColumnTotalTemp);
                    break;
                case 1: // 夜勤
                    dColumnTotalTemp = double.Parse(frmMainSchedule.grdColumnTotal[iDay + 1, 1].Value.ToString()) + dAddNumber;
                    frmMainSchedule.grdColumnTotal[iDay + 1, 1].Value = string.Format("{0:f2}", dColumnTotalTemp);
                    break;
                case 2: // 総夜勤
                    dColumnTotalTemp = double.Parse(frmMainSchedule.grdColumnTotal[iDay + 1, 2].Value.ToString()) + dAddNumber;
                    frmMainSchedule.grdColumnTotal[iDay + 1, 2].Value = string.Format("{0:f2}", dColumnTotalTemp);
                    break;
            }
        }

        /// <summary>
        /// 実績データの削除処理
        /// </summary>
        public void DeleteResultData()
        {
            string strTargetMonthForHeader = frmMainSchedule.lblTargetMonth.Text.Substring(0, 4) + frmMainSchedule.lblTargetMonth.Text.Substring(5, 2);
            string strResultNo = clsDatabaseControl.GetResultHeader_TargetResultNo(frmMainSchedule.cmbWard.SelectedValue.ToString(), strTargetMonthForHeader, frmMainSchedule.pstrStaffKind);

            clsDatabaseControl.DeleteResultHeader_ResultNo(strResultNo);
            clsDatabaseControl.DeleteResultDetail_ResultNo(strResultNo);
        }

        /// <summary>
        /// 実績データの保存処理
        /// </summary>
        public void SaveResultData()
        {
            DataTable dtResultHeader;
            DataTable dtResultDetail;
            DataRow drResultHeader;
            DataRow drResultDetail;

            string strTargetMonthForHeader = frmMainSchedule.lblTargetMonth.Text.Substring(0, 4) + frmMainSchedule.lblTargetMonth.Text.Substring(5, 2);
            string strSQL;

            int piScheduleStaffCount = frmMainSchedule.astrScheduleStaff.GetLength(0);
            int piDayCount = frmMainSchedule.grdMain.ColumnCount - 2;
            int iResultNo;
            int iResultDetailNo = 1;

            // 勤務予定データの最大値を取得
            if (int.TryParse(clsDatabaseControl.GetResultHeader_MaxResultNo(), out iResultNo) == false)
            {
                iResultNo = 1;
            }
            else
                iResultNo++;

            // 勤務予定ヘッダの作成
            dtResultHeader = clsDataTableControl.GetTable_ResultHeader();
            drResultHeader = dtResultHeader.NewRow();

            drResultHeader["result_no"] = iResultNo.ToString();
            drResultHeader["ward"] = frmMainSchedule.cmbWard.SelectedValue.ToString();
            drResultHeader["staff_kind"] = frmMainSchedule.pstrStaffKind;
            drResultHeader["target_month"] = strTargetMonthForHeader;

            clsDatabaseControl.InsertResultHeader(drResultHeader);

            // 勤務予定詳細の作成
            strSQL = clsDatabaseControl.GetInsertResultDetail_Insert();

            for (int iScheduleStaff = 0; iScheduleStaff < piScheduleStaffCount; iScheduleStaff++)
            {
                for (int iDay = 1; iDay <= piDayCount; iDay++)
                {
                    if(frmMainSchedule.astrResultWorkKind[iScheduleStaff, iDay - 1].ToString() != "")
                    {
                        dtResultDetail = clsDataTableControl.GetTable_ResultDetail();
                        drResultDetail = dtResultDetail.NewRow();

                        drResultDetail["result_no"] = iResultNo.ToString();
                        drResultDetail["result_detail_no"] = iResultDetailNo;
                        drResultDetail["staff"] = frmMainSchedule.astrScheduleStaff[iScheduleStaff, 0];
                        drResultDetail["target_date"] = clsCommonControl.GetTargetDateChangeFormat(strTargetMonthForHeader + String.Format("{0:D2}", iDay));
                        drResultDetail["work_kind"] = frmMainSchedule.astrResultWorkKind[iScheduleStaff, iDay - 1].ToString();
                        drResultDetail["work_time_day"] = frmMainSchedule.grdMain[iDay + 1, (iScheduleStaff + 1) * 3 - 3].Value.ToString();
                        drResultDetail["work_time_night"] = frmMainSchedule.grdMain[iDay + 1, (iScheduleStaff + 1) * 3 - 2].Value.ToString();
                        drResultDetail["work_time_night_total"] = frmMainSchedule.grdMain[iDay + 1, (iScheduleStaff + 1) * 3 - 1].Value.ToString();
                        drResultDetail["change_flag"] = frmMainSchedule.astrResultChangeFlag[iScheduleStaff, iDay - 1].ToString();
                        drResultDetail["other1_work_kind"] = frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 1, 0, 0];
                        drResultDetail["other1_start_time"] = frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 1, 0, 1];
                        drResultDetail["other1_end_time"] = frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 1, 0, 2];
                        drResultDetail["other2_work_kind"] = frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 1, 1, 0];
                        drResultDetail["other2_start_time"] = frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 1, 1, 1];
                        drResultDetail["other2_end_time"] = frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 1, 1, 2];
                        drResultDetail["other3_work_kind"] = frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 1, 2, 0];
                        drResultDetail["other3_start_time"] = frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 1, 2, 1];
                        drResultDetail["other3_end_time"] = frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 1, 2, 2];

                        strSQL += clsDatabaseControl.GetInsertResultDetail_Values(drResultDetail);

                        iResultDetailNo++;
                    }
                }
            }

            if (clsDatabaseControl.ExecuteBulkInsertSQL(strSQL) == false)
                MessageBox.Show("勤務予定詳細の登録に失敗しました。");
        }

        /// <summary>
        /// 実績データ入力画面を表示
        /// </summary>
        public void ShowEditResultData()
        {
            EditResultData frmEditResultData = new EditResultData(frmMainSchedule);
            frmEditResultData.ShowDialog();

            // 変更フラグによる背景色をセット
            SetChangeFlagColor(frmMainSchedule.piGrdMain_CurrentColumn - 2, int.Parse(Math.Floor(double.Parse((frmMainSchedule.piGrdMain_CurrentRow / 3).ToString())).ToString()));

            // 夜勤・夜明の人数調整
            CheckAndChangeNightTime(frmMainSchedule.piGrdMain_CurrentColumn);

            // 行・列の合計値をセット
            SetResultRowTotal();
            SetResultColumnTotal();
        }

        /// <summary>
        /// メイングリッドの実績データをクリア
        /// </summary>
        public void ClearResultDataFromMainGrid()
        {
            int iScheduleStaff = int.Parse(clsCommonControl.ToRoundDown(double.Parse(((frmMainSchedule.piGrdMain_CurrentRow + 3) / 3).ToString()), 0).ToString()) - 1;
            int iDay = frmMainSchedule.piGrdMain_CurrentColumn - 2;

            // グリッドのデータクリア
            frmMainSchedule.grdMain[iDay + 2, (iScheduleStaff + 1) * 3 - 3].Value = "";
            frmMainSchedule.grdMain[iDay + 2, (iScheduleStaff + 1) * 3 - 2].Value = "";
            frmMainSchedule.grdMain[iDay + 2, (iScheduleStaff + 1) * 3 - 1].Value = "";
            frmMainSchedule.grdMain[iDay + 2, (iScheduleStaff + 1) * 3 - 3].Style.BackColor = Color.White;
            frmMainSchedule.grdMain[iDay + 2, (iScheduleStaff + 1) * 3 - 2].Style.BackColor = Color.White;
            frmMainSchedule.grdMain[iDay + 2, (iScheduleStaff + 1) * 3 - 1].Style.BackColor = Color.White;

            // 実績データの変数の値クリア
            frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay, 0, 0] = "";
            frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay, 0, 1] = "";
            frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay, 0, 2] = "";
            frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay, 1, 0] = "";
            frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay, 1, 1] = "";
            frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay, 1, 2] = "";
            frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay, 2, 0] = "";
            frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay, 2, 1] = "";
            frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay, 2, 2] = "";
            frmMainSchedule.astrResultChangeFlag[iScheduleStaff, iDay] = "";
            frmMainSchedule.astrResultWorkKind[iScheduleStaff, iDay] = "";

        }

        /// <summary>
        /// メイングリッドの指定列の実績データをクリア
        /// </summary>
        public void ClearResultDataFromMainGridSelectColumn(int iDay)
        {
            for(int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.piScheduleStaffCount; iScheduleStaff++)
            {
                // グリッドのデータクリア
                frmMainSchedule.grdMain[iDay, (iScheduleStaff + 1) * 3 - 3].Value = "";
                frmMainSchedule.grdMain[iDay, (iScheduleStaff + 1) * 3 - 2].Value = "";
                frmMainSchedule.grdMain[iDay, (iScheduleStaff + 1) * 3 - 1].Value = "";
                frmMainSchedule.grdMain[iDay, (iScheduleStaff + 1) * 3 - 3].Style.BackColor = Color.White;
                frmMainSchedule.grdMain[iDay, (iScheduleStaff + 1) * 3 - 2].Style.BackColor = Color.White;
                frmMainSchedule.grdMain[iDay, (iScheduleStaff + 1) * 3 - 1].Style.BackColor = Color.White;

                // 実績データの変数の値クリア
                frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 2, 0, 0] = "";
                frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 2, 0, 1] = "";
                frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 2, 0, 2] = "";
                frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 2, 1, 0] = "";
                frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 2, 1, 1] = "";
                frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 2, 1, 2] = "";
                frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 2, 2, 0] = "";
                frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 2, 2, 1] = "";
                frmMainSchedule.astrResultOtherWorkTime[iScheduleStaff, iDay - 2, 2, 2] = "";
                frmMainSchedule.astrResultChangeFlag[iScheduleStaff, iDay - 2] = "";
                frmMainSchedule.astrResultWorkKind[iScheduleStaff, iDay - 2] = "";
            }
        }

        /// <summary>
        /// 変更フラグによる背景色をセット
        /// </summary>
        private void SetChangeFlagColor(int iTargetDay, int iScheduleStaff)
        {
            switch(frmMainSchedule.astrResultChangeFlag[iScheduleStaff, iTargetDay])
            {
                case "0":
                    frmMainSchedule.grdMain[iTargetDay + 2, (iScheduleStaff + 1) * 3 - 3].Style.BackColor = Color.PaleGreen;
                    frmMainSchedule.grdMain[iTargetDay + 2, (iScheduleStaff + 1) * 3 - 2].Style.BackColor = Color.PaleGreen;
                    frmMainSchedule.grdMain[iTargetDay + 2, (iScheduleStaff + 1) * 3 - 1].Style.BackColor = Color.PaleGreen;
                    break;
                case "1":
                    frmMainSchedule.grdMain[iTargetDay + 2, (iScheduleStaff + 1) * 3 - 3].Style.BackColor = Color.PaleGoldenrod;
                    frmMainSchedule.grdMain[iTargetDay + 2, (iScheduleStaff + 1) * 3 - 2].Style.BackColor = Color.PaleGoldenrod;
                    frmMainSchedule.grdMain[iTargetDay + 2, (iScheduleStaff + 1) * 3 - 1].Style.BackColor = Color.PaleGoldenrod;
                    break;
                default:
                    frmMainSchedule.grdMain[iTargetDay + 2, (iScheduleStaff + 1) * 3 - 3].Style.BackColor = Color.White;
                    frmMainSchedule.grdMain[iTargetDay + 2, (iScheduleStaff + 1) * 3 - 2].Style.BackColor = Color.White;
                    frmMainSchedule.grdMain[iTargetDay + 2, (iScheduleStaff + 1) * 3 - 1].Style.BackColor = Color.White;
                    break;
            }
        }

        /// <summary>
        /// 夜勤および夜明の2人以降は総夜勤のみとする
        /// </summary>
        /// <param name="iTargetColumn"></param>
        private void CheckAndChangeNightTime(int iTargetColumn)
        {
            int iNightCount = 0;
            int iNightAfterCount = 0;

            for (int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.piScheduleStaffCount; iScheduleStaff++)
            {
                // 夜勤の場合
                if(frmMainSchedule.astrResultWorkKind[iScheduleStaff, iTargetColumn - 2] == "02")
                {
                    // Mod Start WataruT 2020.09.11 夜勤と総夜勤の時間反映処理変更
                    //if (iNightCount < 2)
                    //{
                    //    iNightCount++;
                    //    frmMainSchedule.grdMain[iTargetColumn, (iScheduleStaff + 1) * 3 - 2].Value =
                    //        frmMainSchedule.grdMain[iTargetColumn, (iScheduleStaff + 1) * 3 - 1].Value;
                    //}   
                    //else
                    //    frmMainSchedule.grdMain[iTargetColumn, (iScheduleStaff + 1) * 3 - 2].Value = "";
                    if (iNightCount == 2 && frmMainSchedule.cmbWard.SelectedValue.ToString() == "06")
                    {
                        iNightCount++;
                        frmMainSchedule.grdMain[iTargetColumn, (iScheduleStaff + 1) * 3 - 2].Value = "";
                    }
                    else
                    {
                        iNightCount++;
                        frmMainSchedule.grdMain[iTargetColumn, (iScheduleStaff + 1) * 3 - 2].Value =
                            frmMainSchedule.grdMain[iTargetColumn, (iScheduleStaff + 1) * 3 - 1].Value;
                    }
                    // Mod End   WataruT 2020.09.11 夜勤と総夜勤の時間反映処理変更
                }
                // 夜明の場合
                else if(frmMainSchedule.astrResultWorkKind[iScheduleStaff, iTargetColumn - 2] == "03")
                {
                    // Mod Start WataruT 2020.09.11 夜勤と総夜勤の時間反映処理変更
                    //if (iNightAfterCount < 2)
                    //{
                    //    iNightAfterCount++;
                    //    frmMainSchedule.grdMain[iTargetColumn, (iScheduleStaff + 1) * 3 - 2].Value =
                    //        frmMainSchedule.grdMain[iTargetColumn, (iScheduleStaff + 1) * 3 - 1].Value;
                    //}   
                    //else
                    //    frmMainSchedule.grdMain[iTargetColumn, (iScheduleStaff + 1) * 3 - 2].Value = "";
                    if (iNightAfterCount == 2 && frmMainSchedule.cmbWard.SelectedValue.ToString() == "06")
                    {
                        iNightAfterCount++;
                        frmMainSchedule.grdMain[iTargetColumn, (iScheduleStaff + 1) * 3 - 2].Value = "";
                    }
                    else
                    {
                        iNightAfterCount++;
                        frmMainSchedule.grdMain[iTargetColumn, (iScheduleStaff + 1) * 3 - 2].Value =
                            frmMainSchedule.grdMain[iTargetColumn, (iScheduleStaff + 1) * 3 - 1].Value;
                    }
                    // Mod End  WataruT 2020.09.11 夜勤と総夜勤の時間反映処理変更
                }
            }
        }

        /// <summary>
        /// ツールチップに表示する情報を返す
        /// </summary>
        public string GetToolTipText(int iColumn, int iRow)
        {
            int iScheduleStaffColumn;

            string strToolTipText;

            iScheduleStaffColumn = int.Parse(clsCommonControl.ToRoundDown(double.Parse(((iRow + 3) / 3).ToString()), 0).ToString()) - 1;

            // 1行目：「職員：職員氏名」
            strToolTipText = "職員：" + frmMainSchedule.astrScheduleStaff[iScheduleStaffColumn, 1];
            
            // 2行目：「対象年月：MM月DD日(G)」
            strToolTipText = strToolTipText + "\r\n";
            // Mod Start WataruT 2020.09.02 実績画面のツールチップの値不具合対応
            //strToolTipText = strToolTipText + "対象年月：" + frmMainSchedule.lblTargetMonth.Text + string.Format("{0:D2}", iColumn - 2) + 
            //    "日(" + frmMainSchedule.grdMainHeader[iColumn, 1].Value.ToString() + ")";
            strToolTipText = strToolTipText + "対象年月：" + frmMainSchedule.lblTargetMonth.Text + string.Format("{0:D2}", iColumn - 1) +
                "日(" + frmMainSchedule.grdMainHeader[iColumn, 1].Value.ToString() + ")";
            // Mod End   WataruT 2020.09.02 実績画面のツールチップの値不具合対応

            // 3行目：「勤務種類：〇〇」
            strToolTipText = strToolTipText + "\r\n";
            for(int iWorkKind = 0; iWorkKind < frmMainSchedule.astrWorkKind.GetLength(0); iWorkKind++)
            {
                if(frmMainSchedule.astrResultWorkKind[iScheduleStaffColumn, iColumn - 2] == frmMainSchedule.astrWorkKind[iWorkKind, 0])
                {
                    strToolTipText = strToolTipText + "勤務種類：" + frmMainSchedule.astrWorkKind[iWorkKind, 1];
                    break;
                }
            }
            // 4行目：「その他業務1：〇〇(HH:MM～HH:MM)」
            if (frmMainSchedule.astrResultOtherWorkTime[iScheduleStaffColumn, iColumn - 2, 0, 0].ToString() != "")
            {
                strToolTipText = strToolTipText + "\r\n";
                strToolTipText = strToolTipText + "その他業務１：" + frmMainSchedule.astrResultOtherWorkTime[iScheduleStaffColumn, iColumn - 2, 0, 0].ToString() +
                    " (" + frmMainSchedule.astrResultOtherWorkTime[iScheduleStaffColumn, iColumn - 2, 0, 1].ToString() + " ～ " +
                    frmMainSchedule.astrResultOtherWorkTime[iScheduleStaffColumn, iColumn - 2, 0, 2].ToString() + ")";
            }
            
            // 5行目：「その他業務2：〇〇(HH:MM～HH:MM)」
            if (frmMainSchedule.astrResultOtherWorkTime[iScheduleStaffColumn, iColumn - 2, 1, 0].ToString() != "")
            {
                strToolTipText = strToolTipText + "\r\n";
                strToolTipText = strToolTipText + "その他業務２：" + frmMainSchedule.astrResultOtherWorkTime[iScheduleStaffColumn, iColumn - 2, 1, 0].ToString() +
                    " (" + frmMainSchedule.astrResultOtherWorkTime[iScheduleStaffColumn, iColumn - 2, 1, 1].ToString() + " ～ " +
                    frmMainSchedule.astrResultOtherWorkTime[iScheduleStaffColumn, iColumn - 2, 1, 2].ToString() + ")";
            }
            
            // 6行目：「その他業務3：〇〇(HH:MM～HH:MM)」
            if (frmMainSchedule.astrResultOtherWorkTime[iScheduleStaffColumn, iColumn - 2, 2, 0].ToString() != "")
            {
                strToolTipText = strToolTipText + "\r\n";
                strToolTipText = strToolTipText + "その他業務３：" + frmMainSchedule.astrResultOtherWorkTime[iScheduleStaffColumn, iColumn - 2, 2, 0].ToString() +
                    " (" + frmMainSchedule.astrResultOtherWorkTime[iScheduleStaffColumn, iColumn - 2, 2, 1].ToString() + " ～ " +
                    frmMainSchedule.astrResultOtherWorkTime[iScheduleStaffColumn, iColumn - 2, 2, 2].ToString() + ")";
            }

            return strToolTipText;
        }

        /// <summary>
        /// 予定データの未取込チェック Add WataruT 2020.09.09 未取込データチェック機能追加
        /// </summary>
        public void CheckDifference()
        {
            DataTable dtScheduleDetail;
            string strTargetDate;
            bool bTargetFlag;

            strTargetDate = "";

            // 対象月を全日チェック
            for (int iDay = 0; iDay < frmMainSchedule.piDayCount; iDay++)
            {
                bTargetFlag = false;
                for (int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.piScheduleStaffCount; iScheduleStaff++)
                {
                    if (frmMainSchedule.astrResultWorkKind[iScheduleStaff, iDay] != "")
                    {
                        bTargetFlag = true;
                        break;
                    }
                }

                if(bTargetFlag == true)
                {
                    // 対象職員ループ
                    for (int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.piScheduleStaffCount; iScheduleStaff++)
                    {
                        // 病棟、職員ID、職種、対象年月から勤務予定データを取得
                        dtScheduleDetail = clsDatabaseControl.GetScheduleDetail_Ward_Staff_StaffKind_TargetMonth(frmMainSchedule.cmbWard.SelectedValue.ToString(),
                            frmMainSchedule.astrScheduleStaff[iScheduleStaff, 0], frmMainSchedule.pstrStaffKind, frmMainSchedule.pstrTargetMonth);

                        // 既存データがある場合
                        if (dtScheduleDetail.Rows.Count != 0)
                        {
                            if (clsDatabaseControl.GetWorkKind_WorkCheck(dtScheduleDetail.Rows[iDay]["work_kind"].ToString()) == true)
                            {
                                if (dtScheduleDetail.Rows[iDay]["work_kind"].ToString() != frmMainSchedule.astrResultWorkKind[iScheduleStaff, iDay])
                                {
                                    strTargetDate += frmMainSchedule.lblTargetMonth.Text + string.Format("{0:D2}", iDay + 1) + "日(" + frmMainSchedule.grdMainHeader[iDay + 2, 1].Value.ToString() + ")\r\n";
                                    break;
                                }
                            }
                            else
                            {
                                if (dtScheduleDetail.Rows[iDay]["work_kind"].ToString() != frmMainSchedule.astrResultWorkKind[iScheduleStaff, iDay] &&
                                    frmMainSchedule.astrResultWorkKind[iScheduleStaff, iDay] != "")
                                {
                                    strTargetDate += frmMainSchedule.lblTargetMonth.Text + string.Format("{0:D2}", iDay + 1) + "日(" + frmMainSchedule.grdMainHeader[iDay + 2, 1].Value.ToString() + ")\r\n";
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (strTargetDate != "")
                MessageBox.Show("未取込のデータがあります。\r\n\r\n【対象日付】\r\n" + strTargetDate);
            else
                MessageBox.Show("未取込データはありません。");
        }
    }
}
