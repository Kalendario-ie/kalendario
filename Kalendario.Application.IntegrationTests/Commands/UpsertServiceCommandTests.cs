﻿using System;
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

public class UpsertServiceCommandTests : TestBase
{
    private const string CurrentUserAccountServiceCategoryId = "5D42F6EF-08DC-4254-985B-D0D1146DA638";
    private const string DifferentAccountServiceCategoryId = "D39F39BC-30F3-41BE-8E42-DC800DE47BB1";

    private Service TestService(string accountId = Constants.CurrentUserAccountIdString) => new()
    {
        Name = "Example",
        AccountId = Guid.Parse(accountId),
        Description = "Description Example",
        Price = 20.1,
        Duration = TimeSpan.FromHours(1),
        ServiceCategoryId = Guid.Parse(CurrentUserAccountServiceCategoryId)
    };

    private UpsertServiceCommand ValidCommand(string id = "",
        string serviceCategoryId = CurrentUserAccountServiceCategoryId) => new()
    {
        Id = id.IsNullOrEmpty() ? null : Guid.Parse(id),
        Name = "new service name",
        Description = "new service description",
        Price = 100.12,
        Duration = TimeSpan.FromHours(4),
        ServiceCategoryId = Guid.Parse(serviceCategoryId)
    };

    [SetUp]
    public new async Task TestSetUp()
    {
        await base.TestSetUp();
        await AddAsync(new ServiceCategory
        {
            Name = "category 1",
            Id = Guid.Parse(CurrentUserAccountServiceCategoryId), AccountId = Constants.CurrentUserAccountId,
            Colour = Colour.Blue
        });
        await AddAsync(new ServiceCategory
        {
            Name = "category 1",
            Id = Guid.Parse(DifferentAccountServiceCategoryId), AccountId = Constants.RandomAccountId,
            Colour = Colour.Blue
        });
    }

    [Test]
    public async Task UnauthenticatedUser_ShouldThrow_AuthenticationException()
    {
        RunAsAnonymousUser();

        var command = ValidCommand();

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task UnauthorizedUser_ShouldThrow_ForbiddenAccessException()
    {
        await RunAsDefaultUserAsync();

        var command = ValidCommand();

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task UserWithCreateRoleOnly_ShouldGetForbiddenAccess_OnUpdateTry()
    {
        await RunAsAdministratorAsync(typeof(Service), Service.CreateRole, Constants.CurrentUserAccountId);

        var serviceId = await AddAsync(TestService());
        var command = ValidCommand(serviceId.ToString());

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task UserWithUpdateRoleOnly_ShouldGetForbiddenAccess_OnCreateTry()
    {
        await RunAsAdministratorAsync(typeof(Service), Service.UpdateRole, Constants.CurrentUserAccountId);

        var command = ValidCommand();

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task Upsert_AnotherAccountService_ShouldThrow_NotFoundException()
    {
        await RunAsAdministratorAsync(typeof(Service), Service.UpdateRole, Constants.CurrentUserAccountId);

        var serviceId = await AddAsync(TestService(Constants.RandomAccountIdString));
        var command = ValidCommand(serviceId.ToString());

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();

        var service = await FindAsync<Service>(serviceId);
        Assert.AreEqual(serviceId, service.Id);
    }

    [Test]
    public async Task Update_CorrectCommand_UpdatesEntity()
    {
        await RunAsAdministratorAsync(typeof(Service), Service.UpdateRole, Constants.CurrentUserAccountId);

        var serviceId = await AddAsync(TestService());
        var command = ValidCommand(serviceId.ToString());

        var result = await FluentActions.Invoking(() => SendAsync(command)).Invoke();

        var entity = await FindAsync<Service>(serviceId);
        AssertResultEqualCommand(result, command);
        AssertEntityEqualCommand(entity, command);
    }

    [Test]
    public async Task Create_CorrectAccount_ServiceCategory_ShouldCreate()
    {
        await RunAsAdministratorAsync(typeof(Service), Service.CreateRole, Constants.CurrentUserAccountId);

        var command = ValidCommand();

        var result = await FluentActions.Invoking(() => SendAsync(command)).Invoke();

        var entity = await FindAsync<Service>(result.Id);
        AssertResultEqualCommand(result, command);
        AssertEntityEqualCommand(entity, command);
    }

    [Test]
    public async Task Update_DifferentAccount_ServiceCategory_ThrowsValidationError()
    {
        await RunAsAdministratorAsync(typeof(Service), Service.UpdateRole, Constants.CurrentUserAccountId);

        var serviceId = await AddAsync(TestService());
        var command = ValidCommand(serviceId.ToString(), DifferentAccountServiceCategoryId);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task Create_DifferentAccount_ServiceCategory_ThrowsValidationError()
    {
        await RunAsAdministratorAsync(typeof(Service), Service.CreateRole, Constants.CurrentUserAccountId);

        var command = ValidCommand(string.Empty, DifferentAccountServiceCategoryId);
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    private void AssertResultEqualCommand(ServiceAdminResourceModel result, UpsertServiceCommand command)
    {
        Assert.AreEqual(command.Name, result.Name);
        Assert.AreEqual(command.Description, result.Description);
        Assert.AreEqual(command.Price, result.Price);
        Assert.AreEqual(command.Duration, result.Duration);
        Assert.AreEqual(command.ServiceCategoryId, result.ServiceCategoryId);
    }

    private void AssertEntityEqualCommand(Service service, UpsertServiceCommand command)
    {
        Assert.AreEqual(command.Name, service.Name);
        Assert.AreEqual(command.Description, service.Description);
        Assert.AreEqual(command.Price, service.Price);
        Assert.AreEqual(command.Duration, service.Duration);
        Assert.AreEqual(command.ServiceCategoryId, service.ServiceCategoryId);
    }
}