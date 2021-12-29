using System;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Domain;

namespace Kalendario.Application.ResourceModels.Admin;

public class AppointmentAdminResourceModel : IMapFrom<Appointment>
{
    public CustomerAdminResourceModel CustomerAdmin { get; set; }

    public EmployeeAdminResourceModel EmployeeAdmin { get; set; }

    public ServiceAdminResourceModel ServiceAdmin { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public double Price { get; set; }

    public string InternalNotes { get; set; }
}