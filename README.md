<!-- Guia de execução e configuração da aplicação. -->
# WebApiExemplo

API .NET 8 para cadastro de proprietários e veículos, usando repositórios em memória.

## Executar

```powershell
dotnet run --project "D:\Csharp\WebApiExemplo"
```

Abra `http://localhost:<porta>/swagger` para testar a API.

## Endpoints

- `GET`, `POST` em `/api/proprietarios`
- `GET`, `PUT`, `DELETE` em `/api/proprietarios/{id}`
- `GET`, `POST` em `/api/veiculos`
- `GET`, `PUT`, `DELETE` em `/api/veiculos/{id}`
- Filtros: `/api/veiculos?placa=ABC-1234&modelo=Corolla&nomeProprietario=Maria`

## Azure Queue Storage

Configure `AzureQueue:ConnectionString` com a connection string da conta de armazenamento. Ao criar um veículo, a API publica uma mensagem JSON na fila configurada em `AzureQueue:QueueName`; o `AzureQueueWorker` recebe, registra e remove a mensagem.

Para manter segredos fora do arquivo de configuração:

```powershell
dotnet user-secrets set "AzureQueue:ConnectionString" "<sua-connection-string>" --project "D:\Csharp\WebApiExemplo"
```

Sem essa configuração, a API continua funcionando com dados em memória e apenas registra que a fila Azure não está configurada.
