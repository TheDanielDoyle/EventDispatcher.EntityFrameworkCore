using System;

namespace EventDispatcher.EntityFrameworkCore.Samples.InMemory
{
    public class HelloWorldIntegrationEvent : IIntegrationEvent
    {
        public HelloWorldIntegrationEvent()
        {
            Created = DateTimeOffset.UtcNow;
        }

        public DateTimeOffset Created { get; }
    }
}