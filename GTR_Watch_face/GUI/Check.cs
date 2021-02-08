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
        private void radioButton_Background_image_CheckedChanged(object sender, EventArgs e)
        {
            JSON_write();
            PreviewImage();
            bool b = radioButton_Background_image.Checked;
            comboBox_Background_image.Enabled = b;
            comboBox_Background_color.Enabled = !b;
        }

        private void checkBox_Hour_Use_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Hour_Use.Checked;
            comboBox_Hour_image.Enabled = b;
            comboBox_Hour_unit.Enabled = b;
            comboBox_Hour_separator.Enabled = b;
            numericUpDown_HourX.Enabled = b;
            numericUpDown_HourY.Enabled = b;
            numericUpDown_Hour_unitX.Enabled = b;
            numericUpDown_Hour_unitY.Enabled = b;
            comboBox_Hour_alignment.Enabled = b;
            checkBox_Hour_add_zero.Enabled = b;
            //comboBox_Hour_error.Enabled = b;
            numericUpDown_Hour_spacing.Enabled = b;

            label502.Enabled = b;
            label503.Enabled = b;
            label504.Enabled = b;
            label505.Enabled = b;
            label506.Enabled = b;
            label507.Enabled = b;
            label508.Enabled = b;
            label509.Enabled = b;
            label510.Enabled = b;
            label511.Enabled = b;
            label532.Enabled = b;
        }

        private void checkBox_Minute_Use_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Minute_Use.Checked;
            comboBox_Minute_image.Enabled = b;
            comboBox_Minute_unit.Enabled = b;
            comboBox_Minute_separator.Enabled = b;
            numericUpDown_MinuteX.Enabled = b;
            numericUpDown_MinuteY.Enabled = b;
            numericUpDown_Minute_unitX.Enabled = b;
            numericUpDown_Minute_unitY.Enabled = b;
            comboBox_Minute_alignment.Enabled = b;
            checkBox_Minute_add_zero.Enabled = b;
            //comboBox_Minute_error.Enabled = b;
            numericUpDown_Minute_spacing.Enabled = b;
            checkBox_Minute_follow.Enabled = b;

            label512.Enabled = b;
            label513.Enabled = b;
            label514.Enabled = b;
            label515.Enabled = b;
            label516.Enabled = b;
            label517.Enabled = b;
            label518.Enabled = b;
            label519.Enabled = b;
            label520.Enabled = b;
            label521.Enabled = b;
            label533.Enabled = b;

            if (checkBox_Minute_follow.Checked)
            {
                numericUpDown_MinuteX.Enabled = false;
                numericUpDown_MinuteY.Enabled = false;

                label514.Enabled = false;
                label516.Enabled = false;
                label533.Enabled = false;
            }
        }

        private void checkBox_Second_Use_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Second_Use.Checked;
            comboBox_Second_image.Enabled = b;
            comboBox_Second_unit.Enabled = b;
            comboBox_Second_separator.Enabled = b;
            numericUpDown_SecondX.Enabled = b;
            numericUpDown_SecondY.Enabled = b;
            numericUpDown_Second_unitX.Enabled = b;
            numericUpDown_Second_unitY.Enabled = b;
            comboBox_Second_alignment.Enabled = b;
            checkBox_Second_add_zero.Enabled = b;
            //comboBox_Second_error.Enabled = b;
            numericUpDown_Second_spacing.Enabled = b;
            checkBox_Second_follow.Enabled = b;

            label522.Enabled = b;
            label523.Enabled = b;
            label524.Enabled = b;
            label525.Enabled = b;
            label526.Enabled = b;
            label527.Enabled = b;
            label528.Enabled = b;
            label529.Enabled = b;
            label530.Enabled = b;
            label531.Enabled = b;
            label534.Enabled = b;

            if (checkBox_Second_follow.Checked)
            {
                numericUpDown_SecondX.Enabled = false;
                numericUpDown_SecondY.Enabled = false;

                label524.Enabled = false;
                label526.Enabled = false;
                label534.Enabled = false;
            }
        }

        private void checkBox_Hour_hand_Use_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Hour_hand_Use.Checked;
            comboBox_Hour_hand_image.Enabled = b;
            comboBox_Hour_hand_imageCentr.Enabled = b;
            numericUpDown_Hour_handX.Enabled = b;
            numericUpDown_Hour_handY.Enabled = b;
            numericUpDown_Hour_handX_centr.Enabled = b;
            numericUpDown_Hour_handY_centr.Enabled = b;
            numericUpDown_Hour_handX_offset.Enabled = b;
            numericUpDown_Hour_handY_offset.Enabled = b;

            label536.Enabled = b;
            label535.Enabled = b;
            label537.Enabled = b;
            label538.Enabled = b;
            label539.Enabled = b;
            label540.Enabled = b;
            label541.Enabled = b;
            label542.Enabled = b;
            label543.Enabled = b;
            label544.Enabled = b;
            label545.Enabled = b;
        }
        private void checkBox_Minute_hand_Use_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Minute_hand_Use.Checked;
            comboBox_Minute_hand_image.Enabled = b;
            comboBox_Minute_hand_imageCentr.Enabled = b;
            numericUpDown_Minute_handX.Enabled = b;
            numericUpDown_Minute_handY.Enabled = b;
            numericUpDown_Minute_handX_centr.Enabled = b;
            numericUpDown_Minute_handY_centr.Enabled = b;
            numericUpDown_Minute_handX_offset.Enabled = b;
            numericUpDown_Minute_handY_offset.Enabled = b;

            label546.Enabled = b;
            label547.Enabled = b;
            label548.Enabled = b;
            label549.Enabled = b;
            label550.Enabled = b;
            label551.Enabled = b;
            label552.Enabled = b;
            label553.Enabled = b;
            label554.Enabled = b;
            label555.Enabled = b;
            label556.Enabled = b;
        }
        private void checkBox_Second_hand_Use_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Second_hand_Use.Checked;
            comboBox_Second_hand_image.Enabled = b;
            comboBox_Second_hand_imageCentr.Enabled = b;
            numericUpDown_Second_handX.Enabled = b;
            numericUpDown_Second_handY.Enabled = b;
            numericUpDown_Second_handX_centr.Enabled = b;
            numericUpDown_Second_handY_centr.Enabled = b;
            numericUpDown_Second_handX_offset.Enabled = b;
            numericUpDown_Second_handY_offset.Enabled = b;

            label557.Enabled = b;
            label558.Enabled = b;
            label559.Enabled = b;
            label560.Enabled = b;
            label561.Enabled = b;
            label562.Enabled = b;
            label563.Enabled = b;
            label564.Enabled = b;
            label565.Enabled = b;
            label566.Enabled = b;
            label567.Enabled = b;
        }

        private void checkBox_12h_Use_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_12h_Use.Checked;
            comboBox_AM_image.Enabled = b;
            comboBox_PM_image.Enabled = b;
            numericUpDown_AM_X.Enabled = b;
            numericUpDown_AM_Y.Enabled = b;
            numericUpDown_PM_X.Enabled = b;
            numericUpDown_PM_Y.Enabled = b;

            label568.Enabled = b;
            label569.Enabled = b;
            label570.Enabled = b;
            label571.Enabled = b;
            label572.Enabled = b;
            label573.Enabled = b;
            label574.Enabled = b;
            label575.Enabled = b;
        }


        private void checkBox_Day_Use_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Day_Use.Checked;
            comboBox_Day_image.Enabled = b;
            comboBox_Day_unit.Enabled = b;
            comboBox_Day_separator.Enabled = b;
            numericUpDown_DayX.Enabled = b;
            numericUpDown_DayY.Enabled = b;
            numericUpDown_Day_unitX.Enabled = b;
            numericUpDown_Day_unitY.Enabled = b;
            comboBox_Day_alignment.Enabled = b;
            checkBox_Day_add_zero.Enabled = b;
            numericUpDown_Day_spacing.Enabled = b;
            checkBox_Day_follow.Enabled = b;

            label576.Enabled = b; 
            label577.Enabled = b;
            label578.Enabled = b;
            label579.Enabled = b;
            label580.Enabled = b;
            label581.Enabled = b;
            label582.Enabled = b;
            label583.Enabled = b;
            label584.Enabled = b;
            label585.Enabled = b;
            label586.Enabled = b;

            if (checkBox_Day_follow.Checked)
            {
                numericUpDown_DayX.Enabled = false;
                numericUpDown_DayY.Enabled = false;

                label576.Enabled = false;
                label579.Enabled = false;
                label581.Enabled = false;
            }
        }

        private void checkBox_Month_Use_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Month_Use.Checked;
            comboBox_Month_image.Enabled = b;
            comboBox_Month_unit.Enabled = b;
            comboBox_Month_separator.Enabled = b;
            numericUpDown_MonthX.Enabled = b;
            numericUpDown_MonthY.Enabled = b;
            numericUpDown_Month_unitX.Enabled = b;
            numericUpDown_Month_unitY.Enabled = b;
            comboBox_Month_alignment.Enabled = b;
            checkBox_Month_add_zero.Enabled = b;
            numericUpDown_Month_spacing.Enabled = b;
            checkBox_Month_follow.Enabled = b;

            label587.Enabled = b;
            label588.Enabled = b;
            label589.Enabled = b;
            label590.Enabled = b;
            label591.Enabled = b;
            label592.Enabled = b;
            label593.Enabled = b;
            label594.Enabled = b;
            label595.Enabled = b;
            label596.Enabled = b;
            label597.Enabled = b;

            if (checkBox_Month_follow.Checked)
            {
                numericUpDown_MonthX.Enabled = false;
                numericUpDown_MonthY.Enabled = false;

                label587.Enabled = false;
                label590.Enabled = false;
                label592.Enabled = false;
            }
        }

        private void checkBox_Month_pictures_Use_CheckedChanged(object sender, EventArgs e)
        {

            bool b = checkBox_Month_pictures_Use.Checked;
            comboBox_Month_pictures_image.Enabled = b;
            numericUpDown_Month_picturesX.Enabled = b;
            numericUpDown_Month_picturesY.Enabled = b;

            label613.Enabled = b;
            label614.Enabled = b;
            label615.Enabled = b;
            label616.Enabled = b;
        }

        private void checkBox_Year_Use_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Year_Use.Checked;
            comboBox_Year_image.Enabled = b;
            comboBox_Year_unit.Enabled = b;
            comboBox_Year_separator.Enabled = b;
            numericUpDown_YearX.Enabled = b;
            numericUpDown_YearY.Enabled = b;
            numericUpDown_Year_unitX.Enabled = b;
            numericUpDown_Year_unitY.Enabled = b;
            comboBox_Year_alignment.Enabled = b;
            checkBox_Year_add_zero.Enabled = b;
            numericUpDown_Year_spacing.Enabled = b;

            label598.Enabled = b;
            label599.Enabled = b;
            label600.Enabled = b;
            label601.Enabled = b;
            label602.Enabled = b;
            label603.Enabled = b;
            label604.Enabled = b;
            label605.Enabled = b;
            label606.Enabled = b;
            label607.Enabled = b;
            label608.Enabled = b;
        }


    }
}
