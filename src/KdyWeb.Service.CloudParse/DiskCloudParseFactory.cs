using KdyWeb.BaseInterface;
using KdyWeb.CloudParse;
using KdyWeb.CloudParse.Input;
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
            //一般用于清缓存
            //todo:新增业务类型这里
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
                case CloudParseCookieType.TyFamily:
                    {
                        return new TyFamilyCloudParseService(childUserId);
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
                case CloudParseCookieType.TxShare:
                    {
                        return new TShareCloudParseService(childUserId);
                    }
                default:
                    {
                        throw new KdyCustomException($"{nameof(CreateKdyCloudParseService)},未知业务标识,");
                    }
            }
        }

        public static IKdyCloudParseService CreateKdyCloudParseService(string businessFlag, BaseConfigInput baseConfigInput)
        {
            //实际调用
            //todo:新增业务类型这里
            switch (businessFlag)
            {
                case CloudParseCookieType.Ali:
                    {
                        return new AliYunCloudParseService(baseConfigInput);
                    }
                case CloudParseCookieType.TyPerson:
                    {
                        return new TyPersonCloudParseService(baseConfigInput);
                    }
                case CloudParseCookieType.TyFamily:
                    {
                        return new TyFamilyCloudParseService(baseConfigInput);
                    }
                case CloudParseCookieType.TyCrop:
                    {
                        return new TyCropCloudParseService(baseConfigInput);
                    }
                case CloudParseCookieType.BitQiu:
                    {
                        return new StCloudParseService(baseConfigInput);
                    }
                case CloudParseCookieType.Pan139:
                    {
                        return new Pan139CloudParseService(baseConfigInput);
                    }
                case CloudParseCookieType.TxShare:
                    {
                        return new TShareCloudParseService(baseConfigInput);
                    }
                default:
                    {
                        throw new KdyCustomException($"{nameof(CreateKdyCloudParseService)},未知业务标识,");
                    }
            }
        }
    }
}
