using GameLibrary.GameStates;
using GameLibrary.GameStates.Screens;
using GameLibrary.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace SpaceHordes.GameStates.Screens
{
    internal class InitialEntryScreen : GameScreen
    {
        #region Fields

        private GameOverScreen parent;

        private Vector2 position;

        private int selectedChar = 0;
        private InitialEntryChar[] initials = new InitialEntryChar[3];

        private InputAction up;
        private InputAction down;
        private InputAction left;
        private InputAction right;
        private InputAction accept;

        private bool expired = false;

        #endregion Fields

        #region Properties

        /// <summary>
        /// The initials that have been entered by the player.
        /// </summary>
        public string Initials
        {
            get
            {
                string toReturn = "";

                for (int i = 0; i < initials.Length; ++i)
                    toReturn += initials[i].Text;

                return toReturn;
            }
        }

        public bool Expired
        {
            get { return expired; }
            set { expired = value; }
        }

        #endregion Properties

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
                return Path.Combine(which.Path, "initials.txt";
            }
        }
#endif

        #region Initialization

        public InitialEntryScreen(Vector2 position, GameOverScreen parent)
        {
            this.parent = parent;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            IsPopup = true;

            this.position = position;

            up = new InputAction(
                new Buttons[] { Buttons.LeftThumbstickUp, Buttons.DPadUp },
                new Keys[] { Keys.Up },
                true);

            down = new InputAction(
                new Buttons[] { Buttons.LeftThumbstickDown, Buttons.DPadDown },
                new Keys[] { Keys.Down },
                true);

            left = new InputAction(
                new Buttons[] { Buttons.LeftThumbstickLeft, Buttons.DPadLeft },
                new Keys[] { Keys.Left },
                true);

            right = new InputAction(
                new Buttons[] { Buttons.LeftThumbstickRight, Buttons.DPadRight },
                new Keys[] { Keys.Right },
                true);

            accept = new InputAction(
                new Buttons[] { Buttons.Start , Buttons.A },
                new Keys[] { Keys.Enter, Keys.Space },
                true);
        }

        public override void Activate()
        {
            base.Activate();

            //Draw using a center point rather than a top left corner
            Vector2 loc = position
                - new Vector2(7, 0)
                - new Vector2(ScreenManager.InitialEntryFont.MeasureString("AAA").X / 2, 0);

#if WINDOWS
            if (!File.Exists(OptionsMenuScreen.FolderPath + "/initials.txt"))
                setInitialInitials();
#endif
#if XBOX
            StorageContainer c = Container;
            if (!File.Exists(FilePath(c)))
            {
                c.Dispose();
                setInitialInitials();
            }
#endif
            string name = InitialsOf((PlayerIndex)ControllingPlayer);

            for (int i = 0; i < initials.Length; ++i)
            {
                initials[i] = new InitialEntryChar(name[i].ToString());
                initials[i].Position = loc;

                loc += new Vector2(ScreenManager.InitialEntryFont.MeasureString(name[i].ToString()).X + 7, 0);
            }
        }

        #endregion Initialization

        #region Update & Draw

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            foreach (MenuEntry entry in initials)
            {
                if (initials[selectedChar] == entry)
                    entry.Update(true, gameTime);
                else
                    entry.Update(false, gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            ScreenManager.SpriteBatch.Begin();

            foreach (MenuEntry entry in initials)
            {
                if (initials[selectedChar] == entry)
                    entry.Draw(this, true, gameTime);
                else
                    entry.Draw(this, false, gameTime);
            }

            ScreenManager.SpriteBatch.End();
        }

        #endregion Update & Draw

        #region Input

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            base.HandleInput(gameTime, input);

            foreach (MenuEntry entry in initials)
            {
                if (input.MouseHoverIn(entry.ClickRectangle))
                {
                    selectedChar = Array.IndexOf(initials, entry);

                    char ch = entry.Text.ToCharArray()[0];
                    ch = (char)((int)ch + input.RelativeScrollValue);
                    entry.Text = ch.ToString();
                }
            }

            PlayerIndex index;

            if (up.Evaluate(input, ControllingPlayer, out index))
            {
                char ch = initials[selectedChar].Text.ToCharArray()[0];
                ch += (char)1;
                initials[selectedChar].Text = ch.ToString();
            }

            if (down.Evaluate(input, ControllingPlayer, out index))
            {
                char ch = initials[selectedChar].Text.ToCharArray()[0];
                ch -= (char)1;
                initials[selectedChar].Text = ch.ToString();
            }

            char c = initials[selectedChar].Text.ToCharArray()[0];

            int num = (int)c;

            if (num < 65)
                num = 90;

            if (num > 90)
                num = 65;

            c = (char)num;

            initials[selectedChar].Text = c.ToString();

            if (right.Evaluate(input, ControllingPlayer, out index))
            {
                selectedChar++;
            }

            if (left.Evaluate(input, ControllingPlayer, out index))
            {
                selectedChar--;
            }

            selectedChar = (int)MathHelper.Clamp((float)selectedChar, 0f, 2f);

            if (!expired && accept.Evaluate(input, ControllingPlayer, out index))
            {
                
                parent.Initials.Add(Initials);
                SetInitialsOf((PlayerIndex)ControllingPlayer, Initials);
                ExitScreen();
                expired = true;
            }
        }

        #endregion Input

        #region Static Methods

        public static string InitialsOf(PlayerIndex index)
        {
#if WINDOWS
            StreamReader reader = new StreamReader(OptionsMenuScreen.FolderPath + "/initials.txt");
#endif
#if XBOX
            StorageContainer c = Container;
            StreamReader reader = new StreamReader(FilePath(c));
#endif
            string toReturn = "";
            while (reader.ReadLine() != "[" + index.ToString() + "]")
            {
            }
            toReturn = reader.ReadLine();
            reader.Close();
#if XBOX
            c.Dispose();
#endif
            return toReturn;
        }

        public static void SetInitialsOf(PlayerIndex index, string value)
        {
            string[] initials = new string[4];
            for (int i = 0; i < 4; ++i)
            {
                initials[i] = InitialsOf((PlayerIndex)i);
            }

#if WINDOWS
            StreamWriter writer = new StreamWriter(OptionsMenuScreen.FolderPath + "/initials.txt");
#endif
#if XBOX
            StorageContainer c = Container;
            StreamWriter writer = new StreamWriter(FilePath(c));
#endif
            for (int i = 0; i < 4; ++i)
            {
                writer.WriteLine("[" + ((PlayerIndex)i).ToString() + "]");
                if (i != (int)index)
                    writer.WriteLine(initials[i]);
                else
                    writer.WriteLine(value);
            }
            writer.Close();
#if XBOX
            c.Dispose();
#endif
        }

        public static void setInitialInitials()
        {
            string[] initials = new string[]
            {
                "AAA",
                "AAA",
                "AAA",
                "AAA"
            };

#if WINDOWS
            StreamWriter writer = new StreamWriter(OptionsMenuScreen.FolderPath + "/initials.txt");
#endif
#if XBOX
            StorageContainer c = Container;
            StreamWriter writer = new StreamWriter(FilePath(c));
#endif
            for (int i = 0; i < 4; ++i)
            {
                writer.WriteLine("[" + ((PlayerIndex)i).ToString() + "]");
                writer.WriteLine(initials[i]);
            }
            writer.Close();
        }

        #endregion Static Methods
    }
}