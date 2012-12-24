using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using GameLibrary.Input;

namespace GameLibrary.GameStates.Screens
{
    /// <summary>
    /// A screen for displaying 10 high scores, read from a text file.
    /// </summary>
    public class HighScoreScreen : GameScreen
    {
        #region Fields

        const string defaultTitle = "High Scores";
        string titleText;

        const int maxScores = 10;
        string[] initials = new string[maxScores];
        long[] scores = new long[maxScores];

        int selectedScore;
        int players;

        InputAction menuCancel;
        InputAction left;
        InputAction right;

        #endregion

        #region Properties

        public int Players
        {
            get { return players; }
            set 
            {
                players = value;
                if (players < 1)
                    players = 4;
                if (players > 4)
                    players = 1;

                titleText = "Solo Scores";

                if (players > 1)
                {
                    titleText = players.ToString() + " Player Scores";
                }

                ReadScores(players, out initials, out scores);
            }
        }

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
                return FolderPath + @"\scores.txt";
                #endif

                #if XBOX
                return "";
                #endif
            }
        }

        /// <summary>
        /// The default initials in the high score screen.
        /// </summary>
        public static string[] FirstInitials1
        {
            get
            {
                string[] toReturn = new string[10];

                for (int x = 0; x < 10; x++)
                    toReturn[x] = "NLN";

                return toReturn;
            }
        }

        /// <summary>
        /// The default top 10 scores.
        /// </summary>
        public static long[] FirstScores1
        {
            get
            {
                return new long[]
                {
                    10000,
                    7500,
                    5000,
                    2500,
                    2000,
                    1500,
                    1000,
                    750,
                    500,
                    250
                };
            }
        }

        public static string[] FirstInitials2
        {
            get
            {
                string[] toReturn = new string[10];

                for (int x = 0; x < 10; x++)
                {
                    toReturn[x] = "NLN, WHG";
                }

                return toReturn;
            }
        }

        public static long[] FirstScores2
        {
            get
            {
                return new long[]
                {
                    20000,
                    15000,
                    10000,
                    5000,
                    4000,
                    3000,
                    2000,
                    1500,
                    1000,
                    500
                };
            }
        }

        public static string[] FirstInitials3
        {
            get
            {
                string[] toReturn = new string[10];

                for (int x = 0; x < 10; x++)
                {
                    toReturn[x] = "NLN, WHG, DNC";
                }

                return toReturn;
            }
        }

        public static long[] FirstScores3
        {
            get
            {
                return new long[]
                {
                    30000,
                    22500,
                    15000,
                    7500,
                    6000,
                    4500,
                    3000,
                    2250,
                    1500,
                    750
                };
            }
        }

        public static string[] FirstInitials4
        {
            get
            {
                string[] toReturn = new string[10];

                for (int x = 0; x < 10; x++)
                {
                    toReturn[x] = "NLN, WHG, DNC, NKG";
                }

                return toReturn;
            }
        }

        public static long[] FirstScores4
        {
            get
            {
                return new long[]
                {
                    40000,
                    30000,
                    20000,
                    10000,
                    8000,
                    6000,
                    4000,
                    3000,
                    2000,
                    1000
                };
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Makes a new high score screen, with a set title.
        /// </summary>
        /// <param name="titleText"></param>
        public HighScoreScreen(int players, int selected)
        {
            selectedScore = selected;

            menuCancel = new InputAction(
                new Buttons[] { Buttons.B, Buttons.Back },
                new Keys[] { Keys.Escape },
                true);

            left = new InputAction(
                new Buttons[] { Buttons.LeftShoulder, Buttons.LeftTrigger, Buttons.LeftThumbstickLeft },
                new Keys[] { Keys.Left },
                true);

            right = new InputAction(
                new Buttons[] { Buttons.RightShoulder, Buttons.RightTrigger, Buttons.LeftThumbstickRight },
                new Keys[] { Keys.Right },
                true);

#if WINDOWS

            Players = players;

            if (!File.Exists(FilePath))
            {
                WriteInitialScores();
            }

            ReadScores(players, out initials, out scores);
#endif
#if XBOX
            //
#endif
        }
        
        /// <summary>
        /// Makes a high score screen with a specific score highlighted.
        /// </summary>
        /// <param name="selected"></param>
        public HighScoreScreen(int selected)
            : this(1, selected)
        {
        }

        /// <summary>
        /// Makes a high score screen with the top score highlighted.
        /// </summary>
        public HighScoreScreen()
            : this(0)
        {
        }

        #endregion

        #region Input

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            PlayerIndex index;

            if (menuCancel.Evaluate(input, ControllingPlayer, out index))
            {
                ExitScreen(); 
            }

            if (left.Evaluate(input, ControllingPlayer, out index))
            {
                Players--;
            }

            if (right.Evaluate(input, ControllingPlayer, out index))
            {
                Players++;
            }
        }

        #endregion

        #region Update & Draw

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;
            SpriteFont titleFont = ScreenManager.TitleFont;

            spriteBatch.Begin();

            Vector2 nameLocation = new Vector2(300, 180);
            Vector2 scoreLocation = new Vector2(980, 180);

            //Draw each menu entry in turn.
            for (int i = 0; i < maxScores; i++)
            {
                //Draw the initials and number
                Color color = (i == selectedScore) ? Color.Yellow : Color.White;

                spriteBatch.DrawString(font, (i + 1).ToString() + ". " + initials[i], nameLocation, color);

                //Draw the score
                Vector2 loc = new Vector2(scoreLocation.X - font.MeasureString(scores[i].ToString()).X, scoreLocation.Y);

                spriteBatch.DrawString(font, scores[i].ToString(), loc, color);

                //Wrap to next line
                nameLocation.Y += font.LineSpacing;
                scoreLocation.Y += font.LineSpacing;
            }

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            //Draw the menu title centered on the screen
            Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2, 125);
            Vector2 titleOrigin = titleFont.MeasureString(titleText) / 2;
            Color titleColor = new Color(100, 77, 45) * TransitionAlpha;
            float titleScale = 1f;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(titleFont, titleText, titlePosition, titleColor, 0,
                titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Writes the given values into the save file.
        /// </summary>
        /// <param name="players">How many players.</param>
        /// <param name="initials">The 10 initials.</param>
        /// <param name="scores">The 10 scores.</param>
        public static void WriteScores(int players, string[] initials, long[] scores)
        {
            Dictionary<int, string[]> initials1 = new Dictionary<int, string[]>();
            Dictionary<int, long[]> scores1 = new Dictionary<int, long[]>();

            for (int x = 1; x <= 4; x++)
            {
                string[] initials2;
                long[] scores2;
                ReadScores(x, out initials2, out scores2);
                initials1.Add(x, initials2);
                scores1.Add(x, scores2);
            }

            using (StreamWriter sw = new StreamWriter(FilePath))
            {
                for (int x = 1; x <= 4; x++)
                {
                    if (x != players)
                    {
                        //put initials1 rows into 1d array

                        WriteScores(x, initials1[x], scores1[x], sw);
                    }

                    else
                    {
                        WriteScores(players, initials, scores, sw);
                    }
                }

                sw.Close();
            }
        }

        public static void WriteScores(int players, string[] initials, long[] scores, StreamWriter writer)
        {
            writer.WriteLine("[" + players.ToString() + "]");

            for (int x = 0; x < initials.Length; x++)
            {
                writer.WriteLine(initials[x]);
                writer.WriteLine(scores[x]);
            }
        }

        /// <summary>
        /// Writes in the default scores.
        /// </summary>
        public static void WriteInitialScores()
        {
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

            WriteScores(1, FirstInitials1, FirstScores1);
        }

        /// <summary>
        /// Returns the saved initials and scores.
        /// </summary>
        /// <param name="initials"></param>
        /// <param name="scores"></param>
        public static void ReadScores(int players, out string[] initials, out long[] scores)
        {
            initials = new string[10];
            scores = new long[10];

            string[] tags = new string[4];

            if (File.Exists(FilePath))
            {
                using (TextReader tr = new StreamReader(FilePath))
                {
                    int x = 1;
                    while (x <= 4)
                    {
                        bool loop = true;

                        while (loop)
                        {
                            string line = tr.ReadLine();

                            if (line == null)
                            {
                                loop = false;
                            }

                            else if (line == "[" + x.ToString() + "]")
                            {
                                tags[x - 1] = "[" + x.ToString() + "]";
                                loop = false;
                            }
                        }
                        x++;
                    }

                    tr.Close();
                }

                if (tags.Contains<string>("[" + players.ToString() + "]"))
                {
                    using (TextReader tr = new StreamReader(FilePath))
                    {
                        while (tr.ReadLine() != "[" + players.ToString() + "]")
                        {
                        }

                        for (int x = 0; x < maxScores; x++)
                        {
                            string initial = tr.ReadLine();

                            initials[x] = initial;

                            long score = long.Parse(tr.ReadLine());
                            scores[x] = score;
                        }

                        tr.Close();
                    }
                }

                else
                {
                    switch (players)
                    {
                        case 1:
                            initials = FirstInitials1;
                            scores = FirstScores1;
                            break;
                        case 2:
                            initials = FirstInitials2;
                            scores = FirstScores2;
                            break;
                        case 3:
                            initials = FirstInitials3;
                            scores = FirstScores3;
                            break;
                        case 4:
                            initials = FirstInitials4;
                            scores = FirstScores4;
                            break;
                    }
                }
            } 
        }

        /// <summary>
        /// Adds a score to the specified scoreboard.
        /// </summary>
        /// <param name="players">Which scoreboard.</param>
        /// <param name="names">The names of the players</param>
        /// <param name="score">The score</param>
        /// <returns>The place at which the score was placed.</returns>
        public static int AddScore(int players, string names, long score)
        {
            if (!File.Exists(FilePath))
            {
                WriteInitialScores();
            }

            string[] initials;
            long[] scores;
            ReadScores(players, out initials, out scores);

            if (!(score > scores[scores.Length - 1]))
                return -1;

            int place = scores.Length - 1;

            for (int i = 0; i < scores.Length; i++)
            {
                if (score > scores[i])
                {
                    place = i;
                    break;
                }
            }

            for (int i = scores.Length - 1; i > place; i--)
                scores[i] = scores[i - 1];
            
            scores[place] = score;
            initials[place] = names;

            WriteScores(players, initials, scores);

            return place;
        }

        /// <summary>
        /// Tells whether the given number is above the lowest high score and should be added to the score list.
        /// </summary>
        /// <param name="players"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public static bool IsHighScore(int players, long score)
        {
            string[] initials;
            long[] scores;
            ReadScores(players, out initials, out scores);

            return (score > scores[9]);
        }

        #endregion
    }
}
