using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using Microsoft.Xna.Framework;
using GameLibrary.Dependencies.Physics.Dynamics;


namespace GameLibrary.Entities.Components.Physics
{
    /// <summary>
    /// A physical component is a body. Body components are always coupled with ITransform & Velocity
    /// </summary>
    public class Body : GameLibrary.Dependencies.Physics.Dynamics.PhysicsBody, Component, ITransform, IVelocity, IDamping
    {
        public Body(EntityWorld world, Entity e)
            : base(world, e)
        {
            //Add transform and velocity
            e.AddComponent<ITransform>(this);
            e.AddComponent<IVelocity>(this);
            e.AddComponent<IDamping>(this);
        }
        ~Body()
        {
            this.World.RemoveBody(this);   
        }

        #region ITransform
        /// <summary>
        /// The position of an entity.
        /// </summary>
        Vector2 ITransform.Position
        {
            get
            {
                return this.Position;
            }
            set
            {
                this.Position = value;
            }
        }

        /// <summary>
        /// The rotation of an entity.
        /// </summary>
        float ITransform.Rotation
        {
            get
            {
                return this.Rotation;
            }
            set
            {
                this.Rotation = Rotation;
            }
        }
        #endregion

        #region IVelocity
        /// <summary>
        /// The linear velocity of an entity.
        /// </summary>
        Vector2 IVelocity.LinearVelocity
        {
            get
            {
                return this.LinearVelocity;
            }
            set
            {
                this.LinearVelocity = value;
            }
        }

        /// <summary>
        /// The angular velocity of an entity.
        /// </summary>
        float IVelocity.AngularVelocity
        {
            get
            {
                return this.AngularVelocity;
            }
            set
            {
                this.AngularVelocity = value;
            }
        }
        #endregion

        #region IDamping

        /// <summary>
        /// The radian/second quantity by which the angular velocity (omega) will slow.
        /// </summary>
        float IDamping.AngularDamping
        {
            get
            {
                return this.AngularDamping;
            }
            set
            {
                this.AngularDamping = value;
            }
        }

        /// <summary>
        /// The meter/second quantity by which the linear velocity will slow.
        /// </summary>
        float IDamping.LinearDamping
        {
            get
            {
                return this.LinearDamping;
            }
            set
            {
                this.LinearDamping = value;
            }
        }

        #endregion

        #region Helpers
        public override string ToString()
        {
            return "[(Pos=" + this.Position
                + "),\n                (LVel=" + this.LinearVelocity
                + "),\n                (AVel=" + this.AngularVelocity
                + "),\n                (Ent=" + this.UserData + ")]";
        }

        public void RotateTo(Vector2 direction)
        {
            Rotation = (float)Math.Atan2(direction.Y, direction.X);
        }
        #endregion
    }
}
