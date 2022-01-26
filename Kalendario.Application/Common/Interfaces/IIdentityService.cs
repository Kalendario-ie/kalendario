using System;
using System.Threading;
using System.Threading.Tasks;
using Kalendario.Core.Infrastructure;

namespace Kalendario.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AddToRoleAsync(string userId, string role);

    Task<bool> RemoveFromRoleAsync(string userId, string role);
    
    Task<bool> AddToAccountAsync(string userId, Guid accountId);

    Task<bool> UpdateUserRoles(ApplicationUser user, CancellationToken cancellationToken);

}