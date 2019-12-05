using System;

namespace EventDispatcher.EntityFrameworkCore.Samples.InMemory
{
    internal class MerryChristmasIntegrationEvent : IIntegrationEvent
    {
        public MerryChristmasIntegrationEvent()
        {
            Created = DateTimeOffset.UtcNow;
        }

        public DateTimeOffset Created { get; }
    }
}