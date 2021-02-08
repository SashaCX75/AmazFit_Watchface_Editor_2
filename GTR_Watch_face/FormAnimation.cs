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
        ClassStaticAnimation StaticAnimation;
        List<ClassMotiomAnimation> MotiomAnimation;
        //Bitmap PreviewBackground;
        private Bitmap SrcImg;
        float scalePreview = 1.0f;
        float currentDPI; // масштаб экрана

        public FormAnimation(Bitmap previewBackground,List<ClassMotiomAnimation> motiomAnimation,
            ClassStaticAnimation staticAnimation, float cDPI)
        {
            InitializeComponent();
            //PreviewBackground = previewBackground;
            pictureBox_AnimatiomPreview.BackgroundImage = previewBackground;
            //pictureBox_AnimatiomPreview.Image = previewBackground;
            MotiomAnimation = motiomAnimation;
            StaticAnimation = staticAnimation;
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


            //Form1 f1 = this.Owner as Form1;//Получаем ссылку на первую форму

            //f1.PreviewToBitmap(gPanel, scalePreview, false, false, false, false, false, false, false, true);

            //var rand = new Random();
            //Pen p = new Pen(Color.Blue, 5);// цвет линии и ширина
            //Point p1 = new Point(rand.Next(100), rand.Next(100));// первая точка
            //Point p2 = new Point(rand.Next(100), rand.Next(100));// вторая точка
            //gPanel.DrawLine(p, p1, p2);// рисуем линию

            foreach (ClassMotiomAnimation elementMotiomAnimation in MotiomAnimation)
            {
                elementMotiomAnimation.DrawMotiomAnimation(gPanel, timer1.Interval);
            }
            if(StaticAnimation != null) StaticAnimation.DrawStaticAnimation(gPanel, timer1.Interval);

            Form1 form1 = this.Owner as Form1;//Получаем ссылку на первую форму
            form1.PreviewToBitmap(gPanel, 1.0f, false, false, false, false, false, false, false, false, false, 2);

            pictureBox_AnimatiomPreview.Image = SrcImg;

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
                Bitmap mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gtr47.png");
                if (Model_Wath.model_gtr42)
                {
                    bitmap = new Bitmap(Convert.ToInt32(390), Convert.ToInt32(390), PixelFormat.Format32bppArgb);
                    mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gtr42.png");
                }
                if (Model_Wath.model_gts)
                {
                    bitmap = new Bitmap(Convert.ToInt32(348), Convert.ToInt32(442), PixelFormat.Format32bppArgb);
                    mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gts.png");
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
                foreach (ClassMotiomAnimation elementMotiomAnimation in MotiomAnimation)
                {
                    elementMotiomAnimation.ResetDrawMotiomAnimation();
                }
                if (StaticAnimation != null) StaticAnimation.ResetDrawStaticAnimation();

                using (MagickImageCollection collection = new MagickImageCollection())
                {

                    int WeatherSet_Temp = (int)form1.numericUpDown_WeatherSet_Temp.Value;
                    int WeatherSet_DayTemp = (int)form1.numericUpDown_WeatherSet_DayTemp.Value;
                    int WeatherSet_NightTemp = (int)form1.numericUpDown_WeatherSet_NightTemp.Value;
                    int WeatherSet_Icon = form1.comboBox_WeatherSet_Icon.SelectedIndex;

                    for (int i = 0; i < numericUpDown_NumberOfFrames.Value; i++)
                    {
                        save = false;
                        while (!save)
                        {
                            switch (set)
                            {
                                case 0:
                                    //button_Set1.PerformClick();
                                    form1.SetPreferences1();
                                    save = true;
                                    break;
                                case 1:
                                    if (form1.numericUpDown_Calories_Set2.Value != 1234)
                                    {
                                        //button_Set2.PerformClick();
                                        form1.SetPreferences2();
                                        save = true;
                                    }
                                    break;
                                case 2:
                                    if (form1.numericUpDown_Calories_Set3.Value != 1234)
                                    {
                                        //button_Set3.PerformClick();
                                        form1.SetPreferences3();
                                        save = true;
                                    }
                                    break;
                                case 3:
                                    if (form1.numericUpDown_Calories_Set4.Value != 1234)
                                    {
                                        //button_Set4.PerformClick();
                                        form1.SetPreferences4();
                                        save = true;
                                    }
                                    break;
                                case 4:
                                    if (form1.numericUpDown_Calories_Set5.Value != 1234)
                                    {
                                        //button_Set5.PerformClick();
                                        form1.SetPreferences5();
                                        save = true;
                                    }
                                    break;
                                case 5:
                                    if (form1.numericUpDown_Calories_Set6.Value != 1234)
                                    {
                                        //button_Set6.PerformClick();
                                        form1.SetPreferences6();
                                        save = true;
                                    }
                                    break;
                                case 6:
                                    if (form1.numericUpDown_Calories_Set7.Value != 1234)
                                    {
                                        //button_Set7.PerformClick();
                                        form1.SetPreferences7();
                                        save = true;
                                    }
                                    break;
                                case 7:
                                    if (form1.numericUpDown_Calories_Set8.Value != 1234)
                                    {
                                        //button_Set8.PerformClick();
                                        form1.SetPreferences8();
                                        save = true;
                                    }
                                    break;
                                case 8:
                                    if (form1.numericUpDown_Calories_Set9.Value != 1234)
                                    {
                                        //button_Set9.PerformClick();
                                        form1.SetPreferences9();
                                        save = true;
                                    }
                                    break;
                                case 9:
                                    if (form1.numericUpDown_Calories_Set10.Value != 1234)
                                    {
                                        //button_Set10.PerformClick();
                                        form1.SetPreferences10();
                                        save = true;
                                    }
                                    break;
                                case 10:
                                    if (form1.numericUpDown_Calories_Set11.Value != 1234)
                                    {
                                        //button_Set11.PerformClick();
                                        form1.SetPreferences11();
                                        save = true;
                                    }
                                    break;
                                case 11:
                                    if (form1.numericUpDown_Calories_Set12.Value != 1234)
                                    {
                                        //button_Set12.PerformClick();
                                        form1.SetPreferences12();
                                        save = true;
                                    }
                                    break;
                                case 12:
                                    if (form1.numericUpDown_Calories_Set13.Value != 1234)
                                    {
                                        //button_Set13.PerformClick();
                                        form1.SetPreferences13();
                                        save = true;
                                    }
                                    break;

                            }
                            if (!save) set++;
                            if (set > 12) set = 0; 
                        }

                        if (save)
                        {

                            if (oldSet!= set)
                            {
                                form1.numericUpDown_WeatherSet_Temp.Value = rnd.Next(-25, 35) + 1;
                                form1.numericUpDown_WeatherSet_DayTemp.Value = form1.numericUpDown_WeatherSet_Temp.Value;
                                form1.numericUpDown_WeatherSet_NightTemp.Value = form1.numericUpDown_WeatherSet_Temp.Value - rnd.Next(3, 10);
                                form1.comboBox_WeatherSet_Icon.SelectedIndex = rnd.Next(0, 25);
                                oldSet = set;
                            }

                            form1.PreviewToBitmap(gPanel, 1.0f, false, false, false, false, false, false, false, false, false, 1);

                            foreach (ClassMotiomAnimation elementMotiomAnimation in MotiomAnimation)
                            {
                                elementMotiomAnimation.DrawMotiomAnimation(gPanel, 100);
                            }
                            if (StaticAnimation != null) StaticAnimation.DrawStaticAnimation(gPanel, 100);
                            form1.PreviewToBitmap(gPanel, 1.0f, false, false, false, false, false, false, false, false,false, 2);

                            if (form1.checkBox_crop.Checked)
                            {
                                bitmap = form1.ApplyMask(bitmap, mask);
                                gPanel = Graphics.FromImage(bitmap);
                            }
                            // Add first image and set the animation delay to 100ms
                            MagickImage item = new MagickImage(bitmap);
                            //ExifProfile profile = item.GetExifProfile();
                            collection.Add(item);
                            //collection[collection.Count - 1].AnimationDelay = 100;
                            collection[collection.Count - 1].AnimationDelay = 10;

                            
                        }

                        setIndex = setIndex + 100;
                        if (setIndex >= (1000 * form1.numericUpDown_Gif_Speed.Value))
                        {
                            setIndex = 0;
                            set++;
                            if (set > 12) set = 0;
                        }

                        progressBar_SaveAnimation.Value = i;
                        progressBar_SaveAnimation.Update();
                    }

                    form1.numericUpDown_WeatherSet_Temp.Value = WeatherSet_Temp;
                    form1.numericUpDown_WeatherSet_DayTemp.Value = WeatherSet_DayTemp;
                    form1.numericUpDown_WeatherSet_NightTemp.Value = WeatherSet_NightTemp;
                    form1.comboBox_WeatherSet_Icon.SelectedIndex = WeatherSet_Icon;


                    progressBar_SaveAnimation.Visible = false;
                    // Optionally reduce colors
                    QuantizeSettings settings = new QuantizeSettings();
                    //settings.Colors = 256;
                    //collection.Quantize(settings);

                    // Optionally optimize the images (images should have the same size).
                    collection.OptimizeTransparency();
                    //collection.Optimize();

                    // Save gif
                    collection.Write(saveFileDialog.FileName);
                }
                form1.PreviewView = true;
                timer1.Enabled = true;
                mask.Dispose();
            }
        }

        private void button_AnimationReset_Click(object sender, EventArgs e)
        {
            foreach (ClassMotiomAnimation elementMotiomAnimation in MotiomAnimation)
            {
                elementMotiomAnimation.ResetDrawMotiomAnimation();
            }
            if (StaticAnimation != null) StaticAnimation.ResetDrawStaticAnimation();
        }

        private void FormAnimation_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Enabled = false;
            MotiomAnimation.Clear();
            StaticAnimation = null;
            this.Dispose();
        }
    }
}
