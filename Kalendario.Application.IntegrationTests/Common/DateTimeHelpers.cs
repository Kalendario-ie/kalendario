using System;

namespace Kalendario.Application.IntegrationTests.Common;

public static class DateTimeHelpers
{
    public static DateTime NextDayOfWeek(DayOfWeek dayOfWeek)
    {
        return DateTime.Now.AddDays(7 + (dayOfWeek - DateTime.Now.DayOfWeek) % 7).Date;
    }
    
    public static DateOnly DNextDayOfWeek(DayOfWeek dayOfWeek)
    {
        return DateOnly.FromDateTime(NextDayOfWeek(dayOfWeek));
    }
}