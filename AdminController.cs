using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace TSZHApp2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("VerifyPassword")]
        public IActionResult VerifyPassword([FromForm] string password)
        {
            var adminPassword = _configuration["Admin:Password"];
            if (password == adminPassword)
            {
                return Ok(new { success = true });
            }
            return Unauthorized(new { success = false, message = "Неверный пароль" });
        }
    }
}
