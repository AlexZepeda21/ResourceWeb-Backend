using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceWeb.Services.Register.Application.Features.Auth.Commands.UpdateUser
{
    public class RemoveProfileImageCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }
    }
}
