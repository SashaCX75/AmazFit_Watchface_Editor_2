
namespace AmazFit_Watchface_2
{
    partial class UserControl_SystemFont_GroupWeather
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_SystemFont_GroupWeather));
            this.panel_SystemFont = new System.Windows.Forms.Panel();
            this.userControl_FontRotate_weather_Max = new AmazFit_Watchface_2.UserControl_FontRotate_weather();
            this.userControl_FontRotate_weather_Min = new AmazFit_Watchface_2.UserControl_FontRotate_weather();
            this.userControl_FontRotate_weather_Current = new AmazFit_Watchface_2.UserControl_FontRotate_weather();
            this.userControl_SystemFont_weather_Max = new AmazFit_Watchface_2.UserControl_SystemFont_weather();
            this.userControl_SystemFont_weather_Min = new AmazFit_Watchface_2.UserControl_SystemFont_weather();
            this.userControl_SystemFont_weather_Current = new AmazFit_Watchface_2.UserControl_SystemFont_weather();
            this.button_SystemFont = new System.Windows.Forms.Button();
            this.panel_SystemFont.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_SystemFont
            // 
            resources.ApplyResources(this.panel_SystemFont, "panel_SystemFont");
            this.panel_SystemFont.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.panel_SystemFont.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_SystemFont.Controls.Add(this.userControl_FontRotate_weather_Max);
            this.panel_SystemFont.Controls.Add(this.userControl_FontRotate_weather_Min);
            this.panel_SystemFont.Controls.Add(this.userControl_FontRotate_weather_Current);
            this.panel_SystemFont.Controls.Add(this.userControl_SystemFont_weather_Max);
            this.panel_SystemFont.Controls.Add(this.userControl_SystemFont_weather_Min);
            this.panel_SystemFont.Controls.Add(this.userControl_SystemFont_weather_Current);
            this.panel_SystemFont.Name = "panel_SystemFont";
            // 
            // userControl_FontRotate_weather_Max
            // 
            resources.ApplyResources(this.userControl_FontRotate_weather_Max, "userControl_FontRotate_weather_Max");
            this.userControl_FontRotate_weather_Max.AOD = false;
            this.userControl_FontRotate_weather_Max.Collapsed = true;
            this.userControl_FontRotate_weather_Max.Follow = false;
            this.userControl_FontRotate_weather_Max.Name = "userControl_FontRotate_weather_Max";
            this.userControl_FontRotate_weather_Max.Padding_zero = false;
            this.userControl_FontRotate_weather_Max.Separator = false;
            this.userControl_FontRotate_weather_Max.ShowUnit = false;
            this.userControl_FontRotate_weather_Max.ValueChanged += new AmazFit_Watchface_2.UserControl_FontRotate.ValueChangedHandler(this.userControl_ValueChanged);
            this.userControl_FontRotate_weather_Max.AOD_Copy_FontRotate += new AmazFit_Watchface_2.UserControl_FontRotate.AOD_CopyHandler(this.userControl_Copy_SystemFont);
            // 
            // userControl_FontRotate_weather_Min
            // 
            resources.ApplyResources(this.userControl_FontRotate_weather_Min, "userControl_FontRotate_weather_Min");
            this.userControl_FontRotate_weather_Min.AOD = false;
            this.userControl_FontRotate_weather_Min.Collapsed = true;
            this.userControl_FontRotate_weather_Min.Follow = false;
            this.userControl_FontRotate_weather_Min.Name = "userControl_FontRotate_weather_Min";
            this.userControl_FontRotate_weather_Min.Padding_zero = false;
            this.userControl_FontRotate_weather_Min.Separator = false;
            this.userControl_FontRotate_weather_Min.ShowUnit = false;
            this.userControl_FontRotate_weather_Min.ValueChanged += new AmazFit_Watchface_2.UserControl_FontRotate.ValueChangedHandler(this.userControl_ValueChanged);
            this.userControl_FontRotate_weather_Min.AOD_Copy_FontRotate += new AmazFit_Watchface_2.UserControl_FontRotate.AOD_CopyHandler(this.userControl_Copy_SystemFont);
            // 
            // userControl_FontRotate_weather_Current
            // 
            resources.ApplyResources(this.userControl_FontRotate_weather_Current, "userControl_FontRotate_weather_Current");
            this.userControl_FontRotate_weather_Current.AOD = false;
            this.userControl_FontRotate_weather_Current.Collapsed = true;
            this.userControl_FontRotate_weather_Current.Follow = false;
            this.userControl_FontRotate_weather_Current.Name = "userControl_FontRotate_weather_Current";
            this.userControl_FontRotate_weather_Current.Padding_zero = false;
            this.userControl_FontRotate_weather_Current.Separator = false;
            this.userControl_FontRotate_weather_Current.ShowUnit = false;
            this.userControl_FontRotate_weather_Current.ValueChanged += new AmazFit_Watchface_2.UserControl_FontRotate.ValueChangedHandler(this.userControl_ValueChanged);
            this.userControl_FontRotate_weather_Current.AOD_Copy_FontRotate += new AmazFit_Watchface_2.UserControl_FontRotate.AOD_CopyHandler(this.userControl_Copy_SystemFont);
            // 
            // userControl_SystemFont_weather_Max
            // 
            resources.ApplyResources(this.userControl_SystemFont_weather_Max, "userControl_SystemFont_weather_Max");
            this.userControl_SystemFont_weather_Max.AOD = false;
            this.userControl_SystemFont_weather_Max.Collapsed = true;
            this.userControl_SystemFont_weather_Max.Follow = false;
            this.userControl_SystemFont_weather_Max.Name = "userControl_SystemFont_weather_Max";
            this.userControl_SystemFont_weather_Max.Padding_zero = false;
            this.userControl_SystemFont_weather_Max.Separator = false;
            this.userControl_SystemFont_weather_Max.ShowUnit = false;
            this.userControl_SystemFont_weather_Max.ValueChanged += new AmazFit_Watchface_2.UserControl_SystemFont.ValueChangedHandler(this.userControl_ValueChanged);
            this.userControl_SystemFont_weather_Max.AOD_Copy_SystemFont += new AmazFit_Watchface_2.UserControl_SystemFont.AOD_CopyHandler(this.userControl_Copy_SystemFont);
            // 
            // userControl_SystemFont_weather_Min
            // 
            resources.ApplyResources(this.userControl_SystemFont_weather_Min, "userControl_SystemFont_weather_Min");
            this.userControl_SystemFont_weather_Min.AOD = false;
            this.userControl_SystemFont_weather_Min.Collapsed = true;
            this.userControl_SystemFont_weather_Min.Follow = false;
            this.userControl_SystemFont_weather_Min.Name = "userControl_SystemFont_weather_Min";
            this.userControl_SystemFont_weather_Min.Padding_zero = false;
            this.userControl_SystemFont_weather_Min.Separator = false;
            this.userControl_SystemFont_weather_Min.ShowUnit = false;
            this.userControl_SystemFont_weather_Min.ValueChanged += new AmazFit_Watchface_2.UserControl_SystemFont.ValueChangedHandler(this.userControl_ValueChanged);
            this.userControl_SystemFont_weather_Min.AOD_Copy_SystemFont += new AmazFit_Watchface_2.UserControl_SystemFont.AOD_CopyHandler(this.userControl_Copy_SystemFont);
            // 
            // userControl_SystemFont_weather_Current
            // 
            resources.ApplyResources(this.userControl_SystemFont_weather_Current, "userControl_SystemFont_weather_Current");
            this.userControl_SystemFont_weather_Current.AOD = false;
            this.userControl_SystemFont_weather_Current.Collapsed = true;
            this.userControl_SystemFont_weather_Current.Follow = false;
            this.userControl_SystemFont_weather_Current.Name = "userControl_SystemFont_weather_Current";
            this.userControl_SystemFont_weather_Current.Padding_zero = false;
            this.userControl_SystemFont_weather_Current.Separator = false;
            this.userControl_SystemFont_weather_Current.ShowUnit = false;
            this.userControl_SystemFont_weather_Current.ValueChanged += new AmazFit_Watchface_2.UserControl_SystemFont.ValueChangedHandler(this.userControl_ValueChanged);
            this.userControl_SystemFont_weather_Current.AOD_Copy_SystemFont += new AmazFit_Watchface_2.UserControl_SystemFont.AOD_CopyHandler(this.userControl_Copy_SystemFont);
            // 
            // button_SystemFont
            // 
            resources.ApplyResources(this.button_SystemFont, "button_SystemFont");
            this.button_SystemFont.Name = "button_SystemFont";
            this.button_SystemFont.UseVisualStyleBackColor = true;
            this.button_SystemFont.Click += new System.EventHandler(this.button_SystemFont_Click);
            // 
            // UserControl_SystemFont_GroupWeather
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_SystemFont);
            this.Controls.Add(this.button_SystemFont);
            this.Name = "UserControl_SystemFont_GroupWeather";
            this.panel_SystemFont.ResumeLayout(false);
            this.panel_SystemFont.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        protected System.Windows.Forms.Button button_SystemFont;
        public UserControl_SystemFont_weather userControl_SystemFont_weather_Current;
        public UserControl_FontRotate_weather userControl_FontRotate_weather_Max;
        public UserControl_FontRotate_weather userControl_FontRotate_weather_Min;
        public UserControl_FontRotate_weather userControl_FontRotate_weather_Current;
        public UserControl_SystemFont_weather userControl_SystemFont_weather_Max;
        public UserControl_SystemFont_weather userControl_SystemFont_weather_Min;
        public System.Windows.Forms.Panel panel_SystemFont;
    }
}
