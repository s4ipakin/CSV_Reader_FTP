using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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
using VDI_VDO_Graf.GrafSettings;

namespace VDI_VDO_Graf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GrafPage flaw;
        GrafPage level;
        GrafPage cO2;

        GrafPage temperature;
        GrafPage pressure;
        GrafPage conductivity;
        public MainWindow()
        {
            InitializeComponent();
            Dictionary<GrafType, GrafSet> grafSets = new Dictionary<GrafType, GrafSet>();
            //temperature1 = new Temperature1();
            InitializeSets(grafSets);
            temperature = new GrafPage(grafSets[GrafType.Temp]);
            pressure = new GrafPage(grafSets[GrafType.Pressure]);
            conductivity = new GrafPage(grafSets[GrafType.Conductivity]);
            flaw = new GrafPage(grafSets[GrafType.Flaw]);
            level = new GrafPage(grafSets[GrafType.Level]);
            cO2 = new GrafPage(grafSets[GrafType.CO2]);

            Main.Content = temperature;
        }

        private void InitializeSets(Dictionary<GrafType, GrafSet> grafSets)
        {
            grafSets.Clear();
            var allGrafTypes = Assembly.GetAssembly(typeof(GrafSet)).GetTypes()
                .Where(t => typeof(GrafSet).IsAssignableFrom(t) && t.IsAbstract == false);

            foreach (var type in allGrafTypes)
            {
                GrafSet grafSet = Activator.CreateInstance(type) as GrafSet;
                grafSets.Add(grafSet.GrafType, grafSet);
            }
        }

        private void MainBtcClck(object sender, RoutedEventArgs e)
        {
            new Thread(() => this.Dispatcher.Invoke(() => Main.Content = temperature)).Start();
            //Main.Content = temperature;
        }



        private void btnChooseFile_clk(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                GrafPage.FilePath = filePath;
            }
        }

        private void PressureBtnClk(object sender, RoutedEventArgs e)
        {
            //Main.Content = pressure;
            //new Thread(() => this.Dispatcher.Invoke(() => Main.Content = pressure)).Start();
            Thread thread = new Thread(() => this.Dispatcher.Invoke(() => Main.Content = pressure));
            thread.IsBackground = true;
            thread.Start();
        }

        private void btnCondClck(object sender, RoutedEventArgs e)
        {
            //Main.Content = conductivity;
            new Thread(() => this.Dispatcher.Invoke(() => Main.Content = conductivity)).Start();
        }

        private void btnFlawClck(object sender, RoutedEventArgs e)
        {
            //Main.Content = flaw;
            //new Thread(() => this.Dispatcher.Invoke(() => Main.Content = flaw)).Start();
            Thread thread = new Thread(() => this.Dispatcher.Invoke(() => Main.Content = flaw));
            thread.IsBackground = true;
            thread.Start();
        }

        private void btnLewelClck(object sender, RoutedEventArgs e)
        {
            //Main.Content = level;
            //new Thread(() => this.Dispatcher.Invoke(() => Main.Content = level)).Start();
            Thread thread = new Thread(() => this.Dispatcher.Invoke(() => Main.Content = level));
            thread.IsBackground = true;
            thread.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Main.Content = cO2;
            //new Thread(() => this.Dispatcher.Invoke(() => Main.Content = cO2)).Start();
            Thread thread = new Thread(() => this.Dispatcher.Invoke(() => Main.Content = cO2));
            thread.IsBackground = true;
            thread.Start();

        }
    }
}
