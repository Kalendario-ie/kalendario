using Duende.IdentityServer.EntityFramework.Options;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Common;
using Kalendario.Core.Common;
using Kalendario.Core.Entities;
using Kalendario.Core.Infrastructure;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Kalendario.Infrastructure.Persistence
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IKalendarioDbContext
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDomainEventService _domainEventService;
        private readonly IDateTime _dateTime;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IOptions<OperationalStoreOptions> operationalStoreOptions,
            IServiceProvider serviceProvider,
            IDomainEventService domainEventService,
            IDateTime dateTime) : base(options, operationalStoreOptions)
        {
            _serviceProvider = serviceProvider;
            _domainEventService = domainEventService;
            _dateTime = dateTime;
        } 
        
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Service> Services => Set<Service>();
        public DbSet<ServiceCategory> ServiceCategories => Set<ServiceCategory>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<Schedule> Schedules => Set<Schedule>();
        public DbSet<ScheduleFrame> ScheduleFrames => Set<ScheduleFrame>();

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            // ICurrentUserService can't be at the constructor
            // because it will create an instance that has no user id.
            var currentUserService = _serviceProvider.GetService<ICurrentUserService>();
            if (currentUserService != null)
            {
                foreach (var entry in ChangeTracker.Entries<BaseEntity>())
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.Entity.UserCreated = currentUserService.UserId;
                            entry.Entity.DateCreated = _dateTime.Now;
                            break;

                        case EntityState.Modified:
                            entry.Entity.UserModified = currentUserService.UserId;
                            entry.Entity.DateModified = _dateTime.Now;
                            break;
                    }
                }                
            }

            var events = ChangeTracker.Entries<IHasDomainEvent>()
                .Select(x => x.Entity.DomainEvents)
                .SelectMany(x => x)
                .Where(domainEvent => !domainEvent.IsPublished)
                .ToArray();

            var result = await base.SaveChangesAsync(cancellationToken);

            await DispatchEvents(events);

            return result;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

        private async Task DispatchEvents(DomainEvent[] events)
        {
            foreach (var @event in events)
            {
                @event.IsPublished = true;
                await _domainEventService.Publish(@event);
            }
        }
    }
}