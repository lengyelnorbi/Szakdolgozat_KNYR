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
    public class MaganSzemelyRepository : RepositoryBase, IMaganSzemelyRepository
    {
        public bool AddMaganSzemely(MaganSzemely maganSzemely)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = "INSERT INTO `magan_szemelyek` (`id`, `nev`, `telefonszam`, `email`, `lakcim`) VALUES (NULL, @nev, @telefonszam, @email, @lakcim);";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nev", maganSzemely.Nev);
                    command.Parameters.AddWithValue("@telefonszam", maganSzemely.Telefonszam);
                    command.Parameters.AddWithValue("@email", maganSzemely.Email);
                    command.Parameters.AddWithValue("@lakcim", maganSzemely.Lakcim);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }

        public bool DeleteMaganSzemely(int id)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = "DELETE FROM `magan_szemelyek` WHERE `magan_szemelyek`.`id` = @id;";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }

        public ObservableCollection<MaganSzemely> GetMaganSzemelyek()
        {
            ObservableCollection<MaganSzemely> data = new ObservableCollection<MaganSzemely>();
            string query = "SELECT * FROM magan_szemelyek;";

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
                            string telefonszam = Convert.ToString(reader["telefonszam"]);
                            string email = Convert.ToString(reader["email"]);
                            string lakcim = Convert.ToString(reader["lakcim"]);

                            MaganSzemely item = new MaganSzemely(id, nev, telefonszam, email, lakcim);

                            data.Add(item);
                        }
                    }
                }
            }
            return data;
        }

        public bool ModifyMaganSzemely(MaganSzemely maganSzemely)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = "UPDATE `magan_szemelyek` SET `nev`=@nev,`telefonszam`=@telefonszam,`email`=@email,`lakcim`=@lakcim WHERE `magan_szemelyek`.`id` = @id";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nev", maganSzemely.Nev);
                    command.Parameters.AddWithValue("@telefonszam", maganSzemely.Telefonszam);
                    command.Parameters.AddWithValue("@email", maganSzemely.Email);
                    command.Parameters.AddWithValue("@lakcim", maganSzemely.Lakcim);
                    command.Parameters.AddWithValue("@id", maganSzemely.ID);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }
    }
}
