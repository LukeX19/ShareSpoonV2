using ShareSpoon.Domain.Models.Users;

namespace ShareSpoon.App.Abstractions
{
    public interface IAuthenticationService
    {
        Task<AppUser> Register(AppUser newUser, string password, CancellationToken ct = default);
        Task<AppUser> Login(string email, string password, CancellationToken ct = default);
        Task DeleteAccount(string id, CancellationToken ct = default);
    }
}
