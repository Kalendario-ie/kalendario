using System;
using System.Threading.Tasks;

namespace Kalendario.Application.Common.Interfaces;

public interface ICurrentUserManager
{
    Task<bool> IsInRoleAsync(string role);

    Task<bool> AddToRoleAsync(string role);

    Task<bool> RemoveFromRoleAsync(string role);

    Task<bool> AddToAccountAsync(Guid accountId);
}