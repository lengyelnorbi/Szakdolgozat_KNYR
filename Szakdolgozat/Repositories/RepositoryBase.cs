using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdolgozat.Repositories
{
    public abstract class RepositoryBase
    {
        private readonly string _connectionString;
        public RepositoryBase()
        {
            //_connectionString = "Server=localhost;Database=nyilvantarto_rendszer;User=root;Password=";
            _connectionString = "Server=localhost;Database=nyilvantarto_rendszer;User=Norbi;Password=-j@DoZ*S-_7w@EP";
            //_connectionString = "Server=localhost;Database=nyilvantarto_rendszer;User=Norbi;Password=";
        }
        protected MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
