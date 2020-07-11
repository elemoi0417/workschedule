using System;
using System.Data;
using System.Windows.Forms;
using workschedule.Controls;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;

namespace workschedule.Reports
{
    class PrintWorkScheduleHalf
    {
        // 使用クラス宣言
        DatabaseControl clsDatabaseControl = new DatabaseControl();

        // 定数
        const int COLUMN_CREATE_YEAR1 = 0;
        const int COLUMN_CREATE_MONTH1 = 2;
        const int COLUMN_CREATE_NEXT_YEAR1 = 4;
        const int COLUMN_CREATE_NEXT_MONTH1 = 6;
        const int COLUMN_WARD1 = 27;
        const int COLUMN_CREATE_YEAR2 = 0;
        const int COLUMN_CREATE_MONTH2 = 2;
        const int COLUMN_CREATE_NEXT_YEAR2 = 4;
        const int COLUMN_CREATE_NEXT_MONTH2 = 6;
        const int COLUMN_WARD2 = 27;
        const int COLUMN_CREATE_YEAR3 = 0;
        const int COLUMN_CREATE_MONTH3 = 2;
        const int COLUMN_CREATE_NEXT_YEAR3 = 4;
        const int COLUMN_CREATE_NEXT_MONTH3 = 6;
        const int COLUMN_WARD3 = 27;
        
        const int COLUMN_NURSE_DAY_START1 = 5;
        const int COLUMN_NURSE_DAY_START2 = 5;
        const int COLUMN_CARE_DAY_START = 5;
        const int COLUMN_NURSE_DAY_OF_WEEK1 = 5;
        const int COLUMN_NURSE_DAY_OF_WEEK2 = 5;
        const int COLUMN_CARE_DAY_OF_WEEK = 5;
        const int COLUMN_NURSE_STAFF_START1 = 0;
        const int COLUMN_NURSE_STAFF_START2 = 0;
        const int COLUMN_CARE_STAFF_START = 2;

        const int ROW_CREATE_YEAR1 = 1;
        const int ROW_CREATE_MONTH1 = 1;
        const int ROW_CREATE_NEXT_YEAR1 = 1;
        const int ROW_CREATE_NEXT_MONTH1 = 1;
        const int ROW_WARD1 = 0;
        const int ROW_CREATE_YEAR2 = 48;
        const int ROW_CREATE_MONTH2 = 48;
        const int ROW_CREATE_NEXT_YEAR2 = 48;
        const int ROW_CREATE_NEXT_MONTH2 = 48;
        const int ROW_WARD2 = 47;
        const int ROW_CREATE_YEAR3 = 95;
        const int ROW_CREATE_MONTH3 = 95;
        const int ROW_CREATE_NEXT_YEAR3 = 95;
        const int ROW_CREATE_NEXT_MONTH3 = 95;
        const int ROW_WARD3 = 94;

        const int ROW_NURSE_DAY_START1 = 3;
        const int ROW_NURSE_DAY_START2 = 50;
        const int ROW_CARE_DAY_START = 97;
        const int ROW_NURSE_DAY_OF_WEEK1 = 4;
        const int ROW_NURSE_DAY_OF_WEEK2 = 51;
        const int ROW_CARE_DAY_OF_WEEK = 98;
        const int ROW_NURSE_STAFF_START1 = 5;
        const int ROW_NURSE_STAFF_START2 = 52;
        const int ROW_CARE_STAFF_START = 99;

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
            DateTime dtTargetMonth = DateTime.ParseExact(pstrTargetYear + pstrTargetMonth + "01", "yyyyMMdd", null);
            DateTime dtTargetNextMonth = DateTime.ParseExact(pstrTargetNextYear + pstrTargetNextMonth + "01", "yyyyMMdd", null);
            DataTable dtScheduleDetail;
            DataTable dtScheduleFirstDetail;
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

            // Officeオブジェクトの初期化
            IWorkbook xlWorkbook = WorkbookFactory.Create(strFilePath);
            ISheet xlSheet = xlWorkbook.GetSheet("シート");

            // === Excelデータ入力 ===

            // 作成年月
            WriteCellValue(xlSheet, COLUMN_CREATE_YEAR1, ROW_CREATE_YEAR1, pstrTargetYear);
            WriteCellValue(xlSheet, COLUMN_CREATE_YEAR2, ROW_CREATE_YEAR2, pstrTargetYear);
            WriteCellValue(xlSheet, COLUMN_CREATE_YEAR3, ROW_CREATE_YEAR3, pstrTargetYear);
            WriteCellValue(xlSheet, COLUMN_CREATE_MONTH1, ROW_CREATE_MONTH1, pstrTargetMonth);
            WriteCellValue(xlSheet, COLUMN_CREATE_MONTH2, ROW_CREATE_MONTH2, pstrTargetMonth);
            WriteCellValue(xlSheet, COLUMN_CREATE_MONTH3, ROW_CREATE_MONTH3, pstrTargetMonth);
            WriteCellValue(xlSheet, COLUMN_CREATE_NEXT_YEAR1, ROW_CREATE_NEXT_YEAR1, pstrTargetNextYear);
            WriteCellValue(xlSheet, COLUMN_CREATE_NEXT_YEAR2, ROW_CREATE_NEXT_YEAR2, pstrTargetNextYear);
            WriteCellValue(xlSheet, COLUMN_CREATE_NEXT_YEAR3, ROW_CREATE_NEXT_YEAR3, pstrTargetNextYear);
            WriteCellValue(xlSheet, COLUMN_CREATE_NEXT_MONTH1, ROW_CREATE_NEXT_MONTH1, pstrTargetNextMonth);
            WriteCellValue(xlSheet, COLUMN_CREATE_NEXT_MONTH2, ROW_CREATE_NEXT_MONTH2, pstrTargetNextMonth);
            WriteCellValue(xlSheet, COLUMN_CREATE_NEXT_MONTH3, ROW_CREATE_NEXT_MONTH3, pstrTargetNextMonth);

            // 対象病棟
            WriteCellValue(xlSheet, COLUMN_WARD1, ROW_WARD1, "第" + pstrTargetWard);
            WriteCellValue(xlSheet, COLUMN_WARD2, ROW_WARD2, "第" + pstrTargetWard);
            WriteCellValue(xlSheet, COLUMN_WARD3, ROW_WARD3, "第" + pstrTargetWard);


            // == 日付・曜日 ==

            // 当月
            for (int i = 15; i < DateTime.DaysInMonth(int.Parse(pstrTargetYear), int.Parse(pstrTargetMonth)); i++)
            {
                // 日にち
                WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 - 15 + i, ROW_NURSE_DAY_START1, i + 1);
                WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START2 - 15 + i, ROW_NURSE_DAY_START2, i + 1);
                WriteCellValue(xlSheet, COLUMN_CARE_DAY_START - 15 + i, ROW_CARE_DAY_START, i + 1);
                // 曜日
                WriteCellValue(xlSheet, COLUMN_NURSE_DAY_OF_WEEK1 - 15 + i, ROW_NURSE_DAY_OF_WEEK1, dtTargetMonth.AddDays(double.Parse(i.ToString())).ToString("ddd") + "曜");
                WriteCellValue(xlSheet, COLUMN_NURSE_DAY_OF_WEEK2 - 15 + i, ROW_NURSE_DAY_OF_WEEK2, dtTargetMonth.AddDays(double.Parse(i.ToString())).ToString("ddd") + "曜");
                WriteCellValue(xlSheet, COLUMN_CARE_DAY_OF_WEEK - 15 + i, ROW_CARE_DAY_OF_WEEK, dtTargetMonth.AddDays(double.Parse(i.ToString())).ToString("ddd") + "曜");
            }

            // 翌月
            for (int i = 0; i < 15; i++)
            {
                // 日にち
                WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 + iTargetMonthDay + i, ROW_NURSE_DAY_START1, i + 1);
                WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START2 + iTargetMonthDay + i, ROW_NURSE_DAY_START2, i + 1);
                WriteCellValue(xlSheet, COLUMN_CARE_DAY_START + iTargetMonthDay + i, ROW_CARE_DAY_START, i + 1);
                // 曜日
                WriteCellValue(xlSheet, COLUMN_NURSE_DAY_OF_WEEK1 + iTargetMonthDay + i, ROW_NURSE_DAY_OF_WEEK1, dtTargetNextMonth.AddDays(double.Parse(i.ToString())).ToString("ddd") + "曜");
                WriteCellValue(xlSheet, COLUMN_NURSE_DAY_OF_WEEK2 + iTargetMonthDay + i, ROW_NURSE_DAY_OF_WEEK2, dtTargetNextMonth.AddDays(double.Parse(i.ToString())).ToString("ddd") + "曜");
                WriteCellValue(xlSheet, COLUMN_CARE_DAY_OF_WEEK + iTargetMonthDay + i, ROW_CARE_DAY_OF_WEEK, dtTargetNextMonth.AddDays(double.Parse(i.ToString())).ToString("ddd") + "曜");
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
                    WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START1, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, astrScheduleStaffNurse[iStaff, 2]);
                    WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START1, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, "");

                    // 順番
                    WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START1 + 2, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, iStaff + 1);
                    WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START1 + 2, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, "");

                    // 氏名
                    WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START1 + 3, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, astrScheduleStaffNurse[iStaff, 1]);
                    WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START1 + 3, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, "");
                }

                // 初回予定データ、最終実績データ(当月)
                for (int iStaff = 0; iStaff < ROW_NURSE_TOTAL_ROW; iStaff++)
                {
                    // 対象職員の計画データ取得
                    dtScheduleDetail = clsDatabaseControl.GetScheduleDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                        astrScheduleStaffNurse[iStaff, 0], "01", dtTargetMonth.ToString("yyyyMM"));
                    dtScheduleFirstDetail = clsDatabaseControl.GetScheduleFirstDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                                            astrScheduleStaffNurse[iStaff, 0], "01", dtTargetMonth.ToString("yyyyMM"));

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
                                            WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 - 15 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, row["name_short"].ToString());

                                            // 最終計画データと異なる場合
                                            if (row["name_short"].ToString() == row2["name_short"].ToString())
                                            {
                                                // 最終計画データの勤務種類は空欄とする
                                                WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 - 15 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, "");
                                            }
                                            else
                                            {
                                                // 最終計画データの勤務種類をセット
                                                WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 - 15 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, row2["name_short"].ToString());
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
                            WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 - 15 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, "");
                            // 最終計画データ
                            WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 - 15 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, "");
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

                    // 1日から順に処理
                    for (int iDay = 0; iDay <= 15; iDay++)
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
                                            WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, row["name_short"].ToString());

                                            // 最終計画データと異なる場合
                                            if (row["name_short"].ToString() == row2["name_short"].ToString())
                                            {
                                                // 最終計画データの勤務種類は空欄とする
                                                WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, "");
                                            }
                                            else
                                            {
                                                // 最終計画データの勤務種類をセット
                                                WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, row2["name_short"].ToString());
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
                            WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, "");
                            // 最終計画データ
                            WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, "");
                        }
                    }
                }

                // 2ページ目
                // 職種、順番、職員氏名
                for (int iStaff = ROW_NURSE_TOTAL_ROW; iStaff < astrScheduleStaffNurse.GetLength(0); iStaff++)
                {
                    // 種別
                    WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START2, ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 2, astrScheduleStaffNurse[iStaff, 2]);
                    WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START2, ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 1, "");

                    // 順番
                    WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START2 + 2, ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 2, iStaff + 1);
                    WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START2 + 2, ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 1, "");

                    // 氏名
                    WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START2 + 3, ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 2, astrScheduleStaffNurse[iStaff, 1]);
                    WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START2 + 3, ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 1, "");
                }

                // 初回予定データ、最終実績データ(当月)
                for (int iStaff = ROW_NURSE_TOTAL_ROW; iStaff < astrScheduleStaffNurse.GetLength(0); iStaff++)
                {
                    // 対象職員の計画データ取得
                    dtScheduleDetail = clsDatabaseControl.GetScheduleDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                        astrScheduleStaffNurse[iStaff, 0], "01", dtTargetMonth.ToString("yyyyMM"));
                    dtScheduleFirstDetail = clsDatabaseControl.GetScheduleFirstDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                                            astrScheduleStaffNurse[iStaff, 0], "01", dtTargetMonth.ToString("yyyyMM"));

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
                                            WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START2 - 15 + iDay, ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 2, row["name_short"].ToString());

                                            // 最終計画データと異なる場合
                                            if (row["name_short"].ToString() == row2["name_short"].ToString())
                                            {
                                                // 最終計画データの勤務種類は空欄とする
                                                WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START2 - 15 + iDay, ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 1, "");
                                            }
                                            else
                                            {
                                                // 最終計画データの勤務種類をセット
                                                WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START2 - 15 + iDay, ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 1, row2["name_short"].ToString());
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
                            WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START2 - 15 + iDay, ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 2, "");
                            // 最終計画データ
                            WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START2 - 15 + iDay, ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 1, "");
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
                                            WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START2 + iDay, ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 2, row["name_short"].ToString());

                                            // 最終計画データと異なる場合
                                            if (row["name_short"].ToString() == row2["name_short"].ToString())
                                            {
                                                // 最終計画データの勤務種類は空欄とする
                                                WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START2 + iDay, ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 1, "");
                                            }
                                            else
                                            {
                                                // 最終計画データの勤務種類をセット
                                                WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START2 + iDay, ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 1, row2["name_short"].ToString());
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
                            WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START2 + iDay, ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 2, "");
                            // 最終計画データ
                            WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START2 + iDay, ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 1, "");
                        }
                    }
                }

                xlWorkbook.SetPrintArea(0, "A1:AJ141");

            }
            else
            {
                // 1ページ目のみ

                // 職種、順番、職員氏名
                for (int iStaff = 0; iStaff < astrScheduleStaffNurse.GetLength(0); iStaff++)
                {
                    // 種別
                    WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START1, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, astrScheduleStaffNurse[iStaff, 2]);
                    WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START1, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, "");

                    // 順番
                    WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START1 + 2, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, iStaff + 1);
                    WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START1 + 2, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, "");

                    // 氏名
                    WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START1 + 3, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, astrScheduleStaffNurse[iStaff, 1]);
                    WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START1 + 3, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, "");
                }

                // 初回予定データ、最終実績データ(当月)
                for (int iStaff = 0; iStaff < astrScheduleStaffNurse.GetLength(0); iStaff++)
                {
                    // 対象職員の計画データ取得
                    dtScheduleDetail = clsDatabaseControl.GetScheduleDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                        astrScheduleStaffNurse[iStaff, 0], "01", dtTargetMonth.ToString("yyyyMM"));
                    dtScheduleFirstDetail = clsDatabaseControl.GetScheduleFirstDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                                            astrScheduleStaffNurse[iStaff, 0], "01", dtTargetMonth.ToString("yyyyMM"));

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
                                            WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 - 15 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, row["name_short"].ToString());

                                            // 最終計画データと異なる場合
                                            if (row["name_short"].ToString() == row2["name_short"].ToString())
                                            {
                                                // 最終計画データの勤務種類は空欄とする
                                                WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 - 15 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, "");
                                            }
                                            else
                                            {
                                                // 最終計画データの勤務種類をセット
                                                WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 - 15 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, row2["name_short"].ToString());
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
                            WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 - 15 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, "");
                            // 最終計画データ
                            WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 - 15 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, "");
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
                                            WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 + 15 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, row["name_short"].ToString());

                                            // 最終計画データと異なる場合
                                            if (row["name_short"].ToString() == row2["name_short"].ToString())
                                            {
                                                // 最終計画データの勤務種類は空欄とする
                                                WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 + 15 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, "");
                                            }
                                            else
                                            {
                                                // 最終計画データの勤務種類をセット
                                                WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 + 15 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, row2["name_short"].ToString());
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
                            WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 + 15 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, "");
                            // 最終計画データ
                            WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START1 + 15 + iDay, ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, "");
                        }
                    }
                }

                // 改ページをセット
                xlSheet.SetRowBreak(141);
            }

            // == ケア ==

            // 職種、順番、職員氏名
            for (int iStaff = 0; iStaff < astrScheduleStaffCare.GetLength(0); iStaff++)
            {
                // 順番
                WriteCellValue(xlSheet, COLUMN_CARE_STAFF_START, ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 2, iStaff + 1);
                WriteCellValue(xlSheet, COLUMN_CARE_STAFF_START, ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 1, "");

                // 氏名
                WriteCellValue(xlSheet, COLUMN_CARE_STAFF_START + 1, ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 2, astrScheduleStaffCare[iStaff, 1]);
                WriteCellValue(xlSheet, COLUMN_CARE_STAFF_START + 1, ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 1, "");
            }

            // 初回予定データ、最終実績データ(当月)
            for (int iStaff = 0; iStaff < astrScheduleStaffCare.GetLength(0); iStaff++)
            {
                // 対象職員の計画データ取得
                dtScheduleDetail = clsDatabaseControl.GetScheduleDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                    astrScheduleStaffCare[iStaff, 0], "02", dtTargetMonth.ToString("yyyyMM"));
                dtScheduleFirstDetail = clsDatabaseControl.GetScheduleFirstDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                                        astrScheduleStaffCare[iStaff, 0], "02", dtTargetMonth.ToString("yyyyMM"));

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
                                        WriteCellValue(xlSheet, COLUMN_CARE_DAY_START - 15 + iDay, ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 2, row["name_short"].ToString());

                                        // 最終計画データと異なる場合
                                        if (row["name_short"].ToString() == row2["name_short"].ToString())
                                        {
                                            // 最終計画データの勤務種類は空欄とする
                                            WriteCellValue(xlSheet, COLUMN_CARE_DAY_START - 15 + iDay, ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 1, "");
                                        }
                                        else
                                        {
                                            // 最終計画データの勤務種類をセット
                                            WriteCellValue(xlSheet, COLUMN_CARE_DAY_START - 15 + iDay, ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 1, row2["name_short"].ToString());
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
                        WriteCellValue(xlSheet, COLUMN_CARE_DAY_START - 15 + iDay, ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 2, "");
                        // 最終計画データ
                        WriteCellValue(xlSheet, COLUMN_CARE_DAY_START - 15 + iDay, ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 1, "");
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
                                        WriteCellValue(xlSheet, COLUMN_CARE_DAY_START + iDay, ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 2, row["name_short"].ToString());

                                        // 最終計画データと異なる場合
                                        if (row["name_short"].ToString() == row2["name_short"].ToString())
                                        {
                                            // 最終計画データの勤務種類は空欄とする
                                            WriteCellValue(xlSheet, COLUMN_CARE_DAY_START + iDay, ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 1, "");
                                        }
                                        else
                                        {
                                            // 最終計画データの勤務種類をセット
                                            WriteCellValue(xlSheet, COLUMN_CARE_DAY_START + iDay, ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 1, row2["name_short"].ToString());
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
                        WriteCellValue(xlSheet, COLUMN_CARE_DAY_START + iDay, ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 2, "");
                        // 最終計画データ
                        WriteCellValue(xlSheet, COLUMN_CARE_DAY_START + iDay, ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 1, "");
                    }
                }
            }

            // 不要なページを削除
            if (astrScheduleStaffNurse.GetLength(0) <= ROW_NURSE_TOTAL_ROW)
            {
                xlSheet.ShiftRows(94, 136, -47);
                xlSheet.GetRow(47).HeightInPoints = float.Parse("18.75");
                xlSheet.GetRow(48).HeightInPoints = float.Parse("29.25");
                xlSheet.GetRow(49).HeightInPoints = float.Parse("27.75");
                xlSheet.GetRow(50).HeightInPoints = float.Parse("15.75");
                xlSheet.GetRow(51).HeightInPoints = float.Parse("18.75");
                for (int i = 52; i <= 89; i++)
                {
                    xlSheet.GetRow(i).HeightInPoints = float.Parse("21");
                }
            }

            // シート内の各関数の再計算
            XSSFFormulaEvaluator.EvaluateAllFormulaCells(xlWorkbook);

            // ファイル保存
            using (var fs = new FileStream(sfd.FileName, FileMode.Create))
            {
                xlWorkbook.Write(fs);
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
            
            // 様式9チェックデータ

            // 職員リスト一覧(看護師)
            dt = clsDatabaseControl.GetScheduleStaff_Youshiki9_Half(pstrTargetWardCode, pstrTargetYear + pstrTargetMonth, pstrTargetNextYear + pstrTargetNextMonth,"01");

            astrScheduleStaffNurse = new string[dt.Rows.Count, 3];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                astrScheduleStaffNurse[i, 0] = dt.Rows[i]["id"].ToString();
                astrScheduleStaffNurse[i, 1] = dt.Rows[i]["name"].ToString();
                astrScheduleStaffNurse[i, 2] = dt.Rows[i]["staff_kind"].ToString();
            }

            // 職員リスト一覧(ケア)
            dt = clsDatabaseControl.GetScheduleStaff_Youshiki9_Half(pstrTargetWardCode, pstrTargetYear + pstrTargetMonth, pstrTargetNextYear + pstrTargetNextMonth, "02");
            astrScheduleStaffCare = new string[dt.Rows.Count, 3];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                astrScheduleStaffCare[i, 0] = dt.Rows[i]["id"].ToString();
                astrScheduleStaffCare[i, 1] = dt.Rows[i]["name"].ToString();
                astrScheduleStaffCare[i, 2] = dt.Rows[i]["staff_kind"].ToString();
            }
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
        /// セル書き込み(文字列)
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="idxRow"></param>
        /// <param name="idxRow"></param>
        /// <param name="value"></param>
        static void WriteCellValue(ISheet sheet, int idxColumn, int idxRow, string value)
        {
            var row = sheet.GetRow(idxRow) ?? sheet.CreateRow(idxRow); //指定した行を取得できない時はエラーとならないよう新規作成している
            var cell = row.GetCell(idxColumn) ?? row.CreateCell(idxColumn); //一行上の処理の列版

            cell.SetCellValue(value);
        }

        /// <summary>
        /// セル書き込み(文字列)
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="idxRow"></param>
        /// <param name="idxRow"></param>
        /// <param name="value"></param>
        static void WriteCellValue(ISheet sheet, int idxColumn, int idxRow, double value)
        {
            var row = sheet.GetRow(idxRow) ?? sheet.CreateRow(idxRow); //指定した行を取得できない時はエラーとならないよう新規作成している
            var cell = row.GetCell(idxColumn) ?? row.CreateCell(idxColumn); //一行上の処理の列版

            cell.SetCellValue(value);
        }

    }
}
