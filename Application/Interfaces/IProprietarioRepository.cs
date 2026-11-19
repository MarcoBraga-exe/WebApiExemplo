using WebApiExemplo.Domain.Entities;

namespace WebApiExemplo.Application.Interfaces;

/// <summary>Persistence contract for owners.</summary>
public interface IProprietarioRepository
{
    IReadOnlyCollection<Proprietario> GetAll();
    Proprietario? GetById(Guid id);
    Proprietario? GetByCpf(string cpf);
    void Add(Proprietario proprietario);
    void Update(Proprietario proprietario);
    void Remove(Guid id);
}
