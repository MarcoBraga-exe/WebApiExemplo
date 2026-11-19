namespace WebApiExemplo.Application.Interfaces;

/// <summary>Publishes application events without coupling services to Azure SDK types.</summary>
public interface IQueuePublisher
{
    Task PublishAsync<T>(T message, CancellationToken cancellationToken);
}
