using System.Security.Claims;
using Kalendario.Core.Infrastructure;

namespace Kalendario.Infrastructure.Identity;

public static class ClaimsHelper
{
    public static IEnumerable<Claim> UserClaims(ApplicationUser user)
    {
        return new Claim[]
        {
            new(ExtraClaimTypes.AccountId, user.AccountId.ToString() ?? string.Empty)
        };
    }
}