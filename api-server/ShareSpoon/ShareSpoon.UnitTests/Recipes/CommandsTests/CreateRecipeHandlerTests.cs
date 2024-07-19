using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.Exceptions;
using ShareSpoon.App.Recipes.Commands;
using ShareSpoon.App.Recipes.Requests;
using ShareSpoon.App.Recipes.Responses;
using ShareSpoon.App.Users.Responses;
using ShareSpoon.Domain.Enums;
using ShareSpoon.Domain.Models.Ingredients;
using ShareSpoon.Domain.Models.Recipes;
using ShareSpoon.Domain.Models.Users;

namespace ShareSpoon.UnitTests.Recipes.CommandsTests
{
    public class CreateRecipeHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<CreateRecipeHandler>> _loggerMock;
        private readonly CreateRecipeHandler _handler;

        public CreateRecipeHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<CreateRecipeHandler>>();
            _handler = new CreateRecipeHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_CreateRecipe_ValidInput_CreatesRecipe()
        {
            // Arrange
            var command = new CreateRecipe(1, "Chocolate Cake", "Delicious dark chocolate cake", new TimeSpan(1, 20, 0),
                DifficultyLevel.Medium, new List<RecipeIngredientRequestDto> {
                    new RecipeIngredientRequestDto { Id = 1, Quantity = 500, QuantityType = QuantityType.Grams }
                },
                new List<RecipeTagRequestDto> {
                    new RecipeTagRequestDto { Id = 1 }
                },
                "example.url/image"
            );

            var user = new User { Id = 1, FirstName = "John", LastName = "Doe" };
            var ingredients = new List<Ingredient> {
                new Ingredient { Id = 1, Name = "Chocolate" }
            };
            var tags = new List<Tag> {
                new Tag { Id = 1, Name = "Dessert", Type = TagType.Course }
            };
            var recipe = new Recipe { Id = 1 };

            var recipeResponse = new RecipeResponseDto
            {
                Id = 1,
                UserId = user.Id,
                User = new UserResponseDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                },
                Name = "Chocolate Cake",
                Description = "Delicious dark chocolate cake",
                EstimatedTime = new TimeSpan(1, 20, 0),
                Difficulty = DifficultyLevel.Medium,
                RecipeIngredients = ingredients.Select(i => new RecipeIngredientResponseDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Quantity = 500,
                    QuantityType = QuantityType.Grams
                }).ToList(),
                RecipeTags = tags.Select(t => new RecipeTagResponseDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Type = t.Type
                }).ToList(),
                PictureURL = recipe.PictureURL
            };

            _unitOfWorkMock
                .Setup(x => x.IngredientRepository.EntitiesExist(It.IsAny<IEnumerable<long>>()))
                .Returns(true);

            _unitOfWorkMock
                .Setup(x => x.TagRepository.EntitiesExist(It.IsAny<IEnumerable<long>>()))
                .Returns(true);

            _unitOfWorkMock
                .Setup(x => x.UserRepository.GetUserById(command.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _unitOfWorkMock
                .Setup(x => x.IngredientRepository.GetIngredientsByIds(It.IsAny<List<long>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ingredients);

            _unitOfWorkMock
                .Setup(x => x.TagRepository.GetTagsByIds(It.IsAny<List<long>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tags);

            _unitOfWorkMock
                .Setup(x => x.RecipeRepository.Create(It.IsAny<Recipe>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(recipe);

            _mapperMock
                .Setup(x => x.Map<RecipeResponseDto>(It.IsAny<Recipe>()))
                .Returns(recipeResponse);

            // Act
            var actualResult = await _handler.Handle(command, default);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Equal(recipeResponse.Id, actualResult.Id);
            Assert.Equal(recipeResponse.UserId, actualResult.UserId);
            Assert.Equal(recipeResponse.User.Id, actualResult.User.Id);
            Assert.Equal(recipeResponse.User.FirstName, actualResult.User.FirstName);
            Assert.Equal(recipeResponse.User.LastName, actualResult.User.LastName);
            Assert.Equal(recipeResponse.Name, actualResult.Name);
            Assert.Equal(recipeResponse.Description, actualResult.Description);
            Assert.Equal(recipeResponse.EstimatedTime, actualResult.EstimatedTime);
            Assert.Equal(recipeResponse.Difficulty, actualResult.Difficulty);
            Assert.Equal(recipeResponse.RecipeIngredients.Count, actualResult.RecipeIngredients.Count);
            Assert.Equal(recipeResponse.RecipeTags.Count, actualResult.RecipeTags.Count);
            Assert.Equal(recipeResponse.PictureURL, actualResult.PictureURL);
        }

        [Fact]
        public async Task Handle_CreateRecipe_InvalidInputEmptyIngredientsList_ThrowsException()
        {
            // Arrange
            var request = new CreateRecipe(1, "Test Recipe", "Test Description", new TimeSpan(0, 30, 0),
                DifficultyLevel.Easy, new List<RecipeIngredientRequestDto>(),
                new List<RecipeTagRequestDto>
                {
                    new RecipeTagRequestDto { Id = 1 }
                },
                "example.url/image"
            );

            _unitOfWorkMock.Setup(x => x.IngredientRepository.EntitiesExist(It.IsAny<IEnumerable<long>>())).Returns(false);

            // Act + Assert
            await Assert.ThrowsAsync<EmptyIngredientsListException>(() => _handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_CreateRecipe_InvalidInputEmptyTagsList_ThrowsException()
        {
            // Arrange
            var request = new CreateRecipe(1, "Test Recipe", "Test Description", new TimeSpan(0, 30, 0),
                DifficultyLevel.Easy, new List<RecipeIngredientRequestDto>
                {
                    new RecipeIngredientRequestDto { Id = 1, Quantity = 100, QuantityType = QuantityType.Grams }
                },
                new List<RecipeTagRequestDto>(),
                "example.url/image"
            );

            _unitOfWorkMock.Setup(x => x.IngredientRepository.EntitiesExist(It.IsAny<IEnumerable<long>>())).Returns(true);
            _unitOfWorkMock.Setup(x => x.TagRepository.EntitiesExist(It.IsAny<IEnumerable<long>>())).Returns(false);

            // Act + Assert
            await Assert.ThrowsAsync<EmptyTagsListException>(() => _handler.Handle(request, CancellationToken.None));
        }
    }
}
