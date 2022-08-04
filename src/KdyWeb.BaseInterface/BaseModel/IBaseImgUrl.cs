namespace KdyWeb.BaseInterface.BaseModel
{
    /// <summary>
    /// 基础海报Url处理
    /// </summary>
    /// <remarks>
    /// Url需要特殊处理，如果是之前旧的豆瓣等需要转发
    /// </remarks>
    public interface IBaseImgUrl
    {
        /// <summary>
        /// 海报
        /// </summary>
        string VideoImg { get; set; }
    }
}
