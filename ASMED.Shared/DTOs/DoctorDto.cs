namespace ASMED.Shared.DTOs
{
    public class DoctorDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string CRM { get; set; }
        public string Specialty { get; set; }
        public string CPF { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string Email { get; set; }
        public AddressDto? Address { get; set; }
    }
}