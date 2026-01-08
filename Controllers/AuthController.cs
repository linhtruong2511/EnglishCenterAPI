using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EnglishCenter.DTO;
using EnglishCenter.Services;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Controllers
{
    /// <summary>
    /// API dành cho xác thực, cập nhật thông tin người dùng
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("login")] 
        public async Task<IActionResult> Login(string email, string password)
        {
            var token = new JwtSecurityTokenHandler().WriteToken(await authService.Login(email, password));
            return Ok(new
            {
                Token = token,
            });
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto dto)
        {
            return Ok(await authService.Register(dto));
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(string newPassword, string currentPassword)
        {
            var currentUser = await authService.GetUser(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (currentUser is not null)
            {
                var user = await authService.ChangePassword(currentUser, newPassword, currentPassword);
                return Ok(new UserResponseDto {
                    Id = currentUser.Id,
                    Email = user.Email,
                    Avatar = user.Avatar,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber
                });
            }
            else  
                return BadRequest("Tài khoản chưa đăng nhập hoặc là thông tin không hợp lệ"); 

        }

        [Authorize]
        [HttpGet("me")] 
        public async Task<IActionResult> Me()
        {
            var currentUser = await authService.GetUser(User);
            if (currentUser is not null)
                return Ok(new UserResponseDto
                {
                    Email = currentUser.Email,
                    FirstName = currentUser.FirstName,
                    LastName = currentUser.LastName,
                    Avatar = currentUser.Avatar,
                    Id = currentUser.Id,
                    PhoneNumber = currentUser.PhoneNumber
                });
            return Unauthorized("Token không hợp lệ hoặc đã hết hạn");
        }
    
        [Authorize]
        [HttpPost("upload-avatar")]
        public async Task<IActionResult> UploadAvatar(IFormFile file)
        {
            var user = await authService.UploadAvatar(User, file);
            if (user is null)
                return Unauthorized("Chưa đăng nhập");
            return Ok(new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Avatar = user.Avatar
            });
        }

        [Authorize]
        [HttpPost("update-profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto)
        {
            var user = await authService.UpdateProfile(User, dto);
            if (user is null)
                return Unauthorized("Chưa đăng nhập");
            return Ok(new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Avatar = user.Avatar
            });
        }

    }
}
