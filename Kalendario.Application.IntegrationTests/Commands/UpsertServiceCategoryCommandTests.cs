using System;
using System.Threading.Tasks;
using Castle.Core.Internal;
using FluentAssertions;
using Kalendario.Application.Commands.Admin;
using Kalendario.Application.Common.Exceptions;
using Kalendario.Application.IntegrationTests.Common;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Core.Entities;
using Kalendario.Core.ValueObject;
using NUnit.Framework;

namespace Kalendario.Application.IntegrationTests.Commands;

using static Testing;

public class UpsertServiceCategoryCommandTests : TestBase
{
    private ServiceCategory TestServiceCategory(string accountId = Constants.CurrentUserAccountIdString) => new()
    {
        Name = "Example",
        AccountId = Guid.Parse(accountId),
        Colour = Colour.Blue
    };
    
    private UpsertServiceCategoryCommand ValidCommand(string id = "") => new()
    {
        Id = id.IsNullOrEmpty() ? null : Guid.Parse(id),
        Name = "New Category Name",
        Colour = Colour.Green,
    };

    [Test]
    public async Task UnauthenticatedUser_ShouldThrow_AuthenticationException()
    {
        RunAsAnonymousUser();
        await FluentActions.Invoking(() => SendAsync(ValidCommand())).Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task UnauthorizedUser_ShouldThrow_ForbiddenAccessException()
    {
        await RunAsDefaultUserAsync();
        await FluentActions.Invoking(() => SendAsync(ValidCommand())).Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task UserWithCreateRoleOnly_ShouldGetForbiddenAccess_OnUpdateTry()
    {
        await RunAsAdministratorAsync(typeof(ServiceCategory), ServiceCategory.CreateRole, Constants.CurrentUserAccountId);
        
        var serviceId = await AddAsync(TestServiceCategory());
        var command = ValidCommand(serviceId.ToString());
        
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task UserWithUpdateRoleOnly_ShouldGetForbiddenAccess_OnCreateTry()
    {
        await RunAsAdministratorAsync(typeof(ServiceCategory), ServiceCategory.UpdateRole);
        await FluentActions.Invoking(() => SendAsync(ValidCommand())).Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task Upsert_AnotherAccountServiceCategory_ShouldThrow_NotFoundException()
    {
        await RunAsAdministratorAsync(typeof(ServiceCategory), ServiceCategory.UpdateRole, Constants.CurrentUserAccountId);

        var serviceId = await AddAsync(TestServiceCategory(Constants.RandomAccountIdString));
        var command = ValidCommand(serviceId.ToString());

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();

        var service = await FindAsync<ServiceCategory>(serviceId);
        Assert.AreEqual(serviceId, service.Id);
    }

    [Test]
    public async Task Update_CorrectCommand_UpdatesEntity()
    {
        await RunAsAdministratorAsync(typeof(ServiceCategory), ServiceCategory.UpdateRole, Constants.CurrentUserAccountId);

        var serviceId = await AddAsync(TestServiceCategory());
        var command = ValidCommand(serviceId.ToString());

        var result = await FluentActions.Invoking(() => SendAsync(command)).Invoke();

        var entity = await FindAsync<ServiceCategory>(serviceId);
        AssertResultEqualCommand(result, command);
        AssertEntityEqualCommand(entity, command);
    }

    [Test]
    public async Task Create_CorrectAccount_ServiceCategoryCategory_ShouldCreate()
    {
        await RunAsAdministratorAsync(typeof(ServiceCategory), ServiceCategory.CreateRole, Constants.CurrentUserAccountId);

        var command = ValidCommand();
        
        var result = await FluentActions.Invoking(() => SendAsync(command)).Invoke();

        var entity = await FindAsync<ServiceCategory>(result.Id);
        AssertResultEqualCommand(result, command);
        AssertEntityEqualCommand(entity, command);
    }

    private void AssertResultEqualCommand(ServiceCategoryAdminResourceModel result, UpsertServiceCategoryCommand command)
    {
        Assert.AreEqual(command.Name, result.Name);
        Assert.AreEqual(command.Colour, result.Colour.ToString());
    }

    private void AssertEntityEqualCommand(ServiceCategory service, UpsertServiceCategoryCommand command)
    {
        Assert.AreEqual(command.Name, service.Name);
        Assert.AreEqual(command.Colour, service.Colour.ToString());
    }
}