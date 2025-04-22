using ASMED.Data.Context;
using ASMED.Domain.Entities;
using ASMED.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASMED.Data.Repositories;

public class SurgeryRepository : ISurgeryRepository
{
    private readonly AsmedContext _context;

    public SurgeryRepository(AsmedContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Surgery surgery, CancellationToken cancellationToken)
    {
        await _context.Surgeries.AddAsync(surgery, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Surgery>> GetAllByDoctorIdAsync(Guid doctorId, CancellationToken cancellationToken)
    {
        return await _context.Surgeries
            .Include(s => s.Patient)
            .Include(s => s.Documents)
            .Where(s => s.DoctorId == doctorId && s.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<Surgery?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Surgeries
            .Include(s => s.Patient)
            .Include(s => s.Documents)
            .FirstOrDefaultAsync(s => s.Id == id && s.IsActive, cancellationToken);
    }

    public async Task UpdateAsync(Surgery surgery, CancellationToken cancellationToken)
    {
        _context.Surgeries.Update(surgery);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Surgery surgery, CancellationToken cancellationToken)
    {
        surgery.IsActive = false;
        _context.Surgeries.Update(surgery);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task AddDocumentAsync(SurgeryDocument document, CancellationToken cancellationToken)
    {
        await _context.SurgeryDocuments.AddAsync(document, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<SurgeryDocument>> GetDocumentsAsync(Guid surgeryId, CancellationToken cancellationToken)
    {
        return await _context.SurgeryDocuments
            .Where(d => d.SurgeryId == surgeryId)
            .OrderByDescending(d => d.UploadedAt)
            .ToListAsync(cancellationToken);
    }

}
