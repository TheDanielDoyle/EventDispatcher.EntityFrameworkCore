using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventDispatcher.EntityFrameworkCore.Samples.InMemory
{
    public class ThereIsNoSpoonEventHandler : IEventDispatchHandler<ThereIsNoSpoonEvent>
    {
        public void Handle(ThereIsNoSpoonEvent @event)
        {
            Console.WriteLine("There is no spoon!");
        }

        public Task HandleAsync(ThereIsNoSpoonEvent @event, CancellationToken cancellation = new CancellationToken())
        {
            Console.WriteLine("There is no spoon!");
            return Task.CompletedTask;
        }
    }
}