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



        private void button_Hour_AOD_Click(object sender, EventArgs e)
        {
            if (panel_Hour_AOD.Height == 1)
            {
                panel_Hour_AOD.Height = (int)(215 * currentDPI);
            }
            else panel_Hour_AOD.Height = 1;

        }

        private void button_Minute_AOD_Click(object sender, EventArgs e)
        {
            if (panel_Minute_AOD.Height == 1)
            {
                panel_Minute_AOD.Height = (int)(215 * currentDPI);
            }
            else panel_Minute_AOD.Height = 1;
        }

        private void button_Hour_hand_AOD_Click(object sender, EventArgs e)
        {
            if (panel_Hour_hand_AOD.Height == 1)
            {
                panel_Hour_hand_AOD.Height = (int)(155 * currentDPI);
            }
            else panel_Hour_hand_AOD.Height = 1;
        }

        private void button_Minute_hand_AOD_Click(object sender, EventArgs e)
        {
            if (panel_Minute_hand_AOD.Height == 1)
            {
                panel_Minute_hand_AOD.Height = (int)(155 * currentDPI);
            }
            else panel_Minute_hand_AOD.Height = 1;
        }

        private void button_Day_text_AOD_Click(object sender, EventArgs e)
        {
            if (panel_Day_text_AOD.Height == 1)
            {
                panel_Day_text_AOD.Height = (int)(215 * currentDPI);
            }
            else panel_Day_text_AOD.Height = 1;
        }

        private void button_Day_hand_AOD_Click(object sender, EventArgs e)
        {
            if (panel_Day_hand_AOD.Height == 1)
            {
                panel_Day_hand_AOD.Height = (int)(225 * currentDPI);
            }
            else panel_Day_hand_AOD.Height = 1;
        }

        private void button_Month_pictures_AOD_Click(object sender, EventArgs e)
        {
            if (panel_Month_pictures_AOD.Height == 1)
            {
                panel_Month_pictures_AOD.Height = (int)(85 * currentDPI);
            }
            else panel_Month_pictures_AOD.Height = 1;
        }

        private void button_Month_text_AOD_Click(object sender, EventArgs e)
        {
            if (panel_Month_text_AOD.Height == 1)
            {
                panel_Month_text_AOD.Height = (int)(215 * currentDPI);
            }
            else panel_Month_text_AOD.Height = 1;
        }

        private void button_Month_hand_AOD_Click(object sender, EventArgs e)
        {
            if (panel_Month_hand_AOD.Height == 1)
            {
                panel_Month_hand_AOD.Height = (int)(225 * currentDPI);
            }
            else panel_Month_hand_AOD.Height = 1;
        }

        private void button_DOW_image_AOD_Click(object sender, EventArgs e)
        {
            if (panel_DOW_image_AOD.Height == 1)
            {
                panel_DOW_image_AOD.Height = (int)(85 * currentDPI);
            }
            else panel_DOW_image_AOD.Height = 1;
        }

        private void button_DOW_hand_AOD_Click(object sender, EventArgs e)
        {
            if (panel_DOW_hand_AOD.Height == 1)
            {
                panel_DOW_hand_AOD.Height = (int)(225 * currentDPI);
            }
            else panel_DOW_hand_AOD.Height = 1;
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

        private void button_Battery_pictures_AOD_Click(object sender, EventArgs e)
        {
            if (panel_Battery_pictures_AOD.Height == 1)
            {
                panel_Battery_pictures_AOD.Height = (int)(85 * currentDPI);
            }
            else panel_Battery_pictures_AOD.Height = 1;
        }

        private void button_Battery_text_AOD_Click(object sender, EventArgs e)
        {
            if (panel_Battery_text_AOD.Height == 1)
            {
                panel_Battery_text_AOD.Height = (int)(215 * currentDPI);
            }
            else panel_Battery_text_AOD.Height = 1;
        }

        private void button_Battery_hand_AOD_Click(object sender, EventArgs e)
        {
            if (panel_Battery_hand_AOD.Height == 1)
            {
                panel_Battery_hand_AOD.Height = (int)(225 * currentDPI);
            }
            else panel_Battery_hand_AOD.Height = 1;
        }

        private void button_Battery_scaleCircle_AOD_Click(object sender, EventArgs e)
        {
            if (panel_Battery_scaleCircle_AOD.Height == 1)
            {
                panel_Battery_scaleCircle_AOD.Height = (int)(215 * currentDPI);
            }
            else panel_Battery_scaleCircle_AOD.Height = 1;
        }

        private void button_Battery_scaleLinear_AOD_Click(object sender, EventArgs e)
        {
            if (panel_Battery_scaleLinear_AOD.Height == 1)
            {
                panel_Battery_scaleLinear_AOD.Height = (int)(155 * currentDPI);
            }
            else panel_Battery_scaleLinear_AOD.Height = 1;
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

        private void button_Steps_pictures_AOD_Click(object sender, EventArgs e)
        {
            if (panel_Steps_pictures_AOD.Height == 1)
            {
                panel_Steps_pictures_AOD.Height = (int)(85 * currentDPI);
            }
            else panel_Steps_pictures_AOD.Height = 1;
        }

        private void button_Steps_text_AOD_Click(object sender, EventArgs e)
        {
            if (panel_Steps_text_AOD.Height == 1)
            {
                panel_Steps_text_AOD.Height = (int)(215 * currentDPI);
            }
            else panel_Steps_text_AOD.Height = 1;
        }

        private void button_Steps_hand_AOD_Click(object sender, EventArgs e)
        {
            if (panel_Steps_hand_AOD.Height == 1)
            {
                panel_Steps_hand_AOD.Height = (int)(225 * currentDPI);
            }
            else panel_Steps_hand_AOD.Height = 1;
        }

        private void button_Steps_scaleCircle_AOD_Click(object sender, EventArgs e)
        {
            if (panel_Steps_scaleCircle_AOD.Height == 1)
            {
                panel_Steps_scaleCircle_AOD.Height = (int)(215 * currentDPI);
            }
            else panel_Steps_scaleCircle_AOD.Height = 1;
        }

        private void button_Steps_scaleLinear_AOD_Click(object sender, EventArgs e)
        {
            if (panel_Steps_scaleLinear_AOD.Height == 1)
            {
                panel_Steps_scaleLinear_AOD.Height = (int)(155 * currentDPI);
            }
            else panel_Steps_scaleLinear_AOD.Height = 1;
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

        private void button_Calories_pictures_AOD_Click(object sender, EventArgs e)
        {
            if (panel_Calories_pictures_AOD.Height == 1)
            {
                panel_Calories_pictures_AOD.Height = (int)(85 * currentDPI);
            }
            else panel_Calories_pictures_AOD.Height = 1;
        }

        private void button_Calories_text_AOD_Click(object sender, EventArgs e)
        {
            if (panel_Calories_text_AOD.Height == 1)
            {
                panel_Calories_text_AOD.Height = (int)(215 * currentDPI);
            }
            else panel_Calories_text_AOD.Height = 1;
        }

        private void button_Calories_hand_AOD_Click(object sender, EventArgs e)
        {
            if (panel_Calories_hand_AOD.Height == 1)
            {
                panel_Calories_hand_AOD.Height = (int)(225 * currentDPI);
            }
            else panel_Calories_hand_AOD.Height = 1;
        }

        private void button_Calories_scaleCircle_AOD_Click(object sender, EventArgs e)
        {
            if (panel_Calories_scaleCircle_AOD.Height == 1)
            {
                panel_Calories_scaleCircle_AOD.Height = (int)(215 * currentDPI);
            }
            else panel_Calories_scaleCircle_AOD.Height = 1;
        }

        private void button_Calories_scaleLinear_AOD_Click(object sender, EventArgs e)
        {
            if (panel_Calories_scaleLinear_AOD.Height == 1)
            {
                panel_Calories_scaleLinear_AOD.Height = (int)(155 * currentDPI);
            }
            else panel_Calories_scaleLinear_AOD.Height = 1;
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

        private void button_HeartRate_pictures_AOD_Click(object sender, EventArgs e)
        {
            if (panel_HeartRate_pictures_AOD.Height == 1)
            {
                panel_HeartRate_pictures_AOD.Height = (int)(85 * currentDPI);
            }
            else panel_HeartRate_pictures_AOD.Height = 1;
        }

        private void button_HeartRate_text_AOD_Click(object sender, EventArgs e)
        {
            if (panel_HeartRate_text_AOD.Height == 1)
            {
                panel_HeartRate_text_AOD.Height = (int)(215 * currentDPI);
            }
            else panel_HeartRate_text_AOD.Height = 1;
        }

        private void button_HeartRate_hand_AOD_Click(object sender, EventArgs e)
        {
            if (panel_HeartRate_hand_AOD.Height == 1)
            {
                panel_HeartRate_hand_AOD.Height = (int)(225 * currentDPI);
            }
            else panel_HeartRate_hand_AOD.Height = 1;
        }

        private void button_HeartRate_scaleCircle_AOD_Click(object sender, EventArgs e)
        {
            if (panel_HeartRate_scaleCircle_AOD.Height == 1)
            {
                panel_HeartRate_scaleCircle_AOD.Height = (int)(215 * currentDPI);
            }
            else panel_HeartRate_scaleCircle_AOD.Height = 1;
        }

        private void button_HeartRate_scaleLinear_AOD_Click(object sender, EventArgs e)
        {
            if (panel_HeartRate_scaleLinear_AOD.Height == 1)
            {
                panel_HeartRate_scaleLinear_AOD.Height = (int)(155 * currentDPI);
            }
            else panel_HeartRate_scaleLinear_AOD.Height = 1;
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

        private void button_PAI_pictures_AOD_Click(object sender, EventArgs e)
        {
            if (panel_PAI_pictures_AOD.Height == 1)
            {
                panel_PAI_pictures_AOD.Height = (int)(85 * currentDPI);
            }
            else panel_PAI_pictures_AOD.Height = 1;
        }

        private void button_PAI_text_AOD_Click(object sender, EventArgs e)
        {
            if (panel_PAI_text_AOD.Height == 1)
            {
                panel_PAI_text_AOD.Height = (int)(215 * currentDPI);
            }
            else panel_PAI_text_AOD.Height = 1;
        }

        private void button_PAI_hand_AOD_Click(object sender, EventArgs e)
        {
            if (panel_PAI_hand_AOD.Height == 1)
            {
                panel_PAI_hand_AOD.Height = (int)(225 * currentDPI);
            }
            else panel_PAI_hand_AOD.Height = 1;
        }

        private void button_PAI_scaleCircle_AOD_Click(object sender, EventArgs e)
        {
            if (panel_PAI_scaleCircle_AOD.Height == 1)
            {
                panel_PAI_scaleCircle_AOD.Height = (int)(215 * currentDPI);
            }
            else panel_PAI_scaleCircle_AOD.Height = 1;
        }

        private void button_PAI_scaleLinear_AOD_Click(object sender, EventArgs e)
        {
            if (panel_PAI_scaleLinear_AOD.Height == 1)
            {
                panel_PAI_scaleLinear_AOD.Height = (int)(155 * currentDPI);
            }
            else panel_PAI_scaleLinear_AOD.Height = 1;
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

            Copy_pictures_AOD(panel_Battery_pictures, panel_Battery_pictures_AOD);
            Copy_text_AOD(panel_Battery_text, panel_Battery_text_AOD);
            Copy_hand_AOD(panel_Battery_hand, panel_Battery_hand_AOD);
            Copy_scaleCircle_AOD(panel_Battery_scaleCircle, panel_Battery_scaleCircle_AOD);
            Copy_scaleLinear_AOD(panel_Battery_scaleLinear, panel_Battery_scaleLinear_AOD);

            Copy_pictures_AOD(panel_Steps_pictures, panel_Steps_pictures_AOD);
            Copy_text_AOD(panel_Steps_text, panel_Steps_text_AOD);
            Copy_hand_AOD(panel_Steps_hand, panel_Steps_hand_AOD);
            Copy_scaleCircle_AOD(panel_Steps_scaleCircle, panel_Steps_scaleCircle_AOD);
            Copy_scaleLinear_AOD(panel_Steps_scaleLinear, panel_Steps_scaleLinear_AOD);

            Copy_pictures_AOD(panel_Calories_pictures, panel_Calories_pictures_AOD);
            Copy_text_AOD(panel_Calories_text, panel_Calories_text_AOD);
            Copy_hand_AOD(panel_Calories_hand, panel_Calories_hand_AOD);
            Copy_scaleCircle_AOD(panel_Calories_scaleCircle, panel_Calories_scaleCircle_AOD);
            Copy_scaleLinear_AOD(panel_Calories_scaleLinear, panel_Calories_scaleLinear_AOD);

            Copy_pictures_AOD(panel_HeartRate_pictures, panel_HeartRate_pictures_AOD);
            Copy_text_AOD(panel_HeartRate_text, panel_HeartRate_text_AOD);
            Copy_hand_AOD(panel_HeartRate_hand, panel_HeartRate_hand_AOD);
            Copy_scaleCircle_AOD(panel_HeartRate_scaleCircle, panel_HeartRate_scaleCircle_AOD);
            Copy_scaleLinear_AOD(panel_HeartRate_scaleLinear, panel_HeartRate_scaleLinear_AOD);

            Copy_pictures_AOD(panel_PAI_pictures, panel_PAI_pictures_AOD);
            Copy_text_AOD(panel_PAI_text, panel_PAI_text_AOD);
            Copy_hand_AOD(panel_PAI_hand, panel_PAI_hand_AOD);
            Copy_scaleCircle_AOD(panel_PAI_scaleCircle, panel_PAI_scaleCircle_AOD);
            Copy_scaleLinear_AOD(panel_PAI_scaleLinear, panel_PAI_scaleLinear_AOD);

            Copy_text_Distance_AOD(panel_Distance_text, panel_Distance_text_AOD);

            Copy_pictures_AOD(panel_Weather_pictures, panel_Weather_pictures_AOD);
            Copy_Weather_textMin_AOD(panel_Weather_text, panel_Weather_text_AOD);
            Copy_Weather_textMin_AOD(panel_Weather_textMin, panel_Weather_textMin_AOD);
            Copy_Weather_textMax_AOD(panel_Weather_textMax, panel_Weather_textMax_AOD);
            Copy_hand_AOD(panel_Weather_hand, panel_Weather_hand_AOD);
            Copy_scaleCircle_AOD(panel_Weather_scaleCircle, panel_Weather_scaleCircle_AOD);
            Copy_scaleLinear_AOD(panel_Weather_scaleLinear, panel_Weather_scaleLinear_AOD);

            PreviewView = true;
            PreviewImage();
            JSON_write();
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


        private void button_Copy_Battery_pictures_AOD_Click(object sender, EventArgs e)
        {
            Copy_pictures_AOD(panel_Battery_pictures, panel_Battery_pictures_AOD);
        }
        private void button_Copy_Battery_text_AOD_Click(object sender, EventArgs e)
        {
            Copy_text_AOD(panel_Battery_text, panel_Battery_text_AOD);
        }
        private void button_Copy_Battery_hand_AOD_Click(object sender, EventArgs e)
        {
            Copy_hand_AOD(panel_Battery_hand, panel_Battery_hand_AOD);
        }
        private void button_Copy_Battery_scaleCircle_AOD_Click(object sender, EventArgs e)
        {
            Copy_scaleCircle_AOD(panel_Battery_scaleCircle, panel_Battery_scaleCircle_AOD);
        }
        private void button_Copy_Battery_scaleLinear_AOD_Click(object sender, EventArgs e)
        {
            Copy_scaleLinear_AOD(panel_Battery_scaleLinear, panel_Battery_scaleLinear_AOD);
        }

        private void button_Copy_Steps_pictures_AOD_Click(object sender, EventArgs e)
        {
            Copy_pictures_AOD(panel_Steps_pictures, panel_Steps_pictures_AOD);
        }
        private void button_Copy_Steps_text_AOD_Click(object sender, EventArgs e)
        {
            Copy_text_AOD(panel_Steps_text, panel_Steps_text_AOD);
        }
        private void button_Copy_Steps_hand_AOD_Click(object sender, EventArgs e)
        {
            Copy_hand_AOD(panel_Steps_hand, panel_Steps_hand_AOD);
        }
        private void button_Copy_Steps_scaleCircle_AOD_Click(object sender, EventArgs e)
        {
            Copy_scaleCircle_AOD(panel_Steps_scaleCircle, panel_Steps_scaleCircle_AOD);
        }
        private void button_Copy_Steps_scaleLinear_AOD_Click(object sender, EventArgs e)
        {
            Copy_scaleLinear_AOD(panel_Steps_scaleLinear, panel_Steps_scaleLinear_AOD);
        }

        private void button_Copy_Calories_pictures_AOD_Click(object sender, EventArgs e)
        {
            Copy_pictures_AOD(panel_Calories_pictures, panel_Calories_pictures_AOD);
        }
        private void button_Copy_Calories_text_AOD_Click(object sender, EventArgs e)
        {
            Copy_text_AOD(panel_Calories_text, panel_Calories_text_AOD);
        }
        private void button_Copy_Calories_hand_AOD_Click(object sender, EventArgs e)
        {
            Copy_hand_AOD(panel_Calories_hand, panel_Calories_hand_AOD);
        }
        private void button_Copy_Calories_scaleCircle_AOD_Click(object sender, EventArgs e)
        {
            Copy_scaleCircle_AOD(panel_Calories_scaleCircle, panel_Calories_scaleCircle_AOD);
        }
        private void button_Copy_Calories_scaleLinear_AOD_Click(object sender, EventArgs e)
        {
            Copy_scaleLinear_AOD(panel_Calories_scaleLinear, panel_Calories_scaleLinear_AOD);
        }

        private void button_Copy_HeartRate_pictures_AOD_Click(object sender, EventArgs e)
        {
            Copy_pictures_AOD(panel_HeartRate_pictures, panel_HeartRate_pictures_AOD);
        }
        private void button_Copy_HeartRate_text_AOD_Click(object sender, EventArgs e)
        {
            Copy_text_AOD(panel_HeartRate_text, panel_HeartRate_text_AOD);
        }
        private void button_Copy_HeartRate_hand_AOD_Click(object sender, EventArgs e)
        {
            Copy_hand_AOD(panel_HeartRate_hand, panel_HeartRate_hand_AOD);
        }
        private void button_Copy_HeartRate_scaleCircle_AOD_Click(object sender, EventArgs e)
        {
            Copy_scaleCircle_AOD(panel_HeartRate_scaleCircle, panel_HeartRate_scaleCircle_AOD);
        }
        private void button_Copy_HeartRate_scaleLinear_AOD_Click(object sender, EventArgs e)
        {
            Copy_scaleLinear_AOD(panel_HeartRate_scaleLinear, panel_HeartRate_scaleLinear_AOD);
        }

        private void button_Copy_PAI_pictures_AOD_Click(object sender, EventArgs e)
        {
            Copy_pictures_AOD(panel_PAI_pictures, panel_PAI_pictures_AOD);
        }
        private void button_Copy_PAI_text_AOD_Click(object sender, EventArgs e)
        {
            Copy_text_AOD(panel_PAI_text, panel_PAI_text_AOD);
        }
        private void button_Copy_PAI_hand_AOD_Click(object sender, EventArgs e)
        {
            Copy_hand_AOD(panel_PAI_hand, panel_PAI_hand_AOD);
        }
        private void button_Copy_PAI_scaleCircle_AOD_Click(object sender, EventArgs e)
        {
            Copy_scaleCircle_AOD(panel_PAI_scaleCircle, panel_PAI_scaleCircle_AOD);
        }
        private void button_Copy_PAI_scaleLinear_AOD_Click(object sender, EventArgs e)
        {
            Copy_scaleLinear_AOD(panel_PAI_scaleLinear, panel_PAI_scaleLinear_AOD);
        }

        private void button_Copy_Distance_text_AOD_Click(object sender, EventArgs e)
        {
            Copy_text_Distance_AOD(panel_Distance_text, panel_Distance_text_AOD);
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


    }
}
