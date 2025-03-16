using AutoMapper;
using GuideWave.DTO.General;
using GuideWave.DTO.Guides;
using GuideWave.Models;
using GuideWave.Repository;
using GuideWave.Repository.IRepository;
using GuideWave.Services.EmailService;
using GuideWave.Services.JWTService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GuideWave.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuideController : ControllerBase
    {
        private readonly IGuideRepository _guidesRepository;
        private readonly IEmailService _emailService;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public GuideController(IGuideRepository guidesRepository, IMapper mapper , IEmailService emailService,IJwtService jwtService)
        {
            _guidesRepository = guidesRepository;
            _mapper = mapper;
            _emailService = emailService;
            _jwtService = jwtService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<ActionResult<IEnumerable<GuidesDto>>> GetAll()
        {
            var guides = await _guidesRepository.GetAll();

            var guidesDto = _mapper.Map<List<GuidesDto>>(guides);

            if (guidesDto == null)
            {
                return NoContent();
            }

            return Ok(guidesDto);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<GuidesDto>> GetById(int id)
        {


            var guides = await _guidesRepository.Get(id);


            var guidesDto = _mapper.Map<GuidesDto>(guides);
            if (guides == null)
            {
                return NoContent();
            }
            return Ok(guidesDto);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]

        public async Task<ActionResult<CreateGuidesDto>> Create([FromBody] CreateGuidesDto createguidesDto)
        {
            var result = _guidesRepository.IsRecordsExists(x => x.Email == createguidesDto.Email);
            if (result)
            {

                return Conflict("Guide with this Email already exists");
            }


            var guide = _mapper.Map<Guide>(createguidesDto);
            var passwordHasher = new PasswordHasher<Guide>();
            guide.Password = passwordHasher.HashPassword(guide, createguidesDto.Password);

            guide.EmailVerificationToken = Guid.NewGuid().ToString();

            var verificationLink = $"https://localhost:7015/api/Guide/verify-email?token={Uri.EscapeDataString(guide.EmailVerificationToken)}&email={Uri.EscapeDataString(guide.Email)}";

            string emailBody = $@"
    <h2>Welcome to Guide Wave!</h2>
    <p>Dear {guide.FullName},</p>
  <p>Congratulations! Your registration is successful. Please verify your email to activate your account.</p>
          <p>
            <a href='{verificationLink}'
               style='display: inline-block; padding: 10px 20px; background-color: #007BFF; color: white; text-decoration: none; border-radius: 5px;'>
                Verify Email
            </a>
        </p>    <p>We are thrilled to have you join our community of local guides. Your profile is now live, and tourists can connect with you to explore unique experiences and discover new places.</p>
    <p>Here are a few things you can do next:</p>
    <ul>
        <li><b>Update Your Profile:</b> Add more details about your services, areas of expertise, and availability.</li>
        <li><b>Manage Bookings:</b> Stay on top of your bookings and respond to requests promptly.</li>
        <li><b>Earn Positive Reviews:</b> Provide exceptional service to receive great feedback from tourists.</li>
    </ul>
    <p>If you have any questions or need assistance, feel free to reach out to our support team.</p>
    <p>Welcome aboard, and we wish you great success with Guide Wave!</p>
    <p>Best Regards,<br/>The Guide Wave Team</p>
";
            await _emailService.SendEmailAsync(guide.Email, "Registration Successfull- Please Verify Your Email", emailBody);

            await _guidesRepository.Create(guide);
            return CreatedAtAction("GetById", new { id = guide.UserId }, guide);


        }

        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail(string token, string email)
        {


            var guide = await _guidesRepository.GetByEmail(email);

            if (guide == null)
            {
                return BadRequest("Tourist not found.");
            }


            if (guide.EmailVerificationToken != token)
            {
                return BadRequest("Invalid verification token.");
            }

            // Mark as verified
            guide.IsEmailVerified = true;
            guide.EmailVerificationToken = null;
            await _guidesRepository.Update(guide);

            var frontendurl = "http://localhost:3000/register-success";

            return Redirect(frontendurl);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var guide = await _guidesRepository.GetByEmail(loginDto.Email);
            if (guide == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            if (!guide.IsEmailVerified)
            {
                return Unauthorized("Email not verified. Please check your email.");
            }

            var passwordHasher = new PasswordHasher<Guide>();
            var verificationResult = passwordHasher.VerifyHashedPassword(guide, guide.Password, loginDto.Password);

            if (verificationResult != PasswordVerificationResult.Success)
            {
                return Unauthorized("Invalid email or password.");
            }

            var token = _jwtService.GenerateToken_Guide(guide);
            return Ok(new { Token = token });
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Guide>> Update(int id, [FromBody] GuidesDto guidesDto)
        {
            if (guidesDto == null || id != guidesDto.UserId)
            {
                return BadRequest();
            }

            var guide = _mapper.Map<Guide>(guidesDto);

            await _guidesRepository.Update(guide);
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

            var guide = await _guidesRepository.Get(id);


            if (guide == null)
            {
                return NotFound();
            }
            await _guidesRepository.Delete(guide);
            return NoContent();
        }
    }
}
