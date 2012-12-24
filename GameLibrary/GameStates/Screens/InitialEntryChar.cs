using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.GameStates.Screens
{
    class InitialEntryChar : MenuEntry
    {
        /// <summary>
        /// Draws the menu entry using the InitialEntryFont
        /// </summary>
        public override void Draw(GameScreen screen, bool isSelected, GameTime gameTime)
        {
            //Draw the selected entry in yellow, and others in white.
            Color color = isSelected ? Color.Yellow : Color.White;

            ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            SpriteFont font = screenManager.InitialEntryFont;

            clickRectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)(font.MeasureString(Text).X * Scale), (int)(font.MeasureString(Text).Y * Scale));

            color *= screen.TransitionAlpha;

            //Draw text, centered on the middle of each line.

            Vector2 origin = new Vector2(0, font.LineSpacing / 2);

            spriteBatch.DrawString(font, text, position, color, 0,
                origin, Scale, SpriteEffects.None, 0);

        }

        public InitialEntryChar(string text)
            : base(text)
        {
        }
    }
}
