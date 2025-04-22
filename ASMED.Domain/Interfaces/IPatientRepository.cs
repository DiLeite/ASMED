using ASMED.Domain.Entities;

namespace ASMED.Domain.Interfaces;

public interface IPatientRepository
{
    Task AddAsync(Patient patient, CancellationToken cancellationToken);
    Task<bool> ExistsByCpfAsync(string cpf, CancellationToken cancellationToken);
    Task<Patient?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<Patient>> GetAllByDoctorIdAsync(Guid doctorId, CancellationToken cancellationToken);
    Task<Patient?> GetByInsuranceIdAsync(string cardNumber, CancellationToken cancellationToken);
}
