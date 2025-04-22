namespace ASMED.Domain.Entities;

public class DoctorSecretary
{
    public Guid DoctorId { get; set; }
    public Doctor Doctor { get; set; }

    public string SecretaryId { get; set; } // FK to Identity User
}