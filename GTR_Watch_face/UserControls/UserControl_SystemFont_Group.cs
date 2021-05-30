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
    public partial class UserControl_SystemFont_Group : UserControl
    {
        private bool setValue;
        private bool AODmode;
        private bool showUnit;
        private bool PaddingZero;
        private bool Follow_mode;
        private bool FollowGoal_mode;
        private bool Separator_mode;
        private bool Goal_mode;
        public UserControl_SystemFont_Group()
        {
            InitializeComponent();
            panel_SystemFont.AutoSize = true;
            userControl_SystemFont_goal.Padding_zero = false;
            userControl_FontRotate_goal.Padding_zero = false;
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
                userControl_SystemFont.AOD = AODmode;
                userControl_SystemFont_goal.AOD = AODmode;
                userControl_FontRotate.AOD = AODmode;
                userControl_FontRotate_goal.AOD = AODmode;
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
                userControl_SystemFont.ShowUnit = showUnit;
                userControl_SystemFont_goal.ShowUnit = showUnit;
                userControl_FontRotate.ShowUnit = showUnit;
                userControl_FontRotate_goal.ShowUnit = showUnit;
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
                userControl_SystemFont.Padding_zero = PaddingZero;
                //userControl_SystemFont_goal.Padding_zero = PaddingZero;
                userControl_FontRotate.Padding_zero = PaddingZero;
                //userControl_FontRotate_goal.Padding_zero = PaddingZero;
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
                userControl_SystemFont.Separator = Separator_mode;
                userControl_SystemFont_goal.Separator = Separator_mode;
                userControl_FontRotate.Separator = Separator_mode;
                userControl_FontRotate_goal.Separator = Separator_mode;
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
                userControl_SystemFont.Follow = Follow_mode;
                //userControl_SystemFont_goal.Follow = Follow_mode;
                userControl_FontRotate.Follow = Follow_mode;
                //userControl_FontRotate_goal.Follow = Follow_mode;
            }
        }

        [Description("Отображение чекбокса \"Следовать за...\" для цели")]
        public bool FollowGoal
        {
            get
            {
                return FollowGoal_mode;
            }
            set
            {
                FollowGoal_mode = value;
                //userControl_SystemFont.Follow = FollowGoal_mode;
                userControl_SystemFont_goal.Follow = FollowGoal_mode;
                //userControl_FontRotate.Follow = FollowGoal_mode;
                userControl_FontRotate_goal.Follow = FollowGoal_mode;
            }
        }

        [Description("Отображение цели")]
        public bool ShowGoal
        {
            get
            {
                return Goal_mode;
            }
            set
            {
                Goal_mode = value;
                userControl_SystemFont_goal.Visible = Goal_mode;
                userControl_FontRotate_goal.Visible = Goal_mode;
            }
        }

        [Description("Устанавливает надпись \"Следовать за ...\" для наклонного текста")]
        [Localizable(true)]
        public string FollowText
        {
            get
            {
                return userControl_SystemFont_goal.FollowText;
            }
            set
            {
                userControl_SystemFont_goal.FollowText = value;
            }
        }

        [Description("Устанавливает надпись \"Следовать за ...\" для по окружности")]
        [Localizable(true)]
        public string FollowRotateText
        {
            get
            {
                return userControl_FontRotate_goal.FollowText;
            }
            set
            {
                userControl_FontRotate_goal.FollowText = value;
            }
        }

        [Description("Устанавливает надпись на кнопке наклонного текста")]
        [Localizable(true)]
        public string SystemFontText
        {
            get
            {
                return userControl_SystemFont_goal.ButtonText;
            }
            set
            {
                userControl_SystemFont_goal.ButtonText = value;
            }
        }

        [Description("Устанавливает надпись на кнопке текста по окружности")]
        [Localizable(true)]
        public string FontRotateText
        {
            get
            {
                return userControl_FontRotate_goal.ButtonText;
            }
            set
            {
                userControl_FontRotate_goal.ButtonText = value;
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

            userControl_SystemFont.SettingsClear();
            userControl_SystemFont_goal.SettingsClear();
            userControl_FontRotate.SettingsClear();
            userControl_FontRotate_goal.SettingsClear();

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
