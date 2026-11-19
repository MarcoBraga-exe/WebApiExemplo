namespace WebApiExemplo.Domain.Entities;

/// <summary>Vehicle entity linked to one owner.</summary>
public class Veiculo
{
    public Guid Id { get; set; }
    public string Placa { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public int Ano { get; set; }
    public Guid ProprietarioId { get; set; }
}
