using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Infrastructure;
using Kalendario.Infrastructure.Identity;
using Kalendario.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kalendario.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddKalendarioAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddClaimsPrincipalFactory<KalendarioUserClaimsPrincipalFactory>();

        services.AddIdentityServer()
            .AddApiAuthorization<ApplicationUser, ApplicationDbContext>()
            .AddProfileService<ProfileService>();

        services.AddAuthentication()
            .AddIdentityServerJwt();

        services.AddScoped<ICurrentUserService, CurrentUserService>()
            .AddHttpContextAccessor();

        return services;
    }
}