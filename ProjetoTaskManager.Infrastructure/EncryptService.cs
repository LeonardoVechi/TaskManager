using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoTaskManager.Application.Interfaces;

namespace ProjetoTaskManager.Infrastructure
{
    public class EncryptService : IEncryptService
    {  
        public string Hash(string value) =>
            BCrypt.Net.BCrypt.HashPassword(value);

        public bool Verify(string value, string hash) =>
            BCrypt.Net.BCrypt.Verify(value, hash);
    }
}