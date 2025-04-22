using ASMED.API.Utils;
using ASMED.Application.Interfaces;
using ASMED.Data.Context;
using ASMED.Shared.DTOs;
using ASMED.Shared.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASMED.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Doctor")]
public class SurgeryController : ControllerBase
{
    private readonly ISurgeryService _surgeryService;

    public SurgeryController(ISurgeryService surgeryService)
    {
        _surgeryService = surgeryService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SurgeryCreateRequest request, CancellationToken cancellationToken)
    {
        var user = User;
        var result = await _surgeryService.CreateAsync(request, user, cancellationToken);
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var user = User;
        var result = await _surgeryService.GetByIdAsync(id,user, cancellationToken);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _surgeryService.GetAllByDoctorAsync(User, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] SurgeryUpdateRequest request, CancellationToken cancellationToken)
    {
        var result = await _surgeryService.UpdateAsync(id, request, cancellationToken);
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _surgeryService.DeleteAsync(id, cancellationToken);
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [Authorize(Roles = "Doctor")]
    [HttpPost("{id:guid}/documents")]
    public async Task<IActionResult> AddDocument(Guid id, [FromBody] SurgeryDocumentRequest request, CancellationToken cancellationToken)
    {
        var result = await _surgeryService.AddDocumentAsync(id, request, User, cancellationToken);
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [Authorize(Roles = "Doctor")]
    [HttpGet("{id:guid}/documents")]
    public async Task<IActionResult> GetDocuments(Guid id, CancellationToken cancellationToken)
    {
        var result = await _surgeryService.GetDocumentsAsync(id, User, cancellationToken);
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("importar-documentos-locais")]
    public async Task<IActionResult> ImportarDocumentosLocais([FromServices] AsmedContext context)
    {
        var caminho = Path.Combine(Directory.GetCurrentDirectory(), "Anexos");
        var importer = new SurgeryDocumentImporter(context, caminho);
        await importer.ImportAsync();
        return Ok("Documentos importados.");
    }
}
