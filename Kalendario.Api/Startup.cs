using FluentValidation.AspNetCore;
using Kalendario.Api.Common;
using Kalendario.Api.Services;
using Kalendario.Application;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Infrastructure;
using Kalendario.Persistence;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;

namespace Kalendario.Api;

public class Startup
{
    private IServiceCollection _services;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        Environment = environment;
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }
    public IWebHostEnvironment Environment { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        
        services.AddKalendarioAuthentication(Configuration)
            .AddPersistence(Configuration)
            .AddApplication();

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddHealthChecks()
            .AddDbContextCheck<KalendarioDbContext>();

        services.AddScoped<ICurrentUserService, CurrentUserService>()
            .AddHttpContextAccessor();

        services.AddControllers();
        services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            })
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<IKalendarioDbContext>());
        services.AddRazorPages();

        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app)
    {
        // Configure the HTTP request pipeline.
        if (Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }


        app.UseCustomExceptionHandler();
        app.UseHealthChecks("/health");
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthentication();
        app.UseIdentityServer();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");
            endpoints.MapControllers();
            endpoints.MapRazorPages();

            endpoints.MapFallbackToFile("index.html");
        });
    }
}