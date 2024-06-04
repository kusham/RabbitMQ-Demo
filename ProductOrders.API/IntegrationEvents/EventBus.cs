
using MassTransit;

namespace ProductOrders.API.IntegrationEvents
{
    public sealed class EventBus : IEventBus
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<EventBus> _logger;

        public EventBus(IPublishEndpoint publishEndpoint, ILogger<EventBus> logger)
        {
            this._publishEndpoint = publishEndpoint;
            this._logger = logger;
        }

        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) 
            where T : class
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            try
            {
                _logger.LogInformation("Publishing message of type {MessageType}.", typeof(T).Name);
                await _publishEndpoint.Publish(message, cancellationToken);
                _logger.LogInformation("############## Message of type : {MessageType}-----------> published successfully.", typeof(T).Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while publishing message of type {MessageType}.", typeof(T).Name);
                throw;
            }
        }
    }
}
