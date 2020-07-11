namespace workschedule.Functions
{
    public class ItemSet
    {
        // DisplayMemberとValueMemberをプロパティで指定
        public string ItemDisp { get; set; }
        public string ItemValue { get; set; }

        // プロパティをコンストラクタでセット
        public ItemSet(string itemDisp, string itemValue)
        {
            ItemDisp = itemDisp;
            ItemValue = itemValue;
        }
    }
}
