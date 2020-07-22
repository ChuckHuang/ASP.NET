using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStore.Core.ServiceInterfaces
{
    public interface ICryptoService
    {
        string GenerateSalt();
        string HashPassword(string password, string salt);
    }
}
