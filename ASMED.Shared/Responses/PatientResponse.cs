namespace ASMED.Shared.Requests
{
    public class PatientResponse
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
    }
}