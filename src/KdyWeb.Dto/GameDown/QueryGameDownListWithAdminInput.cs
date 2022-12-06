using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity.GameDown;

namespace KdyWeb.Dto.GameDown
{
    /// <summary>
    /// 查询游戏下载列表 input
    /// </summary>
    public class QueryGameDownListWithAdminInput : BasePageInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [KdyQuery(nameof(GameInfoMain.ChineseName), KdyOperator.Like)]
        [KdyQuery(nameof(GameInfoMain.GameName), KdyOperator.Like)]
        public string KeyWord { get; set; }
    }
}
