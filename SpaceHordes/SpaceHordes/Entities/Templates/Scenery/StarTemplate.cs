using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using System;

namespace SpaceHordes.Entities.Templates
{
    public class StarTemplate : IEntityTemplate
    {
        private SpriteSheet spriteSheet;
        private static Random rbitch = new Random();
        private int stars = 0;
        private static Vector2[] nebulaLocs = new Vector2[12];

        public StarTemplate(SpriteSheet spriteSheet)
        {
            this.spriteSheet = spriteSheet;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        /// <param name="args">args[0] = bool bigStar</param>
        /// <returns></returns>
        public Entity BuildEntity(Entity e, params object[] args)
        {
            e.Group = "Stars";
            e.Tag = "Star" + stars.ToString();
            ++stars;

            #region Sprite

            if (args.Length == 0)
            {
                Vector2 loc = new Vector2(rbitch.Next(-ScreenHelper.Viewport.Width / 2, ScreenHelper.Viewport.Width / 2), rbitch.Next(-ScreenHelper.Viewport.Width / 2, ScreenHelper.Viewport.Width / 2));

                Sprite s = new Sprite(spriteSheet, "redstar", 0f);

                s.FrameIndex = rbitch.Next(0, 3);
                s.Scale = (float)rbitch.NextDouble();
                e.AddComponent<Sprite>(s);
                Animation a = e.AddComponent<Animation>(new Animation(AnimationType.Bounce, 5));
                Particle p = e.AddComponent<Particle>(new Particle(e, ConvertUnits.ToSimUnits(loc), (float)Math.Atan2(loc.Y, loc.X), Vector2.Zero, (float)rbitch.Next(-3, 3) * 0.01f));
            }
            else
            {
                Vector2 loc = Vector2.Zero;

                Sprite s = new Sprite(spriteSheet, "nebula", 0f);

                

                e.AddComponent<Sprite>(s);

                Particle p = e.AddComponent<Particle>(new Particle(e, ConvertUnits.ToSimUnits(loc), 0f, Vector2.Zero, 0f));
            }

            #endregion Sprite

            return e;
        }
    }
}