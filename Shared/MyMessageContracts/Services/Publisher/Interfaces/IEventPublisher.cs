using MyMessageContracts.SyncEntities.Events.Base.Interfaces;

namespace MyMessageContracts.Services.Publisher.Interfaces;

public interface IEventPublisher<TEvent> where TEvent : class, IBaseEvent
{
  Task PublishAsync(TEvent eventMessage);
}
