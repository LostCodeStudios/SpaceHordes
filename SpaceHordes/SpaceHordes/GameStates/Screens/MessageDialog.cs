using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceHordes;
using System;
using System.Linq;

namespace GameLibrary.GameStates.Screens
{
    public class MessageDialog
    {
        private ImageFont font;
        private int index = 0;
        public bool Enabled = false;
        private string message;
        private string toDraw;
        private TimeSpan betweenLetters;
        private TimeSpan endPhrase;
        private TimeSpan elapsed;
        private Vector2 position;
        private string soundKey = "DialogSound";

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public bool Visible = true;

        private char[] phraseEnders;

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
            : this(font, position, message, letters, phrase, new char[] { '.', '!', '?', ',' })
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
                    SoundManager.Play(soundKey);
                }
            }
        }

        public bool Complete()
        {
            return toDraw == message && Enabled;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
                font.DrawString(spriteBatch, position, toDraw);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            if (Visible)
                font.DrawString(spriteBatch, position + offset, toDraw);
        }
    }
}