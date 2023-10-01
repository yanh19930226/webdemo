namespace webdemo
{
    /// <summary>
    /// 公用配置
    /// </summary>
    public class GlobalConfig
    {
        /// <summary>
        /// 环境Key
        /// </summary>
        public static string EnvironmentKey = "ASPNETCORE_ENVIRONMENT";

        /// <summary>
        /// 环境
        /// </summary>
        public static IHostEnvironment HostingEnvironment { get; set; }

        /// <summary>
        /// 根配置
        /// </summary>
        public static IConfigurationRoot ConfigurationRoot { get; set; }

        /// <summary>
        /// 根容器
        /// </summary>
        public static IServiceProvider ServiceProviderRoot { get; set; }
    }

    /// <summary>
    /// 公用方法
    /// </summary>
    public static class GlobalMethod
    {

    }
}
