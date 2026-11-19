namespace WebApiExemplo.Application.Exceptions;

/// <summary>Indica que o recurso solicitado não existe no repositório.</summary>
public class NotFoundException(string message) : Exception(message);
