using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using System;
using GameLibrary.Entities.Components.Physics;

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

            Vector2 loc = new Vector2(rbitch.Next(-ScreenHelper.Viewport.Width / 2, ScreenHelper.Viewport.Width / 2), rbitch.Next(-ScreenHelper.Viewport.Width / 2, ScreenHelper.Viewport.Width / 2));

            Sprite s = new Sprite(spriteSheet, "redstar", 0f);

            //Sprite s = e.AddComponent<Sprite>(new Sprite(spriteSheet, "redstar", loc, 1f, Color.White, 0));
            s.FrameIndex = rbitch.Next(0, 3);
            e.AddComponent<Sprite>(s);
            Animation a = e.AddComponent<Animation>(new Animation(AnimationType.Bounce, 5));
            Particle p = e.AddComponent<Particle>(new Particle(e, ConvertUnits.ToSimUnits(loc), (float)Math.Atan2(loc.Y, loc.X), Vector2.Zero, (float)rbitch.Next(-3, 3) * 0.01f));

            #endregion Sprite

            return e;
        }
    }
}