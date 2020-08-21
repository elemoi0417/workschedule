using System;
using System.Windows.Forms;

namespace workschedule
{
    public partial class ShiftControler : Form
    {
        MainSchedule frmMainSchedule;

        public ShiftControler(MainSchedule frmMainSchedule_Parent)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            frmMainSchedule = frmMainSchedule_Parent;
        }

        // 勤務種類ボタンクリックイベント
        private void btnWorkKind_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Text)
            {
                case "日勤":
                    frmMainSchedule.ChangeMainGridData(0);
                    break;
                case "夜勤":
                    frmMainSchedule.ChangeMainGridData(1);
                    break;
                case "夜明":
                    frmMainSchedule.ChangeMainGridData(2);
                    break;
                case "休":
                    frmMainSchedule.ChangeMainGridData(3);
                    break;
                case "前公":
                    frmMainSchedule.ChangeMainGridData(4);
                    break;
                case "後公":
                    frmMainSchedule.ChangeMainGridData(5);
                    break;
                case "有休":
                    frmMainSchedule.ChangeMainGridData(6);
                    break;
                case "前有":
                    frmMainSchedule.ChangeMainGridData(7);
                    break;
                case "後有":
                    frmMainSchedule.ChangeMainGridData(8);
                    break;
                // Mod Start WataruT 2020.07.29 代有を公有に文言変更
                //case "代有":
                case "公有":
                // Mod End   WataruT 2020.07.29 代有を公有に文言変更
                    frmMainSchedule.ChangeMainGridData(9);
                    break;
                case "遅出":
                    frmMainSchedule.ChangeMainGridData(10);
                    break;
                case "研修":
                    frmMainSchedule.ChangeMainGridData(11);
                    break;
                case "特休":
                    frmMainSchedule.ChangeMainGridData(12);
                    break;
                case "欠勤":
                    frmMainSchedule.ChangeMainGridData(13);
                    break;
                case "病休":
                    frmMainSchedule.ChangeMainGridData(14);
                    break;
                case "早出":
                    frmMainSchedule.ChangeMainGridData(15);
                    break;
                // Add Start WataruT 2020.07.22 特定の時短勤務用の項目追加
                case "入職前":
                    frmMainSchedule.ChangeMainGridData(16);
                    break;
                case "5.25":
                    frmMainSchedule.ChangeMainGridData(17);
                    break;
                // Add End   WataruT 2020.07.22 特定の時短勤務用の項目追加
                // Add Start WataruT 2020.07.27 特定の時短勤務用の項目追加
                case "2":
                    frmMainSchedule.ChangeMainGridData(18);
                    break;
                case "6":
                    frmMainSchedule.ChangeMainGridData(19);
                    break;
                case "6.25":
                    frmMainSchedule.ChangeMainGridData(20);
                    break;
                case "7":
                    frmMainSchedule.ChangeMainGridData(21);
                    break;
                // Add End   WataruT 2020.07.27 特定の時短勤務用の項目追加
                // Add Start WataruT 2020.08.06 遅刻・早退入力対応
                case "遅刻":
                    frmMainSchedule.ChangeMainGridData(22);
                    break;
                case "早退":
                    frmMainSchedule.ChangeMainGridData(23);
                    break;
                // Add End   WataruT 2020.08.06 遅刻・早退入力対応
            }

            if (frmMainSchedule.piDayCount != frmMainSchedule.piGrdMain_CurrentColumn)
            {
                frmMainSchedule.piGrdMain_CurrentColumn += 1;
                frmMainSchedule.grdMain.CurrentCell = frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn, frmMainSchedule.piGrdMain_CurrentRow];
            }
        }

        // セル移動コントロールイベント
        private void btnMoveCurrentCellControl_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Text)
            {
                case "↑":
                    if (frmMainSchedule.piGrdMain_CurrentRow != 0)
                    {
                        frmMainSchedule.piGrdMain_CurrentRow -= 1;
                    }
                    break;
                case "←":
                    if (frmMainSchedule.piGrdMain_CurrentColumn != 0)
                    {
                        frmMainSchedule.piGrdMain_CurrentColumn -= 1;
                    }
                    break;
                case "→":
                    if (frmMainSchedule.piDayCount != frmMainSchedule.piGrdMain_CurrentColumn)
                    {
                        frmMainSchedule.piGrdMain_CurrentColumn += 1;
                    }
                    break;
                case "↓":
                    if (frmMainSchedule.piScheduleStaffCount != frmMainSchedule.piGrdMain_CurrentRow)
                    {
                        frmMainSchedule.piGrdMain_CurrentRow += 1;
                    }
                    break;
            }
            frmMainSchedule.grdMain.CurrentCell = frmMainSchedule.grdMain[frmMainSchedule.piGrdMain_CurrentColumn, frmMainSchedule.piGrdMain_CurrentRow];

        }
    }
}
