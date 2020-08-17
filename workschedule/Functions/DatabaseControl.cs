using System;
using System.Data;
using System.IO;// WataruT
using System.Text;// WataruT
using MySql.Data.MySqlClient;

namespace workschedule.Controls
{
    class DatabaseControl
    {
        // ---- SELECT ----

        /// <summary>
        /// 職員マスタ取得(無効も含む)
        /// </summary>
        /// <returns></returns>
        public DataTable GetStaff_All()
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT * FROM m_staff ORDER BY ward, CAST(seq as SIGNED);";

                dt = GetDataTable(lsSQL);

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }

        }

        /// <summary>
        /// 職員マスタ取得(使用フラグありのみ)
        /// </summary>
        /// <returns></returns>
        public DataTable GetStaff()
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT * FROM m_staff WHERE using_flag = '1' ORDER BY ward, CAST(seq as SIGNED);";

                dt = GetDataTable(lsSQL);

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }

        }

        /// <summary>
        /// 職員マスタ取得(職員ID指定)
        /// </summary>
        /// <returns></returns>
        public DataTable GetStaff_ID(string strID)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT * FROM m_staff WHERE id = '" + strID + "' ORDER BY ward, CAST(seq as SIGNED);";

                dt = GetDataTable(lsSQL);

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 職員マスタの対象病棟のSEQ最大値を取得
        /// </summary>
        /// <returns></returns>
        public string GetStaff_MaxSEQ(string strWard)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    MAX(CAST(seq AS SIGNED)) as seq ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    m_staff ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    ward = '" + strWard + "';";

                dt = GetDataTable(lsSQL);

                if (dt.Rows.Count != 0)
                {
                    return dt.Rows[0]["seq"].ToString();
                }

                return "";

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 職員マスタの性別を取得
        /// </summary>
        /// <returns></returns>
        public string GetStaff_Sex(string strID)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    sex ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    m_staff ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    id = '" + strID + "';";

                dt = GetDataTable(lsSQL);

                if (dt.Rows.Count != 0)
                {
                    return dt.Rows[0]["sex"].ToString();
                }

                return "";

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 職員情報取得(対象病棟)
        /// </summary>
        /// <returns></returns>
        public DataTable GetLoginStaff(string strID, string strPassword)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    * ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    m_login_staff ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    id = '" + strID + "' AND ";
                lsSQL = lsSQL + "    password = '" + strPassword + "' ";
                lsSQL = lsSQL + "ORDER BY ";
                lsSQL = lsSQL + "    id ";

                dt = GetDataTable(lsSQL);

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }

        }

        /// <summary>
        /// 職員情報取得(対象病棟)
        /// </summary>
        /// <returns></returns>
        public DataTable GetStaff_Ward(string strWard)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    * ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    m_staff ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    ward = '" + strWard + "' AND ";
                lsSQL = lsSQL + "    using_flag = '1' ";
                lsSQL = lsSQL + "ORDER BY ";
                lsSQL = lsSQL + "    CAST(seq AS SIGNED) ";

                dt = GetDataTable(lsSQL);

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }

        }

        /// <summary>
        /// 職員情報取得(対象病棟、対象職種)
        /// </summary>
        /// <returns></returns>
        public DataTable GetStaff_Ward_StaffKind(string strWard, string strStaffKind)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    * ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    m_staff ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    ward = '" + strWard + "' AND ";
                lsSQL = lsSQL + "    staff_kind = '" + strStaffKind + "' AND ";
                lsSQL = lsSQL + "    using_flag = '1' ";
                lsSQL = lsSQL + "ORDER BY ";
                lsSQL = lsSQL + "    CAST(seq AS SIGNED) ";


                dt = GetDataTable(lsSQL);

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }

        }

        /// <summary>
        /// 病棟マスタ取得
        /// </summary>
        /// <returns></returns>
        public DataTable GetWard()
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT * FROM m_ward;";

                dt = GetDataTable(lsSQL);

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }

        }

        /// <summary>
        /// 病棟別様式9チェックフラグマスタ取得
        /// </summary>
        /// <returns></returns>
        public DataTable GetWardYoushiki9Flag()
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT * FROM m_ward_youshiki9_flag ORDER BY ward;";

                dt = GetDataTable(lsSQL);

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 対象病棟の様式9チェックフラグマスタ取得
        /// </summary>
        /// <returns></returns>
        public DataTable GetWardYoushiki9Flag_Ward(string strWard)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    * ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    m_ward_youshiki9_flag ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    ward = '" + strWard + "';";

                dt = GetDataTable(lsSQL);

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 勤務種類マスタ取得
        /// </summary>
        /// <returns></returns>
        public DataTable GetWorkKind()
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT * FROM m_work_kind;";

                dt = GetDataTable(lsSQL);

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }

        }

        /// <summary>
        /// 対象の勤務種類の勤務時間を取得
        /// </summary>
        /// <returns></returns>
        public string GetWorkKind_WorkTime(string strID)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    work_time ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    m_work_kind ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    id = '" + strID + "';";

                dt = GetDataTable(lsSQL);

                if (dt.Rows.Count != 0)
                {
                    return dt.Rows[0]["work_time"].ToString();
                }

                return "";

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 職種マスタ取得
        /// </summary>
        /// <returns></returns>
        public DataTable GetStaffKind()
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT * FROM m_staff_kind;";

                dt = GetDataTable(lsSQL);

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }

        }

        /// <summary>
        /// 役職マスタ取得
        /// </summary>
        /// <returns></returns>
        public DataTable GetStaffPosition()
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT * FROM m_staff_position;";

                dt = GetDataTable(lsSQL);

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }

        }

        /// <summary>
        /// 祝日マスタ取得
        /// </summary>
        /// <returns></returns>
        public DataTable GetHoliday()
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT * FROM m_holiday;";

                dt = GetDataTable(lsSQL);

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }

        }

        /// <summary>
        /// 曜日マスタ取得
        /// </summary>
        /// <returns></returns>
        public DataTable GetDayOfWeek()
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT * FROM m_day_of_week;";

                dt = GetDataTable(lsSQL);

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }

        }

        /// <summary>
        /// 救急指定日データ取得
        /// </summary>
        /// <returns></returns>
        public DataTable GetEmergencyDate()
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT * FROM d_emergency_date ORDER BY target_date DESC;";

                dt = GetDataTable(lsSQL);

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }

        }

        /// <summary>
        /// 対象月の救急指定日データ取得
        /// </summary>
        /// <returns></returns>
        public DataTable GetEmergencyDate_TargetMonth(string strTargetMonth)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT DATE_FORMAT(target_date, '%Y%m%d') as target_date ";
                lsSQL = lsSQL + "FROM d_emergency_date WHERE target_date like '" + strTargetMonth + "%' ORDER BY target_date DESC;";

                dt = GetDataTable(lsSQL);

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }

        }

        /// <summary>
        /// 対象病棟の希望シフト一覧
        /// </summary>
        /// <returns></returns>
        public DataTable GetRequestShift_Ward(string strWard, string strTagetMonth)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    * ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    d_request_shift ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    ward = '" + strWard + "' AND ";
                lsSQL = lsSQL + "    DATE_FORMAT(target_date, '%Y%m') = '" + strTagetMonth + "' ";
                lsSQL = lsSQL + "ORDER BY ";
                lsSQL = lsSQL + "    staff, ";
                lsSQL = lsSQL + "    target_date;";

                dt = GetDataTable(lsSQL);

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }

        }

        /// <summary>
        /// 常日勤マスタ取得
        /// </summary>
        /// <returns></returns>
        public DataTable GetStaffDayOnly_Ward(string strWard, string strStaffKind)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    * ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    d_staff_day_only ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    ward = '" + strWard + "' AND ";
                lsSQL = lsSQL + "    staff_kind = '" + strStaffKind + "';";

                dt = GetDataTable(lsSQL);

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 対象年月、対象職種の勤務予定者の常日勤マスタ取得
        /// </summary>
        /// <returns></returns>
        public DataTable GetStaffDayOnly_Ward_TargetMonth(string strWard, string strStaffKind, string strTargetMonth)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    dsdo.* ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    d_staff_day_only as dsdo, ";
                lsSQL = lsSQL + "    d_schedule_staff as dss ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    dsdo.ward = dss.ward AND ";
                lsSQL = lsSQL + "    dsdo.staff_kind = dss.staff_kind AND ";
                lsSQL = lsSQL + "    dsdo.staff = dss.staff_id AND ";
                lsSQL = lsSQL + "    dsdo.ward = '" + strWard + "' AND ";
                lsSQL = lsSQL + "    dsdo.staff_kind = '" + strStaffKind + "' AND ";
                lsSQL = lsSQL + "    dss.target_month = " + strTargetMonth + ";";

                dt = GetDataTable(lsSQL);

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 対象の勤務種類の勤務時間を取得
        /// </summary>
        /// <returns></returns>
        public string GetStaffDayOnly_OfficeFlag(string strID)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    office_flag ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    d_staff_day_only ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    staff = '" + strID + "';";

                dt = GetDataTable(lsSQL);

                if (dt.Rows.Count != 0)
                {
                    return dt.Rows[0]["office_flag"].ToString();
                }

                return "0";

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 職員の職能レベルを取得
        /// </summary>
        /// <returns></returns>
        public string GetStaffDayOnly_StaffLevel(string strID)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    staff_level ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    d_staff_day_only ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    staff = '" + strID + "';";

                dt = GetDataTable(lsSQL);

                if (dt.Rows.Count != 0)
                {
                    return dt.Rows[0]["staff_level"].ToString();
                }

                return "1";

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 予定職員一覧に表示する職員情報を取得
        /// </summary>
        /// <returns></returns>
        public DataTable GetScheduleStaff_List(string strWard, string strTargetMonth, string strStaffKind)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    ms.id, ";
                lsSQL = lsSQL + "    ms.name, ";
                lsSQL = lsSQL + "    ms.sex ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    d_schedule_staff dss, ";
                lsSQL = lsSQL + "    m_staff ms ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    dss.staff_id = ms.id AND";
                lsSQL = lsSQL + "    dss.ward = '" + strWard + "' AND";
                lsSQL = lsSQL + "    dss.target_month = '" + strTargetMonth + "' AND";
                lsSQL = lsSQL + "    dss.staff_kind = '" + strStaffKind + "' ";
                lsSQL = lsSQL + "ORDER BY ";
                lsSQL = lsSQL + "    CAST(dss.seq AS SIGNED);";

                dt = GetDataTable(lsSQL);

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 様式9の出力時に使用する職員一覧
        /// </summary>
        /// <returns></returns>
        public DataTable GetScheduleStaff_Youshiki9(string strWard, string strTargetMonth, string strStaffKind)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    ms.id as id, ";
                lsSQL = lsSQL + "    ms.name as name, ";
                lsSQL = lsSQL + "    msk.name as staff_kind, ";
                lsSQL = lsSQL + "    dss.office_flag as office_flag ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    m_staff ms, ";
                lsSQL = lsSQL + "    m_staff_kind msk, ";
                lsSQL = lsSQL + "    d_schedule_staff dss ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    ms.id = dss.staff_id AND ";
                lsSQL = lsSQL + "    ms.staff_kind = msk.id AND ";
                lsSQL = lsSQL + "    ms.staff_kind_sub = msk.sub_id AND ";
                lsSQL = lsSQL + "    dss.ward = '" + strWard + "' AND";
                lsSQL = lsSQL + "    dss.target_month = '" + strTargetMonth + "' AND";
                lsSQL = lsSQL + "    dss.staff_kind = '" + strStaffKind + "' ";
                lsSQL = lsSQL + "ORDER BY ";
                lsSQL = lsSQL + "    CAST(dss.seq AS SIGNED);";

                dt = GetDataTable(lsSQL);

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 様式9の出力時に使用する職員一覧(締日翌日～翌月締まで用)
        /// </summary>
        /// <returns></returns>
        public DataTable GetScheduleStaff_Youshiki9_Half(string strWard, string strTargetMonth, string strTargetNextMonth, string strStaffKind)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT DISTINCT";
                lsSQL = lsSQL + "    ms.id as id, ";
                lsSQL = lsSQL + "    ms.name as name, ";
                lsSQL = lsSQL + "    msk.name as staff_kind, ";
                // Mod Start WataruT 2020.07.15 計画表出力時のSQLエラー対応
                //lsSQL = lsSQL + "    dss.office_flag as office_flag ";
                lsSQL = lsSQL + "    dss.office_flag as office_flag, ";
                lsSQL = lsSQL + "    ms.staff_kind as seq1,";
                lsSQL = lsSQL + "    ms.staff_kind_sub as seq2,";
                lsSQL = lsSQL + "    dss.target_month as seq3,";
                lsSQL = lsSQL + "    dss.seq as seq4 ";
                // Mod End   WataruT 2020.07.15 計画表出力時のSQLエラー対応
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    m_staff ms, ";
                lsSQL = lsSQL + "    m_staff_kind msk, ";
                lsSQL = lsSQL + "    d_schedule_staff dss ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    ms.id = dss.staff_id AND ";
                lsSQL = lsSQL + "    ms.staff_kind = msk.id AND ";
                lsSQL = lsSQL + "    ms.staff_kind_sub = msk.sub_id AND ";
                lsSQL = lsSQL + "    dss.ward = '" + strWard + "' AND";
                lsSQL = lsSQL + "    (dss.target_month = '" + strTargetMonth + "' OR ";
                lsSQL = lsSQL + "    dss.target_month = '" + strTargetNextMonth + "') AND";
                lsSQL = lsSQL + "    dss.staff_kind = '" + strStaffKind + "' ";
                lsSQL = lsSQL + "ORDER BY ";
                // Mod Start WataruT 2020.07.15 計画表出力時のSQLエラー対応
                //lsSQL = lsSQL + "    CAST(ms.staff_kind AS SIGNED), ";
                //lsSQL = lsSQL + "    CAST(ms.staff_kind_sub AS SIGNED), ";
                //lsSQL = lsSQL + "    CAST(dss.target_month AS SIGNED), ";
                //lsSQL = lsSQL + "    CAST(dss.seq AS SIGNED);";
                lsSQL = lsSQL + "    CAST(seq1 AS SIGNED), ";
                lsSQL = lsSQL + "    CAST(seq2 AS SIGNED), ";
                lsSQL = lsSQL + "    CAST(dss.target_month AS SIGNED), ";
                lsSQL = lsSQL + "    CAST(dss.seq AS SIGNED) ";
                // Mod End   WataruT 2020.07.15 計画表出力時のSQLエラー対応

                dt = GetDataTable(lsSQL);

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 対象病棟、対象年月、対象職種の制限値マスタ取得
        /// </summary>
        /// <returns></returns>
        public DataTable GetCountLimitDay_Ward(string strWard, string strStaffKind)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT * FROM d_count_limit_day WHERE ward = '" + strWard + "' AND staff_kind = '" + strStaffKind + "' ORDER BY day_of_week";

                dt = GetDataTable(lsSQL);

                return dt;
            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }

        }

        /// <summary>
        /// 勤務予定のデータ番号の最大値を取得
        /// </summary>
        /// <returns></returns>
        public string GetScheduleHeader_MaxScheduleNo()
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    MAX(CAST(schedule_no AS SIGNED)) as schedule_no ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    d_schedule_header ";

                dt = GetDataTable(lsSQL);

                if (dt.Rows.Count != 0)
                {
                    return dt.Rows[0]["schedule_no"].ToString();
                }

                return "";

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 勤務初回予定のデータ番号の最大値を取得
        /// </summary>
        /// <returns></returns>
        public string GetScheduleFirstHeader_MaxScheduleNo()
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    MAX(CAST(schedule_no AS SIGNED)) as schedule_no ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    d_schedule_first_header ";

                dt = GetDataTable(lsSQL);

                if (dt.Rows.Count != 0)
                {
                    return dt.Rows[0]["schedule_no"].ToString();
                }

                return "";

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 対象年月、対象病棟、対象職種のデータ番号を取得
        /// </summary>
        /// <returns></returns>
        public string GetScheduleHeader_TargetScheduleNo(string strWard, string strTargetMonth, string strStaffKind)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    * ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    d_schedule_header ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    ward = '" + strWard + "' AND ";
                lsSQL = lsSQL + "    target_month = '" + strTargetMonth + "' AND ";
                lsSQL = lsSQL + "    staff_kind = '" + strStaffKind + "';";

                dt = GetDataTable(lsSQL);

                if (dt.Rows.Count != 0)
                {
                    return dt.Rows[0]["schedule_no"].ToString();
                }

                return "";

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 対象年月、対象病棟、対象職種の初回データ番号を取得
        /// </summary>
        /// <returns></returns>
        public string GetScheduleFirstHeader_TargetScheduleNo(string strWard, string strTargetMonth, string strStaffKind)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    * ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    d_schedule_first_header ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    ward = '" + strWard + "' AND ";
                lsSQL = lsSQL + "    target_month = '" + strTargetMonth + "' AND ";
                lsSQL = lsSQL + "    staff_kind = '" + strStaffKind + "';";

                dt = GetDataTable(lsSQL);

                if (dt.Rows.Count != 0)
                {
                    return dt.Rows[0]["schedule_no"].ToString();
                }

                return "";

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 対象年月、対象病棟、対象職種の初回データの存在チェック
        /// </summary>
        /// <returns></returns>
        public bool GetScheduleFirstHeader_FirstFlag(string strWard, string strTargetMonth, string strStaffKind)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    * ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    d_schedule_first_header ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    ward = '" + strWard + "' AND ";
                lsSQL = lsSQL + "    target_month = '" + strTargetMonth + "' AND ";
                lsSQL = lsSQL + "    staff_kind = '" + strStaffKind + "';";

                dt = GetDataTable(lsSQL);

                if (dt.Rows.Count != 0)
                {
                    return true;
                }

                return false;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }

        /// <summary>
        /// 病棟、職員ID、職種、対象年月から勤務予定初回詳細データを取得
        /// </summary>
        /// <returns></returns>
        public DataTable GetScheduleDetail_Ward_Staff_StaffKind_TargetMonth(string strWard, string strStaff, string strStaffKind, string strTargetMonth)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    b.*, ";
                lsSQL = lsSQL + "    c.name_short ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    d_schedule_header a,";
                lsSQL = lsSQL + "    d_schedule_detail b,";
                lsSQL = lsSQL + "    m_work_kind c ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    a.ward = '" + strWard + "' AND ";
                lsSQL = lsSQL + "    a.target_month = '" + strTargetMonth + "' AND ";
                lsSQL = lsSQL + "    a.staff_kind = '" + strStaffKind + "' AND ";
                lsSQL = lsSQL + "    b.staff = '" + strStaff + "' AND ";
                lsSQL = lsSQL + "    a.schedule_no = b.schedule_no AND ";
                lsSQL = lsSQL + "    b.work_kind = c.id ";
                lsSQL = lsSQL + "ORDER BY ";
                lsSQL = lsSQL + "    CAST(b.schedule_detail_no AS SIGNED);";

                dt = GetDataTable(lsSQL);

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 病棟、職員ID、職種、対象年月から勤務予定最終詳細データを取得
        /// </summary>
        /// <returns></returns>
        public DataTable GetScheduleFirstDetail_Ward_Staff_StaffKind_TargetMonth(string strWard, string strStaff, string strStaffKind, string strTargetMonth)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    b.*, ";
                lsSQL = lsSQL + "    c.name_short ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    d_schedule_first_header a,";
                lsSQL = lsSQL + "    d_schedule_first_detail b,";
                lsSQL = lsSQL + "    m_work_kind c ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    a.ward = '" + strWard + "' AND ";
                lsSQL = lsSQL + "    a.target_month = '" + strTargetMonth + "' AND ";
                lsSQL = lsSQL + "    a.staff_kind = '" + strStaffKind + "' AND ";
                lsSQL = lsSQL + "    b.staff = '" + strStaff + "' AND ";
                lsSQL = lsSQL + "    a.schedule_no = b.schedule_no AND ";
                lsSQL = lsSQL + "    b.work_kind = c.id ";
                lsSQL = lsSQL + "ORDER BY ";
                lsSQL = lsSQL + "    CAST(b.schedule_detail_no AS SIGNED);";

                dt = GetDataTable(lsSQL);

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 対象病棟、対象年月、対象職種の夜勤の性別取得用SQL
        /// </summary>
        /// <returns></returns>
        public DataTable GetScheduleDetail_Ward_StaffKind_TargetMonth(string strWard, string strStaffKind, string strTargetMonth)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    dsd.staff, ";
                lsSQL = lsSQL + "    DATE_FORMAT(dsd.target_date, '%e') - 1 as day, ";
                lsSQL = lsSQL + "    ms.sex, ";
                lsSQL = lsSQL + "    dsd.work_kind ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    m_staff ms, ";
                lsSQL = lsSQL + "    d_schedule_staff dss, ";
                lsSQL = lsSQL + "    d_schedule_header dsh, ";
                lsSQL = lsSQL + "    d_schedule_detail dsd ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    ms.id = dsd.staff AND ";
                lsSQL = lsSQL + "    dsh.schedule_no = dsd.schedule_no AND ";
                lsSQL = lsSQL + "    dsh.ward = '" + strWard + "' AND ";
                lsSQL = lsSQL + "    dsd.work_kind = '02' AND ";
                lsSQL = lsSQL + "    dsh.target_month = '" + strTargetMonth + "' AND ";
                lsSQL = lsSQL + "    dsh.staff_kind = '" + strStaffKind + "' AND ";
                lsSQL = lsSQL + "    ms.id = dss.staff_id AND ";
                lsSQL = lsSQL + "    dsh.target_month = dss.target_month AND ";
                lsSQL = lsSQL + "    dsh.staff_kind = dss.staff_kind;";


                dt = GetDataTable(lsSQL);

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 対象病棟、対象年月、対象職種の夜勤の職能レベル別取得用SQL
        /// </summary>
        /// <returns></returns>
        public DataTable GetScheduleDetail_Ward_StaffKind_TargetMonth_ForStaffLevel(string strWard, string strStaffKind, string strTargetMonth)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    dss.staff_id, ";
                lsSQL = lsSQL + "    DATE_FORMAT(dsd.target_date, '%e') - 1 as day, ";
                lsSQL = lsSQL + "    dsdo.staff_level ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    d_staff_day_only dsdo, ";
                lsSQL = lsSQL + "    d_schedule_staff dss, ";
                lsSQL = lsSQL + "    d_schedule_header dsh, ";
                lsSQL = lsSQL + "    d_schedule_detail dsd ";
                lsSQL = lsSQL + "where ";
                lsSQL = lsSQL + "    dss.ward = '" + strWard + "' and ";
                lsSQL = lsSQL + "    dss.target_month = '" + strTargetMonth + "' AND ";
                lsSQL = lsSQL + "    dss.staff_kind = '" + strStaffKind + "' AND ";
                lsSQL = lsSQL + "    dsd.work_kind = '02' AND ";
                lsSQL = lsSQL + "    dsdo.staff_level <> '1' AND ";
                lsSQL = lsSQL + "    dss.ward = dsh.ward AND ";
                lsSQL = lsSQL + "    dss.staff_id = dsdo.staff AND ";
                lsSQL = lsSQL + "    dss.target_month = dsh.target_month AND ";
                lsSQL = lsSQL + "    dsh.schedule_no = dsd.schedule_no AND ";
                lsSQL = lsSQL + "    dss.staff_id = dsd.staff AND ";
                lsSQL = lsSQL + "    dss.staff_kind = dsh.staff_kind;";

                dt = GetDataTable(lsSQL);

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 対象病棟、対象年月、対象職種、対象勤務の勤務情報を取得
        /// Add WataruT 2020.07.30 遅出の表示対応
        /// </summary>
        /// <returns></returns>
        public DataTable GetScheduleDetail_Ward_StaffKind_TargetMonth_WorkKind(string strWard, string strStaffKind, string strTargetMonth, string strWorkKind)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    DATE_FORMAT(dsd.target_date, '%e') - 1 as day, ";
                lsSQL = lsSQL + "    dsd.* ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    d_schedule_staff as dss, ";
                lsSQL = lsSQL + "    d_schedule_header as dsh, ";
                lsSQL = lsSQL + "    d_schedule_detail as dsd ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    dss.ward = dsh.ward AND ";
                lsSQL = lsSQL + "    dss.target_month = dsh.target_month AND ";
                lsSQL = lsSQL + "    dsh.schedule_no = dsd.schedule_no AND ";
                lsSQL = lsSQL + "    dss.ward = '" + strWard + "' AND ";
                lsSQL = lsSQL + "    dss.staff_id = dsd.staff AND ";
                lsSQL = lsSQL + "    dss.target_month = '" + strTargetMonth + "' AND ";
                lsSQL = lsSQL + "    dss.staff_kind = '" + strStaffKind + "' AND ";
                lsSQL = lsSQL + "    dsd.work_kind = '" + strWorkKind + "';";

                dt = GetDataTable(lsSQL);

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 勤務実績のデータ番号の最大値を取得
        /// </summary>
        /// <returns></returns>
        public string GetResultHeader_MaxResultNo()
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    MAX(CAST(result_no AS SIGNED)) as result_no ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    d_result_header ";

                dt = GetDataTable(lsSQL);

                if (dt.Rows.Count != 0)
                {
                    return dt.Rows[0]["result_no"].ToString();
                }

                return "";

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 対象年月、対象病棟、対象職種の実績データ番号を取得
        /// </summary>
        /// <returns></returns>
        public string GetResultHeader_TargetResultNo(string strWard, string strTargetMonth, string strStaffKind)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    * ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    d_result_header ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    ward = '" + strWard + "' AND ";
                lsSQL = lsSQL + "    target_month = '" + strTargetMonth + "' AND ";
                lsSQL = lsSQL + "    staff_kind = '" + strStaffKind + "';";

                dt = GetDataTable(lsSQL);

                if (dt.Rows.Count != 0)
                {
                    return dt.Rows[0]["result_no"].ToString();
                }

                return "";

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 対象職員の対象日の勤務予定情報を取得
        /// </summary>
        /// <returns></returns>
        public string GetScheduleDetail_Staff_TargetDate(string strStaff, string strTargetDate)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    * ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    d_schedule_detail ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    target_date = '" + strTargetDate + "' and ";
                lsSQL = lsSQL + "    staff = '" + strStaff + "';";

                dt = GetDataTable(lsSQL);

                if (dt.Rows.Count != 0)
                {
                    return dt.Rows[0]["work_kind"].ToString();
                }

                return "";

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 対象職員の対象日の勤務情報を取得
        /// </summary>
        /// <returns></returns>
        public string GetResultDetail_Staff_TargetDate(string strStaff, string strTargetDate)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    * ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    d_result_detail ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    target_date = '" + strTargetDate + "' and ";
                lsSQL = lsSQL + "    staff = '" +  strStaff + "';";

                dt = GetDataTable(lsSQL);

                if (dt.Rows.Count != 0)
                {
                    return dt.Rows[0]["work_kind"].ToString();
                }

                return "";

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 病棟、職員ID、職種、対象年月から勤務予定詳細データを取得
        /// </summary>
        /// <returns></returns>
        public DataTable GetResultDetail_Ward_Staff_StaffKind_TargetMonth(string strWard, string strStaff, string strStaffKind, string strTargetMonth)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    * ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    d_result_header rh, ";
                lsSQL = lsSQL + "    d_result_detail rd ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    rh.result_no = rd.result_no AND ";
                lsSQL = lsSQL + "    rh.ward = '" + strWard + "' AND ";
                lsSQL = lsSQL + "    rh.target_month = '" + strTargetMonth + "' AND ";
                lsSQL = lsSQL + "    rd.staff = '" + strStaff + "' AND ";
                lsSQL = lsSQL + "    rh.staff_kind = '" + strStaffKind + "' ";
                lsSQL = lsSQL + "ORDER BY ";
                lsSQL = lsSQL + "    CAST(rd.result_detail_no as SIGNED) ";
                
                dt = GetDataTable(lsSQL);

                return dt;
            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 対象病棟、対象年月の勤務詳細項目絞り込みでデータ取得(勤務詳細帳票出力用)
        /// </summary>
        /// <returns></returns>
        public DataTable GetResultDetail_Ward_TargetDate_ResultDetailItem(string strTargetWard, string strTargetMonth, string strWorkKindName)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    s.name as staff_name, ";
                lsSQL = lsSQL + "    sk.name as staff_kind_name, ";
                lsSQL = lsSQL + "    DATE_FORMAT(rd.target_date, '%m月%d日') as target_date, ";
                lsSQL = lsSQL + "    rd.other1_work_kind as work_kind, ";
                lsSQL = lsSQL + "    rd.other1_start_time as start_time, ";
                lsSQL = lsSQL + "    rd.other1_end_time as end_time, ";
                lsSQL = lsSQL + "    time(rd.other1_end_time - rd.other1_start_time) as total_time ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    m_staff s, ";
                lsSQL = lsSQL + "    m_staff_kind sk, ";
                lsSQL = lsSQL + "    d_result_header rh, ";
                lsSQL = lsSQL + "    d_result_detail rd ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    s.id = rd.staff AND ";
                lsSQL = lsSQL + "    s.staff_kind = sk.id AND ";
                lsSQL = lsSQL + "    s.staff_kind_sub = sk.sub_id AND ";
                lsSQL = lsSQL + "    rh.result_no = rd.result_no AND ";
                lsSQL = lsSQL + "    rh.ward = '" + strTargetWard + "' AND ";
                lsSQL = lsSQL + "    rh.target_month = '" + strTargetMonth + "' AND ";
                lsSQL = lsSQL + "    rd.other1_work_kind = '" + strWorkKindName + "' ";
                lsSQL = lsSQL + "UNION ALL ";
                lsSQL = lsSQL + "SELECT ";
                lsSQL = lsSQL + "    s.name as staff_name, ";
                lsSQL = lsSQL + "    sk.name as staff_kind_name, ";
                lsSQL = lsSQL + "    DATE_FORMAT(rd.target_date, '%m月%d日') as target_date, ";
                lsSQL = lsSQL + "    rd.other2_work_kind as work_kind, ";
                lsSQL = lsSQL + "    rd.other2_start_time as start_time, ";
                lsSQL = lsSQL + "    rd.other2_end_time as end_time, ";
                lsSQL = lsSQL + "    time(rd.other2_end_time - rd.other2_start_time) as total_time ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    m_staff s, ";
                lsSQL = lsSQL + "    m_staff_kind sk, ";
                lsSQL = lsSQL + "    d_result_header rh, ";
                lsSQL = lsSQL + "    d_result_detail rd ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    s.id = rd.staff AND ";
                lsSQL = lsSQL + "    s.staff_kind = sk.id AND ";
                lsSQL = lsSQL + "    s.staff_kind_sub = sk.sub_id AND ";
                lsSQL = lsSQL + "    rh.result_no = rd.result_no AND ";
                lsSQL = lsSQL + "    rh.ward = '" + strTargetWard + "' AND ";
                lsSQL = lsSQL + "    rh.target_month = '" + strTargetMonth + "' AND ";
                lsSQL = lsSQL + "    rd.other2_work_kind = '" + strWorkKindName + "' ";
                lsSQL = lsSQL + "UNION ALL ";
                lsSQL = lsSQL + "SELECT ";
                lsSQL = lsSQL + "    s.name as staff_name, ";
                lsSQL = lsSQL + "    sk.name as staff_kind_name, ";
                lsSQL = lsSQL + "    DATE_FORMAT(rd.target_date, '%m月%d日') as target_date, ";
                lsSQL = lsSQL + "    rd.other3_work_kind as work_kind, ";
                lsSQL = lsSQL + "    rd.other3_start_time as start_time, ";
                lsSQL = lsSQL + "    rd.other3_end_time as end_time, ";
                lsSQL = lsSQL + "    time(rd.other3_end_time - rd.other3_start_time) as total_time ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    m_staff s, ";
                lsSQL = lsSQL + "    m_staff_kind sk, ";
                lsSQL = lsSQL + "    d_result_header rh, ";
                lsSQL = lsSQL + "    d_result_detail rd ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    s.id = rd.staff AND ";
                lsSQL = lsSQL + "    s.staff_kind = sk.id AND ";
                lsSQL = lsSQL + "    s.staff_kind_sub = sk.sub_id AND ";
                lsSQL = lsSQL + "    rh.result_no = rd.result_no AND ";
                lsSQL = lsSQL + "    rh.ward = '" + strTargetWard + "' AND ";
                lsSQL = lsSQL + "    rh.target_month = '" + strTargetMonth + "' AND ";
                lsSQL = lsSQL + "    rd.other3_work_kind = '" + strWorkKindName + "' ";
                lsSQL = lsSQL + "ORDER BY ";
                lsSQL = lsSQL + "    target_date, staff_name;";

                dt = GetDataTable(lsSQL);

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 対象病棟、対象年月の勤務詳細項目絞り込みでデータ取得(勤務詳細帳票出力用_その他)
        /// </summary>
        /// <returns></returns>
        public DataTable GetResultDetail_Ward_TargetDate_ResultDetailItem_Other(string strTargetWard, string strTargetMonth)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    s.name as staff_name, ";
                lsSQL = lsSQL + "    sk.name as staff_kind_name, ";
                lsSQL = lsSQL + "    DATE_FORMAT(rd.target_date, '%m月%d日') as target_date, ";
                lsSQL = lsSQL + "    rd.other1_work_kind as work_kind, ";
                lsSQL = lsSQL + "    rd.other1_start_time as start_time, ";
                lsSQL = lsSQL + "    rd.other1_end_time as end_time, ";
                lsSQL = lsSQL + "    time(rd.other1_end_time - rd.other1_start_time) as total_time ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    m_staff s, ";
                lsSQL = lsSQL + "    m_staff_kind sk, ";
                lsSQL = lsSQL + "    d_result_header rh, ";
                lsSQL = lsSQL + "    d_result_detail rd ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    s.id = rd.staff AND ";
                lsSQL = lsSQL + "    s.staff_kind = sk.id AND ";
                lsSQL = lsSQL + "    s.staff_kind_sub = sk.sub_id AND ";
                lsSQL = lsSQL + "    rh.result_no = rd.result_no AND ";
                lsSQL = lsSQL + "    rh.ward = '" + strTargetWard + "' AND ";
                lsSQL = lsSQL + "    rh.target_month = '" + strTargetMonth + "' AND ";
                lsSQL = lsSQL + "    (rd.other1_work_kind <> '委員会のため' AND rd.other1_work_kind <> '外出のため' AND rd.other1_work_kind <> '研修のため' AND rd.other1_work_kind <> '') ";
                lsSQL = lsSQL + "UNION ALL ";
                lsSQL = lsSQL + "SELECT ";
                lsSQL = lsSQL + "    s.name as staff_name, ";
                lsSQL = lsSQL + "    sk.name as staff_kind_name, ";
                lsSQL = lsSQL + "    DATE_FORMAT(rd.target_date, '%m月%d日') as target_date, ";
                lsSQL = lsSQL + "    rd.other2_work_kind as work_kind, ";
                lsSQL = lsSQL + "    rd.other2_start_time as start_time, ";
                lsSQL = lsSQL + "    rd.other2_end_time as end_time, ";
                lsSQL = lsSQL + "    time(rd.other2_end_time - rd.other2_start_time) as total_time ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    m_staff s, ";
                lsSQL = lsSQL + "    m_staff_kind sk, ";
                lsSQL = lsSQL + "    d_result_header rh, ";
                lsSQL = lsSQL + "    d_result_detail rd ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    s.id = rd.staff AND ";
                lsSQL = lsSQL + "    s.staff_kind = sk.id AND ";
                lsSQL = lsSQL + "    s.staff_kind_sub = sk.sub_id AND ";
                lsSQL = lsSQL + "    rh.result_no = rd.result_no AND ";
                lsSQL = lsSQL + "    rh.ward = '" + strTargetWard + "' AND ";
                lsSQL = lsSQL + "    rh.target_month = '" + strTargetMonth + "' AND ";
                lsSQL = lsSQL + "    (rd.other2_work_kind <> '委員会のため' AND rd.other2_work_kind <> '外出のため' AND rd.other2_work_kind <> '研修のため' AND rd.other2_work_kind <> '') ";
                lsSQL = lsSQL + "UNION ALL ";
                lsSQL = lsSQL + "SELECT ";
                lsSQL = lsSQL + "    s.name as staff_name, ";
                lsSQL = lsSQL + "    sk.name as staff_kind_name, ";
                lsSQL = lsSQL + "    DATE_FORMAT(rd.target_date, '%m月%d日') as target_date, ";
                lsSQL = lsSQL + "    rd.other3_work_kind as work_kind, ";
                lsSQL = lsSQL + "    rd.other3_start_time as start_time, ";
                lsSQL = lsSQL + "    rd.other3_end_time as end_time, ";
                lsSQL = lsSQL + "    time(rd.other3_end_time - rd.other3_start_time) as total_time ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    m_staff s, ";
                lsSQL = lsSQL + "    m_staff_kind sk, ";
                lsSQL = lsSQL + "    d_result_header rh, ";
                lsSQL = lsSQL + "    d_result_detail rd ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    s.id = rd.staff AND ";
                lsSQL = lsSQL + "    s.staff_kind = sk.id AND ";
                lsSQL = lsSQL + "    s.staff_kind_sub = sk.sub_id AND ";
                lsSQL = lsSQL + "    rh.result_no = rd.result_no AND ";
                lsSQL = lsSQL + "    rh.ward = '" + strTargetWard + "' AND ";
                lsSQL = lsSQL + "    rh.target_month = '" + strTargetMonth + "' AND ";
                lsSQL = lsSQL + "    (rd.other3_work_kind <> '委員会のため' AND rd.other3_work_kind <> '外出のため' AND rd.other3_work_kind <> '研修のため' AND rd.other3_work_kind <> '') ";
                lsSQL = lsSQL + "ORDER BY ";
                lsSQL = lsSQL + "    target_date, staff_name;";

                dt = GetDataTable(lsSQL);

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 対象病棟、対象年月の勤務詳細項目絞り込みで特定職員のデータ取得(勤務表時間計算用)
        /// Add WataruT 2020.08.06 遅刻・早退入力対応
        /// </summary>
        /// <returns></returns>
        public DataTable GetResultDetail_Ward_TargetDate_ResultDetailItem_Staff(string strTargetWard, string strTargetMonth, string strWorkKindName, string strID)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    s.name as staff_name, ";
                lsSQL = lsSQL + "    sk.name as staff_kind_name, ";
                lsSQL = lsSQL + "    DATE_FORMAT(rd.target_date, '%m月%d日') as target_date, ";
                lsSQL = lsSQL + "    rd.target_date as target_date_basic, ";    // Add WataruT 2020.08.06 遅刻・早退入力対応
                lsSQL = lsSQL + "    rd.other1_work_kind as work_kind, ";
                lsSQL = lsSQL + "    rd.other1_start_time as start_time, ";
                lsSQL = lsSQL + "    rd.other1_end_time as end_time, ";
                lsSQL = lsSQL + "    time(rd.other1_end_time - rd.other1_start_time) as total_time ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    m_staff s, ";
                lsSQL = lsSQL + "    m_staff_kind sk, ";
                lsSQL = lsSQL + "    d_result_header rh, ";
                lsSQL = lsSQL + "    d_result_detail rd ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    s.id = rd.staff AND ";
                lsSQL = lsSQL + "    s.staff_kind = sk.id AND ";
                lsSQL = lsSQL + "    s.staff_kind_sub = sk.sub_id AND ";
                lsSQL = lsSQL + "    rh.result_no = rd.result_no AND ";
                lsSQL = lsSQL + "    s.id = '" + strID + "' AND ";
                lsSQL = lsSQL + "    rh.ward = '" + strTargetWard + "' AND ";
                lsSQL = lsSQL + "    rh.target_month = '" + strTargetMonth + "' AND ";
                lsSQL = lsSQL + "    rd.other1_work_kind = '" + strWorkKindName + "' ";
                lsSQL = lsSQL + "UNION ALL ";
                lsSQL = lsSQL + "SELECT ";
                lsSQL = lsSQL + "    s.name as staff_name, ";
                lsSQL = lsSQL + "    sk.name as staff_kind_name, ";
                lsSQL = lsSQL + "    DATE_FORMAT(rd.target_date, '%m月%d日') as target_date, ";
                lsSQL = lsSQL + "    rd.target_date as target_date_basic, ";    // Add WataruT 2020.08.06 遅刻・早退入力対応
                lsSQL = lsSQL + "    rd.other2_work_kind as work_kind, ";
                lsSQL = lsSQL + "    rd.other2_start_time as start_time, ";
                lsSQL = lsSQL + "    rd.other2_end_time as end_time, ";
                lsSQL = lsSQL + "    time(rd.other2_end_time - rd.other2_start_time) as total_time ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    m_staff s, ";
                lsSQL = lsSQL + "    m_staff_kind sk, ";
                lsSQL = lsSQL + "    d_result_header rh, ";
                lsSQL = lsSQL + "    d_result_detail rd ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    s.id = rd.staff AND ";
                lsSQL = lsSQL + "    s.staff_kind = sk.id AND ";
                lsSQL = lsSQL + "    s.staff_kind_sub = sk.sub_id AND ";
                lsSQL = lsSQL + "    rh.result_no = rd.result_no AND ";
                lsSQL = lsSQL + "    s.id = '" + strID + "' AND ";
                lsSQL = lsSQL + "    rh.ward = '" + strTargetWard + "' AND ";
                lsSQL = lsSQL + "    rh.target_month = '" + strTargetMonth + "' AND ";
                lsSQL = lsSQL + "    rd.other2_work_kind = '" + strWorkKindName + "' ";
                lsSQL = lsSQL + "UNION ALL ";
                lsSQL = lsSQL + "SELECT ";
                lsSQL = lsSQL + "    s.name as staff_name, ";
                lsSQL = lsSQL + "    sk.name as staff_kind_name, ";
                lsSQL = lsSQL + "    DATE_FORMAT(rd.target_date, '%m月%d日') as target_date, ";
                lsSQL = lsSQL + "    rd.target_date as target_date_basic, ";    // Add WataruT 2020.08.06 遅刻・早退入力対応
                lsSQL = lsSQL + "    rd.other3_work_kind as work_kind, ";
                lsSQL = lsSQL + "    rd.other3_start_time as start_time, ";
                lsSQL = lsSQL + "    rd.other3_end_time as end_time, ";
                lsSQL = lsSQL + "    time(rd.other3_end_time - rd.other3_start_time) as total_time ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    m_staff s, ";
                lsSQL = lsSQL + "    m_staff_kind sk, ";
                lsSQL = lsSQL + "    d_result_header rh, ";
                lsSQL = lsSQL + "    d_result_detail rd ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    s.id = rd.staff AND ";
                lsSQL = lsSQL + "    s.staff_kind = sk.id AND ";
                lsSQL = lsSQL + "    s.staff_kind_sub = sk.sub_id AND ";
                lsSQL = lsSQL + "    rh.result_no = rd.result_no AND ";
                lsSQL = lsSQL + "    s.id = '" + strID + "' AND ";
                lsSQL = lsSQL + "    rh.ward = '" + strTargetWard + "' AND ";
                lsSQL = lsSQL + "    rh.target_month = '" + strTargetMonth + "' AND ";
                lsSQL = lsSQL + "    rd.other3_work_kind = '" + strWorkKindName + "' ";
                lsSQL = lsSQL + "ORDER BY ";
                lsSQL = lsSQL + "    target_date, staff_name;";

                dt = GetDataTable(lsSQL);

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 様式9のデータ一覧取得(対象年月)
        /// </summary>
        /// <returns></returns>
        public DataTable GetWardYoushiki9_TargetMonth(string strTargetMonth)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    * ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    d_ward_youshiki9 ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    target_month = '" + strTargetMonth + "' ";
                lsSQL = lsSQL + "ORDER BY ";
                lsSQL = lsSQL + "    ward;";

                dt = GetDataTable(lsSQL);

                return dt;
            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }

        }

        /// <summary>
        /// 様式9のデータ一覧取得(対象年月、対象病棟)
        /// </summary>
        /// <returns></returns>
        public DataTable GetWardYoushiki9_TargetMonth_Ward(string strTargetMonth, string strWard)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    * ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    d_ward_youshiki9 ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    target_month = '" + strTargetMonth + "' AND ";
                lsSQL = lsSQL + "    ward = '" + strWard + "' ";
                lsSQL = lsSQL + "ORDER BY ";
                lsSQL = lsSQL + "    ward;";

                dt = GetDataTable(lsSQL);

                return dt;
            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }

        }

        /// <summary>
        /// 様式9チェック用データ(予定)
        /// </summary>
        /// <returns></returns>
        public DataTable GetYoushiki9Check_Schedule(string strTargetMonth, string strWard)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    s.id, ";
                lsSQL = lsSQL + "    s.name, ";
                lsSQL = lsSQL + "    s.staff_kind, ";
                lsSQL = lsSQL + "    s.staff_kind_sub, ";
                lsSQL = lsSQL + "    sd.target_date, ";
                lsSQL = lsSQL + "    wk.work_time, ";
                lsSQL = lsSQL + "    sdo.office_flag, ";
                lsSQL = lsSQL + "    wk.id as work_kind ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    m_staff s, ";
                lsSQL = lsSQL + "    m_work_kind wk, ";
                lsSQL = lsSQL + "    d_schedule_staff ss, ";
                lsSQL = lsSQL + "    d_schedule_header sh, ";
                lsSQL = lsSQL + "    d_schedule_detail sd, ";
                lsSQL = lsSQL + "    d_staff_day_only sdo ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    sh.schedule_no = sd.schedule_no AND ";
                lsSQL = lsSQL + "    sh.ward = '" + strWard + "' AND ";
                lsSQL = lsSQL + "    sh.ward = ss.ward AND ";
                lsSQL = lsSQL + "    sh.target_month = '" + strTargetMonth + "' AND ";
                lsSQL = lsSQL + "    sh.target_month = ss.target_month AND ";
                lsSQL = lsSQL + "    s.id = ss.staff_id AND ";
                lsSQL = lsSQL + "    sd.staff = s.id AND ";
                lsSQL = lsSQL + "    sd.work_kind = wk.id AND ";
                lsSQL = lsSQL + "    wk.work_kind = '1' AND ";
                lsSQL = lsSQL + "    sh.ward = sdo.ward AND ";
                lsSQL = lsSQL + "    s.id = sdo.staff ";
                lsSQL = lsSQL + "ORDER BY ";
                lsSQL = lsSQL + "    sd.target_date, ";
                lsSQL = lsSQL + "    CAST(s.seq as SIGNED);";

                dt = GetDataTable(lsSQL);

                return dt;
            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 様式9チェック用データ(実績)
        /// </summary>
        /// <returns></returns>
        public DataTable GetYoushiki9Check_Result(string strTargetMonth, string strWard)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    s.id, ";
                lsSQL = lsSQL + "    s.name, ";
                lsSQL = lsSQL + "    s.staff_kind, ";
                lsSQL = lsSQL + "    s.staff_kind_sub, ";
                lsSQL = lsSQL + "    rd.target_date, ";
                lsSQL = lsSQL + "    rd.work_time_day, ";
                lsSQL = lsSQL + "    rd.work_time_night, ";
                lsSQL = lsSQL + "    rd.work_time_night_total, ";
                lsSQL = lsSQL + "    sdo.office_flag ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    m_staff s, ";
                lsSQL = lsSQL + "    m_work_kind wk, ";
                lsSQL = lsSQL + "    d_schedule_staff ss, ";
                lsSQL = lsSQL + "    d_result_header rh, ";
                lsSQL = lsSQL + "    d_result_detail rd, ";
                lsSQL = lsSQL + "    d_staff_day_only sdo ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    rh.result_no = rd.result_no AND ";
                lsSQL = lsSQL + "    rh.ward = '" + strWard + "' AND ";
                lsSQL = lsSQL + "    rh.ward = ss.ward AND ";
                lsSQL = lsSQL + "    rh.target_month = '" + strTargetMonth + "' AND ";
                lsSQL = lsSQL + "    rh.target_month = ss.target_month AND ";
                lsSQL = lsSQL + "    s.id = ss.staff_id AND ";
                lsSQL = lsSQL + "    s.id = rd.staff AND ";
                lsSQL = lsSQL + "    rd.work_kind = wk.id AND ";
                lsSQL = lsSQL + "    wk.work_kind = '1' AND ";
                lsSQL = lsSQL + "    rh.ward = sdo.ward AND ";
                lsSQL = lsSQL + "    s.id = sdo.staff ";
                lsSQL = lsSQL + "ORDER BY ";
                lsSQL = lsSQL + "    CAST(s.seq as SIGNED), ";
                lsSQL = lsSQL + "    rd.target_date ";

                dt = GetDataTable(lsSQL);   

                return dt;
            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 実績データに取り込む予定データを取得
        /// </summary>
        /// <param name="strWard"></param>
        /// <param name="strTargetMonth"></param>
        /// <param name="strStaffKind"></param>
        /// <param name="strTargetDate"></param>
        /// <returns></returns>
        public DataTable GetScheduleDetail_Ward_TargetMonth_StaffKind_TargetDate(string strWard, string strTargetMonth, string strStaffKind, string strTargetDate)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    b.staff, ";
                lsSQL = lsSQL + "    b.work_kind, ";
                lsSQL = lsSQL + "    c.work_time, ";
                lsSQL = lsSQL + "    c.name as work_kind_name ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    d_schedule_header a, ";
                lsSQL = lsSQL + "    d_schedule_detail b, ";
                lsSQL = lsSQL + "    m_work_kind c ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    a.ward = '" + strWard + "' AND ";
                lsSQL = lsSQL + "    a.target_month = '" + strTargetMonth + "' AND ";
                lsSQL = lsSQL + "    a.staff_kind = '" + strStaffKind + "' AND ";
                lsSQL = lsSQL + "    b.target_date = '" + strTargetDate + "' AND ";
                lsSQL = lsSQL + "    a.schedule_no = b.schedule_no AND ";
                lsSQL = lsSQL + "    b.work_kind = c.id AND ";
                lsSQL = lsSQL + "    c.work_kind = '1' ";
                lsSQL = lsSQL + "ORDER BY ";
                lsSQL = lsSQL + "    CAST(b.schedule_detail_no AS SIGNED);";

                dt = GetDataTable(lsSQL);

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// 対象日が休日かチェック
        /// </summary>
        /// <returns></returns>
        public bool GetHoliday_Check(string strTargetDate)
        {
            try
            {
                string lsSQL;
                DataTable dt;

                lsSQL = "SELECT ";
                lsSQL = lsSQL + "    * ";
                lsSQL = lsSQL + "FROM ";
                lsSQL = lsSQL + "    m_holiday ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    holiday = '" + strTargetDate + "';";

                dt = GetDataTable(lsSQL);

                if (dt.Rows.Count != 0)
                {
                    return true;
                }

                return false;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }

        // --- INSERT ---

        /// <summary>
        /// 職員マスタ登録
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool InsertStaff(DataRow data)
        {
            try
            {
                string lsSQL;

                lsSQL = "INSERT INTO m_staff VALUES(";
                lsSQL = lsSQL + "'" + data["id"] + "', ";
                lsSQL = lsSQL + "'" + data["name"] + "', ";
                lsSQL = lsSQL + "'" + data["ward"] + "', ";
                lsSQL = lsSQL + "'" + data["sex"] + "', ";
                lsSQL = lsSQL + "'" + data["staff_kind"] + "', ";
                lsSQL = lsSQL + "'" + data["staff_kind_sub"] + "', ";
                lsSQL = lsSQL + "'" + data["staff_position"] + "', ";
                lsSQL = lsSQL + "'" + data["seq"] + "', ";
                lsSQL = lsSQL + "'" + data["using_flag"] + "', ";
                lsSQL = lsSQL + "'" + data["created_date"] + "', ";
                lsSQL = lsSQL + "'" + data["updated_date"] + "');";

                if (ExecuteSQL(lsSQL) == true)
                {
                    return true;
                }

                return false;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }

        /// <summary>
        /// 希望シフト登録
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool InsertRequestShift(DataRow data)
        {
            try
            {
                string lsSQL;

                lsSQL = "INSERT INTO d_request_shift VALUES(";
                lsSQL = lsSQL + "'" + data["staff"] + "', ";
                lsSQL = lsSQL + "'" + data["ward"] + "', ";
                lsSQL = lsSQL + "'" + data["target_date"] + "', ";
                lsSQL = lsSQL + "'" + data["staff_kind"] + "', ";
                lsSQL = lsSQL + "'" + data["work_kind"] + "', ";
                lsSQL = lsSQL + "'" + DateTime.Now + "', ";
                lsSQL = lsSQL + "'" + DateTime.Now + "');";

                if (ExecuteSQL(lsSQL) == true)
                {
                    return true;
                }

                return false;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }

        /// <summary>
        /// SQL取得(希望シフトデータ_INSERT側)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetInsertRequestShift_Insert()
        {
            return "INSERT INTO d_request_shift VALUES";
        }

        /// <summary>
        /// SQL取得(希望シフトデータ_VALUES側)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetInsertRequestShift_Values(DataRow data)
        {
            string lsSQL;

            lsSQL = "(";
            lsSQL = lsSQL + "'" + data["staff"] + "', ";
            lsSQL = lsSQL + "'" + data["ward"] + "', ";
            lsSQL = lsSQL + "'" + data["target_date"] + "', ";
            lsSQL = lsSQL + "'" + data["staff_kind"] + "', ";
            lsSQL = lsSQL + "'" + data["work_kind"] + "', ";
            lsSQL = lsSQL + "'" + DateTime.Now + "', ";
            lsSQL = lsSQL + "'" + DateTime.Now + "'),";

            return lsSQL;
        }

        /// <summary>
        /// 勤務予定ヘッダ登録
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool InsertScheduleHeader(DataRow data)
        {
            try
            {
                string lsSQL;

                lsSQL = "INSERT INTO d_schedule_header VALUES(";
                lsSQL = lsSQL + "'" + data["schedule_no"] + "', ";
                lsSQL = lsSQL + "'" + data["ward"] + "', ";
                lsSQL = lsSQL + "'" + data["staff_kind"] + "', ";
                lsSQL = lsSQL + "'" + data["target_month"] + "', ";
                lsSQL = lsSQL + "'" + DateTime.Now + "', ";
                lsSQL = lsSQL + "'" + DateTime.Now + "');";

                if (ExecuteSQL(lsSQL) == true)
                {
                    return true;
                }

                return false;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }

        /// <summary>
        /// SQL取得(勤務予定ヘッダ_INSERT側)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetInsertScheduleHeader_Insert()
        {
            return "INSERT INTO d_schedule_header VALUES";
        }

        /// <summary>
        /// SQL取得(勤務予定ヘッダ_VALUES側)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetInsertScheduleHeader_Values(DataRow data)
        {
            string lsSQL;

            lsSQL = "(";
            lsSQL = lsSQL + "'" + data["schedule_no"] + "', ";
            lsSQL = lsSQL + "'" + data["ward"] + "', ";
            lsSQL = lsSQL + "'" + data["staff_kind"] + "', ";
            lsSQL = lsSQL + "'" + data["target_month"] + "', ";
            lsSQL = lsSQL + "'" + DateTime.Now + "', ";
            lsSQL = lsSQL + "'" + DateTime.Now + "'),";

            return lsSQL;
        }

        /// <summary>
        /// 勤務予定詳細登録
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool InsertScheduleDetail(DataRow data)
        {
            try
            {
                string lsSQL;

                lsSQL = "INSERT INTO d_schedule_detail VALUES(";
                lsSQL = lsSQL + "'" + data["schedule_no"] + "', ";
                lsSQL = lsSQL + "'" + data["schedule_detail_no"] + "', ";
                lsSQL = lsSQL + "'" + data["staff"] + "', ";
                lsSQL = lsSQL + "'" + data["target_date"] + "', ";
                lsSQL = lsSQL + "'" + data["work_kind"] + "', ";
                lsSQL = lsSQL + "'" + data["request_flag"] + "', ";
                lsSQL = lsSQL + "'" + DateTime.Now + "', ";
                lsSQL = lsSQL + "'" + DateTime.Now + "');";

                if (ExecuteSQL(lsSQL) == true)
                {
                    return true;
                }

                return false;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }

        /// <summary>
        /// SQL取得(勤務予定詳細_INSERT側)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetInsertScheduleDetail_Insert()
        {
            return "INSERT INTO d_schedule_detail VALUES";
        }

        /// <summary>
        /// SQL取得(勤務予定詳細_VALUES側)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetInsertScheduleDetail_Values(DataRow data)
        {
            string lsSQL;

            lsSQL = "(";
            lsSQL = lsSQL + "'" + data["schedule_no"] + "', ";
            lsSQL = lsSQL + "'" + data["schedule_detail_no"] + "', ";
            lsSQL = lsSQL + "'" + data["staff"] + "', ";
            lsSQL = lsSQL + "'" + data["target_date"] + "', ";
            lsSQL = lsSQL + "'" + data["work_kind"] + "', ";
            lsSQL = lsSQL + "'" + data["request_flag"] + "', ";
            lsSQL = lsSQL + "'" + DateTime.Now + "', ";
            lsSQL = lsSQL + "'" + DateTime.Now + "'),";

            return lsSQL;
        }

        /// <summary>
        /// 勤務初回予定ヘッダ登録
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool InsertScheduleFirstHeader(DataRow data)
        {
            try
            {
                string lsSQL;

                lsSQL = "INSERT INTO d_schedule_first_header VALUES(";
                lsSQL = lsSQL + "'" + data["schedule_no"] + "', ";
                lsSQL = lsSQL + "'" + data["ward"] + "', ";
                lsSQL = lsSQL + "'" + data["staff_kind"] + "', ";
                lsSQL = lsSQL + "'" + data["target_month"] + "', ";
                lsSQL = lsSQL + "'" + DateTime.Now + "', ";
                lsSQL = lsSQL + "'" + DateTime.Now + "');";

                if (ExecuteSQL(lsSQL) == true)
                {
                    return true;
                }

                return false;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }

        /// <summary>
        /// 勤務初回予定詳細登録
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool InsertScheduleFirstDetail(DataRow data)
        {
            try
            {
                string lsSQL;

                lsSQL = "INSERT INTO d_schedule_first_detail VALUES(";
                lsSQL = lsSQL + "'" + data["schedule_no"] + "', ";
                lsSQL = lsSQL + "'" + data["schedule_detail_no"] + "', ";
                lsSQL = lsSQL + "'" + data["staff"] + "', ";
                lsSQL = lsSQL + "'" + data["target_date"] + "', ";
                lsSQL = lsSQL + "'" + data["work_kind"] + "', ";
                lsSQL = lsSQL + "'" + data["request_flag"] + "', ";
                lsSQL = lsSQL + "'" + DateTime.Now + "', ";
                lsSQL = lsSQL + "'" + DateTime.Now + "');";

                if (ExecuteSQL(lsSQL) == true)
                {
                    return true;
                }

                return false;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }

        /// <summary>
        /// SQL取得(勤務初回予定詳細_INSERT側)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetInsertScheduleFirstDetail_Insert()
        {
            return "INSERT INTO d_schedule_first_detail VALUES";
        }

        /// <summary>
        /// SQL取得(勤務初回予定詳細_VALUES側)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetInsertScheduleFirstDetail_Values(DataRow data)
        {
            string lsSQL;

            lsSQL = "(";
            lsSQL = lsSQL + "'" + data["schedule_no"] + "', ";
            lsSQL = lsSQL + "'" + data["schedule_detail_no"] + "', ";
            lsSQL = lsSQL + "'" + data["staff"] + "', ";
            lsSQL = lsSQL + "'" + data["target_date"] + "', ";
            lsSQL = lsSQL + "'" + data["work_kind"] + "', ";
            lsSQL = lsSQL + "'" + data["request_flag"] + "', ";
            lsSQL = lsSQL + "'" + DateTime.Now + "', ";
            lsSQL = lsSQL + "'" + DateTime.Now + "'),";

            return lsSQL;
        }

        /// <summary>
        /// 勤務実績ヘッダ登録
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool InsertResultHeader(DataRow data)
        {
            try
            {
                string lsSQL;

                lsSQL = "INSERT INTO d_result_header VALUES(";
                lsSQL = lsSQL + "'" + data["result_no"] + "', ";
                lsSQL = lsSQL + "'" + data["ward"] + "', ";
                lsSQL = lsSQL + "'" + data["staff_kind"] + "', ";
                lsSQL = lsSQL + "'" + data["target_month"] + "', ";
                lsSQL = lsSQL + "'" + DateTime.Now + "', ";
                lsSQL = lsSQL + "'" + DateTime.Now + "');";

                if (ExecuteSQL(lsSQL) == true)
                {
                    return true;
                }

                return false;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }

        /// <summary>
        /// 勤務実績詳細登録
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool InsertResultDetail(DataRow data)
        {
            try
            {
                string lsSQL;

                lsSQL = "INSERT INTO d_result_detail VALUES(";
                lsSQL = lsSQL + "'" + data["result_no"] + "', ";
                lsSQL = lsSQL + "'" + data["result_detail_no"] + "', ";
                lsSQL = lsSQL + "'" + data["staff"] + "', ";
                lsSQL = lsSQL + "'" + data["target_date"] + "', ";
                lsSQL = lsSQL + "'" + data["work_kind"] + "', ";
                lsSQL = lsSQL + "'" + data["work_time_day"] + "', ";
                lsSQL = lsSQL + "'" + data["work_time_night"] + "', ";
                lsSQL = lsSQL + "'" + data["work_time_night_total"] + "', ";
                lsSQL = lsSQL + "'" + data["change_flag"] + "', ";
                lsSQL = lsSQL + "'" + data["other1_work_kind"] + "', ";
                lsSQL = lsSQL + "'" + data["other1_start_time"] + "', ";
                lsSQL = lsSQL + "'" + data["other1_end_time"] + "', ";
                lsSQL = lsSQL + "'" + data["other2_work_kind"] + "', ";
                lsSQL = lsSQL + "'" + data["other2_start_time"] + "', ";
                lsSQL = lsSQL + "'" + data["other2_end_time"] + "', ";
                lsSQL = lsSQL + "'" + data["other3_work_kind"] + "', ";
                lsSQL = lsSQL + "'" + data["other3_start_time"] + "', ";
                lsSQL = lsSQL + "'" + data["other3_end_time"] + "', ";
                lsSQL = lsSQL + "'" + DateTime.Now + "', ";
                lsSQL = lsSQL + "'" + DateTime.Now + "');";

                if (ExecuteSQL(lsSQL) == true)
                {
                    return true;
                }

                return false;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }

        /// <summary>
        /// SQL取得(勤務実績詳細_INSERT側)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetInsertResultDetail_Insert()
        {
            return "INSERT INTO d_result_detail VALUES";
        }

        /// <summary>
        /// SQL取得(勤務実績詳細_VALUES側)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetInsertResultDetail_Values(DataRow data)
        {
            string lsSQL;

            lsSQL = "(";
            lsSQL = lsSQL + "'" + data["result_no"] + "', ";
            lsSQL = lsSQL + "'" + data["result_detail_no"] + "', ";
            lsSQL = lsSQL + "'" + data["staff"] + "', ";
            lsSQL = lsSQL + "'" + data["target_date"] + "', ";
            lsSQL = lsSQL + "'" + data["work_kind"] + "', ";
            lsSQL = lsSQL + "'" + data["work_time_day"] + "', ";
            lsSQL = lsSQL + "'" + data["work_time_night"] + "', ";
            lsSQL = lsSQL + "'" + data["work_time_night_total"] + "', ";
            lsSQL = lsSQL + "'" + data["change_flag"] + "', ";
            lsSQL = lsSQL + "'" + data["other1_work_kind"] + "', ";
            lsSQL = lsSQL + "'" + data["other1_start_time"] + "', ";
            lsSQL = lsSQL + "'" + data["other1_end_time"] + "', ";
            lsSQL = lsSQL + "'" + data["other2_work_kind"] + "', ";
            lsSQL = lsSQL + "'" + data["other2_start_time"] + "', ";
            lsSQL = lsSQL + "'" + data["other2_end_time"] + "', ";
            lsSQL = lsSQL + "'" + data["other3_work_kind"] + "', ";
            lsSQL = lsSQL + "'" + data["other3_start_time"] + "', ";
            lsSQL = lsSQL + "'" + data["other3_end_time"] + "', ";
            lsSQL = lsSQL + "'" + DateTime.Now + "', ";
            lsSQL = lsSQL + "'" + DateTime.Now + "'),";

            return lsSQL;
        }

        /// <summary>
        /// 常日勤データ登録
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool InsertStaffDayOnly(DataRow data)
        {
            try
            {
                string lsSQL;

                lsSQL = "INSERT INTO d_staff_day_only VALUES(";
                lsSQL = lsSQL + "'" + data["staff"] + "', ";
                lsSQL = lsSQL + "'" + data["staff_kind"] + "', ";
                lsSQL = lsSQL + "'" + data["ward"] + "', ";
                if (data["target_day_start"].ToString() == "")
                {
                    lsSQL = lsSQL + " null, ";
                }
                else
                {
                    lsSQL = lsSQL + "'" + data["target_day_start"] + "', ";
                }
                if (data["target_day_end"].ToString() == "")
                {
                    lsSQL = lsSQL + " null, ";
                }
                else
                {
                    lsSQL = lsSQL + "'" + data["target_day_end"] + "', ";
                }
                lsSQL = lsSQL + "'" + data["holiday_flag"] + "', ";
                lsSQL = lsSQL + "'" + data["office_flag"] + "', ";
                lsSQL = lsSQL + "'" + data["staff_level"] + "', ";
                lsSQL = lsSQL + "'" + DateTime.Now + "', ";
                lsSQL = lsSQL + "'" + DateTime.Now + "');";

                if (ExecuteSQL(lsSQL) == true)
                {
                    return true;
                }

                return false;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }

        /// <summary>
        /// 予定職員一覧登録
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool InsertScheduleStaff(DataRow data)
        {
            try
            {
                string lsSQL;

                lsSQL = "INSERT INTO d_schedule_staff VALUES(";
                lsSQL = lsSQL + "'" + data["ward"] + "', ";
                lsSQL = lsSQL + "'" + data["target_month"] + "', ";
                lsSQL = lsSQL + "'" + data["staff_kind"] + "', ";
                lsSQL = lsSQL + "'" + data["staff_id"] + "', ";
                lsSQL = lsSQL + "'" + data["seq"] + "', ";
                lsSQL = lsSQL + "'" + data["office_flag"] + "', ";
                lsSQL = lsSQL + "'" + DateTime.Now + "', ";
                lsSQL = lsSQL + "'" + DateTime.Now + "');";

                if (ExecuteSQL(lsSQL) == true)
                {
                    return true;
                }

                return false;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }

        /// <summary>
        /// 様式9設定登録
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool InsertWardYoushiki9(DataRow data)
        {
            try
            {
                string lsSQL;

                lsSQL = "INSERT INTO d_ward_youshiki9 VALUES(";
                lsSQL = lsSQL + "'" + data["ward"] + "', ";
                lsSQL = lsSQL + "'" + data["target_month"] + "', ";
                lsSQL = lsSQL + "'" + data["kubun"] + "', ";
                lsSQL = lsSQL + "'" + data["nurse_count"] + "', ";
                lsSQL = lsSQL + "'" + data["care_count"] + "', ";
                lsSQL = lsSQL + "'" + data["ward_count"] + "', ";
                lsSQL = lsSQL + "'" + data["bed_count"] + "', ";
                lsSQL = lsSQL + "'" + data["average_day"] + "', ";
                lsSQL = lsSQL + "'" + data["nurse_percentage1"] + "', ";
                lsSQL = lsSQL + "'" + data["nurse_percentage2"] + "', ";
                lsSQL = lsSQL + "'" + data["average_year"] + "', ";
                lsSQL = lsSQL + "'" + DateTime.Now + "', ";
                lsSQL = lsSQL + "'" + DateTime.Now + "');";

                if (ExecuteSQL(lsSQL) == true)
                {
                    return true;
                }

                return false;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }

        /// <summary>
        /// 曜日別制限値登録
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool InsertCountLimitDay(DataRow data)
        {
            try
            {
                string lsSQL;

                lsSQL = "INSERT INTO d_count_limit_day VALUES(";
                lsSQL = lsSQL + "'" + data["ward"] + "', ";
                lsSQL = lsSQL + "'" + data["staff_kind"] + "', ";
                lsSQL = lsSQL + "'" + data["day_of_week"] + "', ";
                lsSQL = lsSQL + "'" + data["day_min"] + "', ";
                lsSQL = lsSQL + "'" + data["night_max"] + "', ";
                lsSQL = lsSQL + "'" + DateTime.Now + "', ";
                lsSQL = lsSQL + "'" + DateTime.Now + "');";

                if (ExecuteSQL(lsSQL) == true)
                {
                    return true;
                }

                return false;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }

        /// <summary>
        /// 救急指定日データ登録
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool InsertEmergencyDate(DataRow data)
        {
            try
            {
                string lsSQL;

                lsSQL = "INSERT INTO d_emergency_date VALUES(";
                lsSQL = lsSQL + "'" + data["target_date"] + "', ";
                lsSQL = lsSQL + "'" + DateTime.Now + "', ";
                lsSQL = lsSQL + "'" + DateTime.Now + "');";

                if (ExecuteSQL(lsSQL) == true)
                {
                    return true;
                }

                return false;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }


        // --- DELETE ---

        /// <summary>
        /// 対象病棟の希望シフトを削除
        /// </summary>
        /// <returns></returns>
        public bool DeleteRequestShift_Ward(string strWard, string strTargetMonth, string strStaffKind)
        {
            try
            {
                string lsSQL;
                lsSQL = "DELETE FROM ";
                lsSQL = lsSQL + "    d_request_shift ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    ward = '" + strWard + "' AND ";
                lsSQL = lsSQL + "    staff_kind = '" + strStaffKind + "' AND ";
                lsSQL = lsSQL + "    DATE_FORMAT(target_date, '%Y%m') = '" + strTargetMonth + "';";

                if (ExecuteSQL(lsSQL) == true)
                {
                    return true;
                }

                return false;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }

        /// <summary>
        /// 常日勤データを削除
        /// </summary>
        /// <returns></returns>
        public bool DeleteStaffDayOnly_Ward(string strWard, string strStaffKind)
        {
            try
            {
                string lsSQL;
                lsSQL = "DELETE FROM ";
                lsSQL = lsSQL + "    d_staff_day_only ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    ward = '" + strWard + "' AND ";
                lsSQL = lsSQL + "    staff_kind = '" + strStaffKind + "';";

                if (ExecuteSQL(lsSQL) == true)
                {
                    return true;
                }

                return false;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }

        /// <summary>
        /// 予定職員一覧を削除
        /// </summary>
        /// <returns></returns>
        public bool DeleteScheduleStaff_Ward_TargetMonth(string strWard, string strTargetMonth, string strStaffKind)
        {
            try
            {
                string lsSQL;
                lsSQL = "DELETE FROM ";
                lsSQL = lsSQL + "    d_schedule_staff ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    ward = '" + strWard + "' AND ";
                lsSQL = lsSQL + "    target_month = '" + strTargetMonth + "' AND ";
                lsSQL = lsSQL + "    staff_kind = '" + strStaffKind + "';";

                if (ExecuteSQL(lsSQL) == true)
                {
                    return true;
                }

                return false;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }

        /// <summary>
        /// 勤務予定ヘッダを削除
        /// </summary>
        /// <returns></returns>
        public bool DeleteScheduleHeader_ScheduleNo(string strScheduleNo)
        {
            try
            {
                string lsSQL;
                lsSQL = "DELETE FROM ";
                lsSQL = lsSQL + "    d_schedule_header ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    schedule_no = '" + strScheduleNo + "';";

                if (ExecuteSQL(lsSQL) == true)
                {
                    return true;
                }

                return false;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }

        /// <summary>
        /// 勤務予定詳細を削除
        /// </summary>
        /// <returns></returns>
        public bool DeleteScheduleDetail_ScheduleNo(string strScheduleNo)
        {
            try
            {
                string lsSQL;
                lsSQL = "DELETE FROM ";
                lsSQL = lsSQL + "    d_schedule_detail ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    schedule_no = '" + strScheduleNo + "';";

                if (ExecuteSQL(lsSQL) == true)
                {
                    return true;
                }

                return false;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }

        /// <summary>
        /// 勤務初回予定ヘッダを削除
        /// </summary>
        /// <returns></returns>
        public bool DeleteScheduleFirstHeader_ScheduleNo(string strScheduleNo)
        {
            try
            {
                string lsSQL;
                lsSQL = "DELETE FROM ";
                lsSQL = lsSQL + "    d_schedule_first_header ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    schedule_no = '" + strScheduleNo + "';";

                if (ExecuteSQL(lsSQL) == true)
                {
                    return true;
                }

                return false;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }

        /// <summary>
        /// 勤務予定詳細を削除
        /// </summary>
        /// <returns></returns>
        public bool DeleteScheduleFirstDetail_ScheduleNo(string strScheduleNo)
        {
            try
            {
                string lsSQL;
                lsSQL = "DELETE FROM ";
                lsSQL = lsSQL + "    d_schedule_first_detail ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    schedule_no = '" + strScheduleNo + "';";

                if (ExecuteSQL(lsSQL) == true)
                {
                    return true;
                }

                return false;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }

        /// <summary>
        /// 勤務実績ヘッダを削除
        /// </summary>
        /// <returns></returns>
        public bool DeleteResultHeader_ResultNo(string strResultNo)
        {
            try
            {
                string lsSQL;
                lsSQL = "DELETE FROM ";
                lsSQL = lsSQL + "    d_result_header ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    result_no = '" + strResultNo + "';";

                if (ExecuteSQL(lsSQL) == true)
                {
                    return true;
                }

                return false;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }

        /// <summary>
        /// 勤務実績詳細を削除
        /// </summary>
        /// <returns></returns>
        public bool DeleteResultDetail_ResultNo(string strResultNo)
        {
            try
            {
                string lsSQL;
                lsSQL = "DELETE FROM ";
                lsSQL = lsSQL + "    d_result_detail ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    result_no = '" + strResultNo + "';";

                if (ExecuteSQL(lsSQL) == true)
                {
                    return true;
                }

                return false;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }

        /// <summary>
        /// 様式9データを削除
        /// </summary>
        /// <returns></returns>
        public bool DeleteWardYoushiki9_TargetMonth(string strTargetMonth)
        {
            try
            {
                string lsSQL;
                lsSQL = "DELETE FROM ";
                lsSQL = lsSQL + "    d_ward_youshiki9 ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    target_month = '" + strTargetMonth + "';";

                if (ExecuteSQL(lsSQL) == true)
                {
                    return true;
                }

                return false;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }

        /// <summary>
        /// 曜日別勤務人数設定を削除
        /// </summary>
        /// <returns></returns>
        public bool DeleteCountLimitDay_Ward(string strWard)
        {
            try
            {
                string lsSQL;
                lsSQL = "DELETE FROM ";
                lsSQL = lsSQL + "    d_count_limit_day ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    ward = '" + strWard + "';";

                if (ExecuteSQL(lsSQL) == true)
                {
                    return true;
                }

                return false;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }

        /// <summary>
        /// 救急指定日データを削除
        /// </summary>
        /// <returns></returns>
        public bool DeleteEmergencyDate_TargetDate(string strTargetDate)
        {
            try
            {
                string lsSQL;
                lsSQL = "DELETE FROM ";
                lsSQL = lsSQL + "    d_emergency_date ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    target_date = '" + strTargetDate + "';";

                if (ExecuteSQL(lsSQL) == true)
                {
                    return true;
                }

                return false;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }


        // --- UPDATE --

        /// <summary>
        /// 職員マスタ更新
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool UpdateStaff(DataRow data)
        {
            try
            {
                string lsSQL;

                lsSQL = "UPDATE m_staff SET ";
                lsSQL = lsSQL + "    name = '" + data["name"] + "', ";
                lsSQL = lsSQL + "    ward = '" + data["ward"] + "', ";
                lsSQL = lsSQL + "    sex = '" + data["sex"] + "', ";
                lsSQL = lsSQL + "    staff_kind = '" + data["staff_kind"] + "', ";
                lsSQL = lsSQL + "    staff_kind_sub = '" + data["staff_kind_sub"] + "', ";
                lsSQL = lsSQL + "    staff_position = '" + data["staff_position"] + "', ";
                lsSQL = lsSQL + "    seq = '" + data["seq"] + "', ";
                lsSQL = lsSQL + "    using_flag = '" + data["using_flag"] + "', ";
                lsSQL = lsSQL + "    updated_date = '" + data["updated_date"] + "' ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    id = '" + data["id"] + "';";

                if (ExecuteSQL(lsSQL) == true)
                {
                    return true;
                }

                return false;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }

        /// <summary>
        /// 職員マスタの表示順更新
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool UpdateStaff_SEQ(DataRow data)
        {
            try
            {
                string lsSQL;

                lsSQL = "UPDATE m_staff SET ";
                lsSQL = lsSQL + "    seq = '" + data["seq"] + "' ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    id = '" + data["id"] + "';";

                if (ExecuteSQL(lsSQL) == true)
                {
                    return true;
                }

                return false;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }

        /// <summary>
        /// 予定職員情報の事務的作業フラグ更新
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool UpdateScheduleStaff_OfficeFlag(DataRow data)
        {
            try
            {
                string lsSQL;

                lsSQL = "UPDATE d_schedule_staff SET ";
                lsSQL = lsSQL + "    office_flag = '" + data["office_flag"] + "' ";
                lsSQL = lsSQL + "WHERE ";
                lsSQL = lsSQL + "    staff_id = '" + data["staff_id"] + "' AND ";
                lsSQL = lsSQL + "    target_month = '" + data["target_month"] + "';";

                if (ExecuteSQL(lsSQL) == true)
                {
                    return true;
                }

                return false;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }


        // --- etc ---

        /// <summary>
        /// DataTableデータの取得
        /// </summary>
        /// <param name="lsSQL"></param>
        /// <returns></returns>
        private DataTable GetDataTable(string lsSQL)
        {
            try
            {
                MySqlConnection _con;
                MySqlDataAdapter _adp;
                DataTable dt = new DataTable();
                string lsConStr;

                lsConStr = GetConnectString_MySQL();

                _con = new MySqlConnection(lsConStr);
                _con.Open();

                _adp = new MySqlDataAdapter(lsSQL, _con);
                _adp.Fill(dt);

                // WataruT
                //OutputSQLStrings(lsSQL);

                _con.Close();

                return dt;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return null;
            }
        }

        /// <summary>
        /// SQLの実行(Insert、Update、Delete)
        /// </summary>
        /// <param name="lsSQL"></param>
        /// <returns></returns>
        private bool ExecuteSQL(string lsSQL)
        {
            try
            {
                MySqlConnection _con;
                MySqlCommand _cmd;
                string lsConStr;

                lsConStr = GetConnectString_MySQL();

                _con = new MySqlConnection(lsConStr);

                _cmd = new MySqlCommand(lsSQL, _con);
                _cmd.Connection.Open();
                _cmd.ExecuteNonQuery();
                _cmd.Connection.Close();

                // Mod Start WataruT 2020.07.14 SQLログ出力エラー
                //OutputSQLStrings(lsSQL);
                // Mod End   WataruT 2020.07.14 SQLログ出力エラー
                return true;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }

        /// <summary>
        /// SQLの実行(Bulk Insert)
        /// </summary>
        /// <param name="lsSQL"></param>
        /// <returns></returns>
        public bool ExecuteBulkInsertSQL(string lsSQL)
        {
            try
            {
                MySqlConnection _con;
                MySqlCommand _cmd;
                string lsConStr;
                string strSQL = "";

                lsConStr = GetConnectString_MySQL();

                _con = new MySqlConnection(lsConStr);

                strSQL = lsSQL.Substring(0, lsSQL.Length - 1) + ";";

                _cmd = new MySqlCommand(strSQL, _con);
                _cmd.Connection.Open();
                _cmd.ExecuteNonQuery();
                _cmd.Connection.Close();

                return true;

            }
            catch (MySqlException me)
            {
                Console.WriteLine("ERROR: " + me.Message);
                return false;
            }
        }

        /// <summary>
        /// MySQL接続用文字列の取得
        /// </summary>
        /// <returns></returns>
        private string GetConnectString_MySQL()
        {
            string _cst;

            _cst = "Server=\"172.16.0.205\";Database=\"workschedule\";Uid=\"workschedule\";Pwd=\"workschedule\"";
            //_cst = "Server=\"127.0.0.1\";Database=\"workschedule\";Uid=\"root\";Pwd=\"Ohyachi00\"";

            return _cst;
        }

        /// <summary>
        /// SQL出力(デバッグ用)
        /// </summary>
        /// <param name="strSQL"></param>
        private void OutputSQLStrings(string strSQL)
        {
            // 文字コードを指定
            Encoding enc = Encoding.GetEncoding("Shift_JIS");

            // ファイルを開く
            StreamWriter writer = new StreamWriter(@"C:\Users\alphauser\Desktop\SQL.txt", true, enc);

            // テキストを書き込む
            writer.WriteLine(DateTime.Now.ToString() + ":" + strSQL);

            // ファイルを閉じる
            writer.Close();
        }

    }
}
