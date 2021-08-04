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
using System.Drawing.Text;
using System.Runtime.InteropServices;

namespace AmazFit_Watchface_2
{
    public partial class Form1 : Form
    {
        WATCH_FACE_JSON Watch_Face;
        WATCH_FACE_PREWIEV_TwoDigits Watch_Face_Preview_TwoDigits;
        WATCH_FACE_PREWIEV_SET Watch_Face_Preview_Set;
        List<string> ListImages = new List<string>(); // перечень имен файлов с картинками
        public static List<string> ListImagesFullName = new List<string>(); // перечень путей к файлам с картинками
        public bool PreviewView; // включает прорисовку предпросмотра
        bool Settings_Load; // включать при обновлении настроек для выключения перерисовки
        bool JSON_Modified = false; // JSON файл был изменен
        string FileName; // Запоминает имя для диалогов
        string FullFileDir; // Запоминает папку для диалогов
        string StartFileNameJson; // имя файла из параметров запуска
        string StartFileNameBin; // имя файла из параметров запуска
        float currentDPI; // масштаб экрана
        Form_Preview formPreview;
        public static PROGRAM_SETTINGS Program_Settings;

        Widgets WidgetsTemp; // временная переменная для хранения виджетов

        int offSet_X = 227;
        int offSet_Y = 227;
        string oldTabName = "";


        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);

        private PrivateFontCollection fonts = new PrivateFontCollection();


        public Form1(string[] args)
        {
            if (File.Exists(Application.StartupPath + "\\Program log.txt")) File.Delete(Application.StartupPath + @"\Program log.txt");
            Logger.WriteLine("* Form1");

            SplashScreenStart();

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
                    if (language == "fr")
                    {
                        Program_Settings.language = "French";
                    }
                    if (language == "it")
                    {
                        Program_Settings.language = "Italiano";
                    }
                    if (language == "zh")
                    {
                        Program_Settings.language = "Chinese/简体中文";
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
            //tabControl1.TabPages["tabPageConverting"].Parent = null;

            userControl_SystemFont_Group_Year.userControl_SystemFont.checkBox_addZero.Text =
                checkBox_Year_add_zero.Text;
            userControl_SystemFont_Group_Year.userControl_FontRotate.checkBox_addZero.Text =
                checkBox_Year_add_zero.Text;

            userControl_SystemFont_Group_Year_AOD.userControl_SystemFont.checkBox_addZero.Text =
                checkBox_Year_add_zero_AOD.Text;
            userControl_SystemFont_Group_Year_AOD.userControl_FontRotate.checkBox_addZero.Text =
                checkBox_Year_add_zero_AOD.Text;

            AddDataDataGridView();
#if DEBUG
            tabControl1.SelectTab("tabPageConverting");
            tabControl_EditParameters.SelectTab(4);
#endif

#if !DEBUG

            //tabControl_SystemWeather.TabPages[4].Parent = null;
            tabControl_SystemActivity.TabPages["tabPage_Stress"].Parent = null;
            //tabControl_SystemActivity.TabPages["tabPage_ActivityGoal"].Parent = null;
            tabControl_SystemWeather.TabPages["tabPage_AirQuality"].Parent = null;
            //tabControl_SystemWeather.TabPages["tabPage_Sunrise"].Parent = null;
            tabControl_SystemWeather.TabPages["tabPage_Altitude"].Parent = null;


            //tabControl_SystemWeather_AOD.TabPages[4].Parent = null;
            tabControl_SystemActivity_AOD.TabPages["tabPage_Stress_AOD"].Parent = null;
            //tabControl_SystemActivity_AOD.TabPages["tabPage_ActivityGoal_AOD"].Parent = null;
            tabControl_SystemWeather_AOD.TabPages["tabPage_AirQuality_AOD"].Parent = null;
            //tabControl_SystemWeather_AOD.TabPages["tabPage_Sunrise_AOD"].Parent = null;
            tabControl_SystemWeather_AOD.TabPages["tabPage_Altitude_AOD"].Parent = null;

#endif

            splitContainer_EditParameters.Panel1Collapsed = false;
            splitContainer_EditParameters.Panel2Collapsed = true;

            #region sistem font
            byte[] fontData = Properties.Resources.OpenSans_Regular;
            IntPtr fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
            System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, Properties.Resources.OpenSans_Regular.Length);
            AddFontMemResourceEx(fontPtr, (uint)Properties.Resources.OpenSans_Regular.Length, IntPtr.Zero, ref dummy);
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontPtr);
            #endregion
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

        private void AddDataDataGridView()
        {
            for (int i = 1; i < 19; i++)
            {
                switch (i)
                {
                    case 1:
                        dataGridView_SNL_Activity.Rows.Add("Battery", Properties.FormStrings.ActivityName_Battery);
                        dataGridView_SNL_Activity.Rows[dataGridView_SNL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 2:
                        dataGridView_SNL_Activity.Rows.Add("Steps", Properties.FormStrings.ActivityName_Steps);
                        dataGridView_SNL_Activity.Rows[dataGridView_SNL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 3:
                        dataGridView_SNL_Activity.Rows.Add("Calories", Properties.FormStrings.ActivityName_Calories);
                        dataGridView_SNL_Activity.Rows[dataGridView_SNL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 4:
                        dataGridView_SNL_Activity.Rows.Add("HeartRate", Properties.FormStrings.ActivityName_HeartRate);
                        dataGridView_SNL_Activity.Rows[dataGridView_SNL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 5:
                        dataGridView_SNL_Activity.Rows.Add("PAI", Properties.FormStrings.ActivityName_PAI);
                        dataGridView_SNL_Activity.Rows[dataGridView_SNL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 6:
                        dataGridView_SNL_Activity.Rows.Add("Distance", Properties.FormStrings.ActivityName_Distance);
                        dataGridView_SNL_Activity.Rows[dataGridView_SNL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 7:
                        dataGridView_SNL_Activity.Rows.Add("StandUp", Properties.FormStrings.ActivityName_StandUp);
                        dataGridView_SNL_Activity.Rows[dataGridView_SNL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 8:
                        dataGridView_SNL_Activity.Rows.Add("Weather", Properties.FormStrings.ActivityName_Weather);
                        dataGridView_SNL_Activity.Rows[dataGridView_SNL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 9:
                        dataGridView_SNL_Activity.Rows.Add("UVindex", Properties.FormStrings.ActivityName_UVindex);
                        dataGridView_SNL_Activity.Rows[dataGridView_SNL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 10:
                        dataGridView_SNL_Activity.Rows.Add("AirQuality", Properties.FormStrings.ActivityName_AirQuality);
                        dataGridView_SNL_Activity.Rows[dataGridView_SNL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 11:
                        dataGridView_SNL_Activity.Rows.Add("Humidity", Properties.FormStrings.ActivityName_Humidity);
                        dataGridView_SNL_Activity.Rows[dataGridView_SNL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 12:
                        dataGridView_SNL_Activity.Rows.Add("Sunrise", Properties.FormStrings.ActivityName_Sunrise);
                        dataGridView_SNL_Activity.Rows[dataGridView_SNL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 13:
                        dataGridView_SNL_Activity.Rows.Add("WindForce", Properties.FormStrings.ActivityName_WindForce);
                        dataGridView_SNL_Activity.Rows[dataGridView_SNL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 14:
                        dataGridView_SNL_Activity.Rows.Add("Altitude", Properties.FormStrings.ActivityName_Altitude);
                        dataGridView_SNL_Activity.Rows[dataGridView_SNL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 15:
                        dataGridView_SNL_Activity.Rows.Add("AirPressure", Properties.FormStrings.ActivityName_AirPressure);
                        dataGridView_SNL_Activity.Rows[dataGridView_SNL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 16:
                        dataGridView_SNL_Activity.Rows.Add("Stress", Properties.FormStrings.ActivityName_Stress);
                        dataGridView_SNL_Activity.Rows[dataGridView_SNL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 17:
                        dataGridView_SNL_Activity.Rows.Add("ActivityGoal", Properties.FormStrings.ActivityName_ActivityGoal);
                        dataGridView_SNL_Activity.Rows[dataGridView_SNL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 18:
                        dataGridView_SNL_Activity.Rows.Add("FatBurning", Properties.FormStrings.ActivityName_FatBurning);
                        dataGridView_SNL_Activity.Rows[dataGridView_SNL_Activity.RowCount - 1].Visible = false;
                        break;
                }
            }
            for (int i = 1; i < 4; i++)
            {
                switch (i)
                {
                    case 1:
                        dataGridView_SNL_Date.Rows.Add("Day", Properties.FormStrings.DateName_Day);
                        dataGridView_SNL_Date.Rows[dataGridView_SNL_Date.RowCount - 1].Visible = false;
                        break;
                    case 2:
                        dataGridView_SNL_Date.Rows.Add("Month", Properties.FormStrings.DateName_Month);
                        dataGridView_SNL_Date.Rows[dataGridView_SNL_Date.RowCount - 1].Visible = false;
                        break;
                    case 3:
                        dataGridView_SNL_Date.Rows.Add("Year", Properties.FormStrings.DateName_Year);
                        dataGridView_SNL_Date.Rows[dataGridView_SNL_Date.RowCount - 1].Visible = false;
                        break;
                }
            }

            for (int i = 1; i < 19; i++)
            {
                switch (i)
                {
                    case 1:
                        dataGridView_AODL_Activity.Rows.Add("Battery", Properties.FormStrings.ActivityName_Battery);
                        dataGridView_AODL_Activity.Rows[dataGridView_AODL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 2:
                        dataGridView_AODL_Activity.Rows.Add("Steps", Properties.FormStrings.ActivityName_Steps);
                        dataGridView_AODL_Activity.Rows[dataGridView_AODL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 3:
                        dataGridView_AODL_Activity.Rows.Add("Calories", Properties.FormStrings.ActivityName_Calories);
                        dataGridView_AODL_Activity.Rows[dataGridView_AODL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 4:
                        dataGridView_AODL_Activity.Rows.Add("HeartRate", Properties.FormStrings.ActivityName_HeartRate);
                        dataGridView_AODL_Activity.Rows[dataGridView_AODL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 5:
                        dataGridView_AODL_Activity.Rows.Add("PAI", Properties.FormStrings.ActivityName_PAI);
                        dataGridView_AODL_Activity.Rows[dataGridView_AODL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 6:
                        dataGridView_AODL_Activity.Rows.Add("Distance", Properties.FormStrings.ActivityName_Distance);
                        dataGridView_AODL_Activity.Rows[dataGridView_AODL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 7:
                        dataGridView_AODL_Activity.Rows.Add("StandUp", Properties.FormStrings.ActivityName_StandUp);
                        dataGridView_AODL_Activity.Rows[dataGridView_AODL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 8:
                        dataGridView_AODL_Activity.Rows.Add("Weather", Properties.FormStrings.ActivityName_Weather);
                        dataGridView_AODL_Activity.Rows[dataGridView_AODL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 9:
                        dataGridView_AODL_Activity.Rows.Add("UVindex", Properties.FormStrings.ActivityName_UVindex);
                        dataGridView_AODL_Activity.Rows[dataGridView_AODL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 10:
                        dataGridView_AODL_Activity.Rows.Add("AirQuality", Properties.FormStrings.ActivityName_AirQuality);
                        dataGridView_AODL_Activity.Rows[dataGridView_AODL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 11:
                        dataGridView_AODL_Activity.Rows.Add("Humidity", Properties.FormStrings.ActivityName_Humidity);
                        dataGridView_AODL_Activity.Rows[dataGridView_AODL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 12:
                        dataGridView_AODL_Activity.Rows.Add("Sunrise", Properties.FormStrings.ActivityName_Sunrise);
                        dataGridView_AODL_Activity.Rows[dataGridView_AODL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 13:
                        dataGridView_AODL_Activity.Rows.Add("WindForce", Properties.FormStrings.ActivityName_WindForce);
                        dataGridView_AODL_Activity.Rows[dataGridView_AODL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 14:
                        dataGridView_AODL_Activity.Rows.Add("Altitude", Properties.FormStrings.ActivityName_Altitude);
                        dataGridView_AODL_Activity.Rows[dataGridView_AODL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 15:
                        dataGridView_AODL_Activity.Rows.Add("AirPressure", Properties.FormStrings.ActivityName_AirPressure);
                        dataGridView_AODL_Activity.Rows[dataGridView_AODL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 16:
                        dataGridView_AODL_Activity.Rows.Add("Stress", Properties.FormStrings.ActivityName_Stress);
                        dataGridView_AODL_Activity.Rows[dataGridView_AODL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 17:
                        dataGridView_AODL_Activity.Rows.Add("ActivityGoal", Properties.FormStrings.ActivityName_ActivityGoal);
                        dataGridView_AODL_Activity.Rows[dataGridView_AODL_Activity.RowCount - 1].Visible = false;
                        break;
                    case 18:
                        dataGridView_AODL_Activity.Rows.Add("FatBurning", Properties.FormStrings.ActivityName_FatBurning);
                        dataGridView_AODL_Activity.Rows[dataGridView_AODL_Activity.RowCount - 1].Visible = false;
                        break;
                }
            }
            for (int i = 1; i < 4; i++)
            {
                switch (i)
                {
                    case 1:
                        dataGridView_AODL_Date.Rows.Add("Day", Properties.FormStrings.DateName_Day);
                        dataGridView_AODL_Date.Rows[dataGridView_AODL_Date.RowCount - 1].Visible = false;
                        break;
                    case 2:
                        dataGridView_AODL_Date.Rows.Add("Month", Properties.FormStrings.DateName_Month);
                        dataGridView_AODL_Date.Rows[dataGridView_AODL_Date.RowCount - 1].Visible = false;
                        break;
                    case 3:
                        dataGridView_AODL_Date.Rows.Add("Year", Properties.FormStrings.DateName_Year);
                        dataGridView_AODL_Date.Rows[dataGridView_AODL_Date.RowCount - 1].Visible = false;
                        break;
                }
            }


            for (int i = 1; i < 4; i++)
            {
                switch (i)
                {
                    case 1:
                        dataGridView_Widget_DateAdd.Rows.Add("Day", Properties.FormStrings.DateName_Day);
                        //dataGridView_Widget_DateAdd.Rows[dataGridView_Widget_DateAdd.RowCount - 1].Visible = false;
                        break;
                    case 2:
                        dataGridView_Widget_DateAdd.Rows.Add("Month", Properties.FormStrings.DateName_Month);
                        //dataGridView_Widget_DateAdd.Rows[dataGridView_Widget_DateAdd.RowCount - 1].Visible = false;
                        break;
                    case 3:
                        dataGridView_Widget_DateAdd.Rows.Add("Year", Properties.FormStrings.DateName_Year);
                        //dataGridView_Widget_DateAdd.Rows[dataGridView_Widget_DateAdd.RowCount - 1].Visible = false;
                        break;
                }
            }

        }

        private void SplashScreenStart()
        {
            Logger.WriteLine("* SplashScreenStart");
            string splashScreenPath = Application.StartupPath + @"\Tools\SplashScreen.exe";
            if (File.Exists(splashScreenPath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = splashScreenPath;
                Process exeProcess = Process.Start(startInfo);

                exeProcess.Dispose();
                //exeProcess.CloseMainWindow();
                Logger.WriteLine("* SplashScreenStart (end)");
            }

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



        /// <summary>
        /// Find window by Caption only. Note you must pass IntPtr.Zero as the first parameter.
        /// </summary>
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        const UInt32 WM_CLOSE = 0x0010;
        private void Form1_Load(object sender, EventArgs e)
        {
            Logger.WriteLine("* Form1_Load");
            IntPtr windowPtr = FindWindowByCaption(IntPtr.Zero, "AmazFit WatchFace editor SplashScreen");
            if (windowPtr != IntPtr.Zero)
            {
                Logger.WriteLine("* SplashScreen_CLOSE");
                SendMessage(windowPtr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }


            //Logger.WriteLine("Form1_Load");

            string subPath = Application.StartupPath + @"\Tools\main.exe";
            //Logger.WriteLine("Set textBox.Text");
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
            Logger.WriteLine("Set Model_Watch");
            if (Program_Settings.Model_GTR2)
            {
                radioButton_GTR2.Checked = true;
                Program_Settings.unpack_command = Program_Settings.unpack_command_GTR_2;
                textBox_WatchSkin_Path.Text = Program_Settings.WatchSkin_GTR_2;
            }
            else if (Program_Settings.Model_GTS2)
            {
                radioButton_GTS2.Checked = true;
                Program_Settings.unpack_command = Program_Settings.unpack_command_GTS_2;
                textBox_WatchSkin_Path.Text = Program_Settings.WatchSkin_GTS_2;
            }
            else if (Program_Settings.Model_TRex_pro)
            {
                radioButton_TRex_pro.Checked = true;
                Program_Settings.unpack_command = Program_Settings.unpack_command_TRex_pro;
                textBox_WatchSkin_Path.Text = Program_Settings.WatchSkin_TRex_pro;
            }
            else if (Program_Settings.Model_Zepp_E)
            {
                radioButton_ZeppE.Checked = true;
                Program_Settings.unpack_command = Program_Settings.unpack_command_GTR_2;
                textBox_WatchSkin_Path.Text = Program_Settings.WatchSkin_Zepp_E;
            }
            checkBox_WatchSkin_Use.Checked = Program_Settings.WatchSkin_Use;

            Logger.WriteLine("Set checkBox");
            checkBox_border.Checked = Program_Settings.ShowBorder;
            checkBox_crop.Checked = Program_Settings.Crop;
            checkBox_Show_Shortcuts.Checked = Program_Settings.Show_Shortcuts;
            checkBox_CircleScaleImage.Checked = Program_Settings.Show_CircleScale_Area;
            checkBox_center_marker.Checked = Program_Settings.Shortcuts_Center_marker;
            checkBox_WidgetsArea.Checked = Program_Settings.Show_Widgets_Area;

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

            userControl_text_Distance.Collapsed = false;
            userControl_text_Distance_AOD.Collapsed = false;

            tabPage_Background.ImageIndex = 0;
            tabPage_Time.ImageIndex = 1;
            tabPage_Date.ImageIndex = 2;
            tabPage_Activity.ImageIndex = 3;
            tabPage_Air.ImageIndex = 4;
            tabPage_System.ImageIndex = 5;

            tabPage_Background_AOD.ImageIndex = 0;
            tabPage_Time_AOD.ImageIndex = 1;
            tabPage_Date_AOD.ImageIndex = 2;
            tabPage_Activity_AOD.ImageIndex = 3;
            tabPage_Air_AOD.ImageIndex = 4;
            tabPage_System_AOD.ImageIndex = 5;

            tabPage_WidgetsEdit.ImageIndex = 0;
            tabPage_WidgetAdd.ImageIndex = 1;



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

            checkBox_Shortcuts_Area.Checked = Program_Settings.Shortcuts_Area;
            checkBox_Shortcuts_Border.Checked = Program_Settings.Shortcuts_Border;

            checkBox_ShowIn12hourFormat.Checked = Program_Settings.ShowIn12hourFormat;
            checkBox_SaveID.Checked = Program_Settings.SaveID;

            if (Program_Settings.language.Length>1) comboBox_Language.Text = Program_Settings.language;

            if (Program_Settings.Splitter_Pos > 0 ) 
                splitContainer1.SplitterDistance = Program_Settings.Splitter_Pos;

            Settings_Load = false;

            if (Program_Settings.SaveID) checkBox_UseID.Checked = true;

            StartJsonPreview();
            SetPreferences(userControl_Set1);
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
            //HideWidgetEditElement();
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

                        if (!File.Exists(Program_Settings.pack_unpack_dir))
                        {
                            MessageBox.Show(Properties.FormStrings.Message_error_pack_unpack_dir_Text1 +
                                Program_Settings.pack_unpack_dir + Properties.FormStrings.Message_error_pack_unpack_dir_Text2 +
                                Environment.NewLine + Environment.NewLine + Properties.FormStrings.Message_error_pack_unpack_dir_Text3,
                                Properties.FormStrings.Message_error_pack_unpack_dir_Caption,
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }

                        Logger.WriteLine("UnpackBin");
                        startInfo.FileName = Program_Settings.pack_unpack_dir;
                        startInfo.Arguments = Program_Settings.unpack_command + " \"" + newFullName_bin + "\"";
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

            if (!File.Exists(Program_Settings.pack_unpack_dir))
            {
                MessageBox.Show(Properties.FormStrings.Message_error_pack_unpack_dir_Text1 +
                    Program_Settings.pack_unpack_dir + Properties.FormStrings.Message_error_pack_unpack_dir_Text2 +
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
                    startInfo.FileName = Program_Settings.pack_unpack_dir;
                    startInfo.Arguments = Program_Settings.unpack_command + " \"" + fullPath + "\"";
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

            if (!File.Exists(Program_Settings.pack_unpack_dir))
            {
                MessageBox.Show(Properties.FormStrings.Message_error_pack_unpack_dir_Text1 +
                    Program_Settings.pack_unpack_dir + Properties.FormStrings.Message_error_pack_unpack_dir_Text2 +
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
                    startInfo.FileName = Program_Settings.pack_unpack_dir;
                    startInfo.Arguments = Program_Settings.unpack_command + " \"" + fullfilename + "\"";
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


            if (!File.Exists(Program_Settings.pack_unpack_dir))
            {
                MessageBox.Show(Properties.FormStrings.Message_error_pack_unpack_dir_Text1 +
                    Program_Settings.pack_unpack_dir + Properties.FormStrings.Message_error_pack_unpack_dir_Text2 +
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
                    startInfo.FileName = Program_Settings.pack_unpack_dir;
                    startInfo.Arguments = Program_Settings.unpack_command + " \"" + fullfilename + "\"";
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

                        startInfo.FileName = Application.StartupPath + @"\Tools\GTR2_Packer.exe"; 
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

                            int ID = 0;
                            try
                            {
                                string text = File.ReadAllText(fullfilename);
                                WATCH_FACE_JSON Watch_Face_temp = JsonConvert.DeserializeObject<WATCH_FACE_JSON>(text, new JsonSerializerSettings
                                {
                                    DefaultValueHandling = DefaultValueHandling.Ignore,
                                    NullValueHandling = NullValueHandling.Ignore
                                });
                                if (Watch_Face_temp != null && Watch_Face_temp.Info != null &&
                                    Watch_Face_temp.Info.WatchFaceId != null) ID = (int)Watch_Face_temp.Info.WatchFaceId;
                                using (FileStream fileStream = File.OpenWrite(newFullName_bin))
                                {
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
                            catch {}

                            if (ID < 1000 && checkBox_UseID.Checked)
                            {
                                using (FileStream fileStream = File.OpenWrite(newFullName_bin))
                                {
                                    //int ID = 0;
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
            progressBar1.Visible = false;
            //Logger.WriteLine("Установили значения в соответствии с json файлом");
            string path = Path.GetDirectoryName(fullfilename);
            string newFullName = Path.Combine(path, "Watchface.ID");
            if (Watch_Face != null && Watch_Face.Info != null && Watch_Face.Info.WatchFaceId != null)
            {
                textBox_WatchfaceID.Text = Watch_Face.Info.WatchFaceId.ToString();
            }
            else
            {
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
            }

            newFullName = Path.Combine(path, "Preview.States");
            if (File.Exists(newFullName))
            {
                Logger.WriteLine("Load Preview.States");
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
            else StartJsonPreview();

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

        private void StartJsonPreview()
        {
            Random rnd = new Random();
            userControl_Set1.RandomValue(rnd);
            userControl_Set2.RandomValue(rnd);
            userControl_Set3.RandomValue(rnd);
            userControl_Set4.RandomValue(rnd);
            userControl_Set5.RandomValue(rnd);
            userControl_Set6.RandomValue(rnd);
            userControl_Set7.RandomValue(rnd);
            userControl_Set8.RandomValue(rnd);
            userControl_Set9.RandomValue(rnd);
            userControl_Set10.RandomValue(rnd);
            userControl_Set11.RandomValue(rnd);
            userControl_Set12.RandomValue(rnd);

            for(int i=1; i<13; i++)
            {
                DateTime date = DateTime.Now;
                int year;
                int month;
                int day;
                int weekDay;
                int offsetDay;

                switch (i)
                {
                    case 1:
                        date = userControl_Set1.dateTimePicker_Date_Set.Value;
                        break;
                    case 2:
                        date = userControl_Set2.dateTimePicker_Date_Set.Value;
                        break;
                    case 3:
                        date = userControl_Set3.dateTimePicker_Date_Set.Value;
                        break;
                    case 4:
                        date = userControl_Set5.dateTimePicker_Date_Set.Value;
                        break;
                    case 5:
                        date = userControl_Set5.dateTimePicker_Date_Set.Value;
                        break;
                    case 6:
                        date = userControl_Set6.dateTimePicker_Date_Set.Value;
                        break;
                    case 7:
                        date = userControl_Set7.dateTimePicker_Date_Set.Value;
                        break;
                    case 8:
                        date = userControl_Set8.dateTimePicker_Date_Set.Value;
                        break;
                    case 9:
                        date = userControl_Set9.dateTimePicker_Date_Set.Value;
                        break;
                    case 10:
                        date = userControl_Set10.dateTimePicker_Date_Set.Value;
                        break;
                    case 11:
                        date = userControl_Set11.dateTimePicker_Date_Set.Value;
                        break;
                    case 12:
                        date = userControl_Set12.dateTimePicker_Date_Set.Value;
                        break;
                }


                year = date.Year;
                month = i;
                //int month = date.Month;
                day = date.Day;
                date = new DateTime(year, month, day);
                weekDay = (int)date.DayOfWeek;
                offsetDay = i - weekDay;
                day = day + offsetDay;
                while(day < 1)
                {
                    day = day + 7;
                }
                while (day > 28)
                {
                    day = day - 7;
                }
                date = new DateTime(year, month, day);

                switch (i)
                {
                    case 1:
                        userControl_Set1.dateTimePicker_Date_Set.Value = date;
                        break;
                    case 2:
                        userControl_Set2.dateTimePicker_Date_Set.Value = date;
                        break;
                    case 3:
                        userControl_Set3.dateTimePicker_Date_Set.Value = date;
                        break;
                    case 4:
                        userControl_Set4.dateTimePicker_Date_Set.Value = date;
                        break;
                    case 5:
                        userControl_Set5.dateTimePicker_Date_Set.Value = date;
                        break;
                    case 6:
                        userControl_Set6.dateTimePicker_Date_Set.Value = date;
                        break;
                    case 7:
                        userControl_Set7.dateTimePicker_Date_Set.Value = date;
                        break;
                    case 8:
                        userControl_Set8.dateTimePicker_Date_Set.Value = date;
                        break;
                    case 9:
                        userControl_Set9.dateTimePicker_Date_Set.Value = date;
                        break;
                    case 10:
                        userControl_Set10.dateTimePicker_Date_Set.Value = date;
                        break;
                    case 11:
                        userControl_Set11.dateTimePicker_Date_Set.Value = date;
                        break;
                    case 12:
                        userControl_Set12.dateTimePicker_Date_Set.Value = date;
                        break;
                }
            }

            SetPreferences(userControl_Set12);
            if (!userControl_Set1.Collapsed) SetPreferences(userControl_Set1);
            if (!userControl_Set2.Collapsed) SetPreferences(userControl_Set2);
            if (!userControl_Set3.Collapsed) SetPreferences(userControl_Set3);
            if (!userControl_Set4.Collapsed) SetPreferences(userControl_Set4);
            if (!userControl_Set5.Collapsed) SetPreferences(userControl_Set5);
            if (!userControl_Set6.Collapsed) SetPreferences(userControl_Set6);
            if (!userControl_Set7.Collapsed) SetPreferences(userControl_Set7);
            if (!userControl_Set8.Collapsed) SetPreferences(userControl_Set8);
            if (!userControl_Set9.Collapsed) SetPreferences(userControl_Set9);
            if (!userControl_Set10.Collapsed) SetPreferences(userControl_Set10);
            if (!userControl_Set11.Collapsed) SetPreferences(userControl_Set11);
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
            if (radioButton_TRex_pro.Checked)
            {
                bitmap = new Bitmap(Convert.ToInt32(360), Convert.ToInt32(360), PixelFormat.Format32bppArgb);
            }
            if (radioButton_ZeppE.Checked)
            {
                bitmap = new Bitmap(Convert.ToInt32(416), Convert.ToInt32(416), PixelFormat.Format32bppArgb);
            }
            Graphics gPanel = Graphics.FromImage(bitmap);
#endregion

            Logger.WriteLine("PreviewToBitmap");
            int link = radioButton_ScreenNormal.Checked ? 0 : 1;
            PreviewToBitmap(gPanel, scale, checkBox_crop.Checked, checkBox_WebW.Checked, checkBox_WebB.Checked, 
                checkBox_border.Checked, checkBox_Show_Shortcuts.Checked, checkBox_Shortcuts_Area.Checked, 
                checkBox_Shortcuts_Border.Checked, true, checkBox_CircleScaleImage.Checked, 
                checkBox_center_marker.Checked, checkBox_WidgetsArea.Checked, link);
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
        
        public void SetPreferences(UserControl_Set userControl_Set)
        {
            Dictionary<string, int> Activity = new Dictionary<string, int>();
            Dictionary<string, int> Air = new Dictionary<string, int>();
            Dictionary<string, bool> checkValue = new Dictionary<string, bool>();
            userControl_Set.GetValue(out Activity, out Air, out checkValue);

            int Year = Activity["Year"];
            int Month = Activity["Month"];
            int Day = Activity["Day"];
            int WeekDay = Activity["WeekDay"];

            int Hour = Activity["Hour"];
            int Minute = Activity["Minute"];
            int Second = Activity["Second"];

            int Battery = Activity["Battery"];
            int Calories = Activity["Calories"];
            int HeartRate = Activity["HeartRate"];
            int Distance = Activity["Distance"];
            int Steps = Activity["Steps"];
            int StepsGoal = Activity["StepsGoal"];

            int PAI;
            Activity.TryGetValue("PAI", out PAI);
            int StandUp;
            Activity.TryGetValue("StandUp", out StandUp);
            int Stress;
            Activity.TryGetValue("Stress", out Stress);
            int ActivityGoal;
            Activity.TryGetValue("ActivityGoal", out ActivityGoal);
            int FatBurning;
            Activity.TryGetValue("FatBurning", out FatBurning);


            int Weather_Icon;
            Air.TryGetValue("Weather_Icon", out Weather_Icon);
            int Temperature;
            Air.TryGetValue("Temperature", out Temperature);
            int TemperatureMax;
            Air.TryGetValue("TemperatureMax", out TemperatureMax);
            int TemperatureMin;
            Air.TryGetValue("TemperatureMin", out TemperatureMin);

            int UVindex;
            Air.TryGetValue("UVindex", out UVindex);
            int AirQuality;
            Air.TryGetValue("AirQuality", out AirQuality);
            int Humidity;
            Air.TryGetValue("Humidity", out Humidity);
            int WindForce;
            Air.TryGetValue("WindForce", out WindForce);
            int Altitude;
            Air.TryGetValue("Altitude", out Altitude);
            int AirPressure;
            Air.TryGetValue("AirPressure", out AirPressure);


            bool Bluetooth;
            checkValue.TryGetValue("Bluetooth", out Bluetooth);
            bool Alarm;
            checkValue.TryGetValue("Alarm", out Alarm);
            bool Lock;
            checkValue.TryGetValue("Lock", out Lock);
            bool DND;
            checkValue.TryGetValue("DND", out DND);

            bool ShowTemperature;
            checkValue.TryGetValue("ShowTemperature", out ShowTemperature);

            Watch_Face_Preview_Set.Date.Year = Year;
            Watch_Face_Preview_Set.Date.Month = Month;
            Watch_Face_Preview_Set.Date.Day = Day;
            Watch_Face_Preview_Set.Date.WeekDay = WeekDay;
            if (Watch_Face_Preview_Set.Date.WeekDay == 0) Watch_Face_Preview_Set.Date.WeekDay = 7;

            Watch_Face_Preview_Set.Time.Hours = Hour;
            Watch_Face_Preview_Set.Time.Minutes = Minute;
            Watch_Face_Preview_Set.Time.Seconds = Second;

            Watch_Face_Preview_Set.Battery = Battery;
            Watch_Face_Preview_Set.Activity.Calories = Calories;
            Watch_Face_Preview_Set.Activity.HeartRate = HeartRate;
            Watch_Face_Preview_Set.Activity.Distance = Distance;
            Watch_Face_Preview_Set.Activity.Steps = Steps;
            Watch_Face_Preview_Set.Activity.StepsGoal = StepsGoal;
            Watch_Face_Preview_Set.Activity.PAI = PAI;
            Watch_Face_Preview_Set.Activity.StandUp = StandUp;
            Watch_Face_Preview_Set.Activity.Stress = Stress;
            //Watch_Face_Preview_Set.Activity.ActivityGoal = ActivityGoal;
            Watch_Face_Preview_Set.Activity.FatBurning = FatBurning;

            Watch_Face_Preview_Set.Status.Bluetooth = Bluetooth;
            Watch_Face_Preview_Set.Status.Alarm = Alarm;
            Watch_Face_Preview_Set.Status.Lock = Lock;
            Watch_Face_Preview_Set.Status.DoNotDisturb = DND;

            Watch_Face_Preview_Set.Weather.Temperature = Temperature;
            Watch_Face_Preview_Set.Weather.TemperatureMax = TemperatureMax;
            Watch_Face_Preview_Set.Weather.TemperatureMin = TemperatureMin;
            //Watch_Face_Preview_Set.Weather.TemperatureNoData = !checkBox_WeatherSet_Temp.Checked;
            //Watch_Face_Preview_Set.Weather.TemperatureMinMaxNoData = !checkBox_WeatherSet_MaxMinTemp.Checked;
            Watch_Face_Preview_Set.Weather.Icon = Weather_Icon;

            Watch_Face_Preview_Set.Weather.showTemperature = ShowTemperature;

            Watch_Face_Preview_Set.Weather.UVindex = UVindex;
            Watch_Face_Preview_Set.Weather.AirQuality = AirQuality;
            Watch_Face_Preview_Set.Weather.Humidity = Humidity;
            Watch_Face_Preview_Set.Weather.WindForce = WindForce;
            Watch_Face_Preview_Set.Weather.Altitude = Altitude;
            Watch_Face_Preview_Set.Weather.AirPressure = AirPressure;
            Watch_Face_Preview_Set.SetNumber = userControl_Set.SetNumber;

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
        private void groupBox_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            if(box.Enabled) DrawGroupBox(box, e.Graphics, Color.Black, Color.DarkGray);
            else DrawGroupBox(box, e.Graphics, Color.DarkGray, Color.DarkGray);
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



        #region поменялись предустановки


        private void userControl_Set_ValueChanged(object sender, EventArgs eventArgs, int setNumber)
        {
            switch (setNumber)
            {
                case 1:
                    SetPreferences(userControl_Set1);
                    break;
                case 2:
                    SetPreferences(userControl_Set2);
                    break;
                case 3:
                    SetPreferences(userControl_Set3);
                    break;
                case 4:
                    SetPreferences(userControl_Set4);
                    break;
                case 5:
                    SetPreferences(userControl_Set5);
                    break;
                case 6:
                    SetPreferences(userControl_Set6);
                    break;
                case 7:
                    SetPreferences(userControl_Set7);
                    break;
                case 8:
                    SetPreferences(userControl_Set8);
                    break;
                case 9:
                    SetPreferences(userControl_Set9);
                    break;
                case 10:
                    SetPreferences(userControl_Set10);
                    break;
                case 11:
                    SetPreferences(userControl_Set11);
                    break;
                case 12:
                    SetPreferences(userControl_Set12);
                    break;
            }

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
                    if (Form_Preview.Model_Wath.model_GTR2 != radioButton_GTR2.Checked)
                        Form_Preview.Model_Wath.model_GTR2 = radioButton_GTR2.Checked;
                    if (Form_Preview.Model_Wath.model_GTR2e != radioButton_GTR2e.Checked)
                        Form_Preview.Model_Wath.model_GTR2e = radioButton_GTR2e.Checked;
                    if (Form_Preview.Model_Wath.model_GTS2 != radioButton_GTS2.Checked)
                        Form_Preview.Model_Wath.model_GTS2 = radioButton_GTS2.Checked;
                    if (Form_Preview.Model_Wath.model_TRex_pro != radioButton_TRex_pro.Checked)
                        Form_Preview.Model_Wath.model_TRex_pro = radioButton_TRex_pro.Checked;
                    if (Form_Preview.Model_Wath.model_Zepp_E != radioButton_ZeppE.Checked)
                        Form_Preview.Model_Wath.model_Zepp_E = radioButton_ZeppE.Checked;
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
                    if (radioButton_TRex_pro.Checked)
                    {
                        bitmapPreviewResize = new Bitmap(Convert.ToInt32(360), Convert.ToInt32(360), PixelFormat.Format32bppArgb);
                    }
                    if (radioButton_ZeppE.Checked)
                    {
                        bitmapPreviewResize = new Bitmap(Convert.ToInt32(416), Convert.ToInt32(416), PixelFormat.Format32bppArgb);
                    }
                    Graphics gPanelPreviewResize = Graphics.FromImage(bitmapPreviewResize);
                    #endregion

                    int link_aod = radioButton_ScreenNormal.Checked ? 0 : 1;
                    PreviewToBitmap(gPanelPreviewResize, 1, checkBox_crop.Checked,
                        checkBox_WebW.Checked, checkBox_WebB.Checked, checkBox_border.Checked, 
                        checkBox_Show_Shortcuts.Checked, checkBox_Shortcuts_Area.Checked, checkBox_Shortcuts_Border.Checked, true,
                        checkBox_CircleScaleImage.Checked, checkBox_center_marker.Checked, checkBox_WidgetsArea.Checked, link_aod);
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

            if (Form_Preview.Model_Wath.model_GTR2 != radioButton_GTR2.Checked)
                Form_Preview.Model_Wath.model_GTR2 = radioButton_GTR2.Checked;
            if (Form_Preview.Model_Wath.model_GTR2e != radioButton_GTR2e.Checked)
                Form_Preview.Model_Wath.model_GTR2e = radioButton_GTR2e.Checked;
            if (Form_Preview.Model_Wath.model_GTS2 != radioButton_GTS2.Checked)
                Form_Preview.Model_Wath.model_GTS2 = radioButton_GTS2.Checked;
            if (Form_Preview.Model_Wath.model_TRex_pro != radioButton_TRex_pro.Checked)
                Form_Preview.Model_Wath.model_TRex_pro = radioButton_TRex_pro.Checked;
            if (Form_Preview.Model_Wath.model_Zepp_E != radioButton_ZeppE.Checked)
                Form_Preview.Model_Wath.model_Zepp_E = radioButton_ZeppE.Checked;
            formPreview.radioButton_CheckedChanged(sender, e);
            float scale = 1.0f;

#region BackgroundImage 
            Bitmap bitmap = new Bitmap(Convert.ToInt32(454), Convert.ToInt32(454), PixelFormat.Format32bppArgb);
            if (radioButton_GTS2.Checked)
            {
                bitmap = new Bitmap(Convert.ToInt32(348), Convert.ToInt32(442), PixelFormat.Format32bppArgb);
            }
            if (radioButton_TRex_pro.Checked)
            {
                bitmap = new Bitmap(Convert.ToInt32(360), Convert.ToInt32(360), PixelFormat.Format32bppArgb);
            }
            if (radioButton_ZeppE.Checked)
            {
                bitmap = new Bitmap(Convert.ToInt32(416), Convert.ToInt32(416), PixelFormat.Format32bppArgb);
            }
            Graphics gPanel = Graphics.FromImage(bitmap);
            #endregion

            int link = radioButton_ScreenNormal.Checked ? 0 : 1;
            PreviewToBitmap(gPanel, scale, checkBox_crop.Checked, checkBox_WebW.Checked, checkBox_WebB.Checked, 
                checkBox_border.Checked, checkBox_Show_Shortcuts.Checked, checkBox_Shortcuts_Area.Checked, 
                checkBox_Shortcuts_Border.Checked, true, checkBox_CircleScaleImage.Checked,
                checkBox_center_marker.Checked, checkBox_WidgetsArea.Checked, link);
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
            //openFileDialog.Filter = Properties.FormStrings.FilterJson;
            openFileDialog.FileName = "Preview.States";
            openFileDialog.Filter = "PreviewStates file | *.States|Json files (*.json) | *.json";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = false;
            openFileDialog.Title = Properties.FormStrings.Dialog_Title_PreviewStates;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fullfilename = openFileDialog.FileName;
                JsonPreview_Read(fullfilename);
                PreviewImage();
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
                if (count > 12) count = 12;
                for (int i = 0; i < count; i++)
                {
                    ps = JsonConvert.DeserializeObject<PREWIEV_STATES_Json>(objson[i].ToString(), new JsonSerializerSettings
                    {
                        //DefaultValueHandling = DefaultValueHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });

                    Dictionary<string, int> Activity = new Dictionary<string, int>();
                    Dictionary<string, int> Air = new Dictionary<string, int>();
                    Dictionary<string, bool> checkValue = new Dictionary<string, bool>();

                    Activity.Add("Year", ps.Time.Year);
                    Activity.Add("Month", ps.Time.Month);
                    Activity.Add("Day", ps.Time.Day);

                    Activity.Add("Hour", ps.Time.Hour);
                    Activity.Add("Minute", ps.Time.Minute);
                    Activity.Add("Second", ps.Time.Second);

                    Activity.Add("Battery", ps.BatteryLevel);
                    Activity.Add("Calories", ps.Calories);
                    Activity.Add("HeartRate", ps.Pulse);
                    Activity.Add("Distance", ps.Distance);
                    Activity.Add("Steps", ps.Steps);
                    Activity.Add("StepsGoal", ps.Goal);

                    Activity.Add("PAI", ps.PAI);
                    Activity.Add("StandUp", ps.Stand);
                    Activity.Add("Stress", ps.Stress);
                    //Activity.Add("ActivityGoal", ps.ActivityGoal);
                    Activity.Add("FatBurning", ps.FatBurning);


                    Air.Add("Weather_Icon", ps.CurrentWeather);
                    Air.Add("Temperature", ps.CurrentTemperature);
                    Air.Add("TemperatureMax", ps.TemperatureMax);
                    Air.Add("TemperatureMin", ps.TemperatureMin);

                    Air.Add("UVindex", ps.UVindex);
                    Air.Add("AirQuality", ps.AirQuality);
                    Air.Add("Humidity", ps.Humidity);
                    Air.Add("WindForce", ps.WindForce);
                    Air.Add("Altitude", ps.Altitude);
                    Air.Add("AirPressure", ps.AirPressure);


                    checkValue.Add("Bluetooth", ps.Bluetooth);
                    checkValue.Add("Alarm", ps.Alarm);
                    checkValue.Add("Lock", ps.Unlocked);
                    checkValue.Add("DND", ps.DoNotDisturb);

                    checkValue.Add("ShowTemperature", ps.ShowTemperature);

                    switch (i)
                    {
                        case 0:
                            userControl_Set1.SetValue(Activity, Air, checkValue);
                            break;
                        case 1:
                            userControl_Set2.SetValue(Activity, Air, checkValue);
                            break;
                        case 2:
                            userControl_Set3.SetValue(Activity, Air, checkValue);
                            break;
                        case 3:
                            userControl_Set4.SetValue(Activity, Air, checkValue);
                            break;
                        case 4:
                            userControl_Set5.SetValue(Activity, Air, checkValue);
                            break;
                        case 5:
                            userControl_Set6.SetValue(Activity, Air, checkValue);
                            break;
                        case 6:
                            userControl_Set7.SetValue(Activity, Air, checkValue);
                            break;
                        case 7:
                            userControl_Set8.SetValue(Activity, Air, checkValue);
                            break;
                        case 8:
                            userControl_Set9.SetValue(Activity, Air, checkValue);
                            break;
                        case 9:
                            userControl_Set10.SetValue(Activity, Air, checkValue);
                            break;
                        case 10:
                            userControl_Set11.SetValue(Activity, Air, checkValue);
                            break;
                        case 11:
                            userControl_Set12.SetValue(Activity, Air, checkValue);
                            break;
                    }
                }

                switch (count)
                {
                    case 1:
                        SetPreferences(userControl_Set1);
                        break;
                    case 2:
                        SetPreferences(userControl_Set2);
                        break;
                    case 3:
                        SetPreferences(userControl_Set3);
                        break;
                    case 4:
                        SetPreferences(userControl_Set4);
                        break;
                    case 5:
                        SetPreferences(userControl_Set5);
                        break;
                    case 6:
                        SetPreferences(userControl_Set6);
                        break;
                    case 7:
                        SetPreferences(userControl_Set7);
                        break;
                    case 8:
                        SetPreferences(userControl_Set8);
                        break;
                    case 9:
                        SetPreferences(userControl_Set9);
                        break;
                    case 10:
                        SetPreferences(userControl_Set10);
                        break;
                    case 11:
                        SetPreferences(userControl_Set11);
                        break;
                    case 12:
                        SetPreferences(userControl_Set12);
                        break;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(Properties.FormStrings.Message_JsonReadError_Text, Properties.FormStrings.Message_Error_Caption, 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            SetPreferences(userControl_Set12);
            if (!userControl_Set1.Collapsed) SetPreferences(userControl_Set1);
            if (!userControl_Set2.Collapsed) SetPreferences(userControl_Set2);
            if (!userControl_Set3.Collapsed) SetPreferences(userControl_Set3);
            if (!userControl_Set4.Collapsed) SetPreferences(userControl_Set4);
            if (!userControl_Set5.Collapsed) SetPreferences(userControl_Set5);
            if (!userControl_Set6.Collapsed) SetPreferences(userControl_Set6);
            if (!userControl_Set7.Collapsed) SetPreferences(userControl_Set7);
            if (!userControl_Set8.Collapsed) SetPreferences(userControl_Set8);
            if (!userControl_Set9.Collapsed) SetPreferences(userControl_Set9);
            if (!userControl_Set10.Collapsed) SetPreferences(userControl_Set10);
            if (!userControl_Set11.Collapsed) SetPreferences(userControl_Set11);

            PreviewView = true;
            //PreviewImage();
        }

        // записываем параметры в JsonPreview
        private void button_JsonPreview_Write_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //openFileDialog.InitialDirectory = subPath;
            //saveFileDialog.Filter = Properties.FormStrings.FilterJson;
            saveFileDialog.FileName = "Preview.States";
            saveFileDialog.Filter = "PreviewStates file | *.States";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Title = Properties.FormStrings.Dialog_Title_PreviewStates;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                object[] objson = new object[] { };
                int count = 0;
                for (int i = 0; i < 12; i++)
                {
                    PREWIEV_STATES_Json ps = new PREWIEV_STATES_Json();
                    ps.Time = new TimePreview();
                    Dictionary<string, int> Activity = new Dictionary<string, int>();
                    Dictionary<string, int> Air = new Dictionary<string, int>();
                    Dictionary<string, bool> checkValue = new Dictionary<string, bool>();
                    switch (i)
                    {
                        case 0:
                            userControl_Set1.GetValue(out Activity, out Air, out checkValue);
                            break;
                        case 1:
                            userControl_Set2.GetValue(out Activity, out Air, out checkValue);
                            break;
                        case 2:
                            userControl_Set3.GetValue(out Activity, out Air, out checkValue);
                            break;
                        case 3:
                            userControl_Set4.GetValue(out Activity, out Air, out checkValue);
                            break;
                        case 4:
                            userControl_Set5.GetValue(out Activity, out Air, out checkValue);
                            break;
                        case 5:
                            userControl_Set6.GetValue(out Activity, out Air, out checkValue);
                            break;
                        case 6:
                            userControl_Set7.GetValue(out Activity, out Air, out checkValue);
                            break;
                        case 7:
                            userControl_Set8.GetValue(out Activity, out Air, out checkValue);
                            break;
                        case 8:
                            userControl_Set9.GetValue(out Activity, out Air, out checkValue);
                            break;
                        case 9:
                            userControl_Set10.GetValue(out Activity, out Air, out checkValue);
                            break;
                        case 10:
                            userControl_Set11.GetValue(out Activity, out Air, out checkValue);
                            break;
                        case 11:
                            userControl_Set12.GetValue(out Activity, out Air, out checkValue);
                            break;
                    }

                    ps.Time.Year = Activity["Year"];
                    ps.Time.Month = Activity["Month"];
                    ps.Time.Day = Activity["Day"];

                    ps.Time.Hour = Activity["Hour"];
                    ps.Time.Minute = Activity["Minute"];
                    ps.Time.Second = Activity["Second"];

                    ps.BatteryLevel = Activity["Battery"];
                    ps.Calories = Activity["Calories"];
                    ps.Pulse = Activity["HeartRate"];
                    ps.Distance = Activity["Distance"];
                    ps.Steps = Activity["Steps"];
                    ps.Goal = Activity["StepsGoal"];

                    ps.PAI = Activity["PAI"];
                    ps.Stand = Activity["StandUp"];
                    ps.Stress = Activity["Stress"];
                    //ps.ActivityGoal = Activity["ActivityGoal"];
                    ps.FatBurning = Activity["FatBurning"];


                    ps.CurrentWeather = Air["Weather_Icon"];
                    ps.CurrentTemperature = Air["Temperature"];
                    ps.TemperatureMax = Air["TemperatureMax"];
                    ps.TemperatureMin = Air["TemperatureMin"];

                    ps.UVindex = Air["UVindex"];
                    ps.AirQuality = Air["AirQuality"];
                    ps.Humidity = Air["Humidity"];
                    ps.WindForce = Air["WindForce"];
                    ps.Altitude = Air["Altitude"];
                    ps.AirPressure = Air["AirPressure"];


                    ps.Bluetooth = checkValue["Bluetooth"];
                    ps.Alarm = checkValue["Alarm"];
                    ps.Unlocked = checkValue["Lock"];
                    ps.DoNotDisturb = checkValue["DND"];

                    ps.ShowTemperature = checkValue["ShowTemperature"];

                    if (ps.Calories != 1234)
                    {
                        Array.Resize(ref objson, objson.Length + 1);
                        objson[count] = ps;
                        count++;
                    }
                }

                string string_json_temp = JsonConvert.SerializeObject(objson, Formatting.None, new JsonSerializerSettings
                {
                    //DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
                var objsontemp = JsonConvert.DeserializeObject<object[]>(string_json_temp);

                string formatted = JsonConvert.SerializeObject(objsontemp, Formatting.Indented);
                //richTextBox_JsonText.Text = formatted;


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

                string fullfilename = saveFileDialog.FileName;
                //richTextBox_JsonText.Text = formatted;
                File.WriteAllText(fullfilename, formatted, Encoding.UTF8);
            }
        }

        // случайные значения ностроек
        private void button_JsonPreview_Random_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            userControl_Set1.RandomValue(rnd);
            userControl_Set2.RandomValue(rnd);
            userControl_Set3.RandomValue(rnd);
            userControl_Set4.RandomValue(rnd);
            userControl_Set5.RandomValue(rnd);
            userControl_Set6.RandomValue(rnd);
            userControl_Set7.RandomValue(rnd);
            userControl_Set8.RandomValue(rnd);
            userControl_Set9.RandomValue(rnd);
            userControl_Set10.RandomValue(rnd);
            userControl_Set11.RandomValue(rnd);
            userControl_Set12.RandomValue(rnd);

            //PreviewImage();
            SetPreferences(userControl_Set12);
            if (!userControl_Set1.Collapsed) SetPreferences(userControl_Set1);
            if (!userControl_Set2.Collapsed) SetPreferences(userControl_Set2);
            if (!userControl_Set3.Collapsed) SetPreferences(userControl_Set3);
            if (!userControl_Set4.Collapsed) SetPreferences(userControl_Set4);
            if (!userControl_Set5.Collapsed) SetPreferences(userControl_Set5);
            if (!userControl_Set6.Collapsed) SetPreferences(userControl_Set6);
            if (!userControl_Set7.Collapsed) SetPreferences(userControl_Set7);
            if (!userControl_Set8.Collapsed) SetPreferences(userControl_Set8);
            if (!userControl_Set9.Collapsed) SetPreferences(userControl_Set9);
            if (!userControl_Set10.Collapsed) SetPreferences(userControl_Set10);
            if (!userControl_Set11.Collapsed) SetPreferences(userControl_Set11);
            //PreviewView = true;
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
            if (Watch_Face.DateAmazfit != null && Watch_Face.DateAmazfit.MonthAndDay != null && Watch_Face.DateAmazfit.MonthAndDay.Separate != null)
            {
                if (Watch_Face.DateAmazfit.MonthAndDay.Separate.MonthName != null && Watch_Face.DateAmazfit.MonthAndDay.Separate.Month != null)
                {
                    MessageBox.Show(Properties.FormStrings.Message_WarningMonthName,
                            Properties.FormStrings.Message_Warning_Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            // дата в одну линию и отдельными блоками
            if (Watch_Face.DateAmazfit != null && Watch_Face.DateAmazfit.MonthAndDay != null)
            {
                if (Watch_Face.DateAmazfit.MonthAndDay.Separate != null && Watch_Face.DateAmazfit.MonthAndDay.OneLine != null)
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
                if (radioButton_TRex_pro.Checked)
                {
                    bitmap = new Bitmap(Convert.ToInt32(360), Convert.ToInt32(360), PixelFormat.Format32bppArgb);
                    mask = new Bitmap(Application.StartupPath + @"\Mask\mask_trex_pro.png");
                }
                if (radioButton_ZeppE.Checked)
                {
                    bitmap = new Bitmap(Convert.ToInt32(416), Convert.ToInt32(416), PixelFormat.Format32bppArgb);
                    mask = new Bitmap(Application.StartupPath + @"\Mask\mask_zepp_e.png");
                }
                Graphics gPanel = Graphics.FromImage(bitmap);
                int link = radioButton_ScreenNormal.Checked ? 0 : 1;
                PreviewToBitmap(gPanel, 1.0f, false, false, false, false, false, false, false, true, false, false, false, link);
                if (checkBox_WatchSkin_Use.Checked) bitmap = ApplyWatchSkin(bitmap);
                else if (checkBox_crop.Checked) bitmap = ApplyMask(bitmap, mask);
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
                if (radioButton_TRex_pro.Checked)
                {
                    bitmap = new Bitmap(Convert.ToInt32(360), Convert.ToInt32(360), PixelFormat.Format32bppArgb);
                    mask = new Bitmap(Application.StartupPath + @"\Mask\mask_trex_pro.png");
                }
                if (radioButton_ZeppE.Checked)
                {
                    bitmap = new Bitmap(Convert.ToInt32(416), Convert.ToInt32(416), PixelFormat.Format32bppArgb);
                    mask = new Bitmap(Application.StartupPath + @"\Mask\mask_zepp_e.png");
                }
                Bitmap bitmapTemp = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
                Graphics gPanel = Graphics.FromImage(bitmap);
                bool save = false;
                Random rnd = new Random();
                PreviewView = false;
                int SetNumber = Watch_Face_Preview_Set.SetNumber;

                using (MagickImageCollection collection = new MagickImageCollection())
                {
                    // основной экран
                    for (int i = 0; i < 13; i++)
                    {
                        save = false;
                        switch (i)
                        {
                            case 0:
                                //button_Set1.PerformClick();
                                SetPreferences(userControl_Set1);
                                save = true;
                                break;
                            case 1:
                                if (userControl_Set2.numericUpDown_Calories_Set.Value != 1234)
                                {
                                    SetPreferences(userControl_Set2);
                                    save = true;
                                }
                                break;
                            case 2:
                                if (userControl_Set3.numericUpDown_Calories_Set.Value != 1234)
                                {
                                    SetPreferences(userControl_Set3);
                                    save = true;
                                }
                                break;
                            case 3:
                                if (userControl_Set4.numericUpDown_Calories_Set.Value != 1234)
                                {
                                    SetPreferences(userControl_Set4);
                                    save = true;
                                }
                                break;
                            case 4:
                                if (userControl_Set5.numericUpDown_Calories_Set.Value != 1234)
                                {
                                    SetPreferences(userControl_Set5);
                                    save = true;
                                }
                                break;
                            case 5:
                                if (userControl_Set6.numericUpDown_Calories_Set.Value != 1234)
                                {
                                    SetPreferences(userControl_Set6);
                                    save = true;
                                }
                                break;
                            case 6:
                                if (userControl_Set7.numericUpDown_Calories_Set.Value != 1234)
                                {
                                    SetPreferences(userControl_Set7);
                                    save = true;
                                }
                                break;
                            case 7:
                                if (userControl_Set8.numericUpDown_Calories_Set.Value != 1234)
                                {
                                    SetPreferences(userControl_Set8);
                                    save = true;
                                }
                                break;
                            case 8:
                                if (userControl_Set9.numericUpDown_Calories_Set.Value != 1234)
                                {
                                    SetPreferences(userControl_Set9);
                                    save = true;
                                }
                                break;
                            case 9:
                                if (userControl_Set10.numericUpDown_Calories_Set.Value != 1234)
                                {
                                    SetPreferences(userControl_Set10);
                                    save = true;
                                }
                                break;
                            case 10:
                                if (userControl_Set11.numericUpDown_Calories_Set.Value != 1234)
                                {
                                    SetPreferences(userControl_Set11);
                                    save = true;
                                }
                                break;
                            case 11:
                                if (userControl_Set12.numericUpDown_Calories_Set.Value != 1234)
                                {
                                    SetPreferences(userControl_Set12);
                                    save = true;
                                }
                                break;
                        }

                        if (save)
                        {
                            bitmap = bitmapTemp; 
                            gPanel = Graphics.FromImage(bitmap);
                            Logger.WriteLine("SaveGIF SetPreferences(" + i.ToString() + ")");

                            //int link = radioButton_ScreenNormal.Checked ? 0 : 1;
                            int link = 0;
                            PreviewToBitmap(gPanel, 1.0f, false, false, false, false, false, false, false, true, 
                                false, false, false, link);
                            //if (checkBox_crop.Checked) {
                            //    bitmap = ApplyMask(bitmap, mask);
                            //    gPanel = Graphics.FromImage(bitmap);
                            //}
                            if (checkBox_WatchSkin_Use.Checked) bitmap = ApplyWatchSkin(bitmap);
                            else if (checkBox_crop.Checked) bitmap = ApplyMask(bitmap, mask);
                            // Add first image and set the animation delay to 100ms
                            MagickImage item = new MagickImage(bitmap);
                            //ExifProfile profile = item.GetExifProfile();
                            collection.Add(item);
                            //collection[collection.Count - 1].AnimationDelay = 100;
                            collection[collection.Count - 1].AnimationDelay = (int)(100 * numericUpDown_Gif_Speed.Value);

                        }
                    }

                    Logger.WriteLine("SaveGIF_AOD");
                    // AOD
                    if (Watch_Face.ScreenIdle != null)
                    {

                        bitmap = bitmapTemp;
                        gPanel = Graphics.FromImage(bitmap);
                        //int link = radioButton_ScreenNormal.Checked ? 0 : 1;
                        int link_AOD = 1;
                        PreviewToBitmap(gPanel, 1.0f, false, false, false, false, false, false, false, true, 
                            false, false, false, link_AOD);
                        //if (checkBox_crop.Checked)
                        //{
                        //    bitmap = ApplyMask(bitmap, mask);
                        //    gPanel = Graphics.FromImage(bitmap);
                        //}
                        if (checkBox_WatchSkin_Use.Checked) bitmap = ApplyWatchSkin(bitmap);
                        else if (checkBox_crop.Checked) bitmap = ApplyMask(bitmap, mask);
                        // Add first image and set the animation delay to 100ms
                        MagickImage item_AOD = new MagickImage(bitmap);
                        //ExifProfile profile = item.GetExifProfile();
                        collection.Add(item_AOD);
                        //collection[collection.Count - 1].AnimationDelay = 100;
                        collection[collection.Count - 1].AnimationDelay = (int)(100 * numericUpDown_Gif_Speed.Value);


                        SetPreferences(userControl_Set1);
                        bitmap = bitmapTemp;
                        gPanel = Graphics.FromImage(bitmap);
                        PreviewToBitmap(gPanel, 1.0f, false, false, false, false, false, false, false, true, 
                            false, false, false, link_AOD);
                        //if (checkBox_crop.Checked)
                        //{
                        //    bitmap = ApplyMask(bitmap, mask);
                        //    gPanel = Graphics.FromImage(bitmap);
                        //}
                        if (checkBox_WatchSkin_Use.Checked) bitmap = ApplyWatchSkin(bitmap);
                        else if (checkBox_crop.Checked) bitmap = ApplyMask(bitmap, mask);
                        item_AOD = new MagickImage(bitmap);
                        //ExifProfile profile = item.GetExifProfile();
                        collection.Add(item_AOD);
                        //collection[collection.Count - 1].AnimationDelay = 100;
                        collection[collection.Count - 1].AnimationDelay = (int)(100 * numericUpDown_Gif_Speed.Value);
                    }

                    if (Watch_Face != null && Watch_Face.Widgets != null && Watch_Face.Widgets.Widget != null)
                    {
                        if (comboBox_WidgetNumber.Items.Count > 0)
                        {
                            for (int i = 0; i < comboBox_WidgetNumber.Items.Count; i++)
                            {
                                bitmap = bitmapTemp;
                                gPanel = Graphics.FromImage(bitmap);
                                DrawWidgetEditScreen(gPanel, false, false, i, true);

                                if (checkBox_WatchSkin_Use.Checked) bitmap = ApplyWatchSkin(bitmap);
                                else if (checkBox_crop.Checked) bitmap = ApplyMask(bitmap, mask);
                                MagickImage item = new MagickImage(bitmap);
                                collection.Add(item);
                                collection[collection.Count - 1].AnimationDelay = (int)(100 * numericUpDown_Gif_Speed.Value);
                            }
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
                switch (SetNumber)
                {
                    case 1:
                        SetPreferences(userControl_Set1);
                        break;
                    case 2:
                        SetPreferences(userControl_Set2);
                        break;
                    case 3:
                        SetPreferences(userControl_Set3);
                        break;
                    case 4:
                        SetPreferences(userControl_Set4);
                        break;
                    case 5:
                        SetPreferences(userControl_Set5);
                        break;
                    case 6:
                        SetPreferences(userControl_Set6);
                        break;
                    case 7:
                        SetPreferences(userControl_Set7);
                        break;
                    case 8:
                        SetPreferences(userControl_Set8);
                        break;
                    case 9:
                        SetPreferences(userControl_Set9);
                        break;
                    case 10:
                        SetPreferences(userControl_Set10);
                        break;
                    case 11:
                        SetPreferences(userControl_Set11);
                        break;
                    case 12:
                        SetPreferences(userControl_Set12);
                        break;
                    default:
                        SetPreferences(userControl_Set12);
                        break;
                }
                PreviewView = true;
                mask.Dispose();
                bitmapTemp.Dispose();
                bitmap.Dispose();
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

        public Bitmap ApplyWidgetMask(Bitmap inputImage, int maskIndex)
        {
            Logger.WriteLine("* ApplyWidgetMask");
            ImageMagick.MagickImage image = new ImageMagick.MagickImage(inputImage);
            if (maskIndex >= 0 && maskIndex < ListImagesFullName.Count)
            {
                ImageMagick.MagickImage combineMask = new ImageMagick.MagickImage(ListImagesFullName[maskIndex]);
                image.Composite(combineMask, ImageMagick.CompositeOperator.Xor, Channels.Alpha); 
            }

            Logger.WriteLine("* ApplyWidgetMask (end)");
            return image.ToBitmap();
        }

        // изменили модель часов
        private void radioButton_Model_Changed(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null && !radioButton.Checked) return;
            if (radioButton_GTR2.Checked || radioButton_GTR2e.Checked)
            {
                pictureBox_Preview.Size = new Size((int)(230 * currentDPI), (int)(230 * currentDPI));
                Program_Settings.unpack_command = Program_Settings.unpack_command_GTR_2;
                textBox_WatchSkin_Path.Text = Program_Settings.WatchSkin_GTR_2;
            }
            else if (radioButton_GTS2.Checked)
            {
                pictureBox_Preview.Size = new Size((int)(177 * currentDPI), (int)(224 * currentDPI));
                Program_Settings.unpack_command = Program_Settings.unpack_command_GTS_2;
                textBox_WatchSkin_Path.Text = Program_Settings.WatchSkin_GTS_2;
            }
            else if (radioButton_TRex_pro.Checked)
            {
                pictureBox_Preview.Size = new Size((int)(183 * currentDPI), (int)(183 * currentDPI));
                Program_Settings.unpack_command = Program_Settings.unpack_command_TRex_pro;
                textBox_WatchSkin_Path.Text = Program_Settings.WatchSkin_TRex_pro;
            }
            else if (radioButton_ZeppE.Checked)
            {
                pictureBox_Preview.Size = new Size((int)(211 * currentDPI), (int)(211 * currentDPI));
                Program_Settings.unpack_command = Program_Settings.unpack_command_GTR_2;
                textBox_WatchSkin_Path.Text = Program_Settings.WatchSkin_Zepp_E;
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
                if (Form_Preview.Model_Wath.model_GTR2 != radioButton_GTR2.Checked)
                    Form_Preview.Model_Wath.model_GTR2 = radioButton_GTR2.Checked;
                if (Form_Preview.Model_Wath.model_GTR2e != radioButton_GTR2e.Checked)
                    Form_Preview.Model_Wath.model_GTR2e = radioButton_GTR2e.Checked;
                if (Form_Preview.Model_Wath.model_GTS2 != radioButton_GTS2.Checked)
                    Form_Preview.Model_Wath.model_GTS2 = radioButton_GTS2.Checked;
                if (Form_Preview.Model_Wath.model_TRex_pro != radioButton_TRex_pro.Checked)
                    Form_Preview.Model_Wath.model_TRex_pro = radioButton_TRex_pro.Checked;
                if (Form_Preview.Model_Wath.model_Zepp_E != radioButton_ZeppE.Checked)
                    Form_Preview.Model_Wath.model_Zepp_E = radioButton_ZeppE.Checked;
                formPreview.radioButton_CheckedChanged(sender, e);
            }

            Program_Settings.Model_GTR2 = radioButton_GTR2.Checked;
            Program_Settings.Model_GTR2e = radioButton_GTR2e.Checked;
            Program_Settings.Model_GTS2 = radioButton_GTS2.Checked;
            Program_Settings.Model_TRex_pro = radioButton_TRex_pro.Checked;
            Program_Settings.Model_Zepp_E = radioButton_ZeppE.Checked;
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
            else if (radioButton_GTR2e.Checked)
            {
                FormName = "GTR 2e watch face editor";
            }
            else if (radioButton_GTS2.Checked)
            {
                FormName = "GTS 2 watch face editor";
            }
            else if (radioButton_TRex_pro.Checked)
            {
                FormName = "T-Rex Pro watch face editor";
            }
            else if (radioButton_ZeppE.Checked)
            {
                FormName = "Zepp E Circle watch face editor";
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
                //PreviewImage();
                //JSON_write();
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
        private void checkBox_WidgetsArea_CheckedChanged(object sender, EventArgs e)
        {
            Program_Settings.Show_Widgets_Area = checkBox_WidgetsArea.Checked;
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
                    //MotiomAnimation_Update = true;
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

                    //MotiomAnimation_Update = false;

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
                    //MotiomAnimation_Update = true;
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

                    //MotiomAnimation_Update = false;

                    PreviewImage();
                    JSON_write();
                }
            }
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

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage.Name == "tabPageConverting")
            {
                if (radioButton_GTR2.Checked)
                {
                    radioButton_ConvertingInput_GTR2.Checked = true;
                    numericUpDown_ConvertingInput_Custom.Value = 454;
                }
                if (radioButton_TRex_pro.Checked)
                {
                    radioButton_ConvertingInput_TRexPro.Checked = true;
                    numericUpDown_ConvertingInput_Custom.Value = 360;
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
                
                int DeviceId = 59;
                string suffix = "_GTR_2";
                float scale = 1;
                if (radioButton_ConvertingOutput_TRexPro.Checked)
                {
                    suffix = "_T-Rex_Pro";
                    DeviceId = 83;
                }
                if (radioButton_ConvertingOutput_ZeppE.Checked)
                {
                    suffix = "_Zepp_E";
                    DeviceId = 60;
                }
                if (radioButton_ConvertingOutput_Custom.Checked)
                {
                    suffix = "_Custom_" + numericUpDown_ConvertingOutput_Custom.Value.ToString();
                    DeviceId = 0;
                }

                scale = (float)(numericUpDown_ConvertingOutput_Custom.Value / numericUpDown_ConvertingInput_Custom.Value);

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
            if (scale <= 0) return new Bitmap(image);
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
            if (Watch_Face == null) return;
            if (DeviceId != 0)
            {
                if (Watch_Face.Info == null) Watch_Face.Info = new Device_Id();
                Watch_Face.Info.DeviceId = DeviceId;
            }

            #region Time
            if (Watch_Face.DialFace != null)
            {
                if (Watch_Face.DialFace.AnalogDialFace != null)
                {
                    if (Watch_Face.DialFace.AnalogDialFace.Hours != null) Watch_Face.DialFace.AnalogDialFace.Hours =
                            ClockHand_Scale(Watch_Face.DialFace.AnalogDialFace.Hours, scale);

                    if (Watch_Face.DialFace.AnalogDialFace.Minutes != null) Watch_Face.DialFace.AnalogDialFace.Minutes =
                            ClockHand_Scale(Watch_Face.DialFace.AnalogDialFace.Minutes, scale);

                    if (Watch_Face.DialFace.AnalogDialFace.Seconds != null) Watch_Face.DialFace.AnalogDialFace.Seconds = 
                            ClockHand_Scale(Watch_Face.DialFace.AnalogDialFace.Seconds, scale);
                }

                if (Watch_Face.DialFace.DigitalDialFace != null)
                {
                    if(Watch_Face.DialFace.DigitalDialFace.Digits != null)
                    {
                        foreach (DigitalTimeDigit digitalTimeDigit in Watch_Face.DialFace.DigitalDialFace.Digits)
                        {
                            if(digitalTimeDigit.Digit != null)
                            {
                                if (digitalTimeDigit.Digit.Image != null) 
                                {
                                    if(digitalTimeDigit.Digit.Image.DecimalPointImageIndex != null)
                                        digitalTimeDigit.Digit.Image.DecimalPointImageIndex =
                                            (int)Math.Round((int)digitalTimeDigit.Digit.Image.DecimalPointImageIndex * 
                                            scale, MidpointRounding.AwayFromZero);
                                    if (digitalTimeDigit.Digit.Image.DelimiterImageIndex != null)
                                        digitalTimeDigit.Digit.Image.DelimiterImageIndex =
                                            (int)Math.Round((int)digitalTimeDigit.Digit.Image.DelimiterImageIndex *
                                            scale, MidpointRounding.AwayFromZero);
                                    if (digitalTimeDigit.Digit.Image.NoDataImageIndex != null)
                                        digitalTimeDigit.Digit.Image.NoDataImageIndex =
                                            (int)Math.Round((int)digitalTimeDigit.Digit.Image.NoDataImageIndex *
                                            scale, MidpointRounding.AwayFromZero);

                                    digitalTimeDigit.Digit.Image.X =
                                        (int)Math.Round(digitalTimeDigit.Digit.Image.X * scale, MidpointRounding.AwayFromZero);
                                    digitalTimeDigit.Digit.Image.Y =
                                        (int)Math.Round(digitalTimeDigit.Digit.Image.Y * scale, MidpointRounding.AwayFromZero);

                                }

                                if (digitalTimeDigit.Digit.Spacing != null)
                                    digitalTimeDigit.Digit.Spacing =
                                        (int)Math.Round((int)digitalTimeDigit.Digit.Spacing * scale, MidpointRounding.AwayFromZero);

                                if (digitalTimeDigit.Digit.SystemFont != null)
                                {
                                    digitalTimeDigit.Digit.SystemFont.Size = 
                                        (int)Math.Round(digitalTimeDigit.Digit.SystemFont.Size * scale, MidpointRounding.AwayFromZero);

                                    if (digitalTimeDigit.Digit.SystemFont.Coordinates != null)
                                    {
                                        digitalTimeDigit.Digit.SystemFont.Coordinates.X = 
                                            (int)Math.Round(digitalTimeDigit.Digit.SystemFont.Coordinates.X * 
                                            scale, MidpointRounding.AwayFromZero);
                                        digitalTimeDigit.Digit.SystemFont.Coordinates.Y =
                                            (int)Math.Round(digitalTimeDigit.Digit.SystemFont.Coordinates.Y *
                                            scale, MidpointRounding.AwayFromZero);
                                    }

                                    if (digitalTimeDigit.Digit.SystemFont.FontRotate != null)
                                    {
                                        digitalTimeDigit.Digit.SystemFont.FontRotate.Radius =
                                            (int)Math.Round(digitalTimeDigit.Digit.SystemFont.FontRotate.Radius * 
                                            scale, MidpointRounding.AwayFromZero);
                                        digitalTimeDigit.Digit.SystemFont.FontRotate.X =
                                            (int)Math.Round(digitalTimeDigit.Digit.SystemFont.FontRotate.X *
                                            scale, MidpointRounding.AwayFromZero);
                                        digitalTimeDigit.Digit.SystemFont.FontRotate.Y =
                                            (int)Math.Round(digitalTimeDigit.Digit.SystemFont.FontRotate.Y *
                                            scale, MidpointRounding.AwayFromZero);
                                    }
                                }
                            }

                            if (digitalTimeDigit.Separator != null && digitalTimeDigit.Separator.Coordinates != null)
                            {
                                digitalTimeDigit.Separator.Coordinates.X =
                                    (int)Math.Round(digitalTimeDigit.Separator.Coordinates.X *
                                    scale, MidpointRounding.AwayFromZero);
                                digitalTimeDigit.Separator.Coordinates.Y =
                                    (int)Math.Round(digitalTimeDigit.Separator.Coordinates.Y *
                                    scale, MidpointRounding.AwayFromZero);
                            }
                        }
                    }
                    
                    if (Watch_Face.DialFace.DigitalDialFace.AM != null)
                    {
                        if (Watch_Face.DialFace.DigitalDialFace.AM.Coordinates != null)
                        {
                            Watch_Face.DialFace.DigitalDialFace.AM.Coordinates.X =
                                (int)Math.Round(Watch_Face.DialFace.DigitalDialFace.AM.Coordinates.X *
                                scale, MidpointRounding.AwayFromZero);
                            Watch_Face.DialFace.DigitalDialFace.AM.Coordinates.Y =
                                (int)Math.Round(Watch_Face.DialFace.DigitalDialFace.AM.Coordinates.Y *
                                scale, MidpointRounding.AwayFromZero);
                        }

                    }

                    if (Watch_Face.DialFace.DigitalDialFace.PM != null)
                    {
                        if (Watch_Face.DialFace.DigitalDialFace.PM.Coordinates != null)
                        {
                            Watch_Face.DialFace.DigitalDialFace.PM.Coordinates.X =
                                (int)Math.Round(Watch_Face.DialFace.DigitalDialFace.PM.Coordinates.X *
                                scale, MidpointRounding.AwayFromZero);
                            Watch_Face.DialFace.DigitalDialFace.PM.Coordinates.Y =
                                (int)Math.Round(Watch_Face.DialFace.DigitalDialFace.PM.Coordinates.Y *
                                scale, MidpointRounding.AwayFromZero);
                        }

                    }
                }
            }
            #endregion

            #region Date
            if (Watch_Face.System != null && Watch_Face.System.Date != null)
            {
                if (Watch_Face.System.Date.DateClockHand != null)
                {
                    if (Watch_Face.System.Date.DateClockHand.DayClockHand != null)
                        Watch_Face.System.Date.DateClockHand.DayClockHand = 
                            ClockHand_Scale(Watch_Face.System.Date.DateClockHand.DayClockHand, scale);

                    if (Watch_Face.System.Date.DateClockHand.MonthClockHand != null)
                        Watch_Face.System.Date.DateClockHand.MonthClockHand =
                            ClockHand_Scale(Watch_Face.System.Date.DateClockHand.MonthClockHand, scale);

                    if (Watch_Face.System.Date.DateClockHand.WeekDayClockHand != null)
                        Watch_Face.System.Date.DateClockHand.WeekDayClockHand =
                            ClockHand_Scale(Watch_Face.System.Date.DateClockHand.WeekDayClockHand, scale);
                }

                if (Watch_Face.System.Date.DateDigits != null)
                {
                    foreach (DigitalDateDigit digitalDateDigit in Watch_Face.System.Date.DateDigits)
                    {
                        if (digitalDateDigit.Digit != null)
                        {
                            if (digitalDateDigit.Digit.Image != null)
                            {
                                if (digitalDateDigit.Digit.Image.DecimalPointImageIndex != null)
                                    digitalDateDigit.Digit.Image.DecimalPointImageIndex =
                                        (int)Math.Round((int)digitalDateDigit.Digit.Image.DecimalPointImageIndex *
                                        scale, MidpointRounding.AwayFromZero);
                                if (digitalDateDigit.Digit.Image.DelimiterImageIndex != null)
                                    digitalDateDigit.Digit.Image.DelimiterImageIndex =
                                        (int)Math.Round((int)digitalDateDigit.Digit.Image.DelimiterImageIndex *
                                        scale, MidpointRounding.AwayFromZero);
                                if (digitalDateDigit.Digit.Image.NoDataImageIndex != null)
                                    digitalDateDigit.Digit.Image.NoDataImageIndex =
                                        (int)Math.Round((int)digitalDateDigit.Digit.Image.NoDataImageIndex *
                                        scale, MidpointRounding.AwayFromZero);

                                digitalDateDigit.Digit.Image.X =
                                    (int)Math.Round(digitalDateDigit.Digit.Image.X * scale, MidpointRounding.AwayFromZero);
                                digitalDateDigit.Digit.Image.Y =
                                    (int)Math.Round(digitalDateDigit.Digit.Image.Y * scale, MidpointRounding.AwayFromZero);

                            }

                            if (digitalDateDigit.Digit.Spacing != null)
                                digitalDateDigit.Digit.Spacing =
                                    (int)Math.Round((int)digitalDateDigit.Digit.Spacing * scale, MidpointRounding.AwayFromZero);

                            if (digitalDateDigit.Digit.SystemFont != null)
                            {
                                digitalDateDigit.Digit.SystemFont.Size =
                                    (int)Math.Round(digitalDateDigit.Digit.SystemFont.Size * scale, MidpointRounding.AwayFromZero);

                                if (digitalDateDigit.Digit.SystemFont.Coordinates != null)
                                {
                                    digitalDateDigit.Digit.SystemFont.Coordinates.X =
                                        (int)Math.Round(digitalDateDigit.Digit.SystemFont.Coordinates.X *
                                        scale, MidpointRounding.AwayFromZero);
                                    digitalDateDigit.Digit.SystemFont.Coordinates.Y =
                                        (int)Math.Round(digitalDateDigit.Digit.SystemFont.Coordinates.Y *
                                        scale, MidpointRounding.AwayFromZero);
                                }

                                if (digitalDateDigit.Digit.SystemFont.FontRotate != null)
                                {
                                    digitalDateDigit.Digit.SystemFont.FontRotate.Radius =
                                        (int)Math.Round(digitalDateDigit.Digit.SystemFont.FontRotate.Radius *
                                        scale, MidpointRounding.AwayFromZero);
                                    digitalDateDigit.Digit.SystemFont.FontRotate.X =
                                        (int)Math.Round(digitalDateDigit.Digit.SystemFont.FontRotate.X *
                                        scale, MidpointRounding.AwayFromZero);
                                    digitalDateDigit.Digit.SystemFont.FontRotate.Y =
                                        (int)Math.Round(digitalDateDigit.Digit.SystemFont.FontRotate.Y *
                                        scale, MidpointRounding.AwayFromZero);
                                }
                            }
                        }
                        if (digitalDateDigit.Separator != null && digitalDateDigit.Separator.Coordinates != null)
                        {
                            digitalDateDigit.Separator.Coordinates.X =
                                (int)Math.Round(digitalDateDigit.Separator.Coordinates.X *
                                scale, MidpointRounding.AwayFromZero);
                            digitalDateDigit.Separator.Coordinates.Y =
                                (int)Math.Round(digitalDateDigit.Separator.Coordinates.Y *
                                scale, MidpointRounding.AwayFromZero);
                        }
                    }
                }

                if (Watch_Face.System.Date.WeeksDigits != null)
                {
                    if (Watch_Face.System.Date.WeeksDigits.Digit != null) Watch_Face.System.Date.WeeksDigits.Digit =
                            Digits_Scale(Watch_Face.System.Date.WeeksDigits.Digit, scale);

                    if(Watch_Face.System.Date.WeeksDigits.Separator != null && 
                        Watch_Face.System.Date.WeeksDigits.Separator.Coordinates != null)
                    {
                        Watch_Face.System.Date.WeeksDigits.Separator.Coordinates.X =
                            (int)Math.Round(Watch_Face.System.Date.WeeksDigits.Separator.Coordinates.X * 
                            scale, MidpointRounding.AwayFromZero);
                        Watch_Face.System.Date.WeeksDigits.Separator.Coordinates.Y =
                            (int)Math.Round(Watch_Face.System.Date.WeeksDigits.Separator.Coordinates.Y * 
                            scale, MidpointRounding.AwayFromZero);
                    }
                }
            }
            #endregion

            #region Status
            if (Watch_Face.System != null && Watch_Face.System.Status != null)
            {
                if (Watch_Face.System.Status.Alarm != null &&
                        Watch_Face.System.Status.Alarm.Coordinates != null)
                {
                    Watch_Face.System.Status.Alarm.Coordinates.X =
                        (int)Math.Round(Watch_Face.System.Status.Alarm.Coordinates.X *
                        scale, MidpointRounding.AwayFromZero);
                    Watch_Face.System.Status.Alarm.Coordinates.Y =
                        (int)Math.Round(Watch_Face.System.Status.Alarm.Coordinates.Y *
                        scale, MidpointRounding.AwayFromZero);
                }

                if (Watch_Face.System.Status.Bluetooth != null &&
                        Watch_Face.System.Status.Bluetooth.Coordinates != null)
                {
                    Watch_Face.System.Status.Bluetooth.Coordinates.X =
                        (int)Math.Round(Watch_Face.System.Status.Bluetooth.Coordinates.X *
                        scale, MidpointRounding.AwayFromZero);
                    Watch_Face.System.Status.Bluetooth.Coordinates.Y =
                        (int)Math.Round(Watch_Face.System.Status.Bluetooth.Coordinates.Y *
                        scale, MidpointRounding.AwayFromZero);
                }

                if (Watch_Face.System.Status.DoNotDisturb != null &&
                        Watch_Face.System.Status.DoNotDisturb.Coordinates != null)
                {
                    Watch_Face.System.Status.DoNotDisturb.Coordinates.X =
                        (int)Math.Round(Watch_Face.System.Status.DoNotDisturb.Coordinates.X *
                        scale, MidpointRounding.AwayFromZero);
                    Watch_Face.System.Status.DoNotDisturb.Coordinates.Y =
                        (int)Math.Round(Watch_Face.System.Status.DoNotDisturb.Coordinates.Y *
                        scale, MidpointRounding.AwayFromZero);
                }

                if (Watch_Face.System.Status.Lock != null &&
                        Watch_Face.System.Status.Lock.Coordinates != null)
                {
                    Watch_Face.System.Status.Lock.Coordinates.X =
                        (int)Math.Round(Watch_Face.System.Status.Lock.Coordinates.X *
                        scale, MidpointRounding.AwayFromZero);
                    Watch_Face.System.Status.Lock.Coordinates.Y =
                        (int)Math.Round(Watch_Face.System.Status.Lock.Coordinates.Y *
                        scale, MidpointRounding.AwayFromZero);
                }
            }
            #endregion

            #region Activity 
            if (Watch_Face.System != null && Watch_Face.System.Activity != null)
            {
                foreach (Activity activity in Watch_Face.System.Activity)
                {
                    Activity_Scale(activity, scale);
                }
            }
            #endregion

            #region Widgets 
            if (Watch_Face.Widgets != null && Watch_Face.Widgets.Widget != null)
            {
                foreach (Widget widget in Watch_Face.Widgets.Widget)
                {
                    widget.Height = (int)Math.Round(widget.Height * scale, MidpointRounding.AwayFromZero);
                    widget.Width = (int)Math.Round(widget.Width * scale, MidpointRounding.AwayFromZero);
                    widget.X = (int)Math.Round(widget.X * scale, MidpointRounding.AwayFromZero);
                    widget.Y = (int)Math.Round(widget.Y * scale, MidpointRounding.AwayFromZero);
                    if(widget.WidgetElement != null)
                    {
                        foreach (WidgetElement widgetElement in widget.WidgetElement)
                        {
                            if(widgetElement.Activity != null)
                                foreach (Activity activity in widgetElement.Activity)
                                {
                                    Activity_Scale(activity, scale);
                                }

                            if (widgetElement.Date != null)
                            {
                                if (widgetElement.Date.DateClockHand != null)
                                {
                                    if (widgetElement.Date.DateClockHand.DayClockHand != null)
                                        widgetElement.Date.DateClockHand.DayClockHand =
                                            ClockHand_Scale(widgetElement.Date.DateClockHand.DayClockHand, scale);

                                    if (widgetElement.Date.DateClockHand.MonthClockHand != null)
                                        widgetElement.Date.DateClockHand.MonthClockHand =
                                            ClockHand_Scale(widgetElement.Date.DateClockHand.MonthClockHand, scale);

                                    if (widgetElement.Date.DateClockHand.WeekDayClockHand != null)
                                        widgetElement.Date.DateClockHand.WeekDayClockHand =
                                            ClockHand_Scale(widgetElement.Date.DateClockHand.WeekDayClockHand, scale);
                                }

                                if (widgetElement.Date.DateDigits != null)
                                {
                                    foreach (DigitalDateDigit digitalDateDigit in widgetElement.Date.DateDigits)
                                    {
                                        if (digitalDateDigit.Digit != null)
                                        {
                                            if (digitalDateDigit.Digit.Image != null)
                                            {
                                                if (digitalDateDigit.Digit.Image.DecimalPointImageIndex != null)
                                                    digitalDateDigit.Digit.Image.DecimalPointImageIndex =
                                                        (int)Math.Round((int)digitalDateDigit.Digit.Image.DecimalPointImageIndex *
                                                        scale, MidpointRounding.AwayFromZero);
                                                if (digitalDateDigit.Digit.Image.DelimiterImageIndex != null)
                                                    digitalDateDigit.Digit.Image.DelimiterImageIndex =
                                                        (int)Math.Round((int)digitalDateDigit.Digit.Image.DelimiterImageIndex *
                                                        scale, MidpointRounding.AwayFromZero);
                                                if (digitalDateDigit.Digit.Image.NoDataImageIndex != null)
                                                    digitalDateDigit.Digit.Image.NoDataImageIndex =
                                                        (int)Math.Round((int)digitalDateDigit.Digit.Image.NoDataImageIndex *
                                                        scale, MidpointRounding.AwayFromZero);

                                                digitalDateDigit.Digit.Image.X =
                                                    (int)Math.Round(digitalDateDigit.Digit.Image.X * scale, MidpointRounding.AwayFromZero);
                                                digitalDateDigit.Digit.Image.Y =
                                                    (int)Math.Round(digitalDateDigit.Digit.Image.Y * scale, MidpointRounding.AwayFromZero);

                                            }

                                            if (digitalDateDigit.Digit.Spacing != null)
                                                digitalDateDigit.Digit.Spacing =
                                                    (int)Math.Round((int)digitalDateDigit.Digit.Spacing * scale, MidpointRounding.AwayFromZero);

                                            if (digitalDateDigit.Digit.SystemFont != null)
                                            {
                                                digitalDateDigit.Digit.SystemFont.Size =
                                                    (int)Math.Round(digitalDateDigit.Digit.SystemFont.Size * scale, MidpointRounding.AwayFromZero);

                                                if (digitalDateDigit.Digit.SystemFont.Coordinates != null)
                                                {
                                                    digitalDateDigit.Digit.SystemFont.Coordinates.X =
                                                        (int)Math.Round(digitalDateDigit.Digit.SystemFont.Coordinates.X *
                                                        scale, MidpointRounding.AwayFromZero);
                                                    digitalDateDigit.Digit.SystemFont.Coordinates.Y =
                                                        (int)Math.Round(digitalDateDigit.Digit.SystemFont.Coordinates.Y *
                                                        scale, MidpointRounding.AwayFromZero);
                                                }

                                                if (digitalDateDigit.Digit.SystemFont.FontRotate != null)
                                                {
                                                    digitalDateDigit.Digit.SystemFont.FontRotate.Radius =
                                                        (int)Math.Round(digitalDateDigit.Digit.SystemFont.FontRotate.Radius *
                                                        scale, MidpointRounding.AwayFromZero);
                                                    digitalDateDigit.Digit.SystemFont.FontRotate.X =
                                                        (int)Math.Round(digitalDateDigit.Digit.SystemFont.FontRotate.X *
                                                        scale, MidpointRounding.AwayFromZero);
                                                    digitalDateDigit.Digit.SystemFont.FontRotate.Y =
                                                        (int)Math.Round(digitalDateDigit.Digit.SystemFont.FontRotate.Y *
                                                        scale, MidpointRounding.AwayFromZero);
                                                }
                                            }
                                        }
                                        if (digitalDateDigit.Separator != null && digitalDateDigit.Separator.Coordinates != null)
                                        {
                                            digitalDateDigit.Separator.Coordinates.X =
                                                (int)Math.Round(digitalDateDigit.Separator.Coordinates.X *
                                                scale, MidpointRounding.AwayFromZero);
                                            digitalDateDigit.Separator.Coordinates.Y =
                                                (int)Math.Round(digitalDateDigit.Separator.Coordinates.Y *
                                                scale, MidpointRounding.AwayFromZero);
                                        }
                                    }
                                }

                                if (widgetElement.Date.WeeksDigits != null)
                                {
                                    if (widgetElement.Date.WeeksDigits.Digit != null) widgetElement.Date.WeeksDigits.Digit =
                                            Digits_Scale(widgetElement.Date.WeeksDigits.Digit, scale);

                                    if (widgetElement.Date.WeeksDigits.Separator != null &&
                                        widgetElement.Date.WeeksDigits.Separator.Coordinates != null)
                                    {
                                        widgetElement.Date.WeeksDigits.Separator.Coordinates.X =
                                            (int)Math.Round(widgetElement.Date.WeeksDigits.Separator.Coordinates.X *
                                            scale, MidpointRounding.AwayFromZero);
                                        widgetElement.Date.WeeksDigits.Separator.Coordinates.Y =
                                            (int)Math.Round(widgetElement.Date.WeeksDigits.Separator.Coordinates.Y *
                                            scale, MidpointRounding.AwayFromZero);
                                    }
                                }
                            }
                        }
                    }
                    
                }
            }
            #endregion

            if (Watch_Face.ScreenIdle != null)
            {
                #region Time
                if (Watch_Face.ScreenIdle.DialFace != null)
                {
                    if (Watch_Face.ScreenIdle.DialFace.AnalogDialFace != null)
                    {
                        if (Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Hours != null) Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Hours =
                                ClockHand_Scale(Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Hours, scale);

                        if (Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Minutes != null) Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Minutes =
                                ClockHand_Scale(Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Minutes, scale);

                        if (Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Seconds != null) Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Seconds =
                                ClockHand_Scale(Watch_Face.ScreenIdle.DialFace.AnalogDialFace.Seconds, scale);
                    }

                    if (Watch_Face.ScreenIdle.DialFace.DigitalDialFace != null)
                    {
                        if (Watch_Face.ScreenIdle.DialFace.DigitalDialFace.Digits != null)
                        {
                            foreach (DigitalTimeDigit digitalTimeDigit in Watch_Face.ScreenIdle.DialFace.DigitalDialFace.Digits)
                            {
                                if (digitalTimeDigit.Digit != null)
                                {
                                    if (digitalTimeDigit.Digit.Image != null)
                                    {
                                        if (digitalTimeDigit.Digit.Image.DecimalPointImageIndex != null)
                                            digitalTimeDigit.Digit.Image.DecimalPointImageIndex =
                                                (int)Math.Round((int)digitalTimeDigit.Digit.Image.DecimalPointImageIndex *
                                                scale, MidpointRounding.AwayFromZero);
                                        if (digitalTimeDigit.Digit.Image.DelimiterImageIndex != null)
                                            digitalTimeDigit.Digit.Image.DelimiterImageIndex =
                                                (int)Math.Round((int)digitalTimeDigit.Digit.Image.DelimiterImageIndex *
                                                scale, MidpointRounding.AwayFromZero);
                                        if (digitalTimeDigit.Digit.Image.NoDataImageIndex != null)
                                            digitalTimeDigit.Digit.Image.NoDataImageIndex =
                                                (int)Math.Round((int)digitalTimeDigit.Digit.Image.NoDataImageIndex *
                                                scale, MidpointRounding.AwayFromZero);

                                        digitalTimeDigit.Digit.Image.X =
                                            (int)Math.Round(digitalTimeDigit.Digit.Image.X * scale, MidpointRounding.AwayFromZero);
                                        digitalTimeDigit.Digit.Image.Y =
                                            (int)Math.Round(digitalTimeDigit.Digit.Image.Y * scale, MidpointRounding.AwayFromZero);

                                    }

                                    if (digitalTimeDigit.Digit.Spacing != null)
                                        digitalTimeDigit.Digit.Spacing =
                                            (int)Math.Round((int)digitalTimeDigit.Digit.Spacing * scale, MidpointRounding.AwayFromZero);

                                    if (digitalTimeDigit.Digit.SystemFont != null)
                                    {
                                        digitalTimeDigit.Digit.SystemFont.Size =
                                            (int)Math.Round(digitalTimeDigit.Digit.SystemFont.Size * scale, MidpointRounding.AwayFromZero);

                                        if (digitalTimeDigit.Digit.SystemFont.Coordinates != null)
                                        {
                                            digitalTimeDigit.Digit.SystemFont.Coordinates.X =
                                                (int)Math.Round(digitalTimeDigit.Digit.SystemFont.Coordinates.X *
                                                scale, MidpointRounding.AwayFromZero);
                                            digitalTimeDigit.Digit.SystemFont.Coordinates.Y =
                                                (int)Math.Round(digitalTimeDigit.Digit.SystemFont.Coordinates.Y *
                                                scale, MidpointRounding.AwayFromZero);
                                        }

                                        if (digitalTimeDigit.Digit.SystemFont.FontRotate != null)
                                        {
                                            digitalTimeDigit.Digit.SystemFont.FontRotate.Radius =
                                                (int)Math.Round(digitalTimeDigit.Digit.SystemFont.FontRotate.Radius *
                                                scale, MidpointRounding.AwayFromZero);
                                            digitalTimeDigit.Digit.SystemFont.FontRotate.X =
                                                (int)Math.Round(digitalTimeDigit.Digit.SystemFont.FontRotate.X *
                                                scale, MidpointRounding.AwayFromZero);
                                            digitalTimeDigit.Digit.SystemFont.FontRotate.Y =
                                                (int)Math.Round(digitalTimeDigit.Digit.SystemFont.FontRotate.Y *
                                                scale, MidpointRounding.AwayFromZero);
                                        }
                                    }
                                }

                                if (digitalTimeDigit.Separator != null && digitalTimeDigit.Separator.Coordinates != null)
                                {
                                    digitalTimeDigit.Separator.Coordinates.X =
                                        (int)Math.Round(digitalTimeDigit.Separator.Coordinates.X *
                                        scale, MidpointRounding.AwayFromZero);
                                    digitalTimeDigit.Separator.Coordinates.Y =
                                        (int)Math.Round(digitalTimeDigit.Separator.Coordinates.Y *
                                        scale, MidpointRounding.AwayFromZero);
                                }
                            }
                        }

                        if (Watch_Face.ScreenIdle.DialFace.DigitalDialFace.AM != null)
                        {
                            if (Watch_Face.ScreenIdle.DialFace.DigitalDialFace.AM.Coordinates != null)
                            {
                                Watch_Face.ScreenIdle.DialFace.DigitalDialFace.AM.Coordinates.X =
                                    (int)Math.Round(Watch_Face.ScreenIdle.DialFace.DigitalDialFace.AM.Coordinates.X *
                                    scale, MidpointRounding.AwayFromZero);
                                Watch_Face.ScreenIdle.DialFace.DigitalDialFace.AM.Coordinates.Y =
                                    (int)Math.Round(Watch_Face.ScreenIdle.DialFace.DigitalDialFace.AM.Coordinates.Y *
                                    scale, MidpointRounding.AwayFromZero);
                            }

                        }

                        if (Watch_Face.ScreenIdle.DialFace.DigitalDialFace.PM != null)
                        {
                            if (Watch_Face.ScreenIdle.DialFace.DigitalDialFace.PM.Coordinates != null)
                            {
                                Watch_Face.ScreenIdle.DialFace.DigitalDialFace.PM.Coordinates.X =
                                    (int)Math.Round(Watch_Face.ScreenIdle.DialFace.DigitalDialFace.PM.Coordinates.X *
                                    scale, MidpointRounding.AwayFromZero);
                                Watch_Face.ScreenIdle.DialFace.DigitalDialFace.PM.Coordinates.Y =
                                    (int)Math.Round(Watch_Face.ScreenIdle.DialFace.DigitalDialFace.PM.Coordinates.Y *
                                    scale, MidpointRounding.AwayFromZero);
                            }

                        }
                    }
                }
                #endregion

                #region Date
                if (Watch_Face.ScreenIdle.Date != null)
                {
                    if (Watch_Face.ScreenIdle.Date.DateClockHand != null)
                    {
                        if (Watch_Face.ScreenIdle.Date.DateClockHand.DayClockHand != null)
                            Watch_Face.ScreenIdle.Date.DateClockHand.DayClockHand =
                                ClockHand_Scale(Watch_Face.ScreenIdle.Date.DateClockHand.DayClockHand, scale);

                        if (Watch_Face.ScreenIdle.Date.DateClockHand.MonthClockHand != null)
                            Watch_Face.ScreenIdle.Date.DateClockHand.MonthClockHand =
                                ClockHand_Scale(Watch_Face.ScreenIdle.Date.DateClockHand.MonthClockHand, scale);

                        if (Watch_Face.ScreenIdle.Date.DateClockHand.WeekDayClockHand != null)
                            Watch_Face.ScreenIdle.Date.DateClockHand.WeekDayClockHand =
                                ClockHand_Scale(Watch_Face.ScreenIdle.Date.DateClockHand.WeekDayClockHand, scale);
                    }

                    if (Watch_Face.ScreenIdle.Date.DateDigits != null)
                    {
                        foreach (DigitalDateDigit digitalDateDigit in Watch_Face.ScreenIdle.Date.DateDigits)
                        {
                            if (digitalDateDigit.Digit != null)
                            {
                                if (digitalDateDigit.Digit.Image != null)
                                {
                                    if (digitalDateDigit.Digit.Image.DecimalPointImageIndex != null)
                                        digitalDateDigit.Digit.Image.DecimalPointImageIndex =
                                            (int)Math.Round((int)digitalDateDigit.Digit.Image.DecimalPointImageIndex *
                                            scale, MidpointRounding.AwayFromZero);
                                    if (digitalDateDigit.Digit.Image.DelimiterImageIndex != null)
                                        digitalDateDigit.Digit.Image.DelimiterImageIndex =
                                            (int)Math.Round((int)digitalDateDigit.Digit.Image.DelimiterImageIndex *
                                            scale, MidpointRounding.AwayFromZero);
                                    if (digitalDateDigit.Digit.Image.NoDataImageIndex != null)
                                        digitalDateDigit.Digit.Image.NoDataImageIndex =
                                            (int)Math.Round((int)digitalDateDigit.Digit.Image.NoDataImageIndex *
                                            scale, MidpointRounding.AwayFromZero);

                                    digitalDateDigit.Digit.Image.X =
                                        (int)Math.Round(digitalDateDigit.Digit.Image.X * scale, MidpointRounding.AwayFromZero);
                                    digitalDateDigit.Digit.Image.Y =
                                        (int)Math.Round(digitalDateDigit.Digit.Image.Y * scale, MidpointRounding.AwayFromZero);

                                }

                                if (digitalDateDigit.Digit.Spacing != null)
                                    digitalDateDigit.Digit.Spacing =
                                        (int)Math.Round((int)digitalDateDigit.Digit.Spacing * scale, MidpointRounding.AwayFromZero);

                                if (digitalDateDigit.Digit.SystemFont != null)
                                {
                                    digitalDateDigit.Digit.SystemFont.Size =
                                        (int)Math.Round(digitalDateDigit.Digit.SystemFont.Size * scale, MidpointRounding.AwayFromZero);

                                    if (digitalDateDigit.Digit.SystemFont.Coordinates != null)
                                    {
                                        digitalDateDigit.Digit.SystemFont.Coordinates.X =
                                            (int)Math.Round(digitalDateDigit.Digit.SystemFont.Coordinates.X *
                                            scale, MidpointRounding.AwayFromZero);
                                        digitalDateDigit.Digit.SystemFont.Coordinates.Y =
                                            (int)Math.Round(digitalDateDigit.Digit.SystemFont.Coordinates.Y *
                                            scale, MidpointRounding.AwayFromZero);
                                    }

                                    if (digitalDateDigit.Digit.SystemFont.FontRotate != null)
                                    {
                                        digitalDateDigit.Digit.SystemFont.FontRotate.Radius =
                                            (int)Math.Round(digitalDateDigit.Digit.SystemFont.FontRotate.Radius *
                                            scale, MidpointRounding.AwayFromZero);
                                        digitalDateDigit.Digit.SystemFont.FontRotate.X =
                                            (int)Math.Round(digitalDateDigit.Digit.SystemFont.FontRotate.X *
                                            scale, MidpointRounding.AwayFromZero);
                                        digitalDateDigit.Digit.SystemFont.FontRotate.Y =
                                            (int)Math.Round(digitalDateDigit.Digit.SystemFont.FontRotate.Y *
                                            scale, MidpointRounding.AwayFromZero);
                                    }
                                }
                            }
                            if (digitalDateDigit.Separator != null && digitalDateDigit.Separator.Coordinates != null)
                            {
                                digitalDateDigit.Separator.Coordinates.X =
                                    (int)Math.Round(digitalDateDigit.Separator.Coordinates.X *
                                    scale, MidpointRounding.AwayFromZero);
                                digitalDateDigit.Separator.Coordinates.Y =
                                    (int)Math.Round(digitalDateDigit.Separator.Coordinates.Y *
                                    scale, MidpointRounding.AwayFromZero);
                            }
                        }
                    }

                    if (Watch_Face.ScreenIdle.Date.WeeksDigits != null)
                    {
                        if (Watch_Face.ScreenIdle.Date.WeeksDigits.Digit != null) Watch_Face.ScreenIdle.Date.WeeksDigits.Digit =
                                Digits_Scale(Watch_Face.ScreenIdle.Date.WeeksDigits.Digit, scale);

                        if (Watch_Face.ScreenIdle.Date.WeeksDigits.Separator != null &&
                            Watch_Face.ScreenIdle.Date.WeeksDigits.Separator.Coordinates != null)
                        {
                            Watch_Face.ScreenIdle.Date.WeeksDigits.Separator.Coordinates.X =
                                (int)Math.Round(Watch_Face.ScreenIdle.Date.WeeksDigits.Separator.Coordinates.X *
                                scale, MidpointRounding.AwayFromZero);
                            Watch_Face.ScreenIdle.Date.WeeksDigits.Separator.Coordinates.Y =
                                (int)Math.Round(Watch_Face.ScreenIdle.Date.WeeksDigits.Separator.Coordinates.Y *
                                scale, MidpointRounding.AwayFromZero);
                        }
                    }
                }
                #endregion

                #region Activity 
                if (Watch_Face.System != null && Watch_Face.ScreenIdle.Activity != null)
                {
                    foreach (Activity activity in Watch_Face.ScreenIdle.Activity)
                    {
                        Activity_Scale(activity, scale);
                    }
                }
                #endregion
            }
        }

        private ClockHand ClockHand_Scale(ClockHand clockHand, float scale)
        {
            if (clockHand.Cover != null && clockHand.Cover.Coordinates != null)
            {
                clockHand.Cover.Coordinates.X =
                    (int)Math.Round(clockHand.Cover.Coordinates.X * scale, MidpointRounding.AwayFromZero);
                clockHand.Cover.Coordinates.Y =
                    (int)Math.Round(clockHand.Cover.Coordinates.Y * scale, MidpointRounding.AwayFromZero);
            }

            if (clockHand.Pointer != null && clockHand.Pointer.Coordinates != null)
            {
                clockHand.Pointer.Coordinates.X =
                    (int)Math.Round(clockHand.Pointer.Coordinates.X * scale, MidpointRounding.AwayFromZero);
                clockHand.Pointer.Coordinates.Y =
                    (int)Math.Round(clockHand.Pointer.Coordinates.Y * scale, MidpointRounding.AwayFromZero);
            }

            if (clockHand.Scale != null  && clockHand.Scale.Coordinates != null)
            {
                clockHand.Scale.Coordinates.X =
                    (int)Math.Round(clockHand.Scale.Coordinates.X * scale, MidpointRounding.AwayFromZero);
                clockHand.Scale.Coordinates.Y =
                    (int)Math.Round(clockHand.Scale.Coordinates.Y * scale, MidpointRounding.AwayFromZero);
            }

            clockHand.X = (int)Math.Round(clockHand.X * scale, MidpointRounding.AwayFromZero);
            clockHand.Y = (int)Math.Round(clockHand.Y * scale, MidpointRounding.AwayFromZero);

            return clockHand;
        }

        private Text Digits_Scale(Text digit, float scale)
        {
            if (digit != null)
            {
                if (digit.Image != null)
                {
                    //if (digit.Image.DecimalPointImageIndex != null)
                    //    digit.Image.DecimalPointImageIndex =
                    //        (int)Math.Round((int)digit.Image.DecimalPointImageIndex *
                    //        scale, MidpointRounding.AwayFromZero);
                    //if (digit.Image.DelimiterImageIndex != null)
                    //    digit.Image.DelimiterImageIndex =
                    //        (int)Math.Round((int)digit.Image.DelimiterImageIndex *
                    //        scale, MidpointRounding.AwayFromZero);
                    //if (digit.Image.NoDataImageIndex != null)
                    //    digit.Image.NoDataImageIndex =
                    //        (int)Math.Round((int)digit.Image.NoDataImageIndex *
                    //        scale, MidpointRounding.AwayFromZero);

                    digit.Image.X =
                        (int)Math.Round(digit.Image.X * scale, MidpointRounding.AwayFromZero);
                    digit.Image.Y =
                        (int)Math.Round(digit.Image.Y * scale, MidpointRounding.AwayFromZero);

                }

                if (digit.Spacing != null)
                    digit.Spacing =
                        (int)Math.Round((int)digit.Spacing * scale, MidpointRounding.AwayFromZero);

                if (digit.SystemFont != null)
                {
                    digit.SystemFont.Size =
                        (int)Math.Round(digit.SystemFont.Size * scale, MidpointRounding.AwayFromZero);

                    if (digit.SystemFont.Coordinates != null)
                    {
                        digit.SystemFont.Coordinates.X =
                            (int)Math.Round(digit.SystemFont.Coordinates.X *
                            scale, MidpointRounding.AwayFromZero);
                        digit.SystemFont.Coordinates.Y =
                            (int)Math.Round(digit.SystemFont.Coordinates.Y *
                            scale, MidpointRounding.AwayFromZero);
                    }

                    if (digit.SystemFont.FontRotate != null)
                    {
                        digit.SystemFont.FontRotate.Radius =
                            (int)Math.Round(digit.SystemFont.FontRotate.Radius *
                            scale, MidpointRounding.AwayFromZero);
                        digit.SystemFont.FontRotate.X =
                            (int)Math.Round(digit.SystemFont.FontRotate.X *
                            scale, MidpointRounding.AwayFromZero);
                        digit.SystemFont.FontRotate.Y =
                            (int)Math.Round(digit.SystemFont.FontRotate.Y *
                            scale, MidpointRounding.AwayFromZero);
                    }
                }
            }

            return digit;
        }

        private AngleSettings AngleSettings_Scale(AngleSettings angleSettings, float scale)
        {
            angleSettings.Radius = (int)Math.Round(angleSettings.Radius * scale, MidpointRounding.AwayFromZero);

            angleSettings.X = (int)Math.Round(angleSettings.X * scale, MidpointRounding.AwayFromZero);
            angleSettings.Y = (int)Math.Round(angleSettings.Y * scale, MidpointRounding.AwayFromZero);
            return angleSettings;
        }

        private LinearSettings LinearSettings_Scale(LinearSettings linearSettings, float scale)
        {
            linearSettings.StartX = (int)Math.Round(linearSettings.StartX * scale, MidpointRounding.AwayFromZero);
            linearSettings.StartY = (int)Math.Round(linearSettings.StartY * scale, MidpointRounding.AwayFromZero);
            linearSettings.EndX = (int)Math.Round(linearSettings.EndX * scale, MidpointRounding.AwayFromZero);
            linearSettings.EndY = (int)Math.Round(linearSettings.EndY * scale, MidpointRounding.AwayFromZero);
            return linearSettings;
        }

        private void Activity_Scale(Activity activity, float scale)
        {
            if (activity.Digits != null && activity.Digits.Count > 0)
            {
                foreach (DigitalCommonDigit digitalCommonDigit in activity.Digits)
                {
                    if (digitalCommonDigit.Digit != null) digitalCommonDigit.Digit =
                            Digits_Scale(digitalCommonDigit.Digit, scale);

                    if (digitalCommonDigit.Separator != null && digitalCommonDigit.Separator.Coordinates != null)
                    {
                        digitalCommonDigit.Separator.Coordinates.X =
                            (int)Math.Round(digitalCommonDigit.Separator.Coordinates.X * scale, MidpointRounding.AwayFromZero);
                        digitalCommonDigit.Separator.Coordinates.Y =
                            (int)Math.Round(digitalCommonDigit.Separator.Coordinates.Y * scale, MidpointRounding.AwayFromZero);
                    }
                }
            }

            if (activity.Icon != null && activity.Icon.Coordinates != null)
            {
                activity.Icon.Coordinates.X =
                            (int)Math.Round(activity.Icon.Coordinates.X * scale, MidpointRounding.AwayFromZero);
                activity.Icon.Coordinates.Y =
                    (int)Math.Round(activity.Icon.Coordinates.Y * scale, MidpointRounding.AwayFromZero);
            }

            if (activity.ImageProgress != null && activity.ImageProgress.Coordinates != null)
            {
                foreach (Coordinates coordinate in activity.ImageProgress.Coordinates)
                {
                    coordinate.X = (int)Math.Round(coordinate.X * scale, MidpointRounding.AwayFromZero);
                    coordinate.Y = (int)Math.Round(coordinate.Y * scale, MidpointRounding.AwayFromZero);
                }
            }

            if (activity.PointerProgress != null)
                activity.PointerProgress = ClockHand_Scale(activity.PointerProgress, scale);

            if (activity.ProgressBar != null)
            {
                if (activity.ProgressBar.AngleSettings != null)
                    activity.ProgressBar.AngleSettings = AngleSettings_Scale(activity.ProgressBar.AngleSettings, scale);

                if (activity.ProgressBar.LinearSettings != null)
                    activity.ProgressBar.LinearSettings = LinearSettings_Scale(activity.ProgressBar.LinearSettings, scale);

                activity.ProgressBar.Width =
                    (int)Math.Round(activity.ProgressBar.Width * scale, MidpointRounding.AwayFromZero);
            }

            if (activity.Shortcut != null && activity.Shortcut.BoxElement != null)
            {
                activity.Shortcut.BoxElement.TopLeftX =
                    (int)Math.Round(activity.Shortcut.BoxElement.TopLeftX * scale, MidpointRounding.AwayFromZero);
                activity.Shortcut.BoxElement.TopLeftY =
                    (int)Math.Round(activity.Shortcut.BoxElement.TopLeftY * scale, MidpointRounding.AwayFromZero);
                activity.Shortcut.BoxElement.Height =
                    (int)Math.Round(activity.Shortcut.BoxElement.Height * scale, MidpointRounding.AwayFromZero);
                activity.Shortcut.BoxElement.Width =
                    (int)Math.Round(activity.Shortcut.BoxElement.Width * scale, MidpointRounding.AwayFromZero);
            }
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
                if (radioButton_TRex_pro.Checked)
                {
                    bitmap = new Bitmap(Convert.ToInt32(360), Convert.ToInt32(360), PixelFormat.Format32bppArgb);
                    mask = new Bitmap(Application.StartupPath + @"\Mask\mask_trex_pro.png");
                    PreviewHeight = 220;
                }
                if (radioButton_ZeppE.Checked)
                {
                    bitmap = new Bitmap(Convert.ToInt32(416), Convert.ToInt32(416), PixelFormat.Format32bppArgb);
                    mask = new Bitmap(Application.StartupPath + @"\Mask\mask_zepp_e.png");
                    PreviewHeight = 280;
                }
                Graphics gPanel = Graphics.FromImage(bitmap);
                int link = radioButton_ScreenNormal.Checked ? 0 : 1;
                PreviewToBitmap(gPanel, 1.0f, false, false, false, false, false, false, false, true, false, false, false, link);
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
                        Properties.FormStrings.Message_Warning_Caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (ResultDialog == DialogResult.Yes) scale = (float)loadedImage.Height / bitmap.Height;
                }
                int pixelsOld = loadedImage.Width * loadedImage.Height;
                pixelsOld = pixelsOld * 4 + 20;
                bitmap = ResizeImage(bitmap, scale);
                //bitmap.Save(ListImagesFullName[i], ImageFormat.Png);

                MagickImage optimizedBitmap = new MagickImage(bitmap);

                QuantizeSettings settings = new QuantizeSettings();
                settings.Colors = 256;
                optimizedBitmap.Quantize(settings);

                // Optionally optimize the images (images should have the same size).

                optimizedBitmap.Format = MagickFormat.Png;
                optimizedBitmap.Write(ListImagesFullName[i]);



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
                if (radioButton_TRex_pro.Checked)
                {
                    bitmap = new Bitmap(Convert.ToInt32(360), Convert.ToInt32(360), PixelFormat.Format32bppArgb);
                    mask = new Bitmap(Application.StartupPath + @"\Mask\mask_trex_pro.png");
                    PreviewHeight = 220;
                }
                if (radioButton_ZeppE.Checked)
                {
                    bitmap = new Bitmap(Convert.ToInt32(416), Convert.ToInt32(416), PixelFormat.Format32bppArgb);
                    mask = new Bitmap(Application.StartupPath + @"\Mask\mask_zepp_e.png");
                    PreviewHeight = 280;
                }
                Graphics gPanel = Graphics.FromImage(bitmap);
                int link = radioButton_ScreenNormal.Checked ? 0 : 1;
                PreviewToBitmap(gPanel, 1.0f, false, false, false, false, false, false, false, true, false, false, false, link);
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
                    //var childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(property.Name + "*+*"))];
                    //childNode.Tag = property;
                    if (property.Value is JValue)
                    {
                        var childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(property.Name + ":" + property.Value.ToString()))];
                        childNode.Tag = property;
                    }
                    else
                    {
                        var childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(property.Name))];
                        childNode.Tag = property;
                        AddNode(property.Value, childNode); 
                    }
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
        #endregion

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

            if (checkBox_UseID.Checked)
            {
                if (Watch_Face.Info == null) Watch_Face.Info = new Device_Id();
                int ID = 0;
                Int32.TryParse(textBox_WatchfaceID.Text, out ID);
                if (ID >= 1000)
                {
                    Watch_Face.Info.WatchFaceId = ID;

                    richTextBox_JsonText.Text = JsonConvert.SerializeObject(Watch_Face, Formatting.Indented, new JsonSerializerSettings
                    {
                        //DefaultValueHandling = DefaultValueHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    JsonToTree(richTextBox_JsonText.Text);
                }
            }
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
                    ID = rndID;
                    textBox_WatchfaceID.Text = rndID.ToString();
                    JSON_Modified = true;
                    FormText();
                }
                if (Watch_Face != null && Watch_Face.Info != null)
                {
                    Watch_Face.Info.WatchFaceId = ID;

                    richTextBox_JsonText.Text = JsonConvert.SerializeObject(Watch_Face, Formatting.Indented, new JsonSerializerSettings
                    {
                        //DefaultValueHandling = DefaultValueHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    JsonToTree(richTextBox_JsonText.Text);
                }
            }
            else
            {
                if (Watch_Face != null && Watch_Face.Info != null)
                {
                    Watch_Face.Info.WatchFaceId = null;

                    richTextBox_JsonText.Text = JsonConvert.SerializeObject(Watch_Face, Formatting.Indented, new JsonSerializerSettings
                    {
                        //DefaultValueHandling = DefaultValueHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    JsonToTree(richTextBox_JsonText.Text);
                }
            }
        }

        private void checkBox_pictures_Use_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            Control control = checkBox.Parent;
            Control.ControlCollection controlCollection = control.Controls;

            bool b = checkBox.Checked;
            for (int i = 1; i < controlCollection.Count; i++)
            {
                controlCollection[i].Enabled = b;
            }
        }
        private void checkBox_pictures_Use_AOD_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            Control control = checkBox.Parent;
            Control.ControlCollection controlCollection = control.Controls;

            bool b = checkBox.Checked;
            for (int i = 1; i < controlCollection.Count - 1; i++)
            {
                controlCollection[i].Enabled = b;
            }
        }

        private void checkBox_text_Use_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            Control control = checkBox.Parent;
            Control.ControlCollection controlCollection = control.Controls;

            bool b = checkBox.Checked;
            for (int i = 1; i < controlCollection.Count; i++)
            {
                controlCollection[i].Enabled = b;
            }
        }
        private void checkBox_text_Use_AOD_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            Control control = checkBox.Parent;
            Control.ControlCollection controlCollection = control.Controls;

            bool b = checkBox.Checked;
            for (int i = 1; i < controlCollection.Count - 1; i++)
            {
                controlCollection[i].Enabled = b;
            }
        }

        private void checkBox_hand_Use_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            Control control = checkBox.Parent;
            Control.ControlCollection controlCollection = control.Controls;

            bool b = checkBox.Checked;
            for (int i = 1; i < controlCollection.Count; i++)
            {
                controlCollection[i].Enabled = b;
            }
        }
        private void checkBox_hand_Use_AOD_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            Control control = checkBox.Parent;
            Control.ControlCollection controlCollection = control.Controls;

            bool b = checkBox.Checked;
            for (int i = 1; i < controlCollection.Count - 1; i++)
            {
                controlCollection[i].Enabled = b;
            }
        }

        private void checkBox_scaleCircle_Use_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            Control control = checkBox.Parent;
            Control.ControlCollection controlCollection = control.Controls;

            bool b = checkBox.Checked;
            RadioButton radioButton = (RadioButton)controlCollection[1];
            bool bImage = radioButton.Checked;
            for (int i = 1; i < controlCollection.Count; i++)
            {
                controlCollection[i].Enabled = b;
                if (b)
                {
                    if (i == 3) controlCollection[i].Enabled = bImage;
                    if (i == 4) controlCollection[i].Enabled = !bImage;
                }
            }
        }
        private void checkBox_scaleCircle_Use_AOD_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            Control control = checkBox.Parent;
            Control.ControlCollection controlCollection = control.Controls;

            bool b = checkBox.Checked;
            RadioButton radioButton = (RadioButton)controlCollection[1];
            bool bImage = radioButton.Checked;
            for (int i = 1; i < controlCollection.Count - 1; i++)
            {
                controlCollection[i].Enabled = b;
                if (b)
                {
                    if (i == 3) controlCollection[i].Enabled = bImage;
                    if (i == 4) controlCollection[i].Enabled = !bImage;
                }
            }
        }

        private void checkBox_scaleLinear_Use_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            Control control = checkBox.Parent;
            Control.ControlCollection controlCollection = control.Controls;

            bool b = checkBox.Checked;
            RadioButton radioButton = (RadioButton)controlCollection[1];
            bool bImage = radioButton.Checked;
            for (int i = 1; i < controlCollection.Count; i++)
            {
                controlCollection[i].Enabled = b;
                if (b)
                {
                    if (i == 3) controlCollection[i].Enabled = bImage;
                    if (i == 4) controlCollection[i].Enabled = !bImage;
                }
            }
        }
        private void checkBox_scaleLinear_Use_AOD_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            Control control = checkBox.Parent;
            Control.ControlCollection controlCollection = control.Controls;

            bool b = checkBox.Checked;
            RadioButton radioButton = (RadioButton)controlCollection[1];
            bool bImage = radioButton.Checked;
            for (int i = 1; i < controlCollection.Count - 1; i++)
            {
                controlCollection[i].Enabled = b;
                if (b)
                {
                    if (i == 3) controlCollection[i].Enabled = bImage;
                    if (i == 4) controlCollection[i].Enabled = !bImage;
                }
            }
        }

        private void radioButton_image_color_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            Control control = radioButton.Parent;
            Control.ControlCollection controlCollection = control.Controls;

            bool b = radioButton.Checked;
            controlCollection[3].Enabled = b;
            controlCollection[4].Enabled = !b;

            JSON_write();
            PreviewImage();
        }

        private void userControl_Set1_Collapse(object sender, EventArgs eventArgs, int setNumber)
        {
            SetPreferences(userControl_Set1);
            PreviewImage();
            userControl_Set2.Collapsed = true;
            userControl_Set3.Collapsed = true;
            userControl_Set4.Collapsed = true;
            userControl_Set5.Collapsed = true;
            userControl_Set6.Collapsed = true;
            userControl_Set7.Collapsed = true;
            userControl_Set8.Collapsed = true;
            userControl_Set9.Collapsed = true;
            userControl_Set10.Collapsed = true;
            userControl_Set11.Collapsed = true;
            userControl_Set12.Collapsed = true;
            Watch_Face_Preview_Set.SetNumber = 1;
        }

        private void userControl_Set2_Collapse(object sender, EventArgs eventArgs, int setNumber)
        {
            SetPreferences(userControl_Set2);
            PreviewImage();
            userControl_Set1.Collapsed = true;
            userControl_Set3.Collapsed = true;
            userControl_Set4.Collapsed = true;
            userControl_Set5.Collapsed = true;
            userControl_Set6.Collapsed = true;
            userControl_Set7.Collapsed = true;
            userControl_Set8.Collapsed = true;
            userControl_Set9.Collapsed = true;
            userControl_Set10.Collapsed = true;
            userControl_Set11.Collapsed = true;
            userControl_Set12.Collapsed = true;
            Watch_Face_Preview_Set.SetNumber = 2;
        }

        private void userControl_Set3_Collapse(object sender, EventArgs eventArgs, int setNumber)
        {
            SetPreferences(userControl_Set3);
            PreviewImage();
            userControl_Set1.Collapsed = true;
            userControl_Set2.Collapsed = true;
            userControl_Set4.Collapsed = true;
            userControl_Set5.Collapsed = true;
            userControl_Set6.Collapsed = true;
            userControl_Set7.Collapsed = true;
            userControl_Set8.Collapsed = true;
            userControl_Set9.Collapsed = true;
            userControl_Set10.Collapsed = true;
            userControl_Set11.Collapsed = true;
            userControl_Set12.Collapsed = true;
            Watch_Face_Preview_Set.SetNumber = 3;
        }

        private void userControl_Set4_Collapse(object sender, EventArgs eventArgs, int setNumber)
        {
            SetPreferences(userControl_Set4);
            PreviewImage();
            userControl_Set1.Collapsed = true;
            userControl_Set2.Collapsed = true;
            userControl_Set3.Collapsed = true;
            userControl_Set5.Collapsed = true;
            userControl_Set6.Collapsed = true;
            userControl_Set7.Collapsed = true;
            userControl_Set8.Collapsed = true;
            userControl_Set9.Collapsed = true;
            userControl_Set10.Collapsed = true;
            userControl_Set11.Collapsed = true;
            userControl_Set12.Collapsed = true;
            Watch_Face_Preview_Set.SetNumber = 4;
        }

        private void userControl_Set5_Collapse(object sender, EventArgs eventArgs, int setNumber)
        {
            SetPreferences(userControl_Set5);
            PreviewImage();
            userControl_Set1.Collapsed = true;
            userControl_Set2.Collapsed = true;
            userControl_Set3.Collapsed = true;
            userControl_Set4.Collapsed = true;
            userControl_Set6.Collapsed = true;
            userControl_Set7.Collapsed = true;
            userControl_Set8.Collapsed = true;
            userControl_Set9.Collapsed = true;
            userControl_Set10.Collapsed = true;
            userControl_Set11.Collapsed = true;
            userControl_Set12.Collapsed = true;
            Watch_Face_Preview_Set.SetNumber = 5;
        }

        private void userControl_Set6_Collapse(object sender, EventArgs eventArgs, int setNumber)
        {
            SetPreferences(userControl_Set6);
            PreviewImage();
            userControl_Set1.Collapsed = true;
            userControl_Set2.Collapsed = true;
            userControl_Set3.Collapsed = true;
            userControl_Set4.Collapsed = true;
            userControl_Set5.Collapsed = true;
            userControl_Set7.Collapsed = true;
            userControl_Set8.Collapsed = true;
            userControl_Set9.Collapsed = true;
            userControl_Set10.Collapsed = true;
            userControl_Set11.Collapsed = true;
            userControl_Set12.Collapsed = true;
            Watch_Face_Preview_Set.SetNumber = 6;
        }

        private void userControl_Set7_Collapse(object sender, EventArgs eventArgs, int setNumber)
        {
            SetPreferences(userControl_Set7);
            PreviewImage();
            userControl_Set1.Collapsed = true;
            userControl_Set2.Collapsed = true;
            userControl_Set3.Collapsed = true;
            userControl_Set4.Collapsed = true;
            userControl_Set5.Collapsed = true;
            userControl_Set6.Collapsed = true;
            userControl_Set8.Collapsed = true;
            userControl_Set9.Collapsed = true;
            userControl_Set10.Collapsed = true;
            userControl_Set11.Collapsed = true;
            userControl_Set12.Collapsed = true;
            Watch_Face_Preview_Set.SetNumber = 7;
        }

        private void userControl_Set8_Collapse(object sender, EventArgs eventArgs, int setNumber)
        {
            SetPreferences(userControl_Set8);
            PreviewImage();
            userControl_Set1.Collapsed = true;
            userControl_Set2.Collapsed = true;
            userControl_Set3.Collapsed = true;
            userControl_Set4.Collapsed = true;
            userControl_Set5.Collapsed = true;
            userControl_Set6.Collapsed = true;
            userControl_Set7.Collapsed = true;
            userControl_Set9.Collapsed = true;
            userControl_Set10.Collapsed = true;
            userControl_Set11.Collapsed = true;
            userControl_Set12.Collapsed = true;
            Watch_Face_Preview_Set.SetNumber = 8;
        }

        private void userControl_Set9_Collapse(object sender, EventArgs eventArgs, int setNumber)
        {
            SetPreferences(userControl_Set9);
            PreviewImage();
            userControl_Set1.Collapsed = true;
            userControl_Set2.Collapsed = true;
            userControl_Set3.Collapsed = true;
            userControl_Set4.Collapsed = true;
            userControl_Set5.Collapsed = true;
            userControl_Set6.Collapsed = true;
            userControl_Set7.Collapsed = true;
            userControl_Set8.Collapsed = true;
            userControl_Set10.Collapsed = true;
            userControl_Set11.Collapsed = true;
            userControl_Set12.Collapsed = true;
            Watch_Face_Preview_Set.SetNumber = 9;
        }

        private void userControl_Set10_Collapse(object sender, EventArgs eventArgs, int setNumber)
        {
            SetPreferences(userControl_Set10);
            PreviewImage();
            userControl_Set1.Collapsed = true;
            userControl_Set2.Collapsed = true;
            userControl_Set3.Collapsed = true;
            userControl_Set4.Collapsed = true;
            userControl_Set5.Collapsed = true;
            userControl_Set6.Collapsed = true;
            userControl_Set7.Collapsed = true;
            userControl_Set8.Collapsed = true;
            userControl_Set9.Collapsed = true;
            userControl_Set11.Collapsed = true;
            userControl_Set12.Collapsed = true;
            Watch_Face_Preview_Set.SetNumber = 10;
        }

        private void userControl_Set11_Collapse(object sender, EventArgs eventArgs, int setNumber)
        {
            SetPreferences(userControl_Set11);
            PreviewImage();
            userControl_Set1.Collapsed = true;
            userControl_Set2.Collapsed = true;
            userControl_Set3.Collapsed = true;
            userControl_Set4.Collapsed = true;
            userControl_Set5.Collapsed = true;
            userControl_Set6.Collapsed = true;
            userControl_Set7.Collapsed = true;
            userControl_Set8.Collapsed = true;
            userControl_Set9.Collapsed = true;
            userControl_Set10.Collapsed = true;
            userControl_Set12.Collapsed = true;
            Watch_Face_Preview_Set.SetNumber = 11;
        }

        private void userControl_Set12_Collapse(object sender, EventArgs eventArgs, int setNumber)
        {
            SetPreferences(userControl_Set12);
            PreviewImage();
            userControl_Set1.Collapsed = true;
            userControl_Set2.Collapsed = true;
            userControl_Set3.Collapsed = true;
            userControl_Set4.Collapsed = true;
            userControl_Set5.Collapsed = true;
            userControl_Set6.Collapsed = true;
            userControl_Set7.Collapsed = true;
            userControl_Set8.Collapsed = true;
            userControl_Set9.Collapsed = true;
            userControl_Set10.Collapsed = true;
            userControl_Set11.Collapsed = true;
            Watch_Face_Preview_Set.SetNumber = 12;
        }

        private void numericUpDown_Gif_Speed_ValueChanged_1(object sender, EventArgs e)
        {
            Program_Settings.Gif_Speed = (float)numericUpDown_Gif_Speed.Value;
            string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);
        }

        private void userControl_ValueChanged(object sender, EventArgs eventArgs)
        {
            JSON_write();
            PreviewImage();
        }

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

        private void radioButton_Background_image_CheckedChanged(object sender, EventArgs e)
        {
            bool b = radioButton_Background_image.Checked;
            comboBox_Background_image.Enabled = b;
            comboBox_Background_color.Enabled = !b;
            JSON_write();
            PreviewImage();
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


        private void checkBox_Hour_Use_AOD_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Hour_Use_AOD.Checked;
            comboBox_Hour_image_AOD.Enabled = b;
            comboBox_Hour_unit_AOD.Enabled = b;
            comboBox_Hour_separator_AOD.Enabled = b;
            numericUpDown_HourX_AOD.Enabled = b;
            numericUpDown_HourY_AOD.Enabled = b;
            numericUpDown_Hour_unitX_AOD.Enabled = b;
            numericUpDown_Hour_unitY_AOD.Enabled = b;
            comboBox_Hour_alignment_AOD.Enabled = b;
            checkBox_Hour_add_zero_AOD.Enabled = b;
            numericUpDown_Hour_spacing_AOD.Enabled = b;

            label317.Enabled = b;
            label318.Enabled = b;
            label319.Enabled = b;
            label320.Enabled = b;
            label321.Enabled = b;
            label322.Enabled = b;
            label323.Enabled = b;
            label324.Enabled = b;
            label325.Enabled = b;
            label326.Enabled = b;
            label327.Enabled = b;
        }

        private void checkBox_Minute_Use_AOD_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Minute_Use_AOD.Checked;
            comboBox_Minute_image_AOD.Enabled = b;
            comboBox_Minute_unit_AOD.Enabled = b;
            comboBox_Minute_separator_AOD.Enabled = b;
            numericUpDown_MinuteX_AOD.Enabled = b;
            numericUpDown_MinuteY_AOD.Enabled = b;
            numericUpDown_Minute_unitX_AOD.Enabled = b;
            numericUpDown_Minute_unitY_AOD.Enabled = b;
            comboBox_Minute_alignment_AOD.Enabled = b;
            checkBox_Minute_add_zero_AOD.Enabled = b;
            numericUpDown_Minute_spacing_AOD.Enabled = b;
            checkBox_Minute_follow_AOD.Enabled = b;

            label306.Enabled = b;
            label307.Enabled = b;
            label308.Enabled = b;
            label309.Enabled = b;
            label310.Enabled = b;
            label311.Enabled = b;
            label312.Enabled = b;
            label313.Enabled = b;
            label314.Enabled = b;
            label315.Enabled = b;
            label316.Enabled = b;

            if (checkBox_Minute_follow_AOD.Checked)
            {
                numericUpDown_MinuteX_AOD.Enabled = false;
                numericUpDown_MinuteY_AOD.Enabled = false;

                label306.Enabled = false;
                label309.Enabled = false;
                label311.Enabled = false;
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


        private void checkBox_Hour_hand_Use_AOD_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Hour_hand_Use_AOD.Checked;
            comboBox_Hour_hand_image_AOD.Enabled = b;
            comboBox_Hour_hand_imageCentr_AOD.Enabled = b;
            numericUpDown_Hour_handX_AOD.Enabled = b;
            numericUpDown_Hour_handY_AOD.Enabled = b;
            numericUpDown_Hour_handX_centr_AOD.Enabled = b;
            numericUpDown_Hour_handY_centr_AOD.Enabled = b;
            numericUpDown_Hour_handX_offset_AOD.Enabled = b;
            numericUpDown_Hour_handY_offset_AOD.Enabled = b;

            label350.Enabled = b;
            label351.Enabled = b;
            label352.Enabled = b;
            label353.Enabled = b;
            label354.Enabled = b;
            label357.Enabled = b;
            label358.Enabled = b;
            label359.Enabled = b;
            label360.Enabled = b;
            label361.Enabled = b;
            label362.Enabled = b;
        }

        private void checkBox_Minute_hand_Use_AOD_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Minute_hand_Use_AOD.Checked;
            comboBox_Minute_hand_image_AOD.Enabled = b;
            comboBox_Minute_hand_imageCentr_AOD.Enabled = b;
            numericUpDown_Minute_handX_AOD.Enabled = b;
            numericUpDown_Minute_handY_AOD.Enabled = b;
            numericUpDown_Minute_handX_centr_AOD.Enabled = b;
            numericUpDown_Minute_handY_centr_AOD.Enabled = b;
            numericUpDown_Minute_handX_offset_AOD.Enabled = b;
            numericUpDown_Minute_handY_offset_AOD.Enabled = b;

            label339.Enabled = b;
            label340.Enabled = b;
            label341.Enabled = b;
            label342.Enabled = b;
            label343.Enabled = b;
            label344.Enabled = b;
            label345.Enabled = b;
            label346.Enabled = b;
            label347.Enabled = b;
            label348.Enabled = b;
            label349.Enabled = b;
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

        private void checkBox_12h_Use_AOD_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_12h_Use_AOD.Checked;
            comboBox_AM_image_AOD.Enabled = b;
            comboBox_PM_image_AOD.Enabled = b;
            numericUpDown_AM_X_AOD.Enabled = b;
            numericUpDown_AM_Y_AOD.Enabled = b;
            numericUpDown_PM_X_AOD.Enabled = b;
            numericUpDown_PM_Y_AOD.Enabled = b;

            label285.Enabled = b;
            label286.Enabled = b;
            label287.Enabled = b;
            label289.Enabled = b;
            label290.Enabled = b;
            label291.Enabled = b;
            label292.Enabled = b;
            label293.Enabled = b;
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

        private void checkBox_Day_hand_Use_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Day_hand_Use.Checked;
            comboBox_Day_hand_image.Enabled = b;
            comboBox_Day_hand_imageCentr.Enabled = b;
            comboBox_Day_hand_imageBackground.Enabled = b;
            numericUpDown_Day_handX.Enabled = b;
            numericUpDown_Day_handY.Enabled = b;
            numericUpDown_Day_handX_centr.Enabled = b;
            numericUpDown_Day_handY_centr.Enabled = b;
            numericUpDown_Day_handX_background.Enabled = b;
            numericUpDown_Day_handY_background.Enabled = b;
            numericUpDown_Day_handX_offset.Enabled = b;
            numericUpDown_Day_handY_offset.Enabled = b;
            numericUpDown_Day_hand_startAngle.Enabled = b;
            numericUpDown_Day_hand_endAngle.Enabled = b;

            label609.Enabled = b;
            label610.Enabled = b;
            label611.Enabled = b;
            label612.Enabled = b;
            //label613.Enabled = b;
            //label614.Enabled = b;
            //label615.Enabled = b;
            //label616.Enabled = b;
            label617.Enabled = b;
            label618.Enabled = b;
            label619.Enabled = b;
            label620.Enabled = b;
            label621.Enabled = b;
            label622.Enabled = b;
            label623.Enabled = b;
            label624.Enabled = b;
            label625.Enabled = b;

            label714.Enabled = b;
            label715.Enabled = b;
            label716.Enabled = b;
            label717.Enabled = b;
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

        private void checkBox_Month_hand_Use_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Month_hand_Use.Checked;
            comboBox_Month_hand_image.Enabled = b;
            comboBox_Month_hand_imageCentr.Enabled = b;
            comboBox_Month_hand_imageBackground.Enabled = b;
            numericUpDown_Month_handX.Enabled = b;
            numericUpDown_Month_handY.Enabled = b;
            numericUpDown_Month_handX_centr.Enabled = b;
            numericUpDown_Month_handY_centr.Enabled = b;
            numericUpDown_Month_handX_background.Enabled = b;
            numericUpDown_Month_handY_background.Enabled = b;
            numericUpDown_Month_handX_offset.Enabled = b;
            numericUpDown_Month_handY_offset.Enabled = b;
            numericUpDown_Month_hand_startAngle.Enabled = b;
            numericUpDown_Month_hand_endAngle.Enabled = b;

            label626.Enabled = b;
            label627.Enabled = b;
            label628.Enabled = b;
            label629.Enabled = b;
            label630.Enabled = b;
            label631.Enabled = b;
            label632.Enabled = b;
            label633.Enabled = b;
            label634.Enabled = b;
            label635.Enabled = b;
            label636.Enabled = b;
            label637.Enabled = b;
            label638.Enabled = b;

            label706.Enabled = b;
            label707.Enabled = b;
            label708.Enabled = b;
            label709.Enabled = b;
        }

        private void checkBox_Year_Use_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Year_text_Use.Checked;
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
            checkBox_Year_follow.Enabled = b;

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

        private void checkBox_DOW_pictures_Use_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_DOW_pictures_Use.Checked;
            comboBox_DOW_pictures_image.Enabled = b;
            numericUpDown_DOW_picturesX.Enabled = b;
            numericUpDown_DOW_picturesY.Enabled = b;

            label639.Enabled = b;
            label640.Enabled = b;
            label641.Enabled = b;
            label642.Enabled = b;
        }

        private void checkBox_DOW_hand_Use_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_DOW_hand_Use.Checked;
            comboBox_DOW_hand_image.Enabled = b;
            comboBox_DOW_hand_imageCentr.Enabled = b;
            comboBox_DOW_hand_imageBackground.Enabled = b;
            numericUpDown_DOW_handX.Enabled = b;
            numericUpDown_DOW_handY.Enabled = b;
            numericUpDown_DOW_handX_centr.Enabled = b;
            numericUpDown_DOW_handY_centr.Enabled = b;
            numericUpDown_DOW_handX_background.Enabled = b;
            numericUpDown_DOW_handY_background.Enabled = b;
            numericUpDown_DOW_handX_offset.Enabled = b;
            numericUpDown_DOW_handY_offset.Enabled = b;
            numericUpDown_DOW_hand_startAngle.Enabled = b;
            numericUpDown_DOW_hand_endAngle.Enabled = b;

            label643.Enabled = b;
            label644.Enabled = b;
            label645.Enabled = b;
            label646.Enabled = b;
            label647.Enabled = b;
            label648.Enabled = b;
            label649.Enabled = b;
            label650.Enabled = b;
            label651.Enabled = b;
            label652.Enabled = b;
            label653.Enabled = b;
            label654.Enabled = b;
            label655.Enabled = b;

            label710.Enabled = b;
            label711.Enabled = b;
            label712.Enabled = b;
            label713.Enabled = b;
        }




        private void checkBox_Day_Use_AOD_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Day_Use_AOD.Checked;
            comboBox_Day_image_AOD.Enabled = b;
            comboBox_Day_unit_AOD.Enabled = b;
            comboBox_Day_separator_AOD.Enabled = b;
            numericUpDown_DayX_AOD.Enabled = b;
            numericUpDown_DayY_AOD.Enabled = b;
            numericUpDown_Day_unitX_AOD.Enabled = b;
            numericUpDown_Day_unitY_AOD.Enabled = b;
            comboBox_Day_alignment_AOD.Enabled = b;
            checkBox_Day_add_zero_AOD.Enabled = b;
            numericUpDown_Day_spacing_AOD.Enabled = b;
            checkBox_Day_follow_AOD.Enabled = b;

            label380.Enabled = b;
            label381.Enabled = b;
            label382.Enabled = b;
            label383.Enabled = b;
            label384.Enabled = b;
            label385.Enabled = b;
            label386.Enabled = b;
            label387.Enabled = b;
            label388.Enabled = b;
            label389.Enabled = b;
            label390.Enabled = b;

            if (checkBox_Day_follow_AOD.Checked)
            {
                numericUpDown_DayX_AOD.Enabled = false;
                numericUpDown_DayY_AOD.Enabled = false;

                label380.Enabled = false;
                label388.Enabled = false;
                label389.Enabled = false;
            }
        }

        private void checkBox_Day_hand_Use_AOD_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Day_hand_Use_AOD.Checked;
            comboBox_Day_hand_image_AOD.Enabled = b;
            comboBox_Day_hand_imageCentr_AOD.Enabled = b;
            comboBox_Day_hand_imageBackground_AOD.Enabled = b;
            numericUpDown_Day_handX_AOD.Enabled = b;
            numericUpDown_Day_handY_AOD.Enabled = b;
            numericUpDown_Day_handX_centr_AOD.Enabled = b;
            numericUpDown_Day_handY_centr_AOD.Enabled = b;
            numericUpDown_Day_handX_background_AOD.Enabled = b;
            numericUpDown_Day_handY_background_AOD.Enabled = b;
            numericUpDown_Day_handX_offset_AOD.Enabled = b;
            numericUpDown_Day_handY_offset_AOD.Enabled = b;
            numericUpDown_Day_hand_startAngle_AOD.Enabled = b;
            numericUpDown_Day_hand_endAngle_AOD.Enabled = b;

            label363.Enabled = b;
            label364.Enabled = b;
            label365.Enabled = b;
            label366.Enabled = b;
            label367.Enabled = b;
            label368.Enabled = b;
            label369.Enabled = b;
            label370.Enabled = b;
            label371.Enabled = b;
            label372.Enabled = b;
            label373.Enabled = b;
            label374.Enabled = b;
            label375.Enabled = b;
            label376.Enabled = b;
            label377.Enabled = b;
            label378.Enabled = b;
            label379.Enabled = b;

            label714.Enabled = b;
            label715.Enabled = b;
            label716.Enabled = b;
            label717.Enabled = b;
        }

        private void checkBox_Month_pictures_Use_AOD_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Month_pictures_Use_AOD.Checked;
            comboBox_Month_pictures_image_AOD.Enabled = b;
            numericUpDown_Month_picturesX_AOD.Enabled = b;
            numericUpDown_Month_picturesY_AOD.Enabled = b;

            label428.Enabled = b;
            label429.Enabled = b;
            label430.Enabled = b;
            label431.Enabled = b;
        }

        private void checkBox_Month_Use_AOD_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Month_Use_AOD.Checked;
            comboBox_Month_image_AOD.Enabled = b;
            comboBox_Month_unit_AOD.Enabled = b;
            comboBox_Month_separator_AOD.Enabled = b;
            numericUpDown_MonthX_AOD.Enabled = b;
            numericUpDown_MonthY_AOD.Enabled = b;
            numericUpDown_Month_unitX_AOD.Enabled = b;
            numericUpDown_Month_unitY_AOD.Enabled = b;
            comboBox_Month_alignment_AOD.Enabled = b;
            checkBox_Month_add_zero_AOD.Enabled = b;
            numericUpDown_Month_spacing_AOD.Enabled = b;
            checkBox_Month_follow_AOD.Enabled = b;

            label417.Enabled = b;
            label418.Enabled = b;
            label419.Enabled = b;
            label420.Enabled = b;
            label421.Enabled = b;
            label422.Enabled = b;
            label423.Enabled = b;
            label424.Enabled = b;
            label425.Enabled = b;
            label426.Enabled = b;
            label427.Enabled = b;

            if (checkBox_Month_follow_AOD.Checked)
            {
                numericUpDown_MonthX_AOD.Enabled = false;
                numericUpDown_MonthY_AOD.Enabled = false;

                label417.Enabled = false;
                label425.Enabled = false;
                label426.Enabled = false;
            }
        }

        private void checkBox_Month_hand_Use_AOD_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Month_hand_Use_AOD.Checked;
            comboBox_Month_hand_image_AOD.Enabled = b;
            comboBox_Month_hand_imageCentr_AOD.Enabled = b;
            comboBox_Month_hand_imageBackground_AOD.Enabled = b;
            numericUpDown_Month_handX_AOD.Enabled = b;
            numericUpDown_Month_handY_AOD.Enabled = b;
            numericUpDown_Month_handX_centr_AOD.Enabled = b;
            numericUpDown_Month_handY_centr_AOD.Enabled = b;
            numericUpDown_Month_handX_background_AOD.Enabled = b;
            numericUpDown_Month_handY_background_AOD.Enabled = b;
            numericUpDown_Month_handX_offset_AOD.Enabled = b;
            numericUpDown_Month_handY_offset_AOD.Enabled = b;
            numericUpDown_Month_hand_startAngle_AOD.Enabled = b;
            numericUpDown_Month_hand_endAngle_AOD.Enabled = b;

            label391.Enabled = b;
            label392.Enabled = b;
            label393.Enabled = b;
            label394.Enabled = b;
            label395.Enabled = b;
            label396.Enabled = b;
            label397.Enabled = b;
            label398.Enabled = b;
            label399.Enabled = b;
            label400.Enabled = b;
            label401.Enabled = b;
            label402.Enabled = b;
            label403.Enabled = b;
            label404.Enabled = b;
            label405.Enabled = b;
            label411.Enabled = b;
            label416.Enabled = b;
        }

        private void checkBox_Year_text_Use_AOD_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_Year_text_Use_AOD.Checked;
            comboBox_Year_image_AOD.Enabled = b;
            comboBox_Year_unit_AOD.Enabled = b;
            comboBox_Year_separator_AOD.Enabled = b;
            numericUpDown_YearX_AOD.Enabled = b;
            numericUpDown_YearY_AOD.Enabled = b;
            numericUpDown_Year_unitX_AOD.Enabled = b;
            numericUpDown_Year_unitY_AOD.Enabled = b;
            comboBox_Year_alignment_AOD.Enabled = b;
            checkBox_Year_add_zero_AOD.Enabled = b;
            numericUpDown_Year_spacing_AOD.Enabled = b;
            checkBox_Year_follow_AOD.Enabled = b;

            label432.Enabled = b;
            label433.Enabled = b;
            label434.Enabled = b;
            label435.Enabled = b;
            label436.Enabled = b;
            label437.Enabled = b;
            label438.Enabled = b;
            label439.Enabled = b;
            label440.Enabled = b;
            label441.Enabled = b;
            label442.Enabled = b;
        }

        private void checkBox_DOW_pictures_Use_AOD_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_DOW_pictures_Use_AOD.Checked;
            comboBox_DOW_pictures_image_AOD.Enabled = b;
            numericUpDown_DOW_picturesX_AOD.Enabled = b;
            numericUpDown_DOW_picturesY_AOD.Enabled = b;

            label460.Enabled = b;
            label461.Enabled = b;
            label462.Enabled = b;
            label463.Enabled = b;
        }

        private void checkBox_DOW_hand_Use_AOD_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_DOW_hand_Use_AOD.Checked;
            comboBox_DOW_hand_image_AOD.Enabled = b;
            comboBox_DOW_hand_imageCentr_AOD.Enabled = b;
            comboBox_DOW_hand_imageBackground_AOD.Enabled = b;
            numericUpDown_DOW_handX_AOD.Enabled = b;
            numericUpDown_DOW_handY_AOD.Enabled = b;
            numericUpDown_DOW_handX_centr_AOD.Enabled = b;
            numericUpDown_DOW_handY_centr_AOD.Enabled = b;
            numericUpDown_DOW_handX_background_AOD.Enabled = b;
            numericUpDown_DOW_handY_background_AOD.Enabled = b;
            numericUpDown_DOW_handX_offset_AOD.Enabled = b;
            numericUpDown_DOW_handY_offset_AOD.Enabled = b;
            numericUpDown_DOW_hand_startAngle_AOD.Enabled = b;
            numericUpDown_DOW_hand_endAngle_AOD.Enabled = b;

            label443.Enabled = b;
            label444.Enabled = b;
            label445.Enabled = b;
            label446.Enabled = b;
            label447.Enabled = b;
            label448.Enabled = b;
            label449.Enabled = b;
            label450.Enabled = b;
            label451.Enabled = b;
            label452.Enabled = b;
            label453.Enabled = b;
            label454.Enabled = b;
            label455.Enabled = b;
            label456.Enabled = b;
            label457.Enabled = b;
            label458.Enabled = b;
            label459.Enabled = b;
        }


        private void checkBox_Status_Use_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            Control control = checkBox.Parent;
            Control.ControlCollection controlCollection = control.Controls;

            bool b = checkBox.Checked;
            for (int i = 1; i < controlCollection.Count; i++)
            {
                controlCollection[i].Enabled = b;
            }
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
            int stress = rnd.Next(0, 13);
            int fatBurning = rnd.Next(0, 13);
            bool bluetooth = rnd.Next(2) == 0 ? false : true;
            bool alarm = rnd.Next(2) == 0 ? false : true;
            bool unlocked = rnd.Next(2) == 0 ? false : true;
            bool dnd = rnd.Next(2) == 0 ? false : true;

            int temperature = rnd.Next(-25, 35);
            int temperatureMax = rnd.Next(-25, 35);
            int temperatureMin = temperatureMax - rnd.Next(3, 10);
            int temperatureIcon = rnd.Next(0, 29);
            bool showTemperature = rnd.Next(7) == 0 ? false : true;

            int airPressure = rnd.Next(800, 1200);
            int airQuality = rnd.Next(0, 650);
            int altitude = rnd.Next(0, 100);
            int humidity = rnd.Next(0, 100);
            int UVindex = rnd.Next(0, 13);
            int windForce = rnd.Next(0, 13);

            Watch_Face_Preview_Set.Date.Year = year;
            Watch_Face_Preview_Set.Date.Month = month;
            Watch_Face_Preview_Set.Date.Day = day;
            Watch_Face_Preview_Set.Date.WeekDay = weekDay;

            Watch_Face_Preview_Set.Time.Hours = hour;
            Watch_Face_Preview_Set.Time.Minutes = min;
            Watch_Face_Preview_Set.Time.Seconds = sec;

            Watch_Face_Preview_Set.Battery = battery;
            Watch_Face_Preview_Set.Activity.Steps = steps;
            Watch_Face_Preview_Set.Activity.StepsGoal = goal;
            Watch_Face_Preview_Set.Activity.Calories = calories;
            Watch_Face_Preview_Set.Activity.HeartRate = pulse;
            Watch_Face_Preview_Set.Activity.PAI = pai;
            Watch_Face_Preview_Set.Activity.Distance = distance;
            Watch_Face_Preview_Set.Activity.StandUp = standUp;
            Watch_Face_Preview_Set.Activity.Stress = stress;
            Watch_Face_Preview_Set.Activity.FatBurning = fatBurning;

            Watch_Face_Preview_Set.Status.Bluetooth = bluetooth;
            Watch_Face_Preview_Set.Status.Alarm = alarm;
            Watch_Face_Preview_Set.Status.Lock = unlocked;
            Watch_Face_Preview_Set.Status.DoNotDisturb = dnd;

            Watch_Face_Preview_Set.Weather.Temperature = temperature;
            Watch_Face_Preview_Set.Weather.TemperatureMin = temperatureMin;
            Watch_Face_Preview_Set.Weather.TemperatureMax = temperatureMax;
            Watch_Face_Preview_Set.Weather.Icon = temperatureIcon;
            Watch_Face_Preview_Set.Weather.showTemperature = showTemperature;

            Watch_Face_Preview_Set.Weather.AirPressure = airPressure;
            Watch_Face_Preview_Set.Weather.AirQuality = airQuality;
            Watch_Face_Preview_Set.Weather.Altitude = altitude;
            Watch_Face_Preview_Set.Weather.Humidity = humidity;
            Watch_Face_Preview_Set.Weather.UVindex = UVindex;
            Watch_Face_Preview_Set.Weather.WindForce = windForce;
            PreviewImage();

        }

        #region copy AOD

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


            Copy_pictures_AOD(userControl_pictures_Battery, userControl_pictures_Battery_AOD);
            Copy_text_AOD(userControl_text_Battery, userControl_text_Battery_AOD);
            Copy_hand_AOD(userControl_hand_Battery, userControl_hand_Battery_AOD);
            Copy_scaleCircle_AOD(userControl_scaleCircle_Battery, userControl_scaleCircle_Battery_AOD);
            Copy_scaleLinear_AOD(userControl_scaleLinear_Battery, userControl_scaleLinear_Battery_AOD);
            Copy_SystemFont_Group_AOD(userControl_SystemFont_Group_Battery, userControl_SystemFont_Group_Battery_AOD);
            Copy_icon_AOD(userControl_icon_Battery, userControl_icon_Battery_AOD);

            Copy_pictures_AOD(userControl_pictures_Steps, userControl_pictures_Steps_AOD);
            Copy_text_AOD(userControl_text_Steps, userControl_text_Steps_AOD);
            Copy_text_AOD(userControl_text_goal_Steps_AOD, userControl_text_goal_Steps_AOD);
            Copy_hand_AOD(userControl_hand_Steps, userControl_hand_Steps_AOD);
            Copy_scaleCircle_AOD(userControl_scaleCircle_Steps, userControl_scaleCircle_Steps_AOD);
            Copy_scaleLinear_AOD(userControl_scaleLinear_Steps, userControl_scaleLinear_Steps_AOD);
            Copy_SystemFont_Group_AOD(userControl_SystemFont_Group_Steps, userControl_SystemFont_Group_Steps_AOD);
            Copy_icon_AOD(userControl_icon_Steps, userControl_icon_Steps_AOD);

            Copy_pictures_AOD(userControl_pictures_Calories, userControl_pictures_Calories_AOD);
            Copy_text_AOD(userControl_text_Calories, userControl_text_Calories_AOD);
            Copy_text_AOD(userControl_text_goal_Calories_AOD, userControl_text_goal_Calories_AOD);
            Copy_hand_AOD(userControl_hand_Calories, userControl_hand_Calories_AOD);
            Copy_scaleCircle_AOD(userControl_scaleCircle_Calories, userControl_scaleCircle_Calories_AOD);
            Copy_scaleLinear_AOD(userControl_scaleLinear_Calories, userControl_scaleLinear_Calories_AOD);
            Copy_SystemFont_Group_AOD(userControl_SystemFont_Group_Calories, userControl_SystemFont_Group_Calories_AOD);
            Copy_icon_AOD(userControl_icon_Calories, userControl_icon_Calories_AOD);

            Copy_pictures_AOD(userControl_pictures_HeartRate, userControl_pictures_HeartRate_AOD);
            Copy_text_AOD(userControl_text_HeartRate, userControl_text_HeartRate_AOD);
            Copy_hand_AOD(userControl_hand_HeartRate, userControl_hand_HeartRate_AOD);
            Copy_scaleCircle_AOD(userControl_scaleCircle_HeartRate, userControl_scaleCircle_HeartRate_AOD);
            Copy_scaleLinear_AOD(userControl_scaleLinear_HeartRate, userControl_scaleLinear_HeartRate_AOD);
            Copy_SystemFont_Group_AOD(userControl_SystemFont_Group_HeartRate, userControl_SystemFont_Group_HeartRate_AOD);
            Copy_icon_AOD(userControl_icon_HeartRate, userControl_icon_HeartRate_AOD);

            Copy_pictures_AOD(userControl_pictures_PAI, userControl_pictures_PAI_AOD);
            Copy_text_AOD(userControl_text_PAI, userControl_text_PAI_AOD);
            Copy_hand_AOD(userControl_hand_PAI, userControl_hand_PAI_AOD);
            Copy_scaleCircle_AOD(userControl_scaleCircle_PAI, userControl_scaleCircle_PAI_AOD);
            Copy_scaleLinear_AOD(userControl_scaleLinear_PAI, userControl_scaleLinear_PAI_AOD);
            Copy_SystemFont_Group_AOD(userControl_SystemFont_Group_PAI, userControl_SystemFont_Group_PAI_AOD);
            Copy_icon_AOD(userControl_icon_PAI, userControl_icon_PAI_AOD);

            Copy_text_AOD(userControl_text_Distance, userControl_text_Distance_AOD);
            Copy_SystemFont_Group_AOD(userControl_SystemFont_Group_Distance, userControl_SystemFont_Group_Distance_AOD);
            Copy_icon_AOD(userControl_icon_Distance, userControl_icon_Distance_AOD);

            Copy_pictures_AOD(userControl_pictures_StandUp, userControl_pictures_StandUp_AOD);
            Copy_text_AOD(userControl_text_StandUp, userControl_text_StandUp_AOD);
            Copy_text_AOD(userControl_text_goal_StandUp_AOD, userControl_text_goal_StandUp_AOD);
            Copy_hand_AOD(userControl_hand_StandUp, userControl_hand_StandUp_AOD);
            Copy_scaleCircle_AOD(userControl_scaleCircle_StandUp, userControl_scaleCircle_StandUp_AOD);
            Copy_scaleLinear_AOD(userControl_scaleLinear_StandUp, userControl_scaleLinear_StandUp_AOD);
            Copy_SystemFont_Group_AOD(userControl_SystemFont_Group_StandUp, userControl_SystemFont_Group_StandUp_AOD);
            Copy_icon_AOD(userControl_icon_StandUp, userControl_icon_StandUp_AOD);

            Copy_pictures_AOD(userControl_pictures_weather, userControl_pictures_weather_AOD);
            Copy_text_AOD(userControl_text_weather_Current, userControl_text_weather_Current_AOD);
            Copy_text_AOD(userControl_text_weather_Min, userControl_text_weather_Min_AOD);
            Copy_text_AOD(userControl_text_weather_Max, userControl_text_weather_Max_AOD);
            Copy_hand_AOD(userControl_hand_Weather, userControl_hand_Weather_AOD);
            Copy_scaleCircle_AOD(userControl_scaleCircle_Weather, userControl_scaleCircle_Weather_AOD);
            Copy_scaleLinear_AOD(userControl_scaleLinear_Weather, userControl_scaleLinear_Weather_AOD);
            Copy_SystemFont_GroupWeather_AOD(userControl_SystemFont_GroupWeather, userControl_SystemFont_GroupWeather_AOD);
            Copy_icon_AOD(userControl_icon_Weather, userControl_icon_Weather_AOD);

            Copy_pictures_AOD(userControl_pictures_UVindex, userControl_pictures_UVindex_AOD);
            Copy_text_AOD(userControl_text_UVindex, userControl_text_UVindex_AOD);
            Copy_hand_AOD(userControl_hand_UVindex, userControl_hand_UVindex_AOD);
            Copy_scaleCircle_AOD(userControl_scaleCircle_UVindex, userControl_scaleCircle_UVindex_AOD);
            Copy_scaleLinear_AOD(userControl_scaleLinear_UVindex, userControl_scaleLinear_UVindex_AOD);
            Copy_SystemFont_Group_AOD(userControl_SystemFont_Group_UVindex, userControl_SystemFont_Group_UVindex_AOD);
            Copy_icon_AOD(userControl_icon_UVindex, userControl_icon_UVindex_AOD);

            Copy_pictures_AOD(userControl_pictures_AirQuality, userControl_pictures_AirQuality_AOD);
            Copy_text_AOD(userControl_text_AirQuality, userControl_text_AirQuality_AOD);
            Copy_hand_AOD(userControl_hand_AirQuality, userControl_hand_AirQuality_AOD);
            Copy_scaleCircle_AOD(userControl_scaleCircle_AirQuality, userControl_scaleCircle_AirQuality_AOD);
            Copy_scaleLinear_AOD(userControl_scaleLinear_AirQuality, userControl_scaleLinear_AirQuality_AOD);
            Copy_SystemFont_Group_AOD(userControl_SystemFont_Group_AirQuality, userControl_SystemFont_Group_AirQuality_AOD);
            Copy_icon_AOD(userControl_icon_AirQuality, userControl_icon_AirQuality_AOD);

            Copy_pictures_AOD(userControl_pictures_Humidity, userControl_pictures_Humidity_AOD);
            Copy_text_AOD(userControl_text_Humidity, userControl_text_Humidity_AOD);
            Copy_hand_AOD(userControl_hand_Humidity, userControl_hand_Humidity_AOD);
            Copy_scaleCircle_AOD(userControl_scaleCircle_Humidity, userControl_scaleCircle_Humidity_AOD);
            Copy_scaleLinear_AOD(userControl_scaleLinear_Humidity, userControl_scaleLinear_Humidity_AOD);
            Copy_SystemFont_Group_AOD(userControl_SystemFont_Group_Humidity, userControl_SystemFont_Group_Humidity_AOD);
            Copy_icon_AOD(userControl_icon_Humidity, userControl_icon_Humidity_AOD);

            Copy_pictures_AOD(userControl_pictures_Sunrise, userControl_pictures_Sunrise_AOD);
            Copy_text_AOD(userControl_text_SunriseSunset, userControl_text_SunriseSunset_AOD);
            Copy_text_AOD(userControl_text_Sunrise, userControl_text_Sunrise_AOD);
            Copy_text_AOD(userControl_text_Sunset, userControl_text_Sunset_AOD);
            Copy_hand_AOD(userControl_hand_Sunrise, userControl_hand_Sunrise_AOD);
            Copy_scaleCircle_AOD(userControl_scaleCircle_Sunrise, userControl_scaleCircle_Sunrise_AOD);
            Copy_scaleLinear_AOD(userControl_scaleLinear_Sunrise, userControl_scaleLinear_Sunrise_AOD);
            Copy_SystemFont_GroupWeather_AOD(userControl_SystemFont_GroupSunrise, userControl_SystemFont_GroupSunrise_AOD);
            Copy_icon_AOD(userControl_icon_Sunrise, userControl_icon_Sunrise_AOD);

            Copy_pictures_AOD(userControl_pictures_WindForce, userControl_pictures_WindForce_AOD);
            Copy_text_AOD(userControl_text_WindForce, userControl_text_WindForce_AOD);
            Copy_hand_AOD(userControl_hand_WindForce, userControl_hand_WindForce_AOD);
            Copy_scaleCircle_AOD(userControl_scaleCircle_WindForce, userControl_scaleCircle_WindForce_AOD);
            Copy_scaleLinear_AOD(userControl_scaleLinear_WindForce, userControl_scaleLinear_WindForce_AOD);
            Copy_SystemFont_Group_AOD(userControl_SystemFont_Group_WindForce, userControl_SystemFont_Group_WindForce_AOD);
            Copy_icon_AOD(userControl_icon_WindForce, userControl_icon_WindForce_AOD);

            Copy_pictures_AOD(userControl_pictures_Altitude, userControl_pictures_Altitude_AOD);
            Copy_text_AOD(userControl_text_Altitude, userControl_text_Altitude_AOD);
            Copy_hand_AOD(userControl_hand_Altitude, userControl_hand_Altitude_AOD);
            Copy_scaleCircle_AOD(userControl_scaleCircle_Altitude, userControl_scaleCircle_Altitude_AOD);
            Copy_scaleLinear_AOD(userControl_scaleLinear_Altitude, userControl_scaleLinear_Altitude_AOD);
            Copy_SystemFont_Group_AOD(userControl_SystemFont_Group_Altitude, userControl_SystemFont_Group_Altitude_AOD);
            Copy_icon_AOD(userControl_icon_Altitude, userControl_icon_Altitude_AOD);

            Copy_pictures_AOD(userControl_pictures_AirPressure, userControl_pictures_AirPressure_AOD);
            Copy_text_AOD(userControl_text_AirPressure, userControl_text_AirPressure_AOD);
            Copy_hand_AOD(userControl_hand_AirPressure, userControl_hand_AirPressure_AOD);
            Copy_scaleCircle_AOD(userControl_scaleCircle_AirPressure, userControl_scaleCircle_AirPressure_AOD);
            Copy_scaleLinear_AOD(userControl_scaleLinear_AirPressure, userControl_scaleLinear_AirPressure_AOD);
            Copy_SystemFont_Group_AOD(userControl_SystemFont_Group_AirPressure, userControl_SystemFont_Group_AirPressure_AOD);
            Copy_icon_AOD(userControl_icon_AirPressure, userControl_icon_AirPressure_AOD);

            Copy_pictures_AOD(userControl_pictures_Stress, userControl_pictures_Stress_AOD);
            Copy_text_AOD(userControl_text_Stress, userControl_text_Stress_AOD);
            Copy_hand_AOD(userControl_hand_Stress, userControl_hand_Stress_AOD);
            Copy_scaleCircle_AOD(userControl_scaleCircle_Stress, userControl_scaleCircle_Stress_AOD);
            Copy_scaleLinear_AOD(userControl_scaleLinear_Stress, userControl_scaleLinear_Stress_AOD);
            Copy_SystemFont_Group_AOD(userControl_SystemFont_Group_Stress, userControl_SystemFont_Group_Stress_AOD);
            Copy_icon_AOD(userControl_icon_Stress, userControl_icon_Stress_AOD);

            Copy_pictures_AOD(userControl_pictures_ActivityGoal, userControl_pictures_ActivityGoal_AOD);
            Copy_text_AOD(userControl_text_ActivityGoal, userControl_text_ActivityGoal_AOD);
            Copy_text_AOD(userControl_text_goal_ActivityGoal_AOD, userControl_text_goal_StandUp_AOD);
            Copy_hand_AOD(userControl_hand_ActivityGoal, userControl_hand_ActivityGoal_AOD);
            Copy_scaleCircle_AOD(userControl_scaleCircle_ActivityGoal, userControl_scaleCircle_ActivityGoal_AOD);
            Copy_scaleLinear_AOD(userControl_scaleLinear_ActivityGoal, userControl_scaleLinear_ActivityGoal_AOD);
            Copy_SystemFont_Group_AOD(userControl_SystemFont_Group_ActivityGoal, userControl_SystemFont_Group_ActivityGoal_AOD);
            Copy_icon_AOD(userControl_icon_ActivityGoal, userControl_icon_ActivityGoal_AOD);

            Copy_pictures_AOD(userControl_pictures_FatBurning, userControl_pictures_FatBurning_AOD);
            Copy_text_AOD(userControl_text_FatBurning, userControl_text_FatBurning_AOD);
            Copy_text_AOD(userControl_text_goal_FatBurning_AOD, userControl_text_goal_StandUp_AOD);
            Copy_hand_AOD(userControl_hand_FatBurning, userControl_hand_FatBurning_AOD);
            Copy_scaleCircle_AOD(userControl_scaleCircle_FatBurning, userControl_scaleCircle_FatBurning_AOD);
            Copy_scaleLinear_AOD(userControl_scaleLinear_FatBurning, userControl_scaleLinear_FatBurning_AOD);
            Copy_SystemFont_Group_AOD(userControl_SystemFont_Group_FatBurning, userControl_SystemFont_Group_FatBurning_AOD);
            Copy_icon_AOD(userControl_icon_FatBurning, userControl_icon_FatBurning_AOD);

            Copy_SystemFont_GroupWeather_AOD(userControl_SystemFont_GroupTime, userControl_SystemFont_GroupTime_AOD);

            Copy_SystemFont_Group_AOD(userControl_SystemFont_Group_Day, userControl_SystemFont_Group_Day_AOD);
            Copy_SystemFont_Group_AOD(userControl_SystemFont_Group_Month, userControl_SystemFont_Group_Month_AOD);
            Copy_SystemFont_Group_AOD(userControl_SystemFont_Group_Year, userControl_SystemFont_Group_Year_AOD);


            PreviewView = true;
            JSON_write();
            PreviewImage();
        }


        private void Copy_pictures_AOD(UserControl_pictures userControl_pictures, UserControl_pictures userControl_pictures_AOD)
        {
            userControl_pictures_AOD.checkBox_pictures_Use.Checked = userControl_pictures.checkBox_pictures_Use.Checked;
            userControl_pictures_AOD.comboBoxSetImage(userControl_pictures.comboBoxGetImage());
            userControl_pictures_AOD.numericUpDown_picturesX.Value = userControl_pictures.numericUpDown_picturesX.Value;
            userControl_pictures_AOD.numericUpDown_picturesY.Value = userControl_pictures.numericUpDown_picturesY.Value;
            userControl_pictures_AOD.numericUpDown_pictures_count.Value = userControl_pictures.numericUpDown_pictures_count.Value;
        }
        private void Copy_text_AOD(UserControl_text userControl_text, UserControl_text userControl_text_AOD)
        {
            userControl_text_AOD.checkBox_Use.Checked = userControl_text.checkBox_Use.Checked;
            userControl_text_AOD.comboBoxSetImage(userControl_text.comboBoxGetImage());
            userControl_text_AOD.comboBoxSetIcon(userControl_text.comboBoxGetIcon());
            userControl_text_AOD.comboBoxSetUnit(userControl_text.comboBoxGetUnit());
            userControl_text_AOD.numericUpDown_imageX.Value = userControl_text.numericUpDown_imageX.Value;
            userControl_text_AOD.numericUpDown_imageY.Value = userControl_text.numericUpDown_imageY.Value;
            userControl_text_AOD.numericUpDown_iconX.Value = userControl_text.numericUpDown_iconX.Value;
            userControl_text_AOD.numericUpDown_iconY.Value = userControl_text.numericUpDown_iconY.Value;
            userControl_text_AOD.comboBoxSetAlignment(userControl_text.comboBoxGetAlignment());
            userControl_text_AOD.numericUpDown_spacing.Value = userControl_text.numericUpDown_spacing.Value;
            userControl_text_AOD.checkBox_addZero.Checked = userControl_text.checkBox_addZero.Checked;
            userControl_text_AOD.checkBox_follow.Checked = userControl_text.checkBox_follow.Checked;
            userControl_text_AOD.comboBoxSetImageError(userControl_text.comboBoxGetImageError());
            userControl_text_AOD.comboBoxSetImageDecimalPointOrMinus(userControl_text.comboBoxGetImageDecimalPointOrMinus());

        }
        private void Copy_hand_AOD(UserControl_hand userControl_hand, UserControl_hand userControl_hand_AOD)
        {
            userControl_hand_AOD.checkBox_hand_Use.Checked = userControl_hand.checkBox_hand_Use.Checked;
            userControl_hand_AOD.comboBoxSetHandImage(userControl_hand.comboBoxGetHandImage());
            userControl_hand_AOD.numericUpDown_handX.Value = userControl_hand.numericUpDown_handX.Value;
            userControl_hand_AOD.numericUpDown_handY.Value = userControl_hand.numericUpDown_handY.Value;
            userControl_hand_AOD.numericUpDown_handX_offset.Value = userControl_hand.numericUpDown_handX_offset.Value;
            userControl_hand_AOD.numericUpDown_handY_offset.Value = userControl_hand.numericUpDown_handY_offset.Value;
            userControl_hand_AOD.comboBoxSetHandImageCentr(userControl_hand.comboBoxGetHandImageCentr());
            userControl_hand_AOD.numericUpDown_handX_centr.Value = userControl_hand.numericUpDown_handX_centr.Value;
            userControl_hand_AOD.numericUpDown_handY_centr.Value = userControl_hand.numericUpDown_handY_centr.Value;
            userControl_hand_AOD.numericUpDown_hand_startAngle.Value = userControl_hand.numericUpDown_hand_startAngle.Value;
            userControl_hand_AOD.numericUpDown_hand_endAngle.Value = userControl_hand.numericUpDown_hand_endAngle.Value;
            userControl_hand_AOD.comboBoxSetHandImageBackground(userControl_hand.comboBoxGetHandImageBackground());
            userControl_hand_AOD.numericUpDown_handX_background.Value = userControl_hand.numericUpDown_handX_background.Value;
            userControl_hand_AOD.numericUpDown_handY_background.Value = userControl_hand.numericUpDown_handY_background.Value;

        }
        private void Copy_scaleCircle_AOD(UserControl_scaleCircle userControl_scaleCircle, UserControl_scaleCircle userControl_scaleCircle_AOD)
        {
            userControl_scaleCircle_AOD.checkBox_scaleCircle_Use.Checked = userControl_scaleCircle.checkBox_scaleCircle_Use.Checked;
            userControl_scaleCircle_AOD.radioButton_scaleCircle_image.Checked = userControl_scaleCircle.radioButton_scaleCircle_image.Checked;
            userControl_scaleCircle_AOD.radioButton_scaleCircle_color.Checked = userControl_scaleCircle.radioButton_scaleCircle_color.Checked;
            userControl_scaleCircle_AOD.comboBoxSetImage(userControl_scaleCircle.comboBoxGetImage());
            userControl_scaleCircle_AOD.comboBoxSetColorString(userControl_scaleCircle.comboBoxGetColorString());
            userControl_scaleCircle_AOD.comboBoxSetFlatness(userControl_scaleCircle.comboBoxGetFlatness());
            userControl_scaleCircle_AOD.comboBoxSetImageBackground(userControl_scaleCircle.comboBoxGetImageBackground());
            userControl_scaleCircle_AOD.numericUpDown_scaleCircleX.Value = userControl_scaleCircle.numericUpDown_scaleCircleX.Value;
            userControl_scaleCircle_AOD.numericUpDown_scaleCircleY.Value = userControl_scaleCircle.numericUpDown_scaleCircleY.Value;
            userControl_scaleCircle_AOD.numericUpDown_scaleCircle_radius.Value = userControl_scaleCircle.numericUpDown_scaleCircle_radius.Value;
            userControl_scaleCircle_AOD.numericUpDown_scaleCircle_width.Value = userControl_scaleCircle.numericUpDown_scaleCircle_width.Value;
            userControl_scaleCircle_AOD.numericUpDown_scaleCircle_startAngle.Value = userControl_scaleCircle.numericUpDown_scaleCircle_startAngle.Value;
            userControl_scaleCircle_AOD.numericUpDown_scaleCircle_endAngle.Value = userControl_scaleCircle.numericUpDown_scaleCircle_endAngle.Value;
        }
        private void Copy_scaleLinear_AOD(UserControl_scaleLinear userControl_scaleLinear, UserControl_scaleLinear userControl_scaleLinear_AOD)
        {
            userControl_scaleLinear_AOD.checkBox_scaleLinear_Use.Checked = userControl_scaleLinear.checkBox_scaleLinear_Use.Checked;
            userControl_scaleLinear_AOD.radioButton_scaleLinear_image.Checked = userControl_scaleLinear.radioButton_scaleLinear_image.Checked;
            userControl_scaleLinear_AOD.radioButton_scaleLinear_color.Checked = userControl_scaleLinear.radioButton_scaleLinear_color.Checked;
            userControl_scaleLinear_AOD.comboBoxSetImage(userControl_scaleLinear.comboBoxGetImage());
            userControl_scaleLinear_AOD.comboBoxSetColorString(userControl_scaleLinear.comboBoxGetColorString());
            userControl_scaleLinear_AOD.comboBoxSetImagePointer(userControl_scaleLinear.comboBoxGetImagePointer());
            userControl_scaleLinear_AOD.comboBoxSetImageBackground(userControl_scaleLinear.comboBoxGetImageBackground());
            userControl_scaleLinear_AOD.numericUpDown_scaleLinearX.Value = userControl_scaleLinear.numericUpDown_scaleLinearX.Value;
            userControl_scaleLinear_AOD.numericUpDown_scaleLinearY.Value = userControl_scaleLinear.numericUpDown_scaleLinearY.Value;
            userControl_scaleLinear_AOD.numericUpDown_scaleLinear_length.Value = userControl_scaleLinear.numericUpDown_scaleLinear_length.Value;
            userControl_scaleLinear_AOD.numericUpDown_scaleLinear_width.Value = userControl_scaleLinear.numericUpDown_scaleLinear_width.Value;
            userControl_scaleLinear_AOD.comboBoxSetFlatness(userControl_scaleLinear.comboBoxGetFlatness());
        }
        private void Copy_icon_AOD(UserControl_icon userControl_icon, UserControl_icon userControl_icon_AOD)
        {
            userControl_icon_AOD.checkBox_icon_Use.Checked = userControl_icon.checkBox_icon_Use.Checked;
            userControl_icon_AOD.comboBoxSetImage(userControl_icon.comboBoxGetImage());
            userControl_icon_AOD.comboBoxSetImage2(userControl_icon.comboBoxGetImage2());
            userControl_icon_AOD.numericUpDown_iconX.Value = userControl_icon.numericUpDown_iconX.Value;
            userControl_icon_AOD.numericUpDown_iconY.Value = userControl_icon.numericUpDown_iconY.Value;
        }
        private void Copy_SystemFont_AOD(UserControl_SystemFont userControl_SystemFont,
            UserControl_SystemFont userControl_SystemFont_AOD)
        {
            userControl_SystemFont_AOD.checkBox_Use.Checked = userControl_SystemFont.checkBox_Use.Checked;
            userControl_SystemFont_AOD.numericUpDown_SystemFontX.Value =
                userControl_SystemFont.numericUpDown_SystemFontX.Value;
            userControl_SystemFont_AOD.numericUpDown_SystemFontY.Value =
                userControl_SystemFont.numericUpDown_SystemFontY.Value;
            userControl_SystemFont_AOD.numericUpDown_SystemFont_angle.Value =
                userControl_SystemFont.numericUpDown_SystemFont_angle.Value;
            userControl_SystemFont_AOD.numericUpDown_SystemFont_size.Value =
                userControl_SystemFont.numericUpDown_SystemFont_size.Value;
            userControl_SystemFont_AOD.numericUpDown_SystemFont_spacing.Value =
                userControl_SystemFont.numericUpDown_SystemFont_spacing.Value;
            userControl_SystemFont_AOD.comboBoxSetColorString(userControl_SystemFont.comboBoxGetColorString());
            userControl_SystemFont_AOD.checkBoxSetUnit(userControl_SystemFont.checkBoxGetUnit());
            userControl_SystemFont_AOD.checkBox_addZero.Checked = userControl_SystemFont.checkBox_addZero.Checked;
            userControl_SystemFont_AOD.checkBox_follow.Checked = userControl_SystemFont.checkBox_follow.Checked;
            userControl_SystemFont_AOD.checkBox_separator.Checked = userControl_SystemFont.checkBox_separator.Checked;
        }
        private void Copy_FontRotate_AOD(UserControl_FontRotate userControl_FontRotate,
            UserControl_FontRotate userControl_FontRotate_AOD)
        {
            userControl_FontRotate_AOD.checkBox_Use.Checked = userControl_FontRotate.checkBox_Use.Checked;
            userControl_FontRotate_AOD.numericUpDown_FontRotateX.Value =
                userControl_FontRotate.numericUpDown_FontRotateX.Value;
            userControl_FontRotate_AOD.numericUpDown_FontRotateY.Value =
                userControl_FontRotate.numericUpDown_FontRotateY.Value;
            userControl_FontRotate_AOD.numericUpDown_FontRotate_angle.Value =
                userControl_FontRotate.numericUpDown_FontRotate_angle.Value;
            userControl_FontRotate_AOD.numericUpDown_FontRotate_size.Value =
                userControl_FontRotate.numericUpDown_FontRotate_size.Value;
            userControl_FontRotate_AOD.numericUpDown_FontRotate_spacing.Value =
                userControl_FontRotate.numericUpDown_FontRotate_spacing.Value;
            userControl_FontRotate_AOD.numericUpDown_FontRotate_radius.Value =
                userControl_FontRotate.numericUpDown_FontRotate_radius.Value;
            userControl_FontRotate_AOD.comboBoxSetColorString(userControl_FontRotate.comboBoxGetColorString());
            userControl_FontRotate_AOD.checkBoxSetUnit(userControl_FontRotate.checkBoxGetUnit());
            userControl_FontRotate_AOD.radioButtonSetRotateDirection(userControl_FontRotate.radioButtonGetRotateDirection());
            userControl_FontRotate_AOD.checkBox_addZero.Checked = userControl_FontRotate.checkBox_addZero.Checked;
            userControl_FontRotate_AOD.checkBox_follow.Checked = userControl_FontRotate.checkBox_follow.Checked;
            userControl_FontRotate_AOD.checkBox_separator.Checked = userControl_FontRotate.checkBox_separator.Checked;
        }

        private void Copy_SystemFont_Group_AOD(UserControl_SystemFont_Group userControl_SystemFont_Group,
            UserControl_SystemFont_Group userControl_SystemFont_Group_AOD)
        {
            Copy_SystemFont_AOD(userControl_SystemFont_Group.userControl_SystemFont,
                    userControl_SystemFont_Group_AOD.userControl_SystemFont);
            Copy_SystemFont_AOD(userControl_SystemFont_Group.userControl_SystemFont_goal,
                    userControl_SystemFont_Group_AOD.userControl_SystemFont_goal);
            Copy_FontRotate_AOD(userControl_SystemFont_Group.userControl_FontRotate,
                    userControl_SystemFont_Group_AOD.userControl_FontRotate);
            Copy_FontRotate_AOD(userControl_SystemFont_Group.userControl_FontRotate_goal,
                    userControl_SystemFont_Group_AOD.userControl_FontRotate_goal);
        }
        private void Copy_SystemFont_GroupWeather_AOD(UserControl_SystemFont_GroupWeather userControl_SystemFont_Group,
            UserControl_SystemFont_GroupWeather userControl_SystemFont_Group_AOD)
        {
            Copy_SystemFont_AOD(userControl_SystemFont_Group.userControl_SystemFont_weather_Current,
                    userControl_SystemFont_Group_AOD.userControl_SystemFont_weather_Current);
            Copy_SystemFont_AOD(userControl_SystemFont_Group.userControl_SystemFont_weather_Min,
                    userControl_SystemFont_Group_AOD.userControl_SystemFont_weather_Min);
            Copy_SystemFont_AOD(userControl_SystemFont_Group.userControl_SystemFont_weather_Max,
                    userControl_SystemFont_Group_AOD.userControl_SystemFont_weather_Max);

            Copy_FontRotate_AOD(userControl_SystemFont_Group.userControl_FontRotate_weather_Current,
                    userControl_SystemFont_Group_AOD.userControl_FontRotate_weather_Current);
            Copy_FontRotate_AOD(userControl_SystemFont_Group.userControl_FontRotate_weather_Min,
                    userControl_SystemFont_Group_AOD.userControl_FontRotate_weather_Min);
            Copy_FontRotate_AOD(userControl_SystemFont_Group.userControl_FontRotate_weather_Max,
                    userControl_SystemFont_Group_AOD.userControl_FontRotate_weather_Max);
        }
        private void Copy_SystemFont_GroupSunrise_AOD(UserControl_SystemFont_GroupSunrise userControl_SystemFont_Group,
            UserControl_SystemFont_GroupSunrise userControl_SystemFont_Group_AOD)
        {
            Copy_SystemFont_AOD(userControl_SystemFont_Group.userControl_SystemFont_weather_Current,
                    userControl_SystemFont_Group_AOD.userControl_SystemFont_weather_Current);
            Copy_SystemFont_AOD(userControl_SystemFont_Group.userControl_SystemFont_weather_Min,
                    userControl_SystemFont_Group_AOD.userControl_SystemFont_weather_Min);
            Copy_SystemFont_AOD(userControl_SystemFont_Group.userControl_SystemFont_weather_Max,
                    userControl_SystemFont_Group_AOD.userControl_SystemFont_weather_Max);

            Copy_FontRotate_AOD(userControl_SystemFont_Group.userControl_FontRotate_weather_Current,
                    userControl_SystemFont_Group_AOD.userControl_FontRotate_weather_Current);
            Copy_FontRotate_AOD(userControl_SystemFont_Group.userControl_FontRotate_weather_Min,
                    userControl_SystemFont_Group_AOD.userControl_FontRotate_weather_Min);
            Copy_FontRotate_AOD(userControl_SystemFont_Group.userControl_FontRotate_weather_Max,
                    userControl_SystemFont_Group_AOD.userControl_FontRotate_weather_Max);
        }

        private void button_Copy_Hour_AOD_Click(object sender, EventArgs e)
        {
            PreviewView = false;
            Copy_Hour_AOD();
            PreviewView = true;
            JSON_write();
            PreviewImage();
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
            JSON_write();
            PreviewImage();
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
            JSON_write();
            PreviewImage();
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
            JSON_write();
            PreviewImage();
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
            JSON_write();
            PreviewImage();
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
            JSON_write();
            PreviewImage();
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
            JSON_write();
            PreviewImage();
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
            JSON_write();
            PreviewImage();
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
            JSON_write();
            PreviewImage();
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
            JSON_write();
            PreviewImage();
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
            JSON_write();
            PreviewImage();
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
            JSON_write();
            PreviewImage();
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
            JSON_write();
            PreviewImage();
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

        private void userControl_pictures_Battery_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_pictures_AOD(userControl_pictures_Battery, userControl_pictures_Battery_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_text_Battery_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_text_AOD(userControl_text_Battery, userControl_text_Battery_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_hand_Battery_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_hand_AOD(userControl_hand_Battery, userControl_hand_Battery_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleCircle_Battery_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleCircle_AOD(userControl_scaleCircle_Battery, userControl_scaleCircle_Battery_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleLinear_Battery_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleLinear_AOD(userControl_scaleLinear_Battery, userControl_scaleLinear_Battery_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_SystemFont_Group_Battery_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Type type = sender.GetType();
            if (type.Name == "UserControl_SystemFont")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_Battery.userControl_SystemFont,
                    userControl_SystemFont_Group_Battery_AOD.userControl_SystemFont);
            }
            if (type.Name == "UserControl_SystemFont_weather")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_Battery.userControl_SystemFont_goal,
                    userControl_SystemFont_Group_Battery_AOD.userControl_SystemFont_goal);
            }
            if (type.Name == "UserControl_FontRotate")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_Battery.userControl_FontRotate,
                    userControl_SystemFont_Group_Battery_AOD.userControl_FontRotate);
            }
            if (type.Name == "UserControl_FontRotate_weather")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_Battery.userControl_FontRotate_goal,
                    userControl_SystemFont_Group_Battery_AOD.userControl_FontRotate_goal);
            }

        }
        private void userControl_icon_Battery_AOD_Copy(object sender, EventArgs eventArgs)
        {

            PreviewView = false;
            Copy_icon_AOD(userControl_icon_Battery, userControl_icon_Battery_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        
        private void userControl_pictures_Steps_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_pictures_AOD(userControl_pictures_Steps, userControl_pictures_Steps_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_text_Steps_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_text_AOD(userControl_text_Steps, userControl_text_Steps_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_text_goal_Steps_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_text_AOD(userControl_text_goal_Steps, userControl_text_goal_Steps_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_hand_Steps_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_hand_AOD(userControl_hand_Steps, userControl_hand_Steps_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleCircle_Steps_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleCircle_AOD(userControl_scaleCircle_Steps, userControl_scaleCircle_Steps_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleLinear_Steps_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleLinear_AOD(userControl_scaleLinear_Steps, userControl_scaleLinear_Steps_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_SystemFont_Group_Steps_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Type type = sender.GetType();
            if (type.Name == "UserControl_SystemFont")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_Steps.userControl_SystemFont,
                    userControl_SystemFont_Group_Steps_AOD.userControl_SystemFont);
            }
            if (type.Name == "UserControl_SystemFont_weather")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_Steps.userControl_SystemFont_goal,
                    userControl_SystemFont_Group_Steps_AOD.userControl_SystemFont_goal);
            }
            if (type.Name == "UserControl_FontRotate")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_Steps.userControl_FontRotate,
                    userControl_SystemFont_Group_Steps_AOD.userControl_FontRotate);
            }
            if (type.Name == "UserControl_FontRotate_weather")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_Steps.userControl_FontRotate_goal,
                    userControl_SystemFont_Group_Steps_AOD.userControl_FontRotate_goal);
            }

        }
        private void userControl_icon_Steps_AOD_Copy(object sender, EventArgs eventArgs)
        {

            PreviewView = false;
            Copy_icon_AOD(userControl_icon_Steps, userControl_icon_Steps_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }

        private void userControl_pictures_Calories_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_pictures_AOD(userControl_pictures_Calories, userControl_pictures_Calories_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_text_Calories_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_text_AOD(userControl_text_Calories, userControl_text_Calories_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_text_goal_Calories_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_text_AOD(userControl_text_goal_Calories, userControl_text_goal_Calories_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_hand_Calories_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_hand_AOD(userControl_hand_Calories, userControl_hand_Calories_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleCircle_Calories_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleCircle_AOD(userControl_scaleCircle_Calories, userControl_scaleCircle_Calories_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleLinear_Calories_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleLinear_AOD(userControl_scaleLinear_Calories, userControl_scaleLinear_Calories_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_SystemFont_Group_Calories_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Type type = sender.GetType();
            if (type.Name == "UserControl_SystemFont")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_Calories.userControl_SystemFont,
                    userControl_SystemFont_Group_Calories_AOD.userControl_SystemFont);
            }
            if (type.Name == "UserControl_SystemFont_weather")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_Calories.userControl_SystemFont_goal,
                    userControl_SystemFont_Group_Calories_AOD.userControl_SystemFont_goal);
            }
            if (type.Name == "UserControl_FontRotate")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_Calories.userControl_FontRotate,
                    userControl_SystemFont_Group_Calories_AOD.userControl_FontRotate);
            }
            if (type.Name == "UserControl_FontRotate_weather")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_Calories.userControl_FontRotate_goal,
                    userControl_SystemFont_Group_Calories_AOD.userControl_FontRotate_goal);
            }

        }
        private void userControl_icon_Calories_AOD_Copy(object sender, EventArgs eventArgs)
        {

            PreviewView = false;
            Copy_icon_AOD(userControl_icon_Calories, userControl_icon_Calories_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }

        private void userControl_pictures_HeartRate_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_pictures_AOD(userControl_pictures_HeartRate, userControl_pictures_HeartRate_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_text_HeartRate_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_text_AOD(userControl_text_HeartRate, userControl_text_HeartRate_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_hand_HeartRate_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_hand_AOD(userControl_hand_HeartRate, userControl_hand_HeartRate_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleCircle_HeartRate_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleCircle_AOD(userControl_scaleCircle_HeartRate, userControl_scaleCircle_HeartRate_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleLinear_HeartRate_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleLinear_AOD(userControl_scaleLinear_HeartRate, userControl_scaleLinear_HeartRate_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_SystemFont_Group_HeartRate_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Type type = sender.GetType();
            if (type.Name == "UserControl_SystemFont")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_HeartRate.userControl_SystemFont,
                    userControl_SystemFont_Group_HeartRate_AOD.userControl_SystemFont);
            }
            if (type.Name == "UserControl_SystemFont_weather")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_HeartRate.userControl_SystemFont_goal,
                    userControl_SystemFont_Group_HeartRate_AOD.userControl_SystemFont_goal);
            }
            if (type.Name == "UserControl_FontRotate")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_HeartRate.userControl_FontRotate,
                    userControl_SystemFont_Group_HeartRate_AOD.userControl_FontRotate);
            }
            if (type.Name == "UserControl_FontRotate_weather")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_HeartRate.userControl_FontRotate_goal,
                    userControl_SystemFont_Group_HeartRate_AOD.userControl_FontRotate_goal);
            }

        }
        private void userControl_icon_HeartRate_AOD_Copy(object sender, EventArgs eventArgs)
        {

            PreviewView = false;
            Copy_icon_AOD(userControl_icon_HeartRate, userControl_icon_HeartRate_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }

        private void userControl_pictures_PAI_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_pictures_AOD(userControl_pictures_PAI, userControl_pictures_PAI_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_text_PAI_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_text_AOD(userControl_text_PAI, userControl_text_PAI_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_hand_PAI_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_hand_AOD(userControl_hand_PAI, userControl_hand_PAI_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleCircle_PAI_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleCircle_AOD(userControl_scaleCircle_PAI, userControl_scaleCircle_PAI_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleLinear_PAI_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleLinear_AOD(userControl_scaleLinear_PAI, userControl_scaleLinear_PAI_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_SystemFont_Group_PAI_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Type type = sender.GetType();
            if (type.Name == "UserControl_SystemFont")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_PAI.userControl_SystemFont,
                    userControl_SystemFont_Group_PAI_AOD.userControl_SystemFont);
            }
            if (type.Name == "UserControl_SystemFont_weather")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_PAI.userControl_SystemFont_goal,
                    userControl_SystemFont_Group_PAI_AOD.userControl_SystemFont_goal);
            }
            if (type.Name == "UserControl_FontRotate")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_PAI.userControl_FontRotate,
                    userControl_SystemFont_Group_PAI_AOD.userControl_FontRotate);
            }
            if (type.Name == "UserControl_FontRotate_weather")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_PAI.userControl_FontRotate_goal,
                    userControl_SystemFont_Group_PAI_AOD.userControl_FontRotate_goal);
            }

        }
        private void userControl_icon_PAI_AOD_Copy(object sender, EventArgs eventArgs)
        {

            PreviewView = false;
            Copy_icon_AOD(userControl_icon_PAI, userControl_icon_PAI_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }

        private void userControl_text_Distance_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_text_AOD(userControl_text_Distance, userControl_text_Distance_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_SystemFont_Group_Distance_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Type type = sender.GetType();
            if (type.Name == "UserControl_SystemFont")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_Distance.userControl_SystemFont,
                    userControl_SystemFont_Group_Distance_AOD.userControl_SystemFont);
            }
            if (type.Name == "UserControl_SystemFont_weather")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_Distance.userControl_SystemFont_goal,
                    userControl_SystemFont_Group_Distance_AOD.userControl_SystemFont_goal);
            }
            if (type.Name == "UserControl_FontRotate")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_Distance.userControl_FontRotate,
                    userControl_SystemFont_Group_Distance_AOD.userControl_FontRotate);
            }
            if (type.Name == "UserControl_FontRotate_weather")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_Distance.userControl_FontRotate_goal,
                    userControl_SystemFont_Group_Distance_AOD.userControl_FontRotate_goal);
            }

        }
        private void userControl_icon_Distance_AOD_Copy(object sender, EventArgs eventArgs)
        {

            PreviewView = false;
            Copy_icon_AOD(userControl_icon_Distance, userControl_icon_Distance_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }

        private void userControl_pictures_StandUp_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_pictures_AOD(userControl_pictures_StandUp, userControl_pictures_StandUp_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_text_StandUp_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_text_AOD(userControl_text_StandUp, userControl_text_StandUp_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_text_goal_StandUp_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_text_AOD(userControl_text_goal_StandUp, userControl_text_goal_StandUp_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_hand_StandUp_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_hand_AOD(userControl_hand_StandUp, userControl_hand_StandUp_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleCircle_StandUp_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleCircle_AOD(userControl_scaleCircle_StandUp, userControl_scaleCircle_StandUp_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleLinear_StandUp_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleLinear_AOD(userControl_scaleLinear_StandUp, userControl_scaleLinear_StandUp_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_SystemFont_Group_StandUp_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Type type = sender.GetType();
            if (type.Name == "UserControl_SystemFont")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_StandUp.userControl_SystemFont,
                    userControl_SystemFont_Group_StandUp_AOD.userControl_SystemFont);
            }
            if (type.Name == "UserControl_SystemFont_weather")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_StandUp.userControl_SystemFont_goal,
                    userControl_SystemFont_Group_StandUp_AOD.userControl_SystemFont_goal);
            }
            if (type.Name == "UserControl_FontRotate")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_StandUp.userControl_FontRotate,
                    userControl_SystemFont_Group_StandUp_AOD.userControl_FontRotate);
            }
            if (type.Name == "UserControl_FontRotate_weather")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_StandUp.userControl_FontRotate_goal,
                    userControl_SystemFont_Group_StandUp_AOD.userControl_FontRotate_goal);
            }

        }
        private void userControl_icon_StandUp_AOD_Copy(object sender, EventArgs eventArgs)
        {

            PreviewView = false;
            Copy_icon_AOD(userControl_icon_StandUp, userControl_icon_StandUp_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }

        private void userControl_pictures_Weather_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_pictures_AOD(userControl_pictures_weather, userControl_pictures_weather_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_text_Weather_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_text_AOD(userControl_text_weather_Current, userControl_text_weather_Current_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_text_WeatherMin_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_text_AOD(userControl_text_weather_Min, userControl_text_weather_Min_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_text_WeatherMax_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_text_AOD(userControl_text_weather_Max, userControl_text_weather_Max_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_hand_Weather_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_hand_AOD(userControl_hand_Weather, userControl_hand_Weather_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleCircle_Weather_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleCircle_AOD(userControl_scaleCircle_Weather, userControl_scaleCircle_Weather_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleLinear_Weather_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleLinear_AOD(userControl_scaleLinear_Weather, userControl_scaleLinear_Weather_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_SystemFont_Group_Weather_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Type type = sender.GetType();
            if (type.Name == "UserControl_SystemFont_weather")
            {
                UserControl_SystemFont_weather userControl_SystemFont_weather =
                    sender as UserControl_SystemFont_weather;
                if (userControl_SystemFont_weather.Name == "userControl_SystemFont_weather_Current")
                {
                    Copy_SystemFont_AOD(userControl_SystemFont_GroupWeather.userControl_SystemFont_weather_Current, 
                        userControl_SystemFont_GroupWeather_AOD.userControl_SystemFont_weather_Current);
                }
                if (userControl_SystemFont_weather.Name == "userControl_SystemFont_weather_Min")
                {
                    Copy_SystemFont_AOD(userControl_SystemFont_GroupWeather.userControl_SystemFont_weather_Min,
                        userControl_SystemFont_GroupWeather_AOD.userControl_SystemFont_weather_Min);
                }
                if (userControl_SystemFont_weather.Name == "userControl_SystemFont_weather_Max")
                {
                    Copy_SystemFont_AOD(userControl_SystemFont_GroupWeather.userControl_SystemFont_weather_Max,
                        userControl_SystemFont_GroupWeather_AOD.userControl_SystemFont_weather_Max);
                }
            }
            if (type.Name == "UserControl_FontRotate_weather")
            {
                UserControl_FontRotate_weather userControl_FontRotate_weather =
                    sender as UserControl_FontRotate_weather;
                if (userControl_FontRotate_weather.Name == "userControl_FontRotate_weather_Current")
                {
                    Copy_FontRotate_AOD(userControl_SystemFont_GroupWeather.userControl_FontRotate_weather_Current,
                        userControl_SystemFont_GroupWeather_AOD.userControl_FontRotate_weather_Current);
                }
                if (userControl_FontRotate_weather.Name == "userControl_FontRotate_weather_Min")
                {
                    Copy_FontRotate_AOD(userControl_SystemFont_GroupWeather.userControl_FontRotate_weather_Min,
                        userControl_SystemFont_GroupWeather_AOD.userControl_FontRotate_weather_Min);
                }
                if (userControl_FontRotate_weather.Name == "userControl_FontRotate_weather_Max")
                {
                    Copy_FontRotate_AOD(userControl_SystemFont_GroupWeather.userControl_FontRotate_weather_Max,
                        userControl_SystemFont_GroupWeather_AOD.userControl_FontRotate_weather_Max);
                }
            }

        }
        private void userControl_icon_Weather_AOD_Copy(object sender, EventArgs eventArgs)
        {

            PreviewView = false;
            Copy_icon_AOD(userControl_icon_Weather, userControl_icon_Weather_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }

        private void userControl_pictures_UVindex_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_pictures_AOD(userControl_pictures_UVindex, userControl_pictures_UVindex_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_text_UVindex_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_text_AOD(userControl_text_UVindex, userControl_text_UVindex_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_hand_UVindex_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_hand_AOD(userControl_hand_UVindex, userControl_hand_UVindex_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleCircle_UVindex_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleCircle_AOD(userControl_scaleCircle_UVindex, userControl_scaleCircle_UVindex_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleLinear_UVindex_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleLinear_AOD(userControl_scaleLinear_UVindex, userControl_scaleLinear_UVindex_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_SystemFont_Group_UVindex_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Type type = sender.GetType();
            if (type.Name == "UserControl_SystemFont")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_UVindex.userControl_SystemFont,
                    userControl_SystemFont_Group_UVindex_AOD.userControl_SystemFont);
            }
            if (type.Name == "UserControl_SystemFont_weather")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_UVindex.userControl_SystemFont_goal,
                    userControl_SystemFont_Group_UVindex_AOD.userControl_SystemFont_goal);
            }
            if (type.Name == "UserControl_FontRotate")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_UVindex.userControl_FontRotate,
                    userControl_SystemFont_Group_UVindex_AOD.userControl_FontRotate);
            }
            if (type.Name == "UserControl_FontRotate_weather")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_UVindex.userControl_FontRotate_goal,
                    userControl_SystemFont_Group_UVindex_AOD.userControl_FontRotate_goal);
            }

        }
        private void userControl_icon_UVindex_AOD_Copy(object sender, EventArgs eventArgs)
        {

            PreviewView = false;
            Copy_icon_AOD(userControl_icon_UVindex, userControl_icon_UVindex_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }

        private void userControl_pictures_AirQuality_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_pictures_AOD(userControl_pictures_AirQuality, userControl_pictures_AirQuality_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_text_AirQuality_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_text_AOD(userControl_text_AirQuality, userControl_text_AirQuality_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_hand_AirQuality_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_hand_AOD(userControl_hand_AirQuality, userControl_hand_AirQuality_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleCircle_AirQuality_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleCircle_AOD(userControl_scaleCircle_AirQuality, userControl_scaleCircle_AirQuality_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleLinear_AirQuality_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleLinear_AOD(userControl_scaleLinear_AirQuality, userControl_scaleLinear_AirQuality_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_SystemFont_Group_AirQuality_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Type type = sender.GetType();
            if (type.Name == "UserControl_SystemFont")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_AirQuality.userControl_SystemFont,
                    userControl_SystemFont_Group_AirQuality_AOD.userControl_SystemFont);
            }
            if (type.Name == "UserControl_SystemFont_weather")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_AirQuality.userControl_SystemFont_goal,
                    userControl_SystemFont_Group_AirQuality_AOD.userControl_SystemFont_goal);
            }
            if (type.Name == "UserControl_FontRotate")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_AirQuality.userControl_FontRotate,
                    userControl_SystemFont_Group_AirQuality_AOD.userControl_FontRotate);
            }
            if (type.Name == "UserControl_FontRotate_weather")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_AirQuality.userControl_FontRotate_goal,
                    userControl_SystemFont_Group_AirQuality_AOD.userControl_FontRotate_goal);
            }

        }
        private void userControl_icon_AirQuality_AOD_Copy(object sender, EventArgs eventArgs)
        {

            PreviewView = false;
            Copy_icon_AOD(userControl_icon_AirQuality, userControl_icon_AirQuality_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }

        private void userControl_pictures_Humidity_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_pictures_AOD(userControl_pictures_Humidity, userControl_pictures_Humidity_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_text_Humidity_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_text_AOD(userControl_text_Humidity, userControl_text_Humidity_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_hand_Humidity_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_hand_AOD(userControl_hand_Humidity, userControl_hand_Humidity_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleCircle_Humidity_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleCircle_AOD(userControl_scaleCircle_Humidity, userControl_scaleCircle_Humidity_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleLinear_Humidity_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleLinear_AOD(userControl_scaleLinear_Humidity, userControl_scaleLinear_Humidity_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_SystemFont_Group_Humidity_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Type type = sender.GetType();
            if (type.Name == "UserControl_SystemFont")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_Humidity.userControl_SystemFont,
                    userControl_SystemFont_Group_Humidity_AOD.userControl_SystemFont);
            }
            if (type.Name == "UserControl_SystemFont_weather")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_Humidity.userControl_SystemFont_goal,
                    userControl_SystemFont_Group_Humidity_AOD.userControl_SystemFont_goal);
            }
            if (type.Name == "UserControl_FontRotate")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_Humidity.userControl_FontRotate,
                    userControl_SystemFont_Group_Humidity_AOD.userControl_FontRotate);
            }
            if (type.Name == "UserControl_FontRotate_weather")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_Humidity.userControl_FontRotate_goal,
                    userControl_SystemFont_Group_Humidity_AOD.userControl_FontRotate_goal);
            }

        }
        private void userControl_icon_Humidity_AOD_Copy(object sender, EventArgs eventArgs)
        {

            PreviewView = false;
            Copy_icon_AOD(userControl_icon_Humidity, userControl_icon_Humidity_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }

        private void userControl_pictures_Sunrise_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_pictures_AOD(userControl_pictures_Sunrise, userControl_pictures_Sunrise_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_text_SunriseSunset_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_text_AOD(userControl_text_SunriseSunset, userControl_text_SunriseSunset_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_text_Sunrise_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_text_AOD(userControl_text_Sunrise, userControl_text_Sunrise_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_text_Sunset_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_text_AOD(userControl_text_Sunset, userControl_text_Sunset_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_hand_Sunrise_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_hand_AOD(userControl_hand_Sunrise, userControl_hand_Sunrise_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleCircle_Sunrise_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleCircle_AOD(userControl_scaleCircle_Sunrise, userControl_scaleCircle_Sunrise_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleLinear_Sunrise_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleLinear_AOD(userControl_scaleLinear_Sunrise, userControl_scaleLinear_Sunrise_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_SystemFont_Group_Sunrise_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Type type = sender.GetType();
            if (type.Name == "UserControl_SystemFont_weather")
            {
                UserControl_SystemFont_weather userControl_SystemFont_weather =
                    sender as UserControl_SystemFont_weather;
                if (userControl_SystemFont_weather.Name == "userControl_SystemFont_weather_Current")
                {
                    Copy_SystemFont_AOD(userControl_SystemFont_GroupSunrise.userControl_SystemFont_weather_Current,
                        userControl_SystemFont_GroupSunrise_AOD.userControl_SystemFont_weather_Current);
                }
                if (userControl_SystemFont_weather.Name == "userControl_SystemFont_weather_Min")
                {
                    Copy_SystemFont_AOD(userControl_SystemFont_GroupSunrise.userControl_SystemFont_weather_Min,
                        userControl_SystemFont_GroupSunrise_AOD.userControl_SystemFont_weather_Min);
                }
                if (userControl_SystemFont_weather.Name == "userControl_SystemFont_weather_Max")
                {
                    Copy_SystemFont_AOD(userControl_SystemFont_GroupSunrise.userControl_SystemFont_weather_Max,
                        userControl_SystemFont_GroupSunrise_AOD.userControl_SystemFont_weather_Max);
                }
            }
            if (type.Name == "UserControl_FontRotate_weather")
            {
                UserControl_FontRotate_weather userControl_FontRotate_weather =
                    sender as UserControl_FontRotate_weather;
                if (userControl_FontRotate_weather.Name == "userControl_FontRotate_weather_Current")
                {
                    Copy_FontRotate_AOD(userControl_SystemFont_GroupSunrise.userControl_FontRotate_weather_Current,
                        userControl_SystemFont_GroupSunrise_AOD.userControl_FontRotate_weather_Current);
                }
                if (userControl_FontRotate_weather.Name == "userControl_FontRotate_weather_Min")
                {
                    Copy_FontRotate_AOD(userControl_SystemFont_GroupSunrise.userControl_FontRotate_weather_Min,
                        userControl_SystemFont_GroupSunrise_AOD.userControl_FontRotate_weather_Min);
                }
                if (userControl_FontRotate_weather.Name == "userControl_FontRotate_weather_Max")
                {
                    Copy_FontRotate_AOD(userControl_SystemFont_GroupSunrise.userControl_FontRotate_weather_Max,
                        userControl_SystemFont_GroupSunrise_AOD.userControl_FontRotate_weather_Max);
                }
            }

        }
        private void userControl_icon_Sunrise_AOD_Copy(object sender, EventArgs eventArgs)
        {

            PreviewView = false;
            Copy_icon_AOD(userControl_icon_Sunrise, userControl_icon_Sunrise_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }

        private void userControl_pictures_WindForce_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_pictures_AOD(userControl_pictures_WindForce, userControl_pictures_WindForce_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_text_WindForce_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_text_AOD(userControl_text_WindForce, userControl_text_WindForce_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_hand_WindForce_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_hand_AOD(userControl_hand_WindForce, userControl_hand_WindForce_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleCircle_WindForce_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleCircle_AOD(userControl_scaleCircle_WindForce, userControl_scaleCircle_WindForce_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleLinear_WindForce_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleLinear_AOD(userControl_scaleLinear_WindForce, userControl_scaleLinear_WindForce_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_SystemFont_Group_WindForce_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Type type = sender.GetType();
            if (type.Name == "UserControl_SystemFont")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_WindForce.userControl_SystemFont,
                    userControl_SystemFont_Group_WindForce_AOD.userControl_SystemFont);
            }
            if (type.Name == "UserControl_SystemFont_weather")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_WindForce.userControl_SystemFont_goal,
                    userControl_SystemFont_Group_WindForce_AOD.userControl_SystemFont_goal);
            }
            if (type.Name == "UserControl_FontRotate")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_WindForce.userControl_FontRotate,
                    userControl_SystemFont_Group_WindForce_AOD.userControl_FontRotate);
            }
            if (type.Name == "UserControl_FontRotate_weather")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_WindForce.userControl_FontRotate_goal,
                    userControl_SystemFont_Group_WindForce_AOD.userControl_FontRotate_goal);
            }

        }
        private void userControl_icon_WindForce_AOD_Copy(object sender, EventArgs eventArgs)
        {

            PreviewView = false;
            Copy_icon_AOD(userControl_icon_WindForce, userControl_icon_WindForce_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }

        private void userControl_pictures_Altitude_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_pictures_AOD(userControl_pictures_Altitude, userControl_pictures_Altitude_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_text_Altitude_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_text_AOD(userControl_text_Altitude, userControl_text_Altitude_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_hand_Altitude_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_hand_AOD(userControl_hand_Altitude, userControl_hand_Altitude_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleCircle_Altitude_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleCircle_AOD(userControl_scaleCircle_Altitude, userControl_scaleCircle_Altitude_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleLinear_Altitude_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleLinear_AOD(userControl_scaleLinear_Altitude, userControl_scaleLinear_Altitude_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_SystemFont_Group_Altitude_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Type type = sender.GetType();
            if (type.Name == "UserControl_SystemFont")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_Altitude.userControl_SystemFont,
                    userControl_SystemFont_Group_Altitude_AOD.userControl_SystemFont);
            }
            if (type.Name == "UserControl_SystemFont_weather")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_Altitude.userControl_SystemFont_goal,
                    userControl_SystemFont_Group_Altitude_AOD.userControl_SystemFont_goal);
            }
            if (type.Name == "UserControl_FontRotate")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_Altitude.userControl_FontRotate,
                    userControl_SystemFont_Group_Altitude_AOD.userControl_FontRotate);
            }
            if (type.Name == "UserControl_FontRotate_weather")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_Altitude.userControl_FontRotate_goal,
                    userControl_SystemFont_Group_Altitude_AOD.userControl_FontRotate_goal);
            }

        }
        private void userControl_icon_Altitude_AOD_Copy(object sender, EventArgs eventArgs)
        {

            PreviewView = false;
            Copy_icon_AOD(userControl_icon_Altitude, userControl_icon_Altitude_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }

        private void userControl_pictures_AirPressure_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_pictures_AOD(userControl_pictures_AirPressure, userControl_pictures_AirPressure_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_text_AirPressure_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_text_AOD(userControl_text_AirPressure, userControl_text_AirPressure_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_hand_AirPressure_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_hand_AOD(userControl_hand_AirPressure, userControl_hand_AirPressure_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleCircle_AirPressure_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleCircle_AOD(userControl_scaleCircle_AirPressure, userControl_scaleCircle_AirPressure_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleLinear_AirPressure_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleLinear_AOD(userControl_scaleLinear_AirPressure, userControl_scaleLinear_AirPressure_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_SystemFont_Group_AirPressure_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Type type = sender.GetType();
            if (type.Name == "UserControl_SystemFont")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_AirPressure.userControl_SystemFont,
                    userControl_SystemFont_Group_AirPressure_AOD.userControl_SystemFont);
            }
            if (type.Name == "UserControl_SystemFont_weather")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_AirPressure.userControl_SystemFont_goal,
                    userControl_SystemFont_Group_AirPressure_AOD.userControl_SystemFont_goal);
            }
            if (type.Name == "UserControl_FontRotate")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_AirPressure.userControl_FontRotate,
                    userControl_SystemFont_Group_AirPressure_AOD.userControl_FontRotate);
            }
            if (type.Name == "UserControl_FontRotate_weather")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_AirPressure.userControl_FontRotate_goal,
                    userControl_SystemFont_Group_AirPressure_AOD.userControl_FontRotate_goal);
            }

        }
        private void userControl_icon_AirPressure_AOD_Copy(object sender, EventArgs eventArgs)
        {

            PreviewView = false;
            Copy_icon_AOD(userControl_icon_AirPressure, userControl_icon_AirPressure_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }

        private void userControl_pictures_Stress_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_pictures_AOD(userControl_pictures_Stress, userControl_pictures_Stress_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_text_Stress_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_text_AOD(userControl_text_Stress, userControl_text_Stress_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_hand_Stress_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_hand_AOD(userControl_hand_Stress, userControl_hand_Stress_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleCircle_Stress_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleCircle_AOD(userControl_scaleCircle_Stress, userControl_scaleCircle_Stress_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleLinear_Stress_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleLinear_AOD(userControl_scaleLinear_Stress, userControl_scaleLinear_Stress_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_SystemFont_Group_Stress_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Type type = sender.GetType();
            if (type.Name == "UserControl_SystemFont")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_Stress.userControl_SystemFont,
                    userControl_SystemFont_Group_Stress_AOD.userControl_SystemFont);
            }
            if (type.Name == "UserControl_SystemFont_weather")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_Stress.userControl_SystemFont_goal,
                    userControl_SystemFont_Group_Stress_AOD.userControl_SystemFont_goal);
            }
            if (type.Name == "UserControl_FontRotate")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_Stress.userControl_FontRotate,
                    userControl_SystemFont_Group_Stress_AOD.userControl_FontRotate);
            }
            if (type.Name == "UserControl_FontRotate_weather")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_Stress.userControl_FontRotate_goal,
                    userControl_SystemFont_Group_Stress_AOD.userControl_FontRotate_goal);
            }

        }
        private void userControl_icon_Stress_AOD_Copy(object sender, EventArgs eventArgs)
        {

            PreviewView = false;
            Copy_icon_AOD(userControl_icon_Stress, userControl_icon_Stress_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }

        private void userControl_pictures_ActivityGoal_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_pictures_AOD(userControl_pictures_ActivityGoal, userControl_pictures_ActivityGoal_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_text_ActivityGoal_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_text_AOD(userControl_text_ActivityGoal, userControl_text_ActivityGoal_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_text_goal_ActivityGoal_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_text_AOD(userControl_text_goal_ActivityGoal, userControl_text_goal_ActivityGoal_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_hand_ActivityGoal_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_hand_AOD(userControl_hand_ActivityGoal, userControl_hand_ActivityGoal_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleCircle_ActivityGoal_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleCircle_AOD(userControl_scaleCircle_ActivityGoal, userControl_scaleCircle_ActivityGoal_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleLinear_ActivityGoal_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleLinear_AOD(userControl_scaleLinear_ActivityGoal, userControl_scaleLinear_ActivityGoal_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_SystemFont_Group_ActivityGoal_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Type type = sender.GetType();
            if (type.Name == "UserControl_SystemFont")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_ActivityGoal.userControl_SystemFont,
                    userControl_SystemFont_Group_ActivityGoal_AOD.userControl_SystemFont);
            }
            if (type.Name == "UserControl_SystemFont_weather")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_ActivityGoal.userControl_SystemFont_goal,
                    userControl_SystemFont_Group_ActivityGoal_AOD.userControl_SystemFont_goal);
            }
            if (type.Name == "UserControl_FontRotate")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_ActivityGoal.userControl_FontRotate,
                    userControl_SystemFont_Group_ActivityGoal_AOD.userControl_FontRotate);
            }
            if (type.Name == "UserControl_FontRotate_weather")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_ActivityGoal.userControl_FontRotate_goal,
                    userControl_SystemFont_Group_ActivityGoal_AOD.userControl_FontRotate_goal);
            }

        }
        private void userControl_icon_ActivityGoal_AOD_Copy(object sender, EventArgs eventArgs)
        {

            PreviewView = false;
            Copy_icon_AOD(userControl_icon_ActivityGoal, userControl_icon_ActivityGoal_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }

        private void userControl_pictures_FatBurning_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_pictures_AOD(userControl_pictures_FatBurning, userControl_pictures_FatBurning_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_text_FatBurning_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_text_AOD(userControl_text_FatBurning, userControl_text_FatBurning_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_text_goal_FatBurning_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_text_AOD(userControl_text_goal_FatBurning, userControl_text_goal_FatBurning_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_hand_FatBurning_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_hand_AOD(userControl_hand_FatBurning, userControl_hand_FatBurning_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleCircle_FatBurning_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleCircle_AOD(userControl_scaleCircle_FatBurning, userControl_scaleCircle_FatBurning_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_scaleLinear_FatBurning_AOD_Copy(object sender, EventArgs eventArgs)
        {
            PreviewView = false;
            Copy_scaleLinear_AOD(userControl_scaleLinear_FatBurning, userControl_scaleLinear_FatBurning_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }
        private void userControl_SystemFont_Group_FatBurning_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Type type = sender.GetType();
            if (type.Name == "UserControl_SystemFont")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_FatBurning.userControl_SystemFont,
                    userControl_SystemFont_Group_FatBurning_AOD.userControl_SystemFont);
            }
            if (type.Name == "UserControl_SystemFont_weather")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_FatBurning.userControl_SystemFont_goal,
                    userControl_SystemFont_Group_FatBurning_AOD.userControl_SystemFont_goal);
            }
            if (type.Name == "UserControl_FontRotate")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_FatBurning.userControl_FontRotate,
                    userControl_SystemFont_Group_FatBurning_AOD.userControl_FontRotate);
            }
            if (type.Name == "UserControl_FontRotate_weather")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_FatBurning.userControl_FontRotate_goal,
                    userControl_SystemFont_Group_FatBurning_AOD.userControl_FontRotate_goal);
            }

        }
        private void userControl_icon_FatBurning_AOD_Copy(object sender, EventArgs eventArgs)
        {

            PreviewView = false;
            Copy_icon_AOD(userControl_icon_FatBurning, userControl_icon_FatBurning_AOD);
            PreviewView = true;
            JSON_write();
            PreviewImage();
        }

        private void userControl_SystemFont_GroupTime_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Type type = sender.GetType();
            if (type.Name == "UserControl_SystemFont_weather")
            {
                UserControl_SystemFont_weather userControl_SystemFont_weather =
                    sender as UserControl_SystemFont_weather;
                if (userControl_SystemFont_weather.Name == "userControl_SystemFont_weather_Current")
                {
                    Copy_SystemFont_AOD(userControl_SystemFont_GroupTime.userControl_SystemFont_weather_Current,
                        userControl_SystemFont_GroupTime_AOD.userControl_SystemFont_weather_Current);
                }
                if (userControl_SystemFont_weather.Name == "userControl_SystemFont_weather_Min")
                {
                    Copy_SystemFont_AOD(userControl_SystemFont_GroupTime.userControl_SystemFont_weather_Min,
                        userControl_SystemFont_GroupTime_AOD.userControl_SystemFont_weather_Min);
                }
                if (userControl_SystemFont_weather.Name == "userControl_SystemFont_weather_Max")
                {
                    Copy_SystemFont_AOD(userControl_SystemFont_GroupTime.userControl_SystemFont_weather_Max,
                        userControl_SystemFont_GroupTime_AOD.userControl_SystemFont_weather_Max);
                }
            }
            if (type.Name == "UserControl_FontRotate_weather")
            {
                UserControl_FontRotate_weather userControl_FontRotate_weather =
                    sender as UserControl_FontRotate_weather;
                if (userControl_FontRotate_weather.Name == "userControl_FontRotate_weather_Current")
                {
                    Copy_FontRotate_AOD(userControl_SystemFont_GroupTime.userControl_FontRotate_weather_Current,
                        userControl_SystemFont_GroupTime_AOD.userControl_FontRotate_weather_Current);
                }
                if (userControl_FontRotate_weather.Name == "userControl_FontRotate_weather_Min")
                {
                    Copy_FontRotate_AOD(userControl_SystemFont_GroupTime.userControl_FontRotate_weather_Min,
                        userControl_SystemFont_GroupTime_AOD.userControl_FontRotate_weather_Min);
                }
                if (userControl_FontRotate_weather.Name == "userControl_FontRotate_weather_Max")
                {
                    Copy_FontRotate_AOD(userControl_SystemFont_GroupTime.userControl_FontRotate_weather_Max,
                        userControl_SystemFont_GroupTime_AOD.userControl_FontRotate_weather_Max);
                }
            }
        }

        private void userControl_SystemFont_Group_Day_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Type type = sender.GetType();
            if (type.Name == "UserControl_SystemFont")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_Day.userControl_SystemFont,
                    userControl_SystemFont_Group_Day_AOD.userControl_SystemFont);
            }
            if (type.Name == "UserControl_SystemFont_weather")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_Day.userControl_SystemFont_goal,
                    userControl_SystemFont_Group_Day_AOD.userControl_SystemFont_goal);
            }
            if (type.Name == "UserControl_FontRotate")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_Day.userControl_FontRotate,
                    userControl_SystemFont_Group_Day_AOD.userControl_FontRotate);
            }
            if (type.Name == "UserControl_FontRotate_weather")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_Day.userControl_FontRotate_goal,
                    userControl_SystemFont_Group_Day_AOD.userControl_FontRotate_goal);
            }
        }

        private void userControl_SystemFont_Group_Month_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Type type = sender.GetType();
            if (type.Name == "UserControl_SystemFont")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_Month.userControl_SystemFont,
                    userControl_SystemFont_Group_Month_AOD.userControl_SystemFont);
            }
            if (type.Name == "UserControl_SystemFont_weather")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_Month.userControl_SystemFont_goal,
                    userControl_SystemFont_Group_Month_AOD.userControl_SystemFont_goal);
            }
            if (type.Name == "UserControl_FontRotate")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_Month.userControl_FontRotate,
                    userControl_SystemFont_Group_Month_AOD.userControl_FontRotate);
            }
            if (type.Name == "UserControl_FontRotate_weather")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_Month.userControl_FontRotate_goal,
                    userControl_SystemFont_Group_Month_AOD.userControl_FontRotate_goal);
            }
        }

        private void userControl_SystemFont_Group_Year_AOD_Copy(object sender, EventArgs eventArgs)
        {
            Type type = sender.GetType();
            if (type.Name == "UserControl_SystemFont")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_Year.userControl_SystemFont,
                    userControl_SystemFont_Group_Year_AOD.userControl_SystemFont);
            }
            if (type.Name == "UserControl_SystemFont_weather")
            {
                Copy_SystemFont_AOD(userControl_SystemFont_Group_Year.userControl_SystemFont_goal,
                    userControl_SystemFont_Group_Year_AOD.userControl_SystemFont_goal);
            }
            if (type.Name == "UserControl_FontRotate")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_Year.userControl_FontRotate,
                    userControl_SystemFont_Group_Year_AOD.userControl_FontRotate);
            }
            if (type.Name == "UserControl_FontRotate_weather")
            {
                Copy_FontRotate_AOD(userControl_SystemFont_Group_Year.userControl_FontRotate_goal,
                    userControl_SystemFont_Group_Year_AOD.userControl_FontRotate_goal);
            }
        }


        #endregion



        private void radioButton_ActivityGoal_CheckedChanged(object sender, EventArgs e)
        {
            JSON_write();
            PreviewImage();
        }

        private void dataGridView_SNL_Activity_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView_SNL_Activity.SelectedCells.Count > 0)
            {
                int RowIndex = dataGridView_SNL_Activity.SelectedCells[0].RowIndex;
                int RowIndexMin = -1;
                int RowIndexMax = -1;
                for(int i=0; i < dataGridView_SNL_Activity.Rows.Count; i++)
                {
                    if (dataGridView_SNL_Activity.Rows[i].Visible)
                    {
                        if (RowIndexMin < 0) RowIndexMin = i;
                        RowIndexMax = i;
                    }
                }

                button_SNL_Activity_Start.Enabled = true;
                button_SNL_Activity_Up.Enabled = true;
                button_SNL_Activity_Down.Enabled = true;
                button_SNL_Activity_End.Enabled = true;

                if (RowIndex <= RowIndexMin)
                {
                    button_SNL_Activity_Start.Enabled = false;
                    button_SNL_Activity_Up.Enabled = false;
                }

                if (RowIndex >= RowIndexMax)
                {
                    button_SNL_Activity_Down.Enabled = false;
                    button_SNL_Activity_End.Enabled = false;
                }
            }
            else
            {
                button_SNL_Activity_Start.Enabled = false;
                button_SNL_Activity_Up.Enabled = false;
                button_SNL_Activity_Down.Enabled = false;
                button_SNL_Activity_End.Enabled = false;
            }
        }

        private void Row_FullUp(DataGridView dgv, int rowIndex)
        {
            if (rowIndex == 0) return;
            try
            {
                DataGridViewRow selectedRow = dgv.Rows[rowIndex];
                //dgv.Rows.Remove(selectedRow);
                dgv.Rows.RemoveAt(rowIndex);
                dgv.Rows.Insert(0, selectedRow);
                dgv.ClearSelection();
                dgv.Rows[0].Selected = true;
            }
            catch { }

            JSON_write();
            PreviewImage();
        }

        private void Row_Up(DataGridView dgv, int rowIndex)
        {
            if (rowIndex == 0) return;
            try
            {
                int newRowIndex = rowIndex - 1;
                while (newRowIndex > 0 && !dgv.Rows[newRowIndex].Visible)
                {
                    newRowIndex--;
                }
                DataGridViewRow selectedRow = dgv.Rows[rowIndex];
                //dgv.Rows.Remove(selectedRow);
                dgv.Rows.RemoveAt(rowIndex);
                dgv.Rows.Insert(newRowIndex, selectedRow);
                dgv.ClearSelection();
                dgv.Rows[newRowIndex].Selected = true;
            }
            catch { }

            JSON_write();
            PreviewImage();
        }

        private void Row_Down(DataGridView dgv, int rowIndex)
        {
            int totalRows = dgv.Rows.Count;
            if (rowIndex == totalRows - 1) return;
            try
            {
                int newRowIndex = rowIndex + 1;
                while (newRowIndex < dgv.Rows.Count && !dgv.Rows[newRowIndex].Visible)
                {
                    newRowIndex++;
                }
                DataGridViewRow selectedRow = dgv.Rows[rowIndex];
                //dgv.Rows.Remove(selectedRow);
                dgv.Rows.RemoveAt(rowIndex);
                dgv.Rows.Insert(newRowIndex, selectedRow);
                //dgv.Rows.Add(selectedRow);
                dgv.ClearSelection();
                dgv.Rows[newRowIndex].Selected = true;
            }
            catch { }

            JSON_write();
            PreviewImage();
        }

        private void Row_FullDown(DataGridView dgv, int rowIndex)
        {
            int totalRows = dgv.Rows.Count;
            if (rowIndex == totalRows - 1) return;
            try
            {
                DataGridViewRow selectedRow = dgv.Rows[rowIndex];
                //dgv.Rows.Remove(selectedRow);
                dgv.Rows.RemoveAt(rowIndex);
                //dgv.Rows.Insert(rowIndex + 1, selectedRow);
                dgv.Rows.Add(selectedRow);
                dgv.ClearSelection();
                dgv.Rows[totalRows - 1].Selected = true;
            }
            catch { }

            JSON_write();
            PreviewImage();
        }


        private void button_SNL_Activity_Start_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = dataGridView_SNL_Activity.SelectedCells[0].OwningRow.Index;
                Row_FullUp(dataGridView_SNL_Activity, rowIndex);
            }
            catch { }
        }

        private void button_SNL_Activity_Up_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = dataGridView_SNL_Activity.SelectedCells[0].OwningRow.Index;
                Row_Up(dataGridView_SNL_Activity, rowIndex);
            }
            catch { }
        }

        private void button_SNL_Activity_Down_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = dataGridView_SNL_Activity.SelectedCells[0].OwningRow.Index;
                Row_Down(dataGridView_SNL_Activity, rowIndex);
            }
            catch { }
        }

        private void button_SNL_Activity_End_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = dataGridView_SNL_Activity.SelectedCells[0].OwningRow.Index;
                Row_FullDown(dataGridView_SNL_Activity, rowIndex);
            }
            catch { }
        }


        private void dataGridView_SNL_Date_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView_SNL_Date.SelectedCells.Count > 0)
            {
                int RowIndex = dataGridView_SNL_Date.SelectedCells[0].RowIndex;
                int RowIndexMin = -1;
                int RowIndexMax = -1;
                for (int i = 0; i < dataGridView_SNL_Date.Rows.Count; i++)
                {
                    if (dataGridView_SNL_Date.Rows[i].Visible)
                    {
                        if (RowIndexMin < 0) RowIndexMin = i;
                        RowIndexMax = i;
                    }
                }

                button_SNL_Date_Start.Enabled = true;
                button_SNL_Date_Up.Enabled = true;
                button_SNL_Date_Down.Enabled = true;
                button_SNL_Date_End.Enabled = true;

                if (RowIndex <= RowIndexMin)
                {
                    button_SNL_Date_Start.Enabled = false;
                    button_SNL_Date_Up.Enabled = false;
                }

                if (RowIndex >= RowIndexMax)
                {
                    button_SNL_Date_Down.Enabled = false;
                    button_SNL_Date_End.Enabled = false;
                }
            }
            else
            {
                button_SNL_Date_Start.Enabled = false;
                button_SNL_Date_Up.Enabled = false;
                button_SNL_Date_Down.Enabled = false;
                button_SNL_Date_End.Enabled = false;
            }
        }

        private void button_SNL_Date_Start_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = dataGridView_SNL_Date.SelectedCells[0].OwningRow.Index;
                Row_FullUp(dataGridView_SNL_Date, rowIndex);
            }
            catch { }
        }

        private void button_SNL_Date_Up_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = dataGridView_SNL_Date.SelectedCells[0].OwningRow.Index;
                Row_Up(dataGridView_SNL_Date, rowIndex);
            }
            catch { }
        }

        private void button_SNL_Date_Down_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = dataGridView_SNL_Date.SelectedCells[0].OwningRow.Index;
                Row_Down(dataGridView_SNL_Date, rowIndex);
            }
            catch { }
        }

        private void button_SNL_Date_End_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = dataGridView_SNL_Date.SelectedCells[0].OwningRow.Index;
                Row_FullDown(dataGridView_SNL_Date, rowIndex);
            }
            catch { }
        }

        private void button_AODL_Activity_Start_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = dataGridView_AODL_Activity.SelectedCells[0].OwningRow.Index;
                Row_FullUp(dataGridView_AODL_Activity, rowIndex);
            }
            catch { }
        }

        private void button_AODL_Activity_Up_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = dataGridView_AODL_Activity.SelectedCells[0].OwningRow.Index;
                Row_Up(dataGridView_AODL_Activity, rowIndex);
            }
            catch { }
        }

        private void button_AODL_Activity_Down_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = dataGridView_AODL_Activity.SelectedCells[0].OwningRow.Index;
                Row_Down(dataGridView_AODL_Activity, rowIndex);
            }
            catch { }
        }

        private void button_AODL_Activity_End_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = dataGridView_AODL_Activity.SelectedCells[0].OwningRow.Index;
                Row_FullDown(dataGridView_AODL_Activity, rowIndex);
            }
            catch { }
        }

        private void button_AODL_Date_Start_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = dataGridView_AODL_Date.SelectedCells[0].OwningRow.Index;
                Row_FullUp(dataGridView_AODL_Date, rowIndex);
            }
            catch { }
        }

        private void button_AODL_Date_Up_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = dataGridView_AODL_Date.SelectedCells[0].OwningRow.Index;
                Row_Up(dataGridView_AODL_Date, rowIndex);
            }
            catch { }
        }

        private void button_AODL_Date_Down_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = dataGridView_AODL_Date.SelectedCells[0].OwningRow.Index;
                Row_Down(dataGridView_AODL_Date, rowIndex);
            }
            catch { }
        }

        private void button_AODL_Date_End_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = dataGridView_AODL_Date.SelectedCells[0].OwningRow.Index;
                Row_FullDown(dataGridView_AODL_Date, rowIndex);
            }
            catch { }
        }

        private void textBox_WatchSkin_Path_Leave(object sender, EventArgs e)
        {
            if (radioButton_GTR2.Checked)
            {
                Program_Settings.WatchSkin_GTR_2 = textBox_WatchSkin_Path.Text;
            }
            else if (radioButton_GTS2.Checked)
            {
                Program_Settings.WatchSkin_GTS_2 = textBox_WatchSkin_Path.Text;
            }
            else if (radioButton_TRex_pro.Checked)
            {
                Program_Settings.WatchSkin_TRex_pro = textBox_WatchSkin_Path.Text;
            }
            else if (radioButton_ZeppE.Checked)
            {
                Program_Settings.WatchSkin_Zepp_E = textBox_WatchSkin_Path.Text;
            }

            string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText("Settings.json", JSON_String, Encoding.UTF8);
        }

        private void button_WatchSkin_PathGet_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = FullFileDir;
            openFileDialog.FileName = FileName;
            openFileDialog.Filter = Properties.FormStrings.FilterJson;
            //openFileDialog.Filter = "Json files (*.json) | *.json";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = false;
            openFileDialog.Title = Properties.FormStrings.Dialog_Title_WatchSkin;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Logger.WriteLine("* WatchSkin_PathGet_Click");
                string fullfilename = openFileDialog.FileName;
                if (fullfilename.IndexOf(Application.StartupPath) == 0)
                    fullfilename = fullfilename.Remove(0, Application.StartupPath.Length);
                textBox_WatchSkin_Path.Text = fullfilename;

                if (radioButton_GTR2.Checked)
                {
                    Program_Settings.WatchSkin_GTR_2 = textBox_WatchSkin_Path.Text;
                }
                else if (radioButton_GTS2.Checked)
                {
                    Program_Settings.WatchSkin_GTS_2 = textBox_WatchSkin_Path.Text;
                }
                else if (radioButton_TRex_pro.Checked)
                {
                    Program_Settings.WatchSkin_TRex_pro = textBox_WatchSkin_Path.Text;
                }
                else if (radioButton_ZeppE.Checked)
                {
                    Program_Settings.WatchSkin_Zepp_E = textBox_WatchSkin_Path.Text;
                }

                string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
                {
                    //DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
                File.WriteAllText("Settings.json", JSON_String, Encoding.UTF8);

                Logger.WriteLine("* WatchSkin_PathGet_Click_END");
            }
        }

        private void checkBox_WatchSkin_Use_CheckedChanged(object sender, EventArgs e)
        {
            bool b = checkBox_WatchSkin_Use.Checked;
            textBox_WatchSkin_Path.Enabled = b;
            button_WatchSkin_PathGet.Enabled = b;
            if (Settings_Load) return;
            Program_Settings.WatchSkin_Use = b;

            string JSON_String = JsonConvert.SerializeObject(Program_Settings, Formatting.Indented, new JsonSerializerSettings
            {
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);
        }

        private void dataGridView_AODL_Activity_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView_AODL_Activity.SelectedCells.Count > 0)
            {
                int RowIndex = dataGridView_AODL_Activity.SelectedCells[0].RowIndex;
                int RowIndexMin = -1;
                int RowIndexMax = -1;
                for (int i = 0; i < dataGridView_AODL_Activity.Rows.Count; i++)
                {
                    if (dataGridView_AODL_Activity.Rows[i].Visible)
                    {
                        if (RowIndexMin < 0) RowIndexMin = i;
                        RowIndexMax = i;
                    }
                }

                button_AODL_Activity_Start.Enabled = true;
                button_AODL_Activity_Up.Enabled = true;
                button_AODL_Activity_Down.Enabled = true;
                button_AODL_Activity_End.Enabled = true;

                if (RowIndex <= RowIndexMin)
                {
                    button_AODL_Activity_Start.Enabled = false;
                    button_AODL_Activity_Up.Enabled = false;
                }

                if (RowIndex >= RowIndexMax)
                {
                    button_AODL_Activity_Down.Enabled = false;
                    button_AODL_Activity_End.Enabled = false;
                }
            }
            else
            {
                button_AODL_Activity_Start.Enabled = false;
                button_AODL_Activity_Up.Enabled = false;
                button_AODL_Activity_Down.Enabled = false;
                button_AODL_Activity_End.Enabled = false;
            }
        }

        private void dataGridView_AODL_Date_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView_AODL_Date.SelectedCells.Count > 0)
            {
                int RowIndex = dataGridView_AODL_Date.SelectedCells[0].RowIndex;
                int RowIndexMin = -1;
                int RowIndexMax = -1;
                for (int i = 0; i < dataGridView_AODL_Date.Rows.Count; i++)
                {
                    if (dataGridView_AODL_Date.Rows[i].Visible)
                    {
                        if (RowIndexMin < 0) RowIndexMin = i;
                        RowIndexMax = i;
                    }
                }

                button_AODL_Date_Start.Enabled = true;
                button_AODL_Date_Up.Enabled = true;
                button_AODL_Date_Down.Enabled = true;
                button_AODL_Date_End.Enabled = true;

                if (RowIndex <= RowIndexMin)
                {
                    button_AODL_Date_Start.Enabled = false;
                    button_AODL_Date_Up.Enabled = false;
                }

                if (RowIndex >= RowIndexMax)
                {
                    button_AODL_Date_Down.Enabled = false;
                    button_AODL_Date_End.Enabled = false;
                }
            }
            else
            {
                button_AODL_Date_Start.Enabled = false;
                button_AODL_Date_Up.Enabled = false;
                button_AODL_Date_Down.Enabled = false;
                button_AODL_Date_End.Enabled = false;
            }
        }

        private void dataGridView_SNL_Activity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.PageUp)
            {
                e.SuppressKeyPress = true;
                try
                {
                    if (dataGridView_SNL_Activity.SelectedCells.Count > 0)
                    {
                        int rowIndex = dataGridView_SNL_Activity.SelectedCells[0].OwningRow.Index;
                        Row_Up(dataGridView_SNL_Activity, rowIndex);
                    }
                }
                catch { }
                return;
            }
            if (e.KeyCode == Keys.PageDown)
            {
                e.SuppressKeyPress = true;
                try
                {
                    if (dataGridView_SNL_Activity.SelectedCells.Count > 0)
                    {
                        int rowIndex = dataGridView_SNL_Activity.SelectedCells[0].OwningRow.Index;
                        Row_Down(dataGridView_SNL_Activity, rowIndex); 
                    }
                }
                catch { }
                return;
            }
            if (e.KeyCode == Keys.Home)
            {
                e.SuppressKeyPress = true;
                try
                {
                    if (dataGridView_SNL_Activity.SelectedCells.Count > 0)
                    {
                        int rowIndex = dataGridView_SNL_Activity.SelectedCells[0].OwningRow.Index;
                        Row_FullUp(dataGridView_SNL_Activity, rowIndex); 
                    }
                }
                catch { }
                return;
            }
            if (e.KeyCode == Keys.End)
            {
                e.SuppressKeyPress = true;
                try
                {
                    if (dataGridView_SNL_Activity.SelectedCells.Count > 0)
                    {
                        int rowIndex = dataGridView_SNL_Activity.SelectedCells[0].OwningRow.Index;
                        Row_FullDown(dataGridView_SNL_Activity, rowIndex); 
                    }
                }
                catch { }
                return;
            }
        }

        private void dataGridView_SNL_Date_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.PageUp)
            {
                e.SuppressKeyPress = true;
                try
                {
                    if (dataGridView_SNL_Date.SelectedCells.Count > 0)
                    {
                        int rowIndex = dataGridView_SNL_Date.SelectedCells[0].OwningRow.Index;
                        Row_Up(dataGridView_SNL_Date, rowIndex);
                    }
                }
                catch { }
                return;
            }
            if (e.KeyCode == Keys.PageDown)
            {
                e.SuppressKeyPress = true;
                try
                {
                    if (dataGridView_SNL_Date.SelectedCells.Count > 0)
                    {
                        int rowIndex = dataGridView_SNL_Date.SelectedCells[0].OwningRow.Index;
                        Row_Down(dataGridView_SNL_Date, rowIndex);
                    }
                }
                catch { }
                return;
            }
            if (e.KeyCode == Keys.Home)
            {
                e.SuppressKeyPress = true;
                try
                {
                    if (dataGridView_SNL_Date.SelectedCells.Count > 0)
                    {
                        int rowIndex = dataGridView_SNL_Date.SelectedCells[0].OwningRow.Index;
                        Row_FullUp(dataGridView_SNL_Date, rowIndex);
                    }
                }
                catch { }
                return;
            }
            if (e.KeyCode == Keys.End)
            {
                e.SuppressKeyPress = true;
                try
                {
                    if (dataGridView_SNL_Date.SelectedCells.Count > 0)
                    {
                        int rowIndex = dataGridView_SNL_Date.SelectedCells[0].OwningRow.Index;
                        Row_FullDown(dataGridView_SNL_Date, rowIndex);
                    }
                }
                catch { }
                return;
            }
        }

        private void dataGridView_AODL_Activity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.PageUp)
            {
                e.SuppressKeyPress = true;
                try
                {
                    if (dataGridView_AODL_Activity.SelectedCells.Count > 0)
                    {
                        int rowIndex = dataGridView_AODL_Activity.SelectedCells[0].OwningRow.Index;
                        Row_Up(dataGridView_AODL_Activity, rowIndex);
                    }
                }
                catch { }
                return;
            }
            if (e.KeyCode == Keys.PageDown)
            {
                e.SuppressKeyPress = true;
                try
                {
                    if (dataGridView_AODL_Activity.SelectedCells.Count > 0)
                    {
                        int rowIndex = dataGridView_AODL_Activity.SelectedCells[0].OwningRow.Index;
                        Row_Down(dataGridView_AODL_Activity, rowIndex);
                    }
                }
                catch { }
                return;
            }
            if (e.KeyCode == Keys.Home)
            {
                e.SuppressKeyPress = true;
                try
                {
                    if (dataGridView_AODL_Activity.SelectedCells.Count > 0)
                    {
                        int rowIndex = dataGridView_AODL_Activity.SelectedCells[0].OwningRow.Index;
                        Row_FullUp(dataGridView_AODL_Activity, rowIndex);
                    }
                }
                catch { }
                return;
            }
            if (e.KeyCode == Keys.End)
            {
                e.SuppressKeyPress = true;
                try
                {
                    if (dataGridView_AODL_Activity.SelectedCells.Count > 0)
                    {
                        int rowIndex = dataGridView_AODL_Activity.SelectedCells[0].OwningRow.Index;
                        Row_FullDown(dataGridView_AODL_Activity, rowIndex);
                    }
                }
                catch { }
                return;
            }
        }

        private void dataGridView_AODL_Date_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.PageUp)
            {
                e.SuppressKeyPress = true;
                try
                {
                    if (dataGridView_AODL_Date.SelectedCells.Count > 0)
                    {
                        int rowIndex = dataGridView_AODL_Date.SelectedCells[0].OwningRow.Index;
                        Row_Up(dataGridView_AODL_Date, rowIndex);
                    }
                }
                catch { }
                return;
            }
            if (e.KeyCode == Keys.PageDown)
            {
                e.SuppressKeyPress = true;
                try
                {
                    if (dataGridView_AODL_Date.SelectedCells.Count > 0)
                    {
                        int rowIndex = dataGridView_AODL_Date.SelectedCells[0].OwningRow.Index;
                        Row_Down(dataGridView_AODL_Date, rowIndex);
                    }
                }
                catch { }
                return;
            }
            if (e.KeyCode == Keys.Home)
            {
                e.SuppressKeyPress = true;
                try
                {
                    if (dataGridView_AODL_Date.SelectedCells.Count > 0)
                    {
                        int rowIndex = dataGridView_AODL_Date.SelectedCells[0].OwningRow.Index;
                        Row_FullUp(dataGridView_AODL_Date, rowIndex);
                    }
                }
                catch { }
                return;
            }
            if (e.KeyCode == Keys.End)
            {
                e.SuppressKeyPress = true;
                try
                {
                    if (dataGridView_AODL_Date.SelectedCells.Count > 0)
                    {
                        int rowIndex = dataGridView_AODL_Date.SelectedCells[0].OwningRow.Index;
                        Row_FullDown(dataGridView_AODL_Date, rowIndex);
                    }
                }
                catch { }
                return;
            }
        }

        private void button_WidgetElement_Start_Click(object sender, EventArgs e)
        {
            PreviewView = false;
            try
            {
                int rowIndex = dataGridView_WidgetElement.SelectedCells[0].OwningRow.Index;
                WidgetRow_FullUp(dataGridView_WidgetElement, rowIndex);
            }
            catch { }
            PreviewView = true;
            PreviewImage();
        }

        private void button_WidgetElement_Up_Click(object sender, EventArgs e)
        {
            PreviewView = false;
            try
            {
                int rowIndex = dataGridView_WidgetElement.SelectedCells[0].OwningRow.Index;
                WidgetRow_Up(dataGridView_WidgetElement, rowIndex);
            }
            catch { }
            PreviewView = true;
            PreviewImage();
        }

        private void button_WidgetElement_Down_Click(object sender, EventArgs e)
        {
            PreviewView = false;
            try
            {
                int rowIndex = dataGridView_WidgetElement.SelectedCells[0].OwningRow.Index;
                WidgetRow_Down(dataGridView_WidgetElement, rowIndex);
            }
            catch { }
            PreviewView = true;
            PreviewImage();
        }

        private void button_WidgetElement_End_Click(object sender, EventArgs e)
        {
            PreviewView = false;
            try
            {
                int rowIndex = dataGridView_WidgetElement.SelectedCells[0].OwningRow.Index;
                WidgetRow_FullDown(dataGridView_WidgetElement, rowIndex);
            }
            catch { }
            PreviewView = true;
            PreviewImage();
        }

        private void WidgetRow_FullUp(DataGridView dgv, int rowIndex, bool removWidgetElement = true)
        {
            if (rowIndex == 0) return;
            try
            {
                DataGridViewRow selectedRow = dgv.Rows[rowIndex];
                dgv.Rows.RemoveAt(rowIndex);
                dgv.Rows.Insert(0, selectedRow);
                dgv.ClearSelection();
                dgv.Rows[0].Selected = true;

                if (removWidgetElement)
                {
                    int selectedWidget = comboBox_WidgetNumber.SelectedIndex;
                    if (selectedWidget < 0) return;
                    WidgetElement selectedWidgetElement = WidgetsTemp.Widget[selectedWidget].WidgetElement[rowIndex];
                    WidgetsTemp.Widget[selectedWidget].WidgetElement.RemoveAt(rowIndex);
                    WidgetsTemp.Widget[selectedWidget].WidgetElement.Insert(0, selectedWidgetElement); 
                }
            }
            catch { }

            JSON_write();
        }

        private void WidgetRow_Up(DataGridView dgv, int rowIndex, bool removWidgetElement = true)
        {
            if (rowIndex == 0) return;
            try
            {
                DataGridViewRow selectedRow = dgv.Rows[rowIndex];
                dgv.Rows.RemoveAt(rowIndex);
                dgv.Rows.Insert(rowIndex - 1, selectedRow);
                dgv.ClearSelection();
                dgv.Rows[rowIndex - 1].Selected = true;

                if (removWidgetElement)
                {
                    int selectedWidget = comboBox_WidgetNumber.SelectedIndex;
                    if (selectedWidget < 0) return;
                    WidgetElement selectedWidgetElement = WidgetsTemp.Widget[selectedWidget].WidgetElement[rowIndex];
                    WidgetsTemp.Widget[selectedWidget].WidgetElement.RemoveAt(rowIndex);
                    WidgetsTemp.Widget[selectedWidget].WidgetElement.Insert(rowIndex - 1, selectedWidgetElement); 
                }
            }
            catch { }

            JSON_write();
        }

        private void WidgetRow_Down(DataGridView dgv, int rowIndex, bool removWidgetElement = true)
        {
            int totalRows = dgv.Rows.Count;
            if (rowIndex == totalRows - 1) return;
            try
            {
                DataGridViewRow selectedRow = dgv.Rows[rowIndex];
                dgv.Rows.RemoveAt(rowIndex);
                dgv.Rows.Insert(rowIndex + 1, selectedRow);
                dgv.ClearSelection();
                dgv.Rows[rowIndex + 1].Selected = true;

                if (removWidgetElement)
                {
                    int selectedWidget = comboBox_WidgetNumber.SelectedIndex;
                    if (selectedWidget < 0) return;
                    WidgetElement selectedWidgetElement = WidgetsTemp.Widget[selectedWidget].WidgetElement[rowIndex];
                    WidgetsTemp.Widget[selectedWidget].WidgetElement.RemoveAt(rowIndex);
                    WidgetsTemp.Widget[selectedWidget].WidgetElement.Insert(rowIndex + 1, selectedWidgetElement); 
                }
            }
            catch { }

            JSON_write();
        }

        private void WidgetRow_FullDown(DataGridView dgv, int rowIndex, bool removWidgetElement = true)
        {
            int totalRows = dgv.Rows.Count;
            if (rowIndex == totalRows - 1) return;
            try
            {
                DataGridViewRow selectedRow = dgv.Rows[rowIndex];
                dgv.Rows.RemoveAt(rowIndex);
                dgv.Rows.Add(selectedRow);
                dgv.ClearSelection();
                dgv.Rows[totalRows - 1].Selected = true;

                if (removWidgetElement)
                {
                    int selectedWidget = comboBox_WidgetNumber.SelectedIndex;
                    if (selectedWidget < 0) return;
                    WidgetElement selectedWidgetElement = WidgetsTemp.Widget[selectedWidget].WidgetElement[rowIndex];
                    WidgetsTemp.Widget[selectedWidget].WidgetElement.RemoveAt(rowIndex);
                    WidgetsTemp.Widget[selectedWidget].WidgetElement.Add(selectedWidgetElement); 
                }
            }
            catch { }

            JSON_write();
        }

        private void dataGridView_WidgetElement_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.PageUp)
            {
                e.SuppressKeyPress = true;
                PreviewView = false;
                try
                {
                    if (dataGridView_WidgetElement.SelectedCells.Count > 0)
                    {
                        int rowIndex = dataGridView_WidgetElement.SelectedCells[0].OwningRow.Index;
                        WidgetRow_Up(dataGridView_WidgetElement, rowIndex);
                    }
                }
                catch { }
                PreviewView = true;
                PreviewImage();
                return;
            }
            if (e.KeyCode == Keys.PageDown)
            {
                e.SuppressKeyPress = true;
                PreviewView = false;
                try
                {
                    if (dataGridView_WidgetElement.SelectedCells.Count > 0)
                    {
                        int rowIndex = dataGridView_WidgetElement.SelectedCells[0].OwningRow.Index;
                        WidgetRow_Down(dataGridView_WidgetElement, rowIndex);
                    }
                }
                catch { }
                PreviewView = true;
                PreviewImage();
                return;
            }
            if (e.KeyCode == Keys.Home)
            {
                e.SuppressKeyPress = true;
                PreviewView = false;
                try
                {
                    if (dataGridView_WidgetElement.SelectedCells.Count > 0)
                    {
                        int rowIndex = dataGridView_WidgetElement.SelectedCells[0].OwningRow.Index;
                        WidgetRow_FullUp(dataGridView_WidgetElement, rowIndex);
                    }
                }
                catch { }
                PreviewView = true;
                PreviewImage();
                return;
            }
            if (e.KeyCode == Keys.End)
            {
                e.SuppressKeyPress = true;
                PreviewView = false;
                try
                {
                    if (dataGridView_WidgetElement.SelectedCells.Count > 0)
                    {
                        int rowIndex = dataGridView_WidgetElement.SelectedCells[0].OwningRow.Index;
                        WidgetRow_FullDown(dataGridView_WidgetElement, rowIndex);
                    }
                }
                catch { }
                PreviewView = true;
                PreviewImage();
                return;
            }
            if (e.KeyCode == Keys.Delete)
            {
                e.SuppressKeyPress = true;
                try
                {
                    if (dataGridView_WidgetElement.SelectedCells.Count > 0)
                    {
                        int selectedWidget = comboBox_WidgetNumber.SelectedIndex;
                        int RowIndex = dataGridView_WidgetElement.SelectedCells[0].RowIndex;
                        WidgetElementDel(selectedWidget, RowIndex);
                    }
                }
                catch { }
            }
        }

        private void comboBox_WidgetNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool widgetsExist = false;
            bool PreviewViewTemp = PreviewView;
            PreviewView = false;
            tabControl_Widget.TabStop = true;
            SelectWidgetElementAdd("Steps");
            dataGridView_WidgetElement.Rows.Clear();
            int selectedIndex = comboBox_WidgetNumber.SelectedIndex;
            if (selectedIndex >= 0)
            {
                if (Watch_Face != null && Watch_Face.Widgets != null && Watch_Face.Widgets.Widget != null &&
                    Watch_Face.Widgets.Widget.Count > selectedIndex)
                {
                    widgetsExist = true;
                    JSON_read_widgetElement_order(Watch_Face.Widgets.Widget[selectedIndex]);

                    numericUpDown_WidgetX.Value = Watch_Face.Widgets.Widget[selectedIndex].X;
                    numericUpDown_WidgetY.Value = Watch_Face.Widgets.Widget[selectedIndex].Y;
                    numericUpDown_WidgetWidth.Value = Watch_Face.Widgets.Widget[selectedIndex].Width;
                    numericUpDown_WidgetHeight.Value = Watch_Face.Widgets.Widget[selectedIndex].Height;

                    if (Watch_Face.Widgets.Widget[selectedIndex].DescriptionImageBackground != null)
                    {
                        comboBoxSetText(comboBox_WidgetDescriptionBackground,
                                        Watch_Face.Widgets.Widget[selectedIndex].DescriptionImageBackground.ImageIndex);
                        if (Watch_Face.Widgets.Widget[selectedIndex].DescriptionImageBackground.Coordinates != null)
                        {
                            numericUpDown_WidgetDescriptionBackgroundX.Value =
                                                Watch_Face.Widgets.Widget[selectedIndex].DescriptionImageBackground.Coordinates.X;
                            numericUpDown_WidgetDescriptionBackgroundY.Value =
                                                Watch_Face.Widgets.Widget[selectedIndex].DescriptionImageBackground.Coordinates.Y;
                        }
                    }
                    else
                    {

                        comboBox_WidgetDescriptionBackground.Items.Clear();
                        comboBox_WidgetDescriptionBackground.Text = "";
                        numericUpDown_WidgetDescriptionBackgroundX.Value = 0;
                        numericUpDown_WidgetDescriptionBackgroundY.Value = 0;
                    }
                    numericUpDown_WidgetDescriptionLenght.Value = Watch_Face.Widgets.Widget[selectedIndex].DescriptionWidthCheck;
                    comboBoxSetText(comboBox_WidgetBorderActiv,
                                        Watch_Face.Widgets.Widget[selectedIndex].BorderActivImageIndex);
                    comboBoxSetText(comboBox_WidgetBorderInactiv,
                                        Watch_Face.Widgets.Widget[selectedIndex].BorderInactivImageIndex);
                }
            }
            PreviewView = PreviewViewTemp;

            button_WidgetDel.Enabled = widgetsExist;
            numericUpDown_WidgetX.Enabled = widgetsExist;
            numericUpDown_WidgetY.Enabled = widgetsExist;
            numericUpDown_WidgetWidth.Enabled = widgetsExist;
            numericUpDown_WidgetHeight.Enabled = widgetsExist;
            comboBox_WidgetDescriptionBackground.Enabled = widgetsExist;
            numericUpDown_WidgetDescriptionBackgroundX.Enabled = widgetsExist;
            numericUpDown_WidgetDescriptionBackgroundY.Enabled = widgetsExist;
            numericUpDown_WidgetDescriptionLenght.Enabled = widgetsExist;
            comboBox_WidgetBorderActiv.Enabled = widgetsExist;
            comboBox_WidgetBorderInactiv.Enabled = widgetsExist;

            label02.Enabled = widgetsExist;
            label05.Enabled = widgetsExist;
            label2.Enabled = widgetsExist;
            label6.Enabled = widgetsExist;
            label8.Enabled = widgetsExist;
            label9.Enabled = widgetsExist;
            label10.Enabled = widgetsExist;
            label11.Enabled = widgetsExist;
            label1083.Enabled = widgetsExist;
            label1084.Enabled = widgetsExist;
            label1085.Enabled = widgetsExist;
            label1086.Enabled = widgetsExist;

            groupBox_WidgetElement.Enabled = widgetsExist;

        }

        private void button_WidgetDel_Click(object sender, EventArgs e)
        {
            int widgetIndex = comboBox_WidgetNumber.SelectedIndex;
            if (widgetIndex >= 0)
            {
                if (Watch_Face != null && Watch_Face.Widgets != null && Watch_Face.Widgets.Widget != null &&
                    Watch_Face.Widgets.Widget.Count > widgetIndex) WidgetDel(widgetIndex);
            }
        }

        private void button_WidgetElementDel_Click(object sender, EventArgs e)
        {
            if (dataGridView_WidgetElement.SelectedCells.Count > 0)
            {
                int widgetIndex = comboBox_WidgetNumber.SelectedIndex;
                int widgetElementIndex = dataGridView_WidgetElement.SelectedCells[0].RowIndex;
                WidgetElementDel(widgetIndex, widgetElementIndex);
            }
        }

        private void comboBox_WidgetsMask_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_WidgetsUnderMask.SelectedIndex >= 0)
            {
                if (WidgetsTemp != null && WidgetsTemp.Widget != null) WidgetsTemp.UnderMaskImageIndex = Int32.Parse(comboBox_WidgetsUnderMask.Text);
            }
            if (comboBox_WidgetsTopMask.SelectedIndex >= 0)
            {
                if (WidgetsTemp != null && WidgetsTemp.Widget != null) WidgetsTemp.TopMaskImageIndex = Int32.Parse(comboBox_WidgetsTopMask.Text);
            }

            JSON_write();
            PreviewImage();
        }

        private void dataGridView_Widget_Date_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.PageUp)
            {
                e.SuppressKeyPress = true;
                PreviewView = false;
                try
                {
                    if (dataGridView_Widget_Date.SelectedCells.Count > 0)
                    {
                        int rowIndex = dataGridView_Widget_Date.SelectedCells[0].OwningRow.Index;
                        WidgetRow_Up(dataGridView_Widget_Date, rowIndex, false);
                    }
                }
                catch { }
                PreviewView = true;
                userControl_Widget_ValueChanged(sender, e);
                PreviewImage();
                return;
            }
            if (e.KeyCode == Keys.PageDown)
            {
                e.SuppressKeyPress = true;
                PreviewView = false;
                try
                {
                    if (dataGridView_Widget_Date.SelectedCells.Count > 0)
                    {
                        int rowIndex = dataGridView_Widget_Date.SelectedCells[0].OwningRow.Index;
                        WidgetRow_Down(dataGridView_Widget_Date, rowIndex, false);
                    }
                }
                catch { }
                PreviewView = true;
                userControl_Widget_ValueChanged(sender, e);
                PreviewImage();
                return;
            }
            if (e.KeyCode == Keys.Home)
            {
                e.SuppressKeyPress = true;
                PreviewView = false;
                try
                {
                    if (dataGridView_Widget_Date.SelectedCells.Count > 0)
                    {
                        int rowIndex = dataGridView_Widget_Date.SelectedCells[0].OwningRow.Index;
                        WidgetRow_FullUp(dataGridView_Widget_Date, rowIndex, false);
                    }
                }
                catch { }
                PreviewView = true;
                userControl_Widget_ValueChanged(sender, e);
                PreviewImage();
                return;
            }
            if (e.KeyCode == Keys.End)
            {
                e.SuppressKeyPress = true;
                PreviewView = false;
                try
                {
                    if (dataGridView_Widget_Date.SelectedCells.Count > 0)
                    {
                        int rowIndex = dataGridView_Widget_Date.SelectedCells[0].OwningRow.Index;
                        WidgetRow_FullDown(dataGridView_Widget_Date, rowIndex, false);
                    }
                }
                catch { }
                PreviewView = true;
                userControl_Widget_ValueChanged(sender, e);
                PreviewImage();
                return;
            }
        }

        private void dataGridView_Widget_DateAdd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.PageUp)
            {
                e.SuppressKeyPress = true;
                PreviewView = false;
                try
                {
                    if (dataGridView_Widget_DateAdd.SelectedCells.Count > 0)
                    {
                        int rowIndex = dataGridView_Widget_DateAdd.SelectedCells[0].OwningRow.Index;
                        WidgetRow_Up(dataGridView_Widget_DateAdd, rowIndex, false);
                    }
                }
                catch { }
                PreviewView = true;
                userControl_Widget_ValueChanged(sender, e);
                PreviewImage();
                return;
            }
            if (e.KeyCode == Keys.PageDown)
            {
                e.SuppressKeyPress = true;
                PreviewView = false;
                try
                {
                    if (dataGridView_Widget_DateAdd.SelectedCells.Count > 0)
                    {
                        int rowIndex = dataGridView_Widget_DateAdd.SelectedCells[0].OwningRow.Index;
                        WidgetRow_Down(dataGridView_Widget_DateAdd, rowIndex, false);
                    }
                }
                catch { }
                PreviewView = true;
                userControl_Widget_ValueChanged(sender, e);
                PreviewImage();
                return;
            }
            if (e.KeyCode == Keys.Home)
            {
                e.SuppressKeyPress = true;
                PreviewView = false;
                try
                {
                    if (dataGridView_Widget_DateAdd.SelectedCells.Count > 0)
                    {
                        int rowIndex = dataGridView_Widget_DateAdd.SelectedCells[0].OwningRow.Index;
                        WidgetRow_FullUp(dataGridView_Widget_DateAdd, rowIndex, false);
                    }
                }
                catch { }
                PreviewView = true;
                userControl_Widget_ValueChanged(sender, e);
                PreviewImage();
                return;
            }
            if (e.KeyCode == Keys.End)
            {
                e.SuppressKeyPress = true;
                PreviewView = false;
                try
                {
                    if (dataGridView_Widget_DateAdd.SelectedCells.Count > 0)
                    {
                        int rowIndex = dataGridView_Widget_DateAdd.SelectedCells[0].OwningRow.Index;
                        WidgetRow_FullDown(dataGridView_Widget_DateAdd, rowIndex, false);
                    }
                }
                catch { }
                PreviewView = true;
                userControl_Widget_ValueChanged(sender, e);
                PreviewImage();
                return;
            }
        }

        private void button_Widget_Date_Start_Click(object sender, EventArgs e)
        {
            PreviewView = false;
            try
            {
                int rowIndex = dataGridView_Widget_Date.SelectedCells[0].OwningRow.Index;
                WidgetRow_FullUp(dataGridView_Widget_Date, rowIndex, false);
            }
            catch { }
            PreviewView = true;
            userControl_Widget_ValueChanged(sender, e);
            PreviewImage();
        }

        private void button_Widget_Date_Up_Click(object sender, EventArgs e)
        {
            PreviewView = false;
            try
            {
                int rowIndex = dataGridView_Widget_Date.SelectedCells[0].OwningRow.Index;
                WidgetRow_Up(dataGridView_Widget_Date, rowIndex, false);
            }
            catch { }
            PreviewView = true;
            userControl_Widget_ValueChanged(sender, e);
            PreviewImage();
        }

        private void button_Widget_Date_Down_Click(object sender, EventArgs e)
        {
            PreviewView = false;
            try
            {
                int rowIndex = dataGridView_Widget_Date.SelectedCells[0].OwningRow.Index;
                WidgetRow_Down(dataGridView_Widget_Date, rowIndex, false);
            }
            catch { }
            PreviewView = true;
            userControl_Widget_ValueChanged(sender, e);
            PreviewImage();
        }

        private void button_Widget_Date_End_Click(object sender, EventArgs e)
        {
            PreviewView = false;
            try
            {
                int rowIndex = dataGridView_Widget_Date.SelectedCells[0].OwningRow.Index;
                WidgetRow_FullDown(dataGridView_Widget_Date, rowIndex, false);
            }
            catch { }
            PreviewView = true;
            userControl_Widget_ValueChanged(sender, e);
            PreviewImage();
        }

        private void button_Widget_Date_StartAdd_Click(object sender, EventArgs e)
        {
            PreviewView = false;
            try
            {
                int rowIndex = dataGridView_Widget_DateAdd.SelectedCells[0].OwningRow.Index;
                WidgetRow_FullUp(dataGridView_Widget_DateAdd, rowIndex, false);
            }
            catch { }
            PreviewView = true;
            PreviewImage();
        }

        private void button_Widget_Date_UpAdd_Click(object sender, EventArgs e)
        {
            PreviewView = false;
            try
            {
                int rowIndex = dataGridView_Widget_DateAdd.SelectedCells[0].OwningRow.Index;
                WidgetRow_Up(dataGridView_Widget_DateAdd, rowIndex, false);
            }
            catch { }
            PreviewView = true;
            PreviewImage();
        }

        private void button_Widget_Date_DownAdd_Click(object sender, EventArgs e)
        {
            PreviewView = false;
            try
            {
                int rowIndex = dataGridView_Widget_DateAdd.SelectedCells[0].OwningRow.Index;
                WidgetRow_Down(dataGridView_Widget_DateAdd, rowIndex, false);
            }
            catch { }
            PreviewView = true;
            PreviewImage();
        }

        private void button_Widget_Date_EndAdd_Click(object sender, EventArgs e)
        {
            PreviewView = false;
            try
            {
                int rowIndex = dataGridView_Widget_DateAdd.SelectedCells[0].OwningRow.Index;
                WidgetRow_FullDown(dataGridView_Widget_DateAdd, rowIndex, false);
            }
            catch { }
            PreviewView = true;
            PreviewImage();
        }

        private void checkBox_TimeOnWidgetEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_TimeOnWidgetEdit.Checked)
            {
                if (WidgetsTemp != null && WidgetsTemp.Widget != null) WidgetsTemp.Unknown4 = 1;
            }
            else
            {
                if (WidgetsTemp != null && WidgetsTemp.Widget != null) WidgetsTemp.Unknown4 = 0;
            }

            JSON_write();
            PreviewImage();
        }

        private void numericUpDown_Widget_ValueChanged(object sender, EventArgs e)
        {
            if (!PreviewView) return;
            int widgetIndex = comboBox_WidgetNumber.SelectedIndex;
            WidgetEdit(widgetIndex);

            JSON_write();
            PreviewImage();
        }

        private void comboBox_Widget_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!PreviewView) return;
            int widgetIndex = comboBox_WidgetNumber.SelectedIndex;
            WidgetEdit(widgetIndex);

            if (comboBox_WidgetsUnderMask.SelectedIndex >= 0)
            {
                if (WidgetsTemp != null && WidgetsTemp.Widget != null) WidgetsTemp.UnderMaskImageIndex = Int32.Parse(comboBox_WidgetsUnderMask.Text);
            }
            if (comboBox_WidgetsTopMask.SelectedIndex >= 0)
            {
                if (WidgetsTemp != null && WidgetsTemp.Widget != null) WidgetsTemp.TopMaskImageIndex = Int32.Parse(comboBox_WidgetsTopMask.Text);
            }

            JSON_write();
            PreviewImage();
        }

        private void userControl_Widget_ValueChanged(object sender, EventArgs eventArgs)
        {
            if (!PreviewView) return;
            if (dataGridView_WidgetElement.SelectedCells.Count > 0)
            {
                int widgetIndex = comboBox_WidgetNumber.SelectedIndex;
                int widgetElementIndex = dataGridView_WidgetElement.SelectedCells[0].RowIndex;
                WidgetElementEdit(widgetIndex, widgetElementIndex);
            }
        }

        private void radioButton_WidgetAdd_CheckedChanged(object sender, EventArgs e)
        {
            groupBox_WidgetNumberAdd.Enabled = radioButton_WidgetAdd.Checked;
            PreviewImage();
        }

        private void radioButton_WidgetAddType_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (!radioButton.Checked) return;
            string name = null;
            switch (radioButton.Name)
            {
                case "radioButton_DateWidgetAdd":
                    name = "Date";
                    break;
                case "radioButton_StepsWidgetAdd":
                    name = "Steps";
                    break;
                case "radioButton_CaloriesWidgetAdd":
                    name = "Calories";
                    break;
                case "radioButton_HeartRateWidgetAdd":
                    name = "HeartRate";
                    break;
                case "radioButton_PAIWidgetAdd":
                    name = "PAI";
                    break;
                case "radioButton_DistanceWidgetAdd":
                    name = "Distance";
                    break;
                case "radioButton_StandUpWidgetAdd":
                    name = "StandUp";
                    break;
                case "radioButton_ActivityGoalWidgetAdd":
                    name = "ActivityGoal";
                    break;
                case "radioButton_FatBurningWidgetAdd":
                    name = "FatBurning";
                    break;
                case "radioButton_WeatherWidgetAdd":
                    name = "Weather";
                    break;
                case "radioButton_UVindexWidgetAdd":
                    name = "UVindex";
                    break;
                case "radioButton_HumidityWidgetAdd":
                    name = "Humidity";
                    break;
                case "radioButton_SunriseWidgetAdd":
                    name = "Sunrise";
                    break;
                case "radioButton_WindForceWidgetAdd":
                    name = "WindForce";
                    break;
                case "radioButton_AirPressureWidgetAdd":
                    name = "AirPressure";
                    break;
                case "radioButton_BatteryWidgetAdd":
                    name = "Battery";
                    break;
            }
            SelectWidgetElementAdd(name);
            PreviewImage();
        }

        private void userControl_WidgetAdd_ValueChanged(object sender, EventArgs eventArgs)
        {
            if (PreviewView) PreviewImage();
        }

        private void tabControl_Widget_SelectedIndexChanged(object sender, EventArgs e)
        {
            PreviewImage();
        }

        private void tabControl_Widget_VisibleChanged(object sender, EventArgs e)
        {
            if (!tabControl_Widget.TabStop) 
            {
                tabControl_Widget.TabStop = true;
                HideWidgetEditElement();
            }
        }

        private void button_WidgetAdd_Click(object sender, EventArgs e)
        {
            if (radioButton_WidgetAdd.Checked) AddWidget();
            else if (radioButton_WidgetElementAdd.Checked) AddWidgetElement();
        }

        private void WidgetAdd_ValueChanged(object sender, EventArgs e)
        {
            PreviewImage();
        }

        private void radioButton_WidgetPreviewNormal_CheckedChanged(object sender, EventArgs e)
        {
            PreviewImage();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tabName = tabControl1.SelectedTab.Name;
            if(tabName == "tabPage_Widgets" || oldTabName == "tabPage_Widgets") PreviewImage();
            oldTabName = tabName;
        }

        /// <summary>формируем изображение для предпросмотра редактируемых зон</summary>
        /// <param name="type">1-если редактируем елемент;
        /// 2-если добавляем новый элемент.</param>
        private Bitmap PreviewWidgetAdd(int type)
        {
            // формируем картинку для предпросмотра
            Bitmap bitmap = new Bitmap(Convert.ToInt32(454), Convert.ToInt32(454), PixelFormat.Format32bppArgb);
            Bitmap mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gtr_2.png");
            //int PreviewHeight = 306;
            if (radioButton_GTS2.Checked)
            {
                bitmap = new Bitmap(Convert.ToInt32(348), Convert.ToInt32(442), PixelFormat.Format32bppArgb);
                mask = new Bitmap(Application.StartupPath + @"\Mask\mask_gts_2.png");
                //PreviewHeight = 323;
            }
            if (radioButton_TRex_pro.Checked)
            {
                bitmap = new Bitmap(Convert.ToInt32(360), Convert.ToInt32(360), PixelFormat.Format32bppArgb);
                mask = new Bitmap(Application.StartupPath + @"\Mask\mask_trex_pro.png");
                //PreviewHeight = 220;
            }
            if (radioButton_ZeppE.Checked)
            {
                bitmap = new Bitmap(Convert.ToInt32(416), Convert.ToInt32(416), PixelFormat.Format32bppArgb);
                mask = new Bitmap(Application.StartupPath + @"\Mask\mask_zepp_e.png");
                //PreviewHeight = 220;
            }
            Graphics gPanel = Graphics.FromImage(bitmap);
            int link = -1;
            PreviewToBitmap(gPanel, 1.0f, false, false, false, false, false, false, false, true, false, false, false, link);
            bitmap = ApplyWidgetMask(bitmap, comboBox_WidgetsTopMask.SelectedIndex);
            if (checkBox_crop.Checked) bitmap = ApplyMask(bitmap, mask);
            int x = 0;
            int y = 0;
            int width = 0;
            int height = 0;
            if (type == 1)
            {
                x = (int)numericUpDown_WidgetX.Value;
                y = (int)numericUpDown_WidgetY.Value;
                width = (int)numericUpDown_WidgetWidth.Value;
                height = (int)numericUpDown_WidgetHeight.Value;
            }
            if (type == 2)
            {
                if (radioButton_WidgetAdd.Checked)
                {
                    x = (int)numericUpDown_WidgetXAdd.Value;
                    y = (int)numericUpDown_WidgetYAdd.Value;
                    width = (int)numericUpDown_WidgetWidthAdd.Value;
                    height = (int)numericUpDown_WidgetHeightAdd.Value;
                }
                else
                {
                    x = (int)numericUpDown_WidgetX.Value;
                    y = (int)numericUpDown_WidgetY.Value;
                    width = (int)numericUpDown_WidgetWidth.Value;
                    height = (int)numericUpDown_WidgetHeight.Value;
                } 
            }
            if (width > 1 && height > 1)
            {
                Rectangle cropRect = new Rectangle(x, y, width, height);
                Bitmap tempBitmap = new Bitmap(width, height);
                using (Graphics g = Graphics.FromImage(tempBitmap))
                {
                    g.DrawImage(bitmap, new Rectangle(0, 0, tempBitmap.Width, tempBitmap.Height),
                                     cropRect,
                                     GraphicsUnit.Pixel);
                }
                bitmap = tempBitmap;
            }
            return bitmap;
        }
        
        private void userControl_previewWidgetAdd_CreatePreview(object sender, EventArgs eventArgs)
        {
            if (userControl_previewWidgetAdd.comboBoxGetImage() >= 0) return;
            if ((numericUpDown_WidgetWidth.Value < 2 || numericUpDown_WidgetHeight.Value < 2) 
                && radioButton_WidgetElementAdd.Checked) return;
            if ((numericUpDown_WidgetWidthAdd.Value < 2 || numericUpDown_WidgetHeightAdd.Value < 2)
                && radioButton_WidgetAdd.Checked) return;
            if (FileName != null && FullFileDir != null) // проект уже сохранен
            {
                UserControl_preview userControl_preview = sender as UserControl_preview;
                Bitmap bitmap = PreviewWidgetAdd(2);
                // определяем имя файла для сохранения и сохраняем файл
                string NamePreview = "0001.png";
                string PathPreview = Path.Combine(FullFileDir, NamePreview);
                int index = 1;
                if (ListImagesFullName.Count > 0)
                {
                    Int32.TryParse(Path.GetFileNameWithoutExtension(ListImagesFullName.Last()), out index);
                    index++;
                    NamePreview = index.ToString() + ".png";
                    PathPreview = Path.Combine(FullFileDir, NamePreview);
                }
                while (PathPreview.Length < ListImagesFullName[0].Length)
                {
                    NamePreview = "0" + NamePreview;
                    PathPreview = Path.Combine(FullFileDir, NamePreview);
                }
                if (File.Exists(PathPreview)) return;
                bitmap.Save(PathPreview, ImageFormat.Png);

                PreviewView = false;
                ListImages.Add(index.ToString());
                ListImagesFullName.Add(PathPreview);

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
                dataGridView_ImagesList.Rows.Add( RowNew);

                userControl_preview.comboBox_image.Items.Add(index.ToString());
                //userControl_preview.comboBoxSetImage(index);
                PreviewView = true;
                userControl_preview.comboBoxSetImage(index);
                JSON_Modified = true;
                FormText();

                bitmap.Dispose();

            }
        }

        private void userControl_previewWidgetAdd_RefreshPreview(object sender, EventArgs eventArgs)
        {
            if (userControl_previewWidgetAdd.comboBoxGetImage() < 0) return;
            if ((numericUpDown_WidgetWidth.Value < 2 || numericUpDown_WidgetHeight.Value < 2)
                && radioButton_WidgetElementAdd.Checked) return;
            if ((numericUpDown_WidgetWidthAdd.Value < 2 || numericUpDown_WidgetHeightAdd.Value < 2)
                && radioButton_WidgetAdd.Checked) return;
            if (FileName != null && FullFileDir != null)
            {
                UserControl_preview userControl_preview = sender as UserControl_preview;
                Bitmap bitmap = PreviewWidgetAdd(2);
                int i = userControl_preview.comboBoxGetSelectedIndexImage();
                bitmap.Save(ListImagesFullName[i], ImageFormat.Png);
                bitmap.Dispose();

            }
        }

        private void userControl_previewWidget_CreatePreview(object sender, EventArgs eventArgs)
        {
            if (userControl_previewWidget.comboBoxGetImage() >= 0) return;
            if(radioButton_WidgetPreviewEdit.Checked) return;
            if (numericUpDown_WidgetWidth.Value < 2 || numericUpDown_WidgetHeight.Value < 2) return;
            if (FileName != null && FullFileDir != null) // проект уже сохранен
            {
                UserControl_preview userControl_preview = sender as UserControl_preview;
                Bitmap bitmap = PreviewWidgetAdd(1);
                // определяем имя файла для сохранения и сохраняем файл
                string NamePreview = "0001.png";
                string PathPreview = Path.Combine(FullFileDir, NamePreview);
                int index = 1;
                if (ListImagesFullName.Count > 0)
                {
                    Int32.TryParse(Path.GetFileNameWithoutExtension(ListImagesFullName.Last()), out index);
                    index++;
                    NamePreview = index.ToString() + ".png";
                    PathPreview = Path.Combine(FullFileDir, NamePreview);
                }
                while (PathPreview.Length < ListImagesFullName[0].Length)
                {
                    NamePreview = "0" + NamePreview;
                    PathPreview = Path.Combine(FullFileDir, NamePreview);
                }
                if (File.Exists(PathPreview)) return;
                bitmap.Save(PathPreview, ImageFormat.Png);

                PreviewView = false;
                ListImages.Add(index.ToString());
                ListImagesFullName.Add(PathPreview);

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
                dataGridView_ImagesList.Rows.Add(RowNew);

                userControl_preview.comboBox_image.Items.Add(index.ToString());
                //userControl_preview.comboBoxSetImage(index);
                PreviewView = true;
                userControl_preview.comboBoxSetImage(index);
                JSON_Modified = true;
                FormText();

                bitmap.Dispose();

            }
        }

        private void userControl_previewWidget_RefreshPreview(object sender, EventArgs eventArgs)
        {
            if (userControl_previewWidget.comboBoxGetImage() < 0) return;
            if (radioButton_WidgetPreviewEdit.Checked) return;
            if (numericUpDown_WidgetWidth.Value < 2 || numericUpDown_WidgetHeight.Value < 2) return;
            if (FileName != null && FullFileDir != null)
            {
                UserControl_preview userControl_preview = sender as UserControl_preview;
                Bitmap bitmap = PreviewWidgetAdd(1);
                int i = userControl_preview.comboBoxGetSelectedIndexImage();
                bitmap.Save(ListImagesFullName[i], ImageFormat.Png);
                bitmap.Dispose();

            }
        }

        private void radioButton_ConvertingInput_GTR2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_ConvertingInput_GTR2.Checked)
            {
                numericUpDown_ConvertingInput_Custom.Value = 454;
            }
            if (radioButton_ConvertingInput_TRexPro.Checked)
            {
                numericUpDown_ConvertingInput_Custom.Value = 360;
            }
            if (radioButton_ConvertingInput_ZeppE.Checked)
            {
                numericUpDown_ConvertingInput_Custom.Value = 416;
            }
        }

        private void radioButton_ConvertingOutput_GTR2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_ConvertingOutput_GTR2.Checked)
            {
                numericUpDown_ConvertingOutput_Custom.Value = 454;
            }
            if (radioButton_ConvertingOutput_TRexPro.Checked)
            {
                numericUpDown_ConvertingOutput_Custom.Value = 360;
            }
            if (radioButton_ConvertingOutput_ZeppE.Checked)
            {
                numericUpDown_ConvertingOutput_Custom.Value = 416;
            }
        }







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
