using ArtTogether.Application.DTOs;

namespace ArtTogether.Application.Interfaces;

public interface IAuthService
{
    Task<TokenDto> LoginAsGuestAsync();
    Task<TokenDto> LoginWithGoogleAsync(GoogleSigninDto dto);
}
