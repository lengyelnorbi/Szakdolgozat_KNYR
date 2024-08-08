using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Szakdolgozat.Models;

namespace Szakdolgozat.Repositories
{
    public class KotelezettsegKovetelesRepository : RepositoryBase, IKotelezettsegKovetelesRepository
    {
        public bool AddKotelezettsegKoveteles(KotelezettsegKoveteles kotelezettsegKoveteles)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = "INSERT INTO `kotelezettsegek_kovetelesek` (`id`, `tipus`, `osszeg`, `penznem`, `kifizetes_hatarideje`, `kifizetett`) VALUES (NULL, @tipus, @osszeg, @penznem, @kifizetesHatarideje, @kifizetett);";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@tipus", kotelezettsegKoveteles.Tipus);
                    command.Parameters.AddWithValue("@osszeg", kotelezettsegKoveteles.Osszeg);
                    command.Parameters.AddWithValue("@penznem", kotelezettsegKoveteles.Penznem);
                    command.Parameters.AddWithValue("@kifizetesHatarideje", kotelezettsegKoveteles.KifizetesHatarideje);
                    command.Parameters.AddWithValue("@kifizetett", kotelezettsegKoveteles.Kifizetett);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }

        public bool DeleteKotelezettsegKoveteles(int id)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = "DELETE FROM `kotelezettsegek_kovetelesek` WHERE `kotelezettsegek_kovetelesek`.`id` = @id;";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }

        public ObservableCollection<KotelezettsegKoveteles> GetKotelezettsegekKovetelesek()
        {
            ObservableCollection<KotelezettsegKoveteles> data = new ObservableCollection<KotelezettsegKoveteles>();
            string query = "SELECT * FROM kotelezettsegek_kovetelesek;";

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
                            string tipus = Convert.ToString(reader["tipus"]);
                            int osszeg = Convert.ToInt32(reader["osszeg"]);
                            Penznem penznem;
                            if (reader["penznem"] != DBNull.Value)
                            {
                                penznem = (Penznem)Enum.Parse(typeof(Penznem), (string)reader["penznem"], true);
                            }
                            else
                            {
                                throw new Exception("ERROR");
                            }
                            DateTime kifizetesHatarideje = Convert.ToDateTime(reader["kifizetes_hatarideje"]);
                            Int16 kifizetett = Convert.ToInt16(reader["kifizetett"]);

                            KotelezettsegKoveteles item = new KotelezettsegKoveteles(id, tipus, osszeg, penznem, kifizetesHatarideje, kifizetett);

                            data.Add(item);
                        }
                    }
                }
            }
            return data;
        }

        public bool ModifyKotelezettsegKoveteles(KotelezettsegKoveteles kotelezettsegKoveteles)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = "UPDATE `kotelezettsegek_kovetelesek` SET `tipus`=@tipus,`osszeg`=@osszeg,`penznem`=@penznem,`kifizetes_hatarideje`=@kifizetesHatarideje,`kifizetett`=@kifizetett WHERE `kotelezettsegek_kovetelesek`.`id` = @id";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    MessageBox.Show(kotelezettsegKoveteles.Kifizetett.ToString());
                    command.Parameters.AddWithValue("@osszeg", kotelezettsegKoveteles.Osszeg);
                    command.Parameters.AddWithValue("@tipus", kotelezettsegKoveteles.Tipus);
                    command.Parameters.AddWithValue("@penznem", kotelezettsegKoveteles.Penznem);
                    command.Parameters.AddWithValue("@kifizetesHatarideje", kotelezettsegKoveteles.KifizetesHatarideje);
                    command.Parameters.AddWithValue("@kifizetett", kotelezettsegKoveteles.Kifizetett);
                    command.Parameters.AddWithValue("@id", kotelezettsegKoveteles.ID);
                    MessageBox.Show(command.ToString());
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }
    }
}
