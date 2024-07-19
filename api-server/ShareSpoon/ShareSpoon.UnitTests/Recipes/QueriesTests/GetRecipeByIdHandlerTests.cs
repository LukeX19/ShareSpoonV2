using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.Recipes.Queries;
using ShareSpoon.App.Recipes.Responses;
using ShareSpoon.Domain.Enums;
using ShareSpoon.Domain.Models.Recipes;

namespace ShareSpoon.UnitTests.Recipes.QueriesTests
{
    public class GetRecipeByIdHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetRecipeByIdHandler>> _loggerMock;
        private readonly GetRecipeByIdHandler _handler;

        public GetRecipeByIdHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<GetRecipeByIdHandler>>();
            _handler = new GetRecipeByIdHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_GetRecipeById_ValidInput_ReturnsCorrectResult()
        {
            // Arrange
            var querry = new GetRecipeById(1);

            var recipe = new Recipe
            {
                Id = 1,
                Name = "Chocolate Cake",
                Description = "Delicious dark chocolate cake",
                EstimatedTime = new TimeSpan(1, 20, 0),
                Difficulty = DifficultyLevel.Medium,
                PictureURL = "example.url/image"
            };
            var recipeResponse = new RecipeResponseDto
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                EstimatedTime = recipe.EstimatedTime,
                Difficulty = recipe.Difficulty,
                PictureURL = recipe.PictureURL
            };

            _unitOfWorkMock
                .Setup(u => u.RecipeRepository.GetRecipeById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(recipe);

            _mapperMock
                .Setup(m => m.Map<RecipeResponseDto>(recipe))
                .Returns(recipeResponse);

            // Act
            var actualResult = await _handler.Handle(querry, default);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Equal(recipeResponse.Id, actualResult.Id);
            Assert.Equal(recipeResponse.Name, actualResult.Name);
            Assert.Equal(recipeResponse.Description, actualResult.Description);
            Assert.Equal(recipeResponse.EstimatedTime, actualResult.EstimatedTime);
            Assert.Equal(recipeResponse.Difficulty, actualResult.Difficulty);
            Assert.Equal(recipeResponse.PictureURL, actualResult.PictureURL);
        }
    }
}
