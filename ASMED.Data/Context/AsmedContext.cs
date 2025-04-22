using ASMED.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASMED.Data.Context;

public class AsmedContext : IdentityDbContext<ApplicationUser>
{
    public AsmedContext(DbContextOptions<AsmedContext> options)
        : base(options)
    {
    }

    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<DoctorPatient> DoctorPatients { get; set; }
    public DbSet<DoctorSecretary> DoctorSecretaries { get; set; }
    public DbSet<Surgery> Surgeries { get; set; }
    public DbSet<SurgeryDocument> SurgeryDocuments { get; set; }
    public DbSet<Address> Addresses { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Chave composta Doctor <-> Patient
        modelBuilder.Entity<DoctorPatient>()
            .HasKey(dp => new { dp.DoctorId, dp.PatientId });

        modelBuilder.Entity<DoctorPatient>()
            .HasOne(dp => dp.Doctor)
            .WithMany(d => d.Patients)
            .HasForeignKey(dp => dp.DoctorId);

        modelBuilder.Entity<DoctorPatient>()
            .HasOne(dp => dp.Patient)
            .WithMany(p => p.Doctors)
            .HasForeignKey(dp => dp.PatientId);

        // Chave composta Doctor <-> Secretary
        modelBuilder.Entity<DoctorSecretary>()
            .HasKey(ds => new { ds.DoctorId, ds.SecretaryId });

        modelBuilder.Entity<DoctorSecretary>()
            .HasOne(ds => ds.Doctor)
            .WithMany(d => d.Secretaries)
            .HasForeignKey(ds => ds.DoctorId);

        modelBuilder.Entity<Surgery>()
            .Property(s => s.ProcedureCodes)
            .HasConversion(
                v => string.Join(";", v),
                v => v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList()
            );
        
        modelBuilder.Entity<Doctor>()
            .HasOne(d => d.Address)
            .WithOne(a => a.Doctor)
            .HasForeignKey<Doctor>(d => d.AddressId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Patient>()
            .HasOne(p => p.Address)
            .WithOne(a => a.Patient)
            .HasForeignKey<Patient>(p => p.AddressId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}