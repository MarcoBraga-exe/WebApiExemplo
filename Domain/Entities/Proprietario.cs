namespace WebApiExemplo.Domain.Entities;

/// <summary>Owner entity stored by the application.</summary>
public class Proprietario
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
}
