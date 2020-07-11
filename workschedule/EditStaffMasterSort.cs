using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using workschedule.Controls;
using workschedule.Functions;

namespace workschedule
{
    public partial class EditStaffMasterSort : Form
    {
        public DataTable dtStaff;                           // 職員マスタテーブル
        public DataTable dtWard;                            // 病棟マスタデーブル

        public string pstrWardID;                           // 対象病棟ID
        public string pstrTargetMonth;                      // 対象年月
        public string pstrStaffKind;                        // 対象職種

        // 使用クラス宣言
        DatabaseControl clsDatabaseControl = new DatabaseControl();
        DataTableControl clsDataTableControl = new DataTableControl();

        public EditStaffMasterSort()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            SetWard();
            SetStaffLeft();
        }

        // --- ボタンイベント ---
        /// <summary>
        /// 「閉じる」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 「保存」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // 職員並び順の保存
            SaveStaffSort();
        }

        /// <summary>
        /// 「↑」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUp_Click(object sender, EventArgs e)
        {
            List<ItemSet> srcStaff = new List<ItemSet>();
            int iStaffCount = lstStaff.Items.Count;
            int iSelectedIndex = lstStaff.SelectedIndex;

            // 先頭もしくはデータ無しなら処理終了
            if (iSelectedIndex <= 0)
                return;

            // リストボックスの描画停止
            lstStaff.BeginUpdate();

            // 右の職員のリストをあらかじめ用意
            for (int iStaff = 0; iStaff < iStaffCount; iStaff++)
            {
                if (iStaff + 1 == iSelectedIndex)
                {
                    srcStaff.Add((ItemSet)lstStaff.Items[iStaff + 1]);
                    srcStaff.Add((ItemSet)lstStaff.Items[iStaff]);
                    iStaff++;
                }
                else
                {
                    srcStaff.Add((ItemSet)lstStaff.Items[iStaff]);
                }
            }
            lstStaff.DataSource = srcStaff;
            lstStaff.DisplayMember = "ItemDisp";
            lstStaff.ValueMember = "ItemValue";

            // 選択項目の初期化
            lstStaff.SelectedIndex = iSelectedIndex - 1;

            // リストボックスの描画再開
            lstStaff.EndUpdate();
        }

        /// <summary>
        /// 「↓」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDown_Click(object sender, EventArgs e)
        {
            List<ItemSet> srcStaff = new List<ItemSet>();
            int iStaffCount = lstStaff.Items.Count;
            int iSelectedIndex = lstStaff.SelectedIndex;

            // 先頭もしくはデータ無しなら処理終了
            if (iSelectedIndex == -1 || iSelectedIndex == iStaffCount - 1)
                return;

            // リストボックスの描画停止
            lstStaff.BeginUpdate();

            // 右の職員のリストをあらかじめ用意
            for (int iStaff = 0; iStaff < iStaffCount; iStaff++)
            {
                if (iStaff == iSelectedIndex)
                {
                    srcStaff.Add((ItemSet)lstStaff.Items[iStaff + 1]);
                    srcStaff.Add((ItemSet)lstStaff.Items[iStaff]);
                    iStaff++;
                }
                else
                {
                    srcStaff.Add((ItemSet)lstStaff.Items[iStaff]);
                }
            }
            lstStaff.DataSource = srcStaff;
            lstStaff.DisplayMember = "ItemDisp";
            lstStaff.ValueMember = "ItemValue";

            // 選択項目の初期化
            lstStaff.SelectedIndex = iSelectedIndex + 1;

            // リストボックスの描画再開
            lstStaff.EndUpdate();
        }

        // --- 各種イベント ---

        /// <summary>
        /// 病棟選択時、対象病棟の職員を表示する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstWard_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<ItemSet> srcStaff = new List<ItemSet>();

            // リストボックスの描画停止
            lstStaff.BeginUpdate();

            // 職員マスタの取得
            dtStaff = clsDatabaseControl.GetStaff_Ward(lstWard.SelectedValue.ToString());

            foreach (DataRow row in dtStaff.Rows)
            {
                srcStaff.Add(new ItemSet(row["name"].ToString(), row["id"].ToString()));
            }
            lstStaff.DataSource = srcStaff;
            lstStaff.DisplayMember = "ItemDisp";
            lstStaff.ValueMember = "ItemValue";
            lstStaff.ClearSelected();

            // リストボックスの描画再開
            lstStaff.EndUpdate();
        }


        // --- サブルーチン、ファンクション ---


        /// <summary>
        /// 病棟リストをセット
        /// </summary>
        private void SetWard()
        {
            List<ItemSet> srcWard = new List<ItemSet>();

            // 病棟マスタを取得
            dtWard = clsDatabaseControl.GetWard();

            foreach (DataRow row in dtWard.Rows)
            {
                srcWard.Add(new ItemSet(row["name"].ToString(), row["id"].ToString()));
            }

            lstWard.DataSource = srcWard;
            lstWard.DisplayMember = "ItemDisp";
            lstWard.ValueMember = "ItemValue";
            lstWard.SelectedIndex = 0;
        }

        /// <summary>
        /// 患者リストをセット
        /// </summary>
        private void SetStaffLeft()
        {
            List<ItemSet> srcStaff = new List<ItemSet>();

            // リストボックスの描画停止
            lstStaff.BeginUpdate();

            // 職員マスタの取得
            dtStaff = clsDatabaseControl.GetStaff_Ward(lstWard.SelectedValue.ToString());

            foreach (DataRow row in dtStaff.Rows)
            {
                srcStaff.Add(new ItemSet(row["name"].ToString(), row["id"].ToString()));
            }
            lstStaff.DataSource = srcStaff;
            lstStaff.DisplayMember = "ItemDisp";
            lstStaff.ValueMember = "ItemValue";
            lstStaff.ClearSelected();

            // リストボックスの描画再開
            lstStaff.EndUpdate();
        }

        /// <summary>
        /// 職員並び順の保存
        /// </summary>
        private void SaveStaffSort()
        {
            DataTable dtStaff;
            DataRow drStaff;
            ItemSet isTemp;

            // DataTable、DataRowを初期化
            dtStaff = clsDataTableControl.GetTable_Staff();

            for (int iSEQ = 1; iSEQ < lstStaff.Items.Count; iSEQ++)
            {
                drStaff = dtStaff.NewRow();
                isTemp = lstStaff.Items[iSEQ - 1] as ItemSet;
                
                drStaff["id"] = isTemp.ItemValue;
                drStaff["seq"] = iSEQ;

                clsDatabaseControl.UpdateStaff_SEQ(drStaff);
            }

            MessageBox.Show("保存完了", "");
        }

    }
}
