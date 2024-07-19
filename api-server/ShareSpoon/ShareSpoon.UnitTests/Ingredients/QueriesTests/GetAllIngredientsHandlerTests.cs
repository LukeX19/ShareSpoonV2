using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.Ingredients.Queries;
using ShareSpoon.App.Ingredients.Response;
using ShareSpoon.Domain.Models.Ingredients;

namespace ShareSpoon.UnitTests.Ingredients.QueriesTests
{
    public class GetAllIngredientsHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetAllIngredientsHandler>> _loggerMock;
        private readonly GetAllIngredientsHandler _handler;

        public GetAllIngredientsHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<GetAllIngredientsHandler>>();
            _handler = new GetAllIngredientsHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_GetAllIngredients_ValidInput_ReturnsCorrectResult()
        {
            // Arrange
            var querry = new GetAllIngredients();

            var ingredients = new List<Ingredient>
            {
                new Ingredient
                {
                    Id = 1,
                    Name = "Sugar"
                },
                new Ingredient
                {
                    Id = 2,
                    Name = "Milk",
                }
            };
            var ingredientResponses = new List<IngredientResponseDto>
            {
                new IngredientResponseDto
                {
                    Id = 1,
                    Name = "Sugar"
                },
                new IngredientResponseDto
                {
                    Id = 2,
                    Name = "Milk"
                }
            };

            _unitOfWorkMock
                .Setup(u => u.IngredientRepository.GetAll(It.IsAny<CancellationToken>()))
                .ReturnsAsync(ingredients);

            _mapperMock
                .Setup(m => m.Map<List<IngredientResponseDto>>(ingredients))
                .Returns(ingredientResponses);

            // Act
            var actualResult = await _handler.Handle(querry, default);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Equal(ingredientResponses.Count, actualResult.Count);
            Assert.Equal(ingredientResponses[0].Id, actualResult[0].Id);
            Assert.Equal(ingredientResponses[0].Name, actualResult[0].Name);
            Assert.Equal(ingredientResponses[1].Id, actualResult[1].Id);
            Assert.Equal(ingredientResponses[1].Name, actualResult[1].Name);
        }
    }
}
