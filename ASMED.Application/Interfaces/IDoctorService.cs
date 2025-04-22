using ASMED.Shared.DTOs;
using ASMED.Shared.Models;
using ASMED.Shared.Requests;

namespace ASMED.Application.Interfaces
{
    public interface IDoctorService
    {
        Task<ResponseMessage<DoctorDto?>> GetDoctorByUserIdAsync(string userId, CancellationToken cancellationToken);
        Task<ResponseMessage<DoctorDto?>> GetDoctorByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<ResponseMessage<DoctorDto?>> UpdateDoctorByIdAsync(Guid doctorId, DoctorUpdateRequest request, CancellationToken cancellationToken);
    }
}
