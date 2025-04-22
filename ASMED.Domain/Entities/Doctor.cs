namespace ASMED.Domain.Entities
{
    public class Doctor
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string CRM { get; set; }
        public string Specialty { get; set; }
        public string CPF { get; set; }

        public string UserId { get; set; }
        public Guid? AddressId { get; set; }
        public Address? Address { get; set; }

        public ICollection<DoctorSecretary> Secretaries { get; set; }
        public ICollection<DoctorPatient> Patients { get; set; }
    }
}