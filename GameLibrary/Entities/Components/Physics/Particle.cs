using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using Microsoft.Xna.Framework;

namespace GameLibrary.Entities.Components.Physics
{
    /// <summary>
    /// A non physical body which does not interact with the farseer physics library
    /// </summary>
    public class Particle : Component, ITransform, IVelocity, IDamping
    {
        public Particle(Entity e, Vector2 position, float rotation, Vector2 linearVelocity, float angularVelocity)
        {
            e.AddComponent<ITransform>(this);
            e.AddComponent<IVelocity>(this);
            e.AddComponent<IDamping>(this);
            this.Position = position;
            this.Rotation = rotation;
            this.LinearVelocity = linearVelocity;
            this.AngularVelocity = angularVelocity;
            this.LinearDamping = 0.0f;
            this.AngularDamping = 0.0f;
        }

        public Particle(Entity e)
            : this(e, Vector2.Zero, 0f, Vector2.Zero, 0.0f)
        {
        }

        #region ITransform
        /// <summary>
        /// The world position of a particle.
        /// </summary>
        public Microsoft.Xna.Framework.Vector2 Position
        {
            set;
            get;
        }

        /// <summary>
        /// The rotation of a particle.
        /// </summary>
        public float Rotation
        {
            get;
            set;
        }
        #endregion

        #region IVelocity
        /// <summary>
        /// The linear velocity of a particle.
        /// </summary>
        public Microsoft.Xna.Framework.Vector2 LinearVelocity
        {
            set;
            get;
        }

        /// <summary>
        /// The angular velocity of a particle.
        /// </summary>
        public float AngularVelocity
        {
            set;
            get;
        }
        #endregion

        #region IDamping
        /// <summary>
        /// The meter/second quantity by which the linear velocity will slow.
        /// </summary>
        public float LinearDamping { set; get; }

        /// <summary>
        /// The radian/second quantity by which the angular velocity (omega) will slow.
        /// </summary>
        public float AngularDamping { set; get; }

        #endregion
    }
}
