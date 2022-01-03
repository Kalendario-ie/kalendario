using System;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Entities;

namespace Kalendario.Application.ResourceModels.Admin;

public class CustomerAdminResourceModel : IMapFrom<Customer>
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public string Name { get; set; }
}