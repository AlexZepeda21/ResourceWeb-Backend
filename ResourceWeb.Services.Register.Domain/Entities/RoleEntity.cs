using ResourceWeb.Services.Register.Domain.Common;

namespace ResourceWeb.Services.Register.Domain.Entities
{
    public class RoleEntity : BaseEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }

        // Propiedad de navegación para la relación uno-a-muchos
        public virtual ICollection<UserEntity> Users { get; private set; } = new List<UserEntity>();

        // Constructor sin parámetros para Entity Framework
        protected RoleEntity() { }

        // Constructor principal
        public RoleEntity(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre del rol no puede estar vacío", nameof(name));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("La descripción del rol no puede estar vacía", nameof(description));

            Name = name;
            Description = description;
            IsActive = true;
            Users = new List<UserEntity>();
        }

        // Métodos públicos para modificar la entidad
        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre del rol no puede estar vacío", nameof(name));

            Name = name;
            SetUpdatedAt();
        }

        public void UpdateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("La descripción del rol no puede estar vacía", nameof(description));

            Description = description;
            SetUpdatedAt();
        }

        public void Activate()
        {
            IsActive = true;
            SetUpdatedAt();
        }

        public void Deactivate()
        {
            IsActive = false;
            SetUpdatedAt();
        }
    }
}