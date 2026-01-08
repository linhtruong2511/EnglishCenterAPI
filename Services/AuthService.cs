using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EnglishCenter.Authorization;
using EnglishCenter.DTO;
using EnglishCenter.Extensions;
using EnglishCenter.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace EnglishCenter.Services
{
    public class AuthService : IAuthService
    {
        private UserManager<User> userManager;
        private readonly IConfiguration _config;
        private IHttpContextAccessor http;
        private IFileService fileService;

        public AuthService(UserManager<User> userManager,
            IConfiguration config,
            IHttpContextAccessor http,
            IFileService fileService)
        {
            this.userManager = userManager;
            _config = config;
            this.http = http;
            this.fileService = fileService;
        }

        async Task<User> IAuthService.GetUser(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("Thông tin không hợp lệ");
            var user = await userManager.FindByIdAsync(id);
            if (user is null) throw new Exception("Không tìm thấy user hợp lệ");
            return user;

        }
        async Task<User> IAuthService.ChangePassword(User user, string newPassword, string currentPassword)
        {
            var userEntity = await userManager.FindByEmailAsync(user.Email!);
            if (userEntity is null) throw new Exception("Thông tin xác thực người dùng không chính xác");
            var result = await userManager.ChangePasswordAsync(userEntity, currentPassword, newPassword);
            if (!result.Succeeded)
                throw new Exception("Đổi mật khẩu không thành công");
            return userEntity;
        }

        Task IAuthService.ForgotPassword(string email)
        {
            throw new NotImplementedException("Chưa triển khai");
        }

        async Task<LoginResponse> IAuthService.Login(string email, string password)
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

            var token = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(_config["Jwt:ExpireMinutes"]!)
                ),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            ));

            return new LoginResponse { 
                User = new UserResponseDto
                {
                    Id = user.Id,
                    Avatar = user.Avatar,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                },
                Token = token
            };
        }

        async Task IAuthService.Logout()
        {
            throw new NotImplementedException("Chưa triển khai");
        }

        async Task<User> IAuthService.Register(UserRegisterDto dto)
        {
            var existedUser = await userManager.FindByEmailAsync(dto.Email!);
            if (existedUser is not null) throw new Exception("Email đã được sử dụng");
            var user = new User
            {
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ",
                    result.Errors.Select(e => e.Description)));
            }
            await userManager.AddToRoleAsync(user, Roles.STUDENT.ToString());
            return user;
        }

        async Task<User> IAuthService.GetUser(ClaimsPrincipal user)
        {
            return await userManager.GetUserAsync(user);
        }

        async Task<User> IAuthService.UploadAvatar(ClaimsPrincipal user, IFormFile file)
        {
            var currentUser = await userManager.GetUserAsync(user);
            if (currentUser is not null)
            {
                var result = await fileService.SaveImageAsync(file, "images/avatar");
                currentUser.Avatar = result;
            }
            return currentUser;
        }
        async Task<User> IAuthService.UpdateProfile(ClaimsPrincipal user, UpdateProfileDto dto)
        {
            var currentUser = await userManager.GetUserAsync(user);
            if (currentUser is not null)
            {
                currentUser.FirstName = dto.FirstName;
                currentUser.LastName = dto.LastName;
            }
            return currentUser;
        }

    }
}
