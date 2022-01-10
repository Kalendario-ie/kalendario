using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Kalendario.Application.Commands.Admin.Common;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Commands.Admin;

[Authorize(typeof(Schedule), $"{Schedule.CreateRole},{Schedule.UpdateRole}")]
public class UpsertScheduleCommand : BaseUpsertAdminCommand<ScheduleAdminResourceModel>
{
    public string Name { get; set; }
    public IEnumerable<CreateScheduleFrame> Sunday { get; set; }
    public IEnumerable<CreateScheduleFrame> Monday { get; set; }
    public IEnumerable<CreateScheduleFrame> Tuesday { get; set; }
    public IEnumerable<CreateScheduleFrame> Wednesday { get; set; }
    public IEnumerable<CreateScheduleFrame> Thursday { get; set; }
    public IEnumerable<CreateScheduleFrame> Friday { get; set; }
    public IEnumerable<CreateScheduleFrame> Saturday { get; set; }

    public class Handler : BaseUpsertAdminCommandHandler<UpsertScheduleCommand, Schedule, ScheduleAdminResourceModel>
    {
        public Handler(IKalendarioDbContext context, IMapper mapper, ICurrentUserManager currentUserManager) 
            : base(context, mapper, currentUserManager)
        {
        }

        protected override IQueryable<Schedule> Entities => Context.Schedules.Include(s => s.Frames);

        protected override void UpdateDomain(Schedule domain, UpsertScheduleCommand request)
        {
            domain.Name = request.Name;

            var frames = ConvertToFrame(request.Sunday, DayOfWeek.Sunday)
                .Concat(ConvertToFrame(request.Monday, DayOfWeek.Monday))
                .Concat(ConvertToFrame(request.Tuesday, DayOfWeek.Tuesday))
                .Concat(ConvertToFrame(request.Wednesday, DayOfWeek.Wednesday))
                .Concat(ConvertToFrame(request.Thursday, DayOfWeek.Thursday))
                .Concat(ConvertToFrame(request.Friday, DayOfWeek.Friday))
                .Concat(ConvertToFrame(request.Saturday, DayOfWeek.Saturday))
                .ToList();

            domain.Frames = frames;
        }

        protected override Task AdditionalValidation(UpsertScheduleCommand request)
        {
            return Task.CompletedTask;
        }

        private IEnumerable<ScheduleFrame> ConvertToFrame(IEnumerable<CreateScheduleFrame> frames, DayOfWeek dayOfWeek)
        {
            return frames.Select((f, i) => new ScheduleFrame
            {
                Start = TimeOnly.FromTimeSpan(f.Start), End = TimeOnly.FromTimeSpan(f.End), Order = i, Offset = dayOfWeek, AccountId = CurrentUserManager.CurrentUserAccountId
            });
        }
    }
}

public class CreateScheduleFrame
{
    public TimeSpan Start { get; set; }

    public TimeSpan End { get; set; }
}