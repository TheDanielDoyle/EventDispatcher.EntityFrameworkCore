using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventDispatcher.EntityFrameworkCore.Samples.InMemory
{
    internal class MerryChristmasDomainEventHandler : IEventDispatchHandler<MerryChristmasDomainEvent>
    {
        public void Handle(MerryChristmasDomainEvent @event)
        {
            Console.WriteLine("Merry Christmas! from the Domain.");
        }

        public Task HandleAsync(MerryChristmasDomainEvent @event, CancellationToken cancellation = new CancellationToken())
        {
            Console.WriteLine("Merry Christmas! from the Domain Async.");
            return Task.CompletedTask;
        }
    }
}
