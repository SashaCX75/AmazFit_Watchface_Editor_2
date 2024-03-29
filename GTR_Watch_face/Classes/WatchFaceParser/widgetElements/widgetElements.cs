﻿using System.Collections.Generic;

namespace AmazFit_Watchface_2
{
    public class Widgets
    {
        public long TopMaskImageIndex { get; set; }

        public long UnderMaskImageIndex { get; set; }

        public long Unknown4 { get; set; }

        public List<Widget> Widget { get; set; }

    }

    public class Widget
    {
        public long X { get; set; }

        public long Y { get; set; }

        /// <summary>Ширина</summary>
        public long Width { get; set; }

        /// <summary>Высота</summary>
        public long Height { get; set; }

        public List<WidgetElement> WidgetElement { get; set; }

        public long BorderActivImageIndex { get; set; }

        public long BorderInactivImageIndex { get; set; }

        public ImageCoord DescriptionImageBackground { get; set; }

        public long DescriptionWidthCheck { get; set; }
    }
    public class WidgetElement
    {
        public List<MultilangImage> Preview { get; set; }

        public DateAmazfit Date { get; set; }

        public List<Activity> Activity { get; set; }

    }

}
