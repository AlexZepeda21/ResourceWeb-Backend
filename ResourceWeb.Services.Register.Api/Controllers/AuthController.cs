using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResourceWeb.Services.Register.Application.DTOs;
using ResourceWeb.Services.Register.Application.Features.Auth.Commands.RegisterUser;
using ResourceWeb.Services.Register.Application.Features.Auth.Queries.LoginUser;

namespace ResourceWeb.Services.Register.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDto>> Register([FromBody] RegisterUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginUserQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
