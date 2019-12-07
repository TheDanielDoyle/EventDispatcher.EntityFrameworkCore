using System;

namespace EventDispatcher.EntityFrameworkCore.Samples.InMemory
{
    public class ThereIsNoSpoonEvent : IDomainEvent, IIntegrationEvent
    {
        public ThereIsNoSpoonEvent()
        {
            Created = DateTimeOffset.UtcNow;
        }

        public DateTimeOffset Created { get; }
    }
}
