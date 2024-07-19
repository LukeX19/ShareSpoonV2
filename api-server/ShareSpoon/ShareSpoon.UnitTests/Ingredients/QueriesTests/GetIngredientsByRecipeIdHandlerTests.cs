using Microsoft.Extensions.Logging;
using Moq;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.Ingredients.Queries;
using ShareSpoon.App.Ingredients.Response;
using ShareSpoon.Domain.Enums;
using ShareSpoon.Domain.Models.Associations;
using ShareSpoon.Domain.Models.Ingredients;
using ShareSpoon.Domain.Models.Recipes;

namespace ShareSpoon.UnitTests.Ingredients.QueriesTests
{
    public class GetIngredientsByRecipeIdHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<GetIngredientsByRecipeIdHandler>> _loggerMock;
        private readonly GetIngredientsByRecipeIdHandler _handler;

        public GetIngredientsByRecipeIdHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<GetIngredientsByRecipeIdHandler>>();
            _handler = new GetIngredientsByRecipeIdHandler(_unitOfWorkMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_GetAllIngredients__ValidInput_ReturnsCorrectResult()
        {
            // Arrange
            var querry = new GetIngredientsByRecipeId(1);

            var recipe = new Recipe
            {
                Id = 1,
                RecipeIngredients = new List<RecipeIngredient>
                {
                    new RecipeIngredient { IngredientId = 1, Quantity = 100, QuantityType = QuantityType.Grams },
                    new RecipeIngredient { IngredientId = 2, Quantity = 200, QuantityType = QuantityType.Milliliters }
                }
            };
            var ingredients = new List<Ingredient>
            {
                new Ingredient { Id = 1, Name = "Sugar" },
                new Ingredient { Id = 2, Name = "Milk" }
            };
            var ingredientResponses = ingredients.Select(ing => new CompleteIngredientResponseDto
            {
                Id = ing.Id,
                Name = ing.Name,
                Quantity = recipe.RecipeIngredients.FirstOrDefault(ri => ri.IngredientId == ing.Id).Quantity,
                QuantityType = recipe.RecipeIngredients.FirstOrDefault(ri => ri.IngredientId == ing.Id).QuantityType
            }).ToList();

            _unitOfWorkMock
                .Setup(x => x.RecipeRepository.GetRecipeById(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Recipe
                {
                    Id = 1,
                    RecipeIngredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient { IngredientId = 1, Quantity = 1, QuantityType = QuantityType.Grams },
                        new RecipeIngredient { IngredientId = 2, Quantity = 2, QuantityType = QuantityType.Grams }
                    }
                });

            _unitOfWorkMock
                .Setup(u => u.RecipeRepository.GetRecipeById(recipe.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(recipe);

            foreach (var ingredient in ingredients)
            {
                _unitOfWorkMock
                    .Setup(u => u.IngredientRepository.GetById(ingredient.Id, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(ingredient);
            }

            // Act
            var actualResult = (await _handler.Handle(querry, default)).ToList();

            // Assert
            Assert.NotNull(actualResult);
            Assert.Equal(ingredientResponses.Count, actualResult.Count());
            Assert.Equal(ingredientResponses[0].Id, actualResult[0].Id);
            Assert.Equal(ingredientResponses[0].Name, actualResult[0].Name);
            Assert.Equal(ingredientResponses[0].Quantity, actualResult[0].Quantity);
            Assert.Equal(ingredientResponses[0].QuantityType, actualResult[0].QuantityType);
            Assert.Equal(ingredientResponses[1].Id, actualResult[1].Id);
            Assert.Equal(ingredientResponses[1].Name, actualResult[1].Name);
            Assert.Equal(ingredientResponses[1].Quantity, actualResult[1].Quantity);
            Assert.Equal(ingredientResponses[1].QuantityType, actualResult[1].QuantityType);
        }
    }
}
