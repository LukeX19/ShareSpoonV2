using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.Tags.Queries;
using ShareSpoon.App.Tags.Responses;
using ShareSpoon.Domain.Enums;
using ShareSpoon.Domain.Models.Recipes;

namespace ShareSpoon.UnitTests.Tags.QueriesTests
{
    public class GetAllTagsHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetAllTagsHandler>> _loggerMock;
        private readonly GetAllTagsHandler _handler;

        public GetAllTagsHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<GetAllTagsHandler>>();
            _handler = new GetAllTagsHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_GetAllTags_ValidInput_ReturnsCorrectResult()
        {
            // Arrange
            var querry = new GetAllTags();

            var tags = new List<Tag>
            {
                new Tag
                {
                    Id = 1,
                    Name = "Dessert",
                    Type = TagType.Course
                },
                new Tag
                {
                    Id = 2,
                    Name = "Italian",
                    Type = TagType.Cuisine
                }
            };
            var tagResponses = new List<TagResponseDto>
            {
                new TagResponseDto
                {
                    Id = 1,
                    Name = "Dessert",
                    Type = TagType.Course
                },
                new TagResponseDto
                {
                    Id = 2,
                    Name = "Italian",
                    Type = TagType.Cuisine
                }
            };

            _unitOfWorkMock
                .Setup(u => u.TagRepository.GetAll(It.IsAny<CancellationToken>()))
                .ReturnsAsync(tags);

            _mapperMock
                .Setup(m => m.Map<List<TagResponseDto>>(tags))
                .Returns(tagResponses);

            // Act
            var actualResult = await _handler.Handle(querry, default);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Equal(tagResponses.Count, actualResult.Count);
            Assert.Equal(tagResponses[0].Id, actualResult[0].Id);
            Assert.Equal(tagResponses[0].Name, actualResult[0].Name);
            Assert.Equal(tagResponses[0].Type, actualResult[0].Type);
            Assert.Equal(tagResponses[1].Id, actualResult[1].Id);
            Assert.Equal(tagResponses[1].Name, actualResult[1].Name);
            Assert.Equal(tagResponses[1].Type, actualResult[1].Type);
        }
    }
}
