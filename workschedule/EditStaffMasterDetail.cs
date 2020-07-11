using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using workschedule.Controls;
using workschedule.Functions;

namespace workschedule
{
    public partial class EditStaffMasterDetail : Form
    {
        string strStaffID;          // 職員ID
        string strWard;             // 病棟
        string strSEQ;              // 表示順

        bool bModeFlag;             // 表示モード(TRUE : 新規、FALSE : 修正)

        DataTable dtWard;           // 病棟マスタ
        DataTable dtStaffKind;      // 職種マスタ
        DataTable dtStaffPosition;  // 役職マスタ

        // 使用クラス宣言
        DatabaseControl clsDatabaseControl = new DatabaseControl();
        DataTableControl clsDataTableControl = new DataTableControl();

        public EditStaffMasterDetail(string strStaffID_Parent, bool bModeFlag_Parent)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            // 共通変数値をセット
            strStaffID = strStaffID_Parent;
            bModeFlag = bModeFlag_Parent;

            // 初回表示時の処理
            InitialProcess();

            // 職員情報を各種コントロールにセット
            SetStaffData();
        }

        // --- ボタンイベント ---

        /// <summary>
        /// 「保存」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // 登録・更新処理
            switch(bModeFlag)
            {
                case true:          // 「新規追加」モード
                    InsertStaffData();
                    Close();
                    break;
                case false:         // 「更新」モード
                    UpdateStaffData();
                    Close();
                    break;
            }
        }

        /// <summary>
        /// 「キャンセル」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }


        // --- ファンクション、サブルーチン ---

        /// <summary>
        /// 初期化ルーチン処理
        /// </summary>
        public void InitialProcess()
        {
            // 各種データテーブル取得
            dtWard = clsDatabaseControl.GetWard();
            dtStaffKind = clsDatabaseControl.GetStaffKind();
            dtStaffPosition = clsDatabaseControl.GetStaffPosition();

            // 各種マスタをセット
            SetSexComboBox();
            SetWardComboBox();
            SetStaffKindComboBox();
            SetStaffPositionComboBox();
            SetUsingFlagComboBox();
        }

        /// <summary>
        /// 職員情報を各種コントロールにセット
        /// </summary>
        private void SetStaffData()
        {
            if (bModeFlag == true)      // 「新規追加」モード
            {
                txtID.Text = "";
                txtName.Text = "";
                cmbWard.SelectedIndex = 0;
                cmbSex.SelectedIndex = 0;
                cmbStaffKind.SelectedIndex = 0;
                cmbStaffPosition.SelectedIndex = 0;
                cmbUsingFlag.SelectedIndex = 0;

                strWard = "";
                strSEQ = "";
            }
            else                        // 「編集」モード
            {
                // マスタからデータを取得
                DataTable dtStaff = clsDatabaseControl.GetStaff_ID(strStaffID);

                // データが取得できていない場合は画面を閉じる
                if(dtStaff.Rows.Count == 0)
                {
                    MessageBox.Show("対象職員の情報が表示できません。","");
                    Close();
                }

                // 各種データをコントロールにセット
                txtID.Text = dtStaff.Rows[0]["id"].ToString();
                txtName.Text = dtStaff.Rows[0]["name"].ToString();
                for (int iIndex = 0; iIndex < cmbSex.Items.Count; iIndex++) {
                    cmbSex.SelectedIndex = iIndex;
                    if(cmbSex.SelectedValue.ToString() == dtStaff.Rows[0]["sex"].ToString())
                    {
                        break;
                    }
                }
                for (int iIndex = 0; iIndex < cmbWard.Items.Count; iIndex++)
                {
                    cmbWard.SelectedIndex = iIndex;
                    if (cmbWard.SelectedValue.ToString() == dtStaff.Rows[0]["ward"].ToString())
                    {
                        strWard = dtStaff.Rows[0]["ward"].ToString();
                        break;
                    }
                }
                for (int iIndex = 0; iIndex < cmbStaffKind.Items.Count; iIndex++)
                {
                    cmbStaffKind.SelectedIndex = iIndex;
                    if (cmbStaffKind.SelectedValue.ToString() == dtStaff.Rows[0]["staff_kind"].ToString() + dtStaff.Rows[0]["staff_kind_sub"].ToString())
                    {
                        break;
                    }
                }
                for (int iIndex = 0; iIndex < cmbStaffPosition.Items.Count; iIndex++)
                {
                    cmbStaffPosition.SelectedIndex = iIndex;
                    if (cmbStaffPosition.SelectedValue.ToString() == dtStaff.Rows[0]["staff_position"].ToString())
                    {
                        break;
                    }
                }
                for (int iIndex = 0; iIndex < cmbUsingFlag.Items.Count; iIndex++)
                {
                    cmbUsingFlag.SelectedIndex = iIndex;
                    if (cmbUsingFlag.SelectedValue.ToString() == dtStaff.Rows[0]["using_flag"].ToString())
                    {
                        break;
                    }
                }

                // 表示順を変数に保存しておく
                strSEQ = dtStaff.Rows[0]["seq"].ToString();
            }
        }

        /// <summary>
        /// 性別のコンボボックスをセット
        /// </summary>
        private void SetSexComboBox()
        {
            List<ItemSet> srcSex = new List<ItemSet>();

            cmbSex.DataSource = null;

            srcSex.Add(new ItemSet("男性", "1"));
            srcSex.Add(new ItemSet("女性", "2"));

            cmbSex.DataSource = srcSex;
            cmbSex.DisplayMember = "ItemDisp";
            cmbSex.ValueMember = "ItemValue";
            cmbSex.SelectedIndex = 0;
        }

        /// <summary>
        /// 病棟のコンボボックスをセット
        /// </summary>
        private void SetWardComboBox()
        {
            List<ItemSet> srcWard = new List<ItemSet>();

            cmbWard.DataSource = null;

            foreach (DataRow row in dtWard.Rows)
            {
                srcWard.Add(new ItemSet(row["name"].ToString(), row["id"].ToString()));
            }

            cmbWard.DataSource = srcWard;
            cmbWard.DisplayMember = "ItemDisp";
            cmbWard.ValueMember = "ItemValue";
            cmbWard.SelectedIndex = 0;
        }

        /// <summary>
        /// 職種のコンボボックスをセット
        /// </summary>
        private void SetStaffKindComboBox()
        {
            List<ItemSet> srcStaffKind = new List<ItemSet>();

            cmbStaffKind.DataSource = null;

            foreach (DataRow row in dtStaffKind.Rows)
            {
                srcStaffKind.Add(new ItemSet(row["name"].ToString(), row["id"].ToString() + row["sub_id"].ToString()));
            }

            cmbStaffKind.DataSource = srcStaffKind;
            cmbStaffKind.DisplayMember = "ItemDisp";
            cmbStaffKind.ValueMember = "ItemValue";
            cmbStaffKind.SelectedIndex = 0;
        }

        /// <summary>
        /// 役職のコンボボックスをセット
        /// </summary>
        private void SetStaffPositionComboBox()
        {
            List<ItemSet> srcStaffPosition = new List<ItemSet>();

            cmbStaffPosition.DataSource = null;

            foreach (DataRow row in dtStaffPosition.Rows)
            {
                srcStaffPosition.Add(new ItemSet(row["name"].ToString(), row["id"].ToString()));
            }

            cmbStaffPosition.DataSource = srcStaffPosition;
            cmbStaffPosition.DisplayMember = "ItemDisp";
            cmbStaffPosition.ValueMember = "ItemValue";
            cmbStaffPosition.SelectedIndex = 0;
        }

        /// <summary>
        /// 使用フラグのコンボボックスをセット
        /// </summary>
        private void SetUsingFlagComboBox()
        {
            List<ItemSet> srcUsingFlag = new List<ItemSet>();

            cmbUsingFlag.DataSource = null;

            srcUsingFlag.Add(new ItemSet("有効", "1"));
            srcUsingFlag.Add(new ItemSet("無効", "0"));

            cmbUsingFlag.DataSource = srcUsingFlag;
            cmbUsingFlag.DisplayMember = "ItemDisp";
            cmbUsingFlag.ValueMember = "ItemValue";
            cmbUsingFlag.SelectedIndex = 0;
        }

        /// <summary>
        /// DBに職員情報を登録
        /// </summary>
        private void InsertStaffData()
        {
            DataTable dtStaff;
            DataRow drStaff;

            // DataTable、DataRowを初期化
            dtStaff = clsDataTableControl.GetTable_Staff();
            drStaff = dtStaff.NewRow();

            // 各種値をDataRowにセット
            drStaff["id"] = txtID.Text;
            drStaff["name"] = txtName.Text;
            drStaff["sex"] = cmbSex.SelectedValue;
            drStaff["ward"] = cmbWard.SelectedValue;
            drStaff["staff_kind"] = cmbStaffKind.SelectedValue.ToString().Substring(0, 2);
            drStaff["staff_kind_sub"] = cmbStaffKind.SelectedValue.ToString().Substring(2, 2);
            drStaff["staff_position"] = cmbStaffPosition.SelectedValue;
            drStaff["seq"] = int.Parse(clsDatabaseControl.GetStaff_MaxSEQ(cmbWard.SelectedValue.ToString())) + 1;
            drStaff["using_flag"] = cmbUsingFlag.SelectedValue;
            drStaff["created_date"] = DateTime.Now.ToString();
            drStaff["updated_date"] = DateTime.Now.ToString();

            // データ登録処理
            clsDatabaseControl.InsertStaff(drStaff);

            // 完了メッセージの表示
            MessageBox.Show("保存完了しました。", "", MessageBoxButtons.OK);
        }

        /// <summary>
        /// DBの職員情報を更新
        /// </summary>
        private void UpdateStaffData()
        {
            DataTable dtStaff;
            DataRow drStaff;

            // DataTable、DataRowを初期化
            dtStaff = clsDataTableControl.GetTable_Staff();
            drStaff = dtStaff.NewRow();

            // 各種値をDataRowにセット
            drStaff["id"] = txtID.Text;
            drStaff["name"] = txtName.Text;
            drStaff["sex"] = cmbSex.SelectedValue;
            drStaff["ward"] = cmbWard.SelectedValue;
            drStaff["staff_kind"] = cmbStaffKind.SelectedValue.ToString().Substring(0, 2);
            drStaff["staff_kind_sub"] = cmbStaffKind.SelectedValue.ToString().Substring(2, 2);
            drStaff["staff_position"] = cmbStaffPosition.SelectedValue;
            if (strWard != cmbWard.SelectedValue.ToString())
                drStaff["seq"] = int.Parse(clsDatabaseControl.GetStaff_MaxSEQ(cmbWard.SelectedValue.ToString())) + 1;
            else
                drStaff["seq"] = strSEQ;
            drStaff["using_flag"] = cmbUsingFlag.SelectedValue;
            drStaff["staff_position"] = cmbStaffPosition.SelectedValue;
            drStaff["updated_date"] = DateTime.Now.ToString();

            // データ登録処理
            clsDatabaseControl.UpdateStaff(drStaff);

            // 完了メッセージの表示
            MessageBox.Show("保存完了しました。", "", MessageBoxButtons.OK);
        }
    }
}
