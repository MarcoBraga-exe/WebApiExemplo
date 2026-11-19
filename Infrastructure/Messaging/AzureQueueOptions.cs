namespace WebApiExemplo.Infrastructure.Messaging;

/// <summary>Mapeia a seção AzureQueue da configuração da aplicação.</summary>
// Centraliza as configurações da fila, evitando deixar
// credenciais e nomes de recursos espalhados pelo código.
public class AzureQueueOptions
{
    public const string SectionName = "AzureQueue";
    public string ConnectionString { get; init; } = string.Empty;
    public string QueueName { get; init; } = "veiculo-criado";
    public int PollingIntervalSeconds { get; init; } = 10;
    // Evita tentativas de acesso ao Azure quando o ambiente local não possui credenciais.
    public bool IsConfigured => !string.IsNullOrWhiteSpace(ConnectionString) && !string.IsNullOrWhiteSpace(QueueName);
}
