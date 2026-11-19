namespace WebApiExemplo.Application.Exceptions;

/// <summary>Indica que uma regra de negócio impediu a operação solicitada.</summary>
public class DomainValidationException(string message) : Exception(message);
