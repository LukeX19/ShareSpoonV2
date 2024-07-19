using AutoMapper;
using ShareSpoon.App.ResponseModels;
using ShareSpoon.Domain.Models.Associations;
using ShareSpoon.Domain.Models.Recipes;

namespace ShareSpoon.App.MappingProfiles
{
    public class RecipeMappings : Profile
    {
        public RecipeMappings()
        {
            CreateMap<RecipeIngredient, RecipeIngredientResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IngredientId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Ingredient.Name))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.QuantityType, opt => opt.MapFrom(src => src.QuantityType));
            CreateMap<RecipeTag, RecipeTagResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TagId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Tag.Name))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Tag.Type));
            CreateMap<Recipe, RecipeResponseDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.RecipeIngredients, opt => opt.MapFrom(src => src.RecipeIngredients))
                .ForMember(dest => dest.RecipeTags, opt => opt.MapFrom(src => src.RecipeTags));
        }
    }
}
