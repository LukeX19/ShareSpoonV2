using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;

namespace ShareSpoon.App.Comments.Commands
{
    public record DeleteComment(long Id) : IRequest<Unit>;

    public class DeleteCommentHandler : IRequestHandler<DeleteComment, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteCommentHandler> _logger;

        public DeleteCommentHandler(IUnitOfWork unitOfWork, ILogger<DeleteCommentHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteComment request, CancellationToken ct)
        {
            await _unitOfWork.CommentRepository.Delete(request.Id, ct);

            _logger.LogInformation($"Deleted comment with id {request.Id}");
            return Unit.Value;
        }
    }
}
