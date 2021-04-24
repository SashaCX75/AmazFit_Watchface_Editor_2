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

        public int PAI { get; set; }
        public int Stand { get; set; }
        public int Stress = 7;
        public int ActivityGoal = 7;
        public int FatBurning = 7;

        public int CurrentWeather { get; set; }
        public int CurrentTemperature { get; set; }
        public int TemperatureMax = 5;
        public int TemperatureMin = -15;
        public int UVindex { get; set; }
        public int AirQuality { get; set; }
        public int Humidity { get; set; }
        public int WindForce = 7;
        public int Altitude = 1000;
        public int AirPressure = 1000;

        public bool Bluetooth { get; set; }
        public bool Unlocked { get; set; }
        public bool Alarm { get; set; }
        public bool DoNotDisturb { get; set; }
        public bool ShowTemperature = true;
        //public bool ShowTemperatureMaxMin = true;

        //public bool ScreenIdle { get; set; }

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
