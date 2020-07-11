using System;
using System.Data;
using System.Windows.Forms;
using workschedule.Controls;
using workschedule.Functions;

namespace workschedule
{
    public partial class EditStaffMaster : Form
    {
        public DataTable dtStaff;                           // 職員マスタテーブル
        public DataTable dtWard;                            // 病棟マスタデーブル
        public DataTable dtStaffKind;                       // 職種マスタデーブル
        public DataTable dtStaffPosition;                   // 役職マスタデーブル

        public string pstrWardID;                           // 対象病棟ID
        public string pstrTargetMonth;                      // 対象年月
        public string pstrStaffKind;                        // 対象職種

        // 使用クラス宣言
        DatabaseControl clsDatabaseControl = new DatabaseControl();

        public EditStaffMaster()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            // データ初期化
            InitialProcess();
        }

        // --- ボタンイベント ---

        /// <summary>
        /// 「新規追加」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            EditStaffMasterDetail frmEditStaffMasterDetail = new EditStaffMasterDetail("", true);
            
            this.Hide();
            frmEditStaffMasterDetail.ShowDialog();
            this.Show();

            // データ再取得
            InitialProcess();

            // マスタ情報を表示
            SetStaffList();
        }

        /// <summary>
        /// 「編集」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            EditStaffMasterDetail frmEditStaffMasterDetail = new EditStaffMasterDetail(
                grdStaff[0, grdStaff.CurrentCell.RowIndex].Value.ToString(), false);

            this.Hide();
            frmEditStaffMasterDetail.ShowDialog();
            this.Show();

            // データ再取得
            InitialProcess();

            // マスタ情報を表示
            SetStaffList();
        }

        /// <summary>
        /// 「表示順」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSort_Click(object sender, EventArgs e)
        {
            EditStaffMasterSort frmEditStaffMasterSort = new EditStaffMasterSort();

            this.Hide();
            frmEditStaffMasterSort.ShowDialog();
            this.Show();

            // データ再取得
            InitialProcess();

            // マスタ情報を表示
            SetStaffList();
        }


        /// <summary>
        /// 「閉じる」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        // --- 各種コントロールイベント --

        /// <summary>
        /// フォームアクティベート時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditStaffMaster_Activated(object sender, EventArgs e)
        {
            // マスタ情報を表示
            SetStaffList();
        }

        /// <summary>
        /// 使用フラグのチェックボックス切り替え
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkUsingFlag_CheckedChanged(object sender, EventArgs e)
        {
            // データ再取得
            InitialProcess();

            // マスタ情報を表示
            SetStaffList();
        }


        // --- ファンクション、サブルーチン ---

        /// <summary>
        /// 初期化ルーチン処理
        /// </summary>
        public void InitialProcess()
        {
            // 各種データテーブル取得
            if (chkUsingFlag.Checked == true)
                dtStaff = clsDatabaseControl.GetStaff_All();
            else
                dtStaff = clsDatabaseControl.GetStaff();
            dtStaffKind = clsDatabaseControl.GetStaffKind();
            dtStaffPosition = clsDatabaseControl.GetStaffPosition();
            dtWard = clsDatabaseControl.GetWard();
        }

        /// <summary>
        /// 職員情報をグリッドにセット
        /// </summary>
        private void SetStaffList()
        {
            // DataTableの初期化
            DataTable dt;
            DataRow drStaff;

            // 表示用DataTableを初期化
            dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("name");
            dt.Columns.Add("ward");
            dt.Columns.Add("sex");
            dt.Columns.Add("staff_kind");
            dt.Columns.Add("staff_position");
            dt.Columns.Add("seq");
            dt.Columns.Add("using_flag");
            dt.Columns.Add("created_date");
            dt.Columns.Add("updated_date");

            // DataTableに表示用データをセット
            for (int iRow = 0; iRow < dtStaff.Rows.Count; iRow++)
            {
                // DataRow初期化
                drStaff = dt.NewRow();

                // 各種データをDataRowにセット
                drStaff["id"] = dtStaff.Rows[iRow]["id"];
                drStaff["name"] = dtStaff.Rows[iRow]["name"];
                drStaff["ward"] = ChangeWardToName(dtStaff.Rows[iRow]["ward"].ToString());
                drStaff["sex"] = ChangeSexToName(dtStaff.Rows[iRow]["sex"].ToString());
                drStaff["staff_kind"] = ChangeStaffKindToName(dtStaff.Rows[iRow]["staff_kind"].ToString(), dtStaff.Rows[iRow]["staff_kind_sub"].ToString());
                drStaff["staff_position"] = ChangeStaffPositionToName(dtStaff.Rows[iRow]["staff_position"].ToString());
                drStaff["seq"] = dtStaff.Rows[iRow]["seq"];
                drStaff["using_flag"] = dtStaff.Rows[iRow]["using_flag"];
                drStaff["created_date"] = dtStaff.Rows[iRow]["created_date"];
                drStaff["updated_date"] = dtStaff.Rows[iRow]["updated_date"];

                // DataTableにDataRowを追加
                dt.Rows.Add(drStaff);
            }

            // データをグリッドにセット
            grdStaff.DataSource = dt;

            // 列ヘッダ設定
            SetColumnHeaderProperties();
        }


        /// <summary>
        /// グリッドの列ヘッダ設定
        /// </summary>
        private void SetColumnHeaderProperties()
        {
            // 列名をセット
            grdStaff.Columns[0].HeaderText = "ID";
            grdStaff.Columns[1].HeaderText = "氏名";
            grdStaff.Columns[2].HeaderText = "部署";
            grdStaff.Columns[3].HeaderText = "性別";
            grdStaff.Columns[4].HeaderText = "職種";
            grdStaff.Columns[5].HeaderText = "役職";
            grdStaff.Columns[6].HeaderText = "表示順";
            grdStaff.Columns[7].HeaderText = "有効フラグ";
            grdStaff.Columns[8].HeaderText = "作成日時";
            grdStaff.Columns[9].HeaderText = "更新日時";

            // 列幅をセット
            grdStaff.Columns[0].Width = 58;
            grdStaff.Columns[1].Width = 100;
            grdStaff.Columns[2].Width = 50;
            grdStaff.Columns[3].Width = 50;
            grdStaff.Columns[4].Width = 80;
            grdStaff.Columns[5].Width = 70;
            grdStaff.Columns[6].Width = 50;
            grdStaff.Columns[7].Width = 50;
            grdStaff.Columns[8].Width = 140;
            grdStaff.Columns[9].Width = 140;

        }

        /// <summary>
        /// 病棟の名称取得
        /// </summary>
        private string ChangeWardToName(string strWard)
        {
            for(int iRow = 0; iRow < dtWard.Rows.Count; iRow++)
            {
                if(strWard == dtWard.Rows[iRow]["id"].ToString())
                    return dtWard.Rows[iRow]["name"].ToString();
            }
            return "";
        }

        /// <summary>
        /// 職種の名称取得
        /// </summary>
        private string ChangeStaffKindToName(string strStaffKind, string strStaffKindSub)
        {
            for (int iRow = 0; iRow < dtStaffKind.Rows.Count; iRow++)
            {
                if (strStaffKind == dtStaffKind.Rows[iRow]["id"].ToString() && strStaffKindSub == dtStaffKind.Rows[iRow]["sub_id"].ToString())
                    return dtStaffKind.Rows[iRow]["name"].ToString();
            }
            return "";
        }

        /// <summary>
        /// 役職の名称取得
        /// </summary>
        private string ChangeStaffPositionToName(string strStaffPosition)
        {
            for (int iRow = 0; iRow < dtStaffPosition.Rows.Count; iRow++)
            {
                if (strStaffPosition == dtStaffPosition.Rows[iRow]["id"].ToString())
                    return dtStaffPosition.Rows[iRow]["name"].ToString();
            }
            return "";
        }

        /// <summary>
        /// 性別の名称取得
        /// </summary>
        /// <returns></returns>
        private string ChangeSexToName(string strSex)
        {
            switch(strSex)
            {
                case "1":
                    return "男性";
                case "2":
                    return "女性";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 有効フラグの名称取得
        /// </summary>
        /// <returns></returns>
        private string ChangeUsingFlagToName(string strUsingFlag)
        {
            switch (strUsingFlag)
            {
                case "1":
                    return "有効";
                case "0":
                    return "無効";
                default:
                    return "";
            }
        }


    }
}
