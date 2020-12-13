using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Dto.Job
{
    /// <summary>
    /// 旧数据迁移Job Input
    /// </summary>
    public class OldMigrationJobInput
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="page">迁移第几页</param>
        /// <param name="pageSize">迁移分页大小</param>
        public OldMigrationJobInput(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }

        /// <summary>
        /// 第几页
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }
    }
}
