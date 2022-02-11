using System;
using System.ComponentModel.DataAnnotations;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Entities;

namespace Kalendario.Application.ResourceModels.User;

public class AppointmentUserResourceModel : IMapFrom<Appointment>
{
    public Guid Id { get; set; }

    [Required] public string Name => $"{Start}_{End}";

    public Guid? CustomerId { get; set; }
    public CustomerUserResourceModel Customer { get; set; }

    public Guid? ServiceId { get; set; }
    public ServiceUserResourceModel Service { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public string InternalNotes { get; set; }
}