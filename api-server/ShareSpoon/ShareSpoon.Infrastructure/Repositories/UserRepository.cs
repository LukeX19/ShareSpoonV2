using Microsoft.EntityFrameworkCore;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.ResponseModels;
using ShareSpoon.Domain.Enums;
using ShareSpoon.Domain.Models.Users;
using ShareSpoon.Infrastructure.Exceptions;

namespace ShareSpoon.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AppUser> GetUserById(string id, CancellationToken ct = default)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id, ct)
                ?? throw new EntityNotFoundException("User", id);
        }

        public async Task<AppUser> Update(AppUser updatedEntity, CancellationToken ct = default)
        {
            var entityExists = _context.Users.Any(e => e.Id == updatedEntity.Id);
            if (!entityExists)
            {
                throw new EntityNotFoundException("User", updatedEntity.Id);
            }
            _context.Users.Update(updatedEntity);
            await _context.SaveChangesAsync(ct);
            return updatedEntity;
        }

        public async Task<UserWithInteractionsResponseDto> GetUserWithInteractionsById(string id, CancellationToken ct = default)
        {
            var userQuery = _context.Users
                .Where(u => u.Id == id)
                .Select(user => new UserWithInteractionsResponseDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Birthday = user.Birthday,
                    Age = DateTime.Today.Year - user.Birthday.Year - (DateTime.Today.DayOfYear < user.Birthday.DayOfYear ? 1 : 0),
                    PictureURL = user.PictureURL,
                    Role = user.Role,
                    PostedRecipesCounter = user.Recipes.Count(),
                    ReceivedLikesCounter = user.Recipes.SelectMany(recipe => recipe.Likes).Count()
                });

            var user = await userQuery.FirstOrDefaultAsync(ct);
            if (user == null)
            {
                throw new EntityNotFoundException("User", id);
            }

            return user;
        }

        public async Task<CustomPagedResponseDto<UserWithInteractionsResponseDto>> GetUsersActivity(int daysNumber,
            int pageIndex, int pageSize, CancellationToken ct = default)
        {
            var usersQuery = _context.Users
                .Where(user => user.Role != AppRole.Admin)
                .Select(user => new UserWithInteractionsResponseDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Birthday = user.Birthday,
                    Age = DateTime.Today.Year - user.Birthday.Year - (DateTime.Today.DayOfYear < user.Birthday.DayOfYear ? 1 : 0),
                    PictureURL = user.PictureURL,
                    Role = user.Role,
                    PostedRecipesCounter = user.Recipes
                        .Where(recipe => recipe.CreatedAt >= DateTime.UtcNow.AddDays(-daysNumber))
                        .Count(),
                    ReceivedLikesCounter = user.Recipes
                        .SelectMany(recipe => recipe.Likes)
                        .Where(like => like.CreatedAt >= DateTime.UtcNow.AddDays(-daysNumber))
                        .Count()
                });

            var usersList = await usersQuery.ToListAsync(ct);
            usersList = usersList.OrderByDescending(user => user.ReceivedLikesCounter).ToList();

            var totalUsers = usersList.Count;
            var totalPages = (int)Math.Ceiling(totalUsers / (double)pageSize);

            var users = usersList
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new CustomPagedResponseDto<UserWithInteractionsResponseDto>(users, pageIndex, totalPages, totalUsers);
        }
    }
}
