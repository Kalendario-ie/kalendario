using System;
using Microsoft.AspNetCore.Identity;

namespace Kalendario.Core.Infrastructure;

public class ApplicationUser : IdentityUser
{
    public Guid? AccountId { get; set; }
}