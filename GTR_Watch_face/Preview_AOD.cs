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
            if (Watch_Face == null) return;
            if (Watch_Face.ScreenIdle == null)
            {
                Preview_AOD_WithoutScreenIdle(gPanel, scale, crop, WMesh, BMesh, BBorder,
            showShortcuts, showShortcutsArea, showShortcutsBorder, showAnimation, showProgressArea,
            showCentrHend);
                return;
            }

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
            if (radioButton_TRex_pro.Checked)
            {
                src = OpenFileStream(Application.StartupPath + @"\Mask\mask_trex_pro.png");
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

            UserControl_pictures userPanel_pictures;
            UserControl_text userPanel_text;
            UserControl_hand userPanel_hand;
            UserControl_scaleCircle userPanel_scaleCircle;
            UserControl_scaleLinear userPanel_scaleLinear;

            #region зараяд
            userPanel_pictures = userControl_pictures_Battery_AOD;
            userPanel_text = userControl_text_Battery_AOD;
            userPanel_hand = userControl_hand_Battery_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_Battery_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_Battery_AOD;
            // зараяд картинками
            if (userPanel_pictures.checkBox_pictures_Use.Checked)
            {
                if (userPanel_pictures.comboBoxGetImage() >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_pictures.numericUpDown_picturesX;
                    NumericUpDown numericUpDownY = userPanel_pictures.numericUpDown_picturesY;
                    NumericUpDown numericUpDown_count = userPanel_pictures.numericUpDown_pictures_count;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int count = (int)numericUpDown_count.Value;
                    //int offSet = (int)Math.Ceiling((float)count * Watch_Face_Preview_Set.Activity.Steps / Watch_Face_Preview_Set.Activity.StepsGoal);
                    int offSet = (int)(count * Watch_Face_Preview_Set.Battery / 100f);
                    //offSet--;
                    if (offSet < 0) offSet = 0;
                    if (offSet >= count) offSet = (int)(count - 1);
                    //int offSet = (int)Math.Round(count * Watch_Face_Preview_Set.Battery / 100f, 0);
                    int imageIndex = userPanel_pictures.comboBoxGetSelectedIndexImage() + offSet;

                    if (imageIndex < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[imageIndex]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }
                }
            }

            // зараяд круговой шкалой
            if (userPanel_scaleCircle.checkBox_scaleCircle_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleCircle.radioButton_scaleCircle_image;
                NumericUpDown numericUpDownX = userPanel_scaleCircle.numericUpDown_scaleCircleX;
                NumericUpDown numericUpDownY = userPanel_scaleCircle.numericUpDown_scaleCircleY;
                NumericUpDown numericUpDown_radius = userPanel_scaleCircle.numericUpDown_scaleCircle_radius;
                NumericUpDown numericUpDown_width = userPanel_scaleCircle.numericUpDown_scaleCircle_width;
                NumericUpDown numericUpDown_startAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_startAngle;
                NumericUpDown numericUpDown_endAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_endAngle;

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                float width = (float)numericUpDown_width.Value;
                int radius = (int)numericUpDown_radius.Value;
                int imageIndex = userPanel_scaleCircle.comboBoxGetSelectedIndexImage();
                int imageBackground = userPanel_scaleCircle.comboBoxGetSelectedIndexImageBackground();
                float StartAngle = (float)numericUpDown_startAngle.Value - 90;
                float EndAngle = (float)(numericUpDown_endAngle.Value -
                    numericUpDown_startAngle.Value);
                Color color = userPanel_scaleCircle.comboBoxGetColor();
                float position = (float)Watch_Face_Preview_Set.Battery / 100f;
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleCircle.comboBoxGetFlatness();
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
            if (userPanel_scaleLinear.checkBox_scaleLinear_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleLinear.radioButton_scaleLinear_image;
                NumericUpDown numericUpDownX = userPanel_scaleLinear.numericUpDown_scaleLinearX;
                NumericUpDown numericUpDownY = userPanel_scaleLinear.numericUpDown_scaleLinearY;
                NumericUpDown numericUpDown_length = userPanel_scaleLinear.numericUpDown_scaleLinear_length;
                NumericUpDown numericUpDown_width = userPanel_scaleLinear.numericUpDown_scaleLinear_width;

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                int imageIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImage();
                int pointerIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImagePointer();
                int backgroundIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImageBackground();
                int length = (int)numericUpDown_length.Value;
                int width = (int)numericUpDown_width.Value;
                Color color = userPanel_scaleLinear.comboBoxGetColor();
                float position = Watch_Face_Preview_Set.Battery / 100f;
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleLinear.comboBoxGetFlatness();

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
            if (userPanel_text.checkBox_Use.Checked)
            {
                int imageIndex = userPanel_text.comboBoxGetSelectedIndexImage();
                int unit = userPanel_text.comboBoxGetSelectedIndexIcon();
                NumericUpDown numericUpDownX = userPanel_text.numericUpDown_imageX;
                NumericUpDown numericUpDownY = userPanel_text.numericUpDown_imageY;
                NumericUpDown numericUpDown_unitX = userPanel_text.numericUpDown_iconX;
                NumericUpDown numericUpDown_unitY = userPanel_text.numericUpDown_iconY;
                //ComboBox comboBox_alignment = (ComboBox)panel_text.Controls[8];
                NumericUpDown numericUpDown_spacing = userPanel_text.numericUpDown_spacing;
                CheckBox checkBox_add_zero = userPanel_text.checkBox_addZero;
                //ComboBox comboBox_imageError = (ComboBox)panel_text.Controls[11];

                if (imageIndex >= 0)
                {
                    //int imageIndex = comboBox_image.SelectedIndex;
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = userPanel_text.comboBoxGetSelectedIndexAlignment();
                    bool addZero = checkBox_add_zero.Checked;
                    int value = Watch_Face_Preview_Set.Battery;
                    int separator_index = userPanel_text.comboBoxGetSelectedIndexUnit();
                    Draw_dagital_text(gPanel, imageIndex, x, y,
                        spasing, alignment, value, addZero, 3, separator_index, BBorder);

                    if (unit >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[unit]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // зараяд стрелкой
            if (userPanel_hand.checkBox_hand_Use.Checked)
            {
                int image_index = userPanel_hand.comboBoxGetSelectedIndexHandImage();
                if (image_index >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_hand.numericUpDown_handX;
                    NumericUpDown numericUpDownY = userPanel_hand.numericUpDown_handY;
                    NumericUpDown numericUpDown_offsetX = userPanel_hand.numericUpDown_handX_offset;
                    NumericUpDown numericUpDown_offsetY = userPanel_hand.numericUpDown_handY_offset;
                    int imageCentr = userPanel_hand.comboBoxGetSelectedIndexHandImageCentr();
                    NumericUpDown numericUpDownX_centr = userPanel_hand.numericUpDown_handX_centr;
                    NumericUpDown numericUpDownY_centr = userPanel_hand.numericUpDown_handY_centr;
                    NumericUpDown numericUpDown_startAngle = userPanel_hand.numericUpDown_hand_startAngle;
                    NumericUpDown numericUpDown_endAngle = userPanel_hand.numericUpDown_hand_endAngle;
                    int imageBackground = userPanel_hand.comboBoxGetSelectedIndexHandImageBackground();
                    NumericUpDown numericUpDownX_background = userPanel_hand.numericUpDown_handX_background;
                    NumericUpDown numericUpDownY_background = userPanel_hand.numericUpDown_handY_background;

                    if (imageBackground >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[imageBackground]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_background.Value,
                            (int)numericUpDownY_background.Value, src.Width, src.Height));
                    }

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int offsetX = (int)numericUpDown_offsetX.Value;
                    int offsetY = (int)numericUpDown_offsetY.Value;
                    //int image_index = comboBox_image.SelectedIndex;
                    float startAngle = (float)(numericUpDown_startAngle.Value);
                    float endAngle = (float)(numericUpDown_endAngle.Value);

                    float angle = startAngle + Watch_Face_Preview_Set.Battery * (endAngle - startAngle) / 100;
                    DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                    if (imageCentr >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[imageCentr]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_centr.Value,
                            (int)numericUpDownY_centr.Value, src.Width, src.Height));
                    }
                }
            }
            #endregion

            #region шаги
            userPanel_pictures = userControl_pictures_Steps_AOD;
            userPanel_text = userControl_text_Steps_AOD;
            userPanel_hand = userControl_hand_Steps_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_Steps_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_Steps_AOD;
            // шаги картинками
            if (userPanel_pictures.checkBox_pictures_Use.Checked)
            {
                //ComboBox comboBox_image = (ComboBox)panel_pictures.Controls[1];
                if (userPanel_pictures.comboBoxGetImage() >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_pictures.numericUpDown_picturesX;
                    NumericUpDown numericUpDownY = userPanel_pictures.numericUpDown_picturesY;
                    NumericUpDown numericUpDown_count = userPanel_pictures.numericUpDown_pictures_count;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int count = (int)numericUpDown_count.Value;
                    //int offSet = (int)Math.Ceiling((float)count * Watch_Face_Preview_Set.Activity.Steps / Watch_Face_Preview_Set.Activity.StepsGoal);
                    int offSet = (int)((count - 1f) * Watch_Face_Preview_Set.Activity.Steps / Watch_Face_Preview_Set.Activity.StepsGoal);
                    //offSet--;
                    if (offSet < 0) offSet = 0;
                    if (offSet >= count) offSet = (int)(count - 1);
                    //int offSet = (int)Math.Round(count * Watch_Face_Preview_Set.Battery / 100f, 0);
                    int imageIndex = userPanel_pictures.comboBoxGetSelectedIndexImage() + offSet;

                    if (imageIndex < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[imageIndex]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }
                }
            }

            // шаги круговой шкалой
            if (userPanel_scaleCircle.checkBox_scaleCircle_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleCircle.radioButton_scaleCircle_image;
                NumericUpDown numericUpDownX = userPanel_scaleCircle.numericUpDown_scaleCircleX;
                NumericUpDown numericUpDownY = userPanel_scaleCircle.numericUpDown_scaleCircleY;
                NumericUpDown numericUpDown_radius = userPanel_scaleCircle.numericUpDown_scaleCircle_radius;
                NumericUpDown numericUpDown_width = userPanel_scaleCircle.numericUpDown_scaleCircle_width;
                NumericUpDown numericUpDown_startAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_startAngle;
                NumericUpDown numericUpDown_endAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_endAngle;

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                float width = (float)numericUpDown_width.Value;
                int radius = (int)numericUpDown_radius.Value;
                int imageIndex = userPanel_scaleCircle.comboBoxGetSelectedIndexImage();
                int imageBackground = userPanel_scaleCircle.comboBoxGetSelectedIndexImageBackground();
                float StartAngle = (float)numericUpDown_startAngle.Value - 90;
                float EndAngle = (float)(numericUpDown_endAngle.Value -
                    numericUpDown_startAngle.Value);
                Color color = userPanel_scaleCircle.comboBoxGetColor();
                float position = (float)Watch_Face_Preview_Set.Activity.Steps / Watch_Face_Preview_Set.Activity.StepsGoal;
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleCircle.comboBoxGetFlatness();
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
            if (userPanel_scaleLinear.checkBox_scaleLinear_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleLinear.radioButton_scaleLinear_image;
                NumericUpDown numericUpDownX = userPanel_scaleLinear.numericUpDown_scaleLinearX;
                NumericUpDown numericUpDownY = userPanel_scaleLinear.numericUpDown_scaleLinearY;
                NumericUpDown numericUpDown_length = userPanel_scaleLinear.numericUpDown_scaleLinear_length;
                NumericUpDown numericUpDown_width = userPanel_scaleLinear.numericUpDown_scaleLinear_width;

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                int imageIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImage();
                int pointerIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImagePointer();
                int backgroundIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImageBackground();
                int length = (int)numericUpDown_length.Value;
                int width = (int)numericUpDown_width.Value;
                Color color = userPanel_scaleLinear.comboBoxGetColor();
                float position = (float)Watch_Face_Preview_Set.Activity.Steps / Watch_Face_Preview_Set.Activity.StepsGoal;
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleLinear.comboBoxGetFlatness();

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
            if (userPanel_text.checkBox_Use.Checked)
            {
                int imageIndex = userPanel_text.comboBoxGetSelectedIndexImage();
                int unit = userPanel_text.comboBoxGetSelectedIndexIcon();
                NumericUpDown numericUpDownX = userPanel_text.numericUpDown_imageX;
                NumericUpDown numericUpDownY = userPanel_text.numericUpDown_imageY;
                NumericUpDown numericUpDown_unitX = userPanel_text.numericUpDown_iconX;
                NumericUpDown numericUpDown_unitY = userPanel_text.numericUpDown_iconY;
                NumericUpDown numericUpDown_spacing = userPanel_text.numericUpDown_spacing;
                CheckBox checkBox_add_zero = userPanel_text.checkBox_addZero;

                if (imageIndex >= 0)
                {
                    //int imageIndex = comboBox_image.SelectedIndex;
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = userPanel_text.comboBoxGetSelectedIndexAlignment();
                    bool addZero = checkBox_add_zero.Checked;
                    int value = Watch_Face_Preview_Set.Activity.Steps;
                    int separator_index = userPanel_text.comboBoxGetSelectedIndexUnit();
                    Draw_dagital_text(gPanel, imageIndex, x, y,
                        spasing, alignment, value, addZero, 5, separator_index, BBorder);

                    if (unit >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[unit]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // шаги стрелкой
            if (userPanel_hand.checkBox_hand_Use.Checked)
            {
                int image_index = userPanel_hand.comboBoxGetSelectedIndexHandImage();
                if (image_index >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_hand.numericUpDown_handX;
                    NumericUpDown numericUpDownY = userPanel_hand.numericUpDown_handY;
                    NumericUpDown numericUpDown_offsetX = userPanel_hand.numericUpDown_handX_offset;
                    NumericUpDown numericUpDown_offsetY = userPanel_hand.numericUpDown_handY_offset;
                    int imageCentr = userPanel_hand.comboBoxGetSelectedIndexHandImageCentr();
                    NumericUpDown numericUpDownX_centr = userPanel_hand.numericUpDown_handX_centr;
                    NumericUpDown numericUpDownY_centr = userPanel_hand.numericUpDown_handY_centr;
                    NumericUpDown numericUpDown_startAngle = userPanel_hand.numericUpDown_hand_startAngle;
                    NumericUpDown numericUpDown_endAngle = userPanel_hand.numericUpDown_hand_endAngle;
                    int imageBackground = userPanel_hand.comboBoxGetSelectedIndexHandImageBackground();
                    NumericUpDown numericUpDownX_background = userPanel_hand.numericUpDown_handX_background;
                    NumericUpDown numericUpDownY_background = userPanel_hand.numericUpDown_handY_background;

                    if (imageBackground >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[imageBackground]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_background.Value,
                            (int)numericUpDownY_background.Value, src.Width, src.Height));
                    }

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int offsetX = (int)numericUpDown_offsetX.Value;
                    int offsetY = (int)numericUpDown_offsetY.Value;
                    //int image_index = comboBox_image.SelectedIndex;
                    float startAngle = (float)(numericUpDown_startAngle.Value);
                    float endAngle = (float)(numericUpDown_endAngle.Value);

                    float angle = startAngle + Watch_Face_Preview_Set.Activity.Steps * (endAngle - startAngle) /
                    Watch_Face_Preview_Set.Activity.StepsGoal;
                    if (Watch_Face_Preview_Set.Activity.Steps > Watch_Face_Preview_Set.Activity.StepsGoal) angle = endAngle;
                    DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                    if (imageCentr >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[imageCentr]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_centr.Value,
                            (int)numericUpDownY_centr.Value, src.Width, src.Height));
                    }
                }
            }
            #endregion

            #region калории
            userPanel_pictures = userControl_pictures_Calories_AOD;
            userPanel_text = userControl_text_Calories_AOD;
            userPanel_hand = userControl_hand_Calories_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_Calories_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_Calories_AOD;
            // калории картинками
            if (userPanel_pictures.checkBox_pictures_Use.Checked)
            {
                if (userPanel_pictures.comboBoxGetImage() >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_pictures.numericUpDown_picturesX;
                    NumericUpDown numericUpDownY = userPanel_pictures.numericUpDown_picturesY;
                    NumericUpDown numericUpDown_count = userPanel_pictures.numericUpDown_pictures_count;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int count = (int)numericUpDown_count.Value;
                    //int offSet = (int)Math.Ceiling((float)count * Watch_Face_Preview_Set.Activity.Calories / 300f);
                    int offSet = (int)((count - 1f) * Watch_Face_Preview_Set.Activity.Calories / 300f);
                    //offSet--;
                    if (offSet < 0) offSet = 0;
                    if (offSet >= count) offSet = (int)(count - 1);
                    //int offSet = (int)Math.Round(count * Watch_Face_Preview_Set.Battery / 100f, 0);
                    int imageIndex = userPanel_pictures.comboBoxGetSelectedIndexImage() + offSet;

                    if (imageIndex < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[imageIndex]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }
                }
            }

            // калории круговой шкалой
            if (userPanel_scaleCircle.checkBox_scaleCircle_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleCircle.radioButton_scaleCircle_image;
                NumericUpDown numericUpDownX = userPanel_scaleCircle.numericUpDown_scaleCircleX;
                NumericUpDown numericUpDownY = userPanel_scaleCircle.numericUpDown_scaleCircleY;
                NumericUpDown numericUpDown_radius = userPanel_scaleCircle.numericUpDown_scaleCircle_radius;
                NumericUpDown numericUpDown_width = userPanel_scaleCircle.numericUpDown_scaleCircle_width;
                NumericUpDown numericUpDown_startAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_startAngle;
                NumericUpDown numericUpDown_endAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_endAngle;

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                float width = (float)numericUpDown_width.Value;
                int radius = (int)numericUpDown_radius.Value;
                int imageIndex = userPanel_scaleCircle.comboBoxGetSelectedIndexImage();
                int imageBackground = userPanel_scaleCircle.comboBoxGetSelectedIndexImageBackground();
                float StartAngle = (float)numericUpDown_startAngle.Value - 90;
                float EndAngle = (float)(numericUpDown_endAngle.Value -
                    numericUpDown_startAngle.Value);
                Color color = userPanel_scaleCircle.comboBoxGetColor();
                float position = (float)Watch_Face_Preview_Set.Activity.Calories / 300f;
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleCircle.comboBoxGetFlatness();
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
            if (userPanel_scaleLinear.checkBox_scaleLinear_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleLinear.radioButton_scaleLinear_image;
                NumericUpDown numericUpDownX = userPanel_scaleLinear.numericUpDown_scaleLinearX;
                NumericUpDown numericUpDownY = userPanel_scaleLinear.numericUpDown_scaleLinearY;
                NumericUpDown numericUpDown_length = userPanel_scaleLinear.numericUpDown_scaleLinear_length;
                NumericUpDown numericUpDown_width = userPanel_scaleLinear.numericUpDown_scaleLinear_width;

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                int imageIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImage();
                int pointerIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImagePointer();
                int backgroundIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImageBackground();
                int length = (int)numericUpDown_length.Value;
                int width = (int)numericUpDown_width.Value;
                Color color = userPanel_scaleLinear.comboBoxGetColor();
                float position = (float)Watch_Face_Preview_Set.Activity.Calories / 300f;
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleLinear.comboBoxGetFlatness();

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

            // калории надписью
            if (userPanel_text.checkBox_Use.Checked)
            {
                int imageIndex = userPanel_text.comboBoxGetSelectedIndexImage();
                int unit = userPanel_text.comboBoxGetSelectedIndexIcon();
                NumericUpDown numericUpDownX = userPanel_text.numericUpDown_imageX;
                NumericUpDown numericUpDownY = userPanel_text.numericUpDown_imageY;
                NumericUpDown numericUpDown_unitX = userPanel_text.numericUpDown_iconX;
                NumericUpDown numericUpDown_unitY = userPanel_text.numericUpDown_iconY;
                //ComboBox comboBox_alignment = (ComboBox)panel_text.Controls[8];
                NumericUpDown numericUpDown_spacing = userPanel_text.numericUpDown_spacing;
                CheckBox checkBox_add_zero = userPanel_text.checkBox_addZero;
                //ComboBox comboBox_imageError = (ComboBox)panel_text.Controls[11];

                if (imageIndex >= 0)
                {
                    //int imageIndex = comboBox_image.SelectedIndex;
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = userPanel_text.comboBoxGetSelectedIndexAlignment();
                    bool addZero = checkBox_add_zero.Checked;
                    int value = Watch_Face_Preview_Set.Activity.Calories;
                    int separator_index = userPanel_text.comboBoxGetSelectedIndexUnit();
                    Draw_dagital_text(gPanel, imageIndex, x, y,
                        spasing, alignment, value, addZero, 4, separator_index, BBorder);

                    if (unit >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[unit]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // калории стрелкой
            if (userPanel_hand.checkBox_hand_Use.Checked)
            {
                int image_index = userPanel_hand.comboBoxGetSelectedIndexHandImage();
                if (image_index >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_hand.numericUpDown_handX;
                    NumericUpDown numericUpDownY = userPanel_hand.numericUpDown_handY;
                    NumericUpDown numericUpDown_offsetX = userPanel_hand.numericUpDown_handX_offset;
                    NumericUpDown numericUpDown_offsetY = userPanel_hand.numericUpDown_handY_offset;
                    int imageCentr = userPanel_hand.comboBoxGetSelectedIndexHandImageCentr();
                    NumericUpDown numericUpDownX_centr = userPanel_hand.numericUpDown_handX_centr;
                    NumericUpDown numericUpDownY_centr = userPanel_hand.numericUpDown_handY_centr;
                    NumericUpDown numericUpDown_startAngle = userPanel_hand.numericUpDown_hand_startAngle;
                    NumericUpDown numericUpDown_endAngle = userPanel_hand.numericUpDown_hand_endAngle;
                    int imageBackground = userPanel_hand.comboBoxGetSelectedIndexHandImageBackground();
                    NumericUpDown numericUpDownX_background = userPanel_hand.numericUpDown_handX_background;
                    NumericUpDown numericUpDownY_background = userPanel_hand.numericUpDown_handY_background;

                    if (imageBackground >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[imageBackground]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_background.Value,
                            (int)numericUpDownY_background.Value, src.Width, src.Height));
                    }

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int offsetX = (int)numericUpDown_offsetX.Value;
                    int offsetY = (int)numericUpDown_offsetY.Value;
                    //int image_index = comboBox_image.SelectedIndex;
                    float startAngle = (float)(numericUpDown_startAngle.Value);
                    float endAngle = (float)(numericUpDown_endAngle.Value);

                    float angle = startAngle + Watch_Face_Preview_Set.Activity.Calories * (endAngle - startAngle) / 300f;
                    if (Watch_Face_Preview_Set.Activity.Calories > 300) angle = endAngle;
                    DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                    if (imageCentr >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[imageCentr]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_centr.Value,
                            (int)numericUpDownY_centr.Value, src.Width, src.Height));
                    }
                }
            }
            #endregion

            #region пульс
            userPanel_pictures = userControl_pictures_HeartRate_AOD;
            userPanel_text = userControl_text_HeartRate_AOD;
            userPanel_hand = userControl_hand_HeartRate_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_HeartRate_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_HeartRate_AOD;
            // пульс картинками
            //checkBox_Use = (CheckBox)panel_pictures.Controls[0];
            if (userPanel_pictures.checkBox_pictures_Use.Checked)
            {
                //ComboBox comboBox_image = (ComboBox)panel_pictures.Controls[1];
                if (userPanel_pictures.comboBoxGetImage() >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_pictures.numericUpDown_picturesX;
                    NumericUpDown numericUpDownY = userPanel_pictures.numericUpDown_picturesY;
                    NumericUpDown numericUpDown_count = userPanel_pictures.numericUpDown_pictures_count;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int count = (int)numericUpDown_count.Value;
                    //int offSet = (int)Math.Ceiling((float)count * Watch_Face_Preview_Set.Activity.HeartRate / 200f);
                    //int offSet = (int)((count - 1f) * (Watch_Face_Preview_Set.Activity.HeartRate - 70) / 100f);
                    int offSet = (int)(count * (Watch_Face_Preview_Set.Activity.HeartRate - 70) / 100f);
                    //offSet--;
                    if (offSet < 0) offSet = 0;
                    if (offSet >= count) offSet = (int)(count - 1);
                    int imageIndex = userPanel_pictures.comboBoxGetSelectedIndexImage() + offSet;

                    if (imageIndex < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[imageIndex]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }
                }
            }

            // пульс круговой шкалой
            //checkBox_Use = (CheckBox)panel_scaleCircle.Controls[0];
            if (userPanel_scaleCircle.checkBox_scaleCircle_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleCircle.radioButton_scaleCircle_image;
                //RadioButton radioButton_color = (RadioButton)panel_scaleCircle.Controls[2];
                //int image = userPanel_scaleCircle.comboBoxGetImage();
                //string color = userPanel_scaleCircle.comboBoxGetColorString();
                //int flatness = userPanel_scaleCircle.comboBoxGetFlatness();
                //int background = userPanel_scaleCircle.comboBoxGetImageBackground();
                NumericUpDown numericUpDownX = userPanel_scaleCircle.numericUpDown_scaleCircleX;
                NumericUpDown numericUpDownY = userPanel_scaleCircle.numericUpDown_scaleCircleY;
                NumericUpDown numericUpDown_radius = userPanel_scaleCircle.numericUpDown_scaleCircle_radius;
                NumericUpDown numericUpDown_width = userPanel_scaleCircle.numericUpDown_scaleCircle_width;
                NumericUpDown numericUpDown_startAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_startAngle;
                NumericUpDown numericUpDown_endAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_endAngle;

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                float width = (float)numericUpDown_width.Value;
                int radius = (int)numericUpDown_radius.Value;
                int imageIndex = userPanel_scaleCircle.comboBoxGetSelectedIndexImage();
                int imageBackground = userPanel_scaleCircle.comboBoxGetSelectedIndexImageBackground();
                float StartAngle = (float)numericUpDown_startAngle.Value - 90;
                float EndAngle = (float)(numericUpDown_endAngle.Value -
                    numericUpDown_startAngle.Value);
                Color color = userPanel_scaleCircle.comboBoxGetColor();
                float position = (float)Watch_Face_Preview_Set.Activity.HeartRate / 180f;
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleCircle.comboBoxGetFlatness();
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
            //checkBox_Use = (CheckBox)panel_scaleLinear.Controls[0];
            if (userPanel_scaleLinear.checkBox_scaleLinear_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleLinear.radioButton_scaleLinear_image;
                //RadioButton radioButton_color = (RadioButton)panel_scaleLinear.Controls[2];
                //ComboBox comboBox_image = (ComboBox)panel_scaleLinear.Controls[3];
                //ComboBox comboBox_color = (ComboBox)panel_scaleLinear.Controls[4];
                //ComboBox comboBox_pointer = (ComboBox)panel_scaleLinear.Controls[5];
                //ComboBox comboBox_background = (ComboBox)panel_scaleLinear.Controls[6];
                NumericUpDown numericUpDownX = userPanel_scaleLinear.numericUpDown_scaleLinearX;
                NumericUpDown numericUpDownY = userPanel_scaleLinear.numericUpDown_scaleLinearY;
                NumericUpDown numericUpDown_length = userPanel_scaleLinear.numericUpDown_scaleLinear_length;
                NumericUpDown numericUpDown_width = userPanel_scaleLinear.numericUpDown_scaleLinear_width;
                //ComboBox comboBox_flatness = (ComboBox)panel_scaleLinear.Controls[11];

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                int imageIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImage();
                int pointerIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImagePointer();
                int backgroundIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImageBackground();
                int length = (int)numericUpDown_length.Value;
                int width = (int)numericUpDown_width.Value;
                Color color = userPanel_scaleLinear.comboBoxGetColor();
                float position = (float)Watch_Face_Preview_Set.Activity.HeartRate / 180f;
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleLinear.comboBoxGetFlatness();

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

            // пульс надписью
            //checkBox_Use = (CheckBox)panel_text.Controls[0];
            if (userPanel_text.checkBox_Use.Checked)
            {
                //ComboBox comboBox_image = (ComboBox)panel_text.Controls[1];
                //ComboBox comboBox_unit = (ComboBox)panel_text.Controls[2]; icon
                //ComboBox comboBox_separator = (ComboBox)panel_text.Controls[3]; unit
                int imageIndex = userPanel_text.comboBoxGetSelectedIndexImage();
                int unit = userPanel_text.comboBoxGetSelectedIndexIcon();
                NumericUpDown numericUpDownX = userPanel_text.numericUpDown_imageX;
                NumericUpDown numericUpDownY = userPanel_text.numericUpDown_imageY;
                NumericUpDown numericUpDown_unitX = userPanel_text.numericUpDown_iconX;
                NumericUpDown numericUpDown_unitY = userPanel_text.numericUpDown_iconY;
                //ComboBox comboBox_alignment = (ComboBox)panel_text.Controls[8];
                NumericUpDown numericUpDown_spacing = userPanel_text.numericUpDown_spacing;
                CheckBox checkBox_add_zero = userPanel_text.checkBox_addZero;
                //ComboBox comboBox_imageError = (ComboBox)panel_text.Controls[11];

                if (imageIndex >= 0)
                {
                    //int imageIndex = comboBox_image.SelectedIndex;
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = userPanel_text.comboBoxGetSelectedIndexAlignment();
                    bool addZero = checkBox_add_zero.Checked;
                    int value = Watch_Face_Preview_Set.Activity.HeartRate;
                    int separator_index = userPanel_text.comboBoxGetSelectedIndexUnit();
                    Draw_dagital_text(gPanel, imageIndex, x, y,
                        spasing, alignment, value, addZero, 3, separator_index, BBorder);

                    if (unit >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[unit]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // пульс стрелкой
            //checkBox_Use = (CheckBox)panel_hand.Controls[0];
            if (userPanel_hand.checkBox_hand_Use.Checked)
            {
                int image_index = userPanel_hand.comboBoxGetSelectedIndexHandImage();
                if (image_index >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_hand.numericUpDown_handX;
                    NumericUpDown numericUpDownY = userPanel_hand.numericUpDown_handY;
                    NumericUpDown numericUpDown_offsetX = userPanel_hand.numericUpDown_handX_offset;
                    NumericUpDown numericUpDown_offsetY = userPanel_hand.numericUpDown_handY_offset;
                    int imageCentr = userPanel_hand.comboBoxGetSelectedIndexHandImageCentr();
                    NumericUpDown numericUpDownX_centr = userPanel_hand.numericUpDown_handX_centr;
                    NumericUpDown numericUpDownY_centr = userPanel_hand.numericUpDown_handY_centr;
                    NumericUpDown numericUpDown_startAngle = userPanel_hand.numericUpDown_hand_startAngle;
                    NumericUpDown numericUpDown_endAngle = userPanel_hand.numericUpDown_hand_endAngle;
                    int imageBackground = userPanel_hand.comboBoxGetSelectedIndexHandImageBackground();
                    NumericUpDown numericUpDownX_background = userPanel_hand.numericUpDown_handX_background;
                    NumericUpDown numericUpDownY_background = userPanel_hand.numericUpDown_handY_background;

                    if (imageBackground >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[imageBackground]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_background.Value,
                            (int)numericUpDownY_background.Value, src.Width, src.Height));
                    }

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int offsetX = (int)numericUpDown_offsetX.Value;
                    int offsetY = (int)numericUpDown_offsetY.Value;
                    //int image_index = comboBox_image.SelectedIndex;
                    float startAngle = (float)(numericUpDown_startAngle.Value);
                    float endAngle = (float)(numericUpDown_endAngle.Value);
                    //TODO определить пределы изменения пульса
                    //float position = (Watch_Face_Preview_Set.Activity.HeartRate) / 200f;
                    float position = (Watch_Face_Preview_Set.Activity.HeartRate) / 179f;
                    float angle = startAngle + position * (endAngle - startAngle);
                    if (position < 0) angle = startAngle;
                    if (position > 1) angle = endAngle;
                    DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                    if (imageCentr >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[imageCentr]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_centr.Value,
                            (int)numericUpDownY_centr.Value, src.Width, src.Height));
                    }
                }
            }
            #endregion

            #region PAI
            userPanel_pictures = userControl_pictures_PAI_AOD;
            userPanel_text = userControl_text_PAI_AOD;
            userPanel_hand = userControl_hand_PAI_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_PAI_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_PAI_AOD;
            // PAI картинками
            //checkBox_Use = (CheckBox)panel_pictures.Controls[0];
            if (userPanel_pictures.checkBox_pictures_Use.Checked)
            {
                //ComboBox comboBox_image = (ComboBox)panel_pictures.Controls[1];
                if (userPanel_pictures.comboBoxGetImage() >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_pictures.numericUpDown_picturesX;
                    NumericUpDown numericUpDownY = userPanel_pictures.numericUpDown_picturesY;
                    NumericUpDown numericUpDown_count = userPanel_pictures.numericUpDown_pictures_count;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int count = (int)numericUpDown_count.Value;
                    //int offSet = (int)Math.Ceiling((float)count * Watch_Face_Preview_Set.Activity.PAI / 100f);
                    int offSet = (int)((count + 1f) * Watch_Face_Preview_Set.Activity.PAI / 100f);
                    //offSet--;
                    if (offSet < 0) offSet = 0;
                    if (offSet >= count) offSet = (int)(count - 1);
                    int imageIndex = userPanel_pictures.comboBoxGetSelectedIndexImage() + offSet;

                    if (imageIndex < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[imageIndex]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }
                }
            }

            // PAI круговой шкалой
            //checkBox_Use = (CheckBox)panel_scaleCircle.Controls[0];
            if (userPanel_scaleCircle.checkBox_scaleCircle_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleCircle.radioButton_scaleCircle_image;
                //RadioButton radioButton_color = (RadioButton)panel_scaleCircle.Controls[2];
                //int image = userPanel_scaleCircle.comboBoxGetImage();
                //string color = userPanel_scaleCircle.comboBoxGetColorString();
                //int flatness = userPanel_scaleCircle.comboBoxGetFlatness();
                //int background = userPanel_scaleCircle.comboBoxGetImageBackground();
                NumericUpDown numericUpDownX = userPanel_scaleCircle.numericUpDown_scaleCircleX;
                NumericUpDown numericUpDownY = userPanel_scaleCircle.numericUpDown_scaleCircleY;
                NumericUpDown numericUpDown_radius = userPanel_scaleCircle.numericUpDown_scaleCircle_radius;
                NumericUpDown numericUpDown_width = userPanel_scaleCircle.numericUpDown_scaleCircle_width;
                NumericUpDown numericUpDown_startAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_startAngle;
                NumericUpDown numericUpDown_endAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_endAngle;

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                float width = (float)numericUpDown_width.Value;
                int radius = (int)numericUpDown_radius.Value;
                int imageIndex = userPanel_scaleCircle.comboBoxGetSelectedIndexImage();
                int imageBackground = userPanel_scaleCircle.comboBoxGetSelectedIndexImageBackground();
                float StartAngle = (float)numericUpDown_startAngle.Value - 90;
                float EndAngle = (float)(numericUpDown_endAngle.Value -
                    numericUpDown_startAngle.Value);
                Color color = userPanel_scaleCircle.comboBoxGetColor();
                float position = (float)Watch_Face_Preview_Set.Activity.PAI / 100f;
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleCircle.comboBoxGetFlatness();
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
            //checkBox_Use = (CheckBox)panel_scaleLinear.Controls[0];
            if (userPanel_scaleLinear.checkBox_scaleLinear_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleLinear.radioButton_scaleLinear_image;
                //RadioButton radioButton_color = (RadioButton)panel_scaleLinear.Controls[2];
                //ComboBox comboBox_image = (ComboBox)panel_scaleLinear.Controls[3];
                //ComboBox comboBox_color = (ComboBox)panel_scaleLinear.Controls[4];
                //ComboBox comboBox_pointer = (ComboBox)panel_scaleLinear.Controls[5];
                //ComboBox comboBox_background = (ComboBox)panel_scaleLinear.Controls[6];
                NumericUpDown numericUpDownX = userPanel_scaleLinear.numericUpDown_scaleLinearX;
                NumericUpDown numericUpDownY = userPanel_scaleLinear.numericUpDown_scaleLinearY;
                NumericUpDown numericUpDown_length = userPanel_scaleLinear.numericUpDown_scaleLinear_length;
                NumericUpDown numericUpDown_width = userPanel_scaleLinear.numericUpDown_scaleLinear_width;
                //ComboBox comboBox_flatness = (ComboBox)panel_scaleLinear.Controls[11];

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                int imageIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImage();
                int pointerIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImagePointer();
                int backgroundIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImageBackground();
                int length = (int)numericUpDown_length.Value;
                int width = (int)numericUpDown_width.Value;
                Color color = userPanel_scaleLinear.comboBoxGetColor();
                float position = (float)Watch_Face_Preview_Set.Activity.PAI / 100f;
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleLinear.comboBoxGetFlatness();

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

            // PAI надписью
            //checkBox_Use = (CheckBox)panel_text.Controls[0];
            if (userPanel_text.checkBox_Use.Checked)
            {
                //ComboBox comboBox_image = (ComboBox)panel_text.Controls[1];
                //ComboBox comboBox_unit = (ComboBox)panel_text.Controls[2]; icon
                //ComboBox comboBox_separator = (ComboBox)panel_text.Controls[3]; unit
                int imageIndex = userPanel_text.comboBoxGetSelectedIndexImage();
                int unit = userPanel_text.comboBoxGetSelectedIndexIcon();
                NumericUpDown numericUpDownX = userPanel_text.numericUpDown_imageX;
                NumericUpDown numericUpDownY = userPanel_text.numericUpDown_imageY;
                NumericUpDown numericUpDown_unitX = userPanel_text.numericUpDown_iconX;
                NumericUpDown numericUpDown_unitY = userPanel_text.numericUpDown_iconY;
                //ComboBox comboBox_alignment = (ComboBox)panel_text.Controls[8];
                NumericUpDown numericUpDown_spacing = userPanel_text.numericUpDown_spacing;
                CheckBox checkBox_add_zero = userPanel_text.checkBox_addZero;
                //ComboBox comboBox_imageError = (ComboBox)panel_text.Controls[11];

                if (imageIndex >= 0)
                {
                    //int imageIndex = comboBox_image.SelectedIndex;
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = userPanel_text.comboBoxGetSelectedIndexAlignment();
                    bool addZero = checkBox_add_zero.Checked;
                    int value = Watch_Face_Preview_Set.Activity.PAI;
                    int separator_index = userPanel_text.comboBoxGetSelectedIndexUnit();
                    Draw_dagital_text(gPanel, imageIndex, x, y,
                        spasing, alignment, value, addZero, 3, separator_index, BBorder);

                    if (unit >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[unit]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // PAI стрелкой
            //checkBox_Use = (CheckBox)panel_hand.Controls[0];
            if (userPanel_hand.checkBox_hand_Use.Checked)
            {
                int image_index = userPanel_hand.comboBoxGetSelectedIndexHandImage();
                if (image_index >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_hand.numericUpDown_handX;
                    NumericUpDown numericUpDownY = userPanel_hand.numericUpDown_handY;
                    NumericUpDown numericUpDown_offsetX = userPanel_hand.numericUpDown_handX_offset;
                    NumericUpDown numericUpDown_offsetY = userPanel_hand.numericUpDown_handY_offset;
                    int imageCentr = userPanel_hand.comboBoxGetSelectedIndexHandImageCentr();
                    NumericUpDown numericUpDownX_centr = userPanel_hand.numericUpDown_handX_centr;
                    NumericUpDown numericUpDownY_centr = userPanel_hand.numericUpDown_handY_centr;
                    NumericUpDown numericUpDown_startAngle = userPanel_hand.numericUpDown_hand_startAngle;
                    NumericUpDown numericUpDown_endAngle = userPanel_hand.numericUpDown_hand_endAngle;
                    int imageBackground = userPanel_hand.comboBoxGetSelectedIndexHandImageBackground();
                    NumericUpDown numericUpDownX_background = userPanel_hand.numericUpDown_handX_background;
                    NumericUpDown numericUpDownY_background = userPanel_hand.numericUpDown_handY_background;

                    if (imageBackground >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[imageBackground]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_background.Value,
                            (int)numericUpDownY_background.Value, src.Width, src.Height));
                    }

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int offsetX = (int)numericUpDown_offsetX.Value;
                    int offsetY = (int)numericUpDown_offsetY.Value;
                    //int image_index = comboBox_image.SelectedIndex;
                    float startAngle = (float)(numericUpDown_startAngle.Value);
                    float endAngle = (float)(numericUpDown_endAngle.Value);

                    float angle = startAngle + Watch_Face_Preview_Set.Activity.PAI * (endAngle - startAngle) / 100f;
                    if (Watch_Face_Preview_Set.Activity.PAI > 100) angle = endAngle;
                    DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                    if (imageCentr >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[imageCentr]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_centr.Value,
                            (int)numericUpDownY_centr.Value, src.Width, src.Height));
                    }
                }
            }
            #endregion

            #region путь
            userPanel_text = userControl_text_Distance_AOD;

            // путь надписью
            //checkBox_Use = (CheckBox)panel_text.Controls[0];
            if (userPanel_text.checkBox_Use.Checked)
            {
                //ComboBox comboBox_image = (ComboBox)panel_text.Controls[1];
                //ComboBox comboBox_unit = (ComboBox)panel_text.Controls[2]; icon
                //ComboBox comboBox_separator = (ComboBox)panel_text.Controls[3]; unit
                int imageIndex = userPanel_text.comboBoxGetSelectedIndexImage();
                int unit = userPanel_text.comboBoxGetSelectedIndexIcon();
                NumericUpDown numericUpDownX = userPanel_text.numericUpDown_imageX;
                NumericUpDown numericUpDownY = userPanel_text.numericUpDown_imageY;
                NumericUpDown numericUpDown_unitX = userPanel_text.numericUpDown_iconX;
                NumericUpDown numericUpDown_unitY = userPanel_text.numericUpDown_iconY;
                //ComboBox comboBox_alignment = (ComboBox)panel_text.Controls[8];
                NumericUpDown numericUpDown_spacing = userPanel_text.numericUpDown_spacing;
                CheckBox checkBox_add_zero = userPanel_text.checkBox_addZero;
                //ComboBox comboBox_imageError = (ComboBox)panel_text.Controls[11];
                //ComboBox comboBox_DecimalPoint = (ComboBox)panel_text.Controls[12];

                if (imageIndex >= 0)
                {
                    //int imageIndex = comboBox_image.SelectedIndex;
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = userPanel_text.comboBoxGetSelectedIndexAlignment();
                    bool addZero = checkBox_add_zero.Checked;
                    double value = Watch_Face_Preview_Set.Activity.Distance / 1000d;
                    int separator_index = userPanel_text.comboBoxGetSelectedIndexUnit();
                    int decimalPoint_index = userPanel_text.comboBoxGetSelectedIndexImageDecimalPointOrMinus();

                    Draw_dagital_text(gPanel, imageIndex, x, y,
                        spasing, alignment, value, addZero, 4, separator_index,
                        decimalPoint_index, 2, BBorder);

                    if (unit >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[unit]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }
            #endregion

            #region StandUp
            userPanel_pictures = userControl_pictures_StandUp_AOD;
            userPanel_text = userControl_text_StandUp_AOD;
            userPanel_hand = userControl_hand_StandUp_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_StandUp_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_StandUp_AOD;
            // StandUp картинками
            if (userPanel_pictures.checkBox_pictures_Use.Checked)
            {
                //ComboBox comboBox_image = (ComboBox)panel_pictures.Controls[1];
                if (userPanel_pictures.comboBoxGetImage() >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_pictures.numericUpDown_picturesX;
                    NumericUpDown numericUpDownY = userPanel_pictures.numericUpDown_picturesY;
                    NumericUpDown numericUpDown_count = userPanel_pictures.numericUpDown_pictures_count;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int count = (int)numericUpDown_count.Value;
                    //int offSet = (int)Math.Ceiling((float)count * Watch_Face_Preview_Set.Activity.PAI / 100f);
                    int offSet = (int)((count + 1f) * Watch_Face_Preview_Set.Activity.StandUp / 12f);
                    //offSet--;
                    if (offSet < 0) offSet = 0;
                    if (offSet >= count) offSet = (int)(count - 1);
                    int imageIndex = userPanel_pictures.comboBoxGetSelectedIndexImage() + offSet;

                    if (imageIndex < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[imageIndex]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }
                }
            }

            // StandUp круговой шкалой
            if (userPanel_scaleCircle.checkBox_scaleCircle_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleCircle.radioButton_scaleCircle_image;
                NumericUpDown numericUpDownX = userPanel_scaleCircle.numericUpDown_scaleCircleX;
                NumericUpDown numericUpDownY = userPanel_scaleCircle.numericUpDown_scaleCircleY;
                NumericUpDown numericUpDown_radius = userPanel_scaleCircle.numericUpDown_scaleCircle_radius;
                NumericUpDown numericUpDown_width = userPanel_scaleCircle.numericUpDown_scaleCircle_width;
                NumericUpDown numericUpDown_startAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_startAngle;
                NumericUpDown numericUpDown_endAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_endAngle;

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                float width = (float)numericUpDown_width.Value;
                int radius = (int)numericUpDown_radius.Value;
                int imageIndex = userPanel_scaleCircle.comboBoxGetSelectedIndexImage();
                int imageBackground = userPanel_scaleCircle.comboBoxGetSelectedIndexImageBackground();
                float StartAngle = (float)numericUpDown_startAngle.Value - 90;
                float EndAngle = (float)(numericUpDown_endAngle.Value -
                    numericUpDown_startAngle.Value);
                Color color = userPanel_scaleCircle.comboBoxGetColor();
                float position = (float)Watch_Face_Preview_Set.Activity.StandUp / 12f;
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleCircle.comboBoxGetFlatness();
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

            // StandUp линейной шкалой
            if (userPanel_scaleLinear.checkBox_scaleLinear_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleLinear.radioButton_scaleLinear_image;
                NumericUpDown numericUpDownX = userPanel_scaleLinear.numericUpDown_scaleLinearX;
                NumericUpDown numericUpDownY = userPanel_scaleLinear.numericUpDown_scaleLinearY;
                NumericUpDown numericUpDown_length = userPanel_scaleLinear.numericUpDown_scaleLinear_length;
                NumericUpDown numericUpDown_width = userPanel_scaleLinear.numericUpDown_scaleLinear_width;
                //ComboBox comboBox_flatness = (ComboBox)panel_scaleLinear.Controls[11];

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                int imageIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImage();
                int pointerIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImagePointer();
                int backgroundIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImageBackground();
                int length = (int)numericUpDown_length.Value;
                int width = (int)numericUpDown_width.Value;
                Color color = userPanel_scaleLinear.comboBoxGetColor();
                float position = (float)Watch_Face_Preview_Set.Activity.StandUp / 12f;
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleLinear.comboBoxGetFlatness();

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

            // StandUp надписью
            if (userPanel_text.checkBox_Use.Checked)
            {
                int imageIndex = userPanel_text.comboBoxGetSelectedIndexImage();
                int unit = userPanel_text.comboBoxGetSelectedIndexIcon();
                NumericUpDown numericUpDownX = userPanel_text.numericUpDown_imageX;
                NumericUpDown numericUpDownY = userPanel_text.numericUpDown_imageY;
                NumericUpDown numericUpDown_unitX = userPanel_text.numericUpDown_iconX;
                NumericUpDown numericUpDown_unitY = userPanel_text.numericUpDown_iconY;
                NumericUpDown numericUpDown_spacing = userPanel_text.numericUpDown_spacing;
                CheckBox checkBox_add_zero = userPanel_text.checkBox_addZero;

                if (imageIndex >= 0)
                {
                    //int imageIndex = comboBox_image.SelectedIndex;
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = userPanel_text.comboBoxGetSelectedIndexAlignment();
                    bool addZero = checkBox_add_zero.Checked;
                    int value = Watch_Face_Preview_Set.Activity.StandUp;
                    int separator_index = userPanel_text.comboBoxGetSelectedIndexUnit();
                    Draw_dagital_text(gPanel, imageIndex, x, y,
                        spasing, alignment, value, addZero, 2, separator_index, BBorder);

                    if (unit >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[unit]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // StandUp стрелкой
            if (userPanel_hand.checkBox_hand_Use.Checked)
            {
                int image_index = userPanel_hand.comboBoxGetSelectedIndexHandImage();
                if (image_index >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_hand.numericUpDown_handX;
                    NumericUpDown numericUpDownY = userPanel_hand.numericUpDown_handY;
                    NumericUpDown numericUpDown_offsetX = userPanel_hand.numericUpDown_handX_offset;
                    NumericUpDown numericUpDown_offsetY = userPanel_hand.numericUpDown_handY_offset;
                    int imageCentr = userPanel_hand.comboBoxGetSelectedIndexHandImageCentr();
                    NumericUpDown numericUpDownX_centr = userPanel_hand.numericUpDown_handX_centr;
                    NumericUpDown numericUpDownY_centr = userPanel_hand.numericUpDown_handY_centr;
                    NumericUpDown numericUpDown_startAngle = userPanel_hand.numericUpDown_hand_startAngle;
                    NumericUpDown numericUpDown_endAngle = userPanel_hand.numericUpDown_hand_endAngle;
                    int imageBackground = userPanel_hand.comboBoxGetSelectedIndexHandImageBackground();
                    NumericUpDown numericUpDownX_background = userPanel_hand.numericUpDown_handX_background;
                    NumericUpDown numericUpDownY_background = userPanel_hand.numericUpDown_handY_background;

                    if (imageBackground >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[imageBackground]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_background.Value,
                            (int)numericUpDownY_background.Value, src.Width, src.Height));
                    }

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int offsetX = (int)numericUpDown_offsetX.Value;
                    int offsetY = (int)numericUpDown_offsetY.Value;
                    //int image_index = comboBox_image.SelectedIndex;
                    float startAngle = (float)(numericUpDown_startAngle.Value);
                    float endAngle = (float)(numericUpDown_endAngle.Value);

                    float angle = startAngle + Watch_Face_Preview_Set.Activity.StandUp * (endAngle - startAngle) / 12f;
                    if (Watch_Face_Preview_Set.Activity.StandUp > 12) angle = endAngle;
                    DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                    if (imageCentr >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[imageCentr]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_centr.Value,
                            (int)numericUpDownY_centr.Value, src.Width, src.Height));
                    }
                }
            }
            #endregion

            #region погода

            UserControl_pictures_weather userPanel_pictures_weather = userControl_pictures_weather_AOD;
            UserControl_text_weather userPanel_text_weather_Current = userControl_text_weather_Current_AOD;
            UserControl_text_weather userPanel_text_weather_Min = userControl_text_weather_Min_AOD;
            UserControl_text_weather userPanel_text_weather_Max = userControl_text_weather_Max_AOD;
            userPanel_hand = userControl_hand_Weather_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_Weather_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_Weather_AOD;

            // погода картинками
            if (userPanel_pictures_weather.checkBox_pictures_Use.Checked)
            {
                if (userPanel_pictures_weather.comboBoxGetSelectedIndexImage() >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_pictures_weather.numericUpDown_picturesX;
                    NumericUpDown numericUpDownY = userPanel_pictures_weather.numericUpDown_picturesY;
                    //NumericUpDown numericUpDown_count = (NumericUpDown)panel_pictures.Controls[4];

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    //int count = (int)numericUpDown_count.Value;
                    int offSet = Watch_Face_Preview_Set.Weather.Icon;
                    if (offSet < 0) offSet = 25;
                    //if (offSet >= count) offSet = (int)(count - 1);
                    //int offSet = (int)Math.Round(count * Watch_Face_Preview_Set.Battery / 100f, 0);
                    int imageIndex = userPanel_pictures_weather.comboBoxGetSelectedIndexImage() + offSet;

                    if (imageIndex < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[imageIndex]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }
                }
            }

            // погода круговой шкалой
            if (userPanel_scaleCircle.checkBox_scaleCircle_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleCircle.radioButton_scaleCircle_image;
                //RadioButton radioButton_color = (RadioButton)panel_scaleCircle.Controls[2];
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

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                float width = (float)numericUpDown_width.Value;
                int radius = (int)numericUpDown_radius.Value;
                int imageIndex = userPanel_scaleCircle.comboBoxGetSelectedIndexImage();
                int imageBackground = userPanel_scaleCircle.comboBoxGetSelectedIndexImageBackground();
                float StartAngle = (float)numericUpDown_startAngle.Value - 90;
                float EndAngle = (float)(numericUpDown_endAngle.Value -
                    numericUpDown_startAngle.Value);
                Color color = userPanel_scaleCircle.comboBoxGetColor();
                float position = (float)((Watch_Face_Preview_Set.Weather.Temperature + 25) / 60f);
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleCircle.comboBoxGetFlatness();
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
            if (userPanel_scaleLinear.checkBox_scaleLinear_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleLinear.radioButton_scaleLinear_image;
                //RadioButton radioButton_color = (RadioButton)panel_scaleLinear.Controls[2];
                //ComboBox comboBox_image = (ComboBox)panel_scaleLinear.Controls[3];
                //ComboBox comboBox_color = (ComboBox)panel_scaleLinear.Controls[4];
                //ComboBox comboBox_pointer = (ComboBox)panel_scaleLinear.Controls[5];
                //ComboBox comboBox_background = (ComboBox)panel_scaleLinear.Controls[6];
                NumericUpDown numericUpDownX = userPanel_scaleLinear.numericUpDown_scaleLinearX;
                NumericUpDown numericUpDownY = userPanel_scaleLinear.numericUpDown_scaleLinearY;
                NumericUpDown numericUpDown_length = userPanel_scaleLinear.numericUpDown_scaleLinear_length;
                NumericUpDown numericUpDown_width = userPanel_scaleLinear.numericUpDown_scaleLinear_width;
                //ComboBox comboBox_flatness = (ComboBox)panel_scaleLinear.Controls[11];

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                int imageIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImage();
                int pointerIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImagePointer();
                int backgroundIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImageBackground();
                int length = (int)numericUpDown_length.Value;
                int width = (int)numericUpDown_width.Value;
                Color color = userPanel_scaleLinear.comboBoxGetColor();
                float position = (float)((Watch_Face_Preview_Set.Weather.Temperature + 25) / 60f);
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleLinear.comboBoxGetFlatness();

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
            if (userPanel_text_weather_Current.checkBox_Use.Checked)
            {
                if (userPanel_text_weather_Current.comboBoxGetSelectedIndexImage() >= 0)
                {
                    //ComboBox comboBox_image = (ComboBox)panel_text.Controls[1];
                    //ComboBox comboBox_unit = (ComboBox)panel_text.Controls[2];
                    //ComboBox comboBox_separator = (ComboBox)panel_text.Controls[3];
                    NumericUpDown numericUpDownX = userPanel_text_weather_Current.numericUpDown_imageX;
                    NumericUpDown numericUpDownY = userPanel_text_weather_Current.numericUpDown_imageY;
                    NumericUpDown numericUpDown_unitX = userPanel_text_weather_Current.numericUpDown_iconX;
                    NumericUpDown numericUpDown_unitY = userPanel_text_weather_Current.numericUpDown_iconY;
                    //ComboBox comboBox_alignment = (ComboBox)panel_text.Controls[8];
                    NumericUpDown numericUpDown_spacing = userPanel_text_weather_Current.numericUpDown_spacing;
                    //ComboBox comboBox_imageError = (ComboBox)panel_text.Controls[10];
                    //ComboBox comboBox_imageMinus = (ComboBox)panel_text.Controls[11];
                    CheckBox checkBox_addZero = userPanel_text_weather_Max.checkBox_addZero;

                    int imageIndex = userPanel_text_weather_Current.comboBoxGetSelectedIndexImage();
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = userPanel_text_weather_Current.comboBoxGetSelectedIndexAlignment();
                    bool addZero = checkBox_addZero.Checked;
                    int value = Watch_Face_Preview_Set.Weather.Temperature;
                    int separator_index = userPanel_text_weather_Current.comboBoxGetSelectedIndexUnit();
                    int imageError_index = userPanel_text_weather_Current.comboBoxGetSelectedIndexImageError();
                    int imageMinus_index = userPanel_text_weather_Current.comboBoxGetSelectedIndexImageDecimalPointOrMinus();
                    if (!Watch_Face_Preview_Set.Weather.TemperatureNoData)
                    {
                        Draw_weather_text(gPanel, imageIndex, x, y,
                                        spasing, alignment, value, addZero, imageMinus_index, separator_index, BBorder);
                    }
                    else
                    {
                        src = OpenFileStream(ListImagesFullName[imageError_index]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }

                    if (userPanel_text_weather_Current.comboBoxGetSelectedIndexIcon() >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[userPanel_text_weather_Current.comboBoxGetSelectedIndexIcon()]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // минимальная температура надписью
            int Temperature_offsetX = -1;
            int Temperature_offsetY = -1;
            if (userPanel_text_weather_Min.checkBox_Use.Checked)
            {
                if (userPanel_text_weather_Min.comboBoxGetSelectedIndexImage() >= 0)
                {
                    //ComboBox comboBox_image = (ComboBox)panel_text.Controls[1];
                    //ComboBox comboBox_unit = (ComboBox)panel_text.Controls[2];
                    //ComboBox comboBox_separator = (ComboBox)panel_text.Controls[3];
                    NumericUpDown numericUpDownX = userPanel_text_weather_Min.numericUpDown_imageX;
                    NumericUpDown numericUpDownY = userPanel_text_weather_Min.numericUpDown_imageY;
                    NumericUpDown numericUpDown_unitX = userPanel_text_weather_Min.numericUpDown_iconX;
                    NumericUpDown numericUpDown_unitY = userPanel_text_weather_Min.numericUpDown_iconY;
                    //ComboBox comboBox_alignment = (ComboBox)panel_text.Controls[8];
                    NumericUpDown numericUpDown_spacing = userPanel_text_weather_Min.numericUpDown_spacing;
                    //ComboBox comboBox_imageError = (ComboBox)panel_text.Controls[10];
                    //ComboBox comboBox_imageMinus = (ComboBox)panel_text.Controls[11];
                    CheckBox checkBox_addZero = userPanel_text_weather_Max.checkBox_addZero;

                    int imageIndex = userPanel_text_weather_Min.comboBoxGetSelectedIndexImage();
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    Temperature_offsetY = y;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = userPanel_text_weather_Min.comboBoxGetSelectedIndexAlignment();
                    bool addZero = checkBox_addZero.Checked;
                    int value = Watch_Face_Preview_Set.Weather.TemperatureMin;
                    int separator_index = userPanel_text_weather_Min.comboBoxGetSelectedIndexUnit();
                    int imageError_index = userPanel_text_weather_Min.comboBoxGetSelectedIndexImageError();
                    int imageMinus_index = userPanel_text_weather_Min.comboBoxGetSelectedIndexImageDecimalPointOrMinus();
                    if (!Watch_Face_Preview_Set.Weather.TemperatureMinMaxNoData)
                    {
                        Temperature_offsetX = Draw_weather_text(gPanel, imageIndex, x, y,
                                        spasing, alignment, value, addZero, imageMinus_index, separator_index, BBorder);
                    }
                    else
                    {
                        src = OpenFileStream(ListImagesFullName[imageError_index]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }

                    if (userPanel_text_weather_Min.comboBoxGetSelectedIndexIcon() >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[userPanel_text_weather_Min.comboBoxGetSelectedIndexIcon()]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // максимальная температура надписью
            if (userPanel_text_weather_Max.checkBox_Use.Checked)
            {
                if (userPanel_text_weather_Max.comboBoxGetSelectedIndexImage() >= 0)
                {
                    //ComboBox comboBox_image = (ComboBox)panel_text.Controls[1];
                    //ComboBox comboBox_unit = (ComboBox)panel_text.Controls[2];
                    //ComboBox comboBox_separator = (ComboBox)panel_text.Controls[3];
                    NumericUpDown numericUpDownX = userPanel_text_weather_Max.numericUpDown_imageX;
                    NumericUpDown numericUpDownY = userPanel_text_weather_Max.numericUpDown_imageY;
                    NumericUpDown numericUpDown_unitX = userPanel_text_weather_Max.numericUpDown_iconX;
                    NumericUpDown numericUpDown_unitY = userPanel_text_weather_Max.numericUpDown_iconY;
                    //ComboBox comboBox_alignment = (ComboBox)panel_text.Controls[8];
                    NumericUpDown numericUpDown_spacing = userPanel_text_weather_Max.numericUpDown_spacing;
                    //ComboBox comboBox_imageError = (ComboBox)panel_text.Controls[10];
                    //ComboBox comboBox_imageMinus = (ComboBox)panel_text.Controls[11];
                    CheckBox checkBox_addZero = userPanel_text_weather_Max.checkBox_addZero;
                    CheckBox checkBox_follow = userPanel_text_weather_Max.checkBox_follow;

                    int imageIndex = userPanel_text_weather_Max.comboBoxGetSelectedIndexImage();
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = userPanel_text_weather_Max.comboBoxGetSelectedIndexAlignment();
                    bool addZero = checkBox_addZero.Checked;
                    int value = Watch_Face_Preview_Set.Weather.TemperatureMax;
                    int separator_index = userPanel_text_weather_Max.comboBoxGetSelectedIndexUnit();
                    int imageError_index = userPanel_text_weather_Max.comboBoxGetSelectedIndexImageError();
                    int imageMinus_index = userPanel_text_weather_Max.comboBoxGetSelectedIndexImageDecimalPointOrMinus();

                    if (checkBox_follow.Checked && Temperature_offsetX > -1)
                    {
                        x = Temperature_offsetX;
                        alignment = 0;
                        y = Temperature_offsetY;
                    }

                    if (!Watch_Face_Preview_Set.Weather.TemperatureMinMaxNoData)
                    {
                        Draw_weather_text(gPanel, imageIndex, x, y,
                                        spasing, alignment, value, addZero, imageMinus_index, separator_index, BBorder);
                    }
                    else
                    {
                        src = OpenFileStream(ListImagesFullName[imageError_index]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }

                    if (userPanel_text_weather_Max.comboBoxGetSelectedIndexIcon() >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[userPanel_text_weather_Max.comboBoxGetSelectedIndexIcon()]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // погода стрелкой
            if (userPanel_hand.checkBox_hand_Use.Checked)
            {
                if (userPanel_hand.comboBoxGetSelectedIndexHandImage() >= 0)
                {
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

                    if (userPanel_hand.comboBoxGetSelectedIndexHandImageBackground() >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[userPanel_hand.comboBoxGetSelectedIndexHandImageBackground()]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_background.Value,
                            (int)numericUpDownY_background.Value, src.Width, src.Height));
                    }

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int offsetX = (int)numericUpDown_offsetX.Value;
                    int offsetY = (int)numericUpDown_offsetY.Value;
                    int image_index = userPanel_hand.comboBoxGetSelectedIndexHandImage();
                    float startAngle = (float)(numericUpDown_startAngle.Value);
                    float endAngle = (float)(numericUpDown_endAngle.Value);

                    float position = (float)((Watch_Face_Preview_Set.Weather.Temperature + 25) / 60f);
                    float angle = startAngle + position * (endAngle - startAngle);
                    if (Watch_Face_Preview_Set.Weather.Temperature > 35) angle = endAngle;
                    DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                    if (userPanel_hand.comboBoxGetSelectedIndexHandImageCentr() >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[userPanel_hand.comboBoxGetSelectedIndexHandImageCentr()]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_centr.Value,
                            (int)numericUpDownY_centr.Value, src.Width, src.Height));
                    }
                }
            }
            #endregion

            #region UVindex
            userPanel_pictures = userControl_pictures_UVindex_AOD;
            userPanel_text = userControl_text_UVindex_AOD;
            userPanel_hand = userControl_hand_UVindex_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_UVindex_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_UVindex_AOD;
            // UVindex картинками
            if (userPanel_pictures.checkBox_pictures_Use.Checked)
            {
                //ComboBox comboBox_image = (ComboBox)panel_pictures.Controls[1];
                if (userPanel_pictures.comboBoxGetImage() >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_pictures.numericUpDown_picturesX;
                    NumericUpDown numericUpDownY = userPanel_pictures.numericUpDown_picturesY;
                    NumericUpDown numericUpDown_count = userPanel_pictures.numericUpDown_pictures_count;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int count = (int)numericUpDown_count.Value;
                    //int offSet = (int)Math.Ceiling((float)count * Watch_Face_Preview_Set.Activity.PAI / 100f);
                    int offSet = (int)((count + 1f) * Watch_Face_Preview_Set.Weather.UVindex / 10f);
                    //offSet--;
                    if (offSet < 0) offSet = 0;
                    if (offSet >= count) offSet = (int)(count - 1);
                    int imageIndex = userPanel_pictures.comboBoxGetSelectedIndexImage() + offSet;

                    if (imageIndex < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[imageIndex]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }
                }
            }

            // UVindex круговой шкалой
            if (userPanel_scaleCircle.checkBox_scaleCircle_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleCircle.radioButton_scaleCircle_image;
                NumericUpDown numericUpDownX = userPanel_scaleCircle.numericUpDown_scaleCircleX;
                NumericUpDown numericUpDownY = userPanel_scaleCircle.numericUpDown_scaleCircleY;
                NumericUpDown numericUpDown_radius = userPanel_scaleCircle.numericUpDown_scaleCircle_radius;
                NumericUpDown numericUpDown_width = userPanel_scaleCircle.numericUpDown_scaleCircle_width;
                NumericUpDown numericUpDown_startAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_startAngle;
                NumericUpDown numericUpDown_endAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_endAngle;

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                float width = (float)numericUpDown_width.Value;
                int radius = (int)numericUpDown_radius.Value;
                int imageIndex = userPanel_scaleCircle.comboBoxGetSelectedIndexImage();
                int imageBackground = userPanel_scaleCircle.comboBoxGetSelectedIndexImageBackground();
                float StartAngle = (float)numericUpDown_startAngle.Value - 90;
                float EndAngle = (float)(numericUpDown_endAngle.Value -
                    numericUpDown_startAngle.Value);
                Color color = userPanel_scaleCircle.comboBoxGetColor();
                float position = (float)Watch_Face_Preview_Set.Weather.UVindex / 10f;
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleCircle.comboBoxGetFlatness();
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

            // UVindex линейной шкалой
            if (userPanel_scaleLinear.checkBox_scaleLinear_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleLinear.radioButton_scaleLinear_image;
                NumericUpDown numericUpDownX = userPanel_scaleLinear.numericUpDown_scaleLinearX;
                NumericUpDown numericUpDownY = userPanel_scaleLinear.numericUpDown_scaleLinearY;
                NumericUpDown numericUpDown_length = userPanel_scaleLinear.numericUpDown_scaleLinear_length;
                NumericUpDown numericUpDown_width = userPanel_scaleLinear.numericUpDown_scaleLinear_width;
                //ComboBox comboBox_flatness = (ComboBox)panel_scaleLinear.Controls[11];

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                int imageIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImage();
                int pointerIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImagePointer();
                int backgroundIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImageBackground();
                int length = (int)numericUpDown_length.Value;
                int width = (int)numericUpDown_width.Value;
                Color color = userPanel_scaleLinear.comboBoxGetColor();
                float position = (float)Watch_Face_Preview_Set.Weather.UVindex / 10f;
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleLinear.comboBoxGetFlatness();

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

            // UVindex надписью
            if (userPanel_text.checkBox_Use.Checked)
            {
                int imageIndex = userPanel_text.comboBoxGetSelectedIndexImage();
                int unit = userPanel_text.comboBoxGetSelectedIndexIcon();
                NumericUpDown numericUpDownX = userPanel_text.numericUpDown_imageX;
                NumericUpDown numericUpDownY = userPanel_text.numericUpDown_imageY;
                NumericUpDown numericUpDown_unitX = userPanel_text.numericUpDown_iconX;
                NumericUpDown numericUpDown_unitY = userPanel_text.numericUpDown_iconY;
                NumericUpDown numericUpDown_spacing = userPanel_text.numericUpDown_spacing;
                CheckBox checkBox_add_zero = userPanel_text.checkBox_addZero;

                if (imageIndex >= 0)
                {
                    //int imageIndex = comboBox_image.SelectedIndex;
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = userPanel_text.comboBoxGetSelectedIndexAlignment();
                    bool addZero = checkBox_add_zero.Checked;
                    int value = Watch_Face_Preview_Set.Weather.UVindex;
                    int separator_index = userPanel_text.comboBoxGetSelectedIndexUnit();
                    Draw_dagital_text(gPanel, imageIndex, x, y,
                        spasing, alignment, value, addZero, 2, separator_index, BBorder);

                    if (unit >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[unit]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // UVindex стрелкой
            if (userPanel_hand.checkBox_hand_Use.Checked)
            {
                int image_index = userPanel_hand.comboBoxGetSelectedIndexHandImage();
                if (image_index >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_hand.numericUpDown_handX;
                    NumericUpDown numericUpDownY = userPanel_hand.numericUpDown_handY;
                    NumericUpDown numericUpDown_offsetX = userPanel_hand.numericUpDown_handX_offset;
                    NumericUpDown numericUpDown_offsetY = userPanel_hand.numericUpDown_handY_offset;
                    int imageCentr = userPanel_hand.comboBoxGetSelectedIndexHandImageCentr();
                    NumericUpDown numericUpDownX_centr = userPanel_hand.numericUpDown_handX_centr;
                    NumericUpDown numericUpDownY_centr = userPanel_hand.numericUpDown_handY_centr;
                    NumericUpDown numericUpDown_startAngle = userPanel_hand.numericUpDown_hand_startAngle;
                    NumericUpDown numericUpDown_endAngle = userPanel_hand.numericUpDown_hand_endAngle;
                    int imageBackground = userPanel_hand.comboBoxGetSelectedIndexHandImageBackground();
                    NumericUpDown numericUpDownX_background = userPanel_hand.numericUpDown_handX_background;
                    NumericUpDown numericUpDownY_background = userPanel_hand.numericUpDown_handY_background;

                    if (imageBackground >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[imageBackground]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_background.Value,
                            (int)numericUpDownY_background.Value, src.Width, src.Height));
                    }

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int offsetX = (int)numericUpDown_offsetX.Value;
                    int offsetY = (int)numericUpDown_offsetY.Value;
                    //int image_index = comboBox_image.SelectedIndex;
                    float startAngle = (float)(numericUpDown_startAngle.Value);
                    float endAngle = (float)(numericUpDown_endAngle.Value);

                    float angle = startAngle + Watch_Face_Preview_Set.Weather.UVindex * (endAngle - startAngle) / 10f;
                    if (Watch_Face_Preview_Set.Weather.UVindex > 10) angle = endAngle;
                    DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                    if (imageCentr >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[imageCentr]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_centr.Value,
                            (int)numericUpDownY_centr.Value, src.Width, src.Height));
                    }
                }
            }
            #endregion

            #region AirQuality
            userPanel_pictures = userControl_pictures_AirQuality_AOD;
            userPanel_text = userControl_text_AirQuality_AOD;
            userPanel_hand = userControl_hand_AirQuality_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_AirQuality_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_AirQuality_AOD;
            // AirQuality картинками
            if (userPanel_pictures.checkBox_pictures_Use.Checked)
            {
                //ComboBox comboBox_image = (ComboBox)panel_pictures.Controls[1];
                if (userPanel_pictures.comboBoxGetImage() >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_pictures.numericUpDown_picturesX;
                    NumericUpDown numericUpDownY = userPanel_pictures.numericUpDown_picturesY;
                    NumericUpDown numericUpDown_count = userPanel_pictures.numericUpDown_pictures_count;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int count = (int)numericUpDown_count.Value;
                    //int offSet = (int)Math.Ceiling((float)count * Watch_Face_Preview_Set.Activity.PAI / 100f);
                    int offSet = (int)((count + 1f) * Watch_Face_Preview_Set.Weather.AirQuality / 502f);
                    //offSet--;
                    if (offSet < 0) offSet = 0;
                    if (offSet >= count) offSet = (int)(count - 1);
                    int imageIndex = userPanel_pictures.comboBoxGetSelectedIndexImage() + offSet;

                    if (imageIndex < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[imageIndex]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }
                }
            }

            // AirQuality круговой шкалой
            if (userPanel_scaleCircle.checkBox_scaleCircle_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleCircle.radioButton_scaleCircle_image;
                NumericUpDown numericUpDownX = userPanel_scaleCircle.numericUpDown_scaleCircleX;
                NumericUpDown numericUpDownY = userPanel_scaleCircle.numericUpDown_scaleCircleY;
                NumericUpDown numericUpDown_radius = userPanel_scaleCircle.numericUpDown_scaleCircle_radius;
                NumericUpDown numericUpDown_width = userPanel_scaleCircle.numericUpDown_scaleCircle_width;
                NumericUpDown numericUpDown_startAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_startAngle;
                NumericUpDown numericUpDown_endAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_endAngle;

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                float width = (float)numericUpDown_width.Value;
                int radius = (int)numericUpDown_radius.Value;
                int imageIndex = userPanel_scaleCircle.comboBoxGetSelectedIndexImage();
                int imageBackground = userPanel_scaleCircle.comboBoxGetSelectedIndexImageBackground();
                float StartAngle = (float)numericUpDown_startAngle.Value - 90;
                float EndAngle = (float)(numericUpDown_endAngle.Value -
                    numericUpDown_startAngle.Value);
                Color color = userPanel_scaleCircle.comboBoxGetColor();
                float position = (float)Watch_Face_Preview_Set.Weather.AirQuality / 503f;
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleCircle.comboBoxGetFlatness();
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

            // AirQuality линейной шкалой
            if (userPanel_scaleLinear.checkBox_scaleLinear_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleLinear.radioButton_scaleLinear_image;
                NumericUpDown numericUpDownX = userPanel_scaleLinear.numericUpDown_scaleLinearX;
                NumericUpDown numericUpDownY = userPanel_scaleLinear.numericUpDown_scaleLinearY;
                NumericUpDown numericUpDown_length = userPanel_scaleLinear.numericUpDown_scaleLinear_length;
                NumericUpDown numericUpDown_width = userPanel_scaleLinear.numericUpDown_scaleLinear_width;
                //ComboBox comboBox_flatness = (ComboBox)panel_scaleLinear.Controls[11];

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                int imageIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImage();
                int pointerIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImagePointer();
                int backgroundIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImageBackground();
                int length = (int)numericUpDown_length.Value;
                int width = (int)numericUpDown_width.Value;
                Color color = userPanel_scaleLinear.comboBoxGetColor();
                float position = (float)Watch_Face_Preview_Set.Weather.AirQuality / 502f;
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleLinear.comboBoxGetFlatness();

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

            // AirQuality надписью
            if (userPanel_text.checkBox_Use.Checked)
            {
                int imageIndex = userPanel_text.comboBoxGetSelectedIndexImage();
                int unit = userPanel_text.comboBoxGetSelectedIndexIcon();
                NumericUpDown numericUpDownX = userPanel_text.numericUpDown_imageX;
                NumericUpDown numericUpDownY = userPanel_text.numericUpDown_imageY;
                NumericUpDown numericUpDown_unitX = userPanel_text.numericUpDown_iconX;
                NumericUpDown numericUpDown_unitY = userPanel_text.numericUpDown_iconY;
                NumericUpDown numericUpDown_spacing = userPanel_text.numericUpDown_spacing;
                CheckBox checkBox_add_zero = userPanel_text.checkBox_addZero;

                if (imageIndex >= 0)
                {
                    //int imageIndex = comboBox_image.SelectedIndex;
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = userPanel_text.comboBoxGetSelectedIndexAlignment();
                    bool addZero = checkBox_add_zero.Checked;
                    int value = Watch_Face_Preview_Set.Weather.AirQuality;
                    int separator_index = userPanel_text.comboBoxGetSelectedIndexUnit();
                    Draw_dagital_text(gPanel, imageIndex, x, y,
                        spasing, alignment, value, addZero, 3, separator_index, BBorder);

                    if (unit >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[unit]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // AirQuality стрелкой
            if (userPanel_hand.checkBox_hand_Use.Checked)
            {
                int image_index = userPanel_hand.comboBoxGetSelectedIndexHandImage();
                if (image_index >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_hand.numericUpDown_handX;
                    NumericUpDown numericUpDownY = userPanel_hand.numericUpDown_handY;
                    NumericUpDown numericUpDown_offsetX = userPanel_hand.numericUpDown_handX_offset;
                    NumericUpDown numericUpDown_offsetY = userPanel_hand.numericUpDown_handY_offset;
                    int imageCentr = userPanel_hand.comboBoxGetSelectedIndexHandImageCentr();
                    NumericUpDown numericUpDownX_centr = userPanel_hand.numericUpDown_handX_centr;
                    NumericUpDown numericUpDownY_centr = userPanel_hand.numericUpDown_handY_centr;
                    NumericUpDown numericUpDown_startAngle = userPanel_hand.numericUpDown_hand_startAngle;
                    NumericUpDown numericUpDown_endAngle = userPanel_hand.numericUpDown_hand_endAngle;
                    int imageBackground = userPanel_hand.comboBoxGetSelectedIndexHandImageBackground();
                    NumericUpDown numericUpDownX_background = userPanel_hand.numericUpDown_handX_background;
                    NumericUpDown numericUpDownY_background = userPanel_hand.numericUpDown_handY_background;

                    if (imageBackground >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[imageBackground]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_background.Value,
                            (int)numericUpDownY_background.Value, src.Width, src.Height));
                    }

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int offsetX = (int)numericUpDown_offsetX.Value;
                    int offsetY = (int)numericUpDown_offsetY.Value;
                    //int image_index = comboBox_image.SelectedIndex;
                    float startAngle = (float)(numericUpDown_startAngle.Value);
                    float endAngle = (float)(numericUpDown_endAngle.Value);

                    float angle = startAngle + Watch_Face_Preview_Set.Weather.AirQuality * (endAngle - startAngle) / 502f;
                    if (Watch_Face_Preview_Set.Weather.AirQuality > 502) angle = endAngle;
                    DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                    if (imageCentr >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[imageCentr]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_centr.Value,
                            (int)numericUpDownY_centr.Value, src.Width, src.Height));
                    }
                }
            }
            #endregion

            #region Humidity
            userPanel_pictures = userControl_pictures_Humidity_AOD;
            userPanel_text = userControl_text_Humidity_AOD;
            userPanel_hand = userControl_hand_Humidity_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_Humidity_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_Humidity_AOD;
            // Humidity картинками
            if (userPanel_pictures.checkBox_pictures_Use.Checked)
            {
                //ComboBox comboBox_image = (ComboBox)panel_pictures.Controls[1];
                if (userPanel_pictures.comboBoxGetImage() >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_pictures.numericUpDown_picturesX;
                    NumericUpDown numericUpDownY = userPanel_pictures.numericUpDown_picturesY;
                    NumericUpDown numericUpDown_count = userPanel_pictures.numericUpDown_pictures_count;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int count = (int)numericUpDown_count.Value;
                    //int offSet = (int)Math.Ceiling((float)count * Watch_Face_Preview_Set.Activity.PAI / 100f);
                    int offSet = (int)((count + 1f) * Watch_Face_Preview_Set.Weather.Humidity / 100f);
                    //offSet--;
                    if (offSet < 0) offSet = 0;
                    if (offSet >= count) offSet = (int)(count - 1);
                    int imageIndex = userPanel_pictures.comboBoxGetSelectedIndexImage() + offSet;

                    if (imageIndex < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[imageIndex]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }
                }
            }

            // Humidity круговой шкалой
            if (userPanel_scaleCircle.checkBox_scaleCircle_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleCircle.radioButton_scaleCircle_image;
                NumericUpDown numericUpDownX = userPanel_scaleCircle.numericUpDown_scaleCircleX;
                NumericUpDown numericUpDownY = userPanel_scaleCircle.numericUpDown_scaleCircleY;
                NumericUpDown numericUpDown_radius = userPanel_scaleCircle.numericUpDown_scaleCircle_radius;
                NumericUpDown numericUpDown_width = userPanel_scaleCircle.numericUpDown_scaleCircle_width;
                NumericUpDown numericUpDown_startAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_startAngle;
                NumericUpDown numericUpDown_endAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_endAngle;

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                float width = (float)numericUpDown_width.Value;
                int radius = (int)numericUpDown_radius.Value;
                int imageIndex = userPanel_scaleCircle.comboBoxGetSelectedIndexImage();
                int imageBackground = userPanel_scaleCircle.comboBoxGetSelectedIndexImageBackground();
                float StartAngle = (float)numericUpDown_startAngle.Value - 90;
                float EndAngle = (float)(numericUpDown_endAngle.Value -
                    numericUpDown_startAngle.Value);
                Color color = userPanel_scaleCircle.comboBoxGetColor();
                float position = (float)Watch_Face_Preview_Set.Weather.Humidity / 100f;
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleCircle.comboBoxGetFlatness();
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

            // Humidity линейной шкалой
            if (userPanel_scaleLinear.checkBox_scaleLinear_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleLinear.radioButton_scaleLinear_image;
                NumericUpDown numericUpDownX = userPanel_scaleLinear.numericUpDown_scaleLinearX;
                NumericUpDown numericUpDownY = userPanel_scaleLinear.numericUpDown_scaleLinearY;
                NumericUpDown numericUpDown_length = userPanel_scaleLinear.numericUpDown_scaleLinear_length;
                NumericUpDown numericUpDown_width = userPanel_scaleLinear.numericUpDown_scaleLinear_width;
                //ComboBox comboBox_flatness = (ComboBox)panel_scaleLinear.Controls[11];

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                int imageIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImage();
                int pointerIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImagePointer();
                int backgroundIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImageBackground();
                int length = (int)numericUpDown_length.Value;
                int width = (int)numericUpDown_width.Value;
                Color color = userPanel_scaleLinear.comboBoxGetColor();
                float position = (float)Watch_Face_Preview_Set.Weather.Humidity / 100f;
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleLinear.comboBoxGetFlatness();

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

            // Humidity надписью
            if (userPanel_text.checkBox_Use.Checked)
            {
                int imageIndex = userPanel_text.comboBoxGetSelectedIndexImage();
                int unit = userPanel_text.comboBoxGetSelectedIndexIcon();
                NumericUpDown numericUpDownX = userPanel_text.numericUpDown_imageX;
                NumericUpDown numericUpDownY = userPanel_text.numericUpDown_imageY;
                NumericUpDown numericUpDown_unitX = userPanel_text.numericUpDown_iconX;
                NumericUpDown numericUpDown_unitY = userPanel_text.numericUpDown_iconY;
                NumericUpDown numericUpDown_spacing = userPanel_text.numericUpDown_spacing;
                CheckBox checkBox_add_zero = userPanel_text.checkBox_addZero;

                if (imageIndex >= 0)
                {
                    //int imageIndex = comboBox_image.SelectedIndex;
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = userPanel_text.comboBoxGetSelectedIndexAlignment();
                    bool addZero = checkBox_add_zero.Checked;
                    int value = Watch_Face_Preview_Set.Weather.Humidity;
                    int separator_index = userPanel_text.comboBoxGetSelectedIndexUnit();
                    Draw_dagital_text(gPanel, imageIndex, x, y,
                        spasing, alignment, value, addZero, 2, separator_index, BBorder);

                    if (unit >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[unit]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // Humidity стрелкой
            if (userPanel_hand.checkBox_hand_Use.Checked)
            {
                int image_index = userPanel_hand.comboBoxGetSelectedIndexHandImage();
                if (image_index >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_hand.numericUpDown_handX;
                    NumericUpDown numericUpDownY = userPanel_hand.numericUpDown_handY;
                    NumericUpDown numericUpDown_offsetX = userPanel_hand.numericUpDown_handX_offset;
                    NumericUpDown numericUpDown_offsetY = userPanel_hand.numericUpDown_handY_offset;
                    int imageCentr = userPanel_hand.comboBoxGetSelectedIndexHandImageCentr();
                    NumericUpDown numericUpDownX_centr = userPanel_hand.numericUpDown_handX_centr;
                    NumericUpDown numericUpDownY_centr = userPanel_hand.numericUpDown_handY_centr;
                    NumericUpDown numericUpDown_startAngle = userPanel_hand.numericUpDown_hand_startAngle;
                    NumericUpDown numericUpDown_endAngle = userPanel_hand.numericUpDown_hand_endAngle;
                    int imageBackground = userPanel_hand.comboBoxGetSelectedIndexHandImageBackground();
                    NumericUpDown numericUpDownX_background = userPanel_hand.numericUpDown_handX_background;
                    NumericUpDown numericUpDownY_background = userPanel_hand.numericUpDown_handY_background;

                    if (imageBackground >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[imageBackground]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_background.Value,
                            (int)numericUpDownY_background.Value, src.Width, src.Height));
                    }

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int offsetX = (int)numericUpDown_offsetX.Value;
                    int offsetY = (int)numericUpDown_offsetY.Value;
                    //int image_index = comboBox_image.SelectedIndex;
                    float startAngle = (float)(numericUpDown_startAngle.Value);
                    float endAngle = (float)(numericUpDown_endAngle.Value);

                    float angle = startAngle + Watch_Face_Preview_Set.Weather.Humidity * (endAngle - startAngle) / 100f;
                    if (Watch_Face_Preview_Set.Weather.Humidity > 100) angle = endAngle;
                    DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                    if (imageCentr >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[imageCentr]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_centr.Value,
                            (int)numericUpDownY_centr.Value, src.Width, src.Height));
                    }
                }
            }
            #endregion

            #region WindForce
            userPanel_pictures = userControl_pictures_WindForce_AOD;
            userPanel_text = userControl_text_WindForce_AOD;
            userPanel_hand = userControl_hand_WindForce_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_WindForce_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_WindForce_AOD;
            // WindForce картинками
            if (userPanel_pictures.checkBox_pictures_Use.Checked)
            {
                //ComboBox comboBox_image = (ComboBox)panel_pictures.Controls[1];
                if (userPanel_pictures.comboBoxGetImage() >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_pictures.numericUpDown_picturesX;
                    NumericUpDown numericUpDownY = userPanel_pictures.numericUpDown_picturesY;
                    NumericUpDown numericUpDown_count = userPanel_pictures.numericUpDown_pictures_count;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int count = (int)numericUpDown_count.Value;
                    //int offSet = (int)Math.Ceiling((float)count * Watch_Face_Preview_Set.Activity.PAI / 100f);
                    int offSet = (int)((count + 1f) * Watch_Face_Preview_Set.Weather.WindForce / 12f);
                    //offSet--;
                    if (offSet < 0) offSet = 0;
                    if (offSet >= count) offSet = (int)(count - 1);
                    int imageIndex = userPanel_pictures.comboBoxGetSelectedIndexImage() + offSet;

                    if (imageIndex < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[imageIndex]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }
                }
            }

            // WindForce круговой шкалой
            if (userPanel_scaleCircle.checkBox_scaleCircle_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleCircle.radioButton_scaleCircle_image;
                NumericUpDown numericUpDownX = userPanel_scaleCircle.numericUpDown_scaleCircleX;
                NumericUpDown numericUpDownY = userPanel_scaleCircle.numericUpDown_scaleCircleY;
                NumericUpDown numericUpDown_radius = userPanel_scaleCircle.numericUpDown_scaleCircle_radius;
                NumericUpDown numericUpDown_width = userPanel_scaleCircle.numericUpDown_scaleCircle_width;
                NumericUpDown numericUpDown_startAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_startAngle;
                NumericUpDown numericUpDown_endAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_endAngle;

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                float width = (float)numericUpDown_width.Value;
                int radius = (int)numericUpDown_radius.Value;
                int imageIndex = userPanel_scaleCircle.comboBoxGetSelectedIndexImage();
                int imageBackground = userPanel_scaleCircle.comboBoxGetSelectedIndexImageBackground();
                float StartAngle = (float)numericUpDown_startAngle.Value - 90;
                float EndAngle = (float)(numericUpDown_endAngle.Value -
                    numericUpDown_startAngle.Value);
                Color color = userPanel_scaleCircle.comboBoxGetColor();
                float position = (float)Watch_Face_Preview_Set.Weather.WindForce / 12f;
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleCircle.comboBoxGetFlatness();
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

            // WindForce линейной шкалой
            if (userPanel_scaleLinear.checkBox_scaleLinear_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleLinear.radioButton_scaleLinear_image;
                NumericUpDown numericUpDownX = userPanel_scaleLinear.numericUpDown_scaleLinearX;
                NumericUpDown numericUpDownY = userPanel_scaleLinear.numericUpDown_scaleLinearY;
                NumericUpDown numericUpDown_length = userPanel_scaleLinear.numericUpDown_scaleLinear_length;
                NumericUpDown numericUpDown_width = userPanel_scaleLinear.numericUpDown_scaleLinear_width;
                //ComboBox comboBox_flatness = (ComboBox)panel_scaleLinear.Controls[11];

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                int imageIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImage();
                int pointerIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImagePointer();
                int backgroundIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImageBackground();
                int length = (int)numericUpDown_length.Value;
                int width = (int)numericUpDown_width.Value;
                Color color = userPanel_scaleLinear.comboBoxGetColor();
                float position = (float)Watch_Face_Preview_Set.Weather.WindForce / 12f;
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleLinear.comboBoxGetFlatness();

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

            // WindForce надписью
            if (userPanel_text.checkBox_Use.Checked)
            {
                int imageIndex = userPanel_text.comboBoxGetSelectedIndexImage();
                int unit = userPanel_text.comboBoxGetSelectedIndexIcon();
                NumericUpDown numericUpDownX = userPanel_text.numericUpDown_imageX;
                NumericUpDown numericUpDownY = userPanel_text.numericUpDown_imageY;
                NumericUpDown numericUpDown_unitX = userPanel_text.numericUpDown_iconX;
                NumericUpDown numericUpDown_unitY = userPanel_text.numericUpDown_iconY;
                NumericUpDown numericUpDown_spacing = userPanel_text.numericUpDown_spacing;
                CheckBox checkBox_add_zero = userPanel_text.checkBox_addZero;

                if (imageIndex >= 0)
                {
                    //int imageIndex = comboBox_image.SelectedIndex;
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = userPanel_text.comboBoxGetSelectedIndexAlignment();
                    bool addZero = checkBox_add_zero.Checked;
                    int value = Watch_Face_Preview_Set.Weather.WindForce;
                    int separator_index = userPanel_text.comboBoxGetSelectedIndexUnit();
                    Draw_dagital_text(gPanel, imageIndex, x, y,
                        spasing, alignment, value, addZero, 2, separator_index, BBorder);

                    if (unit >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[unit]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // WindForce стрелкой
            if (userPanel_hand.checkBox_hand_Use.Checked)
            {
                int image_index = userPanel_hand.comboBoxGetSelectedIndexHandImage();
                if (image_index >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_hand.numericUpDown_handX;
                    NumericUpDown numericUpDownY = userPanel_hand.numericUpDown_handY;
                    NumericUpDown numericUpDown_offsetX = userPanel_hand.numericUpDown_handX_offset;
                    NumericUpDown numericUpDown_offsetY = userPanel_hand.numericUpDown_handY_offset;
                    int imageCentr = userPanel_hand.comboBoxGetSelectedIndexHandImageCentr();
                    NumericUpDown numericUpDownX_centr = userPanel_hand.numericUpDown_handX_centr;
                    NumericUpDown numericUpDownY_centr = userPanel_hand.numericUpDown_handY_centr;
                    NumericUpDown numericUpDown_startAngle = userPanel_hand.numericUpDown_hand_startAngle;
                    NumericUpDown numericUpDown_endAngle = userPanel_hand.numericUpDown_hand_endAngle;
                    int imageBackground = userPanel_hand.comboBoxGetSelectedIndexHandImageBackground();
                    NumericUpDown numericUpDownX_background = userPanel_hand.numericUpDown_handX_background;
                    NumericUpDown numericUpDownY_background = userPanel_hand.numericUpDown_handY_background;

                    if (imageBackground >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[imageBackground]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_background.Value,
                            (int)numericUpDownY_background.Value, src.Width, src.Height));
                    }

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int offsetX = (int)numericUpDown_offsetX.Value;
                    int offsetY = (int)numericUpDown_offsetY.Value;
                    //int image_index = comboBox_image.SelectedIndex;
                    float startAngle = (float)(numericUpDown_startAngle.Value);
                    float endAngle = (float)(numericUpDown_endAngle.Value);

                    float angle = startAngle + Watch_Face_Preview_Set.Weather.WindForce * (endAngle - startAngle) / 12f;
                    if (Watch_Face_Preview_Set.Weather.WindForce > 12) angle = endAngle;
                    DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                    if (imageCentr >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[imageCentr]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_centr.Value,
                            (int)numericUpDownY_centr.Value, src.Width, src.Height));
                    }
                }
            }
            #endregion

            #region Altitude
            userPanel_pictures = userControl_pictures_Altitude_AOD;
            userPanel_text = userControl_text_Altitude_AOD;
            userPanel_hand = userControl_hand_Altitude_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_Altitude_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_Altitude_AOD;
            // Altitude картинками
            if (userPanel_pictures.checkBox_pictures_Use.Checked)
            {
                //ComboBox comboBox_image = (ComboBox)panel_pictures.Controls[1];
                if (userPanel_pictures.comboBoxGetImage() >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_pictures.numericUpDown_picturesX;
                    NumericUpDown numericUpDownY = userPanel_pictures.numericUpDown_picturesY;
                    NumericUpDown numericUpDown_count = userPanel_pictures.numericUpDown_pictures_count;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int count = (int)numericUpDown_count.Value;
                    //int offSet = (int)Math.Ceiling((float)count * Watch_Face_Preview_Set.Activity.PAI / 100f);
                    int offSet = (int)((count + 1f) * (Watch_Face_Preview_Set.Weather.Altitude + 1000) / 10000f);
                    //offSet--;
                    if (offSet < 0) offSet = 0;
                    if (offSet >= count) offSet = (int)(count - 1);
                    int imageIndex = userPanel_pictures.comboBoxGetSelectedIndexImage() + offSet;

                    if (imageIndex < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[imageIndex]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }
                }
            }

            // Altitude круговой шкалой
            if (userPanel_scaleCircle.checkBox_scaleCircle_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleCircle.radioButton_scaleCircle_image;
                NumericUpDown numericUpDownX = userPanel_scaleCircle.numericUpDown_scaleCircleX;
                NumericUpDown numericUpDownY = userPanel_scaleCircle.numericUpDown_scaleCircleY;
                NumericUpDown numericUpDown_radius = userPanel_scaleCircle.numericUpDown_scaleCircle_radius;
                NumericUpDown numericUpDown_width = userPanel_scaleCircle.numericUpDown_scaleCircle_width;
                NumericUpDown numericUpDown_startAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_startAngle;
                NumericUpDown numericUpDown_endAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_endAngle;

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                float width = (float)numericUpDown_width.Value;
                int radius = (int)numericUpDown_radius.Value;
                int imageIndex = userPanel_scaleCircle.comboBoxGetSelectedIndexImage();
                int imageBackground = userPanel_scaleCircle.comboBoxGetSelectedIndexImageBackground();
                float StartAngle = (float)numericUpDown_startAngle.Value - 90;
                float EndAngle = (float)(numericUpDown_endAngle.Value -
                    numericUpDown_startAngle.Value);
                Color color = userPanel_scaleCircle.comboBoxGetColor();
                float position = (float)(Watch_Face_Preview_Set.Weather.Altitude + 1000) / 10000f;
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleCircle.comboBoxGetFlatness();
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

            // Altitude линейной шкалой
            if (userPanel_scaleLinear.checkBox_scaleLinear_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleLinear.radioButton_scaleLinear_image;
                NumericUpDown numericUpDownX = userPanel_scaleLinear.numericUpDown_scaleLinearX;
                NumericUpDown numericUpDownY = userPanel_scaleLinear.numericUpDown_scaleLinearY;
                NumericUpDown numericUpDown_length = userPanel_scaleLinear.numericUpDown_scaleLinear_length;
                NumericUpDown numericUpDown_width = userPanel_scaleLinear.numericUpDown_scaleLinear_width;
                //ComboBox comboBox_flatness = (ComboBox)panel_scaleLinear.Controls[11];

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                int imageIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImage();
                int pointerIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImagePointer();
                int backgroundIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImageBackground();
                int length = (int)numericUpDown_length.Value;
                int width = (int)numericUpDown_width.Value;
                Color color = userPanel_scaleLinear.comboBoxGetColor();
                float position = (float)(Watch_Face_Preview_Set.Weather.Altitude + 1000) / 10000f;
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleLinear.comboBoxGetFlatness();

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

            // Altitude надписью
            if (userPanel_text.checkBox_Use.Checked)
            {
                int imageIndex = userPanel_text.comboBoxGetSelectedIndexImage();
                int unit = userPanel_text.comboBoxGetSelectedIndexIcon();
                NumericUpDown numericUpDownX = userPanel_text.numericUpDown_imageX;
                NumericUpDown numericUpDownY = userPanel_text.numericUpDown_imageY;
                NumericUpDown numericUpDown_unitX = userPanel_text.numericUpDown_iconX;
                NumericUpDown numericUpDown_unitY = userPanel_text.numericUpDown_iconY;
                NumericUpDown numericUpDown_spacing = userPanel_text.numericUpDown_spacing;
                CheckBox checkBox_add_zero = userPanel_text.checkBox_addZero;

                if (imageIndex >= 0)
                {
                    //int imageIndex = comboBox_image.SelectedIndex;
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = userPanel_text.comboBoxGetSelectedIndexAlignment();
                    bool addZero = checkBox_add_zero.Checked;
                    int value = Watch_Face_Preview_Set.Weather.Altitude;
                    int separator_index = userPanel_text.comboBoxGetSelectedIndexUnit();
                    Draw_dagital_text(gPanel, imageIndex, x, y,
                        spasing, alignment, value, addZero, 2, separator_index, BBorder);

                    if (unit >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[unit]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // Altitude стрелкой
            if (userPanel_hand.checkBox_hand_Use.Checked)
            {
                int image_index = userPanel_hand.comboBoxGetSelectedIndexHandImage();
                if (image_index >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_hand.numericUpDown_handX;
                    NumericUpDown numericUpDownY = userPanel_hand.numericUpDown_handY;
                    NumericUpDown numericUpDown_offsetX = userPanel_hand.numericUpDown_handX_offset;
                    NumericUpDown numericUpDown_offsetY = userPanel_hand.numericUpDown_handY_offset;
                    int imageCentr = userPanel_hand.comboBoxGetSelectedIndexHandImageCentr();
                    NumericUpDown numericUpDownX_centr = userPanel_hand.numericUpDown_handX_centr;
                    NumericUpDown numericUpDownY_centr = userPanel_hand.numericUpDown_handY_centr;
                    NumericUpDown numericUpDown_startAngle = userPanel_hand.numericUpDown_hand_startAngle;
                    NumericUpDown numericUpDown_endAngle = userPanel_hand.numericUpDown_hand_endAngle;
                    int imageBackground = userPanel_hand.comboBoxGetSelectedIndexHandImageBackground();
                    NumericUpDown numericUpDownX_background = userPanel_hand.numericUpDown_handX_background;
                    NumericUpDown numericUpDownY_background = userPanel_hand.numericUpDown_handY_background;

                    if (imageBackground >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[imageBackground]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_background.Value,
                            (int)numericUpDownY_background.Value, src.Width, src.Height));
                    }

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int offsetX = (int)numericUpDown_offsetX.Value;
                    int offsetY = (int)numericUpDown_offsetY.Value;
                    //int image_index = comboBox_image.SelectedIndex;
                    float startAngle = (float)(numericUpDown_startAngle.Value);
                    float endAngle = (float)(numericUpDown_endAngle.Value);

                    float angle = startAngle + (Watch_Face_Preview_Set.Weather.Altitude + 1000) * (endAngle - startAngle) / 10000f;
                    if (Watch_Face_Preview_Set.Weather.Altitude > 9000) angle = endAngle;
                    DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                    if (imageCentr >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[imageCentr]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_centr.Value,
                            (int)numericUpDownY_centr.Value, src.Width, src.Height));
                    }
                }
            }
            #endregion

            #region AirPressure
            userPanel_pictures = userControl_pictures_AirPressure_AOD;
            userPanel_text = userControl_text_AirPressure_AOD;
            userPanel_hand = userControl_hand_AirPressure_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_AirPressure_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_AirPressure_AOD;
            // AirPressure картинками
            if (userPanel_pictures.checkBox_pictures_Use.Checked)
            {
                //ComboBox comboBox_image = (ComboBox)panel_pictures.Controls[1];
                if (userPanel_pictures.comboBoxGetImage() >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_pictures.numericUpDown_picturesX;
                    NumericUpDown numericUpDownY = userPanel_pictures.numericUpDown_picturesY;
                    NumericUpDown numericUpDown_count = userPanel_pictures.numericUpDown_pictures_count;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int count = (int)numericUpDown_count.Value;
                    //int offSet = (int)Math.Ceiling((float)count * Watch_Face_Preview_Set.Activity.PAI / 100f);
                    int offSet = (int)((count + 1f) * (Watch_Face_Preview_Set.Weather.AirPressure - 200) / 1000f);
                    //offSet--;
                    if (offSet < 0) offSet = 0;
                    if (offSet >= count) offSet = (int)(count - 1);
                    int imageIndex = userPanel_pictures.comboBoxGetSelectedIndexImage() + offSet;

                    if (imageIndex < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[imageIndex]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }
                }
            }

            // AirPressure круговой шкалой
            if (userPanel_scaleCircle.checkBox_scaleCircle_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleCircle.radioButton_scaleCircle_image;
                NumericUpDown numericUpDownX = userPanel_scaleCircle.numericUpDown_scaleCircleX;
                NumericUpDown numericUpDownY = userPanel_scaleCircle.numericUpDown_scaleCircleY;
                NumericUpDown numericUpDown_radius = userPanel_scaleCircle.numericUpDown_scaleCircle_radius;
                NumericUpDown numericUpDown_width = userPanel_scaleCircle.numericUpDown_scaleCircle_width;
                NumericUpDown numericUpDown_startAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_startAngle;
                NumericUpDown numericUpDown_endAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_endAngle;

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                float width = (float)numericUpDown_width.Value;
                int radius = (int)numericUpDown_radius.Value;
                int imageIndex = userPanel_scaleCircle.comboBoxGetSelectedIndexImage();
                int imageBackground = userPanel_scaleCircle.comboBoxGetSelectedIndexImageBackground();
                float StartAngle = (float)numericUpDown_startAngle.Value - 90;
                float EndAngle = (float)(numericUpDown_endAngle.Value -
                    numericUpDown_startAngle.Value);
                Color color = userPanel_scaleCircle.comboBoxGetColor();
                float position = (float)(Watch_Face_Preview_Set.Weather.AirPressure - 170) / 1000f;
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleCircle.comboBoxGetFlatness();
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

            // AirPressure линейной шкалой
            if (userPanel_scaleLinear.checkBox_scaleLinear_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleLinear.radioButton_scaleLinear_image;
                NumericUpDown numericUpDownX = userPanel_scaleLinear.numericUpDown_scaleLinearX;
                NumericUpDown numericUpDownY = userPanel_scaleLinear.numericUpDown_scaleLinearY;
                NumericUpDown numericUpDown_length = userPanel_scaleLinear.numericUpDown_scaleLinear_length;
                NumericUpDown numericUpDown_width = userPanel_scaleLinear.numericUpDown_scaleLinear_width;
                //ComboBox comboBox_flatness = (ComboBox)panel_scaleLinear.Controls[11];

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                int imageIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImage();
                int pointerIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImagePointer();
                int backgroundIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImageBackground();
                int length = (int)numericUpDown_length.Value;
                int width = (int)numericUpDown_width.Value;
                Color color = userPanel_scaleLinear.comboBoxGetColor();
                float position = (float)(Watch_Face_Preview_Set.Weather.AirPressure - 200) / 1000f;
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleLinear.comboBoxGetFlatness();

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

            // AirPressure надписью
            if (userPanel_text.checkBox_Use.Checked)
            {
                int imageIndex = userPanel_text.comboBoxGetSelectedIndexImage();
                int unit = userPanel_text.comboBoxGetSelectedIndexIcon();
                NumericUpDown numericUpDownX = userPanel_text.numericUpDown_imageX;
                NumericUpDown numericUpDownY = userPanel_text.numericUpDown_imageY;
                NumericUpDown numericUpDown_unitX = userPanel_text.numericUpDown_iconX;
                NumericUpDown numericUpDown_unitY = userPanel_text.numericUpDown_iconY;
                NumericUpDown numericUpDown_spacing = userPanel_text.numericUpDown_spacing;
                CheckBox checkBox_add_zero = userPanel_text.checkBox_addZero;

                if (imageIndex >= 0)
                {
                    //int imageIndex = comboBox_image.SelectedIndex;
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = userPanel_text.comboBoxGetSelectedIndexAlignment();
                    bool addZero = checkBox_add_zero.Checked;
                    int value = Watch_Face_Preview_Set.Weather.AirPressure;
                    int separator_index = userPanel_text.comboBoxGetSelectedIndexUnit();
                    Draw_dagital_text(gPanel, imageIndex, x, y,
                        spasing, alignment, value, addZero, 4, separator_index, BBorder);

                    if (unit >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[unit]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // AirPressure стрелкой
            if (userPanel_hand.checkBox_hand_Use.Checked)
            {
                int image_index = userPanel_hand.comboBoxGetSelectedIndexHandImage();
                if (image_index >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_hand.numericUpDown_handX;
                    NumericUpDown numericUpDownY = userPanel_hand.numericUpDown_handY;
                    NumericUpDown numericUpDown_offsetX = userPanel_hand.numericUpDown_handX_offset;
                    NumericUpDown numericUpDown_offsetY = userPanel_hand.numericUpDown_handY_offset;
                    int imageCentr = userPanel_hand.comboBoxGetSelectedIndexHandImageCentr();
                    NumericUpDown numericUpDownX_centr = userPanel_hand.numericUpDown_handX_centr;
                    NumericUpDown numericUpDownY_centr = userPanel_hand.numericUpDown_handY_centr;
                    NumericUpDown numericUpDown_startAngle = userPanel_hand.numericUpDown_hand_startAngle;
                    NumericUpDown numericUpDown_endAngle = userPanel_hand.numericUpDown_hand_endAngle;
                    int imageBackground = userPanel_hand.comboBoxGetSelectedIndexHandImageBackground();
                    NumericUpDown numericUpDownX_background = userPanel_hand.numericUpDown_handX_background;
                    NumericUpDown numericUpDownY_background = userPanel_hand.numericUpDown_handY_background;

                    if (imageBackground >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[imageBackground]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_background.Value,
                            (int)numericUpDownY_background.Value, src.Width, src.Height));
                    }

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int offsetX = (int)numericUpDown_offsetX.Value;
                    int offsetY = (int)numericUpDown_offsetY.Value;
                    //int image_index = comboBox_image.SelectedIndex;
                    float startAngle = (float)(numericUpDown_startAngle.Value);
                    float endAngle = (float)(numericUpDown_endAngle.Value);

                    float angle = startAngle + (Watch_Face_Preview_Set.Weather.AirPressure - 200) * (endAngle - startAngle) / 1000f;
                    if (Watch_Face_Preview_Set.Weather.AirPressure > 1200) angle = endAngle;
                    DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                    if (imageCentr >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[imageCentr]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_centr.Value,
                            (int)numericUpDownY_centr.Value, src.Width, src.Height));
                    }
                }
            }
            #endregion

            #region Stress
            userPanel_pictures = userControl_pictures_Stress_AOD;
            userPanel_text = userControl_text_Stress_AOD;
            userPanel_hand = userControl_hand_Stress_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_Stress_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_Stress_AOD;
            // Stress картинками
            if (userPanel_pictures.checkBox_pictures_Use.Checked)
            {
                //ComboBox comboBox_image = (ComboBox)panel_pictures.Controls[1];
                if (userPanel_pictures.comboBoxGetImage() >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_pictures.numericUpDown_picturesX;
                    NumericUpDown numericUpDownY = userPanel_pictures.numericUpDown_picturesY;
                    NumericUpDown numericUpDown_count = userPanel_pictures.numericUpDown_pictures_count;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int count = (int)numericUpDown_count.Value;
                    //int offSet = (int)Math.Ceiling((float)count * Watch_Face_Preview_Set.Activity.PAI / 100f);
                    int offSet = (int)((count + 1f) * Watch_Face_Preview_Set.Activity.Stress / 12f);
                    //offSet--;
                    if (offSet < 0) offSet = 0;
                    if (offSet >= count) offSet = (int)(count - 1);
                    int imageIndex = userPanel_pictures.comboBoxGetSelectedIndexImage() + offSet;

                    if (imageIndex < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[imageIndex]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }
                }
            }

            // Stress круговой шкалой
            if (userPanel_scaleCircle.checkBox_scaleCircle_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleCircle.radioButton_scaleCircle_image;
                NumericUpDown numericUpDownX = userPanel_scaleCircle.numericUpDown_scaleCircleX;
                NumericUpDown numericUpDownY = userPanel_scaleCircle.numericUpDown_scaleCircleY;
                NumericUpDown numericUpDown_radius = userPanel_scaleCircle.numericUpDown_scaleCircle_radius;
                NumericUpDown numericUpDown_width = userPanel_scaleCircle.numericUpDown_scaleCircle_width;
                NumericUpDown numericUpDown_startAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_startAngle;
                NumericUpDown numericUpDown_endAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_endAngle;

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                float width = (float)numericUpDown_width.Value;
                int radius = (int)numericUpDown_radius.Value;
                int imageIndex = userPanel_scaleCircle.comboBoxGetSelectedIndexImage();
                int imageBackground = userPanel_scaleCircle.comboBoxGetSelectedIndexImageBackground();
                float StartAngle = (float)numericUpDown_startAngle.Value - 90;
                float EndAngle = (float)(numericUpDown_endAngle.Value -
                    numericUpDown_startAngle.Value);
                Color color = userPanel_scaleCircle.comboBoxGetColor();
                float position = (float)Watch_Face_Preview_Set.Activity.Stress / 12f;
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleCircle.comboBoxGetFlatness();
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

            // Stress линейной шкалой
            if (userPanel_scaleLinear.checkBox_scaleLinear_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleLinear.radioButton_scaleLinear_image;
                NumericUpDown numericUpDownX = userPanel_scaleLinear.numericUpDown_scaleLinearX;
                NumericUpDown numericUpDownY = userPanel_scaleLinear.numericUpDown_scaleLinearY;
                NumericUpDown numericUpDown_length = userPanel_scaleLinear.numericUpDown_scaleLinear_length;
                NumericUpDown numericUpDown_width = userPanel_scaleLinear.numericUpDown_scaleLinear_width;
                //ComboBox comboBox_flatness = (ComboBox)panel_scaleLinear.Controls[11];

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                int imageIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImage();
                int pointerIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImagePointer();
                int backgroundIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImageBackground();
                int length = (int)numericUpDown_length.Value;
                int width = (int)numericUpDown_width.Value;
                Color color = userPanel_scaleLinear.comboBoxGetColor();
                float position = (float)Watch_Face_Preview_Set.Activity.Stress / 12f;
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleLinear.comboBoxGetFlatness();

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

            // Stress надписью
            if (userPanel_text.checkBox_Use.Checked)
            {
                int imageIndex = userPanel_text.comboBoxGetSelectedIndexImage();
                int unit = userPanel_text.comboBoxGetSelectedIndexIcon();
                NumericUpDown numericUpDownX = userPanel_text.numericUpDown_imageX;
                NumericUpDown numericUpDownY = userPanel_text.numericUpDown_imageY;
                NumericUpDown numericUpDown_unitX = userPanel_text.numericUpDown_iconX;
                NumericUpDown numericUpDown_unitY = userPanel_text.numericUpDown_iconY;
                NumericUpDown numericUpDown_spacing = userPanel_text.numericUpDown_spacing;
                CheckBox checkBox_add_zero = userPanel_text.checkBox_addZero;

                if (imageIndex >= 0)
                {
                    //int imageIndex = comboBox_image.SelectedIndex;
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = userPanel_text.comboBoxGetSelectedIndexAlignment();
                    bool addZero = checkBox_add_zero.Checked;
                    int value = Watch_Face_Preview_Set.Activity.Stress;
                    int separator_index = userPanel_text.comboBoxGetSelectedIndexUnit();
                    Draw_dagital_text(gPanel, imageIndex, x, y,
                        spasing, alignment, value, addZero, 3, separator_index, BBorder);

                    if (unit >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[unit]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // Stress стрелкой
            if (userPanel_hand.checkBox_hand_Use.Checked)
            {
                int image_index = userPanel_hand.comboBoxGetSelectedIndexHandImage();
                if (image_index >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_hand.numericUpDown_handX;
                    NumericUpDown numericUpDownY = userPanel_hand.numericUpDown_handY;
                    NumericUpDown numericUpDown_offsetX = userPanel_hand.numericUpDown_handX_offset;
                    NumericUpDown numericUpDown_offsetY = userPanel_hand.numericUpDown_handY_offset;
                    int imageCentr = userPanel_hand.comboBoxGetSelectedIndexHandImageCentr();
                    NumericUpDown numericUpDownX_centr = userPanel_hand.numericUpDown_handX_centr;
                    NumericUpDown numericUpDownY_centr = userPanel_hand.numericUpDown_handY_centr;
                    NumericUpDown numericUpDown_startAngle = userPanel_hand.numericUpDown_hand_startAngle;
                    NumericUpDown numericUpDown_endAngle = userPanel_hand.numericUpDown_hand_endAngle;
                    int imageBackground = userPanel_hand.comboBoxGetSelectedIndexHandImageBackground();
                    NumericUpDown numericUpDownX_background = userPanel_hand.numericUpDown_handX_background;
                    NumericUpDown numericUpDownY_background = userPanel_hand.numericUpDown_handY_background;

                    if (imageBackground >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[imageBackground]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_background.Value,
                            (int)numericUpDownY_background.Value, src.Width, src.Height));
                    }

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int offsetX = (int)numericUpDown_offsetX.Value;
                    int offsetY = (int)numericUpDown_offsetY.Value;
                    //int image_index = comboBox_image.SelectedIndex;
                    float startAngle = (float)(numericUpDown_startAngle.Value);
                    float endAngle = (float)(numericUpDown_endAngle.Value);

                    float angle = startAngle + Watch_Face_Preview_Set.Activity.Stress * (endAngle - startAngle) / 12f;
                    if (Watch_Face_Preview_Set.Activity.Stress > 12) angle = endAngle;
                    DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                    if (imageCentr >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[imageCentr]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_centr.Value,
                            (int)numericUpDownY_centr.Value, src.Width, src.Height));
                    }
                }
            }
            #endregion

            #region ActivityGoal
            userPanel_pictures = userControl_pictures_ActivityGoal_AOD;
            userPanel_text = userControl_text_ActivityGoal_AOD;
            userPanel_hand = userControl_hand_ActivityGoal_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_ActivityGoal_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_ActivityGoal_AOD;
            // ActivityGoal картинками
            if (userPanel_pictures.checkBox_pictures_Use.Checked)
            {
                //ComboBox comboBox_image = (ComboBox)panel_pictures.Controls[1];
                if (userPanel_pictures.comboBoxGetImage() >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_pictures.numericUpDown_picturesX;
                    NumericUpDown numericUpDownY = userPanel_pictures.numericUpDown_picturesY;
                    NumericUpDown numericUpDown_count = userPanel_pictures.numericUpDown_pictures_count;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int count = (int)numericUpDown_count.Value;
                    //int offSet = (int)Math.Ceiling((float)count * Watch_Face_Preview_Set.Activity.PAI / 100f);
                    int offSet = (int)((count + 1f) * Watch_Face_Preview_Set.Activity.ActivityGoal / 12f);
                    //offSet--;
                    if (offSet < 0) offSet = 0;
                    if (offSet >= count) offSet = (int)(count - 1);
                    int imageIndex = userPanel_pictures.comboBoxGetSelectedIndexImage() + offSet;

                    if (imageIndex < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[imageIndex]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }
                }
            }

            // ActivityGoal круговой шкалой
            if (userPanel_scaleCircle.checkBox_scaleCircle_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleCircle.radioButton_scaleCircle_image;
                NumericUpDown numericUpDownX = userPanel_scaleCircle.numericUpDown_scaleCircleX;
                NumericUpDown numericUpDownY = userPanel_scaleCircle.numericUpDown_scaleCircleY;
                NumericUpDown numericUpDown_radius = userPanel_scaleCircle.numericUpDown_scaleCircle_radius;
                NumericUpDown numericUpDown_width = userPanel_scaleCircle.numericUpDown_scaleCircle_width;
                NumericUpDown numericUpDown_startAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_startAngle;
                NumericUpDown numericUpDown_endAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_endAngle;

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                float width = (float)numericUpDown_width.Value;
                int radius = (int)numericUpDown_radius.Value;
                int imageIndex = userPanel_scaleCircle.comboBoxGetSelectedIndexImage();
                int imageBackground = userPanel_scaleCircle.comboBoxGetSelectedIndexImageBackground();
                float StartAngle = (float)numericUpDown_startAngle.Value - 90;
                float EndAngle = (float)(numericUpDown_endAngle.Value -
                    numericUpDown_startAngle.Value);
                Color color = userPanel_scaleCircle.comboBoxGetColor();
                float position = (float)Watch_Face_Preview_Set.Activity.ActivityGoal / 12f;
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleCircle.comboBoxGetFlatness();
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

            // ActivityGoal линейной шкалой
            if (userPanel_scaleLinear.checkBox_scaleLinear_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleLinear.radioButton_scaleLinear_image;
                NumericUpDown numericUpDownX = userPanel_scaleLinear.numericUpDown_scaleLinearX;
                NumericUpDown numericUpDownY = userPanel_scaleLinear.numericUpDown_scaleLinearY;
                NumericUpDown numericUpDown_length = userPanel_scaleLinear.numericUpDown_scaleLinear_length;
                NumericUpDown numericUpDown_width = userPanel_scaleLinear.numericUpDown_scaleLinear_width;
                //ComboBox comboBox_flatness = (ComboBox)panel_scaleLinear.Controls[11];

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                int imageIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImage();
                int pointerIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImagePointer();
                int backgroundIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImageBackground();
                int length = (int)numericUpDown_length.Value;
                int width = (int)numericUpDown_width.Value;
                Color color = userPanel_scaleLinear.comboBoxGetColor();
                float position = (float)Watch_Face_Preview_Set.Activity.ActivityGoal / 12f;
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleLinear.comboBoxGetFlatness();

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

            // ActivityGoal надписью
            if (userPanel_text.checkBox_Use.Checked)
            {
                int imageIndex = userPanel_text.comboBoxGetSelectedIndexImage();
                int unit = userPanel_text.comboBoxGetSelectedIndexIcon();
                NumericUpDown numericUpDownX = userPanel_text.numericUpDown_imageX;
                NumericUpDown numericUpDownY = userPanel_text.numericUpDown_imageY;
                NumericUpDown numericUpDown_unitX = userPanel_text.numericUpDown_iconX;
                NumericUpDown numericUpDown_unitY = userPanel_text.numericUpDown_iconY;
                NumericUpDown numericUpDown_spacing = userPanel_text.numericUpDown_spacing;
                CheckBox checkBox_add_zero = userPanel_text.checkBox_addZero;

                if (imageIndex >= 0)
                {
                    //int imageIndex = comboBox_image.SelectedIndex;
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = userPanel_text.comboBoxGetSelectedIndexAlignment();
                    bool addZero = checkBox_add_zero.Checked;
                    int value = Watch_Face_Preview_Set.Activity.ActivityGoal;
                    int separator_index = userPanel_text.comboBoxGetSelectedIndexUnit();
                    Draw_dagital_text(gPanel, imageIndex, x, y,
                        spasing, alignment, value, addZero, 4, separator_index, BBorder, 17);

                    if (unit >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[unit]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // ActivityGoal стрелкой
            if (userPanel_hand.checkBox_hand_Use.Checked)
            {
                int image_index = userPanel_hand.comboBoxGetSelectedIndexHandImage();
                if (image_index >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_hand.numericUpDown_handX;
                    NumericUpDown numericUpDownY = userPanel_hand.numericUpDown_handY;
                    NumericUpDown numericUpDown_offsetX = userPanel_hand.numericUpDown_handX_offset;
                    NumericUpDown numericUpDown_offsetY = userPanel_hand.numericUpDown_handY_offset;
                    int imageCentr = userPanel_hand.comboBoxGetSelectedIndexHandImageCentr();
                    NumericUpDown numericUpDownX_centr = userPanel_hand.numericUpDown_handX_centr;
                    NumericUpDown numericUpDownY_centr = userPanel_hand.numericUpDown_handY_centr;
                    NumericUpDown numericUpDown_startAngle = userPanel_hand.numericUpDown_hand_startAngle;
                    NumericUpDown numericUpDown_endAngle = userPanel_hand.numericUpDown_hand_endAngle;
                    int imageBackground = userPanel_hand.comboBoxGetSelectedIndexHandImageBackground();
                    NumericUpDown numericUpDownX_background = userPanel_hand.numericUpDown_handX_background;
                    NumericUpDown numericUpDownY_background = userPanel_hand.numericUpDown_handY_background;

                    if (imageBackground >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[imageBackground]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_background.Value,
                            (int)numericUpDownY_background.Value, src.Width, src.Height));
                    }

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int offsetX = (int)numericUpDown_offsetX.Value;
                    int offsetY = (int)numericUpDown_offsetY.Value;
                    //int image_index = comboBox_image.SelectedIndex;
                    float startAngle = (float)(numericUpDown_startAngle.Value);
                    float endAngle = (float)(numericUpDown_endAngle.Value);

                    float angle = startAngle + Watch_Face_Preview_Set.Activity.ActivityGoal * (endAngle - startAngle) / 12f;
                    if (Watch_Face_Preview_Set.Activity.ActivityGoal > 12) angle = endAngle;
                    DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                    if (imageCentr >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[imageCentr]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_centr.Value,
                            (int)numericUpDownY_centr.Value, src.Width, src.Height));
                    }
                }
            }
            #endregion

            #region FatBurning
            userPanel_pictures = userControl_pictures_FatBurning_AOD;
            userPanel_text = userControl_text_FatBurning_AOD;
            userPanel_hand = userControl_hand_FatBurning_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_FatBurning_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_FatBurning_AOD;
            // FatBurning картинками
            if (userPanel_pictures.checkBox_pictures_Use.Checked)
            {
                //ComboBox comboBox_image = (ComboBox)panel_pictures.Controls[1];
                if (userPanel_pictures.comboBoxGetImage() >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_pictures.numericUpDown_picturesX;
                    NumericUpDown numericUpDownY = userPanel_pictures.numericUpDown_picturesY;
                    NumericUpDown numericUpDown_count = userPanel_pictures.numericUpDown_pictures_count;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int count = (int)numericUpDown_count.Value;
                    //int offSet = (int)Math.Ceiling((float)count * Watch_Face_Preview_Set.Activity.PAI / 100f);
                    int offSet = (int)((count + 1f) * Watch_Face_Preview_Set.Activity.FatBurning / 30f);
                    //offSet--;
                    if (offSet < 0) offSet = 0;
                    if (offSet >= count) offSet = (int)(count - 1);
                    int imageIndex = userPanel_pictures.comboBoxGetSelectedIndexImage() + offSet;

                    if (imageIndex < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[imageIndex]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }
                }
            }

            // FatBurning круговой шкалой
            if (userPanel_scaleCircle.checkBox_scaleCircle_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleCircle.radioButton_scaleCircle_image;
                NumericUpDown numericUpDownX = userPanel_scaleCircle.numericUpDown_scaleCircleX;
                NumericUpDown numericUpDownY = userPanel_scaleCircle.numericUpDown_scaleCircleY;
                NumericUpDown numericUpDown_radius = userPanel_scaleCircle.numericUpDown_scaleCircle_radius;
                NumericUpDown numericUpDown_width = userPanel_scaleCircle.numericUpDown_scaleCircle_width;
                NumericUpDown numericUpDown_startAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_startAngle;
                NumericUpDown numericUpDown_endAngle = userPanel_scaleCircle.numericUpDown_scaleCircle_endAngle;

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                float width = (float)numericUpDown_width.Value;
                int radius = (int)numericUpDown_radius.Value;
                int imageIndex = userPanel_scaleCircle.comboBoxGetSelectedIndexImage();
                int imageBackground = userPanel_scaleCircle.comboBoxGetSelectedIndexImageBackground();
                float StartAngle = (float)numericUpDown_startAngle.Value - 90;
                float EndAngle = (float)(numericUpDown_endAngle.Value -
                    numericUpDown_startAngle.Value);
                Color color = userPanel_scaleCircle.comboBoxGetColor();
                float position = (float)Watch_Face_Preview_Set.Activity.FatBurning / 30f;
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleCircle.comboBoxGetFlatness();
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

            // FatBurning линейной шкалой
            if (userPanel_scaleLinear.checkBox_scaleLinear_Use.Checked)
            {
                RadioButton radioButton_image = userPanel_scaleLinear.radioButton_scaleLinear_image;
                NumericUpDown numericUpDownX = userPanel_scaleLinear.numericUpDown_scaleLinearX;
                NumericUpDown numericUpDownY = userPanel_scaleLinear.numericUpDown_scaleLinearY;
                NumericUpDown numericUpDown_length = userPanel_scaleLinear.numericUpDown_scaleLinear_length;
                NumericUpDown numericUpDown_width = userPanel_scaleLinear.numericUpDown_scaleLinear_width;
                //ComboBox comboBox_flatness = (ComboBox)panel_scaleLinear.Controls[11];

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                int imageIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImage();
                int pointerIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImagePointer();
                int backgroundIndex = userPanel_scaleLinear.comboBoxGetSelectedIndexImageBackground();
                int length = (int)numericUpDown_length.Value;
                int width = (int)numericUpDown_width.Value;
                Color color = userPanel_scaleLinear.comboBoxGetColor();
                float position = (float)Watch_Face_Preview_Set.Activity.FatBurning / 30f;
                if (position > 1) position = 1;
                int lineCap = userPanel_scaleLinear.comboBoxGetFlatness();

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

            // FatBurning надписью
            if (userPanel_text.checkBox_Use.Checked)
            {
                int imageIndex = userPanel_text.comboBoxGetSelectedIndexImage();
                int unit = userPanel_text.comboBoxGetSelectedIndexIcon();
                NumericUpDown numericUpDownX = userPanel_text.numericUpDown_imageX;
                NumericUpDown numericUpDownY = userPanel_text.numericUpDown_imageY;
                NumericUpDown numericUpDown_unitX = userPanel_text.numericUpDown_iconX;
                NumericUpDown numericUpDown_unitY = userPanel_text.numericUpDown_iconY;
                NumericUpDown numericUpDown_spacing = userPanel_text.numericUpDown_spacing;
                CheckBox checkBox_add_zero = userPanel_text.checkBox_addZero;

                if (imageIndex >= 0)
                {
                    //int imageIndex = comboBox_image.SelectedIndex;
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = userPanel_text.comboBoxGetSelectedIndexAlignment();
                    bool addZero = checkBox_add_zero.Checked;
                    int value = Watch_Face_Preview_Set.Activity.FatBurning;
                    int separator_index = userPanel_text.comboBoxGetSelectedIndexUnit();
                    Draw_dagital_text(gPanel, imageIndex, x, y,
                        spasing, alignment, value, addZero, 2, separator_index, BBorder);

                    if (unit >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[unit]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // FatBurning стрелкой
            if (userPanel_hand.checkBox_hand_Use.Checked)
            {
                int image_index = userPanel_hand.comboBoxGetSelectedIndexHandImage();
                if (image_index >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_hand.numericUpDown_handX;
                    NumericUpDown numericUpDownY = userPanel_hand.numericUpDown_handY;
                    NumericUpDown numericUpDown_offsetX = userPanel_hand.numericUpDown_handX_offset;
                    NumericUpDown numericUpDown_offsetY = userPanel_hand.numericUpDown_handY_offset;
                    int imageCentr = userPanel_hand.comboBoxGetSelectedIndexHandImageCentr();
                    NumericUpDown numericUpDownX_centr = userPanel_hand.numericUpDown_handX_centr;
                    NumericUpDown numericUpDownY_centr = userPanel_hand.numericUpDown_handY_centr;
                    NumericUpDown numericUpDown_startAngle = userPanel_hand.numericUpDown_hand_startAngle;
                    NumericUpDown numericUpDown_endAngle = userPanel_hand.numericUpDown_hand_endAngle;
                    int imageBackground = userPanel_hand.comboBoxGetSelectedIndexHandImageBackground();
                    NumericUpDown numericUpDownX_background = userPanel_hand.numericUpDown_handX_background;
                    NumericUpDown numericUpDownY_background = userPanel_hand.numericUpDown_handY_background;

                    if (imageBackground >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[imageBackground]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_background.Value,
                            (int)numericUpDownY_background.Value, src.Width, src.Height));
                    }

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int offsetX = (int)numericUpDown_offsetX.Value;
                    int offsetY = (int)numericUpDown_offsetY.Value;
                    //int image_index = comboBox_image.SelectedIndex;
                    float startAngle = (float)(numericUpDown_startAngle.Value);
                    float endAngle = (float)(numericUpDown_endAngle.Value);

                    float angle = startAngle + Watch_Face_Preview_Set.Activity.FatBurning * (endAngle - startAngle) / 30f;
                    if (Watch_Face_Preview_Set.Activity.FatBurning > 30) angle = endAngle;
                    DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                    if (imageCentr >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[imageCentr]);
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
                if (radioButton_TRex_pro.Checked)
                {
                    mask = OpenFileStream(Application.StartupPath + @"\Mask\mask_trex_pro.png");
                }
                mask = FormColor(mask);
                gPanel.DrawImage(mask, new Rectangle(0, 0, mask.Width, mask.Height));
                mask.Dispose();
            }

            FormText();
            Logger.WriteLine("* Preview_AOD (end)");
        }

        /// <summary>формируем изображение на панедли Graphics если AOD нет в циферблате</summary>
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
        public void Preview_AOD_WithoutScreenIdle(Graphics gPanel, float scale, bool crop, bool WMesh, bool BMesh, bool BBorder,
            bool showShortcuts, bool showShortcutsArea, bool showShortcutsBorder, bool showAnimation, bool showProgressArea,
            bool showCentrHend)
        {
            Logger.WriteLine("* Preview_AOD_WithoutScreenIdle");
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
            if (radioButton_TRex_pro.Checked)
            {
                src = OpenFileStream(Application.StartupPath + @"\Mask\mask_trex_pro.png");
            }
            offSet_X = src.Width / 2;
            offSet_Y = src.Height / 2;
            gPanel.DrawImage(src, new Rectangle(0, 0, src.Width, src.Height));
            //src.Dispose();
            #endregion

            #region цифровое время
            int time_offsetX = -1;
            int time_offsetY = -1;
            bool _pm = false;
            // часы
            if (checkBox_Hour_Use.Checked && comboBox_Hour_image.SelectedIndex >= 0)
            {
                int imageIndex = comboBox_Hour_image.SelectedIndex;
                int x = (int)numericUpDown_HourX.Value;
                int y = (int)numericUpDown_HourY.Value;
                time_offsetY = y;
                int spasing = (int)numericUpDown_Hour_spacing.Value;
                int alignment = comboBox_Hour_alignment.SelectedIndex;
                bool addZero = checkBox_Hour_add_zero.Checked;
                //addZero = true;
                int value = Watch_Face_Preview_Set.Time.Hours;
                int separator_index = -1;
                if (comboBox_Hour_separator.SelectedIndex >= 0) separator_index = comboBox_Hour_separator.SelectedIndex;

                if (checkBox_12h_Use.Checked && Program_Settings.ShowIn12hourFormat)
                {
                    if (comboBox_AM_image.SelectedIndex >= 0 && comboBox_PM_image.SelectedIndex >= 0)
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

                if (comboBox_Hour_unit.SelectedIndex >= 0)
                {
                    src = OpenFileStream(ListImagesFullName[comboBox_Hour_unit.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Hour_unitX.Value,
                        (int)numericUpDown_Hour_unitY.Value, src.Width, src.Height));
                }
            }

            // минуты
            if (checkBox_Minute_Use.Checked && comboBox_Minute_image.SelectedIndex >= 0)
            {
                int imageIndex = comboBox_Minute_image.SelectedIndex;
                int x = (int)numericUpDown_MinuteX.Value;
                int y = (int)numericUpDown_MinuteY.Value;
                int spasing = (int)numericUpDown_Minute_spacing.Value;
                int alignment = comboBox_Minute_alignment.SelectedIndex;
                bool addZero = checkBox_Minute_add_zero.Checked;
                addZero = true;
                int value = Watch_Face_Preview_Set.Time.Minutes;
                int separator_index = -1;
                if (comboBox_Minute_separator.SelectedIndex >= 0) separator_index = comboBox_Minute_separator.SelectedIndex;
                if (checkBox_Minute_follow.Checked && time_offsetX > -1)
                {
                    x = time_offsetX;
                    alignment = 0;
                    y = time_offsetY;
                }
                time_offsetY = y;
                time_offsetX = Draw_dagital_text(gPanel, imageIndex, x, y,
                    spasing, alignment, value, addZero, 2, separator_index, BBorder);

                if (comboBox_Minute_unit.SelectedIndex >= 0)
                {
                    src = OpenFileStream(ListImagesFullName[comboBox_Minute_unit.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Minute_unitX.Value,
                        (int)numericUpDown_Minute_unitY.Value, src.Width, src.Height));
                }
            }

            // AM/PM
            if (checkBox_12h_Use.Checked && Program_Settings.ShowIn12hourFormat)
            {
                if (comboBox_AM_image.SelectedIndex >= 0 && comboBox_PM_image.SelectedIndex >= 0)
                {
                    if (_pm)
                    {
                        src = OpenFileStream(ListImagesFullName[comboBox_PM_image.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_PM_X.Value,
                            (int)numericUpDown_PM_Y.Value, src.Width, src.Height));
                    }
                    else
                    {
                        src = OpenFileStream(ListImagesFullName[comboBox_AM_image.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_AM_X.Value,
                            (int)numericUpDown_AM_Y.Value, src.Width, src.Height));
                    }
                }
            }
            #endregion

            #region аналоговое время
            // часы
            if (checkBox_Hour_hand_Use.Checked && comboBox_Hour_hand_image.SelectedIndex >= 0)
            {
                int x = (int)numericUpDown_Hour_handX.Value;
                int y = (int)numericUpDown_Hour_handY.Value;
                int offsetX = (int)numericUpDown_Hour_handX_offset.Value;
                int offsetY = (int)numericUpDown_Hour_handY_offset.Value;
                int image_index = comboBox_Hour_hand_image.SelectedIndex;
                int hour = Watch_Face_Preview_Set.Time.Hours;
                int min = Watch_Face_Preview_Set.Time.Minutes;
                //int sec = Watch_Face_Preview_Set.TimeW.Seconds;
                if (hour >= 12) hour = hour - 12;
                float angle = 360 * hour / 12 + 360 * min / (60 * 12);
                DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                if (comboBox_Hour_hand_imageCentr.SelectedIndex >= 0)
                {
                    src = OpenFileStream(ListImagesFullName[comboBox_Hour_hand_imageCentr.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Hour_handX_centr.Value,
                        (int)numericUpDown_Hour_handY_centr.Value, src.Width, src.Height));
                }
            }

            // минуты
            if (checkBox_Minute_hand_Use.Checked && comboBox_Minute_hand_image.SelectedIndex >= 0)
            {
                int x = (int)numericUpDown_Minute_handX.Value;
                int y = (int)numericUpDown_Minute_handY.Value;
                int offsetX = (int)numericUpDown_Minute_handX_offset.Value;
                int offsetY = (int)numericUpDown_Minute_handY_offset.Value;
                int image_index = comboBox_Minute_hand_image.SelectedIndex;
                int min = Watch_Face_Preview_Set.Time.Minutes;
                float angle = 360 * min / 60;
                DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                if (comboBox_Minute_hand_imageCentr.SelectedIndex >= 0)
                {
                    src = OpenFileStream(ListImagesFullName[comboBox_Minute_hand_imageCentr.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Minute_handX_centr.Value,
                        (int)numericUpDown_Minute_handY_centr.Value, src.Width, src.Height));
                }
            }
            #endregion


            #region Mesh
            Logger.WriteLine("Preview_AOD_WithoutScreenIdle (Mesh)");

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
                if (radioButton_TRex_pro.Checked)
                {
                    mask = new Bitmap(Application.StartupPath + @"\Mask\mask_trex_pro.png");
                }
                mask = FormColor(mask);
                gPanel.DrawImage(mask, new Rectangle(0, 0, mask.Width, mask.Height));
                mask.Dispose();
            }

            FormText();
            Logger.WriteLine("* Preview_AOD_WithoutScreenIdle (end)");

        }
    }
}
