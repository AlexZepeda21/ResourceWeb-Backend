using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceWeb.Services.Register.Application.DTOs;
using ResourceWeb.Services.Register.Application.Features.Users.Queries.GetUserProfile;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ResourceWeb.Services.Register.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize] 
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("profile")]
        public async Task<ActionResult<UserProfileDto>> GetProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Token inválido");
            }

            var query = new GetUserProfileQuery(userId);
            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}