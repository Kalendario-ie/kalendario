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

public class GetCustomersRequestTests : TestBase
{
    [Test]
    public async Task UnauthenticatedUsers_ShouldThrow_AuthenticationException()
    {
        RunAsAnonymousUser();
        var query = new GetCustomersQuery();
        await FluentActions.Invoking(() => SendAsync(query)).Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task UnauthorizedUser_ShouldThrow_ForbiddenAccessException()
    {
        await RunAsDefaultUserAsync();
        var query = new GetCustomersQuery();
        await FluentActions.Invoking(() => SendAsync(query)).Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task DefaultQuery_ShouldReturn_CurrentUserAccountCustomers()
    {
        await RunAsAdministratorAsync(typeof(Customer), Customer.ViewRole, Constants.CurrentUserAccountId);
        await AddAsync(Entities.TestCustomer());
        await AddAsync(Entities.TestCustomer());
        await AddAsync(Entities.TestCustomer(Constants.RandomAccountIdString));
        var query = new GetCustomersQuery();
        var result = await FluentActions.Invoking(() => SendAsync(query)).Invoke();
        
        Assert.AreEqual(2, result.TotalCount);
    }
}