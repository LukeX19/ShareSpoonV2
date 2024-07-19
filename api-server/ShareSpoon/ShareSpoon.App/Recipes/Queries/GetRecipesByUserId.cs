using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.ResponseModels;

namespace ShareSpoon.App.Recipes.Queries
{
    public record GetRecipesByUserId(string UserId, int PageIndex, int PageSize) : IRequest<PagedResponseDto<RecipeWithInteractionsResponseDto>>;

    public class GetRecipesByUserIdHandler : IRequestHandler<GetRecipesByUserId, PagedResponseDto<RecipeWithInteractionsResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetRecipesByUserIdHandler> _logger;

        public GetRecipesByUserIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetRecipesByUserIdHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedResponseDto<RecipeWithInteractionsResponseDto>> Handle(GetRecipesByUserId request, CancellationToken ct)
        {
            var recipes = await _unitOfWork.RecipeRepository.GetRecipesByUserId(request.UserId, request.PageIndex, request.PageSize, ct);

            _logger.LogInformation($"Retrieved all recipes posted by user {request.UserId}");
            return recipes;
        }
    }
}
