using System;

namespace EventDispatcher.EntityFrameworkCore.Samples.InMemory
{
    internal class MerryChristmasDomainEvent : IDomainEvent
    {
        public MerryChristmasDomainEvent()
        {
            Created = DateTimeOffset.UtcNow;
        }

        public DateTimeOffset Created { get; }
    }
}
