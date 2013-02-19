using System;
using System.IO;

using Microsoft.Xna.Framework;
using GameLibrary.GameStates.Screens;
using GameLibrary.Helpers;

namespace SpaceHordes.GameStates.Screens
{
    /// <summary>
    /// The options screen is brought up over the top of the
    /// main menu screen, and gives the user a chance to 
    /// configure the game in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        //TODO: Add MenuEntries for options.
        MenuEntry sound  = new MenuEntry("Sound: On");
        MenuEntry music = new MenuEntry("Music: On");
        MenuEntry fullScreen = new MenuEntry("Full Screen: Off");

        #endregion

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

#if XBOX
                return "";
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

        public static bool SoundOn
        {
            get
            {
                bool sound;
                bool music;
                bool fullscreen;

                ReadSettings(out sound, out music, out fullscreen);

                return sound;
            }

            set
            {
                bool sound;
                bool music;
                bool fullscreen;

                ReadSettings(out sound, out music, out fullscreen);

                WriteSettings(value, music, fullscreen);
                SoundManager.Enabled = value;
            }
        }

        public static bool MusicOn
        {
            get
            {
                bool sound;
                bool music;
                bool fullscreen;

                ReadSettings(out sound, out music, out fullscreen);

                return music;
            }

            set
            {
                bool sound;
                bool music;
                bool fullscreen;

                ReadSettings(out sound, out music, out fullscreen);

                WriteSettings(sound, value, fullscreen);
            }
        }

        public static bool FullScreen
        {
            get
            {
                bool sound;
                bool music;
                bool fullscreen;

                ReadSettings(out sound, out music, out fullscreen);

                return fullscreen;
            }

            set
            {
                bool sound;
                bool music;
                bool fullscreen;

                ReadSettings(out sound, out music, out fullscreen);

                WriteSettings(sound, music, value);
            }
        }

        #endregion

        #region Initialization

        //TODO: Hook up the MenuEntries with Event Handlers.
        public OptionsMenuScreen()
            : base("Options")
        {
            this.sound.Selected += sound_selected;
            this.music.Selected += music_selected;
            this.fullScreen.Selected += fullScreen_selected;

            bool sound;
            bool music;
            bool fullscreen;

            ReadSettings(out sound, out music, out fullscreen);

            SoundOn = sound;
            MusicOn = music;
            FullScreen = fullscreen;

            updateMenuEntryText();

            MenuEntries.Add(this.sound);
            MenuEntries.Add(this.music);
            MenuEntries.Add(this.fullScreen);
        }

        #endregion

        #region Events

        //TODO: Add events for each MenuEntry.
        void sound_selected(object sender, EventArgs e)
        {
            SoundOn = !SoundOn;
            
            updateMenuEntryText();
        }

        void music_selected(object sender, EventArgs e)
        {
            MusicOn = !MusicOn;

            updateMenuEntryText();
        }

        void fullScreen_selected(object sender, EventArgs e)
        {
            FullScreen = !FullScreen;

            updateMenuEntryText();
        }

        #endregion

        #region Helpers

        private void updateMenuEntryText()
        {
            this.sound.Text = "Sound: ";

            switch (SoundOn)
            {
                case true:
                    this.sound.Text += "On";
                    break;

                case false:
                    this.sound.Text += "Off";
                    break;
            }

            this.music.Text = "Music: ";

            switch (MusicOn)
            {
                case true:
                    this.music.Text += "On";
                    break;

                case false:
                    this.music.Text += "Off";
                    break;
            }

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
        }
        #endregion

        #region Static Methods

        /// <summary>
        /// Reads the current saved settings.
        /// </summary>
        /// <param name="sound"></param>
        /// <param name="music"></param>
        public static void ReadSettings(out bool sound, out bool music, out bool fullscreen)
        {
            if (!File.Exists(FilePath))
            {
                WriteInitialSettings();
            }

            using (TextReader tr = new StreamReader(FilePath))
            {
                while (tr.ReadLine() != "[Sound]")
                {
                }

                switch (tr.ReadLine())
                {
                    case "On":
                        sound = true;
                        break;

                    case "Off":
                        sound = false;
                        break;

                    default:
                        sound = true;
                        break;
                }

                while (tr.ReadLine() != "[Music]")
                {
                }

                switch (tr.ReadLine())
                {
                    case "On":
                        music = true;
                        break;

                    case "Off":
                        music = false;
                        break;

                    default:
                        music = true;
                        break;
                }

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
        }

        public static void WriteSettings(bool sound, bool music, bool fullscreen)
        {
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine("[Sound]");

                switch (sound)
                {
                    case true:
                        writer.WriteLine("On");
                        break;

                    case false:
                        writer.WriteLine("Off");
                        break;
                }

                writer.WriteLine("[Music]");

                switch (music)
                {
                    case true:
                        writer.WriteLine("On");
                        break;

                    case false:
                        writer.WriteLine("Off");
                        break;
                }

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
        }

        public static void WriteInitialSettings()
        {
            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);

            if (!File.Exists(FilePath))
            {
                using (FileStream fs = File.Create(FilePath))
                {
                    fs.Close();
                }
            }

            WriteSettings(true, true, false);
        }

        #endregion
    }
}
