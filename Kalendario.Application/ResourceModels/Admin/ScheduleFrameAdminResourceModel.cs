﻿using System;
using AutoMapper;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Entities;

namespace Kalendario.Application.ResourceModels.Admin;

public class ScheduleFrameAdminResourceModel : IMapFrom<ScheduleFrame>
{
    public TimeSpan Start { get; set; }

    public TimeSpan End { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ScheduleFrame, ScheduleFrameAdminResourceModel>()
            .ForMember(rm => rm.Start, e => e.MapFrom(s => s.Start.ToTimeSpan()))
            .ForMember(rm => rm.End, e => e.MapFrom(s => s.End.ToTimeSpan()));
    }
}