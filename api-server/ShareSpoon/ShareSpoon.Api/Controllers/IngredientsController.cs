using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareSpoon.App.Ingredients.Commands;
using ShareSpoon.App.Ingredients.Queries;
using ShareSpoon.App.RequestModels;

namespace ShareSpoon.Api.Controllers
{
    [ApiController]
    [Route("api/ingredients")]
    [Authorize]
    public class IngredientsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IngredientsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateIngredient(IngredientRequestDto ingredient)
        {
            var command = new CreateIngredient(ingredient.Name);
            var response = await _mediator.Send(command);

            return Created(string.Empty, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllIngredients()
        {
            var query = new GetAllIngredients();
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpGet]
        [Route("{recipeId}")]
        public async Task<IActionResult> GetIngredientsByRecipeId(long recipeId)
        {
            var query = new GetIngredientsByRecipeId(recipeId);
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> SearchIngredientsByName([FromQuery]string name)
        {
            var query = new SearchIngredientsByName(name);
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateIngredient(long id, IngredientRequestDto ingredient)
        {
            var command = new UpdateIngredient(id, ingredient.Name);
            var response = await _mediator.Send(command);

            return Ok(response);
        }
    }
}
