namespace ASMED.Shared.Requests
{
    public class RegisterUserRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public string? CRM { get; set; }
        public string? Specialty { get; set; }

        public AddressRequest? Address { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? CPF { get; set; }

    }
}