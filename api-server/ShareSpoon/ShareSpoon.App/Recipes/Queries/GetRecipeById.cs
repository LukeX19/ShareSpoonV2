using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.ResponseModels;

namespace ShareSpoon.App.Recipes.Queries
{
    public record GetRecipeById(string UserId, long RecipeId) : IRequest<RecipeWithInteractionsResponseDto>;

    public class GetRecipeByIdHandler : IRequestHandler<GetRecipeById, RecipeWithInteractionsResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetRecipeByIdHandler> _logger;

        public GetRecipeByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetRecipeByIdHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<RecipeWithInteractionsResponseDto> Handle(GetRecipeById request, CancellationToken ct)
        {
            var recipe = await _unitOfWork.RecipeRepository.GetRecipeWithInteractionsById(request.UserId, request.RecipeId, ct);

            _logger.LogInformation($"Retrieved recipe with id {request.RecipeId}");
            return _mapper.Map<RecipeWithInteractionsResponseDto>(recipe);
        }
    }
}
