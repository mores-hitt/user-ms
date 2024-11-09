using AutoMapper;
using user_ms.Src.Models;
using user_ms.Src.Protos;

namespace user_ms.Src.Extensions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<Career, CareerDto>();
            CreateMap<UserProgress, UserProgressDto>();
        }
    }
}