using System.IdentityModel.Tokens.Jwt;
using EnglishCenter.Models;

namespace EnglishCenter.Services
{
    public interface IAuthService
    {
        Task<JwtSecurityToken> Login(string email, string password);
        Task<User> Register(User user);
        Task<User> ChangePassword(User user, string newPassword);
        Task ForgotPassword(string email);
        Task Logout();
    }
}
