using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Szakdolgozat.Models
{
    public interface IUserRepository
    {
        bool AuthenticateUser(NetworkCredential credential);
        Felhasznalo GetUserById(int id);
        Felhasznalo GetUserByUsername(string username);
    }
}
