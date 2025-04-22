using ASMED.Application.Interfaces;
using ASMED.Data.Context;
using ASMED.Domain.Entities;
using ASMED.Domain.Interfaces;
using ASMED.Shared.Models;
using ASMED.Shared.Requests;
using ASMED.Shared.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ASMED.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly AsmedContext _context;


    public AuthService(IUserRepository userRepository, IConfiguration configuration, AsmedContext asmedContext)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _context = asmedContext;
    }

    public async Task<LoginResponse?> Authenticate(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByEmailAsync(request.Email);
        if (user == null || !user.IsActive)
            return null;

        var roles = await _userRepository.GetRolesAsync(user);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email ?? ""),
            new Claim("role", roles.FirstOrDefault() ?? "")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpirationMinutes"] ?? "60")),
            signingCredentials: creds
        );

        return new LoginResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            UserId = user.Id,
            Role = roles.FirstOrDefault() ?? ""
        };
    }

    public async Task<ResponseMessage<string>> RegisterUserAsync(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        if (!await _userRepository.RoleExistsAsync(request.Role))
            return ResponseMessage<string>.CreateInvalidResult($"Role '{request.Role}' não existe.");

        if (await _userRepository.FindByEmailAsync(request.Email) is not null)
            return ResponseMessage<string>.CreateInvalidResult("E-mail já cadastrado.");

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName,
            Role = request.Role,
            EmailConfirmed = true,
            IsActive = true
        };

        var result = await _userRepository.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return ResponseMessage<string>.CreateInvalidResult(string.Join("; ", result.Errors.Select(e => e.Description)));

        await _userRepository.AddToRoleAsync(user, request.Role);

        // Vincular com médico automaticamente se Role == Doctor
        if (request.Role == "Doctor")
        {
            if (string.IsNullOrWhiteSpace(request.CRM) || string.IsNullOrWhiteSpace(request.Specialty))
                return ResponseMessage<string>.CreateInvalidResult("CRM e Especialidade são obrigatórios para médicos.");

            var address = request.Address is not null
                ? new Address
                {
                    Id = Guid.NewGuid(),
                    Street = request.Address.Street,
                    Number = request.Address.Number,
                    Neighborhood = request.Address.Neighborhood,
                    City = request.Address.City,
                    State = request.Address.State,
                    PostalCode = request.Address.PostalCode,
                    Complement = request.Address.Complement
                }
                : null;

            var doctor = new Doctor
            {
                Id = Guid.NewGuid(),
                FullName = request.FullName,
                CRM = request.CRM,
                Specialty = request.Specialty,
                Phone = request.Phone,
                Mobile = request.Mobile,
                Email = request.Email,
                UserId = user.Id,
                Address = address
            };

            await _context.Doctors.AddAsync(doctor);
            await _context.SaveChangesAsync();
        }

        return ResponseMessage<string>.CreateValidResult("Usuário criado com sucesso.");
    }


    public async Task<UserInfoResponse?> GetCurrentUserAsync(ClaimsPrincipal userClaims, CancellationToken cancellationToken)
    {
        var userId = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return null;

        var user = await _userRepository.FindByIdAsync(userId);
        if (user == null) return null;

        var roles = await _userRepository.GetRolesAsync(user);

        return new UserInfoResponse
        {
            UserId = user.Id,
            Email = user.Email ?? "",
            FullName = user.FullName ?? "",
            Role = roles.FirstOrDefault() ?? ""
        };
    }
}
