using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;

namespace ShareSpoon.App.Auth.Commands
{
    public record DeleteUser(string Id) : IRequest<Unit>;

    public class DeleteUserHandler : IRequestHandler<DeleteUser, Unit>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<DeleteUserHandler> _logger;

        public DeleteUserHandler(IAuthenticationService authenticationService, ILogger<DeleteUserHandler> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteUser request, CancellationToken ct)
        {
            await _authenticationService.DeleteAccount(request.Id, ct);

            _logger.LogInformation($"Deleted user with id {request.Id}");
            return Unit.Value;
        }
    }
}
