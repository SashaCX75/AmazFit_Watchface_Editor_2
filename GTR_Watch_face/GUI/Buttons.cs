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
    }
}
