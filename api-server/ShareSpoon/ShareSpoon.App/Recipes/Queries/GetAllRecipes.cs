using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.ResponseModels;

namespace ShareSpoon.App.Recipes.Queries
{
    public record GetAllRecipes(string UserId, int PageIndex, int PageSize) : IRequest<PagedResponseDto<RecipeWithInteractionsResponseDto>>;

    public class GetAllRecipesHandler : IRequestHandler<GetAllRecipes, PagedResponseDto<RecipeWithInteractionsResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllRecipesHandler> _logger;

        public GetAllRecipesHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllRecipesHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedResponseDto<RecipeWithInteractionsResponseDto>> Handle(GetAllRecipes request, CancellationToken ct)
        {
            var recipes = await _unitOfWork.RecipeRepository.GetAllRecipes(request.UserId, request.PageIndex, request.PageSize, ct);

            _logger.LogInformation("Retrieved all recipes");
            return recipes;
        }
    }
}
