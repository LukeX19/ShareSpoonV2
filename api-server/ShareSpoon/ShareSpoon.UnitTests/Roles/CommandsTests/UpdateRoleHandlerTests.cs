using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.Roles.Commands;
using ShareSpoon.App.Roles.Responses;
using ShareSpoon.Domain.Models.Users;

namespace ShareSpoon.UnitTests.Roles.CommandsTests
{
    public class UpdateRoleHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<UpdateRoleHandler>> _loggerMock;
        private readonly UpdateRoleHandler _handler;

        public UpdateRoleHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<UpdateRoleHandler>>();
            _handler = new UpdateRoleHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_UpdateRole_ValidInput_ReturnsCorrectResult()
        {
            // Arrange
            var command = new UpdateRole(1, "Professional Chef from Michelin Stars Restaurant");

            var existingRole = new Role
            {
                Id = 1,
                Name = "Professional Chef"
            };
            var updatedRole = new Role
            {
                Id = 1,
                Name = "Professional Chef from Michelin Stars Restaurant"
            };
            var roleResponse = new RoleResponseDto
            {
                Id = 1,
                Name = "Professional Chef from Michelin Stars Restaurant"
            };

            _unitOfWorkMock
                .Setup(u => u.RoleRepository.GetById(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingRole);

            _unitOfWorkMock
                .Setup(u => u.RoleRepository.Update(existingRole, It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedRole);

            _mapperMock
                .Setup(m => m.Map<RoleResponseDto>(updatedRole))
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
