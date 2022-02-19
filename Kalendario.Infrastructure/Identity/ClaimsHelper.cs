using System.Security.Claims;
using Kalendario.Core.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace Kalendario.Infrastructure.Identity;

public static class ClaimsHelper
{
    public static async Task<IEnumerable<Claim>> UserClaims(ApplicationUser user, UserManager<ApplicationUser> userManager)
    {
        var roles = await userManager.GetRolesAsync(user);
        return new Claim[]
        {
            new(ExtraClaimTypes.AccountId, user.AccountId.ToString() ?? string.Empty),
            new(ExtraClaimTypes.EmployeeId, user.EmployeeId.ToString() ?? string.Empty),
            new(ExtraClaimTypes.UserRoles, roles.Aggregate((role, result) => $"{role},{result}") ?? string.Empty)
        };
    }
}