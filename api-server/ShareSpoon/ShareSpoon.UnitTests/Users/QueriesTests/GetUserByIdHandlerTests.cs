using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.Users.Queries;
using ShareSpoon.App.Users.Responses;
using ShareSpoon.Domain.Models.Users;

namespace ShareSpoon.UnitTests.Users.QueriesTests
{
    public class GetUserByIdHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetUserByIdHandler>> _loggerMock;
        private readonly GetUserByIdHandler _handler;

        public GetUserByIdHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<GetUserByIdHandler>>();
            _handler = new GetUserByIdHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_GetUserById_ValidInput_ReturnsCorrectResult()
        {
            // Arrange
            var querry = new GetUserById(1);

            var user = new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@email.com",
                Password = "password12345",
                Birthday = new DateTime(1990, 1, 1),
                PictureURL = "example.url/image"
            };
            var userResponse = new UserResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                Birthday = user.Birthday,
                PictureURL = user.PictureURL
            };

            _unitOfWorkMock
                .Setup(u => u.UserRepository.GetUserById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _mapperMock
                .Setup(m => m.Map<UserResponseDto>(user))
                .Returns(userResponse);

            // Act
            var actualResult = await _handler.Handle(querry, default);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Equal(userResponse.Id, actualResult.Id);
            Assert.Equal(userResponse.FirstName, actualResult.FirstName);
            Assert.Equal(userResponse.LastName, actualResult.LastName);
            Assert.Equal(userResponse.Email, actualResult.Email);
            Assert.Equal(userResponse.Password, actualResult.Password);
            Assert.Equal(userResponse.Birthday, actualResult.Birthday);
            Assert.Equal(userResponse.PictureURL, actualResult.PictureURL);
        }
    }
}
