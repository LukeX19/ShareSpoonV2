using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareSpoon.Api.Extensions;
using ShareSpoon.App.RequestModels;
using ShareSpoon.App.Users.Commands;
using ShareSpoon.App.Users.Queries;
using ShareSpoon.Domain.Enums;

namespace ShareSpoon.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("current")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = HttpContext.GetUserIdClaimValue();
            var query = new GetUserById(userId);
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpGet]
        [Route("activity")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsersActivity([FromQuery] int daysNumber, [FromQuery] PagedRequestDto request)
        {
            var query = new GetUsersActivity(daysNumber, request.PageIndex, request.PageSize);
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpPut]
        [Route("changeRole")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserRole(UpdateUserRoleRequestDto request)
        {
            var query = new UpdateUserRole(request.UserId, request.Role);
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserRequestDto user)
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var command = new UpdateUser(userId, user.FirstName, user.LastName, user.Birthday, user.PictureURL);
            var response = await _mediator.Send(command);

            return Ok(response);
        }
    }
}
