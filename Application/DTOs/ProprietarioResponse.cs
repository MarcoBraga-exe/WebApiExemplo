namespace WebApiExemplo.Application.DTOs;

/// <summary>Output model returned for an owner.</summary>
public record ProprietarioResponse(Guid Id, string Nome, string Cpf, DateTime DataNascimento);
