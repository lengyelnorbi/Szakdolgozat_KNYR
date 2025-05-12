using LiveCharts.Defaults;
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Szakdolgozat.Models;

namespace Szakdolgozat.Repositories
{
    public class DiagrammRepository : RepositoryBase
    {
        public bool SaveDiagramm(Diagramm diagramm)
        {
            using (var connection = GetConnection())
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();

                using (var command = new MySqlCommand())
                {
                    command.Connection = connection;

                    // If diagramm.ID is 0 (default), this is a new diagram
                    if (diagramm.ID == 0)
                    {
                        command.CommandText = @"
                        INSERT INTO diagrammok (
                            nev, leiras, diagramm_tipus, adathalmaz, diagram_ertekek_adat,
                            szuresi_beallitasok, csoportositasi_beallitasok, kijelolt_diagram_csoportositasok, kijelolt_elemek_ids, 
                            adat_statisztika, letrehozasi_datum, letrehozo_id
                        ) VALUES (
                            @name, @description, @chartType, @dataSource, @dataChartValues, 
                            @filterSettings, @groupbySettings, @seriesGroupBySelection, @selectedItemsIDs, 
                            @dataStatistic, @createdDate, @createdByUserID
                        )";
                    }
                    else
                    {
                        // Update existing diagram
                        command.CommandText = @"
                        UPDATE diagrammok SET 
                            nev = @name, 
                            leiras = @description, 
                            diagramm_tipus = @chartType, 
                            adathalmaz = @dataSource, 
                            diagram_ertekek_adat = @dataChartValues,
                            szuresi_beallitasok = @filterSettings, 
                            csoportositasi_beallitasok = @groupbySettings, 
                            kijelolt_diagram_csoportositasok = @seriesGroupBySelection,
                            kijelolt_elemek_ids = @selectedItemsIDs, 
                            adat_statisztika = @dataStatistic, 
                            modositasi_datum = @modifiedDate
                        WHERE id = @id";

                        command.Parameters.Add("@id", MySqlDbType.Int32).Value = diagramm.ID;
                        command.Parameters.Add("@modifiedDate", MySqlDbType.DateTime).Value = DateTime.Now;
                    }

                    // Add parameters common to both insert and update
                    command.Parameters.Add("@name", MySqlDbType.VarChar).Value = diagramm.Name;
                    command.Parameters.Add("@description", MySqlDbType.Text).Value = diagramm.Description ?? (object)DBNull.Value;
                    command.Parameters.Add("@chartType", MySqlDbType.VarChar).Value = diagramm.ChartType;
                    command.Parameters.Add("@dataSource", MySqlDbType.VarChar).Value = diagramm.DataSource;
                    command.Parameters.Add("@dataChartValues", MySqlDbType.LongText).Value = diagramm.DataChartValues;
                    command.Parameters.Add("@filterSettings", MySqlDbType.Text).Value = diagramm.FilterSettings;
                    command.Parameters.Add("@groupbySettings", MySqlDbType.Text).Value = diagramm.GroupBySettings;
                    command.Parameters.Add("@seriesGroupBySelection", MySqlDbType.Text).Value = diagramm.SeriesGroupBySelection;
                    command.Parameters.Add("@selectedItemsIDs", MySqlDbType.Text).Value = diagramm.SelectedItemsIDs;
                    command.Parameters.Add("@dataStatistic", MySqlDbType.VarChar).Value = diagramm.DataStatistic;

                    if (diagramm.ID == 0) // Only needed for INSERT
                    {
                        command.Parameters.Add("@createdDate", MySqlDbType.DateTime).Value = diagramm.CreatedDate;
                        command.Parameters.Add("@createdByUserID", MySqlDbType.Int32).Value =
                            diagramm.CreatedByUserID.HasValue ? diagramm.CreatedByUserID.Value : (object)DBNull.Value;
                    }

                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }

        public Diagramm GetDiagrammByID(int id)
        {
            using (var connection = GetConnection())
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();

                using (var command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM diagrammok WHERE id = @id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Diagramm
                            {
                                ID = Convert.ToInt32(reader["id"]),
                                Name = reader["nev"].ToString(),
                                Description = reader["leiras"] != DBNull.Value ? reader["leiras"].ToString() : null,
                                ChartType = reader["diagramm_tipus"].ToString(),
                                DataSource = reader["adathalmaz"].ToString(),
                                DataChartValues = reader["diagram_ertekek_adat"].ToString(),
                                FilterSettings = reader["szuresi_beallitasok"].ToString(),
                                GroupBySettings = reader["csoportositasi_beallitasok"].ToString(),
                                SeriesGroupBySelection = reader["kijelolt_diagram_csoportositasok"].ToString(),
                                SelectedItemsIDs = reader["kijelolt_elemek_ids"].ToString(),
                                DataStatistic = reader["adat_statisztika"].ToString(),
                                CreatedDate = Convert.ToDateTime(reader["letrehozasi_datum"]),
                                CreatedByUserID = reader["letrehozo_id"] != DBNull.Value ?
                                    Convert.ToInt32(reader["letrehozo_id"]) : (int?)null
                            };
                        }
                    }
                }
            }

            return null; // Not found
        }

        public List<Diagramm> GetAllDiagramms()
        {
            var diagramms = new List<Diagramm>();

            using (var connection = GetConnection())
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();

                using (var command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT diagrammok.*, dolgozok.vezeteknev, dolgozok.keresztnev FROM diagrammok INNER JOIN felhasznalok on diagrammok.letrehozo_id = felhasznalok.id INNER JOIN dolgozok ON felhasznalok.dolgozo_id = dolgozok.id ORDER BY letrehozasi_datum DESC";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            diagramms.Add(new Diagramm
                            {
                                ID = Convert.ToInt32(reader["id"]),
                                Name = reader["nev"].ToString(),
                                Description = reader["leiras"] != DBNull.Value ? reader["leiras"].ToString() : null,
                                ChartType = reader["diagramm_tipus"].ToString(),
                                DataSource = reader["adathalmaz"].ToString(),
                                DataChartValues = reader["diagram_ertekek_adat"].ToString(),
                                FilterSettings = reader["szuresi_beallitasok"].ToString(),
                                GroupBySettings = reader["csoportositasi_beallitasok"].ToString(),
                                SeriesGroupBySelection = reader["kijelolt_diagram_csoportositasok"].ToString(),
                                SelectedItemsIDs = reader["kijelolt_elemek_ids"].ToString(),
                                DataStatistic = reader["adat_statisztika"].ToString(),
                                CreatedDate = Convert.ToDateTime(reader["letrehozasi_datum"]),
                                CreatedByUserID = reader["letrehozo_id"] != DBNull.Value ?
                                    Convert.ToInt32(reader["letrehozo_id"]) : (int?)null,
                                CreatorName = (reader["vezeteknev"] != DBNull.Value && reader["keresztnev"] != DBNull.Value) ?
                            $"{reader["vezeteknev"]} {reader["keresztnev"]}" : "Unknown",
                            });
                           
                        }
                    }
                }
            }

            return diagramms;
        }

        public bool DeleteDiagramm(int id)
        {
            using (var connection = GetConnection())
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();

                using (var command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM diagrammok WHERE id = @id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }
    }
}
