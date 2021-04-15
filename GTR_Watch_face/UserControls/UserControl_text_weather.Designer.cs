
namespace AmazFit_Watchface_2
{
    partial class UserControl_text_weather
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_text_weather));
            this.SuspendLayout();
            // 
            // label08
            // 
            resources.ApplyResources(this.label08, "label08");
            // 
            // label06
            // 
            resources.ApplyResources(this.label06, "label06");
            // 
            // label07
            // 
            resources.ApplyResources(this.label07, "label07");
            // 
            // label02
            // 
            resources.ApplyResources(this.label02, "label02");
            // 
            // label1084
            // 
            resources.ApplyResources(this.label1084, "label1084");
            // 
            // label1085
            // 
            resources.ApplyResources(this.label1085, "label1085");
            // 
            // button_text
            // 
            resources.ApplyResources(this.button_text, "button_text");
            // 
            // checkBox_addZero
            // 
            resources.ApplyResources(this.checkBox_addZero, "checkBox_addZero");
            this.checkBox_addZero.CheckedChanged += new System.EventHandler(this.checkBox_addZero_CheckedChanged);
            // 
            // UserControl_text_weather
            // 
            resources.ApplyResources(this, "$this");
            this.Name = "UserControl_text_weather";
            this.ResumeLayout(false);

        }

        #endregion
    }
}
