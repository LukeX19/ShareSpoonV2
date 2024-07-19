using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.Roles.Queries;
using ShareSpoon.App.Roles.Responses;
using ShareSpoon.Domain.Models.Users;

namespace ShareSpoon.UnitTests.Roles.QueriesTests
{
    public class GetAllRolesHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetAllRolesHandler>> _loggerMock;
        private readonly GetAllRolesHandler _handler;

        public GetAllRolesHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<GetAllRolesHandler>>();
            _handler = new GetAllRolesHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_GetAllRoles_ValidInput_ReturnsCorrectResult()
        {
            // Arrange
            var querry = new GetAllRoles();

            var ingredients = new List<Role>
            {
                new Role
                {
                    Id = 1,
                    Name = "Professional Chef"
                },
                new Role
                {
                    Id = 2,
                    Name = "Home Cook",
                }
            };
            var roleResponses = new List<RoleResponseDto>
            {
                new RoleResponseDto
                {
                    Id = 1,
                    Name = "Professional Chef"
                },
                new RoleResponseDto
                {
                    Id = 2,
                    Name = "Home Cook"
                }
            };

            _unitOfWorkMock
                .Setup(u => u.RoleRepository.GetAll(It.IsAny<CancellationToken>()))
                .ReturnsAsync(ingredients);

            _mapperMock
                .Setup(m => m.Map<List<RoleResponseDto>>(ingredients))
                .Returns(roleResponses);

            // Act
            var actualResult = await _handler.Handle(querry, default);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Equal(roleResponses.Count, actualResult.Count);
            Assert.Equal(roleResponses[0].Id, actualResult[0].Id);
            Assert.Equal(roleResponses[0].Name, actualResult[0].Name);
            Assert.Equal(roleResponses[1].Id, actualResult[1].Id);
            Assert.Equal(roleResponses[1].Name, actualResult[1].Name);
        }
    }
}
