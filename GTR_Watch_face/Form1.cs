using ImageMagick;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using System.Text.RegularExpressions;

namespace AmazFit_Watchface_2
{
    public partial class Form1 : Form
    {
        WATCH_FACE_JSON Watch_Face;
        WATCH_FACE_PREWIEV_TwoDigits Watch_Face_Preview_TwoDigits;
        WATCH_FACE_PREWIEV_SET Watch_Face_Preview_Set;
        List<string> ListImages = new List<string>(); // перечень имен файлов с картинками
        List<string> ListImagesFullName = new List<string>(); // перечень путей к файлам с картинками
        public bool PreviewView; // включает прорисовку предпросмотра
        bool Settings_Load; // включать при обновлении настроек для выключения перерисовки
        bool MotiomAnimation_Update = false; // включать при обновлении параметров анимации
        bool JSON_Modified = false; // JSON файл был изменен
        string FileName; // Запоминает имя для диалогов
        string FullFileDir; // Запоминает папку для диалогов
        string StartFileNameJson; // имя файла из параметров запуска
        string StartFileNameBin; // имя файла из параметров запуска
        float currentDPI; // масштаб экрана
        Form_Preview formPreview;
        PROGRAM_SETTINGS Program_Settings;

        Widgets WidgetsTemp; // временная переменная для хранения виджетов

        int offSet_X = 227;
        int offSet_Y = 227;



        public Form1(string[] args)
        {
            if (File.Exists(Application.StartupPath + "\\Program log.txt")) File.Delete(Application.StartupPath + @"\Program log.txt");
            Logger.WriteLine("* Form1");


            Program_Settings = new PROGRAM_SETTINGS();
            try
            {
                if (File.Exists(Application.StartupPath + @"\Settings.json"))
                {
                    Logger.WriteLine("Read Settings");
                    Program_Settings = JsonConvert.DeserializeObject<PROGRAM_SETTINGS>
                                (File.ReadAllText(Application.StartupPath + @"\Settings.json"), new JsonSerializerSettings
                                {
                        //DefaultValueHandling = DefaultValueHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                                });
                    //Logger.WriteLine("Чтение Settings.json");
                }


                if ((Program_Settings.language == null) || (Program_Settings.language.Length < 2))
                {
                    string language = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
                    //int language = System.Globalization.CultureInfo.CurrentCulture.LCID;
                    //Program_Settings.language = "Русский";
                    Program_Settings.language = "English";
                    Logger.WriteLine("language = " + language);
                    if (language == "ru")
                    {
                        Program_Settings.language = "Русский";
                    }
                    if (language == "en")
                    {
                        Program_Settings.language = "English";
                    }
                    if (language == "es")
                    {
                        Program_Settings.language = "Español";
                    }
                }
                //Logger.WriteLine("Определили язык");
                SetLanguage();
            }
            catch (Exception)
            {
                //Logger.WriteLine("Ошибка чтения настроек " + ex);
            }
            
            InitializeComponent();

            Watch_Face_Preview_Set = new WATCH_FACE_PREWIEV_SET();
            Watch_Face_Preview_Set.Activity = new ActivitySet();
            Watch_Face_Preview_Set.Date = new DateSet();
            Watch_Face_Preview_Set.Status = new StatusSet();
            Watch_Face_Preview_Set.Time = new TimeSet();
            Watch_Face_Preview_Set.Weather = new WeatherSet();

            Watch_Face_Preview_TwoDigits = new WATCH_FACE_PREWIEV_TwoDigits();
            Watch_Face_Preview_TwoDigits.Date = new DateP();
            Watch_Face_Preview_TwoDigits.Date.Day = new TwoDigitsP();
            Watch_Face_Preview_TwoDigits.Date.Month = new TwoDigitsP();

            Watch_Face_Preview_TwoDigits.Year = new YearP();

            Watch_Face_Preview_TwoDigits.Time = new TimeP();
            Watch_Face_Preview_TwoDigits.Time.Hours = new TwoDigitsP();
            Watch_Face_Preview_TwoDigits.Time.Minutes = new TwoDigitsP();
            Watch_Face_Preview_TwoDigits.Time.Seconds = new TwoDigitsP();

            Watch_Face_Preview_TwoDigits.TimePm = new TimePmP();
            Watch_Face_Preview_TwoDigits.TimePm.Hours = new TwoDigitsP();
            Watch_Face_Preview_TwoDigits.TimePm.Minutes = new TwoDigitsP();
            Watch_Face_Preview_TwoDigits.TimePm.Seconds = new TwoDigitsP();
            

            PreviewView = true;
            Settings_Load = false;
            //currentDPI = (int)Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "LogPixels", 96) / 96f;
            //if (getOSversion() >= 10)
            //{
            //    float AppliedDPI = (int)Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "AppliedDPI", 96) / 96f;
            //    MessageBox.Show(AppliedDPI.ToString());
            //    currentDPI = AppliedDPI / currentDPI;
            //    MessageBox.Show(currentDPI.ToString());
            //}
            currentDPI = tabControl1.Height / 670f;
            tabControl1.TabPages[0].Parent = null;
            tabControl1.TabPages[2].Parent = null;
            tabControl1.TabPages[4].Parent = null;
#if DEBUG
            tabControl1.SelectTab(1);
            tabControl_EditParameters.SelectTab(3);
#endif

#if !DEBUG
            tabControl_SystemActivity.TabPages[5].Parent = null;
            tabControl_SystemActivity.TabPages[5].Parent = null;
            tabControl_SystemActivity.TabPages[5].Parent = null;
            tabControl_SystemActivity.TabPages[5].Parent = null;

            tabControl_SystemWeather.TabPages[1].Parent = null;
            tabControl_SystemWeather.TabPages[1].Parent = null;
            tabControl_SystemWeather.TabPages[1].Parent = null;
            tabControl_SystemWeather.TabPages[1].Parent = null;
            tabControl_SystemWeather.TabPages[1].Parent = null;
            tabControl_SystemWeather.TabPages[1].Parent = null;
            tabControl_SystemWeather.TabPages[1].Parent = null;


            tabControl_SystemActivity_AOD.TabPages[5].Parent = null;
            tabControl_SystemActivity_AOD.TabPages[5].Parent = null;
            tabControl_SystemActivity_AOD.TabPages[5].Parent = null;
            tabControl_SystemActivity_AOD.TabPages[5].Parent = null;

            tabControl_SystemWeather_AOD.TabPages[1].Parent = null;
            tabControl_SystemWeather_AOD.TabPages[1].Parent = null;
            tabControl_SystemWeather_AOD.TabPages[1].Parent = null;
            tabControl_SystemWeather_AOD.TabPages[1].Parent = null;
            tabControl_SystemWeather_AOD.TabPages[1].Parent = null;
            tabControl_SystemWeather_AOD.TabPages[1].Parent = null;
            tabControl_SystemWeather_AOD.TabPages[1].Parent = null;
#endif

            splitContainer_EditParameters.Panel1Collapsed = false;
            splitContainer_EditParameters.Panel2Collapsed = true;

            //Logger.WriteLine("Создали переменные");

            if (args.Length == 1)
            {
                string fileName = args[0].ToString();
                if ((File.Exists(fileName)) && (Path.GetExtension(fileName) == ".json"))
                {
                    Logger.WriteLine("args[0] - *.json");
                    StartFileNameJson = fileName;
                    //Logger.WriteLine("Программа запущена с аргументом: " + fileName);
                }
                if ((File.Exists(fileName)) && (Path.GetExtension(fileName) == ".bin"))
                {
                    Logger.WriteLine("args[0] - *.bin");
                    StartFileNameBin = fileName;
                    //Logger.WriteLine("Программа запущена с аргументом: " + fileName);
                }
            }
            Logger.WriteLine("* Form1 (end)");
        }
        
        private void SetLanguage()
        {
            Logger.WriteLine("* SetLanguage");
            if (Program_Settings.language == "English")
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
            }
            else if (Program_Settings.language == "Español")
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("es");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("es");
            }
            else if (Program_Settings.language == "Português")
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("pt");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("pt");
            }
            else if (Program_Settings.language == "Čeština")
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("cs");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("cs");
            }
            else if (Program_Settings.language == "Magyar")
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("hu");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("hu");
            }
            else if (Program_Settings.language == "Slovenčina")
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("sk");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("sk");
            }
            else if (Program_Settings.language == "French")
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("fr");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("fr");
            }
            else if (Program_Settings.language == "Chinese/简体中文")
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("zh");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("zh");
            }
            else if (Program_Settings.language == "Italian" || Program_Settings.language == "Italiano")
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("it");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("it");
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ru");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("ru");
            }
            Logger.WriteLine("* SetLanguage (end)");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Logger.WriteLine("* Form1_Load ");

            //Logger.WriteLine("Form1_Load");
            helpProvider1.HelpNamespace = Application.StartupPath + Properties.FormStrings.File_ReadMy;
            
            string subPath = Application.StartupPath + @"\Tools\main.exe";
            Logger.WriteLine("Set textBox.Text");
            if (Program_Settings.pack_unpack_dir == null)
            {
                Program_Settings.pack_unpack_dir = subPath;
                string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
                {
                    //DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
                File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);
            }
            PreviewView = false;
            textBox_pack_unpack_dir.Text = Program_Settings.pack_unpack_dir;
            Logger.WriteLine("Set Model");
            if (Program_Settings.Model_GTS)
            {
                radioButton_GTS2.Checked = Program_Settings.Model_GTS;
                textBox_unpack_command.Text = Program_Settings.unpack_command_GTS_2;
            }
            else
            {
                radioButton_GTR2.Checked = true;
                textBox_unpack_command.Text = Program_Settings.unpack_command_GTR_2;
            }
            
            Logger.WriteLine("Set checkBox");
            checkBox_border.Checked = Program_Settings.ShowBorder;
            checkBox_crop.Checked = Program_Settings.Crop;
            checkBox_Show_Shortcuts.Checked = Program_Settings.Show_Shortcuts;
            checkBox_CircleScaleImage.Checked = Program_Settings.Show_CircleScale_Area;
            checkBox_center_marker.Checked = Program_Settings.Shortcuts_Center_marker;

            comboBox_Hour_alignment.SelectedIndex = 0;
            comboBox_Minute_alignment.SelectedIndex = 0;
            comboBox_Second_alignment.SelectedIndex = 0;

            comboBox_Hour_alignment_AOD.SelectedIndex = 0;
            comboBox_Minute_alignment_AOD.SelectedIndex = 0;

            comboBox_Day_alignment.SelectedIndex = 0;
            comboBox_Month_alignment.SelectedIndex = 0;
            comboBox_Year_alignment.SelectedIndex = 0;

            comboBox_Day_alignment_AOD.SelectedIndex = 0;
            comboBox_Month_alignment_AOD.SelectedIndex = 0;
            comboBox_Year_alignment_AOD.SelectedIndex = 0;

            comboBox_Battery_alignment.SelectedIndex = 0;
            comboBox_Battery_scaleCircle_flatness.SelectedIndex = 0;
            comboBox_Battery_scaleLinear_flatness.SelectedIndex = 0;

            comboBox_Battery_alignment_AOD.SelectedIndex = 0;
            comboBox_Battery_scaleCircle_flatness_AOD.SelectedIndex = 0;
            comboBox_Battery_scaleLinear_flatness_AOD.SelectedIndex = 0;

            comboBox_Steps_alignment.SelectedIndex = 0;
            comboBox_Steps_scaleCircle_flatness.SelectedIndex = 0;
            comboBox_Steps_scaleLinear_flatness.SelectedIndex = 0;

            comboBox_Steps_alignment_AOD.SelectedIndex = 0;
            comboBox_Steps_scaleCircle_flatness_AOD.SelectedIndex = 0;
            comboBox_Steps_scaleLinear_flatness_AOD.SelectedIndex = 0;

            comboBox_Calories_alignment.SelectedIndex = 0;
            comboBox_Calories_scaleCircle_flatness.SelectedIndex = 0;
            comboBox_Calories_scaleLinear_flatness.SelectedIndex = 0;

            comboBox_HeartRate_alignment.SelectedIndex = 0;
            comboBox_HeartRate_scaleCircle_flatness.SelectedIndex = 0;
            comboBox_HeartRate_scaleLinear_flatness.SelectedIndex = 0;

            comboBox_PAI_alignment.SelectedIndex = 0;
            comboBox_PAI_scaleCircle_flatness.SelectedIndex = 0;
            comboBox_PAI_scaleLinear_flatness.SelectedIndex = 0;

            comboBox_Distance_alignment.SelectedIndex = 0;
            comboBox_Distance_scaleCircle_flatness.SelectedIndex = 0;
            comboBox_Distance_scaleLinear_flatness.SelectedIndex = 0;

            comboBox_Weather_alignment.SelectedIndex = 0;
            comboBox_Weather_alignmentMin.SelectedIndex = 0;
            comboBox_Weather_alignmentMax.SelectedIndex = 0;
            comboBox_Weather_scaleCircle_flatness.SelectedIndex = 0;
            comboBox_Weather_scaleLinear_flatness.SelectedIndex = 0;

            comboBox_Weather_alignment_AOD.SelectedIndex = 0;
            comboBox_Weather_alignmentMin_AOD.SelectedIndex = 0;
            comboBox_Weather_alignmentMax_AOD.SelectedIndex = 0;
            comboBox_Weather_scaleCircle_flatness_AOD.SelectedIndex = 0;
            comboBox_Weather_scaleLinear_flatness_AOD.SelectedIndex = 0;



            label_version.Text = "v " +
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major.ToString() + "." +
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString();
            label_version_help.Text =
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major.ToString() + "." +
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString();

            Logger.WriteLine("Set Settings");
            Settings_Load = true;
            radioButton_Settings_AfterUnpack_Dialog.Checked = Program_Settings.Settings_AfterUnpack_Dialog;
            radioButton_Settings_AfterUnpack_DoNothing.Checked = Program_Settings.Settings_AfterUnpack_DoNothing;
            radioButton_Settings_AfterUnpack_Download.Checked = Program_Settings.Settings_AfterUnpack_Download;

            radioButton_Settings_Open_Dialog.Checked = Program_Settings.Settings_Open_Dialog;
            radioButton_Settings_Open_DoNotning.Checked = Program_Settings.Settings_Open_DoNotning;
            radioButton_Settings_Open_Download.Checked = Program_Settings.Settings_Open_Download;

            radioButton_Settings_Pack_Dialog.Checked = Program_Settings.Settings_Pack_Dialog;
            radioButton_Settings_Pack_DoNotning.Checked = Program_Settings.Settings_Pack_DoNotning;
            radioButton_Settings_Pack_GoToFile.Checked = Program_Settings.Settings_Pack_GoToFile;

            radioButton_Settings_Unpack_Dialog.Checked = Program_Settings.Settings_Unpack_Dialog;
            radioButton_Settings_Unpack_Replace.Checked = Program_Settings.Settings_Unpack_Replace;
            radioButton_Settings_Unpack_Save.Checked = Program_Settings.Settings_Unpack_Save;
            numericUpDown_Gif_Speed.Value = (decimal)Program_Settings.Gif_Speed;
            comboBox_Animation_Preview_Speed.SelectedIndex = Program_Settings.Animation_Preview_Speed;

            checkBox_Shortcuts_Area.Checked = Program_Settings.Shortcuts_Area;
            checkBox_Shortcuts_Border.Checked = Program_Settings.Shortcuts_Border;

            checkBox_ShowIn12hourFormat.Checked = Program_Settings.ShowIn12hourFormat;
            checkBox_SaveID.Checked = Program_Settings.SaveID;

            if (Program_Settings.language.Length>1) comboBox_Language.Text = Program_Settings.language;

            if (Program_Settings.Splitter_Pos > 0 ) 
                splitContainer1.SplitterDistance = Program_Settings.Splitter_Pos;

            Settings_Load = false;

            if (Program_Settings.SaveID) checkBox_UseID.Checked = true;

            SetPreferences1();
            PreviewView = true;
            Logger.WriteLine("* Form1_Load (end)");
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Logger.WriteLine("* Form1_Shown");
            //Logger.WriteLine("Загружаем файл из значения аргумента " + StartFileNameJson);
            if ((StartFileNameJson != null) && (StartFileNameJson.Length > 0))
            {
                Logger.WriteLine("Загружаем Json файл из значения аргумента " + StartFileNameJson);
                LoadJsonAndImage(StartFileNameJson);
                StartFileNameJson = "";
            }
            else if ((StartFileNameBin != null) && (StartFileNameBin.Length > 0))
            {
                Logger.WriteLine("Загружаем bin файл из значения аргумента " + StartFileNameBin);
                zip_unpack_bin(StartFileNameBin);
                StartFileNameBin = "";
            }
            JSON_Modified = false;
            FormText();
            //Logger.WriteLine("Загрузили файл из значения аргумента " + StartFileNameJson);

            // изменяем размер панели для предпросмотра если она не влазит
            if (pictureBox_Preview.Top + pictureBox_Preview.Height > radioButton_GTR2.Top)
            {
                float newHeight = radioButton_GTR2.Top - pictureBox_Preview.Top;
                float scale = newHeight / pictureBox_Preview.Height;
                pictureBox_Preview.Size = new Size((int)(pictureBox_Preview.Width * scale), (int)(pictureBox_Preview.Height * scale));
            }

            //if (pictureBox_Preview.Top + pictureBox_Preview.Height > 100)
            //{
            //    float newHeight = 100 - pictureBox_Preview.Top;
            //    float scale = newHeight / pictureBox_Preview.Height;
            //    pictureBox_Preview.Size = new Size((int)(pictureBox_Preview.Width * scale), (int)(pictureBox_Preview.Height * scale));
            //}
            button_CreatePreview.Location= new Point(5, 563);
            Logger.WriteLine("* Form1_Shown(end)");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Logger.WriteLine("* FormClosing");
#if !DEBUG
            if (JSON_Modified)
            {
                if (FileName != null)
                {
                    DialogResult dr = MessageBox.Show(Properties.FormStrings.Message_Save_JSON_Modified_Text1 +
                        Path.GetFileNameWithoutExtension(FileName) + Properties.FormStrings.Message_Save_JSON_Modified_Text2, 
                        Properties.FormStrings.Message_Save_JSON_Modified_Caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    if (dr == DialogResult.Yes)
                    {
                        string fullfilename = Path.Combine(FullFileDir, FileName);
                        save_JSON_File(fullfilename, richTextBox_JsonText.Text);
                        if (checkBox_JsonWarnings.Checked) jsonWarnings(fullfilename);
                    }
                    if (dr == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                    } 
                }
                else
                {
                    DialogResult dr = MessageBox.Show(Properties.FormStrings.Message_Save_new_JSON, 
                        Properties.FormStrings.Message_Save_JSON_Modified_Caption,
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    if (dr == DialogResult.Yes)
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.InitialDirectory = FullFileDir;
                        saveFileDialog.FileName = FileName;
                        if(FileName==null || FileName.Length == 0)
                        {
                            if (FullFileDir != null && FullFileDir.Length > 3)
                            {
                                saveFileDialog.FileName = Path.GetFileName(FullFileDir);
                            }
                        }
                        saveFileDialog.Filter = Properties.FormStrings.FilterJson;

                        //openFileDialog.Filter = "Json files (*.json) | *.json";
                        saveFileDialog.RestoreDirectory = true;
                        saveFileDialog.Title = Properties.FormStrings.Dialog_Title_Pack;
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            string fullfilename = saveFileDialog.FileName;
                            save_JSON_File(fullfilename, richTextBox_JsonText.Text);

                            FileName = Path.GetFileName(fullfilename);
                            FullFileDir = Path.GetDirectoryName(fullfilename);
                            JSON_Modified = false;
                            if (checkBox_JsonWarnings.Checked) jsonWarnings(fullfilename);
                        }
                        else e.Cancel = true;
                    }
                    if (dr == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                }
            }
#endif
        }

        private void button_pack_unpack_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = textBox_pack_unpack_dir.Text;
            openFileDialog.Filter = "";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = false;
            openFileDialog.Title = Properties.FormStrings.Dialog_Title_PackUnpack;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Logger.WriteLine("pack_unpack_Click");
                textBox_pack_unpack_dir.Text = openFileDialog.FileName;

                Program_Settings.pack_unpack_dir = openFileDialog.FileName;
                string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
                {
                    //DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
                File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);
                Logger.WriteLine("* pack_unpack_Click (end)");
            }
        }

        private void Form1_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            Logger.WriteLine("* HelpButton");
            //FormFileExists formAnimation = new FormFileExists();
            //formAnimation.ShowDialog();
            //SendKeys.Send("{F1}");
            Help.ShowHelp(this, Application.StartupPath + Properties.FormStrings.File_ReadMy);
            e.Cancel = true;
            Logger.WriteLine("* HelpButton (end)");
        }
        
        private void button_zip_unpack_Click(object sender, EventArgs e)
        {
            Logger.WriteLine("* _packed_unpack_Click");
            if (JSON_Modified) // сохранение если файл не сохранен
            {
                if (FileName != null)
                {
                    DialogResult dr = MessageBox.Show(Properties.FormStrings.Message_Save_JSON_Modified_Text1 +
                        Path.GetFileNameWithoutExtension(FileName) + Properties.FormStrings.Message_Save_JSON_Modified_Text2,
                        Properties.FormStrings.Message_Save_JSON_Modified_Caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    if (dr == DialogResult.Yes)
                    {
                        string fullfilename = Path.Combine(FullFileDir, FileName);
                        save_JSON_File(fullfilename, richTextBox_JsonText.Text);
                        JSON_Modified = false;
                        FormText();
                        if (checkBox_JsonWarnings.Checked) jsonWarnings(fullfilename);
                    }
                    if (dr == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                else
                {
                    DialogResult dr = MessageBox.Show(Properties.FormStrings.Message_Save_new_JSON,
                        Properties.FormStrings.Message_Save_JSON_Modified_Caption,
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    if (dr == DialogResult.Yes)
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.InitialDirectory = FullFileDir;
                        saveFileDialog.FileName = FileName;
                        if (FileName == null || FileName.Length == 0)
                        {
                            if (FullFileDir != null && FullFileDir.Length > 3)
                            {
                                saveFileDialog.FileName = Path.GetFileName(FullFileDir);
                            }
                        }
                        saveFileDialog.Filter = Properties.FormStrings.FilterJson;
                        //saveFileDialog.Filter = "Json files (*.json) | *.json";
                        
                        saveFileDialog.RestoreDirectory = true;
                        saveFileDialog.Title = Properties.FormStrings.Dialog_Title_Pack;
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            string fullfilename = saveFileDialog.FileName;
                            save_JSON_File(fullfilename, richTextBox_JsonText.Text);

                            FileName = Path.GetFileName(fullfilename);
                            FullFileDir = Path.GetDirectoryName(fullfilename);
                            JSON_Modified = false;
                            FormText();
                            if (checkBox_JsonWarnings.Checked) jsonWarnings(fullfilename);
                        }
                        else return;
                    }
                    if (dr == DialogResult.Cancel)
                    {
                        return;
                    }
                }
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Binary File (*.bin)|*.bin";
            openFileDialog.Filter = Properties.FormStrings.FilterBin;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = false;
            openFileDialog.Title = Properties.FormStrings.Dialog_Title_Unpack;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fullfilename = openFileDialog.FileName;
                zip_unpack_bin(fullfilename);
            }
            Logger.WriteLine("* _packed_unpack_Click (end)");
        }

        private void zip_unpack_bin(string fullfilename)
        {
            Logger.WriteLine("* _packed_unpack_bin");
            string subPath = Application.StartupPath + @"\Watch_face\";
            if (!Directory.Exists(subPath)) Directory.CreateDirectory(subPath);
            string respackerPath = Application.StartupPath + @"\Tools\GTR2_Packer.exe";

            if (!File.Exists(respackerPath))
            {
                MessageBox.Show(Properties.FormStrings.Message_error_respackerPath_Text1 + Environment.NewLine +
                    Properties.FormStrings.Message_error_respackerPath_Text2 + respackerPath + "].\r\n\r\n" +
                    Properties.FormStrings.Message_error_respackerPath_Text3,
                    Properties.FormStrings.Message_error_pack_unpack_dir_Caption, 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            string filename = Path.GetFileName(fullfilename);
            filename = filename.Replace(" ", "_");
            string fullPath = subPath + filename;
            // если файл существует
            if (File.Exists(fullPath))
            {
                string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
                string extension = Path.GetExtension(fullPath);
                string path = Path.GetDirectoryName(fullPath);
                string newFullPath = fullPath;
                if (Program_Settings.Settings_Unpack_Dialog)
                {
                    Logger.WriteLine("File.Exists");
                    FormFileExists f = new FormFileExists();
                    f.ShowDialog();
                    int dialogResult = f.Data;
                    
                    switch (dialogResult)
                    {
                        case 0:
                            return;
                        //break;
                        case 1:
                            Logger.WriteLine("File.Copy");
                            File.Copy(fullfilename, fullPath, true);
                            newFullPath = Path.Combine(path, fileNameOnly);
                            if (Directory.Exists(newFullPath)) Directory.Delete(newFullPath, true);
                            break;
                        case 2:
                            Logger.WriteLine("newFileName.Copy");
                            int count = 1;

                            while (File.Exists(newFullPath))
                            {
                                string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                                newFullPath = Path.Combine(path, tempFileName + extension);
                            }
                            File.Copy(fullfilename, newFullPath);
                            fullPath = newFullPath;
                            fileNameOnly = Path.GetFileNameWithoutExtension(newFullPath);
                            path = Path.GetDirectoryName(newFullPath);
                            newFullPath = Path.Combine(path, fileNameOnly);
                            if (Directory.Exists(newFullPath)) Directory.Delete(newFullPath, true);
                            break;
                    }
                }
                else if (Program_Settings.Settings_Unpack_Save)
                {
                    Logger.WriteLine("newFileName.Copy");
                    int count = 1;

                    while (File.Exists(newFullPath))
                    {
                        string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                        newFullPath = Path.Combine(path, tempFileName + extension);
                    }
                    File.Copy(fullfilename, newFullPath);
                    fullPath = newFullPath;
                    fileNameOnly = Path.GetFileNameWithoutExtension(newFullPath);
                    path = Path.GetDirectoryName(newFullPath);
                    newFullPath = Path.Combine(path, fileNameOnly);
                    if (Directory.Exists(newFullPath)) Directory.Delete(newFullPath, true);
                }
                else if (Program_Settings.Settings_Unpack_Replace)
                {
                    Logger.WriteLine("File.Copy");
                    File.Copy(fullfilename, fullPath, true);
                    newFullPath = Path.Combine(path, fileNameOnly);
                    if (Directory.Exists(newFullPath)) Directory.Delete(newFullPath, true);
                }
            }
            else File.Copy(fullfilename, fullPath);

            try
            {
                Logger.WriteLine("UnzipBin");
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = respackerPath;
                startInfo.Arguments = "-unc2" + " \"" + fullPath + "\"";
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();//ждем 
                };
                // этот блок закончится только после окончания работы программы 
                //сюда писать команды после успешного завершения программы

                string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
                string path = Path.GetDirectoryName(fullPath);
                string newFullName_unp = Path.Combine(path, fileNameOnly + ".bin.unc");
                string newFullName_bin = Path.Combine(path, fileNameOnly + ".bin");

                if (File.Exists(newFullName_unp))
                {
                    File.Copy(newFullName_unp, newFullName_bin, true);
                    this.BringToFront();
                    //после декомпресии bin файла
                    if (File.Exists(newFullName_bin))
                    {
                        File.Delete(newFullName_unp);

                        if (!File.Exists(textBox_pack_unpack_dir.Text))
                        {
                            MessageBox.Show(Properties.FormStrings.Message_error_pack_unpack_dir_Text1 +
                                textBox_pack_unpack_dir.Text + Properties.FormStrings.Message_error_pack_unpack_dir_Text2 +
                                Environment.NewLine + Environment.NewLine + Properties.FormStrings.Message_error_pack_unpack_dir_Text3,
                                Properties.FormStrings.Message_error_pack_unpack_dir_Caption,
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }

                        Logger.WriteLine("UnpackBin");
                        startInfo.FileName = textBox_pack_unpack_dir.Text;
                        startInfo.Arguments = textBox_unpack_command.Text + " \"" + newFullName_bin + "\"";
                        using (Process exeProcess = Process.Start(startInfo))
                        {
                            exeProcess.WaitForExit();//ждем 
                        };
                        // этот блок закончится только после окончания работы программы 
                        //сюда писать команды после успешного завершения программы
                        fileNameOnly = Path.GetFileNameWithoutExtension(newFullName_bin);
                        path = Path.GetDirectoryName(newFullName_bin);
                        path = Path.Combine(path, fileNameOnly);
                        string newFullName = Path.Combine(path, fileNameOnly + ".json");

                        if (Program_Settings.SaveID) SaveID(newFullName_bin);

                        if (Program_Settings.Settings_AfterUnpack_Dialog)
                        {
                            if (File.Exists(newFullName))
                            {
                                this.BringToFront();
                                if (MessageBox.Show(Properties.FormStrings.Message_openProject_Text, Properties.FormStrings.
                                    Message_openProject_Caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    LoadJsonAndImage(newFullName);
                                }
                            }
                        }
                        else if (Program_Settings.Settings_AfterUnpack_Download)
                        {
                            if (File.Exists(newFullName)) LoadJsonAndImage(newFullName);
                        }
                    }
                }
            }
            catch
            {
                // сюда писать команды при ошибке вызова 
            }
            Logger.WriteLine("* _packed_unpack_bin (end)");
        }

        private void SaveID(string FileName)
        {
            string fileNameOnly = Path.GetFileNameWithoutExtension(FileName);
            string path = Path.GetDirectoryName(FileName);
            path = Path.Combine(path, fileNameOnly);
            string JSONFileName = Path.Combine(path, "Watchface.ID");

            using (FileStream fileStream = File.OpenRead(FileName))
            {
                BinaryReader _reader = new BinaryReader(fileStream);
                _reader.ReadBytes(18);
                //fileStream.Position = 18;
                int ID = _reader.ReadInt32();

                WatchfaceID watchfaceID = new WatchfaceID();
                watchfaceID.ID = ID;
                watchfaceID.UseID = true;

                string JSON_String = JsonConvert.SerializeObject(watchfaceID, Formatting.Indented, new JsonSerializerSettings
                {
                    //DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
                File.WriteAllText(JSONFileName, JSON_String, Encoding.UTF8);
            }
        }

        private void button_unpack_Click(object sender, EventArgs e)
        {
            Logger.WriteLine("* _unpack_Click");
            if (JSON_Modified) // сохранение если файл не сохранен
            {
                if (FileName != null)
                {
                    DialogResult dr = MessageBox.Show(Properties.FormStrings.Message_Save_JSON_Modified_Text1 +
                        Path.GetFileNameWithoutExtension(FileName) + Properties.FormStrings.Message_Save_JSON_Modified_Text2,
                        Properties.FormStrings.Message_Save_JSON_Modified_Caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    if (dr == DialogResult.Yes)
                    {
                        string fullfilename = Path.Combine(FullFileDir, FileName);
                        save_JSON_File(fullfilename, richTextBox_JsonText.Text);
                        JSON_Modified = false;
                        FormText();
                        if (checkBox_JsonWarnings.Checked) jsonWarnings(fullfilename);
                    }
                    if (dr == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                else
                {
                    DialogResult dr = MessageBox.Show(Properties.FormStrings.Message_Save_new_JSON,
                        Properties.FormStrings.Message_Save_JSON_Modified_Caption,
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    if (dr == DialogResult.Yes)
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.InitialDirectory = FullFileDir;
                        saveFileDialog.FileName = FileName;
                        if (FileName == null || FileName.Length == 0)
                        {
                            if (FullFileDir != null && FullFileDir.Length > 3)
                            {
                                saveFileDialog.FileName = Path.GetFileName(FullFileDir);
                            }
                        }
                        saveFileDialog.Filter = Properties.FormStrings.FilterJson;
                        //saveFileDialog.Filter = "Json files (*.json) | *.json";
                        
                        saveFileDialog.RestoreDirectory = true;
                        saveFileDialog.Title = Properties.FormStrings.Dialog_Title_Pack;
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            string fullfilename = saveFileDialog.FileName;
                            save_JSON_File(fullfilename, richTextBox_JsonText.Text);

                            FileName = Path.GetFileName(fullfilename);
                            FullFileDir = Path.GetDirectoryName(fullfilename);
                            JSON_Modified = false;
                            FormText();
                            if (checkBox_JsonWarnings.Checked) jsonWarnings(fullfilename);
                        }
                        else return;
                    }
                    if (dr == DialogResult.Cancel)
                    {
                        return;
                    }
                }
            }

            string subPath = Application.StartupPath + @"\Watch_face\";
            if (!Directory.Exists(subPath)) Directory.CreateDirectory(subPath);

            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Binary File (*.bin)|*.bin";
            openFileDialog.Filter = Properties.FormStrings.FilterBin;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = false;
            openFileDialog.Title = Properties.FormStrings.Dialog_Title_Unpack;

            if (!File.Exists(textBox_pack_unpack_dir.Text))
            {
                MessageBox.Show(Properties.FormStrings.Message_error_pack_unpack_dir_Text1 +
                    textBox_pack_unpack_dir.Text + Properties.FormStrings.Message_error_pack_unpack_dir_Text2 +
                    Environment.NewLine + Environment.NewLine + Properties.FormStrings.Message_error_pack_unpack_dir_Text3,
                    Properties.FormStrings.Message_error_pack_unpack_dir_Caption,
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Logger.WriteLine("unpack_Click");
                string fullfilename = openFileDialog.FileName;
                string filename = Path.GetFileName(fullfilename);
                filename = filename.Replace(" ", "_");
                string fullPath = subPath + filename;
                if (File.Exists(fullPath))
                {
                    string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
                    string extension = Path.GetExtension(fullPath);
                    string path = Path.GetDirectoryName(fullPath);
                    string newFullPath = fullPath;
                    //if (radioButton_Settings_Unpack_Dialog.Checked)
                    if (Program_Settings.Settings_Unpack_Dialog)
                    {
                        FormFileExists f = new FormFileExists();
                        f.ShowDialog();
                        int dialogResult = f.Data;
                        switch (dialogResult)
                        {
                            case 0:
                                return;
                            case 1:
                                Logger.WriteLine("File.Copy");
                                File.Copy(fullfilename, fullPath, true);
                                newFullPath = Path.Combine(path, fileNameOnly);
                                if (Directory.Exists(newFullPath)) Directory.Delete(newFullPath, true);
                                break;
                            case 2:
                                Logger.WriteLine("newFileName");
                                int count = 1;

                                while (File.Exists(newFullPath))
                                {
                                    string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                                    newFullPath = Path.Combine(path, tempFileName + extension);
                                }
                                File.Copy(fullfilename, newFullPath);
                                fullPath = newFullPath;
                                fileNameOnly = Path.GetFileNameWithoutExtension(newFullPath);
                                path = Path.GetDirectoryName(newFullPath);
                                newFullPath = Path.Combine(path, fileNameOnly);
                                if (Directory.Exists(newFullPath)) Directory.Delete(newFullPath, true);
                                break;
                        } 
                    }
                    //else if (radioButton_Settings_Unpack_Save.Checked)
                    else if (Program_Settings.Settings_Unpack_Save)
                    {
                        Logger.WriteLine("newFileName");
                        int count = 1;
                        
                        while (File.Exists(newFullPath))
                        {
                            string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                            newFullPath = Path.Combine(path, tempFileName + extension);
                        }
                        File.Copy(fullfilename, newFullPath);
                        fullPath = newFullPath;
                        fileNameOnly = Path.GetFileNameWithoutExtension(newFullPath);
                        path = Path.GetDirectoryName(newFullPath);
                        newFullPath = Path.Combine(path, fileNameOnly);
                        if (Directory.Exists(newFullPath)) Directory.Delete(newFullPath, true);
                    }
                    //else if (radioButton_Settings_Unpack_Replace.Checked)
                    else if (Program_Settings.Settings_Unpack_Replace)
                    {
                        Logger.WriteLine("File.Copy");
                        File.Copy(fullfilename, fullPath, true);
                        newFullPath = Path.Combine(path, fileNameOnly);
                        if (Directory.Exists(newFullPath)) Directory.Delete(newFullPath, true);
                    }
                }
                else File.Copy(fullfilename, fullPath);

                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = textBox_pack_unpack_dir.Text;
                    startInfo.Arguments = textBox_unpack_command.Text + " \"" + fullPath + "\"";
                    using (Process exeProcess = Process.Start(startInfo))
                    {
                        exeProcess.WaitForExit();//ждем 
                    };
                    // этот блок закончится только после окончания работы программы 
                    //сюда писать команды после успешного завершения программы
                    string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
                    //string extension = Path.GetExtension(fullPath);
                    string path = Path.GetDirectoryName(fullPath);
                    path = Path.Combine(path, fileNameOnly);
                    string newFullName = Path.Combine(path, fileNameOnly + ".json");

                    if (Program_Settings.Settings_AfterUnpack_Dialog)
                    {
                        if (File.Exists(newFullName))
                        {
                            this.BringToFront();
                            if (MessageBox.Show(Properties.FormStrings.Message_openProject_Text,
                                Properties.FormStrings.Message_openProject_Caption, 
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                LoadJsonAndImage(newFullName);
                            }
                        } 
                    }
                    else if (Program_Settings.Settings_AfterUnpack_Download)
                    {
                        LoadJsonAndImage(newFullName);
                    }
                }
                catch
                {
                    // сюда писать команды при ошибке вызова 
                }
            }
            Logger.WriteLine("* _unpack_Click (end)");
        }

        private void button_pack_Click(object sender, EventArgs e)
        {
            Logger.WriteLine("* _pack_Click");
            if (JSON_Modified) // сохранение если файл не сохранен
            {
                if (FileName != null)
                {
                    DialogResult dr = MessageBox.Show(Properties.FormStrings.Message_Save_JSON_Modified_Text1 +
                        Path.GetFileNameWithoutExtension(FileName) + Properties.FormStrings.Message_Save_JSON_Modified_Text2,
                        Properties.FormStrings.Message_Save_JSON_Modified_Caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    if (dr == DialogResult.Yes)
                    {
                        string fullfilename = Path.Combine(FullFileDir, FileName);
                        save_JSON_File(fullfilename, richTextBox_JsonText.Text);
                        JSON_Modified = false;
                        FormText();
                        if (checkBox_JsonWarnings.Checked) jsonWarnings(fullfilename);
                    }
                    if (dr == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                else
                {
                    DialogResult dr = MessageBox.Show(Properties.FormStrings.Message_Save_new_JSON,
                        Properties.FormStrings.Message_Save_JSON_Modified_Caption,
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    if (dr == DialogResult.Yes)
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.InitialDirectory = FullFileDir;
                        saveFileDialog.FileName = FileName;
                        if (FileName == null || FileName.Length == 0)
                        {
                            if (FullFileDir != null && FullFileDir.Length > 3)
                            {
                                saveFileDialog.FileName = Path.GetFileName(FullFileDir);
                            }
                        }
                        saveFileDialog.Filter = Properties.FormStrings.FilterJson;
                        //saveFileDialog.Filter = "Json files (*.json) | *.json";
                        
                        saveFileDialog.RestoreDirectory = true;
                        saveFileDialog.Title = Properties.FormStrings.Dialog_Title_Pack;
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            string fullfilename = saveFileDialog.FileName;
                            save_JSON_File(fullfilename, richTextBox_JsonText.Text);

                            FileName = Path.GetFileName(fullfilename);
                            FullFileDir = Path.GetDirectoryName(fullfilename);
                            JSON_Modified = false;
                            FormText();
                            if (checkBox_JsonWarnings.Checked) jsonWarnings(fullfilename);
                        }
                        else return;
                    }
                    if (dr == DialogResult.Cancel)
                    {
                        return;
                    }
                }
            }

            string subPath = Application.StartupPath + @"\Watch_face\";
            if (!Directory.Exists(subPath)) Directory.CreateDirectory(subPath);

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = FullFileDir;
            openFileDialog.FileName = FileName;
            openFileDialog.Filter = Properties.FormStrings.FilterJson;
            //openFileDialog.Filter = "Json files (*.json) | *.json";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = false;
            openFileDialog.Title = Properties.FormStrings.Dialog_Title_Pack;

            if (!File.Exists(textBox_pack_unpack_dir.Text))
            {
                MessageBox.Show(Properties.FormStrings.Message_error_pack_unpack_dir_Text1 +
                    textBox_pack_unpack_dir.Text + Properties.FormStrings.Message_error_pack_unpack_dir_Text2 +
                    Environment.NewLine + Environment.NewLine + Properties.FormStrings.Message_error_pack_unpack_dir_Text3,
                    Properties.FormStrings.Message_error_pack_unpack_dir_Caption,
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Logger.WriteLine("pack_Click");
                try
                {
                    string fullfilename = openFileDialog.FileName;
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = textBox_pack_unpack_dir.Text;
                    startInfo.Arguments = textBox_unpack_command.Text + " \"" + fullfilename + "\"";
                    using (Process exeProcess = Process.Start(startInfo))
                    {
                        exeProcess.WaitForExit();//ждем 
                    };
                    // этот блок закончится только после окончания работы программы 
                    //сюда писать команды после успешного завершения программы
                    string fileNameOnly = Path.GetFileNameWithoutExtension(fullfilename);
                    //string extension = Path.GetExtension(fullPath);
                    string path = Path.GetDirectoryName(fullfilename);
                    string newFullName = Path.Combine(path, fileNameOnly + "_packed.bin");

                    //MessageBox.Show(newFullName);
                    if (File.Exists(newFullName))
                    {
                        Logger.WriteLine("GetFileSizeMB");
                        this.BringToFront();
                        double fileSize = (GetFileSizeMB(new FileInfo(newFullName)));
                        Logger.WriteLine("fileSize = " + fileSize.ToString());
                        //if ((fileSize >= 5.5) && (radioButton_TRex.Checked))
                        //{
                        //    MessageBox.Show(Properties.FormStrings.Message_bigFile_Text1_trex + Environment.NewLine + Environment.NewLine +
                        //    Properties.FormStrings.Message_bigFile_Text2, Properties.FormStrings.Message_bigFile_Caption,
                        //    MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //}
                        //if ((fileSize >= 1.5) && (radioButton_42.Checked || radioButton_gts.Checked 
                        //    || radioButton_Verge.Checked || radioButton_AmazfitX.Checked))
                        //{
                        //    MessageBox.Show(Properties.FormStrings.Message_bigFile_Text1_gts + Environment.NewLine + Environment.NewLine +
                        //    Properties.FormStrings.Message_bigFile_Text2, Properties.FormStrings.Message_bigFile_Caption,
                        //    MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //}
                        //if ((fileSize >= 1.95) && (radioButton_47.Checked))
                        //{
                        //    MessageBox.Show(Properties.FormStrings.Message_bigFile_Text1_gtr47 + Environment.NewLine + Environment.NewLine +
                        //    Properties.FormStrings.Message_bigFile_Text2, Properties.FormStrings.Message_bigFile_Caption,
                        //    MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //}

                        //if (radioButton_Settings_Pack_Dialog.Checked)
                        if (Program_Settings.Settings_Pack_Dialog)
                        {
                            if (MessageBox.Show(Properties.FormStrings.Message_GoToFile_Text,
                                Properties.FormStrings.Message_GoToFile_Caption,
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                Process.Start(new ProcessStartInfo("explorer.exe", " /select, " + newFullName));
                            }
                        }
                        else if (Program_Settings.Settings_Pack_GoToFile)
                        {
                            Process.Start(new ProcessStartInfo("explorer.exe", " /select, " + newFullName));
                        }
                    }
                }
                catch
                {
                    // сюда писать команды при ошибке вызова 
                }
            }
            Logger.WriteLine("* _pack_Click (end)");
        }

        private void button_zip_Click(object sender, EventArgs e)
        {
            Logger.WriteLine("* packed_Click");
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Binary File (*.bin)|*.bin";
            openFileDialog.Filter = Properties.FormStrings.FilterBin;
            openFileDialog.FileName = Path.GetFileNameWithoutExtension(FileName) + "_packed";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.InitialDirectory = FullFileDir;
            openFileDialog.Multiselect = false;
            openFileDialog.Title = Properties.FormStrings.Dialog_Title_Unpack;
            string respackerPath = Application.StartupPath + @"\Res_PackerUnpacker\";
            if (Is64Bit()) respackerPath = respackerPath + @"x64\respacker.exe";
            else respackerPath = respackerPath + @"x86\respacker.exe";

#if !DEBUG
            if (!File.Exists(respackerPath))
            {
                MessageBox.Show(Properties.FormStrings.Message_error_respackerPath_Text1 + Environment.NewLine +
                    Properties.FormStrings.Message_error_respackerPath_Text2 + respackerPath + "].\r\n\r\n" +
                    Properties.FormStrings.Message_error_respackerPath_Text3,
                    Properties.FormStrings.Message_error_pack_unpack_dir_Caption,
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
#endif
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Logger.WriteLine("zip_Click");
                string fullfilename = openFileDialog.FileName;
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = respackerPath;
                    startInfo.Arguments = "\"" + fullfilename + "\"";
                    using (Process exeProcess = Process.Start(startInfo))
                    {
                        exeProcess.WaitForExit();//ждем 
                    };
                    // этот блок закончится только после окончания работы программы 
                    //сюда писать команды после успешного завершения программы
                    string fileNameOnly = Path.GetFileNameWithoutExtension(fullfilename);
                    //string extension = Path.GetExtension(fullPath);
                    string path = Path.GetDirectoryName(fullfilename);
                    //path = Path.Combine(path, fileNameOnly);
                    string newFullName_cmp = Path.Combine(path, fileNameOnly + ".bin.cmp");
                    string newFullName_bin = Path.Combine(path, fileNameOnly + "_zip.bin");
                    if (File.Exists(newFullName_cmp)) File.Copy(newFullName_cmp, newFullName_bin, true);
                    if (File.Exists(newFullName_bin))
                    {
                        Logger.WriteLine("newFullName_bin");
                        File.Delete(newFullName_cmp);
                        this.BringToFront();
                        //if (radioButton_Settings_Pack_Dialog.Checked)
                        //MessageBox.Show(GetFileSize(new FileInfo(newFullName_bin)));
                        if (Program_Settings.Settings_Pack_Dialog)
                        {
                            if (MessageBox.Show(Properties.FormStrings.Message_GoToFile_Text,
                                Properties.FormStrings.Message_GoToFile_Caption,
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                Process.Start(new ProcessStartInfo("explorer.exe", " /select, " + newFullName_bin));
                            }
                        }
                        else if (Program_Settings.Settings_Pack_GoToFile)
                        {
                            Process.Start(new ProcessStartInfo("explorer.exe", " /select, " + newFullName_bin));
                        }
                    }
                }
                catch
                {
                }
            }
            Logger.WriteLine("* packed_Click (end)");

        }

        private void button_pack_zip_Click(object sender, EventArgs e)
        {
            Logger.WriteLine("* pack_zip");
            if (JSON_Modified) // сохранение если файл не сохранен
            {
                if (FileName != null)
                {
                    DialogResult dr = MessageBox.Show(Properties.FormStrings.Message_Save_JSON_Modified_Text1 +
                        Path.GetFileNameWithoutExtension(FileName) + Properties.FormStrings.Message_Save_JSON_Modified_Text2,
                        Properties.FormStrings.Message_Save_JSON_Modified_Caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    if (dr == DialogResult.Yes)
                    {
                        string fullfilename = Path.Combine(FullFileDir, FileName);
                        save_JSON_File(fullfilename, richTextBox_JsonText.Text);
                        JSON_Modified = false;
                        FormText();
                        if (checkBox_JsonWarnings.Checked) jsonWarnings(fullfilename);
                    }
                    if (dr == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                else
                {
                    DialogResult dr = MessageBox.Show(Properties.FormStrings.Message_Save_new_JSON,
                        Properties.FormStrings.Message_Save_JSON_Modified_Caption,
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    if (dr == DialogResult.Yes)
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.InitialDirectory = FullFileDir;
                        saveFileDialog.FileName = FileName;
                        if (FileName == null || FileName.Length == 0)
                        {
                            if (FullFileDir != null && FullFileDir.Length > 3)
                            {
                                saveFileDialog.FileName = Path.GetFileName(FullFileDir);
                            }
                        }
                        saveFileDialog.Filter = Properties.FormStrings.FilterJson;

                        //openFileDialog.Filter = "Json files (*.json) | *.json";
                        saveFileDialog.RestoreDirectory = true;
                        saveFileDialog.Title = Properties.FormStrings.Dialog_Title_Pack;
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            string fullfilename = saveFileDialog.FileName;
                            save_JSON_File(fullfilename, richTextBox_JsonText.Text);

                            FileName = Path.GetFileName(fullfilename);
                            FullFileDir = Path.GetDirectoryName(fullfilename);
                            JSON_Modified = false;
                            FormText();
                            if (checkBox_JsonWarnings.Checked) jsonWarnings(fullfilename);
                        }
                        else return;
                    }
                    if (dr == DialogResult.Cancel)
                    {
                        return;
                    }
                }
            }

            //string subPath = Application.StartupPath + @"\Watch_face\";
            //if (!Directory.Exists(subPath)) Directory.CreateDirectory(subPath);

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = FullFileDir;
            openFileDialog.FileName = FileName;
            openFileDialog.Filter = Properties.FormStrings.FilterJson;
            //openFileDialog.Filter = "Json files (*.json) | *.json";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = false;
            openFileDialog.Title = Properties.FormStrings.Dialog_Title_Pack;


            if (!File.Exists(textBox_pack_unpack_dir.Text))
            {
                MessageBox.Show(Properties.FormStrings.Message_error_pack_unpack_dir_Text1 +
                    textBox_pack_unpack_dir.Text + Properties.FormStrings.Message_error_pack_unpack_dir_Text2 +
                    Environment.NewLine + Environment.NewLine + Properties.FormStrings.Message_error_pack_unpack_dir_Text3,
                    Properties.FormStrings.Message_error_pack_unpack_dir_Caption,
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Logger.WriteLine("* pack_zip_Click");
                try
                {
                    string fullfilename = openFileDialog.FileName;
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = textBox_pack_unpack_dir.Text;
                    startInfo.Arguments = textBox_unpack_command.Text + " \"" + fullfilename + "\"";
                    using (Process exeProcess = Process.Start(startInfo))
                    {
                        exeProcess.WaitForExit();//ждем 
                    };
                    // этот блок закончится только после окончания работы программы 
                    //сюда писать команды после успешного завершения программы
                    string fileNameOnly = Path.GetFileNameWithoutExtension(fullfilename);
                    //string extension = Path.GetExtension(fullPath);
                    string path = Path.GetDirectoryName(fullfilename);
                    string newFullName = Path.Combine(path, fileNameOnly + "_packed.bin");

                    //MessageBox.Show(newFullName);
                    if (File.Exists(newFullName))
                    {
                        //Logger.WriteLine("GetFileSizeMB");
                        //double fileSize = (GetFileSizeMB(new FileInfo(newFullName)));

                        startInfo.FileName = Application.StartupPath + @"\Tools\GTR2_Packer.exe"; ;
                        startInfo.Arguments = "-cmp2" + " \"" + newFullName + "\"";
                        using (Process exeProcess = Process.Start(startInfo))
                        {
                            exeProcess.WaitForExit();//ждем 
                        };
                        // этот блок закончится только после окончания работы программы 
                        //сюда писать команды после успешного завершения программы
                        fileNameOnly = Path.GetFileNameWithoutExtension(newFullName);
                        //string extension = Path.GetExtension(fullPath);
                        path = Path.GetDirectoryName(newFullName);
                        //path = Path.Combine(path, fileNameOnly);
                        string newFullName_cmp = Path.Combine(path, fileNameOnly + ".bin.cmp");
                        string newFullName_bin = Path.Combine(path, fileNameOnly + ".bin");
                        //string newFullName_bin = Path.Combine(path, fileNameOnly + "_packed.bin");
                        if (File.Exists(newFullName_cmp)) File.Copy(newFullName_cmp, newFullName_bin, true);
                        if (File.Exists(newFullName_bin))
                        {
                            Logger.WriteLine("newFullName_bin");
                            if (checkBox_UseID.Checked)
                            {
                                using (FileStream fileStream = File.OpenWrite(newFullName_bin))
                                {
                                    int ID = 0;
                                    Int32.TryParse(textBox_WatchfaceID.Text, out ID);
                                    if (ID >= 1000)
                                    {
                                        byte[] arr = BitConverter.GetBytes(ID);
                                        fileStream.Position = 18;
                                        fileStream.WriteByte(arr[0]);
                                        fileStream.WriteByte(arr[1]);
                                        fileStream.WriteByte(arr[2]);
                                        fileStream.WriteByte(arr[3]);
                                        fileStream.Flush();
                                    }
                                }
                            }
                            File.Delete(newFullName_cmp);
                            this.BringToFront();
                            //if (radioButton_Settings_Pack_Dialog.Checked)
                            if (Program_Settings.Settings_Pack_Dialog)
                            {
                                if (MessageBox.Show(Properties.FormStrings.Message_GoToFile_Text,
                                Properties.FormStrings.Message_GoToFile_Caption,
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    Process.Start(new ProcessStartInfo("explorer.exe", " /select, " + newFullName_bin));
                                }
                            }
                            else if (Program_Settings.Settings_Pack_GoToFile)
                            {
                                Process.Start(new ProcessStartInfo("explorer.exe", " /select, " + newFullName_bin));
                            }
                        }
                    }
                }
                catch
                {
                    // сюда писать команды при ошибке вызова 
                }
            }

            Logger.WriteLine("* pack_zip (end)");
        }

        // загружаем перечень картинок
        private void button_images_Click(object sender, EventArgs e)
        {
            Logger.WriteLine("* images (end)");
            string subPath = Application.StartupPath + @"\Watch_face\";
            if (!Directory.Exists(subPath)) Directory.CreateDirectory(subPath);

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = FullFileDir;
            openFileDialog.Filter = Properties.FormStrings.FilterPng;
            //openFileDialog.Filter = "PNG Files: (*.png)|*.png";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = true;
            openFileDialog.Title = Properties.FormStrings.Dialog_Title_Image;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Logger.WriteLine("images_Click");
                FullFileDir = Path.GetDirectoryName(openFileDialog.FileName);
                dataGridView_ImagesList.Rows.Clear();
                ListImages.Clear();
                ListImagesFullName.Clear();
                int i;
                int count = 0;
                int AllFileSize = 0;
                Image loadedImage = null;
                List<string> ErrorImage = new List<string>();
                List<string> FileNames = openFileDialog.FileNames.ToList();
                FileNames.Sort();
                foreach (String file in FileNames)
                    //foreach (String file in openFileDialog.FileNames)
                    {
                    try
                    {
                        string fileNameOnly = Path.GetFileNameWithoutExtension(file);
                        //string fileNameOnly = Path.GetFileName(file);
                        if (int.TryParse(fileNameOnly, out i))
                        {
                            Logger.WriteLine("loadedImage " + fileNameOnly);
                            //Image loadedImage = Image.FromFile(file);
                            using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read))
                            {
                                loadedImage = Image.FromStream(stream);
                            }

                            PixelFormat pf = loadedImage.PixelFormat;
                            if (pf != PixelFormat.Format32bppArgb) ErrorImage.Add(Path.GetFileName(file));
                            int pixels = loadedImage.Width * loadedImage.Height;
                            AllFileSize = AllFileSize + pixels * 4 + 20;

                            var RowNew = new DataGridViewRow();
                            DataGridViewImageCellLayout ZoomType = DataGridViewImageCellLayout.Zoom;
                            if ((loadedImage.Height < 45) && (loadedImage.Width < 110))
                                ZoomType = DataGridViewImageCellLayout.Normal;
                            RowNew.Cells.Add(new DataGridViewTextBoxCell() { Value = count.ToString() });
                            //RowNew.Cells.Add(new DataGridViewTextBoxCell() { Value = i.ToString() });
                            RowNew.Cells.Add(new DataGridViewTextBoxCell() { Value = fileNameOnly });
                            //RowNew.Cells.Add(new DataGridViewTextBoxCell() { Value = file });
                            RowNew.Cells.Add(new DataGridViewImageCell()
                            {
                                Value = loadedImage,
                                ImageLayout = ZoomType
                            });
                            RowNew.Cells.Add(new DataGridViewImageCell()
                            {
                                Value = loadedImage,
                                ImageLayout = ZoomType
                            });
                            RowNew.Height = 45;
                            dataGridView_ImagesList.Rows.Add(RowNew);
                            ListImages.Add(i.ToString());
                            ListImagesFullName.Add(file);
                            count++;
                        }

                    }
                    catch
                    {
                        // Could not load the image - probably related to Windows file system permissions.
                        MessageBox.Show(Properties.FormStrings.Message_error_Image_Text1 + file.Substring(file.LastIndexOf('\\')+1)
                            + Properties.FormStrings.Message_error_Image_Text2);
                    }
                }
                //loadedImage.Dispose();
                if (ErrorImage.Count > 0)
                {
                    Logger.WriteLine("ErrorImage");
                    string StringFileName = string.Join(Environment.NewLine, ErrorImage);
                    if (MessageBox.Show(Properties.FormStrings.Message_ErrorImage_Text1 + ErrorImage.Count.ToString() + "):" +
                        Environment.NewLine + StringFileName + Environment.NewLine + Environment.NewLine +
                        Properties.FormStrings.Message_ErrorImage_Text2, Properties.FormStrings.Message_Warning_Caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        FixImage(Path.GetDirectoryName(openFileDialog.FileName), ErrorImage);
                    }
                }
                PreviewView = false;
                JSON_read();
                PreviewView = true;
                PreviewImage();
                //ShowAllFileSize(AllFileSize);
            }
            Logger.WriteLine("* images (end)");
        }

        private void FixImage(string directory, List<string> errorImage)
        {
            Logger.WriteLine("* FixImage");
            foreach (string fileName in errorImage)
            {
                Logger.WriteLine("FixImage " + fileName);
                string fullFileName = Path.Combine(directory, fileName);

                if (File.Exists(fullFileName))
                {
                    Bitmap bmpTermp = null;
                    using (FileStream stream = new FileStream(fullFileName, FileMode.Open, FileAccess.Read))
                    {
                        bmpTermp = new Bitmap(stream);
                    }
                    try
                    {
                        MagickImage item = new MagickImage(bmpTermp);
                        item.Format = MagickFormat.Png32;
                        item.Write(fullFileName);
                    }
                    catch (Exception){}
                }
            }
            Logger.WriteLine("* FixImage (end)");
        }

        // загружаем JSON файл с настройками
        private void button_JSON_Click(object sender, EventArgs e)
        {
            Logger.WriteLine("* JSON");
            if (JSON_Modified) // сохранение если файл не сохранен
            {
                if (FileName != null)
                {
                    DialogResult dr = MessageBox.Show(Properties.FormStrings.Message_Save_JSON_Modified_Text1 +
                        Path.GetFileNameWithoutExtension(FileName) + Properties.FormStrings.Message_Save_JSON_Modified_Text2,
                        Properties.FormStrings.Message_Save_JSON_Modified_Caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    if (dr == DialogResult.Yes)
                    {
                        string fullfilename = Path.Combine(FullFileDir, FileName);
                        save_JSON_File(fullfilename, richTextBox_JsonText.Text);
                        JSON_Modified = false;
                        FormText();
                        if (checkBox_JsonWarnings.Checked) jsonWarnings(fullfilename);
                    }
                    if (dr == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                else
                {
                    DialogResult dr = MessageBox.Show(Properties.FormStrings.Message_Save_new_JSON,
                        Properties.FormStrings.Message_Save_JSON_Modified_Caption,
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    if (dr == DialogResult.Yes)
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.InitialDirectory = FullFileDir;
                        saveFileDialog.FileName = FileName;
                        if (FileName == null || FileName.Length == 0)
                        {
                            if (FullFileDir != null && FullFileDir.Length > 3)
                            {
                                saveFileDialog.FileName = Path.GetFileName(FullFileDir);
                            }
                        }
                        saveFileDialog.Filter = Properties.FormStrings.FilterJson;

                        //openFileDialog.Filter = "Json files (*.json) | *.json";
                        saveFileDialog.RestoreDirectory = true;
                        saveFileDialog.Title = Properties.FormStrings.Dialog_Title_Pack;
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            string fullfilename = saveFileDialog.FileName;
                            save_JSON_File(fullfilename, richTextBox_JsonText.Text);

                            FileName = Path.GetFileName(fullfilename);
                            FullFileDir = Path.GetDirectoryName(fullfilename);
                            JSON_Modified = false;
                            FormText();
                            if (checkBox_JsonWarnings.Checked) jsonWarnings(fullfilename);
                        }
                        else return;
                    }
                    if (dr == DialogResult.Cancel)
                    {
                        return;
                    }
                }
            }

            string subPath = Application.StartupPath + @"\Watch_face\";
            if (!Directory.Exists(subPath)) Directory.CreateDirectory(subPath);

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = FullFileDir;
            openFileDialog.FileName = FileName;
            openFileDialog.Filter = Properties.FormStrings.FilterJson;
            //openFileDialog.Filter = "Json files (*.json) | *.json";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = false;
            openFileDialog.Title = Properties.FormStrings.Dialog_Title_Pack;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Logger.WriteLine("* JSON_Click");
                //string fullfilename = openFileDialog.FileName;
                LoadJsonAndImage(openFileDialog.FileName);
            }
            Logger.WriteLine("* JSON (end)");
        }

        private void LoadJsonAndImage(string fullfilename)
        {
            Logger.WriteLine("* LoadJsonAndImage");
            FileName = Path.GetFileName(fullfilename);
            FullFileDir = Path.GetDirectoryName(fullfilename);
            string text = File.ReadAllText(fullfilename);
            //richTextBox_JsonText.Text = text;
            PreviewView = false;
            int AllFileSize = 0;
            ListImages.Clear();
            ListImagesFullName.Clear();
            dataGridView_ImagesList.Rows.Clear();
            Logger.WriteLine("Прочитали текст из json файла " + fullfilename);

            DirectoryInfo Folder;
            FileInfo[] Images;
            Folder = new DirectoryInfo(FullFileDir);
            Images = Folder.GetFiles("*.png").OrderBy(p => Path.GetFileNameWithoutExtension(p.Name)).ToArray();
            int count = 1;
            Image loadedImage = null;
            List<string> ErrorImage = new List<string>();
            foreach (FileInfo file in Images)
            {
                try
                {
                    string fileNameOnly = Path.GetFileNameWithoutExtension(file.Name);
                    int i;
                    if (int.TryParse(fileNameOnly, out i))
                    {
                        Logger.WriteLine("loadedImage " + fileNameOnly);
                        //loadedImage = Image.FromFile(file.FullName);
                        using (FileStream stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
                        {
                            loadedImage = Image.FromStream(stream);
                        }

                        PixelFormat pf = loadedImage.PixelFormat;
                        if (pf != PixelFormat.Format32bppArgb) ErrorImage.Add(Path.GetFileName(file.FullName));
                        int pixels = loadedImage.Width * loadedImage.Height;
                        AllFileSize = AllFileSize + pixels * 4 + 20;

                        var RowNew = new DataGridViewRow();
                        DataGridViewImageCellLayout ZoomType = DataGridViewImageCellLayout.Zoom;
                        if ((loadedImage.Height < 45) && (loadedImage.Width < 110))
                            ZoomType = DataGridViewImageCellLayout.Normal;
                        RowNew.Cells.Add(new DataGridViewTextBoxCell() { Value = count.ToString() });
                        //RowNew.Cells.Add(new DataGridViewTextBoxCell() { Value = i.ToString() });
                        RowNew.Cells.Add(new DataGridViewTextBoxCell() { Value = fileNameOnly });
                        RowNew.Cells.Add(new DataGridViewImageCell()
                        {
                            Value = loadedImage,
                            ImageLayout = ZoomType,

                        });
                        RowNew.Cells.Add(new DataGridViewImageCell()
                        {
                            Value = loadedImage,
                            ImageLayout = ZoomType,

                        });
                        //loadedImage.Dispose();
                        RowNew.Height = 45;
                        dataGridView_ImagesList.Rows.Add(RowNew);
                        count++;
                        ListImages.Add(i.ToString());
                        ListImagesFullName.Add(file.FullName);
                    }
                }
                catch
                {
                    // Could not load the image - probably related to Windows file system permissions.
                    MessageBox.Show(Properties.FormStrings.Message_error_Image_Text1 +
                        file.FullName.Substring(file.FullName.LastIndexOf('\\') + 1) + Properties.FormStrings.Message_error_Image_Text2);
                }
            }
            //Logger.WriteLine("Загрузили все файлы изображений");

            //loadedImage.Dispose();
#if !DEBUG
            int LastImage = -1;
            if (ListImages.Count > 0)
            {
                Int32.TryParse(ListImages[0], out LastImage);
                if (LastImage != 1)
                {
                    MessageBox.Show(Properties.FormStrings.Message_PNGFromOne_Text, 
                        Properties.FormStrings.Message_Error_Caption,
                         MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Int32.TryParse(ListImages.Last(), out LastImage);
                    LastImage++;

                    if (count != LastImage) MessageBox.Show(Properties.FormStrings.Message_PNGmissing_Text1 + Environment.NewLine +
                         Properties.FormStrings.Message_PNGmissing_Text2, Properties.FormStrings.Message_Error_Caption,
                         MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
#endif
            if (ErrorImage.Count > 0)
            {
                Logger.WriteLine("ErrorImage");
                string StringFileName = string.Join(Environment.NewLine, ErrorImage);
                if (MessageBox.Show(Properties.FormStrings.Message_ErrorImage_Text1 + ErrorImage.Count.ToString() + "):" +
                    Environment.NewLine + StringFileName + Environment.NewLine + Environment.NewLine +
                    Properties.FormStrings.Message_ErrorImage_Text2, Properties.FormStrings.Message_Warning_Caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    FixImage(FullFileDir, ErrorImage);
                }
            }

            try
            {
                Watch_Face = JsonConvert.DeserializeObject<WATCH_FACE_JSON>(text, new JsonSerializerSettings
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(Properties.FormStrings.Message_JsonError_Text + Environment.NewLine + ex,
                    Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            //Logger.WriteLine("Распознали json формат");

            richTextBox_JsonText.Text = JsonConvert.SerializeObject(Watch_Face, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            });
            JsonToTree(richTextBox_JsonText.Text);



            JSON_read();
            //Logger.WriteLine("Установили значения в соответствии с json файлом");
            string path = Path.GetDirectoryName(fullfilename);
            string newFullName = Path.Combine(path, "Watchface.ID");
            if (File.Exists(newFullName))
            {
                WatchfaceID watchfaceID = new WatchfaceID();
                watchfaceID = JsonConvert.DeserializeObject<WatchfaceID>
                                (File.ReadAllText(newFullName), new JsonSerializerSettings
                                {
                                    //DefaultValueHandling = DefaultValueHandling.Ignore,
                                    NullValueHandling = NullValueHandling.Ignore
                                });
                checkBox_UseID.Checked = watchfaceID.UseID;
                textBox_WatchfaceID.Text = watchfaceID.ID.ToString();
            }
            else
            {
                textBox_WatchfaceID.Text = "";
                checkBox_UseID.Checked = false;
            }

            newFullName = Path.Combine(path, "PreviewStates.json");
            if (File.Exists(newFullName))
            {
                Logger.WriteLine("Load PreviewStates.json");
                if (Program_Settings.Settings_Open_Download)
                {
                    JsonPreview_Read(newFullName);
                }
                else if (Program_Settings.Settings_Open_Dialog)
                {
                    if (MessageBox.Show(Properties.FormStrings.Message_LoadPreviewStates_Text,
                        Properties.FormStrings.Message_openProject_Caption,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        JsonPreview_Read(newFullName);
                    }
                }
            }
            else button_JsonPreview_Random.PerformClick();
            PreviewView = true;
            PreviewImage();
            JSON_Modified = false;
            FormText();
            //ShowAllFileSize(AllFileSize);
            if (comboBox_Preview_image.SelectedIndex >= 0)
            {
                button_RefreshPreview.Visible = true;
                button_CreatePreview.Visible = false;
            }
            else
            {
                button_RefreshPreview.Visible = false;
                if (FileName != null && FullFileDir != null)
                {
                    button_CreatePreview.Visible = true;
                }
                else
                {
                    button_CreatePreview.Visible = false;
                }
            }
            Logger.WriteLine("* LoadJsonAndImage (end)");
        }

        //private void ShowAllFileSize(double sizeinbytes)
        //{
        //    Logger.WriteLine("* ShowAllFileSize");
        //    double AllFileSizeMB = GetFileSizeMB(sizeinbytes);
        //    label_size.Text = "≈" + AllFileSizeMB.ToString() + "MB";
        //    Logger.WriteLine("* ShowAllFileSize (end)");
        //}


        // формируем изображение для предпросмотра
        private void PreviewImage()
        {
            Logger.WriteLine("* PreviewImage");
            if (!PreviewView) return;
            //Graphics gPanel = panel_Preview.CreateGraphics();
            //gPanel.Clear(panel_Preview.BackColor);
            float scale = 1.0f;
            //if (panel_Preview.Height < 300) scale = 0.5f;
#region BackgroundImage
            Logger.WriteLine("BackgroundImage");
            Bitmap bitmap = new Bitmap(Convert.ToInt32(454), Convert.ToInt32(454), PixelFormat.Format32bppArgb);
            if (radioButton_GTS2.Checked)
            {
                bitmap = new Bitmap(Convert.ToInt32(348), Convert.ToInt32(442), PixelFormat.Format32bppArgb);
            }
            Graphics gPanel = Graphics.FromImage(bitmap);
#endregion

            Logger.WriteLine("PreviewToBitmap");
            int link = radioButton_ScreenNormal.Checked ? 0 : 1;
            PreviewToBitmap(gPanel, scale, checkBox_crop.Checked, checkBox_WebW.Checked, checkBox_WebB.Checked, 
                checkBox_border.Checked, checkBox_Show_Shortcuts.Checked, checkBox_Shortcuts_Area.Checked, 
                checkBox_Shortcuts_Border.Checked, true, checkBox_CircleScaleImage.Checked, 
                checkBox_center_marker.Checked, link);
            pictureBox_Preview.BackgroundImage = bitmap;
            gPanel.Dispose();

            if ((formPreview != null) && (formPreview.Visible))
            {
                //Graphics gPanelPreview = formPreview.panel_Preview.CreateGraphics();
                //gPanelPreview.Clear(pictureBox_Preview.BackColor);
                //float scalePreview = 1.0f;
                //if (formPreview.radioButton_small.Checked) scalePreview = 0.5f;
                //if (formPreview.radioButton_large.Checked) scalePreview = 1.5f;
                //if (formPreview.radioButton_xlarge.Checked) scalePreview = 2.0f;
                //if (formPreview.radioButton_xxlarge.Checked) scalePreview = 2.5f;
                //PreviewToBitmap(gPanelPreview, scalePreview, checkBox_crop.Checked, checkBox_WebW.Checked, 
                //    checkBox_WebB.Checked, checkBox_border.Checked, checkBox_Show_Shortcuts.Checked, 
                //    checkBox_Shortcuts_Area.Checked, checkBox_Shortcuts_Border.Checked, true, 0);
                //gPanelPreview.Dispose();

                formPreview.pictureBox_Preview.BackgroundImage = bitmap;
            }
            Logger.WriteLine("* PreviewImage (end)");
        }
        

#region выбираем данные для предпросмотра
        public void SetPreferences1()
        {
            Watch_Face_Preview_Set.Date.Year = dateTimePicker_Date_Set1.Value.Year;
            Watch_Face_Preview_Set.Date.Month = dateTimePicker_Date_Set1.Value.Month;
            Watch_Face_Preview_Set.Date.Day = dateTimePicker_Date_Set1.Value.Day;
            Watch_Face_Preview_Set.Date.WeekDay = (int)dateTimePicker_Date_Set1.Value.DayOfWeek;
            if (Watch_Face_Preview_Set.Date.WeekDay == 0) Watch_Face_Preview_Set.Date.WeekDay = 7;

            Watch_Face_Preview_Set.Time.Hours = dateTimePicker_Time_Set1.Value.Hour;
            Watch_Face_Preview_Set.Time.Minutes = dateTimePicker_Time_Set1.Value.Minute;
            Watch_Face_Preview_Set.Time.Seconds = dateTimePicker_Time_Set1.Value.Second;

            Watch_Face_Preview_Set.Battery = (int)numericUpDown_Battery_Set1.Value;
            Watch_Face_Preview_Set.Activity.Calories = (int)numericUpDown_Calories_Set1.Value;
            Watch_Face_Preview_Set.Activity.HeartRate = (int)numericUpDown_Pulse_Set1.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set1.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set1.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set1.Value;
            Watch_Face_Preview_Set.Activity.PAI = (int)numericUpDown_PAI_Set1.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set1.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set1.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set1.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set1.Checked;

            Watch_Face_Preview_Set.Weather.Temperature = (int)numericUpDown_WeatherSet_Temp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureMax = (int)numericUpDown_WeatherSet_MaxTemp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureMin = (int)numericUpDown_WeatherSet_MinTemp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureNoData = !checkBox_WeatherSet_Temp.Checked;
            Watch_Face_Preview_Set.Weather.TemperatureMinMaxNoData = !checkBox_WeatherSet_MaxMinTemp.Checked;
            Watch_Face_Preview_Set.Weather.Icon = comboBox_WeatherSet_Icon.SelectedIndex;

            SetDigitForPrewiev();
        }

        public void SetPreferences2()
        {
            Watch_Face_Preview_Set.Date.Year = dateTimePicker_Date_Set2.Value.Year;
            Watch_Face_Preview_Set.Date.Month = dateTimePicker_Date_Set2.Value.Month;
            Watch_Face_Preview_Set.Date.Day = dateTimePicker_Date_Set2.Value.Day;
            Watch_Face_Preview_Set.Date.WeekDay = (int)dateTimePicker_Date_Set2.Value.DayOfWeek;
            if (Watch_Face_Preview_Set.Date.WeekDay == 0) Watch_Face_Preview_Set.Date.WeekDay = 7;

            Watch_Face_Preview_Set.Time.Hours = dateTimePicker_Time_Set2.Value.Hour;
            Watch_Face_Preview_Set.Time.Minutes = dateTimePicker_Time_Set2.Value.Minute;
            Watch_Face_Preview_Set.Time.Seconds = dateTimePicker_Time_Set2.Value.Second;

            Watch_Face_Preview_Set.Battery = (int)numericUpDown_Battery_Set2.Value;
            Watch_Face_Preview_Set.Activity.Calories = (int)numericUpDown_Calories_Set2.Value;
            Watch_Face_Preview_Set.Activity.HeartRate = (int)numericUpDown_Pulse_Set2.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set2.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set2.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set2.Value;
            Watch_Face_Preview_Set.Activity.PAI = (int)numericUpDown_PAI_Set2.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set2.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set2.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set2.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set2.Checked;

            Watch_Face_Preview_Set.Weather.Temperature = (int)numericUpDown_WeatherSet_Temp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureMax = (int)numericUpDown_WeatherSet_MaxTemp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureMin = (int)numericUpDown_WeatherSet_MinTemp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureNoData = !checkBox_WeatherSet_Temp.Checked;
            Watch_Face_Preview_Set.Weather.TemperatureMinMaxNoData = !checkBox_WeatherSet_MaxMinTemp.Checked;
            Watch_Face_Preview_Set.Weather.Icon = comboBox_WeatherSet_Icon.SelectedIndex;

            SetDigitForPrewiev();
        }

        public void SetPreferences3()
        {
            Watch_Face_Preview_Set.Date.Year = dateTimePicker_Date_Set3.Value.Year;
            Watch_Face_Preview_Set.Date.Month = dateTimePicker_Date_Set3.Value.Month;
            Watch_Face_Preview_Set.Date.Day = dateTimePicker_Date_Set3.Value.Day;
            Watch_Face_Preview_Set.Date.WeekDay = (int)dateTimePicker_Date_Set3.Value.DayOfWeek;
            if (Watch_Face_Preview_Set.Date.WeekDay == 0) Watch_Face_Preview_Set.Date.WeekDay = 7;

            Watch_Face_Preview_Set.Time.Hours = dateTimePicker_Time_Set3.Value.Hour;
            Watch_Face_Preview_Set.Time.Minutes = dateTimePicker_Time_Set3.Value.Minute;
            Watch_Face_Preview_Set.Time.Seconds = dateTimePicker_Time_Set3.Value.Second;

            Watch_Face_Preview_Set.Battery = (int)numericUpDown_Battery_Set3.Value;
            Watch_Face_Preview_Set.Activity.Calories = (int)numericUpDown_Calories_Set3.Value;
            Watch_Face_Preview_Set.Activity.HeartRate = (int)numericUpDown_Pulse_Set3.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set3.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set3.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set3.Value;
            Watch_Face_Preview_Set.Activity.PAI = (int)numericUpDown_PAI_Set3.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set3.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set3.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set3.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set3.Checked;

            Watch_Face_Preview_Set.Weather.Temperature = (int)numericUpDown_WeatherSet_Temp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureMax = (int)numericUpDown_WeatherSet_MaxTemp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureMin = (int)numericUpDown_WeatherSet_MinTemp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureNoData = !checkBox_WeatherSet_Temp.Checked;
            Watch_Face_Preview_Set.Weather.TemperatureMinMaxNoData = !checkBox_WeatherSet_MaxMinTemp.Checked;
            Watch_Face_Preview_Set.Weather.Icon = comboBox_WeatherSet_Icon.SelectedIndex;

            SetDigitForPrewiev();
        }

        public void SetPreferences4()
        {
            Watch_Face_Preview_Set.Date.Year = dateTimePicker_Date_Set4.Value.Year;
            Watch_Face_Preview_Set.Date.Month = dateTimePicker_Date_Set4.Value.Month;
            Watch_Face_Preview_Set.Date.Day = dateTimePicker_Date_Set4.Value.Day;
            Watch_Face_Preview_Set.Date.WeekDay = (int)dateTimePicker_Date_Set4.Value.DayOfWeek;
            if (Watch_Face_Preview_Set.Date.WeekDay == 0) Watch_Face_Preview_Set.Date.WeekDay = 7;

            Watch_Face_Preview_Set.Time.Hours = dateTimePicker_Time_Set4.Value.Hour;
            Watch_Face_Preview_Set.Time.Minutes = dateTimePicker_Time_Set4.Value.Minute;
            Watch_Face_Preview_Set.Time.Seconds = dateTimePicker_Time_Set4.Value.Second;

            Watch_Face_Preview_Set.Battery = (int)numericUpDown_Battery_Set4.Value;
            Watch_Face_Preview_Set.Activity.Calories = (int)numericUpDown_Calories_Set4.Value;
            Watch_Face_Preview_Set.Activity.HeartRate = (int)numericUpDown_Pulse_Set4.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set4.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set4.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set4.Value;
            Watch_Face_Preview_Set.Activity.PAI = (int)numericUpDown_PAI_Set4.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set4.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set4.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set4.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set4.Checked;

            Watch_Face_Preview_Set.Weather.Temperature = (int)numericUpDown_WeatherSet_Temp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureMax = (int)numericUpDown_WeatherSet_MaxTemp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureMin = (int)numericUpDown_WeatherSet_MinTemp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureNoData = !checkBox_WeatherSet_Temp.Checked;
            Watch_Face_Preview_Set.Weather.TemperatureMinMaxNoData = !checkBox_WeatherSet_MaxMinTemp.Checked;
            Watch_Face_Preview_Set.Weather.Icon = comboBox_WeatherSet_Icon.SelectedIndex;

            SetDigitForPrewiev();
        }

        public void SetPreferences5()
        {
            Watch_Face_Preview_Set.Date.Year = dateTimePicker_Date_Set5.Value.Year;
            Watch_Face_Preview_Set.Date.Month = dateTimePicker_Date_Set5.Value.Month;
            Watch_Face_Preview_Set.Date.Day = dateTimePicker_Date_Set5.Value.Day;
            Watch_Face_Preview_Set.Date.WeekDay = (int)dateTimePicker_Date_Set5.Value.DayOfWeek;
            if (Watch_Face_Preview_Set.Date.WeekDay == 0) Watch_Face_Preview_Set.Date.WeekDay = 7;

            Watch_Face_Preview_Set.Time.Hours = dateTimePicker_Time_Set5.Value.Hour;
            Watch_Face_Preview_Set.Time.Minutes = dateTimePicker_Time_Set5.Value.Minute;
            Watch_Face_Preview_Set.Time.Seconds = dateTimePicker_Time_Set5.Value.Second;

            Watch_Face_Preview_Set.Battery = (int)numericUpDown_Battery_Set5.Value;
            Watch_Face_Preview_Set.Activity.Calories = (int)numericUpDown_Calories_Set5.Value;
            Watch_Face_Preview_Set.Activity.HeartRate = (int)numericUpDown_Pulse_Set5.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set5.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set5.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set5.Value;
            Watch_Face_Preview_Set.Activity.PAI = (int)numericUpDown_PAI_Set5.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set5.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set5.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set5.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set5.Checked;

            Watch_Face_Preview_Set.Weather.Temperature = (int)numericUpDown_WeatherSet_Temp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureMax = (int)numericUpDown_WeatherSet_MaxTemp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureMin = (int)numericUpDown_WeatherSet_MinTemp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureNoData = !checkBox_WeatherSet_Temp.Checked;
            Watch_Face_Preview_Set.Weather.TemperatureMinMaxNoData = !checkBox_WeatherSet_MaxMinTemp.Checked;
            Watch_Face_Preview_Set.Weather.Icon = comboBox_WeatherSet_Icon.SelectedIndex;

            SetDigitForPrewiev();
        }

        public void SetPreferences6()
        {
            Watch_Face_Preview_Set.Date.Year = dateTimePicker_Date_Set6.Value.Year;
            Watch_Face_Preview_Set.Date.Month = dateTimePicker_Date_Set6.Value.Month;
            Watch_Face_Preview_Set.Date.Day = dateTimePicker_Date_Set6.Value.Day;
            Watch_Face_Preview_Set.Date.WeekDay = (int)dateTimePicker_Date_Set6.Value.DayOfWeek;
            if (Watch_Face_Preview_Set.Date.WeekDay == 0) Watch_Face_Preview_Set.Date.WeekDay = 7;

            Watch_Face_Preview_Set.Time.Hours = dateTimePicker_Time_Set6.Value.Hour;
            Watch_Face_Preview_Set.Time.Minutes = dateTimePicker_Time_Set6.Value.Minute;
            Watch_Face_Preview_Set.Time.Seconds = dateTimePicker_Time_Set6.Value.Second;

            Watch_Face_Preview_Set.Battery = (int)numericUpDown_Battery_Set6.Value;
            Watch_Face_Preview_Set.Activity.Calories = (int)numericUpDown_Calories_Set6.Value;
            Watch_Face_Preview_Set.Activity.HeartRate = (int)numericUpDown_Pulse_Set6.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set6.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set6.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set6.Value;
            Watch_Face_Preview_Set.Activity.PAI = (int)numericUpDown_PAI_Set6.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set6.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set6.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set6.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set6.Checked;

            Watch_Face_Preview_Set.Weather.Temperature = (int)numericUpDown_WeatherSet_Temp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureMax = (int)numericUpDown_WeatherSet_MaxTemp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureMin = (int)numericUpDown_WeatherSet_MinTemp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureNoData = !checkBox_WeatherSet_Temp.Checked;
            Watch_Face_Preview_Set.Weather.TemperatureMinMaxNoData = !checkBox_WeatherSet_MaxMinTemp.Checked;
            Watch_Face_Preview_Set.Weather.Icon = comboBox_WeatherSet_Icon.SelectedIndex;

            SetDigitForPrewiev();
        }

        public void SetPreferences7()
        {
            Watch_Face_Preview_Set.Date.Year = dateTimePicker_Date_Set7.Value.Year;
            Watch_Face_Preview_Set.Date.Month = dateTimePicker_Date_Set7.Value.Month;
            Watch_Face_Preview_Set.Date.Day = dateTimePicker_Date_Set7.Value.Day;
            Watch_Face_Preview_Set.Date.WeekDay = (int)dateTimePicker_Date_Set7.Value.DayOfWeek;
            if (Watch_Face_Preview_Set.Date.WeekDay == 0) Watch_Face_Preview_Set.Date.WeekDay = 7;

            Watch_Face_Preview_Set.Time.Hours = dateTimePicker_Time_Set7.Value.Hour;
            Watch_Face_Preview_Set.Time.Minutes = dateTimePicker_Time_Set7.Value.Minute;
            Watch_Face_Preview_Set.Time.Seconds = dateTimePicker_Time_Set7.Value.Second;

            Watch_Face_Preview_Set.Battery = (int)numericUpDown_Battery_Set7.Value;
            Watch_Face_Preview_Set.Activity.Calories = (int)numericUpDown_Calories_Set7.Value;
            Watch_Face_Preview_Set.Activity.HeartRate = (int)numericUpDown_Pulse_Set7.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set7.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set7.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set7.Value;
            Watch_Face_Preview_Set.Activity.PAI = (int)numericUpDown_PAI_Set7.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set7.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set7.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set7.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set7.Checked;

            Watch_Face_Preview_Set.Weather.Temperature = (int)numericUpDown_WeatherSet_Temp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureMax = (int)numericUpDown_WeatherSet_MaxTemp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureMin = (int)numericUpDown_WeatherSet_MinTemp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureNoData = !checkBox_WeatherSet_Temp.Checked;
            Watch_Face_Preview_Set.Weather.TemperatureMinMaxNoData = !checkBox_WeatherSet_MaxMinTemp.Checked;
            Watch_Face_Preview_Set.Weather.Icon = comboBox_WeatherSet_Icon.SelectedIndex;

            SetDigitForPrewiev();
        }

        public void SetPreferences8()
        {
            Watch_Face_Preview_Set.Date.Year = dateTimePicker_Date_Set8.Value.Year;
            Watch_Face_Preview_Set.Date.Month = dateTimePicker_Date_Set8.Value.Month;
            Watch_Face_Preview_Set.Date.Day = dateTimePicker_Date_Set8.Value.Day;
            Watch_Face_Preview_Set.Date.WeekDay = (int)dateTimePicker_Date_Set8.Value.DayOfWeek;
            if (Watch_Face_Preview_Set.Date.WeekDay == 0) Watch_Face_Preview_Set.Date.WeekDay = 7;

            Watch_Face_Preview_Set.Time.Hours = dateTimePicker_Time_Set8.Value.Hour;
            Watch_Face_Preview_Set.Time.Minutes = dateTimePicker_Time_Set8.Value.Minute;
            Watch_Face_Preview_Set.Time.Seconds = dateTimePicker_Time_Set8.Value.Second;

            Watch_Face_Preview_Set.Battery = (int)numericUpDown_Battery_Set8.Value;
            Watch_Face_Preview_Set.Activity.Calories = (int)numericUpDown_Calories_Set8.Value;
            Watch_Face_Preview_Set.Activity.HeartRate = (int)numericUpDown_Pulse_Set8.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set8.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set8.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set8.Value;
            Watch_Face_Preview_Set.Activity.PAI = (int)numericUpDown_PAI_Set8.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set8.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set8.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set8.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set8.Checked;

            Watch_Face_Preview_Set.Weather.Temperature = (int)numericUpDown_WeatherSet_Temp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureMax = (int)numericUpDown_WeatherSet_MaxTemp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureMin = (int)numericUpDown_WeatherSet_MinTemp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureNoData = !checkBox_WeatherSet_Temp.Checked;
            Watch_Face_Preview_Set.Weather.TemperatureMinMaxNoData = !checkBox_WeatherSet_MaxMinTemp.Checked;
            Watch_Face_Preview_Set.Weather.Icon = comboBox_WeatherSet_Icon.SelectedIndex;

            SetDigitForPrewiev();
        }

        public void SetPreferences9()
        {
            Watch_Face_Preview_Set.Date.Year = dateTimePicker_Date_Set9.Value.Year;
            Watch_Face_Preview_Set.Date.Month = dateTimePicker_Date_Set9.Value.Month;
            Watch_Face_Preview_Set.Date.Day = dateTimePicker_Date_Set9.Value.Day;
            Watch_Face_Preview_Set.Date.WeekDay = (int)dateTimePicker_Date_Set9.Value.DayOfWeek;
            if (Watch_Face_Preview_Set.Date.WeekDay == 0) Watch_Face_Preview_Set.Date.WeekDay = 7;

            Watch_Face_Preview_Set.Time.Hours = dateTimePicker_Time_Set9.Value.Hour;
            Watch_Face_Preview_Set.Time.Minutes = dateTimePicker_Time_Set9.Value.Minute;
            Watch_Face_Preview_Set.Time.Seconds = dateTimePicker_Time_Set9.Value.Second;

            Watch_Face_Preview_Set.Battery = (int)numericUpDown_Battery_Set9.Value;
            Watch_Face_Preview_Set.Activity.Calories = (int)numericUpDown_Calories_Set9.Value;
            Watch_Face_Preview_Set.Activity.HeartRate = (int)numericUpDown_Pulse_Set9.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set9.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set9.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set9.Value;
            Watch_Face_Preview_Set.Activity.PAI = (int)numericUpDown_PAI_Set9.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set9.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set9.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set9.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set9.Checked;

            Watch_Face_Preview_Set.Weather.Temperature = (int)numericUpDown_WeatherSet_Temp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureMax = (int)numericUpDown_WeatherSet_MaxTemp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureMin = (int)numericUpDown_WeatherSet_MinTemp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureNoData = !checkBox_WeatherSet_Temp.Checked;
            Watch_Face_Preview_Set.Weather.TemperatureMinMaxNoData = !checkBox_WeatherSet_MaxMinTemp.Checked;
            Watch_Face_Preview_Set.Weather.Icon = comboBox_WeatherSet_Icon.SelectedIndex;

            SetDigitForPrewiev();
        }

        public void SetPreferences10()
        {
            Watch_Face_Preview_Set.Date.Year = dateTimePicker_Date_Set10.Value.Year;
            Watch_Face_Preview_Set.Date.Month = dateTimePicker_Date_Set10.Value.Month;
            Watch_Face_Preview_Set.Date.Day = dateTimePicker_Date_Set10.Value.Day;
            Watch_Face_Preview_Set.Date.WeekDay = (int)dateTimePicker_Date_Set10.Value.DayOfWeek;
            if (Watch_Face_Preview_Set.Date.WeekDay == 0) Watch_Face_Preview_Set.Date.WeekDay = 7;

            Watch_Face_Preview_Set.Time.Hours = dateTimePicker_Time_Set10.Value.Hour;
            Watch_Face_Preview_Set.Time.Minutes = dateTimePicker_Time_Set10.Value.Minute;
            Watch_Face_Preview_Set.Time.Seconds = dateTimePicker_Time_Set10.Value.Second;

            Watch_Face_Preview_Set.Battery = (int)numericUpDown_Battery_Set10.Value;
            Watch_Face_Preview_Set.Activity.Calories = (int)numericUpDown_Calories_Set10.Value;
            Watch_Face_Preview_Set.Activity.HeartRate = (int)numericUpDown_Pulse_Set10.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set10.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set10.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set10.Value;
            Watch_Face_Preview_Set.Activity.PAI = (int)numericUpDown_PAI_Set10.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set10.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set10.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set10.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set10.Checked;

            Watch_Face_Preview_Set.Weather.Temperature = (int)numericUpDown_WeatherSet_Temp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureMax = (int)numericUpDown_WeatherSet_MaxTemp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureMin = (int)numericUpDown_WeatherSet_MinTemp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureNoData = !checkBox_WeatherSet_Temp.Checked;
            Watch_Face_Preview_Set.Weather.TemperatureMinMaxNoData = !checkBox_WeatherSet_MaxMinTemp.Checked;
            Watch_Face_Preview_Set.Weather.Icon = comboBox_WeatherSet_Icon.SelectedIndex;

            SetDigitForPrewiev();
        }
#endregion

        // определяем отдельные цифры для даты и времени
        private void SetDigitForPrewiev()
        {
            Watch_Face_Preview_TwoDigits.Date.Month.Tens = (int)Watch_Face_Preview_Set.Date.Month / 10;
            Watch_Face_Preview_TwoDigits.Date.Month.Ones = Watch_Face_Preview_Set.Date.Month -
                Watch_Face_Preview_TwoDigits.Date.Month.Tens * 10;
            Watch_Face_Preview_TwoDigits.Date.Day.Tens = (int)Watch_Face_Preview_Set.Date.Day / 10;
            Watch_Face_Preview_TwoDigits.Date.Day.Ones = Watch_Face_Preview_Set.Date.Day -
                Watch_Face_Preview_TwoDigits.Date.Day.Tens * 10;

            Watch_Face_Preview_TwoDigits.Year.Thousands = (int)Watch_Face_Preview_Set.Date.Year / 1000;
            Watch_Face_Preview_TwoDigits.Year.Hundreds = (int)(Watch_Face_Preview_Set.Date.Year -
                Watch_Face_Preview_TwoDigits.Year.Thousands * 1000)/100;
            Watch_Face_Preview_TwoDigits.Year.Tens = (int)(Watch_Face_Preview_Set.Date.Year -
                Watch_Face_Preview_TwoDigits.Year.Thousands * 1000 - Watch_Face_Preview_TwoDigits.Year.Hundreds * 100)/10;
            Watch_Face_Preview_TwoDigits.Year.Ones = Watch_Face_Preview_Set.Date.Year -
                Watch_Face_Preview_TwoDigits.Year.Thousands * 1000 - Watch_Face_Preview_TwoDigits.Year.Hundreds * 100 - 
                Watch_Face_Preview_TwoDigits.Year.Tens * 10;

            Watch_Face_Preview_TwoDigits.Time.Hours.Tens = (int)Watch_Face_Preview_Set.Time.Hours / 10;
            Watch_Face_Preview_TwoDigits.Time.Hours.Ones = Watch_Face_Preview_Set.Time.Hours -
                Watch_Face_Preview_TwoDigits.Time.Hours.Tens * 10;
            Watch_Face_Preview_TwoDigits.Time.Minutes.Tens = (int)Watch_Face_Preview_Set.Time.Minutes / 10;
            Watch_Face_Preview_TwoDigits.Time.Minutes.Ones = Watch_Face_Preview_Set.Time.Minutes -
                Watch_Face_Preview_TwoDigits.Time.Minutes.Tens * 10;
            Watch_Face_Preview_TwoDigits.Time.Seconds.Tens = (int)Watch_Face_Preview_Set.Time.Seconds / 10;
            Watch_Face_Preview_TwoDigits.Time.Seconds.Ones = Watch_Face_Preview_Set.Time.Seconds -
                Watch_Face_Preview_TwoDigits.Time.Seconds.Tens * 10;

            int hour = Watch_Face_Preview_Set.Time.Hours;
            if (Watch_Face_Preview_Set.Time.Hours >= 12)
            {
                hour = hour - 12;
                Watch_Face_Preview_TwoDigits.TimePm.Pm = true;
            }
            else
            {
                Watch_Face_Preview_TwoDigits.TimePm.Pm = false;
            }
            if (hour == 0) hour = 12;
            Watch_Face_Preview_TwoDigits.TimePm.Hours.Tens = hour / 10;
            Watch_Face_Preview_TwoDigits.TimePm.Hours.Ones = hour - (int)Watch_Face_Preview_TwoDigits.TimePm.Hours.Tens * 10;
            Watch_Face_Preview_TwoDigits.TimePm.Minutes.Tens = (int)Watch_Face_Preview_Set.Time.Minutes / 10;
            Watch_Face_Preview_TwoDigits.TimePm.Minutes.Ones = (int)Watch_Face_Preview_Set.Time.Minutes -
                (int)Watch_Face_Preview_TwoDigits.TimePm.Minutes.Tens * 10;
            Watch_Face_Preview_TwoDigits.TimePm.Seconds.Tens = (int)Watch_Face_Preview_Set.Time.Seconds / 10;
            Watch_Face_Preview_TwoDigits.TimePm.Seconds.Ones = (int)Watch_Face_Preview_Set.Time.Seconds -
                Watch_Face_Preview_TwoDigits.TimePm.Seconds.Tens * 10;
        }

        // меняем цвет текста и рамки для groupBox
        private void groupBox1_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.Black, Color.DarkGray);
        }
        private void DrawGroupBox(GroupBox box, Graphics g, Color textColor, Color borderColor)
        {
            if (box != null)
            {
                Brush textBrush = new SolidBrush(textColor);
                Brush borderBrush = new SolidBrush(borderColor);
                Pen borderPen = new Pen(borderBrush);
                SizeF strSize = g.MeasureString(box.Text, box.Font);
                Rectangle rect = new Rectangle(box.ClientRectangle.X,
                                               box.ClientRectangle.Y + (int)(strSize.Height / 2),
                                               box.ClientRectangle.Width - 1,
                                               box.ClientRectangle.Height - (int)(strSize.Height / 2) - 5);

                // Clear text and border
                g.Clear(this.BackColor);

                // Draw text
                g.DrawString(box.Text, box.Font, textBrush, box.Padding.Left, 0);

                // Drawing Border
                //Left
                g.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height));
                //Right
                g.DrawLine(borderPen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                //Bottom
                g.DrawLine(borderPen, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                //Top1
                g.DrawLine(borderPen, new Point(rect.X, rect.Y), new Point(rect.X + box.Padding.Left, rect.Y));
                //Top2
                g.DrawLine(borderPen, new Point(rect.X + box.Padding.Left + (int)(strSize.Width), rect.Y), new Point(rect.X + rect.Width, rect.Y));
            }
        }
        

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)(sender);
            
            PreviewImage();
            JSON_write();

            if (comboBox.Name == "comboBox_Preview_image")
            {

                if (comboBox.SelectedIndex >= 0)
                {
                    if (FileName == null || FullFileDir == null) return;
                    button_RefreshPreview.Visible = true;
                    button_CreatePreview.Visible = false;
                }
                else
                {
                    button_RefreshPreview.Visible = false;
                    if (FileName != null && FullFileDir != null)
                    {
                        button_CreatePreview.Visible = true;
                    }
                    else
                    {
                        button_CreatePreview.Visible = false;
                    }
                }
            }
        }

        private void checkBox_Click(object sender, EventArgs e)
        {
            PreviewImage();
            JSON_write();
        }

        private void checkBox_ShowSettings_Click(object sender, EventArgs e)
        {
            PreviewImage();
        }

        private void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            PreviewImage();
            JSON_write();
        }
        

#region сворачиваем и разварачиваем панели с предустановками
        private void button_Set1_Click(object sender, EventArgs e)
        {
            panel_SetWeather.Height = 1;
            panel_Set1.Height = (int)(125 * currentDPI);
            panel_Set2.Height = 1;
            panel_Set3.Height = 1;
            panel_Set4.Height = 1;
            panel_Set5.Height = 1;
            panel_Set6.Height = 1;
            panel_Set7.Height = 1;
            panel_Set8.Height = 1;
            panel_Set9.Height = 1;
            panel_Set10.Height = 1;
            SetPreferences1();
            PreviewImage();
        }

        private void button_Set2_Click(object sender, EventArgs e)
        {
            panel_SetWeather.Height = 1;
            panel_Set1.Height = 1;
            panel_Set2.Height = (int)(125 * currentDPI);
            panel_Set3.Height = 1;
            panel_Set4.Height = 1;
            panel_Set5.Height = 1;
            panel_Set6.Height = 1;
            panel_Set7.Height = 1;
            panel_Set8.Height = 1;
            panel_Set9.Height = 1;
            SetPreferences2();
            PreviewImage();
        }
        private void button_Set3_Click(object sender, EventArgs e)
        {
            panel_SetWeather.Height = 1;
            panel_Set1.Height = 1;
            panel_Set2.Height = 1;
            panel_Set3.Height = (int)(125 * currentDPI);
            panel_Set4.Height = 1;
            panel_Set5.Height = 1;
            panel_Set6.Height = 1;
            panel_Set7.Height = 1;
            panel_Set8.Height = 1;
            panel_Set9.Height = 1;
            SetPreferences3();
            PreviewImage();
        }
        private void button_Set4_Click(object sender, EventArgs e)
        {
            panel_SetWeather.Height = 1;
            panel_Set1.Height = 1;
            panel_Set2.Height = 1;
            panel_Set3.Height = 1;
            panel_Set4.Height = (int)(125 * currentDPI);
            panel_Set5.Height = 1;
            panel_Set6.Height = 1;
            panel_Set7.Height = 1;
            panel_Set8.Height = 1;
            panel_Set9.Height = 1;
            panel_Set10.Height = 1;
            SetPreferences4();
            PreviewImage();
        }
        private void button_Set5_Click(object sender, EventArgs e)
        {
            panel_SetWeather.Height = 1;
            panel_Set1.Height = 1;
            panel_Set2.Height = 1;
            panel_Set3.Height = 1;
            panel_Set4.Height = 1;
            panel_Set5.Height = (int)(125 * currentDPI);
            panel_Set6.Height = 1;
            panel_Set7.Height = 1;
            panel_Set8.Height = 1;
            panel_Set9.Height = 1;
            panel_Set10.Height = 1;
            SetPreferences5();
            PreviewImage();
        }
        private void button_Set6_Click(object sender, EventArgs e)
        {
            panel_SetWeather.Height = 1;
            panel_Set1.Height = 1;
            panel_Set2.Height = 1;
            panel_Set3.Height = 1;
            panel_Set4.Height = 1;
            panel_Set5.Height = 1;
            panel_Set6.Height = (int)(125 * currentDPI);
            panel_Set7.Height = 1;
            panel_Set8.Height = 1;
            panel_Set9.Height = 1;
            panel_Set10.Height = 1;
            SetPreferences6();
            PreviewImage();
        }
        private void button_Set7_Click(object sender, EventArgs e)
        {
            panel_SetWeather.Height = 1;
            panel_Set1.Height = 1;
            panel_Set2.Height = 1;
            panel_Set3.Height = 1;
            panel_Set4.Height = 1;
            panel_Set5.Height = 1;
            panel_Set6.Height = 1;
            panel_Set7.Height = (int)(125 * currentDPI);
            panel_Set8.Height = 1;
            panel_Set9.Height = 1;
            panel_Set10.Height = 1;
            SetPreferences7();
            PreviewImage();
        }
        private void button_Set8_Click(object sender, EventArgs e)
        {
            panel_SetWeather.Height = 1;
            panel_Set1.Height = 1;
            panel_Set2.Height = 1;
            panel_Set3.Height = 1;
            panel_Set4.Height = 1;
            panel_Set5.Height = 1;
            panel_Set6.Height = 1;
            panel_Set7.Height = 1;
            panel_Set8.Height = (int)(125 * currentDPI);
            panel_Set9.Height = 1;
            panel_Set10.Height = 1;
            SetPreferences8();
            PreviewImage();
        }
        private void button_Set9_Click(object sender, EventArgs e)
        {
            panel_SetWeather.Height = 1;
            panel_Set1.Height = 1;
            panel_Set2.Height = 1;
            panel_Set3.Height = 1;
            panel_Set4.Height = 1;
            panel_Set5.Height = 1;
            panel_Set6.Height = 1;
            panel_Set7.Height = 1;
            panel_Set8.Height = 1;
            panel_Set9.Height = (int)(125 * currentDPI);
            panel_Set10.Height = 1;
            SetPreferences9();
            PreviewImage();
        }
        private void button_Set10_Click(object sender, EventArgs e)
        {
            panel_SetWeather.Height = 1;
            panel_Set1.Height = 1;
            panel_Set2.Height = 1;
            panel_Set3.Height = 1;
            panel_Set4.Height = 1;
            panel_Set5.Height = 1;
            panel_Set6.Height = 1;
            panel_Set7.Height = 1;
            panel_Set8.Height = 1;
            panel_Set9.Height = 1;
            panel_Set10.Height = (int)(125 * currentDPI);
            SetPreferences10();
            PreviewImage();
        }

        private void button_SetWeather_Click(object sender, EventArgs e)
        {
            panel_SetWeather.Height = (int)(60 * currentDPI);
            panel_Set1.Height = 1;
            panel_Set2.Height = 1;
            panel_Set3.Height = 1;
            panel_Set4.Height = 1;
            panel_Set5.Height = 1;
            panel_Set6.Height = 1;
            panel_Set7.Height = 1;
            panel_Set8.Height = 1;
            panel_Set9.Height = 1;
            panel_Set10.Height = 1;
        }
#endregion

#region поменялись предустановки
        private void dateTimePicker_Time_Set1_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences1();
            PreviewImage();
        }
        private void dateTimePicker_Time_Set2_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences2();
            PreviewImage();
        }
        private void dateTimePicker_Time_Set3_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences3();
            PreviewImage();
        }
        private void dateTimePicker_Time_Set4_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences4();
            PreviewImage();
        }
        private void dateTimePicker_Time_Set5_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences5();
            PreviewImage();
        }
        private void dateTimePicker_Time_Set6_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences6();
            PreviewImage();
        }
        private void dateTimePicker_Time_Set7_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences7();
            PreviewImage();
        }
        private void dateTimePicker_Time_Set8_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences8();
            PreviewImage();
        }
        private void dateTimePicker_Time_Set9_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences9();
            PreviewImage();
        }
        private void dateTimePicker_Time_Set10_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences10();
            PreviewImage();
        }
        //////////////////////////////
        private void numericUpDown_Battery_Set1_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences1();
            PreviewImage();
        }
        private void numericUpDown_Battery_Set2_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences2();
            PreviewImage();
        }
        private void numericUpDown_Battery_Set3_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences3();
            PreviewImage();
        }
        private void numericUpDown_Battery_Set4_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences4();
            PreviewImage();
        }
        private void numericUpDown_Battery_Set5_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences5();
            PreviewImage();
        }
        private void numericUpDown_Battery_Set6_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences6();
            PreviewImage();
        }
        private void numericUpDown_Battery_Set7_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences7();
            PreviewImage();
        }
        private void numericUpDown_Battery_Set8_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences8();
            PreviewImage();
        }
        private void numericUpDown_Battery_Set9_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences9();
            PreviewImage();
        }
        private void numericUpDown_Battery_Set10_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences10();
            PreviewImage();
        }
        //////////////////////////////
        private void check_BoxBluetooth_Set1_CheckedChanged(object sender, EventArgs e)
        {
            SetPreferences1();
            PreviewImage();
        }
        private void check_BoxBluetooth_Set2_CheckedChanged(object sender, EventArgs e)
        {
            SetPreferences2();
            PreviewImage();
        }
        private void check_BoxBluetooth_Set3_CheckedChanged(object sender, EventArgs e)
        {
            SetPreferences3();
            PreviewImage();
        }
        private void check_BoxBluetooth_Set4_CheckedChanged(object sender, EventArgs e)
        {
            SetPreferences4();
            PreviewImage();
        }
        private void check_BoxBluetooth_Set5_CheckedChanged(object sender, EventArgs e)
        {
            SetPreferences5();
            PreviewImage();
        }
        private void check_BoxBluetooth_Set6_CheckedChanged(object sender, EventArgs e)
        {
            SetPreferences6();
            PreviewImage();
        }
        private void check_BoxBluetooth_Set7_CheckedChanged(object sender, EventArgs e)
        {
            SetPreferences7();
            PreviewImage();
        }
        private void check_BoxBluetooth_Set8_CheckedChanged(object sender, EventArgs e)
        {
            SetPreferences8();
            PreviewImage();
        }
        private void check_BoxBluetooth_Set9_CheckedChanged(object sender, EventArgs e)
        {
            SetPreferences9();
            PreviewImage();
        }
        private void check_BoxBluetooth_Set10_CheckedChanged(object sender, EventArgs e)
        {
            SetPreferences10();
            PreviewImage();
        }
#endregion

        private void pictureBox_Preview_DoubleClick(object sender, EventArgs e)
        {
            if ((formPreview == null) || (!formPreview.Visible))
            {
                formPreview = new Form_Preview(currentDPI);
                formPreview.Show(this);
                //formPreview.Show();

                switch (Program_Settings.Scale)
                {
                    case 0.5f:
                        formPreview.radioButton_small.Checked = true;
                        break;
                    case 1.5f:
                        formPreview.radioButton_large.Checked = true;
                        break;
                    case 2.0f:
                        formPreview.radioButton_xlarge.Checked = true;
                        break;
                    case 2.5f:
                        formPreview.radioButton_xxlarge.Checked = true;
                        break;
                    default:
                        formPreview.radioButton_normal.Checked = true;
                        break;

                }

                formPreview.pictureBox_Preview.Resize += (object senderResize, EventArgs eResize) =>
                {
                    if (Form_Preview.Model_Wath.model_gtr47 != radioButton_GTR2.Checked)
                        Form_Preview.Model_Wath.model_gtr47 = radioButton_GTR2.Checked;
                    if (Form_Preview.Model_Wath.model_gts != radioButton_GTS2.Checked)
                        Form_Preview.Model_Wath.model_gts = radioButton_GTS2.Checked;
                    //Graphics gPanelPreviewResize = formPreview.panel_Preview.CreateGraphics();
                    //gPanelPreviewResize.Clear(panel_Preview.BackColor);
                    //formPreview.radioButton_CheckedChanged(sender, e);
                    float scalePreviewResize = 1.0f;
                    if (formPreview.radioButton_small.Checked) scalePreviewResize = 0.5f;
                    if (formPreview.radioButton_large.Checked) scalePreviewResize = 1.5f;
                    if (formPreview.radioButton_xlarge.Checked) scalePreviewResize = 2.0f;
                    if (formPreview.radioButton_xxlarge.Checked) scalePreviewResize = 2.5f;

                    Program_Settings.Scale = scalePreviewResize;
                    string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
                    {
                        //DefaultValueHandling = DefaultValueHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);
                    
#region BackgroundImage 
                    Bitmap bitmapPreviewResize = new Bitmap(Convert.ToInt32(454), Convert.ToInt32(454), PixelFormat.Format32bppArgb);
                    if (radioButton_GTS2.Checked)
                    {
                        bitmapPreviewResize = new Bitmap(Convert.ToInt32(348), Convert.ToInt32(442), PixelFormat.Format32bppArgb);
                    }
                    Graphics gPanelPreviewResize = Graphics.FromImage(bitmapPreviewResize);
                    #endregion

                    int link_aod = radioButton_ScreenNormal.Checked ? 0 : 1;
                    PreviewToBitmap(gPanelPreviewResize, 1, checkBox_crop.Checked,
                        checkBox_WebW.Checked, checkBox_WebB.Checked, checkBox_border.Checked, 
                        checkBox_Show_Shortcuts.Checked, checkBox_Shortcuts_Area.Checked, checkBox_Shortcuts_Border.Checked, true,
                        checkBox_CircleScaleImage.Checked, checkBox_center_marker.Checked, link_aod);
                    formPreview.pictureBox_Preview.BackgroundImage = bitmapPreviewResize;
                    gPanelPreviewResize.Dispose();
                };

                //formPreview.panel_Preview.Paint += (object senderPaint, PaintEventArgs ePaint) =>
                //{
                //    //Form_Preview.Model_GTR47.model_gtr47 = radioButton_47.Checked;
                //    //Graphics gPanelPreviewPaint = formPreview.panel_Preview.CreateGraphics();
                //    //gPanelPreviewPaint.Clear(panel_Preview.BackColor);
                //    //formPreview.radioButton_CheckedChanged(sender, e);
                //    //float scalePreviewPaint = 1.0f;
                //    //if (formPreview.radioButton_small.Checked) scalePreviewPaint = 0.5f;
                //    //if (formPreview.radioButton_large.Checked) scalePreviewPaint = 1.5f;
                //    //if (formPreview.radioButton_xlarge.Checked) scalePreviewPaint = 2.0f;
                //    //if (formPreview.radioButton_xxlarge.Checked) scalePreviewPaint = 2.5f;
                //    //PreviewToBitmap(gPanelPreviewPaint, scalePreviewPaint, radioButton_47.Checked, checkBox_WebW.Checked, checkBox_WebB.Checked);
                //    //gPanelPreviewPaint.Dispose();
                //    timer2.Enabled = false;
                //    timer2.Enabled = true;
                //};

                formPreview.FormClosing += (object senderClosing, FormClosingEventArgs eClosing) =>
                {
                    button_PreviewBig.Enabled = true;
                };

                formPreview.KeyDown += (object senderKeyDown, KeyEventArgs eKeyDown) =>
                {
                    this.Form1_KeyDown(senderKeyDown, eKeyDown);
                };
            }

            if (Form_Preview.Model_Wath.model_gtr47 != radioButton_GTR2.Checked)
                Form_Preview.Model_Wath.model_gtr47 = radioButton_GTR2.Checked;
            if (Form_Preview.Model_Wath.model_gts != radioButton_GTS2.Checked)
                Form_Preview.Model_Wath.model_gts = radioButton_GTS2.Checked;
            formPreview.radioButton_CheckedChanged(sender, e);
            float scale = 1.0f;

#region BackgroundImage 
            Bitmap bitmap = new Bitmap(Convert.ToInt32(454), Convert.ToInt32(454), PixelFormat.Format32bppArgb);
            if (radioButton_GTS2.Checked)
            {
                bitmap = new Bitmap(Convert.ToInt32(348), Convert.ToInt32(442), PixelFormat.Format32bppArgb);
            }
            Graphics gPanel = Graphics.FromImage(bitmap);
            #endregion

            int link = radioButton_ScreenNormal.Checked ? 0 : 1;
            PreviewToBitmap(gPanel, scale, checkBox_crop.Checked, checkBox_WebW.Checked, checkBox_WebB.Checked, 
                checkBox_border.Checked, checkBox_Show_Shortcuts.Checked, checkBox_Shortcuts_Area.Checked, 
                checkBox_Shortcuts_Border.Checked, true, checkBox_CircleScaleImage.Checked,
                checkBox_center_marker.Checked, link);
            formPreview.pictureBox_Preview.BackgroundImage = bitmap;
            gPanel.Dispose();

            button_PreviewBig.Enabled = false;
        }

        // считываем параметры из JsonPreview
        private void button_JsonPreview_Read_Click(object sender, EventArgs e)
        {
            //string subPath = Application.StartupPath + @"\Watch_face\";
            //if (!Directory.Exists(subPath)) Directory.CreateDirectory(subPath);

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = FullFileDir;
            openFileDialog.Filter = Properties.FormStrings.FilterJson;
            openFileDialog.FileName = "PreviewStates.json";
            //openFileDialog.Filter = "Json files (*.json) | *.json";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = false;
            openFileDialog.Title = Properties.FormStrings.Dialog_Title_PreviewStates;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fullfilename = openFileDialog.FileName;
                JsonPreview_Read(fullfilename);
            }
        }

        // считываем параметры из JsonPreview
        private void JsonPreview_Read (string fullfilename)
        {
            string text = File.ReadAllText(fullfilename);

            PreviewView = false;
            PREWIEV_STATES_Json ps = new PREWIEV_STATES_Json();
            try
            {
                var objson = JsonConvert.DeserializeObject<object[]>(text);

                int count = objson.Count();

                string JSON_Text = JsonConvert.SerializeObject(objson, Formatting.Indented, new JsonSerializerSettings
                {
                    //DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
                //richTextBox_JsonText.Text = JSON_Text;

                if (count == 0) return;
                if (count > 13) count = 13;
                for (int i = 0; i < count; i++)
                {
                    ps = JsonConvert.DeserializeObject<PREWIEV_STATES_Json>(objson[i].ToString(), new JsonSerializerSettings
                    {
                        DefaultValueHandling = DefaultValueHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });

                    int year = ps.Time.Year;
                    int month = ps.Time.Month;
                    int day = ps.Time.Day;
                    int hour = ps.Time.Hour;
                    int min = ps.Time.Minute;
                    int sec = ps.Time.Second;
                    int battery = ps.BatteryLevel;
                    int calories = ps.Calories;
                    int pulse = ps.Pulse;
                    int distance = ps.Distance;
                    int steps = ps.Steps;
                    int goal = ps.Goal;
                    bool bluetooth = ps.Bluetooth;
                    bool alarm = ps.Alarm;
                    bool unlocked = ps.Unlocked;
                    bool dnd = ps.DoNotDisturb;
                    switch (i)
                    {
                        case 0:
                            dateTimePicker_Date_Set1.Value = new DateTime(year, month, day, hour, min, sec);
                            dateTimePicker_Time_Set1.Value = new DateTime(year, month, day, hour, min, sec);
                            numericUpDown_Battery_Set1.Value = battery;
                            numericUpDown_Calories_Set1.Value = calories;
                            numericUpDown_Pulse_Set1.Value = pulse;
                            numericUpDown_Distance_Set1.Value = distance;
                            numericUpDown_Steps_Set1.Value = steps;
                            numericUpDown_Goal_Set1.Value = goal;
                            check_BoxBluetooth_Set1.Checked = bluetooth;
                            checkBox_Alarm_Set1.Checked = alarm;
                            checkBox_Lock_Set1.Checked = unlocked;
                            checkBox_DoNotDisturb_Set1.Checked = dnd;
                            button_Set1.PerformClick();
                            break;
                        case 1:
                            dateTimePicker_Date_Set2.Value = new DateTime(year, month, day, hour, min, sec);
                            dateTimePicker_Time_Set2.Value = new DateTime(year, month, day, hour, min, sec);
                            numericUpDown_Battery_Set2.Value = battery;
                            numericUpDown_Calories_Set2.Value = calories;
                            numericUpDown_Pulse_Set2.Value = pulse;
                            numericUpDown_Distance_Set2.Value = distance;
                            numericUpDown_Steps_Set2.Value = steps;
                            numericUpDown_Goal_Set2.Value = goal;
                            check_BoxBluetooth_Set2.Checked = bluetooth;
                            checkBox_Alarm_Set2.Checked = alarm;
                            checkBox_Lock_Set2.Checked = unlocked;
                            checkBox_DoNotDisturb_Set2.Checked = dnd;
                            button_Set2.PerformClick();
                            break;
                        case 2:
                            dateTimePicker_Date_Set3.Value = new DateTime(year, month, day, hour, min, sec);
                            dateTimePicker_Time_Set3.Value = new DateTime(year, month, day, hour, min, sec);
                            numericUpDown_Battery_Set3.Value = battery;
                            numericUpDown_Calories_Set3.Value = calories;
                            numericUpDown_Pulse_Set3.Value = pulse;
                            numericUpDown_Distance_Set3.Value = distance;
                            numericUpDown_Steps_Set3.Value = steps;
                            numericUpDown_Goal_Set3.Value = goal;
                            check_BoxBluetooth_Set3.Checked = bluetooth;
                            checkBox_Alarm_Set3.Checked = alarm;
                            checkBox_Lock_Set3.Checked = unlocked;
                            checkBox_DoNotDisturb_Set3.Checked = dnd;
                            button_Set3.PerformClick();
                            break;
                        case 3:
                            dateTimePicker_Date_Set4.Value = new DateTime(year, month, day, hour, min, sec);
                            dateTimePicker_Time_Set4.Value = new DateTime(year, month, day, hour, min, sec);
                            numericUpDown_Battery_Set4.Value = battery;
                            numericUpDown_Calories_Set4.Value = calories;
                            numericUpDown_Pulse_Set4.Value = pulse;
                            numericUpDown_Distance_Set4.Value = distance;
                            numericUpDown_Steps_Set4.Value = steps;
                            numericUpDown_Goal_Set4.Value = goal;
                            check_BoxBluetooth_Set4.Checked = bluetooth;
                            checkBox_Alarm_Set4.Checked = alarm;
                            checkBox_Lock_Set4.Checked = unlocked;
                            checkBox_DoNotDisturb_Set4.Checked = dnd;
                            button_Set4.PerformClick();
                            break;
                        case 4:
                            dateTimePicker_Date_Set5.Value = new DateTime(year, month, day, hour, min, sec);
                            dateTimePicker_Time_Set5.Value = new DateTime(year, month, day, hour, min, sec);
                            numericUpDown_Battery_Set5.Value = battery;
                            numericUpDown_Calories_Set5.Value = calories;
                            numericUpDown_Pulse_Set5.Value = pulse;
                            numericUpDown_Distance_Set5.Value = distance;
                            numericUpDown_Steps_Set5.Value = steps;
                            numericUpDown_Goal_Set5.Value = goal;
                            check_BoxBluetooth_Set5.Checked = bluetooth;
                            checkBox_Alarm_Set5.Checked = alarm;
                            checkBox_Lock_Set5.Checked = unlocked;
                            checkBox_DoNotDisturb_Set5.Checked = dnd;
                            button_Set5.PerformClick();
                            break;
                        case 5:
                            dateTimePicker_Date_Set6.Value = new DateTime(year, month, day, hour, min, sec);
                            dateTimePicker_Time_Set6.Value = new DateTime(year, month, day, hour, min, sec);
                            numericUpDown_Battery_Set6.Value = battery;
                            numericUpDown_Calories_Set6.Value = calories;
                            numericUpDown_Pulse_Set6.Value = pulse;
                            numericUpDown_Distance_Set6.Value = distance;
                            numericUpDown_Steps_Set6.Value = steps;
                            numericUpDown_Goal_Set6.Value = goal;
                            check_BoxBluetooth_Set6.Checked = bluetooth;
                            checkBox_Alarm_Set6.Checked = alarm;
                            checkBox_Lock_Set6.Checked = unlocked;
                            checkBox_DoNotDisturb_Set6.Checked = dnd;
                            button_Set6.PerformClick();
                            break;
                        case 6:
                            dateTimePicker_Date_Set7.Value = new DateTime(year, month, day, hour, min, sec);
                            dateTimePicker_Time_Set7.Value = new DateTime(year, month, day, hour, min, sec);
                            numericUpDown_Battery_Set7.Value = battery;
                            numericUpDown_Calories_Set7.Value = calories;
                            numericUpDown_Pulse_Set7.Value = pulse;
                            numericUpDown_Distance_Set7.Value = distance;
                            numericUpDown_Steps_Set7.Value = steps;
                            numericUpDown_Goal_Set7.Value = goal;
                            check_BoxBluetooth_Set7.Checked = bluetooth;
                            checkBox_Alarm_Set7.Checked = alarm;
                            checkBox_Lock_Set7.Checked = unlocked;
                            checkBox_DoNotDisturb_Set7.Checked = dnd;
                            button_Set7.PerformClick();
                            break;
                        case 7:
                            dateTimePicker_Date_Set8.Value = new DateTime(year, month, day, hour, min, sec);
                            dateTimePicker_Time_Set8.Value = new DateTime(year, month, day, hour, min, sec);
                            numericUpDown_Battery_Set8.Value = battery;
                            numericUpDown_Calories_Set8.Value = calories;
                            numericUpDown_Pulse_Set8.Value = pulse;
                            numericUpDown_Distance_Set8.Value = distance;
                            numericUpDown_Steps_Set8.Value = steps;
                            numericUpDown_Goal_Set8.Value = goal;
                            check_BoxBluetooth_Set8.Checked = bluetooth;
                            checkBox_Alarm_Set8.Checked = alarm;
                            checkBox_Lock_Set8.Checked = unlocked;
                            checkBox_DoNotDisturb_Set8.Checked = dnd;
                            button_Set8.PerformClick();
                            break;
                        case 8:
                            dateTimePicker_Date_Set9.Value = new DateTime(year, month, day, hour, min, sec);
                            dateTimePicker_Time_Set9.Value = new DateTime(year, month, day, hour, min, sec);
                            numericUpDown_Battery_Set9.Value = battery;
                            numericUpDown_Calories_Set9.Value = calories;
                            numericUpDown_Pulse_Set9.Value = pulse;
                            numericUpDown_Distance_Set9.Value = distance;
                            numericUpDown_Steps_Set9.Value = steps;
                            numericUpDown_Goal_Set9.Value = goal;
                            check_BoxBluetooth_Set9.Checked = bluetooth;
                            checkBox_Alarm_Set9.Checked = alarm;
                            checkBox_Lock_Set9.Checked = unlocked;
                            checkBox_DoNotDisturb_Set9.Checked = dnd;
                            button_Set9.PerformClick();
                            break;
                        case 9:
                            dateTimePicker_Date_Set10.Value = new DateTime(year, month, day, hour, min, sec);
                            dateTimePicker_Time_Set10.Value = new DateTime(year, month, day, hour, min, sec);
                            numericUpDown_Battery_Set10.Value = battery;
                            numericUpDown_Calories_Set10.Value = calories;
                            numericUpDown_Pulse_Set10.Value = pulse;
                            numericUpDown_Distance_Set10.Value = distance;
                            numericUpDown_Steps_Set10.Value = steps;
                            numericUpDown_Goal_Set10.Value = goal;
                            check_BoxBluetooth_Set10.Checked = bluetooth;
                            checkBox_Alarm_Set10.Checked = alarm;
                            checkBox_Lock_Set10.Checked = unlocked;
                            checkBox_DoNotDisturb_Set10.Checked = dnd;
                            button_Set10.PerformClick();
                            break;
                    }
                }

            }
            catch (Exception)
            {
                MessageBox.Show(Properties.FormStrings.Message_JsonReadError_Text, Properties.FormStrings.Message_Error_Caption, 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            //richTextBox_JsonText.Text = JsonConvert.SerializeObject(objson, Formatting.Indented, new JsonSerializerSettings
            //{
            //    //DefaultValueHandling = DefaultValueHandling.Ignore,
            //    NullValueHandling = NullValueHandling.Ignore
            //});
            //PreviewView = false;
            PreviewView = true;
            PreviewImage();
        }

        // записываем параметры в JsonPreview
        private void button_JsonPreview_Write_Click(object sender, EventArgs e)
        {
            
            object[] objson = new object[] { };
            int count = 0;
            for (int i = 0; i < 13; i++)
            {
                PREWIEV_STATES_Json ps = new PREWIEV_STATES_Json();
                ps.Time = new TimePreview();
                switch (i)
                {
                    case 0:
                        ps.Time.Year = dateTimePicker_Date_Set1.Value.Year;
                        ps.Time.Month = dateTimePicker_Date_Set1.Value.Month;
                        ps.Time.Day = dateTimePicker_Date_Set1.Value.Day;
                        ps.Time.Hour = dateTimePicker_Time_Set1.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Time_Set1.Value.Minute;
                        ps.Time.Second = dateTimePicker_Time_Set1.Value.Second;
                        ps.BatteryLevel = (int)numericUpDown_Battery_Set1.Value;
                        ps.Calories = (int)numericUpDown_Calories_Set1.Value;
                        ps.Pulse = (int)numericUpDown_Pulse_Set1.Value;
                        ps.Distance = (int)numericUpDown_Distance_Set1.Value;
                        ps.Steps = (int)numericUpDown_Steps_Set1.Value;
                        ps.Goal = (int)numericUpDown_Goal_Set1.Value;
                        ps.Bluetooth = check_BoxBluetooth_Set1.Checked;
                        ps.Alarm = checkBox_Alarm_Set1.Checked;
                        ps.Unlocked = checkBox_Lock_Set1.Checked;
                        ps.DoNotDisturb = checkBox_DoNotDisturb_Set1.Checked;

                        if (numericUpDown_Calories_Set1.Value != 1234)
                        {
                            Array.Resize(ref objson, objson.Length + 1); 
                            objson[count] = ps;
                            count++;
                        }
                        break;
                    case 1:
                        ps.Time.Year = dateTimePicker_Date_Set2.Value.Year;
                        ps.Time.Month = dateTimePicker_Date_Set2.Value.Month;
                        ps.Time.Day = dateTimePicker_Date_Set2.Value.Day;
                        ps.Time.Hour = dateTimePicker_Time_Set2.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Time_Set2.Value.Minute;
                        ps.Time.Second = dateTimePicker_Time_Set2.Value.Second;
                        ps.BatteryLevel = (int)numericUpDown_Battery_Set2.Value;
                        ps.Calories = (int)numericUpDown_Calories_Set2.Value;
                        ps.Pulse = (int)numericUpDown_Pulse_Set2.Value;
                        ps.Distance = (int)numericUpDown_Distance_Set2.Value;
                        ps.Steps = (int)numericUpDown_Steps_Set2.Value;
                        ps.Goal = (int)numericUpDown_Goal_Set2.Value;
                        ps.Bluetooth = check_BoxBluetooth_Set2.Checked;
                        ps.Alarm = checkBox_Alarm_Set2.Checked;
                        ps.Unlocked = checkBox_Lock_Set2.Checked;
                        ps.DoNotDisturb = checkBox_DoNotDisturb_Set2.Checked;

                        if (numericUpDown_Calories_Set2.Value != 1234)
                        {
                            Array.Resize(ref objson, objson.Length + 1);
                            objson[count] = ps;
                            count++;
                        }
                        break;
                    case 2:
                        ps.Time.Year = dateTimePicker_Date_Set3.Value.Year;
                        ps.Time.Month = dateTimePicker_Date_Set3.Value.Month;
                        ps.Time.Day = dateTimePicker_Date_Set3.Value.Day;
                        ps.Time.Hour = dateTimePicker_Time_Set3.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Time_Set3.Value.Minute;
                        ps.Time.Second = dateTimePicker_Time_Set3.Value.Second;
                        ps.BatteryLevel = (int)numericUpDown_Battery_Set3.Value;
                        ps.Calories = (int)numericUpDown_Calories_Set3.Value;
                        ps.Pulse = (int)numericUpDown_Pulse_Set3.Value;
                        ps.Distance = (int)numericUpDown_Distance_Set3.Value;
                        ps.Steps = (int)numericUpDown_Steps_Set3.Value;
                        ps.Goal = (int)numericUpDown_Goal_Set3.Value;
                        ps.Bluetooth = check_BoxBluetooth_Set3.Checked;
                        ps.Alarm = checkBox_Alarm_Set3.Checked;
                        ps.Unlocked = checkBox_Lock_Set3.Checked;
                        ps.DoNotDisturb = checkBox_DoNotDisturb_Set3.Checked;

                        if (numericUpDown_Calories_Set3.Value != 1234)
                        {
                            Array.Resize(ref objson, objson.Length + 1);
                            objson[count] = ps;
                            count++;
                        }
                        break;
                    case 3:
                        ps.Time.Year = dateTimePicker_Date_Set4.Value.Year;
                        ps.Time.Month = dateTimePicker_Date_Set4.Value.Month;
                        ps.Time.Day = dateTimePicker_Date_Set4.Value.Day;
                        ps.Time.Hour = dateTimePicker_Time_Set4.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Time_Set4.Value.Minute;
                        ps.Time.Second = dateTimePicker_Time_Set4.Value.Second;
                        ps.BatteryLevel = (int)numericUpDown_Battery_Set4.Value;
                        ps.Calories = (int)numericUpDown_Calories_Set4.Value;
                        ps.Pulse = (int)numericUpDown_Pulse_Set4.Value;
                        ps.Distance = (int)numericUpDown_Distance_Set4.Value;
                        ps.Steps = (int)numericUpDown_Steps_Set4.Value;
                        ps.Goal = (int)numericUpDown_Goal_Set4.Value;
                        ps.Bluetooth = check_BoxBluetooth_Set4.Checked;
                        ps.Alarm = checkBox_Alarm_Set4.Checked;
                        ps.Unlocked = checkBox_Lock_Set4.Checked;
                        ps.DoNotDisturb = checkBox_DoNotDisturb_Set4.Checked;

                        if (numericUpDown_Calories_Set4.Value != 1234)
                        {
                            Array.Resize(ref objson, objson.Length + 1);
                            objson[count] = ps;
                            count++;
                        }
                        break;
                    case 4:
                        ps.Time.Year = dateTimePicker_Date_Set5.Value.Year;
                        ps.Time.Month = dateTimePicker_Date_Set5.Value.Month;
                        ps.Time.Day = dateTimePicker_Date_Set5.Value.Day;
                        ps.Time.Hour = dateTimePicker_Time_Set5.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Time_Set5.Value.Minute;
                        ps.Time.Second = dateTimePicker_Time_Set5.Value.Second;
                        ps.BatteryLevel = (int)numericUpDown_Battery_Set5.Value;
                        ps.Calories = (int)numericUpDown_Calories_Set5.Value;
                        ps.Pulse = (int)numericUpDown_Pulse_Set5.Value;
                        ps.Distance = (int)numericUpDown_Distance_Set5.Value;
                        ps.Steps = (int)numericUpDown_Steps_Set5.Value;
                        ps.Goal = (int)numericUpDown_Goal_Set5.Value;
                        ps.Bluetooth = check_BoxBluetooth_Set5.Checked;
                        ps.Alarm = checkBox_Alarm_Set5.Checked;
                        ps.Unlocked = checkBox_Lock_Set5.Checked;
                        ps.DoNotDisturb = checkBox_DoNotDisturb_Set5.Checked;

                        if (numericUpDown_Calories_Set5.Value != 1234)
                        {
                            Array.Resize(ref objson, objson.Length + 1);
                            objson[count] = ps;
                            count++;
                        }
                        break;
                    case 5:
                        ps.Time.Year = dateTimePicker_Date_Set6.Value.Year;
                        ps.Time.Month = dateTimePicker_Date_Set6.Value.Month;
                        ps.Time.Day = dateTimePicker_Date_Set6.Value.Day;
                        ps.Time.Hour = dateTimePicker_Time_Set6.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Time_Set6.Value.Minute;
                        ps.Time.Second = dateTimePicker_Time_Set6.Value.Second;
                        ps.BatteryLevel = (int)numericUpDown_Battery_Set6.Value;
                        ps.Calories = (int)numericUpDown_Calories_Set6.Value;
                        ps.Pulse = (int)numericUpDown_Pulse_Set6.Value;
                        ps.Distance = (int)numericUpDown_Distance_Set6.Value;
                        ps.Steps = (int)numericUpDown_Steps_Set6.Value;
                        ps.Goal = (int)numericUpDown_Goal_Set6.Value;
                        ps.Bluetooth = check_BoxBluetooth_Set6.Checked;
                        ps.Alarm = checkBox_Alarm_Set6.Checked;
                        ps.Unlocked = checkBox_Lock_Set6.Checked;
                        ps.DoNotDisturb = checkBox_DoNotDisturb_Set6.Checked;

                        if (numericUpDown_Calories_Set6.Value != 1234)
                        {
                            Array.Resize(ref objson, objson.Length + 1);
                            objson[count] = ps;
                            count++;
                        }
                        break;
                    case 6:
                        ps.Time.Year = dateTimePicker_Date_Set7.Value.Year;
                        ps.Time.Month = dateTimePicker_Date_Set7.Value.Month;
                        ps.Time.Day = dateTimePicker_Date_Set7.Value.Day;
                        ps.Time.Hour = dateTimePicker_Time_Set7.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Time_Set7.Value.Minute;
                        ps.Time.Second = dateTimePicker_Time_Set7.Value.Second;
                        ps.BatteryLevel = (int)numericUpDown_Battery_Set7.Value;
                        ps.Calories = (int)numericUpDown_Calories_Set7.Value;
                        ps.Pulse = (int)numericUpDown_Pulse_Set7.Value;
                        ps.Distance = (int)numericUpDown_Distance_Set7.Value;
                        ps.Steps = (int)numericUpDown_Steps_Set7.Value;
                        ps.Goal = (int)numericUpDown_Goal_Set7.Value;
                        ps.Bluetooth = check_BoxBluetooth_Set7.Checked;
                        ps.Alarm = checkBox_Alarm_Set7.Checked;
                        ps.Unlocked = checkBox_Lock_Set7.Checked;
                        ps.DoNotDisturb = checkBox_DoNotDisturb_Set7.Checked;

                        if (numericUpDown_Calories_Set7.Value != 1234)
                        {
                            Array.Resize(ref objson, objson.Length + 1);
                            objson[count] = ps;
                            count++;
                        }
                        break;
                    case 7:
                        ps.Time.Year = dateTimePicker_Date_Set8.Value.Year;
                        ps.Time.Month = dateTimePicker_Date_Set8.Value.Month;
                        ps.Time.Day = dateTimePicker_Date_Set8.Value.Day;
                        ps.Time.Hour = dateTimePicker_Time_Set8.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Time_Set8.Value.Minute;
                        ps.Time.Second = dateTimePicker_Time_Set8.Value.Second;
                        ps.BatteryLevel = (int)numericUpDown_Battery_Set8.Value;
                        ps.Calories = (int)numericUpDown_Calories_Set8.Value;
                        ps.Pulse = (int)numericUpDown_Pulse_Set8.Value;
                        ps.Distance = (int)numericUpDown_Distance_Set8.Value;
                        ps.Steps = (int)numericUpDown_Steps_Set8.Value;
                        ps.Goal = (int)numericUpDown_Goal_Set8.Value;
                        ps.Bluetooth = check_BoxBluetooth_Set8.Checked;
                        ps.Alarm = checkBox_Alarm_Set8.Checked;
                        ps.Unlocked = checkBox_Lock_Set8.Checked;
                        ps.DoNotDisturb = checkBox_DoNotDisturb_Set8.Checked;

                        if (numericUpDown_Calories_Set8.Value != 1234)
                        {
                            Array.Resize(ref objson, objson.Length + 1);
                            objson[count] = ps;
                            count++;
                        }
                        break;
                    case 8:
                        ps.Time.Year = dateTimePicker_Date_Set9.Value.Year;
                        ps.Time.Month = dateTimePicker_Date_Set9.Value.Month;
                        ps.Time.Day = dateTimePicker_Date_Set9.Value.Day;
                        ps.Time.Hour = dateTimePicker_Time_Set9.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Time_Set9.Value.Minute;
                        ps.Time.Second = dateTimePicker_Time_Set9.Value.Second;
                        ps.BatteryLevel = (int)numericUpDown_Battery_Set9.Value;
                        ps.Calories = (int)numericUpDown_Calories_Set9.Value;
                        ps.Pulse = (int)numericUpDown_Pulse_Set9.Value;
                        ps.Distance = (int)numericUpDown_Distance_Set9.Value;
                        ps.Steps = (int)numericUpDown_Steps_Set9.Value;
                        ps.Goal = (int)numericUpDown_Goal_Set9.Value;
                        ps.Bluetooth = check_BoxBluetooth_Set9.Checked;
                        ps.Alarm = checkBox_Alarm_Set9.Checked;
                        ps.Unlocked = checkBox_Lock_Set9.Checked;
                        ps.DoNotDisturb = checkBox_DoNotDisturb_Set9.Checked;

                        if (numericUpDown_Calories_Set9.Value != 1234)
                        {
                            Array.Resize(ref objson, objson.Length + 1);
                            objson[count] = ps;
                            count++;
                        }
                        break;
                    case 9:
                        ps.Time.Year = dateTimePicker_Date_Set10.Value.Year;
                        ps.Time.Month = dateTimePicker_Date_Set10.Value.Month;
                        ps.Time.Day = dateTimePicker_Date_Set10.Value.Day;
                        ps.Time.Hour = dateTimePicker_Time_Set10.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Time_Set10.Value.Minute;
                        ps.Time.Second = dateTimePicker_Time_Set10.Value.Second;
                        ps.BatteryLevel = (int)numericUpDown_Battery_Set10.Value;
                        ps.Calories = (int)numericUpDown_Calories_Set10.Value;
                        ps.Pulse = (int)numericUpDown_Pulse_Set10.Value;
                        ps.Distance = (int)numericUpDown_Distance_Set10.Value;
                        ps.Steps = (int)numericUpDown_Steps_Set10.Value;
                        ps.Goal = (int)numericUpDown_Goal_Set10.Value;
                        ps.Bluetooth = check_BoxBluetooth_Set10.Checked;
                        ps.Alarm = checkBox_Alarm_Set10.Checked;
                        ps.Unlocked = checkBox_Lock_Set10.Checked;
                        ps.DoNotDisturb = checkBox_DoNotDisturb_Set10.Checked;

                        if (numericUpDown_Calories_Set10.Value != 1234)
                        {
                            Array.Resize(ref objson, objson.Length + 1);
                            objson[count] = ps;
                            count++;
                        }
                        break;
                }
            }

            string string_json_temp = JsonConvert.SerializeObject(objson, Formatting.None, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            var objsontemp = JsonConvert.DeserializeObject<object[]>(string_json_temp);

            string formatted = JsonConvert.SerializeObject(objsontemp, Formatting.Indented);
            richTextBox_JsonText.Text = formatted;


            if (formatted.Length < 10)
            {
                MessageBox.Show(Properties.FormStrings.Message_SaveOnly1234_Text);
                return;
            }
            //text = text.Replace(@"\", "");
            //text = text.Replace("\"{", "{");
            //text = text.Replace("}\"", "}");
            //text = text.Replace(",", ",\r\n");
            //text = text.Replace(":", ": ");
            //text = text.Replace(": {", ": {\r\n");
            //string formatted = JsonConvert.SerializeObject(text, Formatting.Indented);

            


            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //openFileDialog.InitialDirectory = subPath;
            saveFileDialog.Filter = Properties.FormStrings.FilterJson;
            saveFileDialog.FileName = "PreviewStates.json";
            //openFileDialog.Filter = "Json files (*.json) | *.json";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Title = Properties.FormStrings.Dialog_Title_PreviewStates;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fullfilename = saveFileDialog.FileName;
                richTextBox_JsonText.Text = formatted;
                File.WriteAllText(fullfilename, formatted, Encoding.UTF8);
            }
        }

        // случайные значения ностроек
        private void button_JsonPreview_Random_Click(object sender, EventArgs e)
        {
            PreviewView = false;
            DateTime now = DateTime.Now;
            Random rnd = new Random();
            for (int i = 0; i < 10; i++)
            {
                int year = now.Year;
                int month = rnd.Next(0, 12)+1;
                int day = rnd.Next(0, 28)+1;
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
                switch (i)
                {
                    case 0:
                        dateTimePicker_Date_Set1.Value = new DateTime(year, month, day, hour, min, sec);
                        dateTimePicker_Time_Set1.Value = new DateTime(year, month, day, hour, min, sec);
                        numericUpDown_Battery_Set1.Value = battery;
                        numericUpDown_Calories_Set1.Value = calories;
                        numericUpDown_Pulse_Set1.Value = pulse;
                        numericUpDown_Distance_Set1.Value = distance;
                        numericUpDown_Steps_Set1.Value = steps;
                        numericUpDown_Goal_Set1.Value = goal;
                        check_BoxBluetooth_Set1.Checked = bluetooth;
                        checkBox_Alarm_Set1.Checked = alarm;
                        checkBox_Lock_Set1.Checked = unlocked;
                        checkBox_DoNotDisturb_Set1.Checked = dnd;
                        numericUpDown_PAI_Set1.Value = pai;
                        numericUpDown_StandUp_Set1.Value = standUp;
                        //button_Set1.PerformClick();
                        break;
                    case 1:
                        dateTimePicker_Date_Set2.Value = new DateTime(year, month, day, hour, min, sec);
                        dateTimePicker_Time_Set2.Value = new DateTime(year, month, day, hour, min, sec);
                        numericUpDown_Battery_Set2.Value = battery;
                        numericUpDown_Calories_Set2.Value = calories;
                        numericUpDown_Pulse_Set2.Value = pulse;
                        numericUpDown_Distance_Set2.Value = distance;
                        numericUpDown_Steps_Set2.Value = steps;
                        numericUpDown_Goal_Set2.Value = goal;
                        check_BoxBluetooth_Set2.Checked = bluetooth;
                        checkBox_Alarm_Set2.Checked = alarm;
                        checkBox_Lock_Set2.Checked = unlocked;
                        checkBox_DoNotDisturb_Set2.Checked = dnd;
                        numericUpDown_PAI_Set2.Value = pai;
                        numericUpDown_StandUp_Set2.Value = standUp;
                        //button_Set2.PerformClick();
                        break;
                    case 2:
                        dateTimePicker_Date_Set3.Value = new DateTime(year, month, day, hour, min, sec);
                        dateTimePicker_Time_Set3.Value = new DateTime(year, month, day, hour, min, sec);
                        numericUpDown_Battery_Set3.Value = battery;
                        numericUpDown_Calories_Set3.Value = calories;
                        numericUpDown_Pulse_Set3.Value = pulse;
                        numericUpDown_Distance_Set3.Value = distance;
                        numericUpDown_Steps_Set3.Value = steps;
                        numericUpDown_Goal_Set3.Value = goal;
                        check_BoxBluetooth_Set3.Checked = bluetooth;
                        checkBox_Alarm_Set3.Checked = alarm;
                        checkBox_Lock_Set3.Checked = unlocked;
                        checkBox_DoNotDisturb_Set3.Checked = dnd;
                        numericUpDown_PAI_Set3.Value = pai;
                        numericUpDown_StandUp_Set3.Value = standUp;
                        //button_Set3.PerformClick();
                        break;
                    case 3:
                        dateTimePicker_Date_Set4.Value = new DateTime(year, month, day, hour, min, sec);
                        dateTimePicker_Time_Set4.Value = new DateTime(year, month, day, hour, min, sec);
                        numericUpDown_Battery_Set4.Value = battery;
                        numericUpDown_Calories_Set4.Value = calories;
                        numericUpDown_Pulse_Set4.Value = pulse;
                        numericUpDown_Distance_Set4.Value = distance;
                        numericUpDown_Steps_Set4.Value = steps;
                        numericUpDown_Goal_Set4.Value = goal;
                        check_BoxBluetooth_Set4.Checked = bluetooth;
                        checkBox_Alarm_Set4.Checked = alarm;
                        checkBox_Lock_Set4.Checked = unlocked;
                        checkBox_DoNotDisturb_Set4.Checked = dnd;
                        numericUpDown_PAI_Set4.Value = pai;
                        numericUpDown_StandUp_Set4.Value = standUp;
                        //button_Set4.PerformClick();
                        break;
                    case 4:
                        dateTimePicker_Date_Set5.Value = new DateTime(year, month, day, hour, min, sec);
                        dateTimePicker_Time_Set5.Value = new DateTime(year, month, day, hour, min, sec);
                        numericUpDown_Battery_Set5.Value = battery;
                        numericUpDown_Calories_Set5.Value = calories;
                        numericUpDown_Pulse_Set5.Value = pulse;
                        numericUpDown_Distance_Set5.Value = distance;
                        numericUpDown_Steps_Set5.Value = steps;
                        numericUpDown_Goal_Set5.Value = goal;
                        check_BoxBluetooth_Set5.Checked = bluetooth;
                        checkBox_Alarm_Set5.Checked = alarm;
                        checkBox_Lock_Set5.Checked = unlocked;
                        checkBox_DoNotDisturb_Set5.Checked = dnd;
                        numericUpDown_PAI_Set5.Value = pai;
                        numericUpDown_StandUp_Set5.Value = standUp;
                        //button_Set5.PerformClick();
                        break;
                    case 5:
                        dateTimePicker_Date_Set6.Value = new DateTime(year, month, day, hour, min, sec);
                        dateTimePicker_Time_Set6.Value = new DateTime(year, month, day, hour, min, sec);
                        numericUpDown_Battery_Set6.Value = battery;
                        numericUpDown_Calories_Set6.Value = calories;
                        numericUpDown_Pulse_Set6.Value = pulse;
                        numericUpDown_Distance_Set6.Value = distance;
                        numericUpDown_Steps_Set6.Value = steps;
                        numericUpDown_Goal_Set6.Value = goal;
                        check_BoxBluetooth_Set6.Checked = bluetooth;
                        checkBox_Alarm_Set6.Checked = alarm;
                        checkBox_Lock_Set6.Checked = unlocked;
                        checkBox_DoNotDisturb_Set6.Checked = dnd;
                        numericUpDown_PAI_Set6.Value = pai;
                        numericUpDown_StandUp_Set6.Value = standUp;
                        //button_Set6.PerformClick();
                        break;
                    case 6:
                        dateTimePicker_Date_Set7.Value = new DateTime(year, month, day, hour, min, sec);
                        dateTimePicker_Time_Set7.Value = new DateTime(year, month, day, hour, min, sec);
                        numericUpDown_Battery_Set7.Value = battery;
                        numericUpDown_Calories_Set7.Value = calories;
                        numericUpDown_Pulse_Set7.Value = pulse;
                        numericUpDown_Distance_Set7.Value = distance;
                        numericUpDown_Steps_Set7.Value = steps;
                        numericUpDown_Goal_Set7.Value = goal;
                        check_BoxBluetooth_Set7.Checked = bluetooth;
                        checkBox_Alarm_Set7.Checked = alarm;
                        checkBox_Lock_Set7.Checked = unlocked;
                        checkBox_DoNotDisturb_Set7.Checked = dnd;
                        numericUpDown_PAI_Set7.Value = pai;
                        numericUpDown_StandUp_Set7.Value = standUp;
                        //button_Set7.PerformClick();
                        break;
                    case 7:
                        dateTimePicker_Date_Set8.Value = new DateTime(year, month, day, hour, min, sec);
                        dateTimePicker_Time_Set8.Value = new DateTime(year, month, day, hour, min, sec);
                        numericUpDown_Battery_Set8.Value = battery;
                        numericUpDown_Calories_Set8.Value = calories;
                        numericUpDown_Pulse_Set8.Value = pulse;
                        numericUpDown_Distance_Set8.Value = distance;
                        numericUpDown_Steps_Set8.Value = steps;
                        numericUpDown_Goal_Set8.Value = goal;
                        check_BoxBluetooth_Set8.Checked = bluetooth;
                        checkBox_Alarm_Set8.Checked = alarm;
                        checkBox_Lock_Set8.Checked = unlocked;
                        checkBox_DoNotDisturb_Set8.Checked = dnd;
                        numericUpDown_PAI_Set8.Value = pai;
                        numericUpDown_StandUp_Set8.Value = standUp;
                        //button_Set8.PerformClick();
                        break;
                    case 8:
                        dateTimePicker_Date_Set9.Value = new DateTime(year, month, day, hour, min, sec);
                        dateTimePicker_Time_Set9.Value = new DateTime(year, month, day, hour, min, sec);
                        numericUpDown_Battery_Set9.Value = battery;
                        numericUpDown_Calories_Set9.Value = calories;
                        numericUpDown_Pulse_Set9.Value = pulse;
                        numericUpDown_Distance_Set9.Value = distance;
                        numericUpDown_Steps_Set9.Value = steps;
                        numericUpDown_Goal_Set9.Value = goal;
                        check_BoxBluetooth_Set9.Checked = bluetooth;
                        checkBox_Alarm_Set9.Checked = alarm;
                        checkBox_Lock_Set9.Checked = unlocked;
                        checkBox_DoNotDisturb_Set9.Checked = dnd;
                        numericUpDown_PAI_Set9.Value = pai;
                        numericUpDown_StandUp_Set9.Value = standUp;
                        //button_Set9.PerformClick();
                        break;
                    case 9:
                        dateTimePicker_Date_Set10.Value = new DateTime(year, month, day, hour, min, sec);
                        dateTimePicker_Time_Set10.Value = new DateTime(year, month, day, hour, min, sec);
                        numericUpDown_Battery_Set10.Value = battery;
                        numericUpDown_Calories_Set10.Value = calories;
                        numericUpDown_Pulse_Set10.Value = pulse;
                        numericUpDown_Distance_Set10.Value = distance;
                        numericUpDown_Steps_Set10.Value = steps;
                        numericUpDown_Goal_Set10.Value = goal;
                        check_BoxBluetooth_Set10.Checked = bluetooth;
                        checkBox_Alarm_Set10.Checked = alarm;
                        checkBox_Lock_Set10.Checked = unlocked;
                        checkBox_DoNotDisturb_Set10.Checked = dnd;
                        numericUpDown_PAI_Set10.Value = pai;
                        numericUpDown_StandUp_Set10.Value = standUp;
                        //button_Set10.PerformClick();
                        break;
                }
            }

            numericUpDown_WeatherSet_Temp.Value = rnd.Next(-25, 35) + 1;
            numericUpDown_WeatherSet_MaxTemp.Value = numericUpDown_WeatherSet_Temp.Value;
            numericUpDown_WeatherSet_MinTemp.Value = numericUpDown_WeatherSet_Temp.Value - rnd.Next(3, 10);
            comboBox_WeatherSet_Icon.SelectedIndex = rnd.Next(0, 25);

            //PreviewImage();
            button_Set10.PerformClick();
            PreviewView = true;
            PreviewImage();
        }

        private void checkBox_WebW_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_WebW.Checked) checkBox_WebB.Checked = false;
            PreviewImage();
        }

        private void checkBox_WebB_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_WebB.Checked) checkBox_WebW.Checked = false;
            PreviewImage();
        }

        private void button_TextToJson_Click(object sender, EventArgs e)
        {
            string text = richTextBox_JsonText.Text;
            //richTextBox_JsonText.Text = text;


            try
            {
                Watch_Face = JsonConvert.DeserializeObject<WATCH_FACE_JSON>(text, new JsonSerializerSettings
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            }
            catch (Exception)
            {

                MessageBox.Show(Properties.FormStrings.Message_JsonError_Text, Properties.FormStrings.Message_Error_Caption, 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            richTextBox_JsonText.Text = JsonConvert.SerializeObject(Watch_Face, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            JsonToTree(richTextBox_JsonText.Text);
            PreviewView = false;
            JSON_read();
            PreviewView = true;
            PreviewImage();
        }

        private void button_SaveJson_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = FullFileDir;
            saveFileDialog.FileName = FileName;
            if(FileName==null || FileName.Length == 0)
            {
                if (FullFileDir != null && FullFileDir.Length > 3)
                {
                    saveFileDialog.FileName = Path.GetFileName(FullFileDir);
                }
            }
            saveFileDialog.Filter = Properties.FormStrings.FilterJson;

            //openFileDialog.Filter = "Json files (*.json) | *.json";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Title = Properties.FormStrings.Dialog_Title_Pack;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fullfilename = saveFileDialog.FileName;
                save_JSON_File(fullfilename, richTextBox_JsonText.Text);

                FileName = Path.GetFileName(fullfilename);
                FullFileDir = Path.GetDirectoryName(fullfilename);
                JSON_Modified = false;
                FormText();

                if (comboBox_Preview_image.SelectedIndex >= 0)
                {
                    button_RefreshPreview.Visible = true;
                    button_CreatePreview.Visible = false;
                }
                else
                {
                    button_RefreshPreview.Visible = false;
                    if (FileName != null && FullFileDir != null)
                    {
                        button_CreatePreview.Visible = true;
                    }
                    else
                    {
                        button_CreatePreview.Visible = false;
                    }
                }

                if (checkBox_JsonWarnings.Checked) jsonWarnings(fullfilename);
            }
        }

        private void button_Open_project_path_Click(object sender, EventArgs e)
        {
            if (FileName == null || FileName.Length == 0)
            {
                if (FullFileDir != null && FullFileDir.Length > 3)
                {
                    Process.Start(FullFileDir);
                }
            }
        }

        private void jsonWarnings(String fullfilename)
        {
            // пробелы в имени
            /*
            if (fullfilename.IndexOf(" ") != -1)
            {
                MessageBox.Show(Properties.FormStrings.Message_WarningSpaceInName_Text,
                    Properties.FormStrings.Message_Warning_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (Watch_Face == null) return;

            // 3 стрелки для аналоговых часов
            if (Watch_Face.MonthClockHand != null)
            {
                int i = 0;
                if ((Watch_Face.MonthClockHand.Hours != null) && (Watch_Face.MonthClockHand.Hours.Image != null)) i++;
                if ((Watch_Face.MonthClockHand.Minutes != null) && (Watch_Face.MonthClockHand.Minutes.Image != null)) i++;
                if ((Watch_Face.MonthClockHand.Seconds != null) && (Watch_Face.MonthClockHand.Seconds.Image != null)) i++;
                if (i < 3 && i > 0) MessageBox.Show(Properties.FormStrings.Message_Warning3clockHand_Text, 
                    Properties.FormStrings.Message_Warning_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // разные значения для десятков и единиц
            if (Watch_Face.Time != null)
            {
                bool err = false;
                if ((Watch_Face.Time.Hours != null) && (Watch_Face.Time.Hours.Tens != null) && (Watch_Face.Time.Hours.Ones != null))
                {
                    if (Watch_Face.Time.Hours.Tens.ImageIndex != Watch_Face.Time.Hours.Ones.ImageIndex) err = true;
                }
                if ((Watch_Face.Time.Minutes != null) && (Watch_Face.Time.Minutes.Tens != null) && (Watch_Face.Time.Minutes.Ones != null))
                {
                    if (Watch_Face.Time.Minutes.Tens.ImageIndex != Watch_Face.Time.Minutes.Ones.ImageIndex) err = true;
                }
                if ((Watch_Face.Time.Seconds != null) && (Watch_Face.Time.Seconds.Tens != null) && (Watch_Face.Time.Seconds.Ones != null))
                {
                    if (Watch_Face.Time.Seconds.Tens.ImageIndex != Watch_Face.Time.Seconds.Ones.ImageIndex) err = true;
                }
                if (err) MessageBox.Show(Properties.FormStrings.Message_WarningTensOnes_Text,
                    Properties.FormStrings.Message_Warning_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // минуты без часоа
            if (Watch_Face.Time != null)
            {
                if ((Watch_Face.Time.Minutes != null) && (Watch_Face.Time.Hours == null))
                {
                    MessageBox.Show(Properties.FormStrings.Message_WarningOnlyMin_Text,
                    Properties.FormStrings.Message_Warning_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if ((Watch_Face.Time.Seconds != null) && ((Watch_Face.Time.Minutes == null) || (Watch_Face.Time.Hours == null)))
                {
                    MessageBox.Show(Properties.FormStrings.Message_WarningOnlySec_Text,
                    Properties.FormStrings.Message_Warning_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            // надпись км для дистанции
            if ((Watch_Face.Activity != null) && (Watch_Face.Activity.Distance != null))
            {
                if(Watch_Face.Activity.Distance.SuffixImageIndex==null)
                    MessageBox.Show(Properties.FormStrings.Message_WarningDistanceSuffix,
                    Properties.FormStrings.Message_Warning_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // индикатор и сегменты для батареи
            if (Watch_Face.Battery != null)
            {
                if ((Watch_Face.Battery.Unknown4 != null) && (Watch_Face.Battery.Icons != null))
                {
                    MessageBox.Show(Properties.FormStrings.Message_WarningBatterySegment_Text,
                    Properties.FormStrings.Message_Warning_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            // количество картинок для батареи
            if (Watch_Face.Battery != null)
            {
                if ((Watch_Face.Battery.Images != null) && (Watch_Face.Battery.Images.ImagesCount > 10))
                {
                    MessageBox.Show(Properties.FormStrings.Message_WarningBatteryCount_Text,
                    Properties.FormStrings.Message_Warning_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            // количество сегментов для батареи
            if (Watch_Face.Battery != null)
            {
                if ((Watch_Face.Battery.Icons != null) && (Watch_Face.Battery.Icons.Coordinates != null) &&
                    (Watch_Face.Battery.Icons.Coordinates.Length > 10))
                {
                    MessageBox.Show(Properties.FormStrings.Message_WarningBatteryCount_Text,
                    Properties.FormStrings.Message_Warning_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            // индикатор и сегменты для прогресса шагов
            if (Watch_Face.StepsProgress != null)
            {
                if ((Watch_Face.StepsProgress.WeekDayClockHand != null && Watch_Face.StepsProgress.WeekDayClockHand.Image != null) 
                    && (Watch_Face.StepsProgress.Sliced != null))
                {
                    MessageBox.Show(Properties.FormStrings.Message_WarningBatterySegment_Text,
                    Properties.FormStrings.Message_Warning_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            // количество сегментов для ЧСС
            if ((Watch_Face.Activity != null) && (Watch_Face.Activity.ColouredSquares != null))
            {
                if ((Watch_Face.Activity.ColouredSquares.Coordinates != null) && 
                    (Watch_Face.Activity.ColouredSquares.Coordinates.Length != 6))
                {
                    MessageBox.Show(Properties.FormStrings.Message_WarningPulseIconCount_Text,
                    Properties.FormStrings.Message_Warning_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            // текущая температура и температура день/ночь
            if (Watch_Face.Weather != null && Watch_Face.Weather.Temperature != null)
            {
                if ((Watch_Face.Weather.Temperature.Current != null) && (Watch_Face.Weather.Temperature.Today == null))
                {
                    //if (Watch_Face.StepsProgress != null && Watch_Face.StepsProgress.WeekDayClockHand != null)
                    {
                        MessageBox.Show(Properties.FormStrings.Message_WarningTemperature_Text,
                            Properties.FormStrings.Message_Warning_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning); 
                    }
                }
            }

            // название и номер месяца
            if (Watch_Face.Date != null && Watch_Face.Date.MonthAndDay != null && Watch_Face.Date.MonthAndDay.Separate != null)
            {
                if (Watch_Face.Date.MonthAndDay.Separate.MonthName != null && Watch_Face.Date.MonthAndDay.Separate.Month != null)
                {
                    MessageBox.Show(Properties.FormStrings.Message_WarningMonthName,
                            Properties.FormStrings.Message_Warning_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            // дата в одну линию и отдельными блоками
            if (Watch_Face.Date != null && Watch_Face.Date.MonthAndDay != null)
            {
                if (Watch_Face.Date.MonthAndDay.Separate != null && Watch_Face.Date.MonthAndDay.OneLine != null)
                {
                    MessageBox.Show(Properties.FormStrings.Message_WarningDateOnelineAndSeparate,
                            Properties.FormStrings.Message_Warning_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            // последняя картинка для анимации
            if (Watch_Face.Unknown11 != null && Watch_Face.Unknown11.Unknown11_1 != null)
            {
                bool MotiomAnimationLastImage = false;
                foreach (MotiomAnimation MotiomAnimation in Watch_Face.Unknown11.Unknown11_1)
                {
                    if (MotiomAnimation.ImageIndex >= ListImages.Count-1)
                    {
                        MotiomAnimationLastImage = true;
                    }
                }
                if (MotiomAnimationLastImage)
                {
                    MessageBox.Show(Properties.FormStrings.Message_WarningAnimationLastImage,
                            Properties.FormStrings.Message_Warning_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            // отсутствует символ ошибки для текещей температуры
            if (Watch_Face.Weather != null && Watch_Face.Weather.Temperature != null 
                && Watch_Face.Weather.Temperature.Symbols != null)
            {
                if (Watch_Face.Weather.Temperature.Symbols.NoDataImageIndex == 0)
                {
                    MessageBox.Show(Properties.FormStrings.Message_WarningWeatherError,
                            Properties.FormStrings.Message_Warning_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            // отсутствует символ ошибки для активностей
            //if (Watch_Face.Activity != null)
            //{
            //    if (Watch_Face.Activity.NoDataImageIndex == null)
            //    {
            //        MessageBox.Show(Properties.FormStrings.Message_WarningActivityError,
            //                Properties.FormStrings.Message_Warning_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    }
            //}
            */
        }


        private void comboBox_WeatherSet_Icon_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Delete) || (e.KeyCode == Keys.Back))
            {
                comboBox_WeatherSet_Icon.Text = "";
                comboBox_WeatherSet_Icon.SelectedIndex = -1;
                PreviewImage();
                //JSON_write();
            }
        }

        private void checkBox_WeatherSet_Temp_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown_WeatherSet_Temp.Enabled = checkBox_WeatherSet_Temp.Checked;
        }

        private void checkBox_WeatherSet_DayTemp_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown_WeatherSet_MinTemp.Enabled = checkBox_WeatherSet_MaxMinTemp.Checked;
            numericUpDown_WeatherSet_MaxTemp.Enabled = checkBox_WeatherSet_MaxMinTemp.Checked;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //timer1.Enabled = false;
            ////pictureBox1.Image.Save(@"C:\test.png");
            //pictureBox1.Image = null;
        }

        private void panel_Preview_DoubleClick(object sender, EventArgs e)
        {
            if (pictureBox_Preview.Height < 300) button_PreviewBig.PerformClick();
            else
            {
                //if (radioButton_47.Checked) button_PreviewSmall.PerformClick();
                //else button_PreviewSmall_42.PerformClick();
            }
        }

        private void button_SavePNG_Click(object sender, EventArgs e)
        {
            Logger.WriteLine("* SavePNG");
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = FullFileDir;
            saveFileDialog.Filter = Properties.FormStrings.FilterPng;
            saveFileDialog.FileName = "Preview.png";
            //openFileDialog.Filter = "PNG Files: (*.png)|*.png";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Title = Properties.FormStrings.Dialog_Title_SavePNG;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap bitmap = new Bitmap(Convert.ToInt32(454), Convert.ToInt32(454), PixelFormat.Format32bppArgb);
                Bitmap mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gtr_2.png");
                if (radioButton_GTS2.Checked)
                {
                    bitmap = new Bitmap(Convert.ToInt32(348), Convert.ToInt32(442), PixelFormat.Format32bppArgb);
                    mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gts_2.png");
                }
                Graphics gPanel = Graphics.FromImage(bitmap);
                int link = radioButton_ScreenNormal.Checked ? 0 : 1;
                PreviewToBitmap(gPanel, 1.0f, false, false, false, false, false, false, false, true, false, false, link);
                if(checkBox_crop.Checked) bitmap = ApplyMask(bitmap, mask);
                bitmap.Save(saveFileDialog.FileName, ImageFormat.Png);
            }
            Logger.WriteLine("* SavePNG(end)");
        }

        private void button_SaveGIF_Click(object sender, EventArgs e)
        {
            Logger.WriteLine("* SaveGIF");
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = FullFileDir;
            saveFileDialog.Filter = Properties.FormStrings.FilterGif;
            saveFileDialog.FileName = "Preview.gif";
            //openFileDialog.Filter = "GIF Files: (*.gif)|*.gif";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Title = Properties.FormStrings.Dialog_Title_SaveGIF;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap bitmap = new Bitmap(Convert.ToInt32(454), Convert.ToInt32(454), PixelFormat.Format32bppArgb);
                Bitmap mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gtr_2.png");
                if (radioButton_GTS2.Checked)
                {
                    bitmap = new Bitmap(Convert.ToInt32(348), Convert.ToInt32(442), PixelFormat.Format32bppArgb);
                    mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gts_2.png");
                }
                Graphics gPanel = Graphics.FromImage(bitmap);
                bool save = false;
                Random rnd = new Random();
                PreviewView = false;

                using (MagickImageCollection collection = new MagickImageCollection())
                {
                    int WeatherSet_Temp = (int)numericUpDown_WeatherSet_Temp.Value;
                    int WeatherSet_DayTemp = (int)numericUpDown_WeatherSet_MaxTemp.Value;
                    int WeatherSet_NightTemp = (int)numericUpDown_WeatherSet_MinTemp.Value;
                    int WeatherSet_Icon = comboBox_WeatherSet_Icon.SelectedIndex;
                    for (int i = 0; i < 13; i++)
                    {
                        save = false;
                        switch (i)
                        {
                            case 0:
                                //button_Set1.PerformClick();
                                SetPreferences1();
                                save = true;
                                break;
                            case 1:
                                if (numericUpDown_Calories_Set2.Value != 1234)
                                {
                                    //button_Set2.PerformClick();
                                    SetPreferences2();
                                    save = true;
                                }
                                break;
                            case 2:
                                if (numericUpDown_Calories_Set3.Value != 1234)
                                {
                                    //button_Set3.PerformClick();
                                    SetPreferences3();
                                    save = true;
                                }
                                break;
                            case 3:
                                if (numericUpDown_Calories_Set4.Value != 1234)
                                {
                                    //button_Set4.PerformClick();
                                    SetPreferences4();
                                    save = true;
                                }
                                break;
                            case 4:
                                if (numericUpDown_Calories_Set5.Value != 1234)
                                {
                                    //button_Set5.PerformClick();
                                    SetPreferences5();
                                    save = true;
                                }
                                break;
                            case 5:
                                if (numericUpDown_Calories_Set6.Value != 1234)
                                {
                                    //button_Set6.PerformClick();
                                    SetPreferences6();
                                    save = true;
                                }
                                break;
                            case 6:
                                if (numericUpDown_Calories_Set7.Value != 1234)
                                {
                                    //button_Set7.PerformClick();
                                    SetPreferences7();
                                    save = true;
                                }
                                break;
                            case 7:
                                if (numericUpDown_Calories_Set8.Value != 1234)
                                {
                                    //button_Set8.PerformClick();
                                    SetPreferences8();
                                    save = true;
                                }
                                break;
                            case 8:
                                if (numericUpDown_Calories_Set9.Value != 1234)
                                {
                                    //button_Set9.PerformClick();
                                    SetPreferences9();
                                    save = true;
                                }
                                break;
                            case 9:
                                if (numericUpDown_Calories_Set10.Value != 1234)
                                {
                                    //button_Set10.PerformClick();
                                    SetPreferences10();
                                    save = true;
                                }
                                break;
                        }

                        if (save)
                        {
                            Logger.WriteLine("SaveGIF SetPreferences(" + i.ToString() + ")");

                            numericUpDown_WeatherSet_Temp.Value = rnd.Next(-25, 35) + 1;
                            numericUpDown_WeatherSet_MaxTemp.Value = numericUpDown_WeatherSet_Temp.Value;
                            numericUpDown_WeatherSet_MinTemp.Value = numericUpDown_WeatherSet_Temp.Value - rnd.Next(3, 10);
                            comboBox_WeatherSet_Icon.SelectedIndex = rnd.Next(0, 25);

                            //int link = radioButton_ScreenNormal.Checked ? 0 : 1;
                            int link = 0;
                            PreviewToBitmap(gPanel, 1.0f, false, false, false, false, false, false, false, true, false, false, link);
                            if (checkBox_crop.Checked) {
                                bitmap = ApplyMask(bitmap, mask);
                                gPanel = Graphics.FromImage(bitmap);
                            }
                            // Add first image and set the animation delay to 100ms
                            MagickImage item = new MagickImage(bitmap);
                            //ExifProfile profile = item.GetExifProfile();
                            collection.Add(item);
                            //collection[collection.Count - 1].AnimationDelay = 100;
                            collection[collection.Count - 1].AnimationDelay = (int)(100 * numericUpDown_Gif_Speed.Value);

                        }
                    }

                    Logger.WriteLine("SaveGIF_AOD");

                    numericUpDown_WeatherSet_Temp.Value = rnd.Next(-25, 35) + 1;
                    numericUpDown_WeatherSet_MaxTemp.Value = numericUpDown_WeatherSet_Temp.Value;
                    numericUpDown_WeatherSet_MinTemp.Value = numericUpDown_WeatherSet_Temp.Value - rnd.Next(3, 10);
                    comboBox_WeatherSet_Icon.SelectedIndex = rnd.Next(0, 25);

                    //int link = radioButton_ScreenNormal.Checked ? 0 : 1;
                    int link_AOD = 1;
                    PreviewToBitmap(gPanel, 1.0f, false, false, false, false, false, false, false, true, false, false, link_AOD);
                    if (checkBox_crop.Checked)
                    {
                        bitmap = ApplyMask(bitmap, mask);
                        gPanel = Graphics.FromImage(bitmap);
                    }
                    // Add first image and set the animation delay to 100ms
                    MagickImage item_AOD = new MagickImage(bitmap);
                    //ExifProfile profile = item.GetExifProfile();
                    collection.Add(item_AOD);
                    //collection[collection.Count - 1].AnimationDelay = 100;
                    collection[collection.Count - 1].AnimationDelay = (int)(100 * numericUpDown_Gif_Speed.Value);

                    numericUpDown_WeatherSet_Temp.Value = WeatherSet_Temp;
                    numericUpDown_WeatherSet_MaxTemp.Value = WeatherSet_DayTemp;
                    numericUpDown_WeatherSet_MinTemp.Value = WeatherSet_NightTemp;
                    comboBox_WeatherSet_Icon.SelectedIndex = WeatherSet_Icon;

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
                PreviewView = true;
                mask.Dispose();
            }
            Logger.WriteLine("* SaveGIF (end)");
        }

        public Bitmap ApplyMask(Bitmap inputImage, Bitmap mask)
        {
            Logger.WriteLine("* ApplyMask");
            //ushort[] bgColors = { 203, 255, 240 };
            ImageMagick.MagickImage image = new ImageMagick.MagickImage(inputImage);
            //ImageMagick.MagickImage image = new ImageMagick.MagickImage("0.png");
            ImageMagick.MagickImage combineMask = new ImageMagick.MagickImage(mask);

            //image.Composite(combineMask, ImageMagick.CompositeOperator.CopyAlpha, Channels.Alpha);
            image.Composite(combineMask, ImageMagick.CompositeOperator.In, Channels.Alpha);
            //image.Settings.BackgroundColor = new ImageMagick.MagickColor(bgColors[0], bgColors[1], bgColors[2]);
            //image.Alpha(ImageMagick.AlphaOption.Remove);
            //image.Transparent(ImageMagick.MagickColor.FromRgba(0, 0, 0, 0));

            Logger.WriteLine("* ApplyMask (end)");
            return image.ToBitmap();
        }

        // изменили модель часов
        private void radioButton_Model_Changed(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null && !radioButton.Checked) return;
            if (radioButton_GTR2.Checked)
            {
                pictureBox_Preview.Size = new Size((int)(230 * currentDPI), (int)(230 * currentDPI));
                
                textBox_unpack_command.Text = Program_Settings.unpack_command_GTR_2;

                button_unpack.Enabled = true;
                button_pack.Enabled = true;
                button_zip.Enabled = true;
            }
            else if (radioButton_GTS2.Checked)
            {
                pictureBox_Preview.Size = new Size((int)(177 * currentDPI), (int)(224 * currentDPI));
                
                textBox_unpack_command.Text = Program_Settings.unpack_command_GTS_2;

                button_unpack.Enabled = false;
                button_pack.Enabled = false;
                button_zip.Enabled = false;
            }

            // изменяем размер панели для предпросмотра если она не влазит
            if (pictureBox_Preview.Top + pictureBox_Preview.Height > radioButton_GTR2.Top)
            {
                float newHeight = radioButton_GTR2.Top - pictureBox_Preview.Top;
                float scale = newHeight / pictureBox_Preview.Height;
                pictureBox_Preview.Size = new Size((int)(pictureBox_Preview.Width * scale), (int)(pictureBox_Preview.Height * scale));
            }

            FormText();

            if ((formPreview != null) && (formPreview.Visible))
            {
                if (Form_Preview.Model_Wath.model_gtr47 != radioButton_GTR2.Checked)
                    Form_Preview.Model_Wath.model_gtr47 = radioButton_GTR2.Checked;
                if (Form_Preview.Model_Wath.model_gts != radioButton_GTS2.Checked)
                    Form_Preview.Model_Wath.model_gts = radioButton_GTS2.Checked;
                formPreview.radioButton_CheckedChanged(sender, e);
            }

            Program_Settings.Model_GTR47 = radioButton_GTR2.Checked;
            Program_Settings.Model_GTS = radioButton_GTS2.Checked;
            string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);


            PreviewImage();
            JSON_write();
        }

        // устанавливаем заголовок окна
        private void FormText()
        {
            //throw new NotImplementedException(); FileName
            string FormName = "GTR watch face editor";
            string FormNameSufix = "";
            if (FileName != null)
            {
                FormNameSufix = Path.GetFileNameWithoutExtension(FileName); 
            }
            if (radioButton_GTR2.Checked)
            {
                FormName = "GTR 2 watch face editor";
            }
            else if (radioButton_GTS2.Checked)
            {
                FormName = "GTS 2 watch face editor";
            }

            if (FormNameSufix.Length == 0)
            {
                this.Text = FormName;
                button_OpenDir.Enabled = false;
            }
            else
            {
                if (JSON_Modified) FormNameSufix = FormNameSufix + "*";
                this.Text = FormName + " (" + FormNameSufix + ")";
                button_OpenDir.Enabled = true;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            //timer2.Enabled = false;
            //if ((formPreview != null) && (formPreview.Visible))
            //{
            //    Form_Preview.Model_Wath.model_gtr47 = radioButton_47.Checked;
            //    Form_Preview.Model_Wath.model_gtr42 = radioButton_42.Checked;
            //    Form_Preview.Model_Wath.model_gts = radioButton_gts.Checked;
            //    Graphics gPanelPreviewPaint = formPreview.panel_Preview.CreateGraphics();
            //    //gPanelPreviewPaint.Clear(panel_Preview.BackColor);
            //    formPreview.radioButton_CheckedChanged(sender, e);
            //    float scalePreviewPaint = 1.0f;
            //    if (formPreview.radioButton_small.Checked) scalePreviewPaint = 0.5f;
            //    if (formPreview.radioButton_large.Checked) scalePreviewPaint = 1.5f;
            //    if (formPreview.radioButton_xlarge.Checked) scalePreviewPaint = 2.0f;
            //    if (formPreview.radioButton_xxlarge.Checked) scalePreviewPaint = 2.5f;
            //    PreviewToBitmap(gPanelPreviewPaint, scalePreviewPaint, checkBox_crop.Checked,
            //        checkBox_WebW.Checked, checkBox_WebB.Checked, checkBox_border.Checked, 
            //        checkBox_Show_Shortcuts.Checked, checkBox_Shortcuts_Area.Checked, checkBox_Shortcuts_Border.Checked, true, 0);
            //    gPanelPreviewPaint.Dispose();
            //}
        }

        // координаты в заголовке при перемещении мыши
        private void pictureBox_Preview_MouseMove(object sender, MouseEventArgs e)
        {
            int CursorX = e.X;
            int CursorY = e.Y;

            label_preview_X.Text = "X=" + (CursorX * 2).ToString();
            label_preview_Y.Text = "Y=" + (CursorY * 2).ToString();

            label_preview_X.Visible = true;
            label_preview_Y.Visible = true;
        }

        private void pictureBox_Preview_MouseLeave(object sender, EventArgs e)
        {
            label_preview_X.Visible = false;
            label_preview_Y.Visible = false;

        }

        private void comboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Delete) || (e.KeyCode == Keys.Back))
            {
                ComboBox comboBox = sender as ComboBox;
                comboBox.Text = "";
                comboBox.SelectedIndex = -1;
                PreviewImage();
                JSON_write();
            }
        }

        private void comboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        //private void numericUpDown_X_DoubleClick(object sender, EventArgs e)
        //{
        //    NumericUpDown numericUpDown = sender as NumericUpDown;
        //    numericUpDown.Value = MouseСoordinates.X;
        //}

        //private void numericUpDown_Y_DoubleClick(object sender, EventArgs e)
        //{
        //    NumericUpDown numericUpDown = sender as NumericUpDown;
        //    numericUpDown.Value = MouseСoordinates.Y;
        //}

        //private void contextMenuStrip_X_Click(object sender, EventArgs e)
        //{
        //    ContextMenuStrip menu = sender as ContextMenuStrip;
        //    Control sourceControl = menu.SourceControl;
        //    NumericUpDown numericUpDown = sourceControl as NumericUpDown;
        //    numericUpDown.Value = MouseСoordinates.X;
        //}

        //private void contextMenuStrip_Y_Click(object sender, EventArgs e)
        //{
        //    ContextMenuStrip menu = sender as ContextMenuStrip;
        //    Control sourceControl = menu.SourceControl;
        //    NumericUpDown numericUpDown = sourceControl as NumericUpDown;
        //    numericUpDown.Value = MouseСoordinates.Y;
        //}

        private void contextMenuStrip_X_Opening(object sender, CancelEventArgs e)
        {
            if ((MouseСoordinates.X < 0) || (MouseСoordinates.Y < 0))
            {
                contextMenuStrip_X.Items[0].Enabled = false;
            }
            else
            {
                contextMenuStrip_X.Items[0].Enabled = true;
            }
            decimal i = 0;
            if ((Clipboard.ContainsText() == true) && (decimal.TryParse(Clipboard.GetText(), out i)))
            { 
                contextMenuStrip_X.Items[2].Enabled = true;
            }
            else
            {
                contextMenuStrip_X.Items[2].Enabled = false;
            }
        }

        private void contextMenuStrip_Y_Opening(object sender, CancelEventArgs e)
        {
            if ((MouseСoordinates.X < 0) || (MouseСoordinates.Y < 0))
            {
                contextMenuStrip_Y.Items[0].Enabled = false;
            }
            else
            {
                contextMenuStrip_Y.Items[0].Enabled = true;
            }
            decimal i = 0;
            if ((Clipboard.ContainsText() == true) && (decimal.TryParse(Clipboard.GetText(), out i)))
            {
                contextMenuStrip_Y.Items[2].Enabled = true;
            }
            else
            {
                contextMenuStrip_Y.Items[2].Enabled = false;
            }
        }

        private void вставитьКоординатуХToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Try to cast the sender to a ToolStripItem
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    // Get the control that is displaying this context menu
                    Control sourceControl = owner.SourceControl;
                    NumericUpDown numericUpDown = sourceControl as NumericUpDown;
                    numericUpDown.Value = MouseСoordinates.X;
                }
            }
        }

        private void вставитьКоординатуYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Try to cast the sender to a ToolStripItem
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    // Get the control that is displaying this context menu
                    Control sourceControl = owner.SourceControl;
                    NumericUpDown numericUpDown = sourceControl as NumericUpDown;
                    numericUpDown.Value = MouseСoordinates.Y;
                }
            }
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Try to cast the sender to a ToolStripItem
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    // Get the control that is displaying this context menu
                    Control sourceControl = owner.SourceControl;
                    NumericUpDown numericUpDown = sourceControl as NumericUpDown;
                    Clipboard.SetText(numericUpDown.Value.ToString());
                }
            }
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    // Get the control that is displaying this context menu
                    Control sourceControl = owner.SourceControl;
                    NumericUpDown numericUpDown = sourceControl as NumericUpDown;
                    //Если в буфере обмен содержится текст
                    if (Clipboard.ContainsText() == true)
                    {
                        //Извлекаем (точнее копируем) его и сохраняем в переменную
                        decimal i = 0;
                        if (decimal.TryParse(Clipboard.GetText(), out i))
                        {
                            if (i > numericUpDown.Maximum) i = numericUpDown.Maximum;
                            if (i < numericUpDown.Minimum) i = numericUpDown.Minimum;
                            numericUpDown.Value = i;
                        } 
                    }

                }
            }
        }
        
        private void numericUpDown_X_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MouseСoordinates.X < 0) return;
            NumericUpDown numericUpDown = sender as NumericUpDown;
            if (e.X <= numericUpDown.Controls[1].Width + 1)
            {
                // Click is in text area
                numericUpDown.Value = MouseСoordinates.X;
            }
            //else
            //{
            //    if (e.Y <= numericUpDown.Controls[1].Height / 2)
            //    {
            //        // Click is on Up arrow
            //    }
            //    else
            //    {
            //        // Click is on Down arrow
            //    }
            //}
        }

        private void numericUpDown_Y_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MouseСoordinates.Y < 0) return;
            NumericUpDown numericUpDown = sender as NumericUpDown;
            if (e.X <= numericUpDown.Controls[1].Width + 1)
            {
                // Click is in text area
                numericUpDown.Value = MouseСoordinates.Y;
            }
            else
            {
                if (e.Y <= numericUpDown.Controls[1].Height / 2)
                {
                    // Click is on Up arrow
                }
                else
                {
                    // Click is on Down arrow
                }
            }
        }

        private void numericUpDown_OffSetX_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MouseСoordinates.X < 0) return;
            NumericUpDown numericUpDown = sender as NumericUpDown;
            if (e.X <= numericUpDown.Controls[1].Width + 1)
            {
                // Click is in text area
                //numericUpDown.Value = MouseСoordinates.X - offSet_X;
            }
            else
            {
                if (e.Y <= numericUpDown.Controls[1].Height / 2)
                {
                    // Click is on Up arrow
                }
                else
                {
                    // Click is on Down arrow
                }
            }
        }

        private void numericUpDown_OffSetY_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MouseСoordinates.Y < 0) return;
            NumericUpDown numericUpDown = sender as NumericUpDown;
            if (e.X <= numericUpDown.Controls[1].Width + 1)
            {
                // Click is in text area
                //numericUpDown.Value = MouseСoordinates.Y - offSet_Y;
            }
            else
            {
                if (e.Y <= numericUpDown.Controls[1].Height / 2)
                {
                    // Click is on Up arrow
                }
                else
                {
                    // Click is on Down arrow
                }
            }
        }

        // сохраняем настройки
        private void radioButton_Settings_Unpack_Dialog_CheckedChanged(object sender, EventArgs e)
        {
            if (Settings_Load) return;
            Program_Settings.Settings_AfterUnpack_Dialog = radioButton_Settings_AfterUnpack_Dialog.Checked;
            Program_Settings.Settings_AfterUnpack_DoNothing = radioButton_Settings_AfterUnpack_DoNothing.Checked;
            Program_Settings.Settings_AfterUnpack_Download = radioButton_Settings_AfterUnpack_Download.Checked;

            Program_Settings.Settings_Open_Dialog = radioButton_Settings_Open_Dialog.Checked;
            Program_Settings.Settings_Open_DoNotning = radioButton_Settings_Open_DoNotning.Checked;
            Program_Settings.Settings_Open_Download = radioButton_Settings_Open_Download.Checked;

            Program_Settings.Settings_Pack_Dialog = radioButton_Settings_Pack_Dialog.Checked;
            Program_Settings.Settings_Pack_DoNotning = radioButton_Settings_Pack_DoNotning.Checked;
            Program_Settings.Settings_Pack_GoToFile = radioButton_Settings_Pack_GoToFile.Checked;

            Program_Settings.Settings_Unpack_Dialog = radioButton_Settings_Unpack_Dialog.Checked;
            Program_Settings.Settings_Unpack_Replace = radioButton_Settings_Unpack_Replace.Checked;
            Program_Settings.Settings_Unpack_Save = radioButton_Settings_Unpack_Save.Checked;

            //string JSON_String = JObject.FromObject(Program_Settings).ToString();
            string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);
        }
        private void numericUpDown_Gif_Speed_ValueChanged(object sender, EventArgs e)
        {
            Program_Settings.Gif_Speed = (float)numericUpDown_Gif_Speed.Value;
            string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);
        }
        private void checkBox_border_CheckedChanged(object sender, EventArgs e)
        {
            Program_Settings.ShowBorder = checkBox_border.Checked;
            string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);
        }
        private void checkBox_crop_CheckedChanged(object sender, EventArgs e)
        {
            Program_Settings.Crop = checkBox_crop.Checked;
            string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);
        }
        private void checkBox_Show_Shortcuts_CheckedChanged(object sender, EventArgs e)
        {
            Program_Settings.Show_Shortcuts = checkBox_Show_Shortcuts.Checked;
            string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);
        }
        private void checkBox_CircleScaleImage_CheckedChanged(object sender, EventArgs e)
        {
            Program_Settings.Show_CircleScale_Area = checkBox_CircleScaleImage.Checked;
            string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);
        }
        private void checkBox_center_marker_CheckedChanged(object sender, EventArgs e)
        {
            Program_Settings.Shortcuts_Center_marker = checkBox_center_marker.Checked;
            string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);
        }
        private void textBox_unpack_command_Leave(object sender, EventArgs e)
        {
            if (radioButton_GTR2.Checked)
            {
                Program_Settings.unpack_command_GTR_2 = textBox_unpack_command.Text;
            }
            else if (radioButton_GTS2.Checked)
            {
                Program_Settings.unpack_command_GTS_2 = textBox_unpack_command.Text;
            }

            string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);
        }
        private void comboBox_Language_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program_Settings.language = comboBox_Language.Text;
            SetLanguage();
            string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);
            if (!Settings_Load)
            {
                if (MessageBox.Show(Properties.FormStrings.Message_Restart_Text1 + Environment.NewLine +
                                Properties.FormStrings.Message_Restart_Text2, Properties.FormStrings.Message_Restart_Caption, 
                                MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Application.Restart();
                }
            }
        }

        private void button_Reset_Click(object sender, EventArgs e)
        {
            if (File.Exists(Application.StartupPath + @"\Settings.json"))
            {
                File.Delete(Application.StartupPath + @"\Settings.json");
                if (MessageBox.Show(Properties.FormStrings.Message_Restart_Text1 + Environment.NewLine +
                                Properties.FormStrings.Message_Restart_Text2, Properties.FormStrings.Message_Restart_Caption,
                                MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Application.Restart();
                }
            }
        }

        // картинки в выпадающем списке
        private void comboBox_Image_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            //if (comboBox.Items.Count < 10) comboBox.DropDownHeight = comboBox.Items.Count * 35;
            //else comboBox.DropDownHeight = 106;
            float size = comboBox.Font.Size;
            Font myFont;
            FontFamily family = comboBox.Font.FontFamily;
            e.DrawBackground();
            int itemWidth = e.Bounds.Height;
            int itemHeight = e.Bounds.Height - 4;

            //SolidBrush solidBrush = new SolidBrush(Color.Black);
            //Rectangle rectangleFill = new Rectangle(2, e.Bounds.Top + 2,
            //        e.Bounds.Width, e.Bounds.Height - 4);
            //e.Graphics.FillRectangle(solidBrush, rectangleFill);
            //var src = new Bitmap(ListImagesFullName[image_index]);
            if (e.Index >= 0)
            {
                try
                {
                    using (FileStream stream = new FileStream(ListImagesFullName[e.Index], FileMode.Open, FileAccess.Read))
                    {
                        Image image = Image.FromStream(stream);
                        float scale = (float)itemWidth / image.Width;
                        if ((float)itemHeight / image.Height < scale) scale = (float)itemHeight / image.Height;
                        float itemWidthRec = image.Width * scale;
                        float itemHeightRec = image.Height * scale;
                        Rectangle rectangle = new Rectangle((int)(itemWidth- itemWidthRec)/2+2,
                            e.Bounds.Top + (int)(itemHeight - itemHeightRec) / 2 + 2, (int)itemWidthRec, (int)itemHeightRec);
                        e.Graphics.DrawImage(image, rectangle);
                    }
                }
                catch { }
            }
            //e.Graphics.DrawImage(imageList1.Images[e.Index], rectangle);
            myFont = new Font(family, size);
            StringFormat lineAlignment = new StringFormat();
            //lineAlignment.Alignment = StringAlignment.Center;
            lineAlignment.LineAlignment = StringAlignment.Center;
            if (e.Index >= 0)
                e.Graphics.DrawString(comboBox.Items[e.Index].ToString(), myFont, System.Drawing.Brushes.Black, new RectangleF(e.Bounds.X + itemWidth, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height), lineAlignment);
            e.DrawFocusRectangle();

        }
        private void comboBox_Image_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 35;
        }

        // проверка разрядности системы
        public static bool Is64Bit()
        {
            if (Environment.Is64BitOperatingSystem) return true;
            else return false;
        }

        public string GetFileSize(FileInfo file)
        {
            try
            {
                double sizeinbytes = file.Length;
                double sizeinkbytes = Math.Round((sizeinbytes / 1024), 2);
                double sizeinmbytes = Math.Round((sizeinkbytes / 1024), 2);
                double sizeingbytes = Math.Round((sizeinmbytes / 1024), 2);
                if (sizeingbytes > 1)
                    return string.Format("{0} GB", sizeingbytes); //размер в гигабайтах
                else if (sizeinmbytes > 1)
                    return string.Format("{0} MB", sizeinmbytes); //возвращает размер в мегабайтах, если размер файла менее одного гигабайта
                else if (sizeinkbytes > 1)
                    return string.Format("{0} KB", sizeinkbytes); //возвращает размер в килобайтах, если размер файла менее одного мегабайта
                else
                    return string.Format("{0} B", sizeinbytes); //возвращает размер в байтах, если размер файла менее одного килобайта
            }
            catch { return "Ошибка получения размера файла"; } //перехват ошибок и возврат сообщения об ошибке
        }

        public double GetFileSizeMB(FileInfo file)
        {
            try
            {
                double sizeinbytes = file.Length;
                double sizeinkbytes = Math.Round((sizeinbytes / 1024), 2);
                double sizeinmbytes = Math.Round((sizeinkbytes / 1024), 2);
                double sizeingbytes = Math.Round((sizeinmbytes / 1024), 2);
                return sizeinmbytes;
            }
            catch { return 0; } //перехват ошибок и возврат сообщения об ошибке
        }
        public double GetFileSizeMB(double sizeinbytes)
        {
            try
            {
                //double sizeinbytes = file.Length;
                double sizeinkbytes = Math.Round((sizeinbytes / 1024), 2);
                double sizeinmbytes = Math.Round((sizeinkbytes / 1024), 2);
                double sizeingbytes = Math.Round((sizeinmbytes / 1024), 2);
                return sizeinmbytes;
            }
            catch { return 0; } //перехват ошибок и возврат сообщения об ошибке
        }

        private void dataGridView_IconSet_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;
            if (dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                Regex my_reg = new Regex(@"[^-\d]");
                string oldValue = dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                string newValue = my_reg.Replace(oldValue, "");
                dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = newValue;
                if (newValue.Length == 0) dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = null;
            }

            try
            {
                if ((dataGridView.Rows[e.RowIndex].Cells[0].Value == null) &&
                        (dataGridView.Rows[e.RowIndex].Cells[1].Value == null) && (e.RowIndex < dataGridView.Rows.Count - 1))
                    dataGridView.Rows.RemoveAt(e.RowIndex);
            }
            catch (Exception )
            {
            }
            PreviewImage();
            JSON_write();
        }

        private void dataGridView_IconSet_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            PreviewImage();
            JSON_write();
        }
        
        private void dataGridView_IconSet_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;
            if (e.ColumnIndex == -1)
            {
                dataGridView.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
                dataGridView.EndEdit();
            }
            else if (dataGridView.EditMode != DataGridViewEditMode.EditOnEnter)
            {
                dataGridView.EditMode = DataGridViewEditMode.EditOnEnter;
                dataGridView.BeginEdit(false);
            }

            try
            {
                for (int i = dataGridView.Rows.Count - 1; i > -1; i--)
                {
                    DataGridViewRow row = dataGridView.Rows[i];
                    if (!row.IsNewRow && row.Cells[0].Value == null && row.Cells[1].Value == null)
                    {
                        dataGridView.Rows.Remove(row);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void dataGridView_MotiomAnimation_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;
            if (e.ColumnIndex == -1)
            {
                dataGridView.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
                dataGridView.EndEdit();
            }
            else if (dataGridView.EditMode != DataGridViewEditMode.EditOnEnter)
            {
                dataGridView.EditMode = DataGridViewEditMode.EditOnEnter;
                dataGridView.BeginEdit(false);
            }

            try
            {
                for (int i = dataGridView.Rows.Count - 1; i > -1; i--)
                {
                    DataGridViewRow row = dataGridView.Rows[i];
                    if (!row.IsNewRow && row.Cells[1].Value == null && row.Cells[2].Value == null &&
                        row.Cells[3].Value == null && row.Cells[4].Value == null && row.Cells[5].Value == null &&
                        row.Cells[6].Value == null && row.Cells[7].Value == null)
                    {
                        dataGridView.Rows.Remove(row);
                    }
                }
            }
            catch (Exception)
            {
            }

        }

        private void dataGridView_IconSet_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;
            object head = dataGridView.Rows[e.RowIndex].HeaderCell.Value;
            if (head == null || !head.Equals((e.RowIndex + 1).ToString()))
                dataGridView.Rows[e.RowIndex].HeaderCell.Value = (e.RowIndex + 1).ToString();
        }

        private void dataGridView_IconSet_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;
            if (e.ColumnIndex == 0 && MouseСoordinates.X >= 0)
            {
                dataGridView.Rows[e.RowIndex].Cells[0].Value = MouseСoordinates.X;
            }
            if (e.ColumnIndex == 1 && MouseСoordinates.Y >= 0)
            {
                dataGridView.Rows[e.RowIndex].Cells[1].Value = MouseСoordinates.Y;
            }
        }

        private void dataGridView_MotiomAnimation_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;
            if ((e.ColumnIndex == 1 || e.ColumnIndex == 3) && MouseСoordinates.X >= 0)
            {
                dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = MouseСoordinates.X;
            }
            if ((e.ColumnIndex == 2 || e.ColumnIndex == 4) && MouseСoordinates.Y >= 0)
            {
                dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = MouseСoordinates.Y;
            }
        }

        private void вставитьКоординатыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Try to cast the sender to a ToolStripItem
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    // Get the control that is displaying this context menu
                    Control sourceControl = owner.SourceControl;
                    DataGridView dataGridView = sourceControl as DataGridView;
                    DataGridViewRow row = dataGridView.CurrentRow;
                    row.Cells[0].Value = MouseСoordinates.X;
                    row.Cells[1].Value = MouseСoordinates.Y;
                    PreviewImage();
                    JSON_write();
                }
            }
        }

        private void копироватьToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    // Get the control that is displaying this context menu
                    Control sourceControl = owner.SourceControl;
                    DataGridView dataGridView = sourceControl as DataGridView;
                    DataGridViewCell cell = dataGridView.CurrentCell;
                    Clipboard.SetText(cell.Value.ToString());
                }
            }
        }

        private void вставитьToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    // Get the control that is displaying this context menu
                    Control sourceControl = owner.SourceControl;
                    DataGridView dataGridView = sourceControl as DataGridView;
                    DataGridViewCell cell = dataGridView.CurrentCell;
                    //Если в буфере обмен содержится текст
                    if (Clipboard.ContainsText() == true)
                    {
                        //Извлекаем (точнее копируем) его и сохраняем в переменную
                        decimal i = 0;
                        if (decimal.TryParse(Clipboard.GetText(), out i))
                        {
                            cell.Value = i;
                            int x = dataGridView.CurrentCellAddress.X;
                            int y = dataGridView.CurrentCellAddress.Y;
                            dataGridView.CurrentCell = dataGridView.Rows[0].Cells[0];
                            dataGridView.CurrentCell = dataGridView.Rows[y].Cells[x];
                            dataGridView.BeginEdit(false);
                            PreviewImage();
                            JSON_write();
                        }
                    }

                }
            }
        }

        private void удалитьСтрокуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    // Get the control that is displaying this context menu
                    Control sourceControl = owner.SourceControl;
                    DataGridView dataGridView = sourceControl as DataGridView;
                    try
                    {
                        int rowIndex = dataGridView.CurrentCellAddress.Y;
                        dataGridView.Rows.RemoveAt(rowIndex);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        private void dataGridView_IconSet_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView dataGridView = sender as DataGridView;
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    dataGridView.CurrentCell = dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex]; 
                }
            }
        }

        private void contextMenuStrip_XY_InTable_Opening(object sender, CancelEventArgs e)
        {
            if ((MouseСoordinates.X < 0) || (MouseСoordinates.Y < 0))
            {
                contextMenuStrip_XY_InTable.Items[0].Enabled = false;
            }
            else
            {
                contextMenuStrip_XY_InTable.Items[0].Enabled = true;
            }
            decimal i = 0;
            if ((Clipboard.ContainsText() == true) && (decimal.TryParse(Clipboard.GetText(), out i)))
            {
                contextMenuStrip_XY_InTable.Items[2].Enabled = true;
            }
            else
            {
                contextMenuStrip_XY_InTable.Items[2].Enabled = false;
            }
        }

        private void linkLabel_py_amazfit_tools_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Tnxec2/py_amazfit_tools/tree/GTS2");
        }

        private void linkLabel_resunpacker_qzip_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://4pda.ru/forum/index.php?showtopic=1015239&st=100#entry103520948");
        }

        private void checkBox_Shortcuts_Area_CheckedChanged(object sender, EventArgs e)
        {
            if (Settings_Load) return;
            Program_Settings.Shortcuts_Area = checkBox_Shortcuts_Area.Checked;
            Program_Settings.Shortcuts_Border = checkBox_Shortcuts_Border.Checked;

            string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);
        }

        private void checkBox_ShowIn12hourFormat_CheckedChanged(object sender, EventArgs e)
        {
            if (Settings_Load) return;
            Program_Settings.ShowIn12hourFormat = checkBox_ShowIn12hourFormat.Checked;

            string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);
        }

        private void checkBox_WeatherSet_Temp_Click(object sender, EventArgs e)
        {
            Watch_Face_Preview_Set.Weather.Temperature = (int)numericUpDown_WeatherSet_Temp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureMax = (int)numericUpDown_WeatherSet_MaxTemp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureMin = (int)numericUpDown_WeatherSet_MinTemp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureNoData = !checkBox_WeatherSet_Temp.Checked;
            Watch_Face_Preview_Set.Weather.TemperatureMinMaxNoData = !checkBox_WeatherSet_MaxMinTemp.Checked;
            Watch_Face_Preview_Set.Weather.Icon = comboBox_WeatherSet_Icon.SelectedIndex;
            PreviewImage();
        }

        private void numericUpDown_WeatherSet_Temp_ValueChanged(object sender, EventArgs e)
        {
            Watch_Face_Preview_Set.Weather.Temperature = (int)numericUpDown_WeatherSet_Temp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureMax = (int)numericUpDown_WeatherSet_MaxTemp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureMin = (int)numericUpDown_WeatherSet_MinTemp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureNoData = !checkBox_WeatherSet_Temp.Checked;
            Watch_Face_Preview_Set.Weather.TemperatureMinMaxNoData = !checkBox_WeatherSet_MaxMinTemp.Checked;
            Watch_Face_Preview_Set.Weather.Icon = comboBox_WeatherSet_Icon.SelectedIndex;
            PreviewImage();
        }

        private void comboBox_WeatherSet_Icon_SelectedIndexChanged(object sender, EventArgs e)
        {
            Watch_Face_Preview_Set.Weather.Temperature = (int)numericUpDown_WeatherSet_Temp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureMax = (int)numericUpDown_WeatherSet_MaxTemp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureMin = (int)numericUpDown_WeatherSet_MinTemp.Value;
            Watch_Face_Preview_Set.Weather.TemperatureNoData = !checkBox_WeatherSet_Temp.Checked;
            Watch_Face_Preview_Set.Weather.TemperatureMinMaxNoData = !checkBox_WeatherSet_MaxMinTemp.Checked;
            Watch_Face_Preview_Set.Weather.Icon = comboBox_WeatherSet_Icon.SelectedIndex;
            PreviewImage();
        }

        private void radioButton_MotiomAnimation_StartCoordinates_CheckedChanged(object sender, EventArgs e)
        {
            PreviewImage();
        }

        private void contextMenuStrip_XY_InAnimationTable_Opening(object sender, CancelEventArgs e)
        {
            if ((MouseСoordinates.X < 0) || (MouseСoordinates.Y < 0))
            {
                contextMenuStrip_XY_InAnimationTable.Items[0].Enabled = false;
                contextMenuStrip_XY_InAnimationTable.Items[1].Enabled = false;
            }
            else
            {
                contextMenuStrip_XY_InAnimationTable.Items[0].Enabled = true;
                contextMenuStrip_XY_InAnimationTable.Items[1].Enabled = true;
            }
            decimal i = 0;
            if ((Clipboard.ContainsText() == true) && (decimal.TryParse(Clipboard.GetText(), out i)))
            {
                contextMenuStrip_XY_InAnimationTable.Items[4].Enabled = true;
            }
            else
            {
                contextMenuStrip_XY_InAnimationTable.Items[4].Enabled = false;
            }
        }

        private void вставитьToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    // Get the control that is displaying this context menu
                    Control sourceControl = owner.SourceControl;
                    DataGridView dataGridView = sourceControl as DataGridView;
                    DataGridViewCell cell = dataGridView.CurrentCell;
                    //Если в буфере обмен содержится текст
                    if (Clipboard.ContainsText() == true)
                    {
                        //Извлекаем (точнее копируем) его и сохраняем в переменную
                        decimal i = 0;
                        if (decimal.TryParse(Clipboard.GetText(), out i))
                        {
                            cell.Value = i;
                            int x = dataGridView.CurrentCellAddress.X;
                            int y = dataGridView.CurrentCellAddress.Y;
                            dataGridView.CurrentCell = dataGridView.Rows[0].Cells[1];
                            dataGridView.CurrentCell = dataGridView.Rows[y].Cells[x];
                            dataGridView.BeginEdit(false);
                            PreviewImage();
                            JSON_write();
                        }
                    }

                }
            }
        }

        private void вставитьНачальныеКоординатыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Try to cast the sender to a ToolStripItem
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    // Get the control that is displaying this context menu
                    Control sourceControl = owner.SourceControl;
                    DataGridView dataGridView = sourceControl as DataGridView;
                    DataGridViewRow row = dataGridView.CurrentRow;
                    row.Cells[1].Value = MouseСoordinates.X;
                    row.Cells[2].Value = MouseСoordinates.Y;

                    // копируем данные в поля для редактирования
                    MotiomAnimation_Update = true;
                    int StartCoordinates_X = 0;
                    int StartCoordinates_Y = 0;
                    int EndCoordinates_X = 0;
                    int EndCoordinates_Y = 0;
                    int ImageIndex = 0;
                    //numericUpDown_MotiomAnimation_StartCoordinates_X.Value = StartCoordinates_X;
                    //numericUpDown_MotiomAnimation_StartCoordinates_Y.Value = StartCoordinates_Y;
                    //numericUpDown_MotiomAnimation_EndCoordinates_X.Value = EndCoordinates_X;
                    //numericUpDown_MotiomAnimation_EndCoordinates_Y.Value = EndCoordinates_Y;
                    //comboBox_MotiomAnimation_Image.Text = "";

                    if (row.Cells[1].Value != null) Int32.TryParse(row.Cells[1].Value.ToString(), out StartCoordinates_X);
                    if (row.Cells[2].Value != null) Int32.TryParse(row.Cells[2].Value.ToString(), out StartCoordinates_Y);
                    if (row.Cells[3].Value != null) Int32.TryParse(row.Cells[3].Value.ToString(), out EndCoordinates_X);
                    if (row.Cells[4].Value != null) Int32.TryParse(row.Cells[4].Value.ToString(), out EndCoordinates_Y);

                    //numericUpDown_MotiomAnimation_StartCoordinates_X.Value = StartCoordinates_X;
                    //numericUpDown_MotiomAnimation_StartCoordinates_Y.Value = StartCoordinates_Y;
                    //numericUpDown_MotiomAnimation_EndCoordinates_X.Value = EndCoordinates_X;
                    //numericUpDown_MotiomAnimation_EndCoordinates_Y.Value = EndCoordinates_Y;

                    //if (row.Cells[5].Value != null && Int32.TryParse(row.Cells[5].Value.ToString(), out ImageIndex))
                    //{
                    //    comboBoxSetText(comboBox_MotiomAnimation_Image, ImageIndex);
                    //}
                    //else
                    //{
                    //    comboBox_MotiomAnimation_Image.Text = "";
                    //}

                    MotiomAnimation_Update = false;

                    PreviewImage();
                    JSON_write();
                }
            }
        }

        private void вставитьКонечныеКоординатыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Try to cast the sender to a ToolStripItem
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    // Get the control that is displaying this context menu
                    Control sourceControl = owner.SourceControl;
                    DataGridView dataGridView = sourceControl as DataGridView;
                    DataGridViewRow row = dataGridView.CurrentRow;
                    row.Cells[3].Value = MouseСoordinates.X;
                    row.Cells[4].Value = MouseСoordinates.Y;

                    // копируем данные в поля для редактирования
                    MotiomAnimation_Update = true;
                    int StartCoordinates_X = 0;
                    int StartCoordinates_Y = 0;
                    int EndCoordinates_X = 0;
                    int EndCoordinates_Y = 0;
                    int ImageIndex = 0;
                    //numericUpDown_MotiomAnimation_StartCoordinates_X.Value = StartCoordinates_X;
                    //numericUpDown_MotiomAnimation_StartCoordinates_Y.Value = StartCoordinates_Y;
                    //numericUpDown_MotiomAnimation_EndCoordinates_X.Value = EndCoordinates_X;
                    //numericUpDown_MotiomAnimation_EndCoordinates_Y.Value = EndCoordinates_Y;
                    //comboBox_MotiomAnimation_Image.Text = "";

                    if (row.Cells[1].Value != null) Int32.TryParse(row.Cells[1].Value.ToString(), out StartCoordinates_X);
                    if (row.Cells[2].Value != null) Int32.TryParse(row.Cells[2].Value.ToString(), out StartCoordinates_Y);
                    if (row.Cells[3].Value != null) Int32.TryParse(row.Cells[3].Value.ToString(), out EndCoordinates_X);
                    if (row.Cells[4].Value != null) Int32.TryParse(row.Cells[4].Value.ToString(), out EndCoordinates_Y);

                    //numericUpDown_MotiomAnimation_StartCoordinates_X.Value = StartCoordinates_X;
                    //numericUpDown_MotiomAnimation_StartCoordinates_Y.Value = StartCoordinates_Y;
                    //numericUpDown_MotiomAnimation_EndCoordinates_X.Value = EndCoordinates_X;
                    //numericUpDown_MotiomAnimation_EndCoordinates_Y.Value = EndCoordinates_Y;

                    //if (row.Cells[5].Value != null && Int32.TryParse(row.Cells[5].Value.ToString(), out ImageIndex))
                    //{
                    //    comboBoxSetText(comboBox_MotiomAnimation_Image, ImageIndex);
                    //}
                    //else
                    //{
                    //    comboBox_MotiomAnimation_Image.Text = "";
                    //}

                    MotiomAnimation_Update = false;

                    PreviewImage();
                    JSON_write();
                }
            }
        }
        private void button_ShowAnimation_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(Convert.ToInt32(454), Convert.ToInt32(454), PixelFormat.Format32bppArgb);
            Bitmap mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gtr_2.png");
            if (radioButton_GTS2.Checked)
            {
                bitmap = new Bitmap(Convert.ToInt32(348), Convert.ToInt32(442), PixelFormat.Format32bppArgb);
                mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gts_2.png");
            }
            Graphics gPanel = Graphics.FromImage(bitmap);
            int link = radioButton_ScreenNormal.Checked ? 0 : 1;
            PreviewToBitmap(gPanel, 1.0f, false, false, false, false, false, false, false, false, false, false, link);

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)       // Ctrl-S Save
            {
                // Do what you want here
                if (FileName != null)
                {
                    string fullfilename = Path.Combine(FullFileDir, FileName);
                    if (File.Exists(fullfilename))
                    {
                        save_JSON_File(fullfilename, richTextBox_JsonText.Text);
                    };

                    JSON_Modified = false;
                    FormText();
                    if (checkBox_JsonWarnings.Checked) jsonWarnings(fullfilename);
                }
                else
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.InitialDirectory = FullFileDir;
                    saveFileDialog.FileName = FileName; if (FileName == null || FileName.Length == 0)
                    {
                        if (FullFileDir != null && FullFileDir.Length > 3)
                        {
                            saveFileDialog.FileName = Path.GetFileName(FullFileDir);
                        }
                    }
                    saveFileDialog.Filter = Properties.FormStrings.FilterJson;

                    //openFileDialog.Filter = "Json files (*.json) | *.json";
                    saveFileDialog.RestoreDirectory = true;
                    saveFileDialog.Title = Properties.FormStrings.Dialog_Title_Pack;
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string fullfilename = saveFileDialog.FileName;
                        save_JSON_File(fullfilename, richTextBox_JsonText.Text);

                        FileName = Path.GetFileName(fullfilename);
                        FullFileDir = Path.GetDirectoryName(fullfilename);
                        JSON_Modified = false;
                        FormText();
                        if (checkBox_JsonWarnings.Checked) jsonWarnings(fullfilename);
                    }
                }
                e.SuppressKeyPress = true;  // Stops other controls on the form receiving event.
            }
        }

        private void comboBox_Animation_Preview_Speed_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Settings_Load) return;

            Program_Settings.Animation_Preview_Speed = comboBox_Animation_Preview_Speed.SelectedIndex;
            //string JSON_String = JObject.FromObject(Program_Settings).ToString();
            string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if(e.TabPage.Name == "tabPageConverting")
            {
                if (radioButton_GTR2.Checked)
                {
                    radioButton_ConvertingInput_GTR47.Checked = true;
                    numericUpDown_ConvertingInput_Custom.Value = 454;
                }
                numericUpDown_ConvertingInput_Custom.Enabled = radioButton_ConvertingInput_Custom.Checked;
            }
            if (FileName != null && FullFileDir != null)
            {
                button_Converting.Enabled = true;
                label486.Visible = false;
            }
            else
            {
                button_Converting.Enabled = false;
                label486.Visible = true;
            }
        }

        private void radioButton_ConvertingInput_Custom_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown_ConvertingInput_Custom.Enabled = radioButton_ConvertingInput_Custom.Checked;
        }

        private void radioButton_ConvertingOutput_Custom_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown_ConvertingOutput_Custom.Enabled = radioButton_ConvertingOutput_Custom.Checked;
        }

        private void button_Converting_Click(object sender, EventArgs e)
        {
            if (FileName != null && FullFileDir != null)
            {
                if (JSON_Modified) // сохранение если файл не сохранен
                {
                    DialogResult dr = MessageBox.Show(Properties.FormStrings.Message_Save_JSON_Modified_Text1 +
                            Path.GetFileNameWithoutExtension(FileName) + Properties.FormStrings.Message_Save_JSON_Modified_Text2,
                            Properties.FormStrings.Message_Save_JSON_Modified_Caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    if (dr == DialogResult.Yes)
                    {
                        string fullfilename = Path.Combine(FullFileDir, FileName);
                        save_JSON_File(fullfilename, richTextBox_JsonText.Text);
                        JSON_Modified = false;
                        FormText();
                        if (checkBox_JsonWarnings.Checked) jsonWarnings(fullfilename);
                    }
                    if (dr == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                
                int DeviceId = 40;
                string suffix = "_GTR_47";
                float scale = 1;
                if (radioButton_ConvertingOutput_GTR42.Checked)
                {
                    suffix = "_GTR_42";
                    DeviceId = 42;
                }
                if (radioButton_ConvertingOutput_TRex.Checked)
                {
                    suffix = "_T-Rex";
                    DeviceId = 52;
                }
                if (radioButton_ConvertingOutput_Verge.Checked)
                {
                    suffix = "_Verge_Lite";
                    DeviceId = 32;
                }
                if (radioButton_ConvertingOutput_Custom.Checked)
                {
                    suffix = "_Custom";
                    DeviceId = 0;
                }

                if (radioButton_ConvertingInput_GTR47.Checked)
                {
                    if (radioButton_ConvertingOutput_GTR47.Checked) scale = 454 / 454f;
                    if (radioButton_ConvertingOutput_GTR42.Checked) scale = 390 / 454f;
                    if (radioButton_ConvertingOutput_TRex.Checked) scale = 360 / 454f;
                    if (radioButton_ConvertingOutput_Verge.Checked) scale = 360 / 454f;
                    if (radioButton_ConvertingOutput_Custom.Checked)
                        scale = (float)(numericUpDown_ConvertingOutput_Custom.Value / 454);
                }
                if (radioButton_ConvertingInput_GTR42.Checked)
                {
                    if (radioButton_ConvertingOutput_GTR47.Checked) scale = 454 / 390f;
                    if (radioButton_ConvertingOutput_GTR42.Checked) scale = 390 / 390f;
                    if (radioButton_ConvertingOutput_TRex.Checked) scale = 360 / 390f;
                    if (radioButton_ConvertingOutput_Verge.Checked) scale = 360 / 390f;
                    if (radioButton_ConvertingOutput_Custom.Checked)
                        scale = (float)(numericUpDown_ConvertingOutput_Custom.Value / 390);
                }
                if (radioButton_ConvertingInput_TRex.Checked || radioButton_ConvertingInput_Verge.Checked)
                {
                    if (radioButton_ConvertingOutput_GTR47.Checked) scale = 454 / 360f;
                    if (radioButton_ConvertingOutput_GTR42.Checked) scale = 390 / 360f;
                    if (radioButton_ConvertingOutput_TRex.Checked) scale = 360 / 360f;
                    if (radioButton_ConvertingOutput_Verge.Checked) scale = 360 / 360f;
                    if (radioButton_ConvertingOutput_Custom.Checked)
                        scale = (float)(numericUpDown_ConvertingOutput_Custom.Value / 360);
                }
                if (radioButton_ConvertingInput_Custom.Checked)
                {
                    float value = (float)numericUpDown_ConvertingInput_Custom.Value;
                    if (radioButton_ConvertingOutput_GTR47.Checked) scale = 454 / value;
                    if (radioButton_ConvertingOutput_GTR42.Checked) scale = 390 / value;
                    if (radioButton_ConvertingOutput_TRex.Checked) scale = 360 / value;
                    if (radioButton_ConvertingOutput_Verge.Checked) scale = 360 / value;
                    if (radioButton_ConvertingOutput_Custom.Checked)
                        scale = (float)numericUpDown_ConvertingOutput_Custom.Value / value;
                }

                string newFullDirName = FullFileDir + suffix;
                string newDirName = Path.GetFileName(newFullDirName);
                if (Directory.Exists(newFullDirName))
                {
                    //DialogResult dr = MessageBox.Show(Properties.FormStrings.Message_Save_JSON_Modified_Text1 +
                    //    Path.GetFileNameWithoutExtension(FileName) + Properties.FormStrings.Message_Save_JSON_Modified_Text2,
                    //    Properties.FormStrings.Message_Save_JSON_Modified_Caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    DialogResult dr = MessageBox.Show(Properties.FormStrings.Message_WarningConverting_Text1
                        + newDirName + Properties.FormStrings.Message_WarningConverting_Text2,
                        Properties.FormStrings.Message_Warning_Caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        Directory.Delete(newFullDirName, true);
                    }
                    else return;
                }

                // Масштабируем изображения
                Image loadedImage = null;
                Directory.CreateDirectory(newFullDirName);
                foreach (string ImageFullName in ListImagesFullName)
                {
                    using (FileStream stream = new FileStream(ImageFullName, FileMode.Open, FileAccess.Read))
                    {
                        loadedImage = Image.FromStream(stream);
                    }
                    string fileName = Path.GetFileName(ImageFullName);
                    string newFullFileName = Path.Combine(newFullDirName, fileName);
                    Bitmap bitmap = ResizeImage(loadedImage, scale);

                    bitmap.Save(newFullFileName, ImageFormat.Png);
                }
                loadedImage = null;

                JSON_Scale(scale, DeviceId);

                string newFullFileNameJson = Path.Combine(newFullDirName,
                    Path.GetFileNameWithoutExtension(FileName) + suffix + ".json");
                string newJson = JsonConvert.SerializeObject(Watch_Face, Formatting.Indented, new JsonSerializerSettings
                {
                    //DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
                File.WriteAllText(newFullFileNameJson, newJson, Encoding.UTF8);

                LoadJsonAndImage(newFullFileNameJson);

                MessageBox.Show(Properties.FormStrings.Message_ConvertingCompleted_Text,
                        Properties.FormStrings.Message_Warning_Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //MessageBox.Show(Properties.FormStrings.Message_ConvertingCompleted_Text);
            }
        }

        /// <summary>
        /// Масштабирование изображения
        /// </summary>
        /// <param name="image">Исходное изображение</param>
        /// <param name="scale">Масштаб</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, float scale)
        {
            int width = (int)Math.Round(image.Width * scale);
            int height = (int)Math.Round(image.Height * scale);
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private void JSON_Scale(float scale, int DeviceId)
        {
            //if (Watch_Face == null) return;
            //if (DeviceId != 0)
            //{
            //    if (Watch_Face.Info == null) Watch_Face.Info = new Device_Id();
            //    Watch_Face.Info.DeviceId = DeviceId;
            //}

            //#region Time
            //if (Watch_Face.Time != null)
            //{
            //    if (Watch_Face.Time.Hours != null)
            //    {
            //        if (Watch_Face.Time.Hours.Tens != null)
            //        {
            //            Watch_Face.Time.Hours.Tens.X = (int)Math.Round(Watch_Face.Time.Hours.Tens.X * scale);
            //            Watch_Face.Time.Hours.Tens.Y = (int)Math.Round(Watch_Face.Time.Hours.Tens.Y * scale); 
            //        }

            //        if (Watch_Face.Time.Hours.Ones != null)
            //        {
            //            Watch_Face.Time.Hours.Ones.X = (int)Math.Round(Watch_Face.Time.Hours.Ones.X * scale);
            //            Watch_Face.Time.Hours.Ones.Y = (int)Math.Round(Watch_Face.Time.Hours.Ones.Y * scale); 
            //        }
            //    }

            //    if (Watch_Face.Time.Minutes != null)
            //    {
            //        if (Watch_Face.Time.Minutes.Tens != null)
            //        {
            //            Watch_Face.Time.Minutes.Tens.X = (int)Math.Round(Watch_Face.Time.Minutes.Tens.X * scale);
            //            Watch_Face.Time.Minutes.Tens.Y = (int)Math.Round(Watch_Face.Time.Minutes.Tens.Y * scale); 
            //        }

            //        if (Watch_Face.Time.Minutes.Ones != null)
            //        {
            //            Watch_Face.Time.Minutes.Ones.X = (int)Math.Round(Watch_Face.Time.Minutes.Ones.X * scale);
            //            Watch_Face.Time.Minutes.Ones.Y = (int)Math.Round(Watch_Face.Time.Minutes.Ones.Y * scale); 
            //        }
            //    }

            //    if (Watch_Face.Time.Seconds != null)
            //    {
            //        if (Watch_Face.Time.Seconds.Tens != null)
            //        {
            //            Watch_Face.Time.Seconds.Tens.X = (int)Math.Round(Watch_Face.Time.Seconds.Tens.X * scale);
            //            Watch_Face.Time.Seconds.Tens.Y = (int)Math.Round(Watch_Face.Time.Seconds.Tens.Y * scale); 
            //        }

            //        if (Watch_Face.Time.Seconds.Ones != null)
            //        {
            //            Watch_Face.Time.Seconds.Ones.X = (int)Math.Round(Watch_Face.Time.Seconds.Ones.X * scale);
            //            Watch_Face.Time.Seconds.Ones.Y = (int)Math.Round(Watch_Face.Time.Seconds.Ones.Y * scale); 
            //        }
            //    }

            //    if (Watch_Face.Time.Delimiter != null)
            //    {
            //        Watch_Face.Time.Delimiter.X = (int)Math.Round(Watch_Face.Time.Delimiter.X * scale);
            //        Watch_Face.Time.Delimiter.Y = (int)Math.Round(Watch_Face.Time.Delimiter.Y * scale);
            //    }

            //    if (Watch_Face.Time.AmPm != null)
            //    {
            //        Watch_Face.Time.AmPm.X = (int)Math.Round(Watch_Face.Time.AmPm.X * scale);
            //        Watch_Face.Time.AmPm.Y = (int)Math.Round(Watch_Face.Time.AmPm.Y * scale);
            //    }
            //}
            //#endregion

            //#region Date
            //if (Watch_Face.Date != null)
            //{
            //    if (Watch_Face.Date.WeekDay != null)
            //    {
            //        Watch_Face.Date.WeekDay.X = (int)Math.Round(Watch_Face.Date.WeekDay.X * scale);
            //        Watch_Face.Date.WeekDay.Y = (int)Math.Round(Watch_Face.Date.WeekDay.Y * scale);
            //    }

            //    if ((Watch_Face.Date.WeekDayProgress != null) && (Watch_Face.Date.WeekDayProgress.Coordinates != null))
            //    {
            //        foreach (Coordinates coordinates in Watch_Face.Date.WeekDayProgress.Coordinates)
            //        {
            //            coordinates.X = (int)Math.Round(coordinates.X * scale);
            //            coordinates.Y = (int)Math.Round(coordinates.Y * scale);
            //        }
            //    }

            //    if (Watch_Face.Date.MonthAndDay != null)
            //    {
            //        if ((Watch_Face.Date.MonthAndDay.OneLine != null) && (Watch_Face.Date.MonthAndDay.OneLine.Number != null))
            //        {
            //            Watch_Face.Date.MonthAndDay.OneLine.Number.TopLeftX =
            //                (int)Math.Round(Watch_Face.Date.MonthAndDay.OneLine.Number.TopLeftX * scale);
            //            Watch_Face.Date.MonthAndDay.OneLine.Number.TopLeftY =
            //                (int)Math.Round(Watch_Face.Date.MonthAndDay.OneLine.Number.TopLeftY * scale);
            //            Watch_Face.Date.MonthAndDay.OneLine.Number.BottomRightX =
            //                (int)Math.Round(Watch_Face.Date.MonthAndDay.OneLine.Number.BottomRightX * scale);
            //            Watch_Face.Date.MonthAndDay.OneLine.Number.BottomRightY =
            //                (int)Math.Round(Watch_Face.Date.MonthAndDay.OneLine.Number.BottomRightY * scale);
            //            Watch_Face.Date.MonthAndDay.OneLine.Number.Spacing =
            //                (int)Math.Round(Watch_Face.Date.MonthAndDay.OneLine.Number.Spacing * scale);
            //        }

            //        if (Watch_Face.Date.MonthAndDay.Separate != null)
            //        {
            //            if (Watch_Face.Date.MonthAndDay.Separate.Day != null)
            //            {
            //                Watch_Face.Date.MonthAndDay.Separate.Day.TopLeftX =
            //                     (int)Math.Round(Watch_Face.Date.MonthAndDay.Separate.Day.TopLeftX * scale);
            //                Watch_Face.Date.MonthAndDay.Separate.Day.TopLeftY =
            //                     (int)Math.Round(Watch_Face.Date.MonthAndDay.Separate.Day.TopLeftY * scale);
            //                Watch_Face.Date.MonthAndDay.Separate.Day.BottomRightX =
            //                     (int)Math.Round(Watch_Face.Date.MonthAndDay.Separate.Day.BottomRightX * scale);
            //                Watch_Face.Date.MonthAndDay.Separate.Day.BottomRightY =
            //                     (int)Math.Round(Watch_Face.Date.MonthAndDay.Separate.Day.BottomRightY * scale);
            //                Watch_Face.Date.MonthAndDay.Separate.Day.Spacing =
            //                     (int)Math.Round(Watch_Face.Date.MonthAndDay.Separate.Day.Spacing * scale);
            //            }

            //            if (Watch_Face.Date.MonthAndDay.Separate.Month != null)
            //            {
            //                Watch_Face.Date.MonthAndDay.Separate.Month.TopLeftX =
            //                    (int)Math.Round(Watch_Face.Date.MonthAndDay.Separate.Month.TopLeftX * scale);
            //                Watch_Face.Date.MonthAndDay.Separate.Month.TopLeftY =
            //                    (int)Math.Round(Watch_Face.Date.MonthAndDay.Separate.Month.TopLeftY * scale);
            //                Watch_Face.Date.MonthAndDay.Separate.Month.BottomRightX =
            //                    (int)Math.Round(Watch_Face.Date.MonthAndDay.Separate.Month.BottomRightX * scale);
            //                Watch_Face.Date.MonthAndDay.Separate.Month.BottomRightY =
            //                    (int)Math.Round(Watch_Face.Date.MonthAndDay.Separate.Month.BottomRightY * scale);
            //                Watch_Face.Date.MonthAndDay.Separate.Month.Spacing =
            //                    (int)Math.Round(Watch_Face.Date.MonthAndDay.Separate.Month.Spacing * scale);
            //            }

            //            if (Watch_Face.Date.MonthAndDay.Separate.MonthName != null)
            //            {
            //                Watch_Face.Date.MonthAndDay.Separate.MonthName.X = 
            //                    (int)Math.Round(Watch_Face.Date.MonthAndDay.Separate.MonthName.X * scale);
            //                Watch_Face.Date.MonthAndDay.Separate.MonthName.Y = 
            //                    (int)Math.Round(Watch_Face.Date.MonthAndDay.Separate.MonthName.Y * scale);
            //            }
            //        }

            //    }

            //    if (Watch_Face.Date.Year != null)
            //    {
            //        if ((Watch_Face.Date.Year.OneLine != null) && (Watch_Face.Date.Year.OneLine.Number != null))
            //        {
            //            Watch_Face.Date.Year.OneLine.Number.TopLeftX = (int)Math.Round(Watch_Face.Date.Year.OneLine.Number.TopLeftX * scale);
            //            Watch_Face.Date.Year.OneLine.Number.TopLeftY = (int)Math.Round(Watch_Face.Date.Year.OneLine.Number.TopLeftY * scale);
            //            Watch_Face.Date.Year.OneLine.Number.BottomRightX = (int)Math.Round(Watch_Face.Date.Year.OneLine.Number.BottomRightX * scale);
            //            Watch_Face.Date.Year.OneLine.Number.BottomRightY = (int)Math.Round(Watch_Face.Date.Year.OneLine.Number.BottomRightY * scale);
            //            Watch_Face.Date.Year.OneLine.Number.Spacing = (int)Math.Round(Watch_Face.Date.Year.OneLine.Number.Spacing * scale);
            //        }
            //    }
            //}
            //#endregion

            //#region AnalogDate
            //if (Watch_Face.DaysProgress != null)
            //{
            //    if ((Watch_Face.DaysProgress.UnknownField2 != null) && (Watch_Face.DaysProgress.UnknownField2.Image != null))
            //    {
            //        Watch_Face.DaysProgress.UnknownField2.Image.X = (int)Math.Round(Watch_Face.DaysProgress.UnknownField2.Image.X * scale);
            //        Watch_Face.DaysProgress.UnknownField2.Image.Y = (int)Math.Round(Watch_Face.DaysProgress.UnknownField2.Image.Y * scale);
            //        if (Watch_Face.DaysProgress.UnknownField2.CenterOffset != null)
            //        {
            //            Watch_Face.DaysProgress.UnknownField2.CenterOffset.X =
            //                (int)Math.Round(Watch_Face.DaysProgress.UnknownField2.CenterOffset.X * scale);
            //            Watch_Face.DaysProgress.UnknownField2.CenterOffset.Y = 
            //                (int)Math.Round(Watch_Face.DaysProgress.UnknownField2.CenterOffset.Y * scale);

            //        }
            //    }

            //    if ((Watch_Face.DaysProgress.AnalogDOW != null) && (Watch_Face.DaysProgress.AnalogDOW.Image != null))
            //    {
            //        Watch_Face.DaysProgress.AnalogDOW.Image.X = (int)Math.Round(Watch_Face.DaysProgress.AnalogDOW.Image.X * scale);
            //        Watch_Face.DaysProgress.AnalogDOW.Image.Y = (int)Math.Round(Watch_Face.DaysProgress.AnalogDOW.Image.Y * scale);
            //        if (Watch_Face.DaysProgress.AnalogDOW.CenterOffset != null)
            //        {
            //            Watch_Face.DaysProgress.AnalogDOW.CenterOffset.X =
            //                (int)Math.Round(Watch_Face.DaysProgress.AnalogDOW.CenterOffset.X * scale);
            //            Watch_Face.DaysProgress.AnalogDOW.CenterOffset.Y =
            //                (int)Math.Round(Watch_Face.DaysProgress.AnalogDOW.CenterOffset.Y * scale);

            //        }
            //    }

            //    if ((Watch_Face.DaysProgress.AnalogMonth != null) && (Watch_Face.DaysProgress.AnalogMonth.Image != null))
            //    {
            //        Watch_Face.DaysProgress.AnalogMonth.Image.X = (int)Math.Round(Watch_Face.DaysProgress.AnalogMonth.Image.X * scale);
            //        Watch_Face.DaysProgress.AnalogMonth.Image.Y = (int)Math.Round(Watch_Face.DaysProgress.AnalogMonth.Image.Y * scale);
            //        if (Watch_Face.DaysProgress.AnalogMonth.CenterOffset != null)
            //        {
            //            Watch_Face.DaysProgress.AnalogMonth.CenterOffset.X = (int)Math.Round(Watch_Face.DaysProgress.AnalogMonth.CenterOffset.X * scale);
            //            Watch_Face.DaysProgress.AnalogMonth.CenterOffset.Y = (int)Math.Round(Watch_Face.DaysProgress.AnalogMonth.CenterOffset.Y * scale);
            //        }
            //    }

            //}
            //#endregion

            //#region StepsProgress
            //if (Watch_Face.StepsProgress != null)
            //{
            //    if (Watch_Face.StepsProgress.WeekDayProgressBar != null)
            //    {
            //        Watch_Face.StepsProgress.WeekDayProgressBar.CenterX = (int)Math.Round(Watch_Face.StepsProgress.WeekDayProgressBar.CenterX * scale);
            //        Watch_Face.StepsProgress.WeekDayProgressBar.CenterY = (int)Math.Round(Watch_Face.StepsProgress.WeekDayProgressBar.CenterY * scale);
            //        Watch_Face.StepsProgress.WeekDayProgressBar.RadiusX = (int)Math.Round(Watch_Face.StepsProgress.WeekDayProgressBar.RadiusX * scale);
            //        Watch_Face.StepsProgress.WeekDayProgressBar.RadiusY = (int)Math.Round(Watch_Face.StepsProgress.WeekDayProgressBar.RadiusY * scale);
            //        Watch_Face.StepsProgress.WeekDayProgressBar.Width = (int)Math.Round(Watch_Face.StepsProgress.WeekDayProgressBar.Width * scale);
            //        if (Watch_Face.StepsProgress.WeekDayProgressBar.ImageIndex != null)
            //        {
            //            int x = 0;
            //            int y = 0;
            //            Color new_color = ColorRead(Watch_Face.StepsProgress.WeekDayProgressBar.Color);
            //            ColorToCoodinates(new_color, out x, out y);
            //            x = (int)Math.Round(x * scale);
            //            y = (int)Math.Round(y * scale);
            //            string colorStr = CoodinatesToColor(x, y);
            //            Watch_Face.StepsProgress.WeekDayProgressBar.Color = colorStr;
            //        }
            //    }

            //    if ((Watch_Face.StepsProgress.WeekDayClockHand != null) && (Watch_Face.StepsProgress.WeekDayClockHand.Image != null))
            //    {
            //        Watch_Face.StepsProgress.WeekDayClockHand.Image.X = (int)Math.Round(Watch_Face.StepsProgress.WeekDayClockHand.Image.X * scale);
            //        Watch_Face.StepsProgress.WeekDayClockHand.Image.Y = (int)Math.Round(Watch_Face.StepsProgress.WeekDayClockHand.Image.Y * scale);
            //        if (Watch_Face.StepsProgress.WeekDayClockHand.CenterOffset != null)
            //        {
            //            Watch_Face.StepsProgress.WeekDayClockHand.CenterOffset.X =
            //                (int)Math.Round(Watch_Face.StepsProgress.WeekDayClockHand.CenterOffset.X * scale);
            //            Watch_Face.StepsProgress.WeekDayClockHand.CenterOffset.Y =
            //                (int)Math.Round(Watch_Face.StepsProgress.WeekDayClockHand.CenterOffset.Y * scale);
            //        }
            //    }

            //    if ((Watch_Face.StepsProgress.Sliced != null) && (Watch_Face.StepsProgress.Sliced.Coordinates != null))
            //    {
            //        foreach (Coordinates coordinates in Watch_Face.StepsProgress.Sliced.Coordinates)
            //        {
            //            coordinates.X = (int)Math.Round(coordinates.X * scale);
            //            coordinates.Y = (int)Math.Round(coordinates.Y * scale);
            //        }
            //    }
            //}
            //#endregion

            //#region Activity
            //if (Watch_Face.Activity != null)
            //{
            //    if ((Watch_Face.Activity.StepsGoal != null))
            //    {
            //        Watch_Face.Activity.StepsGoal.TopLeftX = (int)Math.Round(Watch_Face.Activity.StepsGoal.TopLeftX * scale);
            //        Watch_Face.Activity.StepsGoal.TopLeftY = (int)Math.Round(Watch_Face.Activity.StepsGoal.TopLeftY * scale);
            //        Watch_Face.Activity.StepsGoal.BottomRightX = (int)Math.Round(Watch_Face.Activity.StepsGoal.BottomRightX * scale);
            //        Watch_Face.Activity.StepsGoal.BottomRightY = (int)Math.Round(Watch_Face.Activity.StepsGoal.BottomRightY * scale);
            //        Watch_Face.Activity.StepsGoal.Spacing = (int)Math.Round(Watch_Face.Activity.StepsGoal.Spacing * scale);
            //    }

            //    if ((Watch_Face.Activity.Steps != null) && (Watch_Face.Activity.Steps.Step != null))
            //    {
            //        Watch_Face.Activity.Steps.Step.TopLeftX = (int)Math.Round(Watch_Face.Activity.Steps.Step.TopLeftX * scale);
            //        Watch_Face.Activity.Steps.Step.TopLeftY = (int)Math.Round(Watch_Face.Activity.Steps.Step.TopLeftY * scale);
            //        Watch_Face.Activity.Steps.Step.BottomRightX = (int)Math.Round(Watch_Face.Activity.Steps.Step.BottomRightX * scale);
            //        Watch_Face.Activity.Steps.Step.BottomRightY = (int)Math.Round(Watch_Face.Activity.Steps.Step.BottomRightY * scale);
            //        Watch_Face.Activity.Steps.Step.Spacing = (int)Math.Round(Watch_Face.Activity.Steps.Step.Spacing * scale);
            //    }

            //    if ((Watch_Face.Activity.Distance != null) && (Watch_Face.Activity.Distance.Number != null))
            //    {
            //        Watch_Face.Activity.Distance.Number.TopLeftX = (int)Math.Round(Watch_Face.Activity.Distance.Number.TopLeftX * scale);
            //        Watch_Face.Activity.Distance.Number.TopLeftY = (int)Math.Round(Watch_Face.Activity.Distance.Number.TopLeftY * scale);
            //        Watch_Face.Activity.Distance.Number.BottomRightX = (int)Math.Round(Watch_Face.Activity.Distance.Number.BottomRightX * scale);
            //        Watch_Face.Activity.Distance.Number.BottomRightY = (int)Math.Round(Watch_Face.Activity.Distance.Number.BottomRightY * scale);
            //        Watch_Face.Activity.Distance.Number.Spacing = (int)Math.Round(Watch_Face.Activity.Distance.Number.Spacing * scale);
            //    }

            //    if (Watch_Face.Activity.HeartRate != null)
            //    {
            //        Watch_Face.Activity.HeartRate.TopLeftX = (int)Math.Round(Watch_Face.Activity.HeartRate.TopLeftX * scale);
            //        Watch_Face.Activity.HeartRate.TopLeftY = (int)Math.Round(Watch_Face.Activity.HeartRate.TopLeftY * scale);
            //        Watch_Face.Activity.HeartRate.BottomRightX = (int)Math.Round(Watch_Face.Activity.HeartRate.BottomRightX * scale);
            //        Watch_Face.Activity.HeartRate.BottomRightY = (int)Math.Round(Watch_Face.Activity.HeartRate.BottomRightY * scale);
            //        Watch_Face.Activity.HeartRate.Spacing = (int)Math.Round(Watch_Face.Activity.HeartRate.Spacing * scale);
            //    }

            //    if (Watch_Face.Activity.PulseMeter != null)
            //    {
            //        Watch_Face.Activity.PulseMeter.CenterX = (int)Math.Round(Watch_Face.Activity.PulseMeter.CenterX * scale);
            //        Watch_Face.Activity.PulseMeter.CenterY = (int)Math.Round(Watch_Face.Activity.PulseMeter.CenterY * scale);
            //        Watch_Face.Activity.PulseMeter.RadiusX = (int)Math.Round(Watch_Face.Activity.PulseMeter.RadiusX * scale);
            //        Watch_Face.Activity.PulseMeter.RadiusY = (int)Math.Round(Watch_Face.Activity.PulseMeter.RadiusY * scale);
            //        Watch_Face.Activity.PulseMeter.Width = (int)Math.Round(Watch_Face.Activity.PulseMeter.Width * scale);
            //        if (Watch_Face.Activity.PulseMeter.ImageIndex != null)
            //        {
            //            int x = 0;
            //            int y = 0;
            //            Color new_color = ColorRead(Watch_Face.Activity.PulseMeter.Color);
            //            ColorToCoodinates(new_color, out x, out y);
            //            x = (int)Math.Round(x * scale);
            //            y = (int)Math.Round(y * scale);
            //            string colorStr = CoodinatesToColor(x, y);
            //            Watch_Face.Activity.PulseMeter.Color = colorStr;
            //        }
            //    }

            //    if ((Watch_Face.Activity.PulseGraph != null) &&
            //        (Watch_Face.Activity.PulseGraph.WeekDayClockHand != null) &&
            //        (Watch_Face.Activity.PulseGraph.WeekDayClockHand.Image != null))
            //    {
            //        Watch_Face.Activity.PulseGraph.WeekDayClockHand.Image.X = (int)Math.Round(Watch_Face.Activity.PulseGraph.WeekDayClockHand.Image.X * scale);
            //        Watch_Face.Activity.PulseGraph.WeekDayClockHand.Image.Y = (int)Math.Round(Watch_Face.Activity.PulseGraph.WeekDayClockHand.Image.Y * scale);
            //        if (Watch_Face.Activity.PulseGraph.WeekDayClockHand.CenterOffset != null)
            //        {
            //            Watch_Face.Activity.PulseGraph.WeekDayClockHand.CenterOffset.X =
            //                (int)Math.Round(Watch_Face.Activity.PulseGraph.WeekDayClockHand.CenterOffset.X * scale);
            //            Watch_Face.Activity.PulseGraph.WeekDayClockHand.CenterOffset.Y =
            //                (int)Math.Round(Watch_Face.Activity.PulseGraph.WeekDayClockHand.CenterOffset.Y * scale);
            //        }
            //    }

            //    if ((Watch_Face.Activity.ColouredSquares != null) &&
            //        (Watch_Face.Activity.ColouredSquares.Coordinates != null))
            //    {
            //        foreach (Coordinates coordinates in Watch_Face.Activity.ColouredSquares.Coordinates)
            //        {
            //            coordinates.X = (int)Math.Round(coordinates.X * scale);
            //            coordinates.Y = (int)Math.Round(coordinates.Y * scale);
            //        }
            //    }

            //    if (Watch_Face.Activity.Calories != null)
            //    {
            //        Watch_Face.Activity.Calories.TopLeftX = (int)Math.Round(Watch_Face.Activity.Calories.TopLeftX * scale);
            //        Watch_Face.Activity.Calories.TopLeftY = (int)Math.Round(Watch_Face.Activity.Calories.TopLeftY * scale);
            //        Watch_Face.Activity.Calories.BottomRightX = (int)Math.Round(Watch_Face.Activity.Calories.BottomRightX * scale);
            //        Watch_Face.Activity.Calories.BottomRightY = (int)Math.Round(Watch_Face.Activity.Calories.BottomRightY * scale);
            //        Watch_Face.Activity.Calories.Spacing = (int)Math.Round(Watch_Face.Activity.Calories.Spacing * scale);
            //    }
                
            //    if (Watch_Face.Activity.CaloriesGraph != null && Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar != null)
            //    {
            //        Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.CenterX = (int)Math.Round(Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.CenterX * scale);
            //        Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.CenterY = (int)Math.Round(Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.CenterY * scale);
            //        Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.RadiusX = (int)Math.Round(Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.RadiusX * scale);
            //        Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.RadiusY = (int)Math.Round(Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.RadiusY * scale);
            //        Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.Width = (int)Math.Round(Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.Width * scale);
            //        if (Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.ImageIndex != null)
            //        {
            //            int x = 0;
            //            int y = 0;
            //            Color new_color = ColorRead(Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.Color);
            //            ColorToCoodinates(new_color, out x, out y);
            //            x = (int)Math.Round(x * scale);
            //            y = (int)Math.Round(y * scale);
            //            string colorStr = CoodinatesToColor(x, y);
            //            Watch_Face.Activity.CaloriesGraph.WeekDayProgressBar.Color = colorStr;
            //        }
            //    }

            //    if ((Watch_Face.Activity.CaloriesGraph != null) &&
            //        (Watch_Face.Activity.CaloriesGraph.WeekDayClockHand != null) &&
            //        (Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.Image != null))
            //    {
            //        Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.Image.X = (int)Math.Round(Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.Image.X * scale);
            //        Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.Image.Y = (int)Math.Round(Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.Image.Y * scale);
            //        if (Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.CenterOffset != null)
            //        {
            //            Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.CenterOffset.X =
            //                (int)Math.Round(Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.CenterOffset.X * scale);
            //            Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.CenterOffset.Y =
            //                (int)Math.Round(Watch_Face.Activity.CaloriesGraph.WeekDayClockHand.CenterOffset.Y * scale);
            //        }
            //    }

            //    if (Watch_Face.Activity.StarImage != null)
            //    {
            //        Watch_Face.Activity.StarImage.X = (int)Math.Round(Watch_Face.Activity.StarImage.X * scale);
            //        Watch_Face.Activity.StarImage.Y = (int)Math.Round(Watch_Face.Activity.StarImage.Y * scale);
            //    }
            //}
            //#endregion

            //#region Status
            //if (Watch_Face.Status != null)
            //{
            //    if (Watch_Face.Status.Bluetooth != null)
            //    {
            //        if (Watch_Face.Status.Bluetooth.Coordinates != null)
            //        {
            //            Watch_Face.Status.Bluetooth.Coordinates.X = (int)Math.Round(Watch_Face.Status.Bluetooth.Coordinates.X * scale);
            //            Watch_Face.Status.Bluetooth.Coordinates.Y = (int)Math.Round(Watch_Face.Status.Bluetooth.Coordinates.Y * scale);
            //        }
            //    }

            //    if (Watch_Face.Status.Alarm != null)
            //    {
            //        if (Watch_Face.Status.Alarm.Coordinates != null)
            //        {
            //            Watch_Face.Status.Alarm.Coordinates.X = (int)Math.Round(Watch_Face.Status.Alarm.Coordinates.X * scale);
            //            Watch_Face.Status.Alarm.Coordinates.Y = (int)Math.Round(Watch_Face.Status.Alarm.Coordinates.Y * scale);
            //        }
            //    }

            //    if (Watch_Face.Status.Lock != null)
            //    {
            //        if (Watch_Face.Status.Lock.Coordinates != null)
            //        {
            //            Watch_Face.Status.Lock.Coordinates.X = (int)Math.Round(Watch_Face.Status.Lock.Coordinates.X * scale);
            //            Watch_Face.Status.Lock.Coordinates.Y = (int)Math.Round(Watch_Face.Status.Lock.Coordinates.Y * scale);
            //        }
            //    }

            //    if (Watch_Face.Status.DoNotDisturb != null)
            //    {
            //        if (Watch_Face.Status.DoNotDisturb.Coordinates != null)
            //        {
            //            Watch_Face.Status.DoNotDisturb.Coordinates.X = (int)Math.Round(Watch_Face.Status.DoNotDisturb.Coordinates.X * scale);
            //            Watch_Face.Status.DoNotDisturb.Coordinates.Y = (int)Math.Round(Watch_Face.Status.DoNotDisturb.Coordinates.Y * scale);
            //        }
            //    }
            //}
            //#endregion

            //#region Battery
            //if (Watch_Face.Battery != null)
            //{
            //    if (Watch_Face.Battery.Text != null)
            //    {
            //        Watch_Face.Battery.Text.TopLeftX = (int)Math.Round(Watch_Face.Battery.Text.TopLeftX * scale);
            //        Watch_Face.Battery.Text.TopLeftY = (int)Math.Round(Watch_Face.Battery.Text.TopLeftY * scale);
            //        Watch_Face.Battery.Text.BottomRightX = (int)Math.Round(Watch_Face.Battery.Text.BottomRightX * scale);
            //        Watch_Face.Battery.Text.BottomRightY = (int)Math.Round(Watch_Face.Battery.Text.BottomRightY * scale);
            //        Watch_Face.Battery.Text.Spacing = (int)Math.Round(Watch_Face.Battery.Text.Spacing * scale);
            //    }

            //    if (Watch_Face.Battery.Images != null)
            //    {
            //        Watch_Face.Battery.Images.X = (int)Math.Round(Watch_Face.Battery.Images.X * scale);
            //        Watch_Face.Battery.Images.Y = (int)Math.Round(Watch_Face.Battery.Images.Y * scale);
            //    }

            //    if ((Watch_Face.Battery.Unknown4 != null) && (Watch_Face.Battery.Unknown4.Image != null))
            //    {
            //        Watch_Face.Battery.Unknown4.Image.X = (int)Math.Round(Watch_Face.Battery.Unknown4.Image.X * scale);
            //        Watch_Face.Battery.Unknown4.Image.Y = (int)Math.Round(Watch_Face.Battery.Unknown4.Image.Y * scale);
            //        if (Watch_Face.Battery.Unknown4.CenterOffset != null)
            //        {
            //            Watch_Face.Battery.Unknown4.CenterOffset.X =
            //                 (int)Math.Round(Watch_Face.Battery.Unknown4.CenterOffset.X * scale);
            //            Watch_Face.Battery.Unknown4.CenterOffset.Y =
            //                 (int)Math.Round(Watch_Face.Battery.Unknown4.CenterOffset.Y * scale);
            //        }
            //    }

            //    if (Watch_Face.Battery.Percent != null)
            //    {
            //        Watch_Face.Battery.Percent.X = (int)Math.Round(Watch_Face.Battery.Percent.X * scale);
            //        Watch_Face.Battery.Percent.Y = (int)Math.Round(Watch_Face.Battery.Percent.Y * scale);
            //    }

            //    if (Watch_Face.Battery.Scale != null)
            //    {
            //        Watch_Face.Battery.Scale.CenterX = (int)Math.Round(Watch_Face.Battery.Scale.CenterX * scale);
            //        Watch_Face.Battery.Scale.CenterY = (int)Math.Round(Watch_Face.Battery.Scale.CenterY * scale);
            //        Watch_Face.Battery.Scale.RadiusX = (int)Math.Round(Watch_Face.Battery.Scale.RadiusX * scale);
            //        Watch_Face.Battery.Scale.RadiusY = (int)Math.Round(Watch_Face.Battery.Scale.RadiusY * scale);
            //        Watch_Face.Battery.Scale.Width = (int)Math.Round(Watch_Face.Battery.Scale.Width * scale);
            //        if (Watch_Face.Battery.Scale.ImageIndex != null)
            //        {
            //            int x = 0;
            //            int y = 0;
            //            Color new_color = ColorRead(Watch_Face.Battery.Scale.Color);
            //            ColorToCoodinates(new_color, out x, out y);
            //            x = (int)Math.Round(x * scale);
            //            y = (int)Math.Round(y * scale);
            //            string colorStr = CoodinatesToColor(x, y);
            //            Watch_Face.Battery.Scale.Color = colorStr;
            //        }
            //    }

            //    if ((Watch_Face.Battery.Icons != null) && (Watch_Face.Battery.Icons.Coordinates != null))
            //    {
            //        foreach (Coordinates coordinates in Watch_Face.Battery.Icons.Coordinates)
            //        {
            //            coordinates.X = (int)Math.Round(coordinates.X * scale);
            //            coordinates.Y = (int)Math.Round(coordinates.Y * scale);
            //        }
            //    }
            //}
            //#endregion

            //#region MonthClockHand
            //if (Watch_Face.MonthClockHand != null)
            //{
            //    if ((Watch_Face.MonthClockHand.Hours != null) && (Watch_Face.MonthClockHand.Hours.Image != null))
            //    {
            //        Watch_Face.MonthClockHand.Hours.Image.X = (int)Math.Round(Watch_Face.MonthClockHand.Hours.Image.X * scale);
            //        Watch_Face.MonthClockHand.Hours.Image.Y = (int)Math.Round(Watch_Face.MonthClockHand.Hours.Image.Y * scale);

            //        if (Watch_Face.MonthClockHand.Hours.CenterOffset != null)
            //        {
            //            Watch_Face.MonthClockHand.Hours.CenterOffset.X = 
            //                (int)Math.Round(Watch_Face.MonthClockHand.Hours.CenterOffset.X * scale);
            //            Watch_Face.MonthClockHand.Hours.CenterOffset.Y = 
            //                (int)Math.Round(Watch_Face.MonthClockHand.Hours.CenterOffset.Y * scale);
            //        }
            //    }

            //    if ((Watch_Face.MonthClockHand.Minutes != null) && (Watch_Face.MonthClockHand.Minutes.Image != null))
            //    {
            //        Watch_Face.MonthClockHand.Minutes.Image.X = (int)Math.Round(Watch_Face.MonthClockHand.Minutes.Image.X * scale);
            //        Watch_Face.MonthClockHand.Minutes.Image.Y = (int)Math.Round(Watch_Face.MonthClockHand.Minutes.Image.Y * scale);

            //        if (Watch_Face.MonthClockHand.Minutes.CenterOffset != null)
            //        {
            //            Watch_Face.MonthClockHand.Minutes.CenterOffset.X =
            //                 (int)Math.Round(Watch_Face.MonthClockHand.Minutes.CenterOffset.X * scale);
            //            Watch_Face.MonthClockHand.Minutes.CenterOffset.Y =
            //                 (int)Math.Round(Watch_Face.MonthClockHand.Minutes.CenterOffset.Y * scale);
            //        }
            //    }

            //    if ((Watch_Face.MonthClockHand.Seconds != null) && (Watch_Face.MonthClockHand.Seconds.Image != null))
            //    {
            //        Watch_Face.MonthClockHand.Seconds.Image.X = (int)Math.Round(Watch_Face.MonthClockHand.Seconds.Image.X * scale);
            //        Watch_Face.MonthClockHand.Seconds.Image.Y = (int)Math.Round(Watch_Face.MonthClockHand.Seconds.Image.Y * scale);

            //        if (Watch_Face.MonthClockHand.Seconds.CenterOffset != null)
            //        {
            //            Watch_Face.MonthClockHand.Seconds.CenterOffset.X =
            //                (int)Math.Round(Watch_Face.MonthClockHand.Seconds.CenterOffset.X * scale);
            //            Watch_Face.MonthClockHand.Seconds.CenterOffset.Y =
            //                (int)Math.Round(Watch_Face.MonthClockHand.Seconds.CenterOffset.Y * scale);
            //        }
            //    }

            //    if (Watch_Face.MonthClockHand.HourCenterImage != null)
            //    {
            //        Watch_Face.MonthClockHand.HourCenterImage.X = (int)Math.Round(Watch_Face.MonthClockHand.HourCenterImage.X * scale);
            //        Watch_Face.MonthClockHand.HourCenterImage.Y = (int)Math.Round(Watch_Face.MonthClockHand.HourCenterImage.Y * scale);
            //    }

            //    if (Watch_Face.MonthClockHand.MinCenterImage != null)
            //    {
            //        Watch_Face.MonthClockHand.MinCenterImage.X = (int)Math.Round(Watch_Face.MonthClockHand.MinCenterImage.X * scale);
            //        Watch_Face.MonthClockHand.MinCenterImage.Y = (int)Math.Round(Watch_Face.MonthClockHand.MinCenterImage.Y * scale);
            //    }

            //    if (Watch_Face.MonthClockHand.SecCenterImage != null)
            //    {
            //        Watch_Face.MonthClockHand.SecCenterImage.X = (int)Math.Round(Watch_Face.MonthClockHand.SecCenterImage.X * scale);
            //        Watch_Face.MonthClockHand.SecCenterImage.Y = (int)Math.Round(Watch_Face.MonthClockHand.SecCenterImage.Y * scale);
            //    }
            //}
            //#endregion

            //#region Weather
            //if (Watch_Face.Weather != null)
            //{
            //    if ((Watch_Face.Weather.Temperature != null) && (Watch_Face.Weather.Temperature.Current != null))
            //    {
            //        Watch_Face.Weather.Temperature.Current.TopLeftX = (int)Math.Round(Watch_Face.Weather.Temperature.Current.TopLeftX * scale);
            //        Watch_Face.Weather.Temperature.Current.TopLeftY = (int)Math.Round(Watch_Face.Weather.Temperature.Current.TopLeftY * scale);
            //        Watch_Face.Weather.Temperature.Current.BottomRightX = (int)Math.Round(Watch_Face.Weather.Temperature.Current.BottomRightX * scale);
            //        Watch_Face.Weather.Temperature.Current.BottomRightY = (int)Math.Round(Watch_Face.Weather.Temperature.Current.BottomRightY * scale);
            //        Watch_Face.Weather.Temperature.Current.Spacing = (int)Math.Round(Watch_Face.Weather.Temperature.Current.Spacing * scale);
            //    }

            //    if ((Watch_Face.Weather.Temperature != null) && (Watch_Face.Weather.Temperature.Today != null))
            //    {
            //        if ((Watch_Face.Weather.Temperature.Today.Separate != null) &&
            //            (Watch_Face.Weather.Temperature.Today.Separate.Day != null))
            //        {
            //            Watch_Face.Weather.Temperature.Today.Separate.Day.TopLeftX =
            //                (int)Math.Round(Watch_Face.Weather.Temperature.Today.Separate.Day.TopLeftX * scale);
            //            Watch_Face.Weather.Temperature.Today.Separate.Day.TopLeftY =
            //                (int)Math.Round(Watch_Face.Weather.Temperature.Today.Separate.Day.TopLeftY * scale);
            //            Watch_Face.Weather.Temperature.Today.Separate.Day.BottomRightX =
            //                (int)Math.Round(Watch_Face.Weather.Temperature.Today.Separate.Day.BottomRightX * scale);
            //            Watch_Face.Weather.Temperature.Today.Separate.Day.BottomRightY =
            //                (int)Math.Round(Watch_Face.Weather.Temperature.Today.Separate.Day.BottomRightY * scale);
            //            Watch_Face.Weather.Temperature.Today.Separate.Day.Spacing =
            //                (int)Math.Round(Watch_Face.Weather.Temperature.Today.Separate.Day.Spacing * scale);
            //        }

            //        if ((Watch_Face.Weather.Temperature.Today.Separate != null) &&
            //            (Watch_Face.Weather.Temperature.Today.Separate.Night != null))
            //        {
            //            Watch_Face.Weather.Temperature.Today.Separate.Night.TopLeftX =
            //                (int)Math.Round(Watch_Face.Weather.Temperature.Today.Separate.Night.TopLeftX * scale);
            //            Watch_Face.Weather.Temperature.Today.Separate.Night.TopLeftY =
            //                (int)Math.Round(Watch_Face.Weather.Temperature.Today.Separate.Night.TopLeftY * scale);
            //            Watch_Face.Weather.Temperature.Today.Separate.Night.BottomRightX =
            //                (int)Math.Round(Watch_Face.Weather.Temperature.Today.Separate.Night.BottomRightX * scale);
            //            Watch_Face.Weather.Temperature.Today.Separate.Night.BottomRightY =
            //                (int)Math.Round(Watch_Face.Weather.Temperature.Today.Separate.Night.BottomRightY * scale);
            //            Watch_Face.Weather.Temperature.Today.Separate.Night.Spacing =
            //                (int)Math.Round(Watch_Face.Weather.Temperature.Today.Separate.Night.Spacing * scale);
            //        }
            //    }

            //    if ((Watch_Face.Weather.Icon != null) && (Watch_Face.Weather.Icon.Images != null))
            //    {
            //        Watch_Face.Weather.Icon.Images.X = (int)Math.Round(Watch_Face.Weather.Icon.Images.X * scale);
            //        Watch_Face.Weather.Icon.Images.Y = (int)Math.Round(Watch_Face.Weather.Icon.Images.Y * scale);
            //    }
            //}
            //#endregion

            //#region Shortcuts
            //if (Watch_Face.Shortcuts != null)
            //{
            //    if (Watch_Face.Shortcuts.State != null && Watch_Face.Shortcuts.State.Element != null)
            //    {
            //        Watch_Face.Shortcuts.State.Element.TopLeftX = (int)Math.Round(Watch_Face.Shortcuts.State.Element.TopLeftX * scale);
            //        Watch_Face.Shortcuts.State.Element.TopLeftY = (int)Math.Round(Watch_Face.Shortcuts.State.Element.TopLeftY * scale);
            //        Watch_Face.Shortcuts.State.Element.Width = (int)Math.Round(Watch_Face.Shortcuts.State.Element.Width * scale);
            //        Watch_Face.Shortcuts.State.Element.Height = (int)Math.Round(Watch_Face.Shortcuts.State.Element.Height * scale);
            //    }

            //    if (Watch_Face.Shortcuts.HeartRate != null && Watch_Face.Shortcuts.HeartRate.Element != null)
            //    {
            //        Watch_Face.Shortcuts.HeartRate.Element.TopLeftX = (int)Math.Round(Watch_Face.Shortcuts.HeartRate.Element.TopLeftX * scale);
            //        Watch_Face.Shortcuts.HeartRate.Element.TopLeftY = (int)Math.Round(Watch_Face.Shortcuts.HeartRate.Element.TopLeftY * scale);
            //        Watch_Face.Shortcuts.HeartRate.Element.Width = (int)Math.Round(Watch_Face.Shortcuts.HeartRate.Element.Width * scale);
            //        Watch_Face.Shortcuts.HeartRate.Element.Height = (int)Math.Round(Watch_Face.Shortcuts.HeartRate.Element.Height * scale);
            //    }

            //    if (Watch_Face.Shortcuts.Weather != null && Watch_Face.Shortcuts.Weather.Element != null)
            //    {
            //        Watch_Face.Shortcuts.Weather.Element.TopLeftX = (int)Math.Round(Watch_Face.Shortcuts.Weather.Element.TopLeftX * scale);
            //        Watch_Face.Shortcuts.Weather.Element.TopLeftY = (int)Math.Round(Watch_Face.Shortcuts.Weather.Element.TopLeftY * scale);
            //        Watch_Face.Shortcuts.Weather.Element.Width = (int)Math.Round(Watch_Face.Shortcuts.Weather.Element.Width * scale);
            //        Watch_Face.Shortcuts.Weather.Element.Height = (int)Math.Round(Watch_Face.Shortcuts.Weather.Element.Height * scale);
            //    }

            //    if (Watch_Face.Shortcuts.Unknown4 != null && Watch_Face.Shortcuts.Unknown4.Element != null)
            //    {
            //        Watch_Face.Shortcuts.Unknown4.Element.TopLeftX = (int)Math.Round(Watch_Face.Shortcuts.Unknown4.Element.TopLeftX * scale);
            //        Watch_Face.Shortcuts.Unknown4.Element.TopLeftY = (int)Math.Round(Watch_Face.Shortcuts.Unknown4.Element.TopLeftY * scale);
            //        Watch_Face.Shortcuts.Unknown4.Element.Width = (int)Math.Round(Watch_Face.Shortcuts.Unknown4.Element.Width * scale);
            //        Watch_Face.Shortcuts.Unknown4.Element.Height = (int)Math.Round(Watch_Face.Shortcuts.Unknown4.Element.Height * scale);
            //    }
            //}
            //#endregion

            //#region Animation
            //if (Watch_Face.Unknown11 != null)
            //{
            //    // покадровая анимация
            //    if (Watch_Face.Unknown11.Unknown11_2 != null && Watch_Face.Unknown11.Unknown11_2.Unknown11d2p1 != null)
            //    {
            //        Watch_Face.Unknown11.Unknown11_2.Unknown11d2p1.X = (int)Math.Round(Watch_Face.Unknown11.Unknown11_2.Unknown11d2p1.X * scale);
            //        Watch_Face.Unknown11.Unknown11_2.Unknown11d2p1.Y = (int)Math.Round(Watch_Face.Unknown11.Unknown11_2.Unknown11d2p1.Y * scale);
            //    }

            //    // перемещение между координатами
            //    if (Watch_Face.Unknown11.Unknown11_1 != null)
            //    {
            //        foreach (MotiomAnimation MotiomAnimation in Watch_Face.Unknown11.Unknown11_1)
            //        {
            //            if (MotiomAnimation.Unknown11d1p2 != null && MotiomAnimation.Unknown11d1p3 != null)
            //            {
            //                MotiomAnimation.Unknown11d1p2.X = (int)Math.Round(MotiomAnimation.Unknown11d1p2.X * scale);
            //                MotiomAnimation.Unknown11d1p2.Y = (int)Math.Round(MotiomAnimation.Unknown11d1p2.Y * scale);
            //                MotiomAnimation.Unknown11d1p3.X = (int)Math.Round(MotiomAnimation.Unknown11d1p3.X * scale);
            //                MotiomAnimation.Unknown11d1p3.Y = (int)Math.Round(MotiomAnimation.Unknown11d1p3.Y * scale);
            //            }
            //        }
            //    }
            //}
            //#endregion
        }
        
        private void button_RefreshPreview_Click(object sender, EventArgs e)
        {
            if (FileName == null || FullFileDir == null) return;
            if (comboBox_Preview_image.SelectedIndex >= 0)
            {
                Bitmap bitmap = new Bitmap(Convert.ToInt32(454), Convert.ToInt32(454), PixelFormat.Format32bppArgb);
                Bitmap mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gtr_2.png");
                int PreviewHeight = 306;
                if (radioButton_GTS2.Checked)
                {
                    bitmap = new Bitmap(Convert.ToInt32(348), Convert.ToInt32(442), PixelFormat.Format32bppArgb);
                    mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gts_2.png");
                    PreviewHeight = 323;
                }
                Graphics gPanel = Graphics.FromImage(bitmap);
                int link = radioButton_ScreenNormal.Checked ? 0 : 1;
                PreviewToBitmap(gPanel, 1.0f, false, false, false, false, false, false, false, true, false, false, link);
                if (checkBox_crop.Checked) bitmap = ApplyMask(bitmap, mask);


                int i = comboBox_Preview_image.SelectedIndex;
                Image loadedImage = null;
                using (FileStream stream = new FileStream(ListImagesFullName[i], FileMode.Open, FileAccess.Read))
                {
                    loadedImage = Image.FromStream(stream);
                }
                float scale = (float)PreviewHeight / bitmap.Height;
                if (loadedImage.Height != PreviewHeight)
                {
                    DialogResult ResultDialog = MessageBox.Show(Properties.FormStrings.Message_WarningPreview_Text1 +
                        Environment.NewLine + Properties.FormStrings.Message_WarningPreview_Text2,
                        Properties.FormStrings.Message_Warning_Caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (ResultDialog == DialogResult.Yes) scale = (float)loadedImage.Height / bitmap.Height;
                }
                int pixelsOld = loadedImage.Width * loadedImage.Height;
                pixelsOld = pixelsOld * 4 + 20;
                bitmap = ResizeImage(bitmap, scale);
                bitmap.Save(ListImagesFullName[i], ImageFormat.Png);
                //string s = label_size.Text;
                ////s = s.Trim(new char[] { '≈', 'M' });
                //s = s.Replace("≈", "");
                //s = s.Replace("MB", "");
                //float pixels = float.Parse(s) * 1024 * 1024;
                //int pixelsNew = bitmap.Width * bitmap.Height;
                //pixelsNew = pixelsNew * 4 + 20;
                //pixels = pixels - pixelsOld + pixelsNew;
                //ShowAllFileSize(pixels);
                bitmap.Dispose();
                loadedImage.Dispose();

            }
        }

        private void button_CreatePreview_Click(object sender, EventArgs e)
        {
            if (comboBox_Preview_image.SelectedIndex >= 0) return;
            if (FileName != null && FullFileDir != null) // проект уже сохранен
            {
                // формируем картинку для предпросмотра
                Bitmap bitmap = new Bitmap(Convert.ToInt32(454), Convert.ToInt32(454), PixelFormat.Format32bppArgb);
                Bitmap mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gtr_2.png");
                int PreviewHeight = 306;
                if (radioButton_GTS2.Checked)
                {
                    bitmap = new Bitmap(Convert.ToInt32(348), Convert.ToInt32(442), PixelFormat.Format32bppArgb);
                    mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gts_2.png");
                    PreviewHeight = 323;
                }
                Graphics gPanel = Graphics.FromImage(bitmap);
                int link = radioButton_ScreenNormal.Checked ? 0 : 1;
                PreviewToBitmap(gPanel, 1.0f, false, false, false, false, false, false, false, true, false, false, link);
                if (checkBox_crop.Checked) bitmap = ApplyMask(bitmap, mask);
                float scale = (float)PreviewHeight / bitmap.Height;
                bitmap = ResizeImage(bitmap, scale);
                //bitmap.Save(ListImagesFullName[i], ImageFormat.Png);

                //string s = label_size.Text;
                ////s = s.Trim(new char[] { '≈', 'M' });
                //s = s.Replace("≈", "");
                //s = s.Replace("MB", "");
                //float pixels = float.Parse(s) * 1024 * 1024;
                //int pixelsNew = bitmap.Width * bitmap.Height;
                //pixelsNew = pixelsNew * 4 + 20;
                //pixels = pixels + pixelsNew;
                //ShowAllFileSize(pixels);
                //bitmap.Dispose();

                // определяем имя файла для сохранения и сохраняем файл
                if (ListImages[1] == "1" || ListImages[0] == "1") // файл 0001.png есть
                {
                    int i = Int32.Parse(ListImages[ListImages.Count - 1]) + 1;
                    string NamePreview = i.ToString() + ".png";
                    string PathPreview = Path.Combine(FullFileDir, NamePreview);
                    while (PathPreview.Length < ListImagesFullName[0].Length)
                    {
                        NamePreview = "0" + NamePreview;
                        PathPreview = Path.Combine(FullFileDir, NamePreview);
                    }
                    bitmap.Save(PathPreview, ImageFormat.Png);
                    string fileNameOnly = Path.GetFileNameWithoutExtension(PathPreview);
                    i = Int32.Parse(fileNameOnly);

                    PreviewView = false;
                    ListImages.Add(i.ToString());
                    ListImagesFullName.Add(PathPreview);

                    // добавляем строки в таблицу
                    //Image PreviewImage = Image.FromHbitmap(bitmap.GetHbitmap());
                    Image PreviewImage = null;
                    using (FileStream stream = new FileStream(PathPreview, FileMode.Open, FileAccess.Read))
                    {
                        PreviewImage = Image.FromStream(stream);
                    }
                    var RowNew = new DataGridViewRow();
                    DataGridViewImageCellLayout ZoomType = DataGridViewImageCellLayout.Zoom;
                    if ((bitmap.Height < 45) && (bitmap.Width < 110))
                        ZoomType = DataGridViewImageCellLayout.Normal;
                    RowNew.Cells.Add(new DataGridViewTextBoxCell() { Value = i.ToString() });
                    RowNew.Cells.Add(new DataGridViewTextBoxCell() { Value = fileNameOnly });
                    RowNew.Cells.Add(new DataGridViewImageCell()
                    {
                        Value = PreviewImage,
                        ImageLayout = ZoomType
                    });
                    RowNew.Height = 45;
                    dataGridView_ImagesList.Rows.Add(RowNew);

                    if (Watch_Face.Background == null) Watch_Face.Background = new Background();
                    Watch_Face.Background.Preview = new List<MultilangImage>();
                    MultilangImage multilangImage = new MultilangImage();
                    multilangImage.LangCode = "All";
                    multilangImage.ImageSet = new ImageSetGTR2();
                    multilangImage.ImageSet.ImageIndex = i;
                    multilangImage.ImageSet.ImagesCount = 1;
                    Watch_Face.Background.Preview.Add(multilangImage);
                    JSON_read();
                    richTextBox_JsonText.Text = JsonConvert.SerializeObject(Watch_Face, Formatting.Indented, new JsonSerializerSettings
                    {
                        //DefaultValueHandling = DefaultValueHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    JsonToTree(richTextBox_JsonText.Text);
                    PreviewView = true;
                    JSON_Modified = true;
                    FormText();
                }
                else // файла 0001.png нет
                {
                    string NamePreview = "1.png";
                    string PathPreview = Path.Combine(FullFileDir, NamePreview);
                    while (PathPreview.Length < ListImagesFullName[0].Length)
                    {
                        NamePreview = "0" + NamePreview;
                        PathPreview = Path.Combine(FullFileDir, NamePreview);
                    }
                    bitmap.Save(PathPreview, ImageFormat.Png);

                    PreviewView = false;
                    int index = 0;
                    if (ListImages[0] == "0") // файл 0000.png есть
                    {
                        index = 1;
                    }

                    ListImages.Insert(index, "1");
                    ListImagesFullName.Insert(index, PathPreview);

                    // добавляем строки в таблицу
                    string fileNameOnly = Path.GetFileNameWithoutExtension(PathPreview);
                    Image PreviewImage = null;
                    using (FileStream stream = new FileStream(PathPreview, FileMode.Open, FileAccess.Read))
                    {
                        PreviewImage = Image.FromStream(stream);
                    }
                    var RowNew = new DataGridViewRow();
                    DataGridViewImageCellLayout ZoomType = DataGridViewImageCellLayout.Zoom;
                    if ((bitmap.Height < 45) && (bitmap.Width < 110))
                        ZoomType = DataGridViewImageCellLayout.Normal;
                    RowNew.Cells.Add(new DataGridViewTextBoxCell() { Value = index.ToString() });
                    //RowNew.Cells.Add(new DataGridViewTextBoxCell() { Value = index.ToString() + "*" });
                    RowNew.Cells.Add(new DataGridViewTextBoxCell() { Value = fileNameOnly });
                    RowNew.Cells.Add(new DataGridViewImageCell()
                    {
                        Value = PreviewImage,
                        ImageLayout = ZoomType
                    });
                    RowNew.Height = 45;
                    dataGridView_ImagesList.Rows.Insert(index, RowNew);
                    for (int i = index + 1; i < dataGridView_ImagesList.Rows.Count; i++)
                    {
                        string OldValue = dataGridView_ImagesList[0, i].Value.ToString();
                        dataGridView_ImagesList[0, i].Value = Int32.Parse(OldValue) + 1;
                    }

                    if (Watch_Face.Background == null) Watch_Face.Background = new Background();
                    Watch_Face.Background.Preview = new List<MultilangImage>();
                    MultilangImage multilangImage = new MultilangImage();
                    multilangImage.LangCode = "All";
                    multilangImage.ImageSet = new ImageSetGTR2();
                    multilangImage.ImageSet.ImageIndex = 1;
                    multilangImage.ImageSet.ImagesCount = 1;
                    Watch_Face.Background.Preview.Add(multilangImage);
                    JSON_read();
                    richTextBox_JsonText.Text = JsonConvert.SerializeObject(Watch_Face, Formatting.Indented, new JsonSerializerSettings
                    {
                        //DefaultValueHandling = DefaultValueHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    JsonToTree(richTextBox_JsonText.Text);
                    PreviewView = true;
                    JSON_Modified = true;
                    FormText();
                }

                bitmap.Dispose();

            }

        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            Program_Settings.Splitter_Pos = e.SplitY;
            string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText("Settings.json", JSON_String, Encoding.UTF8);
        }

        private void save_JSON_File(String fullfilename, String saveString)
        {
            saveString = saveString.Replace("\r", "");
            saveString = saveString.Replace("\n", Environment.NewLine);
            File.WriteAllText(fullfilename, saveString, Encoding.UTF8);

            string path = Path.GetDirectoryName(fullfilename);
            string IDFileName = Path.Combine(path, "Watchface.ID");
            if (File.Exists(IDFileName) || checkBox_UseID.Checked)
            {
                WatchfaceID watchfaceID = new WatchfaceID();
                watchfaceID.ID = Int32.Parse(textBox_WatchfaceID.Text);
                watchfaceID.UseID = checkBox_UseID.Checked;

                string JSON_String = JsonConvert.SerializeObject(watchfaceID, Formatting.Indented, new JsonSerializerSettings
                {
                    //DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
                File.WriteAllText(IDFileName, JSON_String, Encoding.UTF8);
            }
        }

#region JsonToTree
        private void JsonToTree(string stringJson)
        {
            var savedExpansionState = treeView_JsonTree.Nodes.GetExpansionState();
            string treeName = "*";
            if (FileName != null && FileName.Length > 0) treeName = FileName;
            using (var jsonReader = new JsonTextReader(new StringReader(stringJson)))
            {
                var root = JToken.Load(jsonReader);
                DisplayTreeView(root, Path.GetFileNameWithoutExtension(treeName));
            }

            treeView_JsonTree.Nodes.SetExpansionState(savedExpansionState);
            if(savedExpansionState.Count == 0)
            {
                treeView_JsonTree.CollapseAll();
                if (treeView_JsonTree.TopNode != null) treeView_JsonTree.TopNode.Expand();
            };

        }
        private void DisplayTreeView(JToken root, string rootName)
        {
            treeView_JsonTree.BeginUpdate();
            try
            {
                treeView_JsonTree.Nodes.Clear();
                var tNode = treeView_JsonTree.Nodes[treeView_JsonTree.Nodes.Add(new TreeNode(rootName))];
                tNode.Tag = root;

                AddNode(root, tNode);

                //treeView_JsonTree.ExpandAll();
            }
            finally
            {
                treeView_JsonTree.EndUpdate();
            }
        }

        private void AddNode(JToken token, TreeNode inTreeNode)
        {
            if (token == null)
                return;
            if (token is JValue)
            {
                var childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(token.ToString()))];
                childNode.Tag = token;
            }
            else if (token is JObject)
            {
                var obj = (JObject)token;
                foreach (var property in obj.Properties())
                {
                    var childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(property.Name))];
                    childNode.Tag = property;
                    AddNode(property.Value, childNode);
                }
            }
            else if (token is JArray)
            {
                var array = (JArray)token;
                for (int i = 0; i < array.Count; i++)
                {
                    var childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(i.ToString()))];
                    childNode.Tag = array[i];
                    AddNode(array[i], childNode);
                }
            }
            else
            {
                Console.WriteLine(string.Format("{0} not implemented", token.Type)); // JConstructor, JRaw
            }

        }

        private void comboBox_color_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            ComboBox comboBox_color = sender as ComboBox;
            colorDialog.Color = comboBox_color.BackColor;
            colorDialog.FullOpen = true;
            colorDialog.CustomColors = Program_Settings.CustomColors;


            if (colorDialog.ShowDialog() == DialogResult.Cancel)
                return;
            // установка цвета формы
            comboBox_color.BackColor = colorDialog.Color;
            if (Program_Settings.CustomColors != colorDialog.CustomColors)
            {
                Program_Settings.CustomColors = colorDialog.CustomColors;

                string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
                {
                    //DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
                File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8); 
            }

            PreviewImage();
            JSON_write();
        }

        private void radioButton_ScreenNormal_CheckedChanged(object sender, EventArgs e)
        {
            bool b = radioButton_ScreenNormal.Checked;
            splitContainer_EditParameters.Panel1Collapsed = !b;
            splitContainer_EditParameters.Panel2Collapsed = b;

            PreviewImage();
        }

        private void button_OpenDir_Click(object sender, EventArgs e)
        {
            if (FullFileDir != null)
            {
                Process.Start(new ProcessStartInfo(FullFileDir));
                //Process.Start(new ProcessStartInfo("explorer.exe", " /select, " + FullFileDir));
            }
        }

        private void checkBox_Weather_followMax_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            Control control = checkBox.Parent;
            Control.ControlCollection controlCollection = control.Controls;

            bool b = !checkBox.Checked;
            for (int i = 1; i < controlCollection.Count; i++)
            {
                if (i == 4 || i == 5 || i == 13 || i == 20 || i == 21) controlCollection[i].Enabled = b;
            }
        }

        private void checkBox_Weather_followMax_AOD_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            Control control = checkBox.Parent;
            Control.ControlCollection controlCollection = control.Controls;

            bool b = !checkBox.Checked;
            for (int i = 1; i < controlCollection.Count-1; i++)
            {
                if (i == 4 || i == 5 || i == 13 || i == 20 || i == 21) controlCollection[i].Enabled = b;
            }
        }

        private void checkBox_SaveID_CheckedChanged(object sender, EventArgs e)
        {
            if (Settings_Load) return;
            Program_Settings.SaveID = checkBox_SaveID.Checked;

            string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);
        }

        private void button_GenerateID_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            ushort rndID = (ushort)rnd.Next(1000, 65530);
            textBox_WatchfaceID.Text = rndID.ToString();
            JSON_Modified = true;
            FormText();
        }

        private void checkBox_UseID_CheckedChanged(object sender, EventArgs e)
        {
            button_GenerateID.Enabled = checkBox_UseID.Checked;
            if (checkBox_UseID.Checked)
            {
                int ID = 0;
                if (!Int32.TryParse(textBox_WatchfaceID.Text, out ID))
                {
                    Random rnd = new Random();
                    ushort rndID = (ushort)rnd.Next(1000, 65530);
                    textBox_WatchfaceID.Text = rndID.ToString();
                    JSON_Modified = true;
                    FormText();
                } 
            }
        }






        #endregion

        //private int getOSversion()
        //{
        //    int version = 7;
        //    RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
        //    string ProductName = registryKey.GetValue("ProductName").ToString();
        //    string[] words = ProductName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        //    Int32.TryParse(words[1], out version);
        //    return version;
        //}
    }
}

// нужен для запоминания узлов дерева
public static class TreeViewExtensions
{
    public static List<string> GetExpansionState(this TreeNodeCollection nodes)
    {
        return nodes.Descendants()
                    .Where(n => n.IsExpanded)
                    .Select(n => n.FullPath)
                    .ToList();
    }

    public static void SetExpansionState(this TreeNodeCollection nodes, List<string> savedExpansionState)
    {
        foreach (var node in nodes.Descendants()
                                  .Where(n => savedExpansionState.Contains(n.FullPath)))
        {
            node.Expand();
        }
    }

    public static IEnumerable<TreeNode> Descendants(this TreeNodeCollection c)
    {
        foreach (var node in c.OfType<TreeNode>())
        {
            yield return node;

            foreach (var child in node.Nodes.Descendants())
            {
                yield return child;
            }
        }
    }
}


public static class MouseСoordinates
{
    //public static int X { get; set; }
    //public static int Y { get; set; }
    public static int X = -1;
    public static int Y = -1;
}

public class WatchfaceID
{
    public int ID { get; set; }
    public bool UseID { get; set; }
}

static class Logger
{
    //----------------------------------------------------------
    // Статический метод записи строки в файл лога без переноса
    //----------------------------------------------------------
    public static void Write(string text)
    {
        try
        {
            //using (StreamWriter sw = new StreamWriter(Application.StartupPath + "\\Program log.txt", true))
            //{
            //    sw.Write(text);
            //}
        }
        catch (Exception)
        {
        }
    }

    //---------------------------------------------------------
    // Статический метод записи строки в файл лога с переносом
    //---------------------------------------------------------
    public static void WriteLine(string message)
    {
        try
        {
            //using (StreamWriter sw = new StreamWriter(Application.StartupPath + "\\Program log.txt", true))
            //{
            //    sw.WriteLine(String.Format("{0,-23} {1}", DateTime.Now.ToString() + ":", message));
            //}
        }
        catch (Exception)
        {
        }
    }
}
