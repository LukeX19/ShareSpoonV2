using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;

namespace ShareSpoon.App.Recipes.Commands
{
    public record DeleteRecipe(long Id) : IRequest<Unit>;

    public class DeleteRecipeHandler : IRequestHandler<DeleteRecipe, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteRecipeHandler> _logger;

        public DeleteRecipeHandler(IUnitOfWork unitOfWork, ILogger<DeleteRecipeHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteRecipe request, CancellationToken ct)
        {
            await _unitOfWork.RecipeRepository.Delete(request.Id, ct);

            _logger.LogInformation($"Deleted recipe with id {request.Id}");
            return Unit.Value;
        }
    }
}
