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
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using LineCap = System.Drawing.Drawing2D.LineCap;
using System.Globalization;
using System.Threading;
using System.Text.RegularExpressions;
using Microsoft.Win32;

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
            Watch_Face_Preview_Set.Activity = new ActivityS();
            Watch_Face_Preview_Set.Date = new DateS();
            Watch_Face_Preview_Set.Status = new StatusS();
            Watch_Face_Preview_Set.Time = new TimeS();

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
            currentDPI = tabControl1.Height / 700f;
            tabControl1.TabPages[0].Parent = null;
            tabControl1.TabPages[2].Parent = null;
            tabControl1.TabPages[2].Parent = null;
            tabControl1.TabPages[4].Parent = null;
            //tabControl1.SelectTab(1);
            //tabControl_EditParameters.SelectTab(5);

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
#if Puthon
            // пробелы в имени
            if (Application.StartupPath.IndexOf(" ") != -1)
            {
                Logger.WriteLine("пробелы в имени");
                MessageBox.Show(Properties.FormStrings.Message_Error_SpaceInProgrammName_Text,
                    Properties.FormStrings.Message_Error_Caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
#endif

            //Logger.WriteLine("Form1_Load");
            helpProvider1.HelpNamespace = Application.StartupPath + Properties.FormStrings.File_ReadMy;
#if Puthon
            string subPath = Application.StartupPath + @"\Tools\py_amazfit_tools-GTR2\main.py";
#else
            string subPath = Application.StartupPath + @"\Tools\main.exe";
#endif
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

            comboBox_Day_alignment.SelectedIndex = 0;
            comboBox_Month_alignment.SelectedIndex = 0;
            comboBox_Year_alignment.SelectedIndex = 0;

            comboBox_MonthAndDayD_Alignment.SelectedIndex = 0;
            comboBox_MonthAndDayM_Alignment.SelectedIndex = 0;
            comboBox_OneLine_Alignment.SelectedIndex = 0;
            comboBox_Year_Alignment2.SelectedIndex = 0;
            
            comboBox_ActivitySteps_Alignment.SelectedIndex = 0;
            comboBox_ActivityStepsGoal_Alignment.SelectedIndex = 0;
            comboBox_ActivityDistance_Alignment.SelectedIndex = 0;
            comboBox_ActivityPuls_Alignment.SelectedIndex = 0;
            comboBox_ActivityCalories_Alignment.SelectedIndex = 0;
            comboBox_Battery_Text_Alignment.SelectedIndex = 0;

            comboBox_Weather_Text_Alignment.SelectedIndex = 0;
            comboBox_Weather_Day_Alignment.SelectedIndex = 0;
            comboBox_Weather_Night_Alignment.SelectedIndex = 0;

            comboBox_Battery_alignment.SelectedIndex = 0;

            comboBox_Battery_Flatness.SelectedIndex = 0;
            comboBox_StepsProgress_Flatness.SelectedIndex = 0;
            comboBox_ActivityPulsScale_Flatness.SelectedIndex = 0;
            comboBox_ActivityCaloriesScale_Flatness.SelectedIndex = 0;

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

            checkBox_ShowMiles.Checked = Program_Settings.ShowMiles;
            checkBox_ShowIn12hourFormat.Checked = Program_Settings.ShowIn12hourFormat;
            checkBox_DoNotShowMaxMinTemp.Checked = Program_Settings.DoNotShowMaxMinTemp;

            if (Program_Settings.language.Length>1) comboBox_Language.Text = Program_Settings.language;
            dataGridView_MotiomAnimation.RowTemplate.Height = (int)(18 * currentDPI);

            if (Program_Settings.Splitter_Pos > 0 ) 
                splitContainer1.SplitterDistance = Program_Settings.Splitter_Pos;

            Settings_Load = false;

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
            button_CreatePreview.Location= new Point(5, 583);
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
                        string newFullName_bin = Path.Combine(path, fileNameOnly + "_packed.bin");
                        if (File.Exists(newFullName_cmp)) File.Copy(newFullName_cmp, newFullName_bin, true);
                        if (File.Exists(newFullName_bin))
                        {
                            Logger.WriteLine("newFullName_bin");
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
                ShowAllFileSize(AllFileSize);
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
            dataGridView_Battery_IconSet.Rows.Clear();
            dataGridView_SPSliced.Rows.Clear();
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
                        file.FullName.Substring(file.FullName.LastIndexOf('\\')+1) + Properties.FormStrings.Message_error_Image_Text2);
                }
            }
            //Logger.WriteLine("Загрузили все файлы изображений");

            //loadedImage.Dispose();
            int LastImage = 0;
            Int32.TryParse(ListImages.Last(), out LastImage);
            LastImage++;
#if !DEBUG
            if (count != LastImage) MessageBox.Show(Properties.FormStrings.Message_PNGmissing_Text1 + Environment.NewLine +
                 Properties.FormStrings.Message_PNGmissing_Text2, Properties.FormStrings.Message_Error_Caption, 
                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            string newFullName = Path.Combine(path, "PreviewStates.json");
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
            ShowAllFileSize(AllFileSize);
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

        private void ShowAllFileSize(double sizeinbytes)
        {
            Logger.WriteLine("* ShowAllFileSize");
            double AllFileSizeMB = GetFileSizeMB(sizeinbytes);
            label_size.Text = "≈" + AllFileSizeMB.ToString() + "MB";
            Logger.WriteLine("* ShowAllFileSize (end)");
        }


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
            PreviewToBitmap(gPanel, scale, checkBox_crop.Checked, checkBox_WebW.Checked, checkBox_WebB.Checked, 
                checkBox_border.Checked, checkBox_Show_Shortcuts.Checked, checkBox_Shortcuts_Area.Checked, 
                checkBox_Shortcuts_Border.Checked, true, checkBox_CircleScaleImage.Checked, 
                checkBox_center_marker.Checked, 0);
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
            Watch_Face_Preview_Set.Activity.Pulse = (int)numericUpDown_Pulse_Set1.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set1.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set1.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set1.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set1.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set1.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set1.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set1.Checked;

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
            Watch_Face_Preview_Set.Activity.Pulse = (int)numericUpDown_Pulse_Set2.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set2.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set2.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set2.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set2.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set2.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set2.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set2.Checked;

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
            Watch_Face_Preview_Set.Activity.Pulse = (int)numericUpDown_Pulse_Set3.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set3.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set3.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set3.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set3.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set3.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set3.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set3.Checked;

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
            Watch_Face_Preview_Set.Activity.Pulse = (int)numericUpDown_Pulse_Set4.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set4.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set4.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set4.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set4.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set4.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set4.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set4.Checked;

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
            Watch_Face_Preview_Set.Activity.Pulse = (int)numericUpDown_Pulse_Set5.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set5.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set5.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set5.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set5.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set5.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set5.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set5.Checked;

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
            Watch_Face_Preview_Set.Activity.Pulse = (int)numericUpDown_Pulse_Set6.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set6.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set6.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set6.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set6.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set6.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set6.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set6.Checked;

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
            Watch_Face_Preview_Set.Activity.Pulse = (int)numericUpDown_Pulse_Set7.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set7.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set7.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set7.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set7.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set7.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set7.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set7.Checked;

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
            Watch_Face_Preview_Set.Activity.Pulse = (int)numericUpDown_Pulse_Set8.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set8.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set8.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set8.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set8.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set8.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set8.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set8.Checked;

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
            Watch_Face_Preview_Set.Activity.Pulse = (int)numericUpDown_Pulse_Set9.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set9.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set9.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set9.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set9.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set9.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set9.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set9.Checked;

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
            Watch_Face_Preview_Set.Activity.Pulse = (int)numericUpDown_Pulse_Set10.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set10.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set10.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set10.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set10.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set10.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set10.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set10.Checked;

            SetDigitForPrewiev();
        }

        public void SetPreferences11()
        {
            Watch_Face_Preview_Set.Date.Year = dateTimePicker_Date_Set11.Value.Year;
            Watch_Face_Preview_Set.Date.Month = dateTimePicker_Date_Set11.Value.Month;
            Watch_Face_Preview_Set.Date.Day = dateTimePicker_Date_Set11.Value.Day;
            Watch_Face_Preview_Set.Date.WeekDay = (int)dateTimePicker_Date_Set11.Value.DayOfWeek;
            if (Watch_Face_Preview_Set.Date.WeekDay == 0) Watch_Face_Preview_Set.Date.WeekDay = 7;

            Watch_Face_Preview_Set.Time.Hours = dateTimePicker_Time_Set11.Value.Hour;
            Watch_Face_Preview_Set.Time.Minutes = dateTimePicker_Time_Set11.Value.Minute;
            Watch_Face_Preview_Set.Time.Seconds = dateTimePicker_Time_Set11.Value.Second;

            Watch_Face_Preview_Set.Battery = (int)numericUpDown_Battery_Set11.Value;
            Watch_Face_Preview_Set.Activity.Calories = (int)numericUpDown_Calories_Set11.Value;
            Watch_Face_Preview_Set.Activity.Pulse = (int)numericUpDown_Pulse_Set11.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set11.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set11.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set11.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set11.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set11.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set11.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set11.Checked;

            SetDigitForPrewiev();
        }

        public void SetPreferences12()
        {
            Watch_Face_Preview_Set.Date.Year = dateTimePicker_Date_Set12.Value.Year;
            Watch_Face_Preview_Set.Date.Month = dateTimePicker_Date_Set12.Value.Month;
            Watch_Face_Preview_Set.Date.Day = dateTimePicker_Date_Set12.Value.Day;
            Watch_Face_Preview_Set.Date.WeekDay = (int)dateTimePicker_Date_Set12.Value.DayOfWeek;
            if (Watch_Face_Preview_Set.Date.WeekDay == 0) Watch_Face_Preview_Set.Date.WeekDay = 7;

            Watch_Face_Preview_Set.Time.Hours = dateTimePicker_Time_Set12.Value.Hour;
            Watch_Face_Preview_Set.Time.Minutes = dateTimePicker_Time_Set12.Value.Minute;
            Watch_Face_Preview_Set.Time.Seconds = dateTimePicker_Time_Set12.Value.Second;

            Watch_Face_Preview_Set.Battery = (int)numericUpDown_Battery_Set12.Value;
            Watch_Face_Preview_Set.Activity.Calories = (int)numericUpDown_Calories_Set12.Value;
            Watch_Face_Preview_Set.Activity.Pulse = (int)numericUpDown_Pulse_Set12.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set12.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set12.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set12.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set12.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set12.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set12.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set12.Checked;

            SetDigitForPrewiev();
        }

        public void SetPreferences13()
        {
            Watch_Face_Preview_Set.Date.Year = dateTimePicker_Date_Set13.Value.Year;
            Watch_Face_Preview_Set.Date.Month = dateTimePicker_Date_Set13.Value.Month;
            Watch_Face_Preview_Set.Date.Day = dateTimePicker_Date_Set13.Value.Day;
            Watch_Face_Preview_Set.Date.WeekDay = (int)dateTimePicker_Date_Set13.Value.DayOfWeek;
            if (Watch_Face_Preview_Set.Date.WeekDay == 0) Watch_Face_Preview_Set.Date.WeekDay = 7;

            Watch_Face_Preview_Set.Time.Hours = dateTimePicker_Time_Set13.Value.Hour;
            Watch_Face_Preview_Set.Time.Minutes = dateTimePicker_Time_Set13.Value.Minute;
            Watch_Face_Preview_Set.Time.Seconds = dateTimePicker_Time_Set13.Value.Second;

            Watch_Face_Preview_Set.Battery = (int)numericUpDown_Battery_Set13.Value;
            Watch_Face_Preview_Set.Activity.Calories = (int)numericUpDown_Calories_Set13.Value;
            Watch_Face_Preview_Set.Activity.Pulse = (int)numericUpDown_Pulse_Set13.Value;
            Watch_Face_Preview_Set.Activity.Distance = (int)numericUpDown_Distance_Set13.Value;
            Watch_Face_Preview_Set.Activity.Steps = (int)numericUpDown_Steps_Set13.Value;
            Watch_Face_Preview_Set.Activity.StepsGoal = (int)numericUpDown_Goal_Set13.Value;

            Watch_Face_Preview_Set.Status.Bluetooth = check_BoxBluetooth_Set13.Checked;
            Watch_Face_Preview_Set.Status.Alarm = checkBox_Alarm_Set13.Checked;
            Watch_Face_Preview_Set.Status.Lock = checkBox_Lock_Set13.Checked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = checkBox_DoNotDisturb_Set13.Checked;

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
        

        private void checkBox_Time_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Time.Checked)
            {
                tabControl_Time.Enabled = true;
                groupBox_AmPm.Enabled = true;
                groupBox_Delimiter.Enabled = true;
            }
            else
            {
                tabControl_Time.Enabled = false;
                groupBox_AmPm.Enabled = false;
                groupBox_Delimiter.Enabled = false;
            }

        }

        private void checkBox_AmPm_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_AmPm.Checked)
            {
                numericUpDown_AmPm_X.Enabled = true;
                numericUpDown_AmPm_Y.Enabled = true;
                comboBox_Image_Am.Enabled = true;
                comboBox_Image_Pm.Enabled = true;

                label32.Enabled = true;
                label33.Enabled = true;
                label34.Enabled = true;
                label35.Enabled = true;
            }
            else
            {
                numericUpDown_AmPm_X.Enabled = false;
                numericUpDown_AmPm_Y.Enabled = false;
                comboBox_Image_Am.Enabled = false;
                comboBox_Image_Pm.Enabled = false;

                label32.Enabled = false;
                label33.Enabled = false;
                label34.Enabled = false;
                label35.Enabled = false;
            }
        }

        private void checkBox_Hours_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Hours.Checked)
            {
                groupBox_Hours_Tens.Enabled = true;
                groupBox_Hours_Ones.Enabled = true;
            }
            else
            {
                groupBox_Hours_Tens.Enabled = false;
                groupBox_Hours_Ones.Enabled = false;
            }
        }

        #region сворачиваем и разварачиваем панели с настройками

        private void button_Background_Click(object sender, EventArgs e)
        {
            if (panel_Background.Height == 1)
            {
                panel_Background.Height = (int)(30 * currentDPI);
                panel_Time.Height = 1;
                panel_Date.Height = 1;
                panel_AnalogDate.Height = 1;
                panel_StepsProgress.Height = 1;
                panel_Activity.Height = 1;
                panel_Status.Height = 1;
                panel_Battery.Height = 1;
                panel_AnalogClock.Height = 1;
                panel_Weather.Height = 1;
                panel_Shortcuts.Height = 1;
                panel_Animation.Height = 1;
            }
            else panel_Background.Height = 1;
        }

        private void button_Time_Click(object sender, EventArgs e)
        {
            if (panel_Time.Height == 1)
            {
                panel_Background.Height = 1;
                panel_Time.Height = (int)(330 * currentDPI);
                panel_Date.Height = 1;
                panel_AnalogDate.Height = 1;
                panel_StepsProgress.Height = 1;
                panel_Activity.Height = 1;
                panel_Status.Height = 1;
                panel_Battery.Height = 1;
                panel_AnalogClock.Height = 1;
                panel_Weather.Height = 1;
                panel_Shortcuts.Height = 1;
                panel_Animation.Height = 1;
            }
            else panel_Time.Height = 1;
        }

        private void button_Date_Click(object sender, EventArgs e)
        {
            if (panel_Date.Height == 1)
            {
                panel_Background.Height = 1;
                panel_Time.Height = 1;
                panel_Date.Height = (int)(240 * currentDPI);
                panel_AnalogDate.Height = 1;
                panel_StepsProgress.Height = 1;
                panel_Activity.Height = 1;
                panel_Status.Height = 1;
                panel_Battery.Height = 1;
                panel_AnalogClock.Height = 1;
                panel_Weather.Height = 1;
                panel_Shortcuts.Height = 1;
                panel_Animation.Height = 1;
            }
            else panel_Date.Height = 1;
        }

        private void button_AnalogDate_Click(object sender, EventArgs e)
        {
            if (panel_AnalogDate.Height == 1)
            {
                panel_Background.Height = 1;
                panel_Time.Height = 1;
                panel_Date.Height = 1;
                panel_AnalogDate.Height = (int)(145 * currentDPI);
                panel_StepsProgress.Height = 1;
                panel_Activity.Height = 1;
                panel_Status.Height = 1;
                panel_Battery.Height = 1;
                panel_AnalogClock.Height = 1;
                panel_Weather.Height = 1;
                panel_Shortcuts.Height = 1;
                panel_Animation.Height = 1;
            }
            else panel_AnalogDate.Height = 1;
        }

        private void button_StepsProgress_Click(object sender, EventArgs e)
        {
            if (panel_StepsProgress.Height == 1)
            {
                panel_Background.Height = 1;
                panel_Time.Height = 1;
                panel_Date.Height = 1;
                panel_AnalogDate.Height = 1;
                panel_StepsProgress.Height = (int)(165 * currentDPI);
                panel_Activity.Height = 1;
                panel_Status.Height = 1;
                panel_Battery.Height = 1;
                panel_AnalogClock.Height = 1;
                panel_Weather.Height = 1;
                panel_Shortcuts.Height = 1;
                panel_Animation.Height = 1;
            }
            else panel_StepsProgress.Height = 1;
        }

        private void button_Activity_Click(object sender, EventArgs e)
        {
            if (panel_Activity.Height == 1)
            {
                panel_Background.Height = 1;
                panel_Time.Height = 1;
                panel_Date.Height = 1;
                panel_AnalogDate.Height = 1;
                panel_StepsProgress.Height = 1;
                panel_Activity.Height = (int)(240 * currentDPI);
                panel_Status.Height = 1;
                panel_Battery.Height = 1;
                panel_AnalogClock.Height = 1;
                panel_Weather.Height = 1;
                panel_Shortcuts.Height = 1;
                panel_Animation.Height = 1;
            }
            else panel_Activity.Height = 1;
        }

        private void button_Status_Click(object sender, EventArgs e)
        {
            if (panel_Status.Height == 1)
            {
                panel_Background.Height = 1;
                panel_Time.Height = 1;
                panel_Date.Height = 1;
                panel_AnalogDate.Height = 1;
                panel_StepsProgress.Height = 1;
                panel_Activity.Height = 1;
                panel_Status.Height = (int)(82 * currentDPI);
                panel_Battery.Height = 1;
                panel_AnalogClock.Height = 1;
                panel_Weather.Height = 1;
                panel_Shortcuts.Height = 1;
                panel_Animation.Height = 1;
            }
            else panel_Status.Height = 1;
        }

        private void button_Battery_Click(object sender, EventArgs e)
        {
            if (panel_Battery.Height == 1)
            {
                panel_Background.Height = 1;
                panel_Time.Height = 1;
                panel_Date.Height = 1;
                panel_AnalogDate.Height = 1;
                panel_StepsProgress.Height = 1;
                panel_Activity.Height = 1;
                panel_Status.Height = 1;
                panel_Battery.Height = (int)(185 * currentDPI);
                panel_AnalogClock.Height = 1;
                panel_Weather.Height = 1;
                panel_Shortcuts.Height = 1;
                panel_Animation.Height = 1;
            }
            else panel_Battery.Height = 1;
        }

        private void button_AnalogClock_Click(object sender, EventArgs e)
        {
            if (panel_AnalogClock.Height == 1)
            {
                panel_Background.Height = 1;
                panel_Time.Height = 1;
                panel_Date.Height = 1;
                panel_AnalogDate.Height = 1;
                panel_StepsProgress.Height = 1;
                panel_Activity.Height = 1;
                panel_Status.Height = 1;
                panel_Battery.Height = 1;
                panel_AnalogClock.Height = (int)(193 * currentDPI);
                panel_Weather.Height = 1;
                panel_Shortcuts.Height = 1;
                panel_Animation.Height = 1;
            }
            else panel_AnalogClock.Height = 1;
        }

        private void button_Weather_Click(object sender, EventArgs e)
        {
            if (panel_Weather.Height == 1)
            {
                panel_Background.Height = 1;
                panel_Time.Height = 1;
                panel_Date.Height = 1;
                panel_AnalogDate.Height = 1;
                panel_StepsProgress.Height = 1;
                panel_Activity.Height = 1;
                panel_Status.Height = 1;
                panel_Battery.Height = 1;
                panel_AnalogClock.Height = 1;
                panel_Weather.Height = (int)(260 * currentDPI);
                panel_Shortcuts.Height = 1;
                panel_Animation.Height = 1;
            }
            else panel_Weather.Height = 1;
        }

        private void button_Shortcuts_Click(object sender, EventArgs e)
        {
            if (panel_Shortcuts.Height == 1)
            {
                panel_Background.Height = 1;
                panel_Time.Height = 1;
                panel_Date.Height = 1;
                panel_AnalogDate.Height = 1;
                panel_StepsProgress.Height = 1;
                panel_Activity.Height = 1;
                panel_Status.Height = 1;
                panel_Battery.Height = 1;
                panel_AnalogClock.Height = 1;
                panel_Weather.Height = 1;
                panel_Shortcuts.Height = (int)(145 * currentDPI);
                panel_Animation.Height = 1;
            }
            else panel_Shortcuts.Height = 1;
        }

        private void button_Animation_Click(object sender, EventArgs e)
        {
            if (panel_Animation.Height == 1)
            {
                panel_Background.Height = 1;
                panel_Time.Height = 1;
                panel_Date.Height = 1;
                panel_AnalogDate.Height = 1;
                panel_StepsProgress.Height = 1;
                panel_Activity.Height = 1;
                panel_Status.Height = 1;
                panel_Battery.Height = 1;
                panel_AnalogClock.Height = 1;
                panel_Weather.Height = 1;
                panel_Shortcuts.Height = 1;
                panel_Animation.Height = (int)(275 * currentDPI);
            }
            else panel_Animation.Height = 1;
        }
        #endregion

        #region активируем и деактивируем настройки
        private void checkBox_Minutes_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Minutes.Checked;
            groupBox_Minutes_Tens.Enabled = b;
            groupBox_Minutes_Ones.Enabled = b;
        }

        private void checkBox_Seconds_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Seconds.Checked;
            groupBox_Seconds_Tens.Enabled = b;
            groupBox_Seconds_Ones.Enabled = b;
        }

        private void checkBox_WeekDay_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_WeekDay.Checked;
            numericUpDown_WeekDay_X.Enabled = b;
            numericUpDown_WeekDay_Y.Enabled = b;
            comboBox_WeekDay_Image.Enabled = b;
            numericUpDown_WeekDay_Count.Enabled = b;

            label55.Enabled = b;
            label56.Enabled = b;
            label57.Enabled = b;
            label58.Enabled = b;
        }

        private void checkBox_DOW_IconSet_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_DOW_IconSet.Checked;
            comboBox_DOW_IconSet_Image.Enabled = b;
            dataGridView_DOW_IconSet.Enabled = b;
            label460.Enabled = b;
        }

        private void checkBox_MonthName_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_MonthName.Checked;
            numericUpDown_MonthName_X.Enabled = b;
            numericUpDown_MonthName_Y.Enabled = b;
            comboBox_MonthName_Image.Enabled = b;
            numericUpDown_MonthName_Count.Enabled = b;

            label59.Enabled = b;
            label60.Enabled = b;
            label61.Enabled = b;
            label62.Enabled = b;
        }

        private void checkBox_Date_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Date.Checked;
            tabControl_Date.Enabled = b;
            groupBox__WeekDay.Enabled = b;
            groupBox_WeekDayProgress.Enabled = b;
            checkBox_TwoDigitsMonth.Enabled = b;
            checkBox_TwoDigitsDay.Enabled = b;
        }

        private void checkBox_MonthAndDayD_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_MonthAndDayD.Checked;
            numericUpDown_MonthAndDayD_StartCorner_X.Enabled = b;
            numericUpDown_MonthAndDayD_StartCorner_Y.Enabled = b;
            numericUpDown_MonthAndDayD_EndCorner_X.Enabled = b;
            numericUpDown_MonthAndDayD_EndCorner_Y.Enabled = b;
            comboBox_MonthAndDayD_Alignment.Enabled = b;
            numericUpDown_MonthAndDayD_Spacing.Enabled = b;
            comboBox_MonthAndDayD_Image.Enabled = b;
            numericUpDown_MonthAndDayD_Count.Enabled = b;

            label63.Enabled = b;
            label64.Enabled = b;
            label65.Enabled = b;
            label66.Enabled = b;
            label67.Enabled = b;
            label68.Enabled = b;
            label69.Enabled = b;
            label70.Enabled = b;
            label71.Enabled = b;
            label72.Enabled = b;
        }

        private void checkBox_MonthAndDayM_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_MonthAndDayM.Checked;
            numericUpDown_MonthAndDayM_StartCorner_X.Enabled = b;
            numericUpDown_MonthAndDayM_StartCorner_Y.Enabled = b;
            numericUpDown_MonthAndDayM_EndCorner_X.Enabled = b;
            numericUpDown_MonthAndDayM_EndCorner_Y.Enabled = b;
            comboBox_MonthAndDayM_Alignment.Enabled = b;
            numericUpDown_MonthAndDayM_Spacing.Enabled = b;
            comboBox_MonthAndDayM_Image.Enabled = b;
            numericUpDown_MonthAndDayM_Count.Enabled = b;

            label73.Enabled = b;
            label74.Enabled = b;
            label75.Enabled = b;
            label76.Enabled = b;
            label77.Enabled = b;
            label78.Enabled = b;
            label79.Enabled = b;
            label80.Enabled = b;
            label81.Enabled = b;
            label82.Enabled = b;
        }

        private void checkBox_OneLine_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_OneLine.Checked;
            numericUpDown_OneLine_StartCorner_X.Enabled = b;
            numericUpDown_OneLine_StartCorner_Y.Enabled = b;
            numericUpDown_OneLine_EndCorner_X.Enabled = b;
            numericUpDown_OneLine_EndCorner_Y.Enabled = b;
            comboBox_OneLine_Alignment.Enabled = b;
            numericUpDown_OneLine_Spacing.Enabled = b;
            comboBox_OneLine_Image.Enabled = b;
            numericUpDown_OneLine_Count.Enabled = b;
            comboBox_OneLine_Delimiter.Enabled = b;

            label93.Enabled = b;
            label94.Enabled = b;
            label95.Enabled = b;
            label96.Enabled = b;
            label97.Enabled = b;
            label98.Enabled = b;
            label99.Enabled = b;
            label100.Enabled = b;
            label101.Enabled = b;
            label102.Enabled = b;
            label103.Enabled = b;
        }

        private void checkBox_Year_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Year.Checked;
            numericUpDown_Year_StartCorner_X.Enabled = b;
            numericUpDown_Year_StartCorner_Y.Enabled = b;
            numericUpDown_Year_EndCorner_X.Enabled = b;
            numericUpDown_Year_EndCorner_Y.Enabled = b;
            comboBox_Year_Alignment2.Enabled = b;
            numericUpDown_Year_Spacing2.Enabled = b;
            comboBox_Year_Image1.Enabled = b;
            numericUpDown_Year_Count.Enabled = b;
            comboBox_Year_Delimiter.Enabled = b;

            label357.Enabled = b;
            label358.Enabled = b;
            label359.Enabled = b;
            label360.Enabled = b;
            label361.Enabled = b;
            label362.Enabled = b;
            label363.Enabled = b;
            label364.Enabled = b;
            label365.Enabled = b;
            label366.Enabled = b;
            label367.Enabled = b;
        }

        private void checkBox_ADDay_ClockHand_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_ADDay_ClockHand.Checked;
            numericUpDown_ADDay_ClockHand_X.Enabled = b;
            numericUpDown_ADDay_ClockHand_Y.Enabled = b;
            numericUpDown_ADDay_ClockHand_Offset_X.Enabled = b;
            numericUpDown_ADDay_ClockHand_Offset_Y.Enabled = b;
            comboBox_ADDay_ClockHand_Image.Enabled = b;
            numericUpDown_ADDay_ClockHand_StartAngle.Enabled = b;
            numericUpDown_ADDay_ClockHand_EndAngle.Enabled = b;
            label396.Enabled = b;
            label397.Enabled = b;
            label398.Enabled = b;
            label399.Enabled = b;
            label400.Enabled = b;
            label401.Enabled = b;
            label402.Enabled = b;
        }

        private void checkBox_ADWeekDay_ClockHand_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_ADWeekDay_ClockHand.Checked;
            numericUpDown_ADWeekDay_ClockHand_X.Enabled = b;
            numericUpDown_ADWeekDay_ClockHand_Y.Enabled = b;
            numericUpDown_ADWeekDay_ClockHand_Offset_X.Enabled = b;
            numericUpDown_ADWeekDay_ClockHand_Offset_Y.Enabled = b;
            comboBox_ADWeekDay_ClockHand_Image.Enabled = b;
            numericUpDown_ADWeekDay_ClockHand_StartAngle.Enabled = b;
            numericUpDown_ADWeekDay_ClockHand_EndAngle.Enabled = b;
            label389.Enabled = b;
            label390.Enabled = b;
            label391.Enabled = b;
            label392.Enabled = b;
            label393.Enabled = b;
            label394.Enabled = b;
            label395.Enabled = b;
        }

        private void checkBox_ADMonth_ClockHand_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_ADMonth_ClockHand.Checked;
            numericUpDown_ADMonth_ClockHand_X.Enabled = b;
            numericUpDown_ADMonth_ClockHand_Y.Enabled = b;
            numericUpDown_ADMonth_ClockHand_Offset_X.Enabled = b;
            numericUpDown_ADMonth_ClockHand_Offset_Y.Enabled = b;
            comboBox_ADMonth_ClockHand_Image.Enabled = b;
            numericUpDown_ADMonth_ClockHand_StartAngle.Enabled = b;
            numericUpDown_ADMonth_ClockHand_EndAngle.Enabled = b;
            label382.Enabled = b;
            label383.Enabled = b;
            label384.Enabled = b;
            label385.Enabled = b;
            label386.Enabled = b;
            label387.Enabled = b;
            label388.Enabled = b;
        }

        private void checkBox_Activity_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Activity.Checked;
            tabControl_Activity.Enabled = b;
            comboBox_Activity_NDImage.Enabled = b;
            label403.Enabled = b;
        }

        private void checkBox_CircleScale_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_StepsProgress.Checked;
            numericUpDown_StepsProgress_Center_X.Enabled = b;
            numericUpDown_StepsProgress_Center_Y.Enabled = b;
            numericUpDown_StepsProgress_Radius_X.Enabled = b;
            checkBox_StepsProgress_Image.Enabled = b;
            comboBox_StepsProgress_Image.Enabled = b;
            numericUpDown_StepsProgress_ImageX.Enabled = b;
            numericUpDown_StepsProgress_ImageY.Enabled = b;

            numericUpDown_StepsProgress_Width.Enabled = b;
            comboBox_StepsProgress_Color.Enabled = b;
            numericUpDown_StepsProgress_StartAngle.Enabled = b;
            numericUpDown_StepsProgress_EndAngle.Enabled = b;
            comboBox_StepsProgress_Flatness.Enabled = b;

            label104.Enabled = b;
            label105.Enabled = b;
            label106.Enabled = b;
            label107.Enabled = b;
            label108.Enabled = b;
            label109.Enabled = b;
            label110.Enabled = b;
            label111.Enabled = b;
            label348.Enabled = b;
            label432.Enabled = b;
            label433.Enabled = b;

            if (b)
            {
                if (checkBox_StepsProgress_Image.Checked)
                {
                    comboBox_StepsProgress_Image.Enabled = true;
                    numericUpDown_StepsProgress_ImageX.Enabled = true;
                    numericUpDown_StepsProgress_ImageY.Enabled = true;
                    label432.Enabled = true;
                    label433.Enabled = true;
                    comboBox_StepsProgress_Color.Enabled = false;
                    numericUpDown_StepsProgress_Center_X.Enabled = false;
                    numericUpDown_StepsProgress_Center_Y.Enabled = false;
                    label104.Enabled = false;
                    label105.Enabled = false;
                    label109.Enabled = false;
                }
                else
                {
                    comboBox_StepsProgress_Image.Enabled = false;
                    numericUpDown_StepsProgress_ImageX.Enabled = false;
                    numericUpDown_StepsProgress_ImageY.Enabled = false;
                    label432.Enabled = false;
                    label433.Enabled = false;
                    comboBox_StepsProgress_Color.Enabled = true;
                    numericUpDown_StepsProgress_Center_X.Enabled = true;
                    numericUpDown_StepsProgress_Center_Y.Enabled = true;
                    label104.Enabled = true;
                    label105.Enabled = true;
                    label109.Enabled = true;
                }
            }
        }

        private void checkBox_StepsProgress_Image_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_StepsProgress_Image.Checked)
            {
                comboBox_StepsProgress_Image.Enabled = true;
                numericUpDown_StepsProgress_ImageX.Enabled = true;
                numericUpDown_StepsProgress_ImageY.Enabled = true;
                label432.Enabled = true;
                label433.Enabled = true;
                comboBox_StepsProgress_Color.Enabled = false;
                numericUpDown_StepsProgress_Center_X.Enabled = false;
                numericUpDown_StepsProgress_Center_Y.Enabled = false;
                label104.Enabled = false;
                label105.Enabled = false;
                label109.Enabled = false;
            }
            else
            {
                comboBox_StepsProgress_Image.Enabled = false;
                numericUpDown_StepsProgress_ImageX.Enabled = false;
                numericUpDown_StepsProgress_ImageY.Enabled = false;
                label432.Enabled = false;
                label433.Enabled = false;
                comboBox_StepsProgress_Color.Enabled = true;
                numericUpDown_StepsProgress_Center_X.Enabled = true;
                numericUpDown_StepsProgress_Center_Y.Enabled = true;
                label104.Enabled = true;
                label105.Enabled = true;
                label109.Enabled = true;
            }
        }

        private void checkBox_StProg_ClockHand_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_StProg_ClockHand.Checked;
            numericUpDown_StProg_ClockHand_X.Enabled = b;
            numericUpDown_StProg_ClockHand_Y.Enabled = b;
            numericUpDown_StProg_ClockHand_Offset_X.Enabled = b;
            numericUpDown_StProg_ClockHand_Offset_Y.Enabled = b;
            comboBox_StProg_ClockHand_Image.Enabled = b;
            numericUpDown_StProg_ClockHand_StartAngle.Enabled = b;
            numericUpDown_StProg_ClockHand_EndAngle.Enabled = b;

            label375.Enabled = b;
            label376.Enabled = b;
            label377.Enabled = b;
            label378.Enabled = b;
            label379.Enabled = b;
            label380.Enabled = b;
            label381.Enabled = b;
        }

        private void checkBox_SPSliced_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_SPSliced.Checked;
            comboBox_SPSliced_Image.Enabled = b;
            dataGridView_SPSliced.Enabled = b;
            label404.Enabled = b;
        }

        private void checkBox_ActivityStar_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_ActivityStar.Checked;
            numericUpDown_ActivityStar_X.Enabled = b;
            numericUpDown_ActivityStar_Y.Enabled = b;
            comboBox_ActivityStar_Image.Enabled = b;

            label163.Enabled = b;
            label164.Enabled = b;
            label166.Enabled = b;
            label171.Enabled = b;
        }

        private void checkBox_ActivitySteps_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_ActivitySteps.Checked;
            numericUpDown_ActivitySteps_StartCorner_X.Enabled = b;
            numericUpDown_ActivitySteps_StartCorner_Y.Enabled = b;
            numericUpDown_ActivitySteps_EndCorner_X.Enabled = b;
            numericUpDown_ActivitySteps_EndCorner_Y.Enabled = b;

            comboBox_ActivitySteps_Image.Enabled = b;
            numericUpDown_ActivitySteps_Count.Enabled = b;
            numericUpDown_ActivitySteps_Spacing.Enabled = b;
            comboBox_ActivitySteps_Alignment.Enabled = b;

            label122.Enabled = b;
            label123.Enabled = b;
            label124.Enabled = b;
            label125.Enabled = b;
            label126.Enabled = b;
            label127.Enabled = b;
            label128.Enabled = b;
            label129.Enabled = b;
            label130.Enabled = b;
            label131.Enabled = b;
        }

        private void checkBox_ActivityDistance_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_ActivityDistance.Checked;
            numericUpDown_ActivityDistance_StartCorner_X.Enabled = b;
            numericUpDown_ActivityDistance_StartCorner_Y.Enabled = b;
            numericUpDown_ActivityDistance_EndCorner_X.Enabled = b;
            numericUpDown_ActivityDistance_EndCorner_Y.Enabled = b;

            comboBox_ActivityDistance_Image.Enabled = b;
            numericUpDown_ActivityDistance_Count.Enabled = b;
            numericUpDown_ActivityDistance_Spacing.Enabled = b;
            comboBox_ActivityDistance_Alignment.Enabled = b;
            comboBox_ActivityDistance_Suffix_km.Enabled = b;
            comboBox_ActivityDistance_Suffix_ml.Enabled = b;
            comboBox_ActivityDistance_Decimal.Enabled = b;

            label132.Enabled = b;
            label133.Enabled = b;
            label134.Enabled = b;
            label135.Enabled = b;
            label136.Enabled = b;
            label137.Enabled = b;
            label138.Enabled = b;
            label139.Enabled = b;
            label140.Enabled = b;
            label141.Enabled = b;
            label172.Enabled = b;
            label173.Enabled = b;
            label501.Enabled = b;
        }

        private void checkBox_ActivityPuls_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_ActivityPuls.Checked;
            numericUpDown_ActivityPuls_StartCorner_X.Enabled = b;
            numericUpDown_ActivityPuls_StartCorner_Y.Enabled = b;
            numericUpDown_ActivityPuls_EndCorner_X.Enabled = b;
            numericUpDown_ActivityPuls_EndCorner_Y.Enabled = b;

            comboBox_ActivityPuls_Image.Enabled = b;
            numericUpDown_ActivityPuls_Count.Enabled = b;
            numericUpDown_ActivityPuls_Spacing.Enabled = b;
            comboBox_ActivityPuls_Alignment.Enabled = b;

            label142.Enabled = b;
            label143.Enabled = b;
            label144.Enabled = b;
            label145.Enabled = b;
            label146.Enabled = b;
            label147.Enabled = b;
            label148.Enabled = b;
            label149.Enabled = b;
            label150.Enabled = b;
            label151.Enabled = b;
        }

        private void checkBox_ActivityPulse_IconSet_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_ActivityPuls_IconSet.Checked;
            comboBox_ActivityPuls_IconSet_Image.Enabled = b;
            dataGridView_ActivityPuls_IconSet.Enabled = b;
            label121.Enabled = b;
        }

        private void checkBox_ActivityPulsScale_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_ActivityPulsScale.Checked;
            numericUpDown_ActivityPulsScale_Center_X.Enabled = b;
            numericUpDown_ActivityPulsScale_Center_Y.Enabled = b;
            numericUpDown_ActivityPulsScale_Radius_X.Enabled = b;
            checkBox_ActivityPulsScale_Image.Enabled = b;
            comboBox_ActivityPulsScale_Image.Enabled = b;
            numericUpDown_ActivityPulsScale_ImageX.Enabled = b;
            numericUpDown_ActivityPulsScale_ImageY.Enabled = b;

            numericUpDown_ActivityPulsScale_Width.Enabled = b;
            comboBox_ActivityPulsScale_Color.Enabled = b;
            numericUpDown_ActivityPulsScale_EndAngle.Enabled = b;
            numericUpDown_ActivityPulsScale_StartAngle.Enabled = b;
            comboBox_ActivityPulsScale_Flatness.Enabled = b;

            label417.Enabled = b;
            label418.Enabled = b;
            label419.Enabled = b;
            label420.Enabled = b;
            label421.Enabled = b;
            label422.Enabled = b;
            label423.Enabled = b;
            label424.Enabled = b;
            label425.Enabled = b;
            label430.Enabled = b;
            label431.Enabled = b;

            if (b)
            {
                if (checkBox_ActivityPulsScale_Image.Checked)
                {
                    comboBox_ActivityPulsScale_Image.Enabled = true;
                    numericUpDown_ActivityPulsScale_ImageX.Enabled = true;
                    numericUpDown_ActivityPulsScale_ImageY.Enabled = true;
                    label430.Enabled = true;
                    label431.Enabled = true;
                    comboBox_ActivityPulsScale_Color.Enabled = false;
                    numericUpDown_ActivityPulsScale_Center_X.Enabled = false;
                    numericUpDown_ActivityPulsScale_Center_Y.Enabled = false;
                    label420.Enabled = false;
                    label424.Enabled = false;
                    label425.Enabled = false;
                }
                else
                {
                    comboBox_ActivityPulsScale_Image.Enabled = false;
                    numericUpDown_ActivityPulsScale_ImageX.Enabled = false;
                    numericUpDown_ActivityPulsScale_ImageY.Enabled = false;
                    label430.Enabled = false;
                    label431.Enabled = false;
                    comboBox_ActivityPulsScale_Color.Enabled = true;
                    numericUpDown_ActivityPulsScale_Center_X.Enabled = true;
                    numericUpDown_ActivityPulsScale_Center_Y.Enabled = true;
                    label420.Enabled = true;
                    label424.Enabled = true;
                    label425.Enabled = true;
                }
            }
        }

        private void checkBox_Pulse_ClockHand_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Pulse_ClockHand.Checked;
            numericUpDown_Pulse_ClockHand_X.Enabled = b;
            numericUpDown_Pulse_ClockHand_Y.Enabled = b;
            numericUpDown_Pulse_ClockHand_Offset_X.Enabled = b;
            numericUpDown_Pulse_ClockHand_Offset_Y.Enabled = b;
            comboBox_Pulse_ClockHand_Image.Enabled = b;
            numericUpDown_Pulse_ClockHand_StartAngle.Enabled = b;
            numericUpDown_Pulse_ClockHand_EndAngle.Enabled = b;
            label466.Enabled = b;
            label467.Enabled = b;
            label468.Enabled = b;
            label469.Enabled = b;
            label470.Enabled = b;
            label471.Enabled = b;
            label472.Enabled = b;
        }

        private void checkBox_ActivityPulsScale_Image_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_ActivityPulsScale_Image.Checked)
            {
                comboBox_ActivityPulsScale_Image.Enabled = true;
                numericUpDown_ActivityPulsScale_ImageX.Enabled = true;
                numericUpDown_ActivityPulsScale_ImageY.Enabled = true;
                label430.Enabled = true;
                label431.Enabled = true;
                comboBox_ActivityPulsScale_Color.Enabled = false;
                numericUpDown_ActivityPulsScale_Center_X.Enabled = false;
                numericUpDown_ActivityPulsScale_Center_Y.Enabled = false;
                label420.Enabled = false;
                label424.Enabled = false;
                label425.Enabled = false;
            }
            else
            {
                comboBox_ActivityPulsScale_Image.Enabled = false;
                numericUpDown_ActivityPulsScale_ImageX.Enabled = false;
                numericUpDown_ActivityPulsScale_ImageY.Enabled = false;
                label430.Enabled = false;
                label431.Enabled = false;
                comboBox_ActivityPulsScale_Color.Enabled = true;
                numericUpDown_ActivityPulsScale_Center_X.Enabled = true;
                numericUpDown_ActivityPulsScale_Center_Y.Enabled = true;
                label420.Enabled = true;
                label424.Enabled = true;
                label425.Enabled = true;
            }
        }

        private void checkBox_ActivityCalories_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_ActivityCalories.Checked;
            numericUpDown_ActivityCalories_StartCorner_X.Enabled = b;
            numericUpDown_ActivityCalories_StartCorner_Y.Enabled = b;
            numericUpDown_ActivityCalories_EndCorner_X.Enabled = b;
            numericUpDown_ActivityCalories_EndCorner_Y.Enabled = b;

            comboBox_ActivityCalories_Image.Enabled = b;
            numericUpDown_ActivityCalories_Count.Enabled = b;
            numericUpDown_ActivityCalories_Spacing.Enabled = b;
            comboBox_ActivityCalories_Alignment.Enabled = b;

            label152.Enabled = b;
            label153.Enabled = b;
            label154.Enabled = b;
            label155.Enabled = b;
            label156.Enabled = b;
            label157.Enabled = b;
            label158.Enabled = b;
            label159.Enabled = b;
            label160.Enabled = b;
            label161.Enabled = b;
        }

        private void checkBox_ActivityCaloriesScale_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_ActivityCaloriesScale.Checked;
            numericUpDown_ActivityCaloriesScale_Center_X.Enabled = b;
            numericUpDown_ActivityCaloriesScale_Center_Y.Enabled = b;
            numericUpDown_ActivityCaloriesScale_Radius_X.Enabled = b;
            checkBox_ActivityCaloriesScale_Image.Enabled = b;
            comboBox_ActivityCaloriesScale_Image.Enabled = b;
            numericUpDown_ActivityCaloriesScale_ImageX.Enabled = b;
            numericUpDown_ActivityCaloriesScale_ImageY.Enabled = b;

            numericUpDown_ActivityCaloriesScale_Width.Enabled = b;
            comboBox_ActivityCaloriesScale_Color.Enabled = b;
            numericUpDown_ActivityCaloriesScale_EndAngle.Enabled = b;
            numericUpDown_ActivityCaloriesScale_StartAngle.Enabled = b;
            comboBox_ActivityCaloriesScale_Flatness.Enabled = b;

            label112.Enabled = b;
            label113.Enabled = b;
            label114.Enabled = b;
            label115.Enabled = b;
            label116.Enabled = b;
            label117.Enabled = b;
            label118.Enabled = b;
            label119.Enabled = b;
            label120.Enabled = b;
            label426.Enabled = b;
            label427.Enabled = b;

            if (b)
            {
                if (checkBox_ActivityCaloriesScale_Image.Checked)
                {
                    comboBox_ActivityCaloriesScale_Image.Enabled = true;
                    numericUpDown_ActivityCaloriesScale_ImageX.Enabled = true;
                    numericUpDown_ActivityCaloriesScale_ImageY.Enabled = true;
                    label426.Enabled = true;
                    label427.Enabled = true;
                    comboBox_ActivityCaloriesScale_Color.Enabled = false;
                    numericUpDown_ActivityCaloriesScale_Center_X.Enabled = false;
                    numericUpDown_ActivityCaloriesScale_Center_Y.Enabled = false;
                    label115.Enabled = false;
                    label119.Enabled = false;
                    label120.Enabled = false;
                }
                else
                {
                    comboBox_ActivityCaloriesScale_Image.Enabled = false;
                    numericUpDown_ActivityCaloriesScale_ImageX.Enabled = false;
                    numericUpDown_ActivityCaloriesScale_ImageY.Enabled = false;
                    label426.Enabled = false;
                    label427.Enabled = false;
                    comboBox_ActivityCaloriesScale_Color.Enabled = true;
                    numericUpDown_ActivityCaloriesScale_Center_X.Enabled = true;
                    numericUpDown_ActivityCaloriesScale_Center_Y.Enabled = true;
                    label115.Enabled = true;
                    label119.Enabled = true;
                    label120.Enabled = true;
                }
            }
        }

        private void checkBox_Calories_ClockHand_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Calories_ClockHand.Checked;
            numericUpDown_Calories_ClockHand_X.Enabled = b;
            numericUpDown_Calories_ClockHand_Y.Enabled = b;
            numericUpDown_Calories_ClockHand_Offset_X.Enabled = b;
            numericUpDown_Calories_ClockHand_Offset_Y.Enabled = b;
            comboBox_Calories_ClockHand_Image.Enabled = b;
            numericUpDown_Calories_ClockHand_StartAngle.Enabled = b;
            numericUpDown_Calories_ClockHand_EndAngle.Enabled = b;
            label458.Enabled = b;
            label459.Enabled = b;
            label461.Enabled = b;
            label462.Enabled = b;
            label463.Enabled = b;
            label464.Enabled = b;
            label465.Enabled = b;
        }

        private void checkBox_ActivityCaloriesScale_Image_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_ActivityCaloriesScale_Image.Checked)
            {
                comboBox_ActivityCaloriesScale_Image.Enabled = true;
                numericUpDown_ActivityCaloriesScale_ImageX.Enabled = true;
                numericUpDown_ActivityCaloriesScale_ImageY.Enabled = true;
                label426.Enabled = true;
                label427.Enabled = true;
                comboBox_ActivityCaloriesScale_Color.Enabled = false;
                numericUpDown_ActivityCaloriesScale_Center_X.Enabled = false;
                numericUpDown_ActivityCaloriesScale_Center_Y.Enabled = false;
                label115.Enabled = false;
                label119.Enabled = false;
                label120.Enabled = false;
            }
            else
            {
                comboBox_ActivityCaloriesScale_Image.Enabled = false;
                numericUpDown_ActivityCaloriesScale_ImageX.Enabled = false;
                numericUpDown_ActivityCaloriesScale_ImageY.Enabled = false;
                label426.Enabled = false;
                label427.Enabled = false;
                comboBox_ActivityCaloriesScale_Color.Enabled = true;
                numericUpDown_ActivityCaloriesScale_Center_X.Enabled = true;
                numericUpDown_ActivityCaloriesScale_Center_Y.Enabled = true;
                label115.Enabled = true;
                label119.Enabled = true;
                label120.Enabled = true;
            }
        }

        private void checkBox_ActivityStepsGoal_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_ActivityStepsGoal.Checked;
            numericUpDown_ActivityStepsGoal_StartCorner_X.Enabled = b;
            numericUpDown_ActivityStepsGoal_StartCorner_Y.Enabled = b;
            numericUpDown_ActivityStepsGoal_EndCorner_X.Enabled = b;
            numericUpDown_ActivityStepsGoal_EndCorner_Y.Enabled = b;

            comboBox_ActivityStepsGoal_Image.Enabled = b;
            numericUpDown_ActivityStepsGoal_Count.Enabled = b;
            numericUpDown_ActivityStepsGoal_Spacing.Enabled = b;
            comboBox_ActivityStepsGoal_Alignment.Enabled = b;

            label491.Enabled = b;
            label492.Enabled = b;
            label493.Enabled = b;
            label494.Enabled = b;
            label495.Enabled = b;
            label496.Enabled = b;
            label497.Enabled = b;
            label498.Enabled = b;
            label499.Enabled = b;
            label500.Enabled = b;
        }

        private void checkBox_DND_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_DND.Checked;
            numericUpDown_DND_X.Enabled = b;
            numericUpDown_DND_Y.Enabled = b;
            comboBox_DND_On.Enabled = b;
            comboBox_DND_Off.Enabled = b;

            label180.Enabled = b;
            label181.Enabled = b;
            label182.Enabled = b;
            label183.Enabled = b;
        }

        private void checkBox_Lock_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Lock.Checked;
            numericUpDown_Lock_X.Enabled = b;
            numericUpDown_Lock_Y.Enabled = b;
            comboBox_Lock_On.Enabled = b;
            comboBox_Lock_Off.Enabled = b;

            label176.Enabled = b;
            label177.Enabled = b;
            label178.Enabled = b;
            label179.Enabled = b;
        }

        private void checkBox_Alarm_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Alarm.Checked;
            numericUpDown_Alarm_X.Enabled = b;
            numericUpDown_Alarm_Y.Enabled = b;
            comboBox_Alarm_On.Enabled = b;
            comboBox_Alarm_Off.Enabled = b;

            label170.Enabled = b;
            label175.Enabled = b;
            label174.Enabled = b;
            label169.Enabled = b;
        }

        private void checkBox_Bluetooth_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Bluetooth.Checked;
            numericUpDown_Bluetooth_X.Enabled = b;
            numericUpDown_Bluetooth_Y.Enabled = b;
            comboBox_Bluetooth_On.Enabled = b;
            comboBox_Bluetooth_Off.Enabled = b;

            label162.Enabled = b;
            label165.Enabled = b;
            label167.Enabled = b;
            label168.Enabled = b;
        }

        private void checkBox_Battery_CheckedChanged(object sender, EventArgs e)
        {
            tabControl4.Enabled = checkBox_Battery.Checked;
        }

        private void checkBox_Battery_Text_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Battery_Text.Checked;
            numericUpDown_Battery_Text_StartCorner_X.Enabled = b;
            numericUpDown_Battery_Text_StartCorner_Y.Enabled = b;
            numericUpDown_Battery_Text_EndCorner_X.Enabled = b;
            numericUpDown_Battery_Text_EndCorner_Y.Enabled = b;
            comboBox_Battery_Text_Image.Enabled = b;
            numericUpDown_Battery_Text_Count.Enabled = b;
            numericUpDown_Battery_Text_Spacing.Enabled = b;
            comboBox_Battery_Text_Alignment.Enabled = b;

            label184.Enabled = b;
            label185.Enabled = b;
            label186.Enabled = b;
            label187.Enabled = b;
            label188.Enabled = b;
            label189.Enabled = b;
            label190.Enabled = b;
            label191.Enabled = b;
            label192.Enabled = b;
            label193.Enabled = b;
        }

        private void checkBox_Battery_Percent_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Battery_Percent.Checked;
            comboBox_Battery_Percent_Image.Enabled = b;
            numericUpDown_Battery_Percent_X.Enabled = b;
            numericUpDown_Battery_Percent_Y.Enabled = b;

            label300.Enabled = b;
            label301.Enabled = b;
            label302.Enabled = b;
            label303.Enabled = b;
        }

        private void checkBox_Battery_Img_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Battery_Img.Checked;
            numericUpDown_Battery_Img_Count.Enabled = b;
            comboBox_Battery_Img_Image.Enabled = b;
            numericUpDown_Battery_Img_X.Enabled = b;
            numericUpDown_Battery_Img_Y.Enabled = b;

            label194.Enabled = b;
            label195.Enabled = b;
            label196.Enabled = b;
            label197.Enabled = b;
            label198.Enabled = b;
        }

        private void checkBox_Battery_Scale_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Battery_Scale.Checked;
            numericUpDown_Battery_Scale_Center_X.Enabled = b;
            numericUpDown_Battery_Scale_Center_Y.Enabled = b;
            numericUpDown_Battery_Scale_Radius_X.Enabled = b;
            checkBox_Battery_Scale_Image.Enabled = b;
            comboBox_Battery_Scale_Image.Enabled = b;
            numericUpDown_Battery_Scale_ImageX.Enabled = b;
            numericUpDown_Battery_Scale_ImageY.Enabled = b;

            numericUpDown_Battery_Scale_Width.Enabled = b;
            comboBox_Battery_Scale_Color.Enabled = b;
            numericUpDown_Battery_Scale_EndAngle.Enabled = b;
            numericUpDown_Battery_Scale_StartAngle.Enabled = b;
            comboBox_Battery_Flatness.Enabled = b;

            label199.Enabled = b;
            label200.Enabled = b;
            label201.Enabled = b;
            label202.Enabled = b;
            label203.Enabled = b;
            label204.Enabled = b;
            label205.Enabled = b;
            label206.Enabled = b;
            label347.Enabled = b;
            label428.Enabled = b;
            label429.Enabled = b;

            if (b)
            {
                if (checkBox_Battery_Scale_Image.Checked)
                {
                    comboBox_Battery_Scale_Image.Enabled = true;
                    numericUpDown_Battery_Scale_ImageX.Enabled = true;
                    numericUpDown_Battery_Scale_ImageY.Enabled = true;
                    label428.Enabled = true;
                    label429.Enabled = true;
                    comboBox_Battery_Scale_Color.Enabled = false;
                    numericUpDown_Battery_Scale_Center_X.Enabled = false;
                    numericUpDown_Battery_Scale_Center_Y.Enabled = false;
                    label201.Enabled = false;
                    label205.Enabled = false;
                    label206.Enabled = false;
                }
                else
                {
                    comboBox_Battery_Scale_Image.Enabled = false;
                    numericUpDown_Battery_Scale_ImageX.Enabled = false;
                    numericUpDown_Battery_Scale_ImageY.Enabled = false;
                    label428.Enabled = false;
                    label429.Enabled = false;
                    comboBox_ActivityCaloriesScale_Color.Enabled = true;
                    numericUpDown_Battery_Scale_Center_X.Enabled = true;
                    numericUpDown_Battery_Scale_Center_Y.Enabled = true;
                    label115.Enabled = true;
                    label205.Enabled = true;
                    label206.Enabled = true;
                }
            }
        }

        private void checkBox_Battery_Scale_Image_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Battery_Scale_Image.Checked)
            {
                comboBox_Battery_Scale_Image.Enabled = true;
                numericUpDown_Battery_Scale_ImageX.Enabled = true;
                numericUpDown_Battery_Scale_ImageY.Enabled = true;
                label428.Enabled = true;
                label429.Enabled = true;
                comboBox_Battery_Scale_Color.Enabled = false;
                numericUpDown_Battery_Scale_Center_X.Enabled = false;
                numericUpDown_Battery_Scale_Center_Y.Enabled = false;
                label201.Enabled = false;
                label205.Enabled = false;
                label206.Enabled = false;
            }
            else
            {
                comboBox_Battery_Scale_Image.Enabled = false;
                numericUpDown_Battery_Scale_ImageX.Enabled = false;
                numericUpDown_Battery_Scale_ImageY.Enabled = false;
                label428.Enabled = false;
                label429.Enabled = false;
                comboBox_Battery_Scale_Color.Enabled = true;
                numericUpDown_Battery_Scale_Center_X.Enabled = true;
                numericUpDown_Battery_Scale_Center_Y.Enabled = true;
                label201.Enabled = true;
                label205.Enabled = true;
                label206.Enabled = true;
            }
        }

        private void checkBox_Battery_ClockHand_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Battery_ClockHand.Checked;
            numericUpDown_Battery_ClockHand_X.Enabled = b;
            numericUpDown_Battery_ClockHand_Y.Enabled = b;
            numericUpDown_Battery_ClockHand_Offset_X.Enabled = b;
            numericUpDown_Battery_ClockHand_Offset_Y.Enabled = b;
            comboBox_Battery_ClockHand_Image.Enabled = b;
            numericUpDown_Battery_ClockHand_StartAngle.Enabled = b;
            numericUpDown_Battery_ClockHand_EndAngle.Enabled = b;
            label368.Enabled = b;
            label369.Enabled = b;
            label370.Enabled = b;
            label371.Enabled = b;
            label372.Enabled = b;
            label373.Enabled = b;
            label374.Enabled = b;
        }

        private void checkBox_Battery_IconSet_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Battery_IconSet.Checked;
            comboBox_Battery_IconSet_Image.Enabled = b;
            dataGridView_Battery_IconSet.Enabled = b;
            label405.Enabled = b;
        }

        private void checkBox_AnalogClock_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_AnalogClock.Checked;
            //groupBox_AnalogClock_Hour.Enabled = b;
            //groupBox_AnalogClock_Min.Enabled = b;
            //groupBox_AnalogClock_Sec.Enabled = b;
            tabControl_AnalogClock.Enabled = b;
        }

        private void checkBox_HourCenterImage_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_HourCenterImage.Checked;
            numericUpDown_HourCenterImage_X.Enabled = b;
            numericUpDown_HourCenterImage_Y.Enabled = b;
            comboBox_HourCenterImage_Image.Enabled = b;

            label310.Enabled = b;
            label311.Enabled = b;
            label312.Enabled = b;
        }

        private void checkBox_MinCenterImage_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_MinCenterImage.Checked;
            numericUpDown_MinCenterImage_X.Enabled = b;
            numericUpDown_MinCenterImage_Y.Enabled = b;
            comboBox_MinCenterImage_Image.Enabled = b;

            label307.Enabled = b;
            label308.Enabled = b;
            label309.Enabled = b;
        }

        private void checkBox_SecCenterImage_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_SecCenterImage.Checked;
            numericUpDown_SecCenterImage_X.Enabled = b;
            numericUpDown_SecCenterImage_Y.Enabled = b;
            comboBox_SecCenterImage_Image.Enabled = b;

            label304.Enabled = b;
            label305.Enabled = b;
            label306.Enabled = b;
        }

        private void checkBox_AnalogClock_Sec_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_AnalogClock_Sec.Checked;
            comboBox_AnalogClock_Sec_Image.Enabled = b;
            numericUpDown_AnalogClock_Sec_X.Enabled = b;
            numericUpDown_AnalogClock_Sec_Y.Enabled = b;
            numericUpDown_AnalogClock_Sec_Offset_X.Enabled = b;
            numericUpDown_AnalogClock_Sec_Offset_Y.Enabled = b;

            label210.Enabled = b;
            label211.Enabled = b;
            label212.Enabled = b;
            label353.Enabled = b;
            label354.Enabled = b;
        }

        private void checkBox_AnalogClock_Min_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_AnalogClock_Min.Checked;
            comboBox_AnalogClock_Min_Image.Enabled = b;
            numericUpDown_AnalogClock_Min_X.Enabled = b;
            numericUpDown_AnalogClock_Min_Y.Enabled = b;
            numericUpDown_AnalogClock_Min_Offset_X.Enabled = b;
            numericUpDown_AnalogClock_Min_Offset_Y.Enabled = b;

            label207.Enabled = b;
            label208.Enabled = b;
            label209.Enabled = b;
            label351.Enabled = b;
            label352.Enabled = b;
        }

        private void checkBox_AnalogClock_Hour_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_AnalogClock_Hour.Checked;
            comboBox_AnalogClock_Hour_Image.Enabled = b;
            numericUpDown_AnalogClock_Hour_X.Enabled = b;
            numericUpDown_AnalogClock_Hour_Y.Enabled = b;
            numericUpDown_AnalogClock_Hour_Offset_X.Enabled = b;
            numericUpDown_AnalogClock_Hour_Offset_Y.Enabled = b;

            label215.Enabled = b;
            label216.Enabled = b;
            label217.Enabled = b;
            label349.Enabled = b;
            label350.Enabled = b;
        }

        private void checkBox_Weather_Text_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Weather_Text.Checked;
            numericUpDown_Weather_Text_StartCorner_X.Enabled = b;
            numericUpDown_Weather_Text_StartCorner_Y.Enabled = b;
            numericUpDown_Weather_Text_EndCorner_X.Enabled = b;
            numericUpDown_Weather_Text_EndCorner_Y.Enabled = b;
            //comboBox_Weather_Text_MinusImage.Enabled = b;
            //comboBox_Weather_Text_DegImage.Enabled = b;
            //comboBox_Weather_Text_NDImage.Enabled = b;
            comboBox_Weather_Text_Image.Enabled = b;
            comboBox_Weather_Text_Alignment.Enabled = b;
            numericUpDown_Weather_Text_Spacing.Enabled = b;
            numericUpDown_Weather_Text_Count.Enabled = b;

            label270.Enabled = b;
            label271.Enabled = b;
            label272.Enabled = b;
            label273.Enabled = b;
            label274.Enabled = b;
            label275.Enabled = b;
            label276.Enabled = b;
            label277.Enabled = b;
            label278.Enabled = b;
            label279.Enabled = b;
            //label284.Enabled = b;
            //label285.Enabled = b;
            //label286.Enabled = b;

            if ((checkBox_Weather_Text.Checked) || (checkBox_Weather_Day.Checked) || (checkBox_Weather_Night.Checked))
            {
                groupBox_Symbols.Enabled = true;
            }
            else
            {
                groupBox_Symbols.Enabled = false;
            }
        }

        private void checkBox_Weather_Icon_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Weather_Icon.Checked;
            numericUpDown_Weather_Icon_X.Enabled = b;
            numericUpDown_Weather_Icon_Y.Enabled = b;
            numericUpDown_Weather_Icon_Count.Enabled = b;
            comboBox_Weather_Icon_NDImage.Enabled = b;
            comboBox_Weather_Icon_Image.Enabled = b;

            label280.Enabled = b;
            label281.Enabled = b;
            label282.Enabled = b;
            label283.Enabled = b;
            label287.Enabled = b;
        }

        private void checkBox_Weather_Day_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Weather_Day.Checked;
            numericUpDown_Weather_Day_StartCorner_X.Enabled = b;
            numericUpDown_Weather_Day_StartCorner_Y.Enabled = b;
            numericUpDown_Weather_Day_EndCorner_X.Enabled = b;
            numericUpDown_Weather_Day_EndCorner_Y.Enabled = b;
            //comboBox_Weather_Text_MinusImage.Enabled = b;
            //comboBox_Weather_Text_DegImage.Enabled = b;
            //comboBox_Weather_Text_NDImage.Enabled = b;
            comboBox_Weather_Day_Image.Enabled = b;
            comboBox_Weather_Day_Alignment.Enabled = b;
            numericUpDown_Weather_Day_Spacing.Enabled = b;
            numericUpDown_Weather_Day_Count.Enabled = b;

            label289.Enabled = b;
            label290.Enabled = b;
            label291.Enabled = b;
            label292.Enabled = b;
            label293.Enabled = b;
            label294.Enabled = b;
            label295.Enabled = b;
            label296.Enabled = b;
            label297.Enabled = b;
            label298.Enabled = b;

            checkBox_Weather_Night.Checked = b;

            if ((checkBox_Weather_Text.Checked) || (checkBox_Weather_Day.Checked) || (checkBox_Weather_Night.Checked))
            {
                groupBox_Symbols.Enabled = true;
            }
            else
            {
                groupBox_Symbols.Enabled = false;
            }
        }

        private void checkBox_Weather_Night_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Weather_Night.Checked;
            numericUpDown_Weather_Night_StartCorner_X.Enabled = b;
            numericUpDown_Weather_Night_StartCorner_Y.Enabled = b;
            numericUpDown_Weather_Night_EndCorner_X.Enabled = b;
            numericUpDown_Weather_Night_EndCorner_Y.Enabled = b;
            //comboBox_Weather_Text_MinusImage.Enabled = b;
            //comboBox_Weather_Text_DegImage.Enabled = b;
            //comboBox_Weather_Text_NDImage.Enabled = b;
            comboBox_Weather_Night_Image.Enabled = b;
            comboBox_Weather_Night_Alignment.Enabled = b;
            numericUpDown_Weather_Night_Spacing.Enabled = b;
            numericUpDown_Weather_Night_Count.Enabled = b;

            label313.Enabled = b;
            label314.Enabled = b;
            label315.Enabled = b;
            label316.Enabled = b;
            label317.Enabled = b;
            label318.Enabled = b;
            label319.Enabled = b;
            label320.Enabled = b;
            label321.Enabled = b;
            label322.Enabled = b;

            checkBox_Weather_Day.Checked = b;

            if ((checkBox_Weather_Text.Checked) || (checkBox_Weather_Day.Checked) || (checkBox_Weather_Night.Checked))
            {
                groupBox_Symbols.Enabled = true;
            }
            else
            {
                groupBox_Symbols.Enabled = false;
            }
        }

        private void checkBox_Shortcuts_CheckedChanged(object sender, EventArgs e)
        {
            tabControl_Shortcuts.Enabled = checkBox_Shortcuts.Checked;
        }

        private void checkBox_Shortcuts_Steps_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Shortcuts_Steps.Checked;
            numericUpDown_Shortcuts_Steps_X.Enabled = b;
            numericUpDown_Shortcuts_Steps_Y.Enabled = b;
            numericUpDown_Shortcuts_Steps_Width.Enabled = b;
            numericUpDown_Shortcuts_Steps_Height.Enabled = b;

            label435.Enabled = b;
            label438.Enabled = b;
            label439.Enabled = b;
            label440.Enabled = b;
            label442.Enabled = b;
            label443.Enabled = b;

        }

        private void checkBox_Shortcuts_Puls_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Shortcuts_Puls.Checked;
            numericUpDown_Shortcuts_Puls_X.Enabled = b;
            numericUpDown_Shortcuts_Puls_Y.Enabled = b;
            numericUpDown_Shortcuts_Puls_Width.Enabled = b;
            numericUpDown_Shortcuts_Puls_Height.Enabled = b;

            label434.Enabled = b;
            label436.Enabled = b;
            label437.Enabled = b;
            label441.Enabled = b;
            label444.Enabled = b;
            label445.Enabled = b;
        }

        private void checkBox_Shortcuts_Weather_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Shortcuts_Weather.Checked;
            numericUpDown_Shortcuts_Weather_X.Enabled = b;
            numericUpDown_Shortcuts_Weather_Y.Enabled = b;
            numericUpDown_Shortcuts_Weather_Width.Enabled = b;
            numericUpDown_Shortcuts_Weather_Height.Enabled = b;

            label446.Enabled = b;
            label447.Enabled = b;
            label448.Enabled = b;
            label449.Enabled = b;
            label450.Enabled = b;
            label451.Enabled = b;
        }

        private void checkBox_Shortcuts_Energy_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Shortcuts_Energy.Checked;
            numericUpDown_Shortcuts_Energy_X.Enabled = b;
            numericUpDown_Shortcuts_Energy_Y.Enabled = b;
            numericUpDown_Shortcuts_Energy_Width.Enabled = b;
            numericUpDown_Shortcuts_Energy_Height.Enabled = b;

            label452.Enabled = b;
            label453.Enabled = b;
            label454.Enabled = b;
            label455.Enabled = b;
            label456.Enabled = b;
            label457.Enabled = b;
        }

        private void checkBox_Animation_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Animation.Checked;
            tabControl_Animation.Enabled = b;
            if (checkBox_Animation.Checked)
            {
                if (checkBox_StaticAnimation.Checked || checkBox_MotiomAnimation.Checked) button_ShowAnimation.Enabled = true;
                else button_ShowAnimation.Enabled = false;
            }
            else button_ShowAnimation.Enabled = false;
        }

        private void checkBox_StaticAnimation_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_StaticAnimation.Checked;
            numericUpDown_StaticAnimation_X.Enabled = b;
            numericUpDown_StaticAnimation_Y.Enabled = b;
            comboBox_StaticAnimation_Image.Enabled = b;
            numericUpDown_StaticAnimation_Count.Enabled = b;

            numericUpDown_StaticAnimation_CyclesCount.Enabled = b;
            numericUpDown_StaticAnimation_SpeedAnimation.Enabled = b;
            numericUpDown_StaticAnimation_TimeAnimation.Enabled = b;
            numericUpDown_StaticAnimation_Pause.Enabled = b;

            label411.Enabled = b;
            label416.Enabled = b;
            label473.Enabled = b;
            label474.Enabled = b;
            label475.Enabled = b;
            label476.Enabled = b;
            label477.Enabled = b;
            label478.Enabled = b;
            label479.Enabled = b;

            if (checkBox_Animation.Checked)
            {
                if (checkBox_StaticAnimation.Checked || checkBox_MotiomAnimation.Checked) button_ShowAnimation.Enabled = true;
                else button_ShowAnimation.Enabled = false;
            }
            else button_ShowAnimation.Enabled = false;
        }

        private void checkBox_MotiomAnimation_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_MotiomAnimation.Checked;
            comboBox_MotiomAnimation_Image.Enabled = b;
            dataGridView_MotiomAnimation.Enabled = b;
            radioButton_MotiomAnimation_StartCoordinates.Enabled = b;
            radioButton_MotiomAnimation_EndCoordinates.Enabled = b;
            numericUpDown_MotiomAnimation_StartCoordinates_X.Enabled = b;
            numericUpDown_MotiomAnimation_StartCoordinates_Y.Enabled = b;
            numericUpDown_MotiomAnimation_EndCoordinates_X.Enabled = b;
            numericUpDown_MotiomAnimation_EndCoordinates_Y.Enabled = b;

            groupBox_MotiomAnimation.Enabled = b;

            label480.Enabled = b;
            label481.Enabled = b;
            label482.Enabled = b;
            label484.Enabled = b;
            label485.Enabled = b;

            if (checkBox_Animation.Checked)
            {
                if (checkBox_StaticAnimation.Checked || checkBox_MotiomAnimation.Checked) button_ShowAnimation.Enabled = true;
                else button_ShowAnimation.Enabled = false;
            }
            else button_ShowAnimation.Enabled = false;
        }
        #endregion

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)(sender);
            //int i = 0;
            //if(Int32.TryParse(comboBox.Text, out i))
            //{
                //try
                //{
                //    using (FileStream stream = new FileStream(ListImagesFullName[comboBox.SelectedIndex], FileMode.Open, FileAccess.Read))
                //    {
                //        pictureBox1.Image = Image.FromStream(stream);
                //        timer1.Enabled = true;
                //    }
                //}
                //catch { }
            //}
            //pictureBox1.Image = null;
            JSON_write();
            PreviewImage();

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

        private void checkBox_Delimiter_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Delimiter.Checked)
            {
                numericUpDown_Delimiter_X.Enabled = true;
                numericUpDown_Delimiter_Y.Enabled = true;
                comboBox_Delimiter_Image.Enabled = true;

                label37.Enabled = true;
                label39.Enabled = true;
                label38.Enabled = true;
            }
            else
            {
                numericUpDown_Delimiter_X.Enabled = false;
                numericUpDown_Delimiter_Y.Enabled = false;
                comboBox_Delimiter_Image.Enabled = false;

                label37.Enabled = false;
                label39.Enabled = false;
                label38.Enabled = false;
            }
        }

        private void checkBox_Click(object sender, EventArgs e)
        {
            JSON_write();
            PreviewImage();
        }

        private void checkBox_ShowSettings_Click(object sender, EventArgs e)
        {
            PreviewImage();
        }

        private void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            JSON_write();
            PreviewImage();
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
            panel_Set11.Height = 1;
            panel_Set12.Height = 1;
            panel_Set13.Height = 1;
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
            panel_Set10.Height = 1;
            panel_Set11.Height = 1;
            panel_Set12.Height = 1;
            panel_Set13.Height = 1;
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
            panel_Set10.Height = 1;
            panel_Set11.Height = 1;
            panel_Set12.Height = 1;
            panel_Set13.Height = 1;
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
            panel_Set11.Height = 1;
            panel_Set12.Height = 1;
            panel_Set13.Height = 1;
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
            panel_Set11.Height = 1;
            panel_Set12.Height = 1;
            panel_Set13.Height = 1;
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
            panel_Set11.Height = 1;
            panel_Set12.Height = 1;
            panel_Set13.Height = 1;
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
            panel_Set11.Height = 1;
            panel_Set12.Height = 1;
            panel_Set13.Height = 1;
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
            panel_Set11.Height = 1;
            panel_Set12.Height = 1;
            panel_Set13.Height = 1;
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
            panel_Set11.Height = 1;
            panel_Set12.Height = 1;
            panel_Set13.Height = 1;
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
            panel_Set11.Height = 1;
            panel_Set12.Height = 1;
            panel_Set13.Height = 1;
            SetPreferences10();
            PreviewImage();
        }
        private void button_Set11_Click(object sender, EventArgs e)
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
            panel_Set10.Height = 1;
            panel_Set11.Height = (int)(125 * currentDPI);
            panel_Set12.Height = 1;
            panel_Set13.Height = 1;
            SetPreferences11();
            PreviewImage();
        }
        private void button_Set12_Click(object sender, EventArgs e)
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
            panel_Set10.Height = 1;
            panel_Set11.Height = 1;
            panel_Set12.Height = (int)(125 * currentDPI);
            panel_Set13.Height = 1;
            SetPreferences12();
            PreviewImage();
        }
        private void button_Set13_Click(object sender, EventArgs e)
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
            panel_Set10.Height = 1;
            panel_Set11.Height = 1;
            panel_Set12.Height = 1;
            panel_Set13.Height = (int)(125 * currentDPI);
            SetPreferences13();
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
            panel_Set11.Height = 1;
            panel_Set12.Height = 1;
            panel_Set13.Height = 1;
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
        private void dateTimePicker_Time_Set11_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences11();
            PreviewImage();
        }
        private void dateTimePicker_Time_Set12_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences12();
            PreviewImage();
        }
        private void dateTimePicker_Time_Set13_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences13();
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
        private void numericUpDown_Battery_Set11_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences11();
            PreviewImage();
        }
        private void numericUpDown_Battery_Set12_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences12();
            PreviewImage();
        }
        private void numericUpDown_Battery_Set13_ValueChanged(object sender, EventArgs e)
        {
            SetPreferences13();
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
        private void check_BoxBluetooth_Set11_CheckedChanged(object sender, EventArgs e)
        {
            SetPreferences11();
            PreviewImage();
        }
        private void check_BoxBluetooth_Set12_CheckedChanged(object sender, EventArgs e)
        {
            SetPreferences12();
            PreviewImage();
        }
        private void check_BoxBluetooth_Set13_CheckedChanged(object sender, EventArgs e)
        {
            SetPreferences13();
            PreviewImage();
        }
#endregion

        // переключаем цвет фона в таблице с картинками
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                dataGridView_ImagesList.DefaultCellStyle.BackColor = Color.Black;
                dataGridView_ImagesList.DefaultCellStyle.ForeColor = Color.White;
            }
            else
            {
                dataGridView_ImagesList.DefaultCellStyle.BackColor = Color.White;
                dataGridView_ImagesList.DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void comboBox_StepsProgress_Color_Click(object sender, EventArgs e)
        {
            if (colorDialog_StepsProgress.ShowDialog() == DialogResult.Cancel)
                return;
            // установка цвета формы
            comboBox_StepsProgress_Color.BackColor = colorDialog_StepsProgress.Color; PreviewImage();
            JSON_write();
            PreviewImage();
        }
        
        private void numericUpDown_StepsProgress_Radius_X_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown_StepsProgress_Radius_Y.Value = numericUpDown_StepsProgress_Radius_X.Value;
            PreviewImage();
        }
        
        private void comboBox_Battery_Scale_Color_Click(object sender, EventArgs e)
        {
            if (colorDialog_Battery.ShowDialog() == DialogResult.Cancel)
                return;
            // установка цвета формы
            comboBox_Battery_Scale_Color.BackColor = colorDialog_Battery.Color;
            JSON_write();
            PreviewImage();
        }

        private void comboBox_ActivityPulsScale_Color_Click(object sender, EventArgs e)
        {
            if (colorDialog_Pulse.ShowDialog() == DialogResult.Cancel)
                return;
            // установка цвета формы
            comboBox_ActivityPulsScale_Color.BackColor = colorDialog_Pulse.Color;
            JSON_write();
            PreviewImage();
        }

        private void comboBox_ActivityCaloriesScale_Color_Click(object sender, EventArgs e)
        {
            if (colorDialog_Calories.ShowDialog() == DialogResult.Cancel)
                return;
            // установка цвета формы
            comboBox_ActivityCaloriesScale_Color.BackColor = colorDialog_Calories.Color;
            JSON_write();
            PreviewImage();
        }


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

                    PreviewToBitmap(gPanelPreviewResize, 1, checkBox_crop.Checked,
                        checkBox_WebW.Checked, checkBox_WebB.Checked, checkBox_border.Checked, 
                        checkBox_Show_Shortcuts.Checked, checkBox_Shortcuts_Area.Checked, checkBox_Shortcuts_Border.Checked, true,
                        checkBox_CircleScaleImage.Checked, checkBox_center_marker.Checked, 0);
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

            PreviewToBitmap(gPanel, scale, checkBox_crop.Checked, checkBox_WebW.Checked, checkBox_WebB.Checked, 
                checkBox_border.Checked, checkBox_Show_Shortcuts.Checked, checkBox_Shortcuts_Area.Checked, 
                checkBox_Shortcuts_Border.Checked, true, checkBox_CircleScaleImage.Checked,
                checkBox_center_marker.Checked, 0);
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
                        case 10:
                            dateTimePicker_Date_Set11.Value = new DateTime(year, month, day, hour, min, sec);
                            dateTimePicker_Time_Set11.Value = new DateTime(year, month, day, hour, min, sec);
                            numericUpDown_Battery_Set11.Value = battery;
                            numericUpDown_Calories_Set11.Value = calories;
                            numericUpDown_Pulse_Set11.Value = pulse;
                            numericUpDown_Distance_Set11.Value = distance;
                            numericUpDown_Steps_Set11.Value = steps;
                            numericUpDown_Goal_Set11.Value = goal;
                            check_BoxBluetooth_Set11.Checked = bluetooth;
                            checkBox_Alarm_Set11.Checked = alarm;
                            checkBox_Lock_Set11.Checked = unlocked;
                            checkBox_DoNotDisturb_Set11.Checked = dnd;
                            button_Set11.PerformClick();
                            break;
                        case 11:
                            dateTimePicker_Date_Set12.Value = new DateTime(year, month, day, hour, min, sec);
                            dateTimePicker_Time_Set12.Value = new DateTime(year, month, day, hour, min, sec);
                            numericUpDown_Battery_Set12.Value = battery;
                            numericUpDown_Calories_Set12.Value = calories;
                            numericUpDown_Pulse_Set12.Value = pulse;
                            numericUpDown_Distance_Set12.Value = distance;
                            numericUpDown_Steps_Set12.Value = steps;
                            numericUpDown_Goal_Set12.Value = goal;
                            check_BoxBluetooth_Set12.Checked = bluetooth;
                            checkBox_Alarm_Set12.Checked = alarm;
                            checkBox_Lock_Set12.Checked = unlocked;
                            checkBox_DoNotDisturb_Set12.Checked = dnd;
                            button_Set12.PerformClick();
                            break;
                        case 12:
                            dateTimePicker_Date_Set13.Value = new DateTime(year, month, day, hour, min, sec);
                            dateTimePicker_Time_Set13.Value = new DateTime(year, month, day, hour, min, sec);
                            numericUpDown_Battery_Set13.Value = battery;
                            numericUpDown_Calories_Set13.Value = calories;
                            numericUpDown_Pulse_Set13.Value = pulse;
                            numericUpDown_Distance_Set13.Value = distance;
                            numericUpDown_Steps_Set13.Value = steps;
                            numericUpDown_Goal_Set13.Value = goal;
                            check_BoxBluetooth_Set13.Checked = bluetooth;
                            checkBox_Alarm_Set13.Checked = alarm;
                            checkBox_Lock_Set13.Checked = unlocked;
                            checkBox_DoNotDisturb_Set13.Checked = dnd;
                            button_Set13.PerformClick();
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
                    case 10:
                        ps.Time.Year = dateTimePicker_Date_Set11.Value.Year;
                        ps.Time.Month = dateTimePicker_Date_Set11.Value.Month;
                        ps.Time.Day = dateTimePicker_Date_Set11.Value.Day;
                        ps.Time.Hour = dateTimePicker_Time_Set11.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Time_Set11.Value.Minute;
                        ps.Time.Second = dateTimePicker_Time_Set11.Value.Second;
                        ps.BatteryLevel = (int)numericUpDown_Battery_Set11.Value;
                        ps.Calories = (int)numericUpDown_Calories_Set11.Value;
                        ps.Pulse = (int)numericUpDown_Pulse_Set11.Value;
                        ps.Distance = (int)numericUpDown_Distance_Set11.Value;
                        ps.Steps = (int)numericUpDown_Steps_Set11.Value;
                        ps.Goal = (int)numericUpDown_Goal_Set11.Value;
                        ps.Bluetooth = check_BoxBluetooth_Set11.Checked;
                        ps.Alarm = checkBox_Alarm_Set10.Checked;
                        ps.Unlocked = checkBox_Lock_Set11.Checked;
                        ps.DoNotDisturb = checkBox_DoNotDisturb_Set11.Checked;

                        if (numericUpDown_Calories_Set11.Value != 1234)
                        {
                            Array.Resize(ref objson, objson.Length + 1);
                            objson[count] = ps;
                            count++;
                        }
                        break;
                    case 11:
                        ps.Time.Year = dateTimePicker_Date_Set12.Value.Year;
                        ps.Time.Month = dateTimePicker_Date_Set12.Value.Month;
                        ps.Time.Day = dateTimePicker_Date_Set12.Value.Day;
                        ps.Time.Hour = dateTimePicker_Time_Set12.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Time_Set12.Value.Minute;
                        ps.Time.Second = dateTimePicker_Time_Set12.Value.Second;
                        ps.BatteryLevel = (int)numericUpDown_Battery_Set12.Value;
                        ps.Calories = (int)numericUpDown_Calories_Set12.Value;
                        ps.Pulse = (int)numericUpDown_Pulse_Set12.Value;
                        ps.Distance = (int)numericUpDown_Distance_Set12.Value;
                        ps.Steps = (int)numericUpDown_Steps_Set12.Value;
                        ps.Goal = (int)numericUpDown_Goal_Set12.Value;
                        ps.Bluetooth = check_BoxBluetooth_Set12.Checked;
                        ps.Alarm = checkBox_Alarm_Set12.Checked;
                        ps.Unlocked = checkBox_Lock_Set12.Checked;
                        ps.DoNotDisturb = checkBox_DoNotDisturb_Set12.Checked;

                        if (numericUpDown_Calories_Set12.Value != 1234)
                        {
                            Array.Resize(ref objson, objson.Length + 1);
                            objson[count] = ps;
                            count++;
                        }
                        break;
                    case 12:
                        ps.Time.Year = dateTimePicker_Date_Set13.Value.Year;
                        ps.Time.Month = dateTimePicker_Date_Set13.Value.Month;
                        ps.Time.Day = dateTimePicker_Date_Set13.Value.Day;
                        ps.Time.Hour = dateTimePicker_Time_Set13.Value.Hour;
                        ps.Time.Minute = dateTimePicker_Time_Set13.Value.Minute;
                        ps.Time.Second = dateTimePicker_Time_Set13.Value.Second;
                        ps.BatteryLevel = (int)numericUpDown_Battery_Set13.Value;
                        ps.Calories = (int)numericUpDown_Calories_Set13.Value;
                        ps.Pulse = (int)numericUpDown_Pulse_Set13.Value;
                        ps.Distance = (int)numericUpDown_Distance_Set13.Value;
                        ps.Steps = (int)numericUpDown_Steps_Set13.Value;
                        ps.Goal = (int)numericUpDown_Goal_Set13.Value;
                        ps.Bluetooth = check_BoxBluetooth_Set13.Checked;
                        ps.Alarm = checkBox_Alarm_Set13.Checked;
                        ps.Unlocked = checkBox_Lock_Set13.Checked;
                        ps.DoNotDisturb = checkBox_DoNotDisturb_Set13.Checked;

                        if (numericUpDown_Calories_Set13.Value != 1234)
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
            for (int i = 0; i < 13; i++)
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
                        //button_Set10.PerformClick();
                        break;
                    case 10:
                        dateTimePicker_Date_Set11.Value = new DateTime(year, month, day, hour, min, sec);
                        dateTimePicker_Time_Set11.Value = new DateTime(year, month, day, hour, min, sec);
                        numericUpDown_Battery_Set11.Value = battery;
                        numericUpDown_Calories_Set11.Value = calories;
                        numericUpDown_Pulse_Set11.Value = pulse;
                        numericUpDown_Distance_Set11.Value = distance;
                        numericUpDown_Steps_Set11.Value = steps;
                        numericUpDown_Goal_Set11.Value = goal;
                        check_BoxBluetooth_Set11.Checked = bluetooth;
                        checkBox_Alarm_Set11.Checked = alarm;
                        checkBox_Lock_Set11.Checked = unlocked;
                        checkBox_DoNotDisturb_Set11.Checked = dnd;
                        //button_Set11.PerformClick();
                        break;
                    case 11:
                        dateTimePicker_Date_Set12.Value = new DateTime(year, month, day, hour, min, sec);
                        dateTimePicker_Time_Set12.Value = new DateTime(year, month, day, hour, min, sec);
                        numericUpDown_Battery_Set12.Value = battery;
                        numericUpDown_Calories_Set12.Value = calories;
                        numericUpDown_Pulse_Set12.Value = pulse;
                        numericUpDown_Distance_Set12.Value = distance;
                        numericUpDown_Steps_Set12.Value = steps;
                        numericUpDown_Goal_Set12.Value = goal;
                        check_BoxBluetooth_Set12.Checked = bluetooth;
                        checkBox_Alarm_Set12.Checked = alarm;
                        checkBox_Lock_Set12.Checked = unlocked;
                        checkBox_DoNotDisturb_Set12.Checked = dnd;
                        //button_Set12.PerformClick();
                        break;
                    case 12:
                        dateTimePicker_Date_Set13.Value = new DateTime(year, month, day, hour, min, sec);
                        dateTimePicker_Time_Set13.Value = new DateTime(year, month, day, hour, min, sec);
                        numericUpDown_Battery_Set13.Value = battery;
                        numericUpDown_Calories_Set13.Value = calories;
                        numericUpDown_Pulse_Set13.Value = pulse;
                        numericUpDown_Distance_Set13.Value = distance;
                        numericUpDown_Steps_Set13.Value = steps;
                        numericUpDown_Goal_Set13.Value = goal;
                        check_BoxBluetooth_Set13.Checked = bluetooth;
                        checkBox_Alarm_Set13.Checked = alarm;
                        checkBox_Lock_Set13.Checked = unlocked;
                        checkBox_DoNotDisturb_Set13.Checked = dnd;
                        //button_Set13.PerformClick();
                        break;
                }
            }

            numericUpDown_WeatherSet_Temp.Value = rnd.Next(-25, 35) + 1;
            numericUpDown_WeatherSet_DayTemp.Value = numericUpDown_WeatherSet_Temp.Value;
            numericUpDown_WeatherSet_NightTemp.Value = numericUpDown_WeatherSet_Temp.Value - rnd.Next(3, 10);
            comboBox_WeatherSet_Icon.SelectedIndex = rnd.Next(0, 25);

            PreviewView = true;
            //PreviewImage();
            button_Set13.PerformClick();
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

        private void numericUpDown_Battery_Scale_Radius_X_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown_Battery_Scale_Radius_Y.Value = numericUpDown_Battery_Scale_Radius_X.Value;
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
            if (Watch_Face.AnalogDialFace != null)
            {
                int i = 0;
                if ((Watch_Face.AnalogDialFace.Hours != null) && (Watch_Face.AnalogDialFace.Hours.Image != null)) i++;
                if ((Watch_Face.AnalogDialFace.Minutes != null) && (Watch_Face.AnalogDialFace.Minutes.Image != null)) i++;
                if ((Watch_Face.AnalogDialFace.Seconds != null) && (Watch_Face.AnalogDialFace.Seconds.Image != null)) i++;
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
                if ((Watch_Face.StepsProgress.ClockHand != null && Watch_Face.StepsProgress.ClockHand.Image != null) 
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
                    //if (Watch_Face.StepsProgress != null && Watch_Face.StepsProgress.ClockHand != null)
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

        private void checkBox_Weather_CheckedChanged(object sender, EventArgs e)
        {
            tabControl_Weather.Enabled = checkBox_Weather.Checked;
            if ((checkBox_Weather_Text.Checked) || (checkBox_Weather_Day.Checked) || (checkBox_Weather_Night.Checked))
            {
                groupBox_Symbols.Enabled = true;
            }
            else
            {
                groupBox_Symbols.Enabled = false;
            }
        }

        private void comboBox_WeatherSet_Icon_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Delete) || (e.KeyCode == Keys.Back))
            {
                comboBox_WeatherSet_Icon.Text = "";
                comboBox_WeatherSet_Icon.SelectedIndex = -1;
                //JSON_write();
                PreviewImage();
            }
        }

        private void checkBox_WeatherSet_Temp_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown_WeatherSet_Temp.Enabled = checkBox_WeatherSet_Temp.Checked;
        }

        private void checkBox_WeatherSet_DayTemp_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown_WeatherSet_NightTemp.Enabled = checkBox_WeatherSet_DayTemp.Checked;
            numericUpDown_WeatherSet_DayTemp.Enabled = checkBox_WeatherSet_DayTemp.Checked;
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
                PreviewToBitmap(gPanel, 1.0f, false, false, false, false, false, false, false, true, false, false, 0);
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
                            case 10:
                                if (numericUpDown_Calories_Set11.Value != 1234)
                                {
                                    //button_Set11.PerformClick();
                                    SetPreferences11();
                                    save = true;
                                }
                                break;
                            case 11:
                                if (numericUpDown_Calories_Set12.Value != 1234)
                                {
                                    //button_Set12.PerformClick();
                                    SetPreferences12();
                                    save = true;
                                }
                                break;
                            case 12:
                                if (numericUpDown_Calories_Set13.Value != 1234)
                                {
                                    //button_Set13.PerformClick();
                                    SetPreferences13();
                                    save = true;
                                }
                                break;
                        }

                        if (save)
                        {
                            Logger.WriteLine("SaveGIF SetPreferences1(" + i.ToString() + ")");
                            int WeatherSet_Temp = (int)numericUpDown_WeatherSet_Temp.Value;
                            int WeatherSet_DayTemp = (int)numericUpDown_WeatherSet_DayTemp.Value;
                            int WeatherSet_NightTemp = (int)numericUpDown_WeatherSet_NightTemp.Value;
                            int WeatherSet_Icon = comboBox_WeatherSet_Icon.SelectedIndex;

                            numericUpDown_WeatherSet_Temp.Value = rnd.Next(-25, 35) + 1;
                            numericUpDown_WeatherSet_DayTemp.Value = numericUpDown_WeatherSet_Temp.Value;
                            numericUpDown_WeatherSet_NightTemp.Value = numericUpDown_WeatherSet_Temp.Value - rnd.Next(3, 10);
                            comboBox_WeatherSet_Icon.SelectedIndex = rnd.Next(0, 25);

                            PreviewToBitmap(gPanel, 1.0f, false, false, false, false, false, false, false, true, false, false, 0);
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

                            numericUpDown_WeatherSet_Temp.Value = WeatherSet_Temp;
                            numericUpDown_WeatherSet_DayTemp.Value = WeatherSet_DayTemp;
                            numericUpDown_WeatherSet_NightTemp.Value = WeatherSet_NightTemp;
                            comboBox_WeatherSet_Icon.SelectedIndex = WeatherSet_Icon;
                        }
                    }
                    
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


            JSON_write();
            PreviewImage();
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
            }
            else
            {
                if (JSON_Modified) FormNameSufix = FormNameSufix + "*";
                this.Text = FormName + " (" + FormNameSufix + ")";
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
                JSON_write();
                PreviewImage();
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
            JSON_write();
            PreviewImage();
        }

        private void dataGridView_MotiomAnimation_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView_MotiomAnimation.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && e.ColumnIndex < 11)
            {
                Regex my_reg = new Regex(@"[^-\d]");
                string oldValue = dataGridView_MotiomAnimation.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                string newValue = my_reg.Replace(oldValue, "");
                int v = 0;
                Int32.TryParse(newValue, out v);
                if (e.ColumnIndex == 6)
                {
                    if (v < 100) v = 100;
                }
                dataGridView_MotiomAnimation.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = v;
                //dataGridView_MotiomAnimation.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = newValue;
                if (newValue.Length == 0) dataGridView_MotiomAnimation.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = null;
            }

            try
            {
                if ((dataGridView_MotiomAnimation.Rows[e.RowIndex].Cells[1].Value == null) &&
                    (dataGridView_MotiomAnimation.Rows[e.RowIndex].Cells[2].Value == null) &&
                    (dataGridView_MotiomAnimation.Rows[e.RowIndex].Cells[3].Value == null) &&
                    (dataGridView_MotiomAnimation.Rows[e.RowIndex].Cells[4].Value == null) &&
                    (dataGridView_MotiomAnimation.Rows[e.RowIndex].Cells[5].Value == null) &&
                    (dataGridView_MotiomAnimation.Rows[e.RowIndex].Cells[6].Value == null) &&
                    (dataGridView_MotiomAnimation.Rows[e.RowIndex].Cells[7].Value == null) && 
                        (e.RowIndex < dataGridView_MotiomAnimation.Rows.Count - 1))
                    dataGridView_MotiomAnimation.Rows.RemoveAt(e.RowIndex);
            }
            catch (Exception)
            {
            }
        
            JSON_write();
            PreviewImage();
        }

        private void dataGridView_IconSet_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            JSON_write();
            PreviewImage();
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

        private void dataGridView_MotiomAnimation_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 11)
            {
                DataGridViewCheckBoxCell chBounce = new DataGridViewCheckBoxCell();
                chBounce = (DataGridViewCheckBoxCell)dataGridView_MotiomAnimation.Rows[e.RowIndex].Cells[11];

                if (chBounce.Value == null)
                    chBounce.Value = false;
                switch (chBounce.Value.ToString())
                {
                    case "True":
                        chBounce.Value = false;
                        break;
                    case "False":
                        chBounce.Value = true;
                        break;
                }
                JSON_write();
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
                    JSON_write();
                    PreviewImage();
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
                            JSON_write();
                            PreviewImage();
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
            System.Diagnostics.Process.Start("https://github.com/amazfitbip/py_amazfit_tools/releases/tag/v0.2-beta");
        }

        private void linkLabel_resunpacker_qzip_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/amazfitbip/resunpacker_qzip");
        }

        private void linkLabel_help_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //helpProvider1.SetHelpNavigator(this, HelpNavigator.Topic);
            //helpProvider1.SetHelpKeyword(this, "kratkaya_instruktsiya.htm");
            //SendKeys.Send("{F1}");
            //Help.ShowHelp(this, Application.StartupPath + Properties.FormStrings.File_ReadMy);
            string help_file = Application.StartupPath + Properties.FormStrings.File_ReadMy;
            //string help_start = Properties.FormStrings.File_ReadMy_Start;
            HelpNavigator navigator = HelpNavigator.Topic;
            //Help.ShowHelp(this, help_file, navigator, help_start);
            Help.ShowHelp(this, help_file, navigator, "quick_guide.htm");
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

        private void checkBox_ShowMiles_CheckedChanged(object sender, EventArgs e)
        {
            if (Settings_Load) return;
            Program_Settings.ShowMiles = checkBox_ShowMiles.Checked;
            Program_Settings.ShowIn12hourFormat = checkBox_ShowIn12hourFormat.Checked;
            Program_Settings.DoNotShowMaxMinTemp = checkBox_DoNotShowMaxMinTemp.Checked;

            string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);
        }

        private void numericUpDown_ActivityPulsScale_Radius_X_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown_ActivityPulsScale_Radius_Y.Value = numericUpDown_ActivityPulsScale_Radius_X.Value;
            PreviewImage();
        }

        private void numericUpDown_ActivityCaloriesScale_Radius_X_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown_ActivityCaloriesScale_Radius_Y.Value = numericUpDown_ActivityCaloriesScale_Radius_X.Value;
            PreviewImage();
        }

        private void checkBox_WeatherSet_Temp_Click(object sender, EventArgs e)
        {
            PreviewImage();
        }

        private void numericUpDown_WeatherSet_Temp_ValueChanged(object sender, EventArgs e)
        {
            PreviewImage();
        }

        private void comboBox_WeatherSet_Icon_SelectedIndexChanged(object sender, EventArgs e)
        {
            PreviewImage();
        }

        private void numericUpDown_StaticAnimation_CyclesCount_ValueChanged(object sender, EventArgs e)
        {
            int Animation_Count = (int)numericUpDown_StaticAnimation_Count.Value;
            int Animation_CyclesCount = (int)numericUpDown_StaticAnimation_CyclesCount.Value;
            //int Animation_TimeAnimation = (int)numericUpDown_StaticAnimation_TimeAnimation.Value;
            int Animation_SpeedAnimation = (int)numericUpDown_StaticAnimation_SpeedAnimation.Value;
            int Animation_TimeAnimation = (Animation_Count * Animation_CyclesCount * Animation_SpeedAnimation) - Animation_SpeedAnimation;
            if (Animation_TimeAnimation < 0) Animation_TimeAnimation = 0;
            if (Animation_TimeAnimation != (int)numericUpDown_StaticAnimation_TimeAnimation.Value && Animation_CyclesCount >= 0)
                numericUpDown_StaticAnimation_TimeAnimation.Value = Animation_TimeAnimation;
        }

        private void numericUpDown_StaticAnimation_TimeAnimation_ValueChanged(object sender, EventArgs e)
        {
            int Animation_Count = (int)numericUpDown_StaticAnimation_Count.Value;
            //int Animation_CyclesCount = (int)numericUpDown_StaticAnimation_CyclesCount.Value;
            int Animation_TimeAnimation = (int)numericUpDown_StaticAnimation_TimeAnimation.Value;
            int Animation_SpeedAnimation = (int)numericUpDown_StaticAnimation_SpeedAnimation.Value;
            int Animation_CyclesCount = (Animation_TimeAnimation + Animation_SpeedAnimation) /
                (Animation_SpeedAnimation * Animation_Count);
            if (Animation_CyclesCount != (Animation_TimeAnimation + Animation_SpeedAnimation) / 
                (float)(Animation_SpeedAnimation * Animation_Count)) Animation_CyclesCount = -1;
            if (Animation_TimeAnimation == 0) Animation_CyclesCount = 0;
            if (Animation_CyclesCount != (int)numericUpDown_StaticAnimation_CyclesCount.Value)
                numericUpDown_StaticAnimation_CyclesCount.Value = Animation_CyclesCount;

            JSON_write();
            JSON_Modified = true;
            FormText();
        }

        private void numericUpDown_StaticAnimation_Count_ValueChanged(object sender, EventArgs e)
        {
            int Animation_CyclesCount = (int)numericUpDown_StaticAnimation_CyclesCount.Value;
            if (Animation_CyclesCount >= 0)
            {
                int Animation_Count = (int)numericUpDown_StaticAnimation_Count.Value;
                int Animation_SpeedAnimation = (int)numericUpDown_StaticAnimation_SpeedAnimation.Value;
                int Animation_TimeAnimation = (Animation_Count * Animation_CyclesCount * Animation_SpeedAnimation) - Animation_SpeedAnimation;
                if (Animation_TimeAnimation < 0) Animation_TimeAnimation = 0;
                if (Animation_TimeAnimation != (int)numericUpDown_StaticAnimation_TimeAnimation.Value && Animation_CyclesCount != 0)
                    numericUpDown_StaticAnimation_TimeAnimation.Value = Animation_TimeAnimation;
            }
            JSON_write();
            JSON_Modified = true;
            FormText();
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
                            JSON_write();
                            PreviewImage();
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
                    numericUpDown_MotiomAnimation_StartCoordinates_X.Value = StartCoordinates_X;
                    numericUpDown_MotiomAnimation_StartCoordinates_Y.Value = StartCoordinates_Y;
                    numericUpDown_MotiomAnimation_EndCoordinates_X.Value = EndCoordinates_X;
                    numericUpDown_MotiomAnimation_EndCoordinates_Y.Value = EndCoordinates_Y;
                    comboBox_MotiomAnimation_Image.Text = "";

                    if (row.Cells[1].Value != null) Int32.TryParse(row.Cells[1].Value.ToString(), out StartCoordinates_X);
                    if (row.Cells[2].Value != null) Int32.TryParse(row.Cells[2].Value.ToString(), out StartCoordinates_Y);
                    if (row.Cells[3].Value != null) Int32.TryParse(row.Cells[3].Value.ToString(), out EndCoordinates_X);
                    if (row.Cells[4].Value != null) Int32.TryParse(row.Cells[4].Value.ToString(), out EndCoordinates_Y);

                    numericUpDown_MotiomAnimation_StartCoordinates_X.Value = StartCoordinates_X;
                    numericUpDown_MotiomAnimation_StartCoordinates_Y.Value = StartCoordinates_Y;
                    numericUpDown_MotiomAnimation_EndCoordinates_X.Value = EndCoordinates_X;
                    numericUpDown_MotiomAnimation_EndCoordinates_Y.Value = EndCoordinates_Y;

                    if (row.Cells[5].Value != null && Int32.TryParse(row.Cells[5].Value.ToString(), out ImageIndex))
                    {
                        comboBoxSetText(comboBox_MotiomAnimation_Image, ImageIndex);
                    }
                    else
                    {
                        comboBox_MotiomAnimation_Image.Text = "";
                    }
                    MotiomAnimation_Update = false;

                    JSON_write();
                    PreviewImage();
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
                    numericUpDown_MotiomAnimation_StartCoordinates_X.Value = StartCoordinates_X;
                    numericUpDown_MotiomAnimation_StartCoordinates_Y.Value = StartCoordinates_Y;
                    numericUpDown_MotiomAnimation_EndCoordinates_X.Value = EndCoordinates_X;
                    numericUpDown_MotiomAnimation_EndCoordinates_Y.Value = EndCoordinates_Y;
                    comboBox_MotiomAnimation_Image.Text = "";

                    if (row.Cells[1].Value != null) Int32.TryParse(row.Cells[1].Value.ToString(), out StartCoordinates_X);
                    if (row.Cells[2].Value != null) Int32.TryParse(row.Cells[2].Value.ToString(), out StartCoordinates_Y);
                    if (row.Cells[3].Value != null) Int32.TryParse(row.Cells[3].Value.ToString(), out EndCoordinates_X);
                    if (row.Cells[4].Value != null) Int32.TryParse(row.Cells[4].Value.ToString(), out EndCoordinates_Y);

                    numericUpDown_MotiomAnimation_StartCoordinates_X.Value = StartCoordinates_X;
                    numericUpDown_MotiomAnimation_StartCoordinates_Y.Value = StartCoordinates_Y;
                    numericUpDown_MotiomAnimation_EndCoordinates_X.Value = EndCoordinates_X;
                    numericUpDown_MotiomAnimation_EndCoordinates_Y.Value = EndCoordinates_Y;

                    if (row.Cells[5].Value != null && Int32.TryParse(row.Cells[5].Value.ToString(), out ImageIndex))
                    {
                        comboBoxSetText(comboBox_MotiomAnimation_Image, ImageIndex);
                    }
                    else
                    {
                        comboBox_MotiomAnimation_Image.Text = "";
                    }
                    MotiomAnimation_Update = false;

                    JSON_write();
                    PreviewImage();
                }
            }
        }

        // копируем данные из полей для редактирования
        private void comboBox_MotiomAnimation_Image_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MotiomAnimation_Update) return;
            DataGridViewRow row = dataGridView_MotiomAnimation.CurrentRow;
            if (row != null)
            {
                row.Cells[5].Value = comboBox_MotiomAnimation_Image.Text;

                JSON_write();
                PreviewImage(); 
            }
        }

        // копируем данные из полей для редактирования
        private void numericUpDown_MotiomAnimation_StartCoordinates_X_ValueChanged(object sender, EventArgs e)
        {
            if (MotiomAnimation_Update) return;
            DataGridViewRow row = dataGridView_MotiomAnimation.CurrentRow;
            if (row != null)
            {
                row.Cells[1].Value = numericUpDown_MotiomAnimation_StartCoordinates_X.Value;
                row.Cells[2].Value = numericUpDown_MotiomAnimation_StartCoordinates_Y.Value;
                row.Cells[3].Value = numericUpDown_MotiomAnimation_EndCoordinates_X.Value;
                row.Cells[4].Value = numericUpDown_MotiomAnimation_EndCoordinates_Y.Value;

                JSON_write();
                PreviewImage(); 
            }
        }

        private void dataGridView_MotiomAnimation_SelectionChanged(object sender, EventArgs e)
        {
            MotiomAnimation_Update = true;
            int StartCoordinates_X = 0;
            int StartCoordinates_Y = 0;
            int EndCoordinates_X = 0;
            int EndCoordinates_Y = 0;
            int ImageIndex = 0;
            numericUpDown_MotiomAnimation_StartCoordinates_X.Value = StartCoordinates_X;
            numericUpDown_MotiomAnimation_StartCoordinates_Y.Value = StartCoordinates_Y;
            numericUpDown_MotiomAnimation_EndCoordinates_X.Value = EndCoordinates_X;
            numericUpDown_MotiomAnimation_EndCoordinates_Y.Value = EndCoordinates_Y;
            comboBox_MotiomAnimation_Image.Text = "";

            if (dataGridView_MotiomAnimation.SelectedCells.Count > 0)
            {
                int RowIndex = dataGridView_MotiomAnimation.SelectedCells[0].RowIndex;
                if (!dataGridView_MotiomAnimation.Rows[RowIndex].IsNewRow)
                {
                    DataGridViewRow row = dataGridView_MotiomAnimation.Rows[RowIndex];
                    if (row.Cells[1].Value != null) Int32.TryParse(row.Cells[1].Value.ToString(), out StartCoordinates_X);
                    if (row.Cells[2].Value != null) Int32.TryParse(row.Cells[2].Value.ToString(), out StartCoordinates_Y);
                    if (row.Cells[3].Value != null) Int32.TryParse(row.Cells[3].Value.ToString(), out EndCoordinates_X);
                    if (row.Cells[4].Value != null) Int32.TryParse(row.Cells[4].Value.ToString(), out EndCoordinates_Y);

                    numericUpDown_MotiomAnimation_StartCoordinates_X.Value = StartCoordinates_X;
                    numericUpDown_MotiomAnimation_StartCoordinates_Y.Value = StartCoordinates_Y;
                    numericUpDown_MotiomAnimation_EndCoordinates_X.Value = EndCoordinates_X;
                    numericUpDown_MotiomAnimation_EndCoordinates_Y.Value = EndCoordinates_Y;

                    if (row.Cells[5].Value != null && Int32.TryParse(row.Cells[5].Value.ToString(), out ImageIndex))
                    {
                        comboBoxSetText(comboBox_MotiomAnimation_Image, ImageIndex);
                    }
                    else
                    {
                        comboBox_MotiomAnimation_Image.Text = "";
                    }
                }
            }
            MotiomAnimation_Update = false;
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
            PreviewToBitmap(gPanel, 1.0f, false, false, false, false, false, false, false, false, false, false, 1);

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

        private void dataGridView_MotiomAnimation_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if(dataGridView_MotiomAnimation.Rows.Count > 5)
            {
                MessageBox.Show(Properties.FormStrings.Message_WarningAnimationCoun_Text,
                    Properties.FormStrings.Message_Warning_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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

        #region Shortcuts_Width_Height
        private void numericUpDown_Shortcuts_Steps_Width_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MouseСoordinates.X < 0) return;
            int value = MouseСoordinates.X - (int)numericUpDown_Shortcuts_Steps_X.Value;
            NumericUpDown numericUpDown = sender as NumericUpDown;
            if ((e.X <= numericUpDown.Controls[1].Width + 1) && (value > 0))
            {
                // Click is in text area
                numericUpDown.Value = value;
            }
        }

        private void numericUpDown_Shortcuts_Steps_Height_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MouseСoordinates.Y < 0) return;
            int value = MouseСoordinates.Y - (int)numericUpDown_Shortcuts_Steps_Y.Value;
            NumericUpDown numericUpDown = sender as NumericUpDown;
            if ((e.X <= numericUpDown.Controls[1].Width + 1) && (value > 0))
            {
                // Click is in text area
                numericUpDown.Value = value;
            }
        }

        private void numericUpDown_Shortcuts_Puls_Width_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MouseСoordinates.X < 0) return;
            int value = MouseСoordinates.X - (int)numericUpDown_Shortcuts_Puls_X.Value;
            NumericUpDown numericUpDown = sender as NumericUpDown;
            if ((e.X <= numericUpDown.Controls[1].Width + 1) && (value > 0))
            {
                // Click is in text area
                numericUpDown.Value = value;
            }
        }

        private void numericUpDown_Shortcuts_Puls_Height_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MouseСoordinates.Y < 0) return;
            int value = MouseСoordinates.Y - (int)numericUpDown_Shortcuts_Puls_Y.Value;
            NumericUpDown numericUpDown = sender as NumericUpDown;
            if ((e.X <= numericUpDown.Controls[1].Width + 1) && (value > 0))
            {
                // Click is in text area
                numericUpDown.Value = value;
            }
        }

        private void numericUpDown_Shortcuts_Weather_Width_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MouseСoordinates.X < 0) return;
            int value = MouseСoordinates.X - (int)numericUpDown_Shortcuts_Weather_X.Value;
            NumericUpDown numericUpDown = sender as NumericUpDown;
            if ((e.X <= numericUpDown.Controls[1].Width + 1) && (value > 0))
            {
                // Click is in text area
                numericUpDown.Value = value;
            }
        }

        private void numericUpDown_Shortcuts_Weather_Height_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MouseСoordinates.Y < 0) return;
            int value = MouseСoordinates.Y - (int)numericUpDown_Shortcuts_Weather_Y.Value;
            NumericUpDown numericUpDown = sender as NumericUpDown;
            if ((e.X <= numericUpDown.Controls[1].Width + 1) && (value > 0))
            {
                // Click is in text area
                numericUpDown.Value = value;
            }
        }

        private void numericUpDown_Shortcuts_Energy_Width_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MouseСoordinates.X < 0) return;
            int value = MouseСoordinates.X - (int)numericUpDown_Shortcuts_Energy_X.Value;
            NumericUpDown numericUpDown = sender as NumericUpDown;
            if ((e.X <= numericUpDown.Controls[1].Width + 1) && (value > 0))
            {
                // Click is in text area
                numericUpDown.Value = value;
            }
        }

        private void numericUpDown_Shortcuts_Energy_Height_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MouseСoordinates.Y < 0) return;
            int value = MouseСoordinates.Y - (int)numericUpDown_Shortcuts_Energy_Y.Value;
            NumericUpDown numericUpDown = sender as NumericUpDown;
            if ((e.X <= numericUpDown.Controls[1].Width + 1) && (value > 0))
            {
                // Click is in text area
                numericUpDown.Value = value;
            }
        }
        #endregion

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
            //    if (Watch_Face.StepsProgress.Circle != null)
            //    {
            //        Watch_Face.StepsProgress.Circle.CenterX = (int)Math.Round(Watch_Face.StepsProgress.Circle.CenterX * scale);
            //        Watch_Face.StepsProgress.Circle.CenterY = (int)Math.Round(Watch_Face.StepsProgress.Circle.CenterY * scale);
            //        Watch_Face.StepsProgress.Circle.RadiusX = (int)Math.Round(Watch_Face.StepsProgress.Circle.RadiusX * scale);
            //        Watch_Face.StepsProgress.Circle.RadiusY = (int)Math.Round(Watch_Face.StepsProgress.Circle.RadiusY * scale);
            //        Watch_Face.StepsProgress.Circle.Width = (int)Math.Round(Watch_Face.StepsProgress.Circle.Width * scale);
            //        if (Watch_Face.StepsProgress.Circle.ImageIndex != null)
            //        {
            //            int x = 0;
            //            int y = 0;
            //            Color new_color = ColorRead(Watch_Face.StepsProgress.Circle.Color);
            //            ColorToCoodinates(new_color, out x, out y);
            //            x = (int)Math.Round(x * scale);
            //            y = (int)Math.Round(y * scale);
            //            string colorStr = CoodinatesToColor(x, y);
            //            Watch_Face.StepsProgress.Circle.Color = colorStr;
            //        }
            //    }

            //    if ((Watch_Face.StepsProgress.ClockHand != null) && (Watch_Face.StepsProgress.ClockHand.Image != null))
            //    {
            //        Watch_Face.StepsProgress.ClockHand.Image.X = (int)Math.Round(Watch_Face.StepsProgress.ClockHand.Image.X * scale);
            //        Watch_Face.StepsProgress.ClockHand.Image.Y = (int)Math.Round(Watch_Face.StepsProgress.ClockHand.Image.Y * scale);
            //        if (Watch_Face.StepsProgress.ClockHand.CenterOffset != null)
            //        {
            //            Watch_Face.StepsProgress.ClockHand.CenterOffset.X =
            //                (int)Math.Round(Watch_Face.StepsProgress.ClockHand.CenterOffset.X * scale);
            //            Watch_Face.StepsProgress.ClockHand.CenterOffset.Y =
            //                (int)Math.Round(Watch_Face.StepsProgress.ClockHand.CenterOffset.Y * scale);
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

            //    if (Watch_Face.Activity.Pulse != null)
            //    {
            //        Watch_Face.Activity.Pulse.TopLeftX = (int)Math.Round(Watch_Face.Activity.Pulse.TopLeftX * scale);
            //        Watch_Face.Activity.Pulse.TopLeftY = (int)Math.Round(Watch_Face.Activity.Pulse.TopLeftY * scale);
            //        Watch_Face.Activity.Pulse.BottomRightX = (int)Math.Round(Watch_Face.Activity.Pulse.BottomRightX * scale);
            //        Watch_Face.Activity.Pulse.BottomRightY = (int)Math.Round(Watch_Face.Activity.Pulse.BottomRightY * scale);
            //        Watch_Face.Activity.Pulse.Spacing = (int)Math.Round(Watch_Face.Activity.Pulse.Spacing * scale);
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
            //        (Watch_Face.Activity.PulseGraph.ClockHand != null) &&
            //        (Watch_Face.Activity.PulseGraph.ClockHand.Image != null))
            //    {
            //        Watch_Face.Activity.PulseGraph.ClockHand.Image.X = (int)Math.Round(Watch_Face.Activity.PulseGraph.ClockHand.Image.X * scale);
            //        Watch_Face.Activity.PulseGraph.ClockHand.Image.Y = (int)Math.Round(Watch_Face.Activity.PulseGraph.ClockHand.Image.Y * scale);
            //        if (Watch_Face.Activity.PulseGraph.ClockHand.CenterOffset != null)
            //        {
            //            Watch_Face.Activity.PulseGraph.ClockHand.CenterOffset.X =
            //                (int)Math.Round(Watch_Face.Activity.PulseGraph.ClockHand.CenterOffset.X * scale);
            //            Watch_Face.Activity.PulseGraph.ClockHand.CenterOffset.Y =
            //                (int)Math.Round(Watch_Face.Activity.PulseGraph.ClockHand.CenterOffset.Y * scale);
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
                
            //    if (Watch_Face.Activity.CaloriesGraph != null && Watch_Face.Activity.CaloriesGraph.Circle != null)
            //    {
            //        Watch_Face.Activity.CaloriesGraph.Circle.CenterX = (int)Math.Round(Watch_Face.Activity.CaloriesGraph.Circle.CenterX * scale);
            //        Watch_Face.Activity.CaloriesGraph.Circle.CenterY = (int)Math.Round(Watch_Face.Activity.CaloriesGraph.Circle.CenterY * scale);
            //        Watch_Face.Activity.CaloriesGraph.Circle.RadiusX = (int)Math.Round(Watch_Face.Activity.CaloriesGraph.Circle.RadiusX * scale);
            //        Watch_Face.Activity.CaloriesGraph.Circle.RadiusY = (int)Math.Round(Watch_Face.Activity.CaloriesGraph.Circle.RadiusY * scale);
            //        Watch_Face.Activity.CaloriesGraph.Circle.Width = (int)Math.Round(Watch_Face.Activity.CaloriesGraph.Circle.Width * scale);
            //        if (Watch_Face.Activity.CaloriesGraph.Circle.ImageIndex != null)
            //        {
            //            int x = 0;
            //            int y = 0;
            //            Color new_color = ColorRead(Watch_Face.Activity.CaloriesGraph.Circle.Color);
            //            ColorToCoodinates(new_color, out x, out y);
            //            x = (int)Math.Round(x * scale);
            //            y = (int)Math.Round(y * scale);
            //            string colorStr = CoodinatesToColor(x, y);
            //            Watch_Face.Activity.CaloriesGraph.Circle.Color = colorStr;
            //        }
            //    }

            //    if ((Watch_Face.Activity.CaloriesGraph != null) &&
            //        (Watch_Face.Activity.CaloriesGraph.ClockHand != null) &&
            //        (Watch_Face.Activity.CaloriesGraph.ClockHand.Image != null))
            //    {
            //        Watch_Face.Activity.CaloriesGraph.ClockHand.Image.X = (int)Math.Round(Watch_Face.Activity.CaloriesGraph.ClockHand.Image.X * scale);
            //        Watch_Face.Activity.CaloriesGraph.ClockHand.Image.Y = (int)Math.Round(Watch_Face.Activity.CaloriesGraph.ClockHand.Image.Y * scale);
            //        if (Watch_Face.Activity.CaloriesGraph.ClockHand.CenterOffset != null)
            //        {
            //            Watch_Face.Activity.CaloriesGraph.ClockHand.CenterOffset.X =
            //                (int)Math.Round(Watch_Face.Activity.CaloriesGraph.ClockHand.CenterOffset.X * scale);
            //            Watch_Face.Activity.CaloriesGraph.ClockHand.CenterOffset.Y =
            //                (int)Math.Round(Watch_Face.Activity.CaloriesGraph.ClockHand.CenterOffset.Y * scale);
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

            //#region AnalogDialFace
            //if (Watch_Face.AnalogDialFace != null)
            //{
            //    if ((Watch_Face.AnalogDialFace.Hours != null) && (Watch_Face.AnalogDialFace.Hours.Image != null))
            //    {
            //        Watch_Face.AnalogDialFace.Hours.Image.X = (int)Math.Round(Watch_Face.AnalogDialFace.Hours.Image.X * scale);
            //        Watch_Face.AnalogDialFace.Hours.Image.Y = (int)Math.Round(Watch_Face.AnalogDialFace.Hours.Image.Y * scale);

            //        if (Watch_Face.AnalogDialFace.Hours.CenterOffset != null)
            //        {
            //            Watch_Face.AnalogDialFace.Hours.CenterOffset.X = 
            //                (int)Math.Round(Watch_Face.AnalogDialFace.Hours.CenterOffset.X * scale);
            //            Watch_Face.AnalogDialFace.Hours.CenterOffset.Y = 
            //                (int)Math.Round(Watch_Face.AnalogDialFace.Hours.CenterOffset.Y * scale);
            //        }
            //    }

            //    if ((Watch_Face.AnalogDialFace.Minutes != null) && (Watch_Face.AnalogDialFace.Minutes.Image != null))
            //    {
            //        Watch_Face.AnalogDialFace.Minutes.Image.X = (int)Math.Round(Watch_Face.AnalogDialFace.Minutes.Image.X * scale);
            //        Watch_Face.AnalogDialFace.Minutes.Image.Y = (int)Math.Round(Watch_Face.AnalogDialFace.Minutes.Image.Y * scale);

            //        if (Watch_Face.AnalogDialFace.Minutes.CenterOffset != null)
            //        {
            //            Watch_Face.AnalogDialFace.Minutes.CenterOffset.X =
            //                 (int)Math.Round(Watch_Face.AnalogDialFace.Minutes.CenterOffset.X * scale);
            //            Watch_Face.AnalogDialFace.Minutes.CenterOffset.Y =
            //                 (int)Math.Round(Watch_Face.AnalogDialFace.Minutes.CenterOffset.Y * scale);
            //        }
            //    }

            //    if ((Watch_Face.AnalogDialFace.Seconds != null) && (Watch_Face.AnalogDialFace.Seconds.Image != null))
            //    {
            //        Watch_Face.AnalogDialFace.Seconds.Image.X = (int)Math.Round(Watch_Face.AnalogDialFace.Seconds.Image.X * scale);
            //        Watch_Face.AnalogDialFace.Seconds.Image.Y = (int)Math.Round(Watch_Face.AnalogDialFace.Seconds.Image.Y * scale);

            //        if (Watch_Face.AnalogDialFace.Seconds.CenterOffset != null)
            //        {
            //            Watch_Face.AnalogDialFace.Seconds.CenterOffset.X =
            //                (int)Math.Round(Watch_Face.AnalogDialFace.Seconds.CenterOffset.X * scale);
            //            Watch_Face.AnalogDialFace.Seconds.CenterOffset.Y =
            //                (int)Math.Round(Watch_Face.AnalogDialFace.Seconds.CenterOffset.Y * scale);
            //        }
            //    }

            //    if (Watch_Face.AnalogDialFace.HourCenterImage != null)
            //    {
            //        Watch_Face.AnalogDialFace.HourCenterImage.X = (int)Math.Round(Watch_Face.AnalogDialFace.HourCenterImage.X * scale);
            //        Watch_Face.AnalogDialFace.HourCenterImage.Y = (int)Math.Round(Watch_Face.AnalogDialFace.HourCenterImage.Y * scale);
            //    }

            //    if (Watch_Face.AnalogDialFace.MinCenterImage != null)
            //    {
            //        Watch_Face.AnalogDialFace.MinCenterImage.X = (int)Math.Round(Watch_Face.AnalogDialFace.MinCenterImage.X * scale);
            //        Watch_Face.AnalogDialFace.MinCenterImage.Y = (int)Math.Round(Watch_Face.AnalogDialFace.MinCenterImage.Y * scale);
            //    }

            //    if (Watch_Face.AnalogDialFace.SecCenterImage != null)
            //    {
            //        Watch_Face.AnalogDialFace.SecCenterImage.X = (int)Math.Round(Watch_Face.AnalogDialFace.SecCenterImage.X * scale);
            //        Watch_Face.AnalogDialFace.SecCenterImage.Y = (int)Math.Round(Watch_Face.AnalogDialFace.SecCenterImage.Y * scale);
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

            //    if (Watch_Face.Shortcuts.Pulse != null && Watch_Face.Shortcuts.Pulse.Element != null)
            //    {
            //        Watch_Face.Shortcuts.Pulse.Element.TopLeftX = (int)Math.Round(Watch_Face.Shortcuts.Pulse.Element.TopLeftX * scale);
            //        Watch_Face.Shortcuts.Pulse.Element.TopLeftY = (int)Math.Round(Watch_Face.Shortcuts.Pulse.Element.TopLeftY * scale);
            //        Watch_Face.Shortcuts.Pulse.Element.Width = (int)Math.Round(Watch_Face.Shortcuts.Pulse.Element.Width * scale);
            //        Watch_Face.Shortcuts.Pulse.Element.Height = (int)Math.Round(Watch_Face.Shortcuts.Pulse.Element.Height * scale);
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

#region Radius
        private void numericUpDown_StepsProgress_Radius_X_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MouseСoordinates.X < 0) return;
            int valueX = MouseСoordinates.X - (int)numericUpDown_StepsProgress_Center_X.Value;
            int valueY = MouseСoordinates.Y - (int)numericUpDown_StepsProgress_Center_Y.Value;
            int value = (int)Math.Round(Math.Sqrt(valueX * valueX + valueY * valueY));
            NumericUpDown numericUpDown = sender as NumericUpDown;
            if (e.X <= numericUpDown.Controls[1].Width + 1)
            {
                // Click is in text area
                numericUpDown.Value = value;
            }
        }

        private void numericUpDown_ActivityPulsScale_Radius_X_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MouseСoordinates.X < 0) return;
            int valueX = MouseСoordinates.X - (int)numericUpDown_ActivityPulsScale_Center_X.Value;
            int valueY = MouseСoordinates.Y - (int)numericUpDown_ActivityPulsScale_Center_Y.Value;
            int value = (int)Math.Round(Math.Sqrt(valueX * valueX + valueY * valueY));
            NumericUpDown numericUpDown = sender as NumericUpDown;
            if (e.X <= numericUpDown.Controls[1].Width + 1)
            {
                // Click is in text area
                numericUpDown.Value = value;
            }
        }

        private void numericUpDown_ActivityCaloriesScale_Radius_X_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MouseСoordinates.X < 0) return;
            int valueX = MouseСoordinates.X - (int)numericUpDown_ActivityCaloriesScale_Center_X.Value;
            int valueY = MouseСoordinates.Y - (int)numericUpDown_ActivityCaloriesScale_Center_Y.Value;
            int value = (int)Math.Round(Math.Sqrt(valueX * valueX + valueY * valueY));
            NumericUpDown numericUpDown = sender as NumericUpDown;
            if (e.X <= numericUpDown.Controls[1].Width + 1)
            {
                // Click is in text area
                numericUpDown.Value = value;
            }
        }

        private void numericUpDown_Battery_Scale_Radius_X_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MouseСoordinates.X < 0) return;
            int valueX = MouseСoordinates.X - (int)numericUpDown_Battery_Scale_Center_X.Value;
            int valueY = MouseСoordinates.Y - (int)numericUpDown_Battery_Scale_Center_Y.Value;
            int value = (int)Math.Round(Math.Sqrt(valueX * valueX + valueY * valueY));
            NumericUpDown numericUpDown = sender as NumericUpDown;
            if (e.X <= numericUpDown.Controls[1].Width + 1)
            {
                // Click is in text area
                numericUpDown.Value = value;
            }
        }

#endregion
        
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
                PreviewToBitmap(gPanel, 1.0f, false, false, false, false, false, false, false, true, false, false, 0);
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
                string s = label_size.Text;
                //s = s.Trim(new char[] { '≈', 'M' });
                s = s.Replace("≈", "");
                s = s.Replace("MB", "");
                float pixels = float.Parse(s) * 1024 * 1024;
                int pixelsNew = bitmap.Width * bitmap.Height;
                pixelsNew = pixelsNew * 4 + 20;
                pixels = pixels - pixelsOld + pixelsNew;
                ShowAllFileSize(pixels);
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
                PreviewToBitmap(gPanel, 1.0f, false, false, false, false, false, false, false, true, false, false, 0);
                if (checkBox_crop.Checked) bitmap = ApplyMask(bitmap, mask);
                float scale = (float)PreviewHeight / bitmap.Height;
                bitmap = ResizeImage(bitmap, scale);
                //bitmap.Save(ListImagesFullName[i], ImageFormat.Png);

                string s = label_size.Text;
                //s = s.Trim(new char[] { '≈', 'M' });
                s = s.Replace("≈", "");
                s = s.Replace("MB", "");
                float pixels = float.Parse(s) * 1024 * 1024;
                int pixelsNew = bitmap.Width * bitmap.Height;
                pixelsNew = pixelsNew * 4 + 20;
                pixels = pixels + pixelsNew;
                ShowAllFileSize(pixels);
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

        private void comboBox_Background_color_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Color = comboBox_Background_color.BackColor;
            colorDialog.FullOpen = true;
            if (colorDialog.ShowDialog() == DialogResult.Cancel)
                return;
            // установка цвета формы
            comboBox_Background_color.BackColor = colorDialog.Color;
            JSON_write();
            PreviewImage();
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
