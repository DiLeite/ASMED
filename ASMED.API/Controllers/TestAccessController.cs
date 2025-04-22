using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASMED.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestAccessController : ControllerBase
{
    [HttpGet("public")]
    public IActionResult Public() =>
        Ok("Acesso público liberado");

    [Authorize]
    [HttpGet("authenticated")]
    public IActionResult Authenticated() =>
        Ok("Você está autenticado!");

    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public IActionResult AdminOnly() =>
        Ok("Acesso liberado apenas para Admin.");

    [Authorize(Roles = "Doctor")]
    [HttpGet("doctor")]
    public IActionResult DoctorOnly() =>
        Ok("Acesso liberado apenas para Médico.");

    [Authorize(Roles = "Secretary")]
    [HttpGet("secretary")]
    public IActionResult SecretaryOnly() =>
        Ok("Acesso liberado apenas para Secretária.");
}
