using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventDispatcher.EntityFrameworkCore.Samples.InMemory
{
    internal class HelloWorldIntegrationEventHandler : IEventDispatchHandler<HelloWorldIntegrationEvent>
    {
        public void Handle(HelloWorldIntegrationEvent @event)
        {
            Console.WriteLine("Hello World! from the Integration.");
        }

        public Task HandleAsync(HelloWorldIntegrationEvent @event, CancellationToken cancellation = new CancellationToken())
        {
            Console.WriteLine("Hello World! from the Integration Async.");
            return Task.CompletedTask;
        
        }
    }
}