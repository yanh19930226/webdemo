namespace webdemo
{
    /// <summary>
    /// SevicesExtension
    /// </summary>
    public static class SevicesExtension
    {

        private const string cookieScheme = "webdemo";
        /// <summary>
        /// 添加配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration Configuration, IWebHostEnvironment webHostEnvironment)
        {
            services
                .AddWeb(webHostEnvironment)
                .AddAutoMapper()
                .AddServices()
                .Auth();

            return services;
        }

        public static IServiceCollection AddWeb(this IServiceCollection services, IWebHostEnvironment webHostEnvironment)
        {
            services.AddSingleton(new AppSettingsHelper(webHostEnvironment.ContentRootPath));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddControllersWithViews();
            services.AddMediatR(typeof(Program));
            return services;
        }

        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.Scan(scan => scan
                   .FromAssemblyOf<Program>()
                   .AddClasses()
                   .AsMatchingInterface()
                   .WithTransientLifetime());
            return services;

        }

        public static IServiceCollection Auth(this IServiceCollection services)
        {
            services.AddAuthentication(cookieScheme)
                        .AddCookie(cookieScheme, option =>
                        {
                            option.LoginPath = new PathString("/Home/Login");
                            option.AccessDeniedPath = new PathString("/Home/Deny");
                            option.ExpireTimeSpan = TimeSpan.FromDays(1);
                        });
            return services;
        }
    }
}
