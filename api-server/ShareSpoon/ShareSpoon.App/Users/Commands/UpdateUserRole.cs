using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.ResponseModels;
using ShareSpoon.Domain.Enums;

namespace ShareSpoon.App.Users.Commands
{
    public record UpdateUserRole(string UserId, AppRole Role) : IRequest<UserResponseDto>;

    public class UpdateUserRoleHandler : IRequestHandler<UpdateUserRole, UserResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateUserRoleHandler> _logger;

        public UpdateUserRoleHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateUserRoleHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserResponseDto> Handle(UpdateUserRole request, CancellationToken ct)
        {
            var user = await _unitOfWork.UserRepository.GetUserById(request.UserId, ct);

            user.Role = request.Role;

            var updatedUser = await _unitOfWork.UserRepository.Update(user, ct);

            _logger.LogInformation($"Updated user {request.UserId} role");
            return _mapper.Map<UserResponseDto>(updatedUser);
        }
    }
}
