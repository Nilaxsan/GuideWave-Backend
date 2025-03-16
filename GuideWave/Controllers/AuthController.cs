using AutoMapper;
using GuideWave.Repository.IRepository;
using GuideWave.Services.EmailService;
using GuideWave.Services.JWTService;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace GuideWave.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
       private readonly IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest fprequest)
        {


            try
            {
                await _authRepository.SendVerificationCodeAsync(fprequest.Email);



                return Ok(new { message = "Verification code sent to email" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }

        }

    }
}
