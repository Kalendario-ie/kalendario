using System.Collections.Generic;
using Kalendario.Application.ResourceModels.Public;

namespace Kalendario.Application.Results.Public;

public class SearchCompaniesResult
{
    public ICollection<CompanySearchResourceModel> Entities { get; set; }
}