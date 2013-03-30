using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceHordes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.GameStates.Screens
{
    public class MessageDialog
    {
        ImageFont font;
        int index = 0;
        public bool Enabled = false;
        string message;
        string toDraw;
        TimeSpan betweenLetters;
        TimeSpan endPhrase;
        TimeSpan elapsed;
        Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        char[] phraseEnders;

        public MessageDialog(ImageFont font, Vector2 position, string message, TimeSpan letters, TimeSpan phrase, char[] phrases)
        {
            this.font = font;
            this.position = position;
            this.message = message;

            betweenLetters = letters;
            endPhrase = phrase;
            elapsed = TimeSpan.Zero;
            toDraw = "";
            this.phraseEnders = phrases;
        }

        public MessageDialog(ImageFont font, Vector2 position, string message, TimeSpan letters, TimeSpan phrase)
            : this(font, position, message, letters, phrase, new char[] { '.', '!', '?', ','})
        {
        }

        public void Update(GameTime gameTime)
        {
            if (Enabled && !Complete())
            {
                elapsed += gameTime.ElapsedGameTime;

                char next = message.ToCharArray()[index];

                TimeSpan toPass;
                if (phraseEnders.Contains(next))
                    toPass = endPhrase;
                else
                    toPass = betweenLetters;

                if (elapsed >= toPass)
                {
                    elapsed = TimeSpan.Zero;
                    index++;
                    toDraw += next;
                }
            }
        }

        public bool Complete()
        {
            return (toDraw == message);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            font.DrawString(spriteBatch, position, toDraw);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            font.DrawString(spriteBatch, position + offset, toDraw);
        }
    }
}
