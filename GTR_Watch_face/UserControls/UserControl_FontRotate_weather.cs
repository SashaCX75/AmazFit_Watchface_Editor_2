using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AmazFit_Watchface_2
{
    public partial class UserControl_FontRotate_weather : AmazFit_Watchface_2.UserControl_FontRotate
    {
        public UserControl_FontRotate_weather()
        {
            InitializeComponent();
        }

        [Description("Устанавливает надпись на кнопке")]
        [Localizable(true)]
        public string ButtonText
        {
            get
            {
                return button_FontRotate.Text;
            }
            set
            {
                button_FontRotate.Text = value;
            }
        }

        private void checkBox_follow_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
