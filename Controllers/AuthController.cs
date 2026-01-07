using System.IdentityModel.Tokens.Jwt;
using EnglishCenter.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost] 
        public async Task<IActionResult> Login(string email, string password)
        {
            var token = new JwtSecurityTokenHandler().WriteToken(await authService.Login(email, password));
            return Ok(new
            {
                Token = token,
            });
        }
    }
}
