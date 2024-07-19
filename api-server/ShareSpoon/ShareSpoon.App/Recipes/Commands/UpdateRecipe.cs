using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.RequestModels;
using ShareSpoon.App.ResponseModels;
using ShareSpoon.Domain.Enums;
using ShareSpoon.Domain.Models.Associations;

namespace ShareSpoon.App.Recipes.Commands
{
    public record UpdateRecipe(long RecipeId, string Name, string Description, TimeSpan EstimatedTime, DifficultyLevel Difficulty,
        ICollection<RecipeIngredientRequestDto> Ingredients, ICollection<RecipeTagRequestDto> Tags, string PictureURL) : IRequest<RecipeResponseDto>;

    public class UpdateRecipeHandler : IRequestHandler<UpdateRecipe, RecipeResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateRecipeHandler> _logger;

        public UpdateRecipeHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateRecipeHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<RecipeResponseDto> Handle(UpdateRecipe request, CancellationToken ct)
        {
            var recipe = await _unitOfWork.RecipeRepository.GetRecipeById(request.RecipeId, ct);

            recipe.Name = request.Name;
            recipe.Description = request.Description;
            recipe.EstimatedTime = request.EstimatedTime;
            recipe.Difficulty = request.Difficulty;
            recipe.PictureURL = request.PictureURL;

            recipe.RecipeIngredients.Clear();
            recipe.RecipeTags.Clear();

            var ingredientIds = request.Ingredients.Select(i => i.Id).ToList();
            var ingredients = await _unitOfWork.IngredientRepository.GetIngredientsByIds(ingredientIds, ct);

            var tagIds = request.Tags.Select(t => t.Id).ToList();
            var tags = await _unitOfWork.TagRepository.GetTagsByIds(tagIds, ct);

            recipe.RecipeIngredients = request.Ingredients.Select(x => new RecipeIngredient
            {
                RecipeId = recipe.Id,
                IngredientId = x.Id,
                Ingredient = ingredients.FirstOrDefault(i => i.Id == x.Id),
                Quantity = x.Quantity,
                QuantityType = x.QuantityType
            }).ToList();
            recipe.RecipeTags = request.Tags.Select(x => new RecipeTag
            {
                RecipeId = recipe.Id,
                TagId = x.Id,
                Tag = tags.FirstOrDefault(t => t.Id == x.Id)
            }).ToList();

            var updatedRecipe = await _unitOfWork.RecipeRepository.Update(recipe, ct);

            _logger.LogInformation($"Updated recipe with id {request.RecipeId}");
            return _mapper.Map<RecipeResponseDto>(updatedRecipe);
        }
    }
}
