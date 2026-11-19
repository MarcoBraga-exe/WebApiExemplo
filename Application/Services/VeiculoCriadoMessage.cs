namespace WebApiExemplo.Application.Services;

/// <summary>Event sent to Azure Queue Storage after a vehicle is created.</summary>
public record VeiculoCriadoMessage(Guid VeiculoId, string Placa, string NomeProprietario, DateTimeOffset CriadoEm);
