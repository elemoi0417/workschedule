using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using workschedule.Controls;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;

namespace workschedule.Reports
{
    class Ward4_Youshiki9
    {
        // 使用クラス宣言
        DatabaseControl clsDatabaseControl = new DatabaseControl();

        // 定数
        const int COLUMN_CREATE_YEAR = 17;
        const int COLUMN_CREATE_MONTH = 19;
        const int COLUMN_CREATE_DAY = 21;
        const int COLUMN_KUBUN = 7;
        const int COLUMN_NURSE_COUNT = 5;
        const int COLUMN_CARE_COUNT = 12;
        const int COLUMN_WARD_COUNT = 5;
        const int COLUMN_BED_COUNT = 5;
        const int COLUMN_AVERAGE_DAY = 9;
        const int COLUMN_AVERAGE_DAY_START_YEAR = 15;
        const int COLUMN_AVERAGE_DAY_START_MONTH = 17;
        const int COLUMN_AVERAGE_DAY_END_YEAR = 19;
        const int COLUMN_AVERAGE_DAY_END_MONTH = 21;
        const int COLUMN_AVERAGE_YEARS = 9;
        const int COLUMN_AVERAGE_YEARS_START_YEAR = 15;
        const int COLUMN_AVERAGE_YEARS_START_MONTH = 17;
        const int COLUMN_AVERAGE_YEARS_END_YEAR = 19;
        const int COLUMN_AVERAGE_YEARS_END_MONTH = 21;
        const int COLUMN_TARGET_YEAR = 2;
        const int COLUMN_TARGET_MONTH = 2;
        const int COLUMN_WORK_DAYS = 13;

        const int ROW_CREATE_YEAR = 0;
        const int ROW_CREATE_MONTH = 0;
        const int ROW_CREATE_DAY = 0;
        const int ROW_KUBUN = 4;
        const int ROW_NURSE_COUNT = 6;
        const int ROW_CARE_COUNT = 6;
        const int ROW_WARD_COUNT = 8;
        const int ROW_BED_COUNT = 10;
        const int ROW_AVERAGE_DAY = 12;
        const int ROW_AVERAGE_DAY_START_YEAR = 12;
        const int ROW_AVERAGE_DAY_START_MONTH = 12;
        const int ROW_AVERAGE_DAY_END_YEAR = 12;
        const int ROW_AVERAGE_DAY_END_MONTH = 12;
        const int ROW_AVERAGE_YEARS = 23;
        const int ROW_AVERAGE_YEARS_START_YEAR = 23;
        const int ROW_AVERAGE_YEARS_START_MONTH = 23;
        const int ROW_AVERAGE_YEARS_END_YEAR = 23;
        const int ROW_AVERAGE_YEARS_END_MONTH = 23;
        const int ROW_TARGET_YEAR = 38;
        const int ROW_TARGET_MONTH = 39;
        const int ROW_WORK_DAYS = 38;

        const int COLUMN_NURSE_STAFF_START = 1;
        const int COLUMN_NURSE_DAY_START = 15;
        const int ROW_NURSE_DAY = 43;
        const int ROW_NURSE_DAY_OF_WEEK = 44;
        const int ROW_NURSE_STAFF_START = 45;
        const int ROW_NURSE_STAFF_END = 344;

        const int COLUMN_CARE_STAFF_START = 2;
        const int COLUMN_CARE_DAY_START = 15;
        const int ROW_CARE_DAY = 352;
        const int ROW_CARE_DAY_OF_WEEK = 353;
        const int ROW_CARE_STAFF_START = 354;
        const int ROW_CARE_STAFF_END = 653;

        // 変数
        string strFilePath = Environment.CurrentDirectory + @"\Report\ward4_youshiki9.xlsx";

        DataRow drWardYoushiki9;                    // 様式9チェック値のデータ行

        string[,] astrScheduleStaffNurse;           // 職員マスタ配列(人数、ID・氏名・職種)
        string[,] astrScheduleStaffCare;            // 職員マスタ配列(人数、ID・氏名・職種)

        string pstrTargetMonth;                     // 対象月(YYYYMM)
        string pstrTargetMonthName;                 // 対象月(YYYY年MM月)
        string pstrWard;                            // 対象病棟コード
        string pstrWardName;                        // 対象病棟名

        /// <summary>
        /// クラス初期化
        /// </summary>
        public Ward4_Youshiki9(string strTargetMonth, string strWard, string strWardName)
        {
            // 共通変数をセット
            pstrTargetMonth = strTargetMonth;
            pstrTargetMonthName = strTargetMonth.Substring(0, 4) + "年" + strTargetMonth.Substring(4, 2) + "月";
            pstrWard = strWard;
            pstrWardName = strWardName;
        }

        /// <summary>
        /// 帳票ファイル作成処理
        /// </summary>
        /// <param name="targetReport"></param>
        /// <param name="staffNo"></param>
        /// <param name="orderNo"></param>
        public void SaveFile()
        {
            bool bNoDataFlag;
            double dOutputTemp;
            ICellStyle cellStyleTemp;
            DateTime dtTargetDay;

            DataTable dtResultDetail;
            DateTime dtTargetMonth = DateTime.ParseExact(pstrTargetMonth + "01", "yyyyMMdd", null);
            SaveFileDialog sfd = new SaveFileDialog();
            JapaneseCalendar clsJapaneseCalendar = new JapaneseCalendar();

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

            // 作成年月日
            WriteCellValue(xlSheet, COLUMN_CREATE_YEAR, ROW_CREATE_YEAR, double.Parse(clsJapaneseCalendar.GetYear(System.DateTime.Now).ToString()));
            WriteCellValue(xlSheet, COLUMN_CREATE_MONTH, ROW_CREATE_MONTH, double.Parse(DateTime.Now.ToString("MM")));
            WriteCellValue(xlSheet, COLUMN_CREATE_DAY, ROW_CREATE_DAY, double.Parse(DateTime.Now.ToString("dd")));

            // 区分
            WriteCellValue(xlSheet, COLUMN_KUBUN, ROW_KUBUN, drWardYoushiki9["kubun"].ToString());

            // 看護配置・看護補助配置人数
            WriteCellValue(xlSheet, COLUMN_NURSE_COUNT, ROW_NURSE_COUNT, double.Parse(drWardYoushiki9["nurse_count"].ToString()));
            WriteCellValue(xlSheet, COLUMN_CARE_COUNT, ROW_CARE_COUNT, double.Parse(drWardYoushiki9["care_count"].ToString()));

            // 届出の病棟数
            WriteCellValue(xlSheet, COLUMN_WARD_COUNT, ROW_WARD_COUNT, double.Parse(drWardYoushiki9["ward_count"].ToString()));

            // 届出の病床数
            WriteCellValue(xlSheet, COLUMN_BED_COUNT, ROW_BED_COUNT, double.Parse(drWardYoushiki9["bed_count"].ToString()));

            // 1日平均入院患者数
            WriteCellValue(xlSheet, COLUMN_AVERAGE_DAY, ROW_AVERAGE_DAY, double.Parse(drWardYoushiki9["average_day"].ToString()));

            // 1日平均入院患者数(算出期間)
            WriteCellValue(xlSheet, COLUMN_AVERAGE_DAY_START_YEAR, ROW_AVERAGE_DAY_START_YEAR, double.Parse(clsJapaneseCalendar.GetYear(dtTargetMonth.AddMonths(-12)).ToString()));
            WriteCellValue(xlSheet, COLUMN_AVERAGE_DAY_START_MONTH, ROW_AVERAGE_DAY_START_MONTH, double.Parse(dtTargetMonth.AddMonths(-12).ToString("MM")));
            WriteCellValue(xlSheet, COLUMN_AVERAGE_DAY_END_YEAR, ROW_AVERAGE_DAY_END_YEAR, double.Parse(clsJapaneseCalendar.GetYear(System.DateTime.Now.AddMonths(-1)).ToString()));
            WriteCellValue(xlSheet, COLUMN_AVERAGE_DAY_END_MONTH, ROW_AVERAGE_DAY_END_MONTH, double.Parse(dtTargetMonth.AddMonths(-1).ToString("MM")));

            // 平均在院日数
            WriteCellValue(xlSheet, COLUMN_AVERAGE_YEARS, ROW_AVERAGE_YEARS, double.Parse(drWardYoushiki9["average_year"].ToString()));

            // 平均在院日数(算出期間)
            WriteCellValue(xlSheet, COLUMN_AVERAGE_YEARS_START_YEAR, ROW_AVERAGE_YEARS_START_YEAR, double.Parse((int.Parse(clsJapaneseCalendar.GetYear(dtTargetMonth.AddMonths(-3)).ToString()) - 1).ToString()));
            WriteCellValue(xlSheet, COLUMN_AVERAGE_YEARS_START_MONTH, ROW_AVERAGE_YEARS_START_MONTH, double.Parse(dtTargetMonth.AddMonths(-3).ToString("MM")));
            WriteCellValue(xlSheet, COLUMN_AVERAGE_YEARS_END_YEAR, ROW_AVERAGE_YEARS_END_YEAR, double.Parse((int.Parse(clsJapaneseCalendar.GetYear(dtTargetMonth.AddMonths(-1)).ToString()) - 1).ToString()));
            WriteCellValue(xlSheet, COLUMN_AVERAGE_YEARS_END_MONTH, ROW_AVERAGE_YEARS_END_MONTH, double.Parse(dtTargetMonth.AddMonths(-1).ToString("MM")));

            // 対象年月
            WriteCellValue(xlSheet, COLUMN_TARGET_YEAR, ROW_TARGET_YEAR, double.Parse(clsJapaneseCalendar.GetYear(dtTargetMonth).ToString()));
            WriteCellValue(xlSheet, COLUMN_TARGET_MONTH, ROW_TARGET_MONTH, double.Parse(dtTargetMonth.ToString("MM")));

            // 稼働日数
            WriteCellValue(xlSheet, COLUMN_WORK_DAYS, ROW_WORK_DAYS, DateTime.DaysInMonth(dtTargetMonth.Year, dtTargetMonth.Month));

            // << 看護職員表 >>

            // == 日付・曜日 ==
            for (int i = 0; i < DateTime.DaysInMonth(dtTargetMonth.Year, dtTargetMonth.Month); i++)
            {
                // 日にち
                WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START + i, ROW_NURSE_DAY, (i + 1).ToString() + "日");

                // 曜日
                WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START + i, ROW_NURSE_DAY_OF_WEEK, dtTargetMonth.AddDays(double.Parse(i.ToString())).ToString("ddd") + "曜");
            }

            // == 職員氏名欄(種別、順番、病棟、氏名、雇用形態) ==
            for (int iStaff = 0; iStaff < astrScheduleStaffNurse.GetLength(0); iStaff++)
            {
                // 種別
                WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 3, astrScheduleStaffNurse[iStaff, 2]);
                WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 2, "");
                WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 1, "");

                // 順番
                WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START + 1, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 3, double.Parse((iStaff + 1).ToString()));
                WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START + 1, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 2, "");
                WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START + 1, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 1, "");

                // 病棟
                WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START + 2, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 3, pstrWardName);
                WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START + 2, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 2, "");
                WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START + 2, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 1, "");

                // 氏名
                WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START + 3, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 3, astrScheduleStaffNurse[iStaff, 1]);
                WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START + 3, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 2, "");
                WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START + 3, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 1, "");

                // 雇用形態(常勤)
                WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START + 4, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 3, "常勤");
                WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START + 4, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 2, 1);
                WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START + 4, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 1, "");

                // 雇用形態(短時間)
                WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START + 5, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 3, "短時間");
                WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START + 5, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 2, 0);
                WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START + 5, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 1, "");

                // 雇用形態(非常勤)
                WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START + 6, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 3, "非常勤");
                WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START + 6, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 2, 0);
                WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START + 6, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 1, "");

                // 雇用形態(他部署兼務)
                WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START + 7, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 3, "他部署兼務");
                WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START + 7, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 2, 0);
                WriteCellValue(xlSheet, COLUMN_NURSE_STAFF_START + 7, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 1, "");
            }

            // == 実績データ ==
            for (int iStaff = 0; iStaff < astrScheduleStaffNurse.GetLength(0); iStaff++)
            {
                // 対象職員の実績データ取得
                dtResultDetail = clsDatabaseControl.GetResultDetail_Ward_Staff_StaffKind_TargetMonth(pstrWard,
                    astrScheduleStaffNurse[iStaff, 0], "01", pstrTargetMonth);

                // 1日から順に処理
                for (int iDay = 0; iDay < DateTime.DaysInMonth(dtTargetMonth.Year, dtTargetMonth.Month); iDay++)
                {
                    // データなしフラグを初期化
                    bNoDataFlag = false;

                    // 実績データがある場合
                    if (dtResultDetail.Rows.Count != 0)
                    {
                        foreach (DataRow row in dtResultDetail.Rows)
                        {
                            if (DateTime.Parse(row["target_date"].ToString()).Day == iDay + 1)
                            {
                                if (double.TryParse(row["work_time_day"].ToString(), out dOutputTemp))
                                    WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START + iDay, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 3, dOutputTemp);
                                if (double.TryParse(row["work_time_night"].ToString(), out dOutputTemp))
                                    WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START + iDay, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 2, dOutputTemp);
                                if (double.TryParse(row["work_time_night_total"].ToString(), out dOutputTemp))
                                    WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START + iDay, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 1, dOutputTemp);
                                bNoDataFlag = true;
                                break;
                            }
                        }
                    }

                    // 実績データがない場合
                    else if (bNoDataFlag == false)
                    {
                        WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START + iDay, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 3, "");
                        WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START + iDay, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 2, "");
                        WriteCellValue(xlSheet, COLUMN_NURSE_DAY_START + iDay, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 1, "");
                    }
                }
            }

            // == 休日の背景色変更 ==
            for (int i = 0; i < DateTime.DaysInMonth(dtTargetMonth.Year, dtTargetMonth.Month); i++)
            {
                // 対象日付をセット
                dtTargetDay = DateTime.Parse(dtTargetMonth.ToString("yyyy/MM/") + String.Format("{0:D2}", i + 1));

                // 日曜または祝日判定
                if (clsDatabaseControl.GetHoliday_Check(dtTargetDay.ToString("MMdd")) || dtTargetDay.ToString("ddd") == "日")
                {
                    for (int iStaff = 0; iStaff < astrScheduleStaffNurse.GetLength(0); iStaff++)
                    {
                        cellStyleTemp = xlWorkbook.CreateCellStyle();
                        cellStyleTemp.CloneStyleFrom(GetCellStyle(xlSheet, COLUMN_NURSE_DAY_START + i, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 3));
                        cellStyleTemp.FillPattern = FillPattern.LessDots;
                        cellStyleTemp.FillForegroundColor = IndexedColors.Black.Index;
                        cellStyleTemp.FillBackgroundColor = IndexedColors.White.Index;
                        WriteCellStyle(xlSheet, COLUMN_NURSE_DAY_START + i, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 3, cellStyleTemp);
                        cellStyleTemp = xlWorkbook.CreateCellStyle();
                        cellStyleTemp.CloneStyleFrom(GetCellStyle(xlSheet, COLUMN_NURSE_DAY_START + i, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 2));
                        cellStyleTemp.FillPattern = FillPattern.LessDots;
                        cellStyleTemp.FillForegroundColor = IndexedColors.Black.Index;
                        cellStyleTemp.FillBackgroundColor = IndexedColors.White.Index;
                        WriteCellStyle(xlSheet, COLUMN_NURSE_DAY_START + i, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 2, cellStyleTemp);
                        cellStyleTemp = xlWorkbook.CreateCellStyle();
                        cellStyleTemp.CloneStyleFrom(GetCellStyle(xlSheet, COLUMN_NURSE_DAY_START + i, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 1));
                        cellStyleTemp.FillPattern = FillPattern.LessDots;
                        cellStyleTemp.FillForegroundColor = IndexedColors.Black.Index;
                        cellStyleTemp.FillBackgroundColor = IndexedColors.White.Index;
                        WriteCellStyle(xlSheet, COLUMN_NURSE_DAY_START + i, ROW_NURSE_STAFF_START + (iStaff + 1) * 3 - 1, cellStyleTemp);
                    }
                }
            }

            // << 看護補助職員表 >>

            // == 日付・曜日 ==
            for (int i = 0; i < DateTime.DaysInMonth(dtTargetMonth.Year, dtTargetMonth.Month); i++)
            {
                // 日にち
                WriteCellValue(xlSheet, COLUMN_CARE_DAY_START + i, ROW_CARE_DAY, (i + 1).ToString() + "日");

                // 曜日
                WriteCellValue(xlSheet, COLUMN_CARE_DAY_START + i, ROW_CARE_DAY_OF_WEEK, dtTargetMonth.AddDays(double.Parse(i.ToString())).ToString("ddd") + "曜");
            }

            // == 職員氏名欄(種別、順番、病棟、氏名、雇用形態) ==
            for (int iStaff = 0; iStaff < astrScheduleStaffCare.GetLength(0); iStaff++)
            {
                // 順番
                WriteCellValue(xlSheet, COLUMN_CARE_STAFF_START, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 3, double.Parse((iStaff + 1).ToString()));
                WriteCellValue(xlSheet, COLUMN_CARE_STAFF_START, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 2, "");
                WriteCellValue(xlSheet, COLUMN_CARE_STAFF_START, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 1, "");

                // 病棟
                WriteCellValue(xlSheet, COLUMN_CARE_STAFF_START + 1, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 3, pstrWardName);
                WriteCellValue(xlSheet, COLUMN_CARE_STAFF_START + 1, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 2, "");
                WriteCellValue(xlSheet, COLUMN_CARE_STAFF_START + 1, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 1, "");

                // 氏名
                WriteCellValue(xlSheet, COLUMN_CARE_STAFF_START + 2, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 3, astrScheduleStaffCare[iStaff, 1]);
                WriteCellValue(xlSheet, COLUMN_CARE_STAFF_START + 2, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 2, "");
                WriteCellValue(xlSheet, COLUMN_CARE_STAFF_START + 2, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 1, "");

                // 雇用形態(常勤)
                WriteCellValue(xlSheet, COLUMN_CARE_STAFF_START + 3, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 3, "常勤");
                WriteCellValue(xlSheet, COLUMN_CARE_STAFF_START + 3, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 2, 1);
                WriteCellValue(xlSheet, COLUMN_CARE_STAFF_START + 3, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 1, "");

                // 雇用形態(短時間)
                WriteCellValue(xlSheet, COLUMN_CARE_STAFF_START + 4, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 3, "短時間");
                WriteCellValue(xlSheet, COLUMN_CARE_STAFF_START + 4, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 2, 0);
                WriteCellValue(xlSheet, COLUMN_CARE_STAFF_START + 4, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 1, "");

                // 雇用形態(非常勤)
                WriteCellValue(xlSheet, COLUMN_CARE_STAFF_START + 5, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 3, "非常勤");
                WriteCellValue(xlSheet, COLUMN_CARE_STAFF_START + 5, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 2, 0);
                WriteCellValue(xlSheet, COLUMN_CARE_STAFF_START + 5, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 1, "");

                // 雇用形態(他部署兼務)
                WriteCellValue(xlSheet, COLUMN_CARE_STAFF_START + 6, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 3, "他部署兼務");
                WriteCellValue(xlSheet, COLUMN_CARE_STAFF_START + 6, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 2, 0);
                WriteCellValue(xlSheet, COLUMN_CARE_STAFF_START + 6, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 1, "");
                
                //Add Start WataruT 2020.08.31 様式9に事務的業務の担当者フラグをセット
                // 事務的業務
                WriteCellValue(xlSheet, COLUMN_CARE_STAFF_START + 7, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 3, "事務的業務");
                WriteCellValue(xlSheet, COLUMN_CARE_STAFF_START + 7, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 2, int.Parse(astrScheduleStaffCare[iStaff, 3]));
                WriteCellValue(xlSheet, COLUMN_CARE_STAFF_START + 7, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 1, "");
                //Add End   WataruT 2020.08.31 様式9に事務的業務の担当者フラグをセット
            }

            // == 実績データ ==
            for (int iStaff = 0; iStaff < astrScheduleStaffCare.GetLength(0); iStaff++)
            {
                // 対象職員の実績データ取得
                dtResultDetail = clsDatabaseControl.GetResultDetail_Ward_Staff_StaffKind_TargetMonth(pstrWard,
                    astrScheduleStaffCare[iStaff, 0], "02", pstrTargetMonth);

                // 1日から順に処理
                for (int iDay = 0; iDay < DateTime.DaysInMonth(dtTargetMonth.Year, dtTargetMonth.Month); iDay++)
                {
                    // データなしフラグを初期化
                    bNoDataFlag = false;

                    // 実績データがある場合
                    if (dtResultDetail.Rows.Count != 0)
                    {
                        foreach (DataRow row in dtResultDetail.Rows)
                        {
                            if (DateTime.Parse(row["target_date"].ToString()).Day == iDay + 1)
                            {
                                if (double.TryParse(row["work_time_day"].ToString(), out dOutputTemp))
                                    WriteCellValue(xlSheet, COLUMN_CARE_DAY_START + iDay, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 3, dOutputTemp);
                                if (double.TryParse(row["work_time_night"].ToString(), out dOutputTemp))
                                    WriteCellValue(xlSheet, COLUMN_CARE_DAY_START + iDay, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 2, dOutputTemp);
                                if (double.TryParse(row["work_time_night_total"].ToString(), out dOutputTemp))
                                    WriteCellValue(xlSheet, COLUMN_CARE_DAY_START + iDay, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 1, dOutputTemp);
                                bNoDataFlag = true;
                                break;
                            }
                        }
                    }
                    // 実績データがない場合
                    else if (bNoDataFlag == false)
                    {
                        WriteCellValue(xlSheet, COLUMN_CARE_DAY_START + iDay, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 3, "");
                        WriteCellValue(xlSheet, COLUMN_CARE_DAY_START + iDay, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 2, "");
                        WriteCellValue(xlSheet, COLUMN_CARE_DAY_START + iDay, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 1, "");
                    }
                }
            }

            // == 休日の背景色変更 ==
            for (int i = 0; i < DateTime.DaysInMonth(dtTargetMonth.Year, dtTargetMonth.Month); i++)
            {
                // 対象日付をセット
                dtTargetDay = DateTime.Parse(dtTargetMonth.ToString("yyyy/MM/") + String.Format("{0:D2}", i + 1));

                // 日曜または祝日判定
                if (clsDatabaseControl.GetHoliday_Check(dtTargetDay.ToString("MMdd")) || dtTargetDay.ToString("ddd") == "日")
                {
                    for (int iStaff = 0; iStaff < astrScheduleStaffCare.GetLength(0); iStaff++)
                    {
                        cellStyleTemp = xlWorkbook.CreateCellStyle();
                        cellStyleTemp.CloneStyleFrom(GetCellStyle(xlSheet, COLUMN_CARE_DAY_START + i, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 3));
                        cellStyleTemp.FillPattern = FillPattern.LessDots;
                        cellStyleTemp.FillForegroundColor = IndexedColors.Black.Index;
                        cellStyleTemp.FillBackgroundColor = IndexedColors.White.Index;
                        WriteCellStyle(xlSheet, COLUMN_CARE_DAY_START + i, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 3, cellStyleTemp);
                        cellStyleTemp = xlWorkbook.CreateCellStyle();
                        cellStyleTemp.CloneStyleFrom(GetCellStyle(xlSheet, COLUMN_CARE_DAY_START + i, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 2));
                        cellStyleTemp.FillPattern = FillPattern.LessDots;
                        cellStyleTemp.FillForegroundColor = IndexedColors.Black.Index;
                        cellStyleTemp.FillBackgroundColor = IndexedColors.White.Index;
                        WriteCellStyle(xlSheet, COLUMN_CARE_DAY_START + i, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 2, cellStyleTemp);
                        cellStyleTemp = xlWorkbook.CreateCellStyle();
                        cellStyleTemp.CloneStyleFrom(GetCellStyle(xlSheet, COLUMN_CARE_DAY_START + i, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 1));
                        cellStyleTemp.FillPattern = FillPattern.LessDots;
                        cellStyleTemp.FillForegroundColor = IndexedColors.Black.Index;
                        cellStyleTemp.FillBackgroundColor = IndexedColors.White.Index;
                        WriteCellStyle(xlSheet, COLUMN_CARE_DAY_START + i, ROW_CARE_STAFF_START + (iStaff + 1) * 3 - 1, cellStyleTemp);
                    }
                }
            }

            // 不要な行を非表示とする
            for (int i = ROW_NURSE_STAFF_START + (3 * astrScheduleStaffNurse.GetLength(0)); i <= ROW_NURSE_STAFF_END; i++)
            {
                xlSheet.GetRow(i).Height = 1;
            }
            for (int i = ROW_CARE_STAFF_START + (3 * astrScheduleStaffCare.GetLength(0)); i <= ROW_CARE_STAFF_END; i++)
            {
                xlSheet.GetRow(i).Height = 1;
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
            dt = clsDatabaseControl.GetWardYoushiki9_TargetMonth_Ward(pstrTargetMonth, pstrWard);
            drWardYoushiki9 = dt.Rows[0];

            // 職員リスト一覧(看護師)
            dt = clsDatabaseControl.GetScheduleStaff_Youshiki9(pstrWard, pstrTargetMonth, "01");
            astrScheduleStaffNurse = new string[dt.Rows.Count, 3];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                astrScheduleStaffNurse[i, 0] = dt.Rows[i]["id"].ToString();
                astrScheduleStaffNurse[i, 1] = dt.Rows[i]["name"].ToString();
                astrScheduleStaffNurse[i, 2] = dt.Rows[i]["staff_kind"].ToString();
            }

            // 職員リスト一覧(ケア)
            dt = clsDatabaseControl.GetScheduleStaff_Youshiki9(pstrWard, pstrTargetMonth, "02");
            //Mod Start WataruT 2020.08.31 様式9に事務的業務の担当者フラグをセット
            //astrScheduleStaffCare = new string[dt.Rows.Count, 3];
            astrScheduleStaffCare = new string[dt.Rows.Count, 4];
            //Mod End   WataruT 2020.08.31 様式9に事務的業務の担当者フラグをセット
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                astrScheduleStaffCare[i, 0] = dt.Rows[i]["id"].ToString();
                astrScheduleStaffCare[i, 1] = dt.Rows[i]["name"].ToString();
                astrScheduleStaffCare[i, 2] = dt.Rows[i]["staff_kind"].ToString();
                //Add Start WataruT 2020.08.31 様式9に事務的業務の担当者フラグをセット
                astrScheduleStaffCare[i, 3] = dt.Rows[i]["office_flag"].ToString();
                //Add End   WataruT 2020.08.31 様式9に事務的業務の担当者フラグをセット
            }
        }

        /// <summary>
        /// 保存ダイアログのプロパティ設定
        /// </summary>
        private void SetSaveFileDialogProperties(ref SaveFileDialog sfd)
        {
            // ファイル名の既定値(YYYY年MM月_〇病棟_様式9.xlsx)
            sfd.FileName = pstrTargetMonthName + "_" + pstrWardName + "_" + "様式9.xlsx";
            // 既定フォルダ
            sfd.InitialDirectory = @"C:\";
            // ファイル種類フィルタ
            sfd.Filter = "Excelファイル(*.xlsx)|*.xlsx";
            //タイトルを設定する
            sfd.Title = "保存先を選択してください。";
        }

        /// <summary>
        /// COMオブジェクトへの参照を作成および取得
        /// </summary>
        /// <param name="progId"></param>
        /// <param name="serverName"></param>
        /// <returns></returns>
        public static object CreateObject(string progId, string serverName)
        {
            Type t;
            if (serverName == null || serverName.Length == 0)
                t = Type.GetTypeFromProgID(progId);
            else
                t = Type.GetTypeFromProgID(progId, serverName, true);
            return Activator.CreateInstance(t);
        }

        /// <summary>
        /// COMオブジェクトへの参照を作成および取得(プログラムIDのみ)
        /// </summary>
        /// <param name="progId"></param>
        /// <param name="serverName"></param>
        /// <returns></returns>
        public static object CreateObject(string progId)
        {
            return CreateObject(progId, null);
        }

        /// <summary>
        /// COMオブジェクトの開放
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objCom"></param>
        public static void ReleaseCOMObject<T>(ref T objCom) where T : class
        {
            // オブジェクトが空なら処理を抜ける
            if (objCom == null)
                return;

            try
            {
                // COMオブジェクトチェック
                if (System.Runtime.InteropServices.Marshal.IsComObject(objCom))
                {
                    int cntRCW = System.Runtime.InteropServices.Marshal.FinalReleaseComObject(objCom);
                    if (cntRCW != 0)
                        MessageBox.Show("解放エラー");
                }
            }
            finally
            {
                objCom = null;
            }
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

        /// <summary>
        /// セルスタイルの取得
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="idxRow"></param>
        /// <param name="idxRow"></param>
        /// <returns></returns>
        static ICellStyle GetCellStyle(ISheet sheet, int idxColumn, int idxRow)
        {
            var row = sheet.GetRow(idxRow) ?? sheet.CreateRow(idxRow); //指定した行を取得できない時はエラーとならないよう新規作成している
            var cell = row.GetCell(idxColumn) ?? row.CreateCell(idxColumn); //一行上の処理の列版

            return cell.CellStyle;
        }

        /// <summary>
        /// セルスタイルをセット
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="idxRow"></param>
        /// <param name="idxRow"></param>
        /// <param name="value"></param>
        static void WriteCellStyle(ISheet sheet, int idxColumn, int idxRow, ICellStyle cellStyle)
        {
            var row = sheet.GetRow(idxRow) ?? sheet.CreateRow(idxRow); //指定した行を取得できない時はエラーとならないよう新規作成している
            var cell = row.GetCell(idxColumn) ?? row.CreateCell(idxColumn); //一行上の処理の列版

            cell.CellStyle = cellStyle;
        }
    }
}
