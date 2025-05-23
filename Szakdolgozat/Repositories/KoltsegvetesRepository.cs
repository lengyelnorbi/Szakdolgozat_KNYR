using MySql.Data.MySqlClient;
using System;
using System.Collections.ObjectModel;
using Szakdolgozat.Models;

namespace Szakdolgozat.Repositories
{
    public class KoltsegvetesRepository : RepositoryBase, IKoltsegvetesRepository
    {
        public bool AddKoltsegvetes(BevetelKiadas bevetelKiadas)
        {
            try
            {
                using (MySqlConnection connection = GetConnection())
                {
                    connection.Open();

                    string query = "INSERT INTO `bevetelek_kiadasok` (`id`, `osszeg`, `penznem`, `be_ki_kod`, `teljesitesi_datum`, `kotel_kovet_id`, `gazdalkodo_szerv_id`, `magan_szemely_id`) VALUES (NULL, @osszeg, @penznem, @be_ki_kod, @teljesitesi_datum, @kotel_kovet_id, @gazdalkodo_szerv_id, @magan_szemely_id);";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@osszeg", bevetelKiadas.Osszeg);
                        command.Parameters.AddWithValue("@penznem", bevetelKiadas.Penznem);
                        command.Parameters.AddWithValue("@be_ki_kod", bevetelKiadas.BeKiKod);
                        command.Parameters.AddWithValue("@teljesitesi_datum", bevetelKiadas.TeljesitesiDatum);
                        command.Parameters.AddWithValue("@kotel_kovet_id", bevetelKiadas.KotelKovetID);
                        command.Parameters.AddWithValue("@gazdalkodo_szerv_id", bevetelKiadas.GazdalkodasiSzervID);
                        command.Parameters.AddWithValue("@magan_szemely_id", bevetelKiadas.MaganszemelyID);

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

        public bool DeleteKoltsegvetes(int id)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = "DELETE FROM `bevetelek_kiadasok` WHERE `bevetelek_kiadasok`.`id` = @id;";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }

        public ObservableCollection<BevetelKiadas> GetKoltsegvetesek()
        {
            ObservableCollection<BevetelKiadas> data = new ObservableCollection<BevetelKiadas>();
            string query = "SELECT * FROM bevetelek_kiadasok;";

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
                            BeKiKod beKiKod;
                            if (reader["be_ki_kod"] != DBNull.Value)
                            {
                                beKiKod = (BeKiKod)Enum.Parse(typeof(BeKiKod), (string)reader["be_ki_kod"], true);
                            }
                            else
                            {
                                throw new Exception("ERROR");
                            }
                            DateTime teljesitesiDatum = Convert.ToDateTime(reader["teljesitesi_datum"]);
                            int kotelKovetID;
                            if (reader["kotel_kovet_id"] != DBNull.Value)
                            {
                                kotelKovetID = Convert.ToInt32(reader["kotel_kovet_id"]);
                            }
                            else
                            {
                                kotelKovetID = 0;
                            }
                            int gazdalkodoSzervID;
                            if (reader["gazdalkodo_szerv_id"] != DBNull.Value)
                            {
                                gazdalkodoSzervID = Convert.ToInt32(reader["gazdalkodo_szerv_id"]);
                            }
                            else
                            {
                                gazdalkodoSzervID = 0;
                            }
                            int maganSzemelyID;
                            if (reader["magan_szemely_id"] != DBNull.Value)
                            {
                                maganSzemelyID = Convert.ToInt32(reader["magan_szemely_id"]);
                            }
                            else
                            {
                                maganSzemelyID = 0;
                            }
                            BevetelKiadas item = new BevetelKiadas(id, osszeg, penznem, beKiKod, teljesitesiDatum, kotelKovetID, gazdalkodoSzervID, maganSzemelyID);

                            data.Add(item);
                        }
                    }
                }
            }
            return data;
        }

        public bool ModifyKoltsegvetes(BevetelKiadas bevetelKiadas)
        {
            try
            {
                using (MySqlConnection connection = GetConnection())
                {
                    connection.Open();
                    string query = "UPDATE `bevetelek_kiadasok` SET `osszeg`=@osszeg,`penznem`=@penznem,`be_ki_kod`=@bekikod,`teljesitesi_datum`=@teljesitesiDatum,`kotel_kovet_id`=@kotelKovetID,`gazdalkodo_szerv_id`=@gazdalkodo_szerv_id, `magan_szemely_id`=@magan_szemely_id WHERE `bevetelek_kiadasok`.`id` = @id";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@osszeg", bevetelKiadas.Osszeg);
                        command.Parameters.AddWithValue("@penznem", bevetelKiadas.Penznem);
                        command.Parameters.AddWithValue("@bekikod", bevetelKiadas.BeKiKod);
                        command.Parameters.AddWithValue("@teljesitesiDatum", bevetelKiadas.TeljesitesiDatum);
                        command.Parameters.AddWithValue("@kotelKovetID", bevetelKiadas.KotelKovetID);
                        command.Parameters.AddWithValue("@gazdalkodo_szerv_id", bevetelKiadas.GazdalkodasiSzervID);
                        command.Parameters.AddWithValue("@magan_szemely_id", bevetelKiadas.MaganszemelyID);
                        command.Parameters.AddWithValue("@id", bevetelKiadas.ID);

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
    }
}
