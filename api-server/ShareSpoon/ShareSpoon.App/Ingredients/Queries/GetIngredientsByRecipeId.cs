using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.ResponseModels;

namespace ShareSpoon.App.Ingredients.Queries
{
    public record GetIngredientsByRecipeId(long Id) : IRequest<IEnumerable<CompleteIngredientResponseDto>>;

    public class GetIngredientsByRecipeIdHandler : IRequestHandler<GetIngredientsByRecipeId, IEnumerable<CompleteIngredientResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetIngredientsByRecipeIdHandler> _logger;

        public GetIngredientsByRecipeIdHandler(IUnitOfWork unitOfWork, ILogger<GetIngredientsByRecipeIdHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<CompleteIngredientResponseDto>> Handle(GetIngredientsByRecipeId request, CancellationToken ct)
        {
            var recipe = await _unitOfWork.RecipeRepository.GetRecipeById(request.Id, ct);
            var ingredientsList = new List<CompleteIngredientResponseDto>();

            foreach (var recipeIngredient in recipe.RecipeIngredients)
            {
                var ingredient = await _unitOfWork.IngredientRepository.GetById(recipeIngredient.IngredientId, ct);

                var recipeIngredientDto = new CompleteIngredientResponseDto
                {
                    Id = ingredient.Id,
                    Name = ingredient.Name,
                    Quantity = recipeIngredient.Quantity,
                    QuantityType = recipeIngredient.QuantityType
                };
                ingredientsList.Add(recipeIngredientDto);
            }

            _logger.LogInformation($"Retrieved ingredients for recipe with id {request.Id}");
            return ingredientsList.AsEnumerable();
        }
    }
}
