using System;
using System.ComponentModel.DataAnnotations;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Entities;

namespace Kalendario.Application.ResourceModels.Public;

public class CompanySearchResourceModel : IMapFrom<Account>
{
    public Guid Id { get; set; }

    [Required] public string Name { get; set; }

    public string Avatar { get; set; }
}