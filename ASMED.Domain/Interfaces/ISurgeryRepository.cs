using ASMED.Domain.Entities;

namespace ASMED.Domain.Interfaces
{
    public interface ISurgeryRepository
    {
        Task AddAsync(Surgery surgery, CancellationToken cancellationToken);
        Task<Surgery?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Surgery>> GetAllByDoctorIdAsync(Guid doctorId, CancellationToken cancellationToken);
        Task UpdateAsync(Surgery surgery, CancellationToken cancellationToken);
        Task AddDocumentAsync(SurgeryDocument document, CancellationToken cancellationToken);
        Task<List<SurgeryDocument>> GetDocumentsAsync(Guid surgeryId, CancellationToken cancellationToken);
    }
}