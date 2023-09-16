namespace webdemo.Data
{
    public static class WebHostMigrationsExtension
    {
        /// <summary>
        /// 初始化database方法
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="host"></param>
        /// <param name="sedder"></param>
        /// <returns></returns>
        public static IHost MigrateDbContext<TContext>(this IHost host, Action<TContext, IServiceProvider> sedder)
            where TContext : DemoDbContext
        {
            //创建数据库实例在本区域有效
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();
                try
                {
                    context.Database.Migrate();//初始化database
                    sedder(context, services);
                    logger.LogInformation($"执行DbContext{typeof(TContext).Name} seed 成功");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"执行dbcontext {typeof(TContext).Name}  seed失败");
                }
            }
            return host;
        }
    }
}
