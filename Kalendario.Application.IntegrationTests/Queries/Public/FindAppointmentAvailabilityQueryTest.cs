using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Kalendario.Application.IntegrationTests.Common;
using Kalendario.Application.Queries.Public;
using NUnit.Framework;

namespace Kalendario.Application.IntegrationTests.Queries.Public;

using static Testing;

public class FindAppointmentAvailabilityQueryTest : TestBase
{
    private Guid _service1Id;
    private Guid _service2Id;
    private Guid _employee1Id;
    private Guid _employee2Id;
    private Guid _employee3Id;
    private Guid _customerId;


    public FindAppointmentAvailabilityQuery Query(DateOnly dateTime, Guid serviceId, Guid? employeeId) => new()
    {
        ServiceId = serviceId,
        Date = dateTime,
        EmployeeId = employeeId,
    };

    [SetUp]
    public new async Task TestSetUp()
    {
        await base.TestSetUp();
        var schedule = Entities.TestSchedule();
        schedule.Frames = schedule.Frames.Where(f => f.Offset != DayOfWeek.Monday).ToList();
        var scheduleId = await AddAsync(schedule);
        
        _service1Id = await AddAsync(Entities.TestService());
        _service2Id = await AddAsync(Entities.TestService());
        
        // service has 1 hour duration.
        _employee1Id = await AddAsync(Entities.TestEmployee(new[] {_service1Id}, scheduleId));
        _employee2Id = await AddAsync(Entities.TestEmployee(new[] {_service1Id}, scheduleId));
        _employee3Id = await AddAsync(Entities.TestEmployee(new[] {_service2Id}, scheduleId));

        _customerId = await AddAsync(Entities.TestCustomer());
    }

    [Test]
    public async Task RequestFor_DateWithNoFrames_ReturnsNoSlots()
    {
        RunAsAnonymousUser();
        var query = Query(DateTimeHelpers.DNextDayOfWeek(DayOfWeek.Monday), _service1Id, _employee1Id);
        var result = await FluentActions.Invoking(() => SendAsync(query)).Invoke();
        Assert.AreEqual(0, result.Slots.Count);
    }
    
    [Test]
    public async Task RequestFor_DateWithNoAppointments_ReturnsSlotsForFrames()
    {
        RunAsAnonymousUser();
        var query = Query(DateTimeHelpers.DNextDayOfWeek(DayOfWeek.Sunday), _service1Id, _employee1Id);
        var result = await FluentActions.Invoking(() => SendAsync(query)).Invoke();
        Assert.AreEqual(8, result.Slots.Count);
    }
    
    [Test]
    public async Task RequestFor_DateWithAppointmentAtStart_ReturnsSlotsForFrames()
    {
        RunAsAnonymousUser();
        var dateToBook = DateTimeHelpers.DNextDayOfWeek(DayOfWeek.Sunday);
        var appointment = Entities.TestAppointment(_service1Id, _employee1Id, _customerId);
        appointment.Start = dateToBook.ToDateTime(TimeOnly.Parse("09:00")).ToUniversalTime();
        appointment.End = appointment.Start.AddHours(1);
        var appointmentId = await AddAsync(appointment);

        var query = Query(dateToBook, _service1Id, _employee1Id);
        var result = await FluentActions.Invoking(() => SendAsync(query)).Invoke();
        
        Assert.AreEqual(7, result.Slots.Count);
    }
    
    [Test]
    public async Task RequestFor_DateWithAppointmentAtMiddle_ReturnsSlotsForFrames()
    {
        RunAsAnonymousUser();
        var dateToBook = DateTimeHelpers.DNextDayOfWeek(DayOfWeek.Sunday);
        var appointment = Entities.TestAppointment(_service1Id, _employee1Id, _customerId);
        appointment.Start = dateToBook.ToDateTime(TimeOnly.Parse("09:30")).ToUniversalTime();
        appointment.End = appointment.Start.AddHours(1);
        var appointmentId = await AddAsync(appointment);

        var query = Query(dateToBook, _service1Id, _employee1Id);
        var result = await FluentActions.Invoking(() => SendAsync(query)).Invoke();
        
        Assert.AreEqual(6, result.Slots.Count);
    }
}