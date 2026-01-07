using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EnglishCenter.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace EnglishCenter.Services
{
    public class AuthService : IAuthService
    {
        private UserManager<User> userManager;
        private readonly IConfiguration _config;

        public AuthService(UserManager<User> userManager, IConfiguration config)
        {
            this.userManager = userManager;
            _config = config;
        }
        Task<User> IAuthService.ChangePassword(User user, string newPassword)
        {
            throw new NotImplementedException();
        }

        Task IAuthService.ForgotPassword(string email)
        {
            throw new NotImplementedException();
        }

        async Task<JwtSecurityToken> IAuthService.Login(string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null || !await userManager.CheckPasswordAsync(user, password))
                throw new UnauthorizedAccessException("Thông tin đăng nhập không chính xác");

            var roles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!)
            };

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
            );

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(_config["Jwt:ExpireMinutes"]!)
                ),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }

        Task IAuthService.Logout()
        {
            throw new NotImplementedException();
        }

        Task<User> IAuthService.Register(User user)
        {
            throw new NotImplementedException();
        }
    }
}
