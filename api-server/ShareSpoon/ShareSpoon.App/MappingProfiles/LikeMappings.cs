using AutoMapper;
using ShareSpoon.App.ResponseModels;
using ShareSpoon.Domain.Models.Interactions;

namespace ShareSpoon.App.MappingProfiles
{
    public class LikeMappings : Profile
    {
        public LikeMappings()
        {
            CreateMap<Like, LikeResponseDto>();
        }
    }
}
