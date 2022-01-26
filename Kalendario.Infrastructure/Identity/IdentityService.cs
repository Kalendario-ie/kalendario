using Kalendario.Application.Authorization;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IKalendarioDbContext _context;

    public IdentityService(UserManager<ApplicationUser> userManager, IKalendarioDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);
        return user != null && await IsInRoleAsync(user, role);
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

    public async Task<bool> UpdateUserRoles(ApplicationUser user, CancellationToken cancellationToken)
    {
        if (!user.RoleGroupId.HasValue) return true;
        
        var newRoles = await _context.Roles
            .Include(r => r.RoleGroups)
            .Where(r => r.RoleGroups
                .Select(rg => rg.Id)
                .Contains(user.RoleGroupId.Value)
            )
            .Select(r => r.Name)
            .ToListAsync(cancellationToken);

        var oldRoles = await _userManager.GetRolesAsync(user);

        var removed = await _userManager.RemoveFromRolesAsync(user, oldRoles);
        var updated = await _userManager.AddToRolesAsync(user, newRoles);
        return removed.Succeeded && updated.Succeeded;
    }

    private async Task<bool> IsInRoleAsync(ApplicationUser user, string role)
    {
        return await _userManager.IsInRoleAsync(user, role) ||
               await _userManager.IsInRoleAsync(user, AuthorizationHelper.MasterRole);
    }
}