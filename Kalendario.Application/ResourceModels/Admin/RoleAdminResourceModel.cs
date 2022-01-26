using System;
using System.ComponentModel.DataAnnotations;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Infrastructure;

namespace Kalendario.Application.ResourceModels.Admin;

public class RoleAdminResourceModel : IMapFrom<ApplicationRole>
{
    public Guid Id { get; set; }
    [Required] public string Name { get; set; }
}