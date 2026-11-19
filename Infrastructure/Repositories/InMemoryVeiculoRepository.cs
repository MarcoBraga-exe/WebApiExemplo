using System.Collections.Concurrent;
using WebApiExemplo.Application.Interfaces;
using WebApiExemplo.Domain.Entities;

namespace WebApiExemplo.Infrastructure.Repositories;

/// <summary>Keeps vehicles in memory for the lifetime of the API process.</summary>
public class InMemoryVeiculoRepository : IVeiculoRepository
{
    private readonly ConcurrentDictionary<Guid, Veiculo> items = new();

    public IReadOnlyCollection<Veiculo> GetAll() => items.Values.ToList();
    public Veiculo? GetById(Guid id) => items.GetValueOrDefault(id);
    public Veiculo? GetByPlaca(string placa) => items.Values.FirstOrDefault(item => item.Placa.Equals(placa, StringComparison.OrdinalIgnoreCase));
    public bool HasByProprietarioId(Guid proprietarioId) => items.Values.Any(item => item.ProprietarioId == proprietarioId);
    public void Add(Veiculo veiculo) => items[veiculo.Id] = veiculo;
    public void Update(Veiculo veiculo) => items[veiculo.Id] = veiculo;
    public void Remove(Guid id) => items.TryRemove(id, out _);
}
