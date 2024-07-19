using AutoMapper;
using ShareSpoon.App.ResponseModels;
using ShareSpoon.Domain.Models.Ingredients;

namespace ShareSpoon.App.MappingProfiles
{
    public class IngredientMappings : Profile
    {
        public IngredientMappings()
        {
            CreateMap<Ingredient, IngredientResponseDto>();
        }
    }
}
