using System;
using System.Collections.Generic;
using System.Linq;
using Kalendario.Core.Entities;
using Kalendario.Core.ValueObject;

namespace Kalendario.Application.IntegrationTests.Common;

public class Entities
{
    public static Schedule TestSchedule(string accountId = Constants.CurrentUserAccountIdString) => new()
    {
        Name = "Example",
        AccountId = Guid.Parse(accountId),
        Frames = new List<ScheduleFrame>
        {
            CreateFrame(DayOfWeek.Sunday, 0, "09:00", "17:00"),
            CreateFrame(DayOfWeek.Monday, 0, "09:00", "13:00"),
            CreateFrame(DayOfWeek.Monday, 1, "14:00", "15:00"),
            CreateFrame(DayOfWeek.Monday, 2, "16:00", "17:00"),
            CreateFrame(DayOfWeek.Tuesday, 0, "09:00", "14:00"),
            CreateFrame(DayOfWeek.Tuesday, 1, "14:00", "17:00"),
            CreateFrame(DayOfWeek.Wednesday, 0, "9:00", "11:00"),
            CreateFrame(DayOfWeek.Thursday, 0, "11:00", "12:00"),
            CreateFrame(DayOfWeek.Friday, 0, "12:00", "13:00"),
            CreateFrame(DayOfWeek.Saturday, 0, "13:00", "14:00"),
        }
    };

    public static Customer TestCustomer(string accountId = Constants.CurrentUserAccountIdString) => new()
    {
        Name = "Example",
        AccountId = Guid.Parse(accountId),
        PhoneNumber = "9834012984",
        Email = "teste@test.com",
        Warning = "this is a warning"
    };

    public static Service TestService(Guid? serviceCategoryId = null,
        string accountId = Constants.CurrentUserAccountIdString) => new()
    {
        Name = "Example",
        AccountId = Guid.Parse(accountId),
        Description = "Description Example",
        Price = 20.1,
        Duration = TimeSpan.FromHours(1),
        ServiceCategoryId = serviceCategoryId
    };

    public static ServiceCategory TestServiceCategory(string accountId = Constants.CurrentUserAccountIdString) => new()
    {
        Name = "category 1",
        AccountId = Guid.Parse(accountId),
        Colour = Colour.Blue
    };

    private static ScheduleFrame CreateFrame(DayOfWeek offset, int order, string start, string end)
    {
        return new ScheduleFrame
        {
            Offset = offset, Order = order, AccountId = Constants.CurrentUserAccountId,
            Start = TimeOnly.Parse(start), End = TimeOnly.Parse(end)
        };
    }

    public static Employee TestEmployee(IEnumerable<Guid> services, Guid? scheduleId = null,
        string accountId = Constants.CurrentUserAccountIdString) => new()
    {
        AccountId = Guid.Parse(accountId),
        Name = "Employee Name",
        Email = "employee@email.com",
        PhoneNumber = "088 877 0000",
        ScheduleId = scheduleId,
        EmployeeServices = services.Select(s => new EmployeeService {ServiceId = s, AccountId = Guid.Parse(accountId)})
            .ToList()
    };

    public static Appointment TestAppointment(Guid serviceId, Guid employeeId, Guid customerId,
        string accountId = Constants.CurrentUserAccountIdString) => new()
    {
        AccountId = Guid.Parse(accountId),
        Start = DateTime.UtcNow,
        End = DateTime.UtcNow.AddHours(1),
        ServiceId = serviceId,
        CustomerId = customerId,
        EmployeeId = employeeId,
    };
}