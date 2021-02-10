using System.Collections.Generic;
using System.Drawing;

namespace AmazFit_Watchface_2
{
    public class SystemAmazfit
    {
        /// <summary>Статусы</summary>
        public Status Status { get; set; }

        /// <summary>Дата</summary>
        public Date Date { get; set; }

        /// <summary>Активности</summary>
        public List<Activity> Activity { get; set; }
    }



    public class Status
    {
        /// <summary>Bluetooth</summary>
        public ImageCoord Bluetooth { get; set; }

        /// <summary>DoNotDisturb</summary>
        public ImageCoord DoNotDisturb { get; set; }

        /// <summary>Блокировка</summary>
        public ImageCoord Lock { get; set; }

        /// <summary>Будильник</summary>
        public ImageCoord Alarm { get; set; }
    }

    public class Date
    {
        /// <summary>Дата</summary>
        public List<DigitalDateDigit> DateDigits { get; set; }

        /// <summary>День недели?</summary>
        public DigitalCommonDigit WeeksDigits { get; set; }

        /// <summary>Прогресс даты</summary>
        public Progress DateProgress { get; set; }

        /// <summary>День недели</summary>
        public DOWProgress DOWProgress { get; set; }
    }

    public class Activity
    {
        /// <summary>Battery = 1; Steps = 2; Calories = 3; HeartRate = 4; PAI = 5; 
        /// Distance = 6; StandUp = 7; Weather = 8; UVindex = 9; AirQuality = 10; 
        /// Humidity = 11; Sunrise = 12 # Two Images possible; ActivityGoal = 17# Two Images possible; 
        /// FatBurning = 18</summary>
        public string Type { get; set; }

        /// <summary>Отображение данных стрелками</summary>
        public ClockHand PointerProgress { get; set; }

        /// <summary>Отображение данных шкалой</summary>
        public CircleProgress CircleProgress { get; set; }

        /// <summary>Отображение данных текстом</summary>
        public ImageProgress ImageProgress { get; set; }

        /// <summary>Отображение данных текстом</summary>
        public List<DigitalCommonDigit> Digits { get; set; }

        /// <summary>Отображение иконки активности</summary>
        public ImageCoord Icon { get; set; }
    }




    public class DigitalDateDigit
    {
        /// <summary>"Year", "Month", "Day"</summary>
        public string DateType { get; set; }

        /// <summary>"Single", "Follow"</summary>
        public string CombingMode { get; set; }

        /// <summary>Надпись</summary>
        public Text Digit { get; set; }

        /// <summary>Разделитель</summary>
        public ImageCoord Separator { get; set; }
    }

    public class DigitalCommonDigit
    {
        /// <summary>1-"Min", 2-"Max"</summary>
        public string Type { get; set; }

        /// <summary>"Single", "Follow"</summary>
        public string CombingMode { get; set; }

        /// <summary>Надпись</summary>
        public Text Digit { get; set; }

        /// <summary>Разделитель</summary>
        public ImageCoord Separator { get; set; }
    }

    public class Progress
    {
        /// <summary>Отображение стрелками</summary>
        public ClockHand AnalogDialFace { get; set; }

        public long? Unknown2 { get; set; }

        /// <summary>Отображение стрелками</summary>
        public ClockHand ClockHand { get; set; }
    }

    public class DOWProgress
    {
        /// <summary>Круговой прогрес</summary>
        public CircleProgress Circle { get; set; }
    }
}
