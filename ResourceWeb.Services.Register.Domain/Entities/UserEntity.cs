using ResourceWeb.Services.Register.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceWeb.Services.Register.Domain.Entities
{
    public class UserEntity : BaseEntity
    {
        public string UserName { get; protected set; }
        public string Email { get; protected set; }
        public bool EmailConfirmed { get; protected set; }
        public string PasswordHash { get; protected set; }
        public DateTime? Birthdate { get; protected set; }
        public bool IsActive { get; protected set; } = true;
        public Guid RoleId { get; protected set; }
        public RoleEntity Role { get; protected set; }

        public UserEntity(string userName, string email, Guid roleId)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            RoleId = roleId;
        }
    }
}
