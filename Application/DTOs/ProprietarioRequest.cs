using System.ComponentModel.DataAnnotations;

namespace WebApiExemplo.Application.DTOs;

/// <summary>Input model for creating or updating an owner.</summary>
public class ProprietarioRequest
{
    [Required(ErrorMessage = "O nome e obrigatorio.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
    public string Nome { get; init; } = string.Empty;

    [Required(ErrorMessage = "O CPF e obrigatorio.")]
    public string Cpf { get; init; } = string.Empty;

    [Required(ErrorMessage = "A data de nascimento e obrigatoria.")]
    public DateTime? DataNascimento { get; init; }
}
