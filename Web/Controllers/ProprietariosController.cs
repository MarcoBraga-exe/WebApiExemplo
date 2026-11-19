using Microsoft.AspNetCore.Mvc;
using WebApiExemplo.Application.DTOs;
using WebApiExemplo.Application.Interfaces;

namespace WebApiExemplo.Web.Controllers;

/// <summary>Exposes the HTTP CRUD endpoints for owners.</summary>
[ApiController]
[Route("api/proprietarios")]
public class ProprietariosController(IProprietarioService service) : ControllerBase
{
    [HttpGet]
    public ActionResult<IReadOnlyCollection<ProprietarioResponse>> GetAll() => Ok(service.GetAll());

    [HttpGet("{id:guid}")]
    public ActionResult<ProprietarioResponse> GetById(Guid id) => Ok(service.GetById(id));

    [HttpPost]
    public ActionResult<ProprietarioResponse> Create(ProprietarioRequest request)
    {
        var result = service.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public ActionResult<ProprietarioResponse> Update(Guid id, ProprietarioRequest request) => Ok(service.Update(id, request));

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        service.Delete(id);
        return NoContent();
    }
}
