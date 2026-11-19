using WebApiExemplo.Domain.Entities;

namespace WebApiExemplo.Application.Interfaces;

/// <summary>Persistence contract for vehicles.</summary>
public interface IVeiculoRepository
{
    IReadOnlyCollection<Veiculo> GetAll();
    Veiculo? GetById(Guid id);
    Veiculo? GetByPlaca(string placa);
    bool HasByProprietarioId(Guid proprietarioId);
    void Add(Veiculo veiculo);
    void Update(Veiculo veiculo);
    void Remove(Guid id);
}
