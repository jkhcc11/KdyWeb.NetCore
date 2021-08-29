
namespace KdyWeb.BaseInterface.BaseModel
{
    /// <summary>
    /// 选项 Out
    /// </summary>
    public class SelectedItemOut
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="text">显示</param>
        /// <param name="value">值</param>
        public SelectedItemOut(string text, string value)
        {
            Text = text;
            Value = value;
        }

        /// <summary>
        /// 显示
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }
}
