using API.DTOS;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API;

public class AutoMapperProfiles:Profile
{
    public AutoMapperProfiles()
    {
      CreateMap<AppUser,MemberDto>()
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src=>src.Photos.FirstOrDefault(x=>x.IsMain).Url)) //individual mapping for an individual property
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge())); //*Defined here since the GetAge() method inside the AppUser cause whole entities fetch to compare age props between the main class and Dto although the mapping between these classes is defined with "ProjectTo()" at the UserRepository. Thus, we assign the value for the age prop of Dto here explicitly.
      CreateMap<Photo, PhotoDto>();
      CreateMap<MemberUpdateDto,AppUser>();
    }
}
