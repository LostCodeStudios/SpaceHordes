using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game_Library.GameStates;
using Game_Library.Input;
using Game_Library.Model.Entities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Library.Model.Managers
{
    public class EntityManager : Manager<string,Entity>
    {
        #region Functioning Loop
        public override void Update(GameTime gameTime)
        {
            foreach (Entity e in this.Values)
                e.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch sprB)
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

        #region Input

        public void HandleInput(InputState input)
        {
            foreach (Entity entity in this.Values)
            {
                if (entity is IInput)
                {
                    IInput thisEntity = entity as IInput;
                    thisEntity.HandleInput(input);
                }
            }
        }

        #endregion

        #region Helpers

        #endregion


    }
}
