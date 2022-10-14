using System.ComponentModel.DataAnnotations;
using KdyWeb.BaseInterface;

namespace KdyWeb.Entity.VideoConverts.Enum
{
    /// <summary>
    /// 影片管理者记录 类型
    /// </summary>
    public enum VodManagerRecordType
    {
        /// <summary>
        /// 电影保存
        /// </summary>
        [Display(Name = "电影保存")]
        SaveMove = 1,

        /// <summary>
        /// 电视剧保存
        /// </summary>
        [Display(Name = "电视剧保存")]
        SaveTv = 2,

        /// <summary>
        /// 创建任务
        /// </summary>
        [Display(Name = "创建任务")]
        CreateTask = 5,

        /// <summary>
        /// 更新资料（存在豆瓣）
        /// </summary>
        [Display(Name = "更新资料（存在豆瓣）")]
        UpdateMainInfo = 10,

        /// <summary>
        /// 更新资料（不存在豆瓣）
        /// </summary>
        [Display(Name = "更新资料（不存在豆瓣）")]
        UpdateMainInfoSelf,

        /// <summary>
        /// 录入和反馈
        /// </summary>
        [Display(Name = "录入和反馈")]
        InputAndFeedBack = 15,

        /// <summary>
        /// 接单审核
        /// </summary>
        [Display(Name = "接单审核")]
        Audit = 20,
        /// <summary>
        /// 接单审核多
        /// </summary>
        [Display(Name = "接单审核多")]
        AuditMore = 21,

        /// <summary>
        /// 下架
        /// </summary>
        [Display(Name = "下架")]
        Down = 25
    }

    public static class VodManagerRecordTypeExtension
    {
        /// <summary>
        /// 根据类型获取结算金额
        /// </summary>
        /// <returns></returns>
        public static decimal GetCheckoutAmount(this VodManagerRecordType recordType)
        {
            switch (recordType)
            {
                case VodManagerRecordType.SaveMove:
                    {
                        return 0.25m;
                    }
                case VodManagerRecordType.SaveTv:
                    {
                        return 0.5m;
                    }
                case VodManagerRecordType.CreateTask:
                    {
                        return 0.25m;
                    }
                case VodManagerRecordType.UpdateMainInfo:
                    {
                        return 0.25m;
                    }
                case VodManagerRecordType.UpdateMainInfoSelf:
                    {
                        return 0.5m;
                    }
                case VodManagerRecordType.InputAndFeedBack:
                    {
                        return 0.5m;
                    }
                case VodManagerRecordType.Audit:
                    {
                        return 0.5m;
                    }
                case VodManagerRecordType.Down:
                    {
                        return 0.15m;
                    }
                default:
                    {
                        throw new KdyCustomException("未知类型");
                    }
            }
        }
    }
}
