using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Entities;
using GameLibrary;
using GameLibrary.Entities.Components;
using Microsoft.Xna.Framework;
using GameLibrary.Physics.Dynamics;
using SpaceHordes.Entities.Components;
using GameLibrary.Helpers;

namespace SpaceHordes.Entities.Templates
{
    public class BulletTemplate : IEntityTemplate
    {
        /// <summary>
        /// Builds a bullet type using a default velocity and sprite
        /// </summary>
        /// <param name="defaultSprite">The default sprite that the sprite will use.</param>
        /// <param name="velocity">The default velocity that the sprite will use. (To make the bullet positioned relative to the gun, use 1 for all variables).</param>
        public BulletTemplate(Sprite defaultSprite, Velocity defaultVelocity)
        {
            this._DefaultVelocity = defaultVelocity;
            this._DefaultSprite = defaultSprite;
        }
        Sprite   _DefaultSprite;
        Velocity _DefaultVelocity;


        /// <summary>
        /// Builds a bullet (PASS IN A VECTOR 2 FOR ARGS[0])
        /// </summary>
        /// <param name="e"></param>
        /// <param name="args">[0] = VECTOR2 POSITION. [1] = FLOAT ROTATION!</param>
        /// <returns></returns>
        public Entity BuildEntity(Entity e, params object[] args)
        {
            System.Diagnostics.Debug.Assert(!(args == null || args[0] == null), "args[0] = Transform; is not defined -_-");
            e.Group = "Bullets";
            e.AddComponent<Transform>("Body", new Transform(((Transform)args[0]).Position, ((Transform)args[0]).Rotation));

            //Make the velocity proportional to the default velocity and the target rotation
            e.AddComponent<Velocity>("Body", 
                new Velocity(
                    _DefaultVelocity.LinearVelocity * new Vector2((float)Math.Cos(((Transform)args[0]).Rotation), (float)Math.Sin(((Transform)args[0]).Rotation)),
                    _DefaultVelocity.AngularVelocity));


            e.AddComponent<Sprite>("Body", _DefaultSprite);
            return e;
        }

    }
}
