using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Entities;

namespace Kalendario.Application.ResourceModels.Admin
{
    public class SchedulingPanelAdminResourceModel : IMapFrom<SchedulingPanel>
    {
        public Guid Id { get; set; }

        [Required] public string Name { get; set; }

        public IEnumerable<Guid> EmployeeIds { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SchedulingPanel, SchedulingPanelAdminResourceModel>()
                .ForMember(m => m.EmployeeIds,
                    expression => expression
                        .MapFrom(schedulingPanel => schedulingPanel.Employees.Select(e => e.Id)));
        }
    }
}