using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using EventDispatcher;
using EventDispatcher.EntityFrameworkCore;

namespace Microsoft.EntityFrameworkCore.Diagnostics
{
    public class EventDispatcherDbCommandInterceptor : DbCommandInterceptor
    {

        public EventDispatcherDbCommandInterceptor()
        {
        }

        public override int NonQueryExecuted(DbCommand command, CommandExecutedEventData eventData, int result)
        {
            int returnCode = base.NonQueryExecuted(command, eventData, result);
            EventDispatcherContext<IIntegrationEvent> context = EventDispatcherContext<IIntegrationEvent>.Create(eventData.Context);
            context.Dispatcher.Dispatch(context.Events, context.Invoker);
            return returnCode;
        }

        public override async Task<int> NonQueryExecutedAsync(DbCommand command, CommandExecutedEventData eventData, int result, CancellationToken cancellationToken = new CancellationToken())
        {
            int returnCode = await base.NonQueryExecutedAsync(command, eventData, result, cancellationToken);
            EventDispatcherContext<IIntegrationEvent> context = EventDispatcherContext<IIntegrationEvent>.Create(eventData.Context);
            await context.Dispatcher.DispatchAsync(context.Events, context.Invoker, cancellationToken);
            return returnCode;
        }

        public override InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<int> result)
        {
            EventDispatcherContext<IDomainEvent> context = EventDispatcherContext<IDomainEvent>.Create(eventData.Context);
            context.Dispatcher.Dispatch(context.Events, context.Invoker);
            InterceptionResult<int> interceptionResult = base.NonQueryExecuting(command, eventData, result);
            return interceptionResult;
        }

        public override async Task<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = new CancellationToken())
        {
            EventDispatcherContext<IDomainEvent> context = EventDispatcherContext<IDomainEvent>.Create(eventData.Context);
            await context.Dispatcher.DispatchAsync(context.Events, context.Invoker, cancellationToken);
            InterceptionResult<int> interceptionResult = await base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
            return interceptionResult;
        }
    }
}
