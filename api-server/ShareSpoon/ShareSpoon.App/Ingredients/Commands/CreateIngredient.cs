using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.ResponseModels;
using ShareSpoon.Domain.Models.Ingredients;

namespace ShareSpoon.App.Ingredients.Commands
{
    public record CreateIngredient(string Name) : IRequest<IngredientResponseDto>;

    public class CreateIngredientHandler : IRequestHandler<CreateIngredient, IngredientResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateIngredientHandler> _logger;

        public CreateIngredientHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateIngredientHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IngredientResponseDto> Handle(CreateIngredient request, CancellationToken ct)
        {
            var ingredientName = char.ToUpper(request.Name[0]) + request.Name.Substring(1);

            var ingredient = new Ingredient()
            {
                Name = ingredientName
            };
            var createdIngredient = await _unitOfWork.IngredientRepository.CreateIngredient(ingredient, ct);

            _logger.LogInformation("Created new ingredient");
            return _mapper.Map<IngredientResponseDto>(createdIngredient);
        }
    }
}
