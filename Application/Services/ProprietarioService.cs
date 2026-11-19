using WebApiExemplo.Application.DTOs;
using WebApiExemplo.Application.Exceptions;
using WebApiExemplo.Application.Interfaces;
using WebApiExemplo.Domain.Entities;

namespace WebApiExemplo.Application.Services;

/// <summary>Applies owner validation rules and protects vehicle ownership links.</summary>
public class ProprietarioService(IProprietarioRepository proprietarioRepository, IVeiculoRepository veiculoRepository) : IProprietarioService
{
    public IReadOnlyCollection<ProprietarioResponse> GetAll() => proprietarioRepository.GetAll().Select(ToResponse).ToList();

    public ProprietarioResponse GetById(Guid id) => ToResponse(GetEntity(id));

    public ProprietarioResponse Create(ProprietarioRequest request)
    {
        Validate(request, null);
        var proprietario = new Proprietario
        {
            Id = Guid.NewGuid(),
            Nome = request.Nome.Trim(),
            Cpf = CpfValidator.Normalize(request.Cpf),
            DataNascimento = request.DataNascimento!.Value.Date
        };
        proprietarioRepository.Add(proprietario);
        return ToResponse(proprietario);
    }

    public ProprietarioResponse Update(Guid id, ProprietarioRequest request)
    {
        var proprietario = GetEntity(id);
        Validate(request, id);
        proprietario.Nome = request.Nome.Trim();
        proprietario.Cpf = CpfValidator.Normalize(request.Cpf);
        proprietario.DataNascimento = request.DataNascimento!.Value.Date;
        proprietarioRepository.Update(proprietario);
        return ToResponse(proprietario);
    }

    public void Delete(Guid id)
    {
        _ = GetEntity(id);
        if (veiculoRepository.HasByProprietarioId(id))
        {
            throw new DomainValidationException("Nao e possivel excluir o proprietario porque ele possui veiculos cadastrados.");
        }

        proprietarioRepository.Remove(id);
    }

    private void Validate(ProprietarioRequest request, Guid? currentId)
    {
        if (!CpfValidator.IsValid(request.Cpf)) throw new DomainValidationException("CPF invalido.");
        if (request.DataNascimento is null || request.DataNascimento.Value.Date > DateTime.UtcNow.Date) throw new DomainValidationException("A data de nascimento deve estar no passado.");

        var existing = proprietarioRepository.GetByCpf(CpfValidator.Normalize(request.Cpf));
        if (existing is not null && existing.Id != currentId) throw new DomainValidationException("Ja existe um proprietario cadastrado com este CPF.");
    }

    private Proprietario GetEntity(Guid id) => proprietarioRepository.GetById(id) ?? throw new NotFoundException("Proprietario nao encontrado.");
    private static ProprietarioResponse ToResponse(Proprietario item) => new(item.Id, item.Nome, item.Cpf, item.DataNascimento);
}
