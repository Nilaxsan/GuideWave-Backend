using AutoMapper;
using GuideWave.DTO.General;
using GuideWave.DTO.Guides;
using GuideWave.DTO.Tourists;
using GuideWave.Models;
using GuideWave.Repository;
using GuideWave.Repository.IRepository;
using GuideWave.Services.EmailService;
using GuideWave.Services.JWTService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using static System.Net.WebRequestMethods;

namespace GuideWave.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TouristsController : ControllerBase
    {
        private readonly ITouristsRepository _touristsRepository;
        private readonly IEmailService _emailService;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public TouristsController(ITouristsRepository touristsRepository, IMapper mapper,IEmailService emailService, IJwtService jwtService)
        {
            _touristsRepository = touristsRepository;
            _mapper = mapper;
            _emailService = emailService;
            _jwtService = jwtService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<ActionResult<IEnumerable<TouristsDto>>> GetAll()
        {
            var tourists = await _touristsRepository.GetAll();

            var touristsDto = _mapper.Map<List<TouristsDto>>(tourists);

            if (touristsDto == null)
            {
                return NoContent();
            }

            return Ok(touristsDto);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<TouristsDto>> GetById(int id)
        {


            var tourists = await _touristsRepository.Get(id);


            var touristsDto = _mapper.Map<TouristsDto>(tourists);
            if (tourists == null)
            {
                return NoContent();
            }
            return Ok(touristsDto);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]

        public async Task<ActionResult<CreateTouristsDto>> Create([FromBody] CreateTouristsDto createtouristsDto)
        {
            var result = _touristsRepository.IsRecordsExists(x => x.Email == createtouristsDto.Email);
            if (result)
            {

                return Conflict("Tourist with this email already exists ");
            }


            var tourist = _mapper.Map<Tourists>(createtouristsDto);
            var passwordHasher = new PasswordHasher<Tourists>();
            tourist.Password = passwordHasher.HashPassword(tourist, createtouristsDto.Password);

            tourist.EmailVerificationToken = Guid.NewGuid().ToString();

            var verificationLink = $"https://localhost:7015/api/Tourists/verify-email?token={Uri.EscapeDataString(tourist.EmailVerificationToken)}&email={Uri.EscapeDataString(tourist.Email)}";

            string emailBody = $@"
                            <h2>Welcome to Guide Wave!</h2>
                               <p>Dear {tourist.FullName},</p>
                                <p>Congratulations! Your registration is successful. Please verify your email to activate your account.</p>
          <p>
            <a href='{verificationLink}'
               style='display: inline-block; padding: 10px 20px; background-color: #007BFF; color: white; text-decoration: none; border-radius: 5px;'>
                Verify Email
            </a>
        </p>    <p>We are excited to have you join our community of explorers. You can now discover local guides and unique experiences to make your journeys unforgettable.</p>
    <p>Here are a few things you can do next:</p>
    <ul>
        <li><b>Search for Local Guides:</b> Find experienced guides based on your preferences and destinations.</li>
        <li><b>Make Bookings:</b> Easily book guided tours and experiences directly through the platform.</li>
        <li><b>Leave Feedback:</b> Share your experience and help others by leaving reviews for guides.</li>
    </ul>
    <p>If you have any questions or need assistance, our support team is here to help.</p>
    <p>Welcome to Guide Wave, and happy exploring!</p>
    <p>Best Regards,<br/>The Guide Wave Team</p>
";

            await _emailService.SendEmailAsync(tourist.Email, "Registration Successful - Please Verify Your Email", emailBody);

            await _touristsRepository.Create(tourist);
            return CreatedAtAction("GetById", new { id = tourist.UserId }, tourist);

        }

        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail(string token, string email)
        {


            var tourist = await _touristsRepository.GetByEmail(email);

            if (tourist == null)
            {
                return BadRequest("Tourist not found.");
            }


            if (tourist.EmailVerificationToken != token)
            {
                return BadRequest("Invalid verification token.");
            }

            // Mark as verified
            tourist.IsEmailVerified = true;
            tourist.EmailVerificationToken = null;
            await _touristsRepository.Update(tourist);

            var frontendurl = "http://localhost:3000/register-success";

            return Redirect(frontendurl);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var tourist = await _touristsRepository.GetByEmail(loginDto.Email);
            if (tourist == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            if (!tourist.IsEmailVerified)
            {
                return Unauthorized("Email not verified. Please check your email.");
            }

            var passwordHasher = new PasswordHasher<Tourists>();
            var verificationResult = passwordHasher.VerifyHashedPassword(tourist, tourist.Password, loginDto.Password);

            if (verificationResult != PasswordVerificationResult.Success)
            {
                return Unauthorized("Invalid email or password.");
            }

            var token = _jwtService.GenerateToken_Tourist(tourist);
            return Ok(new { Token = token });
        }
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Tourists>> Update(int id, [FromBody] TouristsDto touristsDto)
        {
            if (touristsDto == null || id != touristsDto.UserId)
            {
                return BadRequest();
            }

            var tourist = _mapper.Map<Tourists>(touristsDto);

            await _touristsRepository.Update(tourist);
            return NoContent();

        }


        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult> DeleteById(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var tourist = await _touristsRepository.Get(id);


            if (tourist == null)
            {
                return NotFound();
            }
            await _touristsRepository.Delete(tourist);
            return NoContent();
        }
    }
}
