using System.Windows.Forms;

namespace workschedule
{
    public partial class ProgressDialogForm : Form
    {
        public ProgressDialogForm()
        {
            InitializeComponent();

            // コントロールボックスを表示しない
            this.ControlBox = !this.ControlBox;
        }
    }
}
