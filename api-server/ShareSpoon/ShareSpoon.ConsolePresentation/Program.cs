// See https://aka.ms/new-console-template for more information

using ShareSpoon.Domain.Enums;
using ShareSpoon.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using ShareSpoon.App.Abstractions;
using MediatR;
using ShareSpoon.App.Users.Commands;
using ShareSpoon.App.Users.Queries;
using ShareSpoon.App.Tags.Commands;
using ShareSpoon.App.Ingredients.Commands;
using ShareSpoon.App.Recipes.Commands;
using ShareSpoon.App.Recipes.Requests;
using ShareSpoon.App.Recipes.Queries;
using ShareSpoon.App.Ingredients.Queries;
using ShareSpoon.Infrastructure;


// Dependency Injection Container
var diContainer = new ServiceCollection()
    .AddScoped<IUserRepository, UserRepository>()
    .AddScoped<IIngredientRepository, IngredientRepository>()
    .AddScoped<ITagRepository, TagRepository>()
    .AddScoped<IRecipeRepository, RecipeRepository>()
    .AddScoped<ILikeRepository, LikeRepository>()
    .AddScoped<ICommentRepository, CommentRepository>()
    .AddScoped<IUnitOfWork, UnitOfWork>()
    .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IRecipeRepository).Assembly))
    .AddDbContext<AppDbContext>()
    .BuildServiceProvider();

/*// Setup
var dbContext = diContainer.GetRequiredService<AppDbContext>();
var mediator = diContainer.GetRequiredService<IMediator>();

// Clear Database
dbContext.Database.EnsureDeleted();
dbContext.Database.EnsureCreated();


// ----------------------------------------------------------------------------------------------------------
// Roles

var admin = await mediator.Send(new CreateRole("Admin"));
var cook = await mediator.Send(new CreateRole("Home Cook"));
var chef = await mediator.Send(new CreateRole("Professional Chef"));


// ----------------------------------------------------------------------------------------------------------
// Users

var fred = await mediator.Send(new RegisterUser(cook.Id, "Fred", "Noyes", "fred.noyes@gmail.com",
    "passFred", DateTime.UtcNow.AddYears(-27), "picture/link.com"));
var abigail = await mediator.Send(new RegisterUser(chef.Id, "Abigail", "Joelle", "aby90@yahoo.com",
    "passAbigail", DateTime.UtcNow.AddYears(-34), "picture/link.com"));
var becca = await mediator.Send(new RegisterUser(chef.Id, "Becca", "Richards", "richards_becca@outlook.com",
    "passBecca", DateTime.UtcNow.AddYears(-19), "picture/link.com"));


// ----------------------------------------------------------------------------------------------------------
// Tags

var american = await mediator.Send(new CreateTag("American", TagType.Cuisine));
var americanTagRequest = new RecipeTagRequestDto { Id = american.Id };

var dessert = await mediator.Send(new CreateTag("Dessert", TagType.Course));
var dessertTagRequest = new RecipeTagRequestDto { Id = dessert.Id };

var italian = await mediator.Send(new CreateTag("Italian", TagType.Cuisine));
var italianTagRequest = new RecipeTagRequestDto { Id = italian.Id };


// ----------------------------------------------------------------------------------------------------------
// Ingredients

var banana = await mediator.Send(new CreateIngredient("Banana"));
var bread = await mediator.Send(new CreateIngredient("Bread"));
var nutella = await mediator.Send(new CreateIngredient("Nutella Chocolate Cream"));


// ----------------------------------------------------------------------------------------------------------
// Recipes section

var bananaBreadIngredients = new List<RecipeIngredientRequestDto>
{
    new RecipeIngredientRequestDto { Id = banana.Id, Quantity = 2, QuantityType = QuantityType.Pieces },
    new RecipeIngredientRequestDto { Id = bread.Id, Quantity = 100, QuantityType = QuantityType.Grams }
};
var bananaBreadTags = new List<RecipeTagRequestDto> { dessertTagRequest, italianTagRequest };
var bananaBread = await mediator.Send(new CreateRecipe(fred.Id, "Banana Bread", "Banana Bread Description",
    new TimeSpan(1, 20, 0), DifficultyLevel.Medium, bananaBreadIngredients, bananaBreadTags, "picture/link.com"));

var pancakesIngredients = new List<RecipeIngredientRequestDto>
{
    new RecipeIngredientRequestDto { Id = nutella.Id, Quantity = 3, QuantityType = QuantityType.Tablespoons }
};
var pancakesTags = new List<RecipeTagRequestDto> { dessertTagRequest, americanTagRequest };
var pancakes = await mediator.Send(new CreateRecipe(becca.Id, "Pancakes", "Nutella Pancakes Description",
    new TimeSpan(0, 30, 0), DifficultyLevel.Easy, pancakesIngredients, pancakesTags, "picture/link.com"));


// ----------------------------------------------------------------------------------------------------------
// Printing section

var recipe1 = await mediator.Send(new GetRecipeById(bananaBread.Id));
Console.WriteLine($"- Recipe {recipe1.Id}\n- {recipe1.Name} ({recipe1.Description})\n- {recipe1.Difficulty} {recipe1.EstimatedTime}");

long userIdFromRecipe1 = (long)recipe1.UserId!;
var user1 = await mediator.Send(new GetUserById(userIdFromRecipe1));
Console.WriteLine($"- Posted by: {user1.FirstName} {user1.LastName} [{user1.Role.Name}]");

Console.WriteLine("- Ingredients:");
var ingredientsRecipe1 = await mediator.Send(new GetIngredientsByRecipeId(recipe1.Id));
foreach (var item in ingredientsRecipe1)
{
    Console.WriteLine($"\t- {item.Quantity} x {item.QuantityType}: {item.Name}");
}

Console.WriteLine("- Tags:");
foreach (var tag in recipe1.RecipeTags)
{
    Console.WriteLine($"\t# {tag.Name}");
}

Console.WriteLine("\n");

var recipe2 = await mediator.Send(new GetRecipeById(pancakes.Id));
Console.WriteLine($"- Recipe {recipe2.Id}\n- {recipe2.Name} ({recipe2.Description})\n- {recipe2.Difficulty} {recipe2.EstimatedTime}");

long userIdFromRecipe2 = (long)recipe2.UserId!;
var user2 = await mediator.Send(new GetUserById(userIdFromRecipe2));
Console.WriteLine($"- Posted by: {user2.FirstName} {user2.LastName} [{user2.Role.Name}]");

Console.WriteLine("- Ingredients:");
var ingredientsRecipe2 = await mediator.Send(new GetIngredientsByRecipeId(recipe2.Id));
foreach (var item in ingredientsRecipe2)
{
    Console.WriteLine($"\t- {item.Quantity} x {item.QuantityType}: {item.Name}");
}

Console.WriteLine("- Tags:");
foreach (var tag in recipe2.RecipeTags)
{
    Console.WriteLine($"\t# {tag.Name}");
}

Console.WriteLine("\n");


// ----------------------------------------------------------------------------------------------------------
// Update section

//chef = await mediator.Send(new UpdateRole(chef.Id, "Chef"));
//becca = await mediator.Send(new UpdateUser(becca.Id, "Rebecca", "Richards", "richards_rebecca1@outlook.com", "passBecca", DateTime.UtcNow.AddYears(-19), "picture/link.com"));
//nutella = await mediator.Send(new UpdateIngredient(nutella.Id, "Nutella Hazelnut"));
//pancakes = await mediator.Send(new UpdateRecipe(pancakes.Id, "Tasty Pancakes", "Generic description for Nutella Pancakes", new TimeSpan(0, 27, 53), DifficultyLevel.Easy, "www.url.com"));


//var recipe3 = await mediator.Send(new GetRecipeById(pancakes.Id));
//Console.WriteLine($"- Recipe {recipe3.Id}\n- {recipe3.Name} ({recipe3.Description})\n- {recipe3.Difficulty} {recipe3.EstimatedTime}");

//var user3 = await mediator.Send(new GetUserById(recipe3.UserId));
//Console.WriteLine($"- Posted by: {user3.FirstName} {user3.LastName} [{user3.Role.Name}]");

//Console.WriteLine("- Ingredients:");
//var ingredientsRecipe3 = await mediator.Send(new GetIngredientsByRecipeId(recipe3.Id));
foreach (var item in ingredientsRecipe3)
{
    Console.WriteLine($"\t- {item.Quantity} x {item.QuantityType}: {item.Name}");
}

//Console.WriteLine("- Tags:");
foreach (var tag in recipe3.RecipeTags)
{
    Console.WriteLine($"\t# {tag.Tag.Name}");
}

//Console.WriteLine("\n");


// ----------------------------------------------------------------------------------------------------------
// Delete section

//await mediator.Send(new DeleteUser(becca.Id));*/