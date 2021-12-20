using System.Linq;
using Kalendario.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Common.Interfaces
{
    public interface IKalendarioDbContext
    {
        DbSet<Account> Accounts { get; set; }

        DbSet<Employee> Employees { get; set; }
        DbSet<Service> Services { get; set; }
        DbSet<Customer> Customers { get; set; }

        DbSet<TDomain> GetDbSet<TDomain>(IKalendarioDbContext context) where TDomain : class
        {
            var propertyInfo = typeof(IKalendarioDbContext)
                .GetProperties()
                .FirstOrDefault(t => t.PropertyType == typeof(DbSet<TDomain>));

            return (DbSet<TDomain>) propertyInfo?.GetValue(context);
        }
    }
}