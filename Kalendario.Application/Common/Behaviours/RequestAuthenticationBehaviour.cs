using System;
using System.Threading;
using System.Threading.Tasks;
using Kalendario.Application.Common.Interfaces;
using MediatR;

namespace Kalendario.Application.Common.Behaviours;

public class RequestAuthenticationBehaviour<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IKalendarioProtectedRequest<TResponse>
{
    private readonly ICurrentUserService _currentUserService;

    public RequestAuthenticationBehaviour(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }
    
    public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        if (!_currentUserService.IsAuthenticated)
        {
            throw new UnauthorizedAccessException();
        }

        return next();
    }
}