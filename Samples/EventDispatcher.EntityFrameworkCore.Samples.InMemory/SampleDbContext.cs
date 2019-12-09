using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EventDispatcher.EntityFrameworkCore.Samples.InMemory
{
    internal class SampleDbContext : DbContext
    {
        public SampleDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            IEventDispatcherContext dispatcherContext = GetEventDispatcherContext();
            DispatchEvents<IDomainEvent>(dispatcherContext);
            int returnCode = base.SaveChanges(acceptAllChangesOnSuccess);
            DispatchEvents<IIntegrationEvent>(dispatcherContext);
            ClearEvents();
            return returnCode;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            IEventDispatcherContext dispatcherContext = GetEventDispatcherContext();
            await DispatchEventsAsync<IDomainEvent>(dispatcherContext, cancellationToken).ConfigureAwait(false);
            int returnCode = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken).ConfigureAwait(false);
            await DispatchEventsAsync<IIntegrationEvent>(dispatcherContext, cancellationToken).ConfigureAwait(false);
            ClearEvents();
            return returnCode;
        }

        private void ClearEvents()
        {
            foreach (EntityEntry<IEventObject> eventObject in this.ChangeTracker.Entries<IEventObject>())
            {
                eventObject.Entity.EventStore.ClearEvents();
            }
        }

        private void DispatchEvents<TEvent>(IEventDispatcherContext dispatcherContext) where TEvent : IEvent
        {
            dispatcherContext.Dispatch<TEvent>();
        }

        private Task DispatchEventsAsync<TEvent>(IEventDispatcherContext dispatcherContext, CancellationToken cancellationToken = default(CancellationToken)) where TEvent : IEvent
        {
            return dispatcherContext.DispatchAsync<TEvent>(cancellationToken);
        }

        private IEventDispatcherContext GetEventDispatcherContext()
        {
            return new EntityFrameworkEventDispatcherContext(this, this.GetService<IEventDispatcher>());
        }
    }
}
