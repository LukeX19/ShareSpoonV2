using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.ResponseModels;

namespace ShareSpoon.App.Ingredients.Queries
{
    public record GetAllIngredients() : IRequest<List<IngredientResponseDto>>;

    public class GetAllIngredientsHandler : IRequestHandler<GetAllIngredients, List<IngredientResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllIngredientsHandler> _logger;

        public GetAllIngredientsHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllIngredientsHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<IngredientResponseDto>> Handle(GetAllIngredients request, CancellationToken ct)
        {
            var ingredients = await _unitOfWork.IngredientRepository.GetAll(ct);

            _logger.LogInformation("Retrieved all ingredients");
            return _mapper.Map<List<IngredientResponseDto>>(ingredients);
        }
    }
}
