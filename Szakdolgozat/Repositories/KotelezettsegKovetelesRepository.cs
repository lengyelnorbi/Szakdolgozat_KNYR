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
            try
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

                        int count = Convert.ToInt32(command.ExecuteNonQuery());

                        return count > 0;
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Handle MySQL-specific exceptions
                Console.WriteLine($"MySQL Error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine($"General Error: {ex.Message}");
                return false;
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
        public bool DeleteKotelezettsegKoveteles(int id, bool confirmCascade = true)
        {
            // First check if there are related records in bevetelek_kiadasok
            List<string> relatedRecords = new List<string>();
            int count = 0;

            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                // Check for related records
                string checkQuery = "SELECT COUNT(*) FROM bevetelek_kiadasok WHERE kotel_kovet_id = @id";
                using (MySqlCommand command = new MySqlCommand(checkQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    count = Convert.ToInt32(command.ExecuteScalar());

                    if (count > 0)
                    {
                        relatedRecords.Add($"Költségvetés bejegyzések: {count} db");
                    }
                }

                // If there are related records and confirmation is needed, return false
                if (relatedRecords.Count > 0 && confirmCascade == false)
                {
                    return false;
                }

                // Perform the delete with CASCADE option or manually delete related records
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // First delete related records
                        if (count > 0)
                        {
                            string deleteRelatedQuery = "DELETE FROM bevetelek_kiadasok WHERE kotel_kovet_id = @id";
                            using (MySqlCommand command = new MySqlCommand(deleteRelatedQuery, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@id", id);
                                command.ExecuteNonQuery();
                            }
                        }

                        // Then delete the main record
                        string deleteQuery = "DELETE FROM kotelezettsegek_kovetelesek WHERE id = @id";
                        using (MySqlCommand command = new MySqlCommand(deleteQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@id", id);
                            int result = command.ExecuteNonQuery();
                            transaction.Commit();
                            return result > 0;
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
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
            try
            {
                using (MySqlConnection connection = GetConnection())
                {
                    connection.Open();

                    string query = "UPDATE `kotelezettsegek_kovetelesek` SET `tipus`=@tipus,`osszeg`=@osszeg,`penznem`=@penznem,`kifizetes_hatarideje`=@kifizetesHatarideje,`kifizetett`=@kifizetett WHERE `kotelezettsegek_kovetelesek`.`id` = @id";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        //MessageBox.Show(kotelezettsegKoveteles.Kifizetett.ToString());
                        command.Parameters.AddWithValue("@osszeg", kotelezettsegKoveteles.Osszeg);
                        command.Parameters.AddWithValue("@tipus", kotelezettsegKoveteles.Tipus);
                        command.Parameters.AddWithValue("@penznem", kotelezettsegKoveteles.Penznem);
                        command.Parameters.AddWithValue("@kifizetesHatarideje", kotelezettsegKoveteles.KifizetesHatarideje);
                        command.Parameters.AddWithValue("@kifizetett", kotelezettsegKoveteles.Kifizetett);
                        command.Parameters.AddWithValue("@id", kotelezettsegKoveteles.ID);
                        //MessageBox.Show(command.ToString());
                        int count = Convert.ToInt32(command.ExecuteNonQuery());

                        return count > 0;
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Handle MySQL-specific exceptions
                Console.WriteLine($"MySQL Error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine($"General Error: {ex.Message}");
                return false;
            }
        }

        public bool CheckForRelatedRecords(int id, out string relatedInfo)
        {
            List<string> relatedRecords = new List<string>();

            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                // Check bevetelek_kiadasok
                string checkQuery = "SELECT COUNT(*) FROM bevetelek_kiadasok WHERE kotel_kovet_id = @id";
                using (MySqlCommand command = new MySqlCommand(checkQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    if (count > 0)
                    {
                        relatedRecords.Add($"• Költségvetés bejegyzések: {count} db");
                    }
                }

                // Add other related table checks as needed
            }

            relatedInfo = string.Join("\n", relatedRecords);
            return relatedRecords.Count > 0;
        }
    }
}
