using FinTrackPro.Domain.Entities;

namespace FinTrackPro.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}