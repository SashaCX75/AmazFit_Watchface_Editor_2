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
                                if (digitalTimeDigit.Digit != null)
                                {
                                    if (digitalTimeDigit.Digit.Image != null)
                                    {
                                        checkBox_Minute_Use_AOD.Checked = true;
                                        if (digitalTimeDigit.CombingMode == "Single")
                                        {
                                            checkBox_Minute_follow_AOD.Checked = false;
                                        }
                                        else
                                        {
                                            checkBox_Minute_follow_AOD.Checked = true;
                                        }

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
                                        AlignmentToString(comboBox_Minute_alignment_AOD, digitalTimeDigit.Digit.Alignment);
                                        if (digitalTimeDigit.Digit.Spacing != null)
                                            numericUpDown_Minute_spacing_AOD.Value = (decimal)digitalTimeDigit.Digit.Spacing;
                                        checkBox_Minute_add_zero_AOD.Checked = digitalTimeDigit.Digit.PaddingZero;

                                        if (digitalTimeDigit.Separator != null)
                                        {
                                            comboBoxSetText(comboBox_Minute_unit_AOD, digitalTimeDigit.Separator.ImageIndex);
                                            numericUpDown_Minute_unitX_AOD.Value = digitalTimeDigit.Separator.Coordinates.X;
                                            numericUpDown_Minute_unitY_AOD.Value = digitalTimeDigit.Separator.Coordinates.Y;
                                        }
                                    }
                                    else if (digitalTimeDigit.Digit.SystemFont != null)
                                    {
                                        UserControl_SystemFont_weather userControl_SystemFont_Time = userControl_SystemFont_GroupTime_AOD.userControl_SystemFont_weather_Min;
                                        UserControl_FontRotate_weather userControl_FontRotate_Time = userControl_SystemFont_GroupTime_AOD.userControl_FontRotate_weather_Min;

                                        // системный шрифт
                                        if (digitalTimeDigit.Digit.SystemFont.FontRotate == null)
                                        {
                                            userControl_SystemFont_Time.checkBox_Use.Checked = true;
                                            NumericUpDown numericUpDownX = userControl_SystemFont_Time.numericUpDown_SystemFontX;
                                            NumericUpDown numericUpDownY = userControl_SystemFont_Time.numericUpDown_SystemFontY;
                                            NumericUpDown numericUpDown_size = userControl_SystemFont_Time.numericUpDown_SystemFont_size;
                                            NumericUpDown numericUpDown_angle = userControl_SystemFont_Time.numericUpDown_SystemFont_angle;
                                            CheckBox checkBox_add_zero = userControl_SystemFont_Time.checkBox_addZero;
                                            NumericUpDown numericUpDown_spacing = userControl_SystemFont_Time.numericUpDown_SystemFont_spacing;
                                            CheckBox checkBox_follow = userControl_SystemFont_Time.checkBox_follow;
                                            CheckBox checkBox_separator = userControl_SystemFont_Time.checkBox_separator;

                                            if (digitalTimeDigit.CombingMode == null ||
                                                digitalTimeDigit.CombingMode == "Follow") checkBox_follow.Checked = true;
                                            if (digitalTimeDigit.Separator != null) checkBox_separator.Checked = true;

                                            if (digitalTimeDigit.Digit.SystemFont.Coordinates != null)
                                            {
                                                numericUpDownX.Value = digitalTimeDigit.Digit.SystemFont.Coordinates.X;
                                                numericUpDownY.Value = digitalTimeDigit.Digit.SystemFont.Coordinates.Y;
                                            }
                                            numericUpDown_size.Value = digitalTimeDigit.Digit.SystemFont.Size;
                                            numericUpDown_angle.Value = digitalTimeDigit.Digit.SystemFont.Angle;
                                            if (digitalTimeDigit.Digit.Spacing != null)
                                            {
                                                numericUpDown_spacing.Value = (int)digitalTimeDigit.Digit.Spacing;
                                            }
                                            userControl_SystemFont_Time.comboBoxSetColorString(digitalTimeDigit.Digit.SystemFont.Color);
                                            userControl_SystemFont_Time.checkBoxSetUnit((int)digitalTimeDigit.Digit.SystemFont.ShowUnitCheck - 1);
                                            checkBox_add_zero.Checked = digitalTimeDigit.Digit.PaddingZero;
                                        }


                                        // системный шрифт по окружности
                                        if (digitalTimeDigit.Digit != null && digitalTimeDigit.Digit.SystemFont != null &&
                                            digitalTimeDigit.Digit.SystemFont.FontRotate != null && userControl_FontRotate_Time != null)
                                        {
                                            userControl_FontRotate_Time.checkBox_Use.Checked = true;
                                            NumericUpDown numericUpDownX = userControl_FontRotate_Time.numericUpDown_FontRotateX;
                                            NumericUpDown numericUpDownY = userControl_FontRotate_Time.numericUpDown_FontRotateY;
                                            NumericUpDown numericUpDown_size = userControl_FontRotate_Time.numericUpDown_FontRotate_size;
                                            NumericUpDown numericUpDown_angle = userControl_FontRotate_Time.numericUpDown_FontRotate_angle;
                                            NumericUpDown numericUpDown_radius = userControl_FontRotate_Time.numericUpDown_FontRotate_radius;
                                            NumericUpDown numericUpDown_spacing = userControl_FontRotate_Time.numericUpDown_FontRotate_spacing;
                                            CheckBox checkBox_add_zero = userControl_FontRotate_Time.checkBox_addZero;
                                            CheckBox checkBox_follow = userControl_FontRotate_Time.checkBox_follow;
                                            CheckBox checkBox_separator = userControl_FontRotate_Time.checkBox_separator;

                                            if (digitalTimeDigit.CombingMode == null ||
                                                digitalTimeDigit.CombingMode == "Follow") checkBox_follow.Checked = true;
                                            if (digitalTimeDigit.Separator != null) checkBox_separator.Checked = true;

                                            if (digitalTimeDigit.Digit.SystemFont.FontRotate != null)
                                            {
                                                numericUpDownX.Value = digitalTimeDigit.Digit.SystemFont.FontRotate.X;
                                                numericUpDownY.Value = digitalTimeDigit.Digit.SystemFont.FontRotate.Y;
                                                numericUpDown_radius.Value = digitalTimeDigit.Digit.SystemFont.FontRotate.Radius;
                                                userControl_FontRotate_Time.radioButtonSetRotateDirection(
                                                    (int)digitalTimeDigit.Digit.SystemFont.FontRotate.RotateDirection);
                                            }
                                            numericUpDown_size.Value = digitalTimeDigit.Digit.SystemFont.Size;
                                            numericUpDown_angle.Value = digitalTimeDigit.Digit.SystemFont.Angle;
                                            if (digitalTimeDigit.Digit.Spacing != null)
                                            {
                                                numericUpDown_spacing.Value = (int)digitalTimeDigit.Digit.Spacing;
                                            }
                                            userControl_FontRotate_Time.comboBoxSetColorString(digitalTimeDigit.Digit.SystemFont.Color);
                                            userControl_FontRotate_Time.checkBoxSetUnit((int)digitalTimeDigit.Digit.SystemFont.ShowUnitCheck - 1);
                                            checkBox_add_zero.Checked = digitalTimeDigit.Digit.PaddingZero;
                                        }
                                    }
                                }
                                break;

                            default:
                                if (digitalTimeDigit.Digit != null)
                                {
                                    if (digitalTimeDigit.Digit.Image != null)
                                    {
                                        checkBox_Hour_Use_AOD.Checked = true;
                                        AOD_24h = digitalTimeDigit.Digit.DisplayFormAnalog;

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
                                        AlignmentToString(comboBox_Hour_alignment_AOD, digitalTimeDigit.Digit.Alignment);
                                        if (digitalTimeDigit.Digit.Spacing != null)
                                            numericUpDown_Hour_spacing_AOD.Value = (decimal)digitalTimeDigit.Digit.Spacing;
                                        checkBox_Hour_add_zero_AOD.Checked = digitalTimeDigit.Digit.PaddingZero;

                                        if (digitalTimeDigit.Separator != null)
                                        {
                                            comboBoxSetText(comboBox_Hour_unit_AOD, digitalTimeDigit.Separator.ImageIndex);
                                            numericUpDown_Hour_unitX_AOD.Value = digitalTimeDigit.Separator.Coordinates.X;
                                            numericUpDown_Hour_unitY_AOD.Value = digitalTimeDigit.Separator.Coordinates.Y;
                                        }
                                    }
                                    else if (digitalTimeDigit.Digit.SystemFont != null)
                                    {
                                        UserControl_SystemFont_weather userControl_SystemFont_Time = userControl_SystemFont_GroupTime_AOD.userControl_SystemFont_weather_Current;
                                        UserControl_FontRotate_weather userControl_FontRotate_Time = userControl_SystemFont_GroupTime_AOD.userControl_FontRotate_weather_Current;

                                        // системный шрифт
                                        if (digitalTimeDigit.Digit.SystemFont.FontRotate == null)
                                        {
                                            userControl_SystemFont_Time.checkBox_Use.Checked = true;
                                            NumericUpDown numericUpDownX = userControl_SystemFont_Time.numericUpDown_SystemFontX;
                                            NumericUpDown numericUpDownY = userControl_SystemFont_Time.numericUpDown_SystemFontY;
                                            NumericUpDown numericUpDown_size = userControl_SystemFont_Time.numericUpDown_SystemFont_size;
                                            NumericUpDown numericUpDown_angle = userControl_SystemFont_Time.numericUpDown_SystemFont_angle;
                                            CheckBox checkBox_add_zero = userControl_SystemFont_Time.checkBox_addZero;
                                            NumericUpDown numericUpDown_spacing = userControl_SystemFont_Time.numericUpDown_SystemFont_spacing;
                                            CheckBox checkBox_follow = userControl_SystemFont_Time.checkBox_follow;
                                            CheckBox checkBox_separator = userControl_SystemFont_Time.checkBox_separator;

                                            if (digitalTimeDigit.CombingMode == null ||
                                                digitalTimeDigit.CombingMode == "Follow") checkBox_follow.Checked = true;
                                            if (digitalTimeDigit.Separator != null) checkBox_separator.Checked = true;

                                            if (digitalTimeDigit.Digit.SystemFont.Coordinates != null)
                                            {
                                                numericUpDownX.Value = digitalTimeDigit.Digit.SystemFont.Coordinates.X;
                                                numericUpDownY.Value = digitalTimeDigit.Digit.SystemFont.Coordinates.Y;
                                            }
                                            numericUpDown_size.Value = digitalTimeDigit.Digit.SystemFont.Size;
                                            numericUpDown_angle.Value = digitalTimeDigit.Digit.SystemFont.Angle;
                                            if (digitalTimeDigit.Digit.Spacing != null)
                                            {
                                                numericUpDown_spacing.Value = (int)digitalTimeDigit.Digit.Spacing;
                                            }
                                            userControl_SystemFont_Time.comboBoxSetColorString(digitalTimeDigit.Digit.SystemFont.Color);
                                            userControl_SystemFont_Time.checkBoxSetUnit((int)digitalTimeDigit.Digit.SystemFont.ShowUnitCheck - 1);
                                            checkBox_add_zero.Checked = digitalTimeDigit.Digit.PaddingZero;
                                        }


                                        // системный шрифт по окружности
                                        if (digitalTimeDigit.Digit != null && digitalTimeDigit.Digit.SystemFont != null &&
                                            digitalTimeDigit.Digit.SystemFont.FontRotate != null && userControl_FontRotate_Time != null)
                                        {
                                            userControl_FontRotate_Time.checkBox_Use.Checked = true;
                                            NumericUpDown numericUpDownX = userControl_FontRotate_Time.numericUpDown_FontRotateX;
                                            NumericUpDown numericUpDownY = userControl_FontRotate_Time.numericUpDown_FontRotateY;
                                            NumericUpDown numericUpDown_size = userControl_FontRotate_Time.numericUpDown_FontRotate_size;
                                            NumericUpDown numericUpDown_angle = userControl_FontRotate_Time.numericUpDown_FontRotate_angle;
                                            NumericUpDown numericUpDown_radius = userControl_FontRotate_Time.numericUpDown_FontRotate_radius;
                                            NumericUpDown numericUpDown_spacing = userControl_FontRotate_Time.numericUpDown_FontRotate_spacing;
                                            CheckBox checkBox_add_zero = userControl_FontRotate_Time.checkBox_addZero;
                                            CheckBox checkBox_follow = userControl_FontRotate_Time.checkBox_follow;
                                            CheckBox checkBox_separator = userControl_FontRotate_Time.checkBox_separator;

                                            if (digitalTimeDigit.CombingMode == null ||
                                                digitalTimeDigit.CombingMode == "Follow") checkBox_follow.Checked = true;
                                            if (digitalTimeDigit.Separator != null) checkBox_separator.Checked = true;

                                            if (digitalTimeDigit.Digit.SystemFont.FontRotate != null)
                                            {
                                                numericUpDownX.Value = digitalTimeDigit.Digit.SystemFont.FontRotate.X;
                                                numericUpDownY.Value = digitalTimeDigit.Digit.SystemFont.FontRotate.Y;
                                                numericUpDown_radius.Value = digitalTimeDigit.Digit.SystemFont.FontRotate.Radius;
                                                userControl_FontRotate_Time.radioButtonSetRotateDirection(
                                                    (int)digitalTimeDigit.Digit.SystemFont.FontRotate.RotateDirection);
                                            }
                                            numericUpDown_size.Value = digitalTimeDigit.Digit.SystemFont.Size;
                                            numericUpDown_angle.Value = digitalTimeDigit.Digit.SystemFont.Angle;
                                            if (digitalTimeDigit.Digit.Spacing != null)
                                            {
                                                numericUpDown_spacing.Value = (int)digitalTimeDigit.Digit.Spacing;
                                            }
                                            userControl_FontRotate_Time.comboBoxSetColorString(digitalTimeDigit.Digit.SystemFont.Color);
                                            userControl_FontRotate_Time.checkBoxSetUnit((int)digitalTimeDigit.Digit.SystemFont.ShowUnitCheck - 1);
                                            checkBox_add_zero.Checked = digitalTimeDigit.Digit.PaddingZero;
                                        }
                                    }
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

                                // надпись
                                if (digitalDateDigit.Digit != null && digitalDateDigit.Digit.Image != null)
                                {
                                    checkBox_Day_Use_AOD.Checked = true;
                                    if (digitalDateDigit.CombingMode == "Single")
                                    {
                                        checkBox_Day_follow_AOD.Checked = false;
                                    }
                                    else
                                    {
                                        checkBox_Day_follow_AOD.Checked = true;
                                    }

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
                                    AlignmentToString(comboBox_Day_alignment_AOD, digitalDateDigit.Digit.Alignment);
                                    if (digitalDateDigit.Digit.Spacing != null)
                                        numericUpDown_Day_spacing_AOD.Value = (decimal)digitalDateDigit.Digit.Spacing;
                                    checkBox_Day_add_zero_AOD.Checked = digitalDateDigit.Digit.PaddingZero;

                                    if (digitalDateDigit.Separator != null)
                                    {
                                        comboBoxSetText(comboBox_Day_unit_AOD, digitalDateDigit.Separator.ImageIndex);
                                        numericUpDown_Day_unitX_AOD.Value = digitalDateDigit.Separator.Coordinates.X;
                                        numericUpDown_Day_unitY_AOD.Value = digitalDateDigit.Separator.Coordinates.Y;
                                    }
                                }

                                // системный шрифт
                                if (digitalDateDigit.Digit != null && digitalDateDigit.Digit.SystemFont != null &&
                                    digitalDateDigit.Digit.SystemFont.FontRotate == null)
                                {
                                    UserControl_SystemFont userControl_SystemFont_Date =
                                        userControl_SystemFont_Group_Day_AOD.userControl_SystemFont;
                                    userControl_SystemFont_Date.checkBox_Use.Checked = true;
                                    NumericUpDown numericUpDownX = userControl_SystemFont_Date.numericUpDown_SystemFontX;
                                    NumericUpDown numericUpDownY = userControl_SystemFont_Date.numericUpDown_SystemFontY;
                                    NumericUpDown numericUpDown_size = userControl_SystemFont_Date.numericUpDown_SystemFont_size;
                                    NumericUpDown numericUpDown_angle = userControl_SystemFont_Date.numericUpDown_SystemFont_angle;
                                    CheckBox checkBox_add_zero = userControl_SystemFont_Date.checkBox_addZero;
                                    NumericUpDown numericUpDown_spacing = userControl_SystemFont_Date.numericUpDown_SystemFont_spacing;
                                    CheckBox checkBox_follow = userControl_SystemFont_Date.checkBox_follow;
                                    CheckBox checkBox_separator = userControl_SystemFont_Date.checkBox_separator;

                                    if (digitalDateDigit.CombingMode == null ||
                                        digitalDateDigit.CombingMode == "Follow") checkBox_follow.Checked = true;
                                    if (digitalDateDigit.Separator != null) checkBox_separator.Checked = true;

                                    if (digitalDateDigit.Digit.SystemFont.Coordinates != null)
                                    {
                                        numericUpDownX.Value = digitalDateDigit.Digit.SystemFont.Coordinates.X;
                                        numericUpDownY.Value = digitalDateDigit.Digit.SystemFont.Coordinates.Y;
                                    }
                                    numericUpDown_size.Value = digitalDateDigit.Digit.SystemFont.Size;
                                    numericUpDown_angle.Value = digitalDateDigit.Digit.SystemFont.Angle;
                                    if (digitalDateDigit.Digit.Spacing != null)
                                    {
                                        numericUpDown_spacing.Value = (int)digitalDateDigit.Digit.Spacing;
                                    }
                                    userControl_SystemFont_Date.comboBoxSetColorString(digitalDateDigit.Digit.SystemFont.Color);
                                    userControl_SystemFont_Date.checkBoxSetUnit((int)digitalDateDigit.Digit.SystemFont.ShowUnitCheck);
                                    checkBox_add_zero.Checked = digitalDateDigit.Digit.PaddingZero;
                                }

                                // системный шрифт по окружности
                                if (digitalDateDigit.Digit != null && digitalDateDigit.Digit.SystemFont != null &&
                                    digitalDateDigit.Digit.SystemFont.FontRotate != null)
                                {
                                    UserControl_FontRotate userControl_FontRotate_Date =
                                        userControl_SystemFont_Group_Day_AOD.userControl_FontRotate;
                                    userControl_FontRotate_Date.checkBox_Use.Checked = true;
                                    NumericUpDown numericUpDownX = userControl_FontRotate_Date.numericUpDown_FontRotateX;
                                    NumericUpDown numericUpDownY = userControl_FontRotate_Date.numericUpDown_FontRotateY;
                                    NumericUpDown numericUpDown_size = userControl_FontRotate_Date.numericUpDown_FontRotate_size;
                                    NumericUpDown numericUpDown_angle = userControl_FontRotate_Date.numericUpDown_FontRotate_angle;
                                    NumericUpDown numericUpDown_radius = userControl_FontRotate_Date.numericUpDown_FontRotate_radius;
                                    NumericUpDown numericUpDown_spacing = userControl_FontRotate_Date.numericUpDown_FontRotate_spacing;
                                    CheckBox checkBox_add_zero = userControl_FontRotate_Date.checkBox_addZero;
                                    CheckBox checkBox_follow = userControl_FontRotate_Date.checkBox_follow;
                                    CheckBox checkBox_separator = userControl_FontRotate_Date.checkBox_separator;

                                    if (digitalDateDigit.CombingMode == null ||
                                        digitalDateDigit.CombingMode == "Follow") checkBox_follow.Checked = true;
                                    if (digitalDateDigit.Separator != null) checkBox_separator.Checked = true;

                                    if (digitalDateDigit.Digit.SystemFont.FontRotate != null)
                                    {
                                        numericUpDownX.Value = digitalDateDigit.Digit.SystemFont.FontRotate.X;
                                        numericUpDownY.Value = digitalDateDigit.Digit.SystemFont.FontRotate.Y;
                                        numericUpDown_radius.Value = digitalDateDigit.Digit.SystemFont.FontRotate.Radius;
                                        userControl_FontRotate_Date.radioButtonSetRotateDirection(
                                            (int)digitalDateDigit.Digit.SystemFont.FontRotate.RotateDirection);
                                    }
                                    numericUpDown_size.Value = digitalDateDigit.Digit.SystemFont.Size;
                                    numericUpDown_angle.Value = digitalDateDigit.Digit.SystemFont.Angle;
                                    if (digitalDateDigit.Digit.Spacing != null)
                                    {
                                        numericUpDown_spacing.Value = (int)digitalDateDigit.Digit.Spacing;
                                    }
                                    userControl_FontRotate_Date.comboBoxSetColorString(digitalDateDigit.Digit.SystemFont.Color);
                                    userControl_FontRotate_Date.checkBoxSetUnit((int)digitalDateDigit.Digit.SystemFont.ShowUnitCheck);
                                    checkBox_add_zero.Checked = digitalDateDigit.Digit.PaddingZero;
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
                                    // надпись
                                    if (digitalDateDigit.Digit != null && digitalDateDigit.Digit.Image != null)
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
                                        AlignmentToString(comboBox_Month_alignment_AOD, digitalDateDigit.Digit.Alignment);
                                        if (digitalDateDigit.Digit.Spacing != null)
                                            numericUpDown_Month_spacing_AOD.Value = (decimal)digitalDateDigit.Digit.Spacing;
                                        checkBox_Month_add_zero_AOD.Checked = digitalDateDigit.Digit.PaddingZero;

                                        if (digitalDateDigit.Separator != null)
                                        {
                                            comboBoxSetText(comboBox_Month_unit_AOD, digitalDateDigit.Separator.ImageIndex);
                                            numericUpDown_Month_unitX_AOD.Value = digitalDateDigit.Separator.Coordinates.X;
                                            numericUpDown_Month_unitY_AOD.Value = digitalDateDigit.Separator.Coordinates.Y;
                                        }
                                    }

                                    // системный шрифт
                                    if (digitalDateDigit.Digit != null && digitalDateDigit.Digit.SystemFont != null &&
                                        digitalDateDigit.Digit.SystemFont.FontRotate == null)
                                    {
                                        UserControl_SystemFont userControl_SystemFont_Date =
                                            userControl_SystemFont_Group_Month_AOD.userControl_SystemFont;
                                        userControl_SystemFont_Date.checkBox_Use.Checked = true;
                                        NumericUpDown numericUpDownX = userControl_SystemFont_Date.numericUpDown_SystemFontX;
                                        NumericUpDown numericUpDownY = userControl_SystemFont_Date.numericUpDown_SystemFontY;
                                        NumericUpDown numericUpDown_size = userControl_SystemFont_Date.numericUpDown_SystemFont_size;
                                        NumericUpDown numericUpDown_angle = userControl_SystemFont_Date.numericUpDown_SystemFont_angle;
                                        CheckBox checkBox_add_zero = userControl_SystemFont_Date.checkBox_addZero;
                                        NumericUpDown numericUpDown_spacing = userControl_SystemFont_Date.numericUpDown_SystemFont_spacing;
                                        CheckBox checkBox_follow = userControl_SystemFont_Date.checkBox_follow;
                                        CheckBox checkBox_separator = userControl_SystemFont_Date.checkBox_separator;

                                        if (digitalDateDigit.CombingMode == null ||
                                            digitalDateDigit.CombingMode == "Follow") checkBox_follow.Checked = true;
                                        if (digitalDateDigit.Separator != null) checkBox_separator.Checked = true;

                                        if (digitalDateDigit.Digit.SystemFont.Coordinates != null)
                                        {
                                            numericUpDownX.Value = digitalDateDigit.Digit.SystemFont.Coordinates.X;
                                            numericUpDownY.Value = digitalDateDigit.Digit.SystemFont.Coordinates.Y;
                                        }
                                        numericUpDown_size.Value = digitalDateDigit.Digit.SystemFont.Size;
                                        numericUpDown_angle.Value = digitalDateDigit.Digit.SystemFont.Angle;
                                        if (digitalDateDigit.Digit.Spacing != null)
                                        {
                                            numericUpDown_spacing.Value = (int)digitalDateDigit.Digit.Spacing;
                                        }
                                        userControl_SystemFont_Date.comboBoxSetColorString(digitalDateDigit.Digit.SystemFont.Color);
                                        userControl_SystemFont_Date.checkBoxSetUnit((int)digitalDateDigit.Digit.SystemFont.ShowUnitCheck);
                                        checkBox_add_zero.Checked = digitalDateDigit.Digit.PaddingZero;
                                    }

                                    // системный шрифт по окружности
                                    if (digitalDateDigit.Digit != null && digitalDateDigit.Digit.SystemFont != null &&
                                        digitalDateDigit.Digit.SystemFont.FontRotate != null)
                                    {
                                        UserControl_FontRotate userControl_FontRotate_Date =
                                            userControl_SystemFont_Group_Month_AOD.userControl_FontRotate;
                                        userControl_FontRotate_Date.checkBox_Use.Checked = true;
                                        NumericUpDown numericUpDownX = userControl_FontRotate_Date.numericUpDown_FontRotateX;
                                        NumericUpDown numericUpDownY = userControl_FontRotate_Date.numericUpDown_FontRotateY;
                                        NumericUpDown numericUpDown_size = userControl_FontRotate_Date.numericUpDown_FontRotate_size;
                                        NumericUpDown numericUpDown_angle = userControl_FontRotate_Date.numericUpDown_FontRotate_angle;
                                        NumericUpDown numericUpDown_radius = userControl_FontRotate_Date.numericUpDown_FontRotate_radius;
                                        NumericUpDown numericUpDown_spacing = userControl_FontRotate_Date.numericUpDown_FontRotate_spacing;
                                        CheckBox checkBox_add_zero = userControl_FontRotate_Date.checkBox_addZero;
                                        CheckBox checkBox_follow = userControl_FontRotate_Date.checkBox_follow;
                                        CheckBox checkBox_separator = userControl_FontRotate_Date.checkBox_separator;

                                        if (digitalDateDigit.CombingMode == null ||
                                            digitalDateDigit.CombingMode == "Follow") checkBox_follow.Checked = true;
                                        if (digitalDateDigit.Separator != null) checkBox_separator.Checked = true;

                                        if (digitalDateDigit.Digit.SystemFont.FontRotate != null)
                                        {
                                            numericUpDownX.Value = digitalDateDigit.Digit.SystemFont.FontRotate.X;
                                            numericUpDownY.Value = digitalDateDigit.Digit.SystemFont.FontRotate.Y;
                                            numericUpDown_radius.Value = digitalDateDigit.Digit.SystemFont.FontRotate.Radius;
                                            userControl_FontRotate_Date.radioButtonSetRotateDirection(
                                                (int)digitalDateDigit.Digit.SystemFont.FontRotate.RotateDirection);
                                        }
                                        numericUpDown_size.Value = digitalDateDigit.Digit.SystemFont.Size;
                                        numericUpDown_angle.Value = digitalDateDigit.Digit.SystemFont.Angle;
                                        if (digitalDateDigit.Digit.Spacing != null)
                                        {
                                            numericUpDown_spacing.Value = (int)digitalDateDigit.Digit.Spacing;
                                        }
                                        userControl_FontRotate_Date.comboBoxSetColorString(digitalDateDigit.Digit.SystemFont.Color);
                                        userControl_FontRotate_Date.checkBoxSetUnit((int)digitalDateDigit.Digit.SystemFont.ShowUnitCheck);
                                        checkBox_add_zero.Checked = digitalDateDigit.Digit.PaddingZero;
                                    }
                                }
                                break;

                            default:
                                // надпись
                                if (digitalDateDigit.Digit != null && digitalDateDigit.Digit.Image != null)
                                {
                                    checkBox_Year_text_Use_AOD.Checked = true;
                                    if (digitalDateDigit.CombingMode == "Single")
                                    {
                                        checkBox_Year_follow_AOD.Checked = false;
                                    }
                                    else
                                    {
                                        checkBox_Year_follow_AOD.Checked = true;
                                    }

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
                                    AlignmentToString(comboBox_Year_alignment_AOD, digitalDateDigit.Digit.Alignment);
                                    if (digitalDateDigit.Digit.Spacing != null)
                                        numericUpDown_Year_spacing_AOD.Value = (decimal)digitalDateDigit.Digit.Spacing;
                                    checkBox_Year_add_zero_AOD.Checked = digitalDateDigit.Digit.PaddingZero;

                                    if (digitalDateDigit.Separator != null)
                                    {
                                        comboBoxSetText(comboBox_Year_unit_AOD, digitalDateDigit.Separator.ImageIndex);
                                        numericUpDown_Year_unitX_AOD.Value = digitalDateDigit.Separator.Coordinates.X;
                                        numericUpDown_Year_unitY_AOD.Value = digitalDateDigit.Separator.Coordinates.Y;
                                    }
                                }

                                // системный шрифт
                                if (digitalDateDigit.Digit != null && digitalDateDigit.Digit.SystemFont != null &&
                                    digitalDateDigit.Digit.SystemFont.FontRotate == null)
                                {
                                    UserControl_SystemFont userControl_SystemFont_Date =
                                        userControl_SystemFont_Group_Year_AOD.userControl_SystemFont;
                                    userControl_SystemFont_Date.checkBox_Use.Checked = true;
                                    NumericUpDown numericUpDownX = userControl_SystemFont_Date.numericUpDown_SystemFontX;
                                    NumericUpDown numericUpDownY = userControl_SystemFont_Date.numericUpDown_SystemFontY;
                                    NumericUpDown numericUpDown_size = userControl_SystemFont_Date.numericUpDown_SystemFont_size;
                                    NumericUpDown numericUpDown_angle = userControl_SystemFont_Date.numericUpDown_SystemFont_angle;
                                    CheckBox checkBox_add_zero = userControl_SystemFont_Date.checkBox_addZero;
                                    NumericUpDown numericUpDown_spacing = userControl_SystemFont_Date.numericUpDown_SystemFont_spacing;
                                    CheckBox checkBox_follow = userControl_SystemFont_Date.checkBox_follow;
                                    CheckBox checkBox_separator = userControl_SystemFont_Date.checkBox_separator;

                                    if (digitalDateDigit.CombingMode == null ||
                                        digitalDateDigit.CombingMode == "Follow") checkBox_follow.Checked = true;
                                    if (digitalDateDigit.Separator != null) checkBox_separator.Checked = true;

                                    if (digitalDateDigit.Digit.SystemFont.Coordinates != null)
                                    {
                                        numericUpDownX.Value = digitalDateDigit.Digit.SystemFont.Coordinates.X;
                                        numericUpDownY.Value = digitalDateDigit.Digit.SystemFont.Coordinates.Y;
                                    }
                                    numericUpDown_size.Value = digitalDateDigit.Digit.SystemFont.Size;
                                    numericUpDown_angle.Value = digitalDateDigit.Digit.SystemFont.Angle;
                                    if (digitalDateDigit.Digit.Spacing != null)
                                    {
                                        numericUpDown_spacing.Value = (int)digitalDateDigit.Digit.Spacing;
                                    }
                                    userControl_SystemFont_Date.comboBoxSetColorString(digitalDateDigit.Digit.SystemFont.Color);
                                    userControl_SystemFont_Date.checkBoxSetUnit((int)digitalDateDigit.Digit.SystemFont.ShowUnitCheck);
                                    checkBox_add_zero.Checked = digitalDateDigit.Digit.PaddingZero;
                                }

                                // системный шрифт по окружности
                                if (digitalDateDigit.Digit != null && digitalDateDigit.Digit.SystemFont != null &&
                                    digitalDateDigit.Digit.SystemFont.FontRotate != null)
                                {
                                    UserControl_FontRotate userControl_FontRotate_Date =
                                        userControl_SystemFont_Group_Year_AOD.userControl_FontRotate;
                                    userControl_FontRotate_Date.checkBox_Use.Checked = true;
                                    NumericUpDown numericUpDownX = userControl_FontRotate_Date.numericUpDown_FontRotateX;
                                    NumericUpDown numericUpDownY = userControl_FontRotate_Date.numericUpDown_FontRotateY;
                                    NumericUpDown numericUpDown_size = userControl_FontRotate_Date.numericUpDown_FontRotate_size;
                                    NumericUpDown numericUpDown_angle = userControl_FontRotate_Date.numericUpDown_FontRotate_angle;
                                    NumericUpDown numericUpDown_radius = userControl_FontRotate_Date.numericUpDown_FontRotate_radius;
                                    NumericUpDown numericUpDown_spacing = userControl_FontRotate_Date.numericUpDown_FontRotate_spacing;
                                    CheckBox checkBox_add_zero = userControl_FontRotate_Date.checkBox_addZero;
                                    CheckBox checkBox_follow = userControl_FontRotate_Date.checkBox_follow;
                                    CheckBox checkBox_separator = userControl_FontRotate_Date.checkBox_separator;

                                    if (digitalDateDigit.CombingMode == null ||
                                        digitalDateDigit.CombingMode == "Follow") checkBox_follow.Checked = true;
                                    if (digitalDateDigit.Separator != null) checkBox_separator.Checked = true;

                                    if (digitalDateDigit.Digit.SystemFont.FontRotate != null)
                                    {
                                        numericUpDownX.Value = digitalDateDigit.Digit.SystemFont.FontRotate.X;
                                        numericUpDownY.Value = digitalDateDigit.Digit.SystemFont.FontRotate.Y;
                                        numericUpDown_radius.Value = digitalDateDigit.Digit.SystemFont.FontRotate.Radius;
                                        userControl_FontRotate_Date.radioButtonSetRotateDirection(
                                            (int)digitalDateDigit.Digit.SystemFont.FontRotate.RotateDirection);
                                    }
                                    numericUpDown_size.Value = digitalDateDigit.Digit.SystemFont.Size;
                                    numericUpDown_angle.Value = digitalDateDigit.Digit.SystemFont.Angle;
                                    if (digitalDateDigit.Digit.Spacing != null)
                                    {
                                        numericUpDown_spacing.Value = (int)digitalDateDigit.Digit.Spacing;
                                    }
                                    userControl_FontRotate_Date.comboBoxSetColorString(digitalDateDigit.Digit.SystemFont.Color);
                                    userControl_FontRotate_Date.checkBoxSetUnit((int)digitalDateDigit.Digit.SystemFont.ShowUnitCheck);
                                    checkBox_add_zero.Checked = digitalDateDigit.Digit.PaddingZero;
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
                    //if (Watch_Face.ScreenIdle.DateAmazfit.WeeksDigits.Digit != null &&
                    //Watch_Face.ScreenIdle.DateAmazfit.WeeksDigits.Digit.DisplayFormAnalog)
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
                int weatherAlignmentFix_AOD = 0;
                foreach (Activity activity in Watch_Face.ScreenIdle.Activity)
                {
                    UserControl_pictures userPanel_pictures = null;
                    UserControl_segments userControl_segments = null;
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

                    switch (activity.Type)
                    {
                        case "Battery":
                            userPanel_pictures = userControl_pictures_Battery_AOD;
                            userControl_segments = userControl_segments_Battery_AOD;
                            userPanel_text = userControl_text_Battery_AOD;
                            userPanel_hand = userControl_hand_Battery_AOD;
                            userPanel_scaleCircle = userControl_scaleCircle_Battery_AOD;
                            userPanel_scaleLinear = userControl_scaleLinear_Battery_AOD;
                            userControl_SystemFont_Group = userControl_SystemFont_Group_Battery_AOD;
                            userControl_icon = userControl_icon_Battery_AOD;
                            break;
                        case "Steps":
                            userPanel_pictures = userControl_pictures_Steps_AOD;
                            userControl_segments = userControl_segments_Steps_AOD;
                            userPanel_text = userControl_text_Steps_AOD;
                            userPanel_textGoal = userControl_text_goal_Steps_AOD;
                            userPanel_hand = userControl_hand_Steps_AOD;
                            userPanel_scaleCircle = userControl_scaleCircle_Steps_AOD;
                            userPanel_scaleLinear = userControl_scaleLinear_Steps_AOD;
                            userControl_SystemFont_Group = userControl_SystemFont_Group_Steps_AOD;
                            userControl_icon = userControl_icon_Steps_AOD;
                            break;
                        case "Calories":
                            userPanel_pictures = userControl_pictures_Calories_AOD;
                            userControl_segments = userControl_segments_Calories_AOD;
                            userPanel_text = userControl_text_Calories_AOD;
                            userPanel_textGoal = userControl_text_goal_Calories_AOD;
                            userPanel_hand = userControl_hand_Calories_AOD;
                            userPanel_scaleCircle = userControl_scaleCircle_Calories_AOD;
                            userPanel_scaleLinear = userControl_scaleLinear_Calories_AOD;
                            userControl_SystemFont_Group = userControl_SystemFont_Group_Calories_AOD;
                            userControl_icon = userControl_icon_Calories_AOD;
                            break;
                        case "HeartRate":
                            userPanel_pictures = userControl_pictures_HeartRate_AOD;
                            userControl_segments = userControl_segments_HeartRate_AOD;
                            userPanel_text = userControl_text_HeartRate_AOD;
                            userPanel_hand = userControl_hand_HeartRate_AOD;
                            userPanel_scaleCircle = userControl_scaleCircle_HeartRate_AOD;
                            userPanel_scaleLinear = userControl_scaleLinear_HeartRate_AOD;
                            userControl_SystemFont_Group = userControl_SystemFont_Group_HeartRate_AOD;
                            userControl_icon = userControl_icon_HeartRate_AOD;
                            break;
                        case "PAI":
                            userPanel_pictures = userControl_pictures_PAI_AOD;
                            userControl_segments = userControl_segments_PAI_AOD;
                            userPanel_text = userControl_text_PAI_AOD;
                            userPanel_hand = userControl_hand_PAI_AOD;
                            userPanel_scaleCircle = userControl_scaleCircle_PAI_AOD;
                            userPanel_scaleLinear = userControl_scaleLinear_PAI_AOD;
                            userControl_SystemFont_Group = userControl_SystemFont_Group_PAI_AOD;
                            userControl_icon = userControl_icon_PAI_AOD;
                            break;
                        case "Distance":
                            userPanel_text = userControl_text_Distance_AOD;
                            userControl_SystemFont_Group = userControl_SystemFont_Group_Distance_AOD;
                            userControl_icon = userControl_icon_Distance_AOD;
                            break;
                        case "StandUp":
                            userPanel_pictures = userControl_pictures_StandUp_AOD;
                            userControl_segments = userControl_segments_StandUp_AOD;
                            userPanel_text = userControl_text_StandUp_AOD;
                            userPanel_textGoal = userControl_text_goal_StandUp_AOD;
                            userPanel_hand = userControl_hand_StandUp_AOD;
                            userPanel_scaleCircle = userControl_scaleCircle_StandUp_AOD;
                            userPanel_scaleLinear = userControl_scaleLinear_StandUp_AOD;
                            userControl_SystemFont_Group = userControl_SystemFont_Group_StandUp_AOD;
                            userControl_icon = userControl_icon_StandUp_AOD;
                            break;
                        case "Weather":
                            userPanel_pictures = userControl_pictures_weather_AOD;
                            userPanel_text_weather_sunrise = userControl_text_weather_Current_AOD;
                            userControl_icon = userControl_icon_Weather_AOD;
                            break;
                        case "UVindex":
                            userPanel_pictures = userControl_pictures_UVindex_AOD;
                            userControl_segments = userControl_segments_UVindex_AOD;
                            userPanel_text = userControl_text_UVindex_AOD;
                            userPanel_hand = userControl_hand_UVindex_AOD;
                            userPanel_scaleCircle = userControl_scaleCircle_UVindex_AOD;
                            userPanel_scaleLinear = userControl_scaleLinear_UVindex_AOD;
                            userControl_SystemFont_Group = userControl_SystemFont_Group_UVindex_AOD;
                            userControl_icon = userControl_icon_UVindex_AOD;
                            break;
                        case "AirQuality":
                            //userPanel_pictures = userControl_pictures_AirQuality_AOD;
                            //userControl_segments = userControl_segments_AirQuality_AOD;
                            //userPanel_text = userControl_text_AirQuality_AOD;
                            //userPanel_hand = userControl_hand_AirQuality_AOD;
                            //userPanel_scaleCircle = userControl_scaleCircle_AirQuality_AOD;
                            //userPanel_scaleLinear = userControl_scaleLinear_AirQuality_AOD;
                            //userControl_SystemFont_Group = userControl_SystemFont_Group_AirQuality_AOD;
                            //userControl_icon = userControl_icon_AirQuality_AOD;
                            break;
                        case "Humidity":
                            userPanel_pictures = userControl_pictures_Humidity_AOD;
                            userControl_segments = userControl_segments_Humidity_AOD;
                            userPanel_text = userControl_text_Humidity_AOD;
                            userPanel_hand = userControl_hand_Humidity_AOD;
                            userPanel_scaleCircle = userControl_scaleCircle_Humidity_AOD;
                            userPanel_scaleLinear = userControl_scaleLinear_Humidity_AOD;
                            userControl_SystemFont_Group = userControl_SystemFont_Group_Humidity_AOD;
                            userControl_icon = userControl_icon_Humidity_AOD;
                            break;
                        case "Sunrise":
                            userPanel_pictures = userControl_pictures_Sunrise_AOD;
                            userPanel_text = userControl_text_SunriseSunset_AOD;
                            userPanel_hand = userControl_hand_Sunrise_AOD;
                            userPanel_scaleCircle = userControl_scaleCircle_Sunrise_AOD;
                            userPanel_scaleLinear = userControl_scaleLinear_Sunrise_AOD;
                            userControl_icon = userControl_icon_Sunrise_AOD;
                            break;
                        case "WindForce":
                            userPanel_pictures = userControl_pictures_WindForce_AOD;
                            userControl_segments = userControl_segments_WindForce_AOD;
                            userPanel_text = userControl_text_WindForce_AOD;
                            userPanel_hand = userControl_hand_WindForce_AOD;
                            userPanel_scaleCircle = userControl_scaleCircle_WindForce_AOD;
                            userPanel_scaleLinear = userControl_scaleLinear_WindForce_AOD;
                            userControl_SystemFont_Group = userControl_SystemFont_Group_WindForce_AOD;
                            userControl_icon = userControl_icon_WindForce_AOD;
                            break;
                        case "Altitude":
                            //userPanel_pictures = userControl_pictures_Altitude_AOD;
                            //userControl_segments = userControl_segments_Altitude_AOD;
                            //userPanel_text = userControl_text_Altitude_AOD;
                            //userPanel_hand = userControl_hand_Altitude_AOD;
                            //userPanel_scaleCircle = userControl_scaleCircle_Altitude_AOD;
                            //userPanel_scaleLinear = userControl_scaleLinear_Altitude_AOD;
                            //userControl_SystemFont_Group = userControl_SystemFont_Group_Altitude_AOD;
                            //userControl_icon = userControl_icon_Altitude_AOD;
                            break;
                        case "AirPressure":
                            userPanel_pictures = userControl_pictures_AirPressure_AOD;
                            userControl_segments = userControl_segments_AirPressure_AOD;
                            userPanel_text = userControl_text_AirPressure_AOD;
                            userPanel_hand = userControl_hand_AirPressure_AOD;
                            userPanel_scaleCircle = userControl_scaleCircle_AirPressure_AOD;
                            userPanel_scaleLinear = userControl_scaleLinear_AirPressure_AOD;
                            userControl_SystemFont_Group = userControl_SystemFont_Group_AirPressure_AOD;
                            userControl_icon = userControl_icon_AirPressure_AOD;
                            break;
                        case "Stress":
                            //userPanel_pictures = userControl_pictures_Stress_AOD;
                            //userControl_segments = userControl_segments_Stress_AOD;
                            //userPanel_text = userControl_text_Stress_AOD;
                            //userPanel_hand = userControl_hand_Stress_AOD;
                            //userPanel_scaleCircle = userControl_scaleCircle_Stress_AOD;
                            //userPanel_scaleLinear = userControl_scaleLinear_Stress_AOD;
                            //userControl_SystemFont_Group = userControl_SystemFont_Group_Stress_AOD;
                            //userControl_icon = userControl_icon_Stress_AOD;
                            break;
                        case "ActivityGoal":
                            userPanel_pictures = userControl_pictures_ActivityGoal_AOD;
                            userControl_segments = userControl_segments_ActivityGoal_AOD;
                            userPanel_text = userControl_text_ActivityGoal_AOD;
                            userPanel_textGoal = userControl_text_goal_ActivityGoal_AOD;
                            userPanel_hand = userControl_hand_ActivityGoal_AOD;
                            userPanel_scaleCircle = userControl_scaleCircle_ActivityGoal_AOD;
                            userPanel_scaleLinear = userControl_scaleLinear_ActivityGoal_AOD;
                            userControl_SystemFont_Group = userControl_SystemFont_Group_ActivityGoal_AOD;
                            userControl_icon = userControl_icon_ActivityGoal_AOD;
                            break;
                        case "FatBurning":
                            userPanel_pictures = userControl_pictures_FatBurning_AOD;
                            userControl_segments = userControl_segments_FatBurning_AOD;
                            userPanel_text = userControl_text_FatBurning_AOD;
                            userPanel_textGoal = userControl_text_goal_FatBurning_AOD;
                            userPanel_hand = userControl_hand_FatBurning_AOD;
                            userPanel_scaleCircle = userControl_scaleCircle_FatBurning_AOD;
                            userPanel_scaleLinear = userControl_scaleLinear_FatBurning_AOD;
                            userControl_SystemFont_Group = userControl_SystemFont_Group_FatBurning_AOD;
                            userControl_icon = userControl_icon_FatBurning_AOD;
                            break;
                    }

                    if (userControl_SystemFont_Group != null)
                    {
                        userControl_SystemFont = userControl_SystemFont_Group.userControl_SystemFont;
                        userControl_FontRotate = userControl_SystemFont_Group.userControl_FontRotate;
                        userControl_SystemFontGoal = userControl_SystemFont_Group.userControl_SystemFont_goal;
                        userControl_FontRotateGoal = userControl_SystemFont_Group.userControl_FontRotate_goal;
                    }

                    // набор картинок
                    if (userPanel_pictures != null)
                    {
                        if (activity.ImageProgress != null && activity.ImageProgress.ImageSet != null &&
                            activity.ImageProgress.Coordinates != null && OneCoordinates(activity.ImageProgress.Coordinates))

                        //activity.ImageProgress.Coordinates != null && activity.ImageProgress.Coordinates.Count == 1)
                        {
                            userPanel_pictures.checkBox_pictures_Use.Checked = true;
                            if (activity.Type == "Weather" && weatherAlignmentFix_AOD == 0)
                            {
                                weatherAlignmentFix_AOD = 1;
                                checkBox_weatherAlignmentFix_AOD.Checked = true;
                            }

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

                    // сегменты
                    if (userControl_segments != null)
                    {
                        if (activity.ImageProgress != null && activity.ImageProgress.ImageSet != null &&
                            activity.ImageProgress.Coordinates != null && !OneCoordinates(activity.ImageProgress.Coordinates))
                        {
                            userControl_segments.checkBox_pictures_Use.Checked = true;
                            userControl_segments.comboBoxSetImage(activity.ImageProgress.ImageSet.ImageIndex);
                            userControl_segments.SetCoordinates(activity.ImageProgress.Coordinates);
                            userControl_segments.radioButtonSetDisplayType(activity.ImageProgress.DisplayType);
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
                                    if (activity.Type == "Distance")
                                    {
                                        if (digitalCommonDigit.Digit.Image.MultilangImageUnitMile != null)
                                        {
                                            foreach (MultilangImage multilangImage in digitalCommonDigit.Digit.Image.MultilangImageUnitMile)
                                            {
                                                if (multilangImage.LangCode == "All")
                                                    userPanel_text_Activity.comboBoxSetUnitMile((int)multilangImage.ImageSet.ImageIndex);
                                            }
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
                            userControl_SystemFont_GroupWeather_AOD;
                        UserControl_SystemFont_weather userControl_SystemFont_weather = null;
                        UserControl_FontRotate_weather userControl_FontRotate_weather = null;

                        if (activity.Digits != null && activity.Digits.Count > 0)
                        {
                            foreach (DigitalCommonDigit digitalCommonDigit in activity.Digits)
                            {
                                if (digitalCommonDigit.Type != null && digitalCommonDigit.Type == "Min")
                                {
                                    userPanel_text_weather_sunrise = userControl_text_weather_Min_AOD;
                                    userControl_SystemFont_weather =
                                        userControl_SystemFont_Group_Weather.userControl_SystemFont_weather_Min;
                                    userControl_FontRotate_weather =
                                        userControl_SystemFont_Group_Weather.userControl_FontRotate_weather_Min;
                                }
                                if (digitalCommonDigit.Type != null && digitalCommonDigit.Type == "Max")
                                {
                                    userPanel_text_weather_sunrise = userControl_text_weather_Max_AOD;
                                    userControl_SystemFont_weather =
                                        userControl_SystemFont_Group_Weather.userControl_SystemFont_weather_Max;
                                    userControl_FontRotate_weather =
                                        userControl_SystemFont_Group_Weather.userControl_FontRotate_weather_Max;
                                }
                                if (digitalCommonDigit.Type == null)
                                {
                                    userPanel_text_weather_sunrise = userControl_text_weather_Current_AOD;
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

                        if (weatherAlignmentFix_AOD == 0)
                        {
                            weatherAlignmentFix_AOD = -1;
                            checkBox_weatherAlignmentFix_AOD.Checked = false;
                        }
                    }
                    else if (activity.Type == "Sunrise")
                    {
                        UserControl_SystemFont_GroupWeather userControl_SystemFont_Group_Weather =
                            userControl_SystemFont_GroupSunrise_AOD;
                        UserControl_SystemFont_weather userControl_SystemFont_weather = null;
                        UserControl_FontRotate_weather userControl_FontRotate_weather = null;

                        if (activity.Digits != null && activity.Digits.Count > 0)
                        {
                            foreach (DigitalCommonDigit digitalCommonDigit in activity.Digits)
                            {
                                if (digitalCommonDigit.Type != null && digitalCommonDigit.Type == "Min")
                                {
                                    userPanel_text_weather_sunrise = userControl_text_Sunrise_AOD;
                                    userControl_SystemFont_weather =
                                        userControl_SystemFont_Group_Weather.userControl_SystemFont_weather_Min;
                                    userControl_FontRotate_weather =
                                        userControl_SystemFont_Group_Weather.userControl_FontRotate_weather_Min;
                                }
                                if (digitalCommonDigit.Type != null && digitalCommonDigit.Type == "Max")
                                {
                                    userPanel_text_weather_sunrise = userControl_text_Sunset_AOD;
                                    userControl_SystemFont_weather =
                                        userControl_SystemFont_Group_Weather.userControl_SystemFont_weather_Max;
                                    userControl_FontRotate_weather =
                                        userControl_SystemFont_Group_Weather.userControl_FontRotate_weather_Max;
                                }
                                if (digitalCommonDigit.Type == null)
                                {
                                    userPanel_text_weather_sunrise = userControl_text_SunriseSunset_AOD;
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
            #endregion

            JSON_read_activityLayer_order_AOD();
            JSON_read_dateLayer_order_AOD();

        }

        private void JSON_read_activityLayer_order_AOD()
        {
            if (Watch_Face.ScreenIdle != null && Watch_Face.ScreenIdle.Activity != null)
            {
                int count = 0;
                Dictionary<int, string> activity_layer = new Dictionary<int, string>();
                foreach (Activity activity in Watch_Face.ScreenIdle.Activity)
                {
                    switch (activity.Type)
                    {
                        case "Battery":
                            if (!activity_layer.ContainsValue("Battery") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "Battery");
                                count++;
                            }
                            break;
                        case "Steps":
                            if (!activity_layer.ContainsValue("Steps") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "Steps");
                                count++;
                            }
                            break;
                        case "Calories":
                            if (!activity_layer.ContainsValue("Calories") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "Calories");
                                count++;
                            }
                            break;
                        case "HeartRate":
                            if (!activity_layer.ContainsValue("HeartRate") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "HeartRate");
                                count++;
                            }
                            break;
                        case "PAI":
                            if (!activity_layer.ContainsValue("PAI") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "PAI");
                                count++;
                            }
                            break;
                        case "Distance":
                            if (!activity_layer.ContainsValue("Distance") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "Distance");
                                count++;
                            }
                            break;
                        case "StandUp":
                            if (!activity_layer.ContainsValue("StandUp") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "StandUp");
                                count++;
                            }
                            break;
                        case "Weather":
                            if (!activity_layer.ContainsValue("Weather") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "Weather");
                                count++;
                            }
                            break;
                        case "UVindex":
                            if (!activity_layer.ContainsValue("UVindex") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "UVindex");
                                count++;
                            }
                            break;
                        case "AirQuality":
                            if (!activity_layer.ContainsValue("AirQuality") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "AirQuality");
                                count++;
                            }
                            break;
                        case "Humidity":
                            if (!activity_layer.ContainsValue("Humidity") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "Humidity");
                                count++;
                            }
                            break;
                        case "Sunrise":
                            if (!activity_layer.ContainsValue("Sunrise") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "Sunrise");
                                count++;
                            }
                            break;
                        case "WindForce":
                            if (!activity_layer.ContainsValue("WindForce") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "WindForce");
                                count++;
                            }
                            break;
                        case "Altitude":
                            if (!activity_layer.ContainsValue("Altitude") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "Altitude");
                                count++;
                            }
                            break;
                        case "AirPressure":
                            if (!activity_layer.ContainsValue("AirPressure") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "AirPressure");
                                count++;
                            }
                            break;
                        case "Stress":
                            if (!activity_layer.ContainsValue("Stress") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "Stress");
                                count++;
                            }
                            break;
                        case "ActivityGoal":
                            if (!activity_layer.ContainsValue("ActivityGoal") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "ActivityGoal");
                                count++;
                            }
                            break;
                        case "FatBurning":
                            if (!activity_layer.ContainsValue("FatBurning") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "FatBurning");
                                count++;
                            }
                            break;
                    }
                }

                if (activity_layer.Count > 0)
                {
                    for (int i = 0; i < activity_layer.Count; i++)
                    {
                        DataGridView_MoveRow(dataGridView_AODL_Activity, i, activity_layer[i]);
                    }
                }
                dataGridView_AODL_Activity.ClearSelection();
            }
        }

        private void JSON_read_dateLayer_order_AOD()
        {
            if (Watch_Face.ScreenIdle != null && Watch_Face.ScreenIdle.Date != null && Watch_Face.ScreenIdle.Date.DateDigits != null)
            {
                int count = 0;
                Dictionary<int, string> date_layer = new Dictionary<int, string>();
                foreach (DigitalDateDigit date in Watch_Face.ScreenIdle.Date.DateDigits)
                {
                    switch (date.DateType)
                    {
                        case "Day":
                            if (!date_layer.ContainsValue("Day") && !date_layer.ContainsKey(count))
                            {
                                date_layer.Add(count, "Day");
                                count++;
                            }
                            break;
                        case "Month":
                            if (!date_layer.ContainsValue("Month") && !date_layer.ContainsKey(count))
                            {
                                date_layer.Add(count, "Month");
                                count++;
                            }
                            break;
                        case "Year":
                            if (!date_layer.ContainsValue("Year") && !date_layer.ContainsKey(count))
                            {
                                date_layer.Add(count, "Year");
                                count++;
                            }
                            break;
                    }
                }

                if (date_layer.Count > 0)
                {
                    for (int i = 0; i < date_layer.Count; i++)
                    {
                        DataGridView_MoveRow(dataGridView_AODL_Date, i, date_layer[i]);
                    }
                }
                dataGridView_AODL_Date.ClearSelection();
            }
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
                    if (AOD_24h) multilangImage.ImageSet.ImagesCount = 24;
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
                if (AOD_24h) digitalTimeDigit.Digit.DisplayFormAnalog = true;


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

            AddActivityTime_AOD(ScreenIdle, null, null, null, userControl_SystemFont_GroupTime_AOD);
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

            for (int i = 0; i < dataGridView_AODL_Date.RowCount; i++)
            {

                string dataName = dataGridView_AODL_Date.Rows[i].Cells[0].Value.ToString();

                // год
                if (dataName == "Year")
                {
                    if (checkBox_Year_text_Use_AOD.Checked && comboBox_Year_image_AOD.SelectedIndex >= 0)
                    {
                        if (ScreenIdle.Date == null) ScreenIdle.Date = new DateAmazfit();
                        if (ScreenIdle.Date.DateDigits == null)
                            ScreenIdle.Date.DateDigits = new List<DigitalDateDigit>();

                        DigitalDateDigit digitalDateDigit = new DigitalDateDigit();
                        digitalDateDigit.DateType = "Year";
                        //digitalDateDigit.CombingMode = "Single";
                        digitalDateDigit.CombingMode = checkBox_Year_follow_AOD.Checked ? "Follow" : "Single";
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

                    // данные системным шрифтом
                    UserControl_SystemFont userControl_SystemFont =
                        userControl_SystemFont_Group_Year_AOD.userControl_SystemFont;
                    if (userControl_SystemFont != null && userControl_SystemFont.checkBox_Use.Checked)
                    {
                        NumericUpDown numericUpDownX = userControl_SystemFont.numericUpDown_SystemFontX;
                        NumericUpDown numericUpDownY = userControl_SystemFont.numericUpDown_SystemFontY;
                        NumericUpDown numericUpDown_size = userControl_SystemFont.numericUpDown_SystemFont_size;
                        NumericUpDown numericUpDown_angle = userControl_SystemFont.numericUpDown_SystemFont_angle;
                        NumericUpDown numericUpDown_spacing = userControl_SystemFont.numericUpDown_SystemFont_spacing;
                        bool follow = userControl_SystemFont.checkBox_follow.Checked;
                        bool add_zero = userControl_SystemFont.checkBox_addZero.Checked;
                        bool separator = userControl_SystemFont.checkBox_separator.Checked;

                        if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                        if (ScreenIdle.Date == null) ScreenIdle.Date = new DateAmazfit();
                        if (ScreenIdle.Date.DateDigits == null)
                            ScreenIdle.Date.DateDigits = new List<DigitalDateDigit>();

                        DigitalDateDigit digitalDateDigit = new DigitalDateDigit();
                        digitalDateDigit.DateType = "Year";
                        if (!follow) digitalDateDigit.CombingMode = "Single";
                        if (separator)
                        {
                            digitalDateDigit.Separator = new ImageCoord();
                            digitalDateDigit.Separator.Coordinates = new Coordinates();
                            digitalDateDigit.Separator.Coordinates.X = -1;
                            digitalDateDigit.Separator.Coordinates.Y = -1;
                        }


                        if (digitalDateDigit.Digit == null) digitalDateDigit.Digit = new Text();
                        if (digitalDateDigit.Digit.SystemFont == null)
                            digitalDateDigit.Digit.SystemFont = new SystemFont();
                        if (digitalDateDigit.Digit.SystemFont.Coordinates == null)
                            digitalDateDigit.Digit.SystemFont.Coordinates = new Coordinates();

                        digitalDateDigit.Digit.PaddingZero = add_zero;
                        digitalDateDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;

                        digitalDateDigit.Digit.SystemFont.Coordinates.X = (long)numericUpDownX.Value;
                        digitalDateDigit.Digit.SystemFont.Coordinates.Y = (long)numericUpDownY.Value;
                        digitalDateDigit.Digit.SystemFont.Size = (long)numericUpDown_size.Value;
                        digitalDateDigit.Digit.SystemFont.Angle = (long)numericUpDown_angle.Value;

                        digitalDateDigit.Digit.SystemFont.ShowUnitCheck = userControl_SystemFont.checkBoxGetUnit();
                        digitalDateDigit.Digit.SystemFont.Color = userControl_SystemFont.comboBoxGetColorString();

                        ScreenIdle.Date.DateDigits.Add(digitalDateDigit);
                    }

                    // данные системным шрифтом по окружности

                    UserControl_FontRotate userControl_FontRotate =
                        userControl_SystemFont_Group_Year_AOD.userControl_FontRotate;
                    if (userControl_FontRotate != null && userControl_FontRotate.checkBox_Use.Checked)
                    {
                        NumericUpDown numericUpDownX = userControl_FontRotate.numericUpDown_FontRotateX;
                        NumericUpDown numericUpDownY = userControl_FontRotate.numericUpDown_FontRotateY;
                        NumericUpDown numericUpDown_size = userControl_FontRotate.numericUpDown_FontRotate_size;
                        NumericUpDown numericUpDown_angle = userControl_FontRotate.numericUpDown_FontRotate_angle;
                        NumericUpDown numericUpDown_radius = userControl_FontRotate.numericUpDown_FontRotate_radius;
                        NumericUpDown numericUpDown_spacing = userControl_FontRotate.numericUpDown_FontRotate_spacing;
                        bool follow = userControl_FontRotate.checkBox_follow.Checked;
                        bool add_zero = userControl_FontRotate.checkBox_addZero.Checked;
                        bool separator = userControl_FontRotate.checkBox_separator.Checked;

                        if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                        if (ScreenIdle.Date == null) ScreenIdle.Date = new DateAmazfit();
                        if (ScreenIdle.Date.DateDigits == null)
                            ScreenIdle.Date.DateDigits = new List<DigitalDateDigit>();

                        DigitalDateDigit digitalDateDigit = new DigitalDateDigit();
                        digitalDateDigit.DateType = "Year";
                        if (!follow) digitalDateDigit.CombingMode = "Single";
                        if (separator)
                        {
                            digitalDateDigit.Separator = new ImageCoord();
                            digitalDateDigit.Separator.Coordinates = new Coordinates();
                            digitalDateDigit.Separator.Coordinates.X = -1;
                            digitalDateDigit.Separator.Coordinates.Y = -1;
                        }

                        if (digitalDateDigit.Digit == null) digitalDateDigit.Digit = new Text();
                        if (digitalDateDigit.Digit.SystemFont == null)
                            digitalDateDigit.Digit.SystemFont = new SystemFont();
                        if (digitalDateDigit.Digit.SystemFont.FontRotate == null)
                            digitalDateDigit.Digit.SystemFont.FontRotate = new FontRotate();

                        digitalDateDigit.Digit.PaddingZero = add_zero;
                        digitalDateDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;

                        digitalDateDigit.Digit.SystemFont.FontRotate.X = (long)numericUpDownX.Value;
                        digitalDateDigit.Digit.SystemFont.FontRotate.Y = (long)numericUpDownY.Value;
                        digitalDateDigit.Digit.SystemFont.FontRotate.Radius = (long)numericUpDown_radius.Value;
                        digitalDateDigit.Digit.SystemFont.FontRotate.RotateDirection =
                            userControl_FontRotate.radioButtonGetRotateDirection();
                        digitalDateDigit.Digit.SystemFont.Size = (long)numericUpDown_size.Value;
                        digitalDateDigit.Digit.SystemFont.Angle = (long)numericUpDown_angle.Value;

                        digitalDateDigit.Digit.SystemFont.ShowUnitCheck = userControl_FontRotate.checkBoxGetUnit();
                        digitalDateDigit.Digit.SystemFont.Color = userControl_FontRotate.comboBoxGetColorString();

                        ScreenIdle.Date.DateDigits.Add(digitalDateDigit);
                    }
                }

                // месяц
                if (dataName == "Month")
                {
                    if (checkBox_Month_Use_AOD.Checked && comboBox_Month_image_AOD.SelectedIndex >= 0)
                    {
                        if (ScreenIdle.Date == null) ScreenIdle.Date = new DateAmazfit();
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

                    // данные системным шрифтом
                    UserControl_SystemFont userControl_SystemFont =
                        userControl_SystemFont_Group_Month_AOD.userControl_SystemFont;
                    if (userControl_SystemFont != null && userControl_SystemFont.checkBox_Use.Checked)
                    {
                        NumericUpDown numericUpDownX = userControl_SystemFont.numericUpDown_SystemFontX;
                        NumericUpDown numericUpDownY = userControl_SystemFont.numericUpDown_SystemFontY;
                        NumericUpDown numericUpDown_size = userControl_SystemFont.numericUpDown_SystemFont_size;
                        NumericUpDown numericUpDown_angle = userControl_SystemFont.numericUpDown_SystemFont_angle;
                        NumericUpDown numericUpDown_spacing = userControl_SystemFont.numericUpDown_SystemFont_spacing;
                        bool follow = userControl_SystemFont.checkBox_follow.Checked;
                        bool add_zero = userControl_SystemFont.checkBox_addZero.Checked;
                        bool separator = userControl_SystemFont.checkBox_separator.Checked;

                        if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                        if (ScreenIdle.Date == null) ScreenIdle.Date = new DateAmazfit();
                        if (ScreenIdle.Date.DateDigits == null)
                            ScreenIdle.Date.DateDigits = new List<DigitalDateDigit>();

                        DigitalDateDigit digitalDateDigit = new DigitalDateDigit();
                        digitalDateDigit.DateType = "Month";
                        if (!follow) digitalDateDigit.CombingMode = "Single";
                        if (separator)
                        {
                            digitalDateDigit.Separator = new ImageCoord();
                            digitalDateDigit.Separator.Coordinates = new Coordinates();
                            digitalDateDigit.Separator.Coordinates.X = -1;
                            digitalDateDigit.Separator.Coordinates.Y = -1;
                        }


                        if (digitalDateDigit.Digit == null) digitalDateDigit.Digit = new Text();
                        if (digitalDateDigit.Digit.SystemFont == null)
                            digitalDateDigit.Digit.SystemFont = new SystemFont();
                        if (digitalDateDigit.Digit.SystemFont.Coordinates == null)
                            digitalDateDigit.Digit.SystemFont.Coordinates = new Coordinates();

                        digitalDateDigit.Digit.PaddingZero = add_zero;
                        digitalDateDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;

                        digitalDateDigit.Digit.SystemFont.Coordinates.X = (long)numericUpDownX.Value;
                        digitalDateDigit.Digit.SystemFont.Coordinates.Y = (long)numericUpDownY.Value;
                        digitalDateDigit.Digit.SystemFont.Size = (long)numericUpDown_size.Value;
                        digitalDateDigit.Digit.SystemFont.Angle = (long)numericUpDown_angle.Value;

                        digitalDateDigit.Digit.SystemFont.ShowUnitCheck = userControl_SystemFont.checkBoxGetUnit();
                        digitalDateDigit.Digit.SystemFont.Color = userControl_SystemFont.comboBoxGetColorString();

                        ScreenIdle.Date.DateDigits.Add(digitalDateDigit);
                    }

                    // данные системным шрифтом по окружности
                    UserControl_FontRotate userControl_FontRotate =
                        userControl_SystemFont_Group_Month_AOD.userControl_FontRotate;
                    if (userControl_FontRotate != null && userControl_FontRotate.checkBox_Use.Checked)
                    {
                        NumericUpDown numericUpDownX = userControl_FontRotate.numericUpDown_FontRotateX;
                        NumericUpDown numericUpDownY = userControl_FontRotate.numericUpDown_FontRotateY;
                        NumericUpDown numericUpDown_size = userControl_FontRotate.numericUpDown_FontRotate_size;
                        NumericUpDown numericUpDown_angle = userControl_FontRotate.numericUpDown_FontRotate_angle;
                        NumericUpDown numericUpDown_radius = userControl_FontRotate.numericUpDown_FontRotate_radius;
                        NumericUpDown numericUpDown_spacing = userControl_FontRotate.numericUpDown_FontRotate_spacing;
                        bool follow = userControl_FontRotate.checkBox_follow.Checked;
                        bool add_zero = userControl_FontRotate.checkBox_addZero.Checked;
                        bool separator = userControl_FontRotate.checkBox_separator.Checked;

                        if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                        if (ScreenIdle.Date == null) ScreenIdle.Date = new DateAmazfit();
                        if (ScreenIdle.Date.DateDigits == null)
                            ScreenIdle.Date.DateDigits = new List<DigitalDateDigit>();

                        DigitalDateDigit digitalDateDigit = new DigitalDateDigit();
                        digitalDateDigit.DateType = "Month";
                        if (!follow) digitalDateDigit.CombingMode = "Single";
                        if (separator)
                        {
                            digitalDateDigit.Separator = new ImageCoord();
                            digitalDateDigit.Separator.Coordinates = new Coordinates();
                            digitalDateDigit.Separator.Coordinates.X = -1;
                            digitalDateDigit.Separator.Coordinates.Y = -1;
                        }

                        if (digitalDateDigit.Digit == null) digitalDateDigit.Digit = new Text();
                        if (digitalDateDigit.Digit.SystemFont == null)
                            digitalDateDigit.Digit.SystemFont = new SystemFont();
                        if (digitalDateDigit.Digit.SystemFont.FontRotate == null)
                            digitalDateDigit.Digit.SystemFont.FontRotate = new FontRotate();

                        digitalDateDigit.Digit.PaddingZero = add_zero;
                        digitalDateDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;

                        digitalDateDigit.Digit.SystemFont.FontRotate.X = (long)numericUpDownX.Value;
                        digitalDateDigit.Digit.SystemFont.FontRotate.Y = (long)numericUpDownY.Value;
                        digitalDateDigit.Digit.SystemFont.FontRotate.Radius = (long)numericUpDown_radius.Value;
                        digitalDateDigit.Digit.SystemFont.FontRotate.RotateDirection =
                            userControl_FontRotate.radioButtonGetRotateDirection();
                        digitalDateDigit.Digit.SystemFont.Size = (long)numericUpDown_size.Value;
                        digitalDateDigit.Digit.SystemFont.Angle = (long)numericUpDown_angle.Value;

                        digitalDateDigit.Digit.SystemFont.ShowUnitCheck = userControl_FontRotate.checkBoxGetUnit();
                        digitalDateDigit.Digit.SystemFont.Color = userControl_FontRotate.comboBoxGetColorString();

                        ScreenIdle.Date.DateDigits.Add(digitalDateDigit);
                    }

                    // месяц картинкой
                    if (checkBox_Month_pictures_Use_AOD.Checked && comboBox_Month_pictures_image_AOD.SelectedIndex >= 0)
                    {
                        if (ScreenIdle.Date == null) ScreenIdle.Date = new DateAmazfit();
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
                }

                // число
                if (dataName == "Day")
                {
                    if (checkBox_Day_Use_AOD.Checked && comboBox_Day_image_AOD.SelectedIndex >= 0)
                    {
                        if (ScreenIdle.Date == null) ScreenIdle.Date = new DateAmazfit();
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

                    // данные системным шрифтом
                    UserControl_SystemFont userControl_SystemFont =
                        userControl_SystemFont_Group_Day_AOD.userControl_SystemFont;
                    if (userControl_SystemFont != null && userControl_SystemFont.checkBox_Use.Checked)
                    {
                        NumericUpDown numericUpDownX = userControl_SystemFont.numericUpDown_SystemFontX;
                        NumericUpDown numericUpDownY = userControl_SystemFont.numericUpDown_SystemFontY;
                        NumericUpDown numericUpDown_size = userControl_SystemFont.numericUpDown_SystemFont_size;
                        NumericUpDown numericUpDown_angle = userControl_SystemFont.numericUpDown_SystemFont_angle;
                        NumericUpDown numericUpDown_spacing = userControl_SystemFont.numericUpDown_SystemFont_spacing;
                        bool follow = userControl_SystemFont.checkBox_follow.Checked;
                        bool add_zero = userControl_SystemFont.checkBox_addZero.Checked;
                        bool separator = userControl_SystemFont.checkBox_separator.Checked;

                        if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                        if (ScreenIdle.Date == null) ScreenIdle.Date = new DateAmazfit();
                        if (ScreenIdle.Date.DateDigits == null)
                            ScreenIdle.Date.DateDigits = new List<DigitalDateDigit>();

                        DigitalDateDigit digitalDateDigit = new DigitalDateDigit();
                        digitalDateDigit.DateType = "Day";
                        if (!follow) digitalDateDigit.CombingMode = "Single";
                        if (separator)
                        {
                            digitalDateDigit.Separator = new ImageCoord();
                            digitalDateDigit.Separator.Coordinates = new Coordinates();
                            digitalDateDigit.Separator.Coordinates.X = -1;
                            digitalDateDigit.Separator.Coordinates.Y = -1;
                        }


                        if (digitalDateDigit.Digit == null) digitalDateDigit.Digit = new Text();
                        if (digitalDateDigit.Digit.SystemFont == null)
                            digitalDateDigit.Digit.SystemFont = new SystemFont();
                        if (digitalDateDigit.Digit.SystemFont.Coordinates == null)
                            digitalDateDigit.Digit.SystemFont.Coordinates = new Coordinates();

                        digitalDateDigit.Digit.PaddingZero = add_zero;
                        digitalDateDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;

                        digitalDateDigit.Digit.SystemFont.Coordinates.X = (long)numericUpDownX.Value;
                        digitalDateDigit.Digit.SystemFont.Coordinates.Y = (long)numericUpDownY.Value;
                        digitalDateDigit.Digit.SystemFont.Size = (long)numericUpDown_size.Value;
                        digitalDateDigit.Digit.SystemFont.Angle = (long)numericUpDown_angle.Value;

                        digitalDateDigit.Digit.SystemFont.ShowUnitCheck = userControl_SystemFont.checkBoxGetUnit();
                        digitalDateDigit.Digit.SystemFont.Color = userControl_SystemFont.comboBoxGetColorString();

                        ScreenIdle.Date.DateDigits.Add(digitalDateDigit);
                    }

                    // данные системным шрифтом по окружности
                    UserControl_FontRotate userControl_FontRotate =
                        userControl_SystemFont_Group_Day_AOD.userControl_FontRotate;
                    if (userControl_FontRotate != null && userControl_FontRotate.checkBox_Use.Checked)
                    {
                        NumericUpDown numericUpDownX = userControl_FontRotate.numericUpDown_FontRotateX;
                        NumericUpDown numericUpDownY = userControl_FontRotate.numericUpDown_FontRotateY;
                        NumericUpDown numericUpDown_size = userControl_FontRotate.numericUpDown_FontRotate_size;
                        NumericUpDown numericUpDown_angle = userControl_FontRotate.numericUpDown_FontRotate_angle;
                        NumericUpDown numericUpDown_radius = userControl_FontRotate.numericUpDown_FontRotate_radius;
                        NumericUpDown numericUpDown_spacing = userControl_FontRotate.numericUpDown_FontRotate_spacing;
                        bool follow = userControl_FontRotate.checkBox_follow.Checked;
                        bool add_zero = userControl_FontRotate.checkBox_addZero.Checked;
                        bool separator = userControl_FontRotate.checkBox_separator.Checked;

                        if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                        if (ScreenIdle.Date == null) ScreenIdle.Date = new DateAmazfit();
                        if (ScreenIdle.Date.DateDigits == null)
                            ScreenIdle.Date.DateDigits = new List<DigitalDateDigit>();

                        DigitalDateDigit digitalDateDigit = new DigitalDateDigit();
                        digitalDateDigit.DateType = "Day";
                        if (!follow) digitalDateDigit.CombingMode = "Single";
                        if (separator)
                        {
                            digitalDateDigit.Separator = new ImageCoord();
                            digitalDateDigit.Separator.Coordinates = new Coordinates();
                            digitalDateDigit.Separator.Coordinates.X = -1;
                            digitalDateDigit.Separator.Coordinates.Y = -1;
                        }

                        if (digitalDateDigit.Digit == null) digitalDateDigit.Digit = new Text();
                        if (digitalDateDigit.Digit.SystemFont == null)
                            digitalDateDigit.Digit.SystemFont = new SystemFont();
                        if (digitalDateDigit.Digit.SystemFont.FontRotate == null)
                            digitalDateDigit.Digit.SystemFont.FontRotate = new FontRotate();

                        digitalDateDigit.Digit.PaddingZero = add_zero;
                        digitalDateDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;

                        digitalDateDigit.Digit.SystemFont.FontRotate.X = (long)numericUpDownX.Value;
                        digitalDateDigit.Digit.SystemFont.FontRotate.Y = (long)numericUpDownY.Value;
                        digitalDateDigit.Digit.SystemFont.FontRotate.Radius = (long)numericUpDown_radius.Value;
                        digitalDateDigit.Digit.SystemFont.FontRotate.RotateDirection =
                            userControl_FontRotate.radioButtonGetRotateDirection();
                        digitalDateDigit.Digit.SystemFont.Size = (long)numericUpDown_size.Value;
                        digitalDateDigit.Digit.SystemFont.Angle = (long)numericUpDown_angle.Value;

                        digitalDateDigit.Digit.SystemFont.ShowUnitCheck = userControl_FontRotate.checkBoxGetUnit();
                        digitalDateDigit.Digit.SystemFont.Color = userControl_FontRotate.comboBoxGetColorString();

                        ScreenIdle.Date.DateDigits.Add(digitalDateDigit);
                    }
                }
            }

            // день недели картинкой
            if (checkBox_DOW_pictures_Use_AOD.Checked && comboBox_DOW_pictures_image_AOD.SelectedIndex >= 0)
            {
                if (ScreenIdle.Date == null) ScreenIdle.Date = new DateAmazfit();
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
                    ScreenIdle.Date = new DateAmazfit();
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

            // месяц стрелкой
            if (checkBox_Month_hand_Use_AOD.Checked && comboBox_Month_hand_image_AOD.SelectedIndex >= 0)
            {
                if (ScreenIdle.Date == null)
                    ScreenIdle.Date = new DateAmazfit();
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

            // число стрелкой
            if (checkBox_Day_hand_Use_AOD.Checked && comboBox_Day_hand_image_AOD.SelectedIndex >= 0)
            {
                if (ScreenIdle.Date == null)
                    ScreenIdle.Date = new DateAmazfit();
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
            UserControl_segments userControl_segments = null;
            UserControl_text userPanel_text;
            UserControl_text userPanel_textGoal = null;
            UserControl_hand userPanel_hand;
            UserControl_scaleCircle userPanel_scaleCircle;
            UserControl_scaleLinear userPanel_scaleLinear;
            UserControl_SystemFont_Group userControl_SystemFont_Group = null;
            UserControl_icon userControl_icon;

            for (int i = 0; i < dataGridView_AODL_Activity.RowCount; i++)
            {
                string activityName = dataGridView_AODL_Activity.Rows[i].Cells[0].Value.ToString();

                #region Battery

                if (activityName == "Battery")
                {
                    userPanel_pictures = userControl_pictures_Battery_AOD;
                    userControl_segments = userControl_segments_Battery_AOD;
                    userPanel_text = userControl_text_Battery_AOD;
                    userPanel_hand = userControl_hand_Battery_AOD;
                    userPanel_scaleCircle = userControl_scaleCircle_Battery_AOD;
                    userPanel_scaleLinear = userControl_scaleLinear_Battery_AOD;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_Battery_AOD;
                    userControl_icon = userControl_icon_Battery_AOD;

                    AddActivity_AOD(ScreenIdle, userPanel_pictures, userControl_segments, userPanel_text, userPanel_textGoal, userPanel_hand,
                        userPanel_scaleCircle, userPanel_scaleLinear, userControl_SystemFont_Group,
                        userControl_icon, "Battery"); 
                }

                #endregion

                #region Steps

                if (activityName == "Steps")
                {
                    userPanel_pictures = userControl_pictures_Steps_AOD;
                    userControl_segments = userControl_segments_Steps_AOD;
                    userPanel_text = userControl_text_Steps_AOD;
                    userPanel_textGoal = userControl_text_goal_Steps_AOD;
                    userPanel_hand = userControl_hand_Steps_AOD;
                    userPanel_scaleCircle = userControl_scaleCircle_Steps_AOD;
                    userPanel_scaleLinear = userControl_scaleLinear_Steps_AOD;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_Steps_AOD;
                    userControl_icon = userControl_icon_Steps_AOD;

                    AddActivity_AOD(ScreenIdle, userPanel_pictures, userControl_segments, userPanel_text, userPanel_textGoal, userPanel_hand,
                        userPanel_scaleCircle, userPanel_scaleLinear, userControl_SystemFont_Group,
                        userControl_icon, "Steps");

                    userPanel_textGoal = null;
                }
                #endregion

                #region Calories

                if (activityName == "Calories")
                {
                    userPanel_pictures = userControl_pictures_Calories_AOD;
                    userControl_segments = userControl_segments_Calories_AOD;
                    userPanel_text = userControl_text_Calories_AOD;
                    userPanel_textGoal = userControl_text_goal_Calories_AOD;
                    userPanel_hand = userControl_hand_Calories_AOD;
                    userPanel_scaleCircle = userControl_scaleCircle_Calories_AOD;
                    userPanel_scaleLinear = userControl_scaleLinear_Calories_AOD;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_Calories_AOD;
                    userControl_icon = userControl_icon_Calories_AOD;

                    AddActivity_AOD(ScreenIdle, userPanel_pictures, userControl_segments, userPanel_text, userPanel_textGoal, userPanel_hand,
                        userPanel_scaleCircle, userPanel_scaleLinear, userControl_SystemFont_Group,
                        userControl_icon, "Calories");

                    userPanel_textGoal = null;
                }
                #endregion

                #region HeartRate

                if (activityName == "HeartRate")
                {
                    userPanel_pictures = userControl_pictures_HeartRate_AOD;
                    userControl_segments = userControl_segments_HeartRate_AOD;
                    userPanel_text = userControl_text_HeartRate_AOD;
                    userPanel_hand = userControl_hand_HeartRate_AOD;
                    userPanel_scaleCircle = userControl_scaleCircle_HeartRate_AOD;
                    userPanel_scaleLinear = userControl_scaleLinear_HeartRate_AOD;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_HeartRate_AOD;
                    userControl_icon = userControl_icon_HeartRate_AOD;

                    AddActivity_AOD(ScreenIdle, userPanel_pictures, userControl_segments, userPanel_text, userPanel_textGoal, userPanel_hand,
                        userPanel_scaleCircle, userPanel_scaleLinear, userControl_SystemFont_Group,
                        userControl_icon, "HeartRate"); 
                }

                #endregion

                #region PAI

                if (activityName == "PAI")
                {
                    userPanel_pictures = userControl_pictures_PAI_AOD;
                    userControl_segments = userControl_segments_PAI_AOD;
                    userPanel_text = userControl_text_PAI_AOD;
                    userPanel_hand = userControl_hand_PAI_AOD;
                    userPanel_scaleCircle = userControl_scaleCircle_PAI_AOD;
                    userPanel_scaleLinear = userControl_scaleLinear_PAI_AOD;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_PAI_AOD;
                    userControl_icon = userControl_icon_PAI_AOD;

                    AddActivity_AOD(ScreenIdle, userPanel_pictures, userControl_segments, userPanel_text, userPanel_textGoal, userPanel_hand,
                        userPanel_scaleCircle, userPanel_scaleLinear, userControl_SystemFont_Group,
                        userControl_icon, "PAI"); 
                }

                #endregion

                #region Distance

                if (activityName == "Distance")
                {
                    userPanel_pictures = null;
                    userControl_segments = null;
                    userPanel_text = userControl_text_Distance_AOD;
                    userPanel_hand = null;
                    userPanel_scaleCircle = null;
                    userPanel_scaleLinear = null;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_Distance_AOD;
                    userControl_icon = userControl_icon_Distance_AOD;

                    AddActivity_AOD(ScreenIdle, userPanel_pictures, userControl_segments, userPanel_text, userPanel_textGoal, userPanel_hand,
                        userPanel_scaleCircle, userPanel_scaleLinear, userControl_SystemFont_Group,
                        userControl_icon, "Distance"); 
                }

                #endregion

                #region StandUp

                if (activityName == "StandUp")
                {
                    userPanel_pictures = userControl_pictures_StandUp_AOD;
                    userControl_segments = userControl_segments_StandUp_AOD;
                    userPanel_text = userControl_text_StandUp_AOD;
                    userPanel_textGoal = userControl_text_goal_StandUp_AOD;
                    userPanel_hand = userControl_hand_StandUp_AOD;
                    userPanel_scaleCircle = userControl_scaleCircle_StandUp_AOD;
                    userPanel_scaleLinear = userControl_scaleLinear_StandUp_AOD;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_StandUp_AOD;
                    userControl_icon = userControl_icon_StandUp_AOD;

                    AddActivity_AOD(ScreenIdle, userPanel_pictures, userControl_segments, userPanel_text, userPanel_textGoal, userPanel_hand,
                        userPanel_scaleCircle, userPanel_scaleLinear, userControl_SystemFont_Group,
                        userControl_icon, "StandUp");

                    userPanel_textGoal = null; 
                }
                #endregion

                #region Weather

                if (activityName == "Weather")
                {
                    //panel_pictures = panel_Weather_pictures;
                    //panel_text = panel_Weather_text;
                    //panel_hand = panel_Weather_hand;
                    //panel_scaleCircle = panel_Weather_scaleCircle;
                    //panel_scaleLinear = panel_Weather_scaleLinear;
                    //Panel panel_text_min = panel_Weather_textMin;
                    //Panel panel_text_max = panel_Weather_textMax;

                    //AddActivityWeather(panel_pictures, panel_text, panel_text_min, panel_text_max, paneeCircle, panel_scaleLinear);

                    AddActivityWeather_AOD(ScreenIdle, userControl_pictures_weather_AOD, userControl_text_weather_Current_AOD,
                        userControl_text_weather_Min_AOD, userControl_text_weather_Max_AOD, userControl_hand_Weather_AOD,
                        userControl_scaleCircle_Weather_AOD, userControl_scaleLinear_Weather_AOD,
                        userControl_SystemFont_GroupWeather_AOD, userControl_icon_Weather_AOD); 
                }
                #endregion

                #region UVindex

                if (activityName == "UVindex")
                {
                    userPanel_pictures = userControl_pictures_UVindex_AOD;
                    userControl_segments = userControl_segments_UVindex_AOD;
                    userPanel_text = userControl_text_UVindex_AOD;
                    userPanel_hand = userControl_hand_UVindex_AOD;
                    userPanel_scaleCircle = userControl_scaleCircle_UVindex_AOD;
                    userPanel_scaleLinear = userControl_scaleLinear_UVindex_AOD;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_UVindex_AOD;
                    userControl_icon = userControl_icon_UVindex_AOD;

                    AddActivity_AOD(ScreenIdle, userPanel_pictures, userControl_segments, userPanel_text, userPanel_textGoal, userPanel_hand,
                        userPanel_scaleCircle, userPanel_scaleLinear, userControl_SystemFont_Group,
                        userControl_icon, "UVindex"); 
                }

                #endregion

                #region AirQuality

                if (activityName == "AirQuality")
                {
                    userPanel_pictures = userControl_pictures_AirQuality_AOD;
                    userControl_segments = userControl_segments_AirQuality_AOD;
                    userPanel_text = userControl_text_AirQuality_AOD;
                    userPanel_hand = userControl_hand_AirQuality_AOD;
                    userPanel_scaleCircle = userControl_scaleCircle_AirQuality_AOD;
                    userPanel_scaleLinear = userControl_scaleLinear_AirQuality_AOD;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_AirQuality_AOD;
                    userControl_icon = userControl_icon_AirQuality_AOD;

                    AddActivity_AOD(ScreenIdle, userPanel_pictures, userControl_segments, userPanel_text, userPanel_textGoal, userPanel_hand,
                        userPanel_scaleCircle, userPanel_scaleLinear, userControl_SystemFont_Group,
                        userControl_icon, "AirQuality"); 
                }

                #endregion

                #region Humidity

                if (activityName == "Humidity")
                {
                    userPanel_pictures = userControl_pictures_Humidity_AOD;
                    userControl_segments = userControl_segments_Humidity_AOD;
                    userPanel_text = userControl_text_Humidity_AOD;
                    userPanel_hand = userControl_hand_Humidity_AOD;
                    userPanel_scaleCircle = userControl_scaleCircle_Humidity_AOD;
                    userPanel_scaleLinear = userControl_scaleLinear_Humidity_AOD;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_Humidity_AOD;
                    userControl_icon = userControl_icon_Humidity_AOD;

                    AddActivity_AOD(ScreenIdle, userPanel_pictures, userControl_segments, userPanel_text, userPanel_textGoal, userPanel_hand,
                        userPanel_scaleCircle, userPanel_scaleLinear, userControl_SystemFont_Group,
                        userControl_icon, "Humidity"); 
                }

                #endregion

                #region Sunrise

                if (activityName == "Sunrise")
                {
                    AddActivitySunrise_AOD(ScreenIdle, userControl_pictures_Sunrise_AOD, userControl_text_SunriseSunset_AOD,
                                userControl_text_Sunrise_AOD, userControl_text_Sunset_AOD, userControl_hand_Sunrise_AOD,
                                userControl_scaleCircle_Sunrise_AOD, userControl_scaleLinear_Sunrise_AOD,
                                userControl_SystemFont_GroupSunrise_AOD, userControl_icon_Sunrise_AOD); 
                }
                #endregion

                #region WindForce

                if (activityName == "WindForce")
                {
                    userPanel_pictures = userControl_pictures_WindForce_AOD;
                    userControl_segments = userControl_segments_WindForce_AOD;
                    userPanel_text = userControl_text_WindForce_AOD;
                    userPanel_hand = userControl_hand_WindForce_AOD;
                    userPanel_scaleCircle = userControl_scaleCircle_WindForce_AOD;
                    userPanel_scaleLinear = userControl_scaleLinear_WindForce_AOD;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_WindForce_AOD;
                    userControl_icon = userControl_icon_WindForce_AOD;

                    AddActivity_AOD(ScreenIdle, userPanel_pictures, userControl_segments, userPanel_text, userPanel_textGoal, userPanel_hand,
                        userPanel_scaleCircle, userPanel_scaleLinear, userControl_SystemFont_Group,
                        userControl_icon, "WindForce"); 
                }

                #endregion

                #region Altitude

                if (activityName == "Altitude")
                {
                    userPanel_pictures = userControl_pictures_Altitude_AOD;
                    userControl_segments = userControl_segments_Altitude_AOD;
                    userPanel_text = userControl_text_Altitude_AOD;
                    userPanel_hand = userControl_hand_Altitude_AOD;
                    userPanel_scaleCircle = userControl_scaleCircle_Altitude_AOD;
                    userPanel_scaleLinear = userControl_scaleLinear_Altitude_AOD;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_Altitude_AOD;
                    userControl_icon = userControl_icon_Altitude_AOD;

                    AddActivity_AOD(ScreenIdle, userPanel_pictures, userControl_segments, userPanel_text, userPanel_textGoal, userPanel_hand,
                        userPanel_scaleCircle, userPanel_scaleLinear, userControl_SystemFont_Group,
                        userControl_icon, "Altitude"); 
                }

                #endregion

                #region AirPressure

                if (activityName == "AirPressure")
                {
                    userPanel_pictures = userControl_pictures_AirPressure_AOD;
                    userControl_segments = userControl_segments_AirPressure_AOD;
                    userPanel_text = userControl_text_AirPressure_AOD;
                    userPanel_hand = userControl_hand_AirPressure_AOD;
                    userPanel_scaleCircle = userControl_scaleCircle_AirPressure_AOD;
                    userPanel_scaleLinear = userControl_scaleLinear_AirPressure_AOD;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_AirPressure_AOD;
                    userControl_icon = userControl_icon_AirPressure_AOD;

                    AddActivity_AOD(ScreenIdle, userPanel_pictures, userControl_segments, userPanel_text, userPanel_textGoal, userPanel_hand,
                        userPanel_scaleCircle, userPanel_scaleLinear, userControl_SystemFont_Group,
                        userControl_icon, "AirPressure"); 
                }

                #endregion

                #region Stress

                if (activityName == "Stress")
                {
                    userPanel_pictures = userControl_pictures_Stress_AOD;
                    userControl_segments = userControl_segments_Stress_AOD;
                    userPanel_text = userControl_text_Stress_AOD;
                    userPanel_hand = userControl_hand_Stress_AOD;
                    userPanel_scaleCircle = userControl_scaleCircle_Stress_AOD;
                    userPanel_scaleLinear = userControl_scaleLinear_Stress_AOD;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_Stress_AOD;
                    userControl_icon = userControl_icon_Stress_AOD;

                    AddActivity_AOD(ScreenIdle, userPanel_pictures, userControl_segments, userPanel_text, userPanel_textGoal, userPanel_hand,
                        userPanel_scaleCircle, userPanel_scaleLinear, userControl_SystemFont_Group,
                        userControl_icon, "Stress"); 
                }

                #endregion

                #region ActivityGoal

                if (activityName == "ActivityGoal")
                {
                    userPanel_pictures = userControl_pictures_ActivityGoal_AOD;
                    userControl_segments = userControl_segments_ActivityGoal_AOD;
                    userPanel_text = userControl_text_ActivityGoal_AOD;
                    userPanel_textGoal = userControl_text_goal_ActivityGoal_AOD;
                    userPanel_hand = userControl_hand_ActivityGoal_AOD;
                    userPanel_scaleCircle = userControl_scaleCircle_ActivityGoal_AOD;
                    userPanel_scaleLinear = userControl_scaleLinear_ActivityGoal_AOD;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_ActivityGoal_AOD;
                    userControl_icon = userControl_icon_ActivityGoal_AOD;

                    AddActivity_AOD(ScreenIdle, userPanel_pictures, userControl_segments, userPanel_text, userPanel_textGoal, userPanel_hand,
                        userPanel_scaleCircle, userPanel_scaleLinear, userControl_SystemFont_Group,
                        userControl_icon, "ActivityGoal");

                    userPanel_textGoal = null; 
                }
                #endregion

                #region FatBurning

                if (activityName == "FatBurning")
                {
                    userPanel_pictures = userControl_pictures_FatBurning_AOD;
                    userControl_segments = userControl_segments_FatBurning_AOD;
                    userPanel_text = userControl_text_FatBurning_AOD;
                    userPanel_textGoal = userControl_text_goal_FatBurning_AOD;
                    userPanel_hand = userControl_hand_FatBurning_AOD;
                    userPanel_scaleCircle = userControl_scaleCircle_FatBurning_AOD;
                    userPanel_scaleLinear = userControl_scaleLinear_FatBurning_AOD;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_FatBurning_AOD;
                    userControl_icon = userControl_icon_FatBurning_AOD;

                    AddActivity_AOD(ScreenIdle, userPanel_pictures, userControl_segments, userPanel_text, userPanel_textGoal, userPanel_hand,
                        userPanel_scaleCircle, userPanel_scaleLinear, userControl_SystemFont_Group,
                        userControl_icon, "FatBurning");

                    userPanel_textGoal = null; 
                }
                    #endregion

            }
            #endregion


            if (ScreenIdle.Activity != null || ScreenIdle.BackgroundImageIndex != null || 
                ScreenIdle.Date != null || ScreenIdle.DialFace != null) Watch_Face.ScreenIdle = ScreenIdle;

            JSON_write_activityLayer_AOD();
            JSON_write_dateLayer_AOD();
        }

        private void JSON_write_activityLayer_AOD()
        {
            if (Watch_Face.ScreenIdle != null && Watch_Face.ScreenIdle.Activity != null)
            {
                int count = 0;
                Dictionary<int, string> activity_layer = new Dictionary<int, string>();
                foreach (Activity activity in Watch_Face.ScreenIdle.Activity)
                {
                    switch (activity.Type)
                    {
                        case "Battery":
                            if (!activity_layer.ContainsValue("Battery") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "Battery");
                                count++;
                            }
                            break;
                        case "Steps":
                            if (!activity_layer.ContainsValue("Steps") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "Steps");
                                count++;
                            }
                            break;
                        case "Calories":
                            if (!activity_layer.ContainsValue("Calories") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "Calories");
                                count++;
                            }
                            break;
                        case "HeartRate":
                            if (!activity_layer.ContainsValue("HeartRate") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "HeartRate");
                                count++;
                            }
                            break;
                        case "PAI":
                            if (!activity_layer.ContainsValue("PAI") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "PAI");
                                count++;
                            }
                            break;
                        case "Distance":
                            if (!activity_layer.ContainsValue("Distance") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "Distance");
                                count++;
                            }
                            break;
                        case "StandUp":
                            if (!activity_layer.ContainsValue("StandUp") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "StandUp");
                                count++;
                            }
                            break;
                        case "Weather":
                            if (!activity_layer.ContainsValue("Weather") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "Weather");
                                count++;
                            }
                            break;
                        case "UVindex":
                            if (!activity_layer.ContainsValue("UVindex") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "UVindex");
                                count++;
                            }
                            break;
                        case "AirQuality":
                            if (!activity_layer.ContainsValue("AirQuality") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "AirQuality");
                                count++;
                            }
                            break;
                        case "Humidity":
                            if (!activity_layer.ContainsValue("Humidity") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "Humidity");
                                count++;
                            }
                            break;
                        case "Sunrise":
                            if (!activity_layer.ContainsValue("Sunrise") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "Sunrise");
                                count++;
                            }
                            break;
                        case "WindForce":
                            if (!activity_layer.ContainsValue("WindForce") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "WindForce");
                                count++;
                            }
                            break;
                        case "Altitude":
                            if (!activity_layer.ContainsValue("Altitude") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "Altitude");
                                count++;
                            }
                            break;
                        case "AirPressure":
                            if (!activity_layer.ContainsValue("AirPressure") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "AirPressure");
                                count++;
                            }
                            break;
                        case "Stress":
                            if (!activity_layer.ContainsValue("Stress") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "Stress");
                                count++;
                            }
                            break;
                        case "ActivityGoal":
                            if (!activity_layer.ContainsValue("ActivityGoal") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "ActivityGoal");
                                count++;
                            }
                            break;
                        case "FatBurning":
                            if (!activity_layer.ContainsValue("FatBurning") && !activity_layer.ContainsKey(count))
                            {
                                activity_layer.Add(count, "FatBurning");
                                count++;
                            }
                            break;
                    }
                }

                for (int i = 0; i < dataGridView_AODL_Activity.RowCount; i++)
                {
                    string name = dataGridView_AODL_Activity.Rows[i].Cells[0].Value.ToString();
                    if (activity_layer.ContainsValue(name)) dataGridView_AODL_Activity.Rows[i].Visible = true;
                    else dataGridView_AODL_Activity.Rows[i].Visible = false;

                }
                if (dataGridView_AODL_Activity.SelectedCells.Count > 0)
                {
                    int RowIndexSelect = dataGridView_AODL_Activity.SelectedCells[0].RowIndex;
                    dataGridView_AODL_Activity.ClearSelection();
                    if (dataGridView_AODL_Activity.Rows[RowIndexSelect].Visible)
                        dataGridView_AODL_Activity.Rows[RowIndexSelect].Selected = true;
                }
            }
        }

        private void JSON_write_dateLayer_AOD()
        {
            if (Watch_Face.ScreenIdle != null && Watch_Face.ScreenIdle.Date != null && Watch_Face.ScreenIdle.Date.DateDigits != null)
            {
                int count = 0;
                Dictionary<int, string> date_layer = new Dictionary<int, string>();
                foreach (DigitalDateDigit date in Watch_Face.ScreenIdle.Date.DateDigits)
                {
                    switch (date.DateType)
                    {
                        case "Day":
                            if (!date_layer.ContainsValue("Day") && !date_layer.ContainsKey(count))
                            {
                                date_layer.Add(count, "Day");
                                count++;
                            }
                            break;
                        case "Month":
                            if (!date_layer.ContainsValue("Month") && !date_layer.ContainsKey(count))
                            {
                                date_layer.Add(count, "Month");
                                count++;
                            }
                            break;
                        case "Year":
                            if (!date_layer.ContainsValue("Year") && !date_layer.ContainsKey(count))
                            {
                                date_layer.Add(count, "Year");
                                count++;
                            }
                            break;
                    }
                }

                for (int i = 0; i < dataGridView_AODL_Date.RowCount; i++)
                {
                    string name = dataGridView_AODL_Date.Rows[i].Cells[0].Value.ToString();
                    if (date_layer.ContainsValue(name)) dataGridView_AODL_Date.Rows[i].Visible = true;
                    else dataGridView_AODL_Date.Rows[i].Visible = false;

                }
                if (dataGridView_AODL_Date.SelectedCells.Count > 0)
                {
                    int RowIndexSelect = dataGridView_AODL_Date.SelectedCells[0].RowIndex;
                    dataGridView_AODL_Date.ClearSelection();
                    if (dataGridView_AODL_Date.Rows[RowIndexSelect].Visible)
                        dataGridView_AODL_Date.Rows[RowIndexSelect].Selected = true;
                }
            }
        }

        private void AddActivity_AOD(ScreenIdle ScreenIdle, UserControl_pictures panel_pictures, UserControl_segments userControl_segments,
            UserControl_text panel_text, UserControl_text userPanel_textGoal, UserControl_hand panel_hand,
            UserControl_scaleCircle panel_scaleCircle, UserControl_scaleLinear panel_scaleLinear,
            UserControl_SystemFont_Group userControl_SystemFont_Group,
            UserControl_icon userControl_icon, string type)
        {
            UserControl_SystemFont userControl_SystemFont = userControl_SystemFont_Group.userControl_SystemFont;
            UserControl_FontRotate userControl_FontRotate = userControl_SystemFont_Group.userControl_FontRotate;
            UserControl_SystemFont_weather userControl_SystemFontGoal = userControl_SystemFont_Group.userControl_SystemFont_goal;
            UserControl_FontRotate_weather userControl_FontRotateGoal = userControl_SystemFont_Group.userControl_FontRotate_goal;


            Activity activity = null;
            Activity activityMin = null;

            // данные картинками
            //checkBox_Use = (CheckBox)panel_pictures.checkBox_pictures_Use;
            if (panel_pictures != null && panel_pictures.checkBox_pictures_Use.Checked)
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
                    //activity.ImageProgress.DisplayType = "Single";
                    Coordinates coordinates = new Coordinates();
                    coordinates.X = (long)numericUpDownX.Value;
                    coordinates.Y = (long)numericUpDownY.Value;
                    activity.ImageProgress.Coordinates.Add(coordinates);
                }
            }

            // данные сегментами
            if (userControl_segments != null && userControl_segments.checkBox_pictures_Use.Checked)
            {
                int image = userControl_segments.comboBoxGetImage();
                if (image >= 0)
                {
                    List<Coordinates> coordinates = userControl_segments.GetCoordinates();
                    if (coordinates != null)
                    {
                        if (activityMin == null) activityMin = new Activity();
                        activityMin.ImageProgress = new ImageProgress();
                        //activityMin.ImageProgress.DisplayType = "Single"; //"Single" = 0, "Continuous" = 1
                        activityMin.ImageProgress.DisplayType = userControl_segments.radioButtonGetDisplayType();
                        activityMin.ImageProgress.ImageSet = new ImageSetGTR2();
                        activityMin.ImageProgress.Coordinates = coordinates;
                        activityMin.ImageProgress.ImageSet.ImageIndex = image;
                        activityMin.ImageProgress.ImageSet.ImagesCount = coordinates.Count;
                    }
                }
            }

            // данные надписью
            //checkBox_Use = (CheckBox)panel_text.checkBox_Use;
            if (panel_text != null && panel_text.checkBox_Use.Checked)
            {
                //ComboBox comboBox_image = (ComboBox)panel_text.Controls[1];
                int image = panel_text.comboBoxGetImage();
                if (image >= 0)
                {
                    //ComboBox comboBox_unit = (ComboBox)panel_text.Controls[2];
                    //ComboBox comboBox_separator = (ComboBox)panel_text.Controls[3];
                    int unit = panel_text.comboBoxGetIcon();
                    int separator = panel_text.comboBoxGetUnit();
                    int separatorMile = panel_text.comboBoxGetUnitMile();
                    NumericUpDown numericUpDownX = panel_text.numericUpDown_imageX;
                    NumericUpDown numericUpDownY = panel_text.numericUpDown_imageY;
                    NumericUpDown numericUpDown_unitX = panel_text.numericUpDown_iconX;
                    NumericUpDown numericUpDown_unitY = panel_text.numericUpDown_iconY;
                    string Alignment = panel_text.comboBoxGetAlignment();
                    NumericUpDown numericUpDown_spacing = panel_text.numericUpDown_spacing;
                    bool add_zero = panel_text.checkBox_addZero.Checked;
                    bool follow = panel_text.checkBox_follow.Checked;
                    int imageError = panel_text.comboBoxGetImageError();

                    if (activity == null) activity = new Activity();
                    if (activity.Digits == null) activity.Digits = new List<DigitalCommonDigit>();
                    DigitalCommonDigit digitalCommonDigit = new DigitalCommonDigit();
                    if (!follow) digitalCommonDigit.CombingMode = "Single";
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

                        if (separatorMile >= 0)
                        {
                            digitalCommonDigit.Digit.Image.MultilangImageUnitMile = new List<MultilangImage>();
                            MultilangImage multilangImageMile = new MultilangImage();
                            multilangImageMile.LangCode = "All";
                            multilangImageMile.ImageSet = new ImageSetGTR2();
                            multilangImageMile.ImageSet.ImagesCount = 1;
                            multilangImageMile.ImageSet.ImageIndex = separatorMile;
                            digitalCommonDigit.Digit.Image.MultilangImageUnitMile.Add(multilangImageMile);
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

            // цель надписью
            if (userPanel_textGoal != null && userPanel_textGoal.checkBox_Use.Checked)
            {
                //ComboBox comboBox_image = (ComboBox)userPanel_textGoal.Controls[1];
                int image = userPanel_textGoal.comboBoxGetImage();
                if (image >= 0)
                {
                    //ComboBox comboBox_unit = (ComboBox)userPanel_textGoal.Controls[2];
                    //ComboBox comboBox_separator = (ComboBox)userPanel_textGoal.Controls[3];
                    int unit = userPanel_textGoal.comboBoxGetIcon();
                    int separator = userPanel_textGoal.comboBoxGetUnit();
                    NumericUpDown numericUpDownX = userPanel_textGoal.numericUpDown_imageX;
                    NumericUpDown numericUpDownY = userPanel_textGoal.numericUpDown_imageY;
                    NumericUpDown numericUpDown_unitX = userPanel_textGoal.numericUpDown_iconX;
                    NumericUpDown numericUpDown_unitY = userPanel_textGoal.numericUpDown_iconY;
                    string Alignment = userPanel_textGoal.comboBoxGetAlignment();
                    NumericUpDown numericUpDown_spacing = userPanel_textGoal.numericUpDown_spacing;
                    bool add_zero = userPanel_textGoal.checkBox_addZero.Checked;
                    bool follow = userPanel_textGoal.checkBox_follow.Checked;
                    int imageError = userPanel_textGoal.comboBoxGetImageError();

                    if (activity == null) activity = new Activity();
                    if (activity.Digits == null) activity.Digits = new List<DigitalCommonDigit>();
                    DigitalCommonDigit digitalCommonDigit = new DigitalCommonDigit();
                    digitalCommonDigit.Type = "Min";
                    if (!follow) digitalCommonDigit.CombingMode = "Single";
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
                        int DecimalPoint = userPanel_textGoal.comboBoxGetImageDecimalPointOrMinus();
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

            // данные системным шрифтом
            if (userControl_SystemFont != null && userControl_SystemFont.checkBox_Use.Checked)
            {
                NumericUpDown numericUpDownX = userControl_SystemFont.numericUpDown_SystemFontX;
                NumericUpDown numericUpDownY = userControl_SystemFont.numericUpDown_SystemFontY;
                NumericUpDown numericUpDown_size = userControl_SystemFont.numericUpDown_SystemFont_size;
                NumericUpDown numericUpDown_angle = userControl_SystemFont.numericUpDown_SystemFont_angle;
                NumericUpDown numericUpDown_spacing = userControl_SystemFont.numericUpDown_SystemFont_spacing;
                bool follow = userControl_SystemFont.checkBox_follow.Checked;
                bool add_zero = userControl_SystemFont.checkBox_addZero.Checked;
                bool separator = userControl_SystemFont.checkBox_separator.Checked;

                if (activity == null) activity = new Activity();
                if (activity.Digits == null) activity.Digits = new List<DigitalCommonDigit>();
                DigitalCommonDigit digitalCommonDigit = new DigitalCommonDigit();
                if (!follow) digitalCommonDigit.CombingMode = "Single";
                if (separator)
                {
                    digitalCommonDigit.Separator = new ImageCoord();
                    digitalCommonDigit.Separator.Coordinates = new Coordinates();
                    digitalCommonDigit.Separator.Coordinates.X = -1;
                    digitalCommonDigit.Separator.Coordinates.Y = -1;
                }


                if (digitalCommonDigit.Digit == null) digitalCommonDigit.Digit = new Text();
                if (digitalCommonDigit.Digit.SystemFont == null)
                    digitalCommonDigit.Digit.SystemFont = new SystemFont();
                if (digitalCommonDigit.Digit.SystemFont.Coordinates == null)
                    digitalCommonDigit.Digit.SystemFont.Coordinates = new Coordinates();

                digitalCommonDigit.Digit.PaddingZero = add_zero;
                digitalCommonDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;

                digitalCommonDigit.Digit.SystemFont.Coordinates.X = (long)numericUpDownX.Value;
                digitalCommonDigit.Digit.SystemFont.Coordinates.Y = (long)numericUpDownY.Value;
                digitalCommonDigit.Digit.SystemFont.Size = (long)numericUpDown_size.Value;
                digitalCommonDigit.Digit.SystemFont.Angle = (long)numericUpDown_angle.Value;

                digitalCommonDigit.Digit.SystemFont.ShowUnitCheck = userControl_SystemFont.checkBoxGetUnit();
                digitalCommonDigit.Digit.SystemFont.Color = userControl_SystemFont.comboBoxGetColorString();

                activity.Digits.Add(digitalCommonDigit);
            }

            // цель системным шрифтом
            if (userControl_SystemFontGoal != null && userControl_SystemFontGoal.checkBox_Use.Checked)
            {
                NumericUpDown numericUpDownX = userControl_SystemFontGoal.numericUpDown_SystemFontX;
                NumericUpDown numericUpDownY = userControl_SystemFontGoal.numericUpDown_SystemFontY;
                NumericUpDown numericUpDown_size = userControl_SystemFontGoal.numericUpDown_SystemFont_size;
                NumericUpDown numericUpDown_angle = userControl_SystemFontGoal.numericUpDown_SystemFont_angle;
                NumericUpDown numericUpDown_spacing = userControl_SystemFontGoal.numericUpDown_SystemFont_spacing;
                bool follow = userControl_SystemFontGoal.checkBox_follow.Checked;
                bool add_zero = userControl_SystemFontGoal.checkBox_addZero.Checked;
                bool separator = userControl_SystemFontGoal.checkBox_separator.Checked;

                DigitalCommonDigit digitalCommonDigit = new DigitalCommonDigit();
                digitalCommonDigit.Type = "Min";
                if (!follow) digitalCommonDigit.CombingMode = "Single";
                if (separator)
                {
                    digitalCommonDigit.Separator = new ImageCoord();
                    digitalCommonDigit.Separator.Coordinates = new Coordinates();
                    digitalCommonDigit.Separator.Coordinates.X = -1;
                    digitalCommonDigit.Separator.Coordinates.Y = -1;
                }

                if (digitalCommonDigit.Digit == null) digitalCommonDigit.Digit = new Text();
                if (digitalCommonDigit.Digit.SystemFont == null)
                    digitalCommonDigit.Digit.SystemFont = new SystemFont();
                if (digitalCommonDigit.Digit.SystemFont.Coordinates == null)
                    digitalCommonDigit.Digit.SystemFont.Coordinates = new Coordinates();

                digitalCommonDigit.Digit.PaddingZero = add_zero;
                digitalCommonDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;

                digitalCommonDigit.Digit.SystemFont.Coordinates.X = (long)numericUpDownX.Value;
                digitalCommonDigit.Digit.SystemFont.Coordinates.Y = (long)numericUpDownY.Value;
                digitalCommonDigit.Digit.SystemFont.Size = (long)numericUpDown_size.Value;
                digitalCommonDigit.Digit.SystemFont.Angle = (long)numericUpDown_angle.Value;

                digitalCommonDigit.Digit.SystemFont.ShowUnitCheck = userControl_SystemFontGoal.checkBoxGetUnit();
                digitalCommonDigit.Digit.SystemFont.Color = userControl_SystemFontGoal.comboBoxGetColorString();

                if (follow)
                {
                    if (activity == null) activity = new Activity();
                    if (activity.Digits == null) activity.Digits = new List<DigitalCommonDigit>();
                    activity.Digits.Add(digitalCommonDigit);
                }
                else
                {
                    if (activityMin == null) activityMin = new Activity();
                    if (activityMin.Digits == null) activityMin.Digits = new List<DigitalCommonDigit>();
                    activityMin.Digits.Add(digitalCommonDigit);
                }
            }

            // данные системным шрифтом по окружности
            if (userControl_FontRotate != null && userControl_FontRotate.checkBox_Use.Checked)
            {
                NumericUpDown numericUpDownX = userControl_FontRotate.numericUpDown_FontRotateX;
                NumericUpDown numericUpDownY = userControl_FontRotate.numericUpDown_FontRotateY;
                NumericUpDown numericUpDown_size = userControl_FontRotate.numericUpDown_FontRotate_size;
                NumericUpDown numericUpDown_angle = userControl_FontRotate.numericUpDown_FontRotate_angle;
                NumericUpDown numericUpDown_radius = userControl_FontRotate.numericUpDown_FontRotate_radius;
                NumericUpDown numericUpDown_spacing = userControl_FontRotate.numericUpDown_FontRotate_spacing;
                bool follow = userControl_FontRotate.checkBox_follow.Checked;
                bool add_zero = userControl_FontRotate.checkBox_addZero.Checked;
                bool separator = userControl_FontRotate.checkBox_separator.Checked;

                if (activity == null) activity = new Activity();
                if (activity.Digits == null) activity.Digits = new List<DigitalCommonDigit>();
                DigitalCommonDigit digitalCommonDigit = new DigitalCommonDigit();
                if (!follow) digitalCommonDigit.CombingMode = "Single";
                if (separator)
                {
                    digitalCommonDigit.Separator = new ImageCoord();
                    digitalCommonDigit.Separator.Coordinates = new Coordinates();
                    digitalCommonDigit.Separator.Coordinates.X = -1;
                    digitalCommonDigit.Separator.Coordinates.Y = -1;
                }

                if (digitalCommonDigit.Digit == null) digitalCommonDigit.Digit = new Text();
                if (digitalCommonDigit.Digit.SystemFont == null)
                    digitalCommonDigit.Digit.SystemFont = new SystemFont();
                if (digitalCommonDigit.Digit.SystemFont.FontRotate == null)
                    digitalCommonDigit.Digit.SystemFont.FontRotate = new FontRotate();

                digitalCommonDigit.Digit.PaddingZero = add_zero;
                digitalCommonDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;

                digitalCommonDigit.Digit.SystemFont.FontRotate.X = (long)numericUpDownX.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.Y = (long)numericUpDownY.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.Radius = (long)numericUpDown_radius.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.RotateDirection =
                    userControl_FontRotate.radioButtonGetRotateDirection();
                digitalCommonDigit.Digit.SystemFont.Size = (long)numericUpDown_size.Value;
                digitalCommonDigit.Digit.SystemFont.Angle = (long)numericUpDown_angle.Value;

                digitalCommonDigit.Digit.SystemFont.ShowUnitCheck = userControl_FontRotate.checkBoxGetUnit();
                digitalCommonDigit.Digit.SystemFont.Color = userControl_FontRotate.comboBoxGetColorString();

                activity.Digits.Add(digitalCommonDigit);
            }

            // цель системным шрифтом по окружности
            if (userControl_FontRotateGoal != null && userControl_FontRotateGoal.checkBox_Use.Checked)
            {
                NumericUpDown numericUpDownX = userControl_FontRotateGoal.numericUpDown_FontRotateX;
                NumericUpDown numericUpDownY = userControl_FontRotateGoal.numericUpDown_FontRotateY;
                NumericUpDown numericUpDown_size = userControl_FontRotateGoal.numericUpDown_FontRotate_size;
                NumericUpDown numericUpDown_angle = userControl_FontRotateGoal.numericUpDown_FontRotate_angle;
                NumericUpDown numericUpDown_radius = userControl_FontRotateGoal.numericUpDown_FontRotate_radius;
                NumericUpDown numericUpDown_spacing = userControl_FontRotateGoal.numericUpDown_FontRotate_spacing;
                bool follow = userControl_FontRotateGoal.checkBox_follow.Checked;
                bool add_zero = userControl_FontRotateGoal.checkBox_addZero.Checked;
                bool separator = userControl_FontRotateGoal.checkBox_separator.Checked;

                DigitalCommonDigit digitalCommonDigit = new DigitalCommonDigit();
                digitalCommonDigit.Type = "Min";
                if (!follow) digitalCommonDigit.CombingMode = "Single";
                if (separator)
                {
                    digitalCommonDigit.Separator = new ImageCoord();
                    digitalCommonDigit.Separator.Coordinates = new Coordinates();
                    digitalCommonDigit.Separator.Coordinates.X = -1;
                    digitalCommonDigit.Separator.Coordinates.Y = -1;
                }

                if (digitalCommonDigit.Digit == null) digitalCommonDigit.Digit = new Text();
                if (digitalCommonDigit.Digit.SystemFont == null)
                    digitalCommonDigit.Digit.SystemFont = new SystemFont();
                if (digitalCommonDigit.Digit.SystemFont.FontRotate == null)
                    digitalCommonDigit.Digit.SystemFont.FontRotate = new FontRotate();

                digitalCommonDigit.Digit.PaddingZero = add_zero;
                digitalCommonDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;

                digitalCommonDigit.Digit.SystemFont.FontRotate.X = (long)numericUpDownX.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.Y = (long)numericUpDownY.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.Radius = (long)numericUpDown_radius.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.RotateDirection =
                    userControl_FontRotateGoal.radioButtonGetRotateDirection();
                digitalCommonDigit.Digit.SystemFont.Size = (long)numericUpDown_size.Value;
                digitalCommonDigit.Digit.SystemFont.Angle = (long)numericUpDown_angle.Value;

                digitalCommonDigit.Digit.SystemFont.ShowUnitCheck = userControl_FontRotateGoal.checkBoxGetUnit();
                digitalCommonDigit.Digit.SystemFont.Color = userControl_FontRotateGoal.comboBoxGetColorString();

                if (follow)
                {
                    if (activity == null) activity = new Activity();
                    if (activity.Digits == null) activity.Digits = new List<DigitalCommonDigit>();
                    activity.Digits.Add(digitalCommonDigit);
                }
                else
                {
                    if (activityMin == null) activityMin = new Activity();
                    if (activityMin.Digits == null) activityMin.Digits = new List<DigitalCommonDigit>();
                    activityMin.Digits.Add(digitalCommonDigit);
                }
            }

            // данные стрелкой
            if (panel_hand != null && panel_hand.checkBox_hand_Use.Checked)
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
            bool scaleCircle = false;
            if (panel_scaleCircle != null && panel_scaleCircle.checkBox_scaleCircle_Use.Checked)
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
                    activity.ProgressBar.Flatness = panel_scaleCircle.comboBoxGetFlatness();
                    scaleCircle = true;
                }
            }

            // данные линейной шкалой
            if (panel_scaleLinear != null && panel_scaleLinear.checkBox_scaleLinear_Use.Checked)
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
                    if (scaleCircle)
                    {
                        if (activityMin == null) activityMin = new Activity();
                        if (activityMin.ProgressBar == null) activityMin.ProgressBar = new ProgressBar();
                        activityMin.ProgressBar.LinearSettings = new LinearSettings();
                        if (radioButton_image.Checked && panel_scaleLinear.comboBoxGetSelectedIndexImage() >= 0)
                        {
                            activityMin.ProgressBar.ForegroundImageIndex = panel_scaleLinear.comboBoxGetImage();
                        }
                        else
                        {
                            activityMin.ProgressBar.Color = panel_scaleLinear.comboBoxGetColorString();
                        }
                        if (panel_scaleLinear.comboBoxGetSelectedIndexImagePointer() >= 0)
                            activityMin.ProgressBar.PointerImageIndex = panel_scaleLinear.comboBoxGetImagePointer();
                        if (panel_scaleLinear.comboBoxGetSelectedIndexImageBackground() >= 0)
                            activityMin.ProgressBar.BackgroundImageIndex = panel_scaleLinear.comboBoxGetImageBackground();

                        activityMin.ProgressBar.LinearSettings.StartX = (long)numericUpDownX.Value;
                        activityMin.ProgressBar.LinearSettings.StartY = (long)numericUpDownY.Value;
                        long endX = (long)(numericUpDownX.Value + numericUpDown_length.Value);
                        activityMin.ProgressBar.LinearSettings.EndX = endX;
                        activityMin.ProgressBar.LinearSettings.EndY = (long)numericUpDownY.Value;
                        activityMin.ProgressBar.Width = (long)numericUpDown_width.Value;
                        activityMin.ProgressBar.Flatness = panel_scaleLinear.comboBoxGetFlatness();
                    }
                    else
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
                        activity.ProgressBar.Flatness = panel_scaleLinear.comboBoxGetFlatness();
                    }
                }

            }

            // данные иконки
            if (userControl_icon != null && userControl_icon.checkBox_icon_Use.Checked)
            {
                int image = userControl_icon.comboBoxGetImage();
                if (image >= 0)
                {
                    NumericUpDown numericUpDownX = (NumericUpDown)userControl_icon.numericUpDown_iconX;
                    NumericUpDown numericUpDownY = (NumericUpDown)userControl_icon.numericUpDown_iconY;
                    int image2 = userControl_icon.comboBoxGetImage2();

                    if (activity == null) activity = new Activity();
                    activity.Icon = new ImageCoord();
                    activity.Icon.Coordinates = new Coordinates();
                    activity.Icon.ImageIndex = image;
                    if (image2 >= 0) activity.Icon.ImageIndex2 = image2;
                    activity.Icon.Coordinates.X = (long)numericUpDownX.Value;
                    activity.Icon.Coordinates.Y = (long)numericUpDownY.Value;
                }
            }

            if (activity != null)
            {
                activity.Type = type;
                if (ScreenIdle.Activity == null) ScreenIdle.Activity = new List<Activity>();
                ScreenIdle.Activity.Add(activity);
            }

            if (activityMin != null)
            {
                activityMin.Type = type;
                if (ScreenIdle.Activity == null) ScreenIdle.Activity = new List<Activity>();
                ScreenIdle.Activity.Add(activityMin);
            }

        }

        private void AddActivityWeather_AOD(ScreenIdle ScreenIdle, UserControl_pictures panel_pictures,
            UserControl_text_weather panel_text, UserControl_text_weather panel_text_min,
            UserControl_text_weather panel_text_max, UserControl_hand panel_hand,
            UserControl_scaleCircle panel_scaleCircle, UserControl_scaleLinear panel_scaleLinear,
            UserControl_SystemFont_GroupWeather userControl_SystemFont_Group, UserControl_icon userControl_icon)
        {
            UserControl_SystemFont userControl_SystemFont_Current = userControl_SystemFont_Group.userControl_SystemFont_weather_Current;
            UserControl_SystemFont userControl_SystemFont_Min = userControl_SystemFont_Group.userControl_SystemFont_weather_Min;
            UserControl_SystemFont userControl_SystemFont_Max = userControl_SystemFont_Group.userControl_SystemFont_weather_Max;

            UserControl_FontRotate userControl_FontRotate_Current = userControl_SystemFont_Group.userControl_FontRotate_weather_Current;
            UserControl_FontRotate userControl_FontRotate_Min = userControl_SystemFont_Group.userControl_FontRotate_weather_Min;
            UserControl_FontRotate userControl_FontRotate_Max = userControl_SystemFont_Group.userControl_FontRotate_weather_Max;


            Activity activity = null;
            Activity activityMin = null;
            Activity activityMax = null;
            Activity activityPictures = null;

            // данные картинками
            if (panel_pictures.checkBox_pictures_Use.Checked)
            {
                if (panel_pictures.comboBoxGetSelectedIndexImage() >= 0)
                {
                    NumericUpDown numericUpDownX = panel_pictures.numericUpDown_picturesX;
                    NumericUpDown numericUpDownY = panel_pictures.numericUpDown_picturesY;

                    if (activityPictures == null) activityPictures = new Activity();
                    activityPictures.ImageProgress = new ImageProgress();
                    activityPictures.ImageProgress.ImageSet = new ImageSetGTR2();
                    activityPictures.ImageProgress.Coordinates = new List<Coordinates>();
                    activityPictures.ImageProgress.ImageSet.ImageIndex = panel_pictures.comboBoxGetImage();
                    activityPictures.ImageProgress.ImageSet.ImagesCount = 29;
                    //activityPictures.ImageProgress.ImageSet.ImagesCount = (long)numericUpDown_count.Value;
                    Coordinates coordinates = new Coordinates();
                    coordinates.X = (long)numericUpDownX.Value;
                    coordinates.Y = (long)numericUpDownY.Value;
                    activityPictures.ImageProgress.Coordinates.Add(coordinates);
                }
            }

            // данные надписью current temperature
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

            bool follow_min = true;
            bool follow_max = true;

            // данные системным шрифтом current temperature
            if (userControl_SystemFont_Current != null && userControl_SystemFont_Current.checkBox_Use.Checked)
            {
                NumericUpDown numericUpDownX = userControl_SystemFont_Current.numericUpDown_SystemFontX;
                NumericUpDown numericUpDownY = userControl_SystemFont_Current.numericUpDown_SystemFontY;
                NumericUpDown numericUpDown_size = userControl_SystemFont_Current.numericUpDown_SystemFont_size;
                NumericUpDown numericUpDown_angle = userControl_SystemFont_Current.numericUpDown_SystemFont_angle;
                NumericUpDown numericUpDown_spacing = userControl_SystemFont_Current.numericUpDown_SystemFont_spacing;
                bool follow = userControl_SystemFont_Current.checkBox_follow.Checked;
                bool add_zero = userControl_SystemFont_Current.checkBox_addZero.Checked;
                bool separator = userControl_SystemFont_Current.checkBox_separator.Checked;

                if (activity == null) activity = new Activity();
                if (activity.Digits == null) activity.Digits = new List<DigitalCommonDigit>();
                DigitalCommonDigit digitalCommonDigit = new DigitalCommonDigit();
                if (!follow) digitalCommonDigit.CombingMode = "Single";
                if (separator)
                {
                    digitalCommonDigit.Separator = new ImageCoord();
                    digitalCommonDigit.Separator.Coordinates = new Coordinates();
                    digitalCommonDigit.Separator.Coordinates.X = -1;
                    digitalCommonDigit.Separator.Coordinates.Y = -1;
                }


                if (digitalCommonDigit.Digit == null) digitalCommonDigit.Digit = new Text();
                if (digitalCommonDigit.Digit.SystemFont == null)
                    digitalCommonDigit.Digit.SystemFont = new SystemFont();
                if (digitalCommonDigit.Digit.SystemFont.Coordinates == null)
                    digitalCommonDigit.Digit.SystemFont.Coordinates = new Coordinates();

                digitalCommonDigit.Digit.PaddingZero = add_zero;
                digitalCommonDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;

                digitalCommonDigit.Digit.SystemFont.Coordinates.X = (long)numericUpDownX.Value;
                digitalCommonDigit.Digit.SystemFont.Coordinates.Y = (long)numericUpDownY.Value;
                digitalCommonDigit.Digit.SystemFont.Size = (long)numericUpDown_size.Value;
                digitalCommonDigit.Digit.SystemFont.Angle = (long)numericUpDown_angle.Value;

                digitalCommonDigit.Digit.SystemFont.ShowUnitCheck = userControl_SystemFont_Current.checkBoxGetUnit();
                digitalCommonDigit.Digit.SystemFont.Color = userControl_SystemFont_Current.comboBoxGetColorString();

                activity.Digits.Add(digitalCommonDigit);
            }

            // данные системным шрифтом min temperature
            if (userControl_SystemFont_Min != null && userControl_SystemFont_Min.checkBox_Use.Checked)
            {
                NumericUpDown numericUpDownX = userControl_SystemFont_Min.numericUpDown_SystemFontX;
                NumericUpDown numericUpDownY = userControl_SystemFont_Min.numericUpDown_SystemFontY;
                NumericUpDown numericUpDown_size = userControl_SystemFont_Min.numericUpDown_SystemFont_size;
                NumericUpDown numericUpDown_angle = userControl_SystemFont_Min.numericUpDown_SystemFont_angle;
                NumericUpDown numericUpDown_spacing = userControl_SystemFont_Min.numericUpDown_SystemFont_spacing;
                bool follow = userControl_SystemFont_Min.checkBox_follow.Checked;
                follow_min = follow;
                bool add_zero = userControl_SystemFont_Min.checkBox_addZero.Checked;
                bool separator = userControl_SystemFont_Min.checkBox_separator.Checked;

                DigitalCommonDigit digitalCommonDigit = new DigitalCommonDigit();
                digitalCommonDigit.Type = "Min";
                if (!follow) digitalCommonDigit.CombingMode = "Single";
                if (separator)
                {
                    digitalCommonDigit.Separator = new ImageCoord();
                    digitalCommonDigit.Separator.Coordinates = new Coordinates();
                    digitalCommonDigit.Separator.Coordinates.X = -1;
                    digitalCommonDigit.Separator.Coordinates.Y = -1;
                }


                if (digitalCommonDigit.Digit == null) digitalCommonDigit.Digit = new Text();
                if (digitalCommonDigit.Digit.SystemFont == null)
                    digitalCommonDigit.Digit.SystemFont = new SystemFont();
                if (digitalCommonDigit.Digit.SystemFont.Coordinates == null)
                    digitalCommonDigit.Digit.SystemFont.Coordinates = new Coordinates();

                digitalCommonDigit.Digit.PaddingZero = add_zero;
                digitalCommonDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;

                digitalCommonDigit.Digit.SystemFont.Coordinates.X = (long)numericUpDownX.Value;
                digitalCommonDigit.Digit.SystemFont.Coordinates.Y = (long)numericUpDownY.Value;
                digitalCommonDigit.Digit.SystemFont.Size = (long)numericUpDown_size.Value;
                digitalCommonDigit.Digit.SystemFont.Angle = (long)numericUpDown_angle.Value;

                digitalCommonDigit.Digit.SystemFont.ShowUnitCheck = userControl_SystemFont_Min.checkBoxGetUnit();
                digitalCommonDigit.Digit.SystemFont.Color = userControl_SystemFont_Min.comboBoxGetColorString();

                if (follow_min)
                {
                    if (activity == null) activity = new Activity();
                    if (activity.Digits == null) activity.Digits = new List<DigitalCommonDigit>();
                    activity.Digits.Add(digitalCommonDigit);
                }
                else
                {
                    if (activityMin == null) activityMin = new Activity();
                    if (activityMin.Digits == null) activityMin.Digits = new List<DigitalCommonDigit>();
                    activityMin.Digits.Add(digitalCommonDigit);
                }

            }

            // данные системным шрифтом max temperature
            if (userControl_SystemFont_Max != null && userControl_SystemFont_Max.checkBox_Use.Checked)
            {
                NumericUpDown numericUpDownX = userControl_SystemFont_Max.numericUpDown_SystemFontX;
                NumericUpDown numericUpDownY = userControl_SystemFont_Max.numericUpDown_SystemFontY;
                NumericUpDown numericUpDown_size = userControl_SystemFont_Max.numericUpDown_SystemFont_size;
                NumericUpDown numericUpDown_angle = userControl_SystemFont_Max.numericUpDown_SystemFont_angle;
                NumericUpDown numericUpDown_spacing = userControl_SystemFont_Max.numericUpDown_SystemFont_spacing;
                bool follow = userControl_SystemFont_Max.checkBox_follow.Checked;
                follow_max = follow;
                bool add_zero = userControl_SystemFont_Max.checkBox_addZero.Checked;
                bool separator = userControl_SystemFont_Max.checkBox_separator.Checked;

                DigitalCommonDigit digitalCommonDigit = new DigitalCommonDigit();
                digitalCommonDigit.Type = "Max";
                if (!follow) digitalCommonDigit.CombingMode = "Single";
                if (separator)
                {
                    digitalCommonDigit.Separator = new ImageCoord();
                    digitalCommonDigit.Separator.Coordinates = new Coordinates();
                    digitalCommonDigit.Separator.Coordinates.X = -1;
                    digitalCommonDigit.Separator.Coordinates.Y = -1;
                }


                if (digitalCommonDigit.Digit == null) digitalCommonDigit.Digit = new Text();
                if (digitalCommonDigit.Digit.SystemFont == null)
                    digitalCommonDigit.Digit.SystemFont = new SystemFont();
                if (digitalCommonDigit.Digit.SystemFont.Coordinates == null)
                    digitalCommonDigit.Digit.SystemFont.Coordinates = new Coordinates();

                digitalCommonDigit.Digit.PaddingZero = add_zero;
                digitalCommonDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;

                digitalCommonDigit.Digit.SystemFont.Coordinates.X = (long)numericUpDownX.Value;
                digitalCommonDigit.Digit.SystemFont.Coordinates.Y = (long)numericUpDownY.Value;
                digitalCommonDigit.Digit.SystemFont.Size = (long)numericUpDown_size.Value;
                digitalCommonDigit.Digit.SystemFont.Angle = (long)numericUpDown_angle.Value;

                digitalCommonDigit.Digit.SystemFont.ShowUnitCheck = userControl_SystemFont_Max.checkBoxGetUnit();
                digitalCommonDigit.Digit.SystemFont.Color = userControl_SystemFont_Max.comboBoxGetColorString();

                if (follow_max)
                {
                    if (follow_min)
                    {
                        if (activity == null) activity = new Activity();
                        if (activity.Digits == null) activity.Digits = new List<DigitalCommonDigit>();
                        activity.Digits.Add(digitalCommonDigit);
                    }
                    else
                    {
                        if (activityMin == null) activityMin = new Activity();
                        if (activityMin.Digits == null) activityMin.Digits = new List<DigitalCommonDigit>();
                        activityMin.Digits.Add(digitalCommonDigit);
                    }
                }
                else
                {
                    if (activityMax == null) activityMax = new Activity();
                    if (activityMax.Digits == null) activityMax.Digits = new List<DigitalCommonDigit>();
                    activityMax.Digits.Add(digitalCommonDigit);
                }

            }

            follow_min = true;
            follow_max = true;

            // данные системным шрифтом по окружности current temperature
            if (userControl_FontRotate_Current != null && userControl_FontRotate_Current.checkBox_Use.Checked)
            {
                NumericUpDown numericUpDownX = userControl_FontRotate_Current.numericUpDown_FontRotateX;
                NumericUpDown numericUpDownY = userControl_FontRotate_Current.numericUpDown_FontRotateY;
                NumericUpDown numericUpDown_size = userControl_FontRotate_Current.numericUpDown_FontRotate_size;
                NumericUpDown numericUpDown_angle = userControl_FontRotate_Current.numericUpDown_FontRotate_angle;
                NumericUpDown numericUpDown_radius = userControl_FontRotate_Current.numericUpDown_FontRotate_radius;
                NumericUpDown numericUpDown_spacing = userControl_FontRotate_Current.numericUpDown_FontRotate_spacing;
                bool follow = userControl_FontRotate_Current.checkBox_follow.Checked;
                bool add_zero = userControl_FontRotate_Current.checkBox_addZero.Checked;
                bool separator = userControl_FontRotate_Current.checkBox_separator.Checked;

                if (activity == null) activity = new Activity();
                if (activity.Digits == null) activity.Digits = new List<DigitalCommonDigit>();
                DigitalCommonDigit digitalCommonDigit = new DigitalCommonDigit();
                if (!follow) digitalCommonDigit.CombingMode = "Single";
                if (separator)
                {
                    digitalCommonDigit.Separator = new ImageCoord();
                    digitalCommonDigit.Separator.Coordinates = new Coordinates();
                    digitalCommonDigit.Separator.Coordinates.X = -1;
                    digitalCommonDigit.Separator.Coordinates.Y = -1;
                }

                if (digitalCommonDigit.Digit == null) digitalCommonDigit.Digit = new Text();
                if (digitalCommonDigit.Digit.SystemFont == null)
                    digitalCommonDigit.Digit.SystemFont = new SystemFont();
                if (digitalCommonDigit.Digit.SystemFont.FontRotate == null)
                    digitalCommonDigit.Digit.SystemFont.FontRotate = new FontRotate();

                digitalCommonDigit.Digit.PaddingZero = add_zero;
                digitalCommonDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;

                digitalCommonDigit.Digit.SystemFont.FontRotate.X = (long)numericUpDownX.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.Y = (long)numericUpDownY.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.Radius = (long)numericUpDown_radius.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.RotateDirection =
                    userControl_FontRotate_Current.radioButtonGetRotateDirection();
                digitalCommonDigit.Digit.SystemFont.Size = (long)numericUpDown_size.Value;
                digitalCommonDigit.Digit.SystemFont.Angle = (long)numericUpDown_angle.Value;

                digitalCommonDigit.Digit.SystemFont.ShowUnitCheck = userControl_FontRotate_Current.checkBoxGetUnit();
                digitalCommonDigit.Digit.SystemFont.Color = userControl_FontRotate_Current.comboBoxGetColorString();

                activity.Digits.Add(digitalCommonDigit);
            }

            // данные системным шрифтом по окружности min temperature
            if (userControl_FontRotate_Min != null && userControl_FontRotate_Min.checkBox_Use.Checked)
            {
                NumericUpDown numericUpDownX = userControl_FontRotate_Min.numericUpDown_FontRotateX;
                NumericUpDown numericUpDownY = userControl_FontRotate_Min.numericUpDown_FontRotateY;
                NumericUpDown numericUpDown_size = userControl_FontRotate_Min.numericUpDown_FontRotate_size;
                NumericUpDown numericUpDown_angle = userControl_FontRotate_Min.numericUpDown_FontRotate_angle;
                NumericUpDown numericUpDown_radius = userControl_FontRotate_Min.numericUpDown_FontRotate_radius;
                NumericUpDown numericUpDown_spacing = userControl_FontRotate_Min.numericUpDown_FontRotate_spacing;
                bool follow = userControl_FontRotate_Min.checkBox_follow.Checked;
                follow_min = follow;
                bool add_zero = userControl_FontRotate_Min.checkBox_addZero.Checked;
                bool separator = userControl_FontRotate_Min.checkBox_separator.Checked;

                DigitalCommonDigit digitalCommonDigit = new DigitalCommonDigit();
                digitalCommonDigit.Type = "Min";
                if (!follow) digitalCommonDigit.CombingMode = "Single";
                if (separator)
                {
                    digitalCommonDigit.Separator = new ImageCoord();
                    digitalCommonDigit.Separator.Coordinates = new Coordinates();
                    digitalCommonDigit.Separator.Coordinates.X = -1;
                    digitalCommonDigit.Separator.Coordinates.Y = -1;
                }

                if (digitalCommonDigit.Digit == null) digitalCommonDigit.Digit = new Text();
                if (digitalCommonDigit.Digit.SystemFont == null)
                    digitalCommonDigit.Digit.SystemFont = new SystemFont();
                if (digitalCommonDigit.Digit.SystemFont.FontRotate == null)
                    digitalCommonDigit.Digit.SystemFont.FontRotate = new FontRotate();

                digitalCommonDigit.Digit.PaddingZero = add_zero;
                digitalCommonDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;

                digitalCommonDigit.Digit.SystemFont.FontRotate.X = (long)numericUpDownX.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.Y = (long)numericUpDownY.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.Radius = (long)numericUpDown_radius.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.RotateDirection =
                    userControl_FontRotate_Min.radioButtonGetRotateDirection();
                digitalCommonDigit.Digit.SystemFont.Size = (long)numericUpDown_size.Value;
                digitalCommonDigit.Digit.SystemFont.Angle = (long)numericUpDown_angle.Value;

                digitalCommonDigit.Digit.SystemFont.ShowUnitCheck = userControl_FontRotate_Min.checkBoxGetUnit();
                digitalCommonDigit.Digit.SystemFont.Color = userControl_FontRotate_Min.comboBoxGetColorString();

                if (follow_min)
                {
                    if (activity == null) activity = new Activity();
                    if (activity.Digits == null) activity.Digits = new List<DigitalCommonDigit>();
                    activity.Digits.Add(digitalCommonDigit);
                }
                else
                {
                    if (activityMin == null) activityMin = new Activity();
                    if (activityMin.Digits == null) activityMin.Digits = new List<DigitalCommonDigit>();
                    activityMin.Digits.Add(digitalCommonDigit);
                }
            }

            // данные системным шрифтом по окружности max temperature
            if (userControl_FontRotate_Max != null && userControl_FontRotate_Max.checkBox_Use.Checked)
            {
                NumericUpDown numericUpDownX = userControl_FontRotate_Max.numericUpDown_FontRotateX;
                NumericUpDown numericUpDownY = userControl_FontRotate_Max.numericUpDown_FontRotateY;
                NumericUpDown numericUpDown_size = userControl_FontRotate_Max.numericUpDown_FontRotate_size;
                NumericUpDown numericUpDown_angle = userControl_FontRotate_Max.numericUpDown_FontRotate_angle;
                NumericUpDown numericUpDown_radius = userControl_FontRotate_Max.numericUpDown_FontRotate_radius;
                NumericUpDown numericUpDown_spacing = userControl_FontRotate_Max.numericUpDown_FontRotate_spacing;
                bool follow = userControl_FontRotate_Max.checkBox_follow.Checked;
                follow_max = follow;
                bool add_zero = userControl_FontRotate_Max.checkBox_addZero.Checked;
                bool separator = userControl_FontRotate_Max.checkBox_separator.Checked;

                DigitalCommonDigit digitalCommonDigit = new DigitalCommonDigit();
                digitalCommonDigit.Type = "Max";
                if (!follow) digitalCommonDigit.CombingMode = "Single";
                if (separator)
                {
                    digitalCommonDigit.Separator = new ImageCoord();
                    digitalCommonDigit.Separator.Coordinates = new Coordinates();
                    digitalCommonDigit.Separator.Coordinates.X = -1;
                    digitalCommonDigit.Separator.Coordinates.Y = -1;
                }

                if (digitalCommonDigit.Digit == null) digitalCommonDigit.Digit = new Text();
                if (digitalCommonDigit.Digit.SystemFont == null)
                    digitalCommonDigit.Digit.SystemFont = new SystemFont();
                if (digitalCommonDigit.Digit.SystemFont.FontRotate == null)
                    digitalCommonDigit.Digit.SystemFont.FontRotate = new FontRotate();

                digitalCommonDigit.Digit.PaddingZero = add_zero;
                digitalCommonDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;

                digitalCommonDigit.Digit.SystemFont.FontRotate.X = (long)numericUpDownX.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.Y = (long)numericUpDownY.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.Radius = (long)numericUpDown_radius.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.RotateDirection =
                    userControl_FontRotate_Max.radioButtonGetRotateDirection();
                digitalCommonDigit.Digit.SystemFont.Size = (long)numericUpDown_size.Value;
                digitalCommonDigit.Digit.SystemFont.Angle = (long)numericUpDown_angle.Value;

                digitalCommonDigit.Digit.SystemFont.ShowUnitCheck = userControl_FontRotate_Max.checkBoxGetUnit();
                digitalCommonDigit.Digit.SystemFont.Color = userControl_FontRotate_Max.comboBoxGetColorString();

                if (follow_max)
                {
                    if (follow_min)
                    {
                        if (activity == null) activity = new Activity();
                        if (activity.Digits == null) activity.Digits = new List<DigitalCommonDigit>();
                        activity.Digits.Add(digitalCommonDigit);
                    }
                    else
                    {
                        if (activityMin == null) activityMin = new Activity();
                        if (activityMin.Digits == null) activityMin.Digits = new List<DigitalCommonDigit>();
                        activityMin.Digits.Add(digitalCommonDigit);
                    }
                }
                else
                {
                    if (activityMax == null) activityMax = new Activity();
                    if (activityMax.Digits == null) activityMax.Digits = new List<DigitalCommonDigit>();
                    activityMax.Digits.Add(digitalCommonDigit);
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
            bool scaleCircle = false;
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
                    activity.ProgressBar.Flatness = panel_scaleCircle.comboBoxGetFlatness();
                    scaleCircle = true;
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
                    if (scaleCircle)
                    {
                        if (activityMin == null) activityMin = new Activity();
                        if (activityMin.ProgressBar == null) activityMin.ProgressBar = new ProgressBar();
                        activityMin.ProgressBar.LinearSettings = new LinearSettings();
                        if (radioButton_image.Checked && panel_scaleLinear.comboBoxGetSelectedIndexImage() >= 0)
                        {
                            activityMin.ProgressBar.ForegroundImageIndex = panel_scaleLinear.comboBoxGetImage();
                        }
                        else
                        {
                            activityMin.ProgressBar.Color = panel_scaleLinear.comboBoxGetColorString();
                        }
                        if (panel_scaleLinear.comboBoxGetSelectedIndexImagePointer() >= 0)
                            activityMin.ProgressBar.PointerImageIndex = panel_scaleLinear.comboBoxGetImagePointer();
                        if (panel_scaleLinear.comboBoxGetSelectedIndexImageBackground() >= 0)
                            activityMin.ProgressBar.BackgroundImageIndex = panel_scaleLinear.comboBoxGetImageBackground();

                        activityMin.ProgressBar.LinearSettings.StartX = (long)numericUpDownX.Value;
                        activityMin.ProgressBar.LinearSettings.StartY = (long)numericUpDownY.Value;
                        long endX = (long)(numericUpDownX.Value + numericUpDown_length.Value);
                        activityMin.ProgressBar.LinearSettings.EndX = endX;
                        activityMin.ProgressBar.LinearSettings.EndY = (long)numericUpDownY.Value;
                        activityMin.ProgressBar.Width = (long)numericUpDown_width.Value;
                    }
                    else
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

            }

            // данные иконки
            if (userControl_icon != null && userControl_icon.checkBox_icon_Use.Checked)
            {
                int image = userControl_icon.comboBoxGetImage();
                if (image >= 0)
                {
                    NumericUpDown numericUpDownX = (NumericUpDown)userControl_icon.numericUpDown_iconX;
                    NumericUpDown numericUpDownY = (NumericUpDown)userControl_icon.numericUpDown_iconY;
                    int image2 = userControl_icon.comboBoxGetImage2();

                    if (activity == null) activity = new Activity();
                    activity.Icon = new ImageCoord();
                    activity.Icon.Coordinates = new Coordinates();
                    activity.Icon.ImageIndex = image;
                    if (image2 >= 0) activity.Icon.ImageIndex2 = image2;
                    activity.Icon.Coordinates.X = (long)numericUpDownX.Value;
                    activity.Icon.Coordinates.Y = (long)numericUpDownY.Value;
                }
            }

            if (checkBox_weatherAlignmentFix_AOD.Checked)
            {
                if (activityPictures != null)
                {
                    activityPictures.Type = "Weather";
                    //activity.WeatherAlignmentFix = true;
                    if (ScreenIdle.Activity == null) ScreenIdle.Activity = new List<Activity>();
                    ScreenIdle.Activity.Add(activityPictures);
                }
            }

            if (activity != null)
            {
                activity.Type = "Weather";
                if (ScreenIdle.Activity == null) ScreenIdle.Activity = new List<Activity>();
                ScreenIdle.Activity.Add(activity);
            }

            if (activityMin != null)
            {
                activityMin.Type = "Weather";
                if (ScreenIdle.Activity == null) ScreenIdle.Activity = new List<Activity>();
                ScreenIdle.Activity.Add(activityMin);
            }

            if (activityMax != null)
            {
                activityMax.Type = "Weather";
                if (ScreenIdle.Activity == null) ScreenIdle.Activity = new List<Activity>();
                ScreenIdle.Activity.Add(activityMax);
            }

            if (!checkBox_weatherAlignmentFix_AOD.Checked)
            {
                if (activityPictures != null)
                {
                    activityPictures.Type = "Weather";
                    if (ScreenIdle.Activity == null) ScreenIdle.Activity = new List<Activity>();
                    ScreenIdle.Activity.Add(activityPictures);
                } 
            }
        }

        private void AddActivitySunrise_AOD(ScreenIdle ScreenIdle, UserControl_pictures panel_pictures,
            UserControl_text_goal panel_text, UserControl_text_goal panel_text_min,
            UserControl_text_goal panel_text_max, UserControl_hand panel_hand,
            UserControl_scaleCircle panel_scaleCircle, UserControl_scaleLinear panel_scaleLinear,
            UserControl_SystemFont_GroupWeather userControl_SystemFont_Group, UserControl_icon userControl_icon)
        {
            UserControl_SystemFont userControl_SystemFont_Current = userControl_SystemFont_Group.userControl_SystemFont_weather_Current;
            UserControl_SystemFont userControl_SystemFont_Min = userControl_SystemFont_Group.userControl_SystemFont_weather_Min;
            UserControl_SystemFont userControl_SystemFont_Max = userControl_SystemFont_Group.userControl_SystemFont_weather_Max;

            UserControl_FontRotate userControl_FontRotate_Current = userControl_SystemFont_Group.userControl_FontRotate_weather_Current;
            UserControl_FontRotate userControl_FontRotate_Min = userControl_SystemFont_Group.userControl_FontRotate_weather_Min;
            UserControl_FontRotate userControl_FontRotate_Max = userControl_SystemFont_Group.userControl_FontRotate_weather_Max;


            Activity activity = null;
            Activity activitySunrise = null;
            Activity activitySunset = null;

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
                    activity.ImageProgress.ImageSet.ImagesCount = 2;
                    //activity.ImageProgress.ImageSet.ImagesCount = (long)numericUpDown_count.Value;
                    Coordinates coordinates = new Coordinates();
                    coordinates.X = (long)numericUpDownX.Value;
                    coordinates.Y = (long)numericUpDownY.Value;
                    activity.ImageProgress.Coordinates.Add(coordinates);
                }
            }

            // данные надписью SunriseSunset
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
                        digitalCommonDigit.Digit.Image.DecimalPointImageIndex = panel_text.comboBoxGetImageDecimalPointOrMinus();

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

            // данные надписью Sunrise
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
                        digitalCommonDigit.Digit.Image.DecimalPointImageIndex = panel_text_min.comboBoxGetImageDecimalPointOrMinus();

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

            // данные надписью Sunset
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
                        digitalCommonDigit.Digit.Image.DecimalPointImageIndex = panel_text_max.comboBoxGetImageDecimalPointOrMinus();

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

            bool follow_sunrise = true;
            bool follow_sunset = true;

            // данные системным шрифтом SunriseSunset
            if (userControl_SystemFont_Current != null && userControl_SystemFont_Current.checkBox_Use.Checked)
            {
                NumericUpDown numericUpDownX = userControl_SystemFont_Current.numericUpDown_SystemFontX;
                NumericUpDown numericUpDownY = userControl_SystemFont_Current.numericUpDown_SystemFontY;
                NumericUpDown numericUpDown_size = userControl_SystemFont_Current.numericUpDown_SystemFont_size;
                NumericUpDown numericUpDown_angle = userControl_SystemFont_Current.numericUpDown_SystemFont_angle;
                NumericUpDown numericUpDown_spacing = userControl_SystemFont_Current.numericUpDown_SystemFont_spacing;
                bool follow = userControl_SystemFont_Current.checkBox_follow.Checked;
                bool add_zero = userControl_SystemFont_Current.checkBox_addZero.Checked;
                bool separator = userControl_SystemFont_Current.checkBox_separator.Checked;

                if (activity == null) activity = new Activity();
                if (activity.Digits == null) activity.Digits = new List<DigitalCommonDigit>();
                DigitalCommonDigit digitalCommonDigit = new DigitalCommonDigit();
                if (!follow) digitalCommonDigit.CombingMode = "Single";
                if (separator)
                {
                    digitalCommonDigit.Separator = new ImageCoord();
                    digitalCommonDigit.Separator.Coordinates = new Coordinates();
                    digitalCommonDigit.Separator.Coordinates.X = -1;
                    digitalCommonDigit.Separator.Coordinates.Y = -1;
                }


                if (digitalCommonDigit.Digit == null) digitalCommonDigit.Digit = new Text();
                if (digitalCommonDigit.Digit.SystemFont == null)
                    digitalCommonDigit.Digit.SystemFont = new SystemFont();
                if (digitalCommonDigit.Digit.SystemFont.Coordinates == null)
                    digitalCommonDigit.Digit.SystemFont.Coordinates = new Coordinates();

                digitalCommonDigit.Digit.PaddingZero = add_zero;
                digitalCommonDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;

                digitalCommonDigit.Digit.SystemFont.Coordinates.X = (long)numericUpDownX.Value;
                digitalCommonDigit.Digit.SystemFont.Coordinates.Y = (long)numericUpDownY.Value;
                digitalCommonDigit.Digit.SystemFont.Size = (long)numericUpDown_size.Value;
                digitalCommonDigit.Digit.SystemFont.Angle = (long)numericUpDown_angle.Value;

                digitalCommonDigit.Digit.SystemFont.ShowUnitCheck = userControl_SystemFont_Current.checkBoxGetUnit();
                digitalCommonDigit.Digit.SystemFont.Color = userControl_SystemFont_Current.comboBoxGetColorString();

                activity.Digits.Add(digitalCommonDigit);
            }

            // данные системным шрифтом Sunrise
            if (userControl_SystemFont_Min != null && userControl_SystemFont_Min.checkBox_Use.Checked)
            {
                NumericUpDown numericUpDownX = userControl_SystemFont_Min.numericUpDown_SystemFontX;
                NumericUpDown numericUpDownY = userControl_SystemFont_Min.numericUpDown_SystemFontY;
                NumericUpDown numericUpDown_size = userControl_SystemFont_Min.numericUpDown_SystemFont_size;
                NumericUpDown numericUpDown_angle = userControl_SystemFont_Min.numericUpDown_SystemFont_angle;
                NumericUpDown numericUpDown_spacing = userControl_SystemFont_Min.numericUpDown_SystemFont_spacing;
                bool follow = userControl_SystemFont_Min.checkBox_follow.Checked;
                follow_sunrise = follow;
                bool add_zero = userControl_SystemFont_Min.checkBox_addZero.Checked;
                bool separator = userControl_SystemFont_Min.checkBox_separator.Checked;

                DigitalCommonDigit digitalCommonDigit = new DigitalCommonDigit();
                digitalCommonDigit.Type = "Min";
                if (!follow) digitalCommonDigit.CombingMode = "Single";
                if (separator)
                {
                    digitalCommonDigit.Separator = new ImageCoord();
                    digitalCommonDigit.Separator.Coordinates = new Coordinates();
                    digitalCommonDigit.Separator.Coordinates.X = -1;
                    digitalCommonDigit.Separator.Coordinates.Y = -1;
                }


                if (digitalCommonDigit.Digit == null) digitalCommonDigit.Digit = new Text();
                if (digitalCommonDigit.Digit.SystemFont == null)
                    digitalCommonDigit.Digit.SystemFont = new SystemFont();
                if (digitalCommonDigit.Digit.SystemFont.Coordinates == null)
                    digitalCommonDigit.Digit.SystemFont.Coordinates = new Coordinates();

                digitalCommonDigit.Digit.PaddingZero = add_zero;
                digitalCommonDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;

                digitalCommonDigit.Digit.SystemFont.Coordinates.X = (long)numericUpDownX.Value;
                digitalCommonDigit.Digit.SystemFont.Coordinates.Y = (long)numericUpDownY.Value;
                digitalCommonDigit.Digit.SystemFont.Size = (long)numericUpDown_size.Value;
                digitalCommonDigit.Digit.SystemFont.Angle = (long)numericUpDown_angle.Value;

                digitalCommonDigit.Digit.SystemFont.ShowUnitCheck = userControl_SystemFont_Min.checkBoxGetUnit();
                digitalCommonDigit.Digit.SystemFont.Color = userControl_SystemFont_Min.comboBoxGetColorString();

                if (follow_sunrise)
                {
                    if (activity == null) activity = new Activity();
                    if (activity.Digits == null) activity.Digits = new List<DigitalCommonDigit>();
                    activity.Digits.Add(digitalCommonDigit);
                }
                else
                {
                    if (activitySunrise == null) activitySunrise = new Activity();
                    if (activitySunrise.Digits == null) activitySunrise.Digits = new List<DigitalCommonDigit>();
                    activitySunrise.Digits.Add(digitalCommonDigit);
                }
            }

            // данные системным шрифтом Sunset
            if (userControl_SystemFont_Max != null && userControl_SystemFont_Max.checkBox_Use.Checked)
            {
                NumericUpDown numericUpDownX = userControl_SystemFont_Max.numericUpDown_SystemFontX;
                NumericUpDown numericUpDownY = userControl_SystemFont_Max.numericUpDown_SystemFontY;
                NumericUpDown numericUpDown_size = userControl_SystemFont_Max.numericUpDown_SystemFont_size;
                NumericUpDown numericUpDown_angle = userControl_SystemFont_Max.numericUpDown_SystemFont_angle;
                NumericUpDown numericUpDown_spacing = userControl_SystemFont_Max.numericUpDown_SystemFont_spacing;
                bool follow = userControl_SystemFont_Max.checkBox_follow.Checked;
                follow_sunset = follow;
                bool add_zero = userControl_SystemFont_Max.checkBox_addZero.Checked;
                bool separator = userControl_SystemFont_Max.checkBox_separator.Checked;

                DigitalCommonDigit digitalCommonDigit = new DigitalCommonDigit();
                digitalCommonDigit.Type = "Max";
                if (!follow) digitalCommonDigit.CombingMode = "Single";
                if (separator)
                {
                    digitalCommonDigit.Separator = new ImageCoord();
                    digitalCommonDigit.Separator.Coordinates = new Coordinates();
                    digitalCommonDigit.Separator.Coordinates.X = -1;
                    digitalCommonDigit.Separator.Coordinates.Y = -1;
                }


                if (digitalCommonDigit.Digit == null) digitalCommonDigit.Digit = new Text();
                if (digitalCommonDigit.Digit.SystemFont == null)
                    digitalCommonDigit.Digit.SystemFont = new SystemFont();
                if (digitalCommonDigit.Digit.SystemFont.Coordinates == null)
                    digitalCommonDigit.Digit.SystemFont.Coordinates = new Coordinates();

                digitalCommonDigit.Digit.PaddingZero = add_zero;
                digitalCommonDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;

                digitalCommonDigit.Digit.SystemFont.Coordinates.X = (long)numericUpDownX.Value;
                digitalCommonDigit.Digit.SystemFont.Coordinates.Y = (long)numericUpDownY.Value;
                digitalCommonDigit.Digit.SystemFont.Size = (long)numericUpDown_size.Value;
                digitalCommonDigit.Digit.SystemFont.Angle = (long)numericUpDown_angle.Value;

                digitalCommonDigit.Digit.SystemFont.ShowUnitCheck = userControl_SystemFont_Max.checkBoxGetUnit();
                digitalCommonDigit.Digit.SystemFont.Color = userControl_SystemFont_Max.comboBoxGetColorString();

                if (follow_sunset)
                {
                    if (follow_sunrise)
                    {
                        if (activity == null) activity = new Activity();
                        if (activity.Digits == null) activity.Digits = new List<DigitalCommonDigit>();
                        activity.Digits.Add(digitalCommonDigit);
                    }
                    else
                    {
                        if (activitySunrise == null) activitySunrise = new Activity();
                        if (activitySunrise.Digits == null) activitySunrise.Digits = new List<DigitalCommonDigit>();
                        activitySunrise.Digits.Add(digitalCommonDigit);
                    }
                }
                else
                {
                    if (activitySunset == null) activitySunset = new Activity();
                    if (activitySunset.Digits == null) activitySunset.Digits = new List<DigitalCommonDigit>();
                    activitySunset.Digits.Add(digitalCommonDigit);
                }
            }

            follow_sunrise = true;
            follow_sunset = true;

            // данные системным шрифтом по окружности SunriseSunset
            if (userControl_FontRotate_Current != null && userControl_FontRotate_Current.checkBox_Use.Checked)
            {
                NumericUpDown numericUpDownX = userControl_FontRotate_Current.numericUpDown_FontRotateX;
                NumericUpDown numericUpDownY = userControl_FontRotate_Current.numericUpDown_FontRotateY;
                NumericUpDown numericUpDown_size = userControl_FontRotate_Current.numericUpDown_FontRotate_size;
                NumericUpDown numericUpDown_angle = userControl_FontRotate_Current.numericUpDown_FontRotate_angle;
                NumericUpDown numericUpDown_radius = userControl_FontRotate_Current.numericUpDown_FontRotate_radius;
                NumericUpDown numericUpDown_spacing = userControl_FontRotate_Current.numericUpDown_FontRotate_spacing;
                bool follow = userControl_FontRotate_Current.checkBox_follow.Checked;
                bool add_zero = userControl_FontRotate_Current.checkBox_addZero.Checked;
                bool separator = userControl_FontRotate_Current.checkBox_separator.Checked;

                if (activity == null) activity = new Activity();
                if (activity.Digits == null) activity.Digits = new List<DigitalCommonDigit>();
                DigitalCommonDigit digitalCommonDigit = new DigitalCommonDigit();
                if (!follow) digitalCommonDigit.CombingMode = "Single";
                if (separator)
                {
                    digitalCommonDigit.Separator = new ImageCoord();
                    digitalCommonDigit.Separator.Coordinates = new Coordinates();
                    digitalCommonDigit.Separator.Coordinates.X = -1;
                    digitalCommonDigit.Separator.Coordinates.Y = -1;
                }

                if (digitalCommonDigit.Digit == null) digitalCommonDigit.Digit = new Text();
                if (digitalCommonDigit.Digit.SystemFont == null)
                    digitalCommonDigit.Digit.SystemFont = new SystemFont();
                if (digitalCommonDigit.Digit.SystemFont.FontRotate == null)
                    digitalCommonDigit.Digit.SystemFont.FontRotate = new FontRotate();

                digitalCommonDigit.Digit.PaddingZero = add_zero;
                digitalCommonDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;

                digitalCommonDigit.Digit.SystemFont.FontRotate.X = (long)numericUpDownX.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.Y = (long)numericUpDownY.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.Radius = (long)numericUpDown_radius.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.RotateDirection =
                    userControl_FontRotate_Current.radioButtonGetRotateDirection();
                digitalCommonDigit.Digit.SystemFont.Size = (long)numericUpDown_size.Value;
                digitalCommonDigit.Digit.SystemFont.Angle = (long)numericUpDown_angle.Value;

                digitalCommonDigit.Digit.SystemFont.ShowUnitCheck = userControl_FontRotate_Current.checkBoxGetUnit();
                digitalCommonDigit.Digit.SystemFont.Color = userControl_FontRotate_Current.comboBoxGetColorString();

                activity.Digits.Add(digitalCommonDigit);
            }

            // данные системным шрифтом по окружности Sunrise
            if (userControl_FontRotate_Min != null && userControl_FontRotate_Min.checkBox_Use.Checked)
            {
                NumericUpDown numericUpDownX = userControl_FontRotate_Min.numericUpDown_FontRotateX;
                NumericUpDown numericUpDownY = userControl_FontRotate_Min.numericUpDown_FontRotateY;
                NumericUpDown numericUpDown_size = userControl_FontRotate_Min.numericUpDown_FontRotate_size;
                NumericUpDown numericUpDown_angle = userControl_FontRotate_Min.numericUpDown_FontRotate_angle;
                NumericUpDown numericUpDown_radius = userControl_FontRotate_Min.numericUpDown_FontRotate_radius;
                NumericUpDown numericUpDown_spacing = userControl_FontRotate_Min.numericUpDown_FontRotate_spacing;
                bool follow = userControl_FontRotate_Min.checkBox_follow.Checked;
                follow_sunrise = follow;
                bool add_zero = userControl_FontRotate_Min.checkBox_addZero.Checked;
                bool separator = userControl_FontRotate_Min.checkBox_separator.Checked;

                DigitalCommonDigit digitalCommonDigit = new DigitalCommonDigit();
                digitalCommonDigit.Type = "Min";
                if (!follow) digitalCommonDigit.CombingMode = "Single";
                if (separator)
                {
                    digitalCommonDigit.Separator = new ImageCoord();
                    digitalCommonDigit.Separator.Coordinates = new Coordinates();
                    digitalCommonDigit.Separator.Coordinates.X = -1;
                    digitalCommonDigit.Separator.Coordinates.Y = -1;
                }

                if (digitalCommonDigit.Digit == null) digitalCommonDigit.Digit = new Text();
                if (digitalCommonDigit.Digit.SystemFont == null)
                    digitalCommonDigit.Digit.SystemFont = new SystemFont();
                if (digitalCommonDigit.Digit.SystemFont.FontRotate == null)
                    digitalCommonDigit.Digit.SystemFont.FontRotate = new FontRotate();

                digitalCommonDigit.Digit.PaddingZero = add_zero;
                digitalCommonDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;

                digitalCommonDigit.Digit.SystemFont.FontRotate.X = (long)numericUpDownX.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.Y = (long)numericUpDownY.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.Radius = (long)numericUpDown_radius.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.RotateDirection =
                    userControl_FontRotate_Min.radioButtonGetRotateDirection();
                digitalCommonDigit.Digit.SystemFont.Size = (long)numericUpDown_size.Value;
                digitalCommonDigit.Digit.SystemFont.Angle = (long)numericUpDown_angle.Value;

                digitalCommonDigit.Digit.SystemFont.ShowUnitCheck = userControl_FontRotate_Min.checkBoxGetUnit();
                digitalCommonDigit.Digit.SystemFont.Color = userControl_FontRotate_Min.comboBoxGetColorString();

                if (follow_sunrise)
                {
                    if (activity == null) activity = new Activity();
                    if (activity.Digits == null) activity.Digits = new List<DigitalCommonDigit>();
                    activity.Digits.Add(digitalCommonDigit);
                }
                else
                {
                    if (activitySunrise == null) activitySunrise = new Activity();
                    if (activitySunrise.Digits == null) activitySunrise.Digits = new List<DigitalCommonDigit>();
                    activitySunrise.Digits.Add(digitalCommonDigit);
                }
            }

            // данные системным шрифтом по окружности Sunset
            if (userControl_FontRotate_Max != null && userControl_FontRotate_Max.checkBox_Use.Checked)
            {
                NumericUpDown numericUpDownX = userControl_FontRotate_Max.numericUpDown_FontRotateX;
                NumericUpDown numericUpDownY = userControl_FontRotate_Max.numericUpDown_FontRotateY;
                NumericUpDown numericUpDown_size = userControl_FontRotate_Max.numericUpDown_FontRotate_size;
                NumericUpDown numericUpDown_angle = userControl_FontRotate_Max.numericUpDown_FontRotate_angle;
                NumericUpDown numericUpDown_radius = userControl_FontRotate_Max.numericUpDown_FontRotate_radius;
                NumericUpDown numericUpDown_spacing = userControl_FontRotate_Max.numericUpDown_FontRotate_spacing;
                bool follow = userControl_FontRotate_Max.checkBox_follow.Checked;
                follow_sunset = follow;
                bool add_zero = userControl_FontRotate_Max.checkBox_addZero.Checked;
                bool separator = userControl_FontRotate_Max.checkBox_separator.Checked;

                DigitalCommonDigit digitalCommonDigit = new DigitalCommonDigit();
                digitalCommonDigit.Type = "Max";
                if (!follow) digitalCommonDigit.CombingMode = "Single";
                if (separator)
                {
                    digitalCommonDigit.Separator = new ImageCoord();
                    digitalCommonDigit.Separator.Coordinates = new Coordinates();
                    digitalCommonDigit.Separator.Coordinates.X = -1;
                    digitalCommonDigit.Separator.Coordinates.Y = -1;
                }

                if (digitalCommonDigit.Digit == null) digitalCommonDigit.Digit = new Text();
                if (digitalCommonDigit.Digit.SystemFont == null)
                    digitalCommonDigit.Digit.SystemFont = new SystemFont();
                if (digitalCommonDigit.Digit.SystemFont.FontRotate == null)
                    digitalCommonDigit.Digit.SystemFont.FontRotate = new FontRotate();

                digitalCommonDigit.Digit.PaddingZero = add_zero;
                digitalCommonDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;

                digitalCommonDigit.Digit.SystemFont.FontRotate.X = (long)numericUpDownX.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.Y = (long)numericUpDownY.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.Radius = (long)numericUpDown_radius.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.RotateDirection =
                    userControl_FontRotate_Max.radioButtonGetRotateDirection();
                digitalCommonDigit.Digit.SystemFont.Size = (long)numericUpDown_size.Value;
                digitalCommonDigit.Digit.SystemFont.Angle = (long)numericUpDown_angle.Value;

                digitalCommonDigit.Digit.SystemFont.ShowUnitCheck = userControl_FontRotate_Max.checkBoxGetUnit();
                digitalCommonDigit.Digit.SystemFont.Color = userControl_FontRotate_Max.comboBoxGetColorString();

                if (follow_sunset)
                {
                    if (follow_sunrise)
                    {
                        if (activity == null) activity = new Activity();
                        if (activity.Digits == null) activity.Digits = new List<DigitalCommonDigit>();
                        activity.Digits.Add(digitalCommonDigit);
                    }
                    else
                    {
                        if (activitySunrise == null) activitySunrise = new Activity();
                        if (activitySunrise.Digits == null) activitySunrise.Digits = new List<DigitalCommonDigit>();
                        activitySunrise.Digits.Add(digitalCommonDigit);
                    }
                }
                else
                {
                    if (activitySunset == null) activitySunset = new Activity();
                    if (activitySunset.Digits == null) activitySunset.Digits = new List<DigitalCommonDigit>();
                    activitySunset.Digits.Add(digitalCommonDigit);
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

            // данные иконки
            if (userControl_icon != null && userControl_icon.checkBox_icon_Use.Checked)
            {
                int image = userControl_icon.comboBoxGetImage();
                if (image >= 0)
                {
                    NumericUpDown numericUpDownX = (NumericUpDown)userControl_icon.numericUpDown_iconX;
                    NumericUpDown numericUpDownY = (NumericUpDown)userControl_icon.numericUpDown_iconY;
                    int image2 = userControl_icon.comboBoxGetImage2();

                    if (activity == null) activity = new Activity();
                    activity.Icon = new ImageCoord();
                    activity.Icon.Coordinates = new Coordinates();
                    activity.Icon.ImageIndex = image;
                    if (image2 >= 0) activity.Icon.ImageIndex2 = image2;
                    activity.Icon.Coordinates.X = (long)numericUpDownX.Value;
                    activity.Icon.Coordinates.Y = (long)numericUpDownY.Value;
                }
            }

            if (activity != null)
            {
                activity.Type = "Sunrise";
                if (ScreenIdle.Activity == null) ScreenIdle.Activity = new List<Activity>();
                ScreenIdle.Activity.Add(activity);
            }

            if (activitySunrise != null)
            {
                activitySunrise.Type = "Sunrise";
                if (ScreenIdle.Activity == null) ScreenIdle.Activity = new List<Activity>();
                ScreenIdle.Activity.Add(activitySunrise);
            }

            if (activitySunset != null)
            {
                activitySunset.Type = "Sunrise";
                if (ScreenIdle.Activity == null) ScreenIdle.Activity = new List<Activity>();
                ScreenIdle.Activity.Add(activitySunset);
            }
        }

        private void AddActivityTime_AOD(ScreenIdle ScreenIdle, UserControl_text_goal panel_textHour, UserControl_text_goal panel_textMinute,
            UserControl_text_goal panel_textSecond, UserControl_SystemFont_GroupWeather userControl_SystemFont_Group)
        {
            UserControl_SystemFont userControl_SystemFont_Hour = userControl_SystemFont_Group.userControl_SystemFont_weather_Current;
            UserControl_SystemFont userControl_SystemFont_Minute = userControl_SystemFont_Group.userControl_SystemFont_weather_Min;
            UserControl_SystemFont userControl_SystemFont_Second = null;

            UserControl_FontRotate userControl_FontRotate_Hour = userControl_SystemFont_Group.userControl_FontRotate_weather_Current;
            UserControl_FontRotate userControl_FontRotate_Minute = userControl_SystemFont_Group.userControl_FontRotate_weather_Min;
            UserControl_FontRotate userControl_FontRotate_Second = null;


            List<DigitalTimeDigit> digits = null;


            //bool follow_min = true;
            //bool follow_sec = true;

            // данные системным шрифтом часы
            if (userControl_SystemFont_Hour != null && userControl_SystemFont_Hour.checkBox_Use.Checked)
            {
                NumericUpDown numericUpDownX = userControl_SystemFont_Hour.numericUpDown_SystemFontX;
                NumericUpDown numericUpDownY = userControl_SystemFont_Hour.numericUpDown_SystemFontY;
                NumericUpDown numericUpDown_size = userControl_SystemFont_Hour.numericUpDown_SystemFont_size;
                NumericUpDown numericUpDown_angle = userControl_SystemFont_Hour.numericUpDown_SystemFont_angle;
                NumericUpDown numericUpDown_spacing = userControl_SystemFont_Hour.numericUpDown_SystemFont_spacing;
                bool follow = userControl_SystemFont_Hour.checkBox_follow.Checked;
                bool add_zero = userControl_SystemFont_Hour.checkBox_addZero.Checked;
                bool separator = userControl_SystemFont_Hour.checkBox_separator.Checked;

                if (digits == null) digits = new List<DigitalTimeDigit>();
                DigitalTimeDigit digitalCommonDigit = new DigitalTimeDigit();
                digitalCommonDigit.TimeType = "Hour";
                if (!follow) digitalCommonDigit.CombingMode = "Single";
                if (separator)
                {
                    digitalCommonDigit.Separator = new ImageCoord();
                    digitalCommonDigit.Separator.Coordinates = new Coordinates();
                    digitalCommonDigit.Separator.Coordinates.X = -1;
                    digitalCommonDigit.Separator.Coordinates.Y = -1;
                }


                if (digitalCommonDigit.Digit == null) digitalCommonDigit.Digit = new Text();
                if (digitalCommonDigit.Digit.SystemFont == null)
                    digitalCommonDigit.Digit.SystemFont = new SystemFont();
                if (digitalCommonDigit.Digit.SystemFont.Coordinates == null)
                    digitalCommonDigit.Digit.SystemFont.Coordinates = new Coordinates();

                digitalCommonDigit.Digit.PaddingZero = add_zero;
                digitalCommonDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;

                digitalCommonDigit.Digit.SystemFont.Coordinates.X = (long)numericUpDownX.Value;
                digitalCommonDigit.Digit.SystemFont.Coordinates.Y = (long)numericUpDownY.Value;
                digitalCommonDigit.Digit.SystemFont.Size = (long)numericUpDown_size.Value;
                digitalCommonDigit.Digit.SystemFont.Angle = (long)numericUpDown_angle.Value;

                digitalCommonDigit.Digit.SystemFont.ShowUnitCheck = userControl_SystemFont_Hour.checkBoxGetUnit() + 1;
                digitalCommonDigit.Digit.SystemFont.Color = userControl_SystemFont_Hour.comboBoxGetColorString();

                //digits.Digits.Add(digitalCommonDigit);
                digits.Add(digitalCommonDigit);
            }

            // данные системным шрифтом минуты
            if (userControl_SystemFont_Minute != null && userControl_SystemFont_Minute.checkBox_Use.Checked)
            {
                NumericUpDown numericUpDownX = userControl_SystemFont_Minute.numericUpDown_SystemFontX;
                NumericUpDown numericUpDownY = userControl_SystemFont_Minute.numericUpDown_SystemFontY;
                NumericUpDown numericUpDown_size = userControl_SystemFont_Minute.numericUpDown_SystemFont_size;
                NumericUpDown numericUpDown_angle = userControl_SystemFont_Minute.numericUpDown_SystemFont_angle;
                NumericUpDown numericUpDown_spacing = userControl_SystemFont_Minute.numericUpDown_SystemFont_spacing;
                bool follow = userControl_SystemFont_Minute.checkBox_follow.Checked;
                //follow_min = follow;
                bool add_zero = userControl_SystemFont_Minute.checkBox_addZero.Checked;
                bool separator = userControl_SystemFont_Minute.checkBox_separator.Checked;

                if (digits == null) digits = new List<DigitalTimeDigit>();
                DigitalTimeDigit digitalCommonDigit = new DigitalTimeDigit();
                digitalCommonDigit.TimeType = "Minute";
                if (!follow) digitalCommonDigit.CombingMode = "Single";
                if (separator)
                {
                    digitalCommonDigit.Separator = new ImageCoord();
                    digitalCommonDigit.Separator.Coordinates = new Coordinates();
                    digitalCommonDigit.Separator.Coordinates.X = -1;
                    digitalCommonDigit.Separator.Coordinates.Y = -1;
                }


                if (digitalCommonDigit.Digit == null) digitalCommonDigit.Digit = new Text();
                if (digitalCommonDigit.Digit.SystemFont == null)
                    digitalCommonDigit.Digit.SystemFont = new SystemFont();
                if (digitalCommonDigit.Digit.SystemFont.Coordinates == null)
                    digitalCommonDigit.Digit.SystemFont.Coordinates = new Coordinates();

                digitalCommonDigit.Digit.PaddingZero = add_zero;
                digitalCommonDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;

                digitalCommonDigit.Digit.SystemFont.Coordinates.X = (long)numericUpDownX.Value;
                digitalCommonDigit.Digit.SystemFont.Coordinates.Y = (long)numericUpDownY.Value;
                digitalCommonDigit.Digit.SystemFont.Size = (long)numericUpDown_size.Value;
                digitalCommonDigit.Digit.SystemFont.Angle = (long)numericUpDown_angle.Value;

                digitalCommonDigit.Digit.SystemFont.ShowUnitCheck = userControl_SystemFont_Minute.checkBoxGetUnit() + 1;
                digitalCommonDigit.Digit.SystemFont.Color = userControl_SystemFont_Minute.comboBoxGetColorString();

                digits.Add(digitalCommonDigit);
            }

            // данные системным шрифтом секунды
            if (userControl_SystemFont_Second != null && userControl_SystemFont_Second.checkBox_Use.Checked)
            {
                NumericUpDown numericUpDownX = userControl_SystemFont_Second.numericUpDown_SystemFontX;
                NumericUpDown numericUpDownY = userControl_SystemFont_Second.numericUpDown_SystemFontY;
                NumericUpDown numericUpDown_size = userControl_SystemFont_Second.numericUpDown_SystemFont_size;
                NumericUpDown numericUpDown_angle = userControl_SystemFont_Second.numericUpDown_SystemFont_angle;
                NumericUpDown numericUpDown_spacing = userControl_SystemFont_Second.numericUpDown_SystemFont_spacing;
                bool follow = userControl_SystemFont_Second.checkBox_follow.Checked;
                //follow_sec = follow;
                bool add_zero = userControl_SystemFont_Second.checkBox_addZero.Checked;
                bool separator = userControl_SystemFont_Second.checkBox_separator.Checked;

                if (digits == null) digits = new List<DigitalTimeDigit>();
                DigitalTimeDigit digitalCommonDigit = new DigitalTimeDigit();
                digitalCommonDigit.TimeType = "Second";
                if (!follow) digitalCommonDigit.CombingMode = "Single";
                if (separator)
                {
                    digitalCommonDigit.Separator = new ImageCoord();
                    digitalCommonDigit.Separator.Coordinates = new Coordinates();
                    digitalCommonDigit.Separator.Coordinates.X = -1;
                    digitalCommonDigit.Separator.Coordinates.Y = -1;
                }


                if (digitalCommonDigit.Digit == null) digitalCommonDigit.Digit = new Text();
                if (digitalCommonDigit.Digit.SystemFont == null)
                    digitalCommonDigit.Digit.SystemFont = new SystemFont();
                if (digitalCommonDigit.Digit.SystemFont.Coordinates == null)
                    digitalCommonDigit.Digit.SystemFont.Coordinates = new Coordinates();

                digitalCommonDigit.Digit.PaddingZero = add_zero;
                digitalCommonDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;

                digitalCommonDigit.Digit.SystemFont.Coordinates.X = (long)numericUpDownX.Value;
                digitalCommonDigit.Digit.SystemFont.Coordinates.Y = (long)numericUpDownY.Value;
                digitalCommonDigit.Digit.SystemFont.Size = (long)numericUpDown_size.Value;
                digitalCommonDigit.Digit.SystemFont.Angle = (long)numericUpDown_angle.Value;

                digitalCommonDigit.Digit.SystemFont.ShowUnitCheck = userControl_SystemFont_Second.checkBoxGetUnit() + 1;
                digitalCommonDigit.Digit.SystemFont.Color = userControl_SystemFont_Second.comboBoxGetColorString();

                digits.Add(digitalCommonDigit);
            }

            //follow_min = true;
            //follow_sec = true;

            // данные системным шрифтом по окружности часы
            if (userControl_FontRotate_Hour != null && userControl_FontRotate_Hour.checkBox_Use.Checked)
            {
                NumericUpDown numericUpDownX = userControl_FontRotate_Hour.numericUpDown_FontRotateX;
                NumericUpDown numericUpDownY = userControl_FontRotate_Hour.numericUpDown_FontRotateY;
                NumericUpDown numericUpDown_size = userControl_FontRotate_Hour.numericUpDown_FontRotate_size;
                NumericUpDown numericUpDown_angle = userControl_FontRotate_Hour.numericUpDown_FontRotate_angle;
                NumericUpDown numericUpDown_radius = userControl_FontRotate_Hour.numericUpDown_FontRotate_radius;
                NumericUpDown numericUpDown_spacing = userControl_FontRotate_Hour.numericUpDown_FontRotate_spacing;
                bool follow = userControl_FontRotate_Hour.checkBox_follow.Checked;
                bool add_zero = userControl_FontRotate_Hour.checkBox_addZero.Checked;
                bool separator = userControl_FontRotate_Hour.checkBox_separator.Checked;

                if (digits == null) digits = new List<DigitalTimeDigit>();
                DigitalTimeDigit digitalCommonDigit = new DigitalTimeDigit();
                digitalCommonDigit.TimeType = "Hour";
                if (!follow) digitalCommonDigit.CombingMode = "Single";
                if (separator)
                {
                    digitalCommonDigit.Separator = new ImageCoord();
                    digitalCommonDigit.Separator.Coordinates = new Coordinates();
                    digitalCommonDigit.Separator.Coordinates.X = -1;
                    digitalCommonDigit.Separator.Coordinates.Y = -1;
                }

                if (digitalCommonDigit.Digit == null) digitalCommonDigit.Digit = new Text();
                if (digitalCommonDigit.Digit.SystemFont == null)
                    digitalCommonDigit.Digit.SystemFont = new SystemFont();
                if (digitalCommonDigit.Digit.SystemFont.FontRotate == null)
                    digitalCommonDigit.Digit.SystemFont.FontRotate = new FontRotate();

                digitalCommonDigit.Digit.PaddingZero = add_zero;
                digitalCommonDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;

                digitalCommonDigit.Digit.SystemFont.FontRotate.X = (long)numericUpDownX.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.Y = (long)numericUpDownY.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.Radius = (long)numericUpDown_radius.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.RotateDirection =
                    userControl_FontRotate_Hour.radioButtonGetRotateDirection();
                digitalCommonDigit.Digit.SystemFont.Size = (long)numericUpDown_size.Value;
                digitalCommonDigit.Digit.SystemFont.Angle = (long)numericUpDown_angle.Value;

                digitalCommonDigit.Digit.SystemFont.ShowUnitCheck = userControl_FontRotate_Hour.checkBoxGetUnit() + 1;
                digitalCommonDigit.Digit.SystemFont.Color = userControl_FontRotate_Hour.comboBoxGetColorString();

                digits.Add(digitalCommonDigit);
            }

            // данные системным шрифтом по окружности минуты
            if (userControl_FontRotate_Minute != null && userControl_FontRotate_Minute.checkBox_Use.Checked)
            {
                NumericUpDown numericUpDownX = userControl_FontRotate_Minute.numericUpDown_FontRotateX;
                NumericUpDown numericUpDownY = userControl_FontRotate_Minute.numericUpDown_FontRotateY;
                NumericUpDown numericUpDown_size = userControl_FontRotate_Minute.numericUpDown_FontRotate_size;
                NumericUpDown numericUpDown_angle = userControl_FontRotate_Minute.numericUpDown_FontRotate_angle;
                NumericUpDown numericUpDown_radius = userControl_FontRotate_Minute.numericUpDown_FontRotate_radius;
                NumericUpDown numericUpDown_spacing = userControl_FontRotate_Minute.numericUpDown_FontRotate_spacing;
                bool follow = userControl_FontRotate_Minute.checkBox_follow.Checked;
                //follow_min = follow;
                bool add_zero = userControl_FontRotate_Minute.checkBox_addZero.Checked;
                bool separator = userControl_FontRotate_Minute.checkBox_separator.Checked;

                if (digits == null) digits = new List<DigitalTimeDigit>();
                DigitalTimeDigit digitalCommonDigit = new DigitalTimeDigit();
                digitalCommonDigit.TimeType = "Minute";
                if (!follow) digitalCommonDigit.CombingMode = "Single";
                if (separator)
                {
                    digitalCommonDigit.Separator = new ImageCoord();
                    digitalCommonDigit.Separator.Coordinates = new Coordinates();
                    digitalCommonDigit.Separator.Coordinates.X = -1;
                    digitalCommonDigit.Separator.Coordinates.Y = -1;
                }

                if (digitalCommonDigit.Digit == null) digitalCommonDigit.Digit = new Text();
                if (digitalCommonDigit.Digit.SystemFont == null)
                    digitalCommonDigit.Digit.SystemFont = new SystemFont();
                if (digitalCommonDigit.Digit.SystemFont.FontRotate == null)
                    digitalCommonDigit.Digit.SystemFont.FontRotate = new FontRotate();

                digitalCommonDigit.Digit.PaddingZero = add_zero;
                digitalCommonDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;

                digitalCommonDigit.Digit.SystemFont.FontRotate.X = (long)numericUpDownX.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.Y = (long)numericUpDownY.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.Radius = (long)numericUpDown_radius.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.RotateDirection =
                    userControl_FontRotate_Minute.radioButtonGetRotateDirection();
                digitalCommonDigit.Digit.SystemFont.Size = (long)numericUpDown_size.Value;
                digitalCommonDigit.Digit.SystemFont.Angle = (long)numericUpDown_angle.Value;

                digitalCommonDigit.Digit.SystemFont.ShowUnitCheck = userControl_FontRotate_Minute.checkBoxGetUnit() + 1;
                digitalCommonDigit.Digit.SystemFont.Color = userControl_FontRotate_Minute.comboBoxGetColorString();

                digits.Add(digitalCommonDigit);
            }

            // данные системным шрифтом по окружности секунды
            if (userControl_FontRotate_Second != null && userControl_FontRotate_Second.checkBox_Use.Checked)
            {
                NumericUpDown numericUpDownX = userControl_FontRotate_Second.numericUpDown_FontRotateX;
                NumericUpDown numericUpDownY = userControl_FontRotate_Second.numericUpDown_FontRotateY;
                NumericUpDown numericUpDown_size = userControl_FontRotate_Second.numericUpDown_FontRotate_size;
                NumericUpDown numericUpDown_angle = userControl_FontRotate_Second.numericUpDown_FontRotate_angle;
                NumericUpDown numericUpDown_radius = userControl_FontRotate_Second.numericUpDown_FontRotate_radius;
                NumericUpDown numericUpDown_spacing = userControl_FontRotate_Second.numericUpDown_FontRotate_spacing;
                bool follow = userControl_FontRotate_Second.checkBox_follow.Checked;
                //follow_sec = follow;
                bool add_zero = userControl_FontRotate_Second.checkBox_addZero.Checked;
                bool separator = userControl_FontRotate_Second.checkBox_separator.Checked;

                if (digits == null) digits = new List<DigitalTimeDigit>();
                DigitalTimeDigit digitalCommonDigit = new DigitalTimeDigit();
                digitalCommonDigit.TimeType = "Second";
                if (!follow) digitalCommonDigit.CombingMode = "Single";
                if (separator)
                {
                    digitalCommonDigit.Separator = new ImageCoord();
                    digitalCommonDigit.Separator.Coordinates = new Coordinates();
                    digitalCommonDigit.Separator.Coordinates.X = -1;
                    digitalCommonDigit.Separator.Coordinates.Y = -1;
                }

                if (digitalCommonDigit.Digit == null) digitalCommonDigit.Digit = new Text();
                if (digitalCommonDigit.Digit.SystemFont == null)
                    digitalCommonDigit.Digit.SystemFont = new SystemFont();
                if (digitalCommonDigit.Digit.SystemFont.FontRotate == null)
                    digitalCommonDigit.Digit.SystemFont.FontRotate = new FontRotate();

                digitalCommonDigit.Digit.PaddingZero = add_zero;
                digitalCommonDigit.Digit.Spacing = (long)numericUpDown_spacing.Value;

                digitalCommonDigit.Digit.SystemFont.FontRotate.X = (long)numericUpDownX.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.Y = (long)numericUpDownY.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.Radius = (long)numericUpDown_radius.Value;
                digitalCommonDigit.Digit.SystemFont.FontRotate.RotateDirection =
                    userControl_FontRotate_Second.radioButtonGetRotateDirection();
                digitalCommonDigit.Digit.SystemFont.Size = (long)numericUpDown_size.Value;
                digitalCommonDigit.Digit.SystemFont.Angle = (long)numericUpDown_angle.Value;

                digitalCommonDigit.Digit.SystemFont.ShowUnitCheck = userControl_FontRotate_Second.checkBoxGetUnit() + 1;
                digitalCommonDigit.Digit.SystemFont.Color = userControl_FontRotate_Second.comboBoxGetColorString();

                digits.Add(digitalCommonDigit);
            }


            if (digits != null)
            {
                if (ScreenIdle.DialFace == null) ScreenIdle.DialFace = new ScreenNormal();
                if (ScreenIdle.DialFace.DigitalDialFace == null) ScreenIdle.DialFace.DigitalDialFace = new DigitalDialFace();
                if (ScreenIdle.DialFace.DigitalDialFace.Digits == null)
                    ScreenIdle.DialFace.DigitalDialFace.Digits = new List<DigitalTimeDigit>();
                ScreenIdle.DialFace.DigitalDialFace.Digits.AddRange(digits);
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
            userControl_segments_Battery_AOD.ComboBoxAddItems(ListImages);
            userControl_text_Battery_AOD.ComboBoxAddItems(ListImages);
            userControl_hand_Battery_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_Battery_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_Battery_AOD.ComboBoxAddItems(ListImages);
            userControl_icon_Battery_AOD.ComboBoxAddItems(ListImages);
            progressBar1.Value = 65;

            userControl_pictures_Steps_AOD.ComboBoxAddItems(ListImages);
            userControl_segments_Steps_AOD.ComboBoxAddItems(ListImages);
            userControl_text_Steps_AOD.ComboBoxAddItems(ListImages);
            userControl_text_goal_Steps_AOD.ComboBoxAddItems(ListImages);
            userControl_hand_Steps_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_Steps_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_Steps_AOD.ComboBoxAddItems(ListImages);
            userControl_icon_Steps_AOD.ComboBoxAddItems(ListImages);

            userControl_pictures_Calories_AOD.ComboBoxAddItems(ListImages);
            userControl_segments_Calories_AOD.ComboBoxAddItems(ListImages);
            userControl_text_Calories_AOD.ComboBoxAddItems(ListImages);
            userControl_hand_Calories_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_Calories_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_Calories_AOD.ComboBoxAddItems(ListImages);
            userControl_icon_Calories_AOD.ComboBoxAddItems(ListImages);
            progressBar1.Value = 70;

            userControl_pictures_HeartRate_AOD.ComboBoxAddItems(ListImages);
            userControl_segments_HeartRate_AOD.ComboBoxAddItems(ListImages);
            userControl_text_HeartRate_AOD.ComboBoxAddItems(ListImages);
            userControl_hand_HeartRate_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_HeartRate_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_HeartRate_AOD.ComboBoxAddItems(ListImages);
            userControl_icon_HeartRate_AOD.ComboBoxAddItems(ListImages);

            userControl_pictures_PAI_AOD.ComboBoxAddItems(ListImages);
            userControl_segments_PAI_AOD.ComboBoxAddItems(ListImages);
            userControl_text_PAI_AOD.ComboBoxAddItems(ListImages);
            userControl_hand_PAI_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_PAI_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_PAI_AOD.ComboBoxAddItems(ListImages);
            userControl_icon_PAI_AOD.ComboBoxAddItems(ListImages);
            progressBar1.Value = 75;

            userControl_text_Distance_AOD.ComboBoxAddItems(ListImages);
            userControl_icon_Distance_AOD.ComboBoxAddItems(ListImages);

            userControl_pictures_StandUp_AOD.ComboBoxAddItems(ListImages);
            userControl_segments_StandUp_AOD.ComboBoxAddItems(ListImages);
            userControl_text_StandUp_AOD.ComboBoxAddItems(ListImages);
            userControl_text_goal_StandUp_AOD.ComboBoxAddItems(ListImages);
            userControl_hand_StandUp_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_StandUp_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_StandUp_AOD.ComboBoxAddItems(ListImages);
            userControl_icon_StandUp_AOD.ComboBoxAddItems(ListImages);

            userControl_pictures_weather_AOD.ComboBoxAddItems(ListImages);
            userControl_text_weather_Current_AOD.ComboBoxAddItems(ListImages);
            userControl_text_weather_Min_AOD.ComboBoxAddItems(ListImages);
            userControl_text_weather_Max_AOD.ComboBoxAddItems(ListImages);
            //userControl_hand_Weather_AOD.ComboBoxAddItems(ListImages);
            //userControl_scaleCircle_Weather_AOD.ComboBoxAddItems(ListImages);
            //userControl_scaleLinear_Weather_AOD.ComboBoxAddItems(ListImages);
            userControl_icon_Weather_AOD.ComboBoxAddItems(ListImages);
            progressBar1.Value = 80;

            userControl_pictures_UVindex_AOD.ComboBoxAddItems(ListImages);
            userControl_segments_UVindex_AOD.ComboBoxAddItems(ListImages);
            userControl_text_UVindex_AOD.ComboBoxAddItems(ListImages);
            userControl_hand_UVindex_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_UVindex_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_UVindex_AOD.ComboBoxAddItems(ListImages);
            userControl_icon_UVindex_AOD.ComboBoxAddItems(ListImages);

            //userControl_pictures_AirQuality_AOD.ComboBoxAddItems(ListImages);
            //userControl_segments_AirQuality_AOD.ComboBoxAddItems(ListImages);
            //userControl_text_AirQuality_AOD.ComboBoxAddItems(ListImages);
            //userControl_hand_AirQuality_AOD.ComboBoxAddItems(ListImages);
            //userControl_scaleCircle_AirQuality_AOD.ComboBoxAddItems(ListImages);
            //userControl_scaleLinear_AirQuality_AOD.ComboBoxAddItems(ListImages);
            //userControl_icon_AirQuality_AOD.ComboBoxAddItems(ListImages);
            progressBar1.Value = 85;

            userControl_pictures_Humidity_AOD.ComboBoxAddItems(ListImages);
            userControl_segments_Humidity_AOD.ComboBoxAddItems(ListImages);
            userControl_text_Humidity_AOD.ComboBoxAddItems(ListImages);
            userControl_hand_Humidity_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_Humidity_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_Humidity_AOD.ComboBoxAddItems(ListImages);
            userControl_icon_Humidity_AOD.ComboBoxAddItems(ListImages);

            userControl_pictures_Sunrise_AOD.ComboBoxAddItems(ListImages);
            userControl_segments_Sunrise_AOD.ComboBoxAddItems(ListImages);
            userControl_text_SunriseSunset_AOD.ComboBoxAddItems(ListImages);
            userControl_text_Sunrise_AOD.ComboBoxAddItems(ListImages);
            userControl_text_Sunset_AOD.ComboBoxAddItems(ListImages);
            userControl_hand_Sunrise_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_Sunrise_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_Sunrise_AOD.ComboBoxAddItems(ListImages);
            userControl_icon_Sunrise_AOD.ComboBoxAddItems(ListImages);

            userControl_pictures_WindForce_AOD.ComboBoxAddItems(ListImages);
            userControl_segments_WindForce_AOD.ComboBoxAddItems(ListImages);
            userControl_text_WindForce_AOD.ComboBoxAddItems(ListImages);
            userControl_hand_WindForce_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_WindForce_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_WindForce_AOD.ComboBoxAddItems(ListImages);
            userControl_icon_WindForce_AOD.ComboBoxAddItems(ListImages);
            progressBar1.Value = 90;

            //userControl_pictures_Altitude_AOD.ComboBoxAddItems(ListImages);
            //userControl_segments_Altitude_AOD.ComboBoxAddItems(ListImages);
            //userControl_text_Altitude_AOD.ComboBoxAddItems(ListImages);
            //userControl_hand_Altitude_AOD.ComboBoxAddItems(ListImages);
            //userControl_scaleCircle_Altitude_AOD.ComboBoxAddItems(ListImages);
            //userControl_scaleLinear_Altitude_AOD.ComboBoxAddItems(ListImages);
            //userControl_icon_Altitude_AOD.ComboBoxAddItems(ListImages);

            userControl_pictures_AirPressure_AOD.ComboBoxAddItems(ListImages);
            userControl_segments_AirPressure_AOD.ComboBoxAddItems(ListImages);
            userControl_text_AirPressure_AOD.ComboBoxAddItems(ListImages);
            userControl_hand_AirPressure_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_AirPressure_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_AirPressure_AOD.ComboBoxAddItems(ListImages);
            userControl_icon_AirPressure_AOD.ComboBoxAddItems(ListImages);
            progressBar1.Value = 95;

            //userControl_pictures_Stress_AOD.ComboBoxAddItems(ListImages);
            //userControl_segments_Stress_AOD.ComboBoxAddItems(ListImages);
            //userControl_text_Stress_AOD.ComboBoxAddItems(ListImages);
            //userControl_hand_Stress_AOD.ComboBoxAddItems(ListImages);
            //userControl_scaleCircle_Stress_AOD.ComboBoxAddItems(ListImages);
            //userControl_scaleLinear_Stress_AOD.ComboBoxAddItems(ListImages);
            //userControl_icon_Stress_AOD.ComboBoxAddItems(ListImages);

            userControl_pictures_ActivityGoal_AOD.ComboBoxAddItems(ListImages);
            userControl_segments_ActivityGoal_AOD.ComboBoxAddItems(ListImages);
            userControl_text_ActivityGoal_AOD.ComboBoxAddItems(ListImages);
            userControl_text_goal_ActivityGoal_AOD.ComboBoxAddItems(ListImages);
            userControl_hand_ActivityGoal_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_ActivityGoal_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_ActivityGoal_AOD.ComboBoxAddItems(ListImages);
            userControl_icon_ActivityGoal_AOD.ComboBoxAddItems(ListImages);
            progressBar1.Value = 100;

            userControl_pictures_FatBurning_AOD.ComboBoxAddItems(ListImages);
            userControl_segments_FatBurning_AOD.ComboBoxAddItems(ListImages);
            userControl_text_FatBurning_AOD.ComboBoxAddItems(ListImages);
            userControl_hand_FatBurning_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_FatBurning_AOD.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_FatBurning_AOD.ComboBoxAddItems(ListImages);
            userControl_icon_FatBurning_AOD.ComboBoxAddItems(ListImages);
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

            userControl_SystemFont_GroupTime.SettingsClear();


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
            userControl_SystemFont_Group_Day_AOD.SettingsClear();

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
            userControl_SystemFont_Group_Month_AOD.SettingsClear();

            comboBox_Year_image_AOD.Items.Clear();
            comboBox_Year_image_AOD.Text = "";
            comboBox_Year_unit_AOD.Items.Clear();
            comboBox_Year_unit_AOD.Text = "";
            comboBox_Year_separator_AOD.Items.Clear();
            comboBox_Year_separator_AOD.Text = "";
            userControl_SystemFont_Group_Year_AOD.SettingsClear();

            comboBox_DOW_pictures_image_AOD.Items.Clear();
            comboBox_DOW_pictures_image_AOD.Text = "";
            comboBox_DOW_hand_image_AOD.Items.Clear();
            comboBox_DOW_hand_image_AOD.Text = "";
            comboBox_DOW_hand_imageCentr_AOD.Items.Clear();
            comboBox_DOW_hand_imageCentr_AOD.Text = "";
            comboBox_DOW_hand_imageBackground_AOD.Items.Clear();
            comboBox_DOW_hand_imageBackground_AOD.Text = "";

            userControl_pictures_Battery_AOD.SettingsClear();
            userControl_segments_Battery_AOD.SettingsClear();
            userControl_text_Battery_AOD.SettingsClear();
            userControl_hand_Battery_AOD.SettingsClear();
            userControl_scaleCircle_Battery_AOD.SettingsClear();
            userControl_scaleLinear_Battery_AOD.SettingsClear();
            userControl_SystemFont_Group_Battery_AOD.SettingsClear();
            userControl_icon_Battery_AOD.SettingsClear();

            userControl_pictures_Steps_AOD.SettingsClear();
            userControl_segments_Steps_AOD.SettingsClear();
            userControl_text_Steps_AOD.SettingsClear();
            userControl_text_goal_Steps_AOD.SettingsClear();
            userControl_hand_Steps_AOD.SettingsClear();
            userControl_scaleCircle_Steps_AOD.SettingsClear();
            userControl_scaleLinear_Steps_AOD.SettingsClear();
            userControl_SystemFont_Group_Steps_AOD.SettingsClear();
            userControl_icon_Steps_AOD.SettingsClear();

            userControl_pictures_Calories_AOD.SettingsClear();
            userControl_segments_Calories_AOD.SettingsClear();
            userControl_text_Calories_AOD.SettingsClear();
            userControl_text_goal_Calories_AOD.SettingsClear();
            userControl_hand_Calories_AOD.SettingsClear();
            userControl_scaleCircle_Calories_AOD.SettingsClear();
            userControl_scaleLinear_Calories_AOD.SettingsClear();
            userControl_SystemFont_Group_Calories_AOD.SettingsClear();
            userControl_icon_Calories_AOD.SettingsClear();

            userControl_pictures_HeartRate_AOD.SettingsClear();
            userControl_segments_HeartRate_AOD.SettingsClear();
            userControl_text_HeartRate_AOD.SettingsClear();
            userControl_hand_HeartRate_AOD.SettingsClear();
            userControl_scaleCircle_HeartRate_AOD.SettingsClear();
            userControl_scaleLinear_HeartRate_AOD.SettingsClear();
            userControl_SystemFont_Group_HeartRate_AOD.SettingsClear();
            userControl_icon_HeartRate_AOD.SettingsClear();

            userControl_pictures_PAI_AOD.SettingsClear();
            userControl_segments_PAI_AOD.SettingsClear();
            userControl_text_PAI_AOD.SettingsClear();
            userControl_hand_PAI_AOD.SettingsClear();
            userControl_scaleCircle_PAI_AOD.SettingsClear();
            userControl_scaleLinear_PAI_AOD.SettingsClear();
            userControl_SystemFont_Group_PAI_AOD.SettingsClear();
            userControl_icon_PAI_AOD.SettingsClear();


            userControl_text_Distance_AOD.SettingsClear();
            userControl_SystemFont_Group_Distance_AOD.SettingsClear();
            userControl_icon_Distance_AOD.SettingsClear();


            userControl_pictures_StandUp_AOD.SettingsClear();
            userControl_segments_StandUp_AOD.SettingsClear();
            userControl_text_StandUp_AOD.SettingsClear();
            userControl_text_goal_StandUp_AOD.SettingsClear();
            userControl_hand_StandUp_AOD.SettingsClear();
            userControl_scaleCircle_StandUp_AOD.SettingsClear();
            userControl_scaleLinear_StandUp_AOD.SettingsClear();
            userControl_SystemFont_Group_StandUp_AOD.SettingsClear();
            userControl_icon_StandUp_AOD.SettingsClear();

            userControl_pictures_weather_AOD.SettingsClear();
            userControl_text_weather_Current_AOD.SettingsClear();
            userControl_text_weather_Min_AOD.SettingsClear();
            userControl_text_weather_Max_AOD.SettingsClear();
            //userControl_hand_Weather_AOD.SettingsClear();
            //userControl_scaleCircle_Weather_AOD.SettingsClear();
            //userControl_scaleLinear_Weather_AOD.SettingsClear();
            userControl_SystemFont_GroupWeather_AOD.SettingsClear();
            userControl_icon_Weather_AOD.SettingsClear();

            userControl_pictures_UVindex_AOD.SettingsClear();
            userControl_segments_UVindex_AOD.SettingsClear();
            userControl_text_UVindex_AOD.SettingsClear();
            userControl_hand_UVindex_AOD.SettingsClear();
            userControl_scaleCircle_UVindex_AOD.SettingsClear();
            userControl_scaleLinear_UVindex_AOD.SettingsClear();
            userControl_SystemFont_Group_UVindex_AOD.SettingsClear();
            userControl_icon_UVindex_AOD.SettingsClear();

            //userControl_pictures_AirQuality_AOD.SettingsClear();
            //userControl_segments_AirQuality_AOD.SettingsClear();
            //userControl_text_AirQuality_AOD.SettingsClear();
            //userControl_hand_AirQuality_AOD.SettingsClear();
            //userControl_scaleCircle_AirQuality_AOD.SettingsClear();
            //userControl_scaleLinear_AirQuality_AOD.SettingsClear();
            //userControl_SystemFont_AirQuality_AOD.SettingsClear();
            //userControl_FontRotate_AirQuality_AOD.SettingsClear();
            //userControl_icon_AirQuality_AOD.SettingsClear();

            userControl_pictures_Humidity_AOD.SettingsClear();
            userControl_segments_Humidity_AOD.SettingsClear();
            userControl_text_Humidity_AOD.SettingsClear();
            userControl_hand_Humidity_AOD.SettingsClear();
            userControl_scaleCircle_Humidity_AOD.SettingsClear();
            userControl_scaleLinear_Humidity_AOD.SettingsClear();
            userControl_SystemFont_Group_Humidity_AOD.SettingsClear();
            userControl_icon_Humidity_AOD.SettingsClear();

            userControl_pictures_Sunrise_AOD.SettingsClear();
            userControl_segments_Sunrise_AOD.SettingsClear();
            userControl_text_SunriseSunset_AOD.SettingsClear();
            userControl_text_Sunrise_AOD.SettingsClear();
            userControl_text_Sunset_AOD.SettingsClear();
            userControl_hand_Sunrise_AOD.SettingsClear();
            userControl_scaleCircle_Sunrise_AOD.SettingsClear();
            userControl_scaleLinear_Sunrise_AOD.SettingsClear();
            userControl_SystemFont_GroupSunrise_AOD.SettingsClear();
            userControl_icon_Sunrise_AOD.SettingsClear();


            userControl_hand_Sunrise_AOD.SettingsClear();
            userControl_scaleCircle_Sunrise_AOD.SettingsClear();
            userControl_scaleLinear_Sunrise_AOD.SettingsClear();
            userControl_SystemFont_GroupSunrise_AOD.SettingsClear();
            userControl_icon_Sunrise_AOD.SettingsClear();

            userControl_pictures_WindForce_AOD.SettingsClear();
            userControl_segments_WindForce_AOD.SettingsClear();
            userControl_text_WindForce_AOD.SettingsClear();
            userControl_hand_WindForce_AOD.SettingsClear();
            userControl_scaleCircle_WindForce_AOD.SettingsClear();
            userControl_scaleLinear_WindForce_AOD.SettingsClear();
            userControl_SystemFont_Group_WindForce_AOD.SettingsClear();
            userControl_icon_WindForce_AOD.SettingsClear();

            //userControl_pictures_Altitude_AOD.SettingsClear();
            //userControl_segments_Altitude_AOD.SettingsClear();
            //userControl_text_Altitude_AOD.SettingsClear();
            //userControl_hand_Altitude_AOD.SettingsClear();
            //userControl_scaleCircle_Altitude_AOD.SettingsClear();
            //userControl_scaleLinear_Altitude_AOD.SettingsClear();
            //userControl_SystemFont_Altitude_AOD.SettingsClear();
            //userControl_FontRotate_Altitude_AOD.SettingsClear();
            //userControl_icon_Altitude_AOD.SettingsClear();

            userControl_pictures_AirPressure_AOD.SettingsClear();
            userControl_segments_AirPressure_AOD.SettingsClear();
            userControl_text_AirPressure_AOD.SettingsClear();
            userControl_hand_AirPressure_AOD.SettingsClear();
            userControl_scaleCircle_AirPressure_AOD.SettingsClear();
            userControl_scaleLinear_AirPressure_AOD.SettingsClear();
            userControl_SystemFont_Group_AirPressure_AOD.SettingsClear();
            userControl_icon_AirPressure_AOD.SettingsClear();

            //userControl_pictures_Stress_AOD.SettingsClear();
            //userControl_segments_Stress_AOD.SettingsClear();
            //userControl_text_Stress_AOD.SettingsClear();
            //userControl_hand_Stress_AOD.SettingsClear();
            //userControl_scaleCircle_Stress_AOD.SettingsClear();
            //userControl_scaleLinear_Stress_AOD.SettingsClear();
            //userControl_SystemFont_Stress_AOD.SettingsClear();
            //userControl_FontRotate_Stress_AOD.SettingsClear();
            //userControl_icon_Stress_AOD.SettingsClear();

            userControl_pictures_ActivityGoal_AOD.SettingsClear();
            userControl_segments_ActivityGoal_AOD.SettingsClear();
            userControl_text_ActivityGoal_AOD.SettingsClear();
            userControl_text_goal_ActivityGoal_AOD.SettingsClear();
            userControl_hand_ActivityGoal_AOD.SettingsClear();
            userControl_scaleCircle_ActivityGoal_AOD.SettingsClear();
            userControl_scaleLinear_ActivityGoal_AOD.SettingsClear();
            userControl_SystemFont_Group_ActivityGoal_AOD.SettingsClear();
            userControl_icon_ActivityGoal_AOD.SettingsClear();

            userControl_pictures_FatBurning_AOD.SettingsClear();
            userControl_segments_FatBurning_AOD.SettingsClear();
            userControl_text_FatBurning_AOD.SettingsClear();
            userControl_text_goal_FatBurning_AOD.SettingsClear();
            userControl_hand_FatBurning_AOD.SettingsClear();
            userControl_scaleCircle_FatBurning_AOD.SettingsClear();
            userControl_scaleLinear_FatBurning_AOD.SettingsClear();
            userControl_SystemFont_Group_FatBurning_AOD.SettingsClear();
            userControl_icon_FatBurning_AOD.SettingsClear();
            foreach (DataGridViewRow dgvr in dataGridView_AODL_Activity.Rows)
            {
                dgvr.Visible = false;
            }
            foreach (DataGridViewRow dgvr in dataGridView_AODL_Date.Rows)
            {
                dgvr.Visible = false;
            }

        }
    }
}
