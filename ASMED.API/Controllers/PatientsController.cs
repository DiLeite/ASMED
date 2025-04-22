using ASMED.Application.Interfaces;
using ASMED.Application.Services;
using ASMED.Shared.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASMED.API.Controllers;

[Authorize(Roles = "Admin,Doctor,Secretary")]
[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(PatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(PatientCreateRequest request, CancellationToken cancellationToken)
    {
        var result = await _patientService.CreateAsync(request, User, cancellationToken);
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await _patientService.GetAllByLoggedDoctorAsync(User, cancellationToken);
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [Authorize(Roles = "Doctor,Admin")]
    [HttpGet("by-insurance/{cardNumber}")]
    public async Task<IActionResult> GetByInsuranceId(string cardNumber, CancellationToken cancellationToken)
    {
        var result = await _patientService.GetPatientByInsuranceIdAsync(cardNumber, cancellationToken);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }
}
