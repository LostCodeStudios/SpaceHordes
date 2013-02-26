using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Components;
using System;

namespace SpaceHordes.Entities.Templates
{
    internal class BulletTemplate : IEntityTemplate
    {
        /// <summary>
        /// Builds a bullet type using a default velocity and sprite
        /// </summary>
        /// <param name="defaultSprite">The default sprite that the sprite will use.</param>
        /// <param name="velocity">The default velocity that the sprite will use. (To make the bullet positioned relative to the gun, use 1 for all variables).</param>
        public BulletTemplate(Sprite defaultSprite, IVelocity defaultVelocity, Bullet defaultBullet)
        {
            this._DefaultVelocity = defaultVelocity;
            this._DefaultSprite = defaultSprite;
            this._DefaultBullet = defaultBullet;
        }

        /// <summary>
        /// Creates a bullet template with a default sprite
        /// </summary>
        public BulletTemplate(Sprite defaultSprite)
            : this(defaultSprite, new Velocity(Vector2.One, 0.0f), new Bullet(1))
        {
        }

        private Sprite _DefaultSprite;
        private IVelocity _DefaultVelocity;
        private Bullet _DefaultBullet;

        /// <summary>
        /// Builds a bullet entity.
        /// </summary>
        /// <param name="e">The entity to build.</param>
        /// <param name="args">[0] = ITransform; [1] = IVelocity; [2] = Sprite; [3] = Bullet </param>
        /// <returns></returns>
        public Entity BuildEntity(Entity e, params object[] args)
        {
            e.Group = "Bullets";

            //Set up defaults
            IVelocity velocity = _DefaultVelocity;
            Sprite sprite = _DefaultSprite;
            ITransform transform = new Transform(Vector2.Zero, 0.0f);
            Bullet bullet = _DefaultBullet;

            //Check arguments.
            if (args != null)
            {
                if (args.Length > 0)
                    transform = (args[0] as ITransform);
                if (args.Length > 1)
                    velocity = (args[1] as IVelocity);
                if (args.Length > 2)
                    sprite = (Sprite)args[2];
                if (args.Length > 3)
                    bullet = (Bullet)args[3];
            }

            //Make the velocity proportional to the default velocity and the target rotation
            e.AddComponent<Particle>(new Particle(e, transform.Position, transform.Rotation,
                 velocity.LinearVelocity * new Vector2((float)Math.Cos(transform.Rotation),
                     (float)Math.Sin(transform.Rotation)),
                    velocity.AngularVelocity));
            e.AddComponent<Sprite>(sprite);
            e.AddComponent<Bullet>(bullet);

            return e;
        }
    }
}