using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AmazFit_Watchface_2
{
    public partial class UserControl_text_goal : AmazFit_Watchface_2.UserControl_text
    {
        public UserControl_text_goal()
        {
            InitializeComponent();
        }

        /// <summary>Устанавливает надпись на кнопке</summary>
        [Localizable(true)]
        public string ButtonText
        {
            get
            {
                return button_text.Text;
            }
            set
            {
                button_text.Text = value;
            }
        }
    }
}
