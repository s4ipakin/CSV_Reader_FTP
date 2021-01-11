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
using VDI_VDO_Graf.Serialise;
using System.Xml.Serialization;
using System.IO;

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

        PLC_IP iP;

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
            iP = new PLC_IP(192, 168, 1, 18);
            DeserialiseIP();
            temperature.IP = iP;
            pressure.IP = iP;
            conductivity.IP = iP;
            flaw.IP = iP;
            level.IP = iP;
            cO2.IP = iP;

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

        private void SerialiseIP(PLC_IP iP)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(PLC_IP));

            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream("ip.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, iP);
            }
        }

        private void DeserialiseIP()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(PLC_IP));
            try
            {
                using (FileStream fs = new FileStream("ip.xml", FileMode.OpenOrCreate))
                {
                    PLC_IP newIP = (PLC_IP)formatter.Deserialize(fs);
                    if (newIP.IP_192 != 0)
                    {
                        iP = newIP;
                        IP_192.Text = iP.IP_192.ToString();
                        IP_168.Text = iP.IP_168.ToString();
                        IP_1.Text = iP.IP_1.ToString();
                        IP_17.Text = iP.IP_17.ToString();
                    }
                }
            }
            catch (Exception ex) { }           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                PLC_IP newIP = new PLC_IP(Convert.ToInt32(IP_192.Text), Convert.ToInt32(IP_168.Text), Convert.ToInt32(IP_1.Text), Convert.ToInt32(IP_17.Text));
                SerialiseIP(newIP);
                iP = newIP;
                temperature.IP = iP;
                pressure.IP = iP;
                conductivity.IP = iP;
                flaw.IP = iP;
                level.IP = iP;
                cO2.IP = iP;
                
            }
            catch(Exception ex) { }
        }
    }
}
