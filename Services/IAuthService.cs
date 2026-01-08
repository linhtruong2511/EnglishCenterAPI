using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EnglishCenter.DTO;
using EnglishCenter.Models;

namespace EnglishCenter.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(string email, string password);
        Task<User> Register(UserRegisterDto user);
        Task<User> ChangePassword(User user, string newPassword, string currentPassword);
        Task ForgotPassword(string email);
        Task Logout();
        Task<User> GetUser(string id);
        Task<User> GetUser(ClaimsPrincipal user);
        Task<User> UploadAvatar(ClaimsPrincipal user, IFormFile file);
        Task<User> UpdateProfile(ClaimsPrincipal user, UpdateProfileDto dto);
    }
}
