using ImageMagick;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmazFit_Watchface_2
{
    public partial class FormAnimation : Form
    {
        //Bitmap PreviewBackground;
        private Bitmap SrcImg;
        float scalePreview = 1.0f;
        float currentDPI; // масштаб экрана

        public FormAnimation(Bitmap previewBackground, float cDPI)
        {
            InitializeComponent();
            //PreviewBackground = previewBackground;
            pictureBox_AnimatiomPreview.BackgroundImage = previewBackground;
            //pictureBox_AnimatiomPreview.Image = previewBackground;
            //currentDPI = (int)Registry.GetValue("HKEY_CURRENT_USER\\Control Panel\\Desktop", "LogPixels", 96) / 96f;

            currentDPI = cDPI;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Graphics gPanel = pictureBox_AnimatiomPreview.CreateGraphics();
            SrcImg = new Bitmap(pictureBox_AnimatiomPreview.Width, pictureBox_AnimatiomPreview.Height);
            Graphics gPanel = Graphics.FromImage(SrcImg);
            //gPanel.Clear(pictureBox_AnimatiomPreview.BackColor);
            //pictureBox_AnimatiomPreview.Image = PreviewBackground;
            //float scalePreview = 1.0f;
            gPanel.ScaleTransform(scalePreview, scalePreview, MatrixOrder.Prepend);


            gPanel.Dispose();// освобождаем все ресурсы, связанные с отрисовкой
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null && !radioButton.Checked) return;
            pictureBox_AnimatiomPreview.BackgroundImageLayout = ImageLayout.Zoom;
            if (radioButton_normal.Checked)
            {
                pictureBox_AnimatiomPreview.BackgroundImageLayout = ImageLayout.None;
                if (Model_Wath.model_gtr47)
                {
                    pictureBox_AnimatiomPreview.Size = new Size(456, 456);
                    this.Size = new Size(456 + (int)(20 * currentDPI), 456 + (int)(100 * currentDPI));
                }
                else if (Model_Wath.model_gtr42)
                {
                    pictureBox_AnimatiomPreview.Size = new Size(392, 392);
                    this.Size = new Size(392 + (int)(20 * currentDPI), 392 + (int)(100 * currentDPI));
                }
                else if (Model_Wath.model_gts)
                {
                    pictureBox_AnimatiomPreview.Size = new Size(350, 444);
                    this.Size = new Size(350 + (int)(20 * currentDPI), 444 + (int)(100 * currentDPI));
                }
                else if (Model_Wath.model_TRex || Model_Wath.model_Verge)
                {
                    pictureBox_AnimatiomPreview.Size = new Size(362, 362);
                    this.Size = new Size(362 + (int)(20 * currentDPI), 362 + (int)(100 * currentDPI));
                }
                else if (Model_Wath.model_AmazfitX)
                {
                    pictureBox_AnimatiomPreview.Size = new Size(208, 642);
                    this.Size = new Size(209 + (int)(20 * currentDPI), 642 + (int)(100 * currentDPI));
                }
                scalePreview = 1f;
            }

            if (radioButton_large.Checked)
            {
                if (Model_Wath.model_gtr47)
                {
                    pictureBox_AnimatiomPreview.Size = new Size(683, 683);
                    this.Size = new Size(683 + (int)(20 * currentDPI), 683 + (int)(100 * currentDPI));
                }
                else if (Model_Wath.model_gtr42)
                {
                    pictureBox_AnimatiomPreview.Size = new Size(587, 587);
                    this.Size = new Size(587 + (int)(20 * currentDPI), 587 + (int)(100 * currentDPI));
                }
                else if (Model_Wath.model_gts)
                {
                    pictureBox_AnimatiomPreview.Size = new Size(524, 665);
                    this.Size = new Size(524 + (int)(20 * currentDPI), 665 + (int)(100 * currentDPI));
                }
                else if (Model_Wath.model_TRex || Model_Wath.model_Verge)
                {
                    pictureBox_AnimatiomPreview.Size = new Size(542, 542);
                    this.Size = new Size(542 + (int)(20 * currentDPI), 542 + (int)(100 * currentDPI));
                }
                else if (Model_Wath.model_AmazfitX)
                {
                    pictureBox_AnimatiomPreview.Size = new Size(311, 963);
                    this.Size = new Size(311 + (int)(20 * currentDPI), 963 + (int)(100 * currentDPI));
                }
                scalePreview = 1.5f;
            }

            if (radioButton_xlarge.Checked)
            {
                if (Model_Wath.model_gtr47)
                {
                    pictureBox_AnimatiomPreview.Size = new Size(909, 909);
                    this.Size = new Size(909 + (int)(20 * currentDPI), 909 + (int)(100 * currentDPI));
                }
                else if (Model_Wath.model_gtr42)
                {
                    pictureBox_AnimatiomPreview.Size = new Size(781, 781);
                    this.Size = new Size(781 + (int)(20 * currentDPI), 781 + (int)(100 * currentDPI));
                }
                else if (Model_Wath.model_gts)
                {
                    pictureBox_AnimatiomPreview.Size = new Size(697, 885);
                    this.Size = new Size(697 + (int)(20 * currentDPI), 885 + (int)(100 * currentDPI));
                }
                else if (Model_Wath.model_TRex || Model_Wath.model_Verge)
                {
                    pictureBox_AnimatiomPreview.Size = new Size(721, 721);
                    this.Size = new Size(721 + (int)(20 * currentDPI), 721 + (int)(100 * currentDPI));
                }
                else if (Model_Wath.model_AmazfitX)
                {
                    pictureBox_AnimatiomPreview.Size = new Size(414, 1282);
                    this.Size = new Size(414 + (int)(20 * currentDPI), 1282 + (int)(100 * currentDPI));
                }
                scalePreview = 2f;
            }
            int width = button_SaveAnimation.Left + button_SaveAnimation.Width;
            if (this.Width < (int)(width + 20 * currentDPI)) this.Width = (int)(width + 20 * currentDPI);
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

        private void button_SaveAnimation_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //openFileDialog.InitialDirectory = subPath;
            saveFileDialog.Filter = "GIF Files: (*.gif)|*.gif";
            saveFileDialog.FileName = "Preview.gif";
            //openFileDialog.Filter = "Binary File (*.bin)|*.bin";
            ////openFileDialog1.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Title = Properties.FormStrings.Dialog_Title_SaveGIF;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap bitmap = new Bitmap(Convert.ToInt32(454), Convert.ToInt32(454), PixelFormat.Format32bppArgb);
                Bitmap mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gtr_2.png");
                if (Model_Wath.model_gtr42)
                {
                    bitmap = new Bitmap(Convert.ToInt32(390), Convert.ToInt32(390), PixelFormat.Format32bppArgb);
                    mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gtr42.png");
                }
                if (Model_Wath.model_gts)
                {
                    bitmap = new Bitmap(Convert.ToInt32(348), Convert.ToInt32(442), PixelFormat.Format32bppArgb);
                    mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gts_2.png");
                }
                if (Model_Wath.model_TRex || Model_Wath.model_Verge)
                {
                    bitmap = new Bitmap(Convert.ToInt32(360), Convert.ToInt32(360), PixelFormat.Format32bppArgb);
                    mask = new Bitmap(Application.StartupPath + @"\Mask\mask_trex.png");
                }
                if (Model_Wath.model_AmazfitX)
                {
                    bitmap = new Bitmap(Convert.ToInt32(206), Convert.ToInt32(640), PixelFormat.Format32bppArgb);
                    mask = new Bitmap(Application.StartupPath + @"\Mask\mask_Amazfitx.png");
                }
                Graphics gPanel = Graphics.FromImage(bitmap);
                bool save = false;
                int set = 0;
                int oldSet = -1;
                int setIndex = 0;
                Random rnd = new Random();
                progressBar_SaveAnimation.Width = pictureBox_AnimatiomPreview.Width - 100;
                progressBar_SaveAnimation.Maximum = (int)numericUpDown_NumberOfFrames.Value;
                progressBar_SaveAnimation.Value = 0;
                progressBar_SaveAnimation.Visible = true;
                Form1 form1 = this.Owner as Form1;//Получаем ссылку на первую форму
                form1.PreviewView = false;
                timer1.Enabled = false;
                mask.Dispose();
            }
        }

        private void button_AnimationReset_Click(object sender, EventArgs e)
        {
            
        }

        private void FormAnimation_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Enabled = false;
            this.Dispose();
        }
    }
}
