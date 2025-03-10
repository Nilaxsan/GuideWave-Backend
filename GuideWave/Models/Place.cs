using System.ComponentModel.DataAnnotations;

namespace GuideWave.Models
{
    public class Place
    {
        [Key]
        public int PlaceId { get; set; }
        public string PlaceName { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public string[] Availability {  get; set; }
        

    }
}
