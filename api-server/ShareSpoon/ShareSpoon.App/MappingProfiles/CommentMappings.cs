using AutoMapper;
using ShareSpoon.App.ResponseModels;
using ShareSpoon.Domain.Models.Interactions;

namespace ShareSpoon.App.MappingProfiles
{
    public class CommentMappings : Profile
    {
        public CommentMappings()
        {
            CreateMap<Comment, CommentResponseDto>();
        }
    }
}
