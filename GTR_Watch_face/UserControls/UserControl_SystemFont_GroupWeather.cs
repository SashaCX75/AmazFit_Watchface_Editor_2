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
    public partial class UserControl_SystemFont_GroupWeather : UserControl
    {
        private bool setValue;
        private bool AODmode;
        private bool showUnit;
        private bool PaddingZero;
        private bool Follow_mode;
        private bool FollowMin_mode;
        private bool FollowMax_mode;
        private bool Separator_mode;
        private bool showMin;
        private bool showMax;
        public UserControl_SystemFont_GroupWeather()
        {
            InitializeComponent();
            panel_SystemFont.AutoSize = true;
        }

        [Description("Отображение кнопки копирования значений для AOD")]
        public bool AOD
        {
            get
            {
                return AODmode;
            }
            set
            {
                AODmode = value;
                userControl_SystemFont_weather_Current.AOD = AODmode;
                userControl_SystemFont_weather_Min.AOD = AODmode;
                userControl_SystemFont_weather_Max.AOD = AODmode;

                userControl_FontRotate_weather_Current.AOD = AODmode;
                userControl_FontRotate_weather_Min.AOD = AODmode;
                userControl_FontRotate_weather_Max.AOD = AODmode;
            }
        }

        [Description("Отображение возможности выбора единиц измерения")]
        public bool ShowUnit
        {
            get
            {
                return showUnit;
            }
            set
            {
                showUnit = value;
                userControl_SystemFont_weather_Current.ShowUnit = showUnit;
                userControl_SystemFont_weather_Min.ShowUnit = showUnit;
                userControl_SystemFont_weather_Max.ShowUnit = showUnit;

                userControl_FontRotate_weather_Current.ShowUnit = showUnit;
                userControl_FontRotate_weather_Min.ShowUnit = showUnit;
                userControl_FontRotate_weather_Max.ShowUnit = showUnit;
            }
        }

        [Description("Отображение чекбокса добавления нулей в начале")]
        public bool Padding_zero
        {
            get
            {
                return PaddingZero;
            }
            set
            {
                PaddingZero = value;
                userControl_SystemFont_weather_Current.Padding_zero = PaddingZero;
                userControl_SystemFont_weather_Min.Padding_zero = PaddingZero;
                userControl_SystemFont_weather_Max.Padding_zero = PaddingZero;

                userControl_FontRotate_weather_Current.Padding_zero = PaddingZero;
                userControl_FontRotate_weather_Min.Padding_zero = PaddingZero;
                userControl_FontRotate_weather_Max.Padding_zero = PaddingZero;
            }
        }

        [Description("Отображение чекбокса разделитель")]
        public bool Separator
        {
            /// <returns>Returns zero.</returns>
            get
            {
                return Separator_mode;
            }
            set
            {
                Separator_mode = value;
                userControl_SystemFont_weather_Current.Separator = Separator_mode;
                userControl_SystemFont_weather_Min.Separator = Separator_mode;
                userControl_SystemFont_weather_Max.Separator = Separator_mode;

                userControl_FontRotate_weather_Current.Separator = Separator_mode;
                userControl_FontRotate_weather_Min.Separator = Separator_mode;
                userControl_FontRotate_weather_Max.Separator = Separator_mode;
            }
        }

        [Description("Отображение чекбокса \"Следовать за...\"")]
        public bool Follow
        {
            get
            {
                return Follow_mode;
            }
            set
            {
                Follow_mode = value;
                userControl_SystemFont_weather_Current.Follow = Follow_mode;
                userControl_FontRotate_weather_Current.Follow = Follow_mode;
            }
        }

        [Description("Отображение чекбокса \"Следовать за...\" для минимального значения")]
        public bool FollowMin
        {
            get
            {
                return FollowMin_mode;
            }
            set
            {
                FollowMin_mode = value;
                userControl_SystemFont_weather_Min.Follow = FollowMin_mode;
                userControl_FontRotate_weather_Min.Follow = FollowMin_mode;
            }
        }

        [Description("Отображение чекбокса \"Следовать за...\" для масимального значения")]
        public bool FollowMax
        {
            get
            {
                return FollowMax_mode;
            }
            set
            {
                FollowMax_mode = value;
                userControl_SystemFont_weather_Max.Follow = FollowMax_mode;
                userControl_FontRotate_weather_Max.Follow = FollowMax_mode;
            }
        }

        [Description("Отображение минимального значения")]
        public bool ShowMin
        {
            get
            {
                return showMin;
            }
            set
            {
                showMin = value;
                userControl_SystemFont_weather_Min.Visible = showMin;
                userControl_FontRotate_weather_Min.Visible = showMin;
            }
        }

        [Description("Отображение максимального значения")]
        public bool ShowMax
        {
            get
            {
                return showMax;
            }
            set
            {
                showMax = value;
                userControl_SystemFont_weather_Max.Visible = showMax;
                userControl_FontRotate_weather_Max.Visible = showMax;
            }
        }

        [Description("Устанавливает надпись \"Следовать за ...\" для текущего значения наклонного текста")]
        [Localizable(true)]
        public string FollowText_Current
        {
            get
            {
                return userControl_SystemFont_weather_Current.FollowText;
            }
            set
            {
                userControl_SystemFont_weather_Current.FollowText = value;
            }
        }

        [Description("Устанавливает надпись \"Следовать за ...\" для минимального значения наклонного текста")]
        [Localizable(true)]
        public string FollowText_Min
        {
            get
            {
                return userControl_SystemFont_weather_Min.FollowText;
            }
            set
            {
                userControl_SystemFont_weather_Min.FollowText = value;
            }
        }

        [Description("Устанавливает надпись \"Следовать за ...\" для максимального значения наклонного текста")]
        [Localizable(true)]
        public string FollowText_Max
        {
            get
            {
                return userControl_SystemFont_weather_Max.FollowText;
            }
            set
            {
                userControl_SystemFont_weather_Max.FollowText = value;
            }
        }

        [Description("Устанавливает надпись \"Следовать за ...\" для текущего значения текста по окружности")]
        [Localizable(true)]
        public string FollowRotatText_Current
        {
            get
            {
                return userControl_FontRotate_weather_Current.FollowText;
            }
            set
            {
                userControl_FontRotate_weather_Current.FollowText = value;
            }
        }

        [Description("Устанавливает надпись \"Следовать за ...\" для минимального значения текста по окружности")]
        [Localizable(true)]
        public string FollowRotatText_Min
        {
            get
            {
                return userControl_FontRotate_weather_Min.FollowText;
            }
            set
            {
                userControl_FontRotate_weather_Min.FollowText = value;
            }
        }

        [Description("Устанавливает надпись \"Следовать за ...\" для максимального значения текста по окружности")]
        [Localizable(true)]
        public string FollowRotatText_Max
        {
            get
            {
                return userControl_FontRotate_weather_Max.FollowText;
            }
            set
            {
                userControl_FontRotate_weather_Max.FollowText = value;
            }
        }


        [Browsable(true)]
        public event ValueChangedHandler ValueChanged;
        public delegate void ValueChangedHandler(object sender, EventArgs eventArgs);

        [Browsable(true)]
        public event AOD_CopyHandler AOD_Copy_SystemFont;
        public delegate void AOD_CopyHandler(object sender, EventArgs eventArgs);

        [Description("Возвращает true если панель свернута")]
        //[Description("The image associated with the control"), Category("Appearance")]
        public bool Collapsed
        {
            get
            {
                return !panel_SystemFont.Visible;
            }
            set
            {
                panel_SystemFont.Visible = !value;
            }
        }

        private void button_SystemFont_Click(object sender, EventArgs e)
        {
            panel_SystemFont.Visible = !panel_SystemFont.Visible;
        }

        private void userControl_ValueChanged(object sender, EventArgs eventArgs)
        {
            if (ValueChanged != null && !setValue)
            {
                //EventArgs e = new EventArgs();
                ValueChanged(this, eventArgs);
            }
        }

        #region Settings Set/Clear

        /// <summary>Очищает выпадающие списки с картинками, сбрасывает данные на значения по умолчанию</summary>
        internal void SettingsClear()
        {
            setValue = true;

            userControl_SystemFont_weather_Current.SettingsClear();
            userControl_SystemFont_weather_Min.SettingsClear();
            userControl_SystemFont_weather_Max.SettingsClear();
            userControl_FontRotate_weather_Current.SettingsClear();
            userControl_FontRotate_weather_Min.SettingsClear();
            userControl_FontRotate_weather_Max.SettingsClear();

            setValue = false;
        }
        #endregion

        private void userControl_Copy_SystemFont(object sender, EventArgs eventArgs)
        {
            if (AOD_Copy_SystemFont != null)
            {
                //EventArgs eventArgs = new EventArgs();
                AOD_Copy_SystemFont(sender, eventArgs);
            }
        }
    }
}
