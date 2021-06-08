using Newtonsoft.Json;
using System.Collections.Generic;

namespace AmazFit_Watchface_2
{

    public class Device_Id
    {
        /// <summary> Id модели часов</summary>
        public long DeviceId { get; set; }

        /// <summary> Id циферблата</summary>
        public long? WatchFaceId { get; set; }
    }
    public class Background
    {
        /// <summary>Изображение заднего фона</summary>
        public List<MultilangImage> Preview { get; set; }

        /// <summary>Изображение для предпросмотра</summary>
        public long? BackgroundImageIndex { get; set; }

        /// <summary>Цвет фона</summary>
        public string Color { get; set; }
    }
    public class ScreenNormal
    {
        /// <summary>Цифровое время</summary>
        public DigitalDialFace DigitalDialFace { get; set; }

        /// <summary>Аналоговое время/summary>
        public AnalogDialFace AnalogDialFace { get; set; }

        /// <summary>Время в виде шкалы</summary>
        public ProgressgDialFace ProgressDialFace { get; set; }
    }


}
