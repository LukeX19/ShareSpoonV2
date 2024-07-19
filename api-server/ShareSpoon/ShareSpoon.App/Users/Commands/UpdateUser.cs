using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.ResponseModels;

namespace ShareSpoon.App.Users.Commands
{
    public record UpdateUser(string Id, string FirstName, string LastName,
        DateTime Birthday, string PictureURL) : IRequest<UserResponseDto>;

    public class UpdateUserHandler : IRequestHandler<UpdateUser, UserResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateUserHandler> _logger;

        public UpdateUserHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateUserHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserResponseDto> Handle(UpdateUser request, CancellationToken ct)
        {
            var user = await _unitOfWork.UserRepository.GetUserById(request.Id, ct);

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Birthday = request.Birthday;
            user.PictureURL = request.PictureURL;

            var updatedUser = await _unitOfWork.UserRepository.Update(user, ct);

            _logger.LogInformation($"Updated user with id {request.Id}");
            return _mapper.Map<UserResponseDto>(updatedUser);
        }
    }
}
