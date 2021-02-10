using System.Collections.Generic;

namespace AmazFit_Watchface_2
{
    public class DigitalDialFace
    {
        /// <summary>Набор цифр</summary>
        public List<DigitalTimeDigit> Digits { get; set; }

        /// <summary>Координаты</summary>
        public MultilangImageCoord AM { get; set; }

        /// <summary>Координаты</summary>
        public MultilangImageCoord PM { get; set; }
    }

    public class AnalogDialFace
    {
        /// <summary>Часовая чтрелка</summary>
        public ClockHand Hours { get; set; }

        /// <summary>Минутная чтрелка</summary>
        public ClockHand Minutes { get; set; }

        /// <summary>Секундная чтрелка</summary>
        public ClockHand Seconds { get; set; }
    }

    public class ProgressgDialFace
    {
        /// <summary>Шкала отображения времени</summary>
        public CircleProgress Circle { get; set; }
    }




    public class DigitalTimeDigit
    {
        /// <summary>"Hour", "Minute", "Second"</summary>
        public string TimeType { get; set; }

        /// <summary>"Single", "Follow"</summary>
        public string CombingMode { get; set; }

        public Text Digit { get; set; }

        /// <summary>Разделитель (символ после надписи)</summary>
        public ImageCoord Separator { get; set; }
    }

    public class ClockHand
    {
        public long X { get; set; }

        public long Y { get; set; }

        public MultilangImageCoord Scale { get; set; }

        /// <summary>Центр стрелки</summary>
        public ImageCoord Pointer { get; set; }

        public ImageCoord Cover { get; set; }

        /// <summary>Начальный уго</summary>
        public float StartAngle { get; set; }

        /// <summary>Конечный угол</summary>
        public float EndAngle { get; set; }

        public long? Unknown16 { get; set; }
    }

}
