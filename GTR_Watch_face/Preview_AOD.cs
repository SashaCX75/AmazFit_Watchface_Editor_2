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
            UserControl_text userPanel_textGoal = null;
            UserControl_hand userPanel_hand;
            UserControl_scaleCircle userPanel_scaleCircle;
            UserControl_scaleLinear userPanel_scaleLinear;
            UserControl_SystemFont_Group userControl_SystemFont_Group = null;
            UserControl_icon userControl_icon;

            float activityValue = 0;
            int activityGoal = 0;
            float progress = 0;


            #region зараяд
            userPanel_pictures = userControl_pictures_Battery_AOD;
            userPanel_text = userControl_text_Battery_AOD;
            userPanel_hand = userControl_hand_Battery_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_Battery_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_Battery_AOD;
            userControl_SystemFont_Group = userControl_SystemFont_Group_Battery_AOD;
            userControl_icon = userControl_icon_Battery_AOD;

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

            activityValue = Watch_Face_Preview_Set.Battery;
            activityGoal = 100;
            progress = Watch_Face_Preview_Set.Battery / 100f;
            DrawActivity(gPanel, userPanel_text, userPanel_textGoal, userPanel_hand, userPanel_scaleCircle,
                userPanel_scaleLinear, userControl_SystemFont_Group, userControl_icon, activityValue, 3, activityGoal,
                progress, BBorder, showProgressArea, showCentrHend, "Battery");

            #endregion

            #region шаги
            userPanel_pictures = userControl_pictures_Steps_AOD;
            userPanel_text = userControl_text_Steps_AOD;
            userPanel_textGoal = userControl_text_goal_Steps_AOD;
            userPanel_hand = userControl_hand_Steps_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_Steps_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_Steps_AOD;
            userControl_SystemFont_Group = userControl_SystemFont_Group_Steps_AOD;
            userControl_icon = userControl_icon_Steps_AOD;

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
                    int offSet = (int)((count - 1) * Watch_Face_Preview_Set.Activity.Steps / Watch_Face_Preview_Set.Activity.StepsGoal);
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

            activityValue = Watch_Face_Preview_Set.Activity.Steps;
            activityGoal = Watch_Face_Preview_Set.Activity.StepsGoal;
            progress = (float)Watch_Face_Preview_Set.Activity.Steps / Watch_Face_Preview_Set.Activity.StepsGoal;
            DrawActivity(gPanel, userPanel_text, userPanel_textGoal, userPanel_hand, userPanel_scaleCircle,
                userPanel_scaleLinear, userControl_SystemFont_Group, userControl_icon, activityValue, 5, activityGoal,
                progress, BBorder, showProgressArea, showCentrHend, "Steps");

            userPanel_textGoal = null;
            #endregion

            #region калории
            userPanel_pictures = userControl_pictures_Calories_AOD;
            userPanel_text = userControl_text_Calories_AOD;
            userPanel_textGoal = userControl_text_goal_Calories_AOD;
            userPanel_hand = userControl_hand_Calories_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_Calories_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_Calories_AOD;
            userControl_SystemFont_Group = userControl_SystemFont_Group_Calories_AOD;
            userControl_icon = userControl_icon_Calories_AOD;

            // калории картинками
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

            activityValue = Watch_Face_Preview_Set.Activity.Calories;
            activityGoal = 300;
            progress = Watch_Face_Preview_Set.Activity.Calories / 300f;
            DrawActivity(gPanel, userPanel_text, userPanel_textGoal, userPanel_hand, userPanel_scaleCircle,
                userPanel_scaleLinear, userControl_SystemFont_Group, userControl_icon, activityValue, 4, activityGoal,
                progress, BBorder, showProgressArea, showCentrHend, "Calories");

            userPanel_textGoal = null;
            #endregion

            #region пульс
            userPanel_pictures = userControl_pictures_HeartRate_AOD;
            userPanel_text = userControl_text_HeartRate_AOD;
            userPanel_hand = userControl_hand_HeartRate_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_HeartRate_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_HeartRate_AOD;
            userControl_SystemFont_Group = userControl_SystemFont_Group_HeartRate_AOD;
            userControl_icon = userControl_icon_HeartRate_AOD;

            // пульс картинками
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

            activityValue = Watch_Face_Preview_Set.Activity.HeartRate;
            activityGoal = 180;
            progress = Watch_Face_Preview_Set.Activity.HeartRate / 181f;
            DrawActivity(gPanel, userPanel_text, userPanel_textGoal, userPanel_hand, userPanel_scaleCircle,
                userPanel_scaleLinear, userControl_SystemFont_Group, userControl_icon, activityValue, 3, activityGoal,
                progress, BBorder, showProgressArea, showCentrHend, "HeartRate");
            #endregion

            #region PAI
            userPanel_pictures = userControl_pictures_PAI_AOD;
            userPanel_text = userControl_text_PAI_AOD;
            userPanel_hand = userControl_hand_PAI_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_PAI_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_PAI_AOD;
            userControl_SystemFont_Group = userControl_SystemFont_Group_PAI_AOD;
            userControl_icon = userControl_icon_PAI_AOD;

            // PAI картинками
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

            activityValue = Watch_Face_Preview_Set.Activity.PAI;
            activityGoal = 100;
            progress = Watch_Face_Preview_Set.Activity.PAI / 100f;
            DrawActivity(gPanel, userPanel_text, userPanel_textGoal, userPanel_hand, userPanel_scaleCircle,
                userPanel_scaleLinear, userControl_SystemFont_Group, userControl_icon, activityValue, 3, activityGoal,
                progress, BBorder, showProgressArea, showCentrHend, "PAI");

            #endregion

            #region путь
            userPanel_pictures = null;
            userPanel_text = userControl_text_Distance_AOD;
            userPanel_hand = null;
            userPanel_scaleCircle = null;
            userPanel_scaleLinear = null;
            userControl_SystemFont_Group = userControl_SystemFont_Group_Distance_AOD;
            userControl_icon = userControl_icon_Distance_AOD;

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

            userPanel_text = null;
            activityValue = Watch_Face_Preview_Set.Activity.Distance / 1000f;
            activityValue = (float)Math.Round(activityValue, 2, MidpointRounding.AwayFromZero);
            activityGoal = 10;
            progress = Watch_Face_Preview_Set.Activity.Distance / 1000f;
            DrawActivity(gPanel, userPanel_text, userPanel_textGoal, userPanel_hand, userPanel_scaleCircle,
                userPanel_scaleLinear, userControl_SystemFont_Group, userControl_icon, activityValue, 5, activityGoal,
                progress, BBorder, showProgressArea, showCentrHend, "Distance");

            userPanel_textGoal = null;

            #endregion

            #region StandUp
            userPanel_pictures = userControl_pictures_StandUp_AOD;
            userPanel_text = userControl_text_StandUp_AOD;
            userPanel_textGoal = userControl_text_goal_StandUp_AOD;
            userPanel_hand = userControl_hand_StandUp_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_StandUp_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_StandUp_AOD;
            userControl_SystemFont_Group = userControl_SystemFont_Group_StandUp_AOD;
            userControl_icon = userControl_icon_StandUp_AOD;

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

            activityValue = Watch_Face_Preview_Set.Activity.StandUp;
            activityGoal = 12;
            progress = Watch_Face_Preview_Set.Activity.StandUp / 12f;
            DrawActivity(gPanel, userPanel_text, userPanel_textGoal, userPanel_hand, userPanel_scaleCircle,
                userPanel_scaleLinear, userControl_SystemFont_Group, userControl_icon, activityValue, 2, activityGoal,
                progress, BBorder, showProgressArea, showCentrHend, "StandUp");

            userPanel_textGoal = null;
            #endregion

            #region погода
            UserControl_pictures_weather userPanel_pictures_weather = userControl_pictures_weather_AOD;
            UserControl_text_weather userPanel_text_weather_Current = userControl_text_weather_Current_AOD;
            UserControl_text_weather userPanel_text_weather_Min = userControl_text_weather_Min_AOD;
            UserControl_text_weather userPanel_text_weather_Max = userControl_text_weather_Max_AOD;
            userPanel_hand = userControl_hand_Weather_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_Weather_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_Weather_AOD;
            userControl_icon = userControl_icon_Weather_AOD;
            int value_current = Watch_Face_Preview_Set.Weather.Temperature;
            int value_min = Watch_Face_Preview_Set.Weather.TemperatureMin;
            int value_max = Watch_Face_Preview_Set.Weather.TemperatureMax;
            int icon_index = Watch_Face_Preview_Set.Weather.Icon;
            bool showTemperature = Watch_Face_Preview_Set.Weather.showTemperature;

            DrawWeather(gPanel, userPanel_pictures_weather, userPanel_text_weather_Current, userPanel_text_weather_Min,
                userPanel_text_weather_Max, userControl_SystemFont_GroupWeather, userControl_icon, value_current,
                value_min, value_max, icon_index, BBorder, showTemperature);

            #endregion

            #region UVindex
            userPanel_pictures = userControl_pictures_UVindex_AOD;
            userPanel_text = userControl_text_UVindex_AOD;
            userPanel_hand = userControl_hand_UVindex_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_UVindex_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_UVindex_AOD;
            userControl_SystemFont_Group = userControl_SystemFont_Group_UVindex_AOD;
            userControl_icon = userControl_icon_UVindex_AOD;

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

            activityValue = Watch_Face_Preview_Set.Weather.UVindex;
            activityGoal = 10;
            progress = Watch_Face_Preview_Set.Weather.UVindex / 10f;
            DrawActivity(gPanel, userPanel_text, userPanel_textGoal, userPanel_hand, userPanel_scaleCircle,
                userPanel_scaleLinear, userControl_SystemFont_Group, userControl_icon, activityValue, 2, activityGoal,
                progress, BBorder, showProgressArea, showCentrHend, "UVindex");

            #endregion

            #region AirQuality
            userPanel_pictures = userControl_pictures_AirQuality_AOD;
            userPanel_text = userControl_text_AirQuality_AOD;
            userPanel_hand = userControl_hand_AirQuality_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_AirQuality_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_AirQuality_AOD;
            userControl_SystemFont_Group = userControl_SystemFont_Group_AirQuality_AOD;
            userControl_icon = userControl_icon_AirQuality_AOD;

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

            activityValue = Watch_Face_Preview_Set.Weather.AirQuality;
            activityGoal = 500;
            progress = Watch_Face_Preview_Set.Weather.AirQuality / 503f;
            DrawActivity(gPanel, userPanel_text, userPanel_textGoal, userPanel_hand, userPanel_scaleCircle,
                userPanel_scaleLinear, userControl_SystemFont_Group, userControl_icon, activityValue, 3, activityGoal,
                progress, BBorder, showProgressArea, showCentrHend, "AirQuality");

            #endregion

            #region Humidity
            userPanel_pictures = userControl_pictures_Humidity_AOD;
            userPanel_text = userControl_text_Humidity_AOD;
            userPanel_hand = userControl_hand_Humidity_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_Humidity_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_Humidity_AOD;
            userControl_SystemFont_Group = userControl_SystemFont_Group_Humidity_AOD;
            userControl_icon = userControl_icon_Humidity_AOD;

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

            activityValue = Watch_Face_Preview_Set.Weather.Humidity;
            activityGoal = 100;
            progress = Watch_Face_Preview_Set.Weather.Humidity / 100f;
            DrawActivity(gPanel, userPanel_text, userPanel_textGoal, userPanel_hand, userPanel_scaleCircle,
                userPanel_scaleLinear, userControl_SystemFont_Group, userControl_icon, activityValue, 2, activityGoal,
                progress, BBorder, showProgressArea, showCentrHend, "Humidity");

            #endregion

            #region Sunrise
            userPanel_pictures = userControl_pictures_Sunrise_AOD;
            userPanel_text = userControl_text_SunriseSunset_AOD;
            UserControl_text userPanel_text_Sunrise_Min = userControl_text_Sunrise_AOD;
            UserControl_text userPanel_text_Sunrise_Max = userControl_text_Sunset_AOD;
            userPanel_hand = userControl_hand_Sunrise_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_Sunrise_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_Sunrise_AOD;
            UserControl_SystemFont_GroupSunrise userControl_SystemFont_Group_Sunrise
                = userControl_SystemFont_GroupSunrise_AOD;
            userControl_icon = userControl_icon_Sunrise_AOD;

            int hours = Watch_Face_Preview_Set.Time.Hours;
            int minutes = Watch_Face_Preview_Set.Time.Minutes;

            DrawSunrise(gPanel, userPanel_pictures, userPanel_text, userPanel_text_Sunrise_Min,
                userPanel_text_Sunrise_Max, userPanel_hand, userPanel_scaleCircle, userPanel_scaleLinear,
                userControl_SystemFont_Group_Sunrise, userControl_icon, hours, minutes, BBorder, showProgressArea,
                showCentrHend);

            #endregion

            #region WindForce
            userPanel_pictures = userControl_pictures_WindForce_AOD;
            userPanel_text = userControl_text_WindForce_AOD;
            userPanel_hand = userControl_hand_WindForce_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_WindForce_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_WindForce_AOD;
            userControl_SystemFont_Group = userControl_SystemFont_Group_WindForce_AOD;
            userControl_icon = userControl_icon_WindForce_AOD;

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

            activityValue = Watch_Face_Preview_Set.Weather.WindForce;
            activityGoal = 12;
            progress = Watch_Face_Preview_Set.Weather.WindForce / 12f;
            DrawActivity(gPanel, userPanel_text, userPanel_textGoal, userPanel_hand, userPanel_scaleCircle,
                userPanel_scaleLinear, userControl_SystemFont_Group, userControl_icon, activityValue, 2, activityGoal,
                progress, BBorder, showProgressArea, showCentrHend, "WindForce");

            #endregion

            #region Altitude
            userPanel_pictures = userControl_pictures_Altitude_AOD;
            userPanel_text = userControl_text_Altitude_AOD;
            userPanel_hand = userControl_hand_Altitude_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_Altitude_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_Altitude_AOD;
            userControl_SystemFont_Group = userControl_SystemFont_Group_Altitude_AOD;
            userControl_icon = userControl_icon_Altitude_AOD;

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

            activityValue = Watch_Face_Preview_Set.Weather.Altitude;
            activityGoal = 9000;
            progress = (Watch_Face_Preview_Set.Weather.Altitude + 1000) / 10000f;
            DrawActivity(gPanel, userPanel_text, userPanel_textGoal, userPanel_hand, userPanel_scaleCircle,
                userPanel_scaleLinear, userControl_SystemFont_Group, userControl_icon, activityValue, 4, activityGoal,
                progress, BBorder, showProgressArea, showCentrHend, "Altitude");

            #endregion

            #region AirPressure
            userPanel_pictures = userControl_pictures_AirPressure_AOD;
            userPanel_text = userControl_text_AirPressure_AOD;
            userPanel_hand = userControl_hand_AirPressure_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_AirPressure_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_AirPressure_AOD;
            userControl_SystemFont_Group = userControl_SystemFont_Group_AirPressure_AOD;
            userControl_icon = userControl_icon_AirPressure_AOD;

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

            activityValue = Watch_Face_Preview_Set.Weather.AirPressure;
            activityGoal = 1200;
            progress = (Watch_Face_Preview_Set.Weather.AirPressure - 170) / 1000f;
            DrawActivity(gPanel, userPanel_text, userPanel_textGoal, userPanel_hand, userPanel_scaleCircle,
                userPanel_scaleLinear, userControl_SystemFont_Group, userControl_icon, activityValue, 4, activityGoal,
                progress, BBorder, showProgressArea, showCentrHend, "AirPressure");

            #endregion

            #region Stress
            userPanel_pictures = userControl_pictures_Stress_AOD;
            userPanel_text = userControl_text_Stress_AOD;
            userPanel_hand = userControl_hand_Stress_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_Stress_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_Stress_AOD;
            userControl_SystemFont_Group = userControl_SystemFont_Group_Stress_AOD;
            userControl_icon = userControl_icon_Stress_AOD;

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

            activityValue = Watch_Face_Preview_Set.Activity.Stress;
            activityGoal = 12;
            progress = Watch_Face_Preview_Set.Activity.Stress / 12f;
            DrawActivity(gPanel, userPanel_text, userPanel_textGoal, userPanel_hand, userPanel_scaleCircle,
                userPanel_scaleLinear, userControl_SystemFont_Group, userControl_icon, activityValue, 2, activityGoal,
                progress, BBorder, showProgressArea, showCentrHend, "Stress");

            #endregion

            #region ActivityGoal
            userPanel_pictures = userControl_pictures_ActivityGoal_AOD;
            userPanel_text = userControl_text_ActivityGoal_AOD;
            userPanel_textGoal = userControl_text_goal_ActivityGoal_AOD;
            userPanel_hand = userControl_hand_ActivityGoal_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_ActivityGoal_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_ActivityGoal_AOD;
            userControl_SystemFont_Group = userControl_SystemFont_Group_ActivityGoal_AOD;
            userControl_icon = userControl_icon_ActivityGoal_AOD;

            bool ActivityGoal_Calories = false;
            if (radioButton_ScreenNormal.Checked && radioButton_ActivityGoal_Calories.Checked) ActivityGoal_Calories = true;
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

                    int offSet = (int)((count - 1) * Watch_Face_Preview_Set.Activity.Steps / Watch_Face_Preview_Set.Activity.StepsGoal);
                    if (offSet < 0) offSet = 0;
                    if (offSet >= count) offSet = (int)(count - 1);
                    int imageIndex = userPanel_pictures.comboBoxGetSelectedIndexImage() + offSet;
                    if (Watch_Face_Preview_Set.Activity.Steps < Watch_Face_Preview_Set.Activity.StepsGoal / 100f)
                        offSet = -1;
                    if (ActivityGoal_Calories)
                    {
                        offSet = (int)((count - 1f) * Watch_Face_Preview_Set.Activity.Calories / 300f);
                        if (offSet < 0) offSet = 0;
                        if (offSet >= count) offSet = (int)(count - 1);
                        imageIndex = userPanel_pictures.comboBoxGetSelectedIndexImage() + offSet;
                    }

                    if (offSet >= 0 && imageIndex < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[imageIndex]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }
                }
            }

            activityValue = Watch_Face_Preview_Set.Activity.Steps;
            activityGoal = Watch_Face_Preview_Set.Activity.StepsGoal;
            progress = (float)Watch_Face_Preview_Set.Activity.Steps / Watch_Face_Preview_Set.Activity.StepsGoal;
            if (ActivityGoal_Calories)
            {
                activityValue = Watch_Face_Preview_Set.Activity.Calories;
                activityGoal = 300;
                progress = Watch_Face_Preview_Set.Activity.Calories / 300f;
            }
            DrawActivity(gPanel, userPanel_text, userPanel_textGoal, userPanel_hand, userPanel_scaleCircle,
                userPanel_scaleLinear, userControl_SystemFont_Group, userControl_icon, activityValue, 4, activityGoal,
                progress, BBorder, showProgressArea, showCentrHend, "ActivityGoal", ActivityGoal_Calories);

            userPanel_textGoal = null;
            #endregion

            #region FatBurning
            userPanel_pictures = userControl_pictures_FatBurning_AOD;
            userPanel_text = userControl_text_FatBurning_AOD;
            userPanel_textGoal = userControl_text_goal_FatBurning_AOD;
            userPanel_hand = userControl_hand_FatBurning_AOD;
            userPanel_scaleCircle = userControl_scaleCircle_FatBurning_AOD;
            userPanel_scaleLinear = userControl_scaleLinear_FatBurning_AOD;
            userControl_SystemFont_Group = userControl_SystemFont_Group_FatBurning_AOD;
            userControl_icon = userControl_icon_FatBurning_AOD;

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

            activityValue = Watch_Face_Preview_Set.Activity.FatBurning;
            activityGoal = 30;
            progress = Watch_Face_Preview_Set.Activity.FatBurning / 30f;
            DrawActivity(gPanel, userPanel_text, userPanel_textGoal, userPanel_hand, userPanel_scaleCircle,
                userPanel_scaleLinear, userControl_SystemFont_Group, userControl_icon, activityValue, 2, activityGoal,
                progress, BBorder, showProgressArea, showCentrHend, "FatBurning");

            userPanel_textGoal = null;
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
            bool ShowHour = true;
            if((checkBox_Hour_Use.Checked && comboBox_Hour_image.SelectedIndex >= 0) &&
                (checkBox_Minute_Use.Checked && comboBox_Minute_image.SelectedIndex >= 0) &&
                (checkBox_Second_Use.Checked && comboBox_Second_image.SelectedIndex >= 0)) ShowHour = false;
            // часы
            if (checkBox_Hour_hand_Use.Checked && comboBox_Hour_hand_image.SelectedIndex >= 0 && ShowHour)
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
