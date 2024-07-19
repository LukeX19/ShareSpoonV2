using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.Tags.Commands;
using ShareSpoon.App.Tags.Responses;
using ShareSpoon.Domain.Enums;
using ShareSpoon.Domain.Models.Recipes;

namespace ShareSpoon.UnitTests.Tags.CommandsTests
{
    public class CreateTagHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<CreateTagHandler>> _loggerMock;
        private readonly CreateTagHandler _handler;

        public CreateTagHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<CreateTagHandler>>();
            _handler = new CreateTagHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_CreateTag_ValidInput_ReturnsCorrectResult()
        {
            // Arrange
            var command = new CreateTag("Dessert", TagType.Course);

            var tag = new Tag
            {
                Id = 1,
                Name = "Dessert",
                Type = TagType.Course
            };
            var tagResponse = new TagResponseDto
            {
                Id = 1,
                Name = "Dessert",
                Type = TagType.Course
            };

            _unitOfWorkMock
                .Setup(u => u.TagRepository.Create(It.IsAny<Tag>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tag);
            _mapperMock
                .Setup(m => m.Map<TagResponseDto>(tag))
                .Returns(tagResponse);

            // Act
            var actualResult = await _handler.Handle(command, default);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Equal(tagResponse.Id, actualResult.Id);
            Assert.Equal(tagResponse.Name, actualResult.Name);
            Assert.Equal(tagResponse.Type, actualResult.Type);
        }
    }
}
