using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;

namespace ShareSpoon.App.Likes.Commands
{
    public record DeleteLike(string UserId, long RecipeId) : IRequest<Unit>;

    public class DeleteLikeHandler : IRequestHandler<DeleteLike, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteLikeHandler> _logger;

        public DeleteLikeHandler(IUnitOfWork unitOfWork, ILogger<DeleteLikeHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteLike request, CancellationToken ct)
        {
            await _unitOfWork.LikeRepository.DeleteLike(request.UserId, request.RecipeId, ct);

            _logger.LogInformation($"Deleted like for user with id {request.UserId} on recipe with id {request.RecipeId}");
            return Unit.Value;
        }
    }
}
