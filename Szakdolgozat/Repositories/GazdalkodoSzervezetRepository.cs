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
        public bool DeleteGazdalkodoSzervezet(int id, bool confirmCascade = true)
        {
            // First check if there are related records in bevetelek_kiadasok
            List<string> relatedRecords = new List<string>();
            int count = 0;

            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                // Check for related records
                string checkQuery = "SELECT COUNT(*) FROM bevetelek_kiadasok WHERE gazdalkodo_szerv_id = @id";
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
                            string deleteRelatedQuery = "DELETE FROM bevetelek_kiadasok WHERE gazdalkodo_szerv_id = @id";
                            using (MySqlCommand command = new MySqlCommand(deleteRelatedQuery, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@id", id);
                                command.ExecuteNonQuery();
                            }
                        }

                        // Then delete the main record
                        string deleteQuery = "DELETE FROM gazdalkodo_szervezetek WHERE id = @id";
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

        public bool CheckForRelatedRecords(int id, out string relatedInfo)
        {
            List<string> relatedRecords = new List<string>();

            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                // Check for related records in bevetelek_kiadasok (via partner_id)
                string checkBevetelekQuery = "SELECT COUNT(*) FROM bevetelek_kiadasok WHERE gazdalkodo_szerv_id = @id";
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
                // For example, if other tables reference magan_szemelyek
            }

            relatedInfo = string.Join("\n", relatedRecords);
            return relatedRecords.Count > 0;
        }
    }
}
