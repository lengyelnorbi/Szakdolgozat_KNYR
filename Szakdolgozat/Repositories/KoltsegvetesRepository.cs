using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Szakdolgozat.Models;

namespace Szakdolgozat.Repositories
{
    public class KoltsegvetesRepository : RepositoryBase, IKoltsegvetesRepository
    {
        public bool AddKoltsegvetes(BevetelKiadas bevetelKiadas)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = "INSERT INTO `bevetelek_kiadasok` (`id`, `osszeg`, `penznem`, `be_ki_kod`, `teljesitesi_datum`, `kotel_kovet_id`, `partner_id`) VALUES (NULL, @osszeg, @penznem, @be_ki_kod, @teljesitesi_datum, @kotel_kovet_id, @partner_id);";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@osszeg", bevetelKiadas.Osszeg);
                    command.Parameters.AddWithValue("@penznem", bevetelKiadas.Penznem);
                    command.Parameters.AddWithValue("@be_ki_kod", bevetelKiadas.BeKiKod);
                    command.Parameters.AddWithValue("@teljesitesi_datum", bevetelKiadas.TeljesitesiDatum);
                    command.Parameters.AddWithValue("@kotel_kovet_id", bevetelKiadas.KotelKovetID);
                    command.Parameters.AddWithValue("@partner_id", bevetelKiadas.PartnerID);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
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
                            string beKiKod = Convert.ToString(reader["be_ki_kod"]);
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
                            int partnerID;
                            if (reader["partner_id"] != DBNull.Value)
                            {
                                partnerID = Convert.ToInt32(reader["partner_id"]);
                            }
                            else
                            {
                                partnerID = 0;
                            }

                            BevetelKiadas item = new BevetelKiadas(id, osszeg, penznem, beKiKod, teljesitesiDatum, kotelKovetID, partnerID);

                            data.Add(item);
                        }
                    }
                }
            }
            return data;
        }

        public bool ModifyKoltsegvetes(BevetelKiadas bevetelKiadas)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "UPDATE `bevetelek_kiadasok` SET `osszeg`=@osszeg,`penznem`=@penznem,`be_ki_kod`=@bekikod,`teljesitesi_datum`=@teljesitesiDatum,`kotel_kovet_id`=@kotelKovetID,`partner_id`=@partnerID WHERE `bevetelek_kiadasok`.`id` = @id";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@osszeg", bevetelKiadas.Osszeg);
                    command.Parameters.AddWithValue("@penznem", bevetelKiadas.Penznem);
                    command.Parameters.AddWithValue("@bekikod", bevetelKiadas.BeKiKod);
                    command.Parameters.AddWithValue("@teljesitesiDatum", bevetelKiadas.TeljesitesiDatum);
                    command.Parameters.AddWithValue("@kotelKovetID", bevetelKiadas.KotelKovetID);
                    command.Parameters.AddWithValue("@partnerID", bevetelKiadas.PartnerID);
                    command.Parameters.AddWithValue("@id", bevetelKiadas.ID);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }
    }
}
