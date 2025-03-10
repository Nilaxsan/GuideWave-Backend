namespace GuideWave.DTO.Places
{
    public class CreatePlaceDto
    {
        public string PlaceName { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public string[] Availability { get; set; }
    }
}
