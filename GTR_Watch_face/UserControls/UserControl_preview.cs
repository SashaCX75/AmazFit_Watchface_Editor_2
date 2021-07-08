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
    public partial class UserControl_preview : UserControl
    {
        private bool setValue;
        public UserControl_preview()
        {
            InitializeComponent();
            setValue = false;
        }

        [Browsable(true)]
        public event CollapseHandler Collapse;
        public delegate void CollapseHandler(object sender, EventArgs eventArgs);

        [Browsable(true)]
        public event ValueChangedHandler ValueChanged;
        public delegate void ValueChangedHandler(object sender, EventArgs eventArgs);


        /// <summary>Возвращает true если панель свернута</summary>
        public bool Collapsed
        {
            get
            {
                return !panel_preview.Visible;
            }
            set
            {
                panel_preview.Visible = !value;
            }
        }

        private void button_preview_Click(object sender, EventArgs e)
        {
            panel_preview.Visible = !panel_preview.Visible;
            if (Collapse != null)
            {
                EventArgs eventArgs = new EventArgs();
                Collapse(this, eventArgs);
            }
        }

        internal void comboBoxSetImage(long value)
        {
            comboBox_icon_image.Text = value.ToString();
            if (comboBox_icon_image.SelectedIndex < 0) comboBox_icon_image.Text = "";
        }

        /// <summary>Возвращает номер выбранной картинки, в случае ошибки возвращает -1</summary>
        internal int comboBoxGetImage()
        {
            if (comboBox_icon_image.SelectedIndex < 0) return -1;
            int value = -1;
            Int32.TryParse(comboBox_icon_image.Text, out value);
            return value;
        }

        /// <summary>Возвращает SelectedIndex выпадающего списка</summary>
        internal int comboBoxGetSelectedIndexImage()
        {
            return comboBox_icon_image.SelectedIndex;
        }

        #region Standard events
        private void comboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Delete) || (e.KeyCode == Keys.Back))
            {
                ComboBox comboBox = sender as ComboBox;
                comboBox.Text = "";
                comboBox.SelectedIndex = -1;
                if (ValueChanged != null && !setValue)
                {
                    EventArgs eventArgs = new EventArgs();
                    ValueChanged(this, eventArgs);
                }
            }
        }

        private void comboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboBox_DrawItem(object sender, DrawItemEventArgs e)
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
                    using (FileStream stream = new FileStream(Form1.ListImagesFullName[e.Index], FileMode.Open, FileAccess.Read))
                    {
                        Image image = Image.FromStream(stream);
                        float scale = (float)itemWidth / image.Width;
                        if ((float)itemHeight / image.Height < scale) scale = (float)itemHeight / image.Height;
                        float itemWidthRec = image.Width * scale;
                        float itemHeightRec = image.Height * scale;
                        Rectangle rectangle = new Rectangle((int)(itemWidth - itemWidthRec) / 2 + 2,
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

        private void comboBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 35;
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ValueChanged != null && !setValue)
            {
                EventArgs eventArgs = new EventArgs();
                ValueChanged(this, eventArgs);
            }
        }
        #endregion

        #region Settings Set/Clear
        /// <summary>Добавляет ссылки на картинки в выпадающие списки</summary>
        internal void ComboBoxAddItems(List<string> ListImages)
        {
            comboBox_icon_image.Items.AddRange(ListImages.ToArray());
        }

        /// <summary>Очищает выпадающие списки с картинками, сбрасывает данные на значения по умолчанию</summary>
        internal void SettingsClear(bool full = true)
        {
            setValue = true;

            if (full)
            {
                comboBox_icon_image.Items.Clear();
            }
            comboBox_icon_image.Text = null;

            setValue = false;
        }
        #endregion
    }
}
