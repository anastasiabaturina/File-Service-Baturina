using DotNetEnv;
using File_Service.Service;
using File_Service;
using Microsoft.EntityFrameworkCore;
using Scrypt;
using Servise_file.Service;

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
        services.AddScoped<IServiceFile, ServiceFile>();
        services.AddScoped<IRepository, Repository>();
        services.AddSingleton<ScryptEncoder>();
        services.AddHostedService<AutoDeleteFile>();
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