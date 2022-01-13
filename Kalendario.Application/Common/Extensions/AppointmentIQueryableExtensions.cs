using System;
using System.Linq;
using Kalendario.Core.Entities;

namespace Kalendario.Application.Common.Extensions;

public static class AppointmentIQueryableExtensions
{
    public static IQueryable<Appointment> BetweenDates(this IQueryable<Appointment> queryable, DateTime fromDate, DateTime toDate)
    {
        //TODO: create unit tests for this method.
        return queryable.Where(a => a.Start >= fromDate && a.Start <= toDate ||
                                                 a.End >= fromDate && a.End <= toDate ||
                                                 a.Start <= fromDate && a.End >= toDate);
    }
}