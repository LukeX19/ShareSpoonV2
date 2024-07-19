using Microsoft.EntityFrameworkCore;
using ShareSpoon.Domain.Enums;
using ShareSpoon.Domain.Models.Associations;
using ShareSpoon.Domain.Models.Ingredients;
using ShareSpoon.Domain.Models.Recipes;
using ShareSpoon.Domain.Models.Users;
using ShareSpoon.Infrastructure;

namespace ShareSpoon.IntegrationTests.Helpers
{
    public class AppDbContextBuilder : IDisposable
    {
        private readonly AppDbContext _context;

        public AppDbContextBuilder(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var context = new AppDbContext(options);

            _context = context;
        }

        public AppDbContext GetDbContext()
        {
            _context.Database.EnsureCreated();
            return _context;
        }

        public void SeedRoles(int number)
        {
            var roles = new List<Role>();

            for (int i = 0; i < number; i++)
            {
                var id = i + 1;
                roles.Add(new Role
                {
                    Id = id,
                    Name = $"role-{id}"
                });
            }

            _context.Roles.AddRange(roles);
            _context.SaveChanges();
        }

        public void SeedUsers(int number)
        {
            var users = new List<User>();

            for (int i = 0; i < number; i++)
            {
                var id = i + 1;
                users.Add(new User
                {
                    Id = id,
                    FirstName = $"firstName-{id}",
                    LastName = $"lastName-{id}",
                    Email = $"email-{id}",
                    Password = $"password-{id}",
                    Birthday = new DateTime(2000, 1, 1),
                    PictureURL = $"pictureURL-{id}",
                    RoleId = 1
                });
            }

            _context.Users.AddRange(users);
            _context.SaveChanges();
        }

        public void SeedIngredients(int number)
        {
            var ingredients = new List<Ingredient>();

            for (int i = 0; i < number; i++)
            {
                var id = i + 1;
                ingredients.Add(new Ingredient
                {
                    Id = id,
                    Name = $"ingredient-{id}"
                });
            }

            _context.Ingredients.AddRange(ingredients);
            _context.SaveChanges();
        }

        public void SeedTags(int number)
        {
            var tags = new List<Tag>();

            for (int i = 0; i < number; i++)
            {
                var id = i + 1;
                tags.Add(new Tag
                {
                    Id = id,
                    Name = $"tag-{id}",
                    Type = TagType.Course
                });
            }

            _context.Tags.AddRange(tags);
            _context.SaveChanges();
        }

        public void SeedRecipes(int number)
        {
            var recipes = new List<Recipe>();

            for (int i = 0; i < number; i++)
            {
                var id = i + 1;
                recipes.Add(new Recipe
                {
                    Id = id,
                    UserId = 1,
                    Name = $"recipe-{id}",
                    Description = $"descr-recipe-{id}",
                    EstimatedTime = new TimeSpan(1, 0, 0),
                    Difficulty = DifficultyLevel.Medium,
                    RecipeIngredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient { IngredientId = 1, Quantity = 10, QuantityType = QuantityType.Grams },
                        new RecipeIngredient { IngredientId = 2, Quantity = 20, QuantityType = QuantityType.Grams }
                    },
                    RecipeTags = new List<RecipeTag>
                    {
                        new RecipeTag { TagId = 1 },
                        new RecipeTag { TagId = 2 }
                    },
                    CreatedAt = new DateTime(2000, 1, 1),
                    PictureURL = $"link-recipe-{id}"
                });
            }

            _context.Recipes.AddRange(recipes);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}
