using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareSpoon.App.Ingredients.Queries;
using ShareSpoon.App.RequestModels;
using ShareSpoon.App.Tags.Commands;
using ShareSpoon.App.Tags.Queries;

namespace ShareSpoon.Api.Controllers
{
    [ApiController]
    [Route("api/tags")]
    [Authorize]
    public class TagsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TagsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTag(TagRequestDto tag)
        {
            var command = new CreateTag(tag.Name, tag.Type);
            var response = await _mediator.Send(command);

            return Created(string.Empty, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTags()
        {
            var query = new GetAllTags();
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpGet]
        [Route("filter")]
        public async Task<IActionResult> GetFilterTags()
        {
            var query = new GetFilterTags();
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> SearchTagsByName([FromQuery] string name)
        {
            var query = new SearchTagsByName(name);
            var response = await _mediator.Send(query);

            return Ok(response);
        }
    }
}
