using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareSpoon.App.Files.Commands;
using ShareSpoon.App.RequestModels;

namespace ShareSpoon.Api.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FilesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FilesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadFile([FromForm]FileRequestDto request)
        {
            var command = new UploadFile(request.File);
            var response = await _mediator.Send(command);

            return Created(string.Empty, response);
        }

        [HttpDelete]
        [Route("delete")]
        [Authorize]
        public async Task<IActionResult> DeleteFile(string fileName)
        {
            var command = new DeleteFile(fileName);
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
