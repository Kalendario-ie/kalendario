using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Entities;

namespace Kalendario.Application.ResourceModels.User;

public class ScheduleUserResourceModel : IMapFrom<Schedule>
{
    public Guid Id { get; set; }

    [Required] public string Name { get; set; }

    [Required] public List<ScheduleFrameResourceModel> Sunday { get; set; } = new();

    [Required] public List<ScheduleFrameResourceModel> Monday { get; set; } = new();
    
    [Required] public List<ScheduleFrameResourceModel> Tuesday { get; set; } = new();
    
    [Required] public List<ScheduleFrameResourceModel> Wednesday { get; set; } = new();
    
    [Required] public List<ScheduleFrameResourceModel> Thursday { get; set; } = new();
    
    [Required] public List<ScheduleFrameResourceModel> Friday { get; set; } = new();
    
    [Required] public List<ScheduleFrameResourceModel> Saturday { get; set; } = new();
}