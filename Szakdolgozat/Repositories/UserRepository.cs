using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Szakdolgozat.Models;
using MySql.Data.MySqlClient;

namespace Szakdolgozat.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public bool AuthenticateUser(NetworkCredential credential)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM felhasznalok WHERE felhasznalo_nev = @username AND jelszo = @password";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", credential.UserName);
                    command.Parameters.AddWithValue("@password", credential.Password);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }

        public Felhasznalo GetUserById(int id)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM felhasznalok WHERE id = @id";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int userid = Convert.ToInt32(reader["id"]);
                            string felhasznalonev = Convert.ToString(reader["felhasznalo_nev"]);
                            string jelszo = Convert.ToString(reader["jelszo"]);
                            int dolgozoID = Convert.ToInt32(reader["dolgozo_id"]);
                            Felhasznalo user = new Felhasznalo(userid, felhasznalonev, jelszo, dolgozoID);

                            return user;
                        }
                    }
                }
            }

            return null;
        }

        public Felhasznalo GetUserByUsername(string username)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM felhasznalok WHERE felhasznalo_nev = @username";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int userid = Convert.ToInt32(reader["id"]);
                            string felhasznalonev = Convert.ToString(reader["felhasznalo_nev"]);
                            string jelszo = Convert.ToString(reader["jelszo"]);
                            int dolgozoID = Convert.ToInt32(reader["dolgozo_id"]);
                            Felhasznalo user = new Felhasznalo(userid, felhasznalonev, jelszo, dolgozoID);

                            return user;
                        }
                    }
                }
            }

            return null;
        }
    }
}
