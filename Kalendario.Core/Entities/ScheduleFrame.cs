using System;

namespace Kalendario.Core.Entities;

public class ScheduleFrame : AccountEntity
{
    public DayOfWeek Offset { get; set; }

    public int Order { get; set; }

    public TimeOnly Start { get; set; }
    
    public TimeOnly End { get; set; }

    public Guid ScheduleId { get; set; }

    public Schedule Schedule { get; set; }
}