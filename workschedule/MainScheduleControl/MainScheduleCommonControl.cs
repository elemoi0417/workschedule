using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using workschedule.Controls;
using workschedule.Functions;

namespace workschedule.MainScheduleControl
{
    class MainScheduleCommonControl
    {
        // 親フォーム
        MainSchedule frmMainSchedule;

        // 使用クラス宣言
        CommonControl clsCommonControl = new CommonControl();
        DatabaseControl clsDatabaseControl = new DatabaseControl();
        
        // 初回処理
        public MainScheduleCommonControl(MainSchedule frmMainSchedule_Parent)
        {
            frmMainSchedule = frmMainSchedule_Parent;
        }

        /// <summary>
        /// 初期化ルーチン処理
        /// </summary>
        public void InitialProcess(bool bFirst)
        {
            // 各種データテーブル取得
            frmMainSchedule.dtStaffKind = clsDatabaseControl.GetStaffKind();
            frmMainSchedule.dtStaffPosition = clsDatabaseControl.GetStaffPosition();
            frmMainSchedule.dtWard = clsDatabaseControl.GetWard();
            frmMainSchedule.dtWorkKind = clsDatabaseControl.GetWorkKind();
            frmMainSchedule.dtHoliday = clsDatabaseControl.GetHoliday();
            frmMainSchedule.dtDayOfWeek = clsDatabaseControl.GetDayOfWeek();

            // 初回起動時のみ処理
            if (bFirst == true)
            {
                // 病棟マスタをコンボボックスにセット
                SetWardComboBox();

                // 管理者用メニューの表示設定
                SetMasterControlVisible();

                // 現在の年月をセット
                frmMainSchedule.lblTargetMonth.Text = DateTime.Now.ToString("yyyy年MM月");
            }

            // 各種データテーブル取得_2
            frmMainSchedule.dtScheduleStaff = clsDatabaseControl.GetScheduleStaff_List(frmMainSchedule.cmbWard.SelectedValue.ToString(), 
                frmMainSchedule.lblTargetMonth.Text.Substring(0, 4) + frmMainSchedule.lblTargetMonth.Text.Substring(5, 2), frmMainSchedule.pstrStaffKind);
            //Mod Start WataruT 2020.07.30 勤務職員の変更後の常日勤設定不具合対応
            //frmMainSchedule.dtStaffDayOnly = clsDatabaseControl.GetStaffDayOnly_Ward(frmMainSchedule.cmbWard.SelectedValue.ToString(), frmMainSchedule.pstrStaffKind);
            frmMainSchedule.dtStaffDayOnly = clsDatabaseControl.GetStaffDayOnly_Ward_TargetMonth(frmMainSchedule.cmbWard.SelectedValue.ToString(), frmMainSchedule.pstrStaffKind,
                frmMainSchedule.lblTargetMonth.Text.Substring(0, 4) + frmMainSchedule.lblTargetMonth.Text.Substring(5, 2));
            //Mod End   WataruT 2020.07.30 勤務職員の変更後の常日勤設定不具合対応
            frmMainSchedule.dtCountLimitDay = clsDatabaseControl.GetCountLimitDay_Ward(frmMainSchedule.cmbWard.SelectedValue.ToString(), frmMainSchedule.pstrStaffKind);
            frmMainSchedule.dtEmergencyDate = clsDatabaseControl.GetEmergencyDate_TargetMonth(frmMainSchedule.lblTargetMonth.Text.Substring(0, 4) + "-" + frmMainSchedule.lblTargetMonth.Text.Substring(5, 2));

            // 変数の初期化
            CreateNewValiable();

            // 各種マスタをセット
            SetStaffKind();
            SetStaffPosition();
            SetWorkKind();
            SetHoliday();
            SetDayOfWeek();

            SetScheduleStaff();
            SetStaffDayOnly();
            SetCountLimitDay();
            SetHolidayCount();
            CheckScheduleFirstFlag();
        }

        /// <summary>
        /// 変数の初期設定
        /// </summary>
        private void CreateNewValiable()
        {
            int iStaffKindCount = frmMainSchedule.dtStaffKind.Rows.Count;
            int iStaffPositionCount = frmMainSchedule.dtStaffPosition.Rows.Count;
            int iHoliday = frmMainSchedule.dtHoliday.Rows.Count;
            int iDayOfWeek = frmMainSchedule.dtDayOfWeek.Rows.Count;

            frmMainSchedule.piDayCount = clsCommonControl.GetTargetMonthDays(frmMainSchedule.lblTargetMonth.Text);
            frmMainSchedule.piScheduleStaffCount = frmMainSchedule.dtScheduleStaff.Rows.Count;
            frmMainSchedule.piWorkKindCount = frmMainSchedule.dtWorkKind.Rows.Count;
            frmMainSchedule.pstrTargetMonth = frmMainSchedule.lblTargetMonth.Text.Substring(0, 4) + frmMainSchedule.lblTargetMonth.Text.Substring(5, 2);

            frmMainSchedule.aiData = new int[frmMainSchedule.piScheduleStaffCount, frmMainSchedule.piDayCount, frmMainSchedule.piWorkKindCount];
            frmMainSchedule.aiNightLastMonthFlag = new int[frmMainSchedule.piScheduleStaffCount, frmMainSchedule.piDayCount];
            frmMainSchedule.aiDataRequestFlag = new int[frmMainSchedule.piScheduleStaffCount, frmMainSchedule.piDayCount];
            frmMainSchedule.aiDataNow = new int[frmMainSchedule.piScheduleStaffCount, frmMainSchedule.piDayCount];
            frmMainSchedule.adRowTotalData = new double[frmMainSchedule.piScheduleStaffCount, 3];
            // Mod Start WataruT 2020.07.30 遅出の表示対応
            //frmMainSchedule.adColumnTotalData = new double[frmMainSchedule.piDayCount, 6];
            frmMainSchedule.adColumnTotalData = new double[frmMainSchedule.piDayCount, 7];
            // Mod End   WataruT 2020.07.30 遅出の表示対応
            frmMainSchedule.astrWorkKind = new string[frmMainSchedule.piWorkKindCount, 2];
            frmMainSchedule.astrStaffKind = new string[iStaffKindCount, 3];
            frmMainSchedule.astrStaffPosition = new string[iStaffPositionCount];
            frmMainSchedule.astrScheduleStaff = new string[frmMainSchedule.piScheduleStaffCount, 3];
            frmMainSchedule.astrStaffDayOnly = new string[frmMainSchedule.piScheduleStaffCount, 4];
            frmMainSchedule.astrHoliday = new string[iHoliday];
            frmMainSchedule.astrDayOfWeek = new string[iDayOfWeek, 2];
            frmMainSchedule.astrCountLimitDay = new string[frmMainSchedule.astrDayOfWeek.GetLength(0), 2];

            // 実績データ用
            if (frmMainSchedule.piDataKind == 3)
            {
                frmMainSchedule.astrResultOtherWorkTime = new string[frmMainSchedule.piScheduleStaffCount, frmMainSchedule.piDayCount, 3, 3];
                frmMainSchedule.astrResultWorkKind = new string[frmMainSchedule.piScheduleStaffCount, frmMainSchedule.piDayCount];
                frmMainSchedule.astrResultChangeFlag = new string[frmMainSchedule.piScheduleStaffCount, frmMainSchedule.piDayCount];
            }
        }

        /// <summary>
        /// 職種マスタをセット
        /// </summary>
        private void SetStaffKind()
        {
            for (int i = 0; i < frmMainSchedule.dtStaffKind.Rows.Count; i++)
            {
                frmMainSchedule.astrStaffKind[i, 0] = frmMainSchedule.dtStaffKind.Rows[i]["id"].ToString();
                frmMainSchedule.astrStaffKind[i, 1] = frmMainSchedule.dtStaffKind.Rows[i]["sub_id"].ToString();
                frmMainSchedule.astrStaffKind[i, 2] = frmMainSchedule.dtStaffKind.Rows[i]["name"].ToString();
            }
        }

        /// <summary>
        /// 役職マスタをセット
        /// </summary>
        private void SetStaffPosition()
        {
            for (int i = 0; i < frmMainSchedule.dtStaffPosition.Rows.Count; i++)
            {
                frmMainSchedule.astrStaffPosition[i] = frmMainSchedule.dtStaffPosition.Rows[i]["name"].ToString();
            }
        }

        /// <summary>
        /// 勤務種類をセット
        /// </summary>
        private void SetWorkKind()
        {
            for (int i = 0; i < frmMainSchedule.dtWorkKind.Rows.Count; i++)
            {
                frmMainSchedule.astrWorkKind[i, 0] = frmMainSchedule.dtWorkKind.Rows[i]["id"].ToString();
                frmMainSchedule.astrWorkKind[i, 1] = frmMainSchedule.dtWorkKind.Rows[i]["name_short"].ToString();
            }
        }

        /// <summary>
        /// 祝日マスタをセット
        /// </summary>
        private void SetHoliday()
        {
            for (int i = 0; i < frmMainSchedule.dtHoliday.Rows.Count; i++)
            {
                frmMainSchedule.astrHoliday[i] = frmMainSchedule.dtHoliday.Rows[i]["holiday"].ToString();
            }
        }

        /// <summary>
        /// 曜日マスタをセット
        /// </summary>
        private void SetDayOfWeek()
        {
            for (int i = 0; i < frmMainSchedule.dtDayOfWeek.Rows.Count; i++)
            {
                frmMainSchedule.astrDayOfWeek[i, 0] = frmMainSchedule.dtDayOfWeek.Rows[i]["id"].ToString();
                frmMainSchedule.astrDayOfWeek[i, 1] = frmMainSchedule.dtDayOfWeek.Rows[i]["day_of_week"].ToString();
            }
        }

        /// <summary>
        /// 職員マスタをセット
        /// </summary>
        private void SetScheduleStaff()
        {
            for (int i = 0; i < frmMainSchedule.dtScheduleStaff.Rows.Count; i++)
            {
                frmMainSchedule.astrScheduleStaff[i, 0] = frmMainSchedule.dtScheduleStaff.Rows[i]["id"].ToString();
                frmMainSchedule.astrScheduleStaff[i, 1] = frmMainSchedule.dtScheduleStaff.Rows[i]["name"].ToString();
                frmMainSchedule.astrScheduleStaff[i, 2] = frmMainSchedule.dtScheduleStaff.Rows[i]["sex"].ToString();
            }
        }

        /// <summary>
        /// 常日勤データをセット
        /// </summary>
        private void SetStaffDayOnly()
        {
            for(int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.dtScheduleStaff.Rows.Count; iScheduleStaff++)
            {
                for (int i = 0; i < frmMainSchedule.dtStaffDayOnly.Rows.Count; i++)
                {
                    if(frmMainSchedule.astrScheduleStaff[iScheduleStaff, 0] == frmMainSchedule.dtStaffDayOnly.Rows[i]["staff"].ToString())
                    {
                        frmMainSchedule.astrStaffDayOnly[iScheduleStaff, 0] = frmMainSchedule.dtStaffDayOnly.Rows[i]["staff"].ToString();
                        if (frmMainSchedule.dtStaffDayOnly.Rows[i]["target_day_start"].ToString() == "")
                        {
                            frmMainSchedule.astrStaffDayOnly[iScheduleStaff, 1] = "";
                        }
                        else
                        {
                            frmMainSchedule.astrStaffDayOnly[iScheduleStaff, 1] = frmMainSchedule.dtStaffDayOnly.Rows[i]["target_day_start"].ToString().Substring(0, 10);
                        }
                        if (frmMainSchedule.dtStaffDayOnly.Rows[i]["target_day_end"].ToString() == "")
                        {
                            frmMainSchedule.astrStaffDayOnly[iScheduleStaff, 2] = "";
                        }
                        else
                        {
                            frmMainSchedule.astrStaffDayOnly[iScheduleStaff, 2] = frmMainSchedule.dtStaffDayOnly.Rows[i]["target_day_end"].ToString().Substring(0, 10);
                        }
                        frmMainSchedule.astrStaffDayOnly[iScheduleStaff, 3] = frmMainSchedule.dtStaffDayOnly.Rows[i]["holiday_flag"].ToString();
                    }
                }
            }
        }

        /// <summary>
        /// 曜日ごとの制限値データをセット
        /// </summary>
        private void SetCountLimitDay()
        {
            int iCount = 0;
            for (int i = 0; i < frmMainSchedule.astrDayOfWeek.GetLength(0); i++)
            {
                frmMainSchedule.astrCountLimitDay[i, 0] = frmMainSchedule.dtCountLimitDay.Rows[iCount]["day_min"].ToString();
                frmMainSchedule.astrCountLimitDay[i, 1] = frmMainSchedule.dtCountLimitDay.Rows[iCount]["night_max"].ToString();
                iCount++;
            }
        }

        /// <summary>
        /// 病棟マスタをコンボボックスにセット
        /// </summary>
        public void SetWardComboBox()
        {
            List<ItemSet> srcCourse = new List<ItemSet>();

            frmMainSchedule.cmbWard.DataSource = null;

            // マスタコードの場合のみ全て表示
            if(frmMainSchedule.pstrLoginWard == "00")
            {
                foreach (DataRow row in frmMainSchedule.dtWard.Rows)
                {
                    srcCourse.Add(new ItemSet(row["name"].ToString(), row["id"].ToString()));
                }
            }
            else
            {
                foreach (DataRow row in frmMainSchedule.dtWard.Rows)
                {   
                    if(row["id"].ToString() == frmMainSchedule.pstrLoginWard)
                        srcCourse.Add(new ItemSet(row["name"].ToString(), row["id"].ToString()));
                }
            }
            frmMainSchedule.cmbWard.DataSource = srcCourse;
            frmMainSchedule.cmbWard.DisplayMember = "ItemDisp";
            frmMainSchedule.cmbWard.ValueMember = "ItemValue";
            frmMainSchedule.cmbWard.SelectedIndex = 0;
        }

        /// <summary>
        /// 管理者用メニューの表示設定
        /// </summary>
        public void SetMasterControlVisible()
        {
            if(frmMainSchedule.pstrLoginWard != "00")
            {
                frmMainSchedule.btnMaster.Visible = false;
            }
        }

        /// <summary>
        /// 各種グリッドの位置・サイズを設定
        /// </summary>
        public void SetGridPositionAndSize()
        {
            switch (frmMainSchedule.piDataKind)
            {
                case 1: // 希望
                    frmMainSchedule.grdMainHeader.Location = new Point(0, 62);
                    frmMainSchedule.grdMainHeader.Size = new Size(1215, 44);
                    frmMainSchedule.grdMain.Location = new Point(0, 106);
                    frmMainSchedule.grdMain.Size = new Size(1215, 517);
                    frmMainSchedule.grdRowTotalHeader.Location = new Point(1215, 62);
                    frmMainSchedule.grdRowTotalHeader.Size = new Size(93, 44);
                    frmMainSchedule.grdRowTotal.Location = new Point(1215, 106);
                    frmMainSchedule.grdRowTotal.Size = new Size(93, 517);
                    frmMainSchedule.grdColumnTotal.Location = new Point(0, 623);
                    frmMainSchedule.grdColumnTotal.Size = new Size(1215, 65);
                    break;
                case 2: // 予定
                    frmMainSchedule.grdMainHeader.Location = new Point(0, 62);
                    // Mod Start WataruT 2020.08.13 予定画面の合計値が省略されてしまう
                    //frmMainSchedule.grdMainHeader.Size = new Size(1227, 44);
                    frmMainSchedule.grdMainHeader.Size = new Size(1191, 44);
                    // Mod End   WataruT 2020.08.13 予定画面の合計値が省略されてしまう
                    frmMainSchedule.grdMain.Location = new Point(0, 106);
                    // Mod Start WataruT 2020.08.13 予定画面の合計値が省略されてしまう
                    //// Mod Start WataruT 2020.07.30 遅出の表示対応
                    ////frmMainSchedule.grdMain.Size = new Size(1227, 451);
                    //frmMainSchedule.grdMain.Size = new Size(1227, 434);
                    //// Mod End   WataruT 2020.07.30 遅出の表示対応
                    //frmMainSchedule.grdRowTotalHeader.Location = new Point(1227, 62);
                    //frmMainSchedule.grdRowTotalHeader.Size = new Size(81, 44);
                    //frmMainSchedule.grdRowTotal.Location = new Point(1227, 106);
                    //// Mod Start WataruT 2020.07.30 遅出の表示対応
                    ////frmMainSchedule.grdRowTotal.Size = new Size(81, 451);
                    ////frmMainSchedule.grdColumnTotal.Location = new Point(0, 557);
                    ////frmMainSchedule.grdColumnTotal.Size = new Size(1227, 132);
                    //frmMainSchedule.grdRowTotal.Size = new Size(81, 434);
                    //// Mod End   WataruT 2020.07.30 遅出の表示対応
                    frmMainSchedule.grdMain.Size = new Size(1191, 434);
                    frmMainSchedule.grdRowTotalHeader.Location = new Point(1209, 62);
                    frmMainSchedule.grdRowTotalHeader.Size = new Size(102, 44);
                    frmMainSchedule.grdRowTotal.Location = new Point(1209, 106);
                    frmMainSchedule.grdRowTotal.Size = new Size(102, 417);
                    // Mod End   WataruT 2020.08.13 予定画面の合計値が省略されてしまう
                    frmMainSchedule.grdColumnTotal.Location = new Point(0, 540);
                    // Mod Start WataruT 2020.08.13 予定画面の合計値が省略されてしまう
                    //frmMainSchedule.grdColumnTotal.Size = new Size(1227, 148);
                    frmMainSchedule.grdColumnTotal.Size = new Size(1191, 148);
                    // Mod End   WataruT 2020.08.13 予定画面の合計値が省略されてしまう
                    break;
                case 3: // 実績
                    frmMainSchedule.grdMainHeader.Location = new Point(0, 62);
                    frmMainSchedule.grdMainHeader.Size = new Size(1229, 44);
                    frmMainSchedule.grdMain.Location = new Point(0, 106);
                    frmMainSchedule.grdMain.Size = new Size(1246, 517);
                    frmMainSchedule.grdRowTotalHeader.Location = new Point(1246, 62);
                    frmMainSchedule.grdRowTotalHeader.Size = new Size(62, 44);
                    frmMainSchedule.grdRowTotal.Location = new Point(1246, 106);
                    frmMainSchedule.grdRowTotal.Size = new Size(62, 487);
                    frmMainSchedule.grdColumnTotal.Location = new Point(0, 623);
                    frmMainSchedule.grdColumnTotal.Size = new Size(1229, 65);
                    break;
            }
        }

        /// <summary>
        /// 各種グリッドの表示位置を統一
        /// </summary>
        public void SetGridPosition()
        {
            frmMainSchedule.grdRowTotal.FirstDisplayedScrollingRowIndex = frmMainSchedule.grdMain.FirstDisplayedScrollingRowIndex;

            // Mod Start WataruT 2020.07.14 計画表の縦スクロール時エラー
            //frmMainSchedule.grdMainHeader.FirstDisplayedScrollingColumnIndex = frmMainSchedule.grdMain.FirstDisplayedScrollingColumnIndex;
            //frmMainSchedule.grdColumnTotal.FirstDisplayedScrollingColumnIndex = frmMainSchedule.grdMain.FirstDisplayedScrollingColumnIndex;
            if (frmMainSchedule.grdMain.FirstDisplayedScrollingColumnIndex != 0)
            {
                frmMainSchedule.grdMainHeader.FirstDisplayedScrollingColumnIndex = frmMainSchedule.grdMain.FirstDisplayedScrollingColumnIndex;
                frmMainSchedule.grdColumnTotal.FirstDisplayedScrollingColumnIndex = frmMainSchedule.grdMain.FirstDisplayedScrollingColumnIndex;
            }
            // Mod End   WataruT 2020.07.14 計画表の縦スクロール時エラー
        }

        /// <summary>
        /// 常日勤マスタが職員の数だけ登録されているかチェック
        /// </summary>
        public bool CheckStaffDayOnly()
        {
            if (frmMainSchedule.dtScheduleStaff.Rows.Count == frmMainSchedule.dtStaffDayOnly.Rows.Count)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 祝日数を考慮した公休数を共通変数にセット
        /// </summary>
        public void SetHolidayCount()
        {
            string strDayOfWeek;
            // Add Start WataruT 2020.07.14 土日祝の公休数設定変更
            int iSaturdayCount;
            int iSundayCount;
            bool bSaturdayHolidayFlag;
            string strFormat = "yyyyMMdd";
            DateTime dtTargetDate;
            // Add End   WataruT 2020.07.14 土日祝の公休数設定変更

            // 既定の公休数を初期化
            // Mod Start WataruT 2020.07.14 土日祝の公休数設定変更
            //frmMainSchedule.pdHolidayCount = 0;
            frmMainSchedule.pdHolidayCount = 8;
            iSaturdayCount = 0;
            iSundayCount = 0;
            bSaturdayHolidayFlag = true;
            // Mod End   WataruT 2020.07.14 土日祝の公休数設定変更

            // Mod Start WataruT 2020.07.14 土日祝の公休数設定変更
            //// 日曜日の数をカウント
            //for (int iDay = 0; iDay < frmMainSchedule.piDayCount; iDay++)
            //{
            //    strDayOfWeek = clsCommonControl.GetWeekName(frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", iDay + 1), frmMainSchedule.astrHoliday);

            //    switch(strDayOfWeek)
            //    {
            //        case "土":
            //        case "日":
            //        case "祝":
            //            frmMainSchedule.pdHolidayCount++;
            //            break;
            //    }
            //}
            // 祝日マスタ関係なく土日の数をカウント
            for (int iDay = 0; iDay < frmMainSchedule.piDayCount; iDay++)
            {
                dtTargetDate = DateTime.ParseExact(frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", iDay + 1), strFormat, null);

                switch (dtTargetDate.ToString("ddd"))
                {
                    case "土":
                        iSaturdayCount++;
                        if (iSaturdayCount > 4) frmMainSchedule.pdHolidayCount = frmMainSchedule.pdHolidayCount + 0.5;
                        {
                            bSaturdayHolidayFlag = false;
                            break;
                        }
                    case "日":
                        iSundayCount++;
                        if (iSundayCount > 4) frmMainSchedule.pdHolidayCount++;
                        break;
                }
            }
            // 祝日判定
            for (int iDay = 0; iDay < frmMainSchedule.piDayCount; iDay++)
            {
                strDayOfWeek = clsCommonControl.GetWeekName(frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", iDay + 1), frmMainSchedule.astrHoliday);

                switch (strDayOfWeek)
                {
                    case "祝":
                        if(DateTime.ParseExact(frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", iDay + 1), strFormat, null).ToString("ddd") == "日")
                        {
                            break;
                        }
                        if (DateTime.ParseExact(frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", iDay + 1), strFormat, null).ToString("ddd") == "土")
                        {
                            if (bSaturdayHolidayFlag == false)
                            {
                                frmMainSchedule.pdHolidayCount = frmMainSchedule.pdHolidayCount + 0.5;
                                bSaturdayHolidayFlag = true;
                            }   
                            break;
                        }else
                        {
                            frmMainSchedule.pdHolidayCount++;
                            break;
                        }
                }
            }
            // Mod End   WataruT 2020.07.14 土日祝の公休数設定変更
        }

        /// <summary>
        /// 勤務予定初回データチェック
        /// </summary>
        public void CheckScheduleFirstFlag()
        {
            // 初回予定フラグをセット
            frmMainSchedule.pbScheduleFirstFlag = clsDatabaseControl.GetScheduleFirstHeader_FirstFlag(frmMainSchedule.cmbWard.SelectedValue.ToString(),
                            frmMainSchedule.pstrTargetMonth, frmMainSchedule.pstrStaffKind);

            // 初回予定フラグにより、「初回登録」ボタンの有効性を切り替える
            if (frmMainSchedule.pbScheduleFirstFlag)
                // Mod Start WataruT 2020.07.21 初回登録解除機能追加
                //frmMainSchedule.btnSaveFirst.Enabled = false;
                if (frmMainSchedule.pstrLoginWard == "00")
                {
                    frmMainSchedule.btnSaveFirst.Text = "登録\r\n解除";
                    frmMainSchedule.btnSaveFirst.Enabled = true;
                }
                else
                {
                    frmMainSchedule.btnSaveFirst.Enabled = false;
                }
            // Mod End   WataruT 2020.07.21 初回登録解除機能追加
            else
            // Mod Start WataruT 2020.07.21 初回登録解除機能追加
            //frmMainSchedule.btnSaveFirst.Enabled = true;
            {
                frmMainSchedule.btnSaveFirst.Text = "初回\r\n登録";
                frmMainSchedule.btnSaveFirst.Enabled = true;
            }
            // Mod End   WataruT 2020.07.21 初回登録解除機能追加

        }

        /// <summary>
        /// 連勤情報を確認して各種プロパティを変更する
        /// </summary>
        /// <returns></returns>
        public void ChangeContinueWork()
        {
            int iContinueStartDay;
            int iContinueEndDay;
            int iTargetWorkKind = 0;

            // 連勤チェック
            for(int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.astrScheduleStaff.GetLength(0); iScheduleStaff++)
            {
                // 開始日を初期化
                iContinueStartDay = 0;

                for (int iDay = 0; iDay < frmMainSchedule.piDayCount; iDay++)
                {
                    for (int iWorkKind = 0; iWorkKind < frmMainSchedule.astrWorkKind.GetLength(0); iWorkKind++)
                    {
                        // 対象日の勤務種類を取得
                        if (frmMainSchedule.aiData[iScheduleStaff, iDay, iWorkKind] == 1)
                        {
                            iTargetWorkKind = iWorkKind;
                            break;
                        }
                    }
                    
                    // 連勤対象となる勤務種類か判定
                    switch(iTargetWorkKind.ToString())
                    {
                        // 以下の勤務種類は連勤対象とする
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
                            break;
                        default:
                            // 終了日をセット
                            iContinueEndDay = iDay - 1;

                            // 6連勤以上であれば背景色を変更する
                            if(iContinueEndDay - iContinueStartDay >= 5)
                            {
                                for(int iChangeDay = iContinueStartDay; iChangeDay <= iContinueEndDay; iChangeDay++)
                                {
                                    frmMainSchedule.grdMain[iChangeDay + 1, iScheduleStaff].Style.BackColor = Color.Red;
                                }
                            }

                            // 開始日を初期化
                            iContinueStartDay = iDay + 1;

                            break;
                    }
                }
            }

            frmMainSchedule.grdMain.Refresh();

            System.Threading.Thread.Sleep(2000);

            // デザイン設定(列：日付)
            for (int i = 1; i <= frmMainSchedule.piDayCount; i++)
            {
                // 文字色、背景色
                for (int i2 = 0; i2 < frmMainSchedule.piScheduleStaffCount; i2++)
                {
                    frmMainSchedule.grdMain[i, i2].Style.ForeColor = clsCommonControl.GetWorkKindForeColor(frmMainSchedule.grdMain[i, i2].Value.ToString());
                    frmMainSchedule.grdMain[i, i2].Style.BackColor = clsCommonControl.GetWeekNameBackgroundColor(
                        clsCommonControl.GetWeekName(frmMainSchedule.pstrTargetMonth + String.Format("{0:D2}", i), frmMainSchedule.astrHoliday));
                }
            }

            // 希望シフトのセルのみ太文字にして背景色も変更する
            for (int iDay = 0; iDay < frmMainSchedule.grdMain.ColumnCount - 1; iDay++)
            {
                for (int iScheduleStaff = 0; iScheduleStaff < frmMainSchedule.grdMain.RowCount; iScheduleStaff++)
                {
                    if (frmMainSchedule.aiDataRequestFlag[iScheduleStaff, iDay] == 1)
                    {
                        frmMainSchedule.grdMain[iDay + 1, iScheduleStaff].Style.Font = new Font(frmMainSchedule.grdMain.Font, FontStyle.Bold);
                        frmMainSchedule.grdMain[iDay + 1, iScheduleStaff].Style.BackColor = Color.Gold;
                    }
                }
            }
        }

        /// <summary>
        /// 待機時のプログレスダイアログ表示
        /// </summary>
        /// <param name="bFlag"></param>
        public void ShowProgressDialog(bool bFlag)
        {
            if(bFlag == true)
            {
                frmMainSchedule.txtProgressMessage.Visible = true;
                frmMainSchedule.txtProgressMessage.Refresh();
            }
            else
            {
                frmMainSchedule.txtProgressMessage.Visible = false;
                
            }
        }

    }
}
