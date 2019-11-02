using EventDispatcher;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventDispatcher(this IServiceCollection services)
        {
            services.AddScoped<IEventDispatcher, EventDispatcher.EventDispatcher>();
            return services;
        }

        public static IServiceCollection AddDomainEventDispatchInvoker<TInvoker>(this IServiceCollection services)
            where TInvoker : class, IEventDispatchInvoker<IDomainEvent>
        {
            services.AddEventDispatchInvoker<IDomainEvent, TInvoker>();
            return services;
        }

        public static IServiceCollection AddIntegrationEventDispatchInvoker<TInvoker>(this IServiceCollection services)
            where TInvoker : class, IEventDispatchInvoker<IIntegrationEvent>
        {
            services.AddEventDispatchInvoker<IIntegrationEvent, TInvoker>();
            return services;
        }

        internal static IServiceCollection AddEventDispatchInvoker<TEvent, TInvoker>(this IServiceCollection services)
            where TInvoker : class, IEventDispatchInvoker<TEvent>
            where TEvent : IEvent
        {
            services.AddScoped<IEventDispatchInvoker<TEvent>, TInvoker>();
            return services;
        }
    }
}
