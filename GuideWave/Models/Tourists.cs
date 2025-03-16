using System.ComponentModel.DataAnnotations;

namespace GuideWave.Models
{
    public class Tourists
    {
        [Key]
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string Profile { get; set; }
        public string Otp { get; set; }

        public bool IsEmailVerified { get; set; } = false;
        public string? EmailVerificationToken { get; set; }
    }
}
