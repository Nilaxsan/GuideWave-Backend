namespace GuideWave.DTO.Review
{
    public class ReviewDto
    {
        public int ReviewId { get; set; }
        public int Ratings { get; set; }
        public DateTime timestamp { get; set; }

        public string Feedback { get; set; }
    }
}
