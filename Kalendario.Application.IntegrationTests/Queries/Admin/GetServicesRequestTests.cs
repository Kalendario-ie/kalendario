using System;
using System.Threading.Tasks;
using FluentAssertions;
using Kalendario.Application.Common.Exceptions;
using Kalendario.Application.IntegrationTests.Common;
using Kalendario.Application.Queries.Admin;
using Kalendario.Core.Domain;
using NUnit.Framework;

namespace Kalendario.Application.IntegrationTests.Queries.Admin;

using static Testing;

public class GetServicesRequestTests : TestBase
{
    [Test]
    public async Task UnauthenticatedUsers_ShouldThrow_AuthenticationException()
    {
        RunAsAnonymousUser();
        var query = new GetServicesQuery();
        await FluentActions.Invoking(() => SendAsync(query)).Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task UnauthorizedUser_ShouldThrow_ForbiddenAccessException()
    {
        await RunAsDefaultUserAsync();
        var query = new GetServicesQuery();
        await FluentActions.Invoking(() => SendAsync(query)).Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task DefaultQuery_ShouldReturn_CurrentUserAccountCustomers()
    {
        await RunAsAdministratorAsync(typeof(Service), Service.ViewRole, Constants.CurrentUserAccountId);
        await AddAsync(new Service() {Name = "Service 1", AccountId = Constants.CurrentUserAccountId});
        await AddAsync(new Service() {Name = "Service 2", AccountId = Constants.CurrentUserAccountId});
        await AddAsync(new Service() {Name = "Service 1", AccountId = Constants.RandomAccountId});
        var query = new GetServicesQuery();
        var result = await FluentActions.Invoking(() => SendAsync(query)).Invoke();
        
        Assert.AreEqual(2, result.TotalCount);
    }
}