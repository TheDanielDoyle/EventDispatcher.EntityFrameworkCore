namespace EventDispatcher.EntityFrameworkCore.Samples.InMemory
{
    public class User : IEntity
    {
        public User()
        {
            EventStore = new EventStore();
        }

        public IEventStore EventStore { get; }

        public int Id { get; set; }

        public string Name { get; set; }

        public void SayHello() 
            => EventStore.AddEvents(new IEvent[]
            {
                new HelloWorldDomainEvent(),
                new HelloWorldIntegrationEvent(),
            });

        public void SayMerryChristmas()
            => EventStore.AddEvents(new IEvent[]
            {
                new MerryChristmasDomainEvent(), 
                new MerryChristmasIntegrationEvent()
            });

        public void SayThereIsNoSpoon()
            => EventStore.AddEvents(new IEvent[]
            {
                new ThereIsNoSpoonEvent()
            });
    }
}