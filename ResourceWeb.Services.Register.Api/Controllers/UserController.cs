using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceWeb.Services.Register.Application.DTOs;
using ResourceWeb.Services.Register.Application.Features.Auth.Commands.UpdateUser;
using ResourceWeb.Services.Register.Application.Features.Users.Queries.GetUserProfile;
using ResourceWeb.Services.Register.Application.Features.Auth.Commands.UpdateUser;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
            //Obtener siempre el id desde jwt(Hace requerido q dicho id sea enviado en el encabezado, JWT )
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //Esto es una excepción en caso de que el id no sea encontrado o lo que se reciba por token no sea valido
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Token inválido");
            }


            var query = new GetUserProfileQuery(userId);
            var result = await _mediator.Send(query);
            
            return Ok(result);
        }

        [HttpPut("profile")]
        public async Task<ActionResult<UserProfileDto>> UpdateProfile([FromBody] UpdateUserProfileCommand command)
        {
            // Obtener el ID del usuario desde el token JWT
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Token inválido");
            }

            command.UserId = userId;

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPost("profile/image")]
        public async Task<ActionResult<ProfileImageResponseDto>> UploadProfileImage([FromBody] UploadProfileImageCommand command)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Token inválido");
            }

            command.UserId = userId;

            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("profile/image")]
        public async Task<ActionResult> RemoveProfileImage()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Token inválido");
            }

            var command = new RemoveProfileImageCommand { UserId = userId };
            var success = await _mediator.Send(command);

            if (!success)
                return BadRequest("No se pudo eliminar la imagen de perfil");

            return NoContent();
        }
    }
}