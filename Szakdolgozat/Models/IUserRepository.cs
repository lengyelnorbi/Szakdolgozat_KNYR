using System.Collections.ObjectModel;
using System.Net;

namespace Szakdolgozat.Models
{
    public interface IUserRepository
    {
        (bool,int, string) AuthenticateUser(NetworkCredential credential);
        Felhasznalo GetUserById(int id);
        Felhasznalo GetUserByUsername(string username);
        ObservableCollection<string> GetColumnNamesForTables(string tableName);
    }
}
