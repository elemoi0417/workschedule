using OfficeOpenXml;
using System;
using System.Data;
using System.Windows.Forms;
using workschedule.Controls;
using System.IO;
// Add Start WataruT 2020.08.17 勤務表(締翌日)の日曜と祝日の背景色を変更
using workschedule.Functions;
// Add End   WataruT 2020.08.17 勤務表(締翌日)の日曜と祝日の背景色を変更
namespace workschedule.Reports
{
    class PrintWorkScheduleHalf
    {
        // 使用クラス宣言
        DatabaseControl clsDatabaseControl = new DatabaseControl();
        // Add Start WataruT 2020.08.17 勤務表(締翌日)の日曜と祝日の背景色を変更
        CommonControl clsCommonControl = new CommonControl();
        // Add End   WataruT 2020.08.17 勤務表(締翌日)の日曜と祝日の背景色を変更

        // 定数
        const int COLUMN_CREATE_YEAR1 = 1;
        const int COLUMN_CREATE_MONTH1 = 3;
        const int COLUMN_CREATE_NEXT_YEAR1 = 5;
        const int COLUMN_CREATE_NEXT_MONTH1 = 7;
        const int COLUMN_WARD1 = 28;
        const int COLUMN_CREATE_YEAR2 = 1;
        const int COLUMN_CREATE_MONTH2 = 3;
        const int COLUMN_CREATE_NEXT_YEAR2 = 5;
        const int COLUMN_CREATE_NEXT_MONTH2 = 7;
        const int COLUMN_WARD2 = 28;
        const int COLUMN_CREATE_YEAR3 = 1;
        const int COLUMN_CREATE_MONTH3 = 3;
        const int COLUMN_CREATE_NEXT_YEAR3 = 5;
        const int COLUMN_CREATE_NEXT_MONTH3 = 7;
        const int COLUMN_WARD3 = 28;


        const int COLUMN_NURSE_DAY_START1 = 6;
        const int COLUMN_NURSE_DAY_START2 = 6;
        const int COLUMN_CARE_DAY_START = 6;
        const int COLUMN_NURSE_DAY_OF_WEEK1 = 6;
        const int COLUMN_NURSE_DAY_OF_WEEK2 = 6;
        const int COLUMN_CARE_DAY_OF_WEEK = 6;
        const int COLUMN_NURSE_STAFF_START1 = 1;
        const int COLUMN_NURSE_STAFF_START2 = 1;
        const int COLUMN_CARE_STAFF_START = 3;

        const int ROW_CREATE_YEAR1 = 2;
        const int ROW_CREATE_MONTH1 = 2;
        const int ROW_CREATE_NEXT_YEAR1 = 2;
        const int ROW_CREATE_NEXT_MONTH1 = 2;
        const int ROW_WARD1 = 1;
        const int ROW_CREATE_YEAR2 = 49;
        const int ROW_CREATE_MONTH2 = 49;
        const int ROW_CREATE_NEXT_YEAR2 = 49;
        const int ROW_CREATE_NEXT_MONTH2 = 49;
        const int ROW_WARD2 = 48;
        const int ROW_CREATE_YEAR3 = 96;
        const int ROW_CREATE_MONTH3 = 96;
        const int ROW_CREATE_NEXT_YEAR3 = 96;
        const int ROW_CREATE_NEXT_MONTH3 = 96;
        const int ROW_WARD3 = 95;

        const int ROW_NURSE_DAY_START1 = 4;
        const int ROW_NURSE_DAY_START2 = 51;
        const int ROW_CARE_DAY_START = 98;
        const int ROW_NURSE_DAY_OF_WEEK1 = 5;
        const int ROW_NURSE_DAY_OF_WEEK2 = 52;
        const int ROW_CARE_DAY_OF_WEEK = 99;
        const int ROW_NURSE_STAFF_START1 = 6;
        const int ROW_NURSE_STAFF_START2 = 53;
        const int ROW_CARE_STAFF_START = 100;

        const int ROW_NURSE_TOTAL_ROW = 19;

        // 変数
        string pstrTargetWardCode;
        string pstrTargetWard;
        string pstrTargetYear;
        string pstrTargetMonth;
        string pstrTargetNextYear;
        string pstrTargetNextMonth;

        string strFilePath = Environment.CurrentDirectory + @"\Report\workschedulehalf.xlsx";

        string[,] astrScheduleStaffNurse;           // 職員マスタ配列(人数、ID・氏名・職種)
        string[,] astrScheduleStaffCare;            // 職員マスタ配列(人数、ID・氏名・職種)
        // Add Start WataruT 2020.08.17 勤務表(締翌日)の日曜と祝日の背景色を変更
        string[] astrHoliday;                       // 祝日マスタ配列
        // Add End   WataruT 2020.08.17 勤務表(締翌日)の日曜と祝日の背景色を変更

        /// <summary>
        /// クラス初期化
        /// </summary>
        /// <param name="frmMainSchedule_Parent"></param>
        public PrintWorkScheduleHalf(string strTargetWardCode_Parent, string strTargetWard_Parent, string strTargetYear_Parent, string strTargetMonth_Parent)
        {
            // 引数を共通変数にセット
            pstrTargetWardCode = strTargetWardCode_Parent;
            pstrTargetWard = strTargetWard_Parent;
            pstrTargetYear = strTargetYear_Parent;
            pstrTargetMonth = strTargetMonth_Parent;
            pstrTargetNextYear = DateTime.Parse(strTargetYear_Parent + "/" + strTargetMonth_Parent + "/01").AddMonths(1).Year.ToString();
            pstrTargetNextMonth = DateTime.Parse(strTargetYear_Parent + "/" + strTargetMonth_Parent + "/01").AddMonths(1).ToString("MM");
        }

        /// <summary>
        /// 帳票ファイル作成処理
        /// </summary>
        /// <param name="targetReport"></param>
        /// <param name="staffNo"></param>
        /// <param name="orderNo"></param>
        public void SaveFile()
        {
            int iTargetMonthDay;
            bool bNoDataFlag;
            string strLateEarlyTime;     // Add WataruT 2020.08.06 遅刻・早退入力対応
            DateTime dtTargetMonth = DateTime.ParseExact(pstrTargetYear + pstrTargetMonth + "01", "yyyyMMdd", null);
            DateTime dtTargetNextMonth = DateTime.ParseExact(pstrTargetNextYear + pstrTargetNextMonth + "01", "yyyyMMdd", null);
            DataTable dtScheduleDetail;
            DataTable dtScheduleFirstDetail;
            DataTable dtResultDetailItem;       // Add WataruT 2020.08.06 遅刻・早退入力対応
            SaveFileDialog sfd = new SaveFileDialog();

            iTargetMonthDay = DateTime.DaysInMonth(int.Parse(pstrTargetYear), int.Parse(pstrTargetMonth)) - 15;

            // 保存ダイアログのプロパティ設定
            SetSaveFileDialogProperties(ref sfd);

            //ダイアログを表示する
            if (sfd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // オブジェクト初期化
            InitializeObject();

            // Excelファイルの読み込み
            var xlReadFile = new FileInfo(strFilePath);

            // オブジェクトにセット
            using (var xlFile = new ExcelPackage(xlReadFile))
            {
                // シートを選択
                var xlSheet = xlFile.Workbook.Worksheets["シート"];

                // === Excelデータ入力 ===

                // 作成年月
                xlSheet.Cells[ROW_CREATE_YEAR1, COLUMN_CREATE_YEAR1].Value = pstrTargetYear;
                xlSheet.Cells[ROW_CREATE_YEAR2, COLUMN_CREATE_YEAR2].Value = pstrTargetYear;
                xlSheet.Cells[ROW_CREATE_YEAR3, COLUMN_CREATE_YEAR3].Value = pstrTargetYear;

                xlSheet.Cells[ROW_CREATE_MONTH1, COLUMN_CREATE_MONTH1].Value = pstrTargetMonth;
                xlSheet.Cells[ROW_CREATE_MONTH2, COLUMN_CREATE_MONTH2].Value = pstrTargetMonth;
                xlSheet.Cells[ROW_CREATE_MONTH3, COLUMN_CREATE_MONTH3].Value = pstrTargetMonth;

                xlSheet.Cells[ROW_CREATE_NEXT_YEAR1, COLUMN_CREATE_NEXT_YEAR1].Value = pstrTargetNextYear;
                xlSheet.Cells[ROW_CREATE_NEXT_YEAR2, COLUMN_CREATE_NEXT_YEAR2].Value = pstrTargetNextYear;
                xlSheet.Cells[ROW_CREATE_NEXT_YEAR3, COLUMN_CREATE_NEXT_YEAR3].Value = pstrTargetNextYear;

                xlSheet.Cells[ROW_CREATE_NEXT_MONTH1, COLUMN_CREATE_NEXT_MONTH1].Value = pstrTargetNextMonth;
                xlSheet.Cells[ROW_CREATE_NEXT_MONTH2, COLUMN_CREATE_NEXT_MONTH2].Value = pstrTargetNextMonth;
                xlSheet.Cells[ROW_CREATE_NEXT_MONTH3, COLUMN_CREATE_NEXT_MONTH3].Value = pstrTargetNextMonth;

                // 対象病棟
                xlSheet.Cells[ROW_WARD1, COLUMN_WARD1].Value = "第" + pstrTargetWard;
                xlSheet.Cells[ROW_WARD2, COLUMN_WARD2].Value = "第" + pstrTargetWard;
                xlSheet.Cells[ROW_WARD3, COLUMN_WARD3].Value = "第" + pstrTargetWard;

                // == 日付・曜日 ==

                // 当月
                for (int i = 15; i < DateTime.DaysInMonth(int.Parse(pstrTargetYear), int.Parse(pstrTargetMonth)); i++)
                {
                    // 日にち
                    xlSheet.Cells[ROW_NURSE_DAY_START1, COLUMN_NURSE_DAY_START1 - 15 + i].Value = i + 1;
                    xlSheet.Cells[ROW_NURSE_DAY_START2, COLUMN_NURSE_DAY_START2 - 15 + i].Value = i + 1;
                    xlSheet.Cells[ROW_CARE_DAY_START, COLUMN_CARE_DAY_START - 15 + i].Value = i + 1;

                    // 曜日
                    xlSheet.Cells[ROW_NURSE_DAY_OF_WEEK1, COLUMN_NURSE_DAY_OF_WEEK1 - 15 + i].Value = dtTargetMonth.AddDays(double.Parse(i.ToString())).ToString("ddd") + "曜";
                    xlSheet.Cells[ROW_NURSE_DAY_OF_WEEK2, COLUMN_NURSE_DAY_OF_WEEK2 - 15 + i].Value = dtTargetMonth.AddDays(double.Parse(i.ToString())).ToString("ddd") + "曜";
                    xlSheet.Cells[ROW_CARE_DAY_OF_WEEK, COLUMN_CARE_DAY_OF_WEEK - 15 + i].Value = dtTargetMonth.AddDays(double.Parse(i.ToString())).ToString("ddd") + "曜";

                    // Add Start WataruT 2020.08.17 勤務表(締翌日)の日曜と祝日の背景色を変更
                    // 日祝のセル背景
                    if (clsCommonControl.GetWeekName(dtTargetMonth.AddDays(i).ToString("yyyyMMdd"), astrHoliday) == "祝" ||
                        clsCommonControl.GetWeekName(dtTargetMonth.AddDays(i).ToString("yyyyMMdd"), astrHoliday) == "日")
                    {
                        for (int iStaffCount = 0; iStaffCount < 2 * ROW_NURSE_TOTAL_ROW; iStaffCount++)
                        {
                            xlSheet.Cells[ROW_NURSE_DAY_START1 + 2 + iStaffCount, COLUMN_NURSE_DAY_START1 - 15 + i].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Gray125;
                            xlSheet.Cells[ROW_NURSE_DAY_START2 + 2 + iStaffCount, COLUMN_NURSE_DAY_START2 - 15 + i].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Gray125;
                            xlSheet.Cells[ROW_CARE_DAY_START + 2 + iStaffCount, COLUMN_CARE_DAY_START - 15 + i].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Gray125;
                        }
                    }
                    // Add End   WataruT 2020.08.17 勤務表(締翌日)の日曜と祝日の背景色を変更
                }

                // 翌月
                for (int i = 0; i < 15; i++)
                {
                    // 日にち
                    xlSheet.Cells[ROW_NURSE_DAY_START1, COLUMN_NURSE_DAY_START1 + iTargetMonthDay + i].Value = i + 1;
                    xlSheet.Cells[ROW_NURSE_DAY_START2, COLUMN_NURSE_DAY_START2 + iTargetMonthDay + i].Value = i + 1;
                    xlSheet.Cells[ROW_CARE_DAY_START, COLUMN_CARE_DAY_START + iTargetMonthDay + i].Value = i + 1;

                    // 曜日
                    xlSheet.Cells[ROW_NURSE_DAY_OF_WEEK1, COLUMN_NURSE_DAY_START1 + iTargetMonthDay + i].Value = dtTargetNextMonth.AddDays(double.Parse(i.ToString())).ToString("ddd") + "曜";
                    xlSheet.Cells[ROW_NURSE_DAY_OF_WEEK2, COLUMN_NURSE_DAY_START2 + iTargetMonthDay + i].Value = dtTargetNextMonth.AddDays(double.Parse(i.ToString())).ToString("ddd") + "曜";
                    xlSheet.Cells[ROW_CARE_DAY_OF_WEEK, COLUMN_CARE_DAY_START + iTargetMonthDay + i].Value = dtTargetNextMonth.AddDays(double.Parse(i.ToString())).ToString("ddd") + "曜";

                    // Add Start WataruT 2020.08.17 勤務表(締翌日)の日曜と祝日の背景色を変更
                    // 日祝のセル背景
                    if (clsCommonControl.GetWeekName(dtTargetNextMonth.AddDays(i).ToString("yyyyMMdd"), astrHoliday) == "祝" ||
                        clsCommonControl.GetWeekName(dtTargetNextMonth.AddDays(i).ToString("yyyyMMdd"), astrHoliday) == "日")
                    {
                        for (int iStaffCount = 0; iStaffCount < 2 * ROW_NURSE_TOTAL_ROW; iStaffCount++)
                        {
                            xlSheet.Cells[ROW_NURSE_DAY_START1 + 2 + iStaffCount, COLUMN_NURSE_DAY_START1 + iTargetMonthDay + i].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Gray125;
                            xlSheet.Cells[ROW_NURSE_DAY_START2 + 2 + iStaffCount, COLUMN_NURSE_DAY_START2 + iTargetMonthDay + i].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Gray125;
                            xlSheet.Cells[ROW_CARE_DAY_START + 2 + iStaffCount, COLUMN_CARE_DAY_START + iTargetMonthDay + i].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Gray125;
                        }
                    }
                    // Add End   WataruT 2020.08.17 勤務表(締翌日)の日曜と祝日の背景色を変更
                }

                // == 看護師・准看護師 ==

                // 複数ページ対応とするか判定
                if (astrScheduleStaffNurse.GetLength(0) > ROW_NURSE_TOTAL_ROW)
                {
                    // 1ページ目

                    // 職種、順番、職員氏名
                    for (int iStaff = 0; iStaff < ROW_NURSE_TOTAL_ROW; iStaff++)
                    {
                        // 種別
                        xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, COLUMN_NURSE_STAFF_START1].Value = astrScheduleStaffNurse[iStaff, 2];
                        xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, COLUMN_NURSE_STAFF_START1].Value = "";

                        // 順番
                        xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, COLUMN_NURSE_STAFF_START1 + 2].Value = iStaff + 1;
                        xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, COLUMN_NURSE_STAFF_START1 + 2].Value = "";

                        // 氏名
                        xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, COLUMN_NURSE_STAFF_START1 + 3].Value = astrScheduleStaffNurse[iStaff, 1];
                        xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, COLUMN_NURSE_STAFF_START1 + 3].Value = "";
                    }

                    // 初回予定データ、最終実績データ(当月)
                    for (int iStaff = 0; iStaff < ROW_NURSE_TOTAL_ROW; iStaff++)
                    {
                        // 対象職員の計画データ取得
                        dtScheduleDetail = clsDatabaseControl.GetScheduleDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                            astrScheduleStaffNurse[iStaff, 0], "01", dtTargetMonth.ToString("yyyyMM"));
                        dtScheduleFirstDetail = clsDatabaseControl.GetScheduleFirstDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                                                astrScheduleStaffNurse[iStaff, 0], "01", dtTargetMonth.ToString("yyyyMM"));
                        // Add Start WataruT 2020.08.06 遅刻・早退入力対応
                        dtResultDetailItem = clsDatabaseControl.GetResultDetail_Ward_TargetDate_ResultDetailItem_Staff(pstrTargetWardCode, pstrTargetYear + pstrTargetMonth,
                                                "遅刻・早退のため", astrScheduleStaffNurse[iStaff, 0]);
                        // Add End   WataruT 2020.08.06 遅刻・早退入力対応

                        // 1日から順に処理
                        for (int iDay = 15; iDay < DateTime.DaysInMonth(dtTargetMonth.Year, dtTargetMonth.Month); iDay++)
                        {
                            // データなしフラグを初期化
                            bNoDataFlag = false;

                            // 初回計画データがある場合
                            if (dtScheduleFirstDetail.Rows.Count != 0)
                            {
                                // 初回計画データを順に確認
                                foreach (DataRow row in dtScheduleFirstDetail.Rows)
                                {
                                    // 対象日と一致する
                                    if (DateTime.Parse(row["target_date"].ToString()).Day == iDay + 1)
                                    {
                                        // 最終計画データを順に確認
                                        foreach (DataRow row2 in dtScheduleDetail.Rows)
                                        {
                                            // 対象日と一致する
                                            if (DateTime.Parse(row2["target_date"].ToString()).Day == iDay + 1)
                                            {
                                                // 初回計画データ
                                                xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, COLUMN_NURSE_DAY_START1 - 15 + iDay].Value = row["name_short"].ToString();

                                                // 最終計画データと異なる場合
                                                if (row["name_short"].ToString() == row2["name_short"].ToString())
                                                {
                                                    // 最終計画データの勤務種類は空欄とする
                                                    xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, COLUMN_NURSE_DAY_START1 - 15 + iDay].Value = "";
                                                }
                                                else
                                                {
                                                    // Mod Start WataruT 2020.08.06 遅刻・早退入力対応
                                                    //// 最終計画データの勤務種類をセット
                                                    //WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 - 15 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, row2["name_short"].ToString());
                                                    // 遅刻・早退か判定
                                                    if (CheckWorkKindLateEarly(row2["name_short"].ToString()) == true)
                                                    {
                                                        // 遅刻・早退の実績時間を取得
                                                        strLateEarlyTime = GetOtherWorkTimeTotal(dtResultDetailItem, iDay + 1);
                                                        // 遅刻・早退の実績時間を取得
                                                        xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, COLUMN_NURSE_DAY_START1 - 15 + iDay].Value = strLateEarlyTime + "H" + row2["name_short"].ToString();
                                                    }
                                                    else
                                                    {
                                                        xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, COLUMN_NURSE_DAY_START1 - 15 + iDay].Value = row2["name_short"].ToString();
                                                    }
                                                    // Mod End   WataruT 2020.08.06 遅刻・早退入力対応
                                                }
                                                break;
                                            }
                                        }
                                        bNoDataFlag = true;
                                        break;
                                    }
                                }
                            }
                            // 初回計画データがない場合
                            else if (bNoDataFlag == false)
                            {
                                // 初回計画データ
                                xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, COLUMN_NURSE_DAY_START1 - 15 + iDay].Value = "";
                                // 最終計画データ
                                xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, COLUMN_NURSE_DAY_START1 - 15 + iDay].Value = "";
                            }
                        }
                    }

                    // 初回予定データ、最終実績データ(翌月)
                    for (int iStaff = 0; iStaff < ROW_NURSE_TOTAL_ROW; iStaff++)
                    {
                        // 対象職員の計画データ取得
                        dtScheduleDetail = clsDatabaseControl.GetScheduleDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                            astrScheduleStaffNurse[iStaff, 0], "01", dtTargetNextMonth.ToString("yyyyMM"));
                        dtScheduleFirstDetail = clsDatabaseControl.GetScheduleFirstDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                                                astrScheduleStaffNurse[iStaff, 0], "01", dtTargetNextMonth.ToString("yyyyMM"));
                        // Add Start WataruT 2020.08.06 遅刻・早退入力対応
                        dtResultDetailItem = clsDatabaseControl.GetResultDetail_Ward_TargetDate_ResultDetailItem_Staff(pstrTargetWardCode, pstrTargetNextYear + pstrTargetNextMonth,
                                                "遅刻・早退のため", astrScheduleStaffNurse[iStaff, 0]);
                        // Add End   WataruT 2020.08.06 遅刻・早退入力対応

                        // 1日から順に処理
                        // Mod Start WataruT 2020.08.05 計画表(締翌日)の不具合対応
                        //for (int iDay = 0; iDay <= 15; iDay++)
                        for (int iDay = 0; iDay < 15; iDay++)
                        // Mod End   WataruT 2020.08.05 計画表(締翌日)の不具合対応
                        {
                            // データなしフラグを初期化
                            bNoDataFlag = false;

                            // 初回計画データがある場合
                            if (dtScheduleFirstDetail.Rows.Count != 0)
                            {
                                // 初回計画データを順に確認
                                foreach (DataRow row in dtScheduleFirstDetail.Rows)
                                {
                                    // 対象日と一致する
                                    if (DateTime.Parse(row["target_date"].ToString()).Day == iDay + 1)
                                    {
                                        // 最終計画データを順に確認
                                        foreach (DataRow row2 in dtScheduleDetail.Rows)
                                        {
                                            // 対象日と一致する
                                            if (DateTime.Parse(row2["target_date"].ToString()).Day == iDay + 1)
                                            {
                                                // 初回計画データ
                                                // Mod Start WataruT 2020.08.05 計画表(締翌日)の不具合対応
                                                //WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, row["name_short"].ToString());
                                                xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, COLUMN_NURSE_DAY_START1 + iTargetMonthDay + iDay].Value = row["name_short"].ToString();
                                                // Mod End   WataruT 2020.08.05 計画表(締翌日)の不具合対応

                                                // 最終計画データと異なる場合
                                                if (row["name_short"].ToString() == row2["name_short"].ToString())
                                                {
                                                    // 最終計画データの勤務種類は空欄とする
                                                    // Mod Start WataruT 2020.08.05 計画表(締翌日)の不具合対応
                                                    //WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, "");
                                                    xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, COLUMN_NURSE_DAY_START1 + iTargetMonthDay + iDay].Value = "";
                                                    // Mod End   WataruT 2020.08.05 計画表(締翌日)の不具合対応
                                                }
                                                else
                                                {
                                                    // Mod Start WataruT 2020.08.06 遅刻・早退入力対応
                                                    //// 最終計画データの勤務種類をセット
                                                    //// Mod Start WataruT 2020.08.05 計画表(締翌日)の不具合対応
                                                    ////WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, row2["name_short"].ToString());
                                                    //WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 + iTargetMonthDay + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, row2["name_short"].ToString());
                                                    // Mod End   WataruT 2020.08.05 計画表(締翌日)の不具合対応
                                                    // 遅刻・早退か判定
                                                    if (CheckWorkKindLateEarly(row2["name_short"].ToString()) == true)
                                                    {
                                                        // 遅刻・早退の実績時間を取得
                                                        strLateEarlyTime = GetOtherWorkTimeTotal(dtResultDetailItem, iDay + 1);
                                                        // 遅刻・早退の実績時間を取得
                                                        xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, COLUMN_NURSE_DAY_START1 + iTargetMonthDay + iDay].Value = strLateEarlyTime + "H" + row2["name_short"].ToString();
                                                    }
                                                    else
                                                    {
                                                        xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, COLUMN_NURSE_DAY_START1 + iTargetMonthDay + iDay].Value = row2["name_short"].ToString();
                                                    }
                                                    // Mod End   WataruT 2020.08.06 遅刻・早退入力対応
                                                }
                                                break;
                                            }
                                        }
                                        bNoDataFlag = true;
                                        break;
                                    }
                                }
                            }
                            // 初回計画データがない場合
                            else if (bNoDataFlag == false)
                            {
                                // 初回計画データ
                                // Mod Start WataruT 2020.08.05 計画表(締翌日)の不具合対応
                                //WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, "");
                                xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, COLUMN_NURSE_DAY_START1 + iTargetMonthDay + iDay].Value = "";
                                // Mod End   WataruT 2020.08.05 計画表(締翌日)の不具合対応
                                // 最終計画データ
                                // Mod Start WataruT 2020.08.05 計画表(締翌日)の不具合対応
                                //WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, "");
                                xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, COLUMN_NURSE_DAY_START1 + iTargetMonthDay + iDay].Value = "";
                                // Mod End   WataruT 2020.08.05 計画表(締翌日)の不具合対応
                            }
                        }
                    }

                    // 2ページ目
                    // 職種、順番、職員氏名
                    for (int iStaff = ROW_NURSE_TOTAL_ROW; iStaff < astrScheduleStaffNurse.GetLength(0); iStaff++)
                    {
                        // 種別
                        xlSheet.Cells[ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 2, COLUMN_NURSE_STAFF_START2].Value = astrScheduleStaffNurse[iStaff, 2];
                        xlSheet.Cells[ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 1, COLUMN_NURSE_STAFF_START2].Value = "";

                        // 順番
                        xlSheet.Cells[ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 2, COLUMN_NURSE_STAFF_START2 + 2].Value = iStaff + 1;
                        xlSheet.Cells[ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 1, COLUMN_NURSE_STAFF_START2 + 2].Value = "";

                        // 氏名
                        xlSheet.Cells[ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 2, COLUMN_NURSE_STAFF_START2 + 3].Value = astrScheduleStaffNurse[iStaff, 1];
                        xlSheet.Cells[ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 1, COLUMN_NURSE_STAFF_START2 + 3].Value = "";
                    }

                    // 初回予定データ、最終実績データ(当月)
                    for (int iStaff = ROW_NURSE_TOTAL_ROW; iStaff < astrScheduleStaffNurse.GetLength(0); iStaff++)
                    {
                        // 対象職員の計画データ取得
                        dtScheduleDetail = clsDatabaseControl.GetScheduleDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                            astrScheduleStaffNurse[iStaff, 0], "01", dtTargetMonth.ToString("yyyyMM"));
                        dtScheduleFirstDetail = clsDatabaseControl.GetScheduleFirstDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                                                astrScheduleStaffNurse[iStaff, 0], "01", dtTargetMonth.ToString("yyyyMM"));
                        // Add Start WataruT 2020.08.06 遅刻・早退入力対応
                        dtResultDetailItem = clsDatabaseControl.GetResultDetail_Ward_TargetDate_ResultDetailItem_Staff(pstrTargetWardCode, pstrTargetYear + pstrTargetMonth,
                                                "遅刻・早退のため", astrScheduleStaffNurse[iStaff, 0]);
                        // Add End   WataruT 2020.08.06 遅刻・早退入力対応

                        // 1日から順に処理
                        for (int iDay = 15; iDay < DateTime.DaysInMonth(dtTargetMonth.Year, dtTargetMonth.Month); iDay++)
                        {
                            // データなしフラグを初期化
                            bNoDataFlag = false;

                            // 初回計画データがある場合
                            if (dtScheduleFirstDetail.Rows.Count != 0)
                            {
                                // 初回計画データを順に確認
                                foreach (DataRow row in dtScheduleFirstDetail.Rows)
                                {
                                    // 対象日と一致する
                                    if (DateTime.Parse(row["target_date"].ToString()).Day == iDay + 1)
                                    {
                                        // 最終計画データを順に確認
                                        foreach (DataRow row2 in dtScheduleDetail.Rows)
                                        {
                                            // 対象日と一致する
                                            if (DateTime.Parse(row2["target_date"].ToString()).Day == iDay + 1)
                                            {
                                                // 初回計画データ
                                                xlSheet.Cells[ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 2, COLUMN_NURSE_DAY_START2 - 15 + iDay].Value = row["name_short"].ToString();

                                                // 最終計画データと異なる場合
                                                if (row["name_short"].ToString() == row2["name_short"].ToString())
                                                {
                                                    // 最終計画データの勤務種類は空欄とする
                                                    xlSheet.Cells[ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 1, COLUMN_NURSE_DAY_START2 - 15 + iDay].Value = "";
                                                }
                                                else
                                                {
                                                    // Mod Start WataruT 2020.08.06 遅刻・早退入力対応
                                                    //// 最終計画データの勤務種類をセット
                                                    //WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START2 - 15 + iDay, ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 1, row2["name_short"].ToString());
                                                    // 遅刻・早退か判定
                                                    if (CheckWorkKindLateEarly(row2["name_short"].ToString()) == true)
                                                    {
                                                        // 遅刻・早退の実績時間を取得
                                                        strLateEarlyTime = GetOtherWorkTimeTotal(dtResultDetailItem, iDay + 1);
                                                        // 遅刻・早退の実績時間を取得
                                                        xlSheet.Cells[ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 1, COLUMN_NURSE_DAY_START2 - 15 + iDay].Value = strLateEarlyTime + "H" + row2["name_short"].ToString();
                                                    }
                                                    else
                                                    {
                                                        xlSheet.Cells[ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 1, COLUMN_NURSE_DAY_START2 - 15 + iDay].Value = row2["name_short"].ToString();
                                                    }
                                                    // Mod End   WataruT 2020.08.06 遅刻・早退入力対応
                                                }
                                                break;
                                            }
                                        }
                                        bNoDataFlag = true;
                                        break;
                                    }
                                }
                            }
                            // 初回計画データがない場合
                            else if (bNoDataFlag == false)
                            {
                                // 初回計画データ
                                xlSheet.Cells[ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 2, COLUMN_NURSE_DAY_START2 - 15 + iDay].Value = "";
                                // 最終計画データ
                                xlSheet.Cells[ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 1, COLUMN_NURSE_DAY_START2 - 15 + iDay].Value = "";
                            }
                        }
                    }

                    // 初回予定データ、最終実績データ(翌月)
                    for (int iStaff = ROW_NURSE_TOTAL_ROW; iStaff < astrScheduleStaffNurse.GetLength(0); iStaff++)
                    {
                        // 対象職員の計画データ取得
                        dtScheduleDetail = clsDatabaseControl.GetScheduleDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                            astrScheduleStaffNurse[iStaff, 0], "01", dtTargetNextMonth.ToString("yyyyMM"));
                        dtScheduleFirstDetail = clsDatabaseControl.GetScheduleFirstDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                                                astrScheduleStaffNurse[iStaff, 0], "01", dtTargetNextMonth.ToString("yyyyMM"));
                        // Add Start WataruT 2020.08.06 遅刻・早退入力対応
                        dtResultDetailItem = clsDatabaseControl.GetResultDetail_Ward_TargetDate_ResultDetailItem_Staff(pstrTargetWardCode, pstrTargetNextYear + pstrTargetNextMonth,
                                                "遅刻・早退のため", astrScheduleStaffNurse[iStaff, 0]);
                        // Add End   WataruT 2020.08.06 遅刻・早退入力対応

                        // 1日から順に処理
                        for (int iDay = 0; iDay < 15; iDay++)
                        {
                            // データなしフラグを初期化
                            bNoDataFlag = false;

                            // 初回計画データがある場合
                            if (dtScheduleFirstDetail.Rows.Count != 0)
                            {
                                // 初回計画データを順に確認
                                foreach (DataRow row in dtScheduleFirstDetail.Rows)
                                {
                                    // 対象日と一致する
                                    if (DateTime.Parse(row["target_date"].ToString()).Day == iDay + 1)
                                    {
                                        // 最終計画データを順に確認
                                        foreach (DataRow row2 in dtScheduleDetail.Rows)
                                        {
                                            // 対象日と一致する
                                            if (DateTime.Parse(row2["target_date"].ToString()).Day == iDay + 1)
                                            {
                                                // 初回計画データ
                                                // Mod Start WataruT 2020.08.05 計画表(締翌日)の不具合対応
                                                //WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START2 + iDay, ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 2, row["name_short"].ToString());
                                                xlSheet.Cells[ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 2, COLUMN_NURSE_DAY_START2 + iTargetMonthDay + iDay].Value = row["name_short"].ToString();
                                                // Mod End   WataruT 2020.08.05 計画表(締翌日)の不具合対応

                                                // 最終計画データと異なる場合
                                                if (row["name_short"].ToString() == row2["name_short"].ToString())
                                                {
                                                    // 最終計画データの勤務種類は空欄とする
                                                    // Mod Start WataruT 2020.08.05 計画表(締翌日)の不具合対応
                                                    //WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START2 + iDay, ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 1, "");
                                                    xlSheet.Cells[ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 1, COLUMN_NURSE_DAY_START2 + iTargetMonthDay + iDay].Value = "";
                                                    // Mod End   WataruT 2020.08.05 計画表(締翌日)の不具合対応
                                                }
                                                else
                                                {
                                                    // Mod Start WataruT 2020.08.06 遅刻・早退入力対応
                                                    //// 最終計画データの勤務種類をセット
                                                    //// Mod Start WataruT 2020.08.05 計画表(締翌日)の不具合対応
                                                    ////WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START2 + iDay, ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 1, row2["name_short"].ToString());
                                                    //WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START2 + iTargetMonthDay + iDay, ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 1, row2["name_short"].ToString());
                                                    //// Mod End   WataruT 2020.08.05 計画表(締翌日)の不具合対応
                                                    // 遅刻・早退か判定
                                                    if (CheckWorkKindLateEarly(row2["name_short"].ToString()) == true)
                                                    {
                                                        // 遅刻・早退の実績時間を取得
                                                        strLateEarlyTime = GetOtherWorkTimeTotal(dtResultDetailItem, iDay + 1);
                                                        // 遅刻・早退の実績時間を取得
                                                        xlSheet.Cells[ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 1, COLUMN_NURSE_DAY_START2 + iTargetMonthDay + iDay].Value = strLateEarlyTime + "H" + row2["name_short"].ToString();
                                                    }
                                                    else
                                                    {
                                                        xlSheet.Cells[ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 1, COLUMN_NURSE_DAY_START2 + iTargetMonthDay + iDay].Value = row2["name_short"].ToString();
                                                    }
                                                    // Mod End   WataruT 2020.08.06 遅刻・早退入力対応

                                                }
                                                break;
                                            }
                                        }
                                        bNoDataFlag = true;
                                        break;
                                    }
                                }
                            }
                            // 初回計画データがない場合
                            else if (bNoDataFlag == false)
                            {
                                // 初回計画データ
                                // Mod Start WataruT 2020.08.05 計画表(締翌日)の不具合対応
                                //WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START2 + iDay, ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 2, "");
                                xlSheet.Cells[ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 2, COLUMN_NURSE_DAY_START2 + iTargetMonthDay + iDay].Value = "";
                                // Mod End   WataruT 2020.08.05 計画表(締翌日)の不具合対応
                                // 最終計画データ
                                // Mod Start WataruT 2020.08.05 計画表(締翌日)の不具合対応
                                //WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START2 + iDay, ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 1, "");
                                xlSheet.Cells[ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 1, COLUMN_NURSE_DAY_START2 + iTargetMonthDay + iDay].Value = "";
                                // Mod End   WataruT 2020.08.05 計画表(締翌日)の不具合対応
                            }
                        }
                    }
                }
                else
                {
                    // 1ページ目のみ

                    // 職種、順番、職員氏名
                    for (int iStaff = 0; iStaff < astrScheduleStaffNurse.GetLength(0); iStaff++)
                    {
                        // 種別
                        xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, COLUMN_NURSE_STAFF_START1].Value = astrScheduleStaffNurse[iStaff, 2];
                        xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, COLUMN_NURSE_STAFF_START1].Value = "";

                        // 順番
                        xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, COLUMN_NURSE_STAFF_START1 + 2].Value = iStaff + 1;
                        xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, COLUMN_NURSE_STAFF_START1 + 2].Value = "";

                        // 氏名
                        xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, COLUMN_NURSE_STAFF_START1 + 3].Value = astrScheduleStaffNurse[iStaff, 1];
                        xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, COLUMN_NURSE_STAFF_START1 + 3].Value = "";
                    }

                    // 初回予定データ、最終実績データ(当月)
                    for (int iStaff = 0; iStaff < astrScheduleStaffNurse.GetLength(0); iStaff++)
                    {
                        // 対象職員の計画データ取得
                        dtScheduleDetail = clsDatabaseControl.GetScheduleDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                            astrScheduleStaffNurse[iStaff, 0], "01", dtTargetMonth.ToString("yyyyMM"));
                        dtScheduleFirstDetail = clsDatabaseControl.GetScheduleFirstDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                                                astrScheduleStaffNurse[iStaff, 0], "01", dtTargetMonth.ToString("yyyyMM"));
                        // Add Start WataruT 2020.08.06 遅刻・早退入力対応
                        dtResultDetailItem = clsDatabaseControl.GetResultDetail_Ward_TargetDate_ResultDetailItem_Staff(pstrTargetWardCode, pstrTargetYear + pstrTargetMonth,
                                                "遅刻・早退のため", astrScheduleStaffNurse[iStaff, 0]);
                        // Add End   WataruT 2020.08.06 遅刻・早退入力対応

                        // 1日から順に処理
                        for (int iDay = 15; iDay < DateTime.DaysInMonth(dtTargetMonth.Year, dtTargetMonth.Month); iDay++)
                        {
                            // データなしフラグを初期化
                            bNoDataFlag = false;

                            // 初回計画データがある場合
                            if (dtScheduleFirstDetail.Rows.Count != 0)
                            {
                                // 初回計画データを順に確認
                                foreach (DataRow row in dtScheduleFirstDetail.Rows)
                                {
                                    // 対象日と一致する
                                    if (DateTime.Parse(row["target_date"].ToString()).Day == iDay + 1)
                                    {
                                        // 最終計画データを順に確認
                                        foreach (DataRow row2 in dtScheduleDetail.Rows)
                                        {
                                            // 対象日と一致する
                                            if (DateTime.Parse(row2["target_date"].ToString()).Day == iDay + 1)
                                            {
                                                // 初回計画データ
                                                xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, COLUMN_NURSE_DAY_START1 - 15 + iDay].Value = row["name_short"].ToString();

                                                // 最終計画データと異なる場合
                                                if (row["name_short"].ToString() == row2["name_short"].ToString())
                                                {
                                                    // 最終計画データの勤務種類は空欄とする
                                                    xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, COLUMN_NURSE_DAY_START1 - 15 + iDay].Value = "";
                                                }
                                                else
                                                {
                                                    // Mod Start WataruT 2020.08.06 遅刻・早退入力対応
                                                    //// 最終計画データの勤務種類をセット
                                                    //WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 - 15 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, row2["name_short"].ToString());
                                                    // 遅刻・早退か判定
                                                    if (CheckWorkKindLateEarly(row2["name_short"].ToString()) == true)
                                                    {
                                                        // 遅刻・早退の実績時間を取得
                                                        strLateEarlyTime = GetOtherWorkTimeTotal(dtResultDetailItem, iDay + 1);
                                                        // 遅刻・早退の実績時間を取得
                                                        xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, COLUMN_NURSE_DAY_START1 - 15 + iDay].Value = strLateEarlyTime + "H" + row2["name_short"].ToString();
                                                    }
                                                    else
                                                    {
                                                        xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, COLUMN_NURSE_DAY_START1 - 15 + iDay].Value = row2["name_short"].ToString();
                                                    }
                                                    // Mod End   WataruT 2020.08.06 遅刻・早退入力対応
                                                }
                                                break;
                                            }
                                        }
                                        bNoDataFlag = true;
                                        break;
                                    }
                                }
                            }
                            // 初回計画データがない場合
                            else if (bNoDataFlag == false)
                            {
                                // 初回計画データ
                                xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, COLUMN_NURSE_DAY_START1 - 15 + iDay].Value = "";
                                // 最終計画データ
                                xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, COLUMN_NURSE_DAY_START1 - 15 + iDay].Value = "";
                            }
                        }
                    }

                    // 初回予定データ、最終実績データ(翌月)
                    for (int iStaff = 0; iStaff < astrScheduleStaffNurse.GetLength(0); iStaff++)
                    {
                        // 対象職員の計画データ取得
                        dtScheduleDetail = clsDatabaseControl.GetScheduleDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                            astrScheduleStaffNurse[iStaff, 0], "01", dtTargetNextMonth.ToString("yyyyMM"));
                        dtScheduleFirstDetail = clsDatabaseControl.GetScheduleFirstDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                                                astrScheduleStaffNurse[iStaff, 0], "01", dtTargetNextMonth.ToString("yyyyMM"));
                        // Add Start WataruT 2020.08.06 遅刻・早退入力対応
                        dtResultDetailItem = clsDatabaseControl.GetResultDetail_Ward_TargetDate_ResultDetailItem_Staff(pstrTargetWardCode, pstrTargetNextYear + pstrTargetNextMonth,
                                                "遅刻・早退のため", astrScheduleStaffNurse[iStaff, 0]);
                        // Add End   WataruT 2020.08.06 遅刻・早退入力対応

                        // 1日から順に処理
                        for (int iDay = 0; iDay < 15; iDay++)
                        {
                            // データなしフラグを初期化
                            bNoDataFlag = false;

                            // 初回計画データがある場合
                            if (dtScheduleFirstDetail.Rows.Count != 0)
                            {
                                // 初回計画データを順に確認
                                foreach (DataRow row in dtScheduleFirstDetail.Rows)
                                {
                                    // 対象日と一致する
                                    if (DateTime.Parse(row["target_date"].ToString()).Day == iDay + 1)
                                    {
                                        // 最終計画データを順に確認
                                        foreach (DataRow row2 in dtScheduleDetail.Rows)
                                        {
                                            // 対象日と一致する
                                            if (DateTime.Parse(row2["target_date"].ToString()).Day == iDay + 1)
                                            {
                                                // 初回計画データ
                                                // Mod Start WataruT 2020.08.05 計画表(締翌日)の不具合対応
                                                //WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 + 15 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, row["name_short"].ToString());
                                                xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, COLUMN_NURSE_DAY_START1 + iTargetMonthDay + iDay].Value = row["name_short"].ToString();
                                                // Mod End   WataruT 2020.08.05 計画表(締翌日)の不具合対応

                                                // 最終計画データと異なる場合
                                                if (row["name_short"].ToString() == row2["name_short"].ToString())
                                                {
                                                    // 最終計画データの勤務種類は空欄とする
                                                    // Mod Start WataruT 2020.08.05 計画表(締翌日)の不具合対応
                                                    //WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 + 15 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, "");
                                                    xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, COLUMN_NURSE_DAY_START1 + iTargetMonthDay + iDay].Value = "";
                                                    // Mod End   WataruT 2020.08.05 計画表(締翌日)の不具合対応
                                                }
                                                else
                                                {
                                                    // Mod Start WataruT 2020.08.06 遅刻・早退入力対応
                                                    //// 最終計画データの勤務種類をセット
                                                    //// Mod Start WataruT 2020.08.05 計画表(締翌日)の不具合対応
                                                    ////WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 + 15 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, row2["name_short"].ToString());
                                                    //WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 + iTargetMonthDay + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, row2["name_short"].ToString());
                                                    //// Mod End   WataruT 2020.08.05 計画表(締翌日)の不具合対応
                                                    // 遅刻・早退か判定
                                                    if (CheckWorkKindLateEarly(row2["name_short"].ToString()) == true)
                                                    {
                                                        // 遅刻・早退の実績時間を取得
                                                        strLateEarlyTime = GetOtherWorkTimeTotal(dtResultDetailItem, iDay + 1);
                                                        // 遅刻・早退の実績時間を取得
                                                        xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, COLUMN_NURSE_DAY_START1 + iTargetMonthDay + iDay].Value = strLateEarlyTime + "H" + row2["name_short"].ToString();
                                                    }
                                                    else
                                                    {
                                                        xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, COLUMN_NURSE_DAY_START1 + iTargetMonthDay + iDay].Value = row2["name_short"].ToString();
                                                    }
                                                    // Mod End   WataruT 2020.08.06 遅刻・早退入力対応
                                                }
                                                break;
                                            }
                                        }
                                        bNoDataFlag = true;
                                        break;
                                    }
                                }
                            }
                            // 初回計画データがない場合
                            else if (bNoDataFlag == false)
                            {
                                // 初回計画データ
                                // Mod Start WataruT 2020.08.05 計画表(締翌日)の不具合対応
                                //WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 + 15 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, "");
                                xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, COLUMN_NURSE_DAY_START1 + iTargetMonthDay + iDay].Value = "";
                                // Mod End   WataruT 2020.08.05 計画表(締翌日)の不具合対応
                                // 最終計画データ
                                // Mod Start WataruT 2020.08.05 計画表(締翌日)の不具合対応
                                //WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 + 15 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, "");
                                xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, COLUMN_NURSE_DAY_START1 + iTargetMonthDay + iDay].Value = "";
                                // Mod End   WataruT 2020.08.05 計画表(締翌日)の不具合対応
                            }
                        }
                    }
                }

                // == ケア ==

                // 職種、順番、職員氏名
                for (int iStaff = 0; iStaff < astrScheduleStaffCare.GetLength(0); iStaff++)
                {
                    // 順番
                    xlSheet.Cells[ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 2, COLUMN_CARE_STAFF_START].Value = iStaff + 1;
                    xlSheet.Cells[ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 1, COLUMN_CARE_STAFF_START].Value = "";

                    // 氏名
                    xlSheet.Cells[ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 2, COLUMN_CARE_STAFF_START + 1].Value = astrScheduleStaffCare[iStaff, 1];
                    xlSheet.Cells[ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 1, COLUMN_CARE_STAFF_START + 1].Value = "";
                }

                // 初回予定データ、最終実績データ(当月)
                for (int iStaff = 0; iStaff < astrScheduleStaffCare.GetLength(0); iStaff++)
                {
                    // 対象職員の計画データ取得
                    dtScheduleDetail = clsDatabaseControl.GetScheduleDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                        astrScheduleStaffCare[iStaff, 0], "02", dtTargetMonth.ToString("yyyyMM"));
                    dtScheduleFirstDetail = clsDatabaseControl.GetScheduleFirstDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                                            astrScheduleStaffCare[iStaff, 0], "02", dtTargetMonth.ToString("yyyyMM"));
                    // Add Start WataruT 2020.08.06 遅刻・早退入力対応
                    dtResultDetailItem = clsDatabaseControl.GetResultDetail_Ward_TargetDate_ResultDetailItem_Staff(pstrTargetWardCode, pstrTargetYear + pstrTargetMonth,
                                            "遅刻・早退のため", astrScheduleStaffCare[iStaff, 0]);
                    // Add End   WataruT 2020.08.06 遅刻・早退入力対応
                    // 1日から順に処理
                    for (int iDay = 15; iDay < DateTime.DaysInMonth(dtTargetMonth.Year, dtTargetMonth.Month); iDay++)
                    {
                        // データなしフラグを初期化
                        bNoDataFlag = false;

                        // 初回計画データがある場合
                        if (dtScheduleFirstDetail.Rows.Count != 0)
                        {
                            // 初回計画データを順に確認
                            foreach (DataRow row in dtScheduleFirstDetail.Rows)
                            {
                                // 対象日と一致する
                                if (DateTime.Parse(row["target_date"].ToString()).Day == iDay + 1)
                                {
                                    // 最終計画データを順に確認
                                    foreach (DataRow row2 in dtScheduleDetail.Rows)
                                    {
                                        // 対象日と一致する
                                        if (DateTime.Parse(row2["target_date"].ToString()).Day == iDay + 1)
                                        {
                                            // 初回計画データ
                                            xlSheet.Cells[ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 2, COLUMN_CARE_DAY_START - 15 + iDay].Value = row["name_short"].ToString();

                                            // 最終計画データと異なる場合
                                            if (row["name_short"].ToString() == row2["name_short"].ToString())
                                            {
                                                // 最終計画データの勤務種類は空欄とする
                                                xlSheet.Cells[ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 1, COLUMN_CARE_DAY_START - 15 + iDay].Value = "";
                                            }
                                            else
                                            {
                                                // Mod Start WataruT 2020.08.06 遅刻・早退入力対応
                                                //// 最終計画データの勤務種類をセット
                                                //WriteCellValue(xlSheet, COLUMN_CARE_DAY_START - 15 + iDay, ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 1, row2["name_short"].ToString());
                                                // 遅刻・早退か判定
                                                if (CheckWorkKindLateEarly(row2["name_short"].ToString()) == true)
                                                {
                                                    // 遅刻・早退の実績時間を取得
                                                    strLateEarlyTime = GetOtherWorkTimeTotal(dtResultDetailItem, iDay + 1);
                                                    // 遅刻・早退の実績時間を取得
                                                    xlSheet.Cells[ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 1, COLUMN_CARE_DAY_START - 15 + iDay].Value = strLateEarlyTime + "H" + row2["name_short"].ToString();
                                                }
                                                else
                                                {
                                                    xlSheet.Cells[ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 1, COLUMN_CARE_DAY_START - 15 + iDay].Value = row2["name_short"].ToString();
                                                }
                                                // Mod End   WataruT 2020.08.06 遅刻・早退入力対応
                                            }
                                            break;
                                        }
                                    }
                                    bNoDataFlag = true;
                                    break;
                                }
                            }
                        }
                        // 初回計画データがない場合
                        else if (bNoDataFlag == false)
                        {
                            // 初回計画データ
                            xlSheet.Cells[ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 2, COLUMN_CARE_DAY_START - 15 + iDay].Value = "";
                            // 最終計画データ
                            xlSheet.Cells[ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 1, COLUMN_CARE_DAY_START - 15 + iDay].Value = "";
                        }
                    }
                }

                // 初回予定データ、最終実績データ(翌月)
                for (int iStaff = 0; iStaff < astrScheduleStaffCare.GetLength(0); iStaff++)
                {
                    // 対象職員の計画データ取得
                    dtScheduleDetail = clsDatabaseControl.GetScheduleDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                        astrScheduleStaffCare[iStaff, 0], "02", dtTargetNextMonth.ToString("yyyyMM"));
                    dtScheduleFirstDetail = clsDatabaseControl.GetScheduleFirstDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                                            astrScheduleStaffCare[iStaff, 0], "02", dtTargetNextMonth.ToString("yyyyMM"));
                    // Add Start WataruT 2020.08.06 遅刻・早退入力対応
                    dtResultDetailItem = clsDatabaseControl.GetResultDetail_Ward_TargetDate_ResultDetailItem_Staff(pstrTargetWardCode, pstrTargetNextYear + pstrTargetNextMonth,
                                            "遅刻・早退のため", astrScheduleStaffCare[iStaff, 0]);
                    // Add End   WataruT 2020.08.06 遅刻・早退入力対応

                    // 1日から順に処理
                    for (int iDay = 0; iDay < 15; iDay++)
                    {
                        // データなしフラグを初期化
                        bNoDataFlag = false;

                        // 初回計画データがある場合
                        if (dtScheduleFirstDetail.Rows.Count != 0)
                        {
                            // 初回計画データを順に確認
                            foreach (DataRow row in dtScheduleFirstDetail.Rows)
                            {
                                // 対象日と一致する
                                if (DateTime.Parse(row["target_date"].ToString()).Day == iDay + 1)
                                {
                                    // 最終計画データを順に確認
                                    foreach (DataRow row2 in dtScheduleDetail.Rows)
                                    {
                                        // 対象日と一致する
                                        if (DateTime.Parse(row2["target_date"].ToString()).Day == iDay + 1)
                                        {
                                            // 初回計画データ
                                            // Mod Start WataruT 2020.08.05 計画表(締翌日)の不具合対応
                                            //WriteCellValue(xlSheet, COLUMN_CARE_DAY_START + iDay, ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 2, row["name_short"].ToString());
                                            xlSheet.Cells[ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 2, COLUMN_CARE_DAY_START + iTargetMonthDay + iDay].Value = row["name_short"].ToString();
                                            // Mod End   WataruT 2020.08.05 計画表(締翌日)の不具合対応

                                            // 最終計画データと異なる場合
                                            if (row["name_short"].ToString() == row2["name_short"].ToString())
                                            {
                                                // 最終計画データの勤務種類は空欄とする
                                                // Mod Start WataruT 2020.08.05 計画表(締翌日)の不具合対応
                                                //WriteCellValue(xlSheet, COLUMN_CARE_DAY_START + iDay, ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 1, "");
                                                xlSheet.Cells[ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 1, COLUMN_CARE_DAY_START + iTargetMonthDay + iDay].Value = "";
                                                // Mod End   WataruT 2020.08.05 計画表(締翌日)の不具合対応
                                            }
                                            else
                                            {
                                                // Mod Start WataruT 2020.08.06 遅刻・早退入力対応
                                                //// 最終計画データの勤務種類をセット
                                                //// Mod Start WataruT 2020.08.05 計画表(締翌日)の不具合対応
                                                ////WriteCellValue(xlSheet, COLUMN_CARE_DAY_START + iDay, ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 1, row2["name_short"].ToString());
                                                //WriteCellValue(xlSheet, COLUMN_CARE_DAY_START + iTargetMonthDay + iDay, ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 1, row2["name_short"].ToString());
                                                //// Mod End   WataruT 2020.08.05 計画表(締翌日)の不具合対応
                                                // 遅刻・早退か判定
                                                if (CheckWorkKindLateEarly(row2["name_short"].ToString()) == true)
                                                {
                                                    // 遅刻・早退の実績時間を取得
                                                    strLateEarlyTime = GetOtherWorkTimeTotal(dtResultDetailItem, iDay + 1);
                                                    // 遅刻・早退の実績時間を取得
                                                    xlSheet.Cells[ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 1, COLUMN_CARE_DAY_START + iTargetMonthDay + iDay].Value = strLateEarlyTime + "H" + row2["name_short"].ToString();
                                                }
                                                else
                                                {
                                                    xlSheet.Cells[ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 1, COLUMN_CARE_DAY_START + iTargetMonthDay + iDay].Value = row2["name_short"].ToString();
                                                }
                                                // Mod End   WataruT 2020.08.06 遅刻・早退入力対応
                                            }
                                            break;
                                        }
                                    }
                                    bNoDataFlag = true;
                                    break;
                                }
                            }
                        }
                        // 初回計画データがない場合
                        else if (bNoDataFlag == false)
                        {
                            // 初回計画データ
                            // Mod Start WataruT 2020.08.05 計画表(締翌日)の不具合対応
                            //WriteCellValue(xlSheet, COLUMN_CARE_DAY_START + iDay, ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 2, "");
                            xlSheet.Cells[ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 2, COLUMN_CARE_DAY_START + iTargetMonthDay + iDay].Value = "";
                            // Mod End   WataruT 2020.08.05 計画表(締翌日)の不具合対応
                            // 最終計画データ
                            // Mod Start WataruT 2020.08.05 計画表(締翌日)の不具合対応
                            //WriteCellValue(xlSheet, COLUMN_CARE_DAY_START + iDay, ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 1, "");
                            xlSheet.Cells[ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 1, COLUMN_CARE_DAY_START + iTargetMonthDay + iDay].Value = "";
                            // Mod End   WataruT 2020.08.05 計画表(締翌日)の不具合対応
                        }
                    }
                }

                // 印刷範囲の指定
                xlSheet.PrinterSettings.PrintArea = xlSheet.Cells[1, 1, 137, 36];

                // 不要なページを削除
                if (astrScheduleStaffNurse.GetLength(0) <= ROW_NURSE_TOTAL_ROW)
                {
                    xlSheet.DeleteRow(48, 47, true);
                }

                // ファイルを保存
                xlFile.SaveAs(new FileInfo(sfd.FileName));
            }

            // 終了メッセージ
            MessageBox.Show("保存完了");
        }

        /// <summary>
        /// オブジェクト初期化
        /// </summary>
        private void InitializeObject()
        {
            DataTable dt;
            int iStaffCount = 0;                        // Add WataruT 2020.08.17 勤務表(締翌日)の不具合対応
            int iStaffInsertCount;                      // Add WataruT 2020.08.17 勤務表(締翌日)の不具合対応
            bool bDummyCheckFlag;                       // Add WataruT 2020.08.17 勤務表(締翌日)の不具合対応
            string[,] astrScheduleStaffDummy;           // Add WataruT 2020.08.17 勤務表(締翌日)の不具合対応

            // 様式9チェックデータ

            // 職員リスト一覧(看護師)
            dt = clsDatabaseControl.GetScheduleStaff_Youshiki9_Half(pstrTargetWardCode, pstrTargetYear + pstrTargetMonth, pstrTargetNextYear + pstrTargetNextMonth, "01");

            // Mod Start WataruT 2020.08.17 勤務表(締翌日)の不具合対応
            //astrScheduleStaffNurse = new string[dt.Rows.Count, 3];
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    astrScheduleStaffNurse[i, 0] = dt.Rows[i]["id"].ToString();
            //    astrScheduleStaffNurse[i, 1] = dt.Rows[i]["name"].ToString();
            //    astrScheduleStaffNurse[i, 2] = dt.Rows[i]["staff_kind"].ToString();
            //}
            astrScheduleStaffDummy = new string[dt.Rows.Count, 3];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bDummyCheckFlag = false;
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    if (astrScheduleStaffDummy[j, 0] == dt.Rows[i]["id"].ToString() &&
                        astrScheduleStaffDummy[j, 1] == dt.Rows[i]["name"].ToString() &&
                        astrScheduleStaffDummy[j, 2] == dt.Rows[i]["staff_kind"].ToString())
                    {
                        bDummyCheckFlag = true;
                        break;
                    }
                }
                if (bDummyCheckFlag == false)
                {
                    astrScheduleStaffDummy[i, 0] = dt.Rows[i]["id"].ToString();
                    astrScheduleStaffDummy[i, 1] = dt.Rows[i]["name"].ToString();
                    astrScheduleStaffDummy[i, 2] = dt.Rows[i]["staff_kind"].ToString();
                    iStaffCount += 1;
                }
                else
                {
                    astrScheduleStaffDummy[i, 0] = "";
                    astrScheduleStaffDummy[i, 1] = "";
                    astrScheduleStaffDummy[i, 2] = "";
                }
            }
            astrScheduleStaffNurse = new string[iStaffCount, 3];
            iStaffInsertCount = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (astrScheduleStaffDummy[i, 0] != "")
                {
                    astrScheduleStaffNurse[iStaffInsertCount, 0] = astrScheduleStaffDummy[i, 0];
                    astrScheduleStaffNurse[iStaffInsertCount, 1] = astrScheduleStaffDummy[i, 1];
                    astrScheduleStaffNurse[iStaffInsertCount, 2] = astrScheduleStaffDummy[i, 2];
                    iStaffInsertCount += 1;
                }
            }
            // Mod End  WataruT 2020.08.17 勤務表(締翌日)の不具合対応

            // 職員リスト一覧(ケア)
            dt = clsDatabaseControl.GetScheduleStaff_Youshiki9_Half(pstrTargetWardCode, pstrTargetYear + pstrTargetMonth, pstrTargetNextYear + pstrTargetNextMonth, "02");
            // Mod Start  WataruT 2020.08.17 勤務表(締翌日)の不具合対応
            //astrScheduleStaffCare = new string[dt.Rows.Count, 3];
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    astrScheduleStaffCare[i, 0] = dt.Rows[i]["id"].ToString();
            //    astrScheduleStaffCare[i, 1] = dt.Rows[i]["name"].ToString();
            //    astrScheduleStaffCare[i, 2] = dt.Rows[i]["staff_kind"].ToString();
            //}
            astrScheduleStaffDummy = new string[dt.Rows.Count, 3];
            iStaffCount = 0;
            iStaffInsertCount = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bDummyCheckFlag = false;
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    if (astrScheduleStaffDummy[j, 0] == dt.Rows[i]["id"].ToString() &&
                        astrScheduleStaffDummy[j, 1] == dt.Rows[i]["name"].ToString() &&
                        astrScheduleStaffDummy[j, 2] == dt.Rows[i]["staff_kind"].ToString())
                    {
                        bDummyCheckFlag = true;
                        break;
                    }
                }
                if (bDummyCheckFlag == false)
                {
                    astrScheduleStaffDummy[i, 0] = dt.Rows[i]["id"].ToString();
                    astrScheduleStaffDummy[i, 1] = dt.Rows[i]["name"].ToString();
                    astrScheduleStaffDummy[i, 2] = dt.Rows[i]["staff_kind"].ToString();
                    iStaffCount += 1;
                }
                else
                {
                    astrScheduleStaffDummy[i, 0] = "";
                    astrScheduleStaffDummy[i, 1] = "";
                    astrScheduleStaffDummy[i, 2] = "";
                }
            }
            astrScheduleStaffCare = new string[iStaffCount, 3];
            iStaffInsertCount = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (astrScheduleStaffDummy[i, 0] != "")
                {
                    astrScheduleStaffCare[iStaffInsertCount, 0] = astrScheduleStaffDummy[i, 0];
                    astrScheduleStaffCare[iStaffInsertCount, 1] = astrScheduleStaffDummy[i, 1];
                    astrScheduleStaffCare[iStaffInsertCount, 2] = astrScheduleStaffDummy[i, 2];
                    iStaffInsertCount += 1;
                }
            }
            // Mod End  WataruT 2020.08.17 勤務表(締翌日)の不具合対応

            // Add Start WataruT 2020.08.17 勤務表(締翌日)の日曜と祝日の背景色を変更
            /// 祝日マスタ
            dt = clsDatabaseControl.GetHoliday();
            astrHoliday = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                astrHoliday[i] = dt.Rows[i]["holiday"].ToString();
            }
            // Add End   WataruT 2020.08.17 勤務表(締翌日)の日曜と祝日の背景色を変更
        }

        /// <summary>
        /// 保存ダイアログのプロパティ設定
        /// </summary>
        private void SetSaveFileDialogProperties(ref SaveFileDialog sfd)
        {
            // ファイル名の既定値(YYYY年MM月_〇病棟_様式9.xlsx)
            sfd.FileName = pstrTargetYear + pstrTargetMonth + "_" + pstrTargetWard + "_" + "勤務計画表.xlsx";
            // 既定フォルダ
            sfd.InitialDirectory = @"C:\";
            // ファイル種類フィルタ
            sfd.Filter = "Excelファイル(*.xlsx)|*.xlsx";
            //タイトルを設定する
            sfd.Title = "保存先を選択してください。";
        }

        /// <summary>
        /// 遅刻・早退かチェック
        /// Add WataruT 2020.08.06 遅刻・早退入力対応
        /// </summary>
        /// <returns></returns>
        private bool CheckWorkKindLateEarly(string strWorkKind)
        {
            // 遅刻・早退か判定
            if (strWorkKind == "遅刻" || strWorkKind == "早退")
                return true;
            else
                return false;
        }

        /// <summary>
        /// その他業務の合計時間を返す
        /// Add WataruT 2020.08.06 遅刻・早退入力対応
        /// </summary>
        private string GetOtherWorkTimeTotal(DataTable dtResultDetailItem, int iTargetDay)
        {
            string strTotalTime;                                // 合計時間
            double dOtherWorkTimeTotal = 0;                     // 計算時の一時変数
            // Add Start WataruT 2020/08/17 昼休みの時間を遅刻・早退の実績から引いて印字する
            DateTime dtStartTime;
            DateTime dtEndTime;
            // Add End   WataruT 2020/08/17 昼休みの時間を遅刻・早退の実績から引いて印字する


            foreach (DataRow row in dtResultDetailItem.Rows)
            {
                if (DateTime.Parse(row["target_date_basic"].ToString()).Day == iTargetDay)
                {
                    // Mod Start WataruT 2020/08/17 昼休みの時間を遅刻・早退の実績から引いて印字する
                    //dOtherWorkTimeTotal += double.Parse(DateTime.Parse(row["total_time"].ToString()).Hour.ToString()) +
                    //                      double.Parse(DateTime.Parse(row["total_time"].ToString()).Minute.ToString()) / 60;
                    dtStartTime = DateTime.Parse(row["start_time"].ToString());
                    dtEndTime = DateTime.Parse(row["end_time"].ToString());
                    if (DateTime.Compare(dtStartTime, DateTime.Parse("12:00:01")) < 0 && DateTime.Compare(dtEndTime, DateTime.Parse("12:59:59")) > 0)
                    {
                        dOtherWorkTimeTotal += double.Parse(DateTime.Parse(row["total_time"].ToString()).Hour.ToString()) +
                                          double.Parse(DateTime.Parse(row["total_time"].ToString()).Minute.ToString()) / 60 - 1;
                    }
                    else
                    {
                        dOtherWorkTimeTotal += double.Parse(DateTime.Parse(row["total_time"].ToString()).Hour.ToString()) +
                                          double.Parse(DateTime.Parse(row["total_time"].ToString()).Minute.ToString()) / 60;
                    }
                    // Mod End   WataruT 2020/08/17 昼休みの時間を遅刻・早退の実績から引いて印字する
                }
            }

            // 数字を文字列に変換
            strTotalTime = dOtherWorkTimeTotal.ToString();

            return strTotalTime;
        }
    }
}
