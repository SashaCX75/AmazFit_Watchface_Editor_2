﻿
namespace AmazFit_Watchface_2
{
    partial class UserControl_scaleCircle
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_scaleCircle));
            this.contextMenuStrip_X = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.вставитьКоординатуХToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.копироватьToolStripMenuItemX = new System.Windows.Forms.ToolStripMenuItem();
            this.вставитьToolStripMenuItemX = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip_Y = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.вставитьКоординатуYToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.копироватьToolStripMenuItemY = new System.Windows.Forms.ToolStripMenuItem();
            this.вставитьToolStripMenuItemY = new System.Windows.Forms.ToolStripMenuItem();
            this.panel_scaleCircle = new System.Windows.Forms.Panel();
            this.checkBox_scaleCircle_Use = new System.Windows.Forms.CheckBox();
            this.radioButton_scaleCircle_image = new System.Windows.Forms.RadioButton();
            this.radioButton_scaleCircle_color = new System.Windows.Forms.RadioButton();
            this.comboBox_scaleCircle_image = new System.Windows.Forms.ComboBox();
            this.comboBox_scaleCircle_color = new System.Windows.Forms.ComboBox();
            this.comboBox_scaleCircle_flatness = new System.Windows.Forms.ComboBox();
            this.comboBox_scaleCircle_image_background = new System.Windows.Forms.ComboBox();
            this.numericUpDown_scaleCircleX = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_scaleCircleY = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_scaleCircle_radius = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_scaleCircle_width = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_scaleCircle_startAngle = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_scaleCircle_endAngle = new System.Windows.Forms.NumericUpDown();
            this.label01 = new System.Windows.Forms.Label();
            this.label04 = new System.Windows.Forms.Label();
            this.label03 = new System.Windows.Forms.Label();
            this.label06 = new System.Windows.Forms.Label();
            this.label05 = new System.Windows.Forms.Label();
            this.label08 = new System.Windows.Forms.Label();
            this.label07 = new System.Windows.Forms.Label();
            this.label02 = new System.Windows.Forms.Label();
            this.label09 = new System.Windows.Forms.Label();
            this.button_Copy_scaleCircle = new System.Windows.Forms.Button();
            this.button_scaleCircle = new System.Windows.Forms.Button();
            this.contextMenuStrip_X.SuspendLayout();
            this.contextMenuStrip_Y.SuspendLayout();
            this.panel_scaleCircle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_scaleCircleX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_scaleCircleY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_scaleCircle_radius)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_scaleCircle_width)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_scaleCircle_startAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_scaleCircle_endAngle)).BeginInit();
            this.SuspendLayout();
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
            this.вставитьToolStripMenuItemY.DoubleClick += new System.EventHandler(this.вставитьToolStripMenuItem_Click);
            // 
            // panel_scaleCircle
            // 
            this.panel_scaleCircle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_scaleCircle.Controls.Add(this.checkBox_scaleCircle_Use);
            this.panel_scaleCircle.Controls.Add(this.radioButton_scaleCircle_image);
            this.panel_scaleCircle.Controls.Add(this.radioButton_scaleCircle_color);
            this.panel_scaleCircle.Controls.Add(this.comboBox_scaleCircle_image);
            this.panel_scaleCircle.Controls.Add(this.comboBox_scaleCircle_color);
            this.panel_scaleCircle.Controls.Add(this.comboBox_scaleCircle_flatness);
            this.panel_scaleCircle.Controls.Add(this.comboBox_scaleCircle_image_background);
            this.panel_scaleCircle.Controls.Add(this.numericUpDown_scaleCircleX);
            this.panel_scaleCircle.Controls.Add(this.numericUpDown_scaleCircleY);
            this.panel_scaleCircle.Controls.Add(this.numericUpDown_scaleCircle_radius);
            this.panel_scaleCircle.Controls.Add(this.numericUpDown_scaleCircle_width);
            this.panel_scaleCircle.Controls.Add(this.numericUpDown_scaleCircle_startAngle);
            this.panel_scaleCircle.Controls.Add(this.numericUpDown_scaleCircle_endAngle);
            this.panel_scaleCircle.Controls.Add(this.label01);
            this.panel_scaleCircle.Controls.Add(this.label04);
            this.panel_scaleCircle.Controls.Add(this.label03);
            this.panel_scaleCircle.Controls.Add(this.label06);
            this.panel_scaleCircle.Controls.Add(this.label05);
            this.panel_scaleCircle.Controls.Add(this.label08);
            this.panel_scaleCircle.Controls.Add(this.label07);
            this.panel_scaleCircle.Controls.Add(this.label02);
            this.panel_scaleCircle.Controls.Add(this.label09);
            this.panel_scaleCircle.Controls.Add(this.button_Copy_scaleCircle);
            this.panel_scaleCircle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_scaleCircle.Location = new System.Drawing.Point(0, 23);
            this.panel_scaleCircle.Name = "panel_scaleCircle";
            this.panel_scaleCircle.Size = new System.Drawing.Size(435, 150);
            this.panel_scaleCircle.TabIndex = 160;
            // 
            // checkBox_scaleCircle_Use
            // 
            this.checkBox_scaleCircle_Use.AutoSize = true;
            this.checkBox_scaleCircle_Use.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkBox_scaleCircle_Use.Location = new System.Drawing.Point(3, 6);
            this.checkBox_scaleCircle_Use.Name = "checkBox_scaleCircle_Use";
            this.checkBox_scaleCircle_Use.Size = new System.Drawing.Size(99, 17);
            this.checkBox_scaleCircle_Use.TabIndex = 96;
            this.checkBox_scaleCircle_Use.Text = "Использовать";
            this.checkBox_scaleCircle_Use.UseVisualStyleBackColor = true;
            this.checkBox_scaleCircle_Use.CheckedChanged += new System.EventHandler(this.checkBox_Use_CheckedChanged);
            this.checkBox_scaleCircle_Use.Click += new System.EventHandler(this.checkBox_Click);
            // 
            // radioButton_scaleCircle_image
            // 
            this.radioButton_scaleCircle_image.AutoSize = true;
            this.radioButton_scaleCircle_image.Checked = true;
            this.radioButton_scaleCircle_image.Enabled = false;
            this.radioButton_scaleCircle_image.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radioButton_scaleCircle_image.Location = new System.Drawing.Point(177, 38);
            this.radioButton_scaleCircle_image.Name = "radioButton_scaleCircle_image";
            this.radioButton_scaleCircle_image.Size = new System.Drawing.Size(95, 17);
            this.radioButton_scaleCircle_image.TabIndex = 130;
            this.radioButton_scaleCircle_image.TabStop = true;
            this.radioButton_scaleCircle_image.Text = "Изображение";
            this.radioButton_scaleCircle_image.UseVisualStyleBackColor = true;
            this.radioButton_scaleCircle_image.CheckedChanged += new System.EventHandler(this.radioButton_image_color_CheckedChanged);
            // 
            // radioButton_scaleCircle_color
            // 
            this.radioButton_scaleCircle_color.AutoSize = true;
            this.radioButton_scaleCircle_color.Enabled = false;
            this.radioButton_scaleCircle_color.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radioButton_scaleCircle_color.Location = new System.Drawing.Point(286, 38);
            this.radioButton_scaleCircle_color.Name = "radioButton_scaleCircle_color";
            this.radioButton_scaleCircle_color.Size = new System.Drawing.Size(50, 17);
            this.radioButton_scaleCircle_color.TabIndex = 131;
            this.radioButton_scaleCircle_color.TabStop = true;
            this.radioButton_scaleCircle_color.Text = "Цвет";
            this.radioButton_scaleCircle_color.UseVisualStyleBackColor = true;
            // 
            // comboBox_scaleCircle_image
            // 
            this.comboBox_scaleCircle_image.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBox_scaleCircle_image.DropDownWidth = 75;
            this.comboBox_scaleCircle_image.Enabled = false;
            this.comboBox_scaleCircle_image.FormattingEnabled = true;
            this.comboBox_scaleCircle_image.Location = new System.Drawing.Point(177, 58);
            this.comboBox_scaleCircle_image.MaxDropDownItems = 25;
            this.comboBox_scaleCircle_image.Name = "comboBox_scaleCircle_image";
            this.comboBox_scaleCircle_image.Size = new System.Drawing.Size(56, 21);
            this.comboBox_scaleCircle_image.TabIndex = 128;
            this.comboBox_scaleCircle_image.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBox_DrawItem);
            this.comboBox_scaleCircle_image.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.comboBox_MeasureItem);
            this.comboBox_scaleCircle_image.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            this.comboBox_scaleCircle_image.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox_KeyDown);
            this.comboBox_scaleCircle_image.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
            // 
            // comboBox_scaleCircle_color
            // 
            this.comboBox_scaleCircle_color.BackColor = System.Drawing.Color.DarkOrange;
            this.comboBox_scaleCircle_color.DropDownHeight = 1;
            this.comboBox_scaleCircle_color.Enabled = false;
            this.comboBox_scaleCircle_color.FormattingEnabled = true;
            this.comboBox_scaleCircle_color.IntegralHeight = false;
            this.comboBox_scaleCircle_color.Location = new System.Drawing.Point(286, 58);
            this.comboBox_scaleCircle_color.Name = "comboBox_scaleCircle_color";
            this.comboBox_scaleCircle_color.Size = new System.Drawing.Size(45, 21);
            this.comboBox_scaleCircle_color.TabIndex = 129;
            this.comboBox_scaleCircle_color.Click += new System.EventHandler(this.comboBox_color_Click);
            this.comboBox_scaleCircle_color.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
            // 
            // comboBox_scaleCircle_flatness
            // 
            this.comboBox_scaleCircle_flatness.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_scaleCircle_flatness.Enabled = false;
            this.comboBox_scaleCircle_flatness.FormattingEnabled = true;
            this.comboBox_scaleCircle_flatness.Items.AddRange(new object[] {
            "Круглое",
            "Треугольное",
            "Плоское"});
            this.comboBox_scaleCircle_flatness.Location = new System.Drawing.Point(346, 58);
            this.comboBox_scaleCircle_flatness.Name = "comboBox_scaleCircle_flatness";
            this.comboBox_scaleCircle_flatness.Size = new System.Drawing.Size(85, 21);
            this.comboBox_scaleCircle_flatness.TabIndex = 141;
            this.comboBox_scaleCircle_flatness.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // comboBox_scaleCircle_image_background
            // 
            this.comboBox_scaleCircle_image_background.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBox_scaleCircle_image_background.DropDownWidth = 75;
            this.comboBox_scaleCircle_image_background.Enabled = false;
            this.comboBox_scaleCircle_image_background.FormattingEnabled = true;
            this.comboBox_scaleCircle_image_background.Location = new System.Drawing.Point(346, 123);
            this.comboBox_scaleCircle_image_background.MaxDropDownItems = 25;
            this.comboBox_scaleCircle_image_background.Name = "comboBox_scaleCircle_image_background";
            this.comboBox_scaleCircle_image_background.Size = new System.Drawing.Size(56, 21);
            this.comboBox_scaleCircle_image_background.TabIndex = 145;
            this.comboBox_scaleCircle_image_background.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBox_DrawItem);
            this.comboBox_scaleCircle_image_background.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.comboBox_MeasureItem);
            this.comboBox_scaleCircle_image_background.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            this.comboBox_scaleCircle_image_background.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox_KeyDown);
            this.comboBox_scaleCircle_image_background.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
            // 
            // numericUpDown_scaleCircleX
            // 
            this.numericUpDown_scaleCircleX.ContextMenuStrip = this.contextMenuStrip_X;
            this.numericUpDown_scaleCircleX.Enabled = false;
            this.numericUpDown_scaleCircleX.Location = new System.Drawing.Point(20, 58);
            this.numericUpDown_scaleCircleX.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numericUpDown_scaleCircleX.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.numericUpDown_scaleCircleX.Name = "numericUpDown_scaleCircleX";
            this.numericUpDown_scaleCircleX.Size = new System.Drawing.Size(40, 20);
            this.numericUpDown_scaleCircleX.TabIndex = 125;
            this.numericUpDown_scaleCircleX.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            this.numericUpDown_scaleCircleX.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.numericUpDown_picturesX_MouseDoubleClick);
            // 
            // numericUpDown_scaleCircleY
            // 
            this.numericUpDown_scaleCircleY.ContextMenuStrip = this.contextMenuStrip_Y;
            this.numericUpDown_scaleCircleY.Enabled = false;
            this.numericUpDown_scaleCircleY.Location = new System.Drawing.Point(93, 58);
            this.numericUpDown_scaleCircleY.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numericUpDown_scaleCircleY.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.numericUpDown_scaleCircleY.Name = "numericUpDown_scaleCircleY";
            this.numericUpDown_scaleCircleY.Size = new System.Drawing.Size(40, 20);
            this.numericUpDown_scaleCircleY.TabIndex = 126;
            this.numericUpDown_scaleCircleY.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            this.numericUpDown_scaleCircleY.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.numericUpDown_picturesY_MouseDoubleClick);
            // 
            // numericUpDown_scaleCircle_radius
            // 
            this.numericUpDown_scaleCircle_radius.Enabled = false;
            this.numericUpDown_scaleCircle_radius.Location = new System.Drawing.Point(20, 123);
            this.numericUpDown_scaleCircle_radius.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numericUpDown_scaleCircle_radius.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.numericUpDown_scaleCircle_radius.Name = "numericUpDown_scaleCircle_radius";
            this.numericUpDown_scaleCircle_radius.Size = new System.Drawing.Size(40, 20);
            this.numericUpDown_scaleCircle_radius.TabIndex = 132;
            this.numericUpDown_scaleCircle_radius.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDown_scaleCircle_radius.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            // 
            // numericUpDown_scaleCircle_width
            // 
            this.numericUpDown_scaleCircle_width.Enabled = false;
            this.numericUpDown_scaleCircle_width.Location = new System.Drawing.Point(93, 123);
            this.numericUpDown_scaleCircle_width.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numericUpDown_scaleCircle_width.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.numericUpDown_scaleCircle_width.Name = "numericUpDown_scaleCircle_width";
            this.numericUpDown_scaleCircle_width.Size = new System.Drawing.Size(40, 20);
            this.numericUpDown_scaleCircle_width.TabIndex = 134;
            this.numericUpDown_scaleCircle_width.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown_scaleCircle_width.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            // 
            // numericUpDown_scaleCircle_startAngle
            // 
            this.numericUpDown_scaleCircle_startAngle.Enabled = false;
            this.numericUpDown_scaleCircle_startAngle.Location = new System.Drawing.Point(177, 123);
            this.numericUpDown_scaleCircle_startAngle.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numericUpDown_scaleCircle_startAngle.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.numericUpDown_scaleCircle_startAngle.Name = "numericUpDown_scaleCircle_startAngle";
            this.numericUpDown_scaleCircle_startAngle.Size = new System.Drawing.Size(40, 20);
            this.numericUpDown_scaleCircle_startAngle.TabIndex = 136;
            this.numericUpDown_scaleCircle_startAngle.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            // 
            // numericUpDown_scaleCircle_endAngle
            // 
            this.numericUpDown_scaleCircle_endAngle.Enabled = false;
            this.numericUpDown_scaleCircle_endAngle.Location = new System.Drawing.Point(250, 123);
            this.numericUpDown_scaleCircle_endAngle.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numericUpDown_scaleCircle_endAngle.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.numericUpDown_scaleCircle_endAngle.Name = "numericUpDown_scaleCircle_endAngle";
            this.numericUpDown_scaleCircle_endAngle.Size = new System.Drawing.Size(40, 20);
            this.numericUpDown_scaleCircle_endAngle.TabIndex = 138;
            this.numericUpDown_scaleCircle_endAngle.Value = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numericUpDown_scaleCircle_endAngle.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            // 
            // label01
            // 
            this.label01.AutoSize = true;
            this.label01.Enabled = false;
            this.label01.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label01.Location = new System.Drawing.Point(3, 41);
            this.label01.Margin = new System.Windows.Forms.Padding(3);
            this.label01.Name = "label01";
            this.label01.Size = new System.Drawing.Size(139, 13);
            this.label01.TabIndex = 127;
            this.label01.Text = "Координаты ценра шкалы";
            // 
            // label04
            // 
            this.label04.AutoSize = true;
            this.label04.Enabled = false;
            this.label04.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label04.Location = new System.Drawing.Point(76, 60);
            this.label04.Name = "label04";
            this.label04.Size = new System.Drawing.Size(17, 13);
            this.label04.TabIndex = 124;
            this.label04.Text = "Y:";
            // 
            // label03
            // 
            this.label03.AutoSize = true;
            this.label03.Enabled = false;
            this.label03.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label03.Location = new System.Drawing.Point(3, 60);
            this.label03.Name = "label03";
            this.label03.Size = new System.Drawing.Size(17, 13);
            this.label03.TabIndex = 123;
            this.label03.Text = "X:";
            // 
            // label06
            // 
            this.label06.Enabled = false;
            this.label06.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label06.Location = new System.Drawing.Point(76, 88);
            this.label06.Margin = new System.Windows.Forms.Padding(3);
            this.label06.Name = "label06";
            this.label06.Size = new System.Drawing.Size(78, 30);
            this.label06.TabIndex = 135;
            this.label06.Text = "Толщина шкалы";
            this.label06.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label05
            // 
            this.label05.Enabled = false;
            this.label05.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label05.Location = new System.Drawing.Point(3, 88);
            this.label05.Margin = new System.Windows.Forms.Padding(3);
            this.label05.Name = "label05";
            this.label05.Size = new System.Drawing.Size(78, 30);
            this.label05.TabIndex = 133;
            this.label05.Text = "Радиус";
            this.label05.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label08
            // 
            this.label08.Enabled = false;
            this.label08.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label08.Location = new System.Drawing.Point(233, 88);
            this.label08.Margin = new System.Windows.Forms.Padding(3);
            this.label08.Name = "label08";
            this.label08.Size = new System.Drawing.Size(78, 30);
            this.label08.TabIndex = 139;
            this.label08.Text = "Конечный угол";
            this.label08.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label07
            // 
            this.label07.Enabled = false;
            this.label07.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label07.Location = new System.Drawing.Point(160, 88);
            this.label07.Margin = new System.Windows.Forms.Padding(3);
            this.label07.Name = "label07";
            this.label07.Size = new System.Drawing.Size(78, 30);
            this.label07.TabIndex = 137;
            this.label07.Text = "Начальный угол";
            this.label07.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label02
            // 
            this.label02.Enabled = false;
            this.label02.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label02.Location = new System.Drawing.Point(356, 26);
            this.label02.Name = "label02";
            this.label02.Size = new System.Drawing.Size(65, 30);
            this.label02.TabIndex = 140;
            this.label02.Text = "Окончание линии";
            this.label02.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label09
            // 
            this.label09.Enabled = false;
            this.label09.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label09.Location = new System.Drawing.Point(334, 88);
            this.label09.Margin = new System.Windows.Forms.Padding(3);
            this.label09.Name = "label09";
            this.label09.Size = new System.Drawing.Size(80, 30);
            this.label09.TabIndex = 144;
            this.label09.Text = "Фоновое изображение";
            this.label09.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // button_Copy_scaleCircle
            // 
            this.button_Copy_scaleCircle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button_Copy_scaleCircle.Location = new System.Drawing.Point(120, 3);
            this.button_Copy_scaleCircle.Name = "button_Copy_scaleCircle";
            this.button_Copy_scaleCircle.Size = new System.Drawing.Size(195, 23);
            this.button_Copy_scaleCircle.TabIndex = 148;
            this.button_Copy_scaleCircle.Text = "Скопировать с основного экрана";
            this.button_Copy_scaleCircle.UseVisualStyleBackColor = true;
            this.button_Copy_scaleCircle.Click += new System.EventHandler(this.button_Copy_scaleCircle_Click);
            // 
            // button_scaleCircle
            // 
            this.button_scaleCircle.Dock = System.Windows.Forms.DockStyle.Top;
            this.button_scaleCircle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button_scaleCircle.Location = new System.Drawing.Point(0, 0);
            this.button_scaleCircle.Name = "button_scaleCircle";
            this.button_scaleCircle.Size = new System.Drawing.Size(435, 23);
            this.button_scaleCircle.TabIndex = 159;
            this.button_scaleCircle.Text = "Круговая шкала";
            this.button_scaleCircle.UseVisualStyleBackColor = true;
            this.button_scaleCircle.Click += new System.EventHandler(this.button_Click);
            // 
            // UserControl_scaleCircle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_scaleCircle);
            this.Controls.Add(this.button_scaleCircle);
            this.Name = "UserControl_scaleCircle";
            this.Size = new System.Drawing.Size(435, 270);
            this.contextMenuStrip_X.ResumeLayout(false);
            this.contextMenuStrip_Y.ResumeLayout(false);
            this.panel_scaleCircle.ResumeLayout(false);
            this.panel_scaleCircle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_scaleCircleX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_scaleCircleY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_scaleCircle_radius)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_scaleCircle_width)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_scaleCircle_startAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_scaleCircle_endAngle)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_X;
        private System.Windows.Forms.ToolStripMenuItem вставитьКоординатуХToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem копироватьToolStripMenuItemX;
        private System.Windows.Forms.ToolStripMenuItem вставитьToolStripMenuItemX;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Y;
        private System.Windows.Forms.ToolStripMenuItem вставитьКоординатуYToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem копироватьToolStripMenuItemY;
        private System.Windows.Forms.ToolStripMenuItem вставитьToolStripMenuItemY;
        private System.Windows.Forms.Panel panel_scaleCircle;
        private System.Windows.Forms.RadioButton radioButton_scaleCircle_image;
        private System.Windows.Forms.RadioButton radioButton_scaleCircle_color;
        private System.Windows.Forms.ComboBox comboBox_scaleCircle_image;
        private System.Windows.Forms.ComboBox comboBox_scaleCircle_color;
        private System.Windows.Forms.ComboBox comboBox_scaleCircle_flatness;
        private System.Windows.Forms.ComboBox comboBox_scaleCircle_image_background;
        private System.Windows.Forms.Label label01;
        private System.Windows.Forms.Label label04;
        private System.Windows.Forms.Label label03;
        private System.Windows.Forms.Label label06;
        private System.Windows.Forms.Label label05;
        private System.Windows.Forms.Label label08;
        private System.Windows.Forms.Label label07;
        private System.Windows.Forms.Label label02;
        private System.Windows.Forms.Label label09;
        private System.Windows.Forms.Button button_Copy_scaleCircle;
        private System.Windows.Forms.Button button_scaleCircle;
        internal System.Windows.Forms.CheckBox checkBox_scaleCircle_Use;
        internal System.Windows.Forms.NumericUpDown numericUpDown_scaleCircleX;
        internal System.Windows.Forms.NumericUpDown numericUpDown_scaleCircleY;
        internal System.Windows.Forms.NumericUpDown numericUpDown_scaleCircle_radius;
        internal System.Windows.Forms.NumericUpDown numericUpDown_scaleCircle_width;
        internal System.Windows.Forms.NumericUpDown numericUpDown_scaleCircle_startAngle;
        internal System.Windows.Forms.NumericUpDown numericUpDown_scaleCircle_endAngle;
    }
}
