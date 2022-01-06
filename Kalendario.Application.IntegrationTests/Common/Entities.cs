using System;
using System.Collections.Generic;
using Kalendario.Core.Entities;

namespace Kalendario.Application.IntegrationTests.Common;

public class Entities
{
    public static Schedule TestSchedule(string accountId = Constants.CurrentUserAccountIdString) => new()
    {
        Name = "Example",
        AccountId = Guid.Parse(accountId),
        Frames = new List<ScheduleFrame>
        {
            CreateFrame(DayOfWeek.Sunday, 0, "09:00", "17:00"),
            CreateFrame(DayOfWeek.Monday, 0, "09:00", "13:00"),
            CreateFrame(DayOfWeek.Monday, 1, "14:00", "15:00"),
            CreateFrame(DayOfWeek.Monday, 2, "16:00", "17:00"),
            CreateFrame(DayOfWeek.Tuesday, 0, "09:00", "14:00"),
            CreateFrame(DayOfWeek.Tuesday, 1, "14:00", "17:00"),
            CreateFrame(DayOfWeek.Wednesday, 0, "10:00", "11:00"),
            CreateFrame(DayOfWeek.Thursday, 0, "11:00", "12:00"),
            CreateFrame(DayOfWeek.Friday, 0, "12:00", "13:00"),
            CreateFrame(DayOfWeek.Saturday, 0, "13:00", "14:00"),
        }
    };
    
    private static ScheduleFrame CreateFrame(DayOfWeek offset, int order, string start, string end)
    {
        return new ScheduleFrame
        {
            Offset = offset, Order = order, AccountId = Constants.CurrentUserAccountId,
            Start = TimeOnly.Parse(start), End = TimeOnly.Parse(end)
        };
    }
}