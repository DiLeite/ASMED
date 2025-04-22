using ASMED.Domain.Entities;

namespace ASMED.Domain.Interfaces;

public interface IDoctorRepository
{
    Task<Doctor?> GetByUserIdAsync(string userId, CancellationToken cancellationToken);
    Task<Doctor?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddDoctorPatientRelationAsync(Guid doctorId, Guid patientId, CancellationToken cancellationToken);
    Task UpdateAsync(Doctor doctor, CancellationToken cancellationToken);
}
