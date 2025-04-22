using ASMED.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ASMED.Domain.Interfaces;

public interface IUserRepository
{
    Task<ApplicationUser?> FindByIdAsync(string id);
    Task<ApplicationUser?> FindByEmailAsync(string email);
    Task<IList<string>> GetRolesAsync(ApplicationUser user);
    Task<bool> RoleExistsAsync(string role);
    Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
    Task AddToRoleAsync(ApplicationUser user, string role);
}
