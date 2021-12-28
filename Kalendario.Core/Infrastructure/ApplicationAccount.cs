using System.Collections.Generic;
using Kalendario.Core.Domain;

namespace Kalendario.Core.Infrastructure;

public class ApplicationAccount : BaseAccount
{
    public ICollection<ApplicationUser> Users { get; set; }
}