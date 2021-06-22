using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmazFit_Watchface_2
{
    public partial class Form1 : Form
    {
        public void JSON_read_Widgets()
        {
            if (Watch_Face == null || Watch_Face.Widgets == null) return;
            comboBoxSetText(comboBox_WidgetsUnderMask, Watch_Face.Widgets.UnderMaskImageIndex);
            comboBoxSetText(comboBox_WidgetsTopMask, Watch_Face.Widgets.TopMaskImageIndex);
        }

        public void DrawWidgets(Graphics gPanel, float scale, bool crop, bool WMesh, bool BMesh, bool BBorder,
            bool showShortcuts, bool showShortcutsArea, bool showShortcutsBorder, bool showAnimation, bool showProgressArea,
            bool showCentrHend, bool showWidgetsArea)
        {
            if (Watch_Face == null || Watch_Face.Widgets == null) return;
            if (Watch_Face.Widgets.Widget == null || Watch_Face.Widgets.Widget.Count < 1) return;
            int widgetIndex = comboBox_WidgetNumber.SelectedIndex;
            for (int i = 0; i < Watch_Face.Widgets.Widget.Count; i++)
            {
                int widgetElementIndex = 0;
                if (i== widgetIndex) widgetElementIndex = SelectedWidgetElement();
                if (Watch_Face.Widgets.Widget[i].WidgetElement != null &&
                    Watch_Face.Widgets.Widget[i].WidgetElement.Count > 0)
                {
                    WidgetElement widgetElement = Watch_Face.Widgets.Widget[i].WidgetElement[widgetElementIndex];
                    DrawWidgetElement(widgetElement,
                        gPanel, scale, crop, WMesh, BMesh, BBorder, showShortcuts, showShortcutsArea,
                        showShortcutsBorder, showAnimation, showProgressArea, showCentrHend);
                }

                if (showWidgetsArea)
                {
                    Logger.WriteLine("DrawWidgetBorder");
                    int x = (int)Watch_Face.Widgets.Widget[i].X;
                    int y = (int)Watch_Face.Widgets.Widget[i].Y;
                    int width = (int)Watch_Face.Widgets.Widget[i].Width;
                    int height = (int)Watch_Face.Widgets.Widget[i].Height;
                    Rectangle rect = new Rectangle(x, y, width - 1, height - 1);
                    using (Pen pen1 = new Pen(Color.White, 1))
                    {
                        gPanel.DrawRectangle(pen1, rect);
                    }
                    using (Pen pen2 = new Pen(Color.Black, 1))
                    {
                        pen2.DashStyle = DashStyle.Dot;
                        gPanel.DrawRectangle(pen2, rect);
                    }

                    Logger.WriteLine("DrawWidgetBorder (end)");
                }
            }
        }

        public int SelectedWidgetElement()
        {
            if (dataGridView_WidgetElement.SelectedCells.Count <= 0) return 0;
            int RowIndex = 0;
            for (int i = 0; i < dataGridView_WidgetElement.Rows.Count; i++)
            {
                if (dataGridView_WidgetElement.Rows[i].Visible)
                {
                    if (dataGridView_WidgetElement.Rows[i].Selected) break;
                    else RowIndex++;
                }
            }
            return RowIndex;
        }

        public void DrawWidgetElement(WidgetElement widgetElement, Graphics gPanel, float scale, 
            bool crop, bool WMesh, bool BMesh, bool BBorder,bool showShortcuts, bool showShortcutsArea, 
            bool showShortcutsBorder, bool showAnimation, bool showProgressArea, bool showCentrHend)
        {
            //WidgetElement widgetElement = Watch_Face.Widgets.Widget[widgetIndex].WidgetElement[widgetElementIndex];
            Bitmap src = new Bitmap(1, 1);
            // активности
            if (widgetElement.Activity != null)
            {
                foreach (Activity activity in widgetElement.Activity)
                {
                    float activityValue = 0;
                    float activityGoal = 100;
                    float activityGoal2 = 100;
                    float progress = 1;
                    int value_lenght = 2;
                    switch (activity.Type)
                    {
                        case "Battery":
                            activityValue = Watch_Face_Preview_Set.Battery;
                            activityGoal = 100;
                            progress = activityValue / activityGoal;
                            value_lenght = 3;
                            break;
                        case "Steps":
                            activityValue = Watch_Face_Preview_Set.Activity.Steps;
                            activityGoal = Watch_Face_Preview_Set.Activity.StepsGoal;
                            progress = activityValue / activityGoal;
                            value_lenght = 5;
                            break;
                        case "Calories":
                            activityValue = Watch_Face_Preview_Set.Activity.Calories;
                            activityGoal = 300;
                            progress = activityValue / activityGoal;
                            value_lenght = 4;
                            break;
                        case "HeartRate":
                            activityValue = Watch_Face_Preview_Set.Activity.HeartRate;
                            activityGoal = 180;
                            progress = activityValue / 181f;
                            value_lenght = 3;
                            break;
                        case "PAI":
                            activityValue = Watch_Face_Preview_Set.Activity.PAI;
                            activityGoal = 100;
                            progress = activityValue / activityGoal;
                            value_lenght = 3;
                            break;
                        case "Distance":
                            activityValue = Watch_Face_Preview_Set.Activity.Distance / 1000f;
                            activityValue = (float)Math.Round(activityValue, 2, MidpointRounding.AwayFromZero);
                            activityGoal = activityValue;
                            progress = activityValue / activityGoal;
                            value_lenght = 5;
                            break;
                        case "StandUp":
                            activityValue = Watch_Face_Preview_Set.Activity.StandUp;
                            activityGoal = 12;
                            progress = activityValue / activityGoal;
                            value_lenght = 2;
                            break;
                        case "Weather":
                            activityValue = Watch_Face_Preview_Set.Weather.Temperature;
                            activityGoal = Watch_Face_Preview_Set.Weather.TemperatureMin;
                            activityGoal2 = Watch_Face_Preview_Set.Weather.TemperatureMax;
                            progress = activityValue / activityGoal;
                            break;
                        case "UVindex":
                            activityValue = Watch_Face_Preview_Set.Weather.UVindex;
                            activityGoal = 10;
                            progress = activityValue / activityGoal;
                            value_lenght = 2;
                            break;
                        case "AirQuality":
                            activityValue = Watch_Face_Preview_Set.Weather.AirQuality;
                            activityGoal = 500;
                            progress = activityValue / 503f;
                            value_lenght = 3;
                            break;
                        case "Humidity":
                            activityValue = Watch_Face_Preview_Set.Weather.Humidity;
                            activityGoal = 100;
                            progress = activityValue / activityGoal;
                            value_lenght = 2;
                            break;
                        case "Sunrise":
                            activityValue = 19.30f;
                            activityGoal = 5.30f;
                            activityGoal2 = 19.30f;

                            int hour = Watch_Face_Preview_Set.Time.Hours;
                            int minute = Watch_Face_Preview_Set.Time.Minutes;
                            TimeSpan time_now = new TimeSpan(hour, minute, 0);
                            TimeSpan time_sunrise = new TimeSpan(5, 30, 0);
                            TimeSpan time_sunset = new TimeSpan(19, 30, 0);
                            TimeSpan day_lenght = time_sunset - time_sunrise;
                            TimeSpan day_progress = time_now - time_sunrise;

                            progress = (float)(day_progress.TotalSeconds / day_lenght.TotalSeconds);
                            if (progress > 1) 
                            {
                                progress = 1;
                                activityValue = 5.30f;
                            }
                            if (progress < 0)
                            {
                                progress = 0;
                                activityValue = 5.30f;
                            }
                            value_lenght = 4;
                            break;
                        case "WindForce":
                            activityValue = Watch_Face_Preview_Set.Weather.WindForce;
                            activityGoal = 12;
                            progress = activityValue / activityGoal;
                            value_lenght = 2;
                            break;
                        case "Altitude":
                            activityValue = Watch_Face_Preview_Set.Weather.Altitude;
                            activityGoal = 9000;
                            progress = (Watch_Face_Preview_Set.Weather.Altitude + 1000) / 10000f;
                            value_lenght = 4;
                            break;
                        case "AirPressure":
                            activityValue = Watch_Face_Preview_Set.Weather.AirPressure;
                            activityGoal = 1200;
                            progress = (Watch_Face_Preview_Set.Weather.AirPressure - 170) / 1000f;
                            value_lenght = 4;
                            break;
                        case "Stress":
                            activityValue = Watch_Face_Preview_Set.Activity.Stress;
                            activityGoal = 12;
                            progress = Watch_Face_Preview_Set.Activity.Stress / 12f;
                            value_lenght = 2;
                            break;
                        case "ActivityGoal":
                            activityValue = Watch_Face_Preview_Set.Activity.Steps;
                            activityGoal = Watch_Face_Preview_Set.Activity.StepsGoal;
                            progress = activityValue / activityGoal;
                            value_lenght = 4;
                            break;
                        case "FatBurning":
                            activityValue = Watch_Face_Preview_Set.Activity.FatBurning;
                            activityGoal = 30;
                            progress = activityValue / activityGoal;
                            value_lenght = 2;
                            break;
                    }
                    if (progress > 1) progress = 1;

                    // иконка
                    if (activity.Icon != null && activity.Icon.Coordinates != null)
                    {
                        int _imageIndex = (int)activity.Icon.ImageIndex - 1;
                        int _x = (int)activity.Icon.Coordinates.X;
                        int _y = (int)activity.Icon.Coordinates.Y;

                        if (_imageIndex >= 0)
                        {
                            src = OpenFileStream(ListImagesFullName[_imageIndex]);
                            gPanel.DrawImage(src, new Rectangle(_x, _y, src.Width, src.Height));
                        }
                    }

                    // набор картинок
                    if (activity.ImageProgress != null && activity.ImageProgress.ImageSet != null &&
                            activity.ImageProgress.Coordinates != null && OneCoordinates(activity.ImageProgress.Coordinates))

                    {
                        int _imageIndex = (int)activity.ImageProgress.ImageSet.ImageIndex - 1;
                        int _x = (int)activity.ImageProgress.Coordinates[0].X;
                        int _y = (int)activity.ImageProgress.Coordinates[0].Y;
                        int _imagesCount = (int)activity.ImageProgress.ImageSet.ImagesCount;

                        int offSet = (int)((_imagesCount + 1) * progress);
                        if (offSet < 0) offSet = 0;
                        if (offSet >= _imagesCount) offSet = (int)(_imagesCount - 1);
                        int imageIndex = _imageIndex + offSet;

                        if (imageIndex < ListImagesFullName.Count)
                        {
                            src = OpenFileStream(ListImagesFullName[imageIndex]);
                            gPanel.DrawImage(src, new Rectangle(_x, _y, src.Width, src.Height));
                        }

                    }

                    // круговая шкала
                    if (activity.ProgressBar != null && activity.ProgressBar.AngleSettings != null)
                    {
                        bool _image = false;
                        Color _color = Color.Black;
                        int _imageIndex = -1;
                        int _backgroundImageIndex = -1;
                        int _x = (int)activity.ProgressBar.AngleSettings.X;
                        int _y = (int)activity.ProgressBar.AngleSettings.Y;
                        int _width = 0;
                        int _startAngle = 0;
                        int _endAngle = 0;
                        int _radius = 0;
                        int _flatness = 0;
                        if (activity.ProgressBar.ForegroundImageIndex != null)
                        {
                            _image = true;
                            _imageIndex = (int)activity.ProgressBar.ForegroundImageIndex-1;
                        }
                        else
                        {
                            if (activity.ProgressBar.Color != null)
                            {
                                _color = ColorRead(activity.ProgressBar.Color);
                            }
                        }

                        if (activity.ProgressBar.BackgroundImageIndex != null)
                            _backgroundImageIndex = (int)activity.ProgressBar.BackgroundImageIndex - 1;

                        _width = (int)activity.ProgressBar.Width;

                        _x = (int)activity.ProgressBar.AngleSettings.X;
                        _y = (int)activity.ProgressBar.AngleSettings.Y;
                        _startAngle = (int)activity.ProgressBar.AngleSettings.StartAngle;
                        _endAngle = (int)activity.ProgressBar.AngleSettings.EndAngle;
                        _radius = (int)activity.ProgressBar.AngleSettings.Radius;
                        _flatness = (int)activity.ProgressBar.Flatness;



                        
                        float StartAngle = _startAngle - 90;
                        float EndAngle = (float)(_endAngle - _startAngle);
                        if (_image)
                        {
                            if (_imageIndex >= 0)
                            {
                                DrawScaleCircle_image(gPanel, _x, _y, _radius, _width, _flatness, StartAngle, EndAngle, progress,
                                 _imageIndex, _backgroundImageIndex, showProgressArea);
                            }
                        }
                        else
                        {
                            DrawScaleCircle(gPanel, _x, _y, _radius, _width, _flatness, StartAngle, EndAngle, progress,
                                _color, _backgroundImageIndex, showProgressArea);
                        }
                    }

                    // линейная шкала
                    if (activity.ProgressBar != null && activity.ProgressBar.LinearSettings != null)
                    {
                        bool _image = false;
                        Color _color = Color.Black;
                        int _imageIndex = -1;
                        int _backgroundImageIndex = -1;
                        int _pointerImageIndex = -1;
                        int _x = (int)activity.ProgressBar.LinearSettings.StartX;
                        int _y = (int)activity.ProgressBar.LinearSettings.StartY;
                        int _width = 0;
                        int _length = 0;
                        int _flatness = 0;

                        if (activity.ProgressBar.ForegroundImageIndex != null)
                        {
                            _image = true;
                            _imageIndex = (int)activity.ProgressBar.ForegroundImageIndex - 1;
                        }
                        else
                        {
                            if (activity.ProgressBar.Color != null)
                            {
                                _color = ColorRead(activity.ProgressBar.Color);
                            }
                        }
                        if (activity.ProgressBar.PointerImageIndex != null)
                            _pointerImageIndex = (int)activity.ProgressBar.PointerImageIndex - 1;
                        if (activity.ProgressBar.BackgroundImageIndex != null)
                            _backgroundImageIndex = (int)activity.ProgressBar.BackgroundImageIndex - 1;

                        _x = (int)activity.ProgressBar.LinearSettings.StartX;
                        _y = (int)activity.ProgressBar.LinearSettings.StartY;
                        _length = (int)(activity.ProgressBar.LinearSettings.EndX - activity.ProgressBar.LinearSettings.StartX);
                        _width = (int)activity.ProgressBar.Width;
                        _flatness = (int)activity.ProgressBar.Flatness;


                        if (_image)
                        {
                            if (_imageIndex >= 0)
                            {
                                DrawScaleLinearPointer_image(gPanel, _x, _y, _length, _width, progress, _imageIndex, _flatness, _pointerImageIndex, _backgroundImageIndex, showProgressArea);
                            }
                        }
                        else
                        {
                            DrawScaleLinearPointer(gPanel, _x, _y, _length, _width, progress, _color, _flatness, _pointerImageIndex, _backgroundImageIndex, showProgressArea);
                        }
                    }

                    // надпись
                    if (activity.Digits != null && activity.Digits.Count > 0)
                    {
                        float value = activityValue;
                        string sValueOldSF = "";
                        string sValueOldFR = "";
                        int _offsetX = -1;
                        int _offsetY = -1;
                        int _offsetSpacing = 0;

                        int _offsetXSF = -1;
                        int _offsetYSF = -1;
                        int _spacingSF = 0;
                        int _sizeSF = 0;
                        int _angleSF = 0;
                        Color _colorSF = Color.Black;

                        int _offsetXFR = -1;
                        int _offsetYFR = -1;
                        int _spacingFR = 0;
                        int _sizeFR = 0;
                        int _radiusFR = 0;
                        float _angleFR = 0;
                        int _rotate_directionFR = 0;
                        Color _colorFR = Color.Black;

                        foreach (DigitalCommonDigit digitalCommonDigit in activity.Digits)
                        {
                            if (digitalCommonDigit.Type != null && digitalCommonDigit.Type == "Min")
                                value = activityGoal;
                            if (digitalCommonDigit.Type != null && digitalCommonDigit.Type == "Max")
                                value = activityGoal2;

                            // надпись
                            int ActivityType = 0;
                            if (activity.Type == "ActivityGoal") ActivityType = 17;
                            if (activity.Type == "Humidity") ActivityType = 11;
                            if (digitalCommonDigit.Digit != null && digitalCommonDigit.Digit.Image != null)
                            {
                                int _imageIndex = -1;
                                int _imageDecimalIndex = -1;
                                int _imageNoDataIndex = -1;
                                int _imageUnitIndex = -1;
                                int _iconIndex = -1;
                                int _x = (int)digitalCommonDigit.Digit.Image.X;
                                int _y = (int)digitalCommonDigit.Digit.Image.Y;
                                int _iconX = 0;
                                int _iconY = 0;
                                bool _follow = false;
                                bool _addZero = false;
                                int _alignment = 0;
                                int _spacing = 0;
                                if (digitalCommonDigit.CombingMode == null ||
                                    digitalCommonDigit.CombingMode == "Follow") _follow = true;

                                // десятичный разделитель
                                if (activity.Type == "Distance")
                                {
                                    //ComboBox comboBox_DecimalPoint = (ComboBox)panel_text.Controls[12];
                                    if (digitalCommonDigit.Digit.Image.DecimalPointImageIndex != null)
                                        _imageDecimalIndex = (int)digitalCommonDigit.Digit.Image.DecimalPointImageIndex - 1;
                                }

                                if (digitalCommonDigit.Digit.Image.NoDataImageIndex != null)
                                    _imageNoDataIndex = (int)digitalCommonDigit.Digit.Image.NoDataImageIndex - 1;

                                foreach (MultilangImage multilangImage in digitalCommonDigit.Digit.Image.MultilangImage)
                                {
                                    if (multilangImage.LangCode == "All")
                                        _imageIndex = (int)multilangImage.ImageSet.ImageIndex - 1;
                                }
                                if (digitalCommonDigit.Digit.Image.MultilangImageUnit != null)
                                {
                                    foreach (MultilangImage multilangImage in digitalCommonDigit.Digit.Image.MultilangImageUnit)
                                    {
                                        if (multilangImage.LangCode == "All")
                                            _imageUnitIndex = (int)multilangImage.ImageSet.ImageIndex - 1;
                                    }
                                }

                                switch (digitalCommonDigit.Digit.Alignment)
                                {
                                    case "Left":
                                        _alignment = 0;
                                        break;
                                    case "Right":
                                        _alignment = 1;
                                        break;
                                    case "Center":
                                        _alignment = 2;
                                        break;
                                }
                                if (digitalCommonDigit.Digit.Spacing != null)
                                    _spacing = (int)digitalCommonDigit.Digit.Spacing;
                                _addZero = digitalCommonDigit.Digit.PaddingZero;
                                if (digitalCommonDigit.Separator != null)
                                {
                                    _iconIndex = (int)digitalCommonDigit.Separator.ImageIndex - 1;
                                    _iconX = (int)digitalCommonDigit.Separator.Coordinates.X;
                                    _iconY = (int)digitalCommonDigit.Separator.Coordinates.Y;
                                }


                                if (_imageIndex >= 0)
                                {
                                    if (_follow && _offsetX > -1)
                                    {
                                        _x = _offsetX;
                                        _y = _offsetY;
                                        _alignment = 0;
                                        _spacing = _offsetSpacing;
                                    }

                                    _offsetY = _y;
                                    _offsetSpacing = _spacing;
                                    if (activity.Type == "Distance")
                                    {
                                        Draw_dagital_text(gPanel, _imageIndex, _x, _y, _spacing, _alignment, 
                                            value, _addZero, value_lenght, _imageUnitIndex, _imageDecimalIndex, 2, BBorder);
                                    }
                                    else if (activity.Type == "Weather")
                                    {
                                        //TODO проверить зависимость выравнивания температуры от наличия иконки для виджетов
                                        Draw_weather_text(gPanel, _imageIndex, _x, _y,
                                        _spacing, _alignment, (int)value, _addZero, _imageDecimalIndex, _imageUnitIndex,
                                        BBorder, false);
                                    }
                                    else
                                    {
                                        _offsetX = Draw_dagital_text(gPanel, _imageIndex, _x, _y, _spacing, _alignment, 
                                            (int)value, _addZero, value_lenght, _imageUnitIndex, BBorder, ActivityType);
                                    }

                                    if (_iconIndex >= 0)
                                    {
                                        src = OpenFileStream(ListImagesFullName[_iconIndex]);
                                        gPanel.DrawImage(src, new Rectangle(_iconX, _iconY, src.Width, src.Height));
                                    }
                                }
                            }

                            // надпись системным шрифтом
                            if (digitalCommonDigit.Digit != null && digitalCommonDigit.Digit.SystemFont != null &&
                                    digitalCommonDigit.Digit.SystemFont.FontRotate == null)
                            {
                                int _x = 0;
                                int _y = 0;
                                int _size = 0;
                                int _angle = 0;
                                int _unitCheck = 0;
                                bool _follow = false;
                                bool _addZero = false;
                                bool _separator = false;
                                int _spacing = 0;
                                Color _color = Color.Black;

                                if (digitalCommonDigit.Digit.SystemFont.Coordinates != null)
                                {
                                    _x = (int)digitalCommonDigit.Digit.SystemFont.Coordinates.X;
                                    _y = (int)digitalCommonDigit.Digit.SystemFont.Coordinates.Y;
                                }

                                if (digitalCommonDigit.CombingMode == null ||
                                    digitalCommonDigit.CombingMode == "Follow") _follow = true;
                                if (digitalCommonDigit.Separator != null) _separator = true;
                                _size = (int)digitalCommonDigit.Digit.SystemFont.Size;
                                _angle = (int)digitalCommonDigit.Digit.SystemFont.Angle;
                                if (digitalCommonDigit.Digit.Spacing != null)
                                {
                                    _spacing = (int)digitalCommonDigit.Digit.Spacing;
                                }
                                _color = ColorRead(digitalCommonDigit.Digit.SystemFont.Color);
                                _unitCheck = (int)digitalCommonDigit.Digit.SystemFont.ShowUnitCheck;
                                _addZero = digitalCommonDigit.Digit.PaddingZero;

                                if (activity.Type == "Battery") _unitCheck = 1;
                                string sValue = value.ToString();
                                if (activity.Type == "Distance")
                                {
                                    string decimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                                    if (sValue.IndexOf(decimalSeparator) < 0) sValue = sValue + decimalSeparator;
                                    while (sValue.IndexOf(decimalSeparator) > sValue.Length - 3)
                                    {
                                        sValue = sValue + "0";
                                    }
                                }
                                if (_addZero)
                                {
                                    while (sValue.Length < value_lenght)
                                    {
                                        sValue = "0" + sValue;
                                    }
                                }
                                sValue = sValue + UnitName(activity.Type, _unitCheck);
                                if (_separator) sValue = sValue + "/";
                                if (_follow && _offsetXSF > -1)
                                {
                                    _x = _offsetXSF;
                                    _y = _offsetYSF;
                                    _spacing = _spacingSF;
                                    _angle = _angleSF;
                                    _size = _sizeSF;
                                    _color = _colorSF;

                                    sValueOldSF = sValueOldSF + sValue;
                                    _offsetXSF = Draw_text(gPanel, _x, _y, _size, _spacing, _color, _angle, sValue, BBorder);
                                }
                                else
                                {
                                    sValueOldSF = sValueOldSF + sValue;
                                    _offsetXSF = Draw_text(gPanel, _x, _y, _size, _spacing, _color, _angle, sValueOldSF, BBorder);

                                    _offsetYSF = _y;
                                    _spacingSF = _spacing;
                                    _angleSF = _angle;
                                    _sizeSF = _size;
                                    _colorSF = _color;
                                }

                            }

                            // надпись системным шрифтом по окружности
                            if (digitalCommonDigit.Digit != null && digitalCommonDigit.Digit.SystemFont != null &&
                                    digitalCommonDigit.Digit.SystemFont.FontRotate != null)
                            {
                                int _x = 0;
                                int _y = 0;
                                int _size = 0;
                                float _angle = 0;
                                int _radius = 0;
                                int _unitCheck = 0;
                                bool _follow = false;
                                bool _addZero = false;
                                bool _separator = false;
                                int _spacing = 0;
                                int _rotate_direction = 0;
                                Color _color = Color.Black;

                                if (digitalCommonDigit.CombingMode == null ||
                                    digitalCommonDigit.CombingMode == "Follow") _follow = true;
                                if (digitalCommonDigit.Separator != null) _separator = true;

                                if (digitalCommonDigit.Digit.SystemFont.FontRotate != null)
                                {
                                    _x = (int)digitalCommonDigit.Digit.SystemFont.FontRotate.X;
                                    _y = (int)digitalCommonDigit.Digit.SystemFont.FontRotate.Y;
                                    _radius = (int)digitalCommonDigit.Digit.SystemFont.FontRotate.Radius;
                                    _rotate_direction = (int)digitalCommonDigit.Digit.SystemFont.FontRotate.RotateDirection;
                                }
                                _size = (int)digitalCommonDigit.Digit.SystemFont.Size;
                                _angle = (float)digitalCommonDigit.Digit.SystemFont.Angle;
                                if (digitalCommonDigit.Digit.Spacing != null)
                                {
                                    _spacing = (int)digitalCommonDigit.Digit.Spacing;
                                }
                                _color = ColorRead(digitalCommonDigit.Digit.SystemFont.Color);
                                _unitCheck = (int)digitalCommonDigit.Digit.SystemFont.ShowUnitCheck;
                                _addZero = digitalCommonDigit.Digit.PaddingZero;

                                string sValue = value.ToString();
                                if (activity.Type == "Distance")
                                {
                                    string decimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                                    if (sValue.IndexOf(decimalSeparator) < 0) sValue = sValue + decimalSeparator;
                                    while (sValue.IndexOf(decimalSeparator) > sValue.Length - 3)
                                    {
                                        sValue = sValue + "0";
                                    }
                                }
                                if (_addZero)
                                {
                                    while (sValue.Length < value_lenght)
                                    {
                                        sValue = "0" + sValue;
                                    }
                                }
                                if (activity.Type == "Battery") _unitCheck = 1;
                                sValue = sValue + UnitName(activity.Type, _unitCheck);
                                if (_separator) sValue = sValue + "/";
                                if (_follow && _offsetXFR > -1)
                                {
                                    _x = _offsetXFR;
                                    _y = _offsetYFR;
                                    _spacing = _spacingFR;
                                    _angle = _angleFR;
                                    _size = _sizeFR;
                                    _radius = _radiusFR;
                                    _rotate_direction = _rotate_directionFR;
                                    _color = _colorFR;

                                    sValueOldFR = sValueOldFR + sValue;
                                    _angleFR = Draw_text_rotate(gPanel, _x, _y, _radius, _size, _spacing,
                                        _color, _angle, _rotate_direction, sValue, BBorder);
                                }
                                else
                                {
                                    sValueOldFR = sValueOldFR + sValue;
                                    _angleFR = Draw_text_rotate(gPanel, _x, _y, _radius, _size, _spacing,
                                        _color, _angle, _rotate_direction, sValueOldFR, BBorder);

                                    _offsetXFR = _x;
                                    _offsetYFR = _y;
                                    _spacingFR = _spacing;
                                    _angleFR = _angle;
                                    _sizeFR = _size;
                                    _radiusFR = _radius;
                                    _rotate_directionFR = _rotate_direction;
                                    _colorFR = _color;
                                }
                            }
                        } 
                    }

                    // стрелочный указатель
                    if (activity.PointerProgress != null && activity.PointerProgress.Pointer != null)
                    {
                        int _imageIndex = -1;
                        int _centrImageIndex = -1;
                        int _backgroundImageIndex = -1;
                        int _x = (int)activity.PointerProgress.X;
                        int _y = (int)activity.PointerProgress.Y;
                        int _startAngle = 0;
                        int _endAngle = 0;
                        int _offsetX = 0;
                        int _offsetY = 0;
                        int _X_centr = 0;
                        int _Y_centr = 0;
                        int _X_background = 0;
                        int _Y_background = 0;

                        _imageIndex = (int)activity.PointerProgress.Pointer.ImageIndex - 1;

                        _startAngle = (int)activity.PointerProgress.StartAngle;
                        _endAngle = (int)activity.PointerProgress.EndAngle;

                        _offsetX = (int)activity.PointerProgress.Pointer.Coordinates.X;
                        _offsetY = (int)activity.PointerProgress.Pointer.Coordinates.Y;

                        if (activity.PointerProgress.Cover != null)
                        {
                            _centrImageIndex = (int)activity.PointerProgress.Cover.ImageIndex - 1;
                            _X_centr = (int)activity.PointerProgress.Cover.Coordinates.X;
                            _Y_centr = (int)activity.PointerProgress.Cover.Coordinates.Y;
                        }

                        if (activity.PointerProgress.Scale != null && activity.PointerProgress.Scale.ImageSet != null)
                        {
                            foreach (MultilangImage multilangImage in activity.PointerProgress.Scale.ImageSet)
                            {
                                if (multilangImage.LangCode == "All")
                                    _backgroundImageIndex = (int)multilangImage.ImageSet.ImageIndex - 1;
                            }
                            _X_background = (int)activity.PointerProgress.Scale.Coordinates.X;
                            _Y_background = (int)activity.PointerProgress.Scale.Coordinates.Y;
                        }

                        if (_imageIndex >= 0)
                        {
                            if (_backgroundImageIndex >= 0)
                            {
                                src = OpenFileStream(ListImagesFullName[_backgroundImageIndex]);
                                gPanel.DrawImage(src, new Rectangle(_X_background, _Y_background, src.Width, src.Height));
                            }

                            float angle = _startAngle + progress * (_endAngle - _startAngle);
                            //if (Watch_Face_Preview_Set.Activity.Steps > Watch_Face_Preview_Set.Activity.StepsGoal) angle = endAngle;
                            DrawAnalogClock(gPanel, _x, _y, _offsetX, _offsetY, _imageIndex, angle, showCentrHend);

                            if (_centrImageIndex >= 0)
                            {
                                src = OpenFileStream(ListImagesFullName[_centrImageIndex]);
                                gPanel.DrawImage(src, new Rectangle(_X_centr, _Y_centr, src.Width, src.Height));
                            }
                        }
                    }
                }
            }
            // дата
            else if(widgetElement.Date != null)
            {
                // надпись или картинка месяца
                if (widgetElement.Date.DateDigits != null && widgetElement.Date.DateDigits.Count > 0)
                {
                    string sValueOldSF = "";
                    string sValueOldFR = "";
                    int _offsetX = -1;
                    int _offsetY = -1;
                    int _offsetSpacing = 0;

                    int _offsetXSF = -1;
                    int _offsetYSF = -1;
                    int _spacingSF = 0;
                    int _sizeSF = 0;
                    int _angleSF = 0;
                    Color _colorSF = Color.Black;

                    int _offsetXFR = -1;
                    int _offsetYFR = -1;
                    int _spacingFR = 0;
                    int _sizeFR = 0;
                    int _radiusFR = 0;
                    float _angleFR = 0;
                    int _rotate_directionFR = 0;
                    Color _colorFR = Color.Black;

                    foreach (DigitalDateDigit digitalDateDigit in widgetElement.Date.DateDigits)
                    {
                        float activityValue = 0;
                        int value_lenght = 2;
                        switch (digitalDateDigit.DateType)
                        {
                            case "Day":
                                activityValue = Watch_Face_Preview_Set.Date.Day;
                                break;

                            case "Month":
                                activityValue = Watch_Face_Preview_Set.Date.Month;
                                break;
                            default:
                                activityValue = Watch_Face_Preview_Set.Date.Year;
                                value_lenght = 4;
                                break;
                        }

                        // набор картинок
                        if (digitalDateDigit.DateType == "Month" && 
                            digitalDateDigit.Digit != null && digitalDateDigit.Digit.DisplayFormAnalog)
                        {
                            if (digitalDateDigit.Digit != null && digitalDateDigit.Digit.Image != null)
                            {
                                int _x = (int)digitalDateDigit.Digit.Image.X;
                                int _y = (int)digitalDateDigit.Digit.Image.Y;
                                int _imageIndex = - 1;
                                foreach (MultilangImage multilangImage in digitalDateDigit.Digit.Image.MultilangImage)
                                {
                                    if (multilangImage.LangCode == "All")
                                        _imageIndex = (int)(multilangImage.ImageSet.ImageIndex-1);
                                }
                                _imageIndex = _imageIndex + Watch_Face_Preview_Set.Date.Month - 1;

                                if (_imageIndex>=0 && _imageIndex < ListImagesFullName.Count)
                                {
                                    src = OpenFileStream(ListImagesFullName[_imageIndex]);
                                    gPanel.DrawImage(src, new Rectangle(_x, _y, src.Width, src.Height));
                                }
                            }
                        }

                        // надпись
                        else
                        {
                            float value = activityValue;
                            // надпись
                            if (digitalDateDigit.Digit != null && digitalDateDigit.Digit.Image != null)
                            {
                                int _imageIndex = -1;
                                int _imageNoDataIndex = -1;
                                int _imageUnitIndex = -1;
                                int _iconIndex = -1;
                                int _x = (int)digitalDateDigit.Digit.Image.X;
                                int _y = (int)digitalDateDigit.Digit.Image.Y;
                                int _iconX = 0;
                                int _iconY = 0;
                                bool _follow = false;
                                bool _addZero = false;
                                int _alignment = 0;
                                int _spacing = 0;
                                if (digitalDateDigit.CombingMode == null ||
                                    digitalDateDigit.CombingMode == "Follow") _follow = true;

                                if (digitalDateDigit.Digit.Image.NoDataImageIndex != null)
                                    _imageNoDataIndex = (int)digitalDateDigit.Digit.Image.NoDataImageIndex - 1;

                                foreach (MultilangImage multilangImage in digitalDateDigit.Digit.Image.MultilangImage)
                                {
                                    if (multilangImage.LangCode == "All")
                                        _imageIndex = (int)multilangImage.ImageSet.ImageIndex - 1;
                                }
                                if (digitalDateDigit.Digit.Image.MultilangImageUnit != null)
                                {
                                    foreach (MultilangImage multilangImage in digitalDateDigit.Digit.Image.MultilangImageUnit)
                                    {
                                        if (multilangImage.LangCode == "All")
                                            _imageUnitIndex = (int)multilangImage.ImageSet.ImageIndex - 1;
                                    }
                                }

                                switch (digitalDateDigit.Digit.Alignment)
                                {
                                    case "Left":
                                        _alignment = 0;
                                        break;
                                    case "Right":
                                        _alignment = 1;
                                        break;
                                    case "Center":
                                        _alignment = 2;
                                        break;
                                }
                                if (digitalDateDigit.Digit.Spacing != null)
                                    _spacing = (int)digitalDateDigit.Digit.Spacing;
                                _addZero = digitalDateDigit.Digit.PaddingZero;
                                if (digitalDateDigit.Separator != null)
                                {
                                    _iconIndex = (int)digitalDateDigit.Separator.ImageIndex;
                                    _iconX = (int)digitalDateDigit.Separator.Coordinates.X;
                                    _iconY = (int)digitalDateDigit.Separator.Coordinates.Y;
                                }


                                if (_imageIndex >= 0)
                                {
                                    if ((_addZero && digitalDateDigit.DateType == null) ||
                                        (_addZero && digitalDateDigit.DateType != "Month" && digitalDateDigit.DateType != "Day"))
                                    {
                                        value = Watch_Face_Preview_Set.Date.Year % 100;
                                        _addZero = false;
                                    }
                                    if (_follow && _offsetX > -1)
                                    {
                                        _x = _offsetX;
                                        _y = _offsetY;
                                        _alignment = 0;
                                        _spacing = _offsetSpacing;
                                    }

                                    _offsetY = _y;
                                    _offsetSpacing = _spacing;
                                    _offsetX = Draw_dagital_text(gPanel, _imageIndex, _x, _y,
                                            _spacing, _alignment, (int)value, _addZero, value_lenght, _imageUnitIndex, BBorder);

                                    if (_iconIndex >= 0)
                                    {
                                        src = OpenFileStream(ListImagesFullName[_iconIndex]);
                                        gPanel.DrawImage(src, new Rectangle(_iconX, _iconY, src.Width, src.Height));
                                    }
                                }
                            }

                            // надпись системным шрифтом
                            if (digitalDateDigit.Digit != null && digitalDateDigit.Digit.SystemFont != null &&
                                    digitalDateDigit.Digit.SystemFont.FontRotate == null)
                            {
                                int _x = 0;
                                int _y = 0;
                                int _size = 0;
                                int _angle = 0;
                                int _unitCheck = 0;
                                bool _follow = false;
                                bool _addZero = false;
                                bool _separator = false;
                                int _spacing = 0;
                                Color _color = Color.Black;

                                if (digitalDateDigit.Digit.SystemFont.Coordinates != null)
                                {
                                    _x = (int)digitalDateDigit.Digit.SystemFont.Coordinates.X;
                                    _y = (int)digitalDateDigit.Digit.SystemFont.Coordinates.Y;
                                }

                                if (digitalDateDigit.CombingMode == null ||
                                    digitalDateDigit.CombingMode == "Follow") _follow = true;
                                if (digitalDateDigit.Separator != null) _separator = true;
                                _size = (int)digitalDateDigit.Digit.SystemFont.Size;
                                _angle = (int)digitalDateDigit.Digit.SystemFont.Angle;
                                if (digitalDateDigit.Digit.Spacing != null)
                                {
                                    _spacing = (int)digitalDateDigit.Digit.Spacing;
                                }
                                _color = ColorRead(digitalDateDigit.Digit.SystemFont.Color);
                                _unitCheck = (int)digitalDateDigit.Digit.SystemFont.ShowUnitCheck;
                                _addZero = digitalDateDigit.Digit.PaddingZero;
                                if ((_addZero && digitalDateDigit.DateType == null) ||
                                        (_addZero && digitalDateDigit.DateType != "Month" && digitalDateDigit.DateType != "Day"))
                                {
                                    value = Watch_Face_Preview_Set.Date.Year % 100;
                                    _addZero = false;
                                }

                                string sValue = value.ToString();
                                if (_addZero)
                                {
                                    while (sValue.Length < value_lenght)
                                    {
                                        sValue = "0" + sValue;
                                    }
                                }
                                sValue = sValue + UnitName("Date", _unitCheck);
                                if (_separator) sValue = sValue + "/";
                                if (_follow && _offsetXSF > -1)
                                {
                                    _x = _offsetXSF;
                                    _y = _offsetYSF;
                                    _spacing = _spacingSF;
                                    _angle = _angleSF;
                                    _size = _sizeSF;
                                    _color = _colorSF;

                                    sValueOldSF = sValueOldSF + sValue;
                                    _offsetXSF = Draw_text(gPanel, _x, _y, _size, _spacing, _color, _angle, sValue, BBorder);
                                }
                                else
                                {
                                    sValueOldSF = sValueOldSF + sValue;
                                    _offsetXSF = Draw_text(gPanel, _x, _y, _size, _spacing, _color, _angle, sValueOldSF, BBorder);

                                    _offsetYSF = _y;
                                    _spacingSF = _spacing;
                                    _angleSF = _angle;
                                    _sizeSF = _size;
                                    _colorSF = _color;
                                }

                            }

                            // надпись системным шрифтом по окружности
                            if (digitalDateDigit.Digit != null && digitalDateDigit.Digit.SystemFont != null &&
                                    digitalDateDigit.Digit.SystemFont.FontRotate != null)
                            {
                                int _x = 0;
                                int _y = 0;
                                int _size = 0;
                                float _angle = 0;
                                int _radius = 0;
                                int _unitCheck = 0;
                                bool _follow = false;
                                bool _addZero = false;
                                bool _separator = false;
                                int _spacing = 0;
                                int _rotate_direction = 0;
                                Color _color = Color.Black;

                                if (digitalDateDigit.CombingMode == null ||
                                    digitalDateDigit.CombingMode == "Follow") _follow = true;
                                if (digitalDateDigit.Separator != null) _separator = true;

                                if (digitalDateDigit.Digit.SystemFont.FontRotate != null)
                                {
                                    _x = (int)digitalDateDigit.Digit.SystemFont.FontRotate.X;
                                    _y = (int)digitalDateDigit.Digit.SystemFont.FontRotate.Y;
                                    _radius = (int)digitalDateDigit.Digit.SystemFont.FontRotate.Radius;
                                    _rotate_direction = (int)digitalDateDigit.Digit.SystemFont.FontRotate.RotateDirection;
                                }
                                _size = (int)digitalDateDigit.Digit.SystemFont.Size;
                                _angle = (float)digitalDateDigit.Digit.SystemFont.Angle;
                                if (digitalDateDigit.Digit.Spacing != null)
                                {
                                    _spacing = (int)digitalDateDigit.Digit.Spacing;
                                }
                                _color = ColorRead(digitalDateDigit.Digit.SystemFont.Color);
                                _unitCheck = (int)digitalDateDigit.Digit.SystemFont.ShowUnitCheck;
                                _addZero = digitalDateDigit.Digit.PaddingZero;
                                if ((_addZero && digitalDateDigit.DateType == null) ||
                                        (_addZero && digitalDateDigit.DateType != "Month" && digitalDateDigit.DateType != "Day"))
                                {
                                    value = Watch_Face_Preview_Set.Date.Year % 100;
                                    _addZero = false;
                                }

                                string sValue = value.ToString();
                                if (_addZero)
                                {
                                    while (sValue.Length < value_lenght)
                                    {
                                        sValue = "0" + sValue;
                                    }
                                }
                                sValue = sValue + UnitName("Date", _unitCheck);
                                if (_separator) sValue = sValue + "/";
                                if (_follow && _offsetXFR > -1)
                                {
                                    _x = _offsetXFR;
                                    _y = _offsetYFR;
                                    _spacing = _spacingFR;
                                    _angle = _angleFR;
                                    _size = _sizeFR;
                                    _radius = _radiusFR;
                                    _rotate_direction = _rotate_directionFR;
                                    _color = _colorFR;

                                    sValueOldFR = sValueOldFR + sValue;
                                    _angleFR = Draw_text_rotate(gPanel, _x, _y, _radius, _size, _spacing,
                                        _color, _angle, _rotate_direction, sValue, BBorder);
                                }
                                else
                                {
                                    sValueOldFR = sValueOldFR + sValue;
                                    _angleFR = Draw_text_rotate(gPanel, _x, _y, _radius, _size, _spacing,
                                        _color, _angle, _rotate_direction, sValueOldFR, BBorder);

                                    _offsetXFR = _x;
                                    _offsetYFR = _y;
                                    _spacingFR = _spacing;
                                    _angleFR = _angle;
                                    _sizeFR = _size;
                                    _radiusFR = _radius;
                                    _rotate_directionFR = _rotate_direction;
                                    _colorFR = _color;
                                }
                            }
                        }

                    }
                }

                // дата стрелкой
                if (widgetElement.Date.DateClockHand != null && widgetElement.Date.DateClockHand.DayClockHand != null)
                {
                    int _imageIndex = -1;
                    int _centrImageIndex = -1;
                    int _backgroundImageIndex = -1;
                    int _x = -1;
                    int _y = -1;
                    int _startAngle = 0;
                    int _endAngle = 0;
                    int _offsetX = 0;
                    int _offsetY = 0;
                    int _X_centr = 0;
                    int _Y_centr = 0;
                    int _X_background = 0;
                    int _Y_background = 0;


                    _x = (int)widgetElement.Date.DateClockHand.DayClockHand.X;
                    _y = (int)widgetElement.Date.DateClockHand.DayClockHand.Y;
                    _startAngle = (int)widgetElement.Date.DateClockHand.DayClockHand.StartAngle;
                    _endAngle = (int)widgetElement.Date.DateClockHand.DayClockHand.EndAngle;
                    if (widgetElement.Date.DateClockHand.DayClockHand.Pointer != null)
                    {
                        _imageIndex = (int)widgetElement.Date.DateClockHand.DayClockHand.Pointer.ImageIndex - 1;
                        if (widgetElement.Date.DateClockHand.DayClockHand.Pointer.Coordinates != null)
                        {
                            _offsetX = (int)widgetElement.Date.DateClockHand.DayClockHand.Pointer.Coordinates.X;
                            _offsetY = (int)widgetElement.Date.DateClockHand.DayClockHand.Pointer.Coordinates.Y;
                        }
                    }

                    // центральное изображение
                    if (widgetElement.Date.DateClockHand.DayClockHand.Cover != null)
                    {
                        _centrImageIndex = (int)widgetElement.Date.DateClockHand.DayClockHand.Cover.ImageIndex - 1;
                        if (widgetElement.Date.DateClockHand.DayClockHand.Pointer.Coordinates != null)
                        {
                            _X_centr = (int)widgetElement.Date.DateClockHand.DayClockHand.Cover.Coordinates.X;
                            _Y_centr = (int)widgetElement.Date.DateClockHand.DayClockHand.Cover.Coordinates.Y;
                        }
                    }

                    // фон
                    if (widgetElement.Date.DateClockHand.DayClockHand.Scale != null &&
                        widgetElement.Date.DateClockHand.DayClockHand.Scale.ImageSet != null)
                    {
                        foreach (MultilangImage multilangImage in widgetElement.Date.DateClockHand.DayClockHand.Scale.ImageSet)
                        {
                            if (multilangImage.LangCode == "All")
                                _backgroundImageIndex = (int)multilangImage.ImageSet.ImageIndex - 1;
                        }
                        if (widgetElement.Date.DateClockHand.DayClockHand.Scale.Coordinates != null)
                        {
                            _X_background = (int)widgetElement.Date.DateClockHand.DayClockHand.Scale.Coordinates.X;
                            _Y_background = (int)widgetElement.Date.DateClockHand.DayClockHand.Scale.Coordinates.Y;
                        }
                    }

                    if (_backgroundImageIndex >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[_backgroundImageIndex]);
                        gPanel.DrawImage(src, new Rectangle(_X_background, _Y_background, src.Width, src.Height));
                    }
                    int Day = Watch_Face_Preview_Set.Date.Day;
                    Day--;
                    float angle = _startAngle + Day * (_endAngle - _startAngle) / 30;
                    DrawAnalogClock(gPanel, _x, _y, _offsetX, _offsetY, _imageIndex, angle, showCentrHend);

                    if (_centrImageIndex >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[_centrImageIndex]);
                        gPanel.DrawImage(src, new Rectangle(_X_centr, _Y_centr, src.Width, src.Height));
                    }
                }

                // месяц стрелкой
                if (widgetElement.Date.DateClockHand != null && widgetElement.Date.DateClockHand.MonthClockHand != null)
                {
                    int _imageIndex = -1;
                    int _centrImageIndex = -1;
                    int _backgroundImageIndex = -1;
                    int _x = -1;
                    int _y = -1;
                    int _startAngle = 0;
                    int _endAngle = 0;
                    int _offsetX = 0;
                    int _offsetY = 0;
                    int _X_centr = 0;
                    int _Y_centr = 0;
                    int _X_background = 0;
                    int _Y_background = 0;


                    _x = (int)widgetElement.Date.DateClockHand.MonthClockHand.X;
                    _y = (int)widgetElement.Date.DateClockHand.MonthClockHand.Y;
                    _startAngle = (int)widgetElement.Date.DateClockHand.MonthClockHand.StartAngle;
                    _endAngle = (int)widgetElement.Date.DateClockHand.MonthClockHand.EndAngle;
                    if (widgetElement.Date.DateClockHand.MonthClockHand.Pointer != null)
                    {
                        _imageIndex = (int)widgetElement.Date.DateClockHand.MonthClockHand.Pointer.ImageIndex - 1;
                        if (widgetElement.Date.DateClockHand.MonthClockHand.Pointer.Coordinates != null)
                        {
                            _offsetX = (int)widgetElement.Date.DateClockHand.MonthClockHand.Pointer.Coordinates.X;
                            _offsetY = (int)widgetElement.Date.DateClockHand.MonthClockHand.Pointer.Coordinates.Y;
                        }
                    }

                    // центральное изображение
                    if (widgetElement.Date.DateClockHand.MonthClockHand.Cover != null)
                    {
                        _centrImageIndex = (int)widgetElement.Date.DateClockHand.MonthClockHand.Cover.ImageIndex - 1;
                        if (widgetElement.Date.DateClockHand.MonthClockHand.Pointer.Coordinates != null)
                        {
                            _X_centr = (int)widgetElement.Date.DateClockHand.MonthClockHand.Cover.Coordinates.X;
                            _Y_centr = (int)widgetElement.Date.DateClockHand.MonthClockHand.Cover.Coordinates.Y;
                        }
                    }

                    // фон
                    if (widgetElement.Date.DateClockHand.MonthClockHand.Scale != null &&
                        widgetElement.Date.DateClockHand.MonthClockHand.Scale.ImageSet != null)
                    {
                        foreach (MultilangImage multilangImage in widgetElement.Date.DateClockHand.MonthClockHand.Scale.ImageSet)
                        {
                            if (multilangImage.LangCode == "All")
                                _backgroundImageIndex = (int)multilangImage.ImageSet.ImageIndex - 1;
                        }
                        if (widgetElement.Date.DateClockHand.MonthClockHand.Scale.Coordinates != null)
                        {
                            _X_background = (int)widgetElement.Date.DateClockHand.MonthClockHand.Scale.Coordinates.X;
                            _Y_background = (int)widgetElement.Date.DateClockHand.MonthClockHand.Scale.Coordinates.Y;
                        }
                    }

                    if (_backgroundImageIndex >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[_backgroundImageIndex]);
                        gPanel.DrawImage(src, new Rectangle(_X_background, _Y_background, src.Width, src.Height));
                    }
                    int Month = Watch_Face_Preview_Set.Date.Month;
                    Month--;
                    float angle = _startAngle + Month * (_endAngle - _startAngle) / 11;
                    DrawAnalogClock(gPanel, _x, _y, _offsetX, _offsetY, _imageIndex, angle, showCentrHend);

                    if (_centrImageIndex >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[_centrImageIndex]);
                        gPanel.DrawImage(src, new Rectangle(_X_centr, _Y_centr, src.Width, src.Height));
                    }
                }

                // день недели стрелкой
                if (widgetElement.Date.DateClockHand != null && widgetElement.Date.DateClockHand.WeekDayClockHand != null)
                {
                    int _imageIndex = -1;
                    int _centrImageIndex = -1;
                    int _backgroundImageIndex = -1;
                    int _x = -1;
                    int _y = -1;
                    int _startAngle = 0;
                    int _endAngle = 0;
                    int _offsetX = 0;
                    int _offsetY = 0;
                    int _X_centr = 0;
                    int _Y_centr = 0;
                    int _X_background = 0;
                    int _Y_background = 0;


                    _x = (int)widgetElement.Date.DateClockHand.WeekDayClockHand.X;
                    _y = (int)widgetElement.Date.DateClockHand.WeekDayClockHand.Y;
                    _startAngle = (int)widgetElement.Date.DateClockHand.WeekDayClockHand.StartAngle;
                    _endAngle = (int)widgetElement.Date.DateClockHand.WeekDayClockHand.EndAngle;
                    if (widgetElement.Date.DateClockHand.WeekDayClockHand.Pointer != null)
                    {
                        _imageIndex = (int)widgetElement.Date.DateClockHand.WeekDayClockHand.Pointer.ImageIndex - 1;
                        if (widgetElement.Date.DateClockHand.WeekDayClockHand.Pointer.Coordinates != null)
                        {
                            _offsetX = (int)widgetElement.Date.DateClockHand.WeekDayClockHand.Pointer.Coordinates.X;
                            _offsetY = (int)widgetElement.Date.DateClockHand.WeekDayClockHand.Pointer.Coordinates.Y;
                        }
                    }

                    // центральное изображение
                    if (widgetElement.Date.DateClockHand.WeekDayClockHand.Cover != null)
                    {
                        _centrImageIndex = (int)widgetElement.Date.DateClockHand.WeekDayClockHand.Cover.ImageIndex - 1;
                        if (widgetElement.Date.DateClockHand.WeekDayClockHand.Pointer.Coordinates != null)
                        {
                            _X_centr = (int)widgetElement.Date.DateClockHand.WeekDayClockHand.Cover.Coordinates.X;
                            _Y_centr = (int)widgetElement.Date.DateClockHand.WeekDayClockHand.Cover.Coordinates.Y;
                        }
                    }

                    // фон
                    if (widgetElement.Date.DateClockHand.WeekDayClockHand.Scale != null &&
                        widgetElement.Date.DateClockHand.WeekDayClockHand.Scale.ImageSet != null)
                    {
                        foreach (MultilangImage multilangImage in widgetElement.Date.DateClockHand.WeekDayClockHand.Scale.ImageSet)
                        {
                            if (multilangImage.LangCode == "All")
                                _backgroundImageIndex = (int)multilangImage.ImageSet.ImageIndex - 1;
                        }
                        if (widgetElement.Date.DateClockHand.WeekDayClockHand.Scale.Coordinates != null)
                        {
                            _X_background = (int)widgetElement.Date.DateClockHand.WeekDayClockHand.Scale.Coordinates.X;
                            _Y_background = (int)widgetElement.Date.DateClockHand.WeekDayClockHand.Scale.Coordinates.Y;
                        }
                    }

                    if (_backgroundImageIndex >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[_backgroundImageIndex]);
                        gPanel.DrawImage(src, new Rectangle(_X_background, _Y_background, src.Width, src.Height));
                    }
                    int WeekDay = Watch_Face_Preview_Set.Date.WeekDay;
                    WeekDay--;
                    if (WeekDay < 0) WeekDay = 6;
                    float angle = _startAngle + WeekDay * (_endAngle - _startAngle) / 6;
                    DrawAnalogClock(gPanel, _x, _y, _offsetX, _offsetY, _imageIndex, angle, showCentrHend);

                    if (_centrImageIndex >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[_centrImageIndex]);
                        gPanel.DrawImage(src, new Rectangle(_X_centr, _Y_centr, src.Width, src.Height));
                    }
                }

                // день недели картинкой
                if (widgetElement.Date.WeeksDigits != null)
                {
                    if (widgetElement.Date.WeeksDigits.Digit != null)
                    {
                        if (widgetElement.Date.WeeksDigits.Digit.Image != null)
                        {
                            int _x = (int)widgetElement.Date.WeeksDigits.Digit.Image.X;
                            int _y = (int)widgetElement.Date.WeeksDigits.Digit.Image.Y;
                            int _imageIndex = -1;
                            foreach (MultilangImage multilangImage in widgetElement.Date.WeeksDigits.Digit.Image.MultilangImage)
                            {
                                if (multilangImage.LangCode == "All")
                                    _imageIndex = (int)(multilangImage.ImageSet.ImageIndex-1);
                            }
                        }
                    }
                }
            }

            src.Dispose();

            return;
        }

        private void dataGridView_WidgetElement_EnabledChanged(object sender, EventArgs e)
        {
            if (dataGridView_WidgetElement.Enabled)
            {
                dataGridView_WidgetElement.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                dataGridView_WidgetElement.RowsDefaultCellStyle.ForeColor = Color.Black;
                //dataGridView_WidgetElement.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(0,0,0,0);
                dataGridView_WidgetElement.RowsDefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
                dataGridView_WidgetElement.RowsDefaultCellStyle.SelectionForeColor = Color.White;
            }
            else
            {
                dataGridView_WidgetElement.ColumnHeadersDefaultCellStyle.ForeColor = Color.DarkGray;
                dataGridView_WidgetElement.RowsDefaultCellStyle.ForeColor = Color.DarkGray;
                dataGridView_WidgetElement.RowsDefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
                dataGridView_WidgetElement.RowsDefaultCellStyle.SelectionForeColor = Color.DarkGray;
            }
        }

        /// <summary>заполняем таблицу с элементами для выбранно редактируемой зоны</summary>
        private void JSON_read_widgetElement_order(Widget widget)
        {
            dataGridView_WidgetElement.Rows.Clear();
            if (widget != null && widget.WidgetElement != null)
            {
                foreach (WidgetElement widgetElement in widget.WidgetElement)
                {
                    if (widgetElement.Activity != null)
                    {
                        switch (widgetElement.Activity[0].Type)
                        {
                            case "Battery":
                                dataGridView_WidgetElement.Rows.Add("Battery", Properties.FormStrings.WidgetName_Battery);
                                break;
                            case "Steps":
                                dataGridView_WidgetElement.Rows.Add("Steps", Properties.FormStrings.WidgetName_Steps);
                                break;
                            case "Calories":
                                dataGridView_WidgetElement.Rows.Add("Calories", Properties.FormStrings.WidgetName_Calories);
                                break;
                            case "HeartRate":
                                dataGridView_WidgetElement.Rows.Add("HeartRate", Properties.FormStrings.WidgetName_HeartRate);
                                break;
                            case "PAI":
                                dataGridView_WidgetElement.Rows.Add("PAI", Properties.FormStrings.WidgetName_PAI);
                                break;
                            case "Distance":
                                dataGridView_WidgetElement.Rows.Add("Distance", Properties.FormStrings.WidgetName_Distance);
                                break;
                            case "StandUp":
                                dataGridView_WidgetElement.Rows.Add("StandUp", Properties.FormStrings.WidgetName_StandUp);
                                break;
                            case "Weather":
                                dataGridView_WidgetElement.Rows.Add("Weather", Properties.FormStrings.WidgetName_Weather);
                                break;
                            case "UVindex":
                                dataGridView_WidgetElement.Rows.Add("UVindex", Properties.FormStrings.WidgetName_UVindex);
                                break;
                            case "AirQuality":
                                dataGridView_WidgetElement.Rows.Add("AirQuality", Properties.FormStrings.WidgetName_AirQuality);
                                break;
                            case "Humidity":
                                dataGridView_WidgetElement.Rows.Add("Humidity", Properties.FormStrings.WidgetName_Humidity);
                                break;
                            case "Sunrise":
                                dataGridView_WidgetElement.Rows.Add("Sunrise", Properties.FormStrings.WidgetName_Sunrise);
                                break;
                            case "WindForce":
                                dataGridView_WidgetElement.Rows.Add("WindForce", Properties.FormStrings.WidgetName_WindForce);
                                break;
                            case "Altitude":
                                dataGridView_WidgetElement.Rows.Add("Altitude", Properties.FormStrings.WidgetName_Altitude);
                                break;
                            case "AirPressure":
                                dataGridView_WidgetElement.Rows.Add("AirPressure", Properties.FormStrings.WidgetName_AirPressure);
                                break;
                            case "Stress":
                                dataGridView_WidgetElement.Rows.Add("Stress", Properties.FormStrings.WidgetName_Stress);
                                break;
                            case "ActivityGoal":
                                dataGridView_WidgetElement.Rows.Add("ActivityGoal", Properties.FormStrings.WidgetName_ActivityGoal);
                                break;
                            case "FatBurning":
                                dataGridView_WidgetElement.Rows.Add("FatBurning", Properties.FormStrings.WidgetName_FatBurning);
                                break;
                        } 
                    }
                    if(widgetElement.Date != null) dataGridView_WidgetElement.Rows.Add("Date", Properties.FormStrings.WidgetName_Date);
                }
                dataGridView_WidgetElement.ClearSelection();
            }
        }

        private void dataGridView_WidgetElement_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView_WidgetElement.SelectedCells.Count > 0)
            {
                int RowIndex = dataGridView_WidgetElement.SelectedCells[0].RowIndex;

                button_WidgetElement_Start.Enabled = true;
                button_WidgetElement_Up.Enabled = true;
                button_WidgetElement_Down.Enabled = true;
                button_WidgetElement_End.Enabled = true;

                if (RowIndex == 0)
                {
                    button_WidgetElement_Start.Enabled = false;
                    button_WidgetElement_Up.Enabled = false;
                }

                if (RowIndex == dataGridView_WidgetElement.Rows.Count-1)
                {
                    button_WidgetElement_Down.Enabled = false;
                    button_WidgetElement_End.Enabled = false;
                }
                string name = dataGridView_WidgetElement.Rows[RowIndex].Cells[0].Value.ToString();
                int widgetIndex = comboBox_WidgetNumber.SelectedIndex;
                WidgetElement widgetElement = Watch_Face.Widgets.Widget[widgetIndex].WidgetElement[RowIndex];
                SelectWidgetElement(name, widgetElement);
            }
            else
            {
                button_WidgetElement_Start.Enabled = false;
                button_WidgetElement_Up.Enabled = false;
                button_WidgetElement_Down.Enabled = false;
                button_WidgetElement_End.Enabled = false;
                SelectWidgetElement("", null);
            }

            PreviewImage();
        }

        private void SelectWidgetElement(string name, WidgetElement widgetElement)
        {
            UserControl_preview userControl_preview = userControl_previewWidget;
            UserControl_pictures userPanel_pictures = null;
            UserControl_text userPanel_text = null;
            UserControl_text userPanel_textGoal = null;
            UserControl_hand userPanel_hand = null;
            UserControl_scaleCircle userPanel_scaleCircle = null;
            UserControl_scaleLinear userPanel_scaleLinear = null;
            UserControl_SystemFont_Group userControl_SystemFont_Group = null;
            UserControl_SystemFont userControl_SystemFont = null;
            UserControl_SystemFont userControl_SystemFontGoal = null;
            UserControl_FontRotate userControl_FontRotate = null;
            UserControl_FontRotate userControl_FontRotateGoal = null;
            UserControl_text userPanel_text_Activity = null;
            UserControl_SystemFont userControl_SystemFont_Activity = null;
            UserControl_FontRotate userControl_FontRotate_Activity = null;
            UserControl_icon userControl_icon = null;

            UserControl_text userPanel_text_weather_sunrise = null;

            userControl_previewWidget.Visible = false;
            userControl_picturesWidget.Visible = false;
            userControl_pictures_weatherWidget.Visible = false;
            userControl_textWidget.Visible = false;
            userControl_text_goalWidget.Visible = false;
            userControl_text_weatherWidgetCur.Visible = false;
            userControl_text_weatherWidgetMin.Visible = false;
            userControl_text_weatherWidgetMax.Visible = false;
            userControl_text_goalWidgetSunrise.Visible = false;
            userControl_text_goalWidgetSunset.Visible = false;
            userControl_handWidget.Visible = false;
            userControl_scaleCircleWidget.Visible = false;
            userControl_scaleLinearWidget.Visible = false;
            userControl_SystemFont_GroupWidget.Visible = false;
            userControl_SystemFont_GroupWeatherWidget.Visible = false;
            userControl_SystemFont_GroupSunriseWidget.Visible = false;
            userControl_iconWidget.Visible = false;
            tabControl_DateWidget.Visible = false;
            if (widgetElement == null) return;

            userControl_textWidget.OptionalSymbol = false;
            userControl_text_goalWidget.Follow = true;
            userControl_iconWidget.Image2 = false;
            switch (name)
            {
                case "Battery":
                    userControl_previewWidget.Visible = true;
                    userControl_picturesWidget.Visible = true;
                    userControl_textWidget.Visible = true;
                    //userControl_text_goalWidget.Visible = true;
                    //userControl_text_weatherWidgetCur.Visible = true;
                    //userControl_text_weatherWidgetMin.Visible = true;
                    //userControl_text_weatherWidgetMax.Visible = true;
                    //userControl_text_goalWidgetSunrise.Visible = true;
                    //userControl_text_goalWidgetSunset.Visible = true;
                    userControl_handWidget.Visible = true;
                    userControl_scaleCircleWidget.Visible = true;
                    userControl_scaleLinearWidget.Visible = true;
                    userControl_SystemFont_GroupWidget.Visible = true;
                    //userControl_SystemFont_GroupWeatherWidget.Visible = true;
                    //userControl_SystemFont_GroupSunriseWidget.Visible = true;
                    userControl_iconWidget.Visible = true;
                    //tabControl_DateWidget.Visible = true;

                    userControl_SystemFont_GroupWidget.ShowGoal = false;


                    userPanel_pictures = userControl_picturesWidget;
                    userPanel_text = userControl_textWidget;
                    //userPanel_textGoal = userControl_text_goalWidget;
                    userPanel_hand = userControl_handWidget;
                    userPanel_scaleCircle = userControl_scaleCircleWidget;
                    userPanel_scaleLinear = userControl_scaleLinearWidget;
                    userControl_SystemFont_Group = userControl_SystemFont_GroupWidget;
                    userControl_icon = userControl_iconWidget;
                    break;

                case "Steps":
                    userControl_previewWidget.Visible = true;
                    userControl_picturesWidget.Visible = true;
                    userControl_textWidget.Visible = true;
                    userControl_text_goalWidget.Visible = true;
                    //userControl_text_weatherWidgetCur.Visible = true;
                    //userControl_text_weatherWidgetMin.Visible = true;
                    //userControl_text_weatherWidgetMax.Visible = true;
                    //userControl_text_goalWidgetSunrise.Visible = true;
                    //userControl_text_goalWidgetSunset.Visible = true;
                    userControl_handWidget.Visible = true;
                    userControl_scaleCircleWidget.Visible = true;
                    userControl_scaleLinearWidget.Visible = true;
                    userControl_SystemFont_GroupWidget.Visible = true;
                    //userControl_SystemFont_GroupWeatherWidget.Visible = true;
                    //userControl_SystemFont_GroupSunriseWidget.Visible = true;
                    userControl_iconWidget.Visible = true;
                    //tabControl_DateWidget.Visible = true;

                    userControl_SystemFont_GroupWidget.ShowGoal = true;
                    userControl_SystemFont_GroupWidget.FollowText = userControl_SystemFont_Group_Steps.FollowText;
                    userControl_SystemFont_GroupWidget.FollowRotateText = userControl_SystemFont_Group_Steps.FollowRotateText;
                    userControl_SystemFont_GroupWidget.SystemFontText = userControl_SystemFont_Group_Steps.SystemFontText;
                    userControl_SystemFont_GroupWidget.FontRotateText = userControl_SystemFont_Group_Steps.FontRotateText;

                    userControl_text_goalWidget.ButtonText = userControl_text_goal_Steps.ButtonText;
                    userControl_text_goalWidget.FollowText = userControl_text_goal_Steps.FollowText;

                    
                    userPanel_pictures = userControl_picturesWidget;
                    userPanel_text = userControl_textWidget;
                    userPanel_textGoal = userControl_text_goalWidget;
                    userPanel_hand = userControl_handWidget;
                    userPanel_scaleCircle = userControl_scaleCircleWidget;
                    userPanel_scaleLinear = userControl_scaleLinearWidget;
                    userControl_SystemFont_Group = userControl_SystemFont_GroupWidget;
                    userControl_icon = userControl_iconWidget;
                    break;

                case "Calories":
                    userControl_previewWidget.Visible = true;
                    userControl_picturesWidget.Visible = true;
                    userControl_textWidget.Visible = true;
                    userControl_text_goalWidget.Visible = true;
                    //userControl_text_weatherWidgetCur.Visible = true;
                    //userControl_text_weatherWidgetMin.Visible = true;
                    //userControl_text_weatherWidgetMax.Visible = true;
                    //userControl_text_goalWidgetSunrise.Visible = true;
                    //userControl_text_goalWidgetSunset.Visible = true;
                    userControl_handWidget.Visible = true;
                    userControl_scaleCircleWidget.Visible = true;
                    userControl_scaleLinearWidget.Visible = true;
                    userControl_SystemFont_GroupWidget.Visible = true;
                    //userControl_SystemFont_GroupWeatherWidget.Visible = true;
                    //userControl_SystemFont_GroupSunriseWidget.Visible = true;
                    userControl_iconWidget.Visible = true;
                    //tabControl_DateWidget.Visible = true;

                    userControl_SystemFont_GroupWidget.ShowGoal = true;
                    userControl_SystemFont_GroupWidget.FollowText = userControl_SystemFont_Group_Calories.FollowText;
                    userControl_SystemFont_GroupWidget.FollowRotateText = userControl_SystemFont_Group_Calories.FollowRotateText;
                    userControl_SystemFont_GroupWidget.SystemFontText = userControl_SystemFont_Group_Calories.SystemFontText;
                    userControl_SystemFont_GroupWidget.FontRotateText = userControl_SystemFont_Group_Calories.FontRotateText;

                    userControl_text_goalWidget.ButtonText = userControl_text_goal_Calories.ButtonText;
                    userControl_text_goalWidget.FollowText = userControl_text_goal_Calories.FollowText;


                    userPanel_pictures = userControl_picturesWidget;
                    userPanel_text = userControl_textWidget;
                    userPanel_textGoal = userControl_text_goalWidget;
                    userPanel_hand = userControl_handWidget;
                    userPanel_scaleCircle = userControl_scaleCircleWidget;
                    userPanel_scaleLinear = userControl_scaleLinearWidget;
                    userControl_SystemFont_Group = userControl_SystemFont_GroupWidget;
                    userControl_icon = userControl_iconWidget;
                    break;

                case "HeartRate":
                    userControl_previewWidget.Visible = true;
                    userControl_picturesWidget.Visible = true;
                    userControl_textWidget.Visible = true;
                    //userControl_text_goalWidget.Visible = true;
                    //userControl_text_weatherWidgetCur.Visible = true;
                    //userControl_text_weatherWidgetMin.Visible = true;
                    //userControl_text_weatherWidgetMax.Visible = true;
                    //userControl_text_goalWidgetSunrise.Visible = true;
                    //userControl_text_goalWidgetSunset.Visible = true;
                    userControl_handWidget.Visible = true;
                    userControl_scaleCircleWidget.Visible = true;
                    userControl_scaleLinearWidget.Visible = true;
                    userControl_SystemFont_GroupWidget.Visible = true;
                    //userControl_SystemFont_GroupWeatherWidget.Visible = true;
                    //userControl_SystemFont_GroupSunriseWidget.Visible = true;
                    userControl_iconWidget.Visible = true;
                    //tabControl_DateWidget.Visible = true;

                    userControl_SystemFont_GroupWidget.ShowGoal = false;


                    userPanel_pictures = userControl_picturesWidget;
                    userPanel_text = userControl_textWidget;
                    //userPanel_textGoal = userControl_text_goalWidget;
                    userPanel_hand = userControl_handWidget;
                    userPanel_scaleCircle = userControl_scaleCircleWidget;
                    userPanel_scaleLinear = userControl_scaleLinearWidget;
                    userControl_SystemFont_Group = userControl_SystemFont_GroupWidget;
                    userControl_icon = userControl_iconWidget;
                    break;

                case "PAI":
                    userControl_previewWidget.Visible = true;
                    userControl_picturesWidget.Visible = true;
                    userControl_textWidget.Visible = true;
                    //userControl_text_goalWidget.Visible = true;
                    //userControl_text_weatherWidgetCur.Visible = true;
                    //userControl_text_weatherWidgetMin.Visible = true;
                    //userControl_text_weatherWidgetMax.Visible = true;
                    //userControl_text_goalWidgetSunrise.Visible = true;
                    //userControl_text_goalWidgetSunset.Visible = true;
                    userControl_handWidget.Visible = true;
                    userControl_scaleCircleWidget.Visible = true;
                    userControl_scaleLinearWidget.Visible = true;
                    userControl_SystemFont_GroupWidget.Visible = true;
                    //userControl_SystemFont_GroupWeatherWidget.Visible = true;
                    //userControl_SystemFont_GroupSunriseWidget.Visible = true;
                    userControl_iconWidget.Visible = true;
                    //tabControl_DateWidget.Visible = true;

                    userControl_SystemFont_GroupWidget.ShowGoal = false;


                    userPanel_pictures = userControl_picturesWidget;
                    userPanel_text = userControl_textWidget;
                    //userPanel_textGoal = userControl_text_goalWidget;
                    userPanel_hand = userControl_handWidget;
                    userPanel_scaleCircle = userControl_scaleCircleWidget;
                    userPanel_scaleLinear = userControl_scaleLinearWidget;
                    userControl_SystemFont_Group = userControl_SystemFont_GroupWidget;
                    userControl_icon = userControl_iconWidget;
                    break;

                case "Distance":
                    userControl_previewWidget.Visible = true;
                    //userControl_picturesWidget.Visible = true;
                    userControl_textWidget.Visible = true;
                    //userControl_text_goalWidget.Visible = true;
                    //userControl_text_weatherWidgetCur.Visible = true;
                    //userControl_text_weatherWidgetMin.Visible = true;
                    //userControl_text_weatherWidgetMax.Visible = true;
                    //userControl_text_goalWidgetSunrise.Visible = true;
                    //userControl_text_goalWidgetSunset.Visible = true;
                    //userControl_handWidget.Visible = true;
                    //userControl_scaleCircleWidget.Visible = true;
                    //userControl_scaleLinearWidget.Visible = true;
                    userControl_SystemFont_GroupWidget.Visible = true;
                    //userControl_SystemFont_GroupWeatherWidget.Visible = true;
                    //userControl_SystemFont_GroupSunriseWidget.Visible = true;
                    userControl_iconWidget.Visible = true;
                    //tabControl_DateWidget.Visible = true;

                    userControl_SystemFont_GroupWidget.ShowGoal = false;
                    userControl_textWidget.OptionalSymbol = true;


                    userPanel_pictures = userControl_picturesWidget;
                    userPanel_text = userControl_textWidget;
                    //userPanel_textGoal = userControl_text_goalWidget;
                    //userPanel_hand = userControl_handWidget;
                    //userPanel_scaleCircle = userControl_scaleCircleWidget;
                    //userPanel_scaleLinear = userControl_scaleLinearWidget;
                    userControl_SystemFont_Group = userControl_SystemFont_GroupWidget;
                    userControl_icon = userControl_iconWidget;
                    break;

                case "StandUp":
                    userControl_previewWidget.Visible = true;
                    userControl_picturesWidget.Visible = true;
                    userControl_textWidget.Visible = true;
                    userControl_text_goalWidget.Visible = true;
                    //userControl_text_weatherWidgetCur.Visible = true;
                    //userControl_text_weatherWidgetMin.Visible = true;
                    //userControl_text_weatherWidgetMax.Visible = true;
                    //userControl_text_goalWidgetSunrise.Visible = true;
                    //userControl_text_goalWidgetSunset.Visible = true;
                    userControl_handWidget.Visible = true;
                    userControl_scaleCircleWidget.Visible = true;
                    userControl_scaleLinearWidget.Visible = true;
                    userControl_SystemFont_GroupWidget.Visible = true;
                    //userControl_SystemFont_GroupWeatherWidget.Visible = true;
                    //userControl_SystemFont_GroupSunriseWidget.Visible = true;
                    userControl_iconWidget.Visible = true;
                    //tabControl_DateWidget.Visible = true;

                    userControl_SystemFont_GroupWidget.ShowGoal = true;
                    userControl_SystemFont_GroupWidget.FollowText = userControl_SystemFont_Group_StandUp.FollowText;
                    userControl_SystemFont_GroupWidget.FollowRotateText = userControl_SystemFont_Group_StandUp.FollowRotateText;
                    userControl_SystemFont_GroupWidget.SystemFontText = userControl_SystemFont_Group_StandUp.SystemFontText;
                    userControl_SystemFont_GroupWidget.FontRotateText = userControl_SystemFont_Group_StandUp.FontRotateText;

                    userControl_text_goalWidget.ButtonText = userControl_text_goal_StandUp.ButtonText;
                    userControl_text_goalWidget.FollowText = userControl_text_goal_StandUp.FollowText;


                    userPanel_pictures = userControl_picturesWidget;
                    userPanel_text = userControl_textWidget;
                    userPanel_textGoal = userControl_text_goalWidget;
                    userPanel_hand = userControl_handWidget;
                    userPanel_scaleCircle = userControl_scaleCircleWidget;
                    userPanel_scaleLinear = userControl_scaleLinearWidget;
                    userControl_SystemFont_Group = userControl_SystemFont_GroupWidget;
                    userControl_icon = userControl_iconWidget;
                    break;

                case "Weather":
                    userControl_previewWidget.Visible = true;
                    //userControl_picturesWidget.Visible = true;
                    userControl_pictures_weatherWidget.Visible = true;
                    //userControl_textWidget.Visible = true;
                    //userControl_text_goalWidget.Visible = true;
                    userControl_text_weatherWidgetCur.Visible = true;
                    userControl_text_weatherWidgetMin.Visible = true;
                    userControl_text_weatherWidgetMax.Visible = true;
                    //userControl_text_goalWidgetSunrise.Visible = true;
                    //userControl_text_goalWidgetSunset.Visible = true;
                    //userControl_handWidget.Visible = true;
                    //userControl_scaleCircleWidget.Visible = true;
                    //userControl_scaleLinearWidget.Visible = true;
                    //userControl_SystemFont_GroupWidget.Visible = true;
                    userControl_SystemFont_GroupWeatherWidget.Visible = true;
                    //userControl_SystemFont_GroupSunriseWidget.Visible = true;
                    userControl_iconWidget.Visible = true;
                    //tabControl_DateWidget.Visible = true;

                    userPanel_pictures = userControl_pictures_weatherWidget;
                    userPanel_text_weather_sunrise = userControl_text_weatherWidgetCur;
                    userControl_icon = userControl_iconWidget;
                    break;
                case "UVindex":
                    userControl_previewWidget.Visible = true;
                    userControl_picturesWidget.Visible = true;
                    userControl_textWidget.Visible = true;
                    //userControl_text_goalWidget.Visible = true;
                    //userControl_text_weatherWidgetCur.Visible = true;
                    //userControl_text_weatherWidgetMin.Visible = true;
                    //userControl_text_weatherWidgetMax.Visible = true;
                    //userControl_text_goalWidgetSunrise.Visible = true;
                    //userControl_text_goalWidgetSunset.Visible = true;
                    userControl_handWidget.Visible = true;
                    userControl_scaleCircleWidget.Visible = true;
                    userControl_scaleLinearWidget.Visible = true;
                    userControl_SystemFont_GroupWidget.Visible = true;
                    //userControl_SystemFont_GroupWeatherWidget.Visible = true;
                    //userControl_SystemFont_GroupSunriseWidget.Visible = true;
                    userControl_iconWidget.Visible = true;
                    //tabControl_DateWidget.Visible = true;

                    userControl_SystemFont_GroupWidget.ShowGoal = false;


                    userPanel_pictures = userControl_picturesWidget;
                    userPanel_text = userControl_textWidget;
                    //userPanel_textGoal = userControl_text_goalWidget;
                    userPanel_hand = userControl_handWidget;
                    userPanel_scaleCircle = userControl_scaleCircleWidget;
                    userPanel_scaleLinear = userControl_scaleLinearWidget;
                    userControl_SystemFont_Group = userControl_SystemFont_GroupWidget;
                    userControl_icon = userControl_iconWidget;
                    break;

                case "AirQuality":
                    userControl_previewWidget.Visible = true;
                    userControl_picturesWidget.Visible = true;
                    userControl_textWidget.Visible = true;
                    //userControl_text_goalWidget.Visible = true;
                    //userControl_text_weatherWidgetCur.Visible = true;
                    //userControl_text_weatherWidgetMin.Visible = true;
                    //userControl_text_weatherWidgetMax.Visible = true;
                    //userControl_text_goalWidgetSunrise.Visible = true;
                    //userControl_text_goalWidgetSunset.Visible = true;
                    userControl_handWidget.Visible = true;
                    userControl_scaleCircleWidget.Visible = true;
                    userControl_scaleLinearWidget.Visible = true;
                    userControl_SystemFont_GroupWidget.Visible = true;
                    //userControl_SystemFont_GroupWeatherWidget.Visible = true;
                    //userControl_SystemFont_GroupSunriseWidget.Visible = true;
                    userControl_iconWidget.Visible = true;
                    //tabControl_DateWidget.Visible = true;

                    userControl_SystemFont_GroupWidget.ShowGoal = false;


                    userPanel_pictures = userControl_picturesWidget;
                    userPanel_text = userControl_textWidget;
                    //userPanel_textGoal = userControl_text_goalWidget;
                    userPanel_hand = userControl_handWidget;
                    userPanel_scaleCircle = userControl_scaleCircleWidget;
                    userPanel_scaleLinear = userControl_scaleLinearWidget;
                    userControl_SystemFont_Group = userControl_SystemFont_GroupWidget;
                    userControl_icon = userControl_iconWidget;
                    break;

                case "Humidity":
                    userControl_previewWidget.Visible = true;
                    userControl_picturesWidget.Visible = true;
                    userControl_textWidget.Visible = true;
                    //userControl_text_goalWidget.Visible = true;
                    //userControl_text_weatherWidgetCur.Visible = true;
                    //userControl_text_weatherWidgetMin.Visible = true;
                    //userControl_text_weatherWidgetMax.Visible = true;
                    //userControl_text_goalWidgetSunrise.Visible = true;
                    //userControl_text_goalWidgetSunset.Visible = true;
                    userControl_handWidget.Visible = true;
                    userControl_scaleCircleWidget.Visible = true;
                    userControl_scaleLinearWidget.Visible = true;
                    userControl_SystemFont_GroupWidget.Visible = true;
                    //userControl_SystemFont_GroupWeatherWidget.Visible = true;
                    //userControl_SystemFont_GroupSunriseWidget.Visible = true;
                    userControl_iconWidget.Visible = true;
                    //tabControl_DateWidget.Visible = true;

                    userControl_SystemFont_GroupWidget.ShowGoal = false;


                    userPanel_pictures = userControl_picturesWidget;
                    userPanel_text = userControl_textWidget;
                    //userPanel_textGoal = userControl_text_goalWidget;
                    userPanel_hand = userControl_handWidget;
                    userPanel_scaleCircle = userControl_scaleCircleWidget;
                    userPanel_scaleLinear = userControl_scaleLinearWidget;
                    userControl_SystemFont_Group = userControl_SystemFont_GroupWidget;
                    userControl_icon = userControl_iconWidget;
                    break;

                case "Sunrise ":
                    userControl_previewWidget.Visible = true;
                    userControl_picturesWidget.Visible = true;
                    //userControl_textWidget.Visible = true;
                    userControl_text_goalWidget.Visible = true;
                    //userControl_text_weatherWidgetCur.Visible = true;
                    //userControl_text_weatherWidgetMin.Visible = true;
                    //userControl_text_weatherWidgetMax.Visible = true;
                    userControl_text_goalWidgetSunrise.Visible = true;
                    userControl_text_goalWidgetSunset.Visible = true;
                    userControl_handWidget.Visible = true;
                    userControl_scaleCircleWidget.Visible = true;
                    userControl_scaleLinearWidget.Visible = true;
                    //userControl_SystemFont_GroupWidget.Visible = true;
                    //userControl_SystemFont_GroupWeatherWidget.Visible = true;
                    userControl_SystemFont_GroupSunriseWidget.Visible = true;
                    userControl_iconWidget.Visible = true;
                    //tabControl_DateWidget.Visible = true;

                    userControl_text_goalWidget.ButtonText = userControl_text_SunriseSunset.ButtonText;
                    userControl_text_goalWidget.Follow = false;

                    userPanel_pictures = userControl_picturesWidget;
                    userPanel_text = userControl_text_goalWidget;
                    userPanel_hand = userControl_handWidget;
                    userPanel_scaleCircle = userControl_scaleCircleWidget;
                    userPanel_scaleLinear = userControl_scaleLinearWidget;
                    userControl_icon = userControl_iconWidget;
                    break;

                case "WindForce":
                    userControl_previewWidget.Visible = true;
                    userControl_picturesWidget.Visible = true;
                    userControl_textWidget.Visible = true;
                    //userControl_text_goalWidget.Visible = true;
                    //userControl_text_weatherWidgetCur.Visible = true;
                    //userControl_text_weatherWidgetMin.Visible = true;
                    //userControl_text_weatherWidgetMax.Visible = true;
                    //userControl_text_goalWidgetSunrise.Visible = true;
                    //userControl_text_goalWidgetSunset.Visible = true;
                    userControl_handWidget.Visible = true;
                    userControl_scaleCircleWidget.Visible = true;
                    userControl_scaleLinearWidget.Visible = true;
                    userControl_SystemFont_GroupWidget.Visible = true;
                    //userControl_SystemFont_GroupWeatherWidget.Visible = true;
                    //userControl_SystemFont_GroupSunriseWidget.Visible = true;
                    userControl_iconWidget.Visible = true;
                    //tabControl_DateWidget.Visible = true;

                    userControl_SystemFont_GroupWidget.ShowGoal = false;


                    userPanel_pictures = userControl_picturesWidget;
                    userPanel_text = userControl_textWidget;
                    //userPanel_textGoal = userControl_text_goalWidget;
                    userPanel_hand = userControl_handWidget;
                    userPanel_scaleCircle = userControl_scaleCircleWidget;
                    userPanel_scaleLinear = userControl_scaleLinearWidget;
                    userControl_SystemFont_Group = userControl_SystemFont_GroupWidget;
                    userControl_icon = userControl_iconWidget;
                    break;

                case "Altitude":
                    userControl_previewWidget.Visible = true;
                    userControl_picturesWidget.Visible = true;
                    userControl_textWidget.Visible = true;
                    //userControl_text_goalWidget.Visible = true;
                    //userControl_text_weatherWidgetCur.Visible = true;
                    //userControl_text_weatherWidgetMin.Visible = true;
                    //userControl_text_weatherWidgetMax.Visible = true;
                    //userControl_text_goalWidgetSunrise.Visible = true;
                    //userControl_text_goalWidgetSunset.Visible = true;
                    userControl_handWidget.Visible = true;
                    userControl_scaleCircleWidget.Visible = true;
                    userControl_scaleLinearWidget.Visible = true;
                    userControl_SystemFont_GroupWidget.Visible = true;
                    //userControl_SystemFont_GroupWeatherWidget.Visible = true;
                    //userControl_SystemFont_GroupSunriseWidget.Visible = true;
                    userControl_iconWidget.Visible = true;
                    //tabControl_DateWidget.Visible = true;

                    userControl_SystemFont_GroupWidget.ShowGoal = false;


                    userPanel_pictures = userControl_picturesWidget;
                    userPanel_text = userControl_textWidget;
                    //userPanel_textGoal = userControl_text_goalWidget;
                    userPanel_hand = userControl_handWidget;
                    userPanel_scaleCircle = userControl_scaleCircleWidget;
                    userPanel_scaleLinear = userControl_scaleLinearWidget;
                    userControl_SystemFont_Group = userControl_SystemFont_GroupWidget;
                    userControl_icon = userControl_iconWidget;
                    break;

                case "AirPressure":
                    userControl_previewWidget.Visible = true;
                    userControl_picturesWidget.Visible = true;
                    userControl_textWidget.Visible = true;
                    //userControl_text_goalWidget.Visible = true;
                    //userControl_text_weatherWidgetCur.Visible = true;
                    //userControl_text_weatherWidgetMin.Visible = true;
                    //userControl_text_weatherWidgetMax.Visible = true;
                    //userControl_text_goalWidgetSunrise.Visible = true;
                    //userControl_text_goalWidgetSunset.Visible = true;
                    userControl_handWidget.Visible = true;
                    userControl_scaleCircleWidget.Visible = true;
                    userControl_scaleLinearWidget.Visible = true;
                    userControl_SystemFont_GroupWidget.Visible = true;
                    //userControl_SystemFont_GroupWeatherWidget.Visible = true;
                    //userControl_SystemFont_GroupSunriseWidget.Visible = true;
                    userControl_iconWidget.Visible = true;
                    //tabControl_DateWidget.Visible = true;

                    userControl_SystemFont_GroupWidget.ShowGoal = false;


                    userPanel_pictures = userControl_picturesWidget;
                    userPanel_text = userControl_textWidget;
                    //userPanel_textGoal = userControl_text_goalWidget;
                    userPanel_hand = userControl_handWidget;
                    userPanel_scaleCircle = userControl_scaleCircleWidget;
                    userPanel_scaleLinear = userControl_scaleLinearWidget;
                    userControl_SystemFont_Group = userControl_SystemFont_GroupWidget;
                    userControl_icon = userControl_iconWidget;
                    break;

                case "Stress":
                    userControl_previewWidget.Visible = true;
                    userControl_picturesWidget.Visible = true;
                    userControl_textWidget.Visible = true;
                    //userControl_text_goalWidget.Visible = true;
                    //userControl_text_weatherWidgetCur.Visible = true;
                    //userControl_text_weatherWidgetMin.Visible = true;
                    //userControl_text_weatherWidgetMax.Visible = true;
                    //userControl_text_goalWidgetSunrise.Visible = true;
                    //userControl_text_goalWidgetSunset.Visible = true;
                    userControl_handWidget.Visible = true;
                    userControl_scaleCircleWidget.Visible = true;
                    userControl_scaleLinearWidget.Visible = true;
                    userControl_SystemFont_GroupWidget.Visible = true;
                    //userControl_SystemFont_GroupWeatherWidget.Visible = true;
                    //userControl_SystemFont_GroupSunriseWidget.Visible = true;
                    userControl_iconWidget.Visible = true;
                    //tabControl_DateWidget.Visible = true;

                    userControl_SystemFont_GroupWidget.ShowGoal = false;


                    userPanel_pictures = userControl_picturesWidget;
                    userPanel_text = userControl_textWidget;
                    //userPanel_textGoal = userControl_text_goalWidget;
                    userPanel_hand = userControl_handWidget;
                    userPanel_scaleCircle = userControl_scaleCircleWidget;
                    userPanel_scaleLinear = userControl_scaleLinearWidget;
                    userControl_SystemFont_Group = userControl_SystemFont_GroupWidget;
                    userControl_icon = userControl_iconWidget;
                    break;

                case "ActivityGoal":
                    userControl_previewWidget.Visible = true;
                    userControl_picturesWidget.Visible = true;
                    userControl_textWidget.Visible = true;
                    userControl_text_goalWidget.Visible = true;
                    //userControl_text_weatherWidgetCur.Visible = true;
                    //userControl_text_weatherWidgetMin.Visible = true;
                    //userControl_text_weatherWidgetMax.Visible = true;
                    //userControl_text_goalWidgetSunrise.Visible = true;
                    //userControl_text_goalWidgetSunset.Visible = true;
                    userControl_handWidget.Visible = true;
                    userControl_scaleCircleWidget.Visible = true;
                    userControl_scaleLinearWidget.Visible = true;
                    userControl_SystemFont_GroupWidget.Visible = true;
                    //userControl_SystemFont_GroupWeatherWidget.Visible = true;
                    //userControl_SystemFont_GroupSunriseWidget.Visible = true;
                    userControl_iconWidget.Visible = true;
                    //tabControl_DateWidget.Visible = true;

                    userControl_SystemFont_GroupWidget.ShowGoal = true;
                    userControl_iconWidget.Image2 = true;
                    userControl_SystemFont_GroupWidget.FollowText = userControl_SystemFont_Group_ActivityGoal.FollowText;
                    userControl_SystemFont_GroupWidget.FollowRotateText = userControl_SystemFont_Group_ActivityGoal.FollowRotateText;
                    userControl_SystemFont_GroupWidget.SystemFontText = userControl_SystemFont_Group_ActivityGoal.SystemFontText;
                    userControl_SystemFont_GroupWidget.FontRotateText = userControl_SystemFont_Group_ActivityGoal.FontRotateText;

                    userControl_text_goalWidget.ButtonText = userControl_text_goal_ActivityGoal.ButtonText;
                    userControl_text_goalWidget.FollowText = userControl_text_goal_ActivityGoal.FollowText;


                    userPanel_pictures = userControl_picturesWidget;
                    userPanel_text = userControl_textWidget;
                    userPanel_textGoal = userControl_text_goalWidget;
                    userPanel_hand = userControl_handWidget;
                    userPanel_scaleCircle = userControl_scaleCircleWidget;
                    userPanel_scaleLinear = userControl_scaleLinearWidget;
                    userControl_SystemFont_Group = userControl_SystemFont_GroupWidget;
                    userControl_icon = userControl_iconWidget;
                    break;

                case "FatBurning":
                    userControl_previewWidget.Visible = true;
                    userControl_picturesWidget.Visible = true;
                    userControl_textWidget.Visible = true;
                    //userControl_text_goalWidget.Visible = true;
                    //userControl_text_weatherWidgetCur.Visible = true;
                    //userControl_text_weatherWidgetMin.Visible = true;
                    //userControl_text_weatherWidgetMax.Visible = true;
                    //userControl_text_goalWidgetSunrise.Visible = true;
                    //userControl_text_goalWidgetSunset.Visible = true;
                    userControl_handWidget.Visible = true;
                    userControl_scaleCircleWidget.Visible = true;
                    userControl_scaleLinearWidget.Visible = true;
                    userControl_SystemFont_GroupWidget.Visible = true;
                    //userControl_SystemFont_GroupWeatherWidget.Visible = true;
                    //userControl_SystemFont_GroupSunriseWidget.Visible = true;
                    userControl_iconWidget.Visible = true;
                    //tabControl_DateWidget.Visible = true;

                    userControl_SystemFont_GroupWidget.ShowGoal = true;
                    userControl_SystemFont_GroupWidget.FollowText = userControl_SystemFont_Group_FatBurning.FollowText;
                    userControl_SystemFont_GroupWidget.FollowRotateText = userControl_SystemFont_Group_FatBurning.FollowRotateText;
                    userControl_SystemFont_GroupWidget.SystemFontText = userControl_SystemFont_Group_FatBurning.SystemFontText;
                    userControl_SystemFont_GroupWidget.FontRotateText = userControl_SystemFont_Group_FatBurning.FontRotateText;

                    userControl_text_goalWidget.ButtonText = userControl_text_goal_FatBurning.ButtonText;
                    userControl_text_goalWidget.FollowText = userControl_text_goal_FatBurning.FollowText;


                    userPanel_pictures = userControl_picturesWidget;
                    userPanel_text = userControl_textWidget;
                    //userPanel_textGoal = userControl_text_goalWidget;
                    userPanel_hand = userControl_handWidget;
                    userPanel_scaleCircle = userControl_scaleCircleWidget;
                    userPanel_scaleLinear = userControl_scaleLinearWidget;
                    userControl_SystemFont_Group = userControl_SystemFont_GroupWidget;
                    userControl_icon = userControl_iconWidget;
                    break;

                case "Date":
                    userControl_previewWidget.Visible = true;
                    //userControl_picturesWidget.Visible = true;
                    //userControl_textWidget.Visible = true;
                    //userControl_text_goalWidget.Visible = true;
                    //userControl_text_weatherWidgetCur.Visible = true;
                    //userControl_text_weatherWidgetMin.Visible = true;
                    //userControl_text_weatherWidgetMax.Visible = true;
                    //userControl_text_goalWidgetSunrise.Visible = true;
                    //userControl_text_goalWidgetSunset.Visible = true;
                    //userControl_handWidget.Visible = true;
                    //userControl_scaleCircleWidget.Visible = true;
                    //userControl_scaleLinearWidget.Visible = true;
                    //userControl_SystemFont_GroupWidget.Visible = true;
                    //userControl_SystemFont_GroupWeatherWidget.Visible = true;
                    //userControl_SystemFont_GroupSunriseWidget.Visible = true;
                    //userControl_iconWidget.Visible = true;
                    tabControl_DateWidget.Visible = true;
                    break;
            }

            if (userControl_SystemFont_Group != null)
            {
                userControl_SystemFont = userControl_SystemFont_Group.userControl_SystemFont;
                userControl_FontRotate = userControl_SystemFont_Group.userControl_FontRotate;
                userControl_SystemFontGoal = userControl_SystemFont_Group.userControl_SystemFont_goal;
                userControl_FontRotateGoal = userControl_SystemFont_Group.userControl_FontRotate_goal;
            }

            if (widgetElement.Preview != null && userControl_preview != null)
            {
                foreach (MultilangImage multilangImage in widgetElement.Preview)
                {
                    if (multilangImage.LangCode == "All") userControl_preview.comboBoxSetImage(multilangImage.ImageSet.ImageIndex);
                }
            }

            if (widgetElement.Activity != null)
            {
                Activity activity = widgetElement.Activity[0];
                // набор картинок
                if (userPanel_pictures != null)
                {
                    if (activity.ImageProgress != null && activity.ImageProgress.ImageSet != null &&
                        activity.ImageProgress.Coordinates != null && OneCoordinates(activity.ImageProgress.Coordinates))

                    //activity.ImageProgress.Coordinates != null && activity.ImageProgress.Coordinates.Count == 1)
                    {
                        userPanel_pictures.checkBox_pictures_Use.Checked = true;

                        //comboBoxSetText(comboBox_image, activity.ImageProgress.ImageSet.ImageIndex);
                        long numericUpDown_count = activity.ImageProgress.ImageSet.ImagesCount;
                        long numericUpDownX = activity.ImageProgress.Coordinates[0].X;
                        long numericUpDownY = activity.ImageProgress.Coordinates[0].Y;

                        userPanel_pictures.comboBoxSetImage(activity.ImageProgress.ImageSet.ImageIndex);
                        userPanel_pictures.numericUpDown_picturesX.Value = numericUpDownX;
                        userPanel_pictures.numericUpDown_picturesY.Value = numericUpDownY;
                        userPanel_pictures.numericUpDown_pictures_count.Value = numericUpDown_count;
                    }
                }

                // надпиь и системный шрифт
                if (activity.Type != "Weather" && activity.Type != "Sunrise")
                {
                    if (activity.Digits != null && activity.Digits.Count > 0)
                    {
                        foreach (DigitalCommonDigit digitalCommonDigit in activity.Digits)
                        {
                            userPanel_text_Activity = userPanel_text;
                            userControl_SystemFont_Activity = userControl_SystemFont;
                            userControl_FontRotate_Activity = userControl_FontRotate;

                            if (digitalCommonDigit.Type != null && digitalCommonDigit.Type == "Min")
                            {
                                userPanel_text_Activity = userPanel_textGoal;
                                userControl_SystemFont_Activity = userControl_SystemFontGoal;
                                userControl_FontRotate_Activity = userControl_FontRotateGoal;
                            }
                            if (digitalCommonDigit.Type != null && digitalCommonDigit.Type == "Max") continue;

                            // надпиь
                            if (digitalCommonDigit.Digit != null && digitalCommonDigit.Digit.Image != null &&
                            userPanel_text_Activity != null)
                            {
                                userPanel_text_Activity.checkBox_Use.Checked = true;
                                //ComboBox comboBox_image = (ComboBox)userPanel_text_Activity.Controls[1];
                                //ComboBox comboBox_unit = (ComboBox)userPanel_text_Activity.Controls[2];
                                //ComboBox comboBox_separator = (ComboBox)userPanel_text_Activity.Controls[3];
                                NumericUpDown numericUpDownX = userPanel_text_Activity.numericUpDown_imageX;
                                NumericUpDown numericUpDownY = userPanel_text_Activity.numericUpDown_imageY;
                                NumericUpDown numericUpDown_unitX = userPanel_text_Activity.numericUpDown_iconX;
                                NumericUpDown numericUpDown_unitY = userPanel_text_Activity.numericUpDown_iconY;
                                //ComboBox comboBox_alignment = (ComboBox)userPanel_text_Activity.Controls[8];
                                NumericUpDown numericUpDown_spacing = userPanel_text_Activity.numericUpDown_spacing;
                                CheckBox checkBox_add_zero = userPanel_text_Activity.checkBox_addZero;
                                CheckBox checkBox_follow = userPanel_text_Activity.checkBox_follow;

                                numericUpDownX.Value = digitalCommonDigit.Digit.Image.X;
                                numericUpDownY.Value = digitalCommonDigit.Digit.Image.Y;
                                if (digitalCommonDigit.CombingMode == null ||
                                    digitalCommonDigit.CombingMode == "Follow") checkBox_follow.Checked = true;

                                // десятичный разделитель
                                if (activity.Type == "Distance")
                                {
                                    //ComboBox comboBox_DecimalPoint = (ComboBox)panel_text.Controls[12];
                                    if (digitalCommonDigit.Digit.Image.DecimalPointImageIndex != null)
                                        userPanel_text_Activity.comboBoxSetImageDecimalPointOrMinus((int)digitalCommonDigit.Digit.Image.DecimalPointImageIndex);
                                }

                                if (digitalCommonDigit.Digit.Image.NoDataImageIndex != null)
                                    userPanel_text_Activity.comboBoxSetImageError((int)digitalCommonDigit.Digit.Image.NoDataImageIndex);
                                foreach (MultilangImage multilangImage in digitalCommonDigit.Digit.Image.MultilangImage)
                                {
                                    if (multilangImage.LangCode == "All")
                                        userPanel_text_Activity.comboBoxSetImage((int)multilangImage.ImageSet.ImageIndex);
                                }
                                if (digitalCommonDigit.Digit.Image.MultilangImageUnit != null)
                                {
                                    foreach (MultilangImage multilangImage in digitalCommonDigit.Digit.Image.MultilangImageUnit)
                                    {
                                        if (multilangImage.LangCode == "All")
                                            userPanel_text_Activity.comboBoxSetUnit((int)multilangImage.ImageSet.ImageIndex);
                                    }
                                }
                                userPanel_text_Activity.comboBoxSetAlignment(digitalCommonDigit.Digit.Alignment);
                                if (digitalCommonDigit.Digit.Spacing != null)
                                    numericUpDown_spacing.Value = (decimal)digitalCommonDigit.Digit.Spacing;
                                checkBox_add_zero.Checked = digitalCommonDigit.Digit.PaddingZero;
                                if (digitalCommonDigit.Separator != null)
                                {
                                    userPanel_text_Activity.comboBoxSetIcon((int)digitalCommonDigit.Separator.ImageIndex);
                                    numericUpDown_unitX.Value = digitalCommonDigit.Separator.Coordinates.X;
                                    numericUpDown_unitY.Value = digitalCommonDigit.Separator.Coordinates.Y;
                                }
                            }

                            // системный шрифт
                            if (digitalCommonDigit.Digit != null && digitalCommonDigit.Digit.SystemFont != null &&
                                digitalCommonDigit.Digit.SystemFont.FontRotate == null && userControl_SystemFont_Activity != null)
                            {
                                userControl_SystemFont_Activity.checkBox_Use.Checked = true;
                                NumericUpDown numericUpDownX = userControl_SystemFont_Activity.numericUpDown_SystemFontX;
                                NumericUpDown numericUpDownY = userControl_SystemFont_Activity.numericUpDown_SystemFontY;
                                NumericUpDown numericUpDown_size = userControl_SystemFont_Activity.numericUpDown_SystemFont_size;
                                NumericUpDown numericUpDown_angle = userControl_SystemFont_Activity.numericUpDown_SystemFont_angle;
                                CheckBox checkBox_add_zero = userControl_SystemFont_Activity.checkBox_addZero;
                                NumericUpDown numericUpDown_spacing = userControl_SystemFont_Activity.numericUpDown_SystemFont_spacing;
                                CheckBox checkBox_follow = userControl_SystemFont_Activity.checkBox_follow;
                                CheckBox checkBox_separator = userControl_SystemFont_Activity.checkBox_separator;

                                if (digitalCommonDigit.CombingMode == null ||
                                    digitalCommonDigit.CombingMode == "Follow") checkBox_follow.Checked = true;
                                if (digitalCommonDigit.Separator != null) checkBox_separator.Checked = true;

                                if (digitalCommonDigit.Digit.SystemFont.Coordinates != null)
                                {
                                    numericUpDownX.Value = digitalCommonDigit.Digit.SystemFont.Coordinates.X;
                                    numericUpDownY.Value = digitalCommonDigit.Digit.SystemFont.Coordinates.Y;
                                }
                                numericUpDown_size.Value = digitalCommonDigit.Digit.SystemFont.Size;
                                numericUpDown_angle.Value = digitalCommonDigit.Digit.SystemFont.Angle;
                                if (digitalCommonDigit.Digit.Spacing != null)
                                {
                                    numericUpDown_spacing.Value = (int)digitalCommonDigit.Digit.Spacing;
                                }
                                userControl_SystemFont_Activity.comboBoxSetColorString(digitalCommonDigit.Digit.SystemFont.Color);
                                userControl_SystemFont_Activity.checkBoxSetUnit((int)digitalCommonDigit.Digit.SystemFont.ShowUnitCheck);
                                checkBox_add_zero.Checked = digitalCommonDigit.Digit.PaddingZero;
                            }

                            // системный шрифт по окружности
                            if (digitalCommonDigit.Digit != null && digitalCommonDigit.Digit.SystemFont != null &&
                                digitalCommonDigit.Digit.SystemFont.FontRotate != null && userControl_FontRotate_Activity != null)
                            {
                                userControl_FontRotate_Activity.checkBox_Use.Checked = true;
                                NumericUpDown numericUpDownX = userControl_FontRotate_Activity.numericUpDown_FontRotateX;
                                NumericUpDown numericUpDownY = userControl_FontRotate_Activity.numericUpDown_FontRotateY;
                                NumericUpDown numericUpDown_size = userControl_FontRotate_Activity.numericUpDown_FontRotate_size;
                                NumericUpDown numericUpDown_angle = userControl_FontRotate_Activity.numericUpDown_FontRotate_angle;
                                NumericUpDown numericUpDown_radius = userControl_FontRotate_Activity.numericUpDown_FontRotate_radius;
                                NumericUpDown numericUpDown_spacing = userControl_FontRotate_Activity.numericUpDown_FontRotate_spacing;
                                CheckBox checkBox_add_zero = userControl_FontRotate_Activity.checkBox_addZero;
                                CheckBox checkBox_follow = userControl_FontRotate_Activity.checkBox_follow;
                                CheckBox checkBox_separator = userControl_FontRotate_Activity.checkBox_separator;

                                if (digitalCommonDigit.CombingMode == null ||
                                    digitalCommonDigit.CombingMode == "Follow") checkBox_follow.Checked = true;
                                if (digitalCommonDigit.Separator != null) checkBox_separator.Checked = true;

                                if (digitalCommonDigit.Digit.SystemFont.FontRotate != null)
                                {
                                    numericUpDownX.Value = digitalCommonDigit.Digit.SystemFont.FontRotate.X;
                                    numericUpDownY.Value = digitalCommonDigit.Digit.SystemFont.FontRotate.Y;
                                    numericUpDown_radius.Value = digitalCommonDigit.Digit.SystemFont.FontRotate.Radius;
                                    userControl_FontRotate_Activity.radioButtonSetRotateDirection(
                                        (int)digitalCommonDigit.Digit.SystemFont.FontRotate.RotateDirection);
                                }
                                numericUpDown_size.Value = digitalCommonDigit.Digit.SystemFont.Size;
                                numericUpDown_angle.Value = digitalCommonDigit.Digit.SystemFont.Angle;
                                if (digitalCommonDigit.Digit.Spacing != null)
                                {
                                    numericUpDown_spacing.Value = (int)digitalCommonDigit.Digit.Spacing;
                                }
                                userControl_FontRotate_Activity.comboBoxSetColorString(digitalCommonDigit.Digit.SystemFont.Color);
                                userControl_FontRotate_Activity.checkBoxSetUnit((int)digitalCommonDigit.Digit.SystemFont.ShowUnitCheck);
                                checkBox_add_zero.Checked = digitalCommonDigit.Digit.PaddingZero;
                            }
                        }
                    }
                }
                else if (activity.Type == "Weather")
                {
                    UserControl_SystemFont_GroupWeather userControl_SystemFont_Group_Weather =
                        userControl_SystemFont_GroupWeatherWidget;
                    UserControl_SystemFont_weather userControl_SystemFont_weather = null;
                    UserControl_FontRotate_weather userControl_FontRotate_weather = null;

                    if (activity.Digits != null && activity.Digits.Count > 0)
                    {
                        foreach (DigitalCommonDigit digitalCommonDigit in activity.Digits)
                        {
                            if (digitalCommonDigit.Type != null && digitalCommonDigit.Type == "Min")
                            {
                                userPanel_text_weather_sunrise = userControl_text_weatherWidgetMin;
                                userControl_SystemFont_weather =
                                    userControl_SystemFont_Group_Weather.userControl_SystemFont_weather_Min;
                                userControl_FontRotate_weather =
                                    userControl_SystemFont_Group_Weather.userControl_FontRotate_weather_Min;
                            }
                            if (digitalCommonDigit.Type != null && digitalCommonDigit.Type == "Max")
                            {
                                userPanel_text_weather_sunrise = userControl_text_weatherWidgetMax;
                                userControl_SystemFont_weather =
                                    userControl_SystemFont_Group_Weather.userControl_SystemFont_weather_Max;
                                userControl_FontRotate_weather =
                                    userControl_SystemFont_Group_Weather.userControl_FontRotate_weather_Max;
                            }
                            if (digitalCommonDigit.Type == null)
                            {
                                userPanel_text_weather_sunrise = userControl_text_weatherWidgetCur;
                                userControl_SystemFont_weather =
                                    userControl_SystemFont_Group_Weather.userControl_SystemFont_weather_Current;
                                userControl_FontRotate_weather =
                                    userControl_SystemFont_Group_Weather.userControl_FontRotate_weather_Current;
                            }


                            // надпиь
                            if (digitalCommonDigit.Digit != null && digitalCommonDigit.Digit.Image != null &&
                                userPanel_text_weather_sunrise != null)
                            {
                                userPanel_text_weather_sunrise.checkBox_Use.Checked = true;
                                NumericUpDown numericUpDownX = userPanel_text_weather_sunrise.numericUpDown_imageX;
                                NumericUpDown numericUpDownY = userPanel_text_weather_sunrise.numericUpDown_imageY;
                                NumericUpDown numericUpDown_unitX = userPanel_text_weather_sunrise.numericUpDown_iconX;
                                NumericUpDown numericUpDown_unitY = userPanel_text_weather_sunrise.numericUpDown_iconY;
                                NumericUpDown numericUpDown_spacing = userPanel_text_weather_sunrise.numericUpDown_spacing;

                                if (digitalCommonDigit.Type != null && digitalCommonDigit.Type == "Max")
                                {
                                    if (digitalCommonDigit.CombingMode == "Single")
                                    {
                                        userPanel_text_weather_sunrise.checkBox_follow.Checked = false;
                                    }
                                    else
                                    {
                                        userPanel_text_weather_sunrise.checkBox_follow.Checked = true;
                                    }
                                }

                                numericUpDownX.Value = digitalCommonDigit.Digit.Image.X;
                                numericUpDownY.Value = digitalCommonDigit.Digit.Image.Y;

                                if (digitalCommonDigit.Digit.Image.NoDataImageIndex != null)
                                    userPanel_text_weather_sunrise.comboBoxSetImageError((int)digitalCommonDigit.Digit.Image.NoDataImageIndex);

                                if (digitalCommonDigit.Digit.Image.DelimiterImageIndex != null)
                                    userPanel_text_weather_sunrise.comboBoxSetImageDecimalPointOrMinus((int)digitalCommonDigit.Digit.Image.DelimiterImageIndex);

                                foreach (MultilangImage multilangImage in digitalCommonDigit.Digit.Image.MultilangImage)
                                {
                                    if (multilangImage.LangCode == "All")
                                        userPanel_text_weather_sunrise.comboBoxSetImage((int)multilangImage.ImageSet.ImageIndex);
                                }
                                if (digitalCommonDigit.Digit.Image.MultilangImageUnit != null)
                                {
                                    foreach (MultilangImage multilangImage in digitalCommonDigit.Digit.Image.MultilangImageUnit)
                                    {
                                        if (multilangImage.LangCode == null && userPanel_text_weather_sunrise.comboBoxGetSelectedIndexUnit() < 0)
                                            userPanel_text_weather_sunrise.comboBoxSetUnit((int)multilangImage.ImageSet.ImageIndex);
                                        if (multilangImage.LangCode == "All")
                                            userPanel_text_weather_sunrise.comboBoxSetUnit((int)multilangImage.ImageSet.ImageIndex);
                                    }
                                }
                                userPanel_text_weather_sunrise.comboBoxSetAlignment(digitalCommonDigit.Digit.Alignment);
                                if (digitalCommonDigit.Digit.Spacing != null)
                                    numericUpDown_spacing.Value = (decimal)digitalCommonDigit.Digit.Spacing;

                                userPanel_text_weather_sunrise.checkBox_addZero.Checked = digitalCommonDigit.Digit.PaddingZero;

                                if (digitalCommonDigit.Separator != null)
                                {
                                    userPanel_text_weather_sunrise.comboBoxSetIcon((int)digitalCommonDigit.Separator.ImageIndex);
                                    numericUpDown_unitX.Value = digitalCommonDigit.Separator.Coordinates.X;
                                    numericUpDown_unitY.Value = digitalCommonDigit.Separator.Coordinates.Y;
                                }
                            }

                            // системный шрифт
                            if (digitalCommonDigit.Digit != null && digitalCommonDigit.Digit.SystemFont != null &&
                                digitalCommonDigit.Digit.SystemFont.FontRotate == null && userControl_SystemFont_weather != null)
                            {
                                userControl_SystemFont_weather.checkBox_Use.Checked = true;
                                NumericUpDown numericUpDownX = userControl_SystemFont_weather.numericUpDown_SystemFontX;
                                NumericUpDown numericUpDownY = userControl_SystemFont_weather.numericUpDown_SystemFontY;
                                NumericUpDown numericUpDown_size = userControl_SystemFont_weather.numericUpDown_SystemFont_size;
                                NumericUpDown numericUpDown_angle = userControl_SystemFont_weather.numericUpDown_SystemFont_angle;
                                CheckBox checkBox_add_zero = userControl_SystemFont_weather.checkBox_addZero;
                                NumericUpDown numericUpDown_spacing = userControl_SystemFont_weather.numericUpDown_SystemFont_spacing;
                                CheckBox checkBox_follow = userControl_SystemFont_weather.checkBox_follow;
                                CheckBox checkBox_separator = userControl_SystemFont_weather.checkBox_separator;

                                if (digitalCommonDigit.CombingMode == null ||
                                    digitalCommonDigit.CombingMode == "Follow") checkBox_follow.Checked = true;
                                if (digitalCommonDigit.Separator != null) checkBox_separator.Checked = true;

                                if (digitalCommonDigit.Digit.SystemFont.Coordinates != null)
                                {
                                    numericUpDownX.Value = digitalCommonDigit.Digit.SystemFont.Coordinates.X;
                                    numericUpDownY.Value = digitalCommonDigit.Digit.SystemFont.Coordinates.Y;
                                }
                                numericUpDown_size.Value = digitalCommonDigit.Digit.SystemFont.Size;
                                numericUpDown_angle.Value = digitalCommonDigit.Digit.SystemFont.Angle;
                                if (digitalCommonDigit.Digit.Spacing != null)
                                {
                                    numericUpDown_spacing.Value = (int)digitalCommonDigit.Digit.Spacing;
                                }
                                userControl_SystemFont_weather.comboBoxSetColorString(digitalCommonDigit.Digit.SystemFont.Color);
                                userControl_SystemFont_weather.checkBoxSetUnit((int)digitalCommonDigit.Digit.SystemFont.ShowUnitCheck);
                                checkBox_add_zero.Checked = digitalCommonDigit.Digit.PaddingZero;
                            }


                            // системный шрифт по окружности
                            if (digitalCommonDigit.Digit != null && digitalCommonDigit.Digit.SystemFont != null &&
                                digitalCommonDigit.Digit.SystemFont.FontRotate != null && userControl_FontRotate_weather != null)
                            {
                                userControl_FontRotate_weather.checkBox_Use.Checked = true;
                                NumericUpDown numericUpDownX = userControl_FontRotate_weather.numericUpDown_FontRotateX;
                                NumericUpDown numericUpDownY = userControl_FontRotate_weather.numericUpDown_FontRotateY;
                                NumericUpDown numericUpDown_size = userControl_FontRotate_weather.numericUpDown_FontRotate_size;
                                NumericUpDown numericUpDown_angle = userControl_FontRotate_weather.numericUpDown_FontRotate_angle;
                                NumericUpDown numericUpDown_radius = userControl_FontRotate_weather.numericUpDown_FontRotate_radius;
                                NumericUpDown numericUpDown_spacing = userControl_FontRotate_weather.numericUpDown_FontRotate_spacing;
                                CheckBox checkBox_add_zero = userControl_FontRotate_weather.checkBox_addZero;
                                CheckBox checkBox_follow = userControl_FontRotate_weather.checkBox_follow;
                                CheckBox checkBox_separator = userControl_FontRotate_weather.checkBox_separator;

                                if (digitalCommonDigit.CombingMode == null ||
                                    digitalCommonDigit.CombingMode == "Follow") checkBox_follow.Checked = true;
                                if (digitalCommonDigit.Separator != null) checkBox_separator.Checked = true;

                                if (digitalCommonDigit.Digit.SystemFont.FontRotate != null)
                                {
                                    numericUpDownX.Value = digitalCommonDigit.Digit.SystemFont.FontRotate.X;
                                    numericUpDownY.Value = digitalCommonDigit.Digit.SystemFont.FontRotate.Y;
                                    numericUpDown_radius.Value = digitalCommonDigit.Digit.SystemFont.FontRotate.Radius;
                                    userControl_FontRotate_weather.radioButtonSetRotateDirection(
                                        (int)digitalCommonDigit.Digit.SystemFont.FontRotate.RotateDirection);
                                }
                                numericUpDown_size.Value = digitalCommonDigit.Digit.SystemFont.Size;
                                numericUpDown_angle.Value = digitalCommonDigit.Digit.SystemFont.Angle;
                                if (digitalCommonDigit.Digit.Spacing != null)
                                {
                                    numericUpDown_spacing.Value = (int)digitalCommonDigit.Digit.Spacing;
                                }
                                userControl_FontRotate_weather.comboBoxSetColorString(digitalCommonDigit.Digit.SystemFont.Color);
                                userControl_FontRotate_weather.checkBoxSetUnit((int)digitalCommonDigit.Digit.SystemFont.ShowUnitCheck);
                                checkBox_add_zero.Checked = digitalCommonDigit.Digit.PaddingZero;
                            }


                        }
                    }
                }
                else if (activity.Type == "Sunrise")
                {
                    UserControl_SystemFont_GroupWeather userControl_SystemFont_Group_Weather =
                        userControl_SystemFont_GroupSunriseWidget;
                    UserControl_SystemFont_weather userControl_SystemFont_weather = null;
                    UserControl_FontRotate_weather userControl_FontRotate_weather = null;

                    if (activity.Digits != null && activity.Digits.Count > 0)
                    {
                        foreach (DigitalCommonDigit digitalCommonDigit in activity.Digits)
                        {
                            if (digitalCommonDigit.Type != null && digitalCommonDigit.Type == "Min")
                            {
                                userPanel_text_weather_sunrise = userControl_text_goalWidgetSunrise;
                                userControl_SystemFont_weather =
                                    userControl_SystemFont_Group_Weather.userControl_SystemFont_weather_Min;
                                userControl_FontRotate_weather =
                                    userControl_SystemFont_Group_Weather.userControl_FontRotate_weather_Min;
                            }
                            if (digitalCommonDigit.Type != null && digitalCommonDigit.Type == "Max")
                            {
                                userPanel_text_weather_sunrise = userControl_text_goalWidgetSunset;
                                userControl_SystemFont_weather =
                                    userControl_SystemFont_Group_Weather.userControl_SystemFont_weather_Max;
                                userControl_FontRotate_weather =
                                    userControl_SystemFont_Group_Weather.userControl_FontRotate_weather_Max;
                            }
                            if (digitalCommonDigit.Type == null)
                            {
                                userPanel_text_weather_sunrise = userControl_text_goalWidget;
                                userControl_SystemFont_weather =
                                    userControl_SystemFont_Group_Weather.userControl_SystemFont_weather_Current;
                                userControl_FontRotate_weather =
                                    userControl_SystemFont_Group_Weather.userControl_FontRotate_weather_Current;
                            }


                            // надпиь
                            if (digitalCommonDigit.Digit != null && digitalCommonDigit.Digit.Image != null &&
                                userPanel_text_weather_sunrise != null)
                            {
                                userPanel_text_weather_sunrise.checkBox_Use.Checked = true;
                                NumericUpDown numericUpDownX = userPanel_text_weather_sunrise.numericUpDown_imageX;
                                NumericUpDown numericUpDownY = userPanel_text_weather_sunrise.numericUpDown_imageY;
                                NumericUpDown numericUpDown_unitX = userPanel_text_weather_sunrise.numericUpDown_iconX;
                                NumericUpDown numericUpDown_unitY = userPanel_text_weather_sunrise.numericUpDown_iconY;
                                NumericUpDown numericUpDown_spacing = userPanel_text_weather_sunrise.numericUpDown_spacing;

                                if (digitalCommonDigit.Type != null && digitalCommonDigit.Type == "Max")
                                {
                                    if (digitalCommonDigit.CombingMode == "Single")
                                    {
                                        userPanel_text_weather_sunrise.checkBox_follow.Checked = false;
                                    }
                                    else
                                    {
                                        userPanel_text_weather_sunrise.checkBox_follow.Checked = true;
                                    }
                                }

                                numericUpDownX.Value = digitalCommonDigit.Digit.Image.X;
                                numericUpDownY.Value = digitalCommonDigit.Digit.Image.Y;

                                if (digitalCommonDigit.Digit.Image.NoDataImageIndex != null)
                                    userPanel_text_weather_sunrise.comboBoxSetImageError((int)digitalCommonDigit.Digit.Image.NoDataImageIndex);

                                if (digitalCommonDigit.Digit.Image.DecimalPointImageIndex != null)
                                    userPanel_text_weather_sunrise.comboBoxSetImageDecimalPointOrMinus((int)digitalCommonDigit.Digit.Image.DecimalPointImageIndex);

                                foreach (MultilangImage multilangImage in digitalCommonDigit.Digit.Image.MultilangImage)
                                {
                                    if (multilangImage.LangCode == "All")
                                        userPanel_text_weather_sunrise.comboBoxSetImage((int)multilangImage.ImageSet.ImageIndex);
                                }
                                if (digitalCommonDigit.Digit.Image.MultilangImageUnit != null)
                                {
                                    foreach (MultilangImage multilangImage in digitalCommonDigit.Digit.Image.MultilangImageUnit)
                                    {
                                        if (multilangImage.LangCode == null && userPanel_text_weather_sunrise.comboBoxGetSelectedIndexUnit() < 0)
                                            userPanel_text_weather_sunrise.comboBoxSetUnit((int)multilangImage.ImageSet.ImageIndex);
                                        if (multilangImage.LangCode == "All")
                                            userPanel_text_weather_sunrise.comboBoxSetUnit((int)multilangImage.ImageSet.ImageIndex);
                                    }
                                }
                                userPanel_text_weather_sunrise.comboBoxSetAlignment(digitalCommonDigit.Digit.Alignment);
                                if (digitalCommonDigit.Digit.Spacing != null)
                                    numericUpDown_spacing.Value = (decimal)digitalCommonDigit.Digit.Spacing;

                                userPanel_text_weather_sunrise.checkBox_addZero.Checked = digitalCommonDigit.Digit.PaddingZero;

                                if (digitalCommonDigit.Separator != null)
                                {
                                    userPanel_text_weather_sunrise.comboBoxSetIcon((int)digitalCommonDigit.Separator.ImageIndex);
                                    numericUpDown_unitX.Value = digitalCommonDigit.Separator.Coordinates.X;
                                    numericUpDown_unitY.Value = digitalCommonDigit.Separator.Coordinates.Y;
                                }
                            }

                            // системный шрифт
                            if (digitalCommonDigit.Digit != null && digitalCommonDigit.Digit.SystemFont != null &&
                                digitalCommonDigit.Digit.SystemFont.FontRotate == null && userControl_SystemFont_weather != null)
                            {
                                userControl_SystemFont_weather.checkBox_Use.Checked = true;
                                NumericUpDown numericUpDownX = userControl_SystemFont_weather.numericUpDown_SystemFontX;
                                NumericUpDown numericUpDownY = userControl_SystemFont_weather.numericUpDown_SystemFontY;
                                NumericUpDown numericUpDown_size = userControl_SystemFont_weather.numericUpDown_SystemFont_size;
                                NumericUpDown numericUpDown_angle = userControl_SystemFont_weather.numericUpDown_SystemFont_angle;
                                CheckBox checkBox_add_zero = userControl_SystemFont_weather.checkBox_addZero;
                                NumericUpDown numericUpDown_spacing = userControl_SystemFont_weather.numericUpDown_SystemFont_spacing;
                                CheckBox checkBox_follow = userControl_SystemFont_weather.checkBox_follow;
                                CheckBox checkBox_separator = userControl_SystemFont_weather.checkBox_separator;

                                if (digitalCommonDigit.CombingMode == null ||
                                    digitalCommonDigit.CombingMode == "Follow") checkBox_follow.Checked = true;
                                if (digitalCommonDigit.Separator != null) checkBox_separator.Checked = true;

                                if (digitalCommonDigit.Digit.SystemFont.Coordinates != null)
                                {
                                    numericUpDownX.Value = digitalCommonDigit.Digit.SystemFont.Coordinates.X;
                                    numericUpDownY.Value = digitalCommonDigit.Digit.SystemFont.Coordinates.Y;
                                }
                                numericUpDown_size.Value = digitalCommonDigit.Digit.SystemFont.Size;
                                numericUpDown_angle.Value = digitalCommonDigit.Digit.SystemFont.Angle;
                                if (digitalCommonDigit.Digit.Spacing != null)
                                {
                                    numericUpDown_spacing.Value = (int)digitalCommonDigit.Digit.Spacing;
                                }
                                userControl_SystemFont_weather.comboBoxSetColorString(digitalCommonDigit.Digit.SystemFont.Color);
                                userControl_SystemFont_weather.checkBoxSetUnit((int)digitalCommonDigit.Digit.SystemFont.ShowUnitCheck);
                                checkBox_add_zero.Checked = digitalCommonDigit.Digit.PaddingZero;
                            }


                            // системный шрифт по окружности
                            if (digitalCommonDigit.Digit != null && digitalCommonDigit.Digit.SystemFont != null &&
                                digitalCommonDigit.Digit.SystemFont.FontRotate != null && userControl_FontRotate_weather != null)
                            {
                                userControl_FontRotate_weather.checkBox_Use.Checked = true;
                                NumericUpDown numericUpDownX = userControl_FontRotate_weather.numericUpDown_FontRotateX;
                                NumericUpDown numericUpDownY = userControl_FontRotate_weather.numericUpDown_FontRotateY;
                                NumericUpDown numericUpDown_size = userControl_FontRotate_weather.numericUpDown_FontRotate_size;
                                NumericUpDown numericUpDown_angle = userControl_FontRotate_weather.numericUpDown_FontRotate_angle;
                                NumericUpDown numericUpDown_radius = userControl_FontRotate_weather.numericUpDown_FontRotate_radius;
                                NumericUpDown numericUpDown_spacing = userControl_FontRotate_weather.numericUpDown_FontRotate_spacing;
                                CheckBox checkBox_add_zero = userControl_FontRotate_weather.checkBox_addZero;
                                CheckBox checkBox_follow = userControl_FontRotate_weather.checkBox_follow;
                                CheckBox checkBox_separator = userControl_FontRotate_weather.checkBox_separator;

                                if (digitalCommonDigit.CombingMode == null ||
                                    digitalCommonDigit.CombingMode == "Follow") checkBox_follow.Checked = true;
                                if (digitalCommonDigit.Separator != null) checkBox_separator.Checked = true;

                                if (digitalCommonDigit.Digit.SystemFont.FontRotate != null)
                                {
                                    numericUpDownX.Value = digitalCommonDigit.Digit.SystemFont.FontRotate.X;
                                    numericUpDownY.Value = digitalCommonDigit.Digit.SystemFont.FontRotate.Y;
                                    numericUpDown_radius.Value = digitalCommonDigit.Digit.SystemFont.FontRotate.Radius;
                                    userControl_FontRotate_weather.radioButtonSetRotateDirection(
                                        (int)digitalCommonDigit.Digit.SystemFont.FontRotate.RotateDirection);
                                }
                                numericUpDown_size.Value = digitalCommonDigit.Digit.SystemFont.Size;
                                numericUpDown_angle.Value = digitalCommonDigit.Digit.SystemFont.Angle;
                                if (digitalCommonDigit.Digit.Spacing != null)
                                {
                                    numericUpDown_spacing.Value = (int)digitalCommonDigit.Digit.Spacing;
                                }
                                userControl_FontRotate_weather.comboBoxSetColorString(digitalCommonDigit.Digit.SystemFont.Color);
                                userControl_FontRotate_weather.checkBoxSetUnit((int)digitalCommonDigit.Digit.SystemFont.ShowUnitCheck);
                                checkBox_add_zero.Checked = digitalCommonDigit.Digit.PaddingZero;
                            }


                        }
                    }
                }

                // стрелочный индикатор
                if (userPanel_hand != null)
                {
                    if (activity.PointerProgress != null && activity.PointerProgress.Pointer != null)
                    {
                        userPanel_hand.checkBox_hand_Use.Checked = true;
                        //ComboBox comboBox_image = (ComboBox)panel_hand.Controls[1];
                        NumericUpDown numericUpDownX = userPanel_hand.numericUpDown_handX;
                        NumericUpDown numericUpDownY = userPanel_hand.numericUpDown_handY;
                        NumericUpDown numericUpDown_offsetX = userPanel_hand.numericUpDown_handX_offset;
                        NumericUpDown numericUpDown_offsetY = userPanel_hand.numericUpDown_handY_offset;
                        //ComboBox comboBox_imageCentr = (ComboBox)panel_hand.Controls[6];
                        NumericUpDown numericUpDownX_centr = userPanel_hand.numericUpDown_handX_centr;
                        NumericUpDown numericUpDownY_centr = userPanel_hand.numericUpDown_handY_centr;
                        NumericUpDown numericUpDown_startAngle = userPanel_hand.numericUpDown_hand_startAngle;
                        NumericUpDown numericUpDown_endAngle = userPanel_hand.numericUpDown_hand_endAngle;
                        //ComboBox comboBox_imageBackground = (ComboBox)panel_hand.Controls[11];
                        NumericUpDown numericUpDownX_background = userPanel_hand.numericUpDown_handX_background;
                        NumericUpDown numericUpDownY_background = userPanel_hand.numericUpDown_handY_background;

                        userPanel_hand.comboBoxSetHandImage((int)activity.PointerProgress.Pointer.ImageIndex);
                        numericUpDownX.Value = activity.PointerProgress.X;
                        numericUpDownY.Value = activity.PointerProgress.Y;

                        numericUpDown_startAngle.Value = (decimal)activity.PointerProgress.StartAngle;
                        numericUpDown_endAngle.Value = (decimal)activity.PointerProgress.EndAngle;

                        numericUpDown_offsetX.Value = activity.PointerProgress.Pointer.Coordinates.X;
                        numericUpDown_offsetY.Value = activity.PointerProgress.Pointer.Coordinates.Y;

                        if (activity.PointerProgress.Cover != null)
                        {
                            userPanel_hand.comboBoxSetHandImageCentr((int)activity.PointerProgress.Cover.ImageIndex);
                            numericUpDownX_centr.Value = activity.PointerProgress.Cover.Coordinates.X;
                            numericUpDownY_centr.Value = activity.PointerProgress.Cover.Coordinates.Y;
                        }

                        if (activity.PointerProgress.Scale != null && activity.PointerProgress.Scale.ImageSet != null)
                        {
                            foreach (MultilangImage multilangImage in activity.PointerProgress.Scale.ImageSet)
                            {
                                if (multilangImage.LangCode == "All")
                                    userPanel_hand.comboBoxSetHandImageBackground((int)multilangImage.ImageSet.ImageIndex);
                            }
                            numericUpDownX_background.Value = activity.PointerProgress.Scale.Coordinates.X;
                            numericUpDownY_background.Value = activity.PointerProgress.Scale.Coordinates.Y;
                        }

                    }
                }

                // круговая шкала
                if (userPanel_scaleCircle != null)
                {
                    if (activity.ProgressBar != null && activity.ProgressBar.AngleSettings != null)
                    {
                        userPanel_scaleCircle.checkBox_scaleCircle_Use.Checked = true;
                        RadioButton radioButton_image = userPanel_scaleCircle.radioButton_scaleCircle_image;
                        RadioButton radioButton_color = userPanel_scaleCircle.radioButton_scaleCircle_color;
                        //ComboBox comboBox_image = (ComboBox)panel_scaleCircle.Controls[3];
                        //ComboBox comboBox_color = (ComboBox)panel_scaleCircle.Controls[4];
                        //ComboBox comboBox_flatness = (ComboBox)panel_scaleCircle.Controls[5];
                        //ComboBox comboBox_background = (ComboBox)panel_scaleCircle.Controls[6];
                        NumericUpDown numericUpDownX = userPanel_scaleCircle.numericUpDown_scaleCircleX;
                        NumericUpDown numericUpDownY = userPanel_scaleCircle.numericUpDown_scaleCircleY;
                        NumericUpDown numericUpDown_radius = userPanel_scaleCircle.numericUpDown_scaleCircle_radius;
                        NumericUpDown numericUpDown_width = userPanel_scaleCircle.numericUpDown_scaleCircle_width;
                        NumericUpDown numericUpDown_startAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_startAngle;
                        NumericUpDown numericUpDown_endAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_endAngle;
                        if (activity.ProgressBar.ForegroundImageIndex != null)
                        {
                            radioButton_image.Checked = true;
                            userPanel_scaleCircle.comboBoxSetImage((int)activity.ProgressBar.ForegroundImageIndex);
                        }
                        else
                        {
                            radioButton_color.Checked = true;
                            //Color color = Color.DarkOrange;
                            if (activity.ProgressBar.Color != null)
                            {
                                userPanel_scaleCircle.comboBoxSetColorString(activity.ProgressBar.Color);
                            }
                        }

                        if (activity.ProgressBar.BackgroundImageIndex != null)
                            userPanel_scaleCircle.comboBoxSetImageBackground((int)activity.ProgressBar.BackgroundImageIndex);

                        numericUpDown_width.Value = activity.ProgressBar.Width;

                        numericUpDownX.Value = activity.ProgressBar.AngleSettings.X;
                        numericUpDownY.Value = activity.ProgressBar.AngleSettings.Y;
                        numericUpDown_startAngle.Value = (decimal)activity.ProgressBar.AngleSettings.StartAngle;
                        numericUpDown_endAngle.Value = (decimal)activity.ProgressBar.AngleSettings.EndAngle;
                        numericUpDown_radius.Value = (decimal)activity.ProgressBar.AngleSettings.Radius;

                        userPanel_scaleCircle.comboBoxSetFlatness((int)activity.ProgressBar.Flatness);
                    }
                }

                // линейная шкала
                if (userPanel_scaleLinear != null)
                {
                    if (activity.ProgressBar != null && activity.ProgressBar.LinearSettings != null)
                    {
                        userPanel_scaleLinear.checkBox_scaleLinear_Use.Checked = true;
                        RadioButton radioButton_image = userPanel_scaleLinear.radioButton_scaleLinear_image;
                        RadioButton radioButton_color = userPanel_scaleLinear.radioButton_scaleLinear_color;
                        //ComboBox comboBox_image = (ComboBox)panel_scaleLinear.Controls[3];
                        //ComboBox comboBox_color = (ComboBox)panel_scaleLinear.Controls[4];
                        //ComboBox comboBox_pointer = (ComboBox)panel_scaleLinear.Controls[5];
                        //ComboBox comboBox_background = (ComboBox)panel_scaleLinear.Controls[6];
                        NumericUpDown numericUpDownX = userPanel_scaleLinear.numericUpDown_scaleLinearX;
                        NumericUpDown numericUpDownY = userPanel_scaleLinear.numericUpDown_scaleLinearY;
                        NumericUpDown numericUpDown_length = userPanel_scaleLinear.numericUpDown_scaleLinear_length;
                        NumericUpDown numericUpDown_width = userPanel_scaleLinear.numericUpDown_scaleLinear_width;
                        //ComboBox comboBox_flatness = (ComboBox)panel_scaleLinear.Controls[11];
                        if (activity.ProgressBar.ForegroundImageIndex != null)
                        {
                            radioButton_image.Checked = true;
                            userPanel_scaleLinear.comboBoxSetImage((int)activity.ProgressBar.ForegroundImageIndex);
                        }
                        else
                        {
                            radioButton_color.Checked = true;
                            //Color color = Color.DarkOrange;
                            if (activity.ProgressBar.Color != null)
                            {
                                userPanel_scaleLinear.comboBoxSetColorString(activity.ProgressBar.Color);
                            }
                        }
                        if (activity.ProgressBar.PointerImageIndex != null)
                            userPanel_scaleLinear.comboBoxSetImagePointer((int)activity.ProgressBar.PointerImageIndex);
                        if (activity.ProgressBar.BackgroundImageIndex != null)
                            userPanel_scaleLinear.comboBoxSetImageBackground((int)activity.ProgressBar.BackgroundImageIndex);

                        numericUpDownX.Value = activity.ProgressBar.LinearSettings.StartX;
                        numericUpDownY.Value = activity.ProgressBar.LinearSettings.StartY;
                        long length = activity.ProgressBar.LinearSettings.EndX - activity.ProgressBar.LinearSettings.StartX;
                        numericUpDown_length.Value = length;
                        numericUpDown_width.Value = activity.ProgressBar.Width;

                        userPanel_scaleLinear.comboBoxSetFlatness((int)activity.ProgressBar.Flatness);
                    }
                }

                // иконки
                if (userControl_icon != null)
                {
                    if (activity.Icon != null && activity.Icon.Coordinates != null)

                    {
                        userControl_icon.checkBox_icon_Use.Checked = true;

                        long numericUpDownX = activity.Icon.Coordinates.X;
                        long numericUpDownY = activity.Icon.Coordinates.Y;

                        userControl_icon.comboBoxSetImage(activity.Icon.ImageIndex);
                        if (activity.Icon.ImageIndex2 != null)
                            userControl_icon.comboBoxSetImage2((long)activity.Icon.ImageIndex2);
                        userControl_icon.numericUpDown_iconX.Value = numericUpDownX;
                        userControl_icon.numericUpDown_iconY.Value = numericUpDownY;
                    }
                } 
            }
        }

        private void WidgetDel(int widgetIndex) 
        {
            if (widgetIndex < 0) return;
            if (widgetIndex > WidgetsTemp.Widget.Count-1) return;
            if (WidgetsTemp.Widget.Count == 1) WidgetsTemp = null;
            else WidgetsTemp.Widget.RemoveAt(widgetIndex);
            JSON_write();
            PreviewView = false;
            SettingsClear_Widgets();
            ComboBoxAddItems_Widgets();
            PreviewView = true;
            PreviewImage();
        }

        private void WidgetElementDel(int widgetIndex, int widgetElementIndex)
        {
            if (widgetElementIndex < 0) return;
            if (widgetIndex > WidgetsTemp.Widget.Count - 1) return;
            if (widgetElementIndex > WidgetsTemp.Widget[widgetIndex].WidgetElement.Count - 1) return;
            if (WidgetsTemp.Widget[widgetIndex].WidgetElement.Count == 1)
            {
                WidgetDel(widgetIndex);
                return;
            }
            else WidgetsTemp.Widget[widgetIndex].WidgetElement.RemoveAt(widgetElementIndex);
            JSON_write();
            dataGridView_WidgetElement.Rows.RemoveAt(widgetElementIndex);
            PreviewImage();
        }


    }
}
