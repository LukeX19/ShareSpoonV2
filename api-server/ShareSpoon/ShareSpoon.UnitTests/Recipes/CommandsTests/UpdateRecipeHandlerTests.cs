using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.Recipes.Commands;
using ShareSpoon.App.Recipes.Responses;
using ShareSpoon.Domain.Enums;
using ShareSpoon.Domain.Models.Recipes;

namespace ShareSpoon.UnitTests.Recipes.CommandsTests
{
    public class UpdateRecipeHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<UpdateRecipeHandler>> _loggerMock;
        private readonly UpdateRecipeHandler _handler;

        public UpdateRecipeHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<UpdateRecipeHandler>>();
            _handler = new UpdateRecipeHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_UpdateRecipe_ValidInput_ReturnsCorrectResult()
        {
            // Arrange
            var command = new UpdateRecipe(1, "Updated Chocolate Cake", "New description of the delicious cake",
                new TimeSpan(1, 45, 0), DifficultyLevel.Hard, "new-example.url/image");

            var existingRecipe = new Recipe
            {
                Id = 1,
                Name = "Chocolate Cake",
                Description = "Delicious dark chocolate cake",
                EstimatedTime = TimeSpan.FromHours(1),
                Difficulty = DifficultyLevel.Medium,
                PictureURL = "old-example.url/image"
            };

            var updatedRecipe = new Recipe
            {
                Id = command.Id,
                Name = command.Name,
                Description = command.Description,
                EstimatedTime = command.EstimatedTime,
                Difficulty = command.Difficulty,
                PictureURL = command.PictureURL
            };

            var recipeResponse = new RecipeResponseDto
            {
                Id = updatedRecipe.Id,
                Name = updatedRecipe.Name,
                Description = updatedRecipe.Description,
                EstimatedTime = updatedRecipe.EstimatedTime,
                Difficulty = updatedRecipe.Difficulty,
                PictureURL = updatedRecipe.PictureURL
            };

            _unitOfWorkMock
                .Setup(x => x.RecipeRepository.GetRecipeById(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingRecipe);

            _unitOfWorkMock
                .Setup(x => x.RecipeRepository.Update(existingRecipe, It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedRecipe);

            _mapperMock
                .Setup(x => x.Map<RecipeResponseDto>(updatedRecipe))
                .Returns(recipeResponse);

            // Act
            var actualResult = await _handler.Handle(command, default);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Equal(updatedRecipe.Id, actualResult.Id);
            Assert.Equal(updatedRecipe.Name, actualResult.Name);
            Assert.Equal(updatedRecipe.Description, actualResult.Description);
            Assert.Equal(updatedRecipe.EstimatedTime, actualResult.EstimatedTime);
            Assert.Equal(updatedRecipe.Difficulty, actualResult.Difficulty);
            Assert.Equal(updatedRecipe.PictureURL, actualResult.PictureURL);
        }
    }
}
