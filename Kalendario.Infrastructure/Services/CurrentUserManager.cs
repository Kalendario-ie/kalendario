using Kalendario.Application.Common.Interfaces;

namespace Kalendario.Infrastructure.Services;

public class CurrentUserManager : ICurrentUserManager
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IIdentityService _identityService;

    public CurrentUserManager(ICurrentUserService currentUserService, IIdentityService identityService)
    {
        _currentUserService = currentUserService;
        _identityService = identityService;
    }
    public Task<bool> IsInRoleAsync(string role)
    {
        return _identityService.IsInRoleAsync(_currentUserService.UserId, role);
    }

    public Task<bool> AddToRoleAsync(string role)
    {
        return _identityService.AddToRoleAsync(_currentUserService.UserId, role);
    }

    public Task<bool> RemoveFromRoleAsync(string role)
    {
        return _identityService.RemoveFromRoleAsync(_currentUserService.UserId, role);
    }

    public async Task<bool> AddToAccountAsync(Guid accountId)
    {
        return await _identityService.AddToAccountAsync(_currentUserService.UserId, accountId);
    }
}