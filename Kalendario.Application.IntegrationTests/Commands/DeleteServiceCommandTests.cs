using System;
using System.Threading.Tasks;
using FluentAssertions;
using Kalendario.Application.Commands.Admin;
using Kalendario.Application.Common.Exceptions;
using Kalendario.Application.IntegrationTests.Common;
using Kalendario.Core.Entities;
using NUnit.Framework;

namespace Kalendario.Application.IntegrationTests.Commands;

using static Testing;

public class DeleteServiceCommandTests : TestBase
{
    private Guid _serviceId;

    private DeleteServiceCommand DeleteServiceCommand => new()
    {
        Id = _serviceId
    };
    
    [SetUp]
    public new async Task TestSetUp()
    {
        await base.TestSetUp();
        _serviceId = await AddAsync(Entities.TestService());
    }

    [Test]
    public async Task UnauthenticatedUser_ShouldThrow_AuthenticationException()
    {
        RunAsAnonymousUser();
        await FluentActions.Invoking(() => SendAsync(DeleteServiceCommand)).Should().ThrowAsync<UnauthorizedAccessException>();
        await AssertDeleteFlag(false);
    }
    
    [Test]
    public async Task UnauthorizedUser_ShouldThrow_ForbiddenAccessException()
    {
        await RunAsDefaultUserAsync();
        await FluentActions.Invoking(() => SendAsync(DeleteServiceCommand)).Should().ThrowAsync<ForbiddenAccessException>();
        await AssertDeleteFlag(false);
    }
    
    [Test]
    public async Task AdminOfDifferentAccountHasNoAccess()
    {
        await RunAsAdministratorAsync(typeof(Service), Service.DeleteRole, Constants.RandomAccountId);
        await FluentActions.Invoking(() => SendAsync(DeleteServiceCommand)).Should().ThrowAsync<NotFoundException>();
        await AssertDeleteFlag(false);
    }


    [Test]
    public async Task ValidCommand_ShouldSetDeletedFlagTrue()
    {
        await RunAsAdministratorAsync(typeof(Service), Service.DeleteRole, Constants.CurrentUserAccountId);
        await FluentActions.Invoking(() => SendAsync(DeleteServiceCommand)).Invoke();
        await AssertDeleteFlag(true);
    }

    private async Task AssertDeleteFlag(bool flag)
    {
        var service = await IgnoreQueryFiltersAndFindAsync<Service>(_serviceId);
        Assert.IsNotNull(service);
        Assert.AreEqual(flag, service.IsDeleted);
    }
}