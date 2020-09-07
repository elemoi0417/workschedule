using System;
using System.Data;
using System.Windows.Forms;
using workschedule.Controls;
using System.IO;
using OfficeOpenXml;

namespace workschedule.Reports
{
    class PrintResultDetailWorkKind
    {
        // 使用クラス宣言
        DatabaseControl clsDatabaseControl = new DatabaseControl();

        // 定数
        const int COLUMN_TARGET_WARD = 9;
        const int ROW_TARGET_WARD = 2;
        const int COLUMN_TARGET_MONTH = 9;
        const int ROW_TARGET_MONTH = 3;
        const int COLUMN_TARGET_WORK_KIND = 9;
        const int ROW_TARGET_WORK_KIND = 4;

        const int ROW_DETAIL_DATA = 7;

        const int COLUMN_STAFF_NAME = 1;
        const int COLUMN_TARGET_DATE = 10;
        const int COLUMN_START_TIME = 18;
        const int COLUMN_END_TIME = 28;

        const int COLUMN_STAFF_NAME_OTHER = 1;
        const int COLUMN_TARGET_DATE_OTHER = 10;
        const int COLUMN_DETAIL_ITEM_NAME_OTHER = 18;
        const int COLUMN_START_TIME_OTHER = 29;
        const int COLUMN_END_TIME_OTHER = 35;

        // 変数
        string strFilePath = Environment.CurrentDirectory + @"\Report\resultdetailworkkind.xlsx";
        string strFilePathOther = Environment.CurrentDirectory + @"\Report\resultdetailworkkindother.xlsx";

        string pstrTargetMonth;                     // 対象月(YYYYMM)
        string pstrTargetMonthName;                 // 対象月(YYYY年MM月)
        string pstrWard;                            // 対象病棟コード
        string pstrWardName;                        // 対象病棟名
        string pstrTargetWorkKindName;              // 対象となる勤務詳細項目

        /// <summary>
        /// クラス初期化
        /// </summary>
        public PrintResultDetailWorkKind(string strTargetMonth, string strWard, string strWardName, string strTargetWorkKindName)
        {
            // 共通変数をセット
            pstrTargetMonth = strTargetMonth;
            pstrTargetMonthName = strTargetMonth.Substring(0, 4) + "年" + strTargetMonth.Substring(4, 2) + "月";
            pstrWard = strWard;
            pstrWardName = strWardName;
            pstrTargetWorkKindName = strTargetWorkKindName;
        }

        /// <summary>
        /// 帳票ファイル作成処理
        /// </summary>
        /// <param name="targetReport"></param>
        /// <param name="staffNo"></param>
        /// <param name="orderNo"></param>
        public void SaveFile()
        {
            DataTable dtResultDetail;
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

            // == 実績データ取得 == 
            if (pstrTargetWorkKindName == "その他") {
                // Excelファイルの読み込み
                var xlReadFile = new FileInfo(strFilePathOther);
                // オブジェクトにセット
                using (var xlFile = new ExcelPackage(xlReadFile))
                {
                    // シートを選択
                    var xlSheet = xlFile.Workbook.Worksheets["実績項目詳細"];

                    // === Excelデータ入力 ===
                    // 病棟、対象年月、勤務詳細
                    xlSheet.Cells[ROW_TARGET_WARD, COLUMN_TARGET_WARD].Value = pstrWardName;
                    xlSheet.Cells[ROW_TARGET_MONTH, COLUMN_TARGET_MONTH].Value = pstrTargetMonthName;
                    xlSheet.Cells[ROW_TARGET_WORK_KIND, COLUMN_TARGET_WORK_KIND].Value = pstrTargetWorkKindName;

                    dtResultDetail = clsDatabaseControl.GetResultDetail_Ward_TargetDate_ResultDetailItem_Other(pstrWard, pstrTargetMonth);

                    for (int iResultDetailCount = 0; iResultDetailCount < dtResultDetail.Rows.Count; iResultDetailCount++)
                    {
                        // 職員氏名
                        xlSheet.Cells[ROW_DETAIL_DATA + iResultDetailCount, COLUMN_STAFF_NAME_OTHER].Value = dtResultDetail.Rows[iResultDetailCount]["staff_name"].ToString();
                        // 対象日
                        xlSheet.Cells[ROW_DETAIL_DATA + iResultDetailCount, COLUMN_TARGET_DATE_OTHER].Value = dtResultDetail.Rows[iResultDetailCount]["target_date"].ToString();
                        // 項目名
                        xlSheet.Cells[ROW_DETAIL_DATA + iResultDetailCount, COLUMN_DETAIL_ITEM_NAME_OTHER].Value = dtResultDetail.Rows[iResultDetailCount]["work_kind"].ToString();
                        // 開始時刻
                        xlSheet.Cells[ROW_DETAIL_DATA + iResultDetailCount, COLUMN_START_TIME_OTHER].Value = dtResultDetail.Rows[iResultDetailCount]["start_time"].ToString();
                        // 終了時刻
                        xlSheet.Cells[ROW_DETAIL_DATA + iResultDetailCount, COLUMN_END_TIME_OTHER].Value = dtResultDetail.Rows[iResultDetailCount]["end_time"].ToString();
                    }

                    // ファイルを保存
                    xlFile.SaveAs(new FileInfo(sfd.FileName));
                }
            }
            else
            {
                // Excelファイルの読み込み
                var xlReadFile = new FileInfo(strFilePath);

                // オブジェクトにセット
                using (var xlFile = new ExcelPackage(xlReadFile))
                {
                    // シートを選択
                    var xlSheet = xlFile.Workbook.Worksheets["実績項目詳細"];

                    // === Excelデータ入力 ===
                    // 病棟、対象年月、勤務詳細
                    xlSheet.Cells[ROW_TARGET_WARD, COLUMN_TARGET_WARD].Value = pstrWardName;
                    xlSheet.Cells[ROW_TARGET_MONTH, COLUMN_TARGET_MONTH].Value = pstrTargetMonthName;
                    xlSheet.Cells[ROW_TARGET_WORK_KIND, COLUMN_TARGET_WORK_KIND].Value = pstrTargetWorkKindName;

                    dtResultDetail = clsDatabaseControl.GetResultDetail_Ward_TargetDate_ResultDetailItem(pstrWard, pstrTargetMonth, pstrTargetWorkKindName);

                    for (int iResultDetailCount = 0; iResultDetailCount < dtResultDetail.Rows.Count; iResultDetailCount++)
                    {
                        // 職員氏名
                        xlSheet.Cells[ROW_DETAIL_DATA + iResultDetailCount, COLUMN_STAFF_NAME].Value = dtResultDetail.Rows[iResultDetailCount]["staff_name"].ToString();
                        // 対象日
                        xlSheet.Cells[ROW_DETAIL_DATA + iResultDetailCount, COLUMN_TARGET_DATE].Value = dtResultDetail.Rows[iResultDetailCount]["target_date"].ToString();
                        // 開始時刻
                        xlSheet.Cells[ROW_DETAIL_DATA + iResultDetailCount, COLUMN_START_TIME].Value = dtResultDetail.Rows[iResultDetailCount]["start_time"].ToString();
                        // 終了時刻
                        xlSheet.Cells[ROW_DETAIL_DATA + iResultDetailCount, COLUMN_END_TIME].Value = dtResultDetail.Rows[iResultDetailCount]["end_time"].ToString();
                    }
                    xlSheet.Cells[6, 37].Value = "= IF((AB7 - R7) = 0, \"\", AB7 - R7)";

                    // ファイルを保存
                    xlFile.SaveAs(new FileInfo(sfd.FileName));
                }
            }

            // 終了メッセージ
            MessageBox.Show("保存完了");
        }


        /// <summary>
        /// オブジェクト初期化
        /// </summary>
        private void InitializeObject()
        {
           
        }

        /// <summary>
        /// 保存ダイアログのプロパティ設定
        /// </summary>
        private void SetSaveFileDialogProperties(ref SaveFileDialog sfd)
        {   
            // ファイル名の既定値(YYYY年MM月_〇病棟_様式9.xlsx)
            sfd.FileName = pstrTargetMonthName + "_" + pstrWardName + "_実績項目集計(" + pstrTargetWorkKindName + ").xlsx";
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
    }
}
