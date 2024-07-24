using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Szakdolgozat.Models;

namespace Szakdolgozat.Repositories
{
    public class GazdalkodoSzervezetRepository : RepositoryBase, IGazdalkodoSzervezetRepository
    {
        public bool DeleteGazdalkodoSzervezet(int id)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = "DELETE FROM `gazdalkodo_szervezetek` WHERE `gazdalkodo_szervezetek`.`id` = @id;";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }

        public bool AddGazdalkodoSzervezet(GazdalkodoSzervezet gazdalkodoSzervezet)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = "INSERT INTO `gazdalkodo_szervezetek` (`id`, `nev`, `kapcsolattarto`, `email`, `telefonszam`) VALUES (NULL, @nev, @kapcsolattarto, @email, @telefonszam);";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nev", gazdalkodoSzervezet.Nev);
                    command.Parameters.AddWithValue("@kapcsolattarto", gazdalkodoSzervezet.Kapcsolattarto);
                    command.Parameters.AddWithValue("@email", gazdalkodoSzervezet.Email);
                    command.Parameters.AddWithValue("@telefonszam", gazdalkodoSzervezet.Telefonszam);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }

        public ObservableCollection<GazdalkodoSzervezet> GetGazdalkodoSzervezetek()
        {
            ObservableCollection<GazdalkodoSzervezet> data = new ObservableCollection<GazdalkodoSzervezet>();
            string query = "SELECT * FROM gazdalkodo_szervezetek;";

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
                            string nev = Convert.ToString(reader["nev"]);
                            string kapcsolattarto = Convert.ToString(reader["kapcsolattarto"]);
                            string email = Convert.ToString(reader["email"]);
                            string telefonszam = Convert.ToString(reader["telefonszam"]);

                            GazdalkodoSzervezet item = new GazdalkodoSzervezet(id, nev, kapcsolattarto, email, telefonszam);

                            data.Add(item);
                        }
                    }
                }
            }
            return data;
        }

        public bool ModifyGazdalkodoSzervezet(GazdalkodoSzervezet gazdalkodoSzervezet)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = "UPDATE `gazdalkodo_szervezetek` SET `nev`=@nev,`kapcsolattarto`=@kapcsolattarto,`email`=@email,`telefonszam`=@telefonszam WHERE `gazdalkodo_szervezetek`.`id` = @id";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nev", gazdalkodoSzervezet.Nev);
                    command.Parameters.AddWithValue("@kapcsolattarto", gazdalkodoSzervezet.Kapcsolattarto);
                    command.Parameters.AddWithValue("@email", gazdalkodoSzervezet.Email);
                    command.Parameters.AddWithValue("@telefonszam", gazdalkodoSzervezet.Telefonszam);
                    command.Parameters.AddWithValue("@id", gazdalkodoSzervezet.ID);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }
    }
}
