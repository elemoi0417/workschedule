using System;
using System.Drawing;

namespace workschedule.Functions
{
    class CommonControl
    {
        /// <summary>
        /// 指定した桁数で切り上げを行う
        /// </summary>
        /// <param name="dValue"></param>
        /// <param name="iDigits"></param>
        /// <returns></returns>
        public double ToRoundUp(double dValue, int iDigits)
        {
            double dCoef = System.Math.Pow(10, iDigits);

            return dValue > 0 ? System.Math.Ceiling(dValue * dCoef) / dCoef :
                                System.Math.Floor(dValue * dCoef) / dCoef;
        }

        /// <summary>
        /// 指定した桁数で切り下げを行う
        /// </summary>
        /// <param name="dValue"></param>
        /// <param name="iDigits"></param>
        /// <returns></returns>
        public double ToRoundDown(double dValue, int iDigits)
        {
            double dCoef = System.Math.Pow(10, iDigits);

            return dValue > 0 ? System.Math.Floor(dValue * dCoef) / dCoef :
                                System.Math.Ceiling(dValue * dCoef) / dCoef;
        }

        /// <summary>
        /// 対象年月の日数を返す
        /// </summary>
        /// <returns></returns>
        public int GetTargetMonthDays(string strTargetMonth)
        {
            string strFormat = "yyyy年MM月";
            DateTime dtTargetMonth = DateTime.ParseExact(strTargetMonth, strFormat, null);
            
            return DateTime.DaysInMonth(dtTargetMonth.Year, dtTargetMonth.Month);
        }

        /// <summary>
        /// 対象日付から曜日を返す
        /// </summary>
        /// <param name="iVal"></param>
        /// <returns></returns>
        public string GetWeekName(string strTargetDate, string[] astrHoliday)
        
        {
            string strFormat = "yyyyMMdd";
            DateTime dtTargetDate = DateTime.ParseExact(strTargetDate, strFormat, null);
            for (int i = 0; i < astrHoliday.Length; i++)
            {
                if (strTargetDate.Substring(4, 4) == astrHoliday[i])
                    return "祝";
            }

            return dtTargetDate.ToString("ddd");
        }

        /// <summary>
        /// 曜日から曜日マスタのIDを返す
        /// </summary>
        /// <returns></returns>
        public string GetDayOfWeekID(string strDayOfWeek, string[,] astrDayOfWeek)
        {
            for (int i = 0; i < astrDayOfWeek.GetLength(0); i++)
            {
                if (strDayOfWeek == astrDayOfWeek[i, 1])
                {
                    return astrDayOfWeek[i, 0];
                }
            }

            return "";
        }

        /// <summary>
        /// 職種コードから職種の名称を返す
        /// </summary>
        /// <param name="strStaffKind"></param>
        /// <param name="astrStaffKind"></param>
        /// <returns></returns>
        public string GetStaffKindName(string strStaffKind, string[,] astrStaffKind)
        {
            for(int iStaffKind = 0; iStaffKind < astrStaffKind.GetLength(0); iStaffKind++)
            {
                if (strStaffKind == astrStaffKind[iStaffKind, 0])
                    return astrStaffKind[iStaffKind, 2];
            }

            return "";
        }

        /// <summary>
        /// 勤務種類名称から勤務種類IDを返す
        /// </summary>
        /// <param name="strWorkKind"></param>
        /// <param name="astrWorkKind"></param>
        /// <returns></returns>
        public string GetWorkKindID(string strWorkKind, string[,] astrWorkKind)
        {
            for (int i = 0; i < astrWorkKind.GetLength(0); i++)
            {
                if (strWorkKind == astrWorkKind[i, 1])
                {
                    return astrWorkKind[i, 0];
                }
            }

            return "";
        }

        /// <summary>
        /// 曜日に対応した文字の色を返す
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public Color GetWeekNameForeColor(string strValue)
        {
            switch (strValue)
            {
                case "日":
                case "祝":
                    return Color.Red;
                case "土":
                    return Color.Blue;
                default:
                    return Color.Black;
            }
        }

        /// <summary>
        /// 勤務種類に対応した文字の色を返す
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public Color GetWorkKindForeColor(string strValue)
        {
            switch (strValue)
            {
                case "日":
                    return ColorTranslator.FromHtml("#003333");
                case "夜勤":
                    return ColorTranslator.FromHtml("#61C359");
                case "夜明":
                    return ColorTranslator.FromHtml("#61C359");
                case "休":
                    return ColorTranslator.FromHtml("#FA776D");
                default:
                    return Color.Black;
            }
        }

        /// <summary>
        /// 曜日に対応した背景色を返す
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public Color GetWeekNameBackgroundColor(string strValue)
        {
            switch (strValue)
            {
                case "日":
                case "祝":
                    return ColorTranslator.FromHtml("#FFD6CC");
                case "土":
                    return ColorTranslator.FromHtml("#B4DAED");
                default:
                    return Color.White;
            }
        }

        /// <summary>
        /// 性別に対応した色を返す
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public Color GetSexForeColor(string strValue)
        {
            switch (strValue)
            {
                case "1":
                    return Color.Blue;
                case "2":
                    return Color.Red;
                default:
                    return Color.Black;
            }
        }

        /// <summary>
        /// 勤務種類の合計名称を取得
        /// </summary>
        /// <returns></returns>
        public string GetWorkKindTotalName(int iWorkKind)
        {
            switch (iWorkKind)
            {
                case 0:
                    return "日　勤　計";
                case 1:
                    return "夜　勤　計";
                case 2:
                    return "夜　明　計";
                case 3:
                    return "夜 勤 (男 性)";
                case 4:
                    return "夜 勤 (女 性)";
                case 5:
                    return "夜勤 新人以外";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 乱数値を勤務種類の値に変換
        /// </summary>
        /// <param name="iWorkKind"></param>
        /// <returns></returns>
        public int ChangeWorkKindValue(int iWorkKind)
        {
            switch (iWorkKind)
            {
                case 0: // 夜勤
                    return 1;
                case 1: // 夜明
                    return 2;
                case 2: // 公休
                    return 3;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 日付の文字列を"YYYYMMDD"から"YYYY/MM/DD"に変換
        /// </summary>
        /// <returns></returns>
        public string GetTargetDateChangeFormat(string strTargetDate)
        {
            return strTargetDate.ToString().Substring(0, 4) + "/" +
                strTargetDate.ToString().Substring(4, 2) + "/" +
                strTargetDate.ToString().Substring(6, 2);
        }

        /// <summary>
        /// 日付の文字列をMySQLのDATE型の書式に変換
        /// </summary>
        /// <param name="iTargetYear"></param>
        /// <param name="iTargetMonth"></param>
        /// <param name="iTargetDay"></param>
        /// <returns></returns>
        public string GetTargetDateForMySQL(string strTargetYear, string strTargetMonth, string strTargetDay)
        {
            return String.Format("{0:D4}", strTargetYear.ToString()) + "-" +
                String.Format("{0:D2}", strTargetMonth.ToString()) + "-" +
                String.Format("{0:D2}", strTargetDay.ToString());
        }

        /// <summary>
        /// 休日フラグの変換
        /// </summary>
        /// <param name="strTargetDate"></param>
        /// <returns></returns>
        public string ChangeHolidayFlagFormat(string strHolidayFlag)
        {
            switch (strHolidayFlag)
            {
                case "〇":
                    return "1";
                case "×":
                    return "0";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 事務業務フラグの変換
        /// </summary>
        /// <param name="strTargetDate"></param>
        /// <returns></returns>
        public string ChangeOfficeFlagFormat(string strOfficeFlag)
        {
            switch (strOfficeFlag)
            {
                case "〇":
                    return "1";
                case "×":
                    return "0";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 職員レベルの変換
        /// </summary>
        /// <param name="strStaffLevel"></param>
        /// <returns></returns>
        public string ChangeStaffLevelFormat(string strStaffLevel)
        {
            switch (strStaffLevel)
            {
                case "レベル１":
                    return "1";
                case "レベル２":
                    return "2";
                case "レベル３":
                    return "3";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 勤務予定セルの背景色から希望シフトフラグを返す
        /// </summary>
        /// <param name="strRequestFlagBackColor"></param>
        /// <returns></returns>
        public string GetRequestFlag(Color colorRequestFlag)
        {
            if (colorRequestFlag == Color.Gold)
                return "1";

            return "0";
        }

        /// <summary>
        /// EXCELの列の値変換(数字 → カラム文字)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public string ChangeExcelToColumnName(int iColumn)
        {
            // 無効な値の場合は空文字を返す
            if (iColumn < 1) 
                return string.Empty;

            return ChangeExcelToColumnName((iColumn - 1) / 26) + (char)('A' + ((iColumn - 1) % 26));
        }

        /// <summary>
        /// 列合計の色を返す
        /// </summary>
        /// <returns></returns>
        // Mod Start WataruT 2020.07.30 土日祝の日勤計の色変更
        //public Color GetColumnTotalBackColor(double dColumnTotal, int iLimitCount, int iWorkKind)
        public Color GetColumnTotalBackColor(double dColumnTotal, int iLimitCount, int iWorkKind, string strWeekName)
        // Mod End   WataruT 2020.07.30 土日祝の日勤計の色変更
        {
            switch (iWorkKind)
            {
                // 日勤
                case 0:
                    // 合計が制限値を上回る場合
                    if (dColumnTotal > iLimitCount)
                    {
                        // Mod Start WataruT 2020.07.30 土日祝の日勤計の色変更
                        //return Color.White;
                        if (strWeekName == "土" || strWeekName == "日" || strWeekName == "祝")
                            return Color.DodgerBlue;
                        else
                            return Color.White;
                        // Mod End  WataruT 2020.07.30 土日祝の日勤計の色変更

                    }
                    // 合計が制限値と同じ場合
                    else if (dColumnTotal == iLimitCount)
                    {
                        return Color.LimeGreen;
                    }
                    // 合計が制限値を下回る場合
                    else
                    {
                        return Color.Red;
                    }
                // 夜勤・夜明
                case 1:
                case 2:
                    // 合計が制限値を上回る場合
                    if (dColumnTotal > iLimitCount)
                    {
                        return Color.Red;
                    }
                    // 合計が制限値と同じ場合
                    else if (dColumnTotal == iLimitCount)
                    {
                        return Color.LimeGreen;
                    }
                    // 合計が制限値を下回る場合
                    else
                    {
                        return Color.White;
                    }
                // 夜勤(女性)、夜勤(新人以外)
                case 4:
                case 5:
                    if (dColumnTotal == 0)
                        return Color.Red;
                    else
                        return Color.White;
                default:
                    return Color.White;
            }
        }

        /// <summary>
        /// 行合計の色を返す
        /// </summary>
        /// <returns></returns>
        public Color GetRowTotalBackColor(double dRowTotal, double dLimitCount, int iTargetRow)
        {
            switch(iTargetRow)
            {
                case 0:     // 公休
                    // 合計が制限値を上回る場合
                    if (dRowTotal > dLimitCount)
                    {
                        return Color.Red;
                    }
                    // 合計が制限値と同じ場合
                    else if (dRowTotal == dLimitCount)
                    {
                        return Color.LimeGreen;
                    }
                    // 合計が制限値を下回る場合
                    else
                    {
                        return Color.White;
                    }
                default:
                    return Color.White;
            }
        }
    }
}
