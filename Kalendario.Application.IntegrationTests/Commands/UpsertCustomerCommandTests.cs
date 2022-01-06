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

public class UpsertCustomerCommandTests : TestBase
{
    private UpsertCustomerCommand ValidCommand(string id = "") => new()
    {
        Id = id.IsNullOrEmpty() ? null : Guid.Parse(id),
        Name = "New Category Name",
        PhoneNumber = "9834012321984",
        Email = "teste2@test.com",
        Warning = "this is another warning"
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
        await RunAsAdministratorAsync(typeof(Customer), Customer.CreateRole, Constants.CurrentUserAccountId);
        
        var serviceId = await AddAsync(Entities.TestCustomer());
        var command = ValidCommand(serviceId.ToString());
        
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task UserWithUpdateRoleOnly_ShouldGetForbiddenAccess_OnCreateTry()
    {
        await RunAsAdministratorAsync(typeof(Customer), Customer.UpdateRole);
        await FluentActions.Invoking(() => SendAsync(ValidCommand())).Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task Upsert_AnotherAccountCustomer_ShouldThrow_NotFoundException()
    {
        await RunAsAdministratorAsync(typeof(Customer), Customer.UpdateRole, Constants.CurrentUserAccountId);

        var serviceId = await AddAsync(Entities.TestCustomer(Constants.RandomAccountIdString));
        var command = ValidCommand(serviceId.ToString());

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();

        var service = await FindAsync<Customer>(serviceId);
        Assert.AreEqual(serviceId, service.Id);
    }

    [Test]
    public async Task Update_CorrectCommand_UpdatesEntity()
    {
        await RunAsAdministratorAsync(typeof(Customer), Customer.UpdateRole, Constants.CurrentUserAccountId);

        var serviceId = await AddAsync(Entities.TestCustomer());
        var command = ValidCommand(serviceId.ToString());

        var result = await FluentActions.Invoking(() => SendAsync(command)).Invoke();

        var entity = await FindAsync<Customer>(serviceId);
        AssertResultEqualCommand(result, command);
        AssertEntityEqualCommand(entity, command);
    }

    [Test]
    public async Task Create_CorrectAccount_CustomerCategory_ShouldCreate()
    {
        await RunAsAdministratorAsync(typeof(Customer), Customer.CreateRole, Constants.CurrentUserAccountId);

        var command = ValidCommand();
        
        var result = await FluentActions.Invoking(() => SendAsync(command)).Invoke();

        var entity = await FindAsync<Customer>(result.Id);
        AssertResultEqualCommand(result, command);
        AssertEntityEqualCommand(entity, command);
    }

    private void AssertResultEqualCommand(CustomerAdminResourceModel result, UpsertCustomerCommand command)
    {
        Assert.AreEqual(command.Name, result.Name);
        Assert.AreEqual(command.Email, result.Email);
        Assert.AreEqual(command.PhoneNumber, result.PhoneNumber);
        Assert.AreEqual(command.Warning, result.Warning);
    }

    private void AssertEntityEqualCommand(Customer entity, UpsertCustomerCommand command)
    {
        Assert.AreEqual(command.Name, entity.Name);
        Assert.AreEqual(command.Email, entity.Email);
        Assert.AreEqual(command.PhoneNumber, entity.PhoneNumber);
        Assert.AreEqual(command.Warning, entity.Warning);
    }
}