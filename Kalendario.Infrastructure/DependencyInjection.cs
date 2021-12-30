using Kalendario.Application.Common.Interfaces;
using Kalendario.Common;
using Kalendario.Core.Infrastructure;
using Kalendario.Infrastructure.Identity;
using Kalendario.Infrastructure.Persistence;
using Kalendario.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kalendario.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString,
        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IKalendarioDbContext>(provider => provider.GetService<ApplicationDbContext>());

        services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddClaimsPrincipalFactory<KalendarioUserClaimsPrincipalFactory>();

        services.AddIdentityServer()
            .AddApiAuthorization<ApplicationUser, ApplicationDbContext>()
            .AddProfileService<ProfileService>();

        services.AddAuthentication()
            .AddIdentityServerJwt();

        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<IIdentityService, IdentityService>();

        services.AddScoped<ICurrentUserService, CurrentUserService>()
            .AddScoped<IDomainEventService, DomainEventService>()
            .AddHttpContextAccessor();

        return services;
    }
}