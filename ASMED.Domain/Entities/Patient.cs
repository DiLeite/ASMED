namespace ASMED.Domain.Entities;

public class Patient
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public DateTime BirthDate { get; set; }
    public string CPF { get; set; }
    public string Phone { get; set; }
    public string Mobile { get; set; }
    public string Email { get; set; }

    public string Insurance { get; set; }
    public string CardNumber { get; set; }

    public Guid? AddressId { get; set; }
    public Address? Address { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<DoctorPatient> Doctors { get; set; }
}