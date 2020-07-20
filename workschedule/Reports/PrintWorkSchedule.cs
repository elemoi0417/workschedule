﻿using OfficeOpenXml;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using workschedule.Controls;

namespace workschedule.Reports
{
    class PrintWorkSchedule
    {
        // 使用クラス宣言
        DatabaseControl clsDatabaseControl = new DatabaseControl();

        // 定数
        const int COLUMN_CREATE_YEAR1 = 1;
        const int COLUMN_CREATE_MONTH1 = 3;
        const int COLUMN_WARD1 = 28;
        const int COLUMN_CREATE_YEAR2 = 1;
        const int COLUMN_CREATE_MONTH2 = 3;
        const int COLUMN_WARD2 = 28;
        const int COLUMN_CREATE_YEAR3 = 1;
        const int COLUMN_CREATE_MONTH3 = 3;
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
        const int ROW_WARD1 = 1;
        const int ROW_CREATE_YEAR2 = 49;
        const int ROW_CREATE_MONTH2 = 49;
        const int ROW_WARD2 = 48;
        const int ROW_CREATE_YEAR3 = 96;
        const int ROW_CREATE_MONTH3 = 96;
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

        const int ROW_NURSE_TOTAL_ROW = 20;

        // 変数
        string pstrTargetWardCode;
        string pstrTargetWard;
        string pstrTargetYear;
        string pstrTargetMonth;

        string strFilePath = Environment.CurrentDirectory + @"\Report\workschedule.xlsx";
        
        string[,] astrScheduleStaffNurse;           // 職員マスタ配列(人数、ID・氏名・職種)
        string[,] astrScheduleStaffCare;            // 職員マスタ配列(人数、ID・氏名・職種)

        /// <summary>
        /// クラス初期化
        /// </summary>
        /// <param name="frmMainSchedule_Parent"></param>
        public PrintWorkSchedule(string strTargetWardCode_Parent, string strTargetWard_Parent, string strTargetYear_Parent, string strTargetMonth_Parent)
        {
            // 引数を共通変数にセット
            pstrTargetWardCode = strTargetWardCode_Parent;
            pstrTargetWard = strTargetWard_Parent;
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
            bool bNoDataFlag;
            DateTime dtTargetMonth = DateTime.ParseExact(pstrTargetYear + pstrTargetMonth + "01", "yyyyMMdd", null);
            DataTable dtScheduleDetail;
            DataTable dtScheduleFirstDetail;
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
                xlSheet.Cells[ROW_CREATE_YEAR3, COLUMN_CREATE_YEAR3].Value = pstrTargetYear;

                xlSheet.Cells[ROW_CREATE_MONTH1, COLUMN_CREATE_MONTH1].Value = pstrTargetMonth;
                xlSheet.Cells[ROW_CREATE_MONTH2, COLUMN_CREATE_MONTH2].Value = pstrTargetMonth;
                xlSheet.Cells[ROW_CREATE_MONTH3, COLUMN_CREATE_MONTH3].Value = pstrTargetMonth;

                xlSheet.Cells[ROW_WARD1, COLUMN_WARD1].Value = pstrTargetWard;
                xlSheet.Cells[ROW_WARD2, COLUMN_WARD2].Value = pstrTargetWard;
                xlSheet.Cells[ROW_WARD3, COLUMN_WARD3].Value = pstrTargetWard;

                // == 日付・曜日 ==
                for (int i = 0; i < DateTime.DaysInMonth(int.Parse(pstrTargetYear), int.Parse(pstrTargetMonth)); i++)
                {
                    // 日にち
                    xlSheet.Cells[ROW_NURSE_DAY_START1, COLUMN_NURSE_DAY_START1 + i].Value = i + 1;
                    xlSheet.Cells[ROW_NURSE_DAY_START2, COLUMN_NURSE_DAY_START2 + i].Value = i + 1;
                    xlSheet.Cells[ROW_CARE_DAY_START, COLUMN_CARE_DAY_START + i].Value = i + 1;

                    // 曜日
                    xlSheet.Cells[ROW_NURSE_DAY_OF_WEEK1, COLUMN_NURSE_DAY_OF_WEEK1 + i].Value = dtTargetMonth.AddDays(double.Parse(i.ToString())).ToString("ddd") + "曜";
                    xlSheet.Cells[ROW_NURSE_DAY_OF_WEEK2, COLUMN_NURSE_DAY_OF_WEEK2 + i].Value = dtTargetMonth.AddDays(double.Parse(i.ToString())).ToString("ddd") + "曜";
                    xlSheet.Cells[ROW_CARE_DAY_OF_WEEK, COLUMN_CARE_DAY_OF_WEEK + i].Value = dtTargetMonth.AddDays(double.Parse(i.ToString())).ToString("ddd") + "曜";
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

                    // 初回予定データ、最終実績データ
                    for (int iStaff = 0; iStaff < ROW_NURSE_TOTAL_ROW; iStaff++)
                    {
                        // 対象職員の計画データ取得
                        dtScheduleDetail = clsDatabaseControl.GetScheduleDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                            astrScheduleStaffNurse[iStaff, 0], "01", dtTargetMonth.ToString("yyyyMM"));
                        dtScheduleFirstDetail = clsDatabaseControl.GetScheduleFirstDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                                                astrScheduleStaffNurse[iStaff, 0], "01", dtTargetMonth.ToString("yyyyMM"));

                        // 1日から順に処理
                        for (int iDay = 0; iDay < DateTime.DaysInMonth(dtTargetMonth.Year, dtTargetMonth.Month); iDay++)
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
                                                xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, COLUMN_NURSE_DAY_START1 + iDay].Value = row["name_short"].ToString();

                                                // 最終計画データと異なる場合
                                                if (row["name_short"].ToString() == row2["name_short"].ToString())
                                                {
                                                    // 最終計画データの勤務種類は空欄とする
                                                    xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, COLUMN_NURSE_DAY_START1 + iDay].Value = "";
                                                }
                                                else
                                                {
                                                    // 最終計画データの勤務種類をセット
                                                    xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, COLUMN_NURSE_DAY_START1 + iDay].Value = row2["name_short"].ToString();
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
                                xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, COLUMN_NURSE_DAY_START1 + iDay].Value = "";
                                // 最終計画データ
                                xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, COLUMN_NURSE_DAY_START1 + iDay].Value = "";
                            }
                        }
                    }

                    // 2ページ目
                    // 職種、順番、職員氏名
                    for (int iStaff = ROW_NURSE_TOTAL_ROW; iStaff < astrScheduleStaffNurse.GetLength(0); iStaff++)
                    {
                        // 種別
                        xlSheet.Cells[ROW_NURSE_STAFF_START2 + (iStaff + 1) * 2 - 2, COLUMN_NURSE_STAFF_START2].Value = astrScheduleStaffNurse[iStaff, 2];
                        xlSheet.Cells[ROW_NURSE_STAFF_START2 + (iStaff + 1) * 2 - 1, COLUMN_NURSE_STAFF_START2].Value = "";

                        // 順番
                        xlSheet.Cells[ROW_NURSE_STAFF_START2 + (iStaff + 1) * 2 - 2, COLUMN_NURSE_STAFF_START2 + 2].Value = iStaff + 1;
                        xlSheet.Cells[ROW_NURSE_STAFF_START2 + (iStaff + 1) * 2 - 1, COLUMN_NURSE_STAFF_START2 + 2].Value = "";

                        // 氏名
                        xlSheet.Cells[ROW_NURSE_STAFF_START2 + (iStaff + 1) * 2 - 2, COLUMN_NURSE_STAFF_START2 + 3].Value = astrScheduleStaffNurse[iStaff, 1];
                        xlSheet.Cells[ROW_NURSE_STAFF_START2 + (iStaff + 1) * 2 - 1, COLUMN_NURSE_STAFF_START2 + 3].Value = "";
                    }

                    // 初回予定データ、最終実績データ
                    for (int iStaff = ROW_NURSE_TOTAL_ROW; iStaff < astrScheduleStaffNurse.GetLength(0); iStaff++)
                    {
                        // 対象職員の計画データ取得
                        dtScheduleDetail = clsDatabaseControl.GetScheduleDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                            astrScheduleStaffNurse[iStaff, 0], "01", dtTargetMonth.ToString("yyyyMM"));
                        dtScheduleFirstDetail = clsDatabaseControl.GetScheduleFirstDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                                                astrScheduleStaffNurse[iStaff, 0], "01", dtTargetMonth.ToString("yyyyMM"));

                        // 1日から順に処理
                        for (int iDay = 0; iDay < DateTime.DaysInMonth(dtTargetMonth.Year, dtTargetMonth.Month); iDay++)
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
                                                xlSheet.Cells[ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 2, COLUMN_NURSE_DAY_START2 + iDay].Value = row["name_short"].ToString();

                                                // 最終計画データと異なる場合
                                                if (row["name_short"].ToString() == row2["name_short"].ToString())
                                                {
                                                    // 最終計画データの勤務種類は空欄とする
                                                    xlSheet.Cells[ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 1, COLUMN_NURSE_DAY_START2 + iDay].Value = "";
                                                }
                                                else
                                                {
                                                    // 最終計画データの勤務種類をセット
                                                    xlSheet.Cells[ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 1, COLUMN_NURSE_DAY_START2 + iDay].Value = row["name_short"].ToString();
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
                                xlSheet.Cells[ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 2, COLUMN_NURSE_DAY_START2 + iDay].Value = "";
                                // 最終計画データ
                                xlSheet.Cells[ROW_NURSE_STAFF_START2 + (iStaff - ROW_NURSE_TOTAL_ROW + 1) * 2 - 1, COLUMN_NURSE_DAY_START2 + iDay].Value = "";
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

                    // 初回予定データ、最終実績データ
                    for (int iStaff = 0; iStaff < astrScheduleStaffNurse.GetLength(0); iStaff++)
                    {
                        // 対象職員の計画データ取得
                        dtScheduleDetail = clsDatabaseControl.GetScheduleDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                            astrScheduleStaffNurse[iStaff, 0], "01", dtTargetMonth.ToString("yyyyMM"));
                        dtScheduleFirstDetail = clsDatabaseControl.GetScheduleFirstDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                                                astrScheduleStaffNurse[iStaff, 0], "01", dtTargetMonth.ToString("yyyyMM"));

                        // 1日から順に処理
                        for (int iDay = 0; iDay < DateTime.DaysInMonth(dtTargetMonth.Year, dtTargetMonth.Month); iDay++)
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
                                                xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, COLUMN_NURSE_DAY_START1 + iDay].Value = row["name_short"].ToString();

                                                // 最終計画データと異なる場合
                                                if (row["name_short"].ToString() == row2["name_short"].ToString())
                                                {
                                                    // 最終計画データの勤務種類は空欄とする
                                                    xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, COLUMN_NURSE_DAY_START1 + iDay].Value = "";
                                                }
                                                else
                                                {
                                                    // 最終計画データの勤務種類をセット
                                                    xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, COLUMN_NURSE_DAY_START1 + iDay].Value = row2["name_short"].ToString();
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
                                xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 2, COLUMN_NURSE_DAY_START1 + iDay].Value = "";
                                // 最終計画データ
                                xlSheet.Cells[ROW_NURSE_STAFF_START1 + (iStaff + 1) * 2 - 1, COLUMN_NURSE_DAY_START1 + iDay].Value = "";
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

                // 初回予定データ、最終実績データ
                for (int iStaff = 0; iStaff < astrScheduleStaffCare.GetLength(0); iStaff++)
                {
                    // 対象職員の計画データ取得
                    dtScheduleDetail = clsDatabaseControl.GetScheduleDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                        astrScheduleStaffCare[iStaff, 0], "02", dtTargetMonth.ToString("yyyyMM"));
                    dtScheduleFirstDetail = clsDatabaseControl.GetScheduleFirstDetail_Ward_Staff_StaffKind_TargetMonth(pstrTargetWardCode,
                                            astrScheduleStaffCare[iStaff, 0], "02", dtTargetMonth.ToString("yyyyMM"));

                    // 1日から順に処理
                    for (int iDay = 0; iDay < DateTime.DaysInMonth(dtTargetMonth.Year, dtTargetMonth.Month); iDay++)
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
                                            xlSheet.Cells[ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 2, COLUMN_CARE_DAY_START + iDay].Value = row["name_short"].ToString();

                                            // 最終計画データと異なる場合
                                            if (row["name_short"].ToString() == row2["name_short"].ToString())
                                            {
                                                // 最終計画データの勤務種類は空欄とする
                                                xlSheet.Cells[ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 1, COLUMN_CARE_DAY_START + iDay].Value = "";
                                            }
                                            else
                                            {
                                                // 最終計画データの勤務種類をセット
                                                xlSheet.Cells[ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 1, COLUMN_CARE_DAY_START + iDay].Value = row2["name_short"].ToString();
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
                            xlSheet.Cells[ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 2, COLUMN_CARE_DAY_START + iDay].Value = "";
                            // 最終計画データ
                            xlSheet.Cells[ROW_CARE_STAFF_START + (iStaff + 1) * 2 - 1, COLUMN_CARE_DAY_START + iDay].Value = "";
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
            
            // 様式9チェックデータ


            // 職員リスト一覧(看護師)
            dt = clsDatabaseControl.GetScheduleStaff_Youshiki9(pstrTargetWardCode, pstrTargetYear + pstrTargetMonth, "01");
            astrScheduleStaffNurse = new string[dt.Rows.Count, 3];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                astrScheduleStaffNurse[i, 0] = dt.Rows[i]["id"].ToString();
                astrScheduleStaffNurse[i, 1] = dt.Rows[i]["name"].ToString();
                astrScheduleStaffNurse[i, 2] = dt.Rows[i]["staff_kind"].ToString();
            }

            // 職員リスト一覧(ケア)
            dt = clsDatabaseControl.GetScheduleStaff_Youshiki9(pstrTargetWardCode, pstrTargetYear + pstrTargetMonth, "02");
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

    }
}
