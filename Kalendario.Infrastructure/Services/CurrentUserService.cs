using Kalendario.Application.Common.Interfaces;
using Kalendario.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;

namespace Kalendario.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        var user = httpContextAccessor.HttpContext?.User;

        if (user?.Identity == null) return;

        IsAuthenticated = user.Identity.IsAuthenticated;
        UserId = user.GetUserId();
        AccountId = Guid.TryParse(user.GetAccountId(), out var accountId) ? accountId : Guid.Empty;
        EmployeeId = Guid.TryParse(user.GetEmployeeId(), out var employeeId) ? employeeId : Guid.Empty;
    }

    public string UserId { get; }
    public Guid AccountId { get; }
    public Guid EmployeeId { get; }
    public bool IsAuthenticated { get; }
}