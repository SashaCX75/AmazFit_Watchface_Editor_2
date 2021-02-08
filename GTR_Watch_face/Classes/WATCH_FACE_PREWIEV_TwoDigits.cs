namespace AmazFit_Watchface_2
{
    /// <summary>отдельные цифры для даты и времени</summary>
    public class WATCH_FACE_PREWIEV_TwoDigits
    {
        public DateP Date { get; set; }
        public TimeP Time { get; set; }
        public TimePmP TimePm { get; set; }
        public YearP Year { get; set; }
    }

    public class DateP
    {
        public TwoDigitsP Month { get; set; }
        public TwoDigitsP Day { get; set; }
    }

    public class TimeP
    {
        public TwoDigitsP Hours { get; set; }
        public TwoDigitsP Minutes { get; set; }
        public TwoDigitsP Seconds { get; set; }
    }

    public class TimePmP
    {
        public TwoDigitsP Hours { get; set; }
        public TwoDigitsP Minutes { get; set; }
        public TwoDigitsP Seconds { get; set; }
        public bool Pm { get; set; }
    }

    public class TwoDigitsP
    {
        public int Tens { get; set; }
        public int Ones { get; set; }
    }

    public class YearP
    {
        public int Thousands { get; set; }
        public int Hundreds { get; set; }
        public int Tens { get; set; }
        public int Ones { get; set; }
    }
}
