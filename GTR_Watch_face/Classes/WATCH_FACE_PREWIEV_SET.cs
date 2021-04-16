namespace AmazFit_Watchface_2
{
    /// <summary>набор настроек для предпросмотра</summary>
    public class WATCH_FACE_PREWIEV_SET
    {
        public DateSet Date { get; set; }
        public TimeSet Time { get; set; }
        public ActivitySet Activity { get; set; }
        public WeatherSet Weather { get; set; }
        public StatusSet Status { get; set; }
        public int Battery { get; set; }
        public int SetNumber { get; set; }
    }

    public class DateSet
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int WeekDay { get; set; }
        public int Year { get; set; }
    }

    public class TimeSet
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
    }

    public class ActivitySet
    {
        public int Steps { get; set; }
        public int StepsGoal { get; set; }
        public int Calories { get; set; }
        public int HeartRate { get; set; }
        public int PAI { get; set; }
        public int Distance { get; set; }
        public int StandUp { get; set; }
        public int Stress { get; set; }
        public int ActivityGoal { get; set; }
        public int FatBurning { get; set; }
    }

    public class WeatherSet
    {
        public int Temperature { get; set; }
        public int TemperatureMin { get; set; }
        public int TemperatureMax { get; set; }
        public int Icon { get; set; }
        public bool TemperatureNoData { get; set; }
        public bool TemperatureMinMaxNoData { get; set; }
        public int UVindex { get; set; }
        public int AirQuality { get; set; }
        public int Humidity { get; set; }
        public int WindForce { get; set; }
        public int Altitude { get; set; }
        public int AirPressure { get; set; }
        public bool showTemperature { get; set; }
        //public bool showTemperatureMaxMin { get; set; }
    }

    public class StatusSet
    {
        public bool Bluetooth { get; set; }
        public bool Alarm { get; set; }
        public bool Lock { get; set; }
        public bool DoNotDisturb { get; set; }
    }
}
