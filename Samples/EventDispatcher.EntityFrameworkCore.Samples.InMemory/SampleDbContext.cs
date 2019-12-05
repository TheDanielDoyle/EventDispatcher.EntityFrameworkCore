using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EventDispatcher.EntityFrameworkCore.Samples.InMemory
{
    internal class SampleDbContext : DbContext
    {
        private IEventDispatcherContext eventDispatcherContext;

        public SampleDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public override int SaveChanges()
        {
            DispatchDomainEvents();
            int returnCode = base.SaveChanges();
            DispatchIntegrationEvents();
            return returnCode;
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            DispatchDomainEvents();
            int returnCode = base.SaveChanges(acceptAllChangesOnSuccess);
            DispatchIntegrationEvents();
            return returnCode;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            await DispatchDomainEventsAsync(cancellationToken).ConfigureAwait(false);
            int returnCode = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            await DispatchIntegrationEventsAsync(cancellationToken).ConfigureAwait(false);
            return returnCode;
        }

        private void DispatchDomainEvents()
        {
            GetEventDispatcherContext().Dispatch<IDomainEvent>();
        }

        private Task DispatchDomainEventsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetEventDispatcherContext().DispatchAsync<IDomainEvent>(cancellationToken);
        }

        private void DispatchIntegrationEvents()
        {
            GetEventDispatcherContext().Dispatch<IIntegrationEvent>();
        }

        private Task DispatchIntegrationEventsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetEventDispatcherContext().DispatchAsync<IIntegrationEvent>(cancellationToken);
        }

        private IEventDispatcherContext GetEventDispatcherContext()
        {
            return eventDispatcherContext ??= new EntityFrameworkEventDispatcherContext(this, this.GetService<IEventDispatcher>());
        }
    }
}
