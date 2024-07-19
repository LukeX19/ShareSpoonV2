using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.ResponseModels;

namespace ShareSpoon.App.Ingredients.Queries
{
    public record SearchIngredientsByName(string Name) : IRequest<List<IngredientResponseDto>>;

    public class SearchIngredientsByNameHandler : IRequestHandler<SearchIngredientsByName, List<IngredientResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<SearchIngredientsByNameHandler> _logger;

        public SearchIngredientsByNameHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<SearchIngredientsByNameHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<IngredientResponseDto>> Handle(SearchIngredientsByName request, CancellationToken ct)
        {
            var ingredients = await _unitOfWork.IngredientRepository.SearchIngredientsByName(request.Name, ct);

            _logger.LogInformation($"Retrieved all ingredients containing {request.Name}");
            return _mapper.Map<List<IngredientResponseDto>>(ingredients);
        }
    }
}
