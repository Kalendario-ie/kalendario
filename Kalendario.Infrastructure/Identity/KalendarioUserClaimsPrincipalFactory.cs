using System.Security.Claims;
using Kalendario.Core.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Kalendario.Infrastructure.Identity;

public class KalendarioUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
{
    public KalendarioUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager,
        IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
    {
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        identity.AddClaims(await ClaimsHelper.UserClaims(user, UserManager));
        return identity;
    }
}