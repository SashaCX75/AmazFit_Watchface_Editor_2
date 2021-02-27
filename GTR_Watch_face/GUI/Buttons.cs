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
        private void button_Hour_Click(object sender, EventArgs e)
        {
            if (panel_Hour.Height == 1)
            {
                panel_Hour.Height = (int)(215 * currentDPI);
                //panel_Minute.Height = 1;
                //panel_Second.Height = 1;
            }
            else panel_Hour.Height = 1;
        }

        private void button_Minute_Click(object sender, EventArgs e)
        {
            if (panel_Minute.Height == 1)
            {
                //panel_Hour.Height = 1;
                panel_Minute.Height = (int)(215 * currentDPI);
                //panel_Second.Height = 1;
            }
            else panel_Minute.Height = 1;
        }

        private void button_Second_Click(object sender, EventArgs e)
        {
            if (panel_Second.Height == 1)
            {
                //panel_Hour.Height = 1;
                //panel_Minute.Height = 1;
                panel_Second.Height = (int)(215 * currentDPI);
            }
            else panel_Second.Height = 1;
        }

        private void button_Hour_hand_Click(object sender, EventArgs e)
        {
            if (panel_Hour_hand.Height == 1)
            {
                panel_Hour_hand.Height = (int)(155 * currentDPI);
                //panel_Minute_hand.Height = 1;
                //panel_Second_hand.Height = 1;
            }
            else panel_Hour_hand.Height = 1;
        }

        private void button_Minute_hand_Click(object sender, EventArgs e)
        {
            if (panel_Minute_hand.Height == 1)
            {
                //panel_Hour_hand.Height = 1;
                panel_Minute_hand.Height = (int)(155 * currentDPI);
                //panel_Second_hand.Height = 1;
            }
            else panel_Minute_hand.Height = 1;
        }

        private void button_Second_hand_Click(object sender, EventArgs e)
        {
            if (panel_Second_hand.Height == 1)
            {
                //panel_Hour_hand.Height = 1;
                //panel_Minute_hand.Height = 1;
                panel_Second_hand.Height = (int)(155 * currentDPI);
            }
            else panel_Second_hand.Height = 1;
        }

        private void button_Day_text_Click(object sender, EventArgs e)
        {
            if (panel_Day_text.Height == 1)
            {
                panel_Day_text.Height = (int)(215 * currentDPI);
            }
            else panel_Day_text.Height = 1;
        }

        private void button_Day_hand_Click(object sender, EventArgs e)
        {
            if (panel_Day_hand.Height == 1)
            {
                panel_Day_hand.Height = (int)(225 * currentDPI);
            }
            else panel_Day_hand.Height = 1;
        }

        private void button_Month_image_Click(object sender, EventArgs e)
        {
            if (panel_Month_pictures.Height == 1)
            {
                panel_Month_pictures.Height = (int)(85 * currentDPI);
            }
            else panel_Month_pictures.Height = 1;
        }

        private void button_Month_text_Click(object sender, EventArgs e)
        {
            if (panel_Month_text.Height == 1)
            {
                panel_Month_text.Height = (int)(215 * currentDPI);
            }
            else panel_Month_text.Height = 1;
        }

        private void button_Month_hand_Click(object sender, EventArgs e)
        {
            if (panel_Month_hand.Height == 1)
            {
                panel_Month_hand.Height = (int)(225 * currentDPI);
            }
            else panel_Month_hand.Height = 1;
        }
        private void button_DOW_image_Click(object sender, EventArgs e)
        {
            if (panel_DOW_image.Height == 1)
            {
                panel_DOW_image.Height = (int)(85 * currentDPI);
            }
            else panel_DOW_image.Height = 1;
        }

        private void button_DOW_hand_Click(object sender, EventArgs e)
        {
            if (panel_DOW_hand.Height == 1)
            {
                panel_DOW_hand.Height = (int)(225 * currentDPI);
            }
            else panel_DOW_hand.Height = 1;
        }


        private void button_Battery_pictures_Click(object sender, EventArgs e)
        {
            if (panel_Battery_pictures.Height == 1)
            {
                panel_Battery_pictures.Height = (int)(85 * currentDPI);
            }
            else panel_Battery_pictures.Height = 1;
        }

        private void button_Battery_text_Click(object sender, EventArgs e)
        {
            if (panel_Battery_text.Height == 1)
            {
                panel_Battery_text.Height = (int)(215 * currentDPI);
            }
            else panel_Battery_text.Height = 1;
        }

        private void button_Battery_hand_Click(object sender, EventArgs e)
        {
            if (panel_Battery_hand.Height == 1)
            {
                panel_Battery_hand.Height = (int)(225 * currentDPI);
            }
            else panel_Battery_hand.Height = 1;
        }

        private void button_Battery_scaleCircle_Click(object sender, EventArgs e)
        {
            if (panel_Battery_scaleCircle.Height == 1)
            {
                panel_Battery_scaleCircle.Height = (int)(215 * currentDPI);
            }
            else panel_Battery_scaleCircle.Height = 1;
        }

        private void button_Battery_scaleLinear_Click(object sender, EventArgs e)
        {
            if (panel_Battery_scaleLinear.Height == 1)
            {
                panel_Battery_scaleLinear.Height = (int)(155 * currentDPI);
            }
            else panel_Battery_scaleLinear.Height = 1;
        }


        private void button_Steps_pictures_Click(object sender, EventArgs e)
        {
            if (panel_Steps_pictures.Height == 1)
            {
                panel_Steps_pictures.Height = (int)(85 * currentDPI);
            }
            else panel_Steps_pictures.Height = 1;
        }

        private void button_Steps_text_Click(object sender, EventArgs e)
        {
            if (panel_Steps_text.Height == 1)
            {
                panel_Steps_text.Height = (int)(215 * currentDPI);
            }
            else panel_Steps_text.Height = 1;
        }

        private void button_Steps_hand_Click(object sender, EventArgs e)
        {
            if (panel_Steps_hand.Height == 1)
            {
                panel_Steps_hand.Height = (int)(225 * currentDPI);
            }
            else panel_Steps_hand.Height = 1;
        }

        private void button_Steps_scaleCircle_Click(object sender, EventArgs e)
        {
            if (panel_Steps_scaleCircle.Height == 1)
            {
                panel_Steps_scaleCircle.Height = (int)(215 * currentDPI);
            }
            else panel_Steps_scaleCircle.Height = 1;
        }

        private void button_Steps_scaleLinear_Click(object sender, EventArgs e)
        {
            if (panel_Steps_scaleLinear.Height == 1)
            {
                panel_Steps_scaleLinear.Height = (int)(155 * currentDPI);
            }
            else panel_Steps_scaleLinear.Height = 1;
        }


        private void button_Calories_pictures_Click(object sender, EventArgs e)
        {
            if (panel_Calories_pictures.Height == 1)
            {
                panel_Calories_pictures.Height = (int)(85 * currentDPI);
            }
            else panel_Calories_pictures.Height = 1;
        }

        private void button_Calories_text_Click(object sender, EventArgs e)
        {
            if (panel_Calories_text.Height == 1)
            {
                panel_Calories_text.Height = (int)(215 * currentDPI);
            }
            else panel_Calories_text.Height = 1;
        }

        private void button_Calories_hand_Click(object sender, EventArgs e)
        {
            if (panel_Calories_hand.Height == 1)
            {
                panel_Calories_hand.Height = (int)(225 * currentDPI);
            }
            else panel_Calories_hand.Height = 1;
        }

        private void button_Calories_scaleCircle_Click(object sender, EventArgs e)
        {
            if (panel_Calories_scaleCircle.Height == 1)
            {
                panel_Calories_scaleCircle.Height = (int)(215 * currentDPI);
            }
            else panel_Calories_scaleCircle.Height = 1;
        }

        private void button_Calories_scaleLinear_Click(object sender, EventArgs e)
        {
            if (panel_Calories_scaleLinear.Height == 1)
            {
                panel_Calories_scaleLinear.Height = (int)(155 * currentDPI);
            }
            else panel_Calories_scaleLinear.Height = 1;
        }


        private void button_HeartRate_pictures_Click(object sender, EventArgs e)
        {
            if (panel_HeartRate_pictures.Height == 1)
            {
                panel_HeartRate_pictures.Height = (int)(85 * currentDPI);
            }
            else panel_HeartRate_pictures.Height = 1;
        }

        private void button_HeartRate_text_Click(object sender, EventArgs e)
        {
            if (panel_HeartRate_text.Height == 1)
            {
                panel_HeartRate_text.Height = (int)(215 * currentDPI);
            }
            else panel_HeartRate_text.Height = 1;
        }

        private void button_HeartRate_hand_Click(object sender, EventArgs e)
        {
            if (panel_HeartRate_hand.Height == 1)
            {
                panel_HeartRate_hand.Height = (int)(225 * currentDPI);
            }
            else panel_HeartRate_hand.Height = 1;
        }

        private void button_HeartRate_scaleCircle_Click(object sender, EventArgs e)
        {
            if (panel_HeartRate_scaleCircle.Height == 1)
            {
                panel_HeartRate_scaleCircle.Height = (int)(215 * currentDPI);
            }
            else panel_HeartRate_scaleCircle.Height = 1;
        }

        private void button_HeartRate_scaleLinear_Click(object sender, EventArgs e)
        {
            if (panel_HeartRate_scaleLinear.Height == 1)
            {
                panel_HeartRate_scaleLinear.Height = (int)(155 * currentDPI);
            }
            else panel_HeartRate_scaleLinear.Height = 1;
        }

        private void button_PAI_pictures_Click(object sender, EventArgs e)
        {
            if (panel_PAI_pictures.Height == 1)
            {
                panel_PAI_pictures.Height = (int)(85 * currentDPI);
            }
            else panel_PAI_pictures.Height = 1;
        }

        private void button_PAI_text_Click(object sender, EventArgs e)
        {
            if (panel_PAI_text.Height == 1)
            {
                panel_PAI_text.Height = (int)(215 * currentDPI);
            }
            else panel_PAI_text.Height = 1;
        }

        private void button_PAI_hand_Click(object sender, EventArgs e)
        {
            if (panel_PAI_hand.Height == 1)
            {
                panel_PAI_hand.Height = (int)(225 * currentDPI);
            }
            else panel_PAI_hand.Height = 1;
        }

        private void button_PAI_scaleCircle_Click(object sender, EventArgs e)
        {
            if (panel_PAI_scaleCircle.Height == 1)
            {
                panel_PAI_scaleCircle.Height = (int)(215 * currentDPI);
            }
            else panel_PAI_scaleCircle.Height = 1;
        }

        private void button_PAI_scaleLinear_Click(object sender, EventArgs e)
        {
            if (panel_PAI_scaleLinear.Height == 1)
            {
                panel_PAI_scaleLinear.Height = (int)(155 * currentDPI);
            }
            else panel_PAI_scaleLinear.Height = 1;
        }

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



    }
}
