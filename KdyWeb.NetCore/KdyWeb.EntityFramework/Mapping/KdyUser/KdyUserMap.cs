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
    public class KdyUserMap : KdyBaseMap<KdyUser, int>
    {
        public override void MapperConfigure(EntityTypeBuilder<KdyUser> builder)
        {
            builder.HasData(
                new KdyUser()
                {
                    Id = 1,
                    KdyRoleId = 3,
                    UserName = "admin",
                    UserEmail = "137651076@qq.com",
                    UserNick = "管理员",
                    UserPwd = $"123456{KdyWebConst.UserSalt}".Md5Ext(),
                    CreatedTime = DateTime.Now
                },
                new KdyUser()
                {
                    Id = 2,
                    KdyRoleId = 1,
                    UserName = "test",
                    UserEmail = "123456@qq.com",
                    UserNick = "普通用户测试",
                    UserPwd = $"123456{KdyWebConst.UserSalt}".Md5Ext(),
                    CreatedTime = DateTime.Now
                }
                );

            builder.Property(a => a.KdyRoleId).HasDefaultValue(1);
        }
    }
}
