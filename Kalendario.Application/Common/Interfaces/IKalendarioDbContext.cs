using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kalendario.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Common.Interfaces;

public interface IKalendarioDbContext
{
    DbSet<Account> Accounts { get; }

    DbSet<Employee> Employees { get; }

    DbSet<Service> Services { get; }
    
    DbSet<Customer> Customers { get; }
    
    DbSet<Appointment> Appointments { get; }

    DbSet<TDomain> GetDbSet<TDomain>(IKalendarioDbContext context) where TDomain : class
    {
        var propertyInfo = typeof(IKalendarioDbContext)
            .GetProperties()
            .FirstOrDefault(t => t.PropertyType == typeof(DbSet<TDomain>));

        return (DbSet<TDomain>) propertyInfo?.GetValue(context);
    }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}