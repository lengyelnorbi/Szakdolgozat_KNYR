using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using Szakdolgozat.Models;

namespace Szakdolgozat.Repositories
{
    public class DolgozoRepository : RepositoryBase, IDolgozoRepository
    {
        public bool AddDolgozo(Dolgozo dolgozo)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = "INSERT INTO `dolgozok` (`id`, `vezeteknev`, `keresztnev`, `email`, `telefonszam`) VALUES (NULL, @vezeteknev, @keresztnev, @email, @telefonszam);";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@vezeteknev", dolgozo.Vezeteknev);
                    command.Parameters.AddWithValue("@keresztnev", dolgozo.Keresztnev);
                    command.Parameters.AddWithValue("@email", dolgozo.Email);
                    command.Parameters.AddWithValue("@telefonszam", dolgozo.Telefonszam);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }
        
        public bool DeleteDolgozo(int id)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = "DELETE FROM `dolgozok` WHERE `dolgozok`.`id` = @id;";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }

        public ObservableCollection<ModelContainer<object>> GetModelContainerDolgozok()
        {
            ObservableCollection<ModelContainer<object>> data = new ObservableCollection<ModelContainer<object>>();
            string query = "SELECT * FROM dolgozok;";

            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["id"]);
                            string vezeteknev = Convert.ToString(reader["vezeteknev"]);
                            string keresztnev = Convert.ToString(reader["keresztnev"]);
                            string email = Convert.ToString(reader["email"]);
                            string telefonszam = Convert.ToString(reader["telefonszam"]);

                            ModelContainer<object> item = new ModelContainer<object>(new Dolgozo(id, vezeteknev, keresztnev, email, telefonszam));

                            data.Add(item);
                        }
                    }
                }
            }
            return data;
        }

        public ObservableCollection<Dolgozo> GetDolgozok()
        {
            ObservableCollection<Dolgozo> data = new ObservableCollection<Dolgozo>();
            string query = "SELECT * FROM dolgozok;";

            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["id"]);
                            string vezeteknev = Convert.ToString(reader["vezeteknev"]);
                            string keresztnev = Convert.ToString(reader["keresztnev"]);
                            string email = Convert.ToString(reader["email"]);
                            string telefonszam = Convert.ToString(reader["telefonszam"]);

                            Dolgozo item = new Dolgozo(id, vezeteknev, keresztnev, email, telefonszam);

                            data.Add(item);
                        }
                    }
                }
            }
            return data;
        }

        public bool ModifyDolgozo(Dolgozo dolgozo)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = "UPDATE `dolgozok` SET `vezeteknev`='@vezeteknev',`keresztnev`='@keresztnev',`email`='@email',`telefonszam`='@telefonszam' WHERE `dolgozok`.`id` = '@id'";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@vezeteknev", dolgozo.Vezeteknev);
                    command.Parameters.AddWithValue("@keresztnev", dolgozo.Keresztnev);
                    command.Parameters.AddWithValue("@email", dolgozo.Email);
                    command.Parameters.AddWithValue("@telefonszam", dolgozo.Telefonszam);
                    command.Parameters.AddWithValue("@id", dolgozo.ID);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }
    }
}
