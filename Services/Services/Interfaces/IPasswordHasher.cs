using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Interfaces
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        string GenerateSalt();
        bool VerifyPassword(string password, string hashedPassword, string salt);
    }
}
