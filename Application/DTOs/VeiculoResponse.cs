namespace WebApiExemplo.Application.DTOs;

/// <summary>Output model returned for a vehicle and its owner name.</summary>
public record VeiculoResponse(Guid Id, string Placa, string Modelo, int Ano, Guid ProprietarioId, string NomeProprietario);
