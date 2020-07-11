using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using workschedule.Controls;
using workschedule.Functions;

namespace workschedule
{
    public partial class EditStaffList : Form
    {
        public DataTable dtStaff;                           // 職員マスタテーブル
        public DataTable dtWard;                            // 病棟マスタデーブル
        public DataTable dtScheduleStaff;                   // 予定職員データテーブル

        public string pstrWardID;                           // 対象病棟ID
        public string pstrTargetMonth;                      // 対象年月
        public string pstrStaffKind;                        // 対象職種

        // 使用クラス宣言
        DatabaseControl clsDatabaseControl = new DatabaseControl();
        DataTableControl clsDataTableControl = new DataTableControl();

        public EditStaffList(string strWardID, string strWardText, string strTargetMonth, string strStaffKindID, string strStaffKindName)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            // 対象病棟、対象日付、対象職種をラベルにセット
            lblTargetWardValue.Text = strWardText;
            lblTargetDateValue.Text = strTargetMonth;
            lblStaffKindValue.Text = strStaffKindName;

            // 共通変数に病棟ID、日付をセット            
            pstrWardID = strWardID;
            pstrTargetMonth = strTargetMonth.Substring(0, 4) + strTargetMonth.Substring(5, 2);
            pstrStaffKind = strStaffKindID;

            SetWard(strWardID);
            SetStaffLeft();
            SetStaffRight();
        }

        // --- ボタンイベント ---

        /// <summary>
        /// 「保存」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveScheduleStaff();

            Close();
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

        /// <summary>
        /// 「＞」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnToRight_Click(object sender, EventArgs e)
        {
            List<ItemSet> srcStaff = new List<ItemSet>();
            ItemSet isStaffLeft, isStaffRight;
            int iStaffRightCount = lstStaffRight.Items.Count;
            bool bAddFlag = true;

            // リストボックスの描画停止
            lstStaffRight.BeginUpdate();

            // 右の職員のリストをあらかじめ用意
            for (int iStaffRight = 0; iStaffRight < iStaffRightCount; iStaffRight++)
            {
                srcStaff.Add((ItemSet)lstStaffRight.Items[iStaffRight]);
            }

            // 左の選択された職員
            for (int iStaffLeft = 0; iStaffLeft < lstStaffLeft.SelectedItems.Count; iStaffLeft++)
            {
                // 左の職員データをセット
                isStaffLeft = (ItemSet)lstStaffLeft.SelectedItems[iStaffLeft];

                // 右の全職員
                for (int iStaffRight = 0; iStaffRight < iStaffRightCount; iStaffRight++)
                {
                    // 右の職員データをセット
                    isStaffRight = (ItemSet)lstStaffRight.Items[iStaffRight];

                    // 右の職員に同じ人がいるか
                    if (isStaffLeft.ItemValue == isStaffRight.ItemValue)
                    {
                        bAddFlag = false;
                        break;
                    }
                }
                if (bAddFlag == true)
                {
                    srcStaff.Add((ItemSet)lstStaffLeft.SelectedItems[iStaffLeft]);
                }
                bAddFlag = true;
            }
            lstStaffRight.DataSource = srcStaff;
            lstStaffRight.DisplayMember = "ItemDisp";
            lstStaffRight.ValueMember = "ItemValue";

            // 選択項目の初期化
            lstStaffRight.ClearSelected();
            lstStaffLeft.ClearSelected();

            // リストボックスの描画再開
            lstStaffRight.EndUpdate();
        }

        /// <summary>
        /// 「＞＞」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnToRightAll_Click(object sender, EventArgs e)
        {
            List<ItemSet> srcStaff = new List<ItemSet>();
            ItemSet isStaffLeft, isStaffRight;
            int iStaffRightCount = lstStaffRight.Items.Count;
            bool bAddFlag = true;
            
            // リストボックスの描画停止
            lstStaffRight.BeginUpdate();

            // 右の職員のリストをあらかじめ用意
            for (int iStaffRight = 0; iStaffRight < iStaffRightCount; iStaffRight++)
            {
                srcStaff.Add((ItemSet)lstStaffRight.Items[iStaffRight]);
            }

            // 左の選択された職員
            for(int iStaffLeft = 0; iStaffLeft < lstStaffLeft.Items.Count; iStaffLeft++)
            {
                // 左の職員データをセット
                isStaffLeft = (ItemSet)lstStaffLeft.Items[iStaffLeft];
                
                // 右の全職員
                for (int iStaffRight = 0; iStaffRight < iStaffRightCount; iStaffRight++)
                {
                    // 右の職員データをセット
                    isStaffRight = (ItemSet)lstStaffRight.Items[iStaffRight];

                    // 右の職員に同じ人がいるか
                    if (isStaffLeft.ItemValue == isStaffRight.ItemValue)
                    {
                        bAddFlag = false;
                        break;
                    }
                }
                if(bAddFlag == true)
                {
                    srcStaff.Add((ItemSet)lstStaffLeft.Items[iStaffLeft]);
                }
                bAddFlag = true;
            }
            lstStaffRight.DataSource = srcStaff;
            lstStaffRight.DisplayMember = "ItemDisp";
            lstStaffRight.ValueMember = "ItemValue";

            // 選択項目の初期化
            lstStaffRight.ClearSelected();
            lstStaffLeft.ClearSelected();

            // リストボックスの描画再開
            lstStaffRight.EndUpdate();

        }

        /// <summary>
        /// 「＜＜」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnToLeftAll_Click(object sender, EventArgs e)
        {
            // リストボックスの初期化
            lstStaffRight.DataSource = null;

            // 選択項目の初期化
            lstStaffRight.ClearSelected();
            lstStaffLeft.ClearSelected();
        }

        /// <summary>
        /// 「＜」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnToLeft_Click(object sender, EventArgs e)
        {
            List<ItemSet> srcStaff = new List<ItemSet>();
            bool bRemoveFlag = false;

            // リストボックスの描画停止
            lstStaffRight.BeginUpdate();

            // 右の職員のリストをあらかじめ用意
            for (int iStaffRight = 0; iStaffRight < lstStaffRight.Items.Count; iStaffRight++)
            {
                for(int iStaffRightSelect = 0; iStaffRightSelect < lstStaffRight.SelectedItems.Count; iStaffRightSelect++)
                {
                    if(iStaffRight == lstStaffRight.SelectedIndices[iStaffRightSelect])
                    {
                        bRemoveFlag = true;
                        break;
                    }
                }
                if(bRemoveFlag == false)
                    srcStaff.Add((ItemSet)lstStaffRight.Items[iStaffRight]);
                bRemoveFlag = false;
            }

            lstStaffRight.DataSource = srcStaff;
            lstStaffRight.DisplayMember = "ItemDisp";
            lstStaffRight.ValueMember = "ItemValue";

            // 選択項目の初期化
            lstStaffRight.ClearSelected();
            lstStaffLeft.ClearSelected();

            // リストボックスの描画再開
            lstStaffRight.EndUpdate();
        }

        /// <summary>
        /// 「↑」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUp_Click(object sender, EventArgs e)
        {
            List<ItemSet> srcStaff = new List<ItemSet>();
            int iStaffRightCount = lstStaffRight.Items.Count;
            int iSelectedIndex = lstStaffRight.SelectedIndex;

            // 先頭もしくはデータ無しなら処理終了
            if (iSelectedIndex <= 0)
                return;

            // リストボックスの描画停止
            lstStaffRight.BeginUpdate();

            // 右の職員のリストをあらかじめ用意
            for (int iStaffRight = 0; iStaffRight < iStaffRightCount; iStaffRight++)
            {
                if(iStaffRight + 1 == iSelectedIndex)
                {
                    srcStaff.Add((ItemSet)lstStaffRight.Items[iStaffRight + 1]);
                    srcStaff.Add((ItemSet)lstStaffRight.Items[iStaffRight]);
                    iStaffRight++ ;
                }
                else
                {
                    srcStaff.Add((ItemSet)lstStaffRight.Items[iStaffRight]);
                }
            }
            lstStaffRight.DataSource = srcStaff;
            lstStaffRight.DisplayMember = "ItemDisp";
            lstStaffRight.ValueMember = "ItemValue";

            // 選択項目の初期化
            lstStaffRight.SelectedIndex = iSelectedIndex - 1;
            lstStaffLeft.ClearSelected();

            // リストボックスの描画再開
            lstStaffRight.EndUpdate();
        }

        /// <summary>
        /// 「↓」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDown_Click(object sender, EventArgs e)
        {
            List<ItemSet> srcStaff = new List<ItemSet>();
            int iStaffRightCount = lstStaffRight.Items.Count;
            int iSelectedIndex = lstStaffRight.SelectedIndex;

            // 先頭もしくはデータ無しなら処理終了
            if (iSelectedIndex == -1 || iSelectedIndex == iStaffRightCount - 1)
                return;

            // リストボックスの描画停止
            lstStaffRight.BeginUpdate();

            // 右の職員のリストをあらかじめ用意
            for (int iStaffRight = 0; iStaffRight < iStaffRightCount; iStaffRight++)
            {
                if (iStaffRight == iSelectedIndex)
                {
                    srcStaff.Add((ItemSet)lstStaffRight.Items[iStaffRight + 1]);
                    srcStaff.Add((ItemSet)lstStaffRight.Items[iStaffRight]);
                    iStaffRight++;
                }
                else
                {
                    srcStaff.Add((ItemSet)lstStaffRight.Items[iStaffRight]);
                }
            }
            lstStaffRight.DataSource = srcStaff;
            lstStaffRight.DisplayMember = "ItemDisp";
            lstStaffRight.ValueMember = "ItemValue";

            // 選択項目の初期化
            lstStaffRight.SelectedIndex = iSelectedIndex + 1;
            lstStaffLeft.ClearSelected();

            // リストボックスの描画再開
            lstStaffRight.EndUpdate();
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
            lstStaffLeft.BeginUpdate();

            // 職員マスタの取得
            dtStaff = clsDatabaseControl.GetStaff_Ward_StaffKind(lstWard.SelectedValue.ToString(), pstrStaffKind);

            foreach (DataRow row in dtStaff.Rows)
            {
                srcStaff.Add(new ItemSet(row["name"].ToString(), row["id"].ToString()));
            }
            lstStaffLeft.DataSource = srcStaff;
            lstStaffLeft.DisplayMember = "ItemDisp";
            lstStaffLeft.ValueMember = "ItemValue";
            lstStaffLeft.ClearSelected();

            // リストボックスの描画再開
            lstStaffLeft.EndUpdate();
        }


        // サブルーチン、ファンクション

        /// <summary>
        /// 病棟リストをセット
        /// </summary>
        private void SetWard(string strWard)
        {
            List<ItemSet> srcWard = new List<ItemSet>();
            int iWardNum = 0;

            // 病棟マスタを取得
            dtWard = clsDatabaseControl.GetWard();

            foreach (DataRow row in dtWard.Rows)
            {
                srcWard.Add(new ItemSet(row["name"].ToString(), row["id"].ToString()));
                if (row["id"].ToString() == strWard)
                    iWardNum = srcWard.Count;
            }
            lstWard.DataSource = srcWard;
            lstWard.DisplayMember = "ItemDisp";
            lstWard.ValueMember = "ItemValue";
            lstWard.SelectedIndex = iWardNum - 1;
        }

        /// <summary>
        /// 患者リスト(左側)をセット
        /// </summary>
        private void SetStaffLeft()
        {
            List<ItemSet> srcStaff = new List<ItemSet>();

            // リストボックスの描画停止
            lstStaffLeft.BeginUpdate();

            // 職員マスタの取得
            dtStaff = clsDatabaseControl.GetStaff_Ward_StaffKind(lstWard.SelectedValue.ToString(), pstrStaffKind);

            foreach (DataRow row in dtStaff.Rows)
            {
                srcStaff.Add(new ItemSet(row["name"].ToString(), row["id"].ToString()));
            }
            lstStaffLeft.DataSource = srcStaff;
            lstStaffLeft.DisplayMember = "ItemDisp";
            lstStaffLeft.ValueMember = "ItemValue";
            lstStaffLeft.ClearSelected();

            // リストボックスの描画再開
            lstStaffLeft.EndUpdate();
        }

        /// <summary>
        /// 患者リスト(右側)をセット
        /// </summary>
        private void SetStaffRight()
        {
            List<ItemSet> srcScheduleStaff = new List<ItemSet>();

            // 予定職員を取得
            dtScheduleStaff = clsDatabaseControl.GetScheduleStaff_List(pstrWardID, pstrTargetMonth, pstrStaffKind);

            foreach (DataRow row in dtScheduleStaff.Rows)
            {
                srcScheduleStaff.Add(new ItemSet(row["name"].ToString(), row["id"].ToString()));
            }
            lstStaffRight.DataSource = srcScheduleStaff;
            lstStaffRight.DisplayMember = "ItemDisp";
            lstStaffRight.ValueMember = "ItemValue";
            lstStaffRight.ClearSelected();
        }

        /// <summary>
        /// 予定職員一覧を登録
        /// </summary>
        private void SaveScheduleStaff()
        {
            DataTable dtScheduleStaff;
            DataRow drScheduleStaff;
            ItemSet isScheduleStaff;

            // データテーブル準備
            dtScheduleStaff = clsDataTableControl.GetTable_ScheduleStaff();
            drScheduleStaff = dtScheduleStaff.NewRow();

            try
            {
                // 既存データの削除
                clsDatabaseControl.DeleteScheduleStaff_Ward_TargetMonth(pstrWardID, pstrTargetMonth, pstrStaffKind);

                // メイングリッドから希望シフトを検索
                for (int iStaff = 0; iStaff < lstStaffRight.Items.Count; iStaff++)
                {
                    // ItemSetを作成
                    isScheduleStaff = (ItemSet)lstStaffRight.Items[iStaff];

                    // セットする値の準備
                    drScheduleStaff["ward"] = pstrWardID;
                    drScheduleStaff["target_month"] = pstrTargetMonth;
                    drScheduleStaff["staff_kind"] = pstrStaffKind;
                    drScheduleStaff["staff_id"] = isScheduleStaff.ItemValue.ToString();
                    drScheduleStaff["seq"] = (iStaff + 1).ToString();
                    drScheduleStaff["office_flag"] = clsDatabaseControl.GetStaffDayOnly_OfficeFlag(isScheduleStaff.ItemValue.ToString());

                    // INSERT実行
                    clsDatabaseControl.InsertScheduleStaff(drScheduleStaff);
                }

                // 完了メッセージの表示
                MessageBox.Show("保存完了", "", MessageBoxButtons.OK);
            }
            catch (Exception e)
            {
                MessageBox.Show("保存失敗", "", MessageBoxButtons.OK);
                Console.WriteLine("ERROR: " + e.Message);
            }
        }

    }
}
