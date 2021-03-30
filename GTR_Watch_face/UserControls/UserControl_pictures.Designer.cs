
namespace AmazFit_Watchface_2
{
    partial class UserControl_pictures
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_pictures));
            this.button_pictures = new System.Windows.Forms.Button();
            this.panel_pictures = new System.Windows.Forms.Panel();
            this.checkBox_pictures_Use = new System.Windows.Forms.CheckBox();
            this.comboBox_pictures_image = new System.Windows.Forms.ComboBox();
            this.numericUpDown_picturesX = new System.Windows.Forms.NumericUpDown();
            this.contextMenuStrip_X = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.вставитьКоординатуХToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.копироватьToolStripMenuItemX = new System.Windows.Forms.ToolStripMenuItem();
            this.вставитьToolStripMenuItemX = new System.Windows.Forms.ToolStripMenuItem();
            this.numericUpDown_picturesY = new System.Windows.Forms.NumericUpDown();
            this.contextMenuStrip_Y = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.вставитьКоординатуYToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.копироватьToolStripMenuItemY = new System.Windows.Forms.ToolStripMenuItem();
            this.вставитьToolStripMenuItemY = new System.Windows.Forms.ToolStripMenuItem();
            this.numericUpDown_pictures_count = new System.Windows.Forms.NumericUpDown();
            this.label01 = new System.Windows.Forms.Label();
            this.label02 = new System.Windows.Forms.Label();
            this.label03 = new System.Windows.Forms.Label();
            this.label04 = new System.Windows.Forms.Label();
            this.label05 = new System.Windows.Forms.Label();
            this.button_Copy_pictures = new System.Windows.Forms.Button();
            this.panel_pictures.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_picturesX)).BeginInit();
            this.contextMenuStrip_X.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_picturesY)).BeginInit();
            this.contextMenuStrip_Y.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_pictures_count)).BeginInit();
            this.SuspendLayout();
            // 
            // button_pictures
            // 
            this.button_pictures.Dock = System.Windows.Forms.DockStyle.Top;
            this.button_pictures.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button_pictures.Location = new System.Drawing.Point(0, 0);
            this.button_pictures.Name = "button_pictures";
            this.button_pictures.Size = new System.Drawing.Size(435, 23);
            this.button_pictures.TabIndex = 155;
            this.button_pictures.Text = "Набор изображений";
            this.button_pictures.UseVisualStyleBackColor = true;
            this.button_pictures.Click += new System.EventHandler(this.button_pictures_Click);
            // 
            // panel_pictures
            // 
            this.panel_pictures.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_pictures.Controls.Add(this.checkBox_pictures_Use);
            this.panel_pictures.Controls.Add(this.comboBox_pictures_image);
            this.panel_pictures.Controls.Add(this.numericUpDown_picturesX);
            this.panel_pictures.Controls.Add(this.numericUpDown_picturesY);
            this.panel_pictures.Controls.Add(this.numericUpDown_pictures_count);
            this.panel_pictures.Controls.Add(this.label01);
            this.panel_pictures.Controls.Add(this.label02);
            this.panel_pictures.Controls.Add(this.label03);
            this.panel_pictures.Controls.Add(this.label04);
            this.panel_pictures.Controls.Add(this.label05);
            this.panel_pictures.Controls.Add(this.button_Copy_pictures);
            this.panel_pictures.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_pictures.Location = new System.Drawing.Point(0, 23);
            this.panel_pictures.Name = "panel_pictures";
            this.panel_pictures.Size = new System.Drawing.Size(435, 85);
            this.panel_pictures.TabIndex = 156;
            // 
            // checkBox_pictures_Use
            // 
            this.checkBox_pictures_Use.AutoSize = true;
            this.checkBox_pictures_Use.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkBox_pictures_Use.Location = new System.Drawing.Point(3, 6);
            this.checkBox_pictures_Use.Name = "checkBox_pictures_Use";
            this.checkBox_pictures_Use.Size = new System.Drawing.Size(99, 17);
            this.checkBox_pictures_Use.TabIndex = 105;
            this.checkBox_pictures_Use.Text = "Использовать";
            this.checkBox_pictures_Use.UseVisualStyleBackColor = true;
            this.checkBox_pictures_Use.CheckedChanged += new System.EventHandler(this.checkBox_pictures_Use_CheckedChanged);
            this.checkBox_pictures_Use.Click += new System.EventHandler(this.checkBox_pictures_Use_Click);
            // 
            // comboBox_pictures_image
            // 
            this.comboBox_pictures_image.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBox_pictures_image.DropDownWidth = 75;
            this.comboBox_pictures_image.Enabled = false;
            this.comboBox_pictures_image.FormattingEnabled = true;
            this.comboBox_pictures_image.Location = new System.Drawing.Point(3, 58);
            this.comboBox_pictures_image.MaxDropDownItems = 25;
            this.comboBox_pictures_image.Name = "comboBox_pictures_image";
            this.comboBox_pictures_image.Size = new System.Drawing.Size(56, 21);
            this.comboBox_pictures_image.TabIndex = 100;
            this.comboBox_pictures_image.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBox_DrawItem);
            this.comboBox_pictures_image.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.comboBox_MeasureItem);
            this.comboBox_pictures_image.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            this.comboBox_pictures_image.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox_KeyDown);
            this.comboBox_pictures_image.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
            // 
            // numericUpDown_picturesX
            // 
            this.numericUpDown_picturesX.ContextMenuStrip = this.contextMenuStrip_X;
            this.numericUpDown_picturesX.Enabled = false;
            this.numericUpDown_picturesX.Location = new System.Drawing.Point(120, 58);
            this.numericUpDown_picturesX.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numericUpDown_picturesX.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.numericUpDown_picturesX.Name = "numericUpDown_picturesX";
            this.numericUpDown_picturesX.Size = new System.Drawing.Size(40, 20);
            this.numericUpDown_picturesX.TabIndex = 103;
            this.numericUpDown_picturesX.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            this.numericUpDown_picturesX.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.numericUpDown_picturesX_MouseDoubleClick);
            // 
            // contextMenuStrip_X
            // 
            this.contextMenuStrip_X.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip_X.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.вставитьКоординатуХToolStripMenuItem,
            this.копироватьToolStripMenuItemX,
            this.вставитьToolStripMenuItemX});
            this.contextMenuStrip_X.Name = "contextMenuStrip_X";
            this.contextMenuStrip_X.Size = new System.Drawing.Size(204, 82);
            this.contextMenuStrip_X.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_X_Opening);
            // 
            // вставитьКоординатуХToolStripMenuItem
            // 
            this.вставитьКоординатуХToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("вставитьКоординатуХToolStripMenuItem.Image")));
            this.вставитьКоординатуХToolStripMenuItem.Name = "вставитьКоординатуХToolStripMenuItem";
            this.вставитьКоординатуХToolStripMenuItem.Size = new System.Drawing.Size(203, 26);
            this.вставитьКоординатуХToolStripMenuItem.Text = "Вставить координату Х";
            this.вставитьКоординатуХToolStripMenuItem.Click += new System.EventHandler(this.вставитьКоординатуХToolStripMenuItem_Click);
            // 
            // копироватьToolStripMenuItemX
            // 
            this.копироватьToolStripMenuItemX.Image = ((System.Drawing.Image)(resources.GetObject("копироватьToolStripMenuItemX.Image")));
            this.копироватьToolStripMenuItemX.Name = "копироватьToolStripMenuItemX";
            this.копироватьToolStripMenuItemX.Size = new System.Drawing.Size(203, 26);
            this.копироватьToolStripMenuItemX.Text = "Копировать";
            this.копироватьToolStripMenuItemX.Click += new System.EventHandler(this.копироватьToolStripMenuItem_Click);
            // 
            // вставитьToolStripMenuItemX
            // 
            this.вставитьToolStripMenuItemX.Image = ((System.Drawing.Image)(resources.GetObject("вставитьToolStripMenuItemX.Image")));
            this.вставитьToolStripMenuItemX.Name = "вставитьToolStripMenuItemX";
            this.вставитьToolStripMenuItemX.Size = new System.Drawing.Size(203, 26);
            this.вставитьToolStripMenuItemX.Text = "Вставить";
            this.вставитьToolStripMenuItemX.Click += new System.EventHandler(this.вставитьToolStripMenuItem_Click);
            // 
            // numericUpDown_picturesY
            // 
            this.numericUpDown_picturesY.ContextMenuStrip = this.contextMenuStrip_Y;
            this.numericUpDown_picturesY.Enabled = false;
            this.numericUpDown_picturesY.Location = new System.Drawing.Point(193, 58);
            this.numericUpDown_picturesY.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numericUpDown_picturesY.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.numericUpDown_picturesY.Name = "numericUpDown_picturesY";
            this.numericUpDown_picturesY.Size = new System.Drawing.Size(40, 20);
            this.numericUpDown_picturesY.TabIndex = 104;
            this.numericUpDown_picturesY.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            this.numericUpDown_picturesY.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.numericUpDown_picturesY_MouseDoubleClick);
            // 
            // contextMenuStrip_Y
            // 
            this.contextMenuStrip_Y.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip_Y.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.вставитьКоординатуYToolStripMenuItem,
            this.копироватьToolStripMenuItemY,
            this.вставитьToolStripMenuItemY});
            this.contextMenuStrip_Y.Name = "contextMenuStrip_X";
            this.contextMenuStrip_Y.Size = new System.Drawing.Size(204, 82);
            this.contextMenuStrip_Y.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Y_Opening);
            // 
            // вставитьКоординатуYToolStripMenuItem
            // 
            this.вставитьКоординатуYToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("вставитьКоординатуYToolStripMenuItem.Image")));
            this.вставитьКоординатуYToolStripMenuItem.Name = "вставитьКоординатуYToolStripMenuItem";
            this.вставитьКоординатуYToolStripMenuItem.Size = new System.Drawing.Size(203, 26);
            this.вставитьКоординатуYToolStripMenuItem.Text = "Вставить координату Y";
            this.вставитьКоординатуYToolStripMenuItem.Click += new System.EventHandler(this.вставитьКоординатуYToolStripMenuItem_Click);
            // 
            // копироватьToolStripMenuItemY
            // 
            this.копироватьToolStripMenuItemY.Image = ((System.Drawing.Image)(resources.GetObject("копироватьToolStripMenuItemY.Image")));
            this.копироватьToolStripMenuItemY.Name = "копироватьToolStripMenuItemY";
            this.копироватьToolStripMenuItemY.Size = new System.Drawing.Size(203, 26);
            this.копироватьToolStripMenuItemY.Text = "Копировать";
            this.копироватьToolStripMenuItemY.Click += new System.EventHandler(this.копироватьToolStripMenuItem_Click);
            // 
            // вставитьToolStripMenuItemY
            // 
            this.вставитьToolStripMenuItemY.Image = ((System.Drawing.Image)(resources.GetObject("вставитьToolStripMenuItemY.Image")));
            this.вставитьToolStripMenuItemY.Name = "вставитьToolStripMenuItemY";
            this.вставитьToolStripMenuItemY.Size = new System.Drawing.Size(203, 26);
            this.вставитьToolStripMenuItemY.Text = "Вставить";
            this.вставитьToolStripMenuItemY.Click += new System.EventHandler(this.вставитьToolStripMenuItem_Click);
            // 
            // numericUpDown_pictures_count
            // 
            this.numericUpDown_pictures_count.Enabled = false;
            this.numericUpDown_pictures_count.Location = new System.Drawing.Point(275, 58);
            this.numericUpDown_pictures_count.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numericUpDown_pictures_count.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.numericUpDown_pictures_count.Name = "numericUpDown_pictures_count";
            this.numericUpDown_pictures_count.Size = new System.Drawing.Size(40, 20);
            this.numericUpDown_pictures_count.TabIndex = 107;
            this.numericUpDown_pictures_count.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown_pictures_count.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            // 
            // label01
            // 
            this.label01.Enabled = false;
            this.label01.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label01.Location = new System.Drawing.Point(3, 28);
            this.label01.Name = "label01";
            this.label01.Size = new System.Drawing.Size(80, 30);
            this.label01.TabIndex = 99;
            this.label01.Text = "Стартовое изображение";
            // 
            // label02
            // 
            this.label02.AutoSize = true;
            this.label02.Enabled = false;
            this.label02.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label02.Location = new System.Drawing.Point(103, 41);
            this.label02.Margin = new System.Windows.Forms.Padding(3);
            this.label02.Name = "label02";
            this.label02.Size = new System.Drawing.Size(69, 13);
            this.label02.TabIndex = 106;
            this.label02.Text = "Координаты";
            // 
            // label03
            // 
            this.label03.Enabled = false;
            this.label03.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label03.Location = new System.Drawing.Point(256, 28);
            this.label03.Name = "label03";
            this.label03.Size = new System.Drawing.Size(80, 30);
            this.label03.TabIndex = 108;
            this.label03.Text = "Количество изображений";
            this.label03.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label04
            // 
            this.label04.AutoSize = true;
            this.label04.Enabled = false;
            this.label04.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label04.Location = new System.Drawing.Point(103, 60);
            this.label04.Name = "label04";
            this.label04.Size = new System.Drawing.Size(17, 13);
            this.label04.TabIndex = 101;
            this.label04.Text = "X:";
            // 
            // label05
            // 
            this.label05.AutoSize = true;
            this.label05.Enabled = false;
            this.label05.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label05.Location = new System.Drawing.Point(176, 60);
            this.label05.Name = "label05";
            this.label05.Size = new System.Drawing.Size(17, 13);
            this.label05.TabIndex = 102;
            this.label05.Text = "Y:";
            // 
            // button_Copy_pictures
            // 
            this.button_Copy_pictures.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button_Copy_pictures.Location = new System.Drawing.Point(120, 3);
            this.button_Copy_pictures.Name = "button_Copy_pictures";
            this.button_Copy_pictures.Size = new System.Drawing.Size(195, 23);
            this.button_Copy_pictures.TabIndex = 147;
            this.button_Copy_pictures.Text = "Скопировать с основного экрана";
            this.button_Copy_pictures.UseVisualStyleBackColor = true;
            this.button_Copy_pictures.Click += new System.EventHandler(this.button_Copy_pictures_Click);
            // 
            // UserControl_pictures
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_pictures);
            this.Controls.Add(this.button_pictures);
            this.Name = "UserControl_pictures";
            this.Size = new System.Drawing.Size(435, 150);
            this.panel_pictures.ResumeLayout(false);
            this.panel_pictures.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_picturesX)).EndInit();
            this.contextMenuStrip_X.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_picturesY)).EndInit();
            this.contextMenuStrip_Y.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_pictures_count)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_pictures;
        private System.Windows.Forms.Panel panel_pictures;
        private System.Windows.Forms.CheckBox checkBox_pictures_Use;
        private System.Windows.Forms.ComboBox comboBox_pictures_image;
        private System.Windows.Forms.Label label02;
        private System.Windows.Forms.Label label01;
        private System.Windows.Forms.Label label05;
        private System.Windows.Forms.Label label04;
        private System.Windows.Forms.Label label03;
        private System.Windows.Forms.Button button_Copy_pictures;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_X;
        private System.Windows.Forms.ToolStripMenuItem вставитьКоординатуХToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem копироватьToolStripMenuItemX;
        private System.Windows.Forms.ToolStripMenuItem вставитьToolStripMenuItemX;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Y;
        private System.Windows.Forms.ToolStripMenuItem вставитьКоординатуYToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem копироватьToolStripMenuItemY;
        private System.Windows.Forms.ToolStripMenuItem вставитьToolStripMenuItemY;
        internal System.Windows.Forms.NumericUpDown numericUpDown_pictures_count;
        internal System.Windows.Forms.NumericUpDown numericUpDown_picturesX;
        internal System.Windows.Forms.NumericUpDown numericUpDown_picturesY;
    }
}
