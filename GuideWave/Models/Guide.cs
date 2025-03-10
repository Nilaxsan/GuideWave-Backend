using System.ComponentModel.DataAnnotations;

namespace GuideWave.Models
{
    public class Guide 
    {
        [Key]
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string PhoneNumber { get; set; }
        public string Experience { get; set; }
        public string Location { get; set; }
        public string Profile { get; set; }

    }
}
