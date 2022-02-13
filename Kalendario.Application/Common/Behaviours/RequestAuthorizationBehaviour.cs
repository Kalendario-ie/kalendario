using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Kalendario.Application.Common.Exceptions;
using Kalendario.Application.Common.Extensions;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using MediatR;

namespace Kalendario.Application.Common.Behaviours;

public class RequestAuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IIdentityService _identityService;
    private readonly ICurrentUserService _currentUserService;

    public RequestAuthorizationBehaviour(IIdentityService identityService, ICurrentUserService currentUserService)
    {
        _identityService = identityService;
        _currentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {

        var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

        if (!authorizeAttributes.Any())
            return await next();

        var authorizeAttributesWithRoles = authorizeAttributes.Where(a => a.Roles.Any());

        if (!authorizeAttributesWithRoles.Any())
            return await next();
        
        if (!_currentUserService.IsAuthenticated)
        {
            throw new UnauthorizedAccessException();
        }

        var authorized = await authorizeAttributesWithRoles
            .Select(a => a.Roles)
            .SelectMany(roles => roles)
            .Select(role => _identityService.IsInRoleAsync(_currentUserService.UserId, role.Trim()))
            .AnyAsync();

        // Must be a member of at least one role in roles
        if (!authorized)
        {
            throw new ForbiddenAccessException();
        }

        // User is authorized / authorization not required
        return await next();
    }
}