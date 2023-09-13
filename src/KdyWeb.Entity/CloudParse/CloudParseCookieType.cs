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
        public const string Pan139 = "Pan139";

        /// <summary>
        /// 云盘Cookie类型
        /// </summary>
        /// <param name="showText">显示文案</param>
        /// <param name="businessFlag">业务标识</param>
        public CloudParseCookieType(string showText, string businessFlag)
        {
            ShowText = showText;
            BusinessFlag = businessFlag;
        }

        /// <summary>
        /// 显示文案
        /// </summary>
        [StringLength(ShowTextLength)]
        public string ShowText { get; protected set; }

        /// <summary>
        /// 业务标识
        /// </summary>
        /// <remarks>
        /// 网盘标识
        /// </remarks>
        [StringLength(BusinessFlagLength)]
        public string BusinessFlag { get; protected set; }

        /// <summary>
        /// 是否需要服务器Cookie
        /// </summary>
        /// <param name="businessFlag">业务标识</param>
        /// <returns></returns>
        public static bool IsNeedServerCookie(string businessFlag)
        {
            switch (businessFlag)
            {
                case TyCrop:
                case TyPerson:
                case TyFamily:
                    {
                        return true;
                    }
                default:
                    {
                        return false;
                    }
            }
        }
    }
}
