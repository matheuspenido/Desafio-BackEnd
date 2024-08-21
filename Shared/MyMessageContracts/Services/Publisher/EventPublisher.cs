using MassTransit;
using MyMessageContracts.SyncEntities.Events.Base.Interfaces;

namespace MyMessageContracts.Services.Publisher.Interfaces;

public class EventPublisher<TEvent> : IEventPublisher<TEvent> 
  where TEvent : class, IBaseEvent
{
  private readonly IPublishEndpoint _publishEndpoint;

  public EventPublisher(IPublishEndpoint publishEndpoint)
  {
    _publishEndpoint = publishEndpoint;
  }

  public async Task PublishAsync(TEvent eventMessage)
  {
    ArgumentNullException.ThrowIfNull(eventMessage);

    await _publishEndpoint.Publish(eventMessage);
  }
}