using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.ResponseModels;

namespace ShareSpoon.App.Ingredients.Commands
{
    public record UpdateIngredient(long Id, string Name) : IRequest<IngredientResponseDto>;

    public class UpdateIngredientHandler : IRequestHandler<UpdateIngredient, IngredientResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateIngredientHandler> _logger;

        public UpdateIngredientHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateIngredientHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IngredientResponseDto> Handle(UpdateIngredient request, CancellationToken ct)
        {
            var ingredient = await _unitOfWork.IngredientRepository.GetById(request.Id, ct);

            ingredient.Name = request.Name;

            var updatedIngredient = await _unitOfWork.IngredientRepository.Update(ingredient, ct);

            _logger.LogInformation($"Updated ingredient with id {request.Id}");
            return _mapper.Map<IngredientResponseDto>(updatedIngredient);
        }
    }
}
