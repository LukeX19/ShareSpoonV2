using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.ResponseModels;
using ShareSpoon.Domain.Models.Interactions;

namespace ShareSpoon.App.Comments.Commands
{
    public record CreateComment(string UserId, long RecipeId, string Text) : IRequest<CommentResponseDto>;

    public class CreateCommentHandler : IRequestHandler<CreateComment, CommentResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCommentHandler> _logger;

        public CreateCommentHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateCommentHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CommentResponseDto> Handle(CreateComment request, CancellationToken ct)
        {
            var user = await _unitOfWork.UserRepository.GetUserById(request.UserId, ct);
            var recipe = await _unitOfWork.RecipeRepository.GetRecipeById(request.RecipeId, ct);

            var comment = new Comment()
            {
                UserId = request.UserId,
                User = user,
                RecipeId = request.RecipeId,
                Recipe = recipe,
                Text = request.Text,
                CreatedAt = DateTime.UtcNow
            };
            var createdComment = await _unitOfWork.CommentRepository.Create(comment, ct);

            _logger.LogInformation("Created new comment");
            return _mapper.Map<CommentResponseDto>(createdComment);
        }
    }
}
