using ShareSpoon.App.ResponseModels;
using ShareSpoon.Domain.Models.Users;

namespace ShareSpoon.App.Abstractions
{
    public interface IUserRepository
    {
        Task<AppUser> GetUserById(string id, CancellationToken ct = default);
        Task<AppUser> Update(AppUser updatedEntity, CancellationToken ct = default);
        Task<UserWithInteractionsResponseDto> GetUserWithInteractionsById(string id, CancellationToken ct = default);
        Task<CustomPagedResponseDto<UserWithInteractionsResponseDto>> GetUsersActivity(int daysNumber,
            int pageIndex, int pageSize, CancellationToken ct = default);
    }
}
