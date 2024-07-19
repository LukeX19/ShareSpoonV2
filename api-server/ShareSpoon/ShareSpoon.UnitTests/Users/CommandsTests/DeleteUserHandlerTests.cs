using Microsoft.Extensions.Logging;
using Moq;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.Users.Commands;

namespace ShareSpoon.UnitTests.Users.CommandsTests
{
    public class DeleteUserHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<DeleteUserHandler>> _loggerMock;
        private readonly DeleteUserHandler _handler;

        public DeleteUserHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<DeleteUserHandler>>();
            _handler = new DeleteUserHandler(_unitOfWorkMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_DeleteUser_ValidInput_ReturnsCorrectResult()
        {
            // Arrange
            var command = new DeleteUser(1);

            _unitOfWorkMock
                .Setup(x => x.UserRepository.Delete(It.IsAny<long>(), It.IsAny<CancellationToken>()))
               .Returns(Task.CompletedTask);

            // Act
            var actualResult = await _handler.Handle(command, default);

            // Assert
            _unitOfWorkMock.Verify(x => x.UserRepository.Delete(1, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
