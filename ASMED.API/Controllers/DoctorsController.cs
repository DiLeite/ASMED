using ASMED.Application.Interfaces;
using ASMED.Shared.DTOs;
using ASMED.Shared.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASMED.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorsController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    [HttpGet("me")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> GetMyProfile(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var doctor = await _doctorService.GetDoctorByUserIdAsync(userId, cancellationToken);
        if (doctor == null)
            return NotFound();

        return Ok(doctor);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateDoctorById(Guid id, [FromBody] DoctorUpdateRequest request, CancellationToken cancellationToken)
    {
        var result = await _doctorService.UpdateDoctorByIdAsync(id, request, cancellationToken);
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }


}
