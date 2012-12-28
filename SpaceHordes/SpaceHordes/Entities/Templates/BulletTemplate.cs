using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Entities;
using GameLibrary;
using GameLibrary.Entities.Components;
using Microsoft.Xna.Framework;
using GameLibrary.Dependencies.Physics.Dynamics;
using SpaceHordes.Entities.Components;
using GameLibrary.Helpers;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components.Physics;

namespace SpaceHordes.Entities.Templates
{
    public class BulletTemplate : IEntityTemplate
    {
        /// <summary>
        /// Builds a bullet type using a default velocity and sprite
        /// </summary>
        /// <param name="defaultSprite">The default sprite that the sprite will use.</param>
        /// <param name="velocity">The default velocity that the sprite will use. (To make the bullet positioned relative to the gun, use 1 for all variables).</param>
        public BulletTemplate(Sprite defaultSprite, IVelocity defaultVelocity)
        {
            this._DefaultVelocity = defaultVelocity;
            this._DefaultSprite = defaultSprite;
        }
        Sprite   _DefaultSprite;
        IVelocity _DefaultVelocity;


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

            //Make the velocity proportional to the default velocity and the target rotation
            e.AddComponent<Particle>(new Particle(e, ((ITransform)args[0]).Position, ((ITransform)args[0]).Rotation,
                 _DefaultVelocity.LinearVelocity * new Vector2((float)Math.Cos(((ITransform)args[0]).Rotation),
                     (float)Math.Sin(((ITransform)args[0]).Rotation)),
                    _DefaultVelocity.AngularVelocity));
            e.AddComponent<Sprite>(_DefaultSprite);

            e.AddComponent<Bullet>(new Bullet(10, "Structures"));

            return e;
        }

    }
}
