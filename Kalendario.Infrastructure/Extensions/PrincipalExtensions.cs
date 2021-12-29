using System.Security.Claims;
using Kalendario.Infrastructure.Identity;

namespace Kalendario.Infrastructure.Extensions;

public static class PrincipalExtensions
{
    public static string GetAccountId(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(ExtraClaimTypes.AccountId)?.Value;
    }

    public static string GetUserId(this ClaimsPrincipal principal)
    {
        return principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    }

}