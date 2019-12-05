using System;

namespace EventDispatcher.EntityFrameworkCore.Samples.InMemory
{
    public class HelloWorldDomainEvent : IDomainEvent
    {
        public HelloWorldDomainEvent()
        {
            Created = DateTimeOffset.UtcNow;
        }

        public DateTimeOffset Created { get; }
    }
}