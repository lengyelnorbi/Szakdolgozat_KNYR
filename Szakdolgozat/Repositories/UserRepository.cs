using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Szakdolgozat.Models;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Xaml.Behaviors.Media;
using Szakdolgozat.Specials;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Szakdolgozat.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public (bool, int, string) AuthenticateUser(NetworkCredential credential)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = "SELECT id, szerep FROM felhasznalok WHERE felhasznalonev = @username AND jelszo = @password";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", credential.UserName);
                    command.Parameters.AddWithValue("@password", PasswordHasher.HashPassword(credential.Password));

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int userId = Convert.ToInt32(reader["id"]);
                            string role = Convert.ToString(reader["szerep"]);
                            return (true, userId, role);
                        }
                    }
                    return (false, 0, null);
                }
            }
        }

        public ObservableCollection<string> GetColumnNamesForTables(string tableName)
        {
            using(MySqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = "SELECT column_name FROM INFORMATION_SCHEMA.COLUMNS WHERE table_schema = @db AND table_name = @tableName;";

                using(MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@db", "nyilvantarto_rendszer");
                    command.Parameters.AddWithValue("@tableName", tableName);

                    using(MySqlDataReader reader = command.ExecuteReader())
                    {
                        ObservableCollection<string> strings = new ObservableCollection<string>();
                        while (reader.Read())
                        {
                            string columnName = Convert.ToString(reader["column_name"]);
                            strings.Add(columnName);
                        }
                        return strings;
                    }
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
                            string felhasznalonev = Convert.ToString(reader["felhasznalonev"]);
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

                string query = "SELECT * FROM felhasznalok WHERE felhasznalonev = @username";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int userid = Convert.ToInt32(reader["id"]);
                            string felhasznalonev = Convert.ToString(reader["felhasznalonev"]);
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

        public (bool, string, string) CreateAndAddUser(Dolgozo dolgozo)
        {
            try
            {
                using (MySqlConnection connection = GetConnection())
                {
                    connection.Open();

                    string query = "INSERT INTO `felhasznalok` (`id`, `felhasznalonev`, `jelszo`, `dolgozo_id`, `szerep`) VALUES (NULL, @username, @password, @workerID, 'USER');";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        string username = CreateUserName(dolgozo.Vezeteknev, dolgozo.Keresztnev);
                        string password = CreatePassword();
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", PasswordHasher.HashPassword(password));
                        command.Parameters.AddWithValue("@workerID", dolgozo.ID);

                        int count = command.ExecuteNonQuery();

                        return (count > 0, username, password);
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Handle MySQL-specific exceptions
                Debug.WriteLine($"MySQL Error: {ex.Message}");
                return (false, "", "");
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Debug.WriteLine($"General Error: {ex.Message}");
                return (false, "", "");
            }
        }

        private string CreateUserName(string lastname, string firstname)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                // Take first 3 characters of lastname and firstname, or less if shorter
                string lastPart = lastname.Length >= 3 ? lastname.Substring(0, 3) : lastname;
                string firstPart = firstname.Length >= 3 ? firstname.Substring(0, 3) : firstname;
                string baseUsername = (lastPart + firstPart).ToLower();
                string username = baseUsername;
                int suffix = 1;

                string query = "SELECT COUNT(*) FROM felhasznalok WHERE felhasznalonev = @username";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    while (count > 0)
                    {
                        username = baseUsername + suffix.ToString();
                        command.Parameters["@username"].Value = username;
                        count = Convert.ToInt32(command.ExecuteScalar());
                        suffix++;
                    }
                }
                return username;
            }
        }

        private string CreatePassword()
        {
            // Generate a random password
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            string password;

            // Keep generating passwords until one meets our criteria
            do
            {
                char[] passwordChars = new char[10];
                for (int i = 0; i < passwordChars.Length; i++)
                {
                    passwordChars[i] = chars[random.Next(chars.Length)];
                }
                password = new string(passwordChars);
            }
            while (!HasLowerChar(password) || !HasUpperChar(password) || !HasNumber(password));

            return password;
        }

        private bool HasLowerChar(string str)
        {
            return str.Any(c => char.IsLower(c));
        }

        private bool HasUpperChar(string str)
        {
            return str.Any(c => char.IsUpper(c));
        }

        private bool HasNumber(string str)
        {
            return str.Any(c => char.IsDigit(c));
        }
    }
}
