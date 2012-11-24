using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using Game_Library.Input;

namespace Game_Library.GameStates.Screens
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

        InputAction menuCancel;

        #endregion

        #region Initialization

        /// <summary>
        /// Makes a new high score screen, with a set title.
        /// </summary>
        /// <param name="titleText"></param>
        public HighScoreScreen(string titleText, int selected)
        {
            this.titleText = titleText;
            selectedScore = selected;

            menuCancel = new InputAction(
                new Buttons[] { Buttons.B, Buttons.Back },
                new Keys[] { Keys.Escape },
                true);

#if WINDOWS
            string folderPath = @"C:\Users\Nathaniel\Documents\Space Hordes";

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string filePath = folderPath + @"\scores.txt";

            if (!File.Exists(filePath))
            {
                //If there is no scores file, make a new one

                FileStream fs = null;
                using (fs = File.Create(filePath))
                {
                    fs.Close();
                }

                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.WriteLine("NLN 10000");
                    sw.WriteLine("NLN 7500");
                    sw.WriteLine("NLN 5000");
                    sw.WriteLine("NLN 2500");
                    sw.WriteLine("NLN 2000");
                    sw.WriteLine("NLN 1500");
                    sw.WriteLine("NLN 1000");
                    sw.WriteLine("NLN 750");
                    sw.WriteLine("NLN 500");
                    sw.WriteLine("NLN 250");

                    sw.Close();
                }
            }

            if (File.Exists(filePath))
            {
                using (TextReader tr = new StreamReader(filePath))
                {
                    for (int x = 0; x < maxScores; x++)
                    {
                        char[] initial = new char[3];
                        tr.Read(initial, 0, 3);
                        initials[x] = "" + initial[0] + initial[1] + initial[2];

                        long score = long.Parse(tr.ReadLine());
                        scores[x] = score;
                    }
                    tr.Close();
                }
            }
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
            : this(defaultTitle, selected)
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

            if (menuCancel.Evaluate(input, null, out index))
            {
                ExitScreen();
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
    }
}
