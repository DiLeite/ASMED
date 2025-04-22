using ASMED.Shared.Models;
using ASMED.Shared.Requests;
using ASMED.Shared.Responses;
using System.Security.Claims;

namespace ASMED.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse?> Authenticate(LoginRequest request, CancellationToken cancellationToken);
    Task<ResponseMessage<string>> RegisterUserAsync(RegisterUserRequest request, CancellationToken cancellationToken);
    Task<UserInfoResponse?> GetCurrentUserAsync(ClaimsPrincipal userClaims, CancellationToken cancellationToken);
}
