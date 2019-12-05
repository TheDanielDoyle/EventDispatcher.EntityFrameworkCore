using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EventDispatcher.EntityFrameworkCore
{
    public class EntityFrameworkEventDispatcherContext : IEventDispatcherContext
    {
        private readonly DbContext context;
        private readonly IEventDispatcher dispatcher;

        public EntityFrameworkEventDispatcherContext(
            DbContext context,
            IEventDispatcher dispatcher)
        {
            this.context = context;
            this.dispatcher = dispatcher;
        }

        public void Dispatch<TEvent>()
        {
            //dispatcher.Dispatch(GetEvents<TEvent>(), GetHandlers(typeof(TEvent)));
        }

        public async Task DispatchAsync<TEvent>(CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (object @event in GetEvents<TEvent>())
            {
                Type eventType = @event.GetType();
                IEnumerable handlers = GetHandlers(@event.GetType()) as IEnumerable;
                MethodInfo dispatchMethod = typeof(IEventDispatcher).GetMethods()
                    .FirstOrDefault(m => 
                        m.Name == nameof(IEventDispatcher.DispatchAsync) 
                        && m.GetParameters().First().Name == nameof(@event));
                if (dispatchMethod != null && handlers != null)
                {
                    try
                    {
                        MethodInfo genericMethod = dispatchMethod.MakeGenericMethod(new[] { eventType });
                        Task task = (Task)genericMethod.Invoke(this.dispatcher, new[] { @event, handlers, cancellationToken });
                        await task.ConfigureAwait(false);
                    }
                    catch (Exception exception)
                    {
                        throw;
                    }
                }
            }
        }

        private IEnumerable GetEvents<TEvent>()
        {
            return this.context.ChangeTracker
                .Entries<IEventObject>()
                .ToList()
                .SelectMany(entityEntry => entityEntry.Entity.EventStore.Events)
                .Where(x => x is TEvent);
        }

        private object GetHandlers(Type eventType)
        {
            Type genericEnumerableType = typeof(IEnumerable<>);
            Type genericHandlerType = typeof(IEventDispatchHandler<>);
            Type concreteHandlerType = genericHandlerType.MakeGenericType(eventType);
            Type concreteEnumerableHandlerType = genericEnumerableType.MakeGenericType(concreteHandlerType);
            return this.context.GetService<IServiceProvider>().GetService(concreteEnumerableHandlerType);
        }
    }
}
