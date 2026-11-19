using WebApiExemplo.Application.DTOs;

namespace WebApiExemplo.Application.Interfaces;

/// <summary>Use cases for owner management.</summary>
public interface IProprietarioService
{
    IReadOnlyCollection<ProprietarioResponse> GetAll();
    ProprietarioResponse GetById(Guid id);
    ProprietarioResponse Create(ProprietarioRequest request);
    ProprietarioResponse Update(Guid id, ProprietarioRequest request);
    void Delete(Guid id);
}
