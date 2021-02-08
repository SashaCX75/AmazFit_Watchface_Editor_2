using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmazFit_Watchface_2
{
    public partial class Form_Preview : Form
    {
        float scale = 1;
        float currentDPI; // масштаб экрана

        public Form_Preview(float cDPI)
        {
            InitializeComponent();
            //currentDPI = (int)Registry.GetValue("HKEY_CURRENT_USER\\Control Panel\\Desktop", "LogPixels", 96) / 96f;
            currentDPI = cDPI;
        }

        public void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            //Form1 f1 = this.Owner as Form1;//Получаем ссылку на первую форму
            //f1.button1.PerformClick();
            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null && !radioButton.Checked) return;
            pictureBox_Preview.BackgroundImageLayout = ImageLayout.Zoom;
            if (radioButton_small.Checked)
            {
                if (Model_Wath.model_gtr47)
                {
                    pictureBox_Preview.Size = new Size(230, 230);
                    this.Size = new Size(230 + (int)(22 * currentDPI), 230 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_gtr42)
                {
                    pictureBox_Preview.Size = new Size(198, 198);
                    this.Size = new Size(198 + (int)(22 * currentDPI), 198 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_gts)
                {
                    pictureBox_Preview.Size = new Size(177, 224);
                    this.Size = new Size(177 + (int)(22 * currentDPI), 224 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_TRex || Model_Wath.model_Verge)
                {
                    pictureBox_Preview.Size = new Size(183, 183);
                    this.Size = new Size(183 + (int)(22 * currentDPI), 183 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_AmazfitX)
                {
                    pictureBox_Preview.Size = new Size(106, 323);
                    this.Size = new Size(106 + (int)(22 * currentDPI), 323 + (int)(66 * currentDPI));
                }
                scale = 0.5f;
            }

            if (radioButton_normal.Checked)
            {
                pictureBox_Preview.BackgroundImageLayout = ImageLayout.None;
                if (Model_Wath.model_gtr47)
                {
                    pictureBox_Preview.Size = new Size(456, 456);
                    this.Size = new Size(456 + (int)(22 * currentDPI), 456 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_gtr42)
                {
                    pictureBox_Preview.Size = new Size(392, 392);
                    this.Size = new Size(392 + (int)(22 * currentDPI), 392 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_gts)
                {
                    pictureBox_Preview.Size = new Size(350, 444);
                    this.Size = new Size(350 + (int)(22 * currentDPI), 444 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_TRex || Model_Wath.model_Verge)
                {
                    pictureBox_Preview.Size = new Size(362, 362);
                    this.Size = new Size(362 + (int)(22 * currentDPI), 362 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_AmazfitX)
                {
                    pictureBox_Preview.Size = new Size(209, 643);
                    this.Size = new Size(209 + (int)(22 * currentDPI), 643 + (int)(66 * currentDPI));
                }
                scale = 1f;
            }

            if (radioButton_large.Checked)
            {
                if (Model_Wath.model_gtr47)
                {
                    pictureBox_Preview.Size = new Size(683, 683);
                    this.Size = new Size(683 + (int)(22 * currentDPI), 683 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_gtr42)
                {
                    pictureBox_Preview.Size = new Size(587, 587);
                    this.Size = new Size(587 + (int)(22 * currentDPI), 587 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_gts)
                {
                    pictureBox_Preview.Size = new Size(524, 665);
                    this.Size = new Size(524 + (int)(22 * currentDPI), 665 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_TRex || Model_Wath.model_Verge)
                {
                    pictureBox_Preview.Size = new Size(542, 542);
                    this.Size = new Size(542 + (int)(22 * currentDPI), 542 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_AmazfitX)
                {
                    pictureBox_Preview.Size = new Size(312, 963);
                    this.Size = new Size(312 + (int)(22 * currentDPI), 963 + (int)(66 * currentDPI));
                }
                scale = 1.5f;
            }

            if (radioButton_xlarge.Checked)
            {
                if (Model_Wath.model_gtr47)
                {
                    pictureBox_Preview.Size = new Size(909, 909);
                    this.Size = new Size(909 + (int)(22 * currentDPI), 909 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_gtr42)
                {
                    pictureBox_Preview.Size = new Size(781, 781);
                    this.Size = new Size(781 + (int)(22 * currentDPI), 781 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_gts)
                {
                    pictureBox_Preview.Size = new Size(697, 885);
                    this.Size = new Size(697 + (int)(22 * currentDPI), 885 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_TRex || Model_Wath.model_Verge)
                {
                    pictureBox_Preview.Size = new Size(721, 721);
                    this.Size = new Size(721 + (int)(22 * currentDPI), 721 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_AmazfitX)
                {
                    pictureBox_Preview.Size = new Size(414, 1282);
                    this.Size = new Size(414 + (int)(22 * currentDPI), 1282 + (int)(66 * currentDPI));
                }
                scale = 2f;
            }

            if (radioButton_xxlarge.Checked)
            {
                if (Model_Wath.model_gtr47)
                {
                    pictureBox_Preview.Size = new Size(1136, 1136);
                    this.Size = new Size(1136 + (int)(22 * currentDPI), 1136 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_gtr42)
                {
                    pictureBox_Preview.Size = new Size(976, 976);
                    this.Size = new Size(976 + (int)(22 * currentDPI), 976 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_gts)
                {
                    pictureBox_Preview.Size = new Size(871, 1106);
                    this.Size = new Size(871 + (int)(22 * currentDPI), 1106 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_TRex || Model_Wath.model_Verge)
                {
                    pictureBox_Preview.Size = new Size(901, 901);
                    this.Size = new Size(901 + (int)(22 * currentDPI), 901 + (int)(66 * currentDPI));
                }
                else if (Model_Wath.model_AmazfitX)
                {
                    pictureBox_Preview.Size = new Size(517, 1602);
                    this.Size = new Size(517 + (int)(22 * currentDPI), 1602 + (int)(66 * currentDPI));
                }
                scale = 2.5f;
            }
        }

        public class Model_Wath
        {
            public static bool model_gtr47 { get; set; }
            public static bool model_gtr42 { get; set; }
            public static bool model_gts { get; set; }
            public static bool model_TRex { get; set; }
            public static bool model_AmazfitX { get; set; }
            public static bool model_Verge { get; set; }

        }

        private void pictureBox_Preview_MouseMove(object sender, MouseEventArgs e)
        {
            int CursorX = (int)Math.Round(e.X / scale, 0);
            int CursorY = (int)Math.Round(e.Y / scale, 0);

            this.Text = Properties.FormStrings.Form_PreviewX + CursorX.ToString() +
                ";  Y=" + CursorY.ToString() + "]";
        }

        private void pictureBox_Preview_MouseLeave(object sender, EventArgs e)
        {
            this.Text = Properties.FormStrings.Form_Preview;
        }
        private void pictureBox_Preview_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MouseСoordinates.X = (int)Math.Round(e.X / scale, 0);
            MouseСoordinates.Y = (int)Math.Round(e.Y / scale, 0);
            toolTip1.Show(Properties.FormStrings.Message_CopyCoord, this, e.X-5, e.Y-7, 2000);
        }

        private void Form_Preview_Load(object sender, EventArgs e)
        {
            this.Text = Properties.FormStrings.Form_Preview;
        }
    }
    
}
