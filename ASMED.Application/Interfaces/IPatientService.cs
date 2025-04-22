using ASMED.Shared.DTOs;
using ASMED.Shared.Models;
using ASMED.Shared.Requests;
using System.Security.Claims;

namespace ASMED.Application.Interfaces
{
    public interface IPatientService
    {
        Task<ResponseMessage<PatientResponse>> CreateAsync(PatientCreateRequest request, ClaimsPrincipal userClaims, CancellationToken cancellationToken);
        Task<ResponseMessage<List<PatientResponse>>> GetAllByLoggedDoctorAsync(ClaimsPrincipal userClaims, CancellationToken cancellationToken);
        Task<ResponseMessage<PatientDto?>> GetPatientByInsuranceIdAsync(string cardNumber, CancellationToken cancellationToken);
    }
}
