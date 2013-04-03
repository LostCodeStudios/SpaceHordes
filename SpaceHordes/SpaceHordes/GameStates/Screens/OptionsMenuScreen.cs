using GameLibrary.GameStates.Screens;
using System;
using System.IO;

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

#if WINDOWS
        private MenuEntry fullScreen = new MenuEntry("Full Screen: Off");
#endif

        #endregion Fields

#if XBOX
        public StorageContainer Container
        {
            get
            {
                return ScreenManager.StorageDevice.OpenContainer("SpaceHordes");
            }
        }

        public string FilePath(Container which)
        {
            get
            {
                return Path.Combine(which.Path, "settings.txt";
            }
        }
#endif

        #region Static Properties

        /// <summary>
        /// The folder path where save files will be stored for PC.
        /// </summary>
        public static string FolderPath
        {
            get
            {
#if WINDOWS
                return Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Space Hordes";
#endif
            }
        }

        /// <summary>
        /// The path of the scores text file
        /// </summary>
        public static string FilePath
        {
            get
            {
#if WINDOWS
                return FolderPath + @"\settings.txt";
#endif

#if XBOX
                return "";
#endif
            }
        }

        public static int SoundVolume
        {
            get
            {
                int sound;
                int music;
                bool fullscreen;

                ReadSettings(out sound, out music, out fullscreen);

                return sound;
            }

            set
            {
                int sound;
                int music;
                bool fullscreen;

                ReadSettings(out sound, out music, out fullscreen);

                int amount = value % 11;
                WriteSettings(amount, music, fullscreen);

                SpaceHordes.ApplySettings();
            }
        }

        public static int MusicVolume
        {
            get
            {
                int sound;
                int music;
                bool fullscreen;

                ReadSettings(out sound, out music, out fullscreen);

                return music;
            }

            set
            {
                int sound;
                int music;
                bool fullscreen;

                ReadSettings(out sound, out music, out fullscreen);

                int amount = value % 11;
                WriteSettings(sound, amount, fullscreen);

                SpaceHordes.ApplySettings();
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

                ReadSettings(out sound, out music, out fullscreen);

                return fullscreen;
            }

            set
            {
                int sound;
                int music;
                bool fullscreen;

                ReadSettings(out sound, out music, out fullscreen);

                WriteSettings(sound, music, value);

                SpaceHordes.ApplySettings();
            }
        }

#endif

        #endregion Static Properties

        #region Initialization

        //TODO: Hook up the MenuEntries with Event Handlers.
        public OptionsMenuScreen(bool ingame)
            : base("Options")
        {
            this.sound.Selected += sound_selected;
            this.music.Selected += music_selected;

#if WINDOWS
            this.fullScreen.Selected += fullScreen_selected;
#endif

            updateMenuEntryText();

            MenuEntries.Add(this.sound);
            MenuEntries.Add(this.music);
#if WINDOWS
            if (!ingame)
                MenuEntries.Add(this.fullScreen);
#endif
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
        }

        #endregion Helpers

        #region Static Methods

        /// <summary>
        /// Reads the current saved settings.
        /// </summary>
        /// <param name="sound"></param>
        /// <param name="music"></param>
        public static void ReadSettings(out int sound, out int music, out bool fullscreen)
        {
#if WINDOWS
            if (!File.Exists(FilePath))
            {
                WriteInitialSettings();
            }

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
            }
#endif

#if XBOX
            StorageContainer c = Container;

            if (!File.Exists(FilePath(c)))
                WriteInitialSettings();

            using (TextReader tr = new StreamReader(FilePath(c)))
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
            }
            fullscreen = true;
            c.Dispose();
#endif
        }

        public static void WriteSettings(int sound, int music, bool fullscreen)
        {
#if WINDOWS
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
#endif
#if XBOX
            StorageContainer c = Container;
            using (StreamWriter writer = new StreamWriter(FilePath(c))
            {
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
            }
#if XBOX
            c.Dispose();
#endif
        }

        public static void WriteInitialSettings()
        {
            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);

#if WINDOWS
            if (!File.Exists(FilePath))
            {
                using (FileStream fs = File.Create(FilePath))
                {
                    fs.Close();
                }
            }
#endif
#if XBOX
            StorageContainer c = Container;
            if (!File.Exists(FilePath(c)))
            {
                using (FileStream fs = File.Create(FilePath(c)))
                {
                    fs.Close();
                }
            }
#endif

            WriteSettings(10, 10, true);
        }

        #endregion Static Methods
    }
}