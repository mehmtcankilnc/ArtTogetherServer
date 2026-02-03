using ArtTogether.Application.DTOs;
using ArtTogether.Application.Interfaces;
using ArtTogether.Domain.Entities;
using ArtTogether.Infrastructure.Identity;
using ArtTogether.Infrastructure.Persistence;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ArtTogether.Infrastructure.Services;

public class AuthService(
    UserManager<ApplicationIdentityUser> userManager,
    ApplicationDbContext dbContext, ITokenService tokenService,
    IConfiguration configuration) : IAuthService
{
    public async Task<TokenDto> LoginAsGuestAsync()
    {
        using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            var guestId = Guid.NewGuid();
            var guestEmail = $"{guestId}@guest.arttogether.com";

            var identityUser = new ApplicationIdentityUser
            {
                Id = guestId,
                UserName = guestEmail,
                Email = guestEmail,
                EmailConfirmed = true,
            };

            var createResult = await userManager.CreateAsync(identityUser);
            if (!createResult.Succeeded) throw new Exception("Guest Sign In Failed!");

            var user = new User
            {
                Id = guestId,
                Email = guestEmail,
                IsGuest = true
            };

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            var accessToken = tokenService.CreateToken(user);
            var refreshToken = tokenService.GenerateRefreshToken();

            identityUser.RefreshToken = refreshToken;
            identityUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await userManager.UpdateAsync(identityUser);

            return new TokenDto 
            { 
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<TokenDto> LoginWithGoogleAsync(GoogleSigninDto dto)
    {
        GoogleJsonWebSignature.Payload payload;
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = [configuration["Google:ClientId"]!]
            };
            payload = await GoogleJsonWebSignature.ValidateAsync(dto.IdToken, settings);
        }
        catch
        {
            throw new Exception("Google Sign In Failed!");
        }

        var identityUser = await userManager.FindByEmailAsync(payload.Email);

        if (identityUser == null)
        {
            using var transaction = dbContext.Database.BeginTransaction();
            try
            {
                identityUser = new ApplicationIdentityUser
                {
                    UserName = payload.Email,
                    Email = payload.Email,
                    EmailConfirmed = true
                };

                var createResult = await userManager.CreateAsync(identityUser);
                if (!createResult.Succeeded) throw new Exception("Google User Could Not Created!");

                var user = new User
                {
                    Id = identityUser.Id,
                    Email = identityUser.Email,
                    IsGuest = false
                };

                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        var entityUser = await dbContext.Users.FindAsync(identityUser.Id);

        var accessToken = tokenService.CreateToken(entityUser!);
        var refreshToken = tokenService.GenerateRefreshToken();

        identityUser.RefreshToken = refreshToken;
        identityUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await userManager.UpdateAsync(identityUser);

        return new TokenDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };
    }

    public async Task<TokenDto> RefreshTokenAsync(string accessToken, string refreshToken)
    {
        var principal = GetPrincipalFromExpiredToken(accessToken);
        var userId = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (userId == null) throw new Exception("Geçersiz Token");

        var identityUser = await userManager.FindByIdAsync(userId);

        if (identityUser == null || identityUser.RefreshToken != refreshToken
            || identityUser.RefreshTokenExpiryTime <= DateTime.UtcNow)
            throw new Exception("Geçersiz veya süresi dolmuş refresh token");

        var appUser = await dbContext.Users.FindAsync(Guid.Parse(userId));

        var newAccessToken = tokenService.CreateToken(appUser!);
        var newRefreshToken = tokenService.GenerateRefreshToken();

        identityUser.RefreshToken = newRefreshToken;
        identityUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await userManager.UpdateAsync(identityUser);
        return new TokenDto { AccessToken = newAccessToken, RefreshToken = newRefreshToken };
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]!)),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Geçersiz Token");

        return principal;
    }
}
