using System;
using System.Threading.Tasks;
using FluentAssertions;
using Kalendario.Application.Common.Exceptions;
using Kalendario.Application.IntegrationTests.Common;
using Kalendario.Application.Queries.Admin;
using Kalendario.Core.Entities;
using NUnit.Framework;

namespace Kalendario.Application.IntegrationTests.Queries.Admin;

using static Testing;

public class GetAppointmentHistoryQueryTests : TestBase
{
    [Test]
    public async Task UnauthenticatedUsers_ShouldThrow_AuthenticationException()
    {
        RunAsAnonymousUser();
        var query = new GetAppointmentHistoryQuery();
        await FluentActions.Invoking(() => SendAsync(query)).Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task UnauthorizedUser_ShouldThrow_ForbiddenAccessException()
    {
        await RunAsDefaultUserAsync();
        var query = new GetAppointmentHistoryQuery();
        await FluentActions.Invoking(() => SendAsync(query)).Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task DefaultQuery_ShouldReturn_CurrentUserAccountCustomers()
    {
        await RunAsAdministratorAsync(typeof(Appointment), Appointment.ViewRole, Constants.CurrentUserAccountId);
        var serviceId = await AddAsync(Entities.TestService());
        var customerId = await AddAsync(Entities.TestCustomer());
        var employeeId = await AddAsync(Entities.TestEmployee(new[] {serviceId}));
        var appointmentId = await AddAsync(Entities.TestAppointment(serviceId, employeeId, customerId));
        var query = new GetAppointmentHistoryQuery {Id = appointmentId};
        var result = await FluentActions.Invoking(() => SendAsync(query)).Invoke();

        Assert.AreEqual(1, result.Entities.Count);
        foreach (var entity in result.Entities)
        {
            Assert.IsNotNull(entity.Customer);
            Assert.IsNotNull(entity.Employee);
            Assert.IsNotNull(entity.Service);
        }
    }
}