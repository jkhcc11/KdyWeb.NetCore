using KdyWeb.BaseInterface;
using KdyWeb.CloudParse;
using KdyWeb.Entity.CloudParse;
using KdyWeb.Service.CloudParse.DiskCloudParse;

namespace KdyWeb.Service.CloudParse
{
    /// <summary>
    /// 解析平台工厂
    /// </summary>
    public class DiskCloudParseFactory
    {
        public static IKdyCloudParseService CreateKdyCloudParseService(string businessFlag, long childUserId)
        {
            switch (businessFlag)
            {
                case CloudParseCookieType.Ali:
                    {
                        return new AliYunCloudParseService(childUserId);
                    }
                case CloudParseCookieType.TyPerson:
                    {
                        return new TyPersonCloudParseService(childUserId);
                    }
                case CloudParseCookieType.TyCrop:
                    {
                        return new TyCropCloudParseService(childUserId);
                    }
                case CloudParseCookieType.BitQiu:
                    {
                        return new StCloudParseService(childUserId);
                    }
                case CloudParseCookieType.Pan139:
                    {
                        return new Pan139CloudParseService(childUserId);
                    }
                default:
                    {
                        throw new KdyCustomException($"{nameof(CreateKdyCloudParseService)},未知业务标识,");
                    }
            }
        }
    }
}
