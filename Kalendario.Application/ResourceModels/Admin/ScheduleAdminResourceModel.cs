using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Entities;

namespace Kalendario.Application.ResourceModels.Admin;

public class ScheduleAdminResourceModel : IMapFrom<Schedule>
{
    public Guid Id { get; set; }

    [Required] public string Name { get; set; }

    [Required] public List<ScheduleFrameAdminResourceModel> Sunday { get; set; } = new();

    [Required] public List<ScheduleFrameAdminResourceModel> Monday { get; set; } = new();
    
    [Required] public List<ScheduleFrameAdminResourceModel> Tuesday { get; set; } = new();
    
    [Required] public List<ScheduleFrameAdminResourceModel> Wednesday { get; set; } = new();
    
    [Required] public List<ScheduleFrameAdminResourceModel> Thursday { get; set; } = new();
    
    [Required] public List<ScheduleFrameAdminResourceModel> Friday { get; set; } = new();
    
    [Required] public List<ScheduleFrameAdminResourceModel> Saturday { get; set; } = new();
}