
namespace AmazFit_Watchface_2
{
    partial class UserControl_preview
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
            this.panel_preview = new System.Windows.Forms.Panel();
            this.comboBox_icon_image = new System.Windows.Forms.ComboBox();
            this.label01 = new System.Windows.Forms.Label();
            this.button_preview = new System.Windows.Forms.Button();
            this.panel_preview.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_preview
            // 
            this.panel_preview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_preview.Controls.Add(this.comboBox_icon_image);
            this.panel_preview.Controls.Add(this.label01);
            this.panel_preview.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_preview.Location = new System.Drawing.Point(0, 23);
            this.panel_preview.Name = "panel_preview";
            this.panel_preview.Size = new System.Drawing.Size(435, 55);
            this.panel_preview.TabIndex = 160;
            // 
            // comboBox_icon_image
            // 
            this.comboBox_icon_image.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBox_icon_image.DropDownWidth = 75;
            this.comboBox_icon_image.FormattingEnabled = true;
            this.comboBox_icon_image.Location = new System.Drawing.Point(3, 23);
            this.comboBox_icon_image.MaxDropDownItems = 25;
            this.comboBox_icon_image.Name = "comboBox_icon_image";
            this.comboBox_icon_image.Size = new System.Drawing.Size(56, 21);
            this.comboBox_icon_image.TabIndex = 100;
            this.comboBox_icon_image.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBox_DrawItem);
            this.comboBox_icon_image.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.comboBox_MeasureItem);
            this.comboBox_icon_image.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            this.comboBox_icon_image.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox_KeyDown);
            this.comboBox_icon_image.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
            // 
            // label01
            // 
            this.label01.AutoSize = true;
            this.label01.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label01.Location = new System.Drawing.Point(3, 7);
            this.label01.Name = "label01";
            this.label01.Size = new System.Drawing.Size(77, 13);
            this.label01.TabIndex = 99;
            this.label01.Text = "Изображение";
            this.label01.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // button_preview
            // 
            this.button_preview.Dock = System.Windows.Forms.DockStyle.Top;
            this.button_preview.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button_preview.Location = new System.Drawing.Point(0, 0);
            this.button_preview.Name = "button_preview";
            this.button_preview.Size = new System.Drawing.Size(435, 23);
            this.button_preview.TabIndex = 159;
            this.button_preview.Text = "Предпросмотр";
            this.button_preview.UseVisualStyleBackColor = true;
            this.button_preview.Click += new System.EventHandler(this.button_preview_Click);
            // 
            // UserControl_preview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_preview);
            this.Controls.Add(this.button_preview);
            this.Name = "UserControl_preview";
            this.Size = new System.Drawing.Size(435, 150);
            this.panel_preview.ResumeLayout(false);
            this.panel_preview.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_preview;
        public System.Windows.Forms.ComboBox comboBox_icon_image;
        private System.Windows.Forms.Label label01;
        public System.Windows.Forms.Button button_preview;
    }
}
