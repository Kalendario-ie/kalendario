using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Entities;

namespace Kalendario.Application.ResourceModels.Admin;

public class EmployeeAdminResourceModel : IMapFrom<Employee>
{
    public Guid Id { get; set; }

    [Required] public string Name { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public Guid? ScheduleId { get; set; }

    public List<Guid> Services { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Employee, EmployeeAdminResourceModel>()
            .ForMember(m => m.Services, e => e.MapFrom(e => e.EmployeeServices.Select(s => s.ServiceId)));
    }
}