using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.Exceptions;
using ShareSpoon.App.RequestModels;
using ShareSpoon.App.ResponseModels;
using ShareSpoon.Domain.Enums;
using ShareSpoon.Domain.Models.Associations;
using ShareSpoon.Domain.Models.Recipes;

namespace ShareSpoon.App.Recipes.Commands
{
    public record CreateRecipe(string UserId, string Name, string Description, TimeSpan EstimatedTime, DifficultyLevel Difficulty,
        ICollection<RecipeIngredientRequestDto> Ingredients, ICollection<RecipeTagRequestDto> Tags, string PictureURL) : IRequest<RecipeResponseDto>;

    public class CreateRecipeHandler : IRequestHandler<CreateRecipe, RecipeResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateRecipeHandler> _logger;

        public CreateRecipeHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateRecipeHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<RecipeResponseDto> Handle(CreateRecipe request, CancellationToken ct)
        {
            if (!_unitOfWork.IngredientRepository.EntitiesExist(request.Ingredients.Select(item => item.Id)))
            {
                throw new EmptyIngredientsListException();
            }

            if (!_unitOfWork.TagRepository.EntitiesExist(request.Tags.Select(item => item.Id)))
            {
                throw new EmptyTagsListException();
            }

            var user = await _unitOfWork.UserRepository.GetUserById(request.UserId, ct);

            var ingredientIds = request.Ingredients.Select(i => i.Id).ToList();
            var ingredients = await _unitOfWork.IngredientRepository.GetIngredientsByIds(ingredientIds, ct);

            var tagIds = request.Tags.Select(t => t.Id).ToList();
            var tags = await _unitOfWork.TagRepository.GetTagsByIds(tagIds, ct);

            var recipe = new Recipe()
            {
                UserId = request.UserId,
                User = user,
                Name = request.Name,
                Description = request.Description,
                EstimatedTime = request.EstimatedTime,
                Difficulty = request.Difficulty,
                RecipeIngredients = request.Ingredients.Select(x => new RecipeIngredient
                {
                    IngredientId = x.Id,
                    Ingredient = ingredients.FirstOrDefault(i => i.Id == x.Id),
                    Quantity = x.Quantity,
                    QuantityType = x.QuantityType
                }).ToList(),
                RecipeTags = request.Tags.Select(x => new RecipeTag
                {
                    TagId = x.Id,
                    Tag = tags.FirstOrDefault(t => t.Id == x.Id)
                }).ToList(),
                CreatedAt = DateTime.UtcNow,
                PictureURL = request.PictureURL
            };

            var createdRecipe = await _unitOfWork.RecipeRepository.Create(recipe, ct);

            _logger.LogInformation("Created new recipe");
            return _mapper.Map<RecipeResponseDto>(createdRecipe);
        }
    }
}
