using System;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Entities;

namespace Kalendario.Application.ResourceModels.Public;

public class ServicePublicResourceModel : IMapFrom<Service>
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public double Price { get; set; }

    public TimeSpan Duration { get; set; }

    public Guid? ServiceCategoryId { get; set; }
}