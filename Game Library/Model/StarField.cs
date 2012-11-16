using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Library.Model
{
    class StarField : Manager<int, Star>
    {
        #region Fields

        Spritesheet spritesheet;        

        #endregion

        #region Constructor

        public StarField(Spritesheet sheet)
        {
            spritesheet = sheet;

            GenerateStars(3, 50);
        }

        #endregion

        #region Methods

        private void GenerateStars(int big, int small)
        {
            for (int x = 0; x < big; x++)
            {
                this.Add(x, new Star(true, spritesheet));
            }

            for (int x = big; x < big + small; x++)
            {
                this.Add(x, new Star(false, spritesheet));
            }
        }

        #endregion

        #region Update & Draw

        /// <summary>
        /// Updates every star managed by this star field.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            foreach (Star star in this.Values)
            {
                star.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Star star in this.Values)
            {
                star.Draw(gameTime, spriteBatch);
            }
        }

        #endregion
    }
}
