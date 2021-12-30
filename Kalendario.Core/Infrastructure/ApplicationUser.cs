using System;
using Kalendario.Core.Domain;
using Microsoft.AspNetCore.Identity;

namespace Kalendario.Core.Infrastructure;

public class ApplicationUser : IdentityUser
{
    public Guid? AccountId { get; set; }
    public Account Account { get; set; }
}