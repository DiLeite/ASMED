using ASMED.Shared.DTOs;

namespace ASMED.Shared.Requests
{
    public class PatientCreateRequest
    {
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Insurance { get; set; }
        public string CardNumber { get; set; }

        public AddressDto Address { get; set; }

        public Guid? DoctorId { get; set; }
    }
}