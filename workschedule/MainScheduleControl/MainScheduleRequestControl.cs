using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using workschedule.Controls;
using workschedule.Functions;

namespace workschedule.MainScheduleControl
{
    class MainScheduleRequestControl
    {
        // 共通変数
        const int GRID_WIDTH_COLUMN_STAFF = 100;
        const int GRID_WIDTH_COLUMN_DATA = 34;

        // 親フォーム
        MainSchedule frmMainSchedule;

        // 使用クラス宣言
        CommonControl clsCommonControl = new CommonControl();
        DatabaseControl clsDatabaseControl = new DatabaseControl();
        DataTableControl clsDataTableControl = new DataTableControl();

        // 初回処理
        public MainScheduleRequestControl(MainSchedule frmMainSchedule_Parent)
        {
            frmMainSchedule = frmMainSchedule_Parent;
        }

        /// <summary>
        /// データグリッドに希望シフトをセット
        /// </summary>
        public void SetMainData_Request()
        {
            int iScheduleStaffCount, iDayCount, iWorkKindCount;     // データ数(職員、日付、勤務種類)
            string strTargetMonth;                                  // 対象年月
            DataTable dt;
            DataRow dr;

            // データ数を変数にセット
            iScheduleStaffCount = frmMainSchedule.dtScheduleStaff.Rows.Count;
            iDayCount = clsCommonControl.GetTargetMonthDays(frmMainSchedule.lblTargetMonth.Text);
            iWorkKindCount = frmMainSchedule.dtWorkKind.Rows.Count;

            // 対象年月をセット
            strTargetMonth = frmMainSchedule.lblTargetMonth.Text.Substring(0, 4) + frmMainSchedule.lblTargetMonth.Text.Substring(5, 2);

            //グリッドの描画処理停止
            frmMainSchedule.grdMain.SuspendLayout();
            frmMainSchedule.grdMain.DataSource = null;

            // 初期データをセット
            for (int iScheduleStaff = 0; iScheduleStaff < iScheduleStaffCount; iScheduleStaff++)
            {
                for (int iDay = 0; iDay < iDayCount; iDay++)
                {
                    for (int iWorkKind = 0; iWorkKind < iWorkKindCount; iWorkKind++)
                    {
                        frmMainSchedule.aiData[iScheduleStaff, iDay, iWorkKind] = 0;
                    }
                }
                for (int iWorkKind = 0; iWorkKind < 3; iWorkKind++)
                {
                    frmMainSchedule.adRowTotalData[iScheduleStaff, iWorkKind] = 0;
                }
            }

            for (int iDay = 0; iDay < iDayCount; iDay++)
            {
                for (int iWorkKind = 0; iWorkKind < 3; iWorkKind++)
                {
                    frmMainSchedule.adColumnTotalData[iDay, iWorkKind] = 0;
                }
            }

            // 希望シフトデータをセット
            SetRequestData();

            //
            // --- メイングリッドヘッダ ---
            //

            // DataTableを初期化
            dt = new DataTable();

            // DataTableにカラムヘッダを作成
            dt.Columns.Add("NAME", Type.GetType("System.String"));
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
                // 列幅・色(職員)
                frmMainSchedule.grdMainHeader[0, iRow].Style.ForeColor = Color.Black;
                frmMainSchedule.grdMainHeader[0, iRow].Style.BackColor = SystemColors.Control;
                frmMainSchedule.grdMainHeader.Columns[0].Width = GRID_WIDTH_COLUMN_STAFF;

                for (int iColumn = 1; iColumn <= frmMainSchedule.piDayCount; iColumn++)
                {
                    // 列幅
                    frmMainSchedule.grdMainHeader.Columns[iColumn].Width = GRID_WIDTH_COLUMN_DATA;

                    // 色(日付、曜日)
                    frmMainSchedule.grdMainHeader[iColumn, iRow].Style.ForeColor = clsCommonControl.GetWeekNameForeColor(frmMainSchedule.grdMainHeader[iColumn, iRow].Value.ToString());
                    frmMainSchedule.grdMainHeader[iColumn, iRow].Style.BackColor = clsCommonControl.GetWeekNameBackgroundColor(
                        clsCommonControl.GetWeekName(frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", iColumn), frmMainSchedule.astrHoliday));
                }
            }

            //
            // --- メイングリッドデータ ---
            //

            // データテーブル作成
            dt = new DataTable();

            dt.Columns.Add("NAME", Type.GetType("System.String"));

            for (int iDay = 1; iDay <= iDayCount; iDay++)
            {
                dt.Columns.Add(iDay.ToString(), Type.GetType("System.String"));
            }

            for (int iScheduleStaff = 0; iScheduleStaff < iScheduleStaffCount; iScheduleStaff++)
            {
                DataRow nr = dt.NewRow();

                nr["NAME"] = frmMainSchedule.astrScheduleStaff[iScheduleStaff, 1];
                for (int iDay = 1; iDay <= iDayCount; iDay++)
                {
                    for (int iWorkKind = 0; iWorkKind < iWorkKindCount; iWorkKind++)
                    {
                        if (frmMainSchedule.aiData[iScheduleStaff, iDay - 1, iWorkKind] == 1)
                        {
                            nr[iDay.ToString()] = frmMainSchedule.astrWorkKind[iWorkKind, 1];
                            break;
                        }
                    }
                }
                dt.Rows.Add(nr);
            }

            // メイングリッドにデータをセット
            frmMainSchedule.grdMain.DataSource = dt;

            // 職員氏名欄のデザイン設定
            for (int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.piScheduleStaffCount; iScheduleStaff++)
            {
                frmMainSchedule.grdMain[0, iScheduleStaff].Style.ForeColor = Color.Black;
                frmMainSchedule.grdMain[0, iScheduleStaff].Style.BackColor = SystemColors.Control;
            }

            // 列幅(職員)
            frmMainSchedule.grdMain.Columns[0].Width = GRID_WIDTH_COLUMN_STAFF;

            // 勤務種類データのデザイン設定
            for (int iDay = 1; iDay <= frmMainSchedule.piDayCount; iDay++)
            {
                //列幅
                frmMainSchedule.grdMain.Columns[iDay].Width = GRID_WIDTH_COLUMN_DATA;

                // 文字の色
                for (int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.piScheduleStaffCount; iScheduleStaff++)
                {
                    frmMainSchedule.grdMain[iDay, iScheduleStaff].Style.ForeColor = clsCommonControl.GetWorkKindForeColor(
                        frmMainSchedule.grdMain[iDay, iScheduleStaff].Value.ToString());
                    frmMainSchedule.grdMain[iDay, iScheduleStaff].Style.BackColor = clsCommonControl.GetWeekNameBackgroundColor(
                        clsCommonControl.GetWeekName(frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", iDay), frmMainSchedule.astrHoliday));
                }
            }

            // 先頭列のみ固定とする
            frmMainSchedule.grdMain.Columns[0].Frozen = true;

            // 行の合計グリッドをセット
            SetRowTotal();

            // 列の合計グリッドをセット
            SetColumnTotal();

            // グリッドの選択状態を解除
            frmMainSchedule.grdMain.CurrentCell = null;
            frmMainSchedule.grdMainHeader.CurrentCell = null;

            //グリッドの描画再開
            frmMainSchedule.grdMain.ResumeLayout();
            frmMainSchedule.grdMainHeader.ResumeLayout();
        }

        /// <summary>
        /// 行の合計をグリッドにセット
        /// </summary>
        private void SetRowTotal()
        {
            // データテーブル作成
            DataTable dt = new DataTable();
            DataRow dr;

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
            dt.Columns.Add("KIND1", Type.GetType("System.String"));
            dt.Columns.Add("KIND2", Type.GetType("System.String"));
            dt.Columns.Add("KIND3", Type.GetType("System.String"));

            // 勤務種類データをセット
            // 1行目
            dr = dt.NewRow();
            dr["KIND1"] = "公";
            dr["KIND2"] = "夜";
            dr["KIND3"] = "夜";
            dt.Rows.Add(dr);
            // 2行目
            dr = dt.NewRow();
            dr["KIND1"] = "休";
            dr["KIND2"] = "勤";
            dr["KIND3"] = "明";
            dt.Rows.Add(dr);

            // 行合計ヘッダにデータをセット
            frmMainSchedule.grdRowTotalHeader.DataSource = dt;

            //列幅
            for (int iColumn = 0; iColumn < 3; iColumn++)
                frmMainSchedule.grdRowTotalHeader.Columns[iColumn].Width = 31;

            ///
            /// --- 行合計データ ---
            ///

            // DataTableを初期化
            dt = new DataTable();

            // データテーブルのカラムヘッダを作成
            dt.Columns.Add("KIND1", Type.GetType("System.String"));
            dt.Columns.Add("KIND2", Type.GetType("System.String"));
            dt.Columns.Add("KIND3", Type.GetType("System.String"));

            // 初期データをセット
            for (int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.piScheduleStaffCount; iScheduleStaff++)
            {
                dr = dt.NewRow();
                for (int iWorkKind = 0; iWorkKind < 3; iWorkKind++)
                {
                    dr["KIND" + (iWorkKind + 1).ToString()] = frmMainSchedule.adRowTotalData[iScheduleStaff, iWorkKind];
                }
                dt.Rows.Add(dr);
            }

            // グリッドにデータテーブルをセット
            frmMainSchedule.grdRowTotal.DataSource = dt;

            // グリッドのオプション設定
            for (int iWorkKind = 0; iWorkKind < 3; iWorkKind++)
            {
                //列幅
                frmMainSchedule.grdRowTotal.Columns[iWorkKind].Width = 31;
                //ソートモード
                frmMainSchedule.grdRowTotal.Columns[iWorkKind].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // グリッドの選択状態を解除
            frmMainSchedule.grdRowTotalHeader.CurrentCell = null;
            frmMainSchedule.grdRowTotal.CurrentCell = null;

            // グリッドの描画処理停止
            frmMainSchedule.grdRowTotalHeader.ResumeLayout();
            frmMainSchedule.grdRowTotal.ResumeLayout();

        }

        /// <summary>
        /// 列の合計をグリッドにセット
        /// </summary>
        private void SetColumnTotal()
        {
            // データテーブル作成
            DataTable dt = new DataTable();

            // グリッドの描画処理停止
            frmMainSchedule.grdColumnTotal.SuspendLayout();
            frmMainSchedule.grdColumnTotal.DataSource = null;

            // データテーブルのカラムヘッダを作成
            dt.Columns.Add("NAME", Type.GetType("System.String"));
            for (int iDay = 1; iDay < frmMainSchedule.piDayCount + 1; iDay++)
            {
                dt.Columns.Add("DAY" + iDay.ToString(), Type.GetType("System.String"));
            }

            // 初期データをセット
            for (int iWorkKind = 0; iWorkKind < 3; iWorkKind++)
            {
                DataRow dr = dt.NewRow();
                dr["NAME"] = clsCommonControl.GetWorkKindTotalName(iWorkKind);

                for (int iDay = 1; iDay < frmMainSchedule.piDayCount + 1; iDay++)
                {
                    dr["DAY" + iDay.ToString()] = frmMainSchedule.adColumnTotalData[iDay - 1, iWorkKind];
                }
                dt.Rows.Add(dr);
            }

            // グリッドにデータテーブルをセット
            frmMainSchedule.grdColumnTotal.DataSource = dt;

            // 列幅(勤務種類)
            frmMainSchedule.grdColumnTotal.Columns[0].Width = GRID_WIDTH_COLUMN_STAFF;

            // グリッドのオプション設定
            for (int iDay = 1; iDay < frmMainSchedule.piDayCount + 1; iDay++)
            {
                //列幅
                frmMainSchedule.grdColumnTotal.Columns[iDay].Width = GRID_WIDTH_COLUMN_DATA;
                //ソートモード
                frmMainSchedule.grdColumnTotal.Columns[iDay].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // フォント変更
            for (int iColumn = 0; iColumn < frmMainSchedule.grdColumnTotal.Columns.Count; iColumn++)
            {
                frmMainSchedule.grdColumnTotal.Columns[iColumn].DefaultCellStyle.Font = new Font("メイリオ", 9);
            }

            // グリッドの選択状態を解除
            frmMainSchedule.grdColumnTotal.CurrentCell = null;

            // グリッドの描画処理停止
            frmMainSchedule.grdColumnTotal.Columns[0].Frozen = true;
            frmMainSchedule.grdColumnTotal.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
        }

        /// <summary>
        /// 勤務希望シフト登録
        /// </summary>
        public void SaveRequestData()
        {
            DataTable dtRequestShift;
            DataRow drRequestShift;

            string strTargetMonthForHeader = frmMainSchedule.lblTargetMonth.Text.Substring(0, 4) + frmMainSchedule.lblTargetMonth.Text.Substring(5, 2);
            string strSQL = "";

            int piRequestStaffCount = frmMainSchedule.grdMain.Rows.Count;
            int piDayCount = frmMainSchedule.grdMain.ColumnCount - 1;

            // 希望シフトの作成
            strSQL = clsDatabaseControl.GetInsertRequestShift_Insert();
            for (int iRequestStaff = 0; iRequestStaff < piRequestStaffCount; iRequestStaff++)
            {
                for (int iDay = 1; iDay <= piDayCount; iDay++)
                {
                    if (frmMainSchedule.grdMain[iDay, iRequestStaff].Value.ToString() != "")
                    {
                        dtRequestShift = clsDataTableControl.GetTable_RequestShift();
                        drRequestShift = dtRequestShift.NewRow();

                        drRequestShift["staff"] = frmMainSchedule.astrScheduleStaff[iRequestStaff, 0];
                        drRequestShift["ward"] = frmMainSchedule.cmbWard.SelectedValue.ToString();
                        drRequestShift["staff_kind"] = frmMainSchedule.pstrStaffKind;
                        drRequestShift["target_date"] = clsCommonControl.GetTargetDateChangeFormat(strTargetMonthForHeader + String.Format("{0:D2}", iDay));
                        drRequestShift["work_kind"] = clsCommonControl.GetWorkKindID(frmMainSchedule.grdMain[iDay, iRequestStaff].Value.ToString(), frmMainSchedule.astrWorkKind);

                        strSQL += clsDatabaseControl.GetInsertRequestShift_Values(drRequestShift);
                    }
                }
            }

            if (clsDatabaseControl.ExecuteBulkInsertSQL(strSQL) == false)
                MessageBox.Show("勤務希望シフトの登録に失敗しました。");
        }

        /// <summary>
        /// 希望シフトの削除
        /// </summary>
        public void DeleteRequestData()
        {
            string strTargetMonthForHeader = frmMainSchedule.lblTargetMonth.Text.Substring(0, 4) + frmMainSchedule.lblTargetMonth.Text.Substring(5, 2);

            clsDatabaseControl.DeleteRequestShift_Ward(frmMainSchedule.cmbWard.SelectedValue.ToString(), strTargetMonthForHeader, frmMainSchedule.pstrStaffKind);
        }

        /// <summary>
        /// メイングリッドデータの変更
        /// </summary>
        public void ChangeMainGridData(int iWorkKindID)
        {
            int iDeleteTargetRow;
            int iDeleteTargetColumn;

            // 希望シフト判定
            if (clsCommonControl.GetRequestFlag(
                frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn, frmMainSchedule.piGrdMain_CurrentRow].Style.BackColor) == "1")
            {
                if (MessageBox.Show("変更箇所が希望シフトとなりますが、よろしいですか？", "", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }

            // 削除以外なら値をセット
            if (iWorkKindID != 99)
            {
                // 共通変数側の値をリセット
                for (int iWorkKind = 0; iWorkKind < frmMainSchedule.astrWorkKind.GetLength(0); iWorkKind++)
                {
                    if (frmMainSchedule.aiData[frmMainSchedule.piGrdMain_CurrentRow, frmMainSchedule.piGrdMain_CurrentColumn - 1, iWorkKind] == 1)
                    {
                        frmMainSchedule.aiData[frmMainSchedule.piGrdMain_CurrentRow, frmMainSchedule.piGrdMain_CurrentColumn - 1, iWorkKind] = 0;
                        CheckWorkKindForRowTotalData(frmMainSchedule.piGrdMain_CurrentRow, iWorkKind, -1);
                        CheckWorkKindForColumnTotalData(frmMainSchedule.piGrdMain_CurrentColumn - 1, iWorkKind, -1);
                    }
                }

                // 共通変数側の値を設定
                frmMainSchedule.aiData[frmMainSchedule.piGrdMain_CurrentRow, frmMainSchedule.piGrdMain_CurrentColumn - 1, iWorkKindID] = 1;
                CheckWorkKindForRowTotalData(frmMainSchedule.piGrdMain_CurrentRow, iWorkKindID, 1);
                CheckWorkKindForColumnTotalData(frmMainSchedule.piGrdMain_CurrentColumn - 1, iWorkKindID, 1);

                // メイングリッドに値、色設定をセット
                frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn, frmMainSchedule.piGrdMain_CurrentRow].Value = frmMainSchedule.astrWorkKind[iWorkKindID, 1];
                frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn, frmMainSchedule.piGrdMain_CurrentRow].Style.ForeColor = clsCommonControl.GetWorkKindForeColor(
                    frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn, frmMainSchedule.piGrdMain_CurrentRow].Value.ToString());
                frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn, frmMainSchedule.piGrdMain_CurrentRow].Style.BackColor = clsCommonControl.GetWeekNameBackgroundColor(
                    clsCommonControl.GetWeekName(frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", frmMainSchedule.piGrdMain_CurrentColumn), frmMainSchedule.astrHoliday));

                // Add Start WataruT 2020.07.14 夜勤入力時、明けと休みもセットで入力
                // "夜勤"の場合、"夜明"と"休"もセットする
                if(iWorkKindID == 1)
                {
                    // 翌日も同月であれば、"夜明"をセット
                    if(frmMainSchedule.piGrdMain_CurrentColumn + 1 < frmMainSchedule.grdMain.Columns.Count)
                    {
                        // 共通変数側の値をリセット
                        for (int iWorkKind = 0; iWorkKind < frmMainSchedule.astrWorkKind.GetLength(0); iWorkKind++)
                        {
                            if (frmMainSchedule.aiData[frmMainSchedule.piGrdMain_CurrentRow, frmMainSchedule.piGrdMain_CurrentColumn, iWorkKind] == 1)
                            {
                                frmMainSchedule.aiData[frmMainSchedule.piGrdMain_CurrentRow, frmMainSchedule.piGrdMain_CurrentColumn, iWorkKind] = 0;
                                CheckWorkKindForRowTotalData(frmMainSchedule.piGrdMain_CurrentRow, iWorkKind, -1);
                                CheckWorkKindForColumnTotalData(frmMainSchedule.piGrdMain_CurrentColumn, iWorkKind, -1);
                            }
                        }

                        // 共通変数側の値を設定
                        frmMainSchedule.aiData[frmMainSchedule.piGrdMain_CurrentRow, frmMainSchedule.piGrdMain_CurrentColumn, iWorkKindID + 1] = 1;
                        CheckWorkKindForRowTotalData(frmMainSchedule.piGrdMain_CurrentRow, iWorkKindID + 1, 1);
                        CheckWorkKindForColumnTotalData(frmMainSchedule.piGrdMain_CurrentColumn, iWorkKindID + 1, 1);

                        // メイングリッドに値、色設定をセット
                        frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn + 1, frmMainSchedule.piGrdMain_CurrentRow].Value = frmMainSchedule.astrWorkKind[iWorkKindID + 1, 1];
                        frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn + 1, frmMainSchedule.piGrdMain_CurrentRow].Style.ForeColor = clsCommonControl.GetWorkKindForeColor(
                            frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn + 1, frmMainSchedule.piGrdMain_CurrentRow].Value.ToString());
                        frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn + 1, frmMainSchedule.piGrdMain_CurrentRow].Style.BackColor = clsCommonControl.GetWeekNameBackgroundColor(
                            clsCommonControl.GetWeekName(frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", frmMainSchedule.piGrdMain_CurrentColumn + 1), frmMainSchedule.astrHoliday));
                    }
                    // 翌々日も同月であれば、"休"をセット
                    if (frmMainSchedule.piGrdMain_CurrentColumn + 2 < frmMainSchedule.grdMain.Columns.Count)
                    {
                        // 共通変数側の値をリセット
                        for (int iWorkKind = 0; iWorkKind < frmMainSchedule.astrWorkKind.GetLength(0); iWorkKind++)
                        {
                            if (frmMainSchedule.aiData[frmMainSchedule.piGrdMain_CurrentRow, frmMainSchedule.piGrdMain_CurrentColumn + 1, iWorkKind] == 1)
                            {
                                frmMainSchedule.aiData[frmMainSchedule.piGrdMain_CurrentRow, frmMainSchedule.piGrdMain_CurrentColumn + 1, iWorkKind] = 0;
                                CheckWorkKindForRowTotalData(frmMainSchedule.piGrdMain_CurrentRow, iWorkKind, -1);
                                CheckWorkKindForColumnTotalData(frmMainSchedule.piGrdMain_CurrentColumn + 1, iWorkKind, -1);
                            }
                        }

                        // 共通変数側の値を設定
                        frmMainSchedule.aiData[frmMainSchedule.piGrdMain_CurrentRow, frmMainSchedule.piGrdMain_CurrentColumn, iWorkKindID + 2] = 1;
                        CheckWorkKindForRowTotalData(frmMainSchedule.piGrdMain_CurrentRow, iWorkKindID + 2, 1);
                        CheckWorkKindForColumnTotalData(frmMainSchedule.piGrdMain_CurrentColumn + 1, iWorkKindID + 2, 1);

                        // メイングリッドに値、色設定をセット
                        frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn + 2, frmMainSchedule.piGrdMain_CurrentRow].Value = frmMainSchedule.astrWorkKind[iWorkKindID + 2, 1];
                        frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn + 2, frmMainSchedule.piGrdMain_CurrentRow].Style.ForeColor = clsCommonControl.GetWorkKindForeColor(
                            frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn + 2, frmMainSchedule.piGrdMain_CurrentRow].Value.ToString());
                        frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn + 2, frmMainSchedule.piGrdMain_CurrentRow].Style.BackColor = clsCommonControl.GetWeekNameBackgroundColor(
                            clsCommonControl.GetWeekName(frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", frmMainSchedule.piGrdMain_CurrentColumn + 2), frmMainSchedule.astrHoliday));
                    }
                }
                // Add End   WataruT 2020.07.14 夜勤入力時、明けと休みもセットで入力
            }
            else
            {
                iDeleteTargetColumn = frmMainSchedule.grdMain.SelectedCells[0].ColumnIndex;
                iDeleteTargetRow = frmMainSchedule.grdMain.SelectedCells[0].RowIndex;

                // 共通変数側の値をリセット
                for (int iWorkKind = 0; iWorkKind < frmMainSchedule.astrWorkKind.GetLength(0); iWorkKind++)
                {
                    if (frmMainSchedule.aiData[iDeleteTargetRow, iDeleteTargetColumn - 1, iWorkKind] == 1)
                    {
                        frmMainSchedule.aiData[iDeleteTargetRow, iDeleteTargetColumn - 1, iWorkKind] = 0;
                        CheckWorkKindForRowTotalData(iDeleteTargetRow, iWorkKind, -1);
                        CheckWorkKindForColumnTotalData(iDeleteTargetColumn - 1, iWorkKind, -1);
                    }
                }

                frmMainSchedule.grdMain.SelectedCells[0].Value = "";
            }

            // 行・列の合計グリッドを再描画
            SetRowTotal();
            SetColumnTotal();

            // 列・行の合計グリッドの表示位置をセット
            frmMainSchedule.grdRowTotal.FirstDisplayedScrollingRowIndex = frmMainSchedule.grdMain.FirstDisplayedScrollingRowIndex;
            frmMainSchedule.grdColumnTotal.FirstDisplayedScrollingColumnIndex = frmMainSchedule.grdMain.FirstDisplayedScrollingColumnIndex;
        }

        /// <summary>
        /// メイングリッドデータの一括変更
        /// Add WataruT 2020.07.13 複数選択箇所を一括変更可能とする
        /// </summary>
        public void ChangeMainGridMultiData(int iWorkKindID)
        {
            int iDeleteTargetRow;
            int iDeleteTargetColumn;
            int iCurrentRow;
            int iCurrentColumn;

            // 希望シフト判定
            for(int iTargetCell = 0; iTargetCell < frmMainSchedule.grdMain.SelectedCells.Count;iTargetCell++)
            {
                if (clsCommonControl.GetRequestFlag(frmMainSchedule.grdMain.SelectedCells[iTargetCell].Style.BackColor) == "1")
                {
                    if (MessageBox.Show("変更箇所が希望シフトとなりますが、よろしいですか？", "", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }else
                    {
                        break;
                    }
                }

            }

            // 選択しているセル数だけ処理
            for (int iTargetCell = 0; iTargetCell < frmMainSchedule.grdMain.SelectedCells.Count; iTargetCell++)
            {
                // 氏名列は対象外
                if (frmMainSchedule.grdMain.SelectedCells[iTargetCell].ColumnIndex > 0)
                {
                    // 削除以外なら値をセット
                    if (iWorkKindID != 99)
                    {
                        //選択したセルの行・列を取得
                        iCurrentRow = frmMainSchedule.grdMain.SelectedCells[iTargetCell].RowIndex;
                        iCurrentColumn = frmMainSchedule.grdMain.SelectedCells[iTargetCell].ColumnIndex;

                        // 共通変数側の値をリセット
                        for (int iWorkKind = 0; iWorkKind < frmMainSchedule.astrWorkKind.GetLength(0); iWorkKind++)
                        {
                            if (frmMainSchedule.aiData[iCurrentRow, iCurrentColumn - 1, iWorkKind] == 1)
                            {
                                frmMainSchedule.aiData[iCurrentRow, iCurrentColumn - 1, iWorkKind] = 0;
                                CheckWorkKindForRowTotalData(iCurrentRow, iWorkKind, -1);
                                CheckWorkKindForColumnTotalData(iCurrentColumn - 1, iWorkKind, -1);
                            }
                        }

                        // 共通変数側の値を設定
                        frmMainSchedule.aiData[iCurrentRow, iCurrentColumn - 1, iWorkKindID] = 1;
                        CheckWorkKindForRowTotalData(iCurrentRow, iWorkKindID, 1);
                        CheckWorkKindForColumnTotalData(iCurrentColumn - 1, iWorkKindID, 1);

                        // メイングリッドに値、色設定をセット
                        frmMainSchedule.grdMain[iCurrentColumn, iCurrentRow].Value = frmMainSchedule.astrWorkKind[iWorkKindID, 1];
                        frmMainSchedule.grdMain[iCurrentColumn, iCurrentRow].Style.ForeColor = clsCommonControl.GetWorkKindForeColor(
                            frmMainSchedule.grdMain[iCurrentColumn, iCurrentRow].Value.ToString());
                        frmMainSchedule.grdMain[iCurrentColumn, iCurrentRow].Style.BackColor = clsCommonControl.GetWeekNameBackgroundColor(
                            clsCommonControl.GetWeekName(frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", iCurrentColumn), frmMainSchedule.astrHoliday));
                    }
                    else
                    {
                        iDeleteTargetColumn = frmMainSchedule.grdMain.SelectedCells[iTargetCell].ColumnIndex;
                        iDeleteTargetRow = frmMainSchedule.grdMain.SelectedCells[iTargetCell].RowIndex;

                        // 共通変数側の値をリセット
                        for (int iWorkKind = 0; iWorkKind < frmMainSchedule.astrWorkKind.GetLength(0); iWorkKind++)
                        {
                            if (frmMainSchedule.aiData[iDeleteTargetRow, iDeleteTargetColumn - 1, iWorkKind] == 1)
                            {
                                frmMainSchedule.aiData[iDeleteTargetRow, iDeleteTargetColumn - 1, iWorkKind] = 0;
                                CheckWorkKindForRowTotalData(iDeleteTargetRow, iWorkKind, -1);
                                CheckWorkKindForColumnTotalData(iDeleteTargetColumn - 1, iWorkKind, -1);
                            }
                        }

                        frmMainSchedule.grdMain.SelectedCells[iTargetCell].Value = "";
                    }
                }
            }


            // 行・列の合計グリッドを再描画
            SetRowTotal();
            SetColumnTotal();

            // 列・行の合計グリッドの表示位置をセット
            frmMainSchedule.grdRowTotal.FirstDisplayedScrollingRowIndex = frmMainSchedule.grdMain.FirstDisplayedScrollingRowIndex;
            frmMainSchedule.grdColumnTotal.FirstDisplayedScrollingColumnIndex = frmMainSchedule.grdMain.FirstDisplayedScrollingColumnIndex;
        }

        /// <summary>
        /// 希望シフトをセット
        /// </summary>
        private void SetRequestData()
        {
            DataTable dt;
            int iTargetDay;
            dt = clsDatabaseControl.GetRequestShift_Ward(frmMainSchedule.cmbWard.SelectedValue.ToString(), frmMainSchedule.pstrTargetMonth);

            foreach (DataRow row in dt.Rows)
            {
                // 職員IDチェック
                for (int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.dtScheduleStaff.Rows.Count; iScheduleStaff++)
                {
                    if (frmMainSchedule.astrScheduleStaff[iScheduleStaff, 0] == row["staff"].ToString())
                    {
                        // 対象日をセット
                        iTargetDay = int.Parse(row["target_date"].ToString().Substring(8, 2)) - 1;

                        // 対象日の職員の勤務予定データを初期化する
                        for (int iWorkKind = 0; iWorkKind < frmMainSchedule.dtWorkKind.Rows.Count; iWorkKind++)
                        {
                            if (frmMainSchedule.aiData[iScheduleStaff, iTargetDay, iWorkKind] == 1)
                            {
                                frmMainSchedule.aiData[iScheduleStaff, iTargetDay, iWorkKind] = 0;
                                CheckWorkKindForRowTotalData(iScheduleStaff, iWorkKind, -1);
                                CheckWorkKindForColumnTotalData(iTargetDay, iWorkKind, -1);
                            }
                        }

                        // 希望シフトの勤務をセット
                        frmMainSchedule.aiData[iScheduleStaff, iTargetDay, int.Parse(row["work_kind"].ToString()) - 1] = 1;
                        frmMainSchedule.aiDataRequestFlag[iScheduleStaff, iTargetDay] = 1;
                        CheckWorkKindForRowTotalData(iScheduleStaff, int.Parse(row["work_kind"].ToString()) - 1, 1);
                        CheckWorkKindForColumnTotalData(iTargetDay, int.Parse(row["work_kind"].ToString()) - 1, 1);
                    }
                }
            }
        }

        /// <summary>
        /// 行合計用のデータ追加
        /// </summary>
        private void CheckWorkKindForRowTotalData(int iScheduleStaff, int iWorkKind, double dAddNumber)
        {
            switch (iWorkKind)
            {
                case 3: // 公休(1日)
                    frmMainSchedule.adRowTotalData[iScheduleStaff, 0] += dAddNumber;
                    break;
                case 4: // 公休(午前)
                case 5: // 公休(午後)
                //Mod Start WataruT 2020.07.16 代有の公休計算対応
                //case 10:// 代有
                case 9:// 代有
                //Mod End   WataruT 2020.07.16 代有の公休計算対応
                    frmMainSchedule.adRowTotalData[iScheduleStaff, 0] += dAddNumber * 0.5;
                    break;
                case 1: // 夜勤
                    frmMainSchedule.adRowTotalData[iScheduleStaff, 1] += dAddNumber;
                    break;
                case 2: // 夜明
                    frmMainSchedule.adRowTotalData[iScheduleStaff, 2] += dAddNumber;
                    break;
            }
        }

        /// <summary>
        /// 列合計用のデータ追加
        /// </summary>
        private void CheckWorkKindForColumnTotalData(int iDay, int iWorkKind, double dAddNumber)
        {
            switch (iWorkKind)
            {
                case 0: // 日勤
                // Add Start WataruT 2020.07.22 特定の時短勤務用の項目追加
                case 17: // 5.25
                // Add End   WataruT 2020.07.22 特定の時短勤務用の項目追加
                // Add Start WataruT 2020.07.27 特定の時短勤務用の項目追加
                case 19: // 6
                case 20: // 6.25
                case 21: // 7
                // Add End   WataruT 2020.07.27 特定の時短勤務用の項目追加
                    frmMainSchedule.adColumnTotalData[iDay, 0] += dAddNumber;
                    break;
                case 4: // 公休(午前)
                case 5: // 公休(午後)
                case 7: // 有休(午前)
                case 8: // 有休(午後)
                // Add Start WataruT 2020.07.27 特定の時短勤務用の項目追加
                case 18: // 2
                // Add End   WataruT 2020.07.27 特定の時短勤務用の項目追加
                    frmMainSchedule.adColumnTotalData[iDay, 0] += dAddNumber * 0.5;
                    break;
                case 1: // 夜勤
                    frmMainSchedule.adColumnTotalData[iDay, 1] += dAddNumber;
                    break;
                case 2: // 夜明
                    frmMainSchedule.adColumnTotalData[iDay, 2] += dAddNumber;
                    break;
            }
        }


    }
}
