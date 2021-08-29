using System;
using KdyWeb.BaseInterface;
using KdyWeb.BaseInterface.BaseModel;
using KdyWeb.Entity;

namespace KdyWeb.Dto.KdyImg
{
    /// <summary>
    /// 分页查询图床Input
    /// </summary>
    public class QueryKdyImgInput : BasePageInput
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        [KdyQuery(nameof(KdyImgSave.CreatedTime), KdyOperator.GtEqual)]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [KdyQuery(nameof(KdyImgSave.CreatedTime), KdyOperator.LessEqual)]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        [KdyQuery(nameof(KdyImgSave.MainUrl), KdyOperator.Like)]
        [KdyQuery(nameof(KdyImgSave.OneUrl), KdyOperator.Like)]
        [KdyQuery(nameof(KdyImgSave.TwoUrl), KdyOperator.Like)]
        public string KeyWord { get; set; }
    }
}
