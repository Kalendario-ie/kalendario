using System.Threading;
using System.Threading.Tasks;
using Kalendario.Core.Entities;
using Kalendario.Core.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Common.Interfaces;

public interface IKalendarioDbContext
{
    DbSet<Account> Accounts { get; }

    DbSet<Employee> Employees { get; }

    DbSet<Service> Services { get; }

    DbSet<EmployeeService> EmployeeServices { get; }

    DbSet<ServiceCategory> ServiceCategories { get; }
    
    DbSet<Customer> Customers { get; }
    
    DbSet<Appointment> Appointments { get; }

    DbSet<Schedule> Schedules { get; }

    DbSet<ScheduleFrame> ScheduleFrames { get; }

    DbSet<SchedulingPanel> SchedulingPanels { get; }

    DbSet<AuditEntity> AuditEntities { get; }

    DbSet<ApplicationUser> Users { get; }
    
    DbSet<ApplicationRole> Roles { get; }
    
    DbSet<ApplicationRoleGroup> RoleGroups { get; }

    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}