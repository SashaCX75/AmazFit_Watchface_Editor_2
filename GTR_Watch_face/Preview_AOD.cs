using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmazFit_Watchface_2
{
    public partial class Form1 : Form
    {
        /// <summary>формируем изображение на панедли Graphics</summary>
        /// <param name="gPanel">Поверхность для рисования</param>
        /// <param name="scale">Масштаб прорисовки</param>
        /// <param name="crop">Обрезать по форме экрана</param>
        /// <param name="WMesh">Рисовать белую сетку</param>
        /// <param name="BMesh">Рисовать черную сетку</param>
        /// <param name="BBorder">Рисовать рамку по координатам, вокруг элементов с выравниванием</param>
        /// <param name="showShortcuts">Подсвечивать область ярлыков</param>
        /// <param name="showShortcutsArea">Подсвечивать область ярлыков рамкой</param>
        /// <param name="showShortcutsBorder">Подсвечивать область ярлыков заливкой</param>
        /// <param name="showAnimation">Показывать анимацию при предпросмотре</param>
        /// <param name="showProgressArea">Подсвечивать круговую шкалу при наличии фонового изображения</param>
        /// <param name="showCentrHend">Подсвечивать центр стрелки</param>
        /// 2 - если отрисовка только после анимации, в остальных случаях полная отрисовка</param>
        public void Preview_AOD(Graphics gPanel, float scale, bool crop, bool WMesh, bool BMesh, bool BBorder,
            bool showShortcuts, bool showShortcutsArea, bool showShortcutsBorder, bool showAnimation, bool showProgressArea,
            bool showCentrHend)

        {
            Logger.WriteLine("* Preview_AOD");
            var src = new Bitmap(1, 1);
            gPanel.ScaleTransform(scale, scale, MatrixOrder.Prepend);
            int i;

            #region Black background
            Logger.WriteLine("PreviewToBitmap (Black background)");
            src = OpenFileStream(Application.StartupPath + @"\Mask\mask_gtr_2.png");
            if (radioButton_GTS2.Checked)
            {
                src = OpenFileStream(Application.StartupPath + @"\Mask\mask_gts_2.png");
            }
            offSet_X = src.Width / 2;
            offSet_Y = src.Height / 2;
            gPanel.DrawImage(src, new Rectangle(0, 0, src.Width, src.Height));
            //src.Dispose();
            #endregion

            #region Background
            Logger.WriteLine("PreviewToBitmap (Background)");
            if (comboBox_Background_image_AOD.SelectedIndex >= 0)
            {
                i = comboBox_Background_image_AOD.SelectedIndex;
                src = OpenFileStream(ListImagesFullName[i]);
                gPanel.DrawImage(src, new Rectangle(0, 0, src.Width, src.Height));
                //src.Dispose();
            }
            else gPanel.Clear(Color.Black);
            #endregion
            //if (scale == 0.5) gPanel.SmoothingMode = SmoothingMode.AntiAlias;
            gPanel.SmoothingMode = SmoothingMode.AntiAlias;

            #region дата 
            int date_offsetX = -1;
            int date_offsetY = -1;
            // год
            if (checkBox_Year_text_Use_AOD.Checked && comboBox_Year_image_AOD.SelectedIndex >= 0)
            {
                int imageIndex = comboBox_Year_image_AOD.SelectedIndex;
                int x = (int)numericUpDown_YearX_AOD.Value;
                int y = (int)numericUpDown_YearY_AOD.Value;
                date_offsetY = y;
                int spasing = (int)numericUpDown_Year_spacing_AOD.Value;
                int alignment = comboBox_Year_alignment_AOD.SelectedIndex;
                bool addZero = checkBox_Year_add_zero_AOD.Checked;
                int value = Watch_Face_Preview_Set.Date.Year;
                if (addZero) value = Watch_Face_Preview_Set.Date.Year % 100;
                int separator_index = -1;
                if (comboBox_Year_separator_AOD.SelectedIndex >= 0) separator_index = comboBox_Year_separator_AOD.SelectedIndex;
                addZero = false;

                date_offsetX = Draw_dagital_text(gPanel, imageIndex, x, y,
                    spasing, alignment, value, addZero, 4, separator_index, BBorder);

                if (comboBox_Year_unit_AOD.SelectedIndex >= 0)
                {
                    src = OpenFileStream(ListImagesFullName[comboBox_Year_unit_AOD.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Year_unitX_AOD.Value,
                        (int)numericUpDown_Year_unitY_AOD.Value, src.Width, src.Height));
                }
            }

            // месяц
            if (checkBox_Month_Use_AOD.Checked && comboBox_Month_image_AOD.SelectedIndex >= 0)
            {
                int imageIndex = comboBox_Month_image_AOD.SelectedIndex;
                int x = (int)numericUpDown_MonthX_AOD.Value;
                int y = (int)numericUpDown_MonthY_AOD.Value;
                int spasing = (int)numericUpDown_Month_spacing_AOD.Value;
                int alignment = comboBox_Month_alignment_AOD.SelectedIndex;
                bool addZero = checkBox_Month_add_zero_AOD.Checked;
                //addZero = true;
                int value = Watch_Face_Preview_Set.Date.Month;
                int separator_index = -1;
                if (comboBox_Month_separator_AOD.SelectedIndex >= 0) separator_index = comboBox_Month_separator_AOD.SelectedIndex;
                if (checkBox_Month_follow_AOD.Checked && date_offsetX > -1)
                {
                    x = date_offsetX;
                    alignment = 0;
                    y = date_offsetY;
                }
                date_offsetY = y;
                date_offsetX = Draw_dagital_text(gPanel, imageIndex, x, y,
                    spasing, alignment, value, addZero, 2, separator_index, BBorder);

                if (comboBox_Month_unit_AOD.SelectedIndex >= 0)
                {
                    src = OpenFileStream(ListImagesFullName[comboBox_Month_unit_AOD.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Month_unitX_AOD.Value,
                        (int)numericUpDown_Month_unitY_AOD.Value, src.Width, src.Height));
                }
            }
            else date_offsetX = -1;

            // число
            if (checkBox_Day_Use_AOD.Checked && comboBox_Day_image_AOD.SelectedIndex >= 0)
            {
                int imageIndex = comboBox_Day_image_AOD.SelectedIndex;
                int x = (int)numericUpDown_DayX_AOD.Value;
                int y = (int)numericUpDown_DayY_AOD.Value;
                int spasing = (int)numericUpDown_Day_spacing_AOD.Value;
                int alignment = comboBox_Day_alignment_AOD.SelectedIndex;
                bool addZero = checkBox_Day_add_zero_AOD.Checked;
                //addZero = true;
                int value = Watch_Face_Preview_Set.Date.Day;
                int separator_index = -1;
                if (comboBox_Day_separator_AOD.SelectedIndex >= 0) separator_index = comboBox_Day_separator_AOD.SelectedIndex;
                if (checkBox_Day_follow_AOD.Checked && date_offsetX > -1)
                {
                    x = date_offsetX;
                    alignment = 0;
                    y = date_offsetY;
                }
                Draw_dagital_text(gPanel, imageIndex, x, y,
                    spasing, alignment, value, addZero, 2, separator_index, BBorder);

                if (comboBox_Day_unit_AOD.SelectedIndex >= 0)
                {
                    src = OpenFileStream(ListImagesFullName[comboBox_Day_unit_AOD.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Day_unitX_AOD.Value,
                        (int)numericUpDown_Day_unitY_AOD.Value, src.Width, src.Height));
                }
            }

            // число стрелкой
            if (checkBox_Day_hand_Use_AOD.Checked && comboBox_Day_hand_image_AOD.SelectedIndex >= 0)
            {
                if (comboBox_Day_hand_imageBackground_AOD.SelectedIndex >= 0)
                {
                    src = OpenFileStream(ListImagesFullName[comboBox_Day_hand_imageBackground_AOD.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Day_handX_background_AOD.Value,
                        (int)numericUpDown_Day_handY_background_AOD.Value, src.Width, src.Height));
                }

                int x = (int)numericUpDown_Day_handX_AOD.Value;
                int y = (int)numericUpDown_Day_handY_AOD.Value;
                int offsetX = (int)numericUpDown_Day_handX_offset_AOD.Value;
                int offsetY = (int)numericUpDown_Day_handY_offset_AOD.Value;
                int image_index = comboBox_Day_hand_image_AOD.SelectedIndex;
                float startAngle = (float)(numericUpDown_Day_hand_startAngle_AOD.Value);
                float endAngle = (float)(numericUpDown_Day_hand_endAngle_AOD.Value);
                int Day = Watch_Face_Preview_Set.Date.Day;
                Day--;
                float angle = startAngle + Day * (endAngle - startAngle) / 30;
                DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                if (comboBox_Day_hand_imageCentr_AOD.SelectedIndex >= 0)
                {
                    src = OpenFileStream(ListImagesFullName[comboBox_Day_hand_imageCentr_AOD.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Day_handX_centr_AOD.Value,
                        (int)numericUpDown_Day_handY_centr_AOD.Value, src.Width, src.Height));
                }
            }

            // месяц картинкой
            if (checkBox_Month_pictures_Use_AOD.Checked && comboBox_Month_pictures_image_AOD.SelectedIndex >= 0)
            {
                int imageIndex = comboBox_Month_pictures_image_AOD.SelectedIndex;
                int x = (int)numericUpDown_Month_picturesX_AOD.Value;
                int y = (int)numericUpDown_Month_picturesY_AOD.Value;
                imageIndex = imageIndex + Watch_Face_Preview_Set.Date.Month - 1;

                if (imageIndex < ListImagesFullName.Count)
                {
                    src = OpenFileStream(ListImagesFullName[imageIndex]);
                    gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                }
            }

            // месяц стрелкой
            if (checkBox_Month_hand_Use_AOD.Checked && comboBox_Month_hand_image_AOD.SelectedIndex >= 0)
            {
                if (comboBox_Month_hand_imageBackground_AOD.SelectedIndex >= 0)
                {
                    src = OpenFileStream(ListImagesFullName[comboBox_Month_hand_imageBackground_AOD.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Month_handX_background_AOD.Value,
                        (int)numericUpDown_Month_handY_background_AOD.Value, src.Width, src.Height));
                }

                int x = (int)numericUpDown_Month_handX_AOD.Value;
                int y = (int)numericUpDown_Month_handY_AOD.Value;
                int offsetX = (int)numericUpDown_Month_handX_offset_AOD.Value;
                int offsetY = (int)numericUpDown_Month_handY_offset_AOD.Value;
                int image_index = comboBox_Month_hand_image_AOD.SelectedIndex;
                float startAngle = (float)(numericUpDown_Month_hand_startAngle_AOD.Value);
                float endAngle = (float)(numericUpDown_Month_hand_endAngle_AOD.Value);
                int Month = Watch_Face_Preview_Set.Date.Month;
                Month--;
                float angle = startAngle + Month * (endAngle - startAngle) / 11;
                DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                if (comboBox_Month_hand_imageCentr_AOD.SelectedIndex >= 0)
                {
                    src = OpenFileStream(ListImagesFullName[comboBox_Month_hand_imageCentr_AOD.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Month_handX_centr_AOD.Value,
                        (int)numericUpDown_Month_handY_centr_AOD.Value, src.Width, src.Height));
                }
            }

            // день недели картинкой
            if (checkBox_DOW_pictures_Use_AOD.Checked && comboBox_DOW_pictures_image_AOD.SelectedIndex >= 0)
            {
                int imageIndex = comboBox_DOW_pictures_image_AOD.SelectedIndex;
                int x = (int)numericUpDown_DOW_picturesX_AOD.Value;
                int y = (int)numericUpDown_DOW_picturesY_AOD.Value;
                imageIndex = imageIndex + Watch_Face_Preview_Set.Date.WeekDay - 1;

                if (imageIndex < ListImagesFullName.Count)
                {
                    src = OpenFileStream(ListImagesFullName[imageIndex]);
                    gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                }
            }

            // день недели стрелкой
            if (checkBox_DOW_hand_Use_AOD.Checked && comboBox_DOW_hand_image_AOD.SelectedIndex >= 0)
            {
                if (comboBox_DOW_hand_imageBackground_AOD.SelectedIndex >= 0)
                {
                    src = OpenFileStream(ListImagesFullName[comboBox_DOW_hand_imageBackground_AOD.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_DOW_handX_background_AOD.Value,
                        (int)numericUpDown_DOW_handY_background_AOD.Value, src.Width, src.Height));
                }

                int x = (int)numericUpDown_DOW_handX_AOD.Value;
                int y = (int)numericUpDown_DOW_handY_AOD.Value;
                int offsetX = (int)numericUpDown_DOW_handX_offset_AOD.Value;
                int offsetY = (int)numericUpDown_DOW_handY_offset_AOD.Value;
                int image_index = comboBox_DOW_hand_image_AOD.SelectedIndex;
                float startAngle = (float)(numericUpDown_DOW_hand_startAngle_AOD.Value);
                float endAngle = (float)(numericUpDown_DOW_hand_endAngle_AOD.Value);
                int WeekDay = Watch_Face_Preview_Set.Date.WeekDay;
                WeekDay--;
                if (WeekDay < 0) WeekDay = 6;
                float angle = startAngle + WeekDay * (endAngle - startAngle) / 6;
                DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                if (comboBox_DOW_hand_imageCentr_AOD.SelectedIndex >= 0)
                {
                    src = OpenFileStream(ListImagesFullName[comboBox_DOW_hand_imageCentr_AOD.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_DOW_handX_centr_AOD.Value,
                        (int)numericUpDown_DOW_handY_centr_AOD.Value, src.Width, src.Height));
                }

            }
            #endregion

            #region статусы

            //if (checkBox_Bluetooth_Use_AOD.Checked && comboBox_Bluetooth_image_AOD.SelectedIndex >= 0)
            //{
            //    if (!Watch_Face_Preview_Set.Status.Bluetooth)
            //    {
            //        src = OpenFileStream(ListImagesFullName[comboBox_Bluetooth_image_AOD.SelectedIndex]);
            //        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_BluetoothX_AOD.Value,
            //            (int)numericUpDown_BluetoothY_AOD.Value, src.Width, src.Height));
            //    }
            //}

            //if (checkBox_Alarm_Use_AOD.Checked && comboBox_Alarm_image_AOD.SelectedIndex >= 0)
            //{
            //    if (Watch_Face_Preview_Set.Status.Alarm)
            //    {
            //        src = OpenFileStream(ListImagesFullName[comboBox_Alarm_image_AOD.SelectedIndex]);
            //        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_AlarmX_AOD.Value,
            //            (int)numericUpDown_AlarmY_AOD.Value, src.Width, src.Height));
            //    }
            //}

            //if (checkBox_Lock_Use_AOD.Checked && comboBox_Lock_image_AOD.SelectedIndex >= 0)
            //{
            //    if (Watch_Face_Preview_Set.Status.Lock)
            //    {
            //        src = OpenFileStream(ListImagesFullName[comboBox_Lock_image_AOD.SelectedIndex]);
            //        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_LockX_AOD.Value,
            //            (int)numericUpDown_LockY_AOD.Value, src.Width, src.Height));
            //    }
            //}

            //if (checkBox_DND_Use_AOD.Checked && comboBox_DND_image_AOD.SelectedIndex >= 0)
            //{
            //    if (Watch_Face_Preview_Set.Status.DoNotDisturb)
            //    {
            //        src = OpenFileStream(ListImagesFullName[comboBox_DND_image_AOD.SelectedIndex]);
            //        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_DNDX_AOD.Value,
            //            (int)numericUpDown_DNDY_AOD.Value, src.Width, src.Height));
            //    }
            //}
            #endregion

            #region активности
            Panel panel_pictures = panel_Battery_pictures_AOD;
            Panel panel_text = panel_Battery_text_AOD;
            Panel panel_hand = panel_Battery_hand_AOD;
            Panel panel_scaleCircle = panel_Battery_scaleCircle_AOD;
            Panel panel_scaleLinear = panel_Battery_scaleLinear_AOD;
            CheckBox checkBox_Use = null;

            
            #region зараяд
            // зараяд картинками
            //CheckBox checkBox_Use = (CheckBox)panel_pictures.Controls[0];
            checkBox_Use = (CheckBox)panel_pictures.Controls[0];
            if (checkBox_Use.Checked)
            {
                ComboBox comboBox_image = (ComboBox)panel_pictures.Controls[1];
                if (comboBox_image.SelectedIndex >= 0)
                {
                    NumericUpDown numericUpDownX = (NumericUpDown)panel_pictures.Controls[2];
                    NumericUpDown numericUpDownY = (NumericUpDown)panel_pictures.Controls[3];
                    NumericUpDown numericUpDown_count = (NumericUpDown)panel_pictures.Controls[4];

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int count = (int)numericUpDown_count.Value;
                    //int offSet = (int)Math.Ceiling(count * Watch_Face_Preview_Set.Battery / 100f);
                    int offSet = (int)(count * Watch_Face_Preview_Set.Battery / 100f);
                    //offSet--;
                    if (offSet < 0) offSet = 0;
                    if (offSet >= count) offSet = (int)(count - 1);
                    //int offSet = (int)Math.Round(count * Watch_Face_Preview_Set.Battery / 100f, 0);
                    int imageIndex = comboBox_image.SelectedIndex + offSet;

                    if (imageIndex < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[imageIndex]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }
                }
            }

            // зараяд круговой шкалой
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

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                float width = (float)numericUpDown_width.Value;
                int radius = (int)numericUpDown_radius.Value;
                int imageIndex = comboBox_image.SelectedIndex;
                int imageBackground = comboBox_background.SelectedIndex;
                float StartAngle = (float)numericUpDown_startAngle.Value - 90;
                float EndAngle = (float)(numericUpDown_endAngle.Value -
                    numericUpDown_startAngle.Value);
                Color color = comboBox_color.BackColor;
                float position = (float)Watch_Face_Preview_Set.Battery / 100f;
                int lineCap = comboBox_flatness.SelectedIndex;
                if (radioButton_image.Checked)
                {
                    if (imageIndex >= 0)
                    {
                        DrawScaleCircle_image(gPanel, x, y, radius, width, lineCap, StartAngle, EndAngle, position,
                         imageIndex, imageBackground, showProgressArea);
                    }
                }
                else
                {
                    DrawScaleCircle(gPanel, x, y, radius, width, lineCap, StartAngle, EndAngle, position,
                        color, imageBackground, showProgressArea);
                }
            }

            // зараяд линейной шкалой
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
                ComboBox comboBox_flatness = (ComboBox)panel_scaleLinear.Controls[11];

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                int imageIndex = comboBox_image.SelectedIndex;
                int pointerIndex = comboBox_pointer.SelectedIndex;
                int backgroundIndex = comboBox_background.SelectedIndex;
                int length = (int)numericUpDown_length.Value;
                int width = (int)numericUpDown_width.Value;
                Color color = comboBox_color.BackColor;
                float position = Watch_Face_Preview_Set.Battery / 100f;
                int lineCap = comboBox_flatness.SelectedIndex;

                if (radioButton_image.Checked)
                {
                    if (imageIndex >= 0)
                    {
                        DrawScaleLinearPointer_image(gPanel, x, y, length, width, position, imageIndex, lineCap, pointerIndex, backgroundIndex, showProgressArea);
                    }
                }
                else
                {
                    DrawScaleLinearPointer(gPanel, x, y, length, width, position, color, lineCap, pointerIndex, backgroundIndex, showProgressArea);
                }
            }

            // зараяд надписью
            checkBox_Use = (CheckBox)panel_text.Controls[0];
            if (checkBox_Use.Checked)
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
                //ComboBox comboBox_imageError = (ComboBox)panel_text.Controls[11];

                if (comboBox_image.SelectedIndex >= 0)
                {
                    int imageIndex = comboBox_image.SelectedIndex;
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = comboBox_alignment.SelectedIndex;
                    bool addZero = checkBox_add_zero.Checked;
                    int value = Watch_Face_Preview_Set.Battery;
                    int separator_index = comboBox_separator.SelectedIndex;
                    Draw_dagital_text(gPanel, imageIndex, x, y,
                        spasing, alignment, value, addZero, 3, separator_index, BBorder);

                    if (comboBox_unit.SelectedIndex >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[comboBox_unit.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // зараяд стрелкой
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

                    if (comboBox_imageBackground.SelectedIndex >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[comboBox_imageBackground.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_background.Value,
                            (int)numericUpDownY_background.Value, src.Width, src.Height));
                    }

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int offsetX = (int)numericUpDown_offsetX.Value;
                    int offsetY = (int)numericUpDown_offsetY.Value;
                    int image_index = comboBox_image.SelectedIndex;
                    float startAngle = (float)(numericUpDown_startAngle.Value);
                    float endAngle = (float)(numericUpDown_endAngle.Value);

                    float angle = startAngle + Watch_Face_Preview_Set.Battery * (endAngle - startAngle) / 100;
                    DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                    if (comboBox_imageCentr.SelectedIndex >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[comboBox_imageCentr.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_centr.Value,
                            (int)numericUpDownY_centr.Value, src.Width, src.Height));
                    }
                }
            }
            #endregion

            #region шаги
            panel_pictures = panel_Steps_pictures_AOD;
            panel_text = panel_Steps_text_AOD;
            panel_hand = panel_Steps_hand_AOD;
            panel_scaleCircle = panel_Steps_scaleCircle_AOD;
            panel_scaleLinear = panel_Steps_scaleLinear_AOD;
            // шаги картинками
            checkBox_Use = (CheckBox)panel_pictures.Controls[0];
            if (checkBox_Use.Checked)
            {
                ComboBox comboBox_image = (ComboBox)panel_pictures.Controls[1];
                if (comboBox_image.SelectedIndex >= 0)
                {
                    NumericUpDown numericUpDownX = (NumericUpDown)panel_pictures.Controls[2];
                    NumericUpDown numericUpDownY = (NumericUpDown)panel_pictures.Controls[3];
                    NumericUpDown numericUpDown_count = (NumericUpDown)panel_pictures.Controls[4];

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int count = (int)numericUpDown_count.Value;
                    //int offSet = (int)Math.Ceiling((float)count * Watch_Face_Preview_Set.Activity.Steps / Watch_Face_Preview_Set.Activity.StepsGoal);
                    int offSet = (int)((count - 1f) * Watch_Face_Preview_Set.Activity.Steps / Watch_Face_Preview_Set.Activity.StepsGoal);
                    //offSet--;
                    if (offSet < 0) offSet = 0;
                    if (offSet >= count) offSet = (int)(count - 1);
                    //int offSet = (int)Math.Round(count * Watch_Face_Preview_Set.Battery / 100f, 0);
                    int imageIndex = comboBox_image.SelectedIndex + offSet;

                    if (imageIndex < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[imageIndex]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }
                }
            }

            // шаги круговой шкалой
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

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                float width = (float)numericUpDown_width.Value;
                int radius = (int)numericUpDown_radius.Value;
                int imageIndex = comboBox_image.SelectedIndex;
                int imageBackground = comboBox_background.SelectedIndex;
                float StartAngle = (float)numericUpDown_startAngle.Value - 90;
                float EndAngle = (float)(numericUpDown_endAngle.Value -
                    numericUpDown_startAngle.Value);
                Color color = comboBox_color.BackColor;
                float position = (float)Watch_Face_Preview_Set.Activity.Steps / Watch_Face_Preview_Set.Activity.StepsGoal;
                if (position > 1) position = 1;
                int lineCap = comboBox_flatness.SelectedIndex;
                if (radioButton_image.Checked)
                {
                    if (imageIndex >= 0)
                    {
                        DrawScaleCircle_image(gPanel, x, y, radius, width, lineCap, StartAngle, EndAngle, position,
                         imageIndex, imageBackground, showProgressArea);
                    }
                }
                else
                {
                    DrawScaleCircle(gPanel, x, y, radius, width, lineCap, StartAngle, EndAngle, position,
                        color, imageBackground, showProgressArea);
                }
            }

            // шаги линейной шкалой
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
                ComboBox comboBox_flatness = (ComboBox)panel_scaleLinear.Controls[11];

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                int imageIndex = comboBox_image.SelectedIndex;
                int pointerIndex = comboBox_pointer.SelectedIndex;
                int backgroundIndex = comboBox_background.SelectedIndex;
                int length = (int)numericUpDown_length.Value;
                int width = (int)numericUpDown_width.Value;
                Color color = comboBox_color.BackColor;
                float position = (float)Watch_Face_Preview_Set.Activity.Steps / Watch_Face_Preview_Set.Activity.StepsGoal;
                if (position > 1) position = 1;
                int lineCap = comboBox_flatness.SelectedIndex;

                if (radioButton_image.Checked)
                {
                    if (imageIndex >= 0)
                    {
                        DrawScaleLinearPointer_image(gPanel, x, y, length, width, position, imageIndex, lineCap, pointerIndex, backgroundIndex, showProgressArea);
                    }
                }
                else
                {
                    DrawScaleLinearPointer(gPanel, x, y, length, width, position, color, lineCap, pointerIndex, backgroundIndex, showProgressArea);
                }
            }

            // шаги надписью
            checkBox_Use = (CheckBox)panel_text.Controls[0];
            if (checkBox_Use.Checked)
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
                //ComboBox comboBox_imageError = (ComboBox)panel_text.Controls[11];

                if (comboBox_image.SelectedIndex >= 0)
                {
                    int imageIndex = comboBox_image.SelectedIndex;
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = comboBox_alignment.SelectedIndex;
                    bool addZero = checkBox_add_zero.Checked;
                    int value = Watch_Face_Preview_Set.Activity.Steps;
                    int separator_index = comboBox_separator.SelectedIndex;
                    Draw_dagital_text(gPanel, imageIndex, x, y,
                        spasing, alignment, value, addZero, 5, separator_index, BBorder);

                    if (comboBox_unit.SelectedIndex >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[comboBox_unit.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // шаги стрелкой
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

                    if (comboBox_imageBackground.SelectedIndex >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[comboBox_imageBackground.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_background.Value,
                            (int)numericUpDownY_background.Value, src.Width, src.Height));
                    }

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int offsetX = (int)numericUpDown_offsetX.Value;
                    int offsetY = (int)numericUpDown_offsetY.Value;
                    int image_index = comboBox_image.SelectedIndex;
                    float startAngle = (float)(numericUpDown_startAngle.Value);
                    float endAngle = (float)(numericUpDown_endAngle.Value);

                    float angle = startAngle + Watch_Face_Preview_Set.Activity.Steps * (endAngle - startAngle) /
                    Watch_Face_Preview_Set.Activity.StepsGoal;
                    if (Watch_Face_Preview_Set.Activity.Steps > Watch_Face_Preview_Set.Activity.StepsGoal) angle = endAngle;
                    DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                    if (comboBox_imageCentr.SelectedIndex >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[comboBox_imageCentr.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_centr.Value,
                            (int)numericUpDownY_centr.Value, src.Width, src.Height));
                    }
                }
            }
            #endregion

            /*
            #region калории
            panel_pictures = panel_Calories_pictures;
            panel_text = panel_Calories_text;
            panel_hand = panel_Calories_hand;
            panel_scaleCircle = panel_Calories_scaleCircle;
            panel_scaleLinear = panel_Calories_scaleLinear;
            // калории картинками
            checkBox_Use = (CheckBox)panel_pictures.Controls[0];
            if (checkBox_Use.Checked)
            {
                ComboBox comboBox_image = (ComboBox)panel_pictures.Controls[1];
                if (comboBox_image.SelectedIndex >= 0)
                {
                    NumericUpDown numericUpDownX = (NumericUpDown)panel_pictures.Controls[2];
                    NumericUpDown numericUpDownY = (NumericUpDown)panel_pictures.Controls[3];
                    NumericUpDown numericUpDown_count = (NumericUpDown)panel_pictures.Controls[4];

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int count = (int)numericUpDown_count.Value;
                    //int offSet = (int)Math.Ceiling((float)count * Watch_Face_Preview_Set.Activity.Calories / 300f);
                    int offSet = (int)((count - 1f) * Watch_Face_Preview_Set.Activity.Calories / 300f);
                    //offSet--;
                    if (offSet < 0) offSet = 0;
                    if (offSet >= count) offSet = (int)(count - 1);
                    //int offSet = (int)Math.Round(count * Watch_Face_Preview_Set.Battery / 100f, 0);
                    int imageIndex = comboBox_image.SelectedIndex + offSet;

                    if (imageIndex < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[imageIndex]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }
                }
            }

            // калории круговой шкалой
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

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                float width = (float)numericUpDown_width.Value;
                int radius = (int)numericUpDown_radius.Value;
                int imageIndex = comboBox_image.SelectedIndex;
                int imageBackground = comboBox_background.SelectedIndex;
                float StartAngle = (float)numericUpDown_startAngle.Value - 90;
                float EndAngle = (float)(numericUpDown_endAngle.Value -
                    numericUpDown_startAngle.Value);
                Color color = comboBox_color.BackColor;
                float position = (float)Watch_Face_Preview_Set.Activity.Calories / 300f;
                if (position > 1) position = 1;
                int lineCap = comboBox_flatness.SelectedIndex;
                if (radioButton_image.Checked)
                {
                    if (imageIndex >= 0)
                    {
                        DrawScaleCircle_image(gPanel, x, y, radius, width, lineCap, StartAngle, EndAngle, position,
                         imageIndex, imageBackground, showProgressArea);
                    }
                }
                else
                {
                    DrawScaleCircle(gPanel, x, y, radius, width, lineCap, StartAngle, EndAngle, position,
                        color, imageBackground, showProgressArea);
                }
            }

            // калории линейной шкалой
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

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                int imageIndex = comboBox_image.SelectedIndex;
                int pointerIndex = comboBox_pointer.SelectedIndex;
                int backgroundIndex = comboBox_background.SelectedIndex;
                int length = (int)numericUpDown_length.Value;
                int width = (int)numericUpDown_width.Value;
                Color color = comboBox_color.BackColor;
                float position = (float)Watch_Face_Preview_Set.Activity.Calories / 300f;
                if (position > 1) position = 1;

                if (radioButton_image.Checked)
                {
                    if (imageIndex >= 0)
                    {
                        DrawScaleLinearPointer_image(gPanel, x, y, length, width, position, imageIndex, pointerIndex, backgroundIndex, showProgressArea);
                    }
                }
                else
                {
                    DrawScaleLinearPointer(gPanel, x, y, length, width, position, color, pointerIndex, backgroundIndex, showProgressArea);
                }
            }

            // калории надписью
            checkBox_Use = (CheckBox)panel_text.Controls[0];
            if (checkBox_Use.Checked)
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
                //ComboBox comboBox_imageError = (ComboBox)panel_text.Controls[11];

                if (comboBox_image.SelectedIndex >= 0)
                {
                    int imageIndex = comboBox_image.SelectedIndex;
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = comboBox_alignment.SelectedIndex;
                    bool addZero = checkBox_add_zero.Checked;
                    int value = Watch_Face_Preview_Set.Activity.Calories;
                    int separator_index = comboBox_separator.SelectedIndex;
                    Draw_dagital_text(gPanel, imageIndex, x, y,
                        spasing, alignment, value, addZero, 4, separator_index, BBorder);

                    if (comboBox_unit.SelectedIndex >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[comboBox_unit.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // калории стрелкой
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

                    if (comboBox_imageBackground.SelectedIndex >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[comboBox_imageBackground.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_background.Value,
                            (int)numericUpDownY_background.Value, src.Width, src.Height));
                    }

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int offsetX = (int)numericUpDown_offsetX.Value;
                    int offsetY = (int)numericUpDown_offsetY.Value;
                    int image_index = comboBox_image.SelectedIndex;
                    float startAngle = (float)(numericUpDown_startAngle.Value);
                    float endAngle = (float)(numericUpDown_endAngle.Value);

                    float angle = startAngle + Watch_Face_Preview_Set.Activity.Calories * (endAngle - startAngle) / 300f;
                    if (Watch_Face_Preview_Set.Activity.Calories > 300) angle = endAngle;
                    DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                    if (comboBox_imageCentr.SelectedIndex >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[comboBox_imageCentr.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_centr.Value,
                            (int)numericUpDownY_centr.Value, src.Width, src.Height));
                    }
                }
            }
            #endregion

            #region пульс
            panel_pictures = panel_HeartRate_pictures;
            panel_text = panel_HeartRate_text;
            panel_hand = panel_HeartRate_hand;
            panel_scaleCircle = panel_HeartRate_scaleCircle;
            panel_scaleLinear = panel_HeartRate_scaleLinear;
            // пульс картинками
            checkBox_Use = (CheckBox)panel_pictures.Controls[0];
            if (checkBox_Use.Checked)
            {
                ComboBox comboBox_image = (ComboBox)panel_pictures.Controls[1];
                if (comboBox_image.SelectedIndex >= 0)
                {
                    NumericUpDown numericUpDownX = (NumericUpDown)panel_pictures.Controls[2];
                    NumericUpDown numericUpDownY = (NumericUpDown)panel_pictures.Controls[3];
                    NumericUpDown numericUpDown_count = (NumericUpDown)panel_pictures.Controls[4];

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int count = (int)numericUpDown_count.Value;
                    //int offSet = (int)Math.Ceiling((float)count * Watch_Face_Preview_Set.Activity.HeartRate / 200f);
                    int offSet = (int)((count - 1f) * Watch_Face_Preview_Set.Activity.HeartRate / 200f);
                    //offSet--;
                    if (offSet < 0) offSet = 0;
                    if (offSet >= count) offSet = (int)(count - 1);
                    //int offSet = (int)Math.Round(count * Watch_Face_Preview_Set.Battery / 100f, 0);
                    int imageIndex = comboBox_image.SelectedIndex + offSet;

                    if (imageIndex < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[imageIndex]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }
                }
            }

            // пульс круговой шкалой
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

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                float width = (float)numericUpDown_width.Value;
                int radius = (int)numericUpDown_radius.Value;
                int imageIndex = comboBox_image.SelectedIndex;
                int imageBackground = comboBox_background.SelectedIndex;
                float StartAngle = (float)numericUpDown_startAngle.Value - 90;
                float EndAngle = (float)(numericUpDown_endAngle.Value -
                    numericUpDown_startAngle.Value);
                Color color = comboBox_color.BackColor;
                float position = (float)Watch_Face_Preview_Set.Activity.HeartRate / 200f;
                if (position > 1) position = 1;
                int lineCap = comboBox_flatness.SelectedIndex;
                if (radioButton_image.Checked)
                {
                    if (imageIndex >= 0)
                    {
                        DrawScaleCircle_image(gPanel, x, y, radius, width, lineCap, StartAngle, EndAngle, position,
                         imageIndex, imageBackground, showProgressArea);
                    }
                }
                else
                {
                    DrawScaleCircle(gPanel, x, y, radius, width, lineCap, StartAngle, EndAngle, position,
                        color, imageBackground, showProgressArea);
                }
            }

            // пульс линейной шкалой
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

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                int imageIndex = comboBox_image.SelectedIndex;
                int pointerIndex = comboBox_pointer.SelectedIndex;
                int backgroundIndex = comboBox_background.SelectedIndex;
                int length = (int)numericUpDown_length.Value;
                int width = (int)numericUpDown_width.Value;
                Color color = comboBox_color.BackColor;
                float position = (float)Watch_Face_Preview_Set.Activity.HeartRate / 200f;
                if (position > 1) position = 1;

                if (radioButton_image.Checked)
                {
                    if (imageIndex >= 0)
                    {
                        DrawScaleLinearPointer_image(gPanel, x, y, length, width, position, imageIndex, pointerIndex, backgroundIndex, showProgressArea);
                    }
                }
                else
                {
                    DrawScaleLinearPointer(gPanel, x, y, length, width, position, color, pointerIndex, backgroundIndex, showProgressArea);
                }
            }

            // пульс надписью
            checkBox_Use = (CheckBox)panel_text.Controls[0];
            if (checkBox_Use.Checked)
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
                //ComboBox comboBox_imageError = (ComboBox)panel_text.Controls[11];

                if (comboBox_image.SelectedIndex >= 0)
                {
                    int imageIndex = comboBox_image.SelectedIndex;
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = comboBox_alignment.SelectedIndex;
                    bool addZero = checkBox_add_zero.Checked;
                    int value = Watch_Face_Preview_Set.Activity.HeartRate;
                    int separator_index = comboBox_separator.SelectedIndex;
                    Draw_dagital_text(gPanel, imageIndex, x, y,
                        spasing, alignment, value, addZero, 3, separator_index, BBorder);

                    if (comboBox_unit.SelectedIndex >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[comboBox_unit.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // пульс стрелкой
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

                    if (comboBox_imageBackground.SelectedIndex >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[comboBox_imageBackground.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_background.Value,
                            (int)numericUpDownY_background.Value, src.Width, src.Height));
                    }

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int offsetX = (int)numericUpDown_offsetX.Value;
                    int offsetY = (int)numericUpDown_offsetY.Value;
                    int image_index = comboBox_image.SelectedIndex;
                    float startAngle = (float)(numericUpDown_startAngle.Value);
                    float endAngle = (float)(numericUpDown_endAngle.Value);

                    float angle = startAngle + Watch_Face_Preview_Set.Activity.HeartRate * (endAngle - startAngle) / 200f;
                    if (Watch_Face_Preview_Set.Activity.HeartRate > 200) angle = endAngle;
                    DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                    if (comboBox_imageCentr.SelectedIndex >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[comboBox_imageCentr.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_centr.Value,
                            (int)numericUpDownY_centr.Value, src.Width, src.Height));
                    }
                }
            }
            #endregion

            #region PAI
            panel_pictures = panel_PAI_pictures;
            panel_text = panel_PAI_text;
            panel_hand = panel_PAI_hand;
            panel_scaleCircle = panel_PAI_scaleCircle;
            panel_scaleLinear = panel_PAI_scaleLinear;
            // PAI картинками
            checkBox_Use = (CheckBox)panel_pictures.Controls[0];
            if (checkBox_Use.Checked)
            {
                ComboBox comboBox_image = (ComboBox)panel_pictures.Controls[1];
                if (comboBox_image.SelectedIndex >= 0)
                {
                    NumericUpDown numericUpDownX = (NumericUpDown)panel_pictures.Controls[2];
                    NumericUpDown numericUpDownY = (NumericUpDown)panel_pictures.Controls[3];
                    NumericUpDown numericUpDown_count = (NumericUpDown)panel_pictures.Controls[4];

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int count = (int)numericUpDown_count.Value;
                    //int offSet = (int)Math.Ceiling((float)count * Watch_Face_Preview_Set.Activity.PAI / 100f);
                    int offSet = (int)((count + 1f) * Watch_Face_Preview_Set.Activity.PAI / 100f);
                    //offSet--;
                    if (offSet < 0) offSet = 0;
                    if (offSet >= count) offSet = (int)(count - 1);
                    //int offSet = (int)Math.Round(count * Watch_Face_Preview_Set.Battery / 100f, 0);
                    int imageIndex = comboBox_image.SelectedIndex + offSet;

                    if (imageIndex < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[imageIndex]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }
                }
            }

            // PAI круговой шкалой
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

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                float width = (float)numericUpDown_width.Value;
                int radius = (int)numericUpDown_radius.Value;
                int imageIndex = comboBox_image.SelectedIndex;
                int imageBackground = comboBox_background.SelectedIndex;
                float StartAngle = (float)numericUpDown_startAngle.Value - 90;
                float EndAngle = (float)(numericUpDown_endAngle.Value -
                    numericUpDown_startAngle.Value);
                Color color = comboBox_color.BackColor;
                float position = (float)Watch_Face_Preview_Set.Activity.PAI / 100f;
                if (position > 1) position = 1;
                int lineCap = comboBox_flatness.SelectedIndex;
                if (radioButton_image.Checked)
                {
                    if (imageIndex >= 0)
                    {
                        DrawScaleCircle_image(gPanel, x, y, radius, width, lineCap, StartAngle, EndAngle, position,
                         imageIndex, imageBackground, showProgressArea);
                    }
                }
                else
                {
                    DrawScaleCircle(gPanel, x, y, radius, width, lineCap, StartAngle, EndAngle, position,
                        color, imageBackground, showProgressArea);
                }
            }

            // PAI линейной шкалой
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

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                int imageIndex = comboBox_image.SelectedIndex;
                int pointerIndex = comboBox_pointer.SelectedIndex;
                int backgroundIndex = comboBox_background.SelectedIndex;
                int length = (int)numericUpDown_length.Value;
                int width = (int)numericUpDown_width.Value;
                Color color = comboBox_color.BackColor;
                float position = (float)Watch_Face_Preview_Set.Activity.PAI / 100f;
                if (position > 1) position = 1;

                if (radioButton_image.Checked)
                {
                    if (imageIndex >= 0)
                    {
                        DrawScaleLinearPointer_image(gPanel, x, y, length, width, position, imageIndex, pointerIndex, backgroundIndex, showProgressArea);
                    }
                }
                else
                {
                    DrawScaleLinearPointer(gPanel, x, y, length, width, position, color, pointerIndex, backgroundIndex, showProgressArea);
                }
            }

            // PAI надписью
            checkBox_Use = (CheckBox)panel_text.Controls[0];
            if (checkBox_Use.Checked)
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
                //ComboBox comboBox_imageError = (ComboBox)panel_text.Controls[11];

                if (comboBox_image.SelectedIndex >= 0)
                {
                    int imageIndex = comboBox_image.SelectedIndex;
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = comboBox_alignment.SelectedIndex;
                    bool addZero = checkBox_add_zero.Checked;
                    int value = Watch_Face_Preview_Set.Activity.PAI;
                    int separator_index = comboBox_separator.SelectedIndex;
                    Draw_dagital_text(gPanel, imageIndex, x, y,
                        spasing, alignment, value, addZero, 3, separator_index, BBorder);

                    if (comboBox_unit.SelectedIndex >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[comboBox_unit.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // PAI стрелкой
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

                    if (comboBox_imageBackground.SelectedIndex >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[comboBox_imageBackground.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_background.Value,
                            (int)numericUpDownY_background.Value, src.Width, src.Height));
                    }

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int offsetX = (int)numericUpDown_offsetX.Value;
                    int offsetY = (int)numericUpDown_offsetY.Value;
                    int image_index = comboBox_image.SelectedIndex;
                    float startAngle = (float)(numericUpDown_startAngle.Value);
                    float endAngle = (float)(numericUpDown_endAngle.Value);

                    float angle = startAngle + Watch_Face_Preview_Set.Activity.PAI * (endAngle - startAngle) / 100f;
                    if (Watch_Face_Preview_Set.Activity.PAI > 100) angle = endAngle;
                    DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                    if (comboBox_imageCentr.SelectedIndex >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[comboBox_imageCentr.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_centr.Value,
                            (int)numericUpDownY_centr.Value, src.Width, src.Height));
                    }
                }
            }
            #endregion

            #region путь
            panel_pictures = panel_Distance_pictures;
            panel_text = panel_Distance_text;
            panel_hand = panel_Distance_hand;
            panel_scaleCircle = panel_Distance_scaleCircle;
            panel_scaleLinear = panel_Distance_scaleLinear;
            // путь картинками
            checkBox_Use = (CheckBox)panel_pictures.Controls[0];
            if (checkBox_Use.Checked)
            {
                ComboBox comboBox_image = (ComboBox)panel_pictures.Controls[1];
                if (comboBox_image.SelectedIndex >= 0)
                {
                    NumericUpDown numericUpDownX = (NumericUpDown)panel_pictures.Controls[2];
                    NumericUpDown numericUpDownY = (NumericUpDown)panel_pictures.Controls[3];
                    NumericUpDown numericUpDown_count = (NumericUpDown)panel_pictures.Controls[4];

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int count = (int)numericUpDown_count.Value;
                    //int offSet = (int)Math.Ceiling((float)count * Watch_Face_Preview_Set.Activity.Distance / 10000f);
                    int offSet = (int)((count - 1f) * Watch_Face_Preview_Set.Activity.Distance / 10000f);
                    //offSet--;
                    if (offSet < 0) offSet = 0;
                    if (offSet >= count) offSet = (int)(count - 1);
                    //int offSet = (int)Math.Round(count * Watch_Face_Preview_Set.Battery / 100f, 0);
                    int imageIndex = comboBox_image.SelectedIndex + offSet;

                    if (imageIndex < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[imageIndex]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }
                }
            }

            // путь круговой шкалой
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

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                float width = (float)numericUpDown_width.Value;
                int radius = (int)numericUpDown_radius.Value;
                int imageIndex = comboBox_image.SelectedIndex;
                int imageBackground = comboBox_background.SelectedIndex;
                float StartAngle = (float)numericUpDown_startAngle.Value - 90;
                float EndAngle = (float)(numericUpDown_endAngle.Value -
                    numericUpDown_startAngle.Value);
                Color color = comboBox_color.BackColor;
                float position = (float)Watch_Face_Preview_Set.Activity.Distance / 10000f;
                if (position > 1) position = 1;
                int lineCap = comboBox_flatness.SelectedIndex;
                if (radioButton_image.Checked)
                {
                    if (imageIndex >= 0)
                    {
                        DrawScaleCircle_image(gPanel, x, y, radius, width, lineCap, StartAngle, EndAngle, position,
                         imageIndex, imageBackground, showProgressArea);
                    }
                }
                else
                {
                    DrawScaleCircle(gPanel, x, y, radius, width, lineCap, StartAngle, EndAngle, position,
                        color, imageBackground, showProgressArea);
                }
            }

            // путь линейной шкалой
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

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                int imageIndex = comboBox_image.SelectedIndex;
                int pointerIndex = comboBox_pointer.SelectedIndex;
                int backgroundIndex = comboBox_background.SelectedIndex;
                int length = (int)numericUpDown_length.Value;
                int width = (int)numericUpDown_width.Value;
                Color color = comboBox_color.BackColor;
                float position = (float)Watch_Face_Preview_Set.Activity.Distance / 10000f;
                if (position > 1) position = 1;

                if (radioButton_image.Checked)
                {
                    if (imageIndex >= 0)
                    {
                        DrawScaleLinearPointer_image(gPanel, x, y, length, width, position, imageIndex, pointerIndex, backgroundIndex, showProgressArea);
                    }
                }
                else
                {
                    DrawScaleLinearPointer(gPanel, x, y, length, width, position, color, pointerIndex, backgroundIndex, showProgressArea);
                }
            }

            // путь надписью
            checkBox_Use = (CheckBox)panel_text.Controls[0];
            if (checkBox_Use.Checked)
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
                //ComboBox comboBox_imageError = (ComboBox)panel_text.Controls[11];
                ComboBox comboBox_DecimalPoint = (ComboBox)panel_text.Controls[12];

                if (comboBox_image.SelectedIndex >= 0)
                {
                    int imageIndex = comboBox_image.SelectedIndex;
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = comboBox_alignment.SelectedIndex;
                    bool addZero = checkBox_add_zero.Checked;
                    double value = Watch_Face_Preview_Set.Activity.Distance / 1000d;
                    int separator_index = comboBox_separator.SelectedIndex;
                    int decimalPoint_index = comboBox_DecimalPoint.SelectedIndex;
                    Draw_dagital_text(gPanel, imageIndex, x, y,
                        spasing, alignment, value, addZero, 4, separator_index,
                        decimalPoint_index, 2, BBorder);

                    if (comboBox_unit.SelectedIndex >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[comboBox_unit.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // путь стрелкой
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

                    if (comboBox_imageBackground.SelectedIndex >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[comboBox_imageBackground.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_background.Value,
                            (int)numericUpDownY_background.Value, src.Width, src.Height));
                    }

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int offsetX = (int)numericUpDown_offsetX.Value;
                    int offsetY = (int)numericUpDown_offsetY.Value;
                    int image_index = comboBox_image.SelectedIndex;
                    float startAngle = (float)(numericUpDown_startAngle.Value);
                    float endAngle = (float)(numericUpDown_endAngle.Value);

                    float angle = startAngle + Watch_Face_Preview_Set.Activity.Distance * (endAngle - startAngle) / 10000f;
                    if (Watch_Face_Preview_Set.Activity.Distance > 10000) angle = endAngle;
                    DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                    if (comboBox_imageCentr.SelectedIndex >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[comboBox_imageCentr.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_centr.Value,
                            (int)numericUpDownY_centr.Value, src.Width, src.Height));
                    }
                }
            }
            #endregion
            
            */
            #region погода
            panel_pictures = panel_Weather_pictures_AOD;
            panel_text = panel_Weather_text_AOD;
            panel_hand = panel_Weather_hand_AOD;
            panel_scaleCircle = panel_Weather_scaleCircle_AOD;
            panel_scaleLinear = panel_Weather_scaleLinear_AOD;

            Panel panel_text_min = panel_Weather_textMin_AOD;
            Panel panel_text_max = panel_Weather_textMax_AOD;

            // погода картинками
            checkBox_Use = (CheckBox)panel_pictures.Controls[0];
            if (checkBox_Use.Checked)
            {
                ComboBox comboBox_image = (ComboBox)panel_pictures.Controls[1];
                if (comboBox_image.SelectedIndex >= 0)
                {
                    NumericUpDown numericUpDownX = (NumericUpDown)panel_pictures.Controls[2];
                    NumericUpDown numericUpDownY = (NumericUpDown)panel_pictures.Controls[3];
                    //NumericUpDown numericUpDown_count = (NumericUpDown)panel_pictures.Controls[4];

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    //int count = (int)numericUpDown_count.Value;
                    int offSet = Watch_Face_Preview_Set.Weather.Icon;
                    if (offSet < 0) offSet = 25;
                    //if (offSet >= count) offSet = (int)(count - 1);
                    //int offSet = (int)Math.Round(count * Watch_Face_Preview_Set.Battery / 100f, 0);
                    int imageIndex = comboBox_image.SelectedIndex + offSet;

                    if (imageIndex < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[imageIndex]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }
                }
            }

            // погода круговой шкалой
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

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                float width = (float)numericUpDown_width.Value;
                int radius = (int)numericUpDown_radius.Value;
                int imageIndex = comboBox_image.SelectedIndex;
                int imageBackground = comboBox_background.SelectedIndex;
                float StartAngle = (float)numericUpDown_startAngle.Value - 90;
                float EndAngle = (float)(numericUpDown_endAngle.Value -
                    numericUpDown_startAngle.Value);
                Color color = comboBox_color.BackColor;
                float position = (float)((Watch_Face_Preview_Set.Weather.Temperature + 25) / 60f);
                if (position > 1) position = 1;
                int lineCap = comboBox_flatness.SelectedIndex;
                if (radioButton_image.Checked)
                {
                    if (imageIndex >= 0)
                    {
                        DrawScaleCircle_image(gPanel, x, y, radius, width, lineCap, StartAngle, EndAngle, position,
                         imageIndex, imageBackground, showProgressArea);
                    }
                }
                else
                {
                    DrawScaleCircle(gPanel, x, y, radius, width, lineCap, StartAngle, EndAngle, position,
                        color, imageBackground, showProgressArea);
                }
            }

            // погода линейной шкалой
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
                ComboBox comboBox_flatness = (ComboBox)panel_scaleLinear.Controls[11];

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                int imageIndex = comboBox_image.SelectedIndex;
                int pointerIndex = comboBox_pointer.SelectedIndex;
                int backgroundIndex = comboBox_background.SelectedIndex;
                int length = (int)numericUpDown_length.Value;
                int width = (int)numericUpDown_width.Value;
                Color color = comboBox_color.BackColor;
                float position = (float)((Watch_Face_Preview_Set.Weather.Temperature + 25) / 60f);
                if (position > 1) position = 1;
                int lineCap = comboBox_flatness.SelectedIndex;

                if (radioButton_image.Checked)
                {
                    if (imageIndex >= 0)
                    {
                        DrawScaleLinearPointer_image(gPanel, x, y, length, width, position, imageIndex, lineCap, pointerIndex, backgroundIndex, showProgressArea);
                    }
                }
                else
                {
                    DrawScaleLinearPointer(gPanel, x, y, length, width, position, color, lineCap, pointerIndex, backgroundIndex, showProgressArea);
                }
            }
            
            // погода надписью
            checkBox_Use = (CheckBox)panel_text.Controls[0];
            if (checkBox_Use.Checked)
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
                //CheckBox checkBox_add_zero = (CheckBox)panel_text.Controls[10];
                ComboBox comboBox_imageError = (ComboBox)panel_text.Controls[10];
                //ComboBox comboBox_separatorF = (ComboBox)panel_text.Controls[11];
                ComboBox comboBox_imageMinus = (ComboBox)panel_text.Controls[11];

                if (comboBox_image.SelectedIndex >= 0)
                {
                    int imageIndex = comboBox_image.SelectedIndex;
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = comboBox_alignment.SelectedIndex;
                    //bool addZero = checkBox_add_zero.Checked;
                    int value = Watch_Face_Preview_Set.Weather.Temperature;
                    int separator_index = comboBox_separator.SelectedIndex;
                    int imageError_index = comboBox_imageError.SelectedIndex;
                    int imageMinus_index = comboBox_imageMinus.SelectedIndex;
                    if (!Watch_Face_Preview_Set.Weather.TemperatureNoData)
                    {
                        Draw_weather_text(gPanel, imageIndex, x, y,
                                        spasing, alignment, value, imageMinus_index, separator_index, BBorder);
                    }
                    else
                    {
                        src = OpenFileStream(ListImagesFullName[imageError_index]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }

                    if (comboBox_unit.SelectedIndex >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[comboBox_unit.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // минимальная температура надписью
            int Temperature_offsetX = -1;
            int Temperature_offsetY = -1;
            checkBox_Use = (CheckBox)panel_text_min.Controls[0];
            if (checkBox_Use.Checked)
            {
                ComboBox comboBox_image = (ComboBox)panel_text_min.Controls[1];
                ComboBox comboBox_unit = (ComboBox)panel_text_min.Controls[2];
                ComboBox comboBox_separator = (ComboBox)panel_text_min.Controls[3];
                NumericUpDown numericUpDownX = (NumericUpDown)panel_text_min.Controls[4];
                NumericUpDown numericUpDownY = (NumericUpDown)panel_text_min.Controls[5];
                NumericUpDown numericUpDown_unitX = (NumericUpDown)panel_text_min.Controls[6];
                NumericUpDown numericUpDown_unitY = (NumericUpDown)panel_text_min.Controls[7];
                ComboBox comboBox_alignment = (ComboBox)panel_text_min.Controls[8];
                NumericUpDown numericUpDown_spacing = (NumericUpDown)panel_text_min.Controls[9];
                //CheckBox checkBox_add_zero = (CheckBox)panel_text_min.Controls[10];
                ComboBox comboBox_imageError = (ComboBox)panel_text_min.Controls[10];
                //ComboBox comboBox_separatorF = (ComboBox)panel_text_min.Controls[11];
                ComboBox comboBox_imageMinus = (ComboBox)panel_text_min.Controls[11];

                if (comboBox_image.SelectedIndex >= 0)
                {
                    int imageIndex = comboBox_image.SelectedIndex;
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    Temperature_offsetY = y;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = comboBox_alignment.SelectedIndex;
                    //bool addZero = checkBox_add_zero.Checked;
                    int value = Watch_Face_Preview_Set.Weather.TemperatureMin;
                    int separator_index = comboBox_separator.SelectedIndex;
                    int imageError_index = comboBox_imageError.SelectedIndex;
                    int imageMinus_index = comboBox_imageMinus.SelectedIndex;
                    if (!Watch_Face_Preview_Set.Weather.TemperatureMinMaxNoData)
                    {
                        Temperature_offsetX = Draw_weather_text(gPanel, imageIndex, x, y,
                                        spasing, alignment, value, imageMinus_index, separator_index, BBorder);
                    }
                    else
                    {
                        src = OpenFileStream(ListImagesFullName[imageError_index]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }

                    if (comboBox_unit.SelectedIndex >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[comboBox_unit.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // максимальная температура надписью
            checkBox_Use = (CheckBox)panel_text_max.Controls[0];
            if (checkBox_Use.Checked)
            {
                ComboBox comboBox_image = (ComboBox)panel_text_max.Controls[1];
                ComboBox comboBox_unit = (ComboBox)panel_text_max.Controls[2];
                ComboBox comboBox_separator = (ComboBox)panel_text_max.Controls[3];
                NumericUpDown numericUpDownX = (NumericUpDown)panel_text_max.Controls[4];
                NumericUpDown numericUpDownY = (NumericUpDown)panel_text_max.Controls[5];
                NumericUpDown numericUpDown_unitX = (NumericUpDown)panel_text_max.Controls[6];
                NumericUpDown numericUpDown_unitY = (NumericUpDown)panel_text_max.Controls[7];
                ComboBox comboBox_alignment = (ComboBox)panel_text_max.Controls[8];
                NumericUpDown numericUpDown_spacing = (NumericUpDown)panel_text_max.Controls[9];
                //CheckBox checkBox_add_zero = (CheckBox)panel_text_max.Controls[10];
                ComboBox comboBox_imageError = (ComboBox)panel_text_max.Controls[10];
                //ComboBox comboBox_separatorF = (ComboBox)panel_text_max.Controls[11];
                ComboBox comboBox_imageMinus = (ComboBox)panel_text_max.Controls[11];
                CheckBox checkBox_follow = (CheckBox)panel_text_max.Controls[12];

                if (comboBox_image.SelectedIndex >= 0)
                {
                    int imageIndex = comboBox_image.SelectedIndex;
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = comboBox_alignment.SelectedIndex;
                    //bool addZero = checkBox_add_zero.Checked;
                    int value = Watch_Face_Preview_Set.Weather.TemperatureMax;
                    int separator_index = comboBox_separator.SelectedIndex;
                    int imageError_index = comboBox_imageError.SelectedIndex;
                    int imageMinus_index = comboBox_imageMinus.SelectedIndex;

                    if (checkBox_follow.Checked && Temperature_offsetX > -1)
                    {
                        x = Temperature_offsetX;
                        alignment = 0;
                        y = Temperature_offsetY;
                    }

                    if (!Watch_Face_Preview_Set.Weather.TemperatureMinMaxNoData)
                    {
                        Draw_weather_text(gPanel, imageIndex, x, y,
                                        spasing, alignment, value, imageMinus_index, separator_index, BBorder);
                    }
                    else
                    {
                        src = OpenFileStream(ListImagesFullName[imageError_index]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }

                    if (comboBox_unit.SelectedIndex >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[comboBox_unit.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // погода стрелкой
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

                    if (comboBox_imageBackground.SelectedIndex >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[comboBox_imageBackground.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_background.Value,
                            (int)numericUpDownY_background.Value, src.Width, src.Height));
                    }

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int offsetX = (int)numericUpDown_offsetX.Value;
                    int offsetY = (int)numericUpDown_offsetY.Value;
                    int image_index = comboBox_image.SelectedIndex;
                    float startAngle = (float)(numericUpDown_startAngle.Value);
                    float endAngle = (float)(numericUpDown_endAngle.Value);

                    float position = (float)((Watch_Face_Preview_Set.Weather.Temperature + 25) / 60f);
                    float angle = startAngle + position * (endAngle - startAngle);
                    if (Watch_Face_Preview_Set.Weather.Temperature > 35) angle = endAngle;
                    DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                    if (comboBox_imageCentr.SelectedIndex >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[comboBox_imageCentr.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_centr.Value,
                            (int)numericUpDownY_centr.Value, src.Width, src.Height));
                    }
                }
            }

            #endregion


            #endregion

            #region цифровое время
            int time_offsetX = -1;
            int time_offsetY = -1;
            bool _pm = false;
            // часы
            if (checkBox_Hour_Use_AOD.Checked && comboBox_Hour_image_AOD.SelectedIndex >= 0)
            {
                int imageIndex = comboBox_Hour_image_AOD.SelectedIndex;
                int x = (int)numericUpDown_HourX_AOD.Value;
                int y = (int)numericUpDown_HourY_AOD.Value;
                time_offsetY = y;
                int spasing = (int)numericUpDown_Hour_spacing_AOD.Value;
                int alignment = comboBox_Hour_alignment_AOD.SelectedIndex;
                bool addZero = checkBox_Hour_add_zero_AOD.Checked;
                //addZero = true;
                int value = Watch_Face_Preview_Set.Time.Hours;
                int separator_index = -1;
                if (comboBox_Hour_separator_AOD.SelectedIndex >= 0) separator_index = comboBox_Hour_separator_AOD.SelectedIndex;

                if (checkBox_12h_Use_AOD.Checked && Program_Settings.ShowIn12hourFormat)
                {
                    if (comboBox_AM_image_AOD.SelectedIndex >= 0 && comboBox_PM_image_AOD.SelectedIndex >= 0)
                    {
                        if (value > 11)
                        {
                            _pm = true;
                            value -= 12;
                        }
                        if (value == 0) value = 12;
                    }
                }
                time_offsetX = Draw_dagital_text(gPanel, imageIndex, x, y,
                    spasing, alignment, value, addZero, 2, separator_index, BBorder);

                if (comboBox_Hour_unit_AOD.SelectedIndex >= 0)
                {
                    src = OpenFileStream(ListImagesFullName[comboBox_Hour_unit_AOD.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Hour_unitX_AOD.Value,
                        (int)numericUpDown_Hour_unitY_AOD.Value, src.Width, src.Height));
                }
            }

            // минуты
            if (checkBox_Minute_Use_AOD.Checked && comboBox_Minute_image_AOD.SelectedIndex >= 0)
            {
                int imageIndex = comboBox_Minute_image_AOD.SelectedIndex;
                int x = (int)numericUpDown_MinuteX_AOD.Value;
                int y = (int)numericUpDown_MinuteY_AOD.Value;
                int spasing = (int)numericUpDown_Minute_spacing_AOD.Value;
                int alignment = comboBox_Minute_alignment_AOD.SelectedIndex;
                bool addZero = checkBox_Minute_add_zero_AOD.Checked;
                addZero = true;
                int value = Watch_Face_Preview_Set.Time.Minutes;
                int separator_index = -1;
                if (comboBox_Minute_separator_AOD.SelectedIndex >= 0) separator_index = comboBox_Minute_separator_AOD.SelectedIndex;
                if (checkBox_Minute_follow_AOD.Checked && time_offsetX > -1)
                {
                    x = time_offsetX;
                    alignment = 0;
                    y = time_offsetY;
                }
                time_offsetY = y;
                time_offsetX = Draw_dagital_text(gPanel, imageIndex, x, y,
                    spasing, alignment, value, addZero, 2, separator_index, BBorder);

                if (comboBox_Minute_unit_AOD.SelectedIndex >= 0)
                {
                    src = OpenFileStream(ListImagesFullName[comboBox_Minute_unit_AOD.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Minute_unitX_AOD.Value,
                        (int)numericUpDown_Minute_unitY_AOD.Value, src.Width, src.Height));
                }
            }
            else time_offsetX = -1;

            // AM/PM
            if (checkBox_12h_Use_AOD.Checked && Program_Settings.ShowIn12hourFormat)
            {
                if (comboBox_AM_image_AOD.SelectedIndex >= 0 && comboBox_PM_image_AOD.SelectedIndex >= 0)
                {
                    if (_pm)
                    {
                        src = OpenFileStream(ListImagesFullName[comboBox_PM_image_AOD.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_PM_X_AOD.Value,
                            (int)numericUpDown_PM_Y_AOD.Value, src.Width, src.Height));
                    }
                    else
                    {
                        src = OpenFileStream(ListImagesFullName[comboBox_AM_image_AOD.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_AM_X_AOD.Value,
                            (int)numericUpDown_AM_Y_AOD.Value, src.Width, src.Height));
                    }
                }
            }
            #endregion

            #region аналоговое время

            // часы
            if (checkBox_Hour_hand_Use_AOD.Checked && comboBox_Hour_hand_image_AOD.SelectedIndex >= 0)
            {
                int x = (int)numericUpDown_Hour_handX_AOD.Value;
                int y = (int)numericUpDown_Hour_handY_AOD.Value;
                int offsetX = (int)numericUpDown_Hour_handX_offset_AOD.Value;
                int offsetY = (int)numericUpDown_Hour_handY_offset_AOD.Value;
                int image_index = comboBox_Hour_hand_image_AOD.SelectedIndex;
                int hour = Watch_Face_Preview_Set.Time.Hours;
                int min = Watch_Face_Preview_Set.Time.Minutes;
                //int sec = Watch_Face_Preview_Set.TimeW.Seconds;
                if (hour >= 12) hour = hour - 12;
                float angle = 360 * hour / 12 + 360 * min / (60 * 12);
                DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                if (comboBox_Hour_hand_imageCentr_AOD.SelectedIndex >= 0)
                {
                    src = OpenFileStream(ListImagesFullName[comboBox_Hour_hand_imageCentr_AOD.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Hour_handX_centr_AOD.Value,
                        (int)numericUpDown_Hour_handY_centr_AOD.Value, src.Width, src.Height));
                }
            }

            // минуты
            if (checkBox_Minute_hand_Use_AOD.Checked && comboBox_Minute_hand_image_AOD.SelectedIndex >= 0)
            {
                int x = (int)numericUpDown_Minute_handX_AOD.Value;
                int y = (int)numericUpDown_Minute_handY_AOD.Value;
                int offsetX = (int)numericUpDown_Minute_handX_offset_AOD.Value;
                int offsetY = (int)numericUpDown_Minute_handY_offset_AOD.Value;
                int image_index = comboBox_Minute_hand_image_AOD.SelectedIndex;
                int min = Watch_Face_Preview_Set.Time.Minutes;
                float angle = 360 * min / 60;
                DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                if (comboBox_Minute_hand_imageCentr_AOD.SelectedIndex >= 0)
                {
                    src = OpenFileStream(ListImagesFullName[comboBox_Minute_hand_imageCentr_AOD.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Minute_handX_centr_AOD.Value,
                        (int)numericUpDown_Minute_handY_centr_AOD.Value, src.Width, src.Height));
                }
            }
            #endregion


            #region Mesh
            Logger.WriteLine("Preview_AOD (Mesh)");

            if (WMesh)
            {
                Pen pen = new Pen(Color.White, 2);
                int LineDistance = 30;
                if (scale >= 1) LineDistance = 15;
                if (scale >= 2) LineDistance = 10;
                if (scale >= 2) pen.Width = 1;
                //if (panel_Preview.Height > 300) LineDistance = 15;
                //if (panel_Preview.Height > 690) LineDistance = 10;
                for (i = 0; i < 30; i++)
                {
                    gPanel.DrawLine(pen, new Point(offSet_X + i * LineDistance, 0), new Point(offSet_X + i * LineDistance, 454));
                    gPanel.DrawLine(pen, new Point(offSet_X - i * LineDistance, 0), new Point(offSet_X - i * LineDistance, 454));

                    gPanel.DrawLine(pen, new Point(0, offSet_Y + i * LineDistance), new Point(454, offSet_Y + i * LineDistance));
                    gPanel.DrawLine(pen, new Point(0, offSet_Y - i * LineDistance), new Point(454, offSet_Y - i * LineDistance));

                    if (i == 0) pen.Width = pen.Width / 3f;
                }
            }

            if (BMesh)
            {
                Pen pen = new Pen(Color.Black, 2);
                int LineDistance = 30;
                if (scale >= 1) LineDistance = 15;
                if (scale >= 2) LineDistance = 10;
                if (scale >= 2) pen.Width = 1;
                //if (panel_Preview.Height > 300) LineDistance = 15;
                //if (panel_Preview.Height > 690) LineDistance = 10;
                for (i = 0; i < 30; i++)
                {
                    gPanel.DrawLine(pen, new Point(offSet_X + i * LineDistance, 0), new Point(offSet_X + i * LineDistance, 454));
                    gPanel.DrawLine(pen, new Point(offSet_X - i * LineDistance, 0), new Point(offSet_X - i * LineDistance, 454));

                    gPanel.DrawLine(pen, new Point(0, offSet_Y + i * LineDistance), new Point(454, offSet_Y + i * LineDistance));
                    gPanel.DrawLine(pen, new Point(0, offSet_Y - i * LineDistance), new Point(454, offSet_Y - i * LineDistance));

                    if (i == 0) pen.Width = pen.Width / 3f;
                }
            }
            #endregion
            src.Dispose();

            if (crop)
            {
                Logger.WriteLine("Preview_AOD (crop)");
                Bitmap mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gtr_2.png");
                if (radioButton_GTS2.Checked)
                {
                    mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gts_2.png");
                }
                mask = FormColor(mask);
                gPanel.DrawImage(mask, new Rectangle(0, 0, mask.Width, mask.Height));
                mask.Dispose();
            }

            FormText();
            Logger.WriteLine("* Preview_AOD (end)");
        }
    }
}
