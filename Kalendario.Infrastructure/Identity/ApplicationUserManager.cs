using Kalendario.Application.Authorization;
using Kalendario.Core.Entities;
using Kalendario.Core.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kalendario.Infrastructure.Identity;

public class ApplicationUserManager : UserManager<ApplicationUser>
{
    public ApplicationUserManager(
        IUserStore<ApplicationUser> store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<ApplicationUser> passwordHasher,
        IEnumerable<IUserValidator<ApplicationUser>> userValidators,
        IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManager<ApplicationUser>> logger) : base(
        store, optionsAccessor, passwordHasher, userValidators,
        passwordValidators, keyNormalizer, errors, services, logger
    )
    {
    }

    public override async Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user,
        string token)
    {
        var result = await base.ConfirmEmailAsync(user,
            token);

        if (result.Succeeded)
        {
            await this.AddToRoleAsync(user, AuthorizationHelper.RoleName(typeof(Account), Account.CreateRole));
        }

        return result;
    }
}