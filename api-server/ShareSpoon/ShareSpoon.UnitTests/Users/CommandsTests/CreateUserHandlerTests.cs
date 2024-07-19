using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.Roles.Responses;
using ShareSpoon.App.Users.Commands;
using ShareSpoon.App.Users.Responses;
using ShareSpoon.Domain.Models.Users;

namespace ShareSpoon.UnitTests.Users.CommandsTests
{
    public class CreateUserHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<CreateUserHandler>> _loggerMock;
        private readonly CreateUserHandler _handler;

        public CreateUserHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<CreateUserHandler>>();
            _handler = new CreateUserHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_CreateUser_ValidInput_CreatesUser()
        {
            // Arrange
            var command = new RegisterUser(1, "John", "Doe", "john.doe@example.com",
                "password12345", new DateTime(1990, 1, 1), "example.url/image");

            var role = new Role
            {
                Id = 1,
                Name = "TestRole"
            };
            var user = new User
            {
                RoleId = command.RoleId,
                Role = role,
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                Password = command.Password,
                Birthday = command.Birthday,
                PictureURL = command.PictureURL
            };
            var userResponse = new UserResponseDto
            {
                Id = 1,
                RoleId = command.RoleId,
                Role = new RoleResponseDto
                {
                    Id = role.Id,
                    Name = role.Name
                },
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                Birthday = command.Birthday,
                PictureURL = command.PictureURL
            };

            _unitOfWorkMock
                .Setup(u => u.RoleRepository.GetById(command.RoleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(role);

            _unitOfWorkMock
                .Setup(u => u.UserRepository.Create(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _mapperMock
                .Setup(m => m.Map<UserResponseDto>(It.IsAny<User>()))
                .Returns(userResponse);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userResponse.Id, result.Id);
            Assert.Equal(userResponse.RoleId, result.RoleId);
            Assert.Equal(userResponse.Role.Id, result.Role.Id);
            Assert.Equal(userResponse.Role.Name, result.Role.Name);
            Assert.Equal(userResponse.FirstName, result.FirstName);
            Assert.Equal(userResponse.LastName, result.LastName);
            Assert.Equal(userResponse.Email, result.Email);
            Assert.Equal(userResponse.Birthday, result.Birthday);
            Assert.Equal(userResponse.PictureURL, result.PictureURL);
        }
    }
}
