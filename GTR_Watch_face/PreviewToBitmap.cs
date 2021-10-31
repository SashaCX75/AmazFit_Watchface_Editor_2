using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
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
        /// <param name="showWidgetsArea">Подсвечивать область виджетов</param>
        /// <param name="link">0 - основной экран; 1 - AOD</param>
        public void PreviewToBitmap(Graphics gPanel, float scale, bool crop, bool WMesh, bool BMesh, bool BBorder, 
            bool showShortcuts, bool showShortcutsArea, bool showShortcutsBorder, bool showAnimation, bool showProgressArea, 
            bool showCentrHend, bool showWidgetsArea, int link)
        {

            //if (tabControl1.SelectedTab.Name == "tabPage_Widgets" && radioButton_WidgetPreviewEdit.Checked)
            //{
            //    DrawWidgetEditScreen(gPanel, showWidgetsArea);
            //    return;
            //}

            if (tabControl1.SelectedTab.Name == "tabPage_Widgets")
            {
                if (tabControl_Widget.SelectedTab.Name == "tabPage_WidgetsEdit" && radioButton_WidgetPreviewEdit.Checked)
                {
                    int widgetIndex = comboBox_WidgetNumber.SelectedIndex;
                    DrawWidgetEditScreen(gPanel, crop, showWidgetsArea, widgetIndex);
                    FormText();
                    goto TimeEnd;
                    return;
                }
                if (tabControl_Widget.SelectedTab.Name == "tabPage_WidgetAdd" && radioButton_WidgetAdd.Checked)
                {
                    int widgetIndex = comboBox_WidgetNumber.SelectedIndex;
                    DrawWidgetEditScreen(gPanel, crop, showWidgetsArea, widgetIndex);
                    FormText();
                    goto TimeEnd;
                    return;
                }
            }

            Logger.WriteLine("* PreviewToBitmap");
            if(link == 1)
            {
                Preview_AOD(gPanel, scale, crop, WMesh, BMesh, BBorder, showShortcuts, showShortcutsArea, 
                    showShortcutsBorder, showAnimation, showProgressArea, showCentrHend);
                return;
            }
            Bitmap src = new Bitmap(1, 1);
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
            if (radioButton_ZeppE.Checked)
            {
                src = OpenFileStream(Application.StartupPath + @"\Mask\mask_zepp_e.png");
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

            DrawWidgets(gPanel, scale, crop, WMesh, BMesh, BBorder, showShortcuts, showShortcutsArea,
                    showShortcutsBorder, showAnimation, showProgressArea, showCentrHend, showWidgetsArea);

            #region дата 

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

            int date_offsetX = -1;
            int date_offsetY = -1;
            int spasing_offset = 0;

            string sValue_firstSF = "";
            string sValue_secondSF = "";
            string sValue_thirdSF = "";
            bool follow_secondSF = false;
            bool follow_thirdSF = false;

            string sValue_firstFR = "";
            string sValue_secondFR = "";
            string sValue_thirdFR = "";
            bool follow_secondFR = false;
            bool follow_thirdFR = false;

            // формируем надписи
            for (int j = 0; j < dataGridView_SNL_Date.RowCount; j++)
            {
                string dateName = dataGridView_SNL_Date.Rows[j].Cells[0].Value.ToString();

                // год
                if (dateName == "Year")
                {
                    UserControl_SystemFont userControl_SystemFont =
                           userControl_SystemFont_Group_Year.userControl_SystemFont;
                    UserControl_FontRotate userControl_FontRotate =
                           userControl_SystemFont_Group_Year.userControl_FontRotate;

                    if (userControl_SystemFont.checkBox_Use.Checked)
                    {
                        int unitCheck = userControl_SystemFont.checkBoxGetUnit();
                        bool addZero = userControl_SystemFont.checkBox_addZero.Checked;
                        bool separator = userControl_SystemFont.checkBox_separator.Checked;
                        int value = Watch_Face_Preview_Set.Date.Year;
                        if (addZero) value = Watch_Face_Preview_Set.Date.Year % 100;
                        string sValue = value.ToString();
                        sValue = sValue + UnitName("Date", unitCheck);
                        if (separator) sValue = sValue + "/";
                        switch (j)
                        {
                            case 0:
                                sValue_firstSF = sValue;
                                break;
                            case 1:
                                sValue_secondSF = sValue;
                                follow_secondSF = userControl_SystemFont.checkBox_follow.Checked;
                                break;
                            case 2:
                                sValue_thirdSF = sValue;
                                follow_thirdSF = userControl_SystemFont.checkBox_follow.Checked;
                                break;
                        }
                    }

                    if (userControl_FontRotate.checkBox_Use.Checked)
                    {
                        int unitCheck = userControl_FontRotate.checkBoxGetUnit();
                        bool addZero = userControl_FontRotate.checkBox_addZero.Checked;
                        bool separator = userControl_FontRotate.checkBox_separator.Checked;
                        int value = Watch_Face_Preview_Set.Date.Year;
                        if (addZero) value = Watch_Face_Preview_Set.Date.Year % 100;
                        string sValue = value.ToString();
                        sValue = sValue + UnitName("Date", unitCheck);
                        if (separator) sValue = sValue + "/";
                        switch (j)
                        {
                            case 0:
                                sValue_firstFR = sValue;
                                break;
                            case 1:
                                sValue_secondFR = sValue;
                                follow_secondFR = userControl_FontRotate.checkBox_follow.Checked;
                                break;
                            case 2:
                                sValue_thirdFR = sValue;
                                follow_thirdFR = userControl_FontRotate.checkBox_follow.Checked;
                                break;
                        }
                    }

                }

                // месяц
                if (dateName == "Month")
                {
                    UserControl_SystemFont userControl_SystemFont =
                           userControl_SystemFont_Group_Month.userControl_SystemFont;
                    UserControl_FontRotate userControl_FontRotate =
                           userControl_SystemFont_Group_Month.userControl_FontRotate;

                    if (userControl_SystemFont.checkBox_Use.Checked)
                    {
                        int unitCheck = userControl_SystemFont.checkBoxGetUnit();
                        bool addZero = userControl_SystemFont.checkBox_addZero.Checked;
                        bool separator = userControl_SystemFont.checkBox_separator.Checked;
                        int value = Watch_Face_Preview_Set.Date.Month;
                        string sValue = value.ToString();
                        if (addZero)
                        {
                            while (sValue.Length < 2)
                            {
                                sValue = "0" + sValue;
                            }
                        }
                        sValue = sValue + UnitName("Date", unitCheck);
                        if (separator) sValue = sValue + "/";
                        switch (j)
                        {
                            case 0:
                                sValue_firstSF = sValue;
                                break;
                            case 1:
                                sValue_secondSF = sValue;
                                follow_secondSF = userControl_SystemFont.checkBox_follow.Checked;
                                break;
                            case 2:
                                sValue_thirdSF = sValue;
                                follow_thirdSF = userControl_SystemFont.checkBox_follow.Checked;
                                break;
                        }
                    }

                    if (userControl_FontRotate.checkBox_Use.Checked)
                    {
                        int unitCheck = userControl_FontRotate.checkBoxGetUnit();
                        bool addZero = userControl_FontRotate.checkBox_addZero.Checked;
                        bool separator = userControl_FontRotate.checkBox_separator.Checked;
                        int value = Watch_Face_Preview_Set.Date.Month;
                        string sValue = value.ToString();
                        if (addZero)
                        {
                            while (sValue.Length < 2)
                            {
                                sValue = "0" + sValue;
                            }
                        }
                        sValue = sValue + UnitName("Date", unitCheck);
                        if (separator) sValue = sValue + "/";
                        switch (j)
                        {
                            case 0:
                                sValue_firstFR = sValue;
                                break;
                            case 1:
                                sValue_secondFR = sValue;
                                follow_secondFR = userControl_FontRotate.checkBox_follow.Checked;
                                break;
                            case 2:
                                sValue_thirdFR = sValue;
                                follow_thirdFR = userControl_FontRotate.checkBox_follow.Checked;
                                break;
                        }
                    }
                }

                // число
                if (dateName == "Day")
                {
                    UserControl_SystemFont userControl_SystemFont =
                           userControl_SystemFont_Group_Day.userControl_SystemFont;
                    UserControl_FontRotate userControl_FontRotate =
                           userControl_SystemFont_Group_Day.userControl_FontRotate;

                    if (userControl_SystemFont.checkBox_Use.Checked)
                    {
                        int unitCheck = userControl_SystemFont.checkBoxGetUnit();
                        bool addZero = userControl_SystemFont.checkBox_addZero.Checked;
                        bool separator = userControl_SystemFont.checkBox_separator.Checked;
                        int value = Watch_Face_Preview_Set.Date.Day;
                        string sValue = value.ToString();
                        if (addZero)
                        {
                            while (sValue.Length < 2)
                            {
                                sValue = "0" + sValue;
                            }
                        }
                        sValue = sValue + UnitName("Date", unitCheck);
                        if (separator) sValue = sValue + "/";
                        switch (j)
                        {
                            case 0:
                                sValue_firstSF = sValue;
                                break;
                            case 1:
                                sValue_secondSF = sValue;
                                follow_secondSF = userControl_SystemFont.checkBox_follow.Checked;
                                break;
                            case 2:
                                sValue_thirdSF = sValue;
                                follow_thirdSF = userControl_SystemFont.checkBox_follow.Checked;
                                break;
                        }
                    }

                    if (userControl_FontRotate.checkBox_Use.Checked)
                    {
                        int unitCheck = userControl_FontRotate.checkBoxGetUnit();
                        bool addZero = userControl_FontRotate.checkBox_addZero.Checked;
                        bool separator = userControl_FontRotate.checkBox_separator.Checked;
                        int value = Watch_Face_Preview_Set.Date.Day;
                        string sValue = value.ToString();
                        if (addZero)
                        {
                            while (sValue.Length < 2)
                            {
                                sValue = "0" + sValue;
                            }
                        }
                        sValue = sValue + UnitName("Date", unitCheck);
                        if (separator) sValue = sValue + "/";
                        switch (j)
                        {
                            case 0:
                                sValue_firstFR = sValue;
                                break;
                            case 1:
                                sValue_secondFR = sValue;
                                follow_secondFR = userControl_FontRotate.checkBox_follow.Checked;
                                break;
                            case 2:
                                sValue_thirdFR = sValue;
                                follow_thirdFR = userControl_FontRotate.checkBox_follow.Checked;
                                break;
                        }
                    }
                }

            }

            if (follow_thirdSF)
            {
                sValue_secondSF = sValue_secondSF + sValue_thirdSF;
                sValue_thirdSF = "";
            }
            else sValue_thirdSF = sValue_firstSF + sValue_secondSF + sValue_thirdSF;
            if (follow_secondSF)
            {
                sValue_firstSF = sValue_firstSF + sValue_secondSF;
                sValue_secondSF = sValue_thirdSF;
                sValue_thirdSF = "";
            }
            else sValue_secondSF = sValue_firstSF + sValue_secondSF;

            if (follow_thirdFR)
            {
                sValue_secondFR = sValue_secondFR + sValue_thirdFR;
                sValue_thirdFR = "";
            }
            else sValue_thirdFR = sValue_firstFR + sValue_secondFR + sValue_thirdFR;
            if (follow_secondFR)
            {
                sValue_firstFR = sValue_firstFR + sValue_secondFR;
                sValue_secondFR = sValue_thirdFR;
                sValue_thirdFR = "";
            }
            else sValue_secondFR = sValue_firstFR + sValue_secondFR;

            for (int j = 0; j < dataGridView_SNL_Date.RowCount; j++)
            {
                string dateName = dataGridView_SNL_Date.Rows[j].Cells[0].Value.ToString();

                // год
                if (dateName == "Year")
                {
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
                        if (checkBox_Year_follow.Checked && date_offsetX >= 0)
                        {
                            x = date_offsetX;
                            alignment = 0;
                            y = date_offsetY;
                            spasing = spasing_offset;
                        }
                        addZero = false;

                        date_offsetY = y;
                        spasing_offset = spasing;
                        date_offsetX = Draw_dagital_text(gPanel, imageIndex, x, y,
                            spasing, alignment, value, addZero, 4, separator_index, BBorder);

                        if (comboBox_Year_unit.SelectedIndex >= 0)
                        {
                            src = OpenFileStream(ListImagesFullName[comboBox_Year_unit.SelectedIndex]);
                            gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Year_unitX.Value,
                                (int)numericUpDown_Year_unitY.Value, src.Width, src.Height));
                        }
                    }

                    // надпись системным шрифтом
                    UserControl_SystemFont userControl_SystemFont =
                        userControl_SystemFont_Group_Year.userControl_SystemFont;
                    if (userControl_SystemFont != null && userControl_SystemFont.checkBox_Use.Checked)
                    {
                        NumericUpDown numericUpDownX = userControl_SystemFont.numericUpDown_SystemFontX;
                        NumericUpDown numericUpDownY = userControl_SystemFont.numericUpDown_SystemFontY;
                        NumericUpDown numericUpDown_size = userControl_SystemFont.numericUpDown_SystemFont_size;
                        NumericUpDown numericUpDown_angle = userControl_SystemFont.numericUpDown_SystemFont_angle;
                        NumericUpDown numericUpDown_spacing = userControl_SystemFont.numericUpDown_SystemFont_spacing;
                        CheckBox checkBox_follow = userControl_SystemFont.checkBox_follow;

                        //int imageIndex = comboBox_image.SelectedIndex;
                        int x = (int)numericUpDownX.Value;
                        int y = (int)numericUpDownY.Value;
                        int size = (int)numericUpDown_size.Value;
                        int angle = (int)numericUpDown_angle.Value;
                        int spasing = (int)numericUpDown_spacing.Value;
                        string sValue = "";
                        switch (j)
                        {
                            case 0:
                                sValue = sValue_firstSF;
                                break;
                            case 1:
                                sValue = sValue_secondSF;
                                break;
                            case 2:
                                sValue = sValue_thirdSF;
                                break;
                        }
                        Color color = userControl_SystemFont.comboBoxGetColor();

                        //if (checkBox_follow.Checked && date_offsetX_sf>=0)
                        //{
                        //    x = date_offsetX_sf;
                        //    y = date_offsetY_sf;
                        //    spasing = spasing_offset_sf;
                        //}

                        //date_offsetY_sf = y;
                        //spasing_offset_sf = spasing;
                        Draw_text(gPanel, x, y, size, spasing, color, angle, sValue, BBorder);
                    }


                    // надпись системным шрифтом по окружности
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
                        CheckBox checkBox_follow = userControl_FontRotate.checkBox_follow;

                        //int imageIndex = comboBox_image.SelectedIndex;
                        int x = (int)numericUpDownX.Value;
                        int y = (int)numericUpDownY.Value;
                        int size = (int)numericUpDown_size.Value;
                        int angle = (int)numericUpDown_angle.Value;
                        int radius = (int)numericUpDown_radius.Value;
                        int spasing = (int)numericUpDown_spacing.Value;
                        int rotate_direction = userControl_FontRotate.radioButtonGetRotateDirection();
                        string sValue = "";
                        switch (j)
                        {
                            case 0:
                                sValue = sValue_firstFR;
                                break;
                            case 1:
                                sValue = sValue_secondFR;
                                break;
                            case 2:
                                sValue = sValue_thirdFR;
                                break;
                        }
                        Color color = userControl_FontRotate.comboBoxGetColor();

                        //if (checkBox_follow.Checked && date_offsetX_sfr >= 0)
                        //{
                        //    x = date_offsetX_sfr;
                        //    y = date_offsetY_sfr;
                        //    spasing = spasing_offset_sfr;
                        //    angle = (int)date_offsetAngle_sfr;
                        //}

                        //date_offsetX_sfr = x;
                        //date_offsetY_sfr = y;
                        //spasing_offset_sfr = spasing;
                        Draw_text_rotate(gPanel, x, y, radius, size, spasing, color, angle, rotate_direction,
                            sValue, BBorder);
                    }
                }

                // месяц
                if (dateName == "Month")
                {
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

                    // месяц надписью
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
                        if (checkBox_Month_follow.Checked && date_offsetX >= 0)
                        {
                            x = date_offsetX;
                            alignment = 0;
                            y = date_offsetY;
                            spasing = spasing_offset;
                        }
                        date_offsetY = y;
                        spasing_offset = spasing;
                        date_offsetX = Draw_dagital_text(gPanel, imageIndex, x, y,
                            spasing, alignment, value, addZero, 2, separator_index, BBorder);

                        if (comboBox_Month_unit.SelectedIndex >= 0)
                        {
                            src = OpenFileStream(ListImagesFullName[comboBox_Month_unit.SelectedIndex]);
                            gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Month_unitX.Value,
                                (int)numericUpDown_Month_unitY.Value, src.Width, src.Height));
                        }
                    }

                    // надпись системным шрифтом
                    UserControl_SystemFont userControl_SystemFont =
                        userControl_SystemFont_Group_Month.userControl_SystemFont;
                    if (userControl_SystemFont != null && userControl_SystemFont.checkBox_Use.Checked)
                    {
                        NumericUpDown numericUpDownX = userControl_SystemFont.numericUpDown_SystemFontX;
                        NumericUpDown numericUpDownY = userControl_SystemFont.numericUpDown_SystemFontY;
                        NumericUpDown numericUpDown_size = userControl_SystemFont.numericUpDown_SystemFont_size;
                        NumericUpDown numericUpDown_angle = userControl_SystemFont.numericUpDown_SystemFont_angle;
                        NumericUpDown numericUpDown_spacing = userControl_SystemFont.numericUpDown_SystemFont_spacing;
                        CheckBox checkBox_follow = userControl_SystemFont.checkBox_follow;

                        //int imageIndex = comboBox_image.SelectedIndex;
                        int x = (int)numericUpDownX.Value;
                        int y = (int)numericUpDownY.Value;
                        int size = (int)numericUpDown_size.Value;
                        int angle = (int)numericUpDown_angle.Value;
                        int spasing = (int)numericUpDown_spacing.Value;
                        string sValue = "";
                        switch (j)
                        {
                            case 0:
                                sValue = sValue_firstSF;
                                break;
                            case 1:
                                sValue = sValue_secondSF;
                                break;
                            case 2:
                                sValue = sValue_thirdSF;
                                break;
                        }
                        Color color = userControl_SystemFont.comboBoxGetColor();

                        //if (checkBox_follow.Checked && date_offsetX_sf >= 0)
                        //{
                        //    x = date_offsetX_sf;
                        //    y = date_offsetY_sf;
                        //    spasing = spasing_offset_sf;
                        //}

                        //date_offsetY_sf = y;
                        //spasing_offset_sf = spasing;
                        Draw_text(gPanel, x, y, size, spasing, color, angle, sValue, BBorder);
                    }

                    // надпись системным шрифтом по окружности
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
                        CheckBox checkBox_follow = userControl_FontRotate.checkBox_follow;

                        //int imageIndex = comboBox_image.SelectedIndex;
                        int x = (int)numericUpDownX.Value;
                        int y = (int)numericUpDownY.Value;
                        int size = (int)numericUpDown_size.Value;
                        int angle = (int)numericUpDown_angle.Value;
                        int radius = (int)numericUpDown_radius.Value;
                        int spasing = (int)numericUpDown_spacing.Value;
                        int rotate_direction = userControl_FontRotate.radioButtonGetRotateDirection();
                        string sValue = "";
                        switch (j)
                        {
                            case 0:
                                sValue = sValue_firstFR;
                                break;
                            case 1:
                                sValue = sValue_secondFR;
                                break;
                            case 2:
                                sValue = sValue_thirdFR;
                                break;
                        }
                        Color color = userControl_FontRotate.comboBoxGetColor();

                        //if (checkBox_follow.Checked && date_offsetX_sfr >= 0)
                        //{
                        //    x = date_offsetX_sfr;
                        //    y = date_offsetY_sfr;
                        //    spasing = spasing_offset_sfr;
                        //    angle = (int)date_offsetAngle_sfr;
                        //}

                        //date_offsetX_sfr = x;
                        //date_offsetY_sfr = y;
                        //spasing_offset_sfr = spasing;
                        Draw_text_rotate(gPanel, x, y, radius, size, spasing, color, angle, rotate_direction,
                            sValue, BBorder);
                    }
                }

                // число
                if (dateName == "Day")
                {
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
                        if (checkBox_Day_follow.Checked && date_offsetX >= 0)
                        {
                            x = date_offsetX;
                            alignment = 0;
                            y = date_offsetY;
                            spasing = spasing_offset;
                        }

                        date_offsetY = y;
                        spasing_offset = spasing;
                        date_offsetX = Draw_dagital_text(gPanel, imageIndex, x, y,
                            spasing, alignment, value, addZero, 2, separator_index, BBorder);

                        if (comboBox_Day_unit.SelectedIndex >= 0)
                        {
                            src = OpenFileStream(ListImagesFullName[comboBox_Day_unit.SelectedIndex]);
                            gPanel.DrawImage(src, new Rectangle((int)numericUpDown_Day_unitX.Value,
                                (int)numericUpDown_Day_unitY.Value, src.Width, src.Height));
                        }
                    }

                    // надпись системным шрифтом
                    UserControl_SystemFont userControl_SystemFont =
                        userControl_SystemFont_Group_Day.userControl_SystemFont;
                    if (userControl_SystemFont != null && userControl_SystemFont.checkBox_Use.Checked)
                    {
                        NumericUpDown numericUpDownX = userControl_SystemFont.numericUpDown_SystemFontX;
                        NumericUpDown numericUpDownY = userControl_SystemFont.numericUpDown_SystemFontY;
                        NumericUpDown numericUpDown_size = userControl_SystemFont.numericUpDown_SystemFont_size;
                        NumericUpDown numericUpDown_angle = userControl_SystemFont.numericUpDown_SystemFont_angle;
                        NumericUpDown numericUpDown_spacing = userControl_SystemFont.numericUpDown_SystemFont_spacing;
                        CheckBox checkBox_follow = userControl_SystemFont.checkBox_follow;

                        //int imageIndex = comboBox_image.SelectedIndex;
                        int x = (int)numericUpDownX.Value;
                        int y = (int)numericUpDownY.Value;
                        int size = (int)numericUpDown_size.Value;
                        int angle = (int)numericUpDown_angle.Value;
                        int spasing = (int)numericUpDown_spacing.Value;
                        string sValue = "";
                        switch (j)
                        {
                            case 0:
                                sValue = sValue_firstSF;
                                break;
                            case 1:
                                sValue = sValue_secondSF;
                                break;
                            case 2:
                                sValue = sValue_thirdSF;
                                break;
                        }
                        Color color = userControl_SystemFont.comboBoxGetColor();

                        //if (checkBox_follow.Checked && date_offsetX_sf >= 0)
                        //{
                        //    x = date_offsetX_sf;
                        //    y = date_offsetY_sf;
                        //    spasing = spasing_offset_sf;
                        //}

                        //date_offsetY_sf = y;
                        //spasing_offset_sf = spasing;
                        Draw_text(gPanel, x, y, size, spasing, color, angle, sValue, BBorder);
                    }


                    // надпись системным шрифтом по окружности
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
                        CheckBox checkBox_follow = userControl_FontRotate.checkBox_follow;

                        //int imageIndex = comboBox_image.SelectedIndex;
                        int x = (int)numericUpDownX.Value;
                        int y = (int)numericUpDownY.Value;
                        int size = (int)numericUpDown_size.Value;
                        int angle = (int)numericUpDown_angle.Value;
                        int radius = (int)numericUpDown_radius.Value;
                        int spasing = (int)numericUpDown_spacing.Value;
                        int rotate_direction = userControl_FontRotate.radioButtonGetRotateDirection();
                        string sValue = "";
                        switch (j)
                        {
                            case 0:
                                sValue = sValue_firstFR;
                                break;
                            case 1:
                                sValue = sValue_secondFR;
                                break;
                            case 2:
                                sValue = sValue_thirdFR;
                                break;
                        }
                        Color color = userControl_FontRotate.comboBoxGetColor();

                        //if (checkBox_follow.Checked && date_offsetX_sfr >= 0)
                        //{
                        //    x = date_offsetX_sfr;
                        //    y = date_offsetY_sfr;
                        //    spasing = spasing_offset_sfr;
                        //    angle = (int)date_offsetAngle_sfr;
                        //}

                        //date_offsetX_sfr = x;
                        //date_offsetY_sfr = y;
                        //spasing_offset_sfr = spasing;
                        Draw_text_rotate(gPanel, x, y, radius, size, spasing, color, angle, rotate_direction,
                            sValue, BBorder);
                    }
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
            UserControl_segments userControl_segments;
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


            for (int j = 0; j < dataGridView_SNL_Activity.RowCount; j++)
            {
                string activityName = dataGridView_SNL_Activity.Rows[j].Cells[0].Value.ToString();

                #region зараяд
                if (activityName == "Battery")
                {
                    userPanel_pictures = userControl_pictures_Battery;
                    userControl_segments = userControl_segments_Battery;
                    userPanel_text = userControl_text_Battery;
                    userPanel_hand = userControl_hand_Battery;
                    userPanel_scaleCircle = userControl_scaleCircle_Battery;
                    userPanel_scaleLinear = userControl_scaleLinear_Battery;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_Battery;
                    userControl_icon = userControl_icon_Battery;

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

                    // зараяд сегментами
                    if (userControl_segments.checkBox_pictures_Use.Checked)
                    {
                        List<Coordinates> coordinates = userControl_segments.GetCoordinates();
                        if (userControl_segments.comboBoxGetImage() >= 0 && coordinates != null)
                        {
                            int count = coordinates.Count;
                            int offSet = (int)(count * Watch_Face_Preview_Set.Battery / 100f);
                            //offSet--;
                            if (offSet < 0) offSet = 0;
                            if (offSet >= count) offSet = (int)(count - 1);
                            for (i = 0; i < count; i++)
                            {
                                if (userControl_segments.radioButtonGetDisplayType()== "Continuous")
                                {
                                    if (i <= offSet)
                                    {
                                        int imageIndex = userControl_segments.comboBoxGetSelectedIndexImage() + i;
                                        if (imageIndex < ListImagesFullName.Count)
                                        {

                                            int x = (int)coordinates[i].X;
                                            int y = (int)coordinates[i].Y;
                                            src = OpenFileStream(ListImagesFullName[imageIndex]);
                                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                        }
                                    } 
                                }
                                else
                                {
                                    if (i == offSet)
                                    {
                                        int imageIndex = userControl_segments.comboBoxGetSelectedIndexImage() + i;
                                        if (imageIndex < ListImagesFullName.Count)
                                        {

                                            int x = (int)coordinates[i].X;
                                            int y = (int)coordinates[i].Y;
                                            src = OpenFileStream(ListImagesFullName[imageIndex]);
                                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                        }
                                        break;
                                    }
                                }
                            }
                            
                        }
                    }

                    activityValue = Watch_Face_Preview_Set.Battery;
                    activityGoal = 100;
                    progress = Watch_Face_Preview_Set.Battery / 100f;
                    DrawActivity(gPanel, userPanel_text, userPanel_textGoal, userPanel_hand, userPanel_scaleCircle,
                        userPanel_scaleLinear, userControl_SystemFont_Group, userControl_icon, activityValue, 3, activityGoal,
                        progress, BBorder, showProgressArea, showCentrHend, "Battery"); 
                }

                #endregion

                #region шаги
                if (activityName == "Steps")
                {
                    userPanel_pictures = userControl_pictures_Steps;
                    userControl_segments = userControl_segments_Steps;
                    userPanel_text = userControl_text_Steps;
                    userPanel_textGoal = userControl_text_goal_Steps;
                    userPanel_hand = userControl_hand_Steps;
                    userPanel_scaleCircle = userControl_scaleCircle_Steps;
                    userPanel_scaleLinear = userControl_scaleLinear_Steps;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_Steps;
                    userControl_icon = userControl_icon_Steps;

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

                    // шаги сегментами
                    if (userControl_segments.checkBox_pictures_Use.Checked)
                    {
                        List<Coordinates> coordinates = userControl_segments.GetCoordinates();
                        if (userControl_segments.comboBoxGetImage() >= 0 && coordinates != null)
                        {
                            int count = coordinates.Count;
                            int offSet = (int)((count - 1) * Watch_Face_Preview_Set.Activity.Steps / Watch_Face_Preview_Set.Activity.StepsGoal);
                            //offSet--;
                            if (offSet < 0) offSet = 0;
                            if (offSet >= count) offSet = (int)(count - 1); 
                            if (Watch_Face_Preview_Set.Activity.Steps < Watch_Face_Preview_Set.Activity.StepsGoal / 100f)
                                offSet = -1;
                            if (offSet >= 0)
                            {
                                for (i = 0; i < count; i++)
                                {
                                    if (userControl_segments.radioButtonGetDisplayType() == "Continuous")
                                    {
                                        if (i <= offSet)
                                        {
                                            int imageIndex = userControl_segments.comboBoxGetSelectedIndexImage() + i;
                                            if (imageIndex < ListImagesFullName.Count)
                                            {

                                                int x = (int)coordinates[i].X;
                                                int y = (int)coordinates[i].Y;
                                                src = OpenFileStream(ListImagesFullName[imageIndex]);
                                                gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (i == offSet)
                                        {
                                            int imageIndex = userControl_segments.comboBoxGetSelectedIndexImage() + i;
                                            if (imageIndex < ListImagesFullName.Count)
                                            {

                                                int x = (int)coordinates[i].X;
                                                int y = (int)coordinates[i].Y;
                                                src = OpenFileStream(ListImagesFullName[imageIndex]);
                                                gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                            }
                                            break;
                                        }
                                    }
                                } 
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
                }
                #endregion

                #region калории
                if (activityName == "Calories")
                {
                    userPanel_pictures = userControl_pictures_Calories;
                    userControl_segments = userControl_segments_Calories;
                    userPanel_text = userControl_text_Calories;
                    userPanel_textGoal = userControl_text_goal_Calories;
                    userPanel_hand = userControl_hand_Calories;
                    userPanel_scaleCircle = userControl_scaleCircle_Calories;
                    userPanel_scaleLinear = userControl_scaleLinear_Calories;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_Calories;
                    userControl_icon = userControl_icon_Calories;

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

                    // калории сегментами
                    if (userControl_segments.checkBox_pictures_Use.Checked)
                    {
                        List<Coordinates> coordinates = userControl_segments.GetCoordinates();
                        if (userControl_segments.comboBoxGetImage() >= 0 && coordinates != null)
                        {
                            int count = coordinates.Count;
                            int offSet = (int)((count - 1f) * Watch_Face_Preview_Set.Activity.Calories / 300f);
                            //offSet--;
                            if (offSet < 0) offSet = 0;
                            if (offSet >= count) offSet = (int)(count - 1);
                            for (i = 0; i < count; i++)
                            {
                                if (userControl_segments.radioButtonGetDisplayType() == "Continuous")
                                {
                                    if (i <= offSet)
                                    {
                                        int imageIndex = userControl_segments.comboBoxGetSelectedIndexImage() + i;
                                        if (imageIndex < ListImagesFullName.Count)
                                        {

                                            int x = (int)coordinates[i].X;
                                            int y = (int)coordinates[i].Y;
                                            src = OpenFileStream(ListImagesFullName[imageIndex]);
                                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                        }
                                    }
                                }
                                else
                                {
                                    if (i == offSet)
                                    {
                                        int imageIndex = userControl_segments.comboBoxGetSelectedIndexImage() + i;
                                        if (imageIndex < ListImagesFullName.Count)
                                        {

                                            int x = (int)coordinates[i].X;
                                            int y = (int)coordinates[i].Y;
                                            src = OpenFileStream(ListImagesFullName[imageIndex]);
                                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                        }
                                        break;
                                    }
                                }
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
                }
                #endregion

                #region пульс
                if (activityName == "HeartRate")
                {
                    userPanel_pictures = userControl_pictures_HeartRate;
                    userControl_segments = userControl_segments_HeartRate;
                    userPanel_text = userControl_text_HeartRate;
                    userPanel_hand = userControl_hand_HeartRate;
                    userPanel_scaleCircle = userControl_scaleCircle_HeartRate;
                    userPanel_scaleLinear = userControl_scaleLinear_HeartRate;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_HeartRate;
                    userControl_icon = userControl_icon_HeartRate;

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

                    // пульс сегментами
                    if (userControl_segments.checkBox_pictures_Use.Checked)
                    {
                        List<Coordinates> coordinates = userControl_segments.GetCoordinates();
                        if (userControl_segments.comboBoxGetImage() >= 0 && coordinates != null)
                        {
                            int count = coordinates.Count;
                            int offSet = (int)(count * (Watch_Face_Preview_Set.Activity.HeartRate - 70) / 100f);
                            //offSet--;
                            if (offSet < 0) offSet = 0;
                            if (offSet >= count) offSet = (int)(count - 1);
                            for (i = 0; i < count; i++)
                            {
                                if (userControl_segments.radioButtonGetDisplayType() == "Continuous")
                                {
                                    if (i <= offSet)
                                    {
                                        int imageIndex = userControl_segments.comboBoxGetSelectedIndexImage() + i;
                                        if (imageIndex < ListImagesFullName.Count)
                                        {

                                            int x = (int)coordinates[i].X;
                                            int y = (int)coordinates[i].Y;
                                            src = OpenFileStream(ListImagesFullName[imageIndex]);
                                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                        }
                                    }
                                }
                                else
                                {
                                    if (i == offSet)
                                    {
                                        int imageIndex = userControl_segments.comboBoxGetSelectedIndexImage() + i;
                                        if (imageIndex < ListImagesFullName.Count)
                                        {

                                            int x = (int)coordinates[i].X;
                                            int y = (int)coordinates[i].Y;
                                            src = OpenFileStream(ListImagesFullName[imageIndex]);
                                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                        }
                                        break;
                                    }
                                }
                            }

                        }
                    }

                    activityValue = Watch_Face_Preview_Set.Activity.HeartRate;
                    activityGoal = 180;
                    progress = Watch_Face_Preview_Set.Activity.HeartRate / 181f;
                    DrawActivity(gPanel, userPanel_text, userPanel_textGoal, userPanel_hand, userPanel_scaleCircle,
                        userPanel_scaleLinear, userControl_SystemFont_Group, userControl_icon, activityValue, 3, activityGoal,
                        progress, BBorder, showProgressArea, showCentrHend, "HeartRate"); 
                }
                #endregion

                #region PAI
                if (activityName == "PAI")
                {
                    userPanel_pictures = userControl_pictures_PAI;
                    userControl_segments = userControl_segments_PAI;
                    userPanel_text = userControl_text_PAI;
                    userPanel_hand = userControl_hand_PAI;
                    userPanel_scaleCircle = userControl_scaleCircle_PAI;
                    userPanel_scaleLinear = userControl_scaleLinear_PAI;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_PAI;
                    userControl_icon = userControl_icon_PAI;

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

                    // PAI сегментами
                    if (userControl_segments.checkBox_pictures_Use.Checked)
                    {
                        List<Coordinates> coordinates = userControl_segments.GetCoordinates();
                        if (userControl_segments.comboBoxGetImage() >= 0 && coordinates != null)
                        {
                            int count = coordinates.Count;
                            int offSet = (int)((count + 1f) * Watch_Face_Preview_Set.Activity.PAI / 100f);
                            //offSet--;
                            if (offSet < 0) offSet = 0;
                            if (offSet >= count) offSet = (int)(count - 1);
                            for (i = 0; i < count; i++)
                            {
                                if (userControl_segments.radioButtonGetDisplayType() == "Continuous")
                                {
                                    if (i <= offSet)
                                    {
                                        int imageIndex = userControl_segments.comboBoxGetSelectedIndexImage() + i;
                                        if (imageIndex < ListImagesFullName.Count)
                                        {

                                            int x = (int)coordinates[i].X;
                                            int y = (int)coordinates[i].Y;
                                            src = OpenFileStream(ListImagesFullName[imageIndex]);
                                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                        }
                                    }
                                }
                                else
                                {
                                    if (i == offSet)
                                    {
                                        int imageIndex = userControl_segments.comboBoxGetSelectedIndexImage() + i;
                                        if (imageIndex < ListImagesFullName.Count)
                                        {

                                            int x = (int)coordinates[i].X;
                                            int y = (int)coordinates[i].Y;
                                            src = OpenFileStream(ListImagesFullName[imageIndex]);
                                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                        }
                                        break;
                                    }
                                }
                            }

                        }
                    }

                    activityValue = Watch_Face_Preview_Set.Activity.PAI;
                    activityGoal = 100;
                    progress = Watch_Face_Preview_Set.Activity.PAI / 100f;
                    DrawActivity(gPanel, userPanel_text, userPanel_textGoal, userPanel_hand, userPanel_scaleCircle,
                        userPanel_scaleLinear, userControl_SystemFont_Group, userControl_icon, activityValue, 3, activityGoal,
                        progress, BBorder, showProgressArea, showCentrHend, "PAI"); 
                }

                #endregion

                #region путь
                if (activityName == "Distance")
                {
                    userPanel_pictures = null;
                    userPanel_text = userControl_text_Distance;
                    userPanel_hand = null;
                    userPanel_scaleCircle = null;
                    userPanel_scaleLinear = null;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_Distance;
                    userControl_icon = userControl_icon_Distance;

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
                }

                #endregion

                #region StandUp
                if (activityName == "StandUp")
                {
                    userPanel_pictures = userControl_pictures_StandUp;
                    userControl_segments = userControl_segments_StandUp;
                    userPanel_text = userControl_text_StandUp;
                    userPanel_textGoal = userControl_text_goal_StandUp;
                    userPanel_hand = userControl_hand_StandUp;
                    userPanel_scaleCircle = userControl_scaleCircle_StandUp;
                    userPanel_scaleLinear = userControl_scaleLinear_StandUp;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_StandUp;
                    userControl_icon = userControl_icon_StandUp;

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

                    // StandUp сегментами
                    if (userControl_segments.checkBox_pictures_Use.Checked)
                    {
                        List<Coordinates> coordinates = userControl_segments.GetCoordinates();
                        if (userControl_segments.comboBoxGetImage() >= 0 && coordinates != null)
                        {
                            int count = coordinates.Count;
                            int offSet = (int)((count + 1f) * Watch_Face_Preview_Set.Activity.StandUp / 12f);
                            //offSet--;
                            if (offSet < 0) offSet = 0;
                            if (offSet >= count) offSet = (int)(count - 1);
                            for (i = 0; i < count; i++)
                            {
                                if (userControl_segments.radioButtonGetDisplayType() == "Continuous")
                                {
                                    if (i <= offSet)
                                    {
                                        int imageIndex = userControl_segments.comboBoxGetSelectedIndexImage() + i;
                                        if (imageIndex < ListImagesFullName.Count)
                                        {

                                            int x = (int)coordinates[i].X;
                                            int y = (int)coordinates[i].Y;
                                            src = OpenFileStream(ListImagesFullName[imageIndex]);
                                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                        }
                                    }
                                }
                                else
                                {
                                    if (i == offSet)
                                    {
                                        int imageIndex = userControl_segments.comboBoxGetSelectedIndexImage() + i;
                                        if (imageIndex < ListImagesFullName.Count)
                                        {

                                            int x = (int)coordinates[i].X;
                                            int y = (int)coordinates[i].Y;
                                            src = OpenFileStream(ListImagesFullName[imageIndex]);
                                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                        }
                                        break;
                                    }
                                }
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
                }
                #endregion

                #region погода
                if (activityName == "Weather")
                {
                    UserControl_pictures_weather userPanel_pictures_weather = userControl_pictures_weather;
                    UserControl_text_weather userPanel_text_weather_Current = userControl_text_weather_Current;
                    UserControl_text_weather userPanel_text_weather_Min = userControl_text_weather_Min;
                    UserControl_text_weather userPanel_text_weather_Max = userControl_text_weather_Max;
                    userPanel_hand = userControl_hand_Weather;
                    userPanel_scaleCircle = userControl_scaleCircle_Weather;
                    userPanel_scaleLinear = userControl_scaleLinear_Weather;
                    UserControl_SystemFont_GroupWeather userControl_SystemFont_Group_Weather
                        = userControl_SystemFont_GroupWeather;
                    userControl_icon = userControl_icon_Weather;
                    int value_current = Watch_Face_Preview_Set.Weather.Temperature;
                    int value_min = Watch_Face_Preview_Set.Weather.TemperatureMin;
                    int value_max = Watch_Face_Preview_Set.Weather.TemperatureMax;
                    int icon_index = Watch_Face_Preview_Set.Weather.Icon;
                    bool showTemperature = Watch_Face_Preview_Set.Weather.showTemperature;
                    bool weatherAlignmentFix = checkBox_weatherAlignmentFix.Checked;

                    DrawWeather(gPanel, userPanel_pictures_weather, userPanel_text_weather_Current, userPanel_text_weather_Min,
                        userPanel_text_weather_Max, userControl_SystemFont_Group_Weather, userControl_icon, value_current,
                        value_min, value_max, icon_index, BBorder, showTemperature, weatherAlignmentFix); 
                }

                #endregion

                #region UVindex
                if (activityName == "UVindex")
                {
                    userPanel_pictures = userControl_pictures_UVindex;
                    userControl_segments = userControl_segments_UVindex;
                    userPanel_text = userControl_text_UVindex;
                    userPanel_hand = userControl_hand_UVindex;
                    userPanel_scaleCircle = userControl_scaleCircle_UVindex;
                    userPanel_scaleLinear = userControl_scaleLinear_UVindex;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_UVindex;
                    userControl_icon = userControl_icon_UVindex;

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
                            offSet = 0;
                            if (Watch_Face_Preview_Set.Weather.UVindex > 3) offSet++;
                            if (Watch_Face_Preview_Set.Weather.UVindex > 5) offSet++;
                            if (Watch_Face_Preview_Set.Weather.UVindex > 7) offSet++;
                            if (Watch_Face_Preview_Set.Weather.UVindex > 9) offSet++;
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

                    // UVindex сегментами
                    if (userControl_segments.checkBox_pictures_Use.Checked)
                    {
                        List<Coordinates> coordinates = userControl_segments.GetCoordinates();
                        if (userControl_segments.comboBoxGetImage() >= 0 && coordinates != null)
                        {
                            int count = coordinates.Count;
                            int offSet = (int)((count + 1f) * Watch_Face_Preview_Set.Weather.UVindex / 10f);
                            //offSet--;
                            offSet = 0;
                            if (Watch_Face_Preview_Set.Weather.UVindex > 3) offSet++;
                            if (Watch_Face_Preview_Set.Weather.UVindex > 5) offSet++;
                            if (Watch_Face_Preview_Set.Weather.UVindex > 7) offSet++;
                            if (Watch_Face_Preview_Set.Weather.UVindex > 9) offSet++;
                            if (offSet < 0) offSet = 0;
                            if (offSet >= count) offSet = (int)(count - 1);
                            for (i = 0; i < count; i++)
                            {
                                if (userControl_segments.radioButtonGetDisplayType() == "Continuous")
                                {
                                    if (i <= offSet)
                                    {
                                        int imageIndex = userControl_segments.comboBoxGetSelectedIndexImage() + i;
                                        if (imageIndex < ListImagesFullName.Count)
                                        {

                                            int x = (int)coordinates[i].X;
                                            int y = (int)coordinates[i].Y;
                                            src = OpenFileStream(ListImagesFullName[imageIndex]);
                                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                        }
                                    }
                                }
                                else
                                {
                                    if (i == offSet)
                                    {
                                        int imageIndex = userControl_segments.comboBoxGetSelectedIndexImage() + i;
                                        if (imageIndex < ListImagesFullName.Count)
                                        {

                                            int x = (int)coordinates[i].X;
                                            int y = (int)coordinates[i].Y;
                                            src = OpenFileStream(ListImagesFullName[imageIndex]);
                                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                        }
                                        break;
                                    }
                                }
                            }

                        }
                    }

                    activityValue = Watch_Face_Preview_Set.Weather.UVindex;
                    activityGoal = 10;
                    progress = Watch_Face_Preview_Set.Weather.UVindex / 10f;
                    DrawActivity(gPanel, userPanel_text, userPanel_textGoal, userPanel_hand, userPanel_scaleCircle,
                        userPanel_scaleLinear, userControl_SystemFont_Group, userControl_icon, activityValue, 2, activityGoal,
                        progress, BBorder, showProgressArea, showCentrHend, "UVindex"); 
                }

                #endregion

                #region AirQuality
                if (activityName == "AirQuality")
                {
                    userPanel_pictures = userControl_pictures_AirQuality;
                    userControl_segments = userControl_segments_AirQuality;
                    userPanel_text = userControl_text_AirQuality;
                    userPanel_hand = userControl_hand_AirQuality;
                    userPanel_scaleCircle = userControl_scaleCircle_AirQuality;
                    userPanel_scaleLinear = userControl_scaleLinear_AirQuality;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_AirQuality;
                    userControl_icon = userControl_icon_AirQuality;

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

                    // AirQuality сегментами
                    if (userControl_segments.checkBox_pictures_Use.Checked)
                    {
                        List<Coordinates> coordinates = userControl_segments.GetCoordinates();
                        if (userControl_segments.comboBoxGetImage() >= 0 && coordinates != null)
                        {
                            int count = coordinates.Count;
                            int offSet = (int)((count + 1f) * Watch_Face_Preview_Set.Weather.AirQuality / 502f);
                            //offSet--;
                            if (offSet < 0) offSet = 0;
                            if (offSet >= count) offSet = (int)(count - 1);
                            for (i = 0; i < count; i++)
                            {
                                if (userControl_segments.radioButtonGetDisplayType() == "Continuous")
                                {
                                    if (i <= offSet)
                                    {
                                        int imageIndex = userControl_segments.comboBoxGetSelectedIndexImage() + i;
                                        if (imageIndex < ListImagesFullName.Count)
                                        {

                                            int x = (int)coordinates[i].X;
                                            int y = (int)coordinates[i].Y;
                                            src = OpenFileStream(ListImagesFullName[imageIndex]);
                                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                        }
                                    }
                                }
                                else
                                {
                                    if (i == offSet)
                                    {
                                        int imageIndex = userControl_segments.comboBoxGetSelectedIndexImage() + i;
                                        if (imageIndex < ListImagesFullName.Count)
                                        {

                                            int x = (int)coordinates[i].X;
                                            int y = (int)coordinates[i].Y;
                                            src = OpenFileStream(ListImagesFullName[imageIndex]);
                                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                        }
                                        break;
                                    }
                                }
                            }

                        }
                    }

                    activityValue = Watch_Face_Preview_Set.Weather.AirQuality;
                    activityGoal = 500;
                    progress = Watch_Face_Preview_Set.Weather.AirQuality / 503f;
                    DrawActivity(gPanel, userPanel_text, userPanel_textGoal, userPanel_hand, userPanel_scaleCircle,
                        userPanel_scaleLinear, userControl_SystemFont_Group, userControl_icon, activityValue, 3, activityGoal,
                        progress, BBorder, showProgressArea, showCentrHend, "AirQuality"); 
                }

                #endregion

                #region Humidity
                if (activityName == "Humidity")
                {
                    userPanel_pictures = userControl_pictures_Humidity;
                    userControl_segments = userControl_segments_Humidity;
                    userPanel_text = userControl_text_Humidity;
                    userPanel_hand = userControl_hand_Humidity;
                    userPanel_scaleCircle = userControl_scaleCircle_Humidity;
                    userPanel_scaleLinear = userControl_scaleLinear_Humidity;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_Humidity;
                    userControl_icon = userControl_icon_Humidity;

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

                    // Humidity сегментами
                    if (userControl_segments.checkBox_pictures_Use.Checked)
                    {
                        List<Coordinates> coordinates = userControl_segments.GetCoordinates();
                        if (userControl_segments.comboBoxGetImage() >= 0 && coordinates != null)
                        {
                            int count = coordinates.Count;
                            int offSet = (int)((count + 1f) * Watch_Face_Preview_Set.Weather.Humidity / 100f);
                            //offSet--;
                            if (offSet < 0) offSet = 0;
                            if (offSet >= count) offSet = (int)(count - 1);
                            for (i = 0; i < count; i++)
                            {
                                if (userControl_segments.radioButtonGetDisplayType() == "Continuous")
                                {
                                    if (i <= offSet)
                                    {
                                        int imageIndex = userControl_segments.comboBoxGetSelectedIndexImage() + i;
                                        if (imageIndex < ListImagesFullName.Count)
                                        {

                                            int x = (int)coordinates[i].X;
                                            int y = (int)coordinates[i].Y;
                                            src = OpenFileStream(ListImagesFullName[imageIndex]);
                                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                        }
                                    }
                                }
                                else
                                {
                                    if (i == offSet)
                                    {
                                        int imageIndex = userControl_segments.comboBoxGetSelectedIndexImage() + i;
                                        if (imageIndex < ListImagesFullName.Count)
                                        {

                                            int x = (int)coordinates[i].X;
                                            int y = (int)coordinates[i].Y;
                                            src = OpenFileStream(ListImagesFullName[imageIndex]);
                                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                        }
                                        break;
                                    }
                                }
                            }

                        }
                    }

                    activityValue = Watch_Face_Preview_Set.Weather.Humidity;
                    activityGoal = 100;
                    progress = Watch_Face_Preview_Set.Weather.Humidity / 100f;
                    DrawActivity(gPanel, userPanel_text, userPanel_textGoal, userPanel_hand, userPanel_scaleCircle,
                        userPanel_scaleLinear, userControl_SystemFont_Group, userControl_icon, activityValue, 2, activityGoal,
                        progress, BBorder, showProgressArea, showCentrHend, "Humidity"); 
                }

                #endregion

                #region Sunrise
                if (activityName == "Sunrise")
                {
                    userPanel_pictures = userControl_pictures_Sunrise;
                    userPanel_text = userControl_text_SunriseSunset;
                    UserControl_text userPanel_text_Sunrise_Min = userControl_text_Sunrise;
                    UserControl_text userPanel_text_Sunrise_Max = userControl_text_Sunset;
                    userPanel_hand = userControl_hand_Sunrise;
                    userPanel_scaleCircle = userControl_scaleCircle_Sunrise;
                    userPanel_scaleLinear = userControl_scaleLinear_Sunrise;
                    UserControl_SystemFont_GroupSunrise userControl_SystemFont_Group_Sunrise
                        = userControl_SystemFont_GroupSunrise;
                    userControl_icon = userControl_icon_Sunrise;

                    int hours = Watch_Face_Preview_Set.Time.Hours;
                    int minutes = Watch_Face_Preview_Set.Time.Minutes;

                    DrawSunrise(gPanel, userPanel_pictures, userPanel_text, userPanel_text_Sunrise_Min,
                        userPanel_text_Sunrise_Max, userPanel_hand, userPanel_scaleCircle, userPanel_scaleLinear,
                        userControl_SystemFont_Group_Sunrise, userControl_icon, hours, minutes, BBorder, showProgressArea,
                        showCentrHend); 
                }

                #endregion

                #region WindForce
                if (activityName == "WindForce")
                {
                    userPanel_pictures = userControl_pictures_WindForce;
                    userControl_segments = userControl_segments_WindForce;
                    userPanel_text = userControl_text_WindForce;
                    userPanel_hand = userControl_hand_WindForce;
                    userPanel_scaleCircle = userControl_scaleCircle_WindForce;
                    userPanel_scaleLinear = userControl_scaleLinear_WindForce;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_WindForce;
                    userControl_icon = userControl_icon_WindForce;

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

                    // WindForce сегментами
                    if (userControl_segments.checkBox_pictures_Use.Checked)
                    {
                        List<Coordinates> coordinates = userControl_segments.GetCoordinates();
                        if (userControl_segments.comboBoxGetImage() >= 0 && coordinates != null)
                        {
                            int count = coordinates.Count;
                            int offSet = (int)((count + 1f) * Watch_Face_Preview_Set.Weather.WindForce / 12f);
                            //offSet--;
                            if (offSet < 0) offSet = 0;
                            if (offSet >= count) offSet = (int)(count - 1);
                            for (i = 0; i < count; i++)
                            {
                                if (userControl_segments.radioButtonGetDisplayType() == "Continuous")
                                {
                                    if (i <= offSet)
                                    {
                                        int imageIndex = userControl_segments.comboBoxGetSelectedIndexImage() + i;
                                        if (imageIndex < ListImagesFullName.Count)
                                        {

                                            int x = (int)coordinates[i].X;
                                            int y = (int)coordinates[i].Y;
                                            src = OpenFileStream(ListImagesFullName[imageIndex]);
                                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                        }
                                    }
                                }
                                else
                                {
                                    if (i == offSet)
                                    {
                                        int imageIndex = userControl_segments.comboBoxGetSelectedIndexImage() + i;
                                        if (imageIndex < ListImagesFullName.Count)
                                        {

                                            int x = (int)coordinates[i].X;
                                            int y = (int)coordinates[i].Y;
                                            src = OpenFileStream(ListImagesFullName[imageIndex]);
                                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                        }
                                        break;
                                    }
                                }
                            }

                        }
                    }

                    activityValue = Watch_Face_Preview_Set.Weather.WindForce;
                    activityGoal = 12;
                    progress = Watch_Face_Preview_Set.Weather.WindForce / 12f;
                    DrawActivity(gPanel, userPanel_text, userPanel_textGoal, userPanel_hand, userPanel_scaleCircle,
                        userPanel_scaleLinear, userControl_SystemFont_Group, userControl_icon, activityValue, 2, activityGoal,
                        progress, BBorder, showProgressArea, showCentrHend, "WindForce"); 
                }

                #endregion

                #region Altitude
                if (activityName == "Altitude")
                {
                    userPanel_pictures = userControl_pictures_Altitude;
                    userControl_segments = userControl_segments_Altitude;
                    userPanel_text = userControl_text_Altitude;
                    userPanel_hand = userControl_hand_Altitude;
                    userPanel_scaleCircle = userControl_scaleCircle_Altitude;
                    userPanel_scaleLinear = userControl_scaleLinear_Altitude;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_Altitude;
                    userControl_icon = userControl_icon_Altitude;

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

                    // Altitude сегментами
                    if (userControl_segments.checkBox_pictures_Use.Checked)
                    {
                        List<Coordinates> coordinates = userControl_segments.GetCoordinates();
                        if (userControl_segments.comboBoxGetImage() >= 0 && coordinates != null)
                        {
                            int count = coordinates.Count;
                            int offSet = (int)((count + 1f) * (Watch_Face_Preview_Set.Weather.Altitude + 1000) / 10000f);
                            //offSet--;
                            if (offSet < 0) offSet = 0;
                            if (offSet >= count) offSet = (int)(count - 1);
                            for (i = 0; i < count; i++)
                            {
                                if (userControl_segments.radioButtonGetDisplayType() == "Continuous")
                                {
                                    if (i <= offSet)
                                    {
                                        int imageIndex = userControl_segments.comboBoxGetSelectedIndexImage() + i;
                                        if (imageIndex < ListImagesFullName.Count)
                                        {

                                            int x = (int)coordinates[i].X;
                                            int y = (int)coordinates[i].Y;
                                            src = OpenFileStream(ListImagesFullName[imageIndex]);
                                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                        }
                                    }
                                }
                                else
                                {
                                    if (i == offSet)
                                    {
                                        int imageIndex = userControl_segments.comboBoxGetSelectedIndexImage() + i;
                                        if (imageIndex < ListImagesFullName.Count)
                                        {

                                            int x = (int)coordinates[i].X;
                                            int y = (int)coordinates[i].Y;
                                            src = OpenFileStream(ListImagesFullName[imageIndex]);
                                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                        }
                                        break;
                                    }
                                }
                            }

                        }
                    }

                    activityValue = Watch_Face_Preview_Set.Weather.Altitude;
                    activityGoal = 9000;
                    progress = (Watch_Face_Preview_Set.Weather.Altitude + 1000) / 10000f;
                    DrawActivity(gPanel, userPanel_text, userPanel_textGoal, userPanel_hand, userPanel_scaleCircle,
                        userPanel_scaleLinear, userControl_SystemFont_Group, userControl_icon, activityValue, 4, activityGoal,
                        progress, BBorder, showProgressArea, showCentrHend, "Altitude"); 
                }

                #endregion

                #region AirPressure
                if (activityName == "AirPressure")
                {
                    userPanel_pictures = userControl_pictures_AirPressure;
                    userControl_segments = userControl_segments_AirPressure;
                    userPanel_text = userControl_text_AirPressure;
                    userPanel_hand = userControl_hand_AirPressure;
                    userPanel_scaleCircle = userControl_scaleCircle_AirPressure;
                    userPanel_scaleLinear = userControl_scaleLinear_AirPressure;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_AirPressure;
                    userControl_icon = userControl_icon_AirPressure;

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

                    // AirPressure сегментами
                    if (userControl_segments.checkBox_pictures_Use.Checked)
                    {
                        List<Coordinates> coordinates = userControl_segments.GetCoordinates();
                        if (userControl_segments.comboBoxGetImage() >= 0 && coordinates != null)
                        {
                            int count = coordinates.Count;
                            int offSet = (int)((count + 1f) * (Watch_Face_Preview_Set.Weather.AirPressure - 200) / 1000f);
                            //offSet--;
                            if (offSet < 0) offSet = 0;
                            if (offSet >= count) offSet = (int)(count - 1);
                            for (i = 0; i < count; i++)
                            {
                                if (userControl_segments.radioButtonGetDisplayType() == "Continuous")
                                {
                                    if (i <= offSet)
                                    {
                                        int imageIndex = userControl_segments.comboBoxGetSelectedIndexImage() + i;
                                        if (imageIndex < ListImagesFullName.Count)
                                        {

                                            int x = (int)coordinates[i].X;
                                            int y = (int)coordinates[i].Y;
                                            src = OpenFileStream(ListImagesFullName[imageIndex]);
                                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                        }
                                    }
                                }
                                else
                                {
                                    if (i == offSet)
                                    {
                                        int imageIndex = userControl_segments.comboBoxGetSelectedIndexImage() + i;
                                        if (imageIndex < ListImagesFullName.Count)
                                        {

                                            int x = (int)coordinates[i].X;
                                            int y = (int)coordinates[i].Y;
                                            src = OpenFileStream(ListImagesFullName[imageIndex]);
                                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                        }
                                        break;
                                    }
                                }
                            }

                        }
                    }

                    activityValue = Watch_Face_Preview_Set.Weather.AirPressure;
                    activityGoal = 1200;
                    progress = (Watch_Face_Preview_Set.Weather.AirPressure - 170) / 1000f;
                    DrawActivity(gPanel, userPanel_text, userPanel_textGoal, userPanel_hand, userPanel_scaleCircle,
                        userPanel_scaleLinear, userControl_SystemFont_Group, userControl_icon, activityValue, 4, activityGoal,
                        progress, BBorder, showProgressArea, showCentrHend, "AirPressure"); 
                }

                #endregion

                #region Stress
                if (activityName == "Stress")
                {
                    userPanel_pictures = userControl_pictures_Stress;
                    userControl_segments = userControl_segments_Stress;
                    userPanel_text = userControl_text_Stress;
                    userPanel_hand = userControl_hand_Stress;
                    userPanel_scaleCircle = userControl_scaleCircle_Stress;
                    userPanel_scaleLinear = userControl_scaleLinear_Stress;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_Stress;
                    userControl_icon = userControl_icon_Stress;

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

                    // Stress сегментами
                    if (userControl_segments.checkBox_pictures_Use.Checked)
                    {
                        List<Coordinates> coordinates = userControl_segments.GetCoordinates();
                        if (userControl_segments.comboBoxGetImage() >= 0 && coordinates != null)
                        {
                            int count = coordinates.Count;
                            int offSet = (int)((count + 1f) * Watch_Face_Preview_Set.Activity.Stress / 12f);
                            //offSet--;
                            if (offSet < 0) offSet = 0;
                            if (offSet >= count) offSet = (int)(count - 1);
                            for (i = 0; i < count; i++)
                            {
                                if (userControl_segments.radioButtonGetDisplayType() == "Continuous")
                                {
                                    if (i <= offSet)
                                    {
                                        int imageIndex = userControl_segments.comboBoxGetSelectedIndexImage() + i;
                                        if (imageIndex < ListImagesFullName.Count)
                                        {

                                            int x = (int)coordinates[i].X;
                                            int y = (int)coordinates[i].Y;
                                            src = OpenFileStream(ListImagesFullName[imageIndex]);
                                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                        }
                                    }
                                }
                                else
                                {
                                    if (i == offSet)
                                    {
                                        int imageIndex = userControl_segments.comboBoxGetSelectedIndexImage() + i;
                                        if (imageIndex < ListImagesFullName.Count)
                                        {

                                            int x = (int)coordinates[i].X;
                                            int y = (int)coordinates[i].Y;
                                            src = OpenFileStream(ListImagesFullName[imageIndex]);
                                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                        }
                                        break;
                                    }
                                }
                            }

                        }
                    }

                    activityValue = Watch_Face_Preview_Set.Activity.Stress;
                    activityGoal = 12;
                    progress = Watch_Face_Preview_Set.Activity.Stress / 12f;
                    DrawActivity(gPanel, userPanel_text, userPanel_textGoal, userPanel_hand, userPanel_scaleCircle,
                        userPanel_scaleLinear, userControl_SystemFont_Group, userControl_icon, activityValue, 2, activityGoal,
                        progress, BBorder, showProgressArea, showCentrHend, "Stress"); 
                }

                #endregion

                #region ActivityGoal
                if (activityName == "ActivityGoal")
                {
                    userPanel_pictures = userControl_pictures_ActivityGoal;
                    userControl_segments = userControl_segments_ActivityGoal;
                    userPanel_text = userControl_text_ActivityGoal;
                    userPanel_textGoal = userControl_text_goal_ActivityGoal;
                    userPanel_hand = userControl_hand_ActivityGoal;
                    userPanel_scaleCircle = userControl_scaleCircle_ActivityGoal;
                    userPanel_scaleLinear = userControl_scaleLinear_ActivityGoal;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_ActivityGoal;
                    userControl_icon = userControl_icon_ActivityGoal;

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

                    // ActivityGoal сегментами
                    if (userControl_segments.checkBox_pictures_Use.Checked)
                    {
                        List<Coordinates> coordinates = userControl_segments.GetCoordinates();
                        if (userControl_segments.comboBoxGetImage() >= 0 && coordinates != null)
                        {
                            int count = coordinates.Count; int offSet = (int)((count - 1) * Watch_Face_Preview_Set.Activity.Steps / Watch_Face_Preview_Set.Activity.StepsGoal);
                            if (offSet < 0) offSet = 0;
                            if (offSet >= count) offSet = (int)(count - 1);
                            if (Watch_Face_Preview_Set.Activity.Steps < Watch_Face_Preview_Set.Activity.StepsGoal / 100f)
                                offSet = -1;
                            if (ActivityGoal_Calories)
                            {
                                offSet = (int)((count - 1f) * Watch_Face_Preview_Set.Activity.Calories / 300f);
                                if (offSet < 0) offSet = 0;
                                if (offSet >= count) offSet = (int)(count - 1);
                            }

                            for (i = 0; i < count; i++)
                            {
                                if (userControl_segments.radioButtonGetDisplayType() == "Continuous")
                                {
                                    if (i <= offSet)
                                    {
                                        int imageIndex = userControl_segments.comboBoxGetSelectedIndexImage() + i;
                                        if (offSet >= 0 && imageIndex < ListImagesFullName.Count)
                                        {

                                            int x = (int)coordinates[i].X;
                                            int y = (int)coordinates[i].Y;
                                            src = OpenFileStream(ListImagesFullName[imageIndex]);
                                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                        }
                                    }
                                }
                                else
                                {
                                    if (offSet >= 0 && i == offSet)
                                    {
                                        int imageIndex = userControl_segments.comboBoxGetSelectedIndexImage() + i;
                                        if (imageIndex < ListImagesFullName.Count)
                                        {

                                            int x = (int)coordinates[i].X;
                                            int y = (int)coordinates[i].Y;
                                            src = OpenFileStream(ListImagesFullName[imageIndex]);
                                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                        }
                                        break;
                                    }
                                }
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
                }
                #endregion

                #region FatBurning
                if (activityName == "FatBurning")
                {
                    userPanel_pictures = userControl_pictures_FatBurning;
                    userControl_segments = userControl_segments_FatBurning;
                    userPanel_text = userControl_text_FatBurning;
                    userPanel_textGoal = userControl_text_goal_FatBurning;
                    userPanel_hand = userControl_hand_FatBurning;
                    userPanel_scaleCircle = userControl_scaleCircle_FatBurning;
                    userPanel_scaleLinear = userControl_scaleLinear_FatBurning;
                    userControl_SystemFont_Group = userControl_SystemFont_Group_FatBurning;
                    userControl_icon = userControl_icon_FatBurning;

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

                    // FatBurning сегментами
                    if (userControl_segments.checkBox_pictures_Use.Checked)
                    {
                        List<Coordinates> coordinates = userControl_segments.GetCoordinates();
                        if (userControl_segments.comboBoxGetImage() >= 0 && coordinates != null)
                        {
                            int count = coordinates.Count;
                            int offSet = (int)((count + 1f) * Watch_Face_Preview_Set.Activity.FatBurning / 30f);
                            //offSet--;
                            if (offSet < 0) offSet = 0;
                            if (offSet >= count) offSet = (int)(count - 1);
                            for (i = 0; i < count; i++)
                            {
                                if (userControl_segments.radioButtonGetDisplayType() == "Continuous")
                                {
                                    if (i <= offSet)
                                    {
                                        int imageIndex = userControl_segments.comboBoxGetSelectedIndexImage() + i;
                                        if (imageIndex < ListImagesFullName.Count)
                                        {

                                            int x = (int)coordinates[i].X;
                                            int y = (int)coordinates[i].Y;
                                            src = OpenFileStream(ListImagesFullName[imageIndex]);
                                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                        }
                                    }
                                }
                                else
                                {
                                    if (i == offSet)
                                    {
                                        int imageIndex = userControl_segments.comboBoxGetSelectedIndexImage() + i;
                                        if (imageIndex < ListImagesFullName.Count)
                                        {

                                            int x = (int)coordinates[i].X;
                                            int y = (int)coordinates[i].Y;
                                            src = OpenFileStream(ListImagesFullName[imageIndex]);
                                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                                        }
                                        break;
                                    }
                                }
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
                }
                #endregion 
            }



            #endregion

            if (link < 0) goto TimeEnd;
            #region цифровое время
            int time_offsetX = -1;
            int time_offsetY = -1;
            int time_spasing = 0;
            bool _pm = false;
            // часы
            if (checkBox_Hour_Use.Checked && comboBox_Hour_image.SelectedIndex >= 0)
            {
                int imageIndex = comboBox_Hour_image.SelectedIndex;
                int x = (int)numericUpDown_HourX.Value;
                int y = (int)numericUpDown_HourY.Value;
                time_offsetY = y;
                int spasing = (int)numericUpDown_Hour_spacing.Value;
                time_spasing = spasing;
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

                if (!MS_24h)
                {
                    time_offsetX = Draw_dagital_text(gPanel, imageIndex, x, y,
                                    spasing, alignment, value, addZero, 2, separator_index, BBorder); 
                }
                else
                {
                    imageIndex = imageIndex + Watch_Face_Preview_Set.Time.Hours - 1;
                    if (imageIndex < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[imageIndex]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }
                }

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
                //bool addZero = checkBox_Minute_add_zero.Checked;
                bool addZero = true;
                int value = Watch_Face_Preview_Set.Time.Minutes;
                int separator_index = -1;
                if (comboBox_Minute_separator.SelectedIndex >= 0) separator_index = comboBox_Minute_separator.SelectedIndex;
                if (checkBox_Minute_follow.Checked && time_offsetX > -1)
                {
                    x = time_offsetX;
                    alignment = 0;
                    y = time_offsetY;
                    spasing = time_spasing;
                }
                time_offsetY = y;
                time_spasing = spasing;
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
                //bool addZero = checkBox_Second_add_zero.Checked;
                bool addZero = true;
                int value = Watch_Face_Preview_Set.Time.Seconds;
                int separator_index = -1;
                if (comboBox_Second_separator.SelectedIndex >= 0) separator_index = comboBox_Second_separator.SelectedIndex;
                if (checkBox_Second_follow.Checked && time_offsetX > -1)
                {
                    x = time_offsetX;
                    alignment = 0;
                    y = time_offsetY;
                    spasing = time_spasing;
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

            // системный шрифт
            int SFhour = Watch_Face_Preview_Set.Time.Hours;
            int SFminute = Watch_Face_Preview_Set.Time.Minutes;
            int SFsecond = Watch_Face_Preview_Set.Time.Seconds;
            DrawTime(gPanel, null, null, null, userControl_SystemFont_GroupTime, SFhour, SFminute, SFsecond, BBorder, false);
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
            if (radioButton_ZeppE.Checked)
            {
                centerX = 208;
                centerY = 208;
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

            src.Dispose();
            TimeEnd:

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
                if (radioButton_ZeppE.Checked)
                {
                    mask = new Bitmap(Application.StartupPath + @"\Mask\mask_zepp_e.png");
                }
                mask = FormColor(mask);
                gPanel.DrawImage(mask, new Rectangle(0, 0, mask.Width, mask.Height));
                mask.Dispose();
            }

            FormText();
            Logger.WriteLine("* PreviewToBitmap (end)");
        }

        private void DrawActivity(Graphics gPanel, UserControl_text userPanel_text, UserControl_text userPanel_textGoal, 
            UserControl_hand userPanel_hand, UserControl_scaleCircle userPanel_scaleCircle,
            UserControl_scaleLinear userPanel_scaleLinear, UserControl_SystemFont_Group userControl_SystemFont_Group, 
            UserControl_icon userControl_icon, float value, int value_lenght, int goal, float progress, 
            bool  BBorder, bool showProgressArea, bool showCentrHend, string activity,
            bool ActivityGoal_Calories = false)
        {
            if (progress > 1) progress = 1;
            Bitmap src = new Bitmap(1, 1);
            UserControl_SystemFont userControl_SystemFont = userControl_SystemFont_Group.userControl_SystemFont;
            UserControl_FontRotate userControl_FontRotate = userControl_SystemFont_Group.userControl_FontRotate;
            UserControl_SystemFont_weather userControl_SystemFontGoal = userControl_SystemFont_Group.userControl_SystemFont_goal;
            UserControl_FontRotate_weather userControl_FontRotateGoal = userControl_SystemFont_Group.userControl_FontRotate_goal;

            // круговая шкала
            if (userPanel_scaleCircle!= null && userPanel_scaleCircle.checkBox_scaleCircle_Use.Checked)
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
                float EndAngle = (float)(numericUpDown_endAngle.Value - numericUpDown_startAngle.Value);
                Color color = userPanel_scaleCircle.comboBoxGetColor();
                int lineCap = userPanel_scaleCircle.comboBoxGetFlatness();
                if (radioButton_image.Checked)
                {
                    if (imageIndex >= 0)
                    {
                        DrawScaleCircle_image(gPanel, x, y, radius, width, lineCap, StartAngle, EndAngle, progress,
                         imageIndex, imageBackground, showProgressArea);
                    }
                }
                else
                {
                    DrawScaleCircle(gPanel, x, y, radius, width, lineCap, StartAngle, EndAngle, progress,
                        color, imageBackground, showProgressArea);
                }
            }

            // линейная шкала
            if (userPanel_scaleLinear != null && userPanel_scaleLinear.checkBox_scaleLinear_Use.Checked)
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
                int lineCap = userPanel_scaleLinear.comboBoxGetFlatness();

                if (radioButton_image.Checked)
                {
                    if (imageIndex >= 0)
                    {
                        DrawScaleLinearPointer_image(gPanel, x, y, length, width, progress, imageIndex, lineCap, pointerIndex, backgroundIndex, showProgressArea);
                    }
                }
                else
                {
                    DrawScaleLinearPointer(gPanel, x, y, length, width, progress, color, lineCap, pointerIndex, backgroundIndex, showProgressArea);
                }
            }

            // надпись
            int ActivityType = 0;
            if (activity == "ActivityGoal") ActivityType = 17;
            if (activity == "Humidity") ActivityType = 11;
            int goal_offsetX = -1;
            int goal_offsetY = -1;
            int goal_spasing = 0;
            if (userPanel_text != null && userPanel_text.checkBox_Use.Checked)
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
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    goal_offsetY = y;
                    int spasing = (int)numericUpDown_spacing.Value;
                    goal_spasing = spasing;
                    int alignment = userPanel_text.comboBoxGetSelectedIndexAlignment();
                    bool addZero = checkBox_add_zero.Checked;
                    int separator_index = userPanel_text.comboBoxGetSelectedIndexUnit();
                    goal_offsetX = Draw_dagital_text(gPanel, imageIndex, x, y,
                        spasing, alignment, (int)value, addZero, value_lenght, separator_index, BBorder, ActivityType);

                    if (unit >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[unit]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // надпись (цель)
            if (userPanel_textGoal != null && userPanel_textGoal.checkBox_Use.Checked)
            {
                int imageIndex = userPanel_textGoal.comboBoxGetSelectedIndexImage();
                int unit = userPanel_textGoal.comboBoxGetSelectedIndexIcon();
                 NumericUpDown numericUpDownX = userPanel_textGoal.numericUpDown_imageX;
                NumericUpDown numericUpDownY = userPanel_textGoal.numericUpDown_imageY;
                NumericUpDown numericUpDown_unitX = userPanel_textGoal.numericUpDown_iconX;
                NumericUpDown numericUpDown_unitY = userPanel_textGoal.numericUpDown_iconY;
                NumericUpDown numericUpDown_spacing = userPanel_textGoal.numericUpDown_spacing;
                CheckBox checkBox_add_zero = userPanel_textGoal.checkBox_addZero;
                CheckBox checkBox_follow = userPanel_textGoal.checkBox_follow;

                if (imageIndex >= 0)
                {
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spacing = (int)numericUpDown_spacing.Value;
                    int alignment = userPanel_textGoal.comboBoxGetSelectedIndexAlignment();
                    //bool addZero = checkBox_add_zero.Checked;
                    bool follow = checkBox_follow.Checked;
                    int separator_index = userPanel_textGoal.comboBoxGetSelectedIndexUnit();
                    if (follow && goal_offsetX > -1)
                    {
                        x = goal_offsetX;
                        alignment = 0;
                        y = goal_offsetY;
                        spacing = goal_spasing;
                    }
                    Draw_dagital_text(gPanel, imageIndex, x, y,
                        spacing, alignment, goal, false, value_lenght, separator_index, BBorder);

                    if (unit >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[unit]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // надпись системным шрифтом
            if (userControl_SystemFont != null && userControl_SystemFont.checkBox_Use.Checked)
            {
                NumericUpDown numericUpDownX = userControl_SystemFont.numericUpDown_SystemFontX;
                NumericUpDown numericUpDownY = userControl_SystemFont.numericUpDown_SystemFontY;
                NumericUpDown numericUpDown_size = userControl_SystemFont.numericUpDown_SystemFont_size;
                NumericUpDown numericUpDown_angle = userControl_SystemFont.numericUpDown_SystemFont_angle;
                NumericUpDown numericUpDown_spacing = userControl_SystemFont.numericUpDown_SystemFont_spacing;
                //CheckBox checkBox_follow = userControl_SystemFont.checkBox_follow;
                CheckBox checkBox_add_zero = userControl_SystemFont.checkBox_addZero;
                CheckBox checkBox_separator = userControl_SystemFont.checkBox_separator;

                //int imageIndex = comboBox_image.SelectedIndex;
                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                int size = (int)numericUpDown_size.Value;
                int angle = (int)numericUpDown_angle.Value;
                int spasing = (int)numericUpDown_spacing.Value;
                int unitCheck = userControl_SystemFont.checkBoxGetUnit();
                if (activity == "Battery" || activity == "Humidity") unitCheck = 1;
                //bool follow = checkBox_follow.Checked;
                bool addZero = checkBox_add_zero.Checked; 
                bool separator = checkBox_separator.Checked;
                string sValue = value.ToString();
                if(activity == "Distance")
                {
                    string decimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                    if (sValue.IndexOf(decimalSeparator) < 0) sValue = sValue + decimalSeparator;
                    while (sValue.IndexOf(decimalSeparator) > sValue.Length - 3)
                    {
                        sValue = sValue + "0";
                    }
                }
                if (addZero)
                {
                    while (sValue.Length < value_lenght)
                    {
                        sValue = "0" + sValue;
                    }
                }
                sValue = sValue + UnitName(activity, unitCheck);
                if(separator) sValue = sValue + "/";
                Color color = userControl_SystemFont.comboBoxGetColor();

                CheckBox checkBox_follow = userControl_SystemFontGoal.checkBox_follow;
                checkBox_separator = userControl_SystemFontGoal.checkBox_separator;
                bool follow = checkBox_follow.Checked;
                if (follow)
                {
                    int goal_unitCheck = userControl_SystemFontGoal.checkBoxGetUnit();
                    bool goal_separator = checkBox_separator.Checked;
                    string goal_sValue = goal.ToString();
                    //if (addZero)
                    //{
                    //    while (sValue.Length < value_lenght)
                    //    {
                    //        sValue = "0" + sValue;
                    //    }
                    //}
                    goal_sValue = goal_sValue + UnitName(activity, goal_unitCheck);
                    if (goal_separator && goal_unitCheck == -1) goal_sValue = goal_sValue + "/";
                    sValue = sValue + goal_sValue;

                }

                Draw_text(gPanel, x, y, size, spasing, color, angle, sValue, BBorder);
            }

            // надпись системным шрифтом (цель)
            if (userControl_SystemFontGoal != null && userControl_SystemFontGoal.checkBox_Use.Checked)
            {
                NumericUpDown numericUpDownX = userControl_SystemFontGoal.numericUpDown_SystemFontX;
                NumericUpDown numericUpDownY = userControl_SystemFontGoal.numericUpDown_SystemFontY;
                NumericUpDown numericUpDown_size = userControl_SystemFontGoal.numericUpDown_SystemFont_size;
                NumericUpDown numericUpDown_angle = userControl_SystemFontGoal.numericUpDown_SystemFont_angle;
                NumericUpDown numericUpDown_spacing = userControl_SystemFontGoal.numericUpDown_SystemFont_spacing;
                CheckBox checkBox_follow = userControl_SystemFontGoal.checkBox_follow;
                CheckBox checkBox_add_zero = userControl_SystemFontGoal.checkBox_addZero;
                CheckBox checkBox_separator = userControl_SystemFontGoal.checkBox_separator;

                bool follow = checkBox_follow.Checked;
                if (!follow)
                {
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int size = (int)numericUpDown_size.Value;
                    int angle = (int)numericUpDown_angle.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int unitCheck = userControl_SystemFontGoal.checkBoxGetUnit();
                    if (activity == "Battery" || activity == "Humidity") unitCheck = 1;
                    bool addZero = checkBox_add_zero.Checked;
                    bool separator = checkBox_separator.Checked;
                    string sValue = goal.ToString();
                    if (addZero)
                    {
                        while (sValue.Length < value_lenght)
                        {
                            sValue = "0" + sValue;
                        }
                    }
                    sValue = sValue + UnitName(activity, unitCheck);
                    if (separator) sValue = sValue + "/";
                    Color color = userControl_SystemFontGoal.comboBoxGetColor();
                    Draw_text(gPanel, x, y, size, spasing, color, angle, sValue, BBorder); 
                }
            }

            // надпись системным шрифтом по окружности
            if (userControl_FontRotate != null && userControl_FontRotate.checkBox_Use.Checked)
            {
                NumericUpDown numericUpDownX = userControl_FontRotate.numericUpDown_FontRotateX;
                NumericUpDown numericUpDownY = userControl_FontRotate.numericUpDown_FontRotateY;
                NumericUpDown numericUpDown_size = userControl_FontRotate.numericUpDown_FontRotate_size;
                NumericUpDown numericUpDown_angle = userControl_FontRotate.numericUpDown_FontRotate_angle;
                NumericUpDown numericUpDown_radius = userControl_FontRotate.numericUpDown_FontRotate_radius;
                NumericUpDown numericUpDown_spacing = userControl_FontRotate.numericUpDown_FontRotate_spacing;
                //CheckBox checkBox_follow = userControl_FontRotate.checkBox_follow;
                CheckBox checkBox_add_zero = userControl_FontRotate.checkBox_addZero;
                CheckBox checkBox_separator = userControl_FontRotate.checkBox_separator;

                //int imageIndex = comboBox_image.SelectedIndex;
                int x = (int)numericUpDownX.Value;
                int y = (int)numericUpDownY.Value;
                int size = (int)numericUpDown_size.Value;
                int angle = (int)numericUpDown_angle.Value;
                int radius = (int)numericUpDown_radius.Value;
                int spasing = (int)numericUpDown_spacing.Value;
                int unitCheck = userControl_FontRotate.checkBoxGetUnit();
                if (activity == "Battery" || activity == "Humidity") unitCheck = 1;
                //bool follow = checkBox_follow.Checked;
                bool addZero = checkBox_add_zero.Checked;
                bool separator = checkBox_separator.Checked;
                int rotate_direction = userControl_FontRotate.radioButtonGetRotateDirection();
                string sValue = value.ToString();
                if (addZero)
                {
                    while (sValue.Length < value_lenght)
                    {
                        sValue = "0" + sValue;
                    }
                }
                sValue = sValue + UnitName(activity, unitCheck);
                if (separator) sValue = sValue + "/";
                Color color = userControl_FontRotate.comboBoxGetColor();

                CheckBox checkBox_follow = userControl_FontRotateGoal.checkBox_follow;
                checkBox_separator = userControl_FontRotateGoal.checkBox_separator;
                bool follow = checkBox_follow.Checked;
                if (follow)
                {
                    int goal_unitCheck = userControl_FontRotateGoal.checkBoxGetUnit();
                    bool goal_useparator = checkBox_separator.Checked;
                    string goal_sValue = goal.ToString();
                    //if (addZero)
                    //{
                    //    while (sValue.Length < value_lenght)
                    //    {
                    //        sValue = "0" + sValue;
                    //    }
                    //}
                    goal_sValue = goal_sValue + UnitName(activity, goal_unitCheck);
                    if (goal_useparator && goal_unitCheck == -1) goal_sValue = goal_sValue + "/";
                    sValue = sValue + goal_sValue;
                }


                Draw_text_rotate(gPanel, x, y, radius, size, spasing, color, angle, rotate_direction,
                    sValue, BBorder);
            }

            // надпись системным шрифтом по окружности (цель)
            if (userControl_FontRotateGoal != null && userControl_FontRotateGoal.checkBox_Use.Checked)
            {
                NumericUpDown numericUpDownX = userControl_FontRotateGoal.numericUpDown_FontRotateX;
                NumericUpDown numericUpDownY = userControl_FontRotateGoal.numericUpDown_FontRotateY;
                NumericUpDown numericUpDown_size = userControl_FontRotateGoal.numericUpDown_FontRotate_size;
                NumericUpDown numericUpDown_angle = userControl_FontRotateGoal.numericUpDown_FontRotate_angle;
                NumericUpDown numericUpDown_radius = userControl_FontRotateGoal.numericUpDown_FontRotate_radius;
                NumericUpDown numericUpDown_spacing = userControl_FontRotateGoal.numericUpDown_FontRotate_spacing;
                CheckBox checkBox_follow = userControl_FontRotateGoal.checkBox_follow;
                CheckBox checkBox_add_zero = userControl_FontRotateGoal.checkBox_addZero;
                CheckBox checkBox_separator = userControl_FontRotateGoal.checkBox_separator;

                //int imageIndex = comboBox_image.SelectedIndex;
                bool follow = checkBox_follow.Checked;
                if (!follow)
                {
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int size = (int)numericUpDown_size.Value;
                    int angle = (int)numericUpDown_angle.Value;
                    int radius = (int)numericUpDown_radius.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int unitCheck = userControl_FontRotateGoal.checkBoxGetUnit();
                    if (activity == "Battery" || activity == "Humidity") unitCheck = 1;
                    bool addZero = checkBox_add_zero.Checked;
                    bool separator = checkBox_separator.Checked;
                    int rotate_direction = userControl_FontRotateGoal.radioButtonGetRotateDirection();
                    string sValue = goal.ToString();
                    //if (addZero)
                    //{
                    //    while (sValue.Length < value_lenght)
                    //    {
                    //        sValue = "0" + sValue;
                    //    }
                    //}
                    sValue = sValue + UnitName(activity, unitCheck);
                    if (separator) sValue = sValue + "/";
                    Color color = userControl_FontRotateGoal.comboBoxGetColor();
                    Draw_text_rotate(gPanel, x, y, radius, size, spasing, color, angle, rotate_direction,
                        sValue, BBorder); 
                }
            }

            // стрелочный указатель
            if (userPanel_hand != null && userPanel_hand.checkBox_hand_Use.Checked)
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

                    float angle = startAngle + progress * (endAngle - startAngle);
                    //if (Watch_Face_Preview_Set.Activity.Steps > Watch_Face_Preview_Set.Activity.StepsGoal) angle = endAngle;
                    DrawAnalogClock(gPanel, x, y, offsetX, offsetY, image_index, angle, showCentrHend);

                    if (imageCentr >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[imageCentr]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDownX_centr.Value,
                            (int)numericUpDownY_centr.Value, src.Width, src.Height));
                    }
                }
            }

            // иконка
            if (userControl_icon != null && userControl_icon.checkBox_icon_Use.Checked)
            {
                int imageIndex = userControl_icon.comboBoxGetSelectedIndexImage();
                if (activity == "ActivityGoal" && ActivityGoal_Calories)
                {
                    if (userControl_icon.comboBoxGetSelectedIndexImage2() >= 0)
                    {
                    imageIndex = userControl_icon.comboBoxGetSelectedIndexImage2();
                    }
                }
                if (imageIndex >= 0)
                {
                    NumericUpDown numericUpDownX = userControl_icon.numericUpDown_iconX;
                    NumericUpDown numericUpDownY = userControl_icon.numericUpDown_iconY;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;

                    src = OpenFileStream(ListImagesFullName[imageIndex]);
                    gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                }
            }

            src.Dispose();
        }

        private void DrawWeather(Graphics gPanel, UserControl_pictures_weather userPanel_pictures,
            UserControl_text_weather userPanel_text, UserControl_text_weather userPanel_textMin, 
            UserControl_text_weather userPanel_textMax, UserControl_SystemFont_GroupWeather userControl_SystemFont_Group,
            UserControl_icon userControl_icon, int value, int value_min, int value_max, int icon_index, 
            bool BBorder, bool showTemperature, bool weatherAlignmentFix)
        {
            Bitmap src = new Bitmap(1, 1);

            UserControl_SystemFont userControl_SystemFont_Current = userControl_SystemFont_Group.userControl_SystemFont_weather_Current;
            UserControl_SystemFont userControl_SystemFont_Min = userControl_SystemFont_Group.userControl_SystemFont_weather_Min;
            UserControl_SystemFont userControl_SystemFont_Max = userControl_SystemFont_Group.userControl_SystemFont_weather_Max;

            UserControl_FontRotate userControl_FontRotate_Current = userControl_SystemFont_Group.userControl_FontRotate_weather_Current;
            UserControl_FontRotate userControl_FontRotate_Min = userControl_SystemFont_Group.userControl_FontRotate_weather_Min;
            UserControl_FontRotate userControl_FontRotate_Max = userControl_SystemFont_Group.userControl_FontRotate_weather_Max;

            // если имеется иконка
            int centr_alignment = -1;
            
            // погода картинками
            if (weatherAlignmentFix)
            {
                if (userPanel_pictures.checkBox_pictures_Use.Checked)
                {
                    if (userPanel_pictures.comboBoxGetSelectedIndexImage() >= 0)
                    {
                        NumericUpDown numericUpDownX = userPanel_pictures.numericUpDown_picturesX;
                        NumericUpDown numericUpDownY = userPanel_pictures.numericUpDown_picturesY;

                        int x = (int)numericUpDownX.Value;
                        int y = (int)numericUpDownY.Value;
                        int offSet = icon_index;
                        //int offSet = Watch_Face_Preview_Set.Weather.Icon;
                        if (offSet < 0) offSet = 25;
                        int imageIndex = userPanel_pictures.comboBoxGetSelectedIndexImage() + offSet;

                        if (imageIndex < ListImagesFullName.Count)
                        {
                            src = OpenFileStream(ListImagesFullName[imageIndex]);
                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                            centr_alignment = x + src.Width / 2;
                        }
                    }
                } 
            }


            // погода надписью
            if (userPanel_text.checkBox_Use.Checked)
            {
                if (userPanel_text.comboBoxGetSelectedIndexImage() >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_text.numericUpDown_imageX;
                    NumericUpDown numericUpDownY = userPanel_text.numericUpDown_imageY;
                    NumericUpDown numericUpDown_unitX = userPanel_text.numericUpDown_iconX;
                    NumericUpDown numericUpDown_unitY = userPanel_text.numericUpDown_iconY;
                    NumericUpDown numericUpDown_spacing = userPanel_text.numericUpDown_spacing;
                    CheckBox checkBox_addZero = userPanel_text.checkBox_addZero;

                    int imageIndex = userPanel_text.comboBoxGetSelectedIndexImage();
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = userPanel_text.comboBoxGetSelectedIndexAlignment();
                    bool addZero = checkBox_addZero.Checked;
                    //int value = Watch_Face_Preview_Set.Weather.Temperature;
                    int separator_index = userPanel_text.comboBoxGetSelectedIndexUnit();
                    int imageError_index = userPanel_text.comboBoxGetSelectedIndexImageError();
                    int imageMinus_index = userPanel_text.comboBoxGetSelectedIndexImageDecimalPointOrMinus();

                    if (showTemperature)
                    {
                        Draw_weather_text(gPanel, imageIndex, x, y,
                                        spasing, alignment, value, addZero, imageMinus_index, separator_index, BBorder,
                                        -1, false, centr_alignment);
                    }
                    else if (imageError_index >= 0)
                    {
                        //src = OpenFileStream(ListImagesFullName[imageError_index]);
                        //gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));

                        Draw_weather_text(gPanel, imageIndex, x, y,
                                        spasing, alignment, value, addZero, imageMinus_index, separator_index,
                                        BBorder, imageError_index, !showTemperature, centr_alignment);
                    }

                    if (userPanel_text.comboBoxGetSelectedIndexIcon() >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[userPanel_text.comboBoxGetSelectedIndexIcon()]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // минимальная температура надписью
            int Temperature_offsetX = -1;
            int Temperature_offsetY = -1;
            int spasing_offset = 0;
            if (userPanel_textMin.checkBox_Use.Checked)
            {
                if (userPanel_textMin.comboBoxGetSelectedIndexImage() >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_textMin.numericUpDown_imageX;
                    NumericUpDown numericUpDownY = userPanel_textMin.numericUpDown_imageY;
                    NumericUpDown numericUpDown_unitX = userPanel_textMin.numericUpDown_iconX;
                    NumericUpDown numericUpDown_unitY = userPanel_textMin.numericUpDown_iconY;
                    NumericUpDown numericUpDown_spacing = userPanel_textMin.numericUpDown_spacing;
                    CheckBox checkBox_addZero = userPanel_textMin.checkBox_addZero;

                    int imageIndex = userPanel_textMin.comboBoxGetSelectedIndexImage();
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    Temperature_offsetY = y;
                    int spasing = (int)numericUpDown_spacing.Value;
                    spasing_offset = spasing;
                    int alignment = userPanel_textMin.comboBoxGetSelectedIndexAlignment();
                    bool addZero = checkBox_addZero.Checked;
                    //int value = Watch_Face_Preview_Set.Weather.TemperatureMin;
                    int separator_index = userPanel_textMin.comboBoxGetSelectedIndexUnit();
                    int imageError_index = userPanel_textMin.comboBoxGetSelectedIndexImageError();
                    int imageMinus_index = userPanel_textMin.comboBoxGetSelectedIndexImageDecimalPointOrMinus();
                    if (showTemperature)
                    {
                        Temperature_offsetX = Draw_weather_text(gPanel, imageIndex, x, y,
                                        spasing, alignment, value_min, addZero, imageMinus_index, separator_index, BBorder);
                    }
                    else if (imageError_index >= 0)
                    {
                        //src = OpenFileStream(ListImagesFullName[imageError_index]);
                        //gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));

                        Temperature_offsetX = Draw_weather_text(gPanel, imageIndex, x, y,
                                        spasing, alignment, value, addZero, imageMinus_index, separator_index,
                                        BBorder, imageError_index, !showTemperature);
                    }

                    if (userPanel_textMin.comboBoxGetSelectedIndexIcon() >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[userPanel_textMin.comboBoxGetSelectedIndexIcon()]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // максимальная температура надписью
            if (userPanel_textMax.checkBox_Use.Checked)
            {
                if (userPanel_textMax.comboBoxGetSelectedIndexImage() >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_textMax.numericUpDown_imageX;
                    NumericUpDown numericUpDownY = userPanel_textMax.numericUpDown_imageY;
                    NumericUpDown numericUpDown_unitX = userPanel_textMax.numericUpDown_iconX;
                    NumericUpDown numericUpDown_unitY = userPanel_textMax.numericUpDown_iconY;
                    NumericUpDown numericUpDown_spacing = userPanel_textMax.numericUpDown_spacing;
                    CheckBox checkBox_addZero = userPanel_textMax.checkBox_addZero;
                    CheckBox checkBox_follow = userPanel_textMax.checkBox_follow;

                    int imageIndex = userPanel_textMax.comboBoxGetSelectedIndexImage();
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = userPanel_textMax.comboBoxGetSelectedIndexAlignment();
                    bool addZero = checkBox_addZero.Checked;
                    //int value = Watch_Face_Preview_Set.Weather.TemperatureMax;
                    int separator_index = userPanel_textMax.comboBoxGetSelectedIndexUnit();
                    int imageError_index = userPanel_textMax.comboBoxGetSelectedIndexImageError();
                    int imageMinus_index = userPanel_textMax.comboBoxGetSelectedIndexImageDecimalPointOrMinus();

                    if (checkBox_follow.Checked && Temperature_offsetX > -1)
                    {
                        x = Temperature_offsetX;
                        alignment = 0;
                        y = Temperature_offsetY;
                        spasing = spasing_offset;
                    }

                    if (showTemperature)
                    {
                        Draw_weather_text(gPanel, imageIndex, x, y,
                                        spasing, alignment, value_max, addZero, imageMinus_index, separator_index, BBorder);
                    }
                    else if (imageError_index >= 0)
                    {
                        //src = OpenFileStream(ListImagesFullName[imageError_index]);
                        //gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));

                        Draw_weather_text(gPanel, imageIndex, x, y,
                                        spasing, alignment, value, addZero, imageMinus_index, separator_index,
                                        BBorder, imageError_index, !showTemperature);
                    }

                    if (userPanel_textMax.comboBoxGetSelectedIndexIcon() >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[userPanel_textMax.comboBoxGetSelectedIndexIcon()]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            string sValue_cerent = "";
            string sValue_min = "";
            string sValue_max = "";
            bool addZeroSF;
            bool separator;
            bool follow_min = false;
            bool follow_max = false;
            CheckBox checkBox_separator;

            #region текущая температура
            if (userControl_SystemFont_Current != null && userControl_SystemFont_Current.checkBox_Use.Checked)
            {
                sValue_cerent = value.ToString();
                addZeroSF = userControl_SystemFont_Current.checkBox_addZero.Checked;
                if (addZeroSF)
                {
                    while (sValue_cerent.Length < 2)
                    {
                        sValue_cerent = "0" + sValue_cerent;
                    }
                }
                sValue_cerent = sValue_cerent + UnitName("Weather", 1);
                checkBox_separator = userControl_SystemFont_Current.checkBox_separator;
                separator = checkBox_separator.Checked;
                if (separator) sValue_cerent = sValue_cerent + "/"; 
            }
            #endregion

            #region минимальная температура
            if (userControl_SystemFont_Min != null && userControl_SystemFont_Min.checkBox_Use.Checked)
            {
                sValue_min = value_min.ToString();
                addZeroSF = userControl_SystemFont_Min.checkBox_addZero.Checked;
                if (addZeroSF)
                {
                    while (sValue_min.Length < 2)
                    {
                        sValue_min = "0" + sValue_min;
                    }
                }
                sValue_min = sValue_min + UnitName("Weather", 1);
                checkBox_separator = userControl_SystemFont_Min.checkBox_separator;
                separator = checkBox_separator.Checked;
                if (separator) sValue_min = sValue_min + "/";
                follow_min = userControl_SystemFont_Min.checkBox_follow.Checked; 
            }
            #endregion

            #region максимальная температура
            if (userControl_SystemFont_Max != null && userControl_SystemFont_Max.checkBox_Use.Checked)
            {
                sValue_max = value_max.ToString();
                addZeroSF = userControl_SystemFont_Max.checkBox_addZero.Checked;
                if (addZeroSF)
                {
                    while (sValue_max.Length < 2)
                    {
                        sValue_max = "0" + sValue_max;
                    }
                }
                sValue_max = sValue_max + UnitName("Weather", 1);
                checkBox_separator = userControl_SystemFont_Max.checkBox_separator;
                separator = checkBox_separator.Checked;
                if (separator) sValue_max = sValue_max + "/";
                follow_max = userControl_SystemFont_Max.checkBox_follow.Checked; 
            }
            #endregion

            if (follow_max)
            {
                if (sValue_min.Length>0)
                {
                    sValue_min = sValue_min + sValue_max;
                    sValue_max = "";
                }
                else if(sValue_cerent.Length > 0)
                {
                    sValue_cerent = sValue_cerent + sValue_max;
                    sValue_max = "";
                }
            }
            //else sValue_max = sValue_cerent + sValue_min + sValue_max;

            if (follow_min)
            {
                sValue_cerent = sValue_cerent + sValue_min;
                sValue_min = "";
            }
            //else sValue_min = sValue_cerent + sValue_min;

            // надпись системным шрифтом
            if (userControl_SystemFont_Current != null && userControl_SystemFont_Current.checkBox_Use.Checked)
            {
                if (sValue_cerent.Length>0)
                {
                    NumericUpDown numericUpDownX = userControl_SystemFont_Current.numericUpDown_SystemFontX;
                    NumericUpDown numericUpDownY = userControl_SystemFont_Current.numericUpDown_SystemFontY;
                    NumericUpDown numericUpDown_size = userControl_SystemFont_Current.numericUpDown_SystemFont_size;
                    NumericUpDown numericUpDown_angle = userControl_SystemFont_Current.numericUpDown_SystemFont_angle;
                    NumericUpDown numericUpDown_spacing = userControl_SystemFont_Current.numericUpDown_SystemFont_spacing;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int size = (int)numericUpDown_size.Value;
                    int angle = (int)numericUpDown_angle.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    Color color = userControl_SystemFont_Current.comboBoxGetColor();

                    Draw_text(gPanel, x, y, size, spasing, color, angle, sValue_cerent, BBorder); 
                }
            }

            // надпись системным шрифтом (мин)
            if (userControl_SystemFont_Min != null && userControl_SystemFont_Min.checkBox_Use.Checked)
            {
                if (sValue_min.Length>0)
                {
                    NumericUpDown numericUpDownX = userControl_SystemFont_Min.numericUpDown_SystemFontX;
                    NumericUpDown numericUpDownY = userControl_SystemFont_Min.numericUpDown_SystemFontY;
                    NumericUpDown numericUpDown_size = userControl_SystemFont_Min.numericUpDown_SystemFont_size;
                    NumericUpDown numericUpDown_angle = userControl_SystemFont_Min.numericUpDown_SystemFont_angle;
                    NumericUpDown numericUpDown_spacing = userControl_SystemFont_Min.numericUpDown_SystemFont_spacing;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int size = (int)numericUpDown_size.Value;
                    int angle = (int)numericUpDown_angle.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    Color color = userControl_SystemFont_Min.comboBoxGetColor();

                    Draw_text(gPanel, x, y, size, spasing, color, angle, sValue_min, BBorder); 
                }
            }

            // надпись системным шрифтом (макс)
            if (userControl_SystemFont_Max != null && userControl_SystemFont_Max.checkBox_Use.Checked)
            {
                if (sValue_max.Length>0)
                {
                    NumericUpDown numericUpDownX = userControl_SystemFont_Max.numericUpDown_SystemFontX;
                    NumericUpDown numericUpDownY = userControl_SystemFont_Max.numericUpDown_SystemFontY;
                    NumericUpDown numericUpDown_size = userControl_SystemFont_Max.numericUpDown_SystemFont_size;
                    NumericUpDown numericUpDown_angle = userControl_SystemFont_Max.numericUpDown_SystemFont_angle;
                    NumericUpDown numericUpDown_spacing = userControl_SystemFont_Max.numericUpDown_SystemFont_spacing;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int size = (int)numericUpDown_size.Value;
                    int angle = (int)numericUpDown_angle.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    Color color = userControl_SystemFont_Max.comboBoxGetColor();

                    Draw_text(gPanel, x, y, size, spasing, color, angle, sValue_max, BBorder);
                }
            }


            sValue_cerent = "";
            sValue_min = "";
            sValue_max = "";
            follow_min = false;
            follow_max = false;

            #region текущая температура
            if (userControl_FontRotate_Current != null && userControl_FontRotate_Current.checkBox_Use.Checked)
            {
                sValue_cerent = value.ToString();
                addZeroSF = userControl_FontRotate_Current.checkBox_addZero.Checked;
                if (addZeroSF)
                {
                    while (sValue_cerent.Length < 2)
                    {
                        sValue_cerent = "0" + sValue_cerent;
                    }
                }
                sValue_cerent = sValue_cerent + UnitName("Weather", 1);
                checkBox_separator = userControl_FontRotate_Current.checkBox_separator;
                separator = checkBox_separator.Checked;
                if (separator) sValue_cerent = sValue_cerent + "/";
            }
            #endregion

            #region минимальная температура
            if (userControl_FontRotate_Min != null && userControl_FontRotate_Min.checkBox_Use.Checked)
            {
                sValue_min = value_min.ToString();
                addZeroSF = userControl_FontRotate_Min.checkBox_addZero.Checked;
                if (addZeroSF)
                {
                    while (sValue_min.Length < 2)
                    {
                        sValue_min = "0" + sValue_min;
                    }
                }
                sValue_min = sValue_min + UnitName("Weather", 1);
                checkBox_separator = userControl_FontRotate_Min.checkBox_separator;
                separator = checkBox_separator.Checked;
                if (separator) sValue_min = sValue_min + "/";
                follow_min = userControl_FontRotate_Min.checkBox_follow.Checked;
            }
            #endregion

            #region максимальная температура
            if (userControl_FontRotate_Max != null && userControl_FontRotate_Max.checkBox_Use.Checked)
            {
                sValue_max = value_max.ToString();
                addZeroSF = userControl_FontRotate_Max.checkBox_addZero.Checked;
                if (addZeroSF)
                {
                    while (sValue_max.Length < 2)
                    {
                        sValue_max = "0" + sValue_max;
                    }
                }
                sValue_max = sValue_max + UnitName("Weather", 1);
                checkBox_separator = userControl_FontRotate_Max.checkBox_separator;
                separator = checkBox_separator.Checked;
                if (separator) sValue_max = sValue_max + "/";
                follow_max = userControl_FontRotate_Max.checkBox_follow.Checked;
            }
            #endregion

            if (follow_max)
            {
                if (sValue_min.Length > 0)
                {
                    sValue_min = sValue_min + sValue_max;
                    sValue_max = "";
                }
                else if (sValue_cerent.Length > 0)
                {
                    sValue_cerent = sValue_cerent + sValue_max;
                    sValue_max = "";
                }
            }
            //else sValue_max = sValue_cerent + sValue_min + sValue_max;

            if (follow_min)
            {
                sValue_cerent = sValue_cerent + sValue_min;
                sValue_min = "";
            }
            //else sValue_min = sValue_cerent + sValue_min;

            // надпись системным шрифтом по окружности
            if (userControl_FontRotate_Current != null && userControl_FontRotate_Current.checkBox_Use.Checked)
            {
                if (sValue_cerent.Length>0)
                {
                    NumericUpDown numericUpDownX = userControl_FontRotate_Current.numericUpDown_FontRotateX;
                    NumericUpDown numericUpDownY = userControl_FontRotate_Current.numericUpDown_FontRotateY;
                    NumericUpDown numericUpDown_size = userControl_FontRotate_Current.numericUpDown_FontRotate_size;
                    NumericUpDown numericUpDown_angle = userControl_FontRotate_Current.numericUpDown_FontRotate_angle;
                    NumericUpDown numericUpDown_radius = userControl_FontRotate_Current.numericUpDown_FontRotate_radius;
                    NumericUpDown numericUpDown_spacing = userControl_FontRotate_Current.numericUpDown_FontRotate_spacing;
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int size = (int)numericUpDown_size.Value;
                    int angle = (int)numericUpDown_angle.Value;
                    int radius = (int)numericUpDown_radius.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int rotate_direction = userControl_FontRotate_Current.radioButtonGetRotateDirection();
                    Color color = userControl_FontRotate_Current.comboBoxGetColor();

                    Draw_text_rotate(gPanel, x, y, radius, size, spasing, color, angle, rotate_direction,
                                sValue_cerent, BBorder); 
                }
            }

            // надпись системным шрифтом по окружности (мин)
            if (userControl_FontRotate_Min != null && userControl_FontRotate_Min.checkBox_Use.Checked)
            {
                if (sValue_min.Length>0)
                {
                    NumericUpDown numericUpDownX = userControl_FontRotate_Min.numericUpDown_FontRotateX;
                    NumericUpDown numericUpDownY = userControl_FontRotate_Min.numericUpDown_FontRotateY;
                    NumericUpDown numericUpDown_size = userControl_FontRotate_Min.numericUpDown_FontRotate_size;
                    NumericUpDown numericUpDown_angle = userControl_FontRotate_Min.numericUpDown_FontRotate_angle;
                    NumericUpDown numericUpDown_radius = userControl_FontRotate_Min.numericUpDown_FontRotate_radius;
                    NumericUpDown numericUpDown_spacing = userControl_FontRotate_Min.numericUpDown_FontRotate_spacing;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int size = (int)numericUpDown_size.Value;
                    int angle = (int)numericUpDown_angle.Value;
                    int radius = (int)numericUpDown_radius.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int rotate_direction = userControl_FontRotate_Min.radioButtonGetRotateDirection();
                    Color color = userControl_FontRotate_Min.comboBoxGetColor();

                    Draw_text_rotate(gPanel, x, y, radius, size, spasing, color, angle, rotate_direction,
                                sValue_min, BBorder); 
                }
            }

            // надпись системным шрифтом по окружности (макс)
            if (userControl_FontRotate_Max != null && userControl_FontRotate_Max.checkBox_Use.Checked)
            {
                if (sValue_max.Length>0)
                {
                    NumericUpDown numericUpDownX = userControl_FontRotate_Max.numericUpDown_FontRotateX;
                    NumericUpDown numericUpDownY = userControl_FontRotate_Max.numericUpDown_FontRotateY;
                    NumericUpDown numericUpDown_size = userControl_FontRotate_Max.numericUpDown_FontRotate_size;
                    NumericUpDown numericUpDown_angle = userControl_FontRotate_Max.numericUpDown_FontRotate_angle;
                    NumericUpDown numericUpDown_radius = userControl_FontRotate_Max.numericUpDown_FontRotate_radius;
                    NumericUpDown numericUpDown_spacing = userControl_FontRotate_Max.numericUpDown_FontRotate_spacing;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int size = (int)numericUpDown_size.Value;
                    int angle = (int)numericUpDown_angle.Value;
                    int radius = (int)numericUpDown_radius.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int rotate_direction = userControl_FontRotate_Max.radioButtonGetRotateDirection();
                    Color color = userControl_FontRotate_Max.comboBoxGetColor();

                    Draw_text_rotate(gPanel, x, y, radius, size, spasing, color, angle, rotate_direction,
                        sValue_max, BBorder);
                }
            }
            
            // иконка
            if (userControl_icon != null && userControl_icon.checkBox_icon_Use.Checked)
            {
                int imageIndex = userControl_icon.comboBoxGetSelectedIndexImage();
                
                if (imageIndex >= 0)
                {
                    NumericUpDown numericUpDownX = userControl_icon.numericUpDown_iconX;
                    NumericUpDown numericUpDownY = userControl_icon.numericUpDown_iconY;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;

                    src = OpenFileStream(ListImagesFullName[imageIndex]);
                    gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                }
            }

            // погода картинками
            if (!weatherAlignmentFix)
            {
                if (userPanel_pictures.checkBox_pictures_Use.Checked)
                {
                    if (userPanel_pictures.comboBoxGetSelectedIndexImage() >= 0)
                    {
                        NumericUpDown numericUpDownX = userPanel_pictures.numericUpDown_picturesX;
                        NumericUpDown numericUpDownY = userPanel_pictures.numericUpDown_picturesY;

                        int x = (int)numericUpDownX.Value;
                        int y = (int)numericUpDownY.Value;
                        int offSet = icon_index;
                        //int offSet = Watch_Face_Preview_Set.Weather.Icon;
                        if (offSet < 0) offSet = 25;
                        int imageIndex = userPanel_pictures.comboBoxGetSelectedIndexImage() + offSet;

                        if (imageIndex < ListImagesFullName.Count)
                        {
                            src = OpenFileStream(ListImagesFullName[imageIndex]);
                            gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                        }
                    }
                }
            }

            src.Dispose();
        }

        private void DrawSunrise(Graphics gPanel, UserControl_pictures userPanel_pictures, UserControl_text userPanel_text, UserControl_text userPanel_textMin,
            UserControl_text userPanel_textMax, UserControl_hand userPanel_hand, UserControl_scaleCircle userPanel_scaleCircle,
            UserControl_scaleLinear userPanel_scaleLinear, UserControl_SystemFont_GroupSunrise userControl_SystemFont_Group,
            UserControl_icon userControl_icon, int hour, int minute, bool BBorder, bool showProgressArea, bool showCentrHend)
        {
            TimeSpan time_now = new TimeSpan(hour, minute, 0);
            TimeSpan time_sunrise = new TimeSpan(5, 30, 0);
            TimeSpan time_sunset = new TimeSpan(19, 30, 0);
            TimeSpan day_lenght = time_sunset - time_sunrise;
            TimeSpan day_progress = time_now - time_sunrise;

            float progress = (float)(day_progress.TotalSeconds / day_lenght.TotalSeconds);
            if (progress > 1) progress = 1;
            if (progress < 0) progress = 0;
            Bitmap src = new Bitmap(1, 1);

            UserControl_SystemFont userControl_SystemFont_Current = userControl_SystemFont_Group.userControl_SystemFont_weather_Current;
            UserControl_SystemFont userControl_SystemFont_Min = userControl_SystemFont_Group.userControl_SystemFont_weather_Min;
            UserControl_SystemFont userControl_SystemFont_Max = userControl_SystemFont_Group.userControl_SystemFont_weather_Max;

            UserControl_FontRotate userControl_FontRotate_Current = userControl_SystemFont_Group.userControl_FontRotate_weather_Current;
            UserControl_FontRotate userControl_FontRotate_Min = userControl_SystemFont_Group.userControl_FontRotate_weather_Min;
            UserControl_FontRotate userControl_FontRotate_Max = userControl_SystemFont_Group.userControl_FontRotate_weather_Max;


            // Sunrise картинками
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
                    int offSet = 0;
                    if (progress > 0 && progress < 1) offSet = 1;
                    int imageIndex = userPanel_pictures.comboBoxGetSelectedIndexImage() + offSet;

                    if (imageIndex < ListImagesFullName.Count)
                    {
                        src = OpenFileStream(ListImagesFullName[imageIndex]);
                        gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                    }
                }
            }

            // круговая шкала
            if (userPanel_scaleCircle != null && userPanel_scaleCircle.checkBox_scaleCircle_Use.Checked)
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
                float EndAngle = (float)(numericUpDown_endAngle.Value - numericUpDown_startAngle.Value);
                Color color = userPanel_scaleCircle.comboBoxGetColor();
                int lineCap = userPanel_scaleCircle.comboBoxGetFlatness();
                if (radioButton_image.Checked)
                {
                    if (imageIndex >= 0)
                    {
                        DrawScaleCircle_image(gPanel, x, y, radius, width, lineCap, StartAngle, EndAngle, progress,
                         imageIndex, imageBackground, showProgressArea);
                    }
                }
                else
                {
                    DrawScaleCircle(gPanel, x, y, radius, width, lineCap, StartAngle, EndAngle, progress,
                        color, imageBackground, showProgressArea);
                }
            }

            // линейная шкала
            if (userPanel_scaleLinear != null && userPanel_scaleLinear.checkBox_scaleLinear_Use.Checked)
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
                int lineCap = userPanel_scaleLinear.comboBoxGetFlatness();

                if (radioButton_image.Checked)
                {
                    if (imageIndex >= 0)
                    {
                        DrawScaleLinearPointer_image(gPanel, x, y, length, width, progress, imageIndex, lineCap, pointerIndex, backgroundIndex, showProgressArea);
                    }
                }
                else
                {
                    DrawScaleLinearPointer(gPanel, x, y, length, width, progress, color, lineCap, pointerIndex, backgroundIndex, showProgressArea);
                }
            }

            // рассвет/закат надписью
            if (userPanel_text.checkBox_Use.Checked)
            {
                if (userPanel_text.comboBoxGetSelectedIndexImage() >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_text.numericUpDown_imageX;
                    NumericUpDown numericUpDownY = userPanel_text.numericUpDown_imageY;
                    NumericUpDown numericUpDown_unitX = userPanel_text.numericUpDown_iconX;
                    NumericUpDown numericUpDown_unitY = userPanel_text.numericUpDown_iconY;
                    NumericUpDown numericUpDown_spacing = userPanel_text.numericUpDown_spacing;
                    CheckBox checkBox_addZero = userPanel_text.checkBox_addZero;

                    int imageIndex = userPanel_text.comboBoxGetSelectedIndexImage();
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = userPanel_text.comboBoxGetSelectedIndexAlignment();
                    bool addZero = checkBox_addZero.Checked;
                    int separator_index = userPanel_text.comboBoxGetSelectedIndexUnit();
                    int delimiter_index = userPanel_text.comboBoxGetSelectedIndexImageDecimalPointOrMinus();
                    double value = 5.30;
                    if (progress > 0 && progress < 1) value = 19.30;

                    Draw_dagital_text(gPanel, imageIndex, x, y,
                        spasing, alignment, value, addZero, 4, separator_index,
                        delimiter_index, 2, BBorder);

                    if (userPanel_text.comboBoxGetSelectedIndexIcon() >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[userPanel_text.comboBoxGetSelectedIndexIcon()]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // рассвет надписью
            int Sunrise_offsetX = -1;
            int Sunrise_offsetY = -1;
            int spasing_offset = 0;
            if (userPanel_textMin.checkBox_Use.Checked)
            {
                if (userPanel_textMin.comboBoxGetSelectedIndexImage() >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_textMin.numericUpDown_imageX;
                    NumericUpDown numericUpDownY = userPanel_textMin.numericUpDown_imageY;
                    NumericUpDown numericUpDown_unitX = userPanel_textMin.numericUpDown_iconX;
                    NumericUpDown numericUpDown_unitY = userPanel_textMin.numericUpDown_iconY;
                    NumericUpDown numericUpDown_spacing = userPanel_textMin.numericUpDown_spacing;
                    CheckBox checkBox_addZero = userPanel_textMin.checkBox_addZero;

                    int imageIndex = userPanel_textMin.comboBoxGetSelectedIndexImage();
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    Sunrise_offsetY = y;
                    int spasing = (int)numericUpDown_spacing.Value;
                    spasing_offset = spasing;
                    int alignment = userPanel_textMin.comboBoxGetSelectedIndexAlignment();
                    bool addZero = checkBox_addZero.Checked;
                    int separator_index = userPanel_textMin.comboBoxGetSelectedIndexUnit();
                    int delimiter_index = userPanel_textMin.comboBoxGetSelectedIndexImageDecimalPointOrMinus();

                    Sunrise_offsetX = Draw_dagital_text(gPanel, imageIndex, x, y,
                        spasing, alignment, 5.30, addZero, 4, separator_index,
                        delimiter_index, 2, BBorder);

                    if (userPanel_textMin.comboBoxGetSelectedIndexIcon() >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[userPanel_textMin.comboBoxGetSelectedIndexIcon()]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // закат надписью
            if (userPanel_textMax.checkBox_Use.Checked)
            {
                if (userPanel_textMax.comboBoxGetSelectedIndexImage() >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_textMax.numericUpDown_imageX;
                    NumericUpDown numericUpDownY = userPanel_textMax.numericUpDown_imageY;
                    NumericUpDown numericUpDown_unitX = userPanel_textMax.numericUpDown_iconX;
                    NumericUpDown numericUpDown_unitY = userPanel_textMax.numericUpDown_iconY;
                    NumericUpDown numericUpDown_spacing = userPanel_textMax.numericUpDown_spacing;
                    CheckBox checkBox_addZero = userPanel_textMax.checkBox_addZero;
                    CheckBox checkBox_follow = userPanel_textMax.checkBox_follow;

                    int imageIndex = userPanel_textMax.comboBoxGetSelectedIndexImage();
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = userPanel_textMax.comboBoxGetSelectedIndexAlignment();
                    bool addZero = checkBox_addZero.Checked;
                    int separator_index = userPanel_textMax.comboBoxGetSelectedIndexUnit();
                    int delimiter_index = userPanel_textMax.comboBoxGetSelectedIndexImageDecimalPointOrMinus();

                    if (checkBox_follow.Checked && Sunrise_offsetX > -1)
                    {
                        x = Sunrise_offsetX;
                        alignment = 0;
                        y = Sunrise_offsetY;
                        spasing = spasing_offset;
                    }

                    Draw_dagital_text(gPanel, imageIndex, x, y,
                        spasing, alignment, 19.30, addZero, 4, separator_index,
                        delimiter_index, 2, BBorder);

                    if (userPanel_textMax.comboBoxGetSelectedIndexIcon() >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[userPanel_textMax.comboBoxGetSelectedIndexIcon()]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            string sValue_cerent = "";
            string sValue_min = "";
            string sValue_max = "";
            bool addZeroSF;
            bool separator;
            bool follow_min = false;
            bool follow_max = false;
            CheckBox checkBox_separator;

            #region рассвет/закат
            if (userControl_SystemFont_Current != null && userControl_SystemFont_Current.checkBox_Use.Checked)
            {
                sValue_cerent = "5:30";
                if (progress > 0 && progress < 1) sValue_cerent = "19:30";
                addZeroSF = userControl_SystemFont_Current.checkBox_addZero.Checked;
                if (addZeroSF)
                {
                    while (sValue_cerent.Length < 5)
                    {
                        sValue_cerent = "0" + sValue_cerent;
                    }
                }
                sValue_cerent = sValue_cerent + UnitName("Sunrise", 1);
                checkBox_separator = userControl_SystemFont_Current.checkBox_separator;
                separator = checkBox_separator.Checked;
                if (separator) sValue_cerent = sValue_cerent + "/";
            }
            #endregion

            #region рассвет
            if (userControl_SystemFont_Min != null && userControl_SystemFont_Min.checkBox_Use.Checked)
            {
                sValue_min = "5:30";
                addZeroSF = userControl_SystemFont_Min.checkBox_addZero.Checked;
                if (addZeroSF)
                {
                    while (sValue_min.Length < 5)
                    {
                        sValue_min = "0" + sValue_min;
                    }
                }
                sValue_min = sValue_min + UnitName("Sunrise", 1);
                checkBox_separator = userControl_SystemFont_Min.checkBox_separator;
                separator = checkBox_separator.Checked;
                if (separator) sValue_min = sValue_min + "/";
                follow_min = userControl_SystemFont_Min.checkBox_follow.Checked;
            }
            #endregion

            #region закат
            if (userControl_SystemFont_Max != null && userControl_SystemFont_Max.checkBox_Use.Checked)
            {
                sValue_max = "19:30";
                addZeroSF = userControl_SystemFont_Max.checkBox_addZero.Checked;
                if (addZeroSF)
                {
                    while (sValue_max.Length < 5)
                    {
                        sValue_max = "0" + sValue_max;
                    }
                }
                sValue_max = sValue_max + UnitName("Sunrise", 1);
                checkBox_separator = userControl_SystemFont_Max.checkBox_separator;
                separator = checkBox_separator.Checked;
                if (separator) sValue_max = sValue_max + "/";
                follow_max = userControl_SystemFont_Max.checkBox_follow.Checked;
            }
            #endregion

            if (follow_max)
            {
                if (sValue_min.Length > 0)
                {
                    sValue_min = sValue_min + sValue_max;
                    sValue_max = "";
                }
                else if (sValue_cerent.Length > 0)
                {
                    sValue_cerent = sValue_cerent + sValue_max;
                    sValue_max = "";
                }
            }
            //else sValue_max = sValue_cerent + sValue_min + sValue_max;

            if (follow_min)
            {
                sValue_cerent = sValue_cerent + sValue_min;
                sValue_min = "";
            }
            //else sValue_min = sValue_cerent + sValue_min;

            // надпись системным шрифтом (рассвет/закат)
            if (userControl_SystemFont_Current != null && userControl_SystemFont_Current.checkBox_Use.Checked)
            {
                if (sValue_cerent.Length > 0)
                {
                    NumericUpDown numericUpDownX = userControl_SystemFont_Current.numericUpDown_SystemFontX;
                    NumericUpDown numericUpDownY = userControl_SystemFont_Current.numericUpDown_SystemFontY;
                    NumericUpDown numericUpDown_size = userControl_SystemFont_Current.numericUpDown_SystemFont_size;
                    NumericUpDown numericUpDown_angle = userControl_SystemFont_Current.numericUpDown_SystemFont_angle;
                    NumericUpDown numericUpDown_spacing = userControl_SystemFont_Current.numericUpDown_SystemFont_spacing;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int size = (int)numericUpDown_size.Value;
                    int angle = (int)numericUpDown_angle.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    Color color = userControl_SystemFont_Current.comboBoxGetColor();

                    Draw_text(gPanel, x, y, size, spasing, color, angle, sValue_cerent, BBorder);
                }
            }

            // надпись системным шрифтом (рассвет)
            if (userControl_SystemFont_Min != null && userControl_SystemFont_Min.checkBox_Use.Checked)
            {
                if (sValue_min.Length > 0)
                {
                    NumericUpDown numericUpDownX = userControl_SystemFont_Min.numericUpDown_SystemFontX;
                    NumericUpDown numericUpDownY = userControl_SystemFont_Min.numericUpDown_SystemFontY;
                    NumericUpDown numericUpDown_size = userControl_SystemFont_Min.numericUpDown_SystemFont_size;
                    NumericUpDown numericUpDown_angle = userControl_SystemFont_Min.numericUpDown_SystemFont_angle;
                    NumericUpDown numericUpDown_spacing = userControl_SystemFont_Min.numericUpDown_SystemFont_spacing;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int size = (int)numericUpDown_size.Value;
                    int angle = (int)numericUpDown_angle.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    Color color = userControl_SystemFont_Min.comboBoxGetColor();

                    Draw_text(gPanel, x, y, size, spasing, color, angle, sValue_min, BBorder);
                }
            }

            // надпись системным шрифтом (закат)
            if (userControl_SystemFont_Max != null && userControl_SystemFont_Max.checkBox_Use.Checked)
            {
                if (sValue_max.Length > 0)
                {
                    NumericUpDown numericUpDownX = userControl_SystemFont_Max.numericUpDown_SystemFontX;
                    NumericUpDown numericUpDownY = userControl_SystemFont_Max.numericUpDown_SystemFontY;
                    NumericUpDown numericUpDown_size = userControl_SystemFont_Max.numericUpDown_SystemFont_size;
                    NumericUpDown numericUpDown_angle = userControl_SystemFont_Max.numericUpDown_SystemFont_angle;
                    NumericUpDown numericUpDown_spacing = userControl_SystemFont_Max.numericUpDown_SystemFont_spacing;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int size = (int)numericUpDown_size.Value;
                    int angle = (int)numericUpDown_angle.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    Color color = userControl_SystemFont_Max.comboBoxGetColor();

                    Draw_text(gPanel, x, y, size, spasing, color, angle, sValue_max, BBorder);
                }
            }


            sValue_cerent = "";
            sValue_min = "";
            sValue_max = "";
            follow_min = false;
            follow_max = false;

            #region рассвет/закат
            if (userControl_FontRotate_Current != null && userControl_FontRotate_Current.checkBox_Use.Checked)
            {
                sValue_cerent = "5:30";
                if (progress > 0 && progress < 1) sValue_cerent = "19:30";
                addZeroSF = userControl_FontRotate_Current.checkBox_addZero.Checked;
                if (addZeroSF)
                {
                    while (sValue_cerent.Length < 5)
                    {
                        sValue_cerent = "0" + sValue_cerent;
                    }
                }
                sValue_cerent = sValue_cerent + UnitName("Sunrise", 1);
                checkBox_separator = userControl_FontRotate_Current.checkBox_separator;
                separator = checkBox_separator.Checked;
                if (separator) sValue_cerent = sValue_cerent + "/";
            }
            #endregion

            #region рассвет
            if (userControl_FontRotate_Min != null && userControl_FontRotate_Min.checkBox_Use.Checked)
            {
                sValue_min = "5:30";
                addZeroSF = userControl_FontRotate_Min.checkBox_addZero.Checked;
                if (addZeroSF)
                {
                    while (sValue_min.Length < 5)
                    {
                        sValue_min = "0" + sValue_min;
                    }
                }
                sValue_min = sValue_min + UnitName("Sunrise", 1);
                checkBox_separator = userControl_FontRotate_Min.checkBox_separator;
                separator = checkBox_separator.Checked;
                if (separator) sValue_min = sValue_min + "/";
                follow_min = userControl_FontRotate_Min.checkBox_follow.Checked;
            }
            #endregion

            #region закат
            if (userControl_FontRotate_Max != null && userControl_FontRotate_Max.checkBox_Use.Checked)
            {
                sValue_max = "19:30";
                addZeroSF = userControl_FontRotate_Max.checkBox_addZero.Checked;
                if (addZeroSF)
                {
                    while (sValue_max.Length < 5)
                    {
                        sValue_max = "0" + sValue_max;
                    }
                }
                sValue_max = sValue_max + UnitName("Sunrise", 1);
                checkBox_separator = userControl_FontRotate_Max.checkBox_separator;
                separator = checkBox_separator.Checked;
                if (separator) sValue_max = sValue_max + "/";
                follow_max = userControl_FontRotate_Max.checkBox_follow.Checked;
            }
            #endregion

            if (follow_max)
            {
                if (sValue_min.Length > 0)
                {
                    sValue_min = sValue_min + sValue_max;
                    sValue_max = "";
                }
                else if (sValue_cerent.Length > 0)
                {
                    sValue_cerent = sValue_cerent + sValue_max;
                    sValue_max = "";
                }
            }
            //else sValue_max = sValue_cerent + sValue_min + sValue_max;

            if (follow_min)
            {
                sValue_cerent = sValue_cerent + sValue_min;
                sValue_min = "";
            }
            //else sValue_min = sValue_cerent + sValue_min;

            // надпись системным шрифтом по окружности (рассвет/закат)
            if (userControl_FontRotate_Current != null && userControl_FontRotate_Current.checkBox_Use.Checked)
            {
                if (sValue_cerent.Length > 0)
                {
                    NumericUpDown numericUpDownX = userControl_FontRotate_Current.numericUpDown_FontRotateX;
                    NumericUpDown numericUpDownY = userControl_FontRotate_Current.numericUpDown_FontRotateY;
                    NumericUpDown numericUpDown_size = userControl_FontRotate_Current.numericUpDown_FontRotate_size;
                    NumericUpDown numericUpDown_angle = userControl_FontRotate_Current.numericUpDown_FontRotate_angle;
                    NumericUpDown numericUpDown_radius = userControl_FontRotate_Current.numericUpDown_FontRotate_radius;
                    NumericUpDown numericUpDown_spacing = userControl_FontRotate_Current.numericUpDown_FontRotate_spacing;
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int size = (int)numericUpDown_size.Value;
                    int angle = (int)numericUpDown_angle.Value;
                    int radius = (int)numericUpDown_radius.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int rotate_direction = userControl_FontRotate_Current.radioButtonGetRotateDirection();
                    Color color = userControl_FontRotate_Current.comboBoxGetColor();

                    Draw_text_rotate(gPanel, x, y, radius, size, spasing, color, angle, rotate_direction,
                                sValue_cerent, BBorder);
                }
            }

            // надпись системным шрифтом по окружности (рассвет)
            if (userControl_FontRotate_Min != null && userControl_FontRotate_Min.checkBox_Use.Checked)
            {
                if (sValue_min.Length > 0)
                {
                    NumericUpDown numericUpDownX = userControl_FontRotate_Min.numericUpDown_FontRotateX;
                    NumericUpDown numericUpDownY = userControl_FontRotate_Min.numericUpDown_FontRotateY;
                    NumericUpDown numericUpDown_size = userControl_FontRotate_Min.numericUpDown_FontRotate_size;
                    NumericUpDown numericUpDown_angle = userControl_FontRotate_Min.numericUpDown_FontRotate_angle;
                    NumericUpDown numericUpDown_radius = userControl_FontRotate_Min.numericUpDown_FontRotate_radius;
                    NumericUpDown numericUpDown_spacing = userControl_FontRotate_Min.numericUpDown_FontRotate_spacing;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int size = (int)numericUpDown_size.Value;
                    int angle = (int)numericUpDown_angle.Value;
                    int radius = (int)numericUpDown_radius.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int rotate_direction = userControl_FontRotate_Min.radioButtonGetRotateDirection();
                    Color color = userControl_FontRotate_Min.comboBoxGetColor();

                    Draw_text_rotate(gPanel, x, y, radius, size, spasing, color, angle, rotate_direction,
                                sValue_min, BBorder);
                }
            }

            // надпись системным шрифтом по окружности (закат)
            if (userControl_FontRotate_Max != null && userControl_FontRotate_Max.checkBox_Use.Checked)
            {
                if (sValue_max.Length > 0)
                {
                    NumericUpDown numericUpDownX = userControl_FontRotate_Max.numericUpDown_FontRotateX;
                    NumericUpDown numericUpDownY = userControl_FontRotate_Max.numericUpDown_FontRotateY;
                    NumericUpDown numericUpDown_size = userControl_FontRotate_Max.numericUpDown_FontRotate_size;
                    NumericUpDown numericUpDown_angle = userControl_FontRotate_Max.numericUpDown_FontRotate_angle;
                    NumericUpDown numericUpDown_radius = userControl_FontRotate_Max.numericUpDown_FontRotate_radius;
                    NumericUpDown numericUpDown_spacing = userControl_FontRotate_Max.numericUpDown_FontRotate_spacing;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int size = (int)numericUpDown_size.Value;
                    int angle = (int)numericUpDown_angle.Value;
                    int radius = (int)numericUpDown_radius.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int rotate_direction = userControl_FontRotate_Max.radioButtonGetRotateDirection();
                    Color color = userControl_FontRotate_Max.comboBoxGetColor();

                    Draw_text_rotate(gPanel, x, y, radius, size, spasing, color, angle, rotate_direction,
                        sValue_max, BBorder);
                }
            }

            // стрелочный указатель
            if (userPanel_hand != null && userPanel_hand.checkBox_hand_Use.Checked)
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

                    float angle = startAngle + progress * (endAngle - startAngle);
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

            // иконка
            if (userControl_icon != null && userControl_icon.checkBox_icon_Use.Checked)
            {
                int imageIndex = userControl_icon.comboBoxGetSelectedIndexImage();
                if (imageIndex >= 0)
                {
                    NumericUpDown numericUpDownX = userControl_icon.numericUpDown_iconX;
                    NumericUpDown numericUpDownY = userControl_icon.numericUpDown_iconY;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;

                    src = OpenFileStream(ListImagesFullName[imageIndex]);
                    gPanel.DrawImage(src, new Rectangle(x, y, src.Width, src.Height));
                }
            }

            src.Dispose();
        }


        private void DrawTime(Graphics gPanel, UserControl_text userPanel_textHour, UserControl_text userPanel_textMinute,
            UserControl_text userPanel_textSecond, UserControl_SystemFont_GroupTime userControl_SystemFont_Group, 
            int hour, int minute, int second, bool BBorder, bool AOD)
        {
            
            Bitmap src = new Bitmap(1, 1);

            UserControl_SystemFont userControl_SystemFont_Hour = userControl_SystemFont_Group.userControl_SystemFont_weather_Current;
            UserControl_SystemFont userControl_SystemFont_Minute = userControl_SystemFont_Group.userControl_SystemFont_weather_Min;
            UserControl_SystemFont userControl_SystemFont_Second = userControl_SystemFont_Group.userControl_SystemFont_weather_Max;

            UserControl_FontRotate userControl_FontRotate_Hour = userControl_SystemFont_Group.userControl_FontRotate_weather_Current;
            UserControl_FontRotate userControl_FontRotate_Minute = userControl_SystemFont_Group.userControl_FontRotate_weather_Min;
            UserControl_FontRotate userControl_FontRotate_Second = userControl_SystemFont_Group.userControl_FontRotate_weather_Max;


            // часы надписью
            if (userPanel_textHour != null && userPanel_textHour.checkBox_Use.Checked)
            {
                if (userPanel_textHour.comboBoxGetSelectedIndexImage() >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_textHour.numericUpDown_imageX;
                    NumericUpDown numericUpDownY = userPanel_textHour.numericUpDown_imageY;
                    NumericUpDown numericUpDown_unitX = userPanel_textHour.numericUpDown_iconX;
                    NumericUpDown numericUpDown_unitY = userPanel_textHour.numericUpDown_iconY;
                    NumericUpDown numericUpDown_spacing = userPanel_textHour.numericUpDown_spacing;
                    CheckBox checkBox_addZero = userPanel_textHour.checkBox_addZero;

                    int imageIndex = userPanel_textHour.comboBoxGetSelectedIndexImage();
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int alignment = userPanel_textHour.comboBoxGetSelectedIndexAlignment();
                    bool addZero = checkBox_addZero.Checked;
                    int separator_index = userPanel_textHour.comboBoxGetSelectedIndexUnit();
                    int delimiter_index = userPanel_textHour.comboBoxGetSelectedIndexImageDecimalPointOrMinus();

                    Draw_dagital_text(gPanel, imageIndex, x, y,
                        spasing, alignment, hour, addZero, 2, separator_index,
                        delimiter_index, 0, BBorder);

                    if (userPanel_textHour.comboBoxGetSelectedIndexIcon() >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[userPanel_textHour.comboBoxGetSelectedIndexIcon()]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // минуты надписью
            int _offsetX = -1;
            int _offsetY = -1;
            int spasing_offset = 0;
            if (userPanel_textMinute != null && userPanel_textMinute.checkBox_Use.Checked)
            {
                if (userPanel_textMinute.comboBoxGetSelectedIndexImage() >= 0)
                {
                    NumericUpDown numericUpDownX = userPanel_textMinute.numericUpDown_imageX;
                    NumericUpDown numericUpDownY = userPanel_textMinute.numericUpDown_imageY;
                    NumericUpDown numericUpDown_unitX = userPanel_textMinute.numericUpDown_iconX;
                    NumericUpDown numericUpDown_unitY = userPanel_textMinute.numericUpDown_iconY;
                    NumericUpDown numericUpDown_spacing = userPanel_textMinute.numericUpDown_spacing;
                    CheckBox checkBox_addZero = userPanel_textMinute.checkBox_addZero;

                    int imageIndex = userPanel_textMinute.comboBoxGetSelectedIndexImage();
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    _offsetY = y;
                    int spasing = (int)numericUpDown_spacing.Value;
                    spasing_offset = spasing;
                    int alignment = userPanel_textMinute.comboBoxGetSelectedIndexAlignment();
                    bool addZero = checkBox_addZero.Checked;
                    int separator_index = userPanel_textMinute.comboBoxGetSelectedIndexUnit();
                    int delimiter_index = userPanel_textMinute.comboBoxGetSelectedIndexImageDecimalPointOrMinus();

                    _offsetX = Draw_dagital_text(gPanel, imageIndex, x, y,
                        spasing, alignment, minute, addZero, 2, separator_index,
                        delimiter_index, 0, BBorder);

                    if (userPanel_textMinute.comboBoxGetSelectedIndexIcon() >= 0)
                    {
                        src = OpenFileStream(ListImagesFullName[userPanel_textMinute.comboBoxGetSelectedIndexIcon()]);
                        gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                            (int)numericUpDown_unitY.Value, src.Width, src.Height));
                    }
                }
            }

            // секунды надписью
            if (!AOD)
            {
                if (userPanel_textSecond != null && userPanel_textSecond.checkBox_Use.Checked)
                {
                    if (userPanel_textSecond.comboBoxGetSelectedIndexImage() >= 0)
                    {
                        NumericUpDown numericUpDownX = userPanel_textSecond.numericUpDown_imageX;
                        NumericUpDown numericUpDownY = userPanel_textSecond.numericUpDown_imageY;
                        NumericUpDown numericUpDown_unitX = userPanel_textSecond.numericUpDown_iconX;
                        NumericUpDown numericUpDown_unitY = userPanel_textSecond.numericUpDown_iconY;
                        NumericUpDown numericUpDown_spacing = userPanel_textSecond.numericUpDown_spacing;
                        CheckBox checkBox_addZero = userPanel_textSecond.checkBox_addZero;
                        CheckBox checkBox_follow = userPanel_textSecond.checkBox_follow;

                        int imageIndex = userPanel_textSecond.comboBoxGetSelectedIndexImage();
                        int x = (int)numericUpDownX.Value;
                        int y = (int)numericUpDownY.Value;
                        int spasing = (int)numericUpDown_spacing.Value;
                        int alignment = userPanel_textSecond.comboBoxGetSelectedIndexAlignment();
                        bool addZero = checkBox_addZero.Checked;
                        int separator_index = userPanel_textSecond.comboBoxGetSelectedIndexUnit();
                        int delimiter_index = userPanel_textSecond.comboBoxGetSelectedIndexImageDecimalPointOrMinus();

                        if (checkBox_follow.Checked && _offsetX > -1)
                        {
                            x = _offsetX;
                            alignment = 0;
                            y = _offsetY;
                            spasing = spasing_offset;
                        }

                        Draw_dagital_text(gPanel, imageIndex, x, y,
                            spasing, alignment, second, addZero, 2, separator_index,
                            delimiter_index, 0, BBorder);

                        if (userPanel_textSecond.comboBoxGetSelectedIndexIcon() >= 0)
                        {
                            src = OpenFileStream(ListImagesFullName[userPanel_textSecond.comboBoxGetSelectedIndexIcon()]);
                            gPanel.DrawImage(src, new Rectangle((int)numericUpDown_unitX.Value,
                                (int)numericUpDown_unitY.Value, src.Width, src.Height));
                        }
                    }
                } 
            }

            string sValue_hour = "";
            string sValue_minute = "";
            string sValue_second = "";
            bool addZeroSF;
            bool separator;
            bool follow_minute = false;
            bool follow_second = false;
            CheckBox checkBox_separator;

            #region часы 
            if (userControl_SystemFont_Hour != null && userControl_SystemFont_Hour.checkBox_Use.Checked)
            {
                sValue_hour = hour.ToString();
                addZeroSF = userControl_SystemFont_Hour.checkBox_addZero.Checked;
                int unitCheck = userControl_SystemFont_Hour.checkBoxGetUnit();
                if (addZeroSF)
                {
                    while (sValue_hour.Length < 2)
                    {
                        sValue_hour = "0" + sValue_hour;
                    }
                }
                sValue_hour = sValue_hour + UnitName("Time", unitCheck);
                checkBox_separator = userControl_SystemFont_Hour.checkBox_separator;
                separator = checkBox_separator.Checked;
                if (separator) sValue_hour = sValue_hour + "/";
            }
            #endregion

            #region минуты
            if (userControl_SystemFont_Minute != null && userControl_SystemFont_Minute.checkBox_Use.Checked)
            {
                sValue_minute = minute.ToString();
                addZeroSF = userControl_SystemFont_Minute.checkBox_addZero.Checked;
                int unitCheck = userControl_SystemFont_Minute.checkBoxGetUnit();
                //if (addZeroSF)
                {
                    while (sValue_minute.Length < 2)
                    {
                        sValue_minute = "0" + sValue_minute;
                    }
                }
                sValue_minute = sValue_minute + UnitName("Time", unitCheck);
                checkBox_separator = userControl_SystemFont_Minute.checkBox_separator;
                separator = checkBox_separator.Checked;
                if (separator) sValue_minute = sValue_minute + "/";
                follow_minute = userControl_SystemFont_Minute.checkBox_follow.Checked;
            }
            #endregion

            #region секунды
            if (!AOD && userControl_SystemFont_Second != null && userControl_SystemFont_Second.checkBox_Use.Checked)
            {
                sValue_second = second.ToString();
                addZeroSF = userControl_SystemFont_Second.checkBox_addZero.Checked;
                int unitCheck = userControl_SystemFont_Second.checkBoxGetUnit();
                //if (addZeroSF)
                {
                    while (sValue_second.Length < 2)
                    {
                        sValue_second = "0" + sValue_second;
                    }
                }
                sValue_second = sValue_second + UnitName("Time", unitCheck);
                checkBox_separator = userControl_SystemFont_Second.checkBox_separator;
                separator = checkBox_separator.Checked;
                if (separator) sValue_second = sValue_second + "/";
                follow_second = userControl_SystemFont_Second.checkBox_follow.Checked;
            }
            #endregion

            if (!AOD)
            {
                if (follow_second)
                {
                    if (sValue_minute.Length > 0)
                    {
                        sValue_minute = sValue_minute + sValue_second;
                        sValue_second = "";
                    }
                    else if (sValue_hour.Length > 0)
                    {
                        sValue_hour = sValue_hour + sValue_second;
                        sValue_second = "";
                    }
                }
                else sValue_second = sValue_hour + sValue_minute + sValue_second; 
            }

            if (follow_minute)
            {
                sValue_hour = sValue_hour + sValue_minute;
                sValue_minute = "";
            }
            else sValue_minute = sValue_hour + sValue_minute;

            // надпись системным шрифтом (часы)
            if (userControl_SystemFont_Hour != null && userControl_SystemFont_Hour.checkBox_Use.Checked)
            {
                if (sValue_hour.Length > 0)
                {
                    NumericUpDown numericUpDownX = userControl_SystemFont_Hour.numericUpDown_SystemFontX;
                    NumericUpDown numericUpDownY = userControl_SystemFont_Hour.numericUpDown_SystemFontY;
                    NumericUpDown numericUpDown_size = userControl_SystemFont_Hour.numericUpDown_SystemFont_size;
                    NumericUpDown numericUpDown_angle = userControl_SystemFont_Hour.numericUpDown_SystemFont_angle;
                    NumericUpDown numericUpDown_spacing = userControl_SystemFont_Hour.numericUpDown_SystemFont_spacing;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int size = (int)numericUpDown_size.Value;
                    int angle = (int)numericUpDown_angle.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    Color color = userControl_SystemFont_Hour.comboBoxGetColor();

                    Draw_text(gPanel, x, y, size, spasing, color, angle, sValue_hour, BBorder);
                }
            }

            // надпись системным шрифтом (минуты)
            if (userControl_SystemFont_Minute != null && userControl_SystemFont_Minute.checkBox_Use.Checked)
            {
                if (sValue_minute.Length > 0)
                {
                    NumericUpDown numericUpDownX = userControl_SystemFont_Minute.numericUpDown_SystemFontX;
                    NumericUpDown numericUpDownY = userControl_SystemFont_Minute.numericUpDown_SystemFontY;
                    NumericUpDown numericUpDown_size = userControl_SystemFont_Minute.numericUpDown_SystemFont_size;
                    NumericUpDown numericUpDown_angle = userControl_SystemFont_Minute.numericUpDown_SystemFont_angle;
                    NumericUpDown numericUpDown_spacing = userControl_SystemFont_Minute.numericUpDown_SystemFont_spacing;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int size = (int)numericUpDown_size.Value;
                    int angle = (int)numericUpDown_angle.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    Color color = userControl_SystemFont_Minute.comboBoxGetColor();

                    Draw_text(gPanel, x, y, size, spasing, color, angle, sValue_minute, BBorder);
                }
            }

            // надпись системным шрифтом (секунды)
            if (!AOD && userControl_SystemFont_Second != null && userControl_SystemFont_Second.checkBox_Use.Checked)
            {
                if (sValue_second.Length > 0)
                {
                    NumericUpDown numericUpDownX = userControl_SystemFont_Second.numericUpDown_SystemFontX;
                    NumericUpDown numericUpDownY = userControl_SystemFont_Second.numericUpDown_SystemFontY;
                    NumericUpDown numericUpDown_size = userControl_SystemFont_Second.numericUpDown_SystemFont_size;
                    NumericUpDown numericUpDown_angle = userControl_SystemFont_Second.numericUpDown_SystemFont_angle;
                    NumericUpDown numericUpDown_spacing = userControl_SystemFont_Second.numericUpDown_SystemFont_spacing;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int size = (int)numericUpDown_size.Value;
                    int angle = (int)numericUpDown_angle.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    Color color = userControl_SystemFont_Second.comboBoxGetColor();

                    Draw_text(gPanel, x, y, size, spasing, color, angle, sValue_second, BBorder);
                }
            }


            sValue_hour = "";
            sValue_minute = "";
            sValue_second = "";
            follow_minute = false;
            follow_second = false;

            #region часы
            if (userControl_FontRotate_Hour != null && userControl_FontRotate_Hour.checkBox_Use.Checked)
            {
                sValue_hour = hour.ToString();
                addZeroSF = userControl_FontRotate_Hour.checkBox_addZero.Checked;
                int unitCheck = userControl_FontRotate_Hour.checkBoxGetUnit();
                if (addZeroSF)
                {
                    while (sValue_hour.Length < 2)
                    {
                        sValue_hour = "0" + sValue_hour;
                    }
                }
                sValue_hour = sValue_hour + UnitName("Time", unitCheck);
                checkBox_separator = userControl_FontRotate_Hour.checkBox_separator;
                separator = checkBox_separator.Checked;
                if (separator) sValue_hour = sValue_hour + "/";
            }
            #endregion

            #region минуты
            if (userControl_FontRotate_Minute != null && userControl_FontRotate_Minute.checkBox_Use.Checked)
            {
                sValue_minute = minute.ToString();
                addZeroSF = userControl_FontRotate_Minute.checkBox_addZero.Checked;
                int unitCheck = userControl_FontRotate_Minute.checkBoxGetUnit();
                //if (addZeroSF)
                {
                    while (sValue_minute.Length < 2)
                    {
                        sValue_minute = "0" + sValue_minute;
                    }
                }
                sValue_minute = sValue_minute + UnitName("Time", unitCheck);
                checkBox_separator = userControl_FontRotate_Minute.checkBox_separator;
                separator = checkBox_separator.Checked;
                if (separator) sValue_minute = sValue_minute + "/";
                follow_minute = userControl_FontRotate_Minute.checkBox_follow.Checked;
            }
            #endregion

            #region секунды
            if (!AOD && userControl_FontRotate_Second != null && userControl_FontRotate_Second.checkBox_Use.Checked)
            {
                sValue_second = second.ToString();
                addZeroSF = userControl_FontRotate_Second.checkBox_addZero.Checked;
                int unitCheck = userControl_FontRotate_Second.checkBoxGetUnit();
                //if (addZeroSF)
                {
                    while (sValue_second.Length < 2)
                    {
                        sValue_second = "0" + sValue_second;
                    }
                }
                sValue_second = sValue_second + UnitName("Time", unitCheck);
                checkBox_separator = userControl_FontRotate_Second.checkBox_separator;
                separator = checkBox_separator.Checked;
                if (separator) sValue_second = sValue_second + "/";
                follow_second = userControl_FontRotate_Second.checkBox_follow.Checked;
            }
            #endregion

            if (!AOD)
            {
                if (follow_second)
                {
                    if (sValue_minute.Length > 0)
                    {
                        sValue_minute = sValue_minute + sValue_second;
                        sValue_second = "";
                    }
                    else if (sValue_hour.Length > 0)
                    {
                        sValue_hour = sValue_hour + sValue_second;
                        sValue_second = "";
                    }
                }
                else sValue_second = sValue_hour + sValue_minute + sValue_second;
            }

            if (follow_minute)
            {
                sValue_hour = sValue_hour + sValue_minute;
                sValue_minute = "";
            }
            else sValue_minute = sValue_hour + sValue_minute;

            // надпись системным шрифтом по окружности (часы)
            if (userControl_FontRotate_Hour != null && userControl_FontRotate_Hour.checkBox_Use.Checked)
            {
                if (sValue_hour.Length > 0)
                {
                    NumericUpDown numericUpDownX = userControl_FontRotate_Hour.numericUpDown_FontRotateX;
                    NumericUpDown numericUpDownY = userControl_FontRotate_Hour.numericUpDown_FontRotateY;
                    NumericUpDown numericUpDown_size = userControl_FontRotate_Hour.numericUpDown_FontRotate_size;
                    NumericUpDown numericUpDown_angle = userControl_FontRotate_Hour.numericUpDown_FontRotate_angle;
                    NumericUpDown numericUpDown_radius = userControl_FontRotate_Hour.numericUpDown_FontRotate_radius;
                    NumericUpDown numericUpDown_spacing = userControl_FontRotate_Hour.numericUpDown_FontRotate_spacing;
                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int size = (int)numericUpDown_size.Value;
                    int angle = (int)numericUpDown_angle.Value;
                    int radius = (int)numericUpDown_radius.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int rotate_direction = userControl_FontRotate_Hour.radioButtonGetRotateDirection();
                    Color color = userControl_FontRotate_Hour.comboBoxGetColor();

                    Draw_text_rotate(gPanel, x, y, radius, size, spasing, color, angle, rotate_direction,
                                sValue_hour, BBorder);
                }
            }

            // надпись системным шрифтом по окружности (минуты)
            if (userControl_FontRotate_Minute != null && userControl_FontRotate_Minute.checkBox_Use.Checked)
            {
                if (sValue_minute.Length > 0)
                {
                    NumericUpDown numericUpDownX = userControl_FontRotate_Minute.numericUpDown_FontRotateX;
                    NumericUpDown numericUpDownY = userControl_FontRotate_Minute.numericUpDown_FontRotateY;
                    NumericUpDown numericUpDown_size = userControl_FontRotate_Minute.numericUpDown_FontRotate_size;
                    NumericUpDown numericUpDown_angle = userControl_FontRotate_Minute.numericUpDown_FontRotate_angle;
                    NumericUpDown numericUpDown_radius = userControl_FontRotate_Minute.numericUpDown_FontRotate_radius;
                    NumericUpDown numericUpDown_spacing = userControl_FontRotate_Minute.numericUpDown_FontRotate_spacing;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int size = (int)numericUpDown_size.Value;
                    int angle = (int)numericUpDown_angle.Value;
                    int radius = (int)numericUpDown_radius.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int rotate_direction = userControl_FontRotate_Minute.radioButtonGetRotateDirection();
                    Color color = userControl_FontRotate_Minute.comboBoxGetColor();

                    Draw_text_rotate(gPanel, x, y, radius, size, spasing, color, angle, rotate_direction,
                                sValue_minute, BBorder);
                }
            }

            // надпись системным шрифтом по окружности (секунды)
            if (!AOD && userControl_FontRotate_Second != null && userControl_FontRotate_Second.checkBox_Use.Checked)
            {
                if (sValue_second.Length > 0)
                {
                    NumericUpDown numericUpDownX = userControl_FontRotate_Second.numericUpDown_FontRotateX;
                    NumericUpDown numericUpDownY = userControl_FontRotate_Second.numericUpDown_FontRotateY;
                    NumericUpDown numericUpDown_size = userControl_FontRotate_Second.numericUpDown_FontRotate_size;
                    NumericUpDown numericUpDown_angle = userControl_FontRotate_Second.numericUpDown_FontRotate_angle;
                    NumericUpDown numericUpDown_radius = userControl_FontRotate_Second.numericUpDown_FontRotate_radius;
                    NumericUpDown numericUpDown_spacing = userControl_FontRotate_Second.numericUpDown_FontRotate_spacing;

                    int x = (int)numericUpDownX.Value;
                    int y = (int)numericUpDownY.Value;
                    int size = (int)numericUpDown_size.Value;
                    int angle = (int)numericUpDown_angle.Value;
                    int radius = (int)numericUpDown_radius.Value;
                    int spasing = (int)numericUpDown_spacing.Value;
                    int rotate_direction = userControl_FontRotate_Second.radioButtonGetRotateDirection();
                    Color color = userControl_FontRotate_Second.comboBoxGetColor();

                    Draw_text_rotate(gPanel, x, y, radius, size, spasing, color, angle, rotate_direction,
                        sValue_second, BBorder);
                }
            }

            src.Dispose();
        }

        private string UnitName(string ActivityName, int unitCheck)
        {
            string name = "";
            if (unitCheck == 1)
            {
                switch (ActivityName)
                {
                    case "Battery":
                        name = "%";
                        break;
                    case "Steps":
                        name = "Steps";
                        break;
                    case "Calories":
                        name = "kcal";
                        break;
                    case "HeartRate":
                        name = "bpm";
                        break;
                    case "Distance":
                        name = "km";
                        break;
                    case "Weather":
                        name = "°C";
                        break;
                    case "Humidity":
                        name = "%";
                        break;
                    case "AirPressure":
                        name = "KPA";
                        break;
                    case "Time":
                        name = ":";
                        break;
                    case "Date":
                        name = "/";
                        break;
                } 
            }
            if (unitCheck == 2)
            {
                switch (ActivityName)
                {
                    case "Battery":
                        name = "%";
                        break;
                    case "Steps":
                        name = "STEPS";
                        break;
                    case "Calories":
                        name = "Cal";
                        break;
                    case "HeartRate":
                        name = "BPM";
                        break;
                    case "Distance":
                        name = "KM";
                        break;
                    case "Weather":
                        name = "°C";
                        break;
                    case "Humidity":
                        name = "%";
                        break;
                    case "AirPressure":
                        name = "KPA";
                        break;
                    case "Time":
                        name = ":";
                        break;
                    case "Date":
                        name = ".";
                        break;
                }
            }
            return name;
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

            //int srcX = (int)Math.Round(x - radius - width / 2, MidpointRounding.AwayFromZero);
            //int srcY = (int)Math.Round(y - radius - width / 2, MidpointRounding.AwayFromZero);
            int srcX = (int)(x - radius - width / 2);
            int srcY = (int)(y - radius - width / 2);
            int arcX = (int)(x - radius);
            int arcY = (int)(y - radius);
            float CircleWidth = 2 * radius;

            if (backgroundIndex >= 0 && backgroundIndex < ListImagesFullName.Count)
            {
                Bitmap srcBackground = OpenFileStream(ListImagesFullName[backgroundIndex]);
                graphics.DrawImage(srcBackground, new Rectangle(srcX, srcX, srcBackground.Width, srcBackground.Height));
                srcBackground.Dispose();
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
            while (spacing > 127)
            {
                spacing = spacing - 256;
            }
            while (spacing < -128)
            {
                spacing = spacing + 256;
            }

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
            if (ActivityType == 11) value_lenght = 3;
            int DateLenght = width * value_lenght + 1;
            if (spacing > 0) DateLenght = DateLenght + spacing * (value_lenght - 1);
            //else DateLenght = DateLenght - spacing;

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
                        graphics.DrawImage(src, PointX, PointY);
                        PointX = PointX + src.Width + spacing;
                        //src.Dispose();
                    }
                }

            }
            //result = PointX - spacing;
            result = PointX;
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

            Logger.WriteLine("* Draw_dagital_text (end)");
            return result;
        }

        /// <summary>Пишем число системным шрифтом</summary>
        /// <param name="graphics">Поверхность для рисования</param>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата y</param>
        /// <param name="size">Размер шрифта</param>
        /// <param name="spacing">Величина отступа</param>
        /// <param name="color">Цвет шрифта</param>
        /// <param name="angle">Угол поворота надписи в градусах</param>
        /// <param name="value">Отображаемая величина</param>
        /// <param name="addZero">Отображать начальные нули</param>
        /// <param name="value_lenght">Количество отображаемых символов</param>
        /// <param name="BBorder">Рисовать рамку по координатам, вокруг элементов с выравниванием</param>
        /// <param name="ActivityType">Номер активности (при необходимости)</param>
        private int Draw_text(Graphics graphics, int x, int y, float size, int spacing, Color color,
            float angle, string value,  bool BBorder, int ActivityType = 0)
        {
            while (spacing > 127)
            {
                spacing = spacing - 255;
            }
            while (spacing < -127)
            {
                spacing = spacing + 255;
            }

            int result = 0;
            size = size * 0.9f;
            //Font drawFont = new Font("Times New Roman", size, GraphicsUnit.World);
            Font drawFont = new Font(fonts.Families[0], size, GraphicsUnit.World);
            StringFormat strFormat = new StringFormat();
            strFormat.FormatFlags = StringFormatFlags.FitBlackBox;
            strFormat.Alignment = StringAlignment.Near;
            strFormat.LineAlignment = StringAlignment.Far;
            Size strSize1 = TextRenderer.MeasureText(graphics, "0", drawFont);
            Size strSize2 = TextRenderer.MeasureText(graphics, "00", drawFont);
            int chLenght = strSize2.Width - strSize1.Width;
            int offsetX = strSize1.Width - chLenght;
            float offsetY = 1.1f * (strSize1.Height - size);

            Logger.WriteLine("* Draw_text");
            char[] CH = value.ToCharArray();

            int PointX = (int)(-0.3 * offsetX);

            Logger.WriteLine("Draw value");
            SolidBrush drawBrush = new SolidBrush(color);

            graphics.TranslateTransform(x, y);
            graphics.RotateTransform(angle);
            try
            {
                foreach (char ch in CH)
                {
                    string str = ch.ToString();
                    Size strSize = TextRenderer.MeasureText(graphics, str, drawFont);
                    //SizeF stringSize = graphics.MeasureString(str, drawFont, 10000, strFormat);
                    //graphics.FillRectangle(new SolidBrush(Color.White), x + PointX, y + offsetY - strSize.Height, strSize.Width, strSize.Height);
                    graphics.DrawString(str, drawFont, drawBrush, PointX, offsetY, strFormat);

                    PointX = PointX + strSize.Width + spacing - offsetX;
                }

                result = x + TextRenderer.MeasureText(graphics, value, drawFont).Width - offsetX + (value.Length - 1) * spacing;
                if (BBorder)
                {
                    Logger.WriteLine("DrawBorder");
                    Rectangle rect = new Rectangle(0, (int)(-0.75 * size), result - x - 1, (int)(0.75*size));
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
            }
            finally
            {
                //graphics.RotateTransform(-angle);
                //graphics.TranslateTransform(-x, -y);
                graphics.ResetTransform();
            }

            Logger.WriteLine("* Draw_text (end)");
            return result;
        }

        /// <summary>Пишем число системным шрифтом</summary>
        /// <param name="graphics">Поверхность для рисования</param>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата y</param>
        /// <param name="radius">Радиус y</param>
        /// <param name="size">Размер шрифта</param>
        /// <param name="spacing">Величина отступа</param>
        /// <param name="color">Цвет шрифта</param>
        /// <param name="angle">Угол поворота надписи в градусах</param>
        /// <param name="rotate_direction">Направление текста</param>
        /// <param name="value">Отображаемая величина</param>
        /// <param name="addZero">Отображать начальные нули</param>
        /// <param name="value_lenght">Количество отображаемых символов</param>
        /// <param name="BBorder">Рисовать рамку по координатам, вокруг элементов с выравниванием</param>
        /// <param name="ActivityType">Номер активности (при необходимости)</param>
        private float Draw_text_rotate(Graphics graphics, int x, int y, int radius, float size, int spacing, 
            Color color, float angle, int rotate_direction, string value, bool BBorder, int ActivityType = 0)
        {
            while (spacing > 127)
            {
                spacing = spacing - 255;
            }
            while (spacing < -127)
            {
                spacing = spacing + 255;
            }

            size = size * 0.9f;
            if (radius == 0) radius = 1;
             //Font drawFont = new Font("Times New Roman", size, GraphicsUnit.World);
             Font drawFont = new Font(fonts.Families[0], size, GraphicsUnit.World);
            StringFormat strFormat = new StringFormat();
            strFormat.FormatFlags = StringFormatFlags.FitBlackBox;
            strFormat.Alignment = StringAlignment.Near;
            strFormat.LineAlignment = StringAlignment.Far;
            Size strSize1 = TextRenderer.MeasureText(graphics, "0", drawFont);
            Size strSize2 = TextRenderer.MeasureText(graphics, "00", drawFont);
            int chLenght = strSize2.Width - strSize1.Width;
            int offsetX = strSize1.Width - chLenght;
            //float offsetY = 1.1f * (strSize1.Height - size);
            float offsetY = (strSize1.Height - size);

            Logger.WriteLine("* Draw_text_rotate");
            char[] CH = value.ToCharArray();

            int PointX = (int)(-0.3 * offsetX);

            Logger.WriteLine("Draw value");
            SolidBrush drawBrush = new SolidBrush(color);
            graphics.TranslateTransform(x, y);

            if (rotate_direction == 1)
            {
                radius = -radius;
                //PointX = (int)(0.7 * offsetX);
                graphics.RotateTransform(-angle + 180);
            }
            else
            {
                graphics.RotateTransform(angle);
            }

            float offset_angle = 0;
            try
            {
                foreach (char ch in CH)
                {
                    string str = ch.ToString();
                    Size strSize = TextRenderer.MeasureText(graphics, str, drawFont);
                    //SizeF stringSize = graphics.MeasureString(str, drawFont, 10000, strFormat);
                    //graphics.FillRectangle(new SolidBrush(Color.White), x + PointX, y + offsetY - strSize.Height, strSize.Width, strSize.Height);
                    graphics.DrawString(str, drawFont, drawBrush, PointX, offsetY - radius, strFormat);

                    double sircle_lenght_relative = (strSize.Width + spacing - offsetX) / (2 * Math.PI * radius);
                    offset_angle = ((float)(sircle_lenght_relative * 360));
                    offset_angle = offset_angle * 1.05f;
                    //if (rotate_direction == 1) offset_angle = -offset_angle;
                    graphics.RotateTransform(offset_angle);
                    //PointX = PointX + strSize.Width + spacing - offsetX;
                }

            }
            finally
            {
                //graphics.RotateTransform(-angle);
                //graphics.TranslateTransform(-x, -y);
                graphics.ResetTransform();
            }

            Logger.WriteLine("* Draw_text_rotate (end)");
            return offset_angle;
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
        private int Draw_dagital_text(Graphics graphics, int image_index, int x, int y, int spacing,
            int alignment, double value, bool addZero, int value_lenght, int separator_index, 
            int decimalPoint_index, int decCount, bool BBorder)
        {
            Logger.WriteLine("* Draw_dagital_text");
            value = Math.Round(value, decCount, MidpointRounding.AwayFromZero);
            while (spacing > 127)
            {
                spacing = spacing - 255;
            } 
            while (spacing < -127)
            {
                spacing = spacing + 255;
            }
            //var Digit = new Bitmap(ListImagesFullName[image_index]);
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
            if (separator_index >= 0 && separator_index < ListImagesFullName.Count)
            {
                src = OpenFileStream(ListImagesFullName[separator_index]);
                DateLenghtReal = DateLenghtReal + src.Width + spacing;
            }
            DateLenghtReal = DateLenghtReal - spacing;


            src = OpenFileStream(ListImagesFullName[image_index]);
            int width = src.Width;
            int height = src.Height;
            int DateLenght = 4 * width ;
            if (spacing > 0) DateLenght = DateLenght + 3 * spacing;
            if (decimalPoint_index >= 0 && decimalPoint_index < ListImagesFullName.Count)
            {
                src = OpenFileStream(ListImagesFullName[decimalPoint_index]);
                DateLenght = DateLenght + src.Width;
                if (spacing > 0) DateLenght = DateLenght +  spacing;
            }
            if (separator_index >= 0 && separator_index < ListImagesFullName.Count)
            {
                src = OpenFileStream(ListImagesFullName[separator_index]);
                DateLenght = DateLenght + src.Width;
                if (spacing > 0) DateLenght = DateLenght + spacing;
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
            int result = PointX;
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

            Logger.WriteLine("* Draw_dagital_text (end)");
            return result;
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
        /// <param name="imageError_index">Иконка ошибки данны</param>
        /// <param name="errorData">отображать ошибку данный</param>
        /// <param name="centr_alignment">центр выравнивания при наличии иконки (для виджетов)</param>
        private int Draw_weather_text(Graphics graphics, int image_index, int x, int y, int spacing,
            int alignment, int value, bool addZero, int image_minus_index, int separator_index, bool BBorder, 
            int imageError_index = -1, bool errorData = false, int centr_alignment = -1)
        {
            while (spacing > 127)
            {
                spacing = spacing - 255;
            }
            while (spacing < -127)
            {
                spacing = spacing + 255;
            }
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
            //if (alignment == 2 && AvailabilityIcon) DateLenght = DateLenght - widthCF;
            if (spacing > 0) DateLenght = DateLenght + 4*spacing;
            if (widthM == 0) DateLenght = DateLenght - spacing;
            //if (alignment == 2 && AvailabilityIcon) DateLenght = DateLenght - spacing;

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
                    if (centr_alignment != -1) PointX = centr_alignment - (DateLenghtReal - widthCF) / 2;
                    break;
                default:
                    PointX = x;
                    break;
            }

            Logger.WriteLine("Draw value");
            if (!errorData)
            {
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
            }
            else if (imageError_index >= 0)
            {
                src = OpenFileStream(ListImagesFullName[imageError_index]);
                switch (alignment)
                {
                    case 0:
                        PointX = x;
                        break;
                    case 1:
                        PointX = x + DateLenght - src.Width;
                        break;
                    case 2:
                        PointX = x + (DateLenght - src.Width) / 2;
                        break;
                    default:
                        PointX = x;
                        break;
                }
                graphics.DrawImage(src, PointX, PointY);
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
            if (radioButton_ZeppE.Checked)
            {
                centerX = 208;
                centerY = 208;
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

        public Bitmap ApplyWatchSkin(Bitmap bitmap)
        {
            string Watch_Skin_file_name = textBox_WatchSkin_Path.Text;
            if (!File.Exists(Watch_Skin_file_name))
                Watch_Skin_file_name = Application.StartupPath + Watch_Skin_file_name;

            WatchSkin Watch_Skin = new WatchSkin();
            if (File.Exists(Watch_Skin_file_name))
            {
                string text = File.ReadAllText(Watch_Skin_file_name);
                try
                {
                    Watch_Skin = JsonConvert.DeserializeObject<WatchSkin>(text, new JsonSerializerSettings
                    {
                        DefaultValueHandling = DefaultValueHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                        Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return bitmap;
                } 
            }
            else return bitmap;

            Bitmap mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gtr_2.png");
            if (radioButton_GTS2.Checked)
            {
                mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gts_2.png");
            }
            if (radioButton_TRex_pro.Checked)
            {
                mask = new Bitmap(Application.StartupPath + @"\Mask\mask_trex_pro.png");
            }
            if (radioButton_ZeppE.Checked)
            {
                mask = new Bitmap(Application.StartupPath + @"\Mask\mask_zepp_e.png");
            }
            bitmap = ApplyMask(bitmap, mask);
            //Graphics gPanel = Graphics.FromImage(bitmap);

            Bitmap BackgroundImage = null;
            Bitmap ForegroundImage = null;

            float BackgroundImageHeight = -1;
            float ImageHeight = -1;
            float ForegroundImageHeight = -1;

            // Background
            if (Watch_Skin.Background != null && Watch_Skin.Background.Path != null)
            {
                string file_name = Watch_Skin.Background.Path;
                if (!File.Exists(file_name)) file_name = Application.StartupPath + file_name;
                if (File.Exists(file_name))
                {
                    BackgroundImage = new Bitmap(file_name);
                    if (Watch_Skin.Background.ImageHeight != null) BackgroundImageHeight = 
                            (int)Watch_Skin.Background.ImageHeight;
                    float scale = BackgroundImageHeight / BackgroundImage.Height;
                    BackgroundImage = ResizeImage(BackgroundImage, scale);
                }
            }

            // Image
            if (Watch_Skin.Image != null && Watch_Skin.Image.ImageHeight != null)
            {
                if (Watch_Skin.Image.ImageHeight != null) ImageHeight =
                            (int)Watch_Skin.Image.ImageHeight;
                float scale = ImageHeight / bitmap.Height;
                bitmap = ResizeImage(bitmap, scale);
            }

            Bitmap returnBitmap;
            if (BackgroundImage != null) returnBitmap = BackgroundImage;
            else returnBitmap = bitmap;
            Graphics gPanel = Graphics.FromImage(returnBitmap);

            if (BackgroundImage != null)
            {
                int posX = (int)(BackgroundImage.Width / 2f - bitmap.Width / 2f);
                int posY = (int)(BackgroundImage.Height / 2f - bitmap.Height / 2f);
                if (Watch_Skin.Image != null && Watch_Skin.Image.Position != null)
                {
                    posX = Watch_Skin.Image.Position.X;
                    posY = Watch_Skin.Image.Position.Y;
                }
                gPanel.DrawImage(bitmap, posX, posY, bitmap.Width, bitmap.Height);
            }

            // Foreground
            if (Watch_Skin.Foreground != null && Watch_Skin.Foreground.Path != null)
            {
                string file_name = Watch_Skin.Foreground.Path;
                if (!File.Exists(file_name)) file_name = Application.StartupPath + file_name;
                if (File.Exists(file_name))
                {
                    ForegroundImage = new Bitmap(file_name);
                    if (Watch_Skin.Foreground.ImageHeight != null) ForegroundImageHeight =
                            (int)Watch_Skin.Foreground.ImageHeight;
                    float scale = ForegroundImageHeight / ForegroundImage.Height;
                    ForegroundImage = ResizeImage(ForegroundImage, scale);
                }
            }

            if (ForegroundImage != null)
            {
                int posX = (int)(BackgroundImage.Width / 2f - ForegroundImage.Width / 2f);
                int posY = (int)(BackgroundImage.Height / 2f - ForegroundImage.Height / 2f);
                if (Watch_Skin.Image != null && Watch_Skin.Image.Position != null)
                {
                    posX = Watch_Skin.Foreground.Position.X;
                    posY = Watch_Skin.Foreground.Position.Y;
                }
                gPanel.DrawImage(ForegroundImage, posX, posY, ForegroundImage.Width, ForegroundImage.Height);
            }

            return returnBitmap;
        }
    }
}
