using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameLibrary.Entities.Components
{
    public struct Transform : ITransform
    {
        public Transform(Vector2 position, float rotation)
        {
            _Position = position;
            _Rotation = rotation;
        }
        public Microsoft.Xna.Framework.Vector2 Position
        {
            get
            {
                return _Position;
            }
            set
            {
                _Position = value;
            }
        }
        private Vector2 _Position;

        public float Rotation
        {
            get
            {
                return _Rotation;
            }
            set
            {
                _Rotation = value;
            }
        }
        private float _Rotation;
    }
}
