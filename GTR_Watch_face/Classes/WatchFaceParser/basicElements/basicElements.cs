using System.Collections.Generic;
using System.Drawing;

namespace AmazFit_Watchface_2
{
    public class ProgressBar
    {
        /// <summary>Угол наклона</summary>
        public AngleSettings AngleSettings { get; set; }

        public LinearSettings LinearSettings { get; set; }

        /// <summary>Изображение шкалы прогресса?</summary>
        public long? ForegroundImageIndex { get; set; }

        /// <summary>Цвет</summary>
        public string Color { get; set; }

        /// <summary>Ширена линии</summary>
        public long Width { get; set; }

        /// <summary>Тип окончания линии 0 - arc, 90, triangle, 180 - flat </summary>
        public long Flatness { get; set; }

        /// <summary>Точка на шкале?</summary>
        public long? PointerImageIndex { get; set; }

        /// <summary>Подложка?</summary>
        public long? BackgroundImageIndex { get; set; }
    }

    public class FontRotate
    {
        public long X { get; set; }

        public long Y { get; set; }

        /// <summary>Радиус</summary>
        public long Radius { get; set; }

        /// <summary>Направление аращения 0 - по часовой, 1 - против часовой</summary>
        public float RotateDirection { get; set; }
    }

    public class ImageAmazfit
    {
        public long X { get; set; }

        public long Y { get; set; }

        /// <summary>Изображение при отсутствии данных</summary>
        public long? NoDataImageIndex { get; set; }

        /// <summary>Изображения цифр</summary>
        public List<MultilangImage> MultilangImage { get; set; }

        /// <summary>Десятицный разделитель</summary>
        public long? DecimalPointImageIndex { get; set; }

        /// <summary>Единици измерения</summary>
        public List<MultilangImage> MultilangImageUnit { get; set; }

        /// <summary>Разделитель (символ после надписи)</summary>
        public long? DelimiterImageIndex { get; set; }

        /// <summary>Единици измерения мили</summary>
        public List<MultilangImage> MultilangImageUnitMile { get; set; }
    }

    public class AngleSettings
    {
        public long X { get; set; }

        public long Y { get; set; }

        /// <summary>Начальный уго</summary>
        public float StartAngle { get; set; }

        /// <summary>Конечный угол</summary>
        public float EndAngle { get; set; }

        /// <summary>Радиус</summary>
        public float Radius { get; set; }
    }

    public class ImageCoord
    {
        /// <summary>Координаты</summary>
        public Coordinates Coordinates { get; set; }

        /// <summary>Номер изображения</summary>
        public long ImageIndex { get; set; }

        /// <summary>Номер второго изображения (для преключаемых параметров</summary>
        public long? ImageIndex2 { get; set; }

        /// <summary>Номер третего изображения (для преключаемых параметров</summary>
        public long? ImageIndex3 { get; set; }

        /*
        /// <summary>Начальный уго</summary>
        public float? StartAngle { get; set; }

        /// <summary>Конечный угол</summary>
        public float? EndAngle { get; set; }

        /// <summary>Радиус</summary>
        public float? Radius { get; set; }
        */
    }

    public class ImageProgress
    {
        /// <summary>Набор координат</summary>
        public List<Coordinates> Coordinates { get; set; }

        /// <summary>Набор изображений</summary>
        public ImageSetGTR2 ImageSet { get; set; }

        /// <summary>"Single" = 0, "Continuous" = 1</summary>
        public string DisplayType { get; set; }
    }

    public class ImageSetGTR2
    {
        /// <summary>Номер первого изображения</summary>
        public long ImageIndex { get; set; }

        /// <summary>Количество изображений</summary>
        public long ImagesCount { get; set; }
    }

    public class MultilangImage
    {
        /// <summary>Код языка "Zh", "ZhHant", "All"</summary>
        public string LangCode { get; set; }

        /// <summary>Набор изображений</summary>
        public ImageSetGTR2 ImageSet { get; set; }
    }

    public class MultilangImageCoord
    {
        /// <summary>Координатыа</summary>
        public Coordinates Coordinates { get; set; }

        /// <summary>Набор изображений</summary>
        public List<MultilangImage> ImageSet { get; set; }
    }

    public class SystemFont
    {
        /// <summary>Радиальный текст</summary>
        public FontRotate FontRotate { get; set; }
        /// <summary>Координаты</summary>
        /// 
        public Coordinates Coordinates { get; set; }

        /// <summary>Угол наклона</summary>
        public long Angle { get; set; }

        /// <summary>Размер шрифта</summary>
        public long Size { get; set; }

        /// <summary>Цвет</summary>
        public string Color { get; set; }

        /// <summary>Отображать единици измерения?</summary>
        public bool ShowUnit { get; set; }
    }

    public class Text
    {
        /// <summary>Надпись</summary>
        public ImageAmazfit Image { get; set; }

        /// <summary>Шрифт</summary>
        public SystemFont SystemFont { get; set; }

        /// <summary>Выравнивание "Left", "Right", "Center"</summary>
        public string Alignment { get; set; }

        /// <summary>Отступы</summary>
        public long? Spacing { get; set; }

        /// <summary>Показывать ведущие нули</summary>
        public bool PaddingZero { get; set; }

        /// <summary>Для месяца усли true то картинкой иначе номером</summary>
        public bool DisplayFormAnalog { get; set; }
    }

    public class Coordinates
    {
        public long X { get; set; }

        public long Y { get; set; }
    }

    public class LinearSettings
    {
        public long StartX { get; set; }

        public long StartY { get; set; }

        public long EndX { get; set; }

        public long EndY { get; set; }
    }


}
