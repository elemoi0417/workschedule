using System.Data;

namespace workschedule.Functions
{
    class DataTableControl
    {
        /// <summary>
        /// テーブル作成(勤務予定ヘッダ)
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable_ScheduleHeader()
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("schedule_no");
            dataTable.Columns.Add("ward");
            dataTable.Columns.Add("staff_kind");
            dataTable.Columns.Add("target_month");

            return dataTable;
        }

        /// <summary>
        /// テーブル作成(勤務予定詳細)
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable_ScheduleDetail()
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("schedule_no");
            dataTable.Columns.Add("schedule_detail_no");
            dataTable.Columns.Add("staff");
            dataTable.Columns.Add("target_date");
            dataTable.Columns.Add("work_kind");
            dataTable.Columns.Add("request_flag");

            return dataTable;
        }

        /// <summary>
        /// テーブル作成(勤務実績ヘッダ)
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable_ResultHeader()
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("result_no");
            dataTable.Columns.Add("ward");
            dataTable.Columns.Add("staff_kind");
            dataTable.Columns.Add("target_month");

            return dataTable;
        }

        /// <summary>
        /// テーブル作成(勤務実績詳細)
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable_ResultDetail()
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("result_no");
            dataTable.Columns.Add("result_detail_no");
            dataTable.Columns.Add("staff");
            dataTable.Columns.Add("target_date");
            dataTable.Columns.Add("work_kind");
            dataTable.Columns.Add("work_time_day");
            dataTable.Columns.Add("work_time_night");
            dataTable.Columns.Add("work_time_night_total");
            dataTable.Columns.Add("change_flag");
            dataTable.Columns.Add("other1_work_kind");
            dataTable.Columns.Add("other1_start_time");
            dataTable.Columns.Add("other1_end_time");
            dataTable.Columns.Add("other2_work_kind");
            dataTable.Columns.Add("other2_start_time");
            dataTable.Columns.Add("other2_end_time");
            dataTable.Columns.Add("other3_work_kind");
            dataTable.Columns.Add("other3_start_time");
            dataTable.Columns.Add("other3_end_time");

            return dataTable;
        }

        /// <summary>
        /// テーブル作成(職員マスタ)
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable_Staff()
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("id");
            dataTable.Columns.Add("name");
            dataTable.Columns.Add("ward");
            dataTable.Columns.Add("sex");
            dataTable.Columns.Add("staff_kind");
            dataTable.Columns.Add("staff_kind_sub");
            dataTable.Columns.Add("staff_position");
            dataTable.Columns.Add("seq");
            dataTable.Columns.Add("using_flag");
            dataTable.Columns.Add("created_date");
            dataTable.Columns.Add("updated_date");

            return dataTable;
        }

        /// <summary>
        /// テーブル作成(予定職員一覧)
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable_ScheduleStaff()
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("ward");
            dataTable.Columns.Add("target_month");
            dataTable.Columns.Add("staff_kind");
            dataTable.Columns.Add("staff_id");
            dataTable.Columns.Add("seq");
            dataTable.Columns.Add("office_flag");

            return dataTable;
        }

        /// <summary>
        /// テーブル作成(希望シフト)
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable_RequestShift()
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("staff");
            dataTable.Columns.Add("ward");
            dataTable.Columns.Add("target_date");
            dataTable.Columns.Add("staff_kind");
            dataTable.Columns.Add("work_kind");

            return dataTable;
        }

        /// <summary>
        /// テーブル作成(常日勤データ)
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable_StaffDayOnly()
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("staff");
            dataTable.Columns.Add("staff_kind");
            dataTable.Columns.Add("ward");
            dataTable.Columns.Add("target_day_start");
            dataTable.Columns.Add("target_day_end");
            dataTable.Columns.Add("holiday_flag");
            dataTable.Columns.Add("office_flag");
            dataTable.Columns.Add("staff_level");

            return dataTable;
        }

        /// <summary>
        /// テーブル作成(曜日別制限値設定)
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable_CountLimitDay()
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("ward");
            dataTable.Columns.Add("staff_kind");
            dataTable.Columns.Add("day_of_week");
            dataTable.Columns.Add("day_min");
            dataTable.Columns.Add("night_max");

            return dataTable;
        }

        /// <summary>
        /// テーブル作成(様式9データ)
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable_WardYoushiki9()
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("ward");
            dataTable.Columns.Add("target_month");
            dataTable.Columns.Add("kubun");
            dataTable.Columns.Add("nurse_count");
            dataTable.Columns.Add("care_count");
            dataTable.Columns.Add("ward_count");
            dataTable.Columns.Add("bed_count");
            dataTable.Columns.Add("average_day");
            dataTable.Columns.Add("nurse_percentage1");
            dataTable.Columns.Add("nurse_percentage2");
            dataTable.Columns.Add("average_year");

            return dataTable;
        }

        /// <summary>
        /// テーブル作成(救急指定日データ)
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable_EmergencyDate()
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("target_date");

            return dataTable;
        }
    }
}
