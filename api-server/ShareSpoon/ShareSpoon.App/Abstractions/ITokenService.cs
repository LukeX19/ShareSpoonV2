using ShareSpoon.Domain.Models.Users;
using System.Security.Claims;

namespace ShareSpoon.App.Abstractions
{
    public interface ITokenService
    {
        ClaimsIdentity CreateClaimsIdentity(AppUser user);
        string CreateSecurityToken(ClaimsIdentity identity);
    }
}
