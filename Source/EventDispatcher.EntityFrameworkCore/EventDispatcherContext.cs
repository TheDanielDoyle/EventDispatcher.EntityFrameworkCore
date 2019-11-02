using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EventDispatcher.EntityFrameworkCore
{
    internal class EventDispatcherContext<TEvent>
        where TEvent : class, IEvent
    {
        internal EventDispatcherContext(IEventDispatcher dispatcher, IEventDispatchInvoker<TEvent> invoker, IEnumerable<TEvent> events)
        {
            Dispatcher = dispatcher;
            Events = events;
            Invoker = invoker;
        }

        public IEventDispatcher Dispatcher { get; }

        public IEnumerable<TEvent> Events { get; }

        public IEventDispatchInvoker<TEvent> Invoker { get; }

        public static EventDispatcherContext<TEvent> Create(DbContext context)
        {
            IEventDispatcher dispatcher = GetDispatcher(context);
            IEnumerable<TEvent> events = GetEvents(context);
            IEventDispatchInvoker<TEvent> invoker = GetInvoker(context);
            return new EventDispatcherContext<TEvent>(dispatcher, invoker, events);
        }

        private static IEventDispatcher GetDispatcher(DbContext context)
        {
            return context.GetService<IEventDispatcher>();
        }

        private static IEnumerable<TEvent> GetEvents(DbContext context)
        {
            return context.ChangeTracker.Entries<TEvent>().Select(e => e.Entity);
        }

        private static IEventDispatchInvoker<TEvent> GetInvoker(DbContext context)
        {
            return context.GetService<IEventDispatchInvoker<TEvent>>();
        }
    }
}
