using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace Kalendario.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
        private readonly UserManager<ApplicationUser> _userManager;

    public IdentityService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);
        return user != null && await _userManager.IsInRoleAsync(user, role);
    }
}