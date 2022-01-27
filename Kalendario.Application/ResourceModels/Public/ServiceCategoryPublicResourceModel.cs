using System;
using System.ComponentModel.DataAnnotations;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Entities;
using Kalendario.Core.ValueObject;

namespace Kalendario.Application.ResourceModels.Public;

public class ServiceCategoryPublicResourceModel : IMapFrom<ServiceCategory>
{
    public Guid Id { get; set; }

    [Required] public string Name { get; set; }

    public Colour Colour { get; set; } = Colour.White;
}