using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Game_Library.Model;
using Game_Library.Model.Managers;

namespace Game_Library.Model
{
    public class Entity
    {
        /// <summary>
        /// Entity constructor
        /// </summary>
        /// <param name="location">Initial location</param>
        /// <param name="rotation">Initial rotation</param>
        /// <param name="velocity">Initial velocity</param>
        public Entity(Vector2 location, float rotation, double velocity)
        {
            Sprites = new SpriteManager();

            this.Velocity = velocity;
            this.Location = location;
            this.Rotation = rotation;
        }

        #region Functioning Loop
        /// <summary>
        /// Updates the entity.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            Sprites.Update(gameTime);
        }

        /// <summary>
        /// Draws the entity
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Draw(GameTime gameTime, SpriteBatch sprB)
        {
            Sprites.Draw(gameTime, sprB);
        }
        #endregion

        #region Fields

        /// <summary>
        /// The sprites herein contained by the entity
        /// </summary>
        public SpriteManager Sprites;

        #endregion

        #region Properties

        /// <summary>
        /// The location of the entity
        /// </summary>
        public Vector2 Location
        {
            set
            {
                _Location = value;
                foreach (Sprite s in Sprites.Values)
                    s.Location = value;
            }
            get
            {
                return _Location;
            }
        }
        private Vector2 _Location;

        /// <summary>
        /// Velocity of the entity
        /// </summary>
        public double Velocity
        {
            set
            {
                _Velocity = value;
                foreach (Sprite s in Sprites.Values)
                    s.Velocity = value;
            }
            get
            {
                return _Velocity;
            }
        }
        public double _Velocity;

        /// <summary>
        /// Rotation of the entity
        /// </summary>
        public float Rotation
        {
            set
            {
                _Rotation = value;
                foreach (Sprite s in Sprites.Values)
                    s.Rotation = value;
            }
            get
            {
                return _Rotation;
            }
        }
        public float _Rotation;

        #endregion

        #region Helpers

        /// <summary>
        /// The entity manager whereinside this entity was created.
        /// </summary>
        public EntityManager _EntityManager;

        #endregion
    }
}
