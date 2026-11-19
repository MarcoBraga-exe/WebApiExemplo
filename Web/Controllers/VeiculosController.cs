using Microsoft.AspNetCore.Mvc;
using WebApiExemplo.Application.DTOs;
using WebApiExemplo.Application.Interfaces;

namespace WebApiExemplo.Web.Controllers;

/// <summary>Exposes the HTTP CRUD endpoints and search filters for vehicles.</summary>
[ApiController]
[Route("api/veiculos")]
public class VeiculosController(IVeiculoService service) : ControllerBase
{
    [HttpGet]
    public ActionResult<IReadOnlyCollection<VeiculoResponse>> Search([FromQuery] string? placa, [FromQuery] string? modelo, [FromQuery] string? nomeProprietario) => Ok(service.Search(placa, modelo, nomeProprietario));

    [HttpGet("{id:guid}")]
    public ActionResult<VeiculoResponse> GetById(Guid id) => Ok(service.GetById(id));

    [HttpPost]
    public async Task<ActionResult<VeiculoResponse>> Create(VeiculoRequest request, CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public ActionResult<VeiculoResponse> Update(Guid id, VeiculoRequest request) => Ok(service.Update(id, request));

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        service.Delete(id);
        return NoContent();
    }
}
