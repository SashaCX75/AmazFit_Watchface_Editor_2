
namespace AmazFit_Watchface_2
{
    partial class UserControl_scaleLinear
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_scaleLinear));
            this.panel_scaleLinear = new System.Windows.Forms.Panel();
            this.checkBox_scaleLinear_Use = new System.Windows.Forms.CheckBox();
            this.radioButton_scaleLinear_image = new System.Windows.Forms.RadioButton();
            this.radioButton_scaleLinear_color = new System.Windows.Forms.RadioButton();
            this.comboBox_scaleLinear_image = new System.Windows.Forms.ComboBox();
            this.comboBox_scaleLinear_color = new System.Windows.Forms.ComboBox();
            this.comboBox_scaleLinear_image_pointer = new System.Windows.Forms.ComboBox();
            this.comboBox_scaleLinear_image_background = new System.Windows.Forms.ComboBox();
            this.numericUpDown_scaleLinearX = new System.Windows.Forms.NumericUpDown();
            this.contextMenuStrip_X = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.вставитьКоординатуХToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.копироватьToolStripMenuItemX = new System.Windows.Forms.ToolStripMenuItem();
            this.вставитьToolStripMenuItemX = new System.Windows.Forms.ToolStripMenuItem();
            this.numericUpDown_scaleLinearY = new System.Windows.Forms.NumericUpDown();
            this.contextMenuStrip_Y = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.вставитьКоординатуYToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.копироватьToolStripMenuItemY = new System.Windows.Forms.ToolStripMenuItem();
            this.вставитьToolStripMenuItemY = new System.Windows.Forms.ToolStripMenuItem();
            this.numericUpDown_scaleLinear_length = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_scaleLinear_width = new System.Windows.Forms.NumericUpDown();
            this.comboBox_scaleLinear_flatness = new System.Windows.Forms.ComboBox();
            this.label02 = new System.Windows.Forms.Label();
            this.label01 = new System.Windows.Forms.Label();
            this.label04 = new System.Windows.Forms.Label();
            this.label03 = new System.Windows.Forms.Label();
            this.label06 = new System.Windows.Forms.Label();
            this.label05 = new System.Windows.Forms.Label();
            this.label07 = new System.Windows.Forms.Label();
            this.label08 = new System.Windows.Forms.Label();
            this.button_Copy_scaleLinear = new System.Windows.Forms.Button();
            this.button_scaleLinear = new System.Windows.Forms.Button();
            this.panel_scaleLinear.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_scaleLinearX)).BeginInit();
            this.contextMenuStrip_X.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_scaleLinearY)).BeginInit();
            this.contextMenuStrip_Y.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_scaleLinear_length)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_scaleLinear_width)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_scaleLinear
            // 
            resources.ApplyResources(this.panel_scaleLinear, "panel_scaleLinear");
            this.panel_scaleLinear.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_scaleLinear.Controls.Add(this.checkBox_scaleLinear_Use);
            this.panel_scaleLinear.Controls.Add(this.radioButton_scaleLinear_image);
            this.panel_scaleLinear.Controls.Add(this.radioButton_scaleLinear_color);
            this.panel_scaleLinear.Controls.Add(this.comboBox_scaleLinear_image);
            this.panel_scaleLinear.Controls.Add(this.comboBox_scaleLinear_color);
            this.panel_scaleLinear.Controls.Add(this.comboBox_scaleLinear_image_pointer);
            this.panel_scaleLinear.Controls.Add(this.comboBox_scaleLinear_image_background);
            this.panel_scaleLinear.Controls.Add(this.numericUpDown_scaleLinearX);
            this.panel_scaleLinear.Controls.Add(this.numericUpDown_scaleLinearY);
            this.panel_scaleLinear.Controls.Add(this.numericUpDown_scaleLinear_length);
            this.panel_scaleLinear.Controls.Add(this.numericUpDown_scaleLinear_width);
            this.panel_scaleLinear.Controls.Add(this.comboBox_scaleLinear_flatness);
            this.panel_scaleLinear.Controls.Add(this.label02);
            this.panel_scaleLinear.Controls.Add(this.label01);
            this.panel_scaleLinear.Controls.Add(this.label04);
            this.panel_scaleLinear.Controls.Add(this.label03);
            this.panel_scaleLinear.Controls.Add(this.label06);
            this.panel_scaleLinear.Controls.Add(this.label05);
            this.panel_scaleLinear.Controls.Add(this.label07);
            this.panel_scaleLinear.Controls.Add(this.label08);
            this.panel_scaleLinear.Controls.Add(this.button_Copy_scaleLinear);
            this.panel_scaleLinear.Name = "panel_scaleLinear";
            // 
            // checkBox_scaleLinear_Use
            // 
            resources.ApplyResources(this.checkBox_scaleLinear_Use, "checkBox_scaleLinear_Use");
            this.checkBox_scaleLinear_Use.Name = "checkBox_scaleLinear_Use";
            this.checkBox_scaleLinear_Use.UseVisualStyleBackColor = true;
            this.checkBox_scaleLinear_Use.CheckedChanged += new System.EventHandler(this.checkBox_Use_CheckedChanged);
            this.checkBox_scaleLinear_Use.Click += new System.EventHandler(this.checkBox_Click);
            // 
            // radioButton_scaleLinear_image
            // 
            resources.ApplyResources(this.radioButton_scaleLinear_image, "radioButton_scaleLinear_image");
            this.radioButton_scaleLinear_image.Checked = true;
            this.radioButton_scaleLinear_image.Name = "radioButton_scaleLinear_image";
            this.radioButton_scaleLinear_image.TabStop = true;
            this.radioButton_scaleLinear_image.UseVisualStyleBackColor = true;
            this.radioButton_scaleLinear_image.CheckedChanged += new System.EventHandler(this.radioButton_image_color_CheckedChanged);
            // 
            // radioButton_scaleLinear_color
            // 
            resources.ApplyResources(this.radioButton_scaleLinear_color, "radioButton_scaleLinear_color");
            this.radioButton_scaleLinear_color.Name = "radioButton_scaleLinear_color";
            this.radioButton_scaleLinear_color.TabStop = true;
            this.radioButton_scaleLinear_color.UseVisualStyleBackColor = true;
            // 
            // comboBox_scaleLinear_image
            // 
            resources.ApplyResources(this.comboBox_scaleLinear_image, "comboBox_scaleLinear_image");
            this.comboBox_scaleLinear_image.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBox_scaleLinear_image.DropDownWidth = 75;
            this.comboBox_scaleLinear_image.FormattingEnabled = true;
            this.comboBox_scaleLinear_image.Name = "comboBox_scaleLinear_image";
            this.comboBox_scaleLinear_image.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBox_DrawItem);
            this.comboBox_scaleLinear_image.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.comboBox_MeasureItem);
            this.comboBox_scaleLinear_image.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            this.comboBox_scaleLinear_image.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox_KeyDown);
            this.comboBox_scaleLinear_image.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
            // 
            // comboBox_scaleLinear_color
            // 
            resources.ApplyResources(this.comboBox_scaleLinear_color, "comboBox_scaleLinear_color");
            this.comboBox_scaleLinear_color.BackColor = System.Drawing.Color.DarkOrange;
            this.comboBox_scaleLinear_color.DropDownHeight = 1;
            this.comboBox_scaleLinear_color.FormattingEnabled = true;
            this.comboBox_scaleLinear_color.Name = "comboBox_scaleLinear_color";
            this.comboBox_scaleLinear_color.Click += new System.EventHandler(this.comboBox_color_Click);
            this.comboBox_scaleLinear_color.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
            // 
            // comboBox_scaleLinear_image_pointer
            // 
            resources.ApplyResources(this.comboBox_scaleLinear_image_pointer, "comboBox_scaleLinear_image_pointer");
            this.comboBox_scaleLinear_image_pointer.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBox_scaleLinear_image_pointer.DropDownWidth = 75;
            this.comboBox_scaleLinear_image_pointer.FormattingEnabled = true;
            this.comboBox_scaleLinear_image_pointer.Name = "comboBox_scaleLinear_image_pointer";
            this.comboBox_scaleLinear_image_pointer.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBox_DrawItem);
            this.comboBox_scaleLinear_image_pointer.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.comboBox_MeasureItem);
            this.comboBox_scaleLinear_image_pointer.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            this.comboBox_scaleLinear_image_pointer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox_KeyDown);
            this.comboBox_scaleLinear_image_pointer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
            // 
            // comboBox_scaleLinear_image_background
            // 
            resources.ApplyResources(this.comboBox_scaleLinear_image_background, "comboBox_scaleLinear_image_background");
            this.comboBox_scaleLinear_image_background.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBox_scaleLinear_image_background.DropDownWidth = 75;
            this.comboBox_scaleLinear_image_background.FormattingEnabled = true;
            this.comboBox_scaleLinear_image_background.Name = "comboBox_scaleLinear_image_background";
            this.comboBox_scaleLinear_image_background.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBox_DrawItem);
            this.comboBox_scaleLinear_image_background.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.comboBox_MeasureItem);
            this.comboBox_scaleLinear_image_background.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            this.comboBox_scaleLinear_image_background.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox_KeyDown);
            this.comboBox_scaleLinear_image_background.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
            // 
            // numericUpDown_scaleLinearX
            // 
            resources.ApplyResources(this.numericUpDown_scaleLinearX, "numericUpDown_scaleLinearX");
            this.numericUpDown_scaleLinearX.ContextMenuStrip = this.contextMenuStrip_X;
            this.numericUpDown_scaleLinearX.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numericUpDown_scaleLinearX.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.numericUpDown_scaleLinearX.Name = "numericUpDown_scaleLinearX";
            this.numericUpDown_scaleLinearX.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            this.numericUpDown_scaleLinearX.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.numericUpDown_picturesX_MouseDoubleClick);
            // 
            // contextMenuStrip_X
            // 
            resources.ApplyResources(this.contextMenuStrip_X, "contextMenuStrip_X");
            this.contextMenuStrip_X.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip_X.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.вставитьКоординатуХToolStripMenuItem,
            this.копироватьToolStripMenuItemX,
            this.вставитьToolStripMenuItemX});
            this.contextMenuStrip_X.Name = "contextMenuStrip_X";
            this.contextMenuStrip_X.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_X_Opening);
            // 
            // вставитьКоординатуХToolStripMenuItem
            // 
            resources.ApplyResources(this.вставитьКоординатуХToolStripMenuItem, "вставитьКоординатуХToolStripMenuItem");
            this.вставитьКоординатуХToolStripMenuItem.Name = "вставитьКоординатуХToolStripMenuItem";
            this.вставитьКоординатуХToolStripMenuItem.Click += new System.EventHandler(this.вставитьКоординатуХToolStripMenuItem_Click);
            // 
            // копироватьToolStripMenuItemX
            // 
            resources.ApplyResources(this.копироватьToolStripMenuItemX, "копироватьToolStripMenuItemX");
            this.копироватьToolStripMenuItemX.Name = "копироватьToolStripMenuItemX";
            this.копироватьToolStripMenuItemX.Click += new System.EventHandler(this.копироватьToolStripMenuItem_Click);
            // 
            // вставитьToolStripMenuItemX
            // 
            resources.ApplyResources(this.вставитьToolStripMenuItemX, "вставитьToolStripMenuItemX");
            this.вставитьToolStripMenuItemX.Name = "вставитьToolStripMenuItemX";
            this.вставитьToolStripMenuItemX.Click += new System.EventHandler(this.вставитьToolStripMenuItem_Click);
            // 
            // numericUpDown_scaleLinearY
            // 
            resources.ApplyResources(this.numericUpDown_scaleLinearY, "numericUpDown_scaleLinearY");
            this.numericUpDown_scaleLinearY.ContextMenuStrip = this.contextMenuStrip_Y;
            this.numericUpDown_scaleLinearY.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numericUpDown_scaleLinearY.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.numericUpDown_scaleLinearY.Name = "numericUpDown_scaleLinearY";
            this.numericUpDown_scaleLinearY.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            this.numericUpDown_scaleLinearY.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.numericUpDown_picturesY_MouseDoubleClick);
            // 
            // contextMenuStrip_Y
            // 
            resources.ApplyResources(this.contextMenuStrip_Y, "contextMenuStrip_Y");
            this.contextMenuStrip_Y.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip_Y.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.вставитьКоординатуYToolStripMenuItem,
            this.копироватьToolStripMenuItemY,
            this.вставитьToolStripMenuItemY});
            this.contextMenuStrip_Y.Name = "contextMenuStrip_X";
            this.contextMenuStrip_Y.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Y_Opening);
            // 
            // вставитьКоординатуYToolStripMenuItem
            // 
            resources.ApplyResources(this.вставитьКоординатуYToolStripMenuItem, "вставитьКоординатуYToolStripMenuItem");
            this.вставитьКоординатуYToolStripMenuItem.Name = "вставитьКоординатуYToolStripMenuItem";
            this.вставитьКоординатуYToolStripMenuItem.Click += new System.EventHandler(this.вставитьКоординатуYToolStripMenuItem_Click);
            // 
            // копироватьToolStripMenuItemY
            // 
            resources.ApplyResources(this.копироватьToolStripMenuItemY, "копироватьToolStripMenuItemY");
            this.копироватьToolStripMenuItemY.Name = "копироватьToolStripMenuItemY";
            this.копироватьToolStripMenuItemY.Click += new System.EventHandler(this.копироватьToolStripMenuItem_Click);
            // 
            // вставитьToolStripMenuItemY
            // 
            resources.ApplyResources(this.вставитьToolStripMenuItemY, "вставитьToolStripMenuItemY");
            this.вставитьToolStripMenuItemY.Name = "вставитьToolStripMenuItemY";
            this.вставитьToolStripMenuItemY.Click += new System.EventHandler(this.вставитьToolStripMenuItem_Click);
            // 
            // numericUpDown_scaleLinear_length
            // 
            resources.ApplyResources(this.numericUpDown_scaleLinear_length, "numericUpDown_scaleLinear_length");
            this.numericUpDown_scaleLinear_length.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numericUpDown_scaleLinear_length.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.numericUpDown_scaleLinear_length.Name = "numericUpDown_scaleLinear_length";
            this.numericUpDown_scaleLinear_length.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDown_scaleLinear_length.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            // 
            // numericUpDown_scaleLinear_width
            // 
            resources.ApplyResources(this.numericUpDown_scaleLinear_width, "numericUpDown_scaleLinear_width");
            this.numericUpDown_scaleLinear_width.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numericUpDown_scaleLinear_width.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.numericUpDown_scaleLinear_width.Name = "numericUpDown_scaleLinear_width";
            this.numericUpDown_scaleLinear_width.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown_scaleLinear_width.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            // 
            // comboBox_scaleLinear_flatness
            // 
            resources.ApplyResources(this.comboBox_scaleLinear_flatness, "comboBox_scaleLinear_flatness");
            this.comboBox_scaleLinear_flatness.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_scaleLinear_flatness.FormattingEnabled = true;
            this.comboBox_scaleLinear_flatness.Items.AddRange(new object[] {
            resources.GetString("comboBox_scaleLinear_flatness.Items"),
            resources.GetString("comboBox_scaleLinear_flatness.Items1")});
            this.comboBox_scaleLinear_flatness.Name = "comboBox_scaleLinear_flatness";
            this.comboBox_scaleLinear_flatness.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // label02
            // 
            resources.ApplyResources(this.label02, "label02");
            this.label02.Name = "label02";
            // 
            // label01
            // 
            resources.ApplyResources(this.label01, "label01");
            this.label01.Name = "label01";
            // 
            // label04
            // 
            resources.ApplyResources(this.label04, "label04");
            this.label04.Name = "label04";
            // 
            // label03
            // 
            resources.ApplyResources(this.label03, "label03");
            this.label03.Name = "label03";
            // 
            // label06
            // 
            resources.ApplyResources(this.label06, "label06");
            this.label06.Name = "label06";
            // 
            // label05
            // 
            resources.ApplyResources(this.label05, "label05");
            this.label05.Name = "label05";
            // 
            // label07
            // 
            resources.ApplyResources(this.label07, "label07");
            this.label07.Name = "label07";
            // 
            // label08
            // 
            resources.ApplyResources(this.label08, "label08");
            this.label08.Name = "label08";
            // 
            // button_Copy_scaleLinear
            // 
            resources.ApplyResources(this.button_Copy_scaleLinear, "button_Copy_scaleLinear");
            this.button_Copy_scaleLinear.Name = "button_Copy_scaleLinear";
            this.button_Copy_scaleLinear.UseVisualStyleBackColor = true;
            this.button_Copy_scaleLinear.Click += new System.EventHandler(this.button_Copy_scaleLinear_Click);
            // 
            // button_scaleLinear
            // 
            resources.ApplyResources(this.button_scaleLinear, "button_scaleLinear");
            this.button_scaleLinear.Name = "button_scaleLinear";
            this.button_scaleLinear.UseVisualStyleBackColor = true;
            this.button_scaleLinear.Click += new System.EventHandler(this.button_Click);
            // 
            // UserControl_scaleLinear
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_scaleLinear);
            this.Controls.Add(this.button_scaleLinear);
            this.Name = "UserControl_scaleLinear";
            this.panel_scaleLinear.ResumeLayout(false);
            this.panel_scaleLinear.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_scaleLinearX)).EndInit();
            this.contextMenuStrip_X.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_scaleLinearY)).EndInit();
            this.contextMenuStrip_Y.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_scaleLinear_length)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_scaleLinear_width)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_scaleLinear;
        private System.Windows.Forms.ComboBox comboBox_scaleLinear_image;
        private System.Windows.Forms.ComboBox comboBox_scaleLinear_color;
        private System.Windows.Forms.ComboBox comboBox_scaleLinear_image_pointer;
        private System.Windows.Forms.ComboBox comboBox_scaleLinear_image_background;
        private System.Windows.Forms.ComboBox comboBox_scaleLinear_flatness;
        private System.Windows.Forms.Label label02;
        private System.Windows.Forms.Label label01;
        private System.Windows.Forms.Label label04;
        private System.Windows.Forms.Label label03;
        private System.Windows.Forms.Label label06;
        private System.Windows.Forms.Label label05;
        private System.Windows.Forms.Label label07;
        private System.Windows.Forms.Label label08;
        private System.Windows.Forms.Button button_Copy_scaleLinear;
        private System.Windows.Forms.Button button_scaleLinear;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_X;
        private System.Windows.Forms.ToolStripMenuItem вставитьКоординатуХToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem копироватьToolStripMenuItemX;
        private System.Windows.Forms.ToolStripMenuItem вставитьToolStripMenuItemX;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Y;
        private System.Windows.Forms.ToolStripMenuItem вставитьКоординатуYToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem копироватьToolStripMenuItemY;
        private System.Windows.Forms.ToolStripMenuItem вставитьToolStripMenuItemY;
        internal System.Windows.Forms.RadioButton radioButton_scaleLinear_image;
        internal System.Windows.Forms.RadioButton radioButton_scaleLinear_color;
        internal System.Windows.Forms.CheckBox checkBox_scaleLinear_Use;
        internal System.Windows.Forms.NumericUpDown numericUpDown_scaleLinearX;
        internal System.Windows.Forms.NumericUpDown numericUpDown_scaleLinearY;
        internal System.Windows.Forms.NumericUpDown numericUpDown_scaleLinear_length;
        internal System.Windows.Forms.NumericUpDown numericUpDown_scaleLinear_width;
    }
}
