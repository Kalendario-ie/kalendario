using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Kalendario.Application.Commands.Admin;
using Kalendario.Application.Common.Exceptions;
using Kalendario.Application.IntegrationTests.Common;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Core.Entities;
using NUnit.Framework;

namespace Kalendario.Application.IntegrationTests.Commands;

using static Testing;

public class UpsertEmployeeCommandTests : TestBase
{
    private Guid _currentAccountScheduleId;
    private Guid _randomAccountScheduleId;

    private List<Guid> _currentAccountServiceIds = new();
    private List<Guid> _randomAccountServiceIds = new();

    private UpsertEmployeeCommand ValidCommand(Guid? scheduleId, List<Guid> services, Guid? id = null) => new()
    {
        Id = id,
        Name = "new employee name",
        Email = "command@email.com",
        PhoneNumber = "new number",
        ScheduleId = scheduleId,
        Services = services
    };

    [SetUp]
    public new async Task TestSetUp()
    {
        await base.TestSetUp();
        _currentAccountServiceIds = new List<Guid>();
        _randomAccountServiceIds = new List<Guid>();

        _currentAccountScheduleId = await AddAsync(Entities.TestSchedule());
        _currentAccountServiceIds.Add(await AddAsync(Entities.TestService()));
        _currentAccountServiceIds.Add(await AddAsync(Entities.TestService()));

        _randomAccountScheduleId = await AddAsync(Entities.TestSchedule(Constants.RandomAccountIdString));
        _randomAccountServiceIds.Add(await AddAsync(Entities.TestService(null, Constants.RandomAccountIdString)));
    }

    [Test]
    public async Task UnauthenticatedUser_ShouldThrow_AuthenticationException()
    {
        RunAsAnonymousUser();

        var command = ValidCommand(_currentAccountScheduleId, _currentAccountServiceIds);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task UnauthorizedUser_ShouldThrow_ForbiddenAccessException()
    {
        await RunAsDefaultUserAsync();

        var command = ValidCommand(_currentAccountScheduleId, _currentAccountServiceIds);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task UserWithCreateRoleOnly_ShouldGetForbiddenAccess_OnUpdateTry()
    {
        await RunAsAdministratorAsync(typeof(Employee), Employee.CreateRole, Constants.CurrentUserAccountId);

        var employeeId = await AddAsync(Entities.TestEmployee(Array.Empty<Guid>()));
        var command = ValidCommand(_currentAccountScheduleId, _currentAccountServiceIds, employeeId);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task UserWithUpdateRoleOnly_ShouldGetForbiddenAccess_OnCreateTry()
    {
        await RunAsAdministratorAsync(typeof(Employee), Employee.UpdateRole, Constants.CurrentUserAccountId);

        var command = ValidCommand(_currentAccountScheduleId, _currentAccountServiceIds);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task Upsert_AnotherAccountEmployee_ShouldThrow_NotFoundException()
    {
        await RunAsAdministratorAsync(typeof(Employee), Employee.UpdateRole, Constants.CurrentUserAccountId);

        var employeeId =
            await AddAsync(Entities.TestEmployee(Array.Empty<Guid>(), null, Constants.RandomAccountIdString));
        var command = ValidCommand(_currentAccountScheduleId, _currentAccountServiceIds, employeeId);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();

        var employee = await FindAsync<Employee>(employeeId);
        Assert.AreEqual(employeeId, employee.Id);
    }

    [Test]
    public async Task Update_CorrectCommand_UpdatesEntity()
    {
        await RunAsAdministratorAsync(typeof(Employee), Employee.UpdateRole, Constants.CurrentUserAccountId);

        var employeeId = await AddAsync(Entities.TestEmployee(Array.Empty<Guid>()));
        var command = ValidCommand(_currentAccountScheduleId, _currentAccountServiceIds, employeeId);

        var result = await FluentActions.Invoking(() => SendAsync(command)).Invoke();

        var entity = await FirstOrDefaultAsync<Employee, ICollection<Service>>(result.Id,
            employee => employee.Services);
        AssertResultEqualCommand(result, command);
        AssertEntityEqualCommand(entity, command);
    }

    [Test]
    public async Task Create_CorrectAccount_Schedule_ShouldCreate()
    {
        await RunAsAdministratorAsync(typeof(Employee), Employee.CreateRole, Constants.CurrentUserAccountId);

        var command = ValidCommand(_currentAccountScheduleId, _currentAccountServiceIds);

        var result = await FluentActions.Invoking(() => SendAsync(command)).Invoke();

        var entity = await FirstOrDefaultAsync<Employee, ICollection<Service>>(result.Id,
            employee => employee.Services);
        AssertResultEqualCommand(result, command);
        AssertEntityEqualCommand(entity, command);
    }

    [Test]
    public async Task Create_NoSchedule_ShouldCreate()
    {
        await RunAsAdministratorAsync(typeof(Employee), Employee.CreateRole, Constants.CurrentUserAccountId);

        var command = ValidCommand(null, _currentAccountServiceIds);

        var result = await FluentActions.Invoking(() => SendAsync(command)).Invoke();

        var entity = await FirstOrDefaultAsync<Employee, ICollection<Service>>(result.Id,
            employee => employee.Services);
        AssertResultEqualCommand(result, command);
        AssertEntityEqualCommand(entity, command);
    }
    
    [Test]
    public async Task Create_InvalidServiceId_ShouldThrowValidationError()
    {
        await RunAsAdministratorAsync(typeof(Employee), Employee.CreateRole, Constants.CurrentUserAccountId);

        var command = ValidCommand(null, _currentAccountServiceIds.Append(Guid.NewGuid()).ToList());

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }
    
    [Test]
    public async Task Create_DifferentAccountService_ShouldThrowValidationError()
    {
        await RunAsAdministratorAsync(typeof(Employee), Employee.CreateRole, Constants.CurrentUserAccountId);

        var command = ValidCommand(null, _randomAccountServiceIds);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    
    [Test]
    public async Task Update_DifferentAccount_Schedule_ThrowsValidationError()
    {
        await RunAsAdministratorAsync(typeof(Employee), Employee.UpdateRole, Constants.CurrentUserAccountId);

        var employeeId = await AddAsync(Entities.TestEmployee(Array.Empty<Guid>()));
        var command = ValidCommand(_randomAccountScheduleId, _currentAccountServiceIds, employeeId);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task Create_DifferentAccount_Schedule_ThrowsValidationError()
    {
        await RunAsAdministratorAsync(typeof(Employee), Employee.CreateRole, Constants.CurrentUserAccountId);

        var command = ValidCommand(_randomAccountScheduleId, _currentAccountServiceIds);
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    private void AssertResultEqualCommand(EmployeeAdminResourceModel result, UpsertEmployeeCommand command)
    {
        Assert.AreEqual(command.Name, result.Name);
        Assert.AreEqual(command.Email, result.Email);
        Assert.AreEqual(command.PhoneNumber, result.PhoneNumber);
        Assert.AreEqual(command.ScheduleId, result.ScheduleId);
        Assert.AreEqual(command.Services, result.Services);
    }

    private void AssertEntityEqualCommand(Employee entity, UpsertEmployeeCommand command)
    {
        Assert.AreEqual(command.Name, entity.Name);
        Assert.AreEqual(command.Email, entity.Email);
        Assert.AreEqual(command.PhoneNumber, entity.PhoneNumber);
        Assert.AreEqual(command.ScheduleId, entity.ScheduleId);
        Assert.AreEqual(command.Services.OrderBy(s => s),
            entity.Services.Select(s => s.Id).OrderBy(s => s).ToList());
    }
}