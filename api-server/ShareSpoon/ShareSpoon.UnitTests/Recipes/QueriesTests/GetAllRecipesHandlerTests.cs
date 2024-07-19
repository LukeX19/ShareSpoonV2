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
    public class GetAllRecipesHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetAllRecipesHandler>> _loggerMock;
        private readonly GetAllRecipesHandler _handler;

        public GetAllRecipesHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<GetAllRecipesHandler>>();
            _handler = new GetAllRecipesHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_GetAllRecipes_ValidInput_ReturnsCorrectResult()
        {
            // Arrange
            var querry = new GetAllRecipes();

            var recipes = new List<Recipe>
            {
                new Recipe
                {
                    Id = 1,
                    Name = "Chocolate Cake",
                    Description = "Delicious dark chocolate cake",
                    EstimatedTime = new TimeSpan(1, 20, 0),
                    Difficulty = DifficultyLevel.Medium,
                    PictureURL = "example.url/image1"
                },
                new Recipe
                {
                    Id = 2,
                    Name = "Vanilla Ice Cream",
                    Description = "Creamy vanilla ice cream",
                    EstimatedTime = new TimeSpan(0, 45, 0),
                    Difficulty = DifficultyLevel.Easy,
                    PictureURL = "example.url/image2"
                }
            };
            var recipeResponses = new List<RecipeResponseDto>
            {
                new RecipeResponseDto
                {
                    Id = 1,
                    Name = "Chocolate Cake",
                    Description = "Delicious dark chocolate cake",
                    EstimatedTime = new TimeSpan(1, 20, 0),
                    Difficulty = DifficultyLevel.Medium,
                    PictureURL = "example.url/image1"
                },
                new RecipeResponseDto
                {
                    Id = 2,
                    Name = "Vanilla Ice Cream",
                    Description = "Creamy vanilla ice cream",
                    EstimatedTime = new TimeSpan(0, 45, 0),
                    Difficulty = DifficultyLevel.Easy,
                    PictureURL = "example.url/image2"
                }
            };

            _unitOfWorkMock
                .Setup(u => u.RecipeRepository.GetAllRecipes(It.IsAny<CancellationToken>()))
                .ReturnsAsync(recipes);

            _mapperMock
                .Setup(m => m.Map<List<RecipeResponseDto>>(recipes))
                .Returns(recipeResponses);

            // Act
            var actualResult = await _handler.Handle(querry, default);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Equal(recipeResponses.Count, actualResult.Count);
            Assert.Equal(recipeResponses[0].Id, actualResult[0].Id);
            Assert.Equal(recipeResponses[0].Name, actualResult[0].Name);
            Assert.Equal(recipeResponses[0].Description, actualResult[0].Description);
            Assert.Equal(recipeResponses[0].EstimatedTime, actualResult[0].EstimatedTime);
            Assert.Equal(recipeResponses[0].Difficulty, actualResult[0].Difficulty);
            Assert.Equal(recipeResponses[0].PictureURL, actualResult[0].PictureURL);
            Assert.Equal(recipeResponses[1].Id, actualResult[1].Id);
            Assert.Equal(recipeResponses[1].Name, actualResult[1].Name);
            Assert.Equal(recipeResponses[1].Description, actualResult[1].Description);
            Assert.Equal(recipeResponses[1].EstimatedTime, actualResult[1].EstimatedTime);
            Assert.Equal(recipeResponses[1].Difficulty, actualResult[1].Difficulty);
            Assert.Equal(recipeResponses[1].PictureURL, actualResult[1].PictureURL);
        }
    }
}
