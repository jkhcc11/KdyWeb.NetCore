using System;
using KdyWeb.Entity;
using KdyWeb.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KdyWeb.EntityFramework.Mapping
{
    /// <summary>
    /// 注册用户 Map
    /// </summary>
    public class KdyUserMap : KdyBaseMap<KdyUser, long>
    {
        public override void MapperConfigure(EntityTypeBuilder<KdyUser> builder)
        {
            //var test1 = new KdyUser("admin", "管理员", "137651076@qq.com", 3)
            //{
            //    Id = 1
            //};
            //KdyUser.SetPwd(test1, "123456");
            //var test2 = new KdyUser("test", "普通用户测试", "123456@qq.com", 1)
            //{
            //    Id = 2
            //};
            //KdyUser.SetPwd(test2, "123456");
            //builder.HasData(test1, test2);

            builder.Property(a => a.KdyRoleId).HasDefaultValue(1);
        }
    }
}
