namespace WebApiExemplo.Domain.Entities;

/// <summary>Representa o veículo e a referência obrigatória ao seu proprietário.</summary>
public class Veiculo
{
    public Guid Id { get; set; }
    public string Placa { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public int Ano { get; set; }
    public Guid ProprietarioId { get; set; }
}
