using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using workschedule.Controls;
using workschedule.Functions;

namespace workschedule
{
    public partial class EditEmergencyDate : Form
    {
        // 使用クラス宣言
        DatabaseControl clsDatabaseControl = new DatabaseControl();
        DataTableControl clsDataTableControl = new DataTableControl();

        public EditEmergencyDate()
        {
            InitializeComponent();

            // リストにデータをセット
            SetListData();
        }

        // --- ボタンイベント ---

        /// <summary>
        /// 「新規登録」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            // リスト選択状態のクリア
            lstEmergencyDate.ClearSelected();

            // 各種テキストボックスを初期化
            txtYear.Text = DateTime.Now.ToString("yyyy");
            txtMonth.Text = "";
            txtDay.Text = "";

            // 対象月にフォーカスをセット
            txtMonth.Focus();

            // 各種ボタンの設定
            btnDelete.Enabled = false;
            btnNew.Enabled = false;
            btnSave.Enabled = true;
        }

        /// <summary>
        /// 「保存」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dtEmergencyDate;
            DataRow drEmergencyDate;
            DateTime dtTargetDate;

            // 日付入力チェック
            if (txtYear.Text == "" || txtMonth.Text == "" || txtDay.Text == "")
            {
                MessageBox.Show("対象日付を入力してください");
                return;
            }

            // 入力データの日付型変換チェック
            if (DateTime.TryParse(txtYear.Text + "/" + txtMonth.Text + "/" + txtDay.Text, out dtTargetDate))
            {
                dtEmergencyDate = clsDataTableControl.GetTable_EmergencyDate();
                drEmergencyDate = dtEmergencyDate.NewRow();

                drEmergencyDate["target_date"] = dtTargetDate.ToString();

                if (clsDatabaseControl.InsertEmergencyDate(drEmergencyDate) == true)
                { 
                    MessageBox.Show("登録完了しました");
                    
                    // 再描画
                    SetListData();
                }
                else
                    MessageBox.Show("対象日付はすでに登録されています。");
            }
            else
            {
                MessageBox.Show("日付を正しく入力してください");
                return;
            }
        }

        /// <summary>
        /// 「削除」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // 選択データのチェック
            if (lstEmergencyDate.SelectedItems.Count <= 0)
            {
                MessageBox.Show("削除するデータが選択されていません。");
                return;
            }

            // 削除確認
            if(MessageBox.Show("選択しているデータを削除してもよろしいですか？","",MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // データを削除
                clsDatabaseControl.DeleteEmergencyDate_TargetDate(lstEmergencyDate.SelectedItem.ToString());

                // 削除完了のメッセージ表示
                MessageBox.Show("削除が完了しました。");

                // 再描画
                SetListData();
            }
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

        // --- サブルーチン、ファンクション ---

        /// <summary>
        /// リストにデータをセット
        /// </summary>
        private void SetListData()
        {
            DataTable dt;
            List<string> srcEmergencyDate = new List<string>();

            // リストの一覧をクリア
            lstEmergencyDate.DataSource = null;

            // データを取得
            dt = clsDatabaseControl.GetEmergencyDate();
            foreach (DataRow row in dt.Rows)
            {
                srcEmergencyDate.Add(row["target_date"].ToString().Substring(0, 10));
            }

            // リストにデータをセット
            lstEmergencyDate.DataSource = srcEmergencyDate;
            if(lstEmergencyDate.Items.Count > 0)
                lstEmergencyDate.SelectedIndex = 0;
        }

        // --- 各種イベント ---

        /// <summary>
        /// 救急指定日リスト選択時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstEmergencyDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            // データが選択されている場合
            if (lstEmergencyDate.SelectedItems.Count > 0)
            {
                // 選択したデータをセット
                txtYear.Text = lstEmergencyDate.SelectedItem.ToString().Substring(0, 4);
                txtMonth.Text = lstEmergencyDate.SelectedItem.ToString().Substring(5, 2);
                txtDay.Text = lstEmergencyDate.SelectedItem.ToString().Substring(8, 2);

                // 各種ボタンの使用設定
                btnDelete.Enabled = true;
                btnNew.Enabled = true;
                btnSave.Enabled = false;
            }
            else
            {
                txtYear.Text = "";
                txtMonth.Text = "";
                txtDay.Text = "";
            }
        }
    }
}
