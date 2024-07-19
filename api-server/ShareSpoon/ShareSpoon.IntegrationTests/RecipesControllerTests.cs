using Microsoft.AspNetCore.Mvc;
using ShareSpoon.Api.Controllers;
using ShareSpoon.Domain.Enums;
using ShareSpoon.Infrastructure.Repositories;
using ShareSpoon.Infrastructure;
using ShareSpoon.IntegrationTests.Helpers;
using System.Net;
using ShareSpoon.App.Recipes.Requests;
using ShareSpoon.App.Recipes.Responses;
using ShareSpoon.App.Users.Responses;
using ShareSpoon.App.Roles.Responses;
using ShareSpoon.Infrastructure.Exceptions;

namespace ShareSpoon.IntegrationTests
{
    public class RecipesControllerTests
    {
        [Fact]
        public async Task RecipesController_CreateRecipe_CreatesRecipeInDb()
        {
            // Arrange
            var recipeDto = new CreateRecipeRequestDto
            {
                UserId = 1,
                Name = "recipe-1",
                Description = "descr-recipe-1",
                EstimatedTime = new TimeSpan(1, 0, 0),
                Difficulty = DifficultyLevel.Medium,
                Ingredients = new List<RecipeIngredientRequestDto>
                {
                    new RecipeIngredientRequestDto { Id = 1, Quantity = 10, QuantityType = QuantityType.Grams },
                    new RecipeIngredientRequestDto { Id = 2, Quantity = 20, QuantityType = QuantityType.Grams }
                },
                Tags = new List<RecipeTagRequestDto>
                {
                    new RecipeTagRequestDto { Id = 1 },
                    new RecipeTagRequestDto { Id = 2 }
                },
                PictureURL = "link-recipe-1"
            };
            var expectedResponse = new RecipeResponseDto
            {
                Id = 1,
                UserId = 1,
                User = new UserResponseDto
                {
                    Id = 1,
                    RoleId = 1,
                    Role = new RoleResponseDto
                    {
                        Id = 1,
                        Name = "role-1"
                    },
                    FirstName = "firstName-1",
                    LastName = "lastName-1",
                    Email = "email-1",
                    Password = "password-1",
                    Birthday = new DateTime(2000, 1, 1),
                    PictureURL = "pictureURL-1",
                },
                Name = "recipe-1",
                Description = "descr-recipe-1",
                EstimatedTime = new TimeSpan(1, 0, 0),
                Difficulty = DifficultyLevel.Medium,
                RecipeIngredients = new List<RecipeIngredientResponseDto>
                {
                    new RecipeIngredientResponseDto { Id = 1, Name = "ingredient-1", Quantity = 10, QuantityType = QuantityType.Grams },
                    new RecipeIngredientResponseDto { Id = 2, Name = "ingredient-2", Quantity = 20, QuantityType = QuantityType.Grams }
                },
                RecipeTags = new List<RecipeTagResponseDto>
                {
                    new RecipeTagResponseDto { Id = 1, Name = "tag-1", Type = TagType.Course },
                    new RecipeTagResponseDto { Id = 2, Name = "tag-2", Type = TagType.Course }
                },
                CreatedAt = DateTime.UtcNow,
                PictureURL = "link-recipe-1"
            };

            var numberOfRolesAndUsers = 1;
            var numberOfIngredientsAndTags = 2;

            using var contextBuilder = new AppDbContextBuilder(nameof(RecipesController_CreateRecipe_CreatesRecipeInDb));
            contextBuilder.SeedRoles(numberOfRolesAndUsers);
            contextBuilder.SeedUsers(numberOfRolesAndUsers);
            contextBuilder.SeedIngredients(numberOfIngredientsAndTags);
            contextBuilder.SeedTags(numberOfIngredientsAndTags);
            var dbContext = contextBuilder.GetDbContext();

            var roleRepository = new RoleRepository(dbContext);
            var userRepository = new UserRepository(dbContext);
            var ingredientRepository = new IngredientRepository(dbContext);
            var tagRepository = new TagRepository(dbContext);
            var recipeRepository = new RecipeRepository(dbContext);
            var likeRepository = new LikeRepository(dbContext);
            var commentRepository = new CommentRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext, roleRepository, userRepository, ingredientRepository,
                tagRepository, recipeRepository, likeRepository, commentRepository);

            var mediator = TestHelper.CreateMediator(unitOfWork);

            var controller = new RecipesController(mediator);

            // Act
            var requestResult = await controller.CreateRecipe(recipeDto);

            // Assert
            var result = requestResult as CreatedAtActionResult;
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.Created, result.StatusCode);

            var recipe = result!.Value as RecipeResponseDto;
            Assert.NotNull(recipe);
            Assert.Equal(expectedResponse.Id, recipe.Id);
            Assert.Equal(expectedResponse.UserId, recipe.UserId);
            Assert.Equal(expectedResponse.Name, recipe.Name);
            Assert.Equal(expectedResponse.Description, recipe.Description);
            Assert.Equal(expectedResponse.EstimatedTime, recipe.EstimatedTime);
            Assert.Equal(expectedResponse.Difficulty, recipe.Difficulty);
            Assert.Equal(expectedResponse.RecipeIngredients.Count, recipe.RecipeIngredients.Count);
            Assert.Equal(expectedResponse.RecipeTags.Count, recipe.RecipeTags.Count);
            Assert.Equal(expectedResponse.PictureURL, recipe.PictureURL);
        }

        [Fact]
        public async Task RecipesController_GetAllRecipesFromDb_ReturnsAllRecipes()
        {
            // Arrange
            var numberOfRecipes = 3;

            using var contextBuilder = new AppDbContextBuilder(nameof(RecipesController_GetAllRecipesFromDb_ReturnsAllRecipes));
            contextBuilder.SeedRecipes(numberOfRecipes);
            var dbContext = contextBuilder.GetDbContext();

            var roleRepository = new RoleRepository(dbContext);
            var userRepository = new UserRepository(dbContext);
            var ingredientRepository = new IngredientRepository(dbContext);
            var tagRepository = new TagRepository(dbContext);
            var recipeRepository = new RecipeRepository(dbContext);
            var likeRepository = new LikeRepository(dbContext);
            var commentRepository = new CommentRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext, roleRepository, userRepository, ingredientRepository,
                tagRepository, recipeRepository, likeRepository, commentRepository);

            var mediator = TestHelper.CreateMediator(unitOfWork);

            var controller = new RecipesController(mediator);

            // Act
            var requestResult = await controller.GetAllRecipes();

            // Assert
            var result = requestResult as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

            var recipes = result!.Value as List<RecipeResponseDto>;
            Assert.NotNull(recipes);
            Assert.Equal(numberOfRecipes, recipes.Count);
        }

        [Fact]
        public async Task RecipesController_GetRecipeByIdFromDb_ReturnsCorrectRecipe()
        {
            // Arrange
            var expectedResponse = new RecipeResponseDto
            {
                Id = 1,
                UserId = 1,
                User = new UserResponseDto
                {
                    Id = 1,
                    RoleId = 1,
                    Role = new RoleResponseDto
                    {
                        Id = 1,
                        Name = "role-1"
                    },
                    FirstName = "firstName-1",
                    LastName = "lastName-1",
                    Email = "email-1",
                    Password = "password-1",
                    Birthday = new DateTime(2000, 1, 1),
                    PictureURL = "pictureURL-1",
                },
                Name = "recipe-1",
                Description = "descr-recipe-1",
                EstimatedTime = new TimeSpan(1, 0, 0),
                Difficulty = DifficultyLevel.Medium,
                RecipeIngredients = new List<RecipeIngredientResponseDto>
                {
                    new RecipeIngredientResponseDto { Id = 1, Name = "ingredient-1", Quantity = 10, QuantityType = QuantityType.Grams },
                    new RecipeIngredientResponseDto { Id = 2, Name = "ingredient-2", Quantity = 20, QuantityType = QuantityType.Grams }
                },
                RecipeTags = new List<RecipeTagResponseDto>
                {
                    new RecipeTagResponseDto { Id = 1, Name = "tag-1", Type = TagType.Course },
                    new RecipeTagResponseDto { Id = 2, Name = "tag-2", Type = TagType.Course }
                },
                CreatedAt = DateTime.UtcNow,
                PictureURL = "link-recipe-1"
            };

            var numberOfRecipes = 1;

            using var contextBuilder = new AppDbContextBuilder(nameof(RecipesController_GetRecipeByIdFromDb_ReturnsCorrectRecipe));
            contextBuilder.SeedRecipes(numberOfRecipes);
            var dbContext = contextBuilder.GetDbContext();

            var roleRepository = new RoleRepository(dbContext);
            var userRepository = new UserRepository(dbContext);
            var ingredientRepository = new IngredientRepository(dbContext);
            var tagRepository = new TagRepository(dbContext);
            var recipeRepository = new RecipeRepository(dbContext);
            var likeRepository = new LikeRepository(dbContext);
            var commentRepository = new CommentRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext, roleRepository, userRepository, ingredientRepository,
                tagRepository, recipeRepository, likeRepository, commentRepository);

            var mediator = TestHelper.CreateMediator(unitOfWork);

            var controller = new RecipesController(mediator);

            // Act
            var requestResult = await controller.GetRecipeById(1);

            // Assert
            var result = requestResult as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

            var recipe = result!.Value as RecipeResponseDto;
            Assert.NotNull(recipe);
            Assert.Equal(expectedResponse.Id, recipe.Id);
            Assert.Equal(expectedResponse.UserId, recipe.UserId);
            Assert.Equal(expectedResponse.Name, recipe.Name);
            Assert.Equal(expectedResponse.Description, recipe.Description);
            Assert.Equal(expectedResponse.EstimatedTime, recipe.EstimatedTime);
            Assert.Equal(expectedResponse.Difficulty, recipe.Difficulty);
            Assert.Equal(expectedResponse.RecipeIngredients.Count, recipe.RecipeIngredients.Count);
            Assert.Equal(expectedResponse.RecipeTags.Count, recipe.RecipeTags.Count);
            Assert.Equal(expectedResponse.PictureURL, recipe.PictureURL);
        }

        [Fact]
        public async Task RecipesController_UpdateRecipe_UpdatesRecipe()
        {
            // Arrange
            var recipeDto = new UpdateRecipeRequestDto
            {
                Name = "updated-recipe-1",
                Description = "updated-descr-recipe-1",
                EstimatedTime = new TimeSpan(2, 0, 0),
                Difficulty = DifficultyLevel.Hard,
                PictureURL = "updated-link-recipe-1"
            };
            var expectedResponse = new RecipeResponseDto
            {
                Id = 1,
                UserId = 1,
                User = new UserResponseDto
                {
                    Id = 1,
                    RoleId = 1,
                    Role = new RoleResponseDto
                    {
                        Id = 1,
                        Name = "role-1"
                    },
                    FirstName = "firstName-1",
                    LastName = "lastName-1",
                    Email = "email-1",
                    Password = "password-1",
                    Birthday = new DateTime(2000, 1, 1),
                    PictureURL = "pictureURL-1",
                },
                Name = "updated-recipe-1",
                Description = "updated-descr-recipe-1",
                EstimatedTime = new TimeSpan(2, 0, 0),
                Difficulty = DifficultyLevel.Hard,
                RecipeIngredients = new List<RecipeIngredientResponseDto>
                {
                    new RecipeIngredientResponseDto { Id = 1, Name = "ingredient-1", Quantity = 10, QuantityType = QuantityType.Grams },
                    new RecipeIngredientResponseDto { Id = 2, Name = "ingredient-2", Quantity = 20, QuantityType = QuantityType.Grams }
                },
                RecipeTags = new List<RecipeTagResponseDto>
                {
                    new RecipeTagResponseDto { Id = 1, Name = "tag-1", Type = TagType.Course },
                    new RecipeTagResponseDto { Id = 2, Name = "tag-2", Type = TagType.Course }
                },
                CreatedAt = DateTime.UtcNow,
                PictureURL = "updated-link-recipe-1"
            };

            var numberOfIngredients = 1;

            using var contextBuilder = new AppDbContextBuilder(nameof(RecipesController_UpdateRecipe_UpdatesRecipe));
            contextBuilder.SeedRecipes(numberOfIngredients);
            var dbContext = contextBuilder.GetDbContext();

            var roleRepository = new RoleRepository(dbContext);
            var userRepository = new UserRepository(dbContext);
            var ingredientRepository = new IngredientRepository(dbContext);
            var tagRepository = new TagRepository(dbContext);
            var recipeRepository = new RecipeRepository(dbContext);
            var likeRepository = new LikeRepository(dbContext);
            var commentRepository = new CommentRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext, roleRepository, userRepository, ingredientRepository,
                tagRepository, recipeRepository, likeRepository, commentRepository);

            var mediator = TestHelper.CreateMediator(unitOfWork);

            var controller = new RecipesController(mediator);

            // Act
            var requestResult = await controller.UpdateRecipe(1, recipeDto);

            // Assert
            var result = requestResult as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

            var recipe = result!.Value as RecipeResponseDto;
            Assert.NotNull(recipe);
            Assert.Equal(expectedResponse.Id, recipe.Id);
            Assert.Equal(expectedResponse.Name, recipe.Name);
            Assert.Equal(expectedResponse.Description, recipe.Description);
            Assert.Equal(expectedResponse.EstimatedTime, recipe.EstimatedTime);
            Assert.Equal(expectedResponse.Difficulty, recipe.Difficulty);
            Assert.Equal(expectedResponse.PictureURL, recipe.PictureURL);
        }

        [Fact]
        public async Task RecipesController_DeleteRecipe_DeletesRecipe()
        {
            // Arrange
            var numberOfRecipes = 1;

            using var contextBuilder = new AppDbContextBuilder(nameof(RecipesController_DeleteRecipe_DeletesRecipe));
            contextBuilder.SeedRecipes(numberOfRecipes);
            var dbContext = contextBuilder.GetDbContext();

            var roleRepository = new RoleRepository(dbContext);
            var userRepository = new UserRepository(dbContext);
            var ingredientRepository = new IngredientRepository(dbContext);
            var tagRepository = new TagRepository(dbContext);
            var recipeRepository = new RecipeRepository(dbContext);
            var likeRepository = new LikeRepository(dbContext);
            var commentRepository = new CommentRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext, roleRepository, userRepository, ingredientRepository,
                tagRepository, recipeRepository, likeRepository, commentRepository);

            var mediator = TestHelper.CreateMediator(unitOfWork);

            var controller = new RecipesController(mediator);

            // Act
            var requestResult = await controller.DeleteRecipe(1);

            // Assert
            var result = requestResult as NoContentResult;
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.NoContent, result.StatusCode);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => unitOfWork.RecipeRepository.GetById(1));
        }
    }
}
