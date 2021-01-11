using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;
using System.Data;

namespace Vent_Graf
{
    class SeriesCollectionOperate
    {
        System.DateTime timeOfMaxValue;
        System.DateTime timeOfMinValue;
        double maxValue = 0d;
        double minValue = 0d;

        public string[] SetValues(IChartValues collection, DataTable dataTable, int dateColumn, int timeColumn, int dataColumn,
                                   System.DateTime startDate, System.DateTime endDate, int startHour, int endHour,
                                    int startMin, int endMin)
        {
            string[] labels = new string[dataTable.Rows.Count];
            Queue<string> queue = new Queue<string>();
            collection.Clear();

            System.DateTime currentDate = startDate;
            System.DateTime currentTime = startDate;
            System.DateTime stopTime = endDate;
            stopTime = stopTime.AddHours(endHour);
            stopTime = stopTime.AddMinutes(endMin);
            System.DateTime startTime = startDate;
            startTime = startTime.AddHours(startHour);
            startTime = startTime.AddMinutes(startMin);
            object[] values = new object[dataTable.Rows.Count];

            System.DateTime[] dataTime = new DateTime[dataTable.Rows.Count];
            Dictionary<System.DateTime, int> dates = new Dictionary<System.DateTime, int>();
            for (int j = 0; j < dataTable.Rows.Count; j++)
            {
                try
                {
                    dataTime[j] = Convert.ToDateTime(dataTable.Rows[j][dateColumn]);
                    int hours = Convert.ToDateTime(dataTable.Rows[j][timeColumn]).Hour;
                    int minutes = Convert.ToDateTime(dataTable.Rows[j][timeColumn]).Minute;

                    dataTime[j] = dataTime[j].AddHours(hours);
                    dataTime[j] = dataTime[j].AddMinutes(minutes);
                    dates.Add(dataTime[j], j);
                    values[j] = dataTable.Rows[j][dataColumn];
                }
                catch (Exception ex) { }
            }
            int increment = (int)Math.Ceiling(((stopTime - startTime).TotalMinutes) / 72);
            //System.DateTime timeOfMaxValue = dates.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

            int numberOfPoints = 0;
            int statrIndex = 0;
            int stopIndex = 0;
            if (dates.ContainsKey(startTime))
            {
                statrIndex = dates[startTime];
            }
            else
            {
                statrIndex = 0;
            }

            if (dates.ContainsKey(stopTime))
            {
                stopIndex = dates[stopTime];
            }
            else
            {
                stopIndex = dates.Count - 1;
            }

            while (startTime < stopTime)
            {
                queue.Enqueue(startTime.ToString());
                if (dates.ContainsKey(startTime))
                {
                    collection.Add(Convert.ToDouble(values[dates[startTime]]));
                }
                else
                {
                    collection.Add(0d);
                }
                startTime = startTime.AddMinutes(increment);
                //numberOfPoints++;
            }
            //numberOfPoints = numberOfPoints * increment;
            numberOfPoints = stopIndex - statrIndex;

            Double[] vs = new double[numberOfPoints];

            int indexOfMax = 0;
            int indexOfMin = 0;
            maxValue = 0d;
            minValue = 0d;
            for (int i = 0; i < numberOfPoints - 1; i++)
            {
                try
                {
                    vs[i] = Convert.ToDouble(values[i + statrIndex]);
                    if (vs[i] > maxValue)
                    {
                        maxValue = vs[i];
                        indexOfMax = i + statrIndex;
                    }
                    if (vs[i] != 0d)
                    {
                        if ((minValue == 0d) || (vs[i] < minValue))
                        {
                            minValue = vs[i];
                            indexOfMin = i + statrIndex;
                        }
                    }
                }
                catch (Exception ex) { System.Windows.MessageBox.Show(ex.Message + ";" + statrIndex.ToString() + ";" + numberOfPoints.ToString()); break; }

            }

            timeOfMaxValue = dataTime[indexOfMax];
            timeOfMinValue = dataTime[indexOfMin];
            //MessageBox.Show(timeOfMaxValue.ToString() + ";" + maxValue.ToString() + ";" + statrIndex.ToString() + ";" + numberOfPoints.ToString());
            labels = queue.ToArray();
            return labels;
        }

        public KeyValuePair<System.DateTime, double> GetMax()
        {
            return new KeyValuePair<DateTime, double>(timeOfMaxValue, maxValue);
        }

        public KeyValuePair<System.DateTime, double> GetMin()
        {
            return new KeyValuePair<DateTime, double>(timeOfMinValue, minValue);
        }
    }
}
