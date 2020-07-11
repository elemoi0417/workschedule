using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using workschedule.Controls;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;

namespace workschedule.Reports
{
    class PrintResultDetailWorkKind
    {
        // 使用クラス宣言
        DatabaseControl clsDatabaseControl = new DatabaseControl();

        // 定数
        const int COLUMN_TARGET_WARD = 8;
        const int ROW_TARGET_WARD = 1;
        const int COLUMN_TARGET_MONTH = 8;
        const int ROW_TARGET_MONTH = 2;
        const int COLUMN_TARGET_WORK_KIND = 8;
        const int ROW_TARGET_WORK_KIND = 3;

        const int ROW_DETAIL_DATA = 6;

        const int COLUMN_STAFF_NAME = 0;
        const int COLUMN_TARGET_DATE = 9;
        const int COLUMN_START_TIME = 17;
        const int COLUMN_END_TIME = 27;

        const int COLUMN_STAFF_NAME_OTHER = 0;
        const int COLUMN_TARGET_DATE_OTHER = 9;
        const int COLUMN_DETAIL_ITEM_NAME_OTHER = 17;
        const int COLUMN_START_TIME_OTHER = 28;
        const int COLUMN_END_TIME_OTHER = 34;

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
            IWorkbook xlWorkbook;

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
                // Officeオブジェクトの初期化
                xlWorkbook = WorkbookFactory.Create(strFilePathOther);
                ISheet xlSheet = xlWorkbook.GetSheet("実績項目詳細");

                // === Excelデータ入力 ===
                // 病棟、対象年月、勤務詳細
                WriteCellValue(xlSheet, COLUMN_TARGET_WARD, ROW_TARGET_WARD, pstrWardName);
                WriteCellValue(xlSheet, COLUMN_TARGET_MONTH, ROW_TARGET_MONTH, pstrTargetMonthName);
                WriteCellValue(xlSheet, COLUMN_TARGET_WORK_KIND, ROW_TARGET_WORK_KIND, pstrTargetWorkKindName);

                dtResultDetail = clsDatabaseControl.GetResultDetail_Ward_TargetDate_ResultDetailItem_Other(pstrWard, pstrTargetMonth);
                
                for (int iResultDetailCount = 0; iResultDetailCount < dtResultDetail.Rows.Count; iResultDetailCount++)
                {
                    // 職員氏名
                    WriteCellValue(xlSheet, COLUMN_STAFF_NAME_OTHER, ROW_DETAIL_DATA + iResultDetailCount, dtResultDetail.Rows[iResultDetailCount]["staff_name"].ToString());
                    // 対象日
                    WriteCellValue(xlSheet, COLUMN_TARGET_DATE_OTHER, ROW_DETAIL_DATA + iResultDetailCount, dtResultDetail.Rows[iResultDetailCount]["target_date"].ToString());
                    // 項目名
                    WriteCellValue(xlSheet, COLUMN_DETAIL_ITEM_NAME_OTHER, ROW_DETAIL_DATA + iResultDetailCount, dtResultDetail.Rows[iResultDetailCount]["work_kind"].ToString());
                    // 開始時刻
                    WriteCellValue(xlSheet, COLUMN_START_TIME_OTHER, ROW_DETAIL_DATA + iResultDetailCount, DateTime.Parse(dtResultDetail.Rows[iResultDetailCount]["start_time"].ToString()));
                    // 終了時刻
                    WriteCellValue(xlSheet, COLUMN_END_TIME_OTHER, ROW_DETAIL_DATA + iResultDetailCount, DateTime.Parse(dtResultDetail.Rows[iResultDetailCount]["end_time"].ToString()));
                }
            }
            else
            {
                // Officeオブジェクトの初期化
                xlWorkbook = WorkbookFactory.Create(strFilePath);
                ISheet xlSheet = xlWorkbook.GetSheet("実績項目詳細");

                // === Excelデータ入力 ===
                // 病棟、対象年月、勤務詳細
                WriteCellValue(xlSheet, COLUMN_TARGET_WARD, ROW_TARGET_WARD, pstrWardName);
                WriteCellValue(xlSheet, COLUMN_TARGET_MONTH, ROW_TARGET_MONTH, pstrTargetMonthName);
                WriteCellValue(xlSheet, COLUMN_TARGET_WORK_KIND, ROW_TARGET_WORK_KIND, pstrTargetWorkKindName);

                dtResultDetail = clsDatabaseControl.GetResultDetail_Ward_TargetDate_ResultDetailItem(pstrWard, pstrTargetMonth, pstrTargetWorkKindName);

                for (int iResultDetailCount = 0; iResultDetailCount < dtResultDetail.Rows.Count; iResultDetailCount++)
                {
                    // 職員氏名
                    WriteCellValue(xlSheet, COLUMN_STAFF_NAME, ROW_DETAIL_DATA + iResultDetailCount, dtResultDetail.Rows[iResultDetailCount]["staff_name"].ToString());
                    // 対象日
                    WriteCellValue(xlSheet, COLUMN_TARGET_DATE, ROW_DETAIL_DATA + iResultDetailCount, dtResultDetail.Rows[iResultDetailCount]["target_date"].ToString());
                    // 開始時刻
                    WriteCellValue(xlSheet, COLUMN_START_TIME, ROW_DETAIL_DATA + iResultDetailCount, DateTime.Parse(dtResultDetail.Rows[iResultDetailCount]["start_time"].ToString()));
                    // 終了時刻
                    WriteCellValue(xlSheet, COLUMN_END_TIME, ROW_DETAIL_DATA + iResultDetailCount, DateTime.Parse(dtResultDetail.Rows[iResultDetailCount]["end_time"].ToString()));
                }
                WriteCellValue(xlSheet, 37, 6, "= IF((AB7 - R7) = 0, \"\", AB7 - R7)");
            }

            // シート内の各関数の再計算
            XSSFFormulaEvaluator.EvaluateAllFormulaCells(xlWorkbook);

            // ファイル保存
            using (var fs = new FileStream(sfd.FileName, FileMode.Create))
            {
                xlWorkbook.Write(fs);
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
            sfd.FileName = pstrTargetMonthName + "_" + pstrWardName + "_" + "様式9.xlsx";
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
        
        /// <summary>
        /// セル書き込み(時刻)
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="idxRow"></param>
        /// <param name="idxRow"></param>
        /// <param name="value"></param>
        static void WriteCellValue(ISheet sheet, int idxColumn, int idxRow, DateTime value)
        {
            var row = sheet.GetRow(idxRow) ?? sheet.CreateRow(idxRow); //指定した行を取得できない時はエラーとならないよう新規作成している
            var cell = row.GetCell(idxColumn) ?? row.CreateCell(idxColumn); //一行上の処理の列版


            cell.SetCellValue(value);
        }

        /// <summary>
        /// セル書き込み(文字列)
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="idxRow"></param>
        /// <param name="idxRow"></param>
        /// <param name="value"></param>
        static void WriteCellValue(ISheet sheet, int idxColumn, int idxRow, string value)
        {
            var row = sheet.GetRow(idxRow) ?? sheet.CreateRow(idxRow); //指定した行を取得できない時はエラーとならないよう新規作成している
            var cell = row.GetCell(idxColumn) ?? row.CreateCell(idxColumn); //一行上の処理の列版

            cell.SetCellValue(value);
        }

        /// <summary>
        /// セル書き込み(文字列)
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="idxRow"></param>
        /// <param name="idxRow"></param>
        /// <param name="value"></param>
        static void WriteCellValue(ISheet sheet, int idxColumn, int idxRow, double value)
        {
            var row = sheet.GetRow(idxRow) ?? sheet.CreateRow(idxRow); //指定した行を取得できない時はエラーとならないよう新規作成している
            var cell = row.GetCell(idxColumn) ?? row.CreateCell(idxColumn); //一行上の処理の列版

            cell.SetCellValue(value);
        }

        /// <summary>
        /// セルスタイルの取得
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="idxRow"></param>
        /// <param name="idxRow"></param>
        /// <returns></returns>
        static ICellStyle GetCellStyle(ISheet sheet, int idxColumn, int idxRow)
        {
            var row = sheet.GetRow(idxRow) ?? sheet.CreateRow(idxRow); //指定した行を取得できない時はエラーとならないよう新規作成している
            var cell = row.GetCell(idxColumn) ?? row.CreateCell(idxColumn); //一行上の処理の列版

            return cell.CellStyle;
        }

        /// <summary>
        /// セルスタイルをセット
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="idxRow"></param>
        /// <param name="idxRow"></param>
        /// <param name="value"></param>
        static void WriteCellStyle(ISheet sheet, int idxColumn, int idxRow, ICellStyle cellStyle)
        {
            var row = sheet.GetRow(idxRow) ?? sheet.CreateRow(idxRow); //指定した行を取得できない時はエラーとならないよう新規作成している
            var cell = row.GetCell(idxColumn) ?? row.CreateCell(idxColumn); //一行上の処理の列版

            cell.CellStyle = cellStyle;
        }
    }
}
