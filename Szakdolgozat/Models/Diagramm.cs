using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdolgozat.Models
{
    public class Diagramm
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ChartType { get; set; } // "DoughnutSeries", "LineSeries", etc.
        public string DataSource { get; set; } // "Koltsegvetes" or "KotelezettsegKoveteles"
        public string DataChartValues { get; set; } // "Koltsegvetes" or "KotelezettsegKoveteles"
        public string FilterSettings { get; set; } // Serialized filter settings
        public string GroupBySettings { get; set; } // Serialized grouping settings
        public string SeriesGroupBySelection { get; set; } // Serialized grouping settings
        public string SelectedItemsIDs { get; set; } // CSV of selected item IDs
        public string DataStatistic { get; set; } // The statistic method used
        public DateTime CreatedDate { get; set; }
        public int? CreatedByUserID { get; set; }
        public string CreatorName { get; set; } // Name of the user who created the diagram
        public SeriesCollection PreviewChart { get; set; } = new SeriesCollection();
        public SeriesCollection PreviewPieChart { get; set; } = new SeriesCollection();
        public double InnerRadius { get; set; } = 0.0; // For Doughnut charts
    }
}
