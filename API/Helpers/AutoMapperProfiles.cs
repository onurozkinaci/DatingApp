using API.Entities;
using AutoMapper;

namespace API;

public class AutoMapperProfiles:Profile
{
    public AutoMapperProfiles()
    {
      CreateMap<AppUser,MemberDto>();
      CreateMap<Photo, PhotoDto>();
    }
}
