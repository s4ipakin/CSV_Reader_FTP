using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
using System.Xml.Serialization;
using Vent_Graf.GrafSettings;
using Vent_Graf.Serialise;

namespace Vent_Graf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        GrafPage flaw;
        GrafPage perepad;
        GrafPage temperature_K1;

        GrafPage wetness;
        GrafPage temperature_P1;

        PLC_IP iP;

        public MainWindow()
        {
            InitializeComponent();
            Dictionary<GrafType, GrafSet> grafSets = new Dictionary<GrafType, GrafSet>();
            InitializeSets(grafSets);
            temperature_K1 = new GrafPage(grafSets[GrafType.Temp_K1]);
            wetness = new GrafPage(grafSets[GrafType.Wetness]);
            temperature_P1 = new GrafPage(grafSets[GrafType.Temp_P1]);
            flaw = new GrafPage(grafSets[GrafType.FlawAir]);
            perepad = new GrafPage(grafSets[GrafType.Perepad]);
            iP = new PLC_IP(192, 168, 1, 18);
            DeserialiseIP();
            temperature_K1.IP = iP;
            wetness.IP = iP;
            temperature_P1.IP = iP;
            flaw.IP = iP;
            perepad.IP = iP;
            
            Main.Content = temperature_K1;
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


        private void MainBtcClck(object sender, RoutedEventArgs e)
        {
            Main.Content = temperature_K1;
        }

        private void PressureBtnClk(object sender, RoutedEventArgs e)
        {
            Main.Content = temperature_P1;
        }

        

        private void btnCondClck(object sender, RoutedEventArgs e)
        {
            Main.Content = wetness;
        }

        private void btnFlawClck(object sender, RoutedEventArgs e)
        {
            Main.Content = flaw;
        }

        private void btnLewelClck(object sender, RoutedEventArgs e)
        {
            Main.Content = perepad;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                PLC_IP newIP = new PLC_IP(Convert.ToInt32(IP_192.Text), Convert.ToInt32(IP_168.Text), Convert.ToInt32(IP_1.Text), Convert.ToInt32(IP_17.Text));
                SerialiseIP(newIP);
                iP = newIP;
                temperature_K1.IP = iP;
                temperature_P1.IP = iP;
                wetness.IP = iP;
                flaw.IP = iP;
                perepad.IP = iP;
            }
            catch (Exception ex) { }
        }


    }
}
