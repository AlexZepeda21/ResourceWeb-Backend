using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceWeb.Services.Register.Domain.Interfaces
{
    namespace ResourceWeb.Services.Register.Domain.Interfaces
    {
        public interface IPasswordHasher
        {
            string HashPassword(string password);
            bool VerifyPassword(string password, string hashedPassword);
        }
    }

}
