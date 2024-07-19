using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareSpoon.Api.Extensions;
using ShareSpoon.App.Recipes.Commands;
using ShareSpoon.App.Recipes.Queries;
using ShareSpoon.App.RequestModels;
using ShareSpoon.Domain.Enums;

namespace ShareSpoon.Api.Controllers
{
    [ApiController]
    [Route("api/recipes")]
    [Authorize]
    public class RecipesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RecipesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRecipe(CreateRecipeRequestDto recipe)
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var command = new CreateRecipe(userId, recipe.Name, recipe.Description, recipe.EstimatedTime,
                recipe.Difficulty, recipe.Ingredients, recipe.Tags, recipe.PictureURL);
            var response = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetRecipeById), new { recipeId = response.Id }, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRecipes([FromQuery] PagedRequestDto request)
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var query = new GetAllRecipes(userId, request.PageIndex, request.PageSize);
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpGet]
        [Route("{recipeId}")]
        public async Task<IActionResult> GetRecipeById(long recipeId)
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var query = new GetRecipeById(userId, recipeId);
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpGet]
        [Route("user/{userId}")]
        public async Task<IActionResult> GetRecipesByUserId([FromRoute] string userId, [FromQuery] PagedRequestDto request)
        {
            var query = new GetRecipesByUserId(userId, request.PageIndex, request.PageSize);
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpGet]
        [Route("user/{userId}/liked")]
        public async Task<IActionResult> GetLikedRecipesByUserId([FromRoute] string userId, [FromQuery] PagedRequestDto request)
        {
            var query = new GetLikedRecipesByUserId(userId, request.PageIndex, request.PageSize);
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> SearchRecipes([FromQuery] string? input, [FromQuery] bool? promotedUsers,
            [FromQuery] List<DifficultyLevel>? difficulties, [FromQuery] List<long>? tagIds, [FromQuery] PagedRequestDto request)
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var query = new SearchRecipes(input, promotedUsers, difficulties, tagIds, userId, request.PageIndex, request.PageSize);
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpPut]
        [Route("{recipeId}")]
        public async Task<IActionResult> UpdateRecipe(long recipeId, CreateRecipeRequestDto recipe)
        {
            var command = new UpdateRecipe(recipeId, recipe.Name, recipe.Description, recipe.EstimatedTime,
                recipe.Difficulty, recipe.Ingredients, recipe.Tags, recipe.PictureURL);
            var response = await _mediator.Send(command);

            return Ok(response);
        }

        [HttpDelete]
        [Route("{recipeId}")]
        public async Task<IActionResult> DeleteRecipe(long recipeId)
        {
            var command = new DeleteRecipe(recipeId);
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
