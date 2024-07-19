using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.ResponseModels;

namespace ShareSpoon.App.Likes.Queries
{
    public record GetLikesCounterByRecipeId(long RecipeId) : IRequest<LikesCounterResponseDto>;

    public class GetLikeCounterByRecipeIdHandler : IRequestHandler<GetLikesCounterByRecipeId, LikesCounterResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetLikeCounterByRecipeIdHandler> _logger;

        public GetLikeCounterByRecipeIdHandler(IUnitOfWork unitOfWork, ILogger<GetLikeCounterByRecipeIdHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<LikesCounterResponseDto> Handle(GetLikesCounterByRecipeId request, CancellationToken ct)
        {
            var result = await _unitOfWork.LikeRepository.GetLikesCounterByRecipeId(request.RecipeId, ct);

            _logger.LogInformation($"Retrieved total likes counter for recipe with id {request.RecipeId}");
            return new LikesCounterResponseDto
            {
                LikesCounter = result
            };
        }
    }
}
