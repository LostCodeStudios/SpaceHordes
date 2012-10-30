using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game_Library.GameStates;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Library.Model.Managers
{
    public class EntityManager : Manager<string,Entity>
    {
        #region Functioning Loop
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            foreach (Entity e in this.Values)
                e.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, SpriteBatch sprB)
        {
            foreach (Entity e in this.Values)
                e.Draw(gameTime, sprB);
        }
        #endregion

        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Methods
        public override void Add(string key, Entity toAdd)
        {
            toAdd._EntityManager = this;
            base.Add(key, toAdd);
        }
        #endregion

        #region Helpers

        #endregion


    }
}
