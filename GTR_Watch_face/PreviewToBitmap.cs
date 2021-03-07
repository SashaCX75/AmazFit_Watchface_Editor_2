using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading;
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
        /// <param name="link">1 - если отрисовка только до анимации, 
        /// 2 - если отрисовка только после анимации, в остальных случаях полная отрисовка</param>
        public void PreviewToBitmap(Graphics gPanel, float scale, bool crop, bool WMesh, bool BMesh, bool BBorder, 
            bool showShortcuts, bool showShortcutsArea, bool showShortcutsBorder, bool showAnimation, bool showProgressArea, 
            bool showCentrHend, int link)
        {
            Logger.WriteLine("* PreviewToBitmap");
            var src = new Bitmap(1, 1);
            gPanel.ScaleTransform(scale, scale, MatrixOrder.Prepend);
            int i;
            //gPanel.SmoothingMode = SmoothingMode.AntiAlias;
            //if (link == 2) goto AnimationEnd;

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
            if (radioButton_Background_image.Checked)
            {
                if (comboBox_Background_image.SelectedIndex >= 0)
                {
                    i = comboBox_Background_image.SelectedIndex;
                    src = OpenFileStream(ListImagesFullName[i]);
                    gPanel.DrawImage(src, new Rectangle(0, 0, src.Width, src.Height));
                    //src.Dispose();
                }
            }
            else gPanel.Clear(comboBox_Background_color.BackColor);
            #endregion
                //if (scale == 0.5) gPanel.SmoothingMode = SmoothingMode.AntiAlias;
            gPanel.SmoothingMode = SmoothingMode.AntiAlias;

            #region дата 
            int date_offsetX = -1;
            int date_offsetY = -1;
            // год
            if (checkBox__Year_text_Use.Checked && comboBox_Year_image.SelectedIndex >= 0)
            {
                int imageIndex = comboBox_Year_image.SelectedIndex;
                int x = (int)numericUpDown_YearX.Value;
                int y = (int)numericUpDown_YearY.Value;
                date_offsetY = y;
                int spasing = (int)numericUpDown_Year_spacing.Value;
                int alignment = comboBox_Year_alignment.SelectedIndex;
                bool addZero = checkBox_Year_add_zero.Checked;
                int value = Watch_Face_Preview_Set.Date.Year;
                if (addZero) value = Watch_Face_Preview_Set.Date.Year % 100;
                int separator_index = -1;
                if (comboBox_Year_separator.SelectedIndex >= 0) separator_index = comboBox_Year_separator.SelectedIndex;
                addZero = false;

                date_offsetX = Draw_dagital_text(gPanel, imageIndex, x, y,
                    spasing, alignment, value, addZero, 4, separator_index, BBorder);

                if (comboBox_Year_unit.SelectedIndex >= 0)
                {
                    src = OpenFileStream(ListImagesFullName[comboBox_Year_unit.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Year_unitX.Value,
                        (int)numericUpDown_Year_unitY.Value, src.Width, src.Height));
                }
            }

            // месяц
            if (checkBox_Month_Use.Checked && comboBox_Month_image.SelectedIndex >= 0)
            {
                int imageIndex = comboBox_Month_image.SelectedIndex;
                int x = (int)numericUpDown_MonthX.Value;
                int y = (int)numericUpDown_MonthY.Value;
                int spasing = (int)numericUpDown_Month_spacing.Value;
                int alignment = comboBox_Month_alignment.SelectedIndex;
                bool addZero = checkBox_Month_add_zero.Checked;
                //addZero = true;
                int value = Watch_Face_Preview_Set.Date.Month;
                int separator_index = -1;
                if (comboBox_Month_separator.SelectedIndex >= 0) separator_index = comboBox_Month_separator.SelectedIndex;
                if (checkBox_Month_follow.Checked && date_offsetX > -1)
                {
                    x = date_offsetX;
                    alignment = 0;
                    y = date_offsetY;
                }
                date_offsetY = y;
                date_offsetX = Draw_dagital_text(gPanel, imageIndex, x, y,
                    spasing, alignment, value, addZero, 2, separator_index, BBorder);

                if (comboBox_Month_unit.SelectedIndex >= 0)
                {
                    src = OpenFileStream(ListImagesFullName[comboBox_Month_unit.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Month_unitX.Value,
                        (int)numericUpDown_Month_unitY.Value, src.Width, src.Height));
                }
            }
            else date_offsetX = -1;

            // число
            if (checkBox_Day_Use.Checked && comboBox_Day_image.SelectedIndex >= 0)
            {
                int imageIndex = comboBox_Day_image.SelectedIndex;
                int x = (int)numericUpDown_DayX.Value;
                int y = (int)numericUpDown_DayY.Value;
                int spasing = (int)numericUpDown_Day_spacing.Value;
                int alignment = comboBox_Day_alignment.SelectedIndex;
                bool addZero = checkBox_Day_add_zero.Checked;
                //addZero = true;
                int value = Watch_Face_Preview_Set.Date.Day;
                int separator_index = -1;
                if (comboBox_Day_separator.SelectedIndex >= 0) separator_index = comboBox_Day_separator.SelectedIndex;
                if (checkBox_Day_follow.Checked && date_offsetX > -1)
                {
                    x = date_offsetX;
                    alignment = 0;
                    y = date_offsetY;
                }
                Draw_dagital_text(gPanel, imageIndex, x, y,
                    spasing, alignment, value, addZero, 2, separator_index, BBorder);

                if (comboBox_Day_unit.SelectedIndex >= 0)
                {
                    src = OpenFileStream(ListImagesFullName[comboBox_Day_unit.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Day_unitX.Value,
                        (int)numericUpDown_Day_unitY.Value, src.Width, src.Height));
                }
            }

            // число стрелкой
            if (checkBox_Day_hand_Use.Checked && comboBox_Day_hand_image.SelectedIndex >= 0)
            {
                if (comboBox_Day_hand_imageBackground.SelectedIndex >= 0)
                {
                    src = OpenFileStream(ListImagesFullName[comboBox_Day_hand_imageBackground.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Day_handX_background.Value,
                        (int)numericUpDown_Day_handY_background.Value, src.Width, src.Height));
                }

                int x = (int)numericUpDown_Day_handX.Value;
                int y = (int)numericUpDown_Day_handY.Value;
                int offsetX = (int)numericUpDown_Day_handX_offset.Value;
                int offsetY = (int)numericUpDown_Day_handY_offset.Value;
                int image_index = comboBox_Day_hand_image.SelectedIndex;
                float startAngle = (float)(numericUpDown_Day_hand_startAngle.Value);
                float endAngle = (float)(numericUpDown_Day_hand_endAngle.Value);
                int Day = Watch_Face_Preview_Set.Date.Day;
                Day--;
                float angle = startAngle + Day * (endAngle - startAngle) / 30;
                DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                if (comboBox_Day_hand_imageCentr.SelectedIndex >= 0)
                {
                    src = OpenFileStream(ListImagesFullName[comboBox_Day_hand_imageCentr.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Day_handX_centr.Value,
                        (int)numericUpDown_Day_handY_centr.Value, src.Width, src.Height));
                }
            }

            // месяц картинкой
            if (checkBox_Month_pictures_Use.Checked && comboBox_Month_pictures_image.SelectedIndex >= 0)
            {
                int imageIndex = comboBox_Month_pictures_image.SelectedIndex;
                int x = (int)numericUpDown_Month_picturesX.Value;
                int y = (int)numericUpDown_Month_picturesY.Value;
                imageIndex = imageIndex + Watch_Face_Preview_Set.Date.Month - 1;

                if (imageIndex < ListImagesFullName.Count)
                {
                    src = OpenFileStream(ListImagesFullName[imageIndex]);
                    gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                }
            }

            // месяц стрелкой
            if (checkBox_Month_hand_Use.Checked && comboBox_Month_hand_image.SelectedIndex >= 0)
            {
                if (comboBox_Month_hand_imageBackground.SelectedIndex >= 0)
                {
                    src = OpenFileStream(ListImagesFullName[comboBox_Month_hand_imageBackground.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Month_handX_background.Value,
                        (int)numericUpDown_Month_handY_background.Value, src.Width, src.Height));
                }

                int x = (int)numericUpDown_Month_handX.Value;
                int y = (int)numericUpDown_Month_handY.Value;
                int offsetX = (int)numericUpDown_Month_handX_offset.Value;
                int offsetY = (int)numericUpDown_Month_handY_offset.Value;
                int image_index = comboBox_Month_hand_image.SelectedIndex;
                float startAngle = (float)(numericUpDown_Month_hand_startAngle.Value);
                float endAngle = (float)(numericUpDown_Month_hand_endAngle.Value);
                int Month = Watch_Face_Preview_Set.Date.Month;
                Month--;
                float angle = startAngle + Month * (endAngle - startAngle) / 11;
                DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                if (comboBox_Month_hand_imageCentr.SelectedIndex >= 0)
                {
                    src = OpenFileStream(ListImagesFullName[comboBox_Month_hand_imageCentr.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Month_handX_centr.Value,
                        (int)numericUpDown_Month_handY_centr.Value, src.Width, src.Height));
                }
            }

            // день недели картинкой
            if (checkBox_DOW_pictures_Use.Checked && comboBox_DOW_pictures_image.SelectedIndex >= 0)
            {
                int imageIndex = comboBox_DOW_pictures_image.SelectedIndex;
                int x = (int)numericUpDown_DOW_picturesX.Value;
                int y = (int)numericUpDown_DOW_picturesY.Value;
                imageIndex = imageIndex + Watch_Face_Preview_Set.Date.WeekDay - 1;

                if (imageIndex < ListImagesFullName.Count)
                {
                    src = OpenFileStream(ListImagesFullName[imageIndex]);
                    gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                }
            }

            // день недели стрелкой
            if (checkBox_DOW_hand_Use.Checked && comboBox_DOW_hand_image.SelectedIndex >= 0)
            {
                if (comboBox_DOW_hand_imageBackground.SelectedIndex >= 0)
                {
                    src = OpenFileStream(ListImagesFullName[comboBox_DOW_hand_imageBackground.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_DOW_handX_background.Value,
                        (int)numericUpDown_DOW_handY_background.Value, src.Width, src.Height));
                }

                int x = (int)numericUpDown_DOW_handX.Value;
                int y = (int)numericUpDown_DOW_handY.Value;
                int offsetX = (int)numericUpDown_DOW_handX_offset.Value;
                int offsetY = (int)numericUpDown_DOW_handY_offset.Value;
                int image_index = comboBox_DOW_hand_image.SelectedIndex;
                float startAngle = (float)(numericUpDown_DOW_hand_startAngle.Value);
                float endAngle = (float)(numericUpDown_DOW_hand_endAngle.Value);
                int WeekDay = Watch_Face_Preview_Set.Date.WeekDay;
                WeekDay--;
                if (WeekDay < 0) WeekDay = 6;
                float angle = startAngle + WeekDay * (endAngle - startAngle) / 6;
                DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                if (comboBox_DOW_hand_imageCentr.SelectedIndex >= 0)
                {
                    src = OpenFileStream(ListImagesFullName[comboBox_DOW_hand_imageCentr.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_DOW_handX_centr.Value,
                        (int)numericUpDown_DOW_handY_centr.Value, src.Width, src.Height));
                }

            }
            #endregion

            #region статусы

            if (checkBox_Bluetooth_Use.Checked && comboBox_Bluetooth_image.SelectedIndex >= 0)
            {
                if (!Watch_Face_Preview_Set.Status.Bluetooth)
                {
                    src = OpenFileStream(ListImagesFullName[comboBox_Bluetooth_image.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_BluetoothX.Value,
                        (int)numericUpDown_BluetoothY.Value, src.Width, src.Height));
                }
            }

            if (checkBox_Alarm_Use.Checked && comboBox_Alarm_image.SelectedIndex >= 0)
            {
                if (Watch_Face_Preview_Set.Status.Alarm)
                {
                    src = OpenFileStream(ListImagesFullName[comboBox_Alarm_image.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_AlarmX.Value,
                        (int)numericUpDown_AlarmY.Value, src.Width, src.Height));
                }
            }

            if (checkBox_Lock_Use.Checked && comboBox_Lock_image.SelectedIndex >= 0)
            {
                if (Watch_Face_Preview_Set.Status.Lock)
                {
                    src = OpenFileStream(ListImagesFullName[comboBox_Lock_image.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_LockX.Value,
                        (int)numericUpDown_LockY.Value, src.Width, src.Height));
                }
            }

            if (checkBox_DND_Use.Checked && comboBox_DND_image.SelectedIndex >= 0)
            {
                if (Watch_Face_Preview_Set.Status.DoNotDisturb)
                {
                    src = OpenFileStream(ListImagesFullName[comboBox_DND_image.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_DNDX.Value,
                        (int)numericUpDown_DNDY.Value, src.Width, src.Height));
                }
            }
            #endregion

            #region активности
            Panel panel_pictures = panel_Battery_pictures;
            Panel panel_text = panel_Battery_text;
            Panel panel_hand = panel_Battery_hand;
            Panel panel_scaleCircle = panel_Battery_scaleCircle;
            Panel panel_scaleLinear = panel_Battery_scaleLinear;
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
                    int offSet = (int)Math.Ceiling(count * Watch_Face_Preview_Set.Battery / 100f);
                    offSet--;
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

                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                int imageIndex = comboBox_image.SelectedIndex;
                int pointerIndex = comboBox_pointer.SelectedIndex;
                int backgroundIndex = comboBox_background.SelectedIndex;
                int length = (int)numericUpDown_length.Value;
                int width = (int)numericUpDown_width.Value;
                Color color = comboBox_color.BackColor;
                float position = Watch_Face_Preview_Set.Battery / 100f;

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
            panel_pictures = panel_Steps_pictures;
            panel_text = panel_Steps_text;
            panel_hand = panel_Steps_hand;
            panel_scaleCircle = panel_Steps_scaleCircle;
            panel_scaleLinear = panel_Steps_scaleLinear;
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
                    int offSet = (int)Math.Ceiling((float)count * Watch_Face_Preview_Set.Activity.Steps / Watch_Face_Preview_Set.Activity.StepsGoal);
                    offSet--;
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
                    int offSet = (int)Math.Ceiling((float)count * Watch_Face_Preview_Set.Activity.Calories / 300f);
                    offSet--;
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
                    int offSet = (int)Math.Ceiling((float)count * Watch_Face_Preview_Set.Activity.HeartRate / 200f);
                    offSet--;
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
                    int offSet = (int)Math.Ceiling((float)count * Watch_Face_Preview_Set.Activity.PAI / 100f);
                    offSet--;
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
                    int offSet = (int)Math.Ceiling((float)count * Watch_Face_Preview_Set.Activity.Distance / 10000f);
                    offSet--;
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

            #region погода
            panel_pictures = panel_Weather_pictures;
            panel_text = panel_Weather_text;
            panel_hand = panel_Weather_hand;
            panel_scaleCircle = panel_Weather_scaleCircle;
            panel_scaleLinear = panel_Weather_scaleLinear;

            Panel panel_text_min = panel_Weather_textMin;
            Panel panel_text_max = panel_Weather_textMax;

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
                float position = (float)((Watch_Face_Preview_Set.Weather.Temperature + 25) / 60f);
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
            else time_offsetX = -1;

            //секунды
            if (checkBox_Second_Use.Checked && comboBox_Second_image.SelectedIndex >= 0)
            {
                int imageIndex = comboBox_Second_image.SelectedIndex;
                int x = (int)numericUpDown_SecondX.Value;
                int y = (int)numericUpDown_SecondY.Value;
                int spasing = (int)numericUpDown_Second_spacing.Value;
                int alignment = comboBox_Second_alignment.SelectedIndex;
                bool addZero = checkBox_Second_add_zero.Checked;
                addZero = true;
                int value = Watch_Face_Preview_Set.Time.Seconds;
                int separator_index = -1;
                if (comboBox_Second_separator.SelectedIndex >= 0) separator_index = comboBox_Second_separator.SelectedIndex;
                if (checkBox_Second_follow.Checked && time_offsetX > -1)
                {
                    x = time_offsetX;
                    alignment = 0;
                    y = time_offsetY;
                }
                Draw_dagital_text(gPanel, imageIndex, x, y,
                    spasing, alignment, value, addZero, 2, separator_index, BBorder);

                if (comboBox_Second_unit.SelectedIndex >= 0)
                {
                    src = OpenFileStream(ListImagesFullName[comboBox_Second_unit.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Second_unitX.Value,
                        (int)numericUpDown_Second_unitY.Value, src.Width, src.Height));
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

            bool AnalogClockOffSet = false;
            int centerX = 227;
            int centerY = 227;
            if (radioButton_GTS2.Checked)
            {
                centerX = 174;
                centerY = 221;
            }

            if ((numericUpDown_Hour_handX.Value != centerX) ||
                (numericUpDown_Hour_handY.Value != centerY)) AnalogClockOffSet = true;
            if ((numericUpDown_Minute_handX.Value != centerX) ||
                (numericUpDown_Minute_handY.Value != centerY)) AnalogClockOffSet = true;
            if ((numericUpDown_Second_handX.Value != centerX) ||
                (numericUpDown_Second_handY.Value != centerY)) AnalogClockOffSet = true;

            if (AnalogClockOffSet)
            {
                int offsetX_Hour = (int)numericUpDown_Hour_handX.Value;
                int offsetY_Hour = (int)numericUpDown_Hour_handY.Value;
                int offsetX_Min = (int)numericUpDown_Minute_handX.Value;
                int offsetY_Min = (int)numericUpDown_Minute_handY.Value;


                if ((offsetX_Hour != centerX || offsetY_Hour != centerY) && 
                    ((offsetX_Min != centerX || offsetY_Min != centerY))) AnalogClockOffSet = false;
            }

            // секунды
            if (AnalogClockOffSet)
            {
                if (checkBox_Second_hand_Use.Checked && comboBox_Second_hand_image.SelectedIndex >= 0)
                {
                    int x = (int)numericUpDown_Second_handX.Value;
                    int y = (int)numericUpDown_Second_handY.Value;
                    int offsetX = (int)numericUpDown_Second_handX_offset.Value;
                    int offsetY = (int)numericUpDown_Second_handY_offset.Value;
                    int image_index = comboBox_Second_hand_image.SelectedIndex;
                    int sec = Watch_Face_Preview_Set.Time.Seconds;
                    //if (hour >= 12) hour = hour - 12;
                    float angle = 360 * sec / 60;
                    DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                    if (comboBox_Second_hand_imageCentr.SelectedIndex >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[comboBox_Second_hand_imageCentr.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Second_handX_centr.Value,
                            (int)numericUpDown_Second_handY_centr.Value, src.Width, src.Height));
                    }
                }
            }

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

            // секунды
            if (!AnalogClockOffSet)
            {
                if (checkBox_Second_hand_Use.Checked && comboBox_Second_hand_image.SelectedIndex >= 0)
                {
                    int x = (int)numericUpDown_Second_handX.Value;
                    int y = (int)numericUpDown_Second_handY.Value;
                    int offsetX = (int)numericUpDown_Second_handX_offset.Value;
                    int offsetY = (int)numericUpDown_Second_handY_offset.Value;
                    int image_index = comboBox_Second_hand_image.SelectedIndex;
                    int sec = Watch_Face_Preview_Set.Time.Seconds;
                    //if (hour >= 12) hour = hour - 12;
                    float angle = 360 * sec / 60;
                    DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                    if (comboBox_Second_hand_imageCentr.SelectedIndex >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[comboBox_Second_hand_imageCentr.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Second_handX_centr.Value,
                            (int)numericUpDown_Second_handY_centr.Value, src.Width, src.Height));
                    }
                } 
            }
            #endregion



            // ***********************************
            /*
            #region Battery
            Logger.WriteLine("PreviewToBitmap (Battery)");
            if (checkBox_Battery.Checked)
            {
                // заряд числом
                if ((checkBox_Battery_Text.Checked) && (comboBox_Battery_Text_Image.SelectedIndex >= 0))
                {
                    int x1 = (int)numericUpDown_Battery_Text_StartCorner_X.Value;
                    int y1 = (int)numericUpDown_Battery_Text_StartCorner_Y.Value;
                    int x2 = (int)numericUpDown_Battery_Text_EndCorner_X.Value;
                    int y2 = (int)numericUpDown_Battery_Text_EndCorner_Y.Value;
                    x2++;
                    y2++;
                    int image_index = comboBox_Battery_Text_Image.SelectedIndex;
                    int spacing = (int)numericUpDown_Battery_Text_Spacing.Value;
                    int alignment = comboBox_Battery_Text_Alignment.SelectedIndex;
                    int data_number = Watch_Face_Preview_Set.Battery;
                    if ((checkBox_Battery_Percent.Checked) && (comboBox_Battery_Percent_Image.SelectedIndex >= 0) &&
                        (numericUpDown_Battery_Percent_X.Value == 0) && (numericUpDown_Battery_Percent_Y.Value == 0))
                    {
                        if (numericUpDown_Battery_Text_Count.Value == 10)
                            DrawNumber(gPanel, x1, y1, x2, y2, image_index, spacing, alignment, data_number,
                                comboBox_Battery_Percent_Image.SelectedIndex, -1, 0, BBorder);
                    }
                    else if (numericUpDown_Battery_Text_Count.Value == 10)
                        DrawNumber(gPanel, x1, y1, x2, y2, image_index, spacing, alignment, data_number, BBorder);
                }

                // заряд картинкой
                if ((checkBox_Battery_Img.Checked) && (comboBox_Battery_Img_Image.SelectedIndex >= 0))
                {
                    float count = (float)numericUpDown_Battery_Img_Count.Value;
                    if (count > 10) count = 10;
                    int offSet = (int)Math.Ceiling(count * Watch_Face_Preview_Set.Battery / 100f);
                    offSet--;
                    if (offSet < 0) offSet = 0;
                    if (offSet >= count) offSet = (int)(count - 1);
                    //int offSet = (int)Math.Round(count * Watch_Face_Preview_Set.Battery / 100f, 0);
                    i = comboBox_Battery_Img_Image.SelectedIndex + offSet;
                    //src = new Bitmap(ListImagesFullName[i]);
                    src = OpenFileStream(ListImagesFullName[i]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Battery_Img_X.Value,
                        (int)numericUpDown_Battery_Img_Y.Value, src.Width, src.Height));
                    //src.Dispose();
                }

                // значек процентов
                //if ((checkBox_Battery_Percent.Checked) && (comboBox_Battery_Percent_Image.SelectedIndex >= 0))
                if ((checkBox_Battery_Percent.Checked) && (comboBox_Battery_Percent_Image.SelectedIndex >= 0) &&
                        (numericUpDown_Battery_Percent_X.Value != 0) && (numericUpDown_Battery_Percent_Y.Value != 0))
                {
                    if ((checkBox_Battery_Text.Checked) && (comboBox_Battery_Text_Image.SelectedIndex >= 0))
                    {
                        //src = new Bitmap(ListImagesFullName[comboBox_Battery_Percent_Image.SelectedIndex]);
                        src = OpenFileStream(ListImagesFullName[comboBox_Battery_Percent_Image.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Battery_Percent_X.Value,
                            (int)numericUpDown_Battery_Percent_Y.Value, src.Width, src.Height));
                        //src.Dispose();
                    }
                }

                // шкала
                if (checkBox_Battery_Scale.Checked)
                {
                    if (checkBox_Battery_Scale_Image.Checked &&
                        comboBox_Battery_Scale_Image.SelectedIndex >= 0)
                    {
                        int x = (int)numericUpDown_Battery_Scale_ImageX.Value;
                        int y = (int)numericUpDown_Battery_Scale_ImageY.Value;
                        float width = (float)numericUpDown_Battery_Scale_Width.Value;
                        int ImageIndex = comboBox_Battery_Scale_Image.SelectedIndex;
                        int radius = (int)numericUpDown_Battery_Scale_Radius_X.Value;
                        float StartAngle = (float)numericUpDown_Battery_Scale_StartAngle.Value - 90;
                        float EndAngle = (float)(numericUpDown_Battery_Scale_EndAngle.Value -
                            numericUpDown_Battery_Scale_StartAngle.Value);
                        float AngleScale = (float)Watch_Face_Preview_Set.Battery / 100f;
                        if (AngleScale > 1) AngleScale = 1;
                        EndAngle = EndAngle * AngleScale;
                        int lineCap = comboBox_Battery_Flatness.SelectedIndex;
                        CircleOnBitmap(gPanel, x, y, ImageIndex, radius, width, lineCap, StartAngle, EndAngle, showProgressArea);
                    }
                    else
                    {
                        gPanel.SmoothingMode = SmoothingMode.AntiAlias;
                        Pen pen = new Pen(comboBox_Battery_Scale_Color.BackColor,
                            (float)numericUpDown_Battery_Scale_Width.Value);
                        switch (comboBox_Battery_Flatness.SelectedIndex)
                        {
                            case 1:
                                pen.EndCap = LineCap.Triangle;
                                pen.StartCap = LineCap.Triangle;
                                break;
                            case 2:
                                pen.EndCap = LineCap.Flat;
                                pen.StartCap = LineCap.Flat;
                                break;
                            default:
                                pen.EndCap = LineCap.Round;
                                pen.StartCap = LineCap.Round;
                                break;
                        }

                        int x = (int)numericUpDown_Battery_Scale_Center_X.Value -
                            (int)numericUpDown_Battery_Scale_Radius_X.Value;
                        int y = (int)numericUpDown_Battery_Scale_Center_Y.Value -
                            (int)numericUpDown_Battery_Scale_Radius_Y.Value;
                        if (numericUpDown_Battery_Scale_Radius_Y.Value == 0)
                        {
                            y = (int)numericUpDown_Battery_Scale_Center_Y.Value -
                            (int)numericUpDown_Battery_Scale_Radius_X.Value;
                        }
                        int width = (int)numericUpDown_Battery_Scale_Radius_X.Value * 2;
                        //int height = (int)numericUpDown_Battery_Scale_Radius_Y.Value * 2;
                        int height = width;
                        float StartAngle = (float)numericUpDown_Battery_Scale_StartAngle.Value - 90;
                        float EndAngle = (float)(numericUpDown_Battery_Scale_EndAngle.Value -
                            numericUpDown_Battery_Scale_StartAngle.Value);
                        float AngleScale = (float)Watch_Face_Preview_Set.Battery / 100f;
                        if (AngleScale > 1) AngleScale = 1;
                        EndAngle = EndAngle * AngleScale;
                        try
                        {
                            if ((width > 0) && (height > 0)) gPanel.DrawArc(pen, x, y, width, height, StartAngle, EndAngle);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }

                // заряд стрелочный индикатор
                if ((checkBox_Battery_ClockHand.Checked) && (comboBox_Battery_ClockHand_Image.SelectedIndex >= 0))
                {
                    int x1 = (int)numericUpDown_Battery_ClockHand_X.Value;
                    int y1 = (int)numericUpDown_Battery_ClockHand_Y.Value;
                    int offsetX = (int)numericUpDown_Battery_ClockHand_Offset_X.Value;
                    int offsetY = (int)numericUpDown_Battery_ClockHand_Offset_Y.Value;
                    int image_index = comboBox_Battery_ClockHand_Image.SelectedIndex;
                    float startAngle = (float)(numericUpDown_Battery_ClockHand_StartAngle.Value);
                    float endAngle = (float)(numericUpDown_Battery_ClockHand_EndAngle.Value);
                    float angle = startAngle + Watch_Face_Preview_Set.Battery * (endAngle - startAngle) / 100;
                    DrawAnalogClock(gPanel, x1, y1, offsetX, offsetY, image_index, angle);
                }

                // набор иконок
                if (checkBox_Battery_IconSet.Checked)
                {
                    if ((comboBox_Battery_IconSet_Image.SelectedIndex >= 0) && (dataGridView_Battery_IconSet.Rows.Count > 0))
                    {
                        int x = 0;
                        int y = 0;
                        float RowsCount = dataGridView_Battery_IconSet.Rows.Count;
                        RowsCount--;
                        if (RowsCount < 0) RowsCount = 0;
                        if (RowsCount > 10) RowsCount = 10;
                        //int count = 0;
                        for (int count = 0; count < RowsCount; count++)
                        {
                            if ((dataGridView_Battery_IconSet.Rows[count].Cells[0].Value != null) &&
                                (dataGridView_Battery_IconSet.Rows[count].Cells[1].Value != null))
                            {
                                //int x = Int32.Parse(dataGridView_SPSliced.Rows[count].Cells[0].Value.ToString());
                                //int y = Int32.Parse(dataGridView_SPSliced.Rows[count].Cells[1].Value.ToString());
                                if (Int32.TryParse(dataGridView_Battery_IconSet.Rows[count].Cells[0].Value.ToString(), out x) &&
                                    Int32.TryParse(dataGridView_Battery_IconSet.Rows[count].Cells[1].Value.ToString(), out y))
                                {
                                    i = comboBox_Battery_IconSet_Image.SelectedIndex + count;
                                    if (i < ListImagesFullName.Count)
                                    {
                                        int value = (int)Math.Ceiling(RowsCount * Watch_Face_Preview_Set.Battery / 100);
                                        //value--;
                                        if (value <= 0) value = 1;
                                        if (count < value)
                                        {
                                            //src = new Bitmap(ListImagesFullName[i]);
                                            src = OpenFileStream(ListImagesFullName[i]);
                                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                            //count++;
                                            //src.Dispose();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
            #endregion

            #region Date
            Logger.WriteLine("PreviewToBitmap (Date)");
            if (checkBox_Date.Checked)
            {
                // название месяца
                if ((checkBox_MonthName.Checked) && (comboBox_MonthName_Image.SelectedIndex >= 0))
                {
                    i = comboBox_MonthName_Image.SelectedIndex + Watch_Face_Preview_Set.Date.Month - 1;
                    //src = new Bitmap(ListImagesFullName[i]);
                    src = OpenFileStream(ListImagesFullName[i]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_MonthName_X.Value,
                        (int)numericUpDown_MonthName_Y.Value, src.Width, src.Height));
                    //src.Dispose();
                }

                // месяц
                if ((checkBox_MonthAndDayM.Checked) && (comboBox_MonthAndDayM_Image.SelectedIndex >= 0))
                {
                    int x1 = (int)numericUpDown_MonthAndDayM_StartCorner_X.Value;
                    int y1 = (int)numericUpDown_MonthAndDayM_StartCorner_Y.Value;
                    int x2 = (int)numericUpDown_MonthAndDayM_EndCorner_X.Value + 1;
                    int y2 = (int)numericUpDown_MonthAndDayM_EndCorner_Y.Value + 1;
                    x2++;
                    y2++;
                    int image_index = comboBox_MonthAndDayM_Image.SelectedIndex;
                    int spacing = (int)numericUpDown_MonthAndDayM_Spacing.Value;
                    int alignment = comboBox_MonthAndDayM_Alignment.SelectedIndex;
                    int data_number = Watch_Face_Preview_Set.Date.Month;

                    if (numericUpDown_MonthAndDayM_Count.Value == 10)
                        DrawNumberDate(gPanel, x1, y1, x2, y2, image_index, spacing, alignment,
                            data_number, BBorder, checkBox_TwoDigitsMonth.Checked);
                }

                // число
                if ((checkBox_MonthAndDayD.Checked) && (comboBox_MonthAndDayD_Image.SelectedIndex >= 0))
                {
                    int x1 = (int)numericUpDown_MonthAndDayD_StartCorner_X.Value;
                    int y1 = (int)numericUpDown_MonthAndDayD_StartCorner_Y.Value;
                    int x2 = (int)numericUpDown_MonthAndDayD_EndCorner_X.Value + 1;
                    int y2 = (int)numericUpDown_MonthAndDayD_EndCorner_Y.Value + 1;
                    x2++;
                    y2++;
                    int image_index = comboBox_MonthAndDayD_Image.SelectedIndex;
                    int spacing = (int)numericUpDown_MonthAndDayD_Spacing.Value;
                    int alignment = comboBox_MonthAndDayD_Alignment.SelectedIndex;
                    int data_number = Watch_Face_Preview_Set.Date.Day;

                    if (numericUpDown_MonthAndDayD_Count.Value == 10)
                        DrawNumberDate(gPanel, x1, y1, x2, y2, image_index,spacing, alignment,
                            data_number, BBorder, checkBox_TwoDigitsDay.Checked);
                }

                // дата в одну линию
                if ((checkBox_OneLine.Checked) && (comboBox_OneLine_Image.SelectedIndex >= 0))
                {
                    int x1 = (int)numericUpDown_OneLine_StartCorner_X.Value;
                    int y1 = (int)numericUpDown_OneLine_StartCorner_Y.Value;
                    int x2 = (int)numericUpDown_OneLine_EndCorner_X.Value + 1;
                    int y2 = (int)numericUpDown_OneLine_EndCorner_Y.Value + 1;
                    x2++;
                    y2++;
                    int image_index = comboBox_OneLine_Image.SelectedIndex;
                    int spacing = (int)numericUpDown_OneLine_Spacing.Value;
                    int alignment = comboBox_OneLine_Alignment.SelectedIndex;
                    int delimeter = comboBox_OneLine_Delimiter.SelectedIndex;
                    int month_value = Watch_Face_Preview_Set.Date.Month;
                    int day_value = Watch_Face_Preview_Set.Date.Day;

                    if (numericUpDown_OneLine_Count.Value == 10)
                        DrawDateOneLine(gPanel, x1, y1, x2, y2, image_index, spacing, alignment,
                            month_value, day_value, delimeter,BBorder, 
                            checkBox_TwoDigitsMonth.Checked, checkBox_TwoDigitsDay.Checked);
                }

                // день недели
                if ((checkBox_WeekDay.Checked) && (comboBox_WeekDay_Image.SelectedIndex >= 0))
                {
                    i = comboBox_WeekDay_Image.SelectedIndex + Watch_Face_Preview_Set.Date.WeekDay - 1;
                    //src = new Bitmap(ListImagesFullName[i]);
                    src = OpenFileStream(ListImagesFullName[i]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_WeekDay_X.Value,
                        (int)numericUpDown_WeekDay_Y.Value, src.Width, src.Height));
                    //src.Dispose();
                }

                // день недели сегментами
                if (checkBox_DOW_IconSet.Checked)
                {
                    if ((comboBox_DOW_IconSet_Image.SelectedIndex >= 0) && (dataGridView_DOW_IconSet.Rows.Count > 0))
                    {
                        int x = 0;
                        int y = 0;
                        //int count = 0;
                        for (int count = 0; count < dataGridView_DOW_IconSet.Rows.Count; count++)
                        {
                            if ((dataGridView_DOW_IconSet.Rows[count].Cells[0].Value != null) &&
                                (dataGridView_DOW_IconSet.Rows[count].Cells[1].Value != null))
                            {
                                //int x = Int32.Parse(dataGridView_SPSliced.Rows[count].Cells[0].Value.ToString());
                                //int y = Int32.Parse(dataGridView_SPSliced.Rows[count].Cells[1].Value.ToString());
                                if (Int32.TryParse(dataGridView_DOW_IconSet.Rows[count].Cells[0].Value.ToString(), out x) &&
                                    Int32.TryParse(dataGridView_DOW_IconSet.Rows[count].Cells[1].Value.ToString(), out y))
                                {
                                    i = comboBox_DOW_IconSet_Image.SelectedIndex + count;
                                    if (i < ListImagesFullName.Count)
                                    {
                                        int value = Watch_Face_Preview_Set.Date.WeekDay;
                                        value--;
                                        if (count == value)
                                        {
                                            //src = new Bitmap(ListImagesFullName[i]);
                                            src = OpenFileStream(ListImagesFullName[i]);
                                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                            //count++;
                                            //src.Dispose();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // год
                if ((checkBox_Year.Checked) && (comboBox_Year_Image1.SelectedIndex >= 0))
                {
                    int x1 = (int)numericUpDown_Year_StartCorner_X.Value;
                    int y1 = (int)numericUpDown_Year_StartCorner_Y.Value;
                    int x2 = (int)numericUpDown_Year_EndCorner_X.Value + 1;
                    int y2 = (int)numericUpDown_Year_EndCorner_Y.Value + 1;

                    x2++;
                    y2++;
                    int image_index = comboBox_Year_Image1.SelectedIndex;
                    int spacing = (int)numericUpDown_Year_Spacing2.Value;
                    int alignment = comboBox_Year_Alignment2.SelectedIndex;
                    int data_number = Watch_Face_Preview_Set.Date.Year;
                    int delimiter = comboBox_Year_Delimiter.SelectedIndex;
                    if (numericUpDown_Year_Count.Value == 10)
                        DrawYear(gPanel, x1, y1, x2, y2, image_index, spacing, alignment, data_number, delimiter, BBorder);

                }
            }
            #endregion

            #region Status
            Logger.WriteLine("PreviewToBitmap (Status)");
            if (checkBox_Bluetooth.Checked)
            {
                if (Watch_Face_Preview_Set.Status.Bluetooth)
                {
                    if (comboBox_Bluetooth_On.SelectedIndex >= 0)
                    {
                        //src = new Bitmap(ListImagesFullName[comboBox_Bluetooth_On.SelectedIndex]);
                        src = OpenFileStream(ListImagesFullName[comboBox_Bluetooth_On.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Bluetooth_X.Value,
                            (int)numericUpDown_Bluetooth_Y.Value, src.Width, src.Height));
                        //src.Dispose();
                    }
                }
                else
                {
                    if (comboBox_Bluetooth_Off.SelectedIndex >= 0)
                    {
                        //src = new Bitmap(ListImagesFullName[comboBox_Bluetooth_Off.SelectedIndex]);
                        src = OpenFileStream(ListImagesFullName[comboBox_Bluetooth_Off.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Bluetooth_X.Value,
                            (int)numericUpDown_Bluetooth_Y.Value, src.Width, src.Height));
                        //src.Dispose();
                    }
                }
            }

            if (checkBox_Alarm.Checked)
            {
                if (Watch_Face_Preview_Set.Status.Alarm)
                {
                    if (comboBox_Alarm_On.SelectedIndex >= 0)
                    {
                        //src = new Bitmap(ListImagesFullName[comboBox_Alarm_On.SelectedIndex]);
                        src = OpenFileStream(ListImagesFullName[comboBox_Alarm_On.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Alarm_X.Value,
                            (int)numericUpDown_Alarm_Y.Value, src.Width, src.Height));
                        //src.Dispose();
                    }
                }
                else
                {
                    if (comboBox_Alarm_Off.SelectedIndex >= 0)
                    {
                        //src = new Bitmap(ListImagesFullName[comboBox_Alarm_Off.SelectedIndex]);
                        src = OpenFileStream(ListImagesFullName[comboBox_Alarm_Off.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Alarm_X.Value,
                            (int)numericUpDown_Alarm_Y.Value, src.Width, src.Height));
                        //src.Dispose();
                    }
                }
            }

            if (checkBox_Lock.Checked)
            {
                if (Watch_Face_Preview_Set.Status.Lock)
                {
                    if (comboBox_Lock_On.SelectedIndex >= 0)
                    {
                        //src = new Bitmap(ListImagesFullName[comboBox_Lock_On.SelectedIndex]);
                        src = OpenFileStream(ListImagesFullName[comboBox_Lock_On.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Lock_X.Value,
                            (int)numericUpDown_Lock_Y.Value, src.Width, src.Height));
                        //src.Dispose();
                    }
                }
                else
                {
                    if (comboBox_Lock_Off.SelectedIndex >= 0)
                    {
                        //src = new Bitmap(ListImagesFullName[comboBox_Lock_Off.SelectedIndex]);
                        src = OpenFileStream(ListImagesFullName[comboBox_Lock_Off.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Lock_X.Value,
                            (int)numericUpDown_Lock_Y.Value, src.Width, src.Height));
                        //src.Dispose();
                    }
                }
            }

            if (checkBox_DND.Checked)
            {
                if (Watch_Face_Preview_Set.Status.DoNotDisturb)
                {
                    if (comboBox_DND_On.SelectedIndex >= 0)
                    {
                        //src = new Bitmap(ListImagesFullName[comboBox_DND_On.SelectedIndex]);
                        src = OpenFileStream(ListImagesFullName[comboBox_DND_On.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_DND_X.Value,
                            (int)numericUpDown_DND_Y.Value, src.Width, src.Height));
                        //src.Dispose();
                    }
                }
                else
                {
                    if (comboBox_DND_Off.SelectedIndex >= 0)
                    {
                        //src = new Bitmap(ListImagesFullName[comboBox_DND_Off.SelectedIndex]);
                        src = OpenFileStream(ListImagesFullName[comboBox_DND_Off.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_DND_X.Value,
                            (int)numericUpDown_DND_Y.Value, src.Width, src.Height));
                        //src.Dispose();
                    }
                }
            }
            #endregion

            #region Weather
            Logger.WriteLine("PreviewToBitmap (Weather)");
            if (checkBox_Weather.Checked)
            {
                if ((checkBox_Weather_Icon.Checked) && (comboBox_Weather_Icon_Image.SelectedIndex >= 0))
                {
                    if (comboBox_WeatherSet_Icon.SelectedIndex >= 0)
                    {
                        i = comboBox_Weather_Icon_Image.SelectedIndex + comboBox_WeatherSet_Icon.SelectedIndex;
                        //src = new Bitmap(ListImagesFullName[i]);
                        src = OpenFileStream(ListImagesFullName[i]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Weather_Icon_X.Value,
                            (int)numericUpDown_Weather_Icon_Y.Value, src.Width, src.Height));
                        //src.Dispose();
                    }
                    else
                    {
                        if (comboBox_Weather_Icon_NDImage.SelectedIndex >= 0)
                        {
                            //src = new Bitmap(ListImagesFullName[comboBox_Weather_Icon_NDImage.SelectedIndex]);
                            src = OpenFileStream(ListImagesFullName[comboBox_Weather_Icon_NDImage.SelectedIndex]);
                            gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Weather_Icon_X.Value,
                                (int)numericUpDown_Weather_Icon_Y.Value, src.Width, src.Height));
                            //src.Dispose();
                        }
                    }
                }

                bool Show_Weather_Day_Night = true;
                if ((checkBox_Weather_Text.Checked) && (comboBox_Weather_Text_Image.SelectedIndex >= 0))
                {
                    int x1 = (int)numericUpDown_Weather_Text_StartCorner_X.Value;
                    int y1 = (int)numericUpDown_Weather_Text_StartCorner_Y.Value;
                    int x2 = (int)numericUpDown_Weather_Text_EndCorner_X.Value + 1;
                    int y2 = (int)numericUpDown_Weather_Text_EndCorner_Y.Value + 1;
                    int image_index = comboBox_Weather_Text_Image.SelectedIndex;
                    int spacing = (int)numericUpDown_Weather_Text_Spacing.Value;
                    int alignment = comboBox_Weather_Text_Alignment.SelectedIndex;
                    int data_number = (int)numericUpDown_WeatherSet_Temp.Value;
                    int minus = comboBox_Weather_Text_MinusImage.SelectedIndex;
                    int degris = comboBox_Weather_Text_DegImage.SelectedIndex;
                    int error = comboBox_Weather_Text_NDImage.SelectedIndex;
                    bool ND = !checkBox_WeatherSet_Temp.Checked;
                    DrawWeather(gPanel, x1, y1, x2, y2, image_index, spacing, alignment, data_number, minus, degris, error, ND, BBorder);
                    if(Program_Settings.DoNotShowMaxMinTemp) Show_Weather_Day_Night = false;
                }

                if ((checkBox_Weather_Day.Checked) && (comboBox_Weather_Day_Image.SelectedIndex >= 0) 
                    && Show_Weather_Day_Night)
                {
                    int x1 = (int)numericUpDown_Weather_Day_StartCorner_X.Value;
                    int y1 = (int)numericUpDown_Weather_Day_StartCorner_Y.Value;
                    int x2 = (int)numericUpDown_Weather_Day_EndCorner_X.Value + 1;
                    int y2 = (int)numericUpDown_Weather_Day_EndCorner_Y.Value + 1;
                    int image_index = comboBox_Weather_Day_Image.SelectedIndex;
                    int spacing = (int)numericUpDown_Weather_Day_Spacing.Value;
                    int alignment = comboBox_Weather_Day_Alignment.SelectedIndex;

                    int data_number = (int)numericUpDown_WeatherSet_DayTemp.Value;
                    int minus = comboBox_Weather_Text_MinusImage.SelectedIndex;
                    bool ND = !checkBox_WeatherSet_DayTemp.Checked;

                    int degris = comboBox_Weather_Text_DegImage.SelectedIndex;
                    int error = comboBox_Weather_Text_NDImage.SelectedIndex;
                    DrawWeather(gPanel, x1, y1, x2, y2, image_index, spacing, alignment, data_number,
                        minus, degris, error, ND, BBorder);

                }
                if ((checkBox_Weather_Night.Checked) && (comboBox_Weather_Night_Image.SelectedIndex >= 0)
                    && Show_Weather_Day_Night)
                {
                    int x1 = (int)numericUpDown_Weather_Night_StartCorner_X.Value;
                    int y1 = (int)numericUpDown_Weather_Night_StartCorner_Y.Value;
                    int x2 = (int)numericUpDown_Weather_Night_EndCorner_X.Value + 1;
                    int y2 = (int)numericUpDown_Weather_Night_EndCorner_Y.Value + 1;
                    int image_index = comboBox_Weather_Night_Image.SelectedIndex;
                    int spacing = (int)numericUpDown_Weather_Night_Spacing.Value;
                    int alignment = comboBox_Weather_Night_Alignment.SelectedIndex;

                    int data_number = (int)numericUpDown_WeatherSet_NightTemp.Value;
                    int minus = comboBox_Weather_Text_MinusImage.SelectedIndex;
                    bool ND = !checkBox_WeatherSet_DayTemp.Checked;

                    int degris = comboBox_Weather_Text_DegImage.SelectedIndex;
                    int error = comboBox_Weather_Text_NDImage.SelectedIndex;
                    DrawWeather(gPanel, x1, y1, x2, y2, image_index, spacing, alignment, data_number,
                        minus, degris, error, ND, BBorder);

                }
            }
            #endregion

            //gPanel.SmoothingMode = SmoothingMode.AntiAlias;

            #region ActivitySteps
            if (checkBox_Activity.Checked)
            {
                // число шагов
                if ((checkBox_ActivitySteps.Checked) && (comboBox_ActivitySteps_Image.SelectedIndex >= 0))
                {
                    int x1 = (int)numericUpDown_ActivitySteps_StartCorner_X.Value;
                    int y1 = (int)numericUpDown_ActivitySteps_StartCorner_Y.Value;
                    int x2 = (int)numericUpDown_ActivitySteps_EndCorner_X.Value;
                    int y2 = (int)numericUpDown_ActivitySteps_EndCorner_Y.Value;
                    x2++;
                    y2++;
                    int image_index = comboBox_ActivitySteps_Image.SelectedIndex;
                    int spacing = (int)numericUpDown_ActivitySteps_Spacing.Value;
                    int alignment = comboBox_ActivitySteps_Alignment.SelectedIndex;
                    int data_number = Watch_Face_Preview_Set.Activity.Steps;
                    if (numericUpDown_ActivitySteps_Count.Value == 10)
                        DrawNumber(gPanel, x1, y1, x2, y2, image_index, spacing, alignment, data_number, BBorder);
                }

                // достижение цели
                if ((checkBox_ActivityStar.Checked) && (comboBox_ActivityStar_Image.SelectedIndex >= 0))
                {
                    if (Watch_Face_Preview_Set.Activity.Steps >= Watch_Face_Preview_Set.Activity.StepsGoal)
                    {
                        //src = new Bitmap(ListImagesFullName[comboBox_ActivityStar_Image.SelectedIndex]);
                        src = OpenFileStream(ListImagesFullName[comboBox_ActivityStar_Image.SelectedIndex]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_ActivityStar_X.Value,
                            (int)numericUpDown_ActivityStar_Y.Value, src.Width, src.Height));
                        //src.Dispose();
                    }
                }
            }
            #endregion

            #region StepsProgress
            Logger.WriteLine("PreviewToBitmap (StepsProgress)");
            // прогресс шагов круговой прогресс
            if (checkBox_StepsProgress.Checked)
            {
                if (checkBox_StepsProgress_Image.Checked &&
                        comboBox_StepsProgress_Image.SelectedIndex >= 0)
                {
                    int x = (int)numericUpDown_StepsProgress_ImageX.Value;
                    int y = (int)numericUpDown_StepsProgress_ImageY.Value;
                    float width = (float)numericUpDown_StepsProgress_Width.Value;
                    int ImageIndex = comboBox_StepsProgress_Image.SelectedIndex;
                    int radius = (int)numericUpDown_StepsProgress_Radius_X.Value;
                    float StartAngle = (float)numericUpDown_StepsProgress_StartAngle.Value - 90;
                    float EndAngle = (float)(numericUpDown_StepsProgress_EndAngle.Value -
                        numericUpDown_StepsProgress_StartAngle.Value);
                    float AngleScale = (float)Watch_Face_Preview_Set.Activity.Steps / Watch_Face_Preview_Set.Activity.StepsGoal;
                    if (AngleScale > 1) AngleScale = 1;
                    EndAngle = EndAngle * AngleScale;
                    int lineCap = comboBox_StepsProgress_Flatness.SelectedIndex;
                    CircleOnBitmap(gPanel, x, y, ImageIndex, radius, width, lineCap, StartAngle, EndAngle, showProgressArea);
                }
                else
                {
                    Pen pen = new Pen(comboBox_StepsProgress_Color.BackColor,
                    (float)numericUpDown_StepsProgress_Width.Value);
                    switch (comboBox_StepsProgress_Flatness.SelectedIndex)
                    {
                        case 1:
                            pen.EndCap = LineCap.Triangle;
                            pen.StartCap = LineCap.Triangle;
                            break;
                        case 2:
                            pen.EndCap = LineCap.Flat;
                            pen.StartCap = LineCap.Flat;
                            break;
                        default:
                            pen.EndCap = LineCap.Round;
                            pen.StartCap = LineCap.Round;
                            break;
                    }

                    int x = (int)numericUpDown_StepsProgress_Center_X.Value -
                        (int)numericUpDown_StepsProgress_Radius_X.Value;
                    int y = (int)numericUpDown_StepsProgress_Center_Y.Value -
                        (int)numericUpDown_StepsProgress_Radius_Y.Value;
                    if (numericUpDown_StepsProgress_Radius_Y.Value == 0)
                    {
                        y = (int)numericUpDown_StepsProgress_Center_Y.Value -
                        (int)numericUpDown_StepsProgress_Radius_X.Value;
                    }
                    int width = (int)numericUpDown_StepsProgress_Radius_X.Value * 2;
                    //int height = (int)numericUpDown_StepsProgress_Radius_Y.Value * 2;
                    int height = width;
                    float StartAngle = (float)numericUpDown_StepsProgress_StartAngle.Value - 90;
                    float EndAngle = (float)(numericUpDown_StepsProgress_EndAngle.Value -
                        numericUpDown_StepsProgress_StartAngle.Value);
                    float AngleScale = (float)Watch_Face_Preview_Set.Activity.Steps / Watch_Face_Preview_Set.Activity.StepsGoal;
                    if (AngleScale > 1) AngleScale = 1;
                    EndAngle = EndAngle * AngleScale;
                    try
                    {
                        if ((width > 0) && (height > 0)) gPanel.DrawArc(pen, x, y, width, height, StartAngle, EndAngle);
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            // прогресс шагов стрелочный индикатор
            if ((checkBox_StProg_ClockHand.Checked) && (comboBox_StProg_ClockHand_Image.SelectedIndex >= 0))
            {
                int x1 = (int)numericUpDown_StProg_ClockHand_X.Value;
                int y1 = (int)numericUpDown_StProg_ClockHand_Y.Value;
                int offsetX = (int)numericUpDown_StProg_ClockHand_Offset_X.Value;
                int offsetY = (int)numericUpDown_StProg_ClockHand_Offset_Y.Value;
                int image_index = comboBox_StProg_ClockHand_Image.SelectedIndex;
                float startAngle = (float)(numericUpDown_StProg_ClockHand_StartAngle.Value);
                float endAngle = (float)(numericUpDown_StProg_ClockHand_EndAngle.Value);
                float angle = startAngle + Watch_Face_Preview_Set.Activity.Steps * (endAngle - startAngle) /
                    Watch_Face_Preview_Set.Activity.StepsGoal;
                if (Watch_Face_Preview_Set.Activity.Steps > Watch_Face_Preview_Set.Activity.StepsGoal) angle = endAngle;
                DrawAnalogClock(gPanel, x1, y1, offsetX, offsetY, image_index, angle);
            }

            // прогресс шагов набор иконок
            if (checkBox_SPSliced.Checked)
            {
                if ((comboBox_SPSliced_Image.SelectedIndex >= 0) && (dataGridView_SPSliced.Rows.Count > 0))
                {
                    int x = 0;
                    int y = 0;
                    //int count = 0;
                    for (int count = 0; count < dataGridView_SPSliced.Rows.Count; count++)
                    {
                        if ((dataGridView_SPSliced.Rows[count].Cells[0].Value != null) &&
                            (dataGridView_SPSliced.Rows[count].Cells[1].Value != null))
                        {
                            //int x = Int32.Parse(dataGridView_SPSliced.Rows[count].Cells[0].Value.ToString());
                            //int y = Int32.Parse(dataGridView_SPSliced.Rows[count].Cells[1].Value.ToString());
                            if (Int32.TryParse(dataGridView_SPSliced.Rows[count].Cells[0].Value.ToString(), out x) &&
                                Int32.TryParse(dataGridView_SPSliced.Rows[count].Cells[1].Value.ToString(), out y))
                            {
                                i = comboBox_SPSliced_Image.SelectedIndex + count;
                                if (i < ListImagesFullName.Count)
                                {
                                    int value = (dataGridView_SPSliced.Rows.Count - 1) * Watch_Face_Preview_Set.Activity.Steps /
                                        Watch_Face_Preview_Set.Activity.StepsGoal;
                                    if (count < value)
                                    {
                                        //src = new Bitmap(ListImagesFullName[i]);
                                        src = OpenFileStream(ListImagesFullName[i]);
                                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                        //count++;
                                        //src.Dispose();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            #region Activity
            Logger.WriteLine("PreviewToBitmap (Activity)");
            if (checkBox_Activity.Checked)
            {
                //// прогресс шагов
                //if ((checkBox_ActivitySteps.Checked) && (comboBox_ActivitySteps_Image.SelectedIndex >= 0))
                //{
                //    int x1 = (int)numericUpDown_ActivitySteps_StartCorner_X.Value;
                //    int y1 = (int)numericUpDown_ActivitySteps_StartCorner_Y.Value;
                //    int x2 = (int)numericUpDown_ActivitySteps_EndCorner_X.Value;
                //    int y2 = (int)numericUpDown_ActivitySteps_EndCorner_Y.Value;
                //    x2++;
                //    y2++;
                //    int image_index = comboBox_ActivitySteps_Image.SelectedIndex;
                //    int spacing = (int)numericUpDown_ActivitySteps_Spacing.Value;
                //    int alignment = comboBox_ActivitySteps_Alignment.SelectedIndex;
                //    int data_number = Watch_Face_Preview_Set.Activity.Steps;
                //    if (numericUpDown_ActivitySteps_Count.Value == 10)
                //        DrawNumber(gPanel, x1, y1, x2, y2, image_index, spacing, alignment, data_number, BBorder);
                //}

                // пройденный путь
                if ((checkBox_ActivityDistance.Checked) && (comboBox_ActivityDistance_Image.SelectedIndex >= 0))
                {
                    int x1 = (int)numericUpDown_ActivityDistance_StartCorner_X.Value;
                    int y1 = (int)numericUpDown_ActivityDistance_StartCorner_Y.Value;
                    int x2 = (int)numericUpDown_ActivityDistance_EndCorner_X.Value;
                    int y2 = (int)numericUpDown_ActivityDistance_EndCorner_Y.Value;
                    x2++;
                    y2++;
                    int image_index = comboBox_ActivityDistance_Image.SelectedIndex;
                    int spacing = (int)numericUpDown_ActivityDistance_Spacing.Value;
                    int alignment = comboBox_ActivityDistance_Alignment.SelectedIndex;
                    double data_number = Watch_Face_Preview_Set.Activity.Distance / 1000f;
                    int suffix = comboBox_ActivityDistance_Suffix_km.SelectedIndex;
                    if (Program_Settings.ShowMiles && comboBox_ActivityDistance_Suffix_ml.SelectedIndex >= 0)
                        suffix = comboBox_ActivityDistance_Suffix_ml.SelectedIndex;
                    int dec = comboBox_ActivityDistance_Decimal.SelectedIndex;
                    if (numericUpDown_ActivityDistance_Count.Value == 10)
                        DrawNumber(gPanel, x1, y1, x2, y2, image_index, spacing, alignment, data_number, 
                            suffix, dec, 2, BBorder);
                }

                // пульс надпись
                if ((checkBox_ActivityPuls.Checked) && (comboBox_ActivityPuls_Image.SelectedIndex >= 0))
                {
                    int x1 = (int)numericUpDown_ActivityPuls_StartCorner_X.Value;
                    int y1 = (int)numericUpDown_ActivityPuls_StartCorner_Y.Value;
                    int x2 = (int)numericUpDown_ActivityPuls_EndCorner_X.Value;
                    int y2 = (int)numericUpDown_ActivityPuls_EndCorner_Y.Value;
                    x2++;
                    y2++;
                    int image_index = comboBox_ActivityPuls_Image.SelectedIndex;
                    int spacing = (int)numericUpDown_ActivityPuls_Spacing.Value;
                    int alignment = comboBox_ActivityPuls_Alignment.SelectedIndex;
                    int data_number = Watch_Face_Preview_Set.Activity.HeartRate;
                    if (numericUpDown_ActivityPuls_Count.Value == 10)
                        DrawNumber(gPanel, x1, y1, x2, y2, image_index, spacing, alignment,data_number, BBorder);
                }

                // пульс набор иконок
                if (checkBox_ActivityPuls_IconSet.Checked)
                {
                    if ((comboBox_ActivityPuls_IconSet_Image.SelectedIndex >= 0) && (dataGridView_ActivityPuls_IconSet.Rows.Count > 0))
                    {
                        int x = 0;
                        int y = 0;
                        int count = dataGridView_ActivityPuls_IconSet.Rows.Count - 1;
                        int value = (int)((count - 1) * (Watch_Face_Preview_Set.Activity.HeartRate - 73) / (90f));
                        //value++;
                        if (value > count - 1) value = count - 1;
                        if (value <0) value = 0;
                        if ((dataGridView_ActivityPuls_IconSet.Rows[value].Cells[0].Value != null) &&
                                (dataGridView_ActivityPuls_IconSet.Rows[value].Cells[1].Value != null))
                        {
                            if (Int32.TryParse(dataGridView_ActivityPuls_IconSet.Rows[value].Cells[0].Value.ToString(), out x) &&
                                Int32.TryParse(dataGridView_ActivityPuls_IconSet.Rows[value].Cells[1].Value.ToString(), out y))
                            {
                                i = comboBox_ActivityPuls_IconSet_Image.SelectedIndex + value;
                                if (i < ListImagesFullName.Count)
                                {
                                    //src = new Bitmap(ListImagesFullName[i]);
                                    src = OpenFileStream(ListImagesFullName[i]);
                                    gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                    //src.Dispose();
                                }
                            }
                        }
                    }
                }

                // пульс шкала 
                if (checkBox_ActivityPulsScale.Checked)
                {
                    if (checkBox_ActivityPulsScale_Image.Checked &&
                        comboBox_ActivityPulsScale_Image.SelectedIndex >= 0)
                    {
                        int x = (int)numericUpDown_ActivityPulsScale_ImageX.Value;
                        int y = (int)numericUpDown_ActivityPulsScale_ImageY.Value;
                        float width = (float)numericUpDown_ActivityPulsScale_Width.Value;
                        int ImageIndex = comboBox_ActivityPulsScale_Image.SelectedIndex;
                        int radius = (int)numericUpDown_ActivityPulsScale_Radius_X.Value;
                        float StartAngle = (float)numericUpDown_ActivityPulsScale_StartAngle.Value - 90;
                        float EndAngle = (float)(numericUpDown_ActivityPulsScale_EndAngle.Value -
                            numericUpDown_ActivityPulsScale_StartAngle.Value);
                        float AngleScale = (float)Watch_Face_Preview_Set.Activity.HeartRate / 180f;
                        if (AngleScale > 1) AngleScale = 1;
                        EndAngle = EndAngle * AngleScale;
                        int lineCap = comboBox_ActivityPulsScale_Flatness.SelectedIndex;
                        CircleOnBitmap(gPanel, x, y, ImageIndex, radius, width, lineCap, StartAngle, EndAngle, showProgressArea);
                    }
                    else
                    {
                        gPanel.SmoothingMode = SmoothingMode.AntiAlias;
                        Pen pen = new Pen(comboBox_ActivityPulsScale_Color.BackColor,
                            (float)numericUpDown_ActivityPulsScale_Width.Value);

                        switch (comboBox_ActivityPulsScale_Flatness.SelectedIndex)
                        {
                            case 1:
                                pen.EndCap = LineCap.Triangle;
                                pen.StartCap = LineCap.Triangle;
                                break;
                            case 2:
                                pen.EndCap = LineCap.Flat;
                                pen.StartCap = LineCap.Flat;
                                break;
                            default:
                                pen.EndCap = LineCap.Round;
                                pen.StartCap = LineCap.Round;
                                break;
                        }

                        int x = (int)numericUpDown_ActivityPulsScale_Center_X.Value -
                            (int)numericUpDown_ActivityPulsScale_Radius_X.Value;
                        int y = (int)numericUpDown_ActivityPulsScale_Center_Y.Value -
                            (int)numericUpDown_ActivityPulsScale_Radius_Y.Value;
                        if (numericUpDown_ActivityPulsScale_Radius_Y.Value == 0)
                        {
                            y = (int)numericUpDown_ActivityPulsScale_Center_Y.Value -
                            (int)numericUpDown_ActivityPulsScale_Radius_X.Value;
                        }
                        int width = (int)numericUpDown_ActivityPulsScale_Radius_X.Value * 2;
                        //int height = (int)numericUpDown_Battery_Scale_Radius_Y.Value * 2;
                        int height = width;
                        float StartAngle = (float)numericUpDown_ActivityPulsScale_StartAngle.Value - 90;
                        float EndAngle = (float)(numericUpDown_ActivityPulsScale_EndAngle.Value -
                            numericUpDown_ActivityPulsScale_StartAngle.Value);
                        float AngleScale = (float)Watch_Face_Preview_Set.Activity.HeartRate / 180f;
                        if (AngleScale > 1) AngleScale = 1;
                        EndAngle = EndAngle * AngleScale;
                        try
                        {
                            if ((width > 0) && (height > 0)) gPanel.DrawArc(pen, x, y, width, height, StartAngle, EndAngle);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }

                // пульс стрелочный индикатор
                if ((checkBox_Pulse_ClockHand.Checked) && (comboBox_Pulse_ClockHand_Image.SelectedIndex >= 0))
                {
                    int x1 = (int)numericUpDown_Pulse_ClockHand_X.Value;
                    int y1 = (int)numericUpDown_Pulse_ClockHand_Y.Value;
                    int offsetX = (int)numericUpDown_Pulse_ClockHand_Offset_X.Value;
                    int offsetY = (int)numericUpDown_Pulse_ClockHand_Offset_Y.Value;
                    int image_index = comboBox_Pulse_ClockHand_Image.SelectedIndex;
                    float startAngle = (float)(numericUpDown_Pulse_ClockHand_StartAngle.Value);
                    float endAngle = (float)(numericUpDown_Pulse_ClockHand_EndAngle.Value);
                    float angle = startAngle + Watch_Face_Preview_Set.Activity.HeartRate * (endAngle - startAngle) / 180;
                    if (startAngle < endAngle && angle > endAngle) angle = endAngle;
                    if (startAngle > endAngle && angle < endAngle) angle = endAngle;
                    DrawAnalogClock(gPanel, x1, y1, offsetX, offsetY, image_index, angle);
                }

                // калории надпись
                if ((checkBox_ActivityCalories.Checked) && (comboBox_ActivityCalories_Image.SelectedIndex >= 0))
                {
                    int x1 = (int)numericUpDown_ActivityCalories_StartCorner_X.Value;
                    int y1 = (int)numericUpDown_ActivityCalories_StartCorner_Y.Value;
                    int x2 = (int)numericUpDown_ActivityCalories_EndCorner_X.Value;
                    int y2 = (int)numericUpDown_ActivityCalories_EndCorner_Y.Value;
                    x2++;
                    y2++;
                    int image_index = comboBox_ActivityCalories_Image.SelectedIndex;
                    int spacing = (int)numericUpDown_ActivityCalories_Spacing.Value;
                    int alignment = comboBox_ActivityCalories_Alignment.SelectedIndex;
                    int data_number = Watch_Face_Preview_Set.Activity.Calories;
                    if (numericUpDown_ActivityCalories_Count.Value == 10)
                        DrawNumber(gPanel, x1, y1, x2, y2, image_index, spacing, alignment, data_number, BBorder);
                }

                // шкала калорий 
                if (checkBox_ActivityCaloriesScale.Checked)
                {
                    if (checkBox_ActivityCaloriesScale_Image.Checked && 
                        comboBox_ActivityCaloriesScale_Image.SelectedIndex>=0)
                    {
                        int x = (int)numericUpDown_ActivityCaloriesScale_ImageX.Value;
                        int y = (int)numericUpDown_ActivityCaloriesScale_ImageY.Value;
                        float width = (float)numericUpDown_ActivityCaloriesScale_Width.Value;
                        int ImageIndex = comboBox_ActivityCaloriesScale_Image.SelectedIndex;
                        int radius = (int)numericUpDown_ActivityCaloriesScale_Radius_X.Value;
                        float StartAngle = (float)numericUpDown_ActivityCaloriesScale_StartAngle.Value - 90;
                        float EndAngle = (float)(numericUpDown_ActivityCaloriesScale_EndAngle.Value -
                            numericUpDown_ActivityCaloriesScale_StartAngle.Value);
                        float AngleScale = (float)(17f * Watch_Face_Preview_Set.Activity.Calories /
                            Watch_Face_Preview_Set.Activity.StepsGoal);
                        if (AngleScale > 1) AngleScale = 1;
                        EndAngle = EndAngle * AngleScale;
                        int lineCap = comboBox_ActivityCaloriesScale_Flatness.SelectedIndex;
                        CircleOnBitmap(gPanel, x, y, ImageIndex, radius, width, lineCap, StartAngle, EndAngle, showProgressArea);
                    }
                    else
                    {
                        gPanel.SmoothingMode = SmoothingMode.AntiAlias;
                        Pen pen = new Pen(comboBox_ActivityCaloriesScale_Color.BackColor,
                            (float)numericUpDown_ActivityCaloriesScale_Width.Value);

                        switch (comboBox_ActivityCaloriesScale_Flatness.SelectedIndex)
                        {
                            case 1:
                                pen.EndCap = LineCap.Triangle;
                                pen.StartCap = LineCap.Triangle;
                                break;
                            case 2:
                                pen.EndCap = LineCap.Flat;
                                pen.StartCap = LineCap.Flat;
                                break;
                            default:
                                pen.EndCap = LineCap.Round;
                                pen.StartCap = LineCap.Round;
                                break;
                        }

                        int x = (int)numericUpDown_ActivityCaloriesScale_Center_X.Value -
                            (int)numericUpDown_ActivityCaloriesScale_Radius_X.Value;
                        int y = (int)numericUpDown_ActivityCaloriesScale_Center_Y.Value -
                            (int)numericUpDown_ActivityCaloriesScale_Radius_Y.Value;
                        if (numericUpDown_ActivityCaloriesScale_Radius_Y.Value == 0)
                        {
                            y = (int)numericUpDown_ActivityCaloriesScale_Center_Y.Value -
                            (int)numericUpDown_ActivityCaloriesScale_Radius_X.Value;
                        }
                        int width = (int)numericUpDown_ActivityCaloriesScale_Radius_X.Value * 2;
                        //int height = (int)numericUpDown_Battery_Scale_Radius_Y.Value * 2;
                        int height = width;
                        float StartAngle = (float)numericUpDown_ActivityCaloriesScale_StartAngle.Value - 90;
                        float EndAngle = (float)(numericUpDown_ActivityCaloriesScale_EndAngle.Value -
                            numericUpDown_ActivityCaloriesScale_StartAngle.Value);
                        float AngleScale = (float)(17f * Watch_Face_Preview_Set.Activity.Calories /
                            Watch_Face_Preview_Set.Activity.StepsGoal);
                        if (AngleScale > 1) AngleScale = 1;
                        EndAngle = EndAngle * AngleScale;
                        try
                        {
                            if ((width > 0) && (height > 0)) gPanel.DrawArc(pen, x, y, width, height, StartAngle, EndAngle);
                        }
                        catch (Exception)
                        {

                        } 
                    }
                }

                //  калории стрелочный индикатор
                if ((checkBox_Calories_ClockHand.Checked) && (comboBox_Calories_ClockHand_Image.SelectedIndex >= 0))
                {
                    int x1 = (int)numericUpDown_Calories_ClockHand_X.Value;
                    int y1 = (int)numericUpDown_Calories_ClockHand_Y.Value;
                    int offsetX = (int)numericUpDown_Calories_ClockHand_Offset_X.Value;
                    int offsetY = (int)numericUpDown_Calories_ClockHand_Offset_Y.Value;
                    int image_index = comboBox_Calories_ClockHand_Image.SelectedIndex;
                    float startAngle = (float)(numericUpDown_Calories_ClockHand_StartAngle.Value);
                    float endAngle = (float)(numericUpDown_Calories_ClockHand_EndAngle.Value);
                    float angle = startAngle + 17f * Watch_Face_Preview_Set.Activity.Calories * (endAngle - startAngle) /
                        Watch_Face_Preview_Set.Activity.StepsGoal;
                    if (startAngle < endAngle && angle > endAngle) angle = endAngle;
                    if (startAngle > endAngle && angle < endAngle) angle = endAngle;
                    DrawAnalogClock(gPanel, x1, y1, offsetX, offsetY, image_index, angle);
                }

                //// достижение цели
                //if ((checkBox_ActivityStar.Checked) && (comboBox_ActivityStar_Image.SelectedIndex >= 0))
                //{
                //    if (Watch_Face_Preview_Set.Activity.Steps >= Watch_Face_Preview_Set.Activity.StepsGoal)
                //    {
                //        //src = new Bitmap(ListImagesFullName[comboBox_ActivityStar_Image.SelectedIndex]);
                //        src = OpenFileStream(ListImagesFullName[comboBox_ActivityStar_Image.SelectedIndex]);
                //        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_ActivityStar_X.Value,
                //            (int)numericUpDown_ActivityStar_Y.Value, src.Width, src.Height));
                //        //src.Dispose();
                //    }
                //}

                // цель шагов
                if ((checkBox_ActivityStepsGoal.Checked) && (comboBox_ActivityStepsGoal_Image.SelectedIndex >= 0))
                {
                    int x1 = (int)numericUpDown_ActivityStepsGoal_StartCorner_X.Value;
                    int y1 = (int)numericUpDown_ActivityStepsGoal_StartCorner_Y.Value;
                    int x2 = (int)numericUpDown_ActivityStepsGoal_EndCorner_X.Value;
                    int y2 = (int)numericUpDown_ActivityStepsGoal_EndCorner_Y.Value;
                    x2++;
                    y2++;
                    int image_index = comboBox_ActivityStepsGoal_Image.SelectedIndex;
                    int spacing = (int)numericUpDown_ActivityStepsGoal_Spacing.Value;
                    int alignment = comboBox_ActivityStepsGoal_Alignment.SelectedIndex;
                    int data_number = Watch_Face_Preview_Set.Activity.StepsGoal;
                    if (numericUpDown_ActivitySteps_Count.Value == 10)
                        DrawNumber(gPanel, x1, y1, x2, y2, image_index, spacing, alignment, data_number, BBorder);
                }
            }
            #endregion

            #region Animation
            Logger.WriteLine("PreviewToBitmap (Animation)");
            if (link == 1) goto DrawEnd;
            if (showAnimation)
            {
                if (checkBox_Animation.Checked)
                {
                    // анимация (перемещение между координатами)
                    if (checkBox_MotiomAnimation.Checked)
                    {
                        int AnimationIndex = 0;
                        foreach (DataGridViewRow row in dataGridView_MotiomAnimation.Rows)
                        {
                            if (++AnimationIndex > 4) break;
                            int StartCoordinates_X = 0;
                            int StartCoordinates_Y = 0;
                            int EndCoordinates_X = 0;
                            int EndCoordinates_Y = 0;
                            int ImageIndex = 0;
                            if (row.Cells[1].Value != null && row.Cells[2].Value != null && row.Cells[3].Value != null &&
                                row.Cells[4].Value != null && row.Cells[5].Value != null && row.Cells[6].Value != null)
                            {
                                if (Int32.TryParse(row.Cells[1].Value.ToString(), out StartCoordinates_X) &&
                                    Int32.TryParse(row.Cells[2].Value.ToString(), out StartCoordinates_Y) &&
                                    Int32.TryParse(row.Cells[3].Value.ToString(), out EndCoordinates_X) &&
                                    Int32.TryParse(row.Cells[4].Value.ToString(), out EndCoordinates_Y) &&
                                    Int32.TryParse(row.Cells[5].Value.ToString(), out ImageIndex))
                                {
                                    int Coordinates_X = StartCoordinates_X;
                                    int Coordinates_Y = StartCoordinates_Y;
                                    if (radioButton_MotiomAnimation_EndCoordinates.Checked)
                                    {
                                        Coordinates_X = EndCoordinates_X;
                                        Coordinates_Y = EndCoordinates_Y;
                                    }

                                    i = ImageIndex;
                                    if (i < ListImagesFullName.Count)
                                    {
                                        //src = new Bitmap(ListImagesFullName[i]);
                                        src = OpenFileStream(ListImagesFullName[i]);
                                        gPanel.DrawImage(src, new Rectangle(Coordinates_X, Coordinates_Y, src.Width, src.Height));
                                        //src.Dispose();
                                    }
                                }
                            }
                        }
                    }

                    // покадровая анимация
                    if ((checkBox_StaticAnimation.Checked) && (comboBox_StaticAnimation_Image.SelectedIndex >= 0))
                    {
                        i = comboBox_StaticAnimation_Image.SelectedIndex;
                        if (i < ListImagesFullName.Count)
                        {
                            //src = new Bitmap(ListImagesFullName[i]);
                            src = OpenFileStream(ListImagesFullName[i]);
                            gPanel.DrawImage(src, new Rectangle((int)numericUpDown_StaticAnimation_X.Value,
                                (int)numericUpDown_StaticAnimation_Y.Value, src.Width, src.Height));
                            //src.Dispose();
                        }
                    }

                }
            }
            AnimationEnd:
            #endregion

            #region Time
            Logger.WriteLine("PreviewToBitmap (Time)");
            if (checkBox_Time.Checked)
            {
                if (checkBox_AmPm.Checked && Program_Settings.ShowIn12hourFormat)
                {
                    if (checkBox_Hours.Checked)
                    {
                        if (comboBox_Hours_Tens_Image.SelectedIndex >= 0)
                        {
                            i = comboBox_Hours_Tens_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.TimePm.Hours.Tens;
                            if (i < ListImagesFullName.Count)
                            {
                                //src = new Bitmap(ListImagesFullName[i]);
                                src = OpenFileStream(ListImagesFullName[i]);
                                gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Hours_Tens_X.Value,
                                    (int)numericUpDown_Hours_Tens_Y.Value, src.Width, src.Height));
                                //src.Dispose();
                            }
                        }
                        if (comboBox_Hours_Ones_Image.SelectedIndex >= 0)
                        {
                            i = comboBox_Hours_Ones_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.TimePm.Hours.Ones;
                            if (i < ListImagesFullName.Count)
                            {
                                //src = new Bitmap(ListImagesFullName[i]);
                                src = OpenFileStream(ListImagesFullName[i]);
                                gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Hours_Ones_X.Value,
                                    (int)numericUpDown_Hours_Ones_Y.Value, src.Width, src.Height));
                                //src.Dispose();
                            }
                        }
                    }

                    if (checkBox_Minutes.Checked)
                    {
                        if (comboBox_Min_Tens_Image.SelectedIndex >= 0)
                        {
                            i = comboBox_Min_Tens_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.TimePm.Minutes.Tens;
                            if (i < ListImagesFullName.Count)
                            {
                                //src = new Bitmap(ListImagesFullName[i]);
                                src = OpenFileStream(ListImagesFullName[i]);
                                gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Min_Tens_X.Value,
                                    (int)numericUpDown_Min_Tens_Y.Value, src.Width, src.Height));
                                //src.Dispose();
                            }
                        }
                        if (comboBox_Min_Ones_Image.SelectedIndex >= 0)
                        {
                            i = comboBox_Min_Ones_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.TimePm.Minutes.Ones;
                            if (i < ListImagesFullName.Count)
                            {
                                //src = new Bitmap(ListImagesFullName[i]);
                                src = OpenFileStream(ListImagesFullName[i]);
                                gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Min_Ones_X.Value,
                                    (int)numericUpDown_Min_Ones_Y.Value, src.Width, src.Height));
                                //src.Dispose();
                            }
                        }
                    }

                    if (checkBox_Seconds.Checked)
                    {
                        if (comboBox_Sec_Tens_Image.SelectedIndex >= 0)
                        {
                            i = comboBox_Sec_Tens_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.TimePm.Seconds.Tens;
                            if (i < ListImagesFullName.Count)
                            {
                                //src = new Bitmap(ListImagesFullName[i]);
                                src = OpenFileStream(ListImagesFullName[i]);
                                gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Sec_Tens_X.Value,
                                    (int)numericUpDown_Sec_Tens_Y.Value, src.Width, src.Height));
                                //src.Dispose();
                            }
                        }
                        if (comboBox_Sec_Ones_Image.SelectedIndex >= 0)
                        {
                            i = comboBox_Sec_Ones_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.TimePm.Seconds.Ones;
                            if (i < ListImagesFullName.Count)
                            {
                                //src = new Bitmap(ListImagesFullName[i]);
                                src = OpenFileStream(ListImagesFullName[i]);
                                gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Sec_Ones_X.Value,
                                    (int)numericUpDown_Sec_Ones_Y.Value, src.Width, src.Height));
                                //src.Dispose();
                            }
                        }
                    }

                    if (Watch_Face_Preview_TwoDigits.TimePm.Pm)
                    {
                        if (comboBox_Image_Pm.SelectedIndex >= 0)
                        {
                            i = comboBox_Image_Pm.SelectedIndex;
                            //src = new Bitmap(ListImagesFullName[i]);
                            src = OpenFileStream(ListImagesFullName[i]);
                            gPanel.DrawImage(src, new Rectangle((int)numericUpDown_AmPm_X.Value,
                                (int)numericUpDown_AmPm_Y.Value, src.Width, src.Height));
                            //src.Dispose();
                        }
                    }
                    else
                    {
                        if (comboBox_Image_Am.SelectedIndex >= 0)
                        {
                            i = comboBox_Image_Am.SelectedIndex;
                            //src = new Bitmap(ListImagesFullName[i]);
                            src = OpenFileStream(ListImagesFullName[i]);
                            gPanel.DrawImage(src, new Rectangle((int)numericUpDown_AmPm_X.Value,
                                (int)numericUpDown_AmPm_Y.Value, src.Width, src.Height));
                            //src.Dispose();
                        }
                    }

                    if (checkBox_Delimiter.Checked)
                    {
                        if (comboBox_Delimiter_Image.SelectedIndex >= 0)
                        {
                            i = comboBox_Delimiter_Image.SelectedIndex;
                            //src = new Bitmap(ListImagesFullName[i]);
                            src = OpenFileStream(ListImagesFullName[i]);
                            gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Delimiter_X.Value,
                                (int)numericUpDown_Delimiter_Y.Value, src.Width, src.Height));
                            //src.Dispose();
                        }
                    }
                }
                else
                {
                    if (checkBox_Hours.Checked)
                    {
                        if (comboBox_Hours_Tens_Image.SelectedIndex >= 0)
                        {
                            i = comboBox_Hours_Tens_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.Time.Hours.Tens;
                            if (i < ListImagesFullName.Count)
                            {
                                //src = new Bitmap(ListImagesFullName[i]);
                                src = OpenFileStream(ListImagesFullName[i]);
                                gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Hours_Tens_X.Value,
                                    (int)numericUpDown_Hours_Tens_Y.Value, src.Width, src.Height));
                                //src.Dispose();
                            }
                        }
                        if (comboBox_Hours_Ones_Image.SelectedIndex >= 0)
                        {
                            i = comboBox_Hours_Ones_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.Time.Hours.Ones;
                            if (i < ListImagesFullName.Count)
                            {
                                //src = new Bitmap(ListImagesFullName[i]);
                                src = OpenFileStream(ListImagesFullName[i]);
                                gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Hours_Ones_X.Value,
                                    (int)numericUpDown_Hours_Ones_Y.Value, src.Width, src.Height));
                                //src.Dispose();
                            }
                        }
                    }

                    if (checkBox_Minutes.Checked)
                    {
                        if (comboBox_Min_Tens_Image.SelectedIndex >= 0)
                        {
                            i = comboBox_Min_Tens_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.Time.Minutes.Tens;
                            if (i < ListImagesFullName.Count)
                            {
                                //src = new Bitmap(ListImagesFullName[i]);
                                src = OpenFileStream(ListImagesFullName[i]);
                                gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Min_Tens_X.Value,
                                    (int)numericUpDown_Min_Tens_Y.Value, src.Width, src.Height));
                                //src.Dispose();
                            }
                        }
                        if (comboBox_Min_Ones_Image.SelectedIndex >= 0)
                        {
                            i = comboBox_Min_Ones_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.Time.Minutes.Ones;
                            if (i < ListImagesFullName.Count)
                            {
                                //src = new Bitmap(ListImagesFullName[i]);
                                src = OpenFileStream(ListImagesFullName[i]);
                                gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Min_Ones_X.Value,
                                    (int)numericUpDown_Min_Ones_Y.Value, src.Width, src.Height));
                                //src.Dispose();
                            }
                        }
                    }

                    if (checkBox_Seconds.Checked)
                    {
                        if (comboBox_Sec_Tens_Image.SelectedIndex >= 0)
                        {
                            i = comboBox_Sec_Tens_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.Time.Seconds.Tens;
                            if (i < ListImagesFullName.Count)
                            {
                                //src = new Bitmap(ListImagesFullName[i]);
                                src = OpenFileStream(ListImagesFullName[i]);
                                gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Sec_Tens_X.Value,
                                    (int)numericUpDown_Sec_Tens_Y.Value, src.Width, src.Height));
                                //src.Dispose();
                            }
                        }
                        if (comboBox_Sec_Ones_Image.SelectedIndex >= 0)
                        {
                            i = comboBox_Sec_Ones_Image.SelectedIndex + Watch_Face_Preview_TwoDigits.Time.Seconds.Ones;
                            if (i < ListImagesFullName.Count)
                            {
                                //src = new Bitmap(ListImagesFullName[i]);
                                src = OpenFileStream(ListImagesFullName[i]);
                                gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Sec_Ones_X.Value,
                                    (int)numericUpDown_Sec_Ones_Y.Value, src.Width, src.Height));
                                //src.Dispose();
                            }
                        }
                    }

                    if (checkBox_Delimiter.Checked)
                    {
                        if (comboBox_Delimiter_Image.SelectedIndex >= 0)
                        {
                            i = comboBox_Delimiter_Image.SelectedIndex;
                            //src = new Bitmap(ListImagesFullName[i]);
                            src = OpenFileStream(ListImagesFullName[i]);
                            gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Delimiter_X.Value,
                                (int)numericUpDown_Delimiter_Y.Value, src.Width, src.Height));
                            //src.Dispose();
                        }
                    }
                }
            }
            #endregion

            #region DaysProgress
            Logger.WriteLine("PreviewToBitmap (DaysProgress)");
            // прогресс числа стрелкой
            if ((checkBox_ADDay_ClockHand.Checked) && (comboBox_ADDay_ClockHand_Image.SelectedIndex >= 0))
            {
                int x1 = (int)numericUpDown_ADDay_ClockHand_X.Value;
                int y1 = (int)numericUpDown_ADDay_ClockHand_Y.Value;
                int offsetX = (int)numericUpDown_ADDay_ClockHand_Offset_X.Value;
                int offsetY = (int)numericUpDown_ADDay_ClockHand_Offset_Y.Value;
                int image_index = comboBox_ADDay_ClockHand_Image.SelectedIndex;
                float startAngle = (float)(numericUpDown_ADDay_ClockHand_StartAngle.Value);
                float endAngle = (float)(numericUpDown_ADDay_ClockHand_EndAngle.Value);
                int Day = Watch_Face_Preview_Set.Date.Day;
                Day--;
                float angle = startAngle + Day * (endAngle - startAngle) / 30;
                DrawAnalogClock(gPanel, x1, y1, offsetX, offsetY, image_index, angle);
            }

            // прогресс дней недели стрелкой
            if ((checkBox_ADWeekDay_ClockHand.Checked) && (comboBox_ADWeekDay_ClockHand_Image.SelectedIndex >= 0))
            {
                int x1 = (int)numericUpDown_ADWeekDay_ClockHand_X.Value;
                int y1 = (int)numericUpDown_ADWeekDay_ClockHand_Y.Value;
                int offsetX = (int)numericUpDown_ADWeekDay_ClockHand_Offset_X.Value;
                int offsetY = (int)numericUpDown_ADWeekDay_ClockHand_Offset_Y.Value;
                int image_index = comboBox_ADWeekDay_ClockHand_Image.SelectedIndex;
                float startAngle = (float)(numericUpDown_ADWeekDay_ClockHand_StartAngle.Value);
                float endAngle = (float)(numericUpDown_ADWeekDay_ClockHand_EndAngle.Value);
                int WeekDay = Watch_Face_Preview_Set.Date.WeekDay;
                WeekDay--;
                if (WeekDay < 0) WeekDay = 6;
                float angle = startAngle + WeekDay * (endAngle - startAngle) / 6;
                DrawAnalogClock(gPanel, x1, y1, offsetX, offsetY, image_index, angle);
            }

            // прогресс месяца стрелкой
            if ((checkBox_ADMonth_ClockHand.Checked) && (comboBox_ADMonth_ClockHand_Image.SelectedIndex >= 0))
            {
                int x1 = (int)numericUpDown_ADMonth_ClockHand_X.Value;
                int y1 = (int)numericUpDown_ADMonth_ClockHand_Y.Value;
                int offsetX = (int)numericUpDown_ADMonth_ClockHand_Offset_X.Value;
                int offsetY = (int)numericUpDown_ADMonth_ClockHand_Offset_Y.Value;
                int image_index = comboBox_ADMonth_ClockHand_Image.SelectedIndex;
                float startAngle = (float)(numericUpDown_ADMonth_ClockHand_StartAngle.Value);
                float endAngle = (float)(numericUpDown_ADMonth_ClockHand_EndAngle.Value);
                int Month = Watch_Face_Preview_Set.Date.Month;
                Month--;
                float angle = startAngle + Month * (endAngle - startAngle) / 11;
                DrawAnalogClock(gPanel, x1, y1, offsetX, offsetY, image_index, angle);
            }
            #endregion

            #region AnalogDialFace
            Logger.WriteLine("PreviewToBitmap (MonthClockHand)");
            if (checkBox_AnalogClock.Checked)
            {
                bool AnalogClockOffSet = false;
                if ((numericUpDown_AnalogClock_Hour_Offset_X.Value != 0) ||
                    (numericUpDown_AnalogClock_Hour_Offset_Y.Value != 0)) AnalogClockOffSet = true;
                if ((numericUpDown_AnalogClock_Min_Offset_X.Value != 0) ||
                    (numericUpDown_AnalogClock_Min_Offset_Y.Value != 0)) AnalogClockOffSet = true;
                if ((numericUpDown_AnalogClock_Sec_Offset_X.Value != 0) ||
                    (numericUpDown_AnalogClock_Sec_Offset_Y.Value != 0)) AnalogClockOffSet = true;

                if (AnalogClockOffSet)
                {
                    int offsetX_Hour = (int)numericUpDown_AnalogClock_Hour_Offset_X.Value;
                    int offsetY_Hour = (int)numericUpDown_AnalogClock_Hour_Offset_Y.Value;
                    int offsetX_Min = (int)numericUpDown_AnalogClock_Min_Offset_X.Value;
                    int offsetY_Min = (int)numericUpDown_AnalogClock_Min_Offset_Y.Value;


                    if ((offsetX_Hour != 0 || offsetY_Hour != 0) && ((offsetX_Min != 0 || offsetY_Min != 0))) AnalogClockOffSet = false;
                }

                if (AnalogClockOffSet)
                {
                    // секунды
                    if ((checkBox_AnalogClock_Sec.Checked) && (comboBox_AnalogClock_Sec_Image.SelectedIndex >= 0))
                    {
                        int x1 = (int)numericUpDown_AnalogClock_Sec_X.Value;
                        int y1 = (int)numericUpDown_AnalogClock_Sec_Y.Value;
                        int offsetX = (int)numericUpDown_AnalogClock_Sec_Offset_X.Value;
                        int offsetY = (int)numericUpDown_AnalogClock_Sec_Offset_Y.Value;
                        int image_index = comboBox_AnalogClock_Sec_Image.SelectedIndex;
                        //int hour = Watch_Face_Preview_Set.TimeW.Hours;
                        //int min = Watch_Face_Preview_Set.TimeW.Minutes;
                        int sec = Watch_Face_Preview_Set.Time.Seconds;
                        //if (hour >= 12) hour = hour - 12;
                        float angle = 360 * sec / 60;
                        DrawAnalogClock(gPanel, x1, y1, offsetX, offsetY, image_index, angle);
                    }
                }

                // часы
                if ((checkBox_AnalogClock_Hour.Checked) && (comboBox_AnalogClock_Hour_Image.SelectedIndex >= 0))
                {
                    int x1 = (int)numericUpDown_AnalogClock_Hour_X.Value;
                    int y1 = (int)numericUpDown_AnalogClock_Hour_Y.Value;
                    int offsetX = (int)numericUpDown_AnalogClock_Hour_Offset_X.Value;
                    int offsetY = (int)numericUpDown_AnalogClock_Hour_Offset_Y.Value;
                    int image_index = comboBox_AnalogClock_Hour_Image.SelectedIndex;
                    int hour = Watch_Face_Preview_Set.Time.Hours;
                    int min = Watch_Face_Preview_Set.Time.Minutes;
                    //int sec = Watch_Face_Preview_Set.TimeW.Seconds;
                    if (hour >= 12) hour = hour - 12;
                    float angle = 360 * hour / 12 + 360 * min / (60 * 12);
                    DrawAnalogClock(gPanel, x1, y1, offsetX, offsetY, image_index, angle);
                }
                if ((checkBox_HourCenterImage.Checked) && (comboBox_HourCenterImage_Image.SelectedIndex >= 0))
                {
                    //src = new Bitmap(ListImagesFullName[comboBox_HourCenterImage_Image.SelectedIndex]);
                    src = OpenFileStream(ListImagesFullName[comboBox_HourCenterImage_Image.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_HourCenterImage_X.Value,
                        (int)numericUpDown_HourCenterImage_Y.Value, src.Width, src.Height));
                    //src.Dispose();
                }

                // минуты
                if ((checkBox_AnalogClock_Min.Checked) && (comboBox_AnalogClock_Min_Image.SelectedIndex >= 0))
                {
                    int x1 = (int)numericUpDown_AnalogClock_Min_X.Value;
                    int y1 = (int)numericUpDown_AnalogClock_Min_Y.Value;
                    int offsetX = (int)numericUpDown_AnalogClock_Min_Offset_X.Value;
                    int offsetY = (int)numericUpDown_AnalogClock_Min_Offset_Y.Value;
                    int image_index = comboBox_AnalogClock_Min_Image.SelectedIndex;
                    //int hour = Watch_Face_Preview_Set.TimeW.Hours;
                    int min = Watch_Face_Preview_Set.Time.Minutes;
                    //int sec = Watch_Face_Preview_Set.TimeW.Seconds;
                    //if (hour >= 12) hour = hour - 12;
                    float angle = 360 * min / 60;
                    DrawAnalogClock(gPanel, x1, y1, offsetX, offsetY, image_index, angle);
                }
                if ((checkBox_MinCenterImage.Checked) && (comboBox_MinCenterImage_Image.SelectedIndex >= 0))
                {
                    //src = new Bitmap(ListImagesFullName[comboBox_MinCenterImage_Image.SelectedIndex]);
                    src = OpenFileStream(ListImagesFullName[comboBox_MinCenterImage_Image.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_MinCenterImage_X.Value,
                        (int)numericUpDown_MinCenterImage_Y.Value, src.Width, src.Height));
                    //src.Dispose();
                }

                // секунды
                if (!AnalogClockOffSet)
                {
                    // секунды
                    if ((checkBox_AnalogClock_Sec.Checked) && (comboBox_AnalogClock_Sec_Image.SelectedIndex >= 0))
                    {
                        int x1 = (int)numericUpDown_AnalogClock_Sec_X.Value;
                        int y1 = (int)numericUpDown_AnalogClock_Sec_Y.Value;
                        int offsetX = (int)numericUpDown_AnalogClock_Sec_Offset_X.Value;
                        int offsetY = (int)numericUpDown_AnalogClock_Sec_Offset_Y.Value;
                        int image_index = comboBox_AnalogClock_Sec_Image.SelectedIndex;
                        //int hour = Watch_Face_Preview_Set.TimeW.Hours;
                        //int min = Watch_Face_Preview_Set.TimeW.Minutes;
                        int sec = Watch_Face_Preview_Set.Time.Seconds;
                        //if (hour >= 12) hour = hour - 12;
                        float angle = 360 * sec / 60;
                        DrawAnalogClock(gPanel, x1, y1, offsetX, offsetY, image_index, angle);
                    }
                }
                if ((checkBox_SecCenterImage.Checked) && (comboBox_SecCenterImage_Image.SelectedIndex >= 0))
                {
                    //src = new Bitmap(ListImagesFullName[comboBox_SecCenterImage_Image.SelectedIndex]);
                    src = OpenFileStream(ListImagesFullName[comboBox_SecCenterImage_Image.SelectedIndex]);
                    gPanel.DrawImage(src, new Rectangle((int)numericUpDown_SecCenterImage_X.Value,
                        (int)numericUpDown_SecCenterImage_Y.Value, src.Width, src.Height));
                    //src.Dispose();
                }
            }
            #endregion

            #region Shortcuts
            Logger.WriteLine("PreviewToBitmap (Shortcuts)");
            if (showShortcuts)
            {
                if (checkBox_Shortcuts.Checked)
                {
                    if (checkBox_Shortcuts_Steps.Checked)
                    {
                        int X = (int)numericUpDown_Shortcuts_Steps_X.Value;
                        int Y = (int)numericUpDown_Shortcuts_Steps_Y.Value;
                        int Width = (int)numericUpDown_Shortcuts_Steps_Width.Value;
                        int Height = (int)numericUpDown_Shortcuts_Steps_Height.Value;

                        if (showShortcutsArea)
                        {
                            HatchBrush myHatchBrush = new HatchBrush(HatchStyle.Percent10, Color.White, Color.Transparent);
                            Rectangle rect = new Rectangle(X, Y, Width, Height);
                            gPanel.FillRectangle(myHatchBrush, rect);
                            myHatchBrush = new HatchBrush(HatchStyle.Percent05, Color.Black, Color.Transparent);
                            gPanel.FillRectangle(myHatchBrush, rect); 
                        }
                        if (showShortcutsBorder)
                        {
                            Rectangle rect = new Rectangle(X, Y, Width - 1, Height - 1);
                            using (Pen pen1 = new Pen(Color.White, 1))
                            {
                                gPanel.DrawRectangle(pen1, rect);
                            }
                            using (Pen pen2 = new Pen(Color.Black, 1))
                            {
                                pen2.DashStyle = DashStyle.Dot;
                                gPanel.DrawRectangle(pen2, rect);
                            }
                        }
                    }

                    if (checkBox_Shortcuts_Puls.Checked)
                    {
                        int X = (int)numericUpDown_Shortcuts_Puls_X.Value;
                        int Y = (int)numericUpDown_Shortcuts_Puls_Y.Value;
                        int Width = (int)numericUpDown_Shortcuts_Puls_Width.Value;
                        int Height = (int)numericUpDown_Shortcuts_Puls_Height.Value;

                        if (showShortcutsArea)
                        {
                            HatchBrush myHatchBrush = new HatchBrush(HatchStyle.Percent10, Color.White, Color.Transparent);
                            Rectangle rect = new Rectangle(X, Y, Width, Height);
                            gPanel.FillRectangle(myHatchBrush, rect);
                            myHatchBrush = new HatchBrush(HatchStyle.Percent05, Color.Black, Color.Transparent);
                            gPanel.FillRectangle(myHatchBrush, rect);
                        }
                        if (showShortcutsBorder)
                        {
                            Rectangle rect = new Rectangle(X, Y, Width - 1, Height - 1);
                            using (Pen pen1 = new Pen(Color.White, 1))
                            {
                                gPanel.DrawRectangle(pen1, rect);
                            }
                            using (Pen pen2 = new Pen(Color.Black, 1))
                            {
                                pen2.DashStyle = DashStyle.Dot;
                                gPanel.DrawRectangle(pen2, rect);
                            }
                        }
                    }

                    if (checkBox_Shortcuts_Weather.Checked)
                    {
                        int X = (int)numericUpDown_Shortcuts_Weather_X.Value;
                        int Y = (int)numericUpDown_Shortcuts_Weather_Y.Value;
                        int Width = (int)numericUpDown_Shortcuts_Weather_Width.Value;
                        int Height = (int)numericUpDown_Shortcuts_Weather_Height.Value;

                        if (showShortcutsArea)
                        {
                            HatchBrush myHatchBrush = new HatchBrush(HatchStyle.Percent10, Color.White, Color.Transparent);
                            Rectangle rect = new Rectangle(X, Y, Width, Height);
                            gPanel.FillRectangle(myHatchBrush, rect);
                            myHatchBrush = new HatchBrush(HatchStyle.Percent05, Color.Black, Color.Transparent);
                            gPanel.FillRectangle(myHatchBrush, rect);
                        }
                        if (showShortcutsBorder)
                        {
                            Rectangle rect = new Rectangle(X, Y, Width - 1, Height - 1);
                            using (Pen pen1 = new Pen(Color.White, 1))
                            {
                                gPanel.DrawRectangle(pen1, rect);
                            }
                            using (Pen pen2 = new Pen(Color.Black, 1))
                            {
                                pen2.DashStyle = DashStyle.Dot;
                                gPanel.DrawRectangle(pen2, rect);
                            }
                        }
                    }

                    if (checkBox_Shortcuts_Energy.Checked)
                    {
                        int X = (int)numericUpDown_Shortcuts_Energy_X.Value;
                        int Y = (int)numericUpDown_Shortcuts_Energy_Y.Value;
                        int Width = (int)numericUpDown_Shortcuts_Energy_Width.Value;
                        int Height = (int)numericUpDown_Shortcuts_Energy_Height.Value;

                        if (showShortcutsArea)
                        {
                            HatchBrush myHatchBrush = new HatchBrush(HatchStyle.Percent10, Color.White, Color.Transparent);
                            Rectangle rect = new Rectangle(X, Y, Width, Height);
                            gPanel.FillRectangle(myHatchBrush, rect);
                            myHatchBrush = new HatchBrush(HatchStyle.Percent05, Color.Black, Color.Transparent);
                            gPanel.FillRectangle(myHatchBrush, rect);
                        }
                        if (showShortcutsBorder)
                        {
                            Rectangle rect = new Rectangle(X, Y, Width - 1, Height - 1);
                            using (Pen pen1 = new Pen(Color.White, 1))
                            {
                                gPanel.DrawRectangle(pen1, rect);
                            }
                            using (Pen pen2 = new Pen(Color.Black, 1))
                            {
                                pen2.DashStyle = DashStyle.Dot;
                                gPanel.DrawRectangle(pen2, rect);
                            }
                        }
                    }
                } 
            }
            #endregion
            
            */

            #region Mesh
            Logger.WriteLine("PreviewToBitmap (Mesh)");

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

                    if (i == 0) pen.Width = pen.Width/3f;
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

            DrawEnd:

            src.Dispose();

            if (crop)
            {
                Logger.WriteLine("PreviewToBitmap (crop)");
                Bitmap mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gtr_2.png");
                if (radioButton_GTS2.Checked)
                {
                    mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gts_2.png");
                }
                mask = FormColor(mask);
                gPanel.DrawImage(mask, new Rectangle(0, 0, mask.Width, mask.Height));
                mask.Dispose();
            }

            if (link !=1 && link !=2) FormText();
            Logger.WriteLine("* PreviewToBitmap (end)");
        }

        /// <summary>круговая шкала</summary>
        /// <param name="graphics">Поверхность для рисования</param>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        /// <param name="radius">Радиус</param>
        /// <param name="width">Толщина линии</param>
        /// <param name="lineCap">Тип окончания линии</param>
        /// <param name="startAngle">Начальный угол</param>
        /// <param name="endAngle">Общий угол</param>
        /// <param name="position">Отображаемая величина от 0 до 1</param>
        /// <param name="color">Свет шкалы</param>
        /// <param name="backgroundIndex">Номер фонового изображения</param>
        /// <param name="showProgressArea">Подсвечивать круговую шкалу при наличии фонового изображения</param>
        private void DrawScaleCircle(Graphics graphics, int x, int y, float radius, float width,
            int lineCap, float startAngle, float endAngle, float position, Color color,
             int backgroundIndex, bool showProgressArea)
        {
            Logger.WriteLine("* DrawScaleCircle_image");
            if (position > 1) position = 1;
            float valueAngle = endAngle * position;
            //if (valueAngle == 0) return;
            Bitmap src = new Bitmap(1, 1);
            Pen pen = new Pen(color, width);

            switch (lineCap)
            {
                case 1:
                    pen.EndCap = LineCap.Triangle;
                    pen.StartCap = LineCap.Triangle;
                    break;
                case 2:
                    pen.EndCap = LineCap.Flat;
                    pen.StartCap = LineCap.Flat;
                    break;
                default:
                    pen.EndCap = LineCap.Round;
                    pen.StartCap = LineCap.Round;
                    break;
            }

            int srcX = (int)Math.Round(x - radius - width / 2, MidpointRounding.AwayFromZero);
            int srcY = (int)Math.Round(y - radius - width / 2, MidpointRounding.AwayFromZero);
            int arcX = (int)Math.Round(x - radius, MidpointRounding.AwayFromZero);
            int arcY = (int)Math.Round(y - radius, MidpointRounding.AwayFromZero);
            float CircleWidth = 2 * radius ;

            if (backgroundIndex >= 0 && backgroundIndex < ListImagesFullName.Count)
            {
                src = OpenFileStream(ListImagesFullName[backgroundIndex]);
                graphics.DrawImage(src, new Rectangle(srcX, srcX, src.Width, src.Height));
            }

            try
            {
                graphics.DrawArc(pen, arcX, arcY, CircleWidth, CircleWidth, startAngle, valueAngle);
            }
            catch (Exception)
            {
            }

            //if (pointerIndex >= 0 && pointerIndex < ListImagesFullName.Count)
            //{
            //    src = OpenFileStream(ListImagesFullName[pointerIndex]);
            //    graphics.DrawImage(src, new Rectangle(srcX, srcX, src.Width, src.Height));
            //}
            src.Dispose();

            if (showProgressArea)
            {
                // подсвечивание шкалы заливкой
                HatchBrush myHatchBrush = new HatchBrush(HatchStyle.Percent20, Color.White, Color.Transparent);
                pen.Brush = myHatchBrush;
                graphics.DrawArc(pen, arcX, arcY, CircleWidth, CircleWidth, startAngle, endAngle);
                myHatchBrush = new HatchBrush(HatchStyle.Percent10, Color.Black, Color.Transparent);
                pen.Brush = myHatchBrush;
                graphics.DrawArc(pen, arcX, arcY, CircleWidth, CircleWidth, startAngle, endAngle);

                // подсвечивание внешней и внутреней дуги на шкале
                float w2 = width / 2f;
                using (Pen pen1 = new Pen(Color.White, 1))
                {
                    graphics.DrawArc(pen1, srcX, srcY, CircleWidth + width, CircleWidth + width, startAngle, endAngle);
                    graphics.DrawArc(pen1, srcX + width, srcY + width, CircleWidth - width, CircleWidth - width, startAngle, endAngle);
                }
                using (Pen pen2 = new Pen(Color.Black, 1))
                {
                    graphics.DrawArc(pen2, srcX, srcY, CircleWidth + width, CircleWidth + width, startAngle, endAngle);
                    graphics.DrawArc(pen2, srcX + width, srcY + width, CircleWidth - width, CircleWidth - width, startAngle, endAngle);
                }
            }
            Logger.WriteLine("* DrawScaleCircle_image (end)");

        }

        /// <summary>круговая шкала поверх картинки</summary>
        /// <param name="graphics">Поверхность для рисования</param>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        /// <param name="radius">Радиус</param>
        /// <param name="width">Толщина линии</param>
        /// <param name="lineCap">Тип окончания линии</param>
        /// <param name="startAngle">Начальный угол</param>
        /// <param name="endAngle">Общий угол</param>
        /// <param name="position">Отображаемая величина от 0 до 1</param>
        /// <param name="color">Свет шкалы</param>
        /// <param name="image_index">Номер изображения</param>
        /// <param name="backgroundIndex">Номер фонового изображения</param>
        /// <param name="showProgressArea">Подсвечивать круговую шкалу при наличии фонового изображения</param>
        private void DrawScaleCircle_image(Graphics graphics, int x, int y, float radius, float width,
            int lineCap, float startAngle, float endAngle, float position, int imageIndex,
            int backgroundIndex, bool showProgressArea)
        {
            Logger.WriteLine("* DrawScaleCircle_image");
            if (position > 1) position = 1;
            float valueAngle = endAngle * position;
            //if (valueAngle == 0) return;

            Bitmap src = OpenFileStream(ListImagesFullName[imageIndex]);
            //Pen pen = new Pen(Color.Black, width);
            Pen pen = new Pen(Color.FromArgb(1, 0, 0, 0), 1);

            switch (lineCap)
            {
                case 1:
                    pen.EndCap = LineCap.Triangle;
                    pen.StartCap = LineCap.Triangle;
                    break;
                case 2:
                    pen.EndCap = LineCap.Flat;
                    pen.StartCap = LineCap.Flat;
                    break;
                default:
                    pen.EndCap = LineCap.Round;
                    pen.StartCap = LineCap.Round;
                    break;
            }
            int srcX = (int)Math.Round(x - radius - width / 2, MidpointRounding.AwayFromZero);
            int srcY = (int)Math.Round(y - radius - width / 2, MidpointRounding.AwayFromZero);
            int arcX = (int)Math.Round(x - radius, MidpointRounding.AwayFromZero);
            int arcY = (int)Math.Round(y - radius, MidpointRounding.AwayFromZero);
            float CircleWidth = 2 * radius;

            if (backgroundIndex >= 0 && backgroundIndex < ListImagesFullName.Count)
            {
                src = OpenFileStream(ListImagesFullName[backgroundIndex]);
                graphics.DrawImage(src, new Rectangle(srcX, srcX, src.Width, src.Height));
            }

            Bitmap mask = new Bitmap(src.Width, src.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics gPanel = Graphics.FromImage(mask);
            gPanel.SmoothingMode = SmoothingMode.AntiAlias;
            try
            {
                //int centrX = (int)(width / 2f + radius);
                //int centrY = (int)(width / 2f + radius);
                //gPanel.DrawLine(pen, centrX, centrY, centrX + 1, centrY + 1);
                gPanel.DrawLine(pen, 0, 0, 0 + 1, 0 + 1);
                pen = new Pen(Color.Black, width);
                //pen.Width = width;
                //pen.Color = Color.Black;
                gPanel.DrawArc(pen, (int)(width / 2f), (int)(width / 2f), CircleWidth, CircleWidth, 
                    startAngle, valueAngle);

             
                //src = ApplyAlfaMask(src, mask);
                src = ApplyMask(src, mask);
                //src = mask;
                graphics.DrawImage(src, new Rectangle(srcX, srcY, src.Width, src.Height));
                //src.Dispose();
                mask.Dispose();

            }
            catch (Exception)
            {

            }

            if (showProgressArea)
            {
                // подсвечивание шкалы заливкой
                HatchBrush myHatchBrush = new HatchBrush(HatchStyle.Percent20, Color.White, Color.Transparent);
                pen.Brush = myHatchBrush;
                graphics.DrawArc(pen, arcX, arcY, CircleWidth, CircleWidth, startAngle, endAngle);
                myHatchBrush = new HatchBrush(HatchStyle.Percent10, Color.Black, Color.Transparent);
                pen.Brush = myHatchBrush;
                graphics.DrawArc(pen, arcX, arcY, CircleWidth, CircleWidth, startAngle, endAngle);

                // подсвечивание внешней и внутреней дуги на шкале
                float w2 = width / 2f;
                using (Pen pen1 = new Pen(Color.White, 1))
                {
                    graphics.DrawArc(pen1, srcX, srcY, CircleWidth + width, CircleWidth + width, startAngle, endAngle);
                    graphics.DrawArc(pen1, srcX + width, srcY + width, CircleWidth - width, CircleWidth - width, startAngle, endAngle);
                }
                using (Pen pen2 = new Pen(Color.Black, 1))
                {
                    graphics.DrawArc(pen2, srcX, srcY, CircleWidth + width, CircleWidth + width, startAngle, endAngle);
                    graphics.DrawArc(pen2, srcX + width, srcY + width, CircleWidth - width, CircleWidth - width, startAngle, endAngle);
                }
            }
            src.Dispose();
            Logger.WriteLine("* DrawScaleCircle_image (end)");

        }

        /// <summary>Рисует линейную шкалу</summary>
        /// <param name="graphics">Поверхность для рисования</param>
        /// <param name="image_index">Номер изображения</param>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата y</param>
        /// <param name="length">Длина шкалы</param>
        /// <param name="width">Толщина шкалы</param>
        /// <param name="position">Позиция шкалы от 0 до 1</param>
        /// <param name="color">Свет шкалы</param>
        /// <param name="lineCap">Тип окончания линии</param>
        /// <param name="backgroundIndex">Номер фонового изображения</param>
        /// <param name="showProgressArea">Подсвечивать шкалу</param>
        private void DrawScaleLinear(Graphics graphics, int x, int y, int length, int width, float position, 
            Color color, int lineCap, int backgroundIndex)
        {
            Bitmap src = new Bitmap(1, 1);

            if (length > 0)
            {
                int x1 = (int)(x + width / 2f);
                //int x1 = (int)Math.Round(x + width / 2d, MidpointRounding.AwayFromZero);
                int length1 = length - width;
                if (lineCap == 0)
                {
                    x1 = x;
                    length1 = length;
                }
                int position1 = (int)(length1 * position);
                if (position1 < 0) position1 = 0;
                int x2 = x1 + position1;
                int y1 = (int)(y + width / 2f);
                //int y1 = (int)Math.Round(y + width / 2d, MidpointRounding.AwayFromZero);

                if (backgroundIndex >= 0 && backgroundIndex < ListImagesFullName.Count)
                {
                    src = OpenFileStream(ListImagesFullName[backgroundIndex]);
                    graphics.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                }

                Pen pen = new Pen(color, width);
                switch (lineCap)
                {
                    case 1:
                        pen.EndCap = LineCap.Triangle;
                        pen.StartCap = LineCap.Triangle;
                        break;
                    case 2:
                        pen.EndCap = LineCap.Round;
                        pen.StartCap = LineCap.Round;
                        break;
                    default:
                        pen.EndCap = LineCap.Flat;
                        pen.StartCap = LineCap.Flat;
                        break;
                }
                graphics.DrawLine(pen, new Point(x1, y1), new Point(x2, y1));

                
            }
            else
            {
                int x1 = x - width / 2;
                int length1 = length + width;
                int position1 = (int)Math.Round(length1 * position);
                if (position1 > 0) position1 = 0;
                int x2 = x1 + position1;
                int y1 = y + width / 2;

                if (backgroundIndex >= 0 && backgroundIndex < ListImagesFullName.Count)
                {
                    src = OpenFileStream(ListImagesFullName[backgroundIndex]);
                    graphics.DrawImage(src, new Rectangle(x + length, y, src.Width, src.Height));
                }

                Pen pen = new Pen(color, width);
                pen.EndCap = LineCap.Round;
                pen.StartCap = LineCap.Round;
                graphics.DrawLine(pen, new Point(x1, y1), new Point(x2, y1));

            }

            src.Dispose();
        }

        /// <summary>Рисует линейную шкалу с фоновым изображением</summary>
        /// <param name="graphics">Поверхность для рисования</param>
        /// <param name="image_index">Номер изображения</param>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата y</param>
        /// <param name="length">Длина шкалы</param>
        /// <param name="width">Толщина шкалы</param>
        /// <param name="position">Позиция шкалы от 0 до 1</param>
        /// <param name="imageIndex">Изображение шкалы</param>
        /// <param name="lineCap">Тип окончания линии</param>
        /// <param name="backgroundIndex">Номер фонового изображения</param>
        /// <param name="showProgressArea">Подсвечивать шкалу</param>
        private void DrawScaleLinear(Graphics graphics, int x, int y, int length, int width, float position, 
            int imageIndex, int lineCap, int backgroundIndex, bool showProgressArea)
        {
            Bitmap src = new Bitmap(1, 1);

            if (length > 0)
            {
                int x1 = (int)(x + width / 2f);
                //int x1 = (int)Math.Round(x + width / 2d, MidpointRounding.AwayFromZero);
                int length1 = length - width;
                if (lineCap == 0)
                {
                    x1 = x;
                    length1 = length;
                }
                int position1 = (int)(length1 * position);
                //int position1 = (int)Math.Round(length1 * position, MidpointRounding.AwayFromZero);
                if (position1 < 0) position1 = 0;
                int x2 = x1 + position1;
                int y1 = (int)(y + width / 2f);
                //int y1 = (int)Math.Round(y + width / 2d, MidpointRounding.AwayFromZero);
                if (backgroundIndex >= 0 && backgroundIndex < ListImagesFullName.Count)
                {
                    src = OpenFileStream(ListImagesFullName[backgroundIndex]);
                    graphics.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                }

                src = OpenFileStream(ListImagesFullName[imageIndex]);
                Pen pen = new Pen(Color.Black, width);
                switch (lineCap)
                {
                    case 1:
                        pen.EndCap = LineCap.Triangle;
                        pen.StartCap = LineCap.Triangle;
                        break;
                    case 2:
                        pen.EndCap = LineCap.Round;
                        pen.StartCap = LineCap.Round;
                        break;
                    default:
                        pen.EndCap = LineCap.Flat;
                        pen.StartCap = LineCap.Flat;
                        break;
                }

                //graphics.DrawLine(pen, new Point(x1, y1), new Point(x2, y1));

                Bitmap mask = new Bitmap(src.Width, src.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics gPanel = Graphics.FromImage(mask);
                gPanel.SmoothingMode = SmoothingMode.AntiAlias;
                try
                {
                    gPanel.DrawLine(pen, new Point(x1 - x, y1 - y), new Point(x2 - x, y1 - y));
                    //src = ApplyAlfaMask(src, mask);
                    src = ApplyMask(src, mask);

                    graphics.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    //src.Dispose();
                    mask.Dispose();

                    if (showProgressArea)
                    {
                        // подсвечивание шкалы заливкой
                        HatchBrush myHatchBrush = new HatchBrush(HatchStyle.Percent20, Color.White, Color.Transparent);
                        pen.Brush = myHatchBrush;
                        graphics.DrawLine(pen, new Point(x1, y1), new Point(x1 + length1, y1));
                        myHatchBrush = new HatchBrush(HatchStyle.Percent10, Color.Black, Color.Transparent);
                        pen.Brush = myHatchBrush;
                        graphics.DrawLine(pen, new Point(x1, y1), new Point(x1 + length1, y1));

                    }
                }
                catch (Exception)
                {

                }
            }
            else
            {
                int x1 = x - width / 2;
                int length1 = length + width;
                if (lineCap == 0)
                {
                    x1 = x;
                    length1 = length;
                }
                int position1 = (int)Math.Round(length1 * position);
                if (position1 > 0) position1 = 0;
                int x2 = x1 + position1;
                int y1 = y + width / 2;

                if (backgroundIndex >= 0 && backgroundIndex < ListImagesFullName.Count)
                {
                    src = OpenFileStream(ListImagesFullName[backgroundIndex]);
                    graphics.DrawImage(src, new Rectangle(x + length, y, src.Width, src.Height));
                }

                src = OpenFileStream(ListImagesFullName[imageIndex]);
                Pen pen = new Pen(Color.Black, width);
                pen.EndCap = LineCap.Round;
                pen.StartCap = LineCap.Round;

                //graphics.DrawLine(pen, new Point(x1, y1), new Point(x2, y1));

                Bitmap mask = new Bitmap(src.Width, src.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics gPanel = Graphics.FromImage(mask);
                gPanel.SmoothingMode = SmoothingMode.AntiAlias;
                try
                {
                    gPanel.DrawLine(pen, new Point(x - x1, y1 - y), new Point(x - x2, y1 - y));
                    //src = ApplyAlfaMask(src, mask);
                    src = ApplyMask(src, mask);

                    graphics.DrawImage(src, new Rectangle(x2 - width / 2, y, src.Width, src.Height));
                    //src.Dispose();
                    mask.Dispose();

                    if (showProgressArea)
                    {
                        // подсвечивание шкалы заливкой
                        HatchBrush myHatchBrush = new HatchBrush(HatchStyle.Percent20, Color.White, Color.Transparent);
                        pen.Brush = myHatchBrush;
                        graphics.DrawLine(pen, new Point(x1, y1), new Point(x1 + length1, y1));
                        myHatchBrush = new HatchBrush(HatchStyle.Percent10, Color.Black, Color.Transparent);
                        pen.Brush = myHatchBrush;
                        graphics.DrawLine(pen, new Point(x1, y1), new Point(x1 + length1, y1));

                    }
                }
                catch (Exception)
                {

                }
            }

            src.Dispose();
        }

        /// <summary>Рисует линейную шкалу</summary>
        /// <param name="graphics">Поверхность для рисования</param>
        /// <param name="image_index">Номер изображения</param>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата y</param>
        /// <param name="length">Длина шкалы</param>
        /// <param name="width">Толщина шкалы</param>
        /// <param name="position">Позиция шкалы от 0 до 1</param>
        /// <param name="color">Свет шкалы</param>
        /// <param name="pointerIndex">Номер изображения маркера</param>
        /// <param name="backgroundIndex">Номер фонового изображения</param>
        /// <param name="showProgressArea">Подсвечивать шкалу</param>
        private void DrawScaleLinearPointer(Graphics graphics, int x, int y, int length, int width, float position, Color color, 
            int pointerIndex, int backgroundIndex, bool showProgressArea)
        {
            Bitmap src = new Bitmap(1, 1);

            if (length > 0)
            {
                int x1 = (int)(x + width / 2f);
                //int x1 = (int)Math.Round(x + width / 2d, MidpointRounding.AwayFromZero);
                int length1 = length - width;
                int position1 = (int)(length1 * position);
                if (position1 < 0) position1 = 0;
                int x2 = x1 + position1;
                int y1 = (int)(y + width / 2f);
                //int y1 = (int)Math.Round(y + width / 2d, MidpointRounding.AwayFromZero);

                if (backgroundIndex >= 0 && backgroundIndex < ListImagesFullName.Count)
                {
                    src = OpenFileStream(ListImagesFullName[backgroundIndex]);
                    graphics.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                }

                Pen pen = new Pen(color, width);
                pen.EndCap = LineCap.Round;
                pen.StartCap = LineCap.Round;
                graphics.DrawLine(pen, new Point(x1, y1), new Point(x2, y1));

                if (pointerIndex >= 0 && pointerIndex < ListImagesFullName.Count)
                {
                    src = OpenFileStream(ListImagesFullName[pointerIndex]);
                    int x3 = x2 - width / 2;
                    graphics.DrawImage(src, new Rectangle(x3, y1 - src.Height/2, src.Width, src.Height));
                }

                if (showProgressArea)
                {
                    // подсвечивание шкалы заливкой
                    HatchBrush myHatchBrush = new HatchBrush(HatchStyle.Percent20, Color.White, Color.Transparent);
                    pen.Brush = myHatchBrush;
                    graphics.DrawLine(pen, new Point(x1, y1), new Point(x1 + length1, y1));
                    myHatchBrush = new HatchBrush(HatchStyle.Percent10, Color.Black, Color.Transparent);
                    pen.Brush = myHatchBrush;
                    graphics.DrawLine(pen, new Point(x1, y1), new Point(x2, y1));
                }
            }
            else
            {
                int x1 = x - width / 2;
                int length1 = length + width;
                int position1 = (int)Math.Round(length1 * position);
                if (position1 > 0) position1 = 0;
                int x2 = x1 + position1;
                int y1 = y + width / 2;

                if (backgroundIndex >= 0 && backgroundIndex < ListImagesFullName.Count)
                {
                    src = OpenFileStream(ListImagesFullName[backgroundIndex]);
                    graphics.DrawImage(src, new Rectangle(x + length, y, src.Width, src.Height));
                }

                Pen pen = new Pen(color, width);
                pen.EndCap = LineCap.Round;
                pen.StartCap = LineCap.Round;
                graphics.DrawLine(pen, new Point(x1, y1), new Point(x2, y1));

                if (pointerIndex >= 0 && pointerIndex < ListImagesFullName.Count)
                {
                    src = OpenFileStream(ListImagesFullName[pointerIndex]);
                    int x3 = x2 - src.Width / 2;
                    graphics.DrawImage(src, new Rectangle(x3, y, src.Width, src.Height));
                }

                if (showProgressArea)
                {
                    // подсвечивание шкалы заливкой
                    HatchBrush myHatchBrush = new HatchBrush(HatchStyle.Percent20, Color.White, Color.Transparent);
                    pen.Brush = myHatchBrush;
                    graphics.DrawLine(pen, new Point(x1, y1), new Point(x1 + length1, y1));
                    myHatchBrush = new HatchBrush(HatchStyle.Percent10, Color.Black, Color.Transparent);
                    pen.Brush = myHatchBrush;
                    graphics.DrawLine(pen, new Point(x1, y1), new Point(x2, y1));
                }
            }

            src.Dispose();
        }

        /// <summary>Рисует линейную шкалу с фоновым изображением</summary>
        /// <param name="graphics">Поверхность для рисования</param>
        /// <param name="image_index">Номер изображения</param>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата y</param>
        /// <param name="length">Длина шкалы</param>
        /// <param name="width">Толщина шкалы</param>
        /// <param name="position">Позиция шкалы от 0 до 1</param>
        /// <param name="imageIndex">Изображение шкалы</param>
        /// <param name="pointerIndex">Номер изображения маркера</param>
        /// <param name="backgroundIndex">Номер фонового изображения</param>
        /// <param name="showProgressArea">Подсвечивать шкалу</param>
        private void DrawScaleLinearPointer_image(Graphics graphics, int x, int y, int length, int width, float position, int imageIndex,
            int pointerIndex, int backgroundIndex, bool showProgressArea)
        {
            Bitmap src = new Bitmap(1, 1);

            if (length > 0)
            {
                int x1 = (int)(x + width / 2f);
                //int x1 = (int)Math.Round(x + width / 2d, MidpointRounding.AwayFromZero);
                int length1 = length - width;
                int position1 = (int)(length1 * position);
                //int position1 = (int)Math.Round(length1 * position, MidpointRounding.AwayFromZero);
                if (position1 < 0) position1 = 0;
                int x2 = x1 + position1;
                int y1 = (int)(y + width / 2f);
                //int y1 = (int)Math.Round(y + width / 2d, MidpointRounding.AwayFromZero);
                if (backgroundIndex >= 0 && backgroundIndex < ListImagesFullName.Count)
                {
                    src = OpenFileStream(ListImagesFullName[backgroundIndex]);
                    graphics.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                }

                src = OpenFileStream(ListImagesFullName[imageIndex]);
                Pen pen = new Pen(Color.Black, width);
                pen.EndCap = LineCap.Round;
                pen.StartCap = LineCap.Round;

                //graphics.DrawLine(pen, new Point(x1, y1), new Point(x2, y1));

                Bitmap mask = new Bitmap(src.Width, src.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics gPanel = Graphics.FromImage(mask);
                gPanel.SmoothingMode = SmoothingMode.AntiAlias;
                try
                {
                    gPanel.DrawLine(pen, new Point(x1 - x, y1 - y), new Point(x2 - x, y1 - y));
                    //src = ApplyAlfaMask(src, mask);
                    src = ApplyMask(src, mask);

                    graphics.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    //src.Dispose();
                    mask.Dispose();

                    if (showProgressArea)
                    {
                        // подсвечивание шкалы заливкой
                        HatchBrush myHatchBrush = new HatchBrush(HatchStyle.Percent20, Color.White, Color.Transparent);
                        pen.Brush = myHatchBrush;
                        graphics.DrawLine(pen, new Point(x1, y1), new Point(x1 + length1, y1));
                        myHatchBrush = new HatchBrush(HatchStyle.Percent10, Color.Black, Color.Transparent);
                        pen.Brush = myHatchBrush;
                        graphics.DrawLine(pen, new Point(x1, y1), new Point(x1 + length1, y1));

                    }

                    if (pointerIndex >= 0 && pointerIndex < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[pointerIndex]);
                        int x3 = x2 - width / 2;
                        graphics.DrawImage(src, new Rectangle(x3, y1 - src.Height / 2, src.Width, src.Height));
                    }
                }
                catch (Exception)
                {

                } 
            }
            else
            {
                int x1 = x - width / 2;
                int length1 = length + width;
                int position1 = (int)Math.Round(length1 * position);
                if (position1 > 0) position1 = 0;
                int x2 = x1 + position1;
                int y1 = y + width / 2;

                if (backgroundIndex >= 0 && backgroundIndex < ListImagesFullName.Count)
                {
                    src = OpenFileStream(ListImagesFullName[backgroundIndex]);
                    graphics.DrawImage(src, new Rectangle(x + length, y, src.Width, src.Height));
                }

                src = OpenFileStream(ListImagesFullName[imageIndex]);
                Pen pen = new Pen(Color.Black, width);
                pen.EndCap = LineCap.Round;
                pen.StartCap = LineCap.Round;

                //graphics.DrawLine(pen, new Point(x1, y1), new Point(x2, y1));

                Bitmap mask = new Bitmap(src.Width, src.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics gPanel = Graphics.FromImage(mask);
                gPanel.SmoothingMode = SmoothingMode.AntiAlias;
                try
                {
                    gPanel.DrawLine(pen, new Point(x - x1, y1 - y), new Point(x - x2, y1 - y));
                    //src = ApplyAlfaMask(src, mask);
                    src = ApplyMask(src, mask);

                    graphics.DrawImage(src, new Rectangle(x2 - width / 2, y, src.Width, src.Height));
                    //src.Dispose();
                    mask.Dispose();

                    if (showProgressArea)
                    {
                        // подсвечивание шкалы заливкой
                        HatchBrush myHatchBrush = new HatchBrush(HatchStyle.Percent20, Color.White, Color.Transparent);
                        pen.Brush = myHatchBrush;
                        graphics.DrawLine(pen, new Point(x1, y1), new Point(x1 + length1, y1));
                        myHatchBrush = new HatchBrush(HatchStyle.Percent10, Color.Black, Color.Transparent);
                        pen.Brush = myHatchBrush;
                        graphics.DrawLine(pen, new Point(x1, y1), new Point(x1 + length1, y1));

                    }

                    if (pointerIndex >= 0 && pointerIndex < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[pointerIndex]);
                        int x3 = x2 - src.Width / 2;
                        graphics.DrawImage(src, new Rectangle(x3, y, src.Width, src.Height));
                    }
                }
                catch (Exception)
                {

                }
            }

            src.Dispose();
        }

        /// <summary>Рисует число</summary>
        /// <param name="graphics">Поверхность для рисования</param>
        /// <param name="image_index">Номер изображения</param>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата y</param>
        /// <param name="spacing">Величина отступа</param>
        /// <param name="alignment">Выравнивание</param>
        /// <param name="value">Отображаемая величина</param>
        /// <param name="addZero">Отображать начальные нули</param>
        /// <param name="value_lenght">Количество отображаемых символов</param>
        /// <param name="separator_index">Символ разделителя (единиц измерения)</param>
        /// <param name="BBorder">Рисовать рамку по координатам, вокруг элементов с выравниванием</param>
        private int Draw_dagital_text(Graphics graphics, int image_index, int x, int y, int spacing, 
            int alignment, int value, bool addZero, int value_lenght, int separator_index, bool BBorder)
        {
            int result = 0;
            Logger.WriteLine("* Draw_dagital_text");
            var src = new Bitmap(1, 1);
            int _number;
            int i;
            string value_S = value.ToString();
            if (addZero)
            {
                while (value_S.Length < value_lenght)
                {
                    value_S = "0" + value_S;
                }
            }
            char[] CH = value_S.ToCharArray();
            int DateLenghtReal = 0;
            Logger.WriteLine("DateLenght");
            foreach (char ch in CH)
            {
                _number = 0;
                if (int.TryParse(ch.ToString(), out _number))
                {
                    i = image_index + _number;
                    if (i < ListImagesFullName.Count)
                    {
                        //src = new Bitmap(ListImagesFullName[i]);
                        src = OpenFileStream(ListImagesFullName[i]);
                        DateLenghtReal = DateLenghtReal + src.Width + spacing;
                        //src.Dispose();
                    }
                }

            }
            DateLenghtReal = DateLenghtReal - spacing;

            src = OpenFileStream(ListImagesFullName[image_index]);
            int width = src.Width;
            int height = src.Height;
            int DateLenght = width * value_lenght + spacing * (value_lenght - 1);

            int PointX = 0;
            int PointY = y;
            switch (alignment)
            {
                case 0:
                    PointX = x;
                    break;
                case 1:
                    PointX = x + DateLenght - DateLenghtReal;
                    break;
                case 2:
                    PointX = x + DateLenght / 2 - DateLenghtReal / 2;
                    break;
                default:
                    PointX = x;
                    break;
            }

            Logger.WriteLine("Draw value");
            foreach (char ch in CH)
            {
                _number = 0;
                if (int.TryParse(ch.ToString(), out _number))
                {
                    i = image_index + _number;
                    if (i < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[i]);
                        graphics.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                        PointX = PointX + src.Width + spacing;
                        //src.Dispose();
                    }
                }

            }
            result = PointX - spacing;
            if(separator_index > -1)
            {
                src = OpenFileStream(ListImagesFullName[separator_index]);
                graphics.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                result = result + src.Width + spacing;
            }
            src.Dispose();

            if (BBorder)
            {
                Logger.WriteLine("DrawBorder");
                Rectangle rect = new Rectangle(x, y, DateLenght - 1, height - 1);
                using (Pen pen1 = new Pen(Color.White, 1))
                {
                    graphics.DrawRectangle(pen1, rect);
                }
                using (Pen pen2 = new Pen(Color.Black, 1))
                {
                    pen2.DashStyle = DashStyle.Dot;
                    graphics.DrawRectangle(pen2, rect);
                }
            }

            Logger.WriteLine("* Draw_dagital_text (end)");
            return result;
        }

        /// <summary>Рисует число с десятичным разделителем</summary>
        /// <param name="graphics">Поверхность для рисования</param>
        /// <param name="image_index">Номер изображения</param>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата y</param>
        /// <param name="spacing">Величина отступа</param>
        /// <param name="alignment">Выравнивание</param>
        /// <param name="value">Отображаемая величина</param>
        /// <param name="addZero">Отображать начальные нули</param>
        /// <param name="value_lenght">Количество отображаемых символов</param>
        /// <param name="separator_index">Символ разделителя (единиц измерения)</param>
        /// <param name="decimalPoint_index">Символ десятичного разделителя</param>
        /// <param name="decCount">Число знаков после запятой</param>
        /// <param name="BBorder">Рисовать рамку по координатам, вокруг элементов с выравниванием</param>
        private void Draw_dagital_text(Graphics graphics, int image_index, int x, int y, int spacing,
            int alignment, double value, bool addZero, int value_lenght, int separator_index, 
            int decimalPoint_index, int decCount, bool BBorder)
        {
            Logger.WriteLine("* Draw_dagital_text");
            value = Math.Round(value, 2, MidpointRounding.AwayFromZero);
            var Digit = new Bitmap(ListImagesFullName[image_index]);
            //var Delimit = new Bitmap(1, 1);
            //if (dec >= 0) Delimit = new Bitmap(ListImagesFullName[dec]);
            string decimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            string data_numberS = value.ToString();
            if (decCount > 0)
            {
                if (data_numberS.IndexOf(decimalSeparator) < 0) data_numberS = data_numberS + decimalSeparator;
                while (data_numberS.IndexOf(decimalSeparator) > data_numberS.Length - decCount - 1)
                {
                    data_numberS = data_numberS + "0";
                }
            }
            int DateLenghtReal = 0;
            int _number;
            int i;
            var src = new Bitmap(1, 1);
            char[] CH = data_numberS.ToCharArray();

            Logger.WriteLine("DateLenght");
            foreach (char ch in CH)
            {
                _number = 0;
                if (int.TryParse(ch.ToString(), out _number))
                {
                    i = image_index + _number;
                    if (i < ListImagesFullName.Count)
                    {
                        //src = new Bitmap(ListImagesFullName[i]);
                        src = OpenFileStream(ListImagesFullName[i]);
                        DateLenghtReal = DateLenghtReal + src.Width + spacing;
                        //src.Dispose();
                    }
                }
                else
                {
                    if (decimalPoint_index >= 0 && decimalPoint_index < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[decimalPoint_index]);
                        DateLenghtReal = DateLenghtReal + src.Width + spacing;
                        //src.Dispose();
                    }
                }

            }
            DateLenghtReal = DateLenghtReal - spacing;

            src = OpenFileStream(ListImagesFullName[image_index]);
            int width = src.Width;
            int height = src.Height;
            int width_separator = 0;
            if (decimalPoint_index >= 0)
            {
                src = OpenFileStream(ListImagesFullName[decimalPoint_index]);
                width_separator = src.Width + value_lenght; 
            }
            int DateLenght = width * value_lenght + spacing * (value_lenght -1) + width_separator;

            int PointX = 0;
            int PointY = y;
            switch (alignment)
            {
                case 0:
                    PointX = x;
                    break;
                case 1:
                    PointX = x + DateLenght - DateLenghtReal;
                    break;
                case 2:
                    PointX = x + DateLenght / 2 - DateLenghtReal / 2;
                    break;
                default:
                    PointX = x;
                    break;
            }

            Logger.WriteLine("Draw value");
            foreach (char ch in CH)
            {
                _number = 0;
                if (int.TryParse(ch.ToString(), out _number))
                {
                    i = image_index + _number;
                    if (i < ListImagesFullName.Count)
                    {
                        //src = new Bitmap(ListImagesFullName[i]);
                        src = OpenFileStream(ListImagesFullName[i]);
                        graphics.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                        PointX = PointX + src.Width + spacing;
                        //src.Dispose();
                    }
                }
                else
                {
                    if (decimalPoint_index >= 0 && decimalPoint_index < ListImagesFullName.Count)
                    {
                        //src = new Bitmap(ListImagesFullName[dec]);
                        src = OpenFileStream(ListImagesFullName[decimalPoint_index]);
                        graphics.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                        PointX = PointX + src.Width + spacing;
                        //src.Dispose();
                    }
                }

            }
            if (separator_index > -1)
            {
                src = OpenFileStream(ListImagesFullName[separator_index]);
                graphics.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
            }
            src.Dispose();

            if (BBorder)
            {
                Logger.WriteLine("DrawBorder");
                Rectangle rect = new Rectangle(x, y, DateLenght - 1, height - 1);
                using (Pen pen1 = new Pen(Color.White, 1))
                {
                    graphics.DrawRectangle(pen1, rect);
                }
                using (Pen pen2 = new Pen(Color.Black, 1))
                {
                    pen2.DashStyle = DashStyle.Dot;
                    graphics.DrawRectangle(pen2, rect);
                }
            }

            Logger.WriteLine("* Draw_dagital_text (end)");
        }

        /// <summary>Рисует погоду числом</summary>
        /// <param name="graphics">Поверхность для рисования</param>
        /// <param name="image_index">Номер изображения</param>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата y</param>
        /// <param name="spacing">Величина отступа</param>
        /// <param name="alignment">Выравнивание</param>
        /// <param name="value">Отображаемая величина</param>
        /// <param name="image_minus_index">Символ "-"</param>
        /// <param name="separator_index">Символ разделителя (единиц измерения)</param>
        /// <param name="BBorder">Рисовать рамку по координатам, вокруг элементов с выравниванием</param>
        private int Draw_weather_text(Graphics graphics, int image_index, int x, int y, int spacing,
            int alignment, int value, int image_minus_index, int separator_index, bool BBorder)
        {
            int result = 0;
            Logger.WriteLine("* Draw_weather_text");
            var src = new Bitmap(1, 1);
            int _number;
            int i;
            string value_S = value.ToString();
            //if (addZero)
            //{
            //    while (value_S.Length < value_lenght)
            //    {
            //        value_S = "0" + value_S;
            //    }
            //}
            char[] CH = value_S.ToCharArray();
            int DateLenghtReal = 0;
            Logger.WriteLine("DateLenght");
            foreach (char ch in CH)
            {
                _number = 0;
                if (int.TryParse(ch.ToString(), out _number))
                {
                    i = image_index + _number;
                    if (i < ListImagesFullName.Count)
                    {
                        //src = new Bitmap(ListImagesFullName[i]);
                        src = OpenFileStream(ListImagesFullName[i]);
                        DateLenghtReal = DateLenghtReal + src.Width + spacing;
                        //src.Dispose();
                    }
                }
                else
                {
                    if (image_minus_index >= 0 && image_minus_index < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[image_minus_index]);
                        DateLenghtReal = DateLenghtReal + src.Width + spacing;
                        //src.Dispose(); 
                    }
                }
            }

            DateLenghtReal = DateLenghtReal - spacing;

            src = OpenFileStream(ListImagesFullName[image_index]);
            int width = src.Width;
            int height = src.Height;
            int DateLenght = width * 3 + spacing * 2;
            if (radioButton_GTS2.Checked)
            {
                if (image_minus_index >= 0 && image_minus_index < ListImagesFullName.Count)
                {
                    src = OpenFileStream(ListImagesFullName[image_minus_index]);
                    DateLenght = DateLenght + src.Width + spacing;
                } 
            }

            int PointX = 0;
            int PointY = y;
            switch (alignment)
            {
                case 0:
                    PointX = x;
                    break;
                case 1:
                    PointX = x + DateLenght - DateLenghtReal;
                    break;
                case 2:
                    PointX = x + DateLenght / 2 - DateLenghtReal / 2;
                    break;
                default:
                    PointX = x;
                    break;
            }

            Logger.WriteLine("Draw value");
            foreach (char ch in CH)
            {
                _number = 0;
                if (int.TryParse(ch.ToString(), out _number))
                {
                    i = image_index + _number;
                    if (i < ListImagesFullName.Count)
                    {
                        //src = new Bitmap(ListImagesFullName[i]);
                        src = OpenFileStream(ListImagesFullName[i]);
                        graphics.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                        PointX = PointX + src.Width + spacing;
                        //src.Dispose();
                    }
                }
                else
                {
                    if (image_minus_index >= 0 && image_minus_index < ListImagesFullName.Count)
                    {
                        //src = new Bitmap(ListImagesFullName[dec]);
                        src = OpenFileStream(ListImagesFullName[image_minus_index]);
                        graphics.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                        PointX = PointX + src.Width + spacing;
                        //src.Dispose();
                    }
                }

            }
            result = PointX - spacing;
            if (separator_index > -1)
            {
                src = OpenFileStream(ListImagesFullName[separator_index]);
                graphics.DrawImage(src, new Rectangle(PointX, PointY, src.Width, src.Height));
                result = result + src.Width + spacing;
            }
            src.Dispose();

            if (BBorder)
            {
                Logger.WriteLine("DrawBorder");
                Rectangle rect = new Rectangle(x, y, DateLenght - 1, height - 1);
                using (Pen pen1 = new Pen(Color.White, 1))
                {
                    graphics.DrawRectangle(pen1, rect);
                }
                using (Pen pen2 = new Pen(Color.Black, 1))
                {
                    pen2.DashStyle = DashStyle.Dot;
                    graphics.DrawRectangle(pen2, rect);
                }
            }

            Logger.WriteLine("* Draw_weather_text (end)");
            return result;
        }

        /// <summary>Рисует стрелки</summary>
        /// <param name="graphics">Поверхность для рисования</param>
        /// <param name="x">Центр стрелки X</param>
        /// <param name="y">Центр стрелки Y</param>
        /// <param name="offsetX">Смещение от центра по X</param>
        /// <param name="offsetY">Смещение от центра по Y</param>
        /// <param name="image_index">Номер изображения</param>
        /// <param name="angle">Угол поворота стрелки в градусах</param>
        /// <param name="center_marker">Отображать маркер на точке вращения</param>
        public void DrawAnalogClock(Graphics graphics, int x, int y, int offsetX, int offsetY, int image_index, float angle, bool showCentrHend)
        {
            Logger.WriteLine("* DrawAnalogClock");
            Bitmap src = OpenFileStream(ListImagesFullName[image_index]);
            graphics.TranslateTransform(x, y);
            graphics.RotateTransform(angle);
            graphics.DrawImage(src, new Rectangle(-offsetX, -offsetY, src.Width, src.Height));
            graphics.RotateTransform(-angle);
            graphics.TranslateTransform(-x, -y);
            src.Dispose();

            if (showCentrHend)
            {
                Logger.WriteLine("Draw showCentrHend");
                using (Pen pen1 = new Pen(Color.White, 1))
                {
                    graphics.DrawLine(pen1, new Point(x - 5, y), new Point(x + 5, y));
                    graphics.DrawLine(pen1, new Point(x, y - 5), new Point(x, y + 5));
                }
                using (Pen pen2 = new Pen(Color.Black, 1))
                {
                    pen2.DashStyle = DashStyle.Dot;
                    graphics.DrawLine(pen2, new Point(x - 5, y), new Point(x + 5, y));
                    graphics.DrawLine(pen2, new Point(x, y - 5), new Point(x, y + 5));
                }
            }
            Logger.WriteLine("* DrawAnalogClock (end)");
        }


        /// <summary>круговая шкала поверх картинки</summary>
        /// <param name="graphics">Поверхность для рисования</param>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        /// <param name="imageIndex">Номер изображения</param>
        /// <param name="radius">Радиус</param>
        /// <param name="width">Толщина линии</param>
        /// <param name="lineCap">Тип окончания линии</param>
        /// <param name="startAngle">Начальный угол</param>
        /// <param name="endAngle">Общий угол</param>
        /// <param name="showProgressArea">Подсвечивать круговую шкалу при наличии фонового изображения</param>
        private void CircleOnBitmap(Graphics graphics, int x, int y, int imageIndex, int radius, float width,
            int lineCap, float StartAngle, float EndAngle, bool showProgressArea)
        {
            Logger.WriteLine("* CircleOnBitmap");
            if (EndAngle == 0) return;
            //Bitmap src = new Bitmap(ListImagesFullName[imageIndex]);
            Bitmap src = OpenFileStream(ListImagesFullName[imageIndex]);
            Pen pen = new Pen(Color.Black, width);
            //Pen pen = new Pen(Color.FromArgb(1, 0, 0, 0), 1);

            switch (lineCap)
            {
                case 1:
                    pen.EndCap = LineCap.Triangle;
                    pen.StartCap = LineCap.Triangle;
                    break;
                case 2:
                    pen.EndCap = LineCap.Flat;
                    pen.StartCap = LineCap.Flat;
                    break;
                default:
                    pen.EndCap = LineCap.Round;
                    pen.StartCap = LineCap.Round;
                    break;
            }
            float centrX = src.Width / 2f;
            float centrY = src.Height / 2f;
            centrX--;
            centrY--;
            //float srcX = centrX - radius - width / 2f;
            //float srcY = centrY - radius - width / 2f;
            //float CircleWidth = 2 * radius + width;
            float srcX = centrX - radius;
            float srcY = centrY - radius;
            float CircleWidth = 2 * radius;
            Bitmap mask = new Bitmap(src.Width, src.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics gPanel = Graphics.FromImage(mask);
            gPanel.SmoothingMode = SmoothingMode.AntiAlias;
            try
            {
                //gPanel.DrawLine(pen, centrX, centrY, centrX + 1, centrY + 1);
                //pen = new Pen(Color.Black, width);
                //pen.Width = width;
                //pen.Color = Color.Black;
                gPanel.DrawArc(pen, srcX, srcY, CircleWidth, CircleWidth, StartAngle, EndAngle);
                //src = ApplyAlfaMask(src, mask);
                src = ApplyMask(src, mask);
                //src = mask;
                graphics.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                //src.Dispose();
                mask.Dispose();

                if (showProgressArea)
                {
                    // подсвечивание шкалы заливкой
                    HatchBrush myHatchBrush = new HatchBrush(HatchStyle.Percent20, Color.White, Color.Transparent);
                    pen.Brush = myHatchBrush;
                    graphics.DrawArc(pen, x+srcX, y+srcY, CircleWidth, CircleWidth, StartAngle, EndAngle);
                    myHatchBrush = new HatchBrush(HatchStyle.Percent10, Color.Black, Color.Transparent);
                    pen.Brush = myHatchBrush;
                    graphics.DrawArc(pen, x+srcX, y+srcY, CircleWidth, CircleWidth, StartAngle, EndAngle);

                    // подсвечивание внешней и внутреней дуги на шкале
                    float w2 = width / 2f;
                    using (Pen pen1 = new Pen(Color.White, 1))
                    {
                        graphics.DrawArc(pen1, x+srcX + w2, y+srcY + w2, CircleWidth - width, CircleWidth - width, StartAngle, EndAngle);
                        graphics.DrawArc(pen1, x+srcX - w2, y+srcY - w2, CircleWidth + width, CircleWidth + width, StartAngle, EndAngle);
                    }
                    using (Pen pen2 = new Pen(Color.Black, 1))
                    {
                        graphics.DrawArc(pen2, x+srcX + w2, y+srcY + w2, CircleWidth - width, CircleWidth - width, StartAngle, EndAngle);
                        graphics.DrawArc(pen2, x+srcX - w2, y+srcY - w2, CircleWidth + width, CircleWidth + width, StartAngle, EndAngle);
                    } 
                }
            }
            catch (Exception)
            {

            }
            src.Dispose();
            Logger.WriteLine("* CircleOnBitmap (end)");

        }

        //public Bitmap ApplyAlfaMask(Bitmap inputImage, Bitmap mask)
        //{
        //    //Resulting collage.
        //    Bitmap result = new Bitmap(inputImage);
        //    for (int x = 0; x < result.Width; x++)
        //    {
        //        for (int y = 0; y < result.Height; y++)
        //        {
        //            Color colorResult = result.GetPixel(x, y);
        //            Color maskResult = mask.GetPixel(x, y);
        //            if (maskResult.A > 80)
        //            {
        //                result.SetPixel(x, y, Color.FromArgb(colorResult.A, colorResult.R, colorResult.G, colorResult.B));
        //            }
        //            else
        //            {
        //                result.SetPixel(x, y, Color.FromArgb(maskResult.A, colorResult.R, colorResult.G, colorResult.B));
        //            }
        //        }
        //    }
        //    inputImage.Dispose();
        //    mask.Dispose();
        //    return result;
        //}

        public Bitmap FormColor(Bitmap bitmap)
        {
            Logger.WriteLine("* FormColor");
            //int[] bgColors = { 203, 255, 240 };
            Color color = pictureBox_Preview.BackColor;
            ImageMagick.MagickImage image = new ImageMagick.MagickImage(bitmap);
            // меняем прозрачный цвет на цвет фона
            image.Opaque(ImageMagick.MagickColor.FromRgba((byte)0, (byte)0, (byte)0, (byte)0),
                ImageMagick.MagickColor.FromRgb(color.R, color.G, color.B));
            // меняем черный цвет на прозрачный
            image.Opaque(ImageMagick.MagickColor.FromRgb((byte)0, (byte)0, (byte)0),
                ImageMagick.MagickColor.FromRgba((byte)0, (byte)0, (byte)0, (byte)0));

            Logger.WriteLine("* FormColor (end)");
            return image.ToBitmap();
        }

        public Bitmap OpenFileStream (string path)
        {
            Bitmap src = null;
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                src = new Bitmap(Image.FromStream(stream));
            }
            return src;
        }
    }
}
