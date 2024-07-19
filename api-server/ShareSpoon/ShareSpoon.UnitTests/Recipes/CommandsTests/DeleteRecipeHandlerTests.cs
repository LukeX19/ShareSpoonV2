using Microsoft.Extensions.Logging;
using Moq;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.Recipes.Commands;

namespace ShareSpoon.UnitTests.Recipes.CommandsTests
{
    public class DeleteRecipeHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<DeleteRecipeHandler>> _loggerMock;
        private readonly DeleteRecipeHandler _handler;

        public DeleteRecipeHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<DeleteRecipeHandler>>();
            _handler = new DeleteRecipeHandler(_unitOfWorkMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_DeleteRecipe_ValidInput_ReturnsCorrectResult()
        {
            // Arrange
            var command = new DeleteRecipe(1);

            _unitOfWorkMock
                .Setup(x => x.RecipeRepository.Delete(It.IsAny<long>(), It.IsAny<CancellationToken>()))
               .Returns(Task.CompletedTask);

            // Act
            var actualResult = await _handler.Handle(command, default);

            // Assert
            _unitOfWorkMock.Verify(x => x.RecipeRepository.Delete(1, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
