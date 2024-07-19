using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareSpoon.Api.Extensions;
using ShareSpoon.App.Auth.Commands;
using ShareSpoon.App.RequestModels;

namespace ShareSpoon.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterUserRequestDto user)
        {
            var command = new RegisterUser(user.FirstName, user.LastName, user.Birthday, user.PictureURL, user.Email, user.Password, user.Role);
            var response = await _mediator.Send(command);

            return Ok(response);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginUserRequestDto userInfo)
        {
            var command = new LoginUser(userInfo.Email, userInfo.Password);
            var response = await _mediator.Send(command);

            return Ok(response);
        }

        [HttpDelete]
        [Route("delete")]
        [Authorize]
        public async Task<IActionResult> DeleteUser()
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var command = new DeleteUser(userId);
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
