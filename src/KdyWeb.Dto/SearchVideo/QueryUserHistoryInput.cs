using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.SearchVideo;

namespace KdyWeb.Dto.SearchVideo
{
    /// <summary>
    /// 用户播放记录分页查询 Input
    /// </summary>
    public class QueryUserHistoryInput : BasePageInput
    {
        /// <summary>
        /// 用户订阅类型
        /// </summary>
        public UserSubscribeType? UserSubscribeType { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [KdyQuery(nameof(UserHistory.UserName), KdyOperator.Equal)]
        public string UserName { get; set; }
    }
}
