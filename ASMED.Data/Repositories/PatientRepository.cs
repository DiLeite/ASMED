using ASMED.Data.Context;
using ASMED.Domain.Entities;
using ASMED.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASMED.Data.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly AsmedContext _context;

    public PatientRepository(AsmedContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Patient patient, CancellationToken cancellationToken)
    {
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByCpfAsync(string cpf, CancellationToken cancellationToken)
    {
        return await _context.Patients.AnyAsync(p => p.CPF == cpf);
    }

    public async Task<Patient?> GetByIdAsync(Guid id)
    {
        return await _context.Patients.FindAsync(id);
    }

    public async Task<IEnumerable<Patient>> GetAllByDoctorIdAsync(Guid doctorId, CancellationToken cancellationToken)
    {
        return await _context.DoctorPatients
            .Where(dp => dp.DoctorId == doctorId)
            .Select(dp => dp.Patient)
            .Where(p => p.IsActive)
            .ToListAsync();
    }

    public async Task<Patient?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Patients.Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Patient?> GetByInsuranceIdAsync(string cardNumber, CancellationToken cancellationToken)
    {
        return await _context.Patients
            .Include(p => p.Address)
            .FirstOrDefaultAsync(p => p.CardNumber == cardNumber, cancellationToken);
    }

}
