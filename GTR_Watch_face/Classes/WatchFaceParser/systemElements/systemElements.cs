using System.Collections.Generic;
using System.Drawing;

namespace AmazFit_Watchface_2
{
    public class SystemAmazfit
    {
        /// <summary>Статусы</summary>
        public Status Status { get; set; }

        /// <summary>Дата</summary>
        public DateAmazfit Date { get; set; }

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

    public class DateAmazfit
    {
        /// <summary>Дата</summary>
        public List<DigitalDateDigit> DateDigits { get; set; }

        /// <summary>День недели?</summary>
        public DigitalCommonDigit WeeksDigits { get; set; }

        /// <summary>Прогресс даты</summary>
        public DateClockHand DateClockHand { get; set; }

        /// <summary>День недели</summary>
        public DateProgressBar DateProgressBar { get; set; }
    }

    public class Activity
    {
        /// <summary>Battery = 1; Steps = 2; Calories = 3; HeartRate = 4; PAI = 5; 
        /// Distance = 6; StandUp = 7; Weather = 8; UVindex = 9; AirQuality = 10; 
        /// Humidity = 11; Sunrise = 12 # Two Images possible; WindForce = 13; 
        /// Altitude = 14; AirPressure = 15; Stress = 16;
        /// ActivityGoal = 17# Two Images possible; FatBurning = 18</summary>
        public string Type { get; set; }

        /// <summary>Отображение данных стрелками</summary>
        public ClockHand PointerProgress { get; set; }

        /// <summary>Отображение данных шкалой</summary>
        public ProgressBar ProgressBar { get; set; }

        /// <summary>Отображение данных набором картинок?</summary>
        public ImageProgress ImageProgress { get; set; }

        /// <summary>Отображение данных текстом</summary>
        public List<DigitalCommonDigit> Digits { get; set; }

        /// <summary>Ярлыки</summary>
        public Shortcut Shortcut { get; set; }

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

    public class DateClockHand
    {
        /// <summary>Отображение месяца стрелками</summary>
        public ClockHand MonthClockHand { get; set; }

        /// <summary>Отображение даты стрелками</summary>
        public ClockHand DayClockHand { get; set; }

        /// <summary>Отображение дня недели стрелками</summary>
        public ClockHand WeekDayClockHand { get; set; }
    }

    public class DateProgressBar
    {
        /// <summary>Месяц круговой шкалой</summary>
        public ProgressBar MonthProgressBar { get; set; }

        /// <summary>Датакруговой шкалой</summary>
        public ProgressBar DayProgressBar { get; set; }

        /// <summary>День недели круговой шкалой</summary>
        public ProgressBar WeekDayProgressBar { get; set; }
    }

    public class Shortcut
    {
        /// <summary>Прямоугольная область ярлыка</summary>
        public BoxElement BoxElement { get; set; }
        public long ImageIndex { get; set; }
    }

    public class BoxElement
    {
        /// <summary>Координата Х левого верхнего угла</summary>
        public long TopLeftX { get; set; }

        /// <summary>Координата Y левого верхнего угла</summary>
        public long TopLeftY { get; set; }

        /// <summary>Ширина</summary>
        public long Width { get; set; }

        /// <summary>Высота</summary>
        public long Height { get; set; }
    }
}
