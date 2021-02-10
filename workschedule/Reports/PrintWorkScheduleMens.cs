// Add WataruT 2021.02.10 男性職員の予定表出力機能追加
using OfficeOpenXml;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using workschedule.Controls;
using workschedule.Functions;

namespace workschedule.Reports
{
    class PrintWorkScheduleMens
    {
        // 使用クラス宣言
        DatabaseControl clsDatabaseControl = new DatabaseControl();
        CommonControl clsCommonControl = new CommonControl();

        // 定数
        const int COLUMN_CREATE_YEAR1 = 1;
        const int COLUMN_CREATE_MONTH1 = 3;
        const int COLUMN_CREATE_YEAR2 = 1;
        const int COLUMN_CREATE_MONTH2 = 3;
        
        const int COLUMN_DAY_START1 = 6;
        const int COLUMN_DAY_START2 = 6;
        const int COLUMN_DAY_OF_WEEK1 = 6;
        const int COLUMN_DAY_OF_WEEK2 = 6;
        const int COLUMN_STAFF_START1 = 1;
        const int COLUMN_STAFF_START2 = 1;

        const int ROW_CREATE_YEAR1 = 1;
        const int ROW_CREATE_MONTH1 = 1;
        const int ROW_CREATE_YEAR2 = 47;
        const int ROW_CREATE_MONTH2 = 47;

        const int ROW_DAY_START1 = 3;
        const int ROW_DAY_START2 = 49;
        const int ROW_DAY_OF_WEEK1 = 4;
        const int ROW_DAY_OF_WEEK2 = 50;
        const int ROW_STAFF_START1 = 5;
        const int ROW_STAFF_START2 = 51;

        const int ROW_TOTAL_ROW = 41;
        
        // 変数
        string pstrTargetYear;
        string pstrTargetMonth;

        string strFilePath = Environment.CurrentDirectory + @"\Report\workschedulemens.xlsx";
        
        string[,] astrScheduleStaff;           // 職員マスタ配列(人数、ID・氏名・職種・病棟名・病棟コード)
        string[] astrHoliday;                       // 祝日マスタ配列
        
        /// <summary>
        /// クラス初期化
        /// </summary>
        /// <param name="frmMainSchedule_Parent"></param>
        public PrintWorkScheduleMens( string strTargetYear_Parent, string strTargetMonth_Parent)
        {
            // 引数を共通変数にセット
            pstrTargetYear = strTargetYear_Parent;
            pstrTargetMonth = strTargetMonth_Parent;
        }

        /// <summary>
        /// 帳票ファイル作成処理
        /// </summary>
        /// <param name="targetReport"></param>
        /// <param name="staffNo"></param>
        /// <param name="orderNo"></param>
        public void SaveFile()
        {
            DateTime dtTargetMonth = DateTime.ParseExact(pstrTargetYear + pstrTargetMonth + "01", "yyyyMMdd", null);
            DataTable dtScheduleDetail;
            SaveFileDialog sfd = new SaveFileDialog();

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

                //// === Excelデータ入力 ===

                // 作成年月
                xlSheet.Cells[ROW_CREATE_YEAR1, COLUMN_CREATE_YEAR1].Value = pstrTargetYear;
                xlSheet.Cells[ROW_CREATE_YEAR2, COLUMN_CREATE_YEAR2].Value = pstrTargetYear;

                xlSheet.Cells[ROW_CREATE_MONTH1, COLUMN_CREATE_MONTH1].Value = pstrTargetMonth;
                xlSheet.Cells[ROW_CREATE_MONTH2, COLUMN_CREATE_MONTH2].Value = pstrTargetMonth;

                // == 日付・曜日 ==
                for (int i = 0; i < DateTime.DaysInMonth(int.Parse(pstrTargetYear), int.Parse(pstrTargetMonth)); i++)
                {
                    // 日にち
                    xlSheet.Cells[ROW_DAY_START1, COLUMN_DAY_START1 + i].Value = i + 1;
                    xlSheet.Cells[ROW_DAY_START2, COLUMN_DAY_START2 + i].Value = i + 1;

                    // 曜日
                    xlSheet.Cells[ROW_DAY_OF_WEEK1, COLUMN_DAY_OF_WEEK1 + i].Value = dtTargetMonth.AddDays(double.Parse(i.ToString())).ToString("ddd") + "曜";
                    xlSheet.Cells[ROW_DAY_OF_WEEK2, COLUMN_DAY_OF_WEEK2 + i].Value = dtTargetMonth.AddDays(double.Parse(i.ToString())).ToString("ddd") + "曜";

                    if (clsCommonControl.GetWeekName(dtTargetMonth.AddDays(i).ToString("yyyyMMdd"), astrHoliday) == "祝" || 
                        clsCommonControl.GetWeekName(dtTargetMonth.AddDays(i).ToString("yyyyMMdd"), astrHoliday) == "日")
                    {
                        for (int iStaffCount = 0; iStaffCount <= ROW_TOTAL_ROW + 1; iStaffCount++)
                        {
                            xlSheet.Cells[ROW_DAY_START1 + iStaffCount, COLUMN_DAY_START1 + i].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Gray125;
                            xlSheet.Cells[ROW_DAY_START2 + iStaffCount, COLUMN_DAY_START2 + i].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Gray125;
                        }
                    }
                }
                // == 男性職員 ==

                // 複数ページ対応とするか判定
                if (astrScheduleStaff.GetLength(0) > ROW_TOTAL_ROW)
                {
                    // 1ページ目

                    // 職種、病棟名、職員氏名
                    for (int iStaff = 0; iStaff < ROW_TOTAL_ROW; iStaff++)
                    {
                        // 種別
                        xlSheet.Cells[ROW_STAFF_START1 + iStaff, COLUMN_STAFF_START1].Value = astrScheduleStaff[iStaff, 2];
                        // 病棟名
                        xlSheet.Cells[ROW_STAFF_START1 + iStaff, COLUMN_STAFF_START1 + 2].Value = astrScheduleStaff[iStaff, 3];
                        // 氏名
                        xlSheet.Cells[ROW_STAFF_START1 + iStaff, COLUMN_STAFF_START1 + 3].Value = astrScheduleStaff[iStaff, 1];
                    }

                    // 予定データ
                    for (int iStaff = 0; iStaff < ROW_TOTAL_ROW; iStaff++)
                    {
                        // 対象職員の計画データ取得
                        dtScheduleDetail = clsDatabaseControl.GetScheduleDetail_Ward_Staff_TargetMonth(astrScheduleStaff[iStaff,4],
                            astrScheduleStaff[iStaff, 0], dtTargetMonth.ToString("yyyyMM"));
                        // 1日から順に処理
                        for (int iDay = 0; iDay < DateTime.DaysInMonth(dtTargetMonth.Year, dtTargetMonth.Month); iDay++)
                        {      
                            // 予定データがあるか確認
                            if (dtScheduleDetail.Rows.Count != 0)
                            {
                                // 予定データを順に確認
                                foreach (DataRow row in dtScheduleDetail.Rows)
                                {
                                    // 対象日と一致する
                                    if (DateTime.Parse(row["target_date"].ToString()).Day == iDay + 1)
                                    {
                                        // 予定データ
                                        xlSheet.Cells[ROW_STAFF_START1 + iStaff, COLUMN_DAY_START1 + iDay].Value = row["name_short"].ToString();
                                    }
                                }
                            }
                            else
                            {
                                // 空欄データとする
                                xlSheet.Cells[ROW_STAFF_START1 + iStaff, COLUMN_DAY_START1 + iDay].Value = "";
                            }
                        }
                    }

                    // 2ページ目
                    // 職種、病棟名、職員氏名
                    for (int iStaff = ROW_TOTAL_ROW; iStaff < astrScheduleStaff.GetLength(0); iStaff++)
                    {
                        // 種別
                        xlSheet.Cells[ROW_STAFF_START2 + (iStaff - ROW_TOTAL_ROW), COLUMN_STAFF_START2].Value = astrScheduleStaff[iStaff, 2];
                        // 病棟名
                        xlSheet.Cells[ROW_STAFF_START2 + (iStaff - ROW_TOTAL_ROW), COLUMN_STAFF_START2 + 2].Value = astrScheduleStaff[iStaff, 3];
                        // 氏名
                        xlSheet.Cells[ROW_STAFF_START2 + (iStaff - ROW_TOTAL_ROW), COLUMN_STAFF_START2 + 3].Value = astrScheduleStaff[iStaff, 1];
                    }

                    // 予定データ
                    for (int iStaff = ROW_TOTAL_ROW; iStaff < astrScheduleStaff.GetLength(0); iStaff++)
                    {
                        // 対象職員の計画データ取得
                        dtScheduleDetail = clsDatabaseControl.GetScheduleDetail_Ward_Staff_TargetMonth(astrScheduleStaff[iStaff, 4],
                            astrScheduleStaff[iStaff, 0], dtTargetMonth.ToString("yyyyMM"));
                        // 1日から順に処理
                        for (int iDay = 0; iDay < DateTime.DaysInMonth(dtTargetMonth.Year, dtTargetMonth.Month); iDay++)
                        {
                            // 予定データがあるか確認
                            if (dtScheduleDetail.Rows.Count != 0)
                            {
                                // 予定データを順に確認
                                foreach (DataRow row in dtScheduleDetail.Rows)
                                {
                                    // 対象日と一致する
                                    if (DateTime.Parse(row["target_date"].ToString()).Day == iDay + 1)
                                    {
                                        // 予定データ
                                        xlSheet.Cells[ROW_STAFF_START2 + (iStaff - ROW_TOTAL_ROW), COLUMN_DAY_START2 + iDay].Value = row["name_short"].ToString();
                                    }
                                }
                            }
                            else
                            {
                                // 空欄データとする
                                xlSheet.Cells[ROW_STAFF_START1 + (iStaff - ROW_TOTAL_ROW), COLUMN_DAY_START2 + iDay].Value = "";
                            }
                        }

                    }
                }
                else
                {
                    // 1ページ目のみ

                    // 職種、病棟名、職員氏名
                    for (int iStaff = 0; iStaff < astrScheduleStaff.GetLength(0); iStaff++)
                    {
                        // 種別
                        xlSheet.Cells[ROW_STAFF_START1 + (iStaff - 1), COLUMN_STAFF_START1].Value = astrScheduleStaff[iStaff, 2];
                        // 病棟名
                        xlSheet.Cells[ROW_STAFF_START1 + (iStaff - 1), COLUMN_STAFF_START1 + 2].Value = astrScheduleStaff[iStaff, 3];
                        // 氏名
                        xlSheet.Cells[ROW_STAFF_START1 + (iStaff - 1), COLUMN_STAFF_START1 + 3].Value = astrScheduleStaff[iStaff, 1];
                    }

                    // 予定データ
                    for (int iStaff = 0; iStaff < astrScheduleStaff.GetLength(0); iStaff++)
                    {
                        // 対象職員の計画データ取得
                        dtScheduleDetail = clsDatabaseControl.GetScheduleDetail_Ward_Staff_TargetMonth(astrScheduleStaff[iStaff, 4],
                            astrScheduleStaff[iStaff, 0], dtTargetMonth.ToString("yyyyMM"));
                        // 1日から順に処理
                        for (int iDay = 0; iDay < DateTime.DaysInMonth(dtTargetMonth.Year, dtTargetMonth.Month); iDay++)
                        {
                            // 予定データがあるか確認
                            if (dtScheduleDetail.Rows.Count != 0)
                            {
                                // 予定データを順に確認
                                foreach (DataRow row in dtScheduleDetail.Rows)
                                {
                                    // 対象日と一致する
                                    if (DateTime.Parse(row["target_date"].ToString()).Day == iDay + 1)
                                    {
                                        // 予定データ
                                        xlSheet.Cells[ROW_STAFF_START1 + (iStaff - 1), COLUMN_DAY_START1 + iDay].Value = row["name_short"].ToString();
                                    }
                                }
                            }
                            else
                            {
                                // 空欄データとする
                                xlSheet.Cells[ROW_STAFF_START1 + (iStaff - 1), COLUMN_DAY_START1 + iDay].Value = "";
                            }
                        }
                    }
                    
                }

                // 印刷範囲の指定
                xlSheet.PrinterSettings.PrintArea = xlSheet.Cells[1, 1, 91, 36];

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
            
            // 職員リスト一覧(男性職員)
            dt = clsDatabaseControl.GetScheduleStaff_OnlyMens(pstrTargetYear + pstrTargetMonth);
            astrScheduleStaff = new string[dt.Rows.Count, 5];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                astrScheduleStaff[i, 0] = dt.Rows[i]["id"].ToString();
                astrScheduleStaff[i, 1] = dt.Rows[i]["name"].ToString();
                astrScheduleStaff[i, 2] = dt.Rows[i]["staff_kind"].ToString();
                astrScheduleStaff[i, 3] = dt.Rows[i]["ward_name"].ToString();
                astrScheduleStaff[i, 4] = dt.Rows[i]["ward_code"].ToString();
            }

            /// 祝日マスタ
            dt = clsDatabaseControl.GetHoliday();
            astrHoliday = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                astrHoliday[i] = dt.Rows[i]["holiday"].ToString();
            }
        }

        /// <summary>
        /// 保存ダイアログのプロパティ設定
        /// </summary>
        private void SetSaveFileDialogProperties(ref SaveFileDialog sfd)
        {   
            // ファイル名の既定値
            sfd.FileName = pstrTargetYear + pstrTargetMonth + "_"  + "勤務計画表(男性のみ).xlsx";
            // 既定フォルダ
            sfd.InitialDirectory = @"C:\";
            // ファイル種類フィルタ
            sfd.Filter = "Excelファイル(*.xlsx)|*.xlsx";
            //タイトルを設定する
            sfd.Title = "保存先を選択してください。";
        }
    }
}
