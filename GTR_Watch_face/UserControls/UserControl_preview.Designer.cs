
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_preview));
            this.panel_preview = new System.Windows.Forms.Panel();
            this.button_RefreshPreview = new System.Windows.Forms.Button();
            this.button_CreatePreview = new System.Windows.Forms.Button();
            this.comboBox_image = new System.Windows.Forms.ComboBox();
            this.label01 = new System.Windows.Forms.Label();
            this.button_preview = new System.Windows.Forms.Button();
            this.panel_preview.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_preview
            // 
            resources.ApplyResources(this.panel_preview, "panel_preview");
            this.panel_preview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_preview.Controls.Add(this.button_RefreshPreview);
            this.panel_preview.Controls.Add(this.button_CreatePreview);
            this.panel_preview.Controls.Add(this.comboBox_image);
            this.panel_preview.Controls.Add(this.label01);
            this.panel_preview.Name = "panel_preview";
            // 
            // button_RefreshPreview
            // 
            resources.ApplyResources(this.button_RefreshPreview, "button_RefreshPreview");
            this.button_RefreshPreview.Name = "button_RefreshPreview";
            this.button_RefreshPreview.UseVisualStyleBackColor = true;
            this.button_RefreshPreview.Click += new System.EventHandler(this.button_RefreshPreview_Click);
            // 
            // button_CreatePreview
            // 
            resources.ApplyResources(this.button_CreatePreview, "button_CreatePreview");
            this.button_CreatePreview.Name = "button_CreatePreview";
            this.button_CreatePreview.UseVisualStyleBackColor = true;
            this.button_CreatePreview.Click += new System.EventHandler(this.button_CreatePreview_Click);
            // 
            // comboBox_image
            // 
            resources.ApplyResources(this.comboBox_image, "comboBox_image");
            this.comboBox_image.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBox_image.DropDownWidth = 75;
            this.comboBox_image.FormattingEnabled = true;
            this.comboBox_image.Name = "comboBox_image";
            this.comboBox_image.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBox_DrawItem);
            this.comboBox_image.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.comboBox_MeasureItem);
            this.comboBox_image.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            this.comboBox_image.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox_KeyDown);
            this.comboBox_image.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
            // 
            // label01
            // 
            resources.ApplyResources(this.label01, "label01");
            this.label01.Name = "label01";
            // 
            // button_preview
            // 
            resources.ApplyResources(this.button_preview, "button_preview");
            this.button_preview.Name = "button_preview";
            this.button_preview.UseVisualStyleBackColor = true;
            this.button_preview.Click += new System.EventHandler(this.button_preview_Click);
            // 
            // UserControl_preview
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_preview);
            this.Controls.Add(this.button_preview);
            this.Name = "UserControl_preview";
            this.panel_preview.ResumeLayout(false);
            this.panel_preview.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_preview;
        public System.Windows.Forms.ComboBox comboBox_image;
        private System.Windows.Forms.Label label01;
        public System.Windows.Forms.Button button_preview;
        private System.Windows.Forms.Button button_CreatePreview;
        private System.Windows.Forms.Button button_RefreshPreview;
    }
}
