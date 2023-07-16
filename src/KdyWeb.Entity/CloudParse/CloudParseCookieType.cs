using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.CloudParse
{
    /// <summary>
    /// 云盘Cookie类型
    /// </summary>
    public class CloudParseCookieType : BaseEntity<long>
    {
        public const int ShowTextLength = 50;
        public const int BusinessFlagLength = 50;
        public const string Ali = "Ali";
        public const string BitQiu = "BitQiu";
        public const string TyPerson = "TyPerson";
        public const string TyFamily = "TyFamily";
        public const string TyCrop = "TyCrop";

        /// <summary>
        /// 显示文案
        /// </summary>
        [StringLength(ShowTextLength)]
        public string ShowText { get; set; }

        /// <summary>
        /// 业务标识
        /// </summary>
        /// <remarks>
        /// 网盘标识
        /// </remarks>
        [StringLength(BusinessFlagLength)]
        public string BusinessFlag { get; set; }
    }
}
