using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.ResponseModels;
using ShareSpoon.Domain.Enums;

namespace ShareSpoon.App.Recipes.Queries
{
    public record SearchRecipes(string? Input, bool? PromotedUsers, List<DifficultyLevel>? Difficulties, List<long>? TagIds,
        string UserId, int PageIndex, int PageSize) : IRequest<CustomPagedResponseDto<RecipeWithInteractionsResponseDto>>;

    public class SearchRecipesHandler : IRequestHandler<SearchRecipes, CustomPagedResponseDto<RecipeWithInteractionsResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<SearchRecipesHandler> _logger;

        public SearchRecipesHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<SearchRecipesHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CustomPagedResponseDto<RecipeWithInteractionsResponseDto>> Handle(SearchRecipes request, CancellationToken ct)
        {
            var recipes = await _unitOfWork.RecipeRepository.SearchRecipes(request.Input, request.PromotedUsers,
                request.Difficulties, request.TagIds, request.UserId, request.PageIndex, request.PageSize, ct);

            _logger.LogInformation($"Retrieved all recipes based on input");
            return recipes;
        }
    }
}
