using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Kalendario.Core.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace Kalendario.Infrastructure.Identity;

public class ProfileService : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfileService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);
        context.IssuedClaims.AddRange(ClaimsHelper.UserClaims(user));
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);
        context.IsActive = (user != null);
    }
}