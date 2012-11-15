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
    public class Entity : Dictionary<string, Entity>
    {
        public Entity(Vector2 position, float rotation, Sprite sprite)
        {
            this.Position = position;
            this.Rotation = rotation;
            this.Sprite = sprite;
        }

        #region Functioning Loop

        public virtual void Update(GameTime gameTime)
        {
            Velocity += (Acceleration);
            Position += (Velocity);
            foreach (string k in Keys)
            {
                this[k].Update(gameTime);
            }
            Sprite.Update(gameTime);
        }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Sprite.Draw(gameTime, spriteBatch, Position, Rotation);
            foreach (string k in Keys)
            {
                this[k].Draw(gameTime, spriteBatch);
            }
        }

        #endregion

        #region Fields

        protected Entity _Parent;
        public EntityManager _EntityManager;

        #endregion

        #region Properties


        /// <summary>
        /// The Entity's parent
        /// </summary>
        public Sprite Sprite
        {
            set;
            get;
        }

        /// <summary>
        /// Position of the entity
        /// </summary>
        public Vector2 Position
        {
            set
            {
                InvokeAction(x => x._Position = value);
            }
            get
            {
                return _Position;
            }
        }
        private Vector2 _Position;

        /// <summary>
        /// The velocity of the entity.
        /// </summary>
        public Vector2 Velocity
        {
            set
            {
                InvokeAction(x => x._Velocity = value);
            }
            get
            {
                return _Velocity;
            }
        }
        private Vector2 _Velocity;

        /// <summary>
        /// The acceleration of an entity.
        /// </summary>
        public Vector2 Acceleration
        {
            set
            {
                InvokeAction(x => x._Acceleration = value);
                
            }
            get
            {
                return _Acceleration;
            }
        }
        private Vector2 _Acceleration;

        /// <summary>
        /// Rotation of the entity
        /// </summary>
        public float Rotation
        {
            set
            {
                InvokeAction(x => x._Rotation = value);
            }
            get
            {
                return _Rotation;
            }
        }
        private float _Rotation;

        #endregion

        #region Methods

        /// <summary>
        /// Adds a sub-entity
        /// </summary>
        /// <param name="name">The name of the sub-entity</param>
        /// <param name="entity">The sub-entity</param>
        public new virtual void Add(string name, Entity entity)
        {
            entity._Parent = this;
            base.Add(name, entity);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// A delegate for lamdas which handle entities
        /// </summary>
        /// <param name="x"></param>
        public delegate void EntityMethod(Entity x);


        /// <summary>
        /// Invokes a deleage action on all recursive children and parent entities (useful help method).
        /// </summary>
        /// <param name="evoke"></param>
        public virtual void InvokeAction(EntityMethod action)
        {
            action(this);
            foreach (string key in this.Keys)
            {
                this[key].InvokeAction(action);
            }
        }



        #endregion

    }

}
