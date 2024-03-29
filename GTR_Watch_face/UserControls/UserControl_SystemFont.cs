﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmazFit_Watchface_2
{
    public partial class UserControl_SystemFont : UserControl
    {
        private bool setValue;
        private bool AODmode;
        private bool showUnit;
        private bool PaddingZero;
        private bool Follow_mode;
        private bool Separator_mode;
        public UserControl_SystemFont()
        {
            InitializeComponent();
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
                button_Copy_SystemFont.Visible = AODmode;
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
                checkBox_SystemFont_unit.Visible = showUnit;
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
                checkBox_addZero.Visible = PaddingZero;
            }
        }

        [Description("Отображение чекбокса разделитель")]
        public bool Separator
        {
            get
            {
                return Separator_mode;
            }
            set
            {
                Separator_mode = value;
                checkBox_separator.Visible = Separator_mode;
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
                checkBox_follow.Visible = Follow_mode;
            }
        }

        [Description("Устанавливает надпись \"Следовать за...\"")]
        [Localizable(true)]
        public string FollowText
        {
            get
            {
                return checkBox_follow.Text;
            }
            set
            {
                checkBox_follow.Text = value;
            }
        }

        [Browsable(true)]
        public event CollapseHandler Collapse;
        public delegate void CollapseHandler(object sender, EventArgs eventArgs);

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

        // кнопка копирования свойст для AOD
        private void button_Copy_SystemFont_Click(object sender, EventArgs e)
        {
            if (AOD_Copy_SystemFont != null)
            {
                EventArgs eventArgs = new EventArgs();
                AOD_Copy_SystemFont(this, eventArgs);
            }
        }

        // кнопка сворачивания
        private void button_Click(object sender, EventArgs e)
        {
            panel_SystemFont.Visible = !panel_SystemFont.Visible;
            if (Collapse != null)
            {
                EventArgs eventArgs = new EventArgs();
                Collapse(this, eventArgs);
            }
        }

        private void checkBox_Click(object sender, EventArgs e)
        {
            if (ValueChanged != null && !setValue)
            {
                EventArgs eventArgs = new EventArgs();
                ValueChanged(this, eventArgs);
            }
        }
        internal void comboBoxSetColorString(string color)
        {
            if (color.Length == 18) color = color.Remove(2, 8);
            Color old_color = ColorTranslator.FromHtml(color);
            Color new_color = Color.FromArgb(255, old_color.R, old_color.G, old_color.B);
            comboBox_SystemFont_color.BackColor = new_color;
        }
        internal string comboBoxGetColorString()
        {
            Color color = comboBox_SystemFont_color.BackColor;
            Color new_color = Color.FromArgb(0, color.R, color.G, color.B);
            string colorStr = ColorTranslator.ToHtml(new_color);
            colorStr = colorStr.Replace("#", "0xFF");
            return colorStr;
        }
        internal Color comboBoxGetColor()
        {
            return comboBox_SystemFont_color.BackColor;
        }

        internal void checkBoxSetUnit(int unit)
        {
            switch (unit)
            {
                case 1:
                    checkBox_SystemFont_unit.CheckState = CheckState.Checked;
                    break;
                case 2:
                    checkBox_SystemFont_unit.CheckState = CheckState.Indeterminate;
                    break;
                default:
                    checkBox_SystemFont_unit.CheckState = CheckState.Unchecked;
                    break;
            }
        }

        internal int checkBoxGetUnit()
        {
            int value = -1;
            if (checkBox_SystemFont_unit.CheckState == CheckState.Checked) value = 1;
            if (checkBox_SystemFont_unit.CheckState == CheckState.Indeterminate) value = 2;
            return value;
        }

        #region Standard events

        private void comboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void checkBox_Use_CheckedChanged(object sender, EventArgs e)
        {
            Control.ControlCollection controlCollection = panel_SystemFont.Controls;

            bool b = checkBox_Use.Checked;
            for (int i = 1; i < controlCollection.Count - 1; i++)
            {
                controlCollection[i].Enabled = b;
            }

            if (b)
            {
                b = !checkBox_follow.Checked;
                label01.Enabled = b;
                label02.Enabled = b;
                label03.Enabled = b;
                label04.Enabled = b;
                label05.Enabled = b;
                label06.Enabled = b;
                label07.Enabled = b;

                numericUpDown_SystemFontX.Enabled = b;
                numericUpDown_SystemFontY.Enabled = b;

                numericUpDown_SystemFont_size.Enabled = b;
                numericUpDown_SystemFont_angle.Enabled = b;
                numericUpDown_SystemFont_spacing.Enabled = b;
                comboBox_SystemFont_color.Enabled = b; 
            }
        }
        private void comboBox_color_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            ComboBox comboBox_color = sender as ComboBox;
            colorDialog.Color = comboBox_color.BackColor;
            colorDialog.FullOpen = true;
            colorDialog.CustomColors = Form1.Program_Settings.CustomColors;


            if (colorDialog.ShowDialog() == DialogResult.Cancel)
                return;
            // установка цвета формы
            comboBox_color.BackColor = colorDialog.Color;
            if (Form1.Program_Settings.CustomColors != colorDialog.CustomColors)
            {
                Form1.Program_Settings.CustomColors = colorDialog.CustomColors;

                string JSON_String = JsonConvert.SerializeObject(Form1.Program_Settings, Formatting.Indented, new JsonSerializerSettings
                {
                    //DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
                File.WriteAllText(Application.StartupPath + @"\Settings.json", JSON_String, Encoding.UTF8);
            }

            if (ValueChanged != null && !setValue)
            {
                EventArgs eventArgs = new EventArgs();
                ValueChanged(this, eventArgs);
            }
        }

        #endregion

        #region Settings Set/Clear

        /// <summary>Очищает выпадающие списки с картинками, сбрасывает данные на значения по умолчанию</summary>
        internal void SettingsClear()
        {
            setValue = true;

            checkBox_Use.Checked = false;
            checkBox_SystemFont_unit.Checked = false;
            checkBox_addZero.Checked = false;
            checkBox_follow.Checked = false;

            numericUpDown_SystemFontX.Value = 0;
            numericUpDown_SystemFontY.Value = 0;

            numericUpDown_SystemFont_size.Value = 20;
            numericUpDown_SystemFont_angle.Value = 0;
            numericUpDown_SystemFont_spacing.Value = 0;

            setValue = false;
        }
        #endregion

        #region contextMenu
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
        #endregion

        #region numericUpDown
        private void numericUpDown_picturesX_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MouseСoordinates.X < 0) return;
            NumericUpDown numericUpDown = sender as NumericUpDown;
            if (e.X <= numericUpDown.Controls[1].Width + 1)
            {
                // Click is in text area
                numericUpDown.Value = MouseСoordinates.X;
            }
        }

        private void numericUpDown_picturesY_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MouseСoordinates.Y < 0) return;
            NumericUpDown numericUpDown = sender as NumericUpDown;
            if (e.X <= numericUpDown.Controls[1].Width + 1)
            {
                // Click is in text area
                numericUpDown.Value = MouseСoordinates.Y;
            }
        }

        private void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown numericUpDown = sender as NumericUpDown;
            if (numericUpDown.Name == "numericUpDown_SystemFont_size")
            {
                if (numericUpDown.Value > 47) numericUpDown.Value = 47;
                if (numericUpDown.Value < 1) numericUpDown.Value = 1;
            }

            if (ValueChanged != null && !setValue)
            {
                EventArgs eventArgs = new EventArgs();
                ValueChanged(this, eventArgs);
            }
        }

        #endregion

        private void checkBox_follow_CheckedChanged(object sender, EventArgs e)
        {
            bool b = !checkBox_follow.Checked;
            label01.Enabled = b;
            label02.Enabled = b;
            label03.Enabled = b;
            label04.Enabled = b;
            label05.Enabled = b;
            label06.Enabled = b;
            label07.Enabled = b;

            numericUpDown_SystemFontX.Enabled = b;
            numericUpDown_SystemFontY.Enabled = b;

            numericUpDown_SystemFont_size.Enabled = b;
            numericUpDown_SystemFont_angle.Enabled = b;
            numericUpDown_SystemFont_spacing.Enabled = b;
            comboBox_SystemFont_color.Enabled = b;
        }
    }
}
