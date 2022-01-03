using System.Threading;
using System.Threading.Tasks;
using Kalendario.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Common.Interfaces;

public interface IKalendarioDbContext
{
    DbSet<Account> Accounts { get; }

    DbSet<Employee> Employees { get; }

    DbSet<Service> Services { get; }
    
    DbSet<Customer> Customers { get; }
    
    DbSet<Appointment> Appointments { get; }

    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}