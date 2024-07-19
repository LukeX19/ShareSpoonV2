using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.Ingredients.Commands;
using ShareSpoon.App.Ingredients.Response;
using ShareSpoon.Domain.Models.Ingredients;

namespace ShareSpoon.UnitTests.Ingredients.CommandsTests
{
    public class UpdateIngredientHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<UpdateIngredientHandler>> _loggerMock;
        private readonly UpdateIngredientHandler _handler;

        public UpdateIngredientHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<UpdateIngredientHandler>>();
            _handler = new UpdateIngredientHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_UpdateIngredient_ValidInput_ReturnsCorrectResult()
        {
            // Arrange
            var command = new UpdateIngredient(1, "Sweeter Sugar");

            var existingIngredient = new Ingredient
            {
                Id = 1,
                Name = "Sugar"
            };
            var updatedIngredient = new Ingredient
            {
                Id = 1,
                Name = "Refined Sugar"
            };
            var ingredientResponse = new IngredientResponseDto
            {
                Id = 1,
                Name = "Refined Sugar"
            };

            _unitOfWorkMock
                .Setup(u => u.IngredientRepository.GetById(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingIngredient);

            _unitOfWorkMock
                .Setup(u => u.IngredientRepository.Update(existingIngredient, It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedIngredient);

            _mapperMock
                .Setup(m => m.Map<IngredientResponseDto>(updatedIngredient))
                .Returns(ingredientResponse);

            // Act
            var actualResult = await _handler.Handle(command, default);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Equal(ingredientResponse.Id, actualResult.Id);
            Assert.Equal(ingredientResponse.Name, actualResult.Name);
        }
    }
}
