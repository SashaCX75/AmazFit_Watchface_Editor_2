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

        [Description("Устанавливает надпись на кнопке")]
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

        [Description("Устанавливает надпись для десятичного разделителя")]
        [Localizable(true)]
        public string ButtonTextDecimalPoint
        {
            get
            {
                return label07.Text;
            }
            set
            {
                label07.Text = value;
            }
        }
    }
}
