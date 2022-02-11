using System;
using System.ComponentModel.DataAnnotations;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Entities;

namespace Kalendario.Application.ResourceModels.User;

public class CustomerUserResourceModel : IMapFrom<Customer>
{
    public Guid Id { get; set; }
    [Required] public string Name { get; set; }
    public string Warning { get; set; }
}