using AutoMapper;
using GuideWave.DTO.Guides;
using GuideWave.DTO.Places;
using GuideWave.DTO.Review;
using GuideWave.DTO.Tourists;
using GuideWave.Models;
using System.Diagnostics.Metrics;

namespace GuideWave.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Guide mapping
            CreateMap<Guide, CreateGuidesDto>().ReverseMap();
            CreateMap<Guide, GuidesDto>().ReverseMap();

            //Tourist mapping
            CreateMap<Tourists, CreateTouristsDto>().ReverseMap();
            CreateMap<Tourists, TouristsDto>().ReverseMap();

            //Place mapping
            CreateMap<Place, CreatePlaceDto>().ReverseMap();
            CreateMap<Place, PlaceDto>().ReverseMap();

            //Review mapping
            CreateMap<Review, CreateReviewDto>().ReverseMap();
            CreateMap<Review, ReviewDto>().ReverseMap();

           


         
        }
    }
}
