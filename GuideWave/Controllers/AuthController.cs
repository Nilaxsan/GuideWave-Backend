using AutoMapper;
using GuideWave.DTO.General;
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
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto fprequest)
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
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequestDto request)
        {

            var result = await _authRepository.VerifyOtpAsync(request.Otp);
            if (result)
            {
                return Ok(new { message = "OTP verified successfully" });
            }

            return BadRequest(new { message = "Invalid OTP or user not found" });
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
        {

            var result = await _authRepository.ResetPasswordAsync(request.NewPassword);
            if (result)
            {
                return Ok(new { message = "Password reset successful" });
            }

            return BadRequest(new { message = "User not found or password reset failed" });
        }

    }
}
