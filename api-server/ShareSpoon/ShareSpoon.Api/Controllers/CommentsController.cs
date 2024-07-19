using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareSpoon.Api.Extensions;
using ShareSpoon.App.Comments.Commands;
using ShareSpoon.App.Comments.Queries;
using ShareSpoon.App.RequestModels;

namespace ShareSpoon.Api.Controllers
{
    [ApiController]
    [Route("api/comments")]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateCommentRequestDto comment)
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var command = new CreateComment(userId, comment.RecipeId, comment.Text);
            var response = await _mediator.Send(command);

            return Created(string.Empty, response);
        }

        [HttpGet]
        [Route("{recipeId}")]
        public async Task<IActionResult> GetCommentsByRecipeId(long recipeId, [FromQuery]PagedRequestDto request)
        {
            var query = new GetCommentsByRecipeId(recipeId, request.PageIndex, request.PageSize);
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateComment(UpdateCommentRequestDto comment)
        {
            var command = new UpdateComment(comment.Id, comment.Text);
            var response = await _mediator.Send(command);

            return Ok(response);
        }

        [HttpDelete]
        [Route("{commentId}")]
        public async Task<IActionResult> DeleteComment(long commentId)
        {
            var command = new DeleteComment(commentId);
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
