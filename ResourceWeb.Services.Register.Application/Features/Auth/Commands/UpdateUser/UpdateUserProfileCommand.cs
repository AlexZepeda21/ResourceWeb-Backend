using MediatR;
using ResourceWeb.Services.Register.Application.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceWeb.Services.Register.Application.Features.Auth.Commands.UpdateUser
{
    public class UpdateUserProfileCommand : IRequest<UserProfileDto>
    {
        public Guid UserId { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string UserName { get; set; }
        public DateTime? Birthdate { get; set; }

    }
   
}
