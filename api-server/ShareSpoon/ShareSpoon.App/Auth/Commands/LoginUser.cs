using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.ResponseModels;

namespace ShareSpoon.App.Auth.Commands
{
    public record LoginUser(string Email, string Password) : IRequest<AuthenticationResponseDto>;

    public class LoginUserHandler : IRequestHandler<LoginUser, AuthenticationResponseDto>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<LoginUserHandler> _logger;

        public LoginUserHandler(IAuthenticationService authenticationService, ITokenService tokenService, ILogger<LoginUserHandler> logger)
        {
            _authenticationService = authenticationService;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<AuthenticationResponseDto> Handle(LoginUser request, CancellationToken ct)
        {
            var user = await _authenticationService.Login(request.Email, request.Password, ct);

            var claimsIdentity = _tokenService.CreateClaimsIdentity(user);
            var result = _tokenService.CreateSecurityToken(claimsIdentity);

            _logger.LogInformation("Login successful");
            return new AuthenticationResponseDto
            {
                Token = result,
                Role = user.Role
            };
        }
    }
}
