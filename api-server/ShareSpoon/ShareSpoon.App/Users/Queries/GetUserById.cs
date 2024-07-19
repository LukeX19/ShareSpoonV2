using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.ResponseModels;

namespace ShareSpoon.App.Users.Queries
{
    public record GetUserById(string Id) : IRequest<UserWithInteractionsResponseDto>;

    public class GetUserByIdHandler : IRequestHandler<GetUserById, UserWithInteractionsResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetUserByIdHandler> _logger;

        public GetUserByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetUserByIdHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserWithInteractionsResponseDto> Handle(GetUserById request, CancellationToken ct)
        {
            var user = await _unitOfWork.UserRepository.GetUserWithInteractionsById(request.Id, ct);

            _logger.LogInformation($"Retrieved user with id {request.Id}");
            return user;
        }
    }
}
