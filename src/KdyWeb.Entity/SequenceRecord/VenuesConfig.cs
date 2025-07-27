using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.SequenceRecord
{
    /// <summary>
    /// 场馆配置
    /// </summary>
    public class VenuesConfig : BaseEntity<long>
    {
        /// <summary>
        /// 场馆配置
        /// </summary>
        /// <param name="venuesName">场馆名称</param>
        /// <param name="shortName">缩写</param>
        public VenuesConfig(string venuesName, string shortName)
        {
            VenuesName = venuesName;
            ShortName = shortName;
            IsEnableGift = true;
        }

        /// <summary>
        /// 场馆名称
        /// </summary>
        public string VenuesName { get; protected set; }

        /// <summary>
        /// 是否启用奖励计算
        /// </summary>
        public bool IsEnableGift { get; protected set; }

        /// <summary>
        /// 缩写
        /// </summary>
        public string ShortName { get; protected set; }

        /// <summary>
        /// 禁用奖励计算
        /// </summary>
        public void Ban()
        {
            IsEnableGift = false;
        }
    }
}
