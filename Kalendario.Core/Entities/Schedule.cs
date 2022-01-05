using System;
using System.Collections.Generic;
using System.Linq;

namespace Kalendario.Core.Entities;

public class Schedule : AccountEntity
{
    public string Name { get; set; }

    public List<ScheduleFrame> Frames { get; set; } = new();

    public List<ScheduleFrame> Sunday  => FramesOf(DayOfWeek.Sunday);

    public List<ScheduleFrame> Monday => FramesOf(DayOfWeek.Monday);
    
    public List<ScheduleFrame> Tuesday => FramesOf(DayOfWeek.Tuesday);

    public List<ScheduleFrame> Wednesday => FramesOf(DayOfWeek.Wednesday);

    public List<ScheduleFrame> Thursday => FramesOf(DayOfWeek.Thursday);

    public List<ScheduleFrame> Friday => FramesOf(DayOfWeek.Friday);

    public List<ScheduleFrame> Saturday => FramesOf(DayOfWeek.Saturday);

    private List<ScheduleFrame> FramesOf(DayOfWeek dayOfWeek)
    {
        return Frames
            .Where(f => f.Offset == dayOfWeek)
            .OrderBy(f => f.Order)
            .ToList();
    }

}

