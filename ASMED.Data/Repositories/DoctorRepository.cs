using ASMED.Data.Context;
using ASMED.Domain.Entities;
using ASMED.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASMED.Data.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly AsmedContext _context;

    public DoctorRepository(AsmedContext context)
    {
        _context = context;
    }

    public async Task<Doctor?> GetByUserIdAsync(string id, CancellationToken cancellationToken)
    {
        return await _context.Doctors
            .Include(d => d.Address)
            .FirstOrDefaultAsync(d => d.UserId == id, cancellationToken);
    }

    public async Task<Doctor?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Doctors
            .Include(d => d.Address)
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }
    public async Task UpdateAsync(Doctor doctor, CancellationToken cancellationToken)
    {
        _context.Entry(doctor).State = EntityState.Modified;

        if (doctor.Address != null)
            _context.Entry(doctor.Address).State = doctor.Address.Id == Guid.Empty ? EntityState.Added : EntityState.Modified;

        await _context.SaveChangesAsync();
    }

    public async Task AddDoctorPatientRelationAsync(Guid doctorId, Guid patientId, CancellationToken cancellationToken)
    {
        var relation = new DoctorPatient
        {
            DoctorId = doctorId,
            PatientId = patientId
        };

        _context.DoctorPatients.Add(relation);
        await _context.SaveChangesAsync();
    }
}
