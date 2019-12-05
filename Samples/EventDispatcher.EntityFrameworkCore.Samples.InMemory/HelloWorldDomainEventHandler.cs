using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventDispatcher.EntityFrameworkCore.Samples.InMemory
{
    internal class HelloWorldDomainEventHandler : IEventDispatchHandler<HelloWorldDomainEvent>
    {
        public void Handle(HelloWorldDomainEvent @event)
        {
            Console.WriteLine("Hello World! from the Domain.");
        }

        public Task HandleAsync(HelloWorldDomainEvent @event, CancellationToken cancellation = new CancellationToken())
        {
            Console.WriteLine("Hello World! from the Domain Async.");
            return Task.CompletedTask;
        }
    }

    internal class HelloWorldDomainEventHandler2 : IEventDispatchHandler<HelloWorldDomainEvent>
    {
        public void Handle(HelloWorldDomainEvent @event)
        {
            Console.WriteLine("Hello World 2! from the Domain.");
        }

        public Task HandleAsync(HelloWorldDomainEvent @event, CancellationToken cancellation = new CancellationToken())
        {
            Console.WriteLine("Hello World 2! from the Domain Async.");
            return Task.CompletedTask;
        }
    }
}
