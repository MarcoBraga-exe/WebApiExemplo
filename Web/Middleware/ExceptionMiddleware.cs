using Microsoft.AspNetCore.Mvc;
using WebApiExemplo.Application.Exceptions;

namespace WebApiExemplo.Web.Middleware;

/// <summary>Traduz exceções conhecidas em respostas Problem Details para os consumidores da API.</summary>
public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        // Exceções de negócio não expõem detalhes internos da aplicação.
        catch (NotFoundException exception)
        {
            await WriteProblemAsync(context, StatusCodes.Status404NotFound, exception.Message);
        }
        catch (DomainValidationException exception)
        {
            await WriteProblemAsync(context, StatusCodes.Status400BadRequest, exception.Message);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Erro inesperado ao processar a requisição.");
            await WriteProblemAsync(context, StatusCodes.Status500InternalServerError, "Ocorreu um erro inesperado.");
        }
    }

    private static Task WriteProblemAsync(HttpContext context, int statusCode, string detail)
    {
        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsJsonAsync(new ProblemDetails { Status = statusCode, Title = "Erro na requisição", Detail = detail });
    }
}
