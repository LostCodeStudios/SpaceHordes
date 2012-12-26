using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameLibrary.Entities.Components
{
    public class Transform : Component
    {
        public Transform() : this(Vector2.Zero, 0.0f)
        {
        }

        public Transform(Vector2 position, float rotation)
        {
            _Position = position;
            _Rotation = rotation;
            _Set = false;
        }

        public Vector2 Position
        {
            set
            {
                _Set = true;
                _Position = value;
            }
            get
            {
                return _Position;
            }
        }
        internal Vector2 _Position;

        public float Rotation
        {
            set
            {
                _Set = true;
                _Rotation = value;
            }
            get
            {
                return _Rotation;
            }
        }
        internal float _Rotation;

        /// <summary>
        /// True if any properties were changed externally.
        /// </summary>
        internal bool _Set; 
    }
}
