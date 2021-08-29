using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 用户收藏查询 Input
    /// </summary>
    public class QueryUserSubscribeInput : BasePageInput
    {
        /// <summary>
        /// 用户订阅类型
        /// </summary>
        public UserSubscribeType? UserSubscribeType { get; set; }

        /// <summary>
        /// 用户邮箱
        /// </summary>
        [Required(ErrorMessage = "邮箱号必填")]
        [KdyQuery(nameof(UserSubscribe.UserEmail), KdyOperator.Equal)]
        public string UserEmail { get; set; }
    }
}
