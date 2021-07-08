using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AmazFit_Watchface_2
{
    public partial class UserControl_text_date : AmazFit_Watchface_2.UserControl_text
    {
        public UserControl_text_date()
        {
            InitializeComponent();
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override bool OptionalSymbol
        {
            get { return base.OptionalSymbol; }
            set { base.OptionalSymbol = false; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override bool ImageError
        {
            get { return base.ImageError; }
            set { base.ImageError = value; }
        }
    }
}
