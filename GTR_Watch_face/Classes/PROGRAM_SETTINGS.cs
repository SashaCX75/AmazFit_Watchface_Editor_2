namespace AmazFit_Watchface_2
{
    class PROGRAM_SETTINGS
    {
        public bool Settings_Unpack_Dialog = true;
        public bool Settings_Unpack_Save = false;
        public bool Settings_Unpack_Replace = false;

        public bool Settings_Pack_Dialog = false;
        public bool Settings_Pack_GoToFile = true;
        public bool Settings_Pack_DoNotning = false;

        public bool Settings_AfterUnpack_Dialog = false;
        public bool Settings_AfterUnpack_Download = true;
        public bool Settings_AfterUnpack_DoNothing = false;

        public bool Settings_Open_Dialog = false;
        public bool Settings_Open_Download = true;
        public bool Settings_Open_DoNotning = false;

        public bool Model_GTR47 = true;
        public bool Model_GTR42 = false;
        public bool Model_GTS = false;
        public bool Model_TRex = false;
        public bool Model_AmazfitX = false;
        public bool Model_Verge = false;

        public bool ShowBorder = false;
        public bool Crop = true;
        public bool Show_Warnings = true;
        public bool Show_Shortcuts = true;
        public bool Shortcuts_Area = true;
        public bool Shortcuts_Border = true;
        public bool Shortcuts_Center_marker = true;
        public bool Show_CircleScale_Area = false;
        public float Scale = 1f;
        public float Gif_Speed = 1f;
        public int Animation_Preview_Speed = 4;

        public bool ShowMiles = true;
        public bool ShowIn12hourFormat = true;
        public bool DoNotShowMaxMinTemp = false;

        public string pack_unpack_dir { get; set; }
        public string unpack_command_GTR47 = "--gtr2 47 --file";
        public string unpack_command_GTR42 = "--gtr 42 --file";
        public string unpack_command_GTS = "--gts2 --file";
        public string unpack_command_TRex = "--trex --file";
        public string unpack_command_AmazfitX = "--amazfitx --file";
        public string unpack_command_Verge = "--vergelite --file";

        public string language { get; set; }

        public int Splitter_Pos = 0;
    }
}
