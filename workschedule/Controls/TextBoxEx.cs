using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace workschedule
{
    public class TextBoxEx : TextBox
    {
        /// <summary>
        /// テキストが空の場合に表示する文字列を取得・設定します。
        /// </summary>
        [Category("表示")]
        [DefaultValue("")]
        [Description("テキストが空の場合に表示する文字列です。")]
        [RefreshProperties(RefreshProperties.Repaint)]

        public string WatermarkText
        {
            get { return _watermarkText; }
            set
            {
                _watermarkText = value;
                this.Invalidate();
            }
        }

        private string _watermarkText = ""; //ウォーターマーク表示内容text

        ///<summary>
        ///描画拡張（テキスト未設定時、ウォーターマークを描画）
        ///</summary>
        ///<param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            const int WM_PAINT = 0x000F;
            base.WndProc(ref m);
            if (m.Msg == WM_PAINT)
            {
                using (Graphics g = Graphics.FromHwnd(this.Handle))
                {   
                    if (string.IsNullOrEmpty(this.Text) && string.IsNullOrEmpty(WatermarkText) == false)
                    {
                        //テキストボックス内の適切な座標に描画
                        Rectangle rect = this.ClientRectangle;
                        rect.Offset(1, 1);
                        TextRenderer.DrawText(g, WatermarkText, this.Font, rect, SystemColors.ControlDark, TextFormatFlags.Top | TextFormatFlags.HorizontalCenter);
                    }

                    //枠の下線のみ印字
                    drawUnderLine();

                    this.AutoSize = false;
                }
            }
        }
        /// <summary>
        /// Text変更時
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            drawUnderLine();
        }

        /// <summary>
        /// マウスでクリックされた(ボタンが下がったとき)
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            drawUnderLine();
        }

        /// <summary>
        /// フォーカスが移ったとき
        /// </summary>
        /// <param name="e"></param>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            drawUnderLine();
        }

        private void drawUnderLine()
        {
            using (Graphics g = this.CreateGraphics())
            {
                //下だけボーダー表示
                g.DrawLine(new Pen(Color.FromArgb(184, 177, 171)), 0, this.Height - 1, this.Width, this.Height - 1);
            }
        }
    }
}


