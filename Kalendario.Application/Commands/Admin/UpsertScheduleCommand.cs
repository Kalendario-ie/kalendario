using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Kalendario.Application.Authorization;
using Kalendario.Application.Common.Exceptions;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Commands.Admin;

[Authorize(typeof(Schedule), $"{Schedule.CreateRole},{Schedule.UpdateRole}")]
public class UpsertScheduleCommand : IKalendarioProtectedCommand<ScheduleAdminResourceModel>
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<CreateScheduleFrame> Sunday { get; set; }
    public IEnumerable<CreateScheduleFrame> Monday { get; set; }
    public IEnumerable<CreateScheduleFrame> Tuesday { get; set; }
    public IEnumerable<CreateScheduleFrame> Wednesday { get; set; }
    public IEnumerable<CreateScheduleFrame> Thursday { get; set; }
    public IEnumerable<CreateScheduleFrame> Friday { get; set; }
    public IEnumerable<CreateScheduleFrame> Saturday { get; set; }

    public class Handler : IRequestHandler<UpsertScheduleCommand, ScheduleAdminResourceModel>
    {
        private readonly IKalendarioDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;

        public Handler(IKalendarioDbContext context, IMapper mapper, ICurrentUserService currentUserService,
            IIdentityService identityService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _identityService = identityService;
        }

        public async Task<ScheduleAdminResourceModel> Handle(UpsertScheduleCommand request,
            CancellationToken cancellationToken)
        {
            if (!(await _identityService.IsInRoleAsync(_currentUserService.UserId,
                    AuthorizationHelper.RoleName(typeof(Schedule), Schedule.CreateRole))) && !request.Id.HasValue)
            {
                throw new ForbiddenAccessException();
            }

            if (!(await _identityService.IsInRoleAsync(_currentUserService.UserId,
                    AuthorizationHelper.RoleName(typeof(Schedule), Schedule.UpdateRole))) && request.Id.HasValue)
            {
                throw new ForbiddenAccessException();
            }

            var schedule = await (request.Id.HasValue
                ? _context.Schedules
                    .Include(a => a.Frames)
                    .Where(e => e.AccountId == _currentUserService.AccountId)
                    .FirstOrDefaultAsync(e => e.Id == request.Id.Value, cancellationToken)
                : Task.FromResult(new Schedule()));
            
            if (schedule == default)
            {
                throw new NotFoundException(nameof(Schedule), request.Id);
            }
            
            if (schedule.Id == Guid.Empty)
            {
                _context.Schedules.Add(schedule);
                schedule.AccountId = _currentUserService.AccountId;
            }

            schedule.Name = request.Name;
            
            var frames = ConvertToFrame(request.Sunday, DayOfWeek.Sunday)
                .Concat(ConvertToFrame(request.Monday, DayOfWeek.Monday))
                .Concat(ConvertToFrame(request.Tuesday, DayOfWeek.Tuesday))
                .Concat(ConvertToFrame(request.Wednesday, DayOfWeek.Wednesday))
                .Concat(ConvertToFrame(request.Thursday, DayOfWeek.Thursday))
                .Concat(ConvertToFrame(request.Friday, DayOfWeek.Friday))
                .Concat(ConvertToFrame(request.Saturday, DayOfWeek.Saturday))
                .ToList();

            schedule.Frames = frames;

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ScheduleAdminResourceModel>(schedule);
        }

        private IEnumerable<ScheduleFrame> ConvertToFrame(IEnumerable<CreateScheduleFrame> frames, DayOfWeek dayOfWeek)
        {
            return frames.Select((f, i) => new ScheduleFrame {Start = f.Start, End = f.End, Order = i, Offset = dayOfWeek, AccountId = _currentUserService.AccountId});
        }
    }
}

public class CreateScheduleFrame
{
    public TimeOnly Start { get; set; }

    public TimeOnly End { get; set; }
}