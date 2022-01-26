using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Entities;
using Kalendario.Core.Infrastructure;

namespace Kalendario.Application.ResourceModels.Admin;

public class RoleGroupAdminResourceModel : IMapFrom<ApplicationRoleGroup>
{
    public Guid Id { get; set; }

    [Required] public string Name { get; set; }

    public IEnumerable<Guid> Roles { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ApplicationRoleGroup, RoleGroupAdminResourceModel>()
            .ForMember(m => m.Roles,
                e => e
                    .MapFrom(e => e.RoleGroupRoles.Select(s => s.RoleId)));
    }
}