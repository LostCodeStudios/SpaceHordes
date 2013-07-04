using GameLibrary.GameStates.Screens;
using System;
using System.IO;

#if XBOX
using Microsoft.Xna.Framework.Storage;
#endif

using GameLibrary.Helpers;
using GameLibrary.GameStates;

namespace SpaceHordes.GameStates.Screens
{
    /// <summary>
    /// The options screen is brought up over the top of the
    /// main menu screen, and gives the user a chance to
    /// configure the game in various hopefully useful ways.
    /// </summary>
    internal class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        //TODO: Add MenuEntries for options.
        private MenuEntry sound = new MenuEntry("Sound Volume: 10");

        private MenuEntry music = new MenuEntry("Music Volume: 10");

        private MenuEntry rumble = new MenuEntry("Gamepad Rumble: On");

#if WINDOWS
        private MenuEntry fullScreen = new MenuEntry("Full Screen: Off");
#endif

        #endregion Fields

        #region Static Properties

#if WINDOWS

        /// <summary>
        /// The folder path where save files will be stored for PC.
        /// </summary>
        public static string FolderPath
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Space Hordes";
            }
        }

        /// <summary>
        /// The path of the scores text file
        /// </summary>
        public static string FilePath
        {
            get
            {
                return FolderPath + @"\settings.txt";
            }
        }

#endif

        private static int soundVol;

        public static int SoundVolume
        {
            get
            {
                return soundVol;
            }

            set
            {
                soundVol = value % 11;
                SoundManager.Volume = (float)soundVol / 10;
            }
        }

        private static int musicVol;

        public static int MusicVolume
        {
            get
            {
                return musicVol;
            }

            set
            {
                musicVol = value % 11;

                MusicManager.Volume = (float)musicVol / 10;
            }
        }

#if WINDOWS

        public static bool FullScreen
        {
            get
            {
                int sound;
                int music;
                bool fullscreen;
                bool rumbleOn;

                ReadSettings(out sound, out music, out fullscreen, out rumbleOn);

                return fullscreen;
            }

            set
            {
                int sound;
                int music;
                bool fullscreen;
                bool rumbleOn;

                ReadSettings(out sound, out music, out fullscreen, out rumbleOn);

                WriteSettings(sound, music, value, rumbleOn);

                SpaceHordes.ApplySettings();
            }
        }

#endif

        private static bool rumbleOn;

        public static bool RumbleOn
        {
            get
            {
                return rumbleOn;
            }

            set
            {
                rumbleOn = value;
            }
        }

        #endregion Static Properties

        #region Initialization

        //TODO: Hook up the MenuEntries with Event Handlers.
        public OptionsMenuScreen(bool ingame)
            : base("Options")
        {
            needsStorage = true;

            this.sound.Selected += sound_selected;
            this.music.Selected += music_selected;
            this.rumble.Selected += rumble_selected;

#if WINDOWS
            this.fullScreen.Selected += fullScreen_selected;
#endif

            MenuEntries.Add(this.sound);
            MenuEntries.Add(this.music);
            MenuEntries.Add(this.rumble);
#if WINDOWS
            if (!ingame)
                MenuEntries.Add(this.fullScreen);
#endif

            SelectionChangeSound = "SelectChanged";
            SelectionSound = "Selection";
            CancelSound = "MenuCancel";

            OnExit += new EventHandler(onExit);
        }

        public override void Activate()
        {
            bool fullsc;
            ReadSettings(out soundVol, out musicVol, out fullsc, out rumbleOn);
            updateMenuEntryText();
        }

        #endregion Initialization

        #region Events

        //TODO: Add events for each MenuEntry.
        private void sound_selected(object sender, EventArgs e)
        {
            ++SoundVolume;

            updateMenuEntryText();
        }

        private void music_selected(object sender, EventArgs e)
        {
            ++MusicVolume;

            updateMenuEntryText();
        }

        private void rumble_selected(object sender, EventArgs e)
        {
            RumbleOn = !RumbleOn;

            updateMenuEntryText();
        }

        private void onExit(object sender, EventArgs e)
        {
#if XBOX
            if (!StorageHelper.CheckStorage())
                return;
#endif

            int sound;
            int music;
            bool fullscreen;
            bool rum;

            ReadSettings(out sound, out music, out fullscreen, out rum);

            WriteSettings(soundVol, musicVol, fullscreen, rumbleOn);

            SpaceHordes.ApplySettings();
        }

#if WINDOWS

        private void fullScreen_selected(object sender, EventArgs e)
        {
            FullScreen = !FullScreen;

            updateMenuEntryText();
        }

#endif

        #endregion Events

        #region Helpers

        private void updateMenuEntryText()
        {
            this.sound.Text = "Sound Volume: ";
            this.sound.Text += SoundVolume.ToString();

            this.music.Text = "Music Volume: ";
            this.music.Text += MusicVolume.ToString();

#if WINDOWS
            this.fullScreen.Text = "Full Screen: ";

            switch (FullScreen)
            {
                case true:
                    this.fullScreen.Text += "On";
                    break;

                case false:
                    this.fullScreen.Text += "Off";
                    break;
            }
#endif

            this.rumble.Text = "Gamepad Rumble: ";

            switch (RumbleOn)
            {
                case true:
                    this.rumble.Text += "On";
                    break;

                case false:
                    this.rumble.Text += "Off";
                    break;
            }
        }

        #endregion Helpers

        #region Static Methods

        /// <summary>
        /// Reads the current saved settings.
        /// </summary>
        /// <param name="sound"></param>
        /// <param name="music"></param>
        public static void ReadSettings(out int sound, out int music, out bool fullscreen, out bool rumbleOn)
        {
            UpToDate();

#if WINDOWS

            using (TextReader tr = new StreamReader(FilePath))
            {
                while (tr.ReadLine() != "[Sound]")
                {
                }

                sound = int.Parse(tr.ReadLine());

                while (tr.ReadLine() != "[Music]")
                {
                }

                music = int.Parse(tr.ReadLine());

                while (tr.ReadLine() != "[FullScreen]")
                {
                }

                switch (tr.ReadLine())
                {
                    case "On":
                        fullscreen = true;
                        break;

                    case "Off":
                        fullscreen = false;
                        break;

                    default:
                        fullscreen = false;
                        break;
                }

                while (tr.ReadLine() != "[Rumble]")
                { }

                switch (tr.ReadLine())
                {
                    case "On":
                        rumbleOn = true;
                        break;

                    case "Off":
                        rumbleOn = false;
                        break;

                    default:
                        rumbleOn = false;
                        break;
                }
            }
#endif

#if XBOX
            if (!StorageHelper.FileExists("settings.txt"))
            {
                WriteInitialSettings();
            }

            TextReader tr = new StreamReader(StorageHelper.OpenFile("settings.txt", FileMode.Open));

            while (tr.ReadLine() != "[Sound]")
            {
            }

            sound = int.Parse(tr.ReadLine());

            while (tr.ReadLine() != "[Music]")
            {
            }

            music = int.Parse(tr.ReadLine());

            while (tr.ReadLine() != "[FullScreen]")
            {
            }

            switch (tr.ReadLine())
            {
                case "On":
                    fullscreen = true;
                    break;

                case "Off":
                    fullscreen = false;
                    break;

                default:
                    fullscreen = false;
                    break;
            }

            while (tr.ReadLine() != "[Rumble]")
            {
            }

            switch (tr.ReadLine())
            {
                case "On":
                    rumbleOn = true;
                    break;

                case "Off":
                    rumbleOn = false;
                    break;

                default:
                    rumbleOn = false;
                    break;
            }

            fullscreen = true;

            tr.Close();
#endif
        }

        private static void UpToDate()
        {
#if WINDOWS
            if (!File.Exists(FilePath))
            {
                WriteInitialSettings();
                return;
            }

            using (TextReader tr = new StreamReader(FilePath))
            {
                string text = tr.ReadToEnd();
                if (!text.Contains("[Rumble]"))
                {
                    tr.Close();
                    WriteInitialSettings();
                }
            }
#endif

#if XBOX
            if (!StorageHelper.FileExists("settings.txt"))
            {
                WriteInitialSettings();
                return;
            }

            using (TextReader tr = new StreamReader(StorageHelper.OpenFile("settings.txt", FileMode.Open)))
            {
                string text = tr.ReadToEnd();

                if (!text.Contains("[Rumble]"))
                {
                    tr.Close();
                    WriteInitialSettings();
                }

                tr.Close();
            }

#endif
        }

        public static void WriteSettings(int sound, int music, bool fullscreen, bool rumbleOn)
        {
#if WINDOWS
            StreamWriter writer = new StreamWriter(FilePath);
#endif
#if XBOX
            StreamWriter writer = new StreamWriter(StorageHelper.OpenFile("settings.txt", FileMode.Open));

#endif
            writer.WriteLine("[Sound]");
            writer.WriteLine(sound);

            writer.WriteLine("[Music]");
            writer.WriteLine(music);

            writer.WriteLine("[FullScreen]");

            switch (fullscreen)
            {
                case true:
                    writer.WriteLine("On");
                    break;

                case false:
                    writer.WriteLine("Off");
                    break;
            }

            writer.WriteLine("[Rumble]");

            switch (rumbleOn)
            {
                case true:
                    writer.WriteLine("On");
                    break;

                case false:
                    writer.WriteLine("Off");
                    break;
            }

            writer.Close();
#if XBOX
            StorageHelper.SaveChanges();
#endif
        }

        public static void WriteInitialSettings()
        {
#if WINDOWS
            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);

            if (!File.Exists(FilePath))
            {
                using (FileStream fs = File.Create(FilePath))
                {
                    fs.Close();
                }
            }
#endif
#if XBOX
            if (!StorageHelper.FileExists("settings.txt"))
            {
                StorageHelper.OpenFile("settings.txt", FileMode.Create).Close();
            }
#endif

            WriteSettings(10, 10, true, true);
        }

        #endregion Static Methods
    }
}