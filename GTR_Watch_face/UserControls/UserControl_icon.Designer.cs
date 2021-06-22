
namespace AmazFit_Watchface_2
{
    partial class UserControl_icon
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_icon));
            this.panel_icon = new System.Windows.Forms.Panel();
            this.checkBox_icon_Use = new System.Windows.Forms.CheckBox();
            this.comboBox_icon_image = new System.Windows.Forms.ComboBox();
            this.comboBox_icon_image2 = new System.Windows.Forms.ComboBox();
            this.numericUpDown_iconX = new System.Windows.Forms.NumericUpDown();
            this.contextMenuStrip_X = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.вставитьКоординатуХToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.копироватьToolStripMenuItemX = new System.Windows.Forms.ToolStripMenuItem();
            this.вставитьToolStripMenuItemX = new System.Windows.Forms.ToolStripMenuItem();
            this.numericUpDown_iconY = new System.Windows.Forms.NumericUpDown();
            this.contextMenuStrip_Y = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.вставитьКоординатуYToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.копироватьToolStripMenuItemY = new System.Windows.Forms.ToolStripMenuItem();
            this.вставитьToolStripMenuItemY = new System.Windows.Forms.ToolStripMenuItem();
            this.label01 = new System.Windows.Forms.Label();
            this.label02 = new System.Windows.Forms.Label();
            this.label04 = new System.Windows.Forms.Label();
            this.label05 = new System.Windows.Forms.Label();
            this.label06 = new System.Windows.Forms.Label();
            this.button_Copy_icon = new System.Windows.Forms.Button();
            this.button_icon = new System.Windows.Forms.Button();
            this.panel_icon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_iconX)).BeginInit();
            this.contextMenuStrip_X.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_iconY)).BeginInit();
            this.contextMenuStrip_Y.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_icon
            // 
            this.panel_icon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_icon.Controls.Add(this.checkBox_icon_Use);
            this.panel_icon.Controls.Add(this.comboBox_icon_image);
            this.panel_icon.Controls.Add(this.comboBox_icon_image2);
            this.panel_icon.Controls.Add(this.numericUpDown_iconX);
            this.panel_icon.Controls.Add(this.numericUpDown_iconY);
            this.panel_icon.Controls.Add(this.label01);
            this.panel_icon.Controls.Add(this.label02);
            this.panel_icon.Controls.Add(this.label04);
            this.panel_icon.Controls.Add(this.label05);
            this.panel_icon.Controls.Add(this.label06);
            this.panel_icon.Controls.Add(this.button_Copy_icon);
            resources.ApplyResources(this.panel_icon, "panel_icon");
            this.panel_icon.Name = "panel_icon";
            // 
            // checkBox_icon_Use
            // 
            resources.ApplyResources(this.checkBox_icon_Use, "checkBox_icon_Use");
            this.checkBox_icon_Use.Name = "checkBox_icon_Use";
            this.checkBox_icon_Use.UseVisualStyleBackColor = true;
            this.checkBox_icon_Use.CheckedChanged += new System.EventHandler(this.checkBox_icon_Use_CheckedChanged);
            this.checkBox_icon_Use.Click += new System.EventHandler(this.checkBox_icon_Use_Click);
            // 
            // comboBox_icon_image
            // 
            this.comboBox_icon_image.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBox_icon_image.DropDownWidth = 75;
            resources.ApplyResources(this.comboBox_icon_image, "comboBox_icon_image");
            this.comboBox_icon_image.FormattingEnabled = true;
            this.comboBox_icon_image.Name = "comboBox_icon_image";
            this.comboBox_icon_image.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBox_DrawItem);
            this.comboBox_icon_image.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.comboBox_MeasureItem);
            this.comboBox_icon_image.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            this.comboBox_icon_image.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox_KeyDown);
            this.comboBox_icon_image.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
            // 
            // comboBox_icon_image2
            // 
            this.comboBox_icon_image2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBox_icon_image2.DropDownWidth = 75;
            resources.ApplyResources(this.comboBox_icon_image2, "comboBox_icon_image2");
            this.comboBox_icon_image2.FormattingEnabled = true;
            this.comboBox_icon_image2.Name = "comboBox_icon_image2";
            this.comboBox_icon_image2.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBox_DrawItem);
            this.comboBox_icon_image2.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.comboBox_MeasureItem);
            this.comboBox_icon_image2.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            this.comboBox_icon_image2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox_KeyDown);
            this.comboBox_icon_image2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
            // 
            // numericUpDown_iconX
            // 
            this.numericUpDown_iconX.ContextMenuStrip = this.contextMenuStrip_X;
            resources.ApplyResources(this.numericUpDown_iconX, "numericUpDown_iconX");
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
            this.numericUpDown_iconX.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            this.numericUpDown_iconX.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.numericUpDown_iconX_MouseDoubleClick);
            // 
            // contextMenuStrip_X
            // 
            this.contextMenuStrip_X.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip_X.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.вставитьКоординатуХToolStripMenuItem,
            this.копироватьToolStripMenuItemX,
            this.вставитьToolStripMenuItemX});
            this.contextMenuStrip_X.Name = "contextMenuStrip_X";
            resources.ApplyResources(this.contextMenuStrip_X, "contextMenuStrip_X");
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
            // numericUpDown_iconY
            // 
            this.numericUpDown_iconY.ContextMenuStrip = this.contextMenuStrip_Y;
            resources.ApplyResources(this.numericUpDown_iconY, "numericUpDown_iconY");
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
            this.numericUpDown_iconY.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            this.numericUpDown_iconY.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.numericUpDown_iconY_MouseDoubleClick);
            // 
            // contextMenuStrip_Y
            // 
            this.contextMenuStrip_Y.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip_Y.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.вставитьКоординатуYToolStripMenuItem,
            this.копироватьToolStripMenuItemY,
            this.вставитьToolStripMenuItemY});
            this.contextMenuStrip_Y.Name = "contextMenuStrip_X";
            resources.ApplyResources(this.contextMenuStrip_Y, "contextMenuStrip_Y");
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
            // label01
            // 
            resources.ApplyResources(this.label01, "label01");
            this.label01.Name = "label01";
            // 
            // label02
            // 
            resources.ApplyResources(this.label02, "label02");
            this.label02.Name = "label02";
            // 
            // label04
            // 
            resources.ApplyResources(this.label04, "label04");
            this.label04.Name = "label04";
            // 
            // label05
            // 
            resources.ApplyResources(this.label05, "label05");
            this.label05.Name = "label05";
            // 
            // label06
            // 
            resources.ApplyResources(this.label06, "label06");
            this.label06.Name = "label06";
            // 
            // button_Copy_icon
            // 
            resources.ApplyResources(this.button_Copy_icon, "button_Copy_icon");
            this.button_Copy_icon.Name = "button_Copy_icon";
            this.button_Copy_icon.UseVisualStyleBackColor = true;
            this.button_Copy_icon.Click += new System.EventHandler(this.button_Copy_icon_Click);
            // 
            // button_icon
            // 
            resources.ApplyResources(this.button_icon, "button_icon");
            this.button_icon.Name = "button_icon";
            this.button_icon.UseVisualStyleBackColor = true;
            this.button_icon.Click += new System.EventHandler(this.button_icon_Click);
            // 
            // UserControl_icon
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_icon);
            this.Controls.Add(this.button_icon);
            this.Name = "UserControl_icon";
            this.panel_icon.ResumeLayout(false);
            this.panel_icon.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_iconX)).EndInit();
            this.contextMenuStrip_X.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_iconY)).EndInit();
            this.contextMenuStrip_Y.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_icon;
        public System.Windows.Forms.ComboBox comboBox_icon_image;
        internal System.Windows.Forms.NumericUpDown numericUpDown_iconX;
        internal System.Windows.Forms.NumericUpDown numericUpDown_iconY;
        private System.Windows.Forms.Label label01;
        private System.Windows.Forms.Label label02;
        private System.Windows.Forms.Label label04;
        private System.Windows.Forms.Label label05;
        private System.Windows.Forms.Button button_Copy_icon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_X;
        private System.Windows.Forms.ToolStripMenuItem вставитьКоординатуХToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem копироватьToolStripMenuItemX;
        private System.Windows.Forms.ToolStripMenuItem вставитьToolStripMenuItemX;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Y;
        private System.Windows.Forms.ToolStripMenuItem вставитьКоординатуYToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem копироватьToolStripMenuItemY;
        private System.Windows.Forms.ToolStripMenuItem вставитьToolStripMenuItemY;
        public System.Windows.Forms.ComboBox comboBox_icon_image2;
        private System.Windows.Forms.Label label06;
        public System.Windows.Forms.Button button_icon;
        internal System.Windows.Forms.CheckBox checkBox_icon_Use;
    }
}
