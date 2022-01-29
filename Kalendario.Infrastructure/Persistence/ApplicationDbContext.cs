using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.EntityFramework.Extensions;
using Duende.IdentityServer.EntityFramework.Interfaces;
using Duende.IdentityServer.EntityFramework.Options;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Common;
using Kalendario.Core.Common;
using Kalendario.Core.Entities;
using Kalendario.Core.Infrastructure;
using Kalendario.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Kalendario.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string> , IPersistedGrantDbContext, IKalendarioDbContext
    {
        private readonly IOptions<OperationalStoreOptions> _operationalStoreOptions;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDomainEventService _domainEventService;
        private readonly IDateTime _dateTime;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IOptions<OperationalStoreOptions> operationalStoreOptions,
            IServiceProvider serviceProvider,
            IDomainEventService domainEventService,
            IDateTime dateTime) : base(options)
        {
            _operationalStoreOptions = operationalStoreOptions;
            _serviceProvider = serviceProvider;
            _domainEventService = domainEventService;
            _dateTime = dateTime;
        }

        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Service> Services => Set<Service>();

        public DbSet<EmployeeService> EmployeeServices => Set<EmployeeService>();
        public DbSet<ServiceCategory> ServiceCategories => Set<ServiceCategory>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<Schedule> Schedules => Set<Schedule>();
        public DbSet<ScheduleFrame> ScheduleFrames => Set<ScheduleFrame>();
        public DbSet<SchedulingPanel> SchedulingPanels => Set<SchedulingPanel>();
        public DbSet<AuditEntity> AuditEntities => Set<AuditEntity>();
        public DbSet<ApplicationRoleGroup> RoleGroups => Set<ApplicationRoleGroup>();
        public DbSet<PersistedGrant> PersistedGrants { get; set; }
        public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }
        public DbSet<Key> Keys { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            builder.ConfigurePersistedGrantContext(_operationalStoreOptions.Value);
            builder.ApplyGlobalFilters<ISoftDeletable>(e => !e.IsDeleted);
        }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var currentUserService = _serviceProvider.GetService<ICurrentUserService>();
            UpdateBaseEntity(currentUserService);
            SoftDeleteEntities();
            var entities = OnBeforeSaveChanges(currentUserService);
            var events = ChangeTracker.Entries<IHasDomainEvent>()
                .Select(x => x.Entity.DomainEvents)
                .SelectMany(x => x)
                .Where(domainEvent => !domainEvent.IsPublished)
                .ToArray();

            var result = await base.SaveChangesAsync(cancellationToken);

            await OnAfterSaveChanges(entities);
            await DispatchEvents(events);

            return result;
        }

        private void UpdateBaseEntity(ICurrentUserService currentUserService)
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

        private void SoftDeleteEntities()
        {
            foreach (var entry in ChangeTracker.Entries<ISoftDeletable>()
                         .Where((entry) => entry.State == EntityState.Deleted))
            {
                entry.State = EntityState.Unchanged;
                entry.Entity.IsDeleted = true;
            }
        }

        private List<AuditEntry> OnBeforeSaveChanges(ICurrentUserService currentUserService)
        {
            // ICurrentUserService can't be at the constructor
            // because it will create an instance that has no user id.
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();

            foreach (var entry in ChangeTracker.Entries<IAuditable>())
            {
                if (entry.State is EntityState.Detached or EntityState.Unchanged)
                    continue;

                var auditEntry = new AuditEntry(entry, currentUserService.UserId);
                auditEntries.Add(auditEntry);

                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary)
                    {
                        auditEntry.TemporaryProperties.Add(property);
                        continue;
                    }

                    var propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.EntityId = property.CurrentValue.ToString();
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }

                            break;
                    }
                }
            }

            // Save audit entities that have all the modifications
            foreach (var auditEntry in auditEntries.Where(_ => !_.HasTemporaryProperties))
            {
                AuditEntities.Add(auditEntry.ToAudit());
            }

            // keep a list of entries where the value of some properties are unknown at this step
            return auditEntries.Where(_ => _.HasTemporaryProperties).ToList();
        }

        private Task OnAfterSaveChanges(List<AuditEntry> auditEntries)
        {
            if (auditEntries == null || auditEntries.Count == 0)
                return Task.CompletedTask;

            foreach (var auditEntry in auditEntries)
            {
                // Get the final value of the temporary properties
                foreach (var prop in auditEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        auditEntry.EntityId = prop.CurrentValue.ToString();
                    }
                    else
                    {
                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                }

                // Save the Audit entry
                AuditEntities.Add(auditEntry.ToAudit());
            }

            return SaveChangesAsync();
        }

        private async Task DispatchEvents(DomainEvent[] events)
        {
            foreach (var @event in events)
            {
                @event.IsPublished = true;
                await _domainEventService.Publish(@event);
            }
        }

        Task<int> IPersistedGrantDbContext.SaveChangesAsync() => base.SaveChangesAsync();
    }
}