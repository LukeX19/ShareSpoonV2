using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.ResponseModels;

namespace ShareSpoon.App.Comments.Queries
{
    public record GetCommentsByRecipeId(long RecipeId, int PageIndex, int PageSize) : IRequest<PagedResponseDto<CommentResponseDto>>;

    public class GetCommentsByRecipeIdHandler : IRequestHandler<GetCommentsByRecipeId, PagedResponseDto<CommentResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCommentsByRecipeIdHandler> _logger;

        public GetCommentsByRecipeIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetCommentsByRecipeIdHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedResponseDto<CommentResponseDto>> Handle(GetCommentsByRecipeId request, CancellationToken ct)
        {
            var comments = await _unitOfWork.CommentRepository.GetCommentsByRecipeId(request.RecipeId, request.PageIndex, request.PageSize, ct);

            _logger.LogInformation($"Retrieved comments for recipe with id {request.RecipeId}");
            return new PagedResponseDto<CommentResponseDto>(
                elements: _mapper.Map<List<CommentResponseDto>>(comments.Elements),
                pageIndex: comments.PageIndex,
                totalPages: comments.TotalPages
            );
        }
    }
}
