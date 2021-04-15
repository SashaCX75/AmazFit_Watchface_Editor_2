using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AmazFit_Watchface_2
{
    public partial class UserControl_text_weather : AmazFit_Watchface_2.UserControl_text
    {
        public UserControl_text_weather()
        {
            InitializeComponent();
        }

        private void checkBox_addZero_CheckedChanged(object sender, EventArgs e)
        {
            bool b = !checkBox_addZero.Checked;
            label02.Enabled = b;
            label1084.Enabled = b;
            label1085.Enabled = b;
            numericUpDown_imageX.Enabled = b;
            numericUpDown_imageY.Enabled = b;
        }

        protected override void checkBox_Use_CheckedChanged(object sender, EventArgs e)
        {
            base.checkBox_Use_CheckedChanged(sender, e);
            bool b = !checkBox_addZero.Checked;
            label02.Enabled = b;
            label1084.Enabled = b;
            label1085.Enabled = b;
            numericUpDown_imageX.Enabled = b;
            numericUpDown_imageY.Enabled = b;
        }

        /// <summary>Возвращает SelectedIndex выпадающего списка</summary>
        internal int comboBoxGetSelectedIndexImageError()
        {
            return comboBox_imageError.SelectedIndex;
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
