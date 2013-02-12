using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components;
using SpaceHordes.Entities.Components;
using Microsoft.Xna.Framework;
using GameLibrary.Entities.Components.Physics;

namespace SpaceHordes.Entities.Templates
{
    public class CrystalTemplate : IEntityTemplate
    {
        public CrystalTemplate(Sprite defaultSprite, IVelocity defaultVelocity, Crystal defaultCrystal)
        {
            _DefaultSprite = defaultSprite;
            _DefaultVelocity = defaultVelocity;
            _DefaultCrystal = defaultCrystal;
        }

        Sprite _DefaultSprite;
        IVelocity _DefaultVelocity;
        Crystal _DefaultCrystal;

        /// <summary>
        /// Builds a crystal entity. 
        /// </summary>
        /// <param name="e">The entity to build.</param>
        /// <param name="args">[0] = ITransform; [1] = IVelocity; [2] = Sprite; [3] = crystal </param>
        /// <returns></returns>
        public Entity BuildEntity(Entity e, params object[] args)
        {
            e.Group = "Crystals";

            IVelocity velocity = _DefaultVelocity;
            Sprite sprite = _DefaultSprite;
            ITransform transform = new Transform(Vector2.Zero, 0.0f);
            Crystal crystal = _DefaultCrystal;

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
                    crystal = (Crystal)args[3];
            }


            //Make the velocity proportional to the default velocity and the target rotation
            e.AddComponent<Particle>(new Particle(e, transform.Position, transform.Rotation,
                 velocity.LinearVelocity * new Vector2((float)Math.Cos(transform.Rotation),
                     (float)Math.Sin(transform.Rotation)),
                    velocity.AngularVelocity));
            e.AddComponent<Sprite>(sprite);
            e.AddComponent<Crystal>(crystal);

            return e;
        }

    }
}
