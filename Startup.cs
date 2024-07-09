using DotNetEnv;
using FileService.Automapper;
using FileService.Services;
using Microsoft.EntityFrameworkCore;
using Scrypt;

namespace FileService;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        Env.Load(".env");
        var connection = Environment.GetEnvironmentVariable("DATABASE");

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddDbContext<DocumentContext>(options => options.UseNpgsql(connection));
        services.AddScoped<IFileService, Services.FileService>();
        services.AddScoped<IRepository, Repository>();
        services.AddSingleton<ScryptEncoder>();
        services.AddHostedService<FileCleanupService>();
        services.AddAutoMapper(typeof(ApiMappingProfile));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseMiddleware<ExceptionHandingMiddleware>();
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}