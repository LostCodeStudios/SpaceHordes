using System;
using System.Collections.Generic;

#if XBOX
using Microsoft.Xna.Framework.Storage;
#endif

using GameLibrary.GameStates.Screens;
using System.IO;
using SpaceHordes.Entities.Systems;
using GameLibrary.Input;
using Microsoft.Xna.Framework;
using GameLibrary.GameStates;

namespace SpaceHordes.GameStates.Screens
{
    public class LevelSelectScreen : MenuScreen
    {
        #region Static Properties

#if XBOX
        public static StorageContainer MyContainer
        {
            get
            {
                return ScreenManager.GetContainer();
            }
        }
#endif

#if WINDOWS

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
                return FolderPath + @"\levels.txt";
#endif
            }
        }

#endif

        #endregion Static Properties

        private bool[] levels;
        private static int levelCount = 5;
        private string[] levelTitles;

        private int bossStart = 0;

        private string font;
        private PlayerIndex[] indices;

        #region Initialization

        public LevelSelectScreen(string fontName, PlayerIndex[] indices)
            : base("Levels")
        {
            levelTitles = new string[]
            {
                "Wave 1",
                "Wave 2",
                "Wave 3",
                "Boss Rush",
                "Endless"
            };

            levels = ReadData();

            this.font = fontName;
            this.indices = indices;

            for (int i = 0; i < levelCount; ++i)
            {
                string text = "???";
                if (levels[i])
                {
                    text = levelTitles[i];
                }

                MenuEntry entry = new MenuEntry(text);

                if (levels[i])
                {
                    entry.Selected += new EventHandler<PlayerIndexEventArgs>(select);
                }
                MenuEntries.Add(entry);
            }

            SelectionChangeSound = "SelectChanged";
            SelectionSound = "Selection";
            CancelSound = "MenuCancel";
        }

        #endregion Initialization

        private void select(object sender, EventArgs e)
        {
            SpawnState[] states;

            #region Levels

            switch (selectedEntry)
            {
                case 0:
                    states = new SpawnState[]
                    {
                        SpawnState.Wave,
                        SpawnState.Peace,
                        SpawnState.Wave,
                        SpawnState.Boss,
                        SpawnState.Peace,
                        SpawnState.Wave,
                        SpawnState.Boss,
                        SpawnState.Peace,
                        SpawnState.Wave,
                        SpawnState.Boss,
                        SpawnState.Victory
                    };
                    break;

                case 1:
                    bossStart = 3;
                    states = new SpawnState[]
                    {
                        SpawnState.Wave,
                        SpawnState.Boss,
                        SpawnState.Peace,
                        SpawnState.Wave,
                        SpawnState.Peace,
                        SpawnState.Surge,
                        SpawnState.Boss,
                        SpawnState.Peace,
                        SpawnState.Wave,
                        SpawnState.Boss,
                        SpawnState.Victory
                    };
                    break;

                case 2:
                    bossStart = 6;
                    states = new SpawnState[]
                    {
                        SpawnState.Surge,
                        SpawnState.Boss,
                        SpawnState.Peace,
                        SpawnState.Wave,
                        SpawnState.Peace,
                        SpawnState.Wave,
                        SpawnState.Boss,
                        SpawnState.Peace,
                        SpawnState.Wave,
                        SpawnState.Peace,
                        SpawnState.Surge,
                        SpawnState.Boss,
                        SpawnState.Peace,
                        SpawnState.Wave,
                        SpawnState.Peace,
                        SpawnState.Surge,
                        SpawnState.Boss,
                        SpawnState.Victory
                    };
                    break;

                case 3:
                    bossStart = 0;
                    states = new SpawnState[]
                    {
                        SpawnState.Boss,
                        SpawnState.Boss,
                        SpawnState.Boss,
                        SpawnState.Peace,
                        SpawnState.Boss,
                        SpawnState.Boss,
                        SpawnState.Boss,
                        SpawnState.Peace,
                        SpawnState.Boss,
                        SpawnState.Boss,
                        SpawnState.Boss,
                        SpawnState.Boss,
                        SpawnState.Victory
                    };
                    break;

                case 4:
                    states = new SpawnState[]
                    {
                        SpawnState.Endless
                    };
                    break;

                default:
                    states = new SpawnState[]
                    {
                        SpawnState.Victory
                    };
                    break;
            }

            #endregion Levels

            ExitScreen();
            Manager.AddScreen(new GameplayScreen(font, bossStart, indices, selectedEntry, states), null);
        }

        #region Static Methods

        public static void LevelCleared(int level)
        {
            bool[] l = ReadData();

            if (level < l.Length)
            {
                l[level] = true;
            }

            WriteData(l);
        }

        public static bool[] ReadData()
        {
            List<bool> data = new List<bool>();

#if DEBUG
            for (int i = 0; i < levelCount; ++i)
            {
                data.Add(true);
            }

            return data.ToArray();
#endif

#if WINDOWS
            if (File.Exists(FilePath))
            {
                StreamReader reader = new StreamReader(FilePath);
#endif
#if XBOX
            StorageContainer c = MyContainer;
            if (c.FileExists("levels.txt"))
            {
                StreamReader reader = new StreamReader(c.OpenFile("levels.txt", FileMode.Open));
#endif
                for (int x = 0; x < levelCount; ++x)
                {
                    string next = reader.ReadLine();

                    switch (next)
                    {
                        case "True":
                            data.Add(true);
                            break;

                        case "False":
                            data.Add(false);
                            break;

                        default:
                            data.Add(false);
                            break;
                    }
                }

                reader.Close();
#if XBOX
                c.Dispose();
#endif
            }
            else
            {
#if XBOX
                c.Dispose();
#endif
                WriteInitialData();
                return ReadData();
            }

            return data.ToArray();
        }

        public static void WriteData(bool[] data)
        {
#if WINDOWS
            StreamWriter writer = new StreamWriter(FilePath);
#endif
#if XBOX
            StorageContainer c = MyContainer;
            StreamWriter writer = new StreamWriter(c.OpenFile("levels.txt", FileMode.Open));
#endif
            for (int i = 0; i < levelCount; ++i)
            {
                writer.WriteLine(data[i].ToString());
            }

            writer.Close();
#if XBOX
            c.Dispose();
#endif
        }

        public static void WriteInitialData()
        {
#if WINDOWS
            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);

            if (!File.Exists(FilePath))
            {
                //If there is no scores file, make a new one
                using (FileStream fs = File.Create(FilePath))
                {
                    fs.Close();
                }
            }
#endif
#if XBOX
            StorageContainer c = MyContainer;
            if (!c.FileExists("levels.txt"))
            {
                c.OpenFile("levels.txt", FileMode.Create);
            }
#endif

            bool[] data = new bool[levelCount];

            for (int i = 0; i < levelCount; ++i)
            {
                data[i] = false;
            }

            data[0] = true;
            data[4] = true;
#if XBOX
            c.Dispose();
#endif
            WriteData(data);
        }

        #endregion Static Methods
    }
}