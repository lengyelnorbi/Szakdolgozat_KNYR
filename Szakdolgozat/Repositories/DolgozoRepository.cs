using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Szakdolgozat.Models;
using Szakdolgozat.ViewModels;

namespace Szakdolgozat.Repositories
{
    public class DolgozoRepository : RepositoryBase, IDolgozoRepository
    {
        public (bool, int) AddDolgozo(Dolgozo dolgozo)
        {
            int newId = 0;
            try
            {
                using (MySqlConnection connection = GetConnection())
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO `dolgozok` (`vezeteknev`, `keresztnev`, `email`, `telefonszam`) VALUES (@vezeteknev, @keresztnev, @email, @telefonszam);";
                    using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@vezeteknev", dolgozo.Vezeteknev);
                        command.Parameters.AddWithValue("@keresztnev", dolgozo.Keresztnev);
                        command.Parameters.AddWithValue("@email", dolgozo.Email);
                        command.Parameters.AddWithValue("@telefonszam", dolgozo.Telefonszam);

                        int affectedRows = command.ExecuteNonQuery();
                        if (affectedRows > 0)
                        {
                            // Get the new ID
                            command.CommandText = "SELECT LAST_INSERT_ID();";
                            command.Parameters.Clear();
                            newId = Convert.ToInt32(command.ExecuteScalar());
                            return (true, newId);
                        }
                        return (false, newId);
                    }
                }
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine($"MySQL Error: {ex.Message}");
                return (false, newId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"General Error: {ex.Message}");
                return (false, newId);
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
        public bool DeleteDolgozo(int id, bool confirmCascade = true)
        {
            // Check if there are related records
            List<string> relatedRecords = new List<string>();
            int diagrammCount = 0;

            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                int felhasznaloId = 0;
                // Check for related felhasznalok records
                string getFelhasznaloIdQuery = "SELECT id FROM felhasznalok WHERE dolgozo_id = @id";
                using (MySqlCommand command = new MySqlCommand(getFelhasznaloIdQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    felhasznaloId = Convert.ToInt32(command.ExecuteScalar());
                }

                // Check for related diagramm records
                string checkDiagrammQuery = "SELECT COUNT(*) FROM diagrammok WHERE letrehozo_id = @id";
                using (MySqlCommand command = new MySqlCommand(checkDiagrammQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", felhasznaloId);
                    diagrammCount = Convert.ToInt32(command.ExecuteScalar());

                    if (diagrammCount > 0)
                    {
                        relatedRecords.Add($"Diagramm bejegyzések: {diagrammCount} db");
                    }
                }

                // If there are related records and confirmation is needed, return false
                if (felhasznaloId > 0 && confirmCascade == false)
                {
                    return false;
                }
                // Perform the delete with CASCADE option or manually delete related records
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // First delete related diagramm records
                        if (diagrammCount > 0)
                        {
                            string deleteDiagrammQuery = "DELETE FROM diagrammok WHERE letrehozo_id = @id";
                            using (MySqlCommand command = new MySqlCommand(deleteDiagrammQuery, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@id", felhasznaloId);
                                command.ExecuteNonQuery();
                            }
                        }

                        // Then delete related felhasznalok records
                        if (felhasznaloId > 0)
                        {
                            string deleteFelhasznalokQuery = "DELETE FROM felhasznalok WHERE dolgozo_id = @id";
                            using (MySqlCommand command = new MySqlCommand(deleteFelhasznalokQuery, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@id", id);
                                command.ExecuteNonQuery();
                            }
                        }

                        // Finally delete the main record
                        string deleteQuery = "DELETE FROM dolgozok WHERE id = @id";
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
        public ObservableCollection<Dolgozo> GetDolgozok()
        {
            ObservableCollection<Dolgozo> data = new ObservableCollection<Dolgozo>();
            string query = @"SELECT d.* FROM dolgozok d 
                 WHERE d.id NOT IN (
                     SELECT dolgozo_id FROM felhasznalok 
                     WHERE id = @userID
                 )";

            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userID", Mediator.NotifyGetUserID());
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
            try
            {
                using (MySqlConnection connection = GetConnection())
                {
                    connection.Open();

                    string query = "UPDATE `dolgozok` SET `vezeteknev`=@vezeteknev,`keresztnev`=@keresztnev,`email`=@email,`telefonszam`=@telefonszam WHERE `dolgozok`.`id` = @id";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@vezeteknev", dolgozo.Vezeteknev);
                        command.Parameters.AddWithValue("@keresztnev", dolgozo.Keresztnev);
                        command.Parameters.AddWithValue("@email", dolgozo.Email);
                        command.Parameters.AddWithValue("@telefonszam", dolgozo.Telefonszam);
                        command.Parameters.AddWithValue("@id", dolgozo.ID);

                        int count = Convert.ToInt32(command.ExecuteNonQuery());

                        return count > 0;
                    }
                }
            }
            catch(MySqlException ex)
            {
                Debug.WriteLine($"MySQL Error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"General Error: {ex.Message}");
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
                string checkBevetelekQuery = "SELECT COUNT(*) FROM felhasznalok WHERE dolgozo_id = @id";
                using (MySqlCommand command = new MySqlCommand(checkBevetelekQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    if (count > 0)
                    {
                        relatedRecords.Add($"• Felhasznalói fiókok: {count} db");
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
