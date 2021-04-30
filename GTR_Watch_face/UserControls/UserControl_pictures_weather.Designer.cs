
namespace AmazFit_Watchface_2
{
    partial class UserControl_pictures_weather
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

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_pictures_weather));
            this.toolTip_Weather = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_pictures_count)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox_pictures_image
            // 
            resources.ApplyResources(this.comboBox_pictures_image, "comboBox_pictures_image");
            this.toolTip_Weather.SetToolTip(this.comboBox_pictures_image, resources.GetString("comboBox_pictures_image.ToolTip"));
            // 
            // label03
            // 
            resources.ApplyResources(this.label03, "label03");
            this.toolTip_Weather.SetToolTip(this.label03, resources.GetString("label03.ToolTip"));
            // 
            // numericUpDown_pictures_count
            // 
            resources.ApplyResources(this.numericUpDown_pictures_count, "numericUpDown_pictures_count");
            this.toolTip_Weather.SetToolTip(this.numericUpDown_pictures_count, resources.GetString("numericUpDown_pictures_count.ToolTip"));
            // 
            // toolTip_Weather
            // 
            this.toolTip_Weather.ToolTipTitle = "Weather icons";
            // 
            // UserControl_pictures_weather
            // 
            resources.ApplyResources(this, "$this");
            this.Name = "UserControl_pictures_weather";
            this.toolTip_Weather.SetToolTip(this, resources.GetString("$this.ToolTip"));
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_pictures_count)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip_Weather;
    }
}
