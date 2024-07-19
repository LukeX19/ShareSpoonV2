using AutoMapper;
using ShareSpoon.App.ResponseModels;
using ShareSpoon.Domain.Models.Recipes;

namespace ShareSpoon.App.MappingProfiles
{
    public class TagMappings : Profile
    {
        public TagMappings()
        {
            CreateMap<Tag, TagResponseDto>();
        }
    }
}
