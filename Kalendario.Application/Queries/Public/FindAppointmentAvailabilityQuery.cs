using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Results.Public;
using Kalendario.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Queries.Public;

public class FindAppointmentAvailabilityQuery : IPublicQuery<FindAppointmentAvailabilityResult>
{
    public Guid ServiceId { get; set; }

    public Guid? EmployeeId { get; set; }

    public DateOnly Date { get; set; }

    private DateTime FromDate => Date.ToDateTime(TimeOnly.MinValue).ToUniversalTime();
    private DateTime ToDate => Date.ToDateTime(TimeOnly.Parse("23:59")).ToUniversalTime();

    public class Handler : IRequestHandler<FindAppointmentAvailabilityQuery, FindAppointmentAvailabilityResult>
    {
        private readonly IKalendarioDbContext _context;

        public Handler(IKalendarioDbContext context)
        {
            _context = context;
        }

        public async Task<FindAppointmentAvailabilityResult> Handle(FindAppointmentAvailabilityQuery request,
            CancellationToken cancellationToken)
        {
            var result = new FindAppointmentAvailabilityResult();

            if (request.Date < DateOnly.FromDateTime(DateTime.UtcNow))
                return result;
            
            var service = await _context.Services
                .Include(e => e.Employees)
                .ThenInclude(e => e.Schedule)
                .ThenInclude(s => s.Frames)
                .Include(e => e.Appointments.Where(a => a.Start >= request.FromDate && a.Start <= request.ToDate ||
                                                        a.End >= request.FromDate && a.End <= request.ToDate ||
                                                        a.Start <= request.FromDate && a.End >= request.ToDate))
                .FirstOrDefaultAsync(s => s.Id == request.ServiceId, cancellationToken);

            if (service == null)
                return result;

            var employees = request.EmployeeId.HasValue
                ? service.Employees.Where(e => e.Id == request.EmployeeId.Value)
                : service.Employees;

            foreach (var employee in employees)
            {
                var frames = employee.Schedule.FramesOf(request.Date.DayOfWeek);
                var newFrames = new List<ScheduleFrame>();

                foreach (var frame in frames)
                {
                    var frameStart = request.Date.ToDateTime(frame.Start);
                    var frameEnd = request.Date.ToDateTime(frame.End);
                    var appointments = employee.Appointments
                        .Where(a => a.IsBetweenDates(frameStart, frameEnd))
                        .OrderBy(a => a.Start)
                        .ToList();

                    if (appointments.Count == 0)
                    {
                        newFrames.Add(frame);
                        continue;
                    }
                    
                    if (appointments.Any(a => a.Start <= frameStart && a.End >= frameEnd))
                        continue;

                    for (var index = 0; index < appointments.Count; index++)
                    {
                        var previous = index == 0 ? null : appointments[index - 1];
                        var appointment = appointments[index];
                        var next = index + 1 == appointments.Count ? null : appointments[index + 1];
                        
                        if (appointment.Start <= frameStart)
                        {
                            newFrames.Add(new ScheduleFrame
                            {
                                Start = TimeOnly.FromDateTime(appointment.End),
                                End = next != null ? TimeOnly.FromDateTime(next.Start) : frame.End
                            });
                        }
                        else
                        {
                            newFrames.Add(new ScheduleFrame
                            {
                                Start = previous != null ? TimeOnly.FromDateTime(previous.End) : frame.Start,
                                End = TimeOnly.FromDateTime(appointment.Start)
                            });

                            if (appointment.End < frameEnd)
                            {
                                newFrames.Add(new ScheduleFrame
                                {
                                    Start = TimeOnly.FromDateTime(appointment.End),
                                    End = next != null ? TimeOnly.FromDateTime(next.Start) : frame.End
                                });
                            }
                        }
                    }
                }

                foreach (var frame in newFrames)
                {
                    var capacity = (int)((frame.End - frame.Start) / service.Duration);
                    var startTime = frame.Start;
                    for (var i = 0; i < capacity; i++)
                    {
                        result.Slots.Add(new Slot()
                        {
                            Start = request.Date.ToDateTime(startTime.Add(service.Duration * i)),
                            End = request.Date.ToDateTime(startTime.Add(service.Duration * ( i+ 1)))
                        });
                    }
                }

            }

            return result;
        }
    }
}