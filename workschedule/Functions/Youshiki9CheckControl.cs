using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workschedule.Functions
{
    class Youshiki9CheckControl
    {
        // 使用クラス宣言
        CommonControl clsCommonControl = new CommonControl();

        /// <summary>
        /// 月平均１日当たり看護配置数(看護職員+看護補助者)(基準値)
        /// </summary>
        /// <returns></returns>
        public string GetCheck1_Border(DataRow dt)
        {
            double dNurseCount;
            double dAverageDay;

            double.TryParse(dt["nurse_count"].ToString(), out dNurseCount);
            double.TryParse(dt["average_day"].ToString(), out dAverageDay);

            return clsCommonControl.ToRoundUp((dAverageDay / dNurseCount) * 3, 0).ToString();
        }

        /// <summary>
        /// 月平均１日当たり看護配置数(看護職員)(基準値)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string GetCheck2_Border(DataRow dt, string strCheck1)
        {
            double dNursePercentage1;
            double dCheck1;

            // 月平均１日当たり看護配置数(看護職員+看護補助者)が空欄の場合は別処理
            if (strCheck1 == "")
            {
                double dNurseCount;
                double dAverageDay;

                double.TryParse(dt["nurse_count"].ToString(), out dNurseCount);
                double.TryParse(dt["average_day"].ToString(), out dAverageDay);

                return clsCommonControl.ToRoundUp((dAverageDay / dNurseCount) * 3, 0).ToString();
            }

            double.TryParse(dt["nurse_percentage1"].ToString(), out dNursePercentage1);
            double.TryParse(strCheck1, out dCheck1);

            return clsCommonControl.ToRoundUp((dNursePercentage1 * dCheck1) / 100, 0).ToString();
        }

        /// <summary>
        /// 月平均１日当たり看護配置数(看護補助者)(基準値)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string GetCheck3_Border(DataRow dt)
        {
            double dAverageDay;
            double dCareCount;

            double.TryParse(dt["average_day"].ToString(), out dAverageDay);
            double.TryParse(dt["care_count"].ToString(), out dCareCount);

            return clsCommonControl.ToRoundUp((dAverageDay / dCareCount) * 3, 0).ToString();
        }

        /// <summary>
        /// 月平均１日当たり看護配置数(事務的業務を行う看護補助者)(基準値)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string GetCheck4_Border(DataRow dt)
        {
            double dAverageDay;

            double.TryParse(dt["average_day"].ToString(), out dAverageDay);

            return clsCommonControl.ToRoundDown((dAverageDay / 200) * 3, 2).ToString();
        }

        /// <summary>
        /// 月延べ勤務時間数(看護職員+看護補助者)(基準値)
        /// </summary>
        /// <returns></returns>
        public string GetCheck5_Border(string strCheck1, string strTargetDays)
        {
            double dCheck1;
            double dTargetDays;

            double.TryParse(strCheck1, out dCheck1);
            double.TryParse(strTargetDays, out dTargetDays);

            return clsCommonControl.ToRoundUp(dCheck1 * 8 * dTargetDays, 0).ToString();
        }

        /// <summary>
        /// 月延べ勤務時間数(看護職員)(基準値)
        /// </summary>
        /// <returns></returns>
        public string GetCheck6_Border(string strCheck2, string strTargetDays)
        {
            double dCheck2;
            double dTargetDays;

            double.TryParse(strCheck2, out dCheck2);
            double.TryParse(strTargetDays, out dTargetDays);

            return clsCommonControl.ToRoundUp(dCheck2 * 8 * dTargetDays, 0).ToString();
        }

        /// <summary>
        /// 月平均１日当たり看護配置数(看護補助者)(基準値)
        /// </summary>
        /// <returns></returns>
        public string GetCheck7_Border(string strCheck3, string strTargetDays)
        {
            double dCheck3;
            double dTargetDays;

            double.TryParse(strCheck3, out dCheck3);
            double.TryParse(strTargetDays, out dTargetDays);

            return clsCommonControl.ToRoundUp(dCheck3 * 8 * dTargetDays, 0).ToString();
        }

        /// <summary>
        /// 月平均１日当たり看護配置数(看護職員+看護補助者)(予定値)
        /// </summary>
        /// <returns></returns>
        public string GetCheck1_Schedule(string strCheck5, string strTargetDays)
        {
            double dCheck5;
            double dTargetDays;

            double.TryParse(strCheck5, out dCheck5);
            double.TryParse(strTargetDays, out dTargetDays);

            return clsCommonControl.ToRoundDown(dCheck5 / (dTargetDays * 8), 1).ToString();
        }

        /// <summary>
        /// 月平均１日当たり看護配置数(看護職員)(予定値)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string GetCheck2_Schedule(string strCheck6, string strTargetDays)
        {
            double dCheck6;
            double dTargetDays;

            double.TryParse(strCheck6, out dCheck6);
            double.TryParse(strTargetDays, out dTargetDays);

            return clsCommonControl.ToRoundDown(dCheck6 / (dTargetDays * 8), 1).ToString();
        }

        /// <summary>
        /// 月平均１日当たり看護配置数(看護補助者)(予定値)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string GetCheck3_Schedule(string strCheck7, string strTargetDays)
        {
            double dCheck7;
            double dTargetDays;

            double.TryParse(strCheck7, out dCheck7);
            double.TryParse(strTargetDays, out dTargetDays);

            return clsCommonControl.ToRoundDown(dCheck7 / (dTargetDays * 8), 1).ToString();
        }

        /// <summary>
        /// 月平均１日当たり看護配置数(事務的業務を行う看護補助者)(予定値)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string GetCheck4_Schedule(DataTable dtYoushiki9Check_Schedule, string strTargetDays)
        {
            double dTotalWorkHours = 0;
            double dTargetDays = 0;

            dTargetDays = double.Parse(strTargetDays);

            foreach (DataRow dr in dtYoushiki9Check_Schedule.Rows)
            {
                if(dr["staff_kind"].ToString() == "02" && dr["office_flag"].ToString() == "1")
                    dTotalWorkHours += double.Parse(dr["work_time"].ToString());
            }

            if (dTotalWorkHours == 0)
                return "0";
            else
                return clsCommonControl.ToRoundDown(dTotalWorkHours / (dTargetDays * 8), 2).ToString();
        }

        /// <summary>
        /// 月延べ勤務時間数(看護職員+看護補助者)(予定値)
        /// </summary>
        /// <returns></returns>
        public string GetCheck5_Schedule(DataTable dtYoushiki9Check_Schedule)
        {
            int iTotalWorkHours = 0;

            foreach (DataRow dr in dtYoushiki9Check_Schedule.Rows)
            {
                iTotalWorkHours += int.Parse(dr["work_time"].ToString());
            }

            return iTotalWorkHours.ToString();
        }

        /// <summary>
        /// 月延べ勤務時間数(看護職員)(予定値)
        /// </summary>
        /// <returns></returns>
        public string GetCheck6_Schedule(DataTable dtYoushiki9Check_Schedule)
        {
            int iTotalWorkHours = 0;
            string strTargetDay = "";
            int iNightCount = 0;
            int iNightAfterCount = 0;

            foreach (DataRow dr in dtYoushiki9Check_Schedule.Rows)
            {
                // 日付チェック
                if(dr["target_date"].ToString() != strTargetDay)
                {
                    // 対象日を更新
                    strTargetDay = dr["target_date"].ToString();
                    // 夜勤・夜明のカウントクリア
                    iNightCount = 0;
                    iNightAfterCount = 0;
                }
                if(dr["staff_kind"].ToString() == "01")
                {
                    switch(dr["work_kind"].ToString())
                    {
                        case "02":      // 夜勤
                            if(iNightCount != 2)
                            {
                                iTotalWorkHours += int.Parse(dr["work_time"].ToString());
                                iNightCount++;
                            }
                            break;
                        case "03":      // 夜明
                            if (iNightAfterCount != 2)
                            {
                                iTotalWorkHours += int.Parse(dr["work_time"].ToString());
                                iNightAfterCount++;
                            }
                            break;
                        default:
                            iTotalWorkHours += int.Parse(dr["work_time"].ToString());
                            break;
                    }
                }   
            }

            return iTotalWorkHours.ToString();
        }

        /// <summary>
        /// 月平均１日当たり看護配置数(看護補助者)(予定値)
        /// </summary>
        /// <returns></returns>
        public string GetCheck7_Schedule(DataTable dtYoushiki9Check_Schedule, string strCheck5, string strCheck6_Border, string strCheck6)
        {
            double dTotalWorkHours = 0;

            foreach (DataRow dr in dtYoushiki9Check_Schedule.Rows)
            {
                if (dr["staff_kind"].ToString() == "02")
                    dTotalWorkHours += double.Parse(dr["work_time"].ToString());
            }

            if(strCheck5 == "")
            {
                double dCheck6;
                double dCheck6_Border;

                double.TryParse(strCheck6, out dCheck6);
                double.TryParse(strCheck6_Border, out dCheck6_Border);

                if (dCheck6 - dCheck6_Border > 0)
                    dTotalWorkHours += dCheck6 - dCheck6_Border;
            }

            return dTotalWorkHours.ToString();
        }

        /// <summary>
        /// 看護要員中の看護職員の比率(予定値)
        /// </summary>
        /// <returns></returns>
        public string GetCheck8_Schedule(string strSchedule_Check6, string strBorder_Check5)
        {
            double dSchedule_Check6;
            double dBorder_Check5;

            double.TryParse(strSchedule_Check6, out dSchedule_Check6);
            double.TryParse(strBorder_Check5, out dBorder_Check5);

            if(clsCommonControl.ToRoundDown(dSchedule_Check6 / dBorder_Check5 * 100, 1) >= 100)
                return "100";
            else 
                return clsCommonControl.ToRoundDown(dSchedule_Check6 / dBorder_Check5 * 100, 1).ToString();
        }

        /// <summary>
        /// 看護職員中の看護師の比率(予定値)
        /// </summary>
        /// <returns></returns>
        public string GetCheck9_Schedule(DataTable dtYoushiki9Check_Schedule, string strBorder_Check6)
        {
            double dBorder_Check6;
            double dTotalWorkHours = 0;

            double.TryParse(strBorder_Check6, out dBorder_Check6);

            foreach (DataRow dr in dtYoushiki9Check_Schedule.Rows)
            {
                if (dr["staff_kind"].ToString() == "01" && dr["staff_kind_sub"].ToString() == "01")
                    dTotalWorkHours += double.Parse(dr["work_time"].ToString());
            }

            if (clsCommonControl.ToRoundDown(dTotalWorkHours / dBorder_Check6 * 100, 1) >= 100)
                return "100";
            else
                return clsCommonControl.ToRoundDown(dTotalWorkHours / dBorder_Check6 * 100, 1).ToString();
        }

        /// <summary>
        /// 月平均１日当たり看護配置数(看護職員+看護補助者)(実績値)
        /// </summary>
        /// <returns></returns>
        public string GetCheck1_Result(string strCheck5, string strTargetDays)
        {
            double dCheck5;
            double dTargetDays;

            double.TryParse(strCheck5, out dCheck5);
            double.TryParse(strTargetDays, out dTargetDays);

            return clsCommonControl.ToRoundDown(dCheck5 / (dTargetDays * 8), 1).ToString();
        }

        /// <summary>
        /// 月平均１日当たり看護配置数(看護職員)(実績値)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string GetCheck2_Result(string strCheck6, string strTargetDays)
        {
            double dCheck6;
            double dTargetDays;

            double.TryParse(strCheck6, out dCheck6);
            double.TryParse(strTargetDays, out dTargetDays);

            return clsCommonControl.ToRoundDown(dCheck6 / (dTargetDays * 8), 1).ToString();
        }

        /// <summary>
        /// 月平均１日当たり看護配置数(看護補助者)(実績値)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string GetCheck3_Result(string strCheck7, string strTargetDays)
        {
            double dCheck7;
            double dTargetDays;

            double.TryParse(strCheck7, out dCheck7);
            double.TryParse(strTargetDays, out dTargetDays);

            return clsCommonControl.ToRoundDown(dCheck7 / (dTargetDays * 8), 1).ToString();
        }

        /// <summary>
        /// 月平均１日当たり看護配置数(事務的業務を行う看護補助者)(実績値)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string GetCheck4_Result(DataTable dtYoushiki9Check_Result, DataTable dtYoushiki9Check_Schedule, string strTargetMonth, string strTargetDays)
        {
            double dTotalWorkHours = 0;
            double dTemp;
            string strTargetDate;
            bool bResultFlag;
            double dTargetDays;

            dTargetDays = double.Parse(strTargetDays);

            // 日にちごとに処理
            for (int i = 1; i <= int.Parse(strTargetDays); i++)
            {
                // 変数の初期化
                bResultFlag = false;
                strTargetDate = strTargetMonth + String.Format("{0:D2}", i);

                // 実績データ処理
                foreach (DataRow dr in dtYoushiki9Check_Result.Rows)
                {
                    if (dr["staff_kind"].ToString() == "02" && dr["office_flag"].ToString() == "1")
                    {
                        if (DateTime.Parse(dr["target_date"].ToString()).ToString("yyyyMMdd") == strTargetDate)
                        {
                            if (double.TryParse(dr["work_time_day"].ToString(), out dTemp))
                                dTotalWorkHours += dTemp;
                            if (double.TryParse(dr["work_time_night"].ToString(), out dTemp))
                                dTotalWorkHours += dTemp;
                            bResultFlag = true;
                        }
                    }
                }

                // 予定データ処理
                if (bResultFlag == false)
                {
                    foreach (DataRow dr in dtYoushiki9Check_Schedule.Rows)
                    {
                        if (dr["staff_kind"].ToString() == "02" && dr["office_flag"].ToString() == "1")
                        {
                            if (DateTime.Parse(dr["target_date"].ToString()).ToString("yyyyMMdd") == strTargetDate)
                            {
                                dTotalWorkHours += double.Parse(dr["work_time"].ToString());
                            }
                        }
                    }
                }
            }
            if (dTotalWorkHours == 0)
                return "0";
            else
                return clsCommonControl.ToRoundDown(dTotalWorkHours / (dTargetDays * 8), 2).ToString();
        }

        /// <summary>
        /// 月延べ勤務時間数(看護職員+看護補助者)(実績値)
        /// </summary>
        /// <returns></returns>
        public string GetCheck5_Result(DataTable dtYoushiki9Check_Result, DataTable dtYoushiki9Check_Schedule, string strTargetMonth, string strTargetDays)
        {
            double dTotalWorkHours = 0;
            double dTemp;
            string strTargetDate;
            bool bResultFlag;

            // 日にちごとに処理
            for(int i = 1; i <= int.Parse(strTargetDays); i++)
            {
                // 変数の初期化
                bResultFlag = false;
                strTargetDate = strTargetMonth + String.Format("{0:D2}", i);

                // 実績データ処理
                foreach (DataRow dr in dtYoushiki9Check_Result.Rows)
                {
                    if(DateTime.Parse(dr["target_date"].ToString()).ToString("yyyyMMdd") == strTargetDate)
                    {
                        if (double.TryParse(dr["work_time_day"].ToString(), out dTemp))
                            dTotalWorkHours += dTemp;
                        if (double.TryParse(dr["work_time_night"].ToString(), out dTemp))
                            dTotalWorkHours += dTemp;
                        bResultFlag = true;
                    }
                }

                // 予定データ処理
                if(bResultFlag == false)
                {
                    foreach (DataRow dr in dtYoushiki9Check_Schedule.Rows)
                    {
                        if (DateTime.Parse(dr["target_date"].ToString()).ToString("yyyyMMdd") == strTargetDate)
                        {
                            dTotalWorkHours += double.Parse(dr["work_time"].ToString());
                        }
                    }
                }
            }
            return dTotalWorkHours.ToString();
        }

        /// <summary>
        /// 月延べ勤務時間数(看護職員)(実績値)
        /// </summary>
        /// <returns></returns>
        public string GetCheck6_Result(DataTable dtYoushiki9Check_Result, DataTable dtYoushiki9Check_Schedule, string strTargetMonth, string strTargetDays)
        {
            double dTotalWorkHours = 0;
            double dTemp;
            string strTargetDate;
            bool bResultFlag;

            // 日にちごとに処理
            for (int i = 1; i <= int.Parse(strTargetDays); i++)
            {
                // 変数の初期化
                bResultFlag = false;
                strTargetDate = strTargetMonth + String.Format("{0:D2}", i);

                // 実績データ処理
                foreach (DataRow dr in dtYoushiki9Check_Result.Rows)
                {
                    if (dr["staff_kind"].ToString() == "01")
                    {
                        if (DateTime.Parse(dr["target_date"].ToString()).ToString("yyyyMMdd") == strTargetDate)
                        {
                            if (double.TryParse(dr["work_time_day"].ToString(), out dTemp))
                                dTotalWorkHours += dTemp;
                            if (double.TryParse(dr["work_time_night"].ToString(), out dTemp))
                                dTotalWorkHours += dTemp;
                            bResultFlag = true;
                        }
                    }   
                }

                // 予定データ処理
                if (bResultFlag == false)
                {
                    foreach (DataRow dr in dtYoushiki9Check_Schedule.Rows)
                    {
                        if(dr["staff_kind"].ToString() == "01")
                        {
                            if (DateTime.Parse(dr["target_date"].ToString()).ToString("yyyyMMdd") == strTargetDate)
                            {
                                dTotalWorkHours += double.Parse(dr["work_time"].ToString());
                            }
                        }
                    }
                }
            }
            return dTotalWorkHours.ToString();
        }

        /// <summary>
        /// 月平均１日当たり看護配置数(看護補助者)(実績値)
        /// </summary>
        /// <returns></returns>
        public string GetCheck7_Result(DataTable dtYoushiki9Check_Result, DataTable dtYoushiki9Check_Schedule, string strTargetMonth, string strTargetDays,
                                        string strResult_Check5, string strBorder_Check6, string strResult_Check6)
        {
            double dTotalWorkHours = 0;
            double dTemp;
            string strTargetDate;
            bool bResultFlag;

            // 日にちごとに処理
            for (int i = 1; i <= int.Parse(strTargetDays); i++)
            {
                // 変数の初期化
                bResultFlag = false;
                strTargetDate = strTargetMonth + String.Format("{0:D2}", i);

                // 実績データ処理
                foreach (DataRow dr in dtYoushiki9Check_Result.Rows)
                {
                    if (dr["staff_kind"].ToString() == "02")
                    {
                        if (DateTime.Parse(dr["target_date"].ToString()).ToString("yyyyMMdd") == strTargetDate)
                        {
                            if (double.TryParse(dr["work_time_day"].ToString(), out dTemp))
                                dTotalWorkHours += dTemp;
                            if (double.TryParse(dr["work_time_night"].ToString(), out dTemp))
                                dTotalWorkHours += dTemp;
                            bResultFlag = true;
                        }
                    }
                }

                // 予定データ処理
                if (bResultFlag == false)
                {
                    foreach (DataRow dr in dtYoushiki9Check_Schedule.Rows)
                    {
                        if (dr["staff_kind"].ToString() == "02")
                        {
                            if (DateTime.Parse(dr["target_date"].ToString()).ToString("yyyyMMdd") == strTargetDate)
                            {
                                dTotalWorkHours += double.Parse(dr["work_time"].ToString());
                            }
                        }
                    }
                }
            }

            if (strResult_Check5 == "")
            {
                double dResult_Check6;
                double dBorder_Check6;

                double.TryParse(strResult_Check6, out dResult_Check6);
                double.TryParse(strBorder_Check6, out dBorder_Check6);

                if (dResult_Check6 - dBorder_Check6 > 0)
                    dTotalWorkHours += dResult_Check6 - dBorder_Check6;
            }

            return dTotalWorkHours.ToString();
        }

        /// <summary>
        /// 看護要員中の看護職員の比率(実績値)
        /// </summary>
        /// <returns></returns>
        public string GetCheck8_Result(string strResult_Check6, string strBorder_Check5)
        {
            double dResult_Check6;
            double dBorder_Check5;

            double.TryParse(strResult_Check6, out dResult_Check6);
            double.TryParse(strBorder_Check5, out dBorder_Check5);

            if (clsCommonControl.ToRoundDown(dResult_Check6 / dBorder_Check5 * 100, 1) >= 100)
                return "100";
            else
                return clsCommonControl.ToRoundDown(dResult_Check6 / dBorder_Check5 * 100, 1).ToString();
        }

        /// <summary>
        /// 看護職員中の看護師の比率(実績値)
        /// </summary>
        /// <returns></returns>
        public string GetCheck9_Result(DataTable dtYoushiki9Check_Result, DataTable dtYoushiki9Check_Schedule, string strBorder_Check6, string strTargetMonth, string strTargetDays)
        {
            double dTotalWorkHours = 0;
            double dTemp;
            string strTargetDate;
            bool bResultFlag;
            double dBorder_Check6;

            double.TryParse(strBorder_Check6, out dBorder_Check6);

            // 日にちごとに処理
            for (int i = 1; i <= int.Parse(strTargetDays); i++)
            {
                // 変数の初期化
                bResultFlag = false;
                strTargetDate = strTargetMonth + String.Format("{0:D2}", i);

                // 実績データ処理
                foreach (DataRow dr in dtYoushiki9Check_Result.Rows)
                {
                    if (DateTime.Parse(dr["target_date"].ToString()).ToString("yyyyMMdd") == strTargetDate)
                    {
                        if (dr["staff_kind"].ToString() == "01" && dr["staff_kind_sub"].ToString() == "01")
                        {
                            if (double.TryParse(dr["work_time_day"].ToString(), out dTemp))
                                dTotalWorkHours += dTemp;
                            if (double.TryParse(dr["work_time_night"].ToString(), out dTemp))
                                dTotalWorkHours += dTemp;
                            bResultFlag = true;
                        }
                    }
                }

                // 予定データ処理
                if (bResultFlag == false)
                {
                    foreach (DataRow dr in dtYoushiki9Check_Schedule.Rows)
                    {
                        if (DateTime.Parse(dr["target_date"].ToString()).ToString("yyyyMMdd") == strTargetDate)
                        {
                            if (dr["staff_kind"].ToString() == "01" && dr["staff_kind_sub"].ToString() == "01")
                            {
                                dTotalWorkHours += double.Parse(dr["work_time"].ToString());
                            }   
                        }
                    }
                }
            }

            if (clsCommonControl.ToRoundDown(dTotalWorkHours / dBorder_Check6 * 100, 1) >= 100)
                return "100";
            else
                return clsCommonControl.ToRoundDown(dTotalWorkHours / dBorder_Check6 * 100, 1).ToString();
        }
    }
}
