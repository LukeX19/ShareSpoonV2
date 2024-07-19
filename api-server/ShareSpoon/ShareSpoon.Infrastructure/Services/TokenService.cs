using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShareSpoon.App.Abstractions;
using ShareSpoon.Domain.Models.Users;
using ShareSpoon.Infrastructure.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShareSpoon.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings? _settings;
        private readonly byte[] _key;

        public TokenService(IOptions<JwtSettings> jwtOptions)
        {
            _settings = jwtOptions.Value;
            ArgumentNullException.ThrowIfNull(_settings);
            ArgumentNullException.ThrowIfNull(_settings.SigningKey);
            ArgumentNullException.ThrowIfNull(_settings.Audiences);
            ArgumentNullException.ThrowIfNull(_settings.Audiences[0]);
            ArgumentNullException.ThrowIfNull(_settings.Issuer);
            _key = Encoding.ASCII.GetBytes(_settings?.SigningKey!);
        }

        public JwtSecurityTokenHandler TokenHandler = new JwtSecurityTokenHandler();

        public ClaimsIdentity CreateClaimsIdentity(AppUser user)
        {
            return new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            });
        }

        public string CreateSecurityToken(ClaimsIdentity identity)
        {
            var tokenDescription = GetTokenDescriptor(identity);
            var token = TokenHandler.CreateToken(tokenDescription);
            return TokenHandler.WriteToken(token);
        }

        private SecurityTokenDescriptor GetTokenDescriptor(ClaimsIdentity identity)
        {
            return new SecurityTokenDescriptor()
            {
                Subject = identity,
                Expires = DateTime.Now.AddHours(10),
                Audience = _settings!.Audiences?[0]!,
                Issuer = _settings.Issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
        }
    }
}
