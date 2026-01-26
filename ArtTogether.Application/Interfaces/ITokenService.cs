using ArtTogether.Domain.Entities;

namespace ArtTogether.Application.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
    string GenerateRefreshToken();
}
