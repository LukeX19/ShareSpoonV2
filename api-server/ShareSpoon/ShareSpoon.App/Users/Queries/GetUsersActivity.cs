using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.ResponseModels;

namespace ShareSpoon.App.Users.Queries
{
    public record GetUsersActivity(int DaysNumber, int PageIndex, int PageSize) : IRequest<CustomPagedResponseDto<UserWithInteractionsResponseDto>>;

    public class GetUsersActivityHandler : IRequestHandler<GetUsersActivity, CustomPagedResponseDto<UserWithInteractionsResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetUsersActivityHandler> _logger;

        public GetUsersActivityHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetUsersActivityHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CustomPagedResponseDto<UserWithInteractionsResponseDto>> Handle(GetUsersActivity request, CancellationToken ct)
        {
            var users = await _unitOfWork.UserRepository.GetUsersActivity(request.DaysNumber,
                request.PageIndex, request.PageSize, ct);

            _logger.LogInformation($"Retrieved users activity");
            return users;
        }
    }
}
