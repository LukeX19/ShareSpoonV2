using Microsoft.EntityFrameworkCore;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.ResponseModels;
using ShareSpoon.Domain.Enums;
using ShareSpoon.Domain.Models.Recipes;
using ShareSpoon.Infrastructure.Exceptions;
using ShareSpoon.Infrastructure.Repositories.BasicRepositories;

namespace ShareSpoon.Infrastructure.Repositories
{
    public class RecipeRepository : BaseRepositoryEF<Recipe>, IRecipeRepository
    {
        public RecipeRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Recipe> GetRecipeById(long recipeId, CancellationToken ct = default)
        {
            return await _context.Recipes.AsSplitQuery()
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .Include(r => r.RecipeTags)
                    .ThenInclude(rt => rt.Tag)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == recipeId, ct)
                ?? throw new EntityNotFoundException(nameof(Recipe), recipeId);
        }

        public async Task<RecipeWithInteractionsResponseDto> GetRecipeWithInteractionsById(string userId, long recipeId, CancellationToken ct = default)
        {
            var recipes = await _context.Recipes.AsSplitQuery()
                .Where(r => r.Id == recipeId)
                .Select(r => new RecipeWithInteractionsResponseDto
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    User = new UserResponseDto
                    {
                        Id = r.User.Id,
                        FirstName = r.User.FirstName,
                        LastName = r.User.LastName,
                        Email = r.User.Email,
                        Birthday = r.User.Birthday,
                        PictureURL = r.User.PictureURL,
                        Role = r.User.Role
                    },
                    Name = r.Name,
                    Description = r.Description,
                    EstimatedTime = r.EstimatedTime,
                    Difficulty = r.Difficulty,
                    RecipeIngredients = r.RecipeIngredients.Select(ri => new RecipeIngredientResponseDto
                    {
                        Id = ri.IngredientId,
                        Name = ri.Ingredient.Name,
                        Quantity = ri.Quantity,
                        QuantityType = ri.QuantityType
                    }).ToList(),
                    RecipeTags = r.RecipeTags.Select(rt => new RecipeTagResponseDto
                    {
                        Id = rt.TagId,
                        Name = rt.Tag.Name,
                        Type = rt.Tag.Type
                    }).ToList(),
                    CreatedAt = r.CreatedAt,
                    PictureURL = r.PictureURL,
                    LikesCounter = r.Likes.Count(),
                    CurrentUserLiked = r.Likes.Any(l => l.UserId == userId),
                    CommentsCounter = r.Comments.Count()
                })
                .ToListAsync(ct);

            return recipes.FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Recipe), recipeId);
        }

        public async Task<PagedResponseDto<RecipeWithInteractionsResponseDto>> GetAllRecipes(string userId, int pageIndex, int pageSize, CancellationToken ct = default)
        {
            var recipes = await _context.Recipes.AsSplitQuery()
                .OrderByDescending(r => r.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new RecipeWithInteractionsResponseDto
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    User = new UserResponseDto
                    {
                        Id = r.User.Id,
                        FirstName = r.User.FirstName,
                        LastName = r.User.LastName,
                        Email = r.User.Email,
                        Birthday = r.User.Birthday,
                        PictureURL = r.User.PictureURL,
                        Role = r.User.Role
                    },
                    Name = r.Name,
                    Description = r.Description,
                    EstimatedTime = r.EstimatedTime,
                    Difficulty = r.Difficulty,
                    RecipeIngredients = r.RecipeIngredients.Select(ri => new RecipeIngredientResponseDto
                    {
                        Id = ri.IngredientId,
                        Name = ri.Ingredient.Name,
                        Quantity = ri.Quantity,
                        QuantityType = ri.QuantityType
                    }).ToList(),
                    RecipeTags = r.RecipeTags.Select(rt => new RecipeTagResponseDto
                    {
                        Id = rt.TagId,
                        Name = rt.Tag.Name,
                        Type = rt.Tag.Type
                    }).ToList(),
                    CreatedAt = r.CreatedAt,
                    PictureURL = r.PictureURL,
                    LikesCounter = r.Likes.Count(),
                    CurrentUserLiked = r.Likes.Any(l => l.UserId == userId),
                    CommentsCounter = r.Comments.Count()
                })
                .ToListAsync(ct);

            var count = await _context.Recipes.CountAsync(ct);
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PagedResponseDto<RecipeWithInteractionsResponseDto>(recipes, pageIndex, totalPages);
        }

        public async Task<PagedResponseDto<RecipeWithInteractionsResponseDto>> GetRecipesByUserId(string userId, int pageIndex, int pageSize, CancellationToken ct = default)
        {
            var recipes = await _context.Recipes.AsSplitQuery()
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new RecipeWithInteractionsResponseDto
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    User = new UserResponseDto
                    {
                        Id = r.User.Id,
                        FirstName = r.User.FirstName,
                        LastName = r.User.LastName,
                        Email = r.User.Email,
                        Birthday = r.User.Birthday,
                        PictureURL = r.User.PictureURL,
                        Role = r.User.Role
                    },
                    Name = r.Name,
                    Description = r.Description,
                    EstimatedTime = r.EstimatedTime,
                    Difficulty = r.Difficulty,
                    RecipeIngredients = r.RecipeIngredients.Select(ri => new RecipeIngredientResponseDto
                    {
                        Id = ri.IngredientId,
                        Name = ri.Ingredient.Name,
                        Quantity = ri.Quantity,
                        QuantityType = ri.QuantityType
                    }).ToList(),
                    RecipeTags = r.RecipeTags.Select(rt => new RecipeTagResponseDto
                    {
                        Id = rt.TagId,
                        Name = rt.Tag.Name,
                        Type = rt.Tag.Type
                    }).ToList(),
                    CreatedAt = r.CreatedAt,
                    PictureURL = r.PictureURL,
                    LikesCounter = r.Likes.Count(),
                    CurrentUserLiked = r.Likes.Any(l => l.UserId == userId),
                    CommentsCounter = r.Comments.Count()
                })
                .ToListAsync(ct);

            var count = await _context.Recipes.Where(r => r.UserId == userId).CountAsync(ct);
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PagedResponseDto<RecipeWithInteractionsResponseDto>(recipes, pageIndex, totalPages);
        }

        public async Task<PagedResponseDto<RecipeWithInteractionsResponseDto>> GetLikedRecipesByUserId(string userId, int pageIndex, int pageSize, CancellationToken ct = default)
        {
            var likedRecipes = await _context.Likes.AsSplitQuery()
                .Where(l => l.UserId == userId)
                .Select(l => l.Recipe)
                .OrderByDescending(r => r.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new RecipeWithInteractionsResponseDto
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    User = new UserResponseDto
                    {
                        Id = r.User.Id,
                        FirstName = r.User.FirstName,
                        LastName = r.User.LastName,
                        Email = r.User.Email,
                        Birthday = r.User.Birthday,
                        PictureURL = r.User.PictureURL,
                        Role = r.User.Role
                    },
                    Name = r.Name,
                    Description = r.Description,
                    EstimatedTime = r.EstimatedTime,
                    Difficulty = r.Difficulty,
                    RecipeIngredients = r.RecipeIngredients.Select(ri => new RecipeIngredientResponseDto
                    {
                        Id = ri.IngredientId,
                        Name = ri.Ingredient.Name,
                        Quantity = ri.Quantity,
                        QuantityType = ri.QuantityType
                    }).ToList(),
                    RecipeTags = r.RecipeTags.Select(rt => new RecipeTagResponseDto
                    {
                        Id = rt.TagId,
                        Name = rt.Tag.Name,
                        Type = rt.Tag.Type
                    }).ToList(),
                    CreatedAt = r.CreatedAt,
                    PictureURL = r.PictureURL,
                    LikesCounter = r.Likes.Count(),
                    CurrentUserLiked = r.Likes.Any(l => l.UserId == userId),
                    CommentsCounter = r.Comments.Count()
                })
                .ToListAsync(ct);

            var count = await _context.Likes
                .Where(l => l.UserId == userId)
                .Select(l => l.Recipe)
                .CountAsync(ct);
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PagedResponseDto<RecipeWithInteractionsResponseDto>(likedRecipes, pageIndex, totalPages);
        }

        public async Task<CustomPagedResponseDto<RecipeWithInteractionsResponseDto>> SearchRecipes(string? input, bool? promotedUsers,
            List<DifficultyLevel>? difficulties, List<long>? tagIds, string userId, int pageIndex, int pageSize, CancellationToken ct = default)
        {
            var query = _context.Recipes.AsSplitQuery();

            if (!string.IsNullOrWhiteSpace(input))
            {
                query = query.Where(r => r.Name.ToLower().Contains(input.ToLower()) ||
                            r.RecipeIngredients.Any(ri => ri.Ingredient.Name.ToLower().Contains(input.ToLower())) ||
                            r.RecipeTags.Any(rt => rt.Tag.Name.ToLower().Contains(input.ToLower())));
            }

            if (promotedUsers.HasValue && promotedUsers.Value)
            {
                query = query.Where(r => r.User.Role == AppRole.Chef);
            }

            if (difficulties != null && difficulties.Any())
            {
                query = query.Where(r => difficulties.Contains(r.Difficulty));
            }

            if (tagIds != null && tagIds.Any())
            {
                query = query.Where(r => r.RecipeTags.Any(rt => tagIds.Contains(rt.TagId)));
            }

            var query2 = query.OrderByDescending(r => r.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new RecipeWithInteractionsResponseDto
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    User = new UserResponseDto
                    {
                        Id = r.User.Id,
                        FirstName = r.User.FirstName,
                        LastName = r.User.LastName,
                        Email = r.User.Email,
                        Birthday = r.User.Birthday,
                        PictureURL = r.User.PictureURL,
                        Role = r.User.Role
                    },
                    Name = r.Name,
                    Description = r.Description,
                    EstimatedTime = r.EstimatedTime,
                    Difficulty = r.Difficulty,
                    RecipeIngredients = r.RecipeIngredients.Select(ri => new RecipeIngredientResponseDto
                    {
                        Id = ri.IngredientId,
                        Name = ri.Ingredient.Name,
                        Quantity = ri.Quantity,
                        QuantityType = ri.QuantityType
                    }).ToList(),
                    RecipeTags = r.RecipeTags.Select(rt => new RecipeTagResponseDto
                    {
                        Id = rt.TagId,
                        Name = rt.Tag.Name,
                        Type = rt.Tag.Type
                    }).ToList(),
                    CreatedAt = r.CreatedAt,
                    PictureURL = r.PictureURL,
                    LikesCounter = r.Likes.Count(),
                    CurrentUserLiked = r.Likes.Any(l => l.UserId == userId),
                    CommentsCounter = r.Comments.Count()
                });

            var recipes = await query2.ToListAsync(ct);

            var count = await query.CountAsync(ct);

            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new CustomPagedResponseDto<RecipeWithInteractionsResponseDto>(recipes, pageIndex, totalPages, count);
        }
    }
}
