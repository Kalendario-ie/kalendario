using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Common;
using Kalendario.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Persistence
{
    public class KalendarioDbContext : DbContext, IKalendarioDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        public KalendarioDbContext(DbContextOptions<KalendarioDbContext> options)
            : base(options)
        {
        }

        public KalendarioDbContext(
            DbContextOptions<KalendarioDbContext> options,
            ICurrentUserService currentUserService,
            IDateTime dateTime) : base(options)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.UserCreated = _currentUserService.UserId;
                        entry.Entity.DateCreated = _dateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UserModified = _currentUserService.UserId;
                        entry.Entity.DateModified = _dateTime.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasQueryFilter(s => s.AccountId == _currentUserService.AccountId);
            modelBuilder.Entity<Service>().HasQueryFilter(s => s.AccountId == _currentUserService.AccountId);
            modelBuilder.Entity<Customer>().HasQueryFilter(s => s.AccountId == _currentUserService.AccountId);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}