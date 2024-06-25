namespace KdyWeb.Job.Extensions
{
    public static class KdyJobExtension
    {
        ///// <summary>
        ///// 初始化旧版Job实例
        ///// </summary>
        //public static IServiceCollection AddOldJob(this IServiceCollection services)
        //{
        //    //加载当前项目程序集
        //    var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "Kdy*.dll").Select(Assembly.LoadFrom).ToArray();
        //    //所有程序集类型声明
        //    var allTypes = new List<System.Type>();
        //    foreach (var itemAssemblies in assemblies)
        //    {
        //        allTypes.AddRange(itemAssemblies.GetTypes());
        //    }

        //    //公用的接口
        //    var baseType = typeof(IKdyJobFlag);
        //    //过滤需要用到的服务声明接口
        //    var useType = allTypes.Where(a => baseType.IsAssignableFrom(a) && a.IsAbstract == false).ToList();
        //    foreach (var item in useType)
        //    {
        //        //该服务所属接口
        //        var currentInterface = item.GetInterfaces().FirstOrDefault(a => a.Name.EndsWith(item.Name));
        //        if (currentInterface == null)
        //        {
        //            continue;
        //        }

        //        //每次请求，都获取一个新的实例。同一个请求获取多次会得到相同的实例
        //        services.AddScoped(currentInterface, item);
        //    }

        //    return services;
        //}
    }
}
