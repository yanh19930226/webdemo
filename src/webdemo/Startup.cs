namespace webdemo
{
    public class Startup
    {
        private const string cookieScheme = "webdemo";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region MySql
            services.AddDbContext<DemoDbContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("MySqlConnection"));
            });
            #endregion

            #region MediatR
            services.AddMediatR(typeof(Startup));
            #endregion

            #region Service
            services.Scan(scan => scan
                 .FromAssemblyOf<Startup>()
                 .AddClasses()
                 .AsMatchingInterface()
                 .WithTransientLifetime());
            #endregion

            #region AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            #endregion

            #region Authentication
            services.AddAuthentication(cookieScheme)
                        .AddCookie(cookieScheme, option =>
                        {
                            option.LoginPath = new PathString("/Home/Login");
                            option.AccessDeniedPath = new PathString("/Home/Deny");
                            option.ExpireTimeSpan = TimeSpan.FromDays(1);
                        }); 
            #endregion

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
