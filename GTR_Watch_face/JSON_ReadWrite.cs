using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AmazFit_Watchface_2
{
    public partial class Form1 : Form
    {
        /// <summary>заполняем поля с настройками из JSON файла</summary>
        private void JSON_read()
        {
            SettingsClear();
            checkBoxUseClear();
            ComboBoxAddItems();
            WidgetsTemp = null;
            ScreenIdleTemp = null;

            #region Background
            if (Watch_Face == null) return;
            if (Watch_Face.Info != null) ReadDeviceId();
            if (Watch_Face.Background != null)
            {
                if (Watch_Face.Background.BackgroundImageIndex != null)
                {
                    radioButton_Background_image.Checked = true;
                    comboBoxSetText(comboBox_Background_image, (long)Watch_Face.Background.BackgroundImageIndex);
                }
                    
                if (Watch_Face.Background.Preview != null && Watch_Face.Background.Preview.Count > 0)
                {
                    //comboBox_Preview.Text comboBox_Preview Watch_Face.Background.Preview.ImageIndex.ToString();
                    //comboBoxSetText(comboBox_Preview_image, Watch_Face.Background.Preview[0].ImageSet.ImageIndex);
                    foreach (MultilangImage multilangImage in Watch_Face.Background.Preview)
                    {
                        if (multilangImage.LangCode == "All") comboBoxSetText(comboBox_Preview_image, multilangImage.ImageSet.ImageIndex);
                    }
                }

                if (Watch_Face.Background.Color != null && Watch_Face.Background.Color.Length > 6)
                {
                    radioButton_Background_color.Checked = true;
                    comboBox_Background_color.BackColor = ColorBackgroundRead(Watch_Face.Background.Color);
                }
            }
            #endregion

            #region цифровое время
            if (Watch_Face.DialFace != null && Watch_Face.DialFace.DigitalDialFace != null)
            {
                if (Watch_Face.DialFace.DigitalDialFace.Digits != null && 
                    Watch_Face.DialFace.DigitalDialFace.Digits.Count > 0)
                {
                    foreach (DigitalTimeDigit digitalTimeDigit in Watch_Face.DialFace.DigitalDialFace.Digits)
                    {
                        switch (digitalTimeDigit.TimeType)
                        {
                            case "Minute":
                                checkBox_Minute_Use.Checked = true;
                                if (digitalTimeDigit.CombingMode == "Single")
                                {
                                    checkBox_Minute_follow.Checked = false;
                                }
                                else
                                {
                                    checkBox_Minute_follow.Checked = true;
                                }

                                if (digitalTimeDigit.Digit != null)
                                {
                                    if (digitalTimeDigit.Digit.Image != null)
                                    {
                                        numericUpDown_MinuteX.Value = digitalTimeDigit.Digit.Image.X;
                                        numericUpDown_MinuteY.Value = digitalTimeDigit.Digit.Image.Y;
                                        foreach (MultilangImage multilangImage in digitalTimeDigit.Digit.Image.MultilangImage)
                                        {
                                            if (multilangImage.LangCode == "All") 
                                                comboBoxSetText(comboBox_Minute_image, multilangImage.ImageSet.ImageIndex);
                                        }
                                        if (digitalTimeDigit.Digit.Image.MultilangImageUnit != null)
                                        {
                                            foreach (MultilangImage multilangImage in digitalTimeDigit.Digit.Image.MultilangImageUnit)
                                            {
                                                if (multilangImage.LangCode == "All")
                                                    comboBoxSetText(comboBox_Minute_separator, multilangImage.ImageSet.ImageIndex);
                                            } 
                                        }
                                    }
                                    Alignment2ToString(comboBox_Minute_alignment, digitalTimeDigit.Digit.Alignment);
                                    if (digitalTimeDigit.Digit.Spacing != null)
                                        numericUpDown_Minute_spacing.Value = (decimal)digitalTimeDigit.Digit.Spacing;
                                    checkBox_Minute_add_zero.Checked = digitalTimeDigit.Digit.PaddingZero;
                                }

                                if (digitalTimeDigit.Separator != null)
                                {
                                    comboBoxSetText(comboBox_Minute_unit, digitalTimeDigit.Separator.ImageIndex);
                                    numericUpDown_Minute_unitX.Value = digitalTimeDigit.Separator.Coordinates.X;
                                    numericUpDown_Minute_unitY.Value = digitalTimeDigit.Separator.Coordinates.Y;
                                }
                                break;

                            case "Second":
                                checkBox_Second_Use.Checked = true;
                                if (digitalTimeDigit.CombingMode == "Single")
                                {
                                    checkBox_Second_follow.Checked = false;
                                }
                                else
                                {
                                    checkBox_Second_follow.Checked = true;
                                }

                                if (digitalTimeDigit.Digit != null)
                                {
                                    if (digitalTimeDigit.Digit.Image != null)
                                    {
                                        numericUpDown_SecondX.Value = digitalTimeDigit.Digit.Image.X;
                                        numericUpDown_SecondY.Value = digitalTimeDigit.Digit.Image.Y;
                                        foreach (MultilangImage multilangImage in digitalTimeDigit.Digit.Image.MultilangImage)
                                        {
                                            if (multilangImage.LangCode == "All")
                                                comboBoxSetText(comboBox_Second_image, multilangImage.ImageSet.ImageIndex);
                                        }
                                        if (digitalTimeDigit.Digit.Image.MultilangImageUnit != null)
                                        {
                                            foreach (MultilangImage multilangImage in digitalTimeDigit.Digit.Image.MultilangImageUnit)
                                            {
                                                if (multilangImage.LangCode == "All")
                                                    comboBoxSetText(comboBox_Second_separator, multilangImage.ImageSet.ImageIndex);
                                            } 
                                        }
                                    }
                                    Alignment2ToString(comboBox_Second_alignment, digitalTimeDigit.Digit.Alignment);
                                    if (digitalTimeDigit.Digit.Spacing != null)
                                        numericUpDown_Second_spacing.Value = (decimal)digitalTimeDigit.Digit.Spacing;
                                    checkBox_Second_add_zero.Checked = digitalTimeDigit.Digit.PaddingZero;
                                }

                                if (digitalTimeDigit.Separator != null)
                                {
                                    comboBoxSetText(comboBox_Second_unit, digitalTimeDigit.Separator.ImageIndex);
                                    numericUpDown_Second_unitX.Value = digitalTimeDigit.Separator.Coordinates.X;
                                    numericUpDown_Second_unitY.Value = digitalTimeDigit.Separator.Coordinates.Y;
                                }
                                break;

                            default:
                                checkBox_Hour_Use.Checked = true;

                                if (digitalTimeDigit.Digit != null)
                                {
                                    if (digitalTimeDigit.Digit.Image != null)
                                    {
                                        numericUpDown_HourX.Value = digitalTimeDigit.Digit.Image.X;
                                        numericUpDown_HourY.Value = digitalTimeDigit.Digit.Image.Y;
                                        foreach (MultilangImage multilangImage in digitalTimeDigit.Digit.Image.MultilangImage)
                                        {
                                            if (multilangImage.LangCode == "All")
                                                comboBoxSetText(comboBox_Hour_image, multilangImage.ImageSet.ImageIndex);
                                        }
                                        if (digitalTimeDigit.Digit.Image.MultilangImageUnit != null)
                                        {
                                            foreach (MultilangImage multilangImage in digitalTimeDigit.Digit.Image.MultilangImageUnit)
                                            {
                                                if (multilangImage.LangCode == "All")
                                                    comboBoxSetText(comboBox_Hour_separator, multilangImage.ImageSet.ImageIndex);
                                            } 
                                        }
                                    }
                                    Alignment2ToString(comboBox_Hour_alignment, digitalTimeDigit.Digit.Alignment);
                                    if (digitalTimeDigit.Digit.Spacing != null)
                                        numericUpDown_Hour_spacing.Value = (decimal)digitalTimeDigit.Digit.Spacing;
                                    checkBox_Hour_add_zero.Checked = digitalTimeDigit.Digit.PaddingZero;
                                }

                                if (digitalTimeDigit.Separator != null)
                                {
                                    comboBoxSetText(comboBox_Hour_unit, digitalTimeDigit.Separator.ImageIndex);
                                    numericUpDown_Hour_unitX.Value = digitalTimeDigit.Separator.Coordinates.X;
                                    numericUpDown_Hour_unitY.Value = digitalTimeDigit.Separator.Coordinates.Y;
                                }
                                break;
                        }


                    }
                }
                
                if (Watch_Face.DialFace.DigitalDialFace.AM != null &&
                    Watch_Face.DialFace.DigitalDialFace.AM.Coordinates != null)
                {
                    checkBox_12h_Use.Checked = true;
                    numericUpDown_AM_X.Value = Watch_Face.DialFace.DigitalDialFace.AM.Coordinates.X;
                    numericUpDown_AM_Y.Value = Watch_Face.DialFace.DigitalDialFace.AM.Coordinates.Y;
                    foreach (MultilangImage multilangImage in Watch_Face.DialFace.DigitalDialFace.AM.ImageSet)
                    {
                        if (multilangImage.LangCode != null || multilangImage.LangCode == "All")
                            comboBoxSetText(comboBox_AM_image, multilangImage.ImageSet.ImageIndex);
                    }
                }
                if (Watch_Face.DialFace.DigitalDialFace.PM != null &&
                    Watch_Face.DialFace.DigitalDialFace.PM.Coordinates != null)
                {
                    checkBox_12h_Use.Checked = true;
                    numericUpDown_PM_X.Value = Watch_Face.DialFace.DigitalDialFace.PM.Coordinates.X;
                    numericUpDown_PM_Y.Value = Watch_Face.DialFace.DigitalDialFace.PM.Coordinates.Y;
                    foreach (MultilangImage multilangImage in Watch_Face.DialFace.DigitalDialFace.PM.ImageSet)
                    {
                        if (multilangImage.LangCode != null || multilangImage.LangCode == "All")
                            comboBoxSetText(comboBox_PM_image, multilangImage.ImageSet.ImageIndex);
                    }
                }
            }
            #endregion

            #region аналоговое время
            if (Watch_Face.DialFace != null && Watch_Face.DialFace.AnalogDialFace != null)
            {
                // часы
                if(Watch_Face.DialFace.AnalogDialFace.Hours != null)
                {
                    checkBox_Hour_hand_Use.Checked = true;
                    numericUpDown_Hour_handX.Value = Watch_Face.DialFace.AnalogDialFace.Hours.X;
                    numericUpDown_Hour_handY.Value = Watch_Face.DialFace.AnalogDialFace.Hours.Y;
                    if(Watch_Face.DialFace.AnalogDialFace.Hours.Pointer != null)
                    {
                        comboBoxSetText(comboBox_Hour_hand_image, (long)Watch_Face.DialFace.AnalogDialFace.Hours.Pointer.ImageIndex);
                        if (Watch_Face.DialFace.AnalogDialFace.Hours.Pointer.Coordinates != null)
                        {
                            numericUpDown_Hour_handX_offset.Value = Watch_Face.DialFace.AnalogDialFace.Hours.Pointer.Coordinates.X;
                            numericUpDown_Hour_handY_offset.Value = Watch_Face.DialFace.AnalogDialFace.Hours.Pointer.Coordinates.Y;
                        }
                    }

                    if (Watch_Face.DialFace.AnalogDialFace.Hours.Cover != null)
                    {
                        comboBoxSetText(comboBox_Hour_hand_imageCentr, (long)Watch_Face.DialFace.AnalogDialFace.Hours.Cover.ImageIndex);
                        if (Watch_Face.DialFace.AnalogDialFace.Hours.Pointer.Coordinates != null)
                        {
                            numericUpDown_Hour_handX_centr.Value = Watch_Face.DialFace.AnalogDialFace.Hours.Cover.Coordinates.X;
                            numericUpDown_Hour_handY_centr.Value = Watch_Face.DialFace.AnalogDialFace.Hours.Cover.Coordinates.Y;
                        }
                    }
                }

                // минуты
                if (Watch_Face.DialFace.AnalogDialFace.Minutes != null)
                {
                    checkBox_Minute_hand_Use.Checked = true;
                    numericUpDown_Minute_handX.Value = Watch_Face.DialFace.AnalogDialFace.Minutes.X;
                    numericUpDown_Minute_handY.Value = Watch_Face.DialFace.AnalogDialFace.Minutes.Y;
                    if (Watch_Face.DialFace.AnalogDialFace.Minutes.Pointer != null)
                    {
                        comboBoxSetText(comboBox_Minute_hand_image, (long)Watch_Face.DialFace.AnalogDialFace.Minutes.Pointer.ImageIndex);
                        if (Watch_Face.DialFace.AnalogDialFace.Minutes.Pointer.Coordinates != null)
                        {
                            numericUpDown_Minute_handX_offset.Value = Watch_Face.DialFace.AnalogDialFace.Minutes.Pointer.Coordinates.X;
                            numericUpDown_Minute_handY_offset.Value = Watch_Face.DialFace.AnalogDialFace.Minutes.Pointer.Coordinates.Y;
                        }
                    }

                    if (Watch_Face.DialFace.AnalogDialFace.Minutes.Cover != null)
                    {
                        comboBoxSetText(comboBox_Minute_hand_imageCentr, (long)Watch_Face.DialFace.AnalogDialFace.Minutes.Cover.ImageIndex);
                        if (Watch_Face.DialFace.AnalogDialFace.Minutes.Pointer.Coordinates != null)
                        {
                            numericUpDown_Minute_handX_centr.Value = Watch_Face.DialFace.AnalogDialFace.Minutes.Cover.Coordinates.X;
                            numericUpDown_Minute_handY_centr.Value = Watch_Face.DialFace.AnalogDialFace.Minutes.Cover.Coordinates.Y;
                        }
                    }
                }
                
                // секунды
                if (Watch_Face.DialFace.AnalogDialFace.Seconds != null)
                {
                    checkBox_Second_hand_Use.Checked = true;
                    numericUpDown_Second_handX.Value = Watch_Face.DialFace.AnalogDialFace.Seconds.X;
                    numericUpDown_Second_handY.Value = Watch_Face.DialFace.AnalogDialFace.Seconds.Y;
                    if (Watch_Face.DialFace.AnalogDialFace.Seconds.Pointer != null)
                    {
                        comboBoxSetText(comboBox_Second_hand_image, (long)Watch_Face.DialFace.AnalogDialFace.Seconds.Pointer.ImageIndex);
                        if (Watch_Face.DialFace.AnalogDialFace.Seconds.Pointer.Coordinates != null)
                        {
                            numericUpDown_Second_handX_offset.Value = Watch_Face.DialFace.AnalogDialFace.Seconds.Pointer.Coordinates.X;
                            numericUpDown_Second_handY_offset.Value = Watch_Face.DialFace.AnalogDialFace.Seconds.Pointer.Coordinates.Y;
                        }
                    }

                    if (Watch_Face.DialFace.AnalogDialFace.Seconds.Cover != null)
                    {
                        comboBoxSetText(comboBox_Second_hand_imageCentr, (long)Watch_Face.DialFace.AnalogDialFace.Seconds.Cover.ImageIndex);
                        if (Watch_Face.DialFace.AnalogDialFace.Seconds.Pointer.Coordinates != null)
                        {
                            numericUpDown_Second_handX_centr.Value = Watch_Face.DialFace.AnalogDialFace.Seconds.Cover.Coordinates.X;
                            numericUpDown_Second_handY_centr.Value = Watch_Face.DialFace.AnalogDialFace.Seconds.Cover.Coordinates.Y;
                        }
                    }
                }
            }
            #endregion

            #region дата
            if (Watch_Face.System != null && Watch_Face.System.Date != null)
            {

                if (Watch_Face.System.Date.DateDigits != null &&
                    Watch_Face.System.Date.DateDigits.Count > 0)
                {
                    foreach (DigitalDateDigit digitalDateDigit in Watch_Face.System.Date.DateDigits)
                    {
                        switch (digitalDateDigit.DateType)
                        {
                            case "Day":
                                checkBox_Day_Use.Checked = true;
                                if (digitalDateDigit.CombingMode == "Single")
                                {
                                    checkBox_Day_follow.Checked = false;
                                }
                                else
                                {
                                    checkBox_Day_follow.Checked = true;
                                }

                                if (digitalDateDigit.Digit != null)
                                {
                                    if (digitalDateDigit.Digit.Image != null)
                                    {
                                        numericUpDown_DayX.Value = digitalDateDigit.Digit.Image.X;
                                        numericUpDown_DayY.Value = digitalDateDigit.Digit.Image.Y;
                                        foreach (MultilangImage multilangImage in digitalDateDigit.Digit.Image.MultilangImage)
                                        {
                                            if (multilangImage.LangCode == "All")
                                                comboBoxSetText(comboBox_Day_image, multilangImage.ImageSet.ImageIndex);
                                        }
                                        if (digitalDateDigit.Digit.Image.MultilangImageUnit != null)
                                        {
                                            foreach (MultilangImage multilangImage in digitalDateDigit.Digit.Image.MultilangImageUnit)
                                            {
                                                if (multilangImage.LangCode == "All")
                                                    comboBoxSetText(comboBox_Day_separator, multilangImage.ImageSet.ImageIndex);
                                            }
                                        }
                                    }
                                    Alignment2ToString(comboBox_Day_alignment, digitalDateDigit.Digit.Alignment);
                                    if (digitalDateDigit.Digit.Spacing != null)
                                        numericUpDown_Day_spacing.Value = (decimal)digitalDateDigit.Digit.Spacing;
                                    checkBox_Day_add_zero.Checked = digitalDateDigit.Digit.PaddingZero;
                                }

                                if (digitalDateDigit.Separator != null)
                                {
                                    comboBoxSetText(comboBox_Day_unit, digitalDateDigit.Separator.ImageIndex);
                                    numericUpDown_Day_unitX.Value = digitalDateDigit.Separator.Coordinates.X;
                                    numericUpDown_Day_unitY.Value = digitalDateDigit.Separator.Coordinates.Y;
                                }
                                break;

                            case "Month":
                                if (digitalDateDigit.Digit != null && digitalDateDigit.Digit.DisplayFormAnalog)
                                {
                                    checkBox_Month_pictures_Use.Checked = true;
                                    if (digitalDateDigit.Digit != null)
                                    {
                                        if (digitalDateDigit.Digit.Image != null)
                                        {
                                            numericUpDown_Month_picturesX.Value = digitalDateDigit.Digit.Image.X;
                                            numericUpDown_Month_picturesY.Value = digitalDateDigit.Digit.Image.Y;
                                            foreach (MultilangImage multilangImage in digitalDateDigit.Digit.Image.MultilangImage)
                                            {
                                                if (multilangImage.LangCode == "All")
                                                    comboBoxSetText(comboBox_Month_pictures_image, multilangImage.ImageSet.ImageIndex);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    checkBox_Month_Use.Checked = true;
                                    if (digitalDateDigit.CombingMode == "Single")
                                    {
                                        checkBox_Month_follow.Checked = false;
                                    }
                                    else
                                    {
                                        checkBox_Month_follow.Checked = true;
                                    }

                                    if (digitalDateDigit.Digit != null)
                                    {
                                        if (digitalDateDigit.Digit.Image != null)
                                        {
                                            numericUpDown_MonthX.Value = digitalDateDigit.Digit.Image.X;
                                            numericUpDown_MonthY.Value = digitalDateDigit.Digit.Image.Y;
                                            foreach (MultilangImage multilangImage in digitalDateDigit.Digit.Image.MultilangImage)
                                            {
                                                if (multilangImage.LangCode == "All")
                                                    comboBoxSetText(comboBox_Month_image, multilangImage.ImageSet.ImageIndex);
                                            }
                                            if (digitalDateDigit.Digit.Image.MultilangImageUnit != null)
                                            {
                                                foreach (MultilangImage multilangImage in digitalDateDigit.Digit.Image.MultilangImageUnit)
                                                {
                                                    if (multilangImage.LangCode == "All")
                                                        comboBoxSetText(comboBox_Month_separator, multilangImage.ImageSet.ImageIndex);
                                                }
                                            }
                                        }
                                        Alignment2ToString(comboBox_Month_alignment, digitalDateDigit.Digit.Alignment);
                                        if (digitalDateDigit.Digit.Spacing != null)
                                            numericUpDown_Month_spacing.Value = (decimal)digitalDateDigit.Digit.Spacing;
                                        checkBox_Month_add_zero.Checked = digitalDateDigit.Digit.PaddingZero;
                                    }

                                    if (digitalDateDigit.Separator != null)
                                    {
                                        comboBoxSetText(comboBox_Month_unit, digitalDateDigit.Separator.ImageIndex);
                                        numericUpDown_Month_unitX.Value = digitalDateDigit.Separator.Coordinates.X;
                                        numericUpDown_Month_unitY.Value = digitalDateDigit.Separator.Coordinates.Y;
                                    } 
                                }
                                break;

                            default:
                                checkBox__Year_text_Use.Checked = true;
                                
                                if (digitalDateDigit.Digit != null)
                                {
                                    if (digitalDateDigit.Digit.Image != null)
                                    {
                                        numericUpDown_YearX.Value = digitalDateDigit.Digit.Image.X;
                                        numericUpDown_YearY.Value = digitalDateDigit.Digit.Image.Y;
                                        foreach (MultilangImage multilangImage in digitalDateDigit.Digit.Image.MultilangImage)
                                        {
                                            if (multilangImage.LangCode == "All")
                                                comboBoxSetText(comboBox_Year_image, multilangImage.ImageSet.ImageIndex);
                                        }
                                        if (digitalDateDigit.Digit.Image.MultilangImageUnit != null)
                                        {
                                            foreach (MultilangImage multilangImage in digitalDateDigit.Digit.Image.MultilangImageUnit)
                                            {
                                                if (multilangImage.LangCode == "All")
                                                    comboBoxSetText(comboBox_Year_separator, multilangImage.ImageSet.ImageIndex);
                                            }
                                        }
                                    }
                                    Alignment2ToString(comboBox_Year_alignment, digitalDateDigit.Digit.Alignment);
                                    if (digitalDateDigit.Digit.Spacing != null)
                                        numericUpDown_Year_spacing.Value = (decimal)digitalDateDigit.Digit.Spacing;
                                    checkBox_Year_add_zero.Checked = digitalDateDigit.Digit.PaddingZero;
                                }

                                if (digitalDateDigit.Separator != null)
                                {
                                    comboBoxSetText(comboBox_Year_unit, digitalDateDigit.Separator.ImageIndex);
                                    numericUpDown_Year_unitX.Value = digitalDateDigit.Separator.Coordinates.X;
                                    numericUpDown_Year_unitY.Value = digitalDateDigit.Separator.Coordinates.Y;
                                }
                                break;
                        }

                    }
                }

                // дата стрелкой
                if (Watch_Face.System.Date.DateClockHand != null && Watch_Face.System.Date.DateClockHand.DayClockHand != null)
                {
                    checkBox_Day_hand_Use.Checked = true;
                    numericUpDown_Day_handX.Value = Watch_Face.System.Date.DateClockHand.DayClockHand.X;
                    numericUpDown_Day_handY.Value = Watch_Face.System.Date.DateClockHand.DayClockHand.Y;
                    numericUpDown_Day_hand_startAngle.Value =
                        (decimal)Watch_Face.System.Date.DateClockHand.DayClockHand.StartAngle;
                    numericUpDown_Day_hand_endAngle.Value =
                        (decimal)Watch_Face.System.Date.DateClockHand.DayClockHand.EndAngle;
                    if (Watch_Face.System.Date.DateClockHand.DayClockHand.Pointer != null)
                    {
                        comboBoxSetText(comboBox_Day_hand_image,
                            (long)Watch_Face.System.Date.DateClockHand.DayClockHand.Pointer.ImageIndex);
                        if (Watch_Face.System.Date.DateClockHand.DayClockHand.Pointer.Coordinates != null)
                        {
                            numericUpDown_Day_handX_offset.Value =
                                Watch_Face.System.Date.DateClockHand.DayClockHand.Pointer.Coordinates.X;
                            numericUpDown_Day_handY_offset.Value =
                                Watch_Face.System.Date.DateClockHand.DayClockHand.Pointer.Coordinates.Y;
                        }
                    }

                    // центральное изображение
                    if (Watch_Face.System.Date.DateClockHand.DayClockHand.Cover != null)
                    {
                        comboBoxSetText(comboBox_Day_hand_imageCentr,
                            (long)Watch_Face.System.Date.DateClockHand.DayClockHand.Cover.ImageIndex);
                        if (Watch_Face.System.Date.DateClockHand.DayClockHand.Pointer.Coordinates != null)
                        {
                            numericUpDown_Day_handX_centr.Value =
                                Watch_Face.System.Date.DateClockHand.DayClockHand.Cover.Coordinates.X;
                            numericUpDown_Day_handY_centr.Value =
                                Watch_Face.System.Date.DateClockHand.DayClockHand.Cover.Coordinates.Y;
                        }
                    }

                    // фон
                    if (Watch_Face.System.Date.DateClockHand.DayClockHand.Scale != null &&
                        Watch_Face.System.Date.DateClockHand.DayClockHand.Scale.ImageSet != null)
                    {
                        foreach (MultilangImage multilangImage in Watch_Face.System.Date.DateClockHand.DayClockHand.Scale.ImageSet)
                        {
                            if (multilangImage.LangCode == "All")
                                comboBoxSetText(comboBox_Day_hand_imageBackground, multilangImage.ImageSet.ImageIndex);
                        }
                        if (Watch_Face.System.Date.DateClockHand.DayClockHand.Scale.Coordinates != null)
                        {
                            numericUpDown_Day_handX_background.Value =
                                Watch_Face.System.Date.DateClockHand.DayClockHand.Scale.Coordinates.X;
                            numericUpDown_Day_handY_background.Value =
                                Watch_Face.System.Date.DateClockHand.DayClockHand.Scale.Coordinates.Y;
                        }
                    }
                }

                // месяц стрелкой
                if (Watch_Face.System.Date.DateClockHand != null && Watch_Face.System.Date.DateClockHand.MonthClockHand != null)
                {
                    checkBox_Month_hand_Use.Checked = true;
                    numericUpDown_Month_handX.Value = Watch_Face.System.Date.DateClockHand.MonthClockHand.X;
                    numericUpDown_Month_handY.Value = Watch_Face.System.Date.DateClockHand.MonthClockHand.Y;
                    numericUpDown_Month_hand_startAngle.Value = 
                        (decimal)Watch_Face.System.Date.DateClockHand.MonthClockHand.StartAngle;
                    numericUpDown_Month_hand_endAngle.Value =
                        (decimal)Watch_Face.System.Date.DateClockHand.MonthClockHand.EndAngle;
                    if (Watch_Face.System.Date.DateClockHand.MonthClockHand.Pointer != null)
                    {
                        comboBoxSetText(comboBox_Month_hand_image, 
                            (long)Watch_Face.System.Date.DateClockHand.MonthClockHand.Pointer.ImageIndex);
                        if (Watch_Face.System.Date.DateClockHand.MonthClockHand.Pointer.Coordinates != null)
                        {
                            numericUpDown_Month_handX_offset.Value = 
                                Watch_Face.System.Date.DateClockHand.MonthClockHand.Pointer.Coordinates.X;
                            numericUpDown_Month_handY_offset.Value = 
                                Watch_Face.System.Date.DateClockHand.MonthClockHand.Pointer.Coordinates.Y;
                        }
                    }

                    // центральное изображение
                    if (Watch_Face.System.Date.DateClockHand.MonthClockHand.Cover != null)
                    {
                        comboBoxSetText(comboBox_Month_hand_imageCentr, 
                            (long)Watch_Face.System.Date.DateClockHand.MonthClockHand.Cover.ImageIndex);
                        if (Watch_Face.System.Date.DateClockHand.MonthClockHand.Pointer.Coordinates != null)
                        {
                            numericUpDown_Month_handX_centr.Value = 
                                Watch_Face.System.Date.DateClockHand.MonthClockHand.Cover.Coordinates.X;
                            numericUpDown_Month_handY_centr.Value = 
                                Watch_Face.System.Date.DateClockHand.MonthClockHand.Cover.Coordinates.Y;
                        }
                    }
                   
                    // фон
                    if (Watch_Face.System.Date.DateClockHand.MonthClockHand.Scale != null &&
                        Watch_Face.System.Date.DateClockHand.MonthClockHand.Scale.ImageSet != null)
                    {
                        foreach (MultilangImage multilangImage in Watch_Face.System.Date.DateClockHand.MonthClockHand.Scale.ImageSet)
                        {
                            if (multilangImage.LangCode == "All")
                                comboBoxSetText(comboBox_Month_hand_imageBackground, multilangImage.ImageSet.ImageIndex);
                        }
                        if (Watch_Face.System.Date.DateClockHand.MonthClockHand.Scale.Coordinates != null)
                        {
                            numericUpDown_Month_handX_background.Value =
                                Watch_Face.System.Date.DateClockHand.MonthClockHand.Scale.Coordinates.X;
                            numericUpDown_Month_handY_background.Value =
                                Watch_Face.System.Date.DateClockHand.MonthClockHand.Scale.Coordinates.Y;
                        }
                    }
                }

                // день недели стрелкой
                if (Watch_Face.System.Date.DateClockHand != null && Watch_Face.System.Date.DateClockHand.WeekDayClockHand != null)
                {
                    checkBox_DOW_hand_Use.Checked = true;
                    numericUpDown_DOW_handX.Value = Watch_Face.System.Date.DateClockHand.WeekDayClockHand.X;
                    numericUpDown_DOW_handY.Value = Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Y;
                    numericUpDown_DOW_hand_startAngle.Value =
                        (decimal)Watch_Face.System.Date.DateClockHand.WeekDayClockHand.StartAngle;
                    numericUpDown_DOW_hand_endAngle.Value =
                        (decimal)Watch_Face.System.Date.DateClockHand.WeekDayClockHand.EndAngle;
                    if (Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Pointer != null)
                    {
                        comboBoxSetText(comboBox_DOW_hand_image,
                            (long)Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Pointer.ImageIndex);
                        if (Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Pointer.Coordinates != null)
                        {
                            numericUpDown_DOW_handX_offset.Value =
                                Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Pointer.Coordinates.X;
                            numericUpDown_DOW_handY_offset.Value =
                                Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Pointer.Coordinates.Y;
                        }
                    }

                    // центральное изображение
                    if (Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Cover != null)
                    {
                        comboBoxSetText(comboBox_DOW_hand_imageCentr,
                            (long)Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Cover.ImageIndex);
                        if (Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Pointer.Coordinates != null)
                        {
                            numericUpDown_DOW_handX_centr.Value =
                                Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Cover.Coordinates.X;
                            numericUpDown_DOW_handY_centr.Value =
                                Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Cover.Coordinates.Y;
                        }
                    }

                    // фон
                    if (Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Scale != null && 
                        Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Scale.ImageSet != null)
                    {
                        foreach (MultilangImage multilangImage in Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Scale.ImageSet)
                        {
                            if (multilangImage.LangCode == "All")
                                comboBoxSetText(comboBox_DOW_hand_imageBackground, multilangImage.ImageSet.ImageIndex);
                        }
                        if (Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Scale.Coordinates != null)
                        {
                            numericUpDown_DOW_handX_background.Value =
                                Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Scale.Coordinates.X;
                            numericUpDown_DOW_handY_background.Value =
                                Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Scale.Coordinates.Y;
                        }
                    }
                }

                // день недели картинкой
                if (Watch_Face.System.Date.WeeksDigits != null)
                {
                    checkBox_DOW_pictures_Use.Checked = true;
                    if (Watch_Face.System.Date.WeeksDigits.Digit != null && 
                        Watch_Face.System.Date.WeeksDigits.Digit.DisplayFormAnalog)
                    {
                        if (Watch_Face.System.Date.WeeksDigits.Digit.Image != null)
                        {
                            numericUpDown_DOW_picturesX.Value = Watch_Face.System.Date.WeeksDigits.Digit.Image.X;
                            numericUpDown_DOW_picturesY.Value = Watch_Face.System.Date.WeeksDigits.Digit.Image.Y;
                            foreach (MultilangImage multilangImage in Watch_Face.System.Date.WeeksDigits.Digit.Image.MultilangImage)
                            {
                                if (multilangImage.LangCode == "All")
                                    comboBoxSetText(comboBox_DOW_pictures_image, multilangImage.ImageSet.ImageIndex);
                            }
                        }
                    }
                }


            }
            #endregion

            #region статусы
            if (Watch_Face.System != null && Watch_Face.System.Status != null)
            {
                if(Watch_Face.System.Status.Alarm != null)
                {
                    checkBox_Alarm_Use.Checked = true;
                    comboBoxSetText(comboBox_Alarm_image, Watch_Face.System.Status.Alarm.ImageIndex);
                    if(Watch_Face.System.Status.Alarm.Coordinates != null)
                    {
                        numericUpDown_AlarmX.Value = Watch_Face.System.Status.Alarm.Coordinates.X;
                        numericUpDown_AlarmY.Value = Watch_Face.System.Status.Alarm.Coordinates.Y;
                    }
                }
                if (Watch_Face.System.Status.Bluetooth != null)
                {
                    checkBox_Bluetooth_Use.Checked = true;
                    comboBoxSetText(comboBox_Bluetooth_image, Watch_Face.System.Status.Bluetooth.ImageIndex);
                    if (Watch_Face.System.Status.Bluetooth.Coordinates != null)
                    {
                        numericUpDown_BluetoothX.Value = Watch_Face.System.Status.Bluetooth.Coordinates.X;
                        numericUpDown_BluetoothY.Value = Watch_Face.System.Status.Bluetooth.Coordinates.Y;
                    }
                }
                if (Watch_Face.System.Status.DoNotDisturb != null)
                {
                    checkBox_DND_Use.Checked = true;
                    comboBoxSetText(comboBox_DND_image, Watch_Face.System.Status.DoNotDisturb.ImageIndex);
                    if (Watch_Face.System.Status.DoNotDisturb.Coordinates != null)
                    {
                        numericUpDown_DNDX.Value = Watch_Face.System.Status.DoNotDisturb.Coordinates.X;
                        numericUpDown_DNDY.Value = Watch_Face.System.Status.DoNotDisturb.Coordinates.Y;
                    }
                }
                if (Watch_Face.System.Status.Lock != null)
                {
                    checkBox_Lock_Use.Checked = true;
                    comboBoxSetText(comboBox_Lock_image, Watch_Face.System.Status.Lock.ImageIndex);
                    if (Watch_Face.System.Status.Lock.Coordinates != null)
                    {
                        numericUpDown_LockX.Value = Watch_Face.System.Status.Lock.Coordinates.X;
                        numericUpDown_LockY.Value = Watch_Face.System.Status.Lock.Coordinates.Y;
                    }
                }
            }
            #endregion

            #region активности 
            if (Watch_Face.System != null && Watch_Face.System.Activity != null)
            {
                foreach (Activity activity in Watch_Face.System.Activity)
                {
                    Panel panel_pictures = null;
                    Panel panel_text = null;
                    Panel panel_hand = null;
                    Panel panel_scaleCircle = null;
                    Panel panel_scaleLinear = null;
                    CheckBox checkBox_Use;
                    switch (activity.Type)
                    {
                        case "Battery":
                            panel_pictures = panel_Battery_pictures;
                            panel_text = panel_Battery_text;
                            panel_hand = panel_Battery_hand;
                            panel_scaleCircle = panel_Battery_scaleCircle;
                            panel_scaleLinear = panel_Battery_scaleLinear;
                            break;
                        case "Steps":
                            panel_pictures = panel_Steps_pictures;
                            panel_text = panel_Steps_text;
                            panel_hand = panel_Steps_hand;
                            panel_scaleCircle = panel_Steps_scaleCircle;
                            panel_scaleLinear = panel_Steps_scaleLinear;
                            break;
                        case "Calories":
                            panel_pictures = panel_Calories_pictures;
                            panel_text = panel_Calories_text;
                            panel_hand = panel_Calories_hand;
                            panel_scaleCircle = panel_Calories_scaleCircle;
                            panel_scaleLinear = panel_Calories_scaleLinear;
                            break;
                        case "HeartRate":
                            panel_pictures = panel_HeartRate_pictures;
                            panel_text = panel_HeartRate_text;
                            panel_hand = panel_HeartRate_hand;
                            panel_scaleCircle = panel_HeartRate_scaleCircle;
                            panel_scaleLinear = panel_HeartRate_scaleLinear;
                            break;
                        case "PAI":
                            panel_pictures = panel_PAI_pictures;
                            panel_text = panel_PAI_text;
                            panel_hand = panel_PAI_hand;
                            panel_scaleCircle = panel_PAI_scaleCircle;
                            panel_scaleLinear = panel_PAI_scaleLinear;
                            break;
                        case "Distance":
                            panel_pictures = panel_Distance_pictures;
                            panel_text = panel_Distance_text;
                            panel_hand = panel_Distance_hand;
                            panel_scaleCircle = panel_Distance_scaleCircle;
                            panel_scaleLinear = panel_Distance_scaleLinear;
                            break;




                        case "Weather":
                            panel_pictures = panel_Weather_pictures;
                            panel_text = panel_Weather_text;
                            panel_hand = panel_Weather_hand;
                            panel_scaleCircle = panel_Weather_scaleCircle;
                            panel_scaleLinear = panel_Weather_scaleLinear;
                            break;

                    }

                    // набор картинок
                    if (panel_pictures != null)
                    {
                        checkBox_Use = (CheckBox)panel_pictures.Controls[0];
                        if (activity.ImageProgress != null && activity.ImageProgress.ImageSet != null &&
                            activity.ImageProgress.Coordinates != null && OneCoordinates(activity.ImageProgress.Coordinates))

                            //activity.ImageProgress.Coordinates != null && activity.ImageProgress.Coordinates.Count == 1)
                        {
                            checkBox_Use.Checked = true;
                            ComboBox comboBox_image = (ComboBox)panel_pictures.Controls[1];
                            NumericUpDown numericUpDownX = (NumericUpDown)panel_pictures.Controls[2];
                            NumericUpDown numericUpDownY = (NumericUpDown)panel_pictures.Controls[3];
                            NumericUpDown numericUpDown_count = (NumericUpDown)panel_pictures.Controls[4];

                            comboBoxSetText(comboBox_image, activity.ImageProgress.ImageSet.ImageIndex);
                            numericUpDown_count.Value = activity.ImageProgress.ImageSet.ImagesCount;
                            numericUpDownX.Value = activity.ImageProgress.Coordinates[0].X;
                            numericUpDownY.Value = activity.ImageProgress.Coordinates[0].Y;
                        }
                    }

                    // надпись
                    if (panel_text != null && activity.Type != "Weather")
                    {
                        checkBox_Use = (CheckBox)panel_text.Controls[0];
                        if (activity.Digits != null && activity.Digits.Count > 0)
                        {
                            checkBox_Use.Checked = true;
                            if (activity.Digits[0].Digit != null && activity.Digits[0].Digit.Image != null)
                            {
                                ComboBox comboBox_image = (ComboBox)panel_text.Controls[1];
                                ComboBox comboBox_unit = (ComboBox)panel_text.Controls[2];
                                ComboBox comboBox_separator = (ComboBox)panel_text.Controls[3];
                                NumericUpDown numericUpDownX = (NumericUpDown)panel_text.Controls[4];
                                NumericUpDown numericUpDownY = (NumericUpDown)panel_text.Controls[5];
                                NumericUpDown numericUpDown_unitX = (NumericUpDown)panel_text.Controls[6];
                                NumericUpDown numericUpDown_unitY = (NumericUpDown)panel_text.Controls[7];
                                ComboBox comboBox_alignment = (ComboBox)panel_text.Controls[8];
                                NumericUpDown numericUpDown_spacing = (NumericUpDown)panel_text.Controls[9];
                                CheckBox checkBox_add_zero = (CheckBox)panel_text.Controls[10];
                                ComboBox comboBox_imageError = (ComboBox)panel_text.Controls[11];

                                numericUpDownX.Value = activity.Digits[0].Digit.Image.X;
                                numericUpDownY.Value = activity.Digits[0].Digit.Image.Y;

                                // десятичный разделитель
                                if (activity.Type == "Distance")
                                {
                                    ComboBox comboBox_DecimalPoint = (ComboBox)panel_text.Controls[12];
                                    if(activity.Digits[0].Digit.Image.DecimalPointImageIndex != null)
                                        comboBoxSetText(comboBox_DecimalPoint, (long)activity.Digits[0].Digit.Image.DecimalPointImageIndex);
                                }

                                if (activity.Digits[0].Digit.Image.NoDataImageIndex != null)
                                    comboBoxSetText(comboBox_imageError, (long)activity.Digits[0].Digit.Image.NoDataImageIndex);
                                foreach (MultilangImage multilangImage in activity.Digits[0].Digit.Image.MultilangImage)
                                {
                                    if (multilangImage.LangCode == "All")
                                        comboBoxSetText(comboBox_image, multilangImage.ImageSet.ImageIndex);
                                }
                                if (activity.Digits[0].Digit.Image.MultilangImageUnit != null)
                                {
                                    foreach (MultilangImage multilangImage in activity.Digits[0].Digit.Image.MultilangImageUnit)
                                    {
                                        if (multilangImage.LangCode == "All")
                                            comboBoxSetText(comboBox_separator, multilangImage.ImageSet.ImageIndex);
                                    }
                                }
                                Alignment2ToString(comboBox_alignment, activity.Digits[0].Digit.Alignment);
                                if (activity.Digits[0].Digit.Spacing != null)
                                    numericUpDown_spacing.Value = (decimal)activity.Digits[0].Digit.Spacing;
                                checkBox_add_zero.Checked = activity.Digits[0].Digit.PaddingZero;
                                if (activity.Digits[0].Separator != null)
                                {
                                    comboBoxSetText(comboBox_unit, activity.Digits[0].Separator.ImageIndex);
                                    numericUpDown_unitX.Value = activity.Digits[0].Separator.Coordinates.X;
                                    numericUpDown_unitY.Value = activity.Digits[0].Separator.Coordinates.Y;
                                }
                            }
                        }
                    }
                    else if (panel_text != null && activity.Type == "Weather")
                    {
                        if (activity.Digits != null && activity.Digits.Count > 0)
                        {
                            foreach(DigitalCommonDigit digitalCommonDigit in activity.Digits)
                            {
                                if(digitalCommonDigit.Type != null && digitalCommonDigit.Type == "Min")
                                    panel_text = panel_Weather_textMin;
                                if (digitalCommonDigit.Type != null && digitalCommonDigit.Type == "Max")
                                    panel_text = panel_Weather_textMax;
                                if (digitalCommonDigit.Type == null)
                                    panel_text = panel_Weather_text;

                                checkBox_Use = (CheckBox)panel_text.Controls[0];
                                checkBox_Use.Checked = true;
                                if (digitalCommonDigit.Digit != null && digitalCommonDigit.Digit.Image != null)
                                {
                                    ComboBox comboBox_image = (ComboBox)panel_text.Controls[1];
                                    ComboBox comboBox_unit = (ComboBox)panel_text.Controls[2];
                                    ComboBox comboBox_separatorF = (ComboBox)panel_text.Controls[3];
                                    NumericUpDown numericUpDownX = (NumericUpDown)panel_text.Controls[4];
                                    NumericUpDown numericUpDownY = (NumericUpDown)panel_text.Controls[5];
                                    NumericUpDown numericUpDown_unitX = (NumericUpDown)panel_text.Controls[6];
                                    NumericUpDown numericUpDown_unitY = (NumericUpDown)panel_text.Controls[7];
                                    ComboBox comboBox_alignment = (ComboBox)panel_text.Controls[8];
                                    NumericUpDown numericUpDown_spacing = (NumericUpDown)panel_text.Controls[9];
                                    //CheckBox checkBox_add_zero = (CheckBox)panel_text.Controls[10];
                                    ComboBox comboBox_imageError = (ComboBox)panel_text.Controls[10];
                                    ComboBox comboBox_imageMinus = (ComboBox)panel_text.Controls[11];

                                    if (digitalCommonDigit.Type != null && digitalCommonDigit.Type == "Max")
                                    {
                                        CheckBox checkBox_follow = (CheckBox)panel_text.Controls[12];
                                        if (digitalCommonDigit.CombingMode == "Single")
                                        {
                                            checkBox_follow.Checked = false;
                                        }
                                        else
                                        {
                                            checkBox_follow.Checked = true;
                                        }
                                    }

                                    numericUpDownX.Value = digitalCommonDigit.Digit.Image.X;
                                    numericUpDownY.Value = digitalCommonDigit.Digit.Image.Y;

                                    if (digitalCommonDigit.Digit.Image.NoDataImageIndex != null)
                                        comboBoxSetText(comboBox_imageError, (long)digitalCommonDigit.Digit.Image.NoDataImageIndex);

                                    if (digitalCommonDigit.Digit.Image.DelimiterImageIndex != null)
                                        comboBoxSetText(comboBox_imageMinus, (long)digitalCommonDigit.Digit.Image.DelimiterImageIndex);

                                    foreach (MultilangImage multilangImage in digitalCommonDigit.Digit.Image.MultilangImage)
                                    {
                                        if (multilangImage.LangCode == "All")
                                            comboBoxSetText(comboBox_image, multilangImage.ImageSet.ImageIndex);
                                    }
                                    if (digitalCommonDigit.Digit.Image.MultilangImageUnit != null)
                                    {
                                        foreach (MultilangImage multilangImage in digitalCommonDigit.Digit.Image.MultilangImageUnit)
                                        {
                                            if (multilangImage.LangCode == null && comboBox_separatorF.SelectedIndex < 0)
                                                comboBoxSetText(comboBox_separatorF, multilangImage.ImageSet.ImageIndex);
                                            if (multilangImage.LangCode == "All")
                                                comboBoxSetText(comboBox_separatorF, multilangImage.ImageSet.ImageIndex);
                                        }
                                    }
                                    Alignment2ToString(comboBox_alignment, digitalCommonDigit.Digit.Alignment);
                                    if (digitalCommonDigit.Digit.Spacing != null)
                                        numericUpDown_spacing.Value = (decimal)digitalCommonDigit.Digit.Spacing;
                                    if (digitalCommonDigit.Separator != null)
                                    {
                                        comboBoxSetText(comboBox_unit, digitalCommonDigit.Separator.ImageIndex);
                                        numericUpDown_unitX.Value = digitalCommonDigit.Separator.Coordinates.X;
                                        numericUpDown_unitY.Value = digitalCommonDigit.Separator.Coordinates.Y;
                                    }
                                }
                            }
                            
                        }
                    }

                    // стрелочный индикатор
                    if (panel_hand != null)
                    {
                        checkBox_Use = (CheckBox)panel_hand.Controls[0];
                        if (activity.PointerProgress != null && activity.PointerProgress.Pointer != null)
                        {
                            checkBox_Use.Checked = true;
                            ComboBox comboBox_image = (ComboBox)panel_hand.Controls[1];
                            NumericUpDown numericUpDownX = (NumericUpDown)panel_hand.Controls[2];
                            NumericUpDown numericUpDownY = (NumericUpDown)panel_hand.Controls[3];
                            NumericUpDown numericUpDown_offsetX = (NumericUpDown)panel_hand.Controls[4];
                            NumericUpDown numericUpDown_offsetY = (NumericUpDown)panel_hand.Controls[5];
                            ComboBox comboBox_imageCentr = (ComboBox)panel_hand.Controls[6];
                            NumericUpDown numericUpDownX_centr = (NumericUpDown)panel_hand.Controls[7];
                            NumericUpDown numericUpDownY_centr = (NumericUpDown)panel_hand.Controls[8];
                            NumericUpDown numericUpDown_startAngle = (NumericUpDown)panel_hand.Controls[9];
                            NumericUpDown numericUpDown_endAngle = (NumericUpDown)panel_hand.Controls[10];
                            ComboBox comboBox_imageBackground = (ComboBox)panel_hand.Controls[11];
                            NumericUpDown numericUpDownX_background = (NumericUpDown)panel_hand.Controls[12];
                            NumericUpDown numericUpDownY_background = (NumericUpDown)panel_hand.Controls[13];

                            comboBoxSetText(comboBox_image, activity.PointerProgress.Pointer.ImageIndex);
                            numericUpDownX.Value = activity.PointerProgress.X;
                            numericUpDownY.Value = activity.PointerProgress.Y;

                            numericUpDown_startAngle.Value = (decimal)activity.PointerProgress.StartAngle;
                            numericUpDown_endAngle.Value = (decimal)activity.PointerProgress.EndAngle;

                            numericUpDown_offsetX.Value = activity.PointerProgress.Pointer.Coordinates.X;
                            numericUpDown_offsetY.Value = activity.PointerProgress.Pointer.Coordinates.Y;

                            if (activity.PointerProgress.Cover != null)
                            {
                                comboBoxSetText(comboBox_imageCentr, activity.PointerProgress.Cover.ImageIndex);
                                numericUpDownX_centr.Value = activity.PointerProgress.Cover.Coordinates.X;
                                numericUpDownY_centr.Value = activity.PointerProgress.Cover.Coordinates.Y;
                            }

                            if (activity.PointerProgress.Scale != null && activity.PointerProgress.Scale.ImageSet != null)
                            {
                                foreach (MultilangImage multilangImage in activity.PointerProgress.Scale.ImageSet)
                                {
                                    if (multilangImage.LangCode == "All")
                                        comboBoxSetText(comboBox_imageBackground, multilangImage.ImageSet.ImageIndex);
                                }
                                numericUpDownX_background.Value = activity.PointerProgress.Scale.Coordinates.X;
                                numericUpDownY_background.Value = activity.PointerProgress.Scale.Coordinates.Y;
                            }

                        }
                    }

                    // круговая шкала
                    if (panel_scaleCircle != null)
                    {
                        checkBox_Use = (CheckBox)panel_scaleCircle.Controls[0];
                        if (activity.ProgressBar != null && activity.ProgressBar.AngleSettings != null)
                        {
                            checkBox_Use.Checked = true;
                            RadioButton radioButton_image = (RadioButton)panel_scaleCircle.Controls[1];
                            RadioButton radioButton_color = (RadioButton)panel_scaleCircle.Controls[2];
                            ComboBox comboBox_image = (ComboBox)panel_scaleCircle.Controls[3];
                            ComboBox comboBox_color = (ComboBox)panel_scaleCircle.Controls[4];
                            ComboBox comboBox_flatness = (ComboBox)panel_scaleCircle.Controls[5];
                            ComboBox comboBox_background = (ComboBox)panel_scaleCircle.Controls[6];
                            NumericUpDown numericUpDownX = (NumericUpDown)panel_scaleCircle.Controls[7];
                            NumericUpDown numericUpDownY = (NumericUpDown)panel_scaleCircle.Controls[8];
                            NumericUpDown numericUpDown_radius = (NumericUpDown)panel_scaleCircle.Controls[9];
                            NumericUpDown numericUpDown_width = (NumericUpDown)panel_scaleCircle.Controls[10];
                            NumericUpDown numericUpDown_startAngle = (NumericUpDown)panel_scaleCircle.Controls[11];
                            NumericUpDown numericUpDown_endAngle = (NumericUpDown)panel_scaleCircle.Controls[12];
                            if (activity.ProgressBar.ForegroundImageIndex != null)
                            {
                                radioButton_image.Checked = true;
                                comboBoxSetText(comboBox_image, (long)activity.ProgressBar.ForegroundImageIndex);
                            }
                            else
                            {
                                radioButton_color.Checked = true;
                                Color color = Color.DarkOrange;
                                if (activity.ProgressBar.Color != null)
                                {
                                    color = ColorRead(activity.ProgressBar.Color);
                                }
                                comboBox_color.BackColor = color;
                            }
                            
                            if (activity.ProgressBar.BackgroundImageIndex != null)
                                comboBoxSetText(comboBox_background, (long)activity.ProgressBar.BackgroundImageIndex);

                            numericUpDown_width.Value = activity.ProgressBar.Width;

                            numericUpDownX.Value = activity.ProgressBar.AngleSettings.X;
                            numericUpDownY.Value = activity.ProgressBar.AngleSettings.Y;
                            numericUpDown_startAngle.Value = (decimal)activity.ProgressBar.AngleSettings.StartAngle;
                            numericUpDown_endAngle.Value = (decimal)activity.ProgressBar.AngleSettings.EndAngle;
                            numericUpDown_radius.Value = (decimal)activity.ProgressBar.AngleSettings.Radius;

                            switch (activity.ProgressBar.Flatness)
                            {
                                case 90:
                                    comboBox_flatness.SelectedIndex = 1;
                                    break;
                                case 180:
                                    comboBox_flatness.SelectedIndex = 2;
                                    break;
                                default:
                                    comboBox_flatness.SelectedIndex = 0;
                                    break;
                            }
                        }
                    }
               
                    // линейная шкала
                    if (panel_scaleLinear != null)
                        {
                            checkBox_Use = (CheckBox)panel_scaleLinear.Controls[0];
                            if (activity.ProgressBar != null && activity.ProgressBar.LinearSettings != null)
                            {
                                checkBox_Use.Checked = true;
                                RadioButton radioButton_image = (RadioButton)panel_scaleLinear.Controls[1];
                                RadioButton radioButton_color = (RadioButton)panel_scaleLinear.Controls[2];
                                ComboBox comboBox_image = (ComboBox)panel_scaleLinear.Controls[3];
                                ComboBox comboBox_color = (ComboBox)panel_scaleLinear.Controls[4];
                                ComboBox comboBox_pointer = (ComboBox)panel_scaleLinear.Controls[5];
                                ComboBox comboBox_background = (ComboBox)panel_scaleLinear.Controls[6];
                                NumericUpDown numericUpDownX = (NumericUpDown)panel_scaleLinear.Controls[7];
                                NumericUpDown numericUpDownY = (NumericUpDown)panel_scaleLinear.Controls[8];
                                NumericUpDown numericUpDown_length = (NumericUpDown)panel_scaleLinear.Controls[9];
                                NumericUpDown numericUpDown_width = (NumericUpDown)panel_scaleLinear.Controls[10];
                                if(activity.ProgressBar.ForegroundImageIndex != null)
                                {
                                    radioButton_image.Checked = true;
                                    comboBoxSetText(comboBox_image, (long)activity.ProgressBar.ForegroundImageIndex);
                                }
                                else
                                {
                                    radioButton_color.Checked = true;
                                    Color color = Color.DarkOrange;
                                    if (activity.ProgressBar.Color != null)
                                    {
                                        color = ColorRead(activity.ProgressBar.Color); 
                                    }
                                    comboBox_color.BackColor = color;
                                }
                                if (activity.ProgressBar.PointerImageIndex != null)
                                    comboBoxSetText(comboBox_pointer, (long)activity.ProgressBar.PointerImageIndex);
                                if (activity.ProgressBar.BackgroundImageIndex != null)
                                    comboBoxSetText(comboBox_background, (long)activity.ProgressBar.BackgroundImageIndex);

                                numericUpDownX.Value = activity.ProgressBar.LinearSettings.StartX;
                                numericUpDownY.Value = activity.ProgressBar.LinearSettings.StartY;
                                long length = activity.ProgressBar.LinearSettings.EndX - activity.ProgressBar.LinearSettings.StartX;
                                numericUpDown_length.Value = length;
                                numericUpDown_width.Value = activity.ProgressBar.Width;


                            }
                        }
                    }
            }
            #endregion




            //#region Time
            //if (Watch_Face.Time != null)
            //{
            //    checkBox_Time.Checked = true;
            //    if (Watch_Face.Time.Hours != null)
            //    {
            //        checkBox_Hours.Checked = true;
            //        if (Watch_Face.Time.Hours.Tens != null)
            //        {
            //            numericUpDown_Hours_Tens_X.Value = Watch_Face.Time.Hours.Tens.X;
            //            numericUpDown_Hours_Tens_Y.Value = Watch_Face.Time.Hours.Tens.Y;
            //            numericUpDown_Hours_Tens_Count.Value = Watch_Face.Time.Hours.Tens.ImagesCount;
            //            //comboBox_Hours_Tens_Image.Text = Watch_Face.Time.Hours.Tens.ImageIndex.ToString();
            //            comboBoxSetText(comboBox_Hours_Tens_Image, Watch_Face.Time.Hours.Tens.ImageIndex); 
            //        }

            //        if (Watch_Face.Time.Hours.Ones != null)
            //        {
            //            numericUpDown_Hours_Ones_X.Value = Watch_Face.Time.Hours.Ones.X;
            //            numericUpDown_Hours_Ones_Y.Value = Watch_Face.Time.Hours.Ones.Y;
            //            numericUpDown_Hours_Ones_Count.Value = Watch_Face.Time.Hours.Ones.ImagesCount;
            //            //comboBox_Hours_Ones_Image.Text = Watch_Face.Time.Hours.Ones.ImageIndex.ToString();
            //            comboBoxSetText(comboBox_Hours_Ones_Image, Watch_Face.Time.Hours.Ones.ImageIndex); 
            //        }
            //    }
            //    else checkBox_Hours.Checked = false;

            //    if (Watch_Face.Time.Minutes != null)
            //    {
            //        checkBox_Minutes.Checked = true;
            //        if (Watch_Face.Time.Minutes.Tens != null)
            //        {
            //            numericUpDown_Min_Tens_X.Value = Watch_Face.Time.Minutes.Tens.X;
            //            numericUpDown_Min_Tens_Y.Value = Watch_Face.Time.Minutes.Tens.Y;
            //            numericUpDown_Min_Tens_Count.Value = Watch_Face.Time.Minutes.Tens.ImagesCount;
            //            //comboBox_Min_Tens_Image.Text = Watch_Face.Time.Minutes.Tens.ImageIndex.ToString();
            //            comboBoxSetText(comboBox_Min_Tens_Image, Watch_Face.Time.Minutes.Tens.ImageIndex); 
            //        }

            //        if (Watch_Face.Time.Minutes.Ones != null)
            //        {
            //            numericUpDown_Min_Ones_X.Value = Watch_Face.Time.Minutes.Ones.X;
            //            numericUpDown_Min_Ones_Y.Value = Watch_Face.Time.Minutes.Ones.Y;
            //            numericUpDown_Min_Ones_Count.Value = Watch_Face.Time.Minutes.Ones.ImagesCount;
            //            //comboBox_Min_Ones_Image.Text = Watch_Face.Time.Minutes.Ones.ImageIndex.ToString();
            //            comboBoxSetText(comboBox_Min_Ones_Image, Watch_Face.Time.Minutes.Ones.ImageIndex); 
            //        }
            //    }
            //    else checkBox_Minutes.Checked = false;

            //    if (Watch_Face.Time.Seconds != null)
            //    {
            //        checkBox_Seconds.Checked = true;
            //        if (Watch_Face.Time.Seconds.Tens != null)
            //        {
            //            numericUpDown_Sec_Tens_X.Value = Watch_Face.Time.Seconds.Tens.X;
            //            numericUpDown_Sec_Tens_Y.Value = Watch_Face.Time.Seconds.Tens.Y;
            //            numericUpDown_Sec_Tens_Count.Value = Watch_Face.Time.Seconds.Tens.ImagesCount;
            //            //comboBox_Sec_Tens_Image.Text = Watch_Face.Time.Seconds.Tens.ImageIndex.ToString();
            //            comboBoxSetText(comboBox_Sec_Tens_Image, Watch_Face.Time.Seconds.Tens.ImageIndex); 
            //        }

            //        if (Watch_Face.Time.Seconds.Ones != null)
            //        {
            //            numericUpDown_Sec_Ones_X.Value = Watch_Face.Time.Seconds.Ones.X;
            //            numericUpDown_Sec_Ones_Y.Value = Watch_Face.Time.Seconds.Ones.Y;
            //            numericUpDown_Sec_Ones_Count.Value = Watch_Face.Time.Seconds.Ones.ImagesCount;
            //            //comboBox_Sec_Ones_Image.Text = Watch_Face.Time.Seconds.Ones.ImageIndex.ToString();
            //            comboBoxSetText(comboBox_Sec_Ones_Image, Watch_Face.Time.Seconds.Ones.ImageIndex); 
            //        }
            //    }
            //    else checkBox_Seconds.Checked = false;

            //    if (Watch_Face.Time.Delimiter != null)
            //    {
            //        checkBox_Delimiter.Checked = true;
            //        numericUpDown_Delimiter_X.Value = Watch_Face.Time.Delimiter.X;
            //        numericUpDown_Delimiter_Y.Value = Watch_Face.Time.Delimiter.Y;
            //        //comboBox_Delimiter_Image.Text = Watch_Face.Time.Delimiter.ImageIndex.ToString();
            //        comboBoxSetText(comboBox_Delimiter_Image, Watch_Face.Time.Delimiter.ImageIndex);
            //    }
            //    else checkBox_Delimiter.Checked = false;

            //    if (Watch_Face.Time.AmPm != null)
            //    {
            //        checkBox_AmPm.Checked = true;
            //        numericUpDown_AmPm_X.Value = Watch_Face.Time.AmPm.X;
            //        numericUpDown_AmPm_Y.Value = Watch_Face.Time.AmPm.Y;
            //        if (Watch_Face.Time.AmPm.ImageIndexAMCN > 0)
            //            //comboBox_Image_Am.Text = Watch_Face.Time.AmPm.ImageIndexAMCN.ToString();
            //            comboBoxSetText(comboBox_Image_Am, Watch_Face.Time.AmPm.ImageIndexAMCN);
            //        if (Watch_Face.Time.AmPm.ImageIndexAMEN != null && Watch_Face.Time.AmPm.ImageIndexAMEN > 0)
            //            //comboBox_Image_Am.Text = Watch_Face.Time.AmPm.ImageIndexAMEN.ToString();
            //            comboBoxSetText(comboBox_Image_Am, (long)Watch_Face.Time.AmPm.ImageIndexAMEN);
            //        if (Watch_Face.Time.AmPm.ImageIndexPMCN > 0)
            //            //comboBox_Image_Pm.Text = Watch_Face.Time.AmPm.ImageIndexPMCN.ToString();
            //            comboBoxSetText(comboBox_Image_Pm, Watch_Face.Time.AmPm.ImageIndexPMCN);
            //        if (Watch_Face.Time.AmPm.ImageIndexPMEN != null && Watch_Face.Time.AmPm.ImageIndexPMEN > 0)
            //            //comboBox_Image_Pm.Text = Watch_Face.Time.AmPm.ImageIndexPMEN.ToString();
            //            comboBoxSetText(comboBox_Image_Pm, (long)Watch_Face.Time.AmPm.ImageIndexPMEN);
            //    }
            //    else checkBox_AmPm.Checked = false;
            //}
            //else
            //{
            //    checkBox_Time.Checked = false;
            //    checkBox_Hours.Checked = false;
            //    checkBox_Minutes.Checked = false;
            //    checkBox_Seconds.Checked = false;
            //    checkBox_Delimiter.Checked = false;
            //    checkBox_AmPm.Checked = false;
            //}
            //#endregion

            //#region Date
            //if (Watch_Face.Date != null)
            //{
            //    checkBox_Date.Checked = true;
            //    if (Watch_Face.Date.WeekDay != null)
            //    {
            //        checkBox_WeekDay.Checked = true;
            //        numericUpDown_WeekDay_X.Value = Watch_Face.Date.WeekDay.X;
            //        numericUpDown_WeekDay_Y.Value = Watch_Face.Date.WeekDay.Y;
            //        numericUpDown_WeekDay_Count.Value = Watch_Face.Date.WeekDay.ImagesCount;
            //        //comboBox_WeekDay_Image.Text = Watch_Face.Date.WeekDay.ImageIndex.ToString();
            //        comboBoxSetText(comboBox_WeekDay_Image, Watch_Face.Date.WeekDay.ImageIndex);
            //    }
            //    else checkBox_WeekDay.Checked = false;

            //    if ((Watch_Face.Date.WeekDayProgress != null) && (Watch_Face.Date.WeekDayProgress.Coordinates != null))
            //    {
            //        checkBox_DOW_IconSet.Checked = true;
            //        dataGridView_DOW_IconSet.Rows.Clear();
            //        comboBoxSetText(comboBox_DOW_IconSet_Image, Watch_Face.Date.WeekDayProgress.ImageIndex);
            //        foreach (Coordinates coordinates in Watch_Face.Date.WeekDayProgress.Coordinates)
            //        {
            //            var RowNew = new DataGridViewRow();
            //            dataGridView_DOW_IconSet.Rows.Add(coordinates.X, coordinates.Y);
            //        }
            //    }
            //    else checkBox_DOW_IconSet.Checked = false;

            //    if (Watch_Face.Date.MonthAndDay != null)
            //    {
            //        checkBox_TwoDigitsDay.Checked = Watch_Face.Date.MonthAndDay.TwoDigitsDay;
            //        checkBox_TwoDigitsMonth.Checked = Watch_Face.Date.MonthAndDay.TwoDigitsMonth;

            //        if ((Watch_Face.Date.MonthAndDay.OneLine != null) && (Watch_Face.Date.MonthAndDay.OneLine.Number != null))
            //        {
            //            checkBox_OneLine.Checked = true;
            //            numericUpDown_OneLine_StartCorner_X.Value = Watch_Face.Date.MonthAndDay.OneLine.Number.TopLeftX;
            //            numericUpDown_OneLine_StartCorner_Y.Value = Watch_Face.Date.MonthAndDay.OneLine.Number.TopLeftY;
            //            numericUpDown_OneLine_EndCorner_X.Value = Watch_Face.Date.MonthAndDay.OneLine.Number.BottomRightX;
            //            numericUpDown_OneLine_EndCorner_Y.Value = Watch_Face.Date.MonthAndDay.OneLine.Number.BottomRightY;

            //            numericUpDown_OneLine_Spacing.Value = Watch_Face.Date.MonthAndDay.OneLine.Number.Spacing;
            //            numericUpDown_OneLine_Count.Value = Watch_Face.Date.MonthAndDay.OneLine.Number.ImagesCount;
            //            //comboBox_OneLine_Image.Text = Watch_Face.Date.MonthAndDay.OneLine.Number.ImageIndex.ToString();
            //            comboBoxSetText(comboBox_OneLine_Image, Watch_Face.Date.MonthAndDay.OneLine.Number.ImageIndex);
            //            //comboBox_OneLine_Delimiter.Text = Watch_Face.Date.MonthAndDay.OneLine.DelimiterImageIndex.ToString();
            //            if (Watch_Face.Date.MonthAndDay.OneLine.DelimiterImageIndex != null)
            //                comboBoxSetText(comboBox_OneLine_Delimiter, (long)Watch_Face.Date.MonthAndDay.OneLine.DelimiterImageIndex);
            //            AlignmentToString(comboBox_OneLine_Alignment, Watch_Face.Date.MonthAndDay.OneLine.Number.Alignment);
            //            //comboBox_OneLine_Alignment.Text = Alignment;
            //        }
            //        else checkBox_OneLine.Checked = false;

            //        if (Watch_Face.Date.MonthAndDay.Separate != null)
            //        {
            //            if (Watch_Face.Date.MonthAndDay.Separate.Day != null)
            //            {
            //                checkBox_MonthAndDayD.Checked = true;
            //                numericUpDown_MonthAndDayD_StartCorner_X.Value = Watch_Face.Date.MonthAndDay.Separate.Day.TopLeftX;
            //                numericUpDown_MonthAndDayD_StartCorner_Y.Value = Watch_Face.Date.MonthAndDay.Separate.Day.TopLeftY;
            //                numericUpDown_MonthAndDayD_EndCorner_X.Value = Watch_Face.Date.MonthAndDay.Separate.Day.BottomRightX;
            //                numericUpDown_MonthAndDayD_EndCorner_Y.Value = Watch_Face.Date.MonthAndDay.Separate.Day.BottomRightY;

            //                numericUpDown_MonthAndDayD_Spacing.Value = Watch_Face.Date.MonthAndDay.Separate.Day.Spacing;
            //                numericUpDown_MonthAndDayD_Count.Value = Watch_Face.Date.MonthAndDay.Separate.Day.ImagesCount;
            //                //comboBox_MonthAndDayD_Image.Text = Watch_Face.Date.MonthAndDay.Separate.Day.ImageIndex.ToString();
            //                comboBoxSetText(comboBox_MonthAndDayD_Image, Watch_Face.Date.MonthAndDay.Separate.Day.ImageIndex);
            //                AlignmentToString(comboBox_MonthAndDayD_Alignment, Watch_Face.Date.MonthAndDay.Separate.Day.Alignment);
            //                //comboBox_MonthAndDayD_Alignment.Text = Alignment;
            //            }
            //            else checkBox_MonthAndDayD.Checked = false;

            //            if (Watch_Face.Date.MonthAndDay.Separate.Month != null)
            //            {
            //                checkBox_MonthAndDayM.Checked = true;
            //                numericUpDown_MonthAndDayM_StartCorner_X.Value = Watch_Face.Date.MonthAndDay.Separate.Month.TopLeftX;
            //                numericUpDown_MonthAndDayM_StartCorner_Y.Value = Watch_Face.Date.MonthAndDay.Separate.Month.TopLeftY;
            //                numericUpDown_MonthAndDayM_EndCorner_X.Value = Watch_Face.Date.MonthAndDay.Separate.Month.BottomRightX;
            //                numericUpDown_MonthAndDayM_EndCorner_Y.Value = Watch_Face.Date.MonthAndDay.Separate.Month.BottomRightY;

            //                numericUpDown_MonthAndDayM_Spacing.Value = Watch_Face.Date.MonthAndDay.Separate.Month.Spacing;
            //                numericUpDown_MonthAndDayM_Count.Value = Watch_Face.Date.MonthAndDay.Separate.Month.ImagesCount;
            //                //comboBox_MonthAndDayM_Image.Text = Watch_Face.Date.MonthAndDay.Separate.Month.ImageIndex.ToString();
            //                comboBoxSetText(comboBox_MonthAndDayM_Image, Watch_Face.Date.MonthAndDay.Separate.Month.ImageIndex);
            //                AlignmentToString(comboBox_MonthAndDayM_Alignment,Watch_Face.Date.MonthAndDay.Separate.Month.Alignment);
            //                //comboBox_MonthAndDayM_Alignment.Text = Alignment;
            //            }
            //            else checkBox_MonthAndDayM.Checked = false;

            //            if (Watch_Face.Date.MonthAndDay.Separate.MonthName != null)
            //            {
            //                checkBox_MonthName.Checked = true;
            //                numericUpDown_MonthName_X.Value = Watch_Face.Date.MonthAndDay.Separate.MonthName.X;
            //                numericUpDown_MonthName_Y.Value = Watch_Face.Date.MonthAndDay.Separate.MonthName.Y;

            //                numericUpDown_MonthName_Count.Value = Watch_Face.Date.MonthAndDay.Separate.MonthName.ImagesCount;
            //                //comboBox_MonthName_Image.Text = Watch_Face.Date.MonthAndDay.Separate.MonthName.ImageIndex.ToString();
            //                comboBoxSetText(comboBox_MonthName_Image, Watch_Face.Date.MonthAndDay.Separate.MonthName.ImageIndex);
            //            }
            //            else checkBox_MonthName.Checked = false;
            //        }
            //        else
            //        {
            //            checkBox_MonthAndDayD.Checked = false;
            //            checkBox_MonthAndDayM.Checked = false;
            //            checkBox_MonthName.Checked = false;
            //        }

            //    }

            //    if (Watch_Face.Date.Year != null)
            //    {
            //        if ((Watch_Face.Date.Year.OneLine != null) && (Watch_Face.Date.Year.OneLine.Number != null))
            //        {
            //            checkBox_Year.Checked = true;
            //            numericUpDown_Year_StartCorner_X.Value = Watch_Face.Date.Year.OneLine.Number.TopLeftX;
            //            numericUpDown_Year_StartCorner_Y.Value = Watch_Face.Date.Year.OneLine.Number.TopLeftY;
            //            numericUpDown_Year_EndCorner_X.Value = Watch_Face.Date.Year.OneLine.Number.BottomRightX;
            //            numericUpDown_Year_EndCorner_Y.Value = Watch_Face.Date.Year.OneLine.Number.BottomRightY;

            //            numericUpDown_Year_Spacing.Value = Watch_Face.Date.Year.OneLine.Number.Spacing;
            //            numericUpDown_Year_Count.Value = Watch_Face.Date.Year.OneLine.Number.ImagesCount;
            //            comboBoxSetText(comboBox_Year_Image, Watch_Face.Date.Year.OneLine.Number.ImageIndex);
            //            if (Watch_Face.Date.Year.OneLine.DelimiterImageIndex != null)
            //            comboBoxSetText(comboBox_Year_Delimiter, (long)Watch_Face.Date.Year.OneLine.DelimiterImageIndex);
            //            AlignmentToString(comboBox_Year_Alignment, Watch_Face.Date.Year.OneLine.Number.Alignment);
            //            //comboBox_Year_Alignment.Text = Alignment;
            //        }
            //        else checkBox_Year.Checked = false;
            //    }

            //}
            //else
            //{
            //    checkBox_Date.Checked = false;
            //    checkBox_WeekDay.Checked = false;
            //    checkBox_OneLine.Checked = false;
            //    checkBox_MonthAndDayD.Checked = false;
            //    checkBox_MonthAndDayM.Checked = false;
            //    checkBox_MonthName.Checked = false;
            //    checkBox_Year.Checked = false;
            //}
            //#endregion

            //#region AnalogDate
            //if (Watch_Face.DaysProgress != null)
            //{
            //    if ((Watch_Face.DaysProgress.UnknownField2 != null) && (Watch_Face.DaysProgress.UnknownField2.Image != null))
            //    {
            //        checkBox_ADDay_ClockHand.Checked = true;
            //        numericUpDown_ADDay_ClockHand_X.Value = Watch_Face.DaysProgress.UnknownField2.Image.X;
            //        numericUpDown_ADDay_ClockHand_Y.Value = Watch_Face.DaysProgress.UnknownField2.Image.Y;
            //        comboBoxSetText(comboBox_ADDay_ClockHand_Image, Watch_Face.DaysProgress.UnknownField2.Image.ImageIndex);
            //        if (Watch_Face.DaysProgress.UnknownField2.CenterOffset != null)
            //        {
            //            numericUpDown_ADDay_ClockHand_Offset_X.Value = Watch_Face.DaysProgress.UnknownField2.CenterOffset.X;
            //            numericUpDown_ADDay_ClockHand_Offset_Y.Value = Watch_Face.DaysProgress.UnknownField2.CenterOffset.Y;

            //        }
            //        if (Watch_Face.DaysProgress.UnknownField2.Sector != null)
            //        {
            //            numericUpDown_ADDay_ClockHand_StartAngle.Value = (int)(Watch_Face.DaysProgress.UnknownField2.Sector.StartAngle / 100);
            //            numericUpDown_ADDay_ClockHand_EndAngle.Value = (int)(Watch_Face.DaysProgress.UnknownField2.Sector.EndAngle / 100);

            //        }
            //    }
            //    else checkBox_ADDay_ClockHand.Checked = false;

            //    if ((Watch_Face.DaysProgress.AnalogDOW != null) && (Watch_Face.DaysProgress.AnalogDOW.Image != null))
            //    {
            //        checkBox_ADWeekDay_ClockHand.Checked = true;
            //        numericUpDown_ADWeekDay_ClockHand_X.Value = Watch_Face.DaysProgress.AnalogDOW.Image.X;
            //        numericUpDown_ADWeekDay_ClockHand_Y.Value = Watch_Face.DaysProgress.AnalogDOW.Image.Y;
            //        comboBoxSetText(comboBox_ADWeekDay_ClockHand_Image, Watch_Face.DaysProgress.AnalogDOW.Image.ImageIndex);
            //        if (Watch_Face.DaysProgress.AnalogDOW.CenterOffset != null)
            //        {
            //            numericUpDown_ADWeekDay_ClockHand_Offset_X.Value = Watch_Face.DaysProgress.AnalogDOW.CenterOffset.X;
            //            numericUpDown_ADWeekDay_ClockHand_Offset_Y.Value = Watch_Face.DaysProgress.AnalogDOW.CenterOffset.Y;

            //        }
            //        if (Watch_Face.DaysProgress.AnalogDOW.Sector != null)
            //        {
            //            numericUpDown_ADWeekDay_ClockHand_StartAngle.Value = (int)(Watch_Face.DaysProgress.AnalogDOW.Sector.StartAngle / 100);
            //            numericUpDown_ADWeekDay_ClockHand_EndAngle.Value = (int)(Watch_Face.DaysProgress.AnalogDOW.Sector.EndAngle / 100);

            //        }
            //    }
            //    else checkBox_ADWeekDay_ClockHand.Checked = false;

            //    if ((Watch_Face.DaysProgress.AnalogMonth != null) && (Watch_Face.DaysProgress.AnalogMonth.Image != null))
            //    {
            //        checkBox_ADMonth_ClockHand.Checked = true;
            //        numericUpDown_ADMonth_ClockHand_X.Value = Watch_Face.DaysProgress.AnalogMonth.Image.X;
            //        numericUpDown_ADMonth_ClockHand_Y.Value = Watch_Face.DaysProgress.AnalogMonth.Image.Y;
            //        comboBoxSetText(comboBox_ADMonth_ClockHand_Image, Watch_Face.DaysProgress.AnalogMonth.Image.ImageIndex);
            //        if (Watch_Face.DaysProgress.AnalogMonth.CenterOffset != null)
            //        {
            //            numericUpDown_ADMonth_ClockHand_Offset_X.Value = Watch_Face.DaysProgress.AnalogMonth.CenterOffset.X;
            //            numericUpDown_ADMonth_ClockHand_Offset_Y.Value = Watch_Face.DaysProgress.AnalogMonth.CenterOffset.Y;

            //        }
            //        if (Watch_Face.DaysProgress.AnalogMonth.Sector != null)
            //        {
            //            numericUpDown_ADMonth_ClockHand_StartAngle.Value = (int)(Watch_Face.DaysProgress.AnalogMonth.Sector.StartAngle / 100);
            //            numericUpDown_ADMonth_ClockHand_EndAngle.Value = (int)(Watch_Face.DaysProgress.AnalogMonth.Sector.EndAngle / 100);

            //        }
            //    }
            //    else checkBox_ADMonth_ClockHand.Checked = false;

            //}
            //else
            //{
            //    checkBox_ADDay_ClockHand.Checked = false;
            //    checkBox_ADWeekDay_ClockHand.Checked = false;
            //    checkBox_ADMonth_ClockHand.Checked = false;
            //}
            //#endregion

            //#region StepsProgress
            //if (Watch_Face.StepsProgress != null)
            //{
            //    if (Watch_Face.StepsProgress.WeekDayProgressBar != null)
            //    {
            //        checkBox_StepsProgress.Checked = true;
            //        numericUpDown_StepsProgress_Center_X.Value = Watch_Face.StepsProgress.WeekDayProgressBar.CenterX;
            //        numericUpDown_StepsProgress_Center_Y.Value = Watch_Face.StepsProgress.WeekDayProgressBar.CenterY;
            //        numericUpDown_StepsProgress_Radius_X.Value = Watch_Face.StepsProgress.WeekDayProgressBar.RadiusX;
            //        numericUpDown_StepsProgress_Radius_Y.Value = Watch_Face.StepsProgress.WeekDayProgressBar.RadiusY;
            //        if (Watch_Face.StepsProgress.WeekDayProgressBar.RadiusY == 0)
            //            numericUpDown_StepsProgress_Radius_Y.Value = Watch_Face.StepsProgress.WeekDayProgressBar.RadiusX;

            //        numericUpDown_StepsProgress_StartAngle.Value = Watch_Face.StepsProgress.WeekDayProgressBar.StartAngle;
            //        numericUpDown_StepsProgress_EndAngle.Value = Watch_Face.StepsProgress.WeekDayProgressBar.EndAngle;
            //        numericUpDown_StepsProgress_Width.Value = Watch_Face.StepsProgress.WeekDayProgressBar.Width;

            //        Color new_color = ColorRead(Watch_Face.StepsProgress.WeekDayProgressBar.Color);
            //        comboBox_StepsProgress_Color.BackColor = new_color;
            //        colorDialog_StepsProgress.Color = new_color;
            //        switch (Watch_Face.StepsProgress.WeekDayProgressBar.Flatness)
            //        {
            //            case 90:
            //                //comboBox_StepsProgress_Flatness.Text = "Треугольное";
            //                comboBox_StepsProgress_Flatness.SelectedIndex = 1;
            //                break;
            //            case 180:
            //                //comboBox_StepsProgress_Flatness.Text = "Плоское";
            //                comboBox_StepsProgress_Flatness.SelectedIndex = 2;
            //                break;
            //            default:
            //                //comboBox_StepsProgress_Flatness.Text = "Круглое";
            //                comboBox_StepsProgress_Flatness.SelectedIndex = 0;
            //                break;
            //        }

            //        if (Watch_Face.StepsProgress.WeekDayProgressBar.ImageIndex != null)
            //        {
            //            comboBoxSetText(comboBox_StepsProgress_Image, (long)Watch_Face.StepsProgress.WeekDayProgressBar.ImageIndex);
            //            int x = 0;
            //            int y = 0;
            //            ColorToCoodinates(new_color, out x, out y);
            //            numericUpDown_StepsProgress_ImageX.Value = x;
            //            numericUpDown_StepsProgress_ImageY.Value = y;
            //            //ColorToCoodinates(new_color, numericUpDown_StepsProgress_ImageX,
            //            //    numericUpDown_StepsProgress_ImageY);
            //            checkBox_StepsProgress_Image.Checked = true;
            //        }
            //        else checkBox_StepsProgress_Image.Checked = false;
            //    }
            //    else checkBox_StepsProgress.Checked = false;

            //    if ((Watch_Face.StepsProgress.WeekDayClockHand != null) && (Watch_Face.StepsProgress.WeekDayClockHand.Image != null))
            //    {
            //        checkBox_StProg_ClockHand.Checked = true;
            //        numericUpDown_StProg_ClockHand_X.Value = Watch_Face.StepsProgress.WeekDayClockHand.Image.X;
            //        numericUpDown_StProg_ClockHand_Y.Value = Watch_Face.StepsProgress.WeekDayClockHand.Image.Y;
            //        comboBoxSetText(comboBox_StProg_ClockHand_Image, Watch_Face.StepsProgress.WeekDayClockHand.Image.ImageIndex);
            //        if (Watch_Face.StepsProgress.WeekDayClockHand.CenterOffset != null)
            //        {
            //            numericUpDown_StProg_ClockHand_Offset_X.Value = Watch_Face.StepsProgress.WeekDayClockHand.CenterOffset.X;
            //            numericUpDown_StProg_ClockHand_Offset_Y.Value = Watch_Face.StepsProgress.WeekDayClockHand.CenterOffset.Y;

            //        }
            //        if (Watch_Face.StepsProgress.WeekDayClockHand.Sector != null)
            //        {
            //            numericUpDown_StProg_ClockHand_StartAngle.Value = (int)(Watch_Face.StepsProgress.WeekDayClockHand.Sector.StartAngle / 100);
            //            numericUpDown_StProg_ClockHand_EndAngle.Value = (int)(Watch_Face.StepsProgress.WeekDayClockHand.Sector.EndAngle / 100);

            //        }
            //    }
            //    else checkBox_StProg_ClockHand.Checked = false;

            //    if ((Watch_Face.StepsProgress.Sliced != null) && (Watch_Face.StepsProgress.Sliced.Coordinates != null))
            //    {
            //        checkBox_SPSliced.Checked = true;
            //        dataGridView_SPSliced.Rows.Clear();
            //        comboBoxSetText(comboBox_SPSliced_Image, Watch_Face.StepsProgress.Sliced.ImageIndex);
            //        foreach (Coordinates coordinates in Watch_Face.StepsProgress.Sliced.Coordinates)
            //        {
            //            var RowNew = new DataGridViewRow();
            //            dataGridView_SPSliced.Rows.Add(coordinates.X, coordinates.Y);
            //        }
            //    }
            //    else checkBox_SPSliced.Checked = false;

            //}
            //else
            //{
            //    checkBox_StepsProgress.Checked = false;
            //    checkBox_StepsProgress_Image.Checked = false;
            //    checkBox_StProg_ClockHand.Checked = false;
            //    checkBox_SPSliced.Checked = false;
            //}
            //#endregion

            //#region Activity
            //if (Watch_Face.Activity != null)
            //{
            //    checkBox_Activity.Checked = true;

            //    if (Watch_Face.Activity.StepsGoal != null)
            //    {
            //        checkBox_ActivityStepsGoal.Checked = true;
            //        numericUpDown_ActivityStepsGoal_StartCorner_X.Value = Watch_Face.Activity.StepsGoal.TopLeftX;
            //        numericUpDown_ActivityStepsGoal_StartCorner_Y.Value = Watch_Face.Activity.StepsGoal.TopLeftY;
            //        numericUpDown_ActivityStepsGoal_EndCorner_X.Value = Watch_Face.Activity.StepsGoal.BottomRightX;
            //        numericUpDown_ActivityStepsGoal_EndCorner_Y.Value = Watch_Face.Activity.StepsGoal.BottomRightY;

            //        //comboBox_ActivitySteps_Image.Text = Watch_Face.Activity.Steps.Step.ImageIndex.ToString();
            //        comboBoxSetText(comboBox_ActivityStepsGoal_Image, Watch_Face.Activity.StepsGoal.ImageIndex);
            //        numericUpDown_ActivityStepsGoal_Count.Value = Watch_Face.Activity.StepsGoal.ImagesCount;
            //        numericUpDown_ActivityStepsGoal_Spacing.Value = Watch_Face.Activity.StepsGoal.Spacing;
            //        AlignmentToString(comboBox_ActivityStepsGoal_Alignment, Watch_Face.Activity.StepsGoal.Alignment);
            //        //comboBox_ActivitySteps_Alignment.Text = Alignment;
            //    }
            //    else checkBox_ActivityStepsGoal.Checked = false;

            //    if ((Watch_Face.Activity.Steps != null) && (Watch_Face.Activity.Steps.Step != null))
            //    {
            //        checkBox_ActivitySteps.Checked = true;
            //        numericUpDown_ActivitySteps_StartCorner_X.Value = Watch_Face.Activity.Steps.Step.TopLeftX;
            //        numericUpDown_ActivitySteps_StartCorner_Y.Value = Watch_Face.Activity.Steps.Step.TopLeftY;
            //        numericUpDown_ActivitySteps_EndCorner_X.Value = Watch_Face.Activity.Steps.Step.BottomRightX;
            //        numericUpDown_ActivitySteps_EndCorner_Y.Value = Watch_Face.Activity.Steps.Step.BottomRightY;

            //        //comboBox_ActivitySteps_Image.Text = Watch_Face.Activity.Steps.Step.ImageIndex.ToString();
            //        comboBoxSetText(comboBox_ActivitySteps_Image, Watch_Face.Activity.Steps.Step.ImageIndex);
            //        numericUpDown_ActivitySteps_Count.Value = Watch_Face.Activity.Steps.Step.ImagesCount;
            //        numericUpDown_ActivitySteps_Spacing.Value = Watch_Face.Activity.Steps.Step.Spacing;
            //        AlignmentToString(comboBox_ActivitySteps_Alignment, Watch_Face.Activity.Steps.Step.Alignment);
            //        //comboBox_ActivitySteps_Alignment.Text = Alignment;
            //    }
            //    else checkBox_ActivitySteps.Checked = false;

            //    if ((Watch_Face.Activity.Distance != null) && (Watch_Face.Activity.Distance.Number != null))
            //    {
            //        checkBox_ActivityDistance.Checked = true;
            //        numericUpDown_ActivityDistance_StartCorner_X.Value = Watch_Face.Activity.Distance.Number.TopLeftX;
            //        numericUpDown_ActivityDistance_StartCorner_Y.Value = Watch_Face.Activity.Distance.Number.TopLeftY;
            //        numericUpDown_ActivityDistance_EndCorner_X.Value = Watch_Face.Activity.Distance.Number.BottomRightX;
            //        numericUpDown_ActivityDistance_EndCorner_Y.Value = Watch_Face.Activity.Distance.Number.BottomRightY;

            //        //comboBox_ActivityDistance_Image.Text = Watch_Face.Activity.Distance.Number.ImageIndex.ToString();
            //        comboBoxSetText(comboBox_ActivityDistance_Image, Watch_Face.Activity.Distance.Number.ImageIndex);
            //        numericUpDown_ActivityDistance_Count.Value = Watch_Face.Activity.Distance.Number.ImagesCount;
            //        numericUpDown_ActivityDistance_Spacing.Value = Watch_Face.Activity.Distance.Number.Spacing;
            //        AlignmentToString(comboBox_ActivityDistance_Alignment, Watch_Face.Activity.Distance.Number.Alignment);
            //        //comboBox_ActivityDistance_Alignment.Text = Alignment;

            //        //comboBox_ActivityDistance_Suffix.Text = Watch_Face.Activity.Distance.SuffixImageIndex.ToString();
            //        if (Watch_Face.Activity.Distance.SuffixImageIndex != null)
            //            comboBoxSetText(comboBox_ActivityDistance_Suffix_km, (long)Watch_Face.Activity.Distance.SuffixImageIndex);
            //        if (Watch_Face.Activity.Distance.Color != null)
            //        {
            //            Color new_color = ColorRead(Watch_Face.Activity.Distance.Color);
            //            comboBoxSetText(comboBox_ActivityDistance_Suffix_ml, new_color.B);
            //        }
            //            //comboBox_ActivityDistance_Decimal.Text = Watch_Face.Activity.Distance.DecimalPointImageIndex.ToString();
            //            if (Watch_Face.Activity.Distance.DecimalPointImageIndex != null)
            //            comboBoxSetText(comboBox_ActivityDistance_Decimal, (long)Watch_Face.Activity.Distance.DecimalPointImageIndex);
            //    }
            //    else checkBox_ActivityDistance.Checked = false;

            //    if (Watch_Face.Activity.HeartRate != null)
            //    {
            //        checkBox_ActivityPuls.Checked = true;
            //        numericUpDown_ActivityPuls_StartCorner_X.Value = Watch_Face.Activity.HeartRate.TopLeftX;
            //        numericUpDown_ActivityPuls_StartCorner_Y.Value = Watch_Face.Activity.HeartRate.TopLeftY;
            //        numericUpDown_ActivityPuls_EndCorner_X.Value = Watch_Face.Activity.HeartRate.BottomRightX;
            //        numericUpDown_ActivityPuls_EndCorner_Y.Value = Watch_Face.Activity.HeartRate.BottomRightY;

            //        //comboBox_ActivityPuls_Image.Text = Watch_Face.Activity.HeartRate.ImageIndex.ToString();
            //        comboBoxSetText(comboBox_ActivityPuls_Image, Watch_Face.Activity.HeartRate.ImageIndex);
            //        numericUpDown_ActivityPuls_Count.Value = Watch_Face.Activity.HeartRate.ImagesCount;
            //        numericUpDown_ActivityPuls_Spacing.Value = Watch_Face.Activity.HeartRate.Spacing;
            //        AlignmentToString(comboBox_ActivityPuls_Alignment, Watch_Face.Activity.HeartRate.Alignment);
            //        //comboBox_ActivityPuls_Alignment.Text = Alignment;
            //    }
            //    else checkBox_ActivityPuls.Checked = false;

            //    if (Watch_Face.Activity.PulseMeter != null)
            //    {
            //        checkBox_ActivityPulsScale.Checked = true;
            //        numericUpDown_ActivityPulsScale_Center_X.Value = Watch_Face.Activity.PulseMeter.CenterX;
            //        numericUpDown_ActivityPulsScale_Center_Y.Value = Watch_Face.Activity.PulseMeter.CenterY;
            //        numericUpDown_ActivityPulsScale_Radius_X.Value = Watch_Face.Activity.PulseMeter.RadiusX;
            //        numericUpDown_ActivityPulsScale_Radius_Y.Value = Watch_Face.Activity.PulseMeter.RadiusY;
            //        if (Watch_Face.Activity.PulseMeter.RadiusY == 0)
            //            numericUpDown_ActivityPulsScale_Radius_Y.Value = Watch_Face.Activity.PulseMeter.RadiusX;


            //        numericUpDown_ActivityPulsScale_StartAngle.Value = Watch_Face.Activity.PulseMeter.StartAngle;
            //        numericUpDown_ActivityPulsScale_EndAngle.Value = Watch_Face.Activity.PulseMeter.EndAngle;
            //        numericUpDown_ActivityPulsScale_Width.Value = Watch_Face.Activity.PulseMeter.Width;

            //        Color new_color = ColorRead(Watch_Face.Activity.PulseMeter.Color);
            //        comboBox_ActivityPulsScale_Color.BackColor = new_color;
            //        colorDialog_Pulse.Color = new_color;

            //        switch (Watch_Face.Activity.PulseMeter.Flatness)
            //        {
            //            case 90:
            //                //comboBox_Battery_Flatness.Text = "Треугольное";
            //                comboBox_ActivityPulsScale_Flatness.SelectedIndex = 1;
            //                break;
            //            case 180:
            //                //comboBox_Battery_Flatness.Text = "Плоское";
            //                comboBox_ActivityPulsScale_Flatness.SelectedIndex = 2;
            //                break;
            //            default:
            //                //comboBox_Battery_Flatness.Text = "Круглое";
            //                comboBox_ActivityPulsScale_Flatness.SelectedIndex = 0;
            //                break;
            //        }

            //        if (Watch_Face.Activity.PulseMeter.ImageIndex != null)
            //        {
            //            comboBoxSetText(comboBox_ActivityPulsScale_Image, (long)Watch_Face.Activity.PulseMeter.ImageIndex);
            //            int x = 0;
            //            int y = 0;
            //            ColorToCoodinates(new_color, out x, out y);
            //            numericUpDown_ActivityPulsScale_ImageX.Value = x;
            //            numericUpDown_ActivityPulsScale_ImageY.Value = y;
            //            //ColorToCoodinates(new_color, numericUpDown_ActivityPulsScale_ImageX,
            //            //    numericUpDown_ActivityPulsScale_ImageY);
            //            checkBox_ActivityPulsScale_Image.Checked = true;
            //        }
            //        else checkBox_ActivityPulsScale_Image.Checked = false;
            //    }
            //    else checkBox_ActivityPulsScale.Checked = false;

            //    if ((Watch_Face.Activity.PulseGraph != null) && 
            //        (Watch_Face.Activity.PulseGraph.WeekDayClockHand != null) &&
            //        (Watch_Face.Activity.PulseGraph.WeekDayClockHand.Image != null))
            //    {
            //        checkBox_Pulse_ClockHand.Checked = true;
            //        numericUpDown_Pulse_ClockHand_X.Value = Watch_Face.Activity.PulseGraph.WeekDayClockHand.Image.X;
            //        numericUpDown_Pulse_ClockHand_Y.Value = Watch_Face.Activity.PulseGraph.WeekDayClockHand.Image.Y;
            //        comboBoxSetText(comboBox_Pulse_ClockHand_Image, Watch_Face.Activity.PulseGraph.WeekDayClockHand.Image.ImageIndex);
            //        if (Watch_Face.Activity.PulseGraph.WeekDayClockHand.CenterOffset != null)
            //        {
            //            numericUpDown_Pulse_ClockHand_Offset_X.Value = Watch_Face.Activity.PulseGraph.WeekDayClockHand.CenterOffset.X;
            //            numericUpDown_Pulse_ClockHand_Offset_Y.Value = Watch_Face.Activity.PulseGraph.WeekDayClockHand.CenterOffset.Y;

            //        }
            //        if (Watch_Face.Activity.PulseGraph.WeekDayClockHand.Sector != null)
            //        {
            //            numericUpDown_Pulse_ClockHand_StartAngle.Value = (int)(Watch_Face.Activity.PulseGraph.WeekDayClockHand.Sector.StartAngle / 100);
            //            numericUpDown_Pulse_ClockHand_EndAngle.Value = (int)(Watch_Face.Activity.PulseGraph.WeekDayClockHand.Sector.EndAngle / 100);

            //        }
            //    }
            //    else checkBox_Pulse_ClockHand.Checked = false;

            //    if ((Watch_Face.Activity.ColouredSquares != null) && 
            //        (Watch_Face.Activity.ColouredSquares.Coordinates != null))
            //    {
            //        checkBox_ActivityPuls_IconSet.Checked = true;
            //        dataGridView_ActivityPuls_IconSet.Rows.Clear();
            //        comboBoxSetText(comboBox_ActivityPuls_IconSet_Image, Watch_Face.Activity.ColouredSquares.ImageIndex);
            //        foreach (Coordinates coordinates in Watch_Face.Activity.ColouredSquares.Coordinates)
            //        {
            //            var RowNew = new DataGridViewRow();
            //            dataGridView_ActivityPuls_IconSet.Rows.Add(coordinates.X, coordinates.Y);
            //        }
            //    }
            //    else checkBox_ActivityPuls_IconSet.Checked = false;

            //    if (Watch_Face.Activity.Calories != null)
            //    {
            //        checkBox_ActivityCalories.Checked = true;
            //        numericUpDown_ActivityCalories_StartCorner_X.Value = Watch_Face.Activity.Calories.TopLeftX;
            //        numericUpDown_ActivityCalories_StartCorner_Y.Value = Watch_Face.Activity.Calories.TopLeftY;
            //        numericUpDown_ActivityCalories_EndCorner_X.Value = Watch_Face.Activity.Calories.BottomRightX;
            //        numericUpDown_ActivityCalories_EndCorner_Y.Value = Watch_Face.Activity.Calories.BottomRightY;

            //        //comboBox_ActivityCalories_Image.Text = Watch_Face.Activity.Calories.ImageIndex.ToString();
            //        comboBoxSetText(comboBox_ActivityCalories_Image, Watch_Face.Activity.Calories.ImageIndex);
            //        numericUpDown_ActivityCalories_Count.Value = Watch_Face.Activity.Calories.ImagesCount;
            //        numericUpDown_ActivityCalories_Spacing.Value = Watch_Face.Activity.Calories.Spacing;
            //        AlignmentToString(comboBox_ActivityCalories_Alignment, Watch_Face.Activity.Calories.Alignment);
            //        //comboBox_ActivityCalories_Alignment.Text = Alignment;
            //    }
            //    else checkBox_ActivityCalories.Checked = false;

            //    if (Watch_Face.Activity.CaloriesGraph != null && Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar != null)
            //    {
            //        checkBox_ActivityCaloriesScale.Checked = true;
            //        numericUpDown_ActivityCaloriesScale_Center_X.Value = Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.CenterX;
            //        numericUpDown_ActivityCaloriesScale_Center_Y.Value = Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.CenterY;
            //        numericUpDown_ActivityCaloriesScale_Radius_X.Value = Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.RadiusX;
            //        numericUpDown_ActivityCaloriesScale_Radius_Y.Value = Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.RadiusY;
            //        if (Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.RadiusY == 0)
            //            numericUpDown_ActivityCaloriesScale_Radius_Y.Value = Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.RadiusX;

            //        numericUpDown_ActivityCaloriesScale_StartAngle.Value = Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.StartAngle;
            //        numericUpDown_ActivityCaloriesScale_EndAngle.Value = Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.EndAngle;
            //        numericUpDown_ActivityCaloriesScale_Width.Value = Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.Width;

            //        Color new_color = ColorRead(Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.Color);
            //        comboBox_ActivityCaloriesScale_Color.BackColor = new_color;
            //        colorDialog_Calories.Color = new_color;

            //        switch (Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.Flatness)
            //        {
            //            case 90:
            //                //comboBox_Battery_Flatness.Text = "Треугольное";
            //                comboBox_ActivityCaloriesScale_Flatness.SelectedIndex = 1;
            //                break;
            //            case 180:
            //                //comboBox_Battery_Flatness.Text = "Плоское";
            //                comboBox_ActivityCaloriesScale_Flatness.SelectedIndex = 2;
            //                break;
            //            default:
            //                //comboBox_Battery_Flatness.Text = "Круглое";
            //                comboBox_ActivityCaloriesScale_Flatness.SelectedIndex = 0;
            //                break;
            //        }

            //        if (Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.ImageIndex != null)
            //        {
            //            comboBoxSetText(comboBox_ActivityCaloriesScale_Image, 
            //                (long)Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.ImageIndex);
            //            int x = 0;
            //            int y = 0;
            //            ColorToCoodinates(new_color, out x, out y);
            //            numericUpDown_ActivityCaloriesScale_ImageX.Value = x;
            //            numericUpDown_ActivityCaloriesScale_ImageY.Value = y;
            //            //ColorToCoodinates(new_color, numericUpDown_ActivityCaloriesScale_ImageX,
            //            //    numericUpDown_ActivityCaloriesScale_ImageY);
            //            checkBox_ActivityCaloriesScale_Image.Checked = true;
            //        }
            //        else checkBox_ActivityCaloriesScale_Image.Checked = false;
            //    }
            //    else checkBox_ActivityCaloriesScale.Checked = false;

            //    if ((Watch_Face.Activity.CaloriesGraph != null) &&
            //        (Watch_Face.Activity.CaloriesGraph.WeekDayClockHand != null) &&
            //        (Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.Image != null))
            //    {
            //        checkBox_Calories_ClockHand.Checked = true;
            //        numericUpDown_Calories_ClockHand_X.Value = Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.Image.X;
            //        numericUpDown_Calories_ClockHand_Y.Value = Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.Image.Y;
            //        comboBoxSetText(comboBox_Calories_ClockHand_Image, Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.Image.ImageIndex);
            //        if (Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.CenterOffset != null)
            //        {
            //            numericUpDown_Calories_ClockHand_Offset_X.Value = Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.CenterOffset.X;
            //            numericUpDown_Calories_ClockHand_Offset_Y.Value = Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.CenterOffset.Y;

            //        }
            //        if (Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.Sector != null)
            //        {
            //            numericUpDown_Calories_ClockHand_StartAngle.Value = (int)(Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.Sector.StartAngle / 100);
            //            numericUpDown_Calories_ClockHand_EndAngle.Value = (int)(Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.Sector.EndAngle / 100);

            //        }
            //    }
            //    else checkBox_Calories_ClockHand.Checked = false;

            //    if (Watch_Face.Activity.StarImage != null)
            //    {
            //        checkBox_ActivityStar.Checked = true;
            //        numericUpDown_ActivityStar_X.Value = Watch_Face.Activity.StarImage.X;
            //        numericUpDown_ActivityStar_Y.Value = Watch_Face.Activity.StarImage.Y;
            //        //comboBox_ActivityStar_Image.Text = Watch_Face.Activity.StarImage.ImageIndex.ToString();
            //        comboBoxSetText(comboBox_ActivityStar_Image, Watch_Face.Activity.StarImage.ImageIndex);
            //    }
            //    else checkBox_ActivityStar.Checked = false;

            //    if (Watch_Face.Activity.NoDataImageIndex != null)
            //    {
            //        comboBoxSetText(comboBox_Activity_NDImage, (long)Watch_Face.Activity.NoDataImageIndex);
            //    }

            //}
            //else
            //{
            //    checkBox_Activity.Checked = false;
            //    checkBox_ActivitySteps.Checked = false;
            //    checkBox_ActivityDistance.Checked = false;
            //    checkBox_ActivityPuls.Checked = false;
            //    checkBox_ActivityPulsScale.Checked = false;
            //    checkBox_ActivityPulsScale_Image.Checked = false;
            //    checkBox_ActivityPuls_IconSet.Checked = false;
            //    checkBox_ActivityCalories.Checked = false;
            //    checkBox_ActivityCaloriesScale.Checked = false;
            //    checkBox_ActivityCaloriesScale_Image.Checked = false;
            //    checkBox_ActivityStar.Checked = false;
            //}
            //#endregion

            //#region Status
            //if (Watch_Face.Status != null)
            //{
            //    if (Watch_Face.Status.Bluetooth != null)
            //    {
            //        checkBox_Bluetooth.Checked = true;
            //        if (Watch_Face.Status.Bluetooth.Coordinates != null)
            //        {
            //            numericUpDown_Bluetooth_X.Value = Watch_Face.Status.Bluetooth.Coordinates.X;
            //            numericUpDown_Bluetooth_Y.Value = Watch_Face.Status.Bluetooth.Coordinates.Y;
            //        }
            //        if (Watch_Face.Status.Bluetooth.ImageIndexOn != null)
            //            //comboBox_Bluetooth_On.Text = Watch_Face.Status.Bluetooth.ImageIndexOn.Value.ToString();
            //            comboBoxSetText(comboBox_Bluetooth_On, (long)Watch_Face.Status.Bluetooth.ImageIndexOn);
            //        if (Watch_Face.Status.Bluetooth.ImageIndexOff != null)
            //            //comboBox_Bluetooth_Off.Text = Watch_Face.Status.Bluetooth.ImageIndexOff.Value.ToString();
            //            comboBoxSetText(comboBox_Bluetooth_Off, (long)Watch_Face.Status.Bluetooth.ImageIndexOff);
            //    }
            //    else checkBox_Bluetooth.Checked = false;

            //    if (Watch_Face.Status.Alarm != null)
            //    {
            //        checkBox_Alarm.Checked = true;
            //        if (Watch_Face.Status.Alarm.Coordinates != null)
            //        {
            //            numericUpDown_Alarm_X.Value = Watch_Face.Status.Alarm.Coordinates.X;
            //            numericUpDown_Alarm_Y.Value = Watch_Face.Status.Alarm.Coordinates.Y;
            //        }
            //        if (Watch_Face.Status.Alarm.ImageIndexOn != null)
            //            //comboBox_Alarm_On.Text = Watch_Face.Status.Alarm.ImageIndexOn.Value.ToString();
            //            comboBoxSetText(comboBox_Alarm_On, (long)Watch_Face.Status.Alarm.ImageIndexOn);
            //        if (Watch_Face.Status.Alarm.ImageIndexOff != null)
            //            //comboBox_Alarm_Off.Text = Watch_Face.Status.Alarm.ImageIndexOff.Value.ToString();
            //            comboBoxSetText(comboBox_Alarm_Off, (long)Watch_Face.Status.Alarm.ImageIndexOff);
            //    }
            //    else checkBox_Alarm.Checked = false;

            //    if (Watch_Face.Status.Lock != null)
            //    {
            //        checkBox_Lock.Checked = true;
            //        if (Watch_Face.Status.Lock.Coordinates != null)
            //        {
            //            numericUpDown_Lock_X.Value = Watch_Face.Status.Lock.Coordinates.X;
            //            numericUpDown_Lock_Y.Value = Watch_Face.Status.Lock.Coordinates.Y;
            //        }
            //        if (Watch_Face.Status.Lock.ImageIndexOn != null)
            //            //comboBox_Lock_On.Text = Watch_Face.Status.Lock.ImageIndexOn.Value.ToString();
            //            comboBoxSetText(comboBox_Lock_On, (long)Watch_Face.Status.Lock.ImageIndexOn);
            //        if (Watch_Face.Status.Lock.ImageIndexOff != null)
            //            //comboBox_Lock_Off.Text = Watch_Face.Status.Lock.ImageIndexOff.Value.ToString();
            //            comboBoxSetText(comboBox_Lock_Off, (long)Watch_Face.Status.Lock.ImageIndexOff);
            //    }
            //    else checkBox_Lock.Checked = false;

            //    if (Watch_Face.Status.DoNotDisturb != null)
            //    {
            //        checkBox_DND.Checked = true;
            //        if (Watch_Face.Status.DoNotDisturb.Coordinates != null)
            //        {
            //            numericUpDown_DND_X.Value = Watch_Face.Status.DoNotDisturb.Coordinates.X;
            //            numericUpDown_DND_Y.Value = Watch_Face.Status.DoNotDisturb.Coordinates.Y;
            //        }
            //        if (Watch_Face.Status.DoNotDisturb.ImageIndexOn != null)
            //            //comboBox_DND_On.Text = Watch_Face.Status.DoNotDisturb.ImageIndexOn.Value.ToString();
            //            comboBoxSetText(comboBox_DND_On, (long)Watch_Face.Status.DoNotDisturb.ImageIndexOn);
            //        if (Watch_Face.Status.DoNotDisturb.ImageIndexOff != null)
            //            //comboBox_DND_Off.Text = Watch_Face.Status.DoNotDisturb.ImageIndexOff.Value.ToString();
            //            comboBoxSetText(comboBox_DND_Off, (long)Watch_Face.Status.DoNotDisturb.ImageIndexOff);
            //    }
            //    else checkBox_DND.Checked = false;
            //}
            //else
            //{
            //    checkBox_Bluetooth.Checked = false;
            //    checkBox_Alarm.Checked = false;
            //    checkBox_Lock.Checked = false;
            //    checkBox_DND.Checked = false;
            //}
            //#endregion

            //#region Battery
            //if (Watch_Face.Battery != null)
            //{
            //    checkBox_Battery.Checked = true;
            //    if (Watch_Face.Battery.Text != null)
            //    {
            //        checkBox_Battery_Text.Checked = true;
            //        numericUpDown_Battery_Text_StartCorner_X.Value = Watch_Face.Battery.Text.TopLeftX;
            //        numericUpDown_Battery_Text_StartCorner_Y.Value = Watch_Face.Battery.Text.TopLeftY;
            //        numericUpDown_Battery_Text_EndCorner_X.Value = Watch_Face.Battery.Text.BottomRightX;
            //        numericUpDown_Battery_Text_EndCorner_Y.Value = Watch_Face.Battery.Text.BottomRightY;
            //        numericUpDown_Battery_Text_Spacing.Value = Watch_Face.Battery.Text.Spacing;
            //        numericUpDown_Battery_Text_Count.Value = Watch_Face.Battery.Text.ImagesCount;
            //        //comboBox_Battery_Text_Image.Text = Watch_Face.Battery.Text.ImageIndex.ToString();
            //        comboBoxSetText(comboBox_Battery_Text_Image, Watch_Face.Battery.Text.ImageIndex);
            //        AlignmentToString(comboBox_Battery_Text_Alignment, Watch_Face.Battery.Text.Alignment);
            //        //comboBox_Battery_Text_Alignment.Text = Alignment;
            //    }
            //    else checkBox_Battery_Text.Checked = false;

            //    if (Watch_Face.Battery.Images != null)
            //    {
            //        checkBox_Battery_Img.Checked = true;
            //        numericUpDown_Battery_Img_X.Value = Watch_Face.Battery.Images.X;
            //        numericUpDown_Battery_Img_Y.Value = Watch_Face.Battery.Images.Y;
            //        numericUpDown_Battery_Img_Count.Value = Watch_Face.Battery.Images.ImagesCount;
            //        //comboBox_Battery_Img_Image.Text = Watch_Face.Battery.Images.ImageIndex.ToString();
            //        comboBoxSetText(comboBox_Battery_Img_Image, Watch_Face.Battery.Images.ImageIndex);
            //    }
            //    else checkBox_Battery_Img.Checked = false;

            //    if ((Watch_Face.Battery.Unknown4 != null) && (Watch_Face.Battery.Unknown4.Image != null))
            //    {
            //        checkBox_Battery_ClockHand.Checked = true;
            //        numericUpDown_Battery_ClockHand_X.Value = Watch_Face.Battery.Unknown4.Image.X;
            //        numericUpDown_Battery_ClockHand_Y.Value = Watch_Face.Battery.Unknown4.Image.Y;
            //        comboBoxSetText(comboBox_Battery_ClockHand_Image, Watch_Face.Battery.Unknown4.Image.ImageIndex);
            //        if (Watch_Face.Battery.Unknown4.CenterOffset != null)
            //        {
            //            numericUpDown_Battery_ClockHand_Offset_X.Value = Watch_Face.Battery.Unknown4.CenterOffset.X;
            //            numericUpDown_Battery_ClockHand_Offset_Y.Value = Watch_Face.Battery.Unknown4.CenterOffset.Y;

            //        }
            //        if (Watch_Face.Battery.Unknown4.Sector != null)
            //        {
            //            numericUpDown_Battery_ClockHand_StartAngle.Value = (int)(Watch_Face.Battery.Unknown4.Sector.StartAngle/100);
            //            numericUpDown_Battery_ClockHand_EndAngle.Value = (int)(Watch_Face.Battery.Unknown4.Sector.EndAngle/100);

            //        }
            //    }
            //    else checkBox_Battery_ClockHand.Checked = false;

            //    if (Watch_Face.Battery.Percent != null)
            //    {
            //        checkBox_Battery_Percent.Checked = true;
            //        numericUpDown_Battery_Percent_X.Value = Watch_Face.Battery.Percent.X;
            //        numericUpDown_Battery_Percent_Y.Value = Watch_Face.Battery.Percent.Y;
            //        //comboBox_Battery_Percent_Image.Text = Watch_Face.Battery.Percent.ImageIndex.ToString();
            //        comboBoxSetText(comboBox_Battery_Percent_Image, Watch_Face.Battery.Percent.ImageIndex);
            //    }
            //    else checkBox_Battery_Percent.Checked = false;

            //    if (Watch_Face.Battery.Scale != null)
            //    {
            //        checkBox_Battery_Scale.Checked = true;
            //        numericUpDown_Battery_Scale_Center_X.Value = Watch_Face.Battery.Scale.CenterX;
            //        numericUpDown_Battery_Scale_Center_Y.Value = Watch_Face.Battery.Scale.CenterY;
            //        numericUpDown_Battery_Scale_Radius_X.Value = Watch_Face.Battery.Scale.RadiusX;
            //        numericUpDown_Battery_Scale_Radius_Y.Value = Watch_Face.Battery.Scale.RadiusY;
            //        if (Watch_Face.Battery.Scale.RadiusY == 0)
            //            numericUpDown_Battery_Scale_Radius_Y.Value = Watch_Face.Battery.Scale.RadiusX;

            //        numericUpDown_Battery_Scale_StartAngle.Value = Watch_Face.Battery.Scale.StartAngle;
            //        numericUpDown_Battery_Scale_EndAngle.Value = Watch_Face.Battery.Scale.EndAngle;
            //        numericUpDown_Battery_Scale_Width.Value = Watch_Face.Battery.Scale.Width;

            //        Color new_color = ColorRead(Watch_Face.Battery.Scale.Color);
            //        comboBox_Battery_Scale_Color.BackColor = new_color;
            //        colorDialog_Battery.Color = new_color;

            //        switch (Watch_Face.Battery.Scale.Flatness)
            //        {
            //            case 90:
            //                //comboBox_Battery_Flatness.Text = "Треугольное";
            //                comboBox_Battery_Flatness.SelectedIndex = 1;
            //                break;
            //            case 180:
            //                //comboBox_Battery_Flatness.Text = "Плоское";
            //                comboBox_Battery_Flatness.SelectedIndex = 2;
            //                break;
            //            default:
            //                //comboBox_Battery_Flatness.Text = "Круглое";
            //                comboBox_Battery_Flatness.SelectedIndex = 0;
            //                break;
            //        }

            //        if (Watch_Face.Battery.Scale.ImageIndex != null)
            //        {
            //            comboBoxSetText(comboBox_Battery_Scale_Image, (long)Watch_Face.Battery.Scale.ImageIndex);
            //            int x = 0;
            //            int y = 0;
            //            ColorToCoodinates(new_color, out x, out y);
            //            numericUpDown_Battery_Scale_ImageX.Value = x;
            //            numericUpDown_Battery_Scale_ImageY.Value = y;
            //            //ColorToCoodinates(new_color, numericUpDown_Battery_Scale_ImageX,
            //            //    numericUpDown_Battery_Scale_ImageY);
            //            checkBox_Battery_Scale_Image.Checked = true;
            //        }
            //        else checkBox_Battery_Scale_Image.Checked = false;
            //    }
            //    else checkBox_Battery_Scale.Checked = false;

            //    if ((Watch_Face.Battery.Icons != null) && (Watch_Face.Battery.Icons.Coordinates != null))
            //    {
            //        checkBox_Battery_IconSet.Checked = true;
            //        dataGridView_Battery_IconSet.Rows.Clear();
            //        comboBoxSetText(comboBox_Battery_IconSet_Image, Watch_Face.Battery.Icons.ImageIndex);
            //        foreach (Coordinates coordinates in Watch_Face.Battery.Icons.Coordinates)
            //        {
            //            var RowNew = new DataGridViewRow();
            //            dataGridView_Battery_IconSet.Rows.Add(coordinates.X, coordinates.Y);
            //        }
            //    }
            //    else checkBox_Battery_IconSet.Checked = false;
            //}
            //else
            //{
            //    checkBox_Battery.Checked = false;
            //    checkBox_Battery_Text.Checked = false;
            //    checkBox_Battery_Img.Checked = false;
            //    checkBox_Battery_ClockHand.Checked = false;
            //    checkBox_Battery_Percent.Checked = false;
            //    checkBox_Battery_Scale.Checked = false;
            //    checkBox_Battery_Scale_Image.Checked = false;
            //    checkBox_Battery_IconSet.Checked = false;
            //}
            //#endregion

            //#region MonthClockHand
            //if (Watch_Face.MonthClockHand != null)
            //{
            //    checkBox_AnalogClock.Checked = true;
            //    if ((Watch_Face.MonthClockHand.Hours != null) && (Watch_Face.MonthClockHand.Hours.Image != null))
            //    {
            //        checkBox_AnalogClock_Hour.Checked = true;
            //        numericUpDown_AnalogClock_Hour_X.Value = Watch_Face.MonthClockHand.Hours.Image.X;
            //        numericUpDown_AnalogClock_Hour_Y.Value = Watch_Face.MonthClockHand.Hours.Image.Y;
            //        //comboBox_AnalogClock_Hour_Image.Text = Watch_Face.MonthClockHand.Hours.Image.ImageIndex.ToString();
            //        comboBoxSetText(comboBox_AnalogClock_Hour_Image, Watch_Face.MonthClockHand.Hours.Image.ImageIndex);

            //        if (Watch_Face.MonthClockHand.Hours.CenterOffset != null)
            //        {
            //            numericUpDown_AnalogClock_Hour_Offset_X.Value = Watch_Face.MonthClockHand.Hours.CenterOffset.X;
            //            numericUpDown_AnalogClock_Hour_Offset_Y.Value = Watch_Face.MonthClockHand.Hours.CenterOffset.Y;

            //        }
            //    }
            //    else checkBox_AnalogClock_Hour.Checked = false;

            //    if ((Watch_Face.MonthClockHand.Minutes != null) && (Watch_Face.MonthClockHand.Minutes.Image != null))
            //    {
            //        checkBox_AnalogClock_Min.Checked = true;
            //        numericUpDown_AnalogClock_Min_X.Value = Watch_Face.MonthClockHand.Minutes.Image.X;
            //        numericUpDown_AnalogClock_Min_Y.Value = Watch_Face.MonthClockHand.Minutes.Image.Y;
            //        //comboBox_AnalogClock_Min_Image.Text = Watch_Face.MonthClockHand.Minutes.Image.ImageIndex.ToString();
            //        comboBoxSetText(comboBox_AnalogClock_Min_Image, Watch_Face.MonthClockHand.Minutes.Image.ImageIndex);

            //        if (Watch_Face.MonthClockHand.Minutes.CenterOffset != null)
            //        {
            //            numericUpDown_AnalogClock_Min_Offset_X.Value = Watch_Face.MonthClockHand.Minutes.CenterOffset.X;
            //            numericUpDown_AnalogClock_Min_Offset_Y.Value = Watch_Face.MonthClockHand.Minutes.CenterOffset.Y;

            //        }
            //    }
            //    else checkBox_AnalogClock_Min.Checked = false;

            //    if ((Watch_Face.MonthClockHand.Seconds != null) && (Watch_Face.MonthClockHand.Seconds.Image != null))
            //    {
            //        checkBox_AnalogClock_Sec.Checked = true;
            //        numericUpDown_AnalogClock_Sec_X.Value = Watch_Face.MonthClockHand.Seconds.Image.X;
            //        numericUpDown_AnalogClock_Sec_Y.Value = Watch_Face.MonthClockHand.Seconds.Image.Y;
            //        //comboBox_AnalogClock_Sec_Image.Text = Watch_Face.MonthClockHand.Seconds.Image.ImageIndex.ToString();
            //        comboBoxSetText(comboBox_AnalogClock_Sec_Image, Watch_Face.MonthClockHand.Seconds.Image.ImageIndex);

            //        if (Watch_Face.MonthClockHand.Seconds.CenterOffset != null)
            //        {
            //            numericUpDown_AnalogClock_Sec_Offset_X.Value = Watch_Face.MonthClockHand.Seconds.CenterOffset.X;
            //            numericUpDown_AnalogClock_Sec_Offset_Y.Value = Watch_Face.MonthClockHand.Seconds.CenterOffset.Y;

            //        }
            //    }
            //    else checkBox_AnalogClock_Sec.Checked = false;

            //    if (Watch_Face.MonthClockHand.HourCenterImage != null)
            //    {
            //        checkBox_HourCenterImage.Checked = true;
            //        numericUpDown_HourCenterImage_X.Value = Watch_Face.MonthClockHand.HourCenterImage.X;
            //        numericUpDown_HourCenterImage_Y.Value = Watch_Face.MonthClockHand.HourCenterImage.Y;
            //        //comboBox_HourCenterImage_Image.Text = Watch_Face.MonthClockHand.HourCenterImage.ImageIndex.ToString();
            //        comboBoxSetText(comboBox_HourCenterImage_Image, Watch_Face.MonthClockHand.HourCenterImage.ImageIndex);
            //    }
            //    else checkBox_HourCenterImage.Checked = false;

            //    if (Watch_Face.MonthClockHand.MinCenterImage != null)
            //    {
            //        checkBox_MinCenterImage.Checked = true;
            //        numericUpDown_MinCenterImage_X.Value = Watch_Face.MonthClockHand.MinCenterImage.X;
            //        numericUpDown_MinCenterImage_Y.Value = Watch_Face.MonthClockHand.MinCenterImage.Y;
            //        //comboBox_MinCenterImage_Image.Text = Watch_Face.MonthClockHand.MinCenterImage.ImageIndex.ToString();
            //        comboBoxSetText(comboBox_MinCenterImage_Image, Watch_Face.MonthClockHand.MinCenterImage.ImageIndex);
            //    }
            //    else checkBox_MinCenterImage.Checked = false;

            //    if (Watch_Face.MonthClockHand.SecCenterImage != null)
            //    {
            //        checkBox_SecCenterImage.Checked = true;
            //        numericUpDown_SecCenterImage_X.Value = Watch_Face.MonthClockHand.SecCenterImage.X;
            //        numericUpDown_SecCenterImage_Y.Value = Watch_Face.MonthClockHand.SecCenterImage.Y;
            //        //comboBox_SecCenterImage_Image.Text = Watch_Face.MonthClockHand.SecCenterImage.ImageIndex.ToString();
            //        comboBoxSetText(comboBox_SecCenterImage_Image, Watch_Face.MonthClockHand.SecCenterImage.ImageIndex);
            //    }
            //    else checkBox_SecCenterImage.Checked = false;
            //}
            //else
            //{
            //    checkBox_AnalogClock.Checked = false;
            //    checkBox_AnalogClock_Hour.Checked = false;
            //    checkBox_AnalogClock_Min.Checked = false;
            //    checkBox_AnalogClock_Sec.Checked = false;

            //    checkBox_HourCenterImage.Checked = false;
            //    checkBox_MinCenterImage.Checked = false;
            //    checkBox_SecCenterImage.Checked = false;
            //}
            //#endregion

            //#region Weather
            //if (Watch_Face.Weather != null)
            //{
            //    checkBox_Weather.Checked = true;
            //    if ((Watch_Face.Weather.Temperature != null) && (Watch_Face.Weather.Temperature.Current != null))
            //    {
            //        checkBox_Weather_Text.Checked = true;
            //        numericUpDown_Weather_Text_StartCorner_X.Value = Watch_Face.Weather.Temperature.Current.TopLeftX;
            //        numericUpDown_Weather_Text_StartCorner_Y.Value = Watch_Face.Weather.Temperature.Current.TopLeftY;
            //        numericUpDown_Weather_Text_EndCorner_X.Value = Watch_Face.Weather.Temperature.Current.BottomRightX;
            //        numericUpDown_Weather_Text_EndCorner_Y.Value = Watch_Face.Weather.Temperature.Current.BottomRightY;

            //        numericUpDown_Weather_Text_Spacing.Value = Watch_Face.Weather.Temperature.Current.Spacing;
            //        numericUpDown_Weather_Text_Count.Value = Watch_Face.Weather.Temperature.Current.ImagesCount;
            //        //comboBox_Weather_Text_Image.Text = Watch_Face.Weather.Temperature.Current.ImageIndex.ToString();
            //        comboBoxSetText(comboBox_Weather_Text_Image, Watch_Face.Weather.Temperature.Current.ImageIndex);
            //        AlignmentToString(comboBox_Weather_Text_Alignment, Watch_Face.Weather.Temperature.Current.Alignment);
            //        //comboBox_Weather_Text_Alignment.Text = Alignment;
            //    }
            //    else checkBox_Weather_Text.Checked = false;

            //    if ((Watch_Face.Weather.Temperature != null) && (Watch_Face.Weather.Temperature.Today != null))
            //    {
            //        if ((Watch_Face.Weather.Temperature.Today.Separate != null) &&
            //            (Watch_Face.Weather.Temperature.Today.Separate.Day != null))
            //        {
            //            checkBox_Weather_Day.Checked = true;
            //            numericUpDown_Weather_Day_StartCorner_X.Value =
            //                Watch_Face.Weather.Temperature.Today.Separate.Day.TopLeftX;
            //            numericUpDown_Weather_Day_StartCorner_Y.Value =
            //                Watch_Face.Weather.Temperature.Today.Separate.Day.TopLeftY;
            //            numericUpDown_Weather_Day_EndCorner_X.Value =
            //                Watch_Face.Weather.Temperature.Today.Separate.Day.BottomRightX;
            //            numericUpDown_Weather_Day_EndCorner_Y.Value =
            //                Watch_Face.Weather.Temperature.Today.Separate.Day.BottomRightY;

            //            numericUpDown_Weather_Day_Spacing.Value =
            //                Watch_Face.Weather.Temperature.Today.Separate.Day.Spacing;
            //            numericUpDown_Weather_Day_Count.Value =
            //                Watch_Face.Weather.Temperature.Today.Separate.Day.ImagesCount;
            //            //comboBox_Weather_Day_Image.Text =
            //            //Watch_Face.Weather.Temperature.Today.Separate.Day.ImageIndex.ToString();
            //            comboBoxSetText(comboBox_Weather_Day_Image,
            //                Watch_Face.Weather.Temperature.Today.Separate.Day.ImageIndex);
            //            AlignmentToString(comboBox_Weather_Day_Alignment, Watch_Face.Weather.Temperature.Today.Separate.Day.Alignment);
            //            //comboBox_Weather_Day_Alignment.Text = Alignment;
            //        }
            //        else checkBox_Weather_Day.Checked = false;

            //        if ((Watch_Face.Weather.Temperature.Today.Separate != null) &&
            //            (Watch_Face.Weather.Temperature.Today.Separate.Night != null))
            //        {
            //            checkBox_Weather_Night.Checked = true;
            //            numericUpDown_Weather_Night_StartCorner_X.Value =
            //                Watch_Face.Weather.Temperature.Today.Separate.Night.TopLeftX;
            //            numericUpDown_Weather_Night_StartCorner_Y.Value =
            //                Watch_Face.Weather.Temperature.Today.Separate.Night.TopLeftY;
            //            numericUpDown_Weather_Night_EndCorner_X.Value =
            //                Watch_Face.Weather.Temperature.Today.Separate.Night.BottomRightX;
            //            numericUpDown_Weather_Night_EndCorner_Y.Value =
            //                Watch_Face.Weather.Temperature.Today.Separate.Night.BottomRightY;

            //            numericUpDown_Weather_Night_Spacing.Value =
            //                Watch_Face.Weather.Temperature.Today.Separate.Night.Spacing;
            //            numericUpDown_Weather_Night_Count.Value =
            //                Watch_Face.Weather.Temperature.Today.Separate.Night.ImagesCount;
            //            //comboBox_Weather_Night_Image.Text =
            //            //    Watch_Face.Weather.Temperature.Today.Separate.Night.ImageIndex.ToString();
            //            comboBoxSetText(comboBox_Weather_Night_Image,
            //                Watch_Face.Weather.Temperature.Today.Separate.Night.ImageIndex);
            //            AlignmentToString(comboBox_Weather_Night_Alignment, Watch_Face.Weather.Temperature.Today.Separate.Night.Alignment);
            //            //comboBox_Weather_Night_Alignment.Text = Alignment;
            //        }
            //        else checkBox_Weather_Night.Checked = false;
            //    }
            //    else
            //    {
            //        checkBox_Weather_Day.Checked = false;
            //        checkBox_Weather_Night.Checked = false;
            //    }

            //    if ((Watch_Face.Weather.Temperature != null) && (Watch_Face.Weather.Temperature.Symbols != null))
            //    {
            //        //comboBox_Weather_Text_MinusImage.Text = Watch_Face.Weather.Temperature.Symbols.MinusImageIndex.ToString();
            //        comboBoxSetText(comboBox_Weather_Text_MinusImage, Watch_Face.Weather.Temperature.Symbols.MinusImageIndex);
            //        //comboBox_Weather_Text_DegImage.Text = Watch_Face.Weather.Temperature.Symbols.DegreesImageIndex.ToString();
            //        comboBoxSetText(comboBox_Weather_Text_DegImage, Watch_Face.Weather.Temperature.Symbols.DegreesImageIndex);
            //        //comboBox_Weather_Text_NDImage.Text = Watch_Face.Weather.Temperature.Symbols.NoDataImageIndex.ToString();
            //        comboBoxSetText(comboBox_Weather_Text_NDImage, Watch_Face.Weather.Temperature.Symbols.NoDataImageIndex);
            //    }

            //    if ((Watch_Face.Weather.Icon != null) && (Watch_Face.Weather.Icon.Images != null))
            //    {
            //        checkBox_Weather_Icon.Checked = true;
            //        numericUpDown_Weather_Icon_X.Value = Watch_Face.Weather.Icon.Images.X;
            //        numericUpDown_Weather_Icon_Y.Value = Watch_Face.Weather.Icon.Images.Y;

            //        numericUpDown_Weather_Icon_Count.Value = Watch_Face.Weather.Icon.Images.ImagesCount;
            //        //comboBox_Weather_Icon_Image.Text = Watch_Face.Weather.Icon.Images.ImageIndex.ToString();
            //        comboBoxSetText(comboBox_Weather_Icon_Image, Watch_Face.Weather.Icon.Images.ImageIndex);
            //        //comboBox_Weather_Icon_NDImage.Text = Watch_Face.Weather.Icon.NoWeatherImageIndex.ToString();
            //        comboBoxSetText(comboBox_Weather_Icon_NDImage, Watch_Face.Weather.Icon.NoWeatherImageIndex);
            //    }
            //    else checkBox_Weather_Icon.Checked = false;
            //}
            //else
            //{
            //    checkBox_Weather_Text.Checked = false;
            //    checkBox_Weather_Day.Checked = false;
            //    checkBox_Weather_Night.Checked = false;
            //    checkBox_Weather_Icon.Checked = false;
            //    checkBox_Weather.Checked = false;
            //}
            //#endregion

            //#region Shortcuts
            //if(Watch_Face.Shortcuts != null)
            //{
            //    checkBox_Shortcuts.Checked = true;
            //    if (Watch_Face.Shortcuts.State != null && Watch_Face.Shortcuts.State.Element != null)
            //    {
            //        checkBox_Shortcuts_Steps.Checked = true;
            //        numericUpDown_Shortcuts_Steps_X.Value = Watch_Face.Shortcuts.State.Element.TopLeftX;
            //        numericUpDown_Shortcuts_Steps_Y.Value = Watch_Face.Shortcuts.State.Element.TopLeftY;
            //        numericUpDown_Shortcuts_Steps_Width.Value = Watch_Face.Shortcuts.State.Element.Width;
            //        numericUpDown_Shortcuts_Steps_Height.Value = Watch_Face.Shortcuts.State.Element.Height;
            //    }
            //    else checkBox_Shortcuts_Steps.Checked = false;

            //    if (Watch_Face.Shortcuts.HeartRate != null && Watch_Face.Shortcuts.HeartRate.Element != null)
            //    {
            //        checkBox_Shortcuts_Puls.Checked = true;
            //        numericUpDown_Shortcuts_Puls_X.Value = Watch_Face.Shortcuts.HeartRate.Element.TopLeftX;
            //        numericUpDown_Shortcuts_Puls_Y.Value = Watch_Face.Shortcuts.HeartRate.Element.TopLeftY;
            //        numericUpDown_Shortcuts_Puls_Width.Value = Watch_Face.Shortcuts.HeartRate.Element.Width;
            //        numericUpDown_Shortcuts_Puls_Height.Value = Watch_Face.Shortcuts.HeartRate.Element.Height;
            //    }
            //    else checkBox_Shortcuts_Puls.Checked = false;

            //    if (Watch_Face.Shortcuts.Weather != null && Watch_Face.Shortcuts.Weather.Element != null)
            //    {
            //        checkBox_Shortcuts_Weather.Checked = true;
            //        numericUpDown_Shortcuts_Weather_X.Value = Watch_Face.Shortcuts.Weather.Element.TopLeftX;
            //        numericUpDown_Shortcuts_Weather_Y.Value = Watch_Face.Shortcuts.Weather.Element.TopLeftY;
            //        numericUpDown_Shortcuts_Weather_Width.Value = Watch_Face.Shortcuts.Weather.Element.Width;
            //        numericUpDown_Shortcuts_Weather_Height.Value = Watch_Face.Shortcuts.Weather.Element.Height;
            //    }
            //    else checkBox_Shortcuts_Weather.Checked = false;

            //    if (Watch_Face.Shortcuts.Unknown4 != null && Watch_Face.Shortcuts.Unknown4.Element != null)
            //    {
            //        checkBox_Shortcuts_Energy.Checked = true;
            //        numericUpDown_Shortcuts_Energy_X.Value = Watch_Face.Shortcuts.Unknown4.Element.TopLeftX;
            //        numericUpDown_Shortcuts_Energy_Y.Value = Watch_Face.Shortcuts.Unknown4.Element.TopLeftY;
            //        numericUpDown_Shortcuts_Energy_Width.Value = Watch_Face.Shortcuts.Unknown4.Element.Width;
            //        numericUpDown_Shortcuts_Energy_Height.Value = Watch_Face.Shortcuts.Unknown4.Element.Height;
            //    }
            //    else checkBox_Shortcuts_Energy.Checked = false;
            //}
            //else
            //{
            //    checkBox_Shortcuts.Checked = false;
            //    checkBox_Shortcuts_Steps.Checked = false;
            //    checkBox_Shortcuts_Puls.Checked = false;
            //    checkBox_Shortcuts_Weather.Checked = false;
            //    checkBox_Shortcuts_Energy.Checked = false;
            //}
            //#endregion

            //#region Animation
            //if (Watch_Face.Unknown11 != null)
            //{
            //    checkBox_Animation.Checked = true;
            //    // покадровая анимация
            //    if (Watch_Face.Unknown11.Unknown11_2 != null && Watch_Face.Unknown11.Unknown11_2.Unknown11d2p1 != null)
            //    {
            //        checkBox_StaticAnimation.Checked = true;
            //        int v = (int)Watch_Face.Unknown11.Unknown11_2.Unknown11d2p2;
            //        if (v < 100) v = 100;
            //        numericUpDown_StaticAnimation_Count.Value = Watch_Face.Unknown11.Unknown11_2.Unknown11d2p1.ImagesCount;
            //        numericUpDown_StaticAnimation_SpeedAnimation.Value = v;
            //        numericUpDown_StaticAnimation_TimeAnimation.Value = Watch_Face.Unknown11.Unknown11_2.Unknown11d2p4;
            //        numericUpDown_StaticAnimation_Pause.Value = Watch_Face.Unknown11.Unknown11_2.Unknown11d2p5;

            //        comboBoxSetText(comboBox_StaticAnimation_Image, Watch_Face.Unknown11.Unknown11_2.Unknown11d2p1.ImageIndex);
            //        numericUpDown_StaticAnimation_X.Value = Watch_Face.Unknown11.Unknown11_2.Unknown11d2p1.X;
            //        numericUpDown_StaticAnimation_Y.Value = Watch_Face.Unknown11.Unknown11_2.Unknown11d2p1.Y;
            //    }
            //    else checkBox_StaticAnimation.Checked = false;

            //    // перемещение между координатами
            //    if (Watch_Face.Unknown11.Unknown11_1 != null)
            //    {
            //        bool motiomAnimation = false;
            //        dataGridView_MotiomAnimation.Rows.Clear();
            //        foreach (MotiomAnimation MotiomAnimation in Watch_Face.Unknown11.Unknown11_1)
            //        {
            //            if (MotiomAnimation.Unknown11d1p2 != null && MotiomAnimation.Unknown11d1p3 != null)
            //            {
            //                motiomAnimation = true;
            //                int Unknown1 = (int)MotiomAnimation.Unknown11d1p1;
            //                int StartCoordinates_X = (int)MotiomAnimation.Unknown11d1p2.X;
            //                int StartCoordinates_Y = (int)MotiomAnimation.Unknown11d1p2.Y;
            //                int EndCoordinates_X = (int)MotiomAnimation.Unknown11d1p3.X;
            //                int EndCoordinates_Y = (int)MotiomAnimation.Unknown11d1p3.Y;
            //                int ImageIndex = (int)MotiomAnimation.ImageIndex;
            //                int SpeedAnimation = (int)MotiomAnimation.Unknown11d1p5;
            //                int TimeAnimation = (int)MotiomAnimation.Unknown11d1p6;
            //                int Unknown5 = (int)MotiomAnimation.Unknown11d1p7;
            //                int Unknown6 = (int)MotiomAnimation.Unknown11d1p8;
            //                int Unknown7 = (int)MotiomAnimation.Unknown11d1p9;
            //                bool Bounce = false;
            //                if (MotiomAnimation.Unknown11d1p10 == 1) Bounce = true;

            //                if (SpeedAnimation < 10) SpeedAnimation = 10;

            //                //var RowNew = new DataGridViewRow();
            //                dataGridView_MotiomAnimation.Rows.Add(Unknown1, StartCoordinates_X, StartCoordinates_Y,
            //                    EndCoordinates_X, EndCoordinates_Y, ImageIndex, SpeedAnimation, TimeAnimation,
            //                    Unknown5, Unknown6, Unknown7, Bounce);
            //            }
            //        }
            //        if (motiomAnimation)
            //        {
            //            checkBox_MotiomAnimation.Checked = true;

            //            MotiomAnimation_Update = true;

            //            int StartCoordinates_X = 0;
            //            int StartCoordinates_Y = 0;
            //            int EndCoordinates_X = 0;
            //            int EndCoordinates_Y = 0;
            //            int ImageIndex = 0;
            //            numericUpDown_MotiomAnimation_StartCoordinates_X.Value = StartCoordinates_X;
            //            numericUpDown_MotiomAnimation_StartCoordinates_Y.Value = StartCoordinates_Y;
            //            numericUpDown_MotiomAnimation_EndCoordinates_X.Value = EndCoordinates_X;
            //            numericUpDown_MotiomAnimation_EndCoordinates_Y.Value = EndCoordinates_Y;
            //            comboBox_MotiomAnimation_Image.Text = "";

            //            int RowIndex = 0;
            //            if (!dataGridView_MotiomAnimation.Rows[RowIndex].IsNewRow)
            //            {
            //                DataGridViewRow row = dataGridView_MotiomAnimation.Rows[RowIndex];
            //                if (row.Cells[1].Value != null) Int32.TryParse(row.Cells[1].Value.ToString(), out StartCoordinates_X);
            //                if (row.Cells[2].Value != null) Int32.TryParse(row.Cells[2].Value.ToString(), out StartCoordinates_Y);
            //                if (row.Cells[3].Value != null) Int32.TryParse(row.Cells[3].Value.ToString(), out EndCoordinates_X);
            //                if (row.Cells[4].Value != null) Int32.TryParse(row.Cells[4].Value.ToString(), out EndCoordinates_Y);

            //                numericUpDown_MotiomAnimation_StartCoordinates_X.Value = StartCoordinates_X;
            //                numericUpDown_MotiomAnimation_StartCoordinates_Y.Value = StartCoordinates_Y;
            //                numericUpDown_MotiomAnimation_EndCoordinates_X.Value = EndCoordinates_X;
            //                numericUpDown_MotiomAnimation_EndCoordinates_Y.Value = EndCoordinates_Y;

            //                if (row.Cells[5].Value != null && Int32.TryParse(row.Cells[5].Value.ToString(), out ImageIndex))
            //                {
            //                    comboBoxSetText(comboBox_MotiomAnimation_Image, ImageIndex);
            //                }
            //                else
            //                {
            //                    comboBox_MotiomAnimation_Image.Text = "";
            //                }
            //            }
            //            MotiomAnimation_Update = false;
            //        }
            //        else checkBox_MotiomAnimation.Checked = false;
            //    }


            //}
            //else
            //{
            //    checkBox_Animation.Checked = false;
            //    checkBox_StaticAnimation.Checked = false;
            //    checkBox_MotiomAnimation.Checked = false;
            //}
            //#endregion ScreenIdleTemp

            if (Watch_Face.Widgets != null) WidgetsTemp = Watch_Face.Widgets;
            if (Watch_Face.ScreenIdle != null) ScreenIdleTemp = Watch_Face.ScreenIdle;
        }

        private bool OneCoordinates(List<Coordinates> coordinates)
        {
            if (coordinates.Count == 1) return true;
            bool result = true;
            long x = coordinates[0].X;
            long y = coordinates[0].Y;
            foreach(Coordinates coordinate in coordinates)
            {
                if (x != coordinate.X || y != coordinate.Y) result = false;
            }
            return result;
        }

        // формируем JSON файл из настроек
        private void JSON_write()
        {
            if (!PreviewView) return;
            Watch_Face = new WATCH_FACE_JSON();

            if (radioButton_GTR2.Checked)
            {
                Watch_Face.Info = new Device_Id();
                Watch_Face.Info.DeviceId = 59;
            }
            //if (radioButton_42.Checked)
            //{
            //    Watch_Face.Info = new Device_Id();
            //    Watch_Face.Info.DeviceId = 42;
            //}
            if (radioButton_GTS2.Checked)
            {
                Watch_Face.Info = new Device_Id();
                Watch_Face.Info.DeviceId = 65;
            }
            //if (radioButton_TRex.Checked)
            //{
            //    Watch_Face.Info = new Device_Id();
            //    Watch_Face.Info.DeviceId = 52;
            //}
            //if (radioButton_Verge.Checked)
            //{
            //    Watch_Face.Info = new Device_Id();
            //    Watch_Face.Info.DeviceId = 32;
            //}
            //if (radioButton_AmazfitX.Checked)
            //{
            //    Watch_Face.Info = new Device_Id();
            //    Watch_Face.Info.DeviceId = 53;
            //}

            if (Watch_Face == null) Watch_Face = new WATCH_FACE_JSON();
            #region Background
            if (radioButton_Background_image.Checked)
            {
                if (comboBox_Background_image.SelectedIndex >= 0)
                {
                    if (Watch_Face.Background == null) Watch_Face.Background = new Background();
                    Watch_Face.Background.BackgroundImageIndex = Int32.Parse(comboBox_Background_image.Text);
                }
            }
            else
            {
                if (Watch_Face.Background == null) Watch_Face.Background = new Background();
                string colorStr = ColorBackgroundWrite(comboBox_Background_color.BackColor);
                Watch_Face.Background.Color = colorStr;
            }

            if (comboBox_Preview_image.SelectedIndex >= 0)
            {
                //MotiomAnimation[] motiomAnimation = new MotiomAnimation[0];
                List<MultilangImage> MultilangImage = new List<MultilangImage>();

                MultilangImage multilangImage = new MultilangImage();
                multilangImage.LangCode = "All";

                if (Watch_Face.Background == null) Watch_Face.Background = new Background();
                multilangImage.ImageSet = new ImageSetGTR2();
                multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_Preview_image.Text);
                multilangImage.ImageSet.ImagesCount = 1;
                MultilangImage.Add(multilangImage);
                Watch_Face.Background.Preview = MultilangImage;
            }
            #endregion

            #region цифровое время
            // часы
            if (checkBox_Hour_Use.Checked && comboBox_Hour_image.SelectedIndex >= 0)
            {
                if (Watch_Face.DialFace == null) Watch_Face.DialFace = new ScreenNormal();
                if (Watch_Face.DialFace.DigitalDialFace == null)
                    Watch_Face.DialFace.DigitalDialFace = new DigitalDialFace();
                if (Watch_Face.DialFace.DigitalDialFace.Digits == null)
                    Watch_Face.DialFace.DigitalDialFace.Digits = new List<DigitalTimeDigit>();

                DigitalTimeDigit digitalTimeDigit = new DigitalTimeDigit();
                digitalTimeDigit.TimeType = "Hour";
                digitalTimeDigit.CombingMode = "Single";
                digitalTimeDigit.Digit = new Text();
                digitalTimeDigit.Digit.Image = new ImageAmazfit();
                digitalTimeDigit.Digit.Image.X = (long)numericUpDown_HourX.Value;
                digitalTimeDigit.Digit.Image.Y = (long)numericUpDown_HourY.Value;
                digitalTimeDigit.Digit.Image.MultilangImage = new List<MultilangImage>();
                    MultilangImage multilangImage = new MultilangImage();
                    multilangImage.LangCode = "All";
                    multilangImage.ImageSet = new ImageSetGTR2();
                    multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_Hour_image.Text);
                    multilangImage.ImageSet.ImagesCount = 10;
                digitalTimeDigit.Digit.Image.MultilangImage.Add(multilangImage);
                if (comboBox_Hour_separator.SelectedIndex >= 0)
                {
                    digitalTimeDigit.Digit.Image.MultilangImageUnit = new List<MultilangImage>();
                    MultilangImage multilangImageUnit = new MultilangImage();
                    multilangImageUnit.LangCode = "All";
                    multilangImageUnit.ImageSet = new ImageSetGTR2();
                    multilangImageUnit.ImageSet.ImageIndex = Int32.Parse(comboBox_Hour_separator.Text);
                    multilangImageUnit.ImageSet.ImagesCount = 1;
                    digitalTimeDigit.Digit.Image.MultilangImageUnit.Add(multilangImageUnit); 
                }
                string Alignment = StringToAlignment2(comboBox_Hour_alignment.SelectedIndex);
                digitalTimeDigit.Digit.Alignment = Alignment;
                digitalTimeDigit.Digit.Spacing = (long)numericUpDown_Hour_spacing.Value;
                //if (checkBox_Hour_add_zero.Checked) digitalTimeDigit.Digit.PaddingZero = 0;
                //digitalTimeDigit.Digit.PaddingZero = checkBox_Hour_add_zero.Checked? 1 : 0;
                digitalTimeDigit.Digit.PaddingZero = checkBox_Hour_add_zero.Checked;


                if (comboBox_Hour_unit.SelectedIndex >= 0)
                {
                    digitalTimeDigit.Separator = new ImageCoord();
                    digitalTimeDigit.Separator.Coordinates = new Coordinates();
                    digitalTimeDigit.Separator.Coordinates.X = (long)numericUpDown_Hour_unitX.Value;
                    digitalTimeDigit.Separator.Coordinates.Y = (long)numericUpDown_Hour_unitY.Value;
                    digitalTimeDigit.Separator.ImageIndex = Int32.Parse(comboBox_Hour_unit.Text);
                }
                Watch_Face.DialFace.DigitalDialFace.Digits.Add(digitalTimeDigit);

            }

            // минуты
            if (checkBox_Minute_Use.Checked && comboBox_Minute_image.SelectedIndex >= 0)
            {
                if (Watch_Face.DialFace == null) Watch_Face.DialFace = new ScreenNormal();
                if (Watch_Face.DialFace.DigitalDialFace == null)
                    Watch_Face.DialFace.DigitalDialFace = new DigitalDialFace();
                if (Watch_Face.DialFace.DigitalDialFace.Digits == null)
                    Watch_Face.DialFace.DigitalDialFace.Digits = new List<DigitalTimeDigit>();

                DigitalTimeDigit digitalTimeDigit = new DigitalTimeDigit();
                digitalTimeDigit.TimeType = "Minute";
                //digitalTimeDigit.CombingMode = "Single";
                digitalTimeDigit.CombingMode = checkBox_Minute_follow.Checked ? "Follow" : "Single";
                digitalTimeDigit.Digit = new Text();
                digitalTimeDigit.Digit.Image = new ImageAmazfit();
                digitalTimeDigit.Digit.Image.X = (long)numericUpDown_MinuteX.Value;
                digitalTimeDigit.Digit.Image.Y = (long)numericUpDown_MinuteY.Value;
                digitalTimeDigit.Digit.Image.MultilangImage = new List<MultilangImage>();
                MultilangImage multilangImage = new MultilangImage();
                    multilangImage.LangCode = "All";
                    multilangImage.ImageSet = new ImageSetGTR2();
                    if (comboBox_Minute_image.SelectedIndex >= 0)
                        multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_Minute_image.Text);
                    multilangImage.ImageSet.ImagesCount = 10;
                    digitalTimeDigit.Digit.Image.MultilangImage.Add(multilangImage);
                if (comboBox_Minute_separator.SelectedIndex >= 0)
                {
                    digitalTimeDigit.Digit.Image.MultilangImageUnit = new List<MultilangImage>();
                    MultilangImage multilangImageUnit = new MultilangImage();
                    multilangImageUnit.LangCode = "All";
                    multilangImageUnit.ImageSet = new ImageSetGTR2();
                    multilangImageUnit.ImageSet.ImageIndex = Int32.Parse(comboBox_Minute_separator.Text);
                    multilangImageUnit.ImageSet.ImagesCount = 1;
                    digitalTimeDigit.Digit.Image.MultilangImageUnit.Add(multilangImageUnit); 
                }
                string Alignment = StringToAlignment2(comboBox_Minute_alignment.SelectedIndex);
                digitalTimeDigit.Digit.Alignment = Alignment;
                digitalTimeDigit.Digit.Spacing = (long)numericUpDown_Minute_spacing.Value;
                //digitalTimeDigit.Digit.PaddingZero = checkBox_Minute_add_zero.Checked ? 1 : 0;
                digitalTimeDigit.Digit.PaddingZero = checkBox_Minute_add_zero.Checked;

                if (comboBox_Minute_unit.SelectedIndex >= 0)
                {
                    digitalTimeDigit.Separator = new ImageCoord();
                    digitalTimeDigit.Separator.Coordinates = new Coordinates();
                    digitalTimeDigit.Separator.Coordinates.X = (long)numericUpDown_Minute_unitX.Value;
                    digitalTimeDigit.Separator.Coordinates.Y = (long)numericUpDown_Minute_unitY.Value;
                    digitalTimeDigit.Separator.ImageIndex = Int32.Parse(comboBox_Minute_unit.Text);
                }
                Watch_Face.DialFace.DigitalDialFace.Digits.Add(digitalTimeDigit);
            }

            // секунды
            if (checkBox_Second_Use.Checked && comboBox_Second_image.SelectedIndex >= 0)
            {
                if (Watch_Face.DialFace == null) Watch_Face.DialFace = new ScreenNormal();
                if (Watch_Face.DialFace.DigitalDialFace == null)
                    Watch_Face.DialFace.DigitalDialFace = new DigitalDialFace();
                if (Watch_Face.DialFace.DigitalDialFace.Digits == null)
                    Watch_Face.DialFace.DigitalDialFace.Digits = new List<DigitalTimeDigit>();

                DigitalTimeDigit digitalTimeDigit = new DigitalTimeDigit();
                digitalTimeDigit.TimeType = "Second";
                //digitalTimeDigit.CombingMode = "Single";
                digitalTimeDigit.CombingMode = checkBox_Second_follow.Checked ? "Follow" : "Single";
                digitalTimeDigit.Digit = new Text();
                digitalTimeDigit.Digit.Image = new ImageAmazfit();
                digitalTimeDigit.Digit.Image.X = (long)numericUpDown_SecondX.Value;
                digitalTimeDigit.Digit.Image.Y = (long)numericUpDown_SecondY.Value;
                digitalTimeDigit.Digit.Image.MultilangImage = new List<MultilangImage>();
                MultilangImage multilangImage = new MultilangImage();
                    multilangImage.LangCode = "All";
                    multilangImage.ImageSet = new ImageSetGTR2();
                    multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_Second_image.Text);
                    multilangImage.ImageSet.ImagesCount = 10;
                digitalTimeDigit.Digit.Image.MultilangImage.Add(multilangImage);
                if (comboBox_Second_separator.SelectedIndex >= 0)
                {
                    digitalTimeDigit.Digit.Image.MultilangImageUnit = new List<MultilangImage>();
                    MultilangImage multilangImageUnit = new MultilangImage();
                    multilangImageUnit.LangCode = "All";
                    multilangImageUnit.ImageSet = new ImageSetGTR2();
                    multilangImageUnit.ImageSet.ImageIndex = Int32.Parse(comboBox_Second_separator.Text);
                    multilangImageUnit.ImageSet.ImagesCount = 1;
                    digitalTimeDigit.Digit.Image.MultilangImageUnit.Add(multilangImageUnit); 
                }
                string Alignment = StringToAlignment2(comboBox_Second_alignment.SelectedIndex);
                digitalTimeDigit.Digit.Alignment = Alignment;
                digitalTimeDigit.Digit.Spacing = (long)numericUpDown_Second_spacing.Value;
                //digitalTimeDigit.Digit.PaddingZero = checkBox_Second_add_zero.Checked ? 1 : 0;
                digitalTimeDigit.Digit.PaddingZero = checkBox_Second_add_zero.Checked;

                if (comboBox_Second_unit.SelectedIndex >= 0)
                {
                    digitalTimeDigit.Separator = new ImageCoord();
                    digitalTimeDigit.Separator.Coordinates = new Coordinates();
                    digitalTimeDigit.Separator.Coordinates.X = (long)numericUpDown_Second_unitX.Value;
                    digitalTimeDigit.Separator.Coordinates.Y = (long)numericUpDown_Second_unitY.Value;
                    digitalTimeDigit.Separator.ImageIndex = Int32.Parse(comboBox_Second_unit.Text);
                }
                Watch_Face.DialFace.DigitalDialFace.Digits.Add(digitalTimeDigit);
            }

            // AM/PM
            if (checkBox_12h_Use.Checked &&
                comboBox_AM_image.SelectedIndex >= 0 && comboBox_PM_image.SelectedIndex >= 0)
            {
                if (Watch_Face.DialFace == null) Watch_Face.DialFace = new ScreenNormal();
                if (Watch_Face.DialFace.DigitalDialFace == null)
                    Watch_Face.DialFace.DigitalDialFace = new DigitalDialFace();

                Watch_Face.DialFace.DigitalDialFace.AM = new MultilangImageCoord();
                Watch_Face.DialFace.DigitalDialFace.AM.Coordinates = new Coordinates();
                Watch_Face.DialFace.DigitalDialFace.AM.Coordinates.X = (long)numericUpDown_AM_X.Value;
                Watch_Face.DialFace.DigitalDialFace.AM.Coordinates.Y = (long)numericUpDown_AM_Y.Value;
                Watch_Face.DialFace.DigitalDialFace.AM.ImageSet = new List<MultilangImage>();
                MultilangImage multilangImage = new MultilangImage();
                    multilangImage.LangCode = "All";
                    multilangImage.ImageSet = new ImageSetGTR2();
                    multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_AM_image.Text);
                    multilangImage.ImageSet.ImagesCount = 1;
                Watch_Face.DialFace.DigitalDialFace.AM.ImageSet.Add(multilangImage);

                Watch_Face.DialFace.DigitalDialFace.PM = new MultilangImageCoord();
                Watch_Face.DialFace.DigitalDialFace.PM.Coordinates = new Coordinates();
                Watch_Face.DialFace.DigitalDialFace.PM.Coordinates.X = (long)numericUpDown_PM_X.Value;
                Watch_Face.DialFace.DigitalDialFace.PM.Coordinates.Y = (long)numericUpDown_PM_Y.Value;
                Watch_Face.DialFace.DigitalDialFace.PM.ImageSet = new List<MultilangImage>();
                multilangImage = new MultilangImage();
                    multilangImage.LangCode = "All";
                    multilangImage.ImageSet = new ImageSetGTR2();
                    multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_PM_image.Text);
                    multilangImage.ImageSet.ImagesCount = 1;
                Watch_Face.DialFace.DigitalDialFace.PM.ImageSet.Add(multilangImage);
            }
            #endregion

            #region аналоговое  время
            // часы
            if (checkBox_Hour_hand_Use.Checked && comboBox_Hour_hand_image.SelectedIndex >= 0)
            {
                if (Watch_Face.DialFace == null) Watch_Face.DialFace = new ScreenNormal();
                if (Watch_Face.DialFace.AnalogDialFace == null) 
                    Watch_Face.DialFace.AnalogDialFace = new AnalogDialFace();
                Watch_Face.DialFace.AnalogDialFace.Hours = new ClockHand();
                Watch_Face.DialFace.AnalogDialFace.Hours.X = (long)numericUpDown_Hour_handX.Value;
                Watch_Face.DialFace.AnalogDialFace.Hours.Y = (long)numericUpDown_Hour_handY.Value;
                Watch_Face.DialFace.AnalogDialFace.Hours.StartAngle = 0;
                Watch_Face.DialFace.AnalogDialFace.Hours.EndAngle = 360;
                Watch_Face.DialFace.AnalogDialFace.Hours.Pointer = new ImageCoord();
                Watch_Face.DialFace.AnalogDialFace.Hours.Pointer.Coordinates = new Coordinates();
                Watch_Face.DialFace.AnalogDialFace.Hours.Pointer.Coordinates.X = (long)numericUpDown_Hour_handX_offset.Value;
                Watch_Face.DialFace.AnalogDialFace.Hours.Pointer.Coordinates.Y = (long)numericUpDown_Hour_handY_offset.Value;
                Watch_Face.DialFace.AnalogDialFace.Hours.Pointer.ImageIndex = Int32.Parse(comboBox_Hour_hand_image.Text);
                if (comboBox_Hour_hand_imageCentr.SelectedIndex >= 0)
                {
                    Watch_Face.DialFace.AnalogDialFace.Hours.Cover = new ImageCoord();
                    Watch_Face.DialFace.AnalogDialFace.Hours.Cover.Coordinates = new Coordinates();
                    Watch_Face.DialFace.AnalogDialFace.Hours.Cover.Coordinates.X = (long)numericUpDown_Hour_handX_centr.Value;
                    Watch_Face.DialFace.AnalogDialFace.Hours.Cover.Coordinates.Y = (long)numericUpDown_Hour_handY_centr.Value;
                    Watch_Face.DialFace.AnalogDialFace.Hours.Cover.ImageIndex = Int32.Parse(comboBox_Hour_hand_imageCentr.Text);
                }
            }

            // минуты
            if (checkBox_Minute_hand_Use.Checked && comboBox_Minute_hand_image.SelectedIndex >= 0)
            {
                if (Watch_Face.DialFace == null) Watch_Face.DialFace = new ScreenNormal();
                if (Watch_Face.DialFace.AnalogDialFace == null)
                    Watch_Face.DialFace.AnalogDialFace = new AnalogDialFace();
                Watch_Face.DialFace.AnalogDialFace.Minutes = new ClockHand();
                Watch_Face.DialFace.AnalogDialFace.Minutes.X = (long)numericUpDown_Minute_handX.Value;
                Watch_Face.DialFace.AnalogDialFace.Minutes.Y = (long)numericUpDown_Minute_handY.Value;
                Watch_Face.DialFace.AnalogDialFace.Minutes.StartAngle = 0;
                Watch_Face.DialFace.AnalogDialFace.Minutes.EndAngle = 360;
                Watch_Face.DialFace.AnalogDialFace.Minutes.Pointer = new ImageCoord();
                Watch_Face.DialFace.AnalogDialFace.Minutes.Pointer.Coordinates = new Coordinates();
                Watch_Face.DialFace.AnalogDialFace.Minutes.Pointer.Coordinates.X = (long)numericUpDown_Minute_handX_offset.Value;
                Watch_Face.DialFace.AnalogDialFace.Minutes.Pointer.Coordinates.Y = (long)numericUpDown_Minute_handY_offset.Value;
                Watch_Face.DialFace.AnalogDialFace.Minutes.Pointer.ImageIndex = Int32.Parse(comboBox_Minute_hand_image.Text);
                if (comboBox_Minute_hand_imageCentr.SelectedIndex >= 0)
                {
                    Watch_Face.DialFace.AnalogDialFace.Minutes.Cover = new ImageCoord();
                    Watch_Face.DialFace.AnalogDialFace.Minutes.Cover.Coordinates = new Coordinates();
                    Watch_Face.DialFace.AnalogDialFace.Minutes.Cover.Coordinates.X = (long)numericUpDown_Minute_handX_centr.Value;
                    Watch_Face.DialFace.AnalogDialFace.Minutes.Cover.Coordinates.Y = (long)numericUpDown_Minute_handY_centr.Value;
                    Watch_Face.DialFace.AnalogDialFace.Minutes.Cover.ImageIndex = Int32.Parse(comboBox_Minute_hand_imageCentr.Text);
                }
            }

            // секунды
            if (checkBox_Second_hand_Use.Checked && comboBox_Second_hand_image.SelectedIndex >= 0)
            {
                if (Watch_Face.DialFace == null) Watch_Face.DialFace = new ScreenNormal();
                if (Watch_Face.DialFace.AnalogDialFace == null)
                    Watch_Face.DialFace.AnalogDialFace = new AnalogDialFace();
                Watch_Face.DialFace.AnalogDialFace.Seconds = new ClockHand();
                Watch_Face.DialFace.AnalogDialFace.Seconds.X = (long)numericUpDown_Second_handX.Value;
                Watch_Face.DialFace.AnalogDialFace.Seconds.Y = (long)numericUpDown_Second_handY.Value;
                Watch_Face.DialFace.AnalogDialFace.Seconds.StartAngle = 0;
                Watch_Face.DialFace.AnalogDialFace.Seconds.EndAngle = 360;
                Watch_Face.DialFace.AnalogDialFace.Seconds.Pointer = new ImageCoord();
                Watch_Face.DialFace.AnalogDialFace.Seconds.Pointer.Coordinates = new Coordinates();
                Watch_Face.DialFace.AnalogDialFace.Seconds.Pointer.Coordinates.X = (long)numericUpDown_Second_handX_offset.Value;
                Watch_Face.DialFace.AnalogDialFace.Seconds.Pointer.Coordinates.Y = (long)numericUpDown_Second_handY_offset.Value;
                Watch_Face.DialFace.AnalogDialFace.Seconds.Pointer.ImageIndex = Int32.Parse(comboBox_Second_hand_image.Text);
                if (comboBox_Second_hand_imageCentr.SelectedIndex >= 0)
                {
                    Watch_Face.DialFace.AnalogDialFace.Seconds.Cover = new ImageCoord();
                    Watch_Face.DialFace.AnalogDialFace.Seconds.Cover.Coordinates = new Coordinates();
                    Watch_Face.DialFace.AnalogDialFace.Seconds.Cover.Coordinates.X = (long)numericUpDown_Second_handX_centr.Value;
                    Watch_Face.DialFace.AnalogDialFace.Seconds.Cover.Coordinates.Y = (long)numericUpDown_Second_handY_centr.Value;
                    Watch_Face.DialFace.AnalogDialFace.Seconds.Cover.ImageIndex = Int32.Parse(comboBox_Second_hand_imageCentr.Text);
                }
            }
            #endregion

            #region дата

            // день недели картинкой
            if (checkBox_DOW_pictures_Use.Checked && comboBox_DOW_pictures_image.SelectedIndex >= 0)
            {
                if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                if (Watch_Face.System.Date == null) Watch_Face.System.Date = new Date();
                if (Watch_Face.System.Date.WeeksDigits == null)
                    Watch_Face.System.Date.WeeksDigits = new DigitalCommonDigit();

                Watch_Face.System.Date.WeeksDigits.CombingMode = "Single";
                //digitalDateDigit.CombingMode = checkBox_DOW_follow.Checked ? "Follow" : "Single";
                Watch_Face.System.Date.WeeksDigits.Digit = new Text();
                Watch_Face.System.Date.WeeksDigits.Digit.DisplayFormAnalog = true;
                Watch_Face.System.Date.WeeksDigits.Digit.Image = new ImageAmazfit();
                Watch_Face.System.Date.WeeksDigits.Digit.Image.X = (long)numericUpDown_DOW_picturesX.Value;
                Watch_Face.System.Date.WeeksDigits.Digit.Image.Y = (long)numericUpDown_DOW_picturesY.Value;
                Watch_Face.System.Date.WeeksDigits.Digit.Image.MultilangImage = new List<MultilangImage>();
                MultilangImage multilangImage = new MultilangImage();
                multilangImage.LangCode = "All";
                multilangImage.ImageSet = new ImageSetGTR2();
                if (comboBox_DOW_pictures_image.SelectedIndex >= 0)
                    multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_DOW_pictures_image.Text);
                multilangImage.ImageSet.ImagesCount = 7;
                Watch_Face.System.Date.WeeksDigits.Digit.Image.MultilangImage.Add(multilangImage);
            }

            // день недели стрелкой
            if (checkBox_DOW_hand_Use.Checked && comboBox_DOW_hand_image.SelectedIndex >= 0)
            {
                if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                if (Watch_Face.System.Date == null)
                    Watch_Face.System.Date = new Date();
                if (Watch_Face.System.Date.DateClockHand == null)
                    Watch_Face.System.Date.DateClockHand = new DateClockHand();
                Watch_Face.System.Date.DateClockHand.WeekDayClockHand = new ClockHand();
                Watch_Face.System.Date.DateClockHand.WeekDayClockHand.X = (long)numericUpDown_DOW_handX.Value;
                Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Y = (long)numericUpDown_DOW_handY.Value;
                Watch_Face.System.Date.DateClockHand.WeekDayClockHand.StartAngle = (float)numericUpDown_DOW_hand_startAngle.Value;
                Watch_Face.System.Date.DateClockHand.WeekDayClockHand.EndAngle = (float)numericUpDown_DOW_hand_endAngle.Value;
                Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Pointer = new ImageCoord();
                Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Pointer.Coordinates = new Coordinates();
                Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Pointer.Coordinates.X = (long)numericUpDown_DOW_handX_offset.Value;
                Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Pointer.Coordinates.Y = (long)numericUpDown_DOW_handY_offset.Value;
                Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Pointer.ImageIndex = Int32.Parse(comboBox_DOW_hand_image.Text);
                if (comboBox_DOW_hand_imageCentr.SelectedIndex >= 0)
                {
                    Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Cover = new ImageCoord();
                    Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Cover.Coordinates = new Coordinates();
                    Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Cover.Coordinates.X = (long)numericUpDown_DOW_handX_centr.Value;
                    Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Cover.Coordinates.Y = (long)numericUpDown_DOW_handY_centr.Value;
                    Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Cover.ImageIndex = Int32.Parse(comboBox_DOW_hand_imageCentr.Text);
                }

                if (comboBox_DOW_hand_imageBackground.SelectedIndex >= 0)
                {
                    Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Scale = new MultilangImageCoord();
                    Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Scale.Coordinates = new Coordinates();
                    Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Scale.Coordinates.X = (long)numericUpDown_DOW_handX_background.Value;
                    Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Scale.Coordinates.Y = (long)numericUpDown_DOW_handY_background.Value;
                    Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Scale.ImageSet = new List<MultilangImage>();
                    MultilangImage multilangImage = new MultilangImage();
                    multilangImage.LangCode = "All";
                    multilangImage.ImageSet = new ImageSetGTR2();
                    multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_DOW_hand_imageBackground.Text);
                    multilangImage.ImageSet.ImagesCount = 1;
                    Watch_Face.System.Date.DateClockHand.WeekDayClockHand.Scale.ImageSet.Add(multilangImage);
                }
            }

            // год
            if (checkBox__Year_text_Use.Checked && comboBox_Year_image.SelectedIndex >= 0)
            {
                if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                if (Watch_Face.System.Date == null) Watch_Face.System.Date = new Date();
                if (Watch_Face.System.Date.DateDigits == null)
                    Watch_Face.System.Date.DateDigits = new List<DigitalDateDigit>();

                DigitalDateDigit digitalDateDigit = new DigitalDateDigit();
                digitalDateDigit.DateType = "Year";
                digitalDateDigit.CombingMode = "Single";
                //digitalDateDigit.CombingMode = checkBox_Year_follow.Checked ? "Follow" : "Single";
                digitalDateDigit.Digit = new Text();
                digitalDateDigit.Digit.Image = new ImageAmazfit();
                digitalDateDigit.Digit.Image.X = (long)numericUpDown_YearX.Value;
                digitalDateDigit.Digit.Image.Y = (long)numericUpDown_YearY.Value;
                digitalDateDigit.Digit.Image.MultilangImage = new List<MultilangImage>();
                MultilangImage multilangImage = new MultilangImage();
                multilangImage.LangCode = "All";
                multilangImage.ImageSet = new ImageSetGTR2();
                if (comboBox_Year_image.SelectedIndex >= 0)
                    multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_Year_image.Text);
                multilangImage.ImageSet.ImagesCount = 10;
                digitalDateDigit.Digit.Image.MultilangImage.Add(multilangImage);
                if (comboBox_Year_separator.SelectedIndex >= 0)
                {
                    digitalDateDigit.Digit.Image.MultilangImageUnit = new List<MultilangImage>();
                    MultilangImage multilangImageUnit = new MultilangImage();
                    multilangImageUnit.LangCode = "All";
                    multilangImageUnit.ImageSet = new ImageSetGTR2();
                    multilangImageUnit.ImageSet.ImageIndex = Int32.Parse(comboBox_Year_separator.Text);
                    multilangImageUnit.ImageSet.ImagesCount = 1;
                    digitalDateDigit.Digit.Image.MultilangImageUnit.Add(multilangImageUnit);
                }
                string Alignment = StringToAlignment2(comboBox_Year_alignment.SelectedIndex);
                digitalDateDigit.Digit.Alignment = Alignment;
                digitalDateDigit.Digit.Spacing = (long)numericUpDown_Year_spacing.Value;
                //digitalTimeDigit.Digit.PaddingZero = checkBox_Year_add_zero.Checked ? 1 : 0;
                digitalDateDigit.Digit.PaddingZero = checkBox_Year_add_zero.Checked;

                if (comboBox_Year_unit.SelectedIndex >= 0)
                {
                    digitalDateDigit.Separator = new ImageCoord();
                    digitalDateDigit.Separator.Coordinates = new Coordinates();
                    digitalDateDigit.Separator.Coordinates.X = (long)numericUpDown_Year_unitX.Value;
                    digitalDateDigit.Separator.Coordinates.Y = (long)numericUpDown_Year_unitY.Value;
                    digitalDateDigit.Separator.ImageIndex = Int32.Parse(comboBox_Year_unit.Text);
                }
                Watch_Face.System.Date.DateDigits.Add(digitalDateDigit);
            }

            // месяц
            if (checkBox_Month_Use.Checked && comboBox_Month_image.SelectedIndex >= 0)
            {
                if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                if (Watch_Face.System.Date == null) Watch_Face.System.Date = new Date();
                if (Watch_Face.System.Date.DateDigits == null)
                    Watch_Face.System.Date.DateDigits = new List<DigitalDateDigit>();

                DigitalDateDigit digitalDateDigit = new DigitalDateDigit();
                digitalDateDigit.DateType = "Month";
                //digitalTimeDigit.CombingMode = "Single";
                digitalDateDigit.CombingMode = checkBox_Month_follow.Checked ? "Follow" : "Single";
                digitalDateDigit.Digit = new Text();
                digitalDateDigit.Digit.Image = new ImageAmazfit();
                digitalDateDigit.Digit.Image.X = (long)numericUpDown_MonthX.Value;
                digitalDateDigit.Digit.Image.Y = (long)numericUpDown_MonthY.Value;
                digitalDateDigit.Digit.Image.MultilangImage = new List<MultilangImage>();
                MultilangImage multilangImage = new MultilangImage();
                multilangImage.LangCode = "All";
                multilangImage.ImageSet = new ImageSetGTR2();
                if (comboBox_Month_image.SelectedIndex >= 0)
                    multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_Month_image.Text);
                multilangImage.ImageSet.ImagesCount = 10;
                digitalDateDigit.Digit.Image.MultilangImage.Add(multilangImage);
                if (comboBox_Month_separator.SelectedIndex >= 0)
                {
                    digitalDateDigit.Digit.Image.MultilangImageUnit = new List<MultilangImage>();
                    MultilangImage multilangImageUnit = new MultilangImage();
                    multilangImageUnit.LangCode = "All";
                    multilangImageUnit.ImageSet = new ImageSetGTR2();
                    multilangImageUnit.ImageSet.ImageIndex = Int32.Parse(comboBox_Month_separator.Text);
                    multilangImageUnit.ImageSet.ImagesCount = 1;
                    digitalDateDigit.Digit.Image.MultilangImageUnit.Add(multilangImageUnit);
                }
                string Alignment = StringToAlignment2(comboBox_Month_alignment.SelectedIndex);
                digitalDateDigit.Digit.Alignment = Alignment;
                digitalDateDigit.Digit.Spacing = (long)numericUpDown_Month_spacing.Value;
                //digitalTimeDigit.Digit.PaddingZero = checkBox_Month_add_zero.Checked ? 1 : 0;
                digitalDateDigit.Digit.PaddingZero = checkBox_Month_add_zero.Checked;

                if (comboBox_Month_unit.SelectedIndex >= 0)
                {
                    digitalDateDigit.Separator = new ImageCoord();
                    digitalDateDigit.Separator.Coordinates = new Coordinates();
                    digitalDateDigit.Separator.Coordinates.X = (long)numericUpDown_Month_unitX.Value;
                    digitalDateDigit.Separator.Coordinates.Y = (long)numericUpDown_Month_unitY.Value;
                    digitalDateDigit.Separator.ImageIndex = Int32.Parse(comboBox_Month_unit.Text);
                }
                Watch_Face.System.Date.DateDigits.Add(digitalDateDigit);
            }

            // месяц картинкой
            if (checkBox_Month_pictures_Use.Checked && comboBox_Month_pictures_image.SelectedIndex >= 0)
            {
                if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                if (Watch_Face.System.Date == null) Watch_Face.System.Date = new Date();
                if (Watch_Face.System.Date.DateDigits == null)
                    Watch_Face.System.Date.DateDigits = new List<DigitalDateDigit>();

                DigitalDateDigit digitalDateDigit = new DigitalDateDigit();
                digitalDateDigit.DateType = "Month";
                digitalDateDigit.CombingMode = "Single";
                //digitalDateDigit.CombingMode = checkBox_Month_follow.Checked ? "Follow" : "Single";
                digitalDateDigit.Digit = new Text();
                digitalDateDigit.Digit.DisplayFormAnalog = true;
                digitalDateDigit.Digit.Image = new ImageAmazfit();
                digitalDateDigit.Digit.Image.X = (long)numericUpDown_Month_picturesX.Value;
                digitalDateDigit.Digit.Image.Y = (long)numericUpDown_Month_picturesY.Value;
                digitalDateDigit.Digit.Image.MultilangImage = new List<MultilangImage>();
                MultilangImage multilangImage = new MultilangImage();
                multilangImage.LangCode = "All";
                multilangImage.ImageSet = new ImageSetGTR2();
                if (comboBox_Month_pictures_image.SelectedIndex >= 0)
                    multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_Month_pictures_image.Text);
                multilangImage.ImageSet.ImagesCount = 12;
                digitalDateDigit.Digit.Image.MultilangImage.Add(multilangImage);
                
                Watch_Face.System.Date.DateDigits.Add(digitalDateDigit);
            }

            // месяц стрелкой
            if (checkBox_Month_hand_Use.Checked && comboBox_Month_hand_image.SelectedIndex >= 0)
            {
                if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                if (Watch_Face.System.Date == null)
                    Watch_Face.System.Date = new Date();
                if (Watch_Face.System.Date.DateClockHand == null)
                    Watch_Face.System.Date.DateClockHand = new DateClockHand();
                Watch_Face.System.Date.DateClockHand.MonthClockHand = new ClockHand();
                Watch_Face.System.Date.DateClockHand.MonthClockHand.X = (long)numericUpDown_Month_handX.Value;
                Watch_Face.System.Date.DateClockHand.MonthClockHand.Y = (long)numericUpDown_Month_handY.Value;
                Watch_Face.System.Date.DateClockHand.MonthClockHand.StartAngle = (float)numericUpDown_Month_hand_startAngle.Value;
                Watch_Face.System.Date.DateClockHand.MonthClockHand.EndAngle = (float)numericUpDown_Month_hand_endAngle.Value;
                Watch_Face.System.Date.DateClockHand.MonthClockHand.Pointer = new ImageCoord();
                Watch_Face.System.Date.DateClockHand.MonthClockHand.Pointer.Coordinates = new Coordinates();
                Watch_Face.System.Date.DateClockHand.MonthClockHand.Pointer.Coordinates.X = (long)numericUpDown_Month_handX_offset.Value;
                Watch_Face.System.Date.DateClockHand.MonthClockHand.Pointer.Coordinates.Y = (long)numericUpDown_Month_handY_offset.Value;
                Watch_Face.System.Date.DateClockHand.MonthClockHand.Pointer.ImageIndex = Int32.Parse(comboBox_Month_hand_image.Text);
                
                if (comboBox_Month_hand_imageCentr.SelectedIndex >= 0)
                {
                    Watch_Face.System.Date.DateClockHand.MonthClockHand.Cover = new ImageCoord();
                    Watch_Face.System.Date.DateClockHand.MonthClockHand.Cover.Coordinates = new Coordinates();
                    Watch_Face.System.Date.DateClockHand.MonthClockHand.Cover.Coordinates.X = (long)numericUpDown_Month_handX_centr.Value;
                    Watch_Face.System.Date.DateClockHand.MonthClockHand.Cover.Coordinates.Y = (long)numericUpDown_Month_handY_centr.Value;
                    Watch_Face.System.Date.DateClockHand.MonthClockHand.Cover.ImageIndex = Int32.Parse(comboBox_Month_hand_imageCentr.Text);
                }

                if (comboBox_Month_hand_imageBackground.SelectedIndex >= 0)
                {
                    Watch_Face.System.Date.DateClockHand.MonthClockHand.Scale = new MultilangImageCoord();
                    Watch_Face.System.Date.DateClockHand.MonthClockHand.Scale.Coordinates = new Coordinates();
                    Watch_Face.System.Date.DateClockHand.MonthClockHand.Scale.Coordinates.X = (long)numericUpDown_Month_handX_background.Value;
                    Watch_Face.System.Date.DateClockHand.MonthClockHand.Scale.Coordinates.Y = (long)numericUpDown_Month_handY_background.Value;
                    Watch_Face.System.Date.DateClockHand.MonthClockHand.Scale.ImageSet = new List<MultilangImage>();
                    MultilangImage multilangImage = new MultilangImage();
                    multilangImage.LangCode = "All";
                    multilangImage.ImageSet = new ImageSetGTR2();
                    multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_Month_hand_imageBackground.Text);
                    multilangImage.ImageSet.ImagesCount = 1;
                    Watch_Face.System.Date.DateClockHand.MonthClockHand.Scale.ImageSet.Add(multilangImage);
                }
            }

            // число
            if (checkBox_Day_Use.Checked && comboBox_Day_image.SelectedIndex >= 0)
            {
                if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                if (Watch_Face.System.Date == null) Watch_Face.System.Date = new Date();
                if (Watch_Face.System.Date.DateDigits == null)
                    Watch_Face.System.Date.DateDigits = new List<DigitalDateDigit>();

                DigitalDateDigit digitalDateDigit = new DigitalDateDigit();
                digitalDateDigit.DateType = "Day";
                //digitalTimeDigit.CombingMode = "Single";
                digitalDateDigit.CombingMode = checkBox_Day_follow.Checked ? "Follow" : "Single";
                digitalDateDigit.Digit = new Text();
                digitalDateDigit.Digit.Image = new ImageAmazfit();
                digitalDateDigit.Digit.Image.X = (long)numericUpDown_DayX.Value;
                digitalDateDigit.Digit.Image.Y = (long)numericUpDown_DayY.Value;
                digitalDateDigit.Digit.Image.MultilangImage = new List<MultilangImage>();
                MultilangImage multilangImage = new MultilangImage();
                multilangImage.LangCode = "All";
                multilangImage.ImageSet = new ImageSetGTR2();
                if (comboBox_Day_image.SelectedIndex >= 0)
                    multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_Day_image.Text);
                multilangImage.ImageSet.ImagesCount = 10;
                digitalDateDigit.Digit.Image.MultilangImage.Add(multilangImage);
                if (comboBox_Day_separator.SelectedIndex >= 0)
                {
                    digitalDateDigit.Digit.Image.MultilangImageUnit = new List<MultilangImage>();
                    MultilangImage multilangImageUnit = new MultilangImage();
                    multilangImageUnit.LangCode = "All";
                    multilangImageUnit.ImageSet = new ImageSetGTR2();
                    multilangImageUnit.ImageSet.ImageIndex = Int32.Parse(comboBox_Day_separator.Text);
                    multilangImageUnit.ImageSet.ImagesCount = 1;
                    digitalDateDigit.Digit.Image.MultilangImageUnit.Add(multilangImageUnit);
                }
                string Alignment = StringToAlignment2(comboBox_Day_alignment.SelectedIndex);
                digitalDateDigit.Digit.Alignment = Alignment;
                digitalDateDigit.Digit.Spacing = (long)numericUpDown_Day_spacing.Value;
                //digitalTimeDigit.Digit.PaddingZero = checkBox_Day_add_zero.Checked ? 1 : 0;
                digitalDateDigit.Digit.PaddingZero = checkBox_Day_add_zero.Checked;

                if (comboBox_Day_unit.SelectedIndex >= 0)
                {
                    digitalDateDigit.Separator = new ImageCoord();
                    digitalDateDigit.Separator.Coordinates = new Coordinates();
                    digitalDateDigit.Separator.Coordinates.X = (long)numericUpDown_Day_unitX.Value;
                    digitalDateDigit.Separator.Coordinates.Y = (long)numericUpDown_Day_unitY.Value;
                    digitalDateDigit.Separator.ImageIndex = Int32.Parse(comboBox_Day_unit.Text);
                }
                Watch_Face.System.Date.DateDigits.Add(digitalDateDigit);
            }

            // число стрелкой
            if (checkBox_Day_hand_Use.Checked && comboBox_Day_hand_image.SelectedIndex >= 0)
            {
                if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                if (Watch_Face.System.Date == null)
                    Watch_Face.System.Date = new Date();
                if (Watch_Face.System.Date.DateClockHand == null)
                    Watch_Face.System.Date.DateClockHand = new DateClockHand();
                Watch_Face.System.Date.DateClockHand.DayClockHand = new ClockHand();
                Watch_Face.System.Date.DateClockHand.DayClockHand.X = (long)numericUpDown_Day_handX.Value;
                Watch_Face.System.Date.DateClockHand.DayClockHand.Y = (long)numericUpDown_Day_handY.Value;
                Watch_Face.System.Date.DateClockHand.DayClockHand.StartAngle = (float)numericUpDown_Day_hand_startAngle.Value;
                Watch_Face.System.Date.DateClockHand.DayClockHand.EndAngle = (float)numericUpDown_Day_hand_endAngle.Value;
                Watch_Face.System.Date.DateClockHand.DayClockHand.Pointer = new ImageCoord();
                Watch_Face.System.Date.DateClockHand.DayClockHand.Pointer.Coordinates = new Coordinates();
                Watch_Face.System.Date.DateClockHand.DayClockHand.Pointer.Coordinates.X = (long)numericUpDown_Day_handX_offset.Value;
                Watch_Face.System.Date.DateClockHand.DayClockHand.Pointer.Coordinates.Y = (long)numericUpDown_Day_handY_offset.Value;
                Watch_Face.System.Date.DateClockHand.DayClockHand.Pointer.ImageIndex = Int32.Parse(comboBox_Day_hand_image.Text);

                if (comboBox_Day_hand_imageCentr.SelectedIndex >= 0)
                {
                    Watch_Face.System.Date.DateClockHand.DayClockHand.Cover = new ImageCoord();
                    Watch_Face.System.Date.DateClockHand.DayClockHand.Cover.Coordinates = new Coordinates();
                    Watch_Face.System.Date.DateClockHand.DayClockHand.Cover.Coordinates.X = (long)numericUpDown_Day_handX_centr.Value;
                    Watch_Face.System.Date.DateClockHand.DayClockHand.Cover.Coordinates.Y = (long)numericUpDown_Day_handY_centr.Value;
                    Watch_Face.System.Date.DateClockHand.DayClockHand.Cover.ImageIndex = Int32.Parse(comboBox_Day_hand_imageCentr.Text);
                }

                if (comboBox_Day_hand_imageBackground.SelectedIndex >= 0)
                {
                    Watch_Face.System.Date.DateClockHand.DayClockHand.Scale = new MultilangImageCoord();
                    Watch_Face.System.Date.DateClockHand.DayClockHand.Scale.Coordinates = new Coordinates();
                    Watch_Face.System.Date.DateClockHand.DayClockHand.Scale.Coordinates.X = (long)numericUpDown_Day_handX_background.Value;
                    Watch_Face.System.Date.DateClockHand.DayClockHand.Scale.Coordinates.Y = (long)numericUpDown_Day_handY_background.Value;
                    Watch_Face.System.Date.DateClockHand.DayClockHand.Scale.ImageSet = new List<MultilangImage>();
                    MultilangImage multilangImage = new MultilangImage();
                    multilangImage.LangCode = "All";
                    multilangImage.ImageSet = new ImageSetGTR2();
                    multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_Day_hand_imageBackground.Text);
                    multilangImage.ImageSet.ImagesCount = 1;
                    Watch_Face.System.Date.DateClockHand.DayClockHand.Scale.ImageSet.Add(multilangImage);
                }
            }
            #endregion

            #region статусы
            if (checkBox_Bluetooth_Use.Checked && comboBox_Bluetooth_image.SelectedIndex >= 0)
            {
                if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                if (Watch_Face.System.Status == null) Watch_Face.System.Status = new Status();
                Watch_Face.System.Status.Bluetooth = new ImageCoord();
                Watch_Face.System.Status.Bluetooth.ImageIndex = Int32.Parse(comboBox_Bluetooth_image.Text);
                Watch_Face.System.Status.Bluetooth.Coordinates = new Coordinates();
                Watch_Face.System.Status.Bluetooth.Coordinates.X = (long)numericUpDown_BluetoothX.Value;
                Watch_Face.System.Status.Bluetooth.Coordinates.Y = (long)numericUpDown_BluetoothY.Value;
            }

            if (checkBox_Alarm_Use.Checked && comboBox_Alarm_image.SelectedIndex >= 0)
            {
                if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                if (Watch_Face.System.Status == null) Watch_Face.System.Status = new Status();
                Watch_Face.System.Status.Alarm = new ImageCoord();
                Watch_Face.System.Status.Alarm.ImageIndex = Int32.Parse(comboBox_Alarm_image.Text);
                Watch_Face.System.Status.Alarm.Coordinates = new Coordinates();
                Watch_Face.System.Status.Alarm.Coordinates.X = (long)numericUpDown_AlarmX.Value;
                Watch_Face.System.Status.Alarm.Coordinates.Y = (long)numericUpDown_AlarmY.Value;
            }

            if (checkBox_DND_Use.Checked && comboBox_DND_image.SelectedIndex >= 0)
            {
                if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                if (Watch_Face.System.Status == null) Watch_Face.System.Status = new Status();
                Watch_Face.System.Status.DoNotDisturb = new ImageCoord();
                Watch_Face.System.Status.DoNotDisturb.ImageIndex = Int32.Parse(comboBox_DND_image.Text);
                Watch_Face.System.Status.DoNotDisturb.Coordinates = new Coordinates();
                Watch_Face.System.Status.DoNotDisturb.Coordinates.X = (long)numericUpDown_DNDX.Value;
                Watch_Face.System.Status.DoNotDisturb.Coordinates.Y = (long)numericUpDown_DNDY.Value;
            }

            if (checkBox_Lock_Use.Checked && comboBox_Lock_image.SelectedIndex >= 0)
            {
                if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                if (Watch_Face.System.Status == null) Watch_Face.System.Status = new Status();
                Watch_Face.System.Status.Lock = new ImageCoord();
                Watch_Face.System.Status.Lock.ImageIndex = Int32.Parse(comboBox_Lock_image.Text);
                Watch_Face.System.Status.Lock.Coordinates = new Coordinates();
                Watch_Face.System.Status.Lock.Coordinates.X = (long)numericUpDown_LockX.Value;
                Watch_Face.System.Status.Lock.Coordinates.Y = (long)numericUpDown_LockY.Value;
            }
            #endregion

            #region активности

            Panel panel_pictures;
            Panel panel_text;
            Panel panel_hand;
            Panel panel_scaleCircle;
            Panel panel_scaleLinear;

            #region Battery

            panel_pictures = panel_Battery_pictures;
            panel_text = panel_Battery_text;
            panel_hand = panel_Battery_hand;
            panel_scaleCircle = panel_Battery_scaleCircle;
            panel_scaleLinear = panel_Battery_scaleLinear;

            AddActivity(panel_pictures, panel_text, panel_hand, panel_scaleCircle, panel_scaleLinear, "Battery");

            #endregion

            #region Steps
            
            panel_pictures = panel_Steps_pictures;
            panel_text = panel_Steps_text;
            panel_hand = panel_Steps_hand;
            panel_scaleCircle = panel_Steps_scaleCircle;
            panel_scaleLinear = panel_Steps_scaleLinear;

            AddActivity(panel_pictures, panel_text, panel_hand, panel_scaleCircle, panel_scaleLinear, "Steps");

            #endregion

            #region Calories
            
            panel_pictures = panel_Calories_pictures;
            panel_text = panel_Calories_text;
            panel_hand = panel_Calories_hand;
            panel_scaleCircle = panel_Calories_scaleCircle;
            panel_scaleLinear = panel_Calories_scaleLinear;

            AddActivity(panel_pictures, panel_text, panel_hand, panel_scaleCircle, panel_scaleLinear, "Calories");

            #endregion

            #region HeartRate

            panel_pictures = panel_HeartRate_pictures;
            panel_text = panel_HeartRate_text;
            panel_hand = panel_HeartRate_hand;
            panel_scaleCircle = panel_HeartRate_scaleCircle;
            panel_scaleLinear = panel_HeartRate_scaleLinear;

            AddActivity(panel_pictures, panel_text, panel_hand, panel_scaleCircle, panel_scaleLinear, "HeartRate");

            #endregion

            #region PAI

            panel_pictures = panel_PAI_pictures;
            panel_text = panel_PAI_text;
            panel_hand = panel_PAI_hand;
            panel_scaleCircle = panel_PAI_scaleCircle;
            panel_scaleLinear = panel_PAI_scaleLinear;

            AddActivity(panel_pictures, panel_text, panel_hand, panel_scaleCircle, panel_scaleLinear, "PAI");

            #endregion

            #region Distance

            panel_pictures = panel_Distance_pictures;
            panel_text = panel_Distance_text;
            panel_hand = panel_Distance_hand;
            panel_scaleCircle = panel_Distance_scaleCircle;
            panel_scaleLinear = panel_Distance_scaleLinear;

            AddActivity(panel_pictures, panel_text, panel_hand, panel_scaleCircle, panel_scaleLinear, "Distance");

            #endregion

            #region Weather

            panel_pictures = panel_Weather_pictures;
            panel_text = panel_Weather_text;
            panel_hand = panel_Weather_hand;
            panel_scaleCircle = panel_Weather_scaleCircle;
            panel_scaleLinear = panel_Weather_scaleLinear;
            Panel panel_text_min = panel_Weather_textMin;
            Panel panel_text_max = panel_Weather_textMax;

            AddActivityWeather(panel_pictures, panel_text, panel_text_min, panel_text_max, panel_hand, panel_scaleCircle, panel_scaleLinear);

            #endregion

            #endregion



            if (Watch_Face.Widgets != null) WidgetsTemp = Watch_Face.Widgets;
            if (Watch_Face.ScreenIdle != null) ScreenIdleTemp = Watch_Face.ScreenIdle;

            if (WidgetsTemp != null) Watch_Face.Widgets = WidgetsTemp;
            if (ScreenIdleTemp != null) Watch_Face.ScreenIdle = ScreenIdleTemp;

            //if ((comboBox_Background.SelectedIndex >= 0) || (comboBox_Preview.SelectedIndex >= 0))
            //{
            //    Watch_Face.Background = new Background();
            //    if (comboBox_Background.SelectedIndex >= 0)
            //    {
            //        Watch_Face.Background.Image = new ImageW();
            //        Watch_Face.Background.Image.ImageIndex = Int32.Parse(comboBox_Background.Text);
            //    }
            //    if (comboBox_Preview.SelectedIndex >= 0)
            //    {
            //        Watch_Face.Background.Preview = new ImageW();
            //        Watch_Face.Background.Preview.ImageIndex = Int32.Parse(comboBox_Preview.Text);
            //    }
            //}

            //// время 
            //if (checkBox_Time.Checked)
            //{
            //    if ((checkBox_Hours.Checked) && (comboBox_Hours_Tens_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.Time == null) Watch_Face.Time = new TimeW();
            //        if (Watch_Face.Time.Hours == null) Watch_Face.Time.Hours = new TwoDigits();

            //        Watch_Face.Time.Hours.Tens = new ImageSet();
            //        Watch_Face.Time.Hours.Tens.ImageIndex = Int32.Parse(comboBox_Hours_Tens_Image.Text);
            //        Watch_Face.Time.Hours.Tens.ImagesCount = (int)numericUpDown_Hours_Tens_Count.Value;
            //        Watch_Face.Time.Hours.Tens.X = (int)numericUpDown_Hours_Tens_X.Value;
            //        Watch_Face.Time.Hours.Tens.Y = (int)numericUpDown_Hours_Tens_Y.Value;
            //    }
            //    if ((checkBox_Hours.Checked) && (comboBox_Hours_Ones_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.Time == null) Watch_Face.Time = new TimeW();
            //        if (Watch_Face.Time.Hours == null) Watch_Face.Time.Hours = new TwoDigits();

            //        Watch_Face.Time.Hours.Ones = new ImageSet();
            //        Watch_Face.Time.Hours.Ones.ImageIndex = Int32.Parse(comboBox_Hours_Ones_Image.Text);
            //        Watch_Face.Time.Hours.Ones.ImagesCount = (int)numericUpDown_Hours_Ones_Count.Value;
            //        Watch_Face.Time.Hours.Ones.X = (int)numericUpDown_Hours_Ones_X.Value;
            //        Watch_Face.Time.Hours.Ones.Y = (int)numericUpDown_Hours_Ones_Y.Value;
            //    }

            //    if ((checkBox_Minutes.Checked) && (comboBox_Min_Tens_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.Time == null) Watch_Face.Time = new TimeW();
            //        if (Watch_Face.Time.Minutes == null) Watch_Face.Time.Minutes = new TwoDigits();

            //        Watch_Face.Time.Minutes.Tens = new ImageSet();
            //        Watch_Face.Time.Minutes.Tens.ImageIndex = Int32.Parse(comboBox_Min_Tens_Image.Text);
            //        Watch_Face.Time.Minutes.Tens.ImagesCount = (int)numericUpDown_Min_Tens_Count.Value;
            //        Watch_Face.Time.Minutes.Tens.X = (int)numericUpDown_Min_Tens_X.Value;
            //        Watch_Face.Time.Minutes.Tens.Y = (int)numericUpDown_Min_Tens_Y.Value;
            //    }
            //    if ((checkBox_Minutes.Checked) && (comboBox_Min_Ones_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.Time == null) Watch_Face.Time = new TimeW();
            //        if (Watch_Face.Time.Minutes == null) Watch_Face.Time.Minutes = new TwoDigits();

            //        Watch_Face.Time.Minutes.Ones = new ImageSet();
            //        Watch_Face.Time.Minutes.Ones.ImageIndex = Int32.Parse(comboBox_Min_Ones_Image.Text);
            //        Watch_Face.Time.Minutes.Ones.ImagesCount = (int)numericUpDown_Min_Ones_Count.Value;
            //        Watch_Face.Time.Minutes.Ones.X = (int)numericUpDown_Min_Ones_X.Value;
            //        Watch_Face.Time.Minutes.Ones.Y = (int)numericUpDown_Min_Ones_Y.Value;
            //    }

            //    if ((checkBox_Seconds.Checked) && (comboBox_Sec_Tens_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.Time == null) Watch_Face.Time = new TimeW();
            //        if (Watch_Face.Time.Seconds == null) Watch_Face.Time.Seconds = new TwoDigits();

            //        Watch_Face.Time.Seconds.Tens = new ImageSet();
            //        Watch_Face.Time.Seconds.Tens.ImageIndex = Int32.Parse(comboBox_Sec_Tens_Image.Text);
            //        Watch_Face.Time.Seconds.Tens.ImagesCount = (int)numericUpDown_Sec_Tens_Count.Value;
            //        Watch_Face.Time.Seconds.Tens.X = (int)numericUpDown_Sec_Tens_X.Value;
            //        Watch_Face.Time.Seconds.Tens.Y = (int)numericUpDown_Sec_Tens_Y.Value;
            //    }
            //    if ((checkBox_Seconds.Checked) && (comboBox_Sec_Ones_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.Time == null) Watch_Face.Time = new TimeW();
            //        if (Watch_Face.Time.Seconds == null) Watch_Face.Time.Seconds = new TwoDigits();

            //        Watch_Face.Time.Seconds.Ones = new ImageSet();
            //        Watch_Face.Time.Seconds.Ones.ImageIndex = Int32.Parse(comboBox_Sec_Ones_Image.Text);
            //        Watch_Face.Time.Seconds.Ones.ImagesCount = (int)numericUpDown_Sec_Ones_Count.Value;
            //        Watch_Face.Time.Seconds.Ones.X = (int)numericUpDown_Sec_Ones_X.Value;
            //        Watch_Face.Time.Seconds.Ones.Y = (int)numericUpDown_Sec_Ones_Y.Value;
            //    }

            //    if ((checkBox_AmPm.Checked) && (comboBox_Image_Am.SelectedIndex >= 0) &&
            //        (comboBox_Image_Pm.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.Time == null) Watch_Face.Time = new TimeW();
            //        if (Watch_Face.Time.AmPm == null) Watch_Face.Time.AmPm = new AmPm();

            //        Watch_Face.Time.AmPm.ImageIndexAMCN = Int32.Parse(comboBox_Image_Am.Text);
            //        //Watch_Face.Time.AmPm.ImageIndexAMEN = Int32.Parse(comboBox_Image_Am.Text);
            //        Watch_Face.Time.AmPm.ImageIndexPMCN = Int32.Parse(comboBox_Image_Pm.Text);
            //        //Watch_Face.Time.AmPm.ImageIndexPMEN = Int32.Parse(comboBox_Image_Pm.Text);
            //        Watch_Face.Time.AmPm.X = (int)numericUpDown_AmPm_X.Value;
            //        Watch_Face.Time.AmPm.Y = (int)numericUpDown_AmPm_Y.Value;
            //    }

            //    if ((checkBox_Delimiter.Checked) && (comboBox_Delimiter_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.Time == null) Watch_Face.Time = new TimeW();
            //        if (Watch_Face.Time.Delimiter == null) Watch_Face.Time.Delimiter = new ImageW();

            //        Watch_Face.Time.Delimiter.ImageIndex = Int32.Parse(comboBox_Delimiter_Image.Text);
            //        Watch_Face.Time.Delimiter.X = (int)numericUpDown_Delimiter_X.Value;
            //        Watch_Face.Time.Delimiter.Y = (int)numericUpDown_Delimiter_Y.Value;
            //    }
            //}

            //// активити
            //if (checkBox_Activity.Checked)
            //{
            //    if ((checkBox_ActivitySteps.Checked) && (comboBox_ActivitySteps_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.Activity == null) Watch_Face.Activity = new Activity();
            //        if (Watch_Face.Activity.Steps == null) Watch_Face.Activity.Steps = new FormattedNumber();
            //        if (Watch_Face.Activity.Steps.Step == null) Watch_Face.Activity.Steps.Step = new Number();

            //        Watch_Face.Activity.Steps.Step.ImageIndex = Int32.Parse(comboBox_ActivitySteps_Image.Text);
            //        Watch_Face.Activity.Steps.Step.ImagesCount = (int)numericUpDown_ActivitySteps_Count.Value;
            //        Watch_Face.Activity.Steps.Step.TopLeftX = (int)numericUpDown_ActivitySteps_StartCorner_X.Value;
            //        Watch_Face.Activity.Steps.Step.TopLeftY = (int)numericUpDown_ActivitySteps_StartCorner_Y.Value;
            //        Watch_Face.Activity.Steps.Step.BottomRightX = (int)numericUpDown_ActivitySteps_EndCorner_X.Value;
            //        Watch_Face.Activity.Steps.Step.BottomRightY = (int)numericUpDown_ActivitySteps_EndCorner_Y.Value;

            //        Watch_Face.Activity.Steps.Step.Spacing = (int)numericUpDown_ActivitySteps_Spacing.Value;
            //        string Alignment = StringToAlignment(comboBox_ActivitySteps_Alignment.SelectedIndex);
            //        Watch_Face.Activity.Steps.Step.Alignment = Alignment;
            //    }

            //    if ((checkBox_ActivityDistance.Checked) && (comboBox_ActivityDistance_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.Activity == null) Watch_Face.Activity = new Activity();
            //        if (Watch_Face.Activity.Distance == null) Watch_Face.Activity.Distance = new Distance();
            //        if (Watch_Face.Activity.Distance.Number == null) Watch_Face.Activity.Distance.Number = new Number();

            //        Watch_Face.Activity.Distance.Number.ImageIndex = Int32.Parse(comboBox_ActivityDistance_Image.Text);
            //        Watch_Face.Activity.Distance.Number.ImagesCount = (int)numericUpDown_ActivityDistance_Count.Value;
            //        Watch_Face.Activity.Distance.Number.TopLeftX = (int)numericUpDown_ActivityDistance_StartCorner_X.Value;
            //        Watch_Face.Activity.Distance.Number.TopLeftY = (int)numericUpDown_ActivityDistance_StartCorner_Y.Value;
            //        Watch_Face.Activity.Distance.Number.BottomRightX = (int)numericUpDown_ActivityDistance_EndCorner_X.Value;
            //        Watch_Face.Activity.Distance.Number.BottomRightY = (int)numericUpDown_ActivityDistance_EndCorner_Y.Value;

            //        Watch_Face.Activity.Distance.Number.Spacing = (int)numericUpDown_ActivityDistance_Spacing.Value;
            //        string Alignment = StringToAlignment(comboBox_ActivityDistance_Alignment.SelectedIndex);
            //        Watch_Face.Activity.Distance.Number.Alignment = Alignment;

            //        if ((comboBox_ActivityDistance_Suffix_km.SelectedIndex >= 0) &&
            //            (comboBox_ActivityDistance_Suffix_km.Text.Length > 0))
            //            Watch_Face.Activity.Distance.SuffixImageIndex = Int32.Parse(comboBox_ActivityDistance_Suffix_km.Text);

            //        if ((comboBox_ActivityDistance_Suffix_ml.SelectedIndex >= 0) &&
            //            (comboBox_ActivityDistance_Suffix_ml.Text.Length > 0))
            //        {
            //            Color new_color = Color.FromArgb(0, 0, 0, Int32.Parse(comboBox_ActivityDistance_Suffix_ml.Text));
            //            string colorStr = ColorTranslator.ToHtml(new_color);
            //            colorStr = colorStr.Replace("#", "0x00");
            //            Watch_Face.Activity.Distance.Color = colorStr;
            //        }

            //        if ((comboBox_ActivityDistance_Decimal.SelectedIndex >= 0) &&
            //            (comboBox_ActivityDistance_Decimal.Text.Length > 0))
            //            Watch_Face.Activity.Distance.DecimalPointImageIndex = Int32.Parse(comboBox_ActivityDistance_Decimal.Text);
            //    }

            //    if ((checkBox_ActivityPuls.Checked) && (comboBox_ActivityPuls_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.Activity == null) Watch_Face.Activity = new Activity();
            //        if (Watch_Face.Activity.HeartRate == null) Watch_Face.Activity.HeartRate = new Number();

            //        Watch_Face.Activity.HeartRate.ImageIndex = Int32.Parse(comboBox_ActivityPuls_Image.Text);
            //        Watch_Face.Activity.HeartRate.ImagesCount = (int)numericUpDown_ActivityPuls_Count.Value;
            //        Watch_Face.Activity.HeartRate.TopLeftX = (int)numericUpDown_ActivityPuls_StartCorner_X.Value;
            //        Watch_Face.Activity.HeartRate.TopLeftY = (int)numericUpDown_ActivityPuls_StartCorner_Y.Value;
            //        Watch_Face.Activity.HeartRate.BottomRightX = (int)numericUpDown_ActivityPuls_EndCorner_X.Value;
            //        Watch_Face.Activity.HeartRate.BottomRightY = (int)numericUpDown_ActivityPuls_EndCorner_Y.Value;

            //        Watch_Face.Activity.HeartRate.Spacing = (int)numericUpDown_ActivityPuls_Spacing.Value;
            //        string Alignment = StringToAlignment(comboBox_ActivityPuls_Alignment.SelectedIndex);
            //        Watch_Face.Activity.HeartRate.Alignment = Alignment;
            //    }

            //    // пульс набором иконок
            //    if (checkBox_ActivityPuls_IconSet.Checked)
            //    {
            //        if ((comboBox_ActivityPuls_IconSet_Image.SelectedIndex >= 0) && (dataGridView_ActivityPuls_IconSet.Rows.Count > 1))
            //        {
            //            if (Watch_Face.Activity == null) Watch_Face.Activity = new Activity();
            //            if (Watch_Face.Activity.ColouredSquares == null) Watch_Face.Activity.ColouredSquares = new IconSet();

            //            Watch_Face.Activity.ColouredSquares.ImageIndex = Int32.Parse(comboBox_ActivityPuls_IconSet_Image.Text);
            //            Coordinates[] coordinates = new Coordinates[0];
            //            int count = 0;

            //            foreach (DataGridViewRow row in dataGridView_ActivityPuls_IconSet.Rows)
            //            {
            //                //whatever you are currently doing
            //                //Coordinates coordinates = new Coordinates();
            //                int x = 0;
            //                int y = 0;
            //                if ((row.Cells[0].Value != null) && (row.Cells[1].Value != null))
            //                {
            //                    if ((Int32.TryParse(row.Cells[0].Value.ToString(), out x)) && (Int32.TryParse(row.Cells[1].Value.ToString(), out y)))
            //                    {

            //                        //Array.Resize(ref objson, objson.Length + 1);
            //                        Array.Resize(ref coordinates, coordinates.Length + 1);
            //                        //objson[count] = coordinates;
            //                        coordinates[count] = new Coordinates();
            //                        coordinates[count].X = x;
            //                        coordinates[count].Y = y;
            //                        count++;
            //                    }
            //                }
            //                Watch_Face.Activity.ColouredSquares.Coordinates = coordinates;
            //            }
            //        }
            //    }

            //    if (checkBox_ActivityPulsScale.Checked)
            //    {
            //        if (Watch_Face.Activity == null) Watch_Face.Activity = new Activity();
            //        if (Watch_Face.Activity.PulseMeter == null) Watch_Face.Activity.PulseMeter = new CircleScale();

            //        Watch_Face.Activity.PulseMeter.CenterX = (int)numericUpDown_ActivityPulsScale_Center_X.Value;
            //        Watch_Face.Activity.PulseMeter.CenterY = (int)numericUpDown_ActivityPulsScale_Center_Y.Value;
            //        Watch_Face.Activity.PulseMeter.RadiusX = (int)numericUpDown_ActivityPulsScale_Radius_X.Value;
            //        Watch_Face.Activity.PulseMeter.RadiusY = (int)numericUpDown_ActivityPulsScale_Radius_Y.Value;

            //        Watch_Face.Activity.PulseMeter.StartAngle = (int)numericUpDown_ActivityPulsScale_StartAngle.Value;
            //        Watch_Face.Activity.PulseMeter.EndAngle = (int)numericUpDown_ActivityPulsScale_EndAngle.Value;
            //        Watch_Face.Activity.PulseMeter.Width = (int)numericUpDown_ActivityPulsScale_Width.Value;

            //        Color color = comboBox_ActivityPulsScale_Color.BackColor;
            //        Color new_color = Color.FromArgb(0, color.R, color.G, color.B);
            //        string colorStr = ColorTranslator.ToHtml(new_color);
            //        colorStr = colorStr.Replace("#", "0x00");
            //        Watch_Face.Activity.PulseMeter.Color = colorStr;

            //        //switch (comboBox_Battery_Flatness.Text)
            //        //{
            //        //    case "Треугольное":
            //        //        Watch_Face.Battery.Scale.Flatness = 90;
            //        //        break;
            //        //    case "Плоское":
            //        //        Watch_Face.Battery.Scale.Flatness = 180;
            //        //        break;
            //        //    default:
            //        //        Watch_Face.Battery.Scale.Flatness = 0;
            //        //        break;
            //        //}
            //        switch (comboBox_ActivityPulsScale_Flatness.SelectedIndex)
            //        {
            //            case 1:
            //                Watch_Face.Activity.PulseMeter.Flatness = 90;
            //                break;
            //            case 2:
            //                Watch_Face.Activity.PulseMeter.Flatness = 180;
            //                break;
            //            default:
            //                Watch_Face.Activity.PulseMeter.Flatness = 0;
            //                break;
            //        }

            //        if (checkBox_ActivityPulsScale_Image.Checked &&
            //            comboBox_ActivityPulsScale_Image.SelectedIndex >= 0)
            //        {
            //            int imageX = (int)numericUpDown_ActivityPulsScale_ImageX.Value;
            //            int imageY = (int)numericUpDown_ActivityPulsScale_ImageY.Value;
            //            int imageIndex = comboBox_ActivityPulsScale_Image.SelectedIndex;
            //            colorStr = CoodinatesToColor(imageX, imageY);
            //            Bitmap src = new Bitmap(ListImagesFullName[imageIndex]);
            //            Watch_Face.Activity.PulseMeter.CenterX = imageX + src.Width / 2;
            //            Watch_Face.Activity.PulseMeter.CenterY = imageY + src.Height / 2;
            //            Watch_Face.Activity.PulseMeter.Color = colorStr;
            //            Watch_Face.Activity.PulseMeter.ImageIndex = imageIndex;
            //        }
            //    }

            //    if ((checkBox_Pulse_ClockHand.Checked) && (comboBox_Pulse_ClockHand_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.Activity == null) Watch_Face.Activity = new Activity();
            //        if (Watch_Face.Activity.PulseGraph == null) Watch_Face.Activity.PulseGraph = new PulseContainer();
            //        if (Watch_Face.Activity.PulseGraph.WeekDayClockHand == null) Watch_Face.Activity.PulseGraph.WeekDayClockHand = new WeekDayClockHand();
            //        if (Watch_Face.Activity.PulseGraph.WeekDayClockHand.CenterOffset == null)
            //            Watch_Face.Activity.PulseGraph.WeekDayClockHand.CenterOffset = new Coordinates();
            //        if (Watch_Face.Activity.PulseGraph.WeekDayClockHand.Sector == null)
            //            Watch_Face.Activity.PulseGraph.WeekDayClockHand.Sector = new Sector();
            //        //if (Watch_Face.MonthClockHand.Hours.Shape == null)
            //        //Watch_Face.MonthClockHand.Hours.Shape = new Coordinates();
            //        if (Watch_Face.Activity.PulseGraph.WeekDayClockHand.Image == null)
            //            Watch_Face.Activity.PulseGraph.WeekDayClockHand.Image = new ImageW();

            //        Watch_Face.Activity.PulseGraph.WeekDayClockHand.Image.ImageIndex = Int32.Parse(comboBox_Pulse_ClockHand_Image.Text);
            //        Watch_Face.Activity.PulseGraph.WeekDayClockHand.Image.X = (int)numericUpDown_Pulse_ClockHand_X.Value;
            //        Watch_Face.Activity.PulseGraph.WeekDayClockHand.Image.Y = (int)numericUpDown_Pulse_ClockHand_Y.Value;

            //        Watch_Face.Activity.PulseGraph.WeekDayClockHand.Color = "0x00000000";
            //        Watch_Face.Activity.PulseGraph.WeekDayClockHand.OnlyBorder = false;

            //        Watch_Face.Activity.PulseGraph.WeekDayClockHand.CenterOffset.X = (int)numericUpDown_Pulse_ClockHand_Offset_X.Value;
            //        Watch_Face.Activity.PulseGraph.WeekDayClockHand.CenterOffset.Y = (int)numericUpDown_Pulse_ClockHand_Offset_Y.Value;

            //        Watch_Face.Activity.PulseGraph.WeekDayClockHand.Sector.StartAngle =
            //            (int)(numericUpDown_Pulse_ClockHand_StartAngle.Value * 100);
            //        Watch_Face.Activity.PulseGraph.WeekDayClockHand.Sector.EndAngle =
            //            (int)(numericUpDown_Pulse_ClockHand_EndAngle.Value * 100);
            //    }

            //    if ((checkBox_ActivityCalories.Checked) && (comboBox_ActivityCalories_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.Activity == null) Watch_Face.Activity = new Activity();
            //        if (Watch_Face.Activity.Calories == null) Watch_Face.Activity.Calories = new Number();

            //        Watch_Face.Activity.Calories.ImageIndex = Int32.Parse(comboBox_ActivityCalories_Image.Text);
            //        Watch_Face.Activity.Calories.ImagesCount = (int)numericUpDown_ActivityCalories_Count.Value;
            //        Watch_Face.Activity.Calories.TopLeftX = (int)numericUpDown_ActivityCalories_StartCorner_X.Value;
            //        Watch_Face.Activity.Calories.TopLeftY = (int)numericUpDown_ActivityCalories_StartCorner_Y.Value;
            //        Watch_Face.Activity.Calories.BottomRightX = (int)numericUpDown_ActivityCalories_EndCorner_X.Value;
            //        Watch_Face.Activity.Calories.BottomRightY = (int)numericUpDown_ActivityCalories_EndCorner_Y.Value;

            //        Watch_Face.Activity.Calories.Spacing = (int)numericUpDown_ActivityCalories_Spacing.Value;
            //        string Alignment = StringToAlignment(comboBox_ActivityCalories_Alignment.SelectedIndex);
            //        Watch_Face.Activity.Calories.Alignment = Alignment;
            //    }

            //    if ((checkBox_ActivityStepsGoal.Checked) && (comboBox_ActivityStepsGoal_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.Activity == null) Watch_Face.Activity = new Activity();
            //        if (Watch_Face.Activity.StepsGoal == null) Watch_Face.Activity.StepsGoal = new Number();

            //        string www = comboBox_ActivityStepsGoal_Image.Text;
            //        Watch_Face.Activity.StepsGoal.ImageIndex = Int32.Parse(www);
            //        Watch_Face.Activity.StepsGoal.ImageIndex = Int32.Parse(comboBox_ActivityStepsGoal_Image.Text);
            //        Watch_Face.Activity.StepsGoal.ImagesCount = (int)numericUpDown_ActivityStepsGoal_Count.Value;
            //        Watch_Face.Activity.StepsGoal.TopLeftX = (int)numericUpDown_ActivityStepsGoal_StartCorner_X.Value;
            //        Watch_Face.Activity.StepsGoal.TopLeftY = (int)numericUpDown_ActivityStepsGoal_StartCorner_Y.Value;
            //        Watch_Face.Activity.StepsGoal.BottomRightX = (int)numericUpDown_ActivityStepsGoal_EndCorner_X.Value;
            //        Watch_Face.Activity.StepsGoal.BottomRightY = (int)numericUpDown_ActivityStepsGoal_EndCorner_Y.Value;

            //        Watch_Face.Activity.StepsGoal.Spacing = (int)numericUpDown_ActivityStepsGoal_Spacing.Value;
            //        string Alignment = StringToAlignment(comboBox_ActivityStepsGoal_Alignment.SelectedIndex);
            //        Watch_Face.Activity.StepsGoal.Alignment = Alignment;
            //    }

            //    if (checkBox_ActivityCaloriesScale.Checked)
            //    {
            //        if (Watch_Face.Activity == null) Watch_Face.Activity = new Activity();
            //        if (Watch_Face.Activity.CaloriesGraph == null) Watch_Face.Activity.CaloriesGraph = new CaloriesContainer();
            //        if (Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar == null)
            //            Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar = new CircleScale();

            //        Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.CenterX = (int)numericUpDown_ActivityCaloriesScale_Center_X.Value;
            //        Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.CenterY = (int)numericUpDown_ActivityCaloriesScale_Center_Y.Value;
            //        Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.RadiusX = (int)numericUpDown_ActivityCaloriesScale_Radius_X.Value;
            //        Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.RadiusY = (int)numericUpDown_ActivityCaloriesScale_Radius_Y.Value;

            //        Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.StartAngle = (int)numericUpDown_ActivityCaloriesScale_StartAngle.Value;
            //        Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.EndAngle = (int)numericUpDown_ActivityCaloriesScale_EndAngle.Value;
            //        Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.Width = (int)numericUpDown_ActivityCaloriesScale_Width.Value;

            //        Color color = comboBox_ActivityCaloriesScale_Color.BackColor;
            //        Color new_color = Color.FromArgb(0, color.R, color.G, color.B);
            //        string colorStr = ColorTranslator.ToHtml(new_color);
            //        colorStr = colorStr.Replace("#", "0x00");
            //        Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.Color = colorStr;

            //        switch (comboBox_ActivityCaloriesScale_Flatness.SelectedIndex)
            //        {
            //            case 1:
            //                Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.Flatness = 90;
            //                break;
            //            case 2:
            //                Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.Flatness = 180;
            //                break;
            //            default:
            //                Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.Flatness = 0;
            //                break;
            //        }

            //        if (checkBox_ActivityCaloriesScale_Image.Checked &&
            //            comboBox_ActivityCaloriesScale_Image.SelectedIndex >= 0)
            //        {
            //            int imageX = (int)numericUpDown_ActivityCaloriesScale_ImageX.Value;
            //            int imageY = (int)numericUpDown_ActivityCaloriesScale_ImageY.Value;
            //            int imageIndex = comboBox_ActivityCaloriesScale_Image.SelectedIndex;
            //            colorStr = CoodinatesToColor(imageX, imageY);
            //            Bitmap src = new Bitmap(ListImagesFullName[imageIndex]);
            //            Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.CenterX = imageX + src.Width / 2;
            //            Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.CenterY = imageY + src.Height / 2;
            //            Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.Color = colorStr;
            //            Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.ImageIndex = imageIndex;
            //        }
            //    }

            //    if ((checkBox_Calories_ClockHand.Checked) && (comboBox_Calories_ClockHand_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.Activity == null) Watch_Face.Activity = new Activity();
            //        if (Watch_Face.Activity.CaloriesGraph == null) Watch_Face.Activity.CaloriesGraph = new CaloriesContainer();
            //        if (Watch_Face.Activity.CaloriesGraph.WeekDayClockHand == null) Watch_Face.Activity.CaloriesGraph.WeekDayClockHand = new WeekDayClockHand();
            //        if (Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.CenterOffset == null)
            //            Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.CenterOffset = new Coordinates();
            //        if (Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.Sector == null)
            //            Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.Sector = new Sector();
            //        //if (Watch_Face.MonthClockHand.Hours.Shape == null)
            //        //Watch_Face.MonthClockHand.Hours.Shape = new Coordinates();
            //        if (Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.Image == null)
            //            Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.Image = new ImageW();

            //        Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.Image.ImageIndex = Int32.Parse(comboBox_Calories_ClockHand_Image.Text);
            //        Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.Image.X = (int)numericUpDown_Calories_ClockHand_X.Value;
            //        Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.Image.Y = (int)numericUpDown_Calories_ClockHand_Y.Value;

            //        Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.Color = "0x00000000";
            //        Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.OnlyBorder = false;

            //        Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.CenterOffset.X = (int)numericUpDown_Calories_ClockHand_Offset_X.Value;
            //        Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.CenterOffset.Y = (int)numericUpDown_Calories_ClockHand_Offset_Y.Value;

            //        Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.Sector.StartAngle =
            //            (int)(numericUpDown_Calories_ClockHand_StartAngle.Value * 100);
            //        Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.Sector.EndAngle =
            //            (int)(numericUpDown_Calories_ClockHand_EndAngle.Value * 100);
            //    }

            //    if ((checkBox_ActivityStar.Checked) && (comboBox_ActivityStar_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.Activity == null) Watch_Face.Activity = new Activity();
            //        if (Watch_Face.Activity.StarImage == null) Watch_Face.Activity.StarImage = new ImageW();

            //        Watch_Face.Activity.StarImage.ImageIndex = Int32.Parse(comboBox_ActivityStar_Image.Text);
            //        Watch_Face.Activity.StarImage.X = (int)numericUpDown_ActivityStar_X.Value;
            //        Watch_Face.Activity.StarImage.Y = (int)numericUpDown_ActivityStar_Y.Value;
            //    }

            //    if (comboBox_Activity_NDImage.SelectedIndex >= 0)
            //    {
            //        if (Watch_Face.Activity == null) Watch_Face.Activity = new Activity();
            //        Watch_Face.Activity.NoDataImageIndex = Int32.Parse(comboBox_Activity_NDImage.Text);
            //    }
            //}

            //// дата 
            //if (checkBox_Date.Checked)
            //{
            //    if ((checkBox_WeekDay.Checked) && (comboBox_WeekDay_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.Date == null) Watch_Face.Date = new Date();
            //        if (Watch_Face.Date.WeekDay == null) Watch_Face.Date.WeekDay = new ImageSet();

            //        Watch_Face.Date.WeekDay.ImageIndex = Int32.Parse(comboBox_WeekDay_Image.Text);
            //        Watch_Face.Date.WeekDay.ImagesCount = (int)numericUpDown_WeekDay_Count.Value;
            //        Watch_Face.Date.WeekDay.X = (int)numericUpDown_WeekDay_X.Value;
            //        Watch_Face.Date.WeekDay.Y = (int)numericUpDown_WeekDay_Y.Value;
            //    }

            //    // день недели набором иконок
            //    if (checkBox_DOW_IconSet.Checked)
            //    {
            //        if ((comboBox_DOW_IconSet_Image.SelectedIndex >= 0) && (dataGridView_DOW_IconSet.Rows.Count > 1))
            //        {
            //            if (Watch_Face.Date == null) Watch_Face.Date = new Date();
            //            if (Watch_Face.Date.WeekDayProgress == null) Watch_Face.Date.WeekDayProgress = new IconSet();

            //            Watch_Face.Date.WeekDayProgress.ImageIndex = Int32.Parse(comboBox_DOW_IconSet_Image.Text);
            //            Coordinates[] coordinates = new Coordinates[0];
            //            int count = 0;

            //            foreach (DataGridViewRow row in dataGridView_DOW_IconSet.Rows)
            //            {
            //                //whatever you are currently doing
            //                //Coordinates coordinates = new Coordinates();
            //                int x = 0;
            //                int y = 0;
            //                if ((row.Cells[0].Value != null) && (row.Cells[1].Value != null))
            //                {
            //                    if ((Int32.TryParse(row.Cells[0].Value.ToString(), out x)) && (Int32.TryParse(row.Cells[1].Value.ToString(), out y)))
            //                    {

            //                        //Array.Resize(ref objson, objson.Length + 1);
            //                        Array.Resize(ref coordinates, coordinates.Length + 1);
            //                        //objson[count] = coordinates;
            //                        coordinates[count] = new Coordinates();
            //                        coordinates[count].X = x;
            //                        coordinates[count].Y = y;
            //                        count++;
            //                    }
            //                }
            //                Watch_Face.Date.WeekDayProgress.Coordinates = coordinates;
            //            }
            //        }
            //    }

            //    if ((checkBox_MonthName.Checked) && (comboBox_MonthName_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.Date == null) Watch_Face.Date = new Date();
            //        if (Watch_Face.Date.MonthAndDay == null) Watch_Face.Date.MonthAndDay = new MonthAndDay();
            //        if (Watch_Face.Date.MonthAndDay.Separate == null)
            //            Watch_Face.Date.MonthAndDay.Separate = new SeparateMonthAndDay();
            //        if (Watch_Face.Date.MonthAndDay.Separate.MonthName == null)
            //            Watch_Face.Date.MonthAndDay.Separate.MonthName = new ImageSet();

            //        Watch_Face.Date.MonthAndDay.Separate.MonthName.ImageIndex = Int32.Parse(comboBox_MonthName_Image.Text);
            //        Watch_Face.Date.MonthAndDay.Separate.MonthName.ImagesCount = (int)numericUpDown_MonthName_Count.Value;
            //        Watch_Face.Date.MonthAndDay.Separate.MonthName.X = (int)numericUpDown_MonthName_X.Value;
            //        Watch_Face.Date.MonthAndDay.Separate.MonthName.Y = (int)numericUpDown_MonthName_Y.Value;
            //    }
            //    if ((checkBox_MonthAndDayD.Checked) && (comboBox_MonthAndDayD_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.Date == null) Watch_Face.Date = new Date();
            //        if (Watch_Face.Date.MonthAndDay == null) Watch_Face.Date.MonthAndDay = new MonthAndDay();
            //        if (Watch_Face.Date.MonthAndDay.Separate == null)
            //            Watch_Face.Date.MonthAndDay.Separate = new SeparateMonthAndDay();
            //        if (Watch_Face.Date.MonthAndDay.Separate.Day == null)
            //            Watch_Face.Date.MonthAndDay.Separate.Day = new Number();

            //        Watch_Face.Date.MonthAndDay.Separate.Day.ImageIndex = Int32.Parse(comboBox_MonthAndDayD_Image.Text);
            //        Watch_Face.Date.MonthAndDay.Separate.Day.ImagesCount = (int)numericUpDown_MonthAndDayD_Count.Value;
            //        Watch_Face.Date.MonthAndDay.Separate.Day.TopLeftX = (int)numericUpDown_MonthAndDayD_StartCorner_X.Value;
            //        Watch_Face.Date.MonthAndDay.Separate.Day.TopLeftY = (int)numericUpDown_MonthAndDayD_StartCorner_Y.Value;
            //        Watch_Face.Date.MonthAndDay.Separate.Day.BottomRightX = (int)numericUpDown_MonthAndDayD_EndCorner_X.Value;
            //        Watch_Face.Date.MonthAndDay.Separate.Day.BottomRightY = (int)numericUpDown_MonthAndDayD_EndCorner_Y.Value;

            //        Watch_Face.Date.MonthAndDay.Separate.Day.Spacing = (int)numericUpDown_MonthAndDayD_Spacing.Value;
            //        string Alignment = StringToAlignment(comboBox_MonthAndDayD_Alignment.SelectedIndex);
            //        Watch_Face.Date.MonthAndDay.Separate.Day.Alignment = Alignment;
            //    }
            //    if ((checkBox_MonthAndDayM.Checked) && (comboBox_MonthAndDayM_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.Date == null) Watch_Face.Date = new Date();
            //        if (Watch_Face.Date.MonthAndDay == null) Watch_Face.Date.MonthAndDay = new MonthAndDay();
            //        if (Watch_Face.Date.MonthAndDay.Separate == null)
            //            Watch_Face.Date.MonthAndDay.Separate = new SeparateMonthAndDay();
            //        if (Watch_Face.Date.MonthAndDay.Separate.Month == null)
            //            Watch_Face.Date.MonthAndDay.Separate.Month = new Number();

            //        Watch_Face.Date.MonthAndDay.Separate.Month.ImageIndex = Int32.Parse(comboBox_MonthAndDayM_Image.Text);
            //        Watch_Face.Date.MonthAndDay.Separate.Month.ImagesCount = (int)numericUpDown_MonthAndDayM_Count.Value;
            //        Watch_Face.Date.MonthAndDay.Separate.Month.TopLeftX = (int)numericUpDown_MonthAndDayM_StartCorner_X.Value;
            //        Watch_Face.Date.MonthAndDay.Separate.Month.TopLeftY = (int)numericUpDown_MonthAndDayM_StartCorner_Y.Value;
            //        Watch_Face.Date.MonthAndDay.Separate.Month.BottomRightX = (int)numericUpDown_MonthAndDayM_EndCorner_X.Value;
            //        Watch_Face.Date.MonthAndDay.Separate.Month.BottomRightY = (int)numericUpDown_MonthAndDayM_EndCorner_Y.Value;

            //        Watch_Face.Date.MonthAndDay.Separate.Month.Spacing = (int)numericUpDown_MonthAndDayM_Spacing.Value;
            //        string Alignment = StringToAlignment(comboBox_MonthAndDayM_Alignment.SelectedIndex);
            //        Watch_Face.Date.MonthAndDay.Separate.Month.Alignment = Alignment;
            //    }

            //    if ((checkBox_OneLine.Checked) && (comboBox_OneLine_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.Date == null) Watch_Face.Date = new Date();
            //        if (Watch_Face.Date.MonthAndDay == null) Watch_Face.Date.MonthAndDay = new MonthAndDay();
            //        if (Watch_Face.Date.MonthAndDay.OneLine == null)
            //            Watch_Face.Date.MonthAndDay.OneLine = new OneLineMonthAndDay();
            //        if (Watch_Face.Date.MonthAndDay.OneLine.Number == null)
            //            Watch_Face.Date.MonthAndDay.OneLine.Number = new Number();

            //        Watch_Face.Date.MonthAndDay.OneLine.Number.ImageIndex = Int32.Parse(comboBox_OneLine_Image.Text);
            //        Watch_Face.Date.MonthAndDay.OneLine.Number.ImagesCount = (int)numericUpDown_OneLine_Count.Value;
            //        Watch_Face.Date.MonthAndDay.OneLine.Number.TopLeftX = (int)numericUpDown_OneLine_StartCorner_X.Value;
            //        Watch_Face.Date.MonthAndDay.OneLine.Number.TopLeftY = (int)numericUpDown_OneLine_StartCorner_Y.Value;
            //        Watch_Face.Date.MonthAndDay.OneLine.Number.BottomRightX = (int)numericUpDown_OneLine_EndCorner_X.Value;
            //        Watch_Face.Date.MonthAndDay.OneLine.Number.BottomRightY = (int)numericUpDown_OneLine_EndCorner_Y.Value;

            //        Watch_Face.Date.MonthAndDay.OneLine.Number.Spacing = (int)numericUpDown_OneLine_Spacing.Value;
            //        string Alignment = StringToAlignment(comboBox_OneLine_Alignment.SelectedIndex);
            //        Watch_Face.Date.MonthAndDay.OneLine.Number.Alignment = Alignment;

            //        if (comboBox_OneLine_Delimiter.SelectedIndex >= 0)
            //            Watch_Face.Date.MonthAndDay.OneLine.DelimiterImageIndex = Int32.Parse(comboBox_OneLine_Delimiter.Text);
            //    }

            //    if ((Watch_Face.Date != null) && (Watch_Face.Date.MonthAndDay != null))
            //    {
            //        Watch_Face.Date.MonthAndDay.TwoDigitsMonth = checkBox_TwoDigitsMonth.Checked;
            //        Watch_Face.Date.MonthAndDay.TwoDigitsDay = checkBox_TwoDigitsDay.Checked;
            //    }

            //    if ((checkBox_Year.Checked) && (comboBox_Year_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.Date == null) Watch_Face.Date = new Date();
            //        if (Watch_Face.Date.Year == null) Watch_Face.Date.Year = new Year();
            //        if (Watch_Face.Date.Year.OneLine == null)
            //            Watch_Face.Date.Year.OneLine = new OneLineYear();
            //        if (Watch_Face.Date.Year.OneLine.Number == null)
            //            Watch_Face.Date.Year.OneLine.Number = new Number();

            //        Watch_Face.Date.Year.OneLine.Number.ImageIndex = Int32.Parse(comboBox_Year_Image.Text);
            //        Watch_Face.Date.Year.OneLine.Number.ImagesCount = (int)numericUpDown_Year_Count.Value;
            //        Watch_Face.Date.Year.OneLine.Number.TopLeftX = (int)numericUpDown_Year_StartCorner_X.Value;
            //        Watch_Face.Date.Year.OneLine.Number.TopLeftY = (int)numericUpDown_Year_StartCorner_Y.Value;
            //        Watch_Face.Date.Year.OneLine.Number.BottomRightX = (int)numericUpDown_Year_EndCorner_X.Value;
            //        Watch_Face.Date.Year.OneLine.Number.BottomRightY = (int)numericUpDown_Year_EndCorner_Y.Value;

            //        Watch_Face.Date.Year.OneLine.Number.Spacing = (int)numericUpDown_Year_Spacing.Value;
            //        string Alignment = StringToAlignment(comboBox_Year_Alignment.SelectedIndex);
            //        Watch_Face.Date.Year.OneLine.Number.Alignment = Alignment;

            //        if (comboBox_Year_Delimiter.SelectedIndex >= 0)
            //            Watch_Face.Date.Year.OneLine.DelimiterImageIndex = Int32.Parse(comboBox_Year_Delimiter.Text);
            //    }
            //}

            //// число стрелкой
            //if ((checkBox_ADDay_ClockHand.Checked) && (comboBox_ADDay_ClockHand_Image.SelectedIndex >= 0))
            //{
            //    if (Watch_Face.DaysProgress == null) Watch_Face.DaysProgress = new DaysProgress();
            //    if (Watch_Face.DaysProgress.UnknownField2 == null) Watch_Face.DaysProgress.UnknownField2 = new WeekDayClockHand();
            //    if (Watch_Face.DaysProgress.UnknownField2.CenterOffset == null)
            //        Watch_Face.DaysProgress.UnknownField2.CenterOffset = new Coordinates();
            //    if (Watch_Face.DaysProgress.UnknownField2.Sector == null)
            //        Watch_Face.DaysProgress.UnknownField2.Sector = new Sector();
            //    if (Watch_Face.DaysProgress.UnknownField2.Image == null)
            //        Watch_Face.DaysProgress.UnknownField2.Image = new ImageW();

            //    Watch_Face.DaysProgress.UnknownField2.Image.ImageIndex = Int32.Parse(comboBox_ADDay_ClockHand_Image.Text);
            //    Watch_Face.DaysProgress.UnknownField2.Image.X = (int)numericUpDown_ADDay_ClockHand_X.Value;
            //    Watch_Face.DaysProgress.UnknownField2.Image.Y = (int)numericUpDown_ADDay_ClockHand_Y.Value;

            //    Watch_Face.DaysProgress.UnknownField2.Color = "0x00000000";
            //    Watch_Face.DaysProgress.UnknownField2.OnlyBorder = false;

            //    Watch_Face.DaysProgress.UnknownField2.CenterOffset.X = (int)numericUpDown_ADDay_ClockHand_Offset_X.Value;
            //    Watch_Face.DaysProgress.UnknownField2.CenterOffset.Y = (int)numericUpDown_ADDay_ClockHand_Offset_Y.Value;

            //    Watch_Face.DaysProgress.UnknownField2.Sector.StartAngle =
            //        (int)(numericUpDown_ADDay_ClockHand_StartAngle.Value * 100);
            //    Watch_Face.DaysProgress.UnknownField2.Sector.EndAngle =
            //        (int)(numericUpDown_ADDay_ClockHand_EndAngle.Value * 100);
            //}

            //// день недели стрелкой
            //if ((checkBox_ADWeekDay_ClockHand.Checked) && (comboBox_ADWeekDay_ClockHand_Image.SelectedIndex >= 0))
            //{
            //    if (Watch_Face.DaysProgress == null) Watch_Face.DaysProgress = new DaysProgress();
            //    if (Watch_Face.DaysProgress.AnalogDOW == null) Watch_Face.DaysProgress.AnalogDOW = new WeekDayClockHand();
            //    if (Watch_Face.DaysProgress.AnalogDOW.CenterOffset == null)
            //        Watch_Face.DaysProgress.AnalogDOW.CenterOffset = new Coordinates();
            //    if (Watch_Face.DaysProgress.AnalogDOW.Sector == null)
            //        Watch_Face.DaysProgress.AnalogDOW.Sector = new Sector();
            //    if (Watch_Face.DaysProgress.AnalogDOW.Image == null)
            //        Watch_Face.DaysProgress.AnalogDOW.Image = new ImageW();

            //    Watch_Face.DaysProgress.AnalogDOW.Image.ImageIndex = Int32.Parse(comboBox_ADWeekDay_ClockHand_Image.Text);
            //    Watch_Face.DaysProgress.AnalogDOW.Image.X = (int)numericUpDown_ADWeekDay_ClockHand_X.Value;
            //    Watch_Face.DaysProgress.AnalogDOW.Image.Y = (int)numericUpDown_ADWeekDay_ClockHand_Y.Value;

            //    Watch_Face.DaysProgress.AnalogDOW.Color = "0x00000000";
            //    Watch_Face.DaysProgress.AnalogDOW.OnlyBorder = false;

            //    Watch_Face.DaysProgress.AnalogDOW.CenterOffset.X = (int)numericUpDown_ADWeekDay_ClockHand_Offset_X.Value;
            //    Watch_Face.DaysProgress.AnalogDOW.CenterOffset.Y = (int)numericUpDown_ADWeekDay_ClockHand_Offset_Y.Value;

            //    Watch_Face.DaysProgress.AnalogDOW.Sector.StartAngle =
            //        (int)(numericUpDown_ADWeekDay_ClockHand_StartAngle.Value * 100);
            //    Watch_Face.DaysProgress.AnalogDOW.Sector.EndAngle =
            //        (int)(numericUpDown_ADWeekDay_ClockHand_EndAngle.Value * 100);
            //}

            //// месяц стрелкой
            //if ((checkBox_ADMonth_ClockHand.Checked) && (comboBox_ADMonth_ClockHand_Image.SelectedIndex >= 0))
            //{
            //    if (Watch_Face.DaysProgress == null) Watch_Face.DaysProgress = new DaysProgress();
            //    if (Watch_Face.DaysProgress.AnalogMonth == null) Watch_Face.DaysProgress.AnalogMonth = new WeekDayClockHand();
            //    if (Watch_Face.DaysProgress.AnalogMonth.CenterOffset == null)
            //        Watch_Face.DaysProgress.AnalogMonth.CenterOffset = new Coordinates();
            //    if (Watch_Face.DaysProgress.AnalogMonth.Sector == null)
            //        Watch_Face.DaysProgress.AnalogMonth.Sector = new Sector();
            //    if (Watch_Face.DaysProgress.AnalogMonth.Image == null)
            //        Watch_Face.DaysProgress.AnalogMonth.Image = new ImageW();

            //    Watch_Face.DaysProgress.AnalogMonth.Image.ImageIndex = Int32.Parse(comboBox_ADMonth_ClockHand_Image.Text);
            //    Watch_Face.DaysProgress.AnalogMonth.Image.X = (int)numericUpDown_ADMonth_ClockHand_X.Value;
            //    Watch_Face.DaysProgress.AnalogMonth.Image.Y = (int)numericUpDown_ADMonth_ClockHand_Y.Value;

            //    Watch_Face.DaysProgress.AnalogMonth.Color = "0x00000000";
            //    Watch_Face.DaysProgress.AnalogMonth.OnlyBorder = false;

            //    Watch_Face.DaysProgress.AnalogMonth.CenterOffset.X = (int)numericUpDown_ADMonth_ClockHand_Offset_X.Value;
            //    Watch_Face.DaysProgress.AnalogMonth.CenterOffset.Y = (int)numericUpDown_ADMonth_ClockHand_Offset_Y.Value;

            //    Watch_Face.DaysProgress.AnalogMonth.Sector.StartAngle =
            //        (int)(numericUpDown_ADMonth_ClockHand_StartAngle.Value * 100);
            //    Watch_Face.DaysProgress.AnalogMonth.Sector.EndAngle =
            //        (int)(numericUpDown_ADMonth_ClockHand_EndAngle.Value * 100);
            //}

            //// прогресc шагов
            //if (checkBox_StepsProgress.Checked)
            //{
            //    if (Watch_Face.StepsProgress == null) Watch_Face.StepsProgress = new StepsProgress();
            //    if (Watch_Face.StepsProgress.WeekDayProgressBar == null) Watch_Face.StepsProgress.WeekDayProgressBar = new CircleScale();

            //    Watch_Face.StepsProgress.WeekDayProgressBar.CenterX = (int)numericUpDown_StepsProgress_Center_X.Value;
            //    Watch_Face.StepsProgress.WeekDayProgressBar.CenterY = (int)numericUpDown_StepsProgress_Center_Y.Value;
            //    Watch_Face.StepsProgress.WeekDayProgressBar.RadiusX = (int)numericUpDown_StepsProgress_Radius_X.Value;
            //    Watch_Face.StepsProgress.WeekDayProgressBar.RadiusY = (int)numericUpDown_StepsProgress_Radius_Y.Value;

            //    Watch_Face.StepsProgress.WeekDayProgressBar.StartAngle = (int)numericUpDown_StepsProgress_StartAngle.Value;
            //    Watch_Face.StepsProgress.WeekDayProgressBar.EndAngle = (int)numericUpDown_StepsProgress_EndAngle.Value;
            //    Watch_Face.StepsProgress.WeekDayProgressBar.Width = (int)numericUpDown_StepsProgress_Width.Value;

            //    Color color = comboBox_StepsProgress_Color.BackColor;
            //    Color new_color = Color.FromArgb(0, color.R, color.G, color.B);
            //    string colorStr = ColorTranslator.ToHtml(new_color);
            //    colorStr = colorStr.Replace("#", "0x00");
            //    Watch_Face.StepsProgress.WeekDayProgressBar.Color = colorStr;

            //    //switch (comboBox_StepsProgress_Flatness.Text)
            //    //{
            //    //    case "Треугольное":
            //    //        Watch_Face.StepsProgress.WeekDayProgressBar.Flatness = 90;
            //    //        break;
            //    //    case "Плоское":
            //    //        Watch_Face.StepsProgress.WeekDayProgressBar.Flatness = 180;
            //    //        break;
            //    //    default:
            //    //        Watch_Face.StepsProgress.WeekDayProgressBar.Flatness = 0;
            //    //        break;
            //    //}
            //    switch (comboBox_StepsProgress_Flatness.SelectedIndex)
            //    {
            //        case 1:
            //            Watch_Face.StepsProgress.WeekDayProgressBar.Flatness = 90;
            //            break;
            //        case 2:
            //            Watch_Face.StepsProgress.WeekDayProgressBar.Flatness = 180;
            //            break;
            //        default:
            //            Watch_Face.StepsProgress.WeekDayProgressBar.Flatness = 0;
            //            break;
            //    }

            //    if (checkBox_StepsProgress_Image.Checked &&
            //        comboBox_StepsProgress_Image.SelectedIndex >= 0)
            //    {
            //        int imageX = (int)numericUpDown_StepsProgress_ImageX.Value;
            //        int imageY = (int)numericUpDown_StepsProgress_ImageY.Value;
            //        int imageIndex = comboBox_StepsProgress_Image.SelectedIndex;
            //        colorStr = CoodinatesToColor(imageX, imageY);
            //        Bitmap src = new Bitmap(ListImagesFullName[imageIndex]);
            //        Watch_Face.StepsProgress.WeekDayProgressBar.CenterX = imageX + src.Width / 2;
            //        Watch_Face.StepsProgress.WeekDayProgressBar.CenterY = imageY + src.Height / 2;
            //        Watch_Face.StepsProgress.WeekDayProgressBar.Color = colorStr;
            //        Watch_Face.StepsProgress.WeekDayProgressBar.ImageIndex = imageIndex;
            //    }
            //}

            //// прогресc шагов стрелкой
            //if ((checkBox_StProg_ClockHand.Checked) && (comboBox_StProg_ClockHand_Image.SelectedIndex >= 0))
            //{
            //    if (Watch_Face.StepsProgress == null) Watch_Face.StepsProgress = new StepsProgress();
            //    if (Watch_Face.StepsProgress.WeekDayClockHand == null) Watch_Face.StepsProgress.WeekDayClockHand = new WeekDayClockHand();
            //    if (Watch_Face.StepsProgress.WeekDayClockHand.CenterOffset == null)
            //        Watch_Face.StepsProgress.WeekDayClockHand.CenterOffset = new Coordinates();
            //    if (Watch_Face.StepsProgress.WeekDayClockHand.Sector == null)
            //        Watch_Face.StepsProgress.WeekDayClockHand.Sector = new Sector();
            //    //if (Watch_Face.MonthClockHand.Hours.Shape == null)
            //    //Watch_Face.MonthClockHand.Hours.Shape = new Coordinates();
            //    if (Watch_Face.StepsProgress.WeekDayClockHand.Image == null)
            //        Watch_Face.StepsProgress.WeekDayClockHand.Image = new ImageW();

            //    Watch_Face.StepsProgress.WeekDayClockHand.Image.ImageIndex = Int32.Parse(comboBox_StProg_ClockHand_Image.Text);
            //    Watch_Face.StepsProgress.WeekDayClockHand.Image.X = (int)numericUpDown_StProg_ClockHand_X.Value;
            //    Watch_Face.StepsProgress.WeekDayClockHand.Image.Y = (int)numericUpDown_StProg_ClockHand_Y.Value;

            //    Watch_Face.StepsProgress.WeekDayClockHand.Color = "0x00000000";
            //    Watch_Face.StepsProgress.WeekDayClockHand.OnlyBorder = false;

            //    Watch_Face.StepsProgress.WeekDayClockHand.CenterOffset.X = (int)numericUpDown_StProg_ClockHand_Offset_X.Value;
            //    Watch_Face.StepsProgress.WeekDayClockHand.CenterOffset.Y = (int)numericUpDown_StProg_ClockHand_Offset_Y.Value;

            //    Watch_Face.StepsProgress.WeekDayClockHand.Sector.StartAngle =
            //        (int)(numericUpDown_StProg_ClockHand_StartAngle.Value * 100);
            //    Watch_Face.StepsProgress.WeekDayClockHand.Sector.EndAngle =
            //        (int)(numericUpDown_StProg_ClockHand_EndAngle.Value * 100);
            //}

            //// прогресc шагов набором иконок
            //if (checkBox_SPSliced.Checked)
            //{
            //    if ((comboBox_SPSliced_Image.SelectedIndex >= 0) && (dataGridView_SPSliced.Rows.Count > 1))
            //    {
            //        if (Watch_Face.StepsProgress == null) Watch_Face.StepsProgress = new StepsProgress();
            //        if (Watch_Face.StepsProgress.Sliced == null) Watch_Face.StepsProgress.Sliced = new IconSet();

            //        Watch_Face.StepsProgress.Sliced.ImageIndex = Int32.Parse(comboBox_SPSliced_Image.Text);
            //        Coordinates[] coordinates = new Coordinates[0];
            //        int count = 0;

            //        foreach (DataGridViewRow row in dataGridView_SPSliced.Rows)
            //        {
            //            //whatever you are currently doing
            //            //Coordinates coordinates = new Coordinates();
            //            int x = 0;
            //            int y = 0;
            //            if ((row.Cells[0].Value != null) && (row.Cells[1].Value != null))
            //            {
            //                if ((Int32.TryParse(row.Cells[0].Value.ToString(), out x)) && (Int32.TryParse(row.Cells[1].Value.ToString(), out y)))
            //                {

            //                    //Array.Resize(ref objson, objson.Length + 1);
            //                    Array.Resize(ref coordinates, coordinates.Length + 1);
            //                    //objson[count] = coordinates;
            //                    coordinates[count] = new Coordinates();
            //                    coordinates[count].X = x;
            //                    coordinates[count].Y = y;
            //                    count++;
            //                }
            //            }
            //            Watch_Face.StepsProgress.Sliced.Coordinates = coordinates;
            //        }
            //    }
            //}

            //// статусы
            //if ((checkBox_Bluetooth.Checked) &&
            //    ((comboBox_Bluetooth_On.SelectedIndex >= 0) || (comboBox_Bluetooth_Off.SelectedIndex >= 0)))
            //{
            //    if (Watch_Face.Status == null) Watch_Face.Status = new Status();
            //    if (Watch_Face.Status.Bluetooth == null) Watch_Face.Status.Bluetooth = new SwitchW();
            //    if (Watch_Face.Status.Bluetooth.Coordinates == null)
            //        Watch_Face.Status.Bluetooth.Coordinates = new Coordinates();

            //    if (comboBox_Bluetooth_On.SelectedIndex >= 0)
            //        Watch_Face.Status.Bluetooth.ImageIndexOn = Int32.Parse(comboBox_Bluetooth_On.Text);
            //    if (comboBox_Bluetooth_Off.SelectedIndex >= 0)
            //        Watch_Face.Status.Bluetooth.ImageIndexOff = Int32.Parse(comboBox_Bluetooth_Off.Text);
            //    Watch_Face.Status.Bluetooth.Coordinates.X = (int)numericUpDown_Bluetooth_X.Value;
            //    Watch_Face.Status.Bluetooth.Coordinates.Y = (int)numericUpDown_Bluetooth_Y.Value;
            //}
            //if ((checkBox_Alarm.Checked) &&
            //    ((comboBox_Alarm_On.SelectedIndex >= 0) || (comboBox_Alarm_Off.SelectedIndex >= 0)))
            //{
            //    if (Watch_Face.Status == null) Watch_Face.Status = new Status();
            //    if (Watch_Face.Status.Alarm == null) Watch_Face.Status.Alarm = new SwitchW();
            //    if (Watch_Face.Status.Alarm.Coordinates == null)
            //        Watch_Face.Status.Alarm.Coordinates = new Coordinates();
            //    Watch_Face.Status.Alarm.Coordinates = new Coordinates();

            //    if (comboBox_Alarm_On.SelectedIndex >= 0)
            //        Watch_Face.Status.Alarm.ImageIndexOn = Int32.Parse(comboBox_Alarm_On.Text);
            //    if (comboBox_Alarm_Off.SelectedIndex >= 0)
            //        Watch_Face.Status.Alarm.ImageIndexOff = Int32.Parse(comboBox_Alarm_Off.Text);
            //    Watch_Face.Status.Alarm.Coordinates.X = (int)numericUpDown_Alarm_X.Value;
            //    Watch_Face.Status.Alarm.Coordinates.Y = (int)numericUpDown_Alarm_Y.Value;
            //}
            //if ((checkBox_Lock.Checked) &&
            //    ((comboBox_Lock_On.SelectedIndex >= 0) || (comboBox_Lock_Off.SelectedIndex >= 0)))
            //{
            //    if (Watch_Face.Status == null) Watch_Face.Status = new Status();
            //    if (Watch_Face.Status.Lock == null) Watch_Face.Status.Lock = new SwitchW();
            //    if (Watch_Face.Status.Lock.Coordinates == null)
            //        Watch_Face.Status.Lock.Coordinates = new Coordinates();

            //    if (comboBox_Lock_On.SelectedIndex >= 0)
            //        Watch_Face.Status.Lock.ImageIndexOn = Int32.Parse(comboBox_Lock_On.Text);
            //    if (comboBox_Lock_Off.SelectedIndex >= 0)
            //        Watch_Face.Status.Lock.ImageIndexOff = Int32.Parse(comboBox_Lock_Off.Text);
            //    Watch_Face.Status.Lock.Coordinates.X = (int)numericUpDown_Lock_X.Value;
            //    Watch_Face.Status.Lock.Coordinates.Y = (int)numericUpDown_Lock_Y.Value;
            //}
            //if ((checkBox_DND.Checked) &&
            //    ((comboBox_DND_On.SelectedIndex >= 0) || (comboBox_DND_Off.SelectedIndex >= 0)))
            //{
            //    if (Watch_Face.Status == null) Watch_Face.Status = new Status();
            //    if (Watch_Face.Status.DoNotDisturb == null) Watch_Face.Status.DoNotDisturb = new SwitchW();
            //    if (Watch_Face.Status.DoNotDisturb.Coordinates == null)
            //        Watch_Face.Status.DoNotDisturb.Coordinates = new Coordinates();

            //    if (comboBox_DND_On.SelectedIndex >= 0)
            //        Watch_Face.Status.DoNotDisturb.ImageIndexOn = Int32.Parse(comboBox_DND_On.Text);
            //    if (comboBox_DND_Off.SelectedIndex >= 0)
            //        Watch_Face.Status.DoNotDisturb.ImageIndexOff = Int32.Parse(comboBox_DND_Off.Text);
            //    Watch_Face.Status.DoNotDisturb.Coordinates.X = (int)numericUpDown_DND_X.Value;
            //    Watch_Face.Status.DoNotDisturb.Coordinates.Y = (int)numericUpDown_DND_Y.Value;
            //}

            //// батарея
            //if (checkBox_Battery.Checked)
            //{
            //    if ((checkBox_Battery_Text.Checked) && (comboBox_Battery_Text_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.Battery == null) Watch_Face.Battery = new Battery();
            //        if (Watch_Face.Battery.Text == null) Watch_Face.Battery.Text = new Number();

            //        Watch_Face.Battery.Text.ImageIndex = Int32.Parse(comboBox_Battery_Text_Image.Text);
            //        Watch_Face.Battery.Text.ImagesCount = (int)numericUpDown_Battery_Text_Count.Value;
            //        Watch_Face.Battery.Text.TopLeftX = (int)numericUpDown_Battery_Text_StartCorner_X.Value;
            //        Watch_Face.Battery.Text.TopLeftY = (int)numericUpDown_Battery_Text_StartCorner_Y.Value;
            //        Watch_Face.Battery.Text.BottomRightX = (int)numericUpDown_Battery_Text_EndCorner_X.Value;
            //        Watch_Face.Battery.Text.BottomRightY = (int)numericUpDown_Battery_Text_EndCorner_Y.Value;

            //        Watch_Face.Battery.Text.Spacing = (int)numericUpDown_Battery_Text_Spacing.Value;
            //        string Alignment = StringToAlignment(comboBox_Battery_Text_Alignment.SelectedIndex);
            //        Watch_Face.Battery.Text.Alignment = Alignment;
            //    }

            //    if ((checkBox_Battery_Img.Checked) && (comboBox_Battery_Img_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.Battery == null) Watch_Face.Battery = new Battery();
            //        if (Watch_Face.Battery.Images == null) Watch_Face.Battery.Images = new ImageSet();

            //        Watch_Face.Battery.Images.ImageIndex = Int32.Parse(comboBox_Battery_Img_Image.Text);
            //        Watch_Face.Battery.Images.ImagesCount = (int)numericUpDown_Battery_Img_Count.Value;
            //        Watch_Face.Battery.Images.X = (int)numericUpDown_Battery_Img_X.Value;
            //        Watch_Face.Battery.Images.Y = (int)numericUpDown_Battery_Img_Y.Value;
            //    }

            //    if ((checkBox_Battery_ClockHand.Checked) && (comboBox_Battery_ClockHand_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.Battery == null) Watch_Face.Battery = new Battery();
            //        if (Watch_Face.Battery.Unknown4 == null) Watch_Face.Battery.Unknown4 = new WeekDayClockHand();
            //        if (Watch_Face.Battery.Unknown4.CenterOffset == null)
            //            Watch_Face.Battery.Unknown4.CenterOffset = new Coordinates();
            //        if (Watch_Face.Battery.Unknown4.Sector == null)
            //            Watch_Face.Battery.Unknown4.Sector = new Sector();
            //        //if (Watch_Face.MonthClockHand.Hours.Shape == null)
            //        //Watch_Face.MonthClockHand.Hours.Shape = new Coordinates();
            //        if (Watch_Face.Battery.Unknown4.Image == null)
            //            Watch_Face.Battery.Unknown4.Image = new ImageW();

            //        Watch_Face.Battery.Unknown4.Image.ImageIndex = Int32.Parse(comboBox_Battery_ClockHand_Image.Text);
            //        Watch_Face.Battery.Unknown4.Image.X = (int)numericUpDown_Battery_ClockHand_X.Value;
            //        Watch_Face.Battery.Unknown4.Image.Y = (int)numericUpDown_Battery_ClockHand_Y.Value;

            //        Watch_Face.Battery.Unknown4.Color = "0x00000000";
            //        Watch_Face.Battery.Unknown4.OnlyBorder = false;

            //        Watch_Face.Battery.Unknown4.CenterOffset.X = (int)numericUpDown_Battery_ClockHand_Offset_X.Value;
            //        Watch_Face.Battery.Unknown4.CenterOffset.Y = (int)numericUpDown_Battery_ClockHand_Offset_Y.Value;

            //        Watch_Face.Battery.Unknown4.Sector.StartAngle =
            //            (int)(numericUpDown_Battery_ClockHand_StartAngle.Value * 100);
            //        Watch_Face.Battery.Unknown4.Sector.EndAngle =
            //            (int)(numericUpDown_Battery_ClockHand_EndAngle.Value * 100);
            //    }

            //    if ((checkBox_Battery_Percent.Checked) && (comboBox_Battery_Percent_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.Battery == null) Watch_Face.Battery = new Battery();
            //        if (Watch_Face.Battery.Percent == null) Watch_Face.Battery.Percent = new ImageW();

            //        Watch_Face.Battery.Percent.ImageIndex = Int32.Parse(comboBox_Battery_Percent_Image.Text);
            //        Watch_Face.Battery.Percent.X = (int)numericUpDown_Battery_Percent_X.Value;
            //        Watch_Face.Battery.Percent.Y = (int)numericUpDown_Battery_Percent_Y.Value;
            //    }

            //    if (checkBox_Battery_Scale.Checked)
            //    {
            //        if (Watch_Face.Battery == null) Watch_Face.Battery = new Battery();
            //        if (Watch_Face.Battery.Scale == null) Watch_Face.Battery.Scale = new CircleScale();

            //        Watch_Face.Battery.Scale.CenterX = (int)numericUpDown_Battery_Scale_Center_X.Value;
            //        Watch_Face.Battery.Scale.CenterY = (int)numericUpDown_Battery_Scale_Center_Y.Value;
            //        Watch_Face.Battery.Scale.RadiusX = (int)numericUpDown_Battery_Scale_Radius_X.Value;
            //        Watch_Face.Battery.Scale.RadiusY = (int)numericUpDown_Battery_Scale_Radius_Y.Value;

            //        Watch_Face.Battery.Scale.StartAngle = (int)numericUpDown_Battery_Scale_StartAngle.Value;
            //        Watch_Face.Battery.Scale.EndAngle = (int)numericUpDown_Battery_Scale_EndAngle.Value;
            //        Watch_Face.Battery.Scale.Width = (int)numericUpDown_Battery_Scale_Width.Value;

            //        Color color = comboBox_Battery_Scale_Color.BackColor;
            //        Color new_color = Color.FromArgb(0, color.R, color.G, color.B);
            //        string colorStr = ColorTranslator.ToHtml(new_color);
            //        colorStr = colorStr.Replace("#", "0x00");
            //        Watch_Face.Battery.Scale.Color = colorStr;

            //        //switch (comboBox_Battery_Flatness.Text)
            //        //{
            //        //    case "Треугольное":
            //        //        Watch_Face.Battery.Scale.Flatness = 90;
            //        //        break;
            //        //    case "Плоское":
            //        //        Watch_Face.Battery.Scale.Flatness = 180;
            //        //        break;
            //        //    default:
            //        //        Watch_Face.Battery.Scale.Flatness = 0;
            //        //        break;
            //        //}
            //        switch (comboBox_Battery_Flatness.SelectedIndex)
            //        {
            //            case 1:
            //                Watch_Face.Battery.Scale.Flatness = 90;
            //                break;
            //            case 2:
            //                Watch_Face.Battery.Scale.Flatness = 180;
            //                break;
            //            default:
            //                Watch_Face.Battery.Scale.Flatness = 0;
            //                break;
            //        }

            //        if (checkBox_Battery_Scale_Image.Checked &&
            //            comboBox_Battery_Scale_Image.SelectedIndex >= 0)
            //        {
            //            int imageX = (int)numericUpDown_Battery_Scale_ImageX.Value;
            //            int imageY = (int)numericUpDown_Battery_Scale_ImageY.Value;
            //            int imageIndex = comboBox_Battery_Scale_Image.SelectedIndex;
            //            colorStr = CoodinatesToColor(imageX, imageY);
            //            Bitmap src = new Bitmap(ListImagesFullName[imageIndex]);
            //            Watch_Face.Battery.Scale.CenterX = imageX + src.Width / 2;
            //            Watch_Face.Battery.Scale.CenterY = imageY + src.Height / 2;
            //            Watch_Face.Battery.Scale.Color = colorStr;
            //            Watch_Face.Battery.Scale.ImageIndex = imageIndex;
            //        }
            //    }

            //    // батарея набором иконок
            //    if (checkBox_Battery_IconSet.Checked)
            //    {
            //        if ((comboBox_Battery_IconSet_Image.SelectedIndex >= 0) && (dataGridView_Battery_IconSet.Rows.Count > 1))
            //        {
            //            if (Watch_Face.Battery == null) Watch_Face.Battery = new Battery();
            //            if (Watch_Face.Battery.Icons == null) Watch_Face.Battery.Icons = new IconSet();

            //            Watch_Face.Battery.Icons.ImageIndex = Int32.Parse(comboBox_Battery_IconSet_Image.Text);
            //            Coordinates[] coordinates = new Coordinates[0];
            //            int count = 0;

            //            foreach (DataGridViewRow row in dataGridView_Battery_IconSet.Rows)
            //            {
            //                //whatever you are currently doing
            //                //Coordinates coordinates = new Coordinates();
            //                int x = 0;
            //                int y = 0;
            //                if ((row.Cells[0].Value != null) && (row.Cells[1].Value != null))
            //                {
            //                    if ((Int32.TryParse(row.Cells[0].Value.ToString(), out x)) && (Int32.TryParse(row.Cells[1].Value.ToString(), out y)))
            //                    {

            //                        //Array.Resize(ref objson, objson.Length + 1);
            //                        Array.Resize(ref coordinates, coordinates.Length + 1);
            //                        //objson[count] = coordinates;
            //                        coordinates[count] = new Coordinates();
            //                        coordinates[count].X = x;
            //                        coordinates[count].Y = y;
            //                        count++;
            //                    }
            //                }
            //                Watch_Face.Battery.Icons.Coordinates = coordinates;
            //            }
            //        }
            //    }
            //}

            //// стрелки
            //if (checkBox_AnalogClock.Checked)
            //{
            //    if ((checkBox_AnalogClock_Hour.Checked) && (comboBox_AnalogClock_Hour_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.MonthClockHand == null) Watch_Face.MonthClockHand = new Analogdialface();
            //        if (Watch_Face.MonthClockHand.Hours == null) Watch_Face.MonthClockHand.Hours = new WeekDayClockHand();
            //        if (Watch_Face.MonthClockHand.Hours.CenterOffset == null)
            //            Watch_Face.MonthClockHand.Hours.CenterOffset = new Coordinates();
            //        //if (Watch_Face.MonthClockHand.Hours.Shape == null)
            //        //Watch_Face.MonthClockHand.Hours.Shape = new Coordinates();
            //        if (Watch_Face.MonthClockHand.Hours.Image == null)
            //            Watch_Face.MonthClockHand.Hours.Image = new ImageW();

            //        Watch_Face.MonthClockHand.Hours.Image.ImageIndex = Int32.Parse(comboBox_AnalogClock_Hour_Image.Text);
            //        Watch_Face.MonthClockHand.Hours.Image.X = (int)numericUpDown_AnalogClock_Hour_X.Value;
            //        Watch_Face.MonthClockHand.Hours.Image.Y = (int)numericUpDown_AnalogClock_Hour_Y.Value;

            //        Watch_Face.MonthClockHand.Hours.Color = "0x00000000";
            //        Watch_Face.MonthClockHand.Hours.OnlyBorder = false;

            //        Watch_Face.MonthClockHand.Hours.CenterOffset.X = (int)numericUpDown_AnalogClock_Hour_Offset_X.Value;
            //        Watch_Face.MonthClockHand.Hours.CenterOffset.Y = (int)numericUpDown_AnalogClock_Hour_Offset_Y.Value;
            //    }

            //    if ((checkBox_AnalogClock_Min.Checked) && (comboBox_AnalogClock_Min_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.MonthClockHand == null) Watch_Face.MonthClockHand = new Analogdialface();
            //        if (Watch_Face.MonthClockHand.Minutes == null) Watch_Face.MonthClockHand.Minutes = new WeekDayClockHand();
            //        if (Watch_Face.MonthClockHand.Minutes.CenterOffset == null)
            //            Watch_Face.MonthClockHand.Minutes.CenterOffset = new Coordinates();
            //        //if (Watch_Face.MonthClockHand.Minutes.Shape == null)
            //        //    Watch_Face.MonthClockHand.Minutes.Shape = new Coordinates();
            //        if (Watch_Face.MonthClockHand.Minutes.Image == null)
            //            Watch_Face.MonthClockHand.Minutes.Image = new ImageW();

            //        Watch_Face.MonthClockHand.Minutes.Image.ImageIndex = Int32.Parse(comboBox_AnalogClock_Min_Image.Text);
            //        Watch_Face.MonthClockHand.Minutes.Image.X = (int)numericUpDown_AnalogClock_Min_X.Value;
            //        Watch_Face.MonthClockHand.Minutes.Image.Y = (int)numericUpDown_AnalogClock_Min_Y.Value;

            //        Watch_Face.MonthClockHand.Minutes.Color = "0x00000000";
            //        Watch_Face.MonthClockHand.Minutes.OnlyBorder = false;

            //        Watch_Face.MonthClockHand.Minutes.CenterOffset.X = (int)numericUpDown_AnalogClock_Min_Offset_X.Value;
            //        Watch_Face.MonthClockHand.Minutes.CenterOffset.Y = (int)numericUpDown_AnalogClock_Min_Offset_Y.Value;
            //    }

            //    if ((checkBox_AnalogClock_Sec.Checked) && (comboBox_AnalogClock_Sec_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.MonthClockHand == null) Watch_Face.MonthClockHand = new Analogdialface();
            //        if (Watch_Face.MonthClockHand.Seconds == null) Watch_Face.MonthClockHand.Seconds = new WeekDayClockHand();
            //        if (Watch_Face.MonthClockHand.Seconds.CenterOffset == null)
            //            Watch_Face.MonthClockHand.Seconds.CenterOffset = new Coordinates();
            //        //if (Watch_Face.MonthClockHand.Seconds.Shape == null)
            //        //    Watch_Face.MonthClockHand.Seconds.Shape = new Coordinates();
            //        if (Watch_Face.MonthClockHand.Seconds.Image == null)
            //            Watch_Face.MonthClockHand.Seconds.Image = new ImageW();

            //        Watch_Face.MonthClockHand.Seconds.Image.ImageIndex = Int32.Parse(comboBox_AnalogClock_Sec_Image.Text);
            //        Watch_Face.MonthClockHand.Seconds.Image.X = (int)numericUpDown_AnalogClock_Sec_X.Value;
            //        Watch_Face.MonthClockHand.Seconds.Image.Y = (int)numericUpDown_AnalogClock_Sec_Y.Value;

            //        Watch_Face.MonthClockHand.Seconds.Color = "0x00000000";
            //        Watch_Face.MonthClockHand.Seconds.OnlyBorder = false;

            //        Watch_Face.MonthClockHand.Seconds.CenterOffset.X = (int)numericUpDown_AnalogClock_Sec_Offset_X.Value;
            //        Watch_Face.MonthClockHand.Seconds.CenterOffset.Y = (int)numericUpDown_AnalogClock_Sec_Offset_Y.Value;
            //    }

            //    if ((checkBox_HourCenterImage.Checked) && (comboBox_HourCenterImage_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.MonthClockHand == null) Watch_Face.MonthClockHand = new Analogdialface();
            //        if (Watch_Face.MonthClockHand.HourCenterImage == null)
            //            Watch_Face.MonthClockHand.HourCenterImage = new ImageW();

            //        Watch_Face.MonthClockHand.HourCenterImage.ImageIndex = Int32.Parse(comboBox_HourCenterImage_Image.Text);
            //        Watch_Face.MonthClockHand.HourCenterImage.X = (int)numericUpDown_HourCenterImage_X.Value;
            //        Watch_Face.MonthClockHand.HourCenterImage.Y = (int)numericUpDown_HourCenterImage_Y.Value;
            //    }

            //    if ((checkBox_MinCenterImage.Checked) && (comboBox_MinCenterImage_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.MonthClockHand == null) Watch_Face.MonthClockHand = new Analogdialface();
            //        if (Watch_Face.MonthClockHand.MinCenterImage == null)
            //            Watch_Face.MonthClockHand.MinCenterImage = new ImageW();

            //        Watch_Face.MonthClockHand.MinCenterImage.ImageIndex = Int32.Parse(comboBox_MinCenterImage_Image.Text);
            //        Watch_Face.MonthClockHand.MinCenterImage.X = (int)numericUpDown_MinCenterImage_X.Value;
            //        Watch_Face.MonthClockHand.MinCenterImage.Y = (int)numericUpDown_MinCenterImage_Y.Value;
            //    }

            //    if ((checkBox_SecCenterImage.Checked) && (comboBox_SecCenterImage_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.MonthClockHand == null) Watch_Face.MonthClockHand = new Analogdialface();
            //        if (Watch_Face.MonthClockHand.SecCenterImage == null)
            //            Watch_Face.MonthClockHand.SecCenterImage = new ImageW();

            //        Watch_Face.MonthClockHand.SecCenterImage.ImageIndex = Int32.Parse(comboBox_SecCenterImage_Image.Text);
            //        Watch_Face.MonthClockHand.SecCenterImage.X = (int)numericUpDown_SecCenterImage_X.Value;
            //        Watch_Face.MonthClockHand.SecCenterImage.Y = (int)numericUpDown_SecCenterImage_Y.Value;
            //    }
            //}

            //// погода 
            //if (checkBox_Weather.Checked)
            //{
            //    if ((checkBox_Weather_Text.Checked) && (comboBox_Weather_Text_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.Weather == null) Watch_Face.Weather = new Weather();
            //        if (Watch_Face.Weather.Temperature == null) Watch_Face.Weather.Temperature = new Temperature();
            //        if (Watch_Face.Weather.Temperature.Current == null)
            //            Watch_Face.Weather.Temperature.Current = new Number();

            //        Watch_Face.Weather.Temperature.Current.ImageIndex = Int32.Parse(comboBox_Weather_Text_Image.Text);
            //        Watch_Face.Weather.Temperature.Current.ImagesCount = (int)numericUpDown_Weather_Text_Count.Value;
            //        Watch_Face.Weather.Temperature.Current.TopLeftX = (int)numericUpDown_Weather_Text_StartCorner_X.Value;
            //        Watch_Face.Weather.Temperature.Current.TopLeftY = (int)numericUpDown_Weather_Text_StartCorner_Y.Value;
            //        Watch_Face.Weather.Temperature.Current.BottomRightX = (int)numericUpDown_Weather_Text_EndCorner_X.Value;
            //        Watch_Face.Weather.Temperature.Current.BottomRightY = (int)numericUpDown_Weather_Text_EndCorner_Y.Value;

            //        Watch_Face.Weather.Temperature.Current.Spacing = (int)numericUpDown_Weather_Text_Spacing.Value;
            //        string Alignment = StringToAlignment(comboBox_Weather_Text_Alignment.SelectedIndex);
            //        Watch_Face.Weather.Temperature.Current.Alignment = Alignment;

            //        if ((comboBox_Weather_Text_MinusImage.SelectedIndex >= 0) ||
            //            (comboBox_Weather_Text_DegImage.SelectedIndex >= 0) ||
            //            (comboBox_Weather_Text_NDImage.SelectedIndex >= 0))
            //        {
            //            if (Watch_Face.Weather.Temperature.Symbols == null)
            //                Watch_Face.Weather.Temperature.Symbols = new Symbols();
            //            Watch_Face.Weather.Temperature.Symbols.Unknown0800 = 0;
            //            if (comboBox_Weather_Text_MinusImage.SelectedIndex >= 0)
            //                Watch_Face.Weather.Temperature.Symbols.MinusImageIndex =
            //                        Int32.Parse(comboBox_Weather_Text_MinusImage.Text);
            //            if (comboBox_Weather_Text_DegImage.SelectedIndex >= 0)
            //                Watch_Face.Weather.Temperature.Symbols.DegreesImageIndex =
            //                        Int32.Parse(comboBox_Weather_Text_DegImage.Text);
            //            if (comboBox_Weather_Text_NDImage.SelectedIndex >= 0)
            //                Watch_Face.Weather.Temperature.Symbols.NoDataImageIndex =
            //                        Int32.Parse(comboBox_Weather_Text_NDImage.Text);
            //        }
            //    }

            //    if ((checkBox_Weather_Day.Checked) && (comboBox_Weather_Day_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.Weather == null) Watch_Face.Weather = new Weather();
            //        if (Watch_Face.Weather.Temperature == null) Watch_Face.Weather.Temperature = new Temperature();
            //        if (Watch_Face.Weather.Temperature.Today == null)
            //            Watch_Face.Weather.Temperature.Today = new Today();
            //        if (Watch_Face.Weather.Temperature.Today.Separate == null)
            //            Watch_Face.Weather.Temperature.Today.Separate = new Separate();
            //        if (Watch_Face.Weather.Temperature.Today.Separate.Day == null)
            //            Watch_Face.Weather.Temperature.Today.Separate.Day = new Number();

            //        Watch_Face.Weather.Temperature.Today.Separate.Day.ImageIndex =
            //            Int32.Parse(comboBox_Weather_Day_Image.Text);
            //        Watch_Face.Weather.Temperature.Today.Separate.Day.ImagesCount =
            //            (int)numericUpDown_Weather_Day_Count.Value;
            //        Watch_Face.Weather.Temperature.Today.Separate.Day.TopLeftX =
            //            (int)numericUpDown_Weather_Day_StartCorner_X.Value;
            //        Watch_Face.Weather.Temperature.Today.Separate.Day.TopLeftY =
            //            (int)numericUpDown_Weather_Day_StartCorner_Y.Value;
            //        Watch_Face.Weather.Temperature.Today.Separate.Day.BottomRightX =
            //            (int)numericUpDown_Weather_Day_EndCorner_X.Value;
            //        Watch_Face.Weather.Temperature.Today.Separate.Day.BottomRightY =
            //            (int)numericUpDown_Weather_Day_EndCorner_Y.Value;

            //        Watch_Face.Weather.Temperature.Today.Separate.Day.Spacing =
            //            (int)numericUpDown_Weather_Day_Spacing.Value;
            //        string Alignment = StringToAlignment(comboBox_Weather_Day_Alignment.SelectedIndex);
            //        Watch_Face.Weather.Temperature.Today.Separate.Day.Alignment = Alignment;
            //        Watch_Face.Weather.Temperature.Today.AppendDegreesForBoth = true;

            //        if ((comboBox_Weather_Text_MinusImage.SelectedIndex >= 0) ||
            //            (comboBox_Weather_Text_DegImage.SelectedIndex >= 0) ||
            //            (comboBox_Weather_Text_NDImage.SelectedIndex >= 0))
            //        {
            //            if (Watch_Face.Weather.Temperature.Symbols == null)
            //                Watch_Face.Weather.Temperature.Symbols = new Symbols();
            //            Watch_Face.Weather.Temperature.Symbols.Unknown0800 = 0;
            //            if (comboBox_Weather_Text_MinusImage.SelectedIndex >= 0)
            //                Watch_Face.Weather.Temperature.Symbols.MinusImageIndex =
            //                        Int32.Parse(comboBox_Weather_Text_MinusImage.Text);
            //            if (comboBox_Weather_Text_DegImage.SelectedIndex >= 0)
            //                Watch_Face.Weather.Temperature.Symbols.DegreesImageIndex =
            //                        Int32.Parse(comboBox_Weather_Text_DegImage.Text);
            //            if (comboBox_Weather_Text_NDImage.SelectedIndex >= 0)
            //                Watch_Face.Weather.Temperature.Symbols.NoDataImageIndex =
            //                        Int32.Parse(comboBox_Weather_Text_NDImage.Text);
            //        }
            //    }

            //    if ((checkBox_Weather_Night.Checked) && (comboBox_Weather_Night_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.Weather == null) Watch_Face.Weather = new Weather();
            //        if (Watch_Face.Weather.Temperature == null) Watch_Face.Weather.Temperature = new Temperature();
            //        if (Watch_Face.Weather.Temperature.Today == null)
            //            Watch_Face.Weather.Temperature.Today = new Today();
            //        if (Watch_Face.Weather.Temperature.Today.Separate == null)
            //            Watch_Face.Weather.Temperature.Today.Separate = new Separate();
            //        if (Watch_Face.Weather.Temperature.Today.Separate.Night == null)
            //            Watch_Face.Weather.Temperature.Today.Separate.Night = new Number();

            //        Watch_Face.Weather.Temperature.Today.Separate.Night.ImageIndex =
            //            Int32.Parse(comboBox_Weather_Night_Image.Text);
            //        Watch_Face.Weather.Temperature.Today.Separate.Night.ImagesCount =
            //            (int)numericUpDown_Weather_Night_Count.Value;
            //        Watch_Face.Weather.Temperature.Today.Separate.Night.TopLeftX =
            //            (int)numericUpDown_Weather_Night_StartCorner_X.Value;
            //        Watch_Face.Weather.Temperature.Today.Separate.Night.TopLeftY =
            //            (int)numericUpDown_Weather_Night_StartCorner_Y.Value;
            //        Watch_Face.Weather.Temperature.Today.Separate.Night.BottomRightX =
            //            (int)numericUpDown_Weather_Night_EndCorner_X.Value;
            //        Watch_Face.Weather.Temperature.Today.Separate.Night.BottomRightY =
            //            (int)numericUpDown_Weather_Night_EndCorner_Y.Value;

            //        Watch_Face.Weather.Temperature.Today.Separate.Night.Spacing =
            //            (int)numericUpDown_Weather_Night_Spacing.Value;
            //        string Alignment = StringToAlignment(comboBox_Weather_Night_Alignment.SelectedIndex);
            //        Watch_Face.Weather.Temperature.Today.Separate.Night.Alignment = Alignment;
            //        Watch_Face.Weather.Temperature.Today.AppendDegreesForBoth = true;

            //        if ((comboBox_Weather_Text_MinusImage.SelectedIndex >= 0) ||
            //            (comboBox_Weather_Text_DegImage.SelectedIndex >= 0) ||
            //            (comboBox_Weather_Text_NDImage.SelectedIndex >= 0))
            //        {
            //            if (Watch_Face.Weather.Temperature.Symbols == null)
            //                Watch_Face.Weather.Temperature.Symbols = new Symbols();
            //            Watch_Face.Weather.Temperature.Symbols.Unknown0800 = 0;
            //            if (comboBox_Weather_Text_MinusImage.SelectedIndex >= 0)
            //                Watch_Face.Weather.Temperature.Symbols.MinusImageIndex =
            //                        Int32.Parse(comboBox_Weather_Text_MinusImage.Text);
            //            if (comboBox_Weather_Text_DegImage.SelectedIndex >= 0)
            //                Watch_Face.Weather.Temperature.Symbols.DegreesImageIndex =
            //                        Int32.Parse(comboBox_Weather_Text_DegImage.Text);
            //            if (comboBox_Weather_Text_NDImage.SelectedIndex >= 0)
            //                Watch_Face.Weather.Temperature.Symbols.NoDataImageIndex =
            //                        Int32.Parse(comboBox_Weather_Text_NDImage.Text);
            //        }
            //    }

            //    if ((checkBox_Weather_Icon.Checked) && (comboBox_Weather_Icon_Image.SelectedIndex >= 0)
            //        && (comboBox_Weather_Icon_NDImage.SelectedIndex >= 0))
            //    {
            //        // numericUpDown_Weather_Icon_X
            //        if (Watch_Face.Weather == null) Watch_Face.Weather = new Weather();
            //        if (Watch_Face.Weather.Icon == null) Watch_Face.Weather.Icon = new IconW();
            //        if (Watch_Face.Weather.Icon.Images == null) Watch_Face.Weather.Icon.Images = new ImageSet();

            //        Watch_Face.Weather.Icon.Images.X = (int)numericUpDown_Weather_Icon_X.Value;
            //        Watch_Face.Weather.Icon.Images.Y = (int)numericUpDown_Weather_Icon_Y.Value;
            //        Watch_Face.Weather.Icon.Images.ImagesCount = (int)numericUpDown_Weather_Icon_Count.Value;
            //        Watch_Face.Weather.Icon.Images.ImageIndex = Int32.Parse(comboBox_Weather_Icon_Image.Text);
            //        Watch_Face.Weather.Icon.NoWeatherImageIndex = Int32.Parse(comboBox_Weather_Icon_NDImage.Text);
            //    }
            //}

            //// ярлыки
            //if (checkBox_Shortcuts.Checked)
            //{
            //    if (checkBox_Shortcuts_Steps.Checked)
            //    {
            //        if (Watch_Face.Shortcuts == null) Watch_Face.Shortcuts = new Shortcuts();
            //        if (Watch_Face.Shortcuts.State == null) Watch_Face.Shortcuts.State = new Shortcut();
            //        Watch_Face.Shortcuts.State.Element = new Element();
            //        Watch_Face.Shortcuts.State.Element.TopLeftX = (int)numericUpDown_Shortcuts_Steps_X.Value;
            //        Watch_Face.Shortcuts.State.Element.TopLeftY = (int)numericUpDown_Shortcuts_Steps_Y.Value;
            //        Watch_Face.Shortcuts.State.Element.Width = (int)numericUpDown_Shortcuts_Steps_Width.Value;
            //        Watch_Face.Shortcuts.State.Element.Height = (int)numericUpDown_Shortcuts_Steps_Height.Value;
            //    }

            //    if (checkBox_Shortcuts_Puls.Checked)
            //    {
            //        if (Watch_Face.Shortcuts == null) Watch_Face.Shortcuts = new Shortcuts();
            //        if (Watch_Face.Shortcuts.HeartRate == null) Watch_Face.Shortcuts.HeartRate = new Shortcut();
            //        Watch_Face.Shortcuts.HeartRate.Element = new Element();
            //        Watch_Face.Shortcuts.HeartRate.Element.TopLeftX = (int)numericUpDown_Shortcuts_Puls_X.Value;
            //        Watch_Face.Shortcuts.HeartRate.Element.TopLeftY = (int)numericUpDown_Shortcuts_Puls_Y.Value;
            //        Watch_Face.Shortcuts.HeartRate.Element.Width = (int)numericUpDown_Shortcuts_Puls_Width.Value;
            //        Watch_Face.Shortcuts.HeartRate.Element.Height = (int)numericUpDown_Shortcuts_Puls_Height.Value;
            //    }

            //    if (checkBox_Shortcuts_Weather.Checked)
            //    {
            //        if (Watch_Face.Shortcuts == null) Watch_Face.Shortcuts = new Shortcuts();
            //        if (Watch_Face.Shortcuts.Weather == null) Watch_Face.Shortcuts.Weather = new Shortcut();
            //        Watch_Face.Shortcuts.Weather.Element = new Element();
            //        Watch_Face.Shortcuts.Weather.Element.TopLeftX = (int)numericUpDown_Shortcuts_Weather_X.Value;
            //        Watch_Face.Shortcuts.Weather.Element.TopLeftY = (int)numericUpDown_Shortcuts_Weather_Y.Value;
            //        Watch_Face.Shortcuts.Weather.Element.Width = (int)numericUpDown_Shortcuts_Weather_Width.Value;
            //        Watch_Face.Shortcuts.Weather.Element.Height = (int)numericUpDown_Shortcuts_Weather_Height.Value;
            //    }

            //    if (checkBox_Shortcuts_Energy.Checked)
            //    {
            //        if (Watch_Face.Shortcuts == null) Watch_Face.Shortcuts = new Shortcuts();
            //        if (Watch_Face.Shortcuts.Unknown4 == null) Watch_Face.Shortcuts.Unknown4 = new Shortcut();
            //        Watch_Face.Shortcuts.Unknown4.Element = new Element();
            //        Watch_Face.Shortcuts.Unknown4.Element.TopLeftX = (int)numericUpDown_Shortcuts_Energy_X.Value;
            //        Watch_Face.Shortcuts.Unknown4.Element.TopLeftY = (int)numericUpDown_Shortcuts_Energy_Y.Value;
            //        Watch_Face.Shortcuts.Unknown4.Element.Width = (int)numericUpDown_Shortcuts_Energy_Width.Value;
            //        Watch_Face.Shortcuts.Unknown4.Element.Height = (int)numericUpDown_Shortcuts_Energy_Height.Value;
            //    }
            //}

            //// анимация
            //if (checkBox_Animation.Checked)
            //{
            //    // анимация (перемещение между координатами)
            //    if (checkBox_MotiomAnimation.Checked)
            //    {
            //        //if (Watch_Face.Unknown11 == null) Watch_Face.Unknown11 = new Animation();

            //        //MotiomAnimation[] motiomAnimation = new MotiomAnimation[0];
            //        List<MotiomAnimation> MotiomAnimation = new List<MotiomAnimation>();

            //        foreach (DataGridViewRow row in dataGridView_MotiomAnimation.Rows)
            //        {
            //            if (MotiomAnimation.Count >= 4) break;
            //            //Coordinates coordinates = new Coordinates();
            //            MotiomAnimation motiomAnimation = new MotiomAnimation();
            //            int Unknown1 = 0;
            //            int StartCoordinates_X = 0;
            //            int StartCoordinates_Y = 0;
            //            int EndCoordinates_X = 0;
            //            int EndCoordinates_Y = 0;
            //            int ImageIndex = 0;
            //            int SpeedAnimation = 0;
            //            int TimeAnimation = 0;
            //            int Unknown5 = 0;
            //            int Unknown6 = 1;
            //            int Unknown7 = 0;
            //            int Bounce = 0;
            //            bool Bounce_b = false;
            //            if (row.Cells[1].Value != null && row.Cells[2].Value != null && row.Cells[3].Value != null &&
            //                row.Cells[4].Value != null && row.Cells[5].Value != null && row.Cells[6].Value != null)
            //            {
            //                if (Int32.TryParse(row.Cells[1].Value.ToString(), out StartCoordinates_X) &&
            //                    Int32.TryParse(row.Cells[2].Value.ToString(), out StartCoordinates_Y) &&
            //                    Int32.TryParse(row.Cells[3].Value.ToString(), out EndCoordinates_X) &&
            //                    Int32.TryParse(row.Cells[4].Value.ToString(), out EndCoordinates_Y) &&
            //                    Int32.TryParse(row.Cells[5].Value.ToString(), out ImageIndex) &&
            //                    Int32.TryParse(row.Cells[6].Value.ToString(), out SpeedAnimation))
            //                {
            //                    if(row.Cells[7].Value != null) Int32.TryParse(row.Cells[7].Value.ToString(), out TimeAnimation);
            //                    //if (row.Cells[11].Value == true) Bounce = true;
            //                    //Array.Resize(ref motiomAnimation, motiomAnimation.Length + 1);
            //                    //objson[count] = coordinates;
            //                    //motiomAnimation[count] = new MotiomAnimation();
            //                    //motiomAnimation[count].X = x;
            //                    //motiomAnimation[count].Y = y;
            //                    //count++;
            //                    if (row.Cells[11].Value != null) Boolean.TryParse(row.Cells[11].Value.ToString(), out Bounce_b);
            //                    if (Bounce_b) Bounce = 1;
            //                    Coordinates StartCoordinates = new Coordinates();
            //                    Coordinates EndCoordinates = new Coordinates();
            //                    StartCoordinates.X = StartCoordinates_X;
            //                    StartCoordinates.Y = StartCoordinates_Y;
            //                    EndCoordinates.X = EndCoordinates_X;
            //                    EndCoordinates.Y = EndCoordinates_Y;

            //                    motiomAnimation.Unknown11d1p1 = Unknown1;
            //                    motiomAnimation.Unknown11d1p2 = StartCoordinates;
            //                    motiomAnimation.Unknown11d1p3 = EndCoordinates;
            //                    motiomAnimation.ImageIndex = ImageIndex;
            //                    motiomAnimation.Unknown11d1p5 = SpeedAnimation;
            //                    motiomAnimation.Unknown11d1p6 = TimeAnimation;
            //                    motiomAnimation.Unknown11d1p7 = Unknown5;
            //                    motiomAnimation.Unknown11d1p8 = Unknown6;
            //                    motiomAnimation.Unknown11d1p9 = Unknown7;
            //                    motiomAnimation.Unknown11d1p10 = Bounce;

            //                    MotiomAnimation.Add(motiomAnimation);
            //                }
            //            }
            //        }
            //        if (MotiomAnimation.Count > 0)
            //        {
            //            if (Watch_Face.Unknown11 == null) Watch_Face.Unknown11 = new Animation();
            //            Watch_Face.Unknown11.Unknown11_1 = MotiomAnimation;
            //        }
            //    }

            //    if ((checkBox_StaticAnimation.Checked) && (comboBox_StaticAnimation_Image.SelectedIndex >= 0))
            //    {
            //        if (Watch_Face.Unknown11 == null) Watch_Face.Unknown11 = new Animation();
            //        if (Watch_Face.Unknown11.Unknown11_2 == null) Watch_Face.Unknown11.Unknown11_2 = new StaticAnimation();
            //        if (Watch_Face.Unknown11.Unknown11_2.Unknown11d2p1 == null)
            //            Watch_Face.Unknown11.Unknown11_2.Unknown11d2p1 = new ImageSet();

            //        Watch_Face.Unknown11.Unknown11_2.Unknown11d2p1.ImageIndex = Int32.Parse(comboBox_StaticAnimation_Image.Text);
            //        Watch_Face.Unknown11.Unknown11_2.Unknown11d2p1.ImagesCount = (int)numericUpDown_StaticAnimation_Count.Value;
            //        Watch_Face.Unknown11.Unknown11_2.Unknown11d2p1.X = (int)numericUpDown_StaticAnimation_X.Value;
            //        Watch_Face.Unknown11.Unknown11_2.Unknown11d2p1.Y = (int)numericUpDown_StaticAnimation_Y.Value;

            //        Watch_Face.Unknown11.Unknown11_2.Unknown11d2p2 = (int)numericUpDown_StaticAnimation_SpeedAnimation.Value;
            //        Watch_Face.Unknown11.Unknown11_2.Unknown11d2p3 = 0;
            //        Watch_Face.Unknown11.Unknown11_2.Unknown11d2p4 = (int)numericUpDown_StaticAnimation_TimeAnimation.Value;
            //        Watch_Face.Unknown11.Unknown11_2.Unknown11d2p5 = (int)numericUpDown_StaticAnimation_Pause.Value;
            //    }

            //}

            richTextBox_JsonText.Text = JsonConvert.SerializeObject(Watch_Face, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            JsonToTree(richTextBox_JsonText.Text);
            JSON_Modified = true;

            //if (checkBox_UseID.Checked)
            //{
            //    int ID = 0;
            //    if (!Int32.TryParse(textBox_WatchfaceID.Text, out ID))
            //    {
                    
            //    }

            //    string fileNameOnly = Path.GetFileNameWithoutExtension(FileName);
            //    string path = Path.GetDirectoryName(FileName);
            //    path = Path.Combine(path, fileNameOnly);
            //    string JSONFileName = Path.Combine(path, "WatchfaceID.json");

            //    using (FileStream fileStream = File.OpenRead(FileName))
            //    {
            //        BinaryReader _reader = new BinaryReader(fileStream);
            //        _reader.ReadBytes(18);
            //        //fileStream.Position = 18;
            //        int ID = _reader.ReadInt16();

            //        WatchfaceID watchfaceID = new WatchfaceID();
            //        watchfaceID.ID = ID;
            //        watchfaceID.UseID = true;

            //        string JSON_String = JsonConvert.SerializeObject(watchfaceID, Formatting.Indented, new JsonSerializerSettings
            //        {
            //            //DefaultValueHandling = DefaultValueHandling.Ignore,
            //            NullValueHandling = NullValueHandling.Ignore
            //        });
            //        File.WriteAllText(JSONFileName, JSON_String, Encoding.UTF8);
            //    } 
            //}
        }

        private void AddActivityWeather(Panel panel_pictures, Panel panel_text, Panel panel_text_min, Panel panel_text_max, Panel panel_hand, Panel panel_scaleCircle, Panel panel_scaleLinear)
        {
            Activity activity = null;
            CheckBox checkBox_Use;

            // данные картинками
            checkBox_Use = (CheckBox)panel_pictures.Controls[0];
            if (checkBox_Use.Checked)
            {
                ComboBox comboBox_image = (ComboBox)panel_pictures.Controls[1];
                if (comboBox_image.SelectedIndex >= 0)
                {
                    NumericUpDown numericUpDownX = (NumericUpDown)panel_pictures.Controls[2];
                    NumericUpDown numericUpDownY = (NumericUpDown)panel_pictures.Controls[3];
                    NumericUpDown numericUpDown_count = (NumericUpDown)panel_pictures.Controls[4];

                    if (activity == null) activity = new Activity();
                    activity.ImageProgress = new ImageProgress();
                    activity.ImageProgress.ImageSet = new ImageSetGTR2();
                    activity.ImageProgress.Coordinates = new List<Coordinates>();
                    activity.ImageProgress.ImageSet.ImageIndex = Int32.Parse(comboBox_image.Text);
                    activity.ImageProgress.ImageSet.ImagesCount = 29;
                    //activity.ImageProgress.ImageSet.ImagesCount = (long)numericUpDown_count.Value;
                    Coordinates coordinates = new Coordinates();
                    coordinates.X = (long)numericUpDownX.Value;
                    coordinates.Y = (long)numericUpDownY.Value;
                    activity.ImageProgress.Coordinates.Add(coordinates);
                }
            }

            // данные надписью
            checkBox_Use = (CheckBox)panel_text.Controls[0];
            if (checkBox_Use.Checked)
            {
                ComboBox comboBox_image = (ComboBox)panel_text.Controls[1];
                if (comboBox_image.SelectedIndex >= 0)
                {
                    ComboBox comboBox_unit = (ComboBox)panel_text.Controls[2];
                    ComboBox comboBox_separatorF = (ComboBox)panel_text.Controls[3];
                    NumericUpDown numericUpDownX = (NumericUpDown)panel_text.Controls[4];
                    NumericUpDown numericUpDownY = (NumericUpDown)panel_text.Controls[5];
                    NumericUpDown numericUpDown_unitX = (NumericUpDown)panel_text.Controls[6];
                    NumericUpDown numericUpDown_unitY = (NumericUpDown)panel_text.Controls[7];
                    ComboBox comboBox_alignment = (ComboBox)panel_text.Controls[8];
                    NumericUpDown numericUpDown_spacing = (NumericUpDown)panel_text.Controls[9];
                    //CheckBox checkBox_add_zero = (CheckBox)panel_text.Controls[10];
                    ComboBox comboBox_imageError = (ComboBox)panel_text.Controls[10];
                    ComboBox comboBox_imageMinus = (ComboBox)panel_text.Controls[11];

                    if (activity == null) activity = new Activity();
                    activity.Digits = new List<DigitalCommonDigit>();
                    DigitalCommonDigit digitalCommonDigit = new DigitalCommonDigit();
                    digitalCommonDigit.CombingMode = "Single";
                    digitalCommonDigit.Digit = new Text();
                    string Alignment = StringToAlignment2(comboBox_alignment.SelectedIndex);
                    digitalCommonDigit.Digit.Alignment = Alignment;
                    //digitalCommonDigit.Digit.PaddingZero = checkBox_add_zero.Checked;
                    digitalCommonDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;
                    digitalCommonDigit.Digit.Image = new ImageAmazfit();

                    if (comboBox_imageError.SelectedIndex >= 0)
                        digitalCommonDigit.Digit.Image.NoDataImageIndex = Int32.Parse(comboBox_imageError.Text);

                    if (comboBox_imageMinus.SelectedIndex >= 0)
                        digitalCommonDigit.Digit.Image.DelimiterImageIndex = Int32.Parse(comboBox_imageMinus.Text);

                    digitalCommonDigit.Digit.Image.X = (long)numericUpDownX.Value;
                    digitalCommonDigit.Digit.Image.Y = (long)numericUpDownY.Value;
                    
                    digitalCommonDigit.Digit.Image.MultilangImage = new List<MultilangImage>();
                    MultilangImage multilangImage = new MultilangImage();
                    multilangImage.LangCode = "All";
                    multilangImage.ImageSet = new ImageSetGTR2();
                    multilangImage.ImageSet.ImagesCount = 10;
                    multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_image.Text);
                    digitalCommonDigit.Digit.Image.MultilangImage.Add(multilangImage);

                    if (comboBox_separatorF.SelectedIndex >= 0)
                    {
                        if (digitalCommonDigit.Digit.Image.MultilangImageUnit == null)
                            digitalCommonDigit.Digit.Image.MultilangImageUnit = new List<MultilangImage>();
                        multilangImage = new MultilangImage();
                        multilangImage.LangCode = "All";
                        multilangImage.ImageSet = new ImageSetGTR2();
                        multilangImage.ImageSet.ImagesCount = 1;
                        multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_separatorF.Text);
                        digitalCommonDigit.Digit.Image.MultilangImageUnit.Add(multilangImage);
                    }

                    if (comboBox_unit.SelectedIndex >= 0)
                    {
                        digitalCommonDigit.Separator = new ImageCoord();
                        digitalCommonDigit.Separator.ImageIndex = Int32.Parse(comboBox_unit.Text);
                        digitalCommonDigit.Separator.Coordinates = new Coordinates();
                        digitalCommonDigit.Separator.Coordinates.X = (long)numericUpDown_unitX.Value;
                        digitalCommonDigit.Separator.Coordinates.Y = (long)numericUpDown_unitY.Value;
                    }

                    activity.Digits.Add(digitalCommonDigit);
                }
            }

            // данные надписью min temperature
            checkBox_Use = (CheckBox)panel_text_min.Controls[0];
            if (checkBox_Use.Checked)
            {
                ComboBox comboBox_image = (ComboBox)panel_text_min.Controls[1];
                if (comboBox_image.SelectedIndex >= 0)
                {
                    ComboBox comboBox_unit = (ComboBox)panel_text_min.Controls[2];
                    ComboBox comboBox_separatorF = (ComboBox)panel_text_min.Controls[3];
                    NumericUpDown numericUpDownX = (NumericUpDown)panel_text_min.Controls[4];
                    NumericUpDown numericUpDownY = (NumericUpDown)panel_text_min.Controls[5];
                    NumericUpDown numericUpDown_unitX = (NumericUpDown)panel_text_min.Controls[6];
                    NumericUpDown numericUpDown_unitY = (NumericUpDown)panel_text_min.Controls[7];
                    ComboBox comboBox_alignment = (ComboBox)panel_text_min.Controls[8];
                    NumericUpDown numericUpDown_spacing = (NumericUpDown)panel_text_min.Controls[9];
                    //CheckBox checkBox_add_zero = (CheckBox)panel_text_min.Controls[10];
                    ComboBox comboBox_imageError = (ComboBox)panel_text_min.Controls[10];
                    ComboBox comboBox_imageMinus = (ComboBox)panel_text_min.Controls[11];

                    if (activity == null) activity = new Activity();
                    if (activity.Digits == null) activity.Digits = new List<DigitalCommonDigit>();
                    DigitalCommonDigit digitalCommonDigit = new DigitalCommonDigit();
                    digitalCommonDigit.Type = "Min";
                    digitalCommonDigit.CombingMode = "Single";
                    digitalCommonDigit.Digit = new Text();
                    string Alignment = StringToAlignment2(comboBox_alignment.SelectedIndex);
                    digitalCommonDigit.Digit.Alignment = Alignment;
                    //digitalCommonDigit.Digit.PaddingZero = checkBox_add_zero.Checked;
                    digitalCommonDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;
                    digitalCommonDigit.Digit.Image = new ImageAmazfit();

                    if (comboBox_imageError.SelectedIndex >= 0)
                        digitalCommonDigit.Digit.Image.NoDataImageIndex = Int32.Parse(comboBox_imageError.Text);

                    if (comboBox_imageMinus.SelectedIndex >= 0)
                        digitalCommonDigit.Digit.Image.DelimiterImageIndex = Int32.Parse(comboBox_imageMinus.Text);

                    digitalCommonDigit.Digit.Image.X = (long)numericUpDownX.Value;
                    digitalCommonDigit.Digit.Image.Y = (long)numericUpDownY.Value;

                    digitalCommonDigit.Digit.Image.MultilangImage = new List<MultilangImage>();
                    MultilangImage multilangImage = new MultilangImage();
                    multilangImage.LangCode = "All";
                    multilangImage.ImageSet = new ImageSetGTR2();
                    multilangImage.ImageSet.ImagesCount = 10;
                    multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_image.Text);
                    digitalCommonDigit.Digit.Image.MultilangImage.Add(multilangImage);

                    if (comboBox_separatorF.SelectedIndex >= 0)
                    {
                        if (digitalCommonDigit.Digit.Image.MultilangImageUnit == null)
                            digitalCommonDigit.Digit.Image.MultilangImageUnit = new List<MultilangImage>();
                        multilangImage = new MultilangImage();
                        multilangImage.LangCode = "All";
                        multilangImage.ImageSet = new ImageSetGTR2();
                        multilangImage.ImageSet.ImagesCount = 1;
                        multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_separatorF.Text);
                        digitalCommonDigit.Digit.Image.MultilangImageUnit.Add(multilangImage);
                    }

                    if (comboBox_unit.SelectedIndex >= 0)
                    {
                        digitalCommonDigit.Separator = new ImageCoord();
                        digitalCommonDigit.Separator.ImageIndex = Int32.Parse(comboBox_unit.Text);
                        digitalCommonDigit.Separator.Coordinates = new Coordinates();
                        digitalCommonDigit.Separator.Coordinates.X = (long)numericUpDown_unitX.Value;
                        digitalCommonDigit.Separator.Coordinates.Y = (long)numericUpDown_unitY.Value;
                    }

                    activity.Digits.Add(digitalCommonDigit);
                }
            }

            // данные надписью max temperature
            checkBox_Use = (CheckBox)panel_text_max.Controls[0];
            if (checkBox_Use.Checked)
            {
                ComboBox comboBox_image = (ComboBox)panel_text_max.Controls[1];
                if (comboBox_image.SelectedIndex >= 0)
                {
                    ComboBox comboBox_unit = (ComboBox)panel_text_max.Controls[2];
                    ComboBox comboBox_separatorF = (ComboBox)panel_text_max.Controls[3];
                    NumericUpDown numericUpDownX = (NumericUpDown)panel_text_max.Controls[4];
                    NumericUpDown numericUpDownY = (NumericUpDown)panel_text_max.Controls[5];
                    NumericUpDown numericUpDown_unitX = (NumericUpDown)panel_text_max.Controls[6];
                    NumericUpDown numericUpDown_unitY = (NumericUpDown)panel_text_max.Controls[7];
                    ComboBox comboBox_alignment = (ComboBox)panel_text_max.Controls[8];
                    NumericUpDown numericUpDown_spacing = (NumericUpDown)panel_text_max.Controls[9];
                    //CheckBox checkBox_add_zero = (CheckBox)panel_text_max.Controls[10];
                    ComboBox comboBox_imageError = (ComboBox)panel_text_max.Controls[10];
                    ComboBox comboBox_imageMinus = (ComboBox)panel_text_max.Controls[11];
                    CheckBox checkBox_follow = (CheckBox)panel_text_max.Controls[12];

                    if (activity == null) activity = new Activity();
                    if (activity.Digits == null) activity.Digits = new List<DigitalCommonDigit>();
                    DigitalCommonDigit digitalCommonDigit = new DigitalCommonDigit();
                    digitalCommonDigit.Type = "Max";
                    if (!checkBox_follow.Checked) digitalCommonDigit.CombingMode = "Single";
                    //digitalCommonDigit.CombingMode = "Single";
                    digitalCommonDigit.Digit = new Text();
                    string Alignment = StringToAlignment2(comboBox_alignment.SelectedIndex);
                    digitalCommonDigit.Digit.Alignment = Alignment;
                    //digitalCommonDigit.Digit.PaddingZero = checkBox_add_zero.Checked;
                    digitalCommonDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;
                    digitalCommonDigit.Digit.Image = new ImageAmazfit();

                    if (comboBox_imageError.SelectedIndex >= 0)
                        digitalCommonDigit.Digit.Image.NoDataImageIndex = Int32.Parse(comboBox_imageError.Text);

                    if (comboBox_imageMinus.SelectedIndex >= 0)
                        digitalCommonDigit.Digit.Image.DelimiterImageIndex = Int32.Parse(comboBox_imageMinus.Text);

                    digitalCommonDigit.Digit.Image.X = (long)numericUpDownX.Value;
                    digitalCommonDigit.Digit.Image.Y = (long)numericUpDownY.Value;

                    digitalCommonDigit.Digit.Image.MultilangImage = new List<MultilangImage>();
                    MultilangImage multilangImage = new MultilangImage();
                    multilangImage.LangCode = "All";
                    multilangImage.ImageSet = new ImageSetGTR2();
                    multilangImage.ImageSet.ImagesCount = 10;
                    multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_image.Text);
                    digitalCommonDigit.Digit.Image.MultilangImage.Add(multilangImage);

                    if (comboBox_separatorF.SelectedIndex >= 0)
                    {
                        if (digitalCommonDigit.Digit.Image.MultilangImageUnit == null)
                            digitalCommonDigit.Digit.Image.MultilangImageUnit = new List<MultilangImage>();
                        multilangImage = new MultilangImage();
                        multilangImage.LangCode = "All";
                        multilangImage.ImageSet = new ImageSetGTR2();
                        multilangImage.ImageSet.ImagesCount = 1;
                        multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_separatorF.Text);
                        digitalCommonDigit.Digit.Image.MultilangImageUnit.Add(multilangImage);
                    }

                    if (comboBox_unit.SelectedIndex >= 0)
                    {
                        digitalCommonDigit.Separator = new ImageCoord();
                        digitalCommonDigit.Separator.ImageIndex = Int32.Parse(comboBox_unit.Text);
                        digitalCommonDigit.Separator.Coordinates = new Coordinates();
                        digitalCommonDigit.Separator.Coordinates.X = (long)numericUpDown_unitX.Value;
                        digitalCommonDigit.Separator.Coordinates.Y = (long)numericUpDown_unitY.Value;
                    }

                    activity.Digits.Add(digitalCommonDigit);
                }
            }

            // данные стрелкой
            checkBox_Use = (CheckBox)panel_hand.Controls[0];
            if (checkBox_Use.Checked)
            {
                ComboBox comboBox_image = (ComboBox)panel_hand.Controls[1];
                if (comboBox_image.SelectedIndex >= 0)
                {
                    NumericUpDown numericUpDownX = (NumericUpDown)panel_hand.Controls[2];
                    NumericUpDown numericUpDownY = (NumericUpDown)panel_hand.Controls[3];
                    NumericUpDown numericUpDown_offsetX = (NumericUpDown)panel_hand.Controls[4];
                    NumericUpDown numericUpDown_offsetY = (NumericUpDown)panel_hand.Controls[5];
                    ComboBox comboBox_imageCentr = (ComboBox)panel_hand.Controls[6];
                    NumericUpDown numericUpDownX_centr = (NumericUpDown)panel_hand.Controls[7];
                    NumericUpDown numericUpDownY_centr = (NumericUpDown)panel_hand.Controls[8];
                    NumericUpDown numericUpDown_startAngle = (NumericUpDown)panel_hand.Controls[9];
                    NumericUpDown numericUpDown_endAngle = (NumericUpDown)panel_hand.Controls[10];
                    ComboBox comboBox_imageBackground = (ComboBox)panel_hand.Controls[11];
                    NumericUpDown numericUpDownX_background = (NumericUpDown)panel_hand.Controls[12];
                    NumericUpDown numericUpDownY_background = (NumericUpDown)panel_hand.Controls[13];

                    if (activity == null) activity = new Activity();
                    activity.PointerProgress = new ClockHand();
                    activity.PointerProgress.X = (long)numericUpDownX.Value;
                    activity.PointerProgress.Y = (long)numericUpDownY.Value;
                    activity.PointerProgress.StartAngle = (float)numericUpDown_startAngle.Value;
                    activity.PointerProgress.EndAngle = (float)numericUpDown_endAngle.Value;

                    activity.PointerProgress.Pointer = new ImageCoord();
                    activity.PointerProgress.Pointer.ImageIndex = Int32.Parse(comboBox_image.Text);
                    activity.PointerProgress.Pointer.Coordinates = new Coordinates();
                    activity.PointerProgress.Pointer.Coordinates.X = (long)numericUpDown_offsetX.Value;
                    activity.PointerProgress.Pointer.Coordinates.Y = (long)numericUpDown_offsetY.Value;

                    if (comboBox_imageCentr.SelectedIndex >= 0)
                    {
                        activity.PointerProgress.Cover = new ImageCoord();
                        activity.PointerProgress.Cover.ImageIndex = Int32.Parse(comboBox_imageCentr.Text);
                        activity.PointerProgress.Cover.Coordinates = new Coordinates();
                        activity.PointerProgress.Cover.Coordinates.X = (long)numericUpDownX_centr.Value;
                        activity.PointerProgress.Cover.Coordinates.Y = (long)numericUpDownY_centr.Value;
                    }

                    if (comboBox_imageBackground.SelectedIndex >= 0)
                    {
                        activity.PointerProgress.Scale = new MultilangImageCoord();
                        activity.PointerProgress.Scale.Coordinates = new Coordinates();
                        activity.PointerProgress.Scale.Coordinates.X = (long)numericUpDownX_background.Value;
                        activity.PointerProgress.Scale.Coordinates.Y = (long)numericUpDownY_background.Value;
                        activity.PointerProgress.Scale.ImageSet = new List<MultilangImage>();
                        MultilangImage multilangImage = new MultilangImage();
                        multilangImage.LangCode = "All";
                        multilangImage.ImageSet = new ImageSetGTR2();
                        multilangImage.ImageSet.ImagesCount = 1;
                        multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_imageBackground.Text);
                        activity.PointerProgress.Scale.ImageSet.Add(multilangImage);
                    }
                }
            }

            // данные круговой шкалой
            checkBox_Use = (CheckBox)panel_scaleCircle.Controls[0];
            if (checkBox_Use.Checked)
            {
                RadioButton radioButton_image = (RadioButton)panel_scaleCircle.Controls[1];
                //RadioButton radioButton_color = (RadioButton)panel_scaleCircle.Controls[2];
                ComboBox comboBox_image = (ComboBox)panel_scaleCircle.Controls[3];
                ComboBox comboBox_color = (ComboBox)panel_scaleCircle.Controls[4];
                ComboBox comboBox_flatness = (ComboBox)panel_scaleCircle.Controls[5];
                ComboBox comboBox_background = (ComboBox)panel_scaleCircle.Controls[6];
                NumericUpDown numericUpDownX = (NumericUpDown)panel_scaleCircle.Controls[7];
                NumericUpDown numericUpDownY = (NumericUpDown)panel_scaleCircle.Controls[8];
                NumericUpDown numericUpDown_radius = (NumericUpDown)panel_scaleCircle.Controls[9];
                NumericUpDown numericUpDown_width = (NumericUpDown)panel_scaleCircle.Controls[10];
                NumericUpDown numericUpDown_startAngle = (NumericUpDown)panel_scaleCircle.Controls[11];
                NumericUpDown numericUpDown_endAngle = (NumericUpDown)panel_scaleCircle.Controls[12];

                if ((radioButton_image.Checked && comboBox_image.SelectedIndex >= 0) ||
                    (!radioButton_image.Checked))
                {
                    if (activity == null) activity = new Activity();
                    if (activity.ProgressBar == null) activity.ProgressBar = new ProgressBar();
                    activity.ProgressBar.AngleSettings = new AngleSettings();
                    if (radioButton_image.Checked && comboBox_image.SelectedIndex >= 0)
                    {
                        activity.ProgressBar.ForegroundImageIndex = Int32.Parse(comboBox_image.Text);
                    }
                    else
                    {
                        Color color = comboBox_color.BackColor;
                        Color new_color = Color.FromArgb(0, color.R, color.G, color.B);
                        string colorStr = ColorTranslator.ToHtml(new_color);
                        colorStr = colorStr.Replace("#", "0xFF");
                        activity.ProgressBar.Color = colorStr;
                    }

                    if (comboBox_background.SelectedIndex >= 0)
                        activity.ProgressBar.BackgroundImageIndex = Int32.Parse(comboBox_background.Text);

                    activity.ProgressBar.AngleSettings.X = (long)numericUpDownX.Value;
                    activity.ProgressBar.AngleSettings.Y = (long)numericUpDownY.Value;
                    activity.ProgressBar.AngleSettings.StartAngle = (float)numericUpDown_startAngle.Value;
                    activity.ProgressBar.AngleSettings.EndAngle = (float)numericUpDown_endAngle.Value;
                    activity.ProgressBar.AngleSettings.Radius = (float)numericUpDown_radius.Value;

                    activity.ProgressBar.Width = (long)numericUpDown_width.Value;

                    switch (comboBox_flatness.SelectedIndex)
                    {
                        case 1:
                            activity.ProgressBar.Flatness = 90;
                            break;
                        case 2:
                            activity.ProgressBar.Flatness = 180;
                            break;
                        default:
                            activity.ProgressBar.Flatness = 0;
                            break;
                    }
                }
            }

            // данные линейной шкалой
            checkBox_Use = (CheckBox)panel_scaleLinear.Controls[0];
            if (checkBox_Use.Checked)
            {
                RadioButton radioButton_image = (RadioButton)panel_scaleLinear.Controls[1];
                //RadioButton radioButton_color = (RadioButton)panel_scaleLinear.Controls[2];
                ComboBox comboBox_image = (ComboBox)panel_scaleLinear.Controls[3];
                ComboBox comboBox_color = (ComboBox)panel_scaleLinear.Controls[4];
                ComboBox comboBox_pointer = (ComboBox)panel_scaleLinear.Controls[5];
                ComboBox comboBox_background = (ComboBox)panel_scaleLinear.Controls[6];
                NumericUpDown numericUpDownX = (NumericUpDown)panel_scaleLinear.Controls[7];
                NumericUpDown numericUpDownY = (NumericUpDown)panel_scaleLinear.Controls[8];
                NumericUpDown numericUpDown_length = (NumericUpDown)panel_scaleLinear.Controls[9];
                NumericUpDown numericUpDown_width = (NumericUpDown)panel_scaleLinear.Controls[10];

                if ((radioButton_image.Checked && comboBox_image.SelectedIndex >= 0) ||
                    (!radioButton_image.Checked))
                {
                    if (activity == null) activity = new Activity();
                    if (activity.ProgressBar == null) activity.ProgressBar = new ProgressBar();
                    activity.ProgressBar.LinearSettings = new LinearSettings();
                    if (radioButton_image.Checked && comboBox_image.SelectedIndex >= 0)
                    {
                        activity.ProgressBar.ForegroundImageIndex = Int32.Parse(comboBox_image.Text);
                    }
                    else
                    {
                        Color color = comboBox_color.BackColor;
                        Color new_color = Color.FromArgb(0, color.R, color.G, color.B);
                        string colorStr = ColorTranslator.ToHtml(new_color);
                        colorStr = colorStr.Replace("#", "0xFF");
                        activity.ProgressBar.Color = colorStr;
                    }
                    if (comboBox_pointer.SelectedIndex >= 0)
                        activity.ProgressBar.PointerImageIndex = Int32.Parse(comboBox_pointer.Text);
                    if (comboBox_background.SelectedIndex >= 0)
                        activity.ProgressBar.BackgroundImageIndex = Int32.Parse(comboBox_background.Text);

                    activity.ProgressBar.LinearSettings.StartX = (long)numericUpDownX.Value;
                    activity.ProgressBar.LinearSettings.StartY = (long)numericUpDownY.Value;
                    long endX = (long)(numericUpDownX.Value + numericUpDown_length.Value);
                    activity.ProgressBar.LinearSettings.EndX = endX;
                    activity.ProgressBar.LinearSettings.EndY = (long)numericUpDownY.Value;
                    activity.ProgressBar.Width = (long)numericUpDown_width.Value;
                }

            }

            if (activity != null)
            {
                activity.Type = "Weather";
                if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                if (Watch_Face.System.Activity == null) Watch_Face.System.Activity = new List<Activity>();
                Watch_Face.System.Activity.Add(activity);
            }
        }

        private void AddActivity(Panel panel_pictures, Panel panel_text, Panel panel_hand, Panel panel_scaleCircle, Panel panel_scaleLinear, string type)
        {
            Activity activity = null;
            CheckBox checkBox_Use;

            // данные картинками
            checkBox_Use = (CheckBox)panel_pictures.Controls[0];
            if (checkBox_Use.Checked)
            {
                ComboBox comboBox_image = (ComboBox)panel_pictures.Controls[1];
                if (comboBox_image.SelectedIndex >= 0)
                {
                    NumericUpDown numericUpDownX = (NumericUpDown)panel_pictures.Controls[2];
                    NumericUpDown numericUpDownY = (NumericUpDown)panel_pictures.Controls[3];
                    NumericUpDown numericUpDown_count = (NumericUpDown)panel_pictures.Controls[4];

                    if (activity == null) activity = new Activity();
                    activity.ImageProgress = new ImageProgress();
                    activity.ImageProgress.ImageSet = new ImageSetGTR2();
                    activity.ImageProgress.Coordinates = new List<Coordinates>();
                    activity.ImageProgress.ImageSet.ImageIndex = Int32.Parse(comboBox_image.Text);
                    activity.ImageProgress.ImageSet.ImagesCount = (long)numericUpDown_count.Value;
                    Coordinates coordinates = new Coordinates();
                    coordinates.X = (long)numericUpDownX.Value;
                    coordinates.Y = (long)numericUpDownY.Value;
                    activity.ImageProgress.Coordinates.Add(coordinates);
                }
            }

            // данные надписью
            checkBox_Use = (CheckBox)panel_text.Controls[0];
            if (checkBox_Use.Checked)
            {
                ComboBox comboBox_image = (ComboBox)panel_text.Controls[1];
                if (comboBox_image.SelectedIndex >= 0)
                {
                    ComboBox comboBox_unit = (ComboBox)panel_text.Controls[2];
                    ComboBox comboBox_separator = (ComboBox)panel_text.Controls[3];
                    NumericUpDown numericUpDownX = (NumericUpDown)panel_text.Controls[4];
                    NumericUpDown numericUpDownY = (NumericUpDown)panel_text.Controls[5];
                    NumericUpDown numericUpDown_unitX = (NumericUpDown)panel_text.Controls[6];
                    NumericUpDown numericUpDown_unitY = (NumericUpDown)panel_text.Controls[7];
                    ComboBox comboBox_alignment = (ComboBox)panel_text.Controls[8];
                    NumericUpDown numericUpDown_spacing = (NumericUpDown)panel_text.Controls[9];
                    CheckBox checkBox_add_zero = (CheckBox)panel_text.Controls[10];
                    ComboBox comboBox_imageError = (ComboBox)panel_text.Controls[11];

                    if (activity == null) activity = new Activity();
                    activity.Digits = new List<DigitalCommonDigit>();
                    DigitalCommonDigit digitalCommonDigit = new DigitalCommonDigit();
                    digitalCommonDigit.CombingMode = "Single";
                    digitalCommonDigit.Digit = new Text();
                    string Alignment = StringToAlignment2(comboBox_alignment.SelectedIndex);
                    digitalCommonDigit.Digit.Alignment = Alignment;
                    digitalCommonDigit.Digit.PaddingZero = checkBox_add_zero.Checked;
                    digitalCommonDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;
                    digitalCommonDigit.Digit.Image = new ImageAmazfit();
                    if (comboBox_imageError.SelectedIndex >= 0)
                        digitalCommonDigit.Digit.Image.NoDataImageIndex = Int32.Parse(comboBox_imageError.Text);

                    digitalCommonDigit.Digit.Image.X = (long)numericUpDownX.Value;
                    digitalCommonDigit.Digit.Image.Y = (long)numericUpDownY.Value;
                    if (type == "Distance")
                    {
                        ComboBox comboBox_DecimalPoint = (ComboBox)panel_text.Controls[12];
                        if (comboBox_DecimalPoint.SelectedIndex >= 0)
                        {
                            digitalCommonDigit.Digit.Image.DecimalPointImageIndex = Int32.Parse(comboBox_DecimalPoint.Text);
                        }
                    }
                    digitalCommonDigit.Digit.Image.MultilangImage = new List<MultilangImage>();
                    MultilangImage multilangImage = new MultilangImage();
                    multilangImage.LangCode = "All";
                    multilangImage.ImageSet = new ImageSetGTR2();
                    multilangImage.ImageSet.ImagesCount = 10;
                    multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_image.Text);
                    digitalCommonDigit.Digit.Image.MultilangImage.Add(multilangImage);

                    if (comboBox_separator.SelectedIndex >= 0)
                    {
                        digitalCommonDigit.Digit.Image.MultilangImageUnit = new List<MultilangImage>();
                        multilangImage = new MultilangImage();
                        multilangImage.LangCode = "All";
                        multilangImage.ImageSet = new ImageSetGTR2();
                        multilangImage.ImageSet.ImagesCount = 1;
                        multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_separator.Text);
                        digitalCommonDigit.Digit.Image.MultilangImageUnit.Add(multilangImage);
                    }

                    if (comboBox_unit.SelectedIndex >= 0)
                    {
                        digitalCommonDigit.Separator = new ImageCoord();
                        digitalCommonDigit.Separator.ImageIndex = Int32.Parse(comboBox_unit.Text);
                        digitalCommonDigit.Separator.Coordinates = new Coordinates();
                        digitalCommonDigit.Separator.Coordinates.X = (long)numericUpDown_unitX.Value;
                        digitalCommonDigit.Separator.Coordinates.Y = (long)numericUpDown_unitY.Value;
                    }

                    activity.Digits.Add(digitalCommonDigit);
                }
            }

            // данные стрелкой
            checkBox_Use = (CheckBox)panel_hand.Controls[0];
            if (checkBox_Use.Checked)
            {
                ComboBox comboBox_image = (ComboBox)panel_hand.Controls[1];
                if (comboBox_image.SelectedIndex >= 0)
                {
                    NumericUpDown numericUpDownX = (NumericUpDown)panel_hand.Controls[2];
                    NumericUpDown numericUpDownY = (NumericUpDown)panel_hand.Controls[3];
                    NumericUpDown numericUpDown_offsetX = (NumericUpDown)panel_hand.Controls[4];
                    NumericUpDown numericUpDown_offsetY = (NumericUpDown)panel_hand.Controls[5];
                    ComboBox comboBox_imageCentr = (ComboBox)panel_hand.Controls[6];
                    NumericUpDown numericUpDownX_centr = (NumericUpDown)panel_hand.Controls[7];
                    NumericUpDown numericUpDownY_centr = (NumericUpDown)panel_hand.Controls[8];
                    NumericUpDown numericUpDown_startAngle = (NumericUpDown)panel_hand.Controls[9];
                    NumericUpDown numericUpDown_endAngle = (NumericUpDown)panel_hand.Controls[10];
                    ComboBox comboBox_imageBackground = (ComboBox)panel_hand.Controls[11];
                    NumericUpDown numericUpDownX_background = (NumericUpDown)panel_hand.Controls[12];
                    NumericUpDown numericUpDownY_background = (NumericUpDown)panel_hand.Controls[13];

                    if (activity == null) activity = new Activity();
                    activity.PointerProgress = new ClockHand();
                    activity.PointerProgress.X = (long)numericUpDownX.Value;
                    activity.PointerProgress.Y = (long)numericUpDownY.Value;
                    activity.PointerProgress.StartAngle = (float)numericUpDown_startAngle.Value;
                    activity.PointerProgress.EndAngle = (float)numericUpDown_endAngle.Value;

                    activity.PointerProgress.Pointer = new ImageCoord();
                    activity.PointerProgress.Pointer.ImageIndex = Int32.Parse(comboBox_image.Text);
                    activity.PointerProgress.Pointer.Coordinates = new Coordinates();
                    activity.PointerProgress.Pointer.Coordinates.X = (long)numericUpDown_offsetX.Value;
                    activity.PointerProgress.Pointer.Coordinates.Y = (long)numericUpDown_offsetY.Value;

                    if (comboBox_imageCentr.SelectedIndex >= 0)
                    {
                        activity.PointerProgress.Cover = new ImageCoord();
                        activity.PointerProgress.Cover.ImageIndex = Int32.Parse(comboBox_imageCentr.Text);
                        activity.PointerProgress.Cover.Coordinates = new Coordinates();
                        activity.PointerProgress.Cover.Coordinates.X = (long)numericUpDownX_centr.Value;
                        activity.PointerProgress.Cover.Coordinates.Y = (long)numericUpDownY_centr.Value;
                    }

                    if (comboBox_imageBackground.SelectedIndex >= 0)
                    {
                        activity.PointerProgress.Scale = new MultilangImageCoord();
                        activity.PointerProgress.Scale.Coordinates = new Coordinates();
                        activity.PointerProgress.Scale.Coordinates.X = (long)numericUpDownX_background.Value;
                        activity.PointerProgress.Scale.Coordinates.Y = (long)numericUpDownY_background.Value;
                        activity.PointerProgress.Scale.ImageSet = new List<MultilangImage>();
                        MultilangImage multilangImage = new MultilangImage();
                        multilangImage.LangCode = "All";
                        multilangImage.ImageSet = new ImageSetGTR2();
                        multilangImage.ImageSet.ImagesCount = 1;
                        multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_imageBackground.Text);
                        activity.PointerProgress.Scale.ImageSet.Add(multilangImage);
                    }
                }
            }

            // данные круговой шкалой
            checkBox_Use = (CheckBox)panel_scaleCircle.Controls[0];
            if (checkBox_Use.Checked)
            {
                RadioButton radioButton_image = (RadioButton)panel_scaleCircle.Controls[1];
                //RadioButton radioButton_color = (RadioButton)panel_scaleCircle.Controls[2];
                ComboBox comboBox_image = (ComboBox)panel_scaleCircle.Controls[3];
                ComboBox comboBox_color = (ComboBox)panel_scaleCircle.Controls[4];
                ComboBox comboBox_flatness = (ComboBox)panel_scaleCircle.Controls[5];
                ComboBox comboBox_background = (ComboBox)panel_scaleCircle.Controls[6];
                NumericUpDown numericUpDownX = (NumericUpDown)panel_scaleCircle.Controls[7];
                NumericUpDown numericUpDownY = (NumericUpDown)panel_scaleCircle.Controls[8];
                NumericUpDown numericUpDown_radius = (NumericUpDown)panel_scaleCircle.Controls[9];
                NumericUpDown numericUpDown_width = (NumericUpDown)panel_scaleCircle.Controls[10];
                NumericUpDown numericUpDown_startAngle = (NumericUpDown)panel_scaleCircle.Controls[11];
                NumericUpDown numericUpDown_endAngle = (NumericUpDown)panel_scaleCircle.Controls[12];

                if ((radioButton_image.Checked && comboBox_image.SelectedIndex >= 0) ||
                    (!radioButton_image.Checked))
                {
                    if (activity == null) activity = new Activity();
                    if (activity.ProgressBar == null) activity.ProgressBar = new ProgressBar();
                    activity.ProgressBar.AngleSettings = new AngleSettings();
                    if (radioButton_image.Checked && comboBox_image.SelectedIndex >= 0)
                    {
                        activity.ProgressBar.ForegroundImageIndex = Int32.Parse(comboBox_image.Text);
                    }
                    else
                    {
                        Color color = comboBox_color.BackColor;
                        Color new_color = Color.FromArgb(0, color.R, color.G, color.B);
                        string colorStr = ColorTranslator.ToHtml(new_color);
                        colorStr = colorStr.Replace("#", "0xFF");
                        activity.ProgressBar.Color = colorStr;
                    }

                    if (comboBox_background.SelectedIndex >= 0)
                        activity.ProgressBar.BackgroundImageIndex = Int32.Parse(comboBox_background.Text);

                    activity.ProgressBar.AngleSettings.X = (long)numericUpDownX.Value;
                    activity.ProgressBar.AngleSettings.Y = (long)numericUpDownY.Value;
                    activity.ProgressBar.AngleSettings.StartAngle = (float)numericUpDown_startAngle.Value;
                    activity.ProgressBar.AngleSettings.EndAngle = (float)numericUpDown_endAngle.Value;
                    activity.ProgressBar.AngleSettings.Radius = (float)numericUpDown_radius.Value;

                    activity.ProgressBar.Width = (long)numericUpDown_width.Value;

                    switch (comboBox_flatness.SelectedIndex)
                    {
                        case 1:
                            activity.ProgressBar.Flatness = 90;
                            break;
                        case 2:
                            activity.ProgressBar.Flatness = 180;
                            break;
                        default:
                            activity.ProgressBar.Flatness = 0;
                            break;
                    }
                }
            }

            // данные линейной шкалой
            checkBox_Use = (CheckBox)panel_scaleLinear.Controls[0];
            if (checkBox_Use.Checked)
            {
                RadioButton radioButton_image = (RadioButton)panel_scaleLinear.Controls[1];
                //RadioButton radioButton_color = (RadioButton)panel_scaleLinear.Controls[2];
                ComboBox comboBox_image = (ComboBox)panel_scaleLinear.Controls[3];
                ComboBox comboBox_color = (ComboBox)panel_scaleLinear.Controls[4];
                ComboBox comboBox_pointer = (ComboBox)panel_scaleLinear.Controls[5];
                ComboBox comboBox_background = (ComboBox)panel_scaleLinear.Controls[6];
                NumericUpDown numericUpDownX = (NumericUpDown)panel_scaleLinear.Controls[7];
                NumericUpDown numericUpDownY = (NumericUpDown)panel_scaleLinear.Controls[8];
                NumericUpDown numericUpDown_length = (NumericUpDown)panel_scaleLinear.Controls[9];
                NumericUpDown numericUpDown_width = (NumericUpDown)panel_scaleLinear.Controls[10];

                if ((radioButton_image.Checked && comboBox_image.SelectedIndex >= 0) ||
                    (!radioButton_image.Checked))
                {
                    if (activity == null) activity = new Activity();
                    if (activity.ProgressBar == null) activity.ProgressBar = new ProgressBar();
                    activity.ProgressBar.LinearSettings = new LinearSettings();
                    if (radioButton_image.Checked && comboBox_image.SelectedIndex >= 0)
                    {
                        activity.ProgressBar.ForegroundImageIndex = Int32.Parse(comboBox_image.Text);
                    }
                    else
                    {
                        Color color = comboBox_color.BackColor;
                        Color new_color = Color.FromArgb(0, color.R, color.G, color.B);
                        string colorStr = ColorTranslator.ToHtml(new_color);
                        colorStr = colorStr.Replace("#", "0xFF");
                        activity.ProgressBar.Color = colorStr;
                    }
                    if (comboBox_pointer.SelectedIndex >= 0)
                        activity.ProgressBar.PointerImageIndex = Int32.Parse(comboBox_pointer.Text);
                    if (comboBox_background.SelectedIndex >= 0)
                        activity.ProgressBar.BackgroundImageIndex = Int32.Parse(comboBox_background.Text);

                    activity.ProgressBar.LinearSettings.StartX = (long)numericUpDownX.Value;
                    activity.ProgressBar.LinearSettings.StartY = (long)numericUpDownY.Value;
                    long endX = (long)(numericUpDownX.Value + numericUpDown_length.Value);
                    activity.ProgressBar.LinearSettings.EndX = endX;
                    activity.ProgressBar.LinearSettings.EndY = (long)numericUpDownY.Value;
                    activity.ProgressBar.Width = (long)numericUpDown_width.Value;
                }

            }

            if (activity != null)
            {
                activity.Type = type;
                if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                if (Watch_Face.System.Activity == null) Watch_Face.System.Activity = new List<Activity>();
                Watch_Face.System.Activity.Add(activity);
            }
        }

        private void AlignmentToString(ComboBox comboBoxAlignment, string Alignment)
        {
            int result = 0;
            switch (Alignment)
            {
                case "TopLeft":
                    //result = "Вверх влево";
                    result = 0;
                    break;
                case "TopCenter":
                    //result = "Вверх по центру";
                    result = 1;
                    break;
                case "TopRight":
                    //result = "Вверх вправо";
                    result = 2;
                    break;

                case "CenterLeft":
                    //result = "Середина влево";
                    result = 3;
                    break;
                case "Center":
                    //result = "Середина по центру";
                    result = 4;
                    break;
                case "CenterRight":
                    //result = "Середина вправо";
                    result = 5;
                    break;

                case "BottomLeft":
                    //result = "Вниз влево";
                    result = 6;
                    break;
                case "BottomCenter":
                    //result = "Вниз по центру";
                    result = 7;
                    break;
                case "BottomRight":
                    //result = "Вниз вправо";
                    result = 8;
                    break;

                case "Left":
                    //result = "Середина влево";
                    result = 3;
                    break;
                case "Right":
                    //result = "Середина вправо";
                    result = 5;
                    break;
                case "Top":
                    //result = "Вверх по центру";
                    result = 1;
                    break;
                case "Bottom":
                    //result = "Вниз по центру";
                    result = 7;
                    break;

                default:
                    //result = "Середина по центру";
                    result = 4;
                    break;

            }
            //return result;
            comboBoxAlignment.SelectedIndex = result;
        }

        private void Alignment2ToString(ComboBox comboBoxAlignment, string Alignment)
        {
            int result = 0;
            switch (Alignment)
            {
                case "Left":
                    result = 0;
                    break;
                case "Right":
                    result = 1;
                    break;
                case "Center":
                    result = 2;
                    break;

                default:
                    result = 0;
                    break;

            }
            //return result;
            comboBoxAlignment.SelectedIndex = result;
        }

        private string StringToAlignment2(int Alignment)
        {
            string result = "Left";
            switch (Alignment)
            {
                case 0:
                    result = "Left";
                    break;
                case 1:
                    result = "Right";
                    break;
                case 2:
                    result = "Center";
                    break;

                default:
                    result = "Left";
                    break;

            }
            return result;
        }

        private Color ColorRead(string color)
        {
            if(color.Length==18)  color = color.Remove(2, 8);
            Color old_color = ColorTranslator.FromHtml(color);
            Color new_color = Color.FromArgb(255, old_color.R, old_color.G, old_color.B);
            return new_color;
        }

        private Color ColorBackgroundRead(string color)
        {
            if (color.Length == 18) color = color.Remove(2, 8);
            string firstByteS = color.Substring(6, 2);
            string secondByteS = color.Substring(8, 2);
            int firstByte = Convert.ToInt32(firstByteS, 16);
            int secondByte = Convert.ToInt32(secondByteS, 16);

            int r = 0;
            int g = 0;
            int b = 0;

            r = (byte)((byte)((firstByte >> 3) & 0x1f) << 3);
            g = (byte)((byte)(((secondByte >> 5) & 0x7) | ((firstByte & 0x07) << 3)) << 2);
            b = (byte)((byte)(secondByte & 0x1f) << 3);

            Color new_color = Color.FromArgb(255, r, g, b);
            return new_color;
        }

        private string ColorBackgroundWrite(Color color)
        {
            int r = color.R;
            int g = color.G;
            int b = color.B;

            int temp_b = ((b >> 3) & 0x1f);
            int temp_g = (((g >> 2) & 0x7) << 5);
            int secondByte = (temp_b | temp_g);

            int temp_g2 = ((g >> 5) & 0x07);
            int temp_r = (((r >> 3) & 0x1f) << 3);
            int firstByte = (temp_g2 | temp_r);
            string firstByteS = firstByte.ToString("X2");
            string secondByteS = secondByte.ToString("X2");

            string new_color = "0xFFFF" + firstByteS + secondByteS;
            return new_color;
        }

        /// <summary>Преобразует цвет в координаты</summary>
        /// <param name="color">Цвет, кодирующий координаты</param>
        /// <param name="X">Координата X</param>
        /// <param name="Y">Координата Y</param>
        private void ColorToCoodinates(Color color, out int X, out int Y)
        {
            //string sColor = ColorTranslator.ToHtml(color);
            //string sColor = color.A.ToString("X") + color.R.ToString("X") + color.G.ToString("X") + color.B.ToString("X");
            string sColor = color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");

            //sColor = "X123456";
            //sColor = sColor.Remove(0, 1);
            int sColorLenght = sColor.Length;
            string colorX = sColor.Remove(3, sColorLenght - 3);
            string colorY = sColor.Remove(0, sColorLenght - 3);

            //int myInt = 2934;
            //string myHex = myInt.ToString("X");  // Gives you hexadecimal
            //int myNewInt = Convert.ToInt32(myHex, 16);  // Back to int again.

            X = Convert.ToInt32(colorX, 16);
            Y = Convert.ToInt32(colorY, 16);
            //numericUpDown_X.Value = X;
            //numericUpDown_Y.Value = Y;
        }

        /// <summary>Кодирование координат цветом</summary>
        /// <param name="X">Координата X</param>
        /// <param name="Y">Координата Y</param>
        private string CoodinatesToColor(int X, int Y)
        {
            string colorX = X.ToString("X3");
            string colorY = Y.ToString("X3");
            string color = "0xFF" + colorX + colorY;

            //int myInt = 2934;
            //string myHex = myInt.ToString("X");  // Gives you hexadecimal
            //int myNewInt = Convert.ToInt32(myHex, 16);  // Back to int again.
            
            return color;
        }

        private void comboBoxSetText(ComboBox comboBox, long value)
        {
            comboBox.Text = value.ToString();
            if (comboBox.SelectedIndex < 0) comboBox.Text = "";
        }

        // сбрасываем все настройки отображения
        private void checkBoxUseClear()
        {
            checkBox_Hour_Use.Checked = false;
            checkBox_Minute_Use.Checked = false;
            checkBox_Second_Use.Checked = false;
            checkBox_12h_Use.Checked = false;

            checkBox_Hour_hand_Use.Checked = false;
            checkBox_Minute_hand_Use.Checked = false;
            checkBox_Second_hand_Use.Checked = false;

            checkBox__Year_text_Use.Checked = false;
            checkBox_Month_Use.Checked = false;
            checkBox_Month_pictures_Use.Checked = false;
            checkBox_Month_hand_Use.Checked = false;
            checkBox_Day_Use.Checked = false;
            checkBox_Day_hand_Use.Checked = false;
            checkBox_DOW_pictures_Use.Checked = false;
            checkBox_DOW_hand_Use.Checked = false;

            checkBox_Bluetooth_Use.Checked = false;
            checkBox_Lock_Use.Checked = false;
            checkBox_Alarm_Use.Checked = false;
            checkBox_DND_Use.Checked = false;

            checkBox_Battery_pictures_Use.Checked = false;
            checkBox_Battery_Use.Checked = false;
            checkBox_Battery_hand_Use.Checked = false;
            checkBox_Battery_scaleCircle_Use.Checked = false;
            checkBox_Battery_scaleLinear_Use.Checked = false;

            checkBox_Steps_pictures_Use.Checked = false;
            checkBox_Steps_Use.Checked = false;
            checkBox_Steps_hand_Use.Checked = false;
            checkBox_Steps_scaleCircle_Use.Checked = false;
            checkBox_Steps_scaleLinear_Use.Checked = false;

            checkBox_Calories_pictures_Use.Checked = false;
            checkBox_Calories_Use.Checked = false;
            checkBox_Calories_hand_Use.Checked = false;
            checkBox_Calories_scaleCircle_Use.Checked = false;
            checkBox_Calories_scaleLinear_Use.Checked = false;

            checkBox_HeartRate_pictures_Use.Checked = false;
            checkBox_HeartRate_Use.Checked = false;
            checkBox_HeartRate_hand_Use.Checked = false;
            checkBox_HeartRate_scaleCircle_Use.Checked = false;
            checkBox_HeartRate_scaleLinear_Use.Checked = false;

            checkBox_PAI_pictures_Use.Checked = false;
            checkBox_PAI_Use.Checked = false;
            checkBox_PAI_hand_Use.Checked = false;
            checkBox_PAI_scaleCircle_Use.Checked = false;
            checkBox_PAI_scaleLinear_Use.Checked = false;

            checkBox_Distance_pictures_Use.Checked = false;
            checkBox_Distance_Use.Checked = false;
            checkBox_Distance_hand_Use.Checked = false;
            checkBox_Distance_scaleCircle_Use.Checked = false;
            checkBox_Distance_scaleLinear_Use.Checked = false;



            checkBox_Weather_pictures_Use.Checked = false;
            checkBox_Weather_Use.Checked = false;
            checkBox_Weather_UseMin.Checked = false;
            checkBox_Weather_UseMax.Checked = false;
            checkBox_Weather_hand_Use.Checked = false;
            checkBox_Weather_scaleCircle_Use.Checked = false;
            checkBox_Weather_scaleLinear_Use.Checked = false;
            //TODO добавить отключение чекбоксов для активностей


        }
        private void SettingsClear()
        {
            comboBox_Background_image.Items.Clear();
            comboBox_Background_image.Text = "";
            comboBox_Preview_image.Items.Clear();
            comboBox_Preview_image.Text = "";

            comboBox_AM_image.Items.Clear();
            comboBox_AM_image.Text = "";
            comboBox_PM_image.Items.Clear();
            comboBox_PM_image.Text = "";

            comboBox_Hour_image.Items.Clear();
            comboBox_Hour_image.Text = "";
            comboBox_Hour_unit.Items.Clear();
            comboBox_Hour_unit.Text = "";
            comboBox_Hour_separator.Items.Clear();
            comboBox_Hour_separator.Text = "";
            //comboBox_Hour_error.Items.Clear();
            //comboBox_Hour_error.Text = "";

            comboBox_Minute_image.Items.Clear();
            comboBox_Minute_image.Text = "";
            comboBox_Minute_unit.Items.Clear();
            comboBox_Minute_unit.Text = "";
            comboBox_Minute_separator.Items.Clear();
            comboBox_Minute_separator.Text = "";
            //comboBox_Minute_error.Items.Clear();
            //comboBox_Minute_error.Text = "";

            comboBox_Second_image.Items.Clear();
            comboBox_Second_image.Text = "";
            comboBox_Second_unit.Items.Clear();
            comboBox_Second_unit.Text = "";
            comboBox_Second_separator.Items.Clear();
            comboBox_Second_separator.Text = "";
            //comboBox_Second_error.Items.Clear();
            //comboBox_Second_error.Text = "";

            comboBox_Hour_hand_image.Items.Clear();
            comboBox_Hour_hand_image.Text = "";
            comboBox_Hour_hand_imageCentr.Items.Clear();
            comboBox_Hour_hand_imageCentr.Text = "";

            comboBox_Minute_hand_image.Items.Clear();
            comboBox_Minute_hand_image.Text = "";
            comboBox_Minute_hand_imageCentr.Items.Clear();
            comboBox_Minute_hand_imageCentr.Text = "";

            comboBox_Second_hand_image.Items.Clear();
            comboBox_Second_hand_image.Text = "";
            comboBox_Second_hand_imageCentr.Items.Clear();
            comboBox_Second_hand_imageCentr.Text = "";

            comboBox_Day_image.Items.Clear();
            comboBox_Day_image.Text = "";
            comboBox_Day_unit.Items.Clear();
            comboBox_Day_unit.Text = "";
            comboBox_Day_separator.Items.Clear();
            comboBox_Day_separator.Text = "";
            comboBox_Day_hand_image.Items.Clear();
            comboBox_Day_hand_image.Text = "";
            comboBox_Day_hand_imageCentr.Items.Clear();
            comboBox_Day_hand_imageCentr.Text = "";
            comboBox_Day_hand_imageBackground.Items.Clear();
            comboBox_Day_hand_imageBackground.Text = "";

            comboBox_Month_image.Items.Clear();
            comboBox_Month_image.Text = "";
            comboBox_Month_unit.Items.Clear();
            comboBox_Month_unit.Text = "";
            comboBox_Month_separator.Items.Clear();
            comboBox_Month_separator.Text = "";

            comboBox_Month_pictures_image.Items.Clear();
            comboBox_Month_pictures_image.Text = "";
            comboBox_Month_hand_image.Items.Clear();
            comboBox_Month_hand_image.Text = "";
            comboBox_Month_hand_imageCentr.Items.Clear();
            comboBox_Month_hand_imageCentr.Text = "";
            comboBox_Month_hand_imageBackground.Items.Clear();
            comboBox_Month_hand_imageBackground.Text = "";

            comboBox_Year_image.Items.Clear();
            comboBox_Year_image.Text = "";
            comboBox_Year_unit.Items.Clear();
            comboBox_Year_unit.Text = "";
            comboBox_Year_separator.Items.Clear();
            comboBox_Year_separator.Text = "";

            comboBox_DOW_pictures_image.Items.Clear();
            comboBox_DOW_pictures_image.Text = "";
            comboBox_DOW_hand_image.Items.Clear();
            comboBox_DOW_hand_image.Text = "";
            comboBox_DOW_hand_imageCentr.Items.Clear();
            comboBox_DOW_hand_imageCentr.Text = "";
            comboBox_DOW_hand_imageBackground.Items.Clear();
            comboBox_DOW_hand_imageBackground.Text = "";


            comboBox_Bluetooth_image.Items.Clear();
            comboBox_Bluetooth_image.Text = "";
            comboBox_Alarm_image.Items.Clear();
            comboBox_Alarm_image.Text = "";
            comboBox_DND_image.Items.Clear();
            comboBox_DND_image.Text = "";
            comboBox_Lock_image.Items.Clear();
            comboBox_Lock_image.Text = "";

            comboBox_Battery_pictures_image.Items.Clear();
            comboBox_Battery_pictures_image.Text = "";

            comboBox_Battery_image.Items.Clear();
            comboBox_Battery_image.Text = "";
            comboBox_Battery_icon.Items.Clear();
            comboBox_Battery_icon.Text = "";
            comboBox_Battery_unit.Items.Clear();
            comboBox_Battery_unit.Text = "";
            comboBox_Battery_imageError.Items.Clear();
            comboBox_Battery_imageError.Text = "";

            comboBox_Battery_hand_image.Items.Clear();
            comboBox_Battery_hand_image.Text = "";
            comboBox_Battery_hand_imageCentr.Items.Clear();
            comboBox_Battery_hand_imageCentr.Text = "";
            comboBox_Battery_hand_imageBackground.Items.Clear();
            comboBox_Battery_hand_imageBackground.Text = "";

            comboBox_Battery_scaleCircle_image.Items.Clear();
            comboBox_Battery_scaleCircle_image.Text = "";
            comboBox_Battery_scaleCircle_image_background.Items.Clear();
            comboBox_Battery_scaleCircle_image_background.Text = "";

            comboBox_Battery_scaleLinear_image.Items.Clear();
            comboBox_Battery_scaleLinear_image.Text = "";
            comboBox_Battery_scaleLinear_image_pointer.Items.Clear();
            comboBox_Battery_scaleLinear_image_pointer.Text = "";
            comboBox_Battery_scaleLinear_image_background.Items.Clear();
            comboBox_Battery_scaleLinear_image_background.Text = "";

            comboBox_Steps_pictures_image.Items.Clear();
            comboBox_Steps_pictures_image.Text = "";

            comboBox_Steps_image.Items.Clear();
            comboBox_Steps_image.Text = "";
            comboBox_Steps_icon.Items.Clear();
            comboBox_Steps_icon.Text = "";
            comboBox_Steps_unit.Items.Clear();
            comboBox_Steps_unit.Text = "";
            comboBox_Steps_imageError.Items.Clear();
            comboBox_Steps_imageError.Text = "";

            comboBox_Steps_hand_image.Items.Clear();
            comboBox_Steps_hand_image.Text = "";
            comboBox_Steps_hand_imageCentr.Items.Clear();
            comboBox_Steps_hand_imageCentr.Text = "";
            comboBox_Steps_hand_imageBackground.Items.Clear();
            comboBox_Steps_hand_imageBackground.Text = "";

            comboBox_Steps_scaleCircle_image.Items.Clear();
            comboBox_Steps_scaleCircle_image.Text = "";
            comboBox_Steps_scaleCircle_image_background.Items.Clear();
            comboBox_Steps_scaleCircle_image_background.Text = "";

            comboBox_Steps_scaleLinear_image.Items.Clear();
            comboBox_Steps_scaleLinear_image.Text = "";
            comboBox_Steps_scaleLinear_image_pointer.Items.Clear();
            comboBox_Steps_scaleLinear_image_pointer.Text = "";
            comboBox_Steps_scaleLinear_image_background.Items.Clear();
            comboBox_Steps_scaleLinear_image_background.Text = "";

            comboBox_Calories_pictures_image.Items.Clear();
            comboBox_Calories_pictures_image.Text = "";

            comboBox_Calories_image.Items.Clear();
            comboBox_Calories_image.Text = "";
            comboBox_Calories_icon.Items.Clear();
            comboBox_Calories_icon.Text = "";
            comboBox_Calories_unit.Items.Clear();
            comboBox_Calories_unit.Text = "";
            comboBox_Calories_imageError.Items.Clear();
            comboBox_Calories_imageError.Text = "";

            comboBox_Calories_hand_image.Items.Clear();
            comboBox_Calories_hand_image.Text = "";
            comboBox_Calories_hand_imageCentr.Items.Clear();
            comboBox_Calories_hand_imageCentr.Text = "";
            comboBox_Calories_hand_imageBackground.Items.Clear();
            comboBox_Calories_hand_imageBackground.Text = "";

            comboBox_Calories_scaleCircle_image.Items.Clear();
            comboBox_Calories_scaleCircle_image.Text = "";
            comboBox_Calories_scaleCircle_image_background.Items.Clear();
            comboBox_Calories_scaleCircle_image_background.Text = "";

            comboBox_Calories_scaleLinear_image.Items.Clear();
            comboBox_Calories_scaleLinear_image.Text = "";
            comboBox_Calories_scaleLinear_image_pointer.Items.Clear();
            comboBox_Calories_scaleLinear_image_pointer.Text = "";
            comboBox_Calories_scaleLinear_image_background.Items.Clear();
            comboBox_Calories_scaleLinear_image_background.Text = "";

            comboBox_HeartRate_pictures_image.Items.Clear();
            comboBox_HeartRate_pictures_image.Text = "";

            comboBox_HeartRate_image.Items.Clear();
            comboBox_HeartRate_image.Text = "";
            comboBox_HeartRate_icon.Items.Clear();
            comboBox_HeartRate_icon.Text = "";
            comboBox_HeartRate_unit.Items.Clear();
            comboBox_HeartRate_unit.Text = "";
            comboBox_HeartRate_imageError.Items.Clear();
            comboBox_HeartRate_imageError.Text = "";

            comboBox_HeartRate_hand_image.Items.Clear();
            comboBox_HeartRate_hand_image.Text = "";
            comboBox_HeartRate_hand_imageCentr.Items.Clear();
            comboBox_HeartRate_hand_imageCentr.Text = "";
            comboBox_HeartRate_hand_imageBackground.Items.Clear();
            comboBox_HeartRate_hand_imageBackground.Text = "";

            comboBox_HeartRate_scaleCircle_image.Items.Clear();
            comboBox_HeartRate_scaleCircle_image.Text = "";
            comboBox_HeartRate_scaleCircle_image_background.Items.Clear();
            comboBox_HeartRate_scaleCircle_image_background.Text = "";

            comboBox_HeartRate_scaleLinear_image.Items.Clear();
            comboBox_HeartRate_scaleLinear_image.Text = "";
            comboBox_HeartRate_scaleLinear_image_pointer.Items.Clear();
            comboBox_HeartRate_scaleLinear_image_pointer.Text = "";
            comboBox_HeartRate_scaleLinear_image_background.Items.Clear();
            comboBox_HeartRate_scaleLinear_image_background.Text = "";

            comboBox_PAI_pictures_image.Items.Clear();
            comboBox_PAI_pictures_image.Text = "";

            comboBox_PAI_image.Items.Clear();
            comboBox_PAI_image.Text = "";
            comboBox_PAI_icon.Items.Clear();
            comboBox_PAI_icon.Text = "";
            comboBox_PAI_unit.Items.Clear();
            comboBox_PAI_unit.Text = "";
            comboBox_PAI_imageError.Items.Clear();
            comboBox_PAI_imageError.Text = "";

            comboBox_PAI_hand_image.Items.Clear();
            comboBox_PAI_hand_image.Text = "";
            comboBox_PAI_hand_imageCentr.Items.Clear();
            comboBox_PAI_hand_imageCentr.Text = "";
            comboBox_PAI_hand_imageBackground.Items.Clear();
            comboBox_PAI_hand_imageBackground.Text = "";

            comboBox_PAI_scaleCircle_image.Items.Clear();
            comboBox_PAI_scaleCircle_image.Text = "";
            comboBox_PAI_scaleCircle_image_background.Items.Clear();
            comboBox_PAI_scaleCircle_image_background.Text = "";

            comboBox_PAI_scaleLinear_image.Items.Clear();
            comboBox_PAI_scaleLinear_image.Text = "";
            comboBox_PAI_scaleLinear_image_pointer.Items.Clear();
            comboBox_PAI_scaleLinear_image_pointer.Text = "";
            comboBox_PAI_scaleLinear_image_background.Items.Clear();
            comboBox_PAI_scaleLinear_image_background.Text = "";

            comboBox_Distance_pictures_image.Items.Clear();
            comboBox_Distance_pictures_image.Text = "";

            comboBox_Distance_image.Items.Clear();
            comboBox_Distance_image.Text = "";
            comboBox_Distance_icon.Items.Clear();
            comboBox_Distance_icon.Text = "";
            comboBox_Distance_unit.Items.Clear();
            comboBox_Distance_unit.Text = "";
            comboBox_Distance_imageError.Items.Clear();
            comboBox_Distance_imageError.Text = "";
            comboBox_Distance_imageDecimalPoint.Items.Clear();
            comboBox_Distance_imageDecimalPoint.Text = "";

            comboBox_Distance_hand_image.Items.Clear();
            comboBox_Distance_hand_image.Text = "";
            comboBox_Distance_hand_imageCentr.Items.Clear();
            comboBox_Distance_hand_imageCentr.Text = "";
            comboBox_Distance_hand_imageBackground.Items.Clear();
            comboBox_Distance_hand_imageBackground.Text = "";

            comboBox_Distance_scaleCircle_image.Items.Clear();
            comboBox_Distance_scaleCircle_image.Text = "";
            comboBox_Distance_scaleCircle_image_background.Items.Clear();
            comboBox_Distance_scaleCircle_image_background.Text = "";

            comboBox_Distance_scaleLinear_image.Items.Clear();
            comboBox_Distance_scaleLinear_image.Text = "";
            comboBox_Distance_scaleLinear_image_pointer.Items.Clear();
            comboBox_Distance_scaleLinear_image_pointer.Text = "";
            comboBox_Distance_scaleLinear_image_background.Items.Clear();
            comboBox_Distance_scaleLinear_image_background.Text = "";

            comboBox_Weather_pictures_image.Items.Clear();
            comboBox_Weather_pictures_image.Text = "";

            comboBox_Weather_image.Items.Clear();
            comboBox_Weather_image.Text = "";
            comboBox_Weather_icon.Items.Clear();
            comboBox_Weather_icon.Text = "";
            comboBox_Weather_unitF.Items.Clear();
            comboBox_Weather_unitF.Text = "";
            comboBox_Weather_imageError.Items.Clear();
            comboBox_Weather_imageError.Text = "";
            comboBox_Weather_imageMinus.Items.Clear();
            comboBox_Weather_imageMinus.Text = "";

            comboBox_Weather_hand_image.Items.Clear();
            comboBox_Weather_hand_image.Text = "";
            comboBox_Weather_hand_imageCentr.Items.Clear();
            comboBox_Weather_hand_imageCentr.Text = "";
            comboBox_Weather_hand_imageBackground.Items.Clear();
            comboBox_Weather_hand_imageBackground.Text = "";

            comboBox_Weather_scaleCircle_image.Items.Clear();
            comboBox_Weather_scaleCircle_image.Text = "";
            comboBox_Weather_scaleCircle_image_background.Items.Clear();
            comboBox_Weather_scaleCircle_image_background.Text = "";

            comboBox_Weather_scaleLinear_image.Items.Clear();
            comboBox_Weather_scaleLinear_image.Text = "";
            comboBox_Weather_scaleLinear_image_pointer.Items.Clear();
            comboBox_Weather_scaleLinear_image_pointer.Text = "";
            comboBox_Weather_scaleLinear_image_background.Items.Clear();
            comboBox_Weather_scaleLinear_image_background.Text = "";

            comboBox_Weather_imageMax.Items.Clear();
            comboBox_Weather_imageMax.Text = "";
            comboBox_Weather_iconMax.Items.Clear();
            comboBox_Weather_iconMax.Text = "";
            comboBox_Weather_unitFMax.Items.Clear();
            comboBox_Weather_unitFMax.Text = "";
            comboBox_Weather_imageErrorMax.Items.Clear();
            comboBox_Weather_imageErrorMax.Text = "";
            comboBox_Weather_imageMinusMax.Items.Clear();
            comboBox_Weather_imageMinusMax.Text = "";

            comboBox_Weather_imageMin.Items.Clear();
            comboBox_Weather_imageMin.Text = "";
            comboBox_Weather_iconMin.Items.Clear();
            comboBox_Weather_iconMin.Text = "";
            comboBox_Weather_unitFMin.Items.Clear();
            comboBox_Weather_unitFMin.Text = "";
            comboBox_Weather_imageErrorMin.Items.Clear();
            comboBox_Weather_imageErrorMin.Text = "";
            comboBox_Weather_imageMinusMin.Items.Clear();
            comboBox_Weather_imageMinusMin.Text = "";

            // ********************
            //comboBox_Background.Items.Clear();
            //comboBox_Background.Text = "";
            //comboBox_Preview.Items.Clear();
            //comboBox_Preview.Text = "";

            //comboBox_Hours_Tens_Image.Text = "";
            //comboBox_Hours_Tens_Image.Items.Clear();
            //comboBox_Hours_Ones_Image.Text = "";
            //comboBox_Hours_Ones_Image.Items.Clear();

            //comboBox_Min_Tens_Image.Text = "";
            //comboBox_Min_Tens_Image.Items.Clear();
            //comboBox_Min_Ones_Image.Text = "";
            //comboBox_Min_Ones_Image.Items.Clear();

            //comboBox_Sec_Tens_Image.Text = "";
            //comboBox_Sec_Tens_Image.Items.Clear();
            //comboBox_Sec_Ones_Image.Text = "";
            //comboBox_Sec_Ones_Image.Items.Clear();

            //comboBox_Image_Am.Text = "";
            //comboBox_Image_Am.Items.Clear();
            //comboBox_Image_Pm.Text = "";
            //comboBox_Image_Pm.Items.Clear();
            //comboBox_Delimiter_Image.Text = "";
            //comboBox_Delimiter_Image.Items.Clear();


            //comboBox_WeekDay_Image.Text = "";
            //comboBox_WeekDay_Image.Items.Clear();
            //comboBox_DOW_IconSet_Image.Text = "";
            //comboBox_DOW_IconSet_Image.Items.Clear();
            //comboBox_OneLine_Delimiter.Text = "";
            //comboBox_OneLine_Delimiter.Items.Clear();
            //comboBox_OneLine_Image.Text = "";
            //comboBox_OneLine_Image.Items.Clear();
            //comboBox_MonthName_Image.Text = "";
            //comboBox_MonthName_Image.Items.Clear();
            //comboBox_MonthAndDayM_Image.Text = "";
            //comboBox_MonthAndDayM_Image.Items.Clear();
            //comboBox_MonthAndDayD_Image.Text = "";
            //comboBox_MonthAndDayD_Image.Items.Clear();
            //comboBox_Year_Image.Text = "";
            //comboBox_Year_Image.Items.Clear();
            //comboBox_Year_Delimiter.Text = "";
            //comboBox_Year_Delimiter.Items.Clear();

            //comboBox_ADDay_ClockHand_Image.Text = "";
            //comboBox_ADDay_ClockHand_Image.Items.Clear();
            //comboBox_ADWeekDay_ClockHand_Image.Text = "";
            //comboBox_ADWeekDay_ClockHand_Image.Items.Clear();
            //comboBox_ADMonth_ClockHand_Image.Text = "";
            //comboBox_ADMonth_ClockHand_Image.Items.Clear();

            //comboBox_StProg_ClockHand_Image.Text = "";
            //comboBox_StProg_ClockHand_Image.Items.Clear();
            //comboBox_SPSliced_Image.Text = "";
            //comboBox_SPSliced_Image.Items.Clear();
            //comboBox_StepsProgress_Image.Text = "";
            //comboBox_StepsProgress_Image.Items.Clear();

            //comboBox_ActivityCaloriesScale_Image.Text = "";
            //comboBox_ActivityCaloriesScale_Image.Items.Clear();
            //comboBox_ActivitySteps_Image.Text = "";
            //comboBox_ActivitySteps_Image.Items.Clear();
            //comboBox_ActivityStepsGoal_Image.Text = "";
            //comboBox_ActivityStepsGoal_Image.Items.Clear();
            //comboBox_ActivityDistance_Image.Text = "";
            //comboBox_ActivityDistance_Image.Items.Clear();
            //comboBox_ActivityDistance_Decimal.Text = "";
            //comboBox_ActivityDistance_Decimal.Items.Clear();
            //comboBox_ActivityDistance_Suffix_km.Text = "";
            //comboBox_ActivityDistance_Suffix_km.Items.Clear();
            //comboBox_ActivityDistance_Suffix_ml.Text = "";
            //comboBox_ActivityDistance_Suffix_ml.Items.Clear();
            //comboBox_ActivityPuls_Image.Text = "";
            //comboBox_ActivityPuls_Image.Items.Clear();
            //comboBox_ActivityPulsScale_Image.Text = "";
            //comboBox_ActivityPulsScale_Image.Items.Clear();
            //comboBox_Pulse_ClockHand_Image.Text = "";
            //comboBox_Pulse_ClockHand_Image.Items.Clear();
            //comboBox_ActivityPuls_IconSet_Image.Text = "";
            //comboBox_ActivityPuls_IconSet_Image.Items.Clear();
            //comboBox_ActivityCalories_Image.Text = "";
            //comboBox_ActivityCalories_Image.Items.Clear();
            //comboBox_Calories_ClockHand_Image.Text = "";
            //comboBox_Calories_ClockHand_Image.Items.Clear();
            //comboBox_ActivityStar_Image.Text = "";
            //comboBox_ActivityStar_Image.Items.Clear();
            //comboBox_Activity_NDImage.Text = "";
            //comboBox_Activity_NDImage.Items.Clear();

            //comboBox_Bluetooth_On.Text = "";
            //comboBox_Bluetooth_On.Items.Clear();
            //comboBox_Bluetooth_Off.Text = "";
            //comboBox_Bluetooth_Off.Items.Clear();
            //comboBox_Alarm_On.Text = "";
            //comboBox_Alarm_On.Items.Clear();
            //comboBox_Alarm_Off.Text = "";
            //comboBox_Alarm_Off.Items.Clear();
            //comboBox_Lock_On.Text = "";
            //comboBox_Lock_On.Items.Clear();
            //comboBox_Lock_Off.Text = "";
            //comboBox_Lock_Off.Items.Clear();
            //comboBox_DND_On.Text = "";
            //comboBox_DND_On.Items.Clear();
            //comboBox_DND_Off.Text = "";
            //comboBox_DND_Off.Items.Clear();


            //comboBox_Battery_Text_Image.Text = "";
            //comboBox_Battery_Text_Image.Items.Clear();
            //comboBox_Battery_Img_Image.Text = "";
            //comboBox_Battery_Img_Image.Items.Clear();
            //comboBox_Battery_Percent_Image.Text = "";
            //comboBox_Battery_Percent_Image.Items.Clear();
            //comboBox_Battery_ClockHand_Image.Text = "";
            //comboBox_Battery_ClockHand_Image.Items.Clear();
            //comboBox_Battery_IconSet_Image.Text = "";
            //comboBox_Battery_IconSet_Image.Items.Clear();
            //comboBox_Battery_Scale_Image.Text = "";
            //comboBox_Battery_Scale_Image.Items.Clear();

            //comboBox_AnalogClock_Hour_Image.Text = "";
            //comboBox_AnalogClock_Hour_Image.Items.Clear();
            //comboBox_AnalogClock_Min_Image.Text = "";
            //comboBox_AnalogClock_Min_Image.Items.Clear();
            //comboBox_AnalogClock_Sec_Image.Text = "";
            //comboBox_AnalogClock_Sec_Image.Items.Clear();

            //comboBox_HourCenterImage_Image.Text = "";
            //comboBox_HourCenterImage_Image.Items.Clear();
            //comboBox_MinCenterImage_Image.Text = "";
            //comboBox_MinCenterImage_Image.Items.Clear();
            //comboBox_SecCenterImage_Image.Text = "";
            //comboBox_SecCenterImage_Image.Items.Clear();

            //comboBox_StaticAnimation_Image.Text = "";
            //comboBox_StaticAnimation_Image.Items.Clear();
            //comboBox_MotiomAnimation_Image.Text = "";
            //comboBox_MotiomAnimation_Image.Items.Clear();

            //comboBox_Weather_Text_Image.Text = "";
            //comboBox_Weather_Text_Image.Items.Clear();
            //comboBox_Weather_Text_DegImage.Text = "";
            //comboBox_Weather_Text_DegImage.Items.Clear();
            //comboBox_Weather_Text_MinusImage.Text = "";
            //comboBox_Weather_Text_MinusImage.Items.Clear();
            //comboBox_Weather_Text_NDImage.Text = "";
            //comboBox_Weather_Text_NDImage.Items.Clear();
            //comboBox_Weather_Icon_Image.Text = "";
            //comboBox_Weather_Icon_Image.Items.Clear();
            //comboBox_Weather_Icon_NDImage.Text = "";
            //comboBox_Weather_Icon_NDImage.Items.Clear();
            //comboBox_Weather_Day_Image.Text = "";
            //comboBox_Weather_Day_Image.Items.Clear();
            //comboBox_Weather_Night_Image.Text = "";
            //comboBox_Weather_Night_Image.Items.Clear();

            //dataGridView_ActivityPuls_IconSet.Rows.Clear();
            //dataGridView_Battery_IconSet.Rows.Clear();
            //dataGridView_DOW_IconSet.Rows.Clear();
            //dataGridView_MotiomAnimation.Rows.Clear();
            //dataGridView_SPSliced.Rows.Clear();
        }

        private void ComboBoxAddItems()
        {
            comboBox_Background_image.Items.AddRange(ListImages.ToArray());
            comboBox_Preview_image.Items.AddRange(ListImages.ToArray());

            comboBox_AM_image.Items.AddRange(ListImages.ToArray());
            comboBox_PM_image.Items.AddRange(ListImages.ToArray());

            comboBox_Hour_image.Items.AddRange(ListImages.ToArray());
            comboBox_Hour_unit.Items.AddRange(ListImages.ToArray());
            comboBox_Hour_separator.Items.AddRange(ListImages.ToArray());
            //comboBox_Hour_error.Items.AddRange(ListImages.ToArray());

            comboBox_Minute_image.Items.AddRange(ListImages.ToArray());
            comboBox_Minute_unit.Items.AddRange(ListImages.ToArray());
            comboBox_Minute_separator.Items.AddRange(ListImages.ToArray());
            //comboBox_Minute_error.Items.AddRange(ListImages.ToArray());

            comboBox_Second_image.Items.AddRange(ListImages.ToArray());
            comboBox_Second_unit.Items.AddRange(ListImages.ToArray());
            comboBox_Second_separator.Items.AddRange(ListImages.ToArray());
            //comboBox_Second_error.Items.AddRange(ListImages.ToArray());

            comboBox_Hour_hand_image.Items.AddRange(ListImages.ToArray());
            comboBox_Hour_hand_imageCentr.Items.AddRange(ListImages.ToArray());

            comboBox_Minute_hand_image.Items.AddRange(ListImages.ToArray());
            comboBox_Minute_hand_imageCentr.Items.AddRange(ListImages.ToArray());

            comboBox_Second_hand_image.Items.AddRange(ListImages.ToArray());
            comboBox_Second_hand_imageCentr.Items.AddRange(ListImages.ToArray());

            comboBox_Day_image.Items.AddRange(ListImages.ToArray());
            comboBox_Day_unit.Items.AddRange(ListImages.ToArray());
            comboBox_Day_separator.Items.AddRange(ListImages.ToArray());

            comboBox_Day_hand_image.Items.AddRange(ListImages.ToArray());
            comboBox_Day_hand_imageCentr.Items.AddRange(ListImages.ToArray());
            comboBox_Day_hand_imageBackground.Items.AddRange(ListImages.ToArray());

            comboBox_Month_image.Items.AddRange(ListImages.ToArray());
            comboBox_Month_unit.Items.AddRange(ListImages.ToArray());
            comboBox_Month_separator.Items.AddRange(ListImages.ToArray());

            comboBox_Month_pictures_image.Items.AddRange(ListImages.ToArray());
            comboBox_Month_hand_image.Items.AddRange(ListImages.ToArray());
            comboBox_Month_hand_imageCentr.Items.AddRange(ListImages.ToArray());
            comboBox_Month_hand_imageBackground.Items.AddRange(ListImages.ToArray());

            comboBox_Year_image.Items.AddRange(ListImages.ToArray());
            comboBox_Year_unit.Items.AddRange(ListImages.ToArray());
            comboBox_Year_separator.Items.AddRange(ListImages.ToArray());

            comboBox_DOW_pictures_image.Items.AddRange(ListImages.ToArray());
            comboBox_DOW_hand_image.Items.AddRange(ListImages.ToArray());
            comboBox_DOW_hand_imageCentr.Items.AddRange(ListImages.ToArray());
            comboBox_DOW_hand_imageBackground.Items.AddRange(ListImages.ToArray());

            comboBox_Bluetooth_image.Items.AddRange(ListImages.ToArray());
            comboBox_Alarm_image.Items.AddRange(ListImages.ToArray());
            comboBox_DND_image.Items.AddRange(ListImages.ToArray());
            comboBox_Lock_image.Items.AddRange(ListImages.ToArray());

            comboBox_Battery_pictures_image.Items.AddRange(ListImages.ToArray());
            comboBox_Battery_image.Items.AddRange(ListImages.ToArray());
            comboBox_Battery_icon.Items.AddRange(ListImages.ToArray());
            comboBox_Battery_unit.Items.AddRange(ListImages.ToArray());
            comboBox_Battery_imageError.Items.AddRange(ListImages.ToArray());
            comboBox_Battery_hand_image.Items.AddRange(ListImages.ToArray());
            comboBox_Battery_hand_imageCentr.Items.AddRange(ListImages.ToArray());
            comboBox_Battery_hand_imageBackground.Items.AddRange(ListImages.ToArray());

            comboBox_Battery_scaleCircle_image.Items.AddRange(ListImages.ToArray());
            comboBox_Battery_scaleCircle_image_background.Items.AddRange(ListImages.ToArray());

            comboBox_Battery_scaleLinear_image.Items.AddRange(ListImages.ToArray());
            comboBox_Battery_scaleLinear_image_pointer.Items.AddRange(ListImages.ToArray());
            comboBox_Battery_scaleLinear_image_background.Items.AddRange(ListImages.ToArray());

            comboBox_Steps_pictures_image.Items.AddRange(ListImages.ToArray());
            comboBox_Steps_image.Items.AddRange(ListImages.ToArray());
            comboBox_Steps_icon.Items.AddRange(ListImages.ToArray());
            comboBox_Steps_unit.Items.AddRange(ListImages.ToArray());
            comboBox_Steps_imageError.Items.AddRange(ListImages.ToArray());
            comboBox_Steps_hand_image.Items.AddRange(ListImages.ToArray());
            comboBox_Steps_hand_imageCentr.Items.AddRange(ListImages.ToArray());
            comboBox_Steps_hand_imageBackground.Items.AddRange(ListImages.ToArray());

            comboBox_Steps_scaleCircle_image.Items.AddRange(ListImages.ToArray());
            comboBox_Steps_scaleCircle_image_background.Items.AddRange(ListImages.ToArray());

            comboBox_Steps_scaleLinear_image.Items.AddRange(ListImages.ToArray());
            comboBox_Steps_scaleLinear_image_pointer.Items.AddRange(ListImages.ToArray());
            comboBox_Steps_scaleLinear_image_background.Items.AddRange(ListImages.ToArray());

            comboBox_Calories_pictures_image.Items.AddRange(ListImages.ToArray());
            comboBox_Calories_image.Items.AddRange(ListImages.ToArray());
            comboBox_Calories_icon.Items.AddRange(ListImages.ToArray());
            comboBox_Calories_unit.Items.AddRange(ListImages.ToArray());
            comboBox_Calories_imageError.Items.AddRange(ListImages.ToArray());
            comboBox_Calories_hand_image.Items.AddRange(ListImages.ToArray());
            comboBox_Calories_hand_imageCentr.Items.AddRange(ListImages.ToArray());
            comboBox_Calories_hand_imageBackground.Items.AddRange(ListImages.ToArray());

            comboBox_Calories_scaleCircle_image.Items.AddRange(ListImages.ToArray());
            comboBox_Calories_scaleCircle_image_background.Items.AddRange(ListImages.ToArray());

            comboBox_Calories_scaleLinear_image.Items.AddRange(ListImages.ToArray());
            comboBox_Calories_scaleLinear_image_pointer.Items.AddRange(ListImages.ToArray());
            comboBox_Calories_scaleLinear_image_background.Items.AddRange(ListImages.ToArray());

            comboBox_HeartRate_pictures_image.Items.AddRange(ListImages.ToArray());
            comboBox_HeartRate_image.Items.AddRange(ListImages.ToArray());
            comboBox_HeartRate_icon.Items.AddRange(ListImages.ToArray());
            comboBox_HeartRate_unit.Items.AddRange(ListImages.ToArray());
            comboBox_HeartRate_imageError.Items.AddRange(ListImages.ToArray());
            comboBox_HeartRate_hand_image.Items.AddRange(ListImages.ToArray());
            comboBox_HeartRate_hand_imageCentr.Items.AddRange(ListImages.ToArray());
            comboBox_HeartRate_hand_imageBackground.Items.AddRange(ListImages.ToArray());

            comboBox_HeartRate_scaleCircle_image.Items.AddRange(ListImages.ToArray());
            comboBox_HeartRate_scaleCircle_image_background.Items.AddRange(ListImages.ToArray());

            comboBox_HeartRate_scaleLinear_image.Items.AddRange(ListImages.ToArray());
            comboBox_HeartRate_scaleLinear_image_pointer.Items.AddRange(ListImages.ToArray());
            comboBox_HeartRate_scaleLinear_image_background.Items.AddRange(ListImages.ToArray());

            comboBox_PAI_pictures_image.Items.AddRange(ListImages.ToArray());
            comboBox_PAI_image.Items.AddRange(ListImages.ToArray());
            comboBox_PAI_icon.Items.AddRange(ListImages.ToArray());
            comboBox_PAI_unit.Items.AddRange(ListImages.ToArray());
            comboBox_PAI_imageError.Items.AddRange(ListImages.ToArray());
            comboBox_PAI_hand_image.Items.AddRange(ListImages.ToArray());
            comboBox_PAI_hand_imageCentr.Items.AddRange(ListImages.ToArray());
            comboBox_PAI_hand_imageBackground.Items.AddRange(ListImages.ToArray());

            comboBox_PAI_scaleCircle_image.Items.AddRange(ListImages.ToArray());
            comboBox_PAI_scaleCircle_image_background.Items.AddRange(ListImages.ToArray());

            comboBox_PAI_scaleLinear_image.Items.AddRange(ListImages.ToArray());
            comboBox_PAI_scaleLinear_image_pointer.Items.AddRange(ListImages.ToArray());
            comboBox_PAI_scaleLinear_image_background.Items.AddRange(ListImages.ToArray());

            comboBox_Distance_pictures_image.Items.AddRange(ListImages.ToArray());
            comboBox_Distance_image.Items.AddRange(ListImages.ToArray());
            comboBox_Distance_icon.Items.AddRange(ListImages.ToArray());
            comboBox_Distance_unit.Items.AddRange(ListImages.ToArray());
            comboBox_Distance_imageError.Items.AddRange(ListImages.ToArray());
            comboBox_Distance_imageDecimalPoint.Items.AddRange(ListImages.ToArray());
            comboBox_Distance_hand_image.Items.AddRange(ListImages.ToArray());
            comboBox_Distance_hand_imageCentr.Items.AddRange(ListImages.ToArray());
            comboBox_Distance_hand_imageBackground.Items.AddRange(ListImages.ToArray());

            comboBox_Distance_scaleCircle_image.Items.AddRange(ListImages.ToArray());
            comboBox_Distance_scaleCircle_image_background.Items.AddRange(ListImages.ToArray());

            comboBox_Distance_scaleLinear_image.Items.AddRange(ListImages.ToArray());
            comboBox_Distance_scaleLinear_image_pointer.Items.AddRange(ListImages.ToArray());
            comboBox_Distance_scaleLinear_image_background.Items.AddRange(ListImages.ToArray());




            comboBox_Weather_pictures_image.Items.AddRange(ListImages.ToArray());
            comboBox_Weather_image.Items.AddRange(ListImages.ToArray());
            comboBox_Weather_icon.Items.AddRange(ListImages.ToArray());
            comboBox_Weather_unitF.Items.AddRange(ListImages.ToArray());
            comboBox_Weather_imageError.Items.AddRange(ListImages.ToArray());
            comboBox_Weather_imageMinus.Items.AddRange(ListImages.ToArray());
            comboBox_Weather_hand_image.Items.AddRange(ListImages.ToArray());
            comboBox_Weather_hand_imageCentr.Items.AddRange(ListImages.ToArray());
            comboBox_Weather_hand_imageBackground.Items.AddRange(ListImages.ToArray());

            comboBox_Weather_scaleCircle_image.Items.AddRange(ListImages.ToArray());
            comboBox_Weather_scaleCircle_image_background.Items.AddRange(ListImages.ToArray());

            comboBox_Weather_scaleLinear_image.Items.AddRange(ListImages.ToArray());
            comboBox_Weather_scaleLinear_image_pointer.Items.AddRange(ListImages.ToArray());
            comboBox_Weather_scaleLinear_image_background.Items.AddRange(ListImages.ToArray());

            comboBox_Weather_imageMax.Items.AddRange(ListImages.ToArray());
            comboBox_Weather_iconMax.Items.AddRange(ListImages.ToArray());
            comboBox_Weather_unitFMax.Items.AddRange(ListImages.ToArray());
            comboBox_Weather_imageErrorMax.Items.AddRange(ListImages.ToArray());
            comboBox_Weather_imageMinusMax.Items.AddRange(ListImages.ToArray());

            comboBox_Weather_imageMin.Items.AddRange(ListImages.ToArray());
            comboBox_Weather_iconMin.Items.AddRange(ListImages.ToArray());
            comboBox_Weather_unitFMin.Items.AddRange(ListImages.ToArray());
            comboBox_Weather_imageErrorMin.Items.AddRange(ListImages.ToArray());
            comboBox_Weather_imageMinusMin.Items.AddRange(ListImages.ToArray());


            // ******************

            //comboBox_Background.Items.AddRange(ListImages.ToArray());
            //comboBox_Preview.Items.AddRange(ListImages.ToArray());

            //comboBox_Hours_Tens_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_Hours_Ones_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_Min_Tens_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_Min_Ones_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_Sec_Tens_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_Sec_Ones_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_Delimiter_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_Image_Am.Items.AddRange(ListImages.ToArray());
            //comboBox_Image_Pm.Items.AddRange(ListImages.ToArray());

            //comboBox_WeekDay_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_DOW_IconSet_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_OneLine_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_OneLine_Delimiter.Items.AddRange(ListImages.ToArray());
            //comboBox_MonthAndDayD_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_MonthAndDayM_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_MonthName_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_Year_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_Year_Delimiter.Items.AddRange(ListImages.ToArray());
            //comboBox_ADDay_ClockHand_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_ADWeekDay_ClockHand_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_ADMonth_ClockHand_Image.Items.AddRange(ListImages.ToArray());

            //comboBox_StProg_ClockHand_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_SPSliced_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_StepsProgress_Image.Items.AddRange(ListImages.ToArray());

            //comboBox_ActivityCaloriesScale_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_ActivitySteps_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_ActivityStepsGoal_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_ActivityDistance_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_ActivityDistance_Decimal.Items.AddRange(ListImages.ToArray());
            //comboBox_ActivityDistance_Suffix_km.Items.AddRange(ListImages.ToArray());
            //comboBox_ActivityDistance_Suffix_ml.Items.AddRange(ListImages.ToArray());
            //comboBox_ActivityPulsScale_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_Pulse_ClockHand_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_ActivityPuls_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_ActivityPuls_IconSet_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_ActivityCalories_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_Calories_ClockHand_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_ActivityStar_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_Activity_NDImage.Items.AddRange(ListImages.ToArray());

            //comboBox_Bluetooth_On.Items.AddRange(ListImages.ToArray());
            //comboBox_Bluetooth_Off.Items.AddRange(ListImages.ToArray());
            //comboBox_Alarm_On.Items.AddRange(ListImages.ToArray());
            //comboBox_Alarm_Off.Items.AddRange(ListImages.ToArray());
            //comboBox_Lock_On.Items.AddRange(ListImages.ToArray());
            //comboBox_Lock_Off.Items.AddRange(ListImages.ToArray());
            //comboBox_DND_On.Items.AddRange(ListImages.ToArray());
            //comboBox_DND_Off.Items.AddRange(ListImages.ToArray());

            //comboBox_Battery_Text_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_Battery_Img_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_Battery_Percent_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_Battery_ClockHand_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_Battery_IconSet_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_Battery_Scale_Image.Items.AddRange(ListImages.ToArray());

            //comboBox_AnalogClock_Hour_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_AnalogClock_Min_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_AnalogClock_Sec_Image.Items.AddRange(ListImages.ToArray());

            //comboBox_HourCenterImage_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_MinCenterImage_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_SecCenterImage_Image.Items.AddRange(ListImages.ToArray());

            //comboBox_StaticAnimation_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_MotiomAnimation_Image.Items.AddRange(ListImages.ToArray());

            //comboBox_Weather_Text_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_Weather_Text_DegImage.Items.AddRange(ListImages.ToArray());
            //comboBox_Weather_Text_MinusImage.Items.AddRange(ListImages.ToArray());
            //comboBox_Weather_Text_NDImage.Items.AddRange(ListImages.ToArray());
            //comboBox_Weather_Icon_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_Weather_Icon_NDImage.Items.AddRange(ListImages.ToArray());
            //comboBox_Weather_Day_Image.Items.AddRange(ListImages.ToArray());
            //comboBox_Weather_Night_Image.Items.AddRange(ListImages.ToArray());

        }

        // устанавливаем тип циферблата исходя из DeviceId
        private void ReadDeviceId()
        {
            if (Watch_Face.Info != null)
            {
                switch (Watch_Face.Info.DeviceId)
                {
                    case 59:
                        radioButton_GTR2.Checked = true;
                        break;
                    case 64:
                        radioButton_GTR2.Checked = true;
                        break;
                    case 65:
                        radioButton_GTS2.Checked = true;
                        break;
                    case 52:
                    default:
                        return;
                }

                if (radioButton_GTR2.Checked)
                {
                    this.Text = "GTR 2 watch face editor";
                    //pictureBox_Preview.Height = 230;
                    //pictureBox_Preview.Width = 230;
                    pictureBox_Preview.Size = new Size((int)(230 * currentDPI), (int)(230 * currentDPI));
                    
                    textBox_unpack_command.Text = Program_Settings.unpack_command_GTR_2;

                    button_unpack.Enabled = true;
                    button_pack.Enabled = true;
                    button_zip.Enabled = true;
                }
                else if (radioButton_GTS2.Checked)
                {
                    this.Text = "GTS 2 watch face editor";
                    //pictureBox_Preview.Height = 224;
                    //pictureBox_Preview.Width = 177;
                    pictureBox_Preview.Size = new Size((int)(177 * currentDPI), (int)(224 * currentDPI));

                    textBox_unpack_command.Text = Program_Settings.unpack_command_GTS_2;

                    button_unpack.Enabled = false;
                    button_pack.Enabled = false;
                    button_zip.Enabled = false;
                }

                if ((formPreview != null) && (formPreview.Visible))
                {
                    Form_Preview.Model_Wath.model_gtr47 = radioButton_GTR2.Checked;
                    Form_Preview.Model_Wath.model_gts = radioButton_GTS2.Checked;
                }

                Program_Settings.Model_GTR47 = radioButton_GTR2.Checked;
                Program_Settings.Model_GTS = radioButton_GTS2.Checked;
                string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
                {
                    //DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
                System.IO.File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, 
                    System.Text.Encoding.UTF8);
            }
        }



    }
}
