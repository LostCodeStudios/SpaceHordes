using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Library.Model.Managers
{
    public class SpriteManager : Manager<string, Sprite> //TODO: CHANGE TO LIGAMENTMANAGER?
    {

        #region Functioning Loop

        /// <summary>
        /// Updates the sprite manager
        /// </summary>
        /// <param name="gameTime">The current gametime</param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            foreach (Sprite s in this.Values)
                s.Update(gameTime);
        }

        /// <summary>
        /// Draws all the sprites in the sprite manager.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, SpriteBatch sprB)
        {
            foreach (Sprite s in this.Values)
                s.Draw(gameTime, sprB);
        }

        #endregion

        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Methods
        #endregion

        #region Helpers

        #endregion

    }
}
