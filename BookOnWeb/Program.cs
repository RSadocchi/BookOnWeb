using BookOnWeb.Data;
using BookOnWeb.Domain.Implementations;
using BookOnWeb.Domain.Interfaces;
using BookOnWeb.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NLog;
using NLog.Web;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
var logger = NLog.LogManager
    .Setup()
    .LoadConfigurationFromAppSettings(environment: environment, nlogConfigSection: "NLog", optional: false, reloadOnChange: true)
    .GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
    builder.Host.UseNLog();

    builder.Configuration
       .SetBasePath(builder.Environment.ContentRootPath)
       .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
       .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
       .AddJsonFile($"nlog.json", optional: false, reloadOnChange: true)
       .AddEnvironmentVariables();

    builder.Services.Configure<ApplicationOptions>(builder.Configuration.GetSection(nameof(ApplicationOptions)));

    builder.Services.AddDbContext<AppDbContext>(o =>
        o.UseSqlServer(
            builder.Configuration.GetConnectionString("Application"),
            opt => opt.MigrationsAssembly(typeof(Program).Assembly.GetName().Name))
    );

    builder.Services.TryAddSingleton<IConfiguration>(provider => builder.Configuration);

    builder.Services.TryAddScoped<ILibroRepository, LibroRepository>();
    builder.Services.TryAddScoped<IAutoreRepository, AutoreRepository>();
    builder.Services.TryAddScoped<IAppService, AppService>();

    // Add services to the container.
    builder.Services.AddControllersWithViews();

    /**************************************************************/

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    if (bool.TryParse((builder.Configuration["ApplicationOptions:MigrateOnStart"] ?? "false"), out bool _migratOnStart) && _migratOnStart)
        using (var serviceScoped = app.Services.CreateScope())
            using (var ctx = serviceScoped.ServiceProvider.GetService<AppDbContext>())
                if (ctx is not null)
                {
                    ctx.ConnectionString = builder.Configuration.GetConnectionString("Application")!;
                    ctx.Database.Migrate();
                }

    app.UseHttpsRedirection();
    app.UseRouting();

    app.UseAuthorization();

    app.MapStaticAssets();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();


    app.Run();
}
catch (Exception ex)
{
    logger.Fatal(ex, $"Application fatal exception on running");
    throw;
}
