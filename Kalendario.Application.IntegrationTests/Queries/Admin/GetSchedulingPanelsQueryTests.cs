using System;
using System.Threading.Tasks;
using FluentAssertions;
using Kalendario.Application.Common.Exceptions;
using Kalendario.Application.IntegrationTests.Common;
using Kalendario.Application.Queries.Admin;
using Kalendario.Core.Entities;
using NUnit.Framework;

namespace Kalendario.Application.IntegrationTests.Queries.Admin;

using static Testing;

public class GetSchedulingPanelsQueryTests : TestBase
{
    private GetSchedulingPanelsQuery Query => new();
    
    [Test]
    public async Task UnauthenticatedUsers_ShouldThrow_AuthenticationException()
    {
        RunAsAnonymousUser();
        await FluentActions.Invoking(() => SendAsync(Query)).Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task UnauthorizedUser_ShouldThrow_ForbiddenAccessException()
    {
        await RunAsDefaultUserAsync();
        await FluentActions.Invoking(() => SendAsync(Query)).Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task DefaultQuery_ShouldReturn_CurrentUserAccountCustomers()
    {
        await RunAsAdministratorAsync(typeof(SchedulingPanel), SchedulingPanel.ViewRole, Constants.CurrentUserAccountId);
        await AddAsync(Entities.TestSchedulingPanel());
        await AddAsync(Entities.TestSchedulingPanel());
        await AddAsync(Entities.TestSchedulingPanel(Constants.RandomAccountIdString));
        var result = await FluentActions.Invoking(() => SendAsync(Query)).Invoke();
        
        Assert.AreEqual(2, result.TotalCount);
    }
}