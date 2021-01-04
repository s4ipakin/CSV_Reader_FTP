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
    CO2
}

namespace VDI_VDO_Graf.GrafSettings
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
        public override GrafType GrafType => GrafType.Temp;
        public override string yAxesName => "Температура";
        public override string unit => "°C";
        public override double maxValue => 120;

        public override GrafSeries[] GetSettings()
        {
            return new GrafSeries[]
            {
                new GrafSeries("Температура ВВО(основная петля)", 7),
                new GrafSeries("Температура ВВО(дополнительная петля)", 9),
                new GrafSeries("Температура ВДИ\r\n(поступающая в емкость для хранения)", 23),
                new GrafSeries("Температура ВДИ(циркулирующая)", 27),
            };
        }
    }

    public class PressureGrafSet : GrafSet
    {
        public override GrafType GrafType => GrafType.Pressure;
        public override string yAxesName => "Давление";
        public override string unit => "Bar";
        public override double maxValue => 6;
        public override GrafSeries[] GetSettings()
        {
            return new GrafSeries[]
            {
                new GrafSeries("Давление ВВО(основная петля)", 17),
                new GrafSeries("Давление ВВО(дополнительная петля)", 19),
                new GrafSeries("Давление ВДИ(для хранения)", 33)
            };
        }
    }

    public class ConductivityGrafSet : GrafSet
    {
        public override GrafType GrafType => GrafType.Conductivity;
        public override string yAxesName => "Электропроводимость";
        public override string unit => "uSm";
        public override double maxValue => 5;
        public override GrafSeries[] GetSettings()
        {
            return new GrafSeries[]
            {
                new GrafSeries("Электропроводность ВВО(емкость)", 11),
                new GrafSeries("Электропроводность ВВО(основная петля)", 13),
                new GrafSeries("Электропроводность ВВО(дополнительная петля)", 15),
                new GrafSeries("Электропроводность ВДИ(емкость)", 21),
                new GrafSeries("Электропроводность ВДИ(циркулирующая)", 25)
            };
        }
    }

    public class FlawGrafSet : GrafSet
    {
        public override GrafType GrafType => GrafType.Flaw;
        public override string yAxesName => "Скорость потока";
        public override string unit => "м³/ч";
        public override double maxValue => 10;
        public override GrafSeries[] GetSettings()
        {
            return new GrafSeries[]
            {
                new GrafSeries("Скорость потока ВВО(основная петля)", 3),
                new GrafSeries("Скорость потока ВВО(дополнительная петля)", 5),
                new GrafSeries("Скорость потока ВДИ", 31),
            };
        }
    }

    public class LevelGrafSet : GrafSet
    {
        public override GrafType GrafType => GrafType.Level;
        public override string yAxesName => "Уровень";
        public override string unit => "%";
        public override double maxValue => 150;
        public override GrafSeries[] GetSettings()
        {
            return new GrafSeries[]
            {
                new GrafSeries("Уровень ВДИ", 35)
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
