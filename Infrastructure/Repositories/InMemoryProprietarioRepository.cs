using System.Collections.Concurrent;
using WebApiExemplo.Application.Interfaces;
using WebApiExemplo.Domain.Entities;

namespace WebApiExemplo.Infrastructure.Repositories;

/// <summary>Keeps owners in memory for the lifetime of the API process.</summary>
public class InMemoryProprietarioRepository : IProprietarioRepository
{
    private readonly ConcurrentDictionary<Guid, Proprietario> items = new();

    public IReadOnlyCollection<Proprietario> GetAll() => items.Values.ToList();
    public Proprietario? GetById(Guid id) => items.GetValueOrDefault(id);
    public Proprietario? GetByCpf(string cpf) => items.Values.FirstOrDefault(item => item.Cpf == cpf);
    public void Add(Proprietario proprietario) => items[proprietario.Id] = proprietario;
    public void Update(Proprietario proprietario) => items[proprietario.Id] = proprietario;
    public void Remove(Guid id) => items.TryRemove(id, out _);
}
