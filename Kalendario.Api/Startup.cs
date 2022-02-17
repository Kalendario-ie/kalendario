using System.ComponentModel;
using FluentValidation.AspNetCore;
using Kalendario.Api.Converters;
using Kalendario.Api.Filters;
using Kalendario.Api.Services;
using Kalendario.Application;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Infrastructure;
using Kalendario.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

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
        services.AddInfrastructure(Configuration)
            .AddApplication();

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddControllers()
            .AddNewtonsoftJson();

        services.AddControllersWithViews(o =>
            {
                o.Filters.Add<ApiExceptionFilterAttribute>();
                TypeDescriptor.AddAttributes(typeof(DateOnly), new TypeConverterAttribute(typeof(DateOnlyTypeConverter)));
            })
            .AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
            })
            .AddFluentValidation(fv =>
            {
                fv.AutomaticValidationEnabled = true;
                fv.RegisterValidatorsFromAssemblyContaining<IKalendarioDbContext>();
            });
        services.AddRazorPages();

        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["controller"]}{e.ActionDescriptor.RouteValues["action"]}");
            options.SupportNonNullableReferenceTypes();
            options.MapType(typeof(TimeSpan), () => new OpenApiSchema
            {
                Type = "string",
                Example = new OpenApiString("00:00:00")
            });
            options.MapType<DateOnly>(() => new OpenApiSchema 
            { 
                Type = "string", 
                Format = "date" 
            });
        });

        services.AddScoped<IImageUploaderService, ImageUploaderService>();
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