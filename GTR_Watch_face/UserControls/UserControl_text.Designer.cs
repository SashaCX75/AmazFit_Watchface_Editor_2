﻿
namespace AmazFit_Watchface_2
{
    partial class UserControl_text
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_text));
            this.button_text = new System.Windows.Forms.Button();
            this.panel_text = new System.Windows.Forms.Panel();
            this.checkBox_Use = new System.Windows.Forms.CheckBox();
            this.comboBox_image = new System.Windows.Forms.ComboBox();
            this.comboBox_icon = new System.Windows.Forms.ComboBox();
            this.comboBox_unit = new System.Windows.Forms.ComboBox();
            this.numericUpDown_imageX = new System.Windows.Forms.NumericUpDown();
            this.contextMenuStrip_X = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.вставитьКоординатуХToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.копироватьToolStripMenuItemX = new System.Windows.Forms.ToolStripMenuItem();
            this.вставитьToolStripMenuItemX = new System.Windows.Forms.ToolStripMenuItem();
            this.numericUpDown_imageY = new System.Windows.Forms.NumericUpDown();
            this.contextMenuStrip_Y = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.вставитьКоординатуYToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.копироватьToolStripMenuItemY = new System.Windows.Forms.ToolStripMenuItem();
            this.вставитьToolStripMenuItemY = new System.Windows.Forms.ToolStripMenuItem();
            this.numericUpDown_iconX = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_iconY = new System.Windows.Forms.NumericUpDown();
            this.comboBox_alignment = new System.Windows.Forms.ComboBox();
            this.numericUpDown_spacing = new System.Windows.Forms.NumericUpDown();
            this.checkBox_addZero = new System.Windows.Forms.CheckBox();
            this.comboBox_imageError = new System.Windows.Forms.ComboBox();
            this.comboBox_imageDecimalPoint = new System.Windows.Forms.ComboBox();
            this.label02 = new System.Windows.Forms.Label();
            this.label08 = new System.Windows.Forms.Label();
            this.label04 = new System.Windows.Forms.Label();
            this.label01 = new System.Windows.Forms.Label();
            this.label05 = new System.Windows.Forms.Label();
            this.label09 = new System.Windows.Forms.Label();
            this.label03 = new System.Windows.Forms.Label();
            this.label1083 = new System.Windows.Forms.Label();
            this.label1084 = new System.Windows.Forms.Label();
            this.label1085 = new System.Windows.Forms.Label();
            this.label1086 = new System.Windows.Forms.Label();
            this.label06 = new System.Windows.Forms.Label();
            this.label07 = new System.Windows.Forms.Label();
            this.button_Copy_text = new System.Windows.Forms.Button();
            this.panel_text.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_imageX)).BeginInit();
            this.contextMenuStrip_X.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_imageY)).BeginInit();
            this.contextMenuStrip_Y.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_iconX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_iconY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_spacing)).BeginInit();
            this.SuspendLayout();
            // 
            // button_text
            // 
            this.button_text.Dock = System.Windows.Forms.DockStyle.Top;
            this.button_text.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button_text.Location = new System.Drawing.Point(0, 0);
            this.button_text.Name = "button_text";
            this.button_text.Size = new System.Drawing.Size(435, 23);
            this.button_text.TabIndex = 198;
            this.button_text.Text = "Цифровые значения";
            this.button_text.UseVisualStyleBackColor = true;
            this.button_text.Click += new System.EventHandler(this.button_Click);
            // 
            // panel_text
            // 
            this.panel_text.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_text.Controls.Add(this.checkBox_Use);
            this.panel_text.Controls.Add(this.comboBox_image);
            this.panel_text.Controls.Add(this.comboBox_icon);
            this.panel_text.Controls.Add(this.comboBox_unit);
            this.panel_text.Controls.Add(this.numericUpDown_imageX);
            this.panel_text.Controls.Add(this.numericUpDown_imageY);
            this.panel_text.Controls.Add(this.numericUpDown_iconX);
            this.panel_text.Controls.Add(this.numericUpDown_iconY);
            this.panel_text.Controls.Add(this.comboBox_alignment);
            this.panel_text.Controls.Add(this.numericUpDown_spacing);
            this.panel_text.Controls.Add(this.checkBox_addZero);
            this.panel_text.Controls.Add(this.comboBox_imageError);
            this.panel_text.Controls.Add(this.comboBox_imageDecimalPoint);
            this.panel_text.Controls.Add(this.label02);
            this.panel_text.Controls.Add(this.label08);
            this.panel_text.Controls.Add(this.label04);
            this.panel_text.Controls.Add(this.label01);
            this.panel_text.Controls.Add(this.label05);
            this.panel_text.Controls.Add(this.label09);
            this.panel_text.Controls.Add(this.label03);
            this.panel_text.Controls.Add(this.label1083);
            this.panel_text.Controls.Add(this.label1084);
            this.panel_text.Controls.Add(this.label1085);
            this.panel_text.Controls.Add(this.label1086);
            this.panel_text.Controls.Add(this.label06);
            this.panel_text.Controls.Add(this.label07);
            this.panel_text.Controls.Add(this.button_Copy_text);
            this.panel_text.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_text.Location = new System.Drawing.Point(0, 23);
            this.panel_text.Name = "panel_text";
            this.panel_text.Size = new System.Drawing.Size(435, 215);
            this.panel_text.TabIndex = 199;
            // 
            // checkBox_Use
            // 
            this.checkBox_Use.AutoSize = true;
            this.checkBox_Use.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkBox_Use.Location = new System.Drawing.Point(3, 6);
            this.checkBox_Use.Name = "checkBox_Use";
            this.checkBox_Use.Size = new System.Drawing.Size(99, 17);
            this.checkBox_Use.TabIndex = 142;
            this.checkBox_Use.Text = "Использовать";
            this.checkBox_Use.UseVisualStyleBackColor = true;
            this.checkBox_Use.CheckedChanged += new System.EventHandler(this.checkBox_Use_CheckedChanged);
            this.checkBox_Use.Click += new System.EventHandler(this.checkBox_Click);
            // 
            // comboBox_image
            // 
            this.comboBox_image.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBox_image.DropDownWidth = 75;
            this.comboBox_image.Enabled = false;
            this.comboBox_image.FormattingEnabled = true;
            this.comboBox_image.Location = new System.Drawing.Point(3, 58);
            this.comboBox_image.MaxDropDownItems = 25;
            this.comboBox_image.Name = "comboBox_image";
            this.comboBox_image.Size = new System.Drawing.Size(56, 21);
            this.comboBox_image.TabIndex = 124;
            this.comboBox_image.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBox_DrawItem);
            this.comboBox_image.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.comboBox_MeasureItem);
            this.comboBox_image.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            this.comboBox_image.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox_KeyDown);
            this.comboBox_image.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
            // 
            // comboBox_icon
            // 
            this.comboBox_icon.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBox_icon.DropDownWidth = 75;
            this.comboBox_icon.Enabled = false;
            this.comboBox_icon.FormattingEnabled = true;
            this.comboBox_icon.Location = new System.Drawing.Point(3, 121);
            this.comboBox_icon.MaxDropDownItems = 25;
            this.comboBox_icon.Name = "comboBox_icon";
            this.comboBox_icon.Size = new System.Drawing.Size(56, 21);
            this.comboBox_icon.TabIndex = 135;
            this.comboBox_icon.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBox_DrawItem);
            this.comboBox_icon.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.comboBox_MeasureItem);
            this.comboBox_icon.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            this.comboBox_icon.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox_KeyDown);
            this.comboBox_icon.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
            // 
            // comboBox_unit
            // 
            this.comboBox_unit.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBox_unit.DropDownWidth = 75;
            this.comboBox_unit.Enabled = false;
            this.comboBox_unit.FormattingEnabled = true;
            this.comboBox_unit.Location = new System.Drawing.Point(3, 184);
            this.comboBox_unit.MaxDropDownItems = 25;
            this.comboBox_unit.Name = "comboBox_unit";
            this.comboBox_unit.Size = new System.Drawing.Size(56, 21);
            this.comboBox_unit.TabIndex = 133;
            this.comboBox_unit.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBox_DrawItem);
            this.comboBox_unit.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.comboBox_MeasureItem);
            this.comboBox_unit.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            this.comboBox_unit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox_KeyDown);
            this.comboBox_unit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
            // 
            // numericUpDown_imageX
            // 
            this.numericUpDown_imageX.ContextMenuStrip = this.contextMenuStrip_X;
            this.numericUpDown_imageX.Enabled = false;
            this.numericUpDown_imageX.Location = new System.Drawing.Point(120, 58);
            this.numericUpDown_imageX.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numericUpDown_imageX.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.numericUpDown_imageX.Name = "numericUpDown_imageX";
            this.numericUpDown_imageX.Size = new System.Drawing.Size(40, 20);
            this.numericUpDown_imageX.TabIndex = 128;
            this.numericUpDown_imageX.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            this.numericUpDown_imageX.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.numericUpDown_picturesX_MouseDoubleClick);
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
            // numericUpDown_imageY
            // 
            this.numericUpDown_imageY.ContextMenuStrip = this.contextMenuStrip_Y;
            this.numericUpDown_imageY.Enabled = false;
            this.numericUpDown_imageY.Location = new System.Drawing.Point(193, 58);
            this.numericUpDown_imageY.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numericUpDown_imageY.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.numericUpDown_imageY.Name = "numericUpDown_imageY";
            this.numericUpDown_imageY.Size = new System.Drawing.Size(40, 20);
            this.numericUpDown_imageY.TabIndex = 129;
            this.numericUpDown_imageY.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            this.numericUpDown_imageY.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.numericUpDown_picturesY_MouseDoubleClick);
            // 
            // contextMenuStrip_Y
            // 
            this.contextMenuStrip_Y.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip_Y.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.вставитьКоординатуYToolStripMenuItem,
            this.копироватьToolStripMenuItemY,
            this.вставитьToolStripMenuItemY});
            this.contextMenuStrip_Y.Name = "contextMenuStrip_X";
            this.contextMenuStrip_Y.Size = new System.Drawing.Size(204, 104);
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
            // numericUpDown_iconX
            // 
            this.numericUpDown_iconX.ContextMenuStrip = this.contextMenuStrip_X;
            this.numericUpDown_iconX.Enabled = false;
            this.numericUpDown_iconX.Location = new System.Drawing.Point(120, 121);
            this.numericUpDown_iconX.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numericUpDown_iconX.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.numericUpDown_iconX.Name = "numericUpDown_iconX";
            this.numericUpDown_iconX.Size = new System.Drawing.Size(40, 20);
            this.numericUpDown_iconX.TabIndex = 139;
            this.numericUpDown_iconX.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            this.numericUpDown_iconX.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.numericUpDown_picturesX_MouseDoubleClick);
            // 
            // numericUpDown_iconY
            // 
            this.numericUpDown_iconY.ContextMenuStrip = this.contextMenuStrip_Y;
            this.numericUpDown_iconY.Enabled = false;
            this.numericUpDown_iconY.Location = new System.Drawing.Point(193, 121);
            this.numericUpDown_iconY.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numericUpDown_iconY.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.numericUpDown_iconY.Name = "numericUpDown_iconY";
            this.numericUpDown_iconY.Size = new System.Drawing.Size(40, 20);
            this.numericUpDown_iconY.TabIndex = 140;
            this.numericUpDown_iconY.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            this.numericUpDown_iconY.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.numericUpDown_picturesY_MouseDoubleClick);
            // 
            // comboBox_alignment
            // 
            this.comboBox_alignment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_alignment.Enabled = false;
            this.comboBox_alignment.FormattingEnabled = true;
            this.comboBox_alignment.Items.AddRange(new object[] {
            "По левому краю",
            "По праваму краю",
            "По центру"});
            this.comboBox_alignment.Location = new System.Drawing.Point(106, 184);
            this.comboBox_alignment.Name = "comboBox_alignment";
            this.comboBox_alignment.Size = new System.Drawing.Size(127, 21);
            this.comboBox_alignment.TabIndex = 131;
            this.comboBox_alignment.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // numericUpDown_spacing
            // 
            this.numericUpDown_spacing.Enabled = false;
            this.numericUpDown_spacing.Location = new System.Drawing.Point(275, 58);
            this.numericUpDown_spacing.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numericUpDown_spacing.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.numericUpDown_spacing.Name = "numericUpDown_spacing";
            this.numericUpDown_spacing.Size = new System.Drawing.Size(40, 20);
            this.numericUpDown_spacing.TabIndex = 145;
            this.numericUpDown_spacing.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            // 
            // checkBox_addZero
            // 
            this.checkBox_addZero.Enabled = false;
            this.checkBox_addZero.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkBox_addZero.Location = new System.Drawing.Point(275, 179);
            this.checkBox_addZero.Name = "checkBox_addZero";
            this.checkBox_addZero.Size = new System.Drawing.Size(125, 30);
            this.checkBox_addZero.TabIndex = 143;
            this.checkBox_addZero.Text = "Добавлять нули в начале";
            this.checkBox_addZero.UseVisualStyleBackColor = true;
            this.checkBox_addZero.Click += new System.EventHandler(this.checkBox_Click);
            // 
            // comboBox_imageError
            // 
            this.comboBox_imageError.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBox_imageError.DropDownWidth = 75;
            this.comboBox_imageError.Enabled = false;
            this.comboBox_imageError.FormattingEnabled = true;
            this.comboBox_imageError.Location = new System.Drawing.Point(275, 121);
            this.comboBox_imageError.MaxDropDownItems = 25;
            this.comboBox_imageError.Name = "comboBox_imageError";
            this.comboBox_imageError.Size = new System.Drawing.Size(56, 21);
            this.comboBox_imageError.TabIndex = 147;
            this.comboBox_imageError.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBox_DrawItem);
            this.comboBox_imageError.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.comboBox_MeasureItem);
            this.comboBox_imageError.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            this.comboBox_imageError.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox_KeyDown);
            this.comboBox_imageError.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
            // 
            // comboBox_imageDecimalPoint
            // 
            this.comboBox_imageDecimalPoint.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBox_imageDecimalPoint.DropDownWidth = 75;
            this.comboBox_imageDecimalPoint.Enabled = false;
            this.comboBox_imageDecimalPoint.FormattingEnabled = true;
            this.comboBox_imageDecimalPoint.Location = new System.Drawing.Point(355, 121);
            this.comboBox_imageDecimalPoint.MaxDropDownItems = 25;
            this.comboBox_imageDecimalPoint.Name = "comboBox_imageDecimalPoint";
            this.comboBox_imageDecimalPoint.Size = new System.Drawing.Size(56, 21);
            this.comboBox_imageDecimalPoint.TabIndex = 149;
            this.comboBox_imageDecimalPoint.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBox_DrawItem);
            this.comboBox_imageDecimalPoint.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.comboBox_MeasureItem);
            this.comboBox_imageDecimalPoint.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            this.comboBox_imageDecimalPoint.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox_KeyDown);
            this.comboBox_imageDecimalPoint.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
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
            this.label02.TabIndex = 144;
            this.label02.Text = "Координаты";
            // 
            // label08
            // 
            this.label08.Enabled = false;
            this.label08.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label08.Location = new System.Drawing.Point(3, 151);
            this.label08.Margin = new System.Windows.Forms.Padding(3);
            this.label08.Name = "label08";
            this.label08.Size = new System.Drawing.Size(80, 30);
            this.label08.TabIndex = 132;
            this.label08.Text = "Единицы измерения";
            this.label08.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label04
            // 
            this.label04.AutoSize = true;
            this.label04.Enabled = false;
            this.label04.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label04.Location = new System.Drawing.Point(3, 104);
            this.label04.Margin = new System.Windows.Forms.Padding(3);
            this.label04.Name = "label04";
            this.label04.Size = new System.Drawing.Size(45, 13);
            this.label04.TabIndex = 134;
            this.label04.Text = "Иконка";
            // 
            // label01
            // 
            this.label01.Enabled = false;
            this.label01.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label01.Location = new System.Drawing.Point(3, 24);
            this.label01.Name = "label01";
            this.label01.Size = new System.Drawing.Size(80, 30);
            this.label01.TabIndex = 123;
            this.label01.Text = "Стартовое изображение";
            this.label01.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label05
            // 
            this.label05.AutoSize = true;
            this.label05.Enabled = false;
            this.label05.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label05.Location = new System.Drawing.Point(103, 104);
            this.label05.Margin = new System.Windows.Forms.Padding(3);
            this.label05.Name = "label05";
            this.label05.Size = new System.Drawing.Size(108, 13);
            this.label05.TabIndex = 141;
            this.label05.Text = "Координаты иконки";
            // 
            // label09
            // 
            this.label09.AutoSize = true;
            this.label09.Enabled = false;
            this.label09.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label09.Location = new System.Drawing.Point(103, 167);
            this.label09.Name = "label09";
            this.label09.Size = new System.Drawing.Size(82, 13);
            this.label09.TabIndex = 130;
            this.label09.Text = "Выравнивание";
            // 
            // label03
            // 
            this.label03.AutoSize = true;
            this.label03.Enabled = false;
            this.label03.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label03.Location = new System.Drawing.Point(272, 41);
            this.label03.Margin = new System.Windows.Forms.Padding(3);
            this.label03.Name = "label03";
            this.label03.Size = new System.Drawing.Size(50, 13);
            this.label03.TabIndex = 136;
            this.label03.Text = "Отступы";
            // 
            // label1083
            // 
            this.label1083.AutoSize = true;
            this.label1083.Enabled = false;
            this.label1083.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1083.Location = new System.Drawing.Point(176, 123);
            this.label1083.Name = "label1083";
            this.label1083.Size = new System.Drawing.Size(17, 13);
            this.label1083.TabIndex = 138;
            this.label1083.Text = "Y:";
            // 
            // label1084
            // 
            this.label1084.AutoSize = true;
            this.label1084.Enabled = false;
            this.label1084.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1084.Location = new System.Drawing.Point(176, 60);
            this.label1084.Name = "label1084";
            this.label1084.Size = new System.Drawing.Size(17, 13);
            this.label1084.TabIndex = 127;
            this.label1084.Text = "Y:";
            // 
            // label1085
            // 
            this.label1085.AutoSize = true;
            this.label1085.Enabled = false;
            this.label1085.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1085.Location = new System.Drawing.Point(103, 60);
            this.label1085.Name = "label1085";
            this.label1085.Size = new System.Drawing.Size(17, 13);
            this.label1085.TabIndex = 126;
            this.label1085.Text = "X:";
            // 
            // label1086
            // 
            this.label1086.AutoSize = true;
            this.label1086.Enabled = false;
            this.label1086.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1086.Location = new System.Drawing.Point(103, 123);
            this.label1086.Name = "label1086";
            this.label1086.Size = new System.Drawing.Size(17, 13);
            this.label1086.TabIndex = 137;
            this.label1086.Text = "X:";
            // 
            // label06
            // 
            this.label06.Enabled = false;
            this.label06.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label06.Location = new System.Drawing.Point(269, 88);
            this.label06.Margin = new System.Windows.Forms.Padding(3);
            this.label06.Name = "label06";
            this.label06.Size = new System.Drawing.Size(80, 30);
            this.label06.TabIndex = 146;
            this.label06.Text = "Изображение при ошибке";
            this.label06.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label07
            // 
            this.label07.Enabled = false;
            this.label07.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label07.Location = new System.Drawing.Point(355, 88);
            this.label07.Margin = new System.Windows.Forms.Padding(3);
            this.label07.Name = "label07";
            this.label07.Size = new System.Drawing.Size(80, 30);
            this.label07.TabIndex = 148;
            this.label07.Text = "Десятичный разделитель";
            this.label07.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // button_Copy_text
            // 
            this.button_Copy_text.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button_Copy_text.Location = new System.Drawing.Point(120, 3);
            this.button_Copy_text.Name = "button_Copy_text";
            this.button_Copy_text.Size = new System.Drawing.Size(195, 23);
            this.button_Copy_text.TabIndex = 150;
            this.button_Copy_text.Text = "Скопировать с основного экрана";
            this.button_Copy_text.UseVisualStyleBackColor = true;
            this.button_Copy_text.Click += new System.EventHandler(this.button_Copy_text_Click);
            // 
            // UserControl_text
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_text);
            this.Controls.Add(this.button_text);
            this.Name = "UserControl_text";
            this.Size = new System.Drawing.Size(435, 280);
            this.panel_text.ResumeLayout(false);
            this.panel_text.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_imageX)).EndInit();
            this.contextMenuStrip_X.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_imageY)).EndInit();
            this.contextMenuStrip_Y.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_iconX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_iconY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_spacing)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_text;
        private System.Windows.Forms.Panel panel_text;
        private System.Windows.Forms.ComboBox comboBox_image;
        private System.Windows.Forms.ComboBox comboBox_icon;
        private System.Windows.Forms.ComboBox comboBox_unit;
        private System.Windows.Forms.ComboBox comboBox_alignment;
        private System.Windows.Forms.ComboBox comboBox_imageError;
        private System.Windows.Forms.ComboBox comboBox_imageDecimalPoint;
        private System.Windows.Forms.Label label02;
        private System.Windows.Forms.Label label08;
        private System.Windows.Forms.Label label04;
        private System.Windows.Forms.Label label01;
        private System.Windows.Forms.Label label05;
        private System.Windows.Forms.Label label09;
        private System.Windows.Forms.Label label03;
        private System.Windows.Forms.Label label1083;
        private System.Windows.Forms.Label label1084;
        private System.Windows.Forms.Label label1085;
        private System.Windows.Forms.Label label1086;
        private System.Windows.Forms.Label label06;
        private System.Windows.Forms.Label label07;
        private System.Windows.Forms.Button button_Copy_text;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_X;
        private System.Windows.Forms.ToolStripMenuItem вставитьКоординатуХToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem копироватьToolStripMenuItemX;
        private System.Windows.Forms.ToolStripMenuItem вставитьToolStripMenuItemX;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Y;
        private System.Windows.Forms.ToolStripMenuItem вставитьКоординатуYToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem копироватьToolStripMenuItemY;
        private System.Windows.Forms.ToolStripMenuItem вставитьToolStripMenuItemY;
        internal System.Windows.Forms.NumericUpDown numericUpDown_imageX;
        internal System.Windows.Forms.NumericUpDown numericUpDown_imageY;
        internal System.Windows.Forms.NumericUpDown numericUpDown_iconX;
        internal System.Windows.Forms.NumericUpDown numericUpDown_iconY;
        internal System.Windows.Forms.NumericUpDown numericUpDown_spacing;
        internal System.Windows.Forms.CheckBox checkBox_addZero;
        internal System.Windows.Forms.CheckBox checkBox_Use;
    }
}
