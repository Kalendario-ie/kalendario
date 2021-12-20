using Kalendario.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Common.Interfaces
{
    public interface IKalendarioDbContext
    {
        DbSet<Account> Accounts { get; set; }

        DbSet<Employee> Employees { get; set; }
        DbSet<Service> Services { get; set; }
    }
}