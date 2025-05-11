using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Szakdolgozat.Models
{
    public class SeriesItem
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public bool DataLabels { get; set; }
        public ChartValues<double> Values { get; set; }
        public Brush Fill { get; set; }
    }
}
