using System;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Entities;

namespace Kalendario.Application.ResourceModels.Admin;

public class ScheduleFrameAdminResourceModel : IMapFrom<ScheduleFrame>
{
    public TimeOnly Start { get; set; }
    
    public TimeOnly End { get; set; }
}