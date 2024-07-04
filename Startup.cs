using DotNetEnv;
using FileService.Automapper;
using FileService.Service;
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
        services.AddDbContext<Context>(options => options.UseNpgsql(connection));
        services.AddScoped<IFileService, Service.FileService>();
        services.AddScoped<IRepository, Repository>();
        services.AddSingleton<ScryptEncoder>();
        services.AddHostedService<AutoDeleteFile>();
        services.AddAutoMapper(typeof(MapFile));
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

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}