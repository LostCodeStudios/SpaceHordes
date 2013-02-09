using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using GameLibrary.Helpers;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Dependencies.Physics.Dynamics;
using GameLibrary.Dependencies.Physics.Dynamics.Contacts;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;

namespace SpaceHordes.Entities.Systems
{
    public class PlayerControlSystem : GroupSystem
    {
        ComponentMapper<Body> bodyMapper;
        float _Velocity;
        bool WasMoving = false;
        int AnimationHeight = 30;
        float speed = 1 / 8;

        public PlayerControlSystem(float velocity)
            : base("Players")
        {
            this._Velocity = velocity;
        }

        public override void Initialize()
        {
            bodyMapper = new ComponentMapper<Body>(world);
        }

        public override void Process(Entity e)
        {
            Body b = bodyMapper.Get(e);

            #region UserMovement
            KeyboardState keyState = Keyboard.GetState();
            if (WasMoving) //Stops movement
            {
                b.LinearDamping = (float)Math.Pow(_Velocity, _Velocity * 4);
                WasMoving = false;
            }
            else
                b.LinearDamping = 0;

            Vector2 target = Vector2.Zero;
            if (keyState.IsKeyDown(Keys.D))
            { //Right
                target += Vector2.UnitX;
            }
            else if (keyState.IsKeyDown(Keys.A))
            { //Left
                target += -Vector2.UnitX;
            }

            if (keyState.IsKeyDown(Keys.S))
            { //Down
                target += Vector2.UnitY;
            }
            else if (keyState.IsKeyDown(Keys.W))
            { //Up?
                target += -Vector2.UnitY;
            }

            if (target != Vector2.Zero) //If being moved by player
            {
                target.Normalize();
                WasMoving = true;
                b.LinearDamping = _Velocity * 2;
            }

            //Rotation
            if (b.LinearVelocity != Vector2.Zero)
                //b.Rotation = MathHelper.SmoothStep(b.Rotation, (float)Math.Atan2(b.LinearVelocity.Y, b.LinearVelocity.X) + (float)Math.PI/2f, 0.1f);
                b.Rotation = (float)Math.Atan2(b.LinearVelocity.Y, b.LinearVelocity.X) + 0;// (float)Math.PI / 2f;


            //update position
            b.ApplyLinearImpulse((target) * new Vector2(_Velocity));
            #endregion

        }
    }
}
