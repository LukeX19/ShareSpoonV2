using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.Roles.Commands;
using ShareSpoon.App.Roles.Responses;
using ShareSpoon.Domain.Models.Users;

namespace ShareSpoon.UnitTests.Roles.CommandsTests
{
    public class CreateRoleHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<CreateRoleHandler>> _loggerMock;
        private readonly CreateRoleHandler _handler;

        public CreateRoleHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<CreateRoleHandler>>();
            _handler = new CreateRoleHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_CreateRole_ValidInput_ReturnsCorrectResult()
        {
            // Arrange
            var command = new CreateRole("Professional Chef");

            var role = new Role
            {
                Id = 1,
                Name = "Professional Chef"
            };
            var roleResponse = new RoleResponseDto
            {
                Id = 1,
                Name = "Professional Chef"
            };

            _unitOfWorkMock
                .Setup(u => u.RoleRepository.Create(It.IsAny<Role>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(role);
            _mapperMock
                .Setup(m => m.Map<RoleResponseDto>(role))
                .Returns(roleResponse);

            // Act
            var actualResult = await _handler.Handle(command, default);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Equal(roleResponse.Id, actualResult.Id);
            Assert.Equal(roleResponse.Name, actualResult.Name);
        }
    }
}
