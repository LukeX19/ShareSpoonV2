using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.ResponseModels;
using ShareSpoon.Domain.Enums;
using ShareSpoon.Domain.Models.Users;

namespace ShareSpoon.App.Auth.Commands
{
    public record RegisterUser(string FirstName, string LastName, DateTime Birthday, string PictureURL,
        string Email, string Password, AppRole Role) : IRequest<AuthenticationResponseDto>;

    public class RegisterUserHandler : IRequestHandler<RegisterUser, AuthenticationResponseDto>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<RegisterUserHandler> _logger;

        public RegisterUserHandler(IAuthenticationService authenticationService, ITokenService tokenService, ILogger<RegisterUserHandler> logger)
        {
            _authenticationService = authenticationService;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<AuthenticationResponseDto> Handle(RegisterUser request, CancellationToken ct)
        {
            var user = new AppUser()
            {
                Email = request.Email,
                UserName = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Birthday = request.Birthday,
                PictureURL = request.PictureURL,
                Role = request.Role
            };
            var createdUser = await _authenticationService.Register(user, request.Password, ct);

            var claimsIdentity = _tokenService.CreateClaimsIdentity(createdUser);
            var result = _tokenService.CreateSecurityToken(claimsIdentity);

            _logger.LogInformation("Registration successful");
            return new AuthenticationResponseDto
            {
                Token = result,
                Role = createdUser.Role
            };
        }
    }
}
