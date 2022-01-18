using System;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Entities;

namespace Kalendario.Application.ResourceModels.Admin;

public class AppointmentAdminResourceModel : IMapFrom<Appointment>
{
    public Guid Id { get; set; }

    public Guid? CustomerId { get; set; }
    public CustomerAdminResourceModel Customer { get; set; }

    public Guid? EmployeeId { get; set; }
    public EmployeeAdminResourceModel Employee { get; set; }

    public Guid? ServiceId { get; set; }
    public ServiceAdminResourceModel Service { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public double Price { get; set; }

    public string InternalNotes { get; set; }
}