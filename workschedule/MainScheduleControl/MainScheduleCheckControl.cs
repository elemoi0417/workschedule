﻿using System;
using workschedule.Functions;

namespace workschedule.MainScheduleControl
{
    class MainScheduleCheckControl
    {
        MainSchedule frmMainSchedule;

        // 使用クラス宣言
        CommonControl clsCommonControl = new CommonControl();

        /// <summary>
        /// 初回処理
        /// </summary>
        /// <param name="frmMainSchedule_Parent"></param>
        public MainScheduleCheckControl(MainSchedule frmMainSchedule_Parent)
        {
            frmMainSchedule = frmMainSchedule_Parent;
        }

        /// <summary>
        /// 常日勤のみの職員チェック
        /// </summary>
        /// <returns></returns>
        public bool CheckDayOnlyShift(string[,] astrStaffDayOnly, string strStaff, string strTargetDate)
        {
            for (int i = 0; i < astrStaffDayOnly.GetLength(0); i++)
            {
                if (astrStaffDayOnly[i, 1] != "" && astrStaffDayOnly[i, 2] != "")
                {
                    if (astrStaffDayOnly[i, 0] == strStaff &&
                        DateTime.Parse(strTargetDate) >= DateTime.Parse(astrStaffDayOnly[i, 1]) &&
                        DateTime.Parse(strTargetDate) <= DateTime.Parse(astrStaffDayOnly[i, 2]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 対象日が土日祝、土日祝前の平日かチェック（夜勤用）
        /// </summary>
        /// <returns></returns>
        public bool CheckHolidayAndBeforeAfterDayForNight(string strTargetDate, string[] astrHoliday)
        {
            string strTargetMonth = strTargetDate.Substring(0, 6);      // 対象月
            string strTargetDay;                                        // 対象日用変数
            string strDayOfWeek;                                        // 曜日名称

            // 対象日の曜日を取得
            strDayOfWeek = clsCommonControl.GetWeekName(strTargetDate, astrHoliday);

            // 対象日の曜日が土・日・祝ならfalse
            switch(strDayOfWeek)
            {
                case "土":
                case "日":
                case "祝":
                    return true;
            }

            // 対象日の翌日を変数に代入
            strTargetDay = DateTime.ParseExact(strTargetDate, "yyyyMMdd", null).AddDays(1).ToString("yyyyMMdd");

            // 対象月であるかチェック
            if (strTargetDay.Substring(0, 6) == strTargetMonth)
            {
                // 曜日を取得
                strDayOfWeek = clsCommonControl.GetWeekName(strTargetDay, astrHoliday);

                // 曜日が平日ならfalse
                switch (strDayOfWeek)
                {
                    case "土":
                    case "日":
                    case "祝":
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 対象日が土日祝かチェック（公休用）
        /// </summary>
        /// <returns></returns>
        public bool CheckHolidayFlagForHoliday(string strTargetDate, string[] astrHoliday)
        {
            string strDayOfWeek;        // 曜日名称

            // 対象日の曜日を取得
            strDayOfWeek = clsCommonControl.GetWeekName(strTargetDate, astrHoliday);

            // 対象日の曜日が土・日・祝ならtrue
            switch (strDayOfWeek)
            {
                case "土":
                case "日":
                case "祝":
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 夜勤フラグ
        /// </summary>
        /// <param name="iWorkKind"></param>
        /// <returns></returns>
        public bool CheckWorkKindNight(int iWorkKind)
        {
            if (iWorkKind == 1)
                return true;
            return false;
        }

        /// <summary>
        /// 夜勤明けフラグ
        /// </summary>
        /// <param name="iWorkKind"></param>
        /// <returns></returns>
        public bool CheckWorkKindNightAfter(int iWorkKind)
        {
            if (iWorkKind == 1)
                return true;
            return false;
        }

        /// <summary>
        /// 対象曜日の日勤下限値チェック(夜勤用)
        /// </summary>
        /// <param name="iDay"></param>
        /// <param name="iWorkKind"></param>
        /// <param name="aiColumnTotalData"></param>
        /// <returns></returns>
        public bool CheckCountLimitDayMinDayForNight(int iDay, int iScheduleStaff, int iDayOfWeek)
        {
            DateTime dt;
            int iDayOfWeekNum = iDayOfWeek;
            string strDayOfWeekName;

            // 上限値
            if (frmMainSchedule.astrCountLimitDay[iDayOfWeek, 0] != null)
            {
                // == 対象日 ==
                // 対象日の日勤が最小値を下回るか
                if (int.Parse(frmMainSchedule.astrCountLimitDay[iDayOfWeek, 0]) > frmMainSchedule.adColumnTotalData[iDay, 0] - 1)
                {
                    return false;
                }

                // == 対象日 + 1 ==
                // 対象月かチェック
                dt = DateTime.ParseExact(frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", iDay + 1), "yyyyMMdd", null).AddDays(1);
                if(dt.ToString("yyyyMM") == frmMainSchedule.pstrTargetMonth)
                {
                    // 日勤かチェック
                    if (frmMainSchedule.aiData[iScheduleStaff, iDay + 1, 0] == 1)
                    {
                        // 対象日の曜日名を取得
                        strDayOfWeekName = clsCommonControl.GetWeekName(dt.ToString("yyyyMMdd"), frmMainSchedule.astrHoliday);

                        // 曜日名から、配列番号を取得
                        for (int i = 0; i < frmMainSchedule.astrDayOfWeek.GetLength(0); i++)
                        {
                            if (strDayOfWeekName == frmMainSchedule.astrDayOfWeek[i, 1])
                            {
                                iDayOfWeekNum = i;
                                break;
                            }
                        }

                        // 対象日の日勤が最小値を下回るか
                        if (int.Parse(frmMainSchedule.astrCountLimitDay[iDayOfWeekNum, 0]) > frmMainSchedule.adColumnTotalData[iDay + 1, 0] - 1)
                        {
                            return false;
                        }
                    }
                }

                // == 対象日 + 2 ==
                // 対象月かチェック
                dt = dt.AddDays(1);
                if (dt.ToString("yyyyMM") == frmMainSchedule.pstrTargetMonth)
                {
                    // 日勤かチェック
                    if (frmMainSchedule.aiData[iScheduleStaff, iDay + 2, 0] == 1)
                    {
                        // 対象日の曜日名を取得
                        strDayOfWeekName = clsCommonControl.GetWeekName(dt.ToString("yyyyMMdd"), frmMainSchedule.astrHoliday);

                        // 曜日名から、配列番号を取得
                        for (int i = 0; i < frmMainSchedule.astrDayOfWeek.GetLength(0); i++)
                        {
                            if (strDayOfWeekName == frmMainSchedule.astrDayOfWeek[i, 1])
                            {
                                iDayOfWeekNum = i;
                                break;
                            }
                        }

                        // 対象日の日勤が最小値を下回るか
                        if (int.Parse(frmMainSchedule.astrCountLimitDay[iDayOfWeekNum, 0]) > frmMainSchedule.adColumnTotalData[iDay + 2, 0] - 1)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 対象曜日の日勤下限値チェック(公休用)
        /// </summary>
        /// <param name="iDay"></param>
        /// <param name="iWorkKind"></param>
        /// <param name="aiColumnTotalData"></param>
        /// <returns></returns>
        public bool CheckCountLimitDayMinDayForHoliday(int iDay, int iScheduleStaff, int iDayOfWeek)
        {
            // 上限値
            if (frmMainSchedule.astrCountLimitDay[iDayOfWeek, 0] != null)
            {
                // == 対象日 ==
                // 6病棟で土日祝かつ救急指定日の場合
                if(frmMainSchedule.cmbWard.SelectedValue.ToString() == "06" && 
                (iDayOfWeek == 0 || iDayOfWeek == 6 || iDayOfWeek == 7) && 
                CheckEmergencyDate(String.Format("{0:D2}", iDay + 1)))
                {
                    // 対象日の日勤が最小値を下回るか(通常より1人多くカウント)
                    if (int.Parse(frmMainSchedule.astrCountLimitDay[iDayOfWeek, 0]) + 1 > frmMainSchedule.adColumnTotalData[iDay, 0] - 1)
                    {
                        return false;
                    }
                }
                else
                {
                    // 対象日の日勤が最小値を下回るか
                    if (int.Parse(frmMainSchedule.astrCountLimitDay[iDayOfWeek, 0]) > frmMainSchedule.adColumnTotalData[iDay, 0] - 1)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 対象曜日の日勤下限値チェック(半日用)
        /// </summary>
        /// <param name="iDay"></param>
        /// <param name="iWorkKind"></param>
        /// <param name="aiRowTotalData"></param>
        /// <returns></returns>
        public bool CheckCountLimitDayMinDayForHalfWork(int iDay, int iDayOfWeek)
        {
            // 上限値
            if (frmMainSchedule.astrCountLimitDay[iDayOfWeek, 0] != null)
            {
                // == 対象日 ==
                // 対象日の日勤が最小値を下回るか
                if (int.Parse(frmMainSchedule.astrCountLimitDay[iDayOfWeek, 0]) > frmMainSchedule.adColumnTotalData[iDay, 0] - 1)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 対象日の半日勤務人数チェック(土曜)
        /// </summary>
        /// <param name="iDay"></param>
        /// <param name="iWorkKind"></param>
        /// <param name="aiRowTotalData"></param>
        /// <returns></returns>
        public bool CheckHalfWorkForSaturday(int iDay, int iLimitCount)
        {
            int iHalfWorkCount = 0;
            
            // 半日勤務種類のカウント
            for(int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.piScheduleStaffCount; iScheduleStaff++)
            {
                if (frmMainSchedule.aiData[iScheduleStaff, iDay, 4] == 1)
                    iHalfWorkCount++;
                if (frmMainSchedule.aiData[iScheduleStaff, iDay, 5] == 1)
                    iHalfWorkCount++;
                if (frmMainSchedule.aiData[iScheduleStaff, iDay, 7] == 1)
                    iHalfWorkCount++;
                if (frmMainSchedule.aiData[iScheduleStaff, iDay, 8] == 1)
                    iHalfWorkCount++;
            }

            // 人数判定
            if (iLimitCount > iHalfWorkCount)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 対象曜日の夜勤上限値チェック
        /// </summary>
        /// <param name="iDay"></param>
        /// <param name="iWorkKind"></param>
        /// <param name="aiColumnTotalData"></param>
        /// <returns></returns>
        public bool CheckCountLimitDayMaxNight(int iDay, int iScheduleStaff, int iDayOfWeek)
        {
            DateTime dt;
            int iDayOfWeekNum = iDayOfWeek;
            string strDayOfWeekName;

            // 上限値
            if (frmMainSchedule.astrCountLimitDay[iDayOfWeek, 1] != null)
            {
                // == 対象日 ==
                // 対象日の夜勤が最大値を上回るか
                if (int.Parse(frmMainSchedule.astrCountLimitDay[iDayOfWeek, 1]) < frmMainSchedule.adColumnTotalData[iDay, 1] + 1)
                {
                    return false;
                }

                // == 対象日 + 1 ==
                // 対象月かチェック
                dt = DateTime.ParseExact(frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", iDay + 1), "yyyyMMdd", null).AddDays(1);
                if (dt.ToString("yyyyMM") == frmMainSchedule.pstrTargetMonth)
                {
                    // 日勤かチェック
                    if (frmMainSchedule.aiData[iScheduleStaff, iDay + 1, 0] == 1)
                    {
                        // 対象日の曜日名を取得
                        strDayOfWeekName = clsCommonControl.GetWeekName(dt.ToString("yyyyMMdd"), frmMainSchedule.astrHoliday);

                        // 曜日名から、配列番号を取得
                        for (int i = 0; i < frmMainSchedule.astrDayOfWeek.GetLength(0); i++)
                        {
                            if (strDayOfWeekName == frmMainSchedule.astrDayOfWeek[i, 1])
                            {
                                iDayOfWeekNum = i;
                                break;
                            }
                        }

                        // 対象日の夜勤が最大値を上回るのか
                        if (int.Parse(frmMainSchedule.astrCountLimitDay[iDayOfWeekNum, 1]) < frmMainSchedule.adColumnTotalData[iDay + 1, 2] + 1)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 対象曜日の下限値チェック
        /// </summary>
        /// <param name="iDay"></param>
        /// <param name="iWorkKind"></param>
        /// <param name="aiColumnTotalData"></param>
        /// <returns></returns>
        public bool CheckCountLimitDayMin(int iStaff, int iDay, int iWorkKind, int[,,] aiData, int[,] aiColumnTotalData, string[,,] astrCountLimitDay)
        {
            return true;
        }

        /// <summary>
        /// 職員ごとの勤務種類別合計日数チェック
        /// </summary>
        /// <param name="iDay"></param>
        /// <param name="iWorkKind"></param>
        /// <param name="aiColumnTotalData"></param>
        /// <returns></returns>
        public bool CheckWorkRowCount(int iStaff, int iDay, int iWorkKind, int[,,] aiData, double[,] adRowTotalData)
        {
            switch (iWorkKind)
            {
                case 1:     // 夜勤
                    if (adRowTotalData[iStaff, 1] >= 6) return false;
                    return true;
                case 2:     // 夜明
                    if (adRowTotalData[iStaff, 2] >= 6) return false;
                    return true;
                case 3:     // 公休
                    if (adRowTotalData[iStaff, 0] >= 8) return false;
                    return true;
                default:
                    return true;
            }
        }

        /// <summary>
        /// 勤務種類の並び順確認
        /// </summary>
        /// <returns></returns>
        public bool CheckWorkKindOrder(int iStaff, int iDay, int[,] aiDataNow)
        {
            // 現在の勤務種類ごとの判定
            switch (aiDataNow[iStaff, iDay])
            {
                case 1:     // 夜勤
                case 2:     // 夜明
                    return false;
                case 3:     // 公休
                    if (iDay >= 1)
                        if (aiDataNow[iStaff, iDay - 1] == 1 || aiDataNow[iStaff, iDay - 1] == 2)
                            return false;
                    break;
            }

            return true;
        }

        /// <summary>
        /// 夜勤をセット可能か判定
        /// </summary>
        /// <returns></returns>
        public bool CheckWorkKindOrderForNight(int iStaff, int iDay, int iMaxDay, int[,] aiDataNow, int[,] aiDataRequestFlag)
        {
            // 翌日も同じ月の場合
            if (iDay < iMaxDay - 1)
            {
                // 翌日が希望シフト、または夜勤の場合は×
                if (aiDataRequestFlag[iStaff, iDay + 1] == 1 || aiDataNow[iStaff, iDay + 1] == 1) return false;

                // 翌日が土日祝の場合も×
                if (CheckHolidayFlagForHoliday(frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", iDay + 1), frmMainSchedule.astrHoliday) == true) return false;

                // 翌々日も同じ月の場合
                if (iDay < iMaxDay - 2)
                {
                    // 翌々日が希望シフト、または夜勤の場合は×
                    if (aiDataRequestFlag[iStaff, iDay + 2] == 1 || aiDataNow[iStaff, iDay + 2] == 1)
                        return false;

                    // 翌々日が土日祝で公休なら〇
                    if (CheckHolidayFlagForHoliday(frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", iDay + 3), frmMainSchedule.astrHoliday) == true)
                    {
                        if (frmMainSchedule.aiData[iStaff, iDay + 2, 3] == 1) 
                            return true;
                        else
                            return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 夜勤をセット可能か判定(土日祝セット用）
        /// </summary>
        /// <returns></returns>
        public bool CheckWorkKindOrderForNightHoliday(int iStaff, int iDay, int iMaxDay, int[,] aiDataNow, int[,] aiDataRequestFlag)
        {
            // 翌日も同じ月の場合
            if (iDay < iMaxDay - 1)
            {
                // 翌日が希望シフト、または夜勤の場合は×
                if (aiDataRequestFlag[iStaff, iDay + 1] == 1 || aiDataNow[iStaff, iDay + 1] == 1) return false;

                // 翌々日も同じ月の場合
                if (iDay < iMaxDay - 2)
                {
                    // 翌々日が希望シフト、または夜勤の場合は×
                    if (aiDataRequestFlag[iStaff, iDay + 2] == 1 || aiDataNow[iStaff, iDay + 2] == 1)
                        return false;

                }
            }

            return true;
        }

        /// <summary>
        /// 現時点で予定されている夜勤数が職員の中で最小か確認
        /// </summary>
        /// <returns></returns>
        public bool CheckMinNightFlag(int iStaff, double[,] adRowTotalData, string[,] astrStaff, string[,] astrStaffDayOnly, string strTargetDate)
        {
            double dMin = 100;
            bool bStaffDayOnlyFlag = false;

            // 常日勤を除いた職員から夜勤の最小値を取得
            for (int i = 0; i < astrStaff.GetLength(0); i++)
            {
                for (int i2 = 0; i2 < astrStaffDayOnly.GetLength(0); i2++)
                {
                    if (astrStaff[i, 0] == astrStaffDayOnly[i2, 0] && astrStaffDayOnly[i2, 1] != "" && astrStaffDayOnly[i2, 2] != "")
                    {
                        if (DateTime.Parse(strTargetDate) >= DateTime.Parse(astrStaffDayOnly[i2, 1]) && DateTime.Parse(strTargetDate) <= DateTime.Parse(astrStaffDayOnly[i2, 2]))
                        {
                            bStaffDayOnlyFlag = true;
                            continue;
                        }
                    }
                }
                if (bStaffDayOnlyFlag == false && adRowTotalData[i, 1] <= dMin)
                    dMin = adRowTotalData[i, 1];

                bStaffDayOnlyFlag = false;
            }

            if (adRowTotalData[iStaff, 1] <= dMin + 1)
                return true;

            return false;
        }

        /// <summary>
        /// 連勤チェック（公休用）
        /// </summary>
        /// <returns></returns>
        public bool CheckContinueWorkForHoliday(int iStaff, int iDay)
        {
            int iWorkCountBefore = 0;
            int iWorkCountAfter = 0;

            // 対象日より前の連勤数チェック
            for (int iDayBefore = iDay - 1; iDayBefore >= 0; iDayBefore--)
            {
                if (CheckWorkKindForContinueWork(iStaff, iDayBefore) == true)
                    iWorkCountBefore++;
                else
                    break;
            }

            // 対象日以降の連勤数チェック
            for (int iDayAfter = iDay + 1; iDayAfter < frmMainSchedule.piDayCount; iDayAfter++)
            {
                if (CheckWorkKindForContinueWork(iStaff, iDayAfter) == true)
                    iWorkCountAfter++;
                else
                    break;
            }

            if (iWorkCountBefore >= 2 && iWorkCountAfter >= 2)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 連勤数チェック用の勤務種類判定
        /// </summary>
        /// <returns></returns>
        public bool CheckWorkKindForContinueWork(int iStaff, int iDay)
        {
            string strTargetWorkKind = "";

            // 対象職員の対象日のデータをチェック
            for(int iWorkKind = 0; iWorkKind < frmMainSchedule.astrWorkKind.GetLength(0); iWorkKind++)
            {
                if(frmMainSchedule.aiData[iStaff, iDay, iWorkKind] == 1)
                {
                    // 対象日の勤務種類をセット
                    strTargetWorkKind = iWorkKind.ToString();
                }

            }

            // 勤務種類から判定
            switch(strTargetWorkKind)
            {
                case "0":   // 日勤
                case "1":   // 夜勤
                case "2":   // 夜明
                case "4":   // 公休(午前)
                case "5":   // 公休(午後)
                case "7":   // 有休(午前)
                case "8":   // 有休(午後)
                // Add Start WataruT 2020.07.30 遅出の表示対応
                case "10":  // 遅出
                // Add End WataruT 2020.07.30 遅出の表示対応
                // Add Start WataruT 2020.07.22 特定の時短勤務用の項目追加
                case "17":  // 5.25
                // Add End   WataruT 2020.07.22 特定の時短勤務用の項目追加
                // Add Start WataruT 2020.07.27 特定の時短勤務用の項目追加
                case "18":  // 2
                case "19":  // 6
                case "20":  // 6.25
                case "21":  // 7
                // Add End   WataruT 2020.07.27 特定の時短勤務用の項目追加
                // Add Start WataruT 2020.08.06 遅刻・早退入力対応
                case "22":  // 遅刻
                case "23":  // 早退
                // Add End   WataruT 2020.08.06 遅刻・早退入力対応
                // Add Start WataruT 2020.09.02 時間数の項目を作成
                case "24":
                case "25":
                case "26":
                case "27":
                case "28":
                case "29":
                case "30":
                case "31":
                case "32":
                case "33":
                case "34":
                case "35":
                case "36":
                case "37":
                case "38":
                case "39":
                case "40":
                case "41":
                case "42":
                case "43":
                case "44":
                case "45":
                case "46":
                case "47":
                case "48":
                case "49":
                case "50":
                case "51":
                case "52":
                case "53":
                case "54":
                // Add End   WataruT 2020.09.02 時間数の項目を作成
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 公休数チェック
        /// </summary>
        public bool CheckWorkHoliday(int iStaff)
        {
            if (double.Parse(frmMainSchedule.adRowTotalData[iStaff, 0].ToString()) + 1 > frmMainSchedule.pdHolidayCount)
                return false;
            
            return true;
        }

        /// <summary>
        /// 公休数チェック(半日用)
        /// </summary>
        public bool CheckWorkHolidayForHalfWork(int iStaff)
        {
            if (double.Parse(frmMainSchedule.adRowTotalData[iStaff, 0].ToString()) + 0.5 > frmMainSchedule.pdHolidayCount)
                return false;

            return true;
        }

        /// <summary>
        /// 公休数チェック(夜勤用)
        /// </summary>
        /// <returns></returns>
        public bool CheckWorkHolidayForNight(int iStaff, int iDay)
        {
            double dAddNumber = 0;

            // 2日後が対象月か判定
            if(DateTime.ParseExact(frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", iDay + 1), "yyyyMMdd", null).AddDays(2).ToString("yyyyMM") == 
                frmMainSchedule.pstrTargetMonth)
            {
                for(int iWorkKind = 0; iWorkKind < frmMainSchedule.astrWorkKind.GetLength(0); iWorkKind++)
                {
                    if(frmMainSchedule.aiData[iStaff, iDay + 2, iWorkKind] == 1)
                    {
                        switch (iWorkKind)
                        {
                            case 3: // 公休(1日)
                            case 6: // 有休(1日)
                            case 9: // 公有　// Mod WataruT 2020.07.29 代有を公有に文言変更
                                dAddNumber -= 1;
                                break;
                        }
                        break;
                    }
                }

                if (double.Parse(frmMainSchedule.adRowTotalData[iStaff, 0].ToString()) + dAddNumber + 1 > frmMainSchedule.pdHolidayCount)
                    return false;

            }
            return true;

        }

        /// <summary>
        /// 男性職員の人数チェック(土日祝の夜勤用)
        /// </summary>
        /// <returns></returns>
        public bool CheckMenCountForHolidayNight(int iDay)
        {
            // 現在の合計データと比較してチェック
            switch(frmMainSchedule.cmbWard.SelectedValue.ToString())
            {
                case "01":
                case "02":
                case "03":
                case "04":
                case "05":
                    if (frmMainSchedule.adColumnTotalData[iDay, 3] >= 1)
                        return false;
                    break;
                case "06":
                    if (frmMainSchedule.adColumnTotalData[iDay, 3] >= 2)
                        return false;
                    break;
            }
            return true;
        }

        /// <summary>
        /// 男性職員の人数チェック(土日祝以外の夜勤用)
        /// </summary>
        /// <returns></returns>
        public bool CheckMenCountForNight(int iDay)
        {
            // 現在の合計データと比較してチェック
            switch (frmMainSchedule.cmbWard.SelectedValue.ToString())
            {
                case "06":
                    if (frmMainSchedule.adColumnTotalData[iDay, 3] >= 2)
                        return false;
                    break;
            }
            return true;
        }

        /// <summary>
        /// 救急指定日か判定
        /// </summary>
        /// <returns></returns>
        public bool CheckEmergencyDate(string strTargetDay)
        {
            for (int i = 0; i < frmMainSchedule.dtEmergencyDate.Rows.Count; i++)
            {
                if ((frmMainSchedule.pstrTargetMonth + strTargetDay) == frmMainSchedule.dtEmergencyDate.Rows[i]["target_date"].ToString())
                    return true;
            }

            return false;
        }

    }
}
