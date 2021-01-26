using VDI_VDO_Graf.GrafSettings;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
//using System.Drawing.Imaging;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing.Imaging;
using System.Net;
using System.Reflection;
using VDI_VDO_Graf.Serialise;
using System.Threading;

namespace VDI_VDO_Graf
{
    /// <summary>
    /// Interaction logic for GrafPage.xaml
    /// </summary>
    public partial class GrafPage : Page, INotifyPropertyChanged
    {
        protected DataTable csvDataTable;
        protected DataSet dataset = new DataSet();
        public event PropertyChangedEventHandler PropertyChanged;
        public SeriesCollection SeriesCollection { get; set; }
        public PLC_IP IP
        { set
            {
                _ip = value;
            } 
        }
        protected PLC_IP _ip;
        protected string[] _labels;
        protected byte[] hours = new byte[24];
        protected byte[] minutes = new byte[60];
        public static event EventHandler SelectedDateStartChanged;
        public static event EventHandler SelectedDateStopChanged;
        public static event EventHandler SelectedHourStartChanged;
        public static event EventHandler SelectedHourEndChanged;
        public static event EventHandler SelectedMinuteStartChanged;
        public static event EventHandler SelectedMinuteEndChanged;

        protected static string filePath;
        public static string FilePath
        {
            set
            {
                filePath = value;
            }
        }

        protected GrafSeries[] arSeries;
        protected Dictionary<int, CheckBox> checkBoxes = new Dictionary<int, CheckBox>();
        protected Dictionary<int, TextBox> textBoxes = new Dictionary<int, TextBox>();
        protected Dictionary<int, Label> labels = new Dictionary<int, Label>();
        protected Dictionary<int, TextBox> textBoxesMin = new Dictionary<int, TextBox>();
        protected Dictionary<int, Label> labelsMin = new Dictionary<int, Label>();

        public string[] Labels
        {
            get { return _labels; }
            set
            {
                _labels = value;
                OnPropertyChanged("Labels");
            }
        }





        public Func<double, string> YFormatter { get; set; }

        public GrafPage(GrafSet grafSet/*, string yAxesName, string unit, double maxValue*/)
        {
            InitializeComponent();
            SetSeries(grafSet);
            Chart_P.ZoomingSpeed = 0.7;
            Chart_P.DisableAnimations = true;
            csvDataTable = new DataTable();
            dataset.Tables.Add(csvDataTable);
            SeriesCollection = new SeriesCollection();
            Labels = new[] { System.DateTime.Now.ToString() };
            YFormatter = value => value.ToString() + grafSet.unit;
            SetCheckBoxes();

            for (int i = 0; i < arSeries.Length; i++)
            {
                textBoxes.Add(i, new TextBox());
                textBoxes[i].HorizontalAlignment = HorizontalAlignment.Right;
                textBoxes[i].VerticalAlignment = VerticalAlignment.Top;
            }

            foreach (KeyValuePair<int, TextBox> keyValuePair in textBoxes)
            {
                this.grid.Children.Add(keyValuePair.Value);
            }

            for (int i = 0; i < arSeries.Length; i++)
            {

                labels.Add(i, new Label());
                labels[i].HorizontalAlignment = HorizontalAlignment.Right;
                labels[i].VerticalAlignment = VerticalAlignment.Top;
                labels[i].Content = arSeries[i].Name;
            }

            foreach (KeyValuePair<int, Label> keyValuePair in labels)
            {
                this.grid.Children.Add(keyValuePair.Value);
            }
            double chartBottomMargin = Chart_P.Margin.Bottom;
            double lebelHight = 0d;
            for (int i = 0; i < arSeries.Length; i++)
            {
                labelsMin.Add(i, new Label());
                labelsMin[i].HorizontalAlignment = HorizontalAlignment.Right;
                labelsMin[i].VerticalAlignment = VerticalAlignment.Bottom;

                labelsMin[i].Content = arSeries[i].Name;
                lebelHight = labelsMin[i].ActualHeight;

                textBoxesMin.Add(i, new TextBox());
                textBoxesMin[i].HorizontalAlignment = HorizontalAlignment.Right;
                textBoxesMin[i].VerticalAlignment = VerticalAlignment.Bottom;
            }

            foreach (KeyValuePair<int, TextBox> keyValuePair in textBoxesMin)
            {
                this.grid.Children.Add(keyValuePair.Value);
            }
            foreach (KeyValuePair<int, Label> keyValuePair in labelsMin)
            {
                this.grid.Children.Add(keyValuePair.Value);
            }


            SetArrays();
            comboBoxHourStart.ItemsSource = hours;
            comboBoxHourEnd.ItemsSource = hours;
            comboBoxMinuteStart.ItemsSource = minutes;
            comboBoxMinuteEnd.ItemsSource = minutes;
            Chart_P.Zoom = ZoomingOptions.Y;
            DataContext = this;
            PickerDataStop.SelectedDateChanged += PickerDataStop_SelectedDateChanged;
            pickedData.SelectedDateChanged += PickedData_SelectedDateChanged;
            this.grid.PreviewMouseRightButtonDown += Grid_PreviewMouseRightButtonDown;
            this.grid.PreviewMouseRightButtonUp += Grid_PreviewMouseRightButtonUp;
            this.Loaded += GrafPage_Loaded;
            GrafPage.SelectedDateStartChanged += GrafPage_SelectedDateStartChanged;
            GrafPage.SelectedDateStopChanged += GrafPage_SelectedDateStopChanged;
            comboBoxHourStart.SelectionChanged += ComboBoxHourStart_SelectionChanged;
            comboBoxHourEnd.SelectionChanged += ComboBoxHourEnd_SelectionChanged;
            comboBoxMinuteStart.SelectionChanged += ComboBoxMinuteStart_SelectionChanged;
            comboBoxMinuteEnd.SelectionChanged += ComboBoxMinuteEnd_SelectionChanged;
            GrafPage.SelectedHourStartChanged += GrafPage_SelectedHourStartChanged;
            GrafPage.SelectedHourEndChanged += GrafPage_SelectedHourEndChanged;
            GrafPage.SelectedMinuteStartChanged += GrafPage_SelectedMinuteStartChanged;
            GrafPage.SelectedMinuteEndChanged += GrafPage_SelectedMinuteEndChanged;

        }

        

        private void GrafPage_SelectedMinuteEndChanged(object sender, EventArgs e)
        {
            comboBoxMinuteEnd.SelectedIndex = minuteEnd;
        }

        private void GrafPage_SelectedMinuteStartChanged(object sender, EventArgs e)
        {
            comboBoxMinuteStart.SelectedIndex = minuteStart;
        }

        private void GrafPage_SelectedHourEndChanged(object sender, EventArgs e)
        {
            comboBoxHourEnd.SelectedIndex = hourEnd;
        }

        private void GrafPage_SelectedHourStartChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(System.IO.Path.GetDirectoryName(
            //         Assembly.GetAssembly(typeof(GrafPage)).CodeBase));
            comboBoxHourStart.SelectedIndex = hourStart;
        }

        private static int minuteEnd;
        private void ComboBoxMinuteEnd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            minuteEnd = comboBoxMinuteEnd.SelectedIndex;
            if (SelectedMinuteEndChanged != null)
                SelectedMinuteEndChanged(null, EventArgs.Empty);
        }

        private static int minuteStart;
        private void ComboBoxMinuteStart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            minuteStart = comboBoxMinuteStart.SelectedIndex;
            if (SelectedMinuteStartChanged != null)
                SelectedMinuteStartChanged(null, EventArgs.Empty);
        }

        private static int hourEnd;
        private void ComboBoxHourEnd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            hourEnd = comboBoxHourEnd.SelectedIndex;
            if (SelectedHourEndChanged != null)
                SelectedHourEndChanged(null, EventArgs.Empty);
        }

        private static int hourStart;
        private void ComboBoxHourStart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            hourStart = comboBoxHourStart.SelectedIndex;
            
            if (SelectedHourStartChanged != null)
                SelectedHourStartChanged(null, EventArgs.Empty);
        }

        private void GrafPage_SelectedDateStopChanged(object sender, EventArgs e)
        {
            PickerDataStop.SelectedDate = selectedDateStop;
        }

        private void GrafPage_SelectedDateStartChanged(object sender, EventArgs e)
        {
            pickedData.SelectedDate = selectedDateStart;
        }

        private void GrafPage_Loaded(object sender, RoutedEventArgs e)
        {
            double lebelHightMin = 0d;
            double lebelHightMax = 0d;
            double chartBottomMargin = Chart_P.Margin.Bottom;
            double chartTopMargin = Chart_P.Margin.Top;
            List<double> Heights = new List<double>();
            double sumHeight = 0d;
            for (int i = 0; i < arSeries.Length; i++)
            {
                Heights.Add(labels[i].ActualHeight);
                sumHeight = sumHeight + labels[i].ActualHeight;
            }
            for (int i = 0; i < arSeries.Length; i++)
            {

                lebelHightMin = lebelHightMin + Heights[i];
                labels[i].Margin = new Thickness(0, chartTopMargin + lebelHightMax + i * 10 - 40, 10, 0);
                labelsMin[i].Margin = new Thickness(0, 0, 10, chartBottomMargin + (sumHeight - lebelHightMin) + (arSeries.Length - i) * 10 - 40);
                textBoxesMin[i].Margin = new Thickness(0, 0, 10, labelsMin[i].Margin.Bottom - 10);
                textBoxes[i].Margin = new Thickness(0, labels[i].Margin.Top + Heights[i], 10, 0);
                checkBoxes[i].Margin = new Thickness(49, 0, 0, chartBottomMargin - lebelHightMin - 40);
                lebelHightMax = lebelHightMax + Heights[i];
            }
        }



        private void SetCheckBoxes()
        {
            for (int i = 0; i < arSeries.Length; i++)
            {
                SeriesCollection.Add(arSeries[i].LineSeries);
                checkBoxes.Add(i, new CheckBox());
                checkBoxes[i].HorizontalAlignment = HorizontalAlignment.Left;
                checkBoxes[i].VerticalAlignment = VerticalAlignment.Bottom;
                double chartBtmMargin = Chart_P.Margin.Bottom;
                checkBoxes[i].Content = arSeries[i].Name;
                checkBoxes[i].IsChecked = true;
            }

            foreach (KeyValuePair<int, CheckBox> keyValuePair in checkBoxes)
            {
                keyValuePair.Value.Checked += (sender, e) => ChckbxChecked(sender, e, keyValuePair.Key);
                keyValuePair.Value.Unchecked += (sender, e) => ChckbxUnchecked(sender, e, keyValuePair.Key);
                this.grid.Children.Add(keyValuePair.Value);
            }
        }

        private void Grid_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Chart_P.Zoom = ZoomingOptions.Y;
        }

        private void Grid_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Chart_P.Zoom = ZoomingOptions.X;
        }

        protected virtual void SetSeries(GrafSet grafSet)
        {
            arSeries = grafSet.GetSettings();//new GrafSeries[] { };
            chartAxisY.Title = grafSet.yAxesName;
            chartAxisY.MaxValue = grafSet.maxValue;
        }

        private void ChckbxChecked(object sender, RoutedEventArgs e, int index)
        {
            arSeries[index].LineSeries.Visibility = Visibility.Visible;
            textBoxes[index].Visibility = Visibility.Visible;
            labels[index].Visibility = Visibility.Visible;
        }
        private void ChckbxUnchecked(object sender, RoutedEventArgs e, int index)
        {
            arSeries[index].LineSeries.Visibility = Visibility.Hidden;
            textBoxes[index].Visibility = Visibility.Hidden;
            labels[index].Visibility = Visibility.Hidden;

        }

        private static DateTime? selectedDateStart;
        private static DateTime? selectedDateStop;
        protected void PickedData_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDateStart = pickedData.SelectedDate;
            PickerDataStop.DisplayDateStart = selectedDateStart;
            if (SelectedDateStartChanged != null)
                SelectedDateStartChanged(null, EventArgs.Empty);
        }

        protected void PickerDataStop_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDateStop = PickerDataStop.SelectedDate;
            pickedData.DisplayDateEnd = selectedDateStop;
            if (SelectedDateStopChanged != null)
                SelectedDateStopChanged(null, EventArgs.Empty);
        }



        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null) // if subrscribed to event
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetArrays()
        {
            for (byte i = 0; i < 24; i++)
            {
                hours[i] = i;
            }
            for (byte i = 0; i < 60; i++)
            {
                minutes[i] = i;
            }
        }

        protected void btnPrintClick(object sender, RoutedEventArgs e)
        {

            int width = (int)Chart_P.ActualWidth;
            int height = (int)Chart_P.ActualHeight;
            System.Windows.Point position = Chart_P.PointToScreen(new System.Windows.Point(0d, 0d));

            System.Drawing.Rectangle bounds = new System.Drawing.Rectangle(/*(int)position.X*/0, /*(int)position.Y*/0, width, height + 86);
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new System.Drawing.Point((int)position.X, (int)position.Y - 36), System.Drawing.Point.Empty, bounds.Size);
                }
                System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
                //saveFileDialog1.Filter = "JPeg Image|*.jpg";
                //saveFileDialog1.Title = "Сохранить график как изображение JPeg";
                //saveFileDialog1.ShowDialog();
                string imagePath = "hern.jpeg";//saveFileDialog1.FileName;
                bitmap.Save(imagePath, ImageFormat.Jpeg);
                Process.Start(imagePath);
            }
        }

        protected void btnFromDays_Click(object sender, RoutedEventArgs e)
        {
            this.LblLoading.Visibility = Visibility.Visible;
            if (selectedDateStart != null && selectedDateStop != null)
            {
                Task downloadTask = Task.Run(() =>
                {
                    DownloadFileFTP();
                    try
                    {
                        csvDataTable = CSV_DataTable.ConvertCSVtoDataTable(filePath);
                    }
                    catch (Exception ex) { System.Windows.MessageBox.Show(ex.Message); }

                    SeriesCollectionOperate seriesCollectionOperate = new SeriesCollectionOperate();
                    try
                    {
                        //_ = LoopTask(seriesCollectionOperate, arSeries.Length);
                        Dispatcher.Invoke(() => DrawChart(seriesCollectionOperate, arSeries.Length));
                    }
                    catch (Exception ex) { System.Windows.MessageBox.Show(ex.Message); }
                });
                
            }
            else
            {
                MessageBox.Show("Выберете дату начала и конца графика");
            }
        }

        

        private void DownloadFileFTP()
        {
            /*string inputfilepath*/
            filePath = Directory.GetCurrentDirectory();//@"D:\Delete\her.csv";
            /*inputfilepath*/
            filePath = /*inputfilepath*/filePath + @"\log.csv";
            //string ftphost = "192.168.1.17";
            string ftphost = _ip.IP_192.ToString() + "." + _ip.IP_168.ToString() + "." + _ip.IP_1.ToString() + "." + _ip.IP_17.ToString();

            //string ftpfilepath = "/media/SD_Card/Trend/log_2020_06_12.csv";
            string ftpfilepath = "//media/SD_Card/Logs/log.csv";
            //ftpfilepath = "//media/SD_Card/Trend/";

            string ftpfullpath = "ftp://" + ftphost + ftpfilepath;

            

            using (WebClient request = new WebClient())
            {
                request.Credentials = new NetworkCredential("admin", "wago");
                request.DownloadFile(ftpfullpath, "log.csv");
                //byte[] fileData = request.DownloadData(ftpfullpath);                
                //using (FileStream file = File.Create(/*inputfilepath*/filePath))
                //{
                //    file.Write(fileData, 0, fileData.Length);
                //    file.Close();
                //}
            }
        }

        

        private Task SetCollections(SeriesCollectionOperate seriesCollectionOperate, int i)
        {
            
            Labels = seriesCollectionOperate.SetValues(SeriesCollection[i].Values, csvDataTable, 1, 2, arSeries[i].Column,
                    (System.DateTime)pickedData.SelectedDate, (System.DateTime)PickerDataStop.SelectedDate, comboBoxHourStart.SelectedIndex,
                    comboBoxHourEnd.SelectedIndex, comboBoxMinuteStart.SelectedIndex, comboBoxMinuteEnd.SelectedIndex);

            KeyValuePair<System.DateTime, double> maxValue = new KeyValuePair<DateTime, double>();
            maxValue = seriesCollectionOperate.GetMax();
            textBoxes[i].Text = "Max: [" + maxValue.Key.ToString() + "]  " + maxValue.Value.ToString();

            KeyValuePair<System.DateTime, double> minValue = new KeyValuePair<DateTime, double>();
            minValue = seriesCollectionOperate.GetMin();
            textBoxesMin[i].Text = "Min: [" + minValue.Key.ToString() + "]  " + minValue.Value.ToString();

            return Task.CompletedTask;
        }

        private async Task LoopTask(SeriesCollectionOperate seriesCollectionOperate, int nomber)
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < nomber; i++)
            {
                tasks.Add(SetCollections(seriesCollectionOperate, i));
            }
            await Task.WhenAll(tasks);
        }

        private void DrawChart(SeriesCollectionOperate seriesCollectionOperate, int nomber)
        {
            this.LblLoading.Visibility = Visibility.Hidden;
            for (int i = 0; i < nomber; i++)
            {
                SetCollections(seriesCollectionOperate, i);
            }
        }
    }
}
