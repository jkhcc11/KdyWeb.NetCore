using System;
using System.Collections.Generic;
using System.Text;
using KdyWeb.BaseInterface.BaseModel;

namespace KdyWeb.Entity.OldVideo
{
    /// <summary>
    /// 旧 用户订阅
    /// </summary>
    public class OldUserSubscribe : BaseEntity<int>
    {
        /// <summary>
        /// 影片Id或其他Id
        /// </summary>
        public int ObjId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }

        public bool Deleted { get; set; }
    }
}
