# Event Dispatcher Entity Framework Core

Entity Framework Core Domain and Integration event dispatch.

## Quick start

1. Construct a __EntityFrameworkEventDispatcherContext__ class by passing in the DbContext itself, and an implementation of __IEventDispatcher__.

2. Override the following __SaveChanges()__ and __SaveChangesAsync()__ methods with the example code below (_or roll your own_).

3. Include the __ClearEvents()__ and __GetEventDispatcherContext()__ methods if required. 

````csharp
public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        IEventDispatcherContext dispatcherContext = GetEventDispatcherContext();
        DispatchEvents<IDomainEvent>(dispatcherContext);
        int returnCode = base.SaveChanges(acceptAllChangesOnSuccess);
        DispatchEvents<IIntegrationEvent>(dispatcherContext);
        ClearEvents();
        return returnCode;
    }

public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
{
    IEventDispatcherContext dispatcherContext = GetEventDispatcherContext();
    await dispatcherContext.DispatchAsync<IDomainEvent>(dispatcherContext, cancellationToken).ConfigureAwait(false);
    int returnCode = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken).ConfigureAwait(false);
    await dispatcherContext.DispatchAsync<IIntegrationEvent>(dispatcherContext, cancellationToken).ConfigureAwait(false);
    ClearEvents();
    return returnCode;
}

private void ClearEvents()
{
    foreach (EntityEntry<IEventObject> eventObject in this.ChangeTracker.Entries<IEventObject>())
    {
        eventObject.Entity.EventStore.ClearEvents();
    }
}
    
private IEventDispatcherContext GetEventDispatcherContext()
{
    return new EntityFrameworkEventDispatcherContext(this, this.GetService<IEventDispatcher>());
}
````

4. Add the __IEventObject__ interface to any of your __DbContext__ entities.
    * This will require you to implement the __IEventStore__ interface.
    * The following example will demonstrate.

````csharp
public class User : IEntity
{
    public User()
    {
        EventStore = new EventStore();
    }

    public IEventStore EventStore { get; }
}
````

5. To add a domain event, create a class which derives from __IDomainEvent__ and you can add that to your eventstore on the entity.
    * Integration events should follow IIntegrationEvent.
    * You can create your own event types and use them as you wish. Dispatch them at your chosen point in the __SaveChanges()__ or __SaveChangesAsync()__ methods.

* When save changes is called any event dispatch handlers registered will handle that event.

* Your handlers should derive from the following interface to picked up by the __IEventDispatcher__:
    * __IEventDispatchHandler\<MyDomainEvent\>__

* See the link to a sample project below, which should demonstrate clearly.

## Samples

See <https://github.com/TheDanielDoyle/EventDispatcher.EntityFrameworkCore/blob/develop/Samples/EventDispatcher.EntityFrameworkCore.Samples.InMemory/SampleDbContext.cs> for an example DbContext implementation, which uses the __EntityFrameworkEventDispatcherContext__. 
