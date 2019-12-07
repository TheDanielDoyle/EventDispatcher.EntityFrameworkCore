using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EventDispatcher.EntityFrameworkCore
{
    public class EntityFrameworkEventDispatcherContext : DefaultEventDispatcherContext
    {
        private readonly DbContext context;

        public EntityFrameworkEventDispatcherContext(
            DbContext context,
            IEventDispatcher dispatcher) : base(dispatcher)
        {
            this.context = context;
        }

        protected override IEventDispatchHandler[] CreateHandlers(Type handlerCollectionType)
        {
            return this.context.GetService<IServiceProvider>().GetService(handlerCollectionType) as IEventDispatchHandler[];
        }

        protected override IEnumerable<IEvent> GetEvents(Type eventType)
        {
            return this.context.ChangeTracker
                .Entries<IEventObject>()
                .ToList()
                .SelectMany(entityEntry => entityEntry.Entity.EventStore.Events.AssignableTo(eventType));
        }
    }
}
