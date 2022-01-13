using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Castle.Core.Internal;
using FluentAssertions;
using Kalendario.Application.Commands.Admin;
using Kalendario.Application.Common.Exceptions;
using Kalendario.Application.IntegrationTests.Common;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Core.Entities;
using NUnit.Framework;

namespace Kalendario.Application.IntegrationTests.Commands;

using static Testing;

public class UpsertAppointmentCommandTests : TestBase
{
    private Guid CurrentAccountServiceId;
    private Guid CurrentAccountEmployeeId;
    private Guid CurrentAccountCustomerId;

    private Guid RandomAccountServiceId;
    private Guid RandomAccountEmployeeId;
    private Guid RandomAccountCustomerId;

    private UpsertAppointmentCommand Command(Guid serviceId, Guid employeeId, Guid customerId, Guid? id = null) => new()
    {
        Id = id,
        Start = new DateTime(2022, 01, 12, 10, 0, 0),
        End = new DateTime(2022, 01, 12, 11, 0, 0),
        ServiceId = serviceId,
        CustomerId = customerId,
        EmployeeId = employeeId,
        InternalNotes = "new internal notes."
    };

    private UpsertAppointmentCommand ValidCommand(Guid? id = null) => Command(CurrentAccountServiceId,
        CurrentAccountEmployeeId, CurrentAccountCustomerId, id);

    private Appointment CurrentAccountAppointment => Entities.TestAppointment(CurrentAccountServiceId,
        CurrentAccountEmployeeId, CurrentAccountCustomerId);

    private Appointment RandomAccountAppointment => Entities.TestAppointment(RandomAccountServiceId,
        RandomAccountEmployeeId, RandomAccountCustomerId, Constants.RandomAccountIdString);

    private static async Task<Appointment?> FindAppointmentById(Guid appointmentId)
    {
        return await FirstOrDefaultAsync(appointmentId, new List<Expression<Func<Appointment, object>>>()
        {
            appointment => appointment.Customer,
            appointment => appointment.Employee,
            appointment => appointment.Service,
        });
    }

    [SetUp]
    public new async Task TestSetUp()
    {
        await base.TestSetUp();

        var scheduleId = await AddAsync(Entities.TestSchedule());
        
        CurrentAccountCustomerId = await AddAsync(Entities.TestCustomer());
        CurrentAccountServiceId = await AddAsync(Entities.TestService());
        CurrentAccountEmployeeId =
            await AddAsync(Entities.TestEmployee(new List<Guid>() {CurrentAccountServiceId}, scheduleId));

        RandomAccountCustomerId = await AddAsync(Entities.TestCustomer(Constants.RandomAccountIdString));
        RandomAccountEmployeeId =
            await AddAsync(Entities.TestEmployee(new List<Guid>(), null, Constants.RandomAccountIdString));
        RandomAccountServiceId = await AddAsync(Entities.TestService(null, Constants.RandomAccountIdString));
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
        await RunAsAdministratorAsync(typeof(Appointment), Appointment.CreateRole, Constants.CurrentUserAccountId);

        var entityid = await AddAsync(CurrentAccountAppointment);
        var command = ValidCommand(entityid);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task UserWithUpdateRoleOnly_ShouldGetForbiddenAccess_OnCreateTry()
    {
        await RunAsAdministratorAsync(typeof(Appointment), Appointment.UpdateRole, Constants.CurrentUserAccountId);

        var command = ValidCommand();

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task Upsert_AnotherAccountAppointment_ShouldThrow_NotFoundException()
    {
        await RunAsAdministratorAsync(typeof(Appointment), Appointment.UpdateRole, Constants.CurrentUserAccountId);

        var appointmentId = await AddAsync(RandomAccountAppointment);
        var command = ValidCommand(appointmentId);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();

        var appointment = await FindAsync<Appointment>(appointmentId);
        Assert.AreEqual(appointmentId, appointment.Id);
    }

    [Test]
    public async Task Update_CorrectCommand_UpdatesEntity()
    {
        await RunAsAdministratorAsync(typeof(Appointment), Appointment.UpdateRole, Constants.CurrentUserAccountId);

        var appointmentId = await AddAsync(CurrentAccountAppointment);
        var command = ValidCommand(appointmentId);

        var result = await FluentActions.Invoking(() => SendAsync(command)).Invoke();

        var entity = await FindAppointmentById(appointmentId);
        AssertResultEqualCommand(result, command);
        AssertEntityEqualCommand(entity, command);
    }

    [Test]
    public async Task Create_CorrectCommand_CreatesEntity()
    {
        await RunAsAdministratorAsync(typeof(Appointment), Appointment.CreateRole, Constants.CurrentUserAccountId);

        var command = ValidCommand();

        var result = await FluentActions.Invoking(() => SendAsync(command)).Invoke();

        var entity = await FindAppointmentById(result.Id);
        AssertResultEqualCommand(result, command);
        AssertEntityEqualCommand(entity, command);
    }

    [Test]
    public async Task Update_RandomAccount_Service_ThrowsValidationError()
    {
        await RunAsAdministratorAsync(typeof(Appointment), Appointment.UpdateRole, Constants.CurrentUserAccountId);

        var entityId = await AddAsync(CurrentAccountAppointment);
        var command = Command(RandomAccountServiceId, CurrentAccountEmployeeId, CurrentAccountCustomerId, entityId);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task Update_RandomAccount_Employee_ThrowsValidationError()
    {
        await RunAsAdministratorAsync(typeof(Appointment), Appointment.UpdateRole, Constants.CurrentUserAccountId);

        var entityId = await AddAsync(CurrentAccountAppointment);
        var command = Command(CurrentAccountServiceId, RandomAccountEmployeeId, CurrentAccountCustomerId, entityId);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task Update_RandomAccount_Customer_ThrowsValidationError()
    {
        await RunAsAdministratorAsync(typeof(Appointment), Appointment.UpdateRole, Constants.CurrentUserAccountId);

        var entityId = await AddAsync(CurrentAccountAppointment);
        var command = Command(CurrentAccountServiceId, CurrentAccountEmployeeId, RandomAccountCustomerId, entityId);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task Create_RandomAccount_Service_ThrowsValidationError()
    {
        await RunAsAdministratorAsync(typeof(Appointment), Appointment.CreateRole, Constants.CurrentUserAccountId);

        var command = Command(RandomAccountServiceId, CurrentAccountEmployeeId, CurrentAccountCustomerId);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task Create_RandomAccount_Employee_ThrowsValidationError()
    {
        await RunAsAdministratorAsync(typeof(Appointment), Appointment.CreateRole, Constants.CurrentUserAccountId);

        var command = Command(CurrentAccountServiceId, RandomAccountEmployeeId, CurrentAccountCustomerId);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task Create_RandomAccount_Customer_ThrowsValidationError()
    {
        await RunAsAdministratorAsync(typeof(Appointment), Appointment.CreateRole, Constants.CurrentUserAccountId);

        var command = Command(CurrentAccountServiceId, CurrentAccountEmployeeId, RandomAccountCustomerId);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }
    
    [Test]
    public async Task Create_NoSchedule_ThrowsValidationError()
    {
        await RunAsAdministratorAsync(typeof(Appointment), Appointment.CreateRole, Constants.CurrentUserAccountId);

        // Updates the employee to someone with no schedule.
        CurrentAccountEmployeeId =
            await AddAsync(Entities.TestEmployee(new List<Guid>() {CurrentAccountServiceId}));
        var command = ValidCommand();

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }
        
    [Test]
    public async Task Create_OutsideScheduleTimes_ThrowsValidationError()
    {
        await RunAsAdministratorAsync(typeof(Appointment), Appointment.CreateRole, Constants.CurrentUserAccountId);

        var command = ValidCommand();
        command.Start = command.Start.AddHours(-2);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }
    
    [Test]
    public async Task Create_EndTimeBeforeStartTime_ThrowsValidationError()
    {
        await RunAsAdministratorAsync(typeof(Appointment), Appointment.CreateRole, Constants.CurrentUserAccountId);

        var command = ValidCommand();
        command.End = command.Start.AddHours(-1);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }
    
    [Test]
    public async Task Create_EndTimeSameStartTime_ThrowsValidationError()
    {
        await RunAsAdministratorAsync(typeof(Appointment), Appointment.CreateRole, Constants.CurrentUserAccountId);

        var command = ValidCommand();
        command.End = command.Start;

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }
    
    [Test]
    public async Task Create_NoSchedule_WithIgnoreTimeClashes_True_Creates()
    {
        await RunAsAdministratorAsync(typeof(Appointment), Appointment.CreateRole, Constants.CurrentUserAccountId);

        // Updates the employee to someone with no schedule.
        CurrentAccountEmployeeId =
            await AddAsync(Entities.TestEmployee(new List<Guid>() {CurrentAccountServiceId}));
        var command = ValidCommand();
        command.IgnoreTimeClashes = true;

        var result = await FluentActions.Invoking(() => SendAsync(command)).Invoke();

        var entity = await FindAppointmentById(result.Id);
        AssertResultEqualCommand(result, command);
        AssertEntityEqualCommand(entity, command);
    }

    [Test]
    public async Task Create_OutsideScheduleTimes_WithIgnoreTimeClashes_True_Creates()
    {
        await RunAsAdministratorAsync(typeof(Appointment), Appointment.CreateRole, Constants.CurrentUserAccountId);

        var command = ValidCommand();
        command.IgnoreTimeClashes = true;
        command.Start = command.Start.AddHours(-1);

        var result = await FluentActions.Invoking(() => SendAsync(command)).Invoke();

        var entity = await FindAppointmentById(result.Id);
        AssertResultEqualCommand(result, command);
        AssertEntityEqualCommand(entity, command);
    }

    [Test]
    public async Task Create_EndTimeBeforeStartTime_WithIgnoreTimeClashes_True_ThrowsError()
    {
        await RunAsAdministratorAsync(typeof(Appointment), Appointment.CreateRole, Constants.CurrentUserAccountId);

        var command = ValidCommand();
        command.IgnoreTimeClashes = true;
        command.End = command.Start.AddHours(-1);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task Create_OverlappingAppointment_ThrowsError()
    {
        await RunAsAdministratorAsync(typeof(Appointment), Appointment.CreateRole, Constants.CurrentUserAccountId);

        var command = ValidCommand();
        var appointment = CurrentAccountAppointment;
        appointment.Start = command.Start;
        appointment.End = command.Start.AddMinutes(30);
        await AddAsync(appointment);
        
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task Create_OverlappingAppointment_WithIgnoreTimeClashes_True_Creates()
    {
        await RunAsAdministratorAsync(typeof(Appointment), Appointment.CreateRole, Constants.CurrentUserAccountId);

        var command = ValidCommand();
        command.IgnoreTimeClashes = true;
        var appointment = CurrentAccountAppointment;
        appointment.Start = command.Start;
        appointment.End = command.Start.AddMinutes(30);
        await AddAsync(appointment);
        
        var result = await FluentActions.Invoking(() => SendAsync(command)).Invoke();
        
        var entity = await FindAppointmentById(result.Id);
        AssertResultEqualCommand(result, command);
        AssertEntityEqualCommand(entity, command);
    }
    
    private void AssertResultEqualCommand(AppointmentAdminResourceModel result, UpsertAppointmentCommand command)
    {
        Assert.AreEqual(command.Start, result.Start);
        Assert.AreEqual(command.End, result.End);
        Assert.AreEqual(command.InternalNotes, result.InternalNotes);
        Assert.AreEqual(command.CustomerId, result.Customer.Id);
        Assert.AreEqual(command.EmployeeId, result.Employee.Id);
        Assert.AreEqual(command.ServiceId, result.Service.Id);
        Assert.AreEqual(result.Service.Price, result.Price);
    }

    private void AssertEntityEqualCommand(Appointment entity, UpsertAppointmentCommand command)
    {
        Assert.AreEqual(command.Start, entity.Start);
        Assert.AreEqual(command.End, entity.End);
        Assert.AreEqual(command.InternalNotes, entity.InternalNotes);
        Assert.AreEqual(command.CustomerId, entity.Customer.Id);
        Assert.AreEqual(command.EmployeeId, entity.Employee.Id);
        Assert.AreEqual(command.ServiceId, entity.Service.Id);
        Assert.AreEqual(entity.Service.Price, entity.Price);
    }
}