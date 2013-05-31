using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.Helpers;

namespace SpaceHordes.Entities.Components
{
    public class FadingText : Component
    {
        string text;
        float elapsedTime;
        float time;
        Vector2 position;
        Vector2 origin;

        public FadingText(string text, float time, Vector2 position)
        {
            this.text = text;
            this.time = time;
            this.position = ScreenHelper.Center + ConvertUnits.ToDisplayUnits(position) - new Vector2(0, 20);
        }

        public float Fraction()
        {
            return time - elapsedTime / time;
        }

        public void Update(int ticks)
        {
            elapsedTime += (float)ticks / 1000;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            if (origin == Vector2.Zero)
            {
                origin = spriteFont.MeasureString(text) / 2;
            }
            spriteBatch.DrawString(spriteFont, text, position, Color.White * Fraction(), 0f, origin, 1f, SpriteEffects.None, 0.98f);
        }
    }
}
