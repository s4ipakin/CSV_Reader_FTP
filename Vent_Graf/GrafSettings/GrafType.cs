using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum GrafType
{
    Temp,
    Pressure,
    Conductivity,
    Flaw,
    Level,
    CO2,
    Temp_K1,
    Temp_P1,
    Wetness,
    FlawAir,
    Perepad
}

namespace Vent_Graf.GrafSettings
{
    public abstract class GrafSet
    {
        public abstract GrafType GrafType { get; }
        public abstract string yAxesName { get; }
        public abstract string unit { get; }
        public abstract double maxValue { get; }
        public abstract GrafSeries[] GetSettings();
    }


    public class TempGrafSet : GrafSet
    {
        public override GrafType GrafType => GrafType.Temp_K1;
        public override string yAxesName => "Температура K1";
        public override string unit => "°C";
        public override double maxValue => 60;

        public override GrafSeries[] GetSettings()
        {
            return new GrafSeries[]
            {
                new GrafSeries("Температура приточного воздуха К1", 3),
                new GrafSeries("Температура вытяжного воздуха К1", 7),
                new GrafSeries("Температура обратного теплоносителя К1", 23),
                new GrafSeries("Температура наружного воздуха", 25),
            };
        }
    }

    public class PressureGrafSet : GrafSet
    {
        public override GrafType GrafType => GrafType.Temp_P1;
        public override string yAxesName => "Температура P1";
        public override string unit => "°C";
        public override double maxValue => 60;
        public override GrafSeries[] GetSettings()
        {
            return new GrafSeries[]
            {
                new GrafSeries("Температура приточного воздуха П1", 11),
                new GrafSeries("Температура вытяжного воздуха П1", 15),
                new GrafSeries("Температура обратного теплоносителя П1", 27)
            };
        }
    }

    public class ConductivityGrafSet : GrafSet
    {
        public override GrafType GrafType => GrafType.Wetness;
        public override string yAxesName => "Влажность";
        public override string unit => "%";
        public override double maxValue => 100;
        public override GrafSeries[] GetSettings()
        {
            return new GrafSeries[]
            {
                new GrafSeries("Влажность приточного воздуха К1", 5),
                new GrafSeries("Влажность вытяжного воздуха К1", 9),
                new GrafSeries("Влажность приточного воздуха П1", 13),
                new GrafSeries("Влажность вытяжного воздуха П1", 17)
            };
        }
    }

    public class FlawGrafSet : GrafSet
    {
        public override GrafType GrafType => GrafType.FlawAir;
        public override string yAxesName => "Скорость потока";
        public override string unit => "м/c";
        public override double maxValue => 250;
        public override GrafSeries[] GetSettings()
        {
            return new GrafSeries[]
            {
                new GrafSeries("Скорость воздушного потока К1", 25),
                new GrafSeries("Скорость воздушного потока П1", 27)
            };
        }
    }

    public class LevelGrafSet : GrafSet
    {
        public override GrafType GrafType => GrafType.Perepad;
        public override string yAxesName => "Перепад давления";
        public override string unit => "CM";
        public override double maxValue => 150;
        public override GrafSeries[] GetSettings()
        {
            return new GrafSeries[]
            {
                new GrafSeries("Перепад давления помещения K1", 19),
                new GrafSeries("Перепад давления помещения П1", 21)
            };
        }
    }

    public class CO2_GrafSet : GrafSet
    {
        public override GrafType GrafType => GrafType.CO2;
        public override string yAxesName => "Содержание органического углерода ВДИ";
        public override string unit => "ppbC";
        public override double maxValue => 1000;
        public override GrafSeries[] GetSettings()
        {
            return new GrafSeries[]
            {
                new GrafSeries("Содержание органического углерода ВДИ", 29)
            };
        }
    }
}
