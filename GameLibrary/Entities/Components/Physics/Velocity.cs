using Microsoft.Xna.Framework;

namespace GameLibrary.Entities.Components
{
    //Used for creating velocities and stuff
    public struct Velocity : IVelocity
    {
        public Velocity(Microsoft.Xna.Framework.Vector2 linearVelocity, float angularVelocity)
        {
            _LinearVelocity = linearVelocity;
            _AngularVelocity = angularVelocity;
        }

        public Microsoft.Xna.Framework.Vector2 LinearVelocity
        {
            set
            {
                _LinearVelocity = value;
            }
            get
            {
                return _LinearVelocity;
            }
        }

        private Vector2 _LinearVelocity;

        public float AngularVelocity
        {
            set
            {
                _AngularVelocity = value;
            }
            get
            {
                return _AngularVelocity;
            }
        }

        private float _AngularVelocity;
    }
}