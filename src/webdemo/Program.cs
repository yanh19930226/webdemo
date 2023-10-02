var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConfig(builder.Configuration, builder.Environment);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new AutofacModuleRegister());
});

builder.Build().SetupMiddleware().Run();

