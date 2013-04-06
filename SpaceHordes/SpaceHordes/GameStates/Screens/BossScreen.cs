using GameLibrary.GameStates;
using GameLibrary.Helpers;
using GameLibrary.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SpaceHordes.GameStates.Screens
{
    internal struct BossInfo
    {
        public string SpriteKey;
        public string BossName;

        public BossInfo(string key, string name)
        {
            SpriteKey = key;
            BossName = name;
        }
    }

    public class BossScreen : GameScreen
    {
        #region Boss Info
        private static BossInfo[] bosses = new BossInfo[]
        {
            new BossInfo("smasher", "The Smasher"),
            new BossInfo("greenbossship", "Big Green"),
            new BossInfo("clawbossthing", "Clawdia"),
            new BossInfo("eye", "The Oculus"),
            new BossInfo("brain", "Father Brain"),
            new BossInfo("redgunship", "The Gunner"),
            new BossInfo("bigredblobboss", "Big Red"),
            new BossInfo("blimp", "Lead Zeppelin"),
            new BossInfo("giantgraybossship", "Big Blue"),
            new BossInfo("birdbody", "The Harbinger"),
            new BossInfo("flamer", "The Flamer"),
            new BossInfo("massivebluemissile", "The Jabber-W0K"),
            new BossInfo("killerhead", "The Destroyer")
        };
        #endregion

        #region Fields

        private ContentManager content;
        private SpriteSheet spriteSheet;

        private int index = 0;
        private string currentKey;
        private int lastIndex;
        private int nextIndex;

        private InputAction next;
        private InputAction previous;
        private InputAction cancel;

        #endregion Fields

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
                return FolderPath + @"\bosses.txt";
#endif

#if XBOX
                return "";
#endif
            }
        }

        public static bool[] ClearedBosses
        {
            get { return clearedBosses; }
            set
            {
                clearedBosses = value;
                WriteData(clearedBosses);
            }
        }

        private static bool[] clearedBosses;

        #endregion Static Properties

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
                return Path.Combine(which.Path, "bosses.txt";
            }
        }
#endif

        #region Initialization

        public BossScreen(SpriteSheet sheet)
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            spriteSheet = sheet;
            currentKey = bosses[0].SpriteKey;

            next = new InputAction(
                new Buttons[]
                {
                    Buttons.LeftThumbstickRight,
                    Buttons.DPadRight
                },
                new Keys[]
                {
                    Keys.Right
                },
                true);

            previous = new InputAction(
                new Buttons[]
                {
                    Buttons.LeftThumbstickLeft,
                    Buttons.DPadLeft
                },
                new Keys[]
                {
                    Keys.Left
                },
                true);

            cancel = new InputAction(
                new Buttons[]
                {
                    Buttons.Back,
                    Buttons.B
                },
                new Keys[]
                {
                    Keys.Escape
                },
                true);
        }

        public override void Activate()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
        }

        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void Unload()
        {
            content.Unload();
        }

        #endregion Initialization

        #region Update & Draw

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            bool[] data = ReadData();
            bool current = data[index];

            float transitionOffset = TransitionPosition;

            if (lastIndex == -1)
                transitionOffset *= (ScreenState == ScreenState.TransitionOn) ? 1 : -1;
            else if (ScreenState == ScreenState.TransitionOn)
                transitionOffset *= (lastIndex > index) ? -1 : 1;
            else
            {
                if (nextIndex == -1)
                    transitionOffset *= -1;
                else
                    transitionOffset *= (index > nextIndex) ? 1 : -1;
            }

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();

            float scale = 3.5f;

            Rectangle source = spriteSheet[currentKey][0];
            Vector2 spriteLoc = ScreenHelper.Center;

            spriteLoc -= (new Vector2(source.Width / 2, source.Height / 2) * scale);
            Rectangle destination;

            Color color = current ? Color.White : Color.Black;
            destination = new Rectangle((int)spriteLoc.X, (int)spriteLoc.Y, (int)(source.Width * scale), (int)(source.Height * scale));

            #region Special Cases

            Rectangle[] extraSource = new Rectangle[10];
            Rectangle[] extra = new Rectangle[10];
            if (currentKey.Equals("smasher"))
            {
                //special draw handling
                extraSource[0] = spriteSheet["smasherball"][0];
                extra[0] = new Rectangle(destination.X - destination.Width, destination.Y, (int)(extraSource[0].Width * scale), (int)(extraSource[0].Height * scale));
            }

            else if (currentKey.Equals("birdbody"))
            {
                //bird contains two sprites so needs special draw handling
                extraSource[0] = spriteSheet["birdhead"][2];
                extra[0] = new Rectangle(destination.X + (int)(73 * scale), destination.Y + (int)(51 * scale), (int)(extraSource[0].Width * scale), (int)(extraSource[0].Height * scale));
            }

            else if (currentKey.Equals("blimp"))
            {
                scale = 4.5f;
            }

            else if (currentKey.Equals("giantgraybossship"))
            {
                scale = 4f;
            }

            else if (currentKey.Equals("killerhead"))
            {
                //likewise here
                extraSource[0] = spriteSheet["killerleftgun"][0];
                extraSource[1] = spriteSheet["killerrightgun"][0];

                extra[0] = new Rectangle(destination.X - (int)(25 * scale), destination.Y + (int)(19 * scale), (int)(extraSource[0].Width * scale), (int)(extraSource[0].Height * scale));
                extra[1] = new Rectangle(destination.X + (int)(118 * scale), destination.Y + (int)(18 * scale), (int)(extraSource[1].Width * scale), (int)(extraSource[1].Height * scale));
            }

            if (scale != 5f)
            {
                spriteLoc = ScreenHelper.Center;
                spriteLoc -= (new Vector2(source.Width / 2, source.Height / 2) * scale);
                destination.X = (int)(spriteLoc.X);
                destination.Y = (int)(spriteLoc.Y);

                destination.Width = (int)(source.Width * scale);
                destination.Height = (int)(source.Height * scale);

                for (int x = 0; x < extra.Count(); ++x)
                {
                    if (extra[x] != Rectangle.Empty)
                    {
                        extra[x].Width = (int)(extraSource[x].Width * scale);
                        extra[x].Height = (int)(extraSource[x].Height * scale);
                    }
                }
            }

            #endregion Special Cases

            destination.X += (int)(transitionOffset * ScreenHelper.Viewport.Width);
            spriteBatch.Draw(spriteSheet.Texture, destination, source, color);

            for (int x = 0; x < extra.Count(); ++x)
            {
                if (extra[x] != Rectangle.Empty)
                {
                    extra[x].X += (int)(transitionOffset * ScreenHelper.Viewport.Width);
                    spriteBatch.Draw(spriteSheet.Texture, extra[x], extraSource[x], color);
                }
            }

            string text = current ? bosses[index].BossName : "?????";
            Vector2 textDest = new Vector2(ScreenHelper.Viewport.Width / 2 + (float)(transitionOffset * ScreenHelper.Viewport.Width), ScreenHelper.Viewport.Height * 0.80f);
            Vector2 size = ScreenManager.Font.MeasureString(text);
            Vector2 textorigin = size / 2;
            spriteBatch.DrawString(ScreenManager.Font, text, textDest, Color.White, 0f, textorigin, 1f, SpriteEffects.None, 0);
            spriteBatch.End();
        }

        #endregion Update & Draw

        #region Handle Input

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (ScreenState == ScreenState.TransitionOff || ScreenState == ScreenState.TransitionOn)
                return;

            PlayerIndex indx;

            if (next.Evaluate(input, ControllingPlayer, out indx))
            {
                if (index != bosses.Count() - 1)
                {
                    string key = bosses[index + 1].SpriteKey;
                    ExitScreen();
                    BossScreen newS = new BossScreen(spriteSheet);
                    newS.index = index + 1;
                    nextIndex = index + 1;
                    newS.lastIndex = index;
                    newS.currentKey = key;
                    newS.OnExit += MainMenuScreen.BossScreenExited;
                    ScreenManager.AddScreen(newS, ControllingPlayer);
                }
            }

            if (previous.Evaluate(input, ControllingPlayer, out indx))
            {
                if (index != 0)
                {
                    string key = bosses[index - 1].SpriteKey;
                    ExitScreen();
                    BossScreen newS = new BossScreen(spriteSheet);
                    newS.index = index - 1;
                    nextIndex = index - 1;
                    newS.lastIndex = index;
                    newS.currentKey = key;
                    newS.OnExit += MainMenuScreen.BossScreenExited;
                    ScreenManager.AddScreen(newS, ControllingPlayer);
                }
            }

            if (cancel.Evaluate(input, ControllingPlayer, out indx))
            {
                ExitScreen();
                nextIndex = -1;
                CallExit();
            }
        }

        #endregion Handle Input

        #region Static Methods

        public static bool[] ReadData()
        {
            List<bool> data = new List<bool>();

#if WINDOWS
            if (File.Exists(FilePath))
            {
                using (StreamReader reader = new StreamReader(FilePath))
#endif
#if XBOX
            StorageContainer c = Container;
            if (File.Exists(FilePath(c))
            {
                using (StreamReader reader = new StreamReader(FilePath(c)))
#endif
                {
                    for (int x = 0; x < bosses.Length; ++x)
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
                }
            }
            else
            {
                WriteInitialData();
                return ReadData();
            }

#if XBOX
            c.Dispose();
#endif
            return data.ToArray();
        }

        public static void WriteData(bool[] data)
        {
#if WINDOWS
            using (StreamWriter writer = new StreamWriter(FilePath))
#endif
#if XBOX
            StorageContainer c = Container;
            using (StreamWriter writer = new StreamWriter(FilePath(c)))
#endif
            {
                for (int i = 0; i < bosses.Count(); ++i)
                {
                    writer.WriteLine(data[i].ToString());
                }

                writer.Close();
            }

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
            StorageContainer c = Container;
            if (!File.Exists(FilePath(c)))
            {
                using (FileStream fs = File.Create(FilePath(C)))
                {
                    fs.Close();
                }
            }
            c.Dispose();
#endif

            bool[] data = new bool[bosses.Length];

            for (int i = 0; i < bosses.Length; ++i)
            {
                data[i] = false;
            }

            WriteData(data);
        }

        public static void BossKilled(string bossName)
        {
            int index = 0;

            for (index = 0; index < ClearedBosses.Count(); ++index)
            {
                if (bosses[index].BossName == bossName)
                    break;
            }

            bool[] b = ClearedBosses.Clone() as bool[];
            b[index] = true;
            ClearedBosses = b;
        }

        #endregion Static Methods
    }
}