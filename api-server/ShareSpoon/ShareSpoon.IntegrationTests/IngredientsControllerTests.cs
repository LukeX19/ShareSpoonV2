using Microsoft.AspNetCore.Mvc;
using ShareSpoon.Api.Controllers;
using ShareSpoon.App.Ingredients.Requests;
using ShareSpoon.App.Ingredients.Response;
using ShareSpoon.Domain.Enums;
using ShareSpoon.Infrastructure;
using ShareSpoon.Infrastructure.Repositories;
using ShareSpoon.IntegrationTests.Helpers;
using System.Net;

namespace ShareSpoon.IntegrationTests
{
    public class IngredientsControllerTests
    {
        [Fact]
        public async Task IngredientsController_CreateIngredient_CreatesIngredientInDb()
        {
            // Arrange
            var ingredientDto = new IngredientRequestDto
            {
                Name = "ingredient-1"
            };
            var expectedResponse = new IngredientResponseDto
            {
                Id = 1,
                Name = "ingredient-1"
            };

            using var contextBuilder = new AppDbContextBuilder(nameof(IngredientsController_CreateIngredient_CreatesIngredientInDb));
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

            var controller = new IngredientsController(mediator);

            // Act
            var requestResult = await controller.CreateIngredient(ingredientDto);

            // Assert
            var result = requestResult as CreatedResult;
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.Created, result.StatusCode);
            Assert.Equal(string.Empty, result.Location);

            var ingredient = result!.Value as IngredientResponseDto;
            Assert.NotNull(ingredient);
            Assert.Equal(expectedResponse.Id, ingredient.Id);
            Assert.Equal(expectedResponse.Name, ingredient.Name);
        }

        [Fact]
        public async Task IngredientsController_GetAllIngredientsFromDb_ReturnsAllIngredients()
        {
            // Arrange
            var numberOfIngredients = 3;

            using var contextBuilder = new AppDbContextBuilder(nameof(IngredientsController_GetAllIngredientsFromDb_ReturnsAllIngredients));
            contextBuilder.SeedIngredients(numberOfIngredients);
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

            var controller = new IngredientsController(mediator);

            // Act
            var requestResult = await controller.GetAllIngredients();

            // Assert
            var result = requestResult as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

            var ingredients = result!.Value as List<IngredientResponseDto>;
            Assert.NotNull(ingredients);
            Assert.Equal(numberOfIngredients, ingredients.Count);
        }

        [Fact]
        public async Task IngredientsController_GetIngredientsByRecipeIdFromDb_ReturnsAllCorrectIngredients()
        {
            // Arrange
            var expectedIngredients = new List<CompleteIngredientResponseDto>
            {
                new CompleteIngredientResponseDto { Id = 1, Name = "ingredient-1", Quantity = 10, QuantityType = QuantityType.Grams },
                new CompleteIngredientResponseDto { Id = 2, Name = "ingredient-2", Quantity = 20, QuantityType = QuantityType.Grams }
            };

            var numberOfRecipes = 1;
            var numberOfIngredients = 2;

            using var contextBuilder = new AppDbContextBuilder(nameof(IngredientsController_GetIngredientsByRecipeIdFromDb_ReturnsAllCorrectIngredients));
            contextBuilder.SeedRecipes(numberOfRecipes);
            contextBuilder.SeedIngredients(numberOfIngredients);
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

            var controller = new IngredientsController(mediator);

            // Act
            var requestResult = await controller.GetIngredientsByRecipeId(1);

            // Assert
            var result = requestResult as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

            var ingredients = result!.Value as List<CompleteIngredientResponseDto>;
            Assert.NotNull(ingredients);
            Assert.Equal(numberOfIngredients, ingredients.Count);
        }

        [Fact]
        public async Task IngredientsController_UpdateIngredient_UpdatesIngredient()
        {
            // Arrange
            var ingredientDto = new IngredientRequestDto
            {
                Name = "updated-ingredient-1"
            };
            var expectedResponse = new IngredientResponseDto
            {
                Id = 1,
                Name = "updated-ingredient-1"
            };

            var numberOfIngredients = 1;

            using var contextBuilder = new AppDbContextBuilder(nameof(IngredientsController_UpdateIngredient_UpdatesIngredient));
            contextBuilder.SeedIngredients(numberOfIngredients);
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

            var controller = new IngredientsController(mediator);

            // Act
            var requestResult = await controller.UpdateIngredient(1, ingredientDto);

            // Assert
            var result = requestResult as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

            var ingredient = result!.Value as IngredientResponseDto;
            Assert.NotNull(ingredient);
            Assert.Equal(expectedResponse.Id, ingredient.Id);
            Assert.Equal(expectedResponse.Name, ingredient.Name);
        }
    }
}
