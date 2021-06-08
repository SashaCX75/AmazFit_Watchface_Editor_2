using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AmazFit_Watchface_2
{
    public partial class UserControl_SystemFont_GroupTime : AmazFit_Watchface_2.UserControl_SystemFont_GroupSunrise
    {
        private bool PaddingZero;

        public UserControl_SystemFont_GroupTime()
        {
            InitializeComponent();

            userControl_SystemFont_weather_Current.checkBox_SystemFont_unit.ThreeState = false;
            userControl_SystemFont_weather_Min.checkBox_SystemFont_unit.ThreeState = false;
            userControl_SystemFont_weather_Max.checkBox_SystemFont_unit.ThreeState = false;

            userControl_FontRotate_weather_Current.checkBox_FontRotate_unit.ThreeState = false;
            userControl_FontRotate_weather_Min.checkBox_FontRotate_unit.ThreeState = false;
            userControl_FontRotate_weather_Max.checkBox_FontRotate_unit.ThreeState = false;
        }

        [Description("Отображение чекбокса добавления нулей в начале")]
        public override bool Padding_zero
        {
            get
            {
                return PaddingZero;
            }
            set
            {
                PaddingZero = value;
                userControl_SystemFont_weather_Current.Padding_zero = PaddingZero;
                //userControl_SystemFont_weather_Min.Padding_zero = PaddingZero;
                //userControl_SystemFont_weather_Max.Padding_zero = PaddingZero;

                userControl_FontRotate_weather_Current.Padding_zero = PaddingZero;
                //userControl_FontRotate_weather_Min.Padding_zero = PaddingZero;
                //userControl_FontRotate_weather_Max.Padding_zero = PaddingZero;
            }
        }
    }
}
