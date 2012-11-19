using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Game_Library.Model
{
    class Star : Sprite
    {
        #region Fields

        static Random r = new Random();
        Vector2 location;

        #endregion

        #region Constructor

        public Star(bool big, Spritesheet spritesheet)
            : base("redstar", Vector2.Zero, spritesheet)
        {
            if (big)
            {
                //randomize location
                location = new Vector2(r.Next(Screen.Viewport.Width - spritesheet.Animations["rednebula"][0].Width), r.Next(Screen.Viewport.Height - spritesheet.Animations["rednebula"][0].Height));
                SpriteName = "rednebula";
            }

            else
            {
                location = new Vector2(r.Next(Screen.Viewport.Width), r.Next(Screen.Viewport.Height));

                AnimationPattern = AnimationType.Loop;
                AnimationTickRate = 5;
            }
        }

        #endregion

        #region Animation

        protected override void AdvanceFrame()
        {
            //AnimationIndex += frameInc;

            //if (AnimationIndex == Frames.Length || AnimationIndex == 0)
            //    frameInc *= -1;
        }

        #endregion

        #region Draw

        public void Draw(GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch, location, 0.0f);
        }

        #endregion
    }
}
