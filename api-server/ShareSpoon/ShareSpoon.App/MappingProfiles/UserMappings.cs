using AutoMapper;
using ShareSpoon.App.ResponseModels;
using ShareSpoon.Domain.Models.Users;

namespace ShareSpoon.App.MappingProfiles
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<AppUser, UserResponseDto>();
        }
    }
}
