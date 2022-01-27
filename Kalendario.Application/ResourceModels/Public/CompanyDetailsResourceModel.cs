using System;
using System.Collections;
using System.Collections.Generic;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Entities;

namespace Kalendario.Application.ResourceModels.Public;

public class CompanyDetailsResourceModel : IMapFrom<Account>
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Avatar { get; set; }

    public ICollection<ServicePublicResourceModel> Services { get; set; }

    public ICollection<ServiceCategoryPublicResourceModel> ServiceCategories { get; set; }
}