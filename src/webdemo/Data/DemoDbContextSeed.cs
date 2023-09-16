namespace webdemo.Data
{
    public class DemoDbContextSeed
    {
        public async Task SeedAsync(DemoDbContext context, IServiceProvider services)
        {
            if (!context.User.Any())
            {
                var defaultUser = new User
                {
                    UserName = "admin",
                    Password = "admin"
                };

                context.Add(defaultUser);
                var resul = context.SaveChanges();
                if (resul > 0)
                {
                    throw new Exception("初始化默认用户失败");
                }
            }
        }
    }
}
