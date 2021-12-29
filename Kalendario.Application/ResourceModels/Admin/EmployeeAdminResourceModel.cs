using System;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Domain;

namespace Kalendario.Application.ResourceModels.Admin;

public class EmployeeAdminResourceModel : IMapFrom<Employee>
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }
}