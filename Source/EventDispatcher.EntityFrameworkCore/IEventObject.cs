namespace EventDispatcher.EntityFrameworkCore
{
    public interface IEventObject
    {
        IEventStore EventStore { get; }
    }
}
