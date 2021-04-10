using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmazFit_Watchface_2
{
    public partial class Form1 : Form
    {
        private void button_Weather_pictures_Click(object sender, EventArgs e)
        {
            if (panel_Weather_pictures.Height == 1)
            {
                panel_Weather_pictures.Height = (int)(85 * currentDPI);
            }
            else panel_Weather_pictures.Height = 1;
        }

        private void button_Weather_text_Click(object sender, EventArgs e)
        {
            if (panel_Weather_text.Height == 1)
            {
                panel_Weather_text.Height = (int)(215 * currentDPI);
            }
            else panel_Weather_text.Height = 1;
        }

        private void button_Weather_textMin_Click(object sender, EventArgs e)
        {
            if (panel_Weather_textMin.Height == 1)
            {
                panel_Weather_textMin.Height = (int)(215 * currentDPI);
            }
            else panel_Weather_textMin.Height = 1;
        }

        private void button_Weather_textMax_Click(object sender, EventArgs e)
        {
            if (panel_Weather_textMax.Height == 1)
            {
                panel_Weather_textMax.Height = (int)(235 * currentDPI);
            }
            else panel_Weather_textMax.Height = 1;
        }

        private void button_Weather_hand_Click(object sender, EventArgs e)
        {
            if (panel_Weather_hand.Height == 1)
            {
                panel_Weather_hand.Height = (int)(225 * currentDPI);
            }
            else panel_Weather_hand.Height = 1;
        }

        private void button_Weather_scaleCircle_Click(object sender, EventArgs e)
        {
            if (panel_Weather_scaleCircle.Height == 1)
            {
                panel_Weather_scaleCircle.Height = (int)(215 * currentDPI);
            }
            else panel_Weather_scaleCircle.Height = 1;
        }

        private void button_Weather_scaleLinear_Click(object sender, EventArgs e)
        {
            if (panel_Weather_scaleLinear.Height == 1)
            {
                panel_Weather_scaleLinear.Height = (int)(155 * currentDPI);
            }
            else panel_Weather_scaleLinear.Height = 1;
        }



        private void button_Weather_pictures_AOD_Click(object sender, EventArgs e)
        {
            if (panel_Weather_pictures_AOD.Height == 1)
            {
                panel_Weather_pictures_AOD.Height = (int)(85 * currentDPI);
            }
            else panel_Weather_pictures_AOD.Height = 1;
        }

        private void button_Weather_text_AOD_Click(object sender, EventArgs e)
        {
            if (panel_Weather_text_AOD.Height == 1)
            {
                panel_Weather_text_AOD.Height = (int)(215 * currentDPI);
            }
            else panel_Weather_text_AOD.Height = 1;
        }

        private void button_Weather_textMin_AOD_Click(object sender, EventArgs e)
        {
            if (panel_Weather_textMin_AOD.Height == 1)
            {
                panel_Weather_textMin_AOD.Height = (int)(215 * currentDPI);
            }
            else panel_Weather_textMin_AOD.Height = 1;
        }

        private void button_Weather_textMax_AOD_Click(object sender, EventArgs e)
        {
            if (panel_Weather_textMax_AOD.Height == 1)
            {
                panel_Weather_textMax_AOD.Height = (int)(235 * currentDPI);
            }
            else panel_Weather_textMax_AOD.Height = 1;
        }

        private void button_Weather_hand_AOD_Click(object sender, EventArgs e)
        {
            if (panel_Weather_hand_AOD.Height == 1)
            {
                panel_Weather_hand_AOD.Height = (int)(225 * currentDPI);
            }
            else panel_Weather_hand_AOD.Height = 1;
        }

        private void button_Weather_scaleCircle_AOD_Click(object sender, EventArgs e)
        {
            if (panel_Weather_scaleCircle_AOD.Height == 1)
            {
                panel_Weather_scaleCircle_AOD.Height = (int)(215 * currentDPI);
            }
            else panel_Weather_scaleCircle_AOD.Height = 1;
        }

        private void button_Weather_scaleLinear_AOD_Click(object sender, EventArgs e)
        {
            if (panel_Weather_scaleLinear_AOD.Height == 1)
            {
                panel_Weather_scaleLinear_AOD.Height = (int)(155 * currentDPI);
            }
            else panel_Weather_scaleLinear_AOD.Height = 1;
        }





        private void button_RandomPreview_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            Random rnd = new Random();
            int year = now.Year;
            int month = rnd.Next(0, 12) + 1;
            int day = rnd.Next(0, 28) + 1;
            int weekDay = rnd.Next(0, 7) + 1;
            int hour = rnd.Next(0, 24);
            int min = rnd.Next(0, 60);
            int sec = rnd.Next(0, 60);
            int battery = rnd.Next(0, 101);
            int calories = rnd.Next(0, 2500);
            int pulse = rnd.Next(45, 150);
            int distance = rnd.Next(0, 15000);
            int steps = rnd.Next(0, 15000);
            int goal = rnd.Next(0, 15000);
            int pai = rnd.Next(0, 150);
            int standUp = rnd.Next(0, 13);
            bool bluetooth = rnd.Next(2) == 0 ? false : true;
            bool alarm = rnd.Next(2) == 0 ? false : true;
            bool unlocked = rnd.Next(2) == 0 ? false : true;
            bool dnd = rnd.Next(2) == 0 ? false : true;

            int temperature = rnd.Next(-25, 35);
            int temperatureMin = rnd.Next(-25, 35);
            int temperatureMax = rnd.Next(-25, 35);
            int temperatureIcon = rnd.Next(0, 29);

            Watch_Face_Preview_Set.Date.Year = year;
            Watch_Face_Preview_Set.Date.Month = month;
            Watch_Face_Preview_Set.Date.Day = day;
            Watch_Face_Preview_Set.Date.WeekDay = weekDay;

            Watch_Face_Preview_Set.Time.Hours = hour;
            Watch_Face_Preview_Set.Time.Minutes = min;
            Watch_Face_Preview_Set.Time.Seconds = sec;

            Watch_Face_Preview_Set.Battery = battery;
            Watch_Face_Preview_Set.Activity.Calories = calories;
            Watch_Face_Preview_Set.Activity.HeartRate = pulse;
            Watch_Face_Preview_Set.Activity.Distance = distance;
            Watch_Face_Preview_Set.Activity.Steps = steps;
            Watch_Face_Preview_Set.Activity.StepsGoal = goal;
            Watch_Face_Preview_Set.Activity.PAI = pai;
            Watch_Face_Preview_Set.Activity.StandUp = standUp;

            Watch_Face_Preview_Set.Status.Bluetooth = bluetooth;
            Watch_Face_Preview_Set.Status.Alarm = alarm;
            Watch_Face_Preview_Set.Status.Lock = unlocked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = dnd;

            Watch_Face_Preview_Set.Weather.Temperature = temperature;
            Watch_Face_Preview_Set.Weather.TemperatureMin = temperatureMin;
            Watch_Face_Preview_Set.Weather.TemperatureMax = temperatureMax;
            Watch_Face_Preview_Set.Weather.Icon = temperatureIcon;
            Watch_Face_Preview_Set.Weather.TemperatureNoData = false;
            Watch_Face_Preview_Set.Weather.TemperatureMinMaxNoData = false;


            PreviewImage();

        }



        private void button_Capy_ScreenNormal_Click(object sender, EventArgs e)
        {
            PreviewView = false;

            Copy_Hour_AOD();
            Copy_Minute_AOD();
            Copy_AM_PM_AOD();
            Copy_Hour_hand_AOD();
            Copy_Minute_hand_AOD();

            Copy_Day_text_AOD();
            Copy_Day_hand_AOD();
            Copy_Month_pictures_AOD();
            Copy_Month_text_AOD();
            Copy_Month_hand_AOD();
            Copy_Year_text_AOD();
            Copy_DOW_image_AOD();
            Copy_DOW_hand_AOD();


            Copy_pictures_AOD(userControl_pictures_Battery, userControl_pictures_Battery_AOD);
            Copy_text_AOD(userControl_text_Battery, userControl_text_Battery_AOD);
            Copy_hand_AOD(userControl_hand_Battery, userControl_hand_Battery_AOD);
            Copy_scaleCircle_AOD(userControl_scaleCircle_Battery, userControl_scaleCircle_Battery_AOD);
            Copy_scaleLinear_AOD(userControl_scaleLinear_Battery, userControl_scaleLinear_Battery_AOD);

            Copy_pictures_AOD(userControl_pictures_Steps, userControl_pictures_Steps_AOD);
            Copy_text_AOD(userControl_text_Steps, userControl_text_Steps_AOD);
            Copy_hand_AOD(userControl_hand_Steps, userControl_hand_Steps_AOD);
            Copy_scaleCircle_AOD(userControl_scaleCircle_Steps, userControl_scaleCircle_Steps_AOD);
            Copy_scaleLinear_AOD(userControl_scaleLinear_Steps, userControl_scaleLinear_Steps_AOD);

            Copy_pictures_AOD(userControl_pictures_Calories, userControl_pictures_Calories_AOD);
            Copy_text_AOD(userControl_text_Calories, userControl_text_Calories_AOD);
            Copy_hand_AOD(userControl_hand_Calories, userControl_hand_Calories_AOD);
            Copy_scaleCircle_AOD(userControl_scaleCircle_Calories, userControl_scaleCircle_Calories_AOD);
            Copy_scaleLinear_AOD(userControl_scaleLinear_Calories, userControl_scaleLinear_Calories_AOD);

            Copy_pictures_AOD(userControl_pictures_HeartRate, userControl_pictures_HeartRate_AOD);
            Copy_text_AOD(userControl_text_HeartRate, userControl_text_HeartRate_AOD);
            Copy_hand_AOD(userControl_hand_HeartRate, userControl_hand_HeartRate_AOD);
            Copy_scaleCircle_AOD(userControl_scaleCircle_HeartRate, userControl_scaleCircle_HeartRate_AOD);
            Copy_scaleLinear_AOD(userControl_scaleLinear_HeartRate, userControl_scaleLinear_HeartRate_AOD);

            Copy_pictures_AOD(userControl_pictures_PAI, userControl_pictures_PAI_AOD);
            Copy_text_AOD(userControl_text_PAI, userControl_text_PAI_AOD);
            Copy_hand_AOD(userControl_hand_PAI, userControl_hand_PAI_AOD);
            Copy_scaleCircle_AOD(userControl_scaleCircle_PAI, userControl_scaleCircle_PAI_AOD);
            Copy_scaleLinear_AOD(userControl_scaleLinear_PAI, userControl_scaleLinear_PAI_AOD);

            Copy_text_AOD(userControl_text_Distance, userControl_text_Distance_AOD);

            Copy_pictures_AOD(userControl_pictures_StandUp, userControl_pictures_StandUp_AOD);
            Copy_text_AOD(userControl_text_StandUp, userControl_text_StandUp_AOD);
            Copy_hand_AOD(userControl_hand_StandUp, userControl_hand_StandUp_AOD);
            Copy_scaleCircle_AOD(userControl_scaleCircle_StandUp, userControl_scaleCircle_StandUp_AOD);
            Copy_scaleLinear_AOD(userControl_scaleLinear_StandUp, userControl_scaleLinear_StandUp_AOD);




            Copy_pictures_AOD(panel_Weather_pictures, panel_Weather_pictures_AOD);
            Copy_Weather_textMin_AOD(panel_Weather_text, panel_Weather_text_AOD);
            Copy_Weather_textMin_AOD(panel_Weather_textMin, panel_Weather_textMin_AOD);
            Copy_Weather_textMax_AOD(panel_Weather_textMax, panel_Weather_textMax_AOD);
            Copy_hand_AOD(panel_Weather_hand, panel_Weather_hand_AOD);
            Copy_scaleCircle_AOD(panel_Weather_scaleCircle, panel_Weather_scaleCircle_AOD);
            Copy_scaleLinear_AOD(panel_Weather_scaleLinear, panel_Weather_scaleLinear_AOD);




            Copy_pictures_AOD(userControl_pictures_Stress, userControl_pictures_Stress_AOD);
            Copy_text_AOD(userControl_text_Stress, userControl_text_Stress_AOD);
            Copy_hand_AOD(userControl_hand_Stress, userControl_hand_Stress_AOD);
            Copy_scaleCircle_AOD(userControl_scaleCircle_Stress, userControl_scaleCircle_Stress_AOD);
            Copy_scaleLinear_AOD(userControl_scaleLinear_Stress, userControl_scaleLinear_Stress_AOD);

            Copy_pictures_AOD(userControl_pictures_ActivityGoal, userControl_pictures_ActivityGoal_AOD);
            Copy_text_AOD(userControl_text_ActivityGoal, userControl_text_ActivityGoal_AOD);
            Copy_hand_AOD(userControl_hand_ActivityGoal, userControl_hand_ActivityGoal_AOD);
            Copy_scaleCircle_AOD(userControl_scaleCircle_ActivityGoal, userControl_scaleCircle_ActivityGoal_AOD);
            Copy_scaleLinear_AOD(userControl_scaleLinear_ActivityGoal, userControl_scaleLinear_ActivityGoal_AOD);

            Copy_pictures_AOD(userControl_pictures_FatBurning, userControl_pictures_FatBurning_AOD);
            Copy_text_AOD(userControl_text_FatBurning, userControl_text_FatBurning_AOD);
            Copy_hand_AOD(userControl_hand_FatBurning, userControl_hand_FatBurning_AOD);
            Copy_scaleCircle_AOD(userControl_scaleCircle_FatBurning, userControl_scaleCircle_FatBurning_AOD);
            Copy_scaleLinear_AOD(userControl_scaleLinear_FatBurning, userControl_scaleLinear_FatBurning_AOD);







            PreviewView = true;
            JSON_write();
            PreviewImage();
        }

        private void button_Copy_Hour_AOD_Click(object sender, EventArgs e)
        {
            PreviewView = false;
            Copy_Hour_AOD();
            PreviewView = true;
            PreviewImage();
            JSON_write();
        }
        private void Copy_Hour_AOD()
        {
            checkBox_Hour_Use_AOD.Checked = checkBox_Hour_Use.Checked;
            comboBox_Hour_image_AOD.SelectedItem = comboBox_Hour_image.SelectedItem;
            comboBox_Hour_unit_AOD.SelectedItem = comboBox_Hour_unit.SelectedItem;
            comboBox_Hour_separator_AOD.SelectedItem = comboBox_Hour_separator.SelectedItem;
            numericUpDown_HourX_AOD.Value = numericUpDown_HourX.Value;
            numericUpDown_HourY_AOD.Value = numericUpDown_HourY.Value;
            numericUpDown_Hour_unitX_AOD.Value = numericUpDown_Hour_unitX.Value;
            numericUpDown_Hour_unitY_AOD.Value = numericUpDown_Hour_unitY.Value;
            comboBox_Hour_alignment_AOD.SelectedItem = comboBox_Hour_alignment.SelectedItem;
            checkBox_Hour_add_zero_AOD.Checked = checkBox_Hour_add_zero.Checked;
            numericUpDown_Hour_spacing_AOD.Value = numericUpDown_Hour_spacing.Value;
        }

        private void button_Copy_Minute_AOD_Click(object sender, EventArgs e)
        {
            PreviewView = false;
            Copy_Minute_AOD();
            PreviewView = true;
            PreviewImage();
            JSON_write();
        }
        private void Copy_Minute_AOD()
        {
            checkBox_Minute_Use_AOD.Checked = checkBox_Minute_Use.Checked;
            comboBox_Minute_image_AOD.SelectedItem = comboBox_Minute_image.SelectedItem;
            comboBox_Minute_unit_AOD.SelectedItem = comboBox_Minute_unit.SelectedItem;
            comboBox_Minute_separator_AOD.SelectedItem = comboBox_Minute_separator.SelectedItem;
            numericUpDown_MinuteX_AOD.Value = numericUpDown_MinuteX.Value;
            numericUpDown_MinuteY_AOD.Value = numericUpDown_MinuteY.Value;
            numericUpDown_Minute_unitX_AOD.Value = numericUpDown_Minute_unitX.Value;
            numericUpDown_Minute_unitY_AOD.Value = numericUpDown_Minute_unitY.Value;
            comboBox_Minute_alignment_AOD.SelectedItem = comboBox_Minute_alignment.SelectedItem;
            checkBox_Minute_add_zero_AOD.Checked = checkBox_Minute_add_zero.Checked;
            numericUpDown_Minute_spacing_AOD.Value = numericUpDown_Minute_spacing.Value;
            checkBox_Minute_follow_AOD.Checked = checkBox_Minute_follow.Checked;
        }

        private void button_Copy_AM_PM_AOD_Click(object sender, EventArgs e)
        {
            PreviewView = false;
            Copy_AM_PM_AOD();
            PreviewView = true;
            PreviewImage();
            JSON_write();
        }
        private void Copy_AM_PM_AOD()
        {
            checkBox_12h_Use_AOD.Checked = checkBox_12h_Use.Checked;
            comboBox_AM_image_AOD.SelectedItem = comboBox_AM_image.SelectedItem;
            comboBox_PM_image_AOD.SelectedItem = comboBox_PM_image.SelectedItem;
            numericUpDown_AM_X_AOD.Value = numericUpDown_AM_X.Value;
            numericUpDown_AM_Y_AOD.Value = numericUpDown_AM_Y.Value;
            numericUpDown_PM_X_AOD.Value = numericUpDown_PM_X.Value;
            numericUpDown_PM_Y_AOD.Value = numericUpDown_PM_Y.Value;
        }

        private void button_Copy_Hour_hand_AOD_Click(object sender, EventArgs e)
        {
            PreviewView = false;
            Copy_Hour_hand_AOD();
            PreviewView = true;
            PreviewImage();
            JSON_write();
        }
        private void Copy_Hour_hand_AOD()
        {
            checkBox_Hour_hand_Use_AOD.Checked = checkBox_Hour_hand_Use.Checked;
            comboBox_Hour_hand_image_AOD.SelectedItem = comboBox_Hour_hand_image.SelectedItem;
            comboBox_Hour_hand_imageCentr_AOD.SelectedItem = comboBox_Hour_hand_imageCentr.SelectedItem;
            numericUpDown_Hour_handX_AOD.Value = numericUpDown_Hour_handX.Value;
            numericUpDown_Hour_handY_AOD.Value = numericUpDown_Hour_handY.Value;
            numericUpDown_Hour_handX_centr_AOD.Value = numericUpDown_Hour_handX_centr.Value;
            numericUpDown_Hour_handY_centr_AOD.Value = numericUpDown_Hour_handY_centr.Value;
            numericUpDown_Hour_handX_offset_AOD.Value = numericUpDown_Hour_handX_offset.Value;
            numericUpDown_Hour_handY_offset_AOD.Value = numericUpDown_Hour_handY_offset.Value;
        }

        private void button_Copy_Minute_hand_AOD_Click(object sender, EventArgs e)
        {
            PreviewView = false;
            Copy_Minute_hand_AOD();
            PreviewView = true;
            PreviewImage();
            JSON_write();
        }
        private void Copy_Minute_hand_AOD()
        {
            checkBox_Minute_hand_Use_AOD.Checked = checkBox_Minute_hand_Use.Checked;
            comboBox_Minute_hand_image_AOD.SelectedItem = comboBox_Minute_hand_image.SelectedItem;
            comboBox_Minute_hand_imageCentr_AOD.SelectedItem = comboBox_Minute_hand_imageCentr.SelectedItem;
            numericUpDown_Minute_handX_AOD.Value = numericUpDown_Minute_handX.Value;
            numericUpDown_Minute_handY_AOD.Value = numericUpDown_Minute_handY.Value;
            numericUpDown_Minute_handX_centr_AOD.Value = numericUpDown_Minute_handX_centr.Value;
            numericUpDown_Minute_handY_centr_AOD.Value = numericUpDown_Minute_handY_centr.Value;
            numericUpDown_Minute_handX_offset_AOD.Value = numericUpDown_Minute_handX_offset.Value;
            numericUpDown_Minute_handY_offset_AOD.Value = numericUpDown_Minute_handY_offset.Value;
        }

        private void button_Copy_Day_text_AOD_Click(object sender, EventArgs e)
        {
            PreviewView = false;
            Copy_Day_text_AOD();
            PreviewView = true;
            PreviewImage();
            JSON_write();
        }
        private void Copy_Day_text_AOD()
        {
            checkBox_Day_Use_AOD.Checked = checkBox_Day_Use.Checked;
            comboBox_Day_image_AOD.SelectedIndex = comboBox_Day_image.SelectedIndex;
            comboBox_Day_unit_AOD.SelectedIndex = comboBox_Day_unit.SelectedIndex;
            comboBox_Day_separator_AOD.SelectedIndex = comboBox_Day_separator.SelectedIndex;
            numericUpDown_DayX_AOD.Value = numericUpDown_DayX.Value;
            numericUpDown_DayY_AOD.Value = numericUpDown_DayY.Value;
            numericUpDown_Day_unitX_AOD.Value = numericUpDown_Day_unitX.Value;
            numericUpDown_Day_unitY_AOD.Value = numericUpDown_Day_unitY.Value;
            comboBox_Day_alignment_AOD.SelectedIndex = comboBox_Day_alignment.SelectedIndex;
            checkBox_Day_add_zero_AOD.Checked = checkBox_Day_add_zero.Checked;
            numericUpDown_Day_spacing_AOD.Value = numericUpDown_Day_spacing.Value;
            checkBox_Day_follow_AOD.Checked = checkBox_Day_follow.Checked;
        }

        private void button_Copy_Day_hand_AOD_Click(object sender, EventArgs e)
        {
            PreviewView = false;
            Copy_Day_hand_AOD();
            PreviewView = true;
            PreviewImage();
            JSON_write();
        }
        private void Copy_Day_hand_AOD()
        {
            checkBox_Day_hand_Use_AOD.Checked = checkBox_Day_hand_Use.Checked;
            comboBox_Day_hand_image_AOD.SelectedIndex = comboBox_Day_hand_image.SelectedIndex;
            comboBox_Day_hand_imageCentr_AOD.SelectedIndex = comboBox_Day_hand_imageCentr.SelectedIndex;
            comboBox_Day_hand_imageBackground_AOD.SelectedIndex = comboBox_Day_hand_imageBackground.SelectedIndex;
            numericUpDown_Day_handX_AOD.Value = numericUpDown_Day_handX.Value;
            numericUpDown_Day_handY_AOD.Value = numericUpDown_Day_handY.Value;
            numericUpDown_Day_handX_centr_AOD.Value = numericUpDown_Day_handX_centr.Value;
            numericUpDown_Day_handY_centr_AOD.Value = numericUpDown_Day_handY_centr.Value;
            numericUpDown_Day_handX_background_AOD.Value = numericUpDown_Day_handX_background.Value;
            numericUpDown_Day_handY_background_AOD.Value = numericUpDown_Day_handY_background.Value;
            numericUpDown_Day_handX_offset_AOD.Value = numericUpDown_Day_handX_offset.Value;
            numericUpDown_Day_handY_offset_AOD.Value = numericUpDown_Day_handY_offset.Value;
            numericUpDown_Day_hand_startAngle_AOD.Value = numericUpDown_Day_hand_startAngle.Value;
            numericUpDown_Day_hand_endAngle_AOD.Value = numericUpDown_Day_hand_endAngle.Value;
        }

        private void button_Copy_Month_pictures_AOD_Click(object sender, EventArgs e)
        {
            PreviewView = false;
            Copy_Month_pictures_AOD();
            PreviewView = true;
            PreviewImage();
            JSON_write();
        }
        private void Copy_Month_pictures_AOD()
        {
            checkBox_Month_pictures_Use_AOD.Checked = checkBox_Month_pictures_Use.Checked;
            comboBox_Month_pictures_image_AOD.SelectedIndex = comboBox_Month_pictures_image.SelectedIndex;
            numericUpDown_Month_picturesX_AOD.Value = numericUpDown_Month_picturesX.Value;
            numericUpDown_Month_picturesY_AOD.Value = numericUpDown_Month_picturesY.Value;
        }

        private void button_Copy_Month_text_AOD_Click(object sender, EventArgs e)
        {
            PreviewView = false;
            Copy_Month_text_AOD();
            PreviewView = true;
            PreviewImage();
            JSON_write();
        }
        private void Copy_Month_text_AOD()
        {
            checkBox_Month_Use_AOD.Checked = checkBox_Month_Use.Checked;
            comboBox_Month_image_AOD.SelectedIndex = comboBox_Month_image.SelectedIndex;
            comboBox_Month_unit_AOD.SelectedIndex = comboBox_Month_unit.SelectedIndex;
            comboBox_Month_separator_AOD.SelectedIndex = comboBox_Month_separator.SelectedIndex;
            numericUpDown_MonthX_AOD.Value = numericUpDown_MonthX.Value;
            numericUpDown_MonthY_AOD.Value = numericUpDown_MonthY.Value;
            numericUpDown_Month_unitX_AOD.Value = numericUpDown_Month_unitX.Value;
            numericUpDown_Month_unitY_AOD.Value = numericUpDown_Month_unitY.Value;
            comboBox_Month_alignment_AOD.SelectedIndex = comboBox_Month_alignment.SelectedIndex;
            checkBox_Month_add_zero_AOD.Checked = checkBox_Month_add_zero.Checked;
            numericUpDown_Month_spacing_AOD.Value = numericUpDown_Month_spacing.Value;
            checkBox_Month_follow_AOD.Checked = checkBox_Month_follow.Checked;
        }

        private void button_Copy_Month_hand_AOD_Click(object sender, EventArgs e)
        {
            PreviewView = false;
            Copy_Month_hand_AOD();
            PreviewView = true;
            PreviewImage();
            JSON_write();
        }
        private void Copy_Month_hand_AOD()
        {
            checkBox_Month_hand_Use_AOD.Checked = checkBox_Month_hand_Use.Checked;
            comboBox_Month_hand_image_AOD.SelectedIndex = comboBox_Month_hand_image.SelectedIndex;
            comboBox_Month_hand_imageCentr_AOD.SelectedIndex = comboBox_Month_hand_imageCentr.SelectedIndex;
            comboBox_Month_hand_imageBackground_AOD.SelectedIndex = comboBox_Month_hand_imageBackground.SelectedIndex;
            numericUpDown_Month_handX_AOD.Value = numericUpDown_Month_handX.Value;
            numericUpDown_Month_handY_AOD.Value = numericUpDown_Month_handY.Value;
            numericUpDown_Month_handX_centr_AOD.Value = numericUpDown_Month_handX_centr.Value;
            numericUpDown_Month_handY_centr_AOD.Value = numericUpDown_Month_handY_centr.Value;
            numericUpDown_Month_handX_background_AOD.Value = numericUpDown_Month_handX_background.Value;
            numericUpDown_Month_handY_background_AOD.Value = numericUpDown_Month_handY_background.Value;
            numericUpDown_Month_handX_offset_AOD.Value = numericUpDown_Month_handX_offset.Value;
            numericUpDown_Month_handY_offset_AOD.Value = numericUpDown_Month_handY_offset.Value;
            numericUpDown_Month_hand_startAngle_AOD.Value = numericUpDown_Month_hand_startAngle.Value;
            numericUpDown_Month_hand_endAngle_AOD.Value = numericUpDown_Month_hand_endAngle.Value;
        }

        private void button_Copy_Year_text_AOD_Click(object sender, EventArgs e)
        {
            PreviewView = false;
            Copy_Year_text_AOD();
            PreviewView = true;
            PreviewImage();
            JSON_write();
        }
        private void Copy_Year_text_AOD()
        {
            checkBox_Year_text_Use_AOD.Checked = checkBox_Year_text_Use.Checked;
            comboBox_Year_image_AOD.SelectedIndex = comboBox_Year_image.SelectedIndex;
            comboBox_Year_unit_AOD.SelectedIndex = comboBox_Year_unit.SelectedIndex;
            comboBox_Year_separator_AOD.SelectedIndex = comboBox_Year_separator.SelectedIndex;
            numericUpDown_YearX_AOD.Value = numericUpDown_YearX.Value;
            numericUpDown_YearY_AOD.Value = numericUpDown_YearY.Value;
            numericUpDown_Year_unitX_AOD.Value = numericUpDown_Year_unitX.Value;
            numericUpDown_Year_unitY_AOD.Value = numericUpDown_Year_unitY.Value;
            comboBox_Year_alignment_AOD.SelectedIndex = comboBox_Year_alignment.SelectedIndex;
            checkBox_Year_add_zero_AOD.Checked = checkBox_Year_add_zero.Checked;
            numericUpDown_Year_spacing_AOD.Value = numericUpDown_Year_spacing.Value;
        }

        private void button_Copy_DOW_image_AOD_Click(object sender, EventArgs e)
        {
            PreviewView = false;
            Copy_DOW_image_AOD();
            PreviewView = true;
            PreviewImage();
            JSON_write();
        }
        private void Copy_DOW_image_AOD()
        {
            checkBox_DOW_pictures_Use_AOD.Checked = checkBox_DOW_pictures_Use.Checked;
            comboBox_DOW_pictures_image_AOD.SelectedIndex = comboBox_DOW_pictures_image.SelectedIndex;
            numericUpDown_DOW_picturesX_AOD.Value = numericUpDown_DOW_picturesX.Value;
            numericUpDown_DOW_picturesY_AOD.Value = numericUpDown_DOW_picturesY.Value;
        }

        private void button_Copy_DOW_hand_AOD_Click(object sender, EventArgs e)
        {
            PreviewView = false;
            Copy_DOW_hand_AOD();
            PreviewView = true;
            PreviewImage();
            JSON_write();
        }
        private void Copy_DOW_hand_AOD()
        {
            checkBox_DOW_hand_Use_AOD.Checked = checkBox_DOW_hand_Use.Checked;
            comboBox_DOW_hand_image_AOD.SelectedIndex = comboBox_DOW_hand_image.SelectedIndex;
            comboBox_DOW_hand_imageCentr_AOD.SelectedIndex = comboBox_DOW_hand_imageCentr.SelectedIndex;
            comboBox_DOW_hand_imageBackground_AOD.SelectedIndex = comboBox_DOW_hand_imageBackground.SelectedIndex;
            numericUpDown_DOW_handX_AOD.Value = numericUpDown_DOW_handX.Value;
            numericUpDown_DOW_handY_AOD.Value = numericUpDown_DOW_handY.Value;
            numericUpDown_DOW_handX_centr_AOD.Value = numericUpDown_DOW_handX_centr.Value;
            numericUpDown_DOW_handY_centr_AOD.Value = numericUpDown_DOW_handY_centr.Value;
            numericUpDown_DOW_handX_background_AOD.Value = numericUpDown_DOW_handX_background.Value;
            numericUpDown_DOW_handY_background_AOD.Value = numericUpDown_DOW_handY_background.Value;
            numericUpDown_DOW_handX_offset_AOD.Value = numericUpDown_DOW_handX_offset.Value;
            numericUpDown_DOW_handY_offset_AOD.Value = numericUpDown_DOW_handY_offset.Value;
            numericUpDown_DOW_hand_startAngle_AOD.Value = numericUpDown_DOW_hand_startAngle.Value;
            numericUpDown_DOW_hand_endAngle_AOD.Value = numericUpDown_DOW_hand_endAngle.Value;
        }



        private void Copy_pictures_AOD(UserControl_pictures userControl_pictures, UserControl_pictures userControl_pictures_AOD)
        {
            userControl_pictures_AOD.checkBox_pictures_Use.Checked = userControl_pictures.checkBox_pictures_Use.Checked;
            userControl_pictures_AOD.comboBoxSetImage(userControl_pictures.comboBoxGetImage());
            userControl_pictures_AOD.numericUpDown_picturesX.Value = userControl_pictures.numericUpDown_picturesX.Value;
            userControl_pictures_AOD.numericUpDown_picturesY.Value = userControl_pictures.numericUpDown_picturesY.Value;
            userControl_pictures_AOD.numericUpDown_pictures_count.Value = userControl_pictures.numericUpDown_pictures_count.Value;
        }
        private void Copy_text_AOD(UserControl_text userControl_text, UserControl_text userControl_text_AOD)
        {
            userControl_text_AOD.checkBox_Use.Checked = userControl_text.checkBox_Use.Checked;
            userControl_text_AOD.comboBoxSetImage(userControl_text.comboBoxGetImage());
            userControl_text_AOD.comboBoxSetIcon(userControl_text.comboBoxGetIcon());
            userControl_text_AOD.comboBoxSetUnit(userControl_text.comboBoxGetUnit());
            userControl_text_AOD.numericUpDown_imageX.Value = userControl_text.numericUpDown_imageX.Value;
            userControl_text_AOD.numericUpDown_imageY.Value = userControl_text.numericUpDown_imageY.Value;
            userControl_text_AOD.numericUpDown_iconX.Value = userControl_text.numericUpDown_iconX.Value;
            userControl_text_AOD.numericUpDown_iconY.Value = userControl_text.numericUpDown_iconY.Value;
            userControl_text_AOD.comboBoxSetAlignment(userControl_text.comboBoxGetAlignment());
            userControl_text_AOD.numericUpDown_spacing.Value = userControl_text.numericUpDown_spacing.Value;
            userControl_text_AOD.checkBox_addZero.Checked = userControl_text.checkBox_addZero.Checked;
            userControl_text_AOD.comboBoxSetImageError(userControl_text.comboBoxGetImageError());
            userControl_text_AOD.comboBoxSetImageDecimalPointOrMinus(userControl_text.comboBoxGetImageDecimalPointOrMinus());

        }
        private void Copy_hand_AOD(UserControl_hand userControl_hand, UserControl_hand userControl_hand_AOD)
        {
            userControl_hand_AOD.checkBox_hand_Use.Checked = userControl_hand.checkBox_hand_Use.Checked;
            userControl_hand_AOD.comboBoxSetHandImage(userControl_hand.comboBoxGetHandImage());
            userControl_hand_AOD.numericUpDown_handX.Value = userControl_hand.numericUpDown_handX.Value;
            userControl_hand_AOD.numericUpDown_handY.Value = userControl_hand.numericUpDown_handY.Value;
            userControl_hand_AOD.numericUpDown_handX_offset.Value = userControl_hand.numericUpDown_handX_offset.Value;
            userControl_hand_AOD.numericUpDown_handY_offset.Value = userControl_hand.numericUpDown_handY_offset.Value;
            userControl_hand_AOD.comboBoxSetHandImageCentr(userControl_hand.comboBoxGetHandImageCentr());
            userControl_hand_AOD.numericUpDown_handX_centr.Value = userControl_hand.numericUpDown_handX_centr.Value;
            userControl_hand_AOD.numericUpDown_handY_centr.Value = userControl_hand.numericUpDown_handY_centr.Value;
            userControl_hand_AOD.numericUpDown_hand_startAngle.Value = userControl_hand.numericUpDown_hand_startAngle.Value;
            userControl_hand_AOD.numericUpDown_hand_endAngle.Value = userControl_hand.numericUpDown_hand_endAngle.Value;
            userControl_hand_AOD.comboBoxSetHandImageBackground(userControl_hand.comboBoxGetHandImageBackground());
            userControl_hand_AOD.numericUpDown_handX_background.Value = userControl_hand.numericUpDown_handX_background.Value;
            userControl_hand_AOD.numericUpDown_handY_background.Value = userControl_hand.numericUpDown_handY_background.Value;

        }
        private void Copy_scaleCircle_AOD(UserControl_scaleCircle userControl_scaleCircle, UserControl_scaleCircle userControl_scaleCircle_AOD)
        {
            userControl_scaleCircle_AOD.checkBox_scaleCircle_Use.Checked = userControl_scaleCircle.checkBox_scaleCircle_Use.Checked;
            userControl_scaleCircle_AOD.radioButton_scaleCircle_image.Checked = userControl_scaleCircle.radioButton_scaleCircle_image.Checked;
            userControl_scaleCircle_AOD.radioButton_scaleCircle_color.Checked = userControl_scaleCircle.radioButton_scaleCircle_color.Checked;
            userControl_scaleCircle_AOD.comboBoxSetImage(userControl_scaleCircle.comboBoxGetImage());
            userControl_scaleCircle_AOD.comboBoxSetColorString(userControl_scaleCircle.comboBoxGetColorString());
            userControl_scaleCircle_AOD.comboBoxSetFlatness(userControl_scaleCircle.comboBoxGetFlatness());
            userControl_scaleCircle_AOD.comboBoxSetImageBackground(userControl_scaleCircle.comboBoxGetImageBackground());
            userControl_scaleCircle_AOD.numericUpDown_scaleCircleX.Value = userControl_scaleCircle.numericUpDown_scaleCircleX.Value;
            userControl_scaleCircle_AOD.numericUpDown_scaleCircleY.Value = userControl_scaleCircle.numericUpDown_scaleCircleY.Value;
            userControl_scaleCircle_AOD.numericUpDown_scaleCircle_radius.Value = userControl_scaleCircle.numericUpDown_scaleCircle_radius.Value;
            userControl_scaleCircle_AOD.numericUpDown_scaleCircle_width.Value = userControl_scaleCircle.numericUpDown_scaleCircle_width.Value;
            userControl_scaleCircle_AOD.numericUpDown_scaleCircle_startAngle.Value = userControl_scaleCircle.numericUpDown_scaleCircle_startAngle.Value;
            userControl_scaleCircle_AOD.numericUpDown_scaleCircle_endAngle.Value = userControl_scaleCircle.numericUpDown_scaleCircle_endAngle.Value;
        }
        private void Copy_scaleLinear_AOD(UserControl_scaleLinear userControl_scaleLinear, UserControl_scaleLinear userControl_scaleLinear_AOD)
        {
            userControl_scaleLinear_AOD.checkBox_scaleLinear_Use.Checked = userControl_scaleLinear.checkBox_scaleLinear_Use.Checked;
            userControl_scaleLinear_AOD.radioButton_scaleLinear_image.Checked = userControl_scaleLinear.radioButton_scaleLinear_image.Checked;
            userControl_scaleLinear_AOD.radioButton_scaleLinear_color.Checked = userControl_scaleLinear.radioButton_scaleLinear_color.Checked;
            userControl_scaleLinear_AOD.comboBoxSetImage(userControl_scaleLinear.comboBoxGetImage());
            userControl_scaleLinear_AOD.comboBoxSetColorString(userControl_scaleLinear.comboBoxGetColorString());
            userControl_scaleLinear_AOD.comboBoxSetImagePointer(userControl_scaleLinear.comboBoxGetImagePointer());
            userControl_scaleLinear_AOD.comboBoxSetImageBackground(userControl_scaleLinear.comboBoxGetImageBackground());
            userControl_scaleLinear_AOD.numericUpDown_scaleLinearX.Value = userControl_scaleLinear.numericUpDown_scaleLinearX.Value;
            userControl_scaleLinear_AOD.numericUpDown_scaleLinearY.Value = userControl_scaleLinear.numericUpDown_scaleLinearY.Value;
            userControl_scaleLinear_AOD.numericUpDown_scaleLinear_length.Value = userControl_scaleLinear.numericUpDown_scaleLinear_length.Value;
            userControl_scaleLinear_AOD.numericUpDown_scaleLinear_width.Value = userControl_scaleLinear.numericUpDown_scaleLinear_width.Value;
            userControl_scaleLinear_AOD.comboBoxSetFlatness(userControl_scaleLinear.comboBoxGetFlatness());
        }

        private void Copy_pictures_AOD(Panel panel_MainScreen, Panel panel_AOD)
        {
            CheckBox checkBox_Use = (CheckBox)panel_MainScreen.Controls[0];
            ComboBox comboBox_image = (ComboBox)panel_MainScreen.Controls[1];
            NumericUpDown numericUpDownX = (NumericUpDown)panel_MainScreen.Controls[2];
            NumericUpDown numericUpDownY = (NumericUpDown)panel_MainScreen.Controls[3];
            NumericUpDown numericUpDown_count = (NumericUpDown)panel_MainScreen.Controls[4];

            CheckBox checkBox_Use_AOD = (CheckBox)panel_AOD.Controls[0];
            ComboBox comboBox_image_AOD = (ComboBox)panel_AOD.Controls[1];
            NumericUpDown numericUpDownX_AOD = (NumericUpDown)panel_AOD.Controls[2];
            NumericUpDown numericUpDownY_AOD = (NumericUpDown)panel_AOD.Controls[3];
            NumericUpDown numericUpDown_count_AOD = (NumericUpDown)panel_AOD.Controls[4];

            checkBox_Use_AOD.Checked = checkBox_Use.Checked;
            comboBox_image_AOD.SelectedIndex = comboBox_image.SelectedIndex;
            numericUpDownX_AOD.Value = numericUpDownX.Value;
            numericUpDownY_AOD.Value = numericUpDownY.Value;
            numericUpDown_count_AOD.Value = numericUpDown_count.Value;
        }
        private void Copy_text_AOD(Panel panel_MainScreen, Panel panel_AOD)
        {
            CheckBox checkBox_Use = (CheckBox)panel_MainScreen.Controls[0];
            ComboBox comboBox_image = (ComboBox)panel_MainScreen.Controls[1];
            ComboBox comboBox_unit = (ComboBox)panel_MainScreen.Controls[2];
            ComboBox comboBox_separator = (ComboBox)panel_MainScreen.Controls[3];
            NumericUpDown numericUpDownX = (NumericUpDown)panel_MainScreen.Controls[4];
            NumericUpDown numericUpDownY = (NumericUpDown)panel_MainScreen.Controls[5];
            NumericUpDown numericUpDown_unitX = (NumericUpDown)panel_MainScreen.Controls[6];
            NumericUpDown numericUpDown_unitY = (NumericUpDown)panel_MainScreen.Controls[7];
            ComboBox comboBox_alignment = (ComboBox)panel_MainScreen.Controls[8];
            NumericUpDown numericUpDown_spacing = (NumericUpDown)panel_MainScreen.Controls[9];
            CheckBox checkBox_add_zero = (CheckBox)panel_MainScreen.Controls[10];
            ComboBox comboBox_imageError = (ComboBox)panel_MainScreen.Controls[11];

            CheckBox checkBox_Use_AOD = (CheckBox)panel_AOD.Controls[0];
            ComboBox comboBox_image_AOD = (ComboBox)panel_AOD.Controls[1];
            ComboBox comboBox_unit_AOD = (ComboBox)panel_AOD.Controls[2];
            ComboBox comboBox_separator_AOD = (ComboBox)panel_AOD.Controls[3];
            NumericUpDown numericUpDownX_AOD = (NumericUpDown)panel_AOD.Controls[4];
            NumericUpDown numericUpDownY_AOD = (NumericUpDown)panel_AOD.Controls[5];
            NumericUpDown numericUpDown_unitX_AOD = (NumericUpDown)panel_AOD.Controls[6];
            NumericUpDown numericUpDown_unitY_AOD = (NumericUpDown)panel_AOD.Controls[7];
            ComboBox comboBox_alignment_AOD = (ComboBox)panel_AOD.Controls[8];
            NumericUpDown numericUpDown_spacing_AOD = (NumericUpDown)panel_AOD.Controls[9];
            CheckBox checkBox_add_zero_AOD = (CheckBox)panel_AOD.Controls[10];
            ComboBox comboBox_imageError_AOD = (ComboBox)panel_AOD.Controls[11];

            checkBox_Use_AOD.Checked = checkBox_Use.Checked;
            comboBox_image_AOD.SelectedIndex = comboBox_image.SelectedIndex;
            comboBox_unit_AOD.SelectedIndex = comboBox_unit.SelectedIndex;
            comboBox_separator_AOD.SelectedIndex = comboBox_separator.SelectedIndex;
            numericUpDownX_AOD.Value = numericUpDownX.Value;
            numericUpDownY_AOD.Value = numericUpDownY.Value;
            numericUpDown_unitX_AOD.Value = numericUpDown_unitX.Value;
            numericUpDown_unitY_AOD.Value = numericUpDown_unitY.Value;
            comboBox_alignment_AOD.SelectedIndex = comboBox_alignment.SelectedIndex;
            numericUpDown_spacing_AOD.Value = numericUpDown_spacing.Value;
            checkBox_add_zero_AOD.Checked = checkBox_add_zero.Checked;
            comboBox_imageError_AOD.SelectedIndex = comboBox_imageError.SelectedIndex;
        }
        private void Copy_hand_AOD(Panel panel_MainScreen, Panel panel_AOD)
        {
            CheckBox checkBox_Use = (CheckBox)panel_MainScreen.Controls[0];
            ComboBox comboBox_image = (ComboBox)panel_MainScreen.Controls[1];
            NumericUpDown numericUpDownX = (NumericUpDown)panel_MainScreen.Controls[2];
            NumericUpDown numericUpDownY = (NumericUpDown)panel_MainScreen.Controls[3];
            NumericUpDown numericUpDown_offsetX = (NumericUpDown)panel_MainScreen.Controls[4];
            NumericUpDown numericUpDown_offsetY = (NumericUpDown)panel_MainScreen.Controls[5];
            ComboBox comboBox_imageCentr = (ComboBox)panel_MainScreen.Controls[6];
            NumericUpDown numericUpDownX_centr = (NumericUpDown)panel_MainScreen.Controls[7];
            NumericUpDown numericUpDownY_centr = (NumericUpDown)panel_MainScreen.Controls[8];
            NumericUpDown numericUpDown_startAngle = (NumericUpDown)panel_MainScreen.Controls[9];
            NumericUpDown numericUpDown_endAngle = (NumericUpDown)panel_MainScreen.Controls[10];
            ComboBox comboBox_imageBackground = (ComboBox)panel_MainScreen.Controls[11];
            NumericUpDown numericUpDownX_background = (NumericUpDown)panel_MainScreen.Controls[12];
            NumericUpDown numericUpDownY_background = (NumericUpDown)panel_MainScreen.Controls[13];

            CheckBox checkBox_Use_AOD = (CheckBox)panel_AOD.Controls[0];
            ComboBox comboBox_image_AOD = (ComboBox)panel_AOD.Controls[1];
            NumericUpDown numericUpDownX_AOD = (NumericUpDown)panel_AOD.Controls[2];
            NumericUpDown numericUpDownY_AOD = (NumericUpDown)panel_AOD.Controls[3];
            NumericUpDown numericUpDown_offsetX_AOD = (NumericUpDown)panel_AOD.Controls[4];
            NumericUpDown numericUpDown_offsetY_AOD = (NumericUpDown)panel_AOD.Controls[5];
            ComboBox comboBox_imageCentr_AOD = (ComboBox)panel_AOD.Controls[6];
            NumericUpDown numericUpDownX_centr_AOD = (NumericUpDown)panel_AOD.Controls[7];
            NumericUpDown numericUpDownY_centr_AOD = (NumericUpDown)panel_AOD.Controls[8];
            NumericUpDown numericUpDown_startAngle_AOD = (NumericUpDown)panel_AOD.Controls[9];
            NumericUpDown numericUpDown_endAngle_AOD = (NumericUpDown)panel_AOD.Controls[10];
            ComboBox comboBox_imageBackground_AOD = (ComboBox)panel_AOD.Controls[11];
            NumericUpDown numericUpDownX_background_AOD = (NumericUpDown)panel_AOD.Controls[12];
            NumericUpDown numericUpDownY_background_AOD = (NumericUpDown)panel_AOD.Controls[13];

            checkBox_Use_AOD.Checked = checkBox_Use.Checked;
            comboBox_image_AOD.SelectedIndex = comboBox_image.SelectedIndex;
            numericUpDownX_AOD.Value = numericUpDownX.Value;
            numericUpDownY_AOD.Value = numericUpDownY.Value;
            numericUpDown_offsetX_AOD.Value = numericUpDown_offsetX.Value;
            numericUpDown_offsetY_AOD.Value = numericUpDown_offsetY.Value;
            comboBox_imageCentr_AOD.SelectedIndex = comboBox_imageCentr.SelectedIndex;
            numericUpDownX_centr_AOD.Value = numericUpDownX_centr.Value;
            numericUpDownY_centr_AOD.Value = numericUpDownY_centr.Value;
            numericUpDown_startAngle_AOD.Value = numericUpDown_startAngle.Value;
            numericUpDown_endAngle_AOD.Value = numericUpDown_endAngle.Value;
            comboBox_imageBackground_AOD.SelectedIndex = comboBox_imageBackground.SelectedIndex;
            numericUpDownX_background_AOD.Value = numericUpDownX_background.Value;
            numericUpDownY_background_AOD.Value = numericUpDownY_background.Value;
        }
        private void Copy_scaleCircle_AOD(Panel panel_MainScreen, Panel panel_AOD)
        {
            CheckBox checkBox_Use = (CheckBox)panel_MainScreen.Controls[0];
            RadioButton radioButton_image = (RadioButton)panel_MainScreen.Controls[1];
            RadioButton radioButton_color = (RadioButton)panel_MainScreen.Controls[2];
            ComboBox comboBox_image = (ComboBox)panel_MainScreen.Controls[3];
            ComboBox comboBox_color = (ComboBox)panel_MainScreen.Controls[4];
            ComboBox comboBox_flatness = (ComboBox)panel_MainScreen.Controls[5];
            ComboBox comboBox_background = (ComboBox)panel_MainScreen.Controls[6];
            NumericUpDown numericUpDownX = (NumericUpDown)panel_MainScreen.Controls[7];
            NumericUpDown numericUpDownY = (NumericUpDown)panel_MainScreen.Controls[8];
            NumericUpDown numericUpDown_radius = (NumericUpDown)panel_MainScreen.Controls[9];
            NumericUpDown numericUpDown_width = (NumericUpDown)panel_MainScreen.Controls[10];
            NumericUpDown numericUpDown_startAngle = (NumericUpDown)panel_MainScreen.Controls[11];
            NumericUpDown numericUpDown_endAngle = (NumericUpDown)panel_MainScreen.Controls[12];

            CheckBox checkBox_Use_AOD = (CheckBox)panel_AOD.Controls[0];
            RadioButton radioButton_image_AOD = (RadioButton)panel_AOD.Controls[1];
            RadioButton radioButton_color_AOD = (RadioButton)panel_AOD.Controls[2];
            ComboBox comboBox_image_AOD = (ComboBox)panel_AOD.Controls[3];
            ComboBox comboBox_color_AOD = (ComboBox)panel_AOD.Controls[4];
            ComboBox comboBox_flatness_AOD = (ComboBox)panel_AOD.Controls[5];
            ComboBox comboBox_background_AOD = (ComboBox)panel_AOD.Controls[6];
            NumericUpDown numericUpDownX_AOD = (NumericUpDown)panel_AOD.Controls[7];
            NumericUpDown numericUpDownY_AOD = (NumericUpDown)panel_AOD.Controls[8];
            NumericUpDown numericUpDown_radius_AOD = (NumericUpDown)panel_AOD.Controls[9];
            NumericUpDown numericUpDown_width_AOD = (NumericUpDown)panel_AOD.Controls[10];
            NumericUpDown numericUpDown_startAngle_AOD = (NumericUpDown)panel_AOD.Controls[11];
            NumericUpDown numericUpDown_endAngle_AOD = (NumericUpDown)panel_AOD.Controls[12];

            checkBox_Use_AOD.Checked = checkBox_Use.Checked;
            radioButton_image_AOD.Checked = radioButton_image.Checked;
            radioButton_color_AOD.Checked = radioButton_color.Checked;
            comboBox_image_AOD.SelectedIndex = comboBox_image.SelectedIndex;
            comboBox_color_AOD.SelectedIndex = comboBox_color.SelectedIndex;
            comboBox_flatness_AOD.SelectedIndex = comboBox_flatness.SelectedIndex;
            comboBox_background_AOD.SelectedIndex = comboBox_background.SelectedIndex;
            numericUpDownX_AOD.Value = numericUpDownX.Value;
            numericUpDownY_AOD.Value = numericUpDownY.Value;
            numericUpDown_radius_AOD.Value = numericUpDown_radius.Value;
            numericUpDown_width_AOD.Value = numericUpDown_width.Value;
            numericUpDown_startAngle_AOD.Value = numericUpDown_startAngle.Value;
            numericUpDown_endAngle_AOD.Value = numericUpDown_endAngle.Value;
        }
        private void Copy_scaleLinear_AOD(Panel panel_MainScreen, Panel panel_AOD)
        {
            CheckBox checkBox_Use = (CheckBox)panel_MainScreen.Controls[0];
            RadioButton radioButton_image = (RadioButton)panel_MainScreen.Controls[1];
            RadioButton radioButton_color = (RadioButton)panel_MainScreen.Controls[2];
            ComboBox comboBox_image = (ComboBox)panel_MainScreen.Controls[3];
            ComboBox comboBox_color = (ComboBox)panel_MainScreen.Controls[4];
            ComboBox comboBox_pointer = (ComboBox)panel_MainScreen.Controls[5];
            ComboBox comboBox_background = (ComboBox)panel_MainScreen.Controls[6];
            NumericUpDown numericUpDownX = (NumericUpDown)panel_MainScreen.Controls[7];
            NumericUpDown numericUpDownY = (NumericUpDown)panel_MainScreen.Controls[8];
            NumericUpDown numericUpDown_length = (NumericUpDown)panel_MainScreen.Controls[9];
            NumericUpDown numericUpDown_width = (NumericUpDown)panel_MainScreen.Controls[10];
            ComboBox comboBox_flatness = (ComboBox)panel_MainScreen.Controls[11];

            CheckBox checkBox_Use_AOD = (CheckBox)panel_AOD.Controls[0];
            RadioButton radioButton_image_AOD = (RadioButton)panel_AOD.Controls[1];
            RadioButton radioButton_color_AOD = (RadioButton)panel_AOD.Controls[2];
            ComboBox comboBox_image_AOD = (ComboBox)panel_AOD.Controls[3];
            ComboBox comboBox_color_AOD = (ComboBox)panel_AOD.Controls[4];
            ComboBox comboBox_pointer_AOD = (ComboBox)panel_AOD.Controls[5];
            ComboBox comboBox_background_AOD = (ComboBox)panel_AOD.Controls[6];
            NumericUpDown numericUpDownX_AOD = (NumericUpDown)panel_AOD.Controls[7];
            NumericUpDown numericUpDownY_AOD = (NumericUpDown)panel_AOD.Controls[8];
            NumericUpDown numericUpDown_length_AOD = (NumericUpDown)panel_AOD.Controls[9];
            NumericUpDown numericUpDown_width_AOD = (NumericUpDown)panel_AOD.Controls[10];
            ComboBox comboBox_flatness_AOD = (ComboBox)panel_AOD.Controls[11];

            checkBox_Use_AOD.Checked = checkBox_Use.Checked;
            radioButton_image_AOD.Checked = radioButton_image.Checked;
            radioButton_color_AOD.Checked = radioButton_color.Checked;
            comboBox_image_AOD.SelectedIndex = comboBox_image.SelectedIndex;
            comboBox_color_AOD.SelectedIndex = comboBox_color.SelectedIndex;
            comboBox_pointer_AOD.SelectedIndex = comboBox_pointer.SelectedIndex;
            comboBox_background_AOD.SelectedIndex = comboBox_background.SelectedIndex;
            numericUpDownX_AOD.Value = numericUpDownX.Value;
            numericUpDownY_AOD.Value = numericUpDownY.Value;
            numericUpDown_length_AOD.Value = numericUpDown_length.Value;
            numericUpDown_width_AOD.Value = numericUpDown_width.Value;
            comboBox_flatness_AOD.SelectedIndex = comboBox_flatness.SelectedIndex;
        }

        private void Copy_text_Distance_AOD(Panel panel_MainScreen, Panel panel_AOD)
        {
            CheckBox checkBox_Use = (CheckBox)panel_MainScreen.Controls[0];
            ComboBox comboBox_image = (ComboBox)panel_MainScreen.Controls[1];
            ComboBox comboBox_unit = (ComboBox)panel_MainScreen.Controls[2];
            ComboBox comboBox_separator = (ComboBox)panel_MainScreen.Controls[3];
            NumericUpDown numericUpDownX = (NumericUpDown)panel_MainScreen.Controls[4];
            NumericUpDown numericUpDownY = (NumericUpDown)panel_MainScreen.Controls[5];
            NumericUpDown numericUpDown_unitX = (NumericUpDown)panel_MainScreen.Controls[6];
            NumericUpDown numericUpDown_unitY = (NumericUpDown)panel_MainScreen.Controls[7];
            ComboBox comboBox_alignment = (ComboBox)panel_MainScreen.Controls[8];
            NumericUpDown numericUpDown_spacing = (NumericUpDown)panel_MainScreen.Controls[9];
            CheckBox checkBox_add_zero = (CheckBox)panel_MainScreen.Controls[10];
            ComboBox comboBox_imageError = (ComboBox)panel_MainScreen.Controls[11];
            ComboBox comboBox_DecimalPoint = (ComboBox)panel_MainScreen.Controls[12];

            CheckBox checkBox_Use_AOD = (CheckBox)panel_AOD.Controls[0];
            ComboBox comboBox_image_AOD = (ComboBox)panel_AOD.Controls[1];
            ComboBox comboBox_unit_AOD = (ComboBox)panel_AOD.Controls[2];
            ComboBox comboBox_separator_AOD = (ComboBox)panel_AOD.Controls[3];
            NumericUpDown numericUpDownX_AOD = (NumericUpDown)panel_AOD.Controls[4];
            NumericUpDown numericUpDownY_AOD = (NumericUpDown)panel_AOD.Controls[5];
            NumericUpDown numericUpDown_unitX_AOD = (NumericUpDown)panel_AOD.Controls[6];
            NumericUpDown numericUpDown_unitY_AOD = (NumericUpDown)panel_AOD.Controls[7];
            ComboBox comboBox_alignment_AOD = (ComboBox)panel_AOD.Controls[8];
            NumericUpDown numericUpDown_spacing_AOD = (NumericUpDown)panel_AOD.Controls[9];
            CheckBox checkBox_add_zero_AOD = (CheckBox)panel_AOD.Controls[10];
            ComboBox comboBox_imageError_AOD = (ComboBox)panel_AOD.Controls[11];
            ComboBox comboBox_DecimalPoint_AOD = (ComboBox)panel_AOD.Controls[12];

            checkBox_Use_AOD.Checked = checkBox_Use.Checked;
            comboBox_image_AOD.SelectedIndex = comboBox_image.SelectedIndex;
            comboBox_unit_AOD.SelectedIndex = comboBox_unit.SelectedIndex;
            comboBox_separator_AOD.SelectedIndex = comboBox_separator.SelectedIndex;
            numericUpDownX_AOD.Value = numericUpDownX.Value;
            numericUpDownY_AOD.Value = numericUpDownY.Value;
            numericUpDown_unitX_AOD.Value = numericUpDown_unitX.Value;
            numericUpDown_unitY_AOD.Value = numericUpDown_unitY.Value;
            comboBox_alignment_AOD.SelectedIndex = comboBox_alignment.SelectedIndex;
            numericUpDown_spacing_AOD.Value = numericUpDown_spacing.Value;
            checkBox_add_zero_AOD.Checked = checkBox_add_zero.Checked;
            comboBox_imageError_AOD.SelectedIndex = comboBox_imageError.SelectedIndex;
            comboBox_DecimalPoint_AOD.SelectedIndex = comboBox_DecimalPoint.SelectedIndex;
        }

        private void Copy_Weather_textMin_AOD(Panel panel_MainScreen, Panel panel_AOD)
        {
            CheckBox checkBox_Use = (CheckBox)panel_MainScreen.Controls[0];
            ComboBox comboBox_image = (ComboBox)panel_MainScreen.Controls[1];
            ComboBox comboBox_unit = (ComboBox)panel_MainScreen.Controls[2];
            ComboBox comboBox_separatorF = (ComboBox)panel_MainScreen.Controls[3];
            NumericUpDown numericUpDownX = (NumericUpDown)panel_MainScreen.Controls[4];
            NumericUpDown numericUpDownY = (NumericUpDown)panel_MainScreen.Controls[5];
            NumericUpDown numericUpDown_unitX = (NumericUpDown)panel_MainScreen.Controls[6];
            NumericUpDown numericUpDown_unitY = (NumericUpDown)panel_MainScreen.Controls[7];
            ComboBox comboBox_alignment = (ComboBox)panel_MainScreen.Controls[8];
            NumericUpDown numericUpDown_spacing = (NumericUpDown)panel_MainScreen.Controls[9];
            //CheckBox checkBox_add_zero = (CheckBox)panel_MainScreen.Controls[10];
            ComboBox comboBox_imageError = (ComboBox)panel_MainScreen.Controls[10];
            ComboBox comboBox_imageMinus = (ComboBox)panel_MainScreen.Controls[11];

            CheckBox checkBox_Use_AOD = (CheckBox)panel_AOD.Controls[0];
            ComboBox comboBox_image_AOD = (ComboBox)panel_AOD.Controls[1];
            ComboBox comboBox_unit_AOD = (ComboBox)panel_AOD.Controls[2];
            ComboBox comboBox_separatorF_AOD = (ComboBox)panel_AOD.Controls[3];
            NumericUpDown numericUpDownX_AOD = (NumericUpDown)panel_AOD.Controls[4];
            NumericUpDown numericUpDownY_AOD = (NumericUpDown)panel_AOD.Controls[5];
            NumericUpDown numericUpDown_unitX_AOD = (NumericUpDown)panel_AOD.Controls[6];
            NumericUpDown numericUpDown_unitY_AOD = (NumericUpDown)panel_AOD.Controls[7];
            ComboBox comboBox_alignment_AOD = (ComboBox)panel_AOD.Controls[8];
            NumericUpDown numericUpDown_spacing_AOD = (NumericUpDown)panel_AOD.Controls[9];
            //CheckBox checkBox_add_zero = (CheckBox)panel_AOD.Controls[10];
            ComboBox comboBox_imageError_AOD = (ComboBox)panel_AOD.Controls[10];
            ComboBox comboBox_imageMinus_AOD = (ComboBox)panel_AOD.Controls[11];

            checkBox_Use_AOD.Checked = checkBox_Use.Checked;
            comboBox_image_AOD.SelectedIndex = comboBox_image.SelectedIndex;
            comboBox_unit_AOD.SelectedIndex = comboBox_unit.SelectedIndex;
            comboBox_separatorF_AOD.SelectedIndex = comboBox_separatorF.SelectedIndex;
            numericUpDownX_AOD.Value = numericUpDownX.Value;
            numericUpDownY_AOD.Value = numericUpDownY.Value;
            numericUpDown_unitX_AOD.Value = numericUpDown_unitX.Value;
            numericUpDown_unitY_AOD.Value = numericUpDown_unitY.Value;
            comboBox_alignment_AOD.SelectedIndex = comboBox_alignment.SelectedIndex;
            numericUpDown_spacing_AOD.Value = numericUpDown_spacing.Value;
            //checkBox_add_zero_AOD.Checked = checkBox_add_zero.Checked;
            comboBox_imageError_AOD.SelectedIndex = comboBox_imageError.SelectedIndex;
            comboBox_imageMinus_AOD.SelectedIndex = comboBox_imageMinus.SelectedIndex;
        }
        private void Copy_Weather_textMax_AOD(Panel panel_MainScreen, Panel panel_AOD)
        {
            CheckBox checkBox_Use = (CheckBox)panel_MainScreen.Controls[0];
            ComboBox comboBox_image = (ComboBox)panel_MainScreen.Controls[1];
            ComboBox comboBox_unit = (ComboBox)panel_MainScreen.Controls[2];
            ComboBox comboBox_separatorF = (ComboBox)panel_MainScreen.Controls[3];
            NumericUpDown numericUpDownX = (NumericUpDown)panel_MainScreen.Controls[4];
            NumericUpDown numericUpDownY = (NumericUpDown)panel_MainScreen.Controls[5];
            NumericUpDown numericUpDown_unitX = (NumericUpDown)panel_MainScreen.Controls[6];
            NumericUpDown numericUpDown_unitY = (NumericUpDown)panel_MainScreen.Controls[7];
            ComboBox comboBox_alignment = (ComboBox)panel_MainScreen.Controls[8];
            NumericUpDown numericUpDown_spacing = (NumericUpDown)panel_MainScreen.Controls[9];
            //CheckBox checkBox_add_zero = (CheckBox)panel_MainScreen.Controls[10];
            ComboBox comboBox_imageError = (ComboBox)panel_MainScreen.Controls[10];
            ComboBox comboBox_imageMinus = (ComboBox)panel_MainScreen.Controls[11];
            CheckBox checkBox_follow = (CheckBox)panel_MainScreen.Controls[12];

            CheckBox checkBox_Use_AOD = (CheckBox)panel_AOD.Controls[0];
            ComboBox comboBox_image_AOD = (ComboBox)panel_AOD.Controls[1];
            ComboBox comboBox_unit_AOD = (ComboBox)panel_AOD.Controls[2];
            ComboBox comboBox_separatorF_AOD = (ComboBox)panel_AOD.Controls[3];
            NumericUpDown numericUpDownX_AOD = (NumericUpDown)panel_AOD.Controls[4];
            NumericUpDown numericUpDownY_AOD = (NumericUpDown)panel_AOD.Controls[5];
            NumericUpDown numericUpDown_unitX_AOD = (NumericUpDown)panel_AOD.Controls[6];
            NumericUpDown numericUpDown_unitY_AOD = (NumericUpDown)panel_AOD.Controls[7];
            ComboBox comboBox_alignment_AOD = (ComboBox)panel_AOD.Controls[8];
            NumericUpDown numericUpDown_spacing_AOD = (NumericUpDown)panel_AOD.Controls[9];
            //CheckBox checkBox_add_zero = (CheckBox)panel_AOD.Controls[10];
            ComboBox comboBox_imageError_AOD = (ComboBox)panel_AOD.Controls[10];
            ComboBox comboBox_imageMinus_AOD = (ComboBox)panel_AOD.Controls[11];
            CheckBox checkBox_follow_AOD = (CheckBox)panel_AOD.Controls[12];

            checkBox_Use_AOD.Checked = checkBox_Use.Checked;
            comboBox_image_AOD.SelectedIndex = comboBox_image.SelectedIndex;
            comboBox_unit_AOD.SelectedIndex = comboBox_unit.SelectedIndex;
            comboBox_separatorF_AOD.SelectedIndex = comboBox_separatorF.SelectedIndex;
            numericUpDownX_AOD.Value = numericUpDownX.Value;
            numericUpDownY_AOD.Value = numericUpDownY.Value;
            numericUpDown_unitX_AOD.Value = numericUpDown_unitX.Value;
            numericUpDown_unitY_AOD.Value = numericUpDown_unitY.Value;
            comboBox_alignment_AOD.SelectedIndex = comboBox_alignment.SelectedIndex;
            numericUpDown_spacing_AOD.Value = numericUpDown_spacing.Value;
            //checkBox_add_zero_AOD.Checked = checkBox_add_zero.Checked;
            comboBox_imageError_AOD.SelectedIndex = comboBox_imageError.SelectedIndex;
            comboBox_imageMinus_AOD.SelectedIndex = comboBox_imageMinus.SelectedIndex;
            checkBox_follow_AOD.Checked = checkBox_follow.Checked;
        }



        private void userControl_pictures_Battery_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_pictures_AOD(userControl_pictures_Battery, userControl_pictures_Battery_AOD);
        }
        private void userControl_text_Battery_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_text_AOD(userControl_text_Battery, userControl_text_Battery_AOD);
        }
        private void userControl_hand_Battery_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_hand_AOD(userControl_hand_Battery, userControl_hand_Battery_AOD);
        }
        private void userControl_scaleCircle_Battery_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_scaleCircle_AOD(userControl_scaleCircle_Battery, userControl_scaleCircle_Battery_AOD);
        }
        private void userControl_scaleLinear_Battery_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_scaleLinear_AOD(userControl_scaleLinear_Battery, userControl_scaleLinear_Battery_AOD);
        }

        private void userControl_pictures_Steps_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_pictures_AOD(userControl_pictures_Steps, userControl_pictures_Steps_AOD);
        }
        private void userControl_text_Steps_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_text_AOD(userControl_text_Steps, userControl_text_Steps_AOD);
        }
        private void userControl_hand_Steps_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_hand_AOD(userControl_hand_Steps, userControl_hand_Steps_AOD);
        }
        private void userControl_scaleCircle_Steps_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_scaleCircle_AOD(userControl_scaleCircle_Steps, userControl_scaleCircle_Steps_AOD);
        }
        private void userControl_scaleLinear_Steps_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_scaleLinear_AOD(userControl_scaleLinear_Steps, userControl_scaleLinear_Steps_AOD);
        }

        private void userControl_pictures_Calories_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_pictures_AOD(userControl_pictures_Calories, userControl_pictures_Calories_AOD);
        }
        private void userControl_text_Calories_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_text_AOD(userControl_text_Calories, userControl_text_Calories_AOD);
        }
        private void userControl_hand_Calories_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_hand_AOD(userControl_hand_Calories, userControl_hand_Calories_AOD);
        }
        private void userControl_scaleCircle_Calories_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_scaleCircle_AOD(userControl_scaleCircle_Calories, userControl_scaleCircle_Calories_AOD);
        }
        private void userControl_scaleLinear_Calories_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_scaleLinear_AOD(userControl_scaleLinear_Calories, userControl_scaleLinear_Calories_AOD);
        }

        private void userControl_pictures_HeartRate_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_pictures_AOD(userControl_pictures_HeartRate, userControl_pictures_HeartRate_AOD);
        }
        private void userControl_text_HeartRate_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_text_AOD(userControl_text_HeartRate, userControl_text_HeartRate_AOD);
        }
        private void userControl_hand_HeartRate_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_hand_AOD(userControl_hand_HeartRate, userControl_hand_HeartRate_AOD);
        }
        private void userControl_scaleCircle_HeartRate_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_scaleCircle_AOD(userControl_scaleCircle_HeartRate, userControl_scaleCircle_HeartRate_AOD);
        }
        private void userControl_scaleLinear_HeartRate_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_scaleLinear_AOD(userControl_scaleLinear_HeartRate, userControl_scaleLinear_HeartRate_AOD);
        }

        private void userControl_pictures_PAI_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_pictures_AOD(userControl_pictures_PAI, userControl_pictures_PAI_AOD);
        }
        private void userControl_text_PAI_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_text_AOD(userControl_text_PAI, userControl_text_PAI_AOD);
        }
        private void userControl_hand_PAI_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_hand_AOD(userControl_hand_PAI, userControl_hand_PAI_AOD);
        }
        private void userControl_scaleCircle_PAI_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_scaleCircle_AOD(userControl_scaleCircle_PAI, userControl_scaleCircle_PAI_AOD);
        }
        private void userControl_scaleLinear_PAI_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_scaleLinear_AOD(userControl_scaleLinear_PAI, userControl_scaleLinear_PAI_AOD);
        }

        private void userControl_text_Distance_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_text_AOD(userControl_text_Distance, userControl_text_Distance_AOD);
        }

        private void userControl_pictures_StandUp_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_pictures_AOD(userControl_pictures_StandUp, userControl_pictures_StandUp_AOD);
        }
        private void userControl_text_StandUp_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_text_AOD(userControl_text_StandUp, userControl_text_StandUp_AOD);
        }
        private void userControl_hand_StandUp_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_hand_AOD(userControl_hand_StandUp, userControl_hand_StandUp_AOD);
        }
        private void userControl_scaleCircle_StandUp_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_scaleCircle_AOD(userControl_scaleCircle_StandUp, userControl_scaleCircle_StandUp_AOD);
        }
        private void userControl_scaleLinear_StandUp_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_scaleLinear_AOD(userControl_scaleLinear_StandUp, userControl_scaleLinear_StandUp_AOD);
        }









        private void button_Copy_Weather_pictures_AOD_Click(object sender, EventArgs e)
        {
            Copy_pictures_AOD(panel_Weather_pictures, panel_Weather_pictures_AOD);
        }

        private void button_Copy_Weather_text_AOD_Click(object sender, EventArgs e)
        {
            Copy_Weather_textMin_AOD(panel_Weather_text, panel_Weather_text_AOD);
        }

        private void button_Copy_Weather_textMin_AOD_Click(object sender, EventArgs e)
        {
            Copy_Weather_textMin_AOD(panel_Weather_textMin, panel_Weather_textMin_AOD);
        }

        private void button_Copy_Weather_textMax_AOD_Click(object sender, EventArgs e)
        {
            Copy_Weather_textMax_AOD(panel_Weather_textMax, panel_Weather_textMax_AOD);
        }

        private void button_Copy_Weather_hand_AOD_Click(object sender, EventArgs e)
        {
            Copy_hand_AOD(panel_Weather_hand, panel_Weather_hand_AOD);
        }

        private void button_Copy_Weather_scaleCircle_AOD_Click(object sender, EventArgs e)
        {
            Copy_scaleCircle_AOD(panel_Weather_scaleCircle, panel_Weather_scaleCircle_AOD);
        }

        private void button_Copy_Weather_scaleLinear_AOD_Click(object sender, EventArgs e)
        {
            Copy_scaleLinear_AOD(panel_Weather_scaleLinear, panel_Weather_scaleLinear_AOD);
        }






        private void userControl_pictures_Stress_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_pictures_AOD(userControl_pictures_Stress, userControl_pictures_Stress_AOD);
        }
        private void userControl_text_Stress_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_text_AOD(userControl_text_Stress, userControl_text_Stress_AOD);
        }
        private void userControl_hand_Stress_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_hand_AOD(userControl_hand_Stress, userControl_hand_Stress_AOD);
        }
        private void userControl_scaleCircle_Stress_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_scaleCircle_AOD(userControl_scaleCircle_Stress, userControl_scaleCircle_Stress_AOD);
        }
        private void userControl_scaleLinear_Stress_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_scaleLinear_AOD(userControl_scaleLinear_Stress, userControl_scaleLinear_Stress_AOD);
        }

        private void userControl_pictures_ActivityGoal_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_pictures_AOD(userControl_pictures_ActivityGoal, userControl_pictures_ActivityGoal_AOD);
        }
        private void userControl_text_ActivityGoal_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_text_AOD(userControl_text_ActivityGoal, userControl_text_ActivityGoal_AOD);
        }
        private void userControl_hand_ActivityGoal_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_hand_AOD(userControl_hand_ActivityGoal, userControl_hand_ActivityGoal_AOD);
        }
        private void userControl_scaleCircle_ActivityGoal_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_scaleCircle_AOD(userControl_scaleCircle_ActivityGoal, userControl_scaleCircle_ActivityGoal_AOD);
        }
        private void userControl_scaleLinear_ActivityGoal_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_scaleLinear_AOD(userControl_scaleLinear_ActivityGoal, userControl_scaleLinear_ActivityGoal_AOD);
        }

        private void userControl_pictures_FatBurning_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_pictures_AOD(userControl_pictures_FatBurning, userControl_pictures_FatBurning_AOD);
        }
        private void userControl_text_FatBurning_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_text_AOD(userControl_text_FatBurning, userControl_text_FatBurning_AOD);
        }
        private void userControl_hand_FatBurning_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_hand_AOD(userControl_hand_FatBurning, userControl_hand_FatBurning_AOD);
        }
        private void userControl_scaleCircle_FatBurning_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_scaleCircle_AOD(userControl_scaleCircle_FatBurning, userControl_scaleCircle_FatBurning_AOD);
        }
        private void userControl_scaleLinear_FatBurning_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Copy_scaleLinear_AOD(userControl_scaleLinear_FatBurning, userControl_scaleLinear_FatBurning_AOD);
        }


    }
}
