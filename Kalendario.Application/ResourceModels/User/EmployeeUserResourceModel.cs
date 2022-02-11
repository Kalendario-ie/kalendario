using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Entities;

namespace Kalendario.Application.ResourceModels.User;

public class EmployeeUserResourceModel : IMapFrom<Employee>
{
    public Guid Id { get; set; }

    [Required] public string Name { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public string PhotoUrl { get; set; }

    public ScheduleUserResourceModel Schedule { get; set; }

    public List<ServiceUserResourceModel> Services { get; set; }
}