using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameLibrary.Entities.Components
{
    /// <summary>
    /// A component of the entities velocity.
    /// </summary>
    public class Velocity : Component
    {
        public Velocity() : this(Vector2.One, 1.0f)
        {
        }

        public Velocity(Vector2 linearVelocity, float angularVelocity)
        {
            this._LinearVelocity = linearVelocity;
            this._AngularVelocity = angularVelocity;
            this._Set = false;
        }

        public Vector2 LinearVelocity
        {
            set
            {
                _Set = true;
                _LinearVelocity = value;
            }
            get
            {
                return _LinearVelocity;
            }
        }
        internal Vector2 _LinearVelocity;

        public float AngularVelocity
        {
            set
            {
                _Set = true;
                _AngularVelocity = value;
            }
            get
            {
                return _AngularVelocity;
            }
        }
        internal float _AngularVelocity;

        /// <summary>
        /// True if any properties were changed externally.
        /// </summary>
        internal bool _Set; 
    }
}
