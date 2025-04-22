using Microsoft.AspNetCore.Identity;

namespace ASMED.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }
    public string Role { get; set; }
    public bool IsActive { get; set; } = true;
}
