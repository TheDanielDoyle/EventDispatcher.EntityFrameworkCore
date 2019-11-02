using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventDispatcher;

namespace Microsoft.EntityFrameworkCore.Diagnostics
{
    public class EventDispatcherDbCommandInterceptor : DbCommandInterceptor
    {
        private readonly IEventDispatcher dispatcher;
        private readonly IEventDispatchInvoker<IDomainEvent> domainEventInvoker;
        private readonly IEventDispatchInvoker<IIntegrationEvent> integrationEventInvoker;

        public EventDispatcherDbCommandInterceptor(IEventDispatcher dispatcher, IEventDispatchInvoker<IDomainEvent> domainEventInvoker, IEventDispatchInvoker<IIntegrationEvent> integrationEventInvoker)
        {
            this.dispatcher = dispatcher;
            this.domainEventInvoker = domainEventInvoker;
            this.integrationEventInvoker = integrationEventInvoker;
        }

        public override int NonQueryExecuted(DbCommand command, CommandExecutedEventData eventData, int result)
        {
            int returnCode = base.NonQueryExecuted(command, eventData, result);
            this.dispatcher.Dispatch(GetEvents<IIntegrationEvent>(eventData.Context), integrationEventInvoker);
            return returnCode;
        }

        public override async Task<int> NonQueryExecutedAsync(DbCommand command, CommandExecutedEventData eventData, int result, CancellationToken cancellationToken = new CancellationToken())
        {
            int returnCode = await base.NonQueryExecutedAsync(command, eventData, result, cancellationToken);
            await this.dispatcher.DispatchAsync(GetEvents<IIntegrationEvent>(eventData.Context), integrationEventInvoker, cancellationToken);
            return returnCode;
        }

        public override InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<int> result)
        {
            this.dispatcher.Dispatch(GetEvents<IDomainEvent>(eventData.Context), domainEventInvoker);
            InterceptionResult<int> interceptionResult = base.NonQueryExecuting(command, eventData, result);
            return interceptionResult;
        }

        public override async Task<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = new CancellationToken())
        {
            IList<IDomainEvent> events = GetEvents<IDomainEvent>(eventData.Context).ToList();
            await this.dispatcher.DispatchAsync(events, domainEventInvoker, cancellationToken);
            InterceptionResult<int> interceptionResult = await base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
            return interceptionResult;
        }

        private static IEnumerable<TEvent> GetEvents<TEvent>(DbContext context) where TEvent : class, IEvent
        {
            return context.ChangeTracker.Entries<TEvent>().Select(e => e.Entity);
        }
    }
}
