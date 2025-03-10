using System.ComponentModel.DataAnnotations;

namespace GuideWave.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        public int Ratings { get; set; }
        public DateTime timestamp { get; set; }

        public string Feedback {  get; set; }

    }
}
