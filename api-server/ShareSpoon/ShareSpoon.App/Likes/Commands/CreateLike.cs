using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.ResponseModels;
using ShareSpoon.Domain.Models.Interactions;

namespace ShareSpoon.App.Likes.Commands
{
    public record CreateLike(string UserId, long RecipeId) : IRequest<LikeResponseDto>;

    public class CreateLikeHandler : IRequestHandler<CreateLike, LikeResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateLikeHandler> _logger;

        public CreateLikeHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateLikeHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LikeResponseDto> Handle(CreateLike request, CancellationToken ct)
        {
            var user = await _unitOfWork.UserRepository.GetUserById(request.UserId, ct);
            var recipe = await _unitOfWork.RecipeRepository.GetById(request.RecipeId, ct);

            var like = new Like()
            {
                UserId = request.UserId,
                User = user,
                RecipeId = request.RecipeId,
                Recipe = recipe,
                CreatedAt = DateTime.UtcNow
            };
            var createdLike = await _unitOfWork.LikeRepository.Create(like, ct);

            _logger.LogInformation("Created new like");
            return _mapper.Map<LikeResponseDto>(createdLike);
        }
    }
}
