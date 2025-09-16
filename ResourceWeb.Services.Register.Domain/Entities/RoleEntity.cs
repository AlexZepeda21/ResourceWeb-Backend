using ResourceWeb.Services.Register.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceWeb.Services.Register.Domain.Entities
{
    public class RoleEntity : BaseEntity
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public bool IsActive { get; protected set; } = true;

        public ICollection<UserEntity> Users { get; protected set; }

        public RoleEntity(string name, string description)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));

        }
    }
}
