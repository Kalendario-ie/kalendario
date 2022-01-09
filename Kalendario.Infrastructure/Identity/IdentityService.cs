using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);
        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AddToRoleAsync(string userId, string role)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            return false;
        }
        var result = await _userManager.AddToRoleAsync(user, role);
        return result.Succeeded;
    }
    
    
    public async Task<bool> RemoveFromRoleAsync(string userId, string role)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            return false;
        }
        var result = await _userManager.RemoveFromRoleAsync(user, role);
        return result.Succeeded;
    }


    public async Task<bool> AddToAccountAsync(string userId, Guid accountId)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            return false;
        }

        user.AccountId = accountId;
        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }
}