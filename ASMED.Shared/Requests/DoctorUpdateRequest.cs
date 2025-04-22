namespace ASMED.Shared.Requests
{
    public class DoctorUpdateRequest
    {
        public string FullName { get; set; }
        public string CRM { get; set; }
        public string Specialty { get; set; }
        public string CPF { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }

        public AddressRequest Address { get; set; }
    }
}