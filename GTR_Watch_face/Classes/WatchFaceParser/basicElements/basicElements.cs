using System.Collections.Generic;
using System.Drawing;

namespace AmazFit_Watchface_2
{
    public class CircleProgress
    {
        /// <summary>Угол наклона</summary>
        public ImageAngle AngleSettings { get; set; }

        public long? Unknown2 { get; set; }

        /// <summary>Фоновое изображение?</summary>
        public long ForegroundImageIndex { get; set; }

        /// <summary>Цвет</summary>
        public string Color { get; set; }

        /// <summary>Ширена линии</summary>
        public long Width { get; set; }

        /// <summary>Тип окончания линии 0 - flat, 90, triangle, 180 - arc </summary>
        public long Flatness { get; set; }
        public long? Unknown7 { get; set; }

        /// <summary>Номер изображения</summary>
        public long ImageIndex { get; set; }
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

        public List<MultilangImage> MultilangImageUnitMile { get; set; }
    }

    public class ImageAngle
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
        /// <summary>Координатыа</summary>
        public Coordinates Coordinates { get; set; }

        /// <summary>Номер изображения</summary>
        public long ImageIndex { get; set; }

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
        //TODO наверно тип bool
        public bool PaddingZero { get; set; }

        public bool? DisplayFormAnalog { get; set; }
    }

    public class Coordinates
    {
        public long X { get; set; }

        public long Y { get; set; }
    }


}
