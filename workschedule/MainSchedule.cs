using System;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using workschedule.Functions;
using workschedule.MainScheduleControl;
using workschedule.Reports;

namespace workschedule
{
    public partial class MainSchedule : Form
    {
        /// <summary>
        /// テスト用変数の準備
        /// </summary>
        public int WORK_KIND_DAY_ARRAY_NUM = 0;             // 日勤の配列番号
        public int WORK_KIND_NIGHT_ARRAY_NUM = 1;           // 夜勤の配列番号
        public int WORK_KIND_NIGHT_AFTER_ARRAY_NUM = 2;     // 夜明の配列番号
        public int WORK_KIND_PUBLIC_HOLIDAY_ARRAY_NUM = 3;  // 公休の配列番号

        public bool pbActivateFlag = false;                 // 初回実行フラグ
        public bool pbCheckContinueWorkFlag = false;        // 連勤チェック表示フラグ
        public bool pbScheduleFirstFlag = false;            // 初回予定情報フラグ
        public int piRandomCount = 1000;                    // ランダム回数
        public int piDataKind = 2;                          // 現在のデータグリッドの種類(1：希望、2：予定、3：実績)
        public double pdHolidayCount = 8;                   // 対象月の休暇日数
        public string pstrStaffKind = "01";                 // 現在表示している職種(01: 看護師、02: ケア)
        public string strWorkKind_Night = "02";             // 勤務種類(夜勤)
        public string pstrTargetMonth;                      // 対象年月
        public string pstrLoginWard;                        // ログイン職員の対象病棟コード

        public int piScheduleStaffCount;                    // 対象年月、対象病棟の職員数
        public int piDayCount;                              // 対象年月の日数
        public int piWorkKindCount;                         // 勤務種類数

        public int piGrdMain_CurrentColumn;                 // メイングリッドの現在の列(右クリックメニュー用)
        public int piGrdMain_CurrentRow;                    // メイングリッドの現在の行(右クリックメニュー用)

        public DataTable dtStaffKind;                       // 職種マスタデーブル
        public DataTable dtStaffPosition;                   // 役職マスタデーブル
        public DataTable dtWorkKind;                        // 勤務種類マスタテーブル
        public DataTable dtHoliday;                         // 祝日マスタデーブル
        public DataTable dtWard;                            // 病棟マスタデーブル
        public DataTable dtDayOfWeek;                       // 曜日マスタデーブル
        public DataTable dtStaffDayOnly;                    // 常日勤データテーブル
        public DataTable dtCountLimitDay;                   // 対象日の制限値マスタデーブル
        public DataTable dtScheduleStaff;                   // 予定職員テーブル
        public DataTable dtEmergencyDate;                   // 救急指定日テーブル

        public string[,] astrScheduleStaff;                 // 職員マスタ配列(人数、ID・氏名)
        public string[,] astrStaffKind;                     // 職種マスタ配列(ID、SubID、職種名称)
        public string[] astrStaffPosition;                  // 役職マスタ配列
        public string[,] astrWorkKind;                      // 勤務種類マスタ配列(ID、名称)
        public string[] astrHoliday;                        // 祝日マスタ配列
        public string[,] astrDayOfWeek;                     // 曜日マスタ配列(ID、曜日名)
        public string[,] astrStaffDayOnly;                  // 常日勤マスタ配列(人数、ID・開始日・終了日)
        public string[,] astrCountLimitDay;                 // 対象日の制限値マスタ配列(曜日、日勤(最小)・夜勤(最大))

        public int[,,] aiData;                              // 編集用グリッドデータ配列(職員、日にち、勤務種類）
        public int[,] aiNightLastMonthFlag;                 // 前月の夜勤フラグ(職員、日にち)
        public int[,] aiDataRequestFlag;                    // 編集用グリッドデータ配列_希望(職員、日にち）
        public int[,] aiDataNow;                            // 職員別の各日付の現時点の勤務種類データ(職員、日にち）
        public double[,] adRowTotalData;                    // 職員ごとのシフト合計データ配列(職員、勤務種類)
        public double[,] adColumnTotalData;                 // 日にちごとのシフト合計データ配列(日にち、勤務種類)

        public string[,,,] astrResultOtherWorkTime;         // 実績データのその他業務情報(職員、日にち、種類番号、業務名・開始時刻・終了時刻)
        public string[,] astrResultWorkKind;                // 実績データの勤務種類ID(職員、日にち）
        public string[,] astrResultChangeFlag;              // 実績データの手入力フラグ(職員、日にち）

        // 使用クラス宣言
        CommonControl clsCommonControl = new CommonControl();
        
        MainScheduleCommonControl clsMainScheduleCommonControl;
        MainScheduleRequestControl clsMainScheduleRequestControl;
        MainScheduleScheduleControl clsMainScheduleScheduleControl;
        MainScheduleResultControl clsMainScheduleResultControl;

        public MainSchedule(string strWard)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            
            // グリッド描画の高速化(ダブルバッファリングを有効化)
            grdMain.GetType().InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, grdMain, new object[] { true });
            grdMainHeader.GetType().InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, grdMainHeader, new object[] { true });
            grdRowTotal.GetType().InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, grdRowTotal, new object[] { true });
            grdRowTotalHeader.GetType().InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, grdRowTotalHeader, new object[] { true });
            grdColumnTotal.GetType().InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, grdColumnTotal, new object[] { true });

            clsMainScheduleCommonControl = new MainScheduleCommonControl(this);
            clsMainScheduleRequestControl = new MainScheduleRequestControl(this);
            clsMainScheduleScheduleControl = new MainScheduleScheduleControl(this);
            clsMainScheduleResultControl = new MainScheduleResultControl(this);

            // ログイン職員の病棟コードをセット
            pstrLoginWard = strWard;
        }
            
        // --- ボタンイベント ---

        /// <summary>
        /// 「自動作成」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAutoCreate_Click(object sender, EventArgs e)
        {
            // 職員登録チェック
            if(dtScheduleStaff.Rows.Count == 0)
            {
                MessageBox.Show("職員登録をおこなってください。", "");
                return;
            }

            // 常日勤チェック
            if(clsMainScheduleCommonControl.CheckStaffDayOnly() == false)
            {
                MessageBox.Show("常日勤設定のない職員がいます。", "");
                return;
            }

            // プログレスウィンドウの表示
            clsMainScheduleCommonControl.ShowProgressDialog(true);

            // シフトの自動作成
            clsMainScheduleScheduleControl.CreateShiftData();

            // プログレスウィンドウを閉じる
            clsMainScheduleCommonControl.ShowProgressDialog(false);

        }

        /// <summary>
        /// 「詳細設定」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfig_Click(object sender, EventArgs e)
        {
            ScheduleConfigMenu frmScheduleConfigMenu = new ScheduleConfigMenu(cmbWard.SelectedValue.ToString(), cmbWard.Text.ToString(),
                lblTargetMonth.Text, pstrStaffKind, clsCommonControl.GetStaffKindName(pstrStaffKind, astrStaffKind));

            this.Hide();
            frmScheduleConfigMenu.ShowDialog();
            this.Show();

            // グリッドにデータをセット
            SetMainGridData();
        }

        /// <summary>
        /// 「職種切替」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChangeStaffKind_Click(object sender, EventArgs e)
        {
            // 職種コードを変更
            if(pstrStaffKind == "01")
            {
                pstrStaffKind = "02";
            }else
            {
                pstrStaffKind = "01";
            }

            // グリッドにデータをセット
            SetMainGridData();
        }

        /// <summary>
        /// 「連勤チェック」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCheckContinueWork_Click(object sender, EventArgs e)
        {
            clsMainScheduleCommonControl.ChangeContinueWork();
        }

        /// <summary>
        /// 「初回登録」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveFirst_Click(object sender, EventArgs e)
        {
            // Add Start WataruT 2020.07.21 初回登録解除機能追加
            if (btnSaveFirst.Text == "登録\r\n解除")
            {
                if(MessageBox.Show("初回登録を解除しますがよろしいですか？", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    // 初回予定データを削除
                    clsMainScheduleScheduleControl.DeleteScheduleFirstData();

                    // 完了メッセージの表示
                    MessageBox.Show("解除完了", "");

                    // 初回登録ボタンの状態変更
                    clsMainScheduleCommonControl.CheckScheduleFirstFlag();
                }
                return;
            }
            // Add End   WataruT 2020.07.21 初回登録解除機能追加

            // 確認メッセージ
            if (MessageBox.Show("初回データとして登録しますがよろしいですか？","",MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // プログレスウィンドウの表示
                clsMainScheduleCommonControl.ShowProgressDialog(true);

                // 初回予定データの登録
                clsMainScheduleScheduleControl.SaveScheduleFirstData();

                // 予定データの削除
                clsMainScheduleScheduleControl.DeleteScheduleData();

                // 予定データの作成
                clsMainScheduleScheduleControl.SaveScheduleData();

                MessageBox.Show("保存完了", "");

                // チェック機能を使用して「初回登録」ボタンを無効とする
                clsMainScheduleCommonControl.CheckScheduleFirstFlag();

                // プログレスウィンドウを閉じる
                clsMainScheduleCommonControl.ShowProgressDialog(false);
            }
        }

        /// <summary>
        /// 「保存」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // プログレスウィンドウの表示
            clsMainScheduleCommonControl.ShowProgressDialog(true);

            switch (piDataKind)
            {
                case 1:     // 希望
                    // 希望シフトの削除
                    clsMainScheduleRequestControl.DeleteRequestData();

                    // 希望シフトの作成
                    clsMainScheduleRequestControl.SaveRequestData();
                    
                    MessageBox.Show("保存完了", "");
                    break;
                case 2:     // 予定
                    // 予定データの削除
                    clsMainScheduleScheduleControl.DeleteScheduleData();

                    // 予定データの作成
                    clsMainScheduleScheduleControl.SaveScheduleData();

                    MessageBox.Show("保存完了", "");
                    break;
                case 3:     // 実績
                    // 実績データの削除
                    clsMainScheduleResultControl.DeleteResultData();

                    // 実績データの作成
                    clsMainScheduleResultControl.SaveResultData();

                    MessageBox.Show("保存完了", "");
                    break;
            }

            // プログレスウィンドウを閉じる
            clsMainScheduleCommonControl.ShowProgressDialog(false);
        }

        /// <summary>
        /// 「終了」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            // 画面を閉じる
            Close();
        }

        /// <summary>
        /// 「←」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBeforeMonth_Click(object sender, EventArgs e)
        {
            DateTime dt;

            // 現在の年月をDateTimeにセット
            dt = DateTime.ParseExact(pstrTargetMonth + "01", "yyyyMMdd", null);

            // 前月にする
            dt = dt.AddMonths(-1);

            // 各変数に値をセット
            pstrTargetMonth = dt.ToString("yyyyMM");
            lblTargetMonth.Text = dt.ToString("yyyy年MM月");

            // グリッドにデータをセット
            SetMainGridData();

            //Add Start WataruT 2020.07.21 初回登録解除機能追加
            // 初回登録ボタンの制御
            clsMainScheduleCommonControl.CheckScheduleFirstFlag();
            //Add End   WataruT 2020.07.21 初回登録解除機能追加
        }

        /// <summary>
        /// 「→」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNextMonth_Click(object sender, EventArgs e)
        {
            DateTime dt;

            // 現在の年月をDateTimeにセット
            dt = DateTime.ParseExact(pstrTargetMonth + "01", "yyyyMMdd", null);

            // 前月にする
            dt = dt.AddMonths(1);

            // 各変数に値をセット
            pstrTargetMonth = dt.ToString("yyyyMM");
            lblTargetMonth.Text = dt.ToString("yyyy年MM月");

            // グリッドにデータをセット
            SetMainGridData();

            //Add Start WataruT 2020.07.21 初回登録解除機能追加
            // 初回登録ボタンの制御
            clsMainScheduleCommonControl.CheckScheduleFirstFlag();
            //Add End   WataruT 2020.07.21 初回登録解除機能追加
        }

        /// <summary>
        /// 「希望」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDataKind_Request_Click(object sender, EventArgs e)
        {
            if (btnDataKind_Request.BackColor != Color.PaleVioletRed)
            {
                // ボタンの色、フォント変更
                btnDataKind_Request.BackColor = Color.PaleVioletRed;
                btnDataKind_Schedule.BackColor = Color.DimGray;
                btnDataKind_Result.BackColor = Color.DimGray;
                btnDataKind_Request.Font = new Font("Meiryo UI", 11, FontStyle.Bold | FontStyle.Underline);
                btnDataKind_Schedule.Font = new Font("Meiryo UI", 11, FontStyle.Bold);
                btnDataKind_Result.Font = new Font("Meiryo UI", 11, FontStyle.Bold);

                // 対応ボタンの有効・無効
                btnAutoCreate.Visible = false;
                btnCheckContinueWork.Visible = false;
                btnSaveFirst.Visible = false;
                // Add Start WataruT 2020.07.21 希望シフト反映ボタンの表示不具合
                btnImportRequest.Visible = false;
                // Add End   WataruT 2020.07.21 希望シフト反映ボタンの表示不具合

                // 表示するデータの種類の共通変数を変更
                piDataKind = 1;
                
                // ActiveControlを初期化
                ActiveControl = null;

                // グリッドの位置・サイズを設定
                clsMainScheduleCommonControl.SetGridPositionAndSize();

                // グリッドにデータをセット
                SetMainGridData();
            }
        }

        /// <summary>
        /// 「予定」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDataKind_Schedule_Click(object sender, EventArgs e)
        {
            if (btnDataKind_Schedule.BackColor != Color.PaleVioletRed)
            {
                // ボタンの色、フォント変更
                btnDataKind_Request.BackColor = Color.DimGray;
                btnDataKind_Schedule.BackColor = Color.PaleVioletRed;
                btnDataKind_Result.BackColor = Color.DimGray;
                btnDataKind_Request.Font = new Font("Meiryo UI", 11, FontStyle.Bold);
                btnDataKind_Schedule.Font = new Font("Meiryo UI", 11, FontStyle.Bold | FontStyle.Underline);
                btnDataKind_Result.Font = new Font("Meiryo UI", 11, FontStyle.Bold);

                // 対応ボタンの有効・無効
                btnAutoCreate.Visible = true;
                btnCheckContinueWork.Visible = true;
                btnSaveFirst.Visible = true;
                // Add Start WataruT 2020.07.21 希望シフト反映ボタンの表示不具合
                btnImportRequest.Visible = true;
                // Add End   WataruT 2020.07.21 希望シフト反映ボタンの表示不具合

                // 表示するデータの種類の共通変数を変更
                piDataKind = 2;

                // ActiveControlを初期化
                ActiveControl = null;

                // グリッドの位置・サイズを設定
                clsMainScheduleCommonControl.SetGridPositionAndSize();

                // グリッドにデータをセット
                SetMainGridData();
            }
        }

        /// <summary>
        /// 「実績」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDataKind_Result_Click(object sender, EventArgs e)
        {
            if (btnDataKind_Result.BackColor != Color.PaleVioletRed)
            {
                // ボタンの色、フォント変更
                btnDataKind_Request.BackColor = Color.DimGray;
                btnDataKind_Schedule.BackColor = Color.DimGray;
                btnDataKind_Result.BackColor = Color.PaleVioletRed;
                btnDataKind_Request.Font = new Font("Meiryo UI", 11, FontStyle.Bold);
                btnDataKind_Schedule.Font = new Font("Meiryo UI", 11, FontStyle.Bold);
                btnDataKind_Result.Font = new Font("Meiryo UI", 11, FontStyle.Bold | FontStyle.Underline);

                // 対応ボタンの有効・無効
                btnAutoCreate.Visible = false;
                btnCheckContinueWork.Visible = false;
                btnSaveFirst.Visible = false;
                // Add Start WataruT 2020.07.21 希望シフト反映ボタンの表示不具合
                btnImportRequest.Visible = false;
                // Add End   WataruT 2020.07.21 希望シフト反映ボタンの表示不具合

                // 表示するデータの種類の共通変数を変更
                piDataKind = 3;
                
                // ActiveControlを初期化
                ActiveControl = null;

                // グリッドの位置・サイズを設定
                clsMainScheduleCommonControl.SetGridPositionAndSize();

                // グリッドにデータをセット
                SetMainGridData();
            }
        }

        /// <summary>
        /// 「看護師」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStaffKind_Nurse_Click(object sender, EventArgs e)
        {
            if (btnStaffKind_Nurse.BackColor != Color.PaleVioletRed)
            {
                // ボタンの色、フォント変更
                btnStaffKind_Nurse.BackColor = Color.PaleVioletRed;
                btnStaffKind_Care.BackColor = Color.DimGray;
                btnStaffKind_Nurse.Font = new Font("Meiryo UI", 11, FontStyle.Bold | FontStyle.Underline);
                btnStaffKind_Care.Font = new Font("Meiryo UI", 11, FontStyle.Bold);
                
                // 職種の共通変数を"看護師"に変更
                pstrStaffKind = "01";
                
                // ActiveControlを初期化
                ActiveControl = null;

                // グリッドにデータをセット
                SetMainGridData();

                //Add Start WataruT 2020.07.21 初回登録解除機能追加
                // 初回登録ボタンの制御
                clsMainScheduleCommonControl.CheckScheduleFirstFlag();
                //Add End   WataruT 2020.07.21 初回登録解除機能追加
            }
        }

        /// <summary>
        /// 「ケア」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStaffKind_Care_Click(object sender, EventArgs e)
        {
            if (btnStaffKind_Care.BackColor != Color.PaleVioletRed)
            {
                // ボタンの色、フォント変更
                btnStaffKind_Nurse.BackColor = Color.DimGray;
                btnStaffKind_Care.BackColor = Color.PaleVioletRed;
                btnStaffKind_Nurse.Font = new Font("Meiryo UI", 11, FontStyle.Bold);
                btnStaffKind_Care.Font = new Font("Meiryo UI", 11, FontStyle.Bold | FontStyle.Underline);
                
                // 職種の共通変数を"ケア"に変更
                pstrStaffKind = "02";
                
                // ActiveControlを初期化
                ActiveControl = null;

                // グリッドにデータをセット
                SetMainGridData();

                //Add Start WataruT 2020.07.21 初回登録解除機能追加
                // 初回登録ボタンの制御
                clsMainScheduleCommonControl.CheckScheduleFirstFlag();
                //Add End   WataruT 2020.07.21 初回登録解除機能追加
            }
        }

        /// <summary>
        /// 「様式９チェック」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnYoushiki9_Click(object sender, EventArgs e)
        {
            Youshiki9Check frmYoushiki9Check = new Youshiki9Check(cmbWard.SelectedValue.ToString(), cmbWard.Text.ToString(), lblTargetMonth.Text); 

            frmYoushiki9Check.ShowDialog();
        }

        /// <summary>
        /// 「管理者メニュー」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMaster_Click(object sender, EventArgs e)
        {
            MasterMenu frmMasterMenu = new MasterMenu();

            this.Hide();
            frmMasterMenu.ShowDialog();
            this.Show();
        }

        /// <summary>
        /// 「希望取込」ボタン
        /// Add WataruT 2020.07.16 希望シフトのみ取込機能追加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportRequest_Click(object sender, EventArgs e)
        {
            // 職員登録チェック
            if (dtScheduleStaff.Rows.Count == 0)
            {
                MessageBox.Show("職員登録をおこなってください。", "");
                return;
            }

            // 常日勤チェック
            if (clsMainScheduleCommonControl.CheckStaffDayOnly() == false)
            {
                MessageBox.Show("常日勤設定のない職員がいます。", "");
                return;
            }

            // プログレスウィンドウの表示
            clsMainScheduleCommonControl.ShowProgressDialog(true);

            // シフトの自動作成
            clsMainScheduleScheduleControl.ImportRequestData();

            // プログレスウィンドウを閉じる
            clsMainScheduleCommonControl.ShowProgressDialog(false);
        }

        /// <summary>
        /// 「帳票印刷」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrintReport_Click(object sender, EventArgs e)
        {
            ReportListMenu frmReportListMenu = new ReportListMenu();

            this.Hide();
            frmReportListMenu.ShowDialog();
            this.Show();
        }

        // --- 各種イベント ---

        /// <summary>
        /// フォームアクティベート時、初回のみ描画処理を実施
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainSchedule_Activated(object sender, EventArgs e)
        {
            if (pbActivateFlag == false)
            {
                // 初期起動時の処理
                clsMainScheduleCommonControl.InitialProcess(true);

                // 各種グリッドの位置・サイズを設定
                clsMainScheduleCommonControl.SetGridPositionAndSize();

                // 既存データの表示
                clsMainScheduleScheduleControl.SetMainData_Schedule();

                // 初回実行フラグ無効化
                pbActivateFlag = true;
            }
        }

        /// <summary>
        /// メイングリッドスクロール時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdMain_Scroll(object sender, ScrollEventArgs e)
        {
            // グリッドの表示位置を調整
            clsMainScheduleCommonControl.SetGridPosition();
        }
        
        /// <summary>
        /// 病棟選択時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbWard_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(pbActivateFlag == true)
            {
                // グリッドにデータをセット
                SetMainGridData();
            }
        }

        /// <summary>
        /// メイングリッドの右クリックメニュー表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdMain_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            // 勤務データ種類
            switch(piDataKind)
            {
                case 1: // 希望シフト
                        // データセルのみ対象とする
                    if (e.ColumnIndex > 0)
                        e.ContextMenuStrip = ctmsMain_Request;                        
                    else
                        return;
                    break;
                case 2: // 予定データ
                    // データセルのみ対象とする
                    if (e.ColumnIndex > 0)
                        e.ContextMenuStrip = ctmsMain_Schedule;
                    else
                        return;
                    break;
                case 3: // 実績データ
                        // データセルのみ対象とする
                    if (e.ColumnIndex > 1)
                        e.ContextMenuStrip = ctmsMain_Result;
                    else
                        return;
                    break;
            }

            // 右クリックしたセルの行・列番号をセット
            piGrdMain_CurrentColumn = e.ColumnIndex;
            piGrdMain_CurrentRow = e.RowIndex;
            
            // 右クリックしたセルをカレントセルとする
            grdMain.CurrentCell = grdMain[piGrdMain_CurrentColumn, piGrdMain_CurrentRow];

        }

        /// <summary>
        /// 右クリックメニューの各項目イベント(希望)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctmsMain_ClickItem_Request(object sender, EventArgs e)
        {
            switch (sender.ToString())
            {
                case "入力パレット表示":
                    ShiftControler shiftControler = new ShiftControler(this);
                    shiftControler.ShowDialog();
                    break;
                case "日勤":
                    ChangeMainGridData(0);
                    break;
                case "夜勤":
                    ChangeMainGridData(1);
                    break;
                case "夜明":
                    ChangeMainGridData(2);
                    break;
                case "公休(1日)":
                    ChangeMainGridData(3);
                    break;
                case "公休(午前)":
                    ChangeMainGridData(4);
                    break;
                case "公休(午後)":
                    ChangeMainGridData(5);
                    break;
                case "有休(1日)":
                    ChangeMainGridData(6);
                    break;
                case "有休(午前)":
                    ChangeMainGridData(7);
                    break;
                case "有休(午後)":
                    ChangeMainGridData(8);
                    break;
                case "代有":
                    ChangeMainGridData(9);
                    break;
                case "遅出":
                    ChangeMainGridData(10);
                    break;
                case "研修":
                    ChangeMainGridData(11);
                    break;
                case "特別休暇":
                    ChangeMainGridData(12);
                    break;
                case "欠勤":
                    ChangeMainGridData(13);
                    break;
                case "病欠":
                    ChangeMainGridData(14);
                    break;
                case "早出":
                    ChangeMainGridData(15);
                    break;
                case "入職前":
                    ChangeMainGridData(16);
                    break;
                // Add Start WataruT 2020.07.22 特定の時短勤務用の項目追加
                case "5.25":
                    ChangeMainGridData(17);
                    break;
                // Add End   WataruT 2020.07.22 特定の時短勤務用の項目追加
                // Add Start WataruT 2020.07.27 特定の時短勤務用の項目追加
                case "2":
                    ChangeMainGridData(18);
                    break;
                case "6":
                    ChangeMainGridData(19);
                    break;
                case "6.25":
                    ChangeMainGridData(20);
                    break;
                case "7":
                    ChangeMainGridData(21);
                    break;
                // Add End   WataruT 2020.07.27 特定の時短勤務用の項目追加
            }
        }

        /// <summary>
        /// 右クリックメニューの各項目イベント(予定)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctmsMain_ClickItem_Schedule(object sender, EventArgs e)
        {
            switch(sender.ToString())
            {
                case "日勤":
                    ChangeMainGridData(0);
                    break;
                case "夜勤":
                    ChangeMainGridData(1);
                    break;
                case "夜明":
                    ChangeMainGridData(2);
                    break;
                case "公休(1日)":
                    ChangeMainGridData(3);
                    break;
                case "公休(午前)":
                    ChangeMainGridData(4);
                    break;
                case "公休(午後)":
                    ChangeMainGridData(5);
                    break;
                case "有休(1日)":
                    ChangeMainGridData(6);
                    break;
                case "有休(午前)":
                    ChangeMainGridData(7);
                    break;
                case "有休(午後)":
                    ChangeMainGridData(8);
                    break;
                case "代有":
                    ChangeMainGridData(9);
                    break;
                case "遅出":
                    ChangeMainGridData(10);
                    break;
                case "研修":
                    ChangeMainGridData(11);
                    break;
                case "特別休暇":
                    ChangeMainGridData(12);
                    break;
                case "欠勤":
                    ChangeMainGridData(13);
                    break;
                case "病欠":
                    ChangeMainGridData(14);
                    break;
                case "早出":
                    ChangeMainGridData(15);
                    break;
                case "入職前":
                    ChangeMainGridData(16);
                    break;
                // Add Start WataruT 2020.07.22 特定の時短勤務用の項目追加
                case "5.25":
                    ChangeMainGridData(17);
                    break;
                // Add End   WataruT 2020.07.22 特定の時短勤務用の項目追加
                // Add Start WataruT 2020.07.27 特定の時短勤務用の項目追加
                case "2":
                    ChangeMainGridData(18);
                    break;
                case "6":
                    ChangeMainGridData(19);
                    break;
                case "6.25":
                    ChangeMainGridData(20);
                    break;
                case "7":
                    ChangeMainGridData(21);
                    break;
                // Add End   WataruT 2020.07.27 特定の時短勤務用の項目追加
            }
        }

        /// <summary>
        /// 右クリックメニューの各項目イベント(実績)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctmsMain_ClickItem_Result(object sender, EventArgs e)
        {
            switch (sender.ToString())
            {
                case "予定データ取込":
                    clsMainScheduleResultControl.ImportScheduleData();
                    clsMainScheduleCommonControl.SetGridPosition();
                    break;
                case "実績データを入力":
                    clsMainScheduleResultControl.ShowEditResultData();
                    break;
                case "実績データを削除":
                    clsMainScheduleResultControl.ClearResultDataFromMainGrid();
                    break;
            }
        }

        /// <summary>
        /// メイングリッドでのキー操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdMain_KeyDown(object sender, KeyEventArgs e)
        {
            // 勤務データ種類
            switch (piDataKind)
            {
                case 1: // 希望シフト
                    switch (e.KeyData)
                    {
                        case Keys.Delete:
                            ChangeMainGridData(99);
                            break;
                    }
                    break;
                case 2: // 予定データ
                    break;
                case 3: // 実績データ
                    break;
            }
        }

        // --- ファンクション、サブルーチン ---

        /// <summary>
        /// メイングリッドデータの変更
        /// </summary>
        public void ChangeMainGridData(int iWorkKindID)
        {
            switch(piDataKind)
            {
                case 1:
                    // Mod Start WataruT 2020.07.13 複数選択箇所を一括変更可能とする
                    //clsMainScheduleRequestControl.ChangeMainGridData(iWorkKindID);
                    if (grdMain.SelectedCells.Count > 1)
                    {
                        clsMainScheduleRequestControl.ChangeMainGridMultiData(iWorkKindID);
                    }
                    else
                    {
                        clsMainScheduleRequestControl.ChangeMainGridData(iWorkKindID);
                    }
                    // Mod End   WataruT 2020.07.13 複数選択箇所を一括変更可能とする
                    break;
                case 2:
                    // Mod Start WataruT 2020.07.13 複数選択箇所を一括変更可能とする
                    //clsMainScheduleScheduleControl.ChangeMainGridData(iWorkKindID);
                    if (grdMain.SelectedCells.Count > 1)
                    {
                        clsMainScheduleScheduleControl.ChangeMainGridMultiData(iWorkKindID);
                    }
                    else
                    {
                        clsMainScheduleScheduleControl.ChangeMainGridData(iWorkKindID);
                    }
                    // Mod End   WataruT 2020.07.13 複数選択箇所を一括変更可能とする
                    break;
            }
        }

        /// <summary>
        /// メイングリッドのデータをセット
        /// </summary>
        private void SetMainGridData()
        {
            // データセット前の初期化処理
            clsMainScheduleCommonControl.InitialProcess(false);

            switch(piDataKind)
            {
                case 1:     // 希望シフト
                    clsMainScheduleRequestControl.SetMainData_Request();
                    break;
                case 2:     // 予定データ
                    clsMainScheduleScheduleControl.SetMainData_Schedule();
                    break;
                case 3:     // 実績データ
                    clsMainScheduleResultControl.SetMainData_Result();
                    break;
            }
        }

        /// <summary>
        /// メイングリッドのツールチップ表示対応
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdMain_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            // 実績画面の場合のみ表示
            if(piDataKind == 3)
            {   if (e.ColumnIndex >= 2 && grdMain[e.ColumnIndex, e.RowIndex].Style.BackColor != Color.White)
                    e.ToolTipText = clsMainScheduleResultControl.GetToolTipText(e.ColumnIndex, e.RowIndex);
            }
        }
    }
}
