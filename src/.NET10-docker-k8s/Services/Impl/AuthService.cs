using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Net10.docker.k8s.Model;
using Net10.docker.k8s.Repositories;

namespace Net10.docker.k8s.Services.Impl
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenService _tokenService;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUserRepository userRepository, JwtTokenService tokenService, IPasswordHasher<User> passwordHasher, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public (bool Success, string? AccessToken, string? RefreshToken, long ExpiresIn) Authenticate(string username, string password)
        {
            var user = _userRepository.FindByUsername(username);
            if (user == null) return (false, null, null, 0);

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result == PasswordVerificationResult.Failed) return (false, null, null, 0);

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            _userRepository.Update(user);

            _logger.LogInformation("User {Username} authenticated", username);

            // ExpiresIn read from configuration by JwtTokenService when generating token; for simplicity return default 0
            return (true, accessToken, refreshToken, 0);
        }

        public (bool Success, string? AccessToken, string? RefreshToken) Refresh(string username, string refreshToken)
        {
            var user = _userRepository.FindByUsername(username);
            if (user == null) return (false, null, null);
            if (user.RefreshToken != refreshToken) return (false, null, null);
            if (user.RefreshTokenExpiry == null || user.RefreshTokenExpiry <= DateTime.UtcNow) return (false, null, null);

            var newAccess = _tokenService.GenerateAccessToken(user);
            var newRefresh = _tokenService.GenerateRefreshToken();
            user.RefreshToken = newRefresh;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            _userRepository.Update(user);

            return (true, newAccess, newRefresh);
        }

        public bool Revoke(string username)
        {
            var user = _userRepository.FindByUsername(username);
            if (user == null) return false;
            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            _userRepository.Update(user);
            return true;
        }
    }
}
