using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Kalendario.Application.Commands.Admin;
using Kalendario.Application.Common.Exceptions;
using Kalendario.Application.IntegrationTests.Common;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Core.Entities;
using NUnit.Framework;

namespace Kalendario.Application.IntegrationTests.Commands;

using static Testing;

public class CreateScheduleCommandTests : TestBase
{
    private Schedule TestSchedule => new()
    {
        Name = "Example",
        AccountId = Constants.CurrentUserAccountId,
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

    [Test]
    public async Task UnauthenticatedUser_ShouldThrow_AuthenticationException()
    {
        RunAsAnonymousUser();
        var command = new UpsertScheduleCommand();
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task UnauthorizedUser_ShouldThrow_ForbiddenAccessException()
    {
        await RunAsDefaultUserAsync();
        var command = new UpsertScheduleCommand();
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task UserWithCreateRoleOnly_ShouldGetForbiddenAccess_OnUpdateTry()
    {
        await RunAsAdministratorAsync(typeof(Schedule), Schedule.CreateRole);
        var command = new UpsertScheduleCommand {Id = Guid.NewGuid()};
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ForbiddenAccessException>();
    }


    [Test]
    public async Task UserWithUpdateRoleOnly_ShouldGetForbiddenAccess_OnCreateTry()
    {
        await RunAsAdministratorAsync(typeof(Schedule), Schedule.UpdateRole);
        var command = new UpsertScheduleCommand();
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task UpsertToAnotherAccountSchedule_ShouldThrow_NotFoundException()
    {
        await RunAsAdministratorAsync(typeof(Schedule), Schedule.UpdateRole);
        var scheduleId = await AddAsync(new Schedule {AccountId = Constants.RandomAccountId, Name = "Test"});
        var command = new UpsertScheduleCommand() {Id = scheduleId};
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();

        var schedule = await FindAsync<Schedule>(scheduleId);
        Assert.AreEqual(scheduleId, schedule.Id);
    }

    [Test]
    public async Task Update_NoChanges_ShouldReturnUnchangedSchedule()
    {
        await RunAsAdministratorAsync(typeof(Schedule), Schedule.UpdateRole, Constants.CurrentUserAccountId);

        var schedule = TestSchedule;
        var scheduleId = await AddAsync(schedule);

        var command = new UpsertScheduleCommand
        {
            Id = scheduleId,
            Name = schedule.Name,
            Sunday = ConvertFrames(schedule.Sunday),
            Monday = ConvertFrames(schedule.Monday),
            Tuesday = ConvertFrames(schedule.Tuesday),
            Wednesday = ConvertFrames(schedule.Wednesday),
            Thursday = ConvertFrames(schedule.Thursday),
            Friday = ConvertFrames(schedule.Friday),
            Saturday = ConvertFrames(schedule.Saturday),
        };
        var result = await FluentActions.Invoking(() => SendAsync(command)).Invoke();

        Assert.AreEqual(schedule.Name, result.Name);

        Assert.AreEqual(3, result.Monday.Count);
        schedule.Monday.ForEach(f => AssertFrameTimes(result.Monday[f.Order], f.Start, f.End));

        Assert.AreEqual(2, result.Tuesday.Count);
        schedule.Tuesday.ForEach(f => AssertFrameTimes(result.Tuesday[f.Order], f.Start, f.End));

        Assert.AreEqual(1, result.Wednesday.Count);
        schedule.Wednesday.ForEach(f => AssertFrameTimes(result.Wednesday[f.Order], f.Start, f.End));

        Assert.AreEqual(1, result.Thursday.Count);
        schedule.Thursday.ForEach(f => AssertFrameTimes(result.Thursday[f.Order], f.Start, f.End));

        Assert.AreEqual(1, result.Friday.Count);
        schedule.Friday.ForEach(f => AssertFrameTimes(result.Friday[f.Order], f.Start, f.End));

        Assert.AreEqual(1, result.Saturday.Count);
        schedule.Saturday.ForEach(f => AssertFrameTimes(result.Saturday[f.Order], f.Start, f.End));

        Assert.AreEqual(1, result.Sunday.Count);
        schedule.Sunday.ForEach(f => AssertFrameTimes(result.Sunday[f.Order], f.Start, f.End));

        var frames = await WhereAsync<ScheduleFrame>(f => f.ScheduleId == scheduleId);
        Assert.AreEqual(10, frames.Count);
        frames.ForEach(f => { Assert.AreEqual(Constants.CurrentUserAccountId, f.AccountId); });
    }

    [Test]
    public async Task Update_RemoveFrames_ShouldReturnScheduleWithLessFrames()
    {
        await RunAsAdministratorAsync(typeof(Schedule), Schedule.UpdateRole, Constants.CurrentUserAccountId);

        var schedule = TestSchedule;
        var scheduleId = await AddAsync(schedule);

        var command = new UpsertScheduleCommand
        {
            Id = scheduleId,
            Name = schedule.Name,
            Sunday = new List<CreateScheduleFrame>(),
            Monday = ConvertFrames(schedule.Monday.Take(2)),
            Tuesday = ConvertFrames(schedule.Tuesday.Take(1)),
            Wednesday = new List<CreateScheduleFrame>(),
            Thursday = new List<CreateScheduleFrame>(),
            Friday = new List<CreateScheduleFrame>(),
            Saturday = new List<CreateScheduleFrame>(),
        };

        var result = await FluentActions.Invoking(() => SendAsync(command)).Invoke();

        Assert.AreEqual(schedule.Name, result.Name);

        Assert.AreEqual(2, result.Monday.Count);
        schedule.Monday.Take(2).ToList().ForEach(f => AssertFrameTimes(result.Monday[f.Order], f.Start, f.End));

        Assert.AreEqual(1, result.Tuesday.Count);
        schedule.Tuesday.Take(1).ToList().ForEach(f => AssertFrameTimes(result.Tuesday[f.Order], f.Start, f.End));

        Assert.AreEqual(0, result.Wednesday.Count);
        Assert.AreEqual(0, result.Thursday.Count);
        Assert.AreEqual(0, result.Friday.Count);
        Assert.AreEqual(0, result.Saturday.Count);
        Assert.AreEqual(0, result.Sunday.Count);

        var frames = await WhereAsync<ScheduleFrame>(f => f.ScheduleId == scheduleId);
        Assert.AreEqual(3, frames.Count);
        frames.ForEach(f => { Assert.AreEqual(Constants.CurrentUserAccountId, f.AccountId); });
    }

    [Test]
    public async Task Update_AddFrames_ShouldReturnScheduleWitMoreFrames()
    {
        await RunAsAdministratorAsync(typeof(Schedule), Schedule.UpdateRole, Constants.CurrentUserAccountId);

        var schedule = TestSchedule;
        var scheduleId = await AddAsync(schedule);
        var newFrameStart = TimeOnly.Parse("19:00");
        var newFrameEnd = TimeOnly.Parse("20:00");

        var command = new UpsertScheduleCommand
        {
            Id = scheduleId,
            Name = schedule.Name,
            Sunday = AddFrame(schedule.Sunday, newFrameStart, newFrameEnd),
            Monday = AddFrame(schedule.Monday, newFrameStart, newFrameEnd),
            Tuesday = AddFrame(schedule.Tuesday, newFrameStart, newFrameEnd),
            Wednesday = AddFrame(schedule.Wednesday, newFrameStart, newFrameEnd),
            Thursday = AddFrame(schedule.Thursday, newFrameStart, newFrameEnd),
            Friday = AddFrame(schedule.Friday, newFrameStart, newFrameEnd),
            Saturday = AddFrame(schedule.Saturday, newFrameStart, newFrameEnd),
        };

        var result = await FluentActions.Invoking(() => SendAsync(command)).Invoke();

        Assert.AreEqual(schedule.Name, result.Name);

        Assert.AreEqual(4, result.Monday.Count);
        schedule.Monday.ForEach(f => AssertFrameTimes(result.Monday[f.Order], f.Start, f.End));
        AssertFrameTimes(result.Monday[3], newFrameStart, newFrameEnd);

        Assert.AreEqual(3, result.Tuesday.Count);
        schedule.Tuesday.ForEach(f => AssertFrameTimes(result.Tuesday[f.Order], f.Start, f.End));
        AssertFrameTimes(result.Tuesday[2], newFrameStart, newFrameEnd);

        Assert.AreEqual(2, result.Wednesday.Count);
        schedule.Wednesday.ForEach(f => AssertFrameTimes(result.Wednesday[f.Order], f.Start, f.End));
        AssertFrameTimes(result.Wednesday[1], newFrameStart, newFrameEnd);

        Assert.AreEqual(2, result.Thursday.Count);
        schedule.Thursday.ForEach(f => AssertFrameTimes(result.Thursday[f.Order], f.Start, f.End));
        AssertFrameTimes(result.Thursday[1], newFrameStart, newFrameEnd);

        Assert.AreEqual(2, result.Friday.Count);
        schedule.Friday.ForEach(f => AssertFrameTimes(result.Friday[f.Order], f.Start, f.End));
        AssertFrameTimes(result.Friday[1], newFrameStart, newFrameEnd);

        Assert.AreEqual(2, result.Saturday.Count);
        schedule.Saturday.ForEach(f => AssertFrameTimes(result.Saturday[f.Order], f.Start, f.End));
        AssertFrameTimes(result.Saturday[1], newFrameStart, newFrameEnd);

        Assert.AreEqual(2, result.Sunday.Count);
        schedule.Sunday.ForEach(f => AssertFrameTimes(result.Sunday[f.Order], f.Start, f.End));
        AssertFrameTimes(result.Sunday[1], newFrameStart, newFrameEnd);

        var frames = await WhereAsync<ScheduleFrame>(f => f.ScheduleId == scheduleId);
        Assert.AreEqual(17, frames.Count);
        frames.ForEach(f => { Assert.AreEqual(Constants.CurrentUserAccountId, f.AccountId); });
    }

    [Test]
    public async Task Update_UpdateFrames_ShouldReturnScheduleWithUpdatedFrames()
    {
        await RunAsAdministratorAsync(typeof(Schedule), Schedule.UpdateRole, Constants.CurrentUserAccountId);

        var schedule = TestSchedule;
        var scheduleId = await AddAsync(schedule);
        var newFrameStart = TimeOnly.Parse("06:00");
        var newFrameEnd = TimeOnly.Parse("07:00");

        var command = new UpsertScheduleCommand
        {
            Id = scheduleId,
            Name = schedule.Name,
            Sunday = UpdateFrame(schedule.Sunday, newFrameStart, newFrameEnd),
            Monday = UpdateFrame(schedule.Monday, newFrameStart, newFrameEnd),
            Tuesday = UpdateFrame(schedule.Tuesday, newFrameStart, newFrameEnd),
            Wednesday = UpdateFrame(schedule.Wednesday, newFrameStart, newFrameEnd),
            Thursday = UpdateFrame(schedule.Thursday, newFrameStart, newFrameEnd),
            Friday = UpdateFrame(schedule.Friday, newFrameStart, newFrameEnd),
            Saturday = UpdateFrame(schedule.Saturday, newFrameStart, newFrameEnd),
        };

        var result = await FluentActions.Invoking(() => SendAsync(command)).Invoke();

        Assert.AreEqual(schedule.Name, result.Name);

        Assert.AreEqual(3, result.Monday.Count);
        AssertFrameTimes(result.Monday[0], newFrameStart, newFrameEnd);
        schedule.Monday.Skip(1).ToList().ForEach(f => AssertFrameTimes(result.Monday[f.Order], f.Start, f.End));

        Assert.AreEqual(2, result.Tuesday.Count);
        AssertFrameTimes(result.Tuesday[0], newFrameStart, newFrameEnd);
        schedule.Tuesday.Skip(1).ToList().ForEach(f => AssertFrameTimes(result.Tuesday[f.Order], f.Start, f.End));

        Assert.AreEqual(1, result.Wednesday.Count);
        AssertFrameTimes(result.Wednesday[0], newFrameStart, newFrameEnd);

        Assert.AreEqual(1, result.Thursday.Count);
        AssertFrameTimes(result.Thursday[0], newFrameStart, newFrameEnd);

        Assert.AreEqual(1, result.Friday.Count);
        AssertFrameTimes(result.Friday[0], newFrameStart, newFrameEnd);

        Assert.AreEqual(1, result.Saturday.Count);
        AssertFrameTimes(result.Saturday[0], newFrameStart, newFrameEnd);

        Assert.AreEqual(1, result.Sunday.Count);
        AssertFrameTimes(result.Sunday[0], newFrameStart, newFrameEnd);

        var frames = await WhereAsync<ScheduleFrame>(f => f.ScheduleId == scheduleId);
        Assert.AreEqual(10, frames.Count);
        frames.ForEach(f => { Assert.AreEqual(Constants.CurrentUserAccountId, f.AccountId); });
    }

    [Test]
    public async Task Create_ShouldReturnNewSchedule()
    {
        await RunAsAdministratorAsync(typeof(Schedule), Schedule.CreateRole, Constants.CurrentUserAccountId);

        var schedule = TestSchedule;
        var scheduleId = await AddAsync(schedule);

        var command = new UpsertScheduleCommand
        {
            Name = schedule.Name,
            Sunday = ConvertFrames(schedule.Sunday),
            Monday = ConvertFrames(schedule.Monday),
            Tuesday = ConvertFrames(schedule.Tuesday),
            Wednesday = ConvertFrames(schedule.Wednesday),
            Thursday = ConvertFrames(schedule.Thursday),
            Friday = ConvertFrames(schedule.Friday),
            Saturday = ConvertFrames(schedule.Saturday),
        };

        var result = await FluentActions.Invoking(() => SendAsync(command)).Invoke();

        Assert.AreEqual(schedule.Name, result.Name);

        Assert.AreEqual(3, result.Monday.Count);
        schedule.Monday.ForEach(f => AssertFrameTimes(result.Monday[f.Order], f.Start, f.End));

        Assert.AreEqual(2, result.Tuesday.Count);
        schedule.Tuesday.ForEach(f => AssertFrameTimes(result.Tuesday[f.Order], f.Start, f.End));

        Assert.AreEqual(1, result.Wednesday.Count);
        schedule.Wednesday.ForEach(f => AssertFrameTimes(result.Wednesday[f.Order], f.Start, f.End));

        Assert.AreEqual(1, result.Thursday.Count);
        schedule.Thursday.ForEach(f => AssertFrameTimes(result.Thursday[f.Order], f.Start, f.End));

        Assert.AreEqual(1, result.Friday.Count);
        schedule.Friday.ForEach(f => AssertFrameTimes(result.Friday[f.Order], f.Start, f.End));

        Assert.AreEqual(1, result.Saturday.Count);
        schedule.Saturday.ForEach(f => AssertFrameTimes(result.Saturday[f.Order], f.Start, f.End));

        Assert.AreEqual(1, result.Sunday.Count);
        schedule.Sunday.ForEach(f => AssertFrameTimes(result.Sunday[f.Order], f.Start, f.End));

        var schedule1Frames = await WhereAsync<ScheduleFrame>(f => f.ScheduleId == scheduleId);
        Assert.AreEqual(10, schedule1Frames.Count);
        schedule1Frames.ForEach(f => { Assert.AreEqual(Constants.CurrentUserAccountId, f.AccountId); });
        
        var schedule2Frames = await WhereAsync<ScheduleFrame>(f => f.ScheduleId == result.Id);
        Assert.AreEqual(10, schedule2Frames.Count);
        schedule2Frames.ForEach(f => { Assert.AreEqual(Constants.CurrentUserAccountId, f.AccountId); });
    }


    private ScheduleFrame CreateFrame(DayOfWeek offset, int order, string start, string end)
    {
        return new ScheduleFrame
        {
            Offset = offset, Order = order, AccountId = Constants.CurrentUserAccountId,
            Start = TimeOnly.Parse(start), End = TimeOnly.Parse(end)
        };
    }

    private IEnumerable<CreateScheduleFrame> ConvertFrames(IEnumerable<ScheduleFrame> frames)
    {
        return frames.Select(f => new CreateScheduleFrame {Start = f.Start, End = f.End});
    }

    private IEnumerable<CreateScheduleFrame> AddFrame(IEnumerable<ScheduleFrame> frames, TimeOnly start, TimeOnly end)
    {
        return ConvertFrames(frames)
            .Append(new CreateScheduleFrame() {Start = start, End = end});
    }

    private IEnumerable<CreateScheduleFrame> UpdateFrame(List<ScheduleFrame> frames, TimeOnly start, TimeOnly end)
    {
        frames[0].Start = start;
        frames[0].End = end;
        return ConvertFrames(frames);
    }

    private void AssertFrameTimes(ScheduleFrameAdminResourceModel resourceModel, TimeOnly start, TimeOnly end)
    {
        Assert.AreEqual(resourceModel.Start, start);
        Assert.AreEqual(resourceModel.End, end);
    }
}