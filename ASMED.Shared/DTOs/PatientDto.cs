namespace ASMED.Shared.DTOs
{
    public class PatientDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Mobile { get; set; } = null!;
        public string CPF { get; set; } = null!;
        public string CardNumber { get; set; } = null!;
        public string Insurance { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public bool IsActive { get; set; }

        public AddressDto? Address { get; set; }
    }
}