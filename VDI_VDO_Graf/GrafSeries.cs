using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace VDI_VDO_Graf
{
    public class GrafSeries
    {
        public LineSeries LineSeries { get { return _lineSeries; } }
        public string Name { get { return _name; } }
        public int Column { get { return _column; } }
        private LineSeries _lineSeries;
        private string _name;
        private int _column;

        public GrafSeries(string name, int cSVColumn)
        {
            _lineSeries = new LineSeries()
            {
                Title = name,
                Values = new ChartValues<double> { 0/*, 3, 2, 4*/ },
                LineSmoothness = 0.8, //0: straight lines, 1: really smooth lines
                PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
                PointGeometrySize = 5,
                PointForeground = System.Windows.Media.Brushes.Gray

            };
            this._name = name;
            this._column = cSVColumn;
        }
    }
}
