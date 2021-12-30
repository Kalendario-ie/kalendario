using System;
using System.Threading.Tasks;

namespace Kalendario.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<bool> IsInRoleAsync(string userId, string role);
}