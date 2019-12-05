using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventDispatcher.EntityFrameworkCore.Samples.InMemory
{
    internal class MerryChristmasIntegrationEventHandler : IEventDispatchHandler<MerryChristmasIntegrationEvent>
    {
        public void Handle(MerryChristmasIntegrationEvent @event)
        {
            Console.WriteLine("Merry Christmas! from the Integration.");
        }

        public Task HandleAsync(MerryChristmasIntegrationEvent @event, CancellationToken cancellation = new CancellationToken())
        {
            Console.WriteLine("Merry Christmas! from the Integration Async.");
            return Task.CompletedTask;
            ;
        }
    }
}