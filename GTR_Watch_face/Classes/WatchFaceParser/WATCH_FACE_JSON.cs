namespace AmazFit_Watchface_2
{

    /// <summary>Корневая структура JSON файла</summary>
    public class WATCH_FACE_JSON
    {
        /// <summary>Id модели часов</summary>
        public Device_Id Info { get; set; }

        /// <summary>Задний фон</summary>
        public Background Background { get; set; }

        /// <summary>Основной экран</summary>
        public ScreenNormal DialFace { get; set; }

        /// <summary>Системные данные</summary>
        public SystemAmazfit System { get; set; }

        /// <summary>Виджеты (настраевыемые элементы циферблата)</summary>
        public Widgets Widgets { get; set; }

        /// <summary>AOD?</summary>
        public ScreenIdle ScreenIdle { get; set; }
    }

}
