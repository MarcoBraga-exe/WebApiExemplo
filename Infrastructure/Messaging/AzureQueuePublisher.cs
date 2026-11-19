using System.Text.Json;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Options;
using WebApiExemplo.Application.Interfaces;

namespace WebApiExemplo.Infrastructure.Messaging;

/// <summary>Publica eventos serializados em JSON para uma fila do Azure Queue Storage.</summary>
/// // Responsável por serializar a mensagem e enviá-la
// para a fila configurada no Azure Service Bus.
public class AzureQueuePublisher(IOptions<AzureQueueOptions> options, ILogger<AzureQueuePublisher> logger) : IQueuePublisher
{
    private readonly AzureQueueOptions queueOptions = options.Value;

    public async Task PublishAsync<T>(T message, CancellationToken cancellationToken)
    {
        if (!queueOptions.IsConfigured)
        {
            logger.LogWarning("Azure Queue não configurada. A mensagem não foi enviada.");
            return;
        }

        // A fila é criada automaticamente quando ainda não existe na conta de armazenamento.
        var queue = CreateClient();
        await queue.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
        var payload = JsonSerializer.Serialize(message);
        await queue.SendMessageAsync(payload, cancellationToken: cancellationToken);
        logger.LogInformation("Mensagem enviada para a fila {QueueName}.", queueOptions.QueueName);
    }

    private QueueClient CreateClient() => new(queueOptions.ConnectionString, queueOptions.QueueName, new QueueClientOptions { MessageEncoding = QueueMessageEncoding.Base64 });
}
