using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareSpoon.Api.Extensions;
using ShareSpoon.App.Likes.Commands;
using ShareSpoon.App.Likes.Queries;
using ShareSpoon.App.RequestModels;

namespace ShareSpoon.Api.Controllers
{
    [ApiController]
    [Route("api/likes")]
    [Authorize]
    public class LikesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LikesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLike(LikeRequestDto like)
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var command = new CreateLike(userId, like.RecipeId);
            var response = await _mediator.Send(command);

            return Created(string.Empty, response);
        }

        [HttpGet]
        [Route("{recipeId}")]
        public async Task<IActionResult> GetLikesCounterByRecipeId(long recipeId)
        {
            var query = new GetLikesCounterByRecipeId(recipeId);
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpDelete]
        [Route("{recipeId}")]
        public async Task<IActionResult> DeleteLike(long recipeId)
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var command = new DeleteLike(userId, recipeId);
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
