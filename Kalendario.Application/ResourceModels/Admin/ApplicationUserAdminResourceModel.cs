using System;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Infrastructure;

namespace Kalendario.Application.ResourceModels.Admin;

public class ApplicationUserAdminResourceModel : IMapFrom<ApplicationUser>
{
    public Guid Id { get; set; }

    public string UserName { get; set; }
}