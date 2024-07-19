using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.ResponseModels;

namespace ShareSpoon.App.Comments.Commands
{
    public record UpdateComment(long Id, string Text) : IRequest<CommentResponseDto>;

    public class UpdateCommentHandler : IRequestHandler<UpdateComment, CommentResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCommentHandler> _logger;

        public UpdateCommentHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateCommentHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CommentResponseDto> Handle(UpdateComment request, CancellationToken ct)
        {
            var comment = await _unitOfWork.CommentRepository.GetCommentById(request.Id, ct);

            comment.Text = request.Text;
            comment.CreatedAt = DateTime.UtcNow;

            var updatedComment = await _unitOfWork.CommentRepository.Update(comment, ct);

            _logger.LogInformation($"Updated comment with id {request.Id}");
            return _mapper.Map<CommentResponseDto>(updatedComment);
        }
    }
}
