using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EnglishCenter.DTO;
using EnglishCenter.Models;
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

        /// <summary>
        /// Đăng nhập tài khoản
        /// </summary>
        /// <param name="email">Tài khoản</param>
        /// <param name="password">Mật khẩu</param>
        /// <returns>Token và thông tin người dùng</returns>
        [HttpPost("login")] 
        public async Task<ActionResult<LoginResponse>> Login(string email, string password)
        {
            var result = await authService.Login(email, password);
            if (result is null)
            {
                return BadRequest("Đăng nhập thất bại");
            }
            return Ok(result);
        }
        /// <summary>
        /// Đăng ký tài khoản mới
        /// </summary>
        /// <remarks>
        /// API dùng để tạo tài khoản người dùng mới.
        /// 
        /// Mặc định:
        /// - Email phải là duy nhất
        /// - Mật khẩu tuân theo rule của Identity
        /// - User sau khi tạo sẽ được gán role STUDENT
        /// </remarks>
        /// <param name="dto">
        /// Thông tin đăng ký:
        /// - Email
        /// - Mật khẩu
        /// - Họ 
        /// - Tên
        /// - Số điện thoại
        /// </param>
        /// <returns>
        /// Thông tin user vừa được tạo
        /// </returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto dto)
        {
            return Ok(await authService.Register(dto));
        }


        /// <summary>
        /// Thay đổi mật khẩu
        /// </summary>
        /// <remarks>
        /// API sử dụng để đổi mật khẩu, yêu cầu phải gửi kèm theo Token khi đăng nhập thì mới đổi được
        /// </remarks>
        /// <param name="newPassword">
        /// Mật khẩu mới
        /// </param>
        /// <param name="currentPassword">
        /// Mật khẩu cũ
        /// </param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("change-password")]
        public async Task<ActionResult<UserResponseDto>> ChangePassword(string newPassword, string currentPassword)
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

        /// <summary>
        /// Lấy thông tin người dùng
        /// </summary>
        /// <remarks>
        /// API sử dụng để lấy thông tin người dùng đang đăng nhập hiện tại, yêu cầu phải gửi kèm theo Token khi đăng nhập thì mới lấy được
        /// </remarks>
        /// <returns></returns>
        [Authorize]
        [HttpGet("me")] 
        public async Task<ActionResult<UserResponseDto>> Me()
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
    
        /// <summary>
        /// Cập nhật avatar
        /// </summary>
        /// <param name="file">File ảnh</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("upload-avatar")]
        public async Task<ActionResult<UserResponseDto>> UploadAvatar(IFormFile file)
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


        /// <summary>
        /// Cập nhật thông tin tài khoản
        /// </summary>
        /// <param name="dto">
        /// Những thông tin thay đổi được
        /// - Họ
        /// - Tên
        /// </param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("update-profile")]
        public async Task<ActionResult<UserResponseDto>> UpdateProfile(UpdateProfileDto dto)
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
