using System;
using System.ComponentModel.DataAnnotations;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Entities;

namespace Kalendario.Application.ResourceModels.Admin;

public class CustomerAdminResourceModel : IMapFrom<Customer>
{
    public Guid Id { get; set; }
    [Required] public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Warning { get; set; }
}