using AutoMapper;

namespace CityInfo.API.Profiles;

public class PointOfInterestProfile : Profile
{
    public PointOfInterestProfile()
    {
        CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>();
        CreateMap<Models.PointsOfInterestForCreationDto, Entities.PointOfInterest>();
        CreateMap<Models.PointsOfInterestForUpdateDto, Entities.PointOfInterest>();
        CreateMap<Entities.PointOfInterest, Models.PointsOfInterestForUpdateDto>();
    }
}