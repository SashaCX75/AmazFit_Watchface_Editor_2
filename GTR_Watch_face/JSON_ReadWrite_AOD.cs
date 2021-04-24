using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmazFit_Watchface_2
{
    public partial class Form1 : Form
    {
        private void JSON_read_AOD()
        {
            if (Watch_Face == null || Watch_Face.ScreenIdle == null) return;

            #region Background
            if (Watch_Face.ScreenIdle.BackgroundImageIndex != null)
            {
                comboBoxSetText(comboBox_Background_image_AOD, (long)Watch_Face.ScreenIdle.BackgroundImageIndex);
            }
            #endregion

            #region цифровое время
            if (Watch_Face.ScreenIdle.DialFace != null && Watch_Face.ScreenIdle.DialFace.DigitalDialFace != null)
            {
                if (Watch_Face.ScreenIdle.DialFace.DigitalDialFace.Digits != null &&
                    Watch_Face.ScreenIdle.DialFace.DigitalDialFace.Digits.Count > 0)
                {
                    foreach (DigitalTimeDigit digitalTimeDigit in Watch_Face.ScreenIdle.DialFace.DigitalDialFace.Digits)
                    {
                        switch (digitalTimeDigit.TimeType)
                        {
                            case "Minute":
                                checkBox_Minute_Use_AOD.Checked = true;
                                if (digitalTimeDigit.CombingMode == "Single")
                                {
                                    checkBox_Minute_follow_AOD.Checked = false;
                                }
                                else
                                {
                                    checkBox_Minute_follow_AOD.Checked = true;
                                }

                                if (digitalTimeDigit.Digit != null)
                                {
                                    if (digitalTimeDigit.Digit.Image != null)
                                    {
                                        numericUpDown_MinuteX_AOD.Value = digitalTimeDigit.Digit.Image.X;
                                        numericUpDown_MinuteY_AOD.Value = digitalTimeDigit.Digit.Image.Y;
                                        foreach (MultilangImage multilangImage in digitalTimeDigit.Digit.Image.MultilangImage)
                                        {
                                            if (multilangImage.LangCode == "All")
                                                comboBoxSetText(comboBox_Minute_image_AOD, multilangImage.ImageSet.ImageIndex);
                                        }
                                        if (digitalTimeDigit.Digit.Image.MultilangImageUnit != null)
                                        {
                                            foreach (MultilangImage multilangImage in digitalTimeDigit.Digit.Image.MultilangImageUnit)
                                            {
                                                if (multilangImage.LangCode == "All")
                                                    comboBoxSetText(comboBox_Minute_separator_AOD, multilangImage.ImageSet.ImageIndex);
                                            }
                                        }
                                    }
                                    AlignmentToString(comboBox_Minute_alignment_AOD, digitalTimeDigit.Digit.Alignment);
                                    if (digitalTimeDigit.Digit.Spacing != null)
                                        numericUpDown_Minute_spacing_AOD.Value = (decimal)digitalTimeDigit.Digit.Spacing;
                                    checkBox_Minute_add_zero_AOD.Checked = digitalTimeDigit.Digit.PaddingZero;
                                }

                                if (digitalTimeDigit.Separator != null)
                                {
                                    comboBoxSetText(comboBox_Minute_unit_AOD, digitalTimeDigit.Separator.ImageIndex);
                                    numericUpDown_Minute_unitX_AOD.Value = digitalTimeDigit.Separator.Coordinates.X;
                                    numericUpDown_Minute_unitY_AOD.Value = digitalTimeDigit.Separator.Coordinates.Y;
                                }
                                break;

                            default:
                                checkBox_Hour_Use_AOD.Checked = true;

                                if (digitalTimeDigit.Digit != null)
                                {
                                    if (digitalTimeDigit.Digit.Image != null)
                                    {
                                        numericUpDown_HourX_AOD.Value = digitalTimeDigit.Digit.Image.X;
                                        numericUpDown_HourY_AOD.Value = digitalTimeDigit.Digit.Image.Y;
                                        foreach (MultilangImage multilangImage in digitalTimeDigit.Digit.Image.MultilangImage)
                                        {
                                            if (multilangImage.LangCode == "All")
                                                comboBoxSetText(comboBox_Hour_image_AOD, multilangImage.ImageSet.ImageIndex);
                                        }
                                        if (digitalTimeDigit.Digit.Image.MultilangImageUnit != null)
                                        {
                                            foreach (MultilangImage multilangImage in digitalTimeDigit.Digit.Image.MultilangImageUnit)
                                            {
                                                if (multilangImage.LangCode == "All")
                                                    comboBoxSetText(comboBox_Hour_separator_AOD, multilangImage.ImageSet.ImageIndex);
                                            }
                                        }
                                    }
                                    AlignmentToString(comboBox_Hour_alignment, digitalTimeDigit.Digit.Alignment);
                                    if (digitalTimeDigit.Digit.Spacing != null)
                                        numericUpDown_Hour_spacing_AOD.Value = (decimal)digitalTimeDigit.Digit.Spacing;
                                    checkBox_Hour_add_zero_AOD.Checked = digitalTimeDigit.Digit.PaddingZero;
                                }

                                if (digitalTimeDigit.Separator != null)
                                {
                                    comboBoxSetText(comboBox_Hour_unit_AOD, digitalTimeDigit.Separator.ImageIndex);
                                    numericUpDown_Hour_unitX_AOD.Value = digitalTimeDigit.Separator.Coordinates.X;
                                    numericUpDown_Hour_unitY_AOD.Value = digitalTimeDigit.Separator.Coordinates.Y;
                                }
                                break;
                        }


                    }
                }

                if (Watch_Face.ScreenIdle.DialFace.DigitalDialFace.AM != null &&
                    Watch_Face.ScreenIdle.DialFace.DigitalDialFace.AM.Coordinates != null)
                {
                    checkBox_12h_Use_AOD.Checked = true;
                    numericUpDown_AM_X_AOD.Value = Watch_Face.ScreenIdle.DialFace.DigitalDialFace.AM.Coordinates.X;
                    numericUpDown_AM_Y_AOD.Value = Watch_Face.ScreenIdle.DialFace.DigitalDialFace.AM.Coordinates.Y;
                    foreach (MultilangImage multilangImage in Watch_Face.ScreenIdle.DialFace.DigitalDialFace.AM.ImageSet)
                    {
                        if (multilangImage.LangCode != null || multilangImage.LangCode == "All")
                            comboBoxSetText(comboBox_AM_image_AOD, multilangImage.ImageSet.ImageIndex);
                    }
                }
                if (Watch_Face.ScreenIdle.DialFace.DigitalDialFace.PM != null &&
                    Watch_Face.ScreenIdle.DialFace.DigitalDialFace.PM.Coordinates != null)
                {
                    checkBox_12h_Use_AOD.Checked = true;
                    numericUpDown_PM_X_AOD.Value = Watch_Face.ScreenIdle.DialFace.DigitalDialFace.PM.Coordinates.X;
                    numericUpDown_PM_Y_AOD.Value = Watch_Face.ScreenIdle.DialFace.DigitalDialFace.PM.Coordinates.Y;
                    foreach (MultilangImage multilangImage in Watch_Face.ScreenIdle.DialFace.DigitalDialFace.PM.ImageSet)
                    {
                        if (multilangImage.LangCode != null || multilangImage.LangCode == "All")
                            comboBoxSetText(comboBox_PM_image_AOD, multilangImage.ImageSet.ImageIndex);
                    }
                }
            }
            #endregion

            #region аналоговое время
            if (Watch_Face.ScreenIdle.DialFace != null && Watch_Face.ScreenIdle.DialFace.AnalogDialFace != null)
            {
                // часы
                if (Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Hours != null)
                {
                    checkBox_Hour_hand_Use_AOD.Checked = true;
                    numericUpDown_Hour_handX_AOD.Value = Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Hours.X;
                    numericUpDown_Hour_handY_AOD.Value = Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Hours.Y;
                    if (Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Hours.Pointer != null)
                    {
                        comboBoxSetText(comboBox_Hour_hand_image_AOD, (long)Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Hours.Pointer.ImageIndex);
                        if (Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Hours.Pointer.Coordinates != null)
                        {
                            numericUpDown_Hour_handX_offset_AOD.Value = Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Hours.Pointer.Coordinates.X;
                            numericUpDown_Hour_handY_offset_AOD.Value = Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Hours.Pointer.Coordinates.Y;
                        }
                    }

                    if (Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Hours.Cover != null)
                    {
                        comboBoxSetText(comboBox_Hour_hand_imageCentr_AOD, (long)Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Hours.Cover.ImageIndex);
                        if (Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Hours.Pointer.Coordinates != null)
                        {
                            numericUpDown_Hour_handX_centr_AOD.Value = Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Hours.Cover.Coordinates.X;
                            numericUpDown_Hour_handY_centr_AOD.Value = Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Hours.Cover.Coordinates.Y;
                        }
                    }
                }

                // минуты
                if (Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Minutes != null)
                {
                    checkBox_Minute_hand_Use_AOD.Checked = true;
                    numericUpDown_Minute_handX_AOD.Value = Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Minutes.X;
                    numericUpDown_Minute_handY_AOD.Value = Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Minutes.Y;
                    if (Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Minutes.Pointer != null)
                    {
                        comboBoxSetText(comboBox_Minute_hand_image_AOD, (long)Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Minutes.Pointer.ImageIndex);
                        if (Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Minutes.Pointer.Coordinates != null)
                        {
                            numericUpDown_Minute_handX_offset_AOD.Value = Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Minutes.Pointer.Coordinates.X;
                            numericUpDown_Minute_handY_offset_AOD.Value = Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Minutes.Pointer.Coordinates.Y;
                        }
                    }

                    if (Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Minutes.Cover != null)
                    {
                        comboBoxSetText(comboBox_Minute_hand_imageCentr_AOD, (long)Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Minutes.Cover.ImageIndex);
                        if (Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Minutes.Pointer.Coordinates != null)
                        {
                            numericUpDown_Minute_handX_centr_AOD.Value = Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Minutes.Cover.Coordinates.X;
                            numericUpDown_Minute_handY_centr_AOD.Value = Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Minutes.Cover.Coordinates.Y;
                        }
                    }
                }
            }
            #endregion

            #region дата
            if (Watch_Face.ScreenIdle != null && Watch_Face.ScreenIdle.Date != null)
            {

                if (Watch_Face.ScreenIdle.Date.DateDigits != null &&
                    Watch_Face.ScreenIdle.Date.DateDigits.Count > 0)
                {
                    foreach (DigitalDateDigit digitalDateDigit in Watch_Face.ScreenIdle.Date.DateDigits)
                    {
                        switch (digitalDateDigit.DateType)
                        {
                            case "Day":
                                checkBox_Day_Use_AOD.Checked = true;
                                if (digitalDateDigit.CombingMode == "Single")
                                {
                                    checkBox_Day_follow_AOD.Checked = false;
                                }
                                else
                                {
                                    checkBox_Day_follow_AOD.Checked = true;
                                }

                                if (digitalDateDigit.Digit != null)
                                {
                                    if (digitalDateDigit.Digit.Image != null)
                                    {
                                        numericUpDown_DayX_AOD.Value = digitalDateDigit.Digit.Image.X;
                                        numericUpDown_DayY_AOD.Value = digitalDateDigit.Digit.Image.Y;
                                        foreach (MultilangImage multilangImage in digitalDateDigit.Digit.Image.MultilangImage)
                                        {
                                            if (multilangImage.LangCode == "All")
                                                comboBoxSetText(comboBox_Day_image_AOD, multilangImage.ImageSet.ImageIndex);
                                        }
                                        if (digitalDateDigit.Digit.Image.MultilangImageUnit != null)
                                        {
                                            foreach (MultilangImage multilangImage in digitalDateDigit.Digit.Image.MultilangImageUnit)
                                            {
                                                if (multilangImage.LangCode == "All")
                                                    comboBoxSetText(comboBox_Day_separator_AOD, multilangImage.ImageSet.ImageIndex);
                                            }
                                        }
                                    }
                                    AlignmentToString(comboBox_Day_alignment, digitalDateDigit.Digit.Alignment);
                                    if (digitalDateDigit.Digit.Spacing != null)
                                        numericUpDown_Day_spacing_AOD.Value = (decimal)digitalDateDigit.Digit.Spacing;
                                    checkBox_Day_add_zero_AOD.Checked = digitalDateDigit.Digit.PaddingZero;
                                }

                                if (digitalDateDigit.Separator != null)
                                {
                                    comboBoxSetText(comboBox_Day_unit_AOD, digitalDateDigit.Separator.ImageIndex);
                                    numericUpDown_Day_unitX_AOD.Value = digitalDateDigit.Separator.Coordinates.X;
                                    numericUpDown_Day_unitY_AOD.Value = digitalDateDigit.Separator.Coordinates.Y;
                                }
                                break;

                           case "Month":
                                if (digitalDateDigit.Digit != null && digitalDateDigit.Digit.DisplayFormAnalog)
                                {
                                    checkBox_Month_pictures_Use_AOD.Checked = true;
                                    if (digitalDateDigit.Digit != null)
                                    {
                                        if (digitalDateDigit.Digit.Image != null)
                                        {
                                            numericUpDown_Month_picturesX_AOD.Value = digitalDateDigit.Digit.Image.X;
                                            numericUpDown_Month_picturesY_AOD.Value = digitalDateDigit.Digit.Image.Y;
                                            foreach (MultilangImage multilangImage in digitalDateDigit.Digit.Image.MultilangImage)
                                            {
                                                if (multilangImage.LangCode == "All")
                                                    comboBoxSetText(comboBox_Month_pictures_image_AOD, multilangImage.ImageSet.ImageIndex);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    checkBox_Month_Use_AOD.Checked = true;
                                    if (digitalDateDigit.CombingMode == "Single")
                                    {
                                        checkBox_Month_follow_AOD.Checked = false;
                                    }
                                    else
                                    {
                                        checkBox_Month_follow_AOD.Checked = true;
                                    }

                                    if (digitalDateDigit.Digit != null)
                                    {
                                        if (digitalDateDigit.Digit.Image != null)
                                        {
                                            numericUpDown_MonthX_AOD.Value = digitalDateDigit.Digit.Image.X;
                                            numericUpDown_MonthY_AOD.Value = digitalDateDigit.Digit.Image.Y;
                                            foreach (MultilangImage multilangImage in digitalDateDigit.Digit.Image.MultilangImage)
                                            {
                                                if (multilangImage.LangCode == "All")
                                                    comboBoxSetText(comboBox_Month_image_AOD, multilangImage.ImageSet.ImageIndex);
                                            }
                                            if (digitalDateDigit.Digit.Image.MultilangImageUnit != null)
                                            {
                                                foreach (MultilangImage multilangImage in digitalDateDigit.Digit.Image.MultilangImageUnit)
                                                {
                                                    if (multilangImage.LangCode == "All")
                                                        comboBoxSetText(comboBox_Month_separator_AOD, multilangImage.ImageSet.ImageIndex);
                                                }
                                            }
                                        }
                                        AlignmentToString(comboBox_Month_alignment, digitalDateDigit.Digit.Alignment);
                                        if (digitalDateDigit.Digit.Spacing != null)
                                            numericUpDown_Month_spacing_AOD.Value = (decimal)digitalDateDigit.Digit.Spacing;
                                        checkBox_Month_add_zero_AOD.Checked = digitalDateDigit.Digit.PaddingZero;
                                    }

                                    if (digitalDateDigit.Separator != null)
                                    {
                                        comboBoxSetText(comboBox_Month_unit_AOD, digitalDateDigit.Separator.ImageIndex);
                                        numericUpDown_Month_unitX_AOD.Value = digitalDateDigit.Separator.Coordinates.X;
                                        numericUpDown_Month_unitY_AOD.Value = digitalDateDigit.Separator.Coordinates.Y;
                                    }
                                }
                                break;

                            default:
                                checkBox_Year_text_Use_AOD.Checked = true;

                                if (digitalDateDigit.Digit != null)
                                {
                                    if (digitalDateDigit.Digit.Image != null)
                                    {
                                        numericUpDown_YearX_AOD.Value = digitalDateDigit.Digit.Image.X;
                                        numericUpDown_YearY_AOD.Value = digitalDateDigit.Digit.Image.Y;
                                        foreach (MultilangImage multilangImage in digitalDateDigit.Digit.Image.MultilangImage)
                                        {
                                            if (multilangImage.LangCode == "All")
                                                comboBoxSetText(comboBox_Year_image_AOD, multilangImage.ImageSet.ImageIndex);
                                        }
                                        if (digitalDateDigit.Digit.Image.MultilangImageUnit != null)
                                        {
                                            foreach (MultilangImage multilangImage in digitalDateDigit.Digit.Image.MultilangImageUnit)
                                            {
                                                if (multilangImage.LangCode == "All")
                                                    comboBoxSetText(comboBox_Year_separator_AOD, multilangImage.ImageSet.ImageIndex);
                                            }
                                        }
                                    }
                                    AlignmentToString(comboBox_Year_alignment, digitalDateDigit.Digit.Alignment);
                                    if (digitalDateDigit.Digit.Spacing != null)
                                        numericUpDown_Year_spacing_AOD.Value = (decimal)digitalDateDigit.Digit.Spacing;
                                    checkBox_Year_add_zero_AOD.Checked = digitalDateDigit.Digit.PaddingZero;
                                }

                                if (digitalDateDigit.Separator != null)
                                {
                                    comboBoxSetText(comboBox_Year_unit_AOD, digitalDateDigit.Separator.ImageIndex);
                                    numericUpDown_Year_unitX_AOD.Value = digitalDateDigit.Separator.Coordinates.X;
                                    numericUpDown_Year_unitY_AOD.Value = digitalDateDigit.Separator.Coordinates.Y;
                                }
                                break;
                        }

                    }
                }

                // дата стрелкой
                if (Watch_Face.ScreenIdle.Date.DateClockHand != null && Watch_Face.ScreenIdle.Date.DateClockHand.DayClockHand != null)
                {
                    checkBox_Day_hand_Use_AOD.Checked = true;
                    numericUpDown_Day_handX_AOD.Value = Watch_Face.ScreenIdle.Date.DateClockHand.DayClockHand.X;
                    numericUpDown_Day_handY_AOD.Value = Watch_Face.ScreenIdle.Date.DateClockHand.DayClockHand.Y;
                    numericUpDown_Day_hand_startAngle_AOD.Value =
                        (decimal)Watch_Face.ScreenIdle.Date.DateClockHand.DayClockHand.StartAngle;
                    numericUpDown_Day_hand_endAngle_AOD.Value =
                        (decimal)Watch_Face.ScreenIdle.Date.DateClockHand.DayClockHand.EndAngle;
                    if (Watch_Face.ScreenIdle.Date.DateClockHand.DayClockHand.Pointer != null)
                    {
                        comboBoxSetText(comboBox_Day_hand_image_AOD,
                            (long)Watch_Face.ScreenIdle.Date.DateClockHand.DayClockHand.Pointer.ImageIndex);
                        if (Watch_Face.ScreenIdle.Date.DateClockHand.DayClockHand.Pointer.Coordinates != null)
                        {
                            numericUpDown_Day_handX_offset_AOD.Value =
                                Watch_Face.ScreenIdle.Date.DateClockHand.DayClockHand.Pointer.Coordinates.X;
                            numericUpDown_Day_handY_offset_AOD.Value =
                                Watch_Face.ScreenIdle.Date.DateClockHand.DayClockHand.Pointer.Coordinates.Y;
                        }
                    }

                    // центральное изображение
                    if (Watch_Face.ScreenIdle.Date.DateClockHand.DayClockHand.Cover != null)
                    {
                        comboBoxSetText(comboBox_Day_hand_imageCentr_AOD,
                            (long)Watch_Face.ScreenIdle.Date.DateClockHand.DayClockHand.Cover.ImageIndex);
                        if (Watch_Face.ScreenIdle.Date.DateClockHand.DayClockHand.Pointer.Coordinates != null)
                        {
                            numericUpDown_Day_handX_centr_AOD.Value =
                                Watch_Face.ScreenIdle.Date.DateClockHand.DayClockHand.Cover.Coordinates.X;
                            numericUpDown_Day_handY_centr_AOD.Value =
                                Watch_Face.ScreenIdle.Date.DateClockHand.DayClockHand.Cover.Coordinates.Y;
                        }
                    }

                    // фон
                    if (Watch_Face.ScreenIdle.Date.DateClockHand.DayClockHand.Scale != null &&
                        Watch_Face.ScreenIdle.Date.DateClockHand.DayClockHand.Scale.ImageSet != null)
                    {
                        foreach (MultilangImage multilangImage in Watch_Face.ScreenIdle.Date.DateClockHand.DayClockHand.Scale.ImageSet)
                        {
                            if (multilangImage.LangCode == "All")
                                comboBoxSetText(comboBox_Day_hand_imageBackground_AOD, multilangImage.ImageSet.ImageIndex);
                        }
                        if (Watch_Face.ScreenIdle.Date.DateClockHand.DayClockHand.Scale.Coordinates != null)
                        {
                            numericUpDown_Day_handX_background_AOD.Value =
                                Watch_Face.ScreenIdle.Date.DateClockHand.DayClockHand.Scale.Coordinates.X;
                            numericUpDown_Day_handY_background_AOD.Value =
                                Watch_Face.ScreenIdle.Date.DateClockHand.DayClockHand.Scale.Coordinates.Y;
                        }
                    }
                }

                // месяц стрелкой
                if (Watch_Face.ScreenIdle.Date.DateClockHand != null && Watch_Face.ScreenIdle.Date.DateClockHand.MonthClockHand != null)
                {
                    checkBox_Month_hand_Use_AOD.Checked = true;
                    numericUpDown_Month_handX_AOD.Value = Watch_Face.ScreenIdle.Date.DateClockHand.MonthClockHand.X;
                    numericUpDown_Month_handY_AOD.Value = Watch_Face.ScreenIdle.Date.DateClockHand.MonthClockHand.Y;
                    numericUpDown_Month_hand_startAngle_AOD.Value =
                        (decimal)Watch_Face.ScreenIdle.Date.DateClockHand.MonthClockHand.StartAngle;
                    numericUpDown_Month_hand_endAngle_AOD.Value =
                        (decimal)Watch_Face.ScreenIdle.Date.DateClockHand.MonthClockHand.EndAngle;
                    if (Watch_Face.ScreenIdle.Date.DateClockHand.MonthClockHand.Pointer != null)
                    {
                        comboBoxSetText(comboBox_Month_hand_image_AOD,
                            (long)Watch_Face.ScreenIdle.Date.DateClockHand.MonthClockHand.Pointer.ImageIndex);
                        if (Watch_Face.ScreenIdle.Date.DateClockHand.MonthClockHand.Pointer.Coordinates != null)
                        {
                            numericUpDown_Month_handX_offset_AOD.Value =
                                Watch_Face.ScreenIdle.Date.DateClockHand.MonthClockHand.Pointer.Coordinates.X;
                            numericUpDown_Month_handY_offset_AOD.Value =
                                Watch_Face.ScreenIdle.Date.DateClockHand.MonthClockHand.Pointer.Coordinates.Y;
                        }
                    }

                    // центральное изображение
                    if (Watch_Face.ScreenIdle.Date.DateClockHand.MonthClockHand.Cover != null)
                    {
                        comboBoxSetText(comboBox_Month_hand_imageCentr_AOD,
                            (long)Watch_Face.ScreenIdle.Date.DateClockHand.MonthClockHand.Cover.ImageIndex);
                        if (Watch_Face.ScreenIdle.Date.DateClockHand.MonthClockHand.Pointer.Coordinates != null)
                        {
                            numericUpDown_Month_handX_centr_AOD.Value =
                                Watch_Face.ScreenIdle.Date.DateClockHand.MonthClockHand.Cover.Coordinates.X;
                            numericUpDown_Month_handY_centr_AOD.Value =
                                Watch_Face.ScreenIdle.Date.DateClockHand.MonthClockHand.Cover.Coordinates.Y;
                        }
                    }

                    // фон
                    if (Watch_Face.ScreenIdle.Date.DateClockHand.MonthClockHand.Scale != null &&
                        Watch_Face.ScreenIdle.Date.DateClockHand.MonthClockHand.Scale.ImageSet != null)
                    {
                        foreach (MultilangImage multilangImage in Watch_Face.ScreenIdle.Date.DateClockHand.MonthClockHand.Scale.ImageSet)
                        {
                            if (multilangImage.LangCode == "All")
                                comboBoxSetText(comboBox_Month_hand_imageBackground_AOD, multilangImage.ImageSet.ImageIndex);
                        }
                        if (Watch_Face.ScreenIdle.Date.DateClockHand.MonthClockHand.Scale.Coordinates != null)
                        {
                            numericUpDown_Month_handX_background_AOD.Value =
                                Watch_Face.ScreenIdle.Date.DateClockHand.MonthClockHand.Scale.Coordinates.X;
                            numericUpDown_Month_handY_background_AOD.Value =
                                Watch_Face.ScreenIdle.Date.DateClockHand.MonthClockHand.Scale.Coordinates.Y;
                        }
                    }
                }

                // день недели стрелкой
                if (Watch_Face.ScreenIdle.Date.DateClockHand != null && Watch_Face.ScreenIdle.Date.DateClockHand.WeekDayClockHand != null)
                {
                    checkBox_DOW_hand_Use_AOD.Checked = true;
                    numericUpDown_DOW_handX_AOD.Value = Watch_Face.ScreenIdle.Date.DateClockHand.WeekDayClockHand.X;
                    numericUpDown_DOW_handY_AOD.Value = Watch_Face.ScreenIdle.Date.DateClockHand.WeekDayClockHand.Y;
                    numericUpDown_DOW_hand_startAngle_AOD.Value =
                        (decimal)Watch_Face.ScreenIdle.Date.DateClockHand.WeekDayClockHand.StartAngle;
                    numericUpDown_DOW_hand_endAngle_AOD.Value =
                        (decimal)Watch_Face.ScreenIdle.Date.DateClockHand.WeekDayClockHand.EndAngle;
                    if (Watch_Face.ScreenIdle.Date.DateClockHand.WeekDayClockHand.Pointer != null)
                    {
                        comboBoxSetText(comboBox_DOW_hand_image_AOD,
                            (long)Watch_Face.ScreenIdle.Date.DateClockHand.WeekDayClockHand.Pointer.ImageIndex);
                        if (Watch_Face.ScreenIdle.Date.DateClockHand.WeekDayClockHand.Pointer.Coordinates != null)
                        {
                            numericUpDown_DOW_handX_offset_AOD.Value =
                                Watch_Face.ScreenIdle.Date.DateClockHand.WeekDayClockHand.Pointer.Coordinates.X;
                            numericUpDown_DOW_handY_offset_AOD.Value =
                                Watch_Face.ScreenIdle.Date.DateClockHand.WeekDayClockHand.Pointer.Coordinates.Y;
                        }
                    }

                    // центральное изображение
                    if (Watch_Face.ScreenIdle.Date.DateClockHand.WeekDayClockHand.Cover != null)
                    {
                        comboBoxSetText(comboBox_DOW_hand_imageCentr_AOD,
                            (long)Watch_Face.ScreenIdle.Date.DateClockHand.WeekDayClockHand.Cover.ImageIndex);
                        if (Watch_Face.ScreenIdle.Date.DateClockHand.WeekDayClockHand.Pointer.Coordinates != null)
                        {
                            numericUpDown_DOW_handX_centr_AOD.Value =
                                Watch_Face.ScreenIdle.Date.DateClockHand.WeekDayClockHand.Cover.Coordinates.X;
                            numericUpDown_DOW_handY_centr_AOD.Value =
                                Watch_Face.ScreenIdle.Date.DateClockHand.WeekDayClockHand.Cover.Coordinates.Y;
                        }
                    }

                    // фон
                    if (Watch_Face.ScreenIdle.Date.DateClockHand.WeekDayClockHand.Scale != null &&
                        Watch_Face.ScreenIdle.Date.DateClockHand.WeekDayClockHand.Scale.ImageSet != null)
                    {
                        foreach (MultilangImage multilangImage in Watch_Face.ScreenIdle.Date.DateClockHand.WeekDayClockHand.Scale.ImageSet)
                        {
                            if (multilangImage.LangCode == "All")
                                comboBoxSetText(comboBox_DOW_hand_imageBackground_AOD, multilangImage.ImageSet.ImageIndex);
                        }
                        if (Watch_Face.ScreenIdle.Date.DateClockHand.WeekDayClockHand.Scale.Coordinates != null)
                        {
                            numericUpDown_DOW_handX_background_AOD.Value =
                                Watch_Face.ScreenIdle.Date.DateClockHand.WeekDayClockHand.Scale.Coordinates.X;
                            numericUpDown_DOW_handY_background_AOD.Value =
                                Watch_Face.ScreenIdle.Date.DateClockHand.WeekDayClockHand.Scale.Coordinates.Y;
                        }
                    }
                }

                // день недели картинкой
                if (Watch_Face.ScreenIdle.Date.WeeksDigits != null)
                {
                    if (Watch_Face.ScreenIdle.Date.WeeksDigits.Digit != null)
                    //if (Watch_Face.ScreenIdle.Date.WeeksDigits.Digit != null &&
                    //Watch_Face.ScreenIdle.Date.WeeksDigits.Digit.DisplayFormAnalog)
                    {
                        if (Watch_Face.ScreenIdle.Date.WeeksDigits.Digit.Image != null)
                        {
                            checkBox_DOW_pictures_Use_AOD.Checked = true;
                            numericUpDown_DOW_picturesX_AOD.Value = Watch_Face.ScreenIdle.Date.WeeksDigits.Digit.Image.X;
                            numericUpDown_DOW_picturesY_AOD.Value = Watch_Face.ScreenIdle.Date.WeeksDigits.Digit.Image.Y;
                            foreach (MultilangImage multilangImage in Watch_Face.ScreenIdle.Date.WeeksDigits.Digit.Image.MultilangImage)
                            {
                                if (multilangImage.LangCode == "All")
                                    comboBoxSetText(comboBox_DOW_pictures_image_AOD, multilangImage.ImageSet.ImageIndex);
                            }
                        }
                    }
                }

            }
            #endregion

            #region активности 
            if (Watch_Face.ScreenIdle != null && Watch_Face.ScreenIdle.Activity != null)
            {
                foreach (Activity activity in Watch_Face.ScreenIdle.Activity)
                {
                    UserControl_pictures userPanel_pictures = null;
                    UserControl_text userPanel_text = null;
                    UserControl_hand userPanel_hand = null;
                    UserControl_scaleCircle userPanel_scaleCircle = null;
                    UserControl_scaleLinear userPanel_scaleLinear = null;

                    UserControl_text_weather userPanel_text_weather = null;

                    switch (activity.Type)
                    {
                        case "Battery":
                            userPanel_pictures = userControl_pictures_Battery_AOD;
                            userPanel_text = userControl_text_Battery_AOD;
                            userPanel_hand = userControl_hand_Battery_AOD;
                            userPanel_scaleCircle = userControl_scaleCircle_Battery_AOD;
                            userPanel_scaleLinear = userControl_scaleLinear_Battery_AOD;
                            break;
                        case "Steps":
                            userPanel_pictures = userControl_pictures_Steps_AOD;
                            userPanel_text = userControl_text_Steps_AOD;
                            userPanel_hand = userControl_hand_Steps_AOD;
                            userPanel_scaleCircle = userControl_scaleCircle_Steps_AOD;
                            userPanel_scaleLinear = userControl_scaleLinear_Steps_AOD;
                            break;
                        case "Calories":
                            userPanel_pictures = userControl_pictures_Calories_AOD;
                            userPanel_text = userControl_text_Calories_AOD;
                            userPanel_hand = userControl_hand_Calories_AOD;
                            userPanel_scaleCircle = userControl_scaleCircle_Calories_AOD;
                            userPanel_scaleLinear = userControl_scaleLinear_Calories_AOD;
                            break;
                        case "HeartRate":
                            userPanel_pictures = userControl_pictures_HeartRate_AOD;
                            userPanel_text = userControl_text_HeartRate_AOD;
                            userPanel_hand = userControl_hand_HeartRate_AOD;
                            userPanel_scaleCircle = userControl_scaleCircle_HeartRate_AOD;
                            userPanel_scaleLinear = userControl_scaleLinear_HeartRate_AOD;
                            break;
                        case "PAI":
                            userPanel_pictures = userControl_pictures_PAI_AOD;
                            userPanel_text = userControl_text_PAI_AOD;
                            userPanel_hand = userControl_hand_PAI_AOD;
                            userPanel_scaleCircle = userControl_scaleCircle_PAI_AOD;
                            userPanel_scaleLinear = userControl_scaleLinear_PAI_AOD;
                            break;

                        case "Distance":
                            userPanel_text = userControl_text_Distance_AOD;
                            break;

                        case "StandUp":
                            userPanel_pictures = userControl_pictures_StandUp_AOD;
                            userPanel_text = userControl_text_StandUp_AOD;
                            userPanel_hand = userControl_hand_StandUp_AOD;
                            userPanel_scaleCircle = userControl_scaleCircle_StandUp_AOD;
                            userPanel_scaleLinear = userControl_scaleLinear_StandUp_AOD;
                            break;
                        case "Weather":
                            userPanel_pictures = userControl_pictures_weather_AOD;
                            userPanel_text_weather = userControl_text_weather_Current_AOD;
                            //userPanel_hand = userControl_hand_Weather_AOD;
                            //userPanel_scaleCircle = userControl_scaleCircle_Weather_AOD;
                            //userPanel_scaleLinear = userControl_scaleLinear_Weather_AOD;
                            break;
                        case "UVindex":
                            userPanel_pictures = userControl_pictures_UVindex_AOD;
                            userPanel_text = userControl_text_UVindex_AOD;
                            userPanel_hand = userControl_hand_UVindex_AOD;
                            userPanel_scaleCircle = userControl_scaleCircle_UVindex_AOD;
                            userPanel_scaleLinear = userControl_scaleLinear_UVindex_AOD;
                            break;
                        case "AirQuality":
                            //userPanel_pictures = userControl_pictures_AirQuality_AOD;
                            //userPanel_text = userControl_text_AirQuality_AOD;
                            //userPanel_hand = userControl_hand_AirQuality_AOD;
                            //userPanel_scaleCircle = userControl_scaleCircle_AirQuality_AOD;
                            //userPanel_scaleLinear = userControl_scaleLinear_AirQuality_AOD;
                            break;
                        case "Humidity":
                            userPanel_pictures = userControl_pictures_Humidity_AOD;
                            userPanel_text = userControl_text_Humidity_AOD;
                            userPanel_hand = userControl_hand_Humidity_AOD;
                            userPanel_scaleCircle = userControl_scaleCircle_Humidity_AOD;
                            userPanel_scaleLinear = userControl_scaleLinear_Humidity_AOD;
                            break;
                        case "WindForce":
                            userPanel_pictures = userControl_pictures_WindForce_AOD;
                            userPanel_text = userControl_text_WindForce_AOD;
                            userPanel_hand = userControl_hand_WindForce_AOD;
                            userPanel_scaleCircle = userControl_scaleCircle_WindForce_AOD;
                            userPanel_scaleLinear = userControl_scaleLinear_WindForce_AOD;
                            break;
                        case "Altitude":
                            userPanel_pictures = userControl_pictures_Altitude_AOD;
                            userPanel_text = userControl_text_Altitude_AOD;
                            userPanel_hand = userControl_hand_Altitude_AOD;
                            userPanel_scaleCircle = userControl_scaleCircle_Altitude_AOD;
                            userPanel_scaleLinear = userControl_scaleLinear_Altitude_AOD;
                            break;
                        case "AirPressure":
                            userPanel_pictures = userControl_pictures_AirPressure_AOD;
                            userPanel_text = userControl_text_AirPressure_AOD;
                            userPanel_hand = userControl_hand_AirPressure_AOD;
                            userPanel_scaleCircle = userControl_scaleCircle_AirPressure_AOD;
                            userPanel_scaleLinear = userControl_scaleLinear_AirPressure_AOD;
                            break;
                        case "Stress":
                            //userPanel_pictures = userControl_pictures_Stress_AOD;
                            //userPanel_text = userControl_text_Stress_AOD;
                            //userPanel_hand = userControl_hand_Stress_AOD;
                            //userPanel_scaleCircle = userControl_scaleCircle_Stress_AOD;
                            //userPanel_scaleLinear = userControl_scaleLinear_Stress_AOD;
                            break;
                        case "ActivityGoal":
                            //userPanel_pictures = userControl_pictures_ActivityGoal_AOD;
                            //userPanel_text = userControl_text_ActivityGoal_AOD;
                            //userPanel_hand = userControl_hand_ActivityGoal_AOD;
                            //userPanel_scaleCircle = userControl_scaleCircle_ActivityGoal_AOD;
                            //userPanel_scaleLinear = userControl_scaleLinear_ActivityGoal_AOD;
                            break;
                        case "FatBurning":
                            userPanel_pictures = userControl_pictures_FatBurning_AOD;
                            userPanel_text = userControl_text_FatBurning_AOD;
                            userPanel_hand = userControl_hand_FatBurning_AOD;
                            userPanel_scaleCircle = userControl_scaleCircle_FatBurning_AOD;
                            userPanel_scaleLinear = userControl_scaleLinear_FatBurning_AOD;
                            break;

                    }

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

                    if (userPanel_text != null && activity.Type != "Weather")
                    {
                        //checkBox_Use = (CheckBox)panel_text.Controls[0];
                        if (activity.Digits != null && activity.Digits.Count > 0)
                        {
                            userPanel_text.checkBox_Use.Checked = true;
                            if (activity.Digits[0].Digit != null && activity.Digits[0].Digit.Image != null)
                            {
                                //ComboBox comboBox_image = (ComboBox)userPanel_text.Controls[1];
                                //ComboBox comboBox_unit = (ComboBox)userPanel_text.Controls[2];
                                //ComboBox comboBox_separator = (ComboBox)userPanel_text.Controls[3];
                                NumericUpDown numericUpDownX = userPanel_text.numericUpDown_imageX;
                                NumericUpDown numericUpDownY = userPanel_text.numericUpDown_imageY;
                                NumericUpDown numericUpDown_unitX = userPanel_text.numericUpDown_iconX;
                                NumericUpDown numericUpDown_unitY = userPanel_text.numericUpDown_iconY;
                                //ComboBox comboBox_alignment = (ComboBox)userPanel_text.Controls[8];
                                NumericUpDown numericUpDown_spacing = userPanel_text.numericUpDown_spacing;
                                CheckBox checkBox_add_zero = userPanel_text.checkBox_addZero;
                                //ComboBox comboBox_imageError = (ComboBox)userPanel_text.Controls[11];

                                numericUpDownX.Value = activity.Digits[0].Digit.Image.X;
                                numericUpDownY.Value = activity.Digits[0].Digit.Image.Y;

                                // десятичный разделитель
                                if (activity.Type == "Distance")
                                {
                                    //ComboBox comboBox_DecimalPoint = (ComboBox)panel_text.Controls[12];
                                    if (activity.Digits[0].Digit.Image.DecimalPointImageIndex != null)
                                        userPanel_text.comboBoxSetImageDecimalPointOrMinus((int)activity.Digits[0].Digit.Image.DecimalPointImageIndex);
                                }

                                if (activity.Digits[0].Digit.Image.NoDataImageIndex != null)
                                    userPanel_text.comboBoxSetImageError((int)activity.Digits[0].Digit.Image.NoDataImageIndex);
                                foreach (MultilangImage multilangImage in activity.Digits[0].Digit.Image.MultilangImage)
                                {
                                    if (multilangImage.LangCode == "All")
                                        userPanel_text.comboBoxSetImage((int)multilangImage.ImageSet.ImageIndex);
                                }
                                if (activity.Digits[0].Digit.Image.MultilangImageUnit != null)
                                {
                                    foreach (MultilangImage multilangImage in activity.Digits[0].Digit.Image.MultilangImageUnit)
                                    {
                                        if (multilangImage.LangCode == "All")
                                            userPanel_text.comboBoxSetUnit((int)multilangImage.ImageSet.ImageIndex);
                                    }
                                }
                                userPanel_text.comboBoxSetAlignment(activity.Digits[0].Digit.Alignment);
                                if (activity.Digits[0].Digit.Spacing != null)
                                    numericUpDown_spacing.Value = (decimal)activity.Digits[0].Digit.Spacing;
                                checkBox_add_zero.Checked = activity.Digits[0].Digit.PaddingZero;
                                if (activity.Digits[0].Separator != null)
                                {
                                    userPanel_text.comboBoxSetIcon((int)activity.Digits[0].Separator.ImageIndex);
                                    numericUpDown_unitX.Value = activity.Digits[0].Separator.Coordinates.X;
                                    numericUpDown_unitY.Value = activity.Digits[0].Separator.Coordinates.Y;
                                }
                            }
                        }
                    }
                    else if (userPanel_text_weather != null && activity.Type == "Weather")
                    {
                        if (activity.Digits != null && activity.Digits.Count > 0)
                        {
                            foreach (DigitalCommonDigit digitalCommonDigit in activity.Digits)
                            {
                                if (digitalCommonDigit.Type != null && digitalCommonDigit.Type == "Min")
                                    userPanel_text_weather = userControl_text_weather_Min_AOD;
                                if (digitalCommonDigit.Type != null && digitalCommonDigit.Type == "Max")
                                    userPanel_text_weather = userControl_text_weather_Max_AOD;
                                if (digitalCommonDigit.Type == null)
                                    userPanel_text_weather = userControl_text_weather_Current_AOD;


                                userPanel_text_weather.checkBox_Use.Checked = true;
                                if (digitalCommonDigit.Digit != null && digitalCommonDigit.Digit.Image != null)
                                {
                                    //ComboBox comboBox_image = (ComboBox)panel_text.Controls[1];
                                    //ComboBox comboBox_unit = (ComboBox)panel_text.Controls[2]; icon
                                    //ComboBox comboBox_separatorF = (ComboBox)panel_text.Controls[3];
                                    NumericUpDown numericUpDownX = userPanel_text_weather.numericUpDown_imageX;
                                    NumericUpDown numericUpDownY = userPanel_text_weather.numericUpDown_imageY;
                                    NumericUpDown numericUpDown_unitX = userPanel_text_weather.numericUpDown_iconX;
                                    NumericUpDown numericUpDown_unitY = userPanel_text_weather.numericUpDown_iconY;
                                    //ComboBox comboBox_alignment = (ComboBox)panel_text.Controls[8];
                                    NumericUpDown numericUpDown_spacing = userPanel_text_weather.numericUpDown_spacing;
                                    //CheckBox checkBox_add_zero = (CheckBox)panel_text.Controls[10];
                                    //ComboBox comboBox_imageError = (ComboBox)panel_text.Controls[10];
                                    //ComboBox comboBox_imageMinus = (ComboBox)panel_text.Controls[11];

                                    if (digitalCommonDigit.Type != null && digitalCommonDigit.Type == "Max")
                                    {
                                        if (digitalCommonDigit.CombingMode == "Single")
                                        {
                                            userPanel_text_weather.checkBox_follow.Checked = false;
                                        }
                                        else
                                        {
                                            userPanel_text_weather.checkBox_follow.Checked = true;
                                        }
                                    }

                                    numericUpDownX.Value = digitalCommonDigit.Digit.Image.X;
                                    numericUpDownY.Value = digitalCommonDigit.Digit.Image.Y;

                                    if (digitalCommonDigit.Digit.Image.NoDataImageIndex != null)
                                        userPanel_text_weather.comboBoxSetImageError((int)digitalCommonDigit.Digit.Image.NoDataImageIndex);

                                    if (digitalCommonDigit.Digit.Image.DelimiterImageIndex != null)
                                        userPanel_text_weather.comboBoxSetImageDecimalPointOrMinus((int)digitalCommonDigit.Digit.Image.DelimiterImageIndex);

                                    foreach (MultilangImage multilangImage in digitalCommonDigit.Digit.Image.MultilangImage)
                                    {
                                        if (multilangImage.LangCode == "All")
                                            userPanel_text_weather.comboBoxSetImage((int)multilangImage.ImageSet.ImageIndex);
                                    }
                                    if (digitalCommonDigit.Digit.Image.MultilangImageUnit != null)
                                    {
                                        foreach (MultilangImage multilangImage in digitalCommonDigit.Digit.Image.MultilangImageUnit)
                                        {
                                            if (multilangImage.LangCode == null && userPanel_text_weather.comboBoxGetSelectedIndexUnit() < 0)
                                                userPanel_text_weather.comboBoxSetUnit((int)multilangImage.ImageSet.ImageIndex);
                                            if (multilangImage.LangCode == "All")
                                                userPanel_text_weather.comboBoxSetUnit((int)multilangImage.ImageSet.ImageIndex);
                                        }
                                    }
                                    userPanel_text_weather.comboBoxSetAlignment(digitalCommonDigit.Digit.Alignment);
                                    if (digitalCommonDigit.Digit.Spacing != null)
                                        numericUpDown_spacing.Value = (decimal)digitalCommonDigit.Digit.Spacing;

                                    userPanel_text_weather.checkBox_addZero.Checked = digitalCommonDigit.Digit.PaddingZero;

                                    if (digitalCommonDigit.Separator != null)
                                    {
                                        userPanel_text_weather.comboBoxSetIcon((int)digitalCommonDigit.Separator.ImageIndex);
                                        numericUpDown_unitX.Value = digitalCommonDigit.Separator.Coordinates.X;
                                        numericUpDown_unitY.Value = digitalCommonDigit.Separator.Coordinates.Y;
                                    }
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
                }
            }
            #endregion
        }

        private void JSON_write_AOD()
        {
            if (Watch_Face == null) Watch_Face = new WATCH_FACE_JSON();
            ScreenIdle ScreenIdle = new ScreenIdle();

            #region Background
            if (comboBox_Background_image_AOD.SelectedIndex >= 0)
            {
                ScreenIdle.BackgroundImageIndex = Int32.Parse(comboBox_Background_image_AOD.Text);
            }
            #endregion

            #region цифровое время
            // часы
            if (checkBox_Hour_Use_AOD.Checked && comboBox_Hour_image_AOD.SelectedIndex >= 0)
            {
                if (ScreenIdle.DialFace == null) ScreenIdle.DialFace = new ScreenNormal();
                if (ScreenIdle.DialFace.DigitalDialFace == null)
                    ScreenIdle.DialFace.DigitalDialFace = new DigitalDialFace();
                if (ScreenIdle.DialFace.DigitalDialFace.Digits == null)
                    ScreenIdle.DialFace.DigitalDialFace.Digits = new List<DigitalTimeDigit>();

                DigitalTimeDigit digitalTimeDigit = new DigitalTimeDigit();
                digitalTimeDigit.TimeType = "Hour";
                digitalTimeDigit.CombingMode = "Single";
                digitalTimeDigit.Digit = new Text();
                digitalTimeDigit.Digit.Image = new ImageAmazfit();
                digitalTimeDigit.Digit.Image.X = (long)numericUpDown_HourX_AOD.Value;
                digitalTimeDigit.Digit.Image.Y = (long)numericUpDown_HourY_AOD.Value;
                digitalTimeDigit.Digit.Image.MultilangImage = new List<MultilangImage>();
                MultilangImage multilangImage = new MultilangImage();
                multilangImage.LangCode = "All";
                multilangImage.ImageSet = new ImageSetGTR2();
                multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_Hour_image_AOD.Text);
                multilangImage.ImageSet.ImagesCount = 10;
                digitalTimeDigit.Digit.Image.MultilangImage.Add(multilangImage);
                if (comboBox_Hour_separator_AOD.SelectedIndex >= 0)
                {
                    digitalTimeDigit.Digit.Image.MultilangImageUnit = new List<MultilangImage>();
                    MultilangImage multilangImageUnit = new MultilangImage();
                    multilangImageUnit.LangCode = "All";
                    multilangImageUnit.ImageSet = new ImageSetGTR2();
                    multilangImageUnit.ImageSet.ImageIndex = Int32.Parse(comboBox_Hour_separator_AOD.Text);
                    multilangImageUnit.ImageSet.ImagesCount = 1;
                    digitalTimeDigit.Digit.Image.MultilangImageUnit.Add(multilangImageUnit);
                }
                string Alignment = StringToAlignment(comboBox_Hour_alignment_AOD.SelectedIndex);
                digitalTimeDigit.Digit.Alignment = Alignment;
                digitalTimeDigit.Digit.Spacing = (long)numericUpDown_Hour_spacing_AOD.Value;
                digitalTimeDigit.Digit.PaddingZero = checkBox_Hour_add_zero_AOD.Checked;


                if (comboBox_Hour_unit_AOD.SelectedIndex >= 0)
                {
                    digitalTimeDigit.Separator = new ImageCoord();
                    digitalTimeDigit.Separator.Coordinates = new Coordinates();
                    digitalTimeDigit.Separator.Coordinates.X = (long)numericUpDown_Hour_unitX_AOD.Value;
                    digitalTimeDigit.Separator.Coordinates.Y = (long)numericUpDown_Hour_unitY_AOD.Value;
                    digitalTimeDigit.Separator.ImageIndex = Int32.Parse(comboBox_Hour_unit_AOD.Text);
                }
                ScreenIdle.DialFace.DigitalDialFace.Digits.Add(digitalTimeDigit);

            }

            // минуты
            if (checkBox_Minute_Use_AOD.Checked && comboBox_Minute_image_AOD.SelectedIndex >= 0)
            {
                if (ScreenIdle.DialFace == null) ScreenIdle.DialFace = new ScreenNormal();
                if (ScreenIdle.DialFace.DigitalDialFace == null)
                    ScreenIdle.DialFace.DigitalDialFace = new DigitalDialFace();
                if (ScreenIdle.DialFace.DigitalDialFace.Digits == null)
                    ScreenIdle.DialFace.DigitalDialFace.Digits = new List<DigitalTimeDigit>();

                DigitalTimeDigit digitalTimeDigit = new DigitalTimeDigit();
                digitalTimeDigit.TimeType = "Minute";
                //digitalTimeDigit.CombingMode = "Single";
                digitalTimeDigit.CombingMode = checkBox_Minute_follow_AOD.Checked ? "Follow" : "Single";
                digitalTimeDigit.Digit = new Text();
                digitalTimeDigit.Digit.Image = new ImageAmazfit();
                digitalTimeDigit.Digit.Image.X = (long)numericUpDown_MinuteX_AOD.Value;
                digitalTimeDigit.Digit.Image.Y = (long)numericUpDown_MinuteY_AOD.Value;
                digitalTimeDigit.Digit.Image.MultilangImage = new List<MultilangImage>();
                MultilangImage multilangImage = new MultilangImage();
                multilangImage.LangCode = "All";
                multilangImage.ImageSet = new ImageSetGTR2();
                multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_Minute_image_AOD.Text);
                multilangImage.ImageSet.ImagesCount = 10;
                digitalTimeDigit.Digit.Image.MultilangImage.Add(multilangImage);
                if (comboBox_Minute_separator_AOD.SelectedIndex >= 0)
                {
                    digitalTimeDigit.Digit.Image.MultilangImageUnit = new List<MultilangImage>();
                    MultilangImage multilangImageUnit = new MultilangImage();
                    multilangImageUnit.LangCode = "All";
                    multilangImageUnit.ImageSet = new ImageSetGTR2();
                    multilangImageUnit.ImageSet.ImageIndex = Int32.Parse(comboBox_Minute_separator_AOD.Text);
                    multilangImageUnit.ImageSet.ImagesCount = 1;
                    digitalTimeDigit.Digit.Image.MultilangImageUnit.Add(multilangImageUnit);
                }
                string Alignment = StringToAlignment(comboBox_Minute_alignment_AOD.SelectedIndex);
                digitalTimeDigit.Digit.Alignment = Alignment;
                digitalTimeDigit.Digit.Spacing = (long)numericUpDown_Minute_spacing_AOD.Value;
                digitalTimeDigit.Digit.PaddingZero = checkBox_Minute_add_zero_AOD.Checked;

                if (comboBox_Minute_unit_AOD.SelectedIndex >= 0)
                {
                    digitalTimeDigit.Separator = new ImageCoord();
                    digitalTimeDigit.Separator.Coordinates = new Coordinates();
                    digitalTimeDigit.Separator.Coordinates.X = (long)numericUpDown_Minute_unitX_AOD.Value;
                    digitalTimeDigit.Separator.Coordinates.Y = (long)numericUpDown_Minute_unitY_AOD.Value;
                    digitalTimeDigit.Separator.ImageIndex = Int32.Parse(comboBox_Minute_unit_AOD.Text);
                }
                ScreenIdle.DialFace.DigitalDialFace.Digits.Add(digitalTimeDigit);
            }

            // AM/PM
            if (checkBox_12h_Use_AOD.Checked &&
                comboBox_AM_image_AOD.SelectedIndex >= 0 && comboBox_PM_image_AOD.SelectedIndex >= 0)
            {
                if (ScreenIdle.DialFace == null) ScreenIdle.DialFace = new ScreenNormal();
                if (ScreenIdle.DialFace.DigitalDialFace == null)
                    ScreenIdle.DialFace.DigitalDialFace = new DigitalDialFace();

                ScreenIdle.DialFace.DigitalDialFace.AM = new MultilangImageCoord();
                ScreenIdle.DialFace.DigitalDialFace.AM.Coordinates = new Coordinates();
                ScreenIdle.DialFace.DigitalDialFace.AM.Coordinates.X = (long)numericUpDown_AM_X_AOD.Value;
                ScreenIdle.DialFace.DigitalDialFace.AM.Coordinates.Y = (long)numericUpDown_AM_Y_AOD.Value;
                ScreenIdle.DialFace.DigitalDialFace.AM.ImageSet = new List<MultilangImage>();
                MultilangImage multilangImage = new MultilangImage();
                multilangImage.LangCode = "All";
                multilangImage.ImageSet = new ImageSetGTR2();
                multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_AM_image_AOD.Text);
                multilangImage.ImageSet.ImagesCount = 1;
                ScreenIdle.DialFace.DigitalDialFace.AM.ImageSet.Add(multilangImage);

                ScreenIdle.DialFace.DigitalDialFace.PM = new MultilangImageCoord();
                ScreenIdle.DialFace.DigitalDialFace.PM.Coordinates = new Coordinates();
                ScreenIdle.DialFace.DigitalDialFace.PM.Coordinates.X = (long)numericUpDown_PM_X_AOD.Value;
                ScreenIdle.DialFace.DigitalDialFace.PM.Coordinates.Y = (long)numericUpDown_PM_Y_AOD.Value;
                ScreenIdle.DialFace.DigitalDialFace.PM.ImageSet = new List<MultilangImage>();
                multilangImage = new MultilangImage();
                multilangImage.LangCode = "All";
                multilangImage.ImageSet = new ImageSetGTR2();
                multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_PM_image_AOD.Text);
                multilangImage.ImageSet.ImagesCount = 1;
                ScreenIdle.DialFace.DigitalDialFace.PM.ImageSet.Add(multilangImage);
            }
            #endregion

            #region аналоговое  время
            // часы
            if (checkBox_Hour_hand_Use_AOD.Checked && comboBox_Hour_hand_image_AOD.SelectedIndex >= 0)
            {
                if (ScreenIdle.DialFace == null) ScreenIdle.DialFace = new ScreenNormal();
                if (ScreenIdle.DialFace.AnalogDialFace == null)
                    ScreenIdle.DialFace.AnalogDialFace = new AnalogDialFace();
                ScreenIdle.DialFace.AnalogDialFace.Hours = new ClockHand();
                ScreenIdle.DialFace.AnalogDialFace.Hours.X = (long)numericUpDown_Hour_handX_AOD.Value;
                ScreenIdle.DialFace.AnalogDialFace.Hours.Y = (long)numericUpDown_Hour_handY_AOD.Value;
                ScreenIdle.DialFace.AnalogDialFace.Hours.StartAngle = 0;
                ScreenIdle.DialFace.AnalogDialFace.Hours.EndAngle = 360;
                ScreenIdle.DialFace.AnalogDialFace.Hours.Pointer = new ImageCoord();
                ScreenIdle.DialFace.AnalogDialFace.Hours.Pointer.Coordinates = new Coordinates();
                ScreenIdle.DialFace.AnalogDialFace.Hours.Pointer.Coordinates.X = (long)numericUpDown_Hour_handX_offset_AOD.Value;
                ScreenIdle.DialFace.AnalogDialFace.Hours.Pointer.Coordinates.Y = (long)numericUpDown_Hour_handY_offset_AOD.Value;
                ScreenIdle.DialFace.AnalogDialFace.Hours.Pointer.ImageIndex = Int32.Parse(comboBox_Hour_hand_image_AOD.Text);
                if (comboBox_Hour_hand_imageCentr_AOD.SelectedIndex >= 0)
                {
                    ScreenIdle.DialFace.AnalogDialFace.Hours.Cover = new ImageCoord();
                    ScreenIdle.DialFace.AnalogDialFace.Hours.Cover.Coordinates = new Coordinates();
                    ScreenIdle.DialFace.AnalogDialFace.Hours.Cover.Coordinates.X = (long)numericUpDown_Hour_handX_centr_AOD.Value;
                    ScreenIdle.DialFace.AnalogDialFace.Hours.Cover.Coordinates.Y = (long)numericUpDown_Hour_handY_centr_AOD.Value;
                    ScreenIdle.DialFace.AnalogDialFace.Hours.Cover.ImageIndex = Int32.Parse(comboBox_Hour_hand_imageCentr_AOD.Text);
                }
            }

            // минуты
            if (checkBox_Minute_hand_Use_AOD.Checked && comboBox_Minute_hand_image_AOD.SelectedIndex >= 0)
            {
                if (ScreenIdle.DialFace == null) ScreenIdle.DialFace = new ScreenNormal();
                if (ScreenIdle.DialFace.AnalogDialFace == null)
                    ScreenIdle.DialFace.AnalogDialFace = new AnalogDialFace();
                ScreenIdle.DialFace.AnalogDialFace.Minutes = new ClockHand();
                ScreenIdle.DialFace.AnalogDialFace.Minutes.X = (long)numericUpDown_Minute_handX_AOD.Value;
                ScreenIdle.DialFace.AnalogDialFace.Minutes.Y = (long)numericUpDown_Minute_handY_AOD.Value;
                ScreenIdle.DialFace.AnalogDialFace.Minutes.StartAngle = 0;
                ScreenIdle.DialFace.AnalogDialFace.Minutes.EndAngle = 360;
                ScreenIdle.DialFace.AnalogDialFace.Minutes.Pointer = new ImageCoord();
                ScreenIdle.DialFace.AnalogDialFace.Minutes.Pointer.Coordinates = new Coordinates();
                ScreenIdle.DialFace.AnalogDialFace.Minutes.Pointer.Coordinates.X = (long)numericUpDown_Minute_handX_offset_AOD.Value;
                ScreenIdle.DialFace.AnalogDialFace.Minutes.Pointer.Coordinates.Y = (long)numericUpDown_Minute_handY_offset_AOD.Value;
                ScreenIdle.DialFace.AnalogDialFace.Minutes.Pointer.ImageIndex = Int32.Parse(comboBox_Minute_hand_image_AOD.Text);
                if (comboBox_Minute_hand_imageCentr_AOD.SelectedIndex >= 0)
                {
                    ScreenIdle.DialFace.AnalogDialFace.Minutes.Cover = new ImageCoord();
                    ScreenIdle.DialFace.AnalogDialFace.Minutes.Cover.Coordinates = new Coordinates();
                    ScreenIdle.DialFace.AnalogDialFace.Minutes.Cover.Coordinates.X = (long)numericUpDown_Minute_handX_centr_AOD.Value;
                    ScreenIdle.DialFace.AnalogDialFace.Minutes.Cover.Coordinates.Y = (long)numericUpDown_Minute_handY_centr_AOD.Value;
                    ScreenIdle.DialFace.AnalogDialFace.Minutes.Cover.ImageIndex = Int32.Parse(comboBox_Minute_hand_imageCentr_AOD.Text);
                }
            }
            #endregion

            #region дата
            // день недели картинкой
            if (checkBox_DOW_pictures_Use_AOD.Checked && comboBox_DOW_pictures_image_AOD.SelectedIndex >= 0)
            {
                if (ScreenIdle.Date == null) ScreenIdle.Date = new Date();
                if (ScreenIdle.Date.WeeksDigits == null)
                    ScreenIdle.Date.WeeksDigits = new DigitalCommonDigit();

                ScreenIdle.Date.WeeksDigits.CombingMode = "Single";
                //digitalDateDigit.CombingMode = checkBox_DOW_follow.Checked ? "Follow" : "Single";
                ScreenIdle.Date.WeeksDigits.Digit = new Text();
                ScreenIdle.Date.WeeksDigits.Digit.DisplayFormAnalog = true;
                ScreenIdle.Date.WeeksDigits.Digit.Image = new ImageAmazfit();
                ScreenIdle.Date.WeeksDigits.Digit.Image.X = (long)numericUpDown_DOW_picturesX_AOD.Value;
                ScreenIdle.Date.WeeksDigits.Digit.Image.Y = (long)numericUpDown_DOW_picturesY_AOD.Value;
                ScreenIdle.Date.WeeksDigits.Digit.Image.MultilangImage = new List<MultilangImage>();
                MultilangImage multilangImage = new MultilangImage();
                multilangImage.LangCode = "All";
                multilangImage.ImageSet = new ImageSetGTR2();
                if (comboBox_DOW_pictures_image_AOD.SelectedIndex >= 0)
                    multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_DOW_pictures_image_AOD.Text);
                multilangImage.ImageSet.ImagesCount = 7;
                ScreenIdle.Date.WeeksDigits.Digit.Image.MultilangImage.Add(multilangImage);
            }

            // день недели стрелкой
            if (checkBox_DOW_hand_Use_AOD.Checked && comboBox_DOW_hand_image_AOD.SelectedIndex >= 0)
            {
                if (ScreenIdle.Date == null)
                    ScreenIdle.Date = new Date();
                if (ScreenIdle.Date.DateClockHand == null)
                    ScreenIdle.Date.DateClockHand = new DateClockHand();
                ScreenIdle.Date.DateClockHand.WeekDayClockHand = new ClockHand();
                ScreenIdle.Date.DateClockHand.WeekDayClockHand.X = (long)numericUpDown_DOW_handX_AOD.Value;
                ScreenIdle.Date.DateClockHand.WeekDayClockHand.Y = (long)numericUpDown_DOW_handY_AOD.Value;
                ScreenIdle.Date.DateClockHand.WeekDayClockHand.StartAngle = (float)numericUpDown_DOW_hand_startAngle_AOD.Value;
                ScreenIdle.Date.DateClockHand.WeekDayClockHand.EndAngle = (float)numericUpDown_DOW_hand_endAngle_AOD.Value;
                ScreenIdle.Date.DateClockHand.WeekDayClockHand.Pointer = new ImageCoord();
                ScreenIdle.Date.DateClockHand.WeekDayClockHand.Pointer.Coordinates = new Coordinates();
                ScreenIdle.Date.DateClockHand.WeekDayClockHand.Pointer.Coordinates.X = (long)numericUpDown_DOW_handX_offset_AOD.Value;
                ScreenIdle.Date.DateClockHand.WeekDayClockHand.Pointer.Coordinates.Y = (long)numericUpDown_DOW_handY_offset_AOD.Value;
                ScreenIdle.Date.DateClockHand.WeekDayClockHand.Pointer.ImageIndex = Int32.Parse(comboBox_DOW_hand_image_AOD.Text);
                if (comboBox_DOW_hand_imageCentr_AOD.SelectedIndex >= 0)
                {
                    ScreenIdle.Date.DateClockHand.WeekDayClockHand.Cover = new ImageCoord();
                    ScreenIdle.Date.DateClockHand.WeekDayClockHand.Cover.Coordinates = new Coordinates();
                    ScreenIdle.Date.DateClockHand.WeekDayClockHand.Cover.Coordinates.X = (long)numericUpDown_DOW_handX_centr_AOD.Value;
                    ScreenIdle.Date.DateClockHand.WeekDayClockHand.Cover.Coordinates.Y = (long)numericUpDown_DOW_handY_centr_AOD.Value;
                    ScreenIdle.Date.DateClockHand.WeekDayClockHand.Cover.ImageIndex = Int32.Parse(comboBox_DOW_hand_imageCentr_AOD.Text);
                }

                if (comboBox_DOW_hand_imageBackground_AOD.SelectedIndex >= 0)
                {
                    ScreenIdle.Date.DateClockHand.WeekDayClockHand.Scale = new MultilangImageCoord();
                    ScreenIdle.Date.DateClockHand.WeekDayClockHand.Scale.Coordinates = new Coordinates();
                    ScreenIdle.Date.DateClockHand.WeekDayClockHand.Scale.Coordinates.X = (long)numericUpDown_DOW_handX_background_AOD.Value;
                    ScreenIdle.Date.DateClockHand.WeekDayClockHand.Scale.Coordinates.Y = (long)numericUpDown_DOW_handY_background_AOD.Value;
                    ScreenIdle.Date.DateClockHand.WeekDayClockHand.Scale.ImageSet = new List<MultilangImage>();
                    MultilangImage multilangImage = new MultilangImage();
                    multilangImage.LangCode = "All";
                    multilangImage.ImageSet = new ImageSetGTR2();
                    multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_DOW_hand_imageBackground_AOD.Text);
                    multilangImage.ImageSet.ImagesCount = 1;
                    ScreenIdle.Date.DateClockHand.WeekDayClockHand.Scale.ImageSet.Add(multilangImage);
                }
            }

            // год
            if (checkBox_Year_text_Use_AOD.Checked && comboBox_Year_image_AOD.SelectedIndex >= 0)
            {
                if (ScreenIdle.Date == null) ScreenIdle.Date = new Date();
                if (ScreenIdle.Date.DateDigits == null)
                    ScreenIdle.Date.DateDigits = new List<DigitalDateDigit>();

                DigitalDateDigit digitalDateDigit = new DigitalDateDigit();
                digitalDateDigit.DateType = "Year";
                digitalDateDigit.CombingMode = "Single";
                //digitalDateDigit.CombingMode = checkBox_Year_follow.Checked ? "Follow" : "Single";
                digitalDateDigit.Digit = new Text();
                digitalDateDigit.Digit.Image = new ImageAmazfit();
                digitalDateDigit.Digit.Image.X = (long)numericUpDown_YearX_AOD.Value;
                digitalDateDigit.Digit.Image.Y = (long)numericUpDown_YearY_AOD.Value;
                digitalDateDigit.Digit.Image.MultilangImage = new List<MultilangImage>();
                MultilangImage multilangImage = new MultilangImage();
                multilangImage.LangCode = "All";
                multilangImage.ImageSet = new ImageSetGTR2();
                if (comboBox_Year_image_AOD.SelectedIndex >= 0)
                    multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_Year_image_AOD.Text);
                multilangImage.ImageSet.ImagesCount = 10;
                digitalDateDigit.Digit.Image.MultilangImage.Add(multilangImage);
                if (comboBox_Year_separator_AOD.SelectedIndex >= 0)
                {
                    digitalDateDigit.Digit.Image.MultilangImageUnit = new List<MultilangImage>();
                    MultilangImage multilangImageUnit = new MultilangImage();
                    multilangImageUnit.LangCode = "All";
                    multilangImageUnit.ImageSet = new ImageSetGTR2();
                    multilangImageUnit.ImageSet.ImageIndex = Int32.Parse(comboBox_Year_separator_AOD.Text);
                    multilangImageUnit.ImageSet.ImagesCount = 1;
                    digitalDateDigit.Digit.Image.MultilangImageUnit.Add(multilangImageUnit);
                }
                string Alignment = StringToAlignment(comboBox_Year_alignment_AOD.SelectedIndex);
                digitalDateDigit.Digit.Alignment = Alignment;
                digitalDateDigit.Digit.Spacing = (long)numericUpDown_Year_spacing_AOD.Value;
                //digitalTimeDigit.Digit.PaddingZero = checkBox_Year_add_zero.Checked ? 1 : 0;
                digitalDateDigit.Digit.PaddingZero = checkBox_Year_add_zero_AOD.Checked;

                if (comboBox_Year_unit_AOD.SelectedIndex >= 0)
                {
                    digitalDateDigit.Separator = new ImageCoord();
                    digitalDateDigit.Separator.Coordinates = new Coordinates();
                    digitalDateDigit.Separator.Coordinates.X = (long)numericUpDown_Year_unitX_AOD.Value;
                    digitalDateDigit.Separator.Coordinates.Y = (long)numericUpDown_Year_unitY_AOD.Value;
                    digitalDateDigit.Separator.ImageIndex = Int32.Parse(comboBox_Year_unit_AOD.Text);
                }
                ScreenIdle.Date.DateDigits.Add(digitalDateDigit);
            }

            // месяц
            if (checkBox_Month_Use_AOD.Checked && comboBox_Month_image_AOD.SelectedIndex >= 0)
            {
                if (ScreenIdle.Date == null) ScreenIdle.Date = new Date();
                if (ScreenIdle.Date.DateDigits == null)
                    ScreenIdle.Date.DateDigits = new List<DigitalDateDigit>();

                DigitalDateDigit digitalDateDigit = new DigitalDateDigit();
                digitalDateDigit.DateType = "Month";
                //digitalTimeDigit.CombingMode = "Single";
                digitalDateDigit.CombingMode = checkBox_Month_follow.Checked ? "Follow" : "Single";
                digitalDateDigit.Digit = new Text();
                digitalDateDigit.Digit.Image = new ImageAmazfit();
                digitalDateDigit.Digit.Image.X = (long)numericUpDown_MonthX_AOD.Value;
                digitalDateDigit.Digit.Image.Y = (long)numericUpDown_MonthY_AOD.Value;
                digitalDateDigit.Digit.Image.MultilangImage = new List<MultilangImage>();
                MultilangImage multilangImage = new MultilangImage();
                multilangImage.LangCode = "All";
                multilangImage.ImageSet = new ImageSetGTR2();
                if (comboBox_Month_image_AOD.SelectedIndex >= 0)
                    multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_Month_image_AOD.Text);
                multilangImage.ImageSet.ImagesCount = 10;
                digitalDateDigit.Digit.Image.MultilangImage.Add(multilangImage);
                if (comboBox_Month_separator_AOD.SelectedIndex >= 0)
                {
                    digitalDateDigit.Digit.Image.MultilangImageUnit = new List<MultilangImage>();
                    MultilangImage multilangImageUnit = new MultilangImage();
                    multilangImageUnit.LangCode = "All";
                    multilangImageUnit.ImageSet = new ImageSetGTR2();
                    multilangImageUnit.ImageSet.ImageIndex = Int32.Parse(comboBox_Month_separator_AOD.Text);
                    multilangImageUnit.ImageSet.ImagesCount = 1;
                    digitalDateDigit.Digit.Image.MultilangImageUnit.Add(multilangImageUnit);
                }
                string Alignment = StringToAlignment(comboBox_Month_alignment_AOD.SelectedIndex);
                digitalDateDigit.Digit.Alignment = Alignment;
                digitalDateDigit.Digit.Spacing = (long)numericUpDown_Month_spacing_AOD.Value;
                //digitalTimeDigit.Digit.PaddingZero = checkBox_Month_add_zero.Checked ? 1 : 0;
                digitalDateDigit.Digit.PaddingZero = checkBox_Month_add_zero_AOD.Checked;

                if (comboBox_Month_unit_AOD.SelectedIndex >= 0)
                {
                    digitalDateDigit.Separator = new ImageCoord();
                    digitalDateDigit.Separator.Coordinates = new Coordinates();
                    digitalDateDigit.Separator.Coordinates.X = (long)numericUpDown_Month_unitX_AOD.Value;
                    digitalDateDigit.Separator.Coordinates.Y = (long)numericUpDown_Month_unitY_AOD.Value;
                    digitalDateDigit.Separator.ImageIndex = Int32.Parse(comboBox_Month_unit_AOD.Text);
                }
                ScreenIdle.Date.DateDigits.Add(digitalDateDigit);
            }

            // месяц картинкой
            if (checkBox_Month_pictures_Use_AOD.Checked && comboBox_Month_pictures_image_AOD.SelectedIndex >= 0)
            {
                if (ScreenIdle.Date == null) ScreenIdle.Date = new Date();
                if (ScreenIdle.Date.DateDigits == null)
                    ScreenIdle.Date.DateDigits = new List<DigitalDateDigit>();

                DigitalDateDigit digitalDateDigit = new DigitalDateDigit();
                digitalDateDigit.DateType = "Month";
                digitalDateDigit.CombingMode = "Single";
                //digitalDateDigit.CombingMode = checkBox_Month_follow.Checked ? "Follow" : "Single";
                digitalDateDigit.Digit = new Text();
                digitalDateDigit.Digit.DisplayFormAnalog = true;
                digitalDateDigit.Digit.Image = new ImageAmazfit();
                digitalDateDigit.Digit.Image.X = (long)numericUpDown_Month_picturesX_AOD.Value;
                digitalDateDigit.Digit.Image.Y = (long)numericUpDown_Month_picturesY_AOD.Value;
                digitalDateDigit.Digit.Image.MultilangImage = new List<MultilangImage>();
                MultilangImage multilangImage = new MultilangImage();
                multilangImage.LangCode = "All";
                multilangImage.ImageSet = new ImageSetGTR2();
                if (comboBox_Month_pictures_image_AOD.SelectedIndex >= 0)
                    multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_Month_pictures_image_AOD.Text);
                multilangImage.ImageSet.ImagesCount = 12;
                digitalDateDigit.Digit.Image.MultilangImage.Add(multilangImage);

                ScreenIdle.Date.DateDigits.Add(digitalDateDigit);
            }

            // месяц стрелкой
            if (checkBox_Month_hand_Use_AOD.Checked && comboBox_Month_hand_image_AOD.SelectedIndex >= 0)
            {
                if (ScreenIdle.Date == null)
                    ScreenIdle.Date = new Date();
                if (ScreenIdle.Date.DateClockHand == null)
                    ScreenIdle.Date.DateClockHand = new DateClockHand();
                ScreenIdle.Date.DateClockHand.MonthClockHand = new ClockHand();
                ScreenIdle.Date.DateClockHand.MonthClockHand.X = (long)numericUpDown_Month_handX_AOD.Value;
                ScreenIdle.Date.DateClockHand.MonthClockHand.Y = (long)numericUpDown_Month_handY_AOD.Value;
                ScreenIdle.Date.DateClockHand.MonthClockHand.StartAngle = (float)numericUpDown_Month_hand_startAngle_AOD.Value;
                ScreenIdle.Date.DateClockHand.MonthClockHand.EndAngle = (float)numericUpDown_Month_hand_endAngle_AOD.Value;
                ScreenIdle.Date.DateClockHand.MonthClockHand.Pointer = new ImageCoord();
                ScreenIdle.Date.DateClockHand.MonthClockHand.Pointer.Coordinates = new Coordinates();
                ScreenIdle.Date.DateClockHand.MonthClockHand.Pointer.Coordinates.X = (long)numericUpDown_Month_handX_offset_AOD.Value;
                ScreenIdle.Date.DateClockHand.MonthClockHand.Pointer.Coordinates.Y = (long)numericUpDown_Month_handY_offset_AOD.Value;
                ScreenIdle.Date.DateClockHand.MonthClockHand.Pointer.ImageIndex = Int32.Parse(comboBox_Month_hand_image_AOD.Text);

                if (comboBox_Month_hand_imageCentr_AOD.SelectedIndex >= 0)
                {
                    ScreenIdle.Date.DateClockHand.MonthClockHand.Cover = new ImageCoord();
                    ScreenIdle.Date.DateClockHand.MonthClockHand.Cover.Coordinates = new Coordinates();
                    ScreenIdle.Date.DateClockHand.MonthClockHand.Cover.Coordinates.X = (long)numericUpDown_Month_handX_centr_AOD.Value;
                    ScreenIdle.Date.DateClockHand.MonthClockHand.Cover.Coordinates.Y = (long)numericUpDown_Month_handY_centr_AOD.Value;
                    ScreenIdle.Date.DateClockHand.MonthClockHand.Cover.ImageIndex = Int32.Parse(comboBox_Month_hand_imageCentr_AOD.Text);
                }

                if (comboBox_Month_hand_imageBackground_AOD.SelectedIndex >= 0)
                {
                    ScreenIdle.Date.DateClockHand.MonthClockHand.Scale = new MultilangImageCoord();
                    ScreenIdle.Date.DateClockHand.MonthClockHand.Scale.Coordinates = new Coordinates();
                    ScreenIdle.Date.DateClockHand.MonthClockHand.Scale.Coordinates.X = (long)numericUpDown_Month_handX_background_AOD.Value;
                    ScreenIdle.Date.DateClockHand.MonthClockHand.Scale.Coordinates.Y = (long)numericUpDown_Month_handY_background_AOD.Value;
                    ScreenIdle.Date.DateClockHand.MonthClockHand.Scale.ImageSet = new List<MultilangImage>();
                    MultilangImage multilangImage = new MultilangImage();
                    multilangImage.LangCode = "All";
                    multilangImage.ImageSet = new ImageSetGTR2();
                    multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_Month_hand_imageBackground_AOD.Text);
                    multilangImage.ImageSet.ImagesCount = 1;
                    ScreenIdle.Date.DateClockHand.MonthClockHand.Scale.ImageSet.Add(multilangImage);
                }
            }

            // число
            if (checkBox_Day_Use_AOD.Checked && comboBox_Day_image_AOD.SelectedIndex >= 0)
            {
                if (ScreenIdle.Date == null) ScreenIdle.Date = new Date();
                if (ScreenIdle.Date.DateDigits == null)
                    ScreenIdle.Date.DateDigits = new List<DigitalDateDigit>();

                DigitalDateDigit digitalDateDigit = new DigitalDateDigit();
                digitalDateDigit.DateType = "Day";
                //digitalTimeDigit.CombingMode = "Single";
                digitalDateDigit.CombingMode = checkBox_Day_follow_AOD.Checked ? "Follow" : "Single";
                digitalDateDigit.Digit = new Text();
                digitalDateDigit.Digit.Image = new ImageAmazfit();
                digitalDateDigit.Digit.Image.X = (long)numericUpDown_DayX_AOD.Value;
                digitalDateDigit.Digit.Image.Y = (long)numericUpDown_DayY_AOD.Value;
                digitalDateDigit.Digit.Image.MultilangImage = new List<MultilangImage>();
                MultilangImage multilangImage = new MultilangImage();
                multilangImage.LangCode = "All";
                multilangImage.ImageSet = new ImageSetGTR2();
                if (comboBox_Day_image_AOD.SelectedIndex >= 0)
                    multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_Day_image_AOD.Text);
                multilangImage.ImageSet.ImagesCount = 10;
                digitalDateDigit.Digit.Image.MultilangImage.Add(multilangImage);
                if (comboBox_Day_separator_AOD.SelectedIndex >= 0)
                {
                    digitalDateDigit.Digit.Image.MultilangImageUnit = new List<MultilangImage>();
                    MultilangImage multilangImageUnit = new MultilangImage();
                    multilangImageUnit.LangCode = "All";
                    multilangImageUnit.ImageSet = new ImageSetGTR2();
                    multilangImageUnit.ImageSet.ImageIndex = Int32.Parse(comboBox_Day_separator_AOD.Text);
                    multilangImageUnit.ImageSet.ImagesCount = 1;
                    digitalDateDigit.Digit.Image.MultilangImageUnit.Add(multilangImageUnit);
                }
                string Alignment = StringToAlignment(comboBox_Day_alignment_AOD.SelectedIndex);
                digitalDateDigit.Digit.Alignment = Alignment;
                digitalDateDigit.Digit.Spacing = (long)numericUpDown_Day_spacing_AOD.Value;
                //digitalTimeDigit.Digit.PaddingZero = checkBox_Day_add_zero.Checked ? 1 : 0;
                digitalDateDigit.Digit.PaddingZero = checkBox_Day_add_zero_AOD.Checked;

                if (comboBox_Day_unit_AOD.SelectedIndex >= 0)
                {
                    digitalDateDigit.Separator = new ImageCoord();
                    digitalDateDigit.Separator.Coordinates = new Coordinates();
                    digitalDateDigit.Separator.Coordinates.X = (long)numericUpDown_Day_unitX_AOD.Value;
                    digitalDateDigit.Separator.Coordinates.Y = (long)numericUpDown_Day_unitY_AOD.Value;
                    digitalDateDigit.Separator.ImageIndex = Int32.Parse(comboBox_Day_unit_AOD.Text);
                }
                ScreenIdle.Date.DateDigits.Add(digitalDateDigit);
            }

            // число стрелкой
            if (checkBox_Day_hand_Use_AOD.Checked && comboBox_Day_hand_image_AOD.SelectedIndex >= 0)
            {
                if (ScreenIdle.Date == null)
                    ScreenIdle.Date = new Date();
                if (ScreenIdle.Date.DateClockHand == null)
                    ScreenIdle.Date.DateClockHand = new DateClockHand();
                ScreenIdle.Date.DateClockHand.DayClockHand = new ClockHand();
                ScreenIdle.Date.DateClockHand.DayClockHand.X = (long)numericUpDown_Day_handX_AOD.Value;
                ScreenIdle.Date.DateClockHand.DayClockHand.Y = (long)numericUpDown_Day_handY_AOD.Value;
                ScreenIdle.Date.DateClockHand.DayClockHand.StartAngle = (float)numericUpDown_Day_hand_startAngle_AOD.Value;
                ScreenIdle.Date.DateClockHand.DayClockHand.EndAngle = (float)numericUpDown_Day_hand_endAngle_AOD.Value;
                ScreenIdle.Date.DateClockHand.DayClockHand.Pointer = new ImageCoord();
                ScreenIdle.Date.DateClockHand.DayClockHand.Pointer.Coordinates = new Coordinates();
                ScreenIdle.Date.DateClockHand.DayClockHand.Pointer.Coordinates.X = (long)numericUpDown_Day_handX_offset_AOD.Value;
                ScreenIdle.Date.DateClockHand.DayClockHand.Pointer.Coordinates.Y = (long)numericUpDown_Day_handY_offset_AOD.Value;
                ScreenIdle.Date.DateClockHand.DayClockHand.Pointer.ImageIndex = Int32.Parse(comboBox_Day_hand_image_AOD.Text);

                if (comboBox_Day_hand_imageCentr_AOD.SelectedIndex >= 0)
                {
                    ScreenIdle.Date.DateClockHand.DayClockHand.Cover = new ImageCoord();
                    ScreenIdle.Date.DateClockHand.DayClockHand.Cover.Coordinates = new Coordinates();
                    ScreenIdle.Date.DateClockHand.DayClockHand.Cover.Coordinates.X = (long)numericUpDown_Day_handX_centr_AOD.Value;
                    ScreenIdle.Date.DateClockHand.DayClockHand.Cover.Coordinates.Y = (long)numericUpDown_Day_handY_centr_AOD.Value;
                    ScreenIdle.Date.DateClockHand.DayClockHand.Cover.ImageIndex = Int32.Parse(comboBox_Day_hand_imageCentr_AOD.Text);
                }

                if (comboBox_Day_hand_imageBackground_AOD.SelectedIndex >= 0)
                {
                    ScreenIdle.Date.DateClockHand.DayClockHand.Scale = new MultilangImageCoord();
                    ScreenIdle.Date.DateClockHand.DayClockHand.Scale.Coordinates = new Coordinates();
                    ScreenIdle.Date.DateClockHand.DayClockHand.Scale.Coordinates.X = (long)numericUpDown_Day_handX_background_AOD.Value;
                    ScreenIdle.Date.DateClockHand.DayClockHand.Scale.Coordinates.Y = (long)numericUpDown_Day_handY_background_AOD.Value;
                    ScreenIdle.Date.DateClockHand.DayClockHand.Scale.ImageSet = new List<MultilangImage>();
                    MultilangImage multilangImage = new MultilangImage();
                    multilangImage.LangCode = "All";
                    multilangImage.ImageSet = new ImageSetGTR2();
                    multilangImage.ImageSet.ImageIndex = Int32.Parse(comboBox_Day_hand_imageBackground_AOD.Text);
                    multilangImage.ImageSet.ImagesCount = 1;
                    ScreenIdle.Date.DateClockHand.DayClockHand.Scale.ImageSet.Add(multilangImage);
                }
            }
            #endregion

            #region активности

            UserControl_pictures userPanel_pictures;
            UserControl_text userPanel_text;
            UserControl_hand userPanel_hand;
            UserControl_scaleCircle userPanel_scaleCircle;
            UserControl_scaleLinear userPanel_scaleLinear;

            #region Battery

            userPanel_pictures = userControl_pictures_Battery_AOD;
            userPanel_text = userControl_text_Battery_AOD;
            userPanel_hand = userControl_hand_Battery_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_Battery_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_Battery_AOD;

            AddActivity_AOD(ScreenIdle, userPanel_pictures, userPanel_text, userPanel_hand,
                userPanel_scaleCircle, userPanel_scaleLinear, "Battery");
            #endregion

            #region Steps

            userPanel_pictures = userControl_pictures_Steps_AOD;
            userPanel_text = userControl_text_Steps_AOD;
            userPanel_hand = userControl_hand_Steps_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_Steps_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_Steps_AOD;

            AddActivity_AOD(ScreenIdle, userPanel_pictures, userPanel_text, userPanel_hand,
                userPanel_scaleCircle, userPanel_scaleLinear, "Steps");


            #endregion

            #region Calories

            userPanel_pictures = userControl_pictures_Calories_AOD;
            userPanel_text = userControl_text_Calories_AOD;
            userPanel_hand = userControl_hand_Calories_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_Calories_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_Calories_AOD;

            AddActivity_AOD(ScreenIdle, userPanel_pictures, userPanel_text, userPanel_hand,
                userPanel_scaleCircle, userPanel_scaleLinear, "Calories");

            #endregion

            #region HeartRate

            userPanel_pictures = userControl_pictures_HeartRate_AOD;
            userPanel_text = userControl_text_HeartRate_AOD;
            userPanel_hand = userControl_hand_HeartRate_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_HeartRate_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_HeartRate_AOD;

            AddActivity_AOD(ScreenIdle, userPanel_pictures, userPanel_text, userPanel_hand,
                userPanel_scaleCircle, userPanel_scaleLinear, "HeartRate");

            #endregion

            #region PAI

            userPanel_pictures = userControl_pictures_PAI_AOD;
            userPanel_text = userControl_text_PAI_AOD;
            userPanel_hand = userControl_hand_PAI_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_PAI_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_PAI_AOD;

            AddActivity_AOD(ScreenIdle, userPanel_pictures, userPanel_text, userPanel_hand,
                userPanel_scaleCircle, userPanel_scaleLinear, "PAI");

            #endregion

            #region Distance

            userPanel_text = userControl_text_Distance_AOD;

            AddActivity_Distance_AOD(ScreenIdle, userPanel_text);

            #endregion

            #region StandUp

            userPanel_pictures = userControl_pictures_StandUp_AOD;
            userPanel_text = userControl_text_StandUp_AOD;
            userPanel_hand = userControl_hand_StandUp_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_StandUp_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_StandUp_AOD;

            AddActivity_AOD(ScreenIdle, userPanel_pictures, userPanel_text, userPanel_hand,
                userPanel_scaleCircle, userPanel_scaleLinear, "StandUp");

            #endregion

            #region Weather

            //panel_pictures = panel_Weather_pictures_AOD;
            //panel_text = panel_Weather_text_AOD;
            //panel_hand = panel_Weather_hand_AOD;
            //panel_scaleCircle = panel_Weather_scaleCircle_AOD;
            //panel_scaleLinear = panel_Weather_scaleLinear_AOD;
            //Panel panel_text_min = panel_Weather_textMin_AOD;
            //Panel panel_text_max = panel_Weather_textMax_AOD;

            AddActivityWeather_AOD_U(ScreenIdle, userControl_pictures_weather_AOD, userControl_text_weather_Current_AOD,
                userControl_text_weather_Min_AOD, userControl_text_weather_Max_AOD, userControl_hand_Weather_AOD,
                userControl_scaleCircle_Weather_AOD, userControl_scaleLinear_Weather_AOD);

            #endregion

            #region UVindex

            userPanel_pictures = userControl_pictures_UVindex_AOD;
            userPanel_text = userControl_text_UVindex_AOD;
            userPanel_hand = userControl_hand_UVindex_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_UVindex_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_UVindex_AOD;

            AddActivity_AOD(ScreenIdle, userPanel_pictures, userPanel_text, userPanel_hand,
                userPanel_scaleCircle, userPanel_scaleLinear, "UVindex");

            #endregion

            #region AirQuality

            userPanel_pictures = userControl_pictures_AirQuality_AOD;
            userPanel_text = userControl_text_AirQuality_AOD;
            userPanel_hand = userControl_hand_AirQuality_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_AirQuality_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_AirQuality_AOD;

            AddActivity_AOD(ScreenIdle, userPanel_pictures, userPanel_text, userPanel_hand,
                userPanel_scaleCircle, userPanel_scaleLinear, "AirQuality");

            #endregion

            #region Humidity

            userPanel_pictures = userControl_pictures_Humidity_AOD;
            userPanel_text = userControl_text_Humidity_AOD;
            userPanel_hand = userControl_hand_Humidity_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_Humidity_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_Humidity_AOD;

            AddActivity_AOD(ScreenIdle, userPanel_pictures, userPanel_text, userPanel_hand,
                userPanel_scaleCircle, userPanel_scaleLinear, "Humidity");

            #endregion

            #region WindForce

            userPanel_pictures = userControl_pictures_WindForce_AOD;
            userPanel_text = userControl_text_WindForce_AOD;
            userPanel_hand = userControl_hand_WindForce_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_WindForce_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_WindForce_AOD;

            AddActivity_AOD(ScreenIdle, userPanel_pictures, userPanel_text, userPanel_hand,
                userPanel_scaleCircle, userPanel_scaleLinear, "WindForce");

            #endregion

            #region Altitude

            userPanel_pictures = userControl_pictures_Altitude_AOD;
            userPanel_text = userControl_text_Altitude_AOD;
            userPanel_hand = userControl_hand_Altitude_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_Altitude_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_Altitude_AOD;

            AddActivity_AOD(ScreenIdle, userPanel_pictures, userPanel_text, userPanel_hand,
                userPanel_scaleCircle, userPanel_scaleLinear, "Altitude");

            #endregion

            #region AirPressure

            userPanel_pictures = userControl_pictures_AirPressure_AOD;
            userPanel_text = userControl_text_AirPressure_AOD;
            userPanel_hand = userControl_hand_AirPressure_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_AirPressure_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_AirPressure_AOD;

            AddActivity_AOD(ScreenIdle, userPanel_pictures, userPanel_text, userPanel_hand,
                userPanel_scaleCircle, userPanel_scaleLinear, "AirPressure");

            #endregion

            #region Stress

            userPanel_pictures = userControl_pictures_Stress_AOD;
            userPanel_text = userControl_text_Stress_AOD;
            userPanel_hand = userControl_hand_Stress_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_Stress_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_Stress_AOD;

            AddActivity_AOD(ScreenIdle, userPanel_pictures, userPanel_text, userPanel_hand,
                userPanel_scaleCircle, userPanel_scaleLinear, "Stress");

            #endregion

            #region ActivityGoal

            userPanel_pictures = userControl_pictures_ActivityGoal_AOD;
            userPanel_text = userControl_text_ActivityGoal_AOD;
            userPanel_hand = userControl_hand_ActivityGoal_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_ActivityGoal_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_ActivityGoal_AOD;

            AddActivity_AOD(ScreenIdle, userPanel_pictures, userPanel_text, userPanel_hand,
                userPanel_scaleCircle, userPanel_scaleLinear, "ActivityGoal");

            #endregion

            #region FatBurning

            userPanel_pictures = userControl_pictures_FatBurning_AOD;
            userPanel_text = userControl_text_FatBurning_AOD;
            userPanel_hand = userControl_hand_FatBurning_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_FatBurning_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_FatBurning_AOD;

            AddActivity_AOD(ScreenIdle, userPanel_pictures, userPanel_text, userPanel_hand,
                userPanel_scaleCircle, userPanel_scaleLinear, "FatBurning");

            #endregion

            #endregion



            if (ScreenIdle.Activity != null || ScreenIdle.BackgroundImageIndex != null || 
                ScreenIdle.Date != null || ScreenIdle.DialFace != null) Watch_Face.ScreenIdle = ScreenIdle;
        }

        private void AddActivity_AOD(ScreenIdle ScreenIdle, UserControl_pictures panel_pictures, 
            UserControl_text panel_text, UserControl_hand panel_hand, UserControl_scaleCircle panel_scaleCircle,
            UserControl_scaleLinear panel_scaleLinear, string type)
        {
            Activity activity = null;

            // данные картинками
            //checkBox_Use = (CheckBox)panel_pictures.checkBox_pictures_Use;
            if (panel_pictures.checkBox_pictures_Use.Checked)
            {
                //ComboBox comboBox_image = (ComboBox)panel_pictures.Controls[1];
                int image = panel_pictures.comboBoxGetImage();
                if (image >= 0)
                {
                    NumericUpDown numericUpDownX = (NumericUpDown)panel_pictures.numericUpDown_picturesX;
                    NumericUpDown numericUpDownY = (NumericUpDown)panel_pictures.numericUpDown_picturesY;
                    NumericUpDown numericUpDown_count = (NumericUpDown)panel_pictures.numericUpDown_pictures_count;

                    if (activity == null) activity = new Activity();
                    activity.ImageProgress = new ImageProgress();
                    activity.ImageProgress.ImageSet = new ImageSetGTR2();
                    activity.ImageProgress.Coordinates = new List<Coordinates>();
                    activity.ImageProgress.ImageSet.ImageIndex = image;
                    activity.ImageProgress.ImageSet.ImagesCount = (long)numericUpDown_count.Value;
                    Coordinates coordinates = new Coordinates();
                    coordinates.X = (long)numericUpDownX.Value;
                    coordinates.Y = (long)numericUpDownY.Value;
                    activity.ImageProgress.Coordinates.Add(coordinates);
                }
            }

            // данные надписью
            //checkBox_Use = (CheckBox)panel_text.checkBox_Use;
            if (panel_text.checkBox_Use.Checked)
            {
                //ComboBox comboBox_image = (ComboBox)panel_text.Controls[1];
                int image = panel_text.comboBoxGetImage();
                if (image >= 0)
                {
                    //ComboBox comboBox_unit = (ComboBox)panel_text.Controls[2];
                    //ComboBox comboBox_separator = (ComboBox)panel_text.Controls[3];
                    int unit = panel_text.comboBoxGetIcon();
                    int separator = panel_text.comboBoxGetUnit();
                    NumericUpDown numericUpDownX = panel_text.numericUpDown_imageX;
                    NumericUpDown numericUpDownY = panel_text.numericUpDown_imageY;
                    NumericUpDown numericUpDown_unitX = panel_text.numericUpDown_iconX;
                    NumericUpDown numericUpDown_unitY = panel_text.numericUpDown_iconY;
                    string Alignment = panel_text.comboBoxGetAlignment();
                    NumericUpDown numericUpDown_spacing = panel_text.numericUpDown_spacing;
                    bool add_zero = panel_text.checkBox_addZero.Checked;
                    int imageError = panel_text.comboBoxGetImageError();

                    if (activity == null) activity = new Activity();
                    activity.Digits = new List<DigitalCommonDigit>();
                    DigitalCommonDigit digitalCommonDigit = new DigitalCommonDigit();
                    digitalCommonDigit.CombingMode = "Single";
                    digitalCommonDigit.Digit = new Text();
                    //string Alignment = StringToAlignment(comboBox_alignment.SelectedIndex);
                    digitalCommonDigit.Digit.Alignment = Alignment;
                    digitalCommonDigit.Digit.PaddingZero = add_zero;
                    digitalCommonDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;
                    digitalCommonDigit.Digit.Image = new ImageAmazfit();
                    if (imageError >= 0)
                        digitalCommonDigit.Digit.Image.NoDataImageIndex = imageError;

                    digitalCommonDigit.Digit.Image.X = (long)numericUpDownX.Value;
                    digitalCommonDigit.Digit.Image.Y = (long)numericUpDownY.Value;
                    if (type == "Distance")
                    {
                        int DecimalPoint = panel_text.comboBoxGetImageDecimalPointOrMinus();
                        if (DecimalPoint >= 0)
                        {
                            digitalCommonDigit.Digit.Image.DecimalPointImageIndex = DecimalPoint;
                        }
                    }
                    digitalCommonDigit.Digit.Image.MultilangImage = new List<MultilangImage>();
                    MultilangImage multilangImage = new MultilangImage();
                    multilangImage.LangCode = "All";
                    multilangImage.ImageSet = new ImageSetGTR2();
                    multilangImage.ImageSet.ImagesCount = 10;
                    multilangImage.ImageSet.ImageIndex = image;
                    digitalCommonDigit.Digit.Image.MultilangImage.Add(multilangImage);

                    if (separator >= 0)
                    {
                        digitalCommonDigit.Digit.Image.MultilangImageUnit = new List<MultilangImage>();
                        multilangImage = new MultilangImage();
                        multilangImage.LangCode = "All";
                        multilangImage.ImageSet = new ImageSetGTR2();
                        multilangImage.ImageSet.ImagesCount = 1;
                        multilangImage.ImageSet.ImageIndex = separator;
                        digitalCommonDigit.Digit.Image.MultilangImageUnit.Add(multilangImage);
                    }

                    if (unit >= 0)
                    {
                        digitalCommonDigit.Separator = new ImageCoord();
                        digitalCommonDigit.Separator.ImageIndex = unit;
                        digitalCommonDigit.Separator.Coordinates = new Coordinates();
                        digitalCommonDigit.Separator.Coordinates.X = (long)numericUpDown_unitX.Value;
                        digitalCommonDigit.Separator.Coordinates.Y = (long)numericUpDown_unitY.Value;
                    }

                    activity.Digits.Add(digitalCommonDigit);
                }
            }

            // данные стрелкой
            //checkBox_Use = (CheckBox)panel_hand.Controls[0];
            if (panel_hand.checkBox_hand_Use.Checked)
            {
                //ComboBox comboBox_image = (ComboBox)panel_hand.Controls[1];
                int image = panel_hand.comboBoxGetHandImage();
                if (image >= 0)
                {
                    NumericUpDown numericUpDownX = panel_hand.numericUpDown_handX;
                    NumericUpDown numericUpDownY = panel_hand.numericUpDown_handY;
                    NumericUpDown numericUpDown_offsetX = panel_hand.numericUpDown_handX_offset;
                    NumericUpDown numericUpDown_offsetY = panel_hand.numericUpDown_handY_offset;
                    int imageCentr = panel_hand.comboBoxGetHandImageCentr();
                    NumericUpDown numericUpDownX_centr = panel_hand.numericUpDown_handX_centr;
                    NumericUpDown numericUpDownY_centr = panel_hand.numericUpDown_handY_centr;
                    NumericUpDown numericUpDown_startAngle = panel_hand.numericUpDown_hand_startAngle;
                    NumericUpDown numericUpDown_endAngle = panel_hand.numericUpDown_hand_endAngle;
                    int imageBackground = panel_hand.comboBoxGetHandImageBackground();
                    NumericUpDown numericUpDownX_background = panel_hand.numericUpDown_handX_background;
                    NumericUpDown numericUpDownY_background = panel_hand.numericUpDown_handY_background;

                    if (activity == null) activity = new Activity();
                    activity.PointerProgress = new ClockHand();
                    activity.PointerProgress.X = (long)numericUpDownX.Value;
                    activity.PointerProgress.Y = (long)numericUpDownY.Value;
                    activity.PointerProgress.StartAngle = (float)numericUpDown_startAngle.Value;
                    activity.PointerProgress.EndAngle = (float)numericUpDown_endAngle.Value;

                    activity.PointerProgress.Pointer = new ImageCoord();
                    activity.PointerProgress.Pointer.ImageIndex = image;
                    activity.PointerProgress.Pointer.Coordinates = new Coordinates();
                    activity.PointerProgress.Pointer.Coordinates.X = (long)numericUpDown_offsetX.Value;
                    activity.PointerProgress.Pointer.Coordinates.Y = (long)numericUpDown_offsetY.Value;

                    if (imageCentr >= 0)
                    {
                        activity.PointerProgress.Cover = new ImageCoord();
                        activity.PointerProgress.Cover.ImageIndex = imageCentr;
                        activity.PointerProgress.Cover.Coordinates = new Coordinates();
                        activity.PointerProgress.Cover.Coordinates.X = (long)numericUpDownX_centr.Value;
                        activity.PointerProgress.Cover.Coordinates.Y = (long)numericUpDownY_centr.Value;
                    }

                    if (imageBackground >= 0)
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
                        multilangImage.ImageSet.ImageIndex = imageBackground;
                        activity.PointerProgress.Scale.ImageSet.Add(multilangImage);
                    }
                }
            }

            // данные круговой шкалой
            //checkBox_Use = (CheckBox)panel_scaleCircle.Controls[0];
            if (panel_scaleCircle.checkBox_scaleCircle_Use.Checked)
            {
                RadioButton radioButton_image = panel_scaleCircle.radioButton_scaleCircle_image;
                //RadioButton radioButton_color = (RadioButton)panel_scaleCircle.Controls[2];
                int image = panel_scaleCircle.comboBoxGetImage();
                string color = panel_scaleCircle.comboBoxGetColorString();
                int flatness = panel_scaleCircle.comboBoxGetFlatness();
                int background = panel_scaleCircle.comboBoxGetImageBackground();
                NumericUpDown numericUpDownX = panel_scaleCircle.numericUpDown_scaleCircleX;
                NumericUpDown numericUpDownY = panel_scaleCircle.numericUpDown_scaleCircleY;
                NumericUpDown numericUpDown_radius = panel_scaleCircle.numericUpDown_scaleCircle_radius;
                NumericUpDown numericUpDown_width = panel_scaleCircle.numericUpDown_scaleCircle_width;
                NumericUpDown numericUpDown_startAngle = panel_scaleCircle.numericUpDown_scaleCircle_startAngle;
                NumericUpDown numericUpDown_endAngle = panel_scaleCircle.numericUpDown_scaleCircle_endAngle;

                if ((radioButton_image.Checked && image >= 0) || (!radioButton_image.Checked))
                {
                    if (activity == null) activity = new Activity();
                    if (activity.ProgressBar == null) activity.ProgressBar = new ProgressBar();
                    activity.ProgressBar.AngleSettings = new AngleSettings();
                    if (radioButton_image.Checked && image >= 0)
                    {
                        activity.ProgressBar.ForegroundImageIndex = image;
                    }
                    else
                    {
                        //Color color = comboBox_color.BackColor;
                        //Color new_color = Color.FromArgb(0, color.R, color.G, color.B);
                        //string colorStr = ColorTranslator.ToHtml(new_color);
                        //colorStr = colorStr.Replace("#", "0xFF");
                        //activity.ProgressBar.Color = colorStr;
                        activity.ProgressBar.Color = color;
                    }

                    if (background >= 0) activity.ProgressBar.BackgroundImageIndex = background;

                    activity.ProgressBar.AngleSettings.X = (long)numericUpDownX.Value;
                    activity.ProgressBar.AngleSettings.Y = (long)numericUpDownY.Value;
                    activity.ProgressBar.AngleSettings.StartAngle = (float)numericUpDown_startAngle.Value;
                    activity.ProgressBar.AngleSettings.EndAngle = (float)numericUpDown_endAngle.Value;
                    activity.ProgressBar.AngleSettings.Radius = (float)numericUpDown_radius.Value;

                    activity.ProgressBar.Width = (long)numericUpDown_width.Value;

                    //switch (comboBox_flatness.SelectedIndex)
                    //{
                    //    case 1:
                    //        activity.ProgressBar.Flatness = 90;
                    //        break;
                    //    case 2:
                    //        activity.ProgressBar.Flatness = 180;
                    //        break;
                    //    default:
                    //        activity.ProgressBar.Flatness = 0;
                    //        break;
                    //}
                    activity.ProgressBar.Flatness = flatness;
                }
            }

            // данные линейной шкалой
            //checkBox_Use = (CheckBox)panel_scaleLinear.Controls[0];
            if (panel_scaleLinear.checkBox_scaleLinear_Use.Checked)
            {
                RadioButton radioButton_image = (RadioButton)panel_scaleLinear.radioButton_scaleLinear_image;
                //RadioButton radioButton_color = (RadioButton)panel_scaleLinear.Controls[2];
                int image = panel_scaleLinear.comboBoxGetImage();
                string color = panel_scaleLinear.comboBoxGetColorString();
                int pointer = panel_scaleLinear.comboBoxGetImagePointer();
                int background = panel_scaleLinear.comboBoxGetImageBackground();
                NumericUpDown numericUpDownX = panel_scaleLinear.numericUpDown_scaleLinearX;
                NumericUpDown numericUpDownY = panel_scaleLinear.numericUpDown_scaleLinearY;
                NumericUpDown numericUpDown_length = panel_scaleLinear.numericUpDown_scaleLinear_length;
                NumericUpDown numericUpDown_width = panel_scaleLinear.numericUpDown_scaleLinear_width;
                int flatness = panel_scaleLinear.comboBoxGetFlatness();

                if ((radioButton_image.Checked && image >= 0) ||
                    (!radioButton_image.Checked))
                {
                    if (activity == null) activity = new Activity();
                    if (activity.ProgressBar == null) activity.ProgressBar = new ProgressBar();
                    activity.ProgressBar.LinearSettings = new LinearSettings();
                    if (radioButton_image.Checked && image >= 0)
                    {
                        activity.ProgressBar.ForegroundImageIndex = image;
                    }
                    else
                    {
                        //Color color = comboBox_color.BackColor;
                        //Color new_color = Color.FromArgb(0, color.R, color.G, color.B);
                        //string colorStr = ColorTranslator.ToHtml(new_color);
                        //colorStr = colorStr.Replace("#", "0xFF");
                        //activity.ProgressBar.Color = colorStr;
                        activity.ProgressBar.Color = color;
                    }
                    if (pointer >= 0) activity.ProgressBar.PointerImageIndex = pointer;
                    if (background >= 0) activity.ProgressBar.BackgroundImageIndex = background;

                    activity.ProgressBar.LinearSettings.StartX = (long)numericUpDownX.Value;
                    activity.ProgressBar.LinearSettings.StartY = (long)numericUpDownY.Value;
                    long endX = (long)(numericUpDownX.Value + numericUpDown_length.Value);
                    activity.ProgressBar.LinearSettings.EndX = endX;
                    activity.ProgressBar.LinearSettings.EndY = (long)numericUpDownY.Value;
                    activity.ProgressBar.Width = (long)numericUpDown_width.Value;
                    //switch (comboBox_flatness.SelectedIndex)
                    //{
                    //    case 1:
                    //        activity.ProgressBar.Flatness = 180;
                    //        break;
                    //    default:
                    //        activity.ProgressBar.Flatness = 0;
                    //        break;
                    //}
                    activity.ProgressBar.Flatness = flatness;
                }

            }

            if (activity != null)
            {
                activity.Type = type;
                if (ScreenIdle.Activity == null) ScreenIdle.Activity = new List<Activity>();
                ScreenIdle.Activity.Add(activity);
            }
        }

        private void AddActivity_Distance_AOD(ScreenIdle ScreenIdle, UserControl_text panel_text)
        {
            Activity activity = null;

            // данные надписью
            //checkBox_Use = (CheckBox)panel_text.checkBox_Use;
            if (panel_text.checkBox_Use.Checked)
            {
                //ComboBox comboBox_image = (ComboBox)panel_text.Controls[1];
                int image = panel_text.comboBoxGetImage();
                if (image >= 0)
                {
                    //ComboBox comboBox_unit = (ComboBox)panel_text.Controls[2];
                    //ComboBox comboBox_separator = (ComboBox)panel_text.Controls[3];
                    int unit = panel_text.comboBoxGetIcon();
                    int separator = panel_text.comboBoxGetUnit();
                    NumericUpDown numericUpDownX = panel_text.numericUpDown_imageX;
                    NumericUpDown numericUpDownY = panel_text.numericUpDown_imageY;
                    NumericUpDown numericUpDown_unitX = panel_text.numericUpDown_iconX;
                    NumericUpDown numericUpDown_unitY = panel_text.numericUpDown_iconY;
                    string Alignment = panel_text.comboBoxGetAlignment();
                    NumericUpDown numericUpDown_spacing = panel_text.numericUpDown_spacing;
                    bool add_zero = panel_text.checkBox_addZero.Checked;
                    int imageError = panel_text.comboBoxGetImageError();

                    if (activity == null) activity = new Activity();
                    activity.Digits = new List<DigitalCommonDigit>();
                    DigitalCommonDigit digitalCommonDigit = new DigitalCommonDigit();
                    digitalCommonDigit.CombingMode = "Single";
                    digitalCommonDigit.Digit = new Text();
                    //string Alignment = StringToAlignment(comboBox_alignment.SelectedIndex);
                    digitalCommonDigit.Digit.Alignment = Alignment;
                    digitalCommonDigit.Digit.PaddingZero = add_zero;
                    digitalCommonDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;
                    digitalCommonDigit.Digit.Image = new ImageAmazfit();
                    if (imageError >= 0)
                        digitalCommonDigit.Digit.Image.NoDataImageIndex = imageError;

                    digitalCommonDigit.Digit.Image.X = (long)numericUpDownX.Value;
                    digitalCommonDigit.Digit.Image.Y = (long)numericUpDownY.Value;
                    int DecimalPoint = panel_text.comboBoxGetImageDecimalPointOrMinus();
                    if (DecimalPoint >= 0)
                    {
                        digitalCommonDigit.Digit.Image.DecimalPointImageIndex = DecimalPoint;
                    }
                    digitalCommonDigit.Digit.Image.MultilangImage = new List<MultilangImage>();
                    MultilangImage multilangImage = new MultilangImage();
                    multilangImage.LangCode = "All";
                    multilangImage.ImageSet = new ImageSetGTR2();
                    multilangImage.ImageSet.ImagesCount = 10;
                    multilangImage.ImageSet.ImageIndex = image;
                    digitalCommonDigit.Digit.Image.MultilangImage.Add(multilangImage);

                    if (separator >= 0)
                    {
                        digitalCommonDigit.Digit.Image.MultilangImageUnit = new List<MultilangImage>();
                        multilangImage = new MultilangImage();
                        multilangImage.LangCode = "All";
                        multilangImage.ImageSet = new ImageSetGTR2();
                        multilangImage.ImageSet.ImagesCount = 1;
                        multilangImage.ImageSet.ImageIndex = separator;
                        digitalCommonDigit.Digit.Image.MultilangImageUnit.Add(multilangImage);
                    }

                    if (unit >= 0)
                    {
                        digitalCommonDigit.Separator = new ImageCoord();
                        digitalCommonDigit.Separator.ImageIndex = unit;
                        digitalCommonDigit.Separator.Coordinates = new Coordinates();
                        digitalCommonDigit.Separator.Coordinates.X = (long)numericUpDown_unitX.Value;
                        digitalCommonDigit.Separator.Coordinates.Y = (long)numericUpDown_unitY.Value;
                    }

                    activity.Digits.Add(digitalCommonDigit);
                }
            }

            if (activity != null)
            {
                activity.Type = "Distance";
                if (ScreenIdle.Activity == null) ScreenIdle.Activity = new List<Activity>();
                ScreenIdle.Activity.Add(activity);
            }
        }


        private void AddActivityWeather_AOD(ScreenIdle ScreenIdle, Panel panel_pictures, Panel panel_text, Panel panel_text_min, Panel panel_text_max, Panel panel_hand, Panel panel_scaleCircle, Panel panel_scaleLinear)
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
                    string Alignment = StringToAlignment(comboBox_alignment.SelectedIndex);
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
                    string Alignment = StringToAlignment(comboBox_alignment.SelectedIndex);
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
                    string Alignment = StringToAlignment(comboBox_alignment.SelectedIndex);
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
                if (ScreenIdle.Activity == null) ScreenIdle.Activity = new List<Activity>();
                ScreenIdle.Activity.Add(activity);
            }
        }

        private void AddActivityWeather_AOD_U(ScreenIdle ScreenIdle, UserControl_pictures panel_pictures, 
            UserControl_text_weather panel_text, UserControl_text_weather panel_text_min,
            UserControl_text_weather panel_text_max, UserControl_hand panel_hand, 
            UserControl_scaleCircle panel_scaleCircle, UserControl_scaleLinear panel_scaleLinear)
        {
            Activity activity = null;

            // данные картинками
            if (panel_pictures.checkBox_pictures_Use.Checked)
            {
                if (panel_pictures.comboBoxGetSelectedIndexImage() >= 0)
                {
                    NumericUpDown numericUpDownX = panel_pictures.numericUpDown_picturesX;
                    NumericUpDown numericUpDownY = panel_pictures.numericUpDown_picturesY;

                    if (activity == null) activity = new Activity();
                    activity.ImageProgress = new ImageProgress();
                    activity.ImageProgress.ImageSet = new ImageSetGTR2();
                    activity.ImageProgress.Coordinates = new List<Coordinates>();
                    activity.ImageProgress.ImageSet.ImageIndex = panel_pictures.comboBoxGetImage();
                    activity.ImageProgress.ImageSet.ImagesCount = 29;
                    //activity.ImageProgress.ImageSet.ImagesCount = (long)numericUpDown_count.Value;
                    Coordinates coordinates = new Coordinates();
                    coordinates.X = (long)numericUpDownX.Value;
                    coordinates.Y = (long)numericUpDownY.Value;
                    activity.ImageProgress.Coordinates.Add(coordinates);
                }
            }

            // данные надписью
            if (panel_text.checkBox_Use.Checked)
            {
                if (panel_text.comboBoxGetSelectedIndexImage() >= 0)
                {
                    //ComboBox comboBox_unit = (ComboBox)panel_text.Controls[2];
                    //ComboBox comboBox_separatorF = (ComboBox)panel_text.Controls[3];
                    NumericUpDown numericUpDownX = panel_text.numericUpDown_imageX;
                    NumericUpDown numericUpDownY = panel_text.numericUpDown_imageY;
                    NumericUpDown numericUpDown_unitX = panel_text.numericUpDown_iconX;
                    NumericUpDown numericUpDown_unitY = panel_text.numericUpDown_iconY;
                    //ComboBox comboBox_alignment = (ComboBox)panel_text.Controls[8];
                    NumericUpDown numericUpDown_spacing = panel_text.numericUpDown_spacing;
                    CheckBox checkBox_add_zero = panel_text.checkBox_addZero;
                    //ComboBox comboBox_imageError = (ComboBox)panel_text.Controls[10];
                    //ComboBox comboBox_imageMinus = (ComboBox)panel_text.Controls[11];

                    if (activity == null) activity = new Activity();
                    activity.Digits = new List<DigitalCommonDigit>();
                    DigitalCommonDigit digitalCommonDigit = new DigitalCommonDigit();
                    digitalCommonDigit.CombingMode = "Single";
                    digitalCommonDigit.Digit = new Text();
                    //string Alignment = StringToAlignment(comboBox_alignment.SelectedIndex);
                    digitalCommonDigit.Digit.Alignment = panel_text.comboBoxGetAlignment();
                    digitalCommonDigit.Digit.PaddingZero = checkBox_add_zero.Checked;
                    digitalCommonDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;
                    digitalCommonDigit.Digit.Image = new ImageAmazfit();

                    if (panel_text.comboBoxGetImageError() >= 0)
                        digitalCommonDigit.Digit.Image.NoDataImageIndex = panel_text.comboBoxGetImageError();

                    if (panel_text.comboBoxGetSelectedIndexImageDecimalPointOrMinus() >= 0)
                        digitalCommonDigit.Digit.Image.DelimiterImageIndex = panel_text.comboBoxGetImageDecimalPointOrMinus();

                    digitalCommonDigit.Digit.Image.X = (long)numericUpDownX.Value;
                    digitalCommonDigit.Digit.Image.Y = (long)numericUpDownY.Value;

                    digitalCommonDigit.Digit.Image.MultilangImage = new List<MultilangImage>();
                    MultilangImage multilangImage = new MultilangImage();
                    multilangImage.LangCode = "All";
                    multilangImage.ImageSet = new ImageSetGTR2();
                    multilangImage.ImageSet.ImagesCount = 10;
                    multilangImage.ImageSet.ImageIndex = panel_text.comboBoxGetImage();
                    digitalCommonDigit.Digit.Image.MultilangImage.Add(multilangImage);

                    if (panel_text.comboBoxGetSelectedIndexUnit() >= 0)
                    {
                        if (digitalCommonDigit.Digit.Image.MultilangImageUnit == null)
                            digitalCommonDigit.Digit.Image.MultilangImageUnit = new List<MultilangImage>();
                        multilangImage = new MultilangImage();
                        multilangImage.LangCode = "All";
                        multilangImage.ImageSet = new ImageSetGTR2();
                        multilangImage.ImageSet.ImagesCount = 1;
                        multilangImage.ImageSet.ImageIndex = panel_text.comboBoxGetUnit();
                        digitalCommonDigit.Digit.Image.MultilangImageUnit.Add(multilangImage);
                    }

                    if (panel_text.comboBoxGetSelectedIndexIcon() >= 0)
                    {
                        digitalCommonDigit.Separator = new ImageCoord();
                        digitalCommonDigit.Separator.ImageIndex = panel_text.comboBoxGetIcon();
                        digitalCommonDigit.Separator.Coordinates = new Coordinates();
                        digitalCommonDigit.Separator.Coordinates.X = (long)numericUpDown_unitX.Value;
                        digitalCommonDigit.Separator.Coordinates.Y = (long)numericUpDown_unitY.Value;
                    }

                    activity.Digits.Add(digitalCommonDigit);
                }
            }

            // данные надписью min temperature
            if (panel_text_min.checkBox_Use.Checked)
            {
                if (panel_text_min.comboBoxGetSelectedIndexImage() >= 0)
                {
                    //ComboBox comboBox_unit = (ComboBox)panel_text_min.Controls[2];
                    //ComboBox comboBox_separatorF = (ComboBox)panel_text_min.Controls[3];
                    NumericUpDown numericUpDownX = panel_text_min.numericUpDown_imageX;
                    NumericUpDown numericUpDownY = panel_text_min.numericUpDown_imageY;
                    NumericUpDown numericUpDown_unitX = panel_text_min.numericUpDown_iconX;
                    NumericUpDown numericUpDown_unitY = panel_text_min.numericUpDown_iconY;
                    //ComboBox comboBox_alignment = (ComboBox)panel_text_min.Controls[8];
                    NumericUpDown numericUpDown_spacing = panel_text_min.numericUpDown_spacing;
                    CheckBox checkBox_add_zero = panel_text_min.checkBox_addZero;
                    //ComboBox comboBox_imageError = (ComboBox)panel_text_min.Controls[10];
                    //ComboBox comboBox_imageMinus = (ComboBox)panel_text_min.Controls[11];

                    if (activity == null) activity = new Activity();
                    if (activity.Digits == null) activity.Digits = new List<DigitalCommonDigit>();
                    DigitalCommonDigit digitalCommonDigit = new DigitalCommonDigit();
                    digitalCommonDigit.Type = "Min";
                    digitalCommonDigit.CombingMode = "Single";
                    digitalCommonDigit.Digit = new Text();
                    //string Alignment = StringToAlignment(comboBox_alignment.SelectedIndex);
                    digitalCommonDigit.Digit.Alignment = panel_text_min.comboBoxGetAlignment();
                    digitalCommonDigit.Digit.PaddingZero = checkBox_add_zero.Checked;
                    digitalCommonDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;
                    digitalCommonDigit.Digit.Image = new ImageAmazfit();

                    if (panel_text_min.comboBoxGetImageError() >= 0)
                        digitalCommonDigit.Digit.Image.NoDataImageIndex = panel_text_min.comboBoxGetImageError();

                    if (panel_text_min.comboBoxGetSelectedIndexImageDecimalPointOrMinus() >= 0)
                        digitalCommonDigit.Digit.Image.DelimiterImageIndex = panel_text_min.comboBoxGetImageDecimalPointOrMinus();

                    digitalCommonDigit.Digit.Image.X = (long)numericUpDownX.Value;
                    digitalCommonDigit.Digit.Image.Y = (long)numericUpDownY.Value;

                    digitalCommonDigit.Digit.Image.MultilangImage = new List<MultilangImage>();
                    MultilangImage multilangImage = new MultilangImage();
                    multilangImage.LangCode = "All";
                    multilangImage.ImageSet = new ImageSetGTR2();
                    multilangImage.ImageSet.ImagesCount = 10;
                    multilangImage.ImageSet.ImageIndex = panel_text_min.comboBoxGetImage();
                    digitalCommonDigit.Digit.Image.MultilangImage.Add(multilangImage);

                    if (panel_text_min.comboBoxGetSelectedIndexUnit() >= 0)
                    {
                        if (digitalCommonDigit.Digit.Image.MultilangImageUnit == null)
                            digitalCommonDigit.Digit.Image.MultilangImageUnit = new List<MultilangImage>();
                        multilangImage = new MultilangImage();
                        multilangImage.LangCode = "All";
                        multilangImage.ImageSet = new ImageSetGTR2();
                        multilangImage.ImageSet.ImagesCount = 1;
                        multilangImage.ImageSet.ImageIndex = panel_text_min.comboBoxGetUnit();
                        digitalCommonDigit.Digit.Image.MultilangImageUnit.Add(multilangImage);
                    }

                    if (panel_text_min.comboBoxGetSelectedIndexIcon() >= 0)
                    {
                        digitalCommonDigit.Separator = new ImageCoord();
                        digitalCommonDigit.Separator.ImageIndex = panel_text_min.comboBoxGetIcon();
                        digitalCommonDigit.Separator.Coordinates = new Coordinates();
                        digitalCommonDigit.Separator.Coordinates.X = (long)numericUpDown_unitX.Value;
                        digitalCommonDigit.Separator.Coordinates.Y = (long)numericUpDown_unitY.Value;
                    }

                    activity.Digits.Add(digitalCommonDigit);
                }
            }

            // данные надписью max temperature
            if (panel_text_max.checkBox_Use.Checked)
            {
                if (panel_text_max.comboBoxGetSelectedIndexImage() >= 0)
                {
                    //ComboBox comboBox_unit = (ComboBox)panel_text_max.Controls[2];
                    //ComboBox comboBox_separatorF = (ComboBox)panel_text_max.Controls[3];
                    NumericUpDown numericUpDownX = panel_text_max.numericUpDown_imageX;
                    NumericUpDown numericUpDownY = panel_text_max.numericUpDown_imageY;
                    NumericUpDown numericUpDown_unitX = panel_text_max.numericUpDown_iconX;
                    NumericUpDown numericUpDown_unitY = panel_text_max.numericUpDown_iconY;
                    //ComboBox comboBox_alignment = (ComboBox)panel_text_max.Controls[8];
                    NumericUpDown numericUpDown_spacing = panel_text_max.numericUpDown_spacing;
                    CheckBox checkBox_add_zero = panel_text_max.checkBox_addZero;
                    //ComboBox comboBox_imageError = (ComboBox)panel_text_max.Controls[10];
                    //ComboBox comboBox_imageMinus = (ComboBox)panel_text_max.Controls[11];
                    CheckBox checkBox_follow = panel_text_max.checkBox_follow;

                    if (activity == null) activity = new Activity();
                    if (activity.Digits == null) activity.Digits = new List<DigitalCommonDigit>();
                    DigitalCommonDigit digitalCommonDigit = new DigitalCommonDigit();
                    digitalCommonDigit.Type = "Max";
                    if (!checkBox_follow.Checked) digitalCommonDigit.CombingMode = "Single";
                    //digitalCommonDigit.CombingMode = "Single";
                    digitalCommonDigit.Digit = new Text();
                    //string Alignment = StringToAlignment(comboBox_alignment.SelectedIndex);
                    digitalCommonDigit.Digit.Alignment = panel_text_max.comboBoxGetAlignment();
                    digitalCommonDigit.Digit.PaddingZero = checkBox_add_zero.Checked;
                    digitalCommonDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;
                    digitalCommonDigit.Digit.Image = new ImageAmazfit();

                    if (panel_text_max.comboBoxGetImageError() >= 0)
                        digitalCommonDigit.Digit.Image.NoDataImageIndex = panel_text_max.comboBoxGetImageError();

                    if (panel_text_max.comboBoxGetSelectedIndexImageDecimalPointOrMinus() >= 0)
                        digitalCommonDigit.Digit.Image.DelimiterImageIndex = panel_text_max.comboBoxGetImageDecimalPointOrMinus();

                    digitalCommonDigit.Digit.Image.X = (long)numericUpDownX.Value;
                    digitalCommonDigit.Digit.Image.Y = (long)numericUpDownY.Value;

                    digitalCommonDigit.Digit.Image.MultilangImage = new List<MultilangImage>();
                    MultilangImage multilangImage = new MultilangImage();
                    multilangImage.LangCode = "All";
                    multilangImage.ImageSet = new ImageSetGTR2();
                    multilangImage.ImageSet.ImagesCount = 10;
                    multilangImage.ImageSet.ImageIndex = panel_text_max.comboBoxGetImage();
                    digitalCommonDigit.Digit.Image.MultilangImage.Add(multilangImage);

                    if (panel_text_max.comboBoxGetSelectedIndexUnit() >= 0)
                    {
                        if (digitalCommonDigit.Digit.Image.MultilangImageUnit == null)
                            digitalCommonDigit.Digit.Image.MultilangImageUnit = new List<MultilangImage>();
                        multilangImage = new MultilangImage();
                        multilangImage.LangCode = "All";
                        multilangImage.ImageSet = new ImageSetGTR2();
                        multilangImage.ImageSet.ImagesCount = 1;
                        multilangImage.ImageSet.ImageIndex = panel_text_max.comboBoxGetUnit();
                        digitalCommonDigit.Digit.Image.MultilangImageUnit.Add(multilangImage);
                    }

                    if (panel_text_max.comboBoxGetSelectedIndexIcon() >= 0)
                    {
                        digitalCommonDigit.Separator = new ImageCoord();
                        digitalCommonDigit.Separator.ImageIndex = panel_text_max.comboBoxGetIcon();
                        digitalCommonDigit.Separator.Coordinates = new Coordinates();
                        digitalCommonDigit.Separator.Coordinates.X = (long)numericUpDown_unitX.Value;
                        digitalCommonDigit.Separator.Coordinates.Y = (long)numericUpDown_unitY.Value;
                    }

                    activity.Digits.Add(digitalCommonDigit);
                }
            }

            // данные стрелкой
            if (panel_hand.checkBox_hand_Use.Checked)
            {
                if (panel_hand.comboBoxGetSelectedIndexHandImage() >= 0)
                {
                    NumericUpDown numericUpDownX = panel_hand.numericUpDown_handX;
                    NumericUpDown numericUpDownY = panel_hand.numericUpDown_handY;
                    NumericUpDown numericUpDown_offsetX = panel_hand.numericUpDown_handX_offset;
                    NumericUpDown numericUpDown_offsetY = panel_hand.numericUpDown_handY_offset;
                    //ComboBox comboBox_imageCentr = (ComboBox)panel_hand.Controls[6];
                    NumericUpDown numericUpDownX_centr = panel_hand.numericUpDown_handX_centr;
                    NumericUpDown numericUpDownY_centr = panel_hand.numericUpDown_handY_centr;
                    NumericUpDown numericUpDown_startAngle = panel_hand.numericUpDown_hand_startAngle;
                    NumericUpDown numericUpDown_endAngle = panel_hand.numericUpDown_hand_endAngle;
                    //ComboBox comboBox_imageBackground = (ComboBox)panel_hand.Controls[11];
                    NumericUpDown numericUpDownX_background = panel_hand.numericUpDown_handX_background;
                    NumericUpDown numericUpDownY_background = panel_hand.numericUpDown_handY_background;

                    if (activity == null) activity = new Activity();
                    activity.PointerProgress = new ClockHand();
                    activity.PointerProgress.X = (long)numericUpDownX.Value;
                    activity.PointerProgress.Y = (long)numericUpDownY.Value;
                    activity.PointerProgress.StartAngle = (float)numericUpDown_startAngle.Value;
                    activity.PointerProgress.EndAngle = (float)numericUpDown_endAngle.Value;

                    activity.PointerProgress.Pointer = new ImageCoord();
                    activity.PointerProgress.Pointer.ImageIndex = panel_hand.comboBoxGetHandImage();
                    activity.PointerProgress.Pointer.Coordinates = new Coordinates();
                    activity.PointerProgress.Pointer.Coordinates.X = (long)numericUpDown_offsetX.Value;
                    activity.PointerProgress.Pointer.Coordinates.Y = (long)numericUpDown_offsetY.Value;

                    if (panel_hand.comboBoxGetSelectedIndexHandImageCentr() >= 0)
                    {
                        activity.PointerProgress.Cover = new ImageCoord();
                        activity.PointerProgress.Cover.ImageIndex = panel_hand.comboBoxGetHandImageCentr();
                        activity.PointerProgress.Cover.Coordinates = new Coordinates();
                        activity.PointerProgress.Cover.Coordinates.X = (long)numericUpDownX_centr.Value;
                        activity.PointerProgress.Cover.Coordinates.Y = (long)numericUpDownY_centr.Value;
                    }

                    if (panel_hand.comboBoxGetSelectedIndexHandImageBackground() >= 0)
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
                        multilangImage.ImageSet.ImageIndex = panel_hand.comboBoxGetHandImageBackground();
                        activity.PointerProgress.Scale.ImageSet.Add(multilangImage);
                    }
                }
            }

            // данные круговой шкалой
            if (panel_scaleCircle.checkBox_scaleCircle_Use.Checked)
            {
                RadioButton radioButton_image = panel_scaleCircle.radioButton_scaleCircle_image;
                //RadioButton radioButton_color = (RadioButton)panel_scaleCircle.Controls[2];
                //ComboBox comboBox_image = (ComboBox)panel_scaleCircle.Controls[3];
                //ComboBox comboBox_color = (ComboBox)panel_scaleCircle.Controls[4];
                //ComboBox comboBox_flatness = (ComboBox)panel_scaleCircle.Controls[5];
                //ComboBox comboBox_background = (ComboBox)panel_scaleCircle.Controls[6];
                NumericUpDown numericUpDownX = panel_scaleCircle.numericUpDown_scaleCircleX;
                NumericUpDown numericUpDownY = panel_scaleCircle.numericUpDown_scaleCircleY;
                NumericUpDown numericUpDown_radius = panel_scaleCircle.numericUpDown_scaleCircle_radius;
                NumericUpDown numericUpDown_width = panel_scaleCircle.numericUpDown_scaleCircle_width;
                NumericUpDown numericUpDown_startAngle = panel_scaleCircle.numericUpDown_scaleCircle_startAngle;
                NumericUpDown numericUpDown_endAngle = panel_scaleCircle.numericUpDown_scaleCircle_endAngle;

                if ((radioButton_image.Checked && panel_scaleCircle.comboBoxGetSelectedIndexImage() >= 0) ||
                    (!radioButton_image.Checked))
                {
                    if (activity == null) activity = new Activity();
                    if (activity.ProgressBar == null) activity.ProgressBar = new ProgressBar();
                    activity.ProgressBar.AngleSettings = new AngleSettings();
                    if (radioButton_image.Checked && panel_scaleCircle.comboBoxGetSelectedIndexImage() >= 0)
                    {
                        activity.ProgressBar.ForegroundImageIndex = panel_scaleCircle.comboBoxGetImage();
                    }
                    else
                    {
                        //Color color = comboBox_color.BackColor;
                        //Color new_color = Color.FromArgb(0, color.R, color.G, color.B);
                        //string colorStr = ColorTranslator.ToHtml(new_color);
                        //colorStr = colorStr.Replace("#", "0xFF");
                        //activity.ProgressBar.Color = colorStr;

                        activity.ProgressBar.Color = panel_scaleCircle.comboBoxGetColorString();
                    }

                    if (panel_scaleCircle.comboBoxGetSelectedIndexImageBackground() >= 0)
                        activity.ProgressBar.BackgroundImageIndex = panel_scaleCircle.comboBoxGetImageBackground();

                    activity.ProgressBar.AngleSettings.X = (long)numericUpDownX.Value;
                    activity.ProgressBar.AngleSettings.Y = (long)numericUpDownY.Value;
                    activity.ProgressBar.AngleSettings.StartAngle = (float)numericUpDown_startAngle.Value;
                    activity.ProgressBar.AngleSettings.EndAngle = (float)numericUpDown_endAngle.Value;
                    activity.ProgressBar.AngleSettings.Radius = (float)numericUpDown_radius.Value;

                    activity.ProgressBar.Width = (long)numericUpDown_width.Value;

                    //switch (comboBox_flatness.SelectedIndex)
                    //{
                    //    case 1:
                    //        activity.ProgressBar.Flatness = 90;
                    //        break;
                    //    case 2:
                    //        activity.ProgressBar.Flatness = 180;
                    //        break;
                    //    default:
                    //        activity.ProgressBar.Flatness = 0;
                    //        break;
                    //}
                    activity.ProgressBar.Flatness = panel_scaleCircle.comboBoxGetFlatness();
                }
            }

            // данные линейной шкалой
            if (panel_scaleLinear.checkBox_scaleLinear_Use.Checked)
            {
                RadioButton radioButton_image = panel_scaleLinear.radioButton_scaleLinear_image;
                ////RadioButton radioButton_color = (RadioButton)panel_scaleLinear.Controls[2];
                //ComboBox comboBox_image = (ComboBox)panel_scaleLinear.Controls[3];
                //ComboBox comboBox_color = (ComboBox)panel_scaleLinear.Controls[4];
                //ComboBox comboBox_pointer = (ComboBox)panel_scaleLinear.Controls[5];
                //ComboBox comboBox_background = (ComboBox)panel_scaleLinear.Controls[6];
                NumericUpDown numericUpDownX = panel_scaleLinear.numericUpDown_scaleLinearX;
                NumericUpDown numericUpDownY = panel_scaleLinear.numericUpDown_scaleLinearY;
                NumericUpDown numericUpDown_length = panel_scaleLinear.numericUpDown_scaleLinear_length;
                NumericUpDown numericUpDown_width = panel_scaleLinear.numericUpDown_scaleLinear_width;

                if ((radioButton_image.Checked && panel_scaleLinear.comboBoxGetSelectedIndexImage() >= 0) ||
                    (!radioButton_image.Checked))
                {
                    if (activity == null) activity = new Activity();
                    if (activity.ProgressBar == null) activity.ProgressBar = new ProgressBar();
                    activity.ProgressBar.LinearSettings = new LinearSettings();
                    if (radioButton_image.Checked && panel_scaleLinear.comboBoxGetSelectedIndexImage() >= 0)
                    {
                        activity.ProgressBar.ForegroundImageIndex = panel_scaleLinear.comboBoxGetImage();
                    }
                    else
                    {
                        //Color color = comboBox_color.BackColor;
                        //Color new_color = Color.FromArgb(0, color.R, color.G, color.B);
                        //string colorStr = ColorTranslator.ToHtml(new_color);
                        //colorStr = colorStr.Replace("#", "0xFF");
                        //activity.ProgressBar.Color = colorStr;

                        activity.ProgressBar.Color = panel_scaleLinear.comboBoxGetColorString();
                    }
                    if (panel_scaleLinear.comboBoxGetSelectedIndexImagePointer() >= 0)
                        activity.ProgressBar.PointerImageIndex = panel_scaleLinear.comboBoxGetImagePointer();
                    if (panel_scaleLinear.comboBoxGetSelectedIndexImageBackground() >= 0)
                        activity.ProgressBar.BackgroundImageIndex = panel_scaleLinear.comboBoxGetImageBackground();

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
                if (ScreenIdle.Activity == null) ScreenIdle.Activity = new List<Activity>();
                ScreenIdle.Activity.Add(activity);
            }
        }


        private void ComboBoxAddItems_AOD()
        {
            comboBox_Background_image_AOD.Items.AddRange(ListImages.ToArray());

            comboBox_AM_image_AOD.Items.AddRange(ListImages.ToArray());
            comboBox_PM_image_AOD.Items.AddRange(ListImages.ToArray());

            comboBox_Hour_image_AOD.Items.AddRange(ListImages.ToArray());
            comboBox_Hour_unit_AOD.Items.AddRange(ListImages.ToArray());
            comboBox_Hour_separator_AOD.Items.AddRange(ListImages.ToArray());

            comboBox_Minute_image_AOD.Items.AddRange(ListImages.ToArray());
            comboBox_Minute_unit_AOD.Items.AddRange(ListImages.ToArray());
            comboBox_Minute_separator_AOD.Items.AddRange(ListImages.ToArray());

            comboBox_Hour_hand_image_AOD.Items.AddRange(ListImages.ToArray());
            comboBox_Hour_hand_imageCentr_AOD.Items.AddRange(ListImages.ToArray());

            comboBox_Minute_hand_image_AOD.Items.AddRange(ListImages.ToArray());
            comboBox_Minute_hand_imageCentr_AOD.Items.AddRange(ListImages.ToArray());
            progressBar1.Value = 55;


            comboBox_Day_image_AOD.Items.AddRange(ListImages.ToArray());
            comboBox_Day_unit_AOD.Items.AddRange(ListImages.ToArray());
            comboBox_Day_separator_AOD.Items.AddRange(ListImages.ToArray());

            comboBox_Day_hand_image_AOD.Items.AddRange(ListImages.ToArray());
            comboBox_Day_hand_imageCentr_AOD.Items.AddRange(ListImages.ToArray());
            comboBox_Day_hand_imageBackground_AOD.Items.AddRange(ListImages.ToArray());

            comboBox_Month_image_AOD.Items.AddRange(ListImages.ToArray());
            comboBox_Month_unit_AOD.Items.AddRange(ListImages.ToArray());
            comboBox_Month_separator_AOD.Items.AddRange(ListImages.ToArray());

            comboBox_Month_pictures_image_AOD.Items.AddRange(ListImages.ToArray());
            comboBox_Month_hand_image_AOD.Items.AddRange(ListImages.ToArray());
            comboBox_Month_hand_imageCentr_AOD.Items.AddRange(ListImages.ToArray());
            comboBox_Month_hand_imageBackground_AOD.Items.AddRange(ListImages.ToArray());

            comboBox_Year_image_AOD.Items.AddRange(ListImages.ToArray());
            comboBox_Year_unit_AOD.Items.AddRange(ListImages.ToArray());
            comboBox_Year_separator_AOD.Items.AddRange(ListImages.ToArray());

            comboBox_DOW_pictures_image_AOD.Items.AddRange(ListImages.ToArray());
            comboBox_DOW_hand_image_AOD.Items.AddRange(ListImages.ToArray());
            comboBox_DOW_hand_imageCentr_AOD.Items.AddRange(ListImages.ToArray());
            comboBox_DOW_hand_imageBackground_AOD.Items.AddRange(ListImages.ToArray());
            progressBar1.Value = 60;



            userControl_pictures_Battery_AOD.ComboBoxAddItems(ListImages);
            userControl_text_Battery_AOD.ComboBoxAddItems(ListImages);
            userControl_hand_Battery_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_Battery_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_Battery_AOD.ComboBoxAddItems(ListImages);
            progressBar1.Value = 65;

            userControl_pictures_Steps_AOD.ComboBoxAddItems(ListImages);
            userControl_text_Steps_AOD.ComboBoxAddItems(ListImages);
            userControl_hand_Steps_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_Steps_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_Steps_AOD.ComboBoxAddItems(ListImages);

            userControl_pictures_Calories_AOD.ComboBoxAddItems(ListImages);
            userControl_text_Calories_AOD.ComboBoxAddItems(ListImages);
            userControl_hand_Calories_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_Calories_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_Calories_AOD.ComboBoxAddItems(ListImages);
            progressBar1.Value = 70;

            userControl_pictures_HeartRate_AOD.ComboBoxAddItems(ListImages);
            userControl_text_HeartRate_AOD.ComboBoxAddItems(ListImages);
            userControl_hand_HeartRate_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_HeartRate_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_HeartRate_AOD.ComboBoxAddItems(ListImages);

            userControl_pictures_PAI_AOD.ComboBoxAddItems(ListImages);
            userControl_text_PAI_AOD.ComboBoxAddItems(ListImages);
            userControl_hand_PAI_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_PAI_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_PAI_AOD.ComboBoxAddItems(ListImages);
            progressBar1.Value = 75;

            userControl_text_Distance_AOD.ComboBoxAddItems(ListImages);

            userControl_pictures_StandUp_AOD.ComboBoxAddItems(ListImages);
            userControl_text_StandUp_AOD.ComboBoxAddItems(ListImages);
            userControl_hand_StandUp_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_StandUp_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_StandUp_AOD.ComboBoxAddItems(ListImages);

            userControl_pictures_weather_AOD.ComboBoxAddItems(ListImages);
            userControl_text_weather_Current_AOD.ComboBoxAddItems(ListImages);
            userControl_text_weather_Min_AOD.ComboBoxAddItems(ListImages);
            userControl_text_weather_Max_AOD.ComboBoxAddItems(ListImages);
            //userControl_hand_Weather_AOD.ComboBoxAddItems(ListImages);
            //userControl_scaleCircle_Weather_AOD.ComboBoxAddItems(ListImages);
            //userControl_scaleLinear_Weather_AOD.ComboBoxAddItems(ListImages);
            progressBar1.Value = 80;

            userControl_pictures_UVindex_AOD.ComboBoxAddItems(ListImages);
            userControl_text_UVindex_AOD.ComboBoxAddItems(ListImages);
            userControl_hand_UVindex_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_UVindex_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_UVindex_AOD.ComboBoxAddItems(ListImages);

            //userControl_pictures_AirQuality_AOD.ComboBoxAddItems(ListImages);
            //userControl_text_AirQuality_AOD.ComboBoxAddItems(ListImages);
            //userControl_hand_AirQuality_AOD.ComboBoxAddItems(ListImages);
            //userControl_scaleCircle_AirQuality_AOD.ComboBoxAddItems(ListImages);
            //userControl_scaleLinear_AirQuality_AOD.ComboBoxAddItems(ListImages);
            progressBar1.Value = 85;

            userControl_pictures_Humidity_AOD.ComboBoxAddItems(ListImages);
            userControl_text_Humidity_AOD.ComboBoxAddItems(ListImages);
            userControl_hand_Humidity_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_Humidity_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_Humidity_AOD.ComboBoxAddItems(ListImages);

            userControl_pictures_WindForce_AOD.ComboBoxAddItems(ListImages);
            userControl_text_WindForce_AOD.ComboBoxAddItems(ListImages);
            userControl_hand_WindForce_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_WindForce_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_WindForce_AOD.ComboBoxAddItems(ListImages);
            progressBar1.Value = 90;

            //userControl_pictures_Altitude_AOD.ComboBoxAddItems(ListImages);
            //userControl_text_Altitude_AOD.ComboBoxAddItems(ListImages);
            //userControl_hand_Altitude_AOD.ComboBoxAddItems(ListImages);
            //userControl_scaleCircle_Altitude_AOD.ComboBoxAddItems(ListImages);
            //userControl_scaleLinear_Altitude_AOD.ComboBoxAddItems(ListImages);

            userControl_pictures_AirPressure_AOD.ComboBoxAddItems(ListImages);
            userControl_text_AirPressure_AOD.ComboBoxAddItems(ListImages);
            userControl_hand_AirPressure_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_AirPressure_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_AirPressure_AOD.ComboBoxAddItems(ListImages);
            progressBar1.Value = 95;

            //userControl_pictures_Stress_AOD.ComboBoxAddItems(ListImages);
            //userControl_text_Stress_AOD.ComboBoxAddItems(ListImages);
            //userControl_hand_Stress_AOD.ComboBoxAddItems(ListImages);
            //userControl_scaleCircle_Stress_AOD.ComboBoxAddItems(ListImages);
            //userControl_scaleLinear_Stress_AOD.ComboBoxAddItems(ListImages);

            //userControl_pictures_ActivityGoal_AOD.ComboBoxAddItems(ListImages);
            //userControl_text_ActivityGoal_AOD.ComboBoxAddItems(ListImages);
            //userControl_hand_ActivityGoal_AOD.ComboBoxAddItems(ListImages);
            //userControl_scaleCircle_ActivityGoal_AOD.ComboBoxAddItems(ListImages);
            //userControl_scaleLinear_ActivityGoal_AOD.ComboBoxAddItems(ListImages);
            progressBar1.Value = 100;

            userControl_pictures_FatBurning_AOD.ComboBoxAddItems(ListImages);
            userControl_text_FatBurning_AOD.ComboBoxAddItems(ListImages);
            userControl_hand_FatBurning_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_FatBurning_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_FatBurning_AOD.ComboBoxAddItems(ListImages);
        }

        // сбрасываем все настройки отображения
        private void checkBoxUseClear_AOD()
        {
            checkBox_Hour_Use_AOD.Checked = false;
            checkBox_Minute_Use_AOD.Checked = false;
            checkBox_12h_Use_AOD.Checked = false;

            checkBox_Hour_hand_Use_AOD.Checked = false;
            checkBox_Minute_hand_Use_AOD.Checked = false;


            checkBox_Year_text_Use_AOD.Checked = false;
            checkBox_Month_Use_AOD.Checked = false;
            checkBox_Month_pictures_Use_AOD.Checked = false;
            checkBox_Month_hand_Use_AOD.Checked = false;
            checkBox_Day_Use_AOD.Checked = false;
            checkBox_Day_hand_Use_AOD.Checked = false;
            checkBox_DOW_pictures_Use_AOD.Checked = false;
            checkBox_DOW_hand_Use_AOD.Checked = false;

        }

        private void SettingsClear_AOD()
        {
            comboBox_Background_image_AOD.Items.Clear();
            comboBox_Background_image_AOD.Text = "";

            comboBox_AM_image_AOD.Items.Clear();
            comboBox_AM_image_AOD.Text = "";
            comboBox_PM_image_AOD.Items.Clear();
            comboBox_PM_image_AOD.Text = "";

            comboBox_Hour_image_AOD.Items.Clear();
            comboBox_Hour_image_AOD.Text = "";
            comboBox_Hour_unit_AOD.Items.Clear();
            comboBox_Hour_unit_AOD.Text = "";
            comboBox_Hour_separator_AOD.Items.Clear();
            comboBox_Hour_separator_AOD.Text = "";

            comboBox_Minute_image_AOD.Items.Clear();
            comboBox_Minute_image_AOD.Text = "";
            comboBox_Minute_unit_AOD.Items.Clear();
            comboBox_Minute_unit_AOD.Text = "";
            comboBox_Minute_separator_AOD.Items.Clear();
            comboBox_Minute_separator_AOD.Text = "";

            comboBox_Hour_hand_image_AOD.Items.Clear();
            comboBox_Hour_hand_image_AOD.Text = "";
            comboBox_Hour_hand_imageCentr_AOD.Items.Clear();
            comboBox_Hour_hand_imageCentr_AOD.Text = "";

            comboBox_Minute_hand_image_AOD.Items.Clear();
            comboBox_Minute_hand_image_AOD.Text = "";
            comboBox_Minute_hand_imageCentr_AOD.Items.Clear();
            comboBox_Minute_hand_imageCentr_AOD.Text = "";


            comboBox_Day_image_AOD.Items.Clear();
            comboBox_Day_image_AOD.Text = "";
            comboBox_Day_unit_AOD.Items.Clear();
            comboBox_Day_unit_AOD.Text = "";
            comboBox_Day_separator_AOD.Items.Clear();
            comboBox_Day_separator_AOD.Text = "";
            comboBox_Day_hand_image_AOD.Items.Clear();
            comboBox_Day_hand_image_AOD.Text = "";
            comboBox_Day_hand_imageCentr_AOD.Items.Clear();
            comboBox_Day_hand_imageCentr_AOD.Text = "";
            comboBox_Day_hand_imageBackground_AOD.Items.Clear();
            comboBox_Day_hand_imageBackground_AOD.Text = "";

            comboBox_Month_image_AOD.Items.Clear();
            comboBox_Month_image_AOD.Text = "";
            comboBox_Month_unit_AOD.Items.Clear();
            comboBox_Month_unit_AOD.Text = "";
            comboBox_Month_separator_AOD.Items.Clear();
            comboBox_Month_separator_AOD.Text = "";

            comboBox_Month_pictures_image_AOD.Items.Clear();
            comboBox_Month_pictures_image_AOD.Text = "";
            comboBox_Month_hand_image_AOD.Items.Clear();
            comboBox_Month_hand_image_AOD.Text = "";
            comboBox_Month_hand_imageCentr_AOD.Items.Clear();
            comboBox_Month_hand_imageCentr_AOD.Text = "";
            comboBox_Month_hand_imageBackground_AOD.Items.Clear();
            comboBox_Month_hand_imageBackground_AOD.Text = "";

            comboBox_Year_image_AOD.Items.Clear();
            comboBox_Year_image_AOD.Text = "";
            comboBox_Year_unit_AOD.Items.Clear();
            comboBox_Year_unit_AOD.Text = "";
            comboBox_Year_separator_AOD.Items.Clear();
            comboBox_Year_separator_AOD.Text = "";

            comboBox_DOW_pictures_image_AOD.Items.Clear();
            comboBox_DOW_pictures_image_AOD.Text = "";
            comboBox_DOW_hand_image_AOD.Items.Clear();
            comboBox_DOW_hand_image_AOD.Text = "";
            comboBox_DOW_hand_imageCentr_AOD.Items.Clear();
            comboBox_DOW_hand_imageCentr_AOD.Text = "";
            comboBox_DOW_hand_imageBackground_AOD.Items.Clear();
            comboBox_DOW_hand_imageBackground_AOD.Text = "";

            userControl_pictures_Battery_AOD.SettingsClear();
            userControl_text_Battery_AOD.SettingsClear();
            userControl_hand_Battery_AOD.SettingsClear();
            userControl_scaleCircle_Battery_AOD.SettingsClear();
            userControl_scaleLinear_Battery_AOD.SettingsClear();

            userControl_pictures_Steps_AOD.SettingsClear();
            userControl_text_Steps_AOD.SettingsClear();
            userControl_hand_Steps_AOD.SettingsClear();
            userControl_scaleCircle_Steps_AOD.SettingsClear();
            userControl_scaleLinear_Steps_AOD.SettingsClear();

            userControl_pictures_Calories_AOD.SettingsClear();
            userControl_text_Calories_AOD.SettingsClear();
            userControl_hand_Calories_AOD.SettingsClear();
            userControl_scaleCircle_Calories_AOD.SettingsClear();
            userControl_scaleLinear_Calories_AOD.SettingsClear();

            userControl_pictures_HeartRate_AOD.SettingsClear();
            userControl_text_HeartRate_AOD.SettingsClear();
            userControl_hand_HeartRate_AOD.SettingsClear();
            userControl_scaleCircle_HeartRate_AOD.SettingsClear();
            userControl_scaleLinear_HeartRate_AOD.SettingsClear();

            userControl_pictures_PAI_AOD.SettingsClear();
            userControl_text_PAI_AOD.SettingsClear();
            userControl_hand_PAI_AOD.SettingsClear();
            userControl_scaleCircle_PAI_AOD.SettingsClear();
            userControl_scaleLinear_PAI_AOD.SettingsClear();


            userControl_text_Distance_AOD.SettingsClear();


            userControl_pictures_StandUp_AOD.SettingsClear();
            userControl_text_StandUp_AOD.SettingsClear();
            userControl_hand_StandUp_AOD.SettingsClear();
            userControl_scaleCircle_StandUp_AOD.SettingsClear();
            userControl_scaleLinear_StandUp_AOD.SettingsClear();

            userControl_pictures_weather_AOD.SettingsClear();
            userControl_text_weather_Current_AOD.SettingsClear();
            userControl_text_weather_Min_AOD.SettingsClear();
            userControl_text_weather_Max_AOD.SettingsClear();
            //userControl_hand_Weather_AOD.SettingsClear();
            //userControl_scaleCircle_Weather_AOD.SettingsClear();
            //userControl_scaleLinear_Weather_AOD.SettingsClear();

            userControl_pictures_UVindex_AOD.SettingsClear();
            userControl_text_UVindex_AOD.SettingsClear();
            userControl_hand_UVindex_AOD.SettingsClear();
            userControl_scaleCircle_UVindex_AOD.SettingsClear();
            userControl_scaleLinear_UVindex_AOD.SettingsClear();

            //userControl_pictures_AirQuality_AOD.SettingsClear();
            //userControl_text_AirQuality_AOD.SettingsClear();
            //userControl_hand_AirQuality_AOD.SettingsClear();
            //userControl_scaleCircle_AirQuality_AOD.SettingsClear();
            //userControl_scaleLinear_AirQuality_AOD.SettingsClear();

            userControl_pictures_Humidity_AOD.SettingsClear();
            userControl_text_Humidity_AOD.SettingsClear();
            userControl_hand_Humidity_AOD.SettingsClear();
            userControl_scaleCircle_Humidity_AOD.SettingsClear();
            userControl_scaleLinear_Humidity_AOD.SettingsClear();

            userControl_pictures_WindForce_AOD.SettingsClear();
            userControl_text_WindForce_AOD.SettingsClear();
            userControl_hand_WindForce_AOD.SettingsClear();
            userControl_scaleCircle_WindForce_AOD.SettingsClear();
            userControl_scaleLinear_WindForce_AOD.SettingsClear();

            //userControl_pictures_Altitude_AOD.SettingsClear();
            //userControl_text_Altitude_AOD.SettingsClear();
            //userControl_hand_Altitude_AOD.SettingsClear();
            //userControl_scaleCircle_Altitude_AOD.SettingsClear();
            //userControl_scaleLinear_Altitude_AOD.SettingsClear();

            userControl_pictures_AirPressure_AOD.SettingsClear();
            userControl_text_AirPressure_AOD.SettingsClear();
            userControl_hand_AirPressure_AOD.SettingsClear();
            userControl_scaleCircle_AirPressure_AOD.SettingsClear();
            userControl_scaleLinear_AirPressure_AOD.SettingsClear();

            //userControl_pictures_Stress_AOD.SettingsClear();
            //userControl_text_Stress_AOD.SettingsClear();
            //userControl_hand_Stress_AOD.SettingsClear();
            //userControl_scaleCircle_Stress_AOD.SettingsClear();
            //userControl_scaleLinear_Stress_AOD.SettingsClear();

            //userControl_pictures_ActivityGoal_AOD.SettingsClear();
            //userControl_text_ActivityGoal_AOD.SettingsClear();
            //userControl_hand_ActivityGoal_AOD.SettingsClear();
            //userControl_scaleCircle_ActivityGoal_AOD.SettingsClear();
            //userControl_scaleLinear_ActivityGoal_AOD.SettingsClear();

            userControl_pictures_FatBurning_AOD.SettingsClear();
            userControl_text_FatBurning_AOD.SettingsClear();
            userControl_hand_FatBurning_AOD.SettingsClear();
            userControl_scaleCircle_FatBurning_AOD.SettingsClear();
            userControl_scaleLinear_FatBurning_AOD.SettingsClear();

        }
    }
}
