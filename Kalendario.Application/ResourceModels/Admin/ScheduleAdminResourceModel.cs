using System;
using System.Collections.Generic;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Entities;

namespace Kalendario.Application.ResourceModels.Admin;

public class ScheduleAdminResourceModel : IMapFrom<Schedule>
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public List<ScheduleFrameAdminResourceModel> Sunday { get; set; } = new();

    public List<ScheduleFrameAdminResourceModel> Monday { get; set; } = new();
    
    public List<ScheduleFrameAdminResourceModel> Tuesday { get; set; } = new();
    
    public List<ScheduleFrameAdminResourceModel> Wednesday { get; set; } = new();
    
    public List<ScheduleFrameAdminResourceModel> Thursday { get; set; } = new();
    
    public List<ScheduleFrameAdminResourceModel> Friday { get; set; } = new();
    
    public List<ScheduleFrameAdminResourceModel> Saturday { get; set; } = new();
}