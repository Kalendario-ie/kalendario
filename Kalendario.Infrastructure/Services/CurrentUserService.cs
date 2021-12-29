﻿using Kalendario.Application.Common.Interfaces;
using Kalendario.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;

namespace Kalendario.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        var user = httpContextAccessor.HttpContext?.User;

        if (user == null) return;

        IsAuthenticated = true;
        UserId = Guid.Parse(user.GetUserId());
        AccountId = Guid.Parse(user.GetAccountId());
    }

    public Guid UserId { get; }
    public Guid AccountId { get; }
    public bool IsAuthenticated { get; }
}