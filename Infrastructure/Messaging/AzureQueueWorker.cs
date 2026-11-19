using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Options;

namespace WebApiExemplo.Infrastructure.Messaging;

/// <summary>Worker hospedado pela API que consome e confirma mensagens da fila Azure.</summary>
/// // Executa em segundo plano, recebendo e processando
// as mensagens disponibilizadas na fila.
public class AzureQueueWorker(IOptions<AzureQueueOptions> options, ILogger<AzureQueueWorker> logger) : BackgroundService
{
    private readonly AzureQueueOptions queueOptions = options.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!queueOptions.IsConfigured)
        {
            logger.LogWarning("Azure Queue não configurada. O worker não consumirá mensagens.");
            return;
        }

        var queue = new QueueClient(queueOptions.ConnectionString, queueOptions.QueueName, new QueueClientOptions { MessageEncoding = QueueMessageEncoding.Base64 });
        await queue.CreateIfNotExistsAsync(cancellationToken: stoppingToken);
        var interval = TimeSpan.FromSeconds(Math.Max(1, queueOptions.PollingIntervalSeconds));

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var messages = await queue.ReceiveMessagesAsync(maxMessages: 10, visibilityTimeout: TimeSpan.FromMinutes(1), cancellationToken: stoppingToken);
                // A mensagem só é removida após ter sido recebida e registrada com sucesso.
                foreach (var message in messages.Value)
                {
                    logger.LogInformation("Mensagem recebida da fila {QueueName}: {MessageText}", queueOptions.QueueName, message.MessageText);
                    await queue.DeleteMessageAsync(message.MessageId, message.PopReceipt, stoppingToken);
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Erro ao consumir a fila Azure {QueueName}.", queueOptions.QueueName);
            }

            await Task.Delay(interval, stoppingToken);
        }
    }
}
