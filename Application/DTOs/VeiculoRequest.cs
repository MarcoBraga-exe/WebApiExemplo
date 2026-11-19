using System.ComponentModel.DataAnnotations;

namespace WebApiExemplo.Application.DTOs;

/// <summary>Input model for creating or updating a vehicle.</summary>
public class VeiculoRequest
{
    [Required(ErrorMessage = "A placa e obrigatoria.")]
    [RegularExpression("^[A-Za-z]{3}-\\d{4}$", ErrorMessage = "A placa deve estar no formato ABC-1234.")]
    public string Placa { get; init; } = string.Empty;

    [Required(ErrorMessage = "O modelo e obrigatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O modelo deve ter entre 2 e 100 caracteres.")]
    public string Modelo { get; init; } = string.Empty;

    [Range(1981, int.MaxValue, ErrorMessage = "O ano deve ser maior que 1980.")]
    public int Ano { get; init; }

    // Guid.Empty is rejected by VeiculoService when the field is absent or invalid.
    public Guid ProprietarioId { get; init; }
}
