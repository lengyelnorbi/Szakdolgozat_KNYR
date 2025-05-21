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
            try
            {
                using (MySqlConnection connection = GetConnection())
                {
                    connection.Open();

                    string query = "INSERT INTO `maganszemelyek` (`id`, `nev`, `telefonszam`, `email`, `lakcim`) VALUES (NULL, @nev, @telefonszam, @email, @lakcim);";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nev", maganSzemely.Nev);
                        command.Parameters.AddWithValue("@telefonszam", maganSzemely.Telefonszam);
                        command.Parameters.AddWithValue("@email", maganSzemely.Email);
                        command.Parameters.AddWithValue("@lakcim", maganSzemely.Lakcim);

                        int count = Convert.ToInt32(command.ExecuteNonQuery());

                        return count > 0;
                    }
                }
            }
            catch(MySqlException ex)
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

        public bool DeleteMaganSzemely(int id)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = "DELETE FROM `maganszemelyek` WHERE `maganszemelyek`.`id` = @id;";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }
        public bool DeleteMaganSzemely(int id, bool confirmCascade = true)
        {
            // First check if there are related records in bevetelek_kiadasok
            List<string> relatedRecords = new List<string>();
            int count = 0;

            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                // Check for related records
                string checkQuery = "SELECT COUNT(*) FROM bevetelek_kiadasok WHERE magan_szemely_id = @id";
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
                            string deleteRelatedQuery = "DELETE FROM bevetelek_kiadasok WHERE magan_szemely_id = @id";
                            using (MySqlCommand command = new MySqlCommand(deleteRelatedQuery, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@id", id);
                                command.ExecuteNonQuery();
                            }
                        }

                        // Then delete the main record
                        string deleteQuery = "DELETE FROM maganszemelyek WHERE id = @id";
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
        public ObservableCollection<MaganSzemely> GetMaganSzemelyek()
        {
            ObservableCollection<MaganSzemely> data = new ObservableCollection<MaganSzemely>();
            string query = "SELECT * FROM maganszemelyek;";

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
            try
            {
                using (MySqlConnection connection = GetConnection())
                {
                    connection.Open();

                    string query = "UPDATE `maganszemelyek` SET `nev`=@nev,`telefonszam`=@telefonszam,`email`=@email,`lakcim`=@lakcim WHERE `maganszemelyek`.`id` = @id";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nev", maganSzemely.Nev);
                        command.Parameters.AddWithValue("@telefonszam", maganSzemely.Telefonszam);
                        command.Parameters.AddWithValue("@email", maganSzemely.Email);
                        command.Parameters.AddWithValue("@lakcim", maganSzemely.Lakcim);
                        command.Parameters.AddWithValue("@id", maganSzemely.ID);

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

                // Check for related records in bevetelek_kiadasok (via partner_id)
                string checkBevetelekQuery = "SELECT COUNT(*) FROM bevetelek_kiadasok WHERE magan_szemely_id = @id";
                using (MySqlCommand command = new MySqlCommand(checkBevetelekQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    if (count > 0)
                    {
                        relatedRecords.Add($"• Költségvetés bejegyzések: {count} db");
                    }
                }

                // Check for related records in other tables as needed
                // For example, if other tables reference maganszemelyek
            }

            relatedInfo = string.Join("\n", relatedRecords);
            return relatedRecords.Count > 0;
        }
    }
}
