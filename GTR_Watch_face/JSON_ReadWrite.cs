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
            progressBar1.Width = 500;
            progressBar1.Value = 0;
            progressBar1.Visible = true;
            SettingsClear();
            checkBoxUseClear();
            ComboBoxAddItems();

            SettingsClear_AOD();
            checkBoxUseClear_AOD();
            ComboBoxAddItems_AOD();
            //progressBar1.Visible = false;

            SettingsClear_Widgets();
            ComboBoxAddItems_Widgets();

            WidgetsTemp = null;

            if (Watch_Face == null)
            {
                progressBar1.Visible = false;
                return;
            }

            if (Watch_Face.Info != null) ReadDeviceId();
            #region Background
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
                                if (digitalTimeDigit.Digit != null)
                                {
                                    if (digitalTimeDigit.Digit.Image != null)
                                    {
                                        checkBox_Minute_Use.Checked = true;
                                        if (digitalTimeDigit.CombingMode == "Single")
                                        {
                                            checkBox_Minute_follow.Checked = false;
                                        }
                                        else
                                        {
                                            checkBox_Minute_follow.Checked = true;
                                        }

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
                                        AlignmentToString(comboBox_Minute_alignment, digitalTimeDigit.Digit.Alignment);
                                        if (digitalTimeDigit.Digit.Spacing != null)
                                            numericUpDown_Minute_spacing.Value = (decimal)digitalTimeDigit.Digit.Spacing;
                                        checkBox_Minute_add_zero.Checked = digitalTimeDigit.Digit.PaddingZero;

                                        if (digitalTimeDigit.Separator != null)
                                        {
                                            comboBoxSetText(comboBox_Minute_unit, digitalTimeDigit.Separator.ImageIndex);
                                            numericUpDown_Minute_unitX.Value = digitalTimeDigit.Separator.Coordinates.X;
                                            numericUpDown_Minute_unitY.Value = digitalTimeDigit.Separator.Coordinates.Y;
                                        }
                                    }
                                    else if(digitalTimeDigit.Digit.SystemFont != null)
                                    {
                                        UserControl_SystemFont_weather userControl_SystemFont_Time = userControl_SystemFont_GroupTime.userControl_SystemFont_weather_Min;
                                        UserControl_FontRotate_weather userControl_FontRotate_Time = userControl_SystemFont_GroupTime.userControl_FontRotate_weather_Min;

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
                                            userControl_SystemFont_Time.checkBoxSetUnit((int)digitalTimeDigit.Digit.SystemFont.ShowUnitCheck-1);
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

                            case "Second":
                                
                                if (digitalTimeDigit.Digit != null)
                                {
                                    if (digitalTimeDigit.Digit.Image != null)
                                    {
                                        checkBox_Second_Use.Checked = true;
                                        if (digitalTimeDigit.CombingMode == "Single")
                                        {
                                            checkBox_Second_follow.Checked = false;
                                        }
                                        else
                                        {
                                            checkBox_Second_follow.Checked = true;
                                        }

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
                                        AlignmentToString(comboBox_Second_alignment, digitalTimeDigit.Digit.Alignment);
                                        if (digitalTimeDigit.Digit.Spacing != null)
                                            numericUpDown_Second_spacing.Value = (decimal)digitalTimeDigit.Digit.Spacing;
                                        checkBox_Second_add_zero.Checked = digitalTimeDigit.Digit.PaddingZero;

                                        if (digitalTimeDigit.Separator != null)
                                        {
                                            comboBoxSetText(comboBox_Second_unit, digitalTimeDigit.Separator.ImageIndex);
                                            numericUpDown_Second_unitX.Value = digitalTimeDigit.Separator.Coordinates.X;
                                            numericUpDown_Second_unitY.Value = digitalTimeDigit.Separator.Coordinates.Y;
                                        }
                                    }
                                    else if (digitalTimeDigit.Digit.SystemFont != null)
                                    {
                                        UserControl_SystemFont_weather userControl_SystemFont_Time = userControl_SystemFont_GroupTime.userControl_SystemFont_weather_Max;
                                        UserControl_FontRotate_weather userControl_FontRotate_Time = userControl_SystemFont_GroupTime.userControl_FontRotate_weather_Max;

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
                                        checkBox_Hour_Use.Checked = true;

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
                                        AlignmentToString(comboBox_Hour_alignment, digitalTimeDigit.Digit.Alignment);
                                        if (digitalTimeDigit.Digit.Spacing != null)
                                            numericUpDown_Hour_spacing.Value = (decimal)digitalTimeDigit.Digit.Spacing;
                                        checkBox_Hour_add_zero.Checked = digitalTimeDigit.Digit.PaddingZero;

                                        if (digitalTimeDigit.Separator != null)
                                        {
                                            comboBoxSetText(comboBox_Hour_unit, digitalTimeDigit.Separator.ImageIndex);
                                            numericUpDown_Hour_unitX.Value = digitalTimeDigit.Separator.Coordinates.X;
                                            numericUpDown_Hour_unitY.Value = digitalTimeDigit.Separator.Coordinates.Y;
                                        }
                                    }
                                    else if (digitalTimeDigit.Digit.SystemFont != null)
                                    {
                                        UserControl_SystemFont_weather userControl_SystemFont_Time = userControl_SystemFont_GroupTime.userControl_SystemFont_weather_Current;
                                        UserControl_FontRotate_weather userControl_FontRotate_Time = userControl_SystemFont_GroupTime.userControl_FontRotate_weather_Current;

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
                                
                                // надпись
                                if (digitalDateDigit.Digit != null && digitalDateDigit.Digit.Image != null)
                                {
                                    checkBox_Day_Use.Checked = true;
                                    if (digitalDateDigit.CombingMode == "Single")
                                    {
                                        checkBox_Day_follow.Checked = false;
                                    }
                                    else
                                    {
                                        checkBox_Day_follow.Checked = true;
                                    }

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

                                    AlignmentToString(comboBox_Day_alignment, digitalDateDigit.Digit.Alignment);
                                    if (digitalDateDigit.Digit.Spacing != null)
                                        numericUpDown_Day_spacing.Value = (decimal)digitalDateDigit.Digit.Spacing;
                                    checkBox_Day_add_zero.Checked = digitalDateDigit.Digit.PaddingZero;
                                    
                                    if (digitalDateDigit.Separator != null)
                                    {
                                        comboBoxSetText(comboBox_Day_unit, digitalDateDigit.Separator.ImageIndex);
                                        numericUpDown_Day_unitX.Value = digitalDateDigit.Separator.Coordinates.X;
                                        numericUpDown_Day_unitY.Value = digitalDateDigit.Separator.Coordinates.Y;
                                    }
                                }

                                // системный шрифт
                                if (digitalDateDigit.Digit != null && digitalDateDigit.Digit.SystemFont != null &&
                                    digitalDateDigit.Digit.SystemFont.FontRotate == null)
                                {
                                    UserControl_SystemFont userControl_SystemFont_Date = 
                                        userControl_SystemFont_Group_Day.userControl_SystemFont;
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
                                        userControl_SystemFont_Group_Day.userControl_FontRotate;
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
                                // картинка
                                if (digitalDateDigit.Digit != null && digitalDateDigit.Digit.DisplayFormAnalog)
                                {
                                    if (digitalDateDigit.Digit != null && digitalDateDigit.Digit.Image != null)
                                    {
                                        checkBox_Month_pictures_Use.Checked = true;
                                        numericUpDown_Month_picturesX.Value = digitalDateDigit.Digit.Image.X;
                                        numericUpDown_Month_picturesY.Value = digitalDateDigit.Digit.Image.Y;
                                        foreach (MultilangImage multilangImage in digitalDateDigit.Digit.Image.MultilangImage)
                                        {
                                            if (multilangImage.LangCode == "All")
                                                comboBoxSetText(comboBox_Month_pictures_image, multilangImage.ImageSet.ImageIndex);
                                        }
                                    }
                                }
                                else
                                {
                                    // надпись
                                    if (digitalDateDigit.Digit != null && digitalDateDigit.Digit.Image != null)
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

                                        AlignmentToString(comboBox_Month_alignment, digitalDateDigit.Digit.Alignment);
                                        if (digitalDateDigit.Digit.Spacing != null)
                                            numericUpDown_Month_spacing.Value = (decimal)digitalDateDigit.Digit.Spacing;
                                        checkBox_Month_add_zero.Checked = digitalDateDigit.Digit.PaddingZero;

                                        if (digitalDateDigit.Separator != null)
                                        {
                                            comboBoxSetText(comboBox_Month_unit, digitalDateDigit.Separator.ImageIndex);
                                            numericUpDown_Month_unitX.Value = digitalDateDigit.Separator.Coordinates.X;
                                            numericUpDown_Month_unitY.Value = digitalDateDigit.Separator.Coordinates.Y;
                                        }
                                    }

                                    // системный шрифт
                                    if (digitalDateDigit.Digit != null && digitalDateDigit.Digit.SystemFont != null &&
                                        digitalDateDigit.Digit.SystemFont.FontRotate == null)
                                    {
                                        UserControl_SystemFont userControl_SystemFont_Date = 
                                            userControl_SystemFont_Group_Month.userControl_SystemFont;
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
                                            userControl_SystemFont_Group_Month.userControl_FontRotate;
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
                                    checkBox_Year_text_Use.Checked = true;
                                    if (digitalDateDigit.CombingMode == "Single")
                                    {
                                        checkBox_Year_follow.Checked = false;
                                    }
                                    else
                                    {
                                        checkBox_Year_follow.Checked = true;
                                    }

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

                                    AlignmentToString(comboBox_Year_alignment, digitalDateDigit.Digit.Alignment);
                                    if (digitalDateDigit.Digit.Spacing != null)
                                        numericUpDown_Year_spacing.Value = (decimal)digitalDateDigit.Digit.Spacing;
                                    checkBox_Year_add_zero.Checked = digitalDateDigit.Digit.PaddingZero;

                                    if (digitalDateDigit.Separator != null)
                                    {
                                        comboBoxSetText(comboBox_Year_unit, digitalDateDigit.Separator.ImageIndex);
                                        numericUpDown_Year_unitX.Value = digitalDateDigit.Separator.Coordinates.X;
                                        numericUpDown_Year_unitY.Value = digitalDateDigit.Separator.Coordinates.Y;
                                    }
                                }

                                // системный шрифт
                                if (digitalDateDigit.Digit != null && digitalDateDigit.Digit.SystemFont != null &&
                                    digitalDateDigit.Digit.SystemFont.FontRotate == null)
                                {
                                    UserControl_SystemFont userControl_SystemFont_Date =
                                        userControl_SystemFont_Group_Year.userControl_SystemFont;
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
                                        userControl_SystemFont_Group_Year.userControl_FontRotate;
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
                    if (Watch_Face.System.Date.WeeksDigits.Digit != null)
                    //if (Watch_Face.System.DateAmazfit.WeeksDigits.Digit != null &&
                    //Watch_Face.System.DateAmazfit.WeeksDigits.Digit.DisplayFormAnalog)
                    {
                        if (Watch_Face.System.Date.WeeksDigits.Digit.Image != null)
                        {
                            checkBox_DOW_pictures_Use.Checked = true;
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

                    switch (activity.Type)
                    {
                        case "Battery":
                            userPanel_pictures = userControl_pictures_Battery;
                            userPanel_text = userControl_text_Battery;
                            userPanel_hand = userControl_hand_Battery;
                            userPanel_scaleCircle = userControl_scaleCircle_Battery;
                            userPanel_scaleLinear = userControl_scaleLinear_Battery;
                            userControl_SystemFont_Group = userControl_SystemFont_Group_Battery;
                            userControl_icon = userControl_icon_Battery;
                            break;
                        case "Steps":
                            userPanel_pictures = userControl_pictures_Steps;
                            userPanel_text = userControl_text_Steps;
                            userPanel_textGoal = userControl_text_goal_Steps;
                            userPanel_hand = userControl_hand_Steps;
                            userPanel_scaleCircle = userControl_scaleCircle_Steps;
                            userPanel_scaleLinear = userControl_scaleLinear_Steps;
                            userControl_SystemFont_Group = userControl_SystemFont_Group_Steps;
                            userControl_icon = userControl_icon_Steps;
                            break;
                        case "Calories":
                            userPanel_pictures = userControl_pictures_Calories;
                            userPanel_text = userControl_text_Calories;
                            userPanel_textGoal = userControl_text_goal_Calories;
                            userPanel_hand = userControl_hand_Calories;
                            userPanel_scaleCircle = userControl_scaleCircle_Calories;
                            userPanel_scaleLinear = userControl_scaleLinear_Calories;
                            userControl_SystemFont_Group = userControl_SystemFont_Group_Calories;
                            userControl_icon = userControl_icon_Calories;
                            break;
                        case "HeartRate":
                            userPanel_pictures = userControl_pictures_HeartRate;
                            userPanel_text = userControl_text_HeartRate;
                            userPanel_hand = userControl_hand_HeartRate;
                            userPanel_scaleCircle = userControl_scaleCircle_HeartRate;
                            userPanel_scaleLinear = userControl_scaleLinear_HeartRate;
                            userControl_SystemFont_Group = userControl_SystemFont_Group_HeartRate;
                            userControl_icon = userControl_icon_HeartRate;
                            break;
                        case "PAI":
                            userPanel_pictures = userControl_pictures_PAI;
                            userPanel_text = userControl_text_PAI;
                            userPanel_hand = userControl_hand_PAI;
                            userPanel_scaleCircle = userControl_scaleCircle_PAI;
                            userPanel_scaleLinear = userControl_scaleLinear_PAI;
                            userControl_SystemFont_Group = userControl_SystemFont_Group_PAI;
                            userControl_icon = userControl_icon_PAI;
                            break;
                        case "Distance":
                            userPanel_text = userControl_text_Distance;
                            userControl_SystemFont_Group = userControl_SystemFont_Group_Distance;
                            userControl_icon = userControl_icon_Distance;
                            break;
                        case "StandUp":
                            userPanel_pictures = userControl_pictures_StandUp;
                            userPanel_text = userControl_text_StandUp;
                            userPanel_textGoal = userControl_text_goal_StandUp;
                            userPanel_hand = userControl_hand_StandUp;
                            userPanel_scaleCircle = userControl_scaleCircle_StandUp;
                            userPanel_scaleLinear = userControl_scaleLinear_StandUp;
                            userControl_SystemFont_Group = userControl_SystemFont_Group_StandUp;
                            userControl_icon = userControl_icon_StandUp;
                            break;
                        case "Weather":
                            userPanel_pictures = userControl_pictures_weather;
                            userPanel_text_weather_sunrise = userControl_text_weather_Current;
                            //userPanel_hand = userControl_hand_Weather;
                            //userPanel_scaleCircle = userControl_scaleCircle_Weather;
                            //userPanel_scaleLinear = userControl_scaleLinear_Weather;
                            userControl_icon = userControl_icon_Weather;
                            break;
                        case "UVindex":
                            userPanel_pictures = userControl_pictures_UVindex;
                            userPanel_text = userControl_text_UVindex;
                            userPanel_hand = userControl_hand_UVindex;
                            userPanel_scaleCircle = userControl_scaleCircle_UVindex;
                            userPanel_scaleLinear = userControl_scaleLinear_UVindex;
                            userControl_SystemFont_Group = userControl_SystemFont_Group_UVindex;
                            userControl_icon = userControl_icon_UVindex;
                            break;
                        case "AirQuality":
                            //userPanel_pictures = userControl_pictures_AirQuality;
                            //userPanel_text = userControl_text_AirQuality;
                            //userPanel_hand = userControl_hand_AirQuality;
                            //userPanel_scaleCircle = userControl_scaleCircle_AirQuality;
                            //userPanel_scaleLinear = userControl_scaleLinear_AirQuality;
                            //userControl_SystemFont_Group = userControl_SystemFont_Group_AirQuality;
                            //userControl_icon = userControl_icon_AirQuality;
                            break;
                        case "Humidity":
                            userPanel_pictures = userControl_pictures_Humidity;
                            userPanel_text = userControl_text_Humidity;
                            userPanel_hand = userControl_hand_Humidity;
                            userPanel_scaleCircle = userControl_scaleCircle_Humidity;
                            userPanel_scaleLinear = userControl_scaleLinear_Humidity;
                            userControl_SystemFont_Group = userControl_SystemFont_Group_Humidity;
                            userControl_icon = userControl_icon_Humidity;
                            break;
                        case "Sunrise":
                            userPanel_pictures = userControl_pictures_Sunrise;
                            userPanel_text = userControl_text_SunriseSunset;
                            userPanel_hand = userControl_hand_Sunrise;
                            userPanel_scaleCircle = userControl_scaleCircle_Sunrise;
                            userPanel_scaleLinear = userControl_scaleLinear_Sunrise;
                            userControl_icon = userControl_icon_Sunrise;
                            break;
                        case "WindForce":
                            userPanel_pictures = userControl_pictures_WindForce;
                            userPanel_text = userControl_text_WindForce;
                            userPanel_hand = userControl_hand_WindForce;
                            userPanel_scaleCircle = userControl_scaleCircle_WindForce;
                            userPanel_scaleLinear = userControl_scaleLinear_WindForce;
                            userControl_SystemFont_Group = userControl_SystemFont_Group_WindForce;
                            userControl_icon = userControl_icon_WindForce;
                            break;
                        case "Altitude":
                            //userPanel_pictures = userControl_pictures_Altitude;
                            //userPanel_text = userControl_text_Altitude;
                            //userPanel_hand = userControl_hand_Altitude;
                            //userPanel_scaleCircle = userControl_scaleCircle_Altitude;
                            //userPanel_scaleLinear = userControl_scaleLinear_Altitude;
                            //userControl_SystemFont_Group = userControl_SystemFont_Group_Altitude;
                            //userControl_icon = userControl_icon_Altitude;
                            break;
                        case "AirPressure":
                            userPanel_pictures = userControl_pictures_AirPressure;
                            userPanel_text = userControl_text_AirPressure;
                            userPanel_hand = userControl_hand_AirPressure;
                            userPanel_scaleCircle = userControl_scaleCircle_AirPressure;
                            userPanel_scaleLinear = userControl_scaleLinear_AirPressure;
                            userControl_SystemFont_Group = userControl_SystemFont_Group_AirPressure;
                            userControl_icon = userControl_icon_AirPressure;
                            break;
                        case "Stress":
                            //userPanel_pictures = userControl_pictures_Stress;
                            //userPanel_text = userControl_text_Stress;
                            //userPanel_hand = userControl_hand_Stress;
                            //userPanel_scaleCircle = userControl_scaleCircle_Stress;
                            //userPanel_scaleLinear = userControl_scaleLinear_Stress;
                            //userControl_SystemFont_Group = userControl_SystemFont_Group_Stress;
                            //userControl_icon = userControl_icon_Stress;
                            break;
                        case "ActivityGoal":
                            userPanel_pictures = userControl_pictures_ActivityGoal;
                            userPanel_text = userControl_text_ActivityGoal;
                            userPanel_textGoal = userControl_text_goal_ActivityGoal;
                            userPanel_hand = userControl_hand_ActivityGoal;
                            userPanel_scaleCircle = userControl_scaleCircle_ActivityGoal;
                            userPanel_scaleLinear = userControl_scaleLinear_ActivityGoal;
                            userControl_SystemFont_Group = userControl_SystemFont_Group_ActivityGoal;
                            userControl_icon = userControl_icon_ActivityGoal;
                            break;
                        case "FatBurning":
                            userPanel_pictures = userControl_pictures_FatBurning;
                            userPanel_text = userControl_text_FatBurning;
                            userPanel_textGoal = userControl_text_goal_FatBurning;
                            userPanel_hand = userControl_hand_FatBurning;
                            userPanel_scaleCircle = userControl_scaleCircle_FatBurning;
                            userPanel_scaleLinear = userControl_scaleLinear_FatBurning;
                            userControl_SystemFont_Group = userControl_SystemFont_Group_FatBurning;
                            userControl_icon = userControl_icon_FatBurning;
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

                                if (digitalCommonDigit.Type != null && digitalCommonDigit.Type == "Min" )
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
                                    if(digitalCommonDigit.Separator != null) checkBox_separator.Checked = true;

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
                    else if(activity.Type == "Weather")
                    {
                        UserControl_SystemFont_GroupWeather userControl_SystemFont_Group_Weather = 
                            userControl_SystemFont_GroupWeather;
                        UserControl_SystemFont_weather userControl_SystemFont_weather = null;
                        UserControl_FontRotate_weather userControl_FontRotate_weather = null;

                        if (activity.Digits != null && activity.Digits.Count > 0)
                        {
                            foreach (DigitalCommonDigit digitalCommonDigit in activity.Digits)
                            {
                                if (digitalCommonDigit.Type != null && digitalCommonDigit.Type == "Min")
                                {
                                    userPanel_text_weather_sunrise = userControl_text_weather_Min;
                                    userControl_SystemFont_weather =
                                        userControl_SystemFont_Group_Weather.userControl_SystemFont_weather_Min;
                                    userControl_FontRotate_weather =
                                        userControl_SystemFont_Group_Weather.userControl_FontRotate_weather_Min;
                                }
                                if (digitalCommonDigit.Type != null && digitalCommonDigit.Type == "Max")
                                {
                                    userPanel_text_weather_sunrise = userControl_text_weather_Max;
                                    userControl_SystemFont_weather =
                                        userControl_SystemFont_Group_Weather.userControl_SystemFont_weather_Max;
                                    userControl_FontRotate_weather =
                                        userControl_SystemFont_Group_Weather.userControl_FontRotate_weather_Max;
                                }
                                if (digitalCommonDigit.Type == null)
                                {
                                    userPanel_text_weather_sunrise = userControl_text_weather_Current;
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
                            userControl_SystemFont_GroupSunrise;
                        UserControl_SystemFont_weather userControl_SystemFont_weather = null;
                        UserControl_FontRotate_weather userControl_FontRotate_weather = null;

                        if (activity.Digits != null && activity.Digits.Count > 0)
                        {
                            foreach (DigitalCommonDigit digitalCommonDigit in activity.Digits)
                            {
                                if (digitalCommonDigit.Type != null && digitalCommonDigit.Type == "Min")
                                {
                                    userPanel_text_weather_sunrise = userControl_text_Sunrise;
                                    userControl_SystemFont_weather =
                                        userControl_SystemFont_Group_Weather.userControl_SystemFont_weather_Min;
                                    userControl_FontRotate_weather =
                                        userControl_SystemFont_Group_Weather.userControl_FontRotate_weather_Min;
                                }
                                if (digitalCommonDigit.Type != null && digitalCommonDigit.Type == "Max")
                                {
                                    userPanel_text_weather_sunrise = userControl_text_Sunset;
                                    userControl_SystemFont_weather =
                                        userControl_SystemFont_Group_Weather.userControl_SystemFont_weather_Max;
                                    userControl_FontRotate_weather =
                                        userControl_SystemFont_Group_Weather.userControl_FontRotate_weather_Max;
                                }
                                if (digitalCommonDigit.Type == null)
                                {
                                    userPanel_text_weather_sunrise = userControl_text_SunriseSunset;
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
                            if(activity.Icon.ImageIndex2 != null) 
                                userControl_icon.comboBoxSetImage2((long)activity.Icon.ImageIndex2);
                            userControl_icon.numericUpDown_iconX.Value = numericUpDownX;
                            userControl_icon.numericUpDown_iconY.Value = numericUpDownY;
                        }
                    }
                }
            }
            #endregion





            if (Watch_Face.Widgets != null) WidgetsTemp = Watch_Face.Widgets;

            JSON_read_AOD();
            JSON_read_Widgets();
            JSON_read_activityLayer_order();
            JSON_read_dateLayer_order();
            progressBar1.Visible = false;
        }

        private void JSON_read_activityLayer_order()
        {
            if(Watch_Face.System != null && Watch_Face.System.Activity != null)
            {
                int count = 0;
                Dictionary<int, string> activity_layer = new Dictionary<int, string>();
                foreach (Activity activity in Watch_Face.System.Activity)
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
                        DataGridView_MoveRow(dataGridView_SNL_Activity, i, activity_layer[i]);
                    }
                }
                dataGridView_SNL_Activity.ClearSelection();
            }
        }

        private void JSON_read_dateLayer_order()
        {
            if (Watch_Face.System != null && Watch_Face.System.Date != null && Watch_Face.System.Date.DateDigits != null)
            {
                int count = 0;
                Dictionary<int, string> date_layer = new Dictionary<int, string>();
                foreach (DigitalDateDigit date in Watch_Face.System.Date.DateDigits)
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
                        DataGridView_MoveRow(dataGridView_SNL_Date, i, date_layer[i]);
                    }
                }
                dataGridView_SNL_Date.ClearSelection();
            }
        }

        private void DataGridView_MoveRow(DataGridView dgv, int position, string name)
        {
            int oldPosition = -1;
            for (int i = 0; i < dgv.RowCount; i++)
            {
                if(dgv.Rows[i].Cells[0].Value.ToString() == name)
                {
                    dgv.Rows[i].Visible = true;
                    oldPosition = i;
                    break;
                }
            }
            if (oldPosition > 0 && oldPosition != position)
            {
                DataGridViewRow selectedRow = dgv.Rows[oldPosition];
                dgv.Rows.RemoveAt(oldPosition);
                dgv.Rows.Insert(position, selectedRow);
                dgv.ClearSelection();
                //dataGridView_ScreenNormalLayer.Rows[position].Selected = true;
            }
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
            if (radioButton_GTR2e.Checked)
            {
                Watch_Face.Info = new Device_Id();
                Watch_Face.Info.DeviceId = 64;
            }
            if (radioButton_GTS2.Checked)
            {
                Watch_Face.Info = new Device_Id();
                Watch_Face.Info.DeviceId = 65;
            }
            if (radioButton_TRex_pro.Checked)
            {
                Watch_Face.Info = new Device_Id();
                Watch_Face.Info.DeviceId = 83;
            }
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

            if (checkBox_UseID.Checked)
            {
                if(Watch_Face.Info == null) Watch_Face.Info = new Device_Id();
                int ID = 0;
                Int32.TryParse(textBox_WatchfaceID.Text, out ID);
                if (ID >= 1000)
                {
                    Watch_Face.Info.WatchFaceId = ID;
                }
            }

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
                string Alignment = StringToAlignment(comboBox_Hour_alignment.SelectedIndex);
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
                string Alignment = StringToAlignment(comboBox_Minute_alignment.SelectedIndex);
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
                string Alignment = StringToAlignment(comboBox_Second_alignment.SelectedIndex);
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

            AddActivityTime(null, null, null, userControl_SystemFont_GroupTime);
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

            for (int i = 0; i < dataGridView_SNL_Date.RowCount; i++)
            {
                string dataName = dataGridView_SNL_Date.Rows[i].Cells[0].Value.ToString();

                // год
                if (dataName == "Year")
                {
                    if (checkBox_Year_text_Use.Checked && comboBox_Year_image.SelectedIndex >= 0)
                    {
                        if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                        if (Watch_Face.System.Date == null) Watch_Face.System.Date = new DateAmazfit();
                        if (Watch_Face.System.Date.DateDigits == null)
                            Watch_Face.System.Date.DateDigits = new List<DigitalDateDigit>();

                        DigitalDateDigit digitalDateDigit = new DigitalDateDigit();
                        digitalDateDigit.DateType = "Year";
                        //digitalDateDigit.CombingMode = "Single";
                        digitalDateDigit.CombingMode = checkBox_Year_follow.Checked ? "Follow" : "Single";
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
                        string Alignment = StringToAlignment(comboBox_Year_alignment.SelectedIndex);
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

                    // данные системным шрифтом
                    UserControl_SystemFont userControl_SystemFont = 
                        userControl_SystemFont_Group_Year.userControl_SystemFont;
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
                        if (Watch_Face.System.Date == null) Watch_Face.System.Date = new DateAmazfit();
                        if (Watch_Face.System.Date.DateDigits == null)
                            Watch_Face.System.Date.DateDigits = new List<DigitalDateDigit>();

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

                        Watch_Face.System.Date.DateDigits.Add(digitalDateDigit);
                    }

                    // данные системным шрифтом по окружности
                    UserControl_FontRotate userControl_FontRotate = 
                        userControl_SystemFont_Group_Year.userControl_FontRotate;
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
                        if (Watch_Face.System.Date == null) Watch_Face.System.Date = new DateAmazfit();
                        if (Watch_Face.System.Date.DateDigits == null)
                            Watch_Face.System.Date.DateDigits = new List<DigitalDateDigit>();

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

                        Watch_Face.System.Date.DateDigits.Add(digitalDateDigit);
                    }
                }

                // месяц
                if (dataName == "Month")
                {
                    if (checkBox_Month_Use.Checked && comboBox_Month_image.SelectedIndex >= 0)
                    {
                        if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                        if (Watch_Face.System.Date == null) Watch_Face.System.Date = new DateAmazfit();
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
                        string Alignment = StringToAlignment(comboBox_Month_alignment.SelectedIndex);
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

                    // данные системным шрифтом
                    UserControl_SystemFont userControl_SystemFont =
                        userControl_SystemFont_Group_Month.userControl_SystemFont;
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
                        if (Watch_Face.System.Date == null) Watch_Face.System.Date = new DateAmazfit();
                        if (Watch_Face.System.Date.DateDigits == null)
                            Watch_Face.System.Date.DateDigits = new List<DigitalDateDigit>();

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

                        Watch_Face.System.Date.DateDigits.Add(digitalDateDigit);
                    }

                    // данные системным шрифтом по окружности
                    UserControl_FontRotate userControl_FontRotate =
                        userControl_SystemFont_Group_Month.userControl_FontRotate;
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
                        if (Watch_Face.System.Date == null) Watch_Face.System.Date = new DateAmazfit();
                        if (Watch_Face.System.Date.DateDigits == null)
                            Watch_Face.System.Date.DateDigits = new List<DigitalDateDigit>();

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

                        Watch_Face.System.Date.DateDigits.Add(digitalDateDigit);
                    }

                    // месяц картинкой
                    if (checkBox_Month_pictures_Use.Checked && comboBox_Month_pictures_image.SelectedIndex >= 0)
                    {
                        if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                        if (Watch_Face.System.Date == null) Watch_Face.System.Date = new DateAmazfit();
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
                }

                // число
                if (dataName == "Day")
                {
                    if (checkBox_Day_Use.Checked && comboBox_Day_image.SelectedIndex >= 0)
                    {
                        if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                        if (Watch_Face.System.Date == null) Watch_Face.System.Date = new DateAmazfit();
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
                        string Alignment = StringToAlignment(comboBox_Day_alignment.SelectedIndex);
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

                    // данные системным шрифтом
                    UserControl_SystemFont userControl_SystemFont =
                        userControl_SystemFont_Group_Day.userControl_SystemFont;
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
                        if (Watch_Face.System.Date == null) Watch_Face.System.Date = new DateAmazfit();
                        if (Watch_Face.System.Date.DateDigits == null)
                            Watch_Face.System.Date.DateDigits = new List<DigitalDateDigit>();

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

                        Watch_Face.System.Date.DateDigits.Add(digitalDateDigit);
                    }

                    // данные системным шрифтом по окружности
                    UserControl_FontRotate userControl_FontRotate =
                        userControl_SystemFont_Group_Day.userControl_FontRotate;
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
                        if (Watch_Face.System.Date == null) Watch_Face.System.Date = new DateAmazfit();
                        if (Watch_Face.System.Date.DateDigits == null)
                            Watch_Face.System.Date.DateDigits = new List<DigitalDateDigit>();

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

                        Watch_Face.System.Date.DateDigits.Add(digitalDateDigit);
                    }
                }
            }

            // день недели картинкой
            if (checkBox_DOW_pictures_Use.Checked && comboBox_DOW_pictures_image.SelectedIndex >= 0)
            {
                if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                if (Watch_Face.System.Date == null) Watch_Face.System.Date = new DateAmazfit();
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
                    Watch_Face.System.Date = new DateAmazfit();
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

            // месяц стрелкой
            if (checkBox_Month_hand_Use.Checked && comboBox_Month_hand_image.SelectedIndex >= 0)
            {
                if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                if (Watch_Face.System.Date == null)
                    Watch_Face.System.Date = new DateAmazfit();
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

            // число стрелкой
            if (checkBox_Day_hand_Use.Checked && comboBox_Day_hand_image.SelectedIndex >= 0)
            {
                if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                if (Watch_Face.System.Date == null)
                    Watch_Face.System.Date = new DateAmazfit();
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

            UserControl_pictures userPanel_pictures;
            UserControl_text userPanel_text;
            UserControl_text userPanel_textGoal = null;
            UserControl_hand userPanel_hand;
            UserControl_scaleCircle userPanel_scaleCircle;
            UserControl_scaleLinear userPanel_scaleLinear;
            UserControl_SystemFont_Group userControl_SystemFont_Group = null;
            UserControl_icon userControl_icon;

            for (int i = 0; i < dataGridView_SNL_Activity.RowCount; i++)
            {
                string activityName = dataGridView_SNL_Activity.Rows[i].Cells[0].Value.ToString();
               
                #region Battery

                if (activityName == "Battery")
                {
                    userPanel_pictures = userControl_pictures_Battery;
                    userPanel_text = userControl_text_Battery;
                    userPanel_hand = userControl_hand_Battery;
                    userPanel_scaleCircle = userControl_scaleCircle_Battery;
                    userPanel_scaleLinear = userControl_scaleLinear_Battery;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_Battery;
                    userControl_icon = userControl_icon_Battery;

                    AddActivity(userPanel_pictures, userPanel_text, userPanel_textGoal, userPanel_hand,
                        userPanel_scaleCircle, userPanel_scaleLinear, userControl_SystemFont_Group,
                        userControl_icon, "Battery"); 
                }

                #endregion

                #region Steps

                if (activityName == "Steps")
                {
                    userPanel_pictures = userControl_pictures_Steps;
                    userPanel_text = userControl_text_Steps;
                    userPanel_textGoal = userControl_text_goal_Steps;
                    userPanel_hand = userControl_hand_Steps;
                    userPanel_scaleCircle = userControl_scaleCircle_Steps;
                    userPanel_scaleLinear = userControl_scaleLinear_Steps;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_Steps;
                    userControl_icon = userControl_icon_Steps;

                    AddActivity(userPanel_pictures, userPanel_text, userPanel_textGoal, userPanel_hand,
                        userPanel_scaleCircle, userPanel_scaleLinear, userControl_SystemFont_Group,
                        userControl_icon, "Steps");

                    userPanel_textGoal = null; 
                }
                #endregion

                #region Calories

                if (activityName == "Calories")
                {
                    userPanel_pictures = userControl_pictures_Calories;
                    userPanel_text = userControl_text_Calories;
                    userPanel_textGoal = userControl_text_goal_Calories;
                    userPanel_hand = userControl_hand_Calories;
                    userPanel_scaleCircle = userControl_scaleCircle_Calories;
                    userPanel_scaleLinear = userControl_scaleLinear_Calories;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_Calories;
                    userControl_icon = userControl_icon_Calories;

                    AddActivity(userPanel_pictures, userPanel_text, userPanel_textGoal, userPanel_hand,
                        userPanel_scaleCircle, userPanel_scaleLinear, userControl_SystemFont_Group,
                        userControl_icon, "Calories");

                    userPanel_textGoal = null; 
                }
                #endregion

                #region HeartRate

                if (activityName == "HeartRate")
                {
                    userPanel_pictures = userControl_pictures_HeartRate;
                    userPanel_text = userControl_text_HeartRate;
                    userPanel_hand = userControl_hand_HeartRate;
                    userPanel_scaleCircle = userControl_scaleCircle_HeartRate;
                    userPanel_scaleLinear = userControl_scaleLinear_HeartRate;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_HeartRate;
                    userControl_icon = userControl_icon_HeartRate;

                    AddActivity(userPanel_pictures, userPanel_text, userPanel_textGoal, userPanel_hand,
                        userPanel_scaleCircle, userPanel_scaleLinear, userControl_SystemFont_Group,
                        userControl_icon, "HeartRate"); 
                }

                #endregion

                #region PAI

                if (activityName == "PAI")
                {
                    userPanel_pictures = userControl_pictures_PAI;
                    userPanel_text = userControl_text_PAI;
                    userPanel_hand = userControl_hand_PAI;
                    userPanel_scaleCircle = userControl_scaleCircle_PAI;
                    userPanel_scaleLinear = userControl_scaleLinear_PAI;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_PAI;
                    userControl_icon = userControl_icon_PAI;

                    AddActivity(userPanel_pictures, userPanel_text, userPanel_textGoal, userPanel_hand,
                        userPanel_scaleCircle, userPanel_scaleLinear, userControl_SystemFont_Group,
                        userControl_icon, "PAI"); 
                }

                #endregion

                #region Distance

                if (activityName == "Distance")
                {
                    userPanel_pictures = null;
                    userPanel_text = userControl_text_Distance;
                    userPanel_hand = null;
                    userPanel_scaleCircle = null;
                    userPanel_scaleLinear = null;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_Distance;
                    userControl_icon = userControl_icon_Distance;

                    AddActivity(userPanel_pictures, userPanel_text, userPanel_textGoal, userPanel_hand,
                        userPanel_scaleCircle, userPanel_scaleLinear, userControl_SystemFont_Group,
                        userControl_icon, "Distance"); 
                }

                #endregion

                #region StandUp

                if (activityName == "StandUp")
                {
                    userPanel_pictures = userControl_pictures_StandUp;
                    userPanel_text = userControl_text_StandUp;
                    userPanel_textGoal = userControl_text_goal_StandUp;
                    userPanel_hand = userControl_hand_StandUp;
                    userPanel_scaleCircle = userControl_scaleCircle_StandUp;
                    userPanel_scaleLinear = userControl_scaleLinear_StandUp;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_StandUp;
                    userControl_icon = userControl_icon_StandUp;

                    AddActivity(userPanel_pictures, userPanel_text, userPanel_textGoal, userPanel_hand,
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

                    AddActivityWeather(userControl_pictures_weather, userControl_text_weather_Current,
                        userControl_text_weather_Min, userControl_text_weather_Max, userControl_hand_Weather,
                        userControl_scaleCircle_Weather, userControl_scaleLinear_Weather,
                        userControl_SystemFont_GroupWeather, userControl_icon_Weather); 
                }
                #endregion

                #region UVindex

                if (activityName == "UVindex")
                {
                    userPanel_pictures = userControl_pictures_UVindex;
                    userPanel_text = userControl_text_UVindex;
                    userPanel_hand = userControl_hand_UVindex;
                    userPanel_scaleCircle = userControl_scaleCircle_UVindex;
                    userPanel_scaleLinear = userControl_scaleLinear_UVindex;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_UVindex;
                    userControl_icon = userControl_icon_UVindex;

                    AddActivity(userPanel_pictures, userPanel_text, userPanel_textGoal, userPanel_hand,
                        userPanel_scaleCircle, userPanel_scaleLinear, userControl_SystemFont_Group,
                        userControl_icon, "UVindex"); 
                }

                #endregion

                #region AirQuality

                if (activityName == "AirQuality")
                {
                    userPanel_pictures = userControl_pictures_AirQuality;
                    userPanel_text = userControl_text_AirQuality;
                    userPanel_hand = userControl_hand_AirQuality;
                    userPanel_scaleCircle = userControl_scaleCircle_AirQuality;
                    userPanel_scaleLinear = userControl_scaleLinear_AirQuality;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_AirQuality;
                    userControl_icon = userControl_icon_AirQuality;

                    AddActivity(userPanel_pictures, userPanel_text, userPanel_textGoal, userPanel_hand,
                        userPanel_scaleCircle, userPanel_scaleLinear, userControl_SystemFont_Group,
                        userControl_icon, "AirQuality"); 
                }

                #endregion

                #region Humidity

                if (activityName == "Humidity")
                {
                    userPanel_pictures = userControl_pictures_Humidity;
                    userPanel_text = userControl_text_Humidity;
                    userPanel_hand = userControl_hand_Humidity;
                    userPanel_scaleCircle = userControl_scaleCircle_Humidity;
                    userPanel_scaleLinear = userControl_scaleLinear_Humidity;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_Humidity;
                    userControl_icon = userControl_icon_Humidity;

                    AddActivity(userPanel_pictures, userPanel_text, userPanel_textGoal, userPanel_hand,
                        userPanel_scaleCircle, userPanel_scaleLinear, userControl_SystemFont_Group,
                        userControl_icon, "Humidity"); 
                }

                #endregion

                #region Sunrise

                if (activityName == "Sunrise")
                {
                    AddActivitySunrise(userControl_pictures_Sunrise, userControl_text_SunriseSunset,
                                userControl_text_Sunrise, userControl_text_Sunset, userControl_hand_Sunrise,
                                userControl_scaleCircle_Sunrise, userControl_scaleLinear_Sunrise,
                                userControl_SystemFont_GroupSunrise, userControl_icon_Sunrise); 
                }
                #endregion

                #region WindForce

                if (activityName == "WindForce")
                {
                    userPanel_pictures = userControl_pictures_WindForce;
                    userPanel_text = userControl_text_WindForce;
                    userPanel_hand = userControl_hand_WindForce;
                    userPanel_scaleCircle = userControl_scaleCircle_WindForce;
                    userPanel_scaleLinear = userControl_scaleLinear_WindForce;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_WindForce;
                    userControl_icon = userControl_icon_WindForce;

                    AddActivity(userPanel_pictures, userPanel_text, userPanel_textGoal, userPanel_hand,
                        userPanel_scaleCircle, userPanel_scaleLinear, userControl_SystemFont_Group,
                        userControl_icon, "WindForce"); 
                }

                #endregion

                #region Altitude

                if (activityName == "Altitude")
                {
                    userPanel_pictures = userControl_pictures_Altitude;
                    userPanel_text = userControl_text_Altitude;
                    userPanel_hand = userControl_hand_Altitude;
                    userPanel_scaleCircle = userControl_scaleCircle_Altitude;
                    userPanel_scaleLinear = userControl_scaleLinear_Altitude;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_Altitude;
                    userControl_icon = userControl_icon_Altitude;

                    AddActivity(userPanel_pictures, userPanel_text, userPanel_textGoal, userPanel_hand,
                        userPanel_scaleCircle, userPanel_scaleLinear, userControl_SystemFont_Group,
                        userControl_icon, "Altitude"); 
                }

                #endregion

                #region AirPressure

                if (activityName == "AirPressure")
                {
                    userPanel_pictures = userControl_pictures_AirPressure;
                    userPanel_text = userControl_text_AirPressure;
                    userPanel_hand = userControl_hand_AirPressure;
                    userPanel_scaleCircle = userControl_scaleCircle_AirPressure;
                    userPanel_scaleLinear = userControl_scaleLinear_AirPressure;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_AirPressure;
                    userControl_icon = userControl_icon_AirPressure;

                    AddActivity(userPanel_pictures, userPanel_text, userPanel_textGoal, userPanel_hand,
                        userPanel_scaleCircle, userPanel_scaleLinear, userControl_SystemFont_Group,
                        userControl_icon, "AirPressure"); 
                }

                #endregion

                #region Stress

                if (activityName == "Stress")
                {
                    userPanel_pictures = userControl_pictures_Stress;
                    userPanel_text = userControl_text_Stress;
                    userPanel_hand = userControl_hand_Stress;
                    userPanel_scaleCircle = userControl_scaleCircle_Stress;
                    userPanel_scaleLinear = userControl_scaleLinear_Stress;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_Stress;
                    userControl_icon = userControl_icon_Stress;

                    AddActivity(userPanel_pictures, userPanel_text, userPanel_textGoal, userPanel_hand,
                        userPanel_scaleCircle, userPanel_scaleLinear, userControl_SystemFont_Group,
                        userControl_icon, "Stress"); 
                }

                #endregion

                #region ActivityGoal

                if (activityName == "ActivityGoal")
                {
                    userPanel_pictures = userControl_pictures_ActivityGoal;
                    userPanel_text = userControl_text_ActivityGoal;
                    userPanel_textGoal = userControl_text_goal_ActivityGoal;
                    userPanel_hand = userControl_hand_ActivityGoal;
                    userPanel_scaleCircle = userControl_scaleCircle_ActivityGoal;
                    userPanel_scaleLinear = userControl_scaleLinear_ActivityGoal;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_ActivityGoal;
                    userControl_icon = userControl_icon_ActivityGoal;

                    AddActivity(userPanel_pictures, userPanel_text, userPanel_textGoal, userPanel_hand,
                        userPanel_scaleCircle, userPanel_scaleLinear, userControl_SystemFont_Group,
                        userControl_icon, "ActivityGoal");

                    userPanel_textGoal = null; 
                }
                #endregion

                #region FatBurning

                if (activityName == "FatBurning")
                {
                    userPanel_pictures = userControl_pictures_FatBurning;
                    userPanel_text = userControl_text_FatBurning;
                    userPanel_textGoal = userControl_text_goal_FatBurning;
                    userPanel_hand = userControl_hand_FatBurning;
                    userPanel_scaleCircle = userControl_scaleCircle_FatBurning;
                    userPanel_scaleLinear = userControl_scaleLinear_FatBurning;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_FatBurning;
                    userControl_icon = userControl_icon_FatBurning;

                    AddActivity(userPanel_pictures, userPanel_text, userPanel_textGoal, userPanel_hand,
                        userPanel_scaleCircle, userPanel_scaleLinear, userControl_SystemFont_Group,
                        userControl_icon, "FatBurning");

                    userPanel_textGoal = null; 
                }
                #endregion 
            }

            #endregion



            if (Watch_Face.Widgets != null) WidgetsTemp = Watch_Face.Widgets;

            if (WidgetsTemp != null) Watch_Face.Widgets = WidgetsTemp;

           
            JSON_write_AOD();
            JSON_write_activityLayer();
            JSON_write_dateLayer();


            richTextBox_JsonText.Text = JsonConvert.SerializeObject(Watch_Face, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            JsonToTree(richTextBox_JsonText.Text);
            JSON_Modified = true;
        }

        private void JSON_write_activityLayer()
        {
            if (Watch_Face.System != null && Watch_Face.System.Activity != null)
            {
                int count = 0;
                Dictionary<int, string> activity_layer = new Dictionary<int, string>();
                foreach (Activity activity in Watch_Face.System.Activity)
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

                for (int i = 0; i < dataGridView_SNL_Activity.RowCount; i++)
                {
                    string name = dataGridView_SNL_Activity.Rows[i].Cells[0].Value.ToString();
                    if (activity_layer.ContainsValue(name)) dataGridView_SNL_Activity.Rows[i].Visible = true;
                    else dataGridView_SNL_Activity.Rows[i].Visible = false;

                }
                if (dataGridView_SNL_Activity.SelectedCells.Count > 0)
                {
                    int RowIndexSelect = dataGridView_SNL_Activity.SelectedCells[0].RowIndex;
                    dataGridView_SNL_Activity.ClearSelection();
                    if (dataGridView_SNL_Activity.Rows[RowIndexSelect].Visible)
                        dataGridView_SNL_Activity.Rows[RowIndexSelect].Selected = true;
                }
            }
        }

        private void JSON_write_dateLayer()
        {
            if (Watch_Face.System != null && Watch_Face.System.Date != null && Watch_Face.System.Date.DateDigits != null)
            {
                int count = 0;
                Dictionary<int, string> date_layer = new Dictionary<int, string>();
                foreach (DigitalDateDigit date in Watch_Face.System.Date.DateDigits)
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

                for (int i = 0; i < dataGridView_SNL_Date.RowCount; i++)
                {
                    string name = dataGridView_SNL_Date.Rows[i].Cells[0].Value.ToString();
                    if (date_layer.ContainsValue(name)) dataGridView_SNL_Date.Rows[i].Visible = true;
                    else dataGridView_SNL_Date.Rows[i].Visible = false;

                }
                if (dataGridView_SNL_Date.SelectedCells.Count > 0)
                {
                    int RowIndexSelect = dataGridView_SNL_Date.SelectedCells[0].RowIndex;
                    dataGridView_SNL_Date.ClearSelection();
                    if (dataGridView_SNL_Date.Rows[RowIndexSelect].Visible)
                        dataGridView_SNL_Date.Rows[RowIndexSelect].Selected = true;
                }
            }
        }

        private void AddActivityWeather(UserControl_pictures panel_pictures,
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

                if(follow_max)
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

            if (activity != null)
            {
                activity.Type = "Weather";
                if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                if (Watch_Face.System.Activity == null) Watch_Face.System.Activity = new List<Activity>();
                Watch_Face.System.Activity.Add(activity);
            }

            if (activityMin != null)
            {
                activityMin.Type = "Weather";
                if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                if (Watch_Face.System.Activity == null) Watch_Face.System.Activity = new List<Activity>();
                Watch_Face.System.Activity.Add(activityMin);
            }

            if (activityMax != null)
            {
                activityMax.Type = "Weather";
                if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                if (Watch_Face.System.Activity == null) Watch_Face.System.Activity = new List<Activity>();
                Watch_Face.System.Activity.Add(activityMax);
            }

            if (activityPictures != null)
            {
                activityPictures.Type = "Weather";
                if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                if (Watch_Face.System.Activity == null) Watch_Face.System.Activity = new List<Activity>();
                Watch_Face.System.Activity.Add(activityPictures);
            }
        }

        private void AddActivitySunrise(UserControl_pictures panel_pictures,
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
                        if (activitySunrise == null) activitySunrise = new Activity();
                        if (activitySunrise.ProgressBar == null) activitySunrise.ProgressBar = new ProgressBar();
                        activitySunrise.ProgressBar.LinearSettings = new LinearSettings();
                        if (radioButton_image.Checked && panel_scaleLinear.comboBoxGetSelectedIndexImage() >= 0)
                        {
                            activitySunrise.ProgressBar.ForegroundImageIndex = panel_scaleLinear.comboBoxGetImage();
                        }
                        else
                        {
                            activitySunrise.ProgressBar.Color = panel_scaleLinear.comboBoxGetColorString();
                        }
                        if (panel_scaleLinear.comboBoxGetSelectedIndexImagePointer() >= 0)
                            activitySunrise.ProgressBar.PointerImageIndex = panel_scaleLinear.comboBoxGetImagePointer();
                        if (panel_scaleLinear.comboBoxGetSelectedIndexImageBackground() >= 0)
                            activitySunrise.ProgressBar.BackgroundImageIndex = panel_scaleLinear.comboBoxGetImageBackground();

                        activitySunrise.ProgressBar.LinearSettings.StartX = (long)numericUpDownX.Value;
                        activitySunrise.ProgressBar.LinearSettings.StartY = (long)numericUpDownY.Value;
                        long endX = (long)(numericUpDownX.Value + numericUpDown_length.Value);
                        activitySunrise.ProgressBar.LinearSettings.EndX = endX;
                        activitySunrise.ProgressBar.LinearSettings.EndY = (long)numericUpDownY.Value;
                        activitySunrise.ProgressBar.Width = (long)numericUpDown_width.Value; 
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

            if (activity != null)
            {
                activity.Type = "Sunrise";
                if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                if (Watch_Face.System.Activity == null) Watch_Face.System.Activity = new List<Activity>();
                Watch_Face.System.Activity.Add(activity);
            }

            if (activitySunrise != null)
            {
                activitySunrise.Type = "Sunrise";
                if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                if (Watch_Face.System.Activity == null) Watch_Face.System.Activity = new List<Activity>();
                Watch_Face.System.Activity.Add(activitySunrise);
            }

            if (activitySunset != null)
            {
                activitySunset.Type = "Sunrise";
                if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                if (Watch_Face.System.Activity == null) Watch_Face.System.Activity = new List<Activity>();
                Watch_Face.System.Activity.Add(activitySunset);
            }
        }

        private void AddActivityTime(UserControl_text_goal panel_textHour, UserControl_text_goal panel_textMinute,
            UserControl_text_goal panel_textSecond, UserControl_SystemFont_GroupWeather userControl_SystemFont_Group)
        {
            UserControl_SystemFont userControl_SystemFont_Hour = userControl_SystemFont_Group.userControl_SystemFont_weather_Current;
            UserControl_SystemFont userControl_SystemFont_Minute = userControl_SystemFont_Group.userControl_SystemFont_weather_Min;
            UserControl_SystemFont userControl_SystemFont_Second = userControl_SystemFont_Group.userControl_SystemFont_weather_Max;

            UserControl_FontRotate userControl_FontRotate_Hour = userControl_SystemFont_Group.userControl_FontRotate_weather_Current;
            UserControl_FontRotate userControl_FontRotate_Minute = userControl_SystemFont_Group.userControl_FontRotate_weather_Min;
            UserControl_FontRotate userControl_FontRotate_Second = userControl_SystemFont_Group.userControl_FontRotate_weather_Max;


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

                digitalCommonDigit.Digit.SystemFont.ShowUnitCheck = userControl_SystemFont_Hour.checkBoxGetUnit()+1;
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

                digitalCommonDigit.Digit.SystemFont.ShowUnitCheck = userControl_SystemFont_Minute.checkBoxGetUnit()+1;
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

                digitalCommonDigit.Digit.SystemFont.ShowUnitCheck = userControl_SystemFont_Second.checkBoxGetUnit()+1;
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
                if (Watch_Face.DialFace == null) Watch_Face.DialFace = new ScreenNormal();
                if (Watch_Face.DialFace.DigitalDialFace == null) Watch_Face.DialFace.DigitalDialFace = new DigitalDialFace();
                if (Watch_Face.DialFace.DigitalDialFace.Digits == null)
                    Watch_Face.DialFace.DigitalDialFace.Digits = new List<DigitalTimeDigit>();
                Watch_Face.DialFace.DigitalDialFace.Digits.AddRange(digits);
            }
        }

        private void AddActivity(UserControl_pictures panel_pictures, UserControl_text panel_text,
            UserControl_text userPanel_textGoal, UserControl_hand panel_hand, 
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
            if (panel_pictures!=null && panel_pictures.checkBox_pictures_Use.Checked)
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
                    if(image2>=0) activity.Icon.ImageIndex2 = image2;
                    activity.Icon.Coordinates.X = (long)numericUpDownX.Value;
                    activity.Icon.Coordinates.Y = (long)numericUpDownY.Value;
                }
            }

            if (activity != null)
            {
                activity.Type = type;
                if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                if (Watch_Face.System.Activity == null) Watch_Face.System.Activity = new List<Activity>();
                Watch_Face.System.Activity.Add(activity);
            }

            if (activityMin != null)
            {
                activityMin.Type = type;
                if (Watch_Face.System == null) Watch_Face.System = new SystemAmazfit();
                if (Watch_Face.System.Activity == null) Watch_Face.System.Activity = new List<Activity>();
                Watch_Face.System.Activity.Add(activityMin);
            }
        }


        private void AlignmentToString(ComboBox comboBoxAlignment, string Alignment)
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

        private string StringToAlignment(int Alignment)
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


        private void comboBoxSetText(ComboBox comboBox, long value)
        {
            comboBox.Text = value.ToString();
            if (comboBox.SelectedIndex < 0) comboBox.Text = "";
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

            comboBox_Minute_image.Items.AddRange(ListImages.ToArray());
            comboBox_Minute_unit.Items.AddRange(ListImages.ToArray());
            comboBox_Minute_separator.Items.AddRange(ListImages.ToArray());

            comboBox_Second_image.Items.AddRange(ListImages.ToArray());
            comboBox_Second_unit.Items.AddRange(ListImages.ToArray());
            comboBox_Second_separator.Items.AddRange(ListImages.ToArray());

            comboBox_Hour_hand_image.Items.AddRange(ListImages.ToArray());
            comboBox_Hour_hand_imageCentr.Items.AddRange(ListImages.ToArray());

            comboBox_Minute_hand_image.Items.AddRange(ListImages.ToArray());
            comboBox_Minute_hand_imageCentr.Items.AddRange(ListImages.ToArray());

            comboBox_Second_hand_image.Items.AddRange(ListImages.ToArray());
            comboBox_Second_hand_imageCentr.Items.AddRange(ListImages.ToArray());
            progressBar1.Value = 5;

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
            progressBar1.Value = 10;

            userControl_pictures_Battery.ComboBoxAddItems(ListImages);
            userControl_text_Battery.ComboBoxAddItems(ListImages);
            userControl_hand_Battery.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_Battery.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_Battery.ComboBoxAddItems(ListImages);
            userControl_icon_Battery.ComboBoxAddItems(ListImages);

            userControl_pictures_Steps.ComboBoxAddItems(ListImages);
            userControl_text_Steps.ComboBoxAddItems(ListImages);
            userControl_text_goal_Steps.ComboBoxAddItems(ListImages);
            userControl_hand_Steps.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_Steps.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_Steps.ComboBoxAddItems(ListImages);
            userControl_icon_Steps.ComboBoxAddItems(ListImages);
            progressBar1.Value = 15;

            userControl_pictures_Calories.ComboBoxAddItems(ListImages);
            userControl_text_Calories.ComboBoxAddItems(ListImages);
            userControl_hand_Calories.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_Calories.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_Calories.ComboBoxAddItems(ListImages);
            userControl_icon_Calories.ComboBoxAddItems(ListImages);

            userControl_pictures_HeartRate.ComboBoxAddItems(ListImages);
            userControl_text_HeartRate.ComboBoxAddItems(ListImages);
            userControl_hand_HeartRate.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_HeartRate.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_HeartRate.ComboBoxAddItems(ListImages);
            userControl_icon_HeartRate.ComboBoxAddItems(ListImages);
            progressBar1.Value = 20;

            userControl_pictures_PAI.ComboBoxAddItems(ListImages);
            userControl_text_PAI.ComboBoxAddItems(ListImages);
            userControl_hand_PAI.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_PAI.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_PAI.ComboBoxAddItems(ListImages);
            userControl_icon_PAI.ComboBoxAddItems(ListImages);


            userControl_text_Distance.ComboBoxAddItems(ListImages);
            userControl_icon_Distance.ComboBoxAddItems(ListImages);


            userControl_pictures_StandUp.ComboBoxAddItems(ListImages);
            userControl_text_StandUp.ComboBoxAddItems(ListImages);
            userControl_text_goal_StandUp.ComboBoxAddItems(ListImages);
            userControl_hand_StandUp.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_StandUp.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_StandUp.ComboBoxAddItems(ListImages);
            userControl_icon_StandUp.ComboBoxAddItems(ListImages);
            progressBar1.Value = 25;

            userControl_pictures_weather.ComboBoxAddItems(ListImages);
            userControl_text_weather_Current.ComboBoxAddItems(ListImages);
            userControl_text_weather_Min.ComboBoxAddItems(ListImages);
            userControl_text_weather_Max.ComboBoxAddItems(ListImages);
            //userControl_hand_Weather.ComboBoxAddItems(ListImages);
            //userControl_scaleCircle_Weather.ComboBoxAddItems(ListImages);
            //userControl_scaleLinear_Weather.ComboBoxAddItems(ListImages);
            userControl_icon_Weather.ComboBoxAddItems(ListImages);
            progressBar1.Value = 30;

            userControl_pictures_UVindex.ComboBoxAddItems(ListImages);
            userControl_text_UVindex.ComboBoxAddItems(ListImages);
            userControl_hand_UVindex.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_UVindex.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_UVindex.ComboBoxAddItems(ListImages);
            userControl_icon_UVindex.ComboBoxAddItems(ListImages);

            //userControl_pictures_AirQuality.ComboBoxAddItems(ListImages);
            //userControl_text_AirQuality.ComboBoxAddItems(ListImages);
            //userControl_hand_AirQuality.ComboBoxAddItems(ListImages);
            //userControl_scaleCircle_AirQuality.ComboBoxAddItems(ListImages);
            //userControl_scaleLinear_AirQuality.ComboBoxAddItems(ListImages);
            //userControl_icon_AirQuality.ComboBoxAddItems(ListImages);
            progressBar1.Value = 35;

            userControl_pictures_Humidity.ComboBoxAddItems(ListImages);
            userControl_text_Humidity.ComboBoxAddItems(ListImages);
            userControl_hand_Humidity.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_Humidity.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_Humidity.ComboBoxAddItems(ListImages);
            userControl_icon_Humidity.ComboBoxAddItems(ListImages);

            userControl_pictures_Sunrise.ComboBoxAddItems(ListImages);
            userControl_text_SunriseSunset.ComboBoxAddItems(ListImages);
            userControl_text_Sunrise.ComboBoxAddItems(ListImages);
            userControl_text_Sunset.ComboBoxAddItems(ListImages);
            userControl_hand_Sunrise.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_Sunrise.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_Sunrise.ComboBoxAddItems(ListImages);
            userControl_icon_Sunrise.ComboBoxAddItems(ListImages);

            userControl_pictures_WindForce.ComboBoxAddItems(ListImages);
            userControl_text_WindForce.ComboBoxAddItems(ListImages);
            userControl_hand_WindForce.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_WindForce.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_WindForce.ComboBoxAddItems(ListImages);
            userControl_icon_WindForce.ComboBoxAddItems(ListImages);
            progressBar1.Value = 40;

            //userControl_pictures_Altitude.ComboBoxAddItems(ListImages);
            //userControl_text_Altitude.ComboBoxAddItems(ListImages);
            //userControl_hand_Altitude.ComboBoxAddItems(ListImages);
            //userControl_scaleCircle_Altitude.ComboBoxAddItems(ListImages);
            //userControl_scaleLinear_Altitude.ComboBoxAddItems(ListImages);
            //userControl_icon_Altitude.ComboBoxAddItems(ListImages);

            userControl_pictures_AirPressure.ComboBoxAddItems(ListImages);
            userControl_text_AirPressure.ComboBoxAddItems(ListImages);
            userControl_hand_AirPressure.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_AirPressure.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_AirPressure.ComboBoxAddItems(ListImages);
            userControl_icon_AirPressure.ComboBoxAddItems(ListImages);
            progressBar1.Value = 45;

            //userControl_pictures_Stress.ComboBoxAddItems(ListImages);
            //userControl_text_Stress.ComboBoxAddItems(ListImages);
            //userControl_hand_Stress.ComboBoxAddItems(ListImages);
            //userControl_scaleCircle_Stress.ComboBoxAddItems(ListImages);
            //userControl_scaleLinear_Stress.ComboBoxAddItems(ListImages);
            //userControl_icon_Stress.ComboBoxAddItems(ListImages);

            userControl_pictures_ActivityGoal.ComboBoxAddItems(ListImages);
            userControl_text_ActivityGoal.ComboBoxAddItems(ListImages);
            userControl_text_goal_ActivityGoal.ComboBoxAddItems(ListImages);
            userControl_hand_ActivityGoal.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_ActivityGoal.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_ActivityGoal.ComboBoxAddItems(ListImages);
            userControl_icon_ActivityGoal.ComboBoxAddItems(ListImages);

            userControl_pictures_FatBurning.ComboBoxAddItems(ListImages);
            userControl_text_FatBurning.ComboBoxAddItems(ListImages);
            userControl_text_goal_FatBurning.ComboBoxAddItems(ListImages);
            userControl_hand_FatBurning.ComboBoxAddItems(ListImages);
            userControl_scaleCircle_FatBurning.ComboBoxAddItems(ListImages);
            userControl_scaleLinear_FatBurning.ComboBoxAddItems(ListImages);
            userControl_icon_FatBurning.ComboBoxAddItems(ListImages);
            progressBar1.Value = 50;




        }
        private void ComboBoxAddItems_Widgets()
        {
            if(Watch_Face != null && Watch_Face.Widgets != null && Watch_Face.Widgets.Widget != null)
            {
                for(int i = 1; i <= Watch_Face.Widgets.Widget.Count; i++)
                {
                    comboBox_WidgetNumber.Items.Add(i.ToString());
                }
            }

            comboBox_WidgetsUnderMask.Items.AddRange(ListImages.ToArray());
            comboBox_WidgetsTopMask.Items.AddRange(ListImages.ToArray());

            comboBox_WidgetDescriptionBackground.Items.AddRange(ListImages.ToArray());
            comboBox_WidgetBorderActiv.Items.AddRange(ListImages.ToArray());
            comboBox_WidgetBorderInactiv.Items.AddRange(ListImages.ToArray());


            userControl_previewWidget.ComboBoxAddItems(ListImages);
            userControl_picturesWidget.ComboBoxAddItems(ListImages);
            userControl_textWidget.ComboBoxAddItems(ListImages);
            userControl_text_goalWidget.ComboBoxAddItems(ListImages);
            userControl_text_weatherWidgetCur.ComboBoxAddItems(ListImages);
            userControl_text_weatherWidgetMin.ComboBoxAddItems(ListImages);
            userControl_text_weatherWidgetMax.ComboBoxAddItems(ListImages);
            userControl_text_goalWidgetSunrise.ComboBoxAddItems(ListImages);
            userControl_text_goalWidgetSunset.ComboBoxAddItems(ListImages);
            userControl_handWidget.ComboBoxAddItems(ListImages);
            userControl_scaleCircleWidget.ComboBoxAddItems(ListImages);
            userControl_scaleLinearWidget.ComboBoxAddItems(ListImages);
            userControl_iconWidget.ComboBoxAddItems(ListImages);

            userControl_text_date_DayWidget.ComboBoxAddItems(ListImages);
            userControl_hand_DayWidget.ComboBoxAddItems(ListImages);
            userControl_pictures_MonthWidget.ComboBoxAddItems(ListImages);
            userControl_text_date_MonthWidget.ComboBoxAddItems(ListImages);
            userControl_hand_MonthWidget.ComboBoxAddItems(ListImages);
            userControl_text_date_YearWidget.ComboBoxAddItems(ListImages);
            userControl_pictures_DOWWidget.ComboBoxAddItems(ListImages);
            userControl_hand_DOWWidget.ComboBoxAddItems(ListImages);

            if (comboBox_WidgetNumber.Items.Count > 0) comboBox_WidgetNumber.SelectedIndex = 0;
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

            checkBox_Year_text_Use.Checked = false;
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

            comboBox_Minute_image.Items.Clear();
            comboBox_Minute_image.Text = "";
            comboBox_Minute_unit.Items.Clear();
            comboBox_Minute_unit.Text = "";
            comboBox_Minute_separator.Items.Clear();
            comboBox_Minute_separator.Text = "";

            comboBox_Second_image.Items.Clear();
            comboBox_Second_image.Text = "";
            comboBox_Second_unit.Items.Clear();
            comboBox_Second_unit.Text = "";
            comboBox_Second_separator.Items.Clear();
            comboBox_Second_separator.Text = "";


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

            userControl_SystemFont_GroupTime.SettingsClear();

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
            userControl_SystemFont_Group_Day.SettingsClear();

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
            userControl_SystemFont_Group_Month.SettingsClear();

            comboBox_Year_image.Items.Clear();
            comboBox_Year_image.Text = "";
            comboBox_Year_unit.Items.Clear();
            comboBox_Year_unit.Text = "";
            comboBox_Year_separator.Items.Clear();
            comboBox_Year_separator.Text = "";
            userControl_SystemFont_Group_Year.SettingsClear();

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

            userControl_pictures_Battery.SettingsClear();
            userControl_text_Battery.SettingsClear();
            userControl_hand_Battery.SettingsClear();
            userControl_scaleCircle_Battery.SettingsClear();
            userControl_scaleLinear_Battery.SettingsClear();
            userControl_SystemFont_Group_Battery.SettingsClear();
            userControl_icon_Battery.SettingsClear();

            userControl_pictures_Steps.SettingsClear();
            userControl_text_Steps.SettingsClear();
            userControl_text_goal_Steps.SettingsClear();
            userControl_hand_Steps.SettingsClear();
            userControl_scaleCircle_Steps.SettingsClear();
            userControl_scaleLinear_Steps.SettingsClear();
            userControl_SystemFont_Group_Steps.SettingsClear();
            userControl_icon_Steps.SettingsClear();

            userControl_pictures_Calories.SettingsClear();
            userControl_text_Calories.SettingsClear();
            userControl_text_goal_Calories.SettingsClear();
            userControl_hand_Calories.SettingsClear();
            userControl_scaleCircle_Calories.SettingsClear();
            userControl_scaleLinear_Calories.SettingsClear();
            userControl_SystemFont_Group_Calories.SettingsClear();
            userControl_icon_Calories.SettingsClear();

            userControl_pictures_HeartRate.SettingsClear();
            userControl_text_HeartRate.SettingsClear();
            userControl_hand_HeartRate.SettingsClear();
            userControl_scaleCircle_HeartRate.SettingsClear();
            userControl_scaleLinear_HeartRate.SettingsClear();
            userControl_SystemFont_Group_HeartRate.SettingsClear();
            userControl_icon_HeartRate.SettingsClear();

            userControl_pictures_PAI.SettingsClear();
            userControl_text_PAI.SettingsClear();
            userControl_hand_PAI.SettingsClear();
            userControl_scaleCircle_PAI.SettingsClear();
            userControl_scaleLinear_PAI.SettingsClear();
            userControl_SystemFont_Group_PAI.SettingsClear();
            userControl_icon_PAI.SettingsClear();


            userControl_text_Distance.SettingsClear();
            userControl_SystemFont_Group_Distance.SettingsClear();
            userControl_icon_Distance.SettingsClear();


            userControl_pictures_StandUp.SettingsClear();
            userControl_text_StandUp.SettingsClear();
            userControl_text_goal_StandUp.SettingsClear();
            userControl_hand_StandUp.SettingsClear();
            userControl_scaleCircle_StandUp.SettingsClear();
            userControl_scaleLinear_StandUp.SettingsClear();
            userControl_SystemFont_Group_StandUp.SettingsClear();
            userControl_icon_StandUp.SettingsClear();

            userControl_pictures_weather.SettingsClear();
            userControl_text_weather_Current.SettingsClear();
            userControl_text_weather_Min.SettingsClear();
            userControl_text_weather_Max.SettingsClear();
            //userControl_hand_Weather.SettingsClear();
            //userControl_scaleCircle_Weather.SettingsClear();
            //userControl_scaleLinear_Weather.SettingsClear();
            userControl_SystemFont_GroupWeather.SettingsClear();
            userControl_icon_Weather.SettingsClear();

            userControl_pictures_UVindex.SettingsClear();
            userControl_text_UVindex.SettingsClear();
            userControl_hand_UVindex.SettingsClear();
            userControl_scaleCircle_UVindex.SettingsClear();
            userControl_scaleLinear_UVindex.SettingsClear();
            userControl_SystemFont_Group_UVindex.SettingsClear();
            userControl_icon_UVindex.SettingsClear();

            //userControl_pictures_AirQuality.SettingsClear();
            //userControl_text_AirQuality.SettingsClear();
            //userControl_hand_AirQuality.SettingsClear();
            //userControl_scaleCircle_AirQuality.SettingsClear();
            //userControl_scaleLinear_AirQuality.SettingsClear();
            //userControl_SystemFont_AirQuality.SettingsClear();
            //userControl_FontRotate_AirQuality.SettingsClear();
            //userControl_icon_AirQuality.SettingsClear();

            userControl_pictures_Humidity.SettingsClear();
            userControl_text_Humidity.SettingsClear();
            userControl_hand_Humidity.SettingsClear();
            userControl_scaleCircle_Humidity.SettingsClear();
            userControl_scaleLinear_Humidity.SettingsClear();
            userControl_SystemFont_Group_Humidity.SettingsClear();
            userControl_icon_Humidity.SettingsClear();

            userControl_pictures_Sunrise.SettingsClear();
            userControl_text_SunriseSunset.SettingsClear();
            userControl_text_Sunrise.SettingsClear();
            userControl_text_Sunset.SettingsClear();
            userControl_hand_Sunrise.SettingsClear();
            userControl_scaleCircle_Sunrise.SettingsClear();
            userControl_scaleLinear_Sunrise.SettingsClear();
            userControl_SystemFont_GroupSunrise.SettingsClear();
            userControl_icon_Sunrise.SettingsClear();


            userControl_hand_Sunrise.SettingsClear();
            userControl_scaleCircle_Sunrise.SettingsClear();
            userControl_scaleLinear_Sunrise.SettingsClear();
            userControl_SystemFont_GroupSunrise.SettingsClear();
            userControl_icon_Sunrise.SettingsClear();

            userControl_pictures_WindForce.SettingsClear();
            userControl_text_WindForce.SettingsClear();
            userControl_hand_WindForce.SettingsClear();
            userControl_scaleCircle_WindForce.SettingsClear();
            userControl_scaleLinear_WindForce.SettingsClear();
            userControl_SystemFont_Group_WindForce.SettingsClear();
            userControl_icon_WindForce.SettingsClear();

            //userControl_pictures_Altitude.SettingsClear();
            //userControl_text_Altitude.SettingsClear();
            //userControl_hand_Altitude.SettingsClear();
            //userControl_scaleCircle_Altitude.SettingsClear();
            //userControl_scaleLinear_Altitude.SettingsClear();
            //userControl_SystemFont_Altitude.SettingsClear();
            //userControl_FontRotate_Altitude.SettingsClear();
            //userControl_icon_Altitude.SettingsClear();

            userControl_pictures_AirPressure.SettingsClear();
            userControl_text_AirPressure.SettingsClear();
            userControl_hand_AirPressure.SettingsClear();
            userControl_scaleCircle_AirPressure.SettingsClear();
            userControl_scaleLinear_AirPressure.SettingsClear();
            userControl_SystemFont_Group_AirPressure.SettingsClear();
            userControl_icon_AirPressure.SettingsClear();

            //userControl_pictures_Stress.SettingsClear();
            //userControl_text_Stress.SettingsClear();
            //userControl_hand_Stress.SettingsClear();
            //userControl_scaleCircle_Stress.SettingsClear();
            //userControl_scaleLinear_Stress.SettingsClear();
            //userControl_SystemFont_Stress.SettingsClear();
            //userControl_FontRotate_Stress.SettingsClear();
            //userControl_icon_Stress.SettingsClear();

            userControl_pictures_ActivityGoal.SettingsClear();
            userControl_text_ActivityGoal.SettingsClear();
            userControl_text_goal_ActivityGoal.SettingsClear();
            userControl_hand_ActivityGoal.SettingsClear();
            userControl_scaleCircle_ActivityGoal.SettingsClear();
            userControl_scaleLinear_ActivityGoal.SettingsClear();
            userControl_SystemFont_Group_ActivityGoal.SettingsClear();
            userControl_icon_ActivityGoal.SettingsClear();

            userControl_pictures_FatBurning.SettingsClear();
            userControl_text_FatBurning.SettingsClear();
            userControl_text_goal_FatBurning.SettingsClear();
            userControl_hand_FatBurning.SettingsClear();
            userControl_scaleCircle_FatBurning.SettingsClear();
            userControl_scaleLinear_FatBurning.SettingsClear();
            userControl_SystemFont_Group_FatBurning.SettingsClear();
            userControl_icon_FatBurning.SettingsClear();
            foreach(DataGridViewRow dgvr in dataGridView_SNL_Activity.Rows)
            {
                dgvr.Visible = false;
            }
            foreach (DataGridViewRow dgvr in dataGridView_SNL_Date.Rows)
            {
                dgvr.Visible = false;
            }
        }
        private void SettingsClear_Widgets()
        {
            bool PreviewViewTemp = PreviewView;
            comboBox_WidgetNumber.SelectedIndex = -1;
            comboBox_WidgetNumber.Items.Clear();
            comboBox_WidgetNumber.Text = "";

            comboBox_WidgetsUnderMask.Items.Clear();
            comboBox_WidgetsUnderMask.Text = "";
            comboBox_WidgetsTopMask.Items.Clear();
            comboBox_WidgetsTopMask.Text = "";
            PreviewView = PreviewViewTemp;

            //dataGridView_WidgetElement.Rows.Clear();
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
                        //radioButton_GTR2e.Checked = true;
                        radioButton_GTR2.Checked = true;
                        break;
                    case 65:
                        radioButton_GTS2.Checked = true;
                        break;
                    case 83:
                        radioButton_TRex_pro.Checked = true;
                        break;
                    default:
                        return;
                }

                if (radioButton_GTR2.Checked)
                {
                    this.Text = "GTR 2 watch face editor";
                    //pictureBox_Preview.Height = 230;
                    //pictureBox_Preview.Width = 230;
                    pictureBox_Preview.Size = new Size((int)(230 * currentDPI), (int)(230 * currentDPI));
                    Program_Settings.unpack_command = Program_Settings.unpack_command_GTR_2;
                }
                else if (radioButton_GTR2e.Checked)
                {
                    this.Text = "GTS 2e watch face editor";
                    //pictureBox_Preview.Height = 224;
                    //pictureBox_Preview.Width = 177;
                    pictureBox_Preview.Size = new Size((int)(230 * currentDPI), (int)(230 * currentDPI));
                    Program_Settings.unpack_command = Program_Settings.unpack_command_GTR_2;
                }
                else if (radioButton_GTS2.Checked)
                {
                    this.Text = "GTS 2 watch face editor";
                    //pictureBox_Preview.Height = 224;
                    //pictureBox_Preview.Width = 177;
                    pictureBox_Preview.Size = new Size((int)(177 * currentDPI), (int)(224 * currentDPI));
                    Program_Settings.unpack_command = Program_Settings.unpack_command_GTS_2;
                }
                else if (radioButton_TRex_pro.Checked)
                {
                    this.Text = "T-Rex Pro watch face editor";
                    //pictureBox_Preview.Height = 224;
                    //pictureBox_Preview.Width = 177;
                    pictureBox_Preview.Size = new Size((int)(183 * currentDPI), (int)(183 * currentDPI));
                    Program_Settings.unpack_command = Program_Settings.unpack_command_TRex_pro;
                }

                if ((formPreview != null) && (formPreview.Visible))
                {
                    Form_Preview.Model_Wath.model_GTR2 = radioButton_GTR2.Checked;
                    Form_Preview.Model_Wath.model_GTR2e = radioButton_GTR2e.Checked;
                    Form_Preview.Model_Wath.model_GTS2 = radioButton_GTS2.Checked;
                    Form_Preview.Model_Wath.model_TRex_pro = radioButton_TRex_pro.Checked;
                }

                Program_Settings.Model_GTR2 = radioButton_GTR2.Checked;
                Program_Settings.Model_GTR2e = radioButton_GTR2e.Checked;
                Program_Settings.Model_GTS2 = radioButton_GTS2.Checked;
                Program_Settings.Model_TRex_pro = radioButton_TRex_pro.Checked;
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
