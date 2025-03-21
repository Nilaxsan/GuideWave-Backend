﻿using AutoMapper;
using GuideWave.DTO.Tourists;
using GuideWave.Models;
using GuideWave.Repository.IRepository;
using GuideWave.Services.EmailService;
using Microsoft.AspNetCore.Identity;

namespace GuideWave.Repository
{
    public class AuthRepository : IAuthRepository

    {
        private static string _tempEmailForVerification; // Temporary storage for email

        private readonly IGuideRepository _guidesRepository;
        private readonly ITouristsRepository _touristsRepository;
        private readonly IEmailService _emailService;
        public AuthRepository(ITouristsRepository touristsRepository, IGuideRepository guideRepository, IEmailService emailService, IMapper mapper)
        {

            _touristsRepository = touristsRepository;
            _guidesRepository = guideRepository;
            _emailService = emailService;

        }
        public async Task SendVerificationCodeAsync(string email)
        {
            _tempEmailForVerification = email;

            var guide = await _guidesRepository.GetByEmail(email);
            var tourist = await _touristsRepository.GetByEmail(email);

            if (guide != null)
            {
                // Generate verification otp
                var otp = new Random().Next(100000, 999999).ToString();
                guide.Otp = otp;
                await _guidesRepository.Update(guide);
                string emailBody = $@"
                                <h2> Reset Password</h2>
                                <p>Dear {guide.FullName},</p>
                                 <p>To reset your password. please enter the OTP code below on our website:</p>
                                 <p>
                                 <b>Your OTP Code:</b> 
                                    <h4>{otp}</h4>
                                  </p>
                                  <p>Best Regards,<br/>The Guide Wave Team</p>
";
                await _emailService.SendEmailAsync(guide.Email, " Verify Your OTP", emailBody);
            }
            else if (tourist != null)
            {
                var otp = new Random().Next(100000, 999999).ToString();
                tourist.Otp = otp;
                await _touristsRepository.Update(tourist);
                string emailBody = $@"
                                <h2> Reset Password</h2>
                                <p>Dear {tourist.FullName},</p>
                                 <p>To reset your password. please enter the OTP code below on our website:</p>
                                 <p>
                                 <b>Your OTP Code:</b> 
                                    <h4>{otp}</h4>
                                  </p>
                                  <p>Best Regards,<br/>The Guide Wave Team</p>" ;            
                                await _emailService.SendEmailAsync(tourist.Email, "Verify Your OTP", emailBody);
            }
            else
            {
                throw new Exception("Email not found");
            }
        }
        public async Task<bool> VerifyOtpAsync(string otp)
        {
            var email = _tempEmailForVerification; // Use the stored email
            var guide =await _guidesRepository.GetByEmail(email);
            var tourist =await _touristsRepository.GetByEmail(email);

            if (guide != null && guide.Otp == otp)
            {
                return true;
            }
            else if (tourist != null && tourist.Otp == otp)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> ResetPasswordAsync(string newPassword)
        {
            var email = _tempEmailForVerification; // Use the stored email
            var guide =  await _guidesRepository.GetByEmail(email);
            var tourist = await _touristsRepository.GetByEmail(email);

            if (guide != null)
            {
                var passwordHasher = new PasswordHasher<Guide>();
                guide.Password = passwordHasher.HashPassword(guide,newPassword);
                guide.Otp = null; // Clear verification code after reset
                await _guidesRepository.Update(guide);
                return true;
            }
            else if (tourist != null)
            {
                var passwordHasher = new PasswordHasher<Tourists>();
                tourist.Password = passwordHasher.HashPassword(tourist,newPassword);
                tourist.Otp = null; // Clear verification code after reset
                await _touristsRepository.Update(tourist);
                return true;
            }

            return false; // If no user matched
        }
    }
}
