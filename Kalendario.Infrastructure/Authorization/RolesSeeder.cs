using Kalendario.Application.Authorization;
using Kalendario.Core.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Kalendario.Infrastructure.Authorization;

public static class RolesSeeder
{
    public static async Task CreateRoles(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetService<RoleManager<ApplicationRole>>();

        if (roleManager == null)
        {
            throw new Exception("roleManager null");
        }

        foreach (var role in AuthorizationHelper.GetAllRoles())
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new ApplicationRole(role));
            }            
        }

    }
}