namespace webdemo
{
    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //SqlSugar
            LoadSqlSugar(builder);

            //注册UnitOfWork
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            //注册BaseRepository
            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerLifetimeScope();
            //注册Services
            builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly()).Where(t => t.Name.EndsWith("Services")).AsSelf()
            .AsImplementedInterfaces().PropertiesAutowired().InstancePerLifetimeScope();
        }

        private void LoadSqlSugar(ContainerBuilder builder)
        {
            string connectionString = AppSettingsConstVars.DbSqlConnection;

            string dbTypeString = AppSettingsConstVars.DbDbType;

            var dbType = dbTypeString == DbType.MySql.ToString() ? DbType.MySql : DbType.SqlServer;

            var connectionConfig = new ConnectionConfig()
            {
                ConnectionString = connectionString,
                DbType = dbType,
                IsAutoCloseConnection = true,
                LanguageType = LanguageType.Chinese,
                InitKeyType = InitKeyType.Attribute,
                ConfigureExternalServices = new ConfigureExternalServices()
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
                }
            };

            builder.Register(
                c => new SqlSugarScope(connectionConfig, db => {
                    //Sql语句Aop
                    db.Aop.OnLogExecuting = (sql, pars) =>
                    {
                        //输出sql,查看执行sql 性能无影响
                        //ConsoleHelper.WriteWarningLine(sql);
                        //获取原生SQL性能OK
                        Console.WriteLine(UtilMethods.GetNativeSql(sql, pars));
                        //获取无参数化SQL 对性能有影响，特别大的SQL参数多的，调试使用
                        //ConsoleHelper.WriteErrorLine(UtilMethods.GetSqlString(DbType.SqlServer, sql, pars));
                    };
                    db.Aop.OnError = (exp) =>//SQL报错
                    {

                    };
                    //增、删、查、改Aop
                    db.Aop.DataExecuting = (oldValue, entityInfo) =>
                    {
                        /*** 列级别事件：插入的每个列都会进事件 ***/
                        if (entityInfo.PropertyName == "CreateTime" && entityInfo.OperationType == DataFilterType.InsertByObject)
                        {
                            //修改CreateTime字段
                            //entityInfo.SetValue(DateTime.Now);

                        }

                        /*** 行级别事件 ：一条记录只会进一次 ***/
                        if (entityInfo.EntityColumnInfo.IsPrimarykey)
                        {
                            //entityInfo.EntityValue 拿到单条实体对象
                        }


                        /*** 列级别事件 ：更新的每一列都会进事件 ***/
                        if (entityInfo.PropertyName == "UpdateTime" && entityInfo.OperationType == DataFilterType.UpdateByObject)
                        {
                            //entityInfo.SetValue(DateTime.Now);//修改UpdateTime字段
                        }

                        /*** 删除生效 （只有行级事件） ***/
                        if (entityInfo.OperationType == DataFilterType.DeleteByObject)
                        {
                        }

                    };

                })
            ).AsSelf().InstancePerLifetimeScope();
        }
    }
}
