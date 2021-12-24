using FluentValidation.AspNetCore;
using Kalendario.Api.Common;
using Kalendario.Api.Services;
using Kalendario.Application;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Infrastructure;
using Kalendario.Persistence;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true);

builder.Services.AddKalendarioAuthentication(builder.Configuration)
    .AddPersistence(builder.Configuration)
    .AddApplication();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<KalendarioDbContext>();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>()
    .AddHttpContextAccessor();

builder.Services.AddControllers();
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    })
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<IKalendarioDbContext>());
builder.Services.AddRazorPages();

builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");
app.MapControllers();
app.MapRazorPages();

app.MapFallbackToFile("index.html"); ;

app.Run();