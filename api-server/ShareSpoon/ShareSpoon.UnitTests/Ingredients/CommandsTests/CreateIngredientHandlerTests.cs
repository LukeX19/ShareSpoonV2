using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.Ingredients.Commands;
using ShareSpoon.App.Ingredients.Response;
using ShareSpoon.Domain.Models.Ingredients;

namespace ShareSpoon.UnitTests.Ingredients.CommandsTests
{
    public class CreateIngredientHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<CreateIngredientHandler>> _loggerMock;
        private readonly CreateIngredientHandler _handler;

        public CreateIngredientHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<CreateIngredientHandler>>();
            _handler = new CreateIngredientHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_CreateIngredient_ValidInput_ReturnsCorrectResult()
        {
            // Arrange
            var command = new CreateIngredient("Sugar");

            var ingredient = new Ingredient
            {
                Id = 1,
                Name = "Sugar"
            };
            var ingredientResponse = new IngredientResponseDto
            {
                Id = 1,
                Name = "Sugar"
            };

            _unitOfWorkMock
                .Setup(u => u.IngredientRepository.Create(It.IsAny<Ingredient>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ingredient);
            _mapperMock
                .Setup(m => m.Map<IngredientResponseDto>(ingredient))
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
