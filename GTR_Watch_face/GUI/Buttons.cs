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



    }
}
