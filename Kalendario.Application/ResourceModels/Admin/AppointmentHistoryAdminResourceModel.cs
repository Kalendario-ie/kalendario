using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Entities;
using Kalendario.Core.Infrastructure;

namespace Kalendario.Application.ResourceModels.Admin;

public class AppointmentHistoryAdminResourceModel : HistoryResourceModel, IMapFrom<Appointment>
{
    public Guid Id { get; set; }

    [Required] public string Name => $"{Start}_{End}";
    public CustomerAdminResourceModel Customer { get; set; }

    public EmployeeAdminResourceModel Employee { get; set; }

    public ServiceAdminResourceModel Service { get; set; }

    public DateTime? Start { get; set; }

    public DateTime? End { get; set; }

    public double Price { get; set; }

    public string InternalNotes { get; set; }
    
    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Appointment, AppointmentHistoryAdminResourceModel>()
            .ForMember(m => m.Start,
                o => o.MapFrom(a => a.Start == DateTime.MinValue ? (DateTime?) null : a.Start))
            .ForMember(m => m.End,
                o => o.MapFrom(a => a.End == DateTime.MinValue ? (DateTime?) null : a.End));;
    } 
}