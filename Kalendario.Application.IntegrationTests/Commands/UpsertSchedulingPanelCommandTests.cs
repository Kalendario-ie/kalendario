using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

public class UpsertSchedulingPanelCommandTests : TestBase
{
    private List<Guid> _currentAccountEmployeeIds = new();
    private List<Guid> _randomAccountEmployeeIds = new();

    private UpsertSchedulingPanelCommand ValidCommand(List<Guid> employees, Guid? id = null) => new()
    {
        Id = id,
        Name = "New Scheduling Panel Name",
        EmployeeIds = employees
    };
    
    private static async Task<SchedulingPanel?> FindSchedulingPanelById(Guid id)
    {
        var entity = await FirstOrDefaultAsync(id,
            new List<Expression<Func<SchedulingPanel, object>>> {schedulingPanel => schedulingPanel.Employees});
        return entity;
    }


    [SetUp]
    public new async Task TestSetUp()
    {
        await base.TestSetUp();
        _currentAccountEmployeeIds = new List<Guid>();
        _randomAccountEmployeeIds = new List<Guid>();

        _currentAccountEmployeeIds.Add(await AddAsync(Entities.TestEmployee(new List<Guid>())));
        _currentAccountEmployeeIds.Add(await AddAsync(Entities.TestEmployee(new List<Guid>())));

        _randomAccountEmployeeIds.Add(await AddAsync(Entities.TestEmployee(new List<Guid>(), null, Constants.RandomAccountIdString)));
    }

    [Test]
    public async Task UnauthenticatedUser_ShouldThrow_AuthenticationException()
    {
        RunAsAnonymousUser();

        var command = ValidCommand(_currentAccountEmployeeIds);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task UnauthorizedUser_ShouldThrow_ForbiddenAccessException()
    {
        await RunAsDefaultUserAsync();

        var command = ValidCommand(_currentAccountEmployeeIds);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task UserWithCreateRoleOnly_ShouldGetForbiddenAccess_OnUpdateTry()
    {
        await RunAsAdministratorAsync(typeof(SchedulingPanel), SchedulingPanel.CreateRole, Constants.CurrentUserAccountId);

        var entityId = await AddAsync(Entities.TestSchedulingPanel());
        var command = ValidCommand(_currentAccountEmployeeIds, entityId);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task UserWithUpdateRoleOnly_ShouldGetForbiddenAccess_OnCreateTry()
    {
        await RunAsAdministratorAsync(typeof(SchedulingPanel), SchedulingPanel.UpdateRole, Constants.CurrentUserAccountId);

        var command = ValidCommand(_currentAccountEmployeeIds);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task Update_AnotherAccountSchedulingPanel_ShouldThrow_NotFoundException()
    {
        await RunAsAdministratorAsync(typeof(SchedulingPanel), SchedulingPanel.UpdateRole, Constants.CurrentUserAccountId);

        var entityId =
            await AddAsync(Entities.TestSchedulingPanel(Constants.RandomAccountIdString));
        var command = ValidCommand(_currentAccountEmployeeIds, entityId);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();

        var employee = await FindAsync<SchedulingPanel>(entityId);
        Assert.AreEqual(entityId, employee.Id);
    }

    [Test]
    public async Task Update_CorrectCommand_UpdatesEntity()
    {
        await RunAsAdministratorAsync(typeof(SchedulingPanel), SchedulingPanel.UpdateRole, Constants.CurrentUserAccountId);

        var entityId = await AddAsync(Entities.TestSchedulingPanel());
        var command = ValidCommand(_currentAccountEmployeeIds, entityId);

        var result = await FluentActions.Invoking(() => SendAsync(command)).Invoke();

        var entity = await FindSchedulingPanelById(result.Id);
        AssertResultEqualCommand(result, command);
        AssertEntityEqualCommand(entity, command);
    }
    
    [Test]
    public async Task Create_CorrectAccount_Schedule_ShouldCreate()
    {
        await RunAsAdministratorAsync(typeof(SchedulingPanel), SchedulingPanel.CreateRole, Constants.CurrentUserAccountId);

        var command = ValidCommand(_currentAccountEmployeeIds);

        var result = await FluentActions.Invoking(() => SendAsync(command)).Invoke();

        var entity = await FindSchedulingPanelById(result.Id);
        AssertResultEqualCommand(result, command);
        AssertEntityEqualCommand(entity, command);
    }

    [Test]
    public async Task Create_InvalidEmployeeId_ShouldThrowValidationError()
    {
        await RunAsAdministratorAsync(typeof(SchedulingPanel), SchedulingPanel.CreateRole, Constants.CurrentUserAccountId);

        var command = ValidCommand(_currentAccountEmployeeIds.Append(Guid.NewGuid()).ToList());

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task Create_DifferentAccountEmployeePanel_ShouldThrowValidationError()
    {
        await RunAsAdministratorAsync(typeof(SchedulingPanel), SchedulingPanel.CreateRole, Constants.CurrentUserAccountId);

        var command = ValidCommand(_randomAccountEmployeeIds);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    private void AssertResultEqualCommand(SchedulingPanelAdminResourceModel result, UpsertSchedulingPanelCommand command)
    {
        Assert.AreEqual(command.Name, result.Name);
        Assert.AreEqual(command.EmployeeIds, result.EmployeeIds);
    }

    private void AssertEntityEqualCommand(SchedulingPanel entity, UpsertSchedulingPanelCommand command)
    {
        Assert.AreEqual(command.Name, entity.Name);
        Assert.AreEqual(command.EmployeeIds.OrderBy(s => s),
            entity.Employees.Select(s => s.Id).OrderBy(s => s).ToList());
    }
}