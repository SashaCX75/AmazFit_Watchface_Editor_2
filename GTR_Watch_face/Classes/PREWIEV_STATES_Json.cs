namespace AmazFit_Watchface_2
{
    public class PREWIEV_STATES_Json
    {
        public TimePreview Time { get; set; }
        public int Steps { get; set; }
        public int Goal { get; set; }
        public int Pulse { get; set; }
        public int BatteryLevel { get; set; }
        public int Distance { get; set; }
        public int Calories { get; set; }
        public bool Bluetooth { get; set; }
        public bool Unlocked { get; set; }
        public bool Alarm { get; set; }
        public bool DoNotDisturb { get; set; }
    }

    public class TimePreview
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
    }
}
