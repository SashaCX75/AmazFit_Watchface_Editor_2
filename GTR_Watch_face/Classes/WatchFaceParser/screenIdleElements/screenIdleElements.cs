using System.Collections.Generic;

namespace AmazFit_Watchface_2
{
    public class ScreenIdle
    {
        /// <summary>Время</summary>
        public ScreenNormal DialFace { get; set; }

        /// <summary>Дата</summary>
        public DateAmazfit Date { get; set; }

        /// <summary>Активности</summary>
        public List<Activity> Activity { get; set; }

        /// <summary>Задний фон</summary>
        public long? BackgroundImageIndex { get; set; }
    }

}
