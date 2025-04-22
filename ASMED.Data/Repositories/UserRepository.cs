using ASMED.Domain.Entities;
using ASMED.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ASMED.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserRepository(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<ApplicationUser?> FindByIdAsync(string id)
        => await _userManager.FindByIdAsync(id);

    public async Task<ApplicationUser?> FindByEmailAsync(string email)
        => await _userManager.FindByEmailAsync(email);

    public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        => await _userManager.GetRolesAsync(user);

    public async Task<bool> RoleExistsAsync(string role)
        => await _roleManager.RoleExistsAsync(role);

    public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        => await _userManager.CreateAsync(user, password);

    public async Task AddToRoleAsync(ApplicationUser user, string role)
        => await _userManager.AddToRoleAsync(user, role);
}
