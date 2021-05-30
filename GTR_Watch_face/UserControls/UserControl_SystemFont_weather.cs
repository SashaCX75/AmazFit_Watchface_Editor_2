using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AmazFit_Watchface_2
{
    public partial class UserControl_SystemFont_weather : AmazFit_Watchface_2.UserControl_SystemFont
    {
        public UserControl_SystemFont_weather()
        {
            InitializeComponent();
        }

        /// <summary>Устанавливает надпись на кнопке</summary>
        [Localizable(true)]
        public string ButtonText
        {
            get
            {
                return button_SystemFont.Text;
            }
            set
            {
                button_SystemFont.Text = value;
            }
        }
    }
}
