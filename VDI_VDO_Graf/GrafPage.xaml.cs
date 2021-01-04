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
        protected string[] _labels;
        protected byte[] hours = new byte[24];
        protected byte[] minutes = new byte[60];


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

        protected void PickedData_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            PickerDataStop.DisplayDateStart = pickedData.SelectedDate;
        }

        protected void PickerDataStop_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            pickedData.DisplayDateEnd = PickerDataStop.SelectedDate;
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



            try
            {
                csvDataTable = CSV_DataTable.ConvertCSVtoDataTable(filePath);
            }
            catch (Exception ex) { }

            SeriesCollectionOperate seriesCollectionOperate = new SeriesCollectionOperate();
            try
            {
                _ = LoopTask(seriesCollectionOperate, arSeries.Length);
            }
            catch (Exception ex) { /*System.Windows.MessageBox.Show(ex.Message);*/ }
            //DownloadFileFTP();
        }


        private void DownloadFileFTP()
        {
            string inputfilepath = @"D:\Delete\her.csv";
            //string inputfilepath = @"D:\Delete\error_ini.xml";
            string ftphost = "192.168.1.17";
            //string ftpfilepath = "/media/SD_Card/Trend/log_2020_06_12.csv";
            string ftpfilepath = "//media/SD_Card/Trend/log_2020_06_12.csv";

            string ftpfullpath = "ftp://" + ftphost + ftpfilepath;

            using (WebClient request = new WebClient())
            {
                request.Credentials = new NetworkCredential("admin", "wago");

                byte[] fileData = request.DownloadData(ftpfullpath);

                using (FileStream file = File.Create(inputfilepath))
                {
                    file.Write(fileData, 0, fileData.Length);
                    file.Close();
                }
                MessageBox.Show("Download Complete");
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
    }
}
