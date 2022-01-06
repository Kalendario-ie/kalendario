using System;
using System.Threading.Tasks;
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
    private Guid _currentUserAccountServiceCategoryId = Guid.NewGuid();
    private Guid _differentAccountServiceCategoryId = Guid.NewGuid();

    private Service TestService => new()
    {
        Name = "Example",
        AccountId = Constants.CurrentUserAccountId,
        Description = "Description Example",
        Price = 20.1,
        Duration = TimeSpan.FromHours(1),
        ServiceCategoryId = _currentUserAccountServiceCategoryId
    };
    
    private UpsertServiceCommand ValidCommand => new()
    {
        Name = "new service name",
        Description = "new service description",
        Price = 100.12,
        Duration = TimeSpan.FromHours(4),
        ServiceCategoryId = _currentUserAccountServiceCategoryId
    };

    [SetUp]
    public new async Task TestSetUp()
    {
        await base.TestSetUp();
        await AddAsync(new ServiceCategory
        {
            Name = "category 1",
            Id = _currentUserAccountServiceCategoryId, AccountId = Constants.CurrentUserAccountId, Colour = Colour.Blue
        });
        await AddAsync(new ServiceCategory
        {
            Name = "category 1",
            Id = _differentAccountServiceCategoryId, AccountId = Constants.RandomAccountId, Colour = Colour.Blue
        });
    }

    [Test]
    public async Task UnauthenticatedUser_ShouldThrow_AuthenticationException()
    {
        RunAsAnonymousUser();
        var command = ValidCommand;
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task UnauthorizedUser_ShouldThrow_ForbiddenAccessException()
    {
        await RunAsDefaultUserAsync();
        var command = ValidCommand;
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task UserWithCreateRoleOnly_ShouldGetForbiddenAccess_OnUpdateTry()
    {
        await RunAsAdministratorAsync(typeof(Service), Service.CreateRole);
        var serviceId = await AddAsync(new Service {AccountId = Constants.RandomAccountId, Name = "Test"});
        var command = ValidCommand;
        command.Id = serviceId;
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task UserWithUpdateRoleOnly_ShouldGetForbiddenAccess_OnCreateTry()
    {
        await RunAsAdministratorAsync(typeof(Service), Service.UpdateRole);
        var command = ValidCommand;
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task Upsert_AnotherAccountService_ShouldThrow_NotFoundException()
    {
        await RunAsAdministratorAsync(typeof(Service), Service.UpdateRole, Constants.CurrentUserAccountId);

        var serviceId = await AddAsync(new Service {AccountId = Constants.RandomAccountId, Name = "Test"});
        var command = ValidCommand;
        command.Id = serviceId;

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();

        var service = await FindAsync<Service>(serviceId);
        Assert.AreEqual(serviceId, service.Id);
    }

    [Test]
    public async Task Update_CorrectCommand_UpdatesEntity()
    {
        await RunAsAdministratorAsync(typeof(Service), Service.UpdateRole, Constants.CurrentUserAccountId);

        var serviceId = await AddAsync(TestService);
        var command = ValidCommand;
        command.Id = serviceId;

        var result = await FluentActions.Invoking(() => SendAsync(command)).Invoke();

        var entity = await FindAsync<Service>(serviceId);
        AssertResultEqualCommand(result, command);
        AssertEntityEqualCommand(entity, command);
    }

    [Test]
    public async Task Create_CorrectAccount_ServiceCategory_ShouldCreate()
    {
        await RunAsAdministratorAsync(typeof(Service), Service.CreateRole, Constants.CurrentUserAccountId);

        var command = ValidCommand;
        
        var result = await FluentActions.Invoking(() => SendAsync(command)).Invoke();

        var entity = await FindAsync<Service>(result.Id);
        AssertResultEqualCommand(result, command);
        AssertEntityEqualCommand(entity, command);
    }

    [Test]
    public async Task Update_DifferentAccount_ServiceCategory_ThrowsValidationError()
    {
        await RunAsAdministratorAsync(typeof(Service), Service.UpdateRole, Constants.CurrentUserAccountId);

        var serviceId = await AddAsync(TestService);
        var command = ValidCommand;
        command.Id = serviceId;
        command.ServiceCategoryId = _differentAccountServiceCategoryId;
        
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();

    }

    [Test]
    public async Task Create_DifferentAccount_ServiceCategory_ThrowsValidationError()
    {
        await RunAsAdministratorAsync(typeof(Service), Service.CreateRole, Constants.CurrentUserAccountId);

        var command = ValidCommand;
        command.ServiceCategoryId = _differentAccountServiceCategoryId;
        
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