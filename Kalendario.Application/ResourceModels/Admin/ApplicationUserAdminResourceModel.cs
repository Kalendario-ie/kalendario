using System;
using System.ComponentModel.DataAnnotations;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Infrastructure;

namespace Kalendario.Application.ResourceModels.Admin;

public class ApplicationUserAdminResourceModel : IMapFrom<ApplicationUser>
{
    [Required] public string Id { get; set; }

    [Required] public string Name => UserName;

    public string UserName { get; set; }

    public string Email { get; set; }

    public Guid? RoleGroupId { get; set; }

    public Guid? EmployeeId { get; set; }
}