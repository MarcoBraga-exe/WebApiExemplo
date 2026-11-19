using WebApiExemplo.Application.DTOs;
using WebApiExemplo.Application.Exceptions;
using WebApiExemplo.Application.Interfaces;
using WebApiExemplo.Domain.Entities;

namespace WebApiExemplo.Application.Services;

/// <summary>Applies vehicle rules, supports search, and publishes creation events.</summary>
public class VeiculoService(IVeiculoRepository veiculoRepository, IProprietarioRepository proprietarioRepository, IQueuePublisher queuePublisher) : IVeiculoService
{
    public IReadOnlyCollection<VeiculoResponse> Search(string? placa, string? modelo, string? nomeProprietario)
    {
        var proprietarios = proprietarioRepository.GetAll().ToDictionary(item => item.Id);
        return veiculoRepository.GetAll()
            .Where(item => string.IsNullOrWhiteSpace(placa) || item.Placa.Contains(placa.Trim(), StringComparison.OrdinalIgnoreCase))
            .Where(item => string.IsNullOrWhiteSpace(modelo) || item.Modelo.Contains(modelo.Trim(), StringComparison.OrdinalIgnoreCase))
            .Where(item => string.IsNullOrWhiteSpace(nomeProprietario) || (proprietarios.TryGetValue(item.ProprietarioId, out var owner) && owner.Nome.Contains(nomeProprietario.Trim(), StringComparison.OrdinalIgnoreCase)))
            .Select(item => ToResponse(item, proprietarios[item.ProprietarioId]))
            .ToList();
    }

    public VeiculoResponse GetById(Guid id)
    {
        var veiculo = GetEntity(id);
        return ToResponse(veiculo, GetProprietario(veiculo.ProprietarioId));
    }

    public async Task<VeiculoResponse> CreateAsync(VeiculoRequest request, CancellationToken cancellationToken)
    {
        Validate(request, null);
        var proprietario = GetProprietario(request.ProprietarioId);
        var veiculo = new Veiculo
        {
            Id = Guid.NewGuid(),
            Placa = request.Placa.Trim().ToUpperInvariant(),
            Modelo = request.Modelo.Trim(),
            Ano = request.Ano,
            ProprietarioId = proprietario.Id
        };
        veiculoRepository.Add(veiculo);

        var response = ToResponse(veiculo, proprietario);
        await queuePublisher.PublishAsync(new VeiculoCriadoMessage(response.Id, response.Placa, response.NomeProprietario, DateTimeOffset.UtcNow), cancellationToken);
        return response;
    }

    public VeiculoResponse Update(Guid id, VeiculoRequest request)
    {
        var veiculo = GetEntity(id);
        Validate(request, id);
        var proprietario = GetProprietario(request.ProprietarioId);
        veiculo.Placa = request.Placa.Trim().ToUpperInvariant();
        veiculo.Modelo = request.Modelo.Trim();
        veiculo.Ano = request.Ano;
        veiculo.ProprietarioId = proprietario.Id;
        veiculoRepository.Update(veiculo);
        return ToResponse(veiculo, proprietario);
    }

    public void Delete(Guid id)
    {
        _ = GetEntity(id);
        veiculoRepository.Remove(id);
    }

    private void Validate(VeiculoRequest request, Guid? currentId)
    {
        if (request.ProprietarioId == Guid.Empty) throw new DomainValidationException("O proprietario e obrigatorio.");
        if (request.Ano > DateTime.UtcNow.Year + 1) throw new DomainValidationException("O ano informado nao e valido.");

        var existing = veiculoRepository.GetByPlaca(request.Placa.Trim().ToUpperInvariant());
        if (existing is not null && existing.Id != currentId) throw new DomainValidationException("Ja existe um veiculo cadastrado com esta placa.");
    }

    private Veiculo GetEntity(Guid id) => veiculoRepository.GetById(id) ?? throw new NotFoundException("Veiculo nao encontrado.");
    private Proprietario GetProprietario(Guid id) => proprietarioRepository.GetById(id) ?? throw new NotFoundException("Proprietario nao encontrado.");
    private static VeiculoResponse ToResponse(Veiculo item, Proprietario owner) => new(item.Id, item.Placa, item.Modelo, item.Ano, item.ProprietarioId, owner.Nome);
}
