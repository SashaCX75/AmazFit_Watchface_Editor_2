namespace AmazFit_Watchface_2
{
    /// <summary>набор настроек для предпросмотра</summary>
    public class WATCH_FACE_PREWIEV_SET
    {
        public DateS Date { get; set; }
        public TimeS Time { get; set; }
        public ActivityS Activity { get; set; }
        public StatusS Status { get; set; }
        public int Battery { get; set; }
    }

    public class DateS
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int WeekDay { get; set; }
        public int Year { get; set; }
    }

    public class TimeS
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
    }

    public class ActivityS
    {
        public int Calories { get; set; }
        public int Pulse { get; set; }
        public int Distance { get; set; }
        public int Steps { get; set; }
        public int StepsGoal { get; set; }
    }

    public class StatusS
    {
        public bool Bluetooth { get; set; }
        public bool Alarm { get; set; }
        public bool Lock { get; set; }
        public bool DoNotDisturb { get; set; }
    }
}
