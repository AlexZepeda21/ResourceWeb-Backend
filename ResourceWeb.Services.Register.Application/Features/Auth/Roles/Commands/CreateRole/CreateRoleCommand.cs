using MediatR;
using ResourceWeb.Services.Register.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceWeb.Services.Register.Application.Features.Auth.Roles.Commands.CreateRole
{
    public class CreateRoleCommand : IRequest<RoleDto>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
