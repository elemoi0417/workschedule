using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using workschedule.Functions;

class ComboBoxEx : ComboBox
{
    private StringAlignment _textAlign = StringAlignment.Center;
    [Description("String Alignment")]
    [Category("CustomFonts")]
    [DefaultValue(typeof(StringAlignment))]
    public StringAlignment TextAlign
    {
        get { return _textAlign; }
        set
        {
            _textAlign = value;
        }
    }
    private int _textYOffset = 0;
    [Description("When using a non-centered TextAlign, you may want to use TextYOffset to manually center the Item text.")]
    [Category("CustomFonts")]
    [DefaultValue(typeof(int))]
    public int TextYOffset
    {
        get { return _textYOffset; }
        set
        {
            _textYOffset = value;
        }
    }
    public ComboBoxEx()
    {
        // コントロールを自由に編集できるようにする
        this.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
        // 手入力不可とする
        this.DropDownStyle = ComboBoxStyle.DropDownList;
        // テキスト描画処理
        this.DrawItem +=
            new DrawItemEventHandler(ComboBox_DrawItem);
        // サイズ個別指定用
        this.MeasureItem +=
            new MeasureItemEventHandler(ComboBox_MeasureItem);
    }
    /// <summary>
    /// テキストボックスに文字列を描画する
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ComboBox_DrawItem(object sender, DrawItemEventArgs e)
    {
        e.DrawBackground();
        if (e.Index >= 0)
        {
            StringFormat sf = new StringFormat();
            sf.LineAlignment = _textAlign;
            sf.Alignment = _textAlign;
            Brush brush = new SolidBrush(this.ForeColor);
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                brush = SystemBrushes.HighlightText;
            
            //ItemSetの場合は別処理
            if(Items[e.Index] is workschedule.Functions.ItemSet)
            {
                ItemSet itemSet = Items[e.Index] as ItemSet;
                e.Graphics.DrawString(itemSet.ItemDisp, this.Font, brush,
                    new RectangleF(e.Bounds.X, e.Bounds.Y + _textYOffset, e.Bounds.Width, e.Bounds.Height), sf);
            }
            else
            e.Graphics.DrawString(this.Items[e.Index].ToString(), this.Font, brush,
                new RectangleF(e.Bounds.X, e.Bounds.Y + _textYOffset, e.Bounds.Width, e.Bounds.Height), sf);
        }
    }
    /// <summary>
    /// テキストボックスのサイズ指定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ComboBox_MeasureItem(object sender, System.Windows.Forms.MeasureItemEventArgs e)
    {
        // 必要に応じて設定
    }
}