using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace workschedule.Controls
{
    class HeaderCell

    {
        private int _row;
        /// 
        /// 行
        /// 
        /// 
        /// 
        /// 
        [Category("セル位置")]
        [Description("行")]
        public int Row
        {
            get { return _row; }
            set { _row = value; }
        }

        private int _column;
        /// 
        /// 列
        /// 
        /// 
        /// 
        /// 
        [Category("セル位置")]
        [Description("列")]
        public int Column
        {
            get { return _column; }
            set { _column = value; }
        }

        private int _rowSpan = 1;
        /// 
        /// 結合する行数
        /// 
        /// 
        /// 
        /// 
        [Category("セル結合")]
        [Description("行数")]
        public int RowSpan
        {
            get { return _rowSpan; }
            set { _rowSpan = value; }
        }

        private int _columnSpan = 1;
        /// 
        /// 結合する列数
        /// 
        /// 
        /// 
        /// 
        [Category("セル結合")]
        [Description("列数")]
        public int ColumnSpan
        {
            get { return _columnSpan; }
            set { _columnSpan = value; }
        }

        private System.Drawing.Color _backgroundColor = Color.Empty;
        /// 
        /// セルの背景色
        /// 
        /// 
        /// 
        /// 
        [Category("表示")]
        [Description("セルの背景色")]
        public System.Drawing.Color BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }

        private System.Drawing.Color _foreColor = Color.Empty;
        /// 
        /// テキストの文字色
        /// 
        /// 
        /// 
        /// 
        [Category("表示")]
        [Description("テキストの文字色")]
        public System.Drawing.Color ForeColor
        {
            get { return _foreColor; }
            set { _foreColor = value; }
        }


        private string _text;
        /// 
        /// セルに関連付けられたテキスト
        /// 
        /// 
        /// 
        /// 
        [Category("表示")]
        [Description("セルに関連付けられたテキストです")]
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        private DataGridViewContentAlignment _textAlign;
        /// 
        /// 結合されたセル内でのテキストの位置
        /// 
        /// 
        /// 
        /// 
        [Category("表示")]
        [Description("結合されたセル内のテキストの位置を決定します")]
        public DataGridViewContentAlignment TextAlign
        {
            get { return _textAlign; }
            set { _textAlign = value; }
        }

        private DataGridViewTriState _wrapMode = DataGridViewTriState.NotSet;
        /// 
        /// セルに含まれるテキスト形式の内容が 1 行に収まらないほど長い場合に、次の行に折り返されるか、
        /// 切り捨てられるかを示す値を取得または設定する
        /// 
        /// 
        /// 
        /// 
        [Category("表示")]
        [Description("セル内のテキストが一行に収まらない場合にテキストを折り返す")]
        public DataGridViewTriState WrapMode
        {
            get { return _wrapMode; }
            set { _wrapMode = value; }
        }

        private bool _sortVisible;
        /// 
        /// 結合されている列に並び替えがある場合に並び替えの方向を表示する
        /// 
        /// 
        /// 
        /// 
        [Category("表示")]
        [Description("結合されている列に並び替えがある場合に並び替えの方向を表示する")]
        public bool SortVisible
        {
            get { return _sortVisible; }
            set { _sortVisible = value; }
        }
    }
}