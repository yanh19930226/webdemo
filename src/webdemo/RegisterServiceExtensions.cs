using SqlSugar;
using webdemo.Models.Domain.System;

namespace webdemo
{
    public static class RegisterServiceExtensions
    {
        public static WebApplication SetupMiddleware(this WebApplication app)
        {
            GlobalConfig.ServiceProviderRoot = app.Services;

            app.Services.InitDb();

            if (app.Environment.IsDevelopment())
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

            return app;
        }

        public static void InitDb(this IServiceProvider serviceProvider)
        {
            var connf = serviceProvider.GetRequiredService<IConfiguration>();

            if (connf.GetValue<bool>("ConnectionStrings:Init"))
            {
                ConnectionConfig connConfig = new ConnectionConfig();
                connConfig.ConnectionString = connf.GetSection("ConnectionStrings:SqlConnection").Value;
                connConfig.DbType = SqlSugar.DbType.MySql;
                connConfig.InitKeyType = InitKeyType.Attribute;
                connConfig.ConfigureExternalServices = new ConfigureExternalServices()
                {
                    //处理列名
                    EntityService = (x, p) =>
                    {
                        //ToUnderLine驼峰转下划线方法
                        p.DbColumnName = UtilMethods.ToUnderLine(p.DbColumnName);
                        //支持string?和string  
                        if (p.IsPrimarykey == false && new NullabilityInfoContext().Create(x).WriteState is NullabilityState.Nullable)
                        {
                            p.IsNullable = true;
                        }
                    },
                    //处理表名
                    EntityNameService = (x, p) =>
                    {
                        p.DbTableName = UtilMethods.ToUnderLine(p.DbTableName);//ToUnderLine驼峰转下划线方法
                    }
                };

                IEnumerable<Type> types = Assembly.GetExecutingAssembly().GetTypes()
                                          .Where(t => t.GetInterfaces().Contains(typeof(IBase))).ToArray();

                bool isBackup = false; //是否备份

                using (SqlSugarScope Client = new SqlSugarScope(connConfig, db => {
                    db.Aop.OnLogExecuting = (sql, pars) =>
                    {
                        Console.WriteLine(sql);
                    };
                }))
                {
                    Client.DbMaintenance.CreateDatabase();//创建数据库
                    if (isBackup)
                    {
                        Client.CodeFirst.BackupTable().InitTables(types.ToArray());
                    }
                    else
                    {
                        Client.CodeFirst.InitTables(types.ToArray());
                    }
                    User user = new User();
                    user.UserName = "yh";
                    user.Password = "yh";
                    if (Client.Insertable(user).ExecuteCommand()>0)
                    {
                        Console.WriteLine("init success");
                    }
                }
            }
        }
    }
}
