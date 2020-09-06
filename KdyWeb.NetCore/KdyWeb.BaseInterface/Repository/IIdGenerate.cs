namespace KdyWeb.BaseInterface.Repository
{
    /// <summary>
    /// Id生成 接口
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IIdGenerate<TKey>
    {
        /// <summary>
        /// Id具体生成
        /// </summary>
        /// <returns></returns>
        TKey Create();
    }
}
