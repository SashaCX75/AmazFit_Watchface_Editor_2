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
        /// <param name="link">0 - основной экран; 1 - AOD</param>
        public void PreviewToBitmap(Graphics gPanel, float scale, bool crop, bool WMesh, bool BMesh, bool BBorder, 
            bool showShortcuts, bool showShortcutsArea, bool showShortcutsBorder, bool showAnimation, bool showProgressArea, 
            bool showCentrHend, int link)
        {
            Logger.WriteLine("* PreviewToBitmap");
            if(link == 1)
            {
                Preview_AOD(gPanel, scale, crop, WMesh, BMesh, BBorder, showShortcuts, showShortcutsArea, 
                    showShortcutsBorder, showAnimation, showProgressArea, showCentrHend);
                return;
            }
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
            //TODO выравнивание даты при слитном написании
            // год
            if (checkBox_Year_text_Use.Checked && comboBox_Year_image.SelectedIndex >= 0)
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

            UserControl_pictures userPanel_pictures;
            UserControl_text userPanel_text;
            UserControl_hand userPanel_hand;
            UserControl_scaleCircle userPanel_scaleCircle;
            UserControl_scaleLinear userPanel_scaleLinear;

            #region зараяд
            userPanel_pictures = userControl_pictures_Battery;
            userPanel_text = userControl_text_Battery;
            userPanel_hand = userControl_hand_Battery;
            userPanel_scaleCircle = userControl_scaleCircle_Battery;
            userPanel_scaleLinear = userControl_scaleLinear_Battery;
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
            userPanel_pictures = userControl_pictures_Steps;
            userPanel_text = userControl_text_Steps;
            userPanel_hand = userControl_hand_Steps;
            userPanel_scaleCircle = userControl_scaleCircle_Steps;
            userPanel_scaleLinear = userControl_scaleLinear_Steps;
            // шаги картинками
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
                    //int offSet = (int)Math.Ceiling((float)count * Watch_Face_Preview_Set.Activity.Steps / Watch_Face_Preview_Set.Activity.StepsGoal);
                    int offSet = (int)((count-1) * Watch_Face_Preview_Set.Activity.Steps / Watch_Face_Preview_Set.Activity.StepsGoal);
                    //offSet--;
                    if (offSet < 0) offSet = 0;
                    if (offSet >= count) offSet = (int)(count - 1);
                    //int offSet = (int)Math.Round(count * Watch_Face_Preview_Set.Battery / 100f, 0);
                    int imageIndex = userPanel_pictures.comboBoxGetSelectedIndexImage() + offSet;
                    if (Watch_Face_Preview_Set.Activity.Steps < Watch_Face_Preview_Set.Activity.StepsGoal / 100f)
                        offSet = -1;

                    if (offSet >= 0 && imageIndex < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[imageIndex]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }
                }
            }

            // шаги круговой шкалой
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
            userPanel_pictures = userControl_pictures_Calories;
            userPanel_text = userControl_text_Calories;
            userPanel_hand = userControl_hand_Calories;
            userPanel_scaleCircle = userControl_scaleCircle_Calories;
            userPanel_scaleLinear = userControl_scaleLinear_Calories;
            // калории картинками
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
                    //int offSet = (int)Math.Ceiling((float)count * Watch_Face_Preview_Set.Activity.Calories / 300f);
                    int offSet = (int)((count - 1f) * Watch_Face_Preview_Set.Activity.Calories / 300f);
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

            // калории круговой шкалой
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
            userPanel_pictures = userControl_pictures_HeartRate;
            userPanel_text = userControl_text_HeartRate;
            userPanel_hand = userControl_hand_HeartRate;
            userPanel_scaleCircle = userControl_scaleCircle_HeartRate;
            userPanel_scaleLinear = userControl_scaleLinear_HeartRate;
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
                float position = (float)Watch_Face_Preview_Set.Activity.HeartRate / 181f;
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
                float position = (float)Watch_Face_Preview_Set.Activity.HeartRate / 181f;
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
                    float position = (Watch_Face_Preview_Set.Activity.HeartRate) / 180f;
                    float angle = (int)(startAngle + position * (endAngle - startAngle));
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
            userPanel_pictures = userControl_pictures_PAI;
            userPanel_text = userControl_text_PAI;
            userPanel_hand = userControl_hand_PAI;
            userPanel_scaleCircle = userControl_scaleCircle_PAI;
            userPanel_scaleLinear = userControl_scaleLinear_PAI;
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
            userPanel_text = userControl_text_Distance;

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
            userPanel_pictures = userControl_pictures_StandUp;
            userPanel_text = userControl_text_StandUp;
            userPanel_hand = userControl_hand_StandUp;
            userPanel_scaleCircle = userControl_scaleCircle_StandUp;
            userPanel_scaleLinear = userControl_scaleLinear_StandUp;
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

            UserControl_pictures_weather userPanel_pictures_weather = userControl_pictures_weather;
            UserControl_text_weather userPanel_text_weather_Current = userControl_text_weather_Current;
            UserControl_text_weather userPanel_text_weather_Min = userControl_text_weather_Min;
            UserControl_text_weather userPanel_text_weather_Max = userControl_text_weather_Max;
            userPanel_hand = userControl_hand_Weather;
            userPanel_scaleCircle = userControl_scaleCircle_Weather;
            userPanel_scaleLinear = userControl_scaleLinear_Weather;
            bool AvailabilityIcon = false;

            // погода картинками
            if (userPanel_pictures_weather.checkBox_pictures_Use.Checked)
            {
                if (userPanel_pictures_weather.comboBoxGetSelectedIndexImage() >= 0)
                {
                    AvailabilityIcon = true;
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
                    if (Watch_Face_Preview_Set.Weather.showTemperature)
                    {
                        Draw_weather_text(gPanel, imageIndex, x, y,
                                        spasing, alignment, value, addZero, imageMinus_index, separator_index, 
                                        BBorder, AvailabilityIcon);
                    }
                    else if (imageError_index >= 0)
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
                    if (Watch_Face_Preview_Set.Weather.showTemperature)
                    {
                        Temperature_offsetX = Draw_weather_text(gPanel, imageIndex, x, y,
                                        spasing, alignment, value, addZero, imageMinus_index, separator_index, 
                                        BBorder, AvailabilityIcon);
                    }
                    else if (imageError_index >= 0)
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

                    if (Watch_Face_Preview_Set.Weather.showTemperature)
                    {
                        Draw_weather_text(gPanel, imageIndex, x, y,
                                        spasing, alignment, value, addZero, imageMinus_index, separator_index, 
                                        BBorder, AvailabilityIcon);
                    }
                    else if (imageError_index >= 0)
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
            userPanel_pictures = userControl_pictures_UVindex;
            userPanel_text = userControl_text_UVindex;
            userPanel_hand = userControl_hand_UVindex;
            userPanel_scaleCircle = userControl_scaleCircle_UVindex;
            userPanel_scaleLinear = userControl_scaleLinear_UVindex;
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
            userPanel_pictures = userControl_pictures_AirQuality;
            userPanel_text = userControl_text_AirQuality;
            userPanel_hand = userControl_hand_AirQuality;
            userPanel_scaleCircle = userControl_scaleCircle_AirQuality;
            userPanel_scaleLinear = userControl_scaleLinear_AirQuality;
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
            userPanel_pictures = userControl_pictures_Humidity;
            userPanel_text = userControl_text_Humidity;
            userPanel_hand = userControl_hand_Humidity;
            userPanel_scaleCircle = userControl_scaleCircle_Humidity;
            userPanel_scaleLinear = userControl_scaleLinear_Humidity;
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
            userPanel_pictures = userControl_pictures_WindForce;
            userPanel_text = userControl_text_WindForce;
            userPanel_hand = userControl_hand_WindForce;
            userPanel_scaleCircle = userControl_scaleCircle_WindForce;
            userPanel_scaleLinear = userControl_scaleLinear_WindForce;
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
            userPanel_pictures = userControl_pictures_Altitude;
            userPanel_text = userControl_text_Altitude;
            userPanel_hand = userControl_hand_Altitude;
            userPanel_scaleCircle = userControl_scaleCircle_Altitude;
            userPanel_scaleLinear = userControl_scaleLinear_Altitude;
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
                    int offSet = (int)((count + 1f) * (Watch_Face_Preview_Set.Weather.Altitude+1000) / 10000f);
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

                    float angle = startAngle + (Watch_Face_Preview_Set.Weather.Altitude+1000) * (endAngle - startAngle) / 10000f;
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
            userPanel_pictures = userControl_pictures_AirPressure;
            userPanel_text = userControl_text_AirPressure;
            userPanel_hand = userControl_hand_AirPressure;
            userPanel_scaleCircle = userControl_scaleCircle_AirPressure;
            userPanel_scaleLinear = userControl_scaleLinear_AirPressure;
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
                    int offSet = (int)((count + 1f) * (Watch_Face_Preview_Set.Weather.AirPressure-200) / 1000f);
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

                    float angle = startAngle + (Watch_Face_Preview_Set.Weather.AirPressure-200)* (endAngle - startAngle) / 1000f;
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
            userPanel_pictures = userControl_pictures_Stress;
            userPanel_text = userControl_text_Stress;
            userPanel_hand = userControl_hand_Stress;
            userPanel_scaleCircle = userControl_scaleCircle_Stress;
            userPanel_scaleLinear = userControl_scaleLinear_Stress;
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
            userPanel_pictures = userControl_pictures_ActivityGoal;
            userPanel_text = userControl_text_ActivityGoal;
            userPanel_hand = userControl_hand_ActivityGoal;
            userPanel_scaleCircle = userControl_scaleCircle_ActivityGoal;
            userPanel_scaleLinear = userControl_scaleLinear_ActivityGoal;
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
            userPanel_pictures = userControl_pictures_FatBurning;
            userPanel_text = userControl_text_FatBurning;
            userPanel_hand = userControl_hand_FatBurning;
            userPanel_scaleCircle = userControl_scaleCircle_FatBurning;
            userPanel_scaleLinear = userControl_scaleLinear_FatBurning;
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
            if (radioButton_TRex_pro.Checked)
            {
                centerX = 180;
                centerY = 180;
            }

            int Hour_X = (int)numericUpDown_Hour_handX.Value;
            int Hour_Y = (int)numericUpDown_Hour_handY.Value;
            int Minute_X = (int)numericUpDown_Minute_handX.Value;
            int Minute_Y = (int)numericUpDown_Minute_handY.Value;
            int Second_X = (int)numericUpDown_Second_handX.Value;
            int Second_Y = (int)numericUpDown_Second_handY.Value;

            if (Hour_X == 0) Hour_X = centerX;
            if (Minute_X == 0) Minute_X = centerX;
            if (Second_X == 0) Second_X = centerX;

            if (Hour_Y == 0) Hour_Y = centerY;
            if (Minute_Y == 0) Minute_Y = centerY;
            if (Second_Y == 0) Second_Y = centerY;

            if ((Hour_X != centerX) || (Hour_Y != centerY)) AnalogClockOffSet = true;
            if ((Minute_X != centerX) || (Minute_Y != centerY)) AnalogClockOffSet = true;
            if ((Second_X != centerX) || (Second_Y != centerY)) AnalogClockOffSet = true;

            if (AnalogClockOffSet)
            {
                if ((Hour_X != centerX || Hour_Y != centerY) && 
                    ((Minute_X != centerX || Minute_Y != centerY))) AnalogClockOffSet = false;
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
            src.Dispose();

            if (crop)
            {
                Logger.WriteLine("PreviewToBitmap (crop)");
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
                case 90:
                    pen.EndCap = LineCap.Triangle;
                    pen.StartCap = LineCap.Triangle;
                    break;
                case 180:
                    pen.EndCap = LineCap.Flat;
                    pen.StartCap = LineCap.Flat;
                    break;
                default:
                    pen.EndCap = LineCap.Round;
                    pen.StartCap = LineCap.Round;
                    break;
            }

            //int srcX = (int)Math.Round(x - radius - width / 2, MidpointRounding.AwayFromZero);
            //int srcY = (int)Math.Round(y - radius - width / 2, MidpointRounding.AwayFromZero);
            int srcX = (int)(x - radius - width / 2);
            int srcY = (int)(y - radius - width / 2);
            int arcX = (int)(x - radius);
            int arcY = (int)(y - radius);
            float CircleWidth = 2 * radius ;

            if (backgroundIndex >= 0 && backgroundIndex < ListImagesFullName.Count)
            {
                src = OpenFileStream(ListImagesFullName[backgroundIndex]);
                graphics.DrawImage(src, new Rectangle(srcX, srcX, src.Width, src.Height));
            }

            try
            {
                //graphics.DrawArc(pen, arcX, arcY, CircleWidth, CircleWidth, startAngle, valueAngle);
                int s = Math.Sign(valueAngle);
                graphics.DrawArc(pen, arcX, arcY, CircleWidth, CircleWidth,
                    (float)(startAngle - 0.007 * s * width), (float)(valueAngle + 0.015 * s * width));
                //TODO исправить отрисовку при большой толщине
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
                int s = Math.Sign(endAngle);
                //graphics.DrawArc(pen, arcX, arcY, CircleWidth, CircleWidth, startAngle, endAngle);
                graphics.DrawArc(pen, arcX, arcY, CircleWidth, CircleWidth,
                    (float)(startAngle - 0.007 * s * width), (float)(endAngle + 0.015 * s * width));
                myHatchBrush = new HatchBrush(HatchStyle.Percent10, Color.Black, Color.Transparent);
                pen.Brush = myHatchBrush;
                //graphics.DrawArc(pen, arcX, arcY, CircleWidth, CircleWidth, startAngle, endAngle);
                graphics.DrawArc(pen, arcX, arcY, CircleWidth, CircleWidth,
                    (float)(startAngle - 0.007 * s * width), (float)(endAngle + 0.015 * s * width));

                // подсвечивание внешней и внутреней дуги на шкале
                using (Pen pen1 = new Pen(Color.White, 1))
                {
                    graphics.DrawArc(pen1, srcX, srcY, CircleWidth + width, CircleWidth + width, startAngle, endAngle);
                    int ArcWidth = (int)(CircleWidth - width);
                    if (ArcWidth < 1) ArcWidth = 1;
                    graphics.DrawArc(pen1, srcX + width, srcY + width, ArcWidth, ArcWidth, startAngle, endAngle);
                }
                using (Pen pen2 = new Pen(Color.Black, 1))
                {
                    graphics.DrawArc(pen2, srcX, srcY, CircleWidth + width, CircleWidth + width, startAngle, endAngle);
                    int ArcWidth = (int)(CircleWidth - width);
                    if (ArcWidth < 1) ArcWidth = 1;
                    graphics.DrawArc(pen2, srcX + width, srcY + width, ArcWidth, ArcWidth, startAngle, endAngle);
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
                case 90:
                    pen.EndCap = LineCap.Triangle;
                    pen.StartCap = LineCap.Triangle;
                    break;
                case 180:
                    pen.EndCap = LineCap.Flat;
                    pen.StartCap = LineCap.Flat;
                    break;
                default:
                    pen.EndCap = LineCap.Round;
                    pen.StartCap = LineCap.Round;
                    break;
            }
            //int srcX = (int)Math.Round(x - radius - width / 2, MidpointRounding.AwayFromZero);
            //int srcY = (int)Math.Round(y - radius - width / 2, MidpointRounding.AwayFromZero);
            int srcX = (int)(x - radius - width / 2);
            int srcY = (int)(y - radius - width / 2);
            int arcX = (int)(x - radius);
            int arcY = (int)(y - radius);
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

                //gPanel.DrawArc(pen, (int)(width / 2f), (int)(width / 2f), CircleWidth, CircleWidth,
                //    startAngle, valueAngle);

                int s = Math.Sign(valueAngle);
                gPanel.DrawArc(pen, (int)(width / 2f), (int)(width / 2f), CircleWidth, CircleWidth,
                    (float)(startAngle - 0.007 * s * width), (float)(valueAngle + 0.015 * s * width));


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
                //graphics.DrawArc(pen, arcX, arcY, CircleWidth, CircleWidth, startAngle, endAngle);
                int s = Math.Sign(endAngle);
                graphics.DrawArc(pen, arcX, arcY, CircleWidth, CircleWidth,
                    (float)(startAngle - 0.007 * s * width), (float)(endAngle + 0.015 * s * width));
                myHatchBrush = new HatchBrush(HatchStyle.Percent10, Color.Black, Color.Transparent);
                pen.Brush = myHatchBrush;
                //graphics.DrawArc(pen, arcX, arcY, CircleWidth, CircleWidth, startAngle, endAngle);
                graphics.DrawArc(pen, arcX, arcY, CircleWidth, CircleWidth,
                    (float)(startAngle - 0.007 * s * width), (float)(endAngle + 0.015 * s * width));

                // подсвечивание внешней и внутреней дуги на шкале
                using (Pen pen1 = new Pen(Color.White, 1))
                {
                    graphics.DrawArc(pen1, srcX, srcY, CircleWidth + width, CircleWidth + width, startAngle, endAngle);
                    int ArcWidth = (int)(CircleWidth - width);
                    if (ArcWidth < 1) ArcWidth = 1;
                    graphics.DrawArc(pen1, srcX + width, srcY + width, ArcWidth, ArcWidth, startAngle, endAngle);
                }
                using (Pen pen2 = new Pen(Color.Black, 1))
                {
                    graphics.DrawArc(pen2, srcX, srcY, CircleWidth + width, CircleWidth + width, startAngle, endAngle);
                    int ArcWidth = (int)(CircleWidth - width);
                    if (ArcWidth < 1) ArcWidth = 1;
                    graphics.DrawArc(pen2, srcX + width, srcY + width, ArcWidth, ArcWidth, startAngle, endAngle);
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
                    x1 = x-1;
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
        /// <param name="lineCap">Тип окончания линии</param>
        /// <param name="pointerIndex">Номер изображения маркера</param>
        /// <param name="backgroundIndex">Номер фонового изображения</param>
        /// <param name="showProgressArea">Подсвечивать шкалу</param>
        private void DrawScaleLinearPointer(Graphics graphics, int x, int y, int length, int width, float position, Color color,
            int lineCap, int pointerIndex, int backgroundIndex, bool showProgressArea)
        {
            Bitmap src = new Bitmap(1, 1);

            if (length > 0)
            {
                int x1 = (int)(x + width / 2f);
                //int x1 = (int)Math.Round(x + width / 2d, MidpointRounding.AwayFromZero);
                int length1 = length - width; 
                if (lineCap == 1 || lineCap == 180)
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
                pen.EndCap = LineCap.Round;
                pen.StartCap = LineCap.Round;
                if (lineCap == 1 || lineCap == 180)
                {
                    pen.EndCap = LineCap.Flat;
                    pen.StartCap = LineCap.Flat;
                }
                graphics.DrawLine(pen, new Point(x1, y1), new Point(x2, y1));

                if (pointerIndex >= 0 && pointerIndex < ListImagesFullName.Count)
                {
                    src = OpenFileStream(ListImagesFullName[pointerIndex]);
                    int x3 = x2 - width / 2+1;
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
                if (lineCap == 1 || lineCap == 180)
                {
                    x1 = x-1;
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

                Pen pen = new Pen(color, width);
                pen.EndCap = LineCap.Round;
                pen.StartCap = LineCap.Round;
                if (lineCap == 1 || lineCap == 180)
                {
                    pen.EndCap = LineCap.Flat;
                    pen.StartCap = LineCap.Flat;
                }
                graphics.DrawLine(pen, new Point(x1, y1), new Point(x2, y1));

                if (pointerIndex >= 0 && pointerIndex < ListImagesFullName.Count)
                {
                    src = OpenFileStream(ListImagesFullName[pointerIndex]);
                    int x3 = x2 - src.Width / 2+1;
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
        /// <param name="lineCap">Тип окончания линии</param>
        /// <param name="imageIndex">Изображение шкалы</param>
        /// <param name="pointerIndex">Номер изображения маркера</param>
        /// <param name="backgroundIndex">Номер фонового изображения</param>
        /// <param name="showProgressArea">Подсвечивать шкалу</param>
        private void DrawScaleLinearPointer_image(Graphics graphics, int x, int y, int length, int width, float position, int imageIndex,
            int lineCap, int pointerIndex, int backgroundIndex, bool showProgressArea)
        {
            Bitmap src = new Bitmap(1, 1);
            //TODO проверить длинну шкалы для плоского и круглого окончания
            if (length > 0)
            {
                int x1 = (int)(x + width / 2f);
                //int x1 = (int)Math.Round(x + width / 2d, MidpointRounding.AwayFromZero);
                int length1 = length - width;
                if (lineCap == 1 || lineCap == 180)
                {
                    x1 = x;
                    length1 = length;
                }
                int position1 = (int)(length1 * position);
                //int position1 = (int)Math.Round(length1 * position, MidpointRounding.AwayFromZero);
                if (position1 <= 0) position1 = 1;
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
                if (lineCap == 1 || lineCap == 180)
                {
                    pen.EndCap = LineCap.Flat;
                    pen.StartCap = LineCap.Flat;
                }

                //graphics.DrawLine(pen, new Point(x1, y1), new Point(x2, y1));

                Bitmap mask = new Bitmap(src.Width, src.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics gPanel = Graphics.FromImage(mask);
                gPanel.SmoothingMode = SmoothingMode.AntiAlias;
                try
                {
                    gPanel.DrawLine(pen, new Point(x1 - x, y1 - y), new Point(x2 - x, y1 - y));
                    if (x1 == x2)
                    {
                        pen = new Pen(Color.FromArgb(1, 0, 0, 0), 1);
                        gPanel.DrawLine(pen, new Point(x1 - x, y1 - y), new Point(x1 - x + 1, y1 - y));
                    }
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
                if (lineCap == 1 || lineCap == 180)
                {
                    x1 = x;
                    length1 = length;
                }
                int position1 = (int)Math.Round(length1 * position);
                if (position1 >= 0) position1 = -1;
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
                if (lineCap == 1 || lineCap == 180)
                {
                    pen.EndCap = LineCap.Flat;
                    pen.StartCap = LineCap.Flat;
                }

                //graphics.DrawLine(pen, new Point(x1, y1), new Point(x2, y1));

                Bitmap mask = new Bitmap(src.Width, src.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics gPanel = Graphics.FromImage(mask);
                gPanel.SmoothingMode = SmoothingMode.AntiAlias;
                try
                {
                    gPanel.DrawLine(pen, new Point(x1 - x - length, y1 - y), new Point(x2 - x - length, y1 - y));
                    //if (x1 == x2)
                    //{
                    //    pen = new Pen(Color.FromArgb(1, 0, 0, 0), 1);
                    //    gPanel.DrawLine(pen, new Point(x1 - x - length, y1 - y), new Point(x1 - x - length - 1, y1 - y));
                    //}
                    //src = ApplyAlfaMask(src, mask);
                    src = ApplyMask(src, mask);

                    graphics.DrawImage(src, new Rectangle(x + length, y, src.Width, src.Height));
                    //graphics.DrawImage(src, new Rectangle(x2 - width / 2, y, src.Width, src.Height));
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
                        graphics.DrawImage(src, new Rectangle(x3, y1 - src.Height / 2, src.Width, src.Height));
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
        /// <param name="ActivityType">Номер активности (при необходимости)</param>
        private int Draw_dagital_text(Graphics graphics, int image_index, int x, int y, int spacing, 
            int alignment, int value, bool addZero, int value_lenght, int separator_index, bool BBorder,
            int ActivityType = 0)
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
            //int DateLenght = width * value_lenght + spacing * (value_lenght - 1);
            if (ActivityType == 17) value_lenght = 5;
            int DateLenght = width * value_lenght + 1;
            if (spacing > 0) DateLenght = DateLenght + spacing * (value_lenght - 1);

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
                        string s = ListImagesFullName[i];
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
            if (addZero)
            {
                while (data_numberS.Length <= value_lenght)
                {
                    data_numberS = "0" + data_numberS;
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
        /// <param name="addZero">Отображать начальные нули</param>
        /// <param name="image_minus_index">Символ "-"</param>
        /// <param name="separator_index">Символ разделителя (единиц измерения)</param>
        /// <param name="BBorder">Рисовать рамку по координатам, вокруг элементов с выравниванием</param>
        /// <param name="AvailabilityIcon">Наличие иконки погоды</param>
        private int Draw_weather_text(Graphics graphics, int image_index, int x, int y, int spacing,
            int alignment, int value, bool addZero, int image_minus_index, int separator_index, bool BBorder, bool AvailabilityIcon)
        {
            int result = 0;
            Logger.WriteLine("* Draw_weather_text");
            var src = new Bitmap(1, 1);
            int _number;
            int i;
            string value_S = value.ToString();
            if (addZero)
            {
                //while (value_S.Length < value_lenght)
                while (value_S.Length < 2)
                {
                    value_S = "0" + value_S;
                }
            }
            char[] CH = value_S.ToCharArray();

            src = OpenFileStream(ListImagesFullName[image_index]);
            int widthD = src.Width;
            int height = src.Height;
            int widthM = 0;
            int widthCF = 0;
            if (image_minus_index >= 0 && image_minus_index < ListImagesFullName.Count)
            {
                src = OpenFileStream(ListImagesFullName[image_minus_index]);
                widthM = src.Width;
            }
            if (separator_index >= 0 && separator_index < ListImagesFullName.Count)
            {
                src = OpenFileStream(ListImagesFullName[separator_index]);
                widthCF = src.Width;
            }

            int DateLenght = widthD * 3 + widthM + widthCF +1;
            if (alignment == 2 && AvailabilityIcon) DateLenght = DateLenght - widthCF;
            if (spacing > 0) DateLenght = DateLenght + 4*spacing;
            if (widthM == 0) DateLenght = DateLenght - spacing;
            if (alignment == 2 && AvailabilityIcon) DateLenght = DateLenght - spacing;

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
                        DateLenghtReal = DateLenghtReal + widthM + spacing;
                    }
                }
            }
            if (separator_index >= 0 && separator_index < ListImagesFullName.Count)
            {
                DateLenghtReal = DateLenghtReal + widthCF + spacing;
            }

            DateLenghtReal = DateLenghtReal - spacing;

            
            //if (radioButton_GTS2.Checked)
            //{
            //    if (image_minus_index >= 0 && image_minus_index < ListImagesFullName.Count)
            //    {
            //        src = OpenFileStream(ListImagesFullName[image_minus_index]);
            //        DateLenght = DateLenght + src.Width + Math.Abs(spacing);
            //    } 
            //}

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
            int centerX = 227;
            int centerY = 227;
            if (radioButton_GTS2.Checked)
            {
                centerX = 174;
                centerY = 221;
            }
            if (radioButton_TRex_pro.Checked)
            {
                centerX = 180;
                centerY = 180;
            }
            if (x == 0) x = centerX;
            if (y == 0) y = centerY;

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
