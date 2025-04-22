using ASMED.Shared.Requests;
using ASMED.Shared.Responses;
using ASMED.Shared.Models;
using System.Security.Claims;
using ASMED.Domain.Enums;
using ASMED.Domain.Entities;

namespace ASMED.Application.Interfaces
{
    public interface ISurgeryService
    {
        Task<ResponseMessage<SurgeryResponse>> CreateAsync(SurgeryCreateRequest request, ClaimsPrincipal user, CancellationToken cancellationToken);
        Task<ResponseMessage<SurgeryResponse>> GetByIdAsync(Guid id, ClaimsPrincipal user, CancellationToken cancellationToken);
        Task<ResponseMessage<List<SurgeryResponse>>> GetAllByDoctorAsync(ClaimsPrincipal user, CancellationToken cancellationToken);
        Task<ResponseMessage<string>> UpdateStatusAsync(Guid id, ESurgeryStatus status, ClaimsPrincipal user, CancellationToken cancellationToken);
        Task<ResponseMessage<string>> UpdateAsync(Guid id, SurgeryUpdateRequest request, CancellationToken cancellationToken);
        Task<ResponseMessage<string>> DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<ResponseMessage<string>> AddDocumentAsync(Guid surgeryId, SurgeryDocumentRequest request, ClaimsPrincipal user, CancellationToken cancellationToken);
        Task<ResponseMessage<List<SurgeryDocumentResponse>>> GetDocumentsAsync(Guid surgeryId, ClaimsPrincipal user, CancellationToken cancellationToken);
    }
}