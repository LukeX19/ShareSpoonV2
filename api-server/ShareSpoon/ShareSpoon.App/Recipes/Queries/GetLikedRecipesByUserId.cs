using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.ResponseModels;

namespace ShareSpoon.App.Recipes.Queries
{
    public record GetLikedRecipesByUserId(string UserId, int PageIndex, int PageSize) : IRequest<PagedResponseDto<RecipeWithInteractionsResponseDto>>;

    public class GetLikedRecipesByUserIdHandler : IRequestHandler<GetLikedRecipesByUserId, PagedResponseDto<RecipeWithInteractionsResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetLikedRecipesByUserIdHandler> _logger;

        public GetLikedRecipesByUserIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetLikedRecipesByUserIdHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedResponseDto<RecipeWithInteractionsResponseDto>> Handle(GetLikedRecipesByUserId request, CancellationToken ct)
        {
            var recipes = await _unitOfWork.RecipeRepository.GetLikedRecipesByUserId(request.UserId, request.PageIndex, request.PageSize, ct);

            _logger.LogInformation($"Retrieved all recipes liked by user {request.UserId}");
            return recipes;
        }
    }
}
