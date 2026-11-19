namespace WebApiExemplo.Domain.Entities;

/// <summary>Representa o dono de veículos armazenado pela aplicação.</summary>
public class Proprietario
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
}
