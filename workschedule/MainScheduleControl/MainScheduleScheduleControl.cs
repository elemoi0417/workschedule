using NPOI.HSSF.Record.Chart;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using workschedule.Controls;
using workschedule.Functions;

namespace workschedule.MainScheduleControl
{
    class MainScheduleScheduleControl
    {
        // 共通変数
        const int GRID_WIDTH_COLUMN_STAFF = 90;
        const int GRID_WIDTH_COLUMN_DATA = 36;
        const int GRID_WIDTH_COLUMN_DATA_ROWTOTAL = 27;

        // 親フォーム
        MainSchedule frmMainSchedule;

        // 使用クラス宣言
        MainScheduleCheckControl clsMainScheduleCheckControl;
        CommonControl clsCommonControl = new CommonControl();
        DatabaseControl clsDatabaseControl = new DatabaseControl();
        DataTableControl clsDataTableControl = new DataTableControl();

        /// <summary>
        /// 初回処理
        /// </summary>
        /// <param name="frmMainSchedule_Parent"></param>
        public MainScheduleScheduleControl(MainSchedule frmMainSchedule_Parent)
        {
            frmMainSchedule = frmMainSchedule_Parent;
            clsMainScheduleCheckControl = new MainScheduleCheckControl(frmMainSchedule);
        }

        /// <summary>
        /// 既存データをグリッドにセット
        /// </summary>
        public void SetMainData_Schedule()
        {
            DataTable dtScheduleDetail;
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

            // 初期データをセット
            for (int iDay = 0; iDay < frmMainSchedule.piDayCount; iDay++)
            {
                for (int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.piScheduleStaffCount; iScheduleStaff++)
                {
                    for (int iWorkKind = 0; iWorkKind < frmMainSchedule.piWorkKindCount; iWorkKind++)
                    {
                        frmMainSchedule.aiData[iScheduleStaff, iDay, iWorkKind] = 0;
                        frmMainSchedule.aiDataRequestFlag[iScheduleStaff, iDay] = 0;
                        if (iWorkKind < 3)
                            frmMainSchedule.adRowTotalData[iScheduleStaff, iWorkKind] = 0;
                    }
                }
                for (int iWorkKind = 0; iWorkKind < 6; iWorkKind++)
                {
                    frmMainSchedule.adColumnTotalData[iDay, iWorkKind] = 0;
                }
            }

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
                frmMainSchedule.grdMainHeader.Columns[0].Width = GRID_WIDTH_COLUMN_STAFF;
                frmMainSchedule.grdMainHeader[0, iRow].Style.ForeColor = Color.Black;
                frmMainSchedule.grdMainHeader[0, iRow].Style.BackColor = SystemColors.Control;

                for (int iColumn = 1; iColumn <= frmMainSchedule.piDayCount; iColumn++)
                {
                    // 列幅
                    frmMainSchedule.grdMainHeader.Columns[iColumn].Width = GRID_WIDTH_COLUMN_DATA;

                    // 色(日付、曜日)
                    frmMainSchedule.grdMainHeader[iColumn, iRow].Style.ForeColor = clsCommonControl.GetWeekNameForeColor(frmMainSchedule.grdMainHeader[iColumn, iRow].Value.ToString());
                    frmMainSchedule.grdMainHeader[iColumn, iRow].Style.BackColor = clsCommonControl.GetWeekNameBackgroundColor(clsCommonControl.GetWeekName(
                        frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", iColumn), frmMainSchedule.astrHoliday));
                }
            }

            //
            // --- メイングリッドデータ ---
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
            for (int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.piScheduleStaffCount; iScheduleStaff++)
            {
                dr = dt.NewRow();

                dr["NAME"] = frmMainSchedule.astrScheduleStaff[iScheduleStaff, 1];

                // 病棟、職員ID、職種、対象年月から勤務予定データを取得
                dtScheduleDetail = clsDatabaseControl.GetScheduleDetail_Ward_Staff_StaffKind_TargetMonth(frmMainSchedule.cmbWard.SelectedValue.ToString(),
                    frmMainSchedule.astrScheduleStaff[iScheduleStaff, 0], frmMainSchedule.pstrStaffKind, frmMainSchedule.pstrTargetMonth);

                // 既存データがある場合
                if (dtScheduleDetail.Rows.Count != 0)
                {
                    for (int iDay = 1; iDay <= frmMainSchedule.piDayCount; iDay++)
                    {
                        dr[iDay.ToString()] = dtScheduleDetail.Rows[iDay - 1]["name_short"];
                        frmMainSchedule.aiData[iScheduleStaff, iDay - 1, int.Parse(dtScheduleDetail.Rows[iDay - 1]["work_kind"].ToString()) - 1] = 1;
                        frmMainSchedule.aiDataRequestFlag[iScheduleStaff, iDay - 1] = int.Parse(dtScheduleDetail.Rows[iDay - 1]["request_flag"].ToString());
                        CheckWorkKindForRowTotalData(iScheduleStaff, int.Parse(dtScheduleDetail.Rows[iDay - 1]["work_kind"].ToString()) - 1, 1);
                        CheckWorkKindForColumnTotalData(iDay - 1, int.Parse(dtScheduleDetail.Rows[iDay - 1]["work_kind"].ToString()) - 1, 1, frmMainSchedule.astrScheduleStaff[iScheduleStaff, 0]);
                    }
                }
                // 既存データがない場合
                else
                {
                    for (int iDay = 1; iDay <= frmMainSchedule.piDayCount; iDay++)
                    {
                        dr[iDay.ToString()] = frmMainSchedule.astrWorkKind[0, 1];
                        frmMainSchedule.aiData[iScheduleStaff, iDay - 1, 0] = 1;
                        frmMainSchedule.aiDataRequestFlag[iScheduleStaff, iDay - 1] = 0;
                        CheckWorkKindForRowTotalData(iScheduleStaff, 0, 1);
                        CheckWorkKindForColumnTotalData(iDay - 1, 0, 1, frmMainSchedule.astrScheduleStaff[iScheduleStaff, 0]);
                    }
                }

                dt.Rows.Add(dr);
            }

            // メイングリッドにデータをセット
            frmMainSchedule.grdMain.DataSource = dt;

            // 職員氏名欄のデザイン設定
            for (int i = 0; i < frmMainSchedule.piScheduleStaffCount; i++)
            {
                frmMainSchedule.grdMain[0, i].Style.ForeColor = clsCommonControl.GetSexForeColor(clsDatabaseControl.GetStaff_Sex(frmMainSchedule.astrScheduleStaff[i, 0]));
                frmMainSchedule.grdMain[0, i].Style.BackColor = SystemColors.Control;
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
                    frmMainSchedule.grdMain[iDay, iScheduleStaff].Style.ForeColor = clsCommonControl.GetWorkKindForeColor(frmMainSchedule.grdMain[iDay, iScheduleStaff].Value.ToString());
                    frmMainSchedule.grdMain[iDay, iScheduleStaff].Style.BackColor = clsCommonControl.GetWeekNameBackgroundColor(clsCommonControl.GetWeekName(
                        frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", iDay), frmMainSchedule.astrHoliday));
                }
            }

            // 希望シフトのセルのみ太文字にする
            for (int iDay = 0; iDay < frmMainSchedule.grdMain.ColumnCount - 1; iDay++)
            {
                for (int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.grdMain.RowCount - 1; iScheduleStaff++)
                {
                    if (frmMainSchedule.aiDataRequestFlag[iScheduleStaff, iDay] == 1)
                    {
                        frmMainSchedule.grdMain[iDay + 1, iScheduleStaff].Style.Font = new Font(frmMainSchedule.grdMain.Font, FontStyle.Bold);
                        frmMainSchedule.grdMain[iDay + 1, iScheduleStaff].Style.BackColor = Color.Gold;
                    }
                }
            }

            // 先頭列のみ固定とする
            // Mod Start WataruT 2020.07.22 計画表の先頭行のみ固定される不具合修正
            //// MOD START WataruT 2020.07.13 対象年月と対象病棟切り替え時、エラーが発生する
            ////frmMainSchedule.grdMain.Rows[0].Frozen = true;
            //if (frmMainSchedule.grdMain.Rows.Count > 0)
            //{
            //    frmMainSchedule.grdMain.Rows[0].Frozen = true;
            //}
            //// MOD END   WataruT 2020.07.13 対象年月と対象病棟切り替え時、エラーが発生する
            frmMainSchedule.grdMain.Columns[0].Frozen = true;
            // Mod End   WataruT 2020.07.22 計画表の先頭行のみ固定される不具合修正

            // 表示していない職種の列合計値を取得
            SetColumnTotalOtherStaffKindCount();

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
        /// 予定シフトの作成
        /// </summary>
        public void CreateShiftData()
        {
            int iRandomScheduleStaff, iRandomDay, iRandomWorkKind;  // 乱数用変数(職員、日付、勤務種類)
            int iDayOfWeekNum;                                      // 曜日マスタの番号
            int iHalfNurseCount, iHalfCareCount;                    // 半日の看護師、ケア人数
            string strTargetDate;                                   // 対象日（乱数用）

            // データ数を変数にセット
            frmMainSchedule.piScheduleStaffCount = frmMainSchedule.dtScheduleStaff.Rows.Count;
            frmMainSchedule.piDayCount = clsCommonControl.GetTargetMonthDays(frmMainSchedule.lblTargetMonth.Text);
            frmMainSchedule.piWorkKindCount = frmMainSchedule.dtWorkKind.Rows.Count;

            // ランダム変数宣言
            System.Random r;

            //グリッドの描画処理停止
            frmMainSchedule.grdMain.SuspendLayout();

            // グリッドの初期化
            frmMainSchedule.grdMain.DataSource = null;

            // 初期データをセット
            for (int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.piScheduleStaffCount; iScheduleStaff++)
            {
                for (int iDay = 0; iDay < frmMainSchedule.piDayCount; iDay++)
                {
                    for (int iWorkKind = 0; iWorkKind < frmMainSchedule.piWorkKindCount; iWorkKind++)
                    {
                        if (iWorkKind == 0)
                        {
                            frmMainSchedule.aiData[iScheduleStaff, iDay, iWorkKind] = 1;
                            frmMainSchedule.aiNightLastMonthFlag[iScheduleStaff, iDay] = 0;
                            frmMainSchedule.aiDataNow[iScheduleStaff, iDay] = iWorkKind;
                            frmMainSchedule.aiDataRequestFlag[iScheduleStaff, iDay] = 0;
                        }
                        else
                        {
                            frmMainSchedule.aiData[iScheduleStaff, iDay, iWorkKind] = 0;
                            frmMainSchedule.aiNightLastMonthFlag[iScheduleStaff, iDay] = 0;
                            frmMainSchedule.aiDataRequestFlag[iScheduleStaff, iDay] = 0;
                        }
                    }
                }

                for (int iWorkKind = 0; iWorkKind < 3; iWorkKind++)
                {
                    frmMainSchedule.adRowTotalData[iScheduleStaff, iWorkKind] = 0;
                }
            }

            // 列合計データを初期化
            for (int iDay = 0; iDay < frmMainSchedule.piDayCount; iDay++)
            {
                frmMainSchedule.adColumnTotalData[iDay, 0] = frmMainSchedule.piScheduleStaffCount;
                frmMainSchedule.adColumnTotalData[iDay, 1] = 0;
                frmMainSchedule.adColumnTotalData[iDay, 2] = 0;
                frmMainSchedule.adColumnTotalData[iDay, 3] = 0;
                frmMainSchedule.adColumnTotalData[iDay, 4] = 0;
                frmMainSchedule.adColumnTotalData[iDay, 5] = 0;
            }

            // 土日祝タブー職員の公休を土日祝にセット
            SetHolidayData();

            // 希望シフトをセット
            SetRequestData();

            // 前月末の夜勤チェック
            CheckNightLastMonth();

            // 表示していない職種の列合計値を取得
            SetColumnTotalOtherStaffKindCount();

            // **************************************************************************************************
            // ランダムデータの作成（土日祝の夜勤、男性のみ）
            for (int i = 0; i < 30000; i++)
            {
                // 乱数データのセット
                r = MyRandom.Create();
                iRandomScheduleStaff = r.Next() % frmMainSchedule.piScheduleStaffCount;
                iRandomDay = r.Next() % frmMainSchedule.piDayCount;
                iRandomWorkKind = 1;

                // 対象日を文字列としてセット(YYYYMMDD)
                strTargetDate = frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", (iRandomDay + 1)).ToString();

                // 対象日に当てはまる曜日マスタの配列番号をセット
                iDayOfWeekNum = GetDayOfWeekNum(clsCommonControl.GetWeekName(strTargetDate, frmMainSchedule.astrHoliday));

                // --- 判定ロジック(ここから) ---

                // 男性かチェック
                if (frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 2] != "1") continue;

                // 対象日が土日祝かチェック
                if (clsMainScheduleCheckControl.CheckHolidayFlagForHoliday(strTargetDate, frmMainSchedule.astrHoliday) == false) continue;

                // 夜勤人数チェック（男性のみ）
                if (clsMainScheduleCheckControl.CheckMenCountForHolidayNight(iRandomDay) == false) continue;

                // 常日勤職員チェック
                if (clsMainScheduleCheckControl.CheckDayOnlyShift(frmMainSchedule.astrStaffDayOnly, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0], clsCommonControl.GetTargetDateChangeFormat(strTargetDate)))
                    continue;

                // 対象日が日勤かチェック
                if (frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, frmMainSchedule.WORK_KIND_DAY_ARRAY_NUM] != 1)
                    continue;

                // 土日祝の夜勤人数チェック
                if (clsMainScheduleCheckControl.CheckHolidayFlagForHoliday(strTargetDate, frmMainSchedule.astrHoliday) == true)
                {
                    if (clsMainScheduleCheckControl.CheckMenCountForHolidayNight(iRandomDay) == false)
                        continue;
                }

                // 希望シフトチェック
                if (frmMainSchedule.aiDataRequestFlag[iRandomScheduleStaff, iRandomDay] == 1) continue;

                // 前月末の夜勤チェック
                if (frmMainSchedule.aiNightLastMonthFlag[iRandomScheduleStaff, iRandomDay] == 1) continue;

                // 勤務種類の並び確認
                if (clsMainScheduleCheckControl.CheckWorkKindOrder(iRandomScheduleStaff, iRandomDay, frmMainSchedule.aiDataNow) == false) continue;

                // 対象職員の勤務日数チェック
                if (clsMainScheduleCheckControl.CheckWorkRowCount(iRandomScheduleStaff, iRandomDay, iRandomWorkKind, frmMainSchedule.aiData, frmMainSchedule.adRowTotalData) == false) continue;

                // 日勤の最低人数を下回らないかチェック
                if (clsMainScheduleCheckControl.CheckCountLimitDayMinDayForNight(iRandomDay, iRandomScheduleStaff, iDayOfWeekNum) == false)
                { continue; }

                // 夜勤の最高人数を上回らないかチェック
                if (clsMainScheduleCheckControl.CheckCountLimitDayMaxNight(iRandomDay, iRandomScheduleStaff, iDayOfWeekNum) == false)
                { continue; }

                // 夜勤のセットが可能か判定
                if (clsMainScheduleCheckControl.CheckWorkKindOrderForNightHoliday(iRandomScheduleStaff, iRandomDay, clsCommonControl.GetTargetMonthDays(frmMainSchedule.lblTargetMonth.Text),
                    frmMainSchedule.aiDataNow, frmMainSchedule.aiDataRequestFlag) == false) continue;

                // 夜勤の最小数の職員か確認
                if (clsMainScheduleCheckControl.CheckMinNightFlag(iRandomScheduleStaff, frmMainSchedule.adRowTotalData, frmMainSchedule.astrScheduleStaff, frmMainSchedule.astrStaffDayOnly,
                    clsCommonControl.GetTargetDateChangeFormat(strTargetDate)) == false)
                    continue;

                // 公休数チェック（夜勤用）
                if (clsMainScheduleCheckControl.CheckWorkHolidayForNight(iRandomScheduleStaff, iRandomDay) == false) continue;

                // 常日勤なしで土日祝タブー職員の場合、休みが土日祝になるかチェック
                if (frmMainSchedule.astrStaffDayOnly[iRandomScheduleStaff, 3] == "0")
                {
                    if (CheckNightAndHolidayFlag(strTargetDate) == true)
                        continue;
                }

                // --- 判定ロジック(ここまで) ---

                // 対象職員の対象日データを初期化
                for (int i2 = 0; i2 < frmMainSchedule.piWorkKindCount; i2++)
                {
                    if (frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, i2] == 1)
                    {
                        frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, i2] = 0;
                        CheckWorkKindForRowTotalData(iRandomScheduleStaff, i2, -1);
                        CheckWorkKindForColumnTotalData(iRandomDay, i2, -1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);
                    }
                }

                // ランダムで作成されたデータをセット
                frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, iRandomWorkKind] = 1;
                frmMainSchedule.aiDataNow[iRandomScheduleStaff, iRandomDay] = iRandomWorkKind;
                CheckWorkKindForRowTotalData(iRandomScheduleStaff, iRandomWorkKind, 1);
                CheckWorkKindForColumnTotalData(iRandomDay, iRandomWorkKind, 1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);

                if (iRandomDay < frmMainSchedule.piDayCount - 1)
                {
                    // 対象職員の対象日の翌日のデータを初期化
                    for (int i2 = 0; i2 < frmMainSchedule.piWorkKindCount; i2++)
                    {
                        if (frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay + 1, i2] == 1)
                        {
                            frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay + 1, i2] = 0;
                            CheckWorkKindForRowTotalData(iRandomScheduleStaff, i2, -1);
                            CheckWorkKindForColumnTotalData(iRandomDay + 1, i2, -1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);
                        }
                    }

                    // ランダムで作成されたデータをセット
                    frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay + 1, iRandomWorkKind + 1] = 1;
                    frmMainSchedule.aiDataNow[iRandomScheduleStaff, iRandomDay + 1] = iRandomWorkKind + 1;
                    CheckWorkKindForRowTotalData(iRandomScheduleStaff, iRandomWorkKind + 1, 1);
                    CheckWorkKindForColumnTotalData(iRandomDay + 1, iRandomWorkKind + 1, 1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);

                }

                if (iRandomDay < frmMainSchedule.piDayCount - 2)
                {
                    // 対象職員の対象日の翌日のデータを初期化
                    for (int i2 = 0; i2 < frmMainSchedule.piWorkKindCount; i2++)
                    {
                        if (frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay + 2, i2] == 1)
                        {
                            frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay + 2, i2] = 0;
                            CheckWorkKindForRowTotalData(iRandomScheduleStaff, i2, -1);
                            CheckWorkKindForColumnTotalData(iRandomDay + 2, i2, -1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);
                        }
                    }

                    // ランダムで作成されたデータをセット
                    frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay + 2, iRandomWorkKind + 2] = 1;
                    frmMainSchedule.aiDataNow[iRandomScheduleStaff, iRandomDay + 2] = iRandomWorkKind + 2;
                    CheckWorkKindForRowTotalData(iRandomScheduleStaff, iRandomWorkKind + 2, 1);
                    CheckWorkKindForColumnTotalData(iRandomDay + 2, iRandomWorkKind + 2, 1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);
                }
            }
            // **************************************************************************************************

            // **************************************************************************************************
            // ランダムデータの作成（土日祝の夜勤）
            for (int i = 0; i < 30000; i++)
            {
                // 乱数データのセット
                r = MyRandom.Create();
                iRandomScheduleStaff = r.Next() % frmMainSchedule.piScheduleStaffCount;
                iRandomDay = r.Next() % frmMainSchedule.piDayCount;
                iRandomWorkKind = 1;

                // 対象日を文字列としてセット(YYYYMMDD)
                strTargetDate = frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", (iRandomDay + 1)).ToString();

                // 対象日に当てはまる曜日マスタの配列番号をセット
                iDayOfWeekNum = GetDayOfWeekNum(clsCommonControl.GetWeekName(strTargetDate, frmMainSchedule.astrHoliday));

                // --- 判定ロジック(ここから) ---

                // 対象日が土日祝、またはその前日の場合
                if (clsMainScheduleCheckControl.CheckHolidayAndBeforeAfterDayForNight(strTargetDate, frmMainSchedule.astrHoliday) == false)
                continue;

                // 常日勤職員チェック
                if (clsMainScheduleCheckControl.CheckDayOnlyShift(frmMainSchedule.astrStaffDayOnly, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0], clsCommonControl.GetTargetDateChangeFormat(strTargetDate)))
                    continue;

                // 対象日が日勤かチェック
                if (frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, frmMainSchedule.WORK_KIND_DAY_ARRAY_NUM] != 1)
                    continue;

                // 希望シフトチェック
                if (frmMainSchedule.aiDataRequestFlag[iRandomScheduleStaff, iRandomDay] == 1) continue;

                // 前月末の夜勤チェック
                if (frmMainSchedule.aiNightLastMonthFlag[iRandomScheduleStaff, iRandomDay] == 1) continue;

                // 勤務種類の並び確認
                if (clsMainScheduleCheckControl.CheckWorkKindOrder(iRandomScheduleStaff, iRandomDay, frmMainSchedule.aiDataNow) == false) continue;

                // 対象職員の勤務日数チェック
                if (clsMainScheduleCheckControl.CheckWorkRowCount(iRandomScheduleStaff, iRandomDay, iRandomWorkKind, frmMainSchedule.aiData, frmMainSchedule.adRowTotalData) == false) continue;

                // 日勤の最低人数を下回らないかチェック
                if (clsMainScheduleCheckControl.CheckCountLimitDayMinDayForNight(iRandomDay, iRandomScheduleStaff, iDayOfWeekNum) == false)
                { continue; }

                // 夜勤の最高人数を上回らないかチェック
                if (clsMainScheduleCheckControl.CheckCountLimitDayMaxNight(iRandomDay, iRandomScheduleStaff, iDayOfWeekNum) == false)
                { continue; }

                // 夜勤のセットが可能か判定
                if (clsMainScheduleCheckControl.CheckWorkKindOrderForNightHoliday(iRandomScheduleStaff, iRandomDay, clsCommonControl.GetTargetMonthDays(frmMainSchedule.lblTargetMonth.Text),
                    frmMainSchedule.aiDataNow, frmMainSchedule.aiDataRequestFlag) == false) continue;

                // 夜勤の最小数の職員か確認
                if (clsMainScheduleCheckControl.CheckMinNightFlag(iRandomScheduleStaff, frmMainSchedule.adRowTotalData, frmMainSchedule.astrScheduleStaff, frmMainSchedule.astrStaffDayOnly,
                    clsCommonControl.GetTargetDateChangeFormat(strTargetDate)) == false)
                    continue;

                // 公休数チェック（夜勤用）
                if (clsMainScheduleCheckControl.CheckWorkHolidayForNight(iRandomScheduleStaff, iRandomDay) == false) continue;

                // 常日勤なしで土日祝タブー職員の場合、休みが土日祝になるかチェック
                if (frmMainSchedule.astrStaffDayOnly[iRandomScheduleStaff, 3] == "0")
                {
                    if (CheckNightAndHolidayFlag(strTargetDate) == true)
                        continue;
                }

                // --- 判定ロジック(ここまで) ---

                // 対象職員の対象日データを初期化
                for (int i2 = 0; i2 < frmMainSchedule.piWorkKindCount; i2++)
                {
                    if (frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, i2] == 1)
                    {
                        frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, i2] = 0;
                        CheckWorkKindForRowTotalData(iRandomScheduleStaff, i2, -1);
                        CheckWorkKindForColumnTotalData(iRandomDay, i2, -1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);
                    }
                }

                // ランダムで作成されたデータをセット
                frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, iRandomWorkKind] = 1;
                frmMainSchedule.aiDataNow[iRandomScheduleStaff, iRandomDay] = iRandomWorkKind;
                CheckWorkKindForRowTotalData(iRandomScheduleStaff, iRandomWorkKind, 1);
                CheckWorkKindForColumnTotalData(iRandomDay, iRandomWorkKind, 1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);

                if (iRandomDay < frmMainSchedule.piDayCount - 1)
                {
                    // 対象職員の対象日の翌日のデータを初期化
                    for (int i2 = 0; i2 < frmMainSchedule.piWorkKindCount; i2++)
                    {
                        if (frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay + 1, i2] == 1)
                        {
                            frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay + 1, i2] = 0;
                            CheckWorkKindForRowTotalData(iRandomScheduleStaff, i2, -1);
                            CheckWorkKindForColumnTotalData(iRandomDay + 1, i2, -1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);
                        }
                    }

                    // ランダムで作成されたデータをセット
                    frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay + 1, iRandomWorkKind + 1] = 1;
                    frmMainSchedule.aiDataNow[iRandomScheduleStaff, iRandomDay + 1] = iRandomWorkKind + 1;
                    CheckWorkKindForRowTotalData(iRandomScheduleStaff, iRandomWorkKind + 1, 1);
                    CheckWorkKindForColumnTotalData(iRandomDay + 1, iRandomWorkKind + 1, 1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);

                }

                if (iRandomDay < frmMainSchedule.piDayCount - 2)
                {
                    // 対象職員の対象日の翌日のデータを初期化
                    for (int i2 = 0; i2 < frmMainSchedule.piWorkKindCount; i2++)
                    {
                        if (frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay + 2, i2] == 1)
                        {
                            frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay + 2, i2] = 0;
                            CheckWorkKindForRowTotalData(iRandomScheduleStaff, i2, -1);
                            CheckWorkKindForColumnTotalData(iRandomDay + 2, i2, -1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);
                        }
                    }

                    // ランダムで作成されたデータをセット
                    frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay + 2, iRandomWorkKind + 2] = 1;
                    frmMainSchedule.aiDataNow[iRandomScheduleStaff, iRandomDay + 2] = iRandomWorkKind + 2;
                    CheckWorkKindForRowTotalData(iRandomScheduleStaff, iRandomWorkKind + 2, 1);
                    CheckWorkKindForColumnTotalData(iRandomDay + 2, iRandomWorkKind + 2, 1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);
                }
            }
            // **************************************************************************************************

            // **************************************************************************************************
            // ランダムデータの作成(土曜の半日設定）
            // ※看護師　半日２名：１病棟
            // ※ケア　　半日１名：１病棟、４病棟
            switch (frmMainSchedule.cmbWard.SelectedValue)
            {
                case "01":
                case "04":
                    iHalfNurseCount = 2;
                    iHalfCareCount = 1;
                    if (frmMainSchedule.cmbWard.SelectedValue.ToString() == "01" ||
                        (frmMainSchedule.cmbWard.SelectedValue.ToString() == "04" && frmMainSchedule.pstrStaffKind == "02"))
                    {
                        for (int i = 0; i < 10000; i++)
                        {
                            r = MyRandom.Create();
                            iRandomScheduleStaff = r.Next() % frmMainSchedule.piScheduleStaffCount;
                            iRandomDay = r.Next() % frmMainSchedule.piDayCount;
                            iRandomWorkKind = 5;

                            // 対象日を文字列としてセット(YYYYMMDD)
                            strTargetDate = frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", (iRandomDay + 1)).ToString();

                            // --- 判定ロジック(ここから) ---

                            // 対象日が土曜日かチェック
                            if (clsCommonControl.GetWeekName(strTargetDate, frmMainSchedule.astrHoliday) != "土") continue;

                            // データチェック
                            if (CheckMainData(iRandomScheduleStaff, iRandomDay) == false) continue;

                            // 希望シフトチェック
                            if (frmMainSchedule.aiDataRequestFlag[iRandomScheduleStaff, iRandomDay] == 1) continue;

                            // 前月末の夜勤チェック
                            if (frmMainSchedule.aiNightLastMonthFlag[iRandomScheduleStaff, iRandomDay] == 1) continue;

                            // 公休チェック
                            if (clsMainScheduleCheckControl.CheckWorkHoliday(iRandomScheduleStaff) == false) continue;

                            // 人数チェック（看護師）
                            if (frmMainSchedule.pstrStaffKind == "01")
                            {
                                if (clsMainScheduleCheckControl.CheckHalfWorkForSaturday(iRandomDay, iHalfNurseCount) == false) continue;
                            }
                            else
                            {
                                if (clsMainScheduleCheckControl.CheckHalfWorkForSaturday(iRandomDay, iHalfCareCount) == false) continue;
                            }


                            // --- 判定ロジック(ここまで) ---

                            // 対象職員の対象日データを一度初期化
                            for (int i2 = 0; i2 < frmMainSchedule.piWorkKindCount; i2++)
                            {
                                if (frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, i2] == 1)
                                {
                                    frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, i2] = 0;
                                    CheckWorkKindForRowTotalData(iRandomScheduleStaff, i2, -1);
                                    CheckWorkKindForColumnTotalData(iRandomDay, i2, -1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);
                                }
                            }

                            // ランダムで作成されたデータをセット
                            frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, iRandomWorkKind] = 1;
                            frmMainSchedule.aiDataNow[iRandomScheduleStaff, iRandomDay] = iRandomWorkKind;
                            CheckWorkKindForRowTotalData(iRandomScheduleStaff, iRandomWorkKind, 1);
                            CheckWorkKindForColumnTotalData(iRandomDay, iRandomWorkKind, 1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);
                        }
                    }
                    break;
                default:
                    break;
            }
            // **************************************************************************************************

            // **************************************************************************************************
            // ランダムデータの作成(土日祝の公休)
            for (int i = 0; i < 20000; i++)
            {
                r = MyRandom.Create();
                iRandomScheduleStaff = r.Next() % frmMainSchedule.piScheduleStaffCount;
                iRandomDay = r.Next() % frmMainSchedule.piDayCount;
                iRandomWorkKind = 2;

                // 勤務種類の値に変換
                iRandomWorkKind = clsCommonControl.ChangeWorkKindValue(iRandomWorkKind);

                // 対象日を文字列としてセット(YYYYMMDD)
                strTargetDate = frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", (iRandomDay + 1)).ToString();

                // 対象日に当てはまる曜日マスタの配列番号をセット
                iDayOfWeekNum = GetDayOfWeekNum(clsCommonControl.GetWeekName(strTargetDate, frmMainSchedule.astrHoliday));

                // --- 判定ロジック(ここから) ---

                // 対象日が土日祝の場合
                if (clsMainScheduleCheckControl.CheckHolidayFlagForHoliday(strTargetDate, frmMainSchedule.astrHoliday) == false)
                    continue;

                // 対象日が日勤かチェック
                if (frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, frmMainSchedule.WORK_KIND_DAY_ARRAY_NUM] != 1)
                    continue;

                // データチェック
                if (CheckMainData(iRandomScheduleStaff, iRandomDay) == false) continue;

                // 希望シフトチェック
                if (frmMainSchedule.aiDataRequestFlag[iRandomScheduleStaff, iRandomDay] == 1) continue;

                // 前月末の夜勤チェック
                if (frmMainSchedule.aiNightLastMonthFlag[iRandomScheduleStaff, iRandomDay] == 1) continue;

                // 公休チェック
                if (clsMainScheduleCheckControl.CheckWorkHoliday(iRandomScheduleStaff) == false) continue;

                // 対象となる曜日の出勤数チェック
                if (clsMainScheduleCheckControl.CheckCountLimitDayMinDayForHoliday(iRandomDay, iRandomScheduleStaff, iDayOfWeekNum) == false)
                { continue; }

                // --- 判定ロジック(ここまで) ---

                // 対象職員の対象日データを一度初期化
                for (int i2 = 0; i2 < frmMainSchedule.piWorkKindCount; i2++)
                {
                    if (frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, i2] == 1)
                    {
                        frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, i2] = 0;
                        CheckWorkKindForRowTotalData(iRandomScheduleStaff, i2, -1);
                        CheckWorkKindForColumnTotalData(iRandomDay, i2, -1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);
                    }
                }

                // ランダムで作成されたデータをセット
                frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, iRandomWorkKind] = 1;
                frmMainSchedule.aiDataNow[iRandomScheduleStaff, iRandomDay] = iRandomWorkKind;
                CheckWorkKindForRowTotalData(iRandomScheduleStaff, iRandomWorkKind, 1);
                CheckWorkKindForColumnTotalData(iRandomDay, iRandomWorkKind, 1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);
            }
            // **************************************************************************************************

            // **************************************************************************************************
            // ランダムデータの作成（土日祝以外の夜勤）
            for (int i = 0; i < 30000; i++)
            {
                // 乱数データのセット
                r = MyRandom.Create();
                iRandomScheduleStaff = r.Next() % frmMainSchedule.piScheduleStaffCount;
                iRandomDay = r.Next() % frmMainSchedule.piDayCount;
                iRandomWorkKind = 1;

                // 対象日を文字列としてセット(YYYYMMDD)
                strTargetDate = frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", (iRandomDay + 1)).ToString();

                // 対象日に当てはまる曜日マスタの配列番号をセット
                iDayOfWeekNum = GetDayOfWeekNum(clsCommonControl.GetWeekName(strTargetDate, frmMainSchedule.astrHoliday));

                // --- 判定ロジック(ここから) ---

                // 対象日が土日祝、またはその前日の場合は対象外
                if (clsMainScheduleCheckControl.CheckHolidayAndBeforeAfterDayForNight(strTargetDate, frmMainSchedule.astrHoliday) == true)
                    continue;

                // 対象日が日勤かチェック
                if (frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, frmMainSchedule.WORK_KIND_DAY_ARRAY_NUM] != 1)
                    continue;

                // 常日勤職員チェック
                if (clsMainScheduleCheckControl.CheckDayOnlyShift(frmMainSchedule.astrStaffDayOnly, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0], clsCommonControl.GetTargetDateChangeFormat(strTargetDate)))
                    continue;

                // 希望シフトチェック
                if (frmMainSchedule.aiDataRequestFlag[iRandomScheduleStaff, iRandomDay] == 1) continue;

                // 前月末の夜勤チェック
                if (frmMainSchedule.aiNightLastMonthFlag[iRandomScheduleStaff, iRandomDay] == 1) continue;

                // 勤務種類の並び確認
                if (clsMainScheduleCheckControl.CheckWorkKindOrder(iRandomScheduleStaff, iRandomDay, frmMainSchedule.aiDataNow) == false) continue;

                // 対象職員の勤務日数チェック
                if (clsMainScheduleCheckControl.CheckWorkRowCount(iRandomScheduleStaff, iRandomDay, iRandomWorkKind, frmMainSchedule.aiData, frmMainSchedule.adRowTotalData) == false) continue;

                // 日勤の最低人数を下回らないかチェック
                if (clsMainScheduleCheckControl.CheckCountLimitDayMinDayForNight(iRandomDay, iRandomScheduleStaff, iDayOfWeekNum) == false)
                { continue; }

                // 夜勤の最高人数を上回らないかチェック
                if (clsMainScheduleCheckControl.CheckCountLimitDayMaxNight(iRandomDay, iRandomScheduleStaff, iDayOfWeekNum) == false)
                { continue; }

                // 夜勤のセットが可能か判定
                if (clsMainScheduleCheckControl.CheckWorkKindOrderForNight(iRandomScheduleStaff, iRandomDay, clsCommonControl.GetTargetMonthDays(frmMainSchedule.lblTargetMonth.Text),
                    frmMainSchedule.aiDataNow, frmMainSchedule.aiDataRequestFlag) == false) continue;

                // 夜勤の最小数の職員か確認
                if (clsMainScheduleCheckControl.CheckMinNightFlag(iRandomScheduleStaff, frmMainSchedule.adRowTotalData, frmMainSchedule.astrScheduleStaff, frmMainSchedule.astrStaffDayOnly,
                    clsCommonControl.GetTargetDateChangeFormat(strTargetDate)) == false)
                    continue;

                // 男性職員の場合は夜勤人数チェック
                if (frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 2] == "1")
                {
                    if (clsMainScheduleCheckControl.CheckHolidayFlagForHoliday(strTargetDate, frmMainSchedule.astrHoliday) == false)
                    {
                        if (clsMainScheduleCheckControl.CheckMenCountForHolidayNight(iRandomDay) == false)
                            continue;
                    }
                }

                // 公休数チェック（夜勤用）
                if (clsMainScheduleCheckControl.CheckWorkHolidayForNight(iRandomScheduleStaff, iRandomDay) == false) continue;

                // 常日勤なしで土日祝タブー職員の場合、休みが土日祝になるかチェック
                if (CheckNightAndHolidayFlag(strTargetDate) == true)
                {
                    if (frmMainSchedule.astrStaffDayOnly[iRandomScheduleStaff, 3] == "0")
                        continue;
                }

                // --- 判定ロジック(ここまで) ---

                // 対象職員の対象日データを初期化
                for (int i2 = 0; i2 < frmMainSchedule.piWorkKindCount; i2++)
                {
                    if (frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, i2] == 1)
                    {
                        frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, i2] = 0;
                        CheckWorkKindForRowTotalData(iRandomScheduleStaff, i2, -1);
                        CheckWorkKindForColumnTotalData(iRandomDay, i2, -1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);
                    }
                }

                // ランダムで作成されたデータをセット
                frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, iRandomWorkKind] = 1;
                frmMainSchedule.aiDataNow[iRandomScheduleStaff, iRandomDay] = iRandomWorkKind;
                CheckWorkKindForRowTotalData(iRandomScheduleStaff, iRandomWorkKind, 1);
                CheckWorkKindForColumnTotalData(iRandomDay, iRandomWorkKind, 1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);

                if (iRandomDay < frmMainSchedule.piDayCount - 1)
                {
                    // 対象職員の対象日の翌日のデータを初期化
                    for (int i2 = 0; i2 < frmMainSchedule.piWorkKindCount; i2++)
                    {
                        if (frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay + 1, i2] == 1)
                        {
                            frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay + 1, i2] = 0;
                            CheckWorkKindForRowTotalData(iRandomScheduleStaff, i2, -1);
                            CheckWorkKindForColumnTotalData(iRandomDay + 1, i2, -1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);
                        }
                    }

                    // ランダムで作成されたデータをセット
                    frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay + 1, iRandomWorkKind + 1] = 1;
                    frmMainSchedule.aiDataNow[iRandomScheduleStaff, iRandomDay + 1] = iRandomWorkKind + 1;
                    CheckWorkKindForRowTotalData(iRandomScheduleStaff, iRandomWorkKind + 1, 1);
                    CheckWorkKindForColumnTotalData(iRandomDay + 1, iRandomWorkKind + 1, 1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);
                }

                if (iRandomDay < frmMainSchedule.piDayCount - 2)
                {
                    // 対象職員の対象日の翌日のデータを初期化
                    for (int i2 = 0; i2 < frmMainSchedule.piWorkKindCount; i2++)
                    {
                        if (frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay + 2, i2] == 1)
                        {
                            frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay + 2, i2] = 0;
                            CheckWorkKindForRowTotalData(iRandomScheduleStaff, i2, -1);
                            CheckWorkKindForColumnTotalData(iRandomDay + 2, i2, -1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);
                        }
                    }

                    // ランダムで作成されたデータをセット
                    frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay + 2, iRandomWorkKind + 2] = 1;
                    frmMainSchedule.aiDataNow[iRandomScheduleStaff, iRandomDay + 2] = iRandomWorkKind + 2;
                    CheckWorkKindForRowTotalData(iRandomScheduleStaff, iRandomWorkKind + 2, 1);
                    CheckWorkKindForColumnTotalData(iRandomDay + 2, iRandomWorkKind + 2, 1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);
                }
            }
            // **************************************************************************************************

            // **************************************************************************************************
            // ランダムデータの作成（土日祝以外の夜勤）（男性人数チェックなし）
            for (int i = 0; i < 20000; i++)
            {
                // 乱数データのセット
                r = MyRandom.Create();
                iRandomScheduleStaff = r.Next() % frmMainSchedule.piScheduleStaffCount;
                iRandomDay = r.Next() % frmMainSchedule.piDayCount;
                iRandomWorkKind = 1;

                // 対象日を文字列としてセット(YYYYMMDD)
                strTargetDate = frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", (iRandomDay + 1)).ToString();

                // 対象日に当てはまる曜日マスタの配列番号をセット
                iDayOfWeekNum = GetDayOfWeekNum(clsCommonControl.GetWeekName(strTargetDate, frmMainSchedule.astrHoliday));

                // --- 判定ロジック(ここから) ---

                // 対象日が土日祝、またはその前日の場合は対象外
                if (clsMainScheduleCheckControl.CheckHolidayAndBeforeAfterDayForNight(strTargetDate, frmMainSchedule.astrHoliday) == true)
                    continue;

                // 対象日が日勤かチェック
                if (frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, frmMainSchedule.WORK_KIND_DAY_ARRAY_NUM] != 1)
                    continue;

                // 常日勤職員チェック
                if (clsMainScheduleCheckControl.CheckDayOnlyShift(frmMainSchedule.astrStaffDayOnly, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0], clsCommonControl.GetTargetDateChangeFormat(strTargetDate)))
                    continue;

                // 希望シフトチェック
                if (frmMainSchedule.aiDataRequestFlag[iRandomScheduleStaff, iRandomDay] == 1) continue;

                // 前月末の夜勤チェック
                if (frmMainSchedule.aiNightLastMonthFlag[iRandomScheduleStaff, iRandomDay] == 1) continue;

                // 勤務種類の並び確認
                if (clsMainScheduleCheckControl.CheckWorkKindOrder(iRandomScheduleStaff, iRandomDay, frmMainSchedule.aiDataNow) == false) continue;

                // 対象職員の勤務日数チェック
                if (clsMainScheduleCheckControl.CheckWorkRowCount(iRandomScheduleStaff, iRandomDay, iRandomWorkKind, frmMainSchedule.aiData, frmMainSchedule.adRowTotalData) == false) continue;

                // 日勤の最低人数を下回らないかチェック
                if (clsMainScheduleCheckControl.CheckCountLimitDayMinDayForNight(iRandomDay, iRandomScheduleStaff, iDayOfWeekNum) == false)
                { continue; }

                // 夜勤の最高人数を上回らないかチェック
                if (clsMainScheduleCheckControl.CheckCountLimitDayMaxNight(iRandomDay, iRandomScheduleStaff, iDayOfWeekNum) == false)
                { continue; }

                // 夜勤のセットが可能か判定
                if (clsMainScheduleCheckControl.CheckWorkKindOrderForNight(iRandomScheduleStaff, iRandomDay, clsCommonControl.GetTargetMonthDays(frmMainSchedule.lblTargetMonth.Text),
                    frmMainSchedule.aiDataNow, frmMainSchedule.aiDataRequestFlag) == false) continue;

                // 夜勤の最小数の職員か確認
                if (clsMainScheduleCheckControl.CheckMinNightFlag(iRandomScheduleStaff, frmMainSchedule.adRowTotalData, frmMainSchedule.astrScheduleStaff, frmMainSchedule.astrStaffDayOnly,
                    clsCommonControl.GetTargetDateChangeFormat(strTargetDate)) == false)
                    continue;

                // 公休数チェック（夜勤用）
                if (clsMainScheduleCheckControl.CheckWorkHolidayForNight(iRandomScheduleStaff, iRandomDay) == false) continue;

                // 常日勤なしで土日祝タブー職員の場合、休みが土日祝になるかチェック
                if (CheckNightAndHolidayFlag(strTargetDate) == true)
                {
                    if (frmMainSchedule.astrStaffDayOnly[iRandomScheduleStaff, 3] == "0")
                        continue;
                }

                // --- 判定ロジック(ここまで) ---

                // 対象職員の対象日データを初期化
                for (int i2 = 0; i2 < frmMainSchedule.piWorkKindCount; i2++)
                {
                    if (frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, i2] == 1)
                    {
                        frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, i2] = 0;
                        CheckWorkKindForRowTotalData(iRandomScheduleStaff, i2, -1);
                        CheckWorkKindForColumnTotalData(iRandomDay, i2, -1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);
                    }
                }

                // ランダムで作成されたデータをセット
                frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, iRandomWorkKind] = 1;
                frmMainSchedule.aiDataNow[iRandomScheduleStaff, iRandomDay] = iRandomWorkKind;
                CheckWorkKindForRowTotalData(iRandomScheduleStaff, iRandomWorkKind, 1);
                CheckWorkKindForColumnTotalData(iRandomDay, iRandomWorkKind, 1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);

                if (iRandomDay < frmMainSchedule.piDayCount - 1)
                {
                    // 対象職員の対象日の翌日のデータを初期化
                    for (int i2 = 0; i2 < frmMainSchedule.piWorkKindCount; i2++)
                    {
                        if (frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay + 1, i2] == 1)
                        {
                            frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay + 1, i2] = 0;
                            CheckWorkKindForRowTotalData(iRandomScheduleStaff, i2, -1);
                            CheckWorkKindForColumnTotalData(iRandomDay + 1, i2, -1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);
                        }
                    }

                    // ランダムで作成されたデータをセット
                    frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay + 1, iRandomWorkKind + 1] = 1;
                    frmMainSchedule.aiDataNow[iRandomScheduleStaff, iRandomDay + 1] = iRandomWorkKind + 1;
                    CheckWorkKindForRowTotalData(iRandomScheduleStaff, iRandomWorkKind + 1, 1);
                    CheckWorkKindForColumnTotalData(iRandomDay + 1, iRandomWorkKind + 1, 1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);
                }

                if (iRandomDay < frmMainSchedule.piDayCount - 2)
                {
                    // 対象職員の対象日の翌日のデータを初期化
                    for (int i2 = 0; i2 < frmMainSchedule.piWorkKindCount; i2++)
                    {
                        if (frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay + 2, i2] == 1)
                        {
                            frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay + 2, i2] = 0;
                            CheckWorkKindForRowTotalData(iRandomScheduleStaff, i2, -1);
                            CheckWorkKindForColumnTotalData(iRandomDay + 2, i2, -1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);
                        }
                    }

                    // ランダムで作成されたデータをセット
                    frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay + 2, iRandomWorkKind + 2] = 1;
                    frmMainSchedule.aiDataNow[iRandomScheduleStaff, iRandomDay + 2] = iRandomWorkKind + 2;
                    CheckWorkKindForRowTotalData(iRandomScheduleStaff, iRandomWorkKind + 2, 1);
                    CheckWorkKindForColumnTotalData(iRandomDay + 2, iRandomWorkKind + 2, 1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);
                }
            }
            // **************************************************************************************************

            // **************************************************************************************************
            // ランダムデータの作成(土日祝以外の公休)(連勤チェックあり)
            for (int i = 0; i < 30000; i++)
            {
                r = MyRandom.Create();
                iRandomScheduleStaff = r.Next() % frmMainSchedule.piScheduleStaffCount;
                iRandomDay = r.Next() % frmMainSchedule.piDayCount;
                iRandomWorkKind = 2;

                // 勤務種類の値に変換
                iRandomWorkKind = clsCommonControl.ChangeWorkKindValue(iRandomWorkKind);

                // 対象日を文字列としてセット(YYYYMMDD)
                strTargetDate = frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", (iRandomDay + 1)).ToString();

                // 対象日に当てはまる曜日マスタの配列番号をセット
                iDayOfWeekNum = GetDayOfWeekNum(clsCommonControl.GetWeekName(strTargetDate, frmMainSchedule.astrHoliday));

                // --- 判定ロジック(ここから) ---

                // 対象日が土日祝以外の場合
                if (clsMainScheduleCheckControl.CheckHolidayFlagForHoliday(strTargetDate, frmMainSchedule.astrHoliday) == true)
                    continue;

                // データチェック
                if (CheckMainData(iRandomScheduleStaff, iRandomDay) == false) continue;

                // 希望シフトチェック
                if (frmMainSchedule.aiDataRequestFlag[iRandomScheduleStaff, iRandomDay] == 1) continue;

                // 前月末の夜勤チェック
                if (frmMainSchedule.aiNightLastMonthFlag[iRandomScheduleStaff, iRandomDay] == 1) continue;

                // 公休チェック
                if (clsMainScheduleCheckControl.CheckWorkHoliday(iRandomScheduleStaff) == false) continue;

                // 対象となる曜日の出勤数チェック
                if (clsMainScheduleCheckControl.CheckCountLimitDayMinDayForHoliday(iRandomDay, iRandomScheduleStaff, iDayOfWeekNum) == false)
                { continue; }

                // 連勤チェック
                if (clsMainScheduleCheckControl.CheckContinueWorkForHoliday(iRandomScheduleStaff, iRandomDay) == false) continue;

                // --- 判定ロジック(ここまで) ---

                // 対象職員の対象日データを一度初期化
                for (int i2 = 0; i2 < frmMainSchedule.piWorkKindCount; i2++)
                {
                    if (frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, i2] == 1)
                    {
                        frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, i2] = 0;
                        CheckWorkKindForRowTotalData(iRandomScheduleStaff, i2, -1);
                        CheckWorkKindForColumnTotalData(iRandomDay, i2, -1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);
                    }
                }

                // ランダムで作成されたデータをセット
                frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, iRandomWorkKind] = 1;
                frmMainSchedule.aiDataNow[iRandomScheduleStaff, iRandomDay] = iRandomWorkKind;
                CheckWorkKindForRowTotalData(iRandomScheduleStaff, iRandomWorkKind, 1);
                CheckWorkKindForColumnTotalData(iRandomDay, iRandomWorkKind, 1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);
            }
            // **************************************************************************************************

            // **************************************************************************************************
            // ランダムデータの作成(土日祝以外の公休)(連勤チェックなし)
            for (int i = 0; i < 10000; i++)
            {
                r = MyRandom.Create();
                iRandomScheduleStaff = r.Next() % frmMainSchedule.piScheduleStaffCount;
                iRandomDay = r.Next() % frmMainSchedule.piDayCount;
                iRandomWorkKind = 2;

                // 勤務種類の値に変換
                iRandomWorkKind = clsCommonControl.ChangeWorkKindValue(iRandomWorkKind);

                // 対象日を文字列としてセット(YYYYMMDD)
                strTargetDate = frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", (iRandomDay + 1)).ToString();

                // 対象日に当てはまる曜日マスタの配列番号をセット
                iDayOfWeekNum = GetDayOfWeekNum(clsCommonControl.GetWeekName(strTargetDate, frmMainSchedule.astrHoliday));

                // --- 判定ロジック(ここから) ---

                // 対象日が土日祝以外の場合
                if (clsMainScheduleCheckControl.CheckHolidayFlagForHoliday(strTargetDate, frmMainSchedule.astrHoliday) == true)
                    continue;

                // データチェック
                if (CheckMainData(iRandomScheduleStaff, iRandomDay) == false) continue;

                // 希望シフトチェック
                if (frmMainSchedule.aiDataRequestFlag[iRandomScheduleStaff, iRandomDay] == 1) continue;

                // 前月末の夜勤チェック
                if (frmMainSchedule.aiNightLastMonthFlag[iRandomScheduleStaff, iRandomDay] == 1) continue;

                // 公休チェック
                if (clsMainScheduleCheckControl.CheckWorkHoliday(iRandomScheduleStaff) == false) continue;

                // 対象となる曜日の出勤数チェック
                if (clsMainScheduleCheckControl.CheckCountLimitDayMinDayForHoliday(iRandomDay, iRandomScheduleStaff, iDayOfWeekNum) == false)
                { continue; }

                // --- 判定ロジック(ここまで) ---

                // 対象職員の対象日データを一度初期化
                for (int i2 = 0; i2 < frmMainSchedule.piWorkKindCount; i2++)
                {
                    if (frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, i2] == 1)
                    {
                        frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, i2] = 0;
                        CheckWorkKindForRowTotalData(iRandomScheduleStaff, i2, -1);
                        CheckWorkKindForColumnTotalData(iRandomDay, i2, -1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);
                    }
                }

                // ランダムで作成されたデータをセット
                frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, iRandomWorkKind] = 1;
                frmMainSchedule.aiDataNow[iRandomScheduleStaff, iRandomDay] = iRandomWorkKind;
                CheckWorkKindForRowTotalData(iRandomScheduleStaff, iRandomWorkKind, 1);
                CheckWorkKindForColumnTotalData(iRandomDay, iRandomWorkKind, 1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);
            }
            // **************************************************************************************************

            // **************************************************************************************************
            // ランダムデータの作成(土日祝以外の半日設定)
            for (int i = 0; i < 5000; i++)
            {
                r = MyRandom.Create();
                iRandomScheduleStaff = r.Next() % frmMainSchedule.piScheduleStaffCount;
                iRandomDay = r.Next() % frmMainSchedule.piDayCount;
                // Mod Start WataruT 2020.07.15 土日祝以外の半日が前公になっている
                //iRandomWorkKind = 4;
                iRandomWorkKind = 5;
                // Mod End   WataruT 2020.07.15 土日祝以外の半日が前公になっている

                // 対象日を文字列としてセット(YYYYMMDD)
                strTargetDate = frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", (iRandomDay + 1)).ToString();

                // 対象日に当てはまる曜日マスタの配列番号をセット
                iDayOfWeekNum = GetDayOfWeekNum(clsCommonControl.GetWeekName(strTargetDate, frmMainSchedule.astrHoliday));

                // --- 判定ロジック(ここから) ---

                // 対象日が平日かチェック
                if (clsMainScheduleCheckControl.CheckHolidayFlagForHoliday(strTargetDate, frmMainSchedule.astrHoliday) == true)
                    continue;

                // 対象日が日勤かチェック
                if (frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, frmMainSchedule.WORK_KIND_DAY_ARRAY_NUM] != 1)
                    continue;

                // 希望シフトチェック
                if (frmMainSchedule.aiDataRequestFlag[iRandomScheduleStaff, iRandomDay] == 1) continue;

                // 前月末の夜勤チェック
                if (frmMainSchedule.aiNightLastMonthFlag[iRandomScheduleStaff, iRandomDay] == 1) continue;

                // 公休チェック
                if (clsMainScheduleCheckControl.CheckWorkHolidayForHalfWork(iRandomScheduleStaff) == false) continue;

                // 対象となる曜日の出勤数チェック
                if (clsMainScheduleCheckControl.CheckCountLimitDayMinDayForHalfWork(iRandomDay, iDayOfWeekNum) == false)
                { continue; }

                // --- 判定ロジック(ここまで) ---

                // 対象職員の対象日データを一度初期化
                for (int i2 = 0; i2 < frmMainSchedule.piWorkKindCount; i2++)
                {
                    if (frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, i2] == 1)
                    {
                        frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, i2] = 0;
                        CheckWorkKindForRowTotalData(iRandomScheduleStaff, i2, -1);
                        CheckWorkKindForColumnTotalData(iRandomDay, i2, -1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);
                    }
                }

                // ランダムで作成されたデータをセット
                frmMainSchedule.aiData[iRandomScheduleStaff, iRandomDay, iRandomWorkKind] = 1;
                frmMainSchedule.aiDataNow[iRandomScheduleStaff, iRandomDay] = iRandomWorkKind;
                CheckWorkKindForRowTotalData(iRandomScheduleStaff, iRandomWorkKind, 1);
                CheckWorkKindForColumnTotalData(iRandomDay, iRandomWorkKind, 1, frmMainSchedule.astrScheduleStaff[iRandomScheduleStaff, 0]);
            }
            // **************************************************************************************************

            // DataTableの初期化
            DataTable dt = new DataTable();

            // DataTableにカラムヘッダを作成
            dt.Columns.Add("NAME", Type.GetType("System.String"));
            for (int iDay = 1; iDay <= frmMainSchedule.piDayCount; iDay++)
            {
                dt.Columns.Add(iDay.ToString(), Type.GetType("System.String"));
            }

            // DataTableにデータをセット
            for (int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.piScheduleStaffCount; iScheduleStaff++)
            {
                DataRow nr = dt.NewRow();

                // 職員氏名
                nr["NAME"] = frmMainSchedule.astrScheduleStaff[iScheduleStaff, 1];

                // 勤務種類
                for (int iDay = 1; iDay <= frmMainSchedule.piDayCount; iDay++)
                {
                    for (int iWorkKind = 0; iWorkKind < frmMainSchedule.piWorkKindCount; iWorkKind++)
                    {
                        if (frmMainSchedule.aiData[iScheduleStaff, iDay - 1, iWorkKind] == 1)
                        {
                            // 対象となる勤務種類(略称)
                            nr[iDay.ToString()] = frmMainSchedule.astrWorkKind[iWorkKind, 1];
                            break;
                        }
                    }
                }
                dt.Rows.Add(nr);
            }

            // メイングリッドにデータをセット
            frmMainSchedule.grdMain.DataSource = dt;

            // デザイン設定(列：職員)
            for (int i = 0; i < frmMainSchedule.piScheduleStaffCount; i++)
            {
                frmMainSchedule.grdMain[0, i].Style.ForeColor = clsCommonControl.GetSexForeColor(frmMainSchedule.astrScheduleStaff[i, 2]);
                frmMainSchedule.grdMain[0, i].Style.BackColor = SystemColors.Control;
            }

            // 列幅(職員)
            frmMainSchedule.grdMain.Columns[0].Width = GRID_WIDTH_COLUMN_STAFF;

            // デザイン設定(列：日付)
            for (int i = 1; i <= frmMainSchedule.piDayCount; i++)
            {
                // 列幅
                frmMainSchedule.grdMain.Columns[i].Width = GRID_WIDTH_COLUMN_DATA;

                // 文字色、背景色
                for (int i2 = 0; i2 < frmMainSchedule.piScheduleStaffCount; i2++)
                {
                    frmMainSchedule.grdMain[i, i2].Style.ForeColor = clsCommonControl.GetWorkKindForeColor(frmMainSchedule.grdMain[i, i2].Value.ToString());
                    frmMainSchedule.grdMain[i, i2].Style.BackColor = clsCommonControl.GetWeekNameBackgroundColor(
                        clsCommonControl.GetWeekName(frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", i), frmMainSchedule.astrHoliday));
                }
            }

            // 希望シフトのセルのみ太文字にして背景色も変更する
            for (int iDay = 0; iDay < frmMainSchedule.grdMain.ColumnCount - 1; iDay++)
            {
                for (int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.grdMain.RowCount; iScheduleStaff++)
                {
                    if (frmMainSchedule.aiDataRequestFlag[iScheduleStaff, iDay] == 1)
                    {
                        frmMainSchedule.grdMain[iDay + 1, iScheduleStaff].Style.Font = new Font(frmMainSchedule.grdMain.Font, FontStyle.Bold);
                        frmMainSchedule.grdMain[iDay + 1, iScheduleStaff].Style.BackColor = Color.Gold;
                    }
                }
            }

            // 先頭列のみ固定とする
            frmMainSchedule.grdMain.Columns[0].Frozen = true;

            // 行の合計グリッドをセット
            SetRowTotal();

            // 列の合計グリッドをセット
            SetColumnTotal();

            //グリッドの描画再開
            frmMainSchedule.grdMain.ResumeLayout();
        }

        /// <summary>
        /// 希望シフトの取込のみ処理
        /// Add WataruT 2020.07.16 希望シフトのみ取込機能追加
        /// </summary>
        public void ImportRequestData()
        {
            DataTable dtScheduleDetail;
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

            // 初期データをセット
            for (int iDay = 0; iDay < frmMainSchedule.piDayCount; iDay++)
            {
                for (int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.piScheduleStaffCount; iScheduleStaff++)
                {
                    for (int iWorkKind = 0; iWorkKind < frmMainSchedule.piWorkKindCount; iWorkKind++)
                    {
                        frmMainSchedule.aiData[iScheduleStaff, iDay, iWorkKind] = 0;
                        frmMainSchedule.aiDataRequestFlag[iScheduleStaff, iDay] = 0;
                        if (iWorkKind < 3)
                            frmMainSchedule.adRowTotalData[iScheduleStaff, iWorkKind] = 0;
                    }
                }
                for (int iWorkKind = 0; iWorkKind < 6; iWorkKind++)
                {
                    frmMainSchedule.adColumnTotalData[iDay, iWorkKind] = 0;
                }
            }

            // 希望シフトをセット
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
                frmMainSchedule.grdMainHeader.Columns[0].Width = GRID_WIDTH_COLUMN_STAFF;
                frmMainSchedule.grdMainHeader[0, iRow].Style.ForeColor = Color.Black;
                frmMainSchedule.grdMainHeader[0, iRow].Style.BackColor = SystemColors.Control;

                for (int iColumn = 1; iColumn <= frmMainSchedule.piDayCount; iColumn++)
                {
                    // 列幅
                    frmMainSchedule.grdMainHeader.Columns[iColumn].Width = GRID_WIDTH_COLUMN_DATA;

                    // 色(日付、曜日)
                    frmMainSchedule.grdMainHeader[iColumn, iRow].Style.ForeColor = clsCommonControl.GetWeekNameForeColor(frmMainSchedule.grdMainHeader[iColumn, iRow].Value.ToString());
                    frmMainSchedule.grdMainHeader[iColumn, iRow].Style.BackColor = clsCommonControl.GetWeekNameBackgroundColor(clsCommonControl.GetWeekName(
                        frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", iColumn), frmMainSchedule.astrHoliday));
                }
            }

            //
            // --- メイングリッドデータ ---
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
            for (int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.piScheduleStaffCount; iScheduleStaff++)
            {
                dr = dt.NewRow();

                dr["NAME"] = frmMainSchedule.astrScheduleStaff[iScheduleStaff, 1];

                // 病棟、職員ID、職種、対象年月から勤務予定データを取得
                dtScheduleDetail = clsDatabaseControl.GetScheduleDetail_Ward_Staff_StaffKind_TargetMonth(frmMainSchedule.cmbWard.SelectedValue.ToString(),
                    frmMainSchedule.astrScheduleStaff[iScheduleStaff, 0], frmMainSchedule.pstrStaffKind, frmMainSchedule.pstrTargetMonth);

                // 既存データがある場合
                if (dtScheduleDetail.Rows.Count != 0)
                {
                    for (int iDay = 1; iDay <= frmMainSchedule.piDayCount; iDay++)
                    {
                        // 希望シフトのデータの場合
                        if (frmMainSchedule.aiDataRequestFlag[iScheduleStaff, iDay - 1] == 1)
                        {
                            for (int iWorkKind = 0; iWorkKind < frmMainSchedule.piWorkKindCount; iWorkKind++)
                            {
                                if (frmMainSchedule.aiData[iScheduleStaff, iDay - 1, iWorkKind] == 1)
                                {
                                    // 対象となる勤務種類(略称)
                                    dr[iDay.ToString()] = frmMainSchedule.astrWorkKind[iWorkKind, 1];
                                    break;
                                }
                            }
                        }
                        else
                        {
                            dr[iDay.ToString()] = dtScheduleDetail.Rows[iDay - 1]["name_short"];
                            frmMainSchedule.aiData[iScheduleStaff, iDay - 1, int.Parse(dtScheduleDetail.Rows[iDay - 1]["work_kind"].ToString()) - 1] = 1;
                            frmMainSchedule.aiDataRequestFlag[iScheduleStaff, iDay - 1] = 0;
                            CheckWorkKindForRowTotalData(iScheduleStaff, int.Parse(dtScheduleDetail.Rows[iDay - 1]["work_kind"].ToString()) - 1, 1);
                            CheckWorkKindForColumnTotalData(iDay - 1, int.Parse(dtScheduleDetail.Rows[iDay - 1]["work_kind"].ToString()) - 1, 1, frmMainSchedule.astrScheduleStaff[iScheduleStaff, 0]);
                        }
                    }
                }
                // 既存データがない場合
                else
                {
                    for (int iDay = 1; iDay <= frmMainSchedule.piDayCount; iDay++)
                    {
                        dr[iDay.ToString()] = frmMainSchedule.astrWorkKind[0, 1];
                        frmMainSchedule.aiData[iScheduleStaff, iDay - 1, 0] = 1;
                        frmMainSchedule.aiDataRequestFlag[iScheduleStaff, iDay - 1] = 0;
                        CheckWorkKindForRowTotalData(iScheduleStaff, 0, 1);
                        CheckWorkKindForColumnTotalData(iDay - 1, 0, 1, frmMainSchedule.astrScheduleStaff[iScheduleStaff, 0]);
                    }
                }

                dt.Rows.Add(dr);
            }

            // メイングリッドにデータをセット
            frmMainSchedule.grdMain.DataSource = dt;

            // 職員氏名欄のデザイン設定
            for (int i = 0; i < frmMainSchedule.piScheduleStaffCount; i++)
            {
                frmMainSchedule.grdMain[0, i].Style.ForeColor = clsCommonControl.GetSexForeColor(clsDatabaseControl.GetStaff_Sex(frmMainSchedule.astrScheduleStaff[i, 0]));
                frmMainSchedule.grdMain[0, i].Style.BackColor = SystemColors.Control;
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
                    frmMainSchedule.grdMain[iDay, iScheduleStaff].Style.ForeColor = clsCommonControl.GetWorkKindForeColor(frmMainSchedule.grdMain[iDay, iScheduleStaff].Value.ToString());
                    frmMainSchedule.grdMain[iDay, iScheduleStaff].Style.BackColor = clsCommonControl.GetWeekNameBackgroundColor(clsCommonControl.GetWeekName(
                        frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", iDay), frmMainSchedule.astrHoliday));
                }
            }

            // 希望シフトのセルのみ太文字にする
            for (int iDay = 0; iDay < frmMainSchedule.grdMain.ColumnCount - 1; iDay++)
            {
                for (int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.grdMain.RowCount; iScheduleStaff++)
                {
                    if (frmMainSchedule.aiDataRequestFlag[iScheduleStaff, iDay] == 1)
                    {
                        frmMainSchedule.grdMain[iDay + 1, iScheduleStaff].Style.Font = new Font(frmMainSchedule.grdMain.Font, FontStyle.Bold);
                        frmMainSchedule.grdMain[iDay + 1, iScheduleStaff].Style.BackColor = Color.Gold;
                    }
                }
            }

            // 先頭列のみ固定とする
            if (frmMainSchedule.grdMain.Rows.Count > 0)
            {
                frmMainSchedule.grdMain.Rows[0].Frozen = true;
            }

            // 表示していない職種の列合計値を取得
            SetColumnTotalOtherStaffKindCount();

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
            DataTable dt;
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
                frmMainSchedule.grdRowTotalHeader.Columns[iColumn].Width = GRID_WIDTH_COLUMN_DATA_ROWTOTAL;

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
                frmMainSchedule.grdRowTotal.Columns[iWorkKind].Width = GRID_WIDTH_COLUMN_DATA_ROWTOTAL;
                //ソートモード
                frmMainSchedule.grdRowTotal.Columns[iWorkKind].SortMode = DataGridViewColumnSortMode.NotSortable;

                for(int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.piScheduleStaffCount; iScheduleStaff++)
                {
                    frmMainSchedule.grdRowTotal[iWorkKind, iScheduleStaff].Style.BackColor = clsCommonControl.GetRowTotalBackColor(
                         double.Parse(frmMainSchedule.grdRowTotal[iWorkKind, iScheduleStaff].Value.ToString()), frmMainSchedule.pdHolidayCount, iWorkKind);
                }
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
            string strColumnTotal;          // 対象セルの合計値
            string strCountLimitDay;        // 制限値の文字列用変数

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
            for (int iWorkKind = 0; iWorkKind < 6; iWorkKind++)
            {
                DataRow dr = dt.NewRow();
                dr["NAME"] = clsCommonControl.GetWorkKindTotalName(iWorkKind);

                for (int iDay = 1; iDay < frmMainSchedule.piDayCount + 1; iDay++)
                {
                    // 曜日の配列番号を取得
                    int iDayOfWeekNum = GetDayOfWeekNum(clsCommonControl.GetWeekName(frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", iDay), frmMainSchedule.astrHoliday));

                    // 制限値の文言をセット(日勤)
                    if (iWorkKind == 0)
                    {
                        // 6病棟で土日祝かつ救急指定日の場合
                        // Mod Start WataruT 2020.07.22 救急指定日の人数調整処理の不具合
                        //if (frmMainSchedule.cmbWard.SelectedValue.ToString() == "06" &&
                        if (frmMainSchedule.cmbWard.SelectedValue.ToString() == "06" && frmMainSchedule.pstrStaffKind == "01" &&
                        (iDayOfWeekNum == 0 || iDayOfWeekNum == 6 || iDayOfWeekNum == 7) &&
                        CheckEmergencyDate(String.Format("{0:D2}", iDay)))
                        // Mod End   WataruT 2020.07.22 救急指定日の人数調整処理の不具合
                        {
                            strCountLimitDay = "/" + (int.Parse(frmMainSchedule.astrCountLimitDay[iDayOfWeekNum, 0]) + 1).ToString();
                        }
                        else
                        {
                            strCountLimitDay = "/" + frmMainSchedule.astrCountLimitDay[iDayOfWeekNum, 0];
                        }   
                    }
                    // 制限値の文言をセット(夜勤)
                    else if (iWorkKind == 1 || iWorkKind == 2)
                    {
                        strCountLimitDay = "/" + frmMainSchedule.astrCountLimitDay[iDayOfWeekNum, 1];
                    }
                    // 制限値の文言不要（男性職員）
                    else
                        strCountLimitDay = "";

                    dr["DAY" + iDay.ToString()] = frmMainSchedule.adColumnTotalData[iDay - 1, iWorkKind] + strCountLimitDay;
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
                // 列幅
                frmMainSchedule.grdColumnTotal.Columns[iDay].Width = GRID_WIDTH_COLUMN_DATA;
                // ソートモード
                frmMainSchedule.grdColumnTotal.Columns[iDay].SortMode = DataGridViewColumnSortMode.NotSortable;
                
                for(int iWorkKind = 0; iWorkKind < 6; iWorkKind++)
                {
                    if(iWorkKind <= 2)
                    {
                        strColumnTotal = frmMainSchedule.grdColumnTotal[iDay, iWorkKind].Value.ToString().Substring(0,
                        frmMainSchedule.grdColumnTotal[iDay, iWorkKind].Value.ToString().IndexOf("/"));
                        strCountLimitDay = frmMainSchedule.grdColumnTotal[iDay, iWorkKind].Value.ToString().Substring(
                            frmMainSchedule.grdColumnTotal[iDay, iWorkKind].Value.ToString().IndexOf("/") + 1,
                            frmMainSchedule.grdColumnTotal[iDay, iWorkKind].Value.ToString().Length - 1 - frmMainSchedule.grdColumnTotal[iDay, iWorkKind].Value.ToString().IndexOf("/"));
                    }
                    else
                    {
                        strColumnTotal = frmMainSchedule.grdColumnTotal[iDay, iWorkKind].Value.ToString();
                        strCountLimitDay = frmMainSchedule.grdColumnTotal[iDay, iWorkKind].Value.ToString();
                    }

                    // 背景色
                    // Mod Start WataruT 2020.07.30 土日祝の日勤計の色変更
                    //frmMainSchedule.grdColumnTotal[iDay, iWorkKind].Style.BackColor = clsCommonControl.GetColumnTotalBackColor(double.Parse(strColumnTotal), int.Parse(strCountLimitDay), iWorkKind);
                    frmMainSchedule.grdColumnTotal[iDay, iWorkKind].Style.BackColor = clsCommonControl.GetColumnTotalBackColor(double.Parse(strColumnTotal), int.Parse(strCountLimitDay), iWorkKind,
                        clsCommonControl.GetWeekName(frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", iDay), frmMainSchedule.astrHoliday));
                    // Mod End   WataruT 2020.07.30 土日祝の日勤計の色変更
                }
            }

            // フォント変更
            frmMainSchedule.grdColumnTotal.Columns[0].DefaultCellStyle.Font = new Font("メイリオ", 9);
            for (int iColumn = 1; iColumn < frmMainSchedule.grdColumnTotal.Columns.Count; iColumn++)
            {
                frmMainSchedule.grdColumnTotal.Columns[iColumn].DefaultCellStyle.Font = new Font("ＭＳ Ｐゴシック", 8);
            }

            // グリッドの選択状態を解除
            frmMainSchedule.grdColumnTotal.CurrentCell = null;

            // グリッドの描画処理停止
            frmMainSchedule.grdColumnTotal.Columns[0].Frozen = true;
            frmMainSchedule.grdColumnTotal.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
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
        private void CheckWorkKindForColumnTotalData(int iDay, int iWorkKind, double dAddNumber, string strStaff = "")
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
                    if (strStaff != "")
                    {
                        // 男性の合計数も計算
                        if (clsDatabaseControl.GetStaff_Sex(strStaff) == "1")
                            frmMainSchedule.adColumnTotalData[iDay, 3] += dAddNumber;
                        else
                            frmMainSchedule.adColumnTotalData[iDay, 4] += dAddNumber;

                        // ナースレベル判定
                        if (clsDatabaseControl.GetStaffDayOnly_StaffLevel(strStaff) != "1")
                            frmMainSchedule.adColumnTotalData[iDay, 5] += dAddNumber;
                    }
                    break;
                case 2: // 夜明
                    frmMainSchedule.adColumnTotalData[iDay, 2] += dAddNumber;
                    break;

            }
        }

        /// <summary>
        /// 土日祝タブー職員の公休を土日祝にセット
        /// </summary>
        private void SetHolidayData()
        {
            string strHoliday;
            for (int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.piScheduleStaffCount; iScheduleStaff++)
            {
                for (int iStaffDayOnly = 0; iStaffDayOnly < frmMainSchedule.astrStaffDayOnly.GetLength(0); iStaffDayOnly++)
                {
                    if (frmMainSchedule.astrScheduleStaff[iScheduleStaff, 0] == frmMainSchedule.astrStaffDayOnly[iStaffDayOnly, 0])
                    {
                        if (frmMainSchedule.astrStaffDayOnly[iStaffDayOnly, 3] == "0")
                        {
                            for (int iDay = 0; iDay < frmMainSchedule.piDayCount; iDay++)
                            {
                                strHoliday = clsCommonControl.GetWeekName(frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", iDay + 1), frmMainSchedule.astrHoliday);

                                if (strHoliday == "土" || strHoliday == "日" || strHoliday == "祝")
                                {
                                    for (int iWorkKind = 0; iWorkKind < frmMainSchedule.piWorkKindCount; iWorkKind++)
                                    {
                                        // 対象職員の対象日の予定データを初期化
                                        if (frmMainSchedule.aiData[iScheduleStaff, iDay, iWorkKind] == 1)
                                        {
                                            frmMainSchedule.aiData[iScheduleStaff, iDay, iWorkKind] = 0;
                                            CheckWorkKindForRowTotalData(iScheduleStaff, iWorkKind, -1);
                                            CheckWorkKindForColumnTotalData(iDay, iWorkKind, -1, frmMainSchedule.astrScheduleStaff[iScheduleStaff, 0]);
                                        }
                                    }

                                    // 公休を希望シフトとしてセット
                                    frmMainSchedule.aiData[iScheduleStaff, iDay, frmMainSchedule.WORK_KIND_PUBLIC_HOLIDAY_ARRAY_NUM] = 1;
                                    CheckWorkKindForRowTotalData(iScheduleStaff, frmMainSchedule.WORK_KIND_PUBLIC_HOLIDAY_ARRAY_NUM, 1);
                                    CheckWorkKindForColumnTotalData(iDay, frmMainSchedule.WORK_KIND_PUBLIC_HOLIDAY_ARRAY_NUM, 1, frmMainSchedule.astrScheduleStaff[iScheduleStaff, 0]);
                                }
                            }
                        }
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// メイングリッド用データのチェック
        /// </summary>
        /// <returns></returns>
        private bool CheckMainData(int iScheduleStaff, int iDay)
        {
            // 希望シフトチェック
            if (frmMainSchedule.aiDataRequestFlag[iScheduleStaff, iDay] == 1) return false;

            // 勤務種類の並び確認
            if (clsMainScheduleCheckControl.CheckWorkKindOrder(iScheduleStaff, iDay, frmMainSchedule.aiDataNow) == false)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 常日勤なしで土日祝タブー職員の場合、休みが土日祝になるかチェック
        /// </summary>
        /// <param name="strTargetDate"></param>
        /// <returns></returns>
        private bool CheckNightAndHolidayFlag(string strTargetDate)
        {
            DateTime dt = DateTime.ParseExact(strTargetDate, "yyyyMMdd", null);
            string strTargetDayOfWeek;

            // 対象日
            strTargetDayOfWeek = clsCommonControl.GetWeekName(dt.ToString("yyyyMMdd"), frmMainSchedule.astrHoliday);

            if (strTargetDayOfWeek == "土" || strTargetDayOfWeek == "日" || strTargetDayOfWeek == "祝")
                return true;

            // 対象日 + 1
            dt = dt.AddDays(1);
            strTargetDayOfWeek = clsCommonControl.GetWeekName(dt.ToString("yyyyMMdd"), frmMainSchedule.astrHoliday);

            if (strTargetDayOfWeek == "土" || strTargetDayOfWeek == "日" || strTargetDayOfWeek == "祝")
                return true;

            // 対象日 + 2
            dt = dt.AddDays(1);
            strTargetDayOfWeek = clsCommonControl.GetWeekName(dt.ToString("yyyyMMdd"), frmMainSchedule.astrHoliday);

            if (strTargetDayOfWeek != "土" && strTargetDayOfWeek != "日" && strTargetDayOfWeek != "祝")
                return true;

            return false;
        }

        /// <summary>
        /// 曜日マスタの配列番号を取得
        /// </summary>
        /// <param name="strDayOfWeek"></param>
        /// <returns></returns>
        private int GetDayOfWeekNum(string strDayOfWeek)
        {
            for (int i = 0; i < frmMainSchedule.astrDayOfWeek.GetLength(0); i++)
            {
                if (strDayOfWeek == frmMainSchedule.astrDayOfWeek[i, 1])
                {
                    return i;
                }
            }

            return 0;
        }

        /// <summary>
        /// 勤務予定データ登録
        /// </summary>
        public void SaveScheduleData()
        {
            DataTable dtScheduleHeader;
            DataTable dtScheduleDetail;
            DataRow drScheduleHeader;
            DataRow drScheduleDetail;

            string strTargetMonthForHeader = frmMainSchedule.lblTargetMonth.Text.Substring(0, 4) + frmMainSchedule.lblTargetMonth.Text.Substring(5, 2);
            string strSQL;

            int piScheduleStaffCount = frmMainSchedule.grdMain.Rows.Count;
            int piDayCount = frmMainSchedule.grdMain.ColumnCount - 1;
            int iScheduleNo;
            int iScheduleDetailNo = 1;

            // 勤務予定データの最大値を取得
            if (int.TryParse(clsDatabaseControl.GetScheduleHeader_MaxScheduleNo(), out iScheduleNo) == false)
            {
                iScheduleNo = 1;
            }
            else
                iScheduleNo++;

            // 勤務予定ヘッダの作成
            dtScheduleHeader = clsDataTableControl.GetTable_ScheduleHeader();
            drScheduleHeader = dtScheduleHeader.NewRow();

            drScheduleHeader["schedule_no"] = iScheduleNo.ToString();
            drScheduleHeader["ward"] = frmMainSchedule.cmbWard.SelectedValue.ToString();
            drScheduleHeader["staff_kind"] = frmMainSchedule.pstrStaffKind;
            drScheduleHeader["target_month"] = strTargetMonthForHeader;

            clsDatabaseControl.InsertScheduleHeader(drScheduleHeader);

            // 勤務予定詳細のSQL作成
            strSQL = clsDatabaseControl.GetInsertScheduleDetail_Insert();
            for (int iScheduleStaff = 0; iScheduleStaff < piScheduleStaffCount; iScheduleStaff++)
            {
                for (int iDay = 1; iDay <= piDayCount; iDay++)
                {
                    dtScheduleDetail = clsDataTableControl.GetTable_ScheduleDetail();
                    drScheduleDetail = dtScheduleDetail.NewRow();

                    drScheduleDetail["schedule_no"] = iScheduleNo.ToString();
                    drScheduleDetail["schedule_detail_no"] = iScheduleDetailNo;
                    drScheduleDetail["staff"] = frmMainSchedule.astrScheduleStaff[iScheduleStaff, 0];
                    drScheduleDetail["target_date"] = clsCommonControl.GetTargetDateChangeFormat(strTargetMonthForHeader + String.Format("{0:D2}", iDay));
                    drScheduleDetail["work_kind"] = clsCommonControl.GetWorkKindID(frmMainSchedule.grdMain[iDay, iScheduleStaff].Value.ToString(), frmMainSchedule.astrWorkKind);
                    drScheduleDetail["request_flag"] = clsCommonControl.GetRequestFlag(frmMainSchedule.grdMain[iDay, iScheduleStaff].Style.BackColor);

                    strSQL += clsDatabaseControl.GetInsertScheduleDetail_Values(drScheduleDetail);

                    iScheduleDetailNo++;
                }
            }

            // 勤務予定詳細のデータをBulkInsert
            if (clsDatabaseControl.ExecuteBulkInsertSQL(strSQL) == false)
            {
                MessageBox.Show("勤務予定詳細の作成に失敗しました。");
            }
                
        }

        /// <summary>
        /// 勤務予定初回データ登録
        /// </summary>
        public void SaveScheduleFirstData()
        {
            DataTable dtScheduleHeader;
            DataTable dtScheduleDetail;
            DataRow drScheduleHeader;
            DataRow drScheduleDetail;

            string strTargetMonthForHeader = frmMainSchedule.lblTargetMonth.Text.Substring(0, 4) + frmMainSchedule.lblTargetMonth.Text.Substring(5, 2);
            string strSQL;

            int piScheduleStaffCount = frmMainSchedule.grdMain.Rows.Count;
            int piDayCount = frmMainSchedule.grdMain.ColumnCount - 1;
            int iScheduleNo;
            int iScheduleDetailNo = 1;

            // 勤務予定データの最大値を取得
            if (int.TryParse(clsDatabaseControl.GetScheduleFirstHeader_MaxScheduleNo(), out iScheduleNo) == false)
            {
                iScheduleNo = 1;
            }
            else
                iScheduleNo++;

            // 勤務予定ヘッダの作成
            dtScheduleHeader = clsDataTableControl.GetTable_ScheduleHeader();
            drScheduleHeader = dtScheduleHeader.NewRow();

            drScheduleHeader["schedule_no"] = iScheduleNo.ToString();
            drScheduleHeader["ward"] = frmMainSchedule.cmbWard.SelectedValue.ToString();
            drScheduleHeader["staff_kind"] = frmMainSchedule.pstrStaffKind;
            drScheduleHeader["target_month"] = strTargetMonthForHeader;

            clsDatabaseControl.InsertScheduleFirstHeader(drScheduleHeader);

            // 勤務予定詳細の作成
            strSQL = clsDatabaseControl.GetInsertScheduleFirstDetail_Insert();
            for (int iScheduleStaff = 0; iScheduleStaff < piScheduleStaffCount; iScheduleStaff++)
            {
                for (int iDay = 1; iDay <= piDayCount; iDay++)
                {
                    dtScheduleDetail = clsDataTableControl.GetTable_ScheduleDetail();
                    drScheduleDetail = dtScheduleDetail.NewRow();

                    drScheduleDetail["schedule_no"] = iScheduleNo.ToString();
                    drScheduleDetail["schedule_detail_no"] = iScheduleDetailNo;
                    drScheduleDetail["staff"] = frmMainSchedule.astrScheduleStaff[iScheduleStaff, 0];
                    drScheduleDetail["target_date"] = clsCommonControl.GetTargetDateChangeFormat(strTargetMonthForHeader + String.Format("{0:D2}", iDay));
                    drScheduleDetail["work_kind"] = clsCommonControl.GetWorkKindID(frmMainSchedule.grdMain[iDay, iScheduleStaff].Value.ToString(), frmMainSchedule.astrWorkKind);
                    drScheduleDetail["request_flag"] = clsCommonControl.GetRequestFlag(frmMainSchedule.grdMain[iDay, iScheduleStaff].Style.BackColor);

                    strSQL += clsDatabaseControl.GetInsertScheduleFirstDetail_Values(drScheduleDetail);

                    iScheduleDetailNo++;
                }
            }

            // 勤務初回予定詳細のデータをBulkInsert
            if (clsDatabaseControl.ExecuteBulkInsertSQL(strSQL) == false)
            {
                MessageBox.Show("勤務初回予定詳細の作成に失敗しました。");
            }
        }

        /// <summary>
        /// 希望シフトをセット
        /// </summary>
        private void SetRequestData()
        {
            DataTable dt;
            int iTargetDay;
            dt = clsDatabaseControl.GetRequestShift_Ward(frmMainSchedule.cmbWard.SelectedValue.ToString(), frmMainSchedule.pstrTargetMonth);

            // Add Start WataruT 2020.07.16 希望シフトのみ取込機能追加
            for(int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.dtScheduleStaff.Rows.Count; iScheduleStaff++)
            {
                for (int iDay = 0; iDay < frmMainSchedule.piDayCount; iDay++)
                {
                    // 対象日の希望シフトフラグを無効とする
                    frmMainSchedule.aiDataRequestFlag[iScheduleStaff, iDay] = 0;
                }
            }
            // Add End   WataruT 2020.07.16 希望シフトのみ取込機能追加

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
                                CheckWorkKindForColumnTotalData(iTargetDay, iWorkKind, -1, frmMainSchedule.astrScheduleStaff[iScheduleStaff, 0]);
                            }
                        }

                        // 希望シフトの勤務をセット
                        frmMainSchedule.aiData[iScheduleStaff, iTargetDay, int.Parse(row["work_kind"].ToString()) - 1] = 1;
                        frmMainSchedule.aiDataRequestFlag[iScheduleStaff, iTargetDay] = 1;
                        CheckWorkKindForRowTotalData(iScheduleStaff, int.Parse(row["work_kind"].ToString()) - 1, 1);
                        CheckWorkKindForColumnTotalData(iTargetDay, int.Parse(row["work_kind"].ToString()) - 1, 1, frmMainSchedule.astrScheduleStaff[iScheduleStaff, 0]);
                    }
                }
            }
        }

        /// <summary>
        /// メイングリッドデータの変更
        /// </summary>
        public void ChangeMainGridData(int iWorkKindID)
        {
            // 希望シフト判定
            // Mod Start WataruT 2020.07.14 夜勤入力時、明けと休みもセットで入力
            //if (clsCommonControl.GetRequestFlag(
            //    frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn, frmMainSchedule.piGrdMain_CurrentRow].Style.BackColor) == "1")
            //{
            //    if (MessageBox.Show("変更箇所が希望シフトとなりますが、よろしいですか？", "", MessageBoxButtons.YesNo) == DialogResult.No)
            //    {
            //        return;
            //    }
            //}
            // "夜勤"
            if (iWorkKindID == 1)
            {
                for(int iTargetColumnCount = 0; iTargetColumnCount < 2; iTargetColumnCount++)
                {
                    if(frmMainSchedule.piGrdMain_CurrentColumn + iTargetColumnCount < frmMainSchedule.grdMain.Columns.Count)
                    {
                        if (clsCommonControl.GetRequestFlag(frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn + iTargetColumnCount, frmMainSchedule.piGrdMain_CurrentRow].Style.BackColor) == "1")
                        {
                            if (MessageBox.Show("変更箇所が希望シフトとなりますが、よろしいですか？", "", MessageBoxButtons.YesNo) == DialogResult.No)
                            {
                                return;
                            }
                            break;
                        }
                    }
                }
            }
            // "夜勤"以外
            else
            {
                if (clsCommonControl.GetRequestFlag(frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn, frmMainSchedule.piGrdMain_CurrentRow].Style.BackColor) == "1")
                {
                    if (MessageBox.Show("変更箇所が希望シフトとなりますが、よろしいですか？", "", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                }
            }
            // Mod End   WataruT 2020.07.14 夜勤入力時、明けと休みもセットで入力

            // 共通変数側の値をリセット
            for (int iWorkKind = 0; iWorkKind < frmMainSchedule.astrWorkKind.GetLength(0); iWorkKind++)
            {
                if (frmMainSchedule.aiData[frmMainSchedule.piGrdMain_CurrentRow, frmMainSchedule.piGrdMain_CurrentColumn - 1, iWorkKind] == 1)
                {
                    frmMainSchedule.aiData[frmMainSchedule.piGrdMain_CurrentRow, frmMainSchedule.piGrdMain_CurrentColumn - 1, iWorkKind] = 0;
                    CheckWorkKindForRowTotalData(frmMainSchedule.piGrdMain_CurrentRow, iWorkKind, -1);
                    CheckWorkKindForColumnTotalData(frmMainSchedule.piGrdMain_CurrentColumn - 1, iWorkKind, -1, frmMainSchedule.astrScheduleStaff[frmMainSchedule.piGrdMain_CurrentRow, 0]);
                }
            }
            // 共通変数側の値を設定
            frmMainSchedule.aiData[frmMainSchedule.piGrdMain_CurrentRow, frmMainSchedule.piGrdMain_CurrentColumn - 1, iWorkKindID] = 1;
            CheckWorkKindForRowTotalData(frmMainSchedule.piGrdMain_CurrentRow, iWorkKindID, 1);
            CheckWorkKindForColumnTotalData(frmMainSchedule.piGrdMain_CurrentColumn - 1, iWorkKindID, 1, frmMainSchedule.astrScheduleStaff[frmMainSchedule.piGrdMain_CurrentRow, 0]);

            // メイングリッドに値、色設定をセット
            frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn, frmMainSchedule.piGrdMain_CurrentRow].Value = frmMainSchedule.astrWorkKind[iWorkKindID, 1];
            frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn, frmMainSchedule.piGrdMain_CurrentRow].Style.ForeColor = clsCommonControl.GetWorkKindForeColor(
                frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn, frmMainSchedule.piGrdMain_CurrentRow].Value.ToString());
            frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn, frmMainSchedule.piGrdMain_CurrentRow].Style.BackColor = clsCommonControl.GetWeekNameBackgroundColor(
                clsCommonControl.GetWeekName(frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", frmMainSchedule.piGrdMain_CurrentColumn), frmMainSchedule.astrHoliday));

            // Add Start WataruT 2020.07.14 夜勤入力時、明けと休みもセットで入力
            if(iWorkKindID == 1)
            {
                for(int iTargetColumnCount = 1; iTargetColumnCount <= 2; iTargetColumnCount++)
                {
                    if(frmMainSchedule.piGrdMain_CurrentColumn + iTargetColumnCount < frmMainSchedule.grdMain.Columns.Count)
                    {
                        // 共通変数側の値をリセット
                        for (int iWorkKind = 0; iWorkKind < frmMainSchedule.astrWorkKind.GetLength(0); iWorkKind++)
                        {
                            if (frmMainSchedule.aiData[frmMainSchedule.piGrdMain_CurrentRow, frmMainSchedule.piGrdMain_CurrentColumn + iTargetColumnCount - 1, iWorkKind] == 1)
                            {
                                frmMainSchedule.aiData[frmMainSchedule.piGrdMain_CurrentRow, frmMainSchedule.piGrdMain_CurrentColumn + iTargetColumnCount - 1, iWorkKind] = 0;
                                CheckWorkKindForRowTotalData(frmMainSchedule.piGrdMain_CurrentRow, iWorkKind, -1);
                                CheckWorkKindForColumnTotalData(frmMainSchedule.piGrdMain_CurrentColumn + iTargetColumnCount - 1, iWorkKind, -1, frmMainSchedule.astrScheduleStaff[frmMainSchedule.piGrdMain_CurrentRow, 0]);
                            }
                        }
                        // 共通変数側の値を設定
                        frmMainSchedule.aiData[frmMainSchedule.piGrdMain_CurrentRow, frmMainSchedule.piGrdMain_CurrentColumn + iTargetColumnCount - 1, iWorkKindID + iTargetColumnCount] = 1;
                        CheckWorkKindForRowTotalData(frmMainSchedule.piGrdMain_CurrentRow, iWorkKindID + iTargetColumnCount, 1);
                        CheckWorkKindForColumnTotalData(frmMainSchedule.piGrdMain_CurrentColumn + iTargetColumnCount - 1, iWorkKindID + iTargetColumnCount, 1, frmMainSchedule.astrScheduleStaff[frmMainSchedule.piGrdMain_CurrentRow, 0]);

                        // メイングリッドに値、色設定をセット
                        frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn + iTargetColumnCount, frmMainSchedule.piGrdMain_CurrentRow].Value = frmMainSchedule.astrWorkKind[iWorkKindID + iTargetColumnCount, 1];
                        frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn + iTargetColumnCount, frmMainSchedule.piGrdMain_CurrentRow].Style.ForeColor = clsCommonControl.GetWorkKindForeColor(
                            frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn + iTargetColumnCount, frmMainSchedule.piGrdMain_CurrentRow].Value.ToString());
                        frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn + iTargetColumnCount, frmMainSchedule.piGrdMain_CurrentRow].Style.BackColor = clsCommonControl.GetWeekNameBackgroundColor(
                            clsCommonControl.GetWeekName(frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", frmMainSchedule.piGrdMain_CurrentColumn + iTargetColumnCount), frmMainSchedule.astrHoliday));
                    }
                }
            }
            // Add End   WataruT 2020.07.14 夜勤入力時、明けと休みもセットで入力

            // 行・列の合計グリッドを再描画
            SetRowTotal();
            SetColumnTotal();

            // 列・行の合計グリッドの表示位置をセット
            frmMainSchedule.grdRowTotal.FirstDisplayedScrollingRowIndex = frmMainSchedule.grdMain.FirstDisplayedScrollingRowIndex;
            if (frmMainSchedule.grdMain.FirstDisplayedScrollingColumnIndex == 0)
                frmMainSchedule.grdColumnTotal.FirstDisplayedScrollingColumnIndex = 1;
            else
                frmMainSchedule.grdColumnTotal.FirstDisplayedScrollingColumnIndex = frmMainSchedule.grdMain.FirstDisplayedScrollingColumnIndex;
        }

        /// <summary>
        /// メイングリッドデータの一括変更
        /// Add WataruT 2020.07.13 複数選択箇所を一括変更可能とする
        /// </summary>
        public void ChangeMainGridMultiData(int iWorkKindID)
        {
            int iCurrentRow;
            int iCurrentColumn;

            // 希望シフト判定
            for (int iTargetCell = 0; iTargetCell < frmMainSchedule.grdMain.SelectedCells.Count; iTargetCell++)
            {
                if (clsCommonControl.GetRequestFlag(frmMainSchedule.grdMain.SelectedCells[iTargetCell].Style.BackColor) == "1")
                {
                    if (MessageBox.Show("変更箇所が希望シフトとなりますが、よろしいですか？", "", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                    else
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
                            CheckWorkKindForColumnTotalData(iCurrentColumn - 1, iWorkKind, -1, frmMainSchedule.astrScheduleStaff[iCurrentRow, 0]);
                        }
                    }
                    // 共通変数側の値を設定
                    frmMainSchedule.aiData[iCurrentRow, iCurrentColumn - 1, iWorkKindID] = 1;
                    CheckWorkKindForRowTotalData(iCurrentRow, iWorkKindID, 1);
                    CheckWorkKindForColumnTotalData(iCurrentColumn - 1, iWorkKindID, 1, frmMainSchedule.astrScheduleStaff[iCurrentRow, 0]);

                    // メイングリッドに値、色設定をセット
                    frmMainSchedule.grdMain[iCurrentColumn, iCurrentRow].Value = frmMainSchedule.astrWorkKind[iWorkKindID, 1];
                    frmMainSchedule.grdMain[iCurrentColumn, iCurrentRow].Style.ForeColor = clsCommonControl.GetWorkKindForeColor(
                        frmMainSchedule.grdMain[iCurrentColumn, iCurrentRow].Value.ToString());
                    frmMainSchedule.grdMain[iCurrentColumn, iCurrentRow].Style.BackColor = clsCommonControl.GetWeekNameBackgroundColor(
                        clsCommonControl.GetWeekName(frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", iCurrentColumn), frmMainSchedule.astrHoliday));
                }
            }

            // 行・列の合計グリッドを再描画
            SetRowTotal();
            SetColumnTotal();

            // 列・行の合計グリッドの表示位置をセット
            frmMainSchedule.grdRowTotal.FirstDisplayedScrollingRowIndex = frmMainSchedule.grdMain.FirstDisplayedScrollingRowIndex;
            if (frmMainSchedule.grdMain.FirstDisplayedScrollingColumnIndex == 0)
                frmMainSchedule.grdColumnTotal.FirstDisplayedScrollingColumnIndex = 1;
            else
                frmMainSchedule.grdColumnTotal.FirstDisplayedScrollingColumnIndex = frmMainSchedule.grdMain.FirstDisplayedScrollingColumnIndex;
        }

        /// <summary>
        /// 勤務予定データの削除
        /// </summary>
        public void DeleteScheduleData()
        {
            string strTargetMonthForHeader = frmMainSchedule.lblTargetMonth.Text.Substring(0, 4) + frmMainSchedule.lblTargetMonth.Text.Substring(5, 2);
            string strScheduleNo = clsDatabaseControl.GetScheduleHeader_TargetScheduleNo(frmMainSchedule.cmbWard.SelectedValue.ToString(), strTargetMonthForHeader, frmMainSchedule.pstrStaffKind);

            clsDatabaseControl.DeleteScheduleHeader_ScheduleNo(strScheduleNo);
            clsDatabaseControl.DeleteScheduleDetail_ScheduleNo(strScheduleNo);
        }

        /// <summary>
        /// 前月末の夜勤チェック
        /// </summary>
        public void CheckNightLastMonth()
        {
            string strWorkKind;
            string strTargetDate;

            // 前月末の日にちをセット
            strTargetDate = DateTime.ParseExact(frmMainSchedule.pstrTargetMonth + "01", "yyyyMMdd", null).AddDays(-1).ToString("yyyyMMdd");

            for(int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.dtScheduleStaff.Rows.Count; iScheduleStaff++)
            {
                // 前月末の勤務種類を取得
                strWorkKind = clsDatabaseControl.GetScheduleDetail_Staff_TargetDate(frmMainSchedule.astrScheduleStaff[iScheduleStaff, 0], strTargetDate);

                switch(strWorkKind)
                {
                    case "02":      // 夜勤
                        for(int iWorkKind = 0; iWorkKind < frmMainSchedule.dtWorkKind.Rows.Count; iWorkKind++)
                        {
                            frmMainSchedule.aiData[iScheduleStaff, 0, iWorkKind] = 0;
                            frmMainSchedule.aiData[iScheduleStaff, 1, iWorkKind] = 0;
                        }
                        frmMainSchedule.aiData[iScheduleStaff, 0, 2] = 1;
                        frmMainSchedule.aiData[iScheduleStaff, 1, 3] = 1;
                        frmMainSchedule.aiNightLastMonthFlag[iScheduleStaff, 0] = 1;
                        frmMainSchedule.aiNightLastMonthFlag[iScheduleStaff, 1] = 1;
                        CheckWorkKindForColumnTotalData(0, 0, -1, frmMainSchedule.astrScheduleStaff[iScheduleStaff, 0]);
                        CheckWorkKindForColumnTotalData(1, 0, -1, frmMainSchedule.astrScheduleStaff[iScheduleStaff, 0]);
                        CheckWorkKindForRowTotalData(iScheduleStaff, 0, 1);
                        CheckWorkKindForRowTotalData(iScheduleStaff, 2, 1);
                        CheckWorkKindForColumnTotalData(0, 2, 1, frmMainSchedule.astrScheduleStaff[iScheduleStaff, 0]);
                        break;
                        
                    case "03":      // 夜明
                        for (int iWorkKind = 0; iWorkKind < frmMainSchedule.dtWorkKind.Rows.Count; iWorkKind++)
                        {
                            frmMainSchedule.aiData[iScheduleStaff, 0, iWorkKind] = 0;
                        }
                        frmMainSchedule.aiNightLastMonthFlag[iScheduleStaff, 0] = 1;
                        frmMainSchedule.aiData[iScheduleStaff, 0, 3] = 1;
                        CheckWorkKindForColumnTotalData(0, 0, -1, frmMainSchedule.astrScheduleStaff[iScheduleStaff, 0]);
                        CheckWorkKindForRowTotalData(iScheduleStaff, 0, 1);
                        break;
                }
            }
        }

        /// <summary>
        /// 表示していない職種の列合計値を取得
        /// </summary>
        public void SetColumnTotalOtherStaffKindCount()
        {
            DataTable dt;
            string strOtherStaffKind;

            // 表示していない職種をセット
            if (frmMainSchedule.pstrStaffKind == "01")
                strOtherStaffKind = "02";
            else
                strOtherStaffKind = "01";

            // 表示していない職種の性別ごとの夜勤予定データを取得
            dt = clsDatabaseControl.GetScheduleDetail_Ward_StaffKind_TargetMonth(frmMainSchedule.cmbWard.SelectedValue.ToString(), strOtherStaffKind,
                frmMainSchedule.pstrTargetMonth);

            // データをチェックしてカウント
            foreach (DataRow row in dt.Rows)
            {
                if(row["sex"].ToString() == "1")
                    frmMainSchedule.adColumnTotalData[int.Parse(row["day"].ToString()), 3]++;
                else
                    frmMainSchedule.adColumnTotalData[int.Parse(row["day"].ToString()), 4]++;
            }

            // 表示していない職種の新人以外の夜勤予定データを取得
            dt = clsDatabaseControl.GetScheduleDetail_Ward_StaffKind_TargetMonth_ForStaffLevel(frmMainSchedule.cmbWard.SelectedValue.ToString(), strOtherStaffKind,
                frmMainSchedule.pstrTargetMonth);

            // データをカウント
            foreach (DataRow row in dt.Rows)
            {
                frmMainSchedule.adColumnTotalData[int.Parse(row["day"].ToString()), 5]++;
            }
        }

        /// <summary>
        /// 救急指定日か判定
        /// </summary>
        /// <returns></returns>
        public bool CheckEmergencyDate(string strTargetDay)
        {
            for(int i = 0; i < frmMainSchedule.dtEmergencyDate.Rows.Count; i++)
            {
                if ((frmMainSchedule.pstrTargetMonth + strTargetDay) == frmMainSchedule.dtEmergencyDate.Rows[i]["target_date"].ToString())
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 勤務予定初回データを削除
        /// Add WataruT 2020.07.21 初回登録解除機能追加
        /// </summary>
        public void DeleteScheduleFirstData()
        {
            string strScheduleNo;
            
            // 対象の予定番号を取得
            strScheduleNo = clsDatabaseControl.GetScheduleFirstHeader_TargetScheduleNo(frmMainSchedule.cmbWard.SelectedValue.ToString(), frmMainSchedule.pstrTargetMonth, frmMainSchedule.pstrStaffKind);

            // 勤務予定初回データの削除
            clsDatabaseControl.DeleteScheduleFirstHeader_ScheduleNo(strScheduleNo);
            clsDatabaseControl.DeleteScheduleFirstDetail_ScheduleNo(strScheduleNo);
        }
    }
}
