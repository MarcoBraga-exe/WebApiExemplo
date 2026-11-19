using WebApiExemplo.Application.DTOs;

namespace WebApiExemplo.Application.Interfaces;

/// <summary>Use cases for vehicle management and search.</summary>
public interface IVeiculoService
{
    IReadOnlyCollection<VeiculoResponse> Search(string? placa, string? modelo, string? nomeProprietario);
    VeiculoResponse GetById(Guid id);
    Task<VeiculoResponse> CreateAsync(VeiculoRequest request, CancellationToken cancellationToken);
    VeiculoResponse Update(Guid id, VeiculoRequest request);
    void Delete(Guid id);
}
