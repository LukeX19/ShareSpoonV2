using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.Users.Commands;
using ShareSpoon.App.Users.Responses;
using ShareSpoon.Domain.Models.Users;

namespace ShareSpoon.UnitTests.Users.CommandsTests
{
    public class UpdateUserHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<UpdateUserHandler>> _loggerMock;
        private readonly UpdateUserHandler _handler;

        public UpdateUserHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<UpdateUserHandler>>();
            _handler = new UpdateUserHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_UpdateUser_ValidInput_ReturnsCorrectResult()
        {
            // Arrange
            var command = new UpdateUser(1, "John", "Doe", "long_john.doe@email.com",
                "new_password12345", new DateTime(1990, 1, 1), "new-example.url/image");

            var existingUser = new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@email.com",
                Password = "password12345",
                Birthday = new DateTime(1990, 1, 1),
                PictureURL = "example.url/image"
            };
            var updatedUser = new User
            {
                Id = command.Id,
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                Password = command.Password,
                Birthday = command.Birthday,
                PictureURL = command.PictureURL
            };
            var userResponse = new UserResponseDto
            {
                Id = updatedUser.Id,
                FirstName = updatedUser.FirstName,
                LastName = updatedUser.LastName,
                Email = updatedUser.Email,
                Birthday = updatedUser.Birthday,
                PictureURL = updatedUser.PictureURL
            };

            _unitOfWorkMock
                .Setup(u => u.UserRepository.GetUserById(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingUser);

            _unitOfWorkMock
                .Setup(u => u.UserRepository.Update(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedUser);

            _mapperMock
                .Setup(m => m.Map<UserResponseDto>(updatedUser))
                .Returns(userResponse);

            // Act
            var actualResult = await _handler.Handle(command, default);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Equal(userResponse.Id, actualResult.Id);
            Assert.Equal(userResponse.FirstName, actualResult.FirstName);
            Assert.Equal(userResponse.LastName, actualResult.LastName);
            Assert.Equal(userResponse.Email, actualResult.Email);
            Assert.Equal(userResponse.Birthday, actualResult.Birthday);
            Assert.Equal(userResponse.PictureURL, actualResult.PictureURL);
        }
    }
}
