using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Entities;

namespace Kalendario.Application.ResourceModels.Public;

public class CompanyDetailsResourceModel : IMapFrom<Account>
{
    public Guid Id { get; set; }

    [Required] public string Name { get; set; }

    public string Avatar { get; set; }

    public ICollection<ServicePublicResourceModel> Services { get; set; }

    public ICollection<ServiceCategoryPublicResourceModel> ServiceCategories { get; set; }
}